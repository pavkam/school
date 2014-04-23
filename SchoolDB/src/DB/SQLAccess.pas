unit SQLAccess;
interface
Uses SchObject, ZClasses, ZDbcIntfs, ZCompatibility, ZDbcMySql,
     Windows, SysUtils, TransferFields, Classes, GenUtils;

Type
 TConnectSpec = record
                  sHost     : String;
                  wPort     : Word;
                  sUser     : String;
                  sPassword : String;
                  sDBName   : String;
                end;
                
 TSchSQLAccess  =  class( TSchObject )
                    private
                     fConnection    : IZConnection;
                     fBusy          : Boolean;
                     fError         : Boolean;
                     fAccBy         : Integer;
                     
                    public
                     constructor Create();

                     { Connection related methods }
                     function Connect( Conn : TConnectSpec ) : Boolean;
                     procedure Disconnect();

                     procedure Accquire( iBy : Integer );
                     procedure Release();

                     function AccquiredBy() : Integer;

                     procedure ExecuteUpdate( cList : TSchGenericEntry; sTable, sWhere : String );
                     function ExecuteSQLQuery  ( sQuery : String; bIsUpdate : Boolean ) : TSchGenericEntryList;
                     procedure ExecuteSQLModify ( TableName, ValueName : String; EqualsTo : Integer; Entry : TSchGenericEntry );

                     function WasError() : Boolean;
                   end;


Var
 SQL   : TSchSQLAccess;
 Locks : TStrings;
 

implementation

{ TSchSQLAccess }

procedure TSchSQLAccess.Accquire( iBy : Integer );
begin
 while fBusy do Sleep( 10 );
 fBusy := True;
 fAccBy := iBy;
end;

function TSchSQLAccess.AccquiredBy: Integer;
begin
 Result := fAccBy;
end;

function TSchSQLAccess.Connect( Conn : TConnectSpec ) : Boolean;
var
  Url: string;
begin

  // Generating connection URL from the ConnectionSpec received from outside
  // and start the connection.
  
  Result := True;

  if Conn.wPort <> 0 then
    Url := Format( 'zdbc:%s://%s:%d/%s?UID=%s;PWD=%s',
           [ 'mysql', Conn.sHost, Conn.wPort, Conn.sDBName, Conn.sUser, Conn.sPassword ] ) else

    Url := Format('zdbc:%s://%s/%s?UID=%s;PWD=%s',
           [ 'mysql', Conn.sHost, Conn.sDBName, Conn.sUser, Conn.sPassword]);

  try
    fConnection := DriverManager.GetConnectionWithParams( Url, nil );
    fConnection.SetAutoCommit(True);
    fConnection.SetTransactionIsolation( tiReadCommitted );
    fConnection.Open;
  except
    on Exception do Result := False;
  end;

end;

constructor TSchSQLAccess.Create;
begin
 fConnection := nil;
 fBusy       := False;
 fError      := False;
 fAccBy      := 0;
end;

procedure TSchSQLAccess.Disconnect;
begin
 if not fConnection.IsClosed then
    fConnection.Close;
end;

procedure TSchSQLAccess.ExecuteSQLModify(TableName, ValueName : String;
         EqualsTo: Integer; Entry: TSchGenericEntry);
var
 Statement  : IZStatement;
 i, u       : Integer;
 StrUp      : WideString;
 Str        : String;
 Bytes      : TByteDynArray;
 ResultSet  : IZResultSet;
   
begin

  if not Assigned( fConnection ) then
     begin
       fError := True;
       Exit;
     end;

  if fConnection.IsClosed then
     begin
       fError := True;
       Exit;
     end;     

  Statement := fConnection.CreateStatement;
  Statement.SetResultSetConcurrency( rcUpdatable );


   if EqualsTo = 0 then
      ResultSet := Statement.ExecuteQuery( 'SELECT * FROM ' + TableName ) else
      ResultSet := Statement.ExecuteQuery( 'SELECT * FROM ' + TableName + ' WHERE ' + ValueName + ' = ' + IntToStr( EqualsTo ) );

  if ResultSet = nil then Exit; // Exit for NIL

  if (EqualsTo = 0) then
     ResultSet.InsertRow() else // Add a Line !
     ResultSet.Last;

   for i := 0 to Entry.Count - 1 do
    begin
      if Entry.Items[i].isString then
         ResultSet.UpdateStringByName( Entry.Items[i].FieldName, Entry.Items[i].asString );

      if Entry.Items[i].isInteger then
         ResultSet.UpdateIntByName( Entry.Items[i].FieldName, Entry.Items[i].asInteger );

      if Entry.Items[i].isDate then
       if Entry.Items[i].asDate = 0 then
         ResultSet.UpdateNullByName( Entry.Items[i].FieldName ) else
         ResultSet.UpdateDateByName( Entry.Items[i].FieldName, Entry.Items[i].asDate );

    end;

   ResultSet.UpdateRow();
   
   if Assigned( Statement ) then Statement.Close();
   if Assigned( ResultSet ) then ResultSet.Close();
end;


function TSchSQLAccess.ExecuteSQLQuery(sQuery: String;bIsUpdate: Boolean): TSchGenericEntryList;
Var
 Statement  : IZStatement;
 i          : Integer;
 ResultSet  : IZResultSet;

 Ent        : TSchGenericEntry;
 Fld        : TSchGenericField;

begin

  Result := nil;
  if (not Assigned( fConnection )) then
     begin
       fError := True;
       Exit;
     end;

  if fConnection.IsClosed then
     begin
       fError := True;
       Exit;
     end;

  Statement := fConnection.CreateStatement;
  Statement.SetResultSetConcurrency( rcReadOnly );

  if bIsUpdate then
   begin
     Statement.ExecuteUpdate( sQuery );
     Statement.Close();
     Exit;
   end;

  ResultSet := Statement.ExecuteQuery( sQuery );

  if ResultSet <> nil then
  begin
    Result := TSchGenericEntryList.Create();

    while ResultSet.Next do
     begin
      Ent := TSchGenericEntry.Create();

      for I := 1 to ResultSet.GetMetadata.GetColumnCount do
      begin
        Fld := TSchGenericField.Create();
        Fld.FieldName := ResultSet.GetMetadata.GetColumnName( I );

        case ResultSet.GetMetadata.GetColumnType(I) of
          stBoolean    : Fld.asInteger := Integer( ResultSet.GetBoolean(I) );

          stByte, stShort, stInteger, stLong
                       : Fld.asInteger := ResultSet.GetInt(I);

          stDate       : Fld.asDate := ResultSet.GetDate(I);
          stTimeStamp  : Fld.asDate := ResultSet.GetTimeStamp(I);

          stAsciiStream,
           stUnicodeStream :
            Fld.asString := ResultSet.GetBlob(I).GetString();


        else
          Fld.asString := ResultSet.GetString(I);
        end;

        if ( ResultSet.IsNull(I) ) and
           (( ResultSet.GetMetadata.GetColumnType(I) = stDate ) or
            ( ResultSet.GetMetadata.GetColumnType(I) = stTimeStamp ))
           then Fld.asDate := 0;

        Ent.Add( Fld );
      end;

      Result.Add( Ent );
     end;
  end;

 if Assigned( Statement ) then Statement.Close();
 if Assigned( ResultSet ) then ResultSet.Close();
end;

procedure TSchSQLAccess.ExecuteUpdate(cList: TSchGenericEntry; sTable, sWhere: String);
begin
 if cList.Count <> 1 then Exit;

 if cList.Items[0].isInteger then
    ExecuteSQLQuery( 'UPDATE '+sTable+' SET '+cList.Items[0].FieldName+' = '+IntToStr( cList.Items[0].asInteger )+
     ' WHERE '+ sWhere, True);
end;

procedure TSchSQLAccess.Release;
begin
 fBusy := False;
end;

function TSchSQLAccess.WasError: Boolean;
begin
 Result := fError;
 fError := False;
end;

initialization
 SQL   := TSchSQLAccess.Create();
 Locks := TStringList.Create;

finalization
 Locks.Destroy(); 

end.

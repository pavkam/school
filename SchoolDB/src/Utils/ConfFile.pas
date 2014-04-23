unit ConfFile;
interface
Uses Classes, SysUtils, SchObject;

Type
 ESchFileError = class(ESchException);

 TSchConfFile = class( TSchObject )
                 private
                  fValueList,
                   fNameList : TStrings;
                  fFile      : String;

                  function GetValue(Index: String): String;
                  procedure SetValue(Index: String; const Value: String);
                  function GetBValue(Index: String): Boolean;
                  function GetIValue(Index: String): Integer;
                  procedure SetBValue(Index: String; const Value: Boolean);
                  procedure SetIValue(Index: String; const Value: Integer);

                 public
                  constructor Create( AFile : String );
                  destructor Destroy(); override;

                  procedure Save();
                  procedure Load();

                  property sValue[ Index : String ] : String read GetValue write SetValue;
                  property iValue[ Index : String ] : Integer read GetIValue write SetIValue;
                  property bValue[ Index : String ] : Boolean read GetBValue write SetBValue;

                  property  Names : TStrings read fNameList;
                  property Values : TStrings read fValueList;
                end;

implementation

{ TSchConfFile }

function atoi( sVal : String ) : Integer;
var
 Err : Integer;
begin
 Val( sVal, Result, Err );

 if Err <> 0 then Result := 0;
end;

constructor TSchConfFile.Create(AFile: String);
begin
 fFile := AFile;
 fValueList := TStringList.Create();
 fNameList  := TStringList.Create();
end;

destructor TSchConfFile.Destroy;
begin
 fValueList.Destroy();
 fNameList.Destroy();
end;

function TSchConfFile.GetBValue(Index: String): Boolean;
begin
 Result := ( GetIValue( Index ) <> 0 );
end;

function TSchConfFile.GetIValue(Index: String): Integer;
begin
 Result := atoi( GetValue( Index ) );
end;

function TSchConfFile.GetValue( Index : String ): String;
var
 idx : Integer;
begin

 idx := fNameList.IndexOf( UpperCase(Index) );

 if idx <> -1 then
  Result := fValueList.Strings[ idx ] else
  Result := '';

end;

procedure TSchConfFile.Load;
var
 fTemp : TStrings;
 i, x  : Integer;

 sVal,
  sVar : String;
begin

 fTemp := TStringList.Create();

 try
  fTemp.LoadFromFile( fFile );
 except
  on Exception do begin fTemp.Destroy(); raise ESchFileError.Create( 'File Load Error!' ); end;
 end;

 if fTemp.Count > 0 then
  for i := 0 to fTemp.Count - 1 do
  begin
   x := Pos( '=', fTemp.Strings[i] );

   if x = 0 then Continue;

   sVar := UpperCase(Trim(Copy( fTemp.Strings[i], 1, x - 1 )));
   sVal := fTemp.Strings[i];
   Delete( sVal, 1, x );

   fNameList.Add( sVar );
   fValueList.Add( sVal );
  end;

  fTemp.Destroy();

end;

procedure TSchConfFile.Save;
var
 fTemp : TStrings;
 i     : Integer;
begin

 fTemp := TStringList.Create();

 if fNameList.Count > 0 then
  for i := 0 to fNameList.Count - 1 do
   fTemp.Add( fNameList.Strings[i] + '=' + fValueList.Strings[i] );

 try
  fTemp.SaveToFile( fFile );
 except
  on Exception do begin fTemp.Destroy(); raise ESchFileError.Create( 'File Save Error!' ); end;
 end;

 fTemp.Destroy();
 
end;

procedure TSchConfFile.SetBValue(Index: String; const Value: Boolean);
begin
 SetIValue( Index, Integer(Value) );
end;

procedure TSchConfFile.SetIValue(Index: String; const Value: Integer);
begin
 SetValue( Index, IntToStr(Value) );
end;

procedure TSchConfFile.SetValue(Index: String; const Value: String);
var
 idx : Integer;
begin

 idx := fNameList.IndexOf( UpperCase(Index) );

 if idx <> -1 then
  fValueList.Strings[ idx ] := Value else
  begin
   fNameList.Add( UpperCase(Index) );
   fValueList.Add( Value );
  end;

end;

end.

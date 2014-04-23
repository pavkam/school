unit Globals;
interface
Uses Windows, SysUtils, Forms, SQLAccess, GenUtils, WinUtils, TransferFields;

Const
 TABLE_CLASSROOMS   =   'tblClassRooms';

 FIELD_ID           =   'ID';
 FIELD_NAME         =   'sName';


var
 SQL : TSchSQLAccess;

 { Indexes }
 iiClassRooms : Integer;
 


procedure LoadLastIndexes();


procedure Perform_ExecuteUpdate( cList : TSchGenericEntry; sTable, sWhere : String );
function Perform_ExecuteSQLQuery  ( sQuery : String; bIsUpdate : Boolean ) : TSchGenericEntryList;
procedure Perform_ExecuteSQLModify ( TableName, ValueName : String; EqualsTo : Integer; Entry : TSchGenericEntry );

function Perform_Connect( cs : TConnectSpec ) : Boolean;
function DB_Busy() : boolean;

implementation
var
 __cList     : TSchGenericEntry;
 __sTable    : String;
 __sWhere    : String;
 __sQuery    : String;
 __bIsUpdate : Boolean;
 __TableName : String;
 __ValueName : String;
 __EqualsTo  : Integer;
 __Entry     : TSchGenericEntry;
 __Result    : TSchGenericEntryList;
 __cs        : TConnectSpec;
 __Could     : Boolean;

 __Die       : Boolean;



{
 Threading ...
}

function DB_Busy() : boolean;
begin
 Result := not __Die;
end;

function Thread_ExecuteUpdate( pParam : Pointer ) : LongInt; stdcall; forward;
function Thread_ExecuteSQLQuery( pParam : Pointer ) : LongInt; stdcall; forward;
function Thread_ExecuteSQLModify( pParam : Pointer ) : LongInt; stdcall; forward;
function Thread_Connect( pParam : Pointer ) : LongInt; stdcall; forward;

function Perform_Connect( cs : TConnectSpec ) : Boolean;
var
 thId : Cardinal;

begin
 __cs  := cs;
 __Die := False;

 CreateThread( nil, 0, Addr( Thread_Connect ), nil, 0, thId);

 while (not __Die) do
 begin
  Sleep( 50 );
  Application.ProcessMessages();
 end;

 Result := __Could;
end;

procedure Perform_ExecuteUpdate( cList : TSchGenericEntry; sTable, sWhere : String );
var
 thId : Cardinal;

begin
 __cList  := cList;
 __sTable := sTable;
 __sWhere := sWhere;

 __Die := False;
 CreateThread( nil, 0, Addr( Thread_ExecuteUpdate ), nil, 0, thId);

 while (not __Die) do
 begin
  Sleep( 50 );
  Application.ProcessMessages();
 end;
end;

function Perform_ExecuteSQLQuery  ( sQuery : String; bIsUpdate : Boolean ) : TSchGenericEntryList;
var
 thId : Cardinal;

begin
 __sQuery    := sQuery;
 __bIsUpdate := bIsUpdate;

 __Die := False;
 CreateThread( nil, 0, Addr( Thread_ExecuteSQLQuery ), nil, 0, thId);

 while (not __Die) do
 begin
  Sleep( 50 );
  Application.ProcessMessages();
 end;

 Result := __Result;
end;

procedure Perform_ExecuteSQLModify ( TableName, ValueName : String; EqualsTo : Integer; Entry : TSchGenericEntry );
var
 thId : Cardinal;

begin
 __TableName := TableName;
 __ValueName := ValueName;
 __EqualsTo  := EqualsTo;
 __Entry     := Entry;

 __Die := False;
 CreateThread( nil, 0, Addr( Thread_ExecuteSQLModify ), nil, 0, thId);

 while (not __Die) do
 begin
  Sleep( 50 );
  Application.ProcessMessages();
 end;
end;

{
 Threading ...
}

function Thread_ExecuteUpdate( pParam : Pointer ) : LongInt; stdcall;
begin
 SQL.ExecuteUpdate( __cList, __sTable, __sWhere );
 { ----------------- }

 __Die := True; { Notify Thread Death }
 Result := 0;
end;

function Thread_ExecuteSQLQuery( pParam : Pointer ) : LongInt; stdcall;
begin
 __Result := SQL.ExecuteSQLQuery( __sQuery, __bIsUpdate );
 { ----------------- }

 
 __Die := True; { Notify Thread Death }
 Result := 0;
end;

function Thread_ExecuteSQLModify( pParam : Pointer ) : LongInt; stdcall;
begin
 SQL.ExecuteSQLModify ( __TableName, __ValueName, __EqualsTo, __Entry );
 { ----------------- }

 __Die := True; { Notify Thread Death }
 Result := 0;
end;


function Thread_Connect( pParam : Pointer ) : LongInt; stdcall;
begin
 __Could := SQL.Connect ( __cs );
 { ----------------- }

 __Die := True; { Notify Thread Death }
 Result := 0;
end;


function LoadLastIndex( sTable : String ) : Integer;
var
 el : TSchGenericEntryList;
begin
 el     := Perform_ExecuteSQLQuery( 'SELECT MAX('+ FIELD_ID +') FROM ' + sTable, False );
 Result := 1;

 if el.Count = 1 then
  if el.Items[0].Count = 1 then
   if el.Items[0].Items[0].isInteger then
    begin
     Result := el.Items[0].Items[0].asInteger;
    end;

 el.Destroy();   
end;


procedure LoadLastIndexes();
begin
 iiClassRooms := LoadLastIndex( TABLE_CLASSROOMS );
end;


initialization
 __Die := True;

end.

unit TransferFields;
interface
Uses SchObject, SysUtils;

Type
TSchGenericField = class( TSchObject )
                  private
                    __String     : String;
                    __Integer    : Integer;
                    __TDateTime  : TDateTime;
                    __Boolean    : Boolean;

                    is__String : Boolean;
                    is__Integer    : Boolean;
                    is__TDateTime  : Boolean;

                    procedure set_asString( Value : String );
                    procedure set_asInteger( Value : Integer );
                    procedure set_asDate( Value : TDateTime );

                    function get_asString() : String;
                    function get_asInteger()    : Integer;
                    function get_asDate()       : TDateTime;

                  public
                    FieldName             : String;

                    constructor Create();
                    destructor Destroy(); override;

                    function Dupe() : TSchGenericField;

                    property isString : Boolean read is__String;
                    property isInteger    : Boolean read is__Integer;
                    property isDate       : Boolean read is__TDateTime;

                    property asString : String read get_asString  write set_asString;
                    property asInteger    : Integer read get_asInteger        write set_asInteger;
                    property asDate       : TDateTime read get_asDate         write set_asDate;
                 end;

 TSchGenericEntry
              = class( TSchObject )
                 private
                    fCount : Integer;
                    fList  : array of TSchGenericField;
                    function GetField(Index: Integer): TSchGenericField;
                    procedure SetField(Index: Integer; const Value: TSchGenericField);
                    
                    function GetFieldStr(Index: String): TSchGenericField;
                    procedure SetFieldStr(Index: String; const Value: TSchGenericField);

                  public
                    constructor Create();
                    destructor Destroy(); override;

                    function New( sName : String ) : TSchGenericField;

                    function Dupe() : TSchGenericEntry;

                    property Count : Integer read fCount;
                    property Items[ Index : Integer ] : TSchGenericField read GetField write SetField;
                    property nItems[ Index : String ] : TSchGenericField read GetFieldStr write SetFieldStr;

                    procedure Add( Field : TSchGenericField );
                 end;

 TSchGenericEntryList
              = class( TSchObject )
                 private
                    fCount : Integer;
                    fList  : array of TSchGenericEntry;
                    function GetEntry(Index: Integer): TSchGenericEntry;
                    procedure SetEntry(Index: Integer; const Value: TSchGenericEntry);
                  public
                    constructor Create();
                    destructor Destroy(); override;

                    function RemoveEntry( Index : Integer ) : TSchGenericEntry;

                    function Dupe() : TSchGenericEntryList;

                    procedure ClearRefs();

                    property Count : Integer read fCount;
                    property Items[ Index : Integer ] : TSchGenericEntry read GetEntry write SetEntry;
                    procedure Add( Entry : TSchGenericEntry );
                 end;

implementation

{ TSchGenericFieldList }

procedure TSchGenericEntry.Add(Field: TSchGenericField);
begin
 Inc(fCount);
 SetLength(fList, fCount);

 fList[ fCount - 1 ] := Field;
end;

constructor TSchGenericEntry.Create;
begin
 fCount := 0;
 SetLength(fList, 0);

 inherited;
end;

destructor TSchGenericEntry.Destroy;
var
 i : Integer;
begin
  if fCount > 0 then
   for i := 0 to fCount - 1 do
    fList[i].Destroy();

  inherited;
end;

function TSchGenericEntry.GetField(Index: Integer): TSchGenericField;
begin
  if (Index < 0) or (Index >= fCount) then
     raise ERangeError.Create('Index out of bounds');

  result := fList[ Index ];
end;

function TSchGenericEntry.GetFieldStr(Index: String): TSchGenericField;
var
 I : Integer;
begin
 if fCount > 0 then
   for I := 0 to fCount - 1 do
     if UpperCase(GetField( I ).FieldName) = UpperCase(Index) then
     begin
      Result := GetField( I );
      Exit;
     end;

 raise ERangeError.Create('Named field not found in DB Entry');

 Result := nil;
end;

procedure TSchGenericEntry.SetFieldStr(Index: String;const Value: TSchGenericField);
var
 I : Integer;
begin
 if fCount > 0 then
   for I := 0 to fCount - 1 do
     if GetField( I ).FieldName = Index then
     begin
      SetField(I, Value);
      Exit;
     end;
     
 raise ERangeError.Create('Named field not found in DB Entry');
end;

procedure TSchGenericEntry.SetField(Index: Integer;
  const Value: TSchGenericField);
begin
  if (Index < 0) or (Index >= fCount) then
     raise ERangeError.Create('Index out of bounds');

  fList[ Index ] := Value;
end;


{ TSchGenericField }

constructor TSchGenericField.Create;
begin
  is__String := false;
  is__Integer := false;
  is__TDateTime := false;

  inherited;
end;

destructor TSchGenericField.Destroy;
begin
  __String  := '';

  inherited;
end;


function TSchGenericField.get_asDate: TDateTime;
begin
  if not is__TDateTime then raise EConvertError.Create('Invalid typecasting of the field');
  result := __TDateTime;
end;

function TSchGenericField.get_asInteger: Integer;
begin
  if not is__Integer then raise EConvertError.Create('Invalid typecasting of the field');
  result := __Integer;
end;


function TSchGenericField.get_asString: String;
begin
  if not is__String then raise EConvertError.Create('Invalid typecasting of the field');
  result := __String;
end;


procedure TSchGenericField.set_asDate(Value: TDateTime);
begin
  __String := '';

  is__String  := false;
  is__Integer     := false;
  is__TDateTime   := true;


  __TDateTime     := Value;
end;

procedure TSchGenericField.set_asInteger(Value: Integer);
begin
  __String := '';

  is__String  := false;
  is__Integer     := true;
  is__TDateTime   := false;

  __Integer       := Value;
  __Boolean       := (Value > 0);
end;


procedure TSchGenericField.set_asString(Value: String);
begin
  __String := '';

  is__String  := true;
  is__Integer     := false;
  is__TDateTime   := false;

  __String    := Value;
end;

function TSchGenericField.Dupe: TSchGenericField;
begin
 Result := TSchGenericField.Create();

 Result.__String := __String;
 Result.__Integer    := __Integer;
 Result.__TDateTime  := __TDateTime;
 Result.__Boolean    := __Boolean;
 Result.is__String := is__String;
 Result.is__Integer    := is__Integer;
 Result.is__TDateTime  := is__TDateTime;
 
 Result.FieldName := FieldName;
end;

{ TSchGenericEntryList }

procedure TSchGenericEntryList.Add( Entry: TSchGenericEntry);
begin
 Inc(fCount);
 SetLength(fList, fCount);

 fList[ fCount - 1 ] := Entry;
end;

procedure TSchGenericEntryList.ClearRefs;
begin
 fCount := 0;
 SetLength( fList, 0);
end;

constructor TSchGenericEntryList.Create;
begin
 fCount := 0;
 SetLength(fList, 0);

 inherited;
end;

destructor TSchGenericEntryList.Destroy;
var
 i : Integer;
begin
  if fCount > 0 then
   for i := 0 to fCount - 1 do
    fList[i].Destroy();

  inherited;
end;

function TSchGenericEntryList.Dupe: TSchGenericEntryList;
var
 iX : Integer;
 
begin
 Result := TSchGenericEntryList.Create();

 if Count > 0 then
    for iX := 0 to Count - 1 do
    begin
     Result.Add( Items[iX].Dupe() );
    end;

end;

function TSchGenericEntryList.GetEntry(Index: Integer): TSchGenericEntry;
begin
  if (Index < 0) or (Index >= fCount) then
     raise ERangeError.Create('Index out of bounds');

  result := fList[ Index ];
end;

function TSchGenericEntryList.RemoveEntry(Index: Integer): TSchGenericEntry;
var
 i : Integer;
begin
  if (Index < 0) or (Index >= fCount) then
     raise ERangeError.Create('Index out of bounds');

  Result := fList[ Index ];

  if Index < (fCount - 1) then
    for i := Index to (fCount - 2) do fList[ i ] := fList[ i + 1 ];

 Dec(fCount);
 SetLength(fList, fCount);
end;

procedure TSchGenericEntryList.SetEntry(Index: Integer;
  const Value: TSchGenericEntry);
begin
  if (Index < 0) or (Index >= fCount) then
     raise ERangeError.Create('Index out of bounds');

  fList[ Index ] := Value;
end;

function TSchGenericEntry.New(sName: String): TSchGenericField;
begin
 Result := TSchGenericField.Create();
 Result.FieldName := sName;

 Add( Result );
end;

function TSchGenericEntry.Dupe: TSchGenericEntry;
var
 iX : Integer;
 
begin
 Result := TSchGenericEntry.Create();

 if Count > 0 then
    for iX := 0 to Count - 1 do
    begin
     Result.Add( Items[iX].Dupe() );
    end;

end;

end.

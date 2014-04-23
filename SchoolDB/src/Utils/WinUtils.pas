unit WinUtils;
interface
Uses Windows, SysUtils, Printers, TntClasses, Classes, Registry;

procedure SaveOption(Name, Key, Value : String;Root : HKey);
 function LoadOption(Name, Key, Default : String;Root : HKey) : String;
procedure DelOption (Name, Key : String;Root : HKey);

implementation

procedure SaveOption(Name, Key, Value : String;Root : HKey);
Var
 Reg : TRegistry;
begin
 Reg := TRegistry.Create;

 try
   Reg.RootKey := root;

    if Reg.OpenKey(key, True) then
     begin
      Reg.WriteString(name,value);
      Reg.CloseKey;
     end;

   finally
    Reg.Free;
   end;
end;

function LoadOption(Name, Key, Default : String;Root : HKey) : String;
Var
 Reg : TRegistry;
begin
  result:=default;
  Reg := TRegistry.Create;
 try
    Reg.RootKey := root;

    if Reg.OpenKey(Key, True) then
     begin
      if Reg.ValueExists(name) then result:=Reg.ReadString(name)
                               else result:=default;
      Reg.CloseKey;
     end;

   finally
    Reg.Free;
  end;
end;

procedure DelOption (Name, Key : String;Root : HKey);
Var
 Reg : TRegistry;
begin
  Reg := TRegistry.Create;
 try
    Reg.RootKey := root;

    if Reg.OpenKey(Key, True) then
     begin
      if Reg.ValueExists(name) then
       Reg.DeleteValue(name);
      Reg.CloseKey;
     end;

   finally
    Reg.Free;
 end;
end;

end.

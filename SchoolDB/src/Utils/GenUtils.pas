unit GenUtils;
interface
Uses SysUtils, Classes, Graphics, MD5, WinUtils;

function atoi( sVal : String ) : Integer;
function LiniarEncrypt( sVal : WideString ) : String;

function EncryptSimple( sVal : WideString ) : String;
function DecryptSimple( sVal : String ) : WideString;

implementation


function EncryptSimple( sVal : WideString ) : String;
var
 i        : Integer;
 sCk, sPt : String;
begin
 sCk := UTF8Encode( sVal );
 Result := '';

 for i := 1 to Length( sCk ) do
     sCk[i] := Char( Byte(sCk[i]) xor (i*2) );

 for i := Length( sCk ) downto 1 do
     begin
      sPt    := IntToHex( Byte(sCk[i]), 2 );
      sPt[1] := Char( (Byte(sPt[1]) - Ord('0')));
      sPt[2] := Char( (Byte(sPt[2]) - Ord('0') - 11));

      Result := Result + sPt;
     end;

 sCk := Result;
 Result := '';

 for i := 1 to Length(sCk) do
     Result := Result + IntToHex( Byte(sCk[i]), 2 );

end;

function DecryptSimple( sVal : String ) : WideString;
var
 i             : Integer;
 sCk, sPt, sRs : String;
begin
 sCk := '';

 { Cycle #1 }
 for i := 1 to Length(sVal) do
  begin
   if (i mod 2) = 1 then
      sPt := sVal[i] else
      sPt := sPt + sVal[i];

   if Length(sPt) = 2 then
      sCk := sCk + Char( atoi( '$' + sPt ) );
  end;

 sRs := '';

 { Cycle #2 }
 for i := 1 to Length(sCk) do
  begin
   if (i mod 2) = 1 then
      sPt := Char( (Byte(sCk[i])) + Ord('0') ) else
      sPt := sPt + Char( (Byte(sCk[i])) + Ord('0') + 11 );

   if Length(sPt) = 2 then
      sRs := sRs + Char( atoi( '$' + sPt ) );
  end;

 sCk := '';

 for i := Length(sRs) downto 1 do
  sCk := sCk + sRs[i];

 for i := 1 to Length( sCk ) do
     sCk[i] := Char( Byte(sCk[i]) xor (i*2) );

 Result := UTF8Decode( sCk );
end;

function atoi( sVal : String ) : Integer;
var
 Err : Integer;
begin
 Val( sVal, Result, Err );

 if Err <> 0 then Result := 0;
end;

function LiniarEncrypt( sVal : WideString ) : String;
var
 cMD5 : TLbMD5;
 cDG  : TMD5Digest;

 i    : Integer;
begin

 cMD5 := TLbMD5.Create( nil );
 cMD5.HashString( UTF8Encode( sVal ) );
 cMD5.GetDigest( cDG );

 Result := '';

 for i := 0 to SizeOf( cDG ) - 1 do
     Result := Result + IntToHex( cDG[i], 2 );

 cMD5.Destroy();
end;


end.

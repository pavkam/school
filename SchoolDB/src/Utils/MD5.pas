unit MD5;
interface
Uses
 Classes,
 SysUtils;

{ TLbMD5 }
type

  TMD5Digest   = array [0..15]  of Byte;         { 128 bits - MD5 }
  TLMDContext  = array [0..279] of Byte;       { LockBox message digest }
  TMD5Context  = array [0..87]  of Byte;        { MD5 }

  pMD5ContextEx = ^TMD5ContextEx;
  TMD5ContextEx = packed record
    Count : array [0..1] of Cardinal;  {number of bits handled mod 2^64}
    State : array [0..3] of Cardinal;  {scratch buffer}
    Buf   : array [0..63] of Byte;    {input buffer}
  end;

  TLBBase = class(TComponent)
  end;

{ TLbBaseComponent }
  TLBBaseComponent = class(TLBBase)
  protected {private}
    function GetVersion : string;
    procedure SetVersion(const Value : string);
  published {properties}
    property Version : string
      read GetVersion write SetVersion stored False;
  end;


{ TLbHash }
  TLbHash = class(TLbBaseComponent)
    protected {private}
      FBuf : array[0..1023] of Byte;
    public {methods}
      constructor Create(AOwner : TComponent); override;
      destructor Destroy; override;
      procedure HashBuffer(const Buf; BufSize : Cardinal); virtual; abstract;
      procedure HashFile(const AFileName : string); virtual; abstract;
      procedure HashStream(AStream: TStream); virtual; abstract;
      procedure HashString(const AStr : string); virtual; abstract;
    end;

  TLbMD5 = class(TLbHash)
    protected {private}
      FDigest : TMD5Digest;
    public {methods}
      constructor Create(AOwner : TComponent); override;
      destructor Destroy; override;

      procedure GetDigest(var Digest : TMD5Digest);

      procedure HashBuffer (const Buf; BufSize : Cardinal); override;
      procedure HashFile   (const AFileName : String); override;
      procedure HashStream (AStream: TStream); override;
      procedure HashString (const AStr : String); override;
    end;

implementation
Const
sLbVersion = '2.07';

function RolX(I, C : Cardinal) : Cardinal; register;
asm
  mov  ecx, edx         {get count to cl}
  rol  eax, cl          {rotate eax by cl}
end;

procedure Transform(var Buffer : array of Cardinal;  const InBuf : array of Cardinal);
const
  S11 = 7;
  S12 = 12;
  S13 = 17;
  S14 = 22;
  S21 = 5;
  S22 = 9;
  S23 = 14;
  S24 = 20;
  S31 = 4;
  S32 = 11;
  S33 = 16;
  S34 = 23;
  S41 = 6;
  S42 = 10;
  S43 = 15;
  S44 = 21;
var
  Buf : array [0..3] of Cardinal;                                       {!!.01}
  InA : array [0..15] of Cardinal;                                      {!!.01}
var
  A   : Cardinal;
  B   : Cardinal;
  C   : Cardinal;
  D   : Cardinal;

  procedure FF(var A : Cardinal;  B, C, D, X, S, AC : Cardinal);
  begin
    A := RolX(A + ((B and C) or (not B and D)) + X + AC, S) + B;
  end;

  procedure GG(var A : Cardinal;  B, C, D, X, S, AC : Cardinal);
  begin
    A := RolX(A + ((B and D) or (C and not D)) + X + AC, S) + B;
  end;

  procedure HH(var A : Cardinal;  B, C, D, X, S, AC : Cardinal);
  begin
    A := RolX(A + (B xor C xor D) + X + AC, S) + B;
  end;

  procedure II(var A : Cardinal;  B, C, D, X, S, AC : Cardinal);
  begin
    A := RolX(A + (C xor (B or not D)) + X + AC, S) + B;
  end;

begin
  Move(Buffer, Buf, SizeOf(Buf));                                    {!!.01}
  Move(InBuf, InA, SizeOf(InA));                                     {!!.01}
  A := Buf [0];
  B := Buf [1];
  C := Buf [2];
  D := Buf [3];


  {round 1}
  FF(A, B, C, D, InA [ 0], S11, $D76AA478);  { 1 }
  FF(D, A, B, C, InA [ 1], S12, $E8C7B756);  { 2 }
  FF(C, D, A, B, InA [ 2], S13, $242070DB);  { 3 }
  FF(B, C, D, A, InA [ 3], S14, $C1BDCEEE);  { 4 }
  FF(A, B, C, D, InA [ 4], S11, $F57C0FAF);  { 5 }
  FF(D, A, B, C, InA [ 5], S12, $4787C62A);  { 6 }
  FF(C, D, A, B, InA [ 6], S13, $A8304613);  { 7 }
  FF(B, C, D, A, InA [ 7], S14, $FD469501);  { 8 }
  FF(A, B, C, D, InA [ 8], S11, $698098D8);  { 9 }
  FF(D, A, B, C, InA [ 9], S12, $8B44F7AF);  { 10 }
  FF(C, D, A, B, InA [10], S13, $FFFF5BB1);  { 11 }
  FF(B, C, D, A, InA [11], S14, $895CD7BE);  { 12 }
  FF(A, B, C, D, InA [12], S11, $6B901122);  { 13 }
  FF(D, A, B, C, InA [13], S12, $FD987193);  { 14 }
  FF(C, D, A, B, InA [14], S13, $A679438E);  { 15 }
  FF(B, C, D, A, InA [15], S14, $49B40821);  { 16 }

  {round 2}
  GG(A, B, C, D, InA [ 1], S21, $F61E2562);  { 17 }
  GG(D, A, B, C, InA [ 6], S22, $C040B340);  { 18 }
  GG(C, D, A, B, InA [11], S23, $265E5A51);  { 19 }
  GG(B, C, D, A, InA [ 0], S24, $E9B6C7AA);  { 20 }
  GG(A, B, C, D, InA [ 5], S21, $D62F105D);  { 21 }
  GG(D, A, B, C, InA [10], S22, $02441453);  { 22 }
  GG(C, D, A, B, InA [15], S23, $D8A1E681);  { 23 }
  GG(B, C, D, A, InA [ 4], S24, $E7D3FBC8);  { 24 }
  GG(A, B, C, D, InA [ 9], S21, $21E1CDE6);  { 25 }
  GG(D, A, B, C, InA [14], S22, $C33707D6);  { 26 }
  GG(C, D, A, B, InA [ 3], S23, $F4D50D87);  { 27 }
  GG(B, C, D, A, InA [ 8], S24, $455A14ED);  { 28 }
  GG(A, B, C, D, InA [13], S21, $A9E3E905);  { 29 }
  GG(D, A, B, C, InA [ 2], S22, $FCEFA3F8);  { 30 }
  GG(C, D, A, B, InA [ 7], S23, $676F02D9);  { 31 }
  GG(B, C, D, A, InA [12], S24, $8D2A4C8A);  { 32 }

  {round 3}
  HH(A, B, C, D, InA [ 5], S31, $FFFA3942);  { 33 }
  HH(D, A, B, C, InA [ 8], S32, $8771F681);  { 34 }
  HH(C, D, A, B, InA [11], S33, $6D9D6122);  { 35 }
  HH(B, C, D, A, InA [14], S34, $FDE5380C);  { 36 }
  HH(A, B, C, D, InA [ 1], S31, $A4BEEA44);  { 37 }
  HH(D, A, B, C, InA [ 4], S32, $4BDECFA9);  { 38 }
  HH(C, D, A, B, InA [ 7], S33, $F6BB4B60);  { 39 }
  HH(B, C, D, A, InA [10], S34, $BEBFBC70);  { 40 }
  HH(A, B, C, D, InA [13], S31, $289B7EC6);  { 41 }
  HH(D, A, B, C, InA [ 0], S32, $EAA127FA);  { 42 }
  HH(C, D, A, B, InA [ 3], S33, $D4EF3085);  { 43 }
  HH(B, C, D, A, InA [ 6], S34,  $4881D05);  { 44 }
  HH(A, B, C, D, InA [ 9], S31, $D9D4D039);  { 45 }
  HH(D, A, B, C, InA [12], S32, $E6DB99E5);  { 46 }
  HH(C, D, A, B, InA [15], S33, $1FA27CF8);  { 47 }
  HH(B, C, D, A, InA [ 2], S34, $C4AC5665);  { 48 }

  {round 4}
  II(A, B, C, D, InA [ 0], S41, $F4292244);  { 49 }
  II(D, A, B, C, InA [ 7], S42, $432AFF97);  { 50 }
  II(C, D, A, B, InA [14], S43, $AB9423A7);  { 51 }
  II(B, C, D, A, InA [ 5], S44, $FC93A039);  { 52 }
  II(A, B, C, D, InA [12], S41, $655B59C3);  { 53 }
  II(D, A, B, C, InA [ 3], S42, $8F0CCC92);  { 54 }
  II(C, D, A, B, InA [10], S43, $FFEFF47D);  { 55 }
  II(B, C, D, A, InA [ 1], S44, $85845DD1);  { 56 }
  II(A, B, C, D, InA [ 8], S41, $6FA87E4F);  { 57 }
  II(D, A, B, C, InA [15], S42, $FE2CE6E0);  { 58 }
  II(C, D, A, B, InA [ 6], S43, $A3014314);  { 59 }
  II(B, C, D, A, InA [13], S44, $4E0811A1);  { 60 }
  II(A, B, C, D, InA [ 4], S41, $F7537E82);  { 61 }
  II(D, A, B, C, InA [11], S42, $BD3AF235);  { 62 }
  II(C, D, A, B, InA [ 2], S43, $2AD7D2BB);  { 63 }
  II(B, C, D, A, InA [ 9], S44, $EB86D391);  { 64 }

  Inc(Buf [0], A);
  Inc(Buf [1], B);
  Inc(Buf [2], C);
  Inc(Buf [3], D);

  Move(Buf, Buffer, SizeOf(Buffer));                                 {!!.01}
end;

{ -------------------------------------------------------------------------- }
procedure InitMD5(var Context : TMD5Context);
var
  MD5 : TMD5ContextEx;                                               {!!.01}
begin
  Move(Context, MD5, SizeOf(MD5));                                   {!!.01}
  MD5.Count[0] := 0;
  MD5.Count[1] := 0;

  {load magic initialization constants}
  MD5.State[0] := $67452301;
  MD5.State[1] := $EFCDAB89;
  MD5.State[2] := $98BADCFE;
  MD5.State[3] := $10325476;
  Move(MD5, Context, SizeOf(Context));                               {!!.01}
end;
{ -------------------------------------------------------------------------- }
procedure UpdateMD5(var Context : TMD5Context;  const Buf;  BufSize : LongInt);
var
  MD5    : TMD5ContextEx;
  InBuf  : array [0..15] of Cardinal;
  BufOfs : LongInt;
  MDI    : Word;
  I      : Word;
  II     : Word;
begin
  Move(Context, MD5, SizeOf(MD5));                                   {!!.01}

  {compute number of bytes mod 64}
  MDI := (MD5.Count[0] shr 3) and $3F;

  {update number of bits}
  if ((MD5.Count[0] + (Cardinal(BufSize) shl 3)) < MD5.Count[0]) then
    Inc(MD5.Count[1]);
  Inc(MD5.Count[0], BufSize shl 3);
  Inc(MD5.Count[1], BufSize shr 29);

  {add new byte acters to buffer}
  BufOfs := 0;
  while (BufSize > 0) do begin
    Dec(BufSize);
    MD5.Buf[MDI] := TByteArray(Buf)[BufOfs];                         {!!.01}
    Inc(MDI);
    Inc(BufOfs);
    if (MDI = $40) then begin
      II := 0;
      for I := 0 to 15 do begin
        InBuf[I] := LongInt(MD5.Buf[II + 3]) shl 24 or
          LongInt(MD5.Buf[II + 2]) shl 16 or
          LongInt(MD5.Buf[II + 1]) shl 8 or
          LongInt(MD5.Buf[II]);
        Inc(II, 4);
      end;
      Transform(MD5.State, InBuf);
      Transform(TMD5ContextEx( Context ).State, InBuf);
      MDI := 0;
    end;
  end;
  Move(MD5, Context, SizeOf(Context));                               {!!.01}
end;
{ -------------------------------------------------------------------------- }
procedure FinalizeMD5(var Context : TMD5Context; var Digest : TMD5Digest);
const
  Padding: array [0..63] of Byte = (
    $80, $00, $00, $00, $00, $00, $00, $00, $00, $00, $00, $00, $00, $00, $00, $00,
    $00, $00, $00, $00, $00, $00, $00, $00, $00, $00, $00, $00, $00, $00, $00, $00,
    $00, $00, $00, $00, $00, $00, $00, $00, $00, $00, $00, $00, $00, $00, $00, $00,
    $00, $00, $00, $00, $00, $00, $00, $00, $00, $00, $00, $00, $00, $00, $00, $00);
var
  MD5    : TMD5ContextEx;
  InBuf  : array [0..15] of Cardinal;
  MDI    : LongInt;
  I      : Word;
  II     : Word;
  PadLen : Word;
begin
  Move(Context, MD5, SizeOf(MD5));                                   {!!.01}
  {save number of bits}
  InBuf[14] := MD5.Count[0];
  InBuf[15] := MD5.Count[1];
  {compute number of bytes mod 64}
  MDI := (MD5.Count[0] shr 3) and $3F;
  {pad out to 56 mod 64}
  if (MDI < 56) then
    PadLen := 56 - MDI
  else
    PadLen := 120 - MDI;
  UpdateMD5(Context, Padding, PadLen);

  Move(Context, MD5, SizeOf(MD5));                                   {!!.01}

  {append length in bits and transform}
  II := 0;
  for I := 0 to 13 do begin
    InBuf[I] :=
      ( LongInt( MD5.Buf[ II + 3 ]) shl 24 ) or
      ( LongInt( MD5.Buf[ II + 2 ]) shl 16 ) or
      ( LongInt( MD5.Buf[ II + 1 ]) shl 8  ) or
        LongInt( MD5.Buf[ II     ]);
    Inc(II, 4);
  end;
  Transform(MD5.State, InBuf);
  {store buffer in digest}
  II := 0;
  for I := 0 to 3 do begin
    Digest[II] := Byte(MD5.State[I] and $FF);
    Digest[II + 1] := Byte((MD5.State[I] shr 8) and $FF);
    Digest[II + 2] := Byte((MD5.State[I] shr 16) and $FF);
    Digest[II + 3] := Byte((MD5.State[I] shr 24) and $FF);
    Inc(II, 4);
  end;
  Move(MD5, Context, SizeOf(Context));                               {!!.01}
end;
{ -------------------------------------------------------------------------- }
procedure HashMD5(var Digest : TMD5Digest; const Buf;  BufSize : LongInt);
var
  Context : TMD5Context;
begin
  fillchar( context, SizeOf( context ), $00 );
  InitMD5(Context);
  UpdateMD5(Context, Buf, BufSize);
  FinalizeMD5(Context, Digest);
end;

procedure StringHashMD5(var Digest : TMD5Digest; const Str : string);
begin
  HashMD5(Digest, Str[1], Length(Str));
end;


{ == TLbMD5 ================================================================ }
constructor TLbMD5.Create(AOwner : TComponent);
begin
  inherited Create(AOwner);
end;
{ -------------------------------------------------------------------------- }
destructor TLbMD5.Destroy;
begin
  inherited Destroy;
end;
{ -------------------------------------------------------------------------- }
procedure TLbMD5.GetDigest(var Digest : TMD5Digest);
begin
  Move(FDigest, Digest, SizeOf(Digest));
end;
{ -------------------------------------------------------------------------- }
procedure TLbMD5.HashBuffer(const Buf; BufSize : Cardinal);
begin
  HashMD5(FDigest, Buf, BufSize);
end;
{ -------------------------------------------------------------------------- }
procedure TLbMD5.HashFile(const AFileName : String);
var
  FS : TFileStream;
begin
  FS := TFileStream.Create(AFileName, fmOpenRead or fmShareDenyNone);
  try
    HashStream(FS);
  finally
    FS.Free;
  end;
end;
{ -------------------------------------------------------------------------- }
procedure TLbMD5.HashStream(AStream: TStream);
var
  Context : TMD5Context;
  BufSize : Integer;
begin
  InitMD5(Context);
  BufSize := AStream.Read(FBuf, SizeOf(FBuf));
  while (BufSize > 0) do begin
    UpdateMD5(Context, FBuf, BufSize);
    BufSize := AStream.Read(FBuf, SizeOf(FBuf));
  end;
  FinalizeMD5(Context, FDigest);
end;
{ -------------------------------------------------------------------------- }
procedure TLbMD5.HashString(const AStr : string);
begin
  StringHashMD5(FDigest, AStr);
end;


{ == TLbHash =============================================================== }
constructor TLbHash.Create(AOwner : TComponent);
begin
  inherited Create(AOwner);
end;
{ -------------------------------------------------------------------------- }
destructor TLbHash.Destroy;
begin
  inherited Destroy;
end;


{ == TLbBaseComponent ====================================================== }
function TLBBaseComponent.GetVersion : string;
begin
  Result := sLbVersion;
end;
{ -------------------------------------------------------------------------- }
procedure TLBBaseComponent.SetVersion(const Value : string);
begin
  { nop }
end;

end.

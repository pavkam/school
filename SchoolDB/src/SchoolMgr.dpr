program SchoolMgr;

uses
  Forms,
  frmLogOn in 'UI\frmLogOn.pas' {formLogOn},
  ConfFile in 'Utils\ConfFile.pas',
  GenUtils in 'Utils\GenUtils.pas',
  MD5 in 'Utils\MD5.pas',
  SchObject in 'Utils\SchObject.pas',
  WinUtils in 'Utils\WinUtils.pas',
  SQLAccess in 'DB\SQLAccess.pas',
  Globals in 'Utils\Globals.pas',
  TransferFields in 'DB\TransferFields.pas',
  frmMain in 'UI\frmMain.pas' {formMain},
  frmClassRooms in 'UI\frmClassRooms.pas' {formClassRooms};

{$R *.res}

begin
  Application.Initialize;
  Application.Title := 'School Manager';
  Application.CreateForm(TformLogOn, formLogOn);
  Application.CreateForm(TformMain, formMain);
  Application.CreateForm(TformClassRooms, formClassRooms);
  Application.Run;
end.

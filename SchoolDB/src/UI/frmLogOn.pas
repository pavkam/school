unit frmLogOn;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, XPMan, StdCtrls, ExtCtrls, WinUtils, GenUtils, Globals, SQLAccess;

type
  TformLogOn = class(TForm)
    lbUser: TLabel;
    lbPassword: TLabel;
    lbHost: TLabel;
    lbPort: TLabel;
    lbDB: TLabel;
    edtUser: TEdit;
    edtPass: TEdit;
    edtHost: TEdit;
    edtDB: TEdit;
    edtPort: TEdit;
    pnlSep1: TPanel;
    pnlSep2: TPanel;
    btAccept: TButton;
    btCancel: TButton;
    xpManifest: TXPManifest;
    procedure btCancelClick(Sender: TObject);
    procedure FormCreate(Sender: TObject);
    procedure FormDestroy(Sender: TObject);
    procedure btAcceptClick(Sender: TObject);
    procedure FormCloseQuery(Sender: TObject; var CanClose: Boolean);
  private
    { Private declarations }
  public
    { Public declarations }
  end;

var
  formLogOn: TformLogOn;

implementation

uses frmMain;

{$R *.dfm}

procedure TformLogOn.btCancelClick(Sender: TObject);
begin
 Close();
end;

procedure TformLogOn.FormCreate(Sender: TObject);
begin
 {
   Load default settings from Registry and fill in the Edit Boxes.
 }

 edtHost.Text := LoadOption( 'Host', 'Software\SchoolMgr', 'localhost', HKEY_CURRENT_USER );
 edtPort.Text := IntToStr( atoi( LoadOption( 'Port', 'Software\SchoolMgr', '0', HKEY_CURRENT_USER ) ) );
 edtDB.Text   := LoadOption( 'DB', 'Software\SchoolMgr', 'school', HKEY_CURRENT_USER );
 edtUser.Text := LoadOption( 'User', 'Software\SchoolMgr', '', HKEY_CURRENT_USER );
 edtPass.Text := LoadOption( 'Password', 'Software\SchoolMgr', '', HKEY_CURRENT_USER );
end;

procedure TformLogOn.FormDestroy(Sender: TObject);
begin
 {
   Save default settings to Registry.
 }

 SaveOption( 'Host', 'Software\SchoolMgr', edtHost.Text, HKEY_CURRENT_USER );
 SaveOption( 'DB', 'Software\SchoolMgr', edtDB.Text, HKEY_CURRENT_USER );
 SaveOption( 'User', 'Software\SchoolMgr', edtUser.Text, HKEY_CURRENT_USER );
 SaveOption( 'Password', 'Software\SchoolMgr', edtPass.Text, HKEY_CURRENT_USER );
 SaveOption( 'Port', 'Software\SchoolMgr', IntToStr( atoi( edtPort.Text ) ), HKEY_CURRENT_USER );
end;

procedure TformLogOn.btAcceptClick(Sender: TObject);
var
 cs : TConnectSpec;
 bCould : Boolean;
begin
 Globals.SQL := TSchSQLAccess.Create();

 cs.sHost := edtHost.Text;
 cs.wPort := atoi( edtPort.Text );
 cs.sUser := edtUser.Text;
 cs.sPassword := edtPass.Text;
 cs.sDBName := edtDB.Text;

 { Disable Controls }
  edtHost    .Enabled := False;
  edtPort    .Enabled := False;
  edtUser    .Enabled := False;
  edtPass    .Enabled := False;
  edtDB      .Enabled := False;
  btCancel   .Enabled := False;
  btAccept   .Enabled := False;

 bCould := Globals.Perform_Connect( cs );

 if ( not bCould ) then
 begin
  Application.MessageBox( 'Connection to the selected DataBase could not been established!', 'Error', MB_OK or MB_ICONERROR );
  
  { Enable Controls }
  edtHost    .Enabled := True;
  edtPort    .Enabled := True;
  edtUser    .Enabled := True;
  edtPass    .Enabled := True;
  edtDB      .Enabled := True;
  btCancel   .Enabled := True;
  btAccept   .Enabled := True;
 end else
 begin
  Globals.LoadLastIndexes();
  
  Hide();
  formMain.ShowModal();

  Close();
 end;

end;

procedure TformLogOn.FormCloseQuery(Sender: TObject;
  var CanClose: Boolean);
begin
 CanClose := not Globals.DB_Busy();
end;

end.

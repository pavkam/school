unit frmMain;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, JvExControls, JvComponent, JvButton, JvNavigationPane, StdCtrls,
  jpeg, ExtCtrls, JvExExtCtrls, JvPanel, ImgList, Globals;

type
  TformMain = class(TForm)
    btClassRooms: TJvNavPanelButton;
    btStudents: TJvNavPanelButton;
    btProffesors: TJvNavPanelButton;
    pnlLogo: TJvPanel;
    Image1: TImage;
    lbCrBy: TLabel;
    lbAlex: TLabel;
    lbPrikoke: TLabel;
    lbID: TLabel;
    btObjects: TJvNavPanelButton;
    btQuit: TJvNavPanelButton;
    imgList: TImageList;
    procedure btQuitClick(Sender: TObject);
    procedure btClassRoomsClick(Sender: TObject);
    procedure FormCloseQuery(Sender: TObject; var CanClose: Boolean);
  private
    { Private declarations }
  public
    { Public declarations }
  end;

var
  formMain: TformMain;

implementation

uses frmClassRooms;

{$R *.dfm}

procedure TformMain.btQuitClick(Sender: TObject);
begin
 Close();
end;

procedure TformMain.btClassRoomsClick(Sender: TObject);
begin
 btClassRooms    .Enabled := False;
 btStudents      .Enabled := False;
 btProffesors    .Enabled := False;
 btObjects       .Enabled := False;
 btQuit          .Enabled := False;

 formClassRooms.LoadListFromDB();
 formClassRooms.ifaceControls();
 formClassRooms.ShowModal();

 btClassRooms    .Enabled := True;
 btStudents      .Enabled := True;
 btProffesors    .Enabled := True;
 btObjects       .Enabled := True;
 btQuit          .Enabled := True;
end;

procedure TformMain.FormCloseQuery(Sender: TObject; var CanClose: Boolean);
begin
 CanClose := not Globals.DB_Busy();
end;

end.

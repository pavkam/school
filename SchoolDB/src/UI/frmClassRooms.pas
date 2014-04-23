unit frmClassRooms;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, ExtCtrls, StdCtrls, Globals, TransferFields;

type
  TformClassRooms = class(TForm)
    lbAvailClass: TLabel;
    lbClasses: TListBox;
    gbInfo: TGroupBox;
    lbName: TLabel;
    edtName: TEdit;
    btSave: TButton;
    btDelete: TButton;
    btClose: TButton;
    btNew: TButton;
    pnlSep12: TPanel;
    btViewStud: TButton;
    btViewProff: TButton;
    procedure btCloseClick(Sender: TObject);
    procedure btNewClick(Sender: TObject);
    procedure lbClassesClick(Sender: TObject);
    procedure lbClassesKeyPress(Sender: TObject; var Key: Char);
    procedure FormCloseQuery(Sender: TObject; var CanClose: Boolean);
    procedure btSaveClick(Sender: TObject);
    procedure edtNameKeyPress(Sender: TObject; var Key: Char);
    procedure btDeleteClick(Sender: TObject);
    procedure btViewStudClick(Sender: TObject);
    procedure btViewProffClick(Sender: TObject);
  private
    { Private declarations }
  public
    { Public declarations }

    procedure ifaceControls();
    procedure LoadListFromDB();
  end;

var
  formClassRooms: TformClassRooms;

implementation

{$R *.dfm}

procedure TformClassRooms.btCloseClick(Sender: TObject);
begin
 Close();
end;

procedure TformClassRooms.ifaceControls;
var
 tEnt : TSchGenericEntry;
begin
 if lbClasses.ItemIndex = -1 then
 begin
  edtName     .Enabled := False;
  btDelete    .Enabled := False;
  btSave      .Enabled := False;
  btViewStud  .Enabled := False;
  btViewProff .Enabled := False;
 end else
 begin
  edtName     .Enabled := True;
  btDelete    .Enabled := True;
  btSave      .Enabled := True;
  btViewStud  .Enabled := True;
  btViewProff .Enabled := True;

  tEnt := lbClasses.Items.Objects[ lbClasses.ItemIndex ] as TSchGenericEntry;
  edtName.Text := tEnt.nItems[ Globals.FIELD_NAME ].asString;

 end;
end;

procedure TformClassRooms.btNewClick(Sender: TObject);
var
 tEnt : TSchGenericEntry;
 sStd : String;
begin
 sStd := 'New Class Room';

 tEnt := TSchGenericEntry.Create();

 Inc( iiClassRooms );
 tEnt.New( FIELD_ID ).asInteger  := iiClassRooms;
 tEnt.New( FIELD_NAME ).asString := sStd;

 btClose    .Enabled := False;
 btNew      .Enabled := False;
 gbInfo     .Enabled := False;
 lbClasses  .Enabled := False;

 Globals.Perform_ExecuteSQLModify( Globals.TABLE_CLASSROOMS, '', 0, tEnt );

 btClose    .Enabled := True;
 btNew      .Enabled := True;
 gbInfo     .Enabled := True;
 lbClasses  .Enabled := True;

 lbClasses.AddItem( sStd, tEnt );
 lbClasses.ItemIndex := lbClasses.Items.Count - 1;

 ifaceControls();
end;

procedure TformClassRooms.lbClassesClick(Sender: TObject);
begin
 ifaceControls();
end;

procedure TformClassRooms.lbClassesKeyPress(Sender: TObject;var Key: Char);
begin
 ifaceControls();
end;

procedure TformClassRooms.FormCloseQuery(Sender: TObject;
  var CanClose: Boolean);
begin
 CanClose := not Globals.DB_Busy();
end;

procedure TformClassRooms.LoadListFromDB;
var
 tEl : TSchGenericEntryList;
 i   : Integer;
begin
 lbClasses.Clear();
 
 tEl := Globals.Perform_ExecuteSQLQuery( 'SELECT * FROM ' + Globals.TABLE_CLASSROOMS + ';', False );

 if Assigned( tEl ) then
 begin
  if (tEl.Count > 0) then
     for i := 0 to tEl.Count - 1 do
     begin
      lbClasses.AddItem( tEl.Items[i].nItems[ Globals.FIELD_NAME ].asString, tEl.Items[i].Dupe() );
     end;

  tEl.Destroy();
 end;

 if lbClasses.Items.Count > 0 then
    lbClasses.ItemIndex := 0;
end;

procedure TformClassRooms.btSaveClick(Sender: TObject);
var
 tEnt : TSchGenericEntry;
begin
 tEnt := lbClasses.Items.Objects[ lbClasses.ItemIndex ] as TSchGenericEntry;
 tEnt.nItems[ Globals.FIELD_NAME ].asString := edtName.Text;
 lbClasses.Items.Strings[ lbClasses.ItemIndex ] := edtName.Text;

 btClose    .Enabled := False;
 btNew      .Enabled := False;
 gbInfo     .Enabled := False;
 lbClasses  .Enabled := False;

 Globals.Perform_ExecuteSQLModify( Globals.TABLE_CLASSROOMS, Globals.FIELD_ID,
         tEnt.nItems[ Globals.FIELD_ID ].asInteger, tEnt );

 btClose    .Enabled := True;
 btNew      .Enabled := True;
 gbInfo     .Enabled := True;
 lbClasses  .Enabled := True;
end;

procedure TformClassRooms.edtNameKeyPress(Sender: TObject; var Key: Char);
begin
 if Key = #13 then btSave.Click();    
end;

procedure TformClassRooms.btDeleteClick(Sender: TObject);
var
 tEnt : TSchGenericEntry;
begin
 if Application.MessageBox( 'Are you sure you want to delete this item?', 'Confirm', MB_YESNO or MB_ICONQUESTION ) =
    mrNo then exit;

 tEnt := lbClasses.Items.Objects[ lbClasses.ItemIndex ] as TSchGenericEntry;
 lbClasses.Items.Delete( lbClasses.ItemIndex );

 btClose    .Enabled := False;
 btNew      .Enabled := False;
 gbInfo     .Enabled := False;
 lbClasses  .Enabled := False;

 { TODO : Delete Links to tblStudents ( mark their owner field to zero ) }
 { TODO : Delete Links to tblProffesors ... delete link entries }

 Globals.Perform_ExecuteSQLQuery( 'DELETE FROM ' + Globals.TABLE_CLASSROOMS + ' WHERE ' + Globals.FIELD_ID +
                                  ' = ' + IntToStr( tEnt.nItems[ Globals.FIELD_ID].asInteger ), True );
 btClose    .Enabled := True;
 btNew      .Enabled := True;
 gbInfo     .Enabled := True;
 lbClasses  .Enabled := True;

 tEnt.Destroy();
 
 ifaceControls();
end;

procedure TformClassRooms.btViewStudClick(Sender: TObject);
begin
 { TODO : Implement this }
end;

procedure TformClassRooms.btViewProffClick(Sender: TObject);
begin
{ TODO : Implement This }
end;

end.

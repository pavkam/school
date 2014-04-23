object formClassRooms: TformClassRooms
  Left = 329
  Top = 194
  BorderIcons = [biSystemMenu, biMinimize]
  BorderStyle = bsSingle
  Caption = 'Class Rooms'
  ClientHeight = 327
  ClientWidth = 447
  Color = clBtnFace
  Font.Charset = DEFAULT_CHARSET
  Font.Color = clWindowText
  Font.Height = -11
  Font.Name = 'Verdana'
  Font.Style = []
  OldCreateOrder = False
  Position = poScreenCenter
  OnCloseQuery = FormCloseQuery
  PixelsPerInch = 96
  TextHeight = 13
  object lbAvailClass: TLabel
    Left = 8
    Top = 8
    Width = 150
    Height = 13
    Caption = 'Available Class Rooms:'
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -11
    Font.Name = 'Verdana'
    Font.Style = [fsBold]
    ParentFont = False
  end
  object lbClasses: TListBox
    Left = 8
    Top = 24
    Width = 241
    Height = 297
    ItemHeight = 13
    TabOrder = 0
    OnClick = lbClassesClick
    OnKeyPress = lbClassesKeyPress
  end
  object gbInfo: TGroupBox
    Left = 256
    Top = 19
    Width = 185
    Height = 238
    Caption = 'Edit Class Room'
    TabOrder = 1
    object lbName: TLabel
      Left = 8
      Top = 16
      Width = 38
      Height = 13
      Caption = 'Name:'
    end
    object edtName: TEdit
      Left = 8
      Top = 32
      Width = 169
      Height = 21
      TabOrder = 0
      OnKeyPress = edtNameKeyPress
    end
    object btSave: TButton
      Left = 104
      Top = 64
      Width = 75
      Height = 25
      Caption = 'Save'
      TabOrder = 1
      OnClick = btSaveClick
    end
    object btDelete: TButton
      Left = 22
      Top = 64
      Width = 75
      Height = 25
      Caption = 'Delete'
      TabOrder = 2
      OnClick = btDeleteClick
    end
    object pnlSep12: TPanel
      Left = 5
      Top = 96
      Width = 174
      Height = 5
      TabOrder = 3
    end
    object btViewStud: TButton
      Left = 8
      Top = 178
      Width = 113
      Height = 25
      Caption = 'Students ...'
      Font.Charset = DEFAULT_CHARSET
      Font.Color = clWindowText
      Font.Height = -11
      Font.Name = 'Verdana'
      Font.Style = [fsBold]
      ParentFont = False
      TabOrder = 4
      OnClick = btViewStudClick
    end
    object btViewProff: TButton
      Left = 8
      Top = 207
      Width = 113
      Height = 25
      Caption = 'Proffesors ...'
      Font.Charset = DEFAULT_CHARSET
      Font.Color = clWindowText
      Font.Height = -11
      Font.Name = 'Verdana'
      Font.Style = [fsBold]
      ParentFont = False
      TabOrder = 5
      OnClick = btViewProffClick
    end
  end
  object btClose: TButton
    Left = 351
    Top = 298
    Width = 92
    Height = 25
    Caption = 'Close'
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -11
    Font.Name = 'Verdana'
    Font.Style = [fsBold]
    ParentFont = False
    TabOrder = 2
    OnClick = btCloseClick
  end
  object btNew: TButton
    Left = 351
    Top = 270
    Width = 92
    Height = 25
    Caption = 'Add New'
    TabOrder = 3
    OnClick = btNewClick
  end
end

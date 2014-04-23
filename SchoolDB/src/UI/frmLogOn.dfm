object formLogOn: TformLogOn
  Left = 363
  Top = 259
  BorderIcons = [biSystemMenu]
  BorderStyle = bsSingle
  Caption = 'Log On'
  ClientHeight = 178
  ClientWidth = 376
  Color = clBtnFace
  Font.Charset = DEFAULT_CHARSET
  Font.Color = clWindowText
  Font.Height = -11
  Font.Name = 'Verdana'
  Font.Style = []
  OldCreateOrder = False
  OnCloseQuery = FormCloseQuery
  OnCreate = FormCreate
  OnDestroy = FormDestroy
  PixelsPerInch = 96
  TextHeight = 13
  object lbUser: TLabel
    Left = 8
    Top = 80
    Width = 31
    Height = 13
    Caption = 'User:'
  end
  object lbPassword: TLabel
    Left = 8
    Top = 104
    Width = 59
    Height = 13
    Caption = 'Password:'
  end
  object lbHost: TLabel
    Left = 8
    Top = 8
    Width = 30
    Height = 13
    Caption = 'Host:'
  end
  object lbPort: TLabel
    Left = 248
    Top = 8
    Width = 28
    Height = 13
    Caption = 'Port:'
  end
  object lbDB: TLabel
    Left = 8
    Top = 32
    Width = 60
    Height = 13
    Caption = 'DataBase:'
  end
  object edtUser: TEdit
    Left = 96
    Top = 76
    Width = 145
    Height = 21
    TabOrder = 0
  end
  object edtPass: TEdit
    Left = 96
    Top = 100
    Width = 145
    Height = 21
    PasswordChar = '*'
    TabOrder = 1
  end
  object edtHost: TEdit
    Left = 80
    Top = 4
    Width = 145
    Height = 21
    TabOrder = 2
  end
  object edtDB: TEdit
    Left = 80
    Top = 28
    Width = 289
    Height = 21
    TabOrder = 3
  end
  object edtPort: TEdit
    Left = 288
    Top = 4
    Width = 81
    Height = 21
    TabOrder = 4
  end
  object pnlSep1: TPanel
    Left = 8
    Top = 56
    Width = 361
    Height = 5
    TabOrder = 5
  end
  object pnlSep2: TPanel
    Left = 7
    Top = 136
    Width = 361
    Height = 5
    TabOrder = 6
  end
  object btAccept: TButton
    Left = 293
    Top = 149
    Width = 75
    Height = 25
    Caption = 'Log On'
    TabOrder = 7
    OnClick = btAcceptClick
  end
  object btCancel: TButton
    Left = 213
    Top = 150
    Width = 75
    Height = 25
    Caption = 'Cancel'
    TabOrder = 8
    OnClick = btCancelClick
  end
  object xpManifest: TXPManifest
    Left = 344
    Top = 104
  end
end

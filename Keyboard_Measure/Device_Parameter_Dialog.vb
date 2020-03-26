
Public Class Device_Parameter_Dialog

    Dim _Model As Model_Enum
    Dim _Param_Model As Param_Model_Enum
    Public Enum Model_Enum
        add = 1
        updata = 2
    End Enum
    Public Enum Param_Model_Enum
        Double_Value = 1
        Int_Value = 2
        String_Value = 3
        Bool_Value = 4
    End Enum

    Sub New(ByVal Mode As Model_Enum, ByVal Param_Model As Param_Model_Enum)
        _Model = Mode
        _Param_Model = Param_Model

        ' 此调用是设计器所必需的。
        InitializeComponent()
        ' 在 InitializeComponent() 调用之后添加任何初始化。
    End Sub

    Private Sub Btn_OK_Click(sender As System.Object, e As System.EventArgs) Handles Btn_OK.Click
        Select Case _Param_Model
            Case Param_Model_Enum.Double_Value
                Device_ParamType.变量名称 = TextBoxX_Name_Double.Text.ToString.Trim
            Case Param_Model_Enum.Int_Value
                Device_ParamType.变量名称 = TextBoxX_Name_Int.Text.ToString.Trim
            Case Param_Model_Enum.String_Value
                Device_ParamType.变量名称 = TextBoxX_Name_Str.Text.ToString.Trim
            Case Param_Model_Enum.Bool_Value
                Device_ParamType.变量名称 = TextBoxX_Name_Bool.Text.ToString.Trim
        End Select

        Device_ParamType.Double_当前值 = Double_Value.Value
        Device_ParamType.Double_最小值 = Double_Value_Min.Value
        Device_ParamType.Double_最大值 = Double_Value_Max.Value
        Device_ParamType.Integer_当前值 = Integer_Value.Value
        Device_ParamType.Integer_最小值 = Integer_Value_Min.Value
        Device_ParamType.Integer_最大值 = Integer_Value_Max.Value
        Device_ParamType.String_当前值 = String_Value.Text.ToString.Trim
        Device_ParamType.Bool_当前值 = IIf(SwitchButton_Bool.Value = True, 1, 0)

        Me.DialogResult = Windows.Forms.DialogResult.OK
    End Sub


    Private Sub Btn_CANCEL_Click(sender As System.Object, e As System.EventArgs) Handles Btn_CANCEL.Click
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub Device_Bool_Param_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load

        Double型.Visible = False
        Integer型.Visible = False
        STRING型.Visible = False
        BOOLEAN型.Visible = False

        Select Case _Param_Model
            Case Param_Model_Enum.Double_Value
                Double型.Visible = True
                Select Case _Model
                    Case Model_Enum.add
                        TextBoxX_Name_Double.Enabled = True
                        TextBoxX_Name_Double.Text = ""
                        Double_Value_Min.Value = 0
                        Double_Value_Max.Value = 1
                        Double_Value.MinValue = Double_Value_Min.Value
                        Double_Value.MaxValue = Double_Value_Max.Value
                        Double_Value.Value = 0
                    Case Model_Enum.updata
                        TextBoxX_Name_Double.Enabled = False
                        TextBoxX_Name_Double.Text = Device_ParamType.变量名称
                        Double_Value_Min.Value = Device_ParamType.Double_最小值
                        Double_Value_Max.Value = Device_ParamType.Double_最大值
                        Double_Value.MinValue = Double_Value_Min.Value
                        Double_Value.MaxValue = Double_Value_Max.Value
                        Double_Value.Value = Device_ParamType.Double_当前值
                End Select
            Case Param_Model_Enum.Int_Value
                Integer型.Visible = True
                Select Case _Model
                    Case Model_Enum.add
                        TextBoxX_Name_Int.Enabled = True
                        TextBoxX_Name_Int.Text = ""

                        Integer_Value_Min.Value = 0
                        Integer_Value_Max.Value = 1
                        Integer_Value.MinValue = Integer_Value_Min.Value
                        Integer_Value.MaxValue = Integer_Value_Max.Value
                        Integer_Value.Value = 0
                    Case Model_Enum.updata
                        TextBoxX_Name_Int.Enabled = False
                        TextBoxX_Name_Int.Text = Device_ParamType.变量名称

                        Integer_Value_Min.Value = Device_ParamType.Integer_最小值
                        Integer_Value_Max.Value = Device_ParamType.Integer_最大值
                        Integer_Value.MinValue = Integer_Value_Min.Value
                        Integer_Value.MaxValue = Integer_Value_Max.Value
                        Integer_Value.Value = Device_ParamType.Integer_当前值
                End Select
            Case Param_Model_Enum.String_Value
                STRING型.Visible = True
                Select Case _Model
                    Case Model_Enum.add
                        TextBoxX_Name_Str.Enabled = True
                        TextBoxX_Name_Str.Text = ""
                        String_Value.Text = ""
                    Case Model_Enum.updata
                        TextBoxX_Name_Str.Enabled = False
                        TextBoxX_Name_Str.Text = Device_ParamType.变量名称
                        String_Value.Text = Device_ParamType.String_当前值
                End Select
            Case Param_Model_Enum.Bool_Value
                BOOLEAN型.Visible = True
                Select Case _Model
                    Case Model_Enum.add
                        TextBoxX_Name_Bool.Enabled = True
                        TextBoxX_Name_Bool.Text = ""
                        SwitchButton_Bool.Value = False
                    Case Model_Enum.updata
                        TextBoxX_Name_Bool.Enabled = False
                        TextBoxX_Name_Bool.Text = Device_ParamType.变量名称
                        SwitchButton_Bool.Value = IIf(Device_ParamType.Bool_当前值 = 1, True, False)
                End Select
        End Select

        Select Case Main.User_Name.ToUpper
            Case UserName.Admin
                PanelEx14.Enabled = True
                PanelEx20.Enabled = True
                PanelEx2.Enabled = True
                PanelEx1.Enabled = True
            Case Else
                PanelEx14.Enabled = False
                PanelEx20.Enabled = False
                PanelEx2.Enabled = False
                PanelEx1.Enabled = False

        End Select

    End Sub


    Private Sub Double_Value_Min_ValueChanged(sender As System.Object, e As System.EventArgs) Handles Double_Value_Min.ValueChanged
        Double_Value.MinValue = Double_Value_Min.Value
        If Double_Value_Max.Value <= Double_Value_Min.Value Then
            Double_Value_Max.Value = Double_Value_Min.Value
        End If
    End Sub

    Private Sub Double_Value_Max_ValueChanged(sender As System.Object, e As System.EventArgs) Handles Double_Value_Max.ValueChanged
        Double_Value.MaxValue = Double_Value_Max.Value
    End Sub

    Private Sub Integer_Value_Min_ValueChanged(sender As System.Object, e As System.EventArgs) Handles Integer_Value_Min.ValueChanged
        Integer_Value.MinValue = Integer_Value_Min.Value
        If Integer_Value_Max.Value <= Integer_Value_Min.Value Then
            Integer_Value_Max.Value = Integer_Value_Min.Value
        End If
    End Sub

    Private Sub Integer_Value_Max_ValueChanged(sender As System.Object, e As System.EventArgs) Handles Integer_Value_Max.ValueChanged
        Integer_Value.MaxValue = Integer_Value_Max.Value
    End Sub

    Private Sub PanelEx14_Click(sender As System.Object, e As System.EventArgs) Handles PanelEx14.Click
        If TextBoxX_Name_Double.Enabled = True Then
            TextBoxX_Name_Double.Enabled = False
            Double_Value_Min.Enabled = False
            Double_Value_Max.Enabled = False
        Else
            TextBoxX_Name_Double.Enabled = True
            Double_Value_Min.Enabled = True
            Double_Value_Max.Enabled = True
        End If
    End Sub

    Private Sub PanelEx20_Click(sender As System.Object, e As System.EventArgs) Handles PanelEx20.Click
        If TextBoxX_Name_Int.Enabled = True Then
            TextBoxX_Name_Int.Enabled = False
            Integer_Value_Min.Enabled = False
            Integer_Value_Max.Enabled = False
        Else
            TextBoxX_Name_Int.Enabled = True
            Integer_Value_Min.Enabled = True
            Integer_Value_Max.Enabled = True
        End If
    End Sub

    Private Sub PanelEx2_Click(sender As System.Object, e As System.EventArgs) Handles PanelEx2.Click
        If TextBoxX_Name_Str.Enabled = True Then
            TextBoxX_Name_Str.Enabled = False
        Else
            TextBoxX_Name_Str.Enabled = True
        End If
    End Sub

    Private Sub PanelEx1_Click(sender As System.Object, e As System.EventArgs) Handles PanelEx1.Click
        If TextBoxX_Name_Bool.Enabled = False Then
            TextBoxX_Name_Bool.Enabled = True
        Else
            TextBoxX_Name_Bool.Enabled = False
        End If
    End Sub
End Class
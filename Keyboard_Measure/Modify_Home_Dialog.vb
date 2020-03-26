Imports Keyboard_Measure.Home_Dialog

Public Class Modify_Home_Dialog

    Dim _Model As Integer = 0
    Sub New(ByVal Mode As Integer)
        ' 此调用是设计器所必需的。
        InitializeComponent()
        ' 在 InitializeComponent() 调用之后添加任何初始化。
        _Model = Mode
    End Sub

    Private Sub Add_Dialog_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Home_Dialog.SQLCON.HomeParamObj.轴名称 = AXIS_NAME_1.SelectedItem.ToString & AXIS_NAME_2.SelectedItem.ToString
        Home_Dialog.SQLCON.HomeParamObj.轴号 = AXIS_NO.Value
        Home_Dialog.SQLCON.HomeParamObj.导程 = AXIS_M.Value
        Home_Dialog.SQLCON.HomeParamObj.回原点模式 = HOME_SELECT_MODE.SelectedItem.ToString.Trim
        Home_Dialog.SQLCON.HomeParamObj.回原点搜索方向 = HOME_DIRECTION.SelectedItem.ToString.Trim
        Home_Dialog.SQLCON.HomeParamObj.回原点Z相信号 = HOME_EZA.SelectedItem.ToString.Trim
        Home_Dialog.SQLCON.HomeParamObj.回原点曲线 = HOME_CURVE.SelectedItem.ToString.Trim
        Home_Dialog.SQLCON.HomeParamObj.回原点速度 = HOME_SPEED.Value
        Home_Dialog.SQLCON.HomeParamObj.回原点加减速度 = HOME_ADC.Value
        Home_Dialog.SQLCON.HomeParamObj.回原点偏移 = HOME_OFFSET.Value
        Home_Dialog.SQLCON.HomeParamObj.回原点顺序 = HOME_INDEX.Value
    End Sub

    Private Sub Add_Dialog_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Select Case _Model
            Case 1 '添加
                AXIS_NAME_1.SelectedIndex = 0
                AXIS_NAME_2.SelectedIndex = 0
                AXIS_NO.Value = 1
                AXIS_M.Value = 5
                HOME_SELECT_MODE.SelectedIndex = 0
                HOME_DIRECTION.SelectedIndex = 0
                HOME_EZA.SelectedIndex = 1
                HOME_CURVE.SelectedIndex = 1
                HOME_SPEED.Value = 20
                HOME_ADC.Value = 0.1
                HOME_OFFSET.Value = 0
                HOME_INDEX.Value = 1
            Case 2 '更新
                Dim str1, str2, str3 As String
                str1 = Home_Dialog.SQLCON.HomeParamObj.轴名称
                str2 = Mid(str1, 1, 1)
                str3 = Mid(str1, 2, 2)
                If IsNumeric(str2) = False Then
                    Select Case str2
                        Case "X"
                            AXIS_NAME_1.SelectedIndex = 0
                        Case "Y"
                            AXIS_NAME_1.SelectedIndex = 1
                        Case "R"
                            AXIS_NAME_1.SelectedIndex = 2
                        Case "Z"
                            AXIS_NAME_1.SelectedIndex = 3
                        Case "U"
                            AXIS_NAME_1.SelectedIndex = 4
                        Case "S"
                            AXIS_NAME_1.SelectedIndex = 5
                        Case Else
                            AXIS_NAME_1.SelectedIndex = 0
                    End Select
                End If
                If IsNumeric(str3) = True Then
                    Select Case str3
                        Case "01"
                            AXIS_NAME_2.SelectedIndex = 0
                        Case "02"
                            AXIS_NAME_2.SelectedIndex = 1
                        Case "03"
                            AXIS_NAME_2.SelectedIndex = 2
                        Case "04"
                            AXIS_NAME_2.SelectedIndex = 3
                        Case "05"
                            AXIS_NAME_2.SelectedIndex = 4
                        Case "06"
                            AXIS_NAME_2.SelectedIndex = 5
                        Case "07"
                            AXIS_NAME_2.SelectedIndex = 6
                        Case "08"
                            AXIS_NAME_2.SelectedIndex = 7
                        Case "09"
                            AXIS_NAME_2.SelectedIndex = 8
                        Case "10"
                            AXIS_NAME_2.SelectedIndex = 9
                        Case Else
                            AXIS_NAME_2.SelectedIndex = 0
                    End Select
                End If

                AXIS_NO.Value = Home_Dialog.SQLCON.HomeParamObj.轴号
                AXIS_M.Value = Home_Dialog.SQLCON.HomeParamObj.导程

                Select Case Home_Dialog.SQLCON.HomeParamObj.回原点模式
                    Case "HOME_MODE_ORG"
                        HOME_SELECT_MODE.SelectedIndex = 0
                    Case "HOME_MODE_EL"
                        HOME_SELECT_MODE.SelectedIndex = 1
                    Case "HOME_MODE_EZ"
                        HOME_SELECT_MODE.SelectedIndex = 2
                    Case Else
                        HOME_SELECT_MODE.SelectedIndex = 0
                End Select
                Select Case Home_Dialog.SQLCON.HomeParamObj.回原点搜索方向
                    Case "Positive"
                        HOME_DIRECTION.SelectedIndex = 0
                    Case "Negative"
                        HOME_DIRECTION.SelectedIndex = 1
                    Case Else
                        HOME_DIRECTION.SelectedIndex = 0
                End Select
                Select Case Home_Dialog.SQLCON.HomeParamObj.回原点Z相信号
                    Case "Disable"
                        HOME_EZA.SelectedIndex = 0
                    Case "Enable"
                        HOME_EZA.SelectedIndex = 1
                    Case Else
                        HOME_EZA.SelectedIndex = 0
                End Select
                Select Case Home_Dialog.SQLCON.HomeParamObj.回原点曲线
                    Case "T_curve"
                        HOME_CURVE.SelectedIndex = 0
                    Case "S_curve"
                        HOME_CURVE.SelectedIndex = 1
                    Case Else
                        HOME_CURVE.SelectedIndex = 0
                End Select
                HOME_SPEED.Value = Home_Dialog.SQLCON.HomeParamObj.回原点速度
                HOME_ADC.Value = Home_Dialog.SQLCON.HomeParamObj.回原点加减速度
                HOME_OFFSET.Value = Home_Dialog.SQLCON.HomeParamObj.回原点偏移
                HOME_INDEX.Value = Home_Dialog.SQLCON.HomeParamObj.回原点顺序
        End Select
    End Sub

    Private Sub ButtonX1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonX1.Click
        Me.DialogResult = Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub ButtonX2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonX2.Click
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub
End Class
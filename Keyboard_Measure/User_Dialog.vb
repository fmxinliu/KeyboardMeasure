Imports System.Windows.Forms
Imports DevComponents.DotNetBar

Public Class User_Dialog

    Dim SQLCON As New GeneralClass.SQL_LIB
    Dim Switch() As DevComponents.DotNetBar.Controls.SwitchButton
    Dim Select_Row_Index, Max_Row_Count As Integer

    Dim USER_PARAMETER As SQL_LIB.USER_PARAMETER_STRUCTURE

    Sub Dim_Switch()
        Switch = New DevComponents.DotNetBar.Controls.SwitchButton(10) {}
        Switch(0) = SwitchButton1
        Switch(1) = SwitchButton2
        Switch(2) = SwitchButton3
        Switch(3) = SwitchButton4
        Switch(4) = SwitchButton5
        Switch(5) = SwitchButton6
        Switch(6) = SwitchButton7
        Switch(7) = SwitchButton8
        Switch(8) = SwitchButton9
        Switch(9) = SwitchButton10
        Switch(10) = SwitchButton11
    End Sub

    Private Sub Button_SYSTEM_SET_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_SYSTEM_SET.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub User_Dialog_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        SQLCON.Close_DataBase()
    End Sub

    Private Sub User_Dialog_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If SQLCON.DataBase_Initialization(SQLCON.DataBase_Data_Souce, SQLCON.DataBase_ID, SQLCON.DataBase_PassWord, SQLCON.DataBase_Catalog_Name, 50, , ) = True Then
            Dim_Switch()
            SQLCON.Read_User_Parameter(DataGridViewX1)
        End If
    End Sub

    Private Sub DataGridViewX1_CellClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DataGridViewX1.CellClick
        Select_Row_Index = e.RowIndex
        Max_Row_Count = DataGridViewX1.RowCount
        If Max_Row_Count >= 0 And Select_Row_Index > -1 Then
            If Select_Row_Index <> Max_Row_Count - 1 Then
                USER_NAME.用户名 = DataGridViewX1.Item(1, e.RowIndex).Value.ToString.Trim
                If USER_NAME.用户名.ToUpper.IndexOf("ADMIN") < 0 Then
                    SQLCON.Read_User_Parameter(USER_NAME.用户名, Switch)
                    TextBox_User_Name.Text = USER_NAME.用户名
                    TextBox_User_Name.Enabled = False
                    For i As Integer = 0 To Switch.Length - 1
                        Switch(i).IsReadOnly = False
                    Next
                    ButtonX1.Text = "更新用户"
                    ButtonX1.Enabled = True
                    ButtonX2.Enabled = True
                    TextBoxX1.Enabled = True
                    TextBoxX2.Enabled = True
                    TextBoxX1.Clear()
                    TextBoxX2.Clear()
                Else
                    SQLCON.Read_User_Parameter(USER_NAME.用户名, Switch)
                    TextBox_User_Name.Text = USER_NAME.用户名
                    TextBox_User_Name.Enabled = False
                    ButtonX1.Text = "禁止更改"
                    ButtonX1.Enabled = False
                    ButtonX2.Enabled = False
                    TextBoxX1.Enabled = False
                    TextBoxX2.Enabled = False
                    TextBoxX1.Clear()
                    TextBoxX2.Clear()
                    For i As Integer = 0 To Switch.Length - 1
                        Switch(i).IsReadOnly = True
                    Next
                End If

            Else
                TextBox_User_Name.Enabled = True
                TextBox_User_Name.Clear()
                ButtonX1.Text = "添加用户"
                For i As Integer = 0 To Switch.Length - 1
                    Switch(i).IsReadOnly = False
                    Switch(i).Value = False
                Next
                ButtonX1.Enabled = True
                ButtonX2.Enabled = False
                TextBoxX1.Enabled = True
                TextBoxX2.Enabled = True
                TextBoxX1.Clear()
                TextBoxX2.Clear()
            End If
        Else
            TextBox_User_Name.Enabled = True
            TextBox_User_Name.Clear()
            ButtonX1.Text = "添加用户"
            For i As Integer = 0 To Switch.Length - 1
                Switch(i).IsReadOnly = False
                Switch(i).Value = False
            Next
            ButtonX1.Enabled = True
            ButtonX2.Enabled = False
            TextBoxX1.Enabled = True
            TextBoxX2.Enabled = True
            TextBoxX1.Clear()
            TextBoxX2.Clear()
        End If
    End Sub

    Private Sub ButtonX1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonX1.Click
        Select Case ButtonX1.Text
            Case "添加用户"
                If SQLCON.User_Exists(TextBox_User_Name.Text.Trim) <= 0 Then
                    If TextBoxX1.Text.Trim = TextBoxX2.Text.Trim Then
                        If MessageBoxEx.Show("是否添加用户:" & TextBox_User_Name.Text.Trim & "?", "系统消息", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
                            With USER_PARAMETER
                                .坐标添加 = Switch(0).Value
                                .坐标更新 = Switch(1).Value
                                .硬件配置 = Switch(2).Value
                                .功能设置 = Switch(3).Value
                                .参数设置 = Switch(4).Value
                                .程式切换 = Switch(5).Value
                                .手动操作 = Switch(6).Value
                                .单步执行 = Switch(7).Value
                                .增加用户 = Switch(8).Value
                                .删除用户 = Switch(9).Value
                            End With
                            SQLCON.Insert_User(TextBox_User_Name.Text.Trim, TextBoxX1.Text.Trim, USER_PARAMETER)
                            SQLCON.Read_User_Parameter(DataGridViewX1)
                        End If
                    Else
                        MessageBoxEx.Show("2次密码输入有误！请重新输入！", "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
                        TextBoxX1.Clear()
                        TextBoxX2.Clear()
                    End If
                Else
                    MessageBoxEx.Show("用户名:[" & TextBox_User_Name.Text.Trim & "]已经存在！", "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1)
                    TextBox_User_Name.Clear()
                    TextBoxX1.Clear()
                    TextBoxX2.Clear()
                End If
            Case "更新用户"
                If TextBoxX1.Text.Trim = TextBoxX2.Text.Trim Then
                    If MessageBoxEx.Show("是否更新用户:" & USER_NAME.用户名 & "相关参数？", "系统消息", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
                        With USER_PARAMETER
                            .坐标添加 = Switch(0).Value
                            .坐标更新 = Switch(1).Value
                            .硬件配置 = Switch(2).Value
                            .功能设置 = Switch(3).Value
                            .参数设置 = Switch(4).Value
                            .程式切换 = Switch(5).Value
                            .手动操作 = Switch(6).Value
                            .单步执行 = Switch(7).Value
                            .增加用户 = Switch(8).Value
                            .删除用户 = Switch(9).Value
                            .点胶设置 = Switch(10).Value
                        End With
                        If TextBoxX1.Text.Trim.Length + TextBoxX2.Text.Trim.Length = 0 Then
                            SQLCON.Update_User_Parameter(USER_NAME.用户名, USER_PARAMETER)
                        Else
                            SQLCON.Update_User_Parameter(USER_NAME.用户名, TextBoxX1.Text.Trim, USER_PARAMETER)
                        End If
                        SQLCON.Read_User_Parameter(DataGridViewX1)
                    End If
                Else
                    MessageBoxEx.Show("2次密码输入有误！请重新输入！", "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
                    TextBoxX1.Clear()
                    TextBoxX2.Clear()
                End If
        End Select
    End Sub

    Private Sub ButtonX2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonX2.Click
        If MessageBoxEx.Show("是否删除：" & USER_NAME.用户名 & "用户？", "系统消息", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
            SQLCON.Delete_User(USER_NAME.用户名)
            SQLCON.Read_User_Parameter(DataGridViewX1)
        End If
    End Sub
End Class

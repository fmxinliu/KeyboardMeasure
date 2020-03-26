Imports System.Windows.Forms
Imports DevComponents.DotNetBar

Public Class User_Password_Dialog
    Public SQLCON As New SQL_LIB
    Dim _Password_Type As String = Nothing
    Dim rtn As Boolean = False, Public_PassWord As String = Nothing
    Private Sub Button_SYSTEM_SET_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Cancel.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub User_Password_Dialog_FormClosing(sender As Object, e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        SQLCON.Close_DataBase()
    End Sub

    Private Sub User_Password_Dialog_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        USER_NAME.用户名 = User_Login_Dialog.User
        TextBoxX1.Text = ""
        TextBoxX2.Text = ""
        TextBoxX3.Text = ""
        ComboBox_Password_type.SelectedIndex = 0
        If SQLCON.DataBase_Initialization(SQLCON.DataBase_Data_Souce, SQLCON.DataBase_ID, SQLCON.DataBase_PassWord, SQLCON.DataBase_Catalog_Name, 50, , ) = True Then
            SQLCON.Read_User_Password(USER_NAME.用户名, USER_NAME.登陆密码, USER_NAME.清空数据密码, USER_NAME.删除项目密码)
        End If
    End Sub

    Private Sub ButtonX2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonX2.Click
        Select Case ComboBox_Password_type.SelectedItem.ToString.Trim
            Case "登陆密码"
                _Password_Type = "登陆密码"
                Public_PassWord = USER_NAME.登陆密码
            Case "清空数据密码"
                _Password_Type = "清空数据密码"
                Public_PassWord = USER_NAME.清空数据密码
            Case "删除项目密码"
                _Password_Type = "删除项目密码"
                Public_PassWord = USER_NAME.删除项目密码
        End Select
        Select Case TextBoxX1.Text.Trim
            Case Public_PassWord
                If TextBoxX2.Text.Trim = TextBoxX3.Text.Trim Then
                    rtn = SQLCON.Update_User_Password(USER_NAME.用户名, _Password_Type, TextBoxX2.Text.Trim)
                    If rtn = True Then
                        MessageBoxEx.Show("[" & _Password_Type & "]修改成功！", "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1)
                        Me.DialogResult = System.Windows.Forms.DialogResult.OK
                        Me.Close()
                    Else
                        MessageBoxEx.Show("[" & _Password_Type & "]修改失败！", "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
                    End If
                Else
                    MessageBoxEx.Show("2次密码输入有误！请重新输入！", "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
                    TextBoxX2.Clear()
                    TextBoxX3.Clear()
                End If
            Case Else
                MessageBoxEx.Show("原始密码错误，请重新输入！", "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1)
                TextBoxX1.Clear()
        End Select
    End Sub
End Class

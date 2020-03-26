Imports System.Windows.Forms

Public Class Login_Dialog
    Dim User_Name, User_Pass, ClearDataPass, DeleteProductPass As String
    Dim SQLCON As New GeneralClass.SQL_LIB

    Private Sub ButtonX5_Click(sender As Object, e As EventArgs) Handles ButtonX5.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.No
        Select Case TextBox_Password.Text
            Case User_Pass '登陆密码
                Me.DialogResult = System.Windows.Forms.DialogResult.OK
            Case ClearDataPass '清除数据密码
                Me.DialogResult = System.Windows.Forms.DialogResult.Yes
            Case DeleteProductPass '删除项目号密码
                Me.DialogResult = System.Windows.Forms.DialogResult.Retry
            Case Else
                Me.DialogResult = System.Windows.Forms.DialogResult.No
        End Select
        SQLCON.Close_DataBase()
        Me.Close()
    End Sub

    Private Sub ButtonX1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonX1.Click
        SQLCON.Close_DataBase()
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub Login_Dialog_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        SQLCON.Close_DataBase()
    End Sub

    Private Sub Login_Dialog_KeyUp(sender As Object, e As KeyEventArgs) Handles Me.KeyUp
        Select Case e.KeyCode
            Case Keys.Enter
                Select Case TextBox_Password.Text
                    Case User_Pass
                        Me.DialogResult = System.Windows.Forms.DialogResult.OK
                    Case ClearDataPass
                        Me.DialogResult = System.Windows.Forms.DialogResult.Yes
                    Case DeleteProductPass
                        Me.DialogResult = System.Windows.Forms.DialogResult.Retry
                    Case Else
                        Me.DialogResult = System.Windows.Forms.DialogResult.No
                End Select
                SQLCON.Close_DataBase()
                Me.Close()
            Case Keys.Cancel
                Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
                SQLCON.Close_DataBase()
                Me.Close()
        End Select

    End Sub

    Private Sub Login_Dialog_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
        User_Name = Main.User_Name
        If SQLCON.DataBase_Initialization(SQLCON.DataBase_Data_Souce, SQLCON.DataBase_ID, SQLCON.DataBase_PassWord, SQLCON.DataBase_Catalog_Name, 50, , ) = True Then
            SQLCON.Read_User_Password(User_Name, User_Pass, ClearDataPass, DeleteProductPass)
            TextBox_Password.Clear()
        End If
    End Sub
End Class

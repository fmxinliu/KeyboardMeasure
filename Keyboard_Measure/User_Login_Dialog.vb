Imports System.Windows.Forms
Imports DevComponents.DotNetBar

Public Class User_Login_Dialog

    Dim SQLCON As New GeneralClass.SQL_LIB
    Dim User_Name(), User_Pass(), PassWord As String
    Dim USER_PARAMETER() As SQL_LIB.USER_PARAMETER_STRUCTURE
    Public User As String

    Private Sub ButtonX1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonX1.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Application.Exit()
    End Sub
    Private Sub ButtonX5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonX5.Click
        If PassWord = TextBoxX1.Text Then
            If User.ToString.ToUpper = "ADMIN" Then
                Me.DialogResult = System.Windows.Forms.DialogResult.Yes 'ADMIN权限，优先级最高
            Else
                Me.DialogResult = System.Windows.Forms.DialogResult.OK '其他权限
            End If
            Me.Close()
        Else
            MessageBoxEx.Show("密码输入错误！请重新输入！", "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
            TextBoxX1.Clear()
            Exit Sub
        End If
    End Sub

    Private Sub User_Login_Dialog_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        SQLCON.Close_DataBase()
    End Sub

    Private Sub User_Login_Dialog_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Enter Then
            If PassWord = TextBoxX1.Text Then
                If User.ToString.ToUpper = "ADMIN" Then
                    Me.DialogResult = System.Windows.Forms.DialogResult.Yes 'ADMIN才有权限修改
                Else
                    Me.DialogResult = System.Windows.Forms.DialogResult.OK
                End If
                Me.Close()
            Else
                MessageBoxEx.Show("密码输入错误！请重新输入！", "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
                TextBoxX1.Clear()
                Exit Sub
            End If
        End If
        If e.KeyCode = Keys.Escape Then
            Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Application.Exit()
        End If
    End Sub

    Private Sub User_Login_Dialog_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If SQLCON.DataBase_Initialization(SQLCON.DataBase_Data_Souce, SQLCON.DataBase_ID, SQLCON.DataBase_PassWord, SQLCON.DataBase_Catalog_Name, 50, , ) = True Then
            SQLCON.Read_User_Parameter(User_Name, ComboBoxEx1)
            ComboBoxEx1.SelectedIndex = 0
            TextBoxX1.Clear()
            TextBoxX1.Focus()
            TextBoxX1.Text = "111"
            If IO.File.Exists(IO.Directory.GetCurrentDirectory & "\Temp_UpdateSoft.exe") = True Then
                IO.File.Replace(IO.Directory.GetCurrentDirectory & "\Temp_UpdateSoft.exe", IO.Directory.GetCurrentDirectory & "\UpdateSoft.exe", IO.Directory.GetCurrentDirectory & "\UpdateSoft.bak")
            End If
        End If
    End Sub

    Private Sub ComboBoxEx1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBoxEx1.SelectedIndexChanged
        If ComboBoxEx1.SelectedIndex >= 0 Then
            User = ComboBoxEx1.SelectedItem.ToString.Trim
            SQLCON.Read_User_Password(User, PassWord, Nothing, Nothing)
        Else
            User = ""
        End If
    End Sub
End Class

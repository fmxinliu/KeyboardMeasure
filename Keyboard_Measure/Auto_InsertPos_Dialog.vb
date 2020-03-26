Imports DevComponents.DotNetBar

Public Class Auto_InsertPos_Dialog

    Dim Project_Name As String = Nothing
    Dim Coordinate_name As String = Nothing
    Dim PARAMETERS_NAME_ARRAY() As String = Nothing, PARAMETERS_VALUE_ARRAY() As Object = Nothing



    Private Sub Btn_Ok_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Btn_Ok.Click
        If ComboBoxEx_X.SelectedIndex < 0 Then
            MessageBoxEx.Show("没有选中[行轴]，无法自动生成坐标！", "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If
        If ComboBoxEx_Y.SelectedIndex < 0 Then
            MessageBoxEx.Show("没有选中[列轴]，无法自动生成坐标！", "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If
        AutoInsertPosParam.行轴名称 = ComboBoxEx_X.SelectedItem.ToString.Trim
        AutoInsertPosParam.列轴名称 = ComboBoxEx_Y.SelectedItem.ToString.Trim
        AutoInsertPosParam.行轴 = ComboBoxEx_X.SelectedIndex
        AutoInsertPosParam.列轴 = ComboBoxEx_Y.SelectedIndex
        AutoInsertPosParam.行数 = X_Index.Value
        AutoInsertPosParam.列数 = Y_Index.Value
        AutoInsertPosParam.行间距 = X_LEN.Value
        AutoInsertPosParam.列间距 = Y_LEN.Value
        Me.DialogResult = Windows.Forms.DialogResult.Yes
        Me.Close()
    End Sub

    Private Sub Btn_Cancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Btn_Cancel.Click
        Me.DialogResult = Windows.Forms.DialogResult.No
        Me.Close()
    End Sub

    Private Sub Auto_InsertPos_Dialog_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Project_Name = System_SetDialog.Select_Product_Name
        Coordinate_name = System_SetDialog.Select_Coordinate_Name

        ComboBoxEx_X.Items.Clear()
        ComboBoxEx_Y.Items.Clear()
        ComboBoxEx_X.Text = ""
        ComboBoxEx_Y.Text = ""

        Dim Axis_Name As String = Nothing, Axis_Index As String = Nothing
        If Main.SQLCON.Cheng_Query_Coordinate_AxisSystem(Project_Name, Coordinate_name, Axis_Name, Axis_Index) = True Then
            PARAMETERS_NAME_ARRAY = Split(Axis_Name, ";")
            If PARAMETERS_NAME_ARRAY.Length >= 1 Then
                For i As Integer = 0 To PARAMETERS_NAME_ARRAY.Length - 1
                    ComboBoxEx_X.Items.Add(PARAMETERS_NAME_ARRAY(i))
                    ComboBoxEx_Y.Items.Add(PARAMETERS_NAME_ARRAY(i))
                Next
            Else
                MessageBoxEx.Show("项目名称为：" & Project_Name & " 坐标名称为：" & Coordinate_name & " 的轴数必须>=1，否则无法自动生成坐标！", "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        Else
            MessageBoxEx.Show("项目名称为：" & Project_Name & " 坐标名称为：" & Coordinate_name & " 的坐标系参数不存在，自动生成坐标！", "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

    End Sub
End Class
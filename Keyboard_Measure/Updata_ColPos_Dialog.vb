Imports DevComponents.DotNetBar

Public Class Updata_ColPos_Dialog
    Dim Project_Name As String = Nothing
    Dim Coordinate_name As String = Nothing
    Dim PARAMETERS_NAME_ARRAY() As String = Nothing, PARAMETERS_VALUE_ARRAY() As Object = Nothing

    Private Sub DX_Add_Axis_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DX_Add_Axis.CellContentClick

    End Sub

    Private Sub SingleColPos_Dialog_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Project_Name = System_SetDialog.Select_Product_Name
        Coordinate_name = System_SetDialog.Select_Coordinate_Name

        Dim Axis_Name As String = Nothing, Axis_Index As String = Nothing
        If Main.SQLCON.Cheng_Query_Coordinate_AxisSystem(Project_Name, Coordinate_name, Axis_Name, Axis_Index) = True Then
            PARAMETERS_NAME_ARRAY = Split(Axis_Name, ";")
        Else
            MessageBoxEx.Show("项目名称为：" & Project_Name & " 坐标名称为：" & Coordinate_name & " 的坐标系参数不存在，无法统一列坐标！", "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If


        DX_Add_Axis.Rows.Clear()
        For i As Int16 = 0 To PARAMETERS_NAME_ARRAY.Length - 1
            DX_Add_Axis.Rows.Add()
            DX_Add_Axis.Rows(i).Cells(0).Value = PARAMETERS_NAME_ARRAY(i)
            DX_Add_Axis.Rows(i).Cells(1).Value = 0
            DX_Add_Axis.Rows(i).Cells(2).Value = False
        Next

        DX_Add_Axis.RowHeadersVisible = False
        DX_Add_Axis.AllowUserToDeleteRows = False
        DX_Add_Axis.AllowUserToResizeColumns = False
        DX_Add_Axis.AllowUserToResizeRows = False
        DX_Add_Axis.ColumnHeadersHeightSizeMode = Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        DX_Add_Axis.MultiSelect = False
        DX_Add_Axis.ReadOnly = False
        DX_Add_Axis.RowHeadersWidthSizeMode = Windows.Forms.DataGridViewRowHeadersWidthSizeMode.EnableResizing
        DX_Add_Axis.SelectionMode = Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        DX_Add_Axis.EditMode = DataGridViewEditMode.EditOnEnter

        DX_Add_Axis.Columns(0).ReadOnly = True '只读
    End Sub

    Private Sub Btn_OK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Btn_OK.Click
        For i As Int16 = 0 To PARAMETERS_NAME_ARRAY.Length - 1
            If DX_Add_Axis.Rows(i).Cells(2).Value = True Then
                Dim Col_Name As String = CType(DX_Add_Axis.Rows(i).Cells(0).Value, String).Trim
                Dim Col_Value As Double = CType(DX_Add_Axis.Rows(i).Cells(1).Value, Double)
                If Main.SQLCON.Update_Column_Pos(Project_Name, Coordinate_name, Col_Name, Col_Value) = False Then
                    MessageBoxEx.Show("更新列【" & Col_Name & "】出错！", "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Exit For
                End If
            End If
        Next
        Me.DialogResult = Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Btn_CANCEL_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Btn_CANCEL.Click
        Me.DialogResult = Windows.Forms.DialogResult.No
        Me.Close()
    End Sub
End Class
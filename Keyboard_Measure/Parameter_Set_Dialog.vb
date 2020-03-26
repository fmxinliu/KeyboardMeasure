Imports DevComponents.DotNetBar
Imports Keyboard_Measure.Device_Parameter_Dialog
Imports Keyboard_Measure.GeneralClass.ADLINK_LIB.MOTION

Public Class Parameter_Set_Dialog

    Public SQLCON As New SQL_LIB
    Dim Col_Names() As String = Nothing
    Dim SELECT_COORDINATES_ROW_INDEX As Int16 = 0, F_ID As Int16 = 0
    Dim CARDCMD As New ADLINK_LIB.MOTION, BOARD_ID_00 As Int16 = 0, BOARD_ID_01 As Int16 = 1
    Public Enum Param_Model_Enum
        Double_Value = 1
        Int_Value = 2
        String_Value = 3
        Bool_Value = 4
    End Enum
    Dim Param_Model As Param_Model_Enum

    Private Sub Parameter_Set_Dialog_FormClosing(sender As Object, e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        SQLCON.Close_DataBase()
    End Sub

    Private Sub Parameter_Set_Dialog_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        If SQLCON.DataBase_Initialization(SQLCON.DataBase_Data_Souce, SQLCON.DataBase_ID, SQLCON.DataBase_PassWord, SQLCON.DataBase_Catalog_Name, 50, , ) = True Then
            SQLCON.Read_Device_Parameter(DEVICEPARAM_TABLENAME.DOUBLE_TYPE, DataGrid_PARAM_DOUBLE)
            SQLCON.Read_Device_Parameter(DEVICEPARAM_TABLENAME.INTEGER_TYPE, DataGrid_PARAM_INT)
            SQLCON.Read_Device_Parameter(DEVICEPARAM_TABLENAME.STRING_TYPE, DataGrid_PARAM_STR)
            SQLCON.Read_Device_Parameter(DEVICEPARAM_TABLENAME.BOOL_TYPE, DataGrid_PARAM_BOOL)

            Select Case Main.User_Name.ToUpper
                Case UserName.Admin
                    新增参数名ToolStripMenuItem.Enabled = True
                    删除当前参数名ToolStripMenuItem.Enabled = True
                    清空所有参数名ToolStripMenuItem.Enabled = True
                Case Else
                    新增参数名ToolStripMenuItem.Enabled = False
                    删除当前参数名ToolStripMenuItem.Enabled = False
                    清空所有参数名ToolStripMenuItem.Enabled = False
            End Select

        End If
        If Main.BOOL.Init_DIO = False Then
            SuperTabItem_IO.Enabled = False
        Else
            SuperTabItem_IO.Enabled = True
        End If
    End Sub

    Private Sub 新增参数名ToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles 新增参数名ToolStripMenuItem.Click
        Select Case Param_Model
            Case Param_Model_Enum.Double_Value
                Dim Dlg As New Device_Parameter_Dialog(Model_Enum.add, Param_Model_Enum.Double_Value)
                If Dlg.ShowDialog = Windows.Forms.DialogResult.OK Then
                    SQLCON.Insert_Device_Param_Double(Device_ParamType.变量名称, Device_ParamType.Double_当前值, Device_ParamType.Double_最小值, Device_ParamType.Double_最大值)
                    SQLCON.Read_Device_Parameter(DEVICEPARAM_TABLENAME.DOUBLE_TYPE, DataGrid_PARAM_DOUBLE)
                End If
            Case Param_Model_Enum.Int_Value
                Dim Dlg As New Device_Parameter_Dialog(Model_Enum.add, Param_Model_Enum.Int_Value)
                If Dlg.ShowDialog = Windows.Forms.DialogResult.OK Then
                    SQLCON.Insert_Device_Param_Integer(Device_ParamType.变量名称, Device_ParamType.Integer_当前值, Device_ParamType.Integer_最小值, Device_ParamType.Integer_最大值)
                    SQLCON.Read_Device_Parameter(DEVICEPARAM_TABLENAME.INTEGER_TYPE, DataGrid_PARAM_INT)
                End If
            Case Param_Model_Enum.String_Value
                Dim Dlg As New Device_Parameter_Dialog(Model_Enum.add, Param_Model_Enum.String_Value)
                If Dlg.ShowDialog = Windows.Forms.DialogResult.OK Then
                    SQLCON.Insert_Device_Param_String(Device_ParamType.变量名称, Device_ParamType.String_当前值)
                    SQLCON.Read_Device_Parameter(DEVICEPARAM_TABLENAME.STRING_TYPE, DataGrid_PARAM_STR)
                End If
            Case Param_Model_Enum.Bool_Value
                Dim Dlg As New Device_Parameter_Dialog(Model_Enum.add, Param_Model_Enum.Bool_Value)
                If Dlg.ShowDialog = Windows.Forms.DialogResult.OK Then
                    SQLCON.Insert_Device_Param_Bool(Device_ParamType.变量名称, Device_ParamType.Bool_当前值)
                    SQLCON.Read_Device_Parameter(DEVICEPARAM_TABLENAME.BOOL_TYPE, DataGrid_PARAM_BOOL)
                End If
        End Select
    End Sub


    Private Sub 刷新ToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles 刷新ToolStripMenuItem.Click
        SQLCON.Read_Device_Parameter(DEVICEPARAM_TABLENAME.DOUBLE_TYPE, DataGrid_PARAM_DOUBLE)
        SQLCON.Read_Device_Parameter(DEVICEPARAM_TABLENAME.INTEGER_TYPE, DataGrid_PARAM_INT)
        SQLCON.Read_Device_Parameter(DEVICEPARAM_TABLENAME.STRING_TYPE, DataGrid_PARAM_STR)
        SQLCON.Read_Device_Parameter(DEVICEPARAM_TABLENAME.BOOL_TYPE, DataGrid_PARAM_BOOL)
    End Sub

    Private Sub 更新参数名ToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles 更新参数名ToolStripMenuItem.Click
        Select Case Param_Model
            Case Param_Model_Enum.Double_Value
                Dim Dlg As New Device_Parameter_Dialog(Model_Enum.updata, Param_Model_Enum.Double_Value)
                If Dlg.ShowDialog = Windows.Forms.DialogResult.OK Then
                    SQLCON.Updata_Device_Param_Double(Device_ParamType.变量名称, Device_ParamType.Double_当前值, Device_ParamType.Double_最小值, Device_ParamType.Double_最大值, F_ID)
                    SQLCON.Read_Device_Parameter(DEVICEPARAM_TABLENAME.DOUBLE_TYPE, DataGrid_PARAM_DOUBLE)

                    Try
                        DataGrid_PARAM_DOUBLE.FirstDisplayedScrollingRowIndex = SELECT_COORDINATES_ROW_INDEX
                        DataGrid_PARAM_DOUBLE.Rows(SELECT_COORDINATES_ROW_INDEX).Selected = True
                    Catch
                    End Try
                End If
            Case Param_Model_Enum.Int_Value
                Dim Dlg As New Device_Parameter_Dialog(Model_Enum.updata, Param_Model_Enum.Int_Value)
                If Dlg.ShowDialog = Windows.Forms.DialogResult.OK Then
                    SQLCON.Updata_Device_Param_Integer(Device_ParamType.变量名称, Device_ParamType.Integer_当前值, Device_ParamType.Integer_最小值, Device_ParamType.Integer_最大值, F_ID)
                    SQLCON.Read_Device_Parameter(DEVICEPARAM_TABLENAME.INTEGER_TYPE, DataGrid_PARAM_INT)

                    Try
                        DataGrid_PARAM_INT.FirstDisplayedScrollingRowIndex = SELECT_COORDINATES_ROW_INDEX
                        DataGrid_PARAM_INT.Rows(SELECT_COORDINATES_ROW_INDEX).Selected = True
                    Catch
                    End Try
                End If
            Case Param_Model_Enum.String_Value
                Dim Dlg As New Device_Parameter_Dialog(Model_Enum.updata, Param_Model_Enum.String_Value)
                If Dlg.ShowDialog = Windows.Forms.DialogResult.OK Then
                    SQLCON.Updata_Device_Param_String(Device_ParamType.变量名称, Device_ParamType.String_当前值, F_ID)
                    SQLCON.Read_Device_Parameter(DEVICEPARAM_TABLENAME.STRING_TYPE, DataGrid_PARAM_STR)
                    Try
                        DataGrid_PARAM_STR.FirstDisplayedScrollingRowIndex = SELECT_COORDINATES_ROW_INDEX
                        DataGrid_PARAM_STR.Rows(SELECT_COORDINATES_ROW_INDEX).Selected = True
                    Catch
                    End Try
                End If
            Case Param_Model_Enum.Bool_Value
                Dim Dlg As New Device_Parameter_Dialog(Model_Enum.updata, Param_Model_Enum.Bool_Value)
                If Dlg.ShowDialog = Windows.Forms.DialogResult.OK Then
                    SQLCON.Updata_Device_Param_Bool(Device_ParamType.变量名称, Device_ParamType.Bool_当前值, F_ID)
                    SQLCON.Read_Device_Parameter(DEVICEPARAM_TABLENAME.BOOL_TYPE, DataGrid_PARAM_BOOL)
                    Try
                        DataGrid_PARAM_BOOL.FirstDisplayedScrollingRowIndex = SELECT_COORDINATES_ROW_INDEX
                        DataGrid_PARAM_BOOL.Rows(SELECT_COORDINATES_ROW_INDEX).Selected = True
                    Catch
                    End Try
                End If
        End Select

        SQLCON.RedData_NG键帽个数()
        Main.NGCOUNT.NG_Max_Count = NG键帽个数
        Main.NumericUpDown1.Value = Main.NGCOUNT.NG_Max_Count


    End Sub


    Private Sub 清空所有参数名ToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles 清空所有参数名ToolStripMenuItem.Click
        Select Case Param_Model
            Case Param_Model_Enum.Double_Value
                If MessageBoxEx.Show("是否清空所有变量名？【YES】确定，【NO】取消", "系统消息", MessageBoxButtons.YesNo) = Windows.Forms.DialogResult.Yes Then
                    SQLCON.Delete_Param_ALL(DEVICEPARAM_TABLENAME.DOUBLE_TYPE)
                    SQLCON.Read_Device_Parameter(DEVICEPARAM_TABLENAME.DOUBLE_TYPE, DataGrid_PARAM_DOUBLE)
                End If
            Case Param_Model_Enum.Int_Value
                If MessageBoxEx.Show("是否清空所有变量名？【YES】确定，【NO】取消", "系统消息", MessageBoxButtons.YesNo) = Windows.Forms.DialogResult.Yes Then
                    SQLCON.Delete_Param_ALL(DEVICEPARAM_TABLENAME.INTEGER_TYPE)
                    SQLCON.Read_Device_Parameter(DEVICEPARAM_TABLENAME.INTEGER_TYPE, DataGrid_PARAM_INT)
                End If
            Case Param_Model_Enum.String_Value
                If MessageBoxEx.Show("是否清空所有变量名？【YES】确定，【NO】取消", "系统消息", MessageBoxButtons.YesNo) = Windows.Forms.DialogResult.Yes Then
                    SQLCON.Delete_Param_ALL(DEVICEPARAM_TABLENAME.STRING_TYPE)
                    SQLCON.Read_Device_Parameter(DEVICEPARAM_TABLENAME.STRING_TYPE, DataGrid_PARAM_STR)
                End If
            Case Param_Model_Enum.Bool_Value
                If MessageBoxEx.Show("是否清空所有变量名？【YES】确定，【NO】取消", "系统消息", MessageBoxButtons.YesNo) = Windows.Forms.DialogResult.Yes Then
                    SQLCON.Delete_Param_ALL(DEVICEPARAM_TABLENAME.BOOL_TYPE)
                    SQLCON.Read_Device_Parameter(DEVICEPARAM_TABLENAME.BOOL_TYPE, DataGrid_PARAM_BOOL)
                End If
        End Select
    End Sub

    Private Sub 删除当前参数名ToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles 删除当前参数名ToolStripMenuItem.Click
        Select Case Param_Model
            Case Param_Model_Enum.Double_Value
                If MessageBoxEx.Show("是否确定删除ID为" & F_ID & "的变量名？【YES】确定，【NO】取消", "系统消息", MessageBoxButtons.YesNo) = Windows.Forms.DialogResult.Yes Then
                    SQLCON.Delete_Param_ID(DEVICEPARAM_TABLENAME.DOUBLE_TYPE, F_ID)
                    SQLCON.Read_Device_Parameter(DEVICEPARAM_TABLENAME.DOUBLE_TYPE, DataGrid_PARAM_DOUBLE)
                End If
            Case Param_Model_Enum.Int_Value
                If MessageBoxEx.Show("是否确定删除ID为" & F_ID & "的变量名？【YES】确定，【NO】取消", "系统消息", MessageBoxButtons.YesNo) = Windows.Forms.DialogResult.Yes Then
                    SQLCON.Delete_Param_ID(DEVICEPARAM_TABLENAME.INTEGER_TYPE, F_ID)
                    SQLCON.Read_Device_Parameter(DEVICEPARAM_TABLENAME.INTEGER_TYPE, DataGrid_PARAM_INT)
                End If
            Case Param_Model_Enum.String_Value
                If MessageBoxEx.Show("是否确定删除ID为" & F_ID & "的变量名？【YES】确定，【NO】取消", "系统消息", MessageBoxButtons.YesNo) = Windows.Forms.DialogResult.Yes Then
                    SQLCON.Delete_Param_ID(DEVICEPARAM_TABLENAME.STRING_TYPE, F_ID)
                    SQLCON.Read_Device_Parameter(DEVICEPARAM_TABLENAME.STRING_TYPE, DataGrid_PARAM_STR)
                End If
            Case Param_Model_Enum.Bool_Value
                If MessageBoxEx.Show("是否确定删除ID为" & F_ID & "的变量名？【YES】确定，【NO】取消", "系统消息", MessageBoxButtons.YesNo) = Windows.Forms.DialogResult.Yes Then
                    SQLCON.Delete_Param_ID(DEVICEPARAM_TABLENAME.BOOL_TYPE, F_ID)
                    SQLCON.Read_Device_Parameter(DEVICEPARAM_TABLENAME.BOOL_TYPE, DataGrid_PARAM_BOOL)
                End If
        End Select
    End Sub

  

    Private Sub SwitchButton11_ValueChanged(sender As System.Object, e As System.EventArgs) Handles SwitchButton11.ValueChanged
        Select Case SwitchButton11.Value
            Case True
                CARDCMD.SET_DO_BIT(BOARD_ID_00, Main.DIO.Z中线阻挡电磁阀_0, PRA_OUT_STATUS_ENUM.OUT_ON)
            Case False
                CARDCMD.SET_DO_BIT(BOARD_ID_00, Main.DIO.Z中线阻挡电磁阀_0, PRA_OUT_STATUS_ENUM.OUT_OFF)
        End Select
    End Sub

    Private Sub SwitchButton2_ValueChanged(sender As System.Object, e As System.EventArgs) Handles SwitchButton2.ValueChanged
        Select Case SwitchButton2.Value
            Case True
                CARDCMD.SET_DO_BIT(BOARD_ID_00, Main.DIO.ONE1工位阻挡电磁阀1_0, PRA_OUT_STATUS_ENUM.OUT_ON)
            Case False
                CARDCMD.SET_DO_BIT(BOARD_ID_00, Main.DIO.ONE1工位阻挡电磁阀1_0, PRA_OUT_STATUS_ENUM.OUT_OFF)
        End Select
    End Sub

    Private Sub SwitchButton3_ValueChanged(sender As System.Object, e As System.EventArgs) Handles SwitchButton3.ValueChanged
        Select Case SwitchButton3.Value
            Case True
                CARDCMD.SET_DO_BIT(BOARD_ID_00, Main.DIO.ONE1工位阻挡电磁阀2_0, PRA_OUT_STATUS_ENUM.OUT_ON)
            Case False
                CARDCMD.SET_DO_BIT(BOARD_ID_00, Main.DIO.ONE1工位阻挡电磁阀2_0, PRA_OUT_STATUS_ENUM.OUT_OFF)
        End Select
    End Sub

    Private Sub SwitchButton4_ValueChanged(sender As System.Object, e As System.EventArgs) Handles SwitchButton4.ValueChanged
        Select Case SwitchButton4.Value
            Case True
                CARDCMD.SET_DO_BIT(BOARD_ID_00, Main.DIO.TWO2工位阻挡电磁阀1_0, PRA_OUT_STATUS_ENUM.OUT_ON)
            Case False
                CARDCMD.SET_DO_BIT(BOARD_ID_00, Main.DIO.TWO2工位阻挡电磁阀1_0, PRA_OUT_STATUS_ENUM.OUT_OFF)
        End Select
    End Sub

    Private Sub SwitchButton5_ValueChanged(sender As System.Object, e As System.EventArgs) Handles SwitchButton5.ValueChanged
        Select Case SwitchButton5.Value
            Case True
                CARDCMD.SET_DO_BIT(BOARD_ID_00, Main.DIO.TWO2工位阻挡电磁阀2_0, PRA_OUT_STATUS_ENUM.OUT_ON)
            Case False
                CARDCMD.SET_DO_BIT(BOARD_ID_00, Main.DIO.TWO2工位阻挡电磁阀2_0, PRA_OUT_STATUS_ENUM.OUT_OFF)
        End Select
    End Sub

    Private Sub SwitchButton6_ValueChanged(sender As System.Object, e As System.EventArgs) Handles SwitchButton6.ValueChanged
        Select Case SwitchButton6.Value
            Case True
                CARDCMD.SET_DO_BIT(BOARD_ID_00, Main.DIO.ONE1工位顶升电磁阀_0, PRA_OUT_STATUS_ENUM.OUT_ON)
            Case False
                CARDCMD.SET_DO_BIT(BOARD_ID_00, Main.DIO.ONE1工位顶升电磁阀_0, PRA_OUT_STATUS_ENUM.OUT_OFF)
        End Select
    End Sub

    Private Sub SwitchButton7_ValueChanged(sender As System.Object, e As System.EventArgs) Handles SwitchButton7.ValueChanged
        Select Case SwitchButton7.Value
            Case True
                CARDCMD.SET_DO_BIT(BOARD_ID_00, Main.DIO.TWO2工位顶升电磁阀_0, PRA_OUT_STATUS_ENUM.OUT_ON)
            Case False
                CARDCMD.SET_DO_BIT(BOARD_ID_00, Main.DIO.TWO2工位顶升电磁阀_0, PRA_OUT_STATUS_ENUM.OUT_OFF)
        End Select
    End Sub

    Private Sub SwitchButton9_ValueChanged(sender As System.Object, e As System.EventArgs) Handles SwitchButton9.ValueChanged
        Select Case SwitchButton9.Value
            Case True
                CARDCMD.SET_DO_BIT(BOARD_ID_00, Main.DIO.ONE1工位真空吸电磁阀_0, PRA_OUT_STATUS_ENUM.OUT_ON)
            Case False
                CARDCMD.SET_DO_BIT(BOARD_ID_00, Main.DIO.ONE1工位真空吸电磁阀_0, PRA_OUT_STATUS_ENUM.OUT_OFF)
        End Select
    End Sub

    Private Sub SwitchButton10_ValueChanged(sender As System.Object, e As System.EventArgs) Handles SwitchButton10.ValueChanged
        Select Case SwitchButton10.Value
            Case True
                CARDCMD.SET_DO_BIT(BOARD_ID_00, Main.DIO.TWO2工位真空吸电磁阀_0, PRA_OUT_STATUS_ENUM.OUT_ON)
            Case False
                CARDCMD.SET_DO_BIT(BOARD_ID_00, Main.DIO.TWO2工位真空吸电磁阀_0, PRA_OUT_STATUS_ENUM.OUT_OFF)
        End Select
    End Sub

    Private Sub SwitchButton8_ValueChanged(sender As System.Object, e As System.EventArgs) Handles SwitchButton8.ValueChanged
        Select Case SwitchButton8.Value
            Case True
                CARDCMD.SET_DO_BIT(BOARD_ID_00, Main.DIO.Z中线真空吸电磁阀_0, PRA_OUT_STATUS_ENUM.OUT_ON)
            Case False
                CARDCMD.SET_DO_BIT(BOARD_ID_00, Main.DIO.Z中线真空吸电磁阀_0, PRA_OUT_STATUS_ENUM.OUT_OFF)
        End Select
    End Sub

    Private Sub SwitchButton12_ValueChanged(sender As System.Object, e As System.EventArgs) Handles SwitchButton12.ValueChanged
        Select Case SwitchButton12.Value
            Case True
                CARDCMD.SET_DO_BIT(BOARD_ID_00, Main.DIO.ONE1工位皮带控制_0, PRA_OUT_STATUS_ENUM.OUT_ON)
            Case False
                CARDCMD.SET_DO_BIT(BOARD_ID_00, Main.DIO.ONE1工位皮带控制_0, PRA_OUT_STATUS_ENUM.OUT_OFF)
        End Select
    End Sub

    Private Sub SwitchButton13_ValueChanged(sender As System.Object, e As System.EventArgs) Handles SwitchButton13.ValueChanged
        Select Case SwitchButton13.Value
            Case True
                CARDCMD.SET_DO_BIT(BOARD_ID_00, Main.DIO.TWO2工位皮带控制_0, PRA_OUT_STATUS_ENUM.OUT_ON)
            Case False
                CARDCMD.SET_DO_BIT(BOARD_ID_00, Main.DIO.TWO2工位皮带控制_0, PRA_OUT_STATUS_ENUM.OUT_OFF)
        End Select
    End Sub

    Private Sub SwitchButton1_ValueChanged(sender As System.Object, e As System.EventArgs) Handles SwitchButton1.ValueChanged
        Select Case SwitchButton1.Value
            Case True
                CARDCMD.SET_DO_BIT(BOARD_ID_00, Main.DIO.Z中线皮带控制_0, PRA_OUT_STATUS_ENUM.OUT_ON)
            Case False
                CARDCMD.SET_DO_BIT(BOARD_ID_00, Main.DIO.Z中线皮带控制_0, PRA_OUT_STATUS_ENUM.OUT_OFF)
        End Select
    End Sub

    Private Sub SwitchButton14_ValueChanged(sender As System.Object, e As System.EventArgs) Handles SwitchButton14.ValueChanged
        Select Case SwitchButton14.Value
            Case True
                CARDCMD.SET_DO_BIT(BOARD_ID_00, Main.DIO.H回流皮带控制_0, PRA_OUT_STATUS_ENUM.OUT_ON)
            Case False
                CARDCMD.SET_DO_BIT(BOARD_ID_00, Main.DIO.H回流皮带控制_0, PRA_OUT_STATUS_ENUM.OUT_OFF)
        End Select
    End Sub

    Private Sub ButtonX1_Click(sender As System.Object, e As System.EventArgs) Handles ButtonX1.Click
        Me.Close()
    End Sub

    Private Sub SwitchButton15_ValueChanged(sender As System.Object, e As System.EventArgs) Handles SwitchButton15.ValueChanged
        Select Case SwitchButton15.Value
            Case True
                CARDCMD.SET_DO_BIT(BOARD_ID_00, Main.DIO.ONE1工位破真空电磁阀_0, PRA_OUT_STATUS_ENUM.OUT_ON)
            Case False
                CARDCMD.SET_DO_BIT(BOARD_ID_00, Main.DIO.ONE1工位破真空电磁阀_0, PRA_OUT_STATUS_ENUM.OUT_OFF)
        End Select
    End Sub

    Private Sub SwitchButton16_ValueChanged(sender As System.Object, e As System.EventArgs) Handles SwitchButton16.ValueChanged
        Select Case SwitchButton16.Value
            Case True
                CARDCMD.SET_DO_BIT(BOARD_ID_00, Main.DIO.TWO2工位破真空电磁阀_0, PRA_OUT_STATUS_ENUM.OUT_ON)
            Case False
                CARDCMD.SET_DO_BIT(BOARD_ID_00, Main.DIO.TWO2工位破真空电磁阀_0, PRA_OUT_STATUS_ENUM.OUT_OFF)
        End Select
    End Sub

    Private Sub DataGrid_PARAM_DOUBLE_CellClick_1(sender As System.Object, e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DataGrid_PARAM_DOUBLE.CellClick
        Param_Model = Param_Model_Enum.Double_Value
        If e.RowIndex >= 0 Then
            F_ID = DataGrid_PARAM_DOUBLE.Item(0, e.RowIndex).Value
            Try
                Device_ParamType.变量名称 = DataGrid_PARAM_DOUBLE.Item(1, e.RowIndex).Value.ToString.Trim
                Device_ParamType.Double_当前值 = CType(DataGrid_PARAM_DOUBLE.Item(2, e.RowIndex).Value, Double)
                Device_ParamType.Double_最小值 = CType(DataGrid_PARAM_DOUBLE.Item(3, e.RowIndex).Value, Double)
                Device_ParamType.Double_最大值 = CType(DataGrid_PARAM_DOUBLE.Item(4, e.RowIndex).Value, Double)
            Catch ex As Exception
                MsgBox(ex.ToString)
            End Try
            SELECT_COORDINATES_ROW_INDEX = e.RowIndex
        End If
    End Sub

    Private Sub DataGrid_PARAM_INT_CellClick_1(sender As System.Object, e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DataGrid_PARAM_INT.CellClick
        Param_Model = Param_Model_Enum.Int_Value
        If e.RowIndex >= 0 Then
            F_ID = DataGrid_PARAM_INT.Item(0, e.RowIndex).Value
            Try
                Device_ParamType.变量名称 = DataGrid_PARAM_INT.Item(1, e.RowIndex).Value.ToString.Trim
                Device_ParamType.Integer_当前值 = CType(DataGrid_PARAM_INT.Item(2, e.RowIndex).Value, Integer)
                Device_ParamType.Integer_最小值 = CType(DataGrid_PARAM_INT.Item(3, e.RowIndex).Value, Integer)
                Device_ParamType.Integer_最大值 = CType(DataGrid_PARAM_INT.Item(4, e.RowIndex).Value, Integer)
            Catch ex As Exception
                MsgBox(ex.ToString)
            End Try
            SELECT_COORDINATES_ROW_INDEX = e.RowIndex
        End If
    End Sub

    Private Sub DataGrid_PARAM_BOOL_CellClick_1(sender As System.Object, e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DataGrid_PARAM_BOOL.CellClick
        Param_Model = Param_Model_Enum.Bool_Value
        If e.RowIndex >= 0 Then
            F_ID = DataGrid_PARAM_BOOL.Item(0, e.RowIndex).Value
            Try
                Device_ParamType.变量名称 = DataGrid_PARAM_BOOL.Item(1, e.RowIndex).Value.ToString.Trim
                Device_ParamType.Bool_当前值 = IIf(CType(DataGrid_PARAM_BOOL.Item(2, e.RowIndex).Value, Boolean) = True, 1, 0)
            Catch ex As Exception
                MsgBox(ex.ToString)
            End Try
            SELECT_COORDINATES_ROW_INDEX = e.RowIndex
        End If
    End Sub

    Private Sub DataGrid_PARAM_STR_CellClick_1(sender As System.Object, e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DataGrid_PARAM_STR.CellClick
        Param_Model = Param_Model_Enum.String_Value
        If e.RowIndex >= 0 Then
            F_ID = DataGrid_PARAM_STR.Item(0, e.RowIndex).Value
            Try
                Device_ParamType.变量名称 = DataGrid_PARAM_STR.Item(1, e.RowIndex).Value.ToString.Trim
                Device_ParamType.String_当前值 = DataGrid_PARAM_STR.Item(2, e.RowIndex).Value.ToString.Trim
            Catch ex As Exception
                MsgBox(ex.ToString)
            End Try
            SELECT_COORDINATES_ROW_INDEX = e.RowIndex
        End If
    End Sub

    Private Sub DataGrid_PARAM_DOUBLE_Click_1(sender As System.Object, e As System.EventArgs) Handles DataGrid_PARAM_DOUBLE.Click
        Param_Model = Param_Model_Enum.Double_Value
    End Sub

    Private Sub DataGrid_PARAM_INT_Click_1(sender As System.Object, e As System.EventArgs) Handles DataGrid_PARAM_INT.Click
        Param_Model = Param_Model_Enum.Int_Value
    End Sub

    Private Sub DataGrid_PARAM_BOOL_Click_1(sender As System.Object, e As System.EventArgs) Handles DataGrid_PARAM_BOOL.Click
        Param_Model = Param_Model_Enum.Bool_Value
    End Sub

    Private Sub DataGrid_PARAM_STR_Click_1(sender As System.Object, e As System.EventArgs) Handles DataGrid_PARAM_STR.Click
        Param_Model = Param_Model_Enum.String_Value
    End Sub
End Class
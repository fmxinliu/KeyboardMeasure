Imports System.Math
Imports System.IO
Imports System.Text
Imports System.Threading
Imports Keyboard_Measure.ADLINK_LIB
Imports Keyboard_Measure.ADLINK_LIB.MOTION
Imports Keyboard_Measure.SQL_LIB
Imports DevComponents.DotNetBar

Public Class Home_Dialog
    Dim CARDCMD As New ADLINK_LIB.MOTION
    Public SQLCON As New SQL_LIB
    Dim Dataset As New DataSet
    Dim Param_Name_Array() As String
    Dim Param_Value_Array() As String
    Public HomeParamObjArray() As SQL_LIB.HOME_PARAMETERS
    Dim F_ID As Integer, SELECT_COORDINATES_ROW_INDEX As Integer, Board_ID As Integer, rtn As Integer = 0
    Dim Start_Axis_ID, Total_Axis_Num As Integer, Card_NO As Integer, Stop_Code As Integer
    Dim AXIS_INDEX1 As New List(Of Integer)
    Dim AXIS_INDEX2 As New List(Of Integer)
    Dim AXIS_INDEX3 As New List(Of Integer)
    Dim AXIS_INDEX4 As New List(Of Integer)
    Dim AXIS_INDEX5 As New List(Of Integer)
    Dim Card_Init_OK As Boolean = False, Bool_Start_Home As Boolean = False

    ''' <summary>
    ''' 回原点模式，ALL:所有轴回原点 SIN：单轴回原点
    ''' </summary>
    ''' <remarks></remarks>
    Enum Home_Moel_Enum
        ALL = 0
        SIN = 1
    End Enum
    Dim Home_Model As Home_Moel_Enum

    Private Sub Home_Dialog_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        SQLCON.Close_DataBase()
    End Sub

    Private Sub Home_Dialog_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Control.CheckForIllegalCrossThreadCalls = False
        Card_Init_OK = Main.Card_Init_OK

        If SQLCON.DataBase_Initialization(SQLCON.DataBase_Data_Souce, SQLCON.DataBase_ID, SQLCON.DataBase_PassWord, SQLCON.DataBase_Catalog_Name, 50, , ) = True Then
            SQLCON.Read_Home_ParamS(HomeParamObjArray, DataGridViewX1)
            SET_AXIS_PITCH_SUB()

        End If
    End Sub

    Private Sub Btn_Add_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Btn_Add.Click
        If MessageBoxEx.Show("是否新增轴，点击【YES】确认!", "系统消息", MessageBoxButtons.YesNo, MessageBoxIcon.Information) = Windows.Forms.DialogResult.Yes Then
            Dim Add As New Modify_Home_Dialog(1)
            If Add.ShowDialog() = Windows.Forms.DialogResult.OK Then
                Array.Resize(Param_Name_Array, 11)
                Array.Resize(Param_Value_Array, 11)
                Param_Name_Array(0) = "轴名称"
                Param_Name_Array(1) = "轴号"
                Param_Name_Array(2) = "导程"
                Param_Name_Array(3) = "回原点模式"
                Param_Name_Array(4) = "回原点搜索方向"
                Param_Name_Array(5) = "回原点Z相信号"
                Param_Name_Array(6) = "回原点曲线"
                Param_Name_Array(7) = "回原点速度"
                Param_Name_Array(8) = "回原点加减速度"
                Param_Name_Array(9) = "回原点偏移"
                Param_Name_Array(10) = "回原点顺序"

                Param_Value_Array(0) = SQLCON.HomeParamObj.轴名称
                Param_Value_Array(1) = SQLCON.HomeParamObj.轴号
                Param_Value_Array(2) = SQLCON.HomeParamObj.导程
                Param_Value_Array(3) = SQLCON.HomeParamObj.回原点模式
                Param_Value_Array(4) = SQLCON.HomeParamObj.回原点搜索方向
                Param_Value_Array(5) = SQLCON.HomeParamObj.回原点Z相信号
                Param_Value_Array(6) = SQLCON.HomeParamObj.回原点曲线
                Param_Value_Array(7) = SQLCON.HomeParamObj.回原点速度
                Param_Value_Array(8) = SQLCON.HomeParamObj.回原点加减速度
                Param_Value_Array(9) = SQLCON.HomeParamObj.回原点偏移
                Param_Value_Array(10) = SQLCON.HomeParamObj.回原点顺序

                If SQLCON.New_Home_Param(Param_Name_Array, Param_Value_Array) = True Then
                    SQLCON.Read_Home_ParamS(HomeParamObjArray, DataGridViewX1)
                End If
            End If
        End If
    End Sub

    Private Sub Btn_Del_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Btn_Del.Click
        If MessageBoxEx.Show("是否删除此点位，点击【YES】确认删除!", "系统消息", MessageBoxButtons.YesNo, MessageBoxIcon.Information) = Windows.Forms.DialogResult.Yes Then
            If SQLCON.Delete_Param_ID("HOME_PARAMETERS", F_ID) = True Then
                SQLCON.Read_Home_ParamS(HomeParamObjArray, DataGridViewX1)
            End If
        End If

    End Sub

    Private Sub DataGridViewX1_CellClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DataGridViewX1.CellClick
        If e.RowIndex >= 0 Then
            F_ID = DataGridViewX1.Item(0, e.RowIndex).Value
            Try
                SQLCON.HomeParamObj.轴名称 = DataGridViewX1.Item(1, e.RowIndex).Value.ToString.Trim
                SQLCON.HomeParamObj.轴号 = DataGridViewX1.Item(2, e.RowIndex).Value
                SQLCON.HomeParamObj.导程 = DataGridViewX1.Item(3, e.RowIndex).Value
                SQLCON.HomeParamObj.回原点模式 = DataGridViewX1.Item(4, e.RowIndex).Value.ToString.Trim
                SQLCON.HomeParamObj.回原点搜索方向 = DataGridViewX1.Item(5, e.RowIndex).Value.ToString.Trim
                SQLCON.HomeParamObj.回原点Z相信号 = DataGridViewX1.Item(6, e.RowIndex).Value.ToString.Trim
                SQLCON.HomeParamObj.回原点曲线 = DataGridViewX1.Item(7, e.RowIndex).Value.ToString.Trim
                SQLCON.HomeParamObj.回原点速度 = DataGridViewX1.Item(8, e.RowIndex).Value
                SQLCON.HomeParamObj.回原点加减速度 = DataGridViewX1.Item(9, e.RowIndex).Value
                SQLCON.HomeParamObj.回原点偏移 = DataGridViewX1.Item(10, e.RowIndex).Value
                SQLCON.HomeParamObj.回原点顺序 = DataGridViewX1.Item(11, e.RowIndex).Value
            Catch ex As Exception
                MsgBox(ex.ToString)
            End Try
            SELECT_COORDINATES_ROW_INDEX = e.RowIndex
            Btn_Updata.Enabled = True
        End If
    End Sub

    Private Sub Btn_Updata_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Btn_Updata.Click
        Dim Add As New Modify_Home_Dialog(2)
        If Add.ShowDialog() = Windows.Forms.DialogResult.OK Then
            Array.Resize(Param_Name_Array, 11)
            Array.Resize(Param_Value_Array, 11)
            Param_Name_Array(0) = "轴名称"
            Param_Name_Array(1) = "轴号"
            Param_Name_Array(2) = "导程"
            Param_Name_Array(3) = "回原点模式"
            Param_Name_Array(4) = "回原点搜索方向"
            Param_Name_Array(5) = "回原点Z相信号"
            Param_Name_Array(6) = "回原点曲线"
            Param_Name_Array(7) = "回原点速度"
            Param_Name_Array(8) = "回原点加减速度"
            Param_Name_Array(9) = "回原点偏移"
            Param_Name_Array(10) = "回原点顺序"

            Param_Value_Array(0) = SQLCON.HomeParamObj.轴名称
            Param_Value_Array(1) = SQLCON.HomeParamObj.轴号
            Param_Value_Array(2) = SQLCON.HomeParamObj.导程
            Param_Value_Array(3) = SQLCON.HomeParamObj.回原点模式
            Param_Value_Array(4) = SQLCON.HomeParamObj.回原点搜索方向
            Param_Value_Array(5) = SQLCON.HomeParamObj.回原点Z相信号
            Param_Value_Array(6) = SQLCON.HomeParamObj.回原点曲线
            Param_Value_Array(7) = SQLCON.HomeParamObj.回原点速度
            Param_Value_Array(8) = SQLCON.HomeParamObj.回原点加减速度
            Param_Value_Array(9) = SQLCON.HomeParamObj.回原点偏移
            Param_Value_Array(10) = SQLCON.HomeParamObj.回原点顺序

            If SQLCON.Updata_HOme_Param_ID(Param_Name_Array, Param_Value_Array, F_ID) = True Then
                SQLCON.Read_Home_ParamS(HomeParamObjArray, DataGridViewX1)

                DataGridViewX1.FirstDisplayedScrollingRowIndex = SELECT_COORDINATES_ROW_INDEX
                DataGridViewX1.Rows(SELECT_COORDINATES_ROW_INDEX).Selected = True
            End If
        End If
    End Sub



    Private Sub Btn_Servo_On_Click(sender As System.Object, e As System.EventArgs) Handles Btn_Servo_On.Click
        '伺服使能
        For i As Integer = 0 To HomeParamObjArray.Length - 1
            rtn = APS_set_servo_on(HomeParamObjArray(i).轴号, PRA_OUT_STATUS_ENUM.OUT_ON)
            If rtn <> 0 Then
                MessageBox.Show("轴" & i & "使能失败，请检查！", "卡加载消息", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit For
            End If
        Next
        MessageBox.Show("伺服ON完成！", "卡加载消息", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub


    Sub Home_Param_Move(ByVal Index As Integer)
        HOME_PARAM.轴号 = HomeParamObjArray(Index).轴号
        Select Case HomeParamObjArray(Index).回原点模式
            Case "HOME_MODE_ORG"
                HOME_PARAM.回原点模式 = 0
            Case "HOME_MODE_EL"
                HOME_PARAM.回原点模式 = 1
            Case "HOME_MODE_EZ"
                HOME_PARAM.回原点模式 = 2
            Case Else
                HOME_PARAM.回原点模式 = 0
        End Select
        Select Case HomeParamObjArray(Index).回原点搜索方向
            Case "Positive"
                HOME_PARAM.回原点搜索方向 = 0
            Case "Negative"
                HOME_PARAM.回原点搜索方向 = 1
            Case Else
                HOME_PARAM.回原点搜索方向 = 0
        End Select
        Select Case HomeParamObjArray(Index).回原点Z相信号
            Case "Disable"
                HOME_PARAM.回原点Z相信号 = 0
            Case "Enable"
                HOME_PARAM.回原点Z相信号 = 1
            Case Else
                HOME_PARAM.回原点Z相信号 = 0
        End Select
        Select Case HomeParamObjArray(Index).回原点曲线
            Case "T_curve"
                HOME_PARAM.回原点曲线 = 0
            Case "S_curve"
                HOME_PARAM.回原点曲线 = 1
            Case Else
                HOME_PARAM.回原点曲线 = 0
        End Select
        HOME_PARAM.回原点速度 = HomeParamObjArray(Index).回原点速度
        HOME_PARAM.回原点加减速度 = HomeParamObjArray(Index).回原点加减速度
        HOME_PARAM.回原点偏移 = HomeParamObjArray(Index).回原点偏移
        CARDCMD.home_move_ex(HOME_PARAM.轴号, HOME_PARAM.回原点模式, HOME_PARAM.回原点搜索方向, HOME_PARAM.回原点曲线, HOME_PARAM.回原点速度, HOME_PARAM.回原点速度, HOME_PARAM.回原点Z相信号, HOME_PARAM.回原点偏移, HOME_PARAM.回原点加减速度)
    End Sub

    Sub HOME_MOVE()
        AXIS_INDEX1.Clear()
        AXIS_INDEX2.Clear()
        AXIS_INDEX3.Clear()
        AXIS_INDEX4.Clear()
        AXIS_INDEX5.Clear()
        '把程序优先级顺序排序，并存入序列号
        For i As Integer = 0 To HomeParamObjArray.Length - 1
            Select Case HomeParamObjArray(i).回原点顺序
                Case 1
                    AXIS_INDEX1.Add(i)
                Case 2
                    AXIS_INDEX2.Add(i)
                Case 3
                    AXIS_INDEX3.Add(i)
                Case 4
                    AXIS_INDEX4.Add(i)
                Case 5
                    AXIS_INDEX5.Add(i)
            End Select
        Next

        For M As Int16 = 1 To 5
            Dim AXIS_INDEX As New List(Of Integer)
            AXIS_INDEX.Clear()
            Select Case M
                Case 1
                    AXIS_INDEX = AXIS_INDEX1
                Case 2
                    AXIS_INDEX = AXIS_INDEX2
                Case 3
                    AXIS_INDEX = AXIS_INDEX3
                Case 4
                    AXIS_INDEX = AXIS_INDEX4
                Case 5
                    AXIS_INDEX = AXIS_INDEX5
            End Select

            Dim Index As Integer
            If AXIS_INDEX.Count > 0 Then
                For i As Integer = 0 To AXIS_INDEX.Count - 1
                    Index = AXIS_INDEX(i)
                    Home_Param_Move(Index)
                Next
                While True
                    Dim rtn_state As Integer = 1
                    For j As Integer = 0 To AXIS_INDEX.Count - 1
                        Index = AXIS_INDEX(j)
                        HOME_PARAM.轴号 = HomeParamObjArray(Index).轴号
                        rtn_state *= CARDCMD.CHECK_MOTION_DONE(HOME_PARAM.轴号, Stop_Code)
                    Next
                    If rtn_state = 1 Then
                        Exit While
                    End If
                End While
            End If
        Next
    End Sub

    ''' <summary>
    ''' 设置轴导程
    ''' </summary>
    ''' <remarks></remarks>
      Sub SET_AXIS_PITCH_SUB()
        Dim AXIS_PITCH(100) As MAGNIFICATION
        For i As Integer = 0 To HomeParamObjArray.Length - 1
            AXIS_PITCH(HomeParamObjArray(i).轴号).AXIS_MAGNIFICATION = 10000 / HomeParamObjArray(i).导程 '导程 脉冲/距离（plus/mm）
        Next
        CARDCMD.SET_AXIS_PITCH(AXIS_PITCH)
    End Sub

    Private Sub BK_HOME_DoWork(sender As System.Object, e As System.ComponentModel.DoWorkEventArgs) Handles BK_HOME.DoWork
        If Bool_Start_Home = True Then
            Bool_Start_Home = False

            If Card_Init_OK = False Then
                MessageBox.Show("轴初始化失败，无法回原点，请先检查！", "卡加载消息", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            End If

            Btn_Exit.Enabled = False

            Select Case Home_Model
                Case Home_Moel_Enum.ALL '所有轴回原点
                    HOME_MOVE()
                Case Home_Moel_Enum.SIN '单轴回原点
                    If SELECT_COORDINATES_ROW_INDEX >= 0 Then
                        Home_Param_Move(SELECT_COORDINATES_ROW_INDEX)
                        While True
                            Dim rtn_state As Integer = 1
                            rtn_state *= CARDCMD.CHECK_MOTION_DONE(HOME_PARAM.轴号, Stop_Code)
                            If rtn_state = 1 Then
                                Exit While
                            End If
                        End While
                    End If
            End Select

            MessageBoxEx.Show("回原点完成", "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If

        Btn_Exit.Enabled = True
    End Sub

    Private Sub Btn_Exit_Click(sender As System.Object, e As System.EventArgs) Handles Btn_Exit.Click
        Me.Close()
    End Sub

    Private Sub 单轴回原点ToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles 单轴回原点ToolStripMenuItem.Click
        Home_Model = Home_Moel_Enum.SIN
        Bool_Start_Home = True
        If BK_HOME.IsBusy = False Then
            BK_HOME.RunWorkerAsync()
        End If
    End Sub

    Private Sub 所有轴回原点ToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles 所有轴回原点ToolStripMenuItem.Click
        If MessageBoxEx.Show("是否确认所有轴回原点？请确认所有轴的回原点顺序，确认后，点击【YES】", "系统消息", MessageBoxButtons.YesNo, MessageBoxIcon.Information) = Windows.Forms.DialogResult.Yes Then
            Home_Model = Home_Moel_Enum.ALL
            Bool_Start_Home = True
            If BK_HOME.IsBusy = False Then
                BK_HOME.RunWorkerAsync()
            End If
        End If
    End Sub
End Class

Imports System.Threading
Imports Keyboard_Measure.GeneralClass.ADLINK_LIB.MOTION
Imports DevComponents.DotNetBar
Imports DevComponents.DotNetBar.Controls
Imports Keyboard_Measure.Axis_Order_Dialog
Imports Cognex.VisionPro.ToolGroup
Imports Cognex.VisionPro.ToolBlock
Imports Cognex.VisionPro
Imports Cognex.VisionPro.QuickBuild
Imports System.IO

Public Class System_SetDialog

    Dim DataConn As New DATA_CONVERSION_LIB
    Dim DATASET_COORDINATENAME_LEFT, DATASET_COORDINATENAME_RIGHT, DATASET_COORDINATENAME As DataSet
    Public Select_Product_Name As String, Select_Coordinate_Name As String, Select_Coordinate_Name_LEFT As String = "", Select_Coordinate_Name_RIGHT As String = "", Select_Coordinate_Name_Common As String = "", Axis_Pos_Name As String, Axis_Pos_NO As Int16 = 0, F_ID As Int16 = 0
    Dim Select_Product_Name_ID As Integer, Select_Coordiante_name_ID As Int16
    Dim Select_Row_Index, Max_Row_Count, SELECT_COORDINATES_ROW_INDEX As Integer
    Dim Read_Enc_Pos_Boolean As Boolean = False
    Dim Thread_Read_Pos As Thread
    Dim CARDCMD As New ADLINK_LIB.MOTION
    Dim SQLCON As New SQL_LIB
    Dim HomeParamObjArray() As SQL_LIB.HOME_PARAMETERS
    Dim Encoder_Pos() As Double
    Dim KEYDOWNBOOL, Hand_Move As Boolean
    Dim Left_Coordinate_NO As Int16 = 999, Right_Coordinate_NO As Int16 = 999 '左坐标系编号
    Dim PARAMETERS_NAME_ARRAY() As String, PARAMETERS_VALUE_ARRAY() As Double
    Dim DataGridView As DevComponents.DotNetBar.Controls.DataGridViewX = Nothing
    Dim SELECT_TABLE_NAME As String
    Dim PARAMETERS_VALUE_Object() As Object
    Dim AXIS_INDEX1 As New List(Of Integer)
    Dim AXIS_INDEX2 As New List(Of Integer)
    Dim AXIS_INDEX3 As New List(Of Integer)
    Dim AXIS_INDEX4 As New List(Of Integer)
    Dim AXIS_INDEX5 As New List(Of Integer)
    Dim KEY_FRONT As Windows.Forms.Keys = Keys.D, KEY_BACK As Windows.Forms.Keys = Keys.A
    Dim Axis_Name_Str As String = Nothing, Axis_Index_Str As String = Nothing, Error_Str As String = Nothing
    Dim Stop_Code As Integer = 0
    Dim Card_Init_OK As Boolean = False, Rtn_Int As Int16 = 0, PastIndex As Integer = 1
    Dim CLOSE_BOOLEAN, Bool_Error_Close, BOOL_EXIT, Emg_Stop_Button, Moto_Warning_Boolean(100), Moto_MEL_Boolean(100), Moto_PEL_Boolean(100), Bool_Read_MPEL As Boolean
    Dim Axis_ID As Integer, Axis_Pos As Double, Axis_ID_Array() As Integer = Nothing, Axis_Pos_Array() As Double = Nothing
    Dim Int_Copy_Coor_Name As Integer = 0, Int_Past_Coor_Name As Integer = 1, Copy_Coor_Name As String, Copy_Coor_Station As String, Past_Coor_Station As String
    Dim SuperTabItem_Index As Select_Coordinate
    Private Structure PARAM_BOOL
        Shared D登陆密码检测 As Boolean
    End Structure
    Public Structure COORDINATE_SELECT_PARAMS
        Shared 轴系 As String
        Shared 运动顺序 As String
    End Structure
    Public Enum Coordinate
        Add = 1
        updata = 2
    End Enum
    Public Enum Select_Coordinate
        Left
        Right
        Common
    End Enum
 

    Private Sub System_SetDialog_FormClosing(sender As Object, e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
     
    End Sub
    Private Sub System_SetDialog_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        SET_AXIS_PITCH_SUB()
        Dim ACC, DEC, VM, DIST As Double
        VM = DI_HAND_SPEED.Value
        ACC = DI_ACC.Value
        DEC = DI_ACC.Value
        DIST = DI_DIST.Value
        If e.Shift = True And Card_Init_OK = True Then
            Select Case e.KeyCode
                Case KEY_FRONT
                    If KEYDOWNBOOL = False Then
                        KEYDOWNBOOL = True
                        Select Case SwitchButton_Run_continuously.Value
                            Case False
                                CARDCMD.CONTINUOUS_MOVE(Axis_Pos_NO, PRA_JOG_DIR_ENUM.Negative, VM, PRA_SF_ENUM.S_curve, ACC, DEC)
                            Case True
                                CARDCMD.JOG_PTP(Axis_Pos_NO, PRA_APS_OPTION_ENUM.Relative, VM, DIST, Nothing)
                        End Select
                    End If
                Case KEY_BACK
                    If KEYDOWNBOOL = False Then
                        KEYDOWNBOOL = True
                        Select Case SwitchButton_Run_continuously.Value
                            Case False
                                CARDCMD.CONTINUOUS_MOVE(Axis_Pos_NO, PRA_JOG_DIR_ENUM.Positive, VM, PRA_SF_ENUM.S_curve, ACC, DEC)
                            Case True
                                CARDCMD.JOG_PTP(Axis_Pos_NO, PRA_APS_OPTION_ENUM.Relative, VM, -DIST, Nothing)
                        End Select
                    End If
            End Select
        End If
    End Sub

    Private Sub System_SetDialog_KeyUp(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyUp
        Select Case SwitchButton_Run_continuously.Value
            Case False
                If Card_Init_OK = True Then
                    STOP_SPORT(STOP_TYPE.JOG_STOP)
                End If
        End Select
        If e.KeyCode = Keys.Space Then
            If Card_Init_OK = True Then
                STOP_SPORT(STOP_TYPE.JOG_STOP)
            End If
        End If
        KEYDOWNBOOL = False
    End Sub

    Private Sub System_SetDialog_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        Control.CheckForIllegalCrossThreadCalls = False
        Card_Init_OK = Main.Card_Init_OK
        PARAM_BOOL.D登陆密码检测 = Main.PARAM_BOOL.D登陆密码检测
        If SQLCON.DataBase_Initialization(SQLCON.DataBase_Data_Souce, SQLCON.DataBase_ID, SQLCON.DataBase_PassWord, SQLCON.DataBase_Catalog_Name, 50, , ) = True Then
            SQLCON.Read_Home_ParamS(HomeParamObjArray)
            SET_AXIS_PITCH_SUB()
            Init_Pos_DataGridView() '初始化坐标表格控件
            SQLCON.Read_Project_Name(DX_PROJECT_NAME) '读取项目名称

            Array.Resize(PARAMETERS_NAME_ARRAY, 1)
            Array.Resize(PARAMETERS_VALUE_Object, 1)
            Array.Clear(PARAMETERS_NAME_ARRAY, 0, PARAMETERS_NAME_ARRAY.Length)
            Array.Clear(PARAMETERS_VALUE_Object, 0, PARAMETERS_VALUE_Object.Length)
            PARAMETERS_NAME_ARRAY(0) = "坐标复制次数"
            SQLCON.Read_Project_Parameter(PARAMETERS_NAME_ARRAY, PARAMETERS_VALUE_Object)
            PastIndex = CType(PARAMETERS_VALUE_Object(0), Integer)


            SuperTabControl_Coordinate.SelectedTabIndex = 0 '选中第一个
            SuperTabItem_Index = Select_Coordinate.Left

            Read_Enc_Pos_Boolean = True
            Thread_Read_Pos = New Thread(AddressOf Read_Pos_Fun)
            Thread_Read_Pos.Start()

            Timer_Start.Interval = 200
            Timer_Start.Start()

        End If

        Select Case Main.User_Name
            Case UserName.Admin
                Button_Delete_Product.Enabled = True
                Btn_DIO_Set.Enabled = True
                Btn_Auto_InsertPos.Enabled = True
                Btn_New_System_L.Enabled = True
                Btn_Updata_System_L.Enabled = True
                Btn_New_Pos.Enabled = True
                Btn_Delete_Pos.Enabled = True
                Btn_New_System_R.Enabled = True
                Btn_Updata_System_R.Enabled = True
                Btn_New_Pos_R.Enabled = True
                Btn_Delete_Pos_R.Enabled = True
                Btn_New_System_COMMON.Enabled = True
                Btn_Updata_System_COMMON.Enabled = True
                Btn_New_Pos_COMMON.Enabled = True
                Btn_Delete_Pos_COMMON.Enabled = True

                Button_Clear_All_CoorNamePos.Enabled = True
                Button_Updata_Single_Col_Pos.Enabled = True
                CM_Copy_Project.Enabled = True
                CM_Updata_Project.Enabled = True
                CM_COOR_NAME.Enabled = True

                SW_DISPENSING.Enabled = True
                SW_LASER.Enabled = True
                SW_READ_LASER.Enabled = True
                SW_CCD.Enabled = True
                SW_IO.Enabled = True
                SW_OTHER.Enabled = True
            Case Else
                Button_Delete_Product.Enabled = False
                Btn_DIO_Set.Enabled = False
                Btn_Auto_InsertPos.Enabled = False
                Btn_New_System_L.Enabled = False
                Btn_Updata_System_L.Enabled = False
                Btn_New_Pos.Enabled = False
                Btn_Delete_Pos.Enabled = False
                Btn_New_System_R.Enabled = False
                Btn_Updata_System_R.Enabled = False
                Btn_New_Pos_R.Enabled = False
                Btn_Delete_Pos_R.Enabled = False
                Btn_New_System_COMMON.Enabled = False
                Btn_Updata_System_COMMON.Enabled = False
                Btn_New_Pos_COMMON.Enabled = False
                Btn_Delete_Pos_COMMON.Enabled = False

                Button_Clear_All_CoorNamePos.Enabled = False
                Button_Updata_Single_Col_Pos.Enabled = False
                CM_Copy_Project.Enabled = False
                CM_Updata_Project.Enabled = False
                CM_COOR_NAME.Enabled = False

                SW_DISPENSING.Enabled = False
                SW_LASER.Enabled = False
                SW_READ_LASER.Enabled = False
                SW_CCD.Enabled = False
                SW_IO.Enabled = False
                SW_OTHER.Enabled = False
        End Select
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

    ''' <summary>
    ''' 停止运动
    ''' </summary>
    ''' <remarks></remarks>
    Sub STOP_SPORT(ByVal StopType As Integer)
        Select Case StopType
            Case STOP_TYPE.JOG_STOP
                For I As Integer = 0 To HomeParamObjArray.Length - 1
                    CARDCMD.JOG_STOP_MOVE(HomeParamObjArray(I).轴号)
                Next
            Case STOP_TYPE.EMG_STOP
                For I As Integer = 0 To HomeParamObjArray.Length - 1
                    CARDCMD.STOP_MOVE(HomeParamObjArray(I).轴号)
                Next
        End Select
    End Sub

    Private Sub Button_CLOSE_WID_Click(sender As System.Object, e As System.EventArgs) Handles Button_CLOSE_WID.Click
        Read_Enc_Pos_Boolean = False
        Timer_Start.Stop()

        Thread.Sleep(100)
        If Thread_Read_Pos IsNot Nothing Then
            If Thread_Read_Pos.IsAlive = True Then
                Thread_Read_Pos.Abort()
            End If
        End If
        SQLCON.Close_DataBase()

        Me.DialogResult = Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub DX_PROJECT_NAME_CellClick(sender As System.Object, e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DX_PROJECT_NAME.CellClick
        Int_Copy_Coor_Name = 0
        Int_Past_Coor_Name = 1

        Select_Row_Index = e.RowIndex
        Max_Row_Count = DX_PROJECT_NAME.RowCount
        If Max_Row_Count >= 0 And Select_Row_Index > -1 Then
            If Select_Row_Index = Max_Row_Count - 1 Then
                SuperTabControl_Coordinate.Enabled = False
                TextBox_Project_Name.Enabled = True
                TextBox_Project_Name.Clear()
                Button_Delete_Product.Enabled = False
            Else

                SuperTabControl_Coordinate.Enabled = True


                TextBox_Project_Name.Enabled = True
                If Main.User_Name = UserName.Admin Then
                    Button_Delete_Product.Enabled = True
                End If

                Select_Product_Name_ID = DX_PROJECT_NAME.Item(0, Select_Row_Index).Value
                TextBox_Project_Name.Text = DX_PROJECT_NAME.Item(1, Select_Row_Index).Value.ToString.Trim
                Select_Product_Name = DX_PROJECT_NAME.Item(1, Select_Row_Index).Value.ToString.Trim

                SQLCON.Read_Coordinate_Name(Select_Product_Name, STATION.LEFT, COORDINATENAME.LEFT_DATASET, DX_LEFT_COORNAME)  '读取坐标名称
                SQLCON.Read_Coordinate_Name(Select_Product_Name, STATION.RIGHT, COORDINATENAME.RIGHT_DATASET, DX_RIGHT_COORNAME)  '读取坐标名称
                SQLCON.Read_Coordinate_Name(Select_Product_Name, STATION.COMMON, COORDINATENAME.COMMON_DATASET, DX_COMMON_COORNAME)  '读取坐标名称


                Array.Resize(PARAMETERS_NAME_ARRAY, 8)
                Array.Resize(PARAMETERS_VALUE_Object, 8)
                Array.Clear(PARAMETERS_NAME_ARRAY, 0, PARAMETERS_NAME_ARRAY.Length)
                Array.Clear(PARAMETERS_VALUE_Object, 0, PARAMETERS_VALUE_Object.Length)
                PARAMETERS_NAME_ARRAY(0) = "激光程式号"
                PARAMETERS_NAME_ARRAY(1) = "影像程式号"
                PARAMETERS_NAME_ARRAY(2) = "公差上限"
                PARAMETERS_NAME_ARRAY(3) = "公差下限"
                PARAMETERS_NAME_ARRAY(4) = "镭射公差上限"
                PARAMETERS_NAME_ARRAY(5) = "镭射公差下限"
                PARAMETERS_NAME_ARRAY(6) = "功能键公差上限"
                PARAMETERS_NAME_ARRAY(7) = "功能键公差下限"
                SQLCON.Read_Project_Parameter(Select_Product_Name, PARAMETERS_NAME_ARRAY, PARAMETERS_VALUE_Object)
                IntegerInput_laser_program.Value = CType(PARAMETERS_VALUE_Object(0), Int16)
                'IntegerInput_laser_program.Value = CType(PARAMETERS_VALUE_Object(1), Int16)
                D_Limit_Up.Value = CType(PARAMETERS_VALUE_Object(2), Double)
                D_Limit_Down.Value = CType(PARAMETERS_VALUE_Object(3), Double)
                Laser_Limit_Up.Value = CType(PARAMETERS_VALUE_Object(4), Double)
                Laser_Limit_Down.Value = CType(PARAMETERS_VALUE_Object(5), Double)
                D_Limit_Up_F.Value = CType(PARAMETERS_VALUE_Object(6), Double)
                D_Limit_Down_F.Value = CType(PARAMETERS_VALUE_Object(7), Double)
            End If
        Else
            SuperTabControl_Coordinate.Enabled = False
        End If
        SQLCON.Read_Home_ParamS(HomeParamObjArray)
    End Sub

    Private Sub Btn_Updata_Speed_Click(sender As System.Object, e As System.EventArgs) Handles Btn_Updata_Speed.Click
        If Login_Dialog.ShowDialog = Windows.Forms.DialogResult.OK Then
            SQLCON.Update_Speed(Select_Product_Name, Select_Coordinate_Name_LEFT, DI_AUTO_SPEED.Value)
            READ_PROJECT_PARAMETER_FUN(Select_Product_Name, Select_Coordinate_Name_LEFT, DX_LEFT_COORDINATE, Coordinates_Operate.Read)
        End If
    End Sub

    Private Sub Btn_Updata_Acc_Click(sender As System.Object, e As System.EventArgs) Handles Btn_Updata_Acc.Click
        If Login_Dialog.ShowDialog = Windows.Forms.DialogResult.OK Then
            SQLCON.Update_ACC_DCC(Select_Product_Name, Select_Coordinate_Name_LEFT, DI_ACC.Value)
            READ_PROJECT_PARAMETER_FUN(Select_Product_Name, Select_Coordinate_Name_LEFT, DX_LEFT_COORDINATE, Coordinates_Operate.Read)
        End If
    End Sub

    Private Sub Btn_New_Pos_Click(sender As System.Object, e As System.EventArgs) Handles Btn_New_Pos.Click
        READ_PROJECT_PARAMETER_FUN(Select_Product_Name, Select_Coordinate_Name_LEFT, DX_LEFT_COORDINATE, Coordinates_Operate.Add_Updata, ADD_UPDATA_Enum.Add)
        READ_PROJECT_PARAMETER_FUN(Select_Product_Name, Select_Coordinate_Name_LEFT, DX_LEFT_COORDINATE, Coordinates_Operate.Read)
    End Sub

    Private Sub Btn_Updata_Pos_Click(sender As System.Object, e As System.EventArgs) Handles Btn_Updata_Pos.Click
        If F_ID = 0 Then
            If MessageBoxEx.Show("没有选中更新的坐标，是否更新所有【" & Select_Coordinate_Name_LEFT & "】的坐标？", "系统消息", MessageBoxButtons.YesNo, MessageBoxIcon.Information) = Windows.Forms.DialogResult.Yes Then
                READ_PROJECT_PARAMETER_FUN(Select_Product_Name, Select_Coordinate_Name_LEFT, DX_LEFT_COORDINATE, Coordinates_Operate.Add_Updata, ADD_UPDATA_Enum.Updata)
                READ_PROJECT_PARAMETER_FUN(Select_Product_Name, Select_Coordinate_Name_LEFT, DX_LEFT_COORDINATE, Coordinates_Operate.Read)
                Try
                    DX_LEFT_COORDINATE.FirstDisplayedScrollingRowIndex = SELECT_COORDINATES_ROW_INDEX
                    DX_LEFT_COORDINATE.Rows(SELECT_COORDINATES_ROW_INDEX).Selected = True
                Catch
                End Try
            End If
        Else
            READ_PROJECT_PARAMETER_FUN(Select_Product_Name, Select_Coordinate_Name_LEFT, DX_LEFT_COORDINATE, Coordinates_Operate.Add_Updata, ADD_UPDATA_Enum.Updata)
            READ_PROJECT_PARAMETER_FUN(Select_Product_Name, Select_Coordinate_Name_LEFT, DX_LEFT_COORDINATE, Coordinates_Operate.Read)
            Try
                DX_LEFT_COORDINATE.FirstDisplayedScrollingRowIndex = SELECT_COORDINATES_ROW_INDEX
                DX_LEFT_COORDINATE.Rows(SELECT_COORDINATES_ROW_INDEX).Selected = True
            Catch
            End Try
        End If

    End Sub

    Private Sub Btn_Delete_Pos_Click(sender As System.Object, e As System.EventArgs) Handles Btn_Delete_Pos.Click
        If MessageBoxEx.Show("是否删除ID=" & F_ID & "的当前坐标？", "系统消息", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
            SQLCON.Delete_Coordinates(F_ID)
            READ_PROJECT_PARAMETER_FUN(Select_Product_Name, Select_Coordinate_Name_LEFT, DX_LEFT_COORDINATE, Coordinates_Operate.Read)
        Else
            READ_PROJECT_PARAMETER_FUN(Select_Product_Name, Select_Coordinate_Name_LEFT, DX_LEFT_COORDINATE, Coordinates_Operate.Read)
        End If
    End Sub
    ''' <summary>
    ''' 显示当前坐标
    ''' </summary>
    ''' <remarks></remarks>
    Sub Read_Pos_Fun()
        While Read_Enc_Pos_Boolean = True
            Thread.Sleep(30)
            Application.DoEvents()
            READ_ENCODER_POSITION(Encoder_Pos)
            If Hand_Move = True Then
                Select Case Card_Init_OK
                    Case True
                        PE_AXIS_SPORT.Text = "各轴开始运动到指定位置"
                        PE_AXIS_SPORT.Style.BackColor1.Color = Color.Red
                        PE_AXIS_SPORT.Style.BackColor2.Color = Color.Red

                        AXIS_INDEX1.Clear()
                        AXIS_INDEX2.Clear()
                        AXIS_INDEX3.Clear()
                        AXIS_INDEX4.Clear()
                        AXIS_INDEX5.Clear()

                        '轴数量必须>=1
                        If Work_Coordinatess_Obj.轴名称.Count >= 1 Then
                        Else
                            Exit Sub
                        End If
                        '把程序优先级顺序排序，并存入序列号
                        For i As Integer = 0 To Work_Coordinatess_Obj.轴运动顺序.Length - 1 '判断优先级
                            Select Case Work_Coordinatess_Obj.轴运动顺序(i)
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

                            Select Case AXIS_INDEX.Count
                                Case Is = 1 '*****************************单轴运动
                                    '查询对应轴号
                                    For I As Integer = 0 To HomeParamObjArray.Length - 1
                                        If Work_Coordinatess_Obj.轴名称(AXIS_INDEX(0)) = HomeParamObjArray(I).轴名称 Then
                                            Axis_ID = HomeParamObjArray(I).轴号
                                            Axis_Pos = Work_Coordinatess_Obj.轴位置(AXIS_INDEX(0))
                                            Exit For
                                        End If
                                    Next
                                    Rtn_Int = CARDCMD.JOG_PTP_ALL(Axis_ID, PRA_APS_OPTION_ENUM.Absolute, Work_Coordinatess_Obj.运行速度, Work_Coordinatess_Obj.加减速度, Axis_Pos, Nothing)
                                    Select Case Rtn_Int
                                        Case Is = 0
                                            While BOOL_EXIT = False
                                                Dim rtn_state As Integer = 1
                                                rtn_state *= CARDCMD.CHECK_MOTION_DONE(Axis_ID, Stop_Code)
                                                If rtn_state = 1 Then
                                                    Exit While
                                                End If
                                            End While
                                        Case Is <> 0
                                            Error_Str = "单轴运动控制指令错误！请检查电机是否报警！"
                                            MessageBoxEx.Show(Error_Str, "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                    End Select
                                Case Is > 1 '*****************************插补运动
                                    '查询对应轴号
                                    Array.Resize(Axis_ID_Array, AXIS_INDEX.Count)
                                    Array.Resize(Axis_Pos_Array, AXIS_INDEX.Count)

                                    For I As Int16 = 0 To AXIS_INDEX.Count - 1
                                        For J As Integer = 0 To HomeParamObjArray.Length - 1
                                            If Work_Coordinatess_Obj.轴名称(AXIS_INDEX(I)) = HomeParamObjArray(J).轴名称 Then
                                                Axis_ID_Array(I) = HomeParamObjArray(J).轴号
                                                Axis_Pos_Array(I) = Work_Coordinatess_Obj.轴位置(AXIS_INDEX(I))
                                                Exit For
                                            End If
                                        Next
                                    Next
                                    Rtn_Int = CARDCMD.INTERPOLATION_LINE(Axis_ID_Array, Axis_Pos_Array, PRA_APS_OPTION_ENUM.Absolute, Work_Coordinatess_Obj.运行速度, Work_Coordinatess_Obj.加减速度, Work_Coordinatess_Obj.加减速度, PRA_SF_ENUM.S_curve)
                                    Select Case Rtn_Int
                                        Case Is = 0
                                            While BOOL_EXIT = False
                                                Dim rtn_state As Integer = 1
                                                For j As Integer = 0 To AXIS_INDEX.Count - 1
                                                    rtn_state *= CARDCMD.CHECK_MOTION_DONE(Axis_ID_Array(j), Stop_Code)
                                                Next
                                                If rtn_state = 1 Then
                                                    Exit While
                                                End If
                                            End While
                                        Case Is <> 0
                                            Error_Str = "插补运动控制指令错误！请检查电机是否报警或有轴不在同一张控制卡上，无法实现插补运动！"
                                            MessageBoxEx.Show(Error_Str, "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                    End Select
                            End Select
                        Next

                        PE_AXIS_SPORT.Text = "各轴运动完成"
                        PE_AXIS_SPORT.Style.BackColor1.Color = Color.Green
                        PE_AXIS_SPORT.Style.BackColor2.Color = Color.Green
                        Hand_Move = False
                    Case False
                        Hand_Move = False
                        MessageBoxEx.Show("轴初始化失败，无法进行定位操作，请先检查！", "控制卡初始化失败", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End Select
            End If
        End While
    End Sub

    ''' <summary>
    ''' 读取轴位置与报警信息
    ''' </summary>
    ''' <remarks></remarks>
    Sub READ_ENCODER_POSITION(ByRef Enc_Pos() As Double)
        Array.Resize(Enc_Pos, 100)
        If Read_Enc_Pos_Boolean = True And Card_Init_OK = True Then
            For I As Integer = 0 To HomeParamObjArray.Length - 1
                CARDCMD.GET_POSITION(HomeParamObjArray(I).轴号, Enc_Pos(I))
                DX_ENC_POS.Rows(I).Cells(0).Value = HomeParamObjArray(I).轴名称
                DX_ENC_POS.Rows(I).Cells(1).Value = HomeParamObjArray(I).轴号.ToString.Trim.PadLeft(2).Replace(" ", "0")
                DX_ENC_POS.Rows(I).Cells(2).Value = Format(Enc_Pos(I), "000.000")
            Next
        End If
    End Sub

    Private Sub DX_ENC_POS_CellClick(sender As System.Object, e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DX_ENC_POS.CellClick
        If e.RowIndex >= 0 And Card_Init_OK = True Then
            Axis_Pos_Name = Mid(DX_ENC_POS.Item(0, e.RowIndex).Value.ToString.Trim(), 1, 1)
            Axis_Pos_NO = CType(DX_ENC_POS.Item(1, e.RowIndex).Value, Int16)
            Select Case Axis_Pos_Name
                Case "X"
                    KEY_FRONT = Keys.D
                    KEY_BACK = Keys.A
                    PE_AXIS_SPORT.Text = "选中轴系【" & Axis_Pos_Name & "】" & "移动轴按【SHIFT】+【A】或【D】"
                Case "Y"
                    KEY_FRONT = Keys.W
                    KEY_BACK = Keys.S
                    PE_AXIS_SPORT.Text = "选中轴系【" & Axis_Pos_Name & "】" & "移动轴按【SHIFT】+【W】或【S】"
                Case "Z"
                    KEY_FRONT = Keys.Q
                    KEY_BACK = Keys.Z
                    PE_AXIS_SPORT.Text = "选中轴系【" & Axis_Pos_Name & "】" & "移动轴按【SHIFT】+【Z】或【Q】"
                Case "R"
                    KEY_FRONT = Keys.R
                    KEY_BACK = Keys.T
                    PE_AXIS_SPORT.Text = "选中轴系【" & Axis_Pos_Name & "】" & "移动轴按【SHIFT】+【R】或【T】"
                Case "U"
                    KEY_FRONT = Keys.W
                    KEY_BACK = Keys.S
                    PE_AXIS_SPORT.Text = "选中轴系【" & Axis_Pos_Name & "】" & "移动轴按【SHIFT】+【W】或【S】"
                Case "S"
                    KEY_FRONT = Keys.Z
                    KEY_BACK = Keys.Q
                    PE_AXIS_SPORT.Text = "选中轴系【" & Axis_Pos_Name & "】" & "移动轴按【SHIFT】+【Z】或【Q】"
            End Select
        End If
    End Sub


    Private Sub 定位到这ToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles 定位到这ToolStripMenuItem.Click
        If MessageBoxEx.Show("是否确定定位到此点？", "系统消息", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1) = System.Windows.Forms.DialogResult.Yes Then
            If Hand_Move = False Then
                Hand_Move = True
            End If
        End If
    End Sub

    Private Sub Btn_Updata_Speed_R_Click(sender As System.Object, e As System.EventArgs) Handles Btn_Updata_Speed_R.Click
        If Login_Dialog.ShowDialog = Windows.Forms.DialogResult.OK Then
            SQLCON.Update_Speed(Select_Product_Name, Select_Coordinate_Name_RIGHT, DI_AUTO_SPEED.Value)
            READ_PROJECT_PARAMETER_FUN(Select_Product_Name, Select_Coordinate_Name_RIGHT, DX_RIGHT_COORDINATE, Coordinates_Operate.Read)
        End If
    End Sub

    Private Sub Btn_Updata_Acc_R_Click(sender As System.Object, e As System.EventArgs) Handles Btn_Updata_Acc_R.Click
        If Login_Dialog.ShowDialog = Windows.Forms.DialogResult.OK Then
            SQLCON.Update_ACC_DCC(Select_Product_Name, Select_Coordinate_Name_RIGHT, DI_ACC.Value)
            READ_PROJECT_PARAMETER_FUN(Select_Product_Name, Select_Coordinate_Name_RIGHT, DX_RIGHT_COORDINATE, Coordinates_Operate.Read)
        End If
    End Sub

    Private Sub Btn_New_Pos_R_Click(sender As System.Object, e As System.EventArgs) Handles Btn_New_Pos_R.Click
        READ_PROJECT_PARAMETER_FUN(Select_Product_Name, Select_Coordinate_Name_RIGHT, DX_RIGHT_COORDINATE, Coordinates_Operate.Add_Updata, ADD_UPDATA_Enum.Add)
        READ_PROJECT_PARAMETER_FUN(Select_Product_Name, Select_Coordinate_Name_RIGHT, DX_RIGHT_COORDINATE, Coordinates_Operate.Read)
    End Sub

    Private Sub Btn_Updata_Pos_R_Click(sender As System.Object, e As System.EventArgs) Handles Btn_Updata_Pos_R.Click
        If F_ID = 0 Then
            If MessageBoxEx.Show("没有选中更新的坐标，是否更新所有【" & Select_Coordinate_Name_RIGHT & "】的坐标？", "系统消息", MessageBoxButtons.YesNo, MessageBoxIcon.Information) = Windows.Forms.DialogResult.Yes Then
                READ_PROJECT_PARAMETER_FUN(Select_Product_Name, Select_Coordinate_Name_RIGHT, DX_RIGHT_COORDINATE, Coordinates_Operate.Add_Updata, ADD_UPDATA_Enum.Updata)
                READ_PROJECT_PARAMETER_FUN(Select_Product_Name, Select_Coordinate_Name_RIGHT, DX_RIGHT_COORDINATE, Coordinates_Operate.Read)

                Try
                    DX_RIGHT_COORDINATE.FirstDisplayedScrollingRowIndex = SELECT_COORDINATES_ROW_INDEX
                    DX_RIGHT_COORDINATE.Rows(SELECT_COORDINATES_ROW_INDEX).Selected = True
                Catch
                End Try
            End If
        Else
            READ_PROJECT_PARAMETER_FUN(Select_Product_Name, Select_Coordinate_Name_RIGHT, DX_RIGHT_COORDINATE, Coordinates_Operate.Add_Updata, ADD_UPDATA_Enum.Updata)
            READ_PROJECT_PARAMETER_FUN(Select_Product_Name, Select_Coordinate_Name_RIGHT, DX_RIGHT_COORDINATE, Coordinates_Operate.Read)

            Try
                DX_RIGHT_COORDINATE.FirstDisplayedScrollingRowIndex = SELECT_COORDINATES_ROW_INDEX
                DX_RIGHT_COORDINATE.Rows(SELECT_COORDINATES_ROW_INDEX).Selected = True
            Catch
            End Try
        End If

    End Sub

    Private Sub Btn_Delete_Pos_R_Click(sender As System.Object, e As System.EventArgs) Handles Btn_Delete_Pos_R.Click
        If MessageBoxEx.Show("是否删除ID=" & F_ID & "的当前坐标？", "系统消息", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
            SQLCON.Delete_Coordinates(F_ID)
            READ_PROJECT_PARAMETER_FUN(Select_Product_Name, Select_Coordinate_Name_RIGHT, DX_RIGHT_COORDINATE, Coordinates_Operate.Read)
        Else
            READ_PROJECT_PARAMETER_FUN(Select_Product_Name, Select_Coordinate_Name_RIGHT, DX_RIGHT_COORDINATE, Coordinates_Operate.Read)
        End If
    End Sub

    Sub READ_PROJECT_PARAMETER_FUN(ByVal Project_Name As String, ByVal Coordinate_name As String, ByRef Coordinate_GridView As DataGridViewX, ByVal Model As Coordinates_Operate, Optional ByVal ADD_UPDATA As ADD_UPDATA_Enum = ADD_UPDATA_Enum.Add, Optional ByVal XY As PointStc = Nothing)
        Dim Row_Count, Point_Len As Integer, PARAMETERS_NAME_ARRAY() As String = Nothing, PARAMETERS_VALUE_ARRAY() As Object = Nothing
        Dim Axis_Name As String = Nothing, Axis_Index As String = Nothing
        If SQLCON.Cheng_Query_Coordinate_AxisSystem(Project_Name, Coordinate_name, Axis_Name, Axis_Index) = True Then
            PARAMETERS_NAME_ARRAY = Split(Axis_Name, ";")
        Else
            If Model = Coordinates_Operate.Add_Updata Then
                MessageBoxEx.Show("项目名称为：" & Project_Name & " 坐标名称为：" & Coordinate_name & " 的坐标系参数不存在，请先新增坐标系！", "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
            Exit Sub
        End If
        Point_Len = PARAMETERS_NAME_ARRAY.Length
        Select Case Model
            Case Coordinates_Operate.Read '读取坐标
                Array.Resize(PARAMETERS_NAME_ARRAY, Point_Len + 10)
                Array.Resize(PARAMETERS_VALUE_ARRAY, Point_Len + 10)
                PARAMETERS_NAME_ARRAY(Point_Len) = "键名"
                PARAMETERS_NAME_ARRAY(Point_Len + 1) = "运行速度"
                PARAMETERS_NAME_ARRAY(Point_Len + 2) = "加减速度"
                PARAMETERS_NAME_ARRAY(Point_Len + 3) = "点胶状态"
                PARAMETERS_NAME_ARRAY(Point_Len + 4) = "镭射触发状态"
                PARAMETERS_NAME_ARRAY(Point_Len + 5) = "镭射收数据状态"
                PARAMETERS_NAME_ARRAY(Point_Len + 6) = "CCD触发状态"
                PARAMETERS_NAME_ARRAY(Point_Len + 7) = "IO触发状态"
                PARAMETERS_NAME_ARRAY(Point_Len + 8) = "其他触发状态"
                PARAMETERS_NAME_ARRAY(Point_Len + 9) = "ID"
                SQLCON.Read_Coordinates(Project_Name, Coordinate_name, PARAMETERS_NAME_ARRAY, Coordinate_GridView)
            Case Coordinates_Operate.Copy '复制坐标
                Array.Resize(PARAMETERS_NAME_ARRAY, Point_Len + 9)
                Array.Resize(PARAMETERS_VALUE_ARRAY, Point_Len + 9)
                PARAMETERS_NAME_ARRAY(Point_Len) = "键名"
                PARAMETERS_NAME_ARRAY(Point_Len + 1) = "运行速度"
                PARAMETERS_NAME_ARRAY(Point_Len + 2) = "加减速度"
                PARAMETERS_NAME_ARRAY(Point_Len + 3) = "点胶状态"
                PARAMETERS_NAME_ARRAY(Point_Len + 4) = "镭射触发状态"
                PARAMETERS_NAME_ARRAY(Point_Len + 5) = "镭射收数据状态"
                PARAMETERS_NAME_ARRAY(Point_Len + 6) = "CCD触发状态"
                PARAMETERS_NAME_ARRAY(Point_Len + 7) = "IO触发状态"
                PARAMETERS_NAME_ARRAY(Point_Len + 8) = "其他触发状态"
                Row_Count = Coordinate_GridView.RowCount
                If Row_Count >= 1 Then
                    For i = 0 To Row_Count - 1
                        For j As Integer = 0 To Point_Len + 8
                            PARAMETERS_VALUE_ARRAY(j) = Coordinate_GridView.Item(j, i).Value
                        Next
                        SQLCON.Create_Coordinates(TextBox_Project_Name.Text.Trim, Coordinate_name, PARAMETERS_NAME_ARRAY, PARAMETERS_VALUE_ARRAY)
                    Next
                Else
                    For i As Int16 = 0 To Point_Len + 8
                        PARAMETERS_VALUE_ARRAY(i) = 0
                    Next
                    PARAMETERS_VALUE_ARRAY(Point_Len) = SW_KEY_NAME.Text.ToString.ToUpper.Trim
                    PARAMETERS_VALUE_ARRAY(Point_Len + 1) = DI_AUTO_SPEED.Value
                    PARAMETERS_VALUE_ARRAY(Point_Len + 2) = DI_ACC.Value
                    PARAMETERS_VALUE_ARRAY(Point_Len + 3) = IIf(SW_DISPENSING.Value, 1, 0)
                    PARAMETERS_VALUE_ARRAY(Point_Len + 4) = IIf(SW_LASER.Value, 1, 0)
                    PARAMETERS_VALUE_ARRAY(Point_Len + 5) = IIf(SW_READ_LASER.Value, 1, 0)
                    PARAMETERS_VALUE_ARRAY(Point_Len + 6) = IIf(SW_CCD.Value, 1, 0)
                    PARAMETERS_VALUE_ARRAY(Point_Len + 7) = IIf(SW_IO.Value, 1, 0)
                    PARAMETERS_VALUE_ARRAY(Point_Len + 8) = IIf(SW_OTHER.Value, 1, 0)
                    SQLCON.Create_Coordinates(TextBox_Project_Name.Text.Trim, Coordinate_name, PARAMETERS_NAME_ARRAY, PARAMETERS_VALUE_ARRAY)
                End If
            Case Coordinates_Operate.Add_Updata '新增坐标或更新坐标
                Array.Resize(PARAMETERS_NAME_ARRAY, Point_Len + 9)
                Array.Resize(PARAMETERS_VALUE_ARRAY, Point_Len + 9)
                PARAMETERS_NAME_ARRAY(Point_Len) = "键名"
                PARAMETERS_NAME_ARRAY(Point_Len + 1) = "运行速度"
                PARAMETERS_NAME_ARRAY(Point_Len + 2) = "加减速度"
                PARAMETERS_NAME_ARRAY(Point_Len + 3) = "点胶状态"
                PARAMETERS_NAME_ARRAY(Point_Len + 4) = "镭射触发状态"
                PARAMETERS_NAME_ARRAY(Point_Len + 5) = "镭射收数据状态"
                PARAMETERS_NAME_ARRAY(Point_Len + 6) = "CCD触发状态"
                PARAMETERS_NAME_ARRAY(Point_Len + 7) = "IO触发状态"
                PARAMETERS_NAME_ARRAY(Point_Len + 8) = "其他触发状态"
                Select Case ADD_UPDATA
                    Case ADD_UPDATA_Enum.Add
                        For i As Int16 = 0 To Point_Len - 1
                            For j As Integer = 0 To HomeParamObjArray.Length - 1
                                If PARAMETERS_NAME_ARRAY(i) = HomeParamObjArray(j).轴名称 Then '把当前轴的对应坐标写入数据库
                                    PARAMETERS_VALUE_ARRAY(i) = CType(DX_ENC_POS.Rows(j).Cells(2).Value, Double)
                                    Exit For
                                End If
                            Next
                        Next
                    Case ADD_UPDATA_Enum.Add_Auto
                        For i As Int16 = 0 To Point_Len - 1
                            For j As Integer = 0 To HomeParamObjArray.Length - 1
                                If PARAMETERS_NAME_ARRAY(i) = HomeParamObjArray(j).轴名称 Then '把当前轴的对应坐标写入数据库
                                    If PARAMETERS_NAME_ARRAY(i) = XY.X_NAME Then
                                        PARAMETERS_VALUE_ARRAY(i) = XY.X
                                    ElseIf PARAMETERS_NAME_ARRAY(i) = XY.Y_NAME Then
                                        PARAMETERS_VALUE_ARRAY(i) = XY.Y
                                    Else
                                        PARAMETERS_VALUE_ARRAY(i) = CType(DX_ENC_POS.Rows(j).Cells(2).Value, Double)
                                    End If
                                    Exit For
                                End If
                            Next
                        Next
                    Case ADD_UPDATA_Enum.Updata
                        For i As Int16 = 0 To Point_Len - 1
                            For j As Integer = 0 To HomeParamObjArray.Length - 1
                                If PARAMETERS_NAME_ARRAY(i) = HomeParamObjArray(j).轴名称 Then '把当前轴的对应坐标写入数据库
                                    PARAMETERS_VALUE_ARRAY(i) = CType(DX_ENC_POS.Rows(j).Cells(2).Value, Double)
                                    Exit For
                                End If
                            Next
                        Next
                    Case ADD_UPDATA_Enum.Updata_Single
                        For i As Int16 = 0 To Point_Len - 1
                            Dim value As Object = Coordinate_GridView.Item(i, SELECT_COORDINATES_ROW_INDEX).Value
                            If IsDBNull(value) = True Then
                                MessageBoxEx.Show("有空值，无法更新当前坐标！", "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2)
                                Exit Sub
                            ElseIf IsNumeric(value) = False Then
                                MessageBoxEx.Show("不是数字，无法更新当前坐标！", "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2)
                                Exit Sub
                            Else
                                PARAMETERS_VALUE_ARRAY(i) = CType(Coordinate_GridView.Item(i, SELECT_COORDINATES_ROW_INDEX).Value, Double)
                            End If
                        Next
                End Select
                PARAMETERS_VALUE_ARRAY(Point_Len) = SW_KEY_NAME.Text.ToString.ToUpper.Trim
                PARAMETERS_VALUE_ARRAY(Point_Len + 1) = DI_AUTO_SPEED.Value
                PARAMETERS_VALUE_ARRAY(Point_Len + 2) = DI_ACC.Value
                PARAMETERS_VALUE_ARRAY(Point_Len + 3) = IIf(SW_DISPENSING.Value, 1, 0)
                PARAMETERS_VALUE_ARRAY(Point_Len + 4) = IIf(SW_LASER.Value, 1, 0)
                PARAMETERS_VALUE_ARRAY(Point_Len + 5) = IIf(SW_READ_LASER.Value, 1, 0)
                PARAMETERS_VALUE_ARRAY(Point_Len + 6) = IIf(SW_CCD.Value, 1, 0)
                PARAMETERS_VALUE_ARRAY(Point_Len + 7) = IIf(SW_IO.Value, 1, 0)
                PARAMETERS_VALUE_ARRAY(Point_Len + 8) = IIf(SW_OTHER.Value, 1, 0)
                Select Case ADD_UPDATA
                    Case ADD_UPDATA_Enum.Add '新增坐标
                        SQLCON.Create_Coordinates(TextBox_Project_Name.Text.Trim, Coordinate_name, PARAMETERS_NAME_ARRAY, PARAMETERS_VALUE_ARRAY)
                    Case ADD_UPDATA_Enum.Add_Auto '批量新增坐标
                        SQLCON.Create_Coordinates(TextBox_Project_Name.Text.Trim, Coordinate_name, PARAMETERS_NAME_ARRAY, PARAMETERS_VALUE_ARRAY)
                    Case ADD_UPDATA_Enum.Updata '更新当前坐标
                        SQLCON.Update_Coordinates(Project_Name, Coordinate_name, PARAMETERS_NAME_ARRAY, PARAMETERS_VALUE_ARRAY, F_ID)
                    Case ADD_UPDATA_Enum.Updata_Single
                        SQLCON.Update_Coordinates(Project_Name, Coordinate_name, PARAMETERS_NAME_ARRAY, PARAMETERS_VALUE_ARRAY, F_ID)
                End Select
        End Select
    End Sub

    '坐标系操作
    Public Enum Coordinates_Operate
        Read = 1
        Copy = 2
        Add_Updata = 3
    End Enum
    Public Enum ADD_UPDATA_Enum
        Add '新增
        Updata '更新
        Updata_Single '更新单个
        Add_Auto
    End Enum

    Private Sub Btn_New_System_L_Click(sender As System.Object, e As System.EventArgs) Handles Btn_New_System_L.Click
        Coordinate_Axis_System(Coordinate.Add, Select_Coordinate_Name_LEFT, DX_LEFT_COORDINATE)
    End Sub

    Private Sub Btn_Updata_System_L_Click(sender As System.Object, e As System.EventArgs) Handles Btn_Updata_System_L.Click
        Coordinate_Axis_System(Coordinate.updata, Select_Coordinate_Name_LEFT, DX_LEFT_COORDINATE)
        DX_LEFT_COORDINATE.Columns.Clear()
    End Sub

    Sub Coordinate_Axis_System(ByVal Select_Model As Coordinate, ByVal Coordinate_Name As String, ByVal DataGridView As DevComponents.DotNetBar.Controls.DataGridViewX)
        If Coordinate_Name <> "" Then
            If Select_Product_Name IsNot Nothing Then
                Select Case Select_Model
                    Case Coordinate.Add '新增坐标系
                        If MessageBoxEx.Show("是否新增坐标系【" & Coordinate_Name & "】？", "系统消息", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
                            Select Case SQLCON.Cheng_Query_Coordinate_AxisSystem(Select_Product_Name, Coordinate_Name, COORDINATE_SELECT_PARAMS.轴系, COORDINATE_SELECT_PARAMS.运动顺序)
                                Case False '坐标系没有则创建
                                    Dim dlg As New Axis_Order_Dialog(_Model_Enum.Add)
                                    If dlg.ShowDialog() = Windows.Forms.DialogResult.Yes Then
                                        Dim Count As Int16 = dlg.Axis_Add_Name.Count
                                        If Count > 0 Then
                                            Dim Axis_System_Str As String = dlg.Axis_Add_Name_Str
                                            Dim Axis_System_Index As String = dlg.Axis_Add_Index_Str

                                            Select Case Inspect_Coordinate_Nomal(Axis_System_Index)
                                                Case True
                                                    If SQLCON.Cheng_New_Coordinate_AxisSystem(Select_Product_Name, Coordinate_Name, Axis_System_Str, Axis_System_Index) = True Then
                                                        MessageBoxEx.Show("新增坐标系完成,【" & Coordinate_Name & "】包含：" & Axis_System_Str, "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Information)
                                                    End If
                                                Case False
                                                    MessageBoxEx.Show("最多只能建立6轴插补，请重新修改轴的顺序！本次新建无效！", "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Information)
                                            End Select
                                        Else
                                            MessageBoxEx.Show("没有选定轴", "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Information)
                                        End If
                                    End If
                                Case True '坐标系已经创建
                                    MessageBoxEx.Show("坐标系【" & Coordinate_Name & "】已经存在，不能重复创建，请点击更新坐标系！", "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Information)
                            End Select
                        End If
                    Case Coordinate.updata '更新坐标系
                        If MessageBoxEx.Show("是否更新坐标系【" & Coordinate_Name & "】？", "系统消息", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
                            Select Case SQLCON.Cheng_Query_Coordinate_AxisSystem(Select_Product_Name, Coordinate_Name, COORDINATE_SELECT_PARAMS.轴系, COORDINATE_SELECT_PARAMS.运动顺序)
                                Case True '坐标系存在则更新
                                    Dim dlg As New Axis_Order_Dialog(_Model_Enum.Updata)
                                    If dlg.ShowDialog() = Windows.Forms.DialogResult.Yes Then
                                        Dim Count As Int16 = dlg.Axis_Add_Name.Count
                                        If Count > 0 Then
                                            Dim Axis_System_Str As String = dlg.Axis_Add_Name_Str
                                            Dim Axis_System_Index As String = dlg.Axis_Add_Index_Str

                                            Select Case Inspect_Coordinate_Nomal(Axis_System_Index)
                                                Case True
                                                    If SQLCON.Cheng_Updata_Coordinate_AxisSystem(Select_Product_Name, Coordinate_Name, Axis_System_Str, Axis_System_Index) = True Then
                                                        MessageBoxEx.Show("更新坐标系完成，【" & Coordinate_Name & "】包含：" & Axis_System_Str, "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Information)
                                                    End If
                                                Case False
                                                    MessageBoxEx.Show("最多只能建立6轴插补，请重新修改轴的顺序！本次更新无效！", "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Information)
                                            End Select
                                        Else
                                            MessageBoxEx.Show("没有选定轴", "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Information)
                                        End If
                                    End If
                                Case False '坐标系不存在不能更新
                                    MessageBoxEx.Show("坐标系【" & Coordinate_Name & "】已经存在，不能重复创建，请点击更新坐标系！", "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Information)
                            End Select
                        End If
                End Select
            Else
                MessageBoxEx.Show("没有选定项目名称！", "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        Else
            MessageBoxEx.Show("没有选定坐标名称！", "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

    Private Sub DX_LEFT_COORDINATE_CellClick(sender As System.Object, e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DX_LEFT_COORDINATE.CellClick
        Try
            SELECT_COORDINATES_ROW_INDEX = e.RowIndex
            If SELECT_COORDINATES_ROW_INDEX >= 0 Then
                F_ID = DX_LEFT_COORDINATE.Item(DX_LEFT_COORDINATE.ColumnCount - 1, SELECT_COORDINATES_ROW_INDEX).Value
                GP_Coordinate_Left.Text = "坐标系 当前ID【" & F_ID & "】"
                If SQLCON.Cheng_Query_Coordinate_AxisSystem(Select_Product_Name, Select_Coordinate_Name_LEFT, Axis_Name_Str, Axis_Index_Str) = True Then
                    Work_Coordinatess_Obj.轴名称 = Split(Axis_Name_Str, ";")
                    Work_Coordinatess_Obj.轴运动顺序 = Split(Axis_Index_Str, ";")
                Else
                    Exit Sub
                End If

                If Work_Coordinatess_Obj.轴名称.Count >= 1 Then '轴数量至少一个以上
                    Array.Resize(Work_Coordinatess_Obj.轴位置, Work_Coordinatess_Obj.轴名称.Count)
                    For i As Int16 = 0 To Work_Coordinatess_Obj.轴名称.Count - 1
                        If IsDBNull(DX_LEFT_COORDINATE.Item(Work_Coordinatess_Obj.轴名称(i), SELECT_COORDINATES_ROW_INDEX).Value) = False Then '在修改坐标系添加新轴时处理有值为空的情况
                            Work_Coordinatess_Obj.轴位置(i) = DX_LEFT_COORDINATE.Item(Work_Coordinatess_Obj.轴名称(i), SELECT_COORDINATES_ROW_INDEX).Value
                        Else
                            For j As Integer = 0 To HomeParamObjArray.Length - 1
                                If Work_Coordinatess_Obj.轴名称(i) = HomeParamObjArray(j).轴名称 Then '如果为空，则把当前轴的对应坐标保存进去
                                    Work_Coordinatess_Obj.轴位置(i) = CType(DX_ENC_POS.Rows(j).Cells(2).Value, Double)
                                    Exit For
                                End If
                            Next
                        End If
                    Next

                    Work_Coordinatess_Obj.运行速度 = DX_LEFT_COORDINATE.Item("运行速度", SELECT_COORDINATES_ROW_INDEX).Value
                    Work_Coordinatess_Obj.加减速度 = DX_LEFT_COORDINATE.Item("加减速度", SELECT_COORDINATES_ROW_INDEX).Value
                    If IsDBNull(DX_LEFT_COORDINATE.Item("键名", SELECT_COORDINATES_ROW_INDEX).Value) = True Then
                        Work_Coordinatess_Obj.键名 = ""
                    Else
                        Work_Coordinatess_Obj.键名 = DX_LEFT_COORDINATE.Item("键名", SELECT_COORDINATES_ROW_INDEX).Value
                    End If
                    If IsDBNull(DX_LEFT_COORDINATE.Item("点胶状态", SELECT_COORDINATES_ROW_INDEX).Value) = True Then
                        Work_Coordinatess_Obj.点胶状态 = False
                    Else
                        Work_Coordinatess_Obj.点胶状态 = DX_LEFT_COORDINATE.Item("点胶状态", SELECT_COORDINATES_ROW_INDEX).Value
                    End If
                    If IsDBNull(DX_LEFT_COORDINATE.Item("镭射触发状态", SELECT_COORDINATES_ROW_INDEX).Value) = True Then
                        Work_Coordinatess_Obj.镭射触发状态 = False
                    Else
                        Work_Coordinatess_Obj.镭射触发状态 = DX_LEFT_COORDINATE.Item("镭射触发状态", SELECT_COORDINATES_ROW_INDEX).Value
                    End If
                    If IsDBNull(DX_LEFT_COORDINATE.Item("镭射收数据状态", SELECT_COORDINATES_ROW_INDEX).Value) = True Then
                        Work_Coordinatess_Obj.镭射收数据状态 = False
                    Else
                        Work_Coordinatess_Obj.镭射收数据状态 = DX_LEFT_COORDINATE.Item("镭射收数据状态", SELECT_COORDINATES_ROW_INDEX).Value
                    End If
                    If IsDBNull(DX_LEFT_COORDINATE.Item("CCD触发状态", SELECT_COORDINATES_ROW_INDEX).Value) = True Then
                        Work_Coordinatess_Obj.CCD触发状态 = False
                    Else
                        Work_Coordinatess_Obj.CCD触发状态 = DX_LEFT_COORDINATE.Item("CCD触发状态", SELECT_COORDINATES_ROW_INDEX).Value
                    End If
                    If IsDBNull(DX_LEFT_COORDINATE.Item("IO触发状态", SELECT_COORDINATES_ROW_INDEX).Value) = True Then
                        Work_Coordinatess_Obj.IO触发状态 = False
                    Else
                        Work_Coordinatess_Obj.IO触发状态 = DX_LEFT_COORDINATE.Item("IO触发状态", SELECT_COORDINATES_ROW_INDEX).Value
                    End If
                    If IsDBNull(DX_LEFT_COORDINATE.Item("其他触发状态", SELECT_COORDINATES_ROW_INDEX).Value) = True Then
                        Work_Coordinatess_Obj.其他触发状态 = False
                    Else
                        Work_Coordinatess_Obj.其他触发状态 = DX_LEFT_COORDINATE.Item("其他触发状态", SELECT_COORDINATES_ROW_INDEX).Value
                    End If
                    Work_Coordinatess_Obj.慢速 = Work_Coordinatess_Obj.运行速度 * 0.1
                    Work_Coordinatess_Obj.中速 = Work_Coordinatess_Obj.运行速度 * 0.5

                    SW_KEY_NAME.Text = Work_Coordinatess_Obj.键名.Trim
                    DI_AUTO_SPEED.Value = Work_Coordinatess_Obj.运行速度
                    DI_ACC.Value = Work_Coordinatess_Obj.加减速度
                    SW_DISPENSING.Value = Work_Coordinatess_Obj.点胶状态
                    SW_LASER.Value = Work_Coordinatess_Obj.镭射触发状态
                    SW_READ_LASER.Value = Work_Coordinatess_Obj.镭射收数据状态
                    SW_CCD.Value = Work_Coordinatess_Obj.CCD触发状态
                    SW_IO.Value = Work_Coordinatess_Obj.IO触发状态
                    SW_OTHER.Value = Work_Coordinatess_Obj.其他触发状态
                End If
            Else
                SELECT_TABLE_NAME = Nothing
            End If
        Catch ex As Exception
            MessageBoxEx.Show(ex.ToString, "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Try
    End Sub


    Private Sub Button_Delete_Product_Click(sender As System.Object, e As System.EventArgs) Handles Button_Delete_Product.Click
        If MessageBoxEx.Show("是否删除项目名称，删除前请确认被删除项目是否有用！", "系统消息", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
            Select Case Login_Dialog.ShowDialog
                Case Windows.Forms.DialogResult.Retry
                    SQLCON.Delete_Project(Select_Product_Name)
                    SQLCON.Read_Project_Name(DX_PROJECT_NAME)
                Case Windows.Forms.DialogResult.No
                    MessageBoxEx.Show("密码错误！删除项目后再新建项目调试时间会很长，会造成产线停线，请慎重！", "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Stop, MessageBoxDefaultButton.Button1)
            End Select
        End If
    End Sub

    Sub New_Coordinate_Name(ByVal Btn_New_Coordinate_Name As Object, ByVal Text_Coordinate_Name As DevComponents.DotNetBar.Controls.TextBoxX, ByVal DX_COORNAME As DevComponents.DotNetBar.Controls.DataGridViewX, ByVal Select_Coordinate_Name As String, ByVal Station As String)
        Select Case Btn_New_Coordinate_Name.Text
            Case "新增坐标名称"
                If SQLCON.Exists_Coordinate_Name(Select_Product_Name, Text_Coordinate_Name.Text.Trim) = False Then
                    If Text_Coordinate_Name.Text.Trim.Length > 0 Then

                        '新增坐标名称
                        Array.Resize(PARAMETERS_NAME_ARRAY, 3)
                        Array.Resize(PARAMETERS_VALUE_Object, 3)
                        Array.Clear(PARAMETERS_NAME_ARRAY, 0, PARAMETERS_NAME_ARRAY.Length)
                        Array.Clear(PARAMETERS_VALUE_Object, 0, PARAMETERS_NAME_ARRAY.Length)
                        PARAMETERS_NAME_ARRAY(0) = "项目名称"
                        PARAMETERS_NAME_ARRAY(1) = "左右工位"
                        PARAMETERS_NAME_ARRAY(2) = "坐标名称"
                        If IsNumeric(Text_Coordinate_Name.Text.Trim) = True Then
                            MessageBoxEx.Show("工位名称请增加字母加以区分！", "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Stop)
                            Exit Sub
                        End If
                        PARAMETERS_VALUE_Object(0) = TextBox_Project_Name.Text.Trim
                        PARAMETERS_VALUE_Object(1) = Station
                        PARAMETERS_VALUE_Object(2) = Text_Coordinate_Name.Text.Trim
                        SQLCON.Create_Coordinate(PARAMETERS_NAME_ARRAY, PARAMETERS_VALUE_Object)

                        SQLCON.Read_Coordinate_Name(Select_Product_Name, Station, COORDINATENAME.DATASET, DX_COORNAME)  '读取坐标名称

                        MessageBoxEx.Show("工位名称新增完成！", "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1)
                    Else
                        MessageBoxEx.Show("请输入正确的工位参数，并且工位名称不能为空！", "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Stop, MessageBoxDefaultButton.Button1)
                    End If
                Else
                    MessageBoxEx.Show("指定新工位名称已经存在，请另外指定！", "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Stop, MessageBoxDefaultButton.Button1)
                End If
            Case "更新坐标名称"
                If MessageBoxEx.Show("是否修改参数？请记住现有参数！", "系统消息", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
                    If IsNumeric(TextBox_Project_Name.Text.Trim) = True Then
                        MessageBoxEx.Show("工位名称请增加字母加以区分！", "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Stop, MessageBoxDefaultButton.Button1)
                        Exit Sub
                    End If
                    If SQLCON.Exists_Coordinate_Name(Select_Product_Name, Text_Coordinate_Name.Text.Trim) = False Then
                        '更新坐标名称
                        Array.Resize(PARAMETERS_NAME_ARRAY, 3)
                        Array.Resize(PARAMETERS_VALUE_Object, 3)
                        Array.Clear(PARAMETERS_NAME_ARRAY, 0, PARAMETERS_NAME_ARRAY.Length)
                        Array.Clear(PARAMETERS_VALUE_Object, 0, PARAMETERS_NAME_ARRAY.Length)
                        PARAMETERS_NAME_ARRAY(0) = "项目名称"
                        PARAMETERS_NAME_ARRAY(1) = "左右工位"
                        PARAMETERS_NAME_ARRAY(2) = "坐标名称"
                        If IsNumeric(Text_Coordinate_Name.Text.Trim) = True Then
                            MessageBoxEx.Show("工位名称请增加字母加以区分！", "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Stop)
                            Exit Sub
                        End If
                        PARAMETERS_VALUE_Object(0) = TextBox_Project_Name.Text.Trim
                        PARAMETERS_VALUE_Object(1) = Station
                        PARAMETERS_VALUE_Object(2) = Text_Coordinate_Name.Text.Trim

                        If Select_Coordinate_Name <> Text_Coordinate_Name.Text.Trim Then
                            SQLCON.Update_Coordinate_Parameter(Select_Coordinate_Name, Text_Coordinate_Name.Text.Trim)
                            Select_Coordinate_Name = Text_Coordinate_Name.Text.Trim
                        End If

                        SQLCON.Read_Coordinate_Name(Select_Product_Name, Station, COORDINATENAME.DATASET, DX_COORNAME)  '读取坐标名称
                        MessageBoxEx.Show("参数修改完成！", "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1)
                    Else
                        MessageBoxEx.Show("指定新工位名称已经存在，请另外指定！", "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Stop, MessageBoxDefaultButton.Button1)
                    End If
                End If
        End Select
    End Sub

    Private Sub DX_LEFT_COORNAME_CellClick(sender As System.Object, e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DX_LEFT_COORNAME.CellClick
        F_ID = 0 '先清空ID
        Select_Row_Index = e.RowIndex
        Max_Row_Count = DX_LEFT_COORNAME.RowCount
        DX_LEFT_COORDINATE.Columns.Clear()

        If Max_Row_Count >= 0 And Select_Row_Index > -1 Then
            If Select_Row_Index = Max_Row_Count - 1 Then
                CM_Add_CoorName.Text = "新增坐标名称"
                TX_Coordinate_Name_L.Enabled = True
                TX_Coordinate_Name_L.Clear()
                CM_Delete_CoorName.Enabled = False
                Select_Coordinate_Name_LEFT = ""
            Else
                CM_Add_CoorName.Text = "更新坐标名称"
                TX_Coordinate_Name_L.Enabled = True
                CM_Delete_CoorName.Enabled = True

                Select_Coordiante_name_ID = DX_LEFT_COORNAME.Item(0, Select_Row_Index).Value
                TX_Coordinate_Name_L.Text = DX_LEFT_COORNAME.Item(1, Select_Row_Index).Value.ToString.Trim
                Select_Coordinate_Name_LEFT = DX_LEFT_COORNAME.Item(1, Select_Row_Index).Value.ToString.Trim

                Select_Coordinate_Name = Select_Coordinate_Name_LEFT
                READ_PROJECT_PARAMETER_FUN(Select_Product_Name, Select_Coordinate_Name_LEFT, DX_LEFT_COORDINATE, Coordinates_Operate.Read)

            End If
        Else
            DX_LEFT_COORDINATE.Columns.Clear()
        End If
    End Sub


    Private Sub Btn_New_System_R_Click(sender As System.Object, e As System.EventArgs) Handles Btn_New_System_R.Click
        Coordinate_Axis_System(Coordinate.Add, Select_Coordinate_Name_RIGHT, DX_RIGHT_COORDINATE)
    End Sub


    Private Sub Btn_Updata_System_R_Click(sender As System.Object, e As System.EventArgs) Handles Btn_Updata_System_R.Click
        Coordinate_Axis_System(Coordinate.updata, Select_Coordinate_Name_RIGHT, DX_RIGHT_COORDINATE)
        DX_RIGHT_COORDINATE.Columns.Clear()
    End Sub

    Private Sub DX_RIGHT_COORNAME_CellClick(sender As System.Object, e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DX_RIGHT_COORNAME.CellClick
        F_ID = 0 '先清空ID
        Select_Row_Index = e.RowIndex
        Max_Row_Count = DX_RIGHT_COORNAME.RowCount
        DX_RIGHT_COORDINATE.Columns.Clear()

        If Max_Row_Count >= 0 And Select_Row_Index > -1 Then
            If Select_Row_Index = Max_Row_Count - 1 Then
                CM_Add_CoorName.Text = "新增坐标名称"
                TX_Coordinate_Name_R.Enabled = True
                TX_Coordinate_Name_R.Clear()
                CM_Delete_CoorName.Enabled = False
                Select_Coordinate_Name_RIGHT = ""
            Else
                CM_Add_CoorName.Text = "更新坐标名称"

                TX_Coordinate_Name_R.Enabled = True
                CM_Delete_CoorName.Enabled = True

                Select_Coordiante_name_ID = DX_RIGHT_COORNAME.Item(0, Select_Row_Index).Value
                TX_Coordinate_Name_R.Text = DX_RIGHT_COORNAME.Item(1, Select_Row_Index).Value.ToString.Trim
                Select_Coordinate_Name_RIGHT = DX_RIGHT_COORNAME.Item(1, Select_Row_Index).Value.ToString.Trim
                Select_Coordinate_Name = Select_Coordinate_Name_RIGHT
                READ_PROJECT_PARAMETER_FUN(Select_Product_Name, Select_Coordinate_Name_RIGHT, DX_RIGHT_COORDINATE, Coordinates_Operate.Read)

            End If
        Else
            DX_RIGHT_COORDINATE.Columns.Clear()
        End If
    End Sub

    Private Sub DX_RIGHT_COORDINATE_CellClick(sender As System.Object, e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DX_RIGHT_COORDINATE.CellClick
        Try
            SELECT_COORDINATES_ROW_INDEX = e.RowIndex
            If SELECT_COORDINATES_ROW_INDEX >= 0 Then
                F_ID = DX_RIGHT_COORDINATE.Item(DX_RIGHT_COORDINATE.ColumnCount - 1, SELECT_COORDINATES_ROW_INDEX).Value
                GP_Coordinate_Right.Text = "坐标系 当前ID【" & F_ID & "】"
                If SQLCON.Cheng_Query_Coordinate_AxisSystem(Select_Product_Name, Select_Coordinate_Name_RIGHT, Axis_Name_Str, Axis_Index_Str) = True Then
                    Work_Coordinatess_Obj.轴名称 = Split(Axis_Name_Str, ";")
                    Work_Coordinatess_Obj.轴运动顺序 = Split(Axis_Index_Str, ";")
                Else
                    Exit Sub
                End If

                If Work_Coordinatess_Obj.轴名称.Count >= 1 Then '轴数量至少一个以上
                    Array.Resize(Work_Coordinatess_Obj.轴位置, Work_Coordinatess_Obj.轴名称.Count)
                    For i As Int16 = 0 To Work_Coordinatess_Obj.轴名称.Count - 1
                        If IsDBNull(DX_RIGHT_COORDINATE.Item(Work_Coordinatess_Obj.轴名称(i), SELECT_COORDINATES_ROW_INDEX).Value) = False Then '在修改坐标系添加新轴时处理有值为空的情况
                            Work_Coordinatess_Obj.轴位置(i) = DX_RIGHT_COORDINATE.Item(Work_Coordinatess_Obj.轴名称(i), SELECT_COORDINATES_ROW_INDEX).Value
                        Else
                            For j As Integer = 0 To HomeParamObjArray.Length - 1
                                If Work_Coordinatess_Obj.轴名称(i) = HomeParamObjArray(j).轴名称 Then '如果为空，则把当前轴的对应坐标保存进去
                                    Work_Coordinatess_Obj.轴位置(i) = CType(DX_ENC_POS.Rows(j).Cells(2).Value, Double)
                                    Exit For
                                End If
                            Next
                        End If
                    Next
                    Work_Coordinatess_Obj.运行速度 = DX_RIGHT_COORDINATE.Item("运行速度", SELECT_COORDINATES_ROW_INDEX).Value
                    Work_Coordinatess_Obj.加减速度 = DX_RIGHT_COORDINATE.Item("加减速度", SELECT_COORDINATES_ROW_INDEX).Value
                    If IsDBNull(DX_RIGHT_COORDINATE.Item("键名", SELECT_COORDINATES_ROW_INDEX).Value) = True Then
                        Work_Coordinatess_Obj.键名 = ""
                    Else
                        Work_Coordinatess_Obj.键名 = DX_RIGHT_COORDINATE.Item("键名", SELECT_COORDINATES_ROW_INDEX).Value
                    End If
                    If IsDBNull(DX_RIGHT_COORDINATE.Item("点胶状态", SELECT_COORDINATES_ROW_INDEX).Value) = True Then
                        Work_Coordinatess_Obj.点胶状态 = False
                    Else
                        Work_Coordinatess_Obj.点胶状态 = DX_RIGHT_COORDINATE.Item("点胶状态", SELECT_COORDINATES_ROW_INDEX).Value
                    End If
                    If IsDBNull(DX_RIGHT_COORDINATE.Item("镭射触发状态", SELECT_COORDINATES_ROW_INDEX).Value) = True Then
                        Work_Coordinatess_Obj.镭射触发状态 = False
                    Else
                        Work_Coordinatess_Obj.镭射触发状态 = DX_RIGHT_COORDINATE.Item("镭射触发状态", SELECT_COORDINATES_ROW_INDEX).Value
                    End If
                    If IsDBNull(DX_RIGHT_COORDINATE.Item("镭射收数据状态", SELECT_COORDINATES_ROW_INDEX).Value) = True Then
                        Work_Coordinatess_Obj.镭射收数据状态 = False
                    Else
                        Work_Coordinatess_Obj.镭射收数据状态 = DX_RIGHT_COORDINATE.Item("镭射收数据状态", SELECT_COORDINATES_ROW_INDEX).Value
                    End If
                    If IsDBNull(DX_RIGHT_COORDINATE.Item("CCD触发状态", SELECT_COORDINATES_ROW_INDEX).Value) = True Then
                        Work_Coordinatess_Obj.CCD触发状态 = False
                    Else
                        Work_Coordinatess_Obj.CCD触发状态 = DX_RIGHT_COORDINATE.Item("CCD触发状态", SELECT_COORDINATES_ROW_INDEX).Value
                    End If
                    If IsDBNull(DX_RIGHT_COORDINATE.Item("IO触发状态", SELECT_COORDINATES_ROW_INDEX).Value) = True Then
                        Work_Coordinatess_Obj.IO触发状态 = False
                    Else
                        Work_Coordinatess_Obj.IO触发状态 = DX_RIGHT_COORDINATE.Item("IO触发状态", SELECT_COORDINATES_ROW_INDEX).Value
                    End If
                    If IsDBNull(DX_RIGHT_COORDINATE.Item("其他触发状态", SELECT_COORDINATES_ROW_INDEX).Value) = True Then
                        Work_Coordinatess_Obj.其他触发状态 = False
                    Else
                        Work_Coordinatess_Obj.其他触发状态 = DX_RIGHT_COORDINATE.Item("其他触发状态", SELECT_COORDINATES_ROW_INDEX).Value
                    End If
                    Work_Coordinatess_Obj.慢速 = Work_Coordinatess_Obj.运行速度 * 0.1
                    Work_Coordinatess_Obj.中速 = Work_Coordinatess_Obj.运行速度 * 0.5

                    SW_KEY_NAME.Text = Work_Coordinatess_Obj.键名.Trim
                    DI_AUTO_SPEED.Value = Work_Coordinatess_Obj.运行速度
                    DI_ACC.Value = Work_Coordinatess_Obj.加减速度
                    SW_DISPENSING.Value = Work_Coordinatess_Obj.点胶状态
                    SW_LASER.Value = Work_Coordinatess_Obj.镭射触发状态
                    SW_READ_LASER.Value = Work_Coordinatess_Obj.镭射收数据状态
                    SW_CCD.Value = Work_Coordinatess_Obj.CCD触发状态
                    SW_IO.Value = Work_Coordinatess_Obj.IO触发状态
                    SW_OTHER.Value = Work_Coordinatess_Obj.其他触发状态
                End If
            Else
                SELECT_TABLE_NAME = Nothing
            End If
        Catch ex As Exception
            MessageBoxEx.Show(ex.ToString, "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Try
    End Sub

    Private Sub Btn_DIO_Set_Click(sender As System.Object, e As System.EventArgs) Handles Btn_DIO_Set.Click
        Select Case PARAM_BOOL.D登陆密码检测
            Case True
                If Login_Dialog.ShowDialog = Windows.Forms.DialogResult.OK Then
                    Dim IO As New Adlink_DIO_Dialog()
                    IO.ShowDialog()
                End If
            Case False
                Dim IO As New Adlink_DIO_Dialog()
                IO.ShowDialog()
        End Select

    End Sub

    Private Sub Btn_Param_Set_Click(sender As System.Object, e As System.EventArgs) Handles Btn_Param_Set.Click
        Select Case PARAM_BOOL.D登陆密码检测
            Case True
                If Login_Dialog.ShowDialog = Windows.Forms.DialogResult.OK Then
                    Parameter_Set_Dialog.ShowDialog()
                End If
            Case False
                Parameter_Set_Dialog.ShowDialog()
        End Select
    End Sub

    Private Sub Timer_Start_Tick(sender As System.Object, e As System.EventArgs) Handles Timer_Start.Tick
        If Card_Init_OK = True Then
            If BOOL_EXIT = False Then
                READ_EMG_STOP()
            End If
            If Bool_Error_Close = True And BOOL_EXIT = False Then
                BOOL_EXIT = True
                Dim Waring As New WARING(Error_Str, 5)
                Waring.ShowDialog()
                DataConn.KillProcess("excel")
                DataConn.KillProcess(ProductName)
            End If
        End If
    End Sub

    ''' <summary>
    ''' 读取急停状态
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function READ_EMG_STOP() As Integer

        '读取急停状态
        Select Case CARDCMD.GET_EMG_STATUS(0)
            Case True
                If Emg_Stop_Button = False Then
                    STOP_SPORT(STOP_TYPE.EMG_STOP)
                    Emg_Stop_Button = True
                    Error_Str = "[急停按钮]被按下,软件即将关闭"
                    Bool_Error_Close = True
                    Return 1 '函数直接返回
                End If
            Case False
                If Emg_Stop_Button = True Then
                    Thread.Sleep(100)
                    Emg_Stop_Button = False
                End If
                READ_EMG_STOP = -1
        End Select
        '读取报警状态
        If HomeParamObjArray.Length > 1 Then
            For i As Int16 = 0 To HomeParamObjArray.Length - 1
                Select Case CARDCMD.GET_ALM_STATUS(HomeParamObjArray(i).轴号)
                    Case True
                        If Moto_Warning_Boolean(i) = False Then
                            STOP_SPORT(STOP_TYPE.JOG_STOP)
                            Moto_Warning_Boolean(i) = True
                            Error_Str = "[" & HomeParamObjArray(i).轴名称 & "]轴电机报警，软件即将关闭，请清除报警后重新打开软件！"
                            PE_AXIS_SPORT.Style.ForeColor.Color = Color.Red
                            DX_ENC_POS.Rows(i).DefaultCellStyle.ForeColor = Color.Red
                            PE_AXIS_SPORT.Text = Error_Str
                            Bool_Error_Close = True
                            Return 2 '函数直接返回
                        End If
                    Case False
                        If Moto_Warning_Boolean(i) = True Then
                            Moto_Warning_Boolean(i) = False
                            PE_AXIS_SPORT.Style.ForeColor.Color = Color.Silver
                            PE_AXIS_SPORT.Text = ""
                        End If
                        READ_EMG_STOP = -2
                End Select

                '负限位
                Select Case CARDCMD.GET_MEL_STATUS(HomeParamObjArray(i).轴号)
                    Case True
                        If Moto_MEL_Boolean(i) = False Then
                            STOP_SPORT(STOP_TYPE.JOG_STOP)
                            Moto_MEL_Boolean(i) = True
                            Error_Str = "[" & HomeParamObjArray(i).轴名称 & "]轴负限位触发"
                            PE_AXIS_SPORT.Style.ForeColor.Color = Color.Red
                            PE_AXIS_SPORT.Text = Error_Str
                        End If
                        READ_EMG_STOP = 3
                    Case False
                        If Moto_MEL_Boolean(i) = True Then
                            Moto_MEL_Boolean(i) = False
                            PE_AXIS_SPORT.Style.ForeColor.Color = Color.Silver
                            PE_AXIS_SPORT.Text = ""
                        End If
                        READ_EMG_STOP = -3
                End Select
                '正限位
                Select Case CARDCMD.GET_PEL_STATUS(HomeParamObjArray(i).轴号)
                    Case True
                        If Moto_PEL_Boolean(i) = False Then
                            STOP_SPORT(STOP_TYPE.JOG_STOP)
                            Moto_PEL_Boolean(i) = True
                            Error_Str = "[" & HomeParamObjArray(i).轴名称 & "]轴正限位触发"
                            PE_AXIS_SPORT.Style.ForeColor.Color = Color.Red
                            PE_AXIS_SPORT.Text = Error_Str
                        End If
                        READ_EMG_STOP = 4
                    Case False
                        If Moto_PEL_Boolean(i) = True Then
                            Moto_PEL_Boolean(i) = False
                            PE_AXIS_SPORT.Style.ForeColor.Color = Color.Silver
                            PE_AXIS_SPORT.Text = ""
                        End If
                        READ_EMG_STOP = -4
                End Select
            Next
        End If
        Return READ_EMG_STOP
    End Function

    Sub Init_Pos_DataGridView()
        DX_ENC_POS.Rows.Clear()
        For i As Int16 = 0 To HomeParamObjArray.Length - 1
            DX_ENC_POS.Rows.Add()
        Next
        DX_ENC_POS.RowHeadersVisible = False
        DX_ENC_POS.AllowUserToDeleteRows = False
        DX_ENC_POS.AllowUserToResizeColumns = False
        DX_ENC_POS.AllowUserToResizeRows = False
        DX_ENC_POS.ColumnHeadersHeightSizeMode = Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        DX_ENC_POS.MultiSelect = False
        DX_ENC_POS.ReadOnly = True
        DX_ENC_POS.RowHeadersWidthSizeMode = Windows.Forms.DataGridViewRowHeadersWidthSizeMode.EnableResizing
        DX_ENC_POS.SelectionMode = Windows.Forms.DataGridViewSelectionMode.FullRowSelect
    End Sub

    ''' <summary>
    ''' 检测坐标系是否正常
    ''' </summary>
    ''' <param name="InputStr"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function Inspect_Coordinate_Nomal(ByVal InputStr As String) As Boolean
        Dim str() As String = Split(InputStr, ";")
        Dim Index(4) As List(Of Boolean)
        For j As Int16 = 0 To 4
            Index(j) = New List(Of Boolean)
            Index(j).Clear()
        Next
        For i As Int16 = 0 To str.Length - 1
            If str(i).Contains("1") = True Then
                Index(0).Add(True)
            End If
            If str(i).Contains("2") = True Then
                Index(1).Add(True)
            End If
            If str(i).Contains("3") = True Then
                Index(2).Add(True)
            End If
            If str(i).Contains("4") = True Then
                Index(3).Add(True)
            End If
            If str(i).Contains("5") = True Then
                Index(4).Add(True)
            End If
        Next
        For i As Int16 = 0 To 4
            If Index(i).Count > 6 Then
                Return False
            End If
        Next
        Return True
    End Function


    Private Sub DX_COMMON_COORNAME_CellClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DX_COMMON_COORNAME.CellClick
        F_ID = 0 '先清空ID
        Select_Row_Index = e.RowIndex
        Max_Row_Count = DX_COMMON_COORNAME.RowCount
        DX_COMMON_COORDINATE.Columns.Clear()

        If Max_Row_Count >= 0 And Select_Row_Index > -1 Then
            If Select_Row_Index = Max_Row_Count - 1 Then
                CM_Add_CoorName.Text = "新增坐标名称"
                TX_Coordinate_Name_Common.Enabled = True
                TX_Coordinate_Name_Common.Clear()
                CM_Delete_CoorName.Enabled = False
                Select_Coordinate_Name_Common = ""
            Else
                CM_Add_CoorName.Text = "更新坐标名称"

                TX_Coordinate_Name_Common.Enabled = True
                CM_Delete_CoorName.Enabled = True

                Select_Coordiante_name_ID = DX_COMMON_COORNAME.Item(0, Select_Row_Index).Value
                TX_Coordinate_Name_Common.Text = DX_COMMON_COORNAME.Item(1, Select_Row_Index).Value.ToString.Trim
                Select_Coordinate_Name_Common = DX_COMMON_COORNAME.Item(1, Select_Row_Index).Value.ToString.Trim
                Select_Coordinate_Name = Select_Coordinate_Name_Common
                READ_PROJECT_PARAMETER_FUN(Select_Product_Name, Select_Coordinate_Name_Common, DX_COMMON_COORDINATE, Coordinates_Operate.Read)

            End If
        Else
            DX_COMMON_COORDINATE.Columns.Clear()
        End If
    End Sub

    Private Sub Btn_New_System_COMMON_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Btn_New_System_COMMON.Click
        Coordinate_Axis_System(Coordinate.Add, Select_Coordinate_Name_Common, DX_COMMON_COORDINATE)
    End Sub

    Private Sub Btn_Updata_System_COMMON_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Btn_Updata_System_COMMON.Click
        Coordinate_Axis_System(Coordinate.updata, Select_Coordinate_Name_Common, DX_COMMON_COORDINATE)
        DX_COMMON_COORDINATE.Columns.Clear()
    End Sub

    Private Sub Btn_Updata_Speed_COMMON_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Btn_Updata_Speed_COMMON.Click
        If Login_Dialog.ShowDialog = Windows.Forms.DialogResult.OK Then
            SQLCON.Update_Speed(Select_Product_Name, Select_Coordinate_Name_Common, DI_AUTO_SPEED.Value)
            READ_PROJECT_PARAMETER_FUN(Select_Product_Name, Select_Coordinate_Name_Common, DX_COMMON_COORDINATE, Coordinates_Operate.Read)
        End If
    End Sub

    Private Sub Btn_Updata_Acc_COMMON_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Btn_Updata_Acc_COMMON.Click
        If Login_Dialog.ShowDialog = Windows.Forms.DialogResult.OK Then
            SQLCON.Update_ACC_DCC(Select_Product_Name, Select_Coordinate_Name_Common, DI_ACC.Value)
            READ_PROJECT_PARAMETER_FUN(Select_Product_Name, Select_Coordinate_Name_Common, DX_COMMON_COORDINATE, Coordinates_Operate.Read)
        End If
    End Sub

    Private Sub Btn_New_Pos_COMMON_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Btn_New_Pos_COMMON.Click
        READ_PROJECT_PARAMETER_FUN(Select_Product_Name, Select_Coordinate_Name_Common, DX_COMMON_COORDINATE, Coordinates_Operate.Add_Updata, ADD_UPDATA_Enum.Add)
        READ_PROJECT_PARAMETER_FUN(Select_Product_Name, Select_Coordinate_Name_Common, DX_COMMON_COORDINATE, Coordinates_Operate.Read)
    End Sub

    Private Sub Btn_Updata_Pos_COMMON_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Btn_Updata_Pos_COMMON.Click
        If F_ID = 0 Then
            If MessageBoxEx.Show("没有选中更新的坐标，是否更新所有【" & Select_Coordinate_Name_Common & "】的坐标？", "系统消息", MessageBoxButtons.YesNo, MessageBoxIcon.Information) = Windows.Forms.DialogResult.Yes Then
                READ_PROJECT_PARAMETER_FUN(Select_Product_Name, Select_Coordinate_Name_Common, DX_COMMON_COORDINATE, Coordinates_Operate.Add_Updata, ADD_UPDATA_Enum.Updata)
                READ_PROJECT_PARAMETER_FUN(Select_Product_Name, Select_Coordinate_Name_Common, DX_COMMON_COORDINATE, Coordinates_Operate.Read)

                Try
                    DX_COMMON_COORDINATE.FirstDisplayedScrollingRowIndex = SELECT_COORDINATES_ROW_INDEX
                    DX_COMMON_COORDINATE.Rows(SELECT_COORDINATES_ROW_INDEX).Selected = True
                Catch
                End Try
            End If
        Else
            READ_PROJECT_PARAMETER_FUN(Select_Product_Name, Select_Coordinate_Name_Common, DX_COMMON_COORDINATE, Coordinates_Operate.Add_Updata, ADD_UPDATA_Enum.Updata)
            READ_PROJECT_PARAMETER_FUN(Select_Product_Name, Select_Coordinate_Name_Common, DX_COMMON_COORDINATE, Coordinates_Operate.Read)

            Try
                DX_COMMON_COORDINATE.FirstDisplayedScrollingRowIndex = SELECT_COORDINATES_ROW_INDEX
                DX_COMMON_COORDINATE.Rows(SELECT_COORDINATES_ROW_INDEX).Selected = True
            Catch
            End Try
        End If
    End Sub

    Private Sub Btn_Delete_Pos_COMMON_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Btn_Delete_Pos_COMMON.Click
        If MessageBoxEx.Show("是否删除ID=" & F_ID & "的当前坐标？", "系统消息", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
            SQLCON.Delete_Coordinates(F_ID)
            READ_PROJECT_PARAMETER_FUN(Select_Product_Name, Select_Coordinate_Name_Common, DX_COMMON_COORDINATE, Coordinates_Operate.Read)
        Else
            READ_PROJECT_PARAMETER_FUN(Select_Product_Name, Select_Coordinate_Name_Common, DX_COMMON_COORDINATE, Coordinates_Operate.Read)
        End If
    End Sub

    Private Sub DX_COMMON_COORDINATE_CellClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DX_COMMON_COORDINATE.CellClick
        Try
            SELECT_COORDINATES_ROW_INDEX = e.RowIndex
            If SELECT_COORDINATES_ROW_INDEX >= 0 Then
                F_ID = DX_COMMON_COORDINATE.Item(DX_COMMON_COORDINATE.ColumnCount - 1, SELECT_COORDINATES_ROW_INDEX).Value
                GP_Coordinate_Common.Text = "坐标系 当前ID【" & F_ID & "】"
                If SQLCON.Cheng_Query_Coordinate_AxisSystem(Select_Product_Name, Select_Coordinate_Name_Common, Axis_Name_Str, Axis_Index_Str) = True Then
                    Work_Coordinatess_Obj.轴名称 = Split(Axis_Name_Str, ";")
                    Work_Coordinatess_Obj.轴运动顺序 = Split(Axis_Index_Str, ";")
                Else
                    Exit Sub
                End If

                If Work_Coordinatess_Obj.轴名称.Count >= 1 Then '轴数量至少一个以上
                    Array.Resize(Work_Coordinatess_Obj.轴位置, Work_Coordinatess_Obj.轴名称.Count)
                    For i As Int16 = 0 To Work_Coordinatess_Obj.轴名称.Count - 1
                        If IsDBNull(DX_COMMON_COORDINATE.Item(Work_Coordinatess_Obj.轴名称(i), SELECT_COORDINATES_ROW_INDEX).Value) = False Then '在修改坐标系添加新轴时处理有值为空的情况
                            Work_Coordinatess_Obj.轴位置(i) = DX_COMMON_COORDINATE.Item(Work_Coordinatess_Obj.轴名称(i), SELECT_COORDINATES_ROW_INDEX).Value
                        Else
                            For j As Integer = 0 To HomeParamObjArray.Length - 1
                                If Work_Coordinatess_Obj.轴名称(i) = HomeParamObjArray(j).轴名称 Then '如果为空，则把当前轴的对应坐标保存进去
                                    Work_Coordinatess_Obj.轴位置(i) = CType(DX_ENC_POS.Rows(j).Cells(2).Value, Double)
                                    Exit For
                                End If
                            Next
                        End If
                    Next
                    Work_Coordinatess_Obj.运行速度 = DX_COMMON_COORDINATE.Item("运行速度", SELECT_COORDINATES_ROW_INDEX).Value
                    Work_Coordinatess_Obj.加减速度 = DX_COMMON_COORDINATE.Item("加减速度", SELECT_COORDINATES_ROW_INDEX).Value
                    If IsDBNull(DX_COMMON_COORDINATE.Item("键名", SELECT_COORDINATES_ROW_INDEX).Value) = True Then
                        Work_Coordinatess_Obj.键名 = False
                    Else
                        Work_Coordinatess_Obj.键名 = DX_COMMON_COORDINATE.Item("键名", SELECT_COORDINATES_ROW_INDEX).Value
                    End If

                    If IsDBNull(DX_COMMON_COORDINATE.Item("点胶状态", SELECT_COORDINATES_ROW_INDEX).Value) = True Then
                        Work_Coordinatess_Obj.点胶状态 = False
                    Else
                        Work_Coordinatess_Obj.点胶状态 = DX_COMMON_COORDINATE.Item("点胶状态", SELECT_COORDINATES_ROW_INDEX).Value
                    End If
                    If IsDBNull(DX_COMMON_COORDINATE.Item("镭射触发状态", SELECT_COORDINATES_ROW_INDEX).Value) = True Then
                        Work_Coordinatess_Obj.镭射触发状态 = False
                    Else
                        Work_Coordinatess_Obj.镭射触发状态 = DX_COMMON_COORDINATE.Item("镭射触发状态", SELECT_COORDINATES_ROW_INDEX).Value
                    End If
                    If IsDBNull(DX_COMMON_COORDINATE.Item("镭射收数据状态", SELECT_COORDINATES_ROW_INDEX).Value) = True Then
                        Work_Coordinatess_Obj.镭射收数据状态 = False
                    Else
                        Work_Coordinatess_Obj.镭射收数据状态 = DX_COMMON_COORDINATE.Item("镭射收数据状态", SELECT_COORDINATES_ROW_INDEX).Value
                    End If
                    If IsDBNull(DX_COMMON_COORDINATE.Item("CCD触发状态", SELECT_COORDINATES_ROW_INDEX).Value) = True Then
                        Work_Coordinatess_Obj.CCD触发状态 = False
                    Else
                        Work_Coordinatess_Obj.CCD触发状态 = DX_COMMON_COORDINATE.Item("CCD触发状态", SELECT_COORDINATES_ROW_INDEX).Value
                    End If
                    If IsDBNull(DX_COMMON_COORDINATE.Item("IO触发状态", SELECT_COORDINATES_ROW_INDEX).Value) = True Then
                        Work_Coordinatess_Obj.IO触发状态 = False
                    Else
                        Work_Coordinatess_Obj.IO触发状态 = DX_COMMON_COORDINATE.Item("IO触发状态", SELECT_COORDINATES_ROW_INDEX).Value
                    End If
                    If IsDBNull(DX_COMMON_COORDINATE.Item("其他触发状态", SELECT_COORDINATES_ROW_INDEX).Value) = True Then
                        Work_Coordinatess_Obj.其他触发状态 = False
                    Else
                        Work_Coordinatess_Obj.其他触发状态 = DX_COMMON_COORDINATE.Item("其他触发状态", SELECT_COORDINATES_ROW_INDEX).Value
                    End If
                    Work_Coordinatess_Obj.慢速 = Work_Coordinatess_Obj.运行速度 * 0.1
                    Work_Coordinatess_Obj.中速 = Work_Coordinatess_Obj.运行速度 * 0.5

                    SW_KEY_NAME.Text = Work_Coordinatess_Obj.键名.Trim
                    DI_AUTO_SPEED.Value = Work_Coordinatess_Obj.运行速度
                    DI_ACC.Value = Work_Coordinatess_Obj.加减速度
                    SW_DISPENSING.Value = Work_Coordinatess_Obj.点胶状态
                    SW_LASER.Value = Work_Coordinatess_Obj.镭射触发状态
                    SW_READ_LASER.Value = Work_Coordinatess_Obj.镭射收数据状态
                    SW_CCD.Value = Work_Coordinatess_Obj.CCD触发状态
                    SW_IO.Value = Work_Coordinatess_Obj.IO触发状态
                    SW_OTHER.Value = Work_Coordinatess_Obj.其他触发状态
                End If
            Else
                SELECT_TABLE_NAME = Nothing
            End If
        Catch ex As Exception
            MessageBoxEx.Show(ex.ToString, "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Try
    End Sub

    Private Sub Button_Copy_Product_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Select Case Login_Dialog.ShowDialog
            Case Windows.Forms.DialogResult.Retry
                SQLCON.Delete_Project(Select_Product_Name)
                SQLCON.Read_Project_Name(DX_PROJECT_NAME)
            Case Windows.Forms.DialogResult.No
                MessageBoxEx.Show("密码错误！", "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Stop, MessageBoxDefaultButton.Button1)
        End Select
    End Sub

    Private Sub CM_Copy_Project_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CM_Copy_Project.Click
        If SQLCON.Exists_Project_Name(TextBox_Project_Name.Text.Trim) = False Then
            If TextBox_Project_Name.Text.Trim.Length > 0 Then
                Select Case Login_Dialog.ShowDialog
                    Case Windows.Forms.DialogResult.OK
                        '插入项目名称
                        Array.Resize(PARAMETERS_NAME_ARRAY, 5)
                        Array.Resize(PARAMETERS_VALUE_Object, 5)
                        Array.Clear(PARAMETERS_NAME_ARRAY, 0, PARAMETERS_NAME_ARRAY.Length)
                        Array.Clear(PARAMETERS_VALUE_Object, 0, PARAMETERS_NAME_ARRAY.Length)
                        PARAMETERS_NAME_ARRAY(0) = "项目名称"
                        PARAMETERS_NAME_ARRAY(1) = "激光程式号"
                        PARAMETERS_NAME_ARRAY(2) = "影像程式号"
                        PARAMETERS_NAME_ARRAY(3) = "公差上限"
                        PARAMETERS_NAME_ARRAY(4) = "公差下限"
                        If IsNumeric(TextBox_Project_Name.Text.Trim) = True Then
                            MessageBoxEx.Show("项目名称请增加字母加以区分！", "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Stop, MessageBoxDefaultButton.Button1)
                            Exit Sub
                        End If
                        PARAMETERS_VALUE_Object(0) = TextBox_Project_Name.Text.Trim
                        PARAMETERS_VALUE_Object(1) = IntegerInput_laser_program.Value
                        PARAMETERS_VALUE_Object(2) = ""
                        PARAMETERS_VALUE_Object(3) = D_Limit_Up.Value
                        PARAMETERS_VALUE_Object(4) = D_Limit_Down.Value
                        SQLCON.Create_Project(PARAMETERS_NAME_ARRAY, PARAMETERS_VALUE_Object)

                        SQLCON.Copy_Project(Select_Product_Name, TextBox_Project_Name.Text.Trim)

                        SQLCON.Read_Project_Name(DX_PROJECT_NAME)
                        SQLCON.Read_Coordinate_Name(Select_Product_Name, STATION.LEFT, COORDINATENAME.LEFT_DATASET, DX_LEFT_COORNAME)  '读取坐标名称
                        SQLCON.Read_Coordinate_Name(Select_Product_Name, STATION.RIGHT, COORDINATENAME.RIGHT_DATASET, DX_RIGHT_COORNAME)  '读取坐标名称
                        MessageBoxEx.Show("项目名称复制完成！", "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1)

                    Case Windows.Forms.DialogResult.No
                        MessageBoxEx.Show("密码错误，请重试！", "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Stop, MessageBoxDefaultButton.Button1)
                End Select
            Else
                MessageBoxEx.Show("请输入正确的项目参数，并且项目名称不能为空！", "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Stop, MessageBoxDefaultButton.Button1)
            End If
        Else
            MessageBoxEx.Show("指定新项目名称已经存在，请另外指定！", "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Stop, MessageBoxDefaultButton.Button1)
        End If
    End Sub

    Private Sub CM_Updata_Project_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CM_Updata_Project.Click
        If MessageBoxEx.Show("是否修改参数？请记住现有参数！", "系统消息", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
            If IsNumeric(TextBox_Project_Name.Text.Trim) = True Then
                MessageBoxEx.Show("项目名称请增加字母加以区分！", "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Stop, MessageBoxDefaultButton.Button1)
                Exit Sub
            End If
            '更新项目信息
            Array.Resize(PARAMETERS_NAME_ARRAY, 4)
            Array.Resize(PARAMETERS_VALUE_Object, 4)
            Array.Clear(PARAMETERS_NAME_ARRAY, 0, PARAMETERS_NAME_ARRAY.Length)
            Array.Clear(PARAMETERS_VALUE_Object, 0, PARAMETERS_VALUE_Object.Length)
            PARAMETERS_NAME_ARRAY(0) = "激光程式号"
            PARAMETERS_NAME_ARRAY(1) = "影像程式号"
            PARAMETERS_NAME_ARRAY(2) = "公差上限"
            PARAMETERS_NAME_ARRAY(3) = "公差下限"
            PARAMETERS_VALUE_Object(0) = IntegerInput_laser_program.Value
            PARAMETERS_VALUE_Object(1) = IntegerInput_laser_program.Value
            PARAMETERS_VALUE_Object(2) = D_Limit_Up.Value
            PARAMETERS_VALUE_Object(3) = D_Limit_Down.Value
            SQLCON.Update_Project_Parameter(Select_Product_Name, PARAMETERS_NAME_ARRAY, PARAMETERS_VALUE_Object)
            If Select_Product_Name <> TextBox_Project_Name.Text.Trim Then
                SQLCON.Update_Project_Parameter(Select_Product_Name, TextBox_Project_Name.Text.Trim)
                Select_Product_Name = TextBox_Project_Name.Text.Trim
            End If

            SQLCON.Read_Coordinate_Name(Select_Product_Name, STATION.LEFT, COORDINATENAME.LEFT_DATASET, DX_LEFT_COORNAME)  '读取坐标名称
            SQLCON.Read_Coordinate_Name(Select_Product_Name, STATION.RIGHT, COORDINATENAME.RIGHT_DATASET, DX_RIGHT_COORNAME)  '读取坐标名称

            MessageBoxEx.Show("项目参数修改完成！", "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1)
            SQLCON.Read_Project_Name(DX_PROJECT_NAME)
        End If
    End Sub

    Private Sub Button_Updata_Pos_Hand_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Updata_Pos_Hand.Click
        If F_ID <> 0 Then
            If Login_Dialog.ShowDialog = Windows.Forms.DialogResult.OK Then
                If MessageBoxEx.Show("是否确定修改 " & Select_Product_Name & " " & Select_Coordinate_Name_LEFT & " 当前ID=【" & F_ID & "】的坐标？", "系统消息", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1) = Windows.Forms.DialogResult.Yes Then

                    Select Case SuperTabItem_Index
                        Case Select_Coordinate.Left
                            READ_PROJECT_PARAMETER_FUN(Select_Product_Name, Select_Coordinate_Name_LEFT, DX_LEFT_COORDINATE, Coordinates_Operate.Add_Updata, ADD_UPDATA_Enum.Updata_Single)
                            READ_PROJECT_PARAMETER_FUN(Select_Product_Name, Select_Coordinate_Name_LEFT, DX_LEFT_COORDINATE, Coordinates_Operate.Read)
                            Try
                                DX_LEFT_COORDINATE.FirstDisplayedScrollingRowIndex = SELECT_COORDINATES_ROW_INDEX
                                DX_LEFT_COORDINATE.Rows(SELECT_COORDINATES_ROW_INDEX).Selected = True
                            Catch
                            End Try
                        Case Select_Coordinate.Right
                            READ_PROJECT_PARAMETER_FUN(Select_Product_Name, Select_Coordinate_Name_RIGHT, DX_RIGHT_COORDINATE, Coordinates_Operate.Add_Updata, ADD_UPDATA_Enum.Updata_Single)
                            READ_PROJECT_PARAMETER_FUN(Select_Product_Name, Select_Coordinate_Name_RIGHT, DX_RIGHT_COORDINATE, Coordinates_Operate.Read)
                            Try
                                DX_RIGHT_COORDINATE.FirstDisplayedScrollingRowIndex = SELECT_COORDINATES_ROW_INDEX
                                DX_RIGHT_COORDINATE.Rows(SELECT_COORDINATES_ROW_INDEX).Selected = True
                            Catch
                            End Try
                        Case Select_Coordinate.Common
                            READ_PROJECT_PARAMETER_FUN(Select_Product_Name, Select_Coordinate_Name_Common, DX_COMMON_COORDINATE, Coordinates_Operate.Add_Updata, ADD_UPDATA_Enum.Updata_Single)
                            READ_PROJECT_PARAMETER_FUN(Select_Product_Name, Select_Coordinate_Name_Common, DX_COMMON_COORDINATE, Coordinates_Operate.Read)
                            Try
                                DX_COMMON_COORDINATE.FirstDisplayedScrollingRowIndex = SELECT_COORDINATES_ROW_INDEX
                                DX_COMMON_COORDINATE.Rows(SELECT_COORDINATES_ROW_INDEX).Selected = True
                            Catch
                            End Try
                    End Select
                End If
            End If
        Else
            MessageBoxEx.Show("ID为空，没有选中当前需要修改的坐标！", "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button3)
        End If
    End Sub

    Private Sub DX_LEFT_COORDINATE_DataError(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewDataErrorEventArgs) Handles DX_LEFT_COORDINATE.DataError
        MessageBoxEx.Show("输入格式不正确，请输入数字！", "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button2)
    End Sub

    Private Sub DX_RIGHT_COORDINATE_DataError(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewDataErrorEventArgs) Handles DX_RIGHT_COORDINATE.DataError
        MessageBoxEx.Show("输入格式不正确，请输入数字！", "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button2)
    End Sub

    Private Sub DX_COMMON_COORDINATE_DataError(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewDataErrorEventArgs) Handles DX_COMMON_COORDINATE.DataError
        MessageBoxEx.Show("输入格式不正确，请输入数字！", "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button2)
    End Sub

    Private Sub SuperTabItem_Comm_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SuperTabItem_Comm.Click
        SuperTabItem_Index = Select_Coordinate.Common
    End Sub

    Private Sub SuperTabItem_Right_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SuperTabItem_Right.Click
        SuperTabItem_Index = Select_Coordinate.Right
    End Sub

    Private Sub SuperTabItem_Left_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SuperTabItem_Left.Click
        SuperTabItem_Index = Select_Coordinate.Left
    End Sub

    Private Sub CM_cancel_CoorName_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CM_cancel_CoorName.Click
        Int_Copy_Coor_Name = 0
        Int_Past_Coor_Name = 1
    End Sub

    Private Sub CM_Copy_CoorName_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CM_Copy_CoorName.Click
        Select Case SuperTabItem_Index
            Case Select_Coordinate.Left
                If TX_Coordinate_Name_L.Text <> "" Then
                    Int_Copy_Coor_Name = 1
                    Copy_Coor_Station = STATION.LEFT
                    Copy_Coor_Name = Select_Coordinate_Name_LEFT
                Else
                    MessageBoxEx.Show("坐标名称不能为空！", "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button3)
                End If
            Case Select_Coordinate.Right
                If TX_Coordinate_Name_R.Text <> "" Then
                    Int_Copy_Coor_Name = 1
                    Copy_Coor_Station = STATION.RIGHT
                    Copy_Coor_Name = Select_Coordinate_Name_RIGHT
                Else
                    MessageBoxEx.Show("坐标名称不能为空！", "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button3)
                End If
            Case Select_Coordinate.Common
                If TX_Coordinate_Name_Common.Text <> "" Then
                    Int_Copy_Coor_Name = 1
                    Copy_Coor_Station = STATION.COMMON
                    Copy_Coor_Name = Select_Coordinate_Name_Common
                Else
                    MessageBoxEx.Show("坐标名称不能为空！", "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button3)
                End If
        End Select
    End Sub

    Private Sub CM_Past_CoorName_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CM_Past_CoorName.Click
        Int_Past_Coor_Name = 0
        If Int_Copy_Coor_Name = 1 And Int_Past_Coor_Name = 0 Then
            Int_Copy_Coor_Name = 0
            Int_Past_Coor_Name = 1
            Select Case SuperTabItem_Index
                Case Select_Coordinate.Left
                    Past_Coor_Station = STATION.LEFT
                Case Select_Coordinate.Right
                    Past_Coor_Station = STATION.RIGHT
                Case Select_Coordinate.Common
                    Past_Coor_Station = STATION.COMMON
            End Select
            If MessageBoxEx.Show("是否把【" & Copy_Coor_Station & "】的【" & Copy_Coor_Name & "】复制粘贴到【" & Past_Coor_Station & "】？", "系统消息", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button3) = Windows.Forms.DialogResult.Yes Then

                SQLCON.Copy_Coordinate_Name(Select_Product_Name, Copy_Coor_Station, Past_Coor_Station, Copy_Coor_Name, PastIndex)
                '次数保存
                PastIndex += 1

                Array.Resize(PARAMETERS_NAME_ARRAY, 1)
                Array.Resize(PARAMETERS_VALUE_Object, 1)
                Array.Clear(PARAMETERS_NAME_ARRAY, 0, PARAMETERS_NAME_ARRAY.Length)
                Array.Clear(PARAMETERS_VALUE_Object, 0, PARAMETERS_VALUE_Object.Length)
                PARAMETERS_NAME_ARRAY(0) = "坐标复制次数"
                PARAMETERS_VALUE_Object(0) = PastIndex
                If SQLCON.Update_Project_Parameter_Left(PARAMETERS_NAME_ARRAY, PARAMETERS_VALUE_Object) = True Then
                    MessageBoxEx.Show("坐标名称复制完成！", "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button3)
                    Select Case SuperTabItem_Index
                        Case Select_Coordinate.Left
                            SQLCON.Read_Coordinate_Name(Select_Product_Name, STATION.LEFT, COORDINATENAME.LEFT_DATASET, DX_LEFT_COORNAME)  '读取坐标名称
                            DX_LEFT_COORDINATE.Columns.Clear()
                        Case Select_Coordinate.Right
                            SQLCON.Read_Coordinate_Name(Select_Product_Name, STATION.RIGHT, COORDINATENAME.RIGHT_DATASET, DX_RIGHT_COORNAME)  '读取坐标名称
                            DX_RIGHT_COORDINATE.Columns.Clear()
                        Case Select_Coordinate.Common
                            SQLCON.Read_Coordinate_Name(Select_Product_Name, STATION.COMMON, COORDINATENAME.COMMON_DATASET, DX_COMMON_COORNAME)  '读取坐标名称
                            DX_COMMON_COORDINATE.Columns.Clear()
                    End Select
                End If
            End If
        End If
    End Sub

    Private Sub CM_Add_CoorName_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CM_Add_CoorName.Click
        Select Case SuperTabItem_Index
            Case Select_Coordinate.Left
                New_Coordinate_Name(CM_Add_CoorName, TX_Coordinate_Name_L, DX_LEFT_COORNAME, Select_Coordinate_Name_LEFT, STATION.LEFT)
            Case Select_Coordinate.Right
                New_Coordinate_Name(CM_Add_CoorName, TX_Coordinate_Name_R, DX_RIGHT_COORNAME, Select_Coordinate_Name_RIGHT, STATION.RIGHT)
            Case Select_Coordinate.Common
                New_Coordinate_Name(CM_Add_CoorName, TX_Coordinate_Name_Common, DX_COMMON_COORNAME, Select_Coordinate_Name_Common, STATION.COMMON)
        End Select
    End Sub

    Private Sub CM_Delete_CoorName_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CM_Delete_CoorName.Click
        If MessageBoxEx.Show("是否删除坐标名称，删除前请确认被删除坐标是否有用！", "系统消息", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
            Select Case Login_Dialog.ShowDialog
                Case Windows.Forms.DialogResult.Retry
                    Select Case SuperTabItem_Index
                        Case Select_Coordinate.Left
                            SQLCON.Delete_Coordiante(Select_Product_Name, Select_Coordinate_Name_LEFT)
                            SQLCON.Read_Coordinate_Name(Select_Product_Name, STATION.LEFT, COORDINATENAME.LEFT_DATASET, DX_LEFT_COORNAME)  '读取坐标名称
                            DX_LEFT_COORDINATE.Columns.Clear()
                        Case Select_Coordinate.Right
                            SQLCON.Delete_Coordiante(Select_Product_Name, Select_Coordinate_Name_RIGHT)
                            SQLCON.Read_Coordinate_Name(Select_Product_Name, STATION.RIGHT, COORDINATENAME.RIGHT_DATASET, DX_RIGHT_COORNAME)  '读取坐标名称
                            DX_RIGHT_COORDINATE.Columns.Clear()
                        Case Select_Coordinate.Common
                            SQLCON.Delete_Coordiante(Select_Product_Name, Select_Coordinate_Name_Common)
                            SQLCON.Read_Coordinate_Name(Select_Product_Name, STATION.COMMON, COORDINATENAME.COMMON_DATASET, DX_COMMON_COORNAME)  '读取坐标名称
                            DX_COMMON_COORDINATE.Columns.Clear()
                    End Select
                Case Windows.Forms.DialogResult.No
                    MessageBoxEx.Show("密码错误！删除坐标后再新建坐标调试时间会很长，会造成产线停线，请慎重！", "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Stop, MessageBoxDefaultButton.Button1)
            End Select
        End If
    End Sub


    '坐标点
    Public Structure PointStc
        Dim X As Double
        Dim Y As Double
        Dim X_NAME As String
        Dim Y_NAME As String
    End Structure
    Private Sub Btn_Auto_InsertPos_Click(sender As System.Object, e As System.EventArgs) Handles Btn_Auto_InsertPos.Click

        If Auto_InsertPos_Dialog.ShowDialog() = Windows.Forms.DialogResult.Yes Then

            Dim X_Len As Integer, Y_Len As Integer
            If AutoInsertPosParam.行数 = 1 Or AutoInsertPosParam.列数 = 1 Then
                X_Len = AutoInsertPosParam.列间距
                Y_Len = AutoInsertPosParam.行间距
            Else
                X_Len = AutoInsertPosParam.行间距
                Y_Len = AutoInsertPosParam.列间距
            End If
            If F_ID <> 0 Then
                If MessageBoxEx.Show("是否自动生成首点ID为【" & F_ID & "】的所有坐标？", "系统消息", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1) = Windows.Forms.DialogResult.Yes Then
                    If Work_Coordinatess_Obj.轴位置.Count >= 2 Then '多行
                        Dim Array_Pos() As PointStc = Nothing

                        Dim Org_X As Double = Work_Coordinatess_Obj.轴位置(AutoInsertPosParam.行轴)
                        Dim Org_Y As Double = Work_Coordinatess_Obj.轴位置(AutoInsertPosParam.列轴)
                        Dim Index As Integer = 1
                        For m As Integer = 0 To AutoInsertPosParam.行数 - 1
                            For n As Integer = 0 To AutoInsertPosParam.列数 - 1
                                Array.Resize(Array_Pos, Index)
                                Array_Pos(Index - 1).X = Org_X + X_Len * n
                                Array_Pos(Index - 1).Y = Org_Y + Y_Len * m
                                Array_Pos(Index - 1).X_NAME = AutoInsertPosParam.行轴名称
                                Array_Pos(Index - 1).Y_NAME = AutoInsertPosParam.列轴名称
                                If m = 0 And n = 0 Then
                                    '不做任何操作
                                Else
                                    Select Case SuperTabItem_Index
                                        Case Select_Coordinate.Left
                                            READ_PROJECT_PARAMETER_FUN(Select_Product_Name, Select_Coordinate_Name_LEFT, DX_LEFT_COORDINATE, Coordinates_Operate.Add_Updata, ADD_UPDATA_Enum.Add_Auto, Array_Pos(Index - 1))
                                        Case Select_Coordinate.Right
                                            READ_PROJECT_PARAMETER_FUN(Select_Product_Name, Select_Coordinate_Name_RIGHT, DX_RIGHT_COORDINATE, Coordinates_Operate.Add_Updata, ADD_UPDATA_Enum.Add_Auto, Array_Pos(Index - 1))
                                        Case Select_Coordinate.Common
                                            READ_PROJECT_PARAMETER_FUN(Select_Product_Name, Select_Coordinate_Name_Common, DX_COMMON_COORDINATE, Coordinates_Operate.Add_Updata, ADD_UPDATA_Enum.Add_Auto, Array_Pos(Index - 1))
                                    End Select
                                End If
                                Index += 1
                            Next
                        Next
                        Select Case SuperTabItem_Index
                            Case Select_Coordinate.Left
                                READ_PROJECT_PARAMETER_FUN(Select_Product_Name, Select_Coordinate_Name_LEFT, DX_LEFT_COORDINATE, Coordinates_Operate.Read)
                            Case Select_Coordinate.Right
                                READ_PROJECT_PARAMETER_FUN(Select_Product_Name, Select_Coordinate_Name_RIGHT, DX_RIGHT_COORDINATE, Coordinates_Operate.Read)
                            Case Select_Coordinate.Common
                                READ_PROJECT_PARAMETER_FUN(Select_Product_Name, Select_Coordinate_Name_Common, DX_COMMON_COORDINATE, Coordinates_Operate.Read)
                        End Select
                    ElseIf Work_Coordinatess_Obj.轴位置.Count = 1 Then '只有单行
                        Dim Array_Pos() As PointStc = Nothing
                        Dim Org_X As Double = Work_Coordinatess_Obj.轴位置(AutoInsertPosParam.行轴)
                        Dim Index As Integer = 1
                        For m As Integer = 0 To AutoInsertPosParam.行数 - 1
                            Array.Resize(Array_Pos, Index)
                            Array_Pos(Index - 1).X = Org_X + X_Len * m
                            Array_Pos(Index - 1).X_NAME = AutoInsertPosParam.行轴名称
                            If m = 0 Then
                                '不做任何操作
                            Else
                                Select Case SuperTabItem_Index
                                    Case Select_Coordinate.Left
                                        READ_PROJECT_PARAMETER_FUN(Select_Product_Name, Select_Coordinate_Name_LEFT, DX_LEFT_COORDINATE, Coordinates_Operate.Add_Updata, ADD_UPDATA_Enum.Add_Auto, Array_Pos(Index - 1))
                                    Case Select_Coordinate.Right
                                        READ_PROJECT_PARAMETER_FUN(Select_Product_Name, Select_Coordinate_Name_RIGHT, DX_RIGHT_COORDINATE, Coordinates_Operate.Add_Updata, ADD_UPDATA_Enum.Add_Auto, Array_Pos(Index - 1))
                                    Case Select_Coordinate.Common
                                        READ_PROJECT_PARAMETER_FUN(Select_Product_Name, Select_Coordinate_Name_Common, DX_COMMON_COORDINATE, Coordinates_Operate.Add_Updata, ADD_UPDATA_Enum.Add_Auto, Array_Pos(Index - 1))
                                End Select
                            End If
                            Index += 1
                        Next
                        Select Case SuperTabItem_Index
                            Case Select_Coordinate.Left
                                READ_PROJECT_PARAMETER_FUN(Select_Product_Name, Select_Coordinate_Name_LEFT, DX_LEFT_COORDINATE, Coordinates_Operate.Read)
                            Case Select_Coordinate.Right
                                READ_PROJECT_PARAMETER_FUN(Select_Product_Name, Select_Coordinate_Name_RIGHT, DX_RIGHT_COORDINATE, Coordinates_Operate.Read)
                            Case Select_Coordinate.Common
                                READ_PROJECT_PARAMETER_FUN(Select_Product_Name, Select_Coordinate_Name_Common, DX_COMMON_COORDINATE, Coordinates_Operate.Read)
                        End Select
                    End If
                End If
            Else
                MessageBoxEx.Show("没有选中要自动生成的首点坐标，请先选中首点坐标！", "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1)
            End If
        End If

    End Sub

    Private Sub Button_Clear_All_CoorNamePos_Click(sender As System.Object, e As System.EventArgs) Handles Button_Clear_All_CoorNamePos.Click
        If Login_Dialog.ShowDialog = Windows.Forms.DialogResult.Retry Then
            Dim Select_Coordinate_Name As String = Nothing
            Select Case SuperTabItem_Index
                Case Select_Coordinate.Left
                    Select_Coordinate_Name = Select_Coordinate_Name_LEFT
                Case Select_Coordinate.Right
                    Select_Coordinate_Name = Select_Coordinate_Name_RIGHT
                Case Select_Coordinate.Common
                    Select_Coordinate_Name = Select_Coordinate_Name_Common
            End Select
            If MessageBoxEx.Show("是否确定清空当前坐标名称=【" & Select_Coordinate_Name & "】的坐标？", "系统消息", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1) = Windows.Forms.DialogResult.Yes Then
                SQLCON.Clear_All_CoorName_Pos(Select_Product_Name, Select_Coordinate_Name)
                Select Case SuperTabItem_Index
                    Case Select_Coordinate.Left
                        READ_PROJECT_PARAMETER_FUN(Select_Product_Name, Select_Coordinate_Name_LEFT, DX_LEFT_COORDINATE, Coordinates_Operate.Read)
                    Case Select_Coordinate.Right
                        READ_PROJECT_PARAMETER_FUN(Select_Product_Name, Select_Coordinate_Name_RIGHT, DX_RIGHT_COORDINATE, Coordinates_Operate.Read)
                    Case Select_Coordinate.Common
                        READ_PROJECT_PARAMETER_FUN(Select_Product_Name, Select_Coordinate_Name_Common, DX_COMMON_COORDINATE, Coordinates_Operate.Read)
                End Select
            End If
        End If
    End Sub

    Private Sub Button_Updata_Single_Col_Pos_Click(sender As System.Object, e As System.EventArgs) Handles Button_Updata_Single_Col_Pos.Click
        If Select_Coordinate_Name <> "" Then
            If Login_Dialog.ShowDialog = Windows.Forms.DialogResult.OK Then
                If Updata_ColPos_Dialog.ShowDialog = Windows.Forms.DialogResult.OK Then
                    Select Case SuperTabItem_Index
                        Case Select_Coordinate.Left
                            DX_LEFT_COORDINATE.Columns.Clear()
                        Case Select_Coordinate.Right
                            DX_RIGHT_COORDINATE.Columns.Clear()
                        Case Select_Coordinate.Common
                            DX_COMMON_COORDINATE.Columns.Clear()
                    End Select
                End If
            End If
        Else
            MessageBoxEx.Show("没有选定坐标名称！", "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button3)
        End If
    End Sub

    Private Sub Btn_Save_Click(sender As System.Object, e As System.EventArgs) Handles Btn_Save.Click
        '更新项目信息
        Array.Resize(PARAMETERS_NAME_ARRAY, 8)
        Array.Resize(PARAMETERS_VALUE_Object, 8)
        Array.Clear(PARAMETERS_NAME_ARRAY, 0, PARAMETERS_NAME_ARRAY.Length)
        Array.Clear(PARAMETERS_VALUE_Object, 0, PARAMETERS_VALUE_Object.Length)
        PARAMETERS_NAME_ARRAY(0) = "激光程式号"
        PARAMETERS_NAME_ARRAY(1) = "影像程式号"
        PARAMETERS_NAME_ARRAY(2) = "公差上限"
        PARAMETERS_NAME_ARRAY(3) = "公差下限"
        PARAMETERS_NAME_ARRAY(4) = "镭射公差上限"
        PARAMETERS_NAME_ARRAY(5) = "镭射公差下限"
        PARAMETERS_NAME_ARRAY(6) = "功能键公差上限"
        PARAMETERS_NAME_ARRAY(7) = "功能键公差下限"
        PARAMETERS_VALUE_Object(0) = IntegerInput_laser_program.Value
        PARAMETERS_VALUE_Object(1) = IntegerInput_laser_program.Value
        PARAMETERS_VALUE_Object(2) = D_Limit_Up.Value
        PARAMETERS_VALUE_Object(3) = D_Limit_Down.Value
        PARAMETERS_VALUE_Object(4) = Laser_Limit_Up.Value
        PARAMETERS_VALUE_Object(5) = Laser_Limit_Down.Value
        PARAMETERS_VALUE_Object(6) = D_Limit_Up_F.Value
        PARAMETERS_VALUE_Object(7) = D_Limit_Down_F.Value
        If SQLCON.Update_Project_Parameter(Select_Product_Name, PARAMETERS_NAME_ARRAY, PARAMETERS_VALUE_Object) = True Then
            MessageBoxEx.Show("参数保存完成", "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2)
        End If

    End Sub


    Private Sub DX_LASER_DataError(sender As System.Object, e As System.Windows.Forms.DataGridViewDataErrorEventArgs)
        MessageBoxEx.Show("输入格式不正确，请输入数字！", "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button2)
    End Sub
End Class
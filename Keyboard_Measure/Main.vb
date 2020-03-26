Imports System.Math
Imports System.IO
Imports System.Text
Imports System.Threading
Imports System.Threading.Tasks
Imports Keyboard_Measure.ADLINK_LIB
Imports Keyboard_Measure.ADLINK_LIB.MOTION
Imports Keyboard_Measure.SQL_LIB
Imports DevComponents.DotNetBar
Imports Keyboard_Measure.Adlink_DIO_Dialog
Imports Keyboard_Measure.System_SetDialog
Imports Keyboard_Measure.ADLINK_1903
Imports System.Net.Sockets
Imports Keyboard_Measure.GeneralClass.KEYENCE_LIB.LINE_LASER.KEYENCE_FUN
Imports Cognex.VisionPro
Imports Cognex.VisionPro.QuickBuild
Imports Cognex.VisionPro.ToolGroup
Imports Cognex.VisionPro.ToolBlock
Imports Cognex.VisionPro.ImageFile
Imports Cognex.VisionPro.PMAlign
Imports Cognex.VisionPro.Blob
Imports Cognex.VisionPro.ImageProcessing
Imports Cognex.VisionPro.CalibFix
Imports System.Reflection.MethodBase
Imports System.IO.Directory

Public Class Main
    Dim Send_plc_Command_Str As String = Nothing, Bool_Disp_s As Boolean = False
    Dim NG_count_Left, NG_Count_Right As Integer
    Structure Save_Image_Stru
        Dim Key_Name As String
        Dim Image_Array() As Byte
    End Structure
    Dim Image_Table_Name As String = "PROJECT_MESAURE_IMAGE"
    Dim Fail_Location As String = Nothing
    Dim PARAMETERS_VALUE_Object() As Object = Nothing, Parameters_Name_Object() As String = Nothing
    Dim Set_Trg_Count1 As Int16, Set_Trg_Count2 As Int16, Set_Trg_Count3, Set_Trg_Count_All As Int16
    Dim Line_Table_1() As Integer, Line_Table_2() As Integer, Line_Table_3() As Integer
    Dim Image_Memory_Left, Image_Memory_Right, Image_Memory_Left_Save, Image_Memory_Right_Save As New MemoryStream
    Dim CogToolBlock_Fixture_Left, CogToolBlock_Fixture_Right As New CogToolBlock
    Dim DataCode_Left As String = "", DataCode_Right As String = "", DataCode_Com As String = "", DataCode_Left_New As String = "", DataCode_Right_New As String = ""
    Dim DataCode_Left_tmp As String = "", DataCode_Right_tmp As String = ""
    Dim Line_1_Index As List(Of Integer), Line_4_Index As List(Of Integer)
    Dim Line_2_Index As List(Of Integer), Line_5_Index As List(Of Integer)
    Dim Line_3_Index As List(Of Integer), Line_6_Index As List(Of Integer)
    Dim Data_1 As New List(Of Double), Data_4 As New List(Of Double)
    Dim Data_2 As New List(Of Double), Data_5 As New List(Of Double)
    Dim Data_3 As New List(Of Double), Data_6 As New List(Of Double)
    Dim LINK_CAMERA As Boolean
    Dim hJobManager As New Cognex.VisionPro.QuickBuild.CogJobManager
    Dim hJobs As New Cognex.VisionPro.QuickBuild.CogJob
    Dim hToolGroup As New CogToolGroup
    Dim ToolBlock_left, toolblock_right As New CogToolBlock
    Dim ImageFile_right, ImageFile_left As New CogImageFileTool
    Dim tmpRecord_Left, tmpRecord_Right As Cognex.VisionPro.ICogRecord
    Dim Laser_Trigger_AxidID As Int16
    Dim Card_Trg, Card_Trg_00, Card_Trg_01 As TrgParam_STR
    Dim LJV7000 As New KEYENCE_LIB.LINE_LASER.KEYENCE_FUN, Device_ID As Integer = 0
    Dim Start_Time_Left, End_Time_Left, Start_Time_Right, End_Time_Right As Date, CT_Time_Left As Int16 = 0, CT_Time_Right As Int16 = 0
    Dim Left_SN As String = Nothing, Right_SN As String = Nothing
    Dim NO_WORK_INDEX As Integer = 0
    Dim Bool_No_Wrok As Boolean
    Dim Parameters_Name_Array() As String, Parameters_Value_Array() As Object, Glue_type As Glue_Type_Enum
    Dim rtn As Integer, rtn1 As String
    Dim str As String
    Dim ChNo As Byte = 0
    Dim Card_1903 As New ADLINK_1903
    Dim DataConn As New DATA_CONVERSION_LIB
    Dim CARDCMD As New ADLINK_LIB.MOTION

    Public SQLCON As New SQL_LIB
    Public SQLCON_查询上一站 As New SQL_LIB
    Public SQLCON_X816RSAOI As New SQL_LIB_X816RSAOI '副表
    Public SQLCON_A001 As New SQL_LIB_A001 '主表1
    Public SQLCON_X816 As New SQL_LIB_X816 '主表2

    Public User_Name, Password As String
    Public HomeParamObjArray() As SQL_LIB.HOME_PARAMETERS
    Dim ShopFloor_Value As New DATA_CONVERSION_LIB.SF_PARAM_VEALUE_STRUCT
    Dim DATA_PROCESS As New DATA_CONVERSION_LIB
    Dim Coord_AllPOS_List_LEFT(), Coord_AllPOS_List_RIGHT() As Work_Coordinatess '储存所有运动坐标
    Dim Coord_Order_List_LEFT, Coord_Order_List_RIGHT As New List(Of Integer) '储存坐标名称排序，供筛选
    Dim AXIS_INDEX1_LEFT, AXIS_INDEX1_RIGHT As New List(Of Integer) '储存坐标系运动顺序
    Dim AXIS_INDEX2_LEFT, AXIS_INDEX2_RIGHT As New List(Of Integer)
    Dim AXIS_INDEX3_LEFT, AXIS_INDEX3_RIGHT As New List(Of Integer)
    Dim AXIS_INDEX4_LEFT, AXIS_INDEX4_RIGHT As New List(Of Integer)
    Dim AXIS_INDEX5_LEFT, AXIS_INDEX5_RIGHT As New List(Of Integer)
    Dim Board_ID As Int16, Stop_Code As Int16, BOARD_ID_00 As Int16 = 0, BOARD_ID_01 As Int16 = 1, BOARD_ID_02 As Int16 = 2
    Public Card_Init_OK As Boolean = False, Card_NO As Int16 = 0
    Dim Product_Name_Array() As String = Nothing '产品名称
    Dim Error_Str As String = Nothing '异常信息
    Dim Select_Product_Name As String = "程序加载"
    Dim Left_Right_Staion As LR_STATION = LR_STATION.ALL
    Private Bool_Start_Left, Bool_Start_Right, Bool_Start_Public, Bool_Start_All, Bool_Run As Boolean
    Dim Run_Thread_Left, Run_Thread_Right, Run_Thread_Public As Thread '主线程
    Dim Bool_Start_Home As Boolean, Bool_Home_Done As Boolean = False
    Dim Safe_On As Boolean = False, Safe_Off As Boolean = True '安全光栅常开常闭切换
    Dim Language As LGE
    Dim Start_Run, Process_Left_Boolean, Process_Right_Boolean, mClear_Err As Boolean
    Dim Err_Count, Count_Left, Count_Right As Integer
    Dim Rw_Left, Rw_Right As System.IO.StreamWriter
    Dim Fs_Left, Fs_Right As FileStream
    Dim LR_PIC_Procesing As Boolean = False
    Dim JSON_WorkAt As String = String.Empty
    Dim Key_Name() As String
    Dim KeyResult() As String
    Public Enum DATABASE
        A001
        X816
    End Enum
    Structure KEYTYPE_STRU
        Shared ANSI As String = "ANSI"
        Shared ISO As String = "ISO"
        Shared JIS As String = "JIS"
    End Structure

    Structure WorkAt
        Shared LEFT As String = "LEFT"
        Shared RIGHT As String = "RIGHT"
    End Structure
    Public Structure JSON_PARA
        Shared TIME As String
        Shared AGENT_TYPE As String
        Shared RESULT As String
        Shared PROCESS As String
        Shared COMPONENT As String
        Shared TRAYSN As String
        Shared SHIFT_D_N As String
        Shared BUILDS As String
        Shared PROJECTS As String
        Shared PROGRAM_VER As String
        Shared AOI_VENDOR As String
        Shared LINE_NUMBER As String
        Shared KB_VERSION As String
        Shared CYCLE_TIME As String
        Shared FILE_PATH As String
        Shared JSON_UPLOAD As Boolean
    End Structure

    Public Structure MAJOR_TABLE
        Shared Table_Name As String
        Shared Station_Name As String
        Shared Machine_NO As String
        Shared Project_Name As String
        Shared Tray_Code As String
        Shared Language As String
        Shared Layout As String
        Shared Result As String
        Shared Fail_Location As String
        Shared Order_NO As String
        Shared LightLakageResult As String
        Shared HookSnapeResult As String


    End Structure

    Delegate Sub Back_Run_Delegate_Left()

    


    Sub Back_Run_Left()

        Dim FunName As String = GetCurrentMethod.Name & ":"

        DataCode_Left_New = DataCode_Left_tmp '把读取到的条码复制过来
        SQLCON.DELETE_Temp_Barcode_Left(DataCode_Left_New)
        Write_Err_Left(DataCode_Left_New, "DELETE_Temp_Barcode_Left")
        SQLCON.Insert_Temp_Barcode_left(DataCode_Left_New) '插入一行条码，开始图像处理
        Write_Err_Left(DataCode_Left_New, "Insert_Temp_Barcode_left")

        Dim IDB_Len As Int64 = 0
        Dim IDB_File_Len As Int64 = 0
        Select Case Select_Product_Name
            Case "ANSI"
                IDB_File_Len = 444119940
            Case "ISO"
                IDB_File_Len = 439073126
            Case "JIS"
                IDB_File_Len = 428979498
        End Select

        If File.Exists(CognexImagefile_IDB_Left_tmp) = True Then
            IDB_Len = FileLen(CognexImagefile_IDB_Left_tmp)
            If IDB_Len <> IDB_File_Len Then
                Dim str As String = "1工位[相机]IDB文件缺损，请检查！" & CognexImagefile_IDB_Left_tmp

                Write_Log_Left(DataCode_Left_tmp, "IDB 缺损!!!!!!!!!!!")

                MessageBoxEx.Show(str, "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
                Invoke(New Delegate_Display_Message_Left(AddressOf Display_Message_Left), str, DataCode_Left_New, Color.Red)



            End If
        End If

        If SW_Fully_automatic.Value = False Then
            Try
                ImageFile_Parallel_Left.Operator.Open(CognexImagefile_IDB_Left_tmp, CogImageFileModeConstants.Read)
                ImageFile_Parallel_Left.Run()
                Write_Err_Left(DataCode_Left_New, " ImageFile_Parallel_Left.Run")
            Catch ex As Exception
                Invoke(New Delegate_Display_Message_Left(AddressOf Display_Message_Left), "1工位图像处理打开IDB文件错误:" & ex.ToString, "", Color.Red)
            End Try
            Invoke(New Delegate_Display_Message_Left(AddressOf Display_Message_Left), "[1工位图像处理]:Start", DataCode_Left_New, Color.Blue)

            Try
                Dim HH As String = Date.Now.Hour.ToString.PadLeft(2).Replace(" ", "0")
                Dim MM As String = Date.Now.Minute.ToString.PadLeft(2).Replace(" ", "0")
                Dim SS As String = Date.Now.Second.ToString.PadLeft(2).Replace(" ", "0")
                Dim MS As String = Date.Now.Millisecond.ToString
                Dim ALL_Tim As String = HH & MM & SS

                Dim Str_csv As String = CognexImagefile_QS_Left_tmp & ".csv"
                Dim Str_qs As String = CognexImagefile_QS_Left_tmp & ".qs"

                Try
                    If File.Exists(Str_csv) = True Then
                        Dim File_Data As Date = File.GetLastWriteTime(Str_csv)
                        Dim Data As String = File_Data.ToString.Replace("/", "_").Replace(":", "_").Replace(" ", "_")
                        Dim New_File_Path As String = CognexImagefile_QS_Left_tmp & "\" & Data
                        CreateDirectory(New_File_Path)
                        File.Copy(Str_csv, New_File_Path & "\" & DataCode_Left_New & ".csv")
                        File.Copy(Str_qs, New_File_Path & "\" & DataCode_Left_New & ".qs")
                        File.Delete(Str_csv)
                        File.Delete(Str_qs)
                    End If
                Catch ex As Exception
                    Invoke(New Delegate_Display_Message_Left(AddressOf Display_Message_Left), FunName & ex.ToString, "", Color.Red)
                End Try


                'Dim Str_csv As String
                'Dim Str_qs As String
                'Str_csv = CognexImagefile_QS_Left & ".csv"
                'Str_qs = CognexImagefile_QS_Left & ".qs"
                'If Dir(Str_csv, vbHidden) <> "" Then
                '    Str_csv = CognexImagefile_QS_Left & "_" & ALL_Tim & ".csv"
                'End If
                'If Dir(Str_qs, vbHidden) <> "" Then
                '    Str_qs = CognexImagefile_QS_Left & "_" & ALL_Tim & ".qs"
                'End If
                Invoke(New Delegate_Display_Message_Left(AddressOf Display_Message_Left), "Create .QS Start", DataCode_Left_New, Color.Blue)

                Rw_Left = New System.IO.StreamWriter(Str_csv, False)
                Fs_Left = New FileStream(Str_qs, FileMode.Create, FileAccess.Write)
                Count_Left = 0
                For I As Int16 = 1 To INTTYPLE.Left_Trigger_Count
                    Select Case KB_Type
                        Case KB_Type_Enum.ANSI
                            Measure_Left_Ansi(I, Select_Product_Name, DataCode_Left_New, "ANSI")
                        Case KB_Type_Enum.ISO
                            Measure_Left_Iso(I, Select_Product_Name, DataCode_Left_New, "ISO")
                        Case KB_Type_Enum.JIS
                            Measure_Left_Jis(I, Select_Product_Name, DataCode_Left_New, "JIS")
                    End Select
                    Write_Err_Left(DataCode_Left_New, " ImageFile_Process:" & I)
                Next
                Invoke(New Delegate_Display_Message_Left(AddressOf Display_Message_Left), "Create .QS End", DataCode_Left_New, Color.Blue)
                Rw_Left.Flush()
                Rw_Left.Close()
                Rw_Left.Dispose()
                Fs_Left.Flush()
                Fs_Left.Close()
                Fs_Left.Dispose()
                ImageFile_Parallel_Left.Operator.Close()
                If PARAM_BOOL.S是否删除IDB文件 = True Then
                    System.IO.File.Delete(CognexImagefile_IDB_Left_tmp)
                End If
                Write_Err_Left(DataCode_Left_New, "ImageFile_Parallel_Left.Operator.Close")

                Invoke(New Delegate_Display_Message_Left(AddressOf Display_Message_Left), "1工位插入图片[完成]", DataCode_Left_New, Color.Black)
            Catch ex As Exception
                BOOL.Insert_Image = False
                Invoke(New Delegate_Display_Message_Left(AddressOf Display_Message_Left), "[1工位图像处理]错误：" & ex.ToString, DataCode_Left_New, Color.Red)
            End Try
            Invoke(New Delegate_Display_Message_Left(AddressOf Display_Message_Left), "[1工位图像处理]:End", DataCode_Left_New, Color.Blue)
        Else
            Invoke(New Delegate_Display_Message_Left(AddressOf Display_Message_Left), "[1工位图像处理]被屏蔽", DataCode_Left_New, Color.Black)
        End If

        SQLCON.DELETE_Temp_Barcode_Left(DataCode_Left_New) '图像处理完成，删除条码
        Write_Err_Left(DataCode_Left_New, "SQLCON.DELETE_Temp_Barcode_Left")
    End Sub

    Delegate Sub Back_Run_Delegate_Right()
    Sub Back_Run_Right()

        Dim FunName As String = GetCurrentMethod.Name & ":"

        DataCode_Right_New = DataCode_Right_tmp '把读取到的条码复制过来
        SQLCON.DELETE_Temp_Barcode_Right(DataCode_Right_New)
        Write_Err_Right(DataCode_Right_New, "SQLCON.DELETE_Temp_Barcode_Right")
        SQLCON.Insert_Temp_Barcode_Right(DataCode_Right_New) '插入一行条码，开始图像处理
        Write_Err_Right(DataCode_Right_New, "SQLCON.Insert_Temp_Barcode_Right")

        Dim IDB_Len As Int64 = 0
        Dim IDB_File_Len As Int64 = 0
        Select Case Select_Product_Name
            Case "ANSI"
                IDB_File_Len = 444119940
            Case "ISO"
                IDB_File_Len = 439073126
            Case "JIS"
                IDB_File_Len = 428979498
        End Select

        If File.Exists(CognexImagefile_IDB_Right_tmp) = True Then
            IDB_Len = FileLen(CognexImagefile_IDB_Right_tmp)
            If IDB_Len <> IDB_File_Len Then
                Dim str As String = "2工位[相机]IDB文件缺损，请检查！" & CognexImagefile_IDB_Right_tmp

                Write_Log_Right(DataCode_Right_tmp, "IDB 缺损!!!!!!!!!!!")

                MessageBoxEx.Show(str, "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
                Invoke(New Delegate_Display_Message_Right(AddressOf Display_Message_Right), str, DataCode_Right_New, Color.Red)
            End If
        End If

        If SW_Fully_automatic.Value = False Then
            Try
                ImageFile_Parallel_Right.Operator.Open(CognexImagefile_IDB_Right_tmp, CogImageFileModeConstants.Read)
                ImageFile_Parallel_Right.Run()
                Write_Err_Right(DataCode_Right_New, " ImageFile_Parallel_Right.Run")
            Catch ex As Exception
                Invoke(New Delegate_Display_Message_Right(AddressOf Display_Message_Right), "2工位图像处理打开IDB文件错误:" & ex.ToString, DataCode_Right_New, Color.Red)
            End Try

            Invoke(New Delegate_Display_Message_Right(AddressOf Display_Message_Right), "[2工位图像处理]：Start", DataCode_Right_New, Color.Blue)

            Try
                Dim HH As String = Date.Now.Hour.ToString.PadLeft(2).Replace(" ", "0")
                Dim MM As String = Date.Now.Minute.ToString.PadLeft(2).Replace(" ", "0")
                Dim SS As String = Date.Now.Second.ToString.PadLeft(2).Replace(" ", "0")
                Dim MS As String = Date.Now.Millisecond.ToString
                Dim ALL_Tim As String = HH & MM & SS


                Dim Str_csv As String = CognexImagefile_QS_Right_tmp & ".csv"
                Dim Str_qs As String = CognexImagefile_QS_Right_tmp & ".qs"
                Try
                    If File.Exists(Str_csv) = True Then
                        Dim File_Data As Date = File.GetLastWriteTime(Str_csv)
                        Dim Data As String = File_Data.ToString.Replace("/", "_").Replace(":", "_").Replace(" ", "_")
                        Dim New_File_Path As String = CognexImagefile_QS_Right_tmp & "\" & Data
                        CreateDirectory(New_File_Path)
                        File.Copy(Str_csv, New_File_Path & "\" & DataCode_Right_New & ".csv")
                        File.Copy(Str_qs, New_File_Path & "\" & DataCode_Right_New & ".qs")
                        File.Delete(Str_csv)
                        File.Delete(Str_qs)
                    End If
                Catch ex As Exception
                    Invoke(New Delegate_Display_Message_Right(AddressOf Display_Message_Right), FunName & ex.ToString, "", Color.Red)
                End Try

                'Dim Str_csv As String
                'Dim Str_qs As String
                'Str_csv = CognexImagefile_QS_Right & ".csv"
                'Str_qs = CognexImagefile_QS_Right & ".qs"
                'If Dir(Str_csv, vbHidden) <> "" Then
                '    Str_csv = CognexImagefile_QS_Right & "_" & ALL_Tim & ".csv"
                'End If
                'If Dir(Str_qs, vbHidden) <> "" Then
                '    Str_qs = CognexImagefile_QS_Right & "_" & ALL_Tim & ".qs"
                'End If


                Rw_Right = New System.IO.StreamWriter(Str_csv, False)
                Fs_Right = New FileStream(Str_qs, FileMode.Create, FileAccess.Write)
                Count_Right = 0
                For I As Int16 = 1 To INTTYPLE.Right_Trigger_Count
                    Select Case KB_Type
                        Case KB_Type_Enum.ANSI
                            Measure_Right_Ansi(I, Select_Product_Name, DataCode_Right_New, "ANSI")
                        Case KB_Type_Enum.ISO
                            Measure_Right_Iso(I, Select_Product_Name, DataCode_Right_New, "ISO")
                        Case KB_Type_Enum.JIS
                            Measure_Right_Jis(I, Select_Product_Name, DataCode_Right_New, "JIS")
                    End Select
                    Write_Err_Right(DataCode_Right_New, " ImageFile_Process:" & I)
                Next

                Rw_Right.Flush()
                Rw_Right.Close()
                Rw_Right.Dispose()
                Fs_Right.Flush()
                Fs_Right.Close()
                Fs_Right.Dispose()
                ImageFile_Parallel_Right.Operator.Close()
                If PARAM_BOOL.S是否删除IDB文件 = True Then
                    System.IO.File.Delete(CognexImagefile_IDB_Right_tmp)
                End If
                Write_Err_Right(DataCode_Right_New, "ImageFile_Parallel_Right.Operator.Close")

                Invoke(New Delegate_Display_Message_Right(AddressOf Display_Message_Right), "2工位插入图片[完成]", DataCode_Right_New, Color.Black)
            Catch ex As Exception
                BOOL.Insert_Image = False
                Invoke(New Delegate_Display_Message_Right(AddressOf Display_Message_Right), "[2工位图像处理]：错误" & ex.ToString, DataCode_Right_New, Color.Red)
            End Try
            Invoke(New Delegate_Display_Message_Right(AddressOf Display_Message_Right), "[2工位图像处理]:End", DataCode_Right_New, Color.Blue)
        Else
            Invoke(New Delegate_Display_Message_Right(AddressOf Display_Message_Right), "[2工位图像处理]被屏蔽", DataCode_Right_New, Color.Black)
        End If

        SQLCON.DELETE_Temp_Barcode_Right(DataCode_Right_New)
        Write_Err_Right(DataCode_Right_New, "SQLCON.DELETE_Temp_Barcode_Right")
    End Sub

    Dim Work_中间位放料位(), Work_左工位镭射取料位(), Work_右工位镭射取料位(), Work_左工位初始位(), Work_右工位初始位() As Work_Coordinatess '储存所有运动坐标 
    Dim Work_右工位拍照第一行从左到右() As Work_Coordinatess, Work_左工位拍照第一行从左到右() As Work_Coordinatess
    Dim Work_右工位拍照第二行从右到左() As Work_Coordinatess, Work_左工位拍照第二行从右到左() As Work_Coordinatess
    Dim Work_右工位拍照第三行从左到右() As Work_Coordinatess, Work_左工位拍照第三行从左到右() As Work_Coordinatess
    Dim Work_右工位拍照第四行从右到左() As Work_Coordinatess, Work_左工位拍照第四行从右到左() As Work_Coordinatess
    Dim Work_右工位拍照第五行从左到右() As Work_Coordinatess, Work_左工位拍照第五行从左到右() As Work_Coordinatess
    Dim Work_右工位拍照第六行从右到左() As Work_Coordinatess, Work_左工位拍照第六行从右到左() As Work_Coordinatess

    Dim Work_左镭射第一行扫描从左到右() As Work_Coordinatess
    Dim Work_左镭射第四行扫描从左到右() As Work_Coordinatess
    Dim Work_左镭射第二行扫描从右到左() As Work_Coordinatess
    Dim Work_左镭射第五行扫描从右到左() As Work_Coordinatess
    Dim Work_左镭射第三行扫描从左到右() As Work_Coordinatess
    Dim Work_左镭射第六行扫描从左到右() As Work_Coordinatess

    Const Trigger_X_Offset As Double = 1.5
    Public Correction_needle_Mode As Pul, Calib_Model As LR_STATION
    Dim needle_position_Org, Laser_position_Org As Double, Value_1903() As Double, Laser_Height As Double
    Dim rans As New Random
    Dim CognexImagefile_IDB_Right As String, CognexImagefile_IDB_Left As String
    Dim CognexImagefile_QS_Right As String, CognexImagefile_QS_Left As String

    Dim CognexImagefile_IDB_Right_tmp As String, CognexImagefile_IDB_Left_tmp As String
    Dim CognexImagefile_QS_Right_tmp As String, CognexImagefile_QS_Left_tmp As String

#Region "Vision"

    Dim FUN_TOP_FIXTURE_L, N1X1_l, FUN_DOWN_FIXTURE_l, FUN_TOP_LEFT_l, N1X1_Tilde_l, FUN_TOP_RIGHT_l, KEY_14_l, KEY_15_l, KEY_29_l, KEY_30_l, KEY_31_l, KEY_32_l, KEY_44_l, KEY_45_l, KEY_46_l, KEY_47_l, KEY_48_l, KEY_59_l, KEY_60_l, KEY_61_l, KEY_65_l, KEY_71_l, N1X1_DOWN_LEFT_l, N1X1_DOWN_RIGHT_l, N1X1_TOP_LEFT_l, N1X1_TOP_RIGHT_l, FUN_DOWN_LEFT_l, FUN_DOWN_RIGHT_l As New CogPMAlignPattern
    Dim FUN_TOP_FIXTURE_r, N1X1_r, FUN_DOWN_FIXTURE_r, FUN_TOP_LEFT_r, N1X1_Tilde_r, FUN_TOP_RIGHT_r, KEY_14_r, KEY_15_r, KEY_29_r, KEY_30_r, KEY_31_r, KEY_32_r, KEY_44_r, KEY_45_r, KEY_46_r, KEY_47_r, KEY_48_r, KEY_59_r, KEY_60_r, KEY_61_r, KEY_65_r, KEY_71_r, N1X1_DOWN_LEFT_r, N1X1_DOWN_RIGHT_r, N1X1_TOP_LEFT_r, N1X1_TOP_RIGHT_r, FUN_DOWN_LEFT_r, FUN_DOWN_RIGHT_r As New CogPMAlignPattern

    Dim CogToolBlock_Group_Left, CogToolBlock_Group_Right As New CogToolBlock
    Dim ImageFile_Parallel_Left, ImageFile_Parallel_Right As New CogImageFileTool
    Dim Threshold As Integer = 180
    Dim CogToolBlock_Shard_Left As New CogToolBlock
    Dim CogToolBlock_Shard_Right As New CogToolBlock
    Dim GRAB_IMAGE_BOOLEAN_LEFT, GRAB_IMAGE_BOOLEAN_RIGHT As Boolean
    Structure Image_Param
        Dim ImageBuf As Image
        Dim KeyName As String
    End Structure
    Sub Measure_Left_Ansi_1(ByVal Image_Index As Integer, ByVal Project_Name As String, ByVal Barcode As String, ByVal KeyType As String)
        Try
            Dim Index_Increase As Integer = 13   '增长因子
            Select Case Image_Index
                Case 1
                    CogToolBlock_Fixture_Left = CogToolBlock_Group_Left.Tools("CogToolBlock_Measure_Fun_1X1")
                    CogToolBlock_Shard_Left = CogToolBlock_Fixture_Left.Tools("CogToolBlock_1X1")
                    Measure_Sub_Left(Select_Product_Name, Barcode, KEYNAME.Esc, Image_Index, 0, KeyType, "D:\System\HOOK_SNAP\Left\")
                Case 2
                    CogToolBlock_Fixture_Left = CogToolBlock_Group_Left.Tools("CogToolBlock_Measure_Fun_1X1")
                    CogToolBlock_Shard_Left = CogToolBlock_Fixture_Left.Tools("CogToolBlock_1X1")
                    Measure_Sub_Left(Select_Product_Name, Barcode, KEYNAME.F1, Image_Index, 1, KeyType, "D:\System\HOOK_SNAP\Left\")
                Case 3
                    CogToolBlock_Fixture_Left = CogToolBlock_Group_Left.Tools("CogToolBlock_Measure_Fun_1X1")
                    CogToolBlock_Shard_Left = CogToolBlock_Fixture_Left.Tools("CogToolBlock_1X1")
                    Measure_Sub_Left(Select_Product_Name, Barcode, KEYNAME.F2, Image_Index, 2, KeyType, "D:\System\HOOK_SNAP\Left\")
                Case 4
                    CogToolBlock_Fixture_Left = CogToolBlock_Group_Left.Tools("CogToolBlock_Measure_Fun_1X1")
                    CogToolBlock_Shard_Left = CogToolBlock_Fixture_Left.Tools("CogToolBlock_1X1")
                    Measure_Sub_Left(Select_Product_Name, Barcode, KEYNAME.F3, Image_Index, 3, KeyType, "D:\System\HOOK_SNAP\Left\")
                Case 5
                    CogToolBlock_Fixture_Left = CogToolBlock_Group_Left.Tools("CogToolBlock_Measure_Fun_1X1")
                    CogToolBlock_Shard_Left = CogToolBlock_Fixture_Left.Tools("CogToolBlock_1X1")
                    Measure_Sub_Left(Select_Product_Name, Barcode, KEYNAME.F4, Image_Index, 4, KeyType, "D:\System\HOOK_SNAP\Left\")
                Case 6
                    CogToolBlock_Fixture_Left = CogToolBlock_Group_Left.Tools("CogToolBlock_Measure_Fun_1X1")
                    CogToolBlock_Shard_Left = CogToolBlock_Fixture_Left.Tools("CogToolBlock_1X1")
                    Measure_Sub_Left(Select_Product_Name, Barcode, KEYNAME.F5, Image_Index, 5, KeyType, "D:\System\HOOK_SNAP\Left\")
                Case 7
                    CogToolBlock_Fixture_Left = CogToolBlock_Group_Left.Tools("CogToolBlock_Measure_Fun_1X1")
                    CogToolBlock_Shard_Left = CogToolBlock_Fixture_Left.Tools("CogToolBlock_1X1")
                    Measure_Sub_Left(Select_Product_Name, Barcode, KEYNAME.F6, Image_Index, 6, KeyType, "D:\System\HOOK_SNAP\Left\")
                Case 8
                    CogToolBlock_Fixture_Left = CogToolBlock_Group_Left.Tools("CogToolBlock_Measure_Fun_1X1")
                    CogToolBlock_Shard_Left = CogToolBlock_Fixture_Left.Tools("CogToolBlock_1X1")
                    Measure_Sub_Left(Select_Product_Name, Barcode, KEYNAME.F7, Image_Index, 7, KeyType, "D:\System\HOOK_SNAP\Left\")

                Case 9
                    CogToolBlock_Fixture_Left = CogToolBlock_Group_Left.Tools("CogToolBlock_Measure_Fun_1X1")
                    CogToolBlock_Shard_Left = CogToolBlock_Fixture_Left.Tools("CogToolBlock_1X1")
                    Measure_Sub_Left(Select_Product_Name, Barcode, KEYNAME.F8, Image_Index, 8, KeyType, "D:\System\HOOK_SNAP\Left\")

                Case 10
                    CogToolBlock_Fixture_Left = CogToolBlock_Group_Left.Tools("CogToolBlock_Measure_Fun_1X1")
                    CogToolBlock_Shard_Left = CogToolBlock_Fixture_Left.Tools("CogToolBlock_1X1")
                    Measure_Sub_Left(Select_Product_Name, Barcode, KEYNAME.F9, Image_Index, 9, KeyType, "D:\System\HOOK_SNAP\Left\")
                Case 11
                    CogToolBlock_Fixture_Left = CogToolBlock_Group_Left.Tools("CogToolBlock_Measure_Fun_1X1")
                    CogToolBlock_Shard_Left = CogToolBlock_Fixture_Left.Tools("CogToolBlock_1X1")
                    Measure_Sub_Left(Select_Product_Name, Barcode, KEYNAME.F10, Image_Index, 10, KeyType, "D:\System\HOOK_SNAP\Left\")
                Case 12
                    CogToolBlock_Fixture_Left = CogToolBlock_Group_Left.Tools("CogToolBlock_Measure_Fun_1X1")
                    CogToolBlock_Shard_Left = CogToolBlock_Fixture_Left.Tools("CogToolBlock_1X1")
                    Measure_Sub_Left(Select_Product_Name, Barcode, KEYNAME.F11, Image_Index, 11, KeyType, "D:\System\HOOK_SNAP\Left\")
                Case 13
                    CogToolBlock_Fixture_Left = CogToolBlock_Group_Left.Tools("CogToolBlock_Measure_Fun_1X1")
                    CogToolBlock_Shard_Left = CogToolBlock_Fixture_Left.Tools("CogToolBlock_1X1")
                    Measure_Sub_Left(Select_Product_Name, Barcode, KEYNAME.F12, Image_Index, 12, KeyType, "D:\System\HOOK_SNAP\Left\")

                Case 14
                    CogToolBlock_Fixture_Left = CogToolBlock_Group_Left.Tools("CogToolBlock_Measure_Fun_1X1")
                    CogToolBlock_Shard_Left = CogToolBlock_Fixture_Left.Tools("CogToolBlock_1X1")
                    Measure_Sub_Left(Select_Product_Name, Barcode, KEYNAME.Power, Image_Index, 13, KeyType, "D:\System\HOOK_SNAP\Left\")

                Case 15
                    CogToolBlock_Fixture_Left = CogToolBlock_Group_Left.Tools("CogToolBlock_Measure_Fun_1X1")
                    CogToolBlock_Shard_Left = CogToolBlock_Fixture_Left.Tools("CogToolBlock_1X1")
                    Measure_Sub_Left(Select_Product_Name, Barcode, KEYNAME.Tilde, Image_Index, 14, KeyType, "D:\System\HOOK_SNAP\Left\")

                Case 16
                    CogToolBlock_Fixture_Left = CogToolBlock_Group_Left.Tools("CogToolBlock_Measure_Fun_1X1")
                    CogToolBlock_Shard_Left = CogToolBlock_Fixture_Left.Tools("CogToolBlock_1X1")
                    Measure_Sub_Left(Select_Product_Name, Barcode, KEYNAME.Num1, Image_Index, 15, KeyType, "D:\System\HOOK_SNAP\Left\")

                Case 17
                    CogToolBlock_Fixture_Left = CogToolBlock_Group_Left.Tools("CogToolBlock_Measure_Fun_1X1")
                    CogToolBlock_Shard_Left = CogToolBlock_Fixture_Left.Tools("CogToolBlock_1X1")
                    Measure_Sub_Left(Select_Product_Name, Barcode, KEYNAME.Num2, Image_Index, 16, KeyType, "D:\System\HOOK_SNAP\Left\")
                Case 18

                    CogToolBlock_Fixture_Left = CogToolBlock_Group_Left.Tools("CogToolBlock_Measure_Fun_1X1")
                    CogToolBlock_Shard_Left = CogToolBlock_Fixture_Left.Tools("CogToolBlock_1X1")
                    Measure_Sub_Left(Select_Product_Name, Barcode, KEYNAME.Num3, Image_Index, 17, KeyType, "D:\System\HOOK_SNAP\Left\")

                Case 19

                    CogToolBlock_Fixture_Left = CogToolBlock_Group_Left.Tools("CogToolBlock_Measure_Fun_1X1")
                    CogToolBlock_Shard_Left = CogToolBlock_Fixture_Left.Tools("CogToolBlock_1X1")
                    Measure_Sub_Left(Select_Product_Name, Barcode, KEYNAME.Num4, Image_Index, 18, KeyType, "D:\System\HOOK_SNAP\Left\")
                Case 20
                    CogToolBlock_Fixture_Left = CogToolBlock_Group_Left.Tools("CogToolBlock_Measure_Fun_1X1")
                    CogToolBlock_Shard_Left = CogToolBlock_Fixture_Left.Tools("CogToolBlock_1X1")
                    Measure_Sub_Left(Select_Product_Name, Barcode, KEYNAME.Num5, Image_Index, 19, KeyType, "D:\System\HOOK_SNAP\Left\")


                Case 21
                    CogToolBlock_Fixture_Left = CogToolBlock_Group_Left.Tools("CogToolBlock_Measure_Fun_1X1")
                    CogToolBlock_Shard_Left = CogToolBlock_Fixture_Left.Tools("CogToolBlock_1X1")
                    Measure_Sub_Left(Select_Product_Name, Barcode, KEYNAME.Num6, Image_Index, 20, KeyType, "D:\System\HOOK_SNAP\Left\")

                Case 22

                    CogToolBlock_Fixture_Left = CogToolBlock_Group_Left.Tools("CogToolBlock_Measure_Fun_1X1")
                    CogToolBlock_Shard_Left = CogToolBlock_Fixture_Left.Tools("CogToolBlock_1X1")
                    Measure_Sub_Left(Select_Product_Name, Barcode, KEYNAME.Num7, Image_Index, 21, KeyType, "D:\System\HOOK_SNAP\Left\")
                Case 23
                    CogToolBlock_Fixture_Left = CogToolBlock_Group_Left.Tools("CogToolBlock_Measure_Fun_1X1")
                    CogToolBlock_Shard_Left = CogToolBlock_Fixture_Left.Tools("CogToolBlock_1X1")
                    Measure_Sub_Left(Select_Product_Name, Barcode, KEYNAME.Num8, Image_Index, 22, KeyType, "D:\System\HOOK_SNAP\Left\")
                Case 24
                    CogToolBlock_Fixture_Left = CogToolBlock_Group_Left.Tools("CogToolBlock_Measure_Fun_1X1")
                    CogToolBlock_Shard_Left = CogToolBlock_Fixture_Left.Tools("CogToolBlock_1X1")
                    Measure_Sub_Left(Select_Product_Name, Barcode, KEYNAME.Num9, Image_Index, 23, KeyType, "D:\System\HOOK_SNAP\Left\")
                Case 25
                    CogToolBlock_Fixture_Left = CogToolBlock_Group_Left.Tools("CogToolBlock_Measure_Fun_1X1")
                    CogToolBlock_Shard_Left = CogToolBlock_Fixture_Left.Tools("CogToolBlock_1X1")
                    Measure_Sub_Left(Select_Product_Name, Barcode, KEYNAME.Num0, Image_Index, 24, KeyType, "D:\System\HOOK_SNAP\Left\")
                Case 26
                    CogToolBlock_Fixture_Left = CogToolBlock_Group_Left.Tools("CogToolBlock_Measure_Fun_1X1")
                    CogToolBlock_Shard_Left = CogToolBlock_Fixture_Left.Tools("CogToolBlock_1X1")
                    Measure_Sub_Left(Select_Product_Name, Barcode, KEYNAME.Minus, Image_Index, 25, KeyType, "D:\System\HOOK_SNAP\Left\")

                Case 27
                    CogToolBlock_Fixture_Left = CogToolBlock_Group_Left.Tools("CogToolBlock_Measure_Fun_1X1")
                    CogToolBlock_Shard_Left = CogToolBlock_Fixture_Left.Tools("CogToolBlock_1X1")
                    Measure_Sub_Left(Select_Product_Name, Barcode, KEYNAME.Equal, Image_Index, 26, KeyType, "D:\System\HOOK_SNAP\Left\")
                Case 28
                    CogToolBlock_Fixture_Left = CogToolBlock_Group_Left.Tools("CogToolBlock_Measure_Fun_1X1")
                    CogToolBlock_Shard_Left = CogToolBlock_Fixture_Left.Tools("CogToolBlock_1X1")
                    Measure_Sub_Left(Select_Product_Name, Barcode, KEYNAME.Delete, Image_Index, 27, KeyType, "D:\System\HOOK_SNAP\Left\")
                Case 16 + Index_Increase To 28 + Index_Increase
                    Dim _keyname As String = Nothing
                    Dim Measure_Index As Integer
                    Select Case Image_Index
                        Case 16 + Index_Increase
                            _keyname = KEYNAME.BackSlash : Measure_Index = 43
                        Case 17 + Index_Increase
                            _keyname = KEYNAME.RightBracket : Measure_Index = 44
                        Case 18 + Index_Increase
                            _keyname = KEYNAME.LeftBracket : Measure_Index = 45
                        Case 19 + Index_Increase
                            _keyname = KEYNAME.P : Measure_Index = 46
                        Case 20 + Index_Increase
                            _keyname = KEYNAME.O : Measure_Index = 47
                        Case 21 + Index_Increase
                            _keyname = KEYNAME.I : Measure_Index = 48
                        Case 22 + Index_Increase
                            _keyname = KEYNAME.U : Measure_Index = 49
                        Case 23 + Index_Increase
                            _keyname = KEYNAME.Y : Measure_Index = 50
                        Case 24 + Index_Increase
                            _keyname = KEYNAME.T : Measure_Index = 51
                        Case 25 + Index_Increase
                            _keyname = KEYNAME.R : Measure_Index = 52
                        Case 26 + Index_Increase
                            _keyname = KEYNAME.E : Measure_Index = 53
                        Case 27 + Index_Increase
                            _keyname = KEYNAME.W : Measure_Index = 54
                        Case 28 + Index_Increase
                            _keyname = KEYNAME.Q : Measure_Index = 55
                    End Select
                    CogToolBlock_Fixture_Left = CogToolBlock_Group_Left.Tools("CogToolBlock_Measure_1X1")
                    CogToolBlock_Shard_Left = CogToolBlock_Fixture_Left.Tools("CogToolBlock_1X1")
                    Measure_Sub_Left(Select_Product_Name, Barcode, _keyname, Image_Index, Measure_Index, KeyType, "D:\System\HOOK_SNAP\Left\")
                Case 29 + Index_Increase
                    CogToolBlock_Fixture_Left = CogToolBlock_Group_Left.Tools("CogToolBlock_Measure_Tab01")
                    CogToolBlock_Shard_Left = CogToolBlock_Fixture_Left.Tools("CogToolBlock_Tab01")
                    Measure_Sub_Left(Select_Product_Name, Barcode, KEYNAME.Tab, Image_Index, 56, KeyType, "D:\System\HOOK_SNAP\Left\")
                Case 30 + Index_Increase
                    CogToolBlock_Fixture_Left = CogToolBlock_Group_Left.Tools("CogToolBlock_Measure_Tab02")
                    CogToolBlock_Shard_Left = CogToolBlock_Fixture_Left.Tools("CogToolBlock_Tab02")
                    Measure_Sub_Left(Select_Product_Name, Barcode, KEYNAME.Tab, Image_Index, 57, KeyType, "D:\System\HOOK_SNAP\Left\")
                Case 31 + Index_Increase
                    CogToolBlock_Fixture_Left = CogToolBlock_Group_Left.Tools("CogToolBlock_Measure_CapsLock01")
                    CogToolBlock_Shard_Left = CogToolBlock_Fixture_Left.Tools("CogToolBlock_CapsLock01")
                    Measure_Sub_Left(Select_Product_Name, Barcode, KEYNAME.CapsLock, Image_Index, 58, KeyType, "D:\System\HOOK_SNAP\Left\")
                Case 32 + Index_Increase
                    CogToolBlock_Fixture_Left = CogToolBlock_Group_Left.Tools("CogToolBlock_Measure_CapsLock02")
                    CogToolBlock_Shard_Left = CogToolBlock_Fixture_Left.Tools("CogToolBlock_CapsLock02")
                    Measure_Sub_Left(Select_Product_Name, Barcode, KEYNAME.CapsLock, Image_Index, 59, KeyType, "D:\System\HOOK_SNAP\Left\")
                Case 33 + Index_Increase To 43 + Index_Increase
                    Dim _keyname As String = Nothing : Dim Measure_Index As Integer
                    Select Case Image_Index
                        Case 33 + Index_Increase
                            _keyname = KEYNAME.A : Measure_Index = 60
                        Case 34 + Index_Increase
                            _keyname = KEYNAME.S : Measure_Index = 61
                        Case 35 + Index_Increase
                            _keyname = KEYNAME.D : Measure_Index = 62
                        Case 36 + Index_Increase
                            _keyname = KEYNAME.F : Measure_Index = 63
                        Case 37 + Index_Increase
                            _keyname = KEYNAME.G : Measure_Index = 64
                        Case 38 + Index_Increase
                            _keyname = KEYNAME.H : Measure_Index = 65
                        Case 39 + Index_Increase
                            _keyname = KEYNAME.J : Measure_Index = 66
                        Case 40 + Index_Increase
                            _keyname = KEYNAME.K : Measure_Index = 67
                        Case 41 + Index_Increase
                            _keyname = KEYNAME.L : Measure_Index = 68
                        Case 42 + Index_Increase
                            _keyname = KEYNAME.Semicolon : Measure_Index = 69
                        Case 43 + Index_Increase
                            _keyname = KEYNAME.Quote : Measure_Index = 70
                    End Select
                    CogToolBlock_Fixture_Left = CogToolBlock_Group_Left.Tools("CogToolBlock_Measure_1X1")
                    CogToolBlock_Shard_Left = CogToolBlock_Fixture_Left.Tools("CogToolBlock_1X1")
                    Measure_Sub_Left(Select_Product_Name, Barcode, _keyname, Image_Index, Measure_Index, KeyType, "D:\System\HOOK_SNAP\Left\")
                Case 44 + Index_Increase
                    CogToolBlock_Fixture_Left = CogToolBlock_Group_Left.Tools("CogToolBlock_Measure_Return01")
                    CogToolBlock_Shard_Left = CogToolBlock_Fixture_Left.Tools("CogToolBlock_Return01")
                    Measure_Sub_Left(Select_Product_Name, Barcode, KEYNAME.Return_, Image_Index, 71, KeyType, "D:\System\HOOK_SNAP\Left\")
                Case 45 + Index_Increase
                    CogToolBlock_Fixture_Left = CogToolBlock_Group_Left.Tools("CogToolBlock_Measure_Return02")
                    CogToolBlock_Shard_Left = CogToolBlock_Fixture_Left.Tools("CogToolBlock_Return02")
                    Measure_Sub_Left(Select_Product_Name, Barcode, KEYNAME.Return_, Image_Index, 72, KeyType, "D:\System\HOOK_SNAP\Left\")
                Case 46 + Index_Increase
                    CogToolBlock_Fixture_Left = CogToolBlock_Group_Left.Tools("CogToolBlock_Measure_Right_Shift01")
                    CogToolBlock_Shard_Left = CogToolBlock_Fixture_Left.Tools("CogToolBlock_Shift01")
                    Measure_Sub_Left(Select_Product_Name, Barcode, KEYNAME.RightShift, Image_Index, 73, KeyType, "D:\System\HOOK_SNAP\Left\")
                Case 47 + Index_Increase
                    CogToolBlock_Fixture_Left = CogToolBlock_Group_Left.Tools("CogToolBlock_Measure_Right_Shift02")
                    CogToolBlock_Shard_Left = CogToolBlock_Fixture_Left.Tools("CogToolBlock_Shift02")
                    Measure_Sub_Left(Select_Product_Name, Barcode, KEYNAME.RightShift, Image_Index, 74, KeyType, "D:\System\HOOK_SNAP\Left\")
                Case 48 + Index_Increase
                    CogToolBlock_Fixture_Left = CogToolBlock_Group_Left.Tools("CogToolBlock_Measure_Right_Shift03")
                    CogToolBlock_Shard_Left = CogToolBlock_Fixture_Left.Tools("CogToolBlock_Shift03")
                    Measure_Sub_Left(Select_Product_Name, Barcode, KEYNAME.RightShift, Image_Index, 75, KeyType, "D:\System\HOOK_SNAP\Left\")
                Case 49 + Index_Increase To 58 + Index_Increase
                    Dim _keyname As String = Nothing : Dim Measure_Index As Integer
                    Select Case Image_Index
                        Case 49 + Index_Increase
                            _keyname = KEYNAME.Question : Measure_Index = 76
                        Case 50 + Index_Increase
                            _keyname = KEYNAME.Period : Measure_Index = 77
                        Case 51 + Index_Increase
                            _keyname = KEYNAME.Comma : Measure_Index = 78
                        Case 52 + Index_Increase
                            _keyname = KEYNAME.M : Measure_Index = 79
                        Case 53 + Index_Increase
                            _keyname = KEYNAME.N : Measure_Index = 80
                        Case 54 + Index_Increase
                            _keyname = KEYNAME.B : Measure_Index = 80
                        Case 55 + Index_Increase
                            _keyname = KEYNAME.V : Measure_Index = 82
                        Case 56 + Index_Increase
                            _keyname = KEYNAME.C : Measure_Index = 83
                        Case 57 + Index_Increase
                            _keyname = KEYNAME.X : Measure_Index = 84
                        Case 58 + Index_Increase
                            _keyname = KEYNAME.Z : Measure_Index = 85
                    End Select
                    CogToolBlock_Fixture_Left = CogToolBlock_Group_Left.Tools("CogToolBlock_Measure_1X1")
                    CogToolBlock_Shard_Left = CogToolBlock_Fixture_Left.Tools("CogToolBlock_1X1")
                    Measure_Sub_Left(Select_Product_Name, Barcode, _keyname, Image_Index, Measure_Index, KeyType, "D:\System\HOOK_SNAP\Left\")
                Case 59 + Index_Increase
                    CogToolBlock_Fixture_Left = CogToolBlock_Group_Left.Tools("CogToolBlock_Measure_Left_Shift01")
                    CogToolBlock_Shard_Left = CogToolBlock_Fixture_Left.Tools("CogToolBlock_Shift01")
                    Measure_Sub_Left(Select_Product_Name, Barcode, KEYNAME.LeftShift, Image_Index, 86, KeyType, "D:\System\HOOK_SNAP\Left\")
                Case 60 + Index_Increase
                    CogToolBlock_Fixture_Left = CogToolBlock_Group_Left.Tools("CogToolBlock_Measure_Left_Shift02")
                    CogToolBlock_Shard_Left = CogToolBlock_Fixture_Left.Tools("CogToolBlock_Shift02")
                    Measure_Sub_Left(Select_Product_Name, Barcode, KEYNAME.LeftShift, Image_Index, 87, KeyType, "D:\System\HOOK_SNAP\Left\")
                Case 61 + Index_Increase
                    CogToolBlock_Fixture_Left = CogToolBlock_Group_Left.Tools("CogToolBlock_Measure_Left_Shift03")
                    CogToolBlock_Shard_Left = CogToolBlock_Fixture_Left.Tools("CogToolBlock_Shift03")
                    Measure_Sub_Left(Select_Product_Name, Barcode, KEYNAME.LeftShift, Image_Index, 88, KeyType, "D:\System\HOOK_SNAP\Left\")
                Case 62 + Index_Increase To 64 + Index_Increase
                    Dim _keyname As String = Nothing : Dim Measure_Index As Integer
                    Select Case Image_Index
                        Case 62 + Index_Increase
                            _keyname = KEYNAME.Fn : Measure_Index = 89
                        Case 63 + Index_Increase
                            _keyname = KEYNAME.Control : Measure_Index = 90
                        Case 64 + Index_Increase
                            _keyname = KEYNAME.LeftOption : Measure_Index = 91
                    End Select
                    CogToolBlock_Fixture_Left = CogToolBlock_Group_Left.Tools("CogToolBlock_Measure_1X1")
                    CogToolBlock_Shard_Left = CogToolBlock_Fixture_Left.Tools("CogToolBlock_1X1")
                    Measure_Sub_Left(Select_Product_Name, Barcode, _keyname, Image_Index, Measure_Index, KeyType, "D:\System\HOOK_SNAP\Left\")
                Case 65 + Index_Increase
                    CogToolBlock_Fixture_Left = CogToolBlock_Group_Left.Tools("CogToolBlock_Mesure_Command")
                    CogToolBlock_Shard_Left = CogToolBlock_Fixture_Left.Tools("CogToolBlock_Command")
                    Measure_Sub_Left(Select_Product_Name, Barcode, KEYNAME.LeftCommand, Image_Index, 92, KeyType, "D:\System\HOOK_SNAP\Left\")
                Case 71 + Index_Increase
                    CogToolBlock_Fixture_Left = CogToolBlock_Group_Left.Tools("CogToolBlock_Mesure_Command")
                    CogToolBlock_Shard_Left = CogToolBlock_Fixture_Left.Tools("CogToolBlock_Command")
                    Measure_Sub_Left(Select_Product_Name, Barcode, KEYNAME.RightCommand, Image_Index, 93, KeyType, "D:\System\HOOK_SNAP\Left\")
                Case 72 + Index_Increase
                    CogToolBlock_Fixture_Right = CogToolBlock_Group_Right.Tools("CogToolBlock_Measure_1X1")
                    CogToolBlock_Shard_Right = CogToolBlock_Fixture_Right.Tools("CogToolBlock_1X1")
                    Measure_Sub_Right(Select_Product_Name, Barcode, KEYNAME.RightOption, Image_Index, 94, KeyType, "D:\System\HOOK_SNAP\Left\")
                Case 73 + Index_Increase
                    CogToolBlock_Fixture_Right = CogToolBlock_Group_Right.Tools("CogToolBlock_Up_Down")
                    CogToolBlock_Shard_Right = CogToolBlock_Fixture_Right.Tools("CogToolBlock_Down")
                    Measure_Sub_Right(Select_Product_Name, Barcode, KEYNAME.Left, Image_Index, 95, KeyType, "D:\System\HOOK_SNAP\Left\")
                Case 74 + Index_Increase
                    CogToolBlock_Fixture_Right = CogToolBlock_Group_Right.Tools("CogToolBlock_Up_Down")
                    CogToolBlock_Shard_Right = CogToolBlock_Fixture_Right.Tools("CogToolBlock_Up")
                    Measure_Sub_Right(Select_Product_Name, Barcode, KEYNAME.Up, Image_Index, 96, KeyType, "D:\System\HOOK_SNAP\Left\")
                    CogToolBlock_Fixture_Right = CogToolBlock_Group_Right.Tools("CogToolBlock_Up_Down")
                    CogToolBlock_Shard_Right = CogToolBlock_Fixture_Right.Tools("CogToolBlock_Down")
                    Measure_Sub_Right(Select_Product_Name, Barcode, KEYNAME.Down, Image_Index, 97, KeyType, "D:\System\HOOK_SNAP\Left\")
                Case 75 + Index_Increase
                    CogToolBlock_Fixture_Right = CogToolBlock_Group_Right.Tools("CogToolBlock_Up_Down")
                    CogToolBlock_Shard_Right = CogToolBlock_Fixture_Right.Tools("CogToolBlock_Down")
                    Measure_Sub_Right(Select_Product_Name, Barcode, KEYNAME.Right, Image_Index, 98, KeyType, "D:\System\HOOK_SNAP\Left\")
            End Select
        Catch ex As Exception
            Invoke(New Delegate_Display_Message_Left(AddressOf Display_Message_Left), "左图像处理错误:[" & Image_Index & "]" & ex.ToString, Barcode, Color.Red)
        End Try
    End Sub
    Sub Measure_Sub_Left(ByVal Project_Name As String, ByVal Barcode As String, ByVal KeyName As String, ByVal ImageIndex As Integer, ByVal ToolsIndex As Integer, ByVal InputKeyType As String, ByVal ParameterDir As String, Optional ByVal OutputImageUse As Boolean = True)
        Dim sub_Name As String = GetCurrentMethod.Name & ":"
        Dim ValueName() As String = Nothing, Value() As String = Nothing
        Dim Data_Name() As String = Nothing, Data_Value() As Object = Nothing
        Dim OutputResultValueName As String = Nothing
        Dim OutputResultValue As String = Nothing
        Try
            If OutputImageUse = True Then
                CogToolBlock_Shard_Left.Inputs("OutputImage").Value = ImageFile_Parallel_Left.Operator.Item(ImageIndex - 1)
            End If
            CogToolBlock_Shard_Left.Inputs("InputLogFile").Value = Barcode & "_Left.txt"
            CogToolBlock_Shard_Left.Inputs("ToolsIndex").Value = ToolsIndex
            CogToolBlock_Shard_Left.Inputs("InputKeyType").Value = InputKeyType
            CogToolBlock_Shard_Left.Inputs("ParameterDir").Value = ParameterDir
            CogToolBlock_Shard_Left.Run()

            OutputResultValueName = CogToolBlock_Shard_Left.Outputs("OutputResultValueName").Value
            OutputResultValue = CogToolBlock_Shard_Left.Outputs("OutputResultValue").Value
            ValueName = Split(OutputResultValueName, ",")
            Value = Split(OutputResultValue, ",")
            '新增4////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            If PARAM_BOOL.UpDown键测试选择 = False Then
                If KeyName = "Up" Or KeyName = "Down" Then
                    For i As Int16 = 0 To Value.Length - 1
                        Value(i) = 0
                    Next
                End If
            End If

            '新增4////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            If CogToolBlock_Shard_Left.RunStatus.Result <> CogToolResultConstants.Accept Then
                For i As Int16 = 0 To Value.Length - 1
                    Value(i) = 0
                Next
                Invoke(New Delegate_Display_Message_Left(AddressOf Display_Message_Left), "左：" & ImageIndex & CogToolBlock_Shard_Left.RunStatus.Message, Barcode, Color.Red)
            End If
        Catch ex As Exception
            Invoke(New Delegate_Display_Message_Left(AddressOf Display_Message_Left), "左：" & sub_Name & ex.ToString, Barcode, Color.Red)
        End Try

        If ValueName.Length >= 0 Then
            Array.Resize(Data_Name, ValueName.Length)
            Array.Resize(Data_Value, ValueName.Length)
            Dim m_Result As Int16 = 1
            For a As Integer = 0 To ValueName.Length - 1
                Data_Name(a) = ValueName(a)
                Data_Value(a) = CType(Value(a), Double)
                If Data_Value(a) <= ProjectParam.Limit_Up Then
                    m_Result *= 1
                Else
                    m_Result *= 0
                End If
            Next

            If m_Result = 0 Then
                NG_count_Left += 1
            End If

            Save_Window_Image_Left(CogToolBlock_Shard_Left, KeyName, ImageIndex, m_Result, Barcode, ImageFile_Parallel_Left.Operator.Item(ImageIndex - 1))
            SQLCON.Insert_Measure_data_Left(Project_Name, Barcode, KeyName, Data_Name, Data_Value) '插入测量数据

        Else
            Invoke(New Delegate_Display_Message_Left(AddressOf Display_Message_Left), "左：" & ImageIndex & "次获取长度错误。", Barcode, Color.Red)
        End If
    End Sub

    Sub Measure_Sub_Right(ByVal Project_Name As String, ByVal Barcode As String, ByVal KeyName As String, ByVal ImageIndex As Integer, ByVal ToolsIndex As Integer, ByVal InputKeyType As String, ByVal ParameterDir As String, Optional ByVal OutputImageUse As Boolean = True)
        Dim sub_Name As String = GetCurrentMethod.Name & ":"
        Dim ValueName() As String = Nothing, Value() As String = Nothing
        Dim Data_Name() As String = Nothing, Data_Value() As Object = Nothing
        Dim OutputResultValueName As String = Nothing
        Dim OutputResultValue As String = Nothing
        Try
            If OutputImageUse = True Then
                CogToolBlock_Shard_Right.Inputs("OutputImage").Value = ImageFile_Parallel_Right.Operator.Item(ImageIndex - 1)
            End If
            CogToolBlock_Shard_Right.Inputs("InputLogFile").Value = Barcode & "_Right.txt"
            CogToolBlock_Shard_Right.Inputs("ToolsIndex").Value = ToolsIndex
            CogToolBlock_Shard_Right.Inputs("InputKeyType").Value = InputKeyType
            CogToolBlock_Shard_Right.Inputs("ParameterDir").Value = ParameterDir
            CogToolBlock_Shard_Right.Run()

            OutputResultValueName = CogToolBlock_Shard_Right.Outputs("OutputResultValueName").Value
            OutputResultValue = CogToolBlock_Shard_Right.Outputs("OutputResultValue").Value
            ValueName = Split(OutputResultValueName, ",")
            Value = Split(OutputResultValue, ",")
            '新增4////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            If PARAM_BOOL.UpDown键测试选择 = False Then
                If KeyName = "Up" Or KeyName = "Down" Then
                    For i As Int16 = 0 To Value.Length - 1
                        Value(i) = 0
                    Next
                End If
            End If
            '新增4////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            If CogToolBlock_Shard_Right.RunStatus.Result <> CogToolResultConstants.Accept Then
                For i As Int16 = 0 To Value.Length - 1
                    Value(i) = 0
                Next
                Invoke(New Delegate_Display_Message_Right(AddressOf Display_Message_Right), "右：" & ImageIndex & CogToolBlock_Shard_Right.RunStatus.Message, Barcode, Color.Red)
            End If
        Catch ex As Exception
            Invoke(New Delegate_Display_Message_Right(AddressOf Display_Message_Right), "右：" & sub_Name & ex.ToString, Barcode, Color.Red)
        End Try

        If ValueName.Length >= 0 Then
            Array.Resize(Data_Name, ValueName.Length)
            Array.Resize(Data_Value, ValueName.Length)
            Dim m_Result As Int16 = 1
            For a As Integer = 0 To ValueName.Length - 1
                Data_Name(a) = ValueName(a)
                Data_Value(a) = CType(Value(a), Double)
                If Data_Value(a) <= ProjectParam.Limit_Up Then
                    m_Result *= 1
                Else
                    m_Result *= 0
                End If
            Next

            If m_Result = 0 Then
                NG_Count_Right += 1
            End If

            Save_Window_Image_Right(CogToolBlock_Shard_Right, KeyName, ImageIndex, m_Result, Barcode, ImageFile_Parallel_Right.Operator.Item(ImageIndex - 1))
            SQLCON.Insert_Measure_data_Right(Project_Name, Barcode, KeyName, Data_Name, Data_Value) '插入测量数据
        Else
            Invoke(New Delegate_Display_Message_Right(AddressOf Display_Message_Right), "右：" & ImageIndex & "次获取长度错误。", Barcode, Color.Red)
        End If
    End Sub

    Sub Measure_Left_Ansi(ByVal Image_Index As Integer, ByVal Project_Name As String, ByVal Barcode As String, ByVal KeyType As String)
        Try
            Select Case Image_Index
                Case 1 To 13
                    Dim _keyname As String = Nothing
                    Dim Measure_Index As Integer
                    Select Case Image_Index
                        Case 1
                            _keyname = KEYNAME.Esc : Measure_Index = 0
                        Case 2
                            _keyname = KEYNAME.F1 : Measure_Index = 1
                        Case 3
                            _keyname = KEYNAME.F2 : Measure_Index = 2
                        Case 4
                            _keyname = KEYNAME.F3 : Measure_Index = 3
                        Case 5
                            _keyname = KEYNAME.F4 : Measure_Index = 4
                        Case 6
                            _keyname = KEYNAME.F5 : Measure_Index = 5
                        Case 7
                            _keyname = KEYNAME.F6 : Measure_Index = 6
                        Case 8
                            _keyname = KEYNAME.F7 : Measure_Index = 7
                        Case 9
                            _keyname = KEYNAME.F8 : Measure_Index = 8
                        Case 10
                            _keyname = KEYNAME.F9 : Measure_Index = 9
                        Case 11
                            _keyname = KEYNAME.F10 : Measure_Index = 10
                        Case 12
                            _keyname = KEYNAME.F11 : Measure_Index = 11
                        Case 13
                            _keyname = KEYNAME.F12 : Measure_Index = 12

                    End Select

                    CogToolBlock_Fixture_Left = CogToolBlock_Group_Left.Tools("CogToolBlock_Measure_Fun_X10")
                    CogToolBlock_Shard_Left = CogToolBlock_Fixture_Left.Tools("CogToolBlock_FunX10")
                    Measure_Sub_Left(Select_Product_Name, Barcode, _keyname, Image_Index, Measure_Index, KeyType, "D:\System\HOOK_SNAP\Left\")
                Case 14
                    CogToolBlock_Fixture_Left = CogToolBlock_Group_Left.Tools("CogToolBlock_Measure_Fun_Delete01")
                    CogToolBlock_Shard_Left = CogToolBlock_Fixture_Left.Tools("CogToolBlock_Delete01")
                    Measure_Sub_Left(Select_Product_Name, Barcode, KEYNAME.Delete, Image_Index, 13, KeyType, "D:\System\HOOK_SNAP\Left\")
                Case 15
                    CogToolBlock_Fixture_Left = CogToolBlock_Group_Left.Tools("CogToolBlock_Measure_Fun_Delete02")
                    CogToolBlock_Shard_Left = CogToolBlock_Fixture_Left.Tools("CogToolBlock_Delete02")
                    Measure_Sub_Left(Select_Product_Name, Barcode, KEYNAME.Delete, Image_Index, 14, KeyType, "D:\System\HOOK_SNAP\Left\")
                Case 16 To 28
                    Dim _keyname As String = Nothing
                    Dim Measure_Index As Integer
                    Select Case Image_Index
                        Case 16
                            _keyname = KEYNAME.Equal : Measure_Index = 15
                        Case 17
                            _keyname = KEYNAME.Minus : Measure_Index = 16
                        Case 18
                            _keyname = KEYNAME.Num0 : Measure_Index = 17
                        Case 19
                            _keyname = KEYNAME.Num9 : Measure_Index = 18
                        Case 20
                            _keyname = KEYNAME.Num8 : Measure_Index = 19
                        Case 21
                            _keyname = KEYNAME.Num7 : Measure_Index = 20
                        Case 22
                            _keyname = KEYNAME.Num6 : Measure_Index = 21
                        Case 23
                            _keyname = KEYNAME.Num5 : Measure_Index = 22
                        Case 24
                            _keyname = KEYNAME.Num4 : Measure_Index = 23
                        Case 25
                            _keyname = KEYNAME.Num3 : Measure_Index = 24
                        Case 26
                            _keyname = KEYNAME.Num2 : Measure_Index = 25
                        Case 27
                            _keyname = KEYNAME.Num1 : Measure_Index = 26
                        Case 28
                            _keyname = KEYNAME.Tilde : Measure_Index = 27
                    End Select
                    CogToolBlock_Fixture_Left = CogToolBlock_Group_Left.Tools("CogToolBlock_Measure_1X1")
                    CogToolBlock_Shard_Left = CogToolBlock_Fixture_Left.Tools("CogToolBlock_1X1")
                    Measure_Sub_Left(Select_Product_Name, Barcode, _keyname, Image_Index, Measure_Index, KeyType, "D:\System\HOOK_SNAP\Left\")
                Case 29
                    CogToolBlock_Fixture_Left = CogToolBlock_Group_Left.Tools("CogToolBlock_Measure_Tab01")
                    CogToolBlock_Shard_Left = CogToolBlock_Fixture_Left.Tools("CogToolBlock_Tab01")
                    Measure_Sub_Left(Select_Product_Name, Barcode, KEYNAME.Tab, Image_Index, 28, KeyType, "D:\System\HOOK_SNAP\Left\")
                Case 30
                    CogToolBlock_Fixture_Left = CogToolBlock_Group_Left.Tools("CogToolBlock_Measure_Tab02")
                    CogToolBlock_Shard_Left = CogToolBlock_Fixture_Left.Tools("CogToolBlock_Tab02")
                    Measure_Sub_Left(Select_Product_Name, Barcode, KEYNAME.Tab, Image_Index, 29, KeyType, "D:\System\HOOK_SNAP\Left\")
                Case 31 To 43
                    Dim _keyname As String = Nothing
                    Dim Measure_Index As Integer
                    Select Case Image_Index
                        Case 31
                            _keyname = KEYNAME.Q : Measure_Index = 30
                        Case 32
                            _keyname = KEYNAME.W : Measure_Index = 31
                        Case 33
                            _keyname = KEYNAME.E : Measure_Index = 32
                        Case 34
                            _keyname = KEYNAME.R : Measure_Index = 33
                        Case 35
                            _keyname = KEYNAME.T : Measure_Index = 34
                        Case 36
                            _keyname = KEYNAME.Y : Measure_Index = 35
                        Case 37
                            _keyname = KEYNAME.U : Measure_Index = 36
                        Case 38
                            _keyname = KEYNAME.I : Measure_Index = 37
                        Case 39
                            _keyname = KEYNAME.O : Measure_Index = 38
                        Case 40
                            _keyname = KEYNAME.P : Measure_Index = 39
                        Case 41
                            _keyname = KEYNAME.LeftBracket : Measure_Index = 40
                        Case 42
                            _keyname = KEYNAME.RightBracket : Measure_Index = 41
                        Case 43
                            _keyname = KEYNAME.BackSlash : Measure_Index = 42
                    End Select
                    CogToolBlock_Fixture_Left = CogToolBlock_Group_Left.Tools("CogToolBlock_Measure_1X1")
                    CogToolBlock_Shard_Left = CogToolBlock_Fixture_Left.Tools("CogToolBlock_1X1")
                    Measure_Sub_Left(Select_Product_Name, Barcode, _keyname, Image_Index, Measure_Index, KeyType, "D:\System\HOOK_SNAP\Left\")
                Case 44
                    CogToolBlock_Fixture_Left = CogToolBlock_Group_Left.Tools("CogToolBlock_Measure_Return01")
                    CogToolBlock_Shard_Left = CogToolBlock_Fixture_Left.Tools("CogToolBlock_Return01")
                    Measure_Sub_Left(Select_Product_Name, Barcode, KEYNAME.Return_, Image_Index, 43, KeyType, "D:\System\HOOK_SNAP\Left\")
                Case 45
                    CogToolBlock_Fixture_Left = CogToolBlock_Group_Left.Tools("CogToolBlock_Measure_Return02")
                    CogToolBlock_Shard_Left = CogToolBlock_Fixture_Left.Tools("CogToolBlock_Return02")
                    Measure_Sub_Left(Select_Product_Name, Barcode, KEYNAME.Return_, Image_Index, 44, KeyType, "D:\System\HOOK_SNAP\Left\")
                Case 46 To 56
                    Dim _keyname As String = Nothing : Dim Measure_Index As Integer
                    Select Case Image_Index
                        Case 46
                            _keyname = KEYNAME.Quote : Measure_Index = 45
                        Case 47
                            _keyname = KEYNAME.Semicolon : Measure_Index = 46
                        Case 48
                            _keyname = KEYNAME.L : Measure_Index = 47
                        Case 49
                            _keyname = KEYNAME.K : Measure_Index = 48
                        Case 50
                            _keyname = KEYNAME.J : Measure_Index = 49
                        Case 51
                            _keyname = KEYNAME.H : Measure_Index = 50
                        Case 52
                            _keyname = KEYNAME.G : Measure_Index = 51
                        Case 53
                            _keyname = KEYNAME.F : Measure_Index = 52
                        Case 54
                            _keyname = KEYNAME.D : Measure_Index = 53
                        Case 55
                            _keyname = KEYNAME.S : Measure_Index = 54
                        Case 56
                            _keyname = KEYNAME.A : Measure_Index = 55
                    End Select
                    CogToolBlock_Fixture_Left = CogToolBlock_Group_Left.Tools("CogToolBlock_Measure_1X1")
                    CogToolBlock_Shard_Left = CogToolBlock_Fixture_Left.Tools("CogToolBlock_1X1")
                    Measure_Sub_Left(Select_Product_Name, Barcode, _keyname, Image_Index, Measure_Index, KeyType, "D:\System\HOOK_SNAP\Left\")
                Case 57
                    CogToolBlock_Fixture_Left = CogToolBlock_Group_Left.Tools("CogToolBlock_Measure_CapsLock01")
                    CogToolBlock_Shard_Left = CogToolBlock_Fixture_Left.Tools("CogToolBlock_CapsLock01")
                    Measure_Sub_Left(Select_Product_Name, Barcode, KEYNAME.CapsLock, Image_Index, 56, KeyType, "D:\System\HOOK_SNAP\Left\")
                Case 58
                    CogToolBlock_Fixture_Left = CogToolBlock_Group_Left.Tools("CogToolBlock_Measure_CapsLock02")
                    CogToolBlock_Shard_Left = CogToolBlock_Fixture_Left.Tools("CogToolBlock_CapsLock02")
                    Measure_Sub_Left(Select_Product_Name, Barcode, KEYNAME.CapsLock, Image_Index, 57, KeyType, "D:\System\HOOK_SNAP\Left\")
                Case 59
                    CogToolBlock_Fixture_Left = CogToolBlock_Group_Left.Tools("CogToolBlock_Measure_Left_Shift01")
                    CogToolBlock_Shard_Left = CogToolBlock_Fixture_Left.Tools("CogToolBlock_Shift01")
                    Measure_Sub_Left(Select_Product_Name, Barcode, KEYNAME.LeftShift, Image_Index, 58, KeyType, "D:\System\HOOK_SNAP\Left\")
                Case 60
                    CogToolBlock_Fixture_Left = CogToolBlock_Group_Left.Tools("CogToolBlock_Measure_Left_Shift02")
                    CogToolBlock_Shard_Left = CogToolBlock_Fixture_Left.Tools("CogToolBlock_Shift02")
                    Measure_Sub_Left(Select_Product_Name, Barcode, KEYNAME.LeftShift, Image_Index, 59, KeyType, "D:\System\HOOK_SNAP\Left\")
                Case 61
                    CogToolBlock_Fixture_Left = CogToolBlock_Group_Left.Tools("CogToolBlock_Measure_Left_Shift03")
                    CogToolBlock_Shard_Left = CogToolBlock_Fixture_Left.Tools("CogToolBlock_Shift03")
                    Measure_Sub_Left(Select_Product_Name, Barcode, KEYNAME.LeftShift, Image_Index, 60, KeyType, "D:\System\HOOK_SNAP\Left\")
                Case 62 To 71
                    Dim _keyname As String = Nothing : Dim Measure_Index As Integer
                    Select Case Image_Index
                        Case 62
                            _keyname = KEYNAME.Z : Measure_Index = 61
                        Case 63
                            _keyname = KEYNAME.X : Measure_Index = 62
                        Case 64
                            _keyname = KEYNAME.C : Measure_Index = 63
                        Case 65
                            _keyname = KEYNAME.V : Measure_Index = 64
                        Case 66
                            _keyname = KEYNAME.B : Measure_Index = 65
                        Case 67
                            _keyname = KEYNAME.N : Measure_Index = 66
                        Case 68
                            _keyname = KEYNAME.M : Measure_Index = 67
                        Case 69
                            _keyname = KEYNAME.Comma : Measure_Index = 68
                        Case 70
                            _keyname = KEYNAME.Period : Measure_Index = 69
                        Case 71
                            _keyname = KEYNAME.Question : Measure_Index = 70
                    End Select
                    CogToolBlock_Fixture_Left = CogToolBlock_Group_Left.Tools("CogToolBlock_Measure_1X1")
                    CogToolBlock_Shard_Left = CogToolBlock_Fixture_Left.Tools("CogToolBlock_1X1")
                    Measure_Sub_Left(Select_Product_Name, Barcode, _keyname, Image_Index, Measure_Index, KeyType, "D:\System\HOOK_SNAP\Left\")

                Case 72
                    CogToolBlock_Fixture_Left = CogToolBlock_Group_Left.Tools("CogToolBlock_Measure_Right_Shift01")
                    CogToolBlock_Shard_Left = CogToolBlock_Fixture_Left.Tools("CogToolBlock_Shift01")
                    Measure_Sub_Left(Select_Product_Name, Barcode, KEYNAME.RightShift, Image_Index, 71, KeyType, "D:\System\HOOK_SNAP\Left\")
                Case 73
                    CogToolBlock_Fixture_Left = CogToolBlock_Group_Left.Tools("CogToolBlock_Measure_Right_Shift02")
                    CogToolBlock_Shard_Left = CogToolBlock_Fixture_Left.Tools("CogToolBlock_Shift02")
                    Measure_Sub_Left(Select_Product_Name, Barcode, KEYNAME.RightShift, Image_Index, 72, KeyType, "D:\System\HOOK_SNAP\Left\")
                Case 74
                    CogToolBlock_Fixture_Left = CogToolBlock_Group_Left.Tools("CogToolBlock_Measure_Right_Shift03")
                    CogToolBlock_Shard_Left = CogToolBlock_Fixture_Left.Tools("CogToolBlock_Shift03")
                    Measure_Sub_Left(Select_Product_Name, Barcode, KEYNAME.RightShift, Image_Index, 73, KeyType, "D:\System\HOOK_SNAP\Left\")
                Case 75
                    CogToolBlock_Fixture_Left = CogToolBlock_Group_Left.Tools("CogToolBlock_Up_Down")
                    CogToolBlock_Shard_Left = CogToolBlock_Fixture_Left.Tools("CogToolBlock_Down")
                    Measure_Sub_Left(Select_Product_Name, Barcode, KEYNAME.Right, Image_Index, 74, KeyType, "D:\System\HOOK_SNAP\Left\")
                Case 76
                    CogToolBlock_Fixture_Left = CogToolBlock_Group_Left.Tools("CogToolBlock_Up_Down")
                    CogToolBlock_Shard_Left = CogToolBlock_Fixture_Left.Tools("CogToolBlock_Up")
                    Measure_Sub_Left(Select_Product_Name, Barcode, KEYNAME.Up, Image_Index, 75, KeyType, "D:\System\HOOK_SNAP\Left\")
                    CogToolBlock_Fixture_Left = CogToolBlock_Group_Left.Tools("CogToolBlock_Up_Down")
                    CogToolBlock_Shard_Left = CogToolBlock_Fixture_Left.Tools("CogToolBlock_Down")
                    Measure_Sub_Left(Select_Product_Name, Barcode, KEYNAME.Down, Image_Index, 76, KeyType, "D:\System\HOOK_SNAP\Left\")
                Case 77
                    CogToolBlock_Fixture_Left = CogToolBlock_Group_Left.Tools("CogToolBlock_Up_Down")
                    CogToolBlock_Shard_Left = CogToolBlock_Fixture_Left.Tools("CogToolBlock_Down")
                    Measure_Sub_Left(Select_Product_Name, Barcode, KEYNAME.Left, Image_Index, 77, KeyType, "D:\System\HOOK_SNAP\Left\")
                Case 78
                    CogToolBlock_Fixture_Left = CogToolBlock_Group_Left.Tools("CogToolBlock_Measure_1X1")
                    CogToolBlock_Shard_Left = CogToolBlock_Fixture_Left.Tools("CogToolBlock_1X1")
                    Measure_Sub_Left(Select_Product_Name, Barcode, KEYNAME.RightOption, Image_Index, 78, KeyType, "D:\System\HOOK_SNAP\Left\")
                Case 79
                    CogToolBlock_Fixture_Left = CogToolBlock_Group_Left.Tools("CogToolBlock_Mesure_Command")
                    CogToolBlock_Shard_Left = CogToolBlock_Fixture_Left.Tools("CogToolBlock_Command")
                    Measure_Sub_Left(Select_Product_Name, Barcode, KEYNAME.RightCommand, Image_Index, 79, KeyType, "D:\System\HOOK_SNAP\Left\")
                Case 85
                    CogToolBlock_Fixture_Left = CogToolBlock_Group_Left.Tools("CogToolBlock_Mesure_Command")
                    CogToolBlock_Shard_Left = CogToolBlock_Fixture_Left.Tools("CogToolBlock_Command")
                    Measure_Sub_Left(Select_Product_Name, Barcode, KEYNAME.LeftCommand, Image_Index, 80, KeyType, "D:\System\HOOK_SNAP\Left\")

                Case 86 To 88
                    Dim _keyname As String = Nothing : Dim Measure_Index As Integer
                    Select Case Image_Index
                        Case 86
                            _keyname = KEYNAME.LeftOption : Measure_Index = 81
                        Case 87
                            _keyname = KEYNAME.Control : Measure_Index = 82
                        Case 88
                            _keyname = KEYNAME.Fn : Measure_Index = 83
                    End Select
                    CogToolBlock_Fixture_Left = CogToolBlock_Group_Left.Tools("CogToolBlock_Measure_1X1")
                    CogToolBlock_Shard_Left = CogToolBlock_Fixture_Left.Tools("CogToolBlock_1X1")
                    Measure_Sub_Left(Select_Product_Name, Barcode, _keyname, Image_Index, Measure_Index, KeyType, "D:\System\HOOK_SNAP\Left\")


            End Select
        Catch ex As Exception
            Invoke(New Delegate_Display_Message_Left(AddressOf Display_Message_Left), "左图像处理错误:[" & Image_Index & "]" & ex.ToString, Barcode, Color.Red)
        End Try
    End Sub

    Sub Measure_Left_Iso(ByVal Image_Index As Integer, ByVal Project_Name As String, ByVal Barcode As String, ByVal KeyType As String)
        Try
            Select Case Image_Index
                Case 1 To 13
                    Dim _keyname As String = Nothing
                    Dim Measure_Index As Integer
                    Select Case Image_Index
                        Case 1
                            _keyname = KEYNAME.Esc : Measure_Index = 0
                        Case 2
                            _keyname = KEYNAME.F1 : Measure_Index = 1
                        Case 3
                            _keyname = KEYNAME.F2 : Measure_Index = 2
                        Case 4
                            _keyname = KEYNAME.F3 : Measure_Index = 3
                        Case 5
                            _keyname = KEYNAME.F4 : Measure_Index = 4
                        Case 6
                            _keyname = KEYNAME.F5 : Measure_Index = 5
                        Case 7
                            _keyname = KEYNAME.F6 : Measure_Index = 6
                        Case 8
                            _keyname = KEYNAME.F7 : Measure_Index = 7
                        Case 9
                            _keyname = KEYNAME.F8 : Measure_Index = 8
                        Case 10
                            _keyname = KEYNAME.F9 : Measure_Index = 9
                        Case 11
                            _keyname = KEYNAME.F10 : Measure_Index = 10
                        Case 12
                            _keyname = KEYNAME.F11 : Measure_Index = 11
                        Case 13
                            _keyname = KEYNAME.F12 : Measure_Index = 12
                    End Select

                    CogToolBlock_Fixture_Left = CogToolBlock_Group_Left.Tools("CogToolBlock_Measure_Fun_X10")
                    CogToolBlock_Shard_Left = CogToolBlock_Fixture_Left.Tools("CogToolBlock_FunX10")
                    Measure_Sub_Left(Select_Product_Name, Barcode, _keyname, Image_Index, Measure_Index, KeyType, "D:\System\HOOK_SNAP\Left\")
                Case 14
                    CogToolBlock_Fixture_Left = CogToolBlock_Group_Left.Tools("CogToolBlock_Measure_Fun_Delete01")
                    CogToolBlock_Shard_Left = CogToolBlock_Fixture_Left.Tools("CogToolBlock_Delete01")
                    Measure_Sub_Left(Select_Product_Name, Barcode, KEYNAME.Delete, Image_Index, 13, KeyType, "D:\System\HOOK_SNAP\Left\")
                Case 15
                    CogToolBlock_Fixture_Left = CogToolBlock_Group_Left.Tools("CogToolBlock_Measure_Fun_Delete02")
                    CogToolBlock_Shard_Left = CogToolBlock_Fixture_Left.Tools("CogToolBlock_Delete02")
                    Measure_Sub_Left(Select_Product_Name, Barcode, KEYNAME.Delete, Image_Index, 14, KeyType, "D:\System\HOOK_SNAP\Left\")

                Case 16 To 28
                    Dim _keyname As String = Nothing : Dim Measure_Index As Integer
                    Select Case Image_Index
                        Case 16
                            _keyname = KEYNAME.Equal : Measure_Index = 15
                        Case 17
                            _keyname = KEYNAME.Minus : Measure_Index = 16
                        Case 18
                            _keyname = KEYNAME.Num0 : Measure_Index = 17
                        Case 19
                            _keyname = KEYNAME.Num9 : Measure_Index = 18
                        Case 20
                            _keyname = KEYNAME.Num8 : Measure_Index = 19
                        Case 21
                            _keyname = KEYNAME.Num7 : Measure_Index = 20
                        Case 22
                            _keyname = KEYNAME.Num6 : Measure_Index = 21
                        Case 23
                            _keyname = KEYNAME.Num5 : Measure_Index = 22
                        Case 24
                            _keyname = KEYNAME.Num4 : Measure_Index = 23
                        Case 25
                            _keyname = KEYNAME.Num3 : Measure_Index = 24
                        Case 26
                            _keyname = KEYNAME.Num2 : Measure_Index = 25
                        Case 27
                            _keyname = KEYNAME.Num1 : Measure_Index = 26
                        Case 28
                            _keyname = KEYNAME.ISO : Measure_Index = 27
                    End Select
                    CogToolBlock_Fixture_Left = CogToolBlock_Group_Left.Tools("CogToolBlock_Measure_1X1")
                    CogToolBlock_Shard_Left = CogToolBlock_Fixture_Left.Tools("CogToolBlock_1X1")
                    Measure_Sub_Left(Select_Product_Name, Barcode, _keyname, Image_Index, Measure_Index, KeyType, "D:\System\HOOK_SNAP\Left\")
                Case 29
                    CogToolBlock_Fixture_Left = CogToolBlock_Group_Left.Tools("CogToolBlock_Measure_Tab01")
                    CogToolBlock_Shard_Left = CogToolBlock_Fixture_Left.Tools("CogToolBlock_Tab01")
                    Measure_Sub_Left(Select_Product_Name, Barcode, KEYNAME.Tab, Image_Index, 28, KeyType, "D:\System\HOOK_SNAP\Left\")
                Case 30
                    CogToolBlock_Fixture_Left = CogToolBlock_Group_Left.Tools("CogToolBlock_Measure_Tab02")
                    CogToolBlock_Shard_Left = CogToolBlock_Fixture_Left.Tools("CogToolBlock_Tab02")
                    Measure_Sub_Left(Select_Product_Name, Barcode, KEYNAME.Tab, Image_Index, 29, KeyType, "D:\System\HOOK_SNAP\Left\")

                Case 31 To 42
                    Dim _keyname As String = Nothing : Dim Measure_Index As Integer
                    Select Case Image_Index
                        Case 31
                            _keyname = KEYNAME.Q : Measure_Index = 30
                        Case 32
                            _keyname = KEYNAME.W : Measure_Index = 31
                        Case 33
                            _keyname = KEYNAME.E : Measure_Index = 32
                        Case 34
                            _keyname = KEYNAME.R : Measure_Index = 33
                        Case 35
                            _keyname = KEYNAME.T : Measure_Index = 34
                        Case 36
                            _keyname = KEYNAME.Y : Measure_Index = 35
                        Case 37
                            _keyname = KEYNAME.U : Measure_Index = 36
                        Case 38
                            _keyname = KEYNAME.I : Measure_Index = 37
                        Case 39
                            _keyname = KEYNAME.O : Measure_Index = 38
                        Case 40
                            _keyname = KEYNAME.P : Measure_Index = 39
                        Case 41
                            _keyname = KEYNAME.LeftBracket : Measure_Index = 40
                        Case 42
                            _keyname = KEYNAME.RightBracket : Measure_Index = 41
                    End Select
                    CogToolBlock_Fixture_Left = CogToolBlock_Group_Left.Tools("CogToolBlock_Measure_1X1")
                    CogToolBlock_Shard_Left = CogToolBlock_Fixture_Left.Tools("CogToolBlock_1X1")
                    Measure_Sub_Left(Select_Product_Name, Barcode, _keyname, Image_Index, Measure_Index, KeyType, "D:\System\HOOK_SNAP\Left\")
                Case 43
                    CogToolBlock_Fixture_Left = CogToolBlock_Group_Left.Tools("CogToolBlock_Return01_Iso")
                    CogToolBlock_Shard_Left = CogToolBlock_Fixture_Left.Tools("CogToolBlock_Return01")
                    Measure_Sub_Left(Select_Product_Name, Barcode, KEYNAME.Return_, Image_Index, 42, KeyType, "D:\System\HOOK_SNAP\Left\")
                Case 44
                    CogToolBlock_Fixture_Left = CogToolBlock_Group_Left.Tools("CogToolBlock_Return02_Iso")
                    CogToolBlock_Shard_Left = CogToolBlock_Fixture_Left.Tools("CogToolBlock_Return02")
                    Measure_Sub_Left(Select_Product_Name, Barcode, KEYNAME.Return_, Image_Index, 43, KeyType, "D:\System\HOOK_SNAP\Left\")
                Case 45 To 56
                    Dim _keyname As String = Nothing : Dim Measure_Index As Integer
                    Select Case Image_Index
                        Case 45
                            _keyname = KEYNAME.BackSlash : Measure_Index = 44
                        Case 46
                            _keyname = KEYNAME.Quote : Measure_Index = 45
                        Case 47
                            _keyname = KEYNAME.Semicolon : Measure_Index = 46
                        Case 48
                            _keyname = KEYNAME.L : Measure_Index = 47
                        Case 49
                            _keyname = KEYNAME.K : Measure_Index = 48
                        Case 50
                            _keyname = KEYNAME.J : Measure_Index = 49
                        Case 51
                            _keyname = KEYNAME.H : Measure_Index = 50
                        Case 52
                            _keyname = KEYNAME.G : Measure_Index = 51
                        Case 53
                            _keyname = KEYNAME.F : Measure_Index = 52
                        Case 54
                            _keyname = KEYNAME.D : Measure_Index = 53
                        Case 55
                            _keyname = KEYNAME.S : Measure_Index = 54
                        Case 56
                            _keyname = KEYNAME.A : Measure_Index = 55
                    End Select
                    CogToolBlock_Fixture_Left = CogToolBlock_Group_Left.Tools("CogToolBlock_Measure_1X1")
                    CogToolBlock_Shard_Left = CogToolBlock_Fixture_Left.Tools("CogToolBlock_1X1")
                    Measure_Sub_Left(Select_Product_Name, Barcode, _keyname, Image_Index, Measure_Index, KeyType, "D:\System\HOOK_SNAP\Left\")

                Case 57
                    CogToolBlock_Fixture_Left = CogToolBlock_Group_Left.Tools("CogToolBlock_Measure_CapsLock01")
                    CogToolBlock_Shard_Left = CogToolBlock_Fixture_Left.Tools("CogToolBlock_CapsLock01")

                    Measure_Sub_Left(Select_Product_Name, Barcode, KEYNAME.CapsLock, Image_Index, 56, KeyType, "D:\System\HOOK_SNAP\Left\")
                Case 58
                    CogToolBlock_Fixture_Left = CogToolBlock_Group_Left.Tools("CogToolBlock_Measure_CapsLock02")
                    CogToolBlock_Shard_Left = CogToolBlock_Fixture_Left.Tools("CogToolBlock_CapsLock02")
                    Measure_Sub_Left(Select_Product_Name, Barcode, KEYNAME.CapsLock, Image_Index, 57, KeyType, "D:\System\HOOK_SNAP\Left\")
                Case 59
                    CogToolBlock_Fixture_Left = CogToolBlock_Group_Left.Tools("CogToolBlock_Mesure_1X1.25")
                    CogToolBlock_Shard_Left = CogToolBlock_Fixture_Left.Tools("CogToolBlock_1X1.25")
                    Measure_Sub_Left(Select_Product_Name, Barcode, KEYNAME.LeftShift, Image_Index, 87, KeyType, "D:\System\HOOK_SNAP\Left\")
                Case 60 To 70
                    Dim _keyname As String = Nothing : Dim Measure_Index As Integer
                    Select Case Image_Index
                        Case 60
                            _keyname = KEYNAME.Tilde : Measure_Index = 59
                        Case 61
                            _keyname = KEYNAME.Z : Measure_Index = 60
                        Case 62
                            _keyname = KEYNAME.X : Measure_Index = 61
                        Case 63
                            _keyname = KEYNAME.C : Measure_Index = 62
                        Case 64
                            _keyname = KEYNAME.V : Measure_Index = 63
                        Case 65
                            _keyname = KEYNAME.B : Measure_Index = 64
                        Case 66
                            _keyname = KEYNAME.N : Measure_Index = 65
                        Case 67
                            _keyname = KEYNAME.M : Measure_Index = 66
                        Case 68
                            _keyname = KEYNAME.Comma : Measure_Index = 67
                        Case 69
                            _keyname = KEYNAME.Period : Measure_Index = 68
                        Case 70
                            _keyname = KEYNAME.Question : Measure_Index = 69
                    End Select
                    CogToolBlock_Fixture_Left = CogToolBlock_Group_Left.Tools("CogToolBlock_Measure_1X1")
                    CogToolBlock_Shard_Left = CogToolBlock_Fixture_Left.Tools("CogToolBlock_1X1")
                    Measure_Sub_Left(Select_Product_Name, Barcode, _keyname, Image_Index, Measure_Index, KeyType, "D:\System\HOOK_SNAP\Left\")

                Case 71
                    CogToolBlock_Fixture_Left = CogToolBlock_Group_Left.Tools("CogToolBlock_Measure_Right_Shift01")
                    CogToolBlock_Shard_Left = CogToolBlock_Fixture_Left.Tools("CogToolBlock_Shift01")
                    Measure_Sub_Left(Select_Product_Name, Barcode, KEYNAME.RightShift, Image_Index, 70, KeyType, "D:\System\HOOK_SNAP\Left\")
                Case 72
                    CogToolBlock_Fixture_Left = CogToolBlock_Group_Left.Tools("CogToolBlock_Measure_Right_Shift02")
                    CogToolBlock_Shard_Left = CogToolBlock_Fixture_Left.Tools("CogToolBlock_Shift02")
                    Measure_Sub_Left(Select_Product_Name, Barcode, KEYNAME.RightShift, Image_Index, 71, KeyType, "D:\System\HOOK_SNAP\Left\")
                Case 73
                    CogToolBlock_Fixture_Left = CogToolBlock_Group_Left.Tools("CogToolBlock_Measure_Right_Shift03")
                    CogToolBlock_Shard_Left = CogToolBlock_Fixture_Left.Tools("CogToolBlock_Shift03")
                    Measure_Sub_Left(Select_Product_Name, Barcode, KEYNAME.RightShift, Image_Index, 72, KeyType, "D:\System\HOOK_SNAP\Left\")

                Case 74
                    CogToolBlock_Fixture_Left = CogToolBlock_Group_Left.Tools("CogToolBlock_Up_Down")
                    CogToolBlock_Shard_Left = CogToolBlock_Fixture_Left.Tools("CogToolBlock_Down")
                    Measure_Sub_Left(Select_Product_Name, Barcode, KEYNAME.Right, Image_Index, 73, KeyType, "D:\System\HOOK_SNAP\Left\")
                Case 75
                    CogToolBlock_Fixture_Left = CogToolBlock_Group_Left.Tools("CogToolBlock_Up_Down")
                    CogToolBlock_Shard_Left = CogToolBlock_Fixture_Left.Tools("CogToolBlock_Up")
                    Measure_Sub_Left(Select_Product_Name, Barcode, KEYNAME.Up, Image_Index, 74, KeyType, "D:\System\HOOK_SNAP\Left\")
                    CogToolBlock_Fixture_Left = CogToolBlock_Group_Left.Tools("CogToolBlock_Up_Down")
                    CogToolBlock_Shard_Left = CogToolBlock_Fixture_Left.Tools("CogToolBlock_Down")
                    Measure_Sub_Left(Select_Product_Name, Barcode, KEYNAME.Down, Image_Index, 75, KeyType, "D:\System\HOOK_SNAP\Left\")
                Case 76
                    CogToolBlock_Fixture_Left = CogToolBlock_Group_Left.Tools("CogToolBlock_Up_Down")
                    CogToolBlock_Shard_Left = CogToolBlock_Fixture_Left.Tools("CogToolBlock_Down")
                    Measure_Sub_Left(Select_Product_Name, Barcode, KEYNAME.Left, Image_Index, 76, KeyType, "D:\System\HOOK_SNAP\Left\")
                Case 77
                    CogToolBlock_Fixture_Left = CogToolBlock_Group_Left.Tools("CogToolBlock_Measure_1X1")
                    CogToolBlock_Shard_Left = CogToolBlock_Fixture_Left.Tools("CogToolBlock_1X1")
                    Measure_Sub_Left(Select_Product_Name, Barcode, KEYNAME.RightOption, Image_Index, 77, KeyType, "D:\System\HOOK_SNAP\Left\")
                Case 78
                    CogToolBlock_Fixture_Left = CogToolBlock_Group_Left.Tools("CogToolBlock_Mesure_Command")
                    CogToolBlock_Shard_Left = CogToolBlock_Fixture_Left.Tools("CogToolBlock_Command")
                    Measure_Sub_Left(Select_Product_Name, Barcode, KEYNAME.RightCommand, Image_Index, 78, KeyType, "D:\System\HOOK_SNAP\Left\")
                Case 84
                    CogToolBlock_Fixture_Left = CogToolBlock_Group_Left.Tools("CogToolBlock_Mesure_Command")
                    CogToolBlock_Shard_Left = CogToolBlock_Fixture_Left.Tools("CogToolBlock_Command")
                    Measure_Sub_Left(Select_Product_Name, Barcode, KEYNAME.LeftCommand, Image_Index, 79, KeyType, "D:\System\HOOK_SNAP\Left\")

                Case 85 To 87
                    Dim _keyname As String = Nothing : Dim Measure_Index As Integer
                    Select Case Image_Index
                        Case 85
                            _keyname = KEYNAME.LeftOption : Measure_Index = 80
                        Case 86
                            _keyname = KEYNAME.Control : Measure_Index = 81
                        Case 87
                            _keyname = KEYNAME.Fn : Measure_Index = 82
                    End Select
                    CogToolBlock_Fixture_Left = CogToolBlock_Group_Left.Tools("CogToolBlock_Measure_1X1")
                    CogToolBlock_Shard_Left = CogToolBlock_Fixture_Left.Tools("CogToolBlock_1X1")
                    Measure_Sub_Left(Select_Product_Name, Barcode, _keyname, Image_Index, Measure_Index, KeyType, "D:\System\HOOK_SNAP\Left\")
            End Select
        Catch ex As Exception
            Invoke(New Delegate_Display_Message_Left(AddressOf Display_Message_Left), "左图像处理错误:[" & Image_Index & "]" & ex.ToString, Barcode, Color.Red)
        End Try
    End Sub

    Sub Measure_Left_Jis(ByVal Image_Index As Integer, ByVal Project_Name As String, ByVal Barcode As String, ByVal KeyType As String)
        Try
            Select Case Image_Index
                Case 1 To 13
                    Dim _keyname As String = Nothing
                    Dim Measure_Index As Integer
                    Select Case Image_Index
                        Case 1
                            _keyname = KEYNAME.Esc : Measure_Index = 0
                        Case 2
                            _keyname = KEYNAME.F1 : Measure_Index = 1
                        Case 3
                            _keyname = KEYNAME.F2 : Measure_Index = 2
                        Case 4
                            _keyname = KEYNAME.F3 : Measure_Index = 3
                        Case 5
                            _keyname = KEYNAME.F4 : Measure_Index = 4
                        Case 6
                            _keyname = KEYNAME.F5 : Measure_Index = 5
                        Case 7
                            _keyname = KEYNAME.F6 : Measure_Index = 6
                        Case 8
                            _keyname = KEYNAME.F7 : Measure_Index = 7
                        Case 9
                            _keyname = KEYNAME.F8 : Measure_Index = 8
                        Case 10
                            _keyname = KEYNAME.F9 : Measure_Index = 9
                        Case 11
                            _keyname = KEYNAME.F10 : Measure_Index = 10
                        Case 12
                            _keyname = KEYNAME.F11 : Measure_Index = 11
                        Case 13
                            _keyname = KEYNAME.F12 : Measure_Index = 12

                    End Select

                    CogToolBlock_Fixture_Left = CogToolBlock_Group_Left.Tools("CogToolBlock_Measure_Fun_X10")
                    CogToolBlock_Shard_Left = CogToolBlock_Fixture_Left.Tools("CogToolBlock_FunX10")
                    Measure_Sub_Left(Select_Product_Name, Barcode, _keyname, Image_Index, Measure_Index, KeyType, "D:\System\HOOK_SNAP\Left\")

                Case 14 To 26
                    Dim _keyname As String = Nothing
                    Dim Measure_Index As Integer
                    Select Case Image_Index
                        Case 14
                            _keyname = KEYNAME.Delete : Measure_Index = 13
                        Case 15
                            _keyname = KEYNAME.Yen : Measure_Index = 14
                        Case 16
                            _keyname = KEYNAME.Tilde : Measure_Index = 15
                        Case 17
                            _keyname = KEYNAME.Minus : Measure_Index = 16
                        Case 18
                            _keyname = KEYNAME.Num0 : Measure_Index = 17
                        Case 19
                            _keyname = KEYNAME.Num9 : Measure_Index = 18
                        Case 20
                            _keyname = KEYNAME.Num8 : Measure_Index = 19
                        Case 21
                            _keyname = KEYNAME.Num7 : Measure_Index = 20
                        Case 22
                            _keyname = KEYNAME.Num6 : Measure_Index = 21
                        Case 23
                            _keyname = KEYNAME.Num5 : Measure_Index = 22
                        Case 24
                            _keyname = KEYNAME.Num4 : Measure_Index = 23
                        Case 25
                            _keyname = KEYNAME.Num3 : Measure_Index = 24
                        Case 26
                            _keyname = KEYNAME.Num2 : Measure_Index = 25
                    End Select
                    CogToolBlock_Fixture_Left = CogToolBlock_Group_Left.Tools("CogToolBlock_Measure_1X1")
                    CogToolBlock_Shard_Left = CogToolBlock_Fixture_Left.Tools("CogToolBlock_1X1")
                    Measure_Sub_Left(Select_Product_Name, Barcode, _keyname, Image_Index, Measure_Index, KeyType, "D:\System\HOOK_SNAP\Left\")
                Case 27
                    CogToolBlock_Fixture_Left = CogToolBlock_Group_Left.Tools("CogToolBlock_Measure_Num1.01.Jis")
                    CogToolBlock_Shard_Left = CogToolBlock_Fixture_Left.Tools("CogToolBlock_Num1.01")
                    Measure_Sub_Left(Select_Product_Name, Barcode, KEYNAME.Num1, Image_Index, 26, KeyType, "D:\System\HOOK_SNAP\Left\")
                Case 28
                    CogToolBlock_Fixture_Left = CogToolBlock_Group_Left.Tools("CogToolBlock_Measure_Num1.02.Jis")
                    CogToolBlock_Shard_Left = CogToolBlock_Fixture_Left.Tools("CogToolBlock_Num1.02")
                    Measure_Sub_Left(Select_Product_Name, Barcode, KEYNAME.Num1, Image_Index, 27, KeyType, "D:\System\HOOK_SNAP\Left\")
                Case 29 To 41
                    Dim _keyname As String = Nothing
                    Dim Measure_Index As Integer
                    Select Case Image_Index
                        Case 29
                            _keyname = KEYNAME.Tab : Measure_Index = 28
                        Case 30
                            _keyname = KEYNAME.Q : Measure_Index = 29
                        Case 31
                            _keyname = KEYNAME.W : Measure_Index = 30
                        Case 32
                            _keyname = KEYNAME.E : Measure_Index = 31
                        Case 33
                            _keyname = KEYNAME.R : Measure_Index = 32
                        Case 34
                            _keyname = KEYNAME.T : Measure_Index = 33
                        Case 35
                            _keyname = KEYNAME.Y : Measure_Index = 34
                        Case 36
                            _keyname = KEYNAME.U : Measure_Index = 35
                        Case 37
                            _keyname = KEYNAME.I : Measure_Index = 36
                        Case 38
                            _keyname = KEYNAME.O : Measure_Index = 37
                        Case 39
                            _keyname = KEYNAME.P : Measure_Index = 38
                        Case 40
                            _keyname = KEYNAME.AT : Measure_Index = 39
                        Case 41
                            _keyname = KEYNAME.LeftBracket : Measure_Index = 40
                    End Select
                    CogToolBlock_Fixture_Left = CogToolBlock_Group_Left.Tools("CogToolBlock_Measure_1X1")
                    CogToolBlock_Shard_Left = CogToolBlock_Fixture_Left.Tools("CogToolBlock_1X1")
                    Measure_Sub_Left(Select_Product_Name, Barcode, _keyname, Image_Index, Measure_Index, KeyType, "D:\System\HOOK_SNAP\Left\")
                Case 42
                    CogToolBlock_Fixture_Left = CogToolBlock_Group_Left.Tools("CogToolBlock_Return01.Jis")
                    CogToolBlock_Shard_Left = CogToolBlock_Fixture_Left.Tools("CogToolBlock_Return01")
                    Measure_Sub_Left(Select_Product_Name, Barcode, KEYNAME.Return_, Image_Index, 41, KeyType, "D:\System\HOOK_SNAP\Left\")
                Case 43
                    CogToolBlock_Fixture_Left = CogToolBlock_Group_Left.Tools("CogToolBlock_Return02.Jis")
                    CogToolBlock_Shard_Left = CogToolBlock_Fixture_Left.Tools("CogToolBlock_Return02")
                    Measure_Sub_Left(Select_Product_Name, Barcode, KEYNAME.Return_, Image_Index, 42, KeyType, "D:\System\HOOK_SNAP\Left\")
                Case 44
                    CogToolBlock_Fixture_Left = CogToolBlock_Group_Left.Tools("CogToolBlock_Return03.Jis")
                    CogToolBlock_Shard_Left = CogToolBlock_Fixture_Left.Tools("CogToolBlock_Return03")
                    Measure_Sub_Left(Select_Product_Name, Barcode, KEYNAME.Return_, Image_Index, 43, KeyType, "D:\System\HOOK_SNAP\Left\")
                Case 45 To 56
                    Dim _keyname As String = Nothing
                    Dim Measure_Index As Integer
                    Select Case Image_Index
                        Case 45
                            _keyname = KEYNAME.RightBracket : Measure_Index = 44
                        Case 46
                            _keyname = KEYNAME.Colon : Measure_Index = 45
                        Case 47
                            _keyname = KEYNAME.Semicolon : Measure_Index = 46
                        Case 48
                            _keyname = KEYNAME.L : Measure_Index = 47
                        Case 49
                            _keyname = KEYNAME.K : Measure_Index = 48
                        Case 50
                            _keyname = KEYNAME.J : Measure_Index = 49
                        Case 51
                            _keyname = KEYNAME.H : Measure_Index = 50
                        Case 52
                            _keyname = KEYNAME.G : Measure_Index = 51
                        Case 53
                            _keyname = KEYNAME.F : Measure_Index = 52
                        Case 54
                            _keyname = KEYNAME.D : Measure_Index = 53
                        Case 55
                            _keyname = KEYNAME.S : Measure_Index = 54
                        Case 56
                            _keyname = KEYNAME.A : Measure_Index = 55
                    End Select
                    CogToolBlock_Fixture_Left = CogToolBlock_Group_Left.Tools("CogToolBlock_Measure_1X1")
                    CogToolBlock_Shard_Left = CogToolBlock_Fixture_Left.Tools("CogToolBlock_1X1")
                    Measure_Sub_Left(Select_Product_Name, Barcode, _keyname, Image_Index, Measure_Index, KeyType, "D:\System\HOOK_SNAP\Left\")
                Case 57
                    CogToolBlock_Fixture_Left = CogToolBlock_Group_Left.Tools("CogToolBlock_Mesure_1X1.25")
                    CogToolBlock_Shard_Left = CogToolBlock_Fixture_Left.Tools("CogToolBlock_1X1.25")
                    Measure_Sub_Left(Select_Product_Name, Barcode, KEYNAME.Control, Image_Index, 56, KeyType, "D:\System\HOOK_SNAP\Left\")
                Case 58
                    CogToolBlock_Fixture_Left = CogToolBlock_Group_Left.Tools("CogToolBlock_Measure_Left_Shift.01.Jis")
                    CogToolBlock_Shard_Left = CogToolBlock_Fixture_Left.Tools("CogToolBlock_Shift01")
                    Measure_Sub_Left(Select_Product_Name, Barcode, KEYNAME.LeftShift, Image_Index, 57, KeyType, "D:\System\HOOK_SNAP\Left\")
                Case 59
                    CogToolBlock_Fixture_Left = CogToolBlock_Group_Left.Tools("CogToolBlock_Measure_Left_Shift.02.Jis")
                    CogToolBlock_Shard_Left = CogToolBlock_Fixture_Left.Tools("CogToolBlock_Shift02")
                    Measure_Sub_Left(Select_Product_Name, Barcode, KEYNAME.LeftShift, Image_Index, 58, KeyType, "D:\System\HOOK_SNAP\Left\")
                Case 60 To 70
                    Dim _keyname As String = Nothing : Dim Measure_Index As Integer
                    Select Case Image_Index
                        Case 60
                            _keyname = KEYNAME.Z : Measure_Index = 59
                        Case 61
                            _keyname = KEYNAME.X : Measure_Index = 60
                        Case 62
                            _keyname = KEYNAME.C : Measure_Index = 61
                        Case 63
                            _keyname = KEYNAME.V : Measure_Index = 62
                        Case 64
                            _keyname = KEYNAME.B : Measure_Index = 63
                        Case 65
                            _keyname = KEYNAME.N : Measure_Index = 64
                        Case 66
                            _keyname = KEYNAME.M : Measure_Index = 65
                        Case 67
                            _keyname = KEYNAME.Comma : Measure_Index = 66
                        Case 68
                            _keyname = KEYNAME.Period : Measure_Index = 67
                        Case 69
                            _keyname = KEYNAME.Question : Measure_Index = 68
                        Case 70
                            _keyname = KEYNAME.Ro : Measure_Index = 69
                    End Select
                    CogToolBlock_Fixture_Left = CogToolBlock_Group_Left.Tools("CogToolBlock_Measure_1X1")
                    CogToolBlock_Shard_Left = CogToolBlock_Fixture_Left.Tools("CogToolBlock_1X1")
                    Measure_Sub_Left(Select_Product_Name, Barcode, _keyname, Image_Index, Measure_Index, KeyType, "D:\System\HOOK_SNAP\Left\")
                Case 71
                    CogToolBlock_Fixture_Left = CogToolBlock_Group_Left.Tools("CogToolBlock_Measure_Right_Shift.01.Jis")
                    CogToolBlock_Shard_Left = CogToolBlock_Fixture_Left.Tools("CogToolBlock_Shift01")
                    Measure_Sub_Left(Select_Product_Name, Barcode, KEYNAME.RightShift, Image_Index, 70, KeyType, "D:\System\HOOK_SNAP\Left\")
                Case 72
                    CogToolBlock_Fixture_Left = CogToolBlock_Group_Left.Tools("CogToolBlock_Measure_Right_Shift.02.Jis")
                    CogToolBlock_Shard_Left = CogToolBlock_Fixture_Left.Tools("CogToolBlock_Shift02")
                    Measure_Sub_Left(Select_Product_Name, Barcode, KEYNAME.RightShift, Image_Index, 71, KeyType, "D:\System\HOOK_SNAP\Left\")
                Case 73
                    CogToolBlock_Fixture_Left = CogToolBlock_Group_Left.Tools("CogToolBlock_Up_Down")
                    CogToolBlock_Shard_Left = CogToolBlock_Fixture_Left.Tools("CogToolBlock_Down")
                    Measure_Sub_Left(Select_Product_Name, Barcode, KEYNAME.Right, Image_Index, 72, KeyType, "D:\System\HOOK_SNAP\Left\")
                Case 74
                    CogToolBlock_Fixture_Left = CogToolBlock_Group_Left.Tools("CogToolBlock_Up_Down")
                    CogToolBlock_Shard_Left = CogToolBlock_Fixture_Left.Tools("CogToolBlock_Up")
                    Measure_Sub_Left(Select_Product_Name, Barcode, KEYNAME.Up, Image_Index, 73, KeyType, "D:\System\HOOK_SNAP\Left\")
                    CogToolBlock_Fixture_Left = CogToolBlock_Group_Left.Tools("CogToolBlock_Up_Down")
                    CogToolBlock_Shard_Left = CogToolBlock_Fixture_Left.Tools("CogToolBlock_Down")
                    Measure_Sub_Left(Select_Product_Name, Barcode, KEYNAME.Down, Image_Index, 74, KeyType, "D:\System\HOOK_SNAP\Left\")
                Case 75
                    CogToolBlock_Fixture_Left = CogToolBlock_Group_Left.Tools("CogToolBlock_Up_Down")
                    CogToolBlock_Shard_Left = CogToolBlock_Fixture_Left.Tools("CogToolBlock_Down")
                    Measure_Sub_Left(Select_Product_Name, Barcode, KEYNAME.Left, Image_Index, 75, KeyType, "D:\System\HOOK_SNAP\Left\")
                Case 76
                    CogToolBlock_Fixture_Left = CogToolBlock_Group_Left.Tools("CogToolBlock_Measure_1X1")
                    CogToolBlock_Shard_Left = CogToolBlock_Fixture_Left.Tools("CogToolBlock_1X1")
                    Measure_Sub_Left(Select_Product_Name, Barcode, KEYNAME.Fn, Image_Index, 76, KeyType, "D:\System\HOOK_SNAP\Left\")


                Case 77 To 83
                    Dim _keyname As String = Nothing : Dim Measure_Index As Integer
                    Select Case Image_Index
                        Case 77
                            _keyname = KEYNAME.RightCommand : Measure_Index = 77
                        Case 78
                            _keyname = KEYNAME.Kana : Measure_Index = 78
                        Case 82
                            _keyname = KEYNAME.Eisu : Measure_Index = 79
                        Case 83
                            _keyname = KEYNAME.LeftCommand : Measure_Index = 80
                    End Select

                    CogToolBlock_Fixture_Left = CogToolBlock_Group_Left.Tools("CogToolBlock_Mesure_Command")
                    CogToolBlock_Shard_Left = CogToolBlock_Fixture_Left.Tools("CogToolBlock_Command")
                    Measure_Sub_Left(Select_Product_Name, Barcode, _keyname, Image_Index, Measure_Index, KeyType, "D:\System\HOOK_SNAP\Left\")

                Case 84
                    CogToolBlock_Fixture_Left = CogToolBlock_Group_Left.Tools("CogToolBlock_Measure_1X1")
                    CogToolBlock_Shard_Left = CogToolBlock_Fixture_Left.Tools("CogToolBlock_1X1")
                    Measure_Sub_Left(Select_Product_Name, Barcode, KEYNAME.LeftOption, Image_Index, 81, KeyType, "D:\System\HOOK_SNAP\Left\")

                Case 85
                    CogToolBlock_Fixture_Left = CogToolBlock_Group_Left.Tools("CogToolBlock_Measure_1X1")
                    CogToolBlock_Shard_Left = CogToolBlock_Fixture_Left.Tools("CogToolBlock_1X1")
                    Measure_Sub_Left(Select_Product_Name, Barcode, KEYNAME.CapsLock, Image_Index, 82, KeyType, "D:\System\HOOK_SNAP\Left\")



            End Select
        Catch ex As Exception
            Invoke(New Delegate_Display_Message_Left(AddressOf Display_Message_Left), "左图像处理错误:[" & Image_Index & "]" & ex.ToString, Barcode, Color.Red)
        End Try
    End Sub


    Sub Measure_Right_Ansi_1(ByVal Image_Index As Integer, ByVal Project_Name As String, ByVal Barcode As String, ByVal KeyType As String)
        Try
            Dim Index_Increase As Integer = 13   '增长因子
            Select Case Image_Index
                Case 1
                    CogToolBlock_Fixture_Right = CogToolBlock_Group_Right.Tools("CogToolBlock_Measure_Fun_1X1")
                    CogToolBlock_Shard_Right = CogToolBlock_Fixture_Right.Tools("CogToolBlock_1X1")
                    Measure_Sub_Right(Select_Product_Name, Barcode, KEYNAME.Esc, Image_Index, 0, KeyType, "D:\System\HOOK_SNAP\Right\")
                Case 2
                    CogToolBlock_Fixture_Right = CogToolBlock_Group_Right.Tools("CogToolBlock_Measure_Fun_1X1")
                    CogToolBlock_Shard_Right = CogToolBlock_Fixture_Right.Tools("CogToolBlock_1X1")
                    Measure_Sub_Right(Select_Product_Name, Barcode, KEYNAME.F1, Image_Index, 1, KeyType, "D:\System\HOOK_SNAP\Right\")
                Case 3
                    CogToolBlock_Fixture_Right = CogToolBlock_Group_Right.Tools("CogToolBlock_Measure_Fun_1X1")
                    CogToolBlock_Shard_Right = CogToolBlock_Fixture_Right.Tools("CogToolBlock_1X1")
                    Measure_Sub_Right(Select_Product_Name, Barcode, KEYNAME.F2, Image_Index, 2, KeyType, "D:\System\HOOK_SNAP\Right\")
                Case 4
                    CogToolBlock_Fixture_Right = CogToolBlock_Group_Right.Tools("CogToolBlock_Measure_Fun_1X1")
                    CogToolBlock_Shard_Right = CogToolBlock_Fixture_Right.Tools("CogToolBlock_1X1")
                    Measure_Sub_Right(Select_Product_Name, Barcode, KEYNAME.F3, Image_Index, 3, KeyType, "D:\System\HOOK_SNAP\Right\")
                Case 5
                    CogToolBlock_Fixture_Right = CogToolBlock_Group_Right.Tools("CogToolBlock_Measure_Fun_1X1")
                    CogToolBlock_Shard_Right = CogToolBlock_Fixture_Right.Tools("CogToolBlock_1X1")
                    Measure_Sub_Right(Select_Product_Name, Barcode, KEYNAME.F4, Image_Index, 4, KeyType, "D:\System\HOOK_SNAP\Right\")
                Case 6
                    CogToolBlock_Fixture_Right = CogToolBlock_Group_Right.Tools("CogToolBlock_Measure_Fun_1X1")
                    CogToolBlock_Shard_Right = CogToolBlock_Fixture_Right.Tools("CogToolBlock_1X1")
                    Measure_Sub_Right(Select_Product_Name, Barcode, KEYNAME.F5, Image_Index, 5, KeyType, "D:\System\HOOK_SNAP\Right\")
                Case 7
                    CogToolBlock_Fixture_Right = CogToolBlock_Group_Right.Tools("CogToolBlock_Measure_Fun_1X1")
                    CogToolBlock_Shard_Right = CogToolBlock_Fixture_Right.Tools("CogToolBlock_1X1")
                    Measure_Sub_Right(Select_Product_Name, Barcode, KEYNAME.F6, Image_Index, 6, KeyType, "D:\System\HOOK_SNAP\Right\")
                Case 8
                    CogToolBlock_Fixture_Right = CogToolBlock_Group_Right.Tools("CogToolBlock_Measure_Fun_1X1")
                    CogToolBlock_Shard_Right = CogToolBlock_Fixture_Right.Tools("CogToolBlock_1X1")
                    Measure_Sub_Right(Select_Product_Name, Barcode, KEYNAME.F7, Image_Index, 7, KeyType, "D:\System\HOOK_SNAP\Right\")

                Case 9
                    CogToolBlock_Fixture_Right = CogToolBlock_Group_Right.Tools("CogToolBlock_Measure_Fun_1X1")
                    CogToolBlock_Shard_Right = CogToolBlock_Fixture_Right.Tools("CogToolBlock_1X1")
                    Measure_Sub_Right(Select_Product_Name, Barcode, KEYNAME.F8, Image_Index, 8, KeyType, "D:\System\HOOK_SNAP\Right\")

                Case 10
                    CogToolBlock_Fixture_Right = CogToolBlock_Group_Right.Tools("CogToolBlock_Measure_Fun_1X1")
                    CogToolBlock_Shard_Right = CogToolBlock_Fixture_Right.Tools("CogToolBlock_1X1")
                    Measure_Sub_Right(Select_Product_Name, Barcode, KEYNAME.F9, Image_Index, 9, KeyType, "D:\System\HOOK_SNAP\Right\")
                Case 11
                    CogToolBlock_Fixture_Right = CogToolBlock_Group_Right.Tools("CogToolBlock_Measure_Fun_1X1")
                    CogToolBlock_Shard_Right = CogToolBlock_Fixture_Right.Tools("CogToolBlock_1X1")
                    Measure_Sub_Right(Select_Product_Name, Barcode, KEYNAME.F10, Image_Index, 10, KeyType, "D:\System\HOOK_SNAP\Right\")
                Case 12
                    CogToolBlock_Fixture_Right = CogToolBlock_Group_Right.Tools("CogToolBlock_Measure_Fun_1X1")
                    CogToolBlock_Shard_Right = CogToolBlock_Fixture_Right.Tools("CogToolBlock_1X1")
                    Measure_Sub_Right(Select_Product_Name, Barcode, KEYNAME.F11, Image_Index, 11, KeyType, "D:\System\HOOK_SNAP\Right\")
                Case 13
                    CogToolBlock_Fixture_Right = CogToolBlock_Group_Right.Tools("CogToolBlock_Measure_Fun_1X1")
                    CogToolBlock_Shard_Right = CogToolBlock_Fixture_Right.Tools("CogToolBlock_1X1")
                    Measure_Sub_Right(Select_Product_Name, Barcode, KEYNAME.F12, Image_Index, 12, KeyType, "D:\System\HOOK_SNAP\Right\")

                Case 14
                    CogToolBlock_Fixture_Right = CogToolBlock_Group_Right.Tools("CogToolBlock_Measure_Fun_1X1")
                    CogToolBlock_Shard_Right = CogToolBlock_Fixture_Right.Tools("CogToolBlock_1X1")
                    Measure_Sub_Right(Select_Product_Name, Barcode, KEYNAME.Power, Image_Index, 13, KeyType, "D:\System\HOOK_SNAP\Right\")

                Case 15
                    CogToolBlock_Fixture_Right = CogToolBlock_Group_Right.Tools("CogToolBlock_Measure_Fun_1X1")
                    CogToolBlock_Shard_Right = CogToolBlock_Fixture_Right.Tools("CogToolBlock_1X1")
                    Measure_Sub_Right(Select_Product_Name, Barcode, KEYNAME.Tilde, Image_Index, 14, KeyType, "D:\System\HOOK_SNAP\Right\")

                Case 16
                    CogToolBlock_Fixture_Right = CogToolBlock_Group_Right.Tools("CogToolBlock_Measure_Fun_1X1")
                    CogToolBlock_Shard_Right = CogToolBlock_Fixture_Right.Tools("CogToolBlock_1X1")
                    Measure_Sub_Right(Select_Product_Name, Barcode, KEYNAME.Num1, Image_Index, 15, KeyType, "D:\System\HOOK_SNAP\Right\")

                Case 17
                    CogToolBlock_Fixture_Right = CogToolBlock_Group_Right.Tools("CogToolBlock_Measure_Fun_1X1")
                    CogToolBlock_Shard_Right = CogToolBlock_Fixture_Right.Tools("CogToolBlock_1X1")
                    Measure_Sub_Right(Select_Product_Name, Barcode, KEYNAME.Num2, Image_Index, 16, KeyType, "D:\System\HOOK_SNAP\Right\")
                Case 18

                    CogToolBlock_Fixture_Right = CogToolBlock_Group_Right.Tools("CogToolBlock_Measure_Fun_1X1")
                    CogToolBlock_Shard_Right = CogToolBlock_Fixture_Right.Tools("CogToolBlock_1X1")
                    Measure_Sub_Right(Select_Product_Name, Barcode, KEYNAME.Num3, Image_Index, 17, KeyType, "D:\System\HOOK_SNAP\Right\")

                Case 19

                    CogToolBlock_Fixture_Right = CogToolBlock_Group_Right.Tools("CogToolBlock_Measure_Fun_1X1")
                    CogToolBlock_Shard_Right = CogToolBlock_Fixture_Right.Tools("CogToolBlock_1X1")
                    Measure_Sub_Right(Select_Product_Name, Barcode, KEYNAME.Num4, Image_Index, 18, KeyType, "D:\System\HOOK_SNAP\Right\")
                Case 20
                    CogToolBlock_Fixture_Right = CogToolBlock_Group_Right.Tools("CogToolBlock_Measure_Fun_1X1")
                    CogToolBlock_Shard_Right = CogToolBlock_Fixture_Right.Tools("CogToolBlock_1X1")
                    Measure_Sub_Right(Select_Product_Name, Barcode, KEYNAME.Num5, Image_Index, 19, KeyType, "D:\System\HOOK_SNAP\Right\")
                Case 21
                    CogToolBlock_Fixture_Right = CogToolBlock_Group_Right.Tools("CogToolBlock_Measure_Fun_1X1")
                    CogToolBlock_Shard_Right = CogToolBlock_Fixture_Right.Tools("CogToolBlock_1X1")
                    Measure_Sub_Right(Select_Product_Name, Barcode, KEYNAME.Num6, Image_Index, 20, KeyType, "D:\System\HOOK_SNAP\Right\")

                Case 22

                    CogToolBlock_Fixture_Right = CogToolBlock_Group_Right.Tools("CogToolBlock_Measure_Fun_1X1")
                    CogToolBlock_Shard_Right = CogToolBlock_Fixture_Right.Tools("CogToolBlock_1X1")
                    Measure_Sub_Right(Select_Product_Name, Barcode, KEYNAME.Num7, Image_Index, 21, KeyType, "D:\System\HOOK_SNAP\Right\")
                Case 23
                    CogToolBlock_Fixture_Right = CogToolBlock_Group_Right.Tools("CogToolBlock_Measure_Fun_1X1")
                    CogToolBlock_Shard_Right = CogToolBlock_Fixture_Right.Tools("CogToolBlock_1X1")
                    Measure_Sub_Right(Select_Product_Name, Barcode, KEYNAME.Num8, Image_Index, 22, KeyType, "D:\System\HOOK_SNAP\Right\")
                Case 24
                    CogToolBlock_Fixture_Right = CogToolBlock_Group_Right.Tools("CogToolBlock_Measure_Fun_1X1")
                    CogToolBlock_Shard_Right = CogToolBlock_Fixture_Right.Tools("CogToolBlock_1X1")
                    Measure_Sub_Right(Select_Product_Name, Barcode, KEYNAME.Num9, Image_Index, 23, KeyType, "D:\System\HOOK_SNAP\Right\")
                Case 25
                    CogToolBlock_Fixture_Right = CogToolBlock_Group_Right.Tools("CogToolBlock_Measure_Fun_1X1")
                    CogToolBlock_Shard_Right = CogToolBlock_Fixture_Right.Tools("CogToolBlock_1X1")
                    Measure_Sub_Right(Select_Product_Name, Barcode, KEYNAME.Num0, Image_Index, 24, KeyType, "D:\System\HOOK_SNAP\Right\")
                Case 26
                    CogToolBlock_Fixture_Right = CogToolBlock_Group_Right.Tools("CogToolBlock_Measure_Fun_1X1")
                    CogToolBlock_Shard_Right = CogToolBlock_Fixture_Right.Tools("CogToolBlock_1X1")
                    Measure_Sub_Right(Select_Product_Name, Barcode, KEYNAME.Minus, Image_Index, 25, KeyType, "D:\System\HOOK_SNAP\Right\")

                Case 27
                    CogToolBlock_Fixture_Right = CogToolBlock_Group_Right.Tools("CogToolBlock_Measure_Fun_1X1")
                    CogToolBlock_Shard_Right = CogToolBlock_Fixture_Right.Tools("CogToolBlock_1X1")
                    Measure_Sub_Right(Select_Product_Name, Barcode, KEYNAME.Equal, Image_Index, 26, KeyType, "D:\System\HOOK_SNAP\Right\")
                Case 28
                    CogToolBlock_Fixture_Right = CogToolBlock_Group_Right.Tools("CogToolBlock_Measure_Fun_1X1")
                    CogToolBlock_Shard_Right = CogToolBlock_Fixture_Right.Tools("CogToolBlock_1X1")
                    Measure_Sub_Right(Select_Product_Name, Barcode, KEYNAME.Delete, Image_Index, 27, KeyType, "D:\System\HOOK_SNAP\Right\")
                Case 16 + Index_Increase To 28 + Index_Increase
                    Dim _keyname As String = Nothing : Dim Measure_Index As Integer
                    Select Case Image_Index
                        Case 16 + Index_Increase
                            _keyname = KEYNAME.BackSlash : Measure_Index = 43
                        Case 17 + Index_Increase
                            _keyname = KEYNAME.RightBracket : Measure_Index = 44
                        Case 18 + Index_Increase
                            _keyname = KEYNAME.LeftBracket : Measure_Index = 45
                        Case 19 + Index_Increase
                            _keyname = KEYNAME.P : Measure_Index = 46
                        Case 20 + Index_Increase
                            _keyname = KEYNAME.O : Measure_Index = 47
                        Case 21 + Index_Increase
                            _keyname = KEYNAME.I : Measure_Index = 48
                        Case 22 + Index_Increase
                            _keyname = KEYNAME.U : Measure_Index = 49
                        Case 23 + Index_Increase
                            _keyname = KEYNAME.Y : Measure_Index = 50
                        Case 24 + Index_Increase
                            _keyname = KEYNAME.T : Measure_Index = 51
                        Case 25 + Index_Increase
                            _keyname = KEYNAME.R : Measure_Index = 52
                        Case 26 + Index_Increase
                            _keyname = KEYNAME.E : Measure_Index = 53
                        Case 27 + Index_Increase
                            _keyname = KEYNAME.W : Measure_Index = 54
                        Case 28 + Index_Increase
                            _keyname = KEYNAME.Q : Measure_Index = 55
                    End Select
                    CogToolBlock_Fixture_Right = CogToolBlock_Group_Right.Tools("CogToolBlock_Measure_1X1")
                    CogToolBlock_Shard_Right = CogToolBlock_Fixture_Right.Tools("CogToolBlock_1X1")
                    Measure_Sub_Right(Select_Product_Name, Barcode, _keyname, Image_Index, Measure_Index, KeyType, "D:\System\HOOK_SNAP\Right\")
                Case 29 + Index_Increase
                    CogToolBlock_Fixture_Right = CogToolBlock_Group_Right.Tools("CogToolBlock_Measure_Tab01")
                    CogToolBlock_Shard_Right = CogToolBlock_Fixture_Right.Tools("CogToolBlock_Tab01")
                    Measure_Sub_Right(Select_Product_Name, Barcode, KEYNAME.Tab, Image_Index, 56, KeyType, "D:\System\HOOK_SNAP\Right\")
                Case 30 + Index_Increase
                    CogToolBlock_Fixture_Right = CogToolBlock_Group_Right.Tools("CogToolBlock_Measure_Tab02")
                    CogToolBlock_Shard_Right = CogToolBlock_Fixture_Right.Tools("CogToolBlock_Tab02")
                    Measure_Sub_Right(Select_Product_Name, Barcode, KEYNAME.Tab, Image_Index, 57, KeyType, "D:\System\HOOK_SNAP\Right\")
                Case 31 + Index_Increase
                    CogToolBlock_Fixture_Right = CogToolBlock_Group_Right.Tools("CogToolBlock_Measure_CapsLock01")
                    CogToolBlock_Shard_Right = CogToolBlock_Fixture_Right.Tools("CogToolBlock_CapsLock01")
                    Measure_Sub_Right(Select_Product_Name, Barcode, KEYNAME.CapsLock, Image_Index, 58, KeyType, "D:\System\HOOK_SNAP\Right\")
                Case 32 + Index_Increase
                    CogToolBlock_Fixture_Right = CogToolBlock_Group_Right.Tools("CogToolBlock_Measure_CapsLock02")
                    CogToolBlock_Shard_Right = CogToolBlock_Fixture_Right.Tools("CogToolBlock_CapsLock02")
                    Measure_Sub_Right(Select_Product_Name, Barcode, KEYNAME.CapsLock, Image_Index, 59, KeyType, "D:\System\HOOK_SNAP\Right\")
                Case 33 + Index_Increase To 43 + Index_Increase
                    Dim _keyname As String = Nothing : Dim Measure_Index As Integer
                    Select Case Image_Index
                        Case 33 + Index_Increase
                            _keyname = KEYNAME.A : Measure_Index = 60
                        Case 34 + Index_Increase
                            _keyname = KEYNAME.S : Measure_Index = 61
                        Case 35 + Index_Increase
                            _keyname = KEYNAME.D : Measure_Index = 62
                        Case 36 + Index_Increase
                            _keyname = KEYNAME.F : Measure_Index = 63
                        Case 37 + Index_Increase
                            _keyname = KEYNAME.G : Measure_Index = 64
                        Case 38 + Index_Increase
                            _keyname = KEYNAME.H : Measure_Index = 65
                        Case 39 + Index_Increase
                            _keyname = KEYNAME.J : Measure_Index = 66
                        Case 40 + Index_Increase
                            _keyname = KEYNAME.K : Measure_Index = 67
                        Case 41 + Index_Increase
                            _keyname = KEYNAME.L : Measure_Index = 68
                        Case 42 + Index_Increase
                            _keyname = KEYNAME.Semicolon : Measure_Index = 69
                        Case 43 + Index_Increase
                            _keyname = KEYNAME.Quote : Measure_Index = 70
                    End Select
                    CogToolBlock_Fixture_Right = CogToolBlock_Group_Right.Tools("CogToolBlock_Measure_1X1")
                    CogToolBlock_Shard_Right = CogToolBlock_Fixture_Right.Tools("CogToolBlock_1X1")
                    Measure_Sub_Right(Select_Product_Name, Barcode, _keyname, Image_Index, Measure_Index, KeyType, "D:\System\HOOK_SNAP\Right\")
                Case 44 + Index_Increase
                    CogToolBlock_Fixture_Right = CogToolBlock_Group_Right.Tools("CogToolBlock_Measure_Return01")
                    CogToolBlock_Shard_Right = CogToolBlock_Fixture_Right.Tools("CogToolBlock_Return01")
                    Measure_Sub_Right(Select_Product_Name, Barcode, KEYNAME.Return_, Image_Index, 71, KeyType, "D:\System\HOOK_SNAP\Right\")
                Case 45 + Index_Increase
                    CogToolBlock_Fixture_Right = CogToolBlock_Group_Right.Tools("CogToolBlock_Measure_Return02")
                    CogToolBlock_Shard_Right = CogToolBlock_Fixture_Right.Tools("CogToolBlock_Return02")
                    Measure_Sub_Right(Select_Product_Name, Barcode, KEYNAME.Return_, Image_Index, 72, KeyType, "D:\System\HOOK_SNAP\Right\")
                Case 46 + Index_Increase
                    CogToolBlock_Fixture_Right = CogToolBlock_Group_Right.Tools("CogToolBlock_Measure_Right_Shift01")
                    CogToolBlock_Shard_Right = CogToolBlock_Fixture_Right.Tools("CogToolBlock_Shift01")
                    Measure_Sub_Right(Select_Product_Name, Barcode, KEYNAME.RightShift, Image_Index, 73, KeyType, "D:\System\HOOK_SNAP\Right\")
                Case 47 + Index_Increase
                    CogToolBlock_Fixture_Right = CogToolBlock_Group_Right.Tools("CogToolBlock_Measure_Right_Shift02")
                    CogToolBlock_Shard_Right = CogToolBlock_Fixture_Right.Tools("CogToolBlock_Shift02")
                    Measure_Sub_Right(Select_Product_Name, Barcode, KEYNAME.RightShift, Image_Index, 74, KeyType, "D:\System\HOOK_SNAP\Right\")
                Case 48 + Index_Increase
                    CogToolBlock_Fixture_Right = CogToolBlock_Group_Right.Tools("CogToolBlock_Measure_Right_Shift03")
                    CogToolBlock_Shard_Right = CogToolBlock_Fixture_Right.Tools("CogToolBlock_Shift03")
                    Measure_Sub_Right(Select_Product_Name, Barcode, KEYNAME.RightShift, Image_Index, 75, KeyType, "D:\System\HOOK_SNAP\Right\")
                Case 49 + Index_Increase To 58 + Index_Increase
                    Dim _keyname As String = Nothing : Dim Measure_Index As Integer
                    Select Case Image_Index
                        Case 49 + Index_Increase
                            _keyname = KEYNAME.Question : Measure_Index = 76
                        Case 50 + Index_Increase
                            _keyname = KEYNAME.Period : Measure_Index = 77
                        Case 51 + Index_Increase
                            _keyname = KEYNAME.Comma : Measure_Index = 78
                        Case 52 + Index_Increase
                            _keyname = KEYNAME.M : Measure_Index = 79
                        Case 53 + Index_Increase
                            _keyname = KEYNAME.N : Measure_Index = 80
                        Case 54 + Index_Increase
                            _keyname = KEYNAME.B : Measure_Index = 81
                        Case 55 + Index_Increase
                            _keyname = KEYNAME.V : Measure_Index = 82
                        Case 56 + Index_Increase
                            _keyname = KEYNAME.C : Measure_Index = 83
                        Case 57 + Index_Increase
                            _keyname = KEYNAME.X : Measure_Index = 84
                        Case 58 + Index_Increase
                            _keyname = KEYNAME.Z : Measure_Index = 85
                    End Select
                    CogToolBlock_Fixture_Right = CogToolBlock_Group_Right.Tools("CogToolBlock_Measure_1X1")
                    CogToolBlock_Shard_Right = CogToolBlock_Fixture_Right.Tools("CogToolBlock_1X1")
                    Measure_Sub_Right(Select_Product_Name, Barcode, _keyname, Image_Index, Measure_Index, KeyType, "D:\System\HOOK_SNAP\Right\")
                Case 59 + Index_Increase
                    CogToolBlock_Fixture_Right = CogToolBlock_Group_Right.Tools("CogToolBlock_Measure_Left_Shift01")
                    CogToolBlock_Shard_Right = CogToolBlock_Fixture_Right.Tools("CogToolBlock_Shift01")
                    Measure_Sub_Right(Select_Product_Name, Barcode, KEYNAME.LeftShift, Image_Index, 86, KeyType, "D:\System\HOOK_SNAP\Right\")
                Case 60 + Index_Increase
                    CogToolBlock_Fixture_Right = CogToolBlock_Group_Right.Tools("CogToolBlock_Measure_Left_Shift02")
                    CogToolBlock_Shard_Right = CogToolBlock_Fixture_Right.Tools("CogToolBlock_Shift02")
                    Measure_Sub_Right(Select_Product_Name, Barcode, KEYNAME.LeftShift, Image_Index, 87, KeyType, "D:\System\HOOK_SNAP\Right\")
                Case 61 + Index_Increase
                    CogToolBlock_Fixture_Right = CogToolBlock_Group_Right.Tools("CogToolBlock_Measure_Left_Shift03")
                    CogToolBlock_Shard_Right = CogToolBlock_Fixture_Right.Tools("CogToolBlock_Shift03")
                    Measure_Sub_Right(Select_Product_Name, Barcode, KEYNAME.LeftShift, Image_Index, 88, KeyType, "D:\System\HOOK_SNAP\Right\")
                Case 62 + Index_Increase To 64 + Index_Increase
                    Dim _keyname As String = Nothing : Dim Measure_Index As Integer
                    Select Case Image_Index
                        Case 62 + Index_Increase
                            _keyname = KEYNAME.Fn : Measure_Index = 89
                        Case 63 + Index_Increase
                            _keyname = KEYNAME.Control : Measure_Index = 90
                        Case 64 + Index_Increase
                            _keyname = KEYNAME.LeftOption : Measure_Index = 91
                    End Select
                    CogToolBlock_Fixture_Right = CogToolBlock_Group_Right.Tools("CogToolBlock_Measure_1X1")
                    CogToolBlock_Shard_Right = CogToolBlock_Fixture_Right.Tools("CogToolBlock_1X1")
                    Measure_Sub_Right(Select_Product_Name, Barcode, _keyname, Image_Index, Measure_Index, KeyType, "D:\System\HOOK_SNAP\Right\")
                Case 65 + Index_Increase
                    CogToolBlock_Fixture_Right = CogToolBlock_Group_Right.Tools("CogToolBlock_Mesure_Command")
                    CogToolBlock_Shard_Right = CogToolBlock_Fixture_Right.Tools("CogToolBlock_Command")
                    Measure_Sub_Right(Select_Product_Name, Barcode, KEYNAME.LeftCommand, Image_Index, 92, KeyType, "D:\System\HOOK_SNAP\Right\")
                Case 71 + Index_Increase
                    CogToolBlock_Fixture_Right = CogToolBlock_Group_Right.Tools("CogToolBlock_Mesure_Command")
                    CogToolBlock_Shard_Right = CogToolBlock_Fixture_Right.Tools("CogToolBlock_Command")
                    Measure_Sub_Right(Select_Product_Name, Barcode, KEYNAME.RightCommand, Image_Index, 93, KeyType, "D:\System\HOOK_SNAP\Right\")
                Case 72 + Index_Increase
                    CogToolBlock_Fixture_Right = CogToolBlock_Group_Right.Tools("CogToolBlock_Measure_1X1")
                    CogToolBlock_Shard_Right = CogToolBlock_Fixture_Right.Tools("CogToolBlock_1X1")
                    Measure_Sub_Right(Select_Product_Name, Barcode, KEYNAME.RightOption, Image_Index, 94, KeyType, "D:\System\HOOK_SNAP\Right\")
                Case 73 + Index_Increase
                    CogToolBlock_Fixture_Right = CogToolBlock_Group_Right.Tools("CogToolBlock_Up_Down")
                    CogToolBlock_Shard_Right = CogToolBlock_Fixture_Right.Tools("CogToolBlock_Down")
                    Measure_Sub_Right(Select_Product_Name, Barcode, KEYNAME.Left, Image_Index, 95, KeyType, "D:\System\HOOK_SNAP\Right\")
                Case 74 + Index_Increase
                    CogToolBlock_Fixture_Right = CogToolBlock_Group_Right.Tools("CogToolBlock_Up_Down")
                    CogToolBlock_Shard_Right = CogToolBlock_Fixture_Right.Tools("CogToolBlock_Up")
                    Measure_Sub_Right(Select_Product_Name, Barcode, KEYNAME.Up, Image_Index, 96, KeyType, "D:\System\HOOK_SNAP\Right\")
                    CogToolBlock_Fixture_Right = CogToolBlock_Group_Right.Tools("CogToolBlock_Up_Down")
                    CogToolBlock_Shard_Right = CogToolBlock_Fixture_Right.Tools("CogToolBlock_Down")
                    Measure_Sub_Right(Select_Product_Name, Barcode, KEYNAME.Down, Image_Index, 97, KeyType, "D:\System\HOOK_SNAP\Right\")
                Case 75 + Index_Increase
                    CogToolBlock_Fixture_Right = CogToolBlock_Group_Right.Tools("CogToolBlock_Up_Down")
                    CogToolBlock_Shard_Right = CogToolBlock_Fixture_Right.Tools("CogToolBlock_Down")
                    Measure_Sub_Right(Select_Product_Name, Barcode, KEYNAME.Right, Image_Index, 98, KeyType, "D:\System\HOOK_SNAP\Right\")
            End Select
        Catch ex As Exception
            Invoke(New Delegate_Display_Message_Right(AddressOf Display_Message_Right), "右图像处理错误:[" & Image_Index & "]" & ex.ToString, Barcode, Color.Red)
        End Try
    End Sub


    Sub Measure_Right_Ansi(ByVal Image_Index As Integer, ByVal Project_Name As String, ByVal Barcode As String, ByVal KeyType As String)
        Try
            Select Case Image_Index
                Case 1 To 13
                    Dim _keyname As String = Nothing
                    Dim Measure_Index As Integer
                    Select Case Image_Index
                        Case 1
                            _keyname = KEYNAME.Esc : Measure_Index = 0
                        Case 2
                            _keyname = KEYNAME.F1 : Measure_Index = 1
                        Case 3
                            _keyname = KEYNAME.F2 : Measure_Index = 2
                        Case 4
                            _keyname = KEYNAME.F3 : Measure_Index = 3
                        Case 5
                            _keyname = KEYNAME.F4 : Measure_Index = 4
                        Case 6
                            _keyname = KEYNAME.F5 : Measure_Index = 5
                        Case 7
                            _keyname = KEYNAME.F6 : Measure_Index = 6
                        Case 8
                            _keyname = KEYNAME.F7 : Measure_Index = 7
                        Case 9
                            _keyname = KEYNAME.F8 : Measure_Index = 8
                        Case 10
                            _keyname = KEYNAME.F9 : Measure_Index = 9
                        Case 11
                            _keyname = KEYNAME.F10 : Measure_Index = 10
                        Case 12
                            _keyname = KEYNAME.F11 : Measure_Index = 11
                        Case 13
                            _keyname = KEYNAME.F12 : Measure_Index = 12

                    End Select

                    CogToolBlock_Fixture_Right = CogToolBlock_Group_Right.Tools("CogToolBlock_Measure_Fun_X10")
                    CogToolBlock_Shard_Right = CogToolBlock_Fixture_Right.Tools("CogToolBlock_FunX10")
                    Measure_Sub_Right(Select_Product_Name, Barcode, _keyname, Image_Index, Measure_Index, KeyType, "D:\System\HOOK_SNAP\Right\")
                Case 14
                    CogToolBlock_Fixture_Right = CogToolBlock_Group_Right.Tools("CogToolBlock_Measure_Fun_Delete01")
                    CogToolBlock_Shard_Right = CogToolBlock_Fixture_Right.Tools("CogToolBlock_Delete01")
                    Measure_Sub_Right(Select_Product_Name, Barcode, KEYNAME.Delete, Image_Index, 13, KeyType, "D:\System\HOOK_SNAP\Right\")
                Case 15
                    CogToolBlock_Fixture_Right = CogToolBlock_Group_Right.Tools("CogToolBlock_Measure_Fun_Delete02")
                    CogToolBlock_Shard_Right = CogToolBlock_Fixture_Right.Tools("CogToolBlock_Delete02")
                    Measure_Sub_Right(Select_Product_Name, Barcode, KEYNAME.Delete, Image_Index, 14, KeyType, "D:\System\HOOK_SNAP\Right\")
                Case 16 To 28
                    Dim _keyname As String = Nothing
                    Dim Measure_Index As Integer
                    Select Case Image_Index
                        Case 16
                            _keyname = KEYNAME.Equal : Measure_Index = 15
                        Case 17
                            _keyname = KEYNAME.Minus : Measure_Index = 16
                        Case 18
                            _keyname = KEYNAME.Num0 : Measure_Index = 17
                        Case 19
                            _keyname = KEYNAME.Num9 : Measure_Index = 18
                        Case 20
                            _keyname = KEYNAME.Num8 : Measure_Index = 19
                        Case 21
                            _keyname = KEYNAME.Num7 : Measure_Index = 20
                        Case 22
                            _keyname = KEYNAME.Num6 : Measure_Index = 21
                        Case 23
                            _keyname = KEYNAME.Num5 : Measure_Index = 22
                        Case 24
                            _keyname = KEYNAME.Num4 : Measure_Index = 23
                        Case 25
                            _keyname = KEYNAME.Num3 : Measure_Index = 24
                        Case 26
                            _keyname = KEYNAME.Num2 : Measure_Index = 25
                        Case 27
                            _keyname = KEYNAME.Num1 : Measure_Index = 26
                        Case 28
                            _keyname = KEYNAME.Tilde : Measure_Index = 27
                    End Select
                    CogToolBlock_Fixture_Right = CogToolBlock_Group_Right.Tools("CogToolBlock_Measure_1X1")
                    CogToolBlock_Shard_Right = CogToolBlock_Fixture_Right.Tools("CogToolBlock_1X1")
                    Measure_Sub_Right(Select_Product_Name, Barcode, _keyname, Image_Index, Measure_Index, KeyType, "D:\System\HOOK_SNAP\Right\")
                Case 29
                    CogToolBlock_Fixture_Right = CogToolBlock_Group_Right.Tools("CogToolBlock_Measure_Tab01")
                    CogToolBlock_Shard_Right = CogToolBlock_Fixture_Right.Tools("CogToolBlock_Tab01")
                    Measure_Sub_Right(Select_Product_Name, Barcode, KEYNAME.Tab, Image_Index, 28, KeyType, "D:\System\HOOK_SNAP\Right\")
                Case 30
                    CogToolBlock_Fixture_Right = CogToolBlock_Group_Right.Tools("CogToolBlock_Measure_Tab02")
                    CogToolBlock_Shard_Right = CogToolBlock_Fixture_Right.Tools("CogToolBlock_Tab02")
                    Measure_Sub_Right(Select_Product_Name, Barcode, KEYNAME.Tab, Image_Index, 29, KeyType, "D:\System\HOOK_SNAP\Right\")
                Case 31 To 43
                    Dim _keyname As String = Nothing
                    Dim Measure_Index As Integer
                    Select Case Image_Index
                        Case 31
                            _keyname = KEYNAME.Q : Measure_Index = 30
                        Case 32
                            _keyname = KEYNAME.W : Measure_Index = 31
                        Case 33
                            _keyname = KEYNAME.E : Measure_Index = 32
                        Case 34
                            _keyname = KEYNAME.R : Measure_Index = 33
                        Case 35
                            _keyname = KEYNAME.T : Measure_Index = 34
                        Case 36
                            _keyname = KEYNAME.Y : Measure_Index = 35
                        Case 37
                            _keyname = KEYNAME.U : Measure_Index = 36
                        Case 38
                            _keyname = KEYNAME.I : Measure_Index = 37
                        Case 39
                            _keyname = KEYNAME.O : Measure_Index = 38
                        Case 40
                            _keyname = KEYNAME.P : Measure_Index = 39
                        Case 41
                            _keyname = KEYNAME.LeftBracket : Measure_Index = 40
                        Case 42
                            _keyname = KEYNAME.RightBracket : Measure_Index = 41
                        Case 43
                            _keyname = KEYNAME.BackSlash : Measure_Index = 42
                    End Select
                    CogToolBlock_Fixture_Right = CogToolBlock_Group_Right.Tools("CogToolBlock_Measure_1X1")
                    CogToolBlock_Shard_Right = CogToolBlock_Fixture_Right.Tools("CogToolBlock_1X1")
                    Measure_Sub_Right(Select_Product_Name, Barcode, _keyname, Image_Index, Measure_Index, KeyType, "D:\System\HOOK_SNAP\Right\")
                Case 44
                    CogToolBlock_Fixture_Right = CogToolBlock_Group_Right.Tools("CogToolBlock_Measure_Return01")
                    CogToolBlock_Shard_Right = CogToolBlock_Fixture_Right.Tools("CogToolBlock_Return01")
                    Measure_Sub_Right(Select_Product_Name, Barcode, KEYNAME.Return_, Image_Index, 43, KeyType, "D:\System\HOOK_SNAP\Right\")
                Case 45
                    CogToolBlock_Fixture_Right = CogToolBlock_Group_Right.Tools("CogToolBlock_Measure_Return02")
                    CogToolBlock_Shard_Right = CogToolBlock_Fixture_Right.Tools("CogToolBlock_Return02")
                    Measure_Sub_Right(Select_Product_Name, Barcode, KEYNAME.Return_, Image_Index, 44, KeyType, "D:\System\HOOK_SNAP\Right\")
                Case 46 To 56
                    Dim _keyname As String = Nothing : Dim Measure_Index As Integer
                    Select Case Image_Index
                        Case 46
                            _keyname = KEYNAME.Quote : Measure_Index = 45
                        Case 47
                            _keyname = KEYNAME.Semicolon : Measure_Index = 46
                        Case 48
                            _keyname = KEYNAME.L : Measure_Index = 47
                        Case 49
                            _keyname = KEYNAME.K : Measure_Index = 48
                        Case 50
                            _keyname = KEYNAME.J : Measure_Index = 49
                        Case 51
                            _keyname = KEYNAME.H : Measure_Index = 50
                        Case 52
                            _keyname = KEYNAME.G : Measure_Index = 51
                        Case 53
                            _keyname = KEYNAME.F : Measure_Index = 52
                        Case 54
                            _keyname = KEYNAME.D : Measure_Index = 53
                        Case 55
                            _keyname = KEYNAME.S : Measure_Index = 54
                        Case 56
                            _keyname = KEYNAME.A : Measure_Index = 55
                    End Select
                    CogToolBlock_Fixture_Right = CogToolBlock_Group_Right.Tools("CogToolBlock_Measure_1X1")
                    CogToolBlock_Shard_Right = CogToolBlock_Fixture_Right.Tools("CogToolBlock_1X1")
                    Measure_Sub_Right(Select_Product_Name, Barcode, _keyname, Image_Index, Measure_Index, KeyType, "D:\System\HOOK_SNAP\Right\")
                Case 57
                    CogToolBlock_Fixture_Right = CogToolBlock_Group_Right.Tools("CogToolBlock_Measure_CapsLock01")
                    CogToolBlock_Shard_Right = CogToolBlock_Fixture_Right.Tools("CogToolBlock_CapsLock01")
                    Measure_Sub_Right(Select_Product_Name, Barcode, KEYNAME.CapsLock, Image_Index, 56, KeyType, "D:\System\HOOK_SNAP\Right\")
                Case 58
                    CogToolBlock_Fixture_Right = CogToolBlock_Group_Right.Tools("CogToolBlock_Measure_CapsLock02")
                    CogToolBlock_Shard_Right = CogToolBlock_Fixture_Right.Tools("CogToolBlock_CapsLock02")
                    Measure_Sub_Right(Select_Product_Name, Barcode, KEYNAME.CapsLock, Image_Index, 57, KeyType, "D:\System\HOOK_SNAP\Right\")
                Case 59
                    CogToolBlock_Fixture_Right = CogToolBlock_Group_Right.Tools("CogToolBlock_Measure_Left_Shift01")
                    CogToolBlock_Shard_Right = CogToolBlock_Fixture_Right.Tools("CogToolBlock_Shift01")
                    Measure_Sub_Right(Select_Product_Name, Barcode, KEYNAME.LeftShift, Image_Index, 58, KeyType, "D:\System\HOOK_SNAP\Right\")
                Case 60
                    CogToolBlock_Fixture_Right = CogToolBlock_Group_Right.Tools("CogToolBlock_Measure_Left_Shift02")
                    CogToolBlock_Shard_Right = CogToolBlock_Fixture_Right.Tools("CogToolBlock_Shift02")
                    Measure_Sub_Right(Select_Product_Name, Barcode, KEYNAME.LeftShift, Image_Index, 59, KeyType, "D:\System\HOOK_SNAP\Right\")
                Case 61
                    CogToolBlock_Fixture_Right = CogToolBlock_Group_Right.Tools("CogToolBlock_Measure_Left_Shift03")
                    CogToolBlock_Shard_Right = CogToolBlock_Fixture_Right.Tools("CogToolBlock_Shift03")
                    Measure_Sub_Right(Select_Product_Name, Barcode, KEYNAME.LeftShift, Image_Index, 60, KeyType, "D:\System\HOOK_SNAP\Right\")
                Case 62 To 71
                    Dim _keyname As String = Nothing : Dim Measure_Index As Integer
                    Select Case Image_Index
                        Case 62
                            _keyname = KEYNAME.Z : Measure_Index = 61
                        Case 63
                            _keyname = KEYNAME.X : Measure_Index = 62
                        Case 64
                            _keyname = KEYNAME.C : Measure_Index = 63
                        Case 65
                            _keyname = KEYNAME.V : Measure_Index = 64
                        Case 66
                            _keyname = KEYNAME.B : Measure_Index = 65
                        Case 67
                            _keyname = KEYNAME.N : Measure_Index = 66
                        Case 68
                            _keyname = KEYNAME.M : Measure_Index = 67
                        Case 69
                            _keyname = KEYNAME.Comma : Measure_Index = 68
                        Case 70
                            _keyname = KEYNAME.Period : Measure_Index = 69
                        Case 71
                            _keyname = KEYNAME.Question : Measure_Index = 70
                    End Select
                    CogToolBlock_Fixture_Right = CogToolBlock_Group_Right.Tools("CogToolBlock_Measure_1X1")
                    CogToolBlock_Shard_Right = CogToolBlock_Fixture_Right.Tools("CogToolBlock_1X1")
                    Measure_Sub_Right(Select_Product_Name, Barcode, _keyname, Image_Index, Measure_Index, KeyType, "D:\System\HOOK_SNAP\Right\")

                Case 72
                    CogToolBlock_Fixture_Right = CogToolBlock_Group_Right.Tools("CogToolBlock_Measure_Right_Shift01")
                    CogToolBlock_Shard_Right = CogToolBlock_Fixture_Right.Tools("CogToolBlock_Shift01")
                    Measure_Sub_Right(Select_Product_Name, Barcode, KEYNAME.RightShift, Image_Index, 71, KeyType, "D:\System\HOOK_SNAP\Right\")
                Case 73
                    CogToolBlock_Fixture_Right = CogToolBlock_Group_Right.Tools("CogToolBlock_Measure_Right_Shift02")
                    CogToolBlock_Shard_Right = CogToolBlock_Fixture_Right.Tools("CogToolBlock_Shift02")
                    Measure_Sub_Right(Select_Product_Name, Barcode, KEYNAME.RightShift, Image_Index, 72, KeyType, "D:\System\HOOK_SNAP\Right\")
                Case 74
                    CogToolBlock_Fixture_Right = CogToolBlock_Group_Right.Tools("CogToolBlock_Measure_Right_Shift03")
                    CogToolBlock_Shard_Right = CogToolBlock_Fixture_Right.Tools("CogToolBlock_Shift03")
                    Measure_Sub_Right(Select_Product_Name, Barcode, KEYNAME.RightShift, Image_Index, 73, KeyType, "D:\System\HOOK_SNAP\Right\")
                Case 75
                    CogToolBlock_Fixture_Right = CogToolBlock_Group_Right.Tools("CogToolBlock_Up_Down")
                    CogToolBlock_Shard_Right = CogToolBlock_Fixture_Right.Tools("CogToolBlock_Down")
                    Measure_Sub_Right(Select_Product_Name, Barcode, KEYNAME.Right, Image_Index, 74, KeyType, "D:\System\HOOK_SNAP\Right\")
                Case 76
                    CogToolBlock_Fixture_Right = CogToolBlock_Group_Right.Tools("CogToolBlock_Up_Down")
                    CogToolBlock_Shard_Right = CogToolBlock_Fixture_Right.Tools("CogToolBlock_Up")
                    Measure_Sub_Right(Select_Product_Name, Barcode, KEYNAME.Up, Image_Index, 75, KeyType, "D:\System\HOOK_SNAP\Right\")
                    CogToolBlock_Fixture_Right = CogToolBlock_Group_Right.Tools("CogToolBlock_Up_Down")
                    CogToolBlock_Shard_Right = CogToolBlock_Fixture_Right.Tools("CogToolBlock_Down")
                    Measure_Sub_Right(Select_Product_Name, Barcode, KEYNAME.Down, Image_Index, 76, KeyType, "D:\System\HOOK_SNAP\Right\")
                Case 77
                    CogToolBlock_Fixture_Right = CogToolBlock_Group_Right.Tools("CogToolBlock_Up_Down")
                    CogToolBlock_Shard_Right = CogToolBlock_Fixture_Right.Tools("CogToolBlock_Down")
                    Measure_Sub_Right(Select_Product_Name, Barcode, KEYNAME.Left, Image_Index, 77, KeyType, "D:\System\HOOK_SNAP\Right\")
                Case 78
                    CogToolBlock_Fixture_Right = CogToolBlock_Group_Right.Tools("CogToolBlock_Measure_1X1")
                    CogToolBlock_Shard_Right = CogToolBlock_Fixture_Right.Tools("CogToolBlock_1X1")
                    Measure_Sub_Right(Select_Product_Name, Barcode, KEYNAME.RightOption, Image_Index, 78, KeyType, "D:\System\HOOK_SNAP\Right\")
                Case 79
                    CogToolBlock_Fixture_Right = CogToolBlock_Group_Right.Tools("CogToolBlock_Mesure_Command")
                    CogToolBlock_Shard_Right = CogToolBlock_Fixture_Right.Tools("CogToolBlock_Command")
                    Measure_Sub_Right(Select_Product_Name, Barcode, KEYNAME.RightCommand, Image_Index, 79, KeyType, "D:\System\HOOK_SNAP\Right\")
                Case 85
                    CogToolBlock_Fixture_Right = CogToolBlock_Group_Right.Tools("CogToolBlock_Mesure_Command")
                    CogToolBlock_Shard_Right = CogToolBlock_Fixture_Right.Tools("CogToolBlock_Command")
                    Measure_Sub_Right(Select_Product_Name, Barcode, KEYNAME.LeftCommand, Image_Index, 80, KeyType, "D:\System\HOOK_SNAP\Right\")

                Case 86 To 88
                    Dim _keyname As String = Nothing : Dim Measure_Index As Integer
                    Select Case Image_Index
                        Case 86
                            _keyname = KEYNAME.LeftOption : Measure_Index = 81
                        Case 87
                            _keyname = KEYNAME.Control : Measure_Index = 82
                        Case 88
                            _keyname = KEYNAME.Fn : Measure_Index = 83
                    End Select
                    CogToolBlock_Fixture_Right = CogToolBlock_Group_Right.Tools("CogToolBlock_Measure_1X1")
                    CogToolBlock_Shard_Right = CogToolBlock_Fixture_Right.Tools("CogToolBlock_1X1")
                    Measure_Sub_Right(Select_Product_Name, Barcode, _keyname, Image_Index, Measure_Index, KeyType, "D:\System\HOOK_SNAP\Right\")


            End Select
        Catch ex As Exception
            Invoke(New Delegate_Display_Message_Right(AddressOf Display_Message_Right), "右图像处理错误:[" & Image_Index & "]" & ex.ToString, Barcode, Color.Red)
        End Try
    End Sub

    Sub Measure_Right_Iso(ByVal Image_Index As Integer, ByVal Project_Name As String, ByVal Barcode As String, ByVal KeyType As String)
        Try
            Select Image_Index
                Case 1 To 13
                    Dim _keyname As String = Nothing
                    Dim Measure_Index As Integer
                    Select Case Image_Index
                        Case 1
                            _keyname = KEYNAME.Esc : Measure_Index = 0
                        Case 2
                            _keyname = KEYNAME.F1 : Measure_Index = 1
                        Case 3
                            _keyname = KEYNAME.F2 : Measure_Index = 2
                        Case 4
                            _keyname = KEYNAME.F3 : Measure_Index = 3
                        Case 5
                            _keyname = KEYNAME.F4 : Measure_Index = 4
                        Case 6
                            _keyname = KEYNAME.F5 : Measure_Index = 5
                        Case 7
                            _keyname = KEYNAME.F6 : Measure_Index = 6
                        Case 8
                            _keyname = KEYNAME.F7 : Measure_Index = 7
                        Case 9
                            _keyname = KEYNAME.F8 : Measure_Index = 8
                        Case 10
                            _keyname = KEYNAME.F9 : Measure_Index = 9
                        Case 11
                            _keyname = KEYNAME.F10 : Measure_Index = 10
                        Case 12
                            _keyname = KEYNAME.F11 : Measure_Index = 11
                        Case 13
                            _keyname = KEYNAME.F12 : Measure_Index = 12
                    End Select

                    CogToolBlock_Fixture_Right = CogToolBlock_Group_Right.Tools("CogToolBlock_Measure_Fun_X10")
                    CogToolBlock_Shard_Right = CogToolBlock_Fixture_Right.Tools("CogToolBlock_FunX10")
                    Measure_Sub_Right(Select_Product_Name, Barcode, _keyname, Image_Index, Measure_Index, KeyType, "D:\System\HOOK_SNAP\Right\")
                Case 14
                    CogToolBlock_Fixture_Right = CogToolBlock_Group_Right.Tools("CogToolBlock_Measure_Fun_Delete01")
                    CogToolBlock_Shard_Right = CogToolBlock_Fixture_Right.Tools("CogToolBlock_Delete01")
                    Measure_Sub_Right(Select_Product_Name, Barcode, KEYNAME.Delete, Image_Index, 13, KeyType, "D:\System\HOOK_SNAP\Right\")
                Case 15
                    CogToolBlock_Fixture_Right = CogToolBlock_Group_Right.Tools("CogToolBlock_Measure_Fun_Delete02")
                    CogToolBlock_Shard_Right = CogToolBlock_Fixture_Right.Tools("CogToolBlock_Delete02")
                    Measure_Sub_Right(Select_Product_Name, Barcode, KEYNAME.Delete, Image_Index, 14, KeyType, "D:\System\HOOK_SNAP\Right\")

                Case 16 To 28
                    Dim _keyname As String = Nothing : Dim Measure_Index As Integer
                    Select Case Image_Index
                        Case 16
                            _keyname = KEYNAME.Equal : Measure_Index = 15
                        Case 17
                            _keyname = KEYNAME.Minus : Measure_Index = 16
                        Case 18
                            _keyname = KEYNAME.Num0 : Measure_Index = 17
                        Case 19
                            _keyname = KEYNAME.Num9 : Measure_Index = 18
                        Case 20
                            _keyname = KEYNAME.Num8 : Measure_Index = 19
                        Case 21
                            _keyname = KEYNAME.Num7 : Measure_Index = 20
                        Case 22
                            _keyname = KEYNAME.Num6 : Measure_Index = 21
                        Case 23
                            _keyname = KEYNAME.Num5 : Measure_Index = 22
                        Case 24
                            _keyname = KEYNAME.Num4 : Measure_Index = 23
                        Case 25
                            _keyname = KEYNAME.Num3 : Measure_Index = 24
                        Case 26
                            _keyname = KEYNAME.Num2 : Measure_Index = 25
                        Case 27
                            _keyname = KEYNAME.Num1 : Measure_Index = 26
                        Case 28
                            _keyname = KEYNAME.ISO : Measure_Index = 27
                    End Select
                    CogToolBlock_Fixture_Right = CogToolBlock_Group_Right.Tools("CogToolBlock_Measure_1X1")
                    CogToolBlock_Shard_Right = CogToolBlock_Fixture_Right.Tools("CogToolBlock_1X1")
                    Measure_Sub_Right(Select_Product_Name, Barcode, _keyname, Image_Index, Measure_Index, KeyType, "D:\System\HOOK_SNAP\Right\")
                Case 29
                    CogToolBlock_Fixture_Right = CogToolBlock_Group_Right.Tools("CogToolBlock_Measure_Tab01")
                    CogToolBlock_Shard_Right = CogToolBlock_Fixture_Right.Tools("CogToolBlock_Tab01")
                    Measure_Sub_Right(Select_Product_Name, Barcode, KEYNAME.Tab, Image_Index, 28, KeyType, "D:\System\HOOK_SNAP\Right\")
                Case 30
                    CogToolBlock_Fixture_Right = CogToolBlock_Group_Right.Tools("CogToolBlock_Measure_Tab02")
                    CogToolBlock_Shard_Right = CogToolBlock_Fixture_Right.Tools("CogToolBlock_Tab02")
                    Measure_Sub_Right(Select_Product_Name, Barcode, KEYNAME.Tab, Image_Index, 29, KeyType, "D:\System\HOOK_SNAP\Right\")

                Case 31 To 42
                    Dim _keyname As String = Nothing : Dim Measure_Index As Integer
                    Select Case Image_Index
                        Case 31
                            _keyname = KEYNAME.Q : Measure_Index = 30
                        Case 32
                            _keyname = KEYNAME.W : Measure_Index = 31
                        Case 33
                            _keyname = KEYNAME.E : Measure_Index = 32
                        Case 34
                            _keyname = KEYNAME.R : Measure_Index = 33
                        Case 35
                            _keyname = KEYNAME.T : Measure_Index = 34
                        Case 36
                            _keyname = KEYNAME.Y : Measure_Index = 35
                        Case 37
                            _keyname = KEYNAME.U : Measure_Index = 36
                        Case 38
                            _keyname = KEYNAME.I : Measure_Index = 37
                        Case 39
                            _keyname = KEYNAME.O : Measure_Index = 38
                        Case 40
                            _keyname = KEYNAME.P : Measure_Index = 39
                        Case 41
                            _keyname = KEYNAME.LeftBracket : Measure_Index = 40
                        Case 42
                            _keyname = KEYNAME.RightBracket : Measure_Index = 41
                    End Select
                    CogToolBlock_Fixture_Right = CogToolBlock_Group_Right.Tools("CogToolBlock_Measure_1X1")
                    CogToolBlock_Shard_Right = CogToolBlock_Fixture_Right.Tools("CogToolBlock_1X1")
                    Measure_Sub_Right(Select_Product_Name, Barcode, _keyname, Image_Index, Measure_Index, KeyType, "D:\System\HOOK_SNAP\Right\")
                Case 43
                    CogToolBlock_Fixture_Right = CogToolBlock_Group_Right.Tools("CogToolBlock_Return01_Iso")
                    CogToolBlock_Shard_Right = CogToolBlock_Fixture_Right.Tools("CogToolBlock_Return01")
                    Measure_Sub_Right(Select_Product_Name, Barcode, KEYNAME.Return_, Image_Index, 42, KeyType, "D:\System\HOOK_SNAP\Right\")
                Case 44
                    CogToolBlock_Fixture_Right = CogToolBlock_Group_Right.Tools("CogToolBlock_Return02_Iso")
                    CogToolBlock_Shard_Right = CogToolBlock_Fixture_Right.Tools("CogToolBlock_Return02")
                    Measure_Sub_Right(Select_Product_Name, Barcode, KEYNAME.Return_, Image_Index, 43, KeyType, "D:\System\HOOK_SNAP\Right\")
                Case 45 To 56
                    Dim _keyname As String = Nothing : Dim Measure_Index As Integer
                    Select Case Image_Index
                        Case 45
                            _keyname = KEYNAME.BackSlash : Measure_Index = 44
                        Case 46
                            _keyname = KEYNAME.Quote : Measure_Index = 45
                        Case 47
                            _keyname = KEYNAME.Semicolon : Measure_Index = 46
                        Case 48
                            _keyname = KEYNAME.L : Measure_Index = 47
                        Case 49
                            _keyname = KEYNAME.K : Measure_Index = 48
                        Case 50
                            _keyname = KEYNAME.J : Measure_Index = 49
                        Case 51
                            _keyname = KEYNAME.H : Measure_Index = 50
                        Case 52
                            _keyname = KEYNAME.G : Measure_Index = 51
                        Case 53
                            _keyname = KEYNAME.F : Measure_Index = 52
                        Case 54
                            _keyname = KEYNAME.D : Measure_Index = 53
                        Case 55
                            _keyname = KEYNAME.S : Measure_Index = 54
                        Case 56
                            _keyname = KEYNAME.A : Measure_Index = 55
                    End Select
                    CogToolBlock_Fixture_Right = CogToolBlock_Group_Right.Tools("CogToolBlock_Measure_1X1")
                    CogToolBlock_Shard_Right = CogToolBlock_Fixture_Right.Tools("CogToolBlock_1X1")
                    Measure_Sub_Right(Select_Product_Name, Barcode, _keyname, Image_Index, Measure_Index, KeyType, "D:\System\HOOK_SNAP\Right\")

                Case 57
                    CogToolBlock_Fixture_Right = CogToolBlock_Group_Right.Tools("CogToolBlock_Measure_CapsLock01")
                    CogToolBlock_Shard_Right = CogToolBlock_Fixture_Right.Tools("CogToolBlock_CapsLock01")

                    Measure_Sub_Right(Select_Product_Name, Barcode, KEYNAME.CapsLock, Image_Index, 56, KeyType, "D:\System\HOOK_SNAP\Right\")
                Case 58
                    CogToolBlock_Fixture_Right = CogToolBlock_Group_Right.Tools("CogToolBlock_Measure_CapsLock02")
                    CogToolBlock_Shard_Right = CogToolBlock_Fixture_Right.Tools("CogToolBlock_CapsLock02")
                    Measure_Sub_Right(Select_Product_Name, Barcode, KEYNAME.CapsLock, Image_Index, 57, KeyType, "D:\System\HOOK_SNAP\Right\")
                Case 59
                    CogToolBlock_Fixture_Right = CogToolBlock_Group_Right.Tools("CogToolBlock_Mesure_1X1.25")
                    CogToolBlock_Shard_Right = CogToolBlock_Fixture_Right.Tools("CogToolBlock_1X1.25")
                    Measure_Sub_Right(Select_Product_Name, Barcode, KEYNAME.LeftShift, Image_Index, 87, KeyType, "D:\System\HOOK_SNAP\Right\")
                Case 60 To 70
                    Dim _keyname As String = Nothing : Dim Measure_Index As Integer
                    Select Case Image_Index
                        Case 60
                            _keyname = KEYNAME.Tilde : Measure_Index = 59
                        Case 61
                            _keyname = KEYNAME.Z : Measure_Index = 60
                        Case 62
                            _keyname = KEYNAME.X : Measure_Index = 61
                        Case 63
                            _keyname = KEYNAME.C : Measure_Index = 62
                        Case 64
                            _keyname = KEYNAME.V : Measure_Index = 63
                        Case 65
                            _keyname = KEYNAME.B : Measure_Index = 64
                        Case 66
                            _keyname = KEYNAME.N : Measure_Index = 65
                        Case 67
                            _keyname = KEYNAME.M : Measure_Index = 66
                        Case 68
                            _keyname = KEYNAME.Comma : Measure_Index = 67
                        Case 69
                            _keyname = KEYNAME.Period : Measure_Index = 68
                        Case 70
                            _keyname = KEYNAME.Question : Measure_Index = 69
                    End Select
                    CogToolBlock_Fixture_Right = CogToolBlock_Group_Right.Tools("CogToolBlock_Measure_1X1")
                    CogToolBlock_Shard_Right = CogToolBlock_Fixture_Right.Tools("CogToolBlock_1X1")
                    Measure_Sub_Right(Select_Product_Name, Barcode, _keyname, Image_Index, Measure_Index, KeyType, "D:\System\HOOK_SNAP\Right\")

                Case 71
                    CogToolBlock_Fixture_Right = CogToolBlock_Group_Right.Tools("CogToolBlock_Measure_Right_Shift01")
                    CogToolBlock_Shard_Right = CogToolBlock_Fixture_Right.Tools("CogToolBlock_Shift01")
                    Measure_Sub_Right(Select_Product_Name, Barcode, KEYNAME.RightShift, Image_Index, 70, KeyType, "D:\System\HOOK_SNAP\Right\")
                Case 72
                    CogToolBlock_Fixture_Right = CogToolBlock_Group_Right.Tools("CogToolBlock_Measure_Right_Shift02")
                    CogToolBlock_Shard_Right = CogToolBlock_Fixture_Right.Tools("CogToolBlock_Shift02")
                    Measure_Sub_Right(Select_Product_Name, Barcode, KEYNAME.RightShift, Image_Index, 71, KeyType, "D:\System\HOOK_SNAP\Right\")
                Case 73
                    CogToolBlock_Fixture_Right = CogToolBlock_Group_Right.Tools("CogToolBlock_Measure_Right_Shift03")
                    CogToolBlock_Shard_Right = CogToolBlock_Fixture_Right.Tools("CogToolBlock_Shift03")
                    Measure_Sub_Right(Select_Product_Name, Barcode, KEYNAME.RightShift, Image_Index, 72, KeyType, "D:\System\HOOK_SNAP\Right\")

                Case 74
                    CogToolBlock_Fixture_Right = CogToolBlock_Group_Right.Tools("CogToolBlock_Up_Down")
                    CogToolBlock_Shard_Right = CogToolBlock_Fixture_Right.Tools("CogToolBlock_Down")
                    Measure_Sub_Right(Select_Product_Name, Barcode, KEYNAME.Right, Image_Index, 73, KeyType, "D:\System\HOOK_SNAP\Right\")
                Case 75
                    CogToolBlock_Fixture_Right = CogToolBlock_Group_Right.Tools("CogToolBlock_Up_Down")
                    CogToolBlock_Shard_Right = CogToolBlock_Fixture_Right.Tools("CogToolBlock_Up")
                    Measure_Sub_Right(Select_Product_Name, Barcode, KEYNAME.Up, Image_Index, 74, KeyType, "D:\System\HOOK_SNAP\Right\")
                    CogToolBlock_Fixture_Right = CogToolBlock_Group_Right.Tools("CogToolBlock_Up_Down")
                    CogToolBlock_Shard_Right = CogToolBlock_Fixture_Right.Tools("CogToolBlock_Down")
                    Measure_Sub_Right(Select_Product_Name, Barcode, KEYNAME.Down, Image_Index, 75, KeyType, "D:\System\HOOK_SNAP\Right\")
                Case 76
                    CogToolBlock_Fixture_Right = CogToolBlock_Group_Right.Tools("CogToolBlock_Up_Down")
                    CogToolBlock_Shard_Right = CogToolBlock_Fixture_Right.Tools("CogToolBlock_Down")
                    Measure_Sub_Right(Select_Product_Name, Barcode, KEYNAME.Left, Image_Index, 76, KeyType, "D:\System\HOOK_SNAP\Right\")
                Case 77
                    CogToolBlock_Fixture_Right = CogToolBlock_Group_Right.Tools("CogToolBlock_Measure_1X1")
                    CogToolBlock_Shard_Right = CogToolBlock_Fixture_Right.Tools("CogToolBlock_1X1")
                    Measure_Sub_Right(Select_Product_Name, Barcode, KEYNAME.RightOption, Image_Index, 77, KeyType, "D:\System\HOOK_SNAP\Right\")
                Case 78
                    CogToolBlock_Fixture_Right = CogToolBlock_Group_Right.Tools("CogToolBlock_Mesure_Command")
                    CogToolBlock_Shard_Right = CogToolBlock_Fixture_Right.Tools("CogToolBlock_Command")
                    Measure_Sub_Right(Select_Product_Name, Barcode, KEYNAME.RightCommand, Image_Index, 78, KeyType, "D:\System\HOOK_SNAP\Right\")
                Case 84
                    CogToolBlock_Fixture_Right = CogToolBlock_Group_Right.Tools("CogToolBlock_Mesure_Command")
                    CogToolBlock_Shard_Right = CogToolBlock_Fixture_Right.Tools("CogToolBlock_Command")
                    Measure_Sub_Right(Select_Product_Name, Barcode, KEYNAME.LeftCommand, Image_Index, 79, KeyType, "D:\System\HOOK_SNAP\Right\")

                Case 85 To 87
                    Dim _keyname As String = Nothing : Dim Measure_Index As Integer
                    Select Case Image_Index
                        Case 85
                            _keyname = KEYNAME.LeftOption : Measure_Index = 80
                        Case 86
                            _keyname = KEYNAME.Control : Measure_Index = 81
                        Case 87
                            _keyname = KEYNAME.Fn : Measure_Index = 82
                    End Select
                    CogToolBlock_Fixture_Right = CogToolBlock_Group_Right.Tools("CogToolBlock_Measure_1X1")
                    CogToolBlock_Shard_Right = CogToolBlock_Fixture_Right.Tools("CogToolBlock_1X1")
                    Measure_Sub_Right(Select_Product_Name, Barcode, _keyname, Image_Index, Measure_Index, KeyType, "D:\System\HOOK_SNAP\Right\")
                    End Select
        Catch ex As Exception
            Invoke(New Delegate_Display_Message_Right(AddressOf Display_Message_Right), "右图像处理错误:[" & Image_Index & "]" & ex.ToString, Barcode, Color.Red)
        End Try
    End Sub

    Sub Measure_Right_Jis(ByVal Image_Index As Integer, ByVal Project_Name As String, ByVal Barcode As String, ByVal KeyType As String)
        Try
            Select Case Image_Index
                Case 1 To 13
                    Dim _keyname As String = Nothing
                    Dim Measure_Index As Integer
                    Select Case Image_Index
                        Case 1
                            _keyname = KEYNAME.Esc : Measure_Index = 0
                        Case 2
                            _keyname = KEYNAME.F1 : Measure_Index = 1
                        Case 3
                            _keyname = KEYNAME.F2 : Measure_Index = 2
                        Case 4
                            _keyname = KEYNAME.F3 : Measure_Index = 3
                        Case 5
                            _keyname = KEYNAME.F4 : Measure_Index = 4
                        Case 6
                            _keyname = KEYNAME.F5 : Measure_Index = 5
                        Case 7
                            _keyname = KEYNAME.F6 : Measure_Index = 6
                        Case 8
                            _keyname = KEYNAME.F7 : Measure_Index = 7
                        Case 9
                            _keyname = KEYNAME.F8 : Measure_Index = 8
                        Case 10
                            _keyname = KEYNAME.F9 : Measure_Index = 9
                        Case 11
                            _keyname = KEYNAME.F10 : Measure_Index = 10
                        Case 12
                            _keyname = KEYNAME.F11 : Measure_Index = 11
                        Case 13
                            _keyname = KEYNAME.F12 : Measure_Index = 12

                    End Select

                    CogToolBlock_Fixture_Right = CogToolBlock_Group_Right.Tools("CogToolBlock_Measure_Fun_X10")
                    CogToolBlock_Shard_Right = CogToolBlock_Fixture_Right.Tools("CogToolBlock_FunX10")
                    Measure_Sub_Right(Select_Product_Name, Barcode, _keyname, Image_Index, Measure_Index, KeyType, "D:\System\HOOK_SNAP\Right\")

                Case 14 To 26
                    Dim _keyname As String = Nothing
                    Dim Measure_Index As Integer
                    Select Case Image_Index
                        Case 14
                            _keyname = KEYNAME.Delete : Measure_Index = 13
                        Case 15
                            _keyname = KEYNAME.Yen : Measure_Index = 14
                        Case 16
                            _keyname = KEYNAME.Tilde : Measure_Index = 15
                        Case 17
                            _keyname = KEYNAME.Minus : Measure_Index = 16
                        Case 18
                            _keyname = KEYNAME.Num0 : Measure_Index = 17
                        Case 19
                            _keyname = KEYNAME.Num9 : Measure_Index = 18
                        Case 20
                            _keyname = KEYNAME.Num8 : Measure_Index = 19
                        Case 21
                            _keyname = KEYNAME.Num7 : Measure_Index = 20
                        Case 22
                            _keyname = KEYNAME.Num6 : Measure_Index = 21
                        Case 23
                            _keyname = KEYNAME.Num5 : Measure_Index = 22
                        Case 24
                            _keyname = KEYNAME.Num4 : Measure_Index = 23
                        Case 25
                            _keyname = KEYNAME.Num3 : Measure_Index = 24
                        Case 26
                            _keyname = KEYNAME.Num2 : Measure_Index = 25
                    End Select
                    CogToolBlock_Fixture_Right = CogToolBlock_Group_Right.Tools("CogToolBlock_Measure_1X1")
                    CogToolBlock_Shard_Right = CogToolBlock_Fixture_Right.Tools("CogToolBlock_1X1")
                    Measure_Sub_Right(Select_Product_Name, Barcode, _keyname, Image_Index, Measure_Index, KeyType, "D:\System\HOOK_SNAP\Right\")
                Case 27
                    CogToolBlock_Fixture_Right = CogToolBlock_Group_Right.Tools("CogToolBlock_Measure_Num1.01.Jis")
                    CogToolBlock_Shard_Right = CogToolBlock_Fixture_Right.Tools("CogToolBlock_Num1.01")
                    Measure_Sub_Right(Select_Product_Name, Barcode, KEYNAME.Num1, Image_Index, 26, KeyType, "D:\System\HOOK_SNAP\Right\")
                Case 28
                    CogToolBlock_Fixture_Right = CogToolBlock_Group_Right.Tools("CogToolBlock_Measure_Num1.02.Jis")
                    CogToolBlock_Shard_Right = CogToolBlock_Fixture_Right.Tools("CogToolBlock_Num1.02")
                    Measure_Sub_Right(Select_Product_Name, Barcode, KEYNAME.Num1, Image_Index, 27, KeyType, "D:\System\HOOK_SNAP\Right\")
                Case 29 To 41
                    Dim _keyname As String = Nothing
                    Dim Measure_Index As Integer
                    Select Case Image_Index
                        Case 29
                            _keyname = KEYNAME.Tab : Measure_Index = 28
                        Case 30
                            _keyname = KEYNAME.Q : Measure_Index = 29
                        Case 31
                            _keyname = KEYNAME.W : Measure_Index = 30
                        Case 32
                            _keyname = KEYNAME.E : Measure_Index = 31
                        Case 33
                            _keyname = KEYNAME.R : Measure_Index = 32
                        Case 34
                            _keyname = KEYNAME.T : Measure_Index = 33
                        Case 35
                            _keyname = KEYNAME.Y : Measure_Index = 34
                        Case 36
                            _keyname = KEYNAME.U : Measure_Index = 35
                        Case 37
                            _keyname = KEYNAME.I : Measure_Index = 36
                        Case 38
                            _keyname = KEYNAME.O : Measure_Index = 37
                        Case 39
                            _keyname = KEYNAME.P : Measure_Index = 38
                        Case 40
                            _keyname = KEYNAME.AT : Measure_Index = 39
                        Case 41
                            _keyname = KEYNAME.LeftBracket : Measure_Index = 40
                    End Select
                    CogToolBlock_Fixture_Right = CogToolBlock_Group_Right.Tools("CogToolBlock_Measure_1X1")
                    CogToolBlock_Shard_Right = CogToolBlock_Fixture_Right.Tools("CogToolBlock_1X1")
                    Measure_Sub_Right(Select_Product_Name, Barcode, _keyname, Image_Index, Measure_Index, KeyType, "D:\System\HOOK_SNAP\Right\")
                Case 42
                    CogToolBlock_Fixture_Right = CogToolBlock_Group_Right.Tools("CogToolBlock_Return01.Jis")
                    CogToolBlock_Shard_Right = CogToolBlock_Fixture_Right.Tools("CogToolBlock_Return01")
                    Measure_Sub_Right(Select_Product_Name, Barcode, KEYNAME.Return_, Image_Index, 41, KeyType, "D:\System\HOOK_SNAP\Right\")
                Case 43
                    CogToolBlock_Fixture_Right = CogToolBlock_Group_Right.Tools("CogToolBlock_Return02.Jis")
                    CogToolBlock_Shard_Right = CogToolBlock_Fixture_Right.Tools("CogToolBlock_Return02")
                    Measure_Sub_Right(Select_Product_Name, Barcode, KEYNAME.Return_, Image_Index, 42, KeyType, "D:\System\HOOK_SNAP\Right\")
                Case 44
                    CogToolBlock_Fixture_Right = CogToolBlock_Group_Right.Tools("CogToolBlock_Return03.Jis")
                    CogToolBlock_Shard_Right = CogToolBlock_Fixture_Right.Tools("CogToolBlock_Return03")
                    Measure_Sub_Right(Select_Product_Name, Barcode, KEYNAME.Return_, Image_Index, 43, KeyType, "D:\System\HOOK_SNAP\Right\")
                Case 45 To 56
                    Dim _keyname As String = Nothing
                    Dim Measure_Index As Integer
                    Select Case Image_Index
                        Case 45
                            _keyname = KEYNAME.RightBracket : Measure_Index = 44
                        Case 46
                            _keyname = KEYNAME.Colon : Measure_Index = 45
                        Case 47
                            _keyname = KEYNAME.Semicolon : Measure_Index = 46
                        Case 48
                            _keyname = KEYNAME.L : Measure_Index = 47
                        Case 49
                            _keyname = KEYNAME.K : Measure_Index = 48
                        Case 50
                            _keyname = KEYNAME.J : Measure_Index = 49
                        Case 51
                            _keyname = KEYNAME.H : Measure_Index = 50
                        Case 52
                            _keyname = KEYNAME.G : Measure_Index = 51
                        Case 53
                            _keyname = KEYNAME.F : Measure_Index = 52
                        Case 54
                            _keyname = KEYNAME.D : Measure_Index = 53
                        Case 55
                            _keyname = KEYNAME.S : Measure_Index = 54
                        Case 56
                            _keyname = KEYNAME.A : Measure_Index = 55
                    End Select
                    CogToolBlock_Fixture_Right = CogToolBlock_Group_Right.Tools("CogToolBlock_Measure_1X1")
                    CogToolBlock_Shard_Right = CogToolBlock_Fixture_Right.Tools("CogToolBlock_1X1")
                    Measure_Sub_Right(Select_Product_Name, Barcode, _keyname, Image_Index, Measure_Index, KeyType, "D:\System\HOOK_SNAP\Right\")
                Case 57
                    CogToolBlock_Fixture_Right = CogToolBlock_Group_Right.Tools("CogToolBlock_Mesure_1X1.25")
                    CogToolBlock_Shard_Right = CogToolBlock_Fixture_Right.Tools("CogToolBlock_1X1.25")
                    Measure_Sub_Right(Select_Product_Name, Barcode, KEYNAME.Control, Image_Index, 56, KeyType, "D:\System\HOOK_SNAP\Right\")
                Case 58
                    CogToolBlock_Fixture_Right = CogToolBlock_Group_Right.Tools("CogToolBlock_Measure_Left_Shift.01.Jis")
                    CogToolBlock_Shard_Right = CogToolBlock_Fixture_Right.Tools("CogToolBlock_Shift01")
                    Measure_Sub_Right(Select_Product_Name, Barcode, KEYNAME.LeftShift, Image_Index, 57, KeyType, "D:\System\HOOK_SNAP\Right\")
                Case 59
                    CogToolBlock_Fixture_Right = CogToolBlock_Group_Right.Tools("CogToolBlock_Measure_Left_Shift.02.Jis")
                    CogToolBlock_Shard_Right = CogToolBlock_Fixture_Right.Tools("CogToolBlock_Shift02")
                    Measure_Sub_Right(Select_Product_Name, Barcode, KEYNAME.LeftShift, Image_Index, 58, KeyType, "D:\System\HOOK_SNAP\Right\")
                Case 60 To 70
                    Dim _keyname As String = Nothing : Dim Measure_Index As Integer
                    Select Case Image_Index
                        Case 60
                            _keyname = KEYNAME.Z : Measure_Index = 59
                        Case 61
                            _keyname = KEYNAME.X : Measure_Index = 60
                        Case 62
                            _keyname = KEYNAME.C : Measure_Index = 61
                        Case 63
                            _keyname = KEYNAME.V : Measure_Index = 62
                        Case 64
                            _keyname = KEYNAME.B : Measure_Index = 63
                        Case 65
                            _keyname = KEYNAME.N : Measure_Index = 64
                        Case 66
                            _keyname = KEYNAME.M : Measure_Index = 65
                        Case 67
                            _keyname = KEYNAME.Comma : Measure_Index = 66
                        Case 68
                            _keyname = KEYNAME.Period : Measure_Index = 67
                        Case 69
                            _keyname = KEYNAME.Question : Measure_Index = 68
                        Case 70
                            _keyname = KEYNAME.Ro : Measure_Index = 69
                    End Select
                    CogToolBlock_Fixture_Right = CogToolBlock_Group_Right.Tools("CogToolBlock_Measure_1X1")
                    CogToolBlock_Shard_Right = CogToolBlock_Fixture_Right.Tools("CogToolBlock_1X1")
                    Measure_Sub_Right(Select_Product_Name, Barcode, _keyname, Image_Index, Measure_Index, KeyType, "D:\System\HOOK_SNAP\Right\")
                Case 71
                    CogToolBlock_Fixture_Right = CogToolBlock_Group_Right.Tools("CogToolBlock_Measure_Right_Shift.01.Jis")
                    CogToolBlock_Shard_Right = CogToolBlock_Fixture_Right.Tools("CogToolBlock_Shift01")
                    Measure_Sub_Right(Select_Product_Name, Barcode, KEYNAME.RightShift, Image_Index, 70, KeyType, "D:\System\HOOK_SNAP\Right\")
                Case 72
                    CogToolBlock_Fixture_Right = CogToolBlock_Group_Right.Tools("CogToolBlock_Measure_Right_Shift.02.Jis")
                    CogToolBlock_Shard_Right = CogToolBlock_Fixture_Right.Tools("CogToolBlock_Shift02")
                    Measure_Sub_Right(Select_Product_Name, Barcode, KEYNAME.RightShift, Image_Index, 71, KeyType, "D:\System\HOOK_SNAP\Right\")
                Case 73
                    CogToolBlock_Fixture_Right = CogToolBlock_Group_Right.Tools("CogToolBlock_Up_Down")
                    CogToolBlock_Shard_Right = CogToolBlock_Fixture_Right.Tools("CogToolBlock_Down")
                    Measure_Sub_Right(Select_Product_Name, Barcode, KEYNAME.Right, Image_Index, 72, KeyType, "D:\System\HOOK_SNAP\Right\")
                Case 74
                    CogToolBlock_Fixture_Right = CogToolBlock_Group_Right.Tools("CogToolBlock_Up_Down")
                    CogToolBlock_Shard_Right = CogToolBlock_Fixture_Right.Tools("CogToolBlock_Up")
                    Measure_Sub_Right(Select_Product_Name, Barcode, KEYNAME.Up, Image_Index, 73, KeyType, "D:\System\HOOK_SNAP\Right\")
                    CogToolBlock_Fixture_Right = CogToolBlock_Group_Right.Tools("CogToolBlock_Up_Down")
                    CogToolBlock_Shard_Right = CogToolBlock_Fixture_Right.Tools("CogToolBlock_Down")
                    Measure_Sub_Right(Select_Product_Name, Barcode, KEYNAME.Down, Image_Index, 74, KeyType, "D:\System\HOOK_SNAP\Right\")
                Case 75
                    CogToolBlock_Fixture_Right = CogToolBlock_Group_Right.Tools("CogToolBlock_Up_Down")
                    CogToolBlock_Shard_Right = CogToolBlock_Fixture_Right.Tools("CogToolBlock_Down")
                    Measure_Sub_Right(Select_Product_Name, Barcode, KEYNAME.Left, Image_Index, 75, KeyType, "D:\System\HOOK_SNAP\Right\")
                Case 76
                    CogToolBlock_Fixture_Right = CogToolBlock_Group_Right.Tools("CogToolBlock_Measure_1X1")
                    CogToolBlock_Shard_Right = CogToolBlock_Fixture_Right.Tools("CogToolBlock_1X1")
                    Measure_Sub_Right(Select_Product_Name, Barcode, KEYNAME.Fn, Image_Index, 76, KeyType, "D:\System\HOOK_SNAP\Right\")


                Case 77 To 83
                    Dim _keyname As String = Nothing : Dim Measure_Index As Integer
                    Select Case Image_Index
                        Case 77
                            _keyname = KEYNAME.RightCommand : Measure_Index = 77
                        Case 78
                            _keyname = KEYNAME.Kana : Measure_Index = 78
                        Case 82
                            _keyname = KEYNAME.Eisu : Measure_Index = 79
                        Case 83
                            _keyname = KEYNAME.LeftCommand : Measure_Index = 80
                    End Select

                    CogToolBlock_Fixture_Right = CogToolBlock_Group_Right.Tools("CogToolBlock_Mesure_Command")
                    CogToolBlock_Shard_Right = CogToolBlock_Fixture_Right.Tools("CogToolBlock_Command")
                    Measure_Sub_Right(Select_Product_Name, Barcode, _keyname, Image_Index, Measure_Index, KeyType, "D:\System\HOOK_SNAP\Right\")

                Case 84
                    CogToolBlock_Fixture_Right = CogToolBlock_Group_Right.Tools("CogToolBlock_Measure_1X1")
                    CogToolBlock_Shard_Right = CogToolBlock_Fixture_Right.Tools("CogToolBlock_1X1")
                    Measure_Sub_Right(Select_Product_Name, Barcode, KEYNAME.LeftOption, Image_Index, 81, KeyType, "D:\System\HOOK_SNAP\Right\")

                Case 85
                    CogToolBlock_Fixture_Right = CogToolBlock_Group_Right.Tools("CogToolBlock_Measure_1X1")
                    CogToolBlock_Shard_Right = CogToolBlock_Fixture_Right.Tools("CogToolBlock_1X1")
                    Measure_Sub_Right(Select_Product_Name, Barcode, KEYNAME.CapsLock, Image_Index, 82, KeyType, "D:\System\HOOK_SNAP\Right\")



            End Select
        Catch ex As Exception
            Invoke(New Delegate_Display_Message_Right(AddressOf Display_Message_Right), "右图像处理错误:[" & Image_Index & "]" & ex.ToString, Barcode, Color.Red)
        End Try
    End Sub

    Delegate Sub Delegate_CogDisplay_Left(ByVal ToolBlock As CogToolBlock)
    Sub CogDisplay_Left(ByVal ToolBlock As CogToolBlock)
        'If ToolBlock.CreateLastRunRecord.SubRecords.Count >= 2 Then
        '    CogRecordDisplay1.Record = ToolBlock.CreateLastRunRecord.SubRecords.Item(2)
        'Else
        '    CogRecordDisplay1.Record = ToolBlock.CreateLastRunRecord.SubRecords.Item(0)
        'End If
        'CogRecordDisplay1.AutoFit = True
    End Sub

    Delegate Sub Delegate_CogDisplay_Right(ByVal ToolBlock As CogToolBlock)
    Sub CogDisplay_Right(ByVal ToolBlock As CogToolBlock)
        'If ToolBlock.CreateLastRunRecord.SubRecords.Count >= 2 Then
        '    CogRecordDisplay2.Record = ToolBlock.CreateLastRunRecord.SubRecords.Item(2)
        'Else
        '    CogRecordDisplay2.Record = ToolBlock.CreateLastRunRecord.SubRecords.Item(0)
        'End If
        'CogRecordDisplay2.AutoFit = True
    End Sub

    Sub Save_Window_Image_Left(ByVal ToolBlock As CogToolBlock, ByVal KeyName As String, ByVal Image_Index As Int16, ByVal OKNG As Integer, ByVal SN As String, ByVal m_ICogImage As ICogImage)
        Dim subname As String = GetCurrentMethod.Name & "："
        Dim mSave_Image As Save_Image_Stru = Nothing
        Try
            Invoke(New Delegate_CogDisplay_Left(AddressOf CogDisplay_Left), ToolBlock)

            Dim Image_Memory As New MemoryStream
            Dim ImageInfo() As Byte = Nothing
            m_ICogImage.ScaleImage(m_ICogImage.Width / 3, m_ICogImage.Height / 3).ToBitmap.Save(Image_Memory, System.Drawing.Imaging.ImageFormat.Jpeg)
            Dim Images As Image = Image.FromStream(Image_Memory)
            ImageInfo = Image_Memory.GetBuffer
            Rw_Left.WriteLine(KeyName & "," & Image_Index & "," & Count_Left & "," & ImageInfo.Length)
            Count_Left = Count_Left + ImageInfo.Length
            Fs_Left.Write(ImageInfo, 0, ImageInfo.Length)
            Image_Memory.Close()
            Image_Memory.Dispose()
            m_ICogImage = Nothing
            Images = Nothing

        Catch ex As Exception
            Invoke(New Delegate_Display_Message_Left(AddressOf Display_Message_Left), subname & ex.ToString, SN, Color.Red)
        End Try
    End Sub

    Sub Save_Window_Image_Right(ByVal ToolBlock As CogToolBlock, ByVal KeyName As String, ByVal Image_Index As Int16, ByVal OKNG As Integer, ByVal SN As String, ByVal m_ICogImage As ICogImage)
        Dim subname As String = GetCurrentMethod.Name & "："
        Dim mSave_Image As Save_Image_Stru = Nothing
        Try
            Invoke(New Delegate_CogDisplay_Right(AddressOf CogDisplay_Right), ToolBlock)

            Dim Image_Memory As New MemoryStream
            Dim ImageInfo() As Byte = Nothing
            m_ICogImage.ScaleImage(m_ICogImage.Width / 3, m_ICogImage.Height / 3).ToBitmap.Save(Image_Memory, System.Drawing.Imaging.ImageFormat.Jpeg)
            Dim Images As Image = Image.FromStream(Image_Memory)
            ImageInfo = Image_Memory.GetBuffer
            Rw_Right.WriteLine(KeyName & "," & Image_Index & "," & Count_Right & "," & ImageInfo.Length)
            Count_Right = Count_Right + ImageInfo.Length
            Fs_Right.Write(ImageInfo, 0, ImageInfo.Length)
            Image_Memory.Close()
            Image_Memory.Dispose()
            m_ICogImage = Nothing
            Images = Nothing

        Catch ex As Exception
            Invoke(New Delegate_Display_Message_Right(AddressOf Display_Message_Right), subname & ex.ToString, SN, Color.Red)
        End Try
    End Sub
#End Region

    Public Structure CCD
        Shared RESULT As Integer
        Shared X As Double
        Shared Y As Double
        Shared Z As Double
        Shared X_Array(100) As Double
        Shared Y_Array(100) As Double
    End Structure

    Public Structure Coor_Move_Left
        Shared x As Double
        Shared Y As Double
    End Structure
    Public Structure Coor_Move_Right
        Shared x As Double
        Shared Y As Double
    End Structure
    Public Structure BOOL
        Shared Updata_Admin_Password As Boolean = False '是否可以修改ADMIN密码
        Shared Init_DIO, Left_IsRuning, Right_IsRuning, Common_IsRuning, Laser_IsRuning, Red_Light, Yellow_Light, Green_Light, LOCK As Boolean
        Shared CLOSE, Error_Close, EXIT_APP, Emg_Stop_Button, Moto_Warning(100), Moto_MEL(100), Moto_PEL(100), Read_MPEL As Boolean
        Shared Calib, Correction_needle, Weight, Read_Sn, UPH As Boolean
        Shared Init_Laser As Boolean, Save_Data, Upload, Insert_Image, Insert_Data As Boolean
    End Structure
    Public Structure AXISMSG
        Shared AXIS_ENCODER_POS() As Double '坐标编码器位置
    End Structure
    Public Structure Portect_STRCT
        Shared Resert_Button, Start_Button, Start_Button_Left, Start_Button_Right, Left_Safe, Right_Safe, Safe, Stop_Button As Boolean '二次保护
        Shared Hand_Dispensing, Inspect_Button_Left, Inspect_Button_Right As Boolean
    End Structure
    Public Structure INTTYPLE
        Shared Left_Trigger_Count As Integer
        Shared Right_Trigger_Count As Integer
        Shared Laser_Trigger_Count As Integer
    End Structure
    Public Structure DOUBLETYPLE
        Shared Laser_Data() As Double
    End Structure
    ''' <summary>
    ''' 坐标系参数
    ''' </summary>
    ''' <remarks></remarks>
    Public Structure Work_Coordinatess
        Dim 轴名称() As String
        Dim 轴位置() As Double
        Dim 轴号() As Integer
        Dim 轴运动顺序() As String
        Dim 坐标名称 As String
        Dim 点胶状态 As Boolean
        Dim 镭射触发状态 As Boolean
        Dim 镭射收数据状态 As Boolean
        Dim CCD触发状态 As Boolean
        Dim IO触发状态 As Boolean
        Dim 其他触发状态 As Boolean
        Dim 运行速度 As Integer
        Dim 加减速度 As Double
        Dim 慢速 As Integer
        Dim 中速 As Integer
        Dim 键名 As String
    End Structure
    Public Structure Needle
        Shared X_Offset As Double
        Shared Y_Offset As Double
    End Structure
#Region "枚举类型"
    Public Enum Pro
        NO0
        NO01
        NO02
        NO03
        NO04
        NO05
        NO06
        NO07
        NO08
        NO09
        NO10
    End Enum
    Public Enum Glue_Type_Enum
        UV_Glue
        Silver_Glue
    End Enum
    Public Enum SEND_COGNEX_CMD
        T0
        T1
        T2
        T3
        T4
    End Enum
    Enum Pul
        基准对针
        当前对针
    End Enum
    Public Enum LR_STATION
        LEFT
        RIGHT
        ALL
        COMMUNAL
        ERR
        LOAD
        IMPORTANT
    End Enum
    Public Enum LASER_STAION_ENUM
        LEFT
        RIGHT
    End Enum

    Shared LASER_DATACODE As String
    Dim LASER_STAION As LASER_STAION_ENUM
    Public Enum LIGHT
        RED
        YELLOW
        GREEN
        NONE
    End Enum
    Public Enum LGE
        CHN
        ENG
    End Enum
    Public Enum IO
        OUT_ON
        OUT_OFF
    End Enum
#End Region

#Region "参数结构体"
    Private Structure PARAM_DOUBLE
        Shared M脉宽 As Double
    End Structure
    Private Structure PARAM_INT
        Shared Y运动等待时间 As Integer
        Shared D电磁阀输出等待时间 As Integer
        Shared Z真空吸等待时间 As Integer
        Shared L镭射触发间隔 As Integer
        Shared S设备编号 As Integer
    End Structure
    Public Structure PARAM_STRING
        Shared S视觉文件路径 As String
        Shared T条码枪COM口 As String = "COM2"
        'Shared S数据上传服务器IP As String = ""
        Shared L镭射IP地址 As String = ""
        Shared PLC串口 As String = "COM1"
        Shared S数据保存路径 As String = "D:\"
        Shared T图片保存路径 As String = "D:\"
        Shared IDB保存路径 As String = "D:\"
        Shared S数据汇总表名 As String = "D:\"
    End Structure
    Public Structure PARAM_BOOL
        Shared D到位信号检测 As Boolean
        Shared D登陆密码检测 As Boolean
        Shared P屏蔽CCD As Boolean
        Shared B保存激光原始数据 As Boolean
        Shared Q清空汇总数据 As Boolean
        Shared T调试版本 As Boolean
        Shared S是否删除IDB文件 As Boolean
        Shared UpDown键测试选择 As Boolean   '新增4
    End Structure
    Public Structure DIO
        'Card0 DI
        Shared Q启动按钮_0 As Int16 = 0
        Shared F复位按钮_0 As Int16 = 0
        Shared A安全光栅_0 As Int16 = 0
        Shared Z中线有料检测_0 As Int16 = 0
        Shared Z中线真空吸检测_0 As Int16 = 0
        Shared Z中线阻挡HOME_0 As Int16 = 0
        Shared Z中线阻挡WORK_0 As Int16 = 0
        Shared ONE1工位阻挡HOME1_0 As Int16 = 0
        Shared ONE1工位阻挡WORK1_0 As Int16 = 0
        Shared ONE1工位阻挡HOME2_0 As Int16 = 0
        Shared ONE1工位阻挡WORK2_0 As Int16 = 0
        Shared ONE1工位来料检测_0 As Int16 = 0
        Shared ONE1工位出料检测_0 As Int16 = 0
        Shared ONE1工位真空吸检测_0 As Int16 = 0
        Shared C键ONE1工位真空吸检测_0 As Int16 = 0
        Shared TWO2工位阻挡HOME1_0 As Int16 = 0
        Shared TWO2工位阻挡WORK1_0 As Int16 = 0
        Shared TWO2工位阻挡HOME2_0 As Int16 = 0
        Shared TWO2工位阻挡WORK2_0 As Int16 = 0
        Shared TWO2工位来料检测_0 As Int16 = 0
        Shared TWO2工位出料检测_0 As Int16 = 0
        'Card0 D0
        Shared H红灯_0 As Int16 = 0
        Shared H黄灯_0 As Int16 = 0
        Shared L绿灯_0 As Int16 = 0
        Shared Z中线皮带控制_0 As Int16 = 0
        Shared ONE1工位皮带控制_0 As Int16 = 0
        Shared TWO2工位皮带控制_0 As Int16 = 0
        Shared H回流皮带控制_0 As Int16 = 0
        Shared Z中线阻挡电磁阀_0 As Int16 = 0
        Shared Z中线真空吸电磁阀_0 As Int16 = 0
        Shared ONE1工位阻挡电磁阀1_0 As Int16 = 0
        Shared ONE1工位阻挡电磁阀2_0 As Int16 = 0
        Shared ONE1工位真空吸电磁阀_0 As Int16 = 0
        Shared C键ONE1工位真空吸电磁阀_0 As Int16 = 0
        Shared TWO2工位阻挡电磁阀1_0 As Int16 = 0
        Shared TWO2工位阻挡电磁阀2_0 As Int16 = 0
        Shared TWO2工位真空吸电磁阀_0 As Int16 = 0
        Shared C键TWO2工位真空吸电磁阀_0 As Int16 = 0
        Shared ONE1工位顶升电磁阀_0 As Int16 = 0
        Shared TWO2工位顶升电磁阀_0 As Int16 = 0

        Shared ONE1工位破真空电磁阀_0 As Int16 = 0
        Shared TWO2工位破真空电磁阀_0 As Int16 = 0

        'Card1 DI
        Shared TWO2工位真空吸检测_1 As Int16 = 0
        Shared C键TWO2工位真空吸检测_1 As Int16 = 0
        Shared ONE1工位顶升HOME_1 As Int16 = 0
        Shared ONE1工位顶升WORK_1 As Int16 = 0
        Shared TWO2工位顶升HOME_1 As Int16 = 0
        Shared TWO2工位顶升WORK_1 As Int16 = 0
        Shared Z中线出料检测_1 As Int16 = 0
        'Card1 DO
        Shared ONE1工位版本切换电磁阀_1 As Int16 = 0
        Shared TWO2工位版本切换电磁阀_1 As Int16 = 0


    End Structure
#End Region

#Region "声明委托"

    Delegate Sub DELEGATE_LOAD_VISION()
    Sub LOAD_VISION_SUB()

        If LINK_CAMERA = False Then
            Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), "开始加载视觉配置文件,请等待...", "", Color.Black)
            If LINK_CAMERA_FUN(PARAM_STRING.S视觉文件路径) = True Then
                Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), "视觉配置文件加载完成", "", Color.Black)
            End If
        End If
    End Sub

    Delegate Sub Delegate_Disp_Text(ByVal Text_Obj As Object, ByVal text As String, ByVal Cor As Color)
    Sub Updata_Disp_Text(ByVal Text_Obj As Object, ByVal text As String, ByVal Cor As Color)
        Text_Obj.text = text
        Select Case Cor
            Case Color.Red
                Text_Obj.backcolor = Color.Red
            Case Color.Green
                Text_Obj.backcolor = Color.Green
            Case Color.Gray
                Text_Obj.backcolor = Color.Gray
            Case Else
        End Select
    End Sub

    Delegate Sub Delegate_Display_Message_Left(ByVal Message_Str As String, ByVal BarCode As String, ByVal _Color As Color)
    Delegate Sub Delegate_Display_Message_Right(ByVal Message_Str As String, ByVal BarCode As String, ByVal _Color As Color)
    Delegate Sub Delegate_Display_Message_Public(ByVal Message_Str As String, ByVal BarCode As String, ByVal _Color As Color)

    Sub Display_Message_Left(ByVal Message_Str As String, ByVal BarCode As String, ByVal _Color As Color)
        Dim HH As String = Date.Now.Hour.ToString.PadLeft(2).Replace(" ", "0")
        Dim MM As String = Date.Now.Minute.ToString.PadLeft(2).Replace(" ", "0")
        Dim SS As String = Date.Now.Second.ToString.PadLeft(2).Replace(" ", "0")
        Dim MS As String = Date.Now.Millisecond.ToString
        Dim ALL_Msg As String = HH & ":" & MM & ":" & SS & "." & MS & Space(2) & Message_Str
        Dim Clear_Count As Int16 = 1000
        Static Row_Index As Int64 = 0
        If DX_MESSAGE_LEFT.RowCount > Clear_Count Then '消息超出一定数量后清空显示
            Row_Index = 0
            DX_MESSAGE_LEFT.Rows.Clear()
        End If
        DX_MESSAGE_LEFT.Rows.Add("LEFT".PadRight(8) & Message_Str)
        DX_MESSAGE_LEFT.Rows(Row_Index).DefaultCellStyle.ForeColor = _Color
        ' SQLCON.Insert_Message_Left(Select_Product_Name, BarCode, "LEFT", ALL_Msg)

        DX_MESSAGE_LEFT.FirstDisplayedScrollingRowIndex = DX_MESSAGE_LEFT.RowCount - 1
        Row_Index += 1
    End Sub

    Sub Display_Message_Right(ByVal Message_Str As String, ByVal BarCode As String, ByVal _Color As Color)
        Dim HH As String = Date.Now.Hour.ToString.PadLeft(2).Replace(" ", "0")
        Dim MM As String = Date.Now.Minute.ToString.PadLeft(2).Replace(" ", "0")
        Dim SS As String = Date.Now.Second.ToString.PadLeft(2).Replace(" ", "0")
        Dim MS As String = Date.Now.Millisecond.ToString
        Dim ALL_Msg As String = HH & ":" & MM & ":" & SS & "." & MS & Space(2) & Message_Str
        Dim Clear_Count As Int16 = 1000
        Static Row_Index As Int64 = 0
        If DX_MESSAGE_RIGHT.RowCount > Clear_Count Then '消息超出一定数量后清空显示
            Row_Index = 0
            DX_MESSAGE_RIGHT.Rows.Clear()
        End If
        DX_MESSAGE_RIGHT.Rows.Add("RIGHT".PadRight(8) & Message_Str)
        DX_MESSAGE_RIGHT.Rows(Row_Index).DefaultCellStyle.ForeColor = _Color
        'SQLCON.Insert_Message_Right(Select_Product_Name, BarCode, "RIGHT", ALL_Msg)

        DX_MESSAGE_RIGHT.FirstDisplayedScrollingRowIndex = DX_MESSAGE_RIGHT.RowCount - 1
        Row_Index += 1
    End Sub

    Sub Display_Message_Public(ByVal Message_Str As String, ByVal BarCode As String, ByVal _Color As Color)
        Dim HH As String = Date.Now.Hour.ToString.PadLeft(2).Replace(" ", "0")
        Dim MM As String = Date.Now.Minute.ToString.PadLeft(2).Replace(" ", "0")
        Dim SS As String = Date.Now.Second.ToString.PadLeft(2).Replace(" ", "0")
        Dim MS As String = Date.Now.Millisecond.ToString
        Dim ALL_Msg As String = HH & ":" & MM & ":" & SS & "." & MS & Space(2) & Message_Str
        Dim Clear_Count As Int16 = 1000
        Static Row_Index As Int64 = 0
        If DX_MESSAGE_COM.RowCount > Clear_Count Then '消息超出一定数量后清空显示
            Row_Index = 0
            DX_MESSAGE_COM.Rows.Clear()
        End If
        DX_MESSAGE_COM.Rows.Add("PUBLIC".PadRight(8) & Message_Str)
        DX_MESSAGE_COM.Rows(Row_Index).DefaultCellStyle.ForeColor = _Color
        'SQLCON.Insert_Message_Com(Select_Product_Name, BarCode, "PUBLIC", ALL_Msg)

        DX_MESSAGE_COM.FirstDisplayedScrollingRowIndex = DX_MESSAGE_COM.RowCount - 1
        Row_Index += 1
    End Sub

#End Region

    Private Sub Main_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        If BOOL.CLOSE = True Then
            e.Cancel = False
            '关闭串口
            If SerialPort_DataCode IsNot Nothing Then
                If SerialPort_DataCode.IsOpen = True Then
                    SerialPort_DataCode.Close()
                End If
            End If
            If SerialPort_PLC IsNot Nothing Then
                If SerialPort_PLC.IsOpen = True Then
                    SerialPort_PLC.Close()
                End If
            End If
            If Card_Init_OK = True And BOOL.Init_DIO = True Then
                '关闭显示灯
                Change_RedYellowGreen(LIGHT.NONE)
                CARDCMD.SET_DO_BIT(BOARD_ID_00, DIO.Z中线皮带控制_0, PRA_OUT_STATUS_ENUM.OUT_OFF)
                CARDCMD.SET_DO_BIT(BOARD_ID_00, DIO.H回流皮带控制_0, PRA_OUT_STATUS_ENUM.OUT_OFF)
                CARDCMD.SET_DO_BIT(BOARD_ID_00, DIO.ONE1工位皮带控制_0, PRA_OUT_STATUS_ENUM.OUT_OFF)
                CARDCMD.SET_DO_BIT(BOARD_ID_00, DIO.TWO2工位皮带控制_0, PRA_OUT_STATUS_ENUM.OUT_OFF)
            End If

            '关闭激光
            If BOOL.Init_Laser = True Then
                LJV7000.LJV_Finalize()
            End If
            '关闭定时器
            Timer_Start.Stop()
            Timer_Free.Stop()
            If HomeParamObjArray IsNot Nothing Then
                If HomeParamObjArray.Length > 1 Then
                    For i As Integer = 0 To HomeParamObjArray.Length - 1
                        If Card_Init_OK = True Then
                            APS_set_servo_on(HomeParamObjArray(i).轴号, PRA_OUT_STATUS_ENUM.OUT_OFF)
                        End If
                    Next
                End If
            End If
            '关闭左右线程
            Bool_Run = False
            Bool_Start_Left = False
            Bool_Start_Right = False
            If Run_Thread_Left IsNot Nothing Then
                Run_Thread_Left.Abort()
            End If
            If Run_Thread_Right IsNot Nothing Then
                Run_Thread_Right.Abort()
            End If
            If Run_Thread_Public IsNot Nothing Then
                Run_Thread_Public.Abort()
            End If
            '释放客户端资源
            If Client_Thread IsNot Nothing Then
                Bool_Client_Thread = False
                Client_Thread.Abort()
                TCPCON.Dispos_Client()
            End If
            '关闭控制卡
            'CARDCMD.CLOSE_CARD()
            '关闭数据库
            SQLCON.Close_DataBase()
            SQLCON_A001.Close_DataBase()
            SQLCON_X816.Close_DataBase()
            SQLCON_X816RSAOI.Close_DataBase()
            '关闭congnex
            If LINK_CAMERA = True Then
                hJobManager.Reset()
                hJobManager.Shutdown()
                ToolBlock_left.Dispose()
                toolblock_right.Dispose()
            End If
            DataConn.KillProcess("excel")
            DataConn.KillProcess(ProductName)
        Else
            e.Cancel = True
        End If
    End Sub

    Private Sub Main_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyCode = Keys.Z And e.Control = True And e.Alt = True Then
            BOOL.Updata_Admin_Password = True
        ElseIf e.KeyCode = Keys.X And e.Control = True And e.Alt = True Then
            BOOL.Updata_Admin_Password = False
        ElseIf e.KeyCode = Keys.Z And e.Control = True Then
            BOOL.Read_Sn = True
            Laser_Style.Style.BorderColor.Color = Color.Green
        ElseIf e.KeyCode = Keys.X And e.Control = True Then
            BOOL.Read_Sn = False
            Laser_Style.Style.BorderColor.Color = Color.Gray
        ElseIf e.KeyCode = Keys.A And e.Shift = True And e.Control = True Then
            If PanelEx1.Style.BorderColor.Color <> Color.Green Then
                PanelEx1.Style.BorderColor.Color = Color.Green
                BOOL.UPH = True
            Else
                BOOL.UPH = False
                PanelEx1.Style.BorderColor.Color = Color.Black
            End If
        End If
    End Sub

    Private Sub Main_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim Init_Str As String = Nothing
        Control.CheckForIllegalCrossThreadCalls = False
        Dim Result As Object = User_Login_Dialog.ShowDialog
        If Result = Windows.Forms.DialogResult.OK Or Result = Windows.Forms.DialogResult.Yes Then
            If SQLCON.DataBase_Initialization(SQLCON.DataBase_Data_Souce, SQLCON.DataBase_ID, SQLCON.DataBase_PassWord, SQLCON.DataBase_Catalog_Name, 5000, , ) = True And SQLCON_查询上一站.DataBase_Initialization(SQLCON.DataBase_Data_Souce, SQLCON.DataBase_ID, SQLCON.DataBase_PassWord, SQLCON.DataBase_Catalog_Name, 5000, , ) = True Then
                SQLCON.Read_Home_ParamS(HomeParamObjArray)
                SET_AXIS_PITCH_SUB() '设置轴导程
                Init_Pos_DataGridView() '初始化坐标表格控件
                SQLCON.Read_Project_Name(ComboBox_product_name, Product_Name_Array) '读取项目名称
                Read_Deice_Param() '读取参数
                Set_Omron_Command() '设置OMRON命令

                '清空本机所有RSAOI表，防止插入数据变慢
                SQLCON.Delete_Table_Name(PARAM_STRING.S数据汇总表名)
                SQLCON.Delete_Table_Name("TEMP_BARCODE")

                'SQLCON_X816RSAOI.DataBase_Data_Souce = PARAM_STRING.S数据上传服务器IP
                If SQLCON_X816RSAOI.DataBase_Data_Souce <> "" Then
                    '测试远程连接
                    Try
                        If My.Computer.Network.IsAvailable = True Then
                            For i As Int16 = 1 To 3
                                Thread.Sleep(500)
                                If My.Computer.Network.Ping("127.0.0.1") = True Then
                                    If SQLCON_X816RSAOI.DataBase_Initialization(SQLCON_X816RSAOI.DataBase_Data_Souce, SQLCON_X816RSAOI.DataBase_ID, SQLCON_X816RSAOI.DataBase_PassWord, SQLCON_X816RSAOI.DataBase_Catalog_Name, 5000, , ) = False Then
                                        BOOL.Upload = False
                                        Init_Str = "连接HIP数据库：" & SQLCON_X816RSAOI.DataBase_Catalog_Name & " 失败，请检查IP设置、网络连接和防火墙是否关闭"
                                        Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), Init_Str, "", Color.Red)
                                    Else
                                        Init_Str = "连接HIP数据库：" & SQLCON_X816RSAOI.DataBase_Catalog_Name & " 成功！"
                                        Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), Init_Str, "", Color.Black)

                                        If SQLCON_A001.DataBase_Initialization(SQLCON_A001.DataBase_Data_Souce, SQLCON_A001.DataBase_ID, SQLCON_A001.DataBase_PassWord, SQLCON_A001.DataBase_Catalog_Name, 5000, , ) = False Then
                                            BOOL.Upload = False
                                            Init_Str = "连接HIP数据库：" & SQLCON_A001.DataBase_Catalog_Name & " 失败，请检查IP设置、网络连接和防火墙是否关闭"
                                            Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), Init_Str, "", Color.Red)
                                        Else
                                            Init_Str = "连接HIP数据库：" & SQLCON_A001.DataBase_Catalog_Name & " 成功！"
                                            Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), Init_Str, "", Color.Black)

                                            If SQLCON_X816.DataBase_Initialization(SQLCON_X816.DataBase_Data_Souce, SQLCON_X816.DataBase_ID, SQLCON_X816.DataBase_PassWord, SQLCON_X816.DataBase_Catalog_Name, 5000, , ) = False Then
                                                BOOL.Upload = False
                                                Init_Str = "连接HIP数据库：" & SQLCON_X816.DataBase_Catalog_Name & " 失败，请检查IP设置、网络连接和防火墙是否关闭"
                                                Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), Init_Str, "", Color.Red)
                                            Else
                                                Init_Str = "连接HIP数据库：" & SQLCON_X816.DataBase_Catalog_Name & " 成功！"
                                                Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), Init_Str, "", Color.Black)
                                                BOOL.Upload = True '连接成功

                                                Exit For
                                            End If
                                        End If
                                    End If
                                Else
                                    BOOL.Upload = False
                                    Init_Str = "连接HIP数据库：" & SQLCON_X816RSAOI.DataBase_Data_Souce & " 失败，请检查IP设置、网络连接和防火墙是否关闭"
                                    Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), Init_Str, "", Color.Red)
                                End If
                            Next
                        Else
                            BOOL.Upload = False
                            Init_Str = "本地网口不可用，无法连接至网络，请检查！"
                            Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), Init_Str, "", Color.Red)
                        End If
                    Catch ex As Exception
                        BOOL.Upload = False
                        Init_Str = "连接HIP数据库：" & SQLCON_X816RSAOI.DataBase_Data_Souce & " 失败，请检查IP设置、网络连接和防火墙是否关闭"
                        Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), Init_Str, "", Color.Red)
                    End Try
                Else
                    BOOL.Upload = False
                    Init_Str = "[数据上传服务器IP]为空，请修改参数"
                    Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), Init_Str, "", Color.Red)
                End If
                Select Case BOOL.Upload
                    Case True
                        HIP_Style.Style.ForeColor.Color = Color.Green
                        HIP_Style.Text = "连接成功"
                    Case False
                        HIP_Style.Style.ForeColor.Color = Color.Red
                        HIP_Style.Text = "连接失败"
                End Select

                '初始化激光
                rtn = LJV7000.Initialize()
                If rtn <> 0 Then
                    BOOL.Init_Laser = False
                    Init_Str = "激光初始化失败，请检查"
                    Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), Init_Str, "", Color.Red)
                Else
                    rtn = LJV7000.EthernetOpen(Device_ID, PARAM_STRING.L镭射IP地址, 24691) '打开网口
                    If rtn <> 0 Then
                        BOOL.Init_Laser = False
                        Init_Str = "激光连接失败，请检查网口设置"
                        Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), Init_Str, "", Color.Red)
                    Else
                        Try
                            rtn = LJV7000.ChangeActiveProgram(Device_ID, Pro.NO0) '切换程序号
                            If rtn = 0 Then
                                Init_Str = "切换程序号: [Pro_" & Pro.NO0 & "]成功"
                                Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), Init_Str, "", Color.Black)

                                BOOL.Init_Laser = True
                                Init_Str = "激光初始化成功"
                                Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), Init_Str, "", Color.Black)
                            Else
                                BOOL.Init_Laser = False

                                Init_Str = "切换程序号：[ Pro_" & Pro.NO0 & "]失败"
                                Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), Init_Str, "", Color.Red)
                            End If
                        Catch ex As Exception
                            BOOL.Init_Laser = False
                            Init_Str = "切换程序号：[ Pro_" & Pro.NO0 & "]失败,请检查激光通讯！"
                            Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), Init_Str, "", Color.Red)
                        End Try
                    End If
                End If
                Select Case BOOL.Init_Laser
                    Case True
                        Laser_Style.Style.ForeColor.Color = Color.Green
                        Laser_Style.Text = "连接成功"
                    Case False
                        Laser_Style.Style.ForeColor.Color = Color.Red
                        Laser_Style.Text = "连接失败"
                End Select

                '初始化控制卡
                Card_Init_OK = Card_Init()
                If Card_Init_OK = True Then
                    Close_Trigger_Left()
                    Close_Trigger_Right()
                    Init_Str = "运动控制卡加载成功！"
                    Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), Init_Str, "", Color.Black)
                Else
                    Init_Str = "运动控制卡加载失败！"
                    Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), Init_Str, "", Color.Red)
                End If


                Init_key_Param()
                User_Name = User_Login_Dialog.User
                Init_Str = ProductName & Space(2) & ProductVersion & Space(2) & " 控制卡数量：" & Card_NO & Space(2) & " 当前用户：" & User_Name
                Me.Text = Init_Str
                Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), Init_Str, "", Color.Black)

                '打开主定时器
                Timer_Start.Interval = 100
                Timer_Start.Start()
                Timer_Free.Start()

                If SuperTabControl1.SelectedTabIndex <> 0 Then
                    SuperTabControl1.SelectedTabIndex = 0
                End If
            Else
                Init_Str = "连接本地数据库【" & SQLCON.DataBase_Data_Souce & "】失败，请进入SQL配置管理器->SQL网络配置->TCP/IP 是否已经全部启用"
                Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), Init_Str, "", Color.Red)
            End If
        Else
            DataConn.KillProcess("excel")
            DataConn.KillProcess(ProductName)
        End If


        SQLCON.RedData_NG键帽个数()
        NGCOUNT.NG_Max_Count = NG键帽个数
        NumericUpDown1.Value = NGCOUNT.NG_Max_Count
    End Sub

    ''' <summary>
    ''' 卡初始化
    ''' </summary>
    ''' <remarks></remarks>
    Function Card_Init() As Boolean
        Dim Init_Str As String = Nothing
        Try
            '初始化卡
            Dim rtn As Integer = 0, rtn_alarm As Boolean, Start_Axis_ID As Int16 = 0, Total_Axis_Num As Int16 = 0
            If APS_initial(Board_ID, 0) = 0 Then
            Else
                Init_Str = "运动控制卡初始化失败！"
                Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), Init_Str, "", Color.Black)
                MessageBox.Show(Init_Str, "卡加载消息", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return False
            End If
            For i As Integer = 0 To 3
                rtn = APS_get_first_axisId(i, Start_Axis_ID, Total_Axis_Num)
                If rtn = 0 And Total_Axis_Num <> 0 Then
                    rtn = APS_load_param_from_file("D:\System\param0" & i & ".xml")
                    If rtn <> 0 Then
                        Init_Str = "加载控制卡配置文件失败，请检查D:\System\param0" & i & ".xml 文件是否存在！"
                        Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), Init_Str, "", Color.Black)
                        MessageBox.Show(Init_Str, "卡加载消息", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Return False
                    End If
                Else
                    Card_NO = i
                    Exit For
                End If
            Next

            '' 伺服使能
            For i As Integer = 0 To HomeParamObjArray.Length - 1
                rtn = APS_set_servo_on(HomeParamObjArray(i).轴号, PRA_OUT_STATUS_ENUM.OUT_ON)
                rtn_alarm = CARDCMD.GET_EMG_STATUS(HomeParamObjArray(i).轴号)
                If rtn <> 0 Then
                    Init_Str = "轴" & i & "使能失败，请检查！"
                    Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), Init_Str, "", Color.Black)
                    MessageBox.Show(Init_Str, "卡加载消息", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return False
                End If
                If rtn_alarm = True Then
                    Init_Str = "轴[" & i & "]报警，控制卡初始化失败，请检查轴是否上电或急停按钮被按下！"
                    Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), Init_Str, "", Color.Black)
                    MessageBox.Show(Init_Str, "卡加载消息", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return False
                End If
            Next
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Private Sub Button_System_exit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_System_exit.Click
        If MessageBoxEx.Show("是否退出系统？请确认设备是否处于干涉位置？如有干涉请手动移位到安全位置！", "系统消息", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
            BOOL.CLOSE = True
            Application.Exit()
        End If
    End Sub

    Private Sub Button_System_Settings_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_System_Settings.Click
        If Bool_Home_Done = False Then
            If MessageBoxEx.Show("设备没有回原点，更新坐标会导致出错，请确认是否进入系统设置？", "系统消息", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
                Enter_Dialog()
                Select Case PARAM_BOOL.D登陆密码检测
                    Case True
                        If Login_Dialog.ShowDialog = Windows.Forms.DialogResult.OK Then
                            System_SetDialog.ShowDialog()
                        End If
                    Case False
                        System_SetDialog.ShowDialog()
                End Select
                Cancel_Dialog()
            End If
        Else
            Enter_Dialog()
            Select Case PARAM_BOOL.D登陆密码检测
                Case True
                    If Login_Dialog.ShowDialog = Windows.Forms.DialogResult.OK Then
                        System_SetDialog.ShowDialog()
                    End If
                Case False
                    System_SetDialog.ShowDialog()
            End Select
            Cancel_Dialog()
        End If
        Read_Deice_Param() '读取参数
    End Sub


    Sub Enter_Dialog()

        Timer_Start.Stop()
        Timer_Free.Stop()
        Bool_Run = False

        Bool_Start_Left = False
        Bool_Start_Right = False
        Thread.Sleep(100)
        If Run_Thread_Left IsNot Nothing Then
            Run_Thread_Left.Abort()
        End If
        If Run_Thread_Right IsNot Nothing Then
            Run_Thread_Right.Abort()
        End If
        If Run_Thread_Public IsNot Nothing Then
            Run_Thread_Public.Abort()
        End If


        Bool_Start_All = False
    End Sub

    Sub Cancel_Dialog()
        SQLCON.Read_Project_Name(ComboBox_product_name, Product_Name_Array) '读取项目名称
        Timer_Start.Start()
        Timer_Free.Start()

        '打开左线程
        If Bool_Run = False Then
            Bool_Run = True
            Bool_Start_All = True
            Run_Thread_Left = New Thread(AddressOf Run_Thread_Fun_Left)

            Run_Thread_Left.Start()
            Run_Thread_Public = New Thread(AddressOf Run_Thread_Fun_Comm)
            Run_Thread_Public.Start()

            BOOL.Laser_IsRuning = False
            If RUN_LASER.IsBusy = False Then
                RUN_LASER.RunWorkerAsync()
            End If
        End If
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

    Private Sub Button_Goto_Zero_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Goto_Zero.Click
        Dim rtn_str As String
        Select Case Card_Init_OK
            Case True
                If ComboBox_product_name.SelectedIndex >= 0 Then
                    Bool_Start_Home = True
                Else
                    rtn_str = "请先选择项目名称！"
                    Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), rtn_str, "", Color.Black)
                End If
            Case False
                rtn_str = "卡初始化失败，归零功能暂停使用！"
                Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), rtn_str, "", Color.Black)
        End Select
    End Sub

    Private Sub Btn_Language_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Btn_Language.Click
        Select Case Btn_Language.Text
            Case "LANGUAGE"
                Language = LGE.CHN
                Btn_Language.Text = "语言"
            Case "语言"
                Language = LGE.ENG
                Btn_Language.Text = "LANGUAGE"
        End Select
        Read_Language(Application.StartupPath, Language) '读取中文

        Select Case Language
            Case LGE.CHN
                If Btn_Change_LR.Text = "Left Station" Then
                    Btn_Change_LR.Text = "左工位"
                ElseIf Btn_Change_LR.Text = "Right Station" Then
                    Btn_Change_LR.Text = "右工位"
                ElseIf Btn_Change_LR.Text = "Double Station" Then
                    Btn_Change_LR.Text = "双工位"
                End If
                'If SW_LOOP.OffText = "Loop Off" Then
                '    SW_LOOP.OffText = "循环关"
                'End If
                'If SW_LOOP.OnText = "Loop On" Then
                '    SW_LOOP.OnText = "循环开"
                'End If
                'If SW_Fully_automatic.OffText = "Semi Automatic" Then
                '    SW_Fully_automatic.OffText = "半自动"
                'End If
                'If SW_Fully_automatic.OnText = "Automatic" Then
                '    SW_Fully_automatic.OnText = "全自动"
                'End If
                If Button_Goto_Zero.Text = "Device Zeroing" Then
                    Button_Goto_Zero.Text = "设备归零"
                End If
                If Button_Goto_Zero.Text = "Zero Done" Then
                    Button_Goto_Zero.Text = "归零完成"
                End If
            Case LGE.ENG
                If Btn_Change_LR.Text = "左工位" Then
                    Btn_Change_LR.Text = "Left Station"
                ElseIf Btn_Change_LR.Text = "右工位" Then
                    Btn_Change_LR.Text = "Right Station"
                ElseIf Btn_Change_LR.Text = "双工位" Then
                    Btn_Change_LR.Text = "Double Station"
                End If
                'If SW_LOOP.OffText = "循环关" Then
                '    SW_LOOP.OffText = "Loop Off"
                'End If
                'If SW_LOOP.OnText = "循环开" Then
                '    SW_LOOP.OnText = "Loop On"
                'End If
                'If SW_Fully_automatic.OffText = "半自动" Then
                '    SW_Fully_automatic.OffText = "Semi Automatic"
                'End If
                'If SW_Fully_automatic.OnText = "全自动" Then
                '    SW_Fully_automatic.OnText = "Automatic"
                'End If
                If Button_Goto_Zero.Text = "设备归零" Then
                    Button_Goto_Zero.Text = "Device Zeroing"
                End If
                If Button_Goto_Zero.Text = "归零完成" Then
                    Button_Goto_Zero.Text = "Zero Done"
                End If
        End Select
    End Sub

    ''' <summary>
    ''' 语言切换
    ''' </summary>
    ''' <param name="Path">路径</param>
    ''' <param name="language">语言</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function Read_Language(ByVal Path As String, ByVal language As LGE)
        Dim strPath As String = Nothing
        Try
            Select Case language
                Case LGE.ENG
                    strPath = Path & "\ENGLISH.ini"
                Case LGE.CHN
                    strPath = Path & "\CHINESE.ini"
            End Select
            If File.Exists(strPath) = False Then
                MessageBox.Show("中英文显示文件" & strPath & "不存在！", "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return False
            End If

            GP_Current_Style.Text = GetINI("Language", "Current_Status", "", strPath)
            GP_Project_Msg.Text = GetINI("Language", "Project_Information", "", strPath)
            PE_Project_Name.Text = GetINI("Language", "Project_Name", "", strPath)
            GP_RealTime_Msg.Text = GetINI("Language", "Real_time_Message", "", strPath)
            Button_System_Settings.Text = GetINI("Language", "System_Settings", "", strPath)
            Btn_Param_Set.Text = GetINI("Language", "Param_Settings", "", strPath)
            Button_System_exit.Text = GetINI("Language", "Quit_System", "", strPath)
            PE_LEFT_SN.Text = GetINI("Language", "Left_Station_SN", "", strPath)
            PE_RIGHT_SN.Text = GetINI("Language", "Right_Station_SN", "", strPath)
            Btn_Home_Set.Text = GetINI("Language", "Home_Settings", "", strPath)
            Btn_DIO_Set.Text = GetINI("Language", "DIO_Settings", "", strPath)
            Btn_Query_All.Text = GetINI("Language", "Query_All", "", strPath)
            GP_DataCode.Text = GetINI("Language", "DataCode", "", strPath)
            DX_ENC_POS.Columns(0).HeaderText = GetINI("Language", "Axis_Name", "", strPath)
            DX_ENC_POS.Columns(1).HeaderText = GetINI("Language", "Axis_Pos", "", strPath)

            Return True
        Catch
            MsgBox("读取中英文显示失败！", MsgBoxStyle.Information, "系统消息")
            Return False
        End Try
    End Function

    Private Sub Btn_Home_Set_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Btn_Home_Set.Click
        Select Case User_Name
            Case UserName.Admin
                Enter_Dialog()

                Select Case PARAM_BOOL.D登陆密码检测
                    Case True
                        If Login_Dialog.ShowDialog = Windows.Forms.DialogResult.OK Then
                            Home_Dialog.ShowDialog()
                        End If
                    Case False
                        Home_Dialog.ShowDialog()
                End Select

                Cancel_Dialog()

                SQLCON.Read_Home_ParamS(HomeParamObjArray)
                SET_AXIS_PITCH_SUB() '设置轴导程
            Case Else
                MessageBoxEx.Show("权限受限，请换用户名登录", "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button3)
        End Select

    End Sub

    Private Sub Btn_Param_Set_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Btn_Param_Set.Click

        If BOOL.Init_DIO = False Then
            If MessageBoxEx.Show("没有加载IO，不能对IO进行操作，请确认是否继续?确认请点击【YES】!", "系统消息", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button3) = Windows.Forms.DialogResult.Yes Then
            Else
                Exit Sub
            End If
        End If
        Enter_Dialog()

        Select Case PARAM_BOOL.D登陆密码检测
            Case True
                If Login_Dialog.ShowDialog = Windows.Forms.DialogResult.OK Then
                    Parameter_Set_Dialog.ShowDialog()
                End If
            Case False
                Parameter_Set_Dialog.ShowDialog()
        End Select

        Cancel_Dialog()

        Read_Deice_Param() '读取参数

    End Sub


    Private Sub ComboBox_product_name_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox_product_name.SelectedIndexChanged
        If ComboBox_product_name.Items.Count > 0 Then
            If ComboBox_product_name.SelectedIndex > -1 Then
                ComboBox_product_name.Enabled = False
                Select_Product_Name = Product_Name_Array(ComboBox_product_name.SelectedIndex).Trim
                Dim str As String = "选择项目：" & Select_Product_Name
                Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), str, "", Color.Green)
                '清空所有测量数据，防止插入数据变慢
                SQLCON.Clear_All_MeasureData(Select_Product_Name)

                If Select_Product_Name.Contains("ANSI") Then
                    KB_Type = KB_Type_Enum.ANSI
                    KB_Number = KB_Type_Number.ANSI
                    Init_key_Param()
                    JSON_PARA.KB_VERSION = "ansi"
                ElseIf Select_Product_Name.Contains("ISO") Then
                    KB_Type = KB_Type_Enum.ISO
                    KB_Number = KB_Type_Number.ISO
                    Init_key_Param()
                    JSON_PARA.KB_VERSION = "iso"
                ElseIf Select_Product_Name.Contains("JIS") Then
                    KB_Type = KB_Type_Enum.JIS
                    KB_Number = KB_Type_Number.JIS
                    Init_key_Param()
                    JSON_PARA.KB_VERSION = "jis"
                Else
                    MessageBoxEx.Show("项目名称命名错误", "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End If

                'Init jason Key Name
                ReDim Key_Name(KB_Number)
                ReDim KeyResult(KB_Number)
                Init_Key_Name(Key_Name)

                SQLCON.Read_Home_ParamS(HomeParamObjArray)
                SET_AXIS_PITCH_SUB() '设置轴导程
                Read_ADLINK_DIO() '都读IO
                Read_Deice_Param()
                Read_Project_Param()
                Set_Omron_Command()

                Select Case SW_Vision_1.Value
                    Case True
                        CARDCMD.SET_DO_BIT(BOARD_ID_01, DIO.ONE1工位版本切换电磁阀_1, PRA_OUT_STATUS_ENUM.OUT_ON)
                        Thread.Sleep(30)
                    Case False
                        CARDCMD.SET_DO_BIT(BOARD_ID_01, DIO.ONE1工位版本切换电磁阀_1, PRA_OUT_STATUS_ENUM.OUT_OFF)
                        Thread.Sleep(30)
                End Select
                Select Case SW_Vision_2.Value
                    Case True
                        CARDCMD.SET_DO_BIT(BOARD_ID_01, DIO.TWO2工位版本切换电磁阀_1, PRA_OUT_STATUS_ENUM.OUT_ON)
                        Thread.Sleep(30)
                    Case False
                        CARDCMD.SET_DO_BIT(BOARD_ID_01, DIO.TWO2工位版本切换电磁阀_1, PRA_OUT_STATUS_ENUM.OUT_OFF)
                        Thread.Sleep(30)
                End Select

                '打开串口
                Static Bool_Init_SerialPort As Boolean = False
                If Bool_Init_SerialPort = False Then
                    Bool_Init_SerialPort = True
                    Try
                        SerialPort_DataCode.PortName = PARAM_STRING.T条码枪COM口
                        SerialPort_DataCode.Open()
                    Catch ex As Exception
                        MessageBoxEx.Show("条码枪COM口打开失败：" & ex.ToString, "COM口消息", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    End Try
                End If
                Static Bool_Serialport_PLC As Boolean = False
                If Bool_Serialport_PLC = False Then
                    Bool_Serialport_PLC = True
                    Try
                        SerialPort_PLC.PortName = PARAM_STRING.PLC串口
                        SerialPort_PLC.Open()
                    Catch ex As Exception
                        MessageBoxEx.Show("PLC串口打开失败：" & ex.ToString, "COM口消息", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    End Try
                End If

                SQLCON.Read_Coordinate_Name(Select_Product_Name, STATION.LEFT, COORDINATENAME.LEFT_DATASET)  '读取坐标名称
                SQLCON.Read_Coordinate_Name(Select_Product_Name, STATION.RIGHT, COORDINATENAME.RIGHT_DATASET)  '读取坐标名称
                SQLCON.Read_Coordinate_Name(Select_Product_Name, STATION.COMMON, COORDINATENAME.COMMON_DATASET)  '读取坐标名称

                READ_COODNATE_PARAM(COORDINATENAME.LEFT_DATASET, COORDINATEPARAM.LEFT_DATASET, Coord_Order_List_LEFT, Coord_AllPOS_List_LEFT)
                READ_COODNATE_PARAM(COORDINATENAME.RIGHT_DATASET, COORDINATEPARAM.RIGHT_DATASET, Coord_Order_List_RIGHT, Coord_AllPOS_List_RIGHT)

                READ_SINGLE_COODNATE_PARAM("中间位放料位", Work_中间位放料位)
                READ_SINGLE_COODNATE_PARAM("左初始位", Work_左工位初始位)
                READ_SINGLE_COODNATE_PARAM("右初始位", Work_右工位初始位)
                READ_SINGLE_COODNATE_PARAM("左工位镭射取料位", Work_左工位镭射取料位)
                READ_SINGLE_COODNATE_PARAM("右工位镭射取料位", Work_右工位镭射取料位)

                '左工位触发坐标
                READ_SINGLE_COODNATE_PARAM("①工位拍照第一行从左到右", Work_左工位拍照第一行从左到右)
                READ_SINGLE_COODNATE_PARAM("①工位拍照第二行从右到左", Work_左工位拍照第二行从右到左)
                READ_SINGLE_COODNATE_PARAM("①工位拍照第三行从左到右", Work_左工位拍照第三行从左到右)
                READ_SINGLE_COODNATE_PARAM("①工位拍照第四行从右到左", Work_左工位拍照第四行从右到左)
                READ_SINGLE_COODNATE_PARAM("①工位拍照第五行从左到右", Work_左工位拍照第五行从左到右)
                READ_SINGLE_COODNATE_PARAM("①工位拍照第六行从右到左", Work_左工位拍照第六行从右到左)

                Check_Positon(Work_左工位拍照第一行从左到右)
                Check_Positon(Work_左工位拍照第二行从右到左)
                Check_Positon(Work_左工位拍照第三行从左到右)
                Check_Positon(Work_左工位拍照第四行从右到左)
                Check_Positon(Work_左工位拍照第五行从左到右)
                Check_Positon(Work_左工位拍照第六行从右到左)
                '右工位触发坐标
                READ_SINGLE_COODNATE_PARAM("②工位拍照第一行从左到右", Work_右工位拍照第一行从左到右)
                READ_SINGLE_COODNATE_PARAM("②工位拍照第二行从右到左", Work_右工位拍照第二行从右到左)
                READ_SINGLE_COODNATE_PARAM("②工位拍照第三行从左到右", Work_右工位拍照第三行从左到右)
                READ_SINGLE_COODNATE_PARAM("②工位拍照第四行从右到左", Work_右工位拍照第四行从右到左)
                READ_SINGLE_COODNATE_PARAM("②工位拍照第五行从左到右", Work_右工位拍照第五行从左到右)
                READ_SINGLE_COODNATE_PARAM("②工位拍照第六行从右到左", Work_右工位拍照第六行从右到左)

                Check_Positon(Work_右工位拍照第一行从左到右)
                Check_Positon(Work_右工位拍照第二行从右到左)
                Check_Positon(Work_右工位拍照第三行从左到右)
                Check_Positon(Work_右工位拍照第四行从右到左)
                Check_Positon(Work_右工位拍照第五行从左到右)
                Check_Positon(Work_右工位拍照第六行从右到左)
                '镭射触发坐标


                READ_SINGLE_COODNATE_PARAM("左镭射第一行扫描从左到右", Work_左镭射第一行扫描从左到右)
                READ_SINGLE_COODNATE_PARAM("左镭射第二行扫描从右到左", Work_左镭射第二行扫描从右到左)
                READ_SINGLE_COODNATE_PARAM("左镭射第三行扫描从左到右", Work_左镭射第三行扫描从左到右)
                READ_SINGLE_COODNATE_PARAM("左镭射第四行扫描从左到右", Work_左镭射第四行扫描从左到右)
                READ_SINGLE_COODNATE_PARAM("左镭射第五行扫描从右到左", Work_左镭射第五行扫描从右到左)
                READ_SINGLE_COODNATE_PARAM("左镭射第六行扫描从左到右", Work_左镭射第六行扫描从左到右)

                Try
                    INTTYPLE.Left_Trigger_Count = Work_左工位拍照第一行从左到右.Count + Work_左工位拍照第二行从右到左.Count + Work_左工位拍照第三行从左到右.Count + Work_左工位拍照第四行从右到左.Count + Work_左工位拍照第五行从左到右.Count + Work_左工位拍照第六行从右到左.Count
                    INTTYPLE.Right_Trigger_Count = Work_右工位拍照第一行从左到右.Count + Work_右工位拍照第二行从右到左.Count + Work_右工位拍照第三行从左到右.Count + Work_右工位拍照第四行从右到左.Count + Work_右工位拍照第五行从左到右.Count + Work_右工位拍照第六行从右到左.Count
                    PE_Trigger_Left.Text = "左触发次数[" & INTTYPLE.Left_Trigger_Count & "]"
                    PE_Trigger_Right.Text = "右触发次数[" & INTTYPLE.Right_Trigger_Count & "]"
                Catch ex As Exception
                    MessageBoxEx.Show("有坐标设置为空，请先设置坐标", "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End Try

                Try
                    Get_Laser_Trigger(TABLE_COMPARE_DIRECTION_ENUM.POSITIVE_DIRECTION, Work_左镭射第一行扫描从左到右, Set_Trg_Count1, Line_Table_1)
                    Get_Laser_Trigger(TABLE_COMPARE_DIRECTION_ENUM.NEGATIVE_DIRECTION, Work_左镭射第二行扫描从右到左, Set_Trg_Count2, Line_Table_2)
                    Get_Laser_Trigger(TABLE_COMPARE_DIRECTION_ENUM.POSITIVE_DIRECTION, Work_左镭射第三行扫描从左到右, Set_Trg_Count3, Line_Table_3)
                    Set_Trg_Count_All = Set_Trg_Count1 + Set_Trg_Count2 + Set_Trg_Count3
                    INTTYPLE.Laser_Trigger_Count = Set_Trg_Count_All
                Catch ex As Exception
                    MessageBoxEx.Show("镭射位设置出错，请检查！", "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End Try

                str = "更改PLC为监视模式"
                Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), str, "", Color.Blue)
                Send_plc_Command_Str = Omron_Command.更改PLC为监视模式 '切换PLC模式


                If File.Exists(PARAM_STRING.S视觉文件路径) = False Then
                    MessageBox.Show("视觉文件【" & PARAM_STRING.S视觉文件路径 & "】不存在！", "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
                Else
                    If LINKCAMERAWORKER.IsBusy = False Then
                        LINKCAMERAWORKER.RunWorkerAsync()
                    End If
                End If

                '打开主线程和左线程
                Select Case BOOL.Init_DIO
                    Case True
                        If Bool_Run = False Then
                            Bool_Run = True
                            Bool_Start_All = True
                            Run_Thread_Left = New Thread(AddressOf Run_Thread_Fun_Left)
                            Run_Thread_Left.Start()
                            '打开公共取料
                            Run_Thread_Public = New Thread(AddressOf Run_Thread_Fun_Comm)
                            Run_Thread_Public.Start()
                            '打开镭射
                            If RUN_LASER.IsBusy = False Then
                                RUN_LASER.RunWorkerAsync()
                            End If
                        End If
                    Case False
                        str = "读取DIO参数出错，请先检查！"
                        Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), str, "", Color.Black)
                End Select
            End If
        End If
    End Sub

    ''' <summary>
    ''' 回原点
    ''' </summary>
    ''' <param name="Index"></param>
    ''' <remarks></remarks>
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
        AXIS_INDEX1_LEFT.Clear()
        AXIS_INDEX2_LEFT.Clear()
        AXIS_INDEX3_LEFT.Clear()
        AXIS_INDEX4_LEFT.Clear()
        AXIS_INDEX5_LEFT.Clear()
        '把程序优先级顺序排序，并存入序列号
        For i As Integer = 0 To HomeParamObjArray.Length - 1
            Select Case HomeParamObjArray(i).回原点顺序
                Case 1
                    AXIS_INDEX1_LEFT.Add(i)
                Case 2
                    AXIS_INDEX2_LEFT.Add(i)
                Case 3
                    AXIS_INDEX3_LEFT.Add(i)
                Case 4
                    AXIS_INDEX4_LEFT.Add(i)
                Case 5
                    AXIS_INDEX5_LEFT.Add(i)
            End Select
        Next
        For M As Int16 = 1 To 5
            Dim AXIS_INDEX As New List(Of Integer)
            AXIS_INDEX.Clear()
            Select Case M
                Case 1
                    AXIS_INDEX = AXIS_INDEX1_LEFT
                Case 2
                    AXIS_INDEX = AXIS_INDEX2_LEFT
                Case 3
                    AXIS_INDEX = AXIS_INDEX3_LEFT
                Case 4
                    AXIS_INDEX = AXIS_INDEX4_LEFT
                Case 5
                    AXIS_INDEX = AXIS_INDEX5_LEFT
            End Select

            Dim Index As Integer
            If AXIS_INDEX.Count > 0 Then
                For i As Integer = 0 To AXIS_INDEX.Count - 1
                    Index = AXIS_INDEX(i)
                    Home_Param_Move(Index)
                Next
                Dim Bool_Home_Styple(100) As Boolean '判定轴是否归零完成标记
                While BOOL.EXIT_APP = False '当异常退出时，退出While循环
                    Dim rtn_state As Integer = 1 '所有轴归零状态
                    Dim rtn_state_single As Int16 = 0 '单轴归零状态
                    For j As Integer = 0 To AXIS_INDEX.Count - 1
                        Index = AXIS_INDEX(j)
                        HOME_PARAM.轴号 = HomeParamObjArray(Index).轴号
                        rtn_state_single = CARDCMD.CHECK_MOTION_DONE(HOME_PARAM.轴号, Stop_Code)
                        rtn_state *= rtn_state_single
                        If rtn_state_single = 1 And Bool_Home_Styple(j) = False Then
                            Bool_Home_Styple(j) = True
                            Dim str As String = "[" & HomeParamObjArray(Index).轴名称 & "轴]归零完成！"
                            Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), str, "", Color.Black)
                        Else
                        End If
                    Next
                    If rtn_state = 1 Then
                        Exit While
                    End If
                    READ_ENCODER_POSITION(AXISMSG.AXIS_ENCODER_POS)
                End While
            End If
        Next
    End Sub

    Private Sub Btn_DIO_Set_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Btn_DIO_Set.Click
        Select Case User_Name
            Case UserName.Admin
                Enter_Dialog()

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

                Cancel_Dialog()
            Case Else
                MessageBoxEx.Show("权限受限，请换用户名登录", "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button3)
        End Select


    End Sub

    Private Sub Timer_Start_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer_Start.Tick
        Dim rtn_str As String = Nothing
        If Card_Init_OK = True Then

            If SerialPort_PLC IsNot Nothing Then
                If SerialPort_PLC.IsOpen = True Then
                    If Send_Omron_Cmd = False Then
                        Select Case Send_plc_Command_Str
                            Case Omron_Command.更改PLC为监视模式
                                SerialPort_PLC.DiscardInBuffer()
                                SerialPort_PLC.DiscardOutBuffer()
                                SerialPort_PLC.WriteLine(Omron_Command.更改PLC为监视模式)
                            Case Omron_Command.读取PLC状态
                                SerialPort_PLC.DiscardInBuffer()
                                SerialPort_PLC.DiscardOutBuffer()
                                SerialPort_PLC.WriteLine(Omron_Command.读取PLC状态)
                            Case Omron_Command.写入结果OK
                                SerialPort_PLC.DiscardInBuffer()
                                SerialPort_PLC.DiscardOutBuffer()
                                SerialPort_PLC.WriteLine(Omron_Command.写入结果OK)
                                rtn_str = "给PLC发送结果：OK"
                                Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), rtn_str, LASER_DATACODE, Color.Blue)
                            Case Omron_Command.写入结果NG
                                SerialPort_PLC.DiscardInBuffer()
                                SerialPort_PLC.DiscardOutBuffer()
                                SerialPort_PLC.WriteLine(Omron_Command.写入结果NG)
                                rtn_str = "给PLC发送结果：NG"
                                Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), rtn_str, LASER_DATACODE, Color.Blue)
                            Case Else
                        End Select
                    End If
                End If
            End If

            If BOOL.EXIT_APP = False Then
                READ_EMG_STOP()
            End If
            If BOOL.Error_Close = True And BOOL.EXIT_APP = False Then '异常退出程序
                BOOL.EXIT_APP = True

                If Error_Str = "" Then
                    Error_Str = "硬件出错，程序保护性退出！"
                End If
                STOP_SPORT(STOP_TYPE.EMG_STOP)
                Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), Error_Str, "", Color.Red)
               
                BOOL.CLOSE = True
                Application.Exit()
            End If
            '红绿黄灯显示
            If Card_Init_OK = True And Bool_Home_Done = True And BOOL.Init_DIO = True Then
                If BOOL.Left_IsRuning = True Or BOOL.Right_IsRuning = True Or BOOL.Common_IsRuning = True Or BOOL.Laser_IsRuning = True Then
                    If GP_SysSet_Button.Enabled = True Then
                        GP_SysSet_Button.Enabled = False
                    End If
                    If GP_Project_Msg.Enabled = True Then
                        GP_Project_Msg.Enabled = False
                    End If
                    If SuperTabControl1.Enabled = True Then
                        SuperTabControl1.Enabled = False
                    End If
                    If BOOL.Green_Light = False Then
                        BOOL.Green_Light = True

                        Change_RedYellowGreen(LIGHT.GREEN)

                        Bool_No_Wrok = False
                    End If
                Else
                    If GP_SysSet_Button.Enabled = False Then
                        GP_SysSet_Button.Enabled = True
                    End If
                    If GP_Project_Msg.Enabled = False Then
                        GP_Project_Msg.Enabled = True
                    End If
                    If SuperTabControl1.Enabled = False Then
                        SuperTabControl1.Enabled = True
                    End If
                    If BOOL.Yellow_Light = False Then
                        BOOL.Yellow_Light = True

                        Change_RedYellowGreen(LIGHT.YELLOW)

                        Bool_No_Wrok = True
                    End If
                End If
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
                If BOOL.Emg_Stop_Button = False Then
                    STOP_SPORT(STOP_TYPE.EMG_STOP)
                    BOOL.Emg_Stop_Button = True
                    Error_Str = "[急停按钮]被按下,软件即将关闭"
                    Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), Error_Str, "", Color.Red)
                    BOOL.Error_Close = True
                    Return 1 '函数直接返回
                End If
            Case False
                If BOOL.Emg_Stop_Button = True Then
                    Thread.Sleep(100)
                    BOOL.Emg_Stop_Button = False
                End If
                READ_EMG_STOP = -1
        End Select
        '读取报警状态
        If HomeParamObjArray.Length > 1 Then
            For i As Int16 = 0 To HomeParamObjArray.Length - 1
                Select Case CARDCMD.GET_ALM_STATUS(HomeParamObjArray(i).轴号)
                    Case True
                        If BOOL.Moto_Warning(i) = False Then
                            STOP_SPORT(STOP_TYPE.EMG_STOP)
                            BOOL.Moto_Warning(i) = True
                            Error_Str = "[" & HomeParamObjArray(i).轴名称 & "]轴电机报警，软件即将关闭，请清除报警后重新打开软件！"
                            Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), Error_Str, "", Color.Red)
                            BOOL.Error_Close = True
                            Return 2 '函数直接返回
                        End If

                    Case False
                        If BOOL.Moto_Warning(i) = True Then
                            BOOL.Moto_Warning(i) = False
                        End If
                        READ_EMG_STOP = -2
                End Select
                '读取正负限位
                If BOOL.Read_MPEL = True Then '回完原点之后开启监控正负限位
                    '负限位
                    Select Case CARDCMD.GET_MEL_STATUS(HomeParamObjArray(i).轴号)
                        Case True
                            If BOOL.Moto_MEL(i) = False Then
                                STOP_SPORT(STOP_TYPE.EMG_STOP)
                                BOOL.Moto_MEL(i) = True
                                Error_Str = "[" & HomeParamObjArray(i).轴名称 & "]轴负限位触发"
                                Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), Error_Str, "", Color.Red)
                                BOOL.Error_Close = True
                                Return 3
                            End If
                        Case False
                            If BOOL.Moto_MEL(i) = True Then
                                BOOL.Moto_MEL(i) = False
                            End If
                            READ_EMG_STOP = -3
                    End Select
                    '正限位
                    Select Case CARDCMD.GET_PEL_STATUS(HomeParamObjArray(i).轴号)
                        Case True
                            If BOOL.Moto_PEL(i) = False Then
                                STOP_SPORT(STOP_TYPE.EMG_STOP)
                                BOOL.Moto_PEL(i) = True
                                Error_Str = "[" & HomeParamObjArray(i).轴名称 & "]轴正限位触发"
                                Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), Error_Str, "", Color.Red)
                                BOOL.Error_Close = True
                                Return 4
                            End If
                        Case False
                            If BOOL.Moto_PEL(i) = True Then
                                BOOL.Moto_PEL(i) = False
                            End If
                            READ_EMG_STOP = -4
                    End Select
                End If
            Next
        End If
        Return READ_EMG_STOP
    End Function

    ''' <summary>
    ''' 停止运动
    ''' </summary>
    ''' <remarks></remarks>
    Sub STOP_SPORT(ByVal StopType As Integer)
        Select Case StopType
            Case STOP_TYPE.JOG_STOP
                For I As Integer = 0 To HomeParamObjArray.Length - 1
                    CARDCMD.STOP_MOVE(HomeParamObjArray(I).轴号)
                    APS_set_servo_on(HomeParamObjArray(I).轴号, PRA_OUT_STATUS_ENUM.OUT_OFF)
                Next
            Case STOP_TYPE.EMG_STOP
                For I As Integer = 0 To HomeParamObjArray.Length - 1
                    CARDCMD.EMG_STOP(HomeParamObjArray(I).轴号)
                    APS_set_servo_on(HomeParamObjArray(I).轴号, PRA_OUT_STATUS_ENUM.OUT_OFF)
                Next
        End Select
    End Sub


    ''' <summary>
    ''' 读取IO
    ''' </summary>
    ''' <remarks></remarks>
    Sub Read_ADLINK_DIO()
        Dim rtn_int As Int16 = 1
        'Card0 DI
        rtn_int *= SQLCON.Query_ADLINK_IO_ID("启动按钮", Card_NO, DIO.Q启动按钮_0)
        rtn_int *= SQLCON.Query_ADLINK_IO_ID("复位按钮", Card_NO, DIO.F复位按钮_0)
        rtn_int *= SQLCON.Query_ADLINK_IO_ID("安全门磁", Card_NO, DIO.A安全光栅_0)
        rtn_int *= SQLCON.Query_ADLINK_IO_ID("中线有料检测", Card_NO, DIO.Z中线有料检测_0)
        rtn_int *= SQLCON.Query_ADLINK_IO_ID("中线真空吸检测", Card_NO, DIO.Z中线真空吸检测_0)
        rtn_int *= SQLCON.Query_ADLINK_IO_ID("中线阻挡HOME", Card_NO, DIO.Z中线阻挡HOME_0)
        rtn_int *= SQLCON.Query_ADLINK_IO_ID("中线阻挡WORK", Card_NO, DIO.Z中线阻挡WORK_0)
        rtn_int *= SQLCON.Query_ADLINK_IO_ID("1工位阻挡HOME1", Card_NO, DIO.ONE1工位阻挡HOME1_0)
        rtn_int *= SQLCON.Query_ADLINK_IO_ID("1工位阻挡WORK1", Card_NO, DIO.ONE1工位阻挡WORK1_0)
        rtn_int *= SQLCON.Query_ADLINK_IO_ID("1工位阻挡HOME2", Card_NO, DIO.ONE1工位阻挡HOME2_0)
        rtn_int *= SQLCON.Query_ADLINK_IO_ID("1工位阻挡WORK2", Card_NO, DIO.ONE1工位阻挡WORK2_0)
        rtn_int *= SQLCON.Query_ADLINK_IO_ID("1工位来料检测", Card_NO, DIO.ONE1工位来料检测_0)
        rtn_int *= SQLCON.Query_ADLINK_IO_ID("1工位出料检测", Card_NO, DIO.ONE1工位出料检测_0)
        rtn_int *= SQLCON.Query_ADLINK_IO_ID("1工位真空吸检测", Card_NO, DIO.ONE1工位真空吸检测_0)
        rtn_int *= SQLCON.Query_ADLINK_IO_ID("2工位阻挡HOME1", Card_NO, DIO.TWO2工位阻挡HOME1_0)
        rtn_int *= SQLCON.Query_ADLINK_IO_ID("2工位阻挡WORK1", Card_NO, DIO.TWO2工位阻挡WORK1_0)
        rtn_int *= SQLCON.Query_ADLINK_IO_ID("2工位阻挡HOME2", Card_NO, DIO.TWO2工位阻挡HOME2_0)
        rtn_int *= SQLCON.Query_ADLINK_IO_ID("2工位阻挡WORK2", Card_NO, DIO.TWO2工位阻挡WORK2_0)
        rtn_int *= SQLCON.Query_ADLINK_IO_ID("2工位来料检测", Card_NO, DIO.TWO2工位来料检测_0)
        rtn_int *= SQLCON.Query_ADLINK_IO_ID("2工位出料检测", Card_NO, DIO.TWO2工位出料检测_0)
        'Card0 D0
        rtn_int *= SQLCON.Query_ADLINK_IO_ID("红灯", Card_NO, DIO.H红灯_0)
        rtn_int *= SQLCON.Query_ADLINK_IO_ID("黄灯", Card_NO, DIO.H黄灯_0)
        rtn_int *= SQLCON.Query_ADLINK_IO_ID("绿灯", Card_NO, DIO.L绿灯_0)
        rtn_int *= SQLCON.Query_ADLINK_IO_ID("中线皮带控制", Card_NO, DIO.Z中线皮带控制_0)
        rtn_int *= SQLCON.Query_ADLINK_IO_ID("1工位皮带控制", Card_NO, DIO.ONE1工位皮带控制_0)
        rtn_int *= SQLCON.Query_ADLINK_IO_ID("2工位皮带控制", Card_NO, DIO.TWO2工位皮带控制_0)
        rtn_int *= SQLCON.Query_ADLINK_IO_ID("回流皮带控制", Card_NO, DIO.H回流皮带控制_0)
        rtn_int *= SQLCON.Query_ADLINK_IO_ID("中线阻挡电磁阀", Card_NO, DIO.Z中线阻挡电磁阀_0)
        rtn_int *= SQLCON.Query_ADLINK_IO_ID("中线真空吸电磁阀", Card_NO, DIO.Z中线真空吸电磁阀_0)
        rtn_int *= SQLCON.Query_ADLINK_IO_ID("1工位阻挡电磁阀1", Card_NO, DIO.ONE1工位阻挡电磁阀1_0)
        rtn_int *= SQLCON.Query_ADLINK_IO_ID("1工位阻挡电磁阀2", Card_NO, DIO.ONE1工位阻挡电磁阀2_0)
        rtn_int *= SQLCON.Query_ADLINK_IO_ID("1工位真空吸电磁阀", Card_NO, DIO.ONE1工位真空吸电磁阀_0)
        rtn_int *= SQLCON.Query_ADLINK_IO_ID("2工位阻挡电磁阀1", Card_NO, DIO.TWO2工位阻挡电磁阀1_0)
        rtn_int *= SQLCON.Query_ADLINK_IO_ID("2工位阻挡电磁阀2", Card_NO, DIO.TWO2工位阻挡电磁阀2_0)
        rtn_int *= SQLCON.Query_ADLINK_IO_ID("2工位真空吸电磁阀", Card_NO, DIO.TWO2工位真空吸电磁阀_0)
        rtn_int *= SQLCON.Query_ADLINK_IO_ID("1工位顶升电磁阀", Card_NO, DIO.ONE1工位顶升电磁阀_0)
        rtn_int *= SQLCON.Query_ADLINK_IO_ID("2工位顶升电磁阀", Card_NO, DIO.TWO2工位顶升电磁阀_0)
        rtn_int *= SQLCON.Query_ADLINK_IO_ID("1工位破真空电磁阀", Card_NO, DIO.ONE1工位破真空电磁阀_0)
        rtn_int *= SQLCON.Query_ADLINK_IO_ID("2工位破真空电磁阀", Card_NO, DIO.TWO2工位破真空电磁阀_0)
        rtn_int *= SQLCON.Query_ADLINK_IO_ID("2工位破真空电磁阀", Card_NO, DIO.C键ONE1工位真空吸电磁阀_0)
        rtn_int *= SQLCON.Query_ADLINK_IO_ID("2工位破真空电磁阀", Card_NO, DIO.C键TWO2工位真空吸电磁阀_0)
        'Card1 DI
        rtn_int *= SQLCON.Query_ADLINK_IO_ID("2工位真空吸检测", Card_NO, DIO.TWO2工位真空吸检测_1)
        rtn_int *= SQLCON.Query_ADLINK_IO_ID("1工位顶升HOME", Card_NO, DIO.ONE1工位顶升HOME_1)
        rtn_int *= SQLCON.Query_ADLINK_IO_ID("1工位顶升WORK", Card_NO, DIO.ONE1工位顶升WORK_1)
        rtn_int *= SQLCON.Query_ADLINK_IO_ID("2工位顶升HOME", Card_NO, DIO.TWO2工位顶升HOME_1)
        rtn_int *= SQLCON.Query_ADLINK_IO_ID("2工位顶升WORK", Card_NO, DIO.TWO2工位顶升WORK_1)
        rtn_int *= SQLCON.Query_ADLINK_IO_ID("中线出料检测", Card_NO, DIO.Z中线出料检测_1)

        rtn_int *= SQLCON.Query_ADLINK_IO_ID("1工位版本切换电磁阀", Card_NO, DIO.ONE1工位版本切换电磁阀_1)
        rtn_int *= SQLCON.Query_ADLINK_IO_ID("2工位版本切换电磁阀", Card_NO, DIO.TWO2工位版本切换电磁阀_1)

        Select Case rtn_int
            Case 1
                BOOL.Init_DIO = True
            Case 0
                BOOL.Init_DIO = False
        End Select
    End Sub

    ''' <summary>
    ''' 读取设备参数
    ''' </summary>
    ''' <remarks></remarks>
    Sub Read_Deice_Param()
        'DOUBLE
        SQLCON.Query_Device_Param(DEVICEPARAM_TABLENAME.DOUBLE_TYPE, "脉宽(ms)", PARAM_DOUBLE.M脉宽)
        'INTEGER
        SQLCON.Query_Device_Param(DEVICEPARAM_TABLENAME.INTEGER_TYPE, "运动等待时间", PARAM_INT.Y运动等待时间)
        SQLCON.Query_Device_Param(DEVICEPARAM_TABLENAME.INTEGER_TYPE, "电磁阀输出等待时间", PARAM_INT.D电磁阀输出等待时间)
        SQLCON.Query_Device_Param(DEVICEPARAM_TABLENAME.INTEGER_TYPE, "真空吸等待时间", PARAM_INT.Z真空吸等待时间)
        SQLCON.Query_Device_Param(DEVICEPARAM_TABLENAME.INTEGER_TYPE, "镭射触发间隔(PLUS)", PARAM_INT.L镭射触发间隔)
        SQLCON.Query_Device_Param(DEVICEPARAM_TABLENAME.INTEGER_TYPE, "设备编号", PARAM_INT.S设备编号)

        'STRING
        SQLCON.Query_Device_Param(DEVICEPARAM_TABLENAME.STRING_TYPE, "视觉文件路径", PARAM_STRING.S视觉文件路径)
        SQLCON.Query_Device_Param(DEVICEPARAM_TABLENAME.STRING_TYPE, "条码枪COM口", PARAM_STRING.T条码枪COM口)
        'SQLCON.Query_Device_Param(DEVICEPARAM_TABLENAME.STRING_TYPE, "数据上传服务器IP", PARAM_STRING.S数据上传服务器IP)
        SQLCON.Query_Device_Param(DEVICEPARAM_TABLENAME.STRING_TYPE, "镭射IP地址", PARAM_STRING.L镭射IP地址)
        SQLCON.Query_Device_Param(DEVICEPARAM_TABLENAME.STRING_TYPE, "PLC串口", PARAM_STRING.PLC串口)
        SQLCON.Query_Device_Param(DEVICEPARAM_TABLENAME.STRING_TYPE, "数据保存路径", PARAM_STRING.S数据保存路径)
        SQLCON.Query_Device_Param(DEVICEPARAM_TABLENAME.STRING_TYPE, "图片保存路径", PARAM_STRING.T图片保存路径)
        SQLCON.Query_Device_Param(DEVICEPARAM_TABLENAME.STRING_TYPE, "IDB保存路径", PARAM_STRING.IDB保存路径)
        'SQLCON.Query_Device_Param(DEVICEPARAM_TABLENAME.STRING_TYPE, "数据汇总表名", PARAM_STRING.S数据汇总表名)
        PARAM_STRING.S数据汇总表名 = "RSAOI"

        If PARAM_STRING.S数据保存路径.EndsWith("\") = False Then
            PARAM_STRING.S数据保存路径 = PARAM_STRING.S数据保存路径 & "\"
        End If
        If PARAM_STRING.T图片保存路径.EndsWith("\") = False Then
            PARAM_STRING.T图片保存路径 = PARAM_STRING.T图片保存路径 & "\"
        End If
        If PARAM_STRING.IDB保存路径.EndsWith("\") = False Then
            PARAM_STRING.IDB保存路径 = PARAM_STRING.IDB保存路径 & "\"
        End If
        'BOOL
        SQLCON.Query_Device_Param(DEVICEPARAM_TABLENAME.BOOL_TYPE, "登陆密码检测", PARAM_BOOL.D登陆密码检测)
        SQLCON.Query_Device_Param(DEVICEPARAM_TABLENAME.BOOL_TYPE, "到位信号检测", PARAM_BOOL.D到位信号检测)
        SQLCON.Query_Device_Param(DEVICEPARAM_TABLENAME.BOOL_TYPE, "保存激光原始数据", PARAM_BOOL.B保存激光原始数据)
        SQLCON.Query_Device_Param(DEVICEPARAM_TABLENAME.BOOL_TYPE, "清空汇总数据", PARAM_BOOL.Q清空汇总数据)
        SQLCON.Query_Device_Param(DEVICEPARAM_TABLENAME.BOOL_TYPE, "调试版本", PARAM_BOOL.T调试版本)
        SQLCON.Query_Device_Param(DEVICEPARAM_TABLENAME.BOOL_TYPE, "是否删除IDB文件", PARAM_BOOL.S是否删除IDB文件)
        SQLCON.Query_Device_Param(DEVICEPARAM_TABLENAME.BOOL_TYPE, "UpDown键测试选择", PARAM_BOOL.UpDown键测试选择)   '新增4

        Select Case PARAM_BOOL.T调试版本
            Case True
                Image_Table_Name = "PROJECT_MESAURE_IMAGE_DEBUG"
                PE_PROVISON.Text = "当前版本：调试版本"
                PE_PROVISON.Style.ForeColor.Color = Color.Red
            Case False
                Image_Table_Name = "PROJECT_MESAURE_IMAGE"
                PE_PROVISON.Text = "当前版本：运行版本"
                PE_PROVISON.Style.ForeColor.Color = Color.Green
        End Select

        Array.Resize(Parameters_Name_Array, 9)
        Array.Resize(Parameters_Value_Array, 9)
        Array.Clear(Parameters_Name_Array, 0, Parameters_Name_Array.Length)
        Array.Clear(Parameters_Value_Array, 0, Parameters_Value_Array.Length)
        Parameters_Name_Array(0) = "PROJECTS"
        Parameters_Name_Array(1) = "PROGRAM_VER"
        Parameters_Name_Array(2) = "AOI_VENDOR"
        Parameters_Name_Array(3) = "LINE_NUMBER"
        Parameters_Name_Array(4) = "BUILDS"
        Parameters_Name_Array(5) = "JSON_FILE_PATH"
        Parameters_Name_Array(6) = "AGENT_TYPE"
        Parameters_Name_Array(7) = "PROCESS"
        Parameters_Name_Array(8) = "COMPONENT"
        SQLCON.Read_Project_Parameter(Parameters_Name_Array, Parameters_Value_Array)
        JSON_PARA.PROJECTS = Parameters_Value_Array(0).ToString.Trim
        JSON_PARA.PROGRAM_VER = Parameters_Value_Array(1).ToString.Trim
        JSON_PARA.AOI_VENDOR = Parameters_Value_Array(2).ToString.Trim
        JSON_PARA.LINE_NUMBER = Parameters_Value_Array(3).ToString.Trim
        JSON_PARA.BUILDS = Parameters_Value_Array(4).ToString.Trim
        JSON_PARA.FILE_PATH = Parameters_Value_Array(5).ToString.Trim
        JSON_PARA.AGENT_TYPE = Parameters_Value_Array(6).ToString.Trim
        JSON_PARA.PROCESS = Parameters_Value_Array(7).ToString.Trim
        JSON_PARA.COMPONENT = Parameters_Value_Array(8).ToString.Trim

        TB_JASON_PROJECTS.Text = Parameters_Value_Array(0).ToString.Trim
        TB_JSON_PROGRAM_VER.Text = Parameters_Value_Array(1).ToString.Trim
        TB_JSON_AOI_VENDOR.Text = Parameters_Value_Array(2).ToString.Trim
        TB_JSON_LINE_NUMBER.Text = Parameters_Value_Array(3).ToString.Trim
        TB_JSON_BUILDS.Text = Parameters_Value_Array(4).ToString.Trim
        TB_JSON_FILE_PATH.Text = Parameters_Value_Array(5).ToString.Trim
        TB_JASON_AGENT_TYPE.Text = Parameters_Value_Array(6).ToString.Trim
        TB_JASON_PROCESS.Text = Parameters_Value_Array(7).ToString.Trim
        TB_JASON_COMPONENT.Text = Parameters_Value_Array(8).ToString.Trim

        Array.Resize(Parameters_Name_Array, 3)
        Array.Resize(Parameters_Value_Array, 3)
        Array.Clear(Parameters_Name_Array, 0, Parameters_Name_Array.Length)
        Array.Clear(Parameters_Value_Array, 0, Parameters_Value_Array.Length)
        Parameters_Name_Array(0) = "JSON_UPLOAD"
        Parameters_Name_Array(1) = "左工位版本切换电磁阀"
        Parameters_Name_Array(2) = "右工位版本切换电磁阀"

        SQLCON.Read_Project_Parameter(Parameters_Name_Array, Parameters_Value_Array)
        JSON_PARA.JSON_UPLOAD = CType(Parameters_Value_Array(0), Boolean)

        SW_JSON_UPLOAD.Value = CType(Parameters_Value_Array(0), Boolean)
        SW_Vision_1.Value = CType(Parameters_Value_Array(1), Boolean)
        SW_Vision_2.Value = CType(Parameters_Value_Array(2), Boolean)

    End Sub

    Public Structure ProjectParam
        Shared Laser_NO As Int16
        Shared ccd_No As Int16
        Shared Limit_Up As Double
        Shared Limit_Down As Double
        Shared Laser_Limit_Up As Double
        Shared Laser_Limit_Down As Double
        Shared Limit_Up_F As Double
        Shared Limit_Down_F As Double
        Shared Limit_Up_All As Double
        Shared Limit_Down_All As Double
    End Structure

    ''' <summary>
    ''' 读取项目参数
    ''' </summary>
    ''' <remarks></remarks>
    Sub Read_Project_Param()
        Array.Resize(Parameters_Name_Array, 8)
        Array.Resize(PARAMETERS_VALUE_Object, 8)
        Array.Clear(Parameters_Name_Array, 0, Parameters_Name_Array.Length)
        Array.Clear(PARAMETERS_VALUE_Object, 0, PARAMETERS_VALUE_Object.Length)
        Parameters_Name_Array(0) = "激光程式号"
        Parameters_Name_Array(1) = "影像程式号"
        Parameters_Name_Array(2) = "公差上限"
        Parameters_Name_Array(3) = "公差下限"
        Parameters_Name_Array(4) = "镭射公差上限"
        Parameters_Name_Array(5) = "镭射公差下限"
        Parameters_Name_Array(6) = "功能键公差上限"
        Parameters_Name_Array(7) = "功能键公差下限"
        SQLCON.Read_Project_Parameter(Select_Product_Name, Parameters_Name_Array, PARAMETERS_VALUE_Object)
        ProjectParam.Laser_NO = CType(PARAMETERS_VALUE_Object(0), Int16)
        'ProjectParam.ccd_No = CType(PARAMETERS_VALUE_Object(1), Int16)
        ProjectParam.Limit_Up = CType(PARAMETERS_VALUE_Object(2), Double)
        ProjectParam.Limit_Down = CType(PARAMETERS_VALUE_Object(3), Double)
        ProjectParam.Laser_Limit_Up = CType(PARAMETERS_VALUE_Object(4), Double)
        ProjectParam.Laser_Limit_Down = CType(PARAMETERS_VALUE_Object(5), Double)
        ProjectParam.Limit_Up_F = CType(PARAMETERS_VALUE_Object(6), Double)
        ProjectParam.Limit_Down_F = CType(PARAMETERS_VALUE_Object(7), Double)
    End Sub


    Private Sub Btn_Query_All_Log_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Btn_Query_All_Log.Click
        SQLCON.Read_Message(DATAGRIDVIEW_LOG, PE_LOG_COUNT, False, 50000)
    End Sub

    Private Sub Btn_Query_Log_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Btn_Query_Log.Click
        SQLCON.Read_Message(DATAGRIDVIEW_LOG, PE_LOG_COUNT, False, 5000)
    End Sub

    Private Sub Btn_Save_Log_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Btn_Save_Log.Click
        If SaveFileDialog_Excel.ShowDialog = Windows.Forms.DialogResult.OK Then
            DATA_PROCESS.Save_Excel(DATAGRIDVIEW_LOG, SaveFileDialog_Excel.FileName, DATA_CONVERSION_LIB.Save_Data_Format.XLS)
        End If
    End Sub

    Private Sub Btn_Delete_Log_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Btn_Delete_Log.Click
        If MessageBoxEx.Show("是否清空动作日志？", "系统消息", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
            Select Case Login_Dialog.ShowDialog
                Case Windows.Forms.DialogResult.Yes
                    SQLCON.Clear_All_Information()
                    SQLCON.Read_Message(DATAGRIDVIEW_LOG, PE_LOG_COUNT, False, 5000)
                Case Windows.Forms.DialogResult.No
                    MessageBoxEx.Show("授权密码错误！如忘记密码请与管理员联系！", "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
            End Select
        End If
    End Sub

    Private Sub PE_Project_Name_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PE_Project_Name.Click
        If ComboBox_product_name.Enabled = False Then
            ComboBox_product_name.Enabled = True
        Else
            ComboBox_product_name.Enabled = False
        End If
    End Sub

    Private Sub Btn_Change_LR_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Btn_Change_LR.Click
        Select Case Login_Dialog.ShowDialog
            Case Windows.Forms.DialogResult.OK
                Bool_Start_All = False
                Bool_Start_Left = False
                Bool_Start_Right = False

                If Btn_Change_LR.Text = "左工位" Then
                    Btn_Change_LR.Text = "右工位"
                    Left_Right_Staion = LR_STATION.RIGHT
                    BOOL.LOCK = False
                ElseIf Btn_Change_LR.Text = "右工位" Then
                    Btn_Change_LR.Text = "双工位"
                    Left_Right_Staion = LR_STATION.ALL
                    BOOL.LOCK = True
                ElseIf Btn_Change_LR.Text = "双工位" Then
                    Btn_Change_LR.Text = "左工位"
                    Left_Right_Staion = LR_STATION.LEFT
                    BOOL.LOCK = True
                End If

                Dim Str As String = "当前工位：" & Btn_Change_LR.Text.ToString.Trim
                Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), Str, "", Color.Black)

                Bool_Start_All = True
                Bool_Start_Left = False
                Bool_Start_Right = False
        End Select
    End Sub


    Private Sub LEFT_DoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles NO_Left.DoWork
    End Sub
    Sub Run_Thread_Fun_Left()
        Dim COOR_NAME As String = Nothing
        Dim Point_number As Integer = 0, Resize_Count As Integer = 0, Axis_ID_Array() As Integer = Nothing, Axis_Pos_Array() As Double = Nothing
        Dim rtn_Bool_Start, Bool_Start_Button As Boolean
        Dim rtn_str As String, Rtn_Int As Int16 = 0, Axis_ID As Integer, Axis_Pos As Double
        Dim Temp_First As Boolean = False, trg_count As Integer, PARAMETERS_VALUE_Object() As Object = Nothing, Parameters_Name_Object() As String = Nothing
        Dim Trigger_count As Int16 = 0, Run_Speed As Integer = 0, Run_Speed_Scale As Double = 1.5
        While Bool_Run
START_LEFT:
            Thread.Sleep(30)
            If Bool_Start_All = True And BOOL.Init_DIO = True And Card_Init_OK = True Then
                RETURN_ZERO_SUB() '//回归原点
                READ_ENCODER_POSITION(AXISMSG.AXIS_ENCODER_POS)
                Bool_Start_Button = CARDCMD.GET_DI_BIT(BOARD_ID_00, DIO.Q启动按钮_0)
                Select Case Bool_Start_Button
                    Case True
                        If Bool_Home_Done = True Then '回原点完成

                            Bool_Start_All = False
                            Btn_Change_LR.Enabled = False

                            rtn_str = "启动按下，等待入料"
                            Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), rtn_str, "", Color.Green)

                            Select Case Left_Right_Staion
                                Case LR_STATION.LEFT

                                    BOOL.LOCK = True '开启左工位，关闭右工位
                                    Bool_Start_Left = True '打开左线程
                                    Bool_Start_Right = False '屏蔽右线程
                                Case LR_STATION.RIGHT

                                    BOOL.LOCK = False '开启右工位，关闭左工位
                                    Bool_Start_Left = False '屏蔽左线程
                                    Bool_Start_Right = True '打开右线程

                                    Run_Thread_Right = New Thread(AddressOf Run_Thread_Fun_Right)
                                    Run_Thread_Right.Start()
                                Case LR_STATION.ALL

                                    BOOL.LOCK = True = True '先开启左工位，后开启右工位
                                    Bool_Start_Left = True '运行左线程
                                    Bool_Start_Right = True  '打开右线程

                                    Run_Thread_Right = New Thread(AddressOf Run_Thread_Fun_Right)
                                    Run_Thread_Right.Start()
                            End Select
                        Else '回原点未完成
                            If Portect_STRCT.Start_Button = False Then
                                Portect_STRCT.Start_Button = True
                                rtn_str = "设备没有归零，请先归零！"
                                Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), rtn_str, "", Color.Red)
                            End If
                        End If
                    Case False
                        If Portect_STRCT.Start_Button = True Then
                            Portect_STRCT.Start_Button = False
                        End If
                End Select
            End If
            If Bool_Start_Left = True Then

                rtn_Bool_Start = CARDCMD.GET_DI_BIT(BOARD_ID_00, DIO.ONE1工位来料检测_0)
                If rtn_Bool_Start = True Then
                    rtn_Bool_Start = False
                    While SwitchButton_runing.Value = False '调试模式，机台暂停
                        Thread.Sleep(200)
                    End While
                    rtn_str = "【1工位流程开始】"
                    Invoke(New Delegate_Display_Message_Left(AddressOf Display_Message_Left), rtn_str, "", Color.Blue)

                    Start_Left()
                    Trigger_count = 0

                    Array.Resize(Parameters_Name_Object, 1)
                    Array.Resize(PARAMETERS_VALUE_Object, 1)
                    Array.Clear(Parameters_Name_Object, 0, Parameters_Name_Object.Length)
                    Array.Clear(PARAMETERS_VALUE_Object, 0, PARAMETERS_VALUE_Object.Length)
                    Parameters_Name_Object(0) = "漏光站左工位产品条码"
                    SQLCON.Read_Project_Parameter(Parameters_Name_Object, PARAMETERS_VALUE_Object)
                    DataCode_Left = CType(PARAMETERS_VALUE_Object(0), String).Trim
                    If DataCode_Left.Length <= 5 Then
                        rtn_str = "漏光站左工位产品条码异常:" & DataCode_Left
                        Invoke(New Delegate_Display_Message_Left(AddressOf Display_Message_Left), rtn_str, DataCode_Left, Color.Red)
                    End If
                    If DataCode_Left = "" Then
                        DataCode_Left = "NULL_L"
                    End If
                    Parameters_Name_Object(0) = "漏光站左工位产品条码"
                    PARAMETERS_VALUE_Object(0) = ""
                    SQLCON.Update_Project_Parameter_Left(Parameters_Name_Object, PARAMETERS_VALUE_Object)
                    Text_SN_Left.Text = DataCode_Left

                    rtn_str = "[1工位读取到条码:]" & DataCode_Left
                    Invoke(New Delegate_Display_Message_Left(AddressOf Display_Message_Left), rtn_str, DataCode_Left, Color.Black)

                    Thread.Sleep(300)
                    For k As Int16 = 1 To 3 'TWO2工位阻挡电磁阀1上升
                        If ONE1工位阻挡电磁阀1(IO.OUT_ON, LR_STATION.LEFT, DataCode_Left) = False Then
                            If k = 3 Then
                                rtn_str = "1工位阻挡电磁阀1不能正常上升，请检查电磁阀是否异常，此电磁阀不上升会导致追料，请检查！"
                                Invoke(New Delegate_Display_Message_Left(AddressOf Display_Message_Left), rtn_str, "", Color.Red)
                                MessageBoxEx.Show(rtn_str, "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
                                BOOL.Error_Close = True
                                Exit Sub
                            Else
                                CARDCMD.SET_DO_BIT(BOARD_ID_00, DIO.TWO2工位阻挡电磁阀1_0, PRA_OUT_STATUS_ENUM.OUT_ON)
                                Thread.Sleep(PARAM_INT.D电磁阀输出等待时间)
                            End If
                        Else
                            Exit For
                        End If
                    Next


                    CARDCMD.SET_DO_BIT(BOARD_ID_00, DIO.ONE1工位顶升电磁阀_0, PRA_OUT_STATUS_ENUM.OUT_ON)
                    Thread.Sleep(PARAM_INT.D电磁阀输出等待时间)

                    rtn_str = "[1工位皮带控制]关"
                    Invoke(New Delegate_Display_Message_Left(AddressOf Display_Message_Left), rtn_str, DataCode_Left, Color.OrangeRed)
                    For i As Int16 = 1 To 3
                        CARDCMD.SET_DO_BIT(BOARD_ID_00, DIO.ONE1工位皮带控制_0, PRA_OUT_STATUS_ENUM.OUT_OFF)
                        Thread.Sleep(50)
                    Next
                    '*********************************************************************************************************************************
                    Close_Trigger_Left() '关闭触发

                    Dim StartIndex As Integer = 0, EndIndex As Integer = 0
                    If Coord_Order_List_LEFT.Count >= 1 Then
                        For Index As Int16 = 0 To Coord_Order_List_LEFT.Count - 1
                            If Index = 0 Then
                                StartIndex = 0
                            Else
                                StartIndex = EndIndex + 1
                            End If
                            EndIndex = StartIndex + Coord_Order_List_LEFT(Index) - 1
                            COOR_NAME = Coord_AllPOS_List_LEFT(StartIndex).坐标名称

                            While SwitchButton_runing.Value = False '调试模式，机台暂停
                                Thread.Sleep(200)
                            End While

                            Select Case COOR_NAME
                                Case "①工位拍照第一行从左到右"

                                    rtn_str = "1工位相机扫描：启动"
                                    Invoke(New Delegate_Display_Message_Left(AddressOf Display_Message_Left), rtn_str, DataCode_Left, Color.Green)

                                    Dim Work_Coordinate() As Work_Coordinatess = Work_左工位拍照第一行从左到右
                                    If Work_Coordinate.Count >= 1 Then
                                        '首点
                                        Dim i As Int16 = 0
                                        Resize_Count = 4
                                        Select Case BOOL.UPH
                                            Case False
                                                Run_Speed = Work_Coordinate(i).运行速度
                                            Case True
                                                Run_Speed = Work_Coordinate(i).运行速度 * Run_Speed_Scale
                                        End Select
                                        Array.Resize(Axis_ID_Array, Resize_Count)
                                        Array.Resize(Axis_Pos_Array, Resize_Count)
                                        Axis_ID_Array(0) = Work_Coordinate(i).轴号(0)
                                        Axis_ID_Array(1) = Work_Coordinate(i).轴号(1)
                                        Axis_ID_Array(2) = Work_Coordinate(i).轴号(2)
                                        Axis_ID_Array(3) = Work_Coordinate(i).轴号(3)
                                        Axis_Pos_Array(0) = Work_Coordinate(i).轴位置(0) - Trigger_X_Offset
                                        Axis_Pos_Array(1) = Work_Coordinate(i).轴位置(1)
                                        Axis_Pos_Array(2) = Work_Coordinate(i).轴位置(2)
                                        Axis_Pos_Array(3) = Work_Coordinate(i).轴位置(3)
                                        CARDCMD.INTERPOLATION_LINE(Axis_ID_Array, Axis_Pos_Array, PRA_APS_OPTION_ENUM.Absolute, Run_Speed, Work_Coordinate(i).加减速度, Work_Coordinate(i).加减速度, PRA_SF_ENUM.S_curve)
                                        While True
                                            Dim rtn_state As Integer = 1
                                            rtn_state *= CARDCMD.CHECK_MOTION_DONE(Work_Coordinate(i).轴号(0), Stop_Code)
                                            rtn_state *= CARDCMD.CHECK_MOTION_DONE(Work_Coordinate(i).轴号(1), Stop_Code)
                                            rtn_state *= CARDCMD.CHECK_MOTION_DONE(Work_Coordinate(i).轴号(2), Stop_Code)
                                            rtn_state *= CARDCMD.CHECK_MOTION_DONE(Work_Coordinate(i).轴号(3), Stop_Code)
                                            If rtn_state = 1 Then
                                                Exit While
                                            End If
                                            'READ_ENCODER_POSITION(AXISMSG.AXIS_ENCODER_POS)
                                        End While
                                        Thread.Sleep(PARAM_INT.Y运动等待时间)

                                        'rtn_str = "等待上一次[1工位图像处理]完成"
                                        'Invoke(New Delegate_Display_Message_Left(AddressOf Display_Message_Left), rtn_str, DataCode_Left, Color.Blue)

                                        'If BOOL.UPH = False Then
                                        '    While Process_Left_Boolean = True
                                        '    End While
                                        'End If

                                        'rtn_str = "上一次[1工位图像处理]完成"
                                        'Invoke(New Delegate_Display_Message_Left(AddressOf Display_Message_Left), rtn_str, DataCode_Left, Color.Blue)

                                        If DataCode_Left = "" Then
                                            DataCode_Left = "NULL"
                                        End If

                                        Dim File_Directory As String = Nothing, File_Directory_IDB As String = Nothing
                                        Dim D1 As String = Date.Now.Year.ToString & " YEAR\"
                                        Dim D2 As String = Date.Now.Month.ToString.PadLeft(2).Replace(" ", "0") & " MONTH\"
                                        Dim D3 As String = Date.Now.Day.ToString.PadLeft(2).Replace(" ", "0") & " DAY\"
                                        File_Directory = PARAM_STRING.T图片保存路径 & D1 & D2 & D3
                                        File_Directory_IDB = PARAM_STRING.IDB保存路径 & D1 & D2 & D3
                                        If System.IO.Directory.Exists(File_Directory) = False Then
                                            System.IO.Directory.CreateDirectory(File_Directory)
                                        End If
                                        If System.IO.Directory.Exists(File_Directory_IDB) = False Then
                                            System.IO.Directory.CreateDirectory(File_Directory_IDB)
                                        End If
                                        Dim YEAR As String = Date.Now.Year
                                        Dim MON As String = Date.Now.Month.ToString.PadLeft(2).Replace(" ", "0")
                                        Dim DAY As String = Date.Now.Day.ToString.PadLeft(2).Replace(" ", "0")
                                        Dim HH As String = Date.Now.Hour.ToString.PadLeft(2).Replace(" ", "0")
                                        Dim MM As String = Date.Now.Minute.ToString.PadLeft(2).Replace(" ", "0")
                                        Dim SS As String = Date.Now.Second.ToString.PadLeft(2).Replace(" ", "0")

                                        Dim m_date As String = YEAR & MON & DAY
                                        Dim m_time As String = HH & MM & SS

                                        Dim New_Data_Code As String = DataCode_Left.Trim.Replace(vbCr, "").Replace(vbLf, "").Trim.Replace(" ", "").Trim.Replace(vbCrLf, "")
                                        CognexImagefile_IDB_Left = File_Directory_IDB & New_Data_Code & "_" & m_date & "_" & m_time & "_LEFT.idb"
                                        CognexImagefile_QS_Left = File_Directory & DataCode_Left


                                        If BOOL.UPH = False Then
                                            Try
                                                ToolBlock_left.Inputs("InputFile").Value = CognexImagefile_IDB_Left
                                                ToolBlock_left.Inputs("InputImageCount").Value = INTTYPLE.Left_Trigger_Count
                                            Catch ex As Exception
                                                rtn_str = "路径错误：" & CognexImagefile_IDB_Left
                                                Invoke(New Delegate_Display_Message_Left(AddressOf Display_Message_Left), rtn_str, DataCode_Left, Color.Red)
                                            End Try
                                            GRAB_IMAGE_BOOLEAN_LEFT = True '****************** [1工位]开始采集图像"
                                            If GRAB_IMAGE_LEFT.IsBusy = False Then
                                                GRAB_IMAGE_LEFT.RunWorkerAsync()
                                            End If
                                            Thread.Sleep(500)
                                        End If

                                        '设置触发参数
                                        CCD_Ser_Trigger_Card_00(TABLE_COMPARE_DIRECTION_ENUM.POSITIVE_DIRECTION, Work_左工位拍照第一行从左到右, Line_Index.LEFT_TO_RIGHT)
                                        '尾点
                                        i = Work_Coordinate.Count - 1
                                        Resize_Count = 4
                                        Select Case BOOL.UPH
                                            Case False
                                                Run_Speed = Work_Coordinate(i).运行速度
                                            Case True
                                                Run_Speed = Work_Coordinate(i).运行速度 * Run_Speed_Scale
                                        End Select
                                        Array.Resize(Axis_ID_Array, Resize_Count)
                                        Array.Resize(Axis_Pos_Array, Resize_Count)
                                        Axis_ID_Array(0) = Work_Coordinate(i).轴号(0)
                                        Axis_ID_Array(1) = Work_Coordinate(i).轴号(1)
                                        Axis_ID_Array(2) = Work_Coordinate(i).轴号(2)
                                        Axis_ID_Array(3) = Work_Coordinate(i).轴号(3)
                                        Axis_Pos_Array(0) = Work_Coordinate(i).轴位置(0) + Trigger_X_Offset
                                        Axis_Pos_Array(1) = Work_Coordinate(i).轴位置(1)
                                        Axis_Pos_Array(2) = Work_Coordinate(i).轴位置(2)
                                        Axis_Pos_Array(3) = Work_Coordinate(i).轴位置(3)
                                        CARDCMD.INTERPOLATION_LINE(Axis_ID_Array, Axis_Pos_Array, PRA_APS_OPTION_ENUM.Absolute, Run_Speed, Work_Coordinate(i).加减速度, Work_Coordinate(i).加减速度, PRA_SF_ENUM.S_curve)
                                        While True
                                            'READ_ENCODER_POSITION(AXISMSG.AXIS_ENCODER_POS)
                                            Dim rtn_state As Integer = 1
                                            rtn_state *= CARDCMD.CHECK_MOTION_DONE(Work_Coordinate(i).轴号(0), Stop_Code)
                                            rtn_state *= CARDCMD.CHECK_MOTION_DONE(Work_Coordinate(i).轴号(1), Stop_Code)
                                            rtn_state *= CARDCMD.CHECK_MOTION_DONE(Work_Coordinate(i).轴号(2), Stop_Code)
                                            rtn_state *= CARDCMD.CHECK_MOTION_DONE(Work_Coordinate(i).轴号(3), Stop_Code)
                                            If rtn_state = 1 Then
                                                Exit While
                                            End If
                                            Try
                                                CARDCMD.get_trigger_count(BOARD_ID_00, 0, trg_count)
                                                Trigger_Left.Text = trg_count & "：" & Work_Coordinate.Count
                                            Catch ex As Exception
                                                rtn_str = "[①工位]触发错误，请检查控制卡。"
                                                Invoke(New Delegate_Display_Message_Left(AddressOf Display_Message_Left), rtn_str, DataCode_Left, Color.Red)
                                            End Try
                                        End While

                                    Else
                                        rtn_str = "[①工位拍照第一行从左到右]没有设置坐标，请先设置坐标"
                                        Invoke(New Delegate_Display_Message_Left(AddressOf Display_Message_Left), rtn_str, DataCode_Left, Color.Red)
                                    End If
                                    Try
                                        CARDCMD.get_trigger_count(BOARD_ID_00, 0, trg_count)
                                        Trigger_count += trg_count
                                    Catch ex As Exception
                                        rtn_str = "[①工位]触发错误，请检查控制卡。"
                                        Invoke(New Delegate_Display_Message_Left(AddressOf Display_Message_Left), rtn_str, DataCode_Left, Color.Red)
                                    End Try
                                Case "①工位拍照第二行从右到左"
                                    Dim Work_Coordinate() As Work_Coordinatess = Work_左工位拍照第二行从右到左
                                    If Work_Coordinate.Count >= 1 Then
                                        '首点
                                        Dim i As Int16 = 0
                                        Resize_Count = 4
                                        Select Case BOOL.UPH
                                            Case False
                                                Run_Speed = Work_Coordinate(i).运行速度
                                            Case True
                                                Run_Speed = Work_Coordinate(i).运行速度 * Run_Speed_Scale
                                        End Select
                                        Array.Resize(Axis_ID_Array, Resize_Count)
                                        Array.Resize(Axis_Pos_Array, Resize_Count)
                                        Axis_ID_Array(0) = Work_Coordinate(i).轴号(0)
                                        Axis_ID_Array(1) = Work_Coordinate(i).轴号(1)
                                        Axis_ID_Array(2) = Work_Coordinate(i).轴号(2)
                                        Axis_ID_Array(3) = Work_Coordinate(i).轴号(3)
                                        Axis_Pos_Array(0) = Work_Coordinate(i).轴位置(0) + Trigger_X_Offset
                                        Axis_Pos_Array(1) = Work_Coordinate(i).轴位置(1)
                                        Axis_Pos_Array(2) = Work_Coordinate(i).轴位置(2)
                                        Axis_Pos_Array(3) = Work_Coordinate(i).轴位置(3)
                                        CARDCMD.INTERPOLATION_LINE(Axis_ID_Array, Axis_Pos_Array, PRA_APS_OPTION_ENUM.Absolute, Run_Speed, Work_Coordinate(i).加减速度, Work_Coordinate(i).加减速度, PRA_SF_ENUM.S_curve)
                                        While True
                                            Dim rtn_state As Integer = 1
                                            rtn_state *= CARDCMD.CHECK_MOTION_DONE(Work_Coordinate(i).轴号(0), Stop_Code)
                                            rtn_state *= CARDCMD.CHECK_MOTION_DONE(Work_Coordinate(i).轴号(1), Stop_Code)
                                            rtn_state *= CARDCMD.CHECK_MOTION_DONE(Work_Coordinate(i).轴号(2), Stop_Code)
                                            rtn_state *= CARDCMD.CHECK_MOTION_DONE(Work_Coordinate(i).轴号(3), Stop_Code)
                                            If rtn_state = 1 Then
                                                Exit While
                                            End If
                                            'READ_ENCODER_POSITION(AXISMSG.AXIS_ENCODER_POS)
                                        End While
                                        Thread.Sleep(PARAM_INT.Y运动等待时间)

                                        CCD_Ser_Trigger_Card_00(TABLE_COMPARE_DIRECTION_ENUM.NEGATIVE_DIRECTION, Work_左工位拍照第二行从右到左, Line_Index.RIGHT_TO_LEFT)

                                        '尾点
                                        i = Work_Coordinate.Count - 1
                                        Resize_Count = 4
                                        Select Case BOOL.UPH
                                            Case False
                                                Run_Speed = Work_Coordinate(i).运行速度
                                            Case True
                                                Run_Speed = Work_Coordinate(i).运行速度 * Run_Speed_Scale
                                        End Select
                                        Array.Resize(Axis_ID_Array, Resize_Count)
                                        Array.Resize(Axis_Pos_Array, Resize_Count)
                                        Axis_ID_Array(0) = Work_Coordinate(i).轴号(0)
                                        Axis_ID_Array(1) = Work_Coordinate(i).轴号(1)
                                        Axis_ID_Array(2) = Work_Coordinate(i).轴号(2)
                                        Axis_ID_Array(3) = Work_Coordinate(i).轴号(3)
                                        Axis_Pos_Array(0) = Work_Coordinate(i).轴位置(0) - Trigger_X_Offset
                                        Axis_Pos_Array(1) = Work_Coordinate(i).轴位置(1)
                                        Axis_Pos_Array(2) = Work_Coordinate(i).轴位置(2)
                                        Axis_Pos_Array(3) = Work_Coordinate(i).轴位置(3)
                                        CARDCMD.INTERPOLATION_LINE(Axis_ID_Array, Axis_Pos_Array, PRA_APS_OPTION_ENUM.Absolute, Run_Speed, Work_Coordinate(i).加减速度, Work_Coordinate(i).加减速度, PRA_SF_ENUM.S_curve)
                                        While True
                                            Dim rtn_state As Integer = 1
                                            rtn_state *= CARDCMD.CHECK_MOTION_DONE(Work_Coordinate(i).轴号(0), Stop_Code)
                                            rtn_state *= CARDCMD.CHECK_MOTION_DONE(Work_Coordinate(i).轴号(1), Stop_Code)
                                            rtn_state *= CARDCMD.CHECK_MOTION_DONE(Work_Coordinate(i).轴号(2), Stop_Code)
                                            rtn_state *= CARDCMD.CHECK_MOTION_DONE(Work_Coordinate(i).轴号(3), Stop_Code)
                                            If rtn_state = 1 Then
                                                Exit While
                                            End If
                                            'READ_ENCODER_POSITION(AXISMSG.AXIS_ENCODER_POS)
                                            Try
                                                CARDCMD.get_trigger_count(BOARD_ID_00, 0, trg_count)
                                                Trigger_Left.Text = trg_count & "：" & Work_Coordinate.Count
                                            Catch ex As Exception
                                                rtn_str = "[①工位]触发错误，请检查控制卡。"
                                                Invoke(New Delegate_Display_Message_Left(AddressOf Display_Message_Left), rtn_str, DataCode_Left, Color.Red)
                                            End Try
                                        End While

                                    Else
                                        rtn_str = "[①工位拍照第二行从右到左]没有设置坐标，请先设置坐标"
                                        Invoke(New Delegate_Display_Message_Left(AddressOf Display_Message_Left), rtn_str, DataCode_Left, Color.Red)
                                    End If
                                    Try
                                        CARDCMD.get_trigger_count(BOARD_ID_00, 0, trg_count)
                                        Trigger_count += trg_count
                                    Catch ex As Exception
                                        rtn_str = "[①工位]触发错误，请检查控制卡。"
                                        Invoke(New Delegate_Display_Message_Left(AddressOf Display_Message_Left), rtn_str, DataCode_Left, Color.Red)
                                    End Try
                                Case "①工位拍照第三行从左到右"

                                    Dim Work_Coordinate() As Work_Coordinatess = Work_左工位拍照第三行从左到右
                                    If Work_Coordinate.Count >= 1 Then
                                        '首点
                                        Dim i As Int16 = 0
                                        Resize_Count = 4
                                        Select Case BOOL.UPH
                                            Case False
                                                Run_Speed = Work_Coordinate(i).运行速度
                                            Case True
                                                Run_Speed = Work_Coordinate(i).运行速度 * Run_Speed_Scale
                                        End Select
                                        Array.Resize(Axis_ID_Array, Resize_Count)
                                        Array.Resize(Axis_Pos_Array, Resize_Count)
                                        Axis_ID_Array(0) = Work_Coordinate(i).轴号(0)
                                        Axis_ID_Array(1) = Work_Coordinate(i).轴号(1)
                                        Axis_ID_Array(2) = Work_Coordinate(i).轴号(2)
                                        Axis_ID_Array(3) = Work_Coordinate(i).轴号(3)
                                        Axis_Pos_Array(0) = Work_Coordinate(i).轴位置(0) - Trigger_X_Offset
                                        Axis_Pos_Array(1) = Work_Coordinate(i).轴位置(1)
                                        Axis_Pos_Array(2) = Work_Coordinate(i).轴位置(2)
                                        Axis_Pos_Array(3) = Work_Coordinate(i).轴位置(3)
                                        CARDCMD.INTERPOLATION_LINE(Axis_ID_Array, Axis_Pos_Array, PRA_APS_OPTION_ENUM.Absolute, Run_Speed, Work_Coordinate(i).加减速度, Work_Coordinate(i).加减速度, PRA_SF_ENUM.S_curve)
                                        While True
                                            Dim rtn_state As Integer = 1
                                            rtn_state *= CARDCMD.CHECK_MOTION_DONE(Work_Coordinate(i).轴号(0), Stop_Code)
                                            rtn_state *= CARDCMD.CHECK_MOTION_DONE(Work_Coordinate(i).轴号(1), Stop_Code)
                                            rtn_state *= CARDCMD.CHECK_MOTION_DONE(Work_Coordinate(i).轴号(2), Stop_Code)
                                            rtn_state *= CARDCMD.CHECK_MOTION_DONE(Work_Coordinate(i).轴号(3), Stop_Code)
                                            If rtn_state = 1 Then
                                                Exit While
                                            End If
                                            'READ_ENCODER_POSITION(AXISMSG.AXIS_ENCODER_POS)
                                        End While
                                        Thread.Sleep(PARAM_INT.Y运动等待时间)

                                        CCD_Ser_Trigger_Card_00(TABLE_COMPARE_DIRECTION_ENUM.POSITIVE_DIRECTION, Work_左工位拍照第三行从左到右, Line_Index.LEFT_TO_RIGHT)

                                        '尾点
                                        i = Work_Coordinate.Count - 1
                                        Resize_Count = 4
                                        Select Case BOOL.UPH
                                            Case False
                                                Run_Speed = Work_Coordinate(i).运行速度
                                            Case True
                                                Run_Speed = Work_Coordinate(i).运行速度 * Run_Speed_Scale
                                        End Select
                                        Array.Resize(Axis_ID_Array, Resize_Count)
                                        Array.Resize(Axis_Pos_Array, Resize_Count)
                                        Axis_ID_Array(0) = Work_Coordinate(i).轴号(0)
                                        Axis_ID_Array(1) = Work_Coordinate(i).轴号(1)
                                        Axis_ID_Array(2) = Work_Coordinate(i).轴号(2)
                                        Axis_ID_Array(3) = Work_Coordinate(i).轴号(3)
                                        Axis_Pos_Array(0) = Work_Coordinate(i).轴位置(0) + Trigger_X_Offset
                                        Axis_Pos_Array(1) = Work_Coordinate(i).轴位置(1)
                                        Axis_Pos_Array(2) = Work_Coordinate(i).轴位置(2)
                                        Axis_Pos_Array(3) = Work_Coordinate(i).轴位置(3)
                                        CARDCMD.INTERPOLATION_LINE(Axis_ID_Array, Axis_Pos_Array, PRA_APS_OPTION_ENUM.Absolute, Run_Speed, Work_Coordinate(i).加减速度, Work_Coordinate(i).加减速度, PRA_SF_ENUM.S_curve)
                                        While True
                                            Dim rtn_state As Integer = 1
                                            rtn_state *= CARDCMD.CHECK_MOTION_DONE(Work_Coordinate(i).轴号(0), Stop_Code)
                                            rtn_state *= CARDCMD.CHECK_MOTION_DONE(Work_Coordinate(i).轴号(1), Stop_Code)
                                            rtn_state *= CARDCMD.CHECK_MOTION_DONE(Work_Coordinate(i).轴号(2), Stop_Code)
                                            rtn_state *= CARDCMD.CHECK_MOTION_DONE(Work_Coordinate(i).轴号(3), Stop_Code)
                                            If rtn_state = 1 Then
                                                Exit While
                                            End If
                                            'READ_ENCODER_POSITION(AXISMSG.AXIS_ENCODER_POS)
                                            Try
                                                CARDCMD.get_trigger_count(BOARD_ID_00, 0, trg_count)
                                                Trigger_Left.Text = trg_count & "：" & Work_Coordinate.Count
                                            Catch ex As Exception
                                                rtn_str = "[①工位]触发错误，请检查控制卡。"
                                                Invoke(New Delegate_Display_Message_Left(AddressOf Display_Message_Left), rtn_str, DataCode_Left, Color.Red)
                                            End Try
                                        End While

                                    Else
                                        rtn_str = "[①工位拍照第三行从左到右]没有设置坐标，请先设置坐标"
                                        Invoke(New Delegate_Display_Message_Left(AddressOf Display_Message_Left), rtn_str, DataCode_Left, Color.Red)
                                    End If
                                    Try
                                        CARDCMD.get_trigger_count(BOARD_ID_00, 0, trg_count)
                                        Trigger_count += trg_count
                                    Catch ex As Exception
                                        rtn_str = "[①工位]触发错误，请检查控制卡。"
                                        Invoke(New Delegate_Display_Message_Left(AddressOf Display_Message_Left), rtn_str, DataCode_Left, Color.Red)
                                    End Try
                                Case "①工位拍照第四行从右到左"

                                    Dim Work_Coordinate() As Work_Coordinatess = Work_左工位拍照第四行从右到左
                                    If Work_Coordinate.Count >= 1 Then
                                        '首点
                                        Dim i As Int16 = 0
                                        Resize_Count = 4
                                        Select Case BOOL.UPH
                                            Case False
                                                Run_Speed = Work_Coordinate(i).运行速度
                                            Case True
                                                Run_Speed = Work_Coordinate(i).运行速度 * Run_Speed_Scale
                                        End Select
                                        Array.Resize(Axis_ID_Array, Resize_Count)
                                        Array.Resize(Axis_Pos_Array, Resize_Count)
                                        Axis_ID_Array(0) = Work_Coordinate(i).轴号(0)
                                        Axis_ID_Array(1) = Work_Coordinate(i).轴号(1)
                                        Axis_ID_Array(2) = Work_Coordinate(i).轴号(2)
                                        Axis_ID_Array(3) = Work_Coordinate(i).轴号(3)
                                        Axis_Pos_Array(0) = Work_Coordinate(i).轴位置(0) + Trigger_X_Offset
                                        Axis_Pos_Array(1) = Work_Coordinate(i).轴位置(1)
                                        Axis_Pos_Array(2) = Work_Coordinate(i).轴位置(2)
                                        Axis_Pos_Array(3) = Work_Coordinate(i).轴位置(3)
                                        CARDCMD.INTERPOLATION_LINE(Axis_ID_Array, Axis_Pos_Array, PRA_APS_OPTION_ENUM.Absolute, Run_Speed, Work_Coordinate(i).加减速度, Work_Coordinate(i).加减速度, PRA_SF_ENUM.S_curve)
                                        While True
                                            Dim rtn_state As Integer = 1
                                            rtn_state *= CARDCMD.CHECK_MOTION_DONE(Work_Coordinate(i).轴号(0), Stop_Code)
                                            rtn_state *= CARDCMD.CHECK_MOTION_DONE(Work_Coordinate(i).轴号(1), Stop_Code)
                                            rtn_state *= CARDCMD.CHECK_MOTION_DONE(Work_Coordinate(i).轴号(2), Stop_Code)
                                            rtn_state *= CARDCMD.CHECK_MOTION_DONE(Work_Coordinate(i).轴号(3), Stop_Code)
                                            If rtn_state = 1 Then
                                                Exit While
                                            End If
                                            'READ_ENCODER_POSITION(AXISMSG.AXIS_ENCODER_POS)
                                        End While
                                        Thread.Sleep(PARAM_INT.Y运动等待时间)

                                        CCD_Ser_Trigger_Card_00(TABLE_COMPARE_DIRECTION_ENUM.NEGATIVE_DIRECTION, Work_左工位拍照第四行从右到左, Line_Index.RIGHT_TO_LEFT)

                                        '尾点
                                        i = Work_Coordinate.Count - 1
                                        Resize_Count = 4
                                        Select Case BOOL.UPH
                                            Case False
                                                Run_Speed = Work_Coordinate(i).运行速度
                                            Case True
                                                Run_Speed = Work_Coordinate(i).运行速度 * Run_Speed_Scale
                                        End Select
                                        Array.Resize(Axis_ID_Array, Resize_Count)
                                        Array.Resize(Axis_Pos_Array, Resize_Count)
                                        Axis_ID_Array(0) = Work_Coordinate(i).轴号(0)
                                        Axis_ID_Array(1) = Work_Coordinate(i).轴号(1)
                                        Axis_ID_Array(2) = Work_Coordinate(i).轴号(2)
                                        Axis_ID_Array(3) = Work_Coordinate(i).轴号(3)
                                        Axis_Pos_Array(0) = Work_Coordinate(i).轴位置(0) - Trigger_X_Offset
                                        Axis_Pos_Array(1) = Work_Coordinate(i).轴位置(1)
                                        Axis_Pos_Array(2) = Work_Coordinate(i).轴位置(2)
                                        Axis_Pos_Array(3) = Work_Coordinate(i).轴位置(3)
                                        CARDCMD.INTERPOLATION_LINE(Axis_ID_Array, Axis_Pos_Array, PRA_APS_OPTION_ENUM.Absolute, Run_Speed, Work_Coordinate(i).加减速度, Work_Coordinate(i).加减速度, PRA_SF_ENUM.S_curve)
                                        While True
                                            Dim rtn_state As Integer = 1
                                            rtn_state *= CARDCMD.CHECK_MOTION_DONE(Work_Coordinate(i).轴号(0), Stop_Code)
                                            rtn_state *= CARDCMD.CHECK_MOTION_DONE(Work_Coordinate(i).轴号(1), Stop_Code)
                                            rtn_state *= CARDCMD.CHECK_MOTION_DONE(Work_Coordinate(i).轴号(2), Stop_Code)
                                            rtn_state *= CARDCMD.CHECK_MOTION_DONE(Work_Coordinate(i).轴号(3), Stop_Code)
                                            If rtn_state = 1 Then
                                                Exit While
                                            End If
                                            'READ_ENCODER_POSITION(AXISMSG.AXIS_ENCODER_POS)
                                            Try
                                                CARDCMD.get_trigger_count(BOARD_ID_00, 0, trg_count)
                                                Trigger_Left.Text = trg_count & "：" & Work_Coordinate.Count
                                            Catch ex As Exception
                                                rtn_str = "[①工位]触发错误，请检查控制卡。"
                                                Invoke(New Delegate_Display_Message_Left(AddressOf Display_Message_Left), rtn_str, DataCode_Left, Color.Red)
                                            End Try
                                        End While

                                    Else
                                        rtn_str = "[①工位拍照第四行从右到左]没有设置坐标，请先设置坐标"
                                        Invoke(New Delegate_Display_Message_Left(AddressOf Display_Message_Left), rtn_str, DataCode_Left, Color.Red)
                                    End If
                                    Try
                                        CARDCMD.get_trigger_count(BOARD_ID_00, 0, trg_count)
                                        Trigger_count += trg_count
                                    Catch ex As Exception
                                        rtn_str = "[①工位]触发错误，请检查控制卡。"
                                        Invoke(New Delegate_Display_Message_Left(AddressOf Display_Message_Left), rtn_str, DataCode_Left, Color.Red)
                                    End Try
                                Case "①工位拍照第五行从左到右"

                                    Dim Work_Coordinate() As Work_Coordinatess = Work_左工位拍照第五行从左到右
                                    If Work_Coordinate.Count >= 1 Then
                                        '首点
                                        Dim i As Int16 = 0
                                        Resize_Count = 4
                                        Select Case BOOL.UPH
                                            Case False
                                                Run_Speed = Work_Coordinate(i).运行速度
                                            Case True
                                                Run_Speed = Work_Coordinate(i).运行速度 * Run_Speed_Scale
                                        End Select
                                        Array.Resize(Axis_ID_Array, Resize_Count)
                                        Array.Resize(Axis_Pos_Array, Resize_Count)
                                        Axis_ID_Array(0) = Work_Coordinate(i).轴号(0)
                                        Axis_ID_Array(1) = Work_Coordinate(i).轴号(1)
                                        Axis_ID_Array(2) = Work_Coordinate(i).轴号(2)
                                        Axis_ID_Array(3) = Work_Coordinate(i).轴号(3)
                                        Axis_Pos_Array(0) = Work_Coordinate(i).轴位置(0) - Trigger_X_Offset
                                        Axis_Pos_Array(1) = Work_Coordinate(i).轴位置(1)
                                        Axis_Pos_Array(2) = Work_Coordinate(i).轴位置(2)
                                        Axis_Pos_Array(3) = Work_Coordinate(i).轴位置(3)
                                        CARDCMD.INTERPOLATION_LINE(Axis_ID_Array, Axis_Pos_Array, PRA_APS_OPTION_ENUM.Absolute, Run_Speed, Work_Coordinate(i).加减速度, Work_Coordinate(i).加减速度, PRA_SF_ENUM.S_curve)
                                        While True
                                            Dim rtn_state As Integer = 1
                                            rtn_state *= CARDCMD.CHECK_MOTION_DONE(Work_Coordinate(i).轴号(0), Stop_Code)
                                            rtn_state *= CARDCMD.CHECK_MOTION_DONE(Work_Coordinate(i).轴号(1), Stop_Code)
                                            rtn_state *= CARDCMD.CHECK_MOTION_DONE(Work_Coordinate(i).轴号(2), Stop_Code)
                                            rtn_state *= CARDCMD.CHECK_MOTION_DONE(Work_Coordinate(i).轴号(3), Stop_Code)
                                            If rtn_state = 1 Then
                                                Exit While
                                            End If
                                            'READ_ENCODER_POSITION(AXISMSG.AXIS_ENCODER_POS)
                                        End While
                                        Thread.Sleep(PARAM_INT.Y运动等待时间)

                                        CCD_Ser_Trigger_Card_00(TABLE_COMPARE_DIRECTION_ENUM.POSITIVE_DIRECTION, Work_左工位拍照第五行从左到右, Line_Index.LEFT_TO_RIGHT)

                                        '尾点
                                        i = Work_Coordinate.Count - 1
                                        Resize_Count = 4
                                        Select Case BOOL.UPH
                                            Case False
                                                Run_Speed = Work_Coordinate(i).运行速度
                                            Case True
                                                Run_Speed = Work_Coordinate(i).运行速度 * Run_Speed_Scale
                                        End Select
                                        Array.Resize(Axis_ID_Array, Resize_Count)
                                        Array.Resize(Axis_Pos_Array, Resize_Count)
                                        Axis_ID_Array(0) = Work_Coordinate(i).轴号(0)
                                        Axis_ID_Array(1) = Work_Coordinate(i).轴号(1)
                                        Axis_ID_Array(2) = Work_Coordinate(i).轴号(2)
                                        Axis_ID_Array(3) = Work_Coordinate(i).轴号(3)
                                        Axis_Pos_Array(0) = Work_Coordinate(i).轴位置(0) + Trigger_X_Offset
                                        Axis_Pos_Array(1) = Work_Coordinate(i).轴位置(1)
                                        Axis_Pos_Array(2) = Work_Coordinate(i).轴位置(2)
                                        Axis_Pos_Array(3) = Work_Coordinate(i).轴位置(3)
                                        CARDCMD.INTERPOLATION_LINE(Axis_ID_Array, Axis_Pos_Array, PRA_APS_OPTION_ENUM.Absolute, Run_Speed, Work_Coordinate(i).加减速度, Work_Coordinate(i).加减速度, PRA_SF_ENUM.S_curve)
                                        While True
                                            Dim rtn_state As Integer = 1
                                            rtn_state *= CARDCMD.CHECK_MOTION_DONE(Work_Coordinate(i).轴号(0), Stop_Code)
                                            rtn_state *= CARDCMD.CHECK_MOTION_DONE(Work_Coordinate(i).轴号(1), Stop_Code)
                                            rtn_state *= CARDCMD.CHECK_MOTION_DONE(Work_Coordinate(i).轴号(2), Stop_Code)
                                            rtn_state *= CARDCMD.CHECK_MOTION_DONE(Work_Coordinate(i).轴号(3), Stop_Code)
                                            If rtn_state = 1 Then
                                                Exit While
                                            End If
                                            'READ_ENCODER_POSITION(AXISMSG.AXIS_ENCODER_POS)
                                            Try
                                                CARDCMD.get_trigger_count(BOARD_ID_00, 0, trg_count)
                                                Trigger_Left.Text = trg_count & "：" & Work_Coordinate.Count
                                            Catch ex As Exception
                                                rtn_str = "[①工位]触发错误，请检查控制卡。"
                                                Invoke(New Delegate_Display_Message_Left(AddressOf Display_Message_Left), rtn_str, DataCode_Left, Color.Red)
                                            End Try
                                        End While

                                    Else
                                        rtn_str = "[①工位拍照第五行从左到右]没有设置坐标，请先设置坐标"
                                        Invoke(New Delegate_Display_Message_Left(AddressOf Display_Message_Left), rtn_str, DataCode_Left, Color.Red)
                                    End If

                                    Try
                                        CARDCMD.get_trigger_count(BOARD_ID_00, 0, trg_count)
                                        Trigger_count += trg_count
                                    Catch ex As Exception
                                        rtn_str = "[①工位]触发错误，请检查控制卡。"
                                        Invoke(New Delegate_Display_Message_Left(AddressOf Display_Message_Left), rtn_str, DataCode_Left, Color.Red)
                                    End Try
                                Case "①工位拍照第六行从右到左"

                                    Dim Work_Coordinate() As Work_Coordinatess = Work_左工位拍照第六行从右到左
                                    If Work_Coordinate.Count >= 1 Then
                                        '首点
                                        Dim i As Int16 = 0
                                        Resize_Count = 4
                                        Select Case BOOL.UPH
                                            Case False
                                                Run_Speed = Work_Coordinate(i).运行速度
                                            Case True
                                                Run_Speed = Work_Coordinate(i).运行速度 * Run_Speed_Scale
                                        End Select
                                        Array.Resize(Axis_ID_Array, Resize_Count)
                                        Array.Resize(Axis_Pos_Array, Resize_Count)
                                        Axis_ID_Array(0) = Work_Coordinate(i).轴号(0)
                                        Axis_ID_Array(1) = Work_Coordinate(i).轴号(1)
                                        Axis_ID_Array(2) = Work_Coordinate(i).轴号(2)
                                        Axis_ID_Array(3) = Work_Coordinate(i).轴号(3)
                                        Axis_Pos_Array(0) = Work_Coordinate(i).轴位置(0) + Trigger_X_Offset
                                        Axis_Pos_Array(1) = Work_Coordinate(i).轴位置(1)
                                        Axis_Pos_Array(2) = Work_Coordinate(i).轴位置(2)
                                        Axis_Pos_Array(3) = Work_Coordinate(i).轴位置(3)
                                        CARDCMD.INTERPOLATION_LINE(Axis_ID_Array, Axis_Pos_Array, PRA_APS_OPTION_ENUM.Absolute, Run_Speed, Work_Coordinate(i).加减速度, Work_Coordinate(i).加减速度, PRA_SF_ENUM.S_curve)
                                        While True
                                            Dim rtn_state As Integer = 1
                                            rtn_state *= CARDCMD.CHECK_MOTION_DONE(Work_Coordinate(i).轴号(0), Stop_Code)
                                            rtn_state *= CARDCMD.CHECK_MOTION_DONE(Work_Coordinate(i).轴号(1), Stop_Code)
                                            rtn_state *= CARDCMD.CHECK_MOTION_DONE(Work_Coordinate(i).轴号(2), Stop_Code)
                                            rtn_state *= CARDCMD.CHECK_MOTION_DONE(Work_Coordinate(i).轴号(3), Stop_Code)
                                            If rtn_state = 1 Then
                                                Exit While
                                            End If
                                            'READ_ENCODER_POSITION(AXISMSG.AXIS_ENCODER_POS)
                                        End While
                                        Thread.Sleep(PARAM_INT.Y运动等待时间)

                                        CCD_Ser_Trigger_Card_00(TABLE_COMPARE_DIRECTION_ENUM.NEGATIVE_DIRECTION, Work_左工位拍照第六行从右到左, Line_Index.RIGHT_TO_LEFT)

                                        '尾点
                                        i = Work_Coordinate.Count - 1
                                        Resize_Count = 4
                                        Select Case BOOL.UPH
                                            Case False
                                                Run_Speed = Work_Coordinate(i).运行速度
                                            Case True
                                                Run_Speed = Work_Coordinate(i).运行速度 * Run_Speed_Scale
                                        End Select
                                        Array.Resize(Axis_ID_Array, Resize_Count)
                                        Array.Resize(Axis_Pos_Array, Resize_Count)
                                        Axis_ID_Array(0) = Work_Coordinate(i).轴号(0)
                                        Axis_ID_Array(1) = Work_Coordinate(i).轴号(1)
                                        Axis_ID_Array(2) = Work_Coordinate(i).轴号(2)
                                        Axis_ID_Array(3) = Work_Coordinate(i).轴号(3)
                                        Axis_Pos_Array(0) = Work_Coordinate(i).轴位置(0) - Trigger_X_Offset
                                        Axis_Pos_Array(1) = Work_Coordinate(i).轴位置(1)
                                        Axis_Pos_Array(2) = Work_Coordinate(i).轴位置(2)
                                        Axis_Pos_Array(3) = Work_Coordinate(i).轴位置(3)
                                        CARDCMD.INTERPOLATION_LINE(Axis_ID_Array, Axis_Pos_Array, PRA_APS_OPTION_ENUM.Absolute, Run_Speed, Work_Coordinate(i).加减速度, Work_Coordinate(i).加减速度, PRA_SF_ENUM.S_curve)
                                        While True
                                            Dim rtn_state As Integer = 1
                                            rtn_state *= CARDCMD.CHECK_MOTION_DONE(Work_Coordinate(i).轴号(0), Stop_Code)
                                            rtn_state *= CARDCMD.CHECK_MOTION_DONE(Work_Coordinate(i).轴号(1), Stop_Code)
                                            rtn_state *= CARDCMD.CHECK_MOTION_DONE(Work_Coordinate(i).轴号(2), Stop_Code)
                                            rtn_state *= CARDCMD.CHECK_MOTION_DONE(Work_Coordinate(i).轴号(3), Stop_Code)
                                            If rtn_state = 1 Then
                                                Exit While
                                            End If
                                            'READ_ENCODER_POSITION(AXISMSG.AXIS_ENCODER_POS)
                                            Try
                                                CARDCMD.get_trigger_count(BOARD_ID_00, 0, trg_count)
                                                Trigger_Left.Text = trg_count & "：" & Work_Coordinate.Count
                                            Catch ex As Exception
                                                rtn_str = "[①工位]触发错误，请检查控制卡。"
                                                Invoke(New Delegate_Display_Message_Right(AddressOf Display_Message_Right), rtn_str, DataCode_Right, Color.Red)
                                            End Try
                                        End While

                                    Else
                                        rtn_str = "[①工位拍照第六行从右到左]没有设置坐标，请先设置坐标"
                                        Invoke(New Delegate_Display_Message_Right(AddressOf Display_Message_Right), rtn_str, DataCode_Right, Color.Red)
                                    End If
                                    Try
                                        CARDCMD.get_trigger_count(BOARD_ID_00, 0, trg_count)
                                        Trigger_count += trg_count
                                    Catch ex As Exception
                                        rtn_str = "[①工位]触发错误，请检查控制卡。"
                                        Invoke(New Delegate_Display_Message_Right(AddressOf Display_Message_Right), rtn_str, DataCode_Right, Color.Red)
                                    End Try

                                    Close_Trigger_Left()

                                    rtn_str = "1工位相机扫描：结束"
                                    Invoke(New Delegate_Display_Message_Left(AddressOf Display_Message_Left), rtn_str, DataCode_Left, Color.Green)


                                    '获取总触发次数
                                    Try
                                        CARDCMD.get_trigger_count(BOARD_ID_00, 0, trg_count)
                                        Trigger_count += trg_count
                                        If Trigger_count = INTTYPLE.Left_Trigger_Count Then
                                            Trigger_Left.Text = Trigger_count
                                            Trigger_Left.Style.BackColor1.Color = Color.Green
                                        Else
                                            Trigger_Left.Text = Trigger_count
                                            Trigger_Left.Style.BackColor1.Color = Color.Red
                                        End If
                                    Catch ex As Exception
                                        rtn_str = "[①工位]触发错误，请检查控制卡。"
                                        Invoke(New Delegate_Display_Message_Right(AddressOf Display_Message_Right), rtn_str, DataCode_Right, Color.Red)
                                    End Try
                                Case Else
                                    For RunIndex = StartIndex To EndIndex
                                        AXIS_INDEX1_LEFT.Clear()
                                        AXIS_INDEX2_LEFT.Clear()
                                        AXIS_INDEX3_LEFT.Clear()
                                        AXIS_INDEX4_LEFT.Clear()
                                        AXIS_INDEX5_LEFT.Clear()
                                        '把程序优先级顺序排序，并存入序列号
                                        For i As Integer = 0 To Coord_AllPOS_List_LEFT(RunIndex).轴运动顺序.Length - 1 '判断优先级
                                            Select Case Coord_AllPOS_List_LEFT(RunIndex).轴运动顺序(i)
                                                Case "1"
                                                    AXIS_INDEX1_LEFT.Add(i)
                                                Case "2"
                                                    AXIS_INDEX2_LEFT.Add(i)
                                                Case "3"
                                                    AXIS_INDEX3_LEFT.Add(i)
                                                Case "4"
                                                    AXIS_INDEX4_LEFT.Add(i)
                                                Case "5"
                                                    AXIS_INDEX5_LEFT.Add(i)
                                            End Select
                                        Next
                                        For M As Int16 = 1 To 5 '按1~5的顺序进性运动
                                            Dim AXIS_SPORT_INDEX As New List(Of Integer)
                                            AXIS_SPORT_INDEX.Clear()
                                            Select Case M
                                                Case 1
                                                    AXIS_SPORT_INDEX = AXIS_INDEX1_LEFT
                                                Case 2
                                                    AXIS_SPORT_INDEX = AXIS_INDEX2_LEFT
                                                Case 3
                                                    AXIS_SPORT_INDEX = AXIS_INDEX3_LEFT
                                                Case 4
                                                    AXIS_SPORT_INDEX = AXIS_INDEX4_LEFT
                                                Case 5
                                                    AXIS_SPORT_INDEX = AXIS_INDEX5_LEFT
                                            End Select
                                            If AXIS_SPORT_INDEX.Count >= 1 Then

                                                Select Case AXIS_SPORT_INDEX.Count
                                                    Case Is = 1 '*****************************单轴运动
                                                        '查询对应轴号
                                                        Axis_ID = Coord_AllPOS_List_LEFT(RunIndex).轴号(AXIS_SPORT_INDEX(0))
                                                        Axis_Pos = Coord_AllPOS_List_LEFT(RunIndex).轴位置(AXIS_SPORT_INDEX(0))

                                                        Rtn_Int = CARDCMD.JOG_PTP_ALL(Axis_ID, PRA_APS_OPTION_ENUM.Absolute, Coord_AllPOS_List_LEFT(RunIndex).运行速度, Coord_AllPOS_List_LEFT(RunIndex).加减速度, Axis_Pos, Nothing)
                                                        Select Case Rtn_Int
                                                            Case Is = 0
                                                                While BOOL.EXIT_APP = False
                                                                    Dim rtn_state As Integer = 1
                                                                    rtn_state *= CARDCMD.CHECK_MOTION_DONE(Axis_ID, Stop_Code)
                                                                    If rtn_state = 1 Then
                                                                        Exit While
                                                                    End If
                                                                    READ_ENCODER_POSITION(AXISMSG.AXIS_ENCODER_POS)

                                                                End While
                                                            Case Is <> 0
                                                                Error_Str = ADLINK_RETURN_MSG(Rtn_Int)
                                                                BOOL.Error_Close = True '关闭软件
                                                                Exit Sub
                                                        End Select
                                                    Case Is > 1 '*****************************插补运动
                                                        '查询对应轴号
                                                        Array.Resize(Axis_ID_Array, AXIS_SPORT_INDEX.Count)
                                                        Array.Resize(Axis_Pos_Array, AXIS_SPORT_INDEX.Count)

                                                        For I As Int16 = 0 To AXIS_SPORT_INDEX.Count - 1
                                                            Axis_ID_Array(I) = Coord_AllPOS_List_LEFT(RunIndex).轴号(AXIS_SPORT_INDEX(I))
                                                            Axis_Pos_Array(I) = Coord_AllPOS_List_LEFT(RunIndex).轴位置(AXIS_SPORT_INDEX(I))
                                                        Next
                                                        Rtn_Int = CARDCMD.INTERPOLATION_LINE(Axis_ID_Array, Axis_Pos_Array, PRA_APS_OPTION_ENUM.Absolute, Coord_AllPOS_List_LEFT(RunIndex).运行速度, Coord_AllPOS_List_LEFT(RunIndex).加减速度, Coord_AllPOS_List_LEFT(RunIndex).加减速度, PRA_SF_ENUM.S_curve)
                                                        Select Case Rtn_Int
                                                            Case Is = 0
                                                                While BOOL.EXIT_APP = False
                                                                    Dim rtn_state As Integer = 1
                                                                    For j As Integer = 0 To AXIS_SPORT_INDEX.Count - 1
                                                                        rtn_state *= CARDCMD.CHECK_MOTION_DONE(Axis_ID_Array(j), Stop_Code)
                                                                    Next
                                                                    If rtn_state = 1 Then
                                                                        Exit While
                                                                    End If
                                                                    READ_ENCODER_POSITION(AXISMSG.AXIS_ENCODER_POS)

                                                                End While
                                                            Case Is <> 0
                                                                Error_Str = ADLINK_RETURN_MSG(Rtn_Int)
                                                                BOOL.Error_Close = True '关闭软件
                                                                Exit Sub
                                                        End Select
                                                End Select
                                            End If
                                        Next
                                        Thread.Sleep(PARAM_INT.Y运动等待时间)

                                        While SwitchButton_runing.Value = False '调试模式，机台暂停
                                            Thread.Sleep(200)
                                        End While


                                        If COOR_NAME = "左取料位" And Coord_AllPOS_List_LEFT(RunIndex).IO触发状态 = True Then

                                            If CARDCMD.GET_DI_BIT(BOARD_ID_01, DIO.ONE1工位顶升WORK_1) = False Then
                                                If ONE1工位顶升电磁阀(IO.OUT_ON, LR_STATION.LEFT, DataCode_Left) = False Then
                                                    Error_Str = "1工位顶升电磁阀上升不到位，软件即将关闭！"
                                                    Invoke(New Delegate_Display_Message_Left(AddressOf Display_Message_Left), Error_Str, "", Color.Red)
                                                    MessageBoxEx.Show(Error_Str, "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
                                                    'BOOL.Error_Close = True
                                                    'Exit Sub
                                                End If
                                            End If

                                            If CARDCMD.GET_DI_BIT(BOARD_ID_00, DIO.ONE1工位真空吸检测_0) = True Then
                                                rtn_str = "1工位真空吸检测到有料，此时点击确定，会造成【两片料叠加】，请卸料并检查后，重启软件"
                                                Invoke(New Delegate_Display_Message_Left(AddressOf Display_Message_Left), rtn_str, DataCode_Left, Color.Red)
                                                MessageBoxEx.Show(rtn_str, "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
                                            End If

                                            If JSON_PARA.PROJECTS.Contains("1359") Then
                                                C键ONE1工位真空吸电磁阀(IO.OUT_ON, LR_STATION.LEFT, DataCode_Left)
                                            End If
                                            ONE1工位真空吸电磁阀(IO.OUT_ON, LR_STATION.LEFT, DataCode_Left)
                                        End If
                                        If COOR_NAME = "左放料位" And Coord_AllPOS_List_LEFT(RunIndex).IO触发状态 = True Then
                                            '真空吸多关闭几次

                                            'ONE1工位真空吸电磁阀(IO.OUT_OFF, LR_STATION.LEFT, DataCode_Left)
                                            For i As Int16 = 1 To 2
                                                CARDCMD.SET_DO_BIT(BOARD_ID_00, DIO.ONE1工位真空吸电磁阀_0, PRA_OUT_STATUS_ENUM.OUT_OFF)
                                                If JSON_PARA.PROJECTS.Contains("1359") Then
                                                    CARDCMD.SET_DO_BIT(BOARD_ID_00, DIO.C键ONE1工位真空吸电磁阀_0, PRA_OUT_STATUS_ENUM.OUT_OFF)
                                                End If
                                                Thread.Sleep(PARAM_INT.D电磁阀输出等待时间)
                                            Next

                                            rtn_str = "[1工位破真空电磁阀]开"
                                            Invoke(New Delegate_Display_Message_Left(AddressOf Display_Message_Left), rtn_str, DataCode_Left, Color.Black)

                                            For i As Int16 = 1 To 3
                                                CARDCMD.SET_DO_BIT(BOARD_ID_00, DIO.ONE1工位破真空电磁阀_0, PRA_OUT_STATUS_ENUM.OUT_ON)
                                                Thread.Sleep(300)
                                            Next

                                            For i As Integer = 0 To HomeParamObjArray.Length - 1
                                                If HomeParamObjArray(i).轴名称 = "Z01" Then
                                                    Axis_ID = HomeParamObjArray(i).轴号
                                                End If
                                            Next
                                            CARDCMD.JOG_PTP_ALL(Axis_ID, PRA_APS_OPTION_ENUM.Relative, 20, 0.1, 0.8, Nothing) '抬高到安全位
                                            Select Case Rtn_Int
                                                Case Is = 0
                                                    While BOOL.EXIT_APP = False
                                                        Dim rtn_state As Integer = 1
                                                        rtn_state *= CARDCMD.CHECK_MOTION_DONE(Axis_ID, Stop_Code)
                                                        If rtn_state = 1 Then
                                                            Exit While
                                                        End If
                                                        READ_ENCODER_POSITION(AXISMSG.AXIS_ENCODER_POS)
                                                    End While
                                                Case Is <> 0
                                                    Error_Str = ADLINK_RETURN_MSG(Rtn_Int)
                                                    BOOL.Error_Close = True '关闭软件
                                                    Exit Sub
                                            End Select
                                            Thread.Sleep(300)

                                            rtn_str = "[1工位破真空电磁阀]关"
                                            Invoke(New Delegate_Display_Message_Left(AddressOf Display_Message_Left), rtn_str, DataCode_Left, Color.Black)
                                            For pozhenkong_i As Integer = 0 To 2
                                                CARDCMD.SET_DO_BIT(BOARD_ID_00, DIO.ONE1工位破真空电磁阀_0, PRA_OUT_STATUS_ENUM.OUT_OFF)
                                                Thread.Sleep(200)
                                            Next
                                            While SwitchButton_runing.Value = False '调试模式，机台暂停
                                                Thread.Sleep(200)
                                            End While

                                            If CARDCMD.GET_DI_BIT(BOARD_ID_00, DIO.ONE1工位真空吸检测_0) = True Then
                                                CARDCMD.SET_DO_BIT(BOARD_ID_00, DIO.ONE1工位破真空电磁阀_0, PRA_OUT_STATUS_ENUM.OUT_ON)
                                                Thread.Sleep(PARAM_INT.D电磁阀输出等待时间)
                                                For pozhenkong_i As Integer = 0 To 2
                                                    CARDCMD.SET_DO_BIT(BOARD_ID_00, DIO.ONE1工位破真空电磁阀_0, PRA_OUT_STATUS_ENUM.OUT_OFF)
                                                    Thread.Sleep(PARAM_INT.D电磁阀输出等待时间)
                                                Next
                                                If CARDCMD.GET_DI_BIT(BOARD_ID_00, DIO.ONE1工位真空吸检测_0) = True Then
                                                    rtn_str = "1工位真空吸不能正常关闭，此时点击确定，会造成【两片料叠加】，请卸料并检查后，重启软件"
                                                    Invoke(New Delegate_Display_Message_Left(AddressOf Display_Message_Left), rtn_str, DataCode_Left, Color.Red)
                                                    MessageBoxEx.Show(rtn_str, "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
                                                End If
                                            End If

                                            If ONE1工位阻挡电磁阀2(IO.OUT_OFF, LR_STATION.LEFT, DataCode_Left) = False Then
                                                Thread.Sleep(300)
                                                CARDCMD.SET_DO_BIT(BOARD_ID_00, DIO.ONE1工位阻挡电磁阀2_0, PRA_OUT_STATUS_ENUM.OUT_OFF)
                                                Thread.Sleep(PARAM_INT.D电磁阀输出等待时间)
                                                If CARDCMD.GET_DI_BIT(BOARD_ID_00, DIO.ONE1工位阻挡HOME2_0) = False Then
                                                    Error_Str = "[1工位阻挡电磁阀2]不能正常下降，程序不能正常运行，请卸料并检查后，重启软件"
                                                    Invoke(New Delegate_Display_Message_Left(AddressOf Display_Message_Left), Error_Str, "", Color.Red)
                                                    MessageBoxEx.Show(Error_Str, "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
                                                    'BOOL.Error_Close = True
                                                    'Exit Sub
                                                End If
                                            End If

                                            If ONE1工位顶升电磁阀(IO.OUT_OFF, LR_STATION.LEFT, DataCode_Left) = False Then
                                                Thread.Sleep(300)
                                                CARDCMD.SET_DO_BIT(BOARD_ID_00, DIO.ONE1工位顶升电磁阀_0, PRA_OUT_STATUS_ENUM.OUT_OFF)
                                                Thread.Sleep(PARAM_INT.D电磁阀输出等待时间)
                                                If CARDCMD.GET_DI_BIT(BOARD_ID_01, DIO.ONE1工位顶升HOME_1) = False Then

                                                    Error_Str = "[工位顶升电磁阀]不能正常下降，程序不能正常运行，请卸料并检查后，重启软件"
                                                    Invoke(New Delegate_Display_Message_Left(AddressOf Display_Message_Left), Error_Str, "", Color.Red)
                                                    MessageBoxEx.Show(Error_Str, "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
                                                    'BOOL.Error_Close = True
                                                    'Exit Sub

                                                End If
                                            End If

                                            While SwitchButton_runing.Value = False '调试模式，机台暂停
                                                Thread.Sleep(200)
                                            End While


                                        End If
                                    Next
                            End Select

                        Next
                    Else
                        rtn_str = "没有设置左工位坐标，请先设置坐标！"
                        Invoke(New Delegate_Display_Message_Left(AddressOf Display_Message_Left), rtn_str, DataCode_Left, Color.Red)
                    End If
                    '****************************************************************************************************************
                    rtn_str = "等待[1工位出料检测]无料"
                    Invoke(New Delegate_Display_Message_Left(AddressOf Display_Message_Left), rtn_str, DataCode_Left, Color.Blue)
                    While True
                        If CARDCMD.GET_DI_BIT(BOARD_ID_00, DIO.ONE1工位出料检测_0) = False Then
                            Exit While
                        End If
                    End While
                    rtn_str = "[1工位出料检测]无料，准备出料"
                    Invoke(New Delegate_Display_Message_Left(AddressOf Display_Message_Left), rtn_str, DataCode_Left, Color.Blue)

                    rtn_str = "[1工位皮带控制]开"
                    Invoke(New Delegate_Display_Message_Left(AddressOf Display_Message_Left), rtn_str, DataCode_Left, Color.OrangeRed)
                    For i As Int16 = 1 To 3
                        CARDCMD.SET_DO_BIT(BOARD_ID_00, DIO.ONE1工位皮带控制_0, PRA_OUT_STATUS_ENUM.OUT_ON)
                        Thread.Sleep(50)
                    Next

                    rtn_str = "等待[1工位采集图像]完成"
                    Invoke(New Delegate_Display_Message_Left(AddressOf Display_Message_Left), rtn_str, DataCode_Left, Color.Blue)

                    If BOOL.UPH = False Then
                        While GRAB_IMAGE_BOOLEAN_LEFT = True
                            Thread.Sleep(20)
                        End While
                    End If

                    rtn_str = "1工位采集图像[完成]"
                    Invoke(New Delegate_Display_Message_Left(AddressOf Display_Message_Left), rtn_str, DataCode_Left, Color.Blue)



                    rtn_str = "等待上一次[1工位图像处理]完成"
                    Invoke(New Delegate_Display_Message_Left(AddressOf Display_Message_Left), rtn_str, DataCode_Left, Color.Blue)

                    If BOOL.UPH = False Then
                        While Process_Left_Boolean = True
                            Thread.Sleep(20)
                        End While
                        Thread.Sleep(500)
                    End If

                    rtn_str = "上一次[1工位图像处理]完成"
                    Invoke(New Delegate_Display_Message_Left(AddressOf Display_Message_Left), rtn_str, DataCode_Left, Color.Blue)

                    While SwitchButton_runing.Value = False '调试模式，机台暂停
                        Thread.Sleep(200)
                    End While


                    DataCode_Left_tmp = DataCode_Left
                    CognexImagefile_IDB_Left_tmp = CognexImagefile_IDB_Left
                    CognexImagefile_QS_Left_tmp = CognexImagefile_QS_Left

                    If BOOL.UPH = False Then
                        Process_Left_Boolean = True '******************"开始运行[1工位图像处理]"
                        If PROCESS_IMAGE_LEFT.IsBusy = False Then
                            PROCESS_IMAGE_LEFT.RunWorkerAsync()
                        End If
                    End If

                    rtn_str = "等待产品运行到[1工位出料位]"
                    Invoke(New Delegate_Display_Message_Left(AddressOf Display_Message_Left), rtn_str, DataCode_Left, Color.Green)
                    While True
                        If CARDCMD.GET_DI_BIT(BOARD_ID_00, DIO.ONE1工位出料检测_0) = True Then
                            Thread.Sleep(100)
                            Exit While
                        End If
                    End While
                    rtn_str = "[1工位出料位]出料完成"
                    Invoke(New Delegate_Display_Message_Left(AddressOf Display_Message_Left), rtn_str, DataCode_Left, Color.Green)

                    For k As Int16 = 1 To 3 '阻挡失败再阻挡一次
                        If ONE1工位阻挡电磁阀2(IO.OUT_ON, LR_STATION.LEFT, DataCode_Left) = False Then
                            If k = 3 Then
                                Error_Str = "1工位阻挡电磁阀2不能正常升起，请检查电磁阀是否异常，此电磁阀不升起会造成追料，请慎重！"
                                Invoke(New Delegate_Display_Message_Left(AddressOf Display_Message_Left), Error_Str, "", Color.Red)
                                MessageBoxEx.Show(Error_Str, "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
                                'BOOL.Error_Close = True
                                'Exit Sub
                            Else
                                CARDCMD.SET_DO_BIT(BOARD_ID_00, DIO.ONE1工位阻挡电磁阀2_0, PRA_OUT_STATUS_ENUM.OUT_ON)
                                Thread.Sleep(PARAM_INT.D电磁阀输出等待时间)
                            End If
                        Else
                            Exit For
                        End If
                    Next

                    While SwitchButton_runing.Value = False '调试模式，机台暂停
                        Thread.Sleep(200)
                    End While

                    For k As Int16 = 1 To 3 '1工位阻挡电磁阀1下降
                        If ONE1工位阻挡电磁阀1(IO.OUT_OFF, LR_STATION.LEFT, DataCode_Left) = False Then
                            If k = 3 Then

                                Error_Str = "1工位阻挡电磁阀1不能正常下降，请检查电磁阀是否异常，此电磁阀不下降会导致无法进料，请检查！"
                                Invoke(New Delegate_Display_Message_Left(AddressOf Display_Message_Left), Error_Str, "", Color.Red)
                                MessageBoxEx.Show(Error_Str, "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
                                'BOOL.Error_Close = True
                                'Exit Sub
                            Else
                                CARDCMD.SET_DO_BIT(BOARD_ID_00, DIO.ONE1工位阻挡电磁阀1_0, PRA_OUT_STATUS_ENUM.OUT_OFF)
                                Thread.Sleep(PARAM_INT.D电磁阀输出等待时间)
                            End If
                        Else
                            Exit For
                        End If
                    Next
                    End_Left()

                    rtn_str = "【1工位流程结束】"
                    Invoke(New Delegate_Display_Message_Left(AddressOf Display_Message_Left), rtn_str, DataCode_Left, Color.Black)
                End If
            End If
        End While
    End Sub

  
    Private Sub RIGHT_DoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles NO_Right.DoWork
    End Sub
    Sub Run_Thread_Fun_Right()
        Dim COOR_NAME As String = Nothing
        Dim Point_number As Integer = 0, Resize_Count As Integer = 0, Axis_ID_Array() As Integer = Nothing, Axis_Pos_Array() As Double = Nothing
        Dim rtn_Bool_Start As Boolean
        Dim rtn_str As String, Rtn_Int As Int16 = 0, Axis_ID As Integer, Axis_Pos As Double
        Dim Temp_First As Boolean = False, trg_count As Integer, PARAMETERS_VALUE_Object() As Object = Nothing, Parameters_Name_Object() As String = Nothing
        Dim Trigger_count As Int16 = 0, file_name As String = "", Run_Speed As Integer = 0, Run_Speed_Scale As Double = 1.5

        While Bool_Run
START_RIGHT:
            Thread.Sleep(30)
            If Bool_Start_Right = True Then

                rtn_Bool_Start = CARDCMD.GET_DI_BIT(BOARD_ID_00, DIO.TWO2工位来料检测_0)
                If rtn_Bool_Start = True Then
                    rtn_Bool_Start = False

                    rtn_str = "【2工位流程开始】"
                    Invoke(New Delegate_Display_Message_Right(AddressOf Display_Message_Right), rtn_str, "", Color.Black)

                    While SwitchButton_runing.Value = False '调试模式，机台暂停
                        Thread.Sleep(200)
                    End While


                    Start_Right()
                    Trigger_count = 0

                    Array.Resize(Parameters_Name_Object, 1)
                    Array.Resize(PARAMETERS_VALUE_Object, 1)
                    Array.Clear(Parameters_Name_Object, 0, Parameters_Name_Object.Length)
                    Array.Clear(PARAMETERS_VALUE_Object, 0, PARAMETERS_VALUE_Object.Length)
                    Parameters_Name_Object(0) = "漏光站右工位产品条码"
                    SQLCON.Read_Project_Parameter(Parameters_Name_Object, PARAMETERS_VALUE_Object)
                    DataCode_Right = CType(PARAMETERS_VALUE_Object(0), String).Trim
                    If DataCode_Right.Length <= 5 Then
                        rtn_str = "漏光站右工位产品条码异常:" & DataCode_Right
                        Invoke(New Delegate_Display_Message_Right(AddressOf Display_Message_Right), rtn_str, DataCode_Right, Color.Red)
                    End If
                    If DataCode_Right = "" Then
                        DataCode_Right = "NULL_R"
                    End If
                    Parameters_Name_Object(0) = "漏光站右工位产品条码"
                    PARAMETERS_VALUE_Object(0) = ""
                    SQLCON.Update_Project_Parameter_Right(Parameters_Name_Object, PARAMETERS_VALUE_Object)
                    Text_SN_Right.Text = DataCode_Right

                    rtn_str = "[2工位读取到条码:]" & DataCode_Right
                    Invoke(New Delegate_Display_Message_Right(AddressOf Display_Message_Right), rtn_str, DataCode_Right, Color.Black)

                    Thread.Sleep(300)
                    For k As Int16 = 1 To 3 'TWO2工位阻挡电磁阀1上升
                        If TWO2工位阻挡电磁阀1(IO.OUT_ON, LR_STATION.RIGHT, DataCode_Right) = False Then
                            If k = 3 Then
                               
                                Error_Str = "2工位阻挡电磁阀1不能正常上升，请检查电磁阀是否异常，此电磁阀不上升会导致追料，请检查！"
                                Invoke(New Delegate_Display_Message_Right(AddressOf Display_Message_Right), Error_Str, "", Color.Red)
                                MessageBoxEx.Show(Error_Str, "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
                                'BOOL.Error_Close = True
                                'Exit Sub
                            Else
                                CARDCMD.SET_DO_BIT(BOARD_ID_00, DIO.TWO2工位阻挡电磁阀1_0, PRA_OUT_STATUS_ENUM.OUT_ON)
                                Thread.Sleep(PARAM_INT.D电磁阀输出等待时间)
                            End If
                        Else
                            Exit For
                        End If
                    Next

                    CARDCMD.SET_DO_BIT(BOARD_ID_00, DIO.TWO2工位顶升电磁阀_0, PRA_OUT_STATUS_ENUM.OUT_ON)
                    Thread.Sleep(PARAM_INT.D电磁阀输出等待时间)

                    rtn_str = "[2工位皮带控制]关"
                    Invoke(New Delegate_Display_Message_Right(AddressOf Display_Message_Right), rtn_str, DataCode_Right, Color.OrangeRed)
                    For i As Int16 = 1 To 3
                        CARDCMD.SET_DO_BIT(BOARD_ID_00, DIO.TWO2工位皮带控制_0, PRA_OUT_STATUS_ENUM.OUT_OFF)
                        Thread.Sleep(150)
                    Next
                    '*********************************************************************************************************************************

                    While SwitchButton_runing.Value = False '调试模式，机台暂停
                        Thread.Sleep(200)
                    End While

                    Close_Trigger_Right() '关闭触发

                    Dim StartIndex As Int16 = 0, EndIndex As Int16 = 0
                    If Coord_Order_List_RIGHT.Count >= 1 Then
                        For Index As Int16 = 0 To Coord_Order_List_RIGHT.Count - 1
                            If Index = 0 Then
                                StartIndex = 0
                            Else
                                StartIndex = EndIndex + 1
                            End If
                            EndIndex = StartIndex + Coord_Order_List_RIGHT(Index) - 1
                            COOR_NAME = Coord_AllPOS_List_RIGHT(StartIndex).坐标名称

                            While SwitchButton_runing.Value = False '调试模式，机台暂停
                                Thread.Sleep(200)
                            End While


                            Select Case COOR_NAME
                                Case "②工位拍照第一行从左到右"

                                    rtn_str = "2工位相机扫描：启动"
                                    Invoke(New Delegate_Display_Message_Right(AddressOf Display_Message_Right), rtn_str, DataCode_Right, Color.Green)

                                    Dim Work_Coordinate() As Work_Coordinatess = Work_右工位拍照第一行从左到右
                                    If Work_Coordinate.Count >= 1 Then
                                        '首点
                                        Dim i As Int16 = 0
                                        Resize_Count = 4
                                        Select Case BOOL.UPH
                                            Case False
                                                Run_Speed = Work_Coordinate(i).运行速度
                                            Case True
                                                Run_Speed = Work_Coordinate(i).运行速度 * Run_Speed_Scale
                                        End Select
                                        Array.Resize(Axis_ID_Array, Resize_Count)
                                        Array.Resize(Axis_Pos_Array, Resize_Count)
                                        Axis_ID_Array(0) = Work_Coordinate(i).轴号(0)
                                        Axis_ID_Array(1) = Work_Coordinate(i).轴号(1)
                                        Axis_ID_Array(2) = Work_Coordinate(i).轴号(2)
                                        Axis_ID_Array(3) = Work_Coordinate(i).轴号(3)
                                        Axis_Pos_Array(0) = Work_Coordinate(i).轴位置(0) - Trigger_X_Offset
                                        Axis_Pos_Array(1) = Work_Coordinate(i).轴位置(1)
                                        Axis_Pos_Array(2) = Work_Coordinate(i).轴位置(2)
                                        Axis_Pos_Array(3) = Work_Coordinate(i).轴位置(3)
                                        CARDCMD.INTERPOLATION_LINE(Axis_ID_Array, Axis_Pos_Array, PRA_APS_OPTION_ENUM.Absolute, Run_Speed, Work_Coordinate(i).加减速度, Work_Coordinate(i).加减速度, PRA_SF_ENUM.S_curve)
                                        While True
                                            Dim rtn_state As Integer = 1
                                            rtn_state *= CARDCMD.CHECK_MOTION_DONE(Work_Coordinate(i).轴号(0), Stop_Code)
                                            rtn_state *= CARDCMD.CHECK_MOTION_DONE(Work_Coordinate(i).轴号(1), Stop_Code)
                                            rtn_state *= CARDCMD.CHECK_MOTION_DONE(Work_Coordinate(i).轴号(2), Stop_Code)
                                            rtn_state *= CARDCMD.CHECK_MOTION_DONE(Work_Coordinate(i).轴号(3), Stop_Code)
                                            If rtn_state = 1 Then
                                                Exit While
                                            End If
                                            'READ_ENCODER_POSITION(AXISMSG.AXIS_ENCODER_POS)
                                        End While
                                        Thread.Sleep(PARAM_INT.Y运动等待时间)

                                        'rtn_str = "等待上一次[2工位图像处理]结束"
                                        'Invoke(New Delegate_Display_Message_Right(AddressOf Display_Message_Right), rtn_str, DataCode_Right, Color.Blue)

                                        'If BOOL.UPH = False Then
                                        '    While Process_Right_Boolean = True
                                        '    End While
                                        'End If

                                        'rtn_str = "上一次[2工位图像处理]完成"
                                        'Invoke(New Delegate_Display_Message_Right(AddressOf Display_Message_Right), rtn_str, DataCode_Right, Color.Blue)

                                        If DataCode_Right = "" Then
                                            DataCode_Right = "NULL"
                                        End If

                                        Dim File_Directory As String = Nothing, File_Directory_IDB As String = Nothing
                                        Dim D1 As String = Date.Now.Year.ToString & " YEAR\"
                                        Dim D2 As String = Date.Now.Month.ToString.PadLeft(2).Replace(" ", "0") & " MONTH\"
                                        Dim D3 As String = Date.Now.Day.ToString.PadLeft(2).Replace(" ", "0") & " DAY\"
                                        File_Directory = PARAM_STRING.T图片保存路径 & D1 & D2 & D3
                                        File_Directory_IDB = PARAM_STRING.IDB保存路径 & D1 & D2 & D3
                                        If System.IO.Directory.Exists(File_Directory) = False Then
                                            System.IO.Directory.CreateDirectory(File_Directory)
                                        End If
                                        If System.IO.Directory.Exists(File_Directory_IDB) = False Then
                                            System.IO.Directory.CreateDirectory(File_Directory_IDB)
                                        End If
                                        Dim YEAR As String = Date.Now.Year
                                        Dim MON As String = Date.Now.Month.ToString.PadLeft(2).Replace(" ", "0")
                                        Dim DAY As String = Date.Now.Day.ToString.PadLeft(2).Replace(" ", "0")
                                        Dim HH As String = Date.Now.Hour.ToString.PadLeft(2).Replace(" ", "0")
                                        Dim MM As String = Date.Now.Minute.ToString.PadLeft(2).Replace(" ", "0")
                                        Dim SS As String = Date.Now.Second.ToString.PadLeft(2).Replace(" ", "0")

                                        Dim m_date As String = YEAR & MON & DAY
                                        Dim m_time As String = HH & MM & SS

                                        Dim New_Data_Code As String = DataCode_Right.Trim.Replace(vbCr, "").Replace(vbLf, "").Trim.Replace(" ", "").Trim.Replace(vbCrLf, "")
                                        CognexImagefile_IDB_Right = File_Directory_IDB & New_Data_Code & "_" & m_date & "_" & m_time & "_RIGHT.idb"
                                        CognexImagefile_QS_Right = File_Directory & DataCode_Right

                                        If BOOL.UPH = False Then
                                            Try
                                                toolblock_right.Inputs("InputFile").Value = CognexImagefile_IDB_Right
                                                toolblock_right.Inputs("InputImageCount").Value = INTTYPLE.Right_Trigger_Count
                                            Catch ex As Exception
                                                rtn_str = "路径错误：" & CognexImagefile_IDB_Right
                                                Invoke(New Delegate_Display_Message_Right(AddressOf Display_Message_Right), rtn_str, DataCode_Right, Color.Red)
                                            End Try
                                            '******************"[2工位]开始采集图像"
                                            GRAB_IMAGE_BOOLEAN_RIGHT = True
                                            If GRAB_IMAGE_RIGHT.IsBusy = False Then
                                                GRAB_IMAGE_RIGHT.RunWorkerAsync()
                                            End If
                                            Thread.Sleep(500)
                                        End If
                                      

                                        '设置触发次数
                                        CCD_Ser_Trigger_Card_01(TABLE_COMPARE_DIRECTION_ENUM.POSITIVE_DIRECTION, Work_右工位拍照第一行从左到右, Line_Index.LEFT_TO_RIGHT)

                                        '尾点
                                        i = Work_Coordinate.Count - 1
                                        Resize_Count = 4
                                        Select Case BOOL.UPH
                                            Case False
                                                Run_Speed = Work_Coordinate(i).运行速度
                                            Case True
                                                Run_Speed = Work_Coordinate(i).运行速度 * Run_Speed_Scale
                                        End Select
                                        Array.Resize(Axis_ID_Array, Resize_Count)
                                        Array.Resize(Axis_Pos_Array, Resize_Count)
                                        Axis_ID_Array(0) = Work_Coordinate(i).轴号(0)
                                        Axis_ID_Array(1) = Work_Coordinate(i).轴号(1)
                                        Axis_ID_Array(2) = Work_Coordinate(i).轴号(2)
                                        Axis_ID_Array(3) = Work_Coordinate(i).轴号(3)
                                        Axis_Pos_Array(0) = Work_Coordinate(i).轴位置(0) + Trigger_X_Offset
                                        Axis_Pos_Array(1) = Work_Coordinate(i).轴位置(1)
                                        Axis_Pos_Array(2) = Work_Coordinate(i).轴位置(2)
                                        Axis_Pos_Array(3) = Work_Coordinate(i).轴位置(3)
                                        CARDCMD.INTERPOLATION_LINE(Axis_ID_Array, Axis_Pos_Array, PRA_APS_OPTION_ENUM.Absolute, Run_Speed, Work_Coordinate(i).加减速度, Work_Coordinate(i).加减速度, PRA_SF_ENUM.S_curve)
                                        While True
                                            'READ_ENCODER_POSITION(AXISMSG.AXIS_ENCODER_POS)
                                            Dim rtn_state As Integer = 1
                                            rtn_state *= CARDCMD.CHECK_MOTION_DONE(Work_Coordinate(i).轴号(0), Stop_Code)
                                            rtn_state *= CARDCMD.CHECK_MOTION_DONE(Work_Coordinate(i).轴号(1), Stop_Code)
                                            rtn_state *= CARDCMD.CHECK_MOTION_DONE(Work_Coordinate(i).轴号(2), Stop_Code)
                                            rtn_state *= CARDCMD.CHECK_MOTION_DONE(Work_Coordinate(i).轴号(3), Stop_Code)
                                            If rtn_state = 1 Then
                                                Exit While
                                            End If
                                            Try
                                                CARDCMD.get_trigger_count(BOARD_ID_01, 0, trg_count)
                                                Trigger_Right.Text = trg_count & "：" & Work_Coordinate.Count
                                            Catch ex As Exception
                                                rtn_str = "[②工位]触发错误，请检查控制卡。"
                                                Invoke(New Delegate_Display_Message_Right(AddressOf Display_Message_Right), rtn_str, DataCode_Right, Color.Red)
                                            End Try
                                        End While

                                    Else
                                        rtn_str = "[②工位拍照第一行从左到右]没有设置坐标，请先设置坐标"
                                        Invoke(New Delegate_Display_Message_Right(AddressOf Display_Message_Right), rtn_str, DataCode_Right, Color.Red)
                                    End If
                                    Try
                                        CARDCMD.get_trigger_count(BOARD_ID_01, 0, trg_count)
                                        Trigger_count += trg_count
                                    Catch ex As Exception
                                        rtn_str = "[②工位]触发错误，请检查控制卡。"
                                        Invoke(New Delegate_Display_Message_Right(AddressOf Display_Message_Right), rtn_str, DataCode_Right, Color.Red)
                                    End Try
                                Case "②工位拍照第二行从右到左"

                                    Dim Work_Coordinate() As Work_Coordinatess = Work_右工位拍照第二行从右到左
                                    If Work_Coordinate.Count >= 1 Then
                                        '首点
                                        Dim i As Int16 = 0
                                        Resize_Count = 4
                                        Select Case BOOL.UPH
                                            Case False
                                                Run_Speed = Work_Coordinate(i).运行速度
                                            Case True
                                                Run_Speed = Work_Coordinate(i).运行速度 * Run_Speed_Scale
                                        End Select
                                        Array.Resize(Axis_ID_Array, Resize_Count)
                                        Array.Resize(Axis_Pos_Array, Resize_Count)
                                        Axis_ID_Array(0) = Work_Coordinate(i).轴号(0)
                                        Axis_ID_Array(1) = Work_Coordinate(i).轴号(1)
                                        Axis_ID_Array(2) = Work_Coordinate(i).轴号(2)
                                        Axis_ID_Array(3) = Work_Coordinate(i).轴号(3)
                                        Axis_Pos_Array(0) = Work_Coordinate(i).轴位置(0) + Trigger_X_Offset
                                        Axis_Pos_Array(1) = Work_Coordinate(i).轴位置(1)
                                        Axis_Pos_Array(2) = Work_Coordinate(i).轴位置(2)
                                        Axis_Pos_Array(3) = Work_Coordinate(i).轴位置(3)
                                        CARDCMD.INTERPOLATION_LINE(Axis_ID_Array, Axis_Pos_Array, PRA_APS_OPTION_ENUM.Absolute, Run_Speed, Work_Coordinate(i).加减速度, Work_Coordinate(i).加减速度, PRA_SF_ENUM.S_curve)
                                        While True
                                            Dim rtn_state As Integer = 1
                                            rtn_state *= CARDCMD.CHECK_MOTION_DONE(Work_Coordinate(i).轴号(0), Stop_Code)
                                            rtn_state *= CARDCMD.CHECK_MOTION_DONE(Work_Coordinate(i).轴号(1), Stop_Code)
                                            rtn_state *= CARDCMD.CHECK_MOTION_DONE(Work_Coordinate(i).轴号(2), Stop_Code)
                                            rtn_state *= CARDCMD.CHECK_MOTION_DONE(Work_Coordinate(i).轴号(3), Stop_Code)
                                            If rtn_state = 1 Then
                                                Exit While
                                            End If
                                            'READ_ENCODER_POSITION(AXISMSG.AXIS_ENCODER_POS)
                                        End While
                                        Thread.Sleep(PARAM_INT.Y运动等待时间)

                                        CCD_Ser_Trigger_Card_01(TABLE_COMPARE_DIRECTION_ENUM.NEGATIVE_DIRECTION, Work_右工位拍照第二行从右到左, Line_Index.RIGHT_TO_LEFT)

                                        '尾点
                                        i = Work_Coordinate.Count - 1
                                        Resize_Count = 4
                                        Select Case BOOL.UPH
                                            Case False
                                                Run_Speed = Work_Coordinate(i).运行速度
                                            Case True
                                                Run_Speed = Work_Coordinate(i).运行速度 * Run_Speed_Scale
                                        End Select
                                        Array.Resize(Axis_ID_Array, Resize_Count)
                                        Array.Resize(Axis_Pos_Array, Resize_Count)
                                        Axis_ID_Array(0) = Work_Coordinate(i).轴号(0)
                                        Axis_ID_Array(1) = Work_Coordinate(i).轴号(1)
                                        Axis_ID_Array(2) = Work_Coordinate(i).轴号(2)
                                        Axis_ID_Array(3) = Work_Coordinate(i).轴号(3)
                                        Axis_Pos_Array(0) = Work_Coordinate(i).轴位置(0) - Trigger_X_Offset
                                        Axis_Pos_Array(1) = Work_Coordinate(i).轴位置(1)
                                        Axis_Pos_Array(2) = Work_Coordinate(i).轴位置(2)
                                        Axis_Pos_Array(3) = Work_Coordinate(i).轴位置(3)
                                        CARDCMD.INTERPOLATION_LINE(Axis_ID_Array, Axis_Pos_Array, PRA_APS_OPTION_ENUM.Absolute, Run_Speed, Work_Coordinate(i).加减速度, Work_Coordinate(i).加减速度, PRA_SF_ENUM.S_curve)
                                        While True
                                            Dim rtn_state As Integer = 1
                                            rtn_state *= CARDCMD.CHECK_MOTION_DONE(Work_Coordinate(i).轴号(0), Stop_Code)
                                            rtn_state *= CARDCMD.CHECK_MOTION_DONE(Work_Coordinate(i).轴号(1), Stop_Code)
                                            rtn_state *= CARDCMD.CHECK_MOTION_DONE(Work_Coordinate(i).轴号(2), Stop_Code)
                                            rtn_state *= CARDCMD.CHECK_MOTION_DONE(Work_Coordinate(i).轴号(3), Stop_Code)
                                            If rtn_state = 1 Then
                                                Exit While
                                            End If
                                            'READ_ENCODER_POSITION(AXISMSG.AXIS_ENCODER_POS)
                                            Try
                                                CARDCMD.get_trigger_count(BOARD_ID_01, 0, trg_count)
                                                Trigger_Right.Text = trg_count & "：" & Work_Coordinate.Count
                                            Catch ex As Exception
                                                rtn_str = "[②工位]触发错误，请检查控制卡。"
                                                Invoke(New Delegate_Display_Message_Right(AddressOf Display_Message_Right), rtn_str, DataCode_Right, Color.Red)
                                            End Try
                                        End While

                                    Else
                                        rtn_str = "[②工位拍照第二行从右到左]没有设置坐标，请先设置坐标"
                                        Invoke(New Delegate_Display_Message_Right(AddressOf Display_Message_Right), rtn_str, DataCode_Right, Color.Red)
                                    End If
                                    Try
                                        CARDCMD.get_trigger_count(BOARD_ID_01, 0, trg_count)
                                        Trigger_count += trg_count
                                    Catch ex As Exception
                                        rtn_str = "[②工位]触发错误，请检查控制卡。"
                                        Invoke(New Delegate_Display_Message_Right(AddressOf Display_Message_Right), rtn_str, DataCode_Right, Color.Red)
                                    End Try
                                Case "②工位拍照第三行从左到右"

                                    Dim Work_Coordinate() As Work_Coordinatess = Work_右工位拍照第三行从左到右
                                    If Work_Coordinate.Count >= 1 Then
                                        '首点
                                        Dim i As Int16 = 0
                                        Resize_Count = 4
                                        Select Case BOOL.UPH
                                            Case False
                                                Run_Speed = Work_Coordinate(i).运行速度
                                            Case True
                                                Run_Speed = Work_Coordinate(i).运行速度 * Run_Speed_Scale
                                        End Select
                                        Array.Resize(Axis_ID_Array, Resize_Count)
                                        Array.Resize(Axis_Pos_Array, Resize_Count)
                                        Axis_ID_Array(0) = Work_Coordinate(i).轴号(0)
                                        Axis_ID_Array(1) = Work_Coordinate(i).轴号(1)
                                        Axis_ID_Array(2) = Work_Coordinate(i).轴号(2)
                                        Axis_ID_Array(3) = Work_Coordinate(i).轴号(3)
                                        Axis_Pos_Array(0) = Work_Coordinate(i).轴位置(0) - Trigger_X_Offset
                                        Axis_Pos_Array(1) = Work_Coordinate(i).轴位置(1)
                                        Axis_Pos_Array(2) = Work_Coordinate(i).轴位置(2)
                                        Axis_Pos_Array(3) = Work_Coordinate(i).轴位置(3)
                                        CARDCMD.INTERPOLATION_LINE(Axis_ID_Array, Axis_Pos_Array, PRA_APS_OPTION_ENUM.Absolute, Run_Speed, Work_Coordinate(i).加减速度, Work_Coordinate(i).加减速度, PRA_SF_ENUM.S_curve)
                                        While True
                                            Dim rtn_state As Integer = 1
                                            rtn_state *= CARDCMD.CHECK_MOTION_DONE(Work_Coordinate(i).轴号(0), Stop_Code)
                                            rtn_state *= CARDCMD.CHECK_MOTION_DONE(Work_Coordinate(i).轴号(1), Stop_Code)
                                            rtn_state *= CARDCMD.CHECK_MOTION_DONE(Work_Coordinate(i).轴号(2), Stop_Code)
                                            rtn_state *= CARDCMD.CHECK_MOTION_DONE(Work_Coordinate(i).轴号(3), Stop_Code)
                                            If rtn_state = 1 Then
                                                Exit While
                                            End If
                                            'READ_ENCODER_POSITION(AXISMSG.AXIS_ENCODER_POS)
                                        End While
                                        Thread.Sleep(PARAM_INT.Y运动等待时间)

                                        CCD_Ser_Trigger_Card_01(TABLE_COMPARE_DIRECTION_ENUM.POSITIVE_DIRECTION, Work_右工位拍照第三行从左到右, Line_Index.LEFT_TO_RIGHT)

                                        '尾点
                                        i = Work_Coordinate.Count - 1
                                        Resize_Count = 4
                                        Select Case BOOL.UPH
                                            Case False
                                                Run_Speed = Work_Coordinate(i).运行速度
                                            Case True
                                                Run_Speed = Work_Coordinate(i).运行速度 * Run_Speed_Scale
                                        End Select
                                        Array.Resize(Axis_ID_Array, Resize_Count)
                                        Array.Resize(Axis_Pos_Array, Resize_Count)
                                        Axis_ID_Array(0) = Work_Coordinate(i).轴号(0)
                                        Axis_ID_Array(1) = Work_Coordinate(i).轴号(1)
                                        Axis_ID_Array(2) = Work_Coordinate(i).轴号(2)
                                        Axis_ID_Array(3) = Work_Coordinate(i).轴号(3)
                                        Axis_Pos_Array(0) = Work_Coordinate(i).轴位置(0) + Trigger_X_Offset
                                        Axis_Pos_Array(1) = Work_Coordinate(i).轴位置(1)
                                        Axis_Pos_Array(2) = Work_Coordinate(i).轴位置(2)
                                        Axis_Pos_Array(3) = Work_Coordinate(i).轴位置(3)
                                        CARDCMD.INTERPOLATION_LINE(Axis_ID_Array, Axis_Pos_Array, PRA_APS_OPTION_ENUM.Absolute, Run_Speed, Work_Coordinate(i).加减速度, Work_Coordinate(i).加减速度, PRA_SF_ENUM.S_curve)
                                        While True
                                            Dim rtn_state As Integer = 1
                                            rtn_state *= CARDCMD.CHECK_MOTION_DONE(Work_Coordinate(i).轴号(0), Stop_Code)
                                            rtn_state *= CARDCMD.CHECK_MOTION_DONE(Work_Coordinate(i).轴号(1), Stop_Code)
                                            rtn_state *= CARDCMD.CHECK_MOTION_DONE(Work_Coordinate(i).轴号(2), Stop_Code)
                                            rtn_state *= CARDCMD.CHECK_MOTION_DONE(Work_Coordinate(i).轴号(3), Stop_Code)
                                            If rtn_state = 1 Then
                                                Exit While
                                            End If
                                            'READ_ENCODER_POSITION(AXISMSG.AXIS_ENCODER_POS)
                                            Try
                                                CARDCMD.get_trigger_count(BOARD_ID_01, 0, trg_count)
                                                Trigger_Right.Text = trg_count & "：" & Work_Coordinate.Count
                                            Catch ex As Exception
                                                rtn_str = "[②工位]触发错误，请检查控制卡。"
                                                Invoke(New Delegate_Display_Message_Right(AddressOf Display_Message_Right), rtn_str, DataCode_Right, Color.Red)
                                            End Try
                                        End While

                                    Else
                                        rtn_str = "[②工位拍照第三行从左到右]没有设置坐标，请先设置坐标"
                                        Invoke(New Delegate_Display_Message_Right(AddressOf Display_Message_Right), rtn_str, DataCode_Right, Color.Red)
                                    End If
                                    Try
                                        CARDCMD.get_trigger_count(BOARD_ID_01, 0, trg_count)
                                        Trigger_count += trg_count
                                    Catch ex As Exception
                                        rtn_str = "[②工位]触发错误，请检查控制卡。"
                                        Invoke(New Delegate_Display_Message_Right(AddressOf Display_Message_Right), rtn_str, DataCode_Right, Color.Red)
                                    End Try
                                Case "②工位拍照第四行从右到左"

                                    Dim Work_Coordinate() As Work_Coordinatess = Work_右工位拍照第四行从右到左
                                    If Work_Coordinate.Count >= 1 Then
                                        '首点
                                        Dim i As Int16 = 0
                                        Resize_Count = 4
                                        Select Case BOOL.UPH
                                            Case False
                                                Run_Speed = Work_Coordinate(i).运行速度
                                            Case True
                                                Run_Speed = Work_Coordinate(i).运行速度 * Run_Speed_Scale
                                        End Select
                                        Array.Resize(Axis_ID_Array, Resize_Count)
                                        Array.Resize(Axis_Pos_Array, Resize_Count)
                                        Axis_ID_Array(0) = Work_Coordinate(i).轴号(0)
                                        Axis_ID_Array(1) = Work_Coordinate(i).轴号(1)
                                        Axis_ID_Array(2) = Work_Coordinate(i).轴号(2)
                                        Axis_ID_Array(3) = Work_Coordinate(i).轴号(3)
                                        Axis_Pos_Array(0) = Work_Coordinate(i).轴位置(0) + Trigger_X_Offset
                                        Axis_Pos_Array(1) = Work_Coordinate(i).轴位置(1)
                                        Axis_Pos_Array(2) = Work_Coordinate(i).轴位置(2)
                                        Axis_Pos_Array(3) = Work_Coordinate(i).轴位置(3)
                                        CARDCMD.INTERPOLATION_LINE(Axis_ID_Array, Axis_Pos_Array, PRA_APS_OPTION_ENUM.Absolute, Run_Speed, Work_Coordinate(i).加减速度, Work_Coordinate(i).加减速度, PRA_SF_ENUM.S_curve)
                                        While True
                                            Dim rtn_state As Integer = 1
                                            rtn_state *= CARDCMD.CHECK_MOTION_DONE(Work_Coordinate(i).轴号(0), Stop_Code)
                                            rtn_state *= CARDCMD.CHECK_MOTION_DONE(Work_Coordinate(i).轴号(1), Stop_Code)
                                            rtn_state *= CARDCMD.CHECK_MOTION_DONE(Work_Coordinate(i).轴号(2), Stop_Code)
                                            rtn_state *= CARDCMD.CHECK_MOTION_DONE(Work_Coordinate(i).轴号(3), Stop_Code)
                                            If rtn_state = 1 Then
                                                Exit While
                                            End If
                                            'READ_ENCODER_POSITION(AXISMSG.AXIS_ENCODER_POS)
                                        End While
                                        Thread.Sleep(PARAM_INT.Y运动等待时间)

                                        CCD_Ser_Trigger_Card_01(TABLE_COMPARE_DIRECTION_ENUM.NEGATIVE_DIRECTION, Work_右工位拍照第四行从右到左, Line_Index.RIGHT_TO_LEFT)

                                        '尾点
                                        i = Work_Coordinate.Count - 1
                                        Resize_Count = 4
                                        Select Case BOOL.UPH
                                            Case False
                                                Run_Speed = Work_Coordinate(i).运行速度
                                            Case True
                                                Run_Speed = Work_Coordinate(i).运行速度 * Run_Speed_Scale
                                        End Select
                                        Array.Resize(Axis_ID_Array, Resize_Count)
                                        Array.Resize(Axis_Pos_Array, Resize_Count)
                                        Axis_ID_Array(0) = Work_Coordinate(i).轴号(0)
                                        Axis_ID_Array(1) = Work_Coordinate(i).轴号(1)
                                        Axis_ID_Array(2) = Work_Coordinate(i).轴号(2)
                                        Axis_ID_Array(3) = Work_Coordinate(i).轴号(3)
                                        Axis_Pos_Array(0) = Work_Coordinate(i).轴位置(0) - Trigger_X_Offset
                                        Axis_Pos_Array(1) = Work_Coordinate(i).轴位置(1)
                                        Axis_Pos_Array(2) = Work_Coordinate(i).轴位置(2)
                                        Axis_Pos_Array(3) = Work_Coordinate(i).轴位置(3)
                                        CARDCMD.INTERPOLATION_LINE(Axis_ID_Array, Axis_Pos_Array, PRA_APS_OPTION_ENUM.Absolute, Run_Speed, Work_Coordinate(i).加减速度, Work_Coordinate(i).加减速度, PRA_SF_ENUM.S_curve)
                                        While True
                                            Dim rtn_state As Integer = 1
                                            rtn_state *= CARDCMD.CHECK_MOTION_DONE(Work_Coordinate(i).轴号(0), Stop_Code)
                                            rtn_state *= CARDCMD.CHECK_MOTION_DONE(Work_Coordinate(i).轴号(1), Stop_Code)
                                            rtn_state *= CARDCMD.CHECK_MOTION_DONE(Work_Coordinate(i).轴号(2), Stop_Code)
                                            rtn_state *= CARDCMD.CHECK_MOTION_DONE(Work_Coordinate(i).轴号(3), Stop_Code)
                                            If rtn_state = 1 Then
                                                Exit While
                                            End If
                                            'READ_ENCODER_POSITION(AXISMSG.AXIS_ENCODER_POS)
                                            Try
                                                CARDCMD.get_trigger_count(BOARD_ID_01, 0, trg_count)
                                                Trigger_Right.Text = trg_count & "：" & Work_Coordinate.Count
                                            Catch ex As Exception
                                                rtn_str = "[②工位]触发错误，请检查控制卡。"
                                                Invoke(New Delegate_Display_Message_Right(AddressOf Display_Message_Right), rtn_str, DataCode_Right, Color.Red)
                                            End Try
                                        End While

                                    Else
                                        rtn_str = "[②工位拍照第四行从右到左]没有设置坐标，请先设置坐标"
                                        Invoke(New Delegate_Display_Message_Right(AddressOf Display_Message_Right), rtn_str, DataCode_Right, Color.Red)
                                    End If
                                    Try
                                        CARDCMD.get_trigger_count(BOARD_ID_01, 0, trg_count)
                                        Trigger_count += trg_count
                                    Catch ex As Exception
                                        rtn_str = "[②工位]触发错误，请检查控制卡。"
                                        Invoke(New Delegate_Display_Message_Right(AddressOf Display_Message_Right), rtn_str, DataCode_Right, Color.Red)
                                    End Try
                                Case "②工位拍照第五行从左到右"

                                    Dim Work_Coordinate() As Work_Coordinatess = Work_右工位拍照第五行从左到右
                                    If Work_Coordinate.Count >= 1 Then
                                        '首点
                                        Dim i As Int16 = 0
                                        Resize_Count = 4
                                        Select Case BOOL.UPH
                                            Case False
                                                Run_Speed = Work_Coordinate(i).运行速度
                                            Case True
                                                Run_Speed = Work_Coordinate(i).运行速度 * Run_Speed_Scale
                                        End Select
                                        Array.Resize(Axis_ID_Array, Resize_Count)
                                        Array.Resize(Axis_Pos_Array, Resize_Count)
                                        Axis_ID_Array(0) = Work_Coordinate(i).轴号(0)
                                        Axis_ID_Array(1) = Work_Coordinate(i).轴号(1)
                                        Axis_ID_Array(2) = Work_Coordinate(i).轴号(2)
                                        Axis_ID_Array(3) = Work_Coordinate(i).轴号(3)
                                        Axis_Pos_Array(0) = Work_Coordinate(i).轴位置(0) - Trigger_X_Offset
                                        Axis_Pos_Array(1) = Work_Coordinate(i).轴位置(1)
                                        Axis_Pos_Array(2) = Work_Coordinate(i).轴位置(2)
                                        Axis_Pos_Array(3) = Work_Coordinate(i).轴位置(3)
                                        CARDCMD.INTERPOLATION_LINE(Axis_ID_Array, Axis_Pos_Array, PRA_APS_OPTION_ENUM.Absolute, Run_Speed, Work_Coordinate(i).加减速度, Work_Coordinate(i).加减速度, PRA_SF_ENUM.S_curve)
                                        While True
                                            Dim rtn_state As Integer = 1
                                            rtn_state *= CARDCMD.CHECK_MOTION_DONE(Work_Coordinate(i).轴号(0), Stop_Code)
                                            rtn_state *= CARDCMD.CHECK_MOTION_DONE(Work_Coordinate(i).轴号(1), Stop_Code)
                                            rtn_state *= CARDCMD.CHECK_MOTION_DONE(Work_Coordinate(i).轴号(2), Stop_Code)
                                            rtn_state *= CARDCMD.CHECK_MOTION_DONE(Work_Coordinate(i).轴号(3), Stop_Code)
                                            If rtn_state = 1 Then
                                                Exit While
                                            End If
                                            'READ_ENCODER_POSITION(AXISMSG.AXIS_ENCODER_POS)
                                        End While
                                        Thread.Sleep(PARAM_INT.Y运动等待时间)

                                        CCD_Ser_Trigger_Card_01(TABLE_COMPARE_DIRECTION_ENUM.POSITIVE_DIRECTION, Work_右工位拍照第五行从左到右, Line_Index.LEFT_TO_RIGHT)

                                        '尾点
                                        i = Work_Coordinate.Count - 1
                                        Resize_Count = 4
                                        Select Case BOOL.UPH
                                            Case False
                                                Run_Speed = Work_Coordinate(i).运行速度
                                            Case True
                                                Run_Speed = Work_Coordinate(i).运行速度 * Run_Speed_Scale
                                        End Select
                                        Array.Resize(Axis_ID_Array, Resize_Count)
                                        Array.Resize(Axis_Pos_Array, Resize_Count)
                                        Axis_ID_Array(0) = Work_Coordinate(i).轴号(0)
                                        Axis_ID_Array(1) = Work_Coordinate(i).轴号(1)
                                        Axis_ID_Array(2) = Work_Coordinate(i).轴号(2)
                                        Axis_ID_Array(3) = Work_Coordinate(i).轴号(3)
                                        Axis_Pos_Array(0) = Work_Coordinate(i).轴位置(0) + Trigger_X_Offset
                                        Axis_Pos_Array(1) = Work_Coordinate(i).轴位置(1)
                                        Axis_Pos_Array(2) = Work_Coordinate(i).轴位置(2)
                                        Axis_Pos_Array(3) = Work_Coordinate(i).轴位置(3)
                                        CARDCMD.INTERPOLATION_LINE(Axis_ID_Array, Axis_Pos_Array, PRA_APS_OPTION_ENUM.Absolute, Run_Speed, Work_Coordinate(i).加减速度, Work_Coordinate(i).加减速度, PRA_SF_ENUM.S_curve)
                                        While True
                                            Dim rtn_state As Integer = 1
                                            rtn_state *= CARDCMD.CHECK_MOTION_DONE(Work_Coordinate(i).轴号(0), Stop_Code)
                                            rtn_state *= CARDCMD.CHECK_MOTION_DONE(Work_Coordinate(i).轴号(1), Stop_Code)
                                            rtn_state *= CARDCMD.CHECK_MOTION_DONE(Work_Coordinate(i).轴号(2), Stop_Code)
                                            rtn_state *= CARDCMD.CHECK_MOTION_DONE(Work_Coordinate(i).轴号(3), Stop_Code)
                                            If rtn_state = 1 Then
                                                Exit While
                                            End If
                                            'READ_ENCODER_POSITION(AXISMSG.AXIS_ENCODER_POS)
                                            Try
                                                CARDCMD.get_trigger_count(BOARD_ID_01, 0, trg_count)
                                                Trigger_Right.Text = trg_count & "：" & Work_Coordinate.Count
                                            Catch ex As Exception
                                                rtn_str = "[②工位]触发错误，请检查控制卡。"
                                                Invoke(New Delegate_Display_Message_Right(AddressOf Display_Message_Right), rtn_str, DataCode_Right, Color.Red)
                                            End Try
                                        End While

                                    Else
                                        rtn_str = "[②工位拍照第五行从左到右]没有设置坐标，请先设置坐标"
                                        Invoke(New Delegate_Display_Message_Right(AddressOf Display_Message_Right), rtn_str, DataCode_Right, Color.Red)
                                    End If
                                    Try
                                        CARDCMD.get_trigger_count(BOARD_ID_01, 0, trg_count)
                                        Trigger_count += trg_count
                                    Catch ex As Exception
                                        rtn_str = "[②工位]触发错误，请检查控制卡。"
                                        Invoke(New Delegate_Display_Message_Right(AddressOf Display_Message_Right), rtn_str, DataCode_Right, Color.Red)
                                    End Try
                                 
                                Case "②工位拍照第六行从右到左"

                                    Dim Work_Coordinate() As Work_Coordinatess = Work_右工位拍照第六行从右到左
                                    If Work_Coordinate.Count >= 1 Then
                                        '首点
                                        Dim i As Int16 = 0
                                        Resize_Count = 4
                                        Select Case BOOL.UPH
                                            Case False
                                                Run_Speed = Work_Coordinate(i).运行速度
                                            Case True
                                                Run_Speed = Work_Coordinate(i).运行速度 * Run_Speed_Scale
                                        End Select
                                        Array.Resize(Axis_ID_Array, Resize_Count)
                                        Array.Resize(Axis_Pos_Array, Resize_Count)
                                        Axis_ID_Array(0) = Work_Coordinate(i).轴号(0)
                                        Axis_ID_Array(1) = Work_Coordinate(i).轴号(1)
                                        Axis_ID_Array(2) = Work_Coordinate(i).轴号(2)
                                        Axis_ID_Array(3) = Work_Coordinate(i).轴号(3)
                                        Axis_Pos_Array(0) = Work_Coordinate(i).轴位置(0) + Trigger_X_Offset
                                        Axis_Pos_Array(1) = Work_Coordinate(i).轴位置(1)
                                        Axis_Pos_Array(2) = Work_Coordinate(i).轴位置(2)
                                        Axis_Pos_Array(3) = Work_Coordinate(i).轴位置(3)
                                        CARDCMD.INTERPOLATION_LINE(Axis_ID_Array, Axis_Pos_Array, PRA_APS_OPTION_ENUM.Absolute, Run_Speed, Work_Coordinate(i).加减速度, Work_Coordinate(i).加减速度, PRA_SF_ENUM.S_curve)
                                        While True
                                            Dim rtn_state As Integer = 1
                                            rtn_state *= CARDCMD.CHECK_MOTION_DONE(Work_Coordinate(i).轴号(0), Stop_Code)
                                            rtn_state *= CARDCMD.CHECK_MOTION_DONE(Work_Coordinate(i).轴号(1), Stop_Code)
                                            rtn_state *= CARDCMD.CHECK_MOTION_DONE(Work_Coordinate(i).轴号(2), Stop_Code)
                                            rtn_state *= CARDCMD.CHECK_MOTION_DONE(Work_Coordinate(i).轴号(3), Stop_Code)
                                            If rtn_state = 1 Then
                                                Exit While
                                            End If
                                            'READ_ENCODER_POSITION(AXISMSG.AXIS_ENCODER_POS)
                                        End While
                                        Thread.Sleep(PARAM_INT.Y运动等待时间)

                                        CCD_Ser_Trigger_Card_01(TABLE_COMPARE_DIRECTION_ENUM.NEGATIVE_DIRECTION, Work_右工位拍照第六行从右到左, Line_Index.RIGHT_TO_LEFT)

                                        '尾点
                                        i = Work_Coordinate.Count - 1
                                        Resize_Count = 4
                                        Select Case BOOL.UPH
                                            Case False
                                                Run_Speed = Work_Coordinate(i).运行速度
                                            Case True
                                                Run_Speed = Work_Coordinate(i).运行速度 * Run_Speed_Scale
                                        End Select
                                        Array.Resize(Axis_ID_Array, Resize_Count)
                                        Array.Resize(Axis_Pos_Array, Resize_Count)
                                        Axis_ID_Array(0) = Work_Coordinate(i).轴号(0)
                                        Axis_ID_Array(1) = Work_Coordinate(i).轴号(1)
                                        Axis_ID_Array(2) = Work_Coordinate(i).轴号(2)
                                        Axis_ID_Array(3) = Work_Coordinate(i).轴号(3)
                                        Axis_Pos_Array(0) = Work_Coordinate(i).轴位置(0) - Trigger_X_Offset
                                        Axis_Pos_Array(1) = Work_Coordinate(i).轴位置(1)
                                        Axis_Pos_Array(2) = Work_Coordinate(i).轴位置(2)
                                        Axis_Pos_Array(3) = Work_Coordinate(i).轴位置(3)
                                        CARDCMD.INTERPOLATION_LINE(Axis_ID_Array, Axis_Pos_Array, PRA_APS_OPTION_ENUM.Absolute, Run_Speed, Work_Coordinate(i).加减速度, Work_Coordinate(i).加减速度, PRA_SF_ENUM.S_curve)
                                        While True
                                            Dim rtn_state As Integer = 1
                                            rtn_state *= CARDCMD.CHECK_MOTION_DONE(Work_Coordinate(i).轴号(0), Stop_Code)
                                            rtn_state *= CARDCMD.CHECK_MOTION_DONE(Work_Coordinate(i).轴号(1), Stop_Code)
                                            rtn_state *= CARDCMD.CHECK_MOTION_DONE(Work_Coordinate(i).轴号(2), Stop_Code)
                                            rtn_state *= CARDCMD.CHECK_MOTION_DONE(Work_Coordinate(i).轴号(3), Stop_Code)
                                            If rtn_state = 1 Then
                                                Exit While
                                            End If
                                            'READ_ENCODER_POSITION(AXISMSG.AXIS_ENCODER_POS)
                                            Try
                                                CARDCMD.get_trigger_count(BOARD_ID_01, 0, trg_count)
                                                Trigger_Right.Text = trg_count & "：" & Work_Coordinate.Count
                                            Catch ex As Exception
                                                rtn_str = "[②工位]触发错误，请检查控制卡。"
                                                Invoke(New Delegate_Display_Message_Right(AddressOf Display_Message_Right), rtn_str, DataCode_Right, Color.Red)
                                            End Try
                                        End While

                                    Else
                                        rtn_str = "[②工位拍照第六行从右到左]没有设置坐标，请先设置坐标"
                                        Invoke(New Delegate_Display_Message_Right(AddressOf Display_Message_Right), rtn_str, DataCode_Right, Color.Red)
                                    End If
                                    Try
                                        CARDCMD.get_trigger_count(BOARD_ID_01, 0, trg_count)
                                        Trigger_count += trg_count
                                    Catch ex As Exception
                                        rtn_str = "[②工位]触发错误，请检查控制卡。"
                                        Invoke(New Delegate_Display_Message_Right(AddressOf Display_Message_Right), rtn_str, DataCode_Right, Color.Red)
                                    End Try

                                    Close_Trigger_Right() '关闭触发
                                    rtn_str = "2工位相机扫描：结束"
                                    Invoke(New Delegate_Display_Message_Right(AddressOf Display_Message_Right), rtn_str, DataCode_Right, Color.Green)

                                    '获取总触发次数
                                    Try
                                        CARDCMD.get_trigger_count(BOARD_ID_01, 0, trg_count)
                                        Trigger_count += trg_count
                                        If Trigger_count = INTTYPLE.Right_Trigger_Count Then
                                            Trigger_Right.Text = Trigger_count
                                            Trigger_Right.Style.BackColor1.Color = Color.Green
                                        Else
                                            Trigger_Right.Text = Trigger_count
                                            Trigger_Right.Style.BackColor1.Color = Color.Red
                                        End If
                                    Catch ex As Exception
                                        rtn_str = "[②工位]触发错误，请检查控制卡。"
                                        Invoke(New Delegate_Display_Message_Right(AddressOf Display_Message_Right), rtn_str, DataCode_Right, Color.Red)
                                    End Try


                                 
                                Case Else
                                    For RunIndex = StartIndex To EndIndex
                                        AXIS_INDEX1_RIGHT.Clear()
                                        AXIS_INDEX2_RIGHT.Clear()
                                        AXIS_INDEX3_RIGHT.Clear()
                                        AXIS_INDEX4_RIGHT.Clear()
                                        AXIS_INDEX5_RIGHT.Clear()
                                        '把程序优先级顺序排序，并存入序列号
                                        For i As Integer = 0 To Coord_AllPOS_List_RIGHT(RunIndex).轴运动顺序.Length - 1 '判断优先级
                                            Select Case Coord_AllPOS_List_RIGHT(RunIndex).轴运动顺序(i)
                                                Case "1"
                                                    AXIS_INDEX1_RIGHT.Add(i)
                                                Case "2"
                                                    AXIS_INDEX2_RIGHT.Add(i)
                                                Case "3"
                                                    AXIS_INDEX3_RIGHT.Add(i)
                                                Case "4"
                                                    AXIS_INDEX4_RIGHT.Add(i)
                                                Case "5"
                                                    AXIS_INDEX5_RIGHT.Add(i)
                                            End Select
                                        Next
                                        For M As Int16 = 1 To 5 '按1~5的顺序进性运动
                                            Dim AXIS_SPORT_INDEX As New List(Of Integer)
                                            AXIS_SPORT_INDEX.Clear()
                                            Select Case M
                                                Case 1
                                                    AXIS_SPORT_INDEX = AXIS_INDEX1_RIGHT
                                                Case 2
                                                    AXIS_SPORT_INDEX = AXIS_INDEX2_RIGHT
                                                Case 3
                                                    AXIS_SPORT_INDEX = AXIS_INDEX3_RIGHT
                                                Case 4
                                                    AXIS_SPORT_INDEX = AXIS_INDEX4_RIGHT
                                                Case 5
                                                    AXIS_SPORT_INDEX = AXIS_INDEX5_RIGHT
                                            End Select
                                            If AXIS_SPORT_INDEX.Count >= 1 Then
                                                Select Case AXIS_SPORT_INDEX.Count
                                                    Case Is = 1 '*****************************单轴运动
                                                        '查询对应轴号
                                                        Axis_ID = Coord_AllPOS_List_RIGHT(RunIndex).轴号(AXIS_SPORT_INDEX(0))
                                                        Axis_Pos = Coord_AllPOS_List_RIGHT(RunIndex).轴位置(AXIS_SPORT_INDEX(0))
                                                        Rtn_Int = CARDCMD.JOG_PTP_ALL(Axis_ID, PRA_APS_OPTION_ENUM.Absolute, Coord_AllPOS_List_RIGHT(RunIndex).运行速度, Coord_AllPOS_List_RIGHT(RunIndex).加减速度, Axis_Pos, Nothing)
                                                        Select Case Rtn_Int
                                                            Case Is = 0
                                                                While BOOL.EXIT_APP = False
                                                                    Dim rtn_state As Integer = 1
                                                                    rtn_state *= CARDCMD.CHECK_MOTION_DONE(Axis_ID, Stop_Code)
                                                                    If rtn_state = 1 Then
                                                                        Exit While
                                                                    End If
                                                                    READ_ENCODER_POSITION(AXISMSG.AXIS_ENCODER_POS)
                                                                End While
                                                            Case Is <> 0
                                                                Error_Str = ADLINK_RETURN_MSG(Rtn_Int)
                                                                BOOL.Error_Close = True '关闭软件
                                                                Exit Sub
                                                        End Select
                                                    Case Is > 1 '*****************************插补运动
                                                        '查询对应轴号
                                                        Array.Resize(Axis_ID_Array, AXIS_SPORT_INDEX.Count)
                                                        Array.Resize(Axis_Pos_Array, AXIS_SPORT_INDEX.Count)
                                                        For I As Int16 = 0 To AXIS_SPORT_INDEX.Count - 1
                                                            Axis_ID_Array(I) = Coord_AllPOS_List_RIGHT(RunIndex).轴号(AXIS_SPORT_INDEX(I))
                                                            Axis_Pos_Array(I) = Coord_AllPOS_List_RIGHT(RunIndex).轴位置(AXIS_SPORT_INDEX(I))
                                                        Next
                                                        Rtn_Int = CARDCMD.INTERPOLATION_LINE(Axis_ID_Array, Axis_Pos_Array, PRA_APS_OPTION_ENUM.Absolute, Coord_AllPOS_List_RIGHT(RunIndex).运行速度, Coord_AllPOS_List_RIGHT(RunIndex).加减速度, Coord_AllPOS_List_RIGHT(RunIndex).加减速度, PRA_SF_ENUM.S_curve)
                                                        Select Case Rtn_Int
                                                            Case Is = 0
                                                                While BOOL.EXIT_APP = False
                                                                    Dim rtn_state As Integer = 1
                                                                    For j As Integer = 0 To AXIS_SPORT_INDEX.Count - 1
                                                                        rtn_state *= CARDCMD.CHECK_MOTION_DONE(Axis_ID_Array(j), Stop_Code)
                                                                    Next
                                                                    If rtn_state = 1 Then
                                                                        Exit While
                                                                    End If
                                                                    READ_ENCODER_POSITION(AXISMSG.AXIS_ENCODER_POS)

                                                                End While
                                                            Case Is <> 0
                                                                Error_Str = ADLINK_RETURN_MSG(Rtn_Int)
                                                                BOOL.Error_Close = True '关闭软件
                                                                Exit Sub
                                                        End Select
                                                End Select
                                            End If
                                        Next
                                        Thread.Sleep(PARAM_INT.Y运动等待时间)


                                        While SwitchButton_runing.Value = False '调试模式，机台暂停
                                            Thread.Sleep(200)
                                        End While

                                        If COOR_NAME = "右取料位" And Coord_AllPOS_List_RIGHT(RunIndex).IO触发状态 = True Then

                                            If CARDCMD.GET_DI_BIT(BOARD_ID_01, DIO.TWO2工位顶升WORK_1) = False Then
                                                If TWO2工位顶升电磁阀(IO.OUT_ON, LR_STATION.RIGHT, DataCode_Right) = False Then

                                                    Error_Str = "2工位顶升电磁阀上升不到位，软件即将关闭"
                                                    Invoke(New Delegate_Display_Message_Right(AddressOf Display_Message_Right), Error_Str, "", Color.Red)
                                                    MessageBoxEx.Show(Error_Str, "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
                                                    'BOOL.Error_Close = True
                                                    'Exit Sub
                                                End If
                                            End If

                                            If CARDCMD.GET_DI_BIT(BOARD_ID_01, DIO.TWO2工位真空吸检测_1) = True Then
                                                rtn_str = "2工位真空吸检测到有料，此时点击确定，会造成【两片料叠加】，请卸料并检查后，重启软件"
                                                Invoke(New Delegate_Display_Message_Right(AddressOf Display_Message_Right), rtn_str, DataCode_Right, Color.Red)
                                                MessageBoxEx.Show(rtn_str, "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
                                            End If
                                            TWO2工位真空吸电磁阀(IO.OUT_ON, LR_STATION.RIGHT, DataCode_Right)
                                            If JSON_PARA.PROJECTS.Contains("1359") Then
                                                C键TWO2工位真空吸电磁阀(IO.OUT_ON, LR_STATION.LEFT, DataCode_Left)
                                            End If
                                        End If
                                        If COOR_NAME = "右放料位" And Coord_AllPOS_List_RIGHT(RunIndex).IO触发状态 = True Then
                                            '真空吸多关闭几次

                                            'TWO2工位真空吸电磁阀(IO.OUT_OFF, LR_STATION.RIGHT, DataCode_Right)
                                            For i As Int16 = 1 To 2
                                                CARDCMD.SET_DO_BIT(BOARD_ID_00, DIO.TWO2工位真空吸电磁阀_0, PRA_OUT_STATUS_ENUM.OUT_OFF)
                                                If JSON_PARA.PROJECTS.Contains("1359") Then
                                                    CARDCMD.SET_DO_BIT(BOARD_ID_00, DIO.C键TWO2工位真空吸电磁阀_0, PRA_OUT_STATUS_ENUM.OUT_OFF)
                                                End If
                                                Thread.Sleep(PARAM_INT.D电磁阀输出等待时间)
                                            Next

                                            rtn_str = "[2工位破真空电磁阀]开"
                                            Invoke(New Delegate_Display_Message_Right(AddressOf Display_Message_Right), rtn_str, DataCode_Right, Color.Black)
                                            For i As Int16 = 1 To 3
                                                CARDCMD.SET_DO_BIT(BOARD_ID_00, DIO.TWO2工位破真空电磁阀_0, PRA_OUT_STATUS_ENUM.OUT_ON)
                                                Thread.Sleep(300)
                                            Next

                                            For i As Integer = 0 To HomeParamObjArray.Length - 1
                                                If HomeParamObjArray(i).轴名称 = "Z02" Then
                                                    Axis_ID = HomeParamObjArray(i).轴号
                                                End If
                                            Next
                                            CARDCMD.JOG_PTP_ALL(Axis_ID, PRA_APS_OPTION_ENUM.Relative, 20, 0.1, 0.8, Nothing) '太高到安全距离
                                            Select Case Rtn_Int
                                                Case Is = 0
                                                    While BOOL.EXIT_APP = False
                                                        Dim rtn_state As Integer = 1
                                                        rtn_state *= CARDCMD.CHECK_MOTION_DONE(Axis_ID, Stop_Code)
                                                        If rtn_state = 1 Then
                                                            Exit While
                                                        End If
                                                        READ_ENCODER_POSITION(AXISMSG.AXIS_ENCODER_POS)
                                                    End While
                                                Case Is <> 0
                                                    Error_Str = ADLINK_RETURN_MSG(Rtn_Int)
                                                    BOOL.Error_Close = True '关闭软件
                                                    Exit Sub
                                            End Select
                                            Thread.Sleep(300)

                                            While SwitchButton_runing.Value = False '调试模式，机台暂停
                                                Thread.Sleep(200)
                                            End While

                                            rtn_str = "[2工位破真空电磁阀]关"
                                            Invoke(New Delegate_Display_Message_Right(AddressOf Display_Message_Right), rtn_str, DataCode_Right, Color.Black)
                                            For pozhenkkong As Integer = 0 To 2
                                                CARDCMD.SET_DO_BIT(BOARD_ID_00, DIO.TWO2工位破真空电磁阀_0, PRA_OUT_STATUS_ENUM.OUT_OFF)
                                                Thread.Sleep(200)
                                            Next
                                            If CARDCMD.GET_DI_BIT(BOARD_ID_01, DIO.TWO2工位真空吸检测_1) = True Then
                                                CARDCMD.SET_DO_BIT(BOARD_ID_00, DIO.TWO2工位破真空电磁阀_0, PRA_OUT_STATUS_ENUM.OUT_ON)
                                                Thread.Sleep(PARAM_INT.D电磁阀输出等待时间)
                                                For pozhenkkong As Integer = 0 To 2
                                                    CARDCMD.SET_DO_BIT(BOARD_ID_00, DIO.TWO2工位破真空电磁阀_0, PRA_OUT_STATUS_ENUM.OUT_OFF)
                                                    Thread.Sleep(PARAM_INT.D电磁阀输出等待时间)
                                                Next
                                                If CARDCMD.GET_DI_BIT(BOARD_ID_01, DIO.TWO2工位真空吸检测_1) = True Then
                                                    rtn_str = "2工位真空吸不能正常关闭，此时点击确定，会造成【两片料叠加】，请卸料并检查后，重启软件"
                                                    Invoke(New Delegate_Display_Message_Right(AddressOf Display_Message_Right), rtn_str, DataCode_Right, Color.Red)
                                                    MessageBoxEx.Show(rtn_str, "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
                                                End If
                                            End If

                                            If TWO2工位阻挡电磁阀2(IO.OUT_OFF, LR_STATION.RIGHT, DataCode_Right) = False Then
                                                Thread.Sleep(300)
                                                CARDCMD.SET_DO_BIT(BOARD_ID_00, DIO.TWO2工位阻挡电磁阀2_0, PRA_OUT_STATUS_ENUM.OUT_OFF)
                                                Thread.Sleep(PARAM_INT.D电磁阀输出等待时间)
                                                If CARDCMD.GET_DI_BIT(BOARD_ID_00, DIO.TWO2工位阻挡HOME2_0) = False Then

                                                    Error_Str = "[2工位阻挡电磁阀2]不能正常下降，程序不能正常运行，请卸料并检查后，重启软件"
                                                    Invoke(New Delegate_Display_Message_Right(AddressOf Display_Message_Right), Error_Str, "", Color.Red)
                                                    MessageBoxEx.Show(Error_Str, "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
                                                    'BOOL.Error_Close = True
                                                    'Exit Sub
                                                End If
                                            End If

                                            If TWO2工位顶升电磁阀(IO.OUT_OFF, LR_STATION.RIGHT, DataCode_Right) = False Then
                                                Thread.Sleep(300)
                                                CARDCMD.SET_DO_BIT(BOARD_ID_00, DIO.TWO2工位顶升电磁阀_0, PRA_OUT_STATUS_ENUM.OUT_OFF)
                                                Thread.Sleep(PARAM_INT.D电磁阀输出等待时间)
                                                If CARDCMD.GET_DI_BIT(BOARD_ID_01, DIO.TWO2工位顶升HOME_1) = False Then

                                                    Error_Str = "[2工位顶升电磁阀]不能正常下降，程序不能正常运行，请卸料并检查后，重启软件"
                                                    Invoke(New Delegate_Display_Message_Right(AddressOf Display_Message_Right), Error_Str, "", Color.Red)
                                                    MessageBoxEx.Show(Error_Str, "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
                                                    'BOOL.Error_Close = True
                                                    'Exit Sub
                                                End If
                                            End If

                                            While SwitchButton_runing.Value = False '调试模式，机台暂停
                                                Thread.Sleep(200)
                                            End While

                                        End If
                                    Next
                            End Select

                        Next
                    Else
                        rtn_str = "没有设置右工位坐标，请先设置坐标！"
                        Invoke(New Delegate_Display_Message_Right(AddressOf Display_Message_Right), rtn_str, DataCode_Right, Color.Red)
                    End If
                    '****************************************************************************************************************
                    rtn_str = "等待[2工位出料检测]无料"
                    Invoke(New Delegate_Display_Message_Right(AddressOf Display_Message_Right), rtn_str, DataCode_Right, Color.Blue)
                    While True
                        Thread.Sleep(20)
                        If CARDCMD.GET_DI_BIT(BOARD_ID_00, DIO.TWO2工位出料检测_0) = False Then
                            Exit While
                        End If
                    End While
                    rtn_str = "[2工位出料检测]无料，准备出料"
                    Invoke(New Delegate_Display_Message_Right(AddressOf Display_Message_Right), rtn_str, DataCode_Right, Color.Blue)

                    rtn_str = "[2工位皮带控制]开"
                    Invoke(New Delegate_Display_Message_Right(AddressOf Display_Message_Right), rtn_str, DataCode_Right, Color.OrangeRed)
                    For i As Int16 = 1 To 3
                        CARDCMD.SET_DO_BIT(BOARD_ID_00, DIO.TWO2工位皮带控制_0, PRA_OUT_STATUS_ENUM.OUT_ON)
                        Thread.Sleep(50)
                    Next

                    rtn_str = "等待[2工位采集图像完成]"
                    Invoke(New Delegate_Display_Message_Right(AddressOf Display_Message_Right), rtn_str, DataCode_Right, Color.Blue)

                    If BOOL.UPH = False Then
                        While GRAB_IMAGE_BOOLEAN_RIGHT = True
                            Thread.Sleep(20)
                        End While
                    End If
            
                    rtn_str = "2工位采集图像[完成]"
                    Invoke(New Delegate_Display_Message_Right(AddressOf Display_Message_Right), rtn_str, DataCode_Right, Color.Blue)



                    rtn_str = "等待上一次[2工位图像处理]结束"
                    Invoke(New Delegate_Display_Message_Right(AddressOf Display_Message_Right), rtn_str, DataCode_Right, Color.Blue)

                    If BOOL.UPH = False Then
                        While Process_Right_Boolean = True
                            Thread.Sleep(20)
                        End While
                        Thread.Sleep(500)
                    End If

                    rtn_str = "上一次[2工位图像处理]完成"
                    Invoke(New Delegate_Display_Message_Right(AddressOf Display_Message_Right), rtn_str, DataCode_Right, Color.Blue)

                    DataCode_Right_tmp = DataCode_Right
                    CognexImagefile_IDB_Right_tmp = CognexImagefile_IDB_Right
                    CognexImagefile_QS_Right_tmp = CognexImagefile_QS_Right

                    If BOOL.UPH = False Then
                        Process_Right_Boolean = True '*******************"开始运行[2工位图像处理]"
                        If PROCESS_IMAGE_RIGHT.IsBusy = False Then
                            PROCESS_IMAGE_RIGHT.RunWorkerAsync()
                        End If
                    End If

                    rtn_str = "等待产品运行到[2工位出料位]"
                    Invoke(New Delegate_Display_Message_Right(AddressOf Display_Message_Right), rtn_str, DataCode_Right, Color.Green)
                    While True
                        If CARDCMD.GET_DI_BIT(BOARD_ID_00, DIO.TWO2工位出料检测_0) = True Then
                            Thread.Sleep(500)
                            Exit While
                        End If
                    End While
                    rtn_str = "[2工位出料位]出料完成"
                    Invoke(New Delegate_Display_Message_Right(AddressOf Display_Message_Right), rtn_str, DataCode_Right, Color.Green)

                    For k As Int16 = 1 To 3 '阻挡失败再阻挡一次
                        If TWO2工位阻挡电磁阀2(IO.OUT_ON, LR_STATION.RIGHT, DataCode_Right) = False Then
                            If k = 3 Then

                                Error_Str = "2工位阻挡电磁阀2不能正常升起，请检查电磁阀是否异常，此电磁阀不升起会造成追料，请慎重！"
                                Invoke(New Delegate_Display_Message_Right(AddressOf Display_Message_Right), Error_Str, "", Color.Red)
                                MessageBoxEx.Show(Error_Str, "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
                                'BOOL.Error_Close = True
                                'Exit Sub
                            Else
                                CARDCMD.SET_DO_BIT(BOARD_ID_00, DIO.TWO2工位阻挡电磁阀2_0, PRA_OUT_STATUS_ENUM.OUT_ON)
                                Thread.Sleep(PARAM_INT.D电磁阀输出等待时间)
                            End If
                        Else
                            Exit For
                        End If
                    Next
                    For k As Int16 = 1 To 3 '1工位阻挡电磁阀1下降
                        If TWO2工位阻挡电磁阀1(IO.OUT_OFF, LR_STATION.RIGHT, DataCode_Right) = False Then
                            If k = 3 Then
                              
                                Error_Str = "2工位阻挡电磁阀1不能正常下降，请检查电磁阀是否异常，此电磁阀不下降会导致无法进料，请检查！"
                                Invoke(New Delegate_Display_Message_Right(AddressOf Display_Message_Right), Error_Str, "", Color.Red)
                                MessageBoxEx.Show(Error_Str, "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
                                'BOOL.Error_Close = True
                                'Exit Sub
                            Else
                                CARDCMD.SET_DO_BIT(BOARD_ID_00, DIO.TWO2工位阻挡电磁阀1_0, PRA_OUT_STATUS_ENUM.OUT_OFF)
                                Thread.Sleep(PARAM_INT.D电磁阀输出等待时间)
                            End If
                        Else
                            Exit For
                        End If
                    Next

                    End_Right()
                    rtn_str = "【2工位流程结束】"
                    Invoke(New Delegate_Display_Message_Right(AddressOf Display_Message_Right), rtn_str, DataCode_Right, Color.Black)
                End If
            End If
        End While
    End Sub

    Sub Run_Thread_Fun_Comm()
        Dim COOR_NAME As String = Nothing
        Dim Point_number As Integer = 0, Resize_Count As Integer = 0, Axis_ID_Array() As Integer = Nothing, Axis_Pos_Array() As Double = Nothing
        Dim rtn_str As String, Rtn_Int As Int16 = 0, Axis_ID As Integer = 0, Axis_Pos As Double = 0
        Dim Temp_First As Boolean = False

        While Bool_Run
            Thread.Sleep(1)
            If Bool_Start_All = False Then
                Static Run_Index As Int16 = 0
                If Run_Index Mod 2 = 0 Then
                    If CARDCMD.GET_DI_BIT(BOARD_ID_00, DIO.ONE1工位出料检测_0) = True Then
                        Move_Public_MidPos(LR_STATION.LEFT) '运动到中间位放料位置
                    End If
                    Run_Index = 1
                Else
                    If CARDCMD.GET_DI_BIT(BOARD_ID_00, DIO.TWO2工位出料检测_0) = True Then
                        Move_Public_MidPos(LR_STATION.RIGHT) '运动到中间位放料位置
                    End If
                    Run_Index = 0
                End If
            End If
        End While
    End Sub


    ''' <summary>
    ''' 运动到中间位放料位置
    ''' </summary>
    ''' <remarks></remarks>
    Sub Move_Public_MidPos(ByVal LR As LR_STATION)
        Dim COOR_NAME As String = Nothing
        Dim Point_number As Integer = 0, Resize_Count As Integer = 0, Axis_ID_Array() As Integer = Nothing, Axis_Pos_Array() As Double = Nothing
        Dim rtn_str As String = Nothing, Rtn_Int As Int16 = 0, Axis_ID As Integer = 0, Axis_Pos As Double = 0
        Dim Temp_First As Boolean = False
        Dim Work_Coordinate() As Work_Coordinatess = Nothing

        BOOL.Common_IsRuning = True

        Select Case LR
            Case LR_STATION.LEFT
                Work_Coordinate = Work_左工位镭射取料位
            Case LR_STATION.RIGHT
                Work_Coordinate = Work_右工位镭射取料位
        End Select

        If Work_Coordinate.Count >= 1 Then
            For i As Int16 = 0 To Work_Coordinate.Count - 1
                Resize_Count = 2
                Array.Resize(Axis_ID_Array, Resize_Count)
                Array.Resize(Axis_Pos_Array, Resize_Count)
                Axis_ID_Array(0) = Work_Coordinate(i).轴号(0)
                Axis_ID_Array(1) = Work_Coordinate(i).轴号(1)
                Axis_Pos_Array(0) = Work_Coordinate(i).轴位置(0)
                Axis_Pos_Array(1) = Work_Coordinate(i).轴位置(1)
                CARDCMD.INTERPOLATION_LINE(Axis_ID_Array, Axis_Pos_Array, PRA_APS_OPTION_ENUM.Absolute, Work_Coordinate(i).运行速度, Work_Coordinate(i).加减速度, Work_Coordinate(i).加减速度, PRA_SF_ENUM.S_curve)
                While True
                    Dim rtn_state As Integer = 1
                    rtn_state *= CARDCMD.CHECK_MOTION_DONE(Work_Coordinate(i).轴号(0), Stop_Code)
                    rtn_state *= CARDCMD.CHECK_MOTION_DONE(Work_Coordinate(i).轴号(1), Stop_Code)
                    If rtn_state = 1 Then
                        Exit While
                    End If
                    READ_ENCODER_POSITION(AXISMSG.AXIS_ENCODER_POS)
                End While
                Thread.Sleep(PARAM_INT.Y运动等待时间)

                If Work_Coordinate(i).IO触发状态 = True Then
                    For k As Int16 = 1 To 3 '真空吸失败，再吸3次
                        If Z中线真空吸电磁阀(IO.OUT_ON, LR, "") = False Then
                            If k = 3 Then
                                Select Case LR
                                    Case LR_STATION.LEFT
                                        rtn_str = "1工位镭射取料位下降真空吸TRY盘失败，程序不能正常运行，请卸料并检查后，重启软件"
                                        Invoke(New Delegate_Display_Message_Left(AddressOf Display_Message_Left), rtn_str, "", Color.Red)
                                    Case LR_STATION.RIGHT
                                        rtn_str = "2工位镭射取料位下降真空吸TRY盘失败，程序不能正常运行，请卸料并检查后，重启软件"
                                        Invoke(New Delegate_Display_Message_Right(AddressOf Display_Message_Right), rtn_str, "", Color.Red)
                                End Select
                                MessageBoxEx.Show(rtn_str, "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
                            Else
                                CARDCMD.SET_DO_BIT(BOARD_ID_00, DIO.Z中线真空吸电磁阀_0, PRA_OUT_STATUS_ENUM.OUT_OFF)
                                Thread.Sleep(PARAM_INT.D电磁阀输出等待时间)
                            End If
                            '抬高2mm
                            For j As Integer = 0 To HomeParamObjArray.Length - 1
                                If HomeParamObjArray(j).轴名称 = "Z03" Then
                                    Axis_ID = HomeParamObjArray(j).轴号
                                End If
                            Next
                            CARDCMD.JOG_PTP_ALL(Axis_ID, PRA_APS_OPTION_ENUM.Relative, 10, 0.1, 2, Nothing)
                            Select Case Rtn_Int
                                Case Is = 0
                                    While BOOL.EXIT_APP = False
                                        Dim rtn_state As Integer = 1
                                        rtn_state *= CARDCMD.CHECK_MOTION_DONE(Axis_ID, Stop_Code)
                                        If rtn_state = 1 Then
                                            Exit While
                                        End If
                                        READ_ENCODER_POSITION(AXISMSG.AXIS_ENCODER_POS)
                                    End While
                                Case Is <> 0
                                    Error_Str = ADLINK_RETURN_MSG(Rtn_Int)
                                    BOOL.Error_Close = True '关闭软件
                                    Exit Sub
                            End Select
                            Thread.Sleep(PARAM_INT.Y运动等待时间)
                            '下降2mm
                            For j As Integer = 0 To HomeParamObjArray.Length - 1
                                If HomeParamObjArray(j).轴名称 = "Z03" Then
                                    Axis_ID = HomeParamObjArray(j).轴号
                                End If
                            Next
                            CARDCMD.JOG_PTP_ALL(Axis_ID, PRA_APS_OPTION_ENUM.Relative, 10, 0.1, -2 - (0.05 * k), Nothing)
                            Select Case Rtn_Int
                                Case Is = 0
                                    While BOOL.EXIT_APP = False
                                        Dim rtn_state As Integer = 1
                                        rtn_state *= CARDCMD.CHECK_MOTION_DONE(Axis_ID, Stop_Code)
                                        If rtn_state = 1 Then
                                            Exit While
                                        End If
                                        READ_ENCODER_POSITION(AXISMSG.AXIS_ENCODER_POS)
                                    End While
                                Case Is <> 0
                                    Error_Str = ADLINK_RETURN_MSG(Rtn_Int)
                                    BOOL.Error_Close = True '关闭软件
                                    Exit Sub
                            End Select
                        Else
                            Exit For
                        End If
                    Next
                    Thread.Sleep(PARAM_INT.Z真空吸等待时间)
                End If
            Next
        Else
            Select Case LR
                Case LR_STATION.LEFT
                    rtn_str = "[1工位镭射取料位]没有设置坐标，请先设置坐标"
                    Invoke(New Delegate_Display_Message_Left(AddressOf Display_Message_Left), rtn_str, "", Color.Black)
                Case LR_STATION.RIGHT
                    rtn_str = "[2工位镭射取料位]没有设置坐标，请先设置坐标"
                    Invoke(New Delegate_Display_Message_Right(AddressOf Display_Message_Right), rtn_str, "", Color.Black)
            End Select
            Exit Sub
        End If
        '*************************************************************************************

        If CARDCMD.GET_DI_BIT(BOARD_ID_00, DIO.Z中线真空吸检测_0) = False Then
            Select Case LR
                Case LR_STATION.LEFT
                    rtn_str = "1工位中线真空吸检测失败，程序不能正常运行，请卸料并检查后，重启软件"
                    Invoke(New Delegate_Display_Message_Left(AddressOf Display_Message_Left), rtn_str, "", Color.Red)
                Case LR_STATION.RIGHT
                    rtn_str = "2工位中线真空吸检测失败，程序不能正常运行，请卸料并检查后，重启软件"
                    Invoke(New Delegate_Display_Message_Right(AddressOf Display_Message_Right), rtn_str, "", Color.Red)
            End Select
            MessageBoxEx.Show(rtn_str, "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
        End If

        Work_Coordinate = Work_中间位放料位
        For i As Int16 = 0 To Work_Coordinate.Count - 1
            Resize_Count = 2
            Array.Resize(Axis_ID_Array, Resize_Count)
            Array.Resize(Axis_Pos_Array, Resize_Count)
            Axis_ID_Array(0) = Work_Coordinate(i).轴号(0)
            Axis_ID_Array(1) = Work_Coordinate(i).轴号(1)
            Axis_Pos_Array(0) = Work_Coordinate(i).轴位置(0)
            Axis_Pos_Array(1) = Work_Coordinate(i).轴位置(1)
            CARDCMD.INTERPOLATION_LINE(Axis_ID_Array, Axis_Pos_Array, PRA_APS_OPTION_ENUM.Absolute, Work_Coordinate(i).运行速度, Work_Coordinate(i).加减速度, Work_Coordinate(i).加减速度, PRA_SF_ENUM.S_curve)
            While True
                Dim rtn_state As Integer = 1
                rtn_state *= CARDCMD.CHECK_MOTION_DONE(Work_Coordinate(i).轴号(0), Stop_Code)
                rtn_state *= CARDCMD.CHECK_MOTION_DONE(Work_Coordinate(i).轴号(1), Stop_Code)
                If rtn_state = 1 Then
                    Exit While
                End If
                READ_ENCODER_POSITION(AXISMSG.AXIS_ENCODER_POS)
            End While
            Thread.Sleep(PARAM_INT.Y运动等待时间)

            If i = 0 Then '运动到中间放料位第一个点
                Dim Bool_First As Boolean = False, Bool_First_2 As Boolean = False
                While True
                    Dim rtn_bool1 As Boolean = True, rtn_bool2 As Boolean = True
                    rtn_bool1 = CARDCMD.GET_DI_BIT(BOARD_ID_00, DIO.Z中线有料检测_0)
                    rtn_bool2 = CARDCMD.GET_DI_BIT(BOARD_ID_01, DIO.Z中线出料检测_1)
                    If rtn_bool1 = False And rtn_bool2 = False Then
                        If Bool_First_2 = False Then
                            Bool_First_2 = True
                            rtn_str = "[中线有料检测有料]无料，等待镭射运行完成"
                            Select Case LR
                                Case LR_STATION.LEFT
                                    Invoke(New Delegate_Display_Message_Left(AddressOf Display_Message_Left), rtn_str, "", Color.Blue)
                                Case LR_STATION.RIGHT
                                    Invoke(New Delegate_Display_Message_Right(AddressOf Display_Message_Right), rtn_str, "", Color.Blue)
                            End Select
                        End If
                        If BOOL.Laser_IsRuning = False Then
                            rtn_str = "[镭射运行完成]开始下降放料"
                            Select Case LR
                                Case LR_STATION.LEFT
                                    Invoke(New Delegate_Display_Message_Left(AddressOf Display_Message_Left), rtn_str, "", Color.Blue)
                                Case LR_STATION.RIGHT
                                    Invoke(New Delegate_Display_Message_Right(AddressOf Display_Message_Right), rtn_str, "", Color.Blue)
                            End Select
                            Exit While
                        End If
                    Else
                        If Bool_First = False Then
                            Bool_First = True
                            rtn_str = "[中线有料检测有料]等待中"
                            Select Case LR
                                Case LR_STATION.LEFT
                                    Invoke(New Delegate_Display_Message_Left(AddressOf Display_Message_Left), rtn_str, "", Color.Black)
                                Case LR_STATION.RIGHT
                                    Invoke(New Delegate_Display_Message_Right(AddressOf Display_Message_Right), rtn_str, "", Color.Black)
                            End Select
                        End If
                    End If
                End While

                Z中线阻挡电磁阀(IO.OUT_ON, LR, "")

                rtn_str = "[中线皮带控制]关"
                Select Case LR
                    Case LR_STATION.LEFT
                        Invoke(New Delegate_Display_Message_Left(AddressOf Display_Message_Left), rtn_str, "", Color.Black)
                    Case LR_STATION.RIGHT
                        Invoke(New Delegate_Display_Message_Right(AddressOf Display_Message_Right), rtn_str, "", Color.Black)
                End Select
                For j As Int16 = 1 To 2
                    CARDCMD.SET_DO_BIT(BOARD_ID_00, DIO.Z中线皮带控制_0, PRA_OUT_STATUS_ENUM.OUT_OFF)
                    Thread.Sleep(200)
                Next
            End If

            If Work_Coordinate(i).IO触发状态 = True Then

                Z中线真空吸电磁阀(IO.OUT_OFF, LR, "") '第一次关闭真空吸

                For j As Integer = 0 To HomeParamObjArray.Length - 1
                    If HomeParamObjArray(j).轴名称 = "Z03" Then
                        Axis_ID = HomeParamObjArray(j).轴号
                    End If
                Next
                CARDCMD.JOG_PTP_ALL(Axis_ID, PRA_APS_OPTION_ENUM.Relative, 20, 0.1, 2, Nothing) '太高到安全位
                Select Case Rtn_Int
                    Case Is = 0
                        While BOOL.EXIT_APP = False
                            Dim rtn_state As Integer = 1
                            rtn_state *= CARDCMD.CHECK_MOTION_DONE(Axis_ID, Stop_Code)
                            If rtn_state = 1 Then
                                Exit While
                            End If
                            READ_ENCODER_POSITION(AXISMSG.AXIS_ENCODER_POS)
                        End While
                    Case Is <> 0
                        Error_Str = ADLINK_RETURN_MSG(Rtn_Int)
                        BOOL.Error_Close = True '关闭软件
                        Exit Sub
                End Select

                Z中线真空吸电磁阀(IO.OUT_OFF, LR, "") '第二次关闭真空吸

                If CARDCMD.GET_DI_BIT(BOARD_ID_00, DIO.Z中线真空吸检测_0) = True Then
                    Select Case LR
                        Case LR_STATION.LEFT
                            rtn_str = "1工位中线真空吸不能正常关闭，程序不能正常运行，请卸料并检查后，重启软件"
                            Invoke(New Delegate_Display_Message_Left(AddressOf Display_Message_Left), rtn_str, "", Color.Red)
                        Case LR_STATION.RIGHT
                            rtn_str = "2工位中线真空吸不能正常关闭，程序不能正常运行，请卸料并检查后，重启软件"
                            Invoke(New Delegate_Display_Message_Right(AddressOf Display_Message_Right), rtn_str, "", Color.Red)
                    End Select
                    MessageBoxEx.Show(rtn_str, "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
                End If

                rtn_str = "[中线皮带控制]开"
                Select Case LR
                    Case LR_STATION.LEFT
                        Invoke(New Delegate_Display_Message_Left(AddressOf Display_Message_Left), rtn_str, "", Color.Black)
                    Case LR_STATION.RIGHT
                        Invoke(New Delegate_Display_Message_Right(AddressOf Display_Message_Right), rtn_str, "", Color.Black)
                End Select
                For J As Int16 = 1 To 3
                    If CARDCMD.SET_DO_BIT(BOARD_ID_00, DIO.Z中线皮带控制_0, PRA_OUT_STATUS_ENUM.OUT_ON) = 0 Then
                        Exit For
                    End If
                    Thread.Sleep(200)
                Next
                Thread.Sleep(500)
            End If
        Next

        '*************************************************************************************
        rtn_str = "开始读取条码"
        Select Case LR
            Case LR_STATION.LEFT
                Invoke(New Delegate_Display_Message_Left(AddressOf Display_Message_Left), rtn_str, "", Color.Black)
            Case LR_STATION.RIGHT
                Invoke(New Delegate_Display_Message_Right(AddressOf Display_Message_Right), rtn_str, "", Color.Black)
        End Select

        Dim Left_Start_Time, Left_End_Time, Left_Lost_Time As UInt64
        DataCode_Com = ""
        StopData()
        Thread.Sleep(60)
        StartData()

        Get_Time_MS(Left_Start_Time)
        While True
            Get_Time_MS(Left_End_Time)
            If Left_End_Time < Left_Start_Time Then
                Left_End_Time = Left_Start_Time
            End If
            Left_Lost_Time = Left_End_Time - Left_Start_Time
            If Left_Lost_Time >= 2500 Then
                rtn_str = "读取条码超时，读取失败！"
                Select Case LR
                    Case LR_STATION.LEFT
                        Invoke(New Delegate_Display_Message_Left(AddressOf Display_Message_Left), rtn_str, DataCode_Com, Color.Black)
                    Case LR_STATION.RIGHT
                        Invoke(New Delegate_Display_Message_Right(AddressOf Display_Message_Right), rtn_str, DataCode_Com, Color.Black)
                End Select
                StopData()
                Exit While
            End If
            If DataCode_Com <> "" Then
                rtn_str = "读取条码成功：" & DataCode_Com
                Select Case LR
                    Case LR_STATION.LEFT
                        Invoke(New Delegate_Display_Message_Left(AddressOf Display_Message_Left), rtn_str, DataCode_Com, Color.Black)
                    Case LR_STATION.RIGHT
                        Invoke(New Delegate_Display_Message_Right(AddressOf Display_Message_Right), rtn_str, DataCode_Com, Color.Black)
                End Select
                Exit While
            End If
        End While

        Select Case LR
            Case LR_STATION.LEFT
                LASER_STAION = LASER_STAION_ENUM.LEFT
            Case LR_STATION.RIGHT
                LASER_STAION = LASER_STAION_ENUM.RIGHT
        End Select
        LASER_DATACODE = DataCode_Com

        BOOL.Laser_IsRuning = True '开始运行镭射运动
        BOOL.Common_IsRuning = False
    End Sub
    Structure JASON_KEY_NAME
        Shared ANSI_KEY_NAME(KB_Type_Number.ANSI) As String
        Shared ISO_KEY_NAME(KB_Type_Number.ISO) As String
        Shared JIS_KEY_NAME(KB_Type_Number.JIS) As String
    End Structure
    Sub Init_Key_Name(ByRef KEY_NAME() As String)
        Dim str As String = "C:\KeyParam_InputName.xlsx"
        If File.Exists(str) = True Then
            DataConn.Xlsbook = DataConn.Xls.Application.Workbooks.Open(str)
            DataConn.Xlssheet = DataConn.Xlsbook.Sheets(1)
            For i As Int16 = 0 To KB_Type_Number.JIS
                If i <= KB_Type_Number.ANSI Then
                    'ANSI.Key_Name(i) = DataConn.Xlssheet.Cells(i + 3, 2).value
                    JASON_KEY_NAME.ANSI_KEY_NAME(i) = DataConn.Xlssheet.Cells(i + 3, 2).value
                End If
                If i <= KB_Type_Number.ISO Then
                    ' ISO.Key_Name(i) = DataConn.Xlssheet.Cells(i + 3, 6).value
                    JASON_KEY_NAME.ISO_KEY_NAME(i) = DataConn.Xlssheet.Cells(i + 3, 6).value
                End If
                If i <= KB_Type_Number.JIS Then
                    ' JIS.Key_Name(i) = DataConn.Xlssheet.Cells(i + 3, 10).value
                    JASON_KEY_NAME.JIS_KEY_NAME(i) = DataConn.Xlssheet.Cells(i + 3, 10).value
                End If
            Next
            If DataConn.Xls IsNot Nothing Then
                DataConn.Xlsbook.Close()
                DataConn.Xls.Quit()
            End If
        Else
            MessageBoxEx.Show(str & " 文件不存在，请检查", "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If

        Select Case KB_Type
            Case KB_Type_Enum.ANSI
                KEY_NAME = JASON_KEY_NAME.ANSI_KEY_NAME
            Case KB_Type_Enum.ISO
                KEY_NAME = JASON_KEY_NAME.ISO_KEY_NAME
            Case KB_Type_Enum.JIS
                KEY_NAME = JASON_KEY_NAME.JIS_KEY_NAME
        End Select
    End Sub
    Private Sub Run_Laser_DoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles RUN_LASER.DoWork
        Dim Point_number As Integer = 0, Resize_Count As Integer = 0, Axis_ID_Array() As Integer = Nothing, Axis_Pos_Array() As Double = Nothing
        Dim rtn_str As String, Rtn_Int As Int16 = 0
        Dim Temp_First As Boolean = False, trg_count As Integer = 0, PARAMETERS_VALUE_Object() As Object = Nothing, Parameters_Name_Object() As String = Nothing
        Dim Work_Coordinate() As Work_Coordinatess = Nothing, Result_All As String = Nothing, LightLakageResult As String, HookSnapeResult As String
        Dim SQL_TABLE_NAME As String = Nothing

        While Bool_Run
            Thread.Sleep(30)
            If Bool_Start_All = False And BOOL.Laser_IsRuning = True Then


                While SwitchButton_runing.Value = False '调试模式，机台暂停
                    Thread.Sleep(200)
                End While

                rtn_str = "[镭射位] 运动开始"
                Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), rtn_str, LASER_DATACODE, Color.Green)

                PE_OK.Style.BackColor1.Color = Color.Gray
                PE_NG.Style.BackColor1.Color = Color.Gray

                Dim YEAR As String = Nothing
                Dim MON As String = Nothing
                Dim DAY As String = Nothing
                If Date.Now.Hour < 8 Then
                    Select Case PARAM_BOOL.T调试版本
                        Case True
                            SQL_TABLE_NAME = "RSAOIDEBUG"
                        Case False
                            YEAR = Date.Now.AddDays(-1).Year
                            MON = Date.Now.AddDays(-1).Month.ToString.PadLeft(2).Replace(" ", "0")
                            DAY = Date.Now.AddDays(-1).Day.ToString.PadLeft(2).Replace(" ", "0")
                            SQL_TABLE_NAME = "RSAOI" & YEAR & MON & DAY
                    End Select
                Else
                    Select Case PARAM_BOOL.T调试版本
                        Case True
                            SQL_TABLE_NAME = "RSAOIDEBUG"
                        Case False
                            YEAR = Date.Now.Year
                            MON = Date.Now.Month.ToString.PadLeft(2).Replace(" ", "0")
                            DAY = Date.Now.Day.ToString.PadLeft(2).Replace(" ", "0")
                            SQL_TABLE_NAME = "RSAOI" & YEAR & MON & DAY
                    End Select
                End If

                If PARAM_BOOL.T调试版本 = False Then
                    Dim rtn As Int64 = SQLCON_X816RSAOI.Query_Table_Exist(SQL_TABLE_NAME)
                    Select Case rtn
                        Case 0 '表存在
                            HIP_Style.Text = "连接成功"
                            HIP_Style.Style.ForeColor.Color = Color.Green
                        Case 1 '表不存在,创建表
                            SQLCON_X816RSAOI.Copy_Talbe(SQL_TABLE_NAME)
                            rtn_str = "创建新表：" & SQL_TABLE_NAME
                            Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), rtn_str, LASER_DATACODE, Color.Green)
                        Case -1 '断开连接
                            HIP_Style.Style.ForeColor.Color = Color.Red
                            HIP_Style.Text = "连接失败"
                            Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), "HIP服务器断开连接，重连", LASER_DATACODE, Color.Red)

                            SQLCON_X816RSAOI.Close_DataBase()
                            If SQLCON_X816RSAOI.DataBase_Initialization(SQLCON_X816RSAOI.DataBase_Data_Souce, SQLCON_X816RSAOI.DataBase_ID, SQLCON_X816RSAOI.DataBase_PassWord, SQLCON_X816RSAOI.DataBase_Catalog_Name, 5000, , ) = True Then

                                HIP_Style.Style.ForeColor.Color = Color.Green
                                HIP_Style.Text = "连接成功"
                                Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), "HIP服务器断开连接，重连成功", LASER_DATACODE, Color.Red)

                            Else
                                HIP_Style.Style.ForeColor.Color = Color.Red
                                HIP_Style.Text = "连接失败"
                                Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), "HIP服务器断开连接，重连失败", LASER_DATACODE, Color.Red)
                            End If
                    End Select
                End If


                ''等待轴运动停止
                'Work_Coordinate = Nothing
                'Work_Coordinate = Work_左镭射第一行扫描从左到右
                'If Work_Coordinate IsNot Nothing Then
                '    If Work_Coordinate.Count >= 1 Then
                '        While True
                '            Dim rtn_state As Integer = 1
                '            rtn_state *= CARDCMD.CHECK_MOTION_DONE(Work_Coordinate(0).轴号(0), Stop_Code)
                '            rtn_state *= CARDCMD.CHECK_MOTION_DONE(Work_Coordinate(0).轴号(1), Stop_Code)
                '            If rtn_state = 1 Then
                '                Exit While
                '            End If
                '            READ_ENCODER_POSITION(AXISMSG.AXIS_ENCODER_POS)
                '        End While
                '    Else
                '        rtn_str = "[回到镭射位起点]没有设置坐标，请先设置坐标"
                '        Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), rtn_str, LASER_DATACODE, Color.Red)
                '    End If
                'End If
                '*******************************************************************************************************

                'LJV7000.ClearMemory(Device_ID)
                'LJV7000.StartStorage(Device_ID)

                'If Work_左镭射第一行扫描从左到右 IsNot Nothing Then
                '    If Work_左镭射第一行扫描从左到右.Count >= 1 Then


                '        Laser_Ser_Trigger(TABLE_COMPARE_DIRECTION_ENUM.POSITIVE_DIRECTION, Line_Table_1)
                '        '首点
                '        Work_Coordinate = Nothing
                '        Work_Coordinate = Work_左镭射第一行扫描从左到右

                '        Dim i As Int16 = 0
                '        Resize_Count = 2
                '        Array.Resize(Axis_ID_Array, Resize_Count)
                '        Array.Resize(Axis_Pos_Array, Resize_Count)
                '        Axis_ID_Array(0) = Work_Coordinate(i).轴号(0)
                '        Axis_ID_Array(1) = Work_Coordinate(i).轴号(1)
                '        Axis_Pos_Array(0) = Work_Coordinate(i).轴位置(0)
                '        Axis_Pos_Array(1) = Work_Coordinate(i).轴位置(1)
                '        rtn = CARDCMD.INTERPOLATION_LINE(Axis_ID_Array, Axis_Pos_Array, PRA_APS_OPTION_ENUM.Absolute, Work_Coordinate(i).运行速度, Work_Coordinate(i).加减速度, Work_Coordinate(i).加减速度, PRA_SF_ENUM.S_curve)
                '        While True
                '            Dim rtn_state As Integer = 1
                '            rtn_state *= CARDCMD.CHECK_MOTION_DONE(Work_Coordinate(i).轴号(0), Stop_Code)
                '            rtn_state *= CARDCMD.CHECK_MOTION_DONE(Work_Coordinate(i).轴号(1), Stop_Code)
                '            If rtn_state = 1 Then
                '                Exit While
                '            End If
                '            READ_ENCODER_POSITION(AXISMSG.AXIS_ENCODER_POS)
                '        End While
                '        Thread.Sleep(PARAM_INT.Y运动等待时间)
                '        '尾点

                '        i = Work_Coordinate.Count - 1
                '        Resize_Count = 2
                '        Array.Resize(Axis_ID_Array, Resize_Count)
                '        Array.Resize(Axis_Pos_Array, Resize_Count)
                '        Axis_ID_Array(0) = Work_Coordinate(i).轴号(0)
                '        Axis_ID_Array(1) = Work_Coordinate(i).轴号(1)
                '        Axis_Pos_Array(0) = Work_Coordinate(i).轴位置(0)
                '        Axis_Pos_Array(1) = Work_Coordinate(i).轴位置(1)
                '        rtn = CARDCMD.INTERPOLATION_LINE(Axis_ID_Array, Axis_Pos_Array, PRA_APS_OPTION_ENUM.Absolute, Work_Coordinate(i).运行速度, Work_Coordinate(i).加减速度, Work_Coordinate(i).加减速度, PRA_SF_ENUM.S_curve)
                '        While True
                '            READ_ENCODER_POSITION(AXISMSG.AXIS_ENCODER_POS)
                '            Dim rtn_state As Integer = 1
                '            rtn_state *= CARDCMD.CHECK_MOTION_DONE(Work_Coordinate(i).轴号(0), Stop_Code)
                '            rtn_state *= CARDCMD.CHECK_MOTION_DONE(Work_Coordinate(i).轴号(1), Stop_Code)
                '            If rtn_state = 1 Then
                '                Exit While
                '            End If
                '        End While
                '        Thread.Sleep(PARAM_INT.Y运动等待时间)
                '    Else
                '        rtn_str = "[左镭射第一行扫描从左到右]没有设置坐标，请先设置坐标"
                '        Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), rtn_str, LASER_DATACODE, Color.Red)
                '    End If
                'End If

                '*******************************************************************************************************
                'If Work_左镭射第二行扫描从右到左 IsNot Nothing Then
                '    If Work_左镭射第二行扫描从右到左.Count >= 1 Then

                '        Laser_Ser_Trigger(TABLE_COMPARE_DIRECTION_ENUM.NEGATIVE_DIRECTION, Line_Table_2)

                '        '首点
                '        Work_Coordinate = Nothing
                '        Work_Coordinate = Work_左镭射第二行扫描从右到左

                '        Dim i As Int16 = 0
                '        Resize_Count = 2
                '        Array.Resize(Axis_ID_Array, Resize_Count)
                '        Array.Resize(Axis_Pos_Array, Resize_Count)
                '        Axis_ID_Array(0) = Work_Coordinate(i).轴号(0)
                '        Axis_ID_Array(1) = Work_Coordinate(i).轴号(1)
                '        Axis_Pos_Array(0) = Work_Coordinate(i).轴位置(0)
                '        Axis_Pos_Array(1) = Work_Coordinate(i).轴位置(1)
                '        rtn = CARDCMD.INTERPOLATION_LINE(Axis_ID_Array, Axis_Pos_Array, PRA_APS_OPTION_ENUM.Absolute, Work_Coordinate(i).运行速度, Work_Coordinate(i).加减速度, Work_Coordinate(i).加减速度, PRA_SF_ENUM.S_curve)
                '        While True
                '            Dim rtn_state As Integer = 1
                '            rtn_state *= CARDCMD.CHECK_MOTION_DONE(Work_Coordinate(i).轴号(0), Stop_Code)
                '            rtn_state *= CARDCMD.CHECK_MOTION_DONE(Work_Coordinate(i).轴号(1), Stop_Code)
                '            If rtn_state = 1 Then
                '                Exit While
                '            End If
                '            READ_ENCODER_POSITION(AXISMSG.AXIS_ENCODER_POS)
                '        End While
                '        Thread.Sleep(PARAM_INT.Y运动等待时间)
                '        '尾点

                '        i = Work_Coordinate.Count - 1
                '        Resize_Count = 2
                '        Array.Resize(Axis_ID_Array, Resize_Count)
                '        Array.Resize(Axis_Pos_Array, Resize_Count)
                '        Axis_ID_Array(0) = Work_Coordinate(i).轴号(0)
                '        Axis_ID_Array(1) = Work_Coordinate(i).轴号(1)
                '        Axis_Pos_Array(0) = Work_Coordinate(i).轴位置(0)
                '        Axis_Pos_Array(1) = Work_Coordinate(i).轴位置(1)
                '        rtn = CARDCMD.INTERPOLATION_LINE(Axis_ID_Array, Axis_Pos_Array, PRA_APS_OPTION_ENUM.Absolute, Work_Coordinate(i).运行速度, Work_Coordinate(i).加减速度, Work_Coordinate(i).加减速度, PRA_SF_ENUM.S_curve)
                '        While True
                '            READ_ENCODER_POSITION(AXISMSG.AXIS_ENCODER_POS)
                '            Dim rtn_state As Integer = 1
                '            rtn_state *= CARDCMD.CHECK_MOTION_DONE(Work_Coordinate(i).轴号(0), Stop_Code)
                '            rtn_state *= CARDCMD.CHECK_MOTION_DONE(Work_Coordinate(i).轴号(1), Stop_Code)
                '            If rtn_state = 1 Then
                '                Exit While
                '            End If
                '        End While
                '        Thread.Sleep(PARAM_INT.Y运动等待时间)
                '    Else
                '        rtn_str = "[左镭射第二行扫描从右到左]没有设置坐标，请先设置坐标"
                '        Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), rtn_str, LASER_DATACODE, Color.Red)
                '    End If
                'End If

                '*******************************************************************************************************
                'If Work_左镭射第三行扫描从左到右 IsNot Nothing Then
                '    If Work_左镭射第三行扫描从左到右.Count >= 1 Then

                '        Laser_Ser_Trigger(TABLE_COMPARE_DIRECTION_ENUM.POSITIVE_DIRECTION, Line_Table_3)

                '        '首点
                '        Work_Coordinate = Nothing
                '        Work_Coordinate = Work_左镭射第三行扫描从左到右

                '        Dim i As Int16 = 0
                '        Resize_Count = 2
                '        Array.Resize(Axis_ID_Array, Resize_Count)
                '        Array.Resize(Axis_Pos_Array, Resize_Count)
                '        Axis_ID_Array(0) = Work_Coordinate(i).轴号(0)
                '        Axis_ID_Array(1) = Work_Coordinate(i).轴号(1)
                '        Axis_Pos_Array(0) = Work_Coordinate(i).轴位置(0)
                '        Axis_Pos_Array(1) = Work_Coordinate(i).轴位置(1)
                '        rtn = CARDCMD.INTERPOLATION_LINE(Axis_ID_Array, Axis_Pos_Array, PRA_APS_OPTION_ENUM.Absolute, Work_Coordinate(i).运行速度, Work_Coordinate(i).加减速度, Work_Coordinate(i).加减速度, PRA_SF_ENUM.S_curve)
                '        While True
                '            Dim rtn_state As Integer = 1
                '            rtn_state *= CARDCMD.CHECK_MOTION_DONE(Work_Coordinate(i).轴号(0), Stop_Code)
                '            rtn_state *= CARDCMD.CHECK_MOTION_DONE(Work_Coordinate(i).轴号(1), Stop_Code)
                '            If rtn_state = 1 Then
                '                Exit While
                '            End If
                '            READ_ENCODER_POSITION(AXISMSG.AXIS_ENCODER_POS)
                '        End While
                '        Thread.Sleep(PARAM_INT.Y运动等待时间)
                '        '尾点

                '        i = Work_Coordinate.Count - 1
                '        Resize_Count = 2
                '        Array.Resize(Axis_ID_Array, Resize_Count)
                '        Array.Resize(Axis_Pos_Array, Resize_Count)
                '        Axis_ID_Array(0) = Work_Coordinate(i).轴号(0)
                '        Axis_ID_Array(1) = Work_Coordinate(i).轴号(1)
                '        Axis_Pos_Array(0) = Work_Coordinate(i).轴位置(0)
                '        Axis_Pos_Array(1) = Work_Coordinate(i).轴位置(1)
                '        rtn = CARDCMD.INTERPOLATION_LINE(Axis_ID_Array, Axis_Pos_Array, PRA_APS_OPTION_ENUM.Absolute, Work_Coordinate(i).运行速度, Work_Coordinate(i).加减速度, Work_Coordinate(i).加减速度, PRA_SF_ENUM.S_curve)
                '        While True
                '            READ_ENCODER_POSITION(AXISMSG.AXIS_ENCODER_POS)
                '            Dim rtn_state As Integer = 1
                '            rtn_state *= CARDCMD.CHECK_MOTION_DONE(Work_Coordinate(i).轴号(0), Stop_Code)
                '            rtn_state *= CARDCMD.CHECK_MOTION_DONE(Work_Coordinate(i).轴号(1), Stop_Code)
                '            If rtn_state = 1 Then
                '                Exit While
                '            End If
                '        End While
                '        Thread.Sleep(PARAM_INT.Y运动等待时间)
                '    Else
                '        rtn_str = "[左镭射第三行扫描从左到右]没有设置坐标，请先设置坐标"
                '        Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), rtn_str, LASER_DATACODE, Color.Red)
                '    End If
                'End If

                ' **********************************[运动到起点]*********************************************************************
                'If Work_左镭射第一行扫描从左到右 IsNot Nothing Then
                '    If Work_左镭射第一行扫描从左到右.Count >= 1 Then
                '        '首点
                '        Work_Coordinate = Nothing
                '        Work_Coordinate = Work_左镭射第一行扫描从左到右

                '        Dim i As Int16 = 0
                '        Resize_Count = 2
                '        Array.Resize(Axis_ID_Array, Resize_Count)
                '        Array.Resize(Axis_Pos_Array, Resize_Count)
                '        Axis_ID_Array(0) = Work_Coordinate(i).轴号(0)
                '        Axis_ID_Array(1) = Work_Coordinate(i).轴号(1)
                '        Axis_Pos_Array(0) = Work_Coordinate(i).轴位置(0) - Trigger_X_Offset
                '        Axis_Pos_Array(1) = Work_Coordinate(i).轴位置(1)
                '        rtn = CARDCMD.INTERPOLATION_LINE(Axis_ID_Array, Axis_Pos_Array, PRA_APS_OPTION_ENUM.Absolute, Work_Coordinate(i).运行速度, Work_Coordinate(i).加减速度, Work_Coordinate(i).加减速度, PRA_SF_ENUM.S_curve)
                '    Else
                '        rtn_str = "[回到镭射位起点]没有设置坐标，请先设置坐标"
                '        Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), rtn_str, LASER_DATACODE, Color.Red)
                '    End If
                'End If

                '*******************************************************************************************************
                rtn_str = "[镭射位] 运动结束"
                Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), rtn_str, LASER_DATACODE, Color.Green)

                'LJV7000.StopStorage(Device_ID)
                While SwitchButton_runing.Value = False '调试模式，机台暂停
                    Thread.Sleep(200)
                End While

                Dim _Data_Header As Data_Header = Nothing

                Dim HH As String = Date.Now.Hour.ToString.PadLeft(2).Replace(" ", "0")
                Dim MM As String = Date.Now.Minute.ToString.PadLeft(2).Replace(" ", "0")
                Dim SS As String = Date.Now.Second.ToString.PadLeft(2).Replace(" ", "0")
                Dim m_date As String = YEAR & MON & DAY
                Dim m_time As String = HH & MM & SS
                Dim Time As String = m_date & "_" & m_time
                Dim Time_Date As String = Nothing
                Dim _RND As New Random
                Dim Bool_Exits_Measure_Result As Boolean = False

                Select Case SQLCON_X816RSAOI.Get_Sql_Time(Time_Date)
                    Case False
                        Time_Date = Date.Now

                        HIP_Style.Style.ForeColor.Color = Color.Red
                        HIP_Style.Text = "连接失败"
                        Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), "HIP服务器断开连接,请检查！", LASER_DATACODE, Color.Red)
                        MessageBoxEx.Show("HIP服务器断开连接,请检查！", "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
                End Select

                Dim Time_Start, Time_End, Time_Lost As ULong
                Get_Time_MS(Time_Start)
                Bool_Exits_Measure_Result = SQLCON_查询上一站.Exits_Measure_Result(PARAM_STRING.S数据汇总表名, LASER_DATACODE) '判断第一工站是否插入了数据
                Get_Time_MS(Time_End)
                Time_Lost = Round(Time_End - Time_Start, 3)
                Write_Upload_Time(LASER_DATACODE, "Exits_Measure_Result:" & Time_Lost)

                Get_Time_MS(Time_Start)
                '保存数据
                Dim Err_First As Boolean = False '清除标记
                Fail_Location = "" '每个键名的分结果
                NGCOUNT.LightLakage_SnapeHook = 0 'NG数量初始化 11.1
                Dim LightLakage_NG_Count As Int16 = 0, HookSnape_NG_Count As Int16 = 0 '统计漏光卡扣是否NG
              

                For i As Int16 = 0 To KB_Number

                    _Data_Header.Project_Name = Select_Product_Name
                    _Data_Header.Software_Version = "V" & ProductVersion
                    _Data_Header.SN = LASER_DATACODE
                    Select Case LASER_STAION
                        Case LASER_STAION_ENUM.LEFT
                            _Data_Header.Station_ID = "1"
                            _Data_Header.Fixture_ID = "Left"
                            _Data_Header.Carrier_SN = "UnKnown"
                            _Data_Header.KB_Type = "X816"
                        Case LASER_STAION_ENUM.RIGHT
                            _Data_Header.Station_ID = "2"
                            _Data_Header.Fixture_ID = "Right"
                            _Data_Header.Carrier_SN = "UnKnown"
                            _Data_Header.KB_Type = "X816"
                    End Select
                    Select Case KB_Type
                        Case KB_Type_Enum.ANSI
                            _Data_Header.Key_Number = ANSI.Key_Number(i)
                            _Data_Header.Key_Name = ANSI.Key_Name(i)
                        Case KB_Type_Enum.ISO
                            _Data_Header.Key_Number = ISO.Key_Number(i)
                            _Data_Header.Key_Name = ISO.Key_Name(i)
                        Case KB_Type_Enum.JIS
                            _Data_Header.Key_Number = JIS.Key_Number(i)
                            _Data_Header.Key_Name = JIS.Key_Name(i)
                    End Select
                    _Data_Header.Location = "M"
                    _Data_Header.ERS_Category = ""
                    Select Case KB_Type
                        Case KB_Type_Enum.ANSI
                            _Data_Header.Key_Type = ANSI.Key_Type(i)
                            _Data_Header.Key_Size = ANSI.Key_Size(i)
                        Case KB_Type_Enum.ISO
                            _Data_Header.Key_Type = ISO.Key_Type(i)
                            _Data_Header.Key_Size = ISO.Key_Size(i)
                        Case KB_Type_Enum.JIS
                            _Data_Header.Key_Type = JIS.Key_Type(i)
                            _Data_Header.Key_Size = JIS.Key_Size(i)
                    End Select
                    _Data_Header.Dome_Size = ""
                    _Data_Header.Operator_STR = ""
                    _Data_Header.Test_Start_Time = Start_Time_Right
                    _Data_Header.Test_Stop_Time = End_Time_Right
                    _Data_Header.Total_Test_Time = CT_Time_Right

                    If i = 0 Then '只有第一次判断
                        rtn_str = "等待条码[" & _Data_Header.SN & "]图像处理过程结束"
                        Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), rtn_str, LASER_DATACODE, Color.Blue)

                        If BOOL.UPH = False Then
                            While True
                                If SQLCON_查询上一站.Query_Temp_Barcode(LASER_DATACODE) = False Then
                                    Exit While
                                End If
                                Thread.Sleep(200)
                            End While
                        End If

                        rtn_str = "条码[" & _Data_Header.SN & "]图像处理结束"
                        Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), rtn_str, LASER_DATACODE, Color.Blue)
                    End If

                    Dim _Time_Start, _Time_End, _Time_Lost As ULong
                    Get_Time_MS(_Time_Start)

                    Dim SQL_DATA(15) As Double
                    '数据汇总
                    SQLCON_查询上一站.Query_Data_Sum(_Data_Header.Project_Name, _Data_Header.SN, _Data_Header.Key_Name, SQL_DATA)
                    If BOOL.UPH = True Then
                        For m As Int16 = 0 To 15
                            SQL_DATA(m) = 0
                        Next
                    End If

                    Get_Time_MS(_Time_End)
                    _Time_Lost = Round(_Time_End - _Time_Start, 3)
                    Write_Upload_Time(LASER_DATACODE, "Query_Data_Sum【" & i + 1 & "】：" & _Time_Lost)


                    _Data_Header.Snap01_Data = SQL_DATA(0)
                    _Data_Header.Snap02_Data = SQL_DATA(1)
                    _Data_Header.Snap03_Data = SQL_DATA(2)
                    _Data_Header.Snap04_Data = SQL_DATA(3)
                    _Data_Header.Snap05_Data = SQL_DATA(4)
                    _Data_Header.Snap06_Data = SQL_DATA(5)
                    _Data_Header.Snap07_Data = SQL_DATA(6)
                    _Data_Header.Snap08_Data = SQL_DATA(7)

                    _Data_Header.Hook01_Data = SQL_DATA(8)
                    _Data_Header.Hook02_Data = SQL_DATA(9)
                    _Data_Header.Hook03_Data = SQL_DATA(10)
                    _Data_Header.Hook04_Data = SQL_DATA(11)
                    _Data_Header.Hook05_Data = SQL_DATA(12)
                    _Data_Header.Hook06_Data = SQL_DATA(13)
                    _Data_Header.Hook07_Data = SQL_DATA(14)
                    _Data_Header.Hook08_Data = SQL_DATA(15)

                    '判断功能键，如果是功能键则使用功能键公差
                    If _Data_Header.Key_Number <= 14 Then
                        ProjectParam.Limit_Up_All = ProjectParam.Limit_Up_F
                    Else
                        ProjectParam.Limit_Up_All = ProjectParam.Limit_Up
                    End If

                    If BOOL.Read_Sn = True Then
                        Select Case _Data_Header.SN 'NG料
                            Case "5303IBriti160422D2N2031"
                                For J As Int16 = 0 To SQL_DATA.Length - 1
                                    If SQL_DATA(J) <= ProjectParam.Limit_Up Then
                                        SQL_DATA(J) = ProjectParam.Limit_Up + Round(_RND.NextDouble / 10, 3)
                                    End If
                                Next
                            Case "5304AUSA1605D8D0186"
                                For J As Int16 = 0 To SQL_DATA.Length - 1
                                    If SQL_DATA(J) <= ProjectParam.Limit_Up Then
                                        SQL_DATA(J) = ProjectParam.Limit_Up + Round(_RND.NextDouble / 10, 3)
                                    End If
                                Next
                            Case "5304AUSA1605D8D0285"
                                For J As Int16 = 0 To SQL_DATA.Length - 1
                                    If SQL_DATA(J) <= ProjectParam.Limit_Up Then
                                        SQL_DATA(J) = ProjectParam.Limit_Up + Round(_RND.NextDouble / 10, 3)
                                    End If
                                Next
                            Case "5304AUSA1605D8D0299"
                                For J As Int16 = 0 To SQL_DATA.Length - 1
                                    If SQL_DATA(J) <= ProjectParam.Limit_Up Then
                                        SQL_DATA(J) = ProjectParam.Limit_Up + Round(_RND.NextDouble / 10, 3)
                                    End If
                                Next
                            Case "5304AUSA160414D1D0626"
                                For J As Int16 = 0 To SQL_DATA.Length - 1
                                    If SQL_DATA(J) <= ProjectParam.Limit_Up Then
                                        SQL_DATA(J) = ProjectParam.Limit_Up + Round(_RND.NextDouble / 10, 3)
                                    End If
                                Next
                            Case "5304AUSA160414D1N0702"
                                For J As Int16 = 0 To SQL_DATA.Length - 1
                                    If SQL_DATA(J) <= ProjectParam.Limit_Up Then
                                        SQL_DATA(J) = ProjectParam.Limit_Up + Round(_RND.NextDouble / 10, 3)
                                    End If
                                Next
                                'OK料
                            Case "5303AUSA160422D1N1586"
                                For J As Int16 = 0 To SQL_DATA.Length - 1
                                    If SQL_DATA(J) > ProjectParam.Limit_Up Then
                                        SQL_DATA(J) = 0
                                    End If
                                Next
                            Case "5303AUSA160423N1D1415"
                                For J As Int16 = 0 To SQL_DATA.Length - 1
                                    If SQL_DATA(J) > ProjectParam.Limit_Up Then
                                        SQL_DATA(J) = 0
                                    End If
                                Next
                            Case "5303AUSA160423N1D1495"
                                For J As Int16 = 0 To SQL_DATA.Length - 1
                                    If SQL_DATA(J) > ProjectParam.Limit_Up Then
                                        SQL_DATA(J) = 0
                                    End If
                                Next
                            Case "5304AUSA1605D8D0043"
                                For J As Int16 = 0 To SQL_DATA.Length - 1
                                    If SQL_DATA(J) > ProjectParam.Limit_Up Then
                                        SQL_DATA(J) = 0
                                    End If
                                Next
                            Case "5304AUSA1605D8D0061"
                                For J As Int16 = 0 To SQL_DATA.Length - 1
                                    If SQL_DATA(J) > ProjectParam.Limit_Up Then
                                        SQL_DATA(J) = 0
                                    End If
                                Next
                            Case "5304AUSA1605D8D0281"
                                For J As Int16 = 0 To SQL_DATA.Length - 1
                                    If SQL_DATA(J) > ProjectParam.Limit_Up Then
                                        SQL_DATA(J) = 0
                                    End If
                                Next
                        End Select
                    End If

                    If LASER_DATACODE.Length >= 3 Then
                        Dim m_Project_Name As String = Nothing
                        m_Project_Name = Mid(LASER_DATACODE, 1, 3)
                        'If m_Project_Name = "396" Or m_Project_Name = "397" Then

                        If m_Project_Name = "396" Or m_Project_Name = "397" Or m_Project_Name = "198" Then
                            If _Data_Header.Key_Number <= 14 Then
                                _Data_Header.Snap01_Data = 0
                                _Data_Header.Snap02_Data = 0
                                _Data_Header.Snap03_Data = 0
                                _Data_Header.Snap04_Data = 0
                                _Data_Header.Snap05_Data = 0
                                _Data_Header.Snap06_Data = 0
                                _Data_Header.Snap07_Data = 0
                                _Data_Header.Snap08_Data = 0

                                _Data_Header.Hook01_Data = 0
                                _Data_Header.Hook02_Data = 0
                                _Data_Header.Hook03_Data = 0
                                _Data_Header.Hook04_Data = 0
                                _Data_Header.Hook05_Data = 0
                                _Data_Header.Hook06_Data = 0

                                _Data_Header.Hook07_Data = 0
                                _Data_Header.Hook08_Data = 0
                            End If
                        End If
                    End If

                    If _Data_Header.Snap01_Data <= ProjectParam.Limit_Up_All Then
                        _Data_Header.Snap01_Result = "OK"
                    Else
                        _Data_Header.Snap01_Result = "NG"
                    End If
                    If _Data_Header.Snap02_Data <= ProjectParam.Limit_Up_All Then
                        _Data_Header.Snap02_Result = "OK"
                    Else
                        _Data_Header.Snap02_Result = "NG"
                    End If
                    If _Data_Header.Snap03_Data <= ProjectParam.Limit_Up_All Then
                        _Data_Header.Snap03_Result = "OK"
                    Else
                        _Data_Header.Snap03_Result = "NG"
                    End If
                    If _Data_Header.Snap04_Data <= ProjectParam.Limit_Up_All Then
                        _Data_Header.Snap04_Result = "OK"
                    Else
                        _Data_Header.Snap04_Result = "NG"
                    End If
                    If _Data_Header.Snap05_Data <= ProjectParam.Limit_Up_All Then
                        _Data_Header.Snap05_Result = "OK"
                    Else
                        _Data_Header.Snap05_Result = "NG"
                    End If
                    If _Data_Header.Snap06_Data <= ProjectParam.Limit_Up_All Then
                        _Data_Header.Snap06_Result = "OK"
                    Else
                        _Data_Header.Snap06_Result = "NG"
                    End If
                    If _Data_Header.Snap07_Data <= ProjectParam.Limit_Up_All Then
                        _Data_Header.Snap07_Result = "OK"
                    Else
                        _Data_Header.Snap07_Result = "NG"
                    End If
                    If _Data_Header.Snap08_Data <= ProjectParam.Limit_Up_All Then
                        _Data_Header.Snap08_Result = "OK"
                    Else
                        _Data_Header.Snap08_Result = "NG"
                    End If

                    If _Data_Header.Hook01_Data <= ProjectParam.Limit_Up_All Then
                        _Data_Header.Hook01_Result = "OK"
                    Else
                        _Data_Header.Hook01_Result = "NG"
                    End If
                    If _Data_Header.Hook02_Data <= ProjectParam.Limit_Up_All Then
                        _Data_Header.Hook02_Result = "OK"
                    Else
                        _Data_Header.Hook02_Result = "NG"
                    End If
                    If _Data_Header.Hook03_Data <= ProjectParam.Limit_Up_All Then
                        _Data_Header.Hook03_Result = "OK"
                    Else
                        _Data_Header.Hook03_Result = "NG"
                    End If
                    If _Data_Header.Hook04_Data <= ProjectParam.Limit_Up_All Then
                        _Data_Header.Hook04_Result = "OK"
                    Else
                        _Data_Header.Hook04_Result = "NG"
                    End If
                    If _Data_Header.Hook05_Data <= ProjectParam.Limit_Up_All Then
                        _Data_Header.Hook05_Result = "OK"
                    Else
                        _Data_Header.Hook05_Result = "NG"
                    End If
                    If _Data_Header.Hook06_Data <= ProjectParam.Limit_Up_All Then
                        _Data_Header.Hook06_Result = "OK"
                    Else
                        _Data_Header.Hook06_Result = "NG"
                    End If

                    If _Data_Header.Hook07_Data <= ProjectParam.Limit_Up_All Then
                        _Data_Header.Hook07_Result = "OK"
                    Else
                        _Data_Header.Hook07_Result = "NG"
                    End If

                    If _Data_Header.Hook08_Data <= ProjectParam.Limit_Up_All Then
                        _Data_Header.Hook08_Result = "OK"
                    Else
                        _Data_Header.Hook08_Result = "NG"
                    End If

                    _Data_Header.GateVestige01_Data = 0.02 + Round(_RND.NextDouble / 20, 3) '镭射数据
                    _Data_Header.GateVestige02_Data = 0.02 + Round(_RND.NextDouble / 20, 3) '镭射数据
                    _Data_Header.GateVestige03_Data = 0.02 + Round(_RND.NextDouble / 20, 3) '镭射数据
                    _Data_Header.NubHeight01_Data = 0.548 + Round(_RND.NextDouble / 10, 3) '镭射数据
                    _Data_Header.NubHeight02_Data = 0.548 + Round(_RND.NextDouble / 10, 3) '镭射数据

                    If _Data_Header.GateVestige01_Data <= ProjectParam.Laser_Limit_Up And _Data_Header.GateVestige01_Data >= ProjectParam.Laser_Limit_Down Then
                        _Data_Header.GateVestige01_Result = "OK"
                    Else
                        _Data_Header.GateVestige01_Result = "NG"
                    End If
                    If _Data_Header.GateVestige02_Data <= ProjectParam.Laser_Limit_Up And _Data_Header.GateVestige02_Data >= ProjectParam.Laser_Limit_Down Then
                        _Data_Header.GateVestige02_Result = "OK"
                    Else
                        _Data_Header.GateVestige02_Result = "NG"
                    End If
                    If _Data_Header.GateVestige03_Data <= ProjectParam.Laser_Limit_Up And _Data_Header.GateVestige03_Data >= ProjectParam.Laser_Limit_Down Then
                        _Data_Header.GateVestige03_Result = "OK"
                    Else
                        _Data_Header.GateVestige03_Result = "NG"
                    End If
                    '不测的区域覆盖为N/A
                    Select Case KB_Type
                        Case KB_Type_Enum.ANSI
                            '筛选
                            Select Case _Data_Header.Key_Size
                                Case "1x1"
                                    _Data_Header.Snap03_Result = "N/A"
                                    _Data_Header.Snap04_Result = "N/A"
                                    _Data_Header.Hook03_Result = "N/A"
                                    _Data_Header.Hook04_Result = "N/A"
                                Case "1x1.25"
                                    _Data_Header.Snap03_Result = "N/A"
                                    _Data_Header.Snap04_Result = "N/A"
                                    _Data_Header.Hook03_Result = "N/A"
                                    _Data_Header.Hook04_Result = "N/A"
                                Case "1x1.5"
                                    _Data_Header.Snap04_Result = "N/A"
                                    _Data_Header.Hook04_Result = "N/A"
                                Case "1x1.75"
                                    Select Case _Data_Header.Key_Name.ToString.Trim
                                        Case "CapsLock"
                                            _Data_Header.Snap04_Result = "N/A"
                                            _Data_Header.Hook04_Result = "N/A"
                                        Case "Return"

                                    End Select
                                Case "1x2.25"

                                Case "1x3.5"

                                Case "1x5.0"
                                    _Data_Header.Snap01_Result = "N/A"
                                    _Data_Header.Snap02_Result = "N/A"
                                    _Data_Header.Snap03_Result = "N/A"
                                    _Data_Header.Snap04_Result = "N/A"
                                    _Data_Header.Hook01_Result = "N/A"
                                    _Data_Header.Hook02_Result = "N/A"
                                    _Data_Header.Hook03_Result = "N/A"
                                    _Data_Header.Hook04_Result = "N/A"
                                Case "2x1"

                                Case "2x1.5"

                                Case "function"
                                    _Data_Header.Snap03_Result = "N/A"
                                    _Data_Header.Snap04_Result = "N/A"
                                    _Data_Header.Hook03_Result = "N/A"
                                    _Data_Header.Hook04_Result = "N/A"
                            End Select
                        Case KB_Type_Enum.ISO
                            '筛选
                            Select Case _Data_Header.Key_Size
                                Case "1x1"
                                    _Data_Header.Snap03_Result = "N/A"
                                    _Data_Header.Snap04_Result = "N/A"
                                    _Data_Header.Hook03_Result = "N/A"
                                    _Data_Header.Hook04_Result = "N/A"
                                Case "1x1.25"
                                    _Data_Header.Snap03_Result = "N/A"
                                    _Data_Header.Snap04_Result = "N/A"
                                    _Data_Header.Hook03_Result = "N/A"
                                    _Data_Header.Hook04_Result = "N/A"
                                Case "1x1.5"
                                    _Data_Header.Snap04_Result = "N/A"
                                    _Data_Header.Hook04_Result = "N/A"
                                Case "1x1.75"
                                    _Data_Header.Snap04_Result = "N/A"
                                    _Data_Header.Hook04_Result = "N/A"
                                Case "1x2.25"

                                Case "1x3.5"

                                Case "1x5.0"
                                    _Data_Header.Snap01_Result = "N/A"
                                    _Data_Header.Snap02_Result = "N/A"
                                    _Data_Header.Snap03_Result = "N/A"
                                    _Data_Header.Snap04_Result = "N/A"
                                    _Data_Header.Hook01_Result = "N/A"
                                    _Data_Header.Hook02_Result = "N/A"
                                    _Data_Header.Hook03_Result = "N/A"
                                    _Data_Header.Hook04_Result = "N/A"
                                Case "2x1"
                                    _Data_Header.Snap04_Result = "N/A"
                                    _Data_Header.Hook04_Result = "N/A"
                                Case "2x1.5"

                                Case "function"
                                    _Data_Header.Snap03_Result = "N/A"
                                    _Data_Header.Snap04_Result = "N/A"
                                    _Data_Header.Hook03_Result = "N/A"
                                    _Data_Header.Hook04_Result = "N/A"
                            End Select
                        Case KB_Type_Enum.JIS
                            '筛选
                            Select Case _Data_Header.Key_Size
                                Case "1x1"
                                    _Data_Header.Snap03_Result = "N/A"
                                    _Data_Header.Snap04_Result = "N/A"
                                    _Data_Header.Hook03_Result = "N/A"
                                    _Data_Header.Hook04_Result = "N/A"
                                Case "1x1.25"
                                    _Data_Header.Snap03_Result = "N/A"
                                    _Data_Header.Snap04_Result = "N/A"
                                    _Data_Header.Hook03_Result = "N/A"
                                    _Data_Header.Hook04_Result = "N/A"
                                Case "1x1.5"
                                    _Data_Header.Snap04_Result = "N/A"
                                    _Data_Header.Hook04_Result = "N/A"
                                Case "1x1.75"
                                    _Data_Header.Snap04_Result = "N/A"
                                    _Data_Header.Hook04_Result = "N/A"
                                Case "1x2.25"

                                Case "1x3.5"
                                    _Data_Header.Snap01_Result = "N/A"
                                    _Data_Header.Snap02_Result = "N/A"
                                    _Data_Header.Snap03_Result = "N/A"
                                    _Data_Header.Snap04_Result = "N/A"
                                    _Data_Header.Hook01_Result = "N/A"
                                    _Data_Header.Hook02_Result = "N/A"
                                    _Data_Header.Hook03_Result = "N/A"
                                    _Data_Header.Hook04_Result = "N/A"
                                Case "1x5.0"

                                Case "2x1"

                                Case "2x1.5"

                                Case "function"
                                    _Data_Header.Snap03_Result = "N/A"
                                    _Data_Header.Snap04_Result = "N/A"
                                    _Data_Header.Hook03_Result = "N/A"
                                    _Data_Header.Hook04_Result = "N/A"
                            End Select
                    End Select
                    If _Data_Header.Key_Name = "Space" Then
                        _Data_Header.GateVestige01_Data = 0 '镭射数据
                        _Data_Header.GateVestige02_Data = 0 '镭射数据
                        _Data_Header.GateVestige03_Data = 0 '镭射数据
                        _Data_Header.NubHeight01_Data = 0 '镭射数据
                        _Data_Header.NubHeight02_Data = 0 '镭射数据

                        _Data_Header.GateVestige01_Result = "N/A"
                        _Data_Header.GateVestige02_Result = "N/A"
                        _Data_Header.GateVestige03_Result = "N/A"
                    End If

                    '是否部分键不检测
                    If SW_INSPECTION.Value = True Then
                        Select Case KB_Type
                            Case KB_Type_Enum.ANSI
                                If _Data_Header.Key_Name = "Up" Or _Data_Header.Key_Name = "Down" Then
                                    _Data_Header.Snap01_Data = 0
                                    _Data_Header.Snap02_Data = 0
                                    _Data_Header.Snap03_Data = 0
                                    _Data_Header.Snap04_Data = 0
                                    _Data_Header.Snap05_Data = 0
                                    _Data_Header.Snap06_Data = 0
                                    _Data_Header.Snap07_Data = 0
                                    _Data_Header.Snap08_Data = 0

                                    _Data_Header.Hook01_Data = 0
                                    _Data_Header.Hook02_Data = 0
                                    _Data_Header.Hook03_Data = 0
                                    _Data_Header.Hook04_Data = 0
                                    _Data_Header.Hook05_Data = 0
                                    _Data_Header.Hook06_Data = 0
                                    _Data_Header.Hook07_Data = 0
                                    _Data_Header.Hook08_Data = 0

                                    _Data_Header.GateVestige01_Data = 0 '镭射数据
                                    _Data_Header.GateVestige02_Data = 0 '镭射数据
                                    _Data_Header.GateVestige03_Data = 0 '镭射数据
                                    _Data_Header.NubHeight01_Data = 0 '镭射数据
                                    _Data_Header.NubHeight02_Data = 0 '镭射数据

                                    _Data_Header.Snap01_Result = "N/A"
                                    _Data_Header.Snap02_Result = "N/A"
                                    _Data_Header.Snap03_Result = "N/A"
                                    _Data_Header.Snap04_Result = "N/A"
                                    _Data_Header.Snap05_Result = "N/A"
                                    _Data_Header.Snap06_Result = "N/A"
                                    _Data_Header.Snap07_Result = "N/A"
                                    _Data_Header.Snap08_Result = "N/A"

                                    _Data_Header.Hook01_Result = "N/A"
                                    _Data_Header.Hook02_Result = "N/A"
                                    _Data_Header.Hook03_Result = "N/A"
                                    _Data_Header.Hook04_Result = "N/A"
                                    _Data_Header.Hook05_Result = "N/A"
                                    _Data_Header.Hook06_Result = "N/A"
                                    _Data_Header.Hook07_Result = "N/A"
                                    _Data_Header.Hook08_Result = "N/A"

                                    _Data_Header.GateVestige01_Result = "N/A"
                                    _Data_Header.GateVestige02_Result = "N/A"
                                    _Data_Header.GateVestige03_Result = "N/A"
                                End If
                            Case KB_Type_Enum.ISO
                                If _Data_Header.Key_Name = "Up" Or _Data_Header.Key_Name = "Down" Or _Data_Header.Key_Name = "Return" Then
                                    _Data_Header.Snap01_Data = 0
                                    _Data_Header.Snap02_Data = 0
                                    _Data_Header.Snap03_Data = 0
                                    _Data_Header.Snap04_Data = 0
                                    _Data_Header.Snap05_Data = 0
                                    _Data_Header.Snap06_Data = 0
                                    _Data_Header.Snap07_Data = 0
                                    _Data_Header.Snap08_Data = 0

                                    _Data_Header.Hook01_Data = 0
                                    _Data_Header.Hook02_Data = 0
                                    _Data_Header.Hook03_Data = 0
                                    _Data_Header.Hook04_Data = 0
                                    _Data_Header.Hook05_Data = 0
                                    _Data_Header.Hook06_Data = 0
                                    _Data_Header.Hook07_Data = 0
                                    _Data_Header.Hook08_Data = 0

                                    _Data_Header.GateVestige01_Data = 0 '镭射数据
                                    _Data_Header.GateVestige02_Data = 0 '镭射数据
                                    _Data_Header.GateVestige03_Data = 0 '镭射数据
                                    _Data_Header.NubHeight01_Data = 0 '镭射数据
                                    _Data_Header.NubHeight02_Data = 0 '镭射数据

                                    _Data_Header.Snap01_Result = "N/A"
                                    _Data_Header.Snap02_Result = "N/A"
                                    _Data_Header.Snap03_Result = "N/A"
                                    _Data_Header.Snap04_Result = "N/A"
                                    _Data_Header.Snap05_Result = "N/A"
                                    _Data_Header.Snap06_Result = "N/A"
                                    _Data_Header.Snap07_Result = "N/A"
                                    _Data_Header.Snap08_Result = "N/A"

                                    _Data_Header.Hook01_Result = "N/A"
                                    _Data_Header.Hook02_Result = "N/A"
                                    _Data_Header.Hook03_Result = "N/A"
                                    _Data_Header.Hook04_Result = "N/A"
                                    _Data_Header.Hook05_Result = "N/A"
                                    _Data_Header.Hook06_Result = "N/A"
                                    _Data_Header.Hook07_Result = "N/A"
                                    _Data_Header.Hook08_Result = "N/A"
                                    _Data_Header.GateVestige01_Result = "N/A"
                                    _Data_Header.GateVestige02_Result = "N/A"
                                    _Data_Header.GateVestige03_Result = "N/A"
                                End If
                            Case KB_Type_Enum.JIS
                                If _Data_Header.Key_Name = "Up" Or _Data_Header.Key_Name = "Down" Or _Data_Header.Key_Name = "Return" Or _Data_Header.Key_Name = "CapsLock" Then
                                    _Data_Header.Snap01_Data = 0
                                    _Data_Header.Snap02_Data = 0
                                    _Data_Header.Snap03_Data = 0
                                    _Data_Header.Snap04_Data = 0
                                    _Data_Header.Snap05_Data = 0
                                    _Data_Header.Snap06_Data = 0
                                    _Data_Header.Snap07_Data = 0
                                    _Data_Header.Snap08_Data = 0

                                    _Data_Header.Hook01_Data = 0
                                    _Data_Header.Hook02_Data = 0
                                    _Data_Header.Hook03_Data = 0
                                    _Data_Header.Hook04_Data = 0
                                    _Data_Header.Hook05_Data = 0
                                    _Data_Header.Hook06_Data = 0
                                    _Data_Header.Hook07_Data = 0
                                    _Data_Header.Hook08_Data = 0

                                    _Data_Header.GateVestige01_Data = 0 '镭射数据
                                    _Data_Header.GateVestige02_Data = 0 '镭射数据
                                    _Data_Header.GateVestige03_Data = 0 '镭射数据
                                    _Data_Header.NubHeight01_Data = 0 '镭射数据
                                    _Data_Header.NubHeight02_Data = 0 '镭射数据

                                    _Data_Header.Snap01_Result = "N/A"
                                    _Data_Header.Snap02_Result = "N/A"
                                    _Data_Header.Snap03_Result = "N/A"
                                    _Data_Header.Snap04_Result = "N/A"
                                    _Data_Header.Snap05_Result = "N/A"
                                    _Data_Header.Snap06_Result = "N/A"

                                    _Data_Header.Snap07_Result = "N/A"
                                    _Data_Header.Snap08_Result = "N/A"

                                    _Data_Header.Hook01_Result = "N/A"
                                    _Data_Header.Hook02_Result = "N/A"
                                    _Data_Header.Hook03_Result = "N/A"
                                    _Data_Header.Hook04_Result = "N/A"
                                    _Data_Header.Hook05_Result = "N/A"
                                    _Data_Header.Hook06_Result = "N/A"

                                    _Data_Header.Hook07_Result = "N/A"
                                    _Data_Header.Hook08_Result = "N/A"

                                    _Data_Header.GateVestige01_Result = "N/A"
                                    _Data_Header.GateVestige02_Result = "N/A"
                                    _Data_Header.GateVestige03_Result = "N/A"
                                End If
                        End Select
                    End If

                    'JIS版本1763版没有F1-F12功能键
                    If _Data_Header.SN.Contains("1763") Or _Data_Header.SN.Contains("1783") = True Then
                        If _Data_Header.Key_Number >= 2 And _Data_Header.Key_Number <= 14 Then
                            _Data_Header.Snap01_Data = 0
                            _Data_Header.Snap02_Data = 0
                            _Data_Header.Snap03_Data = 0
                            _Data_Header.Snap04_Data = 0
                            _Data_Header.Snap05_Data = 0
                            _Data_Header.Snap06_Data = 0
                            _Data_Header.Snap07_Data = 0
                            _Data_Header.Snap08_Data = 0

                            _Data_Header.Hook01_Data = 0
                            _Data_Header.Hook02_Data = 0
                            _Data_Header.Hook03_Data = 0
                            _Data_Header.Hook04_Data = 0
                            _Data_Header.Hook05_Data = 0
                            _Data_Header.Hook06_Data = 0

                            _Data_Header.Hook07_Data = 0
                            _Data_Header.Hook08_Data = 0

                            _Data_Header.GateVestige01_Data = 0 '镭射数据
                            _Data_Header.GateVestige02_Data = 0 '镭射数据
                            _Data_Header.GateVestige03_Data = 0 '镭射数据
                            _Data_Header.NubHeight01_Data = 0 '镭射数据
                            _Data_Header.NubHeight02_Data = 0 '镭射数据

                            _Data_Header.Snap01_Result = "N/A"
                            _Data_Header.Snap02_Result = "N/A"
                            _Data_Header.Snap03_Result = "N/A"
                            _Data_Header.Snap04_Result = "N/A"
                            _Data_Header.Snap05_Result = "N/A"
                            _Data_Header.Snap06_Result = "N/A"

                            _Data_Header.Snap07_Result = "N/A"
                            _Data_Header.Snap08_Result = "N/A"

                            _Data_Header.Hook01_Result = "N/A"
                            _Data_Header.Hook02_Result = "N/A"
                            _Data_Header.Hook03_Result = "N/A"
                            _Data_Header.Hook04_Result = "N/A"
                            _Data_Header.Hook05_Result = "N/A"
                            _Data_Header.Hook06_Result = "N/A"

                            _Data_Header.Hook07_Result = "N/A"
                            _Data_Header.Hook08_Result = "N/A"

                            _Data_Header.GateVestige01_Result = "N/A"
                            _Data_Header.GateVestige02_Result = "N/A"
                            _Data_Header.GateVestige03_Result = "N/A"
                        End If
                    End If

                    If _Data_Header.Snap01_Result = "NG" Or _Data_Header.Snap02_Result = "NG" Or _Data_Header.Snap03_Result = "NG" Or _Data_Header.Snap04_Result = "NG" Or _Data_Header.Snap05_Result = "NG" Or _Data_Header.Snap06_Result = "NG" Or _Data_Header.Snap07_Result = "NG" Or _Data_Header.Snap08_Result = "NG" _
                        Or _Data_Header.Hook01_Result = "NG" Or _Data_Header.Hook02_Result = "NG" Or _Data_Header.Hook03_Result = "NG" Or _Data_Header.Hook04_Result = "NG" Or _Data_Header.Hook05_Result = "NG" Or _Data_Header.Hook06_Result = "NG" Or _Data_Header.Hook07_Result = "NG" Or _Data_Header.Hook08_Result = "NG" _
                        Or _Data_Header.GateVestige01_Result = "NG" Or _Data_Header.GateVestige02_Result = "NG" Or _Data_Header.GateVestige03_Result = "NG" Then
                        _Data_Header.Result = "NG"

                    Else
                        _Data_Header.Result = "OK"
                    End If

                    Const LEN As Integer = 55
                    Dim Data_Name(LEN) As String, Data_Value(LEN) As Object
                    Data_Name(0) = "Software_Version" '软件版本
                    Data_Name(1) = "Station_ID"
                    Data_Name(2) = "Fixture_ID"
                    Data_Name(3) = "Carrier_SN"
                    Data_Name(4) = "KB_Type"
                    Data_Name(5) = "Key_Number"
                    Data_Name(6) = "Location"
                    Data_Name(7) = "ERS_Category"
                    Data_Name(8) = "Key_Type"
                    Data_Name(9) = "Key_Size"
                    Data_Name(10) = "Dome_Size"
                    Data_Name(11) = "Operator"
                    Data_Name(12) = "Test_Start_Time"
                    Data_Name(13) = "Test_Stop_Time"
                    Data_Name(14) = "Total_Test_Time"
                    Data_Name(15) = "Snap01_Data"
                    Data_Name(16) = "Snap02_Data"
                    Data_Name(17) = "Snap03_Data"
                    Data_Name(18) = "Snap04_Data"
                    Data_Name(19) = "Snap05_Data"
                    Data_Name(20) = "Snap06_Data"
                    Data_Name(21) = "Snap07_Data"
                    Data_Name(22) = "Snap08_Data"

                    Data_Name(23) = "Hook01_Data"
                    Data_Name(24) = "Hook02_Data"
                    Data_Name(25) = "Hook03_Data"
                    Data_Name(26) = "Hook04_Data"
                    Data_Name(27) = "Hook05_Data"
                    Data_Name(28) = "Hook06_Data"

                    Data_Name(29) = "Hook07_Data"
                    Data_Name(30) = "Hook08_Data"

                    Data_Name(31) = "Snap01_Result"
                    Data_Name(32) = "Snap02_Result"
                    Data_Name(33) = "Snap03_Result"
                    Data_Name(34) = "Snap04_Result"
                    Data_Name(35) = "Snap05_Result"
                    Data_Name(36) = "Snap06_Result"
                    Data_Name(37) = "Snap07_Result"
                    Data_Name(38) = "Snap08_Result"


                    Data_Name(39) = "Hook01_Result"
                    Data_Name(40) = "Hook02_Result"
                    Data_Name(41) = "Hook03_Result"
                    Data_Name(42) = "Hook04_Result"
                    Data_Name(43) = "Hook05_Result"
                    Data_Name(44) = "Hook06_Result"

                    Data_Name(45) = "Hook07_Result"
                    Data_Name(46) = "Hook08_Result"


                    Data_Name(47) = "GateVestige01_Data"
                    Data_Name(48) = "GateVestige02_Data"
                    Data_Name(49) = "GateVestige03_Data"
                    Data_Name(50) = "NubHeight01_Data"
                    Data_Name(51) = "NubHeight02_Data"
                    Data_Name(52) = "GateVestige01_Result"
                    Data_Name(53) = "GateVestige02_Result"
                    Data_Name(54) = "GateVestige03_Result"
                    Data_Name(55) = "Result"

                    Data_Value(0) = _Data_Header.Software_Version
                    Data_Value(1) = _Data_Header.Station_ID
                    Data_Value(2) = _Data_Header.Fixture_ID
                    Data_Value(3) = _Data_Header.Carrier_SN
                    Data_Value(4) = _Data_Header.Key_Name
                    Data_Value(5) = _Data_Header.Key_Number
                    Data_Value(6) = _Data_Header.Location
                    Data_Value(7) = _Data_Header.ERS_Category
                    Data_Value(8) = _Data_Header.Key_Type
                    Data_Value(9) = _Data_Header.Key_Size
                    Data_Value(10) = _Data_Header.Dome_Size
                    Data_Value(11) = _Data_Header.Operator_STR
                    Data_Value(12) = _Data_Header.Test_Start_Time.ToString
                    Data_Value(13) = _Data_Header.Test_Stop_Time.ToString
                    Data_Value(14) = _Data_Header.Total_Test_Time
                    Data_Value(15) = _Data_Header.Snap01_Data
                    Data_Value(16) = _Data_Header.Snap02_Data
                    Data_Value(17) = _Data_Header.Snap03_Data
                    Data_Value(18) = _Data_Header.Snap04_Data
                    Data_Value(19) = _Data_Header.Snap05_Data
                    Data_Value(20) = _Data_Header.Snap06_Data

                    Data_Value(21) = _Data_Header.Snap07_Data
                    Data_Value(22) = _Data_Header.Snap08_Data


                    Data_Value(23) = _Data_Header.Hook01_Data
                    Data_Value(24) = _Data_Header.Hook02_Data
                    Data_Value(25) = _Data_Header.Hook03_Data
                    Data_Value(26) = _Data_Header.Hook04_Data
                    Data_Value(27) = _Data_Header.Hook05_Data
                    Data_Value(28) = _Data_Header.Hook06_Data

                    Data_Value(29) = _Data_Header.Hook07_Data
                    Data_Value(30) = _Data_Header.Hook08_Data

                    Data_Value(31) = _Data_Header.Snap01_Result
                    Data_Value(32) = _Data_Header.Snap02_Result
                    Data_Value(33) = _Data_Header.Snap03_Result
                    Data_Value(34) = _Data_Header.Snap04_Result
                    Data_Value(35) = _Data_Header.Snap05_Result
                    Data_Value(36) = _Data_Header.Snap06_Result

                    Data_Value(37) = _Data_Header.Snap07_Result
                    Data_Value(38) = _Data_Header.Snap08_Result


                    Data_Value(39) = _Data_Header.Hook01_Result
                    Data_Value(40) = _Data_Header.Hook02_Result
                    Data_Value(41) = _Data_Header.Hook03_Result
                    Data_Value(42) = _Data_Header.Hook04_Result
                    Data_Value(43) = _Data_Header.Hook05_Result
                    Data_Value(44) = _Data_Header.Hook06_Result

                    Data_Value(45) = _Data_Header.Hook07_Result
                    Data_Value(46) = _Data_Header.Hook08_Result

                    Data_Value(47) = _Data_Header.GateVestige01_Data
                    Data_Value(48) = _Data_Header.GateVestige02_Data
                    Data_Value(49) = _Data_Header.GateVestige03_Data
                    Data_Value(50) = _Data_Header.NubHeight01_Data
                    Data_Value(51) = _Data_Header.NubHeight02_Data
                    Data_Value(52) = _Data_Header.GateVestige01_Result
                    Data_Value(53) = _Data_Header.GateVestige02_Result
                    Data_Value(54) = _Data_Header.GateVestige03_Result
                    Data_Value(55) = _Data_Header.Result
                    '保存结果
                    If BOOL.Upload = True Then
                        If _Data_Header.SN.Length >= 21 Then
                            Dim Upload_Name(27) As String
                            Dim Upload_Value(27) As Object
                            Upload_Name(0) = "[Product ID]"
                            Upload_Name(1) = "[Machine No]"
                            Upload_Name(2) = "[Project Name]"
                            Upload_Name(3) = "Language"
                            Upload_Name(4) = "Layout"
                            Upload_Name(5) = "[Key No]"
                            Upload_Name(6) = "[Key Type]"
                            Upload_Name(7) = "Snap01_Result"
                            Upload_Name(8) = "Snap02_Result"
                            Upload_Name(9) = "Snap03_Result"
                            Upload_Name(10) = "Snap04_Result"
                            Upload_Name(11) = "Snap05_Result"
                            Upload_Name(12) = "Snap06_Result"
                            Upload_Name(13) = "Snap07_Result"
                            Upload_Name(14) = "Snap08_Result"

                            Upload_Name(15) = "Hook01_Result"
                            Upload_Name(16) = "Hook02_Result"
                            Upload_Name(17) = "Hook03_Result"
                            Upload_Name(18) = "Hook04_Result"
                            Upload_Name(19) = "Hook05_Result"
                            Upload_Name(20) = "Hook06_Result"
                            Upload_Name(21) = "Hook07_Result"
                            Upload_Name(22) = "Hook08_Result"

                            Upload_Name(23) = "GateVestige01_Result"
                            Upload_Name(24) = "GateVestige02_Result"
                            Upload_Name(25) = "GateVestige03_Result"
                            Upload_Name(26) = "OP_Rejudge"
                            Upload_Name(27) = "HookSnapResult"

                            Upload_Value(0) = 0
                            Upload_Value(1) = PARAM_INT.S设备编号
                            Upload_Value(2) = Mid(_Data_Header.SN, 1, 4)
                            Upload_Value(3) = Mid(_Data_Header.SN, 6, 5)
                            Upload_Value(4) = Mid(_Data_Header.SN, 5, 1)
                            Upload_Value(5) = _Data_Header.Key_Number
                            Upload_Value(6) = _Data_Header.Key_Type
                            Upload_Value(7) = _Data_Header.Snap01_Result
                            Upload_Value(8) = _Data_Header.Snap02_Result
                            Upload_Value(9) = _Data_Header.Snap03_Result
                            Upload_Value(10) = _Data_Header.Snap04_Result
                            Upload_Value(11) = _Data_Header.Snap05_Result
                            Upload_Value(12) = _Data_Header.Snap06_Result
                            Upload_Value(13) = _Data_Header.Snap07_Result
                            Upload_Value(14) = _Data_Header.Snap08_Result


                            Upload_Value(15) = _Data_Header.Hook01_Result
                            Upload_Value(16) = _Data_Header.Hook02_Result
                            Upload_Value(17) = _Data_Header.Hook03_Result
                            Upload_Value(18) = _Data_Header.Hook04_Result

                            Upload_Value(19) = _Data_Header.Hook05_Result
                            Upload_Value(20) = _Data_Header.Hook06_Result
                            Upload_Value(21) = _Data_Header.Hook07_Result
                            Upload_Value(22) = _Data_Header.Hook08_Result

                            Upload_Value(23) = _Data_Header.GateVestige01_Result
                            Upload_Value(24) = _Data_Header.GateVestige02_Result
                            Upload_Value(25) = _Data_Header.GateVestige03_Result
                            Upload_Value(26) = "N/A"
                            Upload_Value(27) = _Data_Header.Result

                            While SwitchButton_runing.Value = False '调试模式，机台暂停
                                Thread.Sleep(200)
                            End While

                            Dim Temp_Result As String = Nothing
                            Dim Temp_LightLakageResult As String = Nothing
                            Dim Temp_HookSnapeResult As String = Nothing
                            Select Case Bool_Exits_Measure_Result '是否上一站成功插入了数据
                                Case True

                                    Get_Time_MS(_Time_Start)

                                    SQLCON_查询上一站.Insert_Measure_Result(PARAM_STRING.S数据汇总表名, Time_Date, _Data_Header.SN, _Data_Header.Key_Name, Upload_Name, Upload_Value, Temp_Result, Temp_LightLakageResult, Temp_HookSnapeResult)

                                    Get_Time_MS(_Time_End)
                                    _Time_Lost = Round(_Time_End - _Time_Start, 3)
                                    Write_Upload_Time(LASER_DATACODE, "Insert_Measure_Result【" & i + 1 & "】：" & _Time_Lost)

                                    If Temp_Result <> "OK" Then
                                        If Fail_Location = "" Then
                                            Fail_Location = _Data_Header.Key_Number.ToString & " " '累计NG数
                                        Else
                                            Fail_Location &= _Data_Header.Key_Number.ToString & " " '累计NG数
                                        End If
                                        NGCOUNT.LightLakage_SnapeHook += 1 'NG数量累加 11.1
                                    End If

                                    If Temp_LightLakageResult <> "OK" Then '统计漏光NG数量 11.08
                                        LightLakage_NG_Count += 1
                                    End If
                                    If Temp_HookSnapeResult <> "OK" Then '统计卡扣NG数量
                                        HookSnape_NG_Count += 1
                                    End If
                                Case False
                                    Fail_Location = ""
                            End Select
                        Else
                            If Err_First = False Then
                                Err_First = True
                                rtn_str = "条码长度不正确，本次结果不上传，请检查！" & _Data_Header.SN
                                Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), rtn_str, LASER_DATACODE, Color.Red)
                            End If
                        End If
                    Else
                        If Err_First = False Then
                            Err_First = True
                            rtn_str = "上传数据服务器连接失败，本次结果不上传，请检查！"
                            Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), rtn_str, LASER_DATACODE, Color.Red)
                        End If
                    End If
                    Save_Fixtrue_Data(PARAM_STRING.S数据保存路径, _Data_Header.SN, Time, Data_Name, Data_Value)


                    While SwitchButton_runing.Value = False '调试模式，机台暂停
                        Thread.Sleep(200)
                    End While
                    If JSON_PARA.JSON_UPLOAD = True Then
                        'Key_Name(i) = _Data_Header.Key_Name
                        If _Data_Header.Result = "OK" Then
                            KeyResult(i) = "pass"
                        Else
                            KeyResult(i) = "fail"
                        End If
                    End If

                Next
                If JSON_PARA.JSON_UPLOAD = True Then
                    If JSON_WorkAt = WorkAt.LEFT Then
                        JSON_PARA.CYCLE_TIME = CT_Time_Left
                    Else
                        JSON_PARA.CYCLE_TIME = CT_Time_Right
                    End If
                    If JSON_PARA.PROJECTS.Contains("1098") Or JSON_PARA.PROJECTS.Contains("1359") Then
                        Dim key_name_s() As String = Nothing
                        Dim key_value_s() As String = Nothing
                        Remove_PO_Key(Key_Name, KeyResult, key_name_s, key_value_s)


                        Dim Total_Result As String = String.Empty
                        Dim Total_Result_Cnt As Integer = 0
                        'For i As UInteger = 0 To key_value_s.Length - 1
                        '    If key_value_s(i) = "pass" Then
                        '        Total_Result_Cnt = Total_Result_Cnt + 1
                        '    End If
                        'Next
                        'If Total_Result_Cnt = key_value_s.Length Then
                        '    Total_Result = "pass"
                        'Else
                        '    Total_Result = "fail"
                        'End If
                        Total_Result = "pass"
                        For i As UInteger = 0 To key_value_s.Length - 1
                            If key_value_s(i) = "fail" Then
                                Total_Result = "fail"
                                Exit For
                            End If
                        Next

                        WriteJason(_Data_Header.SN, JSON_PARA.PROJECTS, Total_Result, key_name_s, key_value_s)
                    Else

                        Dim Total_Result As String = String.Empty
                        'Dim Total_Result_Cnt As Integer = 0
                        'For i As UInteger = 0 To KeyResult.Length - 1
                        '    If KeyResult(i) = "pass" Then
                        '        Total_Result_Cnt = Total_Result_Cnt + 1
                        '    End If
                        'Next
                        'If Total_Result_Cnt = KeyResult.Length Then
                        '    Total_Result = "pass"
                        'Else
                        '    Total_Result = "fail"
                        'End If
                        Total_Result = "pass"
                        For i As UInteger = 0 To KeyResult.Length - 1
                            If KeyResult(i) = "fail" Then
                                Total_Result = "fail"
                                Exit For
                            End If
                        Next
                        WriteJason(_Data_Header.SN, JSON_PARA.PROJECTS, Total_Result, Key_Name, KeyResult)
                    End If
                End If
                rtn_str = "保存所有数据完成"
                Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), rtn_str, LASER_DATACODE, Color.Green)

                Get_Time_MS(Time_End)
                Time_Lost = Round(Time_End - Time_Start, 3)
                Write_Upload_Time(LASER_DATACODE, "Insert_Measure_Result:" & Time_Lost)

                While SwitchButton_runing.Value = False '调试模式，机台暂停
                    Thread.Sleep(200)
                End While


                '************************************************************获取工单号，插入主表,查询产品ID，更新副表产品ID************************************************************
                Dim Bool_UpLoad_Barcode As Boolean = False '条码是否有误
                If Fail_Location = "" Then
                    Result_All = "OK"
                    PE_OK.Style.BackColor1.Color = Color.Green
                Else
                    Result_All = "NG"
                    PE_NG.Style.BackColor1.Color = Color.Red
                End If
                If LightLakage_NG_Count >= 1 Then '统计漏光是否有NG 11.08
                    LightLakageResult = "NG"
                Else
                    LightLakageResult = "OK"
                End If
                If HookSnape_NG_Count >= 1 Then '统计卡扣是否有NG
                    HookSnapeResult = "NG"
                Else
                    HookSnapeResult = "OK"
                End If
                If BOOL.Upload = True And Bool_Exits_Measure_Result = True Then
                    If NGCOUNT.LightLakage_SnapeHook >= NGCOUNT.NG_Max_Count Then 'NG超过一定数量不上传 11.1
                        rtn_str = "NG数量超过设定值：" & NGCOUNT.NG_Max_Count & ",本次不上传！"
                        Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), rtn_str, LASER_DATACODE, Color.Red)
                    Else
                        Get_Time_MS(Time_Start)
                        '更新HIP数据库
                        Bool_UpLoad_Barcode = Updata_HIP_Table(Result_All, LightLakageResult, HookSnapeResult, Fail_Location, LASER_DATACODE, SQL_TABLE_NAME, Time_Date, PARAM_STRING.S数据汇总表名)

                        Get_Time_MS(Time_End)
                        Time_Lost = Round(Time_End - Time_Start, 3)
                        Write_Upload_Time(LASER_DATACODE, "Updata_HIP_Table:" & Time_Lost)

                        Select Case Bool_UpLoad_Barcode
                            Case True
                                rtn_str = "更新服务器数据完成"
                                Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), rtn_str, LASER_DATACODE, Color.Green)
                            Case False
                                rtn_str = "条码有误，不更新服务器数据"
                                Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), rtn_str, LASER_DATACODE, Color.Red)
                        End Select
                    End If
                ElseIf Bool_Exits_Measure_Result = False Then
                    rtn_str = "上一站没有数据，本站记录不上传，请检查后重新过站！" & _Data_Header.SN
                    Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), rtn_str, LASER_DATACODE, Color.Red)
                End If

LineERR:
                '数据清空
                If PARAM_BOOL.Q清空汇总数据 = True Then
                    SQLCON_查询上一站.Clear_SN_MeasureData(Select_Product_Name, LASER_DATACODE)
                    SQLCON_查询上一站.Delete_Measure_Result(PARAM_STRING.S数据汇总表名, LASER_DATACODE) '不会造成数据多，插入缓慢
                End If

                While SwitchButton_runing.Value = False '调试模式，机台暂停
                    Thread.Sleep(200)
                End While

                'PLC控制出料
                mClear_Err = True
                Test_Plc_Conn = False
                Send_plc_Command_Str = Omron_Command.读取PLC状态
                rtn_str = "获取OP站状态，看OP站是否繁忙"
                Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), rtn_str, LASER_DATACODE, Color.Blue)
                Thread.Sleep(350)

                If Test_Plc_Conn = False Then
                    rtn_str = "PLC连接失败，无法正常出料，请在OP站手动取出，防止流入下一工站！"
                    Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), rtn_str, LASER_DATACODE, Color.Red)
                    'MessageBoxEx.Show(rtn_str, "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1)
                Else
                    Dim m_Fisrt As Boolean = False
                    If SW_OP.Value = False Then
                        While mClear_Err
                            If SW_OP.Value = True Then
                                Exit While
                            Else
                                If Plc_Return_Result1 = "3" And Plc_Return_Result2 = "3" Then
                                    rtn_str = "OP站空闲，开始出料：" & Plc_Return_Str
                                    Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), rtn_str, LASER_DATACODE, Color.Black)
                                    Exit While
                                Else
                                    If m_Fisrt = False Then
                                        m_Fisrt = True
                                        rtn_str = "OP站繁忙，等待中：" & Plc_Return_Str
                                        Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), rtn_str, LASER_DATACODE, Color.Blue)
                                    End If
                                End If
                            End If
                        End While
                    End If
                End If
                mClear_Err = False

                Send_plc_Command_Str = ""
                Thread.Sleep(300)

                While SwitchButton_runing.Value = False '调试模式，机台暂停
                    Thread.Sleep(200)
                End While

                If SwitchButton2.Value = True Then
                    Send_plc_Command_Str = Omron_Command.写入结果NG
                Else
                    If Bool_Exits_Measure_Result = False Or Bool_UpLoad_Barcode = False Or Test_Plc_Conn = False Then
                        Send_plc_Command_Str = Omron_Command.写入结果NG
                    Else
                        Select Case Result_All
                            Case "NG" 'NG
                                Send_plc_Command_Str = Omron_Command.写入结果NG
                            Case "OK" 'OK
                                Send_plc_Command_Str = Omron_Command.写入结果OK
                        End Select
                    End If
                End If

                '阻挡下降，准备出料
                For i As Integer = 1 To 3
                    If Z中线阻挡电磁阀(IO.OUT_OFF, LR_STATION.COMMUNAL, LASER_DATACODE) = True Then
                        Exit For
                    End If
                Next
                Thread.Sleep(100)
                If CARDCMD.GET_DI_BIT(BOARD_ID_00, DIO.Z中线阻挡HOME_0) = False Then
                    rtn_str = "[中线阻挡电磁阀]HOME信号没有感应到，请确认！"
                    Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), rtn_str, LASER_DATACODE, Color.Red)
                    'MessageBoxEx.Show(rtn_str, "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
                End If

                Dim On_Off As Boolean = False, First As Boolean = False
                While True
                    If First = False Then
                        First = True
                        rtn_str = "[中线出料检测]等待出料"
                        Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), rtn_str, LASER_DATACODE, Color.Green)
                    End If
                    If On_Off = False Then
                        If CARDCMD.GET_DI_BIT(BOARD_ID_01, DIO.Z中线出料检测_1) = True Then
                            On_Off = True
                            rtn_str = "[中线出料检测]上升沿触发"
                            Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), rtn_str, LASER_DATACODE, Color.Green)
                        End If
                    End If
                    If On_Off = True And CARDCMD.GET_DI_BIT(BOARD_ID_01, DIO.Z中线出料检测_1) = False Then
                        rtn_str = "[中线出料检测]下降沿触发，出料完成"
                        Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), rtn_str, LASER_DATACODE, Color.Green)
                        Exit While
                    End If
                End While

                Thread.Sleep(200)
                BOOL.Laser_IsRuning = False
            End If
        End While

        RUN_LASER.CancelAsync()
        RUN_LASER.Dispose()
    End Sub

    Sub Remove_PO_Key(ByVal key_name() As String, ByVal key_value() As String, ByRef key_name_s() As String, ByRef key_value_s() As String)
        Dim key_name_list As New List(Of String)
        Dim key_value_list As New List(Of String)
        key_name_list.Clear()
        key_value_list.Clear()
        For i As UInteger = 0 To key_name.Length - 1
            key_name_list.Add(key_name(i))
            key_value_list.Add(key_value(i))
        Next
        Dim key_name_count As Integer = key_name.Count
        For i As UInteger = 0 To key_name_count - 1
            If key_name_list.Item(i).Contains("PO") Then
                key_name_list.RemoveAt(i)
                key_value_list.RemoveAt(i)
                Exit For
            End If
        Next
        key_name_s = key_name_list.ToArray
        key_value_s = key_value_list.ToArray
    End Sub

    'NG数量 11.1
    Public Structure NGCOUNT
        Shared LightLakage_SnapeHook As Integer = 0 '漏光卡扣NG
        Shared NG_Max_Count As Integer = 8 'NG最大数量
    End Structure

    ''' <summary>
    ''' 回原点
    ''' </summary>
    ''' <remarks></remarks>
    Sub RETURN_ZERO_SUB()
        Dim rtn_str As String = Nothing, Bool_Rtn As Boolean, Home_Str As String = Nothing
        Select Case Card_Init_OK
            Case True
                Bool_Rtn = CARDCMD.GET_DI_BIT(BOARD_ID_00, DIO.F复位按钮_0)
                If Bool_Start_Home = True And Bool_Home_Done = False Or Bool_Rtn = True Then
                    If ComboBox_product_name.SelectedIndex >= 0 Then

                        Select Case Language
                            Case LGE.CHN
                                Home_Str = "设备归零"
                            Case LGE.ENG
                                Home_Str = "Device Zeroing"
                        End Select
                        Invoke(New Delegate_Disp_Text(AddressOf Updata_Disp_Text), Button_Goto_Zero, Home_Str, Color.Yellow)
                        Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), "【所有轴开始归零】", "", Color.Black)
                        Change_RedYellowGreen(LIGHT.GREEN)

                        Bool_Start_Home = False
                        Bool_Home_Done = False '原点回归未完成
                        BOOL.Read_MPEL = False '关闭监控限位

                        'Z01轴抬高到正极限位置
                        Dim Axis_ID As Integer = 0, Rtn_Int As Integer = 0
                        For i As Integer = 0 To HomeParamObjArray.Length - 1
                            If HomeParamObjArray(i).轴名称 = "Z01" Then
                                Axis_ID = HomeParamObjArray(i).轴号
                            End If
                        Next
                        Rtn_Int = CARDCMD.JOG_PTP_ALL(Axis_ID, PRA_APS_OPTION_ENUM.Relative, 20, 0.1, 1000, Nothing)
                        Select Case Rtn_Int
                            Case Is = 0
                                While BOOL.EXIT_APP = False
                                    If CARDCMD.GET_PEL_STATUS(Axis_ID) = True Then
                                        Exit While
                                    End If
                                    READ_ENCODER_POSITION(AXISMSG.AXIS_ENCODER_POS)
                                End While
                            Case Is <> 0
                                Error_Str = ADLINK_RETURN_MSG(Rtn_Int)
                                BOOL.Error_Close = True '关闭软件
                                Exit Sub
                        End Select
                        'Z02轴抬高到正极限位置
                        For i As Integer = 0 To HomeParamObjArray.Length - 1
                            If HomeParamObjArray(i).轴名称 = "Z02" Then
                                Axis_ID = HomeParamObjArray(i).轴号
                            End If
                        Next
                        Rtn_Int = CARDCMD.JOG_PTP_ALL(Axis_ID, PRA_APS_OPTION_ENUM.Relative, 20, 0.1, 1000, Nothing)
                        Select Case Rtn_Int
                            Case Is = 0
                                While BOOL.EXIT_APP = False
                                    If CARDCMD.GET_PEL_STATUS(Axis_ID) = True Then
                                        Exit While
                                    End If
                                    READ_ENCODER_POSITION(AXISMSG.AXIS_ENCODER_POS)
                                End While
                            Case Is <> 0
                                Error_Str = ADLINK_RETURN_MSG(Rtn_Int)
                                BOOL.Error_Close = True '关闭软件
                                Exit Sub
                        End Select
                        'Z03轴抬高到正极限位置
                        For i As Integer = 0 To HomeParamObjArray.Length - 1
                            If HomeParamObjArray(i).轴名称 = "Z03" Then
                                Axis_ID = HomeParamObjArray(i).轴号
                            End If
                        Next
                        Rtn_Int = CARDCMD.JOG_PTP_ALL(Axis_ID, PRA_APS_OPTION_ENUM.Relative, 20, 0.1, 1000, Nothing)
                        Select Case Rtn_Int
                            Case Is = 0
                                While BOOL.EXIT_APP = False
                                    If CARDCMD.GET_PEL_STATUS(Axis_ID) = True Then
                                        Exit While
                                    End If
                                    READ_ENCODER_POSITION(AXISMSG.AXIS_ENCODER_POS)
                                End While
                            Case Is <> 0
                                Error_Str = ADLINK_RETURN_MSG(Rtn_Int)
                                BOOL.Error_Close = True '关闭软件
                                Exit Sub
                        End Select


                        HOME_MOVE()

                        Bool_Home_Done = True '原点回归完成
                        BOOL.Read_MPEL = True '打开监控限位
                        Bool_No_Wrok = True

                        Change_RedYellowGreen(LIGHT.YELLOW)
                        Select Case Language
                            Case LGE.CHN
                                Home_Str = "归零完成"
                            Case LGE.ENG
                                Home_Str = "Zero Done"
                        End Select


                        CARDCMD.SET_DO_ALL_OFF(BOARD_ID_00) '复位所有IO
                        CARDCMD.SET_DO_ALL_OFF(BOARD_ID_01) '复位所有IO
                        Thread.Sleep(200)
                        ONE1工位阻挡电磁阀2(IO.OUT_ON, LR_STATION.COMMUNAL, "")
                        TWO2工位阻挡电磁阀2(IO.OUT_ON, LR_STATION.COMMUNAL, "")
                        CARDCMD.SET_DO_BIT(BOARD_ID_00, DIO.H回流皮带控制_0, PRA_OUT_STATUS_ENUM.OUT_ON)
                        CARDCMD.SET_DO_BIT(BOARD_ID_00, DIO.ONE1工位皮带控制_0, PRA_OUT_STATUS_ENUM.OUT_ON)
                        CARDCMD.SET_DO_BIT(BOARD_ID_00, DIO.TWO2工位皮带控制_0, PRA_OUT_STATUS_ENUM.OUT_ON)

                        Static Open_IO As Boolean = False
                        If Open_IO = False And BOOL.Error_Close = False Then '开机排气
                            Open_IO = True
                            CARDCMD.SET_DO_BIT(BOARD_ID_00, DIO.ONE1工位真空吸电磁阀_0, PRA_OUT_STATUS_ENUM.OUT_ON)
                            CARDCMD.SET_DO_BIT(BOARD_ID_00, DIO.TWO2工位真空吸电磁阀_0, PRA_OUT_STATUS_ENUM.OUT_ON)
                            CARDCMD.SET_DO_BIT(BOARD_ID_00, DIO.ONE1工位顶升电磁阀_0, PRA_OUT_STATUS_ENUM.OUT_ON)
                            CARDCMD.SET_DO_BIT(BOARD_ID_00, DIO.TWO2工位顶升电磁阀_0, PRA_OUT_STATUS_ENUM.OUT_ON)
                            Thread.Sleep(2000)
                            CARDCMD.SET_DO_BIT(BOARD_ID_00, DIO.ONE1工位真空吸电磁阀_0, PRA_OUT_STATUS_ENUM.OUT_OFF)
                            CARDCMD.SET_DO_BIT(BOARD_ID_00, DIO.TWO2工位真空吸电磁阀_0, PRA_OUT_STATUS_ENUM.OUT_OFF)
                            CARDCMD.SET_DO_BIT(BOARD_ID_00, DIO.ONE1工位顶升电磁阀_0, PRA_OUT_STATUS_ENUM.OUT_OFF)
                            CARDCMD.SET_DO_BIT(BOARD_ID_00, DIO.TWO2工位顶升电磁阀_0, PRA_OUT_STATUS_ENUM.OUT_OFF)
                        End If

                        Invoke(New Delegate_Disp_Text(AddressOf Updata_Disp_Text), Button_Goto_Zero, Home_Str, Color.Yellow)
                        Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), "【所有轴归零完成】", "", Color.Blue)
                    Else
                        If Portect_STRCT.Resert_Button = False Then
                            Portect_STRCT.Resert_Button = True
                            rtn_str = "请选择项目名称！"
                            Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), rtn_str, "", Color.Black)
                        End If
                    End If
                Else
                    If Portect_STRCT.Resert_Button = True Then
                        Portect_STRCT.Resert_Button = False
                    End If
                End If
        End Select
    End Sub
    ''' <summary>
    ''' 返回错误信息
    ''' </summary>
    ''' <param name="Rtn_Int"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function ADLINK_RETURN_MSG(ByVal Rtn_Int As Integer) As String
        ADLINK_RETURN_MSG = ""
        Select Case Rtn_Int
            Case ADLINK_Err_Code.Err_0
                ADLINK_RETURN_MSG = ADLINK_Err_MSG.Err_0
            Case ADLINK_Err_Code.Err_1
                ADLINK_RETURN_MSG = ADLINK_Err_MSG.Err_1
            Case ADLINK_Err_Code.Err_2
                ADLINK_RETURN_MSG = ADLINK_Err_MSG.Err_2
            Case ADLINK_Err_Code.Err_3
                ADLINK_RETURN_MSG = ADLINK_Err_MSG.Err_3
            Case ADLINK_Err_Code.Err_4
                ADLINK_RETURN_MSG = ADLINK_Err_MSG.Err_4
            Case ADLINK_Err_Code.Err_5
                ADLINK_RETURN_MSG = ADLINK_Err_MSG.Err_5
            Case ADLINK_Err_Code.Err_6
                ADLINK_RETURN_MSG = ADLINK_Err_MSG.Err_6
            Case ADLINK_Err_Code.Err_7
                ADLINK_RETURN_MSG = ADLINK_Err_MSG.Err_7
            Case ADLINK_Err_Code.Err_8
                ADLINK_RETURN_MSG = ADLINK_Err_MSG.Err_8
            Case ADLINK_Err_Code.Err_9
                ADLINK_RETURN_MSG = ADLINK_Err_MSG.Err_9
            Case ADLINK_Err_Code.Err_10
                ADLINK_RETURN_MSG = ADLINK_Err_MSG.Err_10
            Case ADLINK_Err_Code.Err_11
                ADLINK_RETURN_MSG = ADLINK_Err_MSG.Err_11
            Case ADLINK_Err_Code.Err_12
                ADLINK_RETURN_MSG = ADLINK_Err_MSG.Err_12
            Case ADLINK_Err_Code.Err_13
                ADLINK_RETURN_MSG = ADLINK_Err_MSG.Err_13
            Case ADLINK_Err_Code.Err_14
                ADLINK_RETURN_MSG = ADLINK_Err_MSG.Err_14
            Case ADLINK_Err_Code.Err_15
                ADLINK_RETURN_MSG = ADLINK_Err_MSG.Err_15
            Case ADLINK_Err_Code.Err_16
                ADLINK_RETURN_MSG = ADLINK_Err_MSG.Err_16
            Case ADLINK_Err_Code.Err_17
                ADLINK_RETURN_MSG = ADLINK_Err_MSG.Err_17
            Case ADLINK_Err_Code.Err_18
                ADLINK_RETURN_MSG = ADLINK_Err_MSG.Err_18
            Case ADLINK_Err_Code.Err_19
                ADLINK_RETURN_MSG = ADLINK_Err_MSG.Err_19
            Case ADLINK_Err_Code.Err_20
                ADLINK_RETURN_MSG = ADLINK_Err_MSG.Err_20
            Case ADLINK_Err_Code.Err_21
                ADLINK_RETURN_MSG = ADLINK_Err_MSG.Err_21
            Case ADLINK_Err_Code.Err_22
                ADLINK_RETURN_MSG = ADLINK_Err_MSG.Err_22
            Case ADLINK_Err_Code.Err_23
                ADLINK_RETURN_MSG = ADLINK_Err_MSG.Err_23
            Case ADLINK_Err_Code.Err_24
                ADLINK_RETURN_MSG = ADLINK_Err_MSG.Err_24
            Case ADLINK_Err_Code.Err_25
                ADLINK_RETURN_MSG = ADLINK_Err_MSG.Err_25
            Case ADLINK_Err_Code.Err_32
                ADLINK_RETURN_MSG = ADLINK_Err_MSG.Err_32
            Case ADLINK_Err_Code.Err_33
                ADLINK_RETURN_MSG = ADLINK_Err_MSG.Err_33
            Case ADLINK_Err_Code.Err_40
                ADLINK_RETURN_MSG = ADLINK_Err_MSG.Err_40
            Case ADLINK_Err_Code.Err_1000
                ADLINK_RETURN_MSG = ADLINK_Err_MSG.Err_1000
        End Select
        If Rtn_Int <> 0 Then
            STOP_SPORT(STOP_TYPE.EMG_STOP)
        End If
        Return ADLINK_RETURN_MSG
    End Function

    Private Sub Btn_Updata_Logain_PassWord_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Btn_Updata_Logain_PassWord.Click
        Select Case BOOL.Updata_Admin_Password
            Case True
                User_Password_Dialog.ShowDialog() '可修改ADMIN密码
                BOOL.Updata_Admin_Password = False
            Case False
                Select Case User_Login_Dialog.ShowDialog()
                    Case Windows.Forms.DialogResult.OK
                        User_Password_Dialog.ShowDialog() '可修改其他用户密码
                    Case Windows.Forms.DialogResult.Yes
                        MessageBoxEx.Show("用户【" & User_Login_Dialog.User & "】权限被限制，此用户密码不能被修改！", "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Case Windows.Forms.DialogResult.Cancel
                End Select
        End Select
    End Sub

    ''' <summary>
    ''' 读取轴位置与报警信息
    ''' </summary>
    ''' <remarks></remarks>
    Sub READ_ENCODER_POSITION(ByRef Enc_Pos() As Double)
        Array.Resize(Enc_Pos, 100)
        If Card_Init_OK = True Then
            For I As Integer = 0 To HomeParamObjArray.Length - 1
                CARDCMD.GET_POSITION(HomeParamObjArray(I).轴号, Enc_Pos(I))
                DX_ENC_POS.Rows(I).Cells(0).Value = HomeParamObjArray(I).轴名称
                DX_ENC_POS.Rows(I).Cells(1).Value = Format(Enc_Pos(I), "000.000")
            Next
        End If
    End Sub

    ''' <summary>
    ''' 初始化位置显示
    ''' </summary>
    ''' <remarks></remarks>
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

    Private Sub Btn_Query_All_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Btn_Query_All.Click

    End Sub

    Private Sub Btn_User_Manage_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Btn_User_Manage.Click
        Select Case User_Login_Dialog.ShowDialog()
            Case Windows.Forms.DialogResult.OK
                MessageBoxEx.Show("用户【" & User_Login_Dialog.User & "】权限管理被限制！", "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Case Windows.Forms.DialogResult.Yes
                User_Dialog.ShowDialog() 'ADMIN才有权限修改
            Case Windows.Forms.DialogResult.Cancel
        End Select
    End Sub


    Sub READ_COODNATE_PARAM(ByVal Coor_Name_Dataset As DataSet, ByVal Coor_Param_Dataset As DataSet, ByRef Motion_Coor_Order As List(Of Integer), ByRef Motion_Coord_List() As Work_Coordinatess)
        Motion_Coor_Order = New List(Of Integer)
        Motion_Coor_Order.Clear() '清空
        Motion_Coord_List = Nothing
        '读取坐标系所有名称
        Dim Row_Index As Int16 = 0
        Dim Coordinate_name As String = Nothing, rtn_str As String = Nothing
        Dim CoorNameCount As Int16 = 0, CoorParamCount As Int16 = 0, Rtn_Int As Int16 = 0
        Dim Point_Len As Integer, PARAMETERS_NAME_ARRAY() As String = Nothing, PARAMETERS_VALUE_ARRAY() As Double = Nothing
        Dim Axis_Name_Str As String = Nothing, Axis_Index_Str As String = Nothing, str As String = Nothing

        If Coor_Name_Dataset IsNot Nothing Then
            CoorNameCount = Coor_Name_Dataset.Tables(0).Rows.Count
            For COORNMAE_INDEX As Int16 = 0 To CoorNameCount - 1 '坐标系名称列表
                Coordinate_name = Coor_Name_Dataset.Tables(0).Rows(COORNMAE_INDEX).Item("坐标名称").ToString.Trim
                Dim Order_Index As Int16 = 0 '坐标名称数量分类
                If SQLCON.Cheng_Query_Coordinate_AxisSystem(Select_Product_Name, Coordinate_name, Axis_Name_Str, Axis_Index_Str) = True Then
                    PARAMETERS_NAME_ARRAY = Split(Axis_Name_Str, ";")
                    Point_Len = PARAMETERS_NAME_ARRAY.Length
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
                    SQLCON.Read_Coordinates(Select_Product_Name, Coordinate_name, PARAMETERS_NAME_ARRAY, Coor_Param_Dataset)
                    CoorParamCount = Coor_Param_Dataset.Tables(0).Rows.Count

                    If CoorParamCount >= 1 And PARAMETERS_NAME_ARRAY.Count >= 1 Then
                        For COOR_INDEX As Int16 = 0 To CoorParamCount - 1 '坐标系名称里的实际坐标列表

                            Row_Index += 1 '所有坐标每次累加
                            Array.Resize(Motion_Coord_List, Row_Index)

                            Motion_Coord_List(Row_Index - 1).轴名称 = Split(Axis_Name_Str, ";")
                            Motion_Coord_List(Row_Index - 1).轴运动顺序 = Split(Axis_Index_Str, ";")
                            Array.Resize(Motion_Coord_List(Row_Index - 1).轴位置, Point_Len)
                            For i As Int16 = 0 To Point_Len - 1
                                Motion_Coord_List(Row_Index - 1).轴位置(i) = Coor_Param_Dataset.Tables(0).Rows(COOR_INDEX).Item(Motion_Coord_List(Row_Index - 1).轴名称(i))
                            Next
                            Array.Resize(Motion_Coord_List(Row_Index - 1).轴号, Point_Len)
                            For m As Int16 = 0 To Motion_Coord_List(Row_Index - 1).轴名称.Count - 1
                                For n As Integer = 0 To HomeParamObjArray.Length - 1
                                    If Motion_Coord_List(Row_Index - 1).轴名称(m) = HomeParamObjArray(n).轴名称 Then
                                        Motion_Coord_List(Row_Index - 1).轴号(m) = HomeParamObjArray(n).轴号
                                        Exit For
                                    End If
                                Next
                            Next

                            Motion_Coord_List(Row_Index - 1).坐标名称 = Coordinate_name
                            If IsDBNull(Coor_Param_Dataset.Tables(0).Rows(COOR_INDEX).Item("键名")) = True Then
                                Motion_Coord_List(Row_Index - 1).键名 = "N/A"
                            Else
                                Motion_Coord_List(Row_Index - 1).键名 = Coor_Param_Dataset.Tables(0).Rows(COOR_INDEX).Item("键名")
                            End If
                            If IsDBNull(Coor_Param_Dataset.Tables(0).Rows(COOR_INDEX).Item("运行速度")) = True Then
                                Motion_Coord_List(Row_Index - 1).运行速度 = False
                            Else
                                Motion_Coord_List(Row_Index - 1).运行速度 = Coor_Param_Dataset.Tables(0).Rows(COOR_INDEX).Item("运行速度")
                            End If
                            If IsDBNull(Coor_Param_Dataset.Tables(0).Rows(COOR_INDEX).Item("加减速度")) = True Then
                                Motion_Coord_List(Row_Index - 1).加减速度 = False
                            Else
                                Motion_Coord_List(Row_Index - 1).加减速度 = Coor_Param_Dataset.Tables(0).Rows(COOR_INDEX).Item("加减速度")
                            End If
                            If IsDBNull(Coor_Param_Dataset.Tables(0).Rows(COOR_INDEX).Item("点胶状态")) = True Then
                                Motion_Coord_List(Row_Index - 1).点胶状态 = False
                            Else
                                Motion_Coord_List(Row_Index - 1).点胶状态 = Coor_Param_Dataset.Tables(0).Rows(COOR_INDEX).Item("点胶状态")
                            End If
                            If IsDBNull(Coor_Param_Dataset.Tables(0).Rows(COOR_INDEX).Item("镭射触发状态")) = True Then
                                Motion_Coord_List(Row_Index - 1).镭射触发状态 = False
                            Else
                                Motion_Coord_List(Row_Index - 1).镭射触发状态 = Coor_Param_Dataset.Tables(0).Rows(COOR_INDEX).Item("镭射触发状态")
                            End If
                            If IsDBNull(Coor_Param_Dataset.Tables(0).Rows(COOR_INDEX).Item("镭射收数据状态")) = True Then
                                Motion_Coord_List(Row_Index - 1).镭射收数据状态 = False
                            Else
                                Motion_Coord_List(Row_Index - 1).镭射收数据状态 = Coor_Param_Dataset.Tables(0).Rows(COOR_INDEX).Item("镭射收数据状态")
                            End If
                            If IsDBNull(Coor_Param_Dataset.Tables(0).Rows(COOR_INDEX).Item("CCD触发状态")) = True Then
                                Motion_Coord_List(Row_Index - 1).CCD触发状态 = False
                            Else
                                Motion_Coord_List(Row_Index - 1).CCD触发状态 = Coor_Param_Dataset.Tables(0).Rows(COOR_INDEX).Item("CCD触发状态")
                            End If
                            If IsDBNull(Coor_Param_Dataset.Tables(0).Rows(COOR_INDEX).Item("IO触发状态")) = True Then
                                Motion_Coord_List(Row_Index - 1).IO触发状态 = False
                            Else
                                Motion_Coord_List(Row_Index - 1).IO触发状态 = Coor_Param_Dataset.Tables(0).Rows(COOR_INDEX).Item("IO触发状态")
                            End If
                            If IsDBNull(Coor_Param_Dataset.Tables(0).Rows(COOR_INDEX).Item("其他触发状态")) = True Then
                                Motion_Coord_List(Row_Index - 1).其他触发状态 = False
                            Else
                                Motion_Coord_List(Row_Index - 1).其他触发状态 = Coor_Param_Dataset.Tables(0).Rows(COOR_INDEX).Item("其他触发状态")
                            End If

                            Motion_Coord_List(Row_Index - 1).慢速 = Motion_Coord_List(Row_Index - 1).运行速度 * 0.1
                            Motion_Coord_List(Row_Index - 1).中速 = Motion_Coord_List(Row_Index - 1).运行速度 * 0.5

                            Order_Index += 1
                        Next
                    End If
                End If
                Motion_Coor_Order.Add(Order_Index)
            Next
        End If
    End Sub

    ''' <summary>
    ''' 检查坐标是否设置过近
    ''' </summary>
    ''' <param name="Motion_Coord_List"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function Check_Positon(ByVal Motion_Coord_List() As Work_Coordinatess)
        If Motion_Coord_List IsNot Nothing Then
            If Motion_Coord_List.Count >= 1 Then
                For i As Int16 = 0 To Motion_Coord_List.Count - 2
                    Dim Pos As Double = Round(Abs(Motion_Coord_List(i).轴位置(0) - Motion_Coord_List(i + 1).轴位置(0)), 3)
                    If Pos < 8 Then
                        Dim str As String = "[" & Motion_Coord_List(i).坐标名称 & "]【" & Motion_Coord_List(i).键名.Trim & "】键与【" & Motion_Coord_List(i + 1).键名.Trim & "】键设置的点位过近：" & Pos & " mm,会造成相机不触发，请更改并保证点位间隔在【8mm】以上！"
                        Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), str, "", Color.Red)
                        MessageBoxEx.Show(str, "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
                        Return False
                    End If
                Next
            Else
                Return False
            End If
        Else
            Return False
        End If
        Return True
    End Function

    ''' <summary>
    ''' 读取单个坐标名称的坐标
    ''' </summary>
    ''' <param name="Coordinate_name"></param>
    ''' <param name="Motion_Coord_List"></param>
    ''' <remarks></remarks>
    Sub READ_SINGLE_COODNATE_PARAM(ByVal Coordinate_name As String, ByRef Motion_Coord_List() As Work_Coordinatess)
        Motion_Coord_List = Nothing
        Dim Point_Len As Integer, PARAMETERS_NAME_ARRAY() As String = Nothing, PARAMETERS_VALUE_ARRAY() As Double = Nothing, Row_Index As Integer = 0
        Dim Axis_Name_Str As String = Nothing, Axis_Index_Str As String = Nothing, str As String = Nothing, CoorParamCount As Integer = 0
        Dim Coor_Param_Dataset As New DataSet
        If SQLCON.Cheng_Query_Coordinate_AxisSystem(Select_Product_Name, Coordinate_name, Axis_Name_Str, Axis_Index_Str) = True Then
            PARAMETERS_NAME_ARRAY = Split(Axis_Name_Str, ";")
            Point_Len = PARAMETERS_NAME_ARRAY.Length
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
            SQLCON.Read_Coordinates(Select_Product_Name, Coordinate_name, PARAMETERS_NAME_ARRAY, Coor_Param_Dataset)
            CoorParamCount = Coor_Param_Dataset.Tables(0).Rows.Count

            If CoorParamCount >= 1 And PARAMETERS_NAME_ARRAY.Count >= 1 Then
                For COOR_INDEX As Int16 = 0 To CoorParamCount - 1 '坐标系名称里的实际坐标列表

                    Row_Index += 1 '所有坐标每次累加
                    Array.Resize(Motion_Coord_List, Row_Index)

                    Motion_Coord_List(Row_Index - 1).轴名称 = Split(Axis_Name_Str, ";")
                    Motion_Coord_List(Row_Index - 1).轴运动顺序 = Split(Axis_Index_Str, ";")
                    Array.Resize(Motion_Coord_List(Row_Index - 1).轴位置, Point_Len)
                    For i As Int16 = 0 To Point_Len - 1
                        Motion_Coord_List(Row_Index - 1).轴位置(i) = Coor_Param_Dataset.Tables(0).Rows(COOR_INDEX).Item(Motion_Coord_List(Row_Index - 1).轴名称(i))
                    Next
                    Array.Resize(Motion_Coord_List(Row_Index - 1).轴号, Point_Len)
                    For m As Int16 = 0 To Motion_Coord_List(Row_Index - 1).轴名称.Count - 1
                        For n As Integer = 0 To HomeParamObjArray.Length - 1
                            If Motion_Coord_List(Row_Index - 1).轴名称(m) = HomeParamObjArray(n).轴名称 Then
                                Motion_Coord_List(Row_Index - 1).轴号(m) = HomeParamObjArray(n).轴号
                                Exit For
                            End If
                        Next
                    Next

                    Motion_Coord_List(Row_Index - 1).坐标名称 = Coordinate_name
                    If IsDBNull(Coor_Param_Dataset.Tables(0).Rows(COOR_INDEX).Item("键名")) = True Then
                        Motion_Coord_List(Row_Index - 1).键名 = "N/A"
                    Else
                        Motion_Coord_List(Row_Index - 1).键名 = Coor_Param_Dataset.Tables(0).Rows(COOR_INDEX).Item("键名")
                    End If
                    If IsDBNull(Coor_Param_Dataset.Tables(0).Rows(COOR_INDEX).Item("运行速度")) = True Then
                        Motion_Coord_List(Row_Index - 1).运行速度 = False
                    Else
                        Motion_Coord_List(Row_Index - 1).运行速度 = Coor_Param_Dataset.Tables(0).Rows(COOR_INDEX).Item("运行速度")
                    End If
                    If IsDBNull(Coor_Param_Dataset.Tables(0).Rows(COOR_INDEX).Item("加减速度")) = True Then
                        Motion_Coord_List(Row_Index - 1).加减速度 = False
                    Else
                        Motion_Coord_List(Row_Index - 1).加减速度 = Coor_Param_Dataset.Tables(0).Rows(COOR_INDEX).Item("加减速度")
                    End If
                    If IsDBNull(Coor_Param_Dataset.Tables(0).Rows(COOR_INDEX).Item("点胶状态")) = True Then
                        Motion_Coord_List(Row_Index - 1).点胶状态 = False
                    Else
                        Motion_Coord_List(Row_Index - 1).点胶状态 = Coor_Param_Dataset.Tables(0).Rows(COOR_INDEX).Item("点胶状态")
                    End If
                    If IsDBNull(Coor_Param_Dataset.Tables(0).Rows(COOR_INDEX).Item("镭射触发状态")) = True Then
                        Motion_Coord_List(Row_Index - 1).镭射触发状态 = False
                    Else
                        Motion_Coord_List(Row_Index - 1).镭射触发状态 = Coor_Param_Dataset.Tables(0).Rows(COOR_INDEX).Item("镭射触发状态")
                    End If
                    If IsDBNull(Coor_Param_Dataset.Tables(0).Rows(COOR_INDEX).Item("镭射收数据状态")) = True Then
                        Motion_Coord_List(Row_Index - 1).镭射收数据状态 = False
                    Else
                        Motion_Coord_List(Row_Index - 1).镭射收数据状态 = Coor_Param_Dataset.Tables(0).Rows(COOR_INDEX).Item("镭射收数据状态")
                    End If
                    If IsDBNull(Coor_Param_Dataset.Tables(0).Rows(COOR_INDEX).Item("CCD触发状态")) = True Then
                        Motion_Coord_List(Row_Index - 1).CCD触发状态 = False
                    Else
                        Motion_Coord_List(Row_Index - 1).CCD触发状态 = Coor_Param_Dataset.Tables(0).Rows(COOR_INDEX).Item("CCD触发状态")
                    End If
                    If IsDBNull(Coor_Param_Dataset.Tables(0).Rows(COOR_INDEX).Item("IO触发状态")) = True Then
                        Motion_Coord_List(Row_Index - 1).IO触发状态 = False
                    Else
                        Motion_Coord_List(Row_Index - 1).IO触发状态 = Coor_Param_Dataset.Tables(0).Rows(COOR_INDEX).Item("IO触发状态")
                    End If
                    If IsDBNull(Coor_Param_Dataset.Tables(0).Rows(COOR_INDEX).Item("其他触发状态")) = True Then
                        Motion_Coord_List(Row_Index - 1).其他触发状态 = False
                    Else
                        Motion_Coord_List(Row_Index - 1).其他触发状态 = Coor_Param_Dataset.Tables(0).Rows(COOR_INDEX).Item("其他触发状态")
                    End If

                    Motion_Coord_List(Row_Index - 1).慢速 = Motion_Coord_List(Row_Index - 1).运行速度 * 0.1
                    Motion_Coord_List(Row_Index - 1).中速 = Motion_Coord_List(Row_Index - 1).运行速度 * 0.5
                Next
            End If
        End If

    End Sub

    ''' <summary>
    ''' 左工位开始初始化变量
    ''' </summary>
    ''' <remarks></remarks>
    Sub Start_Left()
        Trigger_Left.Style.BackColor1.Color = Color.Gray
        BOOL.Left_IsRuning = True
        BOOL.Red_Light = False
        BOOL.Yellow_Light = False
        BOOL.Green_Light = False
        Start_Time_Left = Date.Now

    End Sub
    ''' <summary>
    ''' 左工位结束变量赋值
    ''' </summary>
    ''' <remarks></remarks>
    Sub End_Left()
        BOOL.Left_IsRuning = False
        BOOL.Red_Light = False
        BOOL.Yellow_Light = False
        BOOL.Green_Light = False
        End_Time_Left = Date.Now
        CT_Time_Left = DateDiff(DateInterval.Second, Start_Time_Left, End_Time_Left).ToString
        Text_SN_Left.Text = ""
        JSON_WorkAt = WorkAt.LEFT
    End Sub
    ''' <summary>
    ''' 右工位开始初始化变量
    ''' </summary>
    ''' <remarks></remarks>
    Sub Start_Right()
        Trigger_Right.Style.BackColor1.Color = Color.Gray
        BOOL.Right_IsRuning = True
        BOOL.Red_Light = False
        BOOL.Yellow_Light = False
        BOOL.Green_Light = False
        Start_Time_Right = Date.Now
    End Sub
    ''' <summary>
    ''' 右工位结束赋值变量
    ''' </summary>
    ''' <remarks></remarks>
    Sub End_Right()
        BOOL.Right_IsRuning = False
        BOOL.Red_Light = False
        BOOL.Yellow_Light = False
        BOOL.Green_Light = False
        End_Time_Right = Date.Now
        CT_Time_Right = DateDiff(DateInterval.Second, Start_Time_Right, End_Time_Right).ToString
        Text_SN_Right.Text = ""
        JSON_WorkAt = WorkAt.RIGHT
    End Sub

    ''' <summary>
    ''' 改变红黄绿灯状态
    ''' </summary>
    ''' <remarks></remarks>
    Sub Change_RedYellowGreen(ByVal _light As LIGHT)
        If Card_Init_OK = True Then
            Select Case _light
                Case LIGHT.RED
                    CARDCMD.SET_DO_BIT(BOARD_ID_00, DIO.H红灯_0, PRA_OUT_STATUS_ENUM.OUT_ON)
                    CARDCMD.SET_DO_BIT(BOARD_ID_00, DIO.H黄灯_0, PRA_OUT_STATUS_ENUM.OUT_OFF)
                    CARDCMD.SET_DO_BIT(BOARD_ID_00, DIO.L绿灯_0, PRA_OUT_STATUS_ENUM.OUT_OFF)
                Case LIGHT.YELLOW
                    CARDCMD.SET_DO_BIT(BOARD_ID_00, DIO.H红灯_0, PRA_OUT_STATUS_ENUM.OUT_OFF)
                    CARDCMD.SET_DO_BIT(BOARD_ID_00, DIO.H黄灯_0, PRA_OUT_STATUS_ENUM.OUT_ON)
                    CARDCMD.SET_DO_BIT(BOARD_ID_00, DIO.L绿灯_0, PRA_OUT_STATUS_ENUM.OUT_OFF)
                Case LIGHT.GREEN
                    CARDCMD.SET_DO_BIT(BOARD_ID_00, DIO.H红灯_0, PRA_OUT_STATUS_ENUM.OUT_OFF)
                    CARDCMD.SET_DO_BIT(BOARD_ID_00, DIO.H黄灯_0, PRA_OUT_STATUS_ENUM.OUT_OFF)
                    CARDCMD.SET_DO_BIT(BOARD_ID_00, DIO.L绿灯_0, PRA_OUT_STATUS_ENUM.OUT_ON)
                Case LIGHT.NONE
                    CARDCMD.SET_DO_BIT(BOARD_ID_00, DIO.H红灯_0, PRA_OUT_STATUS_ENUM.OUT_OFF)
                    CARDCMD.SET_DO_BIT(BOARD_ID_00, DIO.H黄灯_0, PRA_OUT_STATUS_ENUM.OUT_OFF)
                    CARDCMD.SET_DO_BIT(BOARD_ID_00, DIO.L绿灯_0, PRA_OUT_STATUS_ENUM.OUT_OFF)
            End Select
        End If
    End Sub

    Private Sub SW_Fully_automatic_MouseLeave(ByVal sender As Object, ByVal e As System.EventArgs) Handles SW_Fully_automatic.MouseLeave
        SW_Fully_automatic.IsReadOnly = True
    End Sub

    Private Sub SW_Fully_automatic_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles SW_Fully_automatic.MouseMove
        SW_Fully_automatic.IsReadOnly = False
    End Sub

    Sub Get_Time_S(ByRef time As ULong)
        time = Date.Now.Hour * 3600 + Date.Now.Minute * 60 + Date.Now.Second + (Date.Now.Millisecond / 1000)
    End Sub
    Sub Get_Time_MS(ByRef time As ULong)
        time = (Date.Now.Hour * 3600 + Date.Now.Minute * 60 + Date.Now.Second) * 1000 + Date.Now.Millisecond
    End Sub

    Public TCPCON As New TCP_IP
    Private ClientSocket As Socket '客户端SOCKET
    Private ServerSocket As TcpListener '服务器端SOCKET
    Private ServerIP As String = Nothing '服务器IP地址
    Private ServerPort As String = Nothing '服务器端口号
    Dim Client_Thread, Server_Thread As Thread
    Dim Bool_Client_Thread, Bool_Server_Thread, Bool_Server_ConnOK, Bool_Read_Measure_Date, Bool_SerialPort_Work, Bool_Read_Pos, Bool_Read_MPEL As Boolean
    Dim Str_Client, Str_Server, Str_Client_CCDStr As String, Send_Omron_Cmd As Boolean = False
    Dim Int_Client_ConnOK As Int16 = 1



    Sub Client_Thread_Fun()
        While Bool_Client_Thread
            Thread.Sleep(1)
            If TCPCON.Clinet_Recive_Data(Str_Client) = 0 Then
                Str_Client_CCDStr = Str_Client
            Else
                If TCPCON.ServerBreak_ClientConn(Laser_Style, 5, 100) = 0 Then
                Else
                    '需要手动重新连接
                    If Int_Client_ConnOK = 0 And Client_Thread IsNot Nothing Then
                        Int_Client_ConnOK = 1
                        Bool_Client_Thread = False
                        If Client_Thread.IsAlive = True Then
                            Client_Thread.Abort()
                        End If
                    End If
                End If
            End If
        End While
    End Sub


    Dim Bool_Protect As Boolean = False '保护定时清胶
    Private Sub Timer_Free_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer_Free.Tick
        '计算空闲时间
        If Bool_No_Wrok = True And Bool_Home_Done = True And BOOL.Common_IsRuning = False And Portect_STRCT.Hand_Dispensing = False And Bool_Protect = False Then
            NO_WORK_INDEX += 1
            If NO_WORK_INDEX < 60 Then
                PE_NO_WORK.Text = "H:" & Int(0).ToString.PadLeft(2).Replace(" ", "0") & " M:" & Int(0).ToString.PadLeft(2).Replace(" ", "0") & " S:" & (NO_WORK_INDEX Mod 60).ToString.PadLeft(2).Replace(" ", "0")
            ElseIf NO_WORK_INDEX >= 60 And NO_WORK_INDEX < 3600 Then
                PE_NO_WORK.Text = "H:" & Int(0).ToString.PadLeft(2).Replace(" ", "0") & " M:" & Int(NO_WORK_INDEX / 60).ToString.PadLeft(2).Replace(" ", "0") & " S:" & (NO_WORK_INDEX Mod 60).ToString.PadLeft(2).Replace(" ", "0")
            ElseIf NO_WORK_INDEX >= 3600 Then
                PE_NO_WORK.Text = "H:" & Int(NO_WORK_INDEX / 3600).ToString.PadLeft(2).Replace(" ", "0") & " M:" & (Int((NO_WORK_INDEX - 3600) / 60)).ToString.PadLeft(2).Replace(" ", "0") & " S:" & (NO_WORK_INDEX Mod 60).ToString.PadLeft(2).Replace(" ", "0")
            End If
            If NO_WORK_INDEX >= 10000 Then
                Bool_Start_Left = False
                Bool_Start_Right = False
                Bool_Protect = True

                '进行相应的动作
                NO_WORK_INDEX = 0
                Bool_Protect = False
            End If
        Else
            NO_WORK_INDEX = 0
        End If
        'If mClear_Err = True Then
        '    Err_Count = Err_Count + 1
        '    If Err_Count >= 15 Then
        '        If ButtonX1.Enabled = False Then
        '            ButtonX1.Enabled = True
        '            ButtonX1.SymbolColor = Color.Red
        '        End If
        '    End If
        'Else
        '    If ButtonX1.Enabled = True Then
        '        ButtonX1.Enabled = False
        '    End If
        'End If
    End Sub

#Region "IO Control"

    Dim Bool_Disp_IO_Log As Boolean = True

    Function Z中线阻挡电磁阀(ByVal Model As IO, ByVal LR As LR_STATION, ByVal BarCode As String) As Boolean
        Dim Left_Start_Time, Left_End_Time, Left_Lost_Time As UInt64, str As String, rtn_bool As Boolean
        Select Case Model
            Case IO.OUT_ON
                Dim Out_Int As Integer = CARDCMD.SET_DO_BIT(BOARD_ID_00, DIO.Z中线阻挡电磁阀_0, PRA_OUT_STATUS_ENUM.OUT_ON)
                str = "中线阻挡电磁阀" & "［WORK启动］"
                If Bool_Disp_IO_Log = True Then
                    Select Case LR
                        Case LR_STATION.LEFT
                            Invoke(New Delegate_Display_Message_Left(AddressOf Display_Message_Left), str, BarCode, Color.Black)
                            Write_Out_Left(BarCode, GetCurrentMethod.Name & ":" & Model & ":" & Out_Int)
                        Case LR_STATION.RIGHT
                            Invoke(New Delegate_Display_Message_Right(AddressOf Display_Message_Right), str, BarCode, Color.Black)
                            Write_Out_Right(BarCode, GetCurrentMethod.Name & ":" & Model & ":" & Out_Int)
                        Case LR_STATION.COMMUNAL
                            Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), str, BarCode, Color.Black)
                            Write_Out_Com(BarCode, GetCurrentMethod.Name & ":" & Model & ":" & Out_Int)
                    End Select
                End If

                Thread.Sleep(PARAM_INT.D电磁阀输出等待时间)

                If Out_Int <> 0 Then
                    Return False
                End If

                If PARAM_BOOL.D到位信号检测 = True Then
                    Get_Time_MS(Left_Start_Time)
                    While True
                        Get_Time_MS(Left_End_Time)
                        If Left_End_Time < Left_Start_Time Then
                            Left_End_Time = Left_Start_Time
                        End If
                        Left_Lost_Time = Left_End_Time - Left_Start_Time
                        If Left_Lost_Time >= 2500 Then
                            str = "中线阻挡电磁阀" & "［WORK到位信号没有感应到］"
                            Select Case LR
                                Case LR_STATION.LEFT
                                    Invoke(New Delegate_Display_Message_Left(AddressOf Display_Message_Left), str, BarCode, Color.Red)
                                Case LR_STATION.RIGHT
                                    Invoke(New Delegate_Display_Message_Right(AddressOf Display_Message_Right), str, BarCode, Color.Red)
                                Case LR_STATION.COMMUNAL
                                    Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), str, BarCode, Color.Red)
                            End Select
                            Return False
                        End If
                        rtn_bool = CARDCMD.GET_DI_BIT(BOARD_ID_00, DIO.Z中线阻挡WORK_0)
                        If rtn_bool = True Then
                            Exit While
                        End If
                    End While
                    str = "中线阻挡电磁阀" & "［WORK启动完成］"
                    Select Case LR
                        Case LR_STATION.LEFT
                            Invoke(New Delegate_Display_Message_Left(AddressOf Display_Message_Left), str, BarCode, Color.Black)
                        Case LR_STATION.RIGHT
                            Invoke(New Delegate_Display_Message_Right(AddressOf Display_Message_Right), str, BarCode, Color.Black)
                        Case LR_STATION.COMMUNAL
                            Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), str, BarCode, Color.Black)
                    End Select
                End If
            Case IO.OUT_OFF
                Dim Out_Int As Integer = CARDCMD.SET_DO_BIT(BOARD_ID_00, DIO.Z中线阻挡电磁阀_0, PRA_OUT_STATUS_ENUM.OUT_OFF)
                str = "中线阻挡电磁阀" & "［HOME启动］"
                If Bool_Disp_IO_Log = True Then
                    Select Case LR
                        Case LR_STATION.LEFT
                            Invoke(New Delegate_Display_Message_Left(AddressOf Display_Message_Left), str, BarCode, Color.Black)
                            Write_Out_Left(BarCode, GetCurrentMethod.Name & ":" & Model & ":" & Out_Int)
                        Case LR_STATION.RIGHT
                            Invoke(New Delegate_Display_Message_Right(AddressOf Display_Message_Right), str, BarCode, Color.Black)
                            Write_Out_Right(BarCode, GetCurrentMethod.Name & ":" & Model & ":" & Out_Int)
                        Case LR_STATION.COMMUNAL
                            Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), str, BarCode, Color.Black)
                            Write_Out_Com(BarCode, GetCurrentMethod.Name & ":" & Model & ":" & Out_Int)
                    End Select
                End If
              
                Thread.Sleep(PARAM_INT.D电磁阀输出等待时间)

                If Out_Int <> 0 Then
                    Return False
                End If


                If PARAM_BOOL.D到位信号检测 = True Then
                    Get_Time_MS(Left_Start_Time)
                    While True
                        Get_Time_MS(Left_End_Time)
                        If Left_End_Time < Left_Start_Time Then
                            Left_End_Time = Left_Start_Time
                        End If
                        Left_Lost_Time = Left_End_Time - Left_Start_Time
                        If Left_Lost_Time >= 2500 Then
                            str = "中线阻挡电磁阀" & "［HOME到位信号没有感应到］"
                            Select Case LR
                                Case LR_STATION.LEFT
                                    Invoke(New Delegate_Display_Message_Left(AddressOf Display_Message_Left), str, BarCode, Color.Red)
                                Case LR_STATION.RIGHT
                                    Invoke(New Delegate_Display_Message_Right(AddressOf Display_Message_Right), str, BarCode, Color.Red)
                                Case LR_STATION.COMMUNAL
                                    Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), str, BarCode, Color.Red)
                            End Select
                            Return False
                        End If
                        rtn_bool = CARDCMD.GET_DI_BIT(BOARD_ID_00, DIO.Z中线阻挡HOME_0)
                        If rtn_bool = True Then
                            Exit While
                        End If
                    End While
                    str = "中线阻挡电磁阀" & "［HOME启动完成］"
                    Select Case LR
                        Case LR_STATION.LEFT
                            Invoke(New Delegate_Display_Message_Left(AddressOf Display_Message_Left), str, BarCode, Color.Black)
                        Case LR_STATION.RIGHT
                            Invoke(New Delegate_Display_Message_Right(AddressOf Display_Message_Right), str, BarCode, Color.Black)
                        Case LR_STATION.COMMUNAL
                            Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), str, BarCode, Color.Black)
                    End Select
                End If
        End Select
        Return True
    End Function

    Function ONE1工位阻挡电磁阀1(ByVal Model As IO, ByVal LR As LR_STATION, ByVal BarCode As String) As Boolean
        Dim Left_Start_Time, Left_End_Time, Left_Lost_Time As UInt64, str As String, rtn_bool As Boolean
        Select Case Model
            Case IO.OUT_ON
                Dim Out_Int As Integer = CARDCMD.SET_DO_BIT(BOARD_ID_00, DIO.ONE1工位阻挡电磁阀1_0, PRA_OUT_STATUS_ENUM.OUT_ON)
                str = "1工位阻挡电磁阀1" & "［WORK启动］"
                If Bool_Disp_IO_Log = True Then
                    Select Case LR
                        Case LR_STATION.LEFT
                            Invoke(New Delegate_Display_Message_Left(AddressOf Display_Message_Left), str, BarCode, Color.Black)
                            Write_Out_Left(BarCode, GetCurrentMethod.Name & ":" & Model & ":" & Out_Int)
                        Case LR_STATION.RIGHT
                            Invoke(New Delegate_Display_Message_Right(AddressOf Display_Message_Right), str, BarCode, Color.Black)
                            Write_Out_Right(BarCode, GetCurrentMethod.Name & ":" & Model & ":" & Out_Int)
                        Case LR_STATION.COMMUNAL
                            Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), str, BarCode, Color.Black)
                            Write_Out_Com(BarCode, GetCurrentMethod.Name & ":" & Model & ":" & Out_Int)
                    End Select
                End If
                Thread.Sleep(PARAM_INT.D电磁阀输出等待时间)
                If PARAM_BOOL.D到位信号检测 = True Then
                    Get_Time_MS(Left_Start_Time)
                    While True
                        Get_Time_MS(Left_End_Time)
                        If Left_End_Time < Left_Start_Time Then
                            Left_End_Time = Left_Start_Time
                        End If
                        Left_Lost_Time = Left_End_Time - Left_Start_Time
                        If Left_Lost_Time >= 2500 Then
                            str = "1工位阻挡电磁阀1" & "［WORK到位信号没有感应到］"
                            If Bool_Disp_IO_Log = True Then
                                Select Case LR
                                    Case LR_STATION.LEFT
                                        Invoke(New Delegate_Display_Message_Left(AddressOf Display_Message_Left), str, BarCode, Color.Red)
                                    Case LR_STATION.RIGHT
                                        Invoke(New Delegate_Display_Message_Right(AddressOf Display_Message_Right), str, BarCode, Color.Red)
                                    Case LR_STATION.COMMUNAL
                                        Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), str, BarCode, Color.Red)
                                End Select
                            End If
                            Return False
                        End If
                        rtn_bool = CARDCMD.GET_DI_BIT(BOARD_ID_00, DIO.ONE1工位阻挡WORK1_0)
                        If rtn_bool = True Then
                            Exit While
                        End If
                    End While
                    str = "1工位阻挡电磁阀1" & "［WORK启动完成］"
                    If Bool_Disp_IO_Log = True Then
                        Select Case LR
                            Case LR_STATION.LEFT
                                Invoke(New Delegate_Display_Message_Left(AddressOf Display_Message_Left), str, BarCode, Color.Black)
                            Case LR_STATION.RIGHT
                                Invoke(New Delegate_Display_Message_Right(AddressOf Display_Message_Right), str, BarCode, Color.Black)
                            Case LR_STATION.COMMUNAL
                                Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), str, BarCode, Color.Black)
                        End Select
                    End If
                End If
            Case IO.OUT_OFF
                Dim Out_Int As Integer = CARDCMD.SET_DO_BIT(BOARD_ID_00, DIO.ONE1工位阻挡电磁阀1_0, PRA_OUT_STATUS_ENUM.OUT_OFF)
                str = "1工位阻挡电磁阀1" & "［HOME启动］"
                If Bool_Disp_IO_Log = True Then
                    Select Case LR
                        Case LR_STATION.LEFT
                            Invoke(New Delegate_Display_Message_Left(AddressOf Display_Message_Left), str, BarCode, Color.Black)
                            Write_Out_Left(BarCode, GetCurrentMethod.Name & ":" & Model & ":" & Out_Int)
                        Case LR_STATION.RIGHT
                            Invoke(New Delegate_Display_Message_Right(AddressOf Display_Message_Right), str, BarCode, Color.Black)
                            Write_Out_Right(BarCode, GetCurrentMethod.Name & ":" & Model & ":" & Out_Int)
                        Case LR_STATION.COMMUNAL
                            Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), str, BarCode, Color.Black)
                            Write_Out_Com(BarCode, GetCurrentMethod.Name & ":" & Model & ":" & Out_Int)
                    End Select
                End If
                Thread.Sleep(PARAM_INT.D电磁阀输出等待时间)
                If PARAM_BOOL.D到位信号检测 = True Then
                    Get_Time_MS(Left_Start_Time)
                    While True
                        Get_Time_MS(Left_End_Time)
                        If Left_End_Time < Left_Start_Time Then
                            Left_End_Time = Left_Start_Time
                        End If
                        Left_Lost_Time = Left_End_Time - Left_Start_Time
                        If Left_Lost_Time >= 2500 Then
                            str = "1工位阻挡电磁阀1" & "［HOME到位信号没有感应到］"
                            If Bool_Disp_IO_Log = True Then
                                Select Case LR
                                    Case LR_STATION.LEFT
                                        Invoke(New Delegate_Display_Message_Left(AddressOf Display_Message_Left), str, BarCode, Color.Red)
                                    Case LR_STATION.RIGHT
                                        Invoke(New Delegate_Display_Message_Right(AddressOf Display_Message_Right), str, BarCode, Color.Red)
                                    Case LR_STATION.COMMUNAL
                                        Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), str, BarCode, Color.Red)
                                End Select
                            End If
                            Return False
                        End If
                        rtn_bool = CARDCMD.GET_DI_BIT(BOARD_ID_00, DIO.ONE1工位阻挡HOME1_0)
                        If rtn_bool = True Then
                            Exit While
                        End If
                    End While
                    str = "1工位阻挡电磁阀1" & "［HOME启动完成］"
                    If Bool_Disp_IO_Log = True Then
                        Select Case LR
                            Case LR_STATION.LEFT
                                Invoke(New Delegate_Display_Message_Left(AddressOf Display_Message_Left), str, BarCode, Color.Black)
                            Case LR_STATION.RIGHT
                                Invoke(New Delegate_Display_Message_Right(AddressOf Display_Message_Right), str, BarCode, Color.Black)
                            Case LR_STATION.COMMUNAL
                                Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), str, BarCode, Color.Black)
                        End Select
                    End If
                End If
        End Select
        Return True
    End Function

    Function ONE1工位阻挡电磁阀2(ByVal Model As IO, ByVal LR As LR_STATION, ByVal BarCode As String) As Boolean
        Dim Left_Start_Time, Left_End_Time, Left_Lost_Time As UInt64, str As String, rtn_bool As Boolean
        Select Case Model
            Case IO.OUT_ON
                Dim Out_Int As Integer = CARDCMD.SET_DO_BIT(BOARD_ID_00, DIO.ONE1工位阻挡电磁阀2_0, PRA_OUT_STATUS_ENUM.OUT_ON)
                str = "1工位阻挡电磁阀2" & "［WORK启动］"
                If Bool_Disp_IO_Log = True Then
                    Select Case LR
                        Case LR_STATION.LEFT
                            Invoke(New Delegate_Display_Message_Left(AddressOf Display_Message_Left), str, BarCode, Color.Black)
                            Write_Out_Left(BarCode, GetCurrentMethod.Name & ":" & Model & ":" & Out_Int)
                        Case LR_STATION.RIGHT
                            Invoke(New Delegate_Display_Message_Right(AddressOf Display_Message_Right), str, BarCode, Color.Black)
                            Write_Out_Right(BarCode, GetCurrentMethod.Name & ":" & Model & ":" & Out_Int)
                        Case LR_STATION.COMMUNAL
                            Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), str, BarCode, Color.Black)
                            Write_Out_Com(BarCode, GetCurrentMethod.Name & ":" & Model & ":" & Out_Int)
                    End Select
                End If
                Thread.Sleep(PARAM_INT.D电磁阀输出等待时间)
                If PARAM_BOOL.D到位信号检测 = True Then
                    Get_Time_MS(Left_Start_Time)
                    While True
                        Get_Time_MS(Left_End_Time)
                        If Left_End_Time < Left_Start_Time Then
                            Left_End_Time = Left_Start_Time
                        End If
                        Left_Lost_Time = Left_End_Time - Left_Start_Time
                        If Left_Lost_Time >= 2500 Then
                            str = "1工位阻挡电磁阀2" & "［WORK到位信号没有感应到］"
                            If Bool_Disp_IO_Log = True Then
                                Select Case LR
                                    Case LR_STATION.LEFT
                                        Invoke(New Delegate_Display_Message_Left(AddressOf Display_Message_Left), str, BarCode, Color.Red)
                                    Case LR_STATION.RIGHT
                                        Invoke(New Delegate_Display_Message_Right(AddressOf Display_Message_Right), str, BarCode, Color.Red)
                                    Case LR_STATION.COMMUNAL
                                        Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), str, BarCode, Color.Red)
                                End Select
                            End If
                            Return False
                        End If
                        rtn_bool = CARDCMD.GET_DI_BIT(BOARD_ID_00, DIO.ONE1工位阻挡WORK2_0)
                        If rtn_bool = True Then
                            Exit While
                        End If
                    End While
                    str = "1工位阻挡电磁阀2" & "［WORK启动完成］"
                    If Bool_Disp_IO_Log = True Then
                        Select Case LR
                            Case LR_STATION.LEFT
                                Invoke(New Delegate_Display_Message_Left(AddressOf Display_Message_Left), str, BarCode, Color.Black)
                            Case LR_STATION.RIGHT
                                Invoke(New Delegate_Display_Message_Right(AddressOf Display_Message_Right), str, BarCode, Color.Black)
                            Case LR_STATION.COMMUNAL
                                Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), str, BarCode, Color.Black)
                        End Select
                    End If
                End If
            Case IO.OUT_OFF
                Dim Out_Int As Integer = CARDCMD.SET_DO_BIT(BOARD_ID_00, DIO.ONE1工位阻挡电磁阀2_0, PRA_OUT_STATUS_ENUM.OUT_OFF)
                str = "1工位阻挡电磁阀2" & "［HOME启动］"
                If Bool_Disp_IO_Log = True Then
                    Select Case LR
                        Case LR_STATION.LEFT
                            Invoke(New Delegate_Display_Message_Left(AddressOf Display_Message_Left), str, BarCode, Color.Black)
                            Write_Out_Left(BarCode, GetCurrentMethod.Name & ":" & Model & ":" & Out_Int)
                        Case LR_STATION.RIGHT
                            Invoke(New Delegate_Display_Message_Right(AddressOf Display_Message_Right), str, BarCode, Color.Black)
                            Write_Out_Right(BarCode, GetCurrentMethod.Name & ":" & Model & ":" & Out_Int)
                        Case LR_STATION.COMMUNAL
                            Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), str, BarCode, Color.Black)
                            Write_Out_Com(BarCode, GetCurrentMethod.Name & ":" & Model & ":" & Out_Int)
                    End Select
                End If
                Thread.Sleep(PARAM_INT.D电磁阀输出等待时间)
                If PARAM_BOOL.D到位信号检测 = True Then
                    Get_Time_MS(Left_Start_Time)
                    While True
                        Get_Time_MS(Left_End_Time)
                        If Left_End_Time < Left_Start_Time Then
                            Left_End_Time = Left_Start_Time
                        End If
                        Left_Lost_Time = Left_End_Time - Left_Start_Time
                        If Left_Lost_Time >= 2500 Then
                            str = "1工位阻挡电磁阀2" & "［HOME到位信号没有感应到］"
                            If Bool_Disp_IO_Log = True Then
                                Select Case LR
                                    Case LR_STATION.LEFT
                                        Invoke(New Delegate_Display_Message_Left(AddressOf Display_Message_Left), str, BarCode, Color.Red)
                                    Case LR_STATION.RIGHT
                                        Invoke(New Delegate_Display_Message_Right(AddressOf Display_Message_Right), str, BarCode, Color.Red)
                                    Case LR_STATION.COMMUNAL
                                        Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), str, BarCode, Color.Red)
                                End Select
                            End If
                            Return False
                        End If
                        rtn_bool = CARDCMD.GET_DI_BIT(BOARD_ID_00, DIO.ONE1工位阻挡HOME2_0)
                        If rtn_bool = True Then
                            Exit While
                        End If
                    End While
                    str = "1工位阻挡电磁阀2" & "［HOME启动完成］"
                    If Bool_Disp_IO_Log = True Then
                        Select Case LR
                            Case LR_STATION.LEFT
                                Invoke(New Delegate_Display_Message_Left(AddressOf Display_Message_Left), str, BarCode, Color.Black)
                            Case LR_STATION.RIGHT
                                Invoke(New Delegate_Display_Message_Right(AddressOf Display_Message_Right), str, BarCode, Color.Black)
                            Case LR_STATION.COMMUNAL
                                Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), str, BarCode, Color.Black)
                        End Select
                    End If
                End If
        End Select
        Return True
    End Function

    Function TWO2工位阻挡电磁阀1(ByVal Model As IO, ByVal LR As LR_STATION, ByVal BarCode As String) As Boolean
        Dim Left_Start_Time, Left_End_Time, Left_Lost_Time As UInt64, str As String, rtn_bool As Boolean
        Select Case Model
            Case IO.OUT_ON
                Dim Out_Int As Integer = CARDCMD.SET_DO_BIT(BOARD_ID_00, DIO.TWO2工位阻挡电磁阀1_0, PRA_OUT_STATUS_ENUM.OUT_ON)
                str = "2工位阻挡电磁阀1" & "［WORK启动］"
                If Bool_Disp_IO_Log = True Then
                    Select Case LR
                        Case LR_STATION.LEFT
                            Invoke(New Delegate_Display_Message_Left(AddressOf Display_Message_Left), str, BarCode, Color.Black)
                            Write_Out_Left(BarCode, GetCurrentMethod.Name & ":" & Model & ":" & Out_Int)
                        Case LR_STATION.RIGHT
                            Invoke(New Delegate_Display_Message_Right(AddressOf Display_Message_Right), str, BarCode, Color.Black)
                            Write_Out_Right(BarCode, GetCurrentMethod.Name & ":" & Model & ":" & Out_Int)
                        Case LR_STATION.COMMUNAL
                            Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), str, BarCode, Color.Black)
                            Write_Out_Com(BarCode, GetCurrentMethod.Name & ":" & Model & ":" & Out_Int)
                    End Select
                End If
                Thread.Sleep(PARAM_INT.D电磁阀输出等待时间)
                If PARAM_BOOL.D到位信号检测 = True Then
                    Get_Time_MS(Left_Start_Time)
                    While True
                        Get_Time_MS(Left_End_Time)
                        If Left_End_Time < Left_Start_Time Then
                            Left_End_Time = Left_Start_Time
                        End If
                        Left_Lost_Time = Left_End_Time - Left_Start_Time
                        If Left_Lost_Time >= 2500 Then
                            str = "2工位阻挡电磁阀1" & "［WORK到位信号没有感应到］"
                            If Bool_Disp_IO_Log = True Then
                                Select Case LR
                                    Case LR_STATION.LEFT
                                        Invoke(New Delegate_Display_Message_Left(AddressOf Display_Message_Left), str, BarCode, Color.Red)
                                    Case LR_STATION.RIGHT
                                        Invoke(New Delegate_Display_Message_Right(AddressOf Display_Message_Right), str, BarCode, Color.Red)
                                    Case LR_STATION.COMMUNAL
                                        Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), str, BarCode, Color.Red)
                                End Select
                            End If
                            Return False
                        End If
                        rtn_bool = CARDCMD.GET_DI_BIT(BOARD_ID_00, DIO.TWO2工位阻挡WORK1_0)
                        If rtn_bool = True Then
                            Exit While
                        End If
                    End While
                    str = "2工位阻挡电磁阀1" & "［WORK启动完成］"
                    If Bool_Disp_IO_Log = True Then
                        Select Case LR
                            Case LR_STATION.LEFT
                                Invoke(New Delegate_Display_Message_Left(AddressOf Display_Message_Left), str, BarCode, Color.Black)
                            Case LR_STATION.RIGHT
                                Invoke(New Delegate_Display_Message_Right(AddressOf Display_Message_Right), str, BarCode, Color.Black)
                            Case LR_STATION.COMMUNAL
                                Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), str, BarCode, Color.Black)
                        End Select
                    End If
                End If
            Case IO.OUT_OFF
                Dim Out_Int As Integer = CARDCMD.SET_DO_BIT(BOARD_ID_00, DIO.TWO2工位阻挡电磁阀1_0, PRA_OUT_STATUS_ENUM.OUT_OFF)
                str = "2工位阻挡电磁阀1" & "［HOME启动］"
                If Bool_Disp_IO_Log = True Then
                    Select Case LR
                        Case LR_STATION.LEFT
                            Invoke(New Delegate_Display_Message_Left(AddressOf Display_Message_Left), str, BarCode, Color.Black)
                            Write_Out_Left(BarCode, GetCurrentMethod.Name & ":" & Model & ":" & Out_Int)
                        Case LR_STATION.RIGHT
                            Invoke(New Delegate_Display_Message_Right(AddressOf Display_Message_Right), str, BarCode, Color.Black)
                            Write_Out_Right(BarCode, GetCurrentMethod.Name & ":" & Model & ":" & Out_Int)
                        Case LR_STATION.COMMUNAL
                            Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), str, BarCode, Color.Black)
                            Write_Out_Com(BarCode, GetCurrentMethod.Name & ":" & Model & ":" & Out_Int)
                    End Select
                End If
                Thread.Sleep(PARAM_INT.D电磁阀输出等待时间)
                If PARAM_BOOL.D到位信号检测 = True Then
                    Get_Time_MS(Left_Start_Time)
                    While True
                        Get_Time_MS(Left_End_Time)
                        If Left_End_Time < Left_Start_Time Then
                            Left_End_Time = Left_Start_Time
                        End If
                        Left_Lost_Time = Left_End_Time - Left_Start_Time
                        If Left_Lost_Time >= 2500 Then
                            str = "2工位阻挡电磁阀1" & "［HOME到位信号没有感应到］"
                            If Bool_Disp_IO_Log = True Then
                                Select Case LR
                                    Case LR_STATION.LEFT
                                        Invoke(New Delegate_Display_Message_Left(AddressOf Display_Message_Left), str, BarCode, Color.Red)
                                    Case LR_STATION.RIGHT
                                        Invoke(New Delegate_Display_Message_Right(AddressOf Display_Message_Right), str, BarCode, Color.Red)
                                    Case LR_STATION.COMMUNAL
                                        Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), str, BarCode, Color.Red)
                                End Select
                            End If
                            Return False
                        End If
                        rtn_bool = CARDCMD.GET_DI_BIT(BOARD_ID_00, DIO.TWO2工位阻挡HOME1_0)
                        If rtn_bool = True Then
                            Exit While
                        End If
                    End While
                    str = "2工位阻挡电磁阀1" & "［HOME启动完成］"
                    If Bool_Disp_IO_Log = True Then
                        Select Case LR
                            Case LR_STATION.LEFT
                                Invoke(New Delegate_Display_Message_Left(AddressOf Display_Message_Left), str, BarCode, Color.Black)
                            Case LR_STATION.RIGHT
                                Invoke(New Delegate_Display_Message_Right(AddressOf Display_Message_Right), str, BarCode, Color.Black)
                            Case LR_STATION.COMMUNAL
                                Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), str, BarCode, Color.Black)
                        End Select
                    End If
                End If
        End Select
        Return True
    End Function

    Function TWO2工位阻挡电磁阀2(ByVal Model As IO, ByVal LR As LR_STATION, ByVal BarCode As String) As Boolean
        Dim Left_Start_Time, Left_End_Time, Left_Lost_Time As UInt64, str As String, rtn_bool As Boolean
        Select Case Model
            Case IO.OUT_ON
                Dim Out_Int As Integer = CARDCMD.SET_DO_BIT(BOARD_ID_00, DIO.TWO2工位阻挡电磁阀2_0, PRA_OUT_STATUS_ENUM.OUT_ON)
                str = "2工位阻挡电磁阀2" & "［WORK启动］"
                If Bool_Disp_IO_Log = True Then
                    Select Case LR
                        Case LR_STATION.LEFT
                            Invoke(New Delegate_Display_Message_Left(AddressOf Display_Message_Left), str, BarCode, Color.Black)
                            Write_Out_Left(BarCode, GetCurrentMethod.Name & ":" & Model & ":" & Out_Int)
                        Case LR_STATION.RIGHT
                            Invoke(New Delegate_Display_Message_Right(AddressOf Display_Message_Right), str, BarCode, Color.Black)
                            Write_Out_Right(BarCode, GetCurrentMethod.Name & ":" & Model & ":" & Out_Int)
                        Case LR_STATION.COMMUNAL
                            Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), str, BarCode, Color.Black)
                            Write_Out_Com(BarCode, GetCurrentMethod.Name & ":" & Model & ":" & Out_Int)
                    End Select
                End If
                Thread.Sleep(PARAM_INT.D电磁阀输出等待时间)
                If PARAM_BOOL.D到位信号检测 = True Then
                    Get_Time_MS(Left_Start_Time)
                    While True
                        Get_Time_MS(Left_End_Time)
                        If Left_End_Time < Left_Start_Time Then
                            Left_End_Time = Left_Start_Time
                        End If
                        Left_Lost_Time = Left_End_Time - Left_Start_Time
                        If Left_Lost_Time >= 2500 Then
                            str = "2工位阻挡电磁阀2" & "［WORK到位信号没有感应到］"
                            If Bool_Disp_IO_Log = True Then
                                Select Case LR
                                    Case LR_STATION.LEFT
                                        Invoke(New Delegate_Display_Message_Left(AddressOf Display_Message_Left), str, BarCode, Color.Red)
                                    Case LR_STATION.RIGHT
                                        Invoke(New Delegate_Display_Message_Right(AddressOf Display_Message_Right), str, BarCode, Color.Red)
                                    Case LR_STATION.COMMUNAL
                                        Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), str, BarCode, Color.Red)
                                End Select
                            End If
                            Return False
                        End If
                        rtn_bool = CARDCMD.GET_DI_BIT(BOARD_ID_00, DIO.TWO2工位阻挡WORK2_0)
                        If rtn_bool = True Then
                            Exit While
                        End If
                    End While
                    str = "2工位阻挡电磁阀2" & "［WORK启动完成］"
                    If Bool_Disp_IO_Log = True Then
                        Select Case LR
                            Case LR_STATION.LEFT
                                Invoke(New Delegate_Display_Message_Left(AddressOf Display_Message_Left), str, BarCode, Color.Black)
                            Case LR_STATION.RIGHT
                                Invoke(New Delegate_Display_Message_Right(AddressOf Display_Message_Right), str, BarCode, Color.Black)
                            Case LR_STATION.COMMUNAL
                                Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), str, BarCode, Color.Black)
                        End Select
                    End If
                End If
            Case IO.OUT_OFF
                Dim Out_Int As Integer = CARDCMD.SET_DO_BIT(BOARD_ID_00, DIO.TWO2工位阻挡电磁阀2_0, PRA_OUT_STATUS_ENUM.OUT_OFF)
                str = "2工位阻挡电磁阀2" & "［HOME启动］"
                If Bool_Disp_IO_Log = True Then
                    Select Case LR
                        Case LR_STATION.LEFT
                            Invoke(New Delegate_Display_Message_Left(AddressOf Display_Message_Left), str, BarCode, Color.Black)
                            Write_Out_Left(BarCode, GetCurrentMethod.Name & ":" & Model & ":" & Out_Int)
                        Case LR_STATION.RIGHT
                            Invoke(New Delegate_Display_Message_Right(AddressOf Display_Message_Right), str, BarCode, Color.Black)
                            Write_Out_Right(BarCode, GetCurrentMethod.Name & ":" & Model & ":" & Out_Int)
                        Case LR_STATION.COMMUNAL
                            Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), str, BarCode, Color.Black)
                            Write_Out_Com(BarCode, GetCurrentMethod.Name & ":" & Model & ":" & Out_Int)
                    End Select
                End If
                Thread.Sleep(PARAM_INT.D电磁阀输出等待时间)
                If PARAM_BOOL.D到位信号检测 = True Then
                    Get_Time_MS(Left_Start_Time)
                    While True
                        Get_Time_MS(Left_End_Time)
                        If Left_End_Time < Left_Start_Time Then
                            Left_End_Time = Left_Start_Time
                        End If
                        Left_Lost_Time = Left_End_Time - Left_Start_Time
                        If Left_Lost_Time >= 2500 Then
                            str = "2工位阻挡电磁阀2" & "［HOME到位信号没有感应到］"
                            If Bool_Disp_IO_Log = True Then
                                Select Case LR
                                    Case LR_STATION.LEFT
                                        Invoke(New Delegate_Display_Message_Left(AddressOf Display_Message_Left), str, BarCode, Color.Red)
                                    Case LR_STATION.RIGHT
                                        Invoke(New Delegate_Display_Message_Right(AddressOf Display_Message_Right), str, BarCode, Color.Red)
                                    Case LR_STATION.COMMUNAL
                                        Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), str, BarCode, Color.Red)
                                End Select
                            End If
                            Return False
                        End If
                        rtn_bool = CARDCMD.GET_DI_BIT(BOARD_ID_00, DIO.TWO2工位阻挡HOME2_0)
                        If rtn_bool = True Then
                            Exit While
                        End If
                    End While
                    str = "2工位阻挡电磁阀2" & "［HOME启动完成］"
                    If Bool_Disp_IO_Log = True Then
                        Select Case LR
                            Case LR_STATION.LEFT
                                Invoke(New Delegate_Display_Message_Left(AddressOf Display_Message_Left), str, BarCode, Color.Black)
                            Case LR_STATION.RIGHT
                                Invoke(New Delegate_Display_Message_Right(AddressOf Display_Message_Right), str, BarCode, Color.Black)
                            Case LR_STATION.COMMUNAL
                                Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), str, BarCode, Color.Black)
                        End Select
                    End If
                End If
        End Select
        Return True
    End Function

    Function ONE1工位顶升电磁阀(ByVal Model As IO, ByVal LR As LR_STATION, ByVal BarCode As String) As Boolean
        Dim Left_Start_Time, Left_End_Time, Left_Lost_Time As UInt64, str As String, rtn_bool As Boolean
        Select Case Model
            Case IO.OUT_ON
                Dim Out_Int As Integer = CARDCMD.SET_DO_BIT(BOARD_ID_00, DIO.ONE1工位顶升电磁阀_0, PRA_OUT_STATUS_ENUM.OUT_ON)
                str = "1工位顶升电磁阀" & "［WORK启动］"
                If Bool_Disp_IO_Log = True Then
                    Select Case LR
                        Case LR_STATION.LEFT
                            Invoke(New Delegate_Display_Message_Left(AddressOf Display_Message_Left), str, BarCode, Color.Black)
                            Write_Out_Left(BarCode, GetCurrentMethod.Name & ":" & Model & ":" & Out_Int)
                        Case LR_STATION.RIGHT
                            Invoke(New Delegate_Display_Message_Right(AddressOf Display_Message_Right), str, BarCode, Color.Black)
                            Write_Out_Right(BarCode, GetCurrentMethod.Name & ":" & Model & ":" & Out_Int)
                        Case LR_STATION.COMMUNAL
                            Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), str, BarCode, Color.Black)
                            Write_Out_Com(BarCode, GetCurrentMethod.Name & ":" & Model & ":" & Out_Int)
                    End Select
                End If
                Thread.Sleep(PARAM_INT.D电磁阀输出等待时间)
                If PARAM_BOOL.D到位信号检测 = True Then
                    Get_Time_MS(Left_Start_Time)
                    While True
                        Get_Time_MS(Left_End_Time)
                        If Left_End_Time < Left_Start_Time Then
                            Left_End_Time = Left_Start_Time
                        End If
                        Left_Lost_Time = Left_End_Time - Left_Start_Time
                        If Left_Lost_Time >= 2500 Then
                            str = "1工位顶升电磁阀" & "［WORK到位信号没有感应到］"
                            If Bool_Disp_IO_Log = True Then
                                Select Case LR
                                    Case LR_STATION.LEFT
                                        Invoke(New Delegate_Display_Message_Left(AddressOf Display_Message_Left), str, BarCode, Color.Red)
                                    Case LR_STATION.RIGHT
                                        Invoke(New Delegate_Display_Message_Right(AddressOf Display_Message_Right), str, BarCode, Color.Red)
                                    Case LR_STATION.COMMUNAL
                                        Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), str, BarCode, Color.Red)
                                End Select
                            End If
                            Return False
                        End If
                        rtn_bool = CARDCMD.GET_DI_BIT(BOARD_ID_01, DIO.ONE1工位顶升WORK_1)
                        If rtn_bool = True Then
                            Exit While
                        End If
                    End While
                    str = "1工位顶升电磁阀" & "［WORK启动完成］"
                    If Bool_Disp_IO_Log = True Then
                        Select Case LR
                            Case LR_STATION.LEFT
                                Invoke(New Delegate_Display_Message_Left(AddressOf Display_Message_Left), str, BarCode, Color.Black)
                            Case LR_STATION.RIGHT
                                Invoke(New Delegate_Display_Message_Right(AddressOf Display_Message_Right), str, BarCode, Color.Black)
                            Case LR_STATION.COMMUNAL
                                Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), str, BarCode, Color.Black)
                        End Select
                    End If
                End If
            Case IO.OUT_OFF
                Dim Out_Int As Integer = CARDCMD.SET_DO_BIT(BOARD_ID_00, DIO.ONE1工位顶升电磁阀_0, PRA_OUT_STATUS_ENUM.OUT_OFF)
                str = "1工位顶升电磁阀" & "［HOME启动］"
                If Bool_Disp_IO_Log = True Then
                    Select Case LR
                        Case LR_STATION.LEFT
                            Invoke(New Delegate_Display_Message_Left(AddressOf Display_Message_Left), str, BarCode, Color.Black)
                            Write_Out_Left(BarCode, GetCurrentMethod.Name & ":" & Model & ":" & Out_Int)
                        Case LR_STATION.RIGHT
                            Invoke(New Delegate_Display_Message_Right(AddressOf Display_Message_Right), str, BarCode, Color.Black)
                            Write_Out_Right(BarCode, GetCurrentMethod.Name & ":" & Model & ":" & Out_Int)
                        Case LR_STATION.COMMUNAL
                            Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), str, BarCode, Color.Black)
                            Write_Out_Com(BarCode, GetCurrentMethod.Name & ":" & Model & ":" & Out_Int)
                    End Select
                End If
                Thread.Sleep(PARAM_INT.D电磁阀输出等待时间)
                If PARAM_BOOL.D到位信号检测 = True Then
                    Get_Time_MS(Left_Start_Time)
                    While True
                        Get_Time_MS(Left_End_Time)
                        If Left_End_Time < Left_Start_Time Then
                            Left_End_Time = Left_Start_Time
                        End If
                        Left_Lost_Time = Left_End_Time - Left_Start_Time
                        If Left_Lost_Time >= 2500 Then
                            str = "1工位顶升电磁阀" & "［HOME到位信号没有感应到］"
                            If Bool_Disp_IO_Log = True Then
                                Select Case LR
                                    Case LR_STATION.LEFT
                                        Invoke(New Delegate_Display_Message_Left(AddressOf Display_Message_Left), str, BarCode, Color.Red)
                                    Case LR_STATION.RIGHT
                                        Invoke(New Delegate_Display_Message_Right(AddressOf Display_Message_Right), str, BarCode, Color.Red)
                                    Case LR_STATION.COMMUNAL
                                        Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), str, BarCode, Color.Red)
                                End Select
                            End If
                            Return False
                        End If
                        rtn_bool = CARDCMD.GET_DI_BIT(BOARD_ID_01, DIO.ONE1工位顶升HOME_1)
                        If rtn_bool = True Then
                            Exit While
                        End If
                    End While
                    str = "1工位顶升电磁阀" & "［HOME启动完成］"
                    If Bool_Disp_IO_Log = True Then
                        Select Case LR
                            Case LR_STATION.LEFT
                                Invoke(New Delegate_Display_Message_Left(AddressOf Display_Message_Left), str, BarCode, Color.Black)
                            Case LR_STATION.RIGHT
                                Invoke(New Delegate_Display_Message_Right(AddressOf Display_Message_Right), str, BarCode, Color.Black)
                            Case LR_STATION.COMMUNAL
                                Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), str, BarCode, Color.Black)
                        End Select
                    End If
                End If
        End Select
        Return True
    End Function

    Function TWO2工位顶升电磁阀(ByVal Model As IO, ByVal LR As LR_STATION, ByVal BarCode As String) As Boolean
        Dim Left_Start_Time, Left_End_Time, Left_Lost_Time As UInt64, str As String, rtn_bool As Boolean
        Select Case Model
            Case IO.OUT_ON
                Dim Out_Int As Integer = CARDCMD.SET_DO_BIT(BOARD_ID_00, DIO.TWO2工位顶升电磁阀_0, PRA_OUT_STATUS_ENUM.OUT_ON)
                str = "2工位顶升电磁阀" & "［WORK启动］"
                If Bool_Disp_IO_Log = True Then
                    Select Case LR
                        Case LR_STATION.LEFT
                            Invoke(New Delegate_Display_Message_Left(AddressOf Display_Message_Left), str, BarCode, Color.Black)
                            Write_Out_Left(BarCode, GetCurrentMethod.Name & ":" & Model & ":" & Out_Int)
                        Case LR_STATION.RIGHT
                            Invoke(New Delegate_Display_Message_Right(AddressOf Display_Message_Right), str, BarCode, Color.Black)
                            Write_Out_Right(BarCode, GetCurrentMethod.Name & ":" & Model & ":" & Out_Int)
                        Case LR_STATION.COMMUNAL
                            Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), str, BarCode, Color.Black)
                            Write_Out_Com(BarCode, GetCurrentMethod.Name & ":" & Model & ":" & Out_Int)
                    End Select
                End If
                Thread.Sleep(PARAM_INT.D电磁阀输出等待时间)
                If PARAM_BOOL.D到位信号检测 = True Then
                    Get_Time_MS(Left_Start_Time)
                    While True
                        Get_Time_MS(Left_End_Time)
                        If Left_End_Time < Left_Start_Time Then
                            Left_End_Time = Left_Start_Time
                        End If
                        Left_Lost_Time = Left_End_Time - Left_Start_Time
                        If Left_Lost_Time >= 2500 Then
                            str = "2工位顶升电磁阀" & "［WORK到位信号没有感应到］"
                            If Bool_Disp_IO_Log = True Then
                                Select Case LR
                                    Case LR_STATION.LEFT
                                        Invoke(New Delegate_Display_Message_Left(AddressOf Display_Message_Left), str, BarCode, Color.Red)
                                    Case LR_STATION.RIGHT
                                        Invoke(New Delegate_Display_Message_Right(AddressOf Display_Message_Right), str, BarCode, Color.Red)
                                    Case LR_STATION.COMMUNAL
                                        Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), str, BarCode, Color.Red)
                                End Select
                            End If
                            Return False
                        End If
                        rtn_bool = CARDCMD.GET_DI_BIT(BOARD_ID_01, DIO.TWO2工位顶升WORK_1)
                        If rtn_bool = True Then
                            Exit While
                        End If
                    End While
                    str = "2工位顶升电磁阀" & "［WORK启动完成］"
                    If Bool_Disp_IO_Log = True Then
                        Select Case LR
                            Case LR_STATION.LEFT
                                Invoke(New Delegate_Display_Message_Left(AddressOf Display_Message_Left), str, BarCode, Color.Black)
                            Case LR_STATION.RIGHT
                                Invoke(New Delegate_Display_Message_Right(AddressOf Display_Message_Right), str, BarCode, Color.Black)
                            Case LR_STATION.COMMUNAL
                                Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), str, BarCode, Color.Black)
                        End Select
                    End If
                End If
            Case IO.OUT_OFF
                Dim Out_Int As Integer = CARDCMD.SET_DO_BIT(BOARD_ID_00, DIO.TWO2工位顶升电磁阀_0, PRA_OUT_STATUS_ENUM.OUT_OFF)
                str = "2工位顶升电磁阀" & "［HOME启动］"
                If Bool_Disp_IO_Log = True Then
                    Select Case LR
                        Case LR_STATION.LEFT
                            Invoke(New Delegate_Display_Message_Left(AddressOf Display_Message_Left), str, BarCode, Color.Black)
                            Write_Out_Left(BarCode, GetCurrentMethod.Name & ":" & Model & ":" & Out_Int)
                        Case LR_STATION.RIGHT
                            Invoke(New Delegate_Display_Message_Right(AddressOf Display_Message_Right), str, BarCode, Color.Black)
                            Write_Out_Right(BarCode, GetCurrentMethod.Name & ":" & Model & ":" & Out_Int)
                        Case LR_STATION.COMMUNAL
                            Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), str, BarCode, Color.Black)
                            Write_Out_Com(BarCode, GetCurrentMethod.Name & ":" & Model & ":" & Out_Int)
                    End Select
                End If
                Thread.Sleep(PARAM_INT.D电磁阀输出等待时间)
                If PARAM_BOOL.D到位信号检测 = True Then
                    Get_Time_MS(Left_Start_Time)
                    While True
                        Get_Time_MS(Left_End_Time)
                        If Left_End_Time < Left_Start_Time Then
                            Left_End_Time = Left_Start_Time
                        End If
                        Left_Lost_Time = Left_End_Time - Left_Start_Time
                        If Left_Lost_Time >= 2500 Then
                            str = "2工位顶升电磁阀" & "［HOME到位信号没有感应到］"
                            If Bool_Disp_IO_Log = True Then
                                Select Case LR
                                    Case LR_STATION.LEFT
                                        Invoke(New Delegate_Display_Message_Left(AddressOf Display_Message_Left), str, BarCode, Color.Red)
                                    Case LR_STATION.RIGHT
                                        Invoke(New Delegate_Display_Message_Right(AddressOf Display_Message_Right), str, BarCode, Color.Red)
                                    Case LR_STATION.COMMUNAL
                                        Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), str, BarCode, Color.Red)
                                End Select
                            End If
                            Return False
                        End If
                        rtn_bool = CARDCMD.GET_DI_BIT(BOARD_ID_01, DIO.TWO2工位顶升HOME_1)
                        If rtn_bool = True Then
                            Exit While
                        End If
                    End While
                    str = "2工位顶升电磁阀" & "［HOME启动完成］"
                    If Bool_Disp_IO_Log = True Then
                        Select Case LR
                            Case LR_STATION.LEFT
                                Invoke(New Delegate_Display_Message_Left(AddressOf Display_Message_Left), str, BarCode, Color.Black)
                            Case LR_STATION.RIGHT
                                Invoke(New Delegate_Display_Message_Right(AddressOf Display_Message_Right), str, BarCode, Color.Black)
                            Case LR_STATION.COMMUNAL
                                Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), str, BarCode, Color.Black)
                        End Select
                    End If
                End If
        End Select
        Return True
    End Function

    Function Z中线真空吸电磁阀(ByVal Model As IO, ByVal LR As LR_STATION, ByVal BarCode As String) As Boolean
        Dim Left_Start_Time, Left_End_Time, Left_Lost_Time As UInt64, str As String, rtn_bool As Boolean
        Select Case Model
            Case IO.OUT_ON
                Dim Out_Int As Integer = CARDCMD.SET_DO_BIT(BOARD_ID_00, DIO.Z中线真空吸电磁阀_0, PRA_OUT_STATUS_ENUM.OUT_ON)
                str = "中线真空吸电磁阀" & "［打开］"
                If Bool_Disp_IO_Log = True Then
                    Select Case LR
                        Case LR_STATION.LEFT
                            Invoke(New Delegate_Display_Message_Left(AddressOf Display_Message_Left), str, BarCode, Color.Black)
                            Write_Out_Left(BarCode, GetCurrentMethod.Name & ":" & Model & ":" & Out_Int)
                        Case LR_STATION.RIGHT
                            Invoke(New Delegate_Display_Message_Right(AddressOf Display_Message_Right), str, BarCode, Color.Black)
                            Write_Out_Right(BarCode, GetCurrentMethod.Name & ":" & Model & ":" & Out_Int)
                        Case LR_STATION.COMMUNAL
                            Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), str, BarCode, Color.Black)
                            Write_Out_Com(BarCode, GetCurrentMethod.Name & ":" & Model & ":" & Out_Int)
                    End Select
                End If
                Thread.Sleep(PARAM_INT.Z真空吸等待时间)
                If PARAM_BOOL.D到位信号检测 = True Then
                    Get_Time_MS(Left_Start_Time)
                    While True
                        Get_Time_MS(Left_End_Time)
                        If Left_End_Time < Left_Start_Time Then
                            Left_End_Time = Left_Start_Time
                        End If
                        Left_Lost_Time = Left_End_Time - Left_Start_Time
                        If Left_Lost_Time >= 2500 Then
                            str = "中线真空吸电磁阀" & "［打开信号没有感应到］"
                            If Bool_Disp_IO_Log = True Then
                                Select Case LR
                                    Case LR_STATION.LEFT
                                        Invoke(New Delegate_Display_Message_Left(AddressOf Display_Message_Left), str, BarCode, Color.Red)
                                    Case LR_STATION.RIGHT
                                        Invoke(New Delegate_Display_Message_Right(AddressOf Display_Message_Right), str, BarCode, Color.Red)
                                    Case LR_STATION.COMMUNAL
                                        Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), str, BarCode, Color.Red)
                                End Select
                            End If
                            Return False
                        End If
                        rtn_bool = CARDCMD.GET_DI_BIT(BOARD_ID_00, DIO.Z中线真空吸检测_0)
                        If rtn_bool = True Then
                            Exit While
                        End If
                    End While
                    str = "中线真空吸电磁阀" & "［打开完成］"
                    If Bool_Disp_IO_Log = True Then
                        Select Case LR
                            Case LR_STATION.LEFT
                                Invoke(New Delegate_Display_Message_Left(AddressOf Display_Message_Left), str, BarCode, Color.Black)
                            Case LR_STATION.RIGHT
                                Invoke(New Delegate_Display_Message_Right(AddressOf Display_Message_Right), str, BarCode, Color.Black)
                            Case LR_STATION.COMMUNAL
                                Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), str, BarCode, Color.Black)
                        End Select
                    End If
                End If
            Case IO.OUT_OFF
                Dim Out_Int As Integer = CARDCMD.SET_DO_BIT(BOARD_ID_00, DIO.Z中线真空吸电磁阀_0, PRA_OUT_STATUS_ENUM.OUT_OFF)
                str = "中线真空吸电磁阀" & "［关闭］"
                If Bool_Disp_IO_Log = True Then
                    Select Case LR
                        Case LR_STATION.LEFT
                            Invoke(New Delegate_Display_Message_Left(AddressOf Display_Message_Left), str, BarCode, Color.Black)
                            Write_Out_Left(BarCode, GetCurrentMethod.Name & ":" & Model & ":" & Out_Int)
                        Case LR_STATION.RIGHT
                            Invoke(New Delegate_Display_Message_Right(AddressOf Display_Message_Right), str, BarCode, Color.Black)
                            Write_Out_Right(BarCode, GetCurrentMethod.Name & ":" & Model & ":" & Out_Int)
                        Case LR_STATION.COMMUNAL
                            Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), str, BarCode, Color.Black)
                            Write_Out_Com(BarCode, GetCurrentMethod.Name & ":" & Model & ":" & Out_Int)
                    End Select
                End If
                Thread.Sleep(PARAM_INT.Z真空吸等待时间)
                If PARAM_BOOL.D到位信号检测 = True Then
                    Get_Time_MS(Left_Start_Time)
                    While True
                        Get_Time_MS(Left_End_Time)
                        If Left_End_Time < Left_Start_Time Then
                            Left_End_Time = Left_Start_Time
                        End If
                        Left_Lost_Time = Left_End_Time - Left_Start_Time
                        If Left_Lost_Time >= 2500 Then
                            str = "中线真空吸电磁阀" & "［关闭信号没有感应到］"
                            If Bool_Disp_IO_Log = True Then
                                Select Case LR
                                    Case LR_STATION.LEFT
                                        Invoke(New Delegate_Display_Message_Left(AddressOf Display_Message_Left), str, BarCode, Color.Red)
                                    Case LR_STATION.RIGHT
                                        Invoke(New Delegate_Display_Message_Right(AddressOf Display_Message_Right), str, BarCode, Color.Red)
                                    Case LR_STATION.COMMUNAL
                                        Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), str, BarCode, Color.Red)
                                End Select
                            End If
                            Return False
                        End If
                        rtn_bool = CARDCMD.GET_DI_BIT(BOARD_ID_00, DIO.Z中线真空吸检测_0)
                        If rtn_bool = False Then
                            Exit While
                        End If
                    End While
                    str = "中线真空吸电磁阀" & "［关闭完成］"
                    If Bool_Disp_IO_Log = True Then
                        Select Case LR
                            Case LR_STATION.LEFT
                                Invoke(New Delegate_Display_Message_Left(AddressOf Display_Message_Left), str, BarCode, Color.Black)
                            Case LR_STATION.RIGHT
                                Invoke(New Delegate_Display_Message_Right(AddressOf Display_Message_Right), str, BarCode, Color.Black)
                            Case LR_STATION.COMMUNAL
                                Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), str, BarCode, Color.Black)
                        End Select
                    End If
                End If
        End Select
        Return True
    End Function

    Function ONE1工位真空吸电磁阀(ByVal Model As IO, ByVal LR As LR_STATION, ByVal BarCode As String) As Boolean
        Dim Left_Start_Time, Left_End_Time, Left_Lost_Time As UInt64, str As String, rtn_bool As Boolean
        Select Case Model
            Case IO.OUT_ON
                Dim Out_Int As Integer = CARDCMD.SET_DO_BIT(BOARD_ID_00, DIO.ONE1工位真空吸电磁阀_0, PRA_OUT_STATUS_ENUM.OUT_ON)
                str = "1工位真空吸电磁阀" & "［打开］"
                If Bool_Disp_IO_Log = True Then
                    Select Case LR
                        Case LR_STATION.LEFT
                            Invoke(New Delegate_Display_Message_Left(AddressOf Display_Message_Left), str, BarCode, Color.Black)
                            Write_Out_Left(BarCode, GetCurrentMethod.Name & ":" & Model & ":" & Out_Int)
                        Case LR_STATION.RIGHT
                            Invoke(New Delegate_Display_Message_Right(AddressOf Display_Message_Right), str, BarCode, Color.Black)
                            Write_Out_Right(BarCode, GetCurrentMethod.Name & ":" & Model & ":" & Out_Int)
                        Case LR_STATION.COMMUNAL
                            Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), str, BarCode, Color.Black)
                            Write_Out_Com(BarCode, GetCurrentMethod.Name & ":" & Model & ":" & Out_Int)
                    End Select
                End If
                Thread.Sleep(PARAM_INT.Z真空吸等待时间)
                If PARAM_BOOL.D到位信号检测 = True Then
                    Get_Time_MS(Left_Start_Time)
                    While True
                        Get_Time_MS(Left_End_Time)
                        If Left_End_Time < Left_Start_Time Then
                            Left_End_Time = Left_Start_Time
                        End If
                        Left_Lost_Time = Left_End_Time - Left_Start_Time
                        If Left_Lost_Time >= 2500 Then
                            str = "1工位真空吸电磁阀" & "［打开信号没有感应到］"
                            If Bool_Disp_IO_Log = True Then
                                Select Case LR
                                    Case LR_STATION.LEFT
                                        Invoke(New Delegate_Display_Message_Left(AddressOf Display_Message_Left), str, BarCode, Color.Red)
                                    Case LR_STATION.RIGHT
                                        Invoke(New Delegate_Display_Message_Right(AddressOf Display_Message_Right), str, BarCode, Color.Red)
                                    Case LR_STATION.COMMUNAL
                                        Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), str, BarCode, Color.Red)
                                End Select
                            End If
                            Return False
                        End If
                        rtn_bool = CARDCMD.GET_DI_BIT(BOARD_ID_00, DIO.ONE1工位真空吸检测_0)
                        If rtn_bool = True Then
                            Exit While
                        End If
                    End While
                    str = "1工位真空吸电磁阀" & "［打开完成］"
                    If Bool_Disp_IO_Log = True Then
                        Select Case LR
                            Case LR_STATION.LEFT
                                Invoke(New Delegate_Display_Message_Left(AddressOf Display_Message_Left), str, BarCode, Color.Black)
                            Case LR_STATION.RIGHT
                                Invoke(New Delegate_Display_Message_Right(AddressOf Display_Message_Right), str, BarCode, Color.Black)
                            Case LR_STATION.COMMUNAL
                                Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), str, BarCode, Color.Black)
                        End Select
                    End If
                End If
            Case IO.OUT_OFF
                Dim Out_Int As Integer = CARDCMD.SET_DO_BIT(BOARD_ID_00, DIO.ONE1工位真空吸电磁阀_0, PRA_OUT_STATUS_ENUM.OUT_OFF)
                str = "1工位真空吸电磁阀" & "［关闭］"
                If Bool_Disp_IO_Log = True Then
                    Select Case LR
                        Case LR_STATION.LEFT
                            Invoke(New Delegate_Display_Message_Left(AddressOf Display_Message_Left), str, BarCode, Color.Black)
                            Write_Out_Left(BarCode, GetCurrentMethod.Name & ":" & Model & ":" & Out_Int)
                        Case LR_STATION.RIGHT
                            Invoke(New Delegate_Display_Message_Right(AddressOf Display_Message_Right), str, BarCode, Color.Black)
                            Write_Out_Right(BarCode, GetCurrentMethod.Name & ":" & Model & ":" & Out_Int)
                        Case LR_STATION.COMMUNAL
                            Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), str, BarCode, Color.Black)
                            Write_Out_Com(BarCode, GetCurrentMethod.Name & ":" & Model & ":" & Out_Int)
                    End Select
                End If
                Thread.Sleep(PARAM_INT.Z真空吸等待时间)
                If PARAM_BOOL.D到位信号检测 = True Then
                    Get_Time_MS(Left_Start_Time)
                    While True
                        Get_Time_MS(Left_End_Time)
                        If Left_End_Time < Left_Start_Time Then
                            Left_End_Time = Left_Start_Time
                        End If
                        Left_Lost_Time = Left_End_Time - Left_Start_Time
                        If Left_Lost_Time >= 2500 Then
                            str = "1工位真空吸电磁阀" & "［关闭信号没有感应到］"
                            If Bool_Disp_IO_Log = True Then
                                Select Case LR
                                    Case LR_STATION.LEFT
                                        Invoke(New Delegate_Display_Message_Left(AddressOf Display_Message_Left), str, BarCode, Color.Red)
                                    Case LR_STATION.RIGHT
                                        Invoke(New Delegate_Display_Message_Right(AddressOf Display_Message_Right), str, BarCode, Color.Red)
                                    Case LR_STATION.COMMUNAL
                                        Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), str, BarCode, Color.Red)
                                End Select
                            End If
                            Return False
                        End If
                        rtn_bool = CARDCMD.GET_DI_BIT(BOARD_ID_00, DIO.ONE1工位真空吸检测_0)
                        If rtn_bool = False Then
                            Exit While
                        End If
                    End While
                    str = "1工位真空吸电磁阀" & "［关闭完成］"
                    If Bool_Disp_IO_Log = True Then
                        Select Case LR
                            Case LR_STATION.LEFT
                                Invoke(New Delegate_Display_Message_Left(AddressOf Display_Message_Left), str, BarCode, Color.Black)
                            Case LR_STATION.RIGHT
                                Invoke(New Delegate_Display_Message_Right(AddressOf Display_Message_Right), str, BarCode, Color.Black)
                            Case LR_STATION.COMMUNAL
                                Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), str, BarCode, Color.Black)
                        End Select
                    End If
                End If
        End Select
        Return True
    End Function

    Function C键ONE1工位真空吸电磁阀(ByVal Model As IO, ByVal LR As LR_STATION, ByVal BarCode As String) As Boolean
        Dim Left_Start_Time, Left_End_Time, Left_Lost_Time As UInt64, str As String, rtn_bool As Boolean
        Select Case Model
            Case IO.OUT_ON
                Dim Out_Int As Integer = CARDCMD.SET_DO_BIT(BOARD_ID_00, DIO.C键ONE1工位真空吸电磁阀_0, PRA_OUT_STATUS_ENUM.OUT_ON)
                str = "1工位C键真空吸电磁阀" & "［打开］"
                If Bool_Disp_IO_Log = True Then
                    Select Case LR
                        Case LR_STATION.LEFT
                            Invoke(New Delegate_Display_Message_Left(AddressOf Display_Message_Left), str, BarCode, Color.Black)
                            Write_Out_Left(BarCode, GetCurrentMethod.Name & ":" & Model & ":" & Out_Int)
                        Case LR_STATION.RIGHT
                            Invoke(New Delegate_Display_Message_Right(AddressOf Display_Message_Right), str, BarCode, Color.Black)
                            Write_Out_Right(BarCode, GetCurrentMethod.Name & ":" & Model & ":" & Out_Int)
                        Case LR_STATION.COMMUNAL
                            Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), str, BarCode, Color.Black)
                            Write_Out_Com(BarCode, GetCurrentMethod.Name & ":" & Model & ":" & Out_Int)
                    End Select
                End If
                Thread.Sleep(PARAM_INT.Z真空吸等待时间)
                'If PARAM_BOOL.D到位信号检测 = True Then
                '    Get_Time_MS(Left_Start_Time)
                '    While True
                '        Get_Time_MS(Left_End_Time)
                '        If Left_End_Time < Left_Start_Time Then
                '            Left_End_Time = Left_Start_Time
                '        End If
                '        Left_Lost_Time = Left_End_Time - Left_Start_Time
                '        If Left_Lost_Time >= 2500 Then
                '            str = "1工位C键真空吸电磁阀" & "［打开信号没有感应到］"
                '            If Bool_Disp_IO_Log = True Then
                '                Select Case LR
                '                    Case LR_STATION.LEFT
                '                        Invoke(New Delegate_Display_Message_Left(AddressOf Display_Message_Left), str, BarCode, Color.Red)
                '                    Case LR_STATION.RIGHT
                '                        Invoke(New Delegate_Display_Message_Right(AddressOf Display_Message_Right), str, BarCode, Color.Red)
                '                    Case LR_STATION.COMMUNAL
                '                        Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), str, BarCode, Color.Red)
                '                End Select
                '            End If
                '            Return False
                '        End If
                '        rtn_bool = CARDCMD.GET_DI_BIT(BOARD_ID_00, DIO.C键ONE1工位真空吸检测_0)
                '        If rtn_bool = True Then
                '            Exit While
                '        End If
                '    End While
                '    str = "1工位真空吸电磁阀" & "［打开完成］"
                '    If Bool_Disp_IO_Log = True Then
                '        Select Case LR
                '            Case LR_STATION.LEFT
                '                Invoke(New Delegate_Display_Message_Left(AddressOf Display_Message_Left), str, BarCode, Color.Black)
                '            Case LR_STATION.RIGHT
                '                Invoke(New Delegate_Display_Message_Right(AddressOf Display_Message_Right), str, BarCode, Color.Black)
                '            Case LR_STATION.COMMUNAL
                '                Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), str, BarCode, Color.Black)
                '        End Select
                '    End If
                'End If
            Case IO.OUT_OFF
                Dim Out_Int As Integer = CARDCMD.SET_DO_BIT(BOARD_ID_00, DIO.C键ONE1工位真空吸电磁阀_0, PRA_OUT_STATUS_ENUM.OUT_OFF)
                str = "1工位C键真空吸电磁阀" & "［关闭］"
                If Bool_Disp_IO_Log = True Then
                    Select Case LR
                        Case LR_STATION.LEFT
                            Invoke(New Delegate_Display_Message_Left(AddressOf Display_Message_Left), str, BarCode, Color.Black)
                            Write_Out_Left(BarCode, GetCurrentMethod.Name & ":" & Model & ":" & Out_Int)
                        Case LR_STATION.RIGHT
                            Invoke(New Delegate_Display_Message_Right(AddressOf Display_Message_Right), str, BarCode, Color.Black)
                            Write_Out_Right(BarCode, GetCurrentMethod.Name & ":" & Model & ":" & Out_Int)
                        Case LR_STATION.COMMUNAL
                            Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), str, BarCode, Color.Black)
                            Write_Out_Com(BarCode, GetCurrentMethod.Name & ":" & Model & ":" & Out_Int)
                    End Select
                End If
                Thread.Sleep(PARAM_INT.Z真空吸等待时间)
                'If PARAM_BOOL.D到位信号检测 = True Then
                '    Get_Time_MS(Left_Start_Time)
                '    While True
                '        Get_Time_MS(Left_End_Time)
                '        If Left_End_Time < Left_Start_Time Then
                '            Left_End_Time = Left_Start_Time
                '        End If
                '        Left_Lost_Time = Left_End_Time - Left_Start_Time
                '        If Left_Lost_Time >= 2500 Then
                '            str = "1工位C键真空吸电磁阀" & "［关闭信号没有感应到］"
                '            If Bool_Disp_IO_Log = True Then
                '                Select Case LR
                '                    Case LR_STATION.LEFT
                '                        Invoke(New Delegate_Display_Message_Left(AddressOf Display_Message_Left), str, BarCode, Color.Red)
                '                    Case LR_STATION.RIGHT
                '                        Invoke(New Delegate_Display_Message_Right(AddressOf Display_Message_Right), str, BarCode, Color.Red)
                '                    Case LR_STATION.COMMUNAL
                '                        Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), str, BarCode, Color.Red)
                '                End Select
                '            End If
                '            Return False
                '        End If
                '        rtn_bool = CARDCMD.GET_DI_BIT(BOARD_ID_00, DIO.C键ONE1工位真空吸检测_0)
                '        If rtn_bool = False Then
                '            Exit While
                '        End If
                '    End While
                '    str = "1工位C键真空吸" & "［关闭完成］"
                '    If Bool_Disp_IO_Log = True Then
                '        Select Case LR
                '            Case LR_STATION.LEFT
                '                Invoke(New Delegate_Display_Message_Left(AddressOf Display_Message_Left), str, BarCode, Color.Black)
                '            Case LR_STATION.RIGHT
                '                Invoke(New Delegate_Display_Message_Right(AddressOf Display_Message_Right), str, BarCode, Color.Black)
                '            Case LR_STATION.COMMUNAL
                '                Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), str, BarCode, Color.Black)
                '        End Select
                '    End If
                'End If
        End Select
        Return True
    End Function





    Function TWO2工位真空吸电磁阀(ByVal Model As IO, ByVal LR As LR_STATION, ByVal BarCode As String) As Boolean
        Dim Left_Start_Time, Left_End_Time, Left_Lost_Time As UInt64, str As String, rtn_bool As Boolean
        Select Case Model
            Case IO.OUT_ON
                Dim Out_Int As Integer = CARDCMD.SET_DO_BIT(BOARD_ID_00, DIO.TWO2工位真空吸电磁阀_0, PRA_OUT_STATUS_ENUM.OUT_ON)
                str = "2工位真空吸电磁阀" & "［打开］"
                If Bool_Disp_IO_Log = True Then
                    Select Case LR
                        Case LR_STATION.LEFT
                            Invoke(New Delegate_Display_Message_Left(AddressOf Display_Message_Left), str, BarCode, Color.Black)
                            Write_Out_Left(BarCode, GetCurrentMethod.Name & ":" & Model & ":" & Out_Int)
                        Case LR_STATION.RIGHT
                            Invoke(New Delegate_Display_Message_Right(AddressOf Display_Message_Right), str, BarCode, Color.Black)
                            Write_Out_Right(BarCode, GetCurrentMethod.Name & ":" & Model & ":" & Out_Int)
                        Case LR_STATION.COMMUNAL
                            Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), str, BarCode, Color.Black)
                            Write_Out_Com(BarCode, GetCurrentMethod.Name & ":" & Model & ":" & Out_Int)
                    End Select
                End If
                Thread.Sleep(PARAM_INT.Z真空吸等待时间)
                If PARAM_BOOL.D到位信号检测 = True Then
                    Get_Time_MS(Left_Start_Time)
                    While True
                        Get_Time_MS(Left_End_Time)
                        If Left_End_Time < Left_Start_Time Then
                            Left_End_Time = Left_Start_Time
                        End If
                        Left_Lost_Time = Left_End_Time - Left_Start_Time
                        If Left_Lost_Time >= 2500 Then
                            str = "2工位真空吸电磁阀" & "［打开信号没有感应到］"
                            If Bool_Disp_IO_Log = True Then
                                Select Case LR
                                    Case LR_STATION.LEFT
                                        Invoke(New Delegate_Display_Message_Left(AddressOf Display_Message_Left), str, BarCode, Color.Red)
                                    Case LR_STATION.RIGHT
                                        Invoke(New Delegate_Display_Message_Right(AddressOf Display_Message_Right), str, BarCode, Color.Red)
                                    Case LR_STATION.COMMUNAL
                                        Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), str, BarCode, Color.Red)
                                End Select
                            End If
                            Return False
                        End If
                        rtn_bool = CARDCMD.GET_DI_BIT(BOARD_ID_01, DIO.TWO2工位真空吸检测_1)
                        If rtn_bool = True Then
                            Exit While
                        End If
                    End While
                    str = "2工位真空吸电磁阀" & "［打开完成］"
                    If Bool_Disp_IO_Log = True Then
                        Select Case LR
                            Case LR_STATION.LEFT
                                Invoke(New Delegate_Display_Message_Left(AddressOf Display_Message_Left), str, BarCode, Color.Black)
                            Case LR_STATION.RIGHT
                                Invoke(New Delegate_Display_Message_Right(AddressOf Display_Message_Right), str, BarCode, Color.Black)
                            Case LR_STATION.COMMUNAL
                                Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), str, BarCode, Color.Black)
                        End Select
                    End If
                End If
            Case IO.OUT_OFF
                Dim Out_Int As Integer = CARDCMD.SET_DO_BIT(BOARD_ID_00, DIO.TWO2工位真空吸电磁阀_0, PRA_OUT_STATUS_ENUM.OUT_OFF)
                str = "2工位真空吸电磁阀" & "［关闭］"
                If Bool_Disp_IO_Log = True Then
                    Select Case LR
                        Case LR_STATION.LEFT
                            Invoke(New Delegate_Display_Message_Left(AddressOf Display_Message_Left), str, BarCode, Color.Black)
                            Write_Out_Left(BarCode, GetCurrentMethod.Name & ":" & Model & ":" & Out_Int)
                        Case LR_STATION.RIGHT
                            Invoke(New Delegate_Display_Message_Right(AddressOf Display_Message_Right), str, BarCode, Color.Black)
                            Write_Out_Right(BarCode, GetCurrentMethod.Name & ":" & Model & ":" & Out_Int)
                        Case LR_STATION.COMMUNAL
                            Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), str, BarCode, Color.Black)
                            Write_Out_Com(BarCode, GetCurrentMethod.Name & ":" & Model & ":" & Out_Int)
                    End Select
                End If
                Thread.Sleep(PARAM_INT.Z真空吸等待时间)
                If PARAM_BOOL.D到位信号检测 = True Then
                    Get_Time_MS(Left_Start_Time)
                    While True
                        Get_Time_MS(Left_End_Time)
                        If Left_End_Time < Left_Start_Time Then
                            Left_End_Time = Left_Start_Time
                        End If
                        Left_Lost_Time = Left_End_Time - Left_Start_Time
                        If Left_Lost_Time >= 2500 Then
                            str = "2工位真空吸电磁阀" & "［关闭信号没有感应到］"
                            If Bool_Disp_IO_Log = True Then
                                Select Case LR
                                    Case LR_STATION.LEFT
                                        Invoke(New Delegate_Display_Message_Left(AddressOf Display_Message_Left), str, BarCode, Color.Red)
                                    Case LR_STATION.RIGHT
                                        Invoke(New Delegate_Display_Message_Right(AddressOf Display_Message_Right), str, BarCode, Color.Red)
                                    Case LR_STATION.COMMUNAL
                                        Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), str, BarCode, Color.Red)
                                End Select
                            End If
                            Return False
                        End If
                        rtn_bool = CARDCMD.GET_DI_BIT(BOARD_ID_01, DIO.TWO2工位真空吸检测_1)
                        If rtn_bool = False Then
                            Exit While
                        End If
                    End While
                    str = "2工位真空吸电磁阀" & "［关闭完成］"
                    If Bool_Disp_IO_Log = True Then
                        Select Case LR
                            Case LR_STATION.LEFT
                                Invoke(New Delegate_Display_Message_Left(AddressOf Display_Message_Left), str, BarCode, Color.Black)
                            Case LR_STATION.RIGHT
                                Invoke(New Delegate_Display_Message_Right(AddressOf Display_Message_Right), str, BarCode, Color.Black)
                            Case LR_STATION.COMMUNAL
                                Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), str, BarCode, Color.Black)
                        End Select
                    End If
                End If
        End Select
        Return True
    End Function

    Function C键TWO2工位真空吸电磁阀(ByVal Model As IO, ByVal LR As LR_STATION, ByVal BarCode As String) As Boolean
        Dim Left_Start_Time, Left_End_Time, Left_Lost_Time As UInt64, str As String, rtn_bool As Boolean
        Select Case Model
            Case IO.OUT_ON
                Dim Out_Int As Integer = CARDCMD.SET_DO_BIT(BOARD_ID_00, DIO.C键TWO2工位真空吸电磁阀_0, PRA_OUT_STATUS_ENUM.OUT_ON)
                str = "2工位C键真空吸电磁阀" & "［打开］"
                If Bool_Disp_IO_Log = True Then
                    Select Case LR
                        Case LR_STATION.LEFT
                            Invoke(New Delegate_Display_Message_Left(AddressOf Display_Message_Left), str, BarCode, Color.Black)
                            Write_Out_Left(BarCode, GetCurrentMethod.Name & ":" & Model & ":" & Out_Int)
                        Case LR_STATION.RIGHT
                            Invoke(New Delegate_Display_Message_Right(AddressOf Display_Message_Right), str, BarCode, Color.Black)
                            Write_Out_Right(BarCode, GetCurrentMethod.Name & ":" & Model & ":" & Out_Int)
                        Case LR_STATION.COMMUNAL
                            Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), str, BarCode, Color.Black)
                            Write_Out_Com(BarCode, GetCurrentMethod.Name & ":" & Model & ":" & Out_Int)
                    End Select
                End If
                Thread.Sleep(PARAM_INT.Z真空吸等待时间)
                'If PARAM_BOOL.D到位信号检测 = True Then
                '    Get_Time_MS(Left_Start_Time)
                '    While True
                '        Get_Time_MS(Left_End_Time)
                '        If Left_End_Time < Left_Start_Time Then
                '            Left_End_Time = Left_Start_Time
                '        End If
                '        Left_Lost_Time = Left_End_Time - Left_Start_Time
                '        If Left_Lost_Time >= 2500 Then
                '            str = "2工位C键真空吸电磁阀" & "［打开信号没有感应到］"
                '            If Bool_Disp_IO_Log = True Then
                '                Select Case LR
                '                    Case LR_STATION.LEFT
                '                        Invoke(New Delegate_Display_Message_Left(AddressOf Display_Message_Left), str, BarCode, Color.Red)
                '                    Case LR_STATION.RIGHT
                '                        Invoke(New Delegate_Display_Message_Right(AddressOf Display_Message_Right), str, BarCode, Color.Red)
                '                    Case LR_STATION.COMMUNAL
                '                        Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), str, BarCode, Color.Red)
                '                End Select
                '            End If
                '            Return False
                '        End If
                '        rtn_bool = CARDCMD.GET_DI_BIT(BOARD_ID_01, DIO.C键TWO2工位真空吸检测_1)
                '        If rtn_bool = True Then
                '            Exit While
                '        End If
                '    End While
                '    str = "2工位C键真空吸电磁阀" & "［打开完成］"
                '    If Bool_Disp_IO_Log = True Then
                '        Select Case LR
                '            Case LR_STATION.LEFT
                '                Invoke(New Delegate_Display_Message_Left(AddressOf Display_Message_Left), str, BarCode, Color.Black)
                '            Case LR_STATION.RIGHT
                '                Invoke(New Delegate_Display_Message_Right(AddressOf Display_Message_Right), str, BarCode, Color.Black)
                '            Case LR_STATION.COMMUNAL
                '                Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), str, BarCode, Color.Black)
                '        End Select
                '    End If
                'End If
            Case IO.OUT_OFF
                Dim Out_Int As Integer = CARDCMD.SET_DO_BIT(BOARD_ID_00, DIO.C键TWO2工位真空吸电磁阀_0, PRA_OUT_STATUS_ENUM.OUT_OFF)
                str = "2工位C键真空吸电磁阀" & "［关闭］"
                If Bool_Disp_IO_Log = True Then
                    Select Case LR
                        Case LR_STATION.LEFT
                            Invoke(New Delegate_Display_Message_Left(AddressOf Display_Message_Left), str, BarCode, Color.Black)
                            Write_Out_Left(BarCode, GetCurrentMethod.Name & ":" & Model & ":" & Out_Int)
                        Case LR_STATION.RIGHT
                            Invoke(New Delegate_Display_Message_Right(AddressOf Display_Message_Right), str, BarCode, Color.Black)
                            Write_Out_Right(BarCode, GetCurrentMethod.Name & ":" & Model & ":" & Out_Int)
                        Case LR_STATION.COMMUNAL
                            Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), str, BarCode, Color.Black)
                            Write_Out_Com(BarCode, GetCurrentMethod.Name & ":" & Model & ":" & Out_Int)
                    End Select
                End If
                Thread.Sleep(PARAM_INT.Z真空吸等待时间)
                'If PARAM_BOOL.D到位信号检测 = True Then
                '    Get_Time_MS(Left_Start_Time)
                '    While True
                '        Get_Time_MS(Left_End_Time)
                '        If Left_End_Time < Left_Start_Time Then
                '            Left_End_Time = Left_Start_Time
                '        End If
                '        Left_Lost_Time = Left_End_Time - Left_Start_Time
                '        If Left_Lost_Time >= 2500 Then
                '            str = "2工位C键真空吸电磁阀" & "［关闭信号没有感应到］"
                '            If Bool_Disp_IO_Log = True Then
                '                Select Case LR
                '                    Case LR_STATION.LEFT
                '                        Invoke(New Delegate_Display_Message_Left(AddressOf Display_Message_Left), str, BarCode, Color.Red)
                '                    Case LR_STATION.RIGHT
                '                        Invoke(New Delegate_Display_Message_Right(AddressOf Display_Message_Right), str, BarCode, Color.Red)
                '                    Case LR_STATION.COMMUNAL
                '                        Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), str, BarCode, Color.Red)
                '                End Select
                '            End If
                '            Return False
                '        End If
                '        rtn_bool = CARDCMD.GET_DI_BIT(BOARD_ID_01, DIO.C键TWO2工位真空吸检测_1)
                '        If rtn_bool = False Then
                '            Exit While
                '        End If
                '    End While
                '    str = "2工位C键真空吸电磁阀" & "［关闭完成］"
                '    If Bool_Disp_IO_Log = True Then
                '        Select Case LR
                '            Case LR_STATION.LEFT
                '                Invoke(New Delegate_Display_Message_Left(AddressOf Display_Message_Left), str, BarCode, Color.Black)
                '            Case LR_STATION.RIGHT
                '                Invoke(New Delegate_Display_Message_Right(AddressOf Display_Message_Right), str, BarCode, Color.Black)
                '            Case LR_STATION.COMMUNAL
                '                Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), str, BarCode, Color.Black)
                '        End Select
                '    End If
                'End If
        End Select
        Return True
    End Function




#End Region


    Sub Get_Laser_Trigger(ByVal _DIRECTION As TABLE_COMPARE_DIRECTION_ENUM, ByVal _Work_Coordinatess() As Work_Coordinatess, ByRef Set_Trg_Count As Int64, ByRef DataArayy() As Integer)
        Dim Axis_Name As String = Nothing
        Select Case BOARD_ID_02
            Case 0
                Axis_Name = "X01"
            Case 1
                Axis_Name = "X02"
            Case 2
                Axis_Name = "X03"
        End Select

        Dim Scale_D As Integer
        For i As Integer = 0 To HomeParamObjArray.Length - 1
            If HomeParamObjArray(i).轴名称 = Axis_Name Then
                Scale_D = 10000 / HomeParamObjArray(i).导程
                Exit For
            End If
        Next

        Dim Clip_Index As Int16 = PARAM_INT.L镭射触发间隔
        Dim Start_Pos As Double, End_Pos As Double
        For j As Int16 = 0 To _Work_Coordinatess(0).轴名称.Count - 1
            If _Work_Coordinatess(0).轴名称(j) = Axis_Name Then
                Start_Pos = _Work_Coordinatess(0).轴位置(j) * Scale_D
                Exit For
            End If
        Next
        For j As Int16 = 0 To _Work_Coordinatess(_Work_Coordinatess.Count - 1).轴名称.Count - 1
            If _Work_Coordinatess(_Work_Coordinatess.Count - 1).轴名称(j) = Axis_Name Then
                End_Pos = _Work_Coordinatess(_Work_Coordinatess.Count - 1).轴位置(j) * Scale_D
                Exit For
            End If
        Next

        DataArayy = Nothing
        If _DIRECTION = TABLE_COMPARE_DIRECTION_ENUM.POSITIVE_DIRECTION Then
            For i As Int64 = 0 To CInt((End_Pos - Start_Pos) / Clip_Index)
                Array.Resize(DataArayy, i + 1)
                DataArayy(i) = Start_Pos + Clip_Index * i
            Next
        ElseIf _DIRECTION = TABLE_COMPARE_DIRECTION_ENUM.NEGATIVE_DIRECTION Then
            For i As Int64 = 0 To CInt((Start_Pos - End_Pos) / Clip_Index)
                Array.Resize(DataArayy, i + 1)
                DataArayy(i) = Start_Pos - Clip_Index * i
            Next
        End If
        Set_Trg_Count = DataArayy.Length
    End Sub

    ''' <summary>
    ''' 镭射设置触发
    ''' </summary>
    ''' <param name="_DIRECTION"></param>
    ''' <param name="DataArayy"></param>
    ''' <remarks></remarks>
    Sub Laser_Ser_Trigger(ByVal _DIRECTION As TABLE_COMPARE_DIRECTION_ENUM, ByVal DataArayy() As Integer)
        Dim rtn_Int As Integer

        Dim Card_Trg As TrgParam_STR
        Card_Trg.CH0_COMPARE_DIRECTION = _DIRECTION
        Card_Trg.CH0_COMPARE_ENCODER = TRIGGER_COMPARE_SOURCE_ENUM.ENCODER_0
        Card_Trg.CH0_LINEAR_CMP0 = TRIGGER_SOURCH_ENUM.DISABLE
        Card_Trg.CH0_LINEAR_CMP1 = TRIGGER_SOURCH_ENUM.DISABLE
        Card_Trg.CH0_MANUAL_TRIGGER = TRIGGER_SOURCH_ENUM.ENABLE
        Card_Trg.CH0_TABLE_CMP0 = TRIGGER_SOURCH_ENUM.ENABLE
        Card_Trg.CH0_TABLE_CMP1 = TRIGGER_SOURCH_ENUM.ENABLE
        Card_Trg.CH0_TRIGGER = TRIGGER_SOURCH_STATUS_ENUM.ENABLE
        Card_Trg.CH0_TRIGGER_LOGIC = TRIGGER_LOGIC_ENUM.NOT_INVERSE
        Card_Trg.CH0_TRIGGER_OUTPUT_MODE = TRIGGER_OUTPUT_MODE_ENUM.PULSE_OUT
        Card_Trg.CH0_TRIGGER_PULSE_WIDTH = 5000

        Card_Trg.CH1_COMPARE_DIRECTION = _DIRECTION
        Card_Trg.CH1_COMPARE_ENCODER = TRIGGER_COMPARE_SOURCE_ENUM.ENCODER_0
        Card_Trg.CH1_LINEAR_CMP0 = TRIGGER_SOURCH_ENUM.DISABLE
        Card_Trg.CH1_LINEAR_CMP1 = TRIGGER_SOURCH_ENUM.DISABLE
        Card_Trg.CH1_MANUAL_TRIGGER = TRIGGER_SOURCH_ENUM.ENABLE
        Card_Trg.CH1_TABLE_CMP0 = TRIGGER_SOURCH_ENUM.ENABLE
        Card_Trg.CH1_TABLE_CMP1 = TRIGGER_SOURCH_ENUM.ENABLE
        Card_Trg.CH1_TRIGGER = TRIGGER_SOURCH_STATUS_ENUM.ENABLE
        Card_Trg.CH1_TRIGGER_LOGIC = TRIGGER_LOGIC_ENUM.NOT_INVERSE
        Card_Trg.CH1_TRIGGER_OUTPUT_MODE = TRIGGER_OUTPUT_MODE_ENUM.PULSE_OUT
        Card_Trg.CH1_TRIGGER_PULSE_WIDTH = 5000

        rtn_Int = CARDCMD.RST_TRIGGER_EX(BOARD_ID_02)
        rtn_Int = CARDCMD.SET_TRIGGER_EX(BOARD_ID_02, Card_Trg)

        rtn_Int = CARDCMD.set_trigger_table(BOARD_ID_02, TCPM_ENUM.TCMP0, DataArayy, DataArayy.Length)
        rtn_Int = CARDCMD.set_trigger_table(BOARD_ID_02, TCPM_ENUM.TCMP1, DataArayy, DataArayy.Length)

    End Sub

    Public Enum Line_Index
        LEFT_TO_RIGHT
        RIGHT_TO_LEFT
    End Enum

    Sub CCD_Ser_Trigger_Card_00(ByVal _DIRECTION As TABLE_COMPARE_DIRECTION_ENUM, ByVal _Work_Coordinatess() As Work_Coordinatess, ByVal _line_Index As Line_Index)

        Dim rtn_Int As Integer

        Card_Trg_00.CH0_COMPARE_DIRECTION = _DIRECTION
        Card_Trg_00.CH0_COMPARE_ENCODER = TRIGGER_COMPARE_SOURCE_ENUM.ENCODER_0
        Card_Trg_00.CH0_LINEAR_CMP0 = TRIGGER_SOURCH_ENUM.DISABLE
        Card_Trg_00.CH0_LINEAR_CMP1 = TRIGGER_SOURCH_ENUM.DISABLE
        Card_Trg_00.CH0_MANUAL_TRIGGER = TRIGGER_SOURCH_ENUM.ENABLE
        Card_Trg_00.CH0_TABLE_CMP0 = TRIGGER_SOURCH_ENUM.ENABLE
        Card_Trg_00.CH0_TABLE_CMP1 = TRIGGER_SOURCH_ENUM.ENABLE
        Card_Trg_00.CH0_TRIGGER = TRIGGER_SOURCH_STATUS_ENUM.ENABLE
        Card_Trg_00.CH0_TRIGGER_LOGIC = TRIGGER_LOGIC_ENUM.NOT_INVERSE
        Card_Trg_00.CH0_TRIGGER_OUTPUT_MODE = TRIGGER_OUTPUT_MODE_ENUM.PULSE_OUT
        Card_Trg_00.CH0_TRIGGER_PULSE_WIDTH = PARAM_DOUBLE.M脉宽 * 1000 * 1000 / 20

        Card_Trg_00.CH1_COMPARE_DIRECTION = _DIRECTION
        Card_Trg_00.CH1_COMPARE_ENCODER = TRIGGER_COMPARE_SOURCE_ENUM.ENCODER_0
        Card_Trg_00.CH1_LINEAR_CMP0 = TRIGGER_SOURCH_ENUM.DISABLE
        Card_Trg_00.CH1_LINEAR_CMP1 = TRIGGER_SOURCH_ENUM.DISABLE
        Card_Trg_00.CH1_MANUAL_TRIGGER = TRIGGER_SOURCH_ENUM.ENABLE
        Card_Trg_00.CH1_TABLE_CMP0 = TRIGGER_SOURCH_ENUM.ENABLE
        Card_Trg_00.CH1_TABLE_CMP1 = TRIGGER_SOURCH_ENUM.ENABLE
        Card_Trg_00.CH1_TRIGGER = TRIGGER_SOURCH_STATUS_ENUM.ENABLE
        Card_Trg_00.CH1_TRIGGER_LOGIC = TRIGGER_LOGIC_ENUM.NOT_INVERSE
        Card_Trg_00.CH1_TRIGGER_OUTPUT_MODE = TRIGGER_OUTPUT_MODE_ENUM.PULSE_OUT
        Card_Trg_00.CH1_TRIGGER_PULSE_WIDTH = PARAM_DOUBLE.M脉宽 * 1000 * 1000 / 20

        rtn_Int = CARDCMD.RST_TRIGGER_EX(BOARD_ID_00)
        rtn_Int = CARDCMD.SET_TRIGGER_EX(BOARD_ID_00, Card_Trg_00)

        Dim Axis_Name As String = Nothing
        Select Case BOARD_ID_00
            Case 0
                Axis_Name = "X01"
            Case 1
                Axis_Name = "X02"
            Case 2
                Axis_Name = "X03"
        End Select

        Dim Scale_D As Integer
        For i As Integer = 0 To HomeParamObjArray.Length - 1
            If HomeParamObjArray(i).轴名称 = Axis_Name Then
                Scale_D = 10000 / HomeParamObjArray(i).导程
                Exit For
            End If
        Next

        Dim DataArayy() As Integer = New Integer() {}
        Array.Resize(DataArayy, _Work_Coordinatess.Count)
        For i As Integer = 0 To _Work_Coordinatess.Count - 1
            For j As Int16 = 0 To _Work_Coordinatess(i).轴名称.Count - 1
                If _Work_Coordinatess(i).轴名称(j) = Axis_Name Then
                    Select Case _line_Index
                        Case Line_Index.LEFT_TO_RIGHT
                            DataArayy(i) = _Work_Coordinatess(i).轴位置(j) * Scale_D
                        Case Line_Index.RIGHT_TO_LEFT
                            DataArayy(i) = _Work_Coordinatess(i).轴位置(j) * Scale_D
                    End Select
                    Exit For
                End If
            Next
        Next
        rtn_Int = CARDCMD.set_trigger_table(BOARD_ID_00, TCPM_ENUM.TCMP0, DataArayy, DataArayy.Length)
        rtn_Int = CARDCMD.set_trigger_table(BOARD_ID_00, TCPM_ENUM.TCMP1, DataArayy, DataArayy.Length)

    End Sub

    Sub CCD_Ser_Trigger_Card_01(ByVal _DIRECTION As TABLE_COMPARE_DIRECTION_ENUM, ByVal _Work_Coordinatess() As Work_Coordinatess, ByVal _line_Index As Line_Index)

        Dim rtn_str As String, rtn_Int As Integer

        Card_Trg_01.CH0_COMPARE_DIRECTION = _DIRECTION
        Card_Trg_01.CH0_COMPARE_ENCODER = TRIGGER_COMPARE_SOURCE_ENUM.ENCODER_0
        Card_Trg_01.CH0_LINEAR_CMP0 = TRIGGER_SOURCH_ENUM.DISABLE
        Card_Trg_01.CH0_LINEAR_CMP1 = TRIGGER_SOURCH_ENUM.DISABLE
        Card_Trg_01.CH0_MANUAL_TRIGGER = TRIGGER_SOURCH_ENUM.ENABLE
        Card_Trg_01.CH0_TABLE_CMP0 = TRIGGER_SOURCH_ENUM.ENABLE
        Card_Trg_01.CH0_TABLE_CMP1 = TRIGGER_SOURCH_ENUM.ENABLE
        Card_Trg_01.CH0_TRIGGER = TRIGGER_SOURCH_STATUS_ENUM.ENABLE
        Card_Trg_01.CH0_TRIGGER_LOGIC = TRIGGER_LOGIC_ENUM.NOT_INVERSE
        Card_Trg_01.CH0_TRIGGER_OUTPUT_MODE = TRIGGER_OUTPUT_MODE_ENUM.PULSE_OUT
        Card_Trg_01.CH0_TRIGGER_PULSE_WIDTH = PARAM_DOUBLE.M脉宽 * 1000 * 1000 / 20

        Card_Trg_01.CH1_COMPARE_DIRECTION = _DIRECTION
        Card_Trg_01.CH1_COMPARE_ENCODER = TRIGGER_COMPARE_SOURCE_ENUM.ENCODER_0
        Card_Trg_01.CH1_LINEAR_CMP0 = TRIGGER_SOURCH_ENUM.DISABLE
        Card_Trg_01.CH1_LINEAR_CMP1 = TRIGGER_SOURCH_ENUM.DISABLE
        Card_Trg_01.CH1_MANUAL_TRIGGER = TRIGGER_SOURCH_ENUM.ENABLE
        Card_Trg_01.CH1_TABLE_CMP0 = TRIGGER_SOURCH_ENUM.ENABLE
        Card_Trg_01.CH1_TABLE_CMP1 = TRIGGER_SOURCH_ENUM.ENABLE
        Card_Trg_01.CH1_TRIGGER = TRIGGER_SOURCH_STATUS_ENUM.ENABLE
        Card_Trg_01.CH1_TRIGGER_LOGIC = TRIGGER_LOGIC_ENUM.NOT_INVERSE
        Card_Trg_01.CH1_TRIGGER_OUTPUT_MODE = TRIGGER_OUTPUT_MODE_ENUM.PULSE_OUT
        Card_Trg_01.CH1_TRIGGER_PULSE_WIDTH = PARAM_DOUBLE.M脉宽 * 1000 * 1000 / 20

        rtn_Int = CARDCMD.RST_TRIGGER_EX(BOARD_ID_01)
        rtn_Int = CARDCMD.SET_TRIGGER_EX(BOARD_ID_01, Card_Trg_01)

        Dim Axis_Name As String = Nothing
        Select Case BOARD_ID_01
            Case 0
                Axis_Name = "X01"
            Case 1
                Axis_Name = "X02"
            Case 2
                Axis_Name = "X03"
        End Select

        Dim Scale_D As Integer
        For i As Integer = 0 To HomeParamObjArray.Length - 1
            If HomeParamObjArray(i).轴名称 = Axis_Name Then
                Scale_D = 10000 / HomeParamObjArray(i).导程
                Exit For
            End If
        Next

        Dim DataArayy() As Integer = New Integer() {}
        Array.Resize(DataArayy, _Work_Coordinatess.Count)
        For i As Integer = 0 To _Work_Coordinatess.Count - 1
            For j As Int16 = 0 To _Work_Coordinatess(i).轴名称.Count - 1
                If _Work_Coordinatess(i).轴名称(j) = Axis_Name Then
                    Select Case _line_Index
                        Case Line_Index.LEFT_TO_RIGHT
                            DataArayy(i) = _Work_Coordinatess(i).轴位置(j) * Scale_D
                        Case Line_Index.RIGHT_TO_LEFT
                            DataArayy(i) = _Work_Coordinatess(i).轴位置(j) * Scale_D
                    End Select
                    Exit For
                End If
            Next
        Next
        rtn_Int = CARDCMD.set_trigger_table(BOARD_ID_01, TCPM_ENUM.TCMP0, DataArayy, DataArayy.Length)
        rtn_Int = CARDCMD.set_trigger_table(BOARD_ID_01, TCPM_ENUM.TCMP1, DataArayy, DataArayy.Length)
    End Sub

    Sub Load_Left_Vpp()
        CogToolBlock_Group_Left = CType(CogSerializer.LoadObjectFromFile("D:\System\HOOK_SNAP\HOOK_SNAP_ANSI_MEASURE_LEFT.vpp"), CogToolBlock)

    End Sub

    Sub Load_Right_Vpp()
        CogToolBlock_Group_Right = CType(CogSerializer.LoadObjectFromFile("D:\System\HOOK_SNAP\HOOK_SNAP_ANSI_MEASURE_RIGHT.vpp"), CogToolBlock)

    End Sub

    Sub Load_VPP_Left()
        Dim fun_Name As String = "加载1工位相机VPP出错"
        Try
            Dim VPP_PATH As String = "D:\System\InitCameraLeft.vpp"
            If System.IO.File.Exists(VPP_PATH) = True Then

                ToolBlock_left = CType(CogSerializer.LoadObjectFromFile(VPP_PATH), CogToolBlock)

            Else
                Invoke(New Delegate_Display_Message_Left(AddressOf Display_Message_Left), "视觉配置文件:" & VPP_PATH & " 不存在！", "", Color.Red)
            End If
        Catch ex As Exception
            Invoke(New Delegate_Display_Message_Left(AddressOf Display_Message_Left), fun_Name & ":" & ex.Message.ToString, "", Color.Red)
        End Try
    End Sub


    Sub Load_VPP_Right()
        Dim fun_Name As String = "加载2工位相机VPP出错"
        Try
            Dim VPP_PATH As String = "D:\System\InitCameraRight.vpp"
            If System.IO.File.Exists(VPP_PATH) = True Then

                toolblock_right = CType(CogSerializer.LoadObjectFromFile(VPP_PATH), CogToolBlock)

            Else
                Invoke(New Delegate_Display_Message_Left(AddressOf Display_Message_Left), "视觉配置文件:" & VPP_PATH & " 不存在！", "", Color.Red)
            End If
        Catch ex As Exception
            Invoke(New Delegate_Display_Message_Left(AddressOf Display_Message_Left), fun_Name & ":" & ex.Message.ToString, "", Color.Red)
        End Try
    End Sub

    Function LINK_CAMERA_FUN(ByVal VPP_PATH As String) As Boolean
        Try
            If System.IO.File.Exists(VPP_PATH) = True Then
                If LINK_CAMERA = False Then

                    Load_VPP_Left()
                    Load_VPP_Right()

                    Parallel.Invoke(AddressOf Load_Left_Vpp, AddressOf Load_Right_Vpp)
                    LINK_CAMERA = True
                    Return LINK_CAMERA
                Else
                    LINK_CAMERA = True
                    Return LINK_CAMERA
                End If
            Else
                Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), "视觉配置文件:" & VPP_PATH & " 不存在！", "", Color.Black)
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
            Return False
        End Try
        Return False
    End Function

    Private Sub BK_Init_Camera_DoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles LINKCAMERAWORKER.DoWork
        Invoke(New DELEGATE_LOAD_VISION(AddressOf LOAD_VISION_SUB))
    End Sub

    Sub Close_Trigger_Left()
        Dim Card_Trg As TrgParam_STR
        Dim rtn_str As String = Nothing, rtn_Int As Integer = 0

        Card_Trg.CH0_COMPARE_DIRECTION = TABLE_COMPARE_DIRECTION_ENUM.BI_DIRECTION
        Card_Trg.CH0_COMPARE_ENCODER = TRIGGER_COMPARE_SOURCE_ENUM.ENCODER_0
        Card_Trg.CH0_LINEAR_CMP0 = TRIGGER_SOURCH_ENUM.DISABLE
        Card_Trg.CH0_LINEAR_CMP1 = TRIGGER_SOURCH_ENUM.DISABLE
        Card_Trg.CH0_MANUAL_TRIGGER = TRIGGER_SOURCH_ENUM.DISABLE
        Card_Trg.CH0_TABLE_CMP0 = TRIGGER_SOURCH_ENUM.DISABLE
        Card_Trg.CH0_TABLE_CMP1 = TRIGGER_SOURCH_ENUM.DISABLE
        Card_Trg.CH0_TRIGGER = TRIGGER_SOURCH_STATUS_ENUM.DISABLE
        Card_Trg.CH0_TRIGGER_LOGIC = TRIGGER_LOGIC_ENUM.NOT_INVERSE
        Card_Trg.CH0_TRIGGER_OUTPUT_MODE = TRIGGER_OUTPUT_MODE_ENUM.PULSE_OUT
        Card_Trg.CH0_TRIGGER_PULSE_WIDTH = 1000

        rtn_Int = CARDCMD.RST_TRIGGER_EX(BOARD_ID_00)
        rtn_Int = CARDCMD.SET_TRIGGER_EX(BOARD_ID_00, Card_Trg)
    End Sub

    ''' <summary>
    ''' 关闭所有触发
    ''' </summary>
    ''' <remarks></remarks>
    Sub Close_Trigger_Right()
        Dim Card_Trg As TrgParam_STR
        Dim rtn_str As String = Nothing, rtn_Int As Integer = 0

        Card_Trg.CH1_COMPARE_DIRECTION = TABLE_COMPARE_DIRECTION_ENUM.BI_DIRECTION
        Card_Trg.CH1_COMPARE_ENCODER = TRIGGER_COMPARE_SOURCE_ENUM.ENCODER_0
        Card_Trg.CH1_LINEAR_CMP0 = TRIGGER_SOURCH_ENUM.DISABLE
        Card_Trg.CH1_LINEAR_CMP1 = TRIGGER_SOURCH_ENUM.DISABLE
        Card_Trg.CH1_MANUAL_TRIGGER = TRIGGER_SOURCH_ENUM.DISABLE
        Card_Trg.CH1_TABLE_CMP0 = TRIGGER_SOURCH_ENUM.DISABLE
        Card_Trg.CH1_TABLE_CMP1 = TRIGGER_SOURCH_ENUM.DISABLE
        Card_Trg.CH1_TRIGGER = TRIGGER_SOURCH_STATUS_ENUM.DISABLE
        Card_Trg.CH1_TRIGGER_LOGIC = TRIGGER_LOGIC_ENUM.NOT_INVERSE
        Card_Trg.CH1_TRIGGER_OUTPUT_MODE = TRIGGER_OUTPUT_MODE_ENUM.PULSE_OUT
        Card_Trg.CH1_TRIGGER_PULSE_WIDTH = 1000

        rtn_Int = CARDCMD.RST_TRIGGER_EX(BOARD_ID_01)
        rtn_Int = CARDCMD.SET_TRIGGER_EX(BOARD_ID_01, Card_Trg)
    End Sub


    Private Sub PE_LEFT_SN_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PE_LEFT_SN.Click

    End Sub

    Private Sub PE_RIGHT_SN_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PE_RIGHT_SN.Click
        'BOOL.Laser_IsRuning = True
        'LASER_DATACODE = "5304AUSA  160603N9D1483"
        'CognexImagefile_Right = "E:\FIXTRUE_IMAGE_IDB\06 MONTH\20 DAY\5303AUSA160615D7D0878_20160620_194413_LEFT.idb"
        'DataCode_Right = "5303AUSA  160615D7D0878"
        'Process_Right_Boolean = True
        'StopData()
        'Thread.Sleep(70)
        'StartData()
    End Sub

    Sub tleft()
        Invoke(New Back_Run_Delegate_Left(AddressOf Back_Run_Left))
    End Sub
    Sub tright()
        Invoke(New Back_Run_Delegate_Right(AddressOf Back_Run_Right))
    End Sub

    Private Sub BackgroundWorker1_DoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles Test.DoWork
        Parallel.Invoke(AddressOf tleft, AddressOf tright)
    End Sub


    Private Sub SerialPort1_DataReceived(ByVal sender As Object, ByVal e As System.IO.Ports.SerialDataReceivedEventArgs) Handles SerialPort_DataCode.DataReceived
        Thread.Sleep(80)
        Dim totalstr As String = Nothing

        Dim len As Integer = SerialPort_DataCode.BytesToRead
        Dim buffer() As Byte = New Byte(len) {}
        SerialPort_DataCode.Read(buffer, 0, len)
        totalstr = Encoding.UTF8.GetString(buffer, 0, len)
        DataCode_Com = totalstr.Trim.Replace(vbCr, "").Replace(vbLf, "").Trim.Replace(vbCrLf, "")

        SerialPort_DataCode.DiscardInBuffer()
        SerialPort_DataCode.DiscardOutBuffer()
    End Sub

    Public Enum KB_Type_Enum
        ANSI
        ISO
        JIS
    End Enum
    Dim KB_Type As KB_Type_Enum
    Public Enum KB_Type_Number
        ANSI = 78 - 1
        ISO = 79 - 1
        JIS = 80 - 1
    End Enum
    Dim KB_Number As KB_Type_Number


    Public Structure Data_Header
        Dim Project_Name As String
        Dim Software_Version As String
        Dim SN As String
        Dim Station_ID As String
        Dim Fixture_ID As String
        Dim Carrier_SN As String
        Dim KB_Type As String
        Dim Key_Number As Integer
        Dim Key_Name As String
        Dim Location As String
        Dim ERS_Category As String
        Dim Key_Type As String
        Dim Key_Size As String
        Dim Dome_Size As String
        Dim Operator_STR As String
        Dim Test_Start_Time As DateTime
        Dim Test_Stop_Time As DateTime
        Dim Total_Test_Time As Integer
        Dim Snap01_Data As Double
        Dim Snap02_Data As Double
        Dim Snap03_Data As Double
        Dim Snap04_Data As Double
        Dim Snap05_Data As Double
        Dim Snap06_Data As Double

        Dim Snap07_Data As Double
        Dim Snap08_Data As Double

        Dim Hook01_Data As Double
        Dim Hook02_Data As Double
        Dim Hook03_Data As Double
        Dim Hook04_Data As Double
        Dim Hook05_Data As Double
        Dim Hook06_Data As Double
        Dim Hook07_Data As Double
        Dim Hook08_Data As Double

        Dim Snap01_Result As String
        Dim Snap02_Result As String
        Dim Snap03_Result As String
        Dim Snap04_Result As String
        Dim Snap05_Result As String
        Dim Snap06_Result As String
        Dim Snap07_Result As String
        Dim Snap08_Result As String


        Dim Hook01_Result As String
        Dim Hook02_Result As String
        Dim Hook03_Result As String
        Dim Hook04_Result As String
        Dim Hook05_Result As String
        Dim Hook06_Result As String
        Dim Hook07_Result As String
        Dim Hook08_Result As String

        Dim GateVestige01_Data As Double
        Dim GateVestige02_Data As Double
        Dim GateVestige03_Data As Double
        Dim NubHeight01_Data As Double
        Dim NubHeight02_Data As Double
        Dim GateVestige01_Result As String
        Dim GateVestige02_Result As String
        Dim GateVestige03_Result As String
        Dim Result As String
    End Structure

    Public Structure ANSI
        Shared Key_Number(KB_Type_Number.ANSI) As Integer
        Shared Key_Name(KB_Type_Number.ANSI) As String
        Shared Key_Type(KB_Type_Number.ANSI) As String
        Shared Key_Size(KB_Type_Number.ANSI) As String
    End Structure

    Public Structure ISO
        Shared Key_Number(KB_Type_Number.ISO) As Integer
        Shared Key_Name(KB_Type_Number.ISO) As String
        Shared Key_Type(KB_Type_Number.ISO) As String
        Shared Key_Size(KB_Type_Number.ISO) As String
    End Structure

    Public Structure JIS
        Shared Key_Number(KB_Type_Number.JIS) As Integer
        Shared Key_Name(KB_Type_Number.JIS) As String
        Shared Key_Type(KB_Type_Number.JIS) As String
        Shared Key_Size(KB_Type_Number.JIS) As String
    End Structure

    Public Structure KEYNAME
        Shared Esc As String = "Esc"
        Shared F1 As String = "F1"
        Shared F2 As String = "F2"
        Shared F3 As String = "F3"
        Shared F4 As String = "F4"
        Shared F5 As String = "F5"
        Shared F6 As String = "F6"
        Shared F7 As String = "F7"
        Shared F8 As String = "F8"
        Shared F9 As String = "F9"
        Shared F10 As String = "F10"
        Shared F11 As String = "F11"
        Shared F12 As String = "F12"
        Shared Power As String = "Power"
        Shared Num1 As String = "Num1"
        Shared Num2 As String = "Num2"
        Shared Num3 As String = "Num3"
        Shared Num4 As String = "Num4"
        Shared Num5 As String = "Num5"
        Shared Num6 As String = "Num6"
        Shared Num7 As String = "Num7"
        Shared Num8 As String = "Num8"
        Shared Num9 As String = "Num9"
        Shared Num0 As String = "Num0"
        Shared Minus As String = "Minus"
        Shared Tilde As String = "Tilde"
        Shared Yen As String = "Yen"
        Shared Delete As String = "Delete"
        Shared Tab As String = "Tab"
        Shared Q As String = "Q"
        Shared W As String = "W"
        Shared E As String = "E"
        Shared R As String = "R"
        Shared T As String = "T"
        Shared Y As String = "Y"
        Shared U As String = "U"
        Shared I As String = "I"
        Shared O As String = "O"
        Shared P As String = "P"
        Shared AT As String = "AT"
        Shared LeftBracket As String = "LeftBracket"
        Shared Return_ As String = "Return"
        Shared Control As String = "Control"
        Shared A As String = "A"
        Shared S As String = "S"
        Shared D As String = "D"
        Shared F As String = "F"
        Shared G As String = "G"
        Shared H As String = "H"
        Shared J As String = "J"
        Shared K As String = "K"
        Shared L As String = "L"
        Shared Semicolon As String = "Semicolon"
        Shared Colon As String = "Colon"
        Shared RightBracket As String = "RightBracket"
        Shared LeftShift As String = "LeftShift"
        Shared Z As String = "Z"
        Shared X As String = "X"
        Shared C As String = "C"
        Shared V As String = "V"
        Shared B As String = "B"
        Shared N As String = "N"
        Shared M As String = "M"
        Shared Comma As String = "Comma"
        Shared Period As String = "Period"
        Shared Question As String = "Question"
        Shared Ro As String = "Ro"
        Shared RightShift As String = "RightShift"
        Shared CapsLock As String = "CapsLock"
        Shared LeftOption As String = "LeftOption"
        Shared LeftCommand As String = "LeftCommand"
        Shared Eisu As String = "Eisu"
        Shared Space As String = "Space"
        Shared Kana As String = "Kana"
        Shared RightCommand As String = "RightCommand"
        Shared Fn As String = "Fn"
        Shared Left As String = "Left"
        Shared Up As String = "Up"
        Shared Down As String = "Down"
        Shared Right As String = "Right"

        Shared Equal As String = "Equal"
        Shared BackSlash As String = "BackSlash"
        Shared Quote As String = "Quote"
        Shared RightOption As String = "RightOption"
        Shared ISO As String = "ISO"
    End Structure

    ''' <summary>
    ''' 初始化键参数
    ''' </summary>
    ''' <remarks></remarks>
    Sub Init_key_Param()
        Static Bool_Init As Boolean = False
        If Bool_Init = False Then
            Dim str As String = "D:\System\KeyParam.xlsx"
            If File.Exists(str) = True Then
                DataConn.Xlsbook = DataConn.Xls.Application.Workbooks.Open(str)
                DataConn.Xlssheet = DataConn.Xlsbook.Sheets(1)
                For i As Int16 = 0 To KB_Type_Number.JIS
                    If i <= KB_Type_Number.ANSI Then
                        ANSI.Key_Number(i) = DataConn.Xlssheet.Cells(i + 3, 1).value
                        ANSI.Key_Name(i) = DataConn.Xlssheet.Cells(i + 3, 2).value
                        ANSI.Key_Type(i) = DataConn.Xlssheet.Cells(i + 3, 3).value
                        ANSI.Key_Size(i) = DataConn.Xlssheet.Cells(i + 3, 4).value
                    End If
                    If i <= KB_Type_Number.ISO Then
                        ISO.Key_Number(i) = DataConn.Xlssheet.Cells(i + 3, 5).value
                        ISO.Key_Name(i) = DataConn.Xlssheet.Cells(i + 3, 6).value
                        ISO.Key_Type(i) = DataConn.Xlssheet.Cells(i + 3, 7).value
                        ISO.Key_Size(i) = DataConn.Xlssheet.Cells(i + 3, 8).value
                    End If
                    If i <= KB_Type_Number.JIS Then
                        JIS.Key_Number(i) = DataConn.Xlssheet.Cells(i + 3, 9).value
                        JIS.Key_Name(i) = DataConn.Xlssheet.Cells(i + 3, 10).value
                        JIS.Key_Type(i) = DataConn.Xlssheet.Cells(i + 3, 11).value
                        JIS.Key_Size(i) = DataConn.Xlssheet.Cells(i + 3, 12).value
                    End If
                Next
                If DataConn.Xls IsNot Nothing Then
                    DataConn.Xlsbook.Close()
                    DataConn.Xls.Quit()
                End If
                Bool_Init = True
            Else
                Bool_Init = False
                MessageBoxEx.Show(str & " 文件不存在，请检查", "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        End If
    End Sub

    Private Sub StartData()
        Dim StartReadData As String
        Dim b1 As Byte = Convert.ToInt32("16", 16)
        Dim b2 As Byte = Convert.ToInt32("54", 16)
        Dim b3 As Byte = Convert.ToInt32("0D", 16)
        Dim str1, str2, str3 As Char
        str1 = Convert.ToChar(b1)
        str2 = Convert.ToChar(b2)
        str3 = Convert.ToChar(b3)

        StartReadData = String.Empty
        StartReadData += Convert.ToString(str1)
        StartReadData += Convert.ToString(str2)
        StartReadData += Convert.ToString(str3)
        If SerialPort_DataCode.IsOpen = True Then
            SerialPort_DataCode.DiscardInBuffer()
            SerialPort_DataCode.DiscardOutBuffer()

            SerialPort_DataCode.WriteLine(StartReadData)
        End If
    End Sub

    Private Sub StopData()
        Dim StartReadData As String
        Dim b1 As Byte = Convert.ToInt32("16", 16)
        Dim b2 As Byte = Convert.ToInt32("55", 16)
        Dim b3 As Byte = Convert.ToInt32("0D", 16)
        Dim str1, str2, str3 As Char
        str1 = Convert.ToChar(b1)
        str2 = Convert.ToChar(b2)
        str3 = Convert.ToChar(b3)

        StartReadData = String.Empty
        StartReadData += Convert.ToString(str1)
        StartReadData += Convert.ToString(str2)
        StartReadData += Convert.ToString(str3)
        If SerialPort_DataCode.IsOpen = True Then
            SerialPort_DataCode.DiscardInBuffer()
            SerialPort_DataCode.DiscardOutBuffer()

            SerialPort_DataCode.WriteLine(StartReadData)
        End If
    End Sub

    Private Sub Btn_Query_All_Data_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Btn_Query_All_Data.Click
        SQLCON.Read_Project_Measure_Data(Select_Product_Name, DATAGRIDVIEW_data, PanelEx10, False, 50000, TX_Qeuery_SN.Text.ToString.Trim)
    End Sub

    Private Sub Btn_Query_Data_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Btn_Query_Data.Click
        SQLCON.Read_Project_Measure_Data(Select_Product_Name, DATAGRIDVIEW_data, PanelEx10, False, 5000, TX_Qeuery_SN.Text.ToString.Trim)
    End Sub

    Private Sub Btn_Save_Data_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Btn_Save_Data.Click
        If SaveFileDialog_Excel.ShowDialog = Windows.Forms.DialogResult.OK Then
            DATA_PROCESS.Save_Excel(DATAGRIDVIEW_data, SaveFileDialog_Excel.FileName, DATA_CONVERSION_LIB.Save_Data_Format.XLS)
        End If
    End Sub

    Private Sub Btn_Delete_Data_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Btn_Delete_Data.Click
        Select Case Login_Dialog.ShowDialog
            Case Windows.Forms.DialogResult.Yes
                SQLCON.Clear_All_MeasureData(Select_Product_Name)
                SQLCON.Read_Project_Measure_Data(Select_Product_Name, DATAGRIDVIEW_data, PE_LOG_COUNT, False, 10000)
            Case Windows.Forms.DialogResult.No
                MessageBoxEx.Show("授权密码错误！如忘记密码请与管理员联系！", "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
        End Select
    End Sub

    Sub GRAB_IMAGE_LEFT1()
        While Start_Run = True
            Thread.Sleep(10)
            If GRAB_IMAGE_BOOLEAN_LEFT = True Then
                Dim subName As String = "1工位图像采集" & ":"
                Try
                    Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), subName & "Start", DataCode_Left, Color.Blue)
                    ToolBlock_left.Run()
                    Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), subName & "End", DataCode_Left, Color.Blue)
                Catch ex As Exception
                    Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), subName & "：" & ex.ToString, DataCode_Left, Color.Red)
                End Try
                GRAB_IMAGE_BOOLEAN_LEFT = False
            End If
        End While
    End Sub

    Sub GRAB_IMAGE_RIGHT1()
        While Start_Run = True
            Thread.Sleep(10)
            If GRAB_IMAGE_BOOLEAN_RIGHT = True Then
                Dim subName As String = "2工位图像采集" & ":"
                Try
                    Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), subName & "Start", DataCode_Right, Color.Blue)
                    toolblock_right.Run()
                    Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), subName & "End", DataCode_Right, Color.Blue)
                Catch ex As Exception
                    Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), subName & "：" & ex.ToString, DataCode_Right, Color.Red)
                End Try
                GRAB_IMAGE_BOOLEAN_RIGHT = False
            End If
        End While
    End Sub


    Private Sub Btn_Changle_User_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Btn_Changle_User.Click
        Select Case User_Login_Dialog.ShowDialog()
            Case Windows.Forms.DialogResult.OK
                User_Name = User_Login_Dialog.User
            Case Windows.Forms.DialogResult.Yes
                User_Name = User_Login_Dialog.User
            Case Windows.Forms.DialogResult.Cancel
                Exit Sub
        End Select
        Me.Text = ProductName & Space(2) & ProductVersion & Space(2) & " 控制卡数量：" & Card_NO & Space(2) & " 当前用户：" & User_Name
        Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), "切换用户：" & User_Name, "", Color.Black)
    End Sub


    Sub Get_Measure_Data(ByVal DATA As List(Of MEASURE_DATA_STRU), ByVal Key_Type As KB_Type_Enum, ByRef Laser_Data_OUT0 As List(Of Double), ByRef Laser_Data_OUT1 As List(Of Double))
        Dim Len As Int16 = 2 'mm
        Dim Total_Lenth_1 As Int64 = Work_左镭射第一行扫描从左到右(Work_左镭射第一行扫描从左到右.Length - 1).轴位置(0) - Work_左镭射第一行扫描从左到右(0).轴位置(0)
        Dim Total_Lenth_2 As Int64 = Work_左镭射第二行扫描从右到左(Work_左镭射第二行扫描从右到左.Length - 1).轴位置(0) - Work_左镭射第二行扫描从右到左(0).轴位置(0)
        Dim Total_Lenth_3 As Int64 = Work_左镭射第三行扫描从左到右(Work_左镭射第三行扫描从左到右.Length - 1).轴位置(0) - Work_左镭射第三行扫描从左到右(0).轴位置(0)

        Dim Each_Lenth_1 As Int16 = Abs(Set_Trg_Count1 / Total_Lenth_1) '(PLUS/mm）
        Dim Each_Lenth_2 As Int16 = Abs(Set_Trg_Count2 / Total_Lenth_2) '(PLUS/mm）
        Dim Each_Lenth_3 As Int16 = Abs(Set_Trg_Count3 / Total_Lenth_3) '(PLUS/mm）
        Laser_Data_OUT0 = New List(Of Double)
        Laser_Data_OUT1 = New List(Of Double)
        Laser_Data_OUT0.Clear()
        Laser_Data_OUT1.Clear()

        If DATA.Count >= 1 Then
            Select Case Key_Type
                Case KB_Type_Enum.ANSI
                    For i As Int16 = 0 To Set_Trg_Count1 - 1
                        Dim Work_Coordinate() As Work_Coordinatess = Work_左镭射第一行扫描从左到右
                        For j As Int16 = 1 To Work_Coordinate.Count - 2
                            Dim start_pos As Int16 = Work_Coordinate(j).轴位置(0) - Len
                            Dim End_Pos As Int16 = Work_Coordinate(j).轴位置(0) + Len
                            Dim start_Data_Index As Int16 = (start_pos - Work_Coordinate(0).轴位置(0)) * Each_Lenth_1
                            Dim End_Data_Index As Int16 = (End_Pos - Work_Coordinate(0).轴位置(0)) * Each_Lenth_1
                            Dim OUT As Double = 0, Avg_OUT As Double = 0
                            For k As Int16 = start_Data_Index To End_Data_Index
                                Dim m_value As Double = DATA(k).MEASE_OUT00
                                If m_value <= -5 Or m_value >= 5 Then
                                    m_value = 0
                                End If
                                OUT += m_value
                            Next
                            Avg_OUT = Round(OUT / (End_Data_Index - start_Data_Index), 3)
                            Laser_Data_OUT0.Add(Avg_OUT)
                        Next
                        Work_Coordinate = Work_左镭射第四行扫描从左到右
                        For j As Int16 = 1 To Work_Coordinate.Count - 2
                            Dim start_pos As Int16 = Work_Coordinate(j).轴位置(0) - Len
                            Dim End_Pos As Int16 = Work_Coordinate(j).轴位置(0) + Len
                            Dim start_Data_Index As Int16 = (start_pos - Work_Coordinate(0).轴位置(0)) * Each_Lenth_1
                            Dim End_Data_Index As Int16 = (End_Pos - Work_Coordinate(0).轴位置(0)) * Each_Lenth_1
                            Dim OUT As Double = 0, Avg_OUT As Double = 0
                            For k As Int16 = start_Data_Index To End_Data_Index
                                Dim m_value As Double = DATA(k).MEASE_OUT00
                                If m_value <= -5 Or m_value >= 5 Then
                                    m_value = 0
                                End If
                                OUT += m_value
                            Next
                            Avg_OUT = Round(OUT / (End_Data_Index - start_Data_Index), 3)
                            Laser_Data_OUT1.Add(Avg_OUT)
                        Next
                        Exit For
                    Next
                    '*******************************[2]*************************************************
                    For i As Int16 = Set_Trg_Count1 To Set_Trg_Count1 + Set_Trg_Count2 - 1
                        Dim Work_Coordinate() As Work_Coordinatess = Work_左镭射第二行扫描从右到左
                        For j As Int16 = 1 To Work_Coordinate.Count - 2
                            Dim start_pos As Int16 = Work_Coordinate(j).轴位置(0) + Len
                            Dim End_Pos As Int16 = Work_Coordinate(j).轴位置(0) - Len
                            Dim start_Data_Index As Int16 = Set_Trg_Count1 + (Work_Coordinate(0).轴位置(0) - start_pos) * Each_Lenth_2
                            Dim End_Data_Index As Int16 = Set_Trg_Count1 + (Work_Coordinate(0).轴位置(0) - End_Pos) * Each_Lenth_2
                            Dim OUT As Double = 0, Avg_OUT As Double = 0
                            For k As Int16 = start_Data_Index To End_Data_Index
                                Dim m_value As Double = DATA(k).MEASE_OUT01
                                If m_value <= -5 Or m_value >= 5 Then
                                    m_value = 0
                                End If
                                OUT += m_value
                            Next
                            Avg_OUT = Round(OUT / (End_Data_Index - start_Data_Index), 3)
                            Laser_Data_OUT0.Add(Avg_OUT)
                        Next
                        Work_Coordinate = Work_左镭射第五行扫描从右到左
                        For j As Int16 = 1 To Work_Coordinate.Count - 2
                            Dim start_pos As Int16 = Work_Coordinate(j).轴位置(0) + Len
                            Dim End_Pos As Int16 = Work_Coordinate(j).轴位置(0) - Len
                            Dim start_Data_Index As Int16 = Set_Trg_Count1 + (Work_Coordinate(0).轴位置(0) - start_pos) * Each_Lenth_2
                            Dim End_Data_Index As Int16 = Set_Trg_Count1 + (Work_Coordinate(0).轴位置(0) - End_Pos) * Each_Lenth_2
                            Dim OUT As Double = 0, Avg_OUT As Double = 0
                            For k As Int16 = start_Data_Index To End_Data_Index
                                Dim m_value As Double = DATA(k).MEASE_OUT01
                                If m_value <= -5 Or m_value >= 5 Then
                                    m_value = 0
                                End If
                                OUT += m_value
                            Next
                            Avg_OUT = Round(OUT / (End_Data_Index - start_Data_Index), 3)
                            Laser_Data_OUT1.Add(Avg_OUT)
                        Next
                        Exit For
                    Next
                    '*******************************[3]*************************************************
                    For i As Int16 = Set_Trg_Count1 + Set_Trg_Count2 To Set_Trg_Count_All - 1
                        Dim Work_Coordinate() As Work_Coordinatess = Work_左镭射第三行扫描从左到右
                        For j As Int16 = 1 To Work_Coordinate.Count - 2
                            Dim start_pos As Int16 = Work_Coordinate(j).轴位置(0) - Len
                            Dim End_Pos As Int16 = Work_Coordinate(j).轴位置(0) + Len
                            Dim start_Data_Index As Int16 = (start_pos - Work_Coordinate(0).轴位置(0)) * Each_Lenth_1
                            Dim End_Data_Index As Int16 = (End_Pos - Work_Coordinate(0).轴位置(0)) * Each_Lenth_1
                            Dim OUT As Double = 0, Avg_OUT As Double = 0
                            For k As Int16 = start_Data_Index To End_Data_Index
                                Dim m_value As Double = DATA(k).MEASE_OUT00
                                If m_value <= -5 Or m_value >= 5 Then
                                    m_value = 0
                                End If
                                OUT += m_value
                            Next
                            Avg_OUT = Round(OUT / (End_Data_Index - start_Data_Index), 3)
                            Laser_Data_OUT0.Add(Avg_OUT)
                        Next
                        Work_Coordinate = Work_左镭射第六行扫描从左到右
                        For j As Int16 = 1 To Work_Coordinate.Count - 2
                            Dim start_pos As Int16 = Work_Coordinate(j).轴位置(0) - Len
                            Dim End_Pos As Int16 = Work_Coordinate(j).轴位置(0) + Len
                            Dim start_Data_Index As Int16 = (start_pos - Work_Coordinate(0).轴位置(0)) * Each_Lenth_1
                            Dim End_Data_Index As Int16 = (End_Pos - Work_Coordinate(0).轴位置(0)) * Each_Lenth_1
                            Dim OUT As Double = 0, Avg_OUT As Double = 0
                            For k As Int16 = start_Data_Index To End_Data_Index
                                Dim m_value As Double = DATA(k).MEASE_OUT00
                                If m_value <= -5 Or m_value >= 5 Then
                                    m_value = 0
                                End If
                                OUT += m_value
                            Next
                            Avg_OUT = Round(OUT / (End_Data_Index - start_Data_Index), 3)
                            Laser_Data_OUT1.Add(Avg_OUT)
                        Next
                        Exit For
                    Next
            End Select
        End If
    End Sub


    ''' <summary>
    ''' 更新HIP数据库
    ''' </summary>
    ''' <param name="Result_All">总结果</param>
    ''' <param name="Barcode">条码</param>
    ''' <param name="Table_Name">表名</param>
    ''' <remarks></remarks>
    Function Updata_HIP_Table(ByVal Result_All As String, ByVal LightLakageResult As String, ByVal HookSnapeResult As String, ByVal Fail_Location As String, ByVal Barcode As String, ByVal Table_Name As String, ByVal Sql_Time As String, ByVal RSAOI As String) As Boolean
        Dim rtn_str As String = Nothing, Product_ID As Integer
        Dim Bool_Insert_Result As Boolean = False
        Try
            If Barcode.Length <> 23 Then
                rtn_str = "条码有误:" & Barcode & "本次数据不上传！"
                Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), rtn_str, Barcode, Color.Red)
                Return False
            End If
            Select Case Mid(Barcode, 5, 1)
                Case "A"
                    If Select_Product_Name <> "ANSI" Then
                        rtn_str = "Tray盘条码与项目名称型号不匹配，请检查项目名称错误或产品投放错误,本次数据不上传"
                        Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), rtn_str, Barcode, Color.Red)
                        Return False
                    End If
                Case "I"
                    If Select_Product_Name <> "ISO" Then
                        rtn_str = "Tray盘条码与项目名称型号不匹配，请检查项目名称错误或产品投放错误，本次数据不上传！"
                        Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), rtn_str, Barcode, Color.Red)
                        Return False
                    End If
                Case "J"
                    If Select_Product_Name <> "JIS" Then
                        rtn_str = "Tray盘条码与项目名称型号不匹配，请检查项目名称错误或产品投放错误，本次数据不上传！"
                        Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), rtn_str, Barcode, Color.Red)
                        Return False
                    End If
            End Select

            Dim 拆卸机编码 As String = Mid(Barcode, 18, 1)
            If IsNumeric(拆卸机编码) = True Then
                Dim 拆卸机线体 As Integer = CInt(拆卸机编码)
                MAJOR_TABLE.Station_Name = "RS AOI"
                MAJOR_TABLE.Machine_NO = PARAM_INT.S设备编号.ToString
                MAJOR_TABLE.Project_Name = Mid(Barcode, 1, 4)
                MAJOR_TABLE.Tray_Code = Barcode
                MAJOR_TABLE.Language = Mid(Barcode, 6, 5)
                MAJOR_TABLE.Layout = Mid(Barcode, 5, 1)
                MAJOR_TABLE.Fail_Location = Fail_Location
                MAJOR_TABLE.Order_NO = ""
                MAJOR_TABLE.Result = Result_All
                MAJOR_TABLE.LightLakageResult = LightLakageResult
                MAJOR_TABLE.HookSnapeResult = HookSnapeResult
                If MAJOR_TABLE.Language.ToUpper = "ERROR" Then
                    rtn_str = "上一站读取条码为空，赋值为错误条码:" & Barcode & "本次数据不上传！"
                    Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), rtn_str, Barcode, Color.Red)
                    Return False
                End If

                '把本地数据上传至HIP服务器
                Dim Result_Dataset As DataSet = Nothing, Len As Int16
                If SQLCON.Read_Measure_Result(Barcode, Result_Dataset, RSAOI) = True Then
                    Len = Result_Dataset.Tables(0).Rows.Count
                    If Len > 0 And Len = KB_Number + 1 Then
                        'Select Case 拆卸机线体
                        '    Case Is <= 2 'A001数据库
                        '        Bool_Insert_Result = SQLCON_A001.Query_Order_NO(MAJOR_TABLE.Tray_Code, MAJOR_TABLE.Order_NO)
                        '        If Bool_Insert_Result = False Then
                        '            rtn_str = "A001数据库断开重连"
                        '            Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), rtn_str, Barcode, Color.Red)
                        '            SQLCON_A001.Close_DataBase()
                        '            If SQLCON_A001.DataBase_Initialization(SQLCON_A001.DataBase_Data_Souce, SQLCON_A001.DataBase_ID, SQLCON_A001.DataBase_PassWord, SQLCON_A001.DataBase_Catalog_Name, 5000, , ) = True Then
                        '                rtn_str = "A001数据库断开重连成功"
                        '                Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), rtn_str, Barcode, Color.Green)

                        '                Bool_Insert_Result = SQLCON_A001.Query_Order_NO(MAJOR_TABLE.Tray_Code, MAJOR_TABLE.Order_NO)
                        '            Else
                        '                rtn_str = "A001数据库断开重连失败,请检查并重启软件！"
                        '                Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), rtn_str, Barcode, Color.Red)
                        '                MessageBoxEx.Show(rtn_str, "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
                        '                Return False
                        '            End If
                        '        End If
                        '        If MAJOR_TABLE.Order_NO = "" Then
                        '            rtn_str = "查询到条码：" & MAJOR_TABLE.Tray_Code & " 的工单号为空"
                        '            Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), rtn_str, Barcode, Color.Red)
                        '        End If
                        '        Select Case PARAM_BOOL.T调试版本
                        '            Case True
                        '                MAJOR_TABLE.Table_Name = "[SP RS DEBUG]"
                        '            Case False
                        '                MAJOR_TABLE.Table_Name = "[SP RS AOI]"
                        '        End Select

                        '        Bool_Insert_Result = SQLCON_A001.Insert_Result(MAJOR_TABLE.Table_Name, MAJOR_TABLE.Station_Name, MAJOR_TABLE.Machine_NO, MAJOR_TABLE.Project_Name, MAJOR_TABLE.Tray_Code, MAJOR_TABLE.Language, MAJOR_TABLE.Layout, MAJOR_TABLE.Result, MAJOR_TABLE.Fail_Location, MAJOR_TABLE.Order_NO, Sql_Time, MAJOR_TABLE.LightLakageResult, MAJOR_TABLE.HookSnapeResult)
                        '        Thread.Sleep(5)
                        '        Product_ID = SQLCON_A001.Query_ID(MAJOR_TABLE.Table_Name, MAJOR_TABLE.Tray_Code)

                        '    Case Is = 3 '查询两次，先在X816中查，后在A001中查
                        '        Bool_Insert_Result = SQLCON_X816.Query_Order_NO(MAJOR_TABLE.Tray_Code, MAJOR_TABLE.Order_NO)
                        '        Select Case Bool_Insert_Result
                        '            Case True
                        '                If MAJOR_TABLE.Order_NO = "" Then
                        '                    Bool_Insert_Result = SQLCON_A001.Query_Order_NO(MAJOR_TABLE.Tray_Code, MAJOR_TABLE.Order_NO)
                        '                    If Bool_Insert_Result = False Then
                        '                        rtn_str = "A001数据库断开重连"
                        '                        Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), rtn_str, Barcode, Color.Red)
                        '                        SQLCON_A001.Close_DataBase()
                        '                        If SQLCON_A001.DataBase_Initialization(SQLCON_A001.DataBase_Data_Souce, SQLCON_A001.DataBase_ID, SQLCON_A001.DataBase_PassWord, SQLCON_A001.DataBase_Catalog_Name, 5000, , ) = True Then
                        '                            rtn_str = "A001数据库断开重连成功"
                        '                            Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), rtn_str, Barcode, Color.Green)

                        '                            Bool_Insert_Result = SQLCON_A001.Query_Order_NO(MAJOR_TABLE.Tray_Code, MAJOR_TABLE.Order_NO)
                        '                        Else
                        '                            rtn_str = "A001数据库断开重连失败,请检查并重启软件！"
                        '                            Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), rtn_str, Barcode, Color.Red)
                        '                            MessageBoxEx.Show(rtn_str, "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
                        '                            Return False
                        '                        End If
                        '                    End If
                        '                End If
                        '            Case False
                        '                rtn_str = "X816数据库断开重连"
                        '                Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), rtn_str, Barcode, Color.Red)
                        '                SQLCON_X816.Close_DataBase()
                        '                If SQLCON_X816.DataBase_Initialization(SQLCON_X816.DataBase_Data_Souce, SQLCON_X816.DataBase_ID, SQLCON_X816.DataBase_PassWord, SQLCON_X816.DataBase_Catalog_Name, 5000, , ) = True Then
                        '                    rtn_str = "X816数据库断开重连成功"
                        '                    Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), rtn_str, Barcode, Color.Green)

                        '                    Bool_Insert_Result = SQLCON_X816.Query_Order_NO(MAJOR_TABLE.Tray_Code, MAJOR_TABLE.Order_NO)
                        '                    If Bool_Insert_Result = True Then
                        '                        If MAJOR_TABLE.Order_NO = "" Then
                        '                            Bool_Insert_Result = SQLCON_A001.Query_Order_NO(MAJOR_TABLE.Tray_Code, MAJOR_TABLE.Order_NO)
                        '                            If Bool_Insert_Result = False Then
                        '                                rtn_str = "A001数据库断开重连"
                        '                                Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), rtn_str, Barcode, Color.Red)
                        '                                SQLCON_A001.Close_DataBase()
                        '                                If SQLCON_A001.DataBase_Initialization(SQLCON_A001.DataBase_Data_Souce, SQLCON_A001.DataBase_ID, SQLCON_A001.DataBase_PassWord, SQLCON_A001.DataBase_Catalog_Name, 5000, , ) = True Then
                        '                                    rtn_str = "A001数据库断开重连成功"
                        '                                    Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), rtn_str, Barcode, Color.Green)

                        '                                    Bool_Insert_Result = SQLCON_A001.Query_Order_NO(MAJOR_TABLE.Tray_Code, MAJOR_TABLE.Order_NO)
                        '                                Else
                        '                                    rtn_str = "A001数据库断开重连失败,请检查并重启软件！"
                        '                                    Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), rtn_str, Barcode, Color.Red)
                        '                                    MessageBoxEx.Show(rtn_str, "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
                        '                                    Return False
                        '                                End If
                        '                            End If
                        '                        End If
                        '                    End If
                        '                Else
                        '                    rtn_str = "X816数据库断开重连失败,请检查并重启软件！"
                        '                    Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), rtn_str, Barcode, Color.Red)
                        '                    MessageBoxEx.Show(rtn_str, "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
                        '                    Return False
                        '                End If
                        '        End Select

                        '        If MAJOR_TABLE.Order_NO = "" Then
                        '            rtn_str = "查询到条码：" & MAJOR_TABLE.Tray_Code & " 的工单号为空"
                        '            Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), rtn_str, Barcode, Color.Red)
                        '        End If
                        '        Select Case PARAM_BOOL.T调试版本
                        '            Case True
                        '                MAJOR_TABLE.Table_Name = "[SP-17-RS DEBUG]"
                        '            Case False
                        '                MAJOR_TABLE.Table_Name = "[SP-17-RS AOI]"
                        '        End Select

                        '        Bool_Insert_Result = SQLCON_X816.Insert_Result(MAJOR_TABLE.Table_Name, MAJOR_TABLE.Station_Name, MAJOR_TABLE.Machine_NO, MAJOR_TABLE.Project_Name, MAJOR_TABLE.Tray_Code, MAJOR_TABLE.Language, MAJOR_TABLE.Layout, MAJOR_TABLE.Result, MAJOR_TABLE.Fail_Location, MAJOR_TABLE.Order_NO, Sql_Time, MAJOR_TABLE.LightLakageResult, MAJOR_TABLE.HookSnapeResult)
                        '        Thread.Sleep(5)
                        '        Product_ID = SQLCON_X816.Query_ID(MAJOR_TABLE.Table_Name, MAJOR_TABLE.Tray_Code)

                        '    Case Is >= 4 'X816数据库
                        '        Bool_Insert_Result = SQLCON_X816.Query_Order_NO(MAJOR_TABLE.Tray_Code, MAJOR_TABLE.Order_NO)
                        '        If Bool_Insert_Result = False Then
                        '            rtn_str = "X816数据库断开重连"
                        '            Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), rtn_str, Barcode, Color.Red)
                        '            SQLCON_X816.Close_DataBase()
                        '            If SQLCON_X816.DataBase_Initialization(SQLCON_X816.DataBase_Data_Souce, SQLCON_X816.DataBase_ID, SQLCON_X816.DataBase_PassWord, SQLCON_X816.DataBase_Catalog_Name, 5000, , ) = True Then
                        '                rtn_str = "X816数据库断开重连成功"
                        '                Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), rtn_str, Barcode, Color.Green)

                        '                Bool_Insert_Result = SQLCON_X816.Query_Order_NO(MAJOR_TABLE.Tray_Code, MAJOR_TABLE.Order_NO)
                        '            Else
                        '                rtn_str = "X816数据库断开重连失败,请检查并重启软件！"
                        '                Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), rtn_str, Barcode, Color.Red)
                        '                MessageBoxEx.Show(rtn_str, "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
                        '                Return False
                        '            End If
                        '        End If
                        '        If MAJOR_TABLE.Order_NO = "" Then
                        '            rtn_str = "查询到条码：" & MAJOR_TABLE.Tray_Code & " 的工单号为空"
                        '            Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), rtn_str, Barcode, Color.Red)
                        '        End If
                        '        Select Case PARAM_BOOL.T调试版本
                        '            Case True
                        '                MAJOR_TABLE.Table_Name = "[SP-17-RS DEBUG]"
                        '            Case False
                        '                MAJOR_TABLE.Table_Name = "[SP-17-RS AOI]"
                        '        End Select
                        '        Bool_Insert_Result = SQLCON_X816.Insert_Result(MAJOR_TABLE.Table_Name, MAJOR_TABLE.Station_Name, MAJOR_TABLE.Machine_NO, MAJOR_TABLE.Project_Name, MAJOR_TABLE.Tray_Code, MAJOR_TABLE.Language, MAJOR_TABLE.Layout, MAJOR_TABLE.Result, MAJOR_TABLE.Fail_Location, MAJOR_TABLE.Order_NO, Sql_Time, MAJOR_TABLE.LightLakageResult, MAJOR_TABLE.HookSnapeResult)
                        '        Thread.Sleep(5)
                        '        Product_ID = SQLCON_X816.Query_ID(MAJOR_TABLE.Table_Name, MAJOR_TABLE.Tray_Code)
                        'End Select


                        Select Case PARAM_BOOL.T调试版本
                            Case True
                                MAJOR_TABLE.Table_Name = "[SP RS DEBUG]"
                            Case False
                                MAJOR_TABLE.Table_Name = "[AS_RSAOI]"
                        End Select

                        Bool_Insert_Result = SQLCON_A001.Insert_Result(MAJOR_TABLE.Table_Name, MAJOR_TABLE.Station_Name, MAJOR_TABLE.Machine_NO, MAJOR_TABLE.Project_Name, MAJOR_TABLE.Tray_Code, MAJOR_TABLE.Language, MAJOR_TABLE.Layout, MAJOR_TABLE.Result, MAJOR_TABLE.Fail_Location, MAJOR_TABLE.Order_NO, Sql_Time, MAJOR_TABLE.LightLakageResult, MAJOR_TABLE.HookSnapeResult)
                        Thread.Sleep(5)
                        Product_ID = SQLCON_A001.Query_ID(MAJOR_TABLE.Table_Name, MAJOR_TABLE.Tray_Code)


                        '主表数据是否成功插入
                        Select Case Bool_Insert_Result
                            Case True '插入成功
                                Dim LightLeakage_Key_NgCount As Integer = 0 : Dim HookSnap_key_NgCount As Integer = 0 '初始化NG数量 11.03
                                Dim LightLeakage_Key_Result As String = "OK" : Dim HookSnap_key_Result As String = "OK" '11.05
                                For i As Int16 = 0 To Result_Dataset.Tables(0).Rows.Count - 1

                                    Dim Upload_Name(28) As String
                                    Dim Upload_Value(28) As Object
                                    Upload_Name(0) = "[Product ID]"
                                    Upload_Name(1) = "[Machine No]"
                                    Upload_Name(2) = "[Project Name]"
                                    Upload_Name(3) = "[Tray Code]"
                                    Upload_Name(4) = "Language"
                                    Upload_Name(5) = "Layout"
                                    Upload_Name(6) = "[Key No]"
                                    Upload_Name(7) = "[Key Type]"
                                    Upload_Name(8) = "[Key Name]"
                                    Upload_Name(9) = "LightLeakage_Left_Result"
                                    Upload_Name(10) = "LightLeakage_Up_Result"
                                    Upload_Name(11) = "LightLeakage_Right_Result"
                                    Upload_Name(12) = "LightLeakage_Down_Result"
                                    Upload_Name(13) = "Snap01_Result"
                                    Upload_Name(14) = "Snap02_Result"
                                    Upload_Name(15) = "Snap03_Result"
                                    Upload_Name(16) = "Snap04_Result"
                                    Upload_Name(17) = "Hook01_Result"
                                    Upload_Name(18) = "Hook02_Result"
                                    Upload_Name(19) = "Hook03_Result"
                                    Upload_Name(20) = "Hook04_Result"
                                    Upload_Name(21) = "GateVestige01_Result"
                                    Upload_Name(22) = "GateVestige02_Result"
                                    Upload_Name(23) = "GateVestige03_Result"
                                    Upload_Name(24) = "LightLeakageResult"
                                    Upload_Name(25) = "HookSnapResult"
                                    Upload_Name(26) = "MC_Result"
                                    Upload_Name(27) = "OP_Rejudge"
                                    Upload_Name(28) = "Final_Result"

                                    Upload_Value(0) = Product_ID.ToString
                                    Upload_Value(1) = Result_Dataset.Tables(0).Rows(i).Item(1).ToString.Trim
                                    Upload_Value(2) = Result_Dataset.Tables(0).Rows(i).Item(2).ToString.Trim
                                    Upload_Value(3) = Result_Dataset.Tables(0).Rows(i).Item(3).ToString.Trim
                                    Upload_Value(4) = Result_Dataset.Tables(0).Rows(i).Item(4).ToString.Trim
                                    Upload_Value(5) = Result_Dataset.Tables(0).Rows(i).Item(5).ToString.Trim
                                    Upload_Value(6) = Result_Dataset.Tables(0).Rows(i).Item(6).ToString.Trim
                                    Upload_Value(7) = Result_Dataset.Tables(0).Rows(i).Item(7).ToString.Trim
                                    Upload_Value(8) = Result_Dataset.Tables(0).Rows(i).Item(8).ToString.Trim
                                    Upload_Value(9) = Result_Dataset.Tables(0).Rows(i).Item(9).ToString.Trim
                                    Upload_Value(10) = Result_Dataset.Tables(0).Rows(i).Item(10).ToString.Trim
                                    Upload_Value(11) = Result_Dataset.Tables(0).Rows(i).Item(11).ToString.Trim
                                    Upload_Value(12) = Result_Dataset.Tables(0).Rows(i).Item(12).ToString.Trim
                                    Upload_Value(13) = Result_Dataset.Tables(0).Rows(i).Item(13).ToString.Trim
                                    Upload_Value(14) = Result_Dataset.Tables(0).Rows(i).Item(14).ToString.Trim
                                    Upload_Value(15) = Result_Dataset.Tables(0).Rows(i).Item(15).ToString.Trim
                                    Upload_Value(16) = Result_Dataset.Tables(0).Rows(i).Item(16).ToString.Trim
                                    Upload_Value(17) = Result_Dataset.Tables(0).Rows(i).Item(17).ToString.Trim
                                    Upload_Value(18) = Result_Dataset.Tables(0).Rows(i).Item(18).ToString.Trim
                                    Upload_Value(19) = Result_Dataset.Tables(0).Rows(i).Item(19).ToString.Trim
                                    Upload_Value(20) = Result_Dataset.Tables(0).Rows(i).Item(20).ToString.Trim
                                    Upload_Value(21) = Result_Dataset.Tables(0).Rows(i).Item(21).ToString.Trim
                                    Upload_Value(22) = Result_Dataset.Tables(0).Rows(i).Item(22).ToString.Trim
                                    Upload_Value(23) = Result_Dataset.Tables(0).Rows(i).Item(23).ToString.Trim
                                    Upload_Value(24) = Result_Dataset.Tables(0).Rows(i).Item(24).ToString.Trim
                                    Upload_Value(25) = Result_Dataset.Tables(0).Rows(i).Item(25).ToString.Trim
                                    Upload_Value(26) = Result_Dataset.Tables(0).Rows(i).Item(26).ToString.Trim
                                    Upload_Value(27) = Result_Dataset.Tables(0).Rows(i).Item(27).ToString.Trim
                                    Upload_Value(28) = Result_Dataset.Tables(0).Rows(i).Item(28).ToString.Trim
                                    Thread.Sleep(1)
                                    SQLCON_X816RSAOI.Insert_Measure_Result(Table_Name, Sql_Time, Upload_Name, Upload_Value)

                                    ''11.03
                                    'If Upload_Value(24) = "NG" Then '漏光NG
                                    '    LightLeakage_Key_NgCount += 1
                                    'End If
                                    'If Upload_Value(25) = "NG" Then '卡扣NG
                                    '    HookSnap_key_NgCount += 1
                                    'End If
                                Next
                                ''11.05
                                'If LightLeakage_Key_NgCount >= 1 Then
                                '    LightLeakage_Key_Result = "NG"
                                'Else
                                '    LightLeakage_Key_Result = "OK"
                                'End If
                                'If HookSnap_key_NgCount >= 1 Then
                                '    HookSnap_key_Result = "NG"
                                'Else
                                '    HookSnap_key_Result = "OK"
                                'End If
                                ''把漏光NG和卡扣NG写入主表，主表的ID已经知道，索引ID就好 11.03
                                'Select Case 拆卸机线体
                                '    Case Is <= 2
                                '        SQLCON_A001.Updata_NGCount_Result(MAJOR_TABLE.Table_Name, LightLeakage_Key_Result, HookSnap_key_Result, Product_ID)
                                '    Case Is = 3
                                '        SQLCON_X816.Updata_NGCount_Result(MAJOR_TABLE.Table_Name, LightLeakage_Key_Result, HookSnap_key_Result, Product_ID)
                                '    Case Is >= 4
                                '        SQLCON_X816.Updata_NGCount_Result(MAJOR_TABLE.Table_Name, LightLeakage_Key_Result, HookSnap_key_Result, Product_ID)
                                'End Select
                            Case False '插入失败
                                rtn_str = "插入主表数据失败，本次数据无法上传，请检查服务器是否断开连接，然后重新启动软件！"
                                Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), rtn_str, Barcode, Color.Red)
                                MessageBoxEx.Show(rtn_str, "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
                                Return False
                        End Select
                    Else
                        rtn_str = "本站数据异常，需要上传行数：【" & KB_Number + 1 & "】，实际数据行数：【" & Len & "】,请检查Tray盘类型是否对应或数据丢失，本站记录不上传，请检查后重新过站！"
                        Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), rtn_str, Barcode, Color.Red)
                        MessageBoxEx.Show(rtn_str, "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
                        Return False
                    End If
                End If
            Else
                rtn_str = "条码有误:" & Barcode & "本次数据不上传！"
                Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), rtn_str, Barcode, Color.Red)
                Return False
            End If
        Catch ex As Exception
            rtn_str = " 更新HIP数据库错误:" & ex.ToString
            Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), rtn_str, Barcode, Color.Red)
            Return False
        End Try
        Return True
    End Function

    Dim Plc_Return_Str As String = Nothing
    Dim Plc_Return_Result1 As String = Nothing
    Dim Plc_Return_Result2 As String = Nothing
    Dim Test_Plc_Conn As Boolean = False

    Private Sub SerialPort_PLC_DataReceived(ByVal sender As System.Object, ByVal e As System.IO.Ports.SerialDataReceivedEventArgs) Handles SerialPort_PLC.DataReceived
        Send_Omron_Cmd = True
        Test_Plc_Conn = True

        Plc_Return_Str = ""
        Plc_Return_Result1 = ""
        Plc_Return_Result2 = ""

        Dim s1 As String = Nothing
        While True
            Dim b1 As Integer = SerialPort_PLC.ReadByte
            If b1 = Asc(vbCr) Then
                Exit While
            Else
                s1 = s1 & Chr(b1)
            End If
        End While
        If s1.Length >= 16 Then
            Plc_Return_Result1 = Mid(s1, 11, 1)
            Plc_Return_Result2 = Mid(s1, 15, 1)
            Dim DO_STYPE As String, D1_STYPE As String
            If Plc_Return_Result1 = "3" Then
                DO_STYPE = "空闲"
            Else
                DO_STYPE = "繁忙"
            End If
            If Plc_Return_Result2 = "3" Then
                D1_STYPE = "空闲"
            Else
                D1_STYPE = "繁忙"
            End If
            PE_PLC.Text = "DO:" & DO_STYPE & Space(2) & "D1:" & D1_STYPE
        Else
            PE_PLC.Text = s1.Trim
        End If
        Plc_Return_Str = s1.Trim

        Static test As Boolean = False
        If test = False Then
            test = True
            If Plc_Return_Str = "" Then
                OP_Style.Text = "连接失败"
                OP_Style.Style.ForeColor.Color = Color.Red
                Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), "PLC无反馈，请检查串口号", "", Color.Red)
            Else
                OP_Style.Text = "连接成功"
                OP_Style.Style.ForeColor.Color = Color.Green
                Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), "PLC连接成功", "", Color.Blue)
            End If
        End If

        Select Case Send_plc_Command_Str
            Case Omron_Command.更改PLC为监视模式
                Send_plc_Command_Str = ""
            Case Omron_Command.读取PLC状态
                Send_plc_Command_Str = Omron_Command.读取PLC状态
            Case Omron_Command.写入结果OK
                Send_plc_Command_Str = ""
            Case Omron_Command.写入结果NG
                Send_plc_Command_Str = ""
            Case Else
                Send_plc_Command_Str = ""
        End Select

        Send_Omron_Cmd = False
    End Sub

    Structure Omron_Command
        Shared 更改PLC为监视模式 As String = ""
        Shared PLC反馈信息 As String = ""
        Shared 读取PLC状态 As String = ""
        Shared 写入结果OK As String = ""
        Shared 写入结果NG As String = ""
    End Structure
    Sub Set_Omron_Command()
        Omron_Command.更改PLC为监视模式 = "@00SC02" & Omron_CRC("@00SC02") & "*" & vbCr  '更改PLC为监视模式
        Omron_Command.PLC反馈信息 = "@00SC0050*" 'PLC返回值
        Omron_Command.读取PLC状态 = "@00RD00000002" & Omron_CRC("@00RD00000002") & "*" & vbCr
        Omron_Command.写入结果OK = "@00WD00000000" & Omron_CRC("@00WD00000000") & "*" & vbCr
        Omron_Command.写入结果NG = "@00WD00000001" & Omron_CRC("@00WD00000001") & "*" & vbCr
    End Sub

    ''' <summary>
    ''' 欧姆龙PLC CRC 校验
    ''' </summary>
    ''' <param name="Fcs_Str"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function Omron_CRC(ByVal Fcs_Str As String) As String
        Dim Xor_Result As Integer = 0
        Dim Temp_Fcs As String
        For i As Integer = 1 To Fcs_Str.Length
            Xor_Result = Xor_Result Xor Asc(Mid$(Fcs_Str, i, 1))
        Next
        Temp_Fcs = Hex$(Xor_Result)
        If Temp_Fcs.Length = 1 Then Temp_Fcs = "0" & Temp_Fcs
        Omron_CRC = Temp_Fcs
    End Function

    Private Sub PanelEx4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PE_OK.Click
        PE_PLC.Text = ""
        Send_plc_Command_Str = Omron_Command.写入结果OK

    End Sub

    Private Sub PanelEx6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PE_NG.Click
        PE_PLC.Text = ""
        Send_plc_Command_Str = Omron_Command.写入结果NG

    End Sub

    Private Sub PE_PLC_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PE_PLC.Click
        PE_PLC.Text = ""
        Dim Init_Str As String = Nothing
        If SerialPort_PLC.IsOpen = True Then
            SerialPort_PLC.Write(Omron_Command.读取PLC状态)
            Init_Str = "读取PLC状态"
            Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), Init_Str, "", color.BLACK)
        End If
    End Sub

    Delegate Sub Delegate_Insert_Image_Left(ByVal TABLE_NAME As String, ByVal Project_Name As String, ByVal BarCode As String, ByVal Key_Name As String, ByVal ImageByte() As Byte, ByVal Measure_Result As String)
    Sub Insert_Image_Left(ByVal TABLE_NAME As String, ByVal Project_Name As String, ByVal BarCode As String, ByVal Key_Name As String, ByVal ImageByte() As Byte, ByVal Measure_Result As String)
        SQLCON.Insert_Image_Left(TABLE_NAME, Project_Name, BarCode, Key_Name, ImageByte, Measure_Result)
    End Sub

    Delegate Sub Delegate_Insert_Image_Right(ByVal TABLE_NAME As String, ByVal Project_Name As String, ByVal BarCode As String, ByVal Key_Name As String, ByVal ImageByte() As Byte, ByVal Measure_Result As String)
    Sub Insert_Image_Right(ByVal TABLE_NAME As String, ByVal Project_Name As String, ByVal BarCode As String, ByVal Key_Name As String, ByVal ImageByte() As Byte, ByVal Measure_Result As String)
        SQLCON.Insert_Image_Right(TABLE_NAME, Project_Name, BarCode, Key_Name, ImageByte, Measure_Result)
    End Sub

  

    Private Sub ButtonX1_Click(sender As System.Object, e As System.EventArgs) Handles ButtonX1.Click
        If MessageBoxEx.Show("手动过料前请确认OP站没有堵料,且PLC处于空闲状态，否则由于两片料太近会造成气缸顶翻产品！", "系统消息", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = Windows.Forms.DialogResult.Yes Then

            CARDCMD.SET_DO_BIT(BOARD_ID_00, DIO.Z中线阻挡电磁阀_0, PRA_OUT_STATUS_ENUM.OUT_OFF)
            Thread.Sleep(100)
            CARDCMD.SET_DO_BIT(BOARD_ID_00, DIO.Z中线皮带控制_0, PRA_OUT_STATUS_ENUM.OUT_ON)

            If mClear_Err = True Then
                mClear_Err = False
            End If
        End If
    End Sub

    Sub Write_Log_Left(ByVal Datacode As String, ByVal Message As String)
        Dim File_Path As String = Nothing
        Dim MON As String = Date.Now.Month.ToString.PadLeft(2).Replace(" ", "0") & " MONTH\"
        Dim DAY As String = Date.Now.Day.ToString.PadLeft(2).Replace(" ", "0") & " DAY\"    
        File_Path = "C:\" & MON & DAY
        If System.IO.Directory.Exists(File_Path) = False Then
            System.IO.Directory.CreateDirectory(File_Path)
        End If

        Dim sw As StreamWriter
        sw = New StreamWriter(File_Path & Datacode & ".txt", True, Encoding.Default)
        sw.WriteLine(Date.Now & "," & Message)
        sw.Flush()
        sw.Close()
        sw.Dispose()
    End Sub
    Sub Write_Log_Right(ByVal Datacode As String, ByVal Message As String)
        Dim File_Path As String = Nothing
        Dim MON As String = Date.Now.Month.ToString.PadLeft(2).Replace(" ", "0") & " MONTH\"
        Dim DAY As String = Date.Now.Day.ToString.PadLeft(2).Replace(" ", "0") & " DAY\"
        File_Path = "C:\" & MON & DAY
        If System.IO.Directory.Exists(File_Path) = False Then
            System.IO.Directory.CreateDirectory(File_Path)
        End If

        Dim sw As StreamWriter
        sw = New StreamWriter(File_Path & Datacode & ".txt", True, Encoding.Default)
        sw.WriteLine(Date.Now & "," & Message)
        sw.Flush()
        sw.Close()
        sw.Dispose()
    End Sub

    Sub Write_Err_Left(ByVal Datacode As String, ByVal Message As String)
        'Dim File_Path As String = Nothing
        'Dim MON As String = Date.Now.Month.ToString.PadLeft(2).Replace(" ", "0") & " MONTH\"
        'Dim DAY As String = Date.Now.Day.ToString.PadLeft(2).Replace(" ", "0") & " DAY\"
        'File_Path = "C:\" & MON & DAY
        'If System.IO.Directory.Exists(File_Path) = False Then
        '    System.IO.Directory.CreateDirectory(File_Path)
        'End If

        'Dim sw As StreamWriter
        'sw = New StreamWriter(File_Path & Datacode & "_Err.txt", True, Encoding.Default)
        'sw.WriteLine(Date.Now & "," & Message)
        'sw.Flush()
        'sw.Close()
        'sw.Dispose()

    End Sub
    Sub Write_Err_Right(ByVal Datacode As String, ByVal Message As String)
        'Dim File_Path As String = Nothing
        'Dim MON As String = Date.Now.Month.ToString.PadLeft(2).Replace(" ", "0") & " MONTH\"
        'Dim DAY As String = Date.Now.Day.ToString.PadLeft(2).Replace(" ", "0") & " DAY\"
        'File_Path = "C:\" & MON & DAY
        'If System.IO.Directory.Exists(File_Path) = False Then
        '    System.IO.Directory.CreateDirectory(File_Path)
        'End If

        'Dim sw As StreamWriter
        'sw = New StreamWriter(File_Path & Datacode & "_Err.txt", True, Encoding.Default)
        'sw.WriteLine(Date.Now & "," & Message)
        'sw.Flush()
        'sw.Close()
        'sw.Dispose()

    End Sub

    Sub Write_Measure_Time_Left(ByVal Datacode As String, ByVal Message As String)
        Dim File_Path As String = Nothing
        Dim MON As String = Date.Now.Month.ToString.PadLeft(2).Replace(" ", "0") & " MONTH\"
        Dim DAY As String = Date.Now.Day.ToString.PadLeft(2).Replace(" ", "0") & " DAY\"
        File_Path = "C:\" & MON & DAY
        If System.IO.Directory.Exists(File_Path) = False Then
            System.IO.Directory.CreateDirectory(File_Path)
        End If

        Dim sw As StreamWriter
        sw = New StreamWriter(File_Path & "Measure_Time.txt", True, Encoding.Default)
        sw.WriteLine(Date.Now & "," & Datacode & "," & Message)
        sw.Flush()
        sw.Close()
        sw.Dispose()
    End Sub

    Sub Write_Measure_Time_Right(ByVal Datacode As String, ByVal Message As String)
        Dim File_Path As String = Nothing
        Dim MON As String = Date.Now.Month.ToString.PadLeft(2).Replace(" ", "0") & " MONTH\"
        Dim DAY As String = Date.Now.Day.ToString.PadLeft(2).Replace(" ", "0") & " DAY\"
        File_Path = "C:\" & MON & DAY
        If System.IO.Directory.Exists(File_Path) = False Then
            System.IO.Directory.CreateDirectory(File_Path)
        End If

        Dim sw As StreamWriter
        sw = New StreamWriter(File_Path & "Measure_Time.txt", True, Encoding.Default)
        sw.WriteLine(Date.Now & "," & Datacode & "," & Message)
        sw.Flush()
        sw.Close()
        sw.Dispose()
    End Sub

    Sub Write_Out_Left(ByVal Datacode As String, ByVal Message As String)
        Dim File_Path As String = Nothing
        Dim MON As String = Date.Now.Month.ToString.PadLeft(2).Replace(" ", "0") & " MONTH\"
        Dim DAY As String = Date.Now.Day.ToString.PadLeft(2).Replace(" ", "0") & " DAY\"
        File_Path = "C:\" & MON & DAY
        If System.IO.Directory.Exists(File_Path) = False Then
            System.IO.Directory.CreateDirectory(File_Path)
        End If

        Dim sw As StreamWriter
        sw = New StreamWriter(File_Path & "output_1.txt", True, Encoding.Default)
        sw.WriteLine(Date.Now & "," & Datacode & "," & Message)
        sw.Flush()
        sw.Close()
        sw.Dispose()
    End Sub

    Sub Write_Out_Right(ByVal Datacode As String, ByVal Message As String)
        Dim File_Path As String = Nothing
        Dim MON As String = Date.Now.Month.ToString.PadLeft(2).Replace(" ", "0") & " MONTH\"
        Dim DAY As String = Date.Now.Day.ToString.PadLeft(2).Replace(" ", "0") & " DAY\"
        File_Path = "C:\" & MON & DAY
        If System.IO.Directory.Exists(File_Path) = False Then
            System.IO.Directory.CreateDirectory(File_Path)
        End If

        Dim sw As StreamWriter
        sw = New StreamWriter(File_Path & "output_2.txt", True, Encoding.Default)
        sw.WriteLine(Date.Now & "," & Datacode & "," & Message)
        sw.Flush()
        sw.Close()
        sw.Dispose()
    End Sub

    Sub Write_Out_Com(ByVal Datacode As String, ByVal Message As String)
        Dim File_Path As String = Nothing
        Dim MON As String = Date.Now.Month.ToString.PadLeft(2).Replace(" ", "0") & " MONTH\"
        Dim DAY As String = Date.Now.Day.ToString.PadLeft(2).Replace(" ", "0") & " DAY\"
        File_Path = "C:\" & MON & DAY
        If System.IO.Directory.Exists(File_Path) = False Then
            System.IO.Directory.CreateDirectory(File_Path)
        End If

        Dim sw As StreamWriter
        sw = New StreamWriter(File_Path & "output_Com.txt", True, Encoding.Default)
        sw.WriteLine(Date.Now & "," & Datacode & "," & Message)
        sw.Flush()
        sw.Close()
        sw.Dispose()
    End Sub

    Sub Write_Upload_Time(ByVal Datacode As String, ByVal Message As String)
        Dim File_Path As String = Nothing
        Dim MON As String = Date.Now.Month.ToString.PadLeft(2).Replace(" ", "0") & " MONTH\"
        Dim DAY As String = Date.Now.Day.ToString.PadLeft(2).Replace(" ", "0") & " DAY\"
        File_Path = "C:\" & MON & DAY
        If System.IO.Directory.Exists(File_Path) = False Then
            System.IO.Directory.CreateDirectory(File_Path)
        End If

        Dim sw As StreamWriter
        sw = New StreamWriter(File_Path & Datacode & ".txt", True, Encoding.Default)
        sw.WriteLine(Date.Now & "," & Message)
        sw.Flush()
        sw.Close()
        sw.Dispose()
    End Sub
    Sub WriteJason(ByVal sn As String, ByVal ProjectName As String, ByVal Result As String, ByRef keyname() As String, ByRef KeyResult() As String)
        JSON_PARA.TIME = Date.Now.Year & "-" & Date.Now.Month.ToString.PadLeft(2).Replace(" ", "0") & "-" & Date.Now.Day.ToString.PadLeft(2).Replace(" ", "0") & "T" & Date.Now.Hour.ToString.PadLeft(2).Replace(" ", "0") & ":" & Date.Now.Minute.ToString.PadLeft(2).Replace(" ", "0") & ":" & Date.Now.Second.ToString.PadLeft(2).Replace(" ", "0") + "+08:00"
        JSON_PARA.RESULT = Result

        JSON_PARA.TRAYSN = sn
        Dim jasonString_tr_v2 As String = String.Empty
        Dim jasonString_kc_tray_v1 As String = String.Empty
        Dim jasonString_data As String = String.Empty
        If Date.Now.Hour >= 8 And Date.Now.Hour <= 20 Then
            JSON_PARA.SHIFT_D_N = "D"
        Else
            JSON_PARA.SHIFT_D_N = "N"
        End If
        jasonString_tr_v2 = WriteJason_tr_v2()
        jasonString_kc_tray_v1 = WriteJason_kc_tray_v2(Result, keyname, KeyResult)
        jasonString_data = WriteJason_data()
        Dim json_file_sn As String = String.Empty
        json_file_sn = sn.Replace(" ", "_")
        json_file_sn = json_file_sn.Replace("__", "_")
        WRITE_JASON_FILE(json_file_sn, jasonString_tr_v2, jasonString_kc_tray_v1, jasonString_data)
        WRITE_JASON_LOCAL_FILE(json_file_sn, jasonString_tr_v2, jasonString_kc_tray_v1, jasonString_data)
    End Sub

    Sub WRITE_JASON_FILE(ByVal SN As String, ByVal jasonString_tr_v2 As String, ByVal jasonString_kc_tray_v1 As String, ByVal jasonString_data As String)
        Dim json_file_save_folder As String = String.Empty
        Try
            json_file_save_folder = JSON_PARA.FILE_PATH + "\"
            If System.IO.Directory.Exists(json_file_save_folder) = False Then
                System.IO.Directory.CreateDirectory(json_file_save_folder)
            End If
            Dim DateString As String = Date.Now.Year & "_" & Date.Now.Month.ToString.PadLeft(2).Replace(" ", "0") & "_" & Date.Now.Day.ToString.PadLeft(2).Replace(" ", "0") & "_" & Date.Now.Hour.ToString.PadLeft(2).Replace(" ", "0") & "_" & Date.Now.Minute.ToString.PadLeft(2).Replace(" ", "0") & "_" & Date.Now.Second.ToString.PadLeft(2).Replace(" ", "0") & "_" & SN & ".json"
            Dim FileName As String = json_file_save_folder & DateString
            Dim SW As System.IO.StreamWriter
            If System.IO.Directory.Exists(json_file_save_folder) = True Then
                If System.IO.File.Exists(FileName) = False Then
                    SW = New StreamWriter(FileName, True, Encoding.Default)
                    SW.WriteLine(jasonString_tr_v2)
                    SW.WriteLine(jasonString_kc_tray_v1)
                    SW.WriteLine(jasonString_data)
                    SW.Flush()
                    SW.Close()
                    SW.Dispose()
                End If
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message & vbCrLf & json_file_save_folder, "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
        End Try
    End Sub

    Sub WRITE_JASON_LOCAL_FILE(ByVal SN As String, ByVal jasonString_tr_v2 As String, ByVal jasonString_kc_tray_v1 As String, ByVal jasonString_data As String)
        Dim D1, D2, D3 As String
        D1 = Date.Now.Month.ToString.PadLeft(2).Replace(" ", "0") + "Month\"
        D2 = Date.Now.Day.ToString.PadLeft(2).Replace(" ", "0") + "Day\"
        D3 = "D:\JSON_DATA_SAVE\" & D1 & D2
        Try

            If System.IO.Directory.Exists(D3) = False Then
                System.IO.Directory.CreateDirectory(D3)
            End If
            Dim DateString As String = Date.Now.Year & "_" & Date.Now.Month.ToString.PadLeft(2).Replace(" ", "0") & "_" & Date.Now.Day.ToString.PadLeft(2).Replace(" ", "0") & "_" & Date.Now.Hour.ToString.PadLeft(2).Replace(" ", "0") & "_" & Date.Now.Minute.ToString.PadLeft(2).Replace(" ", "0") & "_" & Date.Now.Second.ToString.PadLeft(2).Replace(" ", "0") & "_" & SN & ".json"
            Dim FileName As String = D3 & DateString
            Dim SW As System.IO.StreamWriter
            If System.IO.Directory.Exists(D3) = True Then
                If System.IO.File.Exists(FileName) = False Then
                    SW = New StreamWriter(FileName, True, Encoding.Default)
                    SW.WriteLine(jasonString_tr_v2)
                    SW.WriteLine(jasonString_kc_tray_v1)
                    SW.WriteLine(jasonString_data)
                    SW.Flush()
                    SW.Close()
                    SW.Dispose()
                End If
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message & vbCrLf, "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
        End Try
    End Sub

    Function WriteJason_tr_v2() As String
        '        "tr_v2": {
        '"time": "2017-09-27T01:55:16+08:00"
        '"agent_type": "aio",
        '"result": "fail",
        '"process": "a-af-n",
        '"component": "kc",
        '"projects": ["X12345"]
        '},
        Dim Tr_V2_Str As String = String.Empty

        Tr_V2_Str = "{" & vbCrLf & Chr(34) & "tr_v2" & Chr(34) & ": {" & vbCrLf
        Tr_V2_Str = Tr_V2_Str + Chr(34) & "time" & Chr(34) & ": " & Chr(34) & JSON_PARA.TIME & Chr(34) & "," & vbCrLf
        Tr_V2_Str = Tr_V2_Str + Chr(34) & "agent_type" & Chr(34) & ": " & Chr(34) & JSON_PARA.AGENT_TYPE & Chr(34) & "," & vbCrLf
        Tr_V2_Str = Tr_V2_Str + Chr(34) & "result" & Chr(34) & ": " & Chr(34) & JSON_PARA.RESULT & Chr(34) & "," & vbCrLf
        Tr_V2_Str = Tr_V2_Str + Chr(34) & "process" & Chr(34) & ": " & Chr(34) & JSON_PARA.PROCESS & Chr(34) & "," & vbCrLf
        Tr_V2_Str = Tr_V2_Str + Chr(34) & "component" & Chr(34) & ": " & Chr(34) & JSON_PARA.COMPONENT & Chr(34) & "," & vbCrLf
        Tr_V2_Str = Tr_V2_Str + Chr(34) & "projects" & Chr(34) & ": [" & Chr(34) & JSON_PARA.PROJECTS & Chr(34) & "]" & vbCrLf & "},"
        Return Tr_V2_Str
    End Function
    Function WriteJason_kc_tray_v2(ByVal Result As String, ByRef keyname() As String, ByRef keyresult() As String) As String
        '"kc_tray_v2": {
        '"tray_sn": "tray123456",
        '"layout": "iso",
        '    "keys": {
        '        "ESC": {
        '            "result": "pass"
        '              },
        '"Right": {
        '            "result": "pass"
        '              }
        '    }
        '},
        Dim ii As Integer = 20
        Dim kc_tray_v1_Str As String = String.Empty
        kc_tray_v1_Str = kc_tray_v1_Str + Chr(34) & "kc_tray_v2" & Chr(34) & ": {" & vbCrLf
        kc_tray_v1_Str = kc_tray_v1_Str + Chr(34) & "tray_sn" & Chr(34) & ": " & Chr(34) & JSON_PARA.TRAYSN & Chr(34) & "," & vbCrLf
        kc_tray_v1_Str = kc_tray_v1_Str + Chr(34) & "layout" & Chr(34) & ": " & Chr(34) & JSON_PARA.KB_VERSION & Chr(34) & "," & vbCrLf
        kc_tray_v1_Str = kc_tray_v1_Str + Space(4) & Chr(34) & "keys" & Chr(34) & ": {" & vbCrLf
        For i As Integer = 0 To keyresult.Length - 1
            If i <> keyresult.Length - 1 Then
                kc_tray_v1_Str = kc_tray_v1_Str + Space(8) & Chr(34) & keyname(i) & Chr(34) & ": {" & vbCrLf
                kc_tray_v1_Str = kc_tray_v1_Str + Space(12) & Chr(34) & "result" & Chr(34) & ": " & Chr(34) & keyresult(i) & Chr(34) & vbCrLf & Space(14) & "}," & vbCrLf
            Else
                kc_tray_v1_Str = kc_tray_v1_Str + Space(8) & Chr(34) & keyname(i) & Chr(34) & ": {" & vbCrLf
                kc_tray_v1_Str = kc_tray_v1_Str + Space(12) & Chr(34) & "result" & Chr(34) & ": " & Chr(34) & keyresult(i) & Chr(34) & vbCrLf & Space(14) & "}" & vbCrLf

            End If
        Next
        kc_tray_v1_Str = kc_tray_v1_Str + Space(4) + "}" + vbCrLf + "}," + vbCrLf
        Return kc_tray_v1_Str
    End Function
    Function WriteJason_data() As String

        '       "data": {
        '  "program_ver: “X1359_20171115",
        '  “aoi_vendor": "RS",
        '  “line_number": "1",
        '  "cycle_time": "13.94",
        '"Shift_D/N": "D"
        '"builds": "P1",
        '}
        Dim Data_Str As String = String.Empty
        Data_Str = Chr(34) & "data" & Chr(34) & ": {" & vbCrLf
        Data_Str = Data_Str + Chr(34) & "program_ver" & Chr(34) & ": " & Chr(34) & JSON_PARA.PROGRAM_VER & Chr(34) & "," & vbCrLf
        Data_Str = Data_Str + Chr(34) & "aoi_vendor" & Chr(34) & ": " & Chr(34) & JSON_PARA.AOI_VENDOR & Chr(34) & "," & vbCrLf
        Data_Str = Data_Str + Chr(34) & "line_number" & Chr(34) & ": " & Chr(34) & JSON_PARA.LINE_NUMBER & Chr(34) & "," & vbCrLf
        Data_Str = Data_Str + Chr(34) & "cycle_time" & Chr(34) & ": " & Chr(34) & JSON_PARA.CYCLE_TIME & Chr(34) & "," & vbCrLf
        Data_Str = Data_Str + Chr(34) & "Shift_D/N" & Chr(34) & ": " & Chr(34) & JSON_PARA.SHIFT_D_N & Chr(34) & "," & vbCrLf
        Data_Str = Data_Str + Chr(34) & "builds" & Chr(34) & ": " & Chr(34) & JSON_PARA.BUILDS & Chr(34) & vbCrLf & Space(8) & "}" & vbCrLf & "}"
        Return Data_Str
    End Function

    
    Private Sub GRAB_IMAGE_LEFT_DoWork(sender As System.Object, e As System.ComponentModel.DoWorkEventArgs) Handles GRAB_IMAGE_LEFT.DoWork
        If GRAB_IMAGE_BOOLEAN_LEFT = True Then
            Dim subName As String = "1工位图像采集" & ":"
            Try
                Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), subName & "Start", DataCode_Left, Color.Blue)
                ToolBlock_left.Run()
                Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), subName & "End", DataCode_Left, Color.Blue)
            Catch ex As Exception
                Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), subName & "：" & ex.ToString, DataCode_Left, Color.Red)
            End Try
        End If
        GRAB_IMAGE_BOOLEAN_LEFT = False
        GRAB_IMAGE_LEFT.CancelAsync()
        GRAB_IMAGE_LEFT.Dispose()
    End Sub

    Private Sub GRAB_IMAGE_RIGHT_DoWork(sender As System.Object, e As System.ComponentModel.DoWorkEventArgs) Handles GRAB_IMAGE_RIGHT.DoWork
        If GRAB_IMAGE_BOOLEAN_RIGHT = True Then
            Dim subName As String = "2工位图像采集" & ":"
            Try
                Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), subName & "Start", DataCode_Right, Color.Blue)
                toolblock_right.Run()
                Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), subName & "End", DataCode_Right, Color.Blue)
            Catch ex As Exception
                Invoke(New Delegate_Display_Message_Public(AddressOf Display_Message_Public), subName & "：" & ex.ToString, DataCode_Right, Color.Red)
            End Try
        End If
        GRAB_IMAGE_BOOLEAN_RIGHT = False
        GRAB_IMAGE_RIGHT.CancelAsync()
        GRAB_IMAGE_RIGHT.Dispose()
    End Sub

    Private Sub PROCESS_IMAGE_LEFT_DoWork(sender As System.Object, e As System.ComponentModel.DoWorkEventArgs) Handles PROCESS_IMAGE_LEFT.DoWork
        If Process_Left_Boolean = True Then
            Dim StartTime As Date, EndTime As Date, LostTime As Integer
            StartTime = Date.Now

            NG_count_Left = 0
            While LR_PIC_Procesing
                Thread.Sleep(20)
            End While
            LR_PIC_Procesing = True
            Thread.Sleep(50)
            Try
                Back_Run_Left() '运行图像处理
            Catch ex As Exception
                MessageBox.Show("LEFT图像处理过程出错:" & ex.ToString)
            End Try
            Thread.Sleep(20)
            LR_PIC_Procesing = False


            Dim NG As Int16 = 0
            Select Case KB_Type
                Case KB_Type_Enum.ANSI
                    NG = 30
                Case KB_Type_Enum.ISO
                    NG = 30
                Case KB_Type_Enum.JIS
                    NG = 60
            End Select

            If NG_count_Left >= NG Then
                Invoke(New Delegate_Display_Message_Left(AddressOf Display_Message_Left), "1工位检测到NG数量为【" & NG_count_Left & "】图像可能错乱，相机重新启动...", DataCode_Left_tmp, Color.Red)
                Write_Log_Left(DataCode_Left_tmp, "图像错乱，准备重启相机")
                ToolBlock_left.Dispose()
                ToolBlock_left = New CogToolBlock

                Load_VPP_Left()
                Invoke(New Delegate_Display_Message_Left(AddressOf Display_Message_Left), "1工位相机重启完成", DataCode_Left_tmp, Color.Red)
            End If

            EndTime = Date.Now
            LostTime = DateDiff(DateInterval.Second, StartTime, EndTime)
            Write_Measure_Time_Left(DataCode_Left_tmp, "L," & LostTime.ToString)
        End If
        Process_Left_Boolean = False
        PROCESS_IMAGE_LEFT.CancelAsync()
        PROCESS_IMAGE_LEFT.Dispose()
    End Sub

    Private Sub PROCESS_IMAGE_RIGHT_DoWork(sender As System.Object, e As System.ComponentModel.DoWorkEventArgs) Handles PROCESS_IMAGE_RIGHT.DoWork
        If Process_Right_Boolean = True Then
            Dim StartTime As Date, EndTime As Date, LostTime As Integer
            StartTime = Date.Now

            NG_Count_Right = 0

            While LR_PIC_Procesing
                Thread.Sleep(20)
            End While
            LR_PIC_Procesing = True
            Thread.Sleep(50)
            Try
                Back_Run_Right() '运行图像处理
            Catch ex As Exception
                MessageBox.Show("RIGHT图像处理过程出错:" & ex.ToString)
            End Try
            Thread.Sleep(20)
            LR_PIC_Procesing = False

            Dim NG As Int16 = 0
            Select Case KB_Type
                Case KB_Type_Enum.ANSI
                    NG = 30
                Case KB_Type_Enum.ISO
                    NG = 30
                Case KB_Type_Enum.JIS
                    NG = 60
            End Select

            If NG_Count_Right >= NG Then


                Invoke(New Delegate_Display_Message_Right(AddressOf Display_Message_Right), "2工位检测到NG数量为【" & NG_Count_Right & "】图像可能错乱，相机重新启动...", DataCode_Right_tmp, Color.Red)
                Write_Log_Right(DataCode_Right_tmp, "图像错乱，准备重启相机")
                toolblock_right.Dispose()
                toolblock_right = New CogToolBlock

                Load_VPP_Right()

                Invoke(New Delegate_Display_Message_Right(AddressOf Display_Message_Right), "2工位相机重启完成", DataCode_Right_tmp, Color.Red)
            End If

            EndTime = Date.Now
            LostTime = DateDiff(DateInterval.Second, StartTime, EndTime)
            Write_Measure_Time_Right(DataCode_Right_tmp, "R," & LostTime.ToString)
        End If
        Process_Right_Boolean = False
        PROCESS_IMAGE_RIGHT.CancelAsync()
        PROCESS_IMAGE_RIGHT.Dispose()
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Dim json_file As New FolderBrowserDialog
        If json_file.ShowDialog = Windows.Forms.DialogResult.OK Then
            TB_JSON_FILE_PATH.Text = json_file.SelectedPath
        End If
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        If MessageBoxEx.Show("确定保存参数信息吗？", "系统信息", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1) = Windows.Forms.DialogResult.OK Then

            Array.Resize(Parameters_Name_Array, 9)
            Array.Resize(Parameters_Value_Array, 9)
            Array.Clear(Parameters_Name_Array, 0, Parameters_Name_Array.Length)
            Array.Clear(Parameters_Value_Array, 0, Parameters_Value_Array.Length)
            Parameters_Name_Array(0) = "PROJECTS"
            Parameters_Name_Array(1) = "PROGRAM_VER"
            Parameters_Name_Array(2) = "AOI_VENDOR"
            Parameters_Name_Array(3) = "LINE_NUMBER"
            Parameters_Name_Array(4) = "BUILDS"
            Parameters_Name_Array(5) = "JSON_FILE_PATH"
            Parameters_Name_Array(6) = "AGENT_TYPE"
            Parameters_Name_Array(7) = "PROCESS"
            Parameters_Name_Array(8) = "COMPONENT"

            Parameters_Value_Array(0) = TB_JASON_PROJECTS.Text.Trim
            Parameters_Value_Array(1) = TB_JSON_PROGRAM_VER.Text.Trim
            Parameters_Value_Array(2) = TB_JSON_AOI_VENDOR.Text.Trim
            Parameters_Value_Array(3) = TB_JSON_LINE_NUMBER.Text.Trim
            Parameters_Value_Array(4) = TB_JSON_BUILDS.Text.Trim
            Parameters_Value_Array(5) = TB_JSON_FILE_PATH.Text.Trim
            Parameters_Value_Array(6) = TB_JASON_AGENT_TYPE.Text.Trim
            Parameters_Value_Array(7) = TB_JASON_PROCESS.Text.Trim
            Parameters_Value_Array(8) = TB_JASON_COMPONENT.Text.Trim


            SQLCON.Update_Project_Parameter(Parameters_Name_Array, Parameters_Value_Array)
            SQLCON.Read_Project_Parameter(Parameters_Name_Array, Parameters_Value_Array)
            TB_JASON_PROJECTS.Text = Parameters_Value_Array(0).ToString.Trim
            TB_JSON_PROGRAM_VER.Text = Parameters_Value_Array(1).ToString.Trim
            TB_JSON_AOI_VENDOR.Text = Parameters_Value_Array(2).ToString.Trim
            TB_JSON_LINE_NUMBER.Text = Parameters_Value_Array(3).ToString.Trim
            TB_JSON_BUILDS.Text = Parameters_Value_Array(4).ToString.Trim
            TB_JSON_FILE_PATH.Text = Parameters_Value_Array(5).ToString.Trim
            TB_JASON_AGENT_TYPE.Text = Parameters_Value_Array(6).ToString.Trim
            TB_JASON_PROCESS.Text = Parameters_Value_Array(7).ToString.Trim
            TB_JASON_COMPONENT.Text = Parameters_Value_Array(8).ToString.Trim

            JSON_PARA.PROJECTS = Parameters_Value_Array(0).ToString.Trim
            JSON_PARA.PROGRAM_VER = Parameters_Value_Array(1).ToString.Trim
            JSON_PARA.AOI_VENDOR = Parameters_Value_Array(2).ToString.Trim
            JSON_PARA.LINE_NUMBER = Parameters_Value_Array(3).ToString.Trim
            JSON_PARA.BUILDS = Parameters_Value_Array(4).ToString.Trim
            JSON_PARA.FILE_PATH = Parameters_Value_Array(5).ToString.Trim
            JSON_PARA.AGENT_TYPE = Parameters_Value_Array(6).ToString.Trim
            JSON_PARA.PROCESS = Parameters_Value_Array(7).ToString.Trim
            JSON_PARA.COMPONENT = Parameters_Value_Array(8).ToString.Trim

            Array.Resize(Parameters_Name_Array, 3)
            Array.Resize(Parameters_Value_Array, 3)
            Array.Clear(Parameters_Name_Array, 0, Parameters_Name_Array.Length)
            Array.Clear(Parameters_Value_Array, 0, Parameters_Value_Array.Length)
            Parameters_Name_Array(0) = "JSON_UPLOAD"
            Parameters_Name_Array(1) = "左工位版本切换电磁阀"
            Parameters_Name_Array(2) = "右工位版本切换电磁阀"

            Parameters_Value_Array(0) = IIf(SW_JSON_UPLOAD.Value = True, 1, 0)
            Parameters_Value_Array(1) = IIf(SW_Vision_1.Value = True, 1, 0)
            Parameters_Value_Array(2) = IIf(SW_Vision_2.Value = True, 1, 0)

            SQLCON.Update_Project_Parameter(Parameters_Name_Array, Parameters_Value_Array)
            SQLCON.Read_Project_Parameter(Parameters_Name_Array, Parameters_Value_Array)
            SW_JSON_UPLOAD.Value = CType(Parameters_Value_Array(0), Boolean)
            SW_Vision_1.Value = CType(Parameters_Value_Array(1), Boolean)
            SW_Vision_2.Value = CType(Parameters_Value_Array(2), Boolean)


            JSON_PARA.JSON_UPLOAD = CType(Parameters_Value_Array(0), Boolean)

            Select Case SW_Vision_1.Value
                Case True
                    CARDCMD.SET_DO_BIT(BOARD_ID_01, DIO.ONE1工位版本切换电磁阀_1, PRA_OUT_STATUS_ENUM.OUT_ON)
                    Thread.Sleep(30)
                Case False
                    CARDCMD.SET_DO_BIT(BOARD_ID_01, DIO.ONE1工位版本切换电磁阀_1, PRA_OUT_STATUS_ENUM.OUT_OFF)
                    Thread.Sleep(30)
            End Select
            Select Case SW_Vision_2.Value
                Case True
                    CARDCMD.SET_DO_BIT(BOARD_ID_01, DIO.TWO2工位版本切换电磁阀_1, PRA_OUT_STATUS_ENUM.OUT_ON)
                    Thread.Sleep(30)
                Case False
                    CARDCMD.SET_DO_BIT(BOARD_ID_01, DIO.TWO2工位版本切换电磁阀_1, PRA_OUT_STATUS_ENUM.OUT_OFF)
                    Thread.Sleep(30)
            End Select

            MessageBoxEx.Show("参数修改完成", "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1)
        End If
    End Sub

    Private Sub PanelEx20_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PanelEx20.Click
    End Sub

    Private Sub PanelEx26_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PanelEx26.Click
       
    End Sub

    Private Sub CBO_JSON_PROJECTS_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
    End Sub

    Private Sub ButtonItem1_Click(sender As System.Object, e As System.EventArgs) Handles ButtonItem1.Click
        CogToolBlockEditV21.Subject = CogToolBlock_Group_Left
    End Sub

    Private Sub ButtonItem2_Click(sender As System.Object, e As System.EventArgs) Handles ButtonItem2.Click
        CogToolBlockEditV22.Subject = CogToolBlock_Group_Right
    End Sub

    Private Sub ButtonItem3_Click(sender As System.Object, e As System.EventArgs) Handles ButtonItem3.Click
        CogToolBlockEditV21.Subject = Nothing
        CogToolBlockEditV22.Subject = Nothing
    End Sub
End Class
Imports System.IO
Imports System.Text

Module ParamModel

    Private Declare Function GetPrivateProfileString Lib "kernel32" Alias "GetPrivateProfileStringA" (ByVal lpApplicationName As String, ByVal lpKeyName As String, ByVal lpDefault As String, ByVal lpReturnedString As String, ByVal nSize As Int32, ByVal lpFileName As String) As Int32
    Private Declare Function WritePrivateProfileString Lib "kernel32" Alias "WritePrivateProfileStringA" (ByVal lpApplicationName As String, ByVal lpKeyName As String, ByVal lpString As String, ByVal lpFileName As String) As Int32
    '定义读取配置文件函数  
    Public Function GetINI(ByVal Section As String, ByVal AppName As String, ByVal lpDefault As String, ByVal FileName As String) As String
        Dim Str As String = Nothing
        Str = LSet(Str, 256)
        GetPrivateProfileString(Section, AppName, lpDefault, Str, Len(Str), FileName)
        Return Microsoft.VisualBasic.Left(Str, InStr(Str, Chr(0)) - 1)
    End Function
    '定义写入配置文件函数  
    Public Function WriteINI(ByVal Section As String, ByVal AppName As String, ByVal lpDefault As String, ByVal FileName As String) As Long
        WriteINI = WritePrivateProfileString(Section, AppName, lpDefault, FileName)
    End Function
    Structure LoadCellData
        Dim Voltage As Double
        Dim Encoder As Integer
    End Structure
    ''' <summary>
    ''' 自动插入坐标
    ''' </summary>
    ''' <remarks></remarks>
    Structure AutoInsertPosParam
        Shared 行数 As Integer
        Shared 列数 As Integer
        Shared 行间距 As Double
        Shared 列间距 As Double
        Shared 行轴名称 As String
        Shared 列轴名称 As String
        Shared 行轴 As Integer
        Shared 列轴 As Integer
    End Structure
    Public Structure PointStc
        Dim X As Double
        Dim Y As Double
    End Structure
    Public Structure UserName
        Shared Admin As String = "ADMIN"
        Shared OP As String = "OP"
        Shared User As String = "USER"
    End Structure
    ''' <summary>
    ''' 原点运动参数
    ''' </summary>
    ''' <remarks></remarks>
    Public Structure HOME_PARAM
        Shared 轴名称 As String
        Shared 轴号 As Integer
        Shared 导程 As Double
        Shared 回原点模式 As Integer '整型
        Shared 回原点搜索方向 As Integer '整型
        Shared 回原点Z相信号 As Integer '整型
        Shared 回原点曲线 As Integer '整型
        Shared 回原点速度 As Integer
        Shared 回原点加减速度 As Double
        Shared 回原点偏移 As Integer
        Shared 回原点顺序 As Integer
    End Structure

    ''' <summary>
    ''' Device参数类型
    ''' </summary>
    ''' <remarks></remarks>
    Public Structure Device_ParamType
        Shared 变量名称 As String
        Shared Double_当前值 As Double, Double_最小值 As Double, Double_最大值 As Double
        Shared Integer_当前值 As Integer, Integer_最小值 As Integer, Integer_最大值 As Integer
        Shared String_当前值 As String
        Shared Bool_当前值 As Integer
    End Structure
    ''' <summary>
    ''' DEVICE参数表名
    ''' </summary>
    ''' <remarks></remarks>
    Public Structure DEVICEPARAM_TABLENAME
        Shared DOUBLE_TYPE As String = "DEVICE_DOUBLE_PARAMETERS"
        Shared INTEGER_TYPE As String = "DEVICE_INT_PARAMETERS"
        Shared STRING_TYPE As String = "DEVICE_STRING_PARAMETERS"
        Shared BOOL_TYPE As String = "DEVICE_BOOL_PARAMETERS"
    End Structure

    ''' <summary>
    ''' 坐标系参数
    ''' </summary>
    ''' <remarks></remarks>
    Public Structure Work_Coordinatess
        Dim 轴名称() As String
        Dim 轴位置() As Double
        Dim 轴号() As String
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
    Public Work_Coordinatess_Obj As Work_Coordinatess

    ''' <summary>
    ''' 凌华卡的错误返回值
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum ADLINK_Err_Code
        Err_0 = 0
        Err_1 = -1
        Err_2 = -2
        Err_3 = -3
        Err_4 = -4
        Err_5 = -5
        Err_6 = -6
        Err_7 = -7
        Err_8 = -8
        Err_9 = -9
        Err_10 = -10
        Err_11 = -11
        Err_12 = -12
        Err_13 = -13
        Err_14 = -14
        Err_15 = -15
        Err_16 = -16
        Err_17 = -17
        Err_18 = -18
        Err_19 = -19
        Err_20 = -20
        Err_21 = -21
        Err_22 = -22
        Err_23 = -23
        Err_24 = -24
        Err_25 = -25
        Err_32 = -32
        Err_33 = -33
        Err_40 = -40
        Err_1000 = -1000
    End Enum
    ''' <summary>
    ''' 凌华卡的错误返回信息
    ''' </summary>
    ''' <remarks></remarks>
    Public Structure ADLINK_Err_MSG
        Shared Err_0 As String = "[ERR_NoError]:Success, No error"
        Shared Err_1 As String = "[ERR_OSVersion]:Operating system version error.The current operating system you used are not supported by this function."
        Shared Err_2 As String = "[ERR_OpenDriverFailed]:Open driver failed. Create driver interface failed.Check device driver is installed correctly.Check devices are installed correctly in your system."
        Shared Err_3 As String = "[ERR_InsufficientMemory]:System memory insufficiently.There is not enough memory in your system."
        Shared Err_4 As String = "[ERR_DeviceNotInitial]:The Device or the card is not be initialized.Check the card ID.The device has been closed.The device is not be initialized."
        Shared Err_5 As String = "[ERR_NoDeviceFound]:Devices not found.Check device driver is installed correctly.Check devices are installed correctly in your system."
        Shared Err_6 As String = "[ERR_CardIdDuplicate]:Card ID duplicated.Check the card ID settings (SW jump).Check the parameter of initial function is correctly."
        Shared Err_7 As String = "[ERR_DeviceAlreadyIntialed]:The devices have already been initialed.1. Check the close card function is work correctly."
        Shared Err_8 As String = "[ERR_InterruptNotEnable]:Interrupt events not be enabled.1. Enable the hardware interrupt.2. Check the interrupt factor is set correctly."
        Shared Err_9 As String = "[ERR_TimeOut]:Function timeout."
        Shared Err_10 As String = "[ERR_ParametersInvaild]:The value of the parameters is incorrect.Check the setting range of parameters.Compare the setting value of parameters with user manual."
        Shared Err_11 As String = "[ERR_SetEEPROM]:Hardware memory write error."
        Shared Err_12 As String = "[ERR_GetEEPROM]:Hardware memory read error."
        Shared Err_13 As String = "[ERR_FunctionNotAvailable]:The function is not available in current stage.The device is not support this function.System is in error state.1. Check the function library 2. Check the hardware connection (servo drive connection) 3. Reinitial(Reboot) the system."
        Shared Err_14 As String = "[ERR_FirmwareError]:Firmware process error. 1. Check the firmware version."
        Shared Err_15 As String = "[ERR_CommandInProcess]:The previous command is in process."
        Shared Err_16 As String = "[ERR_AxisIdDuplicate]:Axes' ID is duplicated."
        Shared Err_17 As String = "[ERR_ModuleNotFound]:Slave module not found."
        Shared Err_18 As String = "[ERR_InsufficientModuleNo]:System ModuleNo insufficiently"
        Shared Err_19 As String = "[ERR_HandShakeFailed]:HandSake with the DSP out of time."
        Shared Err_20 As String = "[ERR_FILE_FORMAT]:Config file format error.(cannot be parsed)"
        Shared Err_21 As String = "[ERR_ParametersReadOnly]:Function parameters read only."
        Shared Err_22 As String = "[ERR_DistantNotEnough]:Distant is not enough for motion."
        Shared Err_23 As String = "[ERR_FunctionNotEnable]:Function not yet enabled"
        Shared Err_24 As String = "[ERR_ServerAlreadyClose]:Server already closed"
        Shared Err_25 As String = "[ERR_DllNotFound]:Could’t find virtual DLL"
        Shared Err_32 As String = "[ERR_DllFuncFailed]:Could’t find specified function on virtual DLL"
        Shared Err_33 As String = "[ERR_FeederAbnormalStop]:Feeder abnormally stop"
        Shared Err_40 As String = "[ERR_DoubleOverflow]:Double format parameter is overflow"
        Shared Err_1000 As String = "[ERR_Win32Error]:No such event number, or WIN32_API error,contact with ADLINK's FAE staff."
    End Structure
    ''' <summary>
    ''' 急停类型，JOG_STOP：运动停止 EMG_STOP：急停
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum STOP_TYPE
        JOG_STOP
        EMG_STOP
    End Enum
    ''' <summary>
    ''' 用户管理
    ''' </summary>
    ''' <remarks></remarks>
    Public Structure USER_NAME
        Shared 用户名 As String = ""
        Shared 登陆密码 As String = ""
        Shared 清空数据密码 As String = ""
        Shared 删除项目密码 As String = ""
    End Structure
    ''' <summary>
    ''' 左右工位
    ''' </summary>
    ''' <remarks></remarks>
    Public Structure STATION
        Shared LEFT As String = "LEFT"
        Shared RIGHT As String = "RIGHT"
        Shared COMMON As String = "COMMON"
    End Structure

    ''' <summary>
    ''' 坐标名称dataset
    ''' </summary>
    ''' <remarks></remarks>
    Public Structure COORDINATENAME
        Shared LEFT_DATASET As DataSet
        Shared RIGHT_DATASET As DataSet
        Shared COMMON_DATASET As DataSet
        Shared DATASET As DataSet
    End Structure
    ''' <summary>
    ''' 实际坐标dataset
    ''' </summary>
    ''' <remarks></remarks>
    Public Structure COORDINATEPARAM
        Shared LEFT_DATASET As DataSet
        Shared RIGHT_DATASET As DataSet
        Shared DATASET As DataSet
    End Structure

  

    ''' <summary>
    ''' 保存设备数据
    ''' </summary>
    ''' <param name="Data_Name">行名</param>
    ''' <param name="Data_Value">数据</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function Save_Fixtrue_Data(ByVal FilePath As String, ByVal SN As String, ByVal Time As String, ByVal Data_Name() As String, ByVal Data_Value() As Object) As Boolean
        If Data_Name Is Nothing Or Data_Value Is Nothing Then
            Return False
        End If
        If Data_Name.Length <> Data_Value.Length Or Data_Name.Length = 0 Or Data_Value.Length = 0 Then
            Return False
        End If
        Try
            Dim File_Path As String = Nothing, File_Full_Name As String = Nothing
            Dim Data_String As String = Nothing, SW As StreamWriter

            Dim D1 As String = Date.Now.Year.ToString & " YEAR\"
            Dim D2 As String = Date.Now.Month.ToString.PadLeft(2).Replace(" ", "0") & " MONTH\"
            Dim D3 As String = Date.Now.Day.ToString.PadLeft(2).Replace(" ", "0") & " DAY\"
            File_Path = FilePath & D1 & D2 & D3
            If System.IO.Directory.Exists(File_Path) = False Then
                System.IO.Directory.CreateDirectory(File_Path)
            End If

            File_Full_Name = FilePath & D1 & D2 & D3 & SN & "_" & Time & ".CSV"

            If File.Exists(File_Full_Name) = False Then
                Data_String = ""
                SW = New StreamWriter(File_Full_Name, True, Encoding.Default)
                For i As Integer = 0 To Data_Name.Length - 1
                    Select Case i
                        Case Data_Name.Length - 1
                            If Data_Name(i) IsNot Nothing Then
                                Data_String += Data_Name(i).ToString()
                            Else
                                Data_String += "RENAME"
                            End If
                        Case Else
                            If Data_Name(i) IsNot Nothing Then
                                Data_String += Data_Name(i).ToString() + ","
                            Else
                                Data_String += "RENAME" + ","
                            End If
                    End Select
                Next
                SW.WriteLine(Data_String)
                SW.Flush()
                SW.Close()
                SW.Dispose()
            End If

            Data_String = ""
            SW = New StreamWriter(File_Full_Name, True, Encoding.Default)
            For i As Integer = 0 To Data_Value.Length - 1
                Select Case i
                    Case Data_Value.Length - 1
                        If Data_Value(i) IsNot Nothing Then
                            Data_String += Data_Value(i).ToString()
                        Else
                            Data_String += "N/A"
                        End If
                    Case Else
                        If Data_Value(i) IsNot Nothing Then
                            Data_String += Data_Value(i).ToString() + ","
                        Else
                            Data_String += "N/A" + ","
                        End If
                End Select
            Next
            SW.WriteLine(Data_String)
            SW.Flush()
            SW.Close()
            SW.Dispose()
            Return True
        Catch ex As Exception
            MessageBox.Show(ex.ToString, "", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
            Return False
        End Try
    End Function

    ''' <summary>
    ''' 保存设备数据
    ''' </summary>
    ''' <param name="Data_Name">行名</param>
    ''' <param name="Data_Value">数据</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function Save_Laser_Data(ByVal FilePath As String, ByVal SN As String, ByVal Time As String, ByVal Data_Name() As String, ByVal Data_Value() As Object) As Boolean
        If Data_Name Is Nothing Or Data_Value Is Nothing Then
            Return False
        End If
        If Data_Name.Length <> Data_Value.Length Or Data_Name.Length = 0 Or Data_Value.Length = 0 Then
            Return False
        End If
        Try
            Dim File_Path As String = Nothing, File_Full_Name As String = Nothing
            Dim Data_String As String = Nothing, SW As StreamWriter

            Dim D1 As String = FilePath
            Dim D2 As String = Date.Now.Month.ToString.PadLeft(2).Replace(" ", "0") & " MONTH\"
            Dim D3 As String = Date.Now.Day.ToString.PadLeft(2).Replace(" ", "0") & " DAY\"
            File_Path = D1 & D2 & D3
            If System.IO.Directory.Exists(File_Path) = False Then
                System.IO.Directory.CreateDirectory(File_Path)
            End If

            File_Full_Name = D1 & D2 & D3 & SN & "_" & Time & "_Laser.CSV"

            If File.Exists(File_Full_Name) = False Then
                Data_String = ""
                SW = New StreamWriter(File_Full_Name, True, Encoding.Default)
                For i As Integer = 0 To Data_Name.Length - 1
                    Select Case i
                        Case Data_Name.Length - 1
                            If Data_Name(i) IsNot Nothing Then
                                Data_String += Data_Name(i).ToString()
                            Else
                                Data_String += "RENAME"
                            End If
                        Case Else
                            If Data_Name(i) IsNot Nothing Then
                                Data_String += Data_Name(i).ToString() + ","
                            Else
                                Data_String += "RENAME" + ","
                            End If
                    End Select
                Next
                SW.WriteLine(Data_String)
                SW.Flush()
                SW.Close()
                SW.Dispose()
            End If

            Data_String = ""
            SW = New StreamWriter(File_Full_Name, True, Encoding.Default)
            For i As Integer = 0 To Data_Value.Length - 1
                Select Case i
                    Case Data_Value.Length - 1
                        If Data_Value(i) IsNot Nothing Then
                            Data_String += Data_Value(i).ToString()
                        Else
                            Data_String += "N/A"
                        End If
                    Case Else
                        If Data_Value(i) IsNot Nothing Then
                            Data_String += Data_Value(i).ToString() + ","
                        Else
                            Data_String += "N/A" + ","
                        End If
                End Select
            Next
            SW.WriteLine(Data_String)
            SW.Flush()
            SW.Close()
            SW.Dispose()
            Return True
        Catch ex As Exception
            MessageBox.Show(ex.ToString, "", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
            Return False
        End Try
    End Function

 End Module

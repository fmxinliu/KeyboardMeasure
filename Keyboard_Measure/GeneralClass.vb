Imports System.Threading
Imports System.Reflection.MethodBase
Imports System.Management
Imports System.Data.SqlClient
Imports System.Text
Imports System.IO
Imports DevComponents.DotNetBar
Imports System.Windows.Forms
Imports System.Drawing
Imports System.Runtime.InteropServices
Imports Microsoft.Office.Interop
Imports System.Math

Public Module GeneralClass

    Dim ProcessName As Process()
    Const ErrName = "DLL."
    Const ErrCode = "ErrCode:["

    Public Class DATA_CONVERSION_LIB

        Public Xls As Excel.Application = New Excel.Application '定义Excel.App
        Public Xlsbook As Excel.Workbook '定义Excel.Book
        Public Xlssheet As Excel.Worksheet '定义Excel.Sheet
        Dim ProcessName As Process()

        Public Enum Save_Data_Format
            XLS
            CSV
            XLSX
        End Enum

        Structure SF_PARAM
            Shared STATION As String = "&station="
            Shared SN As String = "&sn="
            Shared VENDOR As String = "&vendor="
            Shared FIXTURENAME As String = "&fixturename="
            Shared FIXTUREID As String = "&fixtureid="
            Shared HWVERSION As String = "&hwversion="
            Shared SWVERSION As String = ",swversion="
            Shared POSITIONID As String = "&positionid="
            Shared GLUESN As String = "&gluesn="
            Shared GASKETSN As String = "&gasketsn="
            Shared ACOUSTICSN As String = "&acousticsn="
            Shared RESULT As String = "&result="
            Shared ASSYTIME As String = "&assytime="
            Shared FINISHTIME As String = "&finishtime="
            Shared OPID As String = "&opid="
            Dim T1 As Byte
        End Structure

        Structure SF_PARAM_VEALUE_STRUCT
            Dim STATION As String
            Dim SN As String
            Dim VENDOR As String
            Dim FIXTURENAME As String
            Dim FIXTUREID As String
            Dim HWVERSION As String
            Dim SWVERSION As String
            Dim POSITIONID As String
            Dim GLUESN As String
            Dim GASKETSN As String
            Dim ACOUSTICSN As String
            Dim RESULT As String
            Dim ASSYTIME As String
            Dim FINISHTIME As String
            Dim UPLOAD_ADD As String
            Dim SELECT_ADD As String
            Dim OPID As String
            Dim VISION_VERSION As String
        End Structure

        Public Function UPDATE_SF_PARAM_VEALUE(ByVal ShopFloor_Value As SF_PARAM_VEALUE_STRUCT, ByVal SN_TEXT As String, ByVal startTime As Date, ByVal stopTime As Date) As String
            ShopFloor_Value.SN = SN_TEXT.Trim
            ShopFloor_Value.ASSYTIME = Date.Now
            ShopFloor_Value.FINISHTIME = Date.Now
            UPDATE_SF_PARAM_VEALUE = ShopFloor_Value.UPLOAD_ADD & SF_PARAM.STATION & ShopFloor_Value.STATION & SF_PARAM.SN & ShopFloor_Value.SN & SF_PARAM.FIXTURENAME & ShopFloor_Value.FIXTURENAME & SF_PARAM.FIXTUREID & ShopFloor_Value.FIXTUREID & SF_PARAM.HWVERSION & ShopFloor_Value.HWVERSION & SF_PARAM.SWVERSION & ShopFloor_Value.SWVERSION & SF_PARAM.POSITIONID & ShopFloor_Value.POSITIONID & SF_PARAM.GLUESN & ShopFloor_Value.GLUESN & SF_PARAM.GASKETSN & ShopFloor_Value.GASKETSN & SF_PARAM.ACOUSTICSN & ShopFloor_Value.ACOUSTICSN & SF_PARAM.RESULT & ShopFloor_Value.RESULT & SF_PARAM.ASSYTIME & ShopFloor_Value.ASSYTIME & SF_PARAM.FINISHTIME & ShopFloor_Value.FINISHTIME
        End Function

        Sub Order_Index(ByVal data1_Array() As Double, ByVal data2_Array() As Double, ByRef Data1_Index As List(Of Integer), ByRef data2_Index As List(Of Integer), ByVal Order As Integer)
            Dim tuple_List As New List(Of Double)
            Data1_Index = New List(Of Integer)
            data2_Index = New List(Of Integer)
            Data1_Index.Clear()
            data2_Index.Clear()

            Dim data1() As Double, data2 As Double()
            If data1_Array.Length <= data2_Array.Length Then
                data2 = data1_Array
                data1 = data2_Array
            Else
                data1 = data1_Array
                data2 = data2_Array
            End If
            Dim Start_Index As Int64 = 0

            If Order = 0 Then '升序
                For i As Int16 = 0 To data1.Length - 1
                    For j As Int16 = Start_Index To data1.Length - 1
                        If j > data2.Length - 1 Then
                            j = data2.Length - 1
                            If data2(j) <= data1(i) Then
                                tuple_List.Add(data1(i))
                                Exit For
                            Else
                                tuple_List.Add(data1(i))
                                Exit For
                            End If
                        Else
                            If i = data1.Length - 1 Then
                                If data2(j) <= data1(i) Then
                                    tuple_List.Add(data2(j))
                                    Start_Index += 1
                                Else
                                    tuple_List.Add(data1(i))
                                    For m As Int16 = j To data2.Length - 1
                                        tuple_List.Add(data2(m))
                                    Next
                                    Exit For
                                End If
                            Else
                                If data2(j) <= data1(i) Then
                                    tuple_List.Add(data2(j))
                                    Start_Index += 1
                                Else
                                    tuple_List.Add(data1(i))
                                    Exit For
                                End If
                            End If
                        End If
                    Next
                Next
            Else '降序
                For i As Int16 = 0 To data1.Length - 1
                    For j As Int16 = Start_Index To data1.Length - 1
                        If j > data2.Length - 1 Then
                            j = data2.Length - 1
                            If data2(j) >= data1(i) Then
                                tuple_List.Add(data1(i))
                                Exit For
                            Else
                                tuple_List.Add(data1(i))
                                Exit For
                            End If
                        Else
                            If i = data1.Length - 1 Then
                                If data2(j) >= data1(i) Then
                                    tuple_List.Add(data2(j))
                                    Start_Index += 1
                                Else
                                    tuple_List.Add(data1(i))
                                    For m As Int16 = j To data2.Length - 1
                                        tuple_List.Add(data2(m))
                                    Next
                                    Exit For
                                End If
                            Else
                                If data2(j) >= data1(i) Then
                                    tuple_List.Add(data2(j))
                                    Start_Index += 1
                                Else
                                    tuple_List.Add(data1(i))
                                    Exit For
                                End If
                            End If
                        End If
                    Next
                Next
            End If

            For i As Int16 = 0 To data1_Array.Length - 1
                For j As Int16 = 0 To tuple_List.Count - 1
                    If data1_Array(i) = tuple_List(j) Then
                        Data1_Index.Add(j)
                        Exit For
                    End If
                Next
            Next
            For i As Int16 = 0 To data2_Array.Length - 1
                For j As Int16 = 0 To tuple_List.Count - 1
                    If data2_Array(i) = tuple_List(j) Then
                        data2_Index.Add(j)
                        Exit For
                    End If
                Next
            Next
        End Sub

        ''' <summary>
        ''' OPT_CRC
        ''' </summary>
        ''' <param name="FCS_STR"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function OPT_CRC(ByVal FCS_STR As String) As String
            Dim SLEN, I, XOR_RESULT As Integer
            Dim TEMP_FCS As String
            XOR_RESULT = 0
            SLEN = FCS_STR.Length
            For I = 1 To SLEN
                XOR_RESULT = XOR_RESULT Xor Asc(Mid$(FCS_STR, I, 1))
            Next
            TEMP_FCS = Hex$(XOR_RESULT)
            If Len(TEMP_FCS) = 1 Then TEMP_FCS = "0" & TEMP_FCS
            OPT_CRC = TEMP_FCS
        End Function


        ''' <summary>
        ''' 十进制转十六进制
        ''' </summary>
        ''' <param name="DEC"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function DEC_TO_HEX(ByVal DEC As Integer) As String
            Dim RET As String = Nothing
            Try
                RET = Convert.ToString(DEC, 16)
            Catch ex As Exception
                RET = Nothing
            End Try
            Return RET
        End Function

        ''' <summary>
        ''' 十六进制转十进制
        ''' </summary>
        ''' <param name="HEX"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function HEX_TO_DEC(ByVal HEX As String) As Integer
            Dim RET As Integer = 0
            Try
                RET = Convert.ToInt32(HEX, 16)
            Catch ex As Exception
                RET = 0
            End Try
            Return RET
        End Function

        ''' <summary>
        ''' 二进制转十进制
        ''' </summary>
        ''' <param name="BIN"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function BIN_TO_DEC(ByVal BIN As String) As Integer
            Dim RET As Integer = 0
            Try
                RET = Convert.ToInt32(BIN, 2)
            Catch ex As Exception
                RET = 0
            End Try
            Return RET
        End Function

        ''' <summary>
        ''' 十进制转二进制
        ''' </summary>
        ''' <param name="DEC"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function DEC_TO_BIN(ByVal DEC As Integer) As String
            Dim RET As String = Nothing
            Try
                RET = Convert.ToString(DEC, 2)
            Catch ex As Exception
                RET = Nothing
            End Try
            Return RET
        End Function

        ''' <summary>
        ''' 十六进制转二进制
        ''' </summary>
        ''' <param name="HEX"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function HEX_TO_BIN(ByVal HEX As String) As String
            Dim RET As String = Nothing
            Try
                RET = Convert.ToString(CType(Convert.ToInt32(HEX, 16), Integer), 2)
            Catch ex As Exception
                RET = Nothing
            End Try
            Return RET
        End Function

        ''' <summary>
        ''' 二进制转十六进制
        ''' </summary>
        ''' <param name="BIN"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function BIN_TO_HEX(ByVal BIN As String) As String
            Dim RET As String = Nothing
            Try
                RET = Convert.ToString(CType(Convert.ToInt32(BIN, 2), Integer), 16)
            Catch ex As Exception
                RET = Nothing
            End Try
            Return RET
        End Function

        ''' <summary>
        ''' 线性校正函数
        ''' </summary>
        ''' <param name="Data1">数据列1</param>
        ''' <param name="Data2">数据列2</param>
        ''' <param name="Correlation">相关性系数</param>
        ''' <param name="Slope">斜率</param>
        ''' <param name="Intercept">截距</param>
        ''' <remarks></remarks>
        Sub Get_Linearity_Correction(ByVal Data1() As Double, ByVal Data2() As Double, ByRef Correlation As Double, ByRef Slope As Double, ByRef Intercept As Double)
            If Data1.Length <> Data2.Length Then
                MessageBoxEx.Show("提供的数据不一致，请检查原始数据！", "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1)
            Else
                Dim AvgX As Double = 0
                Dim AvgY As Double = 0
                Dim SumX As Double = 0
                Dim SumY As Double = 0
                Dim Xy As Double = 0
                Dim X2 As Double = 0
                Dim Y2 As Double = 0
                Dim Len As Integer = Data1.Length
                For i As Integer = 0 To Len - 1
                    SumX += Data1(i)
                    SumY += Data2(i)
                    Xy += Data1(i) * Data2(i)
                    X2 += Data1(i) ^ 2
                    Y2 += Data2(i) ^ 2
                Next
                Slope = (Xy - Len * (SumX / Len) * (SumY / Len)) / ((Y2) - Len * (SumY / Len) ^ 2)
                Intercept = (SumX / Len) - Slope * (SumY / Len)
                Correlation = (Xy / Len - (SumX / Len) * (SumY / Len)) / ((Math.Sqrt(X2 / Len - (SumX / Len) ^ 2)) * (Math.Sqrt(Y2 / Len - (SumY / Len) ^ 2)))
            End If
        End Sub

        ''' <summary>
        ''' 获取标准差，求传入数组的标准差
        ''' </summary>
        ''' <param name="Data">数据列</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function Get_Stdev(ByVal Data() As Double) As Double
            Dim x2, Sum As Double
            Dim N As Integer = Data.Length
            For Each d As Double In Data
                x2 += d * d
                Sum += d
            Next
            Get_Stdev = Math.Sqrt((x2 * N - Sum ^ 2) / (N * (N - 1)))
        End Function

        ''' <summary>
        ''' 获取预警区间，（均值-标准差，均值+标准差）
        ''' </summary>
        ''' <param name="Data">数据列</param>
        ''' <remarks></remarks>
        Sub GetForewarnInterval(ByVal Data() As Double, ByRef Warning_Interval() As Double)
            Array.Resize(Warning_Interval, 2)
            Dim X2, Sum As Double
            Dim N As Integer = Data.Length
            For Each Sp As Double In Data
                X2 += Sp ^ 2
                Sum += Sp
            Next
            Dim a As Double = Sum / N
            Dim b As Double = Math.Sqrt((X2 * N - Sum ^ 2) / (N * (N - 1)))
            Warning_Interval(0) = a - b
            Warning_Interval(1) = a + b
        End Sub

        ''' <summary>
        ''' 获取离散系数，求传入数组的标准差/均值即离散系数。
        ''' </summary>
        ''' <param name="Data"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetCoefficientofVariation(ByVal Data() As Double) As Double
            Dim X2, Sum As Double
            Dim N As Integer = Data.Length
            For Each Sp As Double In Data
                X2 += Sp ^ 2
                Sum += Sp
            Next
            GetCoefficientofVariation = Math.Sqrt((X2 * N - Sum ^ 2) / (N * (N - 1))) / (Sum / N)
        End Function

        ''' <summary>
        '''结束指定进程
        ''' </summary>
        ''' <remarks></remarks>
        Sub KillProcess(ByVal Process_Name As String)
            Try
                If System.Diagnostics.Process.GetProcesses.Length > 0 Then '结束指定进程
                    ProcessName = Process.GetProcesses
                    For I = 0 To ProcessName.Length - 1
                        If ProcessName(I).ProcessName.ToUpper.IndexOf(Process_Name.ToUpper) >= 0 Then
                            ProcessName(I).Kill()
                        End If
                    Next
                End If
            Catch
            End Try
        End Sub

        ''' <summary>
        ''' 保存Excel数据
        ''' </summary>
        ''' <param name="DataGridView_Test_Data">要保存的数据表</param>
        ''' <param name="Excel_File_Dir">保存路径</param>
        ''' <remarks></remarks>
        Sub Save_Excel(ByVal DataGridView_Test_Data As DevComponents.DotNetBar.Controls.DataGridViewX, ByVal Excel_File_Dir As String, ByVal Data_Format As Save_Data_Format)
            Try
                Select Case Data_Format
                    Case Save_Data_Format.XLS
                        Dim ColumnCount_Number As Integer = DataGridView_Test_Data.ColumnCount
                        Dim RowCount_Number As Integer = DataGridView_Test_Data.RowCount
                        Dim Save_data_Array(RowCount_Number - 1, ColumnCount_Number - 1) As String
                        For a = 0 To ColumnCount_Number - 1
                            Save_data_Array(0, a) = DataGridView_Test_Data.Columns(a).HeaderText
                        Next
                        For i = 0 To RowCount_Number - 2
                            For j = 0 To ColumnCount_Number - 1
                                Save_data_Array(i + 1, j) = DataGridView_Test_Data.Item(j, i).Value.ToString.Trim
                            Next
                        Next
                        Xlsbook = Xls.Application.Workbooks.Add
                        Xlssheet = Xlsbook.Sheets(1)
                        Xlssheet.Range("A1").Resize(RowCount_Number, ColumnCount_Number).Value = Save_data_Array
                        'Xlssheet.Range("A1").Font = Microsoft.Office.Interop.Excel.Font
                        Xlsbook.SaveAs(Excel_File_Dir, Excel.XlFileFormat.xlExcel7)
                        Xlsbook.Close()
                        If Xls IsNot Nothing Then Xls.Quit()
                        MessageBoxEx.Show("数据保存完成！", "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1)
                    Case Save_Data_Format.CSV
                        Dim ColumnCount_Number As Integer = DataGridView_Test_Data.ColumnCount
                        Dim RowCount_Number As Integer = DataGridView_Test_Data.RowCount
                        Dim Save_data_Array(RowCount_Number - 1, ColumnCount_Number - 1) As String
                        For a = 0 To ColumnCount_Number - 1
                            Save_data_Array(0, a) = DataGridView_Test_Data.Columns(a).HeaderText
                        Next
                        For i = 0 To RowCount_Number - 2
                            For j = 0 To ColumnCount_Number - 1
                                Save_data_Array(i + 1, j) = DataGridView_Test_Data.Item(j, i).Value.ToString.Trim
                            Next
                        Next
                        Xlsbook = Xls.Application.Workbooks.Add
                        Xlssheet = Xlsbook.Sheets(1)
                        Xlssheet.Range("A1").Resize(RowCount_Number, ColumnCount_Number).Value = Save_data_Array
                        Xlsbook.SaveAs(Excel_File_Dir, Excel.XlFileFormat.xlCSV)
                        Xlsbook.Close()
                        If Xls IsNot Nothing Then Xls.Quit()
                        MessageBoxEx.Show("数据保存完成！", "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1)
                    Case Save_Data_Format.XLSX
                        Dim ColumnCount_Number As Integer = DataGridView_Test_Data.ColumnCount
                        Dim RowCount_Number As Integer = DataGridView_Test_Data.RowCount
                        Dim Save_data_Array(RowCount_Number - 1, ColumnCount_Number - 1) As String
                        For a = 0 To ColumnCount_Number - 1
                            Save_data_Array(0, a) = DataGridView_Test_Data.Columns(a).HeaderText
                        Next
                        For i = 0 To RowCount_Number - 2
                            For j = 0 To ColumnCount_Number - 1
                                Save_data_Array(i + 1, j) = DataGridView_Test_Data.Item(j, i).Value.ToString.Trim
                            Next
                        Next
                        Xlsbook = Xls.Application.Workbooks.Add
                        Xlssheet = Xlsbook.Sheets(1)
                        Xlssheet.Range("A1").Resize(RowCount_Number, ColumnCount_Number).Value = Save_data_Array
                        Xlsbook.SaveAs(Excel_File_Dir, Excel.XlFileFormat.xlExcel12)
                        Xlsbook.Close()
                        If Xls IsNot Nothing Then Xls.Quit()
                        MessageBoxEx.Show("数据保存完成！", "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1)
                End Select
             
            Catch ex As Exception
                MessageBoxEx.Show("数据保存到Excel出现问题，此问题可以忽略，以下是详细错误消息：" & vbCrLf & ex.Message, "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
            End Try
        End Sub

        ''' <summary>
        ''' 保存Excel数据
        ''' </summary>
        ''' <param name="DataGridView_Test_Data">要保存的数据表</param>
        ''' <param name="Excel_File_Dir">保存路径</param>
        ''' <remarks></remarks>
        Sub Save_Excel(ByVal DataGridView_Test_Data As Windows.Forms.DataGridView, ByVal Excel_File_Dir As String, ByVal Data_Format As Save_Data_Format)
            Try
                Select Case Data_Format
                    Case Save_Data_Format.XLS
                        Dim ColumnCount_Number As Integer = DataGridView_Test_Data.ColumnCount
                        Dim RowCount_Number As Integer = DataGridView_Test_Data.RowCount
                        Dim Save_data_Array(RowCount_Number - 1, ColumnCount_Number - 1) As String
                        For a = 0 To ColumnCount_Number - 1
                            Save_data_Array(0, a) = DataGridView_Test_Data.Columns(a).HeaderText
                        Next
                        For i = 0 To RowCount_Number - 2
                            For j = 0 To ColumnCount_Number - 1
                                Save_data_Array(i + 1, j) = DataGridView_Test_Data.Item(j, i).Value.ToString.Trim
                            Next
                        Next
                        Xlsbook = Xls.Application.Workbooks.Add
                        Xlssheet = Xlsbook.Sheets(1)
                        Xlssheet.Range("A1").Resize(RowCount_Number, ColumnCount_Number).Value = Save_data_Array
                        Xlsbook.SaveAs(Excel_File_Dir, Excel.XlFileFormat.xlExcel8)
                        Xlsbook.Close()
                        If Xls IsNot Nothing Then Xls.Quit()
                        MessageBoxEx.Show("数据保存完成！", "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1)
                    Case Save_Data_Format.CSV
                        Dim ColumnCount_Number As Integer = DataGridView_Test_Data.ColumnCount
                        Dim RowCount_Number As Integer = DataGridView_Test_Data.RowCount
                        Dim Save_data_Array(RowCount_Number - 1, ColumnCount_Number - 1) As String
                        For a = 0 To ColumnCount_Number - 1
                            Save_data_Array(0, a) = DataGridView_Test_Data.Columns(a).HeaderText
                        Next
                        For i = 0 To RowCount_Number - 2
                            For j = 0 To ColumnCount_Number - 1
                                Save_data_Array(i + 1, j) = DataGridView_Test_Data.Item(j, i).Value.ToString.Trim
                            Next
                        Next
                        Xlsbook = Xls.Application.Workbooks.Add
                        Xlssheet = Xlsbook.Sheets(1)
                        Xlssheet.Range("A1").Resize(RowCount_Number, ColumnCount_Number).Value = Save_data_Array
                        Xlsbook.SaveAs(Excel_File_Dir, Excel.XlFileFormat.xlCSV)
                        Xlsbook.Close()
                        If Xls IsNot Nothing Then Xls.Quit()
                        MessageBoxEx.Show("数据保存完成！", "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1)
                    Case Save_Data_Format.XLSX
                        Dim ColumnCount_Number As Integer = DataGridView_Test_Data.ColumnCount
                        Dim RowCount_Number As Integer = DataGridView_Test_Data.RowCount
                        Dim Save_data_Array(RowCount_Number - 1, ColumnCount_Number - 1) As String
                        For a = 0 To ColumnCount_Number - 1
                            Save_data_Array(0, a) = DataGridView_Test_Data.Columns(a).HeaderText
                        Next
                        For i = 0 To RowCount_Number - 2
                            For j = 0 To ColumnCount_Number - 1
                                Save_data_Array(i + 1, j) = DataGridView_Test_Data.Item(j, i).Value.ToString.Trim
                            Next
                        Next
                        Xlsbook = Xls.Application.Workbooks.Add
                        Xlssheet = Xlsbook.Sheets(1)
                        Xlssheet.Range("A1").Resize(RowCount_Number, ColumnCount_Number).Value = Save_data_Array
                        Xlsbook.SaveAs(Excel_File_Dir, Excel.XlFileFormat.xlExcel12)
                        Xlsbook.Close()
                        If Xls IsNot Nothing Then Xls.Quit()
                        MessageBoxEx.Show("数据保存完成！", "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1)
                End Select
            Catch ex As Exception
                MessageBoxEx.Show("数据保存到Excel出现问题，此问题可以忽略，以下是详细错误消息：" & vbCrLf & ex.Message, "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
            End Try
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

        ''' <summary>
        ''' 讀取硬盤序列號
        ''' </summary>
        ''' <param name="ReadHd">硬盤序列號</param>
        ''' <remarks></remarks>
        Public Sub HDID(Optional ByRef ReadHd As String = "")
            ReadHd = ""
            Dim info As ManagementBaseObject
            Dim query As New SelectQuery("Win32_DiskDrive")
            Dim search As New ManagementObjectSearcher(query)
            For Each info In search.Get()
                If info("Model") IsNot Nothing Then
                    ReadHd = info("Model").ToString
                Else
                    ReadHd = 0
                End If
            Next
        End Sub

        ''' <summary>
        ''' 讀取主板序列號
        ''' </summary>
        ''' <param name="ReadMb">主板序列號</param>
        ''' <remarks></remarks>
        Sub MBID(Optional ByRef ReadMb As String = "")
            Dim info As ManagementBaseObject
            Dim query As New SelectQuery("Win32_BaseBoard")
            Dim search As New ManagementObjectSearcher("Select * FROM Win32_BaseBoard")
            For Each info In search.Get
                If info("Product") IsNot Nothing Then
                    ReadMb = info("Product").ToString
                Else
                    ReadMb = 0
                End If
            Next
        End Sub

        ''' <summary>
        ''' 讀取CPU序列號
        ''' </summary>
        ''' <param name="ReadCP">CPU序列號</param>
        ''' <remarks></remarks>
        Public Sub CPID(Optional ByRef ReadCP As String = "")
            Dim info As ManagementBaseObject
            Dim query As New SelectQuery("Win32_Processor")
            Dim search As New ManagementObjectSearcher(query)
            For Each info In search.Get
                If info("ProcessorId") IsNot Nothing Then
                Else
                    ReadCP = 0
                End If
            Next
        End Sub

        Public Sub MCID(Optional ByRef MACADD() As String = Nothing)
            Dim query As System.Management.ManagementObjectSearcher = New System.Management.ManagementObjectSearcher("SELECT * FROM Win32_NetworkAdapterConfiguration")
            Dim queryCollection As System.Management.ManagementObjectCollection = query.Get()
            Dim mo As New System.Management.ManagementObject
            Dim TEMPI As Integer = 0
            For Each mo In queryCollection
                If IsDBNull(mo.Item("macaddress")) = False Then
                    If mo.Item("macaddress") <> "" Then
                        TEMPI = TEMPI + 1
                        Array.Resize(MACADD, TEMPI)
                        MACADD(TEMPI - 1) = mo.Item("macaddress")
                    End If
                End If
            Next
        End Sub

    End Class

    Public Class ADLINK_LIB

        Class MOTION
            Const IDX_DLL = (0)
            Const IDX_DRIVER = (1)
            Const IDX_KERNEL = (2)
            Const IDX_FIRMWARE = (3)
            Const IDX_PCB = (4)
            Const IDX_MAX = (5)
            Const MAX_DO_CH = (24)
            Const MAX_DI_CH = (24)
            Const Dll_Version = &H0
            Const Driver_Version = &H10
            Const Dsp_Kernel_Version = &H40
            Const Fpga_Version = &H21
            Const Pcb_Version = &H30
            Const MAXHEXLIX = &H3 'Helix axes is 3.
            Const MAXARC3 = &H3 'ARC3 axes is 3.
            Const MAXARC2 = &H2 'ARC2 axes is 2.
            Const MAX_SAMPL_CH = (8)
            Const MAX_SAMPL_SRC = (2)
            Dim axisMagnification() As MAGNIFICATION
            Dim DATA_CONVER As New DATA_CONVERSION_LIB

            Structure MAGNIFICATION
                Dim AXIS_MAGNIFICATION As Double
            End Structure

            Structure STR_SAMP_DATA_8CH
                Dim tick As Int32
                Dim data_0 As Int32 'Total channel = 4
                Dim data_1 As Int32 'Total channel = 4
                Dim data_2 As Int32 'Total channel = 4
                Dim data_3 As Int32 'Total channel = 4
                Dim data_4 As Int32 'Total channel = 4
                Dim data_5 As Int32 'Total channel = 4
                Dim data_6 As Int32 'Total channel = 4
                Dim data_7 As Int32 'Total channel = 4
            End Structure

            Structure POINT_DATA
                Dim i32_pos As Integer ' Position data  Lib "APS168x64.dll" (relative or absolute)  Lib "APS168x64.dll" (pulse)
                Dim i16_accType As Short ' Acceleration pattern 0: T-curve,  1: S-curve
                Dim i16_decType As Short ' Deceleration pattern 0: T-curve,  1: S-curve
                Dim i32_acc As Integer ' Acceleration rate  Lib "APS168x64.dll" ( pulse / ss )
                Dim i32_dec As Integer ' Deceleration rate  Lib "APS168x64.dll" ( pulse / ss )
                Dim i32_initSpeed As Integer ' Start velocity         Lib "APS168x64.dll" ( pulse / s )
                Dim i32_maxSpeed As Integer ' Maximum velocity   Lib "APS168x64.dll" ( pulse / s )
                Dim i32_endSpeed As Integer ' End velocity           Lib "APS168x64.dll" ( pulse / s )
                Dim i32_angle As Integer ' Arc move angle     Lib "APS168x64.dll" ( degree, -360 ~ 360 )
                Dim u32_dwell As Integer ' Dwell times        Lib "APS168x64.dll" ( unit: ms )
                Dim i32_opt As Integer ' Option '0xABCD , D:0 absolute, 1:relative
            End Structure

            Structure POINT_DATA2
                Dim i32_pos_0 As Integer ' Position data  Lib "APS168x64.dll" (relative or absolute)  Lib "APS168x64.dll" (pulse) , Arraysize = 16
                Dim i32_pos_1 As Integer ' Position data  Lib "APS168x64.dll" (relative or absolute)  Lib "APS168x64.dll" (pulse) , Arraysize = 16
                Dim i32_pos_2 As Integer ' Position data  Lib "APS168x64.dll" (relative or absolute)  Lib "APS168x64.dll" (pulse) , Arraysize = 16
                Dim i32_pos_3 As Integer ' Position data  Lib "APS168x64.dll" (relative or absolute)  Lib "APS168x64.dll" (pulse) , Arraysize = 16
                Dim i32_pos_4 As Integer ' Position data  Lib "APS168x64.dll" (relative or absolute)  Lib "APS168x64.dll" (pulse) , Arraysize = 16
                Dim i32_pos_5 As Integer ' Position data  Lib "APS168x64.dll" (relative or absolute)  Lib "APS168x64.dll" (pulse) , Arraysize = 16
                Dim i32_pos_6 As Integer ' Position data  Lib "APS168x64.dll" (relative or absolute)  Lib "APS168x64.dll" (pulse) , Arraysize = 16
                Dim i32_pos_7 As Integer ' Position data  Lib "APS168x64.dll" (relative or absolute)  Lib "APS168x64.dll" (pulse) , Arraysize = 16
                Dim i32_pos_8 As Integer ' Position data  Lib "APS168x64.dll" (relative or absolute)  Lib "APS168x64.dll" (pulse) , Arraysize = 16
                Dim i32_pos_9 As Integer ' Position data  Lib "APS168x64.dll" (relative or absolute)  Lib "APS168x64.dll" (pulse) , Arraysize = 16
                Dim i32_pos_10 As Integer ' Position data  Lib "APS168x64.dll" (relative or absolute)  Lib "APS168x64.dll" (pulse) , Arraysize = 16
                Dim i32_pos_11 As Integer ' Position data  Lib "APS168x64.dll" (relative or absolute)  Lib "APS168x64.dll" (pulse) , Arraysize = 16
                Dim i32_pos_12 As Integer ' Position data  Lib "APS168x64.dll" (relative or absolute)  Lib "APS168x64.dll" (pulse) , Arraysize = 16
                Dim i32_pos_13 As Integer ' Position data  Lib "APS168x64.dll" (relative or absolute)  Lib "APS168x64.dll" (pulse) , Arraysize = 16
                Dim i32_pos_14 As Integer ' Position data  Lib "APS168x64.dll" (relative or absolute)  Lib "APS168x64.dll" (pulse) , Arraysize = 16
                Dim i32_pos_15 As Integer ' Position data  Lib "APS168x64.dll" (relative or absolute)  Lib "APS168x64.dll" (pulse) , Arraysize = 16
                Dim i32_initSpeed As Integer ' Start velocity         Lib "APS168x64.dll" ( pulse / s )
                Dim i32_maxSpeed As Integer ' Maximum velocity   Lib "APS168x64.dll" ( pulse / s )
                Dim i32_angle As Integer ' Arc move angle     Lib "APS168x64.dll" ( degree, -360 ~ 360 )
                Dim u32_dwell As Integer ' Dwell times        Lib "APS168x64.dll" ( unit: ms )
                Dim i32_opt As Integer ' Option '0xABCD , D:0 absolute, 1:relative
            End Structure

            Structure JOG_DATA
                Dim i16_jogMode As Short ' Jog mode. 0:Free running mode, 1:Step mode
                Dim i16_dir As Short ' Jog direction. 0:positive, 1:negative direction
                Dim i16_accType As Short ' Acceleration pattern 0: T-curve,  1: S-curve
                Dim i32_acc As Integer ' Acceleration rate  Lib "APS168x64.dll" ( pulse / ss )
                Dim i32_dec As Integer ' Deceleration rate  Lib "APS168x64.dll" ( pulse / ss )
                Dim i32_maxSpeed As Integer ' Positive value, maximum velocity   Lib "APS168x64.dll" ( pulse / s )
                Dim i32_offset As Integer ' Positive value, a step  Lib "APS168x64.dll" (pulse)
                Dim i32_delayTime As Integer ' Delay time,  Lib "APS168x64.dll" ( range: 0 ~ 65535 millisecond, align by cycle time)
            End Structure

            Structure PNT_DATA_2D
                Dim u32_opt As Integer        ' option, [0x00000000,0xFFFFFFFF]
                Dim i32_x As Integer          ' x-axis component  Lib "APS168x64.dll" (pulse), [-2147483648,2147484647]
                Dim i32_y As Integer          ' y-axis component  Lib "APS168x64.dll" (pulse), [-2147483648,2147484647]
                Dim i32_theta As Integer      ' x-y plane arc move angle  Lib "APS168x64.dll" (0.001 degree), [-360000,360000]
                Dim i32_acc As Integer        ' acceleration rate  Lib "APS168x64.dll" (pulse/ss), [0,2147484647]
                Dim i32_dec As Integer        ' deceleration rate  Lib "APS168x64.dll" (pulse/ss), [0,2147484647]
                Dim i32_vi As Integer         ' initial velocity  Lib "APS168x64.dll" (pulse/s), [0,2147484647]
                Dim i32_vm As Integer         ' maximum velocity  Lib "APS168x64.dll" (pulse/s), [0,2147484647]
                Dim i32_ve As Integer         ' ending velocity  Lib "APS168x64.dll" (pulse/s), [0,2147484647]
            End Structure

            Structure PNT_DATA_2D_F64
                Dim u32_opt As Integer        '// option, [0x00000000,0xFFFFFFFF]
                Dim f64_x As Double          '// x-axis component (pulse), [-2147483648,2147484647]
                Dim f64_y As Double          '// y-axis component (pulse), [-2147483648,2147484647]
                Dim f64_theta As Double      '// x-y plane arc move angle (0.000001 degree), [-360000,360000]
                Dim f64_acc As Double        '// acceleration rate (pulse/ss), [0,2147484647]
                Dim f64_dec As Double        '// deceleration rate (pulse/ss), [0,2147484647]
                Dim f64_vi As Double         '// initial velocity (pulse/s), [0,2147484647]
                Dim f64_vm As Double         '// maximum velocity (pulse/s), [0,2147484647]
                Dim f64_ve As Double         '// ending velocity (pulse/s), [0,2147484647]
                Dim f64_sf As Double             ' // s-factor [0.0 ~ 1.0]
            End Structure

            ' Point table structure  Lib "APS168x64.dll" (Four dimension)
            Structure PNT_DATA_4DL
                Dim u32_opt As Integer        ' option, [0x00000000,0xFFFFFFFF]
                Dim i32_x As Integer          ' x-axis component  Lib "APS168x64.dll" (pulse), [-2147483648,2147484647]
                Dim i32_y As Integer          ' y-axis component  Lib "APS168x64.dll" (pulse), [-2147483648,2147484647]
                Dim i32_z As Integer          ' z-axis component  Lib "APS168x64.dll" (pulse), [-2147483648,2147484647]
                Dim i32_u As Integer          ' u-axis component  Lib "APS168x64.dll" (pulse), [-2147483648,2147484647]
                Dim i32_acc As Integer        ' acceleration rate  Lib "APS168x64.dll" (pulse/ss), [0,2147484647]
                Dim i32_dec As Integer        ' deceleration rate  Lib "APS168x64.dll" (pulse/ss), [0,2147484647]
                Dim i32_vi As Integer         ' initial velocity  Lib "APS168x64.dll" (pulse/s), [0,2147484647]
                Dim i32_vm As Integer         ' maximum velocity  Lib "APS168x64.dll" (pulse/s), [0,2147484647]
                Dim i32_ve As Integer         ' ending velocity  Lib "APS168x64.dll" (pulse/s), [0,2147484647]
            End Structure

            ' Point table structure  Lib "APS168x64.dll" (One dimension)
            Structure PNT_DATA
                Dim u32_opt As Integer        ' option, [0x00000000,0xFFFFFFFF]
                Dim i32_x As Integer          ' x-axis component  Lib "APS168x64.dll" (pulse), [-2147483648,2147484647]
                Dim i32_theta As Integer      ' x-y plane arc move angle  Lib "APS168x64.dll" (0.001 degree), [-360000,360000]
                Dim i32_acc As Integer        ' acceleration rate  Lib "APS168x64.dll" (pulse/ss), [0,2147484647]
                Dim i32_dec As Integer        ' deceleration rate  Lib "APS168x64.dll" (pulse/ss), [0,2147484647]
                Dim i32_vi As Integer         ' initial velocity  Lib "APS168x64.dll" (pulse/s), [0,2147484647]
                Dim i32_vm As Integer         ' maximum velocity  Lib "APS168x64.dll" (pulse/s), [0,2147484647]
                Dim i32_ve As Integer         ' ending velocity  Lib "APS168x64.dll" (pulse/s), [0,2147484647]
            End Structure

            Structure ASYNCALL
                Dim h_event As IntPtr
                Dim i32_ret As Integer
            End Structure

            Structure TSK_INFO
                Dim State As UShort
                Dim RunTimeErr As UShort
                Dim IP As UShort
                Dim SP As UShort
                Dim BP As UShort
                Dim MsgQueueSts As UShort
            End Structure

            Structure SAMP_PARAM
                Dim rate As Integer             '//Sampling rate
                Dim edge As Integer             '//Trigger edge
                Dim level As Integer            '//Trigger level
                Dim trigCh As Integer           '//Trigger channel
                Dim sourceByCh_0_0 As Integer
                Dim sourceByCh_0_1 As Integer
                Dim sourceByCh_1_0 As Integer
                Dim sourceByCh_1_1 As Integer
                Dim sourceByCh_2_0 As Integer
                Dim sourceByCh_2_1 As Integer
                Dim sourceByCh_3_0 As Integer
                Dim sourceByCh_3_1 As Integer
                Dim sourceByCh_4_0 As Integer
                Dim sourceByCh_4_1 As Integer
                Dim sourceByCh_5_0 As Integer
                Dim sourceByCh_5_1 As Integer
                Dim sourceByCh_6_0 As Integer
                Dim sourceByCh_6_1 As Integer
                Dim sourceByCh_7_0 As Integer
                Dim sourceByCh_7_1 As Integer

            End Structure

            Const MAX_PT_DIM = (6)

            Structure PTINFO
                Dim Dimension As Integer
                Dim AxisArr_0 As Integer
                Dim AxisArr_1 As Integer
                Dim AxisArr_2 As Integer
                Dim AxisArr_3 As Integer
                Dim AxisArr_4 As Integer
                Dim AxisArr_5 As Integer
            End Structure

            Structure PTDWL
                Dim DwTime As Double '//Unit is ms
            End Structure

            Structure PTLINE
                Dim Dimension As Integer
                Dim Pos_0 As Double
                Dim Pos_1 As Double
                Dim Pos_2 As Double
                Dim Pos_3 As Double
                Dim Pos_4 As Double
                Dim Pos_5 As Double
                Dim vs As Double
                Dim vm As Double
                Dim ve As Double
            End Structure

            Structure PTA2CA
                'Dim Index(MAXARC2) As Byte 'Index X,Y
                Dim Index_0 As Byte
                Dim Index_1 As Byte
                'Dim Center#(MAXARC2) As Double 'Center Arr
                Dim Center_0 As Double
                Dim Center_1 As Double
                Dim Angle As Double 'Angle
            End Structure

            Structure PTA2CE
                'Dim Index(MAXARC2) As Byte 'Index X,Y
                Dim index_0 As Byte
                Dim index_1 As Byte
                'Dim Center#(MAXARC2) As Double
                Dim Center_0 As Double
                Dim Center_1 As Double
                'Dim End_pos(MAXARC2) As Double
                Dim End_pos_0 As Double
                Dim End_pos_1 As Double
                Dim Dir As Short
            End Structure

            Structure PTA3CA
                Dim Index_0 As Byte
                Dim Index_1 As Byte
                Dim Index_2 As Byte
                Dim Center_0 As Double
                Dim Center_1 As Double
                Dim Center_2 As Double
                Dim Noraml_0 As Double
                Dim Noraml_1 As Double
                Dim Noraml_2 As Double
                Dim Angle As Double 'Angle
            End Structure

            Structure PTA3CE
                Dim index_0 As Byte
                Dim index_1 As Byte
                Dim index_2 As Byte
                Dim Center_0 As Double
                Dim Center_1 As Double
                Dim Center_2 As Double
                Dim End_pos_0 As Double
                Dim End_pos_1 As Double
                Dim End_pos_2 As Double
                Dim Dir As Short
            End Structure

            Structure PTHCA
                Dim Index_0 As Byte
                Dim Index_1 As Byte
                Dim Index_2 As Byte
                Dim Center_0 As Double
                Dim Center_1 As Double
                Dim Center_2 As Double
                Dim Noraml_0 As Double
                Dim Noraml_1 As Double
                Dim Noraml_2 As Double
                Dim Angle As Double 'Angle
                Dim DeltaH As Double
                Dim FinalR As Double
            End Structure

            Structure PTHCE
                Dim Index_0 As Byte
                Dim Index_1 As Byte
                Dim Index_2 As Byte
                Dim Center_0 As Double
                Dim Center_1 As Double
                Dim Center_2 As Double
                Dim Noraml_0 As Double
                Dim Noraml_1 As Double
                Dim Noraml_2 As Double
                Dim End_pos_0 As Double
                Dim End_pos_1 As Double
                Dim End_pos_2 As Double
                Dim Dir As Short
            End Structure

            Structure PTSTS
                Dim BitSts As UShort
                Dim PntBufFreeSpace As UShort
                Dim PntBufUsageSpace As UShort
                Dim RunningCnt As UInteger
            End Structure

            Enum PWM_STRU
                DISABLE = 0
                PWM_CH0 = 1
                PWM_CH1 = 2
            End Enum

            Enum PAR_STOP_CODE_ENUM
                STOP_NORMAL = 0
                STOP_EMG = 1
                STOP_ALM = 2
                STOP_SVNO = 3
                STOP_PEL = 4
                STOP_MEL = 5
                STOP_SPEL = 6
                STOP_SMEL = 7
                STOP_USER_EMG = 8
                STOP_USER = 9
                STOP_GAN_L1 = 10
                STOP_GAN_L2 = 11
                STOP_GEAR_SLAVE = 12
                STOP_ERROR_LEVEL = 13
                STOP_DI = 14
            End Enum

            Enum PAR_ERROR_CODE_ENUM
                ''' <summary>
                ''' 无错误
                ''' </summary>
                ''' <remarks></remarks>
                ERR_NoError = 0
                ERR_OSVersion = -1
                ERR_OpenDriverFailed = -2
                ERR_InsufficientMemory = -3
                ERR_DeviceNotInitial = -4
                ERR_NoDeviceFound = -5
                ERR_CardIdDuplicate = -6
                ERR_DeviceAlreadyIntialed = -7
                ERR_InterruptNotEnable = -8
                ERR_TimeOut = -9
                ERR_ParametersInvaild = -10
                ERR_SetEEPROM = -11
            End Enum

            Enum JOG_PARAMETER_ENUM
                PRA_JG_MODE = &H40
                PRA_JG_DIR = &H41
                PRA_JG_SF = &H42
                PRA_JG_ACC = &H43
                PRA_JG_DEC = &H44
                PRA_JG_VM = &H45
                PRA_JG_OFFSET = &H46
                PRA_JG_DELAY = &H47
                PRA_JG_MAP_DI_EN = &H48
                PRA_JG_P_JOG_DI = &H49
                PRA_JG_N_JOG_DI = &H4A
                PRA_JG_JOG_DI = &H4B
            End Enum

            Enum JOG_SWITCH_ENUM
                JOG_OFF = 0
                JOG_ON = 1
            End Enum

            Enum PAR_AMETER_TABLE_ENUM
                INIT_AUTO_CARD_ID = &H0S           '    ' (Bit 0) CardId assigned by system, Input parameter of APS_initial( cardId, "MODE" )
                INIT_MANUAL_ID = &H1S              '    ' (Bit 0) CardId manual by dip switch, Input parameter of APS_initial( cardId, "MODE" )
                INIT_PARAM_IGNORE = &H0S           '    ' (Bit 4-5) Load parameter method - ignore, keep current value
                INIT_PARAM_LOAD_DEFAULT = &H10S    '    ' (Bit 4-5) Load parameter method - load parameter as default value 
                INIT_PARAM_LOAD_FLASH = &H20S      '    ' (Bit 4-5) Load parameter method - load parameter from flash memory
                INIT_MNET_INTERRUPT = &H40         ' (Bit 6) Enable MNET interrupt mode. (Support motion interrupt for MotionNet series)
                ''' <summary>
                ''' 急停信号
                ''' </summary>
                ''' <remarks></remarks>
                PRB_EMG_LOGIC = &H0S ' Board EMG logic
                ''' <summary>
                ''' 输出信号
                ''' </summary>
                ''' <remarks></remarks>
                PRB_DO_LOGIC = &H14S '//DO logic, 0: no invert; 1: invert  
                ''' <summary>
                ''' 输入信号
                ''' </summary>
                ''' <remarks></remarks>
                PRB_DI_LOGIC = &H15S     '//DI logic, 0: no invert; 1: invert
                MHS_GET_SERVO_OFF_INFO = &H16S
                MHS_RESET_SERVO_OFF_INFO = &H17S
                MHS_GET_ALL_STATE = &H18S
                PRB_WDT0_VALUE = &H10S ' Set / Get watch dog limit.
                PRB_WDT0_COUNTER = &H11S ' Reset Wdt / Get Wdt_Count_Value
                PRB_WDT0_UNIT = &H12S ' wdt_unit
                PRB_WDT0_ACTION = &H13S ' wdt_action
                PRA_HOME_VA = &H16S  'homing approach velocity [PCI-8253/56 only]
                ''' <summary>
                ''' 原点偏移距离
                ''' </summary>
                ''' <remarks></remarks>
                PRA_HOME_SHIFT = &H17S ' The shift from ORG [PCI-8254/58 only]
                ''' <summary>
                ''' 归零EZ信号
                ''' </summary>
                ''' <remarks></remarks>
                PRA_HOME_EZA = &H18S ' EZ alignment enable

                ''' <summary>
                ''' 离开原点速度
                ''' </summary>
                ''' <remarks></remarks>
                PRA_HOME_VO = &H19 ' Homing leave ORG velocity
                PRA_HOME_OFFSET = &H1AS ' The escape pulse amounts(Leaving home by position)
                PRA_HOME_POS = &H1BS 'The position from ORG [PCI-8254/58 only]
                PRB_TMR0_BASE = &H20S ' Set TMR Value
                PRB_TMR0_VALUE = &H21S ' Get timer int count value
                ''' <summary>
                ''' 曲线加速系数
                ''' </summary>
                ''' <remarks></remarks>
                PRA_CURVE = &H20S '// Move curve pattern
                ''' <summary>
                ''' 平滑系数
                ''' </summary>
                ''' <remarks></remarks>
                PRA_SF = &H20S '// Move s-factor
                ''' <summary>
                ''' 加速度
                ''' </summary>
                ''' <remarks></remarks>
                PRA_ACC = &H21S '// Move acceleration
                ''' <summary>
                ''' 减速度
                ''' </summary>
                ''' <remarks></remarks>
                PRA_DEC = &H22S '// Move deceleration
                ''' <summary>
                ''' 开始速度
                ''' </summary>
                ''' <remarks></remarks>
                PRA_VS = &H23S '// Move start velocity
                ''' <summary>
                ''' 最大速度
                ''' </summary>
                ''' <remarks></remarks>
                PRA_VM = &H24S '// Move max velocity
                ''' <summary>
                ''' 终点速度
                ''' </summary>
                ''' <remarks></remarks>
                PRA_VE = &H25S '// Move end velocity
                PRA_SACC = &H26S '// S curve acceleration
                PRA_SDEC = &H27S '// S curve deceleration
                PRA_ACC_SR = &H28S '// S curve ratio in acceleration( S curve with linear acceleration)
                PRA_DEC_SR = &H29S '// S curve ratio in deceleration( S curve with linear deceleration)
                PRA_PRE_EVENT_DIST = &H2AS 'Pre-event distance
                PRA_POST_EVENT_DIST = &H2BS 'Post-event distance
                PRA_DIST = &H30S  '// Move distance
                PRA_MAX_VELOCITY = &H31S  '// Maximum velocity
                PRA_SCUR_PERCENTAGE = &H32S  '// Scurve percentage
                PRA_MagnificationENDING_MODE = &H33S  '// Blending mode
                PRA_STOP_MODE = &H34S  '// Stop mode
                PRA_STOP_DELRATE = &H35S  '// Stop function deceleration rate
                PRA_PT_STOP_ENDO = &H32S  '// Disable do when point table stopping.
                PRA_PT_STP_DO_EN = &H32S  '// Disable do when point table stopping.
                PRA_PT_STOP_DO = &H33S  '// Set do value when point table stopping.
                PRA_PT_STP_DO = &H33S  '// Set do value when point table stopping.
                PRA_PWM_OFF = &H34S  '// Disable specified PWM output when ASTP input signal is active.
                PRA_DO_OFF = &H35S  '// Set DO value when ASTP input signal is active.
                PRB_SYS_TMP_MONITOR = &H30S ' Get system temperature monitor data
                PRB_CPU_TMP_MONITOR = &H31S ' Get CPU temperature monitor data
                PRB_AUX_TMP_MONITOR = &H32S ' Get AUX temperature monitor data
                PRB_UART_MULTIPLIER = &H40S ' Set UART Multiplier
                PRA_JG_STEP = &H46S '// Jog offset (For step mode)
                PRA_JG_DELAY = &H47S '// Jog delay (For step mode)
                PRA_JG_MAP_DI_EN = &H48 '// (I32) Enable Digital input map to jog command signal
                PRA_JG_P_JOG_DI = &H49S '// (I32) Mapping configuration for positive jog and digital input.
                PRA_JG_N_JOG_DI = &H4AS '// (I32) Mapping configuration for negative jog and digital input.
                PRA_JG_JOG_DI = &H4BS '// (I32) Mapping configuration for jog and digital input.
                PRA_MDN_DELAY = &H50S '// NSTP delay setting
                PRA_SINP_WDW = &H51S '// Soft INP window setting
                PRA_SINP_STBL = &H52S '// Soft INP stable cycle
                PRA_SINP_STBT = &H52S '// Soft INP stable cycle
                PRA_SERVO_LOGIC = &H53S '//  SERVO logic
                PRA_GEAR_MASTER = &H60S '// (I32) Select gearing master
                PRA_GEAR_ENGAGE_RATE = &H61S '// (F64) Gear engage rate
                PRA_GEAR_RATIO = &H62S '// (F64) Gear ratio
                PRA_GANTRY_PROTECT_1 = &H63S '// (F64) E-gear gantry mode protection level 1
                PRA_GANTRY_PROTECT_2 = &H64S '// (F64) E-gear gantry mode protection level 2
                PRA_MOVE_RATIO = &H88S       '//Move ratio
                PRA_ENCODER_DIR = &H85S '(I32) 
                PRA_POS_UNIT_FACTOR = &H86S '(F64) position unit factor setting
                PRB_PSR_MODE = &H90S ' Config pulser mode
                PRB_PSR_EA_LOGIC = &H91S ' Set EA inverted
                PRB_PSR_EB_LOGIC = &H92S ' Set EB inverted
                PRB_DENOMINATOR = &H80S ' Floating number denominator
                PRB_PSR_ENABLE = &H91S ' Enable/disable pulser mode
                PRB_BOOT_SETTING = &H100S ' Load motion parameter method when DSP boot
                PRB_PWM0_MAP_DO = &H110S  '// Enable & Map PWM0 to Do channels
                PRB_PWM1_MAP_DO = &H111S  '// Enable & Map PWM1 to Do channels
                PRB_PWM2_MAP_DO = &H112S  '// Enable & Map PWM2 to Do channels
                PRB_PWM3_MAP_DO = &H113S  '// Enable & Map PWM3 to Do channels
                PRA_D_SAMPLE_TIME = &H12CS '(I32) Derivative Sample Time
                PRA_BIQUAD0_A1 = &H132S '(F64) Biquad filter0 coefficient A1
                PRA_BIQUAD0_A2 = &H133S '(F64) Biquad filter0 coefficient A2
                PRA_BIQUAD0_B0 = &H134S '(F64) Biquad filter0 coefficient B0
                PRA_BIQUAD0_B1 = &H135S '(F64) Biquad filter0 coefficient B1
                PRA_BIQUAD0_B2 = &H136S '(F64) Biquad filter0 coefficient B2
                PRA_BIQUAD0_DIV = &H137S '(F64) Biquad filter0 divider
                PRA_BIQUAD1_A1 = &H138S '(F64) Biquad filter1 coefficient A1
                PRA_BIQUAD1_A2 = &H139S '(F64) Biquad filter1 coefficient A2
                PRA_BIQUAD1_B0 = &H13AS '(F64) Biquad filter1 coefficient B0
                PRA_BIQUAD1_B1 = &H13BS '(F64) Biquad filter1 coefficient B1
                PRA_BIQUAD1_B2 = &H13CS '(F64) Biquad filter1 coefficient B2
                PRA_BIQUAD1_DIV = &H13DS '(F64) Biquad filter1 divider
                PRA_FRIC_GAIN = &H13ES '// (F64) Friction voltage compensation
                PRB_SSC_APPLICATION = &H10000 ' Reserved
                PRB_SSC_CYCLE_TIME = &H10000 ' SSCNET cycle time selection(vaild befor start sscnet
                PRB_PARA_INIT_OPT = &H20S ' Initial boot mode.
                PRB_DPAC_DISPLAY_MODE = &H10001 'DPAC Display mode
                PRB_DPAC_DI_MODE = &H10002 'Set DI pin modes
                PRB_DPAC_THERMAL_MONITOR_NO = &H20001 'DPAC TEST
                PRB_DPAC_THERMAL_MONITOR_VALUE = &H20002 'DPAC TEST
                PRA_EL_LOGIC = &H0S ' EL logic
                PRA_ORG_LOGIC = &H1S ' ORG logic
                PRA_EL_MODE = &H2S ' EL stop mode
                PRA_MDM_CONDI = &H3S ' Motion done condition
                PRA_EL_EXCHANGE = &H4S ' PEL, MEL exchange enable
                PRA_ALM_LOGIC = &H4S ' ALM logic [PCI-8253/56 only]
                PRA_ZSP_LOGIC = &H5S ' ZSP logic [PCI-8253/56 only]
                PRA_EZ_LOGIC = &H6S ' EZ logic  [PCI-8253/56 only]
                PRA_STP_DEC = &H7S ' Stop deceleration
                PRA_SPEL_EN = &H8S ' SPEL Enable
                PRA_SMEL_EN = &H9S ' SMEL Enable
                PRA_EFB_POS0 = &HAS ' EFB position 0
                PRA_SPEL_POS = &HAS ' EFB position 0
                PRA_EFB_POS1 = &HBS ' EFB position 1
                PRA_SMEL_POS = &HBS ' EFB position 1
                PRA_EFB_CONDI0 = &HCS ' EFB position 0 condition
                PRA_EFB_CONDI1 = &HDS ' EFB position 1 condition
                PRA_EFB_SRC0 = &HES ' EFB position 0 source
                PRA_EFB_SRC1 = &HFS ' EFB position 1 source
                ''' <summary>
                ''' 原点模式设置
                ''' </summary>
                ''' <remarks></remarks>
                PRA_HOME_MODE = &H10S ' home mode

                ''' <summary>
                ''' 原点方向
                ''' </summary>
                ''' <remarks></remarks>
                PRA_HOME_DIR = &H11S ' homing direction
                ''' <summary>
                ''' 归零曲线加速系数
                ''' </summary>
                ''' <remarks></remarks>
                PRA_HOME_CURVE = &H12S ' homing curve parten(T or s curve

                PRA_HOME_ACC = &H13S ' Acceleration deceleration rate
                PRA_HOME_VS = &H14S ' homing start velocity
                PRA_HOME_VM = &H15S ' homing max velocity
                PRA_JG_MODE = &H40S ' Jog mode
                PRA_JG_DIR = &H41S ' Jog move direction
                PRA_JG_CURVE = &H42S ' Jog curve parten(T or s curve
                PRA_JG_SF = &H42S ' Jog curve parten(T or s curve
                PRA_JG_ACC = &H43S ' Jog move acceleration
                PRA_JG_DEC = &H44S ' Jog move deceleration
                PRA_JG_VM = &H45S ' Jog move max velocity
                PRA_PLS_IPT_MODE = &H80S ' Pulse input mode setting
                PRA_PLS_OPT_MODE = &H81S ' Pulse output mode setting
                PRA_MAX_E_LIMIT = &H82S ' Maximum encoder count limit
                PRA_ENC_FILTER = &H83S ' Encoder filter
                PRA_ENCODER_FILTER = &H83S ' Encoder filter
                PRA_EGEAR = &H84S ' E-Gear ratio
                PRA_KP_GAIN = &H90S ' PID controller Kp gain
                PRA_KI_GAIN = &H91S ' PID controller Ki gain
                PRA_KD_GAIN = &H92S ' PID controller Kd gain
                PRA_KFF_GAIN = &H93S ' Feed forward Kff gain
                PRA_KVFF_GAIN = &H93S ' Feed forward Kff gain
                PRA_KVGTY_GAIN = &H94S ' Gantry controller Kvgty gain
                PRA_KPGTY_GAIN = &H95S ' Gantry controller Kpgty gain
                PRA_IKP_GAIN = &H96S ' PID controller Kp gain in torque mode
                PRA_IKI_GAIN = &H97S ' PID controller Ki gain in torque mode
                PRA_IKD_GAIN = &H98S ' PID controller Kd gain in torque mode
                PRA_IKFF_GAIN = &H99S ' Feed forward Kff gain in torque mode
                PRA_KAFF_GAIN = &H9AS  ' Acceleration feedforward Kaff gain
                PRA_KP_SHIFT = &H9BS           '//Proportional control result shift
                PRA_KI_SHIFT = &H9CS            '//Integral control result shift
                PRA_KD_SHIFT = &H9DS           '// Derivative control result shift
                PRA_KVFF_SHIFT = &H9ES          '//Velocity feed-forward control result shift
                PRA_KAFF_SHIFT = &H9FS          '//Acceleration feed-forward control result shift
                PRA_PID_SHIFT = &HA0S          '//PID control result shift
                PRA_VOLTAGE_MAX = &H9BS  ' Maximum output limit
                PRA_VOLTAGE_MIN = &H9CS  ' Minimum output limit
                PRA_M_INTERFACE = &H100S ' Motion interface
                PRA_M_VOL_RANGE = &H110S ' Motor voltage input range
                PRA_M_MAX_SPEED = &H111S ' Motor maximum speed
                PRA_M_ENC_RES = &H112S ' Motor encoder resolution
                PRA_V_OFFSET = &H120S ' Voltage offset
                PRA_SERVO_V_BIAS = &H120S ' Voltage offset
                PRA_DZ_LOW = &H121S ' Dead zone low side
                PRA_DZ_UP = &H122S ' Dead zone up side
                PRA_SAT_LIMIT = &H123S ' Voltage saturation output limit
                PRA_SERVO_V_LIMIT = &H123S ' Voltage saturation output limit
                PRA_ERR_C_LEVEL = &H124S ' Error counter check level
                PRA_ERR_POS_LEVEL = &H124S ' Error counter check level
                PRA_V_INVERSE = &H125S ' Output voltage inverse
                PRA_SERVO_V_INVERSE = &H125S ' Output voltage inverse
                PRA_PSR_LINK = &H130S ' Connect pulser number
                PRA_PSR_RATIO = &H131S ' Set pulser ratio
                PRA_DA_TYPE = &H140S ' DAC output type
                PRA_CONTROL_MODE = &H141S ' Closed loop control mode
                PRA_BKL_DIST = &H129S 'Backlash distance
                PRA_BKL_CNSP = &H12AS 'Backlash consumption
                PRA_INTEGRAL_LIMIT = &H12BS '(I32) Integral limit
                PRA_PLS_IPT_LOGIC = &H200S 'Reverse pulse input counting
                PRA_FEEDBACK_SRC = &H201S 'Select feedback conter
                PRA_ALM_MODE = &H210S 'ALM Mode
                PRA_INP_LOGIC = &H211S 'INP Logic
                PRA_SD_EN = &H212S 'SD Enable -- Bit 8
                PRA_SD_MODE = &H213S 'SD Mode
                PRA_SD_LOGIC = &H214S 'SD Logic
                PRA_SD_LATCH = &H215S 'SD Latch
                PRA_ERC_MODE = &H216S 'ERC Mode
                PRA_ERC_LOGIC = &H217S 'ERC logic
                PRA_ERC_LEN = &H218S 'ERC pulse width
                PRA_CLR_MODE = &H219S 'CLR Mode
                PRA_CLR_TARGET = &H21AS 'CLR Target counter
                PRA_PIN_FLT = &H21BS 'EA/EB Filter Enable
                PRA_INP_MODE = &H21CS 'INP Mode
                PRA_LTC_LOGIC = &H21DS 'LTC LOGIC
                PRA_SOFT_PLIMIT = &H21ES 'SOFT PLIMIT
                PRA_SOFT_MLIMIT = &H21FS 'SOFT MLIMIT
                PRA_SOFT_LIMIT_EN = &H220S 'SOFT ENABLE
                PRA_BACKLASH_PULSE = &H221S 'BACKLASH PULSE
                PRA_BACKLASH_MODE = &H222S 'BACKLASH MODE
                PRA_LTC_SRC = &H223S 'LTC Source
                PRA_LTC_DEST = &H224S 'LTC Destination
                PRA_LTC_DATA = &H225S 'Get LTC DATA
                PRA_GCMP_EN = &H226S ' CMP Enable
                PRA_GCMP_POS = &H227S ' Get CMP position
                PRA_GCMP_SRC = &H228S ' CMP source
                PRA_GCMP_ACTION = &H229S ' CMP Action
                PRA_GCMP_STS = &H22AS ' CMP Status
                PRA_VIBSUP_RT = &H22BS '// Vibration Reverse Time
                PRA_VIBSUP_FT = &H22CS '// Vibration Forward Time
                PRA_LTC_DATA_SPD = &H22DS '// Choose latch data for current speed or error position
                PRA_GPDO_SEL = &H230S 'Select DO/CMP Output mode
                PRA_GPDI_SEL = &H231S 'Select DO/CMP Output mode
                PRA_GPDI_LOGIC = &H232S 'Set gpio input logic
                PRA_RDY_LOGIC = &H233S 'RDY logic
                PRA_SPD_LIMIT = &H240S ' Set Fixed Speed
                PRA_MAX_ACCDEC = &H241S ' Get max acceleration by fixed speed
                PRA_MIN_ACCDEC = &H242S ' Get min acceleration by fixed speed
                PRA_ENABLE_SPD = &H243S ' Disable/Enable Fixed Speed only for HSL-4XMO.
                PRA_CONTI_MODE = &H250S ' Continuous Mode
                PRA_CONTI_BUFF = &H251S ' Continuous Buffer
                PRA_SYNC_STOP_MODE = &H260S '// Sync Mode
                PRA_CMD_CNT_EN = &H10000
                PRA_MIO_SEN = &H10001
                PRA_START_STA = &H10002
                PRA_SPEED_CHN = &H10003
                PRA_ORG_STP = &H1AS
                PRA_SSC_SERVO_PARAM_SRC = &H10000 'Servo parameter source
                PRA_SSC_SERVO_ABS_POS_OPT = &H10001 'Absolute position system option
                PRA_SSC_SERVO_ABS_CYC_CNT = &H10002 'Absolute cycle counter of servo driver
                PRA_SSC_SERVO_ABS_RES_CNT = &H10003 'Absolute resolution counter of servo driver
                PRA_SSC_TORQUE_LIMIT_P = &H10004 'Torque limit positive (0.1%
                PRA_SSC_TORQUE_LIMIT_N = &H10005 'Torque limit negative (0.1%
                PRA_SSC_TORQUE_CTRL = &H10006 'Torque control
                PRA_SSC_RESOLUTION = &H10007 'resolution (E-gear
                PRA_SSC_GMR = &H10008 'resolution (New E-gear
                PRA_SSC_GDR = &H10009 'resolution (New E-gear
                SAMP_PA_RATE = &H0S 'Sampling rate
                SAMP_PA_EDGE = &H2S 'Edge select
                SAMP_PA_LEVEL = &H3S 'Level select
                SAMP_PA_TRIGCH = &H5S 'Select trigger channel
                SAMP_PA_SEL = &H6S
                SAMP_PA_SRC_CH0 = &H10S 'Sample source of channel 0
                SAMP_PA_SRC_CH1 = &H11S 'Sample source of channel 1
                SAMP_PA_SRC_CH2 = &H12S 'Sample source of channel 2
                SAMP_PA_SRC_CH3 = &H13S 'Sample source of channel 3
                SAMP_AXIS_MASK = &HF00S
                SAMP_PARAM_MASK = &HFFS
                SAMP_COM_POS = &H0S 'command position
                SAMP_FBK_POS = &H1S 'feedback position
                SAMP_CMD_VEL = &H2S 'command velocity
                SAMP_FBK_VEL = &H3S 'feedback velocity
                SAMP_MIO = &H4S 'motion IO
                SAMP_MSTS = &H5S 'motion status
                SAMP_MSTS_ACC = &H6S 'motion status acc
                SAMP_MSTS_MV = &H7S 'motion status at max velocity
                SAMP_MSTS_DEC = &H8S 'motion status at dec
                SAMP_MSTS_CSTP = &H9S 'motion status CSTP
                SAMP_MSTS_NSTP = &HAS 'motion status NSTP
                SAMP_MSTS_MDN = &HAS 'motion status NSTP
                SAMP_MIO_INP = &HBS 'motion status INP
                SAMP_MIO_ZERO = &HCS 'motion status ZERO
                SAMP_MIO_ORG = &HDS 'motion status OGR
                SAMP_SSC_MON_0 = &H10S ' SSCNET servo monitor ch0
                SAMP_SSC_MON_1 = &H11S ' SSCNET servo monitor ch1
                SAMP_SSC_MON_2 = &H12S ' SSCNET servo monitor ch2
                SAMP_SSC_MON_3 = &H13S ' SSCNET servo monitor ch3
                SAMP_COM_POS_F64 = &H10S '// Command position
                SAMP_FBK_POS_F64 = &H11S '//Feedback position
                SAMP_CMD_VEL_F64 = &H12S '// Command velocity
                SAMP_FBK_VEL_F64 = &H13S '// Feedback velocity
                SAMP_CONTROL_VOL_F64 = &H14S '// Control command voltage
                SAMP_ERR_POS_F64 = &H15S '// Error position
                SAMP_PWM_FREQUENCY_F64 = &H18S '// PWM frequency (Hz)
                SAMP_PWM_DUTY_CYCLE_F64 = &H19S '// PWM duty cycle (%)
                SAMP_PWM_WIDTH_F64 = &H1AS '// PWM width (ns)
                SAMP_VAO_COMP_VEL_F64 = &H1BS '// Composed velocity for Laser power control (pps)
                SAMP_PTBUFF_COMP_VEL_F64 = &H1CS '// Composed velocity of point table
                SAMP_PTBUFF_COMP_ACC_F64 = &H1DS '// Composed acceleration of point table
                SAMP_CONTROL_VOL = &H20S '
                SAMP_GTY_DEVIATION = &H21S
                SAMP_ENCODER_RAW = &H22S
                SAMP_ERROR_COUNTER = &H23S
                SAMP_ERROR_POS = &H23S '//Error position [PCI-8254/58]
                SAMP_PTBUFF_RUN_INDEX = &H24S '//Point table running index
                PRF_COMMUNICATION_TYPE = &H0S ' FiledBus Communication Type(Full/half duplex
                PRF_TRANSFER_RATE = &H1S ' FiledBus Transfer Rate
                PRF_HUB_NUMBER = &H2S ' FiledBus Hub Number
                PRF_INITIAL_TYPE = &H3S ' FiledBus Initial Type(Clear/Reserve Do area
                PRF_CHKERRCNT_LAYER = &H4S '// Set the check error count layer.
                GANTRY_MODE = &H0S
                GENTRY_DEVIATION = &H1S
                GENTRY_DEVIATION_STP = &H2S
                FTR_TYPE_ST0 = &H0S  '// Station 0 filter type
                FTR_FC_ST0 = &H1S  '// Station 0 filter cutoff frequency
                FTR_BW_ST0 = &H2S  '// Station 0 filter bandwidth
                FTR_ENABLE_ST0 = &H3S  '// Station 0 filter enable/disable
                FTR_TYPE_ST1 = &H10S  '// Station 1 filter type
                FTR_FC_ST1 = &H11S  '// Station 1 filter cutoff frequency
                FTR_BW_ST1 = &H12S  '// Station 1 filter bandwidth
                FTR_ENABLE_ST1 = &H13S  '// Station 1 filter enable/disable
                FTR_TYPE = &H0S ' Filter type
                FTR_FC = &H1S ' Filter cutoff frequency
                FTR_BW = &H2S ' Filter bandwidth
                FTR_ENABLE = &H3S ' Filter enable/disable
                DEVICE_NAME_NULL = &HFFFFS
                DEVICE_NAME_PCI_8392 = 0
                DEVICE_NAME_PCI_825X = 1
                DEVICE_NAME_PCI_8154 = 2
                DEVICE_NAME_PCI_785X = 3
                DEVICE_NAME_PCI_8158 = 4
                DEVICE_NAME_PCI_7856 = 5
                DEVICE_NAME_ISA_DPAC1000 = 6
                DEVICE_NAME_ISA_DPAC3000 = 7
                DEVICE_NAME_PCI_8144 = 8
                DEVICE_NAME_PCI_825458 = 9
                DEVICE_NAME_PCI_8102 = 10
                DEVICE_NAME_PCI_V8258 = 11
                DEVICE_NAME_PCI_V8254 = 12
                DEVICE_NAME_PCI_8158A = 13
                DEVICE_NAME_AMP_20408C = 14
                SLAVE_NAME_UNKNOWN = &H0S
                SLAVE_NAME_HSL_DI32 = &H100S
                SLAVE_NAME_HSL_DO32 = &H101S
                SLAVE_NAME_HSL_DI16DO16 = &H102S
                SLAVE_NAME_HSL_AO4 = &H103S
                SLAVE_NAME_HSL_AI16AO2_VV = &H104S
                SLAVE_NAME_HSL_AI16AO2_AV = &H105S
                SLAVE_NAME_HSL_DI16UL = &H106S
                SLAVE_NAME_HSL_DI16RO8 = &H107S
                SLAVE_NAME_HSL_4XMO = &H108S
                SLAVE_NAME_HSL_DI16_UCT = &H109S
                SLAVE_NAME_HSL_DO16_UCT = &H10AS
                SLAVE_NAME_HSL_DI8DO8 = &H10BS
                SLAVE_NAME_MNET_1XMO = &H200S
                SLAVE_NAME_MNET_4XMO = &H201S
                SLAVE_NAME_MNET_4XMO_C = &H202S
                TG_PWM0_PULSE_WIDTH = &H0S
                TG_PWM1_PULSE_WIDTH = &H1S
                TG_PWM0_MODE = &H2S
                TG_PWM1_MODE = &H3S
                TG_TIMER0_INTERVAL = &H4S
                TG_TIMER1_INTERVAL = &H5S
                TG_ENC0_CNT_DIR = &H6S
                TG_ENC1_CNT_DIR = &H7S
                TG_IPT0_MODE = &H8S
                TG_IPT1_MODE = &H9S
                TG_EZ0_CLEAR_EN = &HAS
                TG_EZ1_CLEAR_EN = &HBS
                TG_EZ0_CLEAR_LOGIC = &HCS
                TG_EZ1_CLEAR_LOGIC = &HDS
                TG_CNT0_SOURCE = &HES
                TG_CNT1_SOURCE = &HFS
                TG_FTR0_EN = &H10S
                TG_FTR1_EN = &H11S
                TG_DI_LATCH0_EN = &H12S
                TG_DI_LATCH1_EN = &H13S
                TG_DI_LATCH0_EDGE = &H14S
                TG_DI_LATCH1_EDGE = &H15S
                TG_DI_LATCH0_VALUE = &H16S
                TG_DI_LATCH1_VALUE = &H17S
                TG_TRGOUT_MAP = &H18S
                TG_TRGOUT_LOGIC = &H19S
                TG_FIFO_LEVEL = &H1AS
                TG_PWM0_SOURCE = &H1BS
                TG_PWM1_SOURCE = &H1CS
                TG_LCMP0_SRC = &H0S
                TG_LCMP1_SRC = &H1S
                TG_TCMP0_SRC = &H2S
                TG_TCMP1_SRC = &H3S
                TG_LCMP0_EN = &H4S
                TG_LCMP1_EN = &H5S
                TG_TCMP0_EN = &H6S
                TG_TCMP1_EN = &H7S
                TG_TRG0_SRC = &H10S
                TG_TRG1_SRC = &H11S
                TG_TRG2_SRC = &H12S
                TG_TRG3_SRC = &H13S
                TG_TRG0_PWD = &H14S
                TG_TRG1_PWD = &H15S
                TG_TRG2_PWD = &H16S
                TG_TRG3_PWD = &H17S
                TG_TRG0_CFG = &H18S
                TG_TRG1_CFG = &H19S
                TG_TRG2_CFG = &H1AS
                TG_TRG3_CFG = &H1BS
                TMR_ITV = &H20S
                TMR_EN = &H21S
                TG_CMP0_SRC = &H0S
                TG_CMP1_SRC = &H1S
                TG_CMP2_SRC = &H2S
                TG_CMP3_SRC = &H3S
                TG_CMP0_EN = &H4S
                TG_CMP1_EN = &H5S
                TG_CMP2_EN = &H6S
                TG_CMP3_EN = &H7S
                TG_CMP0_TYPE = &H8S
                TG_CMP1_TYPE = &H9S
                TG_CMP2_TYPE = &HAS
                TG_CMP3_TYPE = &HBS
                TG_CMPH_EN = &HCS
                TG_CMPH_DIR_EN = &HDS
                TG_CMPH_DIR = &HES
                TG_ENCH_CFG = &H20S
                TG_TRG0_CMP_DIR = &H21S
                TG_TRG1_CMP_DIR = &H22S
                TG_TRG2_CMP_DIR = &H23S
                TG_TRG3_CMP_DIR = &H24S
                MIO_ALM = 0 ' Servo alarm.
                MIO_PEL = 1 ' Positive end limit.
                MIO_MEL = 2 ' Negative end limit.
                MIO_ORG = 3 ' ORG =Home
                MIO_EMG = 4 ' Emergency stop
                MIO_EZ = 5 ' EZ.
                MIO_INP = 6 ' In position.
                MIO_SVON = 7 ' Servo on signal.
                MIO_RDY = 8 ' Ready.
                MIO_WARN = 9 ' Warning.
                MIO_ZSP = 10 ' Zero speed.
                MIO_SPEL = 11 ' Soft positive end limit.
                MIO_SMEL = 12 ' Soft negative end limit.
                MIO_TLC = 13 ' Torque is limited by torque limit value.
                MIO_ABSL = 14 ' Absolute position lost.
                MIO_STA = 15 ' External start signal.
                MIO_PSD = 16 ' Positive slow down signal
                MIO_MSD = 17 ' Negative slow down signal
                MTS_CSTP = 0 ' Command stop signal.
                MTS_VM = 1 ' At maximum velocity.
                MTS_ACC = 2 ' In acceleration.
                MTS_DEC = 3 ' In deceleration.
                MTS_DIR = 4 ' LastMoving direction.
                MTS_NSTP = 5 ' Normal stop(Motion done.
                MTS_HMV = 6 ' In home operation.
                MTS_SMV = 7 ' Single axis move relative, absolute, velocity move.
                MTS_LIP = 8 ' Linear interpolation.
                MTS_CIP = 9 ' Circular interpolation.
                MTS_VS = 10 ' At start velocity.
                MTS_PMV = 11 ' Point table move.
                MTS_PDW = 12 ' Point table dwell move.
                MTS_PPS = 13 ' Point table pause state.
                MTS_SLV = 14 ' Slave axis move.
                MTS_JOG = 15 ' Jog move.
                MTS_ASTP = 16 ' Abnormal stop.
                MTS_SVONS = 17 ' Servo off stopped.
                MTS_EMGS = 18 ' EMG / SEMG stopped.
                MTS_ALMS = 19 ' Alarm stop.
                MTS_WANS = 20 ' Warning stopped.
                MTS_PELS = 21 ' PEL stopped.
                MTS_MELS = 22 ' MEL stopped.
                MTS_ECES = 23 ' Error counter check level reaches and stopped.
                MTS_SPELS = 24 ' Soft PEL stopped.
                MTS_SMELS = 25 ' Soft MEL stopped.
                MTS_STPOA = 26 ' Stop by others axes.
                MTS_GDCES = 27 ' Gantry deviation error level reaches and stopped.
                MTS_GTM = 28 ' Gantry mode turn on.
                MTS_PAPB = 29 ' Pulsar mode turn on.
                MTS_MDN = 5         '// Motion done. 0: In motion, 1: Motion done ( It could be abnormal stop)
                MTS_WAIT = 10        '// Axis is in waiting state. ( Wait move trigger )
                MTS_PTB = 11       ' // Axis is in point buffer moving. ( When this bit on, MDN and ASTP will be cleared )
                MTS_MagnificationD = 17        '// Axis (Axes) in blending moving
                MTS_PRED = 18        '// Pre-distance event, 1: event arrived. The event will be clear when axis start moving 
                MTS_POSTD = 19        '// Post-distance event. 1: event arrived. The event will be clear when axis start moving
                MTS_GER = 28       ' // 1: In geared ( This axis as slave axis and it follow a master specified in axis parameter. )
                MIO_ALM_V = &H1S ' Servo alarm.
                MIO_PEL_V = &H2S ' Positive end limit.
                MIO_MEL_V = &H4S ' Negative end limit.
                MIO_ORG_V = &H8S ' ORG =Home.
                MIO_EMG_V = &H10S ' Emergency stop.
                MIO_EZ_V = &H20S ' EZ.
                MIO_INP_V = &H40S ' In position.
                MIO_SVON_V = &H80S ' Servo on signal.
                MIO_RDY_V = &H100S ' Ready.
                MIO_WARN_V = &H200S ' Warning.
                MIO_ZSP_V = &H400S ' Zero speed.
                MIO_SPEL_V = &H800S ' Soft positive end limit.
                MIO_SMEL_V = &H1000S ' Soft negative end limit.
                MIO_TLC_V = &H2000S ' Torque is limited by torque limit value.
                MIO_ABSL_V = &H4000S ' Absolute position lost.
                MIO_STA_V = &H8000S ' External start signal.
                MIO_PSD_V = &H10000 ' Positive slow down signal.
                MIO_MSD_V = &H20000 ' Negative slow down signal.
                MTS_CSTP_V = &H1S ' Command stop signal.
                MTS_VM_V = &H2S ' At maximum velocity.
                MTS_ACC_V = &H4S ' In acceleration.
                MTS_DEC_V = &H8S ' In deceleration.
                MTS_DIR_V = &H10S ' LastMoving direction.
                MTS_NSTP_V = &H20S ' Normal stop Motion done.
                MTS_HMV_V = &H40S ' In home operation.
                MTS_SMV_V = &H80S ' Single axis move( relative, absolute, velocity move.
                MTS_LIP_V = &H100S ' Linear interpolation.
                MTS_CIP_V = &H200S ' Circular interpolation.
                MTS_VS_V = &H400S ' At start velocity.
                MTS_PMV_V = &H800S ' Point table move.
                MTS_PDW_V = &H1000S ' Point table dwell move.
                MTS_PPS_V = &H2000S ' Point table pause state.
                MTS_SLV_V = &H4000S ' Slave axis move.
                MTS_JOG_V = &H8000S ' Jog move.
                MTS_ASTP_V = &H10000 ' Abnormal stop.
                MTS_SVONS_V = &H20000 ' Servo off stopped.
                MTS_EMGS_V = &H40000 ' EMG / SEMG stopped.
                MTS_ALMS_V = &H80000 ' Alarm stop.
                MTS_WANS_V = &H100000 ' Warning stopped.
                MTS_PELS_V = &H200000 ' PEL stopped.
                MTS_MELS_V = &H400000 ' MEL stopped.
                MTS_ECES_V = &H800000 ' Error counter check level reaches and stopped.
                MTS_SPELS_V = &H1000000 ' Soft PEL stopped.
                MTS_SMELS_V = &H2000000 ' Soft MEL stopped.
                MTS_STPOA_V = &H4000000 ' Stop by others axes.
                MTS_GDCES_V = &H8000000 ' Gantry deviation error level reaches and stopped.
                MTS_GTM_V = &H10000000 ' Gantry mode turn on.
                MTS_PAPB_V = &H20000000 ' Pulsar mode turn on.
                PT_OPT_ABS = &H0    '// move, absolute
                PT_OPT_REL = &H1    '// move, relative
                PT_OPT_LINEAR = &H0    '// move, linear
                PT_OPT_ARC = &H4   '// move, arc
                PT_OPT_FC_CSTP = &H0    '// signal, command stop (finish condition)
                PT_OPT_FC_INP = &H10    '// signal, in position
                PT_OPT_LAST_POS = &H20    '// last point index
                PT_OPT_DWELL = &H40   '// dwell
                PT_OPT_RAPID = &H80    '// rapid positioning
                PT_OPT_NOARC = &H10000    '// do not add arc
                PT_OPT_SCUVE = &H2    '// s-curve
                TGR_LCMP0_SRC = &H0S
                TGR_LCMP1_SRC = &H1S
                TGR_TCMP0_SRC = &H2S
                TGR_TCMP1_SRC = &H3S
                TGR_TCMP0_DIR = &H4S
                TGR_TCMP1_DIR = &H5S
                TGR_TRG_EN = &H6S
                TGR_TRG0_SRC = &H10S
                TGR_TRG1_SRC = &H11S
                TGR_TRG2_SRC = &H12S
                TGR_TRG3_SRC = &H13S
                TGR_TRG0_PWD = &H14S
                TGR_TRG1_PWD = &H15S
                TGR_TRG2_PWD = &H16S
                TGR_TRG3_PWD = &H17S
                TGR_TRG0_LOGIC = &H18S
                TGR_TRG1_LOGIC = &H19S
                TGR_TRG2_LOGIC = &H1AS
                TGR_TRG3_LOGIC = &H1BS
                TGR_TRG0_TGL = &H1CS
                TGR_TRG1_TGL = &H1DS
                TGR_TRG2_TGL = &H1ES
                TGR_TRG3_TGL = &H1FS
                TIMR_ITV = &H20S
                TIMR_DIR = &H21S
                TIMR_RING_EN = &H22S
                TIMR_EN = &H23S
                TIG_LCMP0_SRC = &H0S
                TIG_LCMP1_SRC = &H1S
                TIG_LCMP2_SRC = &H2S
                TIG_LCMP3_SRC = &H3S
                TIG_LCMP4_SRC = &H4S
                TIG_LCMP5_SRC = &H5S
                TIG_LCMP6_SRC = &H6S
                TIG_LCMP7_SRC = &H7S
                TIG_TCMP0_SRC = &H8S
                TIG_TCMP1_SRC = &H9S
                TIG_TCMP2_SRC = &HAS
                TIG_TCMP3_SRC = &HBS
                TIG_TCMP4_SRC = &HCS
                TIG_TCMP5_SRC = &HDS
                TIG_TCMP6_SRC = &HES
                TIG_TCMP7_SRC = &HFS
                TIG_TRG0_EN = &H10S
                TIG_TRG1_EN = &H11S
                TIG_TRG2_EN = &H12S
                TIG_TRG3_EN = &H13S
                TIG_TRG4_EN = &H14S
                TIG_TRG5_EN = &H15S
                TIG_TRG6_EN = &H16S
                TIG_TRG7_EN = &H17S
                TIG_TRG0_SRC = &H18S
                TIG_TRG1_SRC = &H19S
                TIG_TRG2_SRC = &H1AS
                TIG_TRG3_SRC = &H1BS
                TIG_TRG4_SRC = &H1CS
                TIG_TRG5_SRC = &H1DS
                TIG_TRG6_SRC = &H1ES
                TIG_TRG7_SRC = &H1FS
                TIG_TRG0_PWD = &H20S
                TIG_TRG1_PWD = &H21S
                TIG_TRG2_PWD = &H20S
                TIG_TRG3_PWD = &H23S
                TIG_TRG4_PWD = &H24S
                TIG_TRG5_PWD = &H25S
                TIG_TRG6_PWD = &H26S
                TIG_TRG7_PWD = &H27S
                TIG_TRG0_LOGIC = &H28S
                TIG_TRG1_LOGIC = &H29S
                TIG_TRG2_LOGIC = &H2AS
                TIG_TRG3_LOGIC = &H2BS
                TIG_TRG4_LOGIC = &H2CS
                TIG_TRG5_LOGIC = &H2DS
                TIG_TRG6_LOGIC = &H2ES
                TIG_TRG7_LOGIC = &H2FS
                TIG_TRG0_TGL = &H30S
                TIG_TRG1_TGL = &H31S
                TIG_TRG2_TGL = &H32S
                TIG_TRG3_TGL = &H33S
                TIG_TRG4_TGL = &H34S
                TIG_TRG5_TGL = &H35S
                TIG_TRG6_TGL = &H36S
                TIG_TRG7_TGL = &H37S
                TIG_PWMTMR0_ITV = &H40S
                TIG_PWMTMR1_ITV = &H41S
                TIG_PWMTMR2_ITV = &H42S
                TIG_PWMTMR3_ITV = &H43S
                TIG_PWMTMR4_ITV = &H44S
                TIG_PWMTMR5_ITV = &H45S
                TIG_PWMTMR6_ITV = &H46S
                TIG_PWMTMR7_ITV = &H47S
                TIG_TMR0_ITV = &H50S
                TIG_TMR0_DIR = &H51S
                OPT_ABSOLUTE = &H0
                OPT_RELATIVE = &H1
                OPT_WAIT = &H100
                PTP_OPT_ABORTING = &H0S
                PTP_OPT_BUFFERED = &H1000S
                PTP_OPT_MagnificationEND_LOW = &H2000S
                PTP_OPT_MagnificationEND_PREVIOUS = &H3000
                PTP_OPT_MagnificationEND_NEXT = &H4000S
                PTP_OPT_MagnificationEND_HIGH = &H5000S
                ITP_OPT_ABORT_MagnificationEND = &H0S
                ITP_OPT_ABORT_FORCE = &H1000S
                ITP_OPT_ABORT_STOP = &H2000S
                ITP_OPT_BUFFERED = &H3000S
                ITP_OPT_MagnificationEND_DEC_EVENT = &H4000S
                ITP_OPT_MagnificationEND_RES_DIST = &H5000S
                ITP_OPT_MagnificationEND_RES_DIST_PERCENT = &H6000S
                LTC_ENC_IPT_MODE = &H0
                LTC_ENC_EA_INV = &H1
                LTC_ENC_EB_INV = &H2
                LTC_ENC_EZ_CLR_LOGIC = &H3
                LTC_ENC_EZ_CLR_EN = &H4
                LTC_ENC_SIGNAL_FILITER_EN = &H5
                LTC_FIFO_HIGH_LEVEL = &H6
                LTC_SIGNAL_FILITER_EN = &H7
                LTC_SIGNAL_TRIG_LOGIC = &H8
                ERR_NoError = (0)        '//No Error	
                ERR_OSVersion = (-1)   '// Operation System type mismatched
                ERR_OpenDriverFailed = (-2) '	// Open device driver failed - Create driver interface failed
                ERR_InsufficientMemory = (-3)   '// System memory insufficiently
                ERR_DeviceNotInitial = (-4)   '// Cards not be initialized
                ERR_NoDeviceFound = (-5)    '// Cards not found(No card in your system)
                ERR_CardIdDuplicate = (-6) '	// Cards' ID is duplicated. 
                ERR_DeviceAlreadyInitialed = (-7) '	// Cards have been initialed 
                ERR_InterruptNotEnable = (-8) '	// Cards' interrupt events not enable or not be initialized
                ERR_TimeOut = (-9)    '// Function time out
                ERR_ParametersInvalid = (-10) '	// Function input parameters are invalid
                ERR_SetEEPROM = (-11)  '// Set data to EEPROM (or nonvolatile memory) failed
                ERR_GetEEPROM = (-12)  '// Get data from EEPROM (or nonvolatile memory) failed
                ERR_FunctionNotAvailable = (-13) '	// Function is not available in this step, The device is not support this function or Internal process failed
                ERR_FirmwareError = (-14)   '// Firmware error, please reboot the system
                ERR_CommandInProcess = (-15)   '// Previous command is in process
                ERR_Axis_IdDuplicate = (-16) '	// Axes' ID is duplicated.
                ERR_ModuleNotFound = (-17)  ' // Slave module not found.
                ERR_InsufficientModuleNo = (-18)  '// System ModuleNo insufficiently
                ERR_HandShakeFailed = (-19)   '// HandSake with the DSP out of time.
                ERR_FILE_FORMAT = (-20)  '// Config file format error.(cannot be parsed)
                ERR_ParametersReadOnly = (-21)   '// Function parameters read only.
                ERR_DistantNotEnough = (-22)  '// Distant is not enough for motion.
                ERR_FunctionNotEnable = (-23)   '// Function is not enabled.
                ERR_ServerAlreadyClose = (-24)  '// Server already closed.
                ERR_DllNotFound = (-25)  '// Related dll is not found, not in correct path.
                ERR_TrimDAC_Channel = (-26)
                ERR_Satellite_Type = (-27)
                ERR_Over_Voltage_Spec = (-28)
                ERR_Over_Current_Spec = (-29)
                ERR_SlaveIsNotAI = (-30)
                ERR_Over_AO_Channel_Scope = (-31)
                ERR_DllFuncFailed = (-32)  '// Failed to invoke dll function. Extension Dll version is wrong.
                ERR_FeederAbnormalStop = (-33) '//Feeder abnormal stop, External stop or feeding stop
                ERR_Read_ModuleType_Dismatch = (-34)
                ERR_Win32Error = (-1000) '// No such INT number, or WIN32_API error, contact with ADLINK's FAE staff.
                ERR_DspStart = (-2000)
            End Enum

            Enum PRA_HOME_MODE_ENUM
                HOME_MODE_ORG = 0
                HOME_MODE_EL = 1
                HOME_MODE_EZ = 2
            End Enum

            Enum PRA_HOME_DIR_ENUM
                Positive = 0
                Negative = 1
            End Enum

            Enum PRA_HOME_CURVE_ENUM
                T_curve = 0
                S_curve = 1
            End Enum

            Enum PRA_SF_ENUM
                T_curve = 0
                S_curve = 1
            End Enum

            Enum PRA_HOME_EZA_ENUM
                Disable = 0
                Enable = 1
            End Enum

            Enum PRA_EMG_MODE_ENUM
                EM0 = 0
                EMS = 1
            End Enum

            Enum PRA_EL_MODE_ENUM
                DECELERATION_STOP = 0
                STOP_IMMEDIATELY = 1
            End Enum

            Enum PRA_EL_LOGIC_ENUM
                Not_Inverse = 0
                Inverse = 1
            End Enum

            Enum PRA_ORG_LOGIC_ENUM
                Not_Inverse = 0
                Inverse = 1
            End Enum

            Enum PRA_MDM_CONDI_ENUM
                Command_Done = 0
                Command_Done_With_Inp = 1
            End Enum

            Enum PRA_ALM_LOGIC_ENUM
                Low_Active = 0
                High_Active = 1
            End Enum

            Enum PRA_EZ_LOGIC_ENUM
                Low_Active = 0
                High_Active = 1
            End Enum

            Enum PRA_SPEL_EN_ENUM
                Disable = 0
                Reserved = 1
                Soft_Limit = 2
            End Enum

            Enum PRA_SMEL_EN_ENUM
                Disable = 0
                Reserved = 1
                Soft_Limit = 2
            End Enum

            Enum PRA_APS_OPTION_ENUM
                Absolute = 0
                Relative = 1
            End Enum

            Enum PRA_AXIS_ID_ENUM
                X1 = 0
                Y1 = 1
                Z3 = 2
                R1 = 3
                X2 = 4
                Y2 = 5
                U1 = 6
                R2 = 7
                Z1 = 8
                Z4 = 9
                Y3 = 12
                Z2 = 13
            End Enum

            Structure PRA_AXIS
                Shared X1 As Double
                Shared X2 As Double
                Shared R1 As Double
                Shared R2 As Double
                Shared Y1 As Double
                Shared Y2 As Double
                Shared Y3 As Double
                Shared Z1 As Double
                Shared Z2 As Double
                Shared Z3 As Double
                Shared Z4 As Double
                Shared U1 As Double
                Shared Needle_X As Double
                Shared Needle_Y As Double
                Shared Needle_Z As Double
                Shared SPEED As Integer
                Shared TACC As Double
                Shared SLOW_SPEED_1 As Integer
                Shared SLOW_SPEED_5 As Integer
            End Structure

            Structure PRA_AXIS_NAME
                Shared X01 As String = "X01"
                Shared X02 As String = "X02"
                Shared R01 As String = "R01"
                Shared R02 As String = "R02"
                Shared Y01 As String = "Y01"
                Shared Y02 As String = "Y02"
                Shared Y03 As String = "Y02"
                Shared Z01 As String = "Z01"
                Shared Z02 As String = "Z02"
                Shared Z03 As String = "Z03"
                Shared Z04 As String = "Z04"
                Shared U01 As String = "U01"
                Shared Speed As String = "运行速度"
                Shared ACC As String = "加减速度"
            End Structure

            Structure PRA_AXIS_STATE
                Shared X1 As Integer = 0
                Shared X2 As Integer = 0
                Shared R1 As Integer = 0
                Shared R2 As Integer = 0
                Shared Y1 As Integer = 0
                Shared Z3 As Integer = 0
                Shared Z4 As Integer = 0
                Shared Z1 As Integer = 0
                Shared Z2 As Integer = 0
            End Structure

            Enum PAR_APS_DIR_ENUM
                Positive = 0
                Negative = 1
            End Enum

            Enum PRA_OUT_STATUS_ENUM
                OUT_OFF = 0
                OUT_ON = 1
            End Enum

            Enum PRA_JOG_MODE_ENUM
                Continuous_mode = 0
                Step_mode = 1
            End Enum

            Enum PRA_JOG_DIR_ENUM
                Negative = 0
                Positive = 1
            End Enum

            Enum PRA_DO_ENUM
                DO_08 = 8
                DO_09 = 9
                DO_10 = 10
                DO_11 = 11
                DO_12 = 12
                DO_13 = 13
                DO_14 = 14
                DO_15 = 15
                DO_16 = 16
                DO_17 = 17
                DO_18 = 18
                DO_19 = 19
                DO_20 = 20
                DO_21 = 21
                DO_22 = 22
                DO_23 = 23
            End Enum

            Enum PRA_DI_ENUM
                DI_08 = 8
                DI_09 = 9
                DI_10 = 10
                DI_11 = 11
                DI_12 = 12
                DI_13 = 13
                DI_14 = 14
                DI_15 = 15
                DI_16 = 16
                DI_17 = 17
                DI_18 = 18
                DI_19 = 19
                DI_20 = 20
                DI_21 = 21
                DI_22 = 22
                DI_23 = 23
            End Enum

            Enum PRA_DIMENSION_ENUM
                Dimension_01 = 1
                Dimension_02 = 2
                Dimension_03 = 3
                Dimension_04 = 4
                Dimension_05 = 5
                Dimension_06 = 6
            End Enum

            Enum MOTION_STATUS_ENUM
                MIO_ALM = 0 ' Servo alarm.
                MIO_PEL = 1 ' Positive end limit.
                MIO_MEL = 2 ' Negative end limit.
                MIO_ORG = 3 ' ORG =Home
                MIO_EMG = 4 ' Emergency stop
                MIO_EZ = 5 ' EZ.
                MIO_INP = 6 ' In position.
                MIO_SVON = 7 ' Servo on signal.
                MIO_RDY = 8 ' Ready.
                MIO_WARN = 9 ' Warning.
                MIO_ZSP = 10 ' Zero speed.
                MIO_SPEL = 11 ' Soft positive end limit.
                MIO_SMEL = 12 ' Soft negative end limit.
                MIO_TLC = 13 ' Torque is limited by torque limit value.
                MIO_ABSL = 14 ' Absolute position lost.
                MIO_STA = 15 ' External start signal.
                MIO_PSD = 16 ' Positive slow down signal
                MIO_MSD = 17 ' Negative slow down signal

                ' Motion status bit number define.
                MTS_CSTP = 0 ' Command stop signal.
                MTS_VM = 1 ' At maximum velocity.
                MTS_ACC = 2 ' In acceleration.
                MTS_DEC = 3 ' In deceleration.
                MTS_DIR = 4 ' LastMoving direction.
                MTS_NSTP = 5 ' Normal stop(Motion done.
                MTS_HMV = 6 ' In home operation.
                MTS_SMV = 7 ' Single axis move relative, absolute, velocity move.
                MTS_LIP = 8 ' Linear interpolation.
                MTS_CIP = 9 ' Circular interpolation.
                MTS_VS = 10 ' At start velocity.
                MTS_PMV = 11 ' Point table move.
                MTS_PDW = 12 ' Point table dwell move.
                MTS_PPS = 13 ' Point table pause state.
                MTS_SLV = 14 ' Slave axis move.
                MTS_JOG = 15 ' Jog move.
                MTS_ASTP = 16 ' Abnormal stop.
                MTS_SVONS = 17 ' Servo off stopped.
                MTS_EMGS = 18 ' EMG / SEMG stopped.
                MTS_ALMS = 19 ' Alarm stop.
                MTS_WANS = 20 ' Warning stopped.
                MTS_PELS = 21 ' PEL stopped.
                MTS_MELS = 22 ' MEL stopped.
                MTS_ECES = 23 ' Error counter check level reaches and stopped.
                MTS_SPELS = 24 ' Soft PEL stopped.
                MTS_SMELS = 25 ' Soft MEL stopped.
                MTS_STPOA = 26 ' Stop by others axes.
                MTS_GDCES = 27 ' Gantry deviation error level reaches and stopped.
                MTS_GTM = 28 ' Gantry mode turn on.
                MTS_PAPB = 29 ' Pulsar mode turn on.
            End Enum

            Enum TRIGGER_SOURCH_STATUS_ENUM
                DISABLE = 0
                ENABLE = 1
            End Enum

            Enum TRIGGER_SOURCH_ENUM
                DISABLE = 0
                ENABLE = 1
            End Enum

            Enum TRIGGER_COMPARE_SOURCE_ENUM
                ENCODER_0 = 0
                ENCODER_1 = 1
                ENCODER_2 = 2
                ENCODER_3 = 3
                ENCODER_4 = 4
                ENCODER_5 = 5
                ENCODER_6 = 6
                ENCODER_7 = 7
                DISABLE = 9
            End Enum

            Enum TABLE_COMPARE_DIRECTION_ENUM
                NEGATIVE_DIRECTION = 0
                POSITIVE_DIRECTION = 1
                BI_DIRECTION = 2
            End Enum

            Enum TRIGGER_OUTPUT_MODE_ENUM
                PULSE_OUT = 0
                TOGGLE_OUT = 1
            End Enum

            Enum TRIGGER_LOGIC_ENUM
                NOT_INVERSE = 0
                INVERSE = 1
            End Enum

            Structure AXIS_STATUS
                ''' <summary>
                ''' 指令脉冲
                ''' </summary>
                ''' <remarks></remarks>
                Dim command_position As Double

                ''' <summary>
                ''' 反馈脉冲
                ''' </summary>
                ''' <remarks></remarks>
                Dim feedback_position As Double

                ''' <summary>
                ''' 目标脉冲
                ''' </summary>
                ''' <remarks></remarks>
                Dim target_position As Double

                ''' <summary>
                ''' 错位脉冲
                ''' </summary>
                ''' <remarks></remarks>
                Dim error_position As Double

                ''' <summary>
                ''' 指令速度
                ''' </summary>
                ''' <remarks></remarks>
                Dim command_velocity As Double

                ''' <summary>
                ''' 反馈速度
                ''' </summary>
                ''' <remarks></remarks>
                Dim feedback_velocity As Double

                ''' <summary>
                ''' 电机IO
                ''' </summary>
                ''' <remarks></remarks>
                Dim motion_io As Integer

                ''' <summary>
                ''' 电机状态
                ''' </summary>
                ''' <remarks></remarks>
                Dim motion_status As Integer
            End Structure

            Structure TRIGGER_PRA_STRU
                ''' <summary>
                ''' 手动触发
                ''' </summary>
                ''' <remarks></remarks>
                Shared MANUAL As UShort = 0

                Shared RESERVED As UShort = 0
                Shared FCMP0 As UShort = 0
                Shared FCMP1 As UShort = 0
                Shared LCMP0 As UShort = 0
                Shared LCMP1 As UShort = 0
                Shared TRG0 As UShort = 0
                Shared TRG1 As UShort = 0
                Shared TRG2 As UShort = 0
                Shared TRG3 As UShort = 0

                ''' <summary>
                ''' 比较轴1编码器
                ''' </summary>
                ''' <remarks></remarks>
                Shared ENCODER0 As UShort = 0

                ''' <summary>
                ''' 比较轴2编码器
                ''' </summary>
                ''' <remarks></remarks>
                Shared ENCODER1 As UShort = 1

                ''' <summary>
                ''' 比较轴3编码器
                ''' </summary>
                ''' <remarks></remarks>
                Shared ENCODER2 As UShort = 2

                ''' <summary>
                ''' 比较轴4编码器
                ''' </summary>
                ''' <remarks></remarks>
                Shared ENCODER3 As UShort = 3

                ''' <summary>
                ''' 比较轴5编码器
                ''' </summary>
                ''' <remarks></remarks>
                Shared ENCODER4 As UShort = 4

                ''' <summary>
                ''' 比较轴6编码器
                ''' </summary>
                ''' <remarks></remarks>
                Shared ENCODER5 As UShort = 5

                ''' <summary>
                ''' 比较轴7编码器
                ''' </summary>
                ''' <remarks></remarks>
                Shared ENCODER6 As UShort = 6

                ''' <summary>
                ''' 比较轴8编码器
                ''' </summary>
                ''' <remarks></remarks>
                Shared ENCODER7 As UShort = 7
            End Structure

            ''' <summary>
            ''' 初始化设备
            ''' </summary>
            ''' <param name="BoardID_InBits">卡ID信息</param>
            ''' <param name="Mode">模式</param>
            ''' <param name="SubName">函数名称</param>
            ''' <param name="ErrLine">异常所在的行数</param>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Function INITIAL_CARD(ByRef BoardID_InBits As Integer, ByVal Mode As Integer, Optional ByRef SubName As String = Nothing, Optional ByRef ErrLine As String = Nothing) As Integer
                Dim ret As Integer = Nothing
                Try
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    ret = APS_initial(BoardID_InBits, Mode)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    Else
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    End If

                Catch ex As Exception
                   
                 
                    Write_Log( SubName & ex.Message  )
                    ret = -1
                End Try
                Return ret
            End Function

            Function SET_AXIS_PITCH(ByVal AXIS_PITCH() As MAGNIFICATION, Optional ByRef SubName As String = Nothing, Optional ByRef ErrLine As String = Nothing) As Integer
                Dim ret As Integer = Nothing
                Try
                    Array.Resize(axisMagnification, AXIS_PITCH.Length)
                    If AXIS_PITCH.Length > 0 Then
                        For i As Integer = 0 To AXIS_PITCH.Length - 1
                            axisMagnification(i).AXIS_MAGNIFICATION = AXIS_PITCH(i).AXIS_MAGNIFICATION
                        Next
                    Else
                        ret = -1
                    End If
                Catch ex As Exception
                   
                 
                    Write_Log( SubName & ex.Message  )
                    ret = -1
                End Try
                Return ret
            End Function

            Sub Write_Log(ByVal Message As String)
                Dim File_Path As String = Nothing
                Dim MON As String = Date.Now.Month.ToString.PadLeft(2).Replace(" ", "0") & " MONTH\"
                Dim DAY As String = Date.Now.Day.ToString.PadLeft(2).Replace(" ", "0") & " DAY\"
                File_Path = "C:\" & MON & DAY
                If System.IO.Directory.Exists(File_Path) = False Then
                    System.IO.Directory.CreateDirectory(File_Path)
                End If

                Dim sw As IO.StreamWriter
                MessageBoxEx.Show(Message & vbCrLf & "以上错误如自行不能解决请联系软件工程师！", "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
                sw = New IO.StreamWriter(File_Path & "card_log.txt", True, Encoding.Default)
                sw.WriteLine(Date.Now & "," & Message)
                sw.Flush()
                sw.Close()
                sw.Dispose()
            End Sub

            ''' <summary>
            ''' 关闭设备
            ''' </summary>
            ''' <param name="SubName">函数名称</param>
            ''' <param name="ErrLine">异常所在的行数</param>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Function CLOSE_CARD(Optional ByRef SubName As String = Nothing, Optional ByRef ErrLine As String = Nothing) As Integer
                Dim ret As Integer = Nothing
                Try

                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    ret = APS_close()
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    Else
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    End If
                Catch ex As Exception
                    Write_Log(SubName & ex.Message)
                    ret = -1
                End Try
                Return ret
            End Function

            ''' <summary>
            ''' 板卡参数设置
            ''' </summary>
            ''' <param name="Board_ID">板卡ID，0~31</param>
            ''' <param name="BOD_Param_No">板卡参数号</param>
            ''' <param name="BOD_Param">板卡参数值</param>
            ''' <param name="SubName">函数名称</param>
            ''' <param name="ErrLine">异常所在的行数</param>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Function SET_BOARD_PARAM(ByVal Board_ID As Integer, ByVal BOD_Param_No As PAR_AMETER_TABLE_ENUM, ByVal BOD_Param As Integer, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Integer
                Dim ret As Integer = Nothing
                Try
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    ret = APS_set_board_param(Board_ID, BOD_Param_No, BOD_Param)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    Else
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    End If
                Catch ex As Exception
                   
                 
                    Write_Log( SubName & ex.Message  )
                    ret = -1
                End Try
                Return ret
            End Function

            ''' <summary>
            ''' 获取板卡参数
            ''' </summary>
            ''' <param name="Board_ID">板卡ID，0~31</param>
            ''' <param name="BOD_Param_No">板卡参数号</param>
            ''' <param name="BOD_Param">板卡参数值</param>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Function GET_BOARD_PARAM(ByVal Board_ID As Integer, ByVal BOD_Param_No As PAR_AMETER_TABLE_ENUM, ByRef BOD_Param As Integer, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Integer
                Dim ret As Integer = Nothing
                Try
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    ret = APS_get_board_param(Board_ID, BOD_Param_No, BOD_Param)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    Else
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    End If
                Catch ex As Exception
                   
                 
                    Write_Log( SubName & ex.Message  )
                    ret = -1
                End Try
                Return ret
            End Function

            ''' <summary>
            ''' 设置轴参数
            ''' </summary>
            ''' <param name="Axis_Id">轴ID</param>
            ''' <param name="AXS_Param_No">参数号</param>
            ''' <param name="AXS_Param">参数值</param>
            ''' <param name="SubName"></param>
            ''' <param name="ErrLine"></param>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Function SET_AXIS_PARAM(ByVal Axis_Id As PRA_AXIS_ID_ENUM, ByVal AXS_Param_No As PAR_AMETER_TABLE_ENUM, ByVal AXS_Param As Integer, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Integer
                Dim ret As Integer = Nothing
                Try
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    ret = APS_set_axis_param(Axis_Id, AXS_Param_No, AXS_Param)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    Else
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    End If
                Catch ex As Exception
                   
                 
                    Write_Log( SubName & ex.Message  )
                    ret = -1
                End Try
                Return ret
            End Function

            ''' <summary>
            ''' 读取轴参数
            ''' </summary>
            ''' <param name="Axis_Id">轴ID</param>
            ''' <param name="AXS_Param_No">参数号</param>
            ''' <param name="AXS_Param">参数值</param>
            ''' <param name="SubName"></param>
            ''' <param name="ErrLine"></param>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Function GET_AXIS_PARAM(ByVal Axis_Id As PRA_AXIS_ID_ENUM, ByVal AXS_Param_No As PAR_AMETER_TABLE_ENUM, ByRef AXS_Param As Integer, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Integer
                Dim ret As Integer = Nothing
                Try
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    ret = APS_get_axis_param(Axis_Id, AXS_Param_No, AXS_Param)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    Else
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    End If
                Catch ex As Exception
                   
                 
                    Write_Log( SubName & ex.Message  )
                    ret = -1
                End Try
                Return ret
            End Function

            ''' <summary>
            ''' 获取卡名称
            ''' </summary>
            ''' <param name="Board_ID">卡ID</param>
            ''' <param name="CardName">卡名称</param>
            ''' <param name="SubName"></param>
            ''' <param name="ErrLine"></param>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Function GET_CARD_NAME(ByVal Board_ID As Integer, ByRef CardName As Integer, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Integer
                Dim ret As Integer = Nothing
                Try
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    ret = APS_get_card_name(Board_ID, CardName)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    Else
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    End If
                Catch ex As Exception
                   
                 
                    Write_Log( SubName & ex.Message  )
                    ret = -1
                End Try
                Return ret
            End Function

            ''' <summary>
            ''' 获取第一个轴ID
            ''' </summary>
            ''' <param name="Board_ID">卡ID</param>
            ''' <param name="Start_Axis_Id">轴开始编号</param>
            ''' <param name="Total_Axis_Num">轴数量</param>
            ''' <param name="SubName"></param>
            ''' <param name="ErrLine"></param>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Function GET_FIRST_AXIS_ID(ByVal Board_ID As Integer, ByRef Start_Axis_Id As Integer, ByRef Total_Axis_Num As Integer, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Integer
                Dim ret As Integer = Nothing
                Try
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    ret = APS_get_first_axisId(Board_ID, Start_Axis_Id, Total_Axis_Num)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    Else
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    End If
                Catch ex As Exception
                   
                 
                    Write_Log( SubName & ex.Message  )
                    ret = -1
                End Try
                Return ret
            End Function

            ''' <summary>
            ''' 加载控制卡配置文件
            ''' </summary>
            ''' <param name="FilePath">配置文件绝对路径</param>
            ''' <remarks></remarks>
            Function LOAD_PARAM_FILE(ByVal FilePath As String, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Integer
                Dim ret As Integer = Nothing
                Try
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    If IO.File.Exists(FilePath) = True Then
                        ret = APS_load_param_from_file(FilePath)
                        If ret <> 0 Then
                            SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                        Else
                            SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                        End If
                    Else
                        Return -1
                    End If
                Catch ex As Exception
                   
                 
                    Write_Log( SubName & ex.Message  )
                    ret = -1
                End Try
                Return ret
            End Function

            ''' <summary>
            ''' 加载控制卡配置文件
            ''' </summary>
            ''' <param name="FilePath"></param>
            ''' <param name="SubName"></param>
            ''' <param name="ErrLine"></param>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Function LOAD_PARAM_FILE_MU(ByVal FilePath As String, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Integer
                Dim ret As Integer = Nothing
                Try
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    If IO.File.Exists(FilePath) = True Then
                        ret = APS_load_param_from_file(FilePath)
                        If ret <> 0 Then
                            SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                        Else
                            SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                        End If
                    Else
                        Return -1
                    End If
                Catch ex As Exception
                   
                 
                    Write_Log( SubName & ex.Message  )
                    ret = -1
                End Try
                Return ret
            End Function

            ''' <summary>
            ''' 设置指令位置
            ''' </summary>
            ''' <param name="Axis_Id">轴ID</param>
            ''' <param name="CommandCnt">指令位置</param>
            ''' <param name="SubName"></param>
            ''' <param name="ErrLine"></param>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Function SET_COMMAND(ByVal Axis_Id As PRA_AXIS_ID_ENUM, ByVal CommandCnt As Integer, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Integer
                Dim ret As Integer = Nothing
                Try
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    ret = APS_set_command(Axis_Id, CommandCnt)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    Else
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    End If
                Catch ex As Exception
                   
                 
                    Write_Log( SubName & ex.Message  )
                    ret = -1
                End Try
                Return ret
            End Function

            ''' <summary>
            ''' 获取指令位置
            ''' </summary>
            ''' <param name="Axis_Id">轴ID</param>
            ''' <param name="CommandCnt">指令位置</param>
            ''' <param name="SubName"></param>
            ''' <param name="ErrLine"></param>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Function GET_COMMAND(ByVal Axis_Id As PRA_AXIS_ID_ENUM, ByRef CommandCnt As Integer, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Integer
                Dim ret As Integer = Nothing
                Try
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    ret = APS_get_command(Axis_Id, CommandCnt)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    Else
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    End If
                Catch ex As Exception
                   
                 
                    Write_Log( SubName & ex.Message  )
                    ret = -1
                End Try
                Return ret
            End Function

            ''' <summary>
            ''' 获取电机状态
            ''' </summary>
            ''' <param name="Axis_Id">轴ID</param>
            ''' <param name="SubName"></param>
            ''' <param name="ErrLine"></param>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Function MOTION_STATUS(ByVal Axis_Id As PRA_AXIS_ID_ENUM, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Integer
                Dim ret As Integer = Nothing
                Try
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    ret = APS_motion_status(Axis_Id)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    Else
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    End If
                Catch ex As Exception
                   
                 
                    Write_Log( SubName & ex.Message  )
                    ret = -1
                End Try
                Return ret
            End Function

            ''' <summary>
            ''' 获取电机IO状态
            ''' </summary>
            ''' <param name="Axis_Id">轴ID</param>
            ''' <param name="SubName"></param>
            ''' <param name="ErrLine"></param>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Function MOTION_IO_STATUS(ByVal Axis_Id As PRA_AXIS_ID_ENUM, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Integer
                Dim ret As Integer = Nothing
                Try
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    ret = APS_motion_io_status(Axis_Id)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    Else
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    End If
                Catch ex As Exception
                   
                 
                    Write_Log( SubName & ex.Message  )
                    ret = -1
                End Try
                Return ret
            End Function

            ''' <summary>
            ''' 设置伺服电机ON OR OFF
            ''' </summary>
            ''' <param name="Axis_Id">轴ID</param>
            ''' <param name="Servo_On">ON.OFF状态</param>
            ''' <param name="SubName"></param>
            ''' <param name="ErrLine"></param>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Function SET_SERVO_STATUS(ByVal Axis_Id As PRA_AXIS_ID_ENUM, ByVal Servo_On As PRA_OUT_STATUS_ENUM, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Integer
                Dim ret As Integer = Nothing
                Try
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    ret = APS_set_servo_on(Axis_Id, Servo_On)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    Else
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    End If
                Catch ex As Exception
                   
                 
                    Write_Log( SubName & ex.Message  )
                    ret = -1
                End Try
                Return ret
            End Function

            ''' <summary>
            ''' 获取电机实际位置
            ''' </summary>
            ''' <param name="Axis_Id">轴ID</param>
            ''' <param name="Position">实际位置</param>
            ''' <param name="SubName"></param>
            ''' <param name="ErrLine"></param>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Function GET_POSITION(ByVal Axis_Id As PRA_AXIS_ID_ENUM, ByRef Position As Double, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Integer
                Dim ret As Integer = Nothing
                Try
                    Dim Position_Fun As Integer = Nothing
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    ret = APS_get_position(Axis_Id, Position_Fun)
                    Position = Round(Position_Fun / axisMagnification(Axis_Id).AXIS_MAGNIFICATION, 4)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    Else
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    End If
                Catch ex As Exception
                   
                 
                    Write_Log( SubName & ex.Message  )
                    ret = -1
                End Try
                Return ret
            End Function

            ''' <summary>
            ''' 设置电机实际位置
            ''' </summary>
            ''' <param name="Axis_Id">轴ID</param>
            ''' <param name="Position">实际位置</param>
            ''' <param name="SubName"></param>
            ''' <param name="ErrLine"></param>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Function SET_POSITION(ByVal Axis_Id As PRA_AXIS_ID_ENUM, ByVal Position As Double, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Integer
                Dim ret As Integer = Nothing
                Try
                    Dim Position_Fun As Integer = Position * axisMagnification(Axis_Id).AXIS_MAGNIFICATION
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    ret = APS_set_position(Axis_Id, Position_Fun)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    Else
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    End If
                Catch ex As Exception
                   
                 
                    Write_Log( SubName & ex.Message  )
                    ret = -1
                End Try
                Return ret
            End Function

            ''' <summary>
            ''' 获取跟随误差
            ''' </summary>
            ''' <param name="Axis_Id">轴ID</param>
            ''' <param name="Err_Pos">跟随误差</param>
            ''' <param name="SubName"></param>
            ''' <param name="ErrLine"></param>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Function GET_ERROR_POSITION(ByVal Axis_Id As PRA_AXIS_ID_ENUM, ByRef Err_Pos As Integer, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Integer
                Dim ret As Integer = Nothing
                Try
                    Dim Err_Pos_Fun As Integer = Nothing
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    ret = APS_get_error_position(Axis_Id, Err_Pos_Fun)
                    Err_Pos = Err_Pos_Fun / axisMagnification(Axis_Id).AXIS_MAGNIFICATION
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    Else
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    End If
                Catch ex As Exception
                   
                 
                    Write_Log( SubName & ex.Message  )
                    ret = -1
                End Try
                Return ret
            End Function


            ''' <summary>
            ''' 相对运动
            ''' </summary>
            ''' <param name="Axis_Id">轴ID</param>
            ''' <param name="Distance">距离</param>
            ''' <param name="Max_Speed">最大速度</param>
            ''' <param name="SubName"></param>
            ''' <param name="ErrLine"></param>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Function RELATIVE_MOVE(ByVal Axis_Id As PRA_AXIS_ID_ENUM, ByVal Distance As Integer, ByVal Max_Speed As Integer, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Integer
                Dim ret As Integer = Nothing
                Try
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    ret = APS_relative_move(Axis_Id, Distance * axisMagnification(Axis_Id).AXIS_MAGNIFICATION, Max_Speed)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    Else
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    End If
                Catch ex As Exception
                   
                 
                    Write_Log( SubName & ex.Message  )
                    ret = -1
                End Try
                Return ret
            End Function

            ''' <summary>
            ''' 绝对运动
            ''' </summary>
            ''' <param name="Axis_Id">轴ID</param>
            ''' <param name="Position">距离</param>
            ''' <param name="Max_Speed">最大速度</param>
            ''' <param name="SubName"></param>
            ''' <param name="ErrLine"></param>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Function ABSOLUTE_MOVE(ByVal Axis_Id As PRA_AXIS_ID_ENUM, ByVal Position As Integer, ByVal Max_Speed As Integer, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Integer
                Dim ret As Integer = Nothing
                Try
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    ret = APS_absolute_move(Axis_Id, Position * axisMagnification(Axis_Id).AXIS_MAGNIFICATION, Max_Speed)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    Else
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    End If
                Catch ex As Exception
                   
                 
                    Write_Log( SubName & ex.Message  )
                    ret = -1
                End Try
                Return ret
            End Function



            ''' <summary>
            ''' 指定轴归零
            ''' </summary>
            ''' <param name="Axis_Id">轴ID</param>
            ''' <param name="SubName"></param>
            ''' <param name="ErrLine"></param>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Function HOME_MOVE(ByVal Axis_Id As PRA_AXIS_ID_ENUM, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Integer
                Dim ret As Integer = Nothing
                Try
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    ret = APS_home_move(Axis_Id)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    Else
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    End If
                Catch ex As Exception
                   
                 
                    Write_Log( SubName & ex.Message  )
                    ret = -1
                End Try
                Return ret
            End Function

            ''' <summary>
            ''' 指定轴停止
            ''' </summary>
            ''' <param name="Axis_Id">轴ID</param>
            ''' <param name="SubName"></param>
            ''' <param name="ErrLine"></param>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Function STOP_MOVE(ByVal Axis_Id As PRA_AXIS_ID_ENUM, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Integer
                Dim ret As Integer = Nothing
                Try
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    ret = APS_stop_move(Axis_Id)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    Else
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    End If
                Catch ex As Exception
                   
                 
                    Write_Log( SubName & ex.Message  )
                    ret = -1
                End Try
                Return ret
            End Function

            ''' <summary>
            ''' 绝对路径插补
            ''' </summary>
            ''' <param name="Dimension">坐标维数</param>
            ''' <param name="Axis_Id_Array">轴数组</param>
            ''' <param name="Position_Array">坐标数组</param>
            ''' <param name="Max_Linear_Speed">最大速度</param>
            ''' <param name="SubName"></param>
            ''' <param name="ErrLine"></param>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Function ABSOLUTE_LINER_MOVE(ByVal Dimension As PRA_DIMENSION_ENUM, ByVal Axis_Id_Array() As Integer, ByVal Position_Array() As Integer, ByVal Max_Linear_Speed As Integer, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Integer
                Dim ret As Integer = Nothing
                Try
                    If Axis_Id_Array.Length = Position_Array.Length Then
                        SubName = ErrName & GetCurrentMethod.Name & ":"
                        ret = APS_absolute_linear_move(Dimension, Axis_Id_Array, Position_Array, Max_Linear_Speed)
                        If ret <> 0 Then
                            SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                        Else
                            SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                        End If
                    Else
                        ret = -1
                    End If
                Catch ex As Exception
                   
                 
                    Write_Log( SubName & ex.Message  )
                    ret = -1
                End Try
                Return ret
            End Function

            ''' <summary>
            ''' 相对路径插补
            ''' </summary>
            ''' <param name="Dimension">坐标维数</param>
            ''' <param name="Axis_Id_Array">轴数组</param>
            ''' <param name="Position_Array">坐标数组</param>
            ''' <param name="Max_Linear_Speed">最大速度</param>
            ''' <param name="SubName"></param>
            ''' <param name="ErrLine"></param>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Function RELATIVE_LINER_MOVE(ByVal Dimension As PRA_DIMENSION_ENUM, ByVal Axis_Id_Array() As Integer, ByVal Position_Array() As Integer, ByVal Max_Linear_Speed As Integer, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Integer
                Dim ret As Integer = Nothing
                Try
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    ret = APS_relative_linear_move(Dimension, Axis_Id_Array, Position_Array, Max_Linear_Speed)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    Else
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    End If
                Catch ex As Exception
                   
                 
                    Write_Log( SubName & ex.Message  )
                    ret = -1
                End Try
                Return ret
            End Function

            Function ABSOLUTE_ARC_MOVE(ByVal Dimension As PRA_DIMENSION_ENUM, ByVal Axis_Id_Array() As Integer, ByVal Center_Pos_Array() As Integer, ByVal Max_Arc_Speed As Integer, ByVal Angle As Integer, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Integer
                Dim ret As Integer = Nothing
                Try
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    ret = APS_absolute_arc_move(Dimension, Axis_Id_Array, Center_Pos_Array, Max_Arc_Speed, Angle)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    Else
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    End If
                Catch ex As Exception
                   
                 
                    Write_Log( SubName & ex.Message  )
                    ret = -1
                End Try
                Return ret
            End Function

            Function RELATIVE_ARC_MOVE(ByVal Dimension As PRA_DIMENSION_ENUM, ByVal Axis_Id_Array() As Integer, ByVal Center_Offset_Array() As Integer, ByVal Max_Arc_Speed As Integer, ByVal Angle As Integer, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Integer
                Dim ret As Integer = Nothing
                Try
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    ret = APS_relative_arc_move(Dimension, Axis_Id_Array, Center_Offset_Array, Max_Arc_Speed, Angle)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    Else
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    End If
                Catch ex As Exception
                   
                 
                    Write_Log( SubName & ex.Message  )
                    ret = -1
                End Try
                Return ret
            End Function

            Function WRITE_D_OUTPUT(ByVal Board_ID As Integer, ByVal DO_Group As Integer, ByVal DO_Data As Integer, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Integer
                Dim ret As Integer = Nothing
                Try
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    ret = APS_write_d_output(Board_ID, DO_Group, DO_Data)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    Else
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    End If
                Catch ex As Exception
                   
                 
                    Write_Log( SubName & ex.Message  )
                    ret = -1
                End Try
                Return ret
            End Function

            Function READ_D_OUTPUT(ByVal Board_ID As Integer, ByVal DO_Group As Integer, ByRef DO_Data As Integer, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Integer
                Dim ret As Integer = Nothing
                Try
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    ret = APS_read_d_output(Board_ID, DO_Group, DO_Data)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    Else
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    End If
                Catch ex As Exception
                   
                 
                    Write_Log( SubName & ex.Message  )
                    ret = -1
                End Try
                Return ret
            End Function

            Function READ_D_INPUT(ByVal Board_ID As Integer, ByVal DI_Group As Integer, ByRef DI_Data As Integer, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Integer
                Dim ret As Integer = Nothing
                Try
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    ret = APS_read_d_input(Board_ID, DI_Group, DI_Data)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    Else
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    End If
                Catch ex As Exception
                   
                 
                    Write_Log( SubName & ex.Message  )
                    ret = -1
                End Try
                Return ret
            End Function

            Function WRITE_D_CHANNEL_OUTPUT(ByVal Board_ID As Integer, ByVal DO_Group As Integer, ByVal Ch_No As Integer, ByVal DO_Data As Integer, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Integer
                Dim ret As Integer = Nothing
                Try
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    ret = APS_write_d_channel_output(Board_ID, DO_Group, Ch_No, DO_Data)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    Else
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    End If
                Catch ex As Exception
                   
                 
                    Write_Log( SubName & ex.Message  )
                    ret = -1
                End Try
                Return ret
            End Function

            Function READ_D_CHANNEL_OUTPUT(ByVal Board_ID As Integer, ByVal DO_Group As Integer, ByVal Ch_No As Integer, ByRef DO_Data As Integer, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Integer
                Dim ret As Integer = Nothing
                Try
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    ret = APS_read_d_channel_output(Board_ID, DO_Group, Ch_No, DO_Data)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    Else
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    End If
                Catch ex As Exception
                   
                 
                    Write_Log( SubName & ex.Message  )
                    ret = -1
                End Try
                Return ret
            End Function

            Function SET_POINT_TABLE(ByVal Axis_Id As PRA_AXIS_ID_ENUM, ByVal Index As Integer, ByRef Point As POINT_DATA, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Integer
                Dim ret As Integer = Nothing
                Try
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    ret = APS_set_point_table(Axis_Id, Index, Point)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    Else
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    End If
                Catch ex As Exception
                   
                 
                    Write_Log( SubName & ex.Message  )
                    ret = -1
                End Try
                Return ret
            End Function

            Function GET_POINT_TABLE(ByVal Axis_Id As PRA_AXIS_ID_ENUM, ByVal Index As Integer, ByRef Point As POINT_DATA, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Integer
                Dim ret As Integer = Nothing
                Try
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    ret = APS_get_point_table(Axis_Id, Index, Point)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    Else
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    End If
                Catch ex As Exception
                   
                 
                    Write_Log( SubName & ex.Message  )
                    ret = -1
                End Try
                Return ret
            End Function

            Function GET_RUNNING_POINT_INDEX(ByVal Axis_Id As PRA_AXIS_ID_ENUM, ByRef Index As Integer, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Integer
                Dim ret As Integer = Nothing
                Try
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    ret = APS_get_running_point_index(Axis_Id, Index)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    Else
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    End If
                Catch ex As Exception
                   
                 
                    Write_Log( SubName & ex.Message  )
                    ret = -1
                End Try
                Return ret
            End Function

            Function GET_START_POINT_INDEX(ByVal Axis_Id As PRA_AXIS_ID_ENUM, ByRef Index As Integer, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Integer
                Dim ret As Integer = Nothing
                Try
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    ret = APS_get_start_point_index(Axis_Id, Index)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    Else
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    End If
                Catch ex As Exception
                   
                 
                    Write_Log( SubName & ex.Message  )
                    ret = -1
                End Try
                Return ret
            End Function

            Function GET_END_POINT_INDEX(ByVal Axis_Id As PRA_AXIS_ID_ENUM, ByRef Index As Integer, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Integer
                Dim ret As Integer = Nothing
                Try
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    ret = APS_get_end_point_index(Axis_Id, Index)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    Else
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    End If
                Catch ex As Exception
                   
                 
                    Write_Log( SubName & ex.Message  )
                    ret = -1
                End Try
                Return ret
            End Function

            Function set_table_move_pause(ByVal Axis_Id As PRA_AXIS_ID_ENUM, ByVal Pause_en As Integer, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Integer
                Dim ret As Integer = Nothing
                Try
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    ret = APS_set_table_move_pause(Axis_Id, Pause_en)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    Else
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    End If
                Catch ex As Exception
                   
                 
                    Write_Log( SubName & ex.Message  )
                    ret = -1
                End Try
                Return ret
            End Function

            Function set_table_move_repeat(ByVal Axis_Id As PRA_AXIS_ID_ENUM, ByVal Repeat_en As Integer, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Integer
                Dim ret As Integer = Nothing
                Try
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    ret = APS_set_table_move_repeat(Axis_Id, Repeat_en)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    Else
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    End If
                Catch ex As Exception
                   
                 
                    Write_Log( SubName & ex.Message  )
                    ret = -1
                End Try
                Return ret
            End Function

            Function point_table_move(ByVal Dimension As PRA_DIMENSION_ENUM, ByVal Axis_Id_Array() As Integer, ByVal StartIndex As Integer, ByVal EndIndex As Integer, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Integer
                Dim ret As Integer = Nothing
                Try
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    ret = APS_point_table_move(Dimension, Axis_Id_Array, StartIndex, EndIndex)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    Else
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    End If
                Catch ex As Exception
                   
                 
                    Write_Log( SubName & ex.Message  )
                    ret = -1
                End Try
                Return ret
            End Function

            Function set_point_tableEx(ByVal Axis_Id As PRA_AXIS_ID_ENUM, ByVal Index As Integer, ByRef Point As PNT_DATA, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Integer
                Dim ret As Integer = Nothing
                Try
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    ret = APS_set_point_tableEx(Axis_Id, Index, Point)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    Else
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    End If
                Catch ex As Exception
                   
                 
                    Write_Log( SubName & ex.Message  )
                    ret = -1
                End Try
                Return ret
            End Function

            Function set_table_move_ex_pause(ByVal Axis_Id As PRA_AXIS_ID_ENUM, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Integer
                Dim ret As Integer = Nothing
                Try
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    ret = APS_set_table_move_ex_pause(Axis_Id)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    Else
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    End If
                Catch ex As Exception
                   
                 
                    Write_Log( SubName & ex.Message  )
                    ret = -1
                End Try
                Return ret
            End Function

            Function set_table_move_ex_rollback(ByVal Axis_Id As PRA_AXIS_ID_ENUM, ByVal Max_Speed As Integer, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Integer
                Dim ret As Integer = Nothing
                Try
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    ret = APS_set_table_move_ex_rollback(Axis_Id, Max_Speed)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    Else
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    End If
                Catch ex As Exception
                   
                 
                    Write_Log( SubName & ex.Message  )
                    ret = -1
                End Try
                Return ret
            End Function


            Function set_point_table_mode2(ByVal Axis_Id As PRA_AXIS_ID_ENUM, ByVal Mode As Integer, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Integer
                Dim ret As Integer = Nothing
                Try
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    ret = APS_set_point_table_mode2(Axis_Id, Mode)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    Else
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    End If
                Catch ex As Exception
                   
                 
                    Write_Log( SubName & ex.Message  )
                    ret = -1
                End Try
                Return ret
            End Function

            Function set_point_table2(ByVal Dimension As Integer, ByVal Axis_Id_Array() As Integer, ByVal Index As Integer, ByRef Point As POINT_DATA2, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Integer
                Dim ret As Integer = Nothing
                Try
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    ret = APS_set_point_table2(Dimension, Axis_Id_Array, Index, Point)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    Else
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    End If
                Catch ex As Exception
                   
                 
                    Write_Log( SubName & ex.Message  )
                    ret = -1
                End Try
                Return ret
            End Function

            Function point_table_continuous_move2(ByVal Dimension As Integer, ByVal Axis_Id_Array() As Integer, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Integer
                Dim ret As Integer = Nothing
                Try
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    ret = APS_point_table_continuous_move2(Dimension, Axis_Id_Array)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    Else
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    End If
                Catch ex As Exception
                   
                 
                    Write_Log( SubName & ex.Message  )
                    ret = -1
                End Try
                Return ret
            End Function

            Function point_table_single_move2(ByVal Axis_Id As PRA_AXIS_ID_ENUM, ByVal Index As Integer, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Integer
                Dim ret As Integer = Nothing
                Try
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    ret = APS_point_table_single_move2(Axis_Id, Index)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    Else
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    End If
                Catch ex As Exception
                   
                 
                    Write_Log( SubName & ex.Message  )
                    ret = -1
                End Try
                Return ret
            End Function

            Function get_running_point_index2(ByVal Axis_Id As PRA_AXIS_ID_ENUM, ByRef Index As Integer, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Integer
                Dim ret As Integer = Nothing
                Try
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    ret = APS_get_running_point_index2(Axis_Id, Index)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    Else
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    End If
                Catch ex As Exception
                   
                 
                    Write_Log( SubName & ex.Message  )
                    ret = -1
                End Try
                Return ret
            End Function

            Function set_trigger_param(ByVal Board_ID As Integer, ByVal Param_No As PAR_AMETER_TABLE_ENUM, ByVal param_val As Integer, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Integer
                Dim ret As Integer = Nothing
                Try
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    ret = APS_set_trigger_param(Board_ID, Param_No, param_val)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    Else
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    End If
                Catch ex As Exception
                   
                 
                    Write_Log( SubName & ex.Message  )
                    ret = -1

                End Try
                Return ret
            End Function

            Function get_trigger_param(ByVal Board_ID As Integer, ByVal Param_No As PAR_AMETER_TABLE_ENUM, ByRef param_val As Integer, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Integer
                Dim ret As Integer = Nothing
                Try
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    ret = APS_get_trigger_param(Board_ID, Param_No, param_val)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    Else
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    End If
                Catch ex As Exception
                   
                 
                    Write_Log( SubName & ex.Message  )
                    ret = -1
                End Try
                Return ret
            End Function

            Function set_trigger_linear(ByVal Board_ID As Integer, ByVal LCmpCh As Integer, ByVal StartPoint As Integer, ByVal RepeatTimes As Integer, ByVal Interval As Integer, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Integer
                Dim ret As Integer = Nothing
                Try
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    ret = APS_set_trigger_linear(Board_ID, LCmpCh, StartPoint, RepeatTimes, Interval)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    Else
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    End If
                Catch ex As Exception
                   
                 
                    Write_Log( SubName & ex.Message  )
                    ret = -1
                End Try
                Return ret
            End Function

            Public Enum TCPM_ENUM
                TCMP0
                TCMP1
            End Enum

            Function set_trigger_table(ByVal Board_ID As Integer, ByVal TCmpCh As TCPM_ENUM, ByVal DataArr() As Integer, ByVal ArraySize As Integer, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Integer
                Dim ret As Integer = Nothing
                Try
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    ret = APS_set_trigger_table(Board_ID, TCmpCh, DataArr, ArraySize)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    Else
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    End If
                Catch ex As Exception
                   
                 
                    Write_Log( SubName & ex.Message  )
                    ret = -1
                End Try
                Return ret
            End Function

            Function set_trigger_manual(ByVal Board_ID As Integer, ByVal TrgCh As Integer, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Integer
                Dim ret As Integer = Nothing
                Try
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    ret = APS_set_trigger_manual(Board_ID, TrgCh)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    Else
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    End If
                Catch ex As Exception
                   
                 
                    Write_Log( SubName & ex.Message  )
                    ret = -1
                End Try
                Return ret
            End Function

            Function set_trigger_manual_s(ByVal Board_ID As Integer, ByVal TrgChInBit As Integer, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Integer
                Dim ret As Integer = Nothing
                Try
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    ret = APS_set_trigger_manual_s(Board_ID, TrgChInBit)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    Else
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    End If
                Catch ex As Exception
                   
                 
                    Write_Log( SubName & ex.Message  )
                    ret = -1
                End Try
                Return ret
            End Function

            Function get_trigger_table_cmp(ByVal Board_ID As Integer, ByVal TCmpCh As Integer, ByRef CmpVal As Integer, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Integer
                Dim ret As Integer = Nothing
                Try
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    ret = APS_get_trigger_table_cmp(Board_ID, TCmpCh, CmpVal)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    Else
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    End If
                Catch ex As Exception
                   
                 
                    Write_Log( SubName & ex.Message  )
                    ret = -1
                End Try
                Return ret
            End Function

            Function get_trigger_linear_cmp(ByVal Board_ID As Integer, ByVal LCmpCh As Integer, ByRef CmpVal As Integer, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Integer
                Dim ret As Integer = Nothing
                Try
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    ret = APS_get_trigger_linear_cmp(Board_ID, LCmpCh, CmpVal)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    Else
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    End If
                Catch ex As Exception
                   
                 
                    Write_Log( SubName & ex.Message  )
                    ret = -1
                End Try
                Return ret
            End Function

            Function get_trigger_count(ByVal Board_ID As Integer, ByVal TrgCh As Integer, ByRef TrgCnt As Integer, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Integer
                Dim ret As Integer = Nothing
                Try
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    ret = APS_get_trigger_count(Board_ID, TrgCh, TrgCnt)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    Else
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    End If
                Catch ex As Exception
                   
                 
                    Write_Log( SubName & ex.Message  )
                    ret = -1
                End Try
                Return ret
            End Function

            Function reset_trigger_count(ByVal Board_ID As Integer, ByVal TrgCh As Integer, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Integer
                Dim ret As Integer = Nothing
                Try
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    ret = APS_reset_trigger_count(Board_ID, TrgCh)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    Else
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    End If
                Catch ex As Exception
                   
                 
                    Write_Log( SubName & ex.Message  )
                    ret = -1
                End Try
                Return ret

            End Function

            Function enable_trigger_fifo_cmp(ByVal Board_ID As Integer, ByVal FCmpCh As Integer, ByVal Enable As Integer, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Integer
                Dim ret As Integer = Nothing
                Try
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    ret = APS_enable_trigger_fifo_cmp(Board_ID, FCmpCh, Enable)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    Else
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    End If
                Catch ex As Exception
                   
                 
                    Write_Log( SubName & ex.Message  )
                    ret = -1
                End Try
                Return ret
            End Function

            Function get_trigger_fifo_cmp(ByVal Board_ID As Integer, ByVal FCmpCh As Integer, ByRef CmpVal As Integer, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Integer
                Dim ret As Integer = Nothing
                Try
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    ret = APS_get_trigger_fifo_cmp(Board_ID, FCmpCh, CmpVal)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    Else
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    End If
                Catch ex As Exception
                   
                 
                    Write_Log( SubName & ex.Message  )
                    ret = -1
                End Try
                Return ret
            End Function

            Function get_trigger_fifo_status(ByVal Board_ID As Integer, ByVal FCmpCh As Integer, ByVal FifoSts As Integer, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Integer
                Dim ret As Integer = Nothing
                Try
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    ret = APS_get_trigger_fifo_status(Board_ID, FCmpCh, FifoSts)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    Else
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    End If
                Catch ex As Exception
                   
                 
                    Write_Log( SubName & ex.Message  )
                    ret = -1
                End Try
                Return ret
            End Function

            Function set_trigger_fifo_data(ByVal Board_ID As Integer, ByVal FCmpCh As Integer, ByRef DataArr As Integer, ByVal ArraySize As Integer, ByVal ShiftFlag As Integer, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Integer
                Dim ret As Integer = Nothing
                Try
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    ret = APS_set_trigger_fifo_data(Board_ID, FCmpCh, DataArr, ArraySize, ShiftFlag)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    Else
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    End If
                Catch ex As Exception
                   
                 
                    Write_Log( SubName & ex.Message  )
                    ret = -1
                End Try
                Return ret
            End Function

            Function set_trigger_encoder_counter(ByVal Board_ID As Integer, ByVal TrgCh As Integer, ByVal TrgCnt As Integer, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Integer
                Dim ret As Integer = Nothing
                Try
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    ret = APS_set_trigger_encoder_counter(Board_ID, TrgCh, TrgCnt)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    Else
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    End If
                Catch ex As Exception
                   
                 
                    Write_Log( SubName & ex.Message  )
                    ret = -1
                End Try
                Return ret
            End Function

            Function get_trigger_encoder_counter(ByVal Board_ID As Integer, ByVal TrgCh As Integer, ByRef TrgCnt As Integer, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Integer
                Dim ret As Integer = Nothing
                Try
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    ret = APS_get_trigger_encoder_counter(Board_ID, TrgCh, TrgCnt)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    Else
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    End If
                Catch ex As Exception
                   
                 
                    Write_Log( SubName & ex.Message  )
                    ret = -1
                End Try
                Return ret
            End Function

            Function get_pulser_counter(ByVal Board_ID As Integer, ByRef Counter As Integer, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Integer
                Dim ret As Integer = Nothing
                Try
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    ret = APS_get_pulser_counter(Board_ID, Counter)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    Else
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    End If
                Catch ex As Exception
                   
                 
                    Write_Log( SubName & ex.Message  )
                    ret = -1
                End Try
                Return ret
            End Function

            Function set_pulser_counter(ByVal Board_ID As Integer, ByVal Counter As Integer, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Integer
                Dim ret As Integer = Nothing
                Try
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    ret = APS_set_pulser_counter(Board_ID, Counter)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    Else
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    End If
                Catch ex As Exception
                   
                 
                    Write_Log( SubName & ex.Message  )
                    ret = -1
                End Try
                Return ret
            End Function

            Function rescan_CF(ByVal Board_ID As Integer, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Integer
                Dim ret As Integer = Nothing
                Try
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    ret = APS_rescan_CF(Board_ID)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    Else
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    End If
                Catch ex As Exception
                   
                 
                    Write_Log( SubName & ex.Message  )
                    ret = -1
                End Try
                Return ret
            End Function

            Function get_battery_status(ByVal Board_ID As Integer, ByRef Battery_status As Integer, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Integer
                Dim ret As Integer = Nothing
                Try
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    ret = APS_get_battery_status(Board_ID, Battery_status)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    Else
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    End If
                Catch ex As Exception
                   
                 
                    Write_Log( SubName & ex.Message  )
                    ret = -1
                End Try
                Return ret
            End Function

            Function get_timer_counter(ByVal Board_ID As Integer, ByVal TmrCh As Integer, ByRef Cnt As Integer, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Integer
                Dim ret As Integer = Nothing
                Try
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    ret = APS_get_timer_counter(Board_ID, TmrCh, Cnt)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    Else
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    End If
                Catch ex As Exception
                   
                 
                    Write_Log( SubName & ex.Message  )
                    ret = -1
                End Try
                Return ret
            End Function

            Function set_timer_counter(ByVal Board_ID As Integer, ByVal TmrCh As Integer, ByVal Cnt As Integer, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Integer
                Dim ret As Integer = Nothing
                Try
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    ret = APS_set_timer_counter(Board_ID, TmrCh, Cnt)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    Else
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    End If
                Catch ex As Exception
                   
                 
                    Write_Log( SubName & ex.Message  )
                    ret = -1
                End Try
                Return ret
            End Function

            Function set_axis_param_f(ByVal Axis_Id As PRA_AXIS_ID_ENUM, ByVal AXS_Param_No As PAR_AMETER_TABLE_ENUM, ByVal AXS_Param As Double, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Integer
                Dim ret As Integer = Nothing
                Try
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    ret = APS_set_axis_param_f(Axis_Id, AXS_Param_No, AXS_Param)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    Else
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    End If
                Catch ex As Exception
                   
                 
                    Write_Log( SubName & ex.Message  )
                    ret = -1
                End Try
                Return ret
            End Function

            Function get_axis_param_f(ByVal Axis_Id As PRA_AXIS_ID_ENUM, ByVal AXS_Param_No As PAR_AMETER_TABLE_ENUM, ByRef AXS_Param As Double, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Integer
                Dim ret As Integer = Nothing
                Try
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    ret = APS_get_axis_param_f(Axis_Id, AXS_Param_No, AXS_Param)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    Else
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    End If
                Catch ex As Exception
                   
                 
                    Write_Log( SubName & ex.Message  )
                    ret = -1
                End Try
                Return ret
            End Function

            Function get_eep_curr_drv_ctrl_mode(ByVal Board_ID As Integer, ByRef ModeInBit As Integer, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Integer
                Dim ret As Integer = Nothing
                Try
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    ret = APS_get_eep_curr_drv_ctrl_mode(Board_ID, ModeInBit)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    Else
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    End If
                Catch ex As Exception
                   
                 
                    Write_Log( SubName & ex.Message  )
                    ret = -1
                End Try
                Return ret
            End Function

            Function get_command_f(ByVal Axis_Id As PRA_AXIS_ID_ENUM, ByRef Command As Double, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Integer
                Dim ret As Integer = Nothing
                Try
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    ret = APS_get_command_f(Axis_Id, Command)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    Else
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    End If
                Catch ex As Exception
                   
                 
                    Write_Log( SubName & ex.Message  )
                    ret = -1
                End Try
                Return ret
            End Function

            Function set_command_f(ByVal Axis_Id As PRA_AXIS_ID_ENUM, ByVal Command As Double, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Integer
                Dim ret As Integer = Nothing
                Try
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    ret = APS_get_command_f(Axis_Id, Command)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    Else
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    End If
                Catch ex As Exception
                   
                 
                    Write_Log( SubName & ex.Message  )
                    ret = -1
                End Try
                Return ret
            End Function

            Function get_position_f(ByVal Axis_Id As PRA_AXIS_ID_ENUM, ByRef Position As Double, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Integer
                Dim ret As Integer = Nothing
                Try
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    ret = APS_get_position_f(Axis_Id, Position)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    Else
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    End If
                Catch ex As Exception
                   
                 
                    Write_Log( SubName & ex.Message  )
                    ret = -1
                End Try
                Return ret
            End Function

            Function set_position_f(ByVal Axis_Id As PRA_AXIS_ID_ENUM, ByVal Position As Double, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Integer
                Dim ret As Integer = Nothing
                Try
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    ret = APS_set_position_f(Axis_Id, Position)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    Else
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    End If
                Catch ex As Exception
                   
                 
                    Write_Log( SubName & ex.Message  )
                    ret = -1
                End Try
                Return ret
            End Function

            Function get_command_velocity_f(ByVal Axis_Id As PRA_AXIS_ID_ENUM, ByRef Velocity As Double, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Integer
                Dim ret As Integer = Nothing
                Try
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    ret = APS_get_command_velocity_f(Axis_Id, Velocity)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    Else
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    End If
                Catch ex As Exception
                   
                 
                    Write_Log( SubName & ex.Message  )
                    ret = -1
                End Try
                Return ret
            End Function

            Function get_target_position_f(ByVal Axis_Id As PRA_AXIS_ID_ENUM, ByRef Targ_Pos As Double, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Integer
                Dim ret As Integer = Nothing
                Try
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    ret = APS_get_target_position_f(Axis_Id, Targ_Pos)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    Else
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    End If
                Catch ex As Exception
                   
                 
                    Write_Log( SubName & ex.Message  )
                    ret = -1
                End Try
                Return ret
            End Function

            Function get_error_position_f(ByVal Axis_Id As PRA_AXIS_ID_ENUM, ByRef Err_Pos As Double, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Integer
                Dim ret As Integer = Nothing
                Try
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    ret = APS_get_error_position_f(Axis_Id, Err_Pos)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    Else
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    End If
                Catch ex As Exception
                   
                 
                    Write_Log( SubName & ex.Message  )
                    ret = -1
                End Try
                Return ret
            End Function

            Function get_feedback_velocity_f(ByVal Axis_Id As PRA_AXIS_ID_ENUM, ByRef Velocity As Double, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Integer
                Dim ret As Integer = Nothing
                Try
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    ret = APS_get_feedback_velocity_f(Axis_Id, Velocity)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    Else
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    End If
                Catch ex As Exception
                   
                 
                    Write_Log( SubName & ex.Message  )
                    ret = -1
                End Try
                Return ret
            End Function



            Function ptp_v(ByVal Axis_Id As PRA_AXIS_ID_ENUM, ByVal APS_Option As PRA_APS_OPTION_ENUM, ByVal Position As Double, ByVal Vm As Double, ByRef Wait As ASYNCALL, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Integer
                Dim ret As Integer = Nothing
                Try
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    ret = APS_ptp_v(Axis_Id, APS_Option, Position, Vm, Wait)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    Else
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    End If
                Catch ex As Exception
                   
                 
                    Write_Log( SubName & ex.Message  )
                    ret = -1
                End Try
                Return ret
            End Function

            Function ptp_all(ByVal Axis_Id As PRA_AXIS_ID_ENUM, ByVal APS_Option As PRA_APS_OPTION_ENUM, ByVal Position As Double, ByVal Vs As Double, ByVal Vm As Double, ByVal Ve As Double, ByVal Acc As Double, ByVal Dec As Double, ByVal SFac As Double, ByRef Wait As ASYNCALL, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Integer
                Dim ret As Integer = Nothing
                Try
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    ret = APS_ptp_all(Axis_Id, APS_Option, Position, Vs, Vm, Ve, Acc, Dec, SFac, Wait)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    Else
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    End If
                Catch ex As Exception
                   
                 
                    Write_Log( SubName & ex.Message  )
                    ret = -1
                End Try
                Return ret
            End Function

            Function vel(ByVal Axis_Id As PRA_AXIS_ID_ENUM, ByVal APS_Option As PRA_APS_OPTION_ENUM, ByVal Vm As Double, ByRef Wait As ASYNCALL, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Integer
                Dim ret As Integer = Nothing
                Try
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    ret = APS_vel(Axis_Id, APS_Option, Vm, Wait)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    Else
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    End If
                Catch ex As Exception
                   
                 
                    Write_Log( SubName & ex.Message  )
                    ret = -1
                End Try
                Return ret
            End Function

            Function vel_all(ByVal Axis_Id As PRA_AXIS_ID_ENUM, ByVal APS_Option As PRA_APS_OPTION_ENUM, ByVal Vs As Double, ByVal Vm As Double, ByVal Ve As Double, ByVal Acc As Double, ByVal Dec As Double, ByVal SFac As Double, ByRef Wait As ASYNCALL, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Integer
                Dim ret As Integer = Nothing
                Try
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    ret = APS_vel_all(Axis_Id, APS_Option, Vs, Vm, Ve, Acc, Dec, SFac, Wait)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    Else
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    End If
                Catch ex As Exception
                   
                 
                    Write_Log( SubName & ex.Message  )
                    ret = -1
                End Try
                Return ret
            End Function

            Function line(ByVal Dimension As Integer, ByVal Axis_Id_Array() As Integer, ByVal APS_Option As PRA_APS_OPTION_ENUM, ByVal PositionArray() As Double, ByRef TransPara As Double, ByRef Wait As ASYNCALL, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Integer
                Dim ret As Integer = Nothing
                Try
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    ret = APS_line(Dimension, Axis_Id_Array, APS_Option, PositionArray, TransPara, Wait)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    Else
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    End If
                Catch ex As Exception
                   
                 
                    Write_Log( SubName & ex.Message  )
                    ret = -1
                End Try
                Return ret
            End Function

            Function line_v(ByVal Dimension As Integer, ByVal Axis_Id_Array() As Integer, ByVal APS_Option As PRA_APS_OPTION_ENUM, ByVal PositionArray() As Double, ByRef TransPara As Double, ByVal Vm As Double, ByRef Wait As ASYNCALL, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Integer
                Dim ret As Integer = Nothing
                Try
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    ret = APS_line_v(Dimension, Axis_Id_Array, APS_Option, PositionArray, TransPara, Vm, Wait)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    Else
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    End If
                Catch ex As Exception
                   
                 
                    Write_Log( SubName & ex.Message  )
                    ret = -1
                End Try
                Return ret
            End Function

            Function line_all(ByVal Dimension As Integer, ByVal Axis_Id_Array() As Integer, ByVal APS_Option As PRA_APS_OPTION_ENUM, ByVal PositionArray() As Double, ByRef TransPara As Double, ByVal Vs As Double, ByVal Vm As Double, ByVal Ve As Double, ByVal Acc As Double, ByVal Dec As Double, ByVal SFac As Double, ByRef Wait As ASYNCALL, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Integer
                Dim ret As Integer = Nothing
                Try
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    ret = APS_line_all(Dimension, Axis_Id_Array, APS_Option, PositionArray, TransPara, Vs, Vm, Ve, Acc, Dec, SFac, Wait)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    Else
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    End If
                Catch ex As Exception
                   
                 
                    Write_Log( SubName & ex.Message  )
                    ret = -1
                End Try
                Return ret
            End Function

            Function arc2_ca(ByVal Axis_Id_Array() As Integer, ByVal APS_Option As PRA_APS_OPTION_ENUM, ByVal CenterArray() As Double, ByVal Angle As Double, ByRef TransPara As Double, ByRef Wait As ASYNCALL, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Integer
                Dim ret As Integer = Nothing
                Try
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    ret = APS_arc2_ca(Axis_Id_Array, APS_Option, CenterArray, Angle, TransPara, Wait)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    Else
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    End If
                Catch ex As Exception
                   
                 
                    Write_Log( SubName & ex.Message  )
                    ret = -1
                End Try
                Return ret
            End Function

            Function arc2_ca_v(ByVal Axis_Id_Array() As Integer, ByVal APS_Option As PRA_APS_OPTION_ENUM, ByVal CenterArray() As Double, ByVal Angle As Double, ByRef TransPara As Double, ByVal Vm As Double, ByRef Wait As ASYNCALL, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Integer
                Dim ret As Integer = Nothing
                Try
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    ret = APS_arc2_ca_v(Axis_Id_Array, APS_Option, CenterArray, Angle, TransPara, Vm, Wait)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    Else
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    End If
                Catch ex As Exception
                   
                 
                    Write_Log( SubName & ex.Message  )
                    ret = -1
                End Try
                Return ret
            End Function

            Function arc2_ca_all(ByVal Axis_Id_Array() As Integer, ByVal APS_Option As PRA_APS_OPTION_ENUM, ByVal CenterArray() As Double, ByVal Angle As Double, ByRef TransPara As Double, ByVal Vs As Double, ByVal Vm As Double, ByVal Ve As Double, ByVal Acc As Double, ByVal Dec As Double, ByVal SFac As Double, ByRef Wait As ASYNCALL, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Integer
                Dim ret As Integer = Nothing
                Try
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    ret = APS_arc2_ca_all(Axis_Id_Array, APS_Option, CenterArray, Angle, TransPara, Vs, Vm, Ve, Acc, Dec, SFac, Wait)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    Else
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    End If
                Catch ex As Exception
                   
                 
                    Write_Log( SubName & ex.Message  )
                    ret = -1

                End Try
                Return ret
            End Function

            Function arc2_ce(ByVal Axis_Id_Array() As Integer, ByVal APS_Option As PRA_APS_OPTION_ENUM, ByVal CenterArray() As Double, ByVal EndArray() As Double, ByVal Dir As Short, ByRef TransPara As Double, ByRef Wait As ASYNCALL, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Integer
                Dim ret As Integer = Nothing
                Try
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    ret = APS_arc2_ce(Axis_Id_Array, APS_Option, CenterArray, EndArray, Dir, TransPara, Wait)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    Else
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    End If
                Catch ex As Exception
                   
                 
                    Write_Log( SubName & ex.Message  )
                    ret = -1
                End Try
                Return ret
            End Function

            Function arc2_ce_v(ByVal Axis_Id_Array() As Integer, ByVal APS_Option As PRA_APS_OPTION_ENUM, ByVal CenterArray() As Double, ByVal EndArray() As Double, ByVal Dir As Short, ByRef TransPara As Double, ByVal Vm As Double, ByRef Wait As ASYNCALL, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Integer
                Dim ret As Integer = Nothing
                Try
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    ret = APS_arc2_ce_v(Axis_Id_Array, APS_Option, CenterArray, EndArray, Dir, TransPara, Vm, Wait)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    Else
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    End If
                Catch ex As Exception
                   
                 
                    Write_Log( SubName & ex.Message  )
                    ret = -1
                End Try
                Return ret
            End Function

            Function arc2_ce_all(ByVal Axis_Id_Array() As Integer, ByVal APS_Option As PRA_APS_OPTION_ENUM, ByVal CenterArray() As Double, ByVal EndArray() As Double, ByVal Dir As Short, ByRef TransPara As Double, ByVal Vs As Double, ByVal Vm As Double, ByVal Ve As Double, ByVal Acc As Double, ByVal Dec As Double, ByVal SFac As Double, ByRef Wait As ASYNCALL, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Integer
                Dim ret As Integer = Nothing
                Try
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    ret = APS_arc2_ce_all(Axis_Id_Array, APS_Option, CenterArray, EndArray, Dir, TransPara, Vs, Vm, Ve, Acc, Dec, SFac, Wait)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    Else
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    End If
                Catch ex As Exception
                   
                 
                    Write_Log( SubName & ex.Message  )
                    ret = -1
                End Try
                Return ret
            End Function

            Function arc3_ca(ByVal Axis_Id_Array() As Integer, ByVal APS_Option As PRA_APS_OPTION_ENUM, ByVal CenterArray() As Double, ByVal NormalArray() As Double, ByVal Angle As Double, ByRef TransPara As Double, ByRef Wait As ASYNCALL, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Integer
                Dim ret As Integer = Nothing
                Try
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    ret = APS_arc3_ca(Axis_Id_Array, APS_Option, CenterArray, NormalArray, Angle, TransPara, Wait)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    Else
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    End If
                Catch ex As Exception
                   
                 
                    Write_Log( SubName & ex.Message  )
                    ret = -1
                End Try
                Return ret
            End Function

            Function arc3_ca_v(ByVal Axis_Id_Array() As Integer, ByVal APS_Option As PRA_APS_OPTION_ENUM, ByVal CenterArray() As Double, ByVal NormalArray() As Double, ByVal Angle As Double, ByRef TransPara As Double, ByVal Vm As Double, ByRef Wait As ASYNCALL, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Integer
                Dim ret As Integer = Nothing
                Try
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    ret = APS_arc3_ca_v(Axis_Id_Array, APS_Option, CenterArray, NormalArray, Angle, TransPara, Vm, Wait)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    Else
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    End If
                Catch ex As Exception
                   
                 
                    Write_Log( SubName & ex.Message  )
                    ret = -1
                End Try
                Return ret
            End Function

            Function arc3_ca_all(ByVal Axis_Id_Array() As Integer, ByVal APS_Option As PRA_APS_OPTION_ENUM, ByVal CenterArray() As Double, ByVal NormalArray() As Double, ByVal Angle As Double, ByRef TransPara As Double, ByVal Vs As Double, ByVal Vm As Double, ByVal Ve As Double, ByVal Acc As Double, ByVal Dec As Double, ByVal SFac As Double, ByRef Wait As ASYNCALL, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Integer
                Dim ret As Integer = Nothing
                Try
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    ret = APS_arc3_ca_all(Axis_Id_Array, APS_Option, CenterArray, NormalArray, Angle, TransPara, Vs, Vm, Ve, Acc, Dec, SFac, Wait)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    Else
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    End If
                Catch ex As Exception
                   
                 
                    Write_Log( SubName & ex.Message  )
                    ret = -1
                End Try
                Return ret
            End Function

            Function arc3_ce(ByVal Axis_Id_Array() As Integer, ByVal APS_Option As PRA_APS_OPTION_ENUM, ByVal CenterArray() As Double, ByVal EndArray() As Double, ByVal Dir As Short, ByRef TransPara As Double, ByRef Wait As ASYNCALL, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Integer
                Dim ret As Integer = Nothing
                Try
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    ret = APS_arc3_ce(Axis_Id_Array, APS_Option, CenterArray, EndArray, Dir, TransPara, Wait)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    Else
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    End If
                Catch ex As Exception
                   
                 
                    Write_Log( SubName & ex.Message  )
                    ret = -1
                End Try
                Return ret
            End Function

            Function arc3_ce_v(ByVal Axis_Id_Array() As Integer, ByVal APS_Option As PRA_APS_OPTION_ENUM, ByVal CenterArray() As Double, ByVal EndArray() As Double, ByVal Dir As Short, ByRef TransPara As Double, ByVal Vm As Double, ByRef Wait As ASYNCALL, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Integer
                Dim ret As Integer = Nothing
                Try
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    ret = APS_arc3_ce_v(Axis_Id_Array, APS_Option, CenterArray, EndArray, Dir, TransPara, Vm, Wait)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    Else
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    End If
                Catch ex As Exception
                   
                 
                    Write_Log( SubName & ex.Message  )
                    ret = -1
                End Try
                Return ret
            End Function

            Function arc3_ce_all(ByVal Axis_Id_Array() As Integer, ByVal APS_Option As PRA_APS_OPTION_ENUM, ByVal CenterArray() As Double, ByVal EndArray() As Double, ByVal Dir As Short, ByRef TransPara As Double, ByVal Vs As Double, ByVal Vm As Double, ByVal Ve As Double, ByVal Acc As Double, ByVal Dec As Double, ByVal SFac As Double, ByRef Wait As ASYNCALL, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Integer
                Dim ret As Integer = Nothing
                Try
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    ret = APS_arc3_ce_all(Axis_Id_Array, APS_Option, CenterArray, EndArray, Dir, TransPara, Vs, Vm, Ve, Acc, Dec, SFac, Wait)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    Else
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    End If
                Catch ex As Exception
                   
                 
                    Write_Log( SubName & ex.Message  )
                    ret = -1
                End Try
                Return ret
            End Function

            Function spiral_ca(ByVal Axis_Id_Array() As Integer, ByVal APS_Option As PRA_APS_OPTION_ENUM, ByVal CenterArray() As Double, ByVal NormalArray() As Double, ByVal Angle As Double, ByVal DeltaH As Double, ByVal FinalR As Double, ByRef TransPara As Double, ByRef Wait As ASYNCALL, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Integer
                Dim ret As Integer = Nothing
                Try
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    ret = APS_spiral_ca(Axis_Id_Array, APS_Option, CenterArray, NormalArray, Angle, DeltaH, FinalR, TransPara, Wait)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    Else
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    End If
                Catch ex As Exception
                   
                 
                    Write_Log( SubName & ex.Message  )
                    ret = -1

                End Try
                Return ret
            End Function

            Function spiral_ca_v(ByVal Axis_Id_Array() As Integer, ByVal APS_Option As PRA_APS_OPTION_ENUM, ByVal CenterArray() As Double, ByVal NormalArray() As Double, ByVal Angle As Double, ByVal DeltaH As Double, ByVal FinalR As Double, ByRef TransPara As Double, ByVal Vm As Double, ByRef Wait As ASYNCALL, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Integer
                Dim ret As Integer = Nothing
                Try
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    ret = APS_spiral_ca_v(Axis_Id_Array, APS_Option, CenterArray, NormalArray, Angle, DeltaH, FinalR, TransPara, Vm, Wait)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    Else
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    End If
                Catch ex As Exception
                   
                 
                    Write_Log( SubName & ex.Message  )
                    ret = -1
                End Try
                Return ret
            End Function

            Function spiral_ca_all(ByVal Axis_Id_Array() As Integer, ByVal APS_Option As PRA_APS_OPTION_ENUM, ByVal CenterArray() As Double, ByVal NormalArray() As Double, ByVal Angle As Double, ByVal DeltaH As Double, ByVal FinalR As Double, ByRef TransPara As Double, ByVal Vs As Double, ByVal Vm As Double, ByVal Ve As Double, ByVal Acc As Double, ByVal Dec As Double, ByVal SFac As Double, ByRef Wait As ASYNCALL, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Integer
                Dim ret As Integer = Nothing
                Try
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    ret = APS_spiral_ca_all(Axis_Id_Array, APS_Option, CenterArray, NormalArray, Angle, DeltaH, FinalR, TransPara, Vs, Vm, Ve, Acc, Dec, SFac, Wait)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    Else
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    End If
                Catch ex As Exception
                   
                 
                    Write_Log( SubName & ex.Message  )
                    ret = -1
                End Try
                Return ret
            End Function

            Function spiral_ce(ByVal Axis_Id_Array() As Integer, ByVal APS_Option As PRA_APS_OPTION_ENUM, ByVal CenterArray() As Double, ByVal NormalArray() As Double, ByVal EndArray() As Double, ByVal Dir As Short, ByRef TransPara As Double, ByRef Wait As ASYNCALL, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Integer
                Dim ret As Integer = Nothing
                Try
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    ret = APS_spiral_ce(Axis_Id_Array, APS_Option, CenterArray, NormalArray, EndArray, Dir, TransPara, Wait)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    Else
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    End If
                Catch ex As Exception
                   
                 
                    Write_Log( SubName & ex.Message  )
                    ret = -1
                End Try
                Return ret
            End Function

            Function spiral_ce_v(ByVal Axis_Id_Array() As Integer, ByVal APS_Option As PRA_APS_OPTION_ENUM, ByVal CenterArray() As Double, ByVal NormalArray() As Double, ByVal EndArray() As Double, ByVal Dir As Short, ByRef TransPara As Double, ByVal Vm As Double, ByRef Wait As ASYNCALL, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Integer
                Dim ret As Integer = Nothing
                Try
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    ret = APS_spiral_ce_v(Axis_Id_Array, APS_Option, CenterArray, NormalArray, EndArray, Dir, TransPara, Vm, Wait)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    Else
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    End If
                Catch ex As Exception
                   
                 
                    Write_Log( SubName & ex.Message  )
                    ret = -1
                End Try
                Return ret
            End Function

            Function spiral_ce_all(ByVal Axis_Id_Array() As Integer, ByVal APS_Option As PRA_APS_OPTION_ENUM, ByVal CenterArray() As Double, ByVal NormalArray() As Double, ByVal EndArray() As Double, ByVal Dir As Short, ByRef TransPara As Double, ByVal Vs As Double, ByVal Vm As Double, ByVal Ve As Double, ByVal Acc As Double, ByVal Dec As Double, ByVal SFac As Double, ByRef Wait As ASYNCALL, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Integer
                Dim ret As Integer = Nothing
                Try
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    ret = APS_spiral_ce_all(Axis_Id_Array, APS_Option, CenterArray, NormalArray, EndArray, Dir, TransPara, Vs, Vm, Ve, Acc, Dec, SFac, Wait)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    Else
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    End If
                Catch ex As Exception
                   
                 
                    Write_Log( SubName & ex.Message  )
                    ret = -1
                End Try
                Return ret
            End Function

            Function pt_dwell(ByVal Board_ID As Integer, ByVal PtbId As Integer, ByRef Prof As PTDWL, ByRef Status As PTSTS, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Integer
                Dim ret As Integer = Nothing
                Try
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    ret = APS_pt_dwell(Board_ID, PtbId, Prof, Status)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    Else
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    End If
                Catch ex As Exception
                   
                 
                    Write_Log( SubName & ex.Message  )
                    ret = -1
                End Try
                Return ret
            End Function

            Function pt_line(ByVal Board_ID As Integer, ByVal PtbId As Integer, ByRef Prof As PTLINE, ByRef Status As PTSTS, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Integer
                Dim ret As Integer = Nothing
                Try
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    ret = APS_pt_line(Board_ID, PtbId, Prof, Status)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    Else
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    End If
                Catch ex As Exception
                   
                 
                    Write_Log( SubName & ex.Message  )
                    ret = -1
                End Try
                Return ret
            End Function

            Function pt_arc2_ca(ByVal Board_ID As Integer, ByVal PtbId As Integer, ByRef Prof As PTA2CA, ByRef Status As PTSTS, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Integer
                Dim ret As Integer = Nothing
                Try
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    ret = APS_pt_arc2_ca(Board_ID, PtbId, Prof, Status)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    Else
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    End If
                Catch ex As Exception
                   
                 
                    Write_Log( SubName & ex.Message  )
                    ret = -1
                End Try
                Return ret
            End Function

            Function pt_arc2_ce(ByVal Board_ID As Integer, ByVal PtbId As Integer, ByRef Prof As PTA2CE, ByRef Status As PTSTS, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Integer
                Dim ret As Integer = Nothing
                Try
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    ret = APS_pt_arc2_ce(Board_ID, PtbId, Prof, Status)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    Else
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    End If
                Catch ex As Exception
                   
                 
                    Write_Log( SubName & ex.Message  )
                    ret = -1
                End Try
                Return ret
            End Function

            Function pt_arc3_ca(ByVal Board_ID As Integer, ByVal PtbId As Integer, ByRef Prof As PTA3CA, ByRef Status As PTSTS, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Integer
                Dim ret As Integer = Nothing
                Try
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    ret = APS_pt_arc3_ca(Board_ID, PtbId, Prof, Status)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    Else
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    End If
                Catch ex As Exception
                   
                 
                    Write_Log( SubName & ex.Message  )
                    ret = -1
                End Try
                Return ret
            End Function

            Function pt_arc3_ce(ByVal Board_ID As Integer, ByVal PtbId As Integer, ByRef Prof As PTA3CE, ByRef Status As PTSTS, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Integer
                Dim ret As Integer = Nothing
                Try
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    ret = APS_pt_arc3_ce(Board_ID, PtbId, Prof, Status)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    Else
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    End If
                Catch ex As Exception
                   
                 
                    Write_Log( SubName & ex.Message  )
                    ret = -1
                End Try
                Return ret
            End Function

            Function pt_spiral_ca(ByVal Board_ID As Integer, ByVal PtbId As Integer, ByRef Prof As PTHCA, ByRef Status As PTSTS, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Integer
                Dim ret As Integer = Nothing
                Try
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    ret = APS_pt_spiral_ca(Board_ID, PtbId, Prof, Status)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    Else
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    End If
                Catch ex As Exception
                   
                 
                    Write_Log( SubName & ex.Message  )
                    ret = -1
                End Try
                Return ret
            End Function

            Function pt_spiral_ce(ByVal Board_ID As Integer, ByVal PtbId As Integer, ByRef Prof As PTHCE, ByRef Status As PTSTS, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Integer
                Dim ret As Integer = Nothing
                Try
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    ret = APS_pt_spiral_ce(Board_ID, PtbId, Prof, Status)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    Else
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    End If
                Catch ex As Exception
                   
                 
                    Write_Log( SubName & ex.Message  )
                    ret = -1
                End Try
                Return ret
            End Function

            Function pt_enable(ByVal Board_ID As Integer, ByVal PtbId As Integer, ByVal Dimension As Integer, ByVal AxisArr() As Integer, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Integer
                Dim ret As Integer = Nothing
                Try
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    ret = APS_pt_enable(Board_ID, PtbId, Dimension, AxisArr)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    Else
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    End If
                Catch ex As Exception
                   
                 
                    Write_Log( SubName & ex.Message  )
                    ret = -1
                End Try
                Return ret
            End Function

            Function pt_disable(ByVal Board_ID As Integer, ByVal PtbId As Integer, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Integer
                Dim ret As Integer = Nothing
                Try
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    ret = APS_pt_disable(Board_ID, PtbId)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    Else
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    End If
                Catch ex As Exception
                   
                 
                    Write_Log( SubName & ex.Message  )
                    ret = -1
                End Try
                Return ret
            End Function

            Function get_pt_info(ByVal Board_ID As Integer, ByVal PtbId As Integer, ByRef Info As PTINFO, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Integer
                Dim ret As Integer = Nothing
                Try
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    ret = APS_get_pt_info(Board_ID, PtbId, Info)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    Else
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    End If
                Catch ex As Exception
                   
                 
                    Write_Log( SubName & ex.Message  )
                    ret = -1
                End Try
                Return ret
            End Function

            Function pt_set_vs(ByVal Board_ID As Integer, ByVal PtbId As Integer, ByVal Vs As Double, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Integer
                Dim ret As Integer = Nothing
                Try
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    ret = APS_pt_set_vs(Board_ID, PtbId, Vs)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    Else
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    End If
                Catch ex As Exception
                   
                 
                    Write_Log( SubName & ex.Message  )
                    ret = -1
                End Try
                Return ret
            End Function

            Function pt_get_vs(ByVal Board_ID As Integer, ByVal PtbId As Integer, ByRef Vs As Double, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Integer
                Dim ret As Integer = Nothing
                Try
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    ret = APS_pt_get_vs(Board_ID, PtbId, Vs)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    Else
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    End If
                Catch ex As Exception
                   
                 
                    Write_Log( SubName & ex.Message  )
                    ret = -1
                End Try
                Return ret
            End Function

            Function pt_start(ByVal Board_ID As Integer, ByVal PtbId As Integer, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Integer
                Dim ret As Integer = Nothing
                Try
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    ret = APS_pt_start(Board_ID, PtbId)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    Else
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    End If
                Catch ex As Exception
                   
                 
                    Write_Log( SubName & ex.Message  )
                    ret = -1
                End Try
                Return ret
            End Function

            Function pt_stop(ByVal Board_ID As Integer, ByVal PtbId As Integer, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Integer
                Dim ret As Integer = Nothing
                Try
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    ret = APS_pt_stop(Board_ID, PtbId)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    Else
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    End If
                Catch ex As Exception
                   
                 
                    Write_Log( SubName & ex.Message  )
                    ret = -1
                End Try
                Return ret
            End Function

            Function get_pt_status(ByVal Board_ID As Integer, ByVal PtbId As Integer, ByRef Status As PTSTS, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Integer
                Dim ret As Integer = Nothing
                Try
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    ret = APS_get_pt_status(Board_ID, PtbId, Status)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    Else
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    End If
                Catch ex As Exception
                   
                 
                    Write_Log( SubName & ex.Message  )
                    ret = -1
                End Try
                Return ret
            End Function

            Function reset_pt_buffer(ByVal Board_ID As Integer, ByVal PtbId As Integer, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Integer
                Dim ret As Integer = Nothing
                Try
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    ret = APS_reset_pt_buffer(Board_ID, PtbId)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    Else
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    End If
                Catch ex As Exception
                   
                 
                    Write_Log( SubName & ex.Message  )
                    ret = -1
                End Try
                Return ret
            End Function

            Function pt_roll_back(ByVal Board_ID As Integer, ByVal PtbId As Integer, ByVal Max_Speed As Double, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Integer
                Dim ret As Integer = Nothing
                Try
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    ret = APS_pt_roll_back(Board_ID, PtbId, Max_Speed)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    Else
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    End If
                Catch ex As Exception
                   
                 
                    Write_Log( SubName & ex.Message  )
                    ret = -1
                End Try
                Return ret
            End Function

            Function pt_get_error(ByVal Board_ID As Integer, ByVal PtbId As Integer, ByRef ErrCode As Integer, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Integer
                Dim ret As Integer = Nothing
                Try
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    ret = APS_pt_get_error(Board_ID, PtbId, ErrCode)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    Else
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    End If
                Catch ex As Exception
                   
                 
                    Write_Log( SubName & ex.Message  )
                    ret = -1
                End Try
                Return ret
            End Function

            Function pt_ext_set_do_ch(ByVal Board_ID As Integer, ByVal PtbId As Integer, ByVal Channel As Integer, ByVal OnOff As Integer, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Integer
                Dim ret As Integer = Nothing
                Try
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    ret = APS_pt_ext_set_do_ch(Board_ID, PtbId, Channel, OnOff)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    Else
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    End If
                Catch ex As Exception
                   
                 
                    Write_Log( SubName & ex.Message  )
                    ret = -1
                End Try
                Return ret
            End Function

            Function pt_ext_set_table_no(ByVal Board_ID As Integer, ByVal PtbId As Integer, ByVal CtrlNo As Integer, ByVal TableNo As Integer, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Integer
                Dim ret As Integer = Nothing
                Try
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    ret = APS_pt_ext_set_table_no(Board_ID, PtbId, CtrlNo, TableNo)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    Else
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    End If
                Catch ex As Exception
                   
                 
                    Write_Log( SubName & ex.Message  )
                    ret = -1
                End Try
                Return ret
            End Function

            Function pt_set_absolute(ByVal Board_ID As Integer, ByVal PtbId As Integer, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Integer
                Dim ret As Integer = Nothing
                Try
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    ret = APS_pt_set_absolute(Board_ID, PtbId)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    Else
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    End If
                Catch ex As Exception
                   
                 
                    Write_Log( SubName & ex.Message  )
                    ret = -1
                End Try
                Return ret
            End Function

            Function pt_set_relative(ByVal Board_ID As Integer, ByVal PtbId As Integer, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Integer
                Dim ret As Integer = Nothing
                Try
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    ret = APS_pt_set_relative(Board_ID, PtbId)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    Else
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    End If
                Catch ex As Exception
                   
                 
                    Write_Log( SubName & ex.Message  )
                    ret = -1
                End Try
                Return ret
            End Function

            Function pt_set_trans_buffered(ByVal Board_ID As Integer, ByVal PtbId As Integer, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Integer
                Dim ret As Integer = Nothing
                Try
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    ret = APS_pt_set_trans_buffered(Board_ID, PtbId)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    Else
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    End If
                Catch ex As Exception
                   
                 
                    Write_Log( SubName & ex.Message  )
                    ret = -1
                End Try
                Return ret
            End Function

            Function pt_set_trans_inp(ByVal Board_ID As Integer, ByVal PtbId As Integer, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Integer
                Dim ret As Integer = Nothing
                Try
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    ret = APS_pt_set_trans_inp(Board_ID, PtbId)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    Else
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    End If
                Catch ex As Exception
                   
                 
                    Write_Log( SubName & ex.Message  )
                    ret = -1
                End Try
                Return ret
            End Function

            Function pt_set_trans_Magnificationend_dec(ByVal Board_ID As Integer, ByVal PtbId As Integer, ByVal Bp As Double, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Integer
                Dim ret As Integer = Nothing
                Try
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    ret = APS_pt_set_trans_Magnificationend_dec(Board_ID, PtbId, Bp)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    Else
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    End If
                Catch ex As Exception
                   
                 
                    Write_Log( SubName & ex.Message  )
                    ret = -1
                End Try
                Return ret
            End Function

            Function pt_set_trans_Magnificationend_dist(ByVal Board_ID As Integer, ByVal PtbId As Integer, ByVal Bp As Double, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Integer
                Dim ret As Integer = Nothing
                Try
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    ret = APS_pt_set_trans_Magnificationend_dist(Board_ID, PtbId, Bp)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    Else
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    End If
                Catch ex As Exception
                   
                 
                    Write_Log( SubName & ex.Message  )
                    ret = -1
                End Try
                Return ret
            End Function

            Function pt_set_trans_Magnificationend_pcnt(ByVal Board_ID As Integer, ByVal PtbId As Integer, ByVal Bp As Double, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Integer
                Dim ret As Integer = Nothing
                Try
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    ret = APS_pt_set_trans_Magnificationend_pcnt(Board_ID, PtbId, Bp)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    Else
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    End If
                Catch ex As Exception
                   
                 
                    Write_Log( SubName & ex.Message  )
                    ret = -1
                End Try
                Return ret
            End Function

            Function pt_set_acc(ByVal Board_ID As Integer, ByVal PtbId As Integer, ByVal Acc As Double, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Integer
                Dim ret As Integer = Nothing
                Try
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    ret = APS_pt_set_acc(Board_ID, PtbId, Acc)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    Else
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    End If
                Catch ex As Exception
                   
                 
                    Write_Log( SubName & ex.Message  )
                    ret = -1
                End Try
                Return ret
            End Function

            Function pt_set_dec(ByVal Board_ID As Integer, ByVal PtbId As Integer, ByVal Dec As Double, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Integer
                Dim ret As Integer = Nothing
                Try
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    ret = APS_pt_set_dec(Board_ID, PtbId, Dec)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    Else
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    End If
                Catch ex As Exception
                   
                 
                    Write_Log( SubName & ex.Message  )
                    ret = -1
                End Try
                Return ret
            End Function

            Function pt_set_acc_dec(ByVal Board_ID As Integer, ByVal PtbId As Integer, ByVal AccDec As Double, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Integer
                Dim ret As Integer = Nothing
                Try
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    ret = APS_pt_set_acc_dec(Board_ID, PtbId, AccDec)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    Else
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    End If
                Catch ex As Exception
                   
                 
                    Write_Log( SubName & ex.Message  )
                    ret = -1
                End Try
                Return ret
            End Function

            Function pt_set_s(ByVal Board_ID As Integer, ByVal PtbId As Integer, ByVal Sf As Double, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Integer
                Dim ret As Integer = Nothing
                Try
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    ret = APS_pt_set_s(Board_ID, PtbId, Sf)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    Else
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    End If
                Catch ex As Exception
                   
                 
                    Write_Log( SubName & ex.Message  )
                    ret = -1
                End Try
                Return ret
            End Function

            Function pt_set_vm(ByVal Board_ID As Integer, ByVal PtbId As Integer, ByVal Vm As Double, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Integer
                Dim ret As Integer = Nothing
                Try
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    ret = APS_pt_set_vm(Board_ID, PtbId, Vm)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    Else
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    End If
                Catch ex As Exception
                   
                 
                    Write_Log( SubName & ex.Message  )
                    ret = -1
                End Try
                Return ret
            End Function

            Function pt_set_ve(ByVal Board_ID As Integer, ByVal PtbId As Integer, ByVal Ve As Double, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Integer
                Dim ret As Integer = Nothing
                Try
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    ret = APS_pt_set_ve(Board_ID, PtbId, Ve)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    Else
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    End If
                Catch ex As Exception
                   
                 
                    Write_Log( SubName & ex.Message  )
                    ret = -1
                End Try
                Return ret
            End Function

            Function move_trigger(ByVal Dimension As Integer, ByVal Axis_Id_Array() As Integer, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Integer
                Dim ret As Integer = Nothing
                Try
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    ret = APS_move_trigger(Dimension, Axis_Id_Array)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    Else
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    End If
                Catch ex As Exception
                   
                 
                    Write_Log( SubName & ex.Message  )
                    ret = -1
                End Try
                Return ret
            End Function

            Function stop_move_multi(ByVal Dimension As Integer, ByVal Axis_Id_Array() As Integer, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Integer
                Dim ret As Integer = Nothing
                Try
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    ret = APS_stop_move_multi(Dimension, Axis_Id_Array)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    Else
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    End If
                Catch ex As Exception
                   
                 
                    Write_Log( SubName & ex.Message  )
                    ret = -1
                End Try
                Return ret
            End Function

            Function emg_stop_multi(ByVal Dimension As Integer, ByVal Axis_Id_Array() As Integer, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Integer
                Dim ret As Integer = Nothing
                Try
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    ret = APS_emg_stop_multi(Dimension, Axis_Id_Array)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    Else
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    End If
                Catch ex As Exception
                   
                 
                    Write_Log( SubName & ex.Message  )
                    ret = -1
                End Try
                Return ret
            End Function

            Function start_gear(ByVal Axis_Id As PRA_AXIS_ID_ENUM, ByVal Mode As Integer, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Integer
                Dim ret As Integer = Nothing
                Try
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    ret = APS_start_gear(Axis_Id, Mode)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    Else
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    End If
                Catch ex As Exception
                   
                 
                    Write_Log( SubName & ex.Message  )
                    ret = -1
                End Try
                Return ret
            End Function

            Function get_gear_status(ByVal Axis_Id As PRA_AXIS_ID_ENUM, ByRef Status As Integer, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Integer
                Dim ret As Integer = Nothing
                Try
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    ret = APS_get_gear_status(Axis_Id, Status)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    Else
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    End If
                Catch ex As Exception
                   
                 
                    Write_Log( SubName & ex.Message  )
                    ret = -1
                End Try
                Return ret
            End Function

            ''' <summary>
            ''' 获取控制卡版本信息
            ''' </summary>
            ''' <param name="Array_size"></param>
            ''' <param name="Board_ID"></param>
            ''' <remarks></remarks>
            Function get_version_info(ByVal Board_ID As Integer, ByRef Array_size() As Integer, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Integer
                Dim ret As Integer = Nothing
                Try
                    Array_size(Dll_Version) = APS_version()
                    ret = APS_get_device_info(Board_ID, Driver_Version, Array_size(IDX_DRIVER))
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                        Return ret
                    Else
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    End If
                    ret = APS_get_device_info(Board_ID, Dsp_Kernel_Version, Array_size(IDX_KERNEL))
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                        Return ret
                    Else
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    End If
                    ret = APS_get_device_info(Board_ID, Fpga_Version, Array_size(IDX_FIRMWARE))
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                        Return ret
                    Else
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    End If
                    ret = APS_get_device_info(Board_ID, Pcb_Version, Array_size(IDX_PCB))
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                        Return ret
                    Else
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    End If
                Catch ex As Exception
                   
                 
                    Write_Log( SubName & ex.Message  )
                    ret = -1
                End Try
                Return ret
            End Function

            ''' <summary>
            ''' 获取轴状态
            ''' </summary>
            ''' <param name="Axis_Id"></param>
            ''' <param name="P_Axis_status"></param>
            ''' <remarks></remarks>
            Function GET_AXIS_STATUS(ByVal Axis_Id As PRA_AXIS_ID_ENUM, ByRef P_Axis_status As AXIS_STATUS, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Integer
                Dim ret As Integer = Nothing
                Try
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    ret = APS_get_command_f(Axis_Id, P_Axis_status.command_position)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                        Return ret
                    End If
                    ret = APS_get_position_f(Axis_Id, P_Axis_status.feedback_position)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                        Return ret
                    End If
                    ret = APS_get_target_position_f(Axis_Id, P_Axis_status.target_position)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                        Return ret
                    End If
                    ret = APS_get_error_position_f(Axis_Id, P_Axis_status.error_position)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                        Return ret
                    End If
                    ret = APS_get_command_velocity_f(Axis_Id, P_Axis_status.command_velocity)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                        Return ret
                    End If
                    ret = APS_get_feedback_velocity_f(Axis_Id, P_Axis_status.feedback_velocity)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                        Return ret
                    End If
                    P_Axis_status.motion_io = APS_motion_io_status(Axis_Id)
                    P_Axis_status.motion_status = APS_motion_status(Axis_Id)
                Catch ex As Exception
                   
                 
                    Write_Log( SubName & ex.Message  )
                    ret = -1
                End Try
                Return ret
            End Function

            ''' <summary>
            ''' 查询轴运动状态
            ''' </summary>
            ''' <param name="Axis_Id"></param>
            ''' <param name="Stop_Code"></param>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Function CHECK_MOTION_DONE(ByVal Axis_Id As PRA_AXIS_ID_ENUM, ByRef Stop_Code As Integer, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Integer
                Dim ret As Integer = Nothing
                Try
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    Dim msts As Integer
                    msts = APS_motion_status(Axis_Id)
                    msts = (msts >> PAR_AMETER_TABLE_ENUM.MTS_NSTP) And 1
                    ret = APS_get_stop_code(Axis_Id, Stop_Code)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                        Return ret
                    Else
                        If msts = 1 Then
                            msts = APS_motion_status(Axis_Id)
                            msts = (msts >> PAR_AMETER_TABLE_ENUM.MTS_ASTP) And 1
                            If msts = 1 Then
                                Dim stop_code_sub As Integer
                                APS_get_stop_code(Axis_Id, stop_code_sub)
                                Return -1
                            Else
                                Return 1
                            End If
                        End If
                    End If
                Catch ex As Exception
                   
                 
                    Write_Log( SubName & ex.Message  )
                    ret = -1
                End Try
                Return 0
            End Function

            Function stop_code_to_string(ByVal Stop_Code As Integer) As String
                Select Case Stop_Code
                    Case 0
                        Return "STOP_NORMAL"
                    Case 1
                        Return "STOP_EMG"
                    Case 2
                        Return "STOP_ALM"
                    Case 3
                        Return "STOP_SVNO"
                    Case 4
                        Return "STOP_PEL"
                    Case 5
                        Return "STOP_MEL"
                    Case 6
                        Return "STOP_SPEL"
                    Case 7
                        Return "STOP_SMEL"
                    Case 8
                        Return "STOP_USER_EMG"
                    Case 9
                        Return "STOP_USER"
                    Case 10
                        Return "STOP_GAN_L1"
                    Case 11
                        Return "STOP_GAN_L2"
                    Case 12
                        Return "STOP_GEAR_SLAVE"
                    Case 13
                        Return "STOP_ERROR_LEVEL"
                    Case 20
                        Return "STOP_ORG_EL"
                    Case 21
                        Return "STOP_NO_ORG"
                    Case 22
                        Return "STOP_NO_EZ"
                    Case 23
                        Return "STOP_BOTH_EL"
                    Case Else
                        Return "Unknown stop code!"
                End Select
            End Function

            Function home_move_ex(ByVal Axis_Id As PRA_AXIS_ID_ENUM, ByVal HOME_MODE As PRA_HOME_MODE_ENUM, ByVal HOME_DIR As PRA_HOME_DIR_ENUM, ByVal HOME_CURVE As PRA_HOME_CURVE_ENUM, ByVal HOME_VM As Integer, ByVal HOME_VO As Integer, ByVal HOME_EZA As PRA_HOME_EZA_ENUM, Optional ByVal HOME_POS As Double = 0, Optional ByVal HOME_ACC As Single = 0.1, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Integer
                Dim ret As Integer
                Try
                    HOME_VM = HOME_VM * axisMagnification(Axis_Id).AXIS_MAGNIFICATION
                    HOME_VO = HOME_VO * axisMagnification(Axis_Id).AXIS_MAGNIFICATION
                    HOME_ACC = HOME_VM / HOME_ACC
                    HOME_POS = HOME_POS * axisMagnification(Axis_Id).AXIS_MAGNIFICATION
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    ret = APS_set_axis_param(Axis_Id, PAR_AMETER_TABLE_ENUM.PRA_HOME_MODE, HOME_MODE)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                        Return ret
                    Else
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    End If
                    ret = APS_set_axis_param(Axis_Id, PAR_AMETER_TABLE_ENUM.PRA_HOME_DIR, HOME_DIR)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                        Return ret
                    Else
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    End If
                    ret = APS_set_axis_param(Axis_Id, PAR_AMETER_TABLE_ENUM.PRA_HOME_CURVE, HOME_CURVE)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                        Return ret
                    Else
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    End If
                    ret = APS_set_axis_param(Axis_Id, PAR_AMETER_TABLE_ENUM.PRA_HOME_ACC, HOME_ACC)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                        Return ret
                    Else
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    End If
                    ret = APS_set_axis_param(Axis_Id, PAR_AMETER_TABLE_ENUM.PRA_HOME_VM, HOME_VM)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                        Return ret
                    Else
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    End If
                    ret = APS_set_axis_param(Axis_Id, PAR_AMETER_TABLE_ENUM.PRA_HOME_VO, HOME_VO)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                        Return ret
                    Else
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    End If
                    ret = APS_set_axis_param(Axis_Id, PAR_AMETER_TABLE_ENUM.PRA_HOME_EZA, HOME_EZA)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                        Return ret
                    Else
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    End If
                    ret = APS_set_axis_param_f(Axis_Id, PAR_AMETER_TABLE_ENUM.PRA_HOME_SHIFT, HOME_POS)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                        Return ret
                    Else
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    End If
                    ret = APS_home_move(Axis_Id)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                        Return ret
                    Else
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    End If
                Catch ex As Exception
                   

                 
                    Write_Log( SubName & ex.Message  )
                    ret = -1
                End Try
                Return ret
            End Function

            Function CONTINUOUS_MOVE(ByVal Axis_Id As PRA_AXIS_ID_ENUM, ByVal JG_DIR As PRA_JOG_DIR_ENUM, ByVal JG_VM As Double, Optional ByVal JG_SF As PRA_SF_ENUM = PRA_SF_ENUM.S_curve, Optional ByVal JG_ACC As Single = 0.1, Optional ByVal JG_DEC As Single = 0.1, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Integer
                Dim ret As Integer = Nothing
                Try
                    JG_VM = JG_VM * axisMagnification(Axis_Id).AXIS_MAGNIFICATION
                    JG_ACC = JG_VM / JG_ACC
                    JG_DEC = JG_VM / JG_DEC
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    ret = APS_set_axis_param(Axis_Id, PAR_AMETER_TABLE_ENUM.PRA_JG_MODE, PRA_JOG_MODE_ENUM.Continuous_mode)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                        Return ret
                    End If
                    ret = APS_set_axis_param(Axis_Id, PAR_AMETER_TABLE_ENUM.PRA_JG_DIR, JG_DIR)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                        Return ret
                    End If
                    ret = APS_set_axis_param_f(Axis_Id, PAR_AMETER_TABLE_ENUM.PRA_JG_SF, JG_SF)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                        Return ret
                    End If
                    ret = APS_set_axis_param_f(Axis_Id, PAR_AMETER_TABLE_ENUM.PRA_JG_ACC, JG_ACC)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                        Return ret
                    End If
                    ret = APS_set_axis_param_f(Axis_Id, PAR_AMETER_TABLE_ENUM.PRA_JG_DEC, JG_DEC)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                        Return ret
                    End If
                    ret = APS_set_axis_param_f(Axis_Id, PAR_AMETER_TABLE_ENUM.PRA_JG_VM, JG_VM)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                        Return ret
                    End If
                    ret = APS_jog_start(Axis_Id, 1)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                        Return ret
                    End If
                Catch ex As Exception
                   
                 
                    Write_Log( SubName & ex.Message  )
                    ret = -1
                End Try
                Return ret
            End Function

            Function JOG_PTP(ByVal AXIS_ID As PRA_AXIS_ID_ENUM, ByVal APS_OPTION As PRA_APS_OPTION_ENUM, ByVal JOG_VM As Double, ByVal POSITION As Double, ByRef Wait As ASYNCALL, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Integer


                Dim ret As Integer = Nothing
                Try
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    ret = APS_ptp_v(AXIS_ID, APS_OPTION, POSITION * axisMagnification(AXIS_ID).AXIS_MAGNIFICATION, JOG_VM * axisMagnification(AXIS_ID).AXIS_MAGNIFICATION, Wait)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    Else
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    End If
                Catch ex As Exception
                   
                 
                    Write_Log( SubName & ex.Message  )
                    ret = -1
                End Try
                Return ret
            End Function

            Function JOG_PTP_ALL(ByVal AXIS_ID As PRA_AXIS_ID_ENUM, ByVal APS_OPTION As PRA_APS_OPTION_ENUM, ByVal JOG_VM As Double, ByVal JOG_ACC As Double, ByVal POSITION As Double, ByRef Wait As ASYNCALL, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Integer

                Dim ret As Integer = Nothing
                Try
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    JOG_VM = JOG_VM * axisMagnification(AXIS_ID).AXIS_MAGNIFICATION
                    JOG_ACC = JOG_VM / JOG_ACC

                    ret = APS_ptp_all(AXIS_ID, APS_OPTION, POSITION * axisMagnification(AXIS_ID).AXIS_MAGNIFICATION, 0, JOG_VM, 0, JOG_ACC, JOG_ACC, 0.5, Wait)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    Else
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    End If
                Catch ex As Exception
                   
                 
                    Write_Log( SubName & ex.Message  )
                    ret = -1
                End Try
                Return ret
            End Function

            Function STEP_MOVE(ByVal AXIS_ID As PRA_AXIS_ID_ENUM, ByVal JOG_DIR As PRA_JOG_DIR_ENUM, ByVal JOG_SF As PRA_SF_ENUM, ByVal JOG_VM As Double, ByVal JOG_OFFSET As Double, ByVal JOG_DELAY As Double, ByVal JOG_ACC As Double, JOG_DEC As Double, Optional ByVal SUBNAME As String = Nothing, Optional ByVal ERRLINE As String = Nothing) As Integer
                Dim ret As Integer = Nothing
                Try
                    SUBNAME = ErrName & GetCurrentMethod.Name & ":"
                    ret = APS_set_axis_param(AXIS_ID, JOG_PARAMETER_ENUM.PRA_JG_MODE, PRA_JOG_MODE_ENUM.Step_mode)
                    If ret <> 0 Then
                        Return -1
                    End If
                    ret = APS_set_axis_param(AXIS_ID, JOG_PARAMETER_ENUM.PRA_JG_DIR, JOG_DIR)
                    If ret <> 0 Then
                        Return -1
                    End If
                    ret = APS_set_axis_param(AXIS_ID, JOG_PARAMETER_ENUM.PRA_JG_SF, JOG_SF)
                    If ret <> 0 Then
                        Return -1
                    End If
                    ret = APS_set_axis_param(AXIS_ID, JOG_PARAMETER_ENUM.PRA_JG_ACC, JOG_VM / JOG_ACC)
                    If ret <> 0 Then
                        Return -1
                    End If
                    ret = APS_set_axis_param(AXIS_ID, JOG_PARAMETER_ENUM.PRA_JG_DEC, JOG_VM / JOG_DEC)
                    If ret <> 0 Then
                        Return -1
                    End If
                    ret = APS_set_axis_param(AXIS_ID, JOG_PARAMETER_ENUM.PRA_JG_VM, JOG_VM)
                    If ret <> 0 Then
                        Return -1
                    End If
                    ret = APS_set_axis_param(AXIS_ID, JOG_PARAMETER_ENUM.PRA_JG_OFFSET, JOG_OFFSET)
                    If ret <> 0 Then
                        Return -1
                    End If
                    ret = APS_set_axis_param(AXIS_ID, JOG_PARAMETER_ENUM.PRA_JG_DELAY, JOG_DELAY)
                    If ret <> 0 Then
                        Return -1
                    End If
                    ret = APS_jog_start(AXIS_ID, JOG_SWITCH_ENUM.JOG_ON)
                    If ret <> 0 Then
                        Return -1
                    End If
                    ret = 0
                Catch ex As Exception
                   
                    Write_Log(SUBNAME & ex.Message)
                    ret = -1
                End Try
                Return ret
            End Function

            Function JOG_STOP_MOVE(ByVal Axis_Id As PRA_AXIS_ID_ENUM, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Integer
                Dim ret As Integer = Nothing
                Try
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    ret = APS_jog_start(Axis_Id, JOG_SWITCH_ENUM.JOG_OFF)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                        Return ret
                    End If
                Catch ex As Exception
                   
                 
                    Write_Log( SubName & ex.Message  )
                    ret = -1
                End Try
                Return ret

            End Function


            Function deceleration_stop(ByVal Axis_Id As PRA_AXIS_ID_ENUM, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Integer
                Dim ret As Integer = Nothing
                Try
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    ret = APS_set_axis_param_f(Axis_Id, PAR_AMETER_TABLE_ENUM.PRA_STP_DEC, 10000)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                        Return ret
                    End If
                    ret = APS_stop_move(Axis_Id)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                        Return ret
                    End If
                Catch ex As Exception
                   
                 
                    Write_Log( SubName & ex.Message  )
                    ret = -1
                End Try
                Return ret
            End Function

            Function EMG_STOP(ByVal Axis_Id As PRA_AXIS_ID_ENUM, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Integer
                Dim ret As Integer = Nothing
                Try
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    ret = APS_emg_stop(Axis_Id)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    End If
                Catch ex As Exception
                   
                 
                    Write_Log( SubName & ex.Message  )
                    ret = -1
                End Try
                Return ret
            End Function

            Function INTERPOLATION_LINE(ByVal Axis_Id_Array() As Integer, ByVal Axis_Pos_Array() As Double, ByVal APS_OPTION As PRA_APS_OPTION_ENUM, ByVal VM As Integer, Optional ByVal ACC As Single = 0.1, Optional ByVal DEC As Single = 0.1, Optional ByVal SF As PRA_SF_ENUM = PRA_SF_ENUM.S_curve, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Integer
                Dim ret As Integer = Nothing
                Dim PositionArray() As Double = Nothing
                Array.Resize(PositionArray, Axis_Pos_Array.Length)
                Try
                    If Axis_Id_Array.Length = Axis_Pos_Array.Length Then
                        For i As Integer = 0 To Axis_Pos_Array.Length - 1
                            PositionArray(i) = Axis_Pos_Array(i) * axisMagnification(Axis_Id_Array(i)).AXIS_MAGNIFICATION
                        Next
                        SubName = ErrName & GetCurrentMethod.Name & ":"
                        Dim TransPara As Double = 0
                        VM = VM * axisMagnification(Axis_Id_Array(0)).AXIS_MAGNIFICATION
                        ACC = VM / ACC
                        DEC = VM / DEC
                        'ret = APS_set_axis_param_f(Axis_Id_Array(0), PAR_AMETER_TABLE_ENUM.PRA_SF, SF)
                        'If ret <> 0 Then
                        '    SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                        '    Return ret
                        'End If
                        'ret = APS_set_axis_param_f(Axis_Id_Array(0), PAR_AMETER_TABLE_ENUM.PRA_ACC, ACC)
                        'If ret <> 0 Then
                        '    SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                        '    Return ret
                        'End If
                        'ret = APS_set_axis_param_f(Axis_Id_Array(0), PAR_AMETER_TABLE_ENUM.PRA_DEC, DEC)
                        'If ret <> 0 Then
                        '    SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                        '    Return ret
                        'End If
                        'ret = APS_set_axis_param_f(Axis_Id_Array(0), PAR_AMETER_TABLE_ENUM.PRA_VM, VM)
                        'If ret <> 0 Then
                        '    SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                        '    Return ret
                        'End If
                        'ret = APS_line(Axis_Id_Array.Length, Axis_Id_Array, APS_OPTION, PositionArray, TransPara, Nothing)
                        ret = APS_line_all(Axis_Id_Array.Length, Axis_Id_Array, APS_OPTION, PositionArray, TransPara, 0, VM, 0, ACC, DEC, SF, Nothing)
                        If ret <> 0 Then
                            SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                            Return ret
                        End If
                    Else
                        ret = -1
                    End If
                Catch ex As Exception
                   
                 
                    Write_Log( SubName & ex.Message  )
                    ret = -1
                End Try
                Return ret
            End Function

            Function INTERPOLATION_LINE_ALL(ByVal Axis_Id_Array() As Integer, ByVal PositionArray() As Double, ByVal APS_OPTION As PRA_APS_OPTION_ENUM, ByVal VS As Double, ByVal VM As Integer, ByVal VE As Double, Optional ByVal ACC As Single = 0.1, Optional ByVal DEC As Single = 0.1, Optional ByVal SF As PRA_SF_ENUM = PRA_SF_ENUM.S_curve, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Integer
                Dim ret As Integer = Nothing
                Try
                    If Axis_Id_Array.Length = PositionArray.Length Then
                        For i As Integer = 0 To PositionArray.Length - 1
                            PositionArray(i) = PositionArray(i) * axisMagnification(Axis_Id_Array(i)).AXIS_MAGNIFICATION
                        Next
                        SubName = ErrName & GetCurrentMethod.Name & ":"
                        Dim TransPara As Double = 0
                        VS = VS * axisMagnification(0).AXIS_MAGNIFICATION
                        VM = VM * axisMagnification(0).AXIS_MAGNIFICATION
                        VE = VE * axisMagnification(0).AXIS_MAGNIFICATION
                        ACC = VM / ACC
                        DEC = VM / ACC
                        ret = APS_line_all(Axis_Id_Array.Length, Axis_Id_Array, PRA_APS_OPTION_ENUM.Absolute, PositionArray, TransPara, VS, VM, VE, ACC, DEC, SF, Nothing)
                        If ret <> 0 Then
                            SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                            Return ret
                        End If
                    Else
                        ret = -1
                    End If
                Catch ex As Exception
                   
                 
                    Write_Log( SubName & ex.Message  )
                    ret = -1
                End Try
                Return ret
            End Function

            Function HELICAL(ByVal Axis_Id_Array() As Integer, ByVal CenterArray() As Double, ByVal APS_Option As PRA_APS_OPTION_ENUM, ByVal R2_Angle As Double, ByVal Direction As PAR_APS_DIR_ENUM, ByVal Vm As Double, Optional ByVal Sf As PRA_SF_ENUM = PRA_SF_ENUM.S_curve, Optional ByVal Acc As Single = 0.1, Optional ByVal Dec As Single = 0.1, Optional ByVal Ro As Double = 0, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Boolean
                Dim ret As Integer = Nothing
                Try
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    Dim NormalArray(2) As Double
                    NormalArray(0) = 0
                    NormalArray(1) = 0
                    NormalArray(2) = 1
                    Dim EndPosition(2) As Double
                    EndPosition(0) = 0 * axisMagnification(Axis_Id_Array(0)).AXIS_MAGNIFICATION
                    EndPosition(1) = 0 * axisMagnification(Axis_Id_Array(1)).AXIS_MAGNIFICATION
                    EndPosition(2) = R2_Angle * axisMagnification(Axis_Id_Array(2)).AXIS_MAGNIFICATION
                    ret = APS_set_axis_param_f(Axis_Id_Array(0), PAR_AMETER_TABLE_ENUM.PRA_SF, Sf)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                        Return ret
                    End If
                    ret = APS_set_axis_param_f(Axis_Id_Array(0), PAR_AMETER_TABLE_ENUM.PRA_ACC, Vm / Acc)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                        Return ret
                    End If
                    ret = APS_set_axis_param_f(Axis_Id_Array(0), PAR_AMETER_TABLE_ENUM.PRA_DEC, Vm / Dec)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                        Return ret
                    End If
                    ret = APS_set_axis_param_f(Axis_Id_Array(0), PAR_AMETER_TABLE_ENUM.PRA_VM, Vm)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                        Return ret
                    End If
                    ret = APS_spiral_ce(Axis_Id_Array, APS_Option, CenterArray, NormalArray, EndPosition, Direction, 0, Nothing)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                        Return ret
                    End If
                Catch ex As Exception
                   
                 
                    Write_Log( SubName & ex.Message  )
                    ret = -1
                End Try
                Return ret
            End Function

            ''' <summary>
            ''' 读取输入点状态
            ''' </summary>
            ''' <param name="Board_ID">控制卡ID</param>
            ''' <param name="DI_ID">输入点ID</param>
            ''' <param name="SubName"></param>
            ''' <param name="ErrLine"></param>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Function GET_DI_BIT(ByVal Board_ID As Integer, ByVal DI_ID As Integer, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Boolean
                Dim ret As Integer = Nothing
                Dim RetBoolean As Boolean = False
                Try
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    Dim digital_input_value As Integer = 0
                    Dim di_ch(MAX_DI_CH) As Integer
                    Dim i As Integer
                    ret = APS_read_d_input(Board_ID, 0, digital_input_value)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    Else
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                        For i = 0 To (MAX_DI_CH - 1)
                            di_ch(i) = ((digital_input_value >> i) And 1)
                        Next
                        If di_ch(DI_ID) = 1 Then
                            RetBoolean = True
                        Else
                            RetBoolean = False
                        End If
                    End If
                Catch ex As Exception
                   
                 
                    Write_Log( SubName & ex.Message  )
                    ret = -1
                End Try
                Return RetBoolean
            End Function

            ''' <summary>
            ''' 输出指定端口IO
            ''' </summary>
            ''' <param name="Board_ID">卡号</param>
            ''' <param name="DO_ID">输出ID</param>
            ''' <param name="DO_ON_OFF">输出类型ON OR OFF</param>
            ''' <param name="SubName"></param>
            ''' <param name="ErrLine"></param>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Function SET_DO_BIT(ByVal Board_ID As Integer, ByVal DO_ID As PRA_DO_ENUM, ByVal DO_ON_OFF As PRA_OUT_STATUS_ENUM, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Integer
                Dim ret As Integer = 0
                Try
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    Dim digital_output_value As Integer = 0
                    Dim do_ch(MAX_DO_CH) As Integer
                    Dim i As Integer
                    ret = APS_read_d_output(Board_ID, 0, digital_output_value)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    Else
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                        For i = 0 To (MAX_DO_CH - 1)
                            do_ch(i) = ((digital_output_value >> i) And 1)
                        Next
                        do_ch(DO_ID) = DO_ON_OFF
                        digital_output_value = 0
                        For i = 0 To (MAX_DO_CH - 1)
                            digital_output_value = (digital_output_value) Or (do_ch(i) << i)
                        Next
                        ret = APS_write_d_output(Board_ID, 0, digital_output_value)
                        If ret <> 0 Then
                            SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                        Else
                            SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                        End If
                    End If
                Catch ex As Exception
                   
                 
                    Write_Log( SubName & ex.Message  )
                    ret = -1
                End Try
                Return ret
            End Function

            ''' <summary>
            ''' 关闭所有输出
            ''' </summary>
            ''' <param name="Board_ID">控制卡ID</param>
            ''' <param name="SubName"></param>
            ''' <param name="ErrLine"></param>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Function SET_DO_ALL_OFF(ByVal Board_ID As Integer, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Integer
                Dim ret As Integer = Nothing
                Try
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    ret = APS_write_d_output(Board_ID, 0, 0)
                    If ret <> 0 Then
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    Else
                        SubName = SubName & ErrCode & stop_code_to_string(ret) & "]"
                    End If
                Catch ex As Exception
                   
                 
                    Write_Log( SubName & ex.Message  )
                    ret = -1
                End Try
                Return ret
            End Function

            Function GET_EMG_STATUS(ByRef Axis_Id As PRA_AXIS_ID_ENUM, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Boolean
                Dim ret As Boolean = False
                Dim motion_io As Integer = Nothing
                Try
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    If ((APS_motion_io_status(Axis_Id) >> MOTION_STATUS_ENUM.MIO_EMG) And 1) Then
                        ret = True
                    Else
                        ret = False
                    End If
                Catch ex As Exception
                   
                 
                    Write_Log( SubName & ex.Message  )
                    ret = False
                End Try
                Return ret
            End Function

            Function GET_ALM_STATUS(ByRef Axis_Id As PRA_AXIS_ID_ENUM, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Boolean
                Dim ret As Boolean = False
                Dim motion_io As Integer = Nothing
                Try
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    If ((APS_motion_io_status(Axis_Id) >> MOTION_STATUS_ENUM.MIO_ALM) And 1) Then
                        ret = True
                    Else
                        ret = False
                    End If
                Catch ex As Exception
                    Write_Log(SubName & ex.Message)
                    ret = False
                End Try
                Return ret
            End Function

            Function GET_INP_STATUS(ByRef Axis_Id As PRA_AXIS_ID_ENUM, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Boolean
                Dim ret As Boolean = False
                Dim motion_io As Integer = Nothing
                Try
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    If ((APS_motion_io_status(Axis_Id) >> MOTION_STATUS_ENUM.MIO_INP) And 1) Then
                        ret = True
                    Else
                        ret = False
                    End If
                Catch ex As Exception
                   
                 
                    Write_Log( SubName & ex.Message  )
                    ret = False
                End Try
                Return ret
            End Function

            Function GET_MEL_STATUS(ByRef Axis_Id As PRA_AXIS_ID_ENUM, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Boolean
                Dim ret As Boolean = False
                Dim motion_io As Integer = Nothing
                Try
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    If ((APS_motion_io_status(Axis_Id) >> MOTION_STATUS_ENUM.MIO_MEL) And 1) Then
                        ret = True
                    Else
                        ret = False
                    End If
                Catch ex As Exception
                   
                 
                    Write_Log( SubName & ex.Message  )
                    ret = False
                End Try
                Return ret
            End Function

            Function GET_PEL_STATUS(ByRef Axis_Id As PRA_AXIS_ID_ENUM, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Boolean
                Dim ret As Boolean = False
                Dim motion_io As Integer = Nothing
                Try
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    If ((APS_motion_io_status(Axis_Id) >> MOTION_STATUS_ENUM.MIO_PEL) And 1) Then
                        ret = True
                    Else
                        ret = False
                    End If
                Catch ex As Exception
                   
                 
                    Write_Log( SubName & ex.Message  )
                    ret = False
                End Try
                Return ret
            End Function

            Function GET_RDY_STATUS(ByRef Axis_Id As PRA_AXIS_ID_ENUM, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Boolean
                Dim ret As Boolean = False
                Dim motion_io As Integer = Nothing
                Try
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    If ((APS_motion_io_status(Axis_Id) >> MOTION_STATUS_ENUM.MIO_RDY) And 1) Then
                        ret = True
                    Else
                        ret = False
                    End If
                Catch ex As Exception
                   
                 
                    Write_Log( SubName & ex.Message  )
                    ret = False
                End Try
                Return ret
            End Function

            Function GET_ORG_STATUS(ByRef Axis_Id As PRA_AXIS_ID_ENUM, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Boolean
                Dim ret As Boolean = False
                Dim motion_io As Integer = Nothing
                Try
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    If ((APS_motion_io_status(Axis_Id) >> MOTION_STATUS_ENUM.MIO_ORG) And 1) Then
                        ret = True
                    Else
                        ret = False
                    End If
                Catch ex As Exception
                   
                 
                    Write_Log( SubName & ex.Message  )
                    ret = False
                End Try
                Return ret
            End Function

            Function GET_SVON_STATUS(ByRef Axis_Id As PRA_AXIS_ID_ENUM, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Boolean
                Dim ret As Boolean = False
                Dim motion_io As Integer = Nothing
                Try
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    If ((APS_motion_io_status(Axis_Id) >> MOTION_STATUS_ENUM.MIO_SVON) And 1) Then
                        ret = True
                    Else
                        ret = False
                    End If
                Catch ex As Exception
                   
                 
                    Write_Log( SubName & ex.Message  )
                    ret = False
                End Try
                Return ret
            End Function

            Public Structure TrgParam_STR
                ''' <summary>
                ''' 设置通道0比较来源
                ''' </summary>
                ''' <remarks></remarks>
                Dim CH0_COMPARE_ENCODER As TRIGGER_COMPARE_SOURCE_ENUM
                ''' <summary>
                ''' 设置通道0比较方向
                ''' </summary>
                ''' <remarks></remarks>
                Dim CH0_COMPARE_DIRECTION As TABLE_COMPARE_DIRECTION_ENUM
                ''' <summary>
                ''' 设置通道0 ON/OFF
                ''' </summary>
                ''' <remarks></remarks>
                Dim CH0_TRIGGER As TRIGGER_SOURCH_STATUS_ENUM
                ''' <summary>
                ''' 比较输出模式
                ''' </summary>
                ''' <remarks></remarks>
                Dim CH0_TRIGGER_OUTPUT_MODE As TRIGGER_OUTPUT_MODE_ENUM
                ''' <summary>
                ''' 输出电平逻辑
                ''' </summary>
                ''' <remarks></remarks>
                Dim CH0_TRIGGER_LOGIC As TRIGGER_LOGIC_ENUM
                ''' <summary>
                ''' 输出脉冲宽度 *20NS
                ''' </summary>
                ''' <remarks></remarks>
                Dim CH0_TRIGGER_PULSE_WIDTH As Double
                ''' <summary>
                ''' 手动触发开关
                ''' </summary>
                ''' <remarks></remarks>
                Dim CH0_MANUAL_TRIGGER As TRIGGER_SOURCH_ENUM
                ''' <summary>
                ''' 点表触发通道0开关
                ''' </summary>
                ''' <remarks></remarks>
                Dim CH0_TABLE_CMP0 As TRIGGER_SOURCH_ENUM
                ''' <summary>
                ''' 点表触发通道1开关
                ''' </summary>
                ''' <remarks></remarks>
                Dim CH0_TABLE_CMP1 As TRIGGER_SOURCH_ENUM
                ''' <summary>
                ''' 线性触发通道0开关
                ''' </summary>
                ''' <remarks></remarks>
                Dim CH0_LINEAR_CMP0 As TRIGGER_SOURCH_ENUM
                ''' <summary>
                ''' 线性触发通道1开关
                ''' </summary>
                ''' <remarks></remarks>
                Dim CH0_LINEAR_CMP1 As TRIGGER_SOURCH_ENUM
                ''' <summary>
                ''' 设置通道1比较来源
                ''' </summary>
                ''' <remarks></remarks>
                Dim CH1_COMPARE_ENCODER As TRIGGER_COMPARE_SOURCE_ENUM
                ''' <summary>
                ''' 设置通道1比较方向
                ''' </summary>
                ''' <remarks></remarks>
                Dim CH1_COMPARE_DIRECTION As TABLE_COMPARE_DIRECTION_ENUM
                ''' <summary>
                ''' 设置通道1 ON/OFF
                ''' </summary>
                ''' <remarks></remarks>
                Dim CH1_TRIGGER As TRIGGER_SOURCH_STATUS_ENUM
                ''' <summary>
                ''' 比较输出模式
                ''' </summary>
                ''' <remarks></remarks>
                Dim CH1_TRIGGER_OUTPUT_MODE As TRIGGER_OUTPUT_MODE_ENUM
                ''' <summary>
                ''' 输出电平逻辑
                ''' </summary>
                ''' <remarks></remarks>
                Dim CH1_TRIGGER_LOGIC As TRIGGER_LOGIC_ENUM
                ''' <summary>
                ''' 输出脉冲宽度 *20NS
                ''' </summary>
                ''' <remarks></remarks>
                Dim CH1_TRIGGER_PULSE_WIDTH As Double
                ''' <summary>
                ''' 手动触发开关
                ''' </summary>
                ''' <remarks></remarks>
                Dim CH1_MANUAL_TRIGGER As TRIGGER_SOURCH_ENUM
                ''' <summary>
                ''' 点表触发通道0开关
                ''' </summary>
                ''' <remarks></remarks>
                Dim CH1_TABLE_CMP0 As TRIGGER_SOURCH_ENUM
                ''' <summary>
                ''' 点表触发通道1开关
                ''' </summary>
                ''' <remarks></remarks>
                Dim CH1_TABLE_CMP1 As TRIGGER_SOURCH_ENUM
                ''' <summary>
                ''' 线性触发通道0开关
                ''' </summary>
                ''' <remarks></remarks>
                Dim CH1_LINEAR_CMP0 As TRIGGER_SOURCH_ENUM
                ''' <summary>
                ''' 线性触发通道1开关
                ''' </summary>
                ''' <remarks></remarks>
                Dim CH1_LINEAR_CMP1 As TRIGGER_SOURCH_ENUM
            End Structure

            Function SET_TRIGGER_EX(ByVal BOARD_ID As Integer, ByVal TrgParam As TrgParam_STR) As Integer
                Dim ret As Integer = Nothing
                Dim TRIGGER_CH As String = Nothing
                Dim TRIGGER_SOURCH As String = Nothing
                Try
                    TRIGGER_CH = TrgParam.CH1_TRIGGER & TrgParam.CH0_TRIGGER
                    ret = set_trigger_param(BOARD_ID, ADLINK_LIB.MOTION.PAR_AMETER_TABLE_ENUM.TGR_TRG_EN, DATA_CONVER.BIN_TO_DEC(TRIGGER_CH))
                    If ret <> 0 Then
                        Return ret
                    End If
                    If TrgParam.CH0_TRIGGER = TRIGGER_SOURCH_STATUS_ENUM.ENABLE Then
                        If TrgParam.CH0_COMPARE_ENCODER <> TRIGGER_COMPARE_SOURCE_ENUM.DISABLE Then
                            TRIGGER_SOURCH = TrgParam.CH0_LINEAR_CMP1 & TrgParam.CH0_LINEAR_CMP0 & TrgParam.CH0_TABLE_CMP1 & TrgParam.CH0_TABLE_CMP0 & "0" & TrgParam.CH0_MANUAL_TRIGGER
                            ret = set_trigger_param(BOARD_ID, ADLINK_LIB.MOTION.PAR_AMETER_TABLE_ENUM.TGR_TRG0_SRC, DATA_CONVER.BIN_TO_DEC(TRIGGER_SOURCH))
                            If ret <> 0 Then
                                Return ret
                            End If
                            ret = APS_set_trigger_param(BOARD_ID, ADLINK_LIB.MOTION.PAR_AMETER_TABLE_ENUM.TGR_TCMP0_SRC, TrgParam.CH0_COMPARE_ENCODER)
                            If ret <> 0 Then
                                Return ret
                            End If
                            ret = APS_set_trigger_param(BOARD_ID, ADLINK_LIB.MOTION.PAR_AMETER_TABLE_ENUM.TGR_TCMP0_DIR, TrgParam.CH0_COMPARE_DIRECTION)
                            If ret <> 0 Then
                                Return ret
                            End If
                            ret = APS_set_trigger_param(BOARD_ID, ADLINK_LIB.MOTION.PAR_AMETER_TABLE_ENUM.TGR_TRG0_PWD, TrgParam.CH0_TRIGGER_PULSE_WIDTH)
                            If ret <> 0 Then
                                Return ret
                            End If
                            ret = APS_set_trigger_param(BOARD_ID, ADLINK_LIB.MOTION.PAR_AMETER_TABLE_ENUM.TGR_TRG0_LOGIC, TrgParam.CH0_TRIGGER_LOGIC)
                            If ret <> 0 Then
                                Return ret
                            End If
                            ret = APS_set_trigger_param(BOARD_ID, ADLINK_LIB.MOTION.PAR_AMETER_TABLE_ENUM.TGR_TRG0_TGL, TrgParam.CH0_TRIGGER_OUTPUT_MODE)
                            If ret <> 0 Then
                                Return ret
                            End If
                        End If
                    End If
                    If TrgParam.CH1_TRIGGER = TRIGGER_SOURCH_STATUS_ENUM.ENABLE Then
                        If TrgParam.CH1_COMPARE_ENCODER <> TRIGGER_COMPARE_SOURCE_ENUM.DISABLE Then
                            TRIGGER_SOURCH = TrgParam.CH1_LINEAR_CMP1 & TrgParam.CH1_LINEAR_CMP0 & TrgParam.CH1_TABLE_CMP1 & TrgParam.CH1_TABLE_CMP0 & "0" & TrgParam.CH1_MANUAL_TRIGGER
                            ret = set_trigger_param(BOARD_ID, ADLINK_LIB.MOTION.PAR_AMETER_TABLE_ENUM.TGR_TRG1_SRC, DATA_CONVER.BIN_TO_DEC(TRIGGER_SOURCH))
                            If ret <> 0 Then
                                Return ret
                            End If
                            ret = APS_set_trigger_param(BOARD_ID, ADLINK_LIB.MOTION.PAR_AMETER_TABLE_ENUM.TGR_TCMP1_SRC, TrgParam.CH1_COMPARE_ENCODER)
                            If ret <> 0 Then
                                Return ret
                            End If
                            ret = APS_set_trigger_param(BOARD_ID, ADLINK_LIB.MOTION.PAR_AMETER_TABLE_ENUM.TGR_TCMP1_DIR, TrgParam.CH1_COMPARE_DIRECTION)
                            If ret <> 0 Then
                                Return ret
                            End If
                            ret = APS_set_trigger_param(BOARD_ID, ADLINK_LIB.MOTION.PAR_AMETER_TABLE_ENUM.TGR_TRG1_PWD, TrgParam.CH1_TRIGGER_PULSE_WIDTH)
                            If ret <> 0 Then
                                Return ret
                            End If
                            ret = APS_set_trigger_param(BOARD_ID, ADLINK_LIB.MOTION.PAR_AMETER_TABLE_ENUM.TGR_TRG1_LOGIC, TrgParam.CH1_TRIGGER_LOGIC)
                            If ret <> 0 Then
                                Return ret
                            End If
                            ret = APS_set_trigger_param(BOARD_ID, ADLINK_LIB.MOTION.PAR_AMETER_TABLE_ENUM.TGR_TRG1_TGL, TrgParam.CH1_TRIGGER_OUTPUT_MODE)
                            If ret <> 0 Then
                                Return ret
                            End If
                        End If
                    End If
                Catch ex As Exception
                    ret = -1
                End Try
                Return ret
            End Function


            Function RST_TRIGGER_EX(ByVal BOARD_ID As Integer, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Integer
                Dim ret As Integer = Nothing
                Dim TRIGGER_CH As String = Nothing
                Dim TRIGGER_SOURCH As String = Nothing
                Try
                    ret = set_trigger_param(BOARD_ID, ADLINK_LIB.MOTION.PAR_AMETER_TABLE_ENUM.TGR_TRG_EN, DATA_CONVER.BIN_TO_DEC("00"))
                    If ret <> 0 Then
                        Return ret
                    End If
                    ret = set_trigger_param(BOARD_ID, ADLINK_LIB.MOTION.PAR_AMETER_TABLE_ENUM.TGR_TRG0_SRC, DATA_CONVER.BIN_TO_DEC("00000"))
                    If ret <> 0 Then
                        Return ret
                    End If
                    ret = APS_set_trigger_param(BOARD_ID, ADLINK_LIB.MOTION.PAR_AMETER_TABLE_ENUM.TGR_TCMP0_SRC, TRIGGER_COMPARE_SOURCE_ENUM.DISABLE)
                    If ret <> 0 Then
                        Return ret
                    End If
                    ret = APS_set_trigger_param(BOARD_ID, ADLINK_LIB.MOTION.PAR_AMETER_TABLE_ENUM.TGR_TCMP0_DIR, TABLE_COMPARE_DIRECTION_ENUM.POSITIVE_DIRECTION)
                    If ret <> 0 Then
                        Return ret
                    End If
                    ret = APS_set_trigger_param(BOARD_ID, ADLINK_LIB.MOTION.PAR_AMETER_TABLE_ENUM.TGR_TRG0_PWD, 10)
                    If ret <> 0 Then
                        Return ret
                    End If
                    ret = APS_set_trigger_param(BOARD_ID, ADLINK_LIB.MOTION.PAR_AMETER_TABLE_ENUM.TGR_TRG0_LOGIC, TRIGGER_LOGIC_ENUM.NOT_INVERSE)
                    If ret <> 0 Then
                        Return ret
                    End If
                    ret = APS_set_trigger_param(BOARD_ID, ADLINK_LIB.MOTION.PAR_AMETER_TABLE_ENUM.TGR_TRG0_TGL, TRIGGER_OUTPUT_MODE_ENUM.PULSE_OUT)
                    If ret <> 0 Then
                        Return ret
                    End If
                    ret = set_trigger_param(BOARD_ID, ADLINK_LIB.MOTION.PAR_AMETER_TABLE_ENUM.TGR_TRG1_SRC, DATA_CONVER.BIN_TO_DEC("00000"))
                    If ret <> 0 Then
                        Return ret
                    End If
                    ret = APS_set_trigger_param(BOARD_ID, ADLINK_LIB.MOTION.PAR_AMETER_TABLE_ENUM.TGR_TCMP1_SRC, TRIGGER_COMPARE_SOURCE_ENUM.DISABLE)
                    If ret <> 0 Then
                        Return ret
                    End If
                    ret = APS_set_trigger_param(BOARD_ID, ADLINK_LIB.MOTION.PAR_AMETER_TABLE_ENUM.TGR_TCMP1_DIR, TABLE_COMPARE_DIRECTION_ENUM.POSITIVE_DIRECTION)
                    If ret <> 0 Then
                        Return ret
                    End If
                    ret = APS_set_trigger_param(BOARD_ID, ADLINK_LIB.MOTION.PAR_AMETER_TABLE_ENUM.TGR_TRG1_PWD, 10)
                    If ret <> 0 Then
                        Return ret
                    End If
                    ret = APS_set_trigger_param(BOARD_ID, ADLINK_LIB.MOTION.PAR_AMETER_TABLE_ENUM.TGR_TRG1_LOGIC, TRIGGER_LOGIC_ENUM.NOT_INVERSE)
                    If ret <> 0 Then
                        Return ret
                    End If
                    ret = APS_set_trigger_param(BOARD_ID, ADLINK_LIB.MOTION.PAR_AMETER_TABLE_ENUM.TGR_TRG1_TGL, TRIGGER_OUTPUT_MODE_ENUM.PULSE_OUT)
                    If ret <> 0 Then
                        Return ret
                    End If
                    ret = APS_reset_trigger_count(BOARD_ID, 0)
                    If ret <> 0 Then
                        Return ret
                    End If
                    ret = APS_reset_trigger_count(BOARD_ID, 1)
                Catch ex As Exception
                   
                 
                    Write_Log( SubName & ex.Message  )
                    ret = -1
                End Try
                Return ret
            End Function

            '  System & Initialization
            Declare Function APS_initial Lib "APS168x64.dll" (ByRef BoardID_InBits As Integer, ByVal Mode As Integer) As Integer
            Declare Function APS_close Lib "APS168x64.dll" () As Integer
            Declare Function APS_version Lib "APS168x64.dll" () As Integer
            Declare Function APS_device_driver_version Lib "APS168x64.dll" (ByVal Board_ID As Integer) As Integer
            Declare Function APS_get_axis_info Lib "APS168x64.dll" (ByVal Axis_Id As PRA_AXIS_ID_ENUM, ByRef Board_ID As Integer, ByRef Axis_No As Integer, ByRef Port_ID As Integer, ByRef Module_ID As Integer) As Integer
            Declare Function APS_set_board_param Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal BOD_Param_No As Integer, ByVal BOD_Param As Integer) As Integer
            Declare Function APS_get_board_param Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal BOD_Param_No As Integer, ByRef BOD_Param As Integer) As Integer
            Declare Function APS_set_axis_param Lib "APS168x64.dll" (ByVal Axis_Id As PRA_AXIS_ID_ENUM, ByVal AXS_Param_No As JOG_PARAMETER_ENUM, ByVal AXS_Param As Integer) As Integer
            Declare Function APS_get_axis_param Lib "APS168x64.dll" (ByVal Axis_Id As PRA_AXIS_ID_ENUM, ByVal AXS_Param_No As Integer, ByRef AXS_Param As Integer) As Integer
            Declare Function APS_get_system_timer Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByRef SysTimer As Integer) As Integer
            Declare Function APS_get_device_info Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal Info_No As Integer, ByRef Info As Integer) As Integer
            Declare Function APS_get_card_name Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByRef CardName As Integer) As Integer
            Declare Function APS_disable_device Lib "APS168x64.dll" (ByVal DeviceName As Integer) As Integer
            Declare Function APS_get_first_axisId Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByRef StartAxis_Id As Integer, ByRef TotalAxisNum As Integer) As Integer
            Declare Function APS_set_security_key Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal OldPassword As Integer, ByVal NewPassword As Integer) As Integer
            Declare Function APS_check_security_key Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal Password As Integer) As Integer
            Declare Function APS_reset_security_key Lib "APS168x64.dll" (ByVal Board_ID As Integer) As Integer
            Declare Function APS_load_param_from_file Lib "APS168x64.dll" (ByVal pXMLFile As String) As Integer
            Declare Function APS_get_curr_sys_ctrl_mode Lib "APS168x64.dll" (ByVal Axis_Id As PRA_AXIS_ID_ENUM, ByRef Mode As Integer) As Integer
            Declare Function APS_set_pwm_width Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal PWM_Ch As Integer, ByVal Width As Integer) As Integer
            Declare Function APS_get_pwm_width Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal PWM_Ch As Integer, ByRef Width As Integer) As Integer
            Declare Function APS_set_pwm_frequency Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal PWM_Ch As Integer, ByVal Frequency As Integer) As Integer
            Declare Function APS_get_pwm_frequency Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal PWM_Ch As Integer, ByRef Frequency As Integer) As Integer
            Declare Function APS_set_pwm_on Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal PWM_Ch As Integer, ByVal PWM_On As Integer) As Integer
            Declare Function APS_get_command Lib "APS168x64.dll" (ByVal Axis_Id As PRA_AXIS_ID_ENUM, ByRef CommandCnt As Integer) As Integer
            Declare Function APS_set_command Lib "APS168x64.dll" (ByVal Axis_Id As PRA_AXIS_ID_ENUM, ByVal CommandCnt As Integer) As Integer
            Declare Function APS_motion_status Lib "APS168x64.dll" (ByVal Axis_Id As PRA_AXIS_ID_ENUM) As Integer
            Declare Function APS_motion_io_status Lib "APS168x64.dll" (ByVal Axis_Id As PRA_AXIS_ID_ENUM) As Integer
            Declare Function APS_set_servo_on Lib "APS168x64.dll" (ByVal Axis_Id As PRA_AXIS_ID_ENUM, ByVal Servo_On As PRA_OUT_STATUS_ENUM) As Integer
            Declare Function APS_get_position Lib "APS168x64.dll" (ByVal Axis_Id As PRA_AXIS_ID_ENUM, ByRef Position As Integer) As Integer
            Declare Function APS_set_position Lib "APS168x64.dll" (ByVal Axis_Id As PRA_AXIS_ID_ENUM, ByVal Position As Integer) As Integer
            Declare Function APS_get_command_velocity Lib "APS168x64.dll" (ByVal Axis_Id As PRA_AXIS_ID_ENUM, ByRef Velocity As Integer) As Integer
            Declare Function APS_get_feedback_velocity Lib "APS168x64.dll" (ByVal Axis_Id As PRA_AXIS_ID_ENUM, ByRef Velocity As Integer) As Integer
            Declare Function APS_get_error_position Lib "APS168x64.dll" (ByVal Axis_Id As PRA_AXIS_ID_ENUM, ByRef Err_Pos As Integer) As Integer
            Declare Function APS_get_target_position Lib "APS168x64.dll" (ByVal Axis_Id As PRA_AXIS_ID_ENUM, ByRef Targ_Pos As Integer) As Integer
            Declare Function APS_relative_move Lib "APS168x64.dll" (ByVal Axis_Id As PRA_AXIS_ID_ENUM, ByVal Distance As Integer, ByVal Max_Speed As Integer) As Integer
            Declare Function APS_absolute_move Lib "APS168x64.dll" (ByVal Axis_Id As PRA_AXIS_ID_ENUM, ByVal Position As Integer, ByVal Max_Speed As Integer) As Integer
            Declare Function APS_velocity_move Lib "APS168x64.dll" (ByVal Axis_Id As PRA_AXIS_ID_ENUM, ByVal Max_Speed As Integer) As Integer
            Declare Function APS_home_move Lib "APS168x64.dll" (ByVal Axis_Id As PRA_AXIS_ID_ENUM) As Integer
            Declare Function APS_stop_move Lib "APS168x64.dll" (ByVal Axis_Id As PRA_AXIS_ID_ENUM) As Integer
            Declare Function APS_emg_stop Lib "APS168x64.dll" (ByVal Axis_Id As PRA_AXIS_ID_ENUM) As Integer
            Declare Function APS_home_escape Lib "APS168x64.dll" (ByVal Axis_Id As PRA_AXIS_ID_ENUM) As Integer
            Declare Function APS_set_jog_param Lib "APS168x64.dll" (ByVal Axis_Id As PRA_AXIS_ID_ENUM, ByRef pStr_Jog As JOG_DATA, ByVal Mask As Integer) As Integer
            Declare Function APS_get_jog_param Lib "APS168x64.dll" (ByVal Axis_Id As PRA_AXIS_ID_ENUM, ByRef pStr_Jog As JOG_DATA) As Integer
            Declare Function APS_jog_mode_switch Lib "APS168x64.dll" (ByVal Axis_Id As PRA_AXIS_ID_ENUM, ByVal Turn_No As Integer) As Integer
            Declare Function APS_jog_start Lib "APS168x64.dll" (ByVal Axis_Id As PRA_AXIS_ID_ENUM, ByVal STA_On As JOG_SWITCH_ENUM) As Integer
            Declare Function APS_absolute_linear_move Lib "APS168x64.dll" (ByVal Dimension As PRA_DIMENSION_ENUM, ByVal Axis_Id_Array() As Integer, ByVal Position_Array() As Integer, ByVal Max_Linear_Speed As Integer) As Integer
            Declare Function APS_relative_linear_move Lib "APS168x64.dll" (ByVal Dimension As PRA_DIMENSION_ENUM, ByVal Axis_Id_Array() As Integer, ByVal Distance_Array() As Integer, ByVal Max_Linear_Speed As Integer) As Integer
            Declare Function APS_absolute_arc_move Lib "APS168x64.dll" (ByVal Dimension As PRA_DIMENSION_ENUM, ByVal Axis_Id_Array() As Integer, ByVal Center_Pos_Array() As Integer, ByVal Max_Arc_Speed As Integer, ByVal Angle As Integer) As Integer
            Declare Function APS_relative_arc_move Lib "APS168x64.dll" (ByVal Dimension As PRA_DIMENSION_ENUM, ByVal Axis_Id_Array() As Integer, ByVal Center_Offset_Array() As Integer, ByVal Max_Arc_Speed As Integer, ByVal Angle As Integer) As Integer
            Declare Function APS_int_enable Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal Enable As Integer) As Integer
            Declare Function APS_set_int_factor Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal Item_No As Integer, ByVal Factor_No As Integer, ByVal Enable As Integer) As Integer
            Declare Function APS_get_int_factor Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal Item_No As Integer, ByVal Factor_No As Integer, ByRef Enable As Integer) As Integer
            Declare Function APS_set_int_factorH Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal Item_No As Integer, ByVal Factor_No As Integer, ByVal Enable As Integer) As Integer
            Declare Function APS_int_no_to_handle Lib "APS168x64.dll" (ByVal Int_No As Integer) As Integer
            Declare Function APS_wait_single_int Lib "APS168x64.dll" (ByVal Int_No As Integer, ByVal Time_Out As Integer) As Integer
            Declare Function APS_wait_multiple_int Lib "APS168x64.dll" (ByVal Int_Count As Integer, ByVal Int_No_Array() As Integer, ByVal Wait_All As Integer, ByVal Time_Out As Integer) As Integer
            Declare Function APS_reset_int Lib "APS168x64.dll" (ByVal Int_No As Integer) As Integer
            Declare Function APS_set_int Lib "APS168x64.dll" (ByVal Int_No As Integer) As Integer
            Declare Function APS_write_d_output Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal DO_Group As Integer, ByVal DO_Data As Integer) As Integer
            Declare Function APS_read_d_output Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal DO_Group As Integer, ByRef DO_Data As Integer) As Integer
            Declare Function APS_read_d_input Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal DI_Group As Integer, ByRef DI_Data As Integer) As Integer
            Declare Function APS_write_d_channel_output Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal DO_Group As Integer, ByVal Ch_No As Integer, ByVal DO_Data As Integer) As Integer
            Declare Function APS_read_d_channel_output Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal DO_Group As Integer, ByVal Ch_No As Integer, ByRef DO_Data As Integer) As Integer
            Declare Function APS_read_a_input_value Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal Channel_No As Integer, ByRef Convert_Data As Double) As Integer
            Declare Function APS_read_a_input_data Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal Channel_No As Integer, ByRef Raw_Data As Integer) As Integer
            Declare Function APS_write_a_output_value Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal Channel_No As Integer, ByVal Convert_Data As Double) As Integer
            Declare Function APS_write_a_output_data Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal Channel_No As Integer, ByVal Raw_Data As Integer) As Integer
            Declare Function APS_set_point_table Lib "APS168x64.dll" (ByVal Axis_Id As PRA_AXIS_ID_ENUM, ByVal Index As Integer, ByRef Point As POINT_DATA) As Integer
            Declare Function APS_get_point_table Lib "APS168x64.dll" (ByVal Axis_Id As PRA_AXIS_ID_ENUM, ByVal Index As Integer, ByRef Point As POINT_DATA) As Integer
            Declare Function APS_get_running_point_index Lib "APS168x64.dll" (ByVal Axis_Id As PRA_AXIS_ID_ENUM, ByRef Index As Integer) As Integer
            Declare Function APS_get_start_point_index Lib "APS168x64.dll" (ByVal Axis_Id As PRA_AXIS_ID_ENUM, ByRef Index As Integer) As Integer
            Declare Function APS_get_end_point_index Lib "APS168x64.dll" (ByVal Axis_Id As PRA_AXIS_ID_ENUM, ByRef Index As Integer) As Integer
            Declare Function APS_set_table_move_pause Lib "APS168x64.dll" (ByVal Axis_Id As PRA_AXIS_ID_ENUM, ByVal Pause_en As Integer) As Integer
            Declare Function APS_set_table_move_repeat Lib "APS168x64.dll" (ByVal Axis_Id As PRA_AXIS_ID_ENUM, ByVal Repeat_en As Integer) As Integer
            Declare Function APS_point_table_move Lib "APS168x64.dll" (ByVal Dimension As PRA_DIMENSION_ENUM, ByVal Axis_Id_Array() As Integer, ByVal StartIndex As Integer, ByVal EndIndex As Integer) As Integer
            Declare Function APS_set_point_tableEx Lib "APS168x64.dll" (ByVal Axis_Id As PRA_AXIS_ID_ENUM, ByVal Index As Integer, ByRef Point As PNT_DATA) As Integer
            Declare Function APS_set_point_table_4DL Lib "APS168x64.dll" (ByRef Axis_Id_Array As Integer, ByVal Index As Integer, ByRef Point As PNT_DATA_4DL) As Integer
            Declare Function APS_set_point_tableEx_2D Lib "APS168x64.dll" (ByVal Axis_Id As PRA_AXIS_ID_ENUM, ByVal Axis_Id_2 As PRA_AXIS_ID_ENUM, ByVal Index As Integer, ByRef Point As PNT_DATA_2D) As Integer
            Declare Function APS_set_table_move_ex_pause Lib "APS168x64.dll" (ByVal Axis_Id As PRA_AXIS_ID_ENUM) As Integer
            Declare Function APS_set_table_move_ex_rollback Lib "APS168x64.dll" (ByVal Axis_Id As PRA_AXIS_ID_ENUM, ByVal Max_Speed As Integer) As Integer
            Declare Function APS_set_table_move_ex_resume Lib "APS168x64.dll" (ByVal Axis_Id As PRA_AXIS_ID_ENUM) As Integer
            Declare Function APS_set_point_table_mode2 Lib "APS168x64.dll" (ByVal Axis_Id As PRA_AXIS_ID_ENUM, ByVal Mode As Integer) As Integer
            Declare Function APS_set_point_table2 Lib "APS168x64.dll" (ByVal Dimension As PRA_DIMENSION_ENUM, ByVal Axis_Id_Array() As Integer, ByVal Index As Integer, ByRef Point As POINT_DATA2) As Integer
            Declare Function APS_point_table_continuous_move2 Lib "APS168x64.dll" (ByVal Dimension As PRA_DIMENSION_ENUM, ByVal Axis_Id_Array() As Integer) As Integer
            Declare Function APS_point_table_single_move2 Lib "APS168x64.dll" (ByVal Axis_Id As PRA_AXIS_ID_ENUM, ByVal Index As Integer) As Integer
            Declare Function APS_get_running_point_index2 Lib "APS168x64.dll" (ByVal Axis_Id As PRA_AXIS_ID_ENUM, ByRef Index As Integer) As Integer
            Declare Function APS_point_table_status2 Lib "APS168x64.dll" (ByVal Axis_Id As PRA_AXIS_ID_ENUM, ByRef Status As Integer) As Integer
            Declare Function APS_set_trigger_param Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal Param_No As Integer, ByVal param_val As Integer) As Integer
            Declare Function APS_get_trigger_param Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal Param_No As Integer, ByRef param_val As Integer) As Integer
            Declare Function APS_set_trigger_linear Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal LCmpCh As Integer, ByVal StartPoint As Integer, ByVal RepeatTimes As Integer, ByVal Interval As Integer) As Integer
            Declare Function APS_set_trigger_table Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal TCmpCh As Integer, ByVal DataArr() As Integer, ByVal ArraySize As Integer) As Integer
            Declare Function APS_set_trigger_manual Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal TrgCh As Integer) As Integer
            Declare Function APS_set_trigger_manual_s Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal TrgChInBit As Integer) As Integer
            Declare Function APS_get_trigger_table_cmp Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal TCmpCh As Integer, ByRef CmpVal As Integer) As Integer
            Declare Function APS_get_trigger_linear_cmp Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal LCmpCh As Integer, ByRef CmpVal As Integer) As Integer
            Declare Function APS_get_trigger_count Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal TrgCh As Integer, ByRef TrgCnt As Integer) As Integer
            Declare Function APS_reset_trigger_count Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal TrgCh As Integer) As Integer
            Declare Function APS_enable_trigger_fifo_cmp Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal FCmpCh As Integer, ByVal Enable As Integer) As Integer
            Declare Function APS_get_trigger_fifo_cmp Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal FCmpCh As Integer, ByRef CmpVal As Integer) As Integer
            Declare Function APS_get_trigger_fifo_status Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal FCmpCh As Integer, ByVal FifoSts As Integer) As Integer
            Declare Function APS_set_trigger_fifo_data Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal FCmpCh As Integer, ByRef DataArr As Integer, ByVal ArraySize As Integer, ByVal ShiftFlag As Integer) As Integer
            Declare Function APS_set_trigger_encoder_counter Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal TrgCh As Integer, ByVal TrgCnt As Integer) As Integer
            Declare Function APS_get_trigger_encoder_counter Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal TrgCh As Integer, ByRef TrgCnt As Integer) As Integer
            Declare Function APS_get_pulser_counter Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByRef Counter As Integer) As Integer
            Declare Function APS_set_pulser_counter Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal Counter As Integer) As Integer
            Declare Function APS_rescan_CF Lib "APS168x64.dll" (ByVal Board_ID As Integer) As Integer
            Declare Function APS_get_battery_status Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByRef Battery_status As Integer) As Integer
            Declare Function APS_get_display_data Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal displayDigit As Integer, ByRef displayIndex As Integer) As Integer
            Declare Function APS_set_display_data Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal displayDigit As Integer, ByVal displayIndex As Integer) As Integer
            Declare Function APS_get_button_status Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByRef buttonstatus As Integer) As Integer
            Declare Function APS_set_nv_ram Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal RamNo As Integer, ByVal DataWidth As Integer, ByVal Offset As Integer, ByVal data As Integer) As Integer
            Declare Function APS_get_nv_ram Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal RamNo As Integer, ByVal DataWidth As Integer, ByVal Offset As Integer, ByRef data As Integer) As Integer
            Declare Function APS_clear_nv_ram Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal RamNo As Integer) As Integer
            Declare Function APS_get_timer_counter Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal TmrCh As Integer, ByRef Cnt As Integer) As Integer
            Declare Function APS_set_timer_counter Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal TmrCh As Integer, ByVal Cnt As Integer) As Integer
            Declare Function APS_start_timer Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal TrgCh As Integer, ByVal Start As Integer) As Integer
            Declare Function APS_register_virtual_board Lib "APS168x64.dll" (ByVal VirCardIndex As Integer, ByVal Enable As Integer) As Integer
            Declare Function APS_get_virtual_board_info Lib "APS168x64.dll" (ByVal VirCardIndex As Integer, ByRef Enable As Integer) As Integer
            Declare Function APS_register_virtual_board_ex Lib "APS168x64.dll" (ByVal VirCardIndex As Integer, ByVal Count As Integer, ByVal Enable As Integer) As Integer
            Declare Function APS_get_virtual_board_info_ex Lib "APS168x64.dll" (ByVal VirCardIndex As Integer, ByRef Count As Integer, ByRef Enable As Integer) As Integer
            Declare Function APS_set_axis_param_f Lib "APS168x64.dll" (ByVal Axis_Id As PRA_AXIS_ID_ENUM, ByVal AXS_Param_No As Integer, ByVal AXS_Param As Double) As Integer
            Declare Function APS_get_axis_param_f Lib "APS168x64.dll" (ByVal Axis_Id As PRA_AXIS_ID_ENUM, ByVal AXS_Param_No As Integer, ByRef AXS_Param As Double) As Integer
            Declare Function APS_get_eep_curr_drv_ctrl_mode Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByRef ModeInBit As Integer) As Integer
            Declare Function APS_get_command_f Lib "APS168x64.dll" (ByVal Axis_Id As PRA_AXIS_ID_ENUM, ByRef Command As Double) As Integer
            Declare Function APS_set_command_f Lib "APS168x64.dll" (ByVal Axis_Id As PRA_AXIS_ID_ENUM, ByVal Command As Double) As Integer
            Declare Function APS_get_position_f Lib "APS168x64.dll" (ByVal Axis_Id As PRA_AXIS_ID_ENUM, ByRef Position As Double) As Integer
            Declare Function APS_set_position_f Lib "APS168x64.dll" (ByVal Axis_Id As PRA_AXIS_ID_ENUM, ByVal Position As Double) As Integer
            Declare Function APS_get_command_velocity_f Lib "APS168x64.dll" (ByVal Axis_Id As PRA_AXIS_ID_ENUM, ByRef Velocity As Double) As Integer
            Declare Function APS_get_target_position_f Lib "APS168x64.dll" (ByVal Axis_Id As PRA_AXIS_ID_ENUM, ByRef Targ_Pos As Double) As Integer
            Declare Function APS_get_error_position_f Lib "APS168x64.dll" (ByVal Axis_Id As PRA_AXIS_ID_ENUM, ByRef Err_Pos As Double) As Integer
            Declare Function APS_get_feedback_velocity_f Lib "APS168x64.dll" (ByVal Axis_Id As PRA_AXIS_ID_ENUM, ByRef Velocity As Double) As Integer
            Declare Function APS_get_mq_free_space Lib "APS168x64.dll" (ByVal Axis_Id As PRA_AXIS_ID_ENUM, ByRef Space As Integer) As Integer
            Declare Function APS_get_mq_usage Lib "APS168x64.dll" (ByVal Axis_Id As PRA_AXIS_ID_ENUM, ByRef Usage As Integer) As Integer
            Declare Function APS_get_stop_code Lib "APS168x64.dll" (ByVal Axis_Id As PRA_AXIS_ID_ENUM, ByRef Code As Integer) As Integer
            Declare Function APS_set_sampling_param_ex Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByRef Param As SAMP_PARAM) As Integer
            Declare Function APS_get_sampling_param_ex Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByRef Param As SAMP_PARAM) As Integer
            Declare Function APS_wait_trigger_sampling_ex Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal Length As Integer, ByVal PreTrgLen As Integer, ByVal TimeOutMs As Integer, ByRef DataArr As STR_SAMP_DATA_8CH) As Integer
            Declare Function APS_wait_trigger_sampling_async_ex Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal Length As Integer, ByVal PreTrgLen As Integer, ByVal TimeOutMs As Integer, ByRef DataArr As STR_SAMP_DATA_8CH) As Integer
            Declare Function APS_get_sampling_data_ex Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByRef Length As Integer, ByRef DataArr As STR_SAMP_DATA_8CH, ByRef Status As Integer) As Integer
            Declare Function APS_read_a_output_value Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal Channel_No As Integer, ByRef Convert_Data As Double) As Integer
            Declare Function APS_ptp Lib "APS168x64.dll" (ByVal Axis_Id As PRA_AXIS_ID_ENUM, ByVal APS_Option As PRA_APS_OPTION_ENUM, ByVal Position As Double, ByRef Wait As ASYNCALL) As Integer
            Declare Function APS_ptp_v Lib "APS168x64.dll" (ByVal Axis_Id As PRA_AXIS_ID_ENUM, ByVal APS_Option As PRA_APS_OPTION_ENUM, ByVal Position As Double, ByVal Vm As Double, ByRef Wait As ASYNCALL) As Integer
            Declare Function APS_ptp_all Lib "APS168x64.dll" (ByVal Axis_Id As PRA_AXIS_ID_ENUM, ByVal APS_Option As PRA_APS_OPTION_ENUM, ByVal Position As Double, ByVal Vs As Double, ByVal Vm As Double, ByVal Ve As Double, ByVal Acc As Double, ByVal Dec As Double, ByVal SFac As Double, ByRef Wait As ASYNCALL) As Integer
            Declare Function APS_vel Lib "APS168x64.dll" (ByVal Axis_Id As PRA_AXIS_ID_ENUM, ByVal APS_Option As PRA_APS_OPTION_ENUM, ByVal Vm As Double, ByRef Wait As ASYNCALL) As Integer
            Declare Function APS_vel_all Lib "APS168x64.dll" (ByVal Axis_Id As PRA_AXIS_ID_ENUM, ByVal APS_Option As PRA_APS_OPTION_ENUM, ByVal Vs As Double, ByVal Vm As Double, ByVal Ve As Double, ByVal Acc As Double, ByVal Dec As Double, ByVal SFac As Double, ByRef Wait As ASYNCALL) As Integer
            Declare Function APS_line Lib "APS168x64.dll" (ByVal Dimension As PRA_DIMENSION_ENUM, ByVal Axis_Id_Array() As Integer, ByVal APS_Option As PRA_APS_OPTION_ENUM, ByVal PositionArray() As Double, ByRef TransPara As Double, ByRef Wait As ASYNCALL) As Integer
            Declare Function APS_line_v Lib "APS168x64.dll" (ByVal Dimension As PRA_DIMENSION_ENUM, ByVal Axis_Id_Array() As Integer, ByVal APS_Option As PRA_APS_OPTION_ENUM, ByVal PositionArray() As Double, ByRef TransPara As Double, ByVal Vm As Double, ByRef Wait As ASYNCALL) As Integer
            Declare Function APS_line_all Lib "APS168x64.dll" (ByVal Dimension As PRA_DIMENSION_ENUM, ByVal Axis_Id_Array() As Integer, ByVal APS_Option As PRA_APS_OPTION_ENUM, ByVal PositionArray() As Double, ByRef TransPara As Double, ByVal Vs As Double, ByVal Vm As Double, ByVal Ve As Double, ByVal Acc As Double, ByVal Dec As Double, ByVal SFac As Double, ByRef Wait As ASYNCALL) As Integer
            Declare Function APS_arc2_ca Lib "APS168x64.dll" (ByVal Axis_Id_Array() As Integer, ByVal APS_Option As PRA_APS_OPTION_ENUM, ByVal CenterArray() As Double, ByVal Angle As Double, ByRef TransPara As Double, ByRef Wait As ASYNCALL) As Integer
            Declare Function APS_arc2_ca_v Lib "APS168x64.dll" (ByVal Axis_Id_Array() As Integer, ByVal APS_Option As PRA_APS_OPTION_ENUM, ByVal CenterArray() As Double, ByVal Angle As Double, ByRef TransPara As Double, ByVal Vm As Double, ByRef Wait As ASYNCALL) As Integer
            Declare Function APS_arc2_ca_all Lib "APS168x64.dll" (ByVal Axis_Id_Array() As Integer, ByVal APS_Option As PRA_APS_OPTION_ENUM, ByVal CenterArray() As Double, ByVal Angle As Double, ByRef TransPara As Double, ByVal Vs As Double, ByVal Vm As Double, ByVal Ve As Double, ByVal Acc As Double, ByVal Dec As Double, ByVal SFac As Double, ByRef Wait As ASYNCALL) As Integer
            Declare Function APS_arc2_ce Lib "APS168x64.dll" (ByVal Axis_Id_Array() As Integer, ByVal APS_Option As PRA_APS_OPTION_ENUM, ByVal CenterArray() As Double, ByVal EndArray() As Double, ByVal Dir As Short, ByRef TransPara As Double, ByRef Wait As ASYNCALL) As Integer
            Declare Function APS_arc2_ce_v Lib "APS168x64.dll" (ByVal Axis_Id_Array() As Integer, ByVal APS_Option As PRA_APS_OPTION_ENUM, ByVal CenterArray() As Double, ByVal EndArray() As Double, ByVal Dir As Short, ByRef TransPara As Double, ByVal Vm As Double, ByRef Wait As ASYNCALL) As Integer
            Declare Function APS_arc2_ce_all Lib "APS168x64.dll" (ByVal Axis_Id_Array() As Integer, ByVal APS_Option As PRA_APS_OPTION_ENUM, ByVal CenterArray() As Double, ByVal EndArray() As Double, ByVal Dir As Short, ByRef TransPara As Double, ByVal Vs As Double, ByVal Vm As Double, ByVal Ve As Double, ByVal Acc As Double, ByVal Dec As Double, ByVal SFac As Double, ByRef Wait As ASYNCALL) As Integer
            Declare Function APS_arc3_ca Lib "APS168x64.dll" (ByVal Axis_Id_Array() As Integer, ByVal APS_Option As PRA_APS_OPTION_ENUM, ByVal CenterArray() As Double, ByVal NormalArray() As Double, ByVal Angle As Double, ByRef TransPara As Double, ByRef Wait As ASYNCALL) As Integer
            Declare Function APS_arc3_ca_v Lib "APS168x64.dll" (ByVal Axis_Id_Array() As Integer, ByVal APS_Option As PRA_APS_OPTION_ENUM, ByVal CenterArray() As Double, ByVal NormalArray() As Double, ByVal Angle As Double, ByRef TransPara As Double, ByVal Vm As Double, ByRef Wait As ASYNCALL) As Integer
            Declare Function APS_arc3_ca_all Lib "APS168x64.dll" (ByVal Axis_Id_Array() As Integer, ByVal APS_Option As PRA_APS_OPTION_ENUM, ByVal CenterArray() As Double, ByVal NormalArray() As Double, ByVal Angle As Double, ByRef TransPara As Double, ByVal Vs As Double, ByVal Vm As Double, ByVal Ve As Double, ByVal Acc As Double, ByVal Dec As Double, ByVal SFac As Double, ByRef Wait As ASYNCALL) As Integer
            Declare Function APS_arc3_ce Lib "APS168x64.dll" (ByVal Axis_Id_Array() As Integer, ByVal APS_Option As PRA_APS_OPTION_ENUM, ByVal CenterArray() As Double, ByVal EndArray() As Double, ByVal Dir As Short, ByRef TransPara As Double, ByRef Wait As ASYNCALL) As Integer
            Declare Function APS_arc3_ce_v Lib "APS168x64.dll" (ByVal Axis_Id_Array() As Integer, ByVal APS_Option As PRA_APS_OPTION_ENUM, ByVal CenterArray() As Double, ByVal EndArray() As Double, ByVal Dir As Short, ByRef TransPara As Double, ByVal Vm As Double, ByRef Wait As ASYNCALL) As Integer
            Declare Function APS_arc3_ce_all Lib "APS168x64.dll" (ByVal Axis_Id_Array() As Integer, ByVal APS_Option As PRA_APS_OPTION_ENUM, ByVal CenterArray() As Double, ByVal EndArray() As Double, ByVal Dir As Short, ByRef TransPara As Double, ByVal Vs As Double, ByVal Vm As Double, ByVal Ve As Double, ByVal Acc As Double, ByVal Dec As Double, ByVal SFac As Double, ByRef Wait As ASYNCALL) As Integer
            Declare Function APS_spiral_ca Lib "APS168x64.dll" (ByVal Axis_Id_Array() As Integer, ByVal APS_Option As PRA_APS_OPTION_ENUM, ByVal CenterArray() As Double, ByVal NormalArray() As Double, ByVal Angle As Double, ByVal DeltaH As Double, ByVal FinalR As Double, ByRef TransPara As Double, ByRef Wait As ASYNCALL) As Integer
            Declare Function APS_spiral_ca_v Lib "APS168x64.dll" (ByVal Axis_Id_Array() As Integer, ByVal APS_Option As Integer, ByVal CenterArray() As Double, ByVal NormalArray() As Double, ByVal Angle As Double, ByVal DeltaH As Double, ByVal FinalR As Double, ByRef TransPara As Double, ByVal Vm As Double, ByRef Wait As ASYNCALL) As Integer
            Declare Function APS_spiral_ca_all Lib "APS168x64.dll" (ByVal Axis_Id_Array() As Integer, ByVal APS_Option As Integer, ByVal CenterArray() As Double, ByVal NormalArray() As Double, ByVal Angle As Double, ByVal DeltaH As Double, ByVal FinalR As Double, ByRef TransPara As Double, ByVal Vs As Double, ByVal Vm As Double, ByVal Ve As Double, ByVal Acc As Double, ByVal Dec As Double, ByVal SFac As Double, ByRef Wait As ASYNCALL) As Integer
            Declare Function APS_spiral_ce Lib "APS168x64.dll" (ByVal Axis_Id_Array() As Integer, ByVal APS_Option As Integer, ByVal CenterArray() As Double, ByVal NormalArray() As Double, ByVal EndArray() As Double, ByVal Dir As Short, ByRef TransPara As Double, ByRef Wait As ASYNCALL) As Integer
            Declare Function APS_spiral_ce_v Lib "APS168x64.dll" (ByVal Axis_Id_Array() As Integer, ByVal APS_Option As Integer, ByVal CenterArray() As Double, ByVal NormalArray() As Double, ByVal EndArray() As Double, ByVal Dir As Short, ByRef TransPara As Double, ByVal Vm As Double, ByRef Wait As ASYNCALL) As Integer
            Declare Function APS_spiral_ce_all Lib "APS168x64.dll" (ByVal Axis_Id_Array() As Integer, ByVal APS_Option As Integer, ByVal CenterArray() As Double, ByVal NormalArray() As Double, ByVal EndArray() As Double, ByVal Dir As Short, ByRef TransPara As Double, ByVal Vs As Double, ByVal Vm As Double, ByVal Ve As Double, ByVal Acc As Double, ByVal Dec As Double, ByVal SFac As Double, ByRef Wait As ASYNCALL) As Integer
            Declare Function APS_pt_dwell Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal PtbId As Integer, ByRef Prof As PTDWL, ByRef Status As PTSTS) As Integer
            Declare Function APS_pt_line Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal PtbId As Integer, ByRef Prof As PTLINE, ByRef Status As PTSTS) As Integer
            Declare Function APS_pt_arc2_ca Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal PtbId As Integer, ByRef Prof As PTA2CA, ByRef Status As PTSTS) As Integer
            Declare Function APS_pt_arc2_ce Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal PtbId As Integer, ByRef Prof As PTA2CE, ByRef Status As PTSTS) As Integer
            Declare Function APS_pt_arc3_ca Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal PtbId As Integer, ByRef Prof As PTA3CA, ByRef Status As PTSTS) As Integer
            Declare Function APS_pt_arc3_ce Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal PtbId As Integer, ByRef Prof As PTA3CE, ByRef Status As PTSTS) As Integer
            Declare Function APS_pt_spiral_ca Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal PtbId As Integer, ByRef Prof As PTHCA, ByRef Status As PTSTS) As Integer
            Declare Function APS_pt_spiral_ce Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal PtbId As Integer, ByRef Prof As PTHCE, ByRef Status As PTSTS) As Integer
            Declare Function APS_pt_enable Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal PtbId As Integer, ByVal Dimension As Integer, ByVal AxisArr() As Integer) As Integer
            Declare Function APS_pt_disable Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal PtbId As Integer) As Integer
            Declare Function APS_get_pt_info Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal PtbId As Integer, ByRef Info As PTINFO) As Integer
            Declare Function APS_pt_set_vs Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal PtbId As Integer, ByVal Vs As Double) As Integer
            Declare Function APS_pt_get_vs Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal PtbId As Integer, ByRef Vs As Double) As Integer
            Declare Function APS_pt_start Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal PtbId As Integer) As Integer
            Declare Function APS_pt_stop Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal PtbId As Integer) As Integer
            Declare Function APS_get_pt_status Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal PtbId As Integer, ByRef Status As PTSTS) As Integer
            Declare Function APS_reset_pt_buffer Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal PtbId As Integer) As Integer
            Declare Function APS_pt_roll_back Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal PtbId As Integer, ByVal Max_Speed As Double) As Integer
            Declare Function APS_pt_get_error Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal PtbId As Integer, ByRef ErrCode As Integer) As Integer
            Declare Function APS_pt_ext_set_do_ch Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal PtbId As Integer, ByVal Channel As Integer, ByVal OnOff As Integer) As Integer
            Declare Function APS_pt_ext_set_table_no Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal PtbId As Integer, ByVal CtrlNo As Integer, ByVal TableNo As Integer) As Integer
            Declare Function APS_pt_set_absolute Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal PtbId As Integer) As Integer
            Declare Function APS_pt_set_relative Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal PtbId As Integer) As Integer
            Declare Function APS_pt_set_trans_buffered Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal PtbId As Integer) As Integer
            Declare Function APS_pt_set_trans_inp Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal PtbId As Integer) As Integer
            Declare Function APS_pt_set_trans_Magnificationend_dec Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal PtbId As Integer, ByVal Bp As Double) As Integer
            Declare Function APS_pt_set_trans_Magnificationend_dist Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal PtbId As Integer, ByVal Bp As Double) As Integer
            Declare Function APS_pt_set_trans_Magnificationend_pcnt Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal PtbId As Integer, ByVal Bp As Double) As Integer
            Declare Function APS_pt_set_acc Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal PtbId As Integer, ByVal Acc As Double) As Integer
            Declare Function APS_pt_set_dec Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal PtbId As Integer, ByVal Dec As Double) As Integer
            Declare Function APS_pt_set_acc_dec Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal PtbId As Integer, ByVal AccDec As Double) As Integer
            Declare Function APS_pt_set_s Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal PtbId As Integer, ByVal Sf As Double) As Integer
            Declare Function APS_pt_set_vm Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal PtbId As Integer, ByVal Vm As Double) As Integer
            Declare Function APS_pt_set_ve Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal PtbId As Integer, ByVal Ve As Double) As Integer
            Declare Function APS_load_vmc_program Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal TaskNum As Integer, ByRef pFile As String, ByVal Password As Integer) As Integer
            Declare Function APS_save_vmc_program Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal TaskNum As Integer, ByRef pFile As String, ByVal Password As Integer) As Integer
            Declare Function APS_load_amc_program Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal TaskNum As Integer, ByRef pFile As String, ByVal Password As Integer) As Integer
            Declare Function APS_save_amc_program Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal TaskNum As Integer, ByRef pFile As String, ByVal Password As Integer) As Integer
            Declare Function APS_set_task_mode Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal TaskNum As Integer, ByVal Mode As Byte, ByVal LastIP As UShort) As Integer
            Declare Function APS_get_task_mode Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal TaskNum As Integer, ByRef Mode As Byte, ByRef LastIP As UShort) As Integer
            Declare Function APS_start_task Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal TaskNum As Integer, ByVal CtrlCmd As Integer) As Integer
            Declare Function APS_get_task_info Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal TaskNum As Integer, ByRef Info As TSK_INFO) As Integer
            Declare Function APS_get_task_msg Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByRef QueueSts As UShort, ByRef ActualSize As UShort, ByRef CharArr As Byte) As Integer
            Declare Function APS_get_encoder Lib "APS168x64.dll" (ByVal Axis_Id As Integer, ByRef Encoder As Integer) As Integer
            Declare Function APS_get_latch_counter Lib "APS168x64.dll" (ByVal Axis_Id As Integer, ByVal Src As Integer, ByRef Counter As Integer) As Integer
            Declare Function APS_get_latch_event Lib "APS168x64.dll" (ByVal Axis_Id As Integer, ByVal Src As Integer, ByRef ENT As Integer) As Integer
            Declare Function APS_get_command_counter Lib "APS168x64.dll" (ByVal Axis_Id As Integer, ByRef Counter As Integer) As Integer
            Declare Function APS_wdt_start Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal TimerNo As Integer, ByVal TimeOut As Integer) As Integer
            Declare Function APS_wdt_get_timeout_period Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal TimerNo As Integer, ByRef TimeOut As Integer) As Integer
            Declare Function APS_wdt_reset_counter Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal TimerNo As Integer) As Integer
            Declare Function APS_wdt_get_counter Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal TimerNo As Integer, ByRef Counter As Integer) As Integer
            Declare Function APS_wdt_set_action_event Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal TimerNo As Integer, ByVal EventByBit As Integer) As Integer
            Declare Function APS_wdt_get_action_event Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal TimerNo As Integer, ByRef EventByBit As Integer) As Integer
            Declare Function APS_move_trigger Lib "APS168x64.dll" (ByVal Dimension As Integer, ByVal Axis_Id_Array() As Integer) As Integer
            Declare Function APS_stop_move_multi Lib "APS168x64.dll" (ByVal Dimension As Integer, ByVal Axis_Id_Array() As Integer) As Integer
            Declare Function APS_emg_stop_multi Lib "APS168x64.dll" (ByVal Dimension As Integer, ByVal Axis_Id_Array() As Integer) As Integer
            Declare Function APS_start_gear Lib "APS168x64.dll" (ByVal Axis_Id As Integer, ByVal Mode As Integer) As Integer
            Declare Function APS_get_gear_status Lib "APS168x64.dll" (ByVal Axis_Id As Integer, ByRef Status As Integer) As Integer
            Declare Function APS_set_ltc_counter Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal CntNum As Integer, ByVal CntValue As Integer) As Integer
            Declare Function APS_get_ltc_counter Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal CntNum As Integer, ByRef CntValue As Integer) As Integer
            Declare Function APS_set_ltc_fifo_param Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal FLtcCh As Integer, ByVal Param_No As Integer, ByVal Param_No As Integer) As Integer
            Declare Function APS_get_ltc_fifo_param Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal FLtcCh As Integer, ByVal Param_No As Integer, ByRef Param_No As Integer) As Integer
            Declare Function APS_manual_latch Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal LatchSignalInBits As Integer) As Integer
            Declare Function APS_enable_ltc_fifo Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal FLtcCh As Integer, ByVal Enable As Integer) As Integer
            Declare Function APS_reset_ltc_fifo Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal FLtcCh As Integer) As Integer
            Declare Function APS_get_ltc_fifo_data Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal FLtcCh As Integer, ByRef Data As Integer) As Integer
            Declare Function APS_get_ltc_fifo_usage Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal FLtcCh As Integer, ByRef Usage As Integer) As Integer
            Declare Function APS_get_ltc_fifo_free_space Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal FLtcCh As Integer, ByRef FreeSpace As Integer) As Integer
            Declare Function APS_get_ltc_fifo_status Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal FLtcCh As Integer, ByRef Status As Integer) As Integer
        End Class

        Class DASK_LIB
            'ADLink PCI Card Type
            Public Const PCI_6208V As Short = 1
            Public Const PCI_6208A As Short = 2
            Public Const PCI_6308V As Short = 3
            Public Const PCI_6308A As Short = 4
            Public Const PCI_7200 As Short = 5
            Public Const PCI_7230 As Short = 6
            Public Const PCI_7233 As Short = 7
            Public Const PCI_7234 As Short = 8
            Public Const PCI_7248 As Short = 9
            Public Const PCI_7249 As Short = 10
            Public Const PCI_7250 As Short = 11
            Public Const PCI_7252 As Short = 12
            Public Const PCI_7296 As Short = 13
            Public Const PCI_7300A_RevA As Short = 14
            Public Const PCI_7300A_RevB As Short = 15
            Public Const PCI_7432 As Short = 16
            Public Const PCI_7433 As Short = 17
            Public Const PCI_7434 As Short = 18
            Public Const PCI_8554 As Short = 19
            Public Const PCI_9111DG As Short = 20
            Public Const PCI_9111HR As Short = 21
            Public Const PCI_9112 As Short = 22
            Public Const PCI_9113 As Short = 23
            Public Const PCI_9114DG As Short = 24
            Public Const PCI_9114HG As Short = 25
            Public Const PCI_9118DG As Short = 26
            Public Const PCI_9118HG As Short = 27
            Public Const PCI_9118HR As Short = 28
            Public Const PCI_9810 As Short = 29
            Public Const PCI_9812 As Short = 30
            Public Const PCI_7396 As Short = 31
            Public Const PCI_9116 As Short = 32
            Public Const PCI_7256 As Short = 33
            Public Const PCI_7258 As Short = 34
            Public Const PCI_7260 As Short = 35
            Public Const PCI_7452 As Short = 36
            Public Const PCI_7442 As Short = 37
            Public Const PCI_7443 As Short = 38
            Public Const PCI_7444 As Short = 39
            Public Const PCI_9221 As Short = 40

            Public Const MAX_CARD As Short = 32

            'Error Code
            Public Const NoError As Short = 0
            Public Const ErrorUnknownCardType As Short = -1
            Public Const ErrorInvalidCardNumber As Short = -2
            Public Const ErrorTooManyCardRegistered As Short = -3
            Public Const ErrorCardNotRegistered As Short = -4
            Public Const ErrorFuncNotSupport As Short = -5
            Public Const ErrorInvalidIoChannel As Short = -6
            Public Const ErrorInvalidAdRange As Short = -7
            Public Const ErrorContIoNotAllowed As Short = -8
            Public Const ErrorDiffRangeNotSupport As Short = -9
            Public Const ErrorLastChannelNotZero As Short = -10
            Public Const ErrorChannelNotDescending As Short = -11
            Public Const ErrorChannelNotAscending As Short = -12
            Public Const ErrorOpenDriverFailed As Short = -13
            Public Const ErrorOpenEventFailed As Short = -14
            Public Const ErrorTransferCountTooLarge As Short = -15
            Public Const ErrorNotDoubleBufferMode As Short = -16
            Public Const ErrorInvalidSampleRate As Short = -17
            Public Const ErrorInvalidCounterMode As Short = -18
            Public Const ErrorInvalidCounter As Short = -19
            Public Const ErrorInvalidCounterState As Short = -20
            Public Const ErrorInvalidBinBcdParam As Short = -21
            Public Const ErrorBadCardType As Short = -22
            Public Const ErrorInvalidDaRange As Short = -23
            Public Const ErrorAdTimeOut As Short = -24
            Public Const ErrorNoAsyncAI As Short = -25
            Public Const ErrorNoAsyncAO As Short = -26
            Public Const ErrorNoAsyncDI As Short = -27
            Public Const ErrorNoAsyncDO As Short = -28
            Public Const ErrorNotInputPort As Short = -29
            Public Const ErrorNotOutputPort As Short = -30
            Public Const ErrorInvalidDioPort As Short = -31
            Public Const ErrorInvalidDioLine As Short = -32
            Public Const ErrorContIoActive As Short = -33
            Public Const ErrorDblBufModeNotAllowed As Short = -34
            Public Const ErrorConfigFailed As Short = -35
            Public Const ErrorInvalidPortDirection As Short = -36
            Public Const ErrorBeginThreadError As Short = -37
            Public Const ErrorInvalidPortWidth As Short = -38
            Public Const ErrorInvalidCtrSource As Short = -39
            Public Const ErrorOpenFile As Short = -40
            Public Const ErrorAllocateMemory As Short = -41
            Public Const ErrorDaVoltageOutOfRange As Short = -42
            Public Const ErrorDaExtRefNotAllowed As Short = -43
            Public Const ErrorDIODataWidthError As Short = -44
            Public Const ErrorTaskCodeError As Short = -45
            Public Const ErrortriggercountError As Short = -46
            Public Const ErrorInvalidTriggerMode As Short = -47
            Public Const ErrorInvalidTriggerType As Short = -48
            Public Const ErrorInvalidCounterValue As Short = -50
            Public Const ErrorInvalidEventHandle As Short = -60
            Public Const ErrorNoMessageAvailable As Short = -61
            Public Const ErrorEventMessgaeNotAdded As Short = -62
            Public Const ErrorCalibrationTimeOut As Short = -63
            Public Const ErrorUndefinedParameter As Short = -64
            Public Const ErrorInvalidBufferID As Short = -65

            'Error code for driver API
            Public Const ErrorConfigIoctl As Short = -201
            Public Const ErrorAsyncSetIoctl As Short = -202
            Public Const ErrorDBSetIoctl As Short = -203
            Public Const ErrorDBHalfReadyIoctl As Short = -204
            Public Const ErrorContOPIoctl As Short = -205
            Public Const ErrorContStatusIoctl As Short = -206
            Public Const ErrorPIOIoctl As Short = -207
            Public Const ErrorDIntSetIoctl As Short = -208
            Public Const ErrorWaitEvtIoctl As Short = -209
            Public Const ErrorOpenEvtIoctl As Short = -210
            Public Const ErrorCOSIntSetIoctl As Short = -211
            Public Const ErrorMemMapIoctl As Short = -212
            Public Const ErrorMemUMapSetIoctl As Short = -213
            Public Const ErrorCTRIoctl As Short = -214
            Public Const ErrorGetResIoctl As Short = -215
            Public Const ErrorCalIoctl As Short = -216

            'Synchronous Mode
            Public Const SYNCH_OP As Short = 1
            Public Const ASYNCH_OP As Short = 2

            'AD Range
            Public Const AD_B_10_V As Short = 1
            Public Const AD_B_5_V As Short = 2
            Public Const AD_B_2_5_V As Short = 3
            Public Const AD_B_1_25_V As Short = 4
            Public Const AD_B_0_625_V As Short = 5
            Public Const AD_B_0_3125_V As Short = 6
            Public Const AD_B_0_5_V As Short = 7
            Public Const AD_B_0_05_V As Short = 8
            Public Const AD_B_0_005_V As Short = 9
            Public Const AD_B_1_V As Short = 10
            Public Const AD_B_0_1_V As Short = 11
            Public Const AD_B_0_01_V As Short = 12
            Public Const AD_B_0_001_V As Short = 13
            Public Const AD_U_20_V As Short = 14
            Public Const AD_U_10_V As Short = 15
            Public Const AD_U_5_V As Short = 16
            Public Const AD_U_2_5_V As Short = 17
            Public Const AD_U_1_25_V As Short = 18
            Public Const AD_U_1_V As Short = 19
            Public Const AD_U_0_1_V As Short = 20
            Public Const AD_U_0_01_V As Short = 21
            Public Const AD_U_0_001_V As Short = 22
            Public Const AD_B_2_V As Short = 23
            Public Const AD_U_2_V As Short = 24
            Public Const AD_B_0_2_V As Short = 25

            'Trigger Source
            Public Const TRIG_SOFTWARE As Short = 0
            Public Const TRIG_INT_PACER As Short = 1
            Public Const TRIG_EXT_STROBE As Short = 2
            Public Const TRIG_HANDSHAKE As Short = 3
            Public Const TRIG_CLK_10MHZ As Short = 4 'PCI-7300A
            Public Const TRIG_CLK_20MHZ As Short = 5 'PCI-7300A
            Public Const TRIG_DO_CLK_TIMER_ACK As Short = 6 'PCI-7300A Rev. B
            Public Const TRIG_DO_CLK_10M_ACK As Short = 7 'PCI-7300A Rev. B
            Public Const TRIG_DO_CLK_20M_ACK As Short = 8 'PCI-7300A Rev. B

            'Virtual sampling rate for using external clock as the clock source
            Public Const CLKSRC_EXT_SampRate As Short = 10000

            '--------- Constants for PCI-6208A --------------
            'Output Mode
            Public Const P6208_CURRENT_0_20MA As Short = 0
            Public Const P6208_CURRENT_5_25MA As Short = 1
            Public Const P6208_CURRENT_4_20MA As Short = 3

            '--------- Constants for PCI-6308A/PCI-6308V --------------
            'Output Mode
            Public Const P6308_CURRENT_0_20MA As Short = 0
            Public Const P6308_CURRENT_5_25MA As Short = 1
            Public Const P6308_CURRENT_4_20MA As Short = 3
            'AO Setting
            Public Const P6308V_AO_CH0_3 As Short = 0
            Public Const P6308V_AO_CH4_7 As Short = 1
            Public Const P6308V_AO_UNIPOLAR As Short = 0
            Public Const P6308V_AO_BIPOLAR As Short = 1

            '--------- Constants for PCI-7200 --------------
            'InputMode
            Public Const DI_WAITING As Short = &H2S
            Public Const DI_NOWAITING As Short = &H0S

            Public Const DI_TRIG_RISING As Short = &H4S
            Public Const DI_TRIG_FALLING As Short = &H0S

            Public Const IREQ_RISING As Short = &H8S
            Public Const IREQ_FALLING As Short = &H0S

            'Output Mode
            Public Const OREQ_ENABLE As Short = &H10S
            Public Const OREQ_DISABLE As Short = &H0S

            Public Const OTRIG_HIGH As Short = &H20S
            Public Const OTRIG_LOW As Short = &H0S

            '--------- Constants for PCI-7248/7296/7442 --------------
            'DIO Port Direction
            Public Const INPUT_PORT As Short = 1
            Public Const OUTPUT_PORT As Short = 2
            'DIO Line Direction
            Public Const INPUT_LINE As Short = 1
            Public Const OUTPUT_LINE As Short = 2

            'Channel&Port
            Public Const Channel_P1A As Short = 0
            Public Const Channel_P1B As Short = 1
            Public Const Channel_P1C As Short = 2
            Public Const Channel_P1CL As Short = 3
            Public Const Channel_P1CH As Short = 4
            Public Const Channel_P1AE As Short = 10
            Public Const Channel_P1BE As Short = 11
            Public Const Channel_P1CE As Short = 12
            Public Const Channel_P2A As Short = 5
            Public Const Channel_P2B As Short = 6
            Public Const Channel_P2C As Short = 7
            Public Const Channel_P2CL As Short = 8
            Public Const Channel_P2CH As Short = 9
            Public Const Channel_P2AE As Short = 15
            Public Const Channel_P2BE As Short = 16
            Public Const Channel_P2CE As Short = 17
            Public Const Channel_P3A As Short = 10
            Public Const Channel_P3B As Short = 11
            Public Const Channel_P3C As Short = 12
            Public Const Channel_P3CL As Short = 13
            Public Const Channel_P3CH As Short = 14
            Public Const Channel_P4A As Short = 15
            Public Const Channel_P4B As Short = 16
            Public Const Channel_P4C As Short = 17
            Public Const Channel_P4CL As Short = 18
            Public Const Channel_P4CH As Short = 19
            Public Const Channel_P5A As Short = 20
            Public Const Channel_P5B As Short = 21
            Public Const Channel_P5C As Short = 22
            Public Const Channel_P5CL As Short = 23
            Public Const Channel_P5CH As Short = 24
            Public Const Channel_P6A As Short = 25
            Public Const Channel_P6B As Short = 26
            Public Const Channel_P6C As Short = 27
            Public Const Channel_P6CL As Short = 28
            Public Const Channel_P6CH As Short = 29
            Public Const Channel_P1 As Short = 30
            Public Const Channel_P2 As Short = 31
            Public Const Channel_P3 As Short = 32
            Public Const Channel_P4 As Short = 33
            Public Const Channel_P1E As Short = 34
            Public Const Channel_P2E As Short = 35
            Public Const Channel_P3E As Short = 36
            Public Const Channel_P4E As Short = 37
            ' 7442	
            Public Const P7442_CH0 As Short = 0
            Public Const P7442_CH1 As Short = 1
            Public Const P7442_TTL0 As Short = 2
            Public Const P7442_TTL1 As Short = 3
            ' P7443
            Public Const P7443_CH0 As Short = 0
            Public Const P7443_CH1 As Short = 1
            Public Const P7443_CH2 As Short = 2
            Public Const P7443_CH3 As Short = 3
            Public Const P7443_TTL0 As Short = 4
            Public Const P7443_TTL1 As Short = 5
            ' P7444
            Public Const P7444_CH0 As Short = 0
            Public Const P7444_CH1 As Short = 1
            Public Const P7444_CH2 As Short = 2
            Public Const P7444_CH3 As Short = 3
            Public Const P7444_TTL0 As Short = 4
            Public Const P7444_TTL1 As Short = 5
            '--------- Constants for PCI-7300A --------------
            'Wait Status
            Public Const P7300_WAIT_NO As Short = 0
            Public Const P7300_WAIT_TRG As Short = 1
            Public Const P7300_WAIT_FIFO As Short = 2
            Public Const P7300_WAIT_BOTH As Short = 3

            'Terminator control
            Public Const P7300_TERM_OFF As Short = 0
            Public Const P7300_TERM_ON As Short = 1

            'DI control signals polarity for PCI-7300A Rev. B
            Public Const P7300_DIREQ_POS As Short = &H0S
            Public Const P7300_DIREQ_NEG As Short = &H1S
            Public Const P7300_DIACK_POS As Short = &H0S
            Public Const P7300_DIACK_NEG As Short = &H2S
            Public Const P7300_DITRIG_POS As Short = &H0S
            Public Const P7300_DITRIG_NEG As Short = &H4S

            'DO control signals polarity for PCI-7300A Rev. B
            Public Const P7300_DOREQ_POS As Short = &H0S
            Public Const P7300_DOREQ_NEG As Short = &H8S
            Public Const P7300_DOACK_POS As Short = &H0S
            Public Const P7300_DOACK_NEG As Short = &H10S
            Public Const P7300_DOTRIG_POS As Short = &H0S
            Public Const P7300_DOTRIG_NEG As Short = &H20S

            '--------- Constants for PCI-7432/7433/7434 --------------
            Public Const CHANNEL_DI_LOW As Short = 0
            Public Const CHANNEL_DI_HIGH As Short = 1
            Public Const CHANNEL_DO_LOW As Short = 0
            Public Const CHANNEL_DO_HIGH As Short = 1
            Public Const P7432R_DO_LED As Short = 1
            Public Const P7433R_DO_LED As Short = 0
            Public Const P7434R_DO_LED As Short = 2
            Public Const P7432R_DI_SLOT As Short = 1
            Public Const P7433R_DI_SLOT As Short = 2
            Public Const P7434R_DI_SLOT As Short = 0

            '----- Dual-Interrupt Source control for PCI-7248/49/96 & 7230 & 8554 & 7396 &7256/58 & 7260-----
            Public Const INT1_DISABLE As Short = -1 'INT1 Disabled
            Public Const INT1_COS As Short = 0 'INT1 COS : only available for PCI-7396, PCI-7256/58 & PCI-7260
            Public Const INT1_FP1C0 As Short = 1 'INT1 by Falling edge of P1C0
            Public Const INT1_RP1C0_FP1C3 As Short = 2 'INT1 by P1C0 Rising or P1C3 Falling
            Public Const INT1_EVENT_COUNTER As Short = 3 'INT1 by Event Counter down to zero
            Public Const INT1_EXT_SIGNAL As Short = 1 'INT1 by external signal : only available for PCI7432/PCI7433/PCI7230
            Public Const INT1_COUT12 As Short = 1 'INT1 COUT12 : only available for PCI8554
            Public Const INT1_CH0 As Short = 1 'INT1 CH0 : only available for PCI7256/58/60
            Public Const INT1_COS0 As Short = 1 'INT1 COS0 : only available for PCI-7452/PCI-7443
            Public Const INT1_COS1 As Short = 2 'INT1 COS1 : only available for PCI-7452/PCI-7443
            Public Const INT1_COS2 As Short = 3 'INT1 COS2 : only available for PCI-7452/PCI-7443
            Public Const INT1_COS3 As Short = 8 'INT1 COS3 : only available for PCI-7452/PCI-7443
            Public Const INT2_DISABLE As Short = -1 'INT2 Disabled
            Public Const INT2_COS As Short = 0 'INT2 COS : only available for PCI-7396
            Public Const INT2_FP2C0 As Short = 1 'INT2 by Falling edge of P2C0
            Public Const INT2_RP2C0_FP2C3 As Short = 2 'INT2 by P2C0 Rising or P2C3 Falling
            Public Const INT2_TIMER_COUNTER As Short = 3 'INT2 by Timer Counter down to zero
            Public Const INT2_EXT_SIGNAL As Short = 1 'INT2 by external signal : only available for PCI7432/PCI7433/PCI7230
            Public Const INT2_CH1 As Short = 2 'INT2 CH1 : only available for PCI7256/58/60
            Public Const INT2_WDT As Short = 4 'INT2 by WDT

            Public Const WDT_OVRFLOW_SAFETYOUT As Short = &H8000S 'enable safteyout while WDT overflow
            '-------- Constants for PCI-8554 --------------------
            'Clock Source of Cunter N
            Public Const ECKN As Short = 0
            Public Const COUTN_1 As Short = 1
            Public Const CK1 As Short = 2
            Public Const COUT10 As Short = 3

            'Clock Source of CK1
            Public Const CK1_C8M As Short = 0
            Public Const CK1_COUT11 As Short = 1

            'Debounce Clock
            Public Const DBCLK_COUT11 As Short = 0
            Public Const DBCLK_2MHZ As Short = 1

            '--------- Constants for PCI-9111 --------------
            'Dual Interrupt Mode
            Public Const P9111_INT1_EOC As Short = 0 'Ending of AD conversion
            Public Const P9111_INT1_FIFO_HF As Short = 1 'FIFO Half Full
            Public Const P9111_INT2_PACER As Short = 0 'Every Timer tick
            Public Const P9111_INT2_EXT_TRG As Short = 1 'ExtTrig High->Low

            'Channel Count
            Public Const P9111_CHANNEL_DO As Short = 0
            Public Const P9111_CHANNEL_EDO As Short = 1
            Public Const P9111_CHANNEL_DI As Short = 0
            Public Const P9111_CHANNEL_EDI As Short = 1

            'Trigger Mode
            Public Const P9111_TRGMOD_SOFT As Short = 0 'Software Trigger Mode
            Public Const P9111_TRGMOD_PRE As Short = 1 'Pre-Trigger Mode
            Public Const P9111_TRGMOD_POST As Short = 2 'Post Trigger Mode

            'EDO function
            Public Const P9111_EDO_INPUT As Short = 1 'EDO port set as Input port
            Public Const P9111_EDO_OUT_EDO As Short = 2 'EDO port set as Output port
            Public Const P9111_EDO_OUT_CHN As Short = 3 'EDO port set as channel number ouput port

            'AO Setting
            Public Const P9111_AO_UNIPOLAR As Short = 0
            Public Const P9111_AO_BIPOLAR As Short = 1

            '--------- Constants for PCI-9118 --------------
            Public Const P9118_AI_BiPolar As Short = &H0S
            Public Const P9118_AI_UniPolar As Short = &H1S

            Public Const P9118_AI_SingEnded As Short = &H0S
            Public Const P9118_AI_Differential As Short = &H2S

            Public Const P9118_AI_ExtG As Short = &H4S

            Public Const P9118_AI_ExtTrig As Short = &H8S

            Public Const P9118_AI_DtrgNegative As Short = &H0S
            Public Const P9118_AI_DtrgPositive As Short = &H10S

            Public Const P9118_AI_EtrgNegative As Short = &H0S
            Public Const P9118_AI_EtrgPositive As Short = &H20S

            Public Const P9118_AI_BurstModeEn As Short = &H40S
            Public Const P9118_AI_SampleHold As Short = &H80S
            Public Const P9118_AI_PostTrgEn As Short = &H100S
            Public Const P9118_AI_AboutTrgEn As Short = &H200S

            '--------- Constants for PCI-9812/9810 --------------
            'Channel Count
            Public Const P9116_AI_LocalGND As Short = &H0S
            Public Const P9116_AI_UserCMMD As Short = &H1S
            Public Const P9116_AI_SingEnded As Short = &H0S
            Public Const P9116_AI_Differential As Short = &H2S
            Public Const P9116_AI_BiPolar As Short = &H0S
            Public Const P9116_AI_UniPolar As Short = &H4S

            Public Const P9116_TRGMOD_SOFT As Short = &H0S 'Software Trigger Mode
            Public Const P9116_TRGMOD_POST As Short = &H10S 'Post Trigger Mode
            Public Const P9116_TRGMOD_DELAY As Short = &H20S 'Delay Trigger Mode
            Public Const P9116_TRGMOD_PRE As Short = &H30S 'Pre-Trigger Mode
            Public Const P9116_TRGMOD_MIDL As Short = &H40S 'Middle Trigger Mode
            Public Const P9116_AI_TrgPositive As Short = &H0S
            Public Const P9116_AI_TrgNegative As Short = &H80S
            Public Const P9116_AI_IntTimeBase As Short = &H0S
            Public Const P9116_AI_ExtTimeBase As Short = &H100S
            Public Const P9116_AI_DlyInSamples As Short = &H200S
            Public Const P9116_AI_DlyInTimebase As Short = &H0S
            Public Const P9116_AI_ReTrigEn As Short = &H400S
            Public Const P9116_AI_MCounterEn As Short = &H800S
            Public Const P9116_AI_SoftPolling As Short = &H0S
            Public Const P9116_AI_INT As Short = &H1000S
            Public Const P9116_AI_DMA As Short = &H2000S

            '--------- Constants for PCI-9812/9810 --------------
            'Channel Count
            Public Const P9812_CHANNEL_CNT1 As Short = 1
            Public Const P9812_CHANNEL_CNT2 As Short = 2
            Public Const P9812_CHANNEL_CNT4 As Short = 4

            'Trigger Mode
            Public Const P9812_TRGMOD_SOFT As Short = 0 'Software Trigger Mode
            Public Const P9812_TRGMOD_POST As Short = 1 'Post Trigger Mode
            Public Const P9812_TRGMOD_PRE As Short = 2 'Pre-Trigger Mode
            Public Const P9812_TRGMOD_DELAY As Short = 3 'Delay Trigger Mode
            Public Const P9812_TRGMOD_MIDL As Short = 4 'Middle Trigger Mode

            'Trigger Source
            Public Const P9812_TRGSRC_CH0 As Short = 0 'trigger source --CH0
            Public Const P9812_TRGSRC_CH1 As Short = 8 'trigger source --CH1
            Public Const P9812_TRGSRC_CH2 As Short = &H10S 'trigger source --CH2
            Public Const P9812_TRGSRC_CH3 As Short = &H18S 'trigger source --CH3
            Public Const P9812_TRGSRC_EXT_DIG As Short = &H20S 'External Digital Trigger

            'Trigger Polarity
            Public Const P9812_TRGSLP_POS As Short = 0 'Positive slope trigger
            Public Const P9812_TRGSLP_NEG As Short = &H40S 'Negative slope trigger

            'Frequency Selection
            Public Const P9812_AD2_GT_PCI As Short = &H80S 'Freq. of A/D clock > PCI clock freq.
            Public Const P9812_AD2_LT_PCI As Short = &H0S 'Freq. of A/D clock < PCI clock freq.

            'Clock Source
            Public Const P9812_CLKSRC_INT As Short = &H0S 'Internal clock
            Public Const P9812_CLKSRC_EXT_SIN As Short = &H100S 'External SIN wave clock
            Public Const P9812_CLKSRC_EXT_DIG As Short = &H200S 'External Square wave clock

            '-------- Constants for PCI-9221 --------------------
            'Input Type
            Public Const P9221_AI_SingEnded As Short = &H0
            Public Const P9221_AI_NonRef_SingEnded As Short = &H1
            Public Const P9221_AI_Differential As Short = &H2

            'Trigger Mode
            Public Const P9221_TRGMOD_SOFT As Short = &H0
            Public Const P9221_TRGMOD_ExtD As Short = &H8
            'Trigger Source
            Public Const P9221_TRGSRC_GPI0 As Short = &H0
            Public Const P9221_TRGSRC_GPI1 As Short = &H1
            Public Const P9221_TRGSRC_GPI2 As Short = &H2
            Public Const P9221_TRGSRC_GPI3 As Short = &H3
            Public Const P9221_TRGSRC_GPI4 As Short = &H4
            Public Const P9221_TRGSRC_GPI5 As Short = &H5
            Public Const P9221_TRGSRC_GPI6 As Short = &H6
            Public Const P9221_TRGSRC_GPI7 As Short = &H7

            'TimeBase Mode
            Public Const P9221_AI_IntTimeBase As Short = &H0
            Public Const P9221_AI_ExtTimeBase As Short = &H80
            'TimeBase Source
            Public Const P9221_TimeBaseSRC_GPI0 As Short = &H0
            Public Const P9221_TimeBaseSRC_GPI1 As Short = &H10
            Public Const P9221_TimeBaseSRC_GPI2 As Short = &H20
            Public Const P9221_TimeBaseSRC_GPI3 As Short = &H30
            Public Const P9221_TimeBaseSRC_GPI4 As Short = &H40
            Public Const P9221_TimeBaseSRC_GPI5 As Short = &H50
            Public Const P9221_TimeBaseSRC_GPI6 As Short = &H60
            Public Const P9221_TimeBaseSRC_GPI7 As Short = &H70

            'EMG shdn ctrl code
            Public Const EMGSHDN_OFF As Short = 0 'off
            Public Const EMGSHDN_ON As Short = 1 'on
            Public Const EMGSHDN_RECOVERY As Short = 2 'recovery

            'Hot Reset Hold ctrl code
            Public Const HRH_OFF As Short = 0 'off
            Public Const HRH_ON As Short = 1 'on

            '--------- Constants for Timer/Counter --------------
            'Counter Mode (8254)
            Public Const TOGGLE_OUTPUT As Short = 0 'Toggle output from low to high on terminal count
            Public Const PROG_ONE_SHOT As Short = 1 'Programmable one-shot
            Public Const RATE_GENERATOR As Short = 2 'Rate generator
            Public Const SQ_WAVE_RATE_GENERATOR As Short = 3 'Square wave rate generator
            Public Const SOFT_TRIG As Short = 4 'Software-triggered strobe
            Public Const HARD_TRIG As Short = 5 'Hardware-triggered strobe

            '------- General Purpose Timer/Counter -----------------
            'Counter Mode
            Public Const General_Counter As Short = &H0S 'general counter
            Public Const Pulse_Generation As Short = &H1S 'pulse generation
            'GPTC clock source
            Public Const GPTC_CLKSRC_EXT As Short = &H8S
            Public Const GPTC_CLKSRC_INT As Short = &H0S
            Public Const GPTC_GATESRC_EXT As Short = &H10S
            Public Const GPTC_GATESRC_INT As Short = &H0S
            Public Const GPTC_UPDOWN_SELECT_EXT As Short = &H20S
            Public Const GPTC_UPDOWN_SELECT_SOFT As Short = &H0S
            Public Const GPTC_UP_CTR As Short = &H40S
            Public Const GPTC_DOWN_CTR As Short = &H0S
            Public Const GPTC_ENABLE As Short = &H80S
            Public Const GPTC_DISABLE As Short = &H0S

            'General Purpose Timer/Counter for 9221
            'Counter Mode
            Public Const SimpleGatedEventCNT As Short = &H1
            Public Const SinglePeriodMSR As Short = &H2
            Public Const SinglePulseWidthMSR As Short = &H3
            Public Const SingleGatedPulseGen As Short = &H4
            Public Const SingleTrigPulseGen As Short = &H5
            Public Const RetrigSinglePulseGen As Short = &H6
            Public Const SingleTrigContPulseGen As Short = &H7
            Public Const ContGatedPulseGen As Short = &H8
            Public Const EdgeSeparationMSR As Short = &H9
            Public Const SingleTrigContPulseGenPWM As Short = &HA
            Public Const ContGatedPulseGenPWM As Short = &HB
            Public Const CW_CCW_Encoder As Short = &HC
            Public Const x1_AB_Phase_Encoder As Short = &HD
            Public Const x2_AB_Phase_Encoder As Short = &HE
            Public Const x4_AB_Phase_Encoder As Short = &HF
            Public Const Phase_Z As Short = &H10

            'GPTC clock source
            Public Const GPTC_CLK_SRC_Ext As Short = &H1
            Public Const GPTC_CLK_SRC_Int As Short = &H0
            Public Const GPTC_GATE_SRC_Ext As Short = &H2
            Public Const GPTC_GATE_SRC_Int As Short = &H0
            Public Const GPTC_UPDOWN_Ext As Short = &H4
            Public Const GPTC_UPDOWN_Int As Short = &H0

            'GPTC clock polarity
            Public Const GPTC_CLKSRC_LACTIVE As Short = &H1
            Public Const GPTC_CLKSRC_HACTIVE As Short = &H0
            Public Const GPTC_GATE_LACTIVE As Short = &H2
            Public Const GPTC_GATE_HACTIVE As Short = &H0
            Public Const GPTC_UPDOWN_LACTIVE As Short = &H4
            Public Const GPTC_UPDOWN_HACTIVE As Short = &H0
            Public Const GPTC_OUTPUT_LACTIVE As Short = &H8
            Public Const GPTC_OUTPUT_HACTIVE As Short = &H0

            Public Const IntGate As Short = &H0
            Public Const IntUpDnCTR As Short = &H1
            Public Const IntENABLE As Short = &H2

            Public Const GPTC_EZ0_ClearPhase0 As Short = &H0
            Public Const GPTC_EZ0_ClearPhase1 As Short = &H1
            Public Const GPTC_EZ0_ClearPhase2 As Short = &H2
            Public Const GPTC_EZ0_ClearPhase3 As Short = &H3

            Public Const GPTC_EZ0_ClearMode0 As Short = &H0
            Public Const GPTC_EZ0_ClearMode1 As Short = &H1
            Public Const GPTC_EZ0_ClearMode2 As Short = &H2

            'Watchdog Timer
            'Counter action
            Public Const WDT_DISARM As Short = 0
            Public Const WDT_ARM As Short = 1
            Public Const WDT_RESTART As Short = 2

            'Pattern ID
            Public Const INIT_PTN As Short = 0
            Public Const EMGSHDN_PTN As Short = 1

            'Pattern ID for 7442/7444
            Public Const INIT_PTN_CH0 As Short = 0
            Public Const INIT_PTN_CH1 As Short = 1
            Public Const INIT_PTN_CH2 As Short = 2 'only for 7444
            Public Const INIT_PTN_CH3 As Short = 3 'only for 7444
            Public Const SAFTOUT_PTN_CH0 As Short = 4
            Public Const SAFTOUT_PTN_CH1 As Short = 5
            Public Const SAFTOUT_PTN_CH2 As Short = 6 'only for 7444
            Public Const SAFTOUT_PTN_CH3 As Short = 7 'only for 7444	

            '16-bit binary or 4-decade BCD counter
            Public Const BIN As Short = 0
            Public Const BCD As Short = 1

            'DAQ Event type for the event message
            Public Const AIEnd As Short = 0
            Public Const DIEnd As Short = 0
            Public Const DOEnd As Short = 0
            Public Const DBEvent As Short = 1

            'EEPROM
            Public Const EEPROM_DEFAULT_BANK As Short = 0
            Public Const EEPROM_USER_BANK1 As Short = 1

            Declare Function WaitForSingleObject Lib "kernel32" (ByVal hHandle As Integer, ByVal dwMilliseconds As Integer) As Integer
            Public Delegate Sub CallbackDelegate()

            '-------------------------------------------------------------------
            '  PCIS-DASK Function prototype
            '-----------------------------------------------------------------*/
            Declare Function Register_Card Lib "Pci-Dask.dll" (ByVal cardType As Short, ByVal card_num As Short) As Short
            Declare Function Release_Card Lib "Pci-Dask.dll" (ByVal CardNumber As Short) As Short
            Declare Function GetActualRate Lib "Pci-Dask.dll" (ByVal CardNumber As Short, ByVal SampleRate As Double, ByRef ActualRate As Double) As Short
            Declare Function GetCardType Lib "Pci-Dask.dll" (ByVal CardNumber As Short, ByRef cardType As Short) As Short
            Declare Function GetBaseAddr Lib "Pci-Dask.dll" (ByVal CardNumber As Short, ByRef BaseAddr As Integer, ByRef BaseAddr2 As Integer) As Short
            Declare Function GetLCRAddr Lib "Pci-Dask.dll" (ByVal CardNumber As Short, ByRef LcrAddr As Integer) As Short
            Declare Function GetCardIndexFromID Lib "Pci-Dask.dll" (ByVal CardNumber As Short, ByRef cardType As Short, ByRef cardIndex As Short) As Short
            Declare Function EMGShutDownControl Lib "Pci-Dask.dll" (ByVal CardNumber As Short, ByVal ctrl As Byte) As Short
            Declare Function EMGShutDownStatus Lib "Pci-Dask.dll" (ByVal CardNumber As Short, ByRef ctrl As Byte) As Short
            Declare Function HotResetHoldControl Lib "Pci-Dask.dll" (ByVal wCardNumber As Short, ByVal Enable As Byte) As Short
            Declare Function HotResetHoldStatus Lib "Pci-Dask.dll" (ByVal wCardNumber As Short, ByRef sts As Byte) As Short
            Declare Function SetInitPattern Lib "Pci-Dask.dll" (ByVal wCardNumber As Short, ByVal patID As Byte, ByVal pattern As Integer) As Short
            Declare Function GetInitPattern Lib "Pci-Dask.dll" (ByVal CardNumber As Short, ByVal patID As Byte, ByRef pattern As Integer) As Short
            Declare Function IdentifyLED_Control Lib "Pci-Dask.dll" (ByVal CardNumber As Short, ByVal ctrl As Byte) As Short

            'AI Functions
            Declare Function AI_9111_Config Lib "Pci-Dask.dll" (ByVal CardNumber As Short, ByVal TrigSource As Short, ByVal TrgMode As Short, ByVal wTraceCnt As Short) As Short
            Declare Function AI_9112_Config Lib "Pci-Dask.dll" (ByVal CardNumber As Short, ByVal TrigSource As Short) As Short
            Declare Function AI_9113_Config Lib "Pci-Dask.dll" (ByVal CardNumber As Short, ByVal TrigSource As Short) As Short
            Declare Function AI_9114_Config Lib "Pci-Dask.dll" (ByVal CardNumber As Short, ByVal TrigSource As Short) As Short
            Declare Function AI_9114_PreTrigConfig Lib "Pci-Dask.dll" (ByVal CardNumber As Short, ByVal PreTrgEn As Short, ByVal TraceCnt As Short) As Short
            Declare Function AI_9116_Config Lib "Pci-Dask.dll" (ByVal CardNumber As Short, ByVal ConfigCtrl As Short, ByVal TrigCtrl As Short, ByVal PostCnt As Short, ByVal MCnt As Short, ByVal ReTrgCnt As Short) As Short
            Declare Function AI_9118_Config Lib "Pci-Dask.dll" (ByVal CardNumber As Short, ByVal wModeCtrl As Short, ByVal wFunCtrl As Short, ByVal wBurstCnt As Short, ByVal wPostCnt As Short) As Short
            Declare Function AI_9221_Config Lib "Pci-Dask.dll" (ByVal CardNumber As Short, ByVal ConfigCtrl As Short, ByVal TrigCtrl As Short, ByVal ByValAutoResetBuf As Byte)
            Declare Function AI_9812_Config Lib "Pci-Dask.dll" (ByVal CardNumber As Short, ByVal wTrgMode As Short, ByVal wTrgSrc As Short, ByVal wTrgPol As Short, ByVal wClkSel As Short, ByVal wTrgLevel As Short, ByVal wPostCnt As Short) As Short
            Declare Function AI_9116_CounterInterval Lib "Pci-Dask.dll" (ByVal CardNumber As Short, ByVal ScanIntrv As Integer, ByVal SampIntrv As Integer) As Short
            Declare Function AI_9221_CounterInterval Lib "Pci-Dask.dll" (ByVal CardNumber As Short, ByVal ScanIntrv As Integer, ByVal SampIntrv As Integer) As Short
            Declare Function AI_9812_SetDiv Lib "Pci-Dask.dll" (ByVal CardNumber As Short, ByVal pacerVal As Integer) As Short
            Declare Function AI_AsyncCheck Lib "Pci-Dask.dll" (ByVal CardNumber As Short, ByRef Stopped As Byte, ByRef AccessCnt As Integer) As Short
            Declare Function AI_AsyncClear Lib "Pci-Dask.dll" (ByVal CardNumber As Short, ByRef AccessCnt As Integer) As Short
            Declare Function AI_AsyncDblBufferHalfReady Lib "Pci-Dask.dll" (ByVal CardNumber As Short, ByRef HalfReady As Byte, ByRef StopFlag As Byte) As Short
            Declare Function AI_AsyncDblBufferMode Lib "Pci-Dask.dll" (ByVal CardNumber As Short, ByVal Enable As Byte) As Short
            Declare Function AI_AsyncDblBufferTransfer Lib "Pci-Dask.dll" (ByVal CardNumber As Short, ByRef Buffer As Short) As Short
            Declare Function AI_AsyncDblBufferHandled Lib "Pci-Dask.dll" (ByVal CardNumber As Integer) As Integer
            Declare Function AI_AsyncDblBufferToFile Lib "Pci-Dask.dll" (ByVal CardNumber As Integer) As Integer
            Declare Function AI_ContReadChannel Lib "Pci-Dask.dll" (ByVal CardNumber As Short, ByVal Channel As Short, ByVal AdRange As Short, ByRef Buffer As Short, ByVal ReadCount As Integer, ByVal SampleRate As Double, ByVal SyncMode As Short) As Short
            Declare Function AI_ContScanChannels Lib "Pci-Dask.dll" (ByVal wCardNumber As Short, ByVal wChannel As Short, ByVal wAdRange As Short, ByRef pwBuffer As Short, ByVal dwReadCount As Integer, ByVal SampleRate As Double, ByVal SyncMode As Short) As Short
            Declare Function AI_ContReadMultiChannels Lib "Pci-Dask.dll" (ByVal CardNumber As Short, ByVal NumChans As Short, ByRef chans As Short, ByRef AdRanges As Short, ByRef Buffer As Short, ByVal ReadCount As Integer, ByVal SampleRate As Double, ByVal SyncMode As Short) As Short
            Declare Function AI_ContReadChannelToFile Lib "Pci-Dask.dll" (ByVal CardNumber As Short, ByVal Channel As Short, ByVal AdRange As Short, ByVal FileName As String, ByVal ReadCount As Integer, ByVal SampleRate As Double, ByVal SyncMode As Short) As Short
            Declare Function AI_ContScanChannelsToFile Lib "Pci-Dask.dll" (ByVal wCardNumber As Short, ByVal wChannel As Short, ByVal wAdRange As Short, ByVal FileName As String, ByVal dwReadCount As Integer, ByVal SampleRate As Double, ByVal SyncMode As Short) As Short
            Declare Function AI_ContReadMultiChannelsToFile Lib "Pci-Dask.dll" (ByVal CardNumber As Short, ByVal NumChans As Short, ByRef chans As Short, ByRef AdRanges As Short, ByVal FileName As String, ByVal ReadCount As Integer, ByVal SampleRate As Double, ByVal SyncMode As Short) As Short
            Declare Function AI_ContStatus Lib "Pci-Dask.dll" (ByVal CardNumber As Short, ByRef Status As Short) As Short
            Declare Function AI_InitialMemoryAllocated Lib "Pci-Dask.dll" (ByVal CardNumber As Short, ByRef MemSize As Integer) As Short
            Declare Function AI_ReadChannel Lib "Pci-Dask.dll" (ByVal CardNumber As Short, ByVal Channel As Short, ByVal AdRange As Short, ByRef Value As Short) As Short
            Declare Function AI_VReadChannel Lib "Pci-Dask.dll" (ByVal CardNumber As Short, ByVal Channel As Short, ByVal AdRange As Short, ByRef Voltage As Double) As Short
            Declare Function AI_ScanReadChannels Lib "Pci-Dask.dll" (ByVal CardNumber As Integer, ByVal Channel As Integer, ByVal AdRange As Integer, ByVal pwBuffer As Integer) As Integer
            Declare Function AI_ReadMultiChannels Lib "Pci-Dask.dll" (ByVal CardNumber As Integer, ByVal NumChans As Integer, ByVal pwChans As Integer, ByVal pwAdRanges As Integer, ByVal pwBuffer As Integer) As Integer
            Declare Function AI_VoltScale Lib "Pci-Dask.dll" (ByVal CardNumber As Short, ByVal AdRange As Short, ByVal reading As Short, ByRef Voltage As Double) As Short
            Declare Function AI_ContVScale Lib "Pci-Dask.dll" (ByVal CardNumber As Short, ByVal AdRange As Short, ByRef readingArray As UShort, ByRef voltageArray As Double, ByVal Count As Integer) As Short
            Declare Function AI_AsyncDblBufferOverrun Lib "Pci-Dask.dll" (ByVal CardNumber As Short, ByVal op As Short, ByRef overrunFlag As Short) As Short
            Declare Function AI_EventCallBack Lib "Pci-Dask.dll" (ByVal CardNumber As Short, ByVal Mode As Short, ByVal EventType As Short, ByVal callbackAddr As CallbackDelegate) As Short
            Declare Function AI_SetTimeOut Lib "Pci-Dask.dll" (ByVal CardNumber As Integer, ByVal dwTimeOut As Long) As Integer
            Declare Function AI_ContBufferReset Lib "Pci-Dask.dll" (ByVal CardNumber As Integer) As Integer
            Declare Function AI_ContBufferSetup Lib "Pci-Dask.dll" (ByVal CardNumber As Integer, ByVal pwBuffer As Short, ByVal ReadCount As Long, ByVal BufferId As Integer) As Integer

            'AO Functions
            Declare Function AO_6208A_Config Lib "Pci-Dask.dll" (ByVal CardNumber As Short, ByVal V2AMode As Short) As Short
            Declare Function AO_6308A_Config Lib "Pci-Dask.dll" (ByVal CardNumber As Short, ByVal V2AMode As Short) As Short
            Declare Function AO_6308V_Config Lib "Pci-Dask.dll" (ByVal CardNumber As Short, ByVal Channel As Short, ByVal OutputPolarity As Short, ByVal refVoltage As Double) As Short
            Declare Function AO_9111_Config Lib "Pci-Dask.dll" (ByVal CardNumber As Short, ByVal OutputPolarity As Short) As Short
            Declare Function AO_9112_Config Lib "Pci-Dask.dll" (ByVal CardNumber As Short, ByVal Channel As Short, ByVal refVoltage As Double) As Short
            Declare Function AO_WriteChannel Lib "Pci-Dask.dll" (ByVal CardNumber As Short, ByVal Channel As Short, ByVal Value As Short) As Short
            Declare Function AO_VWriteChannel Lib "Pci-Dask.dll" (ByVal CardNumber As Short, ByVal Channel As Short, ByVal Voltage As Double) As Short
            Declare Function AO_VoltScale Lib "Pci-Dask.dll" (ByVal CardNumber As Short, ByVal Channel As Short, ByVal Voltage As Double, ByRef binValue As Short) As Short
            Declare Function AO_SimuWriteChannel Lib "Pci-Dask.dll" (ByVal CardNumber As Short, ByVal wGroup As Short, ByRef valueArray As Short) As Short
            Declare Function AO_SimuVWriteChannel Lib "Pci-Dask.dll" (ByVal CardNumber As Short, ByVal wGroup As Short, ByRef voltageArray As Double) As Short

            'DI Functions
            Declare Function DI_7200_Config Lib "Pci-Dask.dll" (ByVal CardNumber As Short, ByVal TrigSource As Short, ByVal wExtTrigEn As Short, ByVal wTrigPol As Short, ByVal wI_REQ_Pol As Short) As Short
            Declare Function DI_7300A_Config Lib "Pci-Dask.dll" (ByVal CardNumber As Short, ByVal PortWidth As Short, ByVal TrigSource As Short, ByVal WaitStatus As Short, ByVal Terminaor As Short, ByVal I_REQ_Pol As Short, ByVal clear_fifo As Byte, ByVal disable_di As Byte) As Short
            Declare Function DI_7300B_Config Lib "Pci-Dask.dll" (ByVal CardNumber As Short, ByVal PortWidth As Short, ByVal TrigSource As Short, ByVal WaitStatus As Short, ByVal Terminator As Short, ByVal I_Cntrl_Pol As Short, ByVal clear_fifo As Byte, ByVal disable_di As Byte) As Short
            Declare Function DI_AsyncCheck Lib "Pci-Dask.dll" (ByVal CardNumber As Short, ByRef Stopped As Byte, ByRef AccessCnt As Integer) As Short
            Declare Function DI_AsyncClear Lib "Pci-Dask.dll" (ByVal CardNumber As Short, ByRef AccessCnt As Integer) As Short
            Declare Function DI_AsyncDblBufferHalfReady Lib "Pci-Dask.dll" (ByVal CardNumber As Short, ByRef HalfReady As Byte) As Short
            Declare Function DI_AsyncDblBufferMode Lib "Pci-Dask.dll" (ByVal CardNumber As Short, ByVal Enable As Byte) As Short
            Declare Function DI_AsyncDblBufferTransfer Lib "Pci-Dask.dll" (ByVal CardNumber As Short, ByRef Buffer As Byte) As Short
            Declare Function DI_AsyncDblBufferTransfer Lib "Pci-Dask.dll" (ByVal CardNumber As Short, ByRef Buffer As Short) As Short
            Declare Function DI_AsyncDblBufferTransfer Lib "Pci-Dask.dll" (ByVal CardNumber As Short, ByRef Buffer As UInt32) As Short
            Declare Function DI_ContMultiBufferSetup Lib "Pci-Dask.dll" (ByVal CardNumber As Short, ByRef Buffer As Byte, ByVal ReadCount As Integer, ByRef BufferId As Short) As Short
            Declare Function DI_ContMultiBufferSetup Lib "Pci-Dask.dll" (ByVal CardNumber As Short, ByRef Buffer As Short, ByVal ReadCount As Integer, ByRef BufferId As Short) As Short
            Declare Function DI_ContMultiBufferSetup Lib "Pci-Dask.dll" (ByVal CardNumber As Short, ByRef Buffer As UInt32, ByVal ReadCount As Integer, ByRef BufferId As Short) As Short
            Declare Function DI_ContMultiBufferStart Lib "Pci-Dask.dll" (ByVal CardNumber As Short, ByVal Port As Short, ByVal SampleRate As Double) As Short
            Declare Function DI_AsyncMultiBufferNextReady Lib "Pci-Dask.dll" (ByVal CardNumber As Short, ByRef NextReady As Byte, ByRef BufferId As Short) As Short
            Declare Function DI_ContReadPort Lib "Pci-Dask.dll" (ByVal CardNumber As Short, ByVal Port As Integer, ByRef Buffer As Byte, ByVal ReadCount As Integer, ByVal SampleRate As Double, ByVal SyncMode As Short) As Short
            Declare Function DI_ContReadPort Lib "Pci-Dask.dll" (ByVal CardNumber As Short, ByVal Port As Integer, ByRef Buffer As UShort, ByVal ReadCount As Integer, ByVal SampleRate As Double, ByVal SyncMode As Short) As Short
            Declare Function DI_ContReadPort Lib "Pci-Dask.dll" (ByVal CardNumber As Short, ByVal Port As Integer, ByRef Buffer As UInt32, ByVal ReadCount As Integer, ByVal SampleRate As Double, ByVal SyncMode As Short) As Short
            Declare Function DI_ContReadPortToFile Lib "Pci-Dask.dll" (ByVal CardNumber As Short, ByVal Port As Short, ByVal FileName As String, ByVal ReadCount As Integer, ByVal SampleRate As Double, ByVal SyncMode As Short) As Short
            Declare Function DI_ContStatus Lib "Pci-Dask.dll" (ByVal CardNumber As Short, ByRef Status As Short) As Short
            Declare Function DI_InitialMemoryAllocated Lib "Pci-Dask.dll" (ByVal CardNumber As Short, ByRef MemSize As Integer) As Short
            Declare Function DI_ReadPort Lib "Pci-Dask.dll" (ByVal CardNumber As Short, ByVal Port As Short, ByRef Value As Integer) As Short
            Declare Function DI_ReadLine Lib "Pci-Dask.dll" (ByVal CardNumber As Short, ByVal Port As Short, ByVal Line As Short, ByRef Value As Short) As Short
            Declare Function DI_AsyncDblBufferOverrun Lib "Pci-Dask.dll" (ByVal CardNumber As Short, ByVal op As Short, ByRef overrunFlag As Short) As Short
            Declare Function DI_EventCallBack Lib "Pci-Dask.dll" (ByVal CardNumber As Short, ByVal Mode As Short, ByVal EventType As Short, ByVal callbackAddr As Integer) As Short

            'DO Functions
            Declare Function DO_7200_Config Lib "Pci-Dask.dll" (ByVal CardNumber As Short, ByVal TrigSource As Short, ByVal wOutReqEn As Short, ByVal wOutTrigSig As Short) As Short
            Declare Function DO_7300A_Config Lib "Pci-Dask.dll" (ByVal CardNumber As Short, ByVal PortWidth As Short, ByVal TrigSource As Short, ByVal WaitStatus As Short, ByVal Terminaor As Short, ByVal O_REQ_Pol As Short) As Short
            Declare Function DO_7300B_Config Lib "Pci-Dask.dll" (ByVal CardNumber As Short, ByVal PortWidth As Short, ByVal TrigSource As Short, ByVal WaitStatus As Short, ByVal Terminator As Short, ByVal O_Cntrl_Pol As Short, ByVal FifoThreshold As Integer) As Short
            Declare Function DO_AsyncCheck Lib "Pci-Dask.dll" (ByVal CardNumber As Short, ByRef Stopped As Byte, ByRef AccessCnt As Integer) As Short
            Declare Function DO_AsyncClear Lib "Pci-Dask.dll" (ByVal CardNumber As Short, ByRef AccessCnt As Integer) As Short
            Declare Function DO_ContWritePort Lib "Pci-Dask.dll" (ByVal CardNumber As Short, ByVal Port As Integer, ByRef Buffer As Byte, ByVal WriteCount As Integer, ByVal Iterations As Integer, ByVal SampleRate As Double, ByVal SyncMode As Short) As Short
            Declare Function DO_ContWritePort Lib "Pci-Dask.dll" (ByVal CardNumber As Short, ByVal Port As Integer, ByRef Buffer As UShort, ByVal WriteCount As Integer, ByVal Iterations As Integer, ByVal SampleRate As Double, ByVal SyncMode As Short) As Short
            Declare Function DO_ContWritePort Lib "Pci-Dask.dll" (ByVal CardNumber As Short, ByVal Port As Integer, ByRef Buffer As UInt32, ByVal WriteCount As Integer, ByVal Iterations As Integer, ByVal SampleRate As Double, ByVal SyncMode As Short) As Short
            Declare Function DO_ContStatus Lib "Pci-Dask.dll" (ByVal CardNumber As Short, ByRef Status As Short) As Short
            Declare Function DO_InitialMemoryAllocated Lib "Pci-Dask.dll" (ByVal CardNumber As Short, ByRef MemSize As Integer) As Short
            Declare Function DO_PGStart Lib "Pci-Dask.dll" (ByVal CardNumber As Short, ByRef Buffer As Byte, ByVal WriteCount As Integer, ByVal SampleRate As Double) As Short
            Declare Function DO_PGStart Lib "Pci-Dask.dll" (ByVal CardNumber As Short, ByRef Buffer As Short, ByVal WriteCount As Integer, ByVal SampleRate As Double) As Short
            Declare Function DO_PGStart Lib "Pci-Dask.dll" (ByVal CardNumber As Short, ByRef Buffer As UInt32, ByVal WriteCount As Integer, ByVal SampleRate As Double) As Short
            Declare Function DO_PGStop Lib "Pci-Dask.dll" (ByVal CardNumber As Short) As Short
            Declare Function DO_WritePort Lib "Pci-Dask.dll" (ByVal CardNumber As Short, ByVal Port As Short, ByVal Value As Integer) As Short
            Declare Function DO_WriteLine Lib "Pci-Dask.dll" (ByVal CardNumber As Short, ByVal Port As Short, ByVal Line As Short, ByVal Value As Short) As Short
            Declare Function DO_SimuWritePort Lib "Pci-Dask.dll" (ByVal wCardNumber As Short, ByVal wNumChans As Short, ByRef pdwBuffer As Integer) As Short
            Declare Function DO_ReadLine Lib "Pci-Dask.dll" (ByVal CardNumber As Short, ByVal Port As Short, ByVal Line As Short, ByRef Value As Short) As Short
            Declare Function DO_ReadPort Lib "Pci-Dask.dll" (ByVal CardNumber As Short, ByVal Port As Short, ByRef Value As Integer) As Short
            Declare Function EDO_9111_Config Lib "Pci-Dask.dll" (ByVal CardNumber As Short, ByVal wEDO_Fun As Short) As Short
            Declare Function DO_WriteExtTrigLine Lib "Pci-Dask.dll" (ByVal CardNumber As Short, ByVal Value As Short) As Short
            Declare Function DO_ContMultiBufferSetup Lib "Pci-Dask.dll" (ByVal CardNumber As Short, ByRef Buffer As Byte, ByVal WriteCount As Integer, ByRef BufferId As Short) As Short
            Declare Function DO_ContMultiBufferSetup Lib "Pci-Dask.dll" (ByVal CardNumber As Short, ByRef Buffer As Short, ByVal WriteCount As Integer, ByRef BufferId As Short) As Short
            Declare Function DO_ContMultiBufferSetup Lib "Pci-Dask.dll" (ByVal CardNumber As Short, ByRef Buffer As UInt32, ByVal WriteCount As Integer, ByRef BufferId As Short) As Short
            Declare Function DO_ContMultiBufferStart Lib "Pci-Dask.dll" (ByVal CardNumber As Short, ByVal Port As Short, ByVal SampleRate As Double) As Short
            Declare Function DO_AsyncMultiBufferNextReady Lib "Pci-Dask.dll" (ByVal CardNumber As Short, ByRef NextReady As Byte, ByRef BufferId As Short) As Short
            Declare Function DO_EventCallBack Lib "Pci-Dask.dll" (ByVal CardNumber As Short, ByVal Mode As Short, ByVal EventType As Short, ByVal callbackAddr As Integer) As Short

            'DIO Functions
            Declare Function DIO_PortConfig Lib "Pci-Dask.dll" (ByVal CardNumber As Short, ByVal Port As Short, ByVal Direction As Short) As Short
            Declare Function DIO_LinesConfig Lib "Pci-Dask.dll" (ByVal wCardNumber As Short, ByVal wPort As Short, ByVal wLinesdirmap As Short) As Short
            Declare Function DIO_LineConfig Lib "Pci-Dask.dll" (ByVal wCardNumber As Short, ByVal wPort As Short, ByVal wLine As Short, ByVal wDirection As Short) As Short
            Declare Function DIO_SetDualInterrupt Lib "Pci-Dask.dll" (ByVal wCardNumber As Short, ByVal wInt1Mode As Short, ByVal wInt2Mode As Short, ByRef hEvent As Integer) As Short
            Declare Function DIO_SetCOSInterrupt Lib "Pci-Dask.dll" (ByVal wCardNumber As Short, ByVal Port As Short, ByVal ctlA As Short, ByVal ctlB As Short, ByVal ctlC As Short) As Short
            Declare Function DIO_GetCOSLatchData Lib "Pci-Dask.dll" (ByVal CardNumber As Short, ByRef CosLData As Integer) As Short
            Declare Function DIO_SetCOSInterrupt32 Lib "Pci-Dask.dll" (ByVal wCardNumber As Short, ByVal Port As Byte, ByVal ctl As Integer, ByRef hEvent As Integer, ByVal bManualReset As Byte) As Short
            Declare Function DIO_GetCOSLatchData32 Lib "Pci-Dask.dll" (ByVal CardNumber As Short, ByRef CosLData As Integer) As Short
            Declare Function DIO_INT_EventMessage Lib "Pci-Dask.dll" (ByVal CardNumber As Short, ByVal Mode As Short, ByVal hEvent As Integer, ByVal windowHandle As Integer, ByVal message As Integer, ByVal callbackAddr As CallbackDelegate) As Short
            Declare Function DIO_INT1_EventMessage Lib "Pci-Dask.dll" (ByVal CardNumber As Short, ByVal Int1Mode As Short, ByVal windowHandle As Integer, ByVal message As Integer, ByVal callbackAddr As CallbackDelegate) As Short
            Declare Function DIO_INT2_EventMessage Lib "Pci-Dask.dll" (ByVal CardNumber As Short, ByVal Int2Mode As Short, ByVal windowHandle As Integer, ByVal message As Integer, ByVal callbackAddr As CallbackDelegate) As Short
            Declare Function DIO_7300SetInterrupt Lib "Pci-Dask.dll" (ByVal wCardNumber As Short, ByVal AuxDIEn As Short, ByVal T2En As Short, ByRef hEvent As Integer) As Short
            Declare Function DIO_AUXDI_EventMessage Lib "Pci-Dask.dll" (ByVal CardNumber As Short, ByVal AuxDIEn As Short, ByVal windowHandle As Integer, ByVal message As Integer, ByVal callbackAddr As CallbackDelegate) As Short
            Declare Function DIO_T2_EventMessage Lib "Pci-Dask.dll" (ByVal CardNumber As Short, ByVal T2En As Short, ByVal windowHandle As Integer, ByVal message As Integer, ByVal callbackAddr As CallbackDelegate) As Short

            'Counter Functions
            Declare Function CTR_Setup Lib "Pci-Dask.dll" (ByVal CardNumber As Short, ByVal Ctr As Short, ByVal Mode As Short, ByVal Count As Integer, ByVal BinBcd As Short) As Short
            Declare Function CTR_Clear Lib "Pci-Dask.dll" (ByVal CardNumber As Short, ByVal Ctr As Short, ByVal State As Short) As Short
            Declare Function CTR_Read Lib "Pci-Dask.dll" (ByVal CardNumber As Short, ByVal Ctr As Short, ByRef Value As Integer) As Short
            Declare Function CTR_Update Lib "Pci-Dask.dll" (ByVal CardNumber As Short, ByVal Ctr As Short, ByVal Count As Integer) As Short
            Declare Function CTR_8554_ClkSrc_Config Lib "Pci-Dask.dll" (ByVal CardNumber As Short, ByVal Ctr As Short, ByVal ClockSource As Short) As Short
            Declare Function CTR_8554_CK1_Config Lib "Pci-Dask.dll" (ByVal CardNumber As Short, ByVal ClockSource As Short) As Short
            Declare Function CTR_8554_Debounce_Config Lib "Pci-Dask.dll" (ByVal CardNumber As Short, ByVal DebounceClock As Short) As Short
            Declare Function GCTR_Setup Lib "Pci-Dask.dll" (ByVal CardNumber As Short, ByVal wGCtr As Short, ByVal wGCtrCtrl As Short, ByVal dwCount As Integer) As Short
            Declare Function GCTR_Clear Lib "Pci-Dask.dll" (ByVal CardNumber As Short, ByVal wGCtr As Short) As Short
            Declare Function GCTR_Read Lib "Pci-Dask.dll" (ByVal CardNumber As Short, ByVal wGCtr As Short, ByRef pValue As Integer) As Short
            Declare Function GPTC_Clear Lib "Pci-Dask.dll" (ByVal CardNumber As Integer, ByVal wGCtr As Integer) As Integer
            Declare Function GPTC_Control Lib "Pci-Dask.dll" (ByVal CardNumber As Integer, ByVal wGCtr As Integer, ByVal ParamID As Integer, ByVal Value As Integer) As Integer
            Declare Function GPTC_Read Lib "Pci-Dask.dll" (ByVal CardNumber As Integer, ByVal wGCtr As Integer, ByVal pValue As Long) As Integer
            Declare Function GPTC_Setup Lib "Pci-Dask.dll" (ByVal CardNumber As Integer, ByVal wGCtr As Integer, ByVal wMode As Integer, ByVal wSrcCtrl As Integer, ByVal wPolCtrl As Integer, ByVal LReg1_Val As Long, ByVal LReg2_Val As Long) As Integer
            Declare Function GPTC_Status Lib "Pci-Dask.dll" (ByVal CardNumber As Integer, ByVal wGCtr As Integer, ByVal pValue As Long) As Integer
            Declare Function WDT_Setup Lib "Pci-Dask.dll" (ByVal CardNumber As Short, ByVal wCtr As Short, ByVal ovflowSec As Single, ByRef actualSec As Single, ByRef hEvent As Integer) As Short
            Declare Function WDT_Control Lib "Pci-Dask.dll" (ByVal CardNumber As Short, ByVal wCtr As Short, ByVal action As Short) As Short
            Declare Function WDT_Status Lib "Pci-Dask.dll" (ByVal CardNumber As Short, ByVal wCtr As Short, ByRef pValue As Integer) As Short
            Declare Function WDT_Reload Lib "Pci-Dask.dll" (ByVal wCardNumber As Short, ByVal ovflowSec As Single, ByRef actualSec As Single) As Short

            Declare Function AI_GetEvent Lib "Pci-Dask.dll" (ByVal CardNumber As Short, ByRef hEvent As Integer) As Short
            Declare Function AO_GetEvent Lib "Pci-Dask.dll" (ByVal CardNumber As Short, ByRef hEvent As Integer) As Short
            Declare Function DI_GetEvent Lib "Pci-Dask.dll" (ByVal CardNumber As Short, ByRef hEvent As Integer) As Short
            Declare Function DO_GetEvent Lib "Pci-Dask.dll" (ByVal CardNumber As Short, ByRef hEvent As Integer) As Short

            'Cal Function
            Declare Function PCI_DB_Auto_Calibration_ALL Lib "Pci-Dask.dll" (ByVal CardNumber As Integer) As Integer
            Declare Function PCI_Load_CAL_Data Lib "Pci-Dask.dll" (ByVal CardNumber As Integer, ByVal bank As Integer) As Integer
            Declare Function PCI_EEPROM_CAL_Constant_Update Lib "Pci-Dask.dll" (ByVal CardNumber As Integer, ByVal bank As Integer) As Integer
        End Class
    End Class

    Public Class SQL_LIB_A001

        Public DataBase_Data_Souce As String = "192.168.117.253"
        Public DataBase_ID As String = "sfs"
        Public DataBase_PassWord As String = "Hippih.*"
        Public DataBase_Catalog_Name As String = "X17"

        Dim DataBase_Link_Boolean As Boolean
        Dim Soft_Default_Path As String = IO.Directory.GetCurrentDirectory & "\"
        Dim DataBase_Connection As New SqlConnection
        Dim Sw As IO.StreamWriter
        Dim SqlServerName As String
        Dim InitialCatalogName As String

        Sub Write_Log(ByVal Message As String)
            Dim File_Path As String = Nothing
            Dim MON As String = Date.Now.Month.ToString.PadLeft(2).Replace(" ", "0") & " MONTH\"
            Dim DAY As String = Date.Now.Day.ToString.PadLeft(2).Replace(" ", "0") & " DAY\"
            File_Path = "C:\" & MON & DAY
            If System.IO.Directory.Exists(File_Path) = False Then
                System.IO.Directory.CreateDirectory(File_Path)
            End If
            'MessageBox.Show(Message & vbCrLf & "以上错误如自行不能解决请联系软件工程师！", "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
            Sw = New IO.StreamWriter(File_Path & "SystemLog.txt", True, Encoding.Default)
            Sw.WriteLine(Date.Now & "," & Message)
            Sw.Flush()
            Sw.Close()
            Sw.Dispose()
        End Sub

        ''' <summary>
        ''' 初始化数据库
        ''' </summary>
        ''' <param name="Server_ADD">数据库地址</param>
        ''' <param name="Use_ID">数据库用户名</param>
        ''' <param name="Pass_Word">数据库密码，缺省无密码</param>
        ''' <param name="Initial_Catalog_Name">数据库名称</param>
        ''' <returns>返回值：TRUE（初始化成功），FALSE（初始化失败）</returns>
        ''' <param name="TimeOut">超时</param>
        ''' <remarks></remarks>
        Function DataBase_Initialization(Optional ByVal Server_Add As String = "127.0.0.1", Optional ByVal Use_ID As String = "SA", Optional ByVal Pass_Word As String = Nothing, Optional ByVal Initial_Catalog_Name As String = Nothing, Optional ByVal TimeOut As Integer = 50, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Boolean
            Dim ret As Boolean = False
            Try
                SubName = ErrName & GetCurrentMethod.Name & ":"
                Select Case DataBase_Link_Boolean
                    Case False
                        DataBase_Connection.ConnectionString = "Data Source=" & Server_Add & ";Initial Catalog=" & Initial_Catalog_Name & ";User ID=" & Use_ID & ";Password=" & Pass_Word & ";Connect Timeout=" & TimeOut & ";Asynchronous Processing=True;MultipleActiveResultSets=True"
                        If DataBase_Connection.State = ConnectionState.Closed Then
                            DataBase_Connection.Open()
                            ret = True
                            DataBase_Link_Boolean = True
                        Else
                            ret = False
                        End If
                    Case True
                End Select
            Catch ex As Exception
               
             
                Write_Log( SubName & ex.Message  )
                DataBase_Link_Boolean = False
                ret = False
            End Try
            Return ret
        End Function

        ''' <summary>
        ''' 关闭数据库
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function Close_DataBase(Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Boolean
            Dim ret As Boolean = False
            Try
                SubName = ErrName & GetCurrentMethod.Name & ":"
                If DataBase_Connection.State = ConnectionState.Open Then
                    DataBase_Connection.Close()
                    DataBase_Connection.Dispose()
                    DataBase_Link_Boolean = False
                    ret = True
                Else
                    ret = False
                End If
            Catch ex As Exception
                Write_Log(SubName & ex.Message)
                ret = False
            End Try
            Return ret
        End Function

        Function Delete_Debug(ByVal Tab_Name As String) As Boolean
            Dim Fun_Name As String = GetCurrentMethod.Name
            Try
                Select Case DataBase_Link_Boolean
                    Case True
                        Dim DataBase_Com As SqlCommand
                        DataBase_Com = New SqlCommand("DELETE FROM " & Tab_Name & "", DataBase_Connection)
                        DataBase_Com.ExecuteScalar()
                        DataBase_Com.Dispose()
                End Select
            Catch ex As Exception
                Return False
            End Try
            Return True
        End Function

        ''' <summary>
        ''' 插入测量结果，主表AOI
        ''' </summary>
        ''' <param name="Station_Name"></param>
        ''' <param name="Machine_NO"></param>
        ''' <param name="Project_Name"></param>
        ''' <param name="Tray_Code"></param>
        ''' <param name="Language"></param>
        ''' <param name="Layout"></param>
        ''' <param name="Result"></param>
        ''' <param name="Fail_Location"></param>
        ''' <param name="Order_NO"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function Insert_Result(ByVal Table_Name As String, ByVal Station_Name As String, ByVal Machine_NO As String, ByVal Project_Name As String, ByVal Tray_Code As String, ByVal Language As String, ByVal Layout As String, ByVal Result As String, ByVal Fail_Location As String, ByVal Order_NO As String, ByVal Sql_Time As String, ByVal LightLakageResult As String, ByVal HookSnapeResult As String) As Boolean
            Dim ret As Boolean = False, SubName As String = Nothing
            Try
                SubName = ErrName & GetCurrentMethod.Name & ":"
                Select Case DataBase_Link_Boolean
                    Case True
                        Dim DataBase_Com As SqlCommand

                        DataBase_Com = New SqlCommand("INSERT INTO " & Table_Name & " (StationName, MachineNo, ProjectName, TrayCode, Language, Layout,FailLocation, OrderNo,CreationTime,MC_Result,OP_Rejudge,Final_Result,LightLeakage_Key_NgCount,HookSnap_key_NgCount)VALUES(N'" & Station_Name & "', N'" & Machine_NO & "', N'" & Project_Name & "', N'" & Tray_Code & "',N'" & Language & "', N'" & Layout & "', N'" & Fail_Location & "', N'" & Order_NO & "',CONVERT(DATETIME, '" & Sql_Time & "'),N'" & Result & "',N'N/A',N'" & Result & "',N'" & LightLakageResult & "',N'" & HookSnapeResult & "')", DataBase_Connection)
                        DataBase_Com.ExecuteNonQuery()
                        DataBase_Com.Dispose()
                        ret = True
                End Select
            Catch ex As Exception
                Write_Log(SubName & ex.Message)
                ret = False
            End Try
            Return ret
        End Function

        ''' <summary>
        ''' 查询ID
        ''' </summary>
        ''' <param name="Tray_Code"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function Query_ID(ByVal Table_Name As String, ByVal Tray_Code As String) As Integer
            Dim ret As Integer = 0, SubName As String = Nothing
            Try
                SubName = ErrName & GetCurrentMethod.Name & ":"
                Select Case DataBase_Link_Boolean
                    Case True
                        Dim DataBase_Com As SqlCommand

                        DataBase_Com = New SqlCommand("SELECT ID FROM " & Table_Name & " WHERE TrayCode='" & Tray_Code & "' ORDER BY ID DESC", DataBase_Connection)
                        ret = CType(DataBase_Com.ExecuteScalar(), Integer)
                        DataBase_Com.Dispose()
                End Select
            Catch ex As Exception
                Write_Log(SubName & ex.Message)
                ret = 0
            End Try
            Return ret
        End Function

        Function Updata_NGCount_Result(ByVal Table_Name As String, ByVal LightLeakage_Key_NgCount As String, ByVal HookSnap_key_NgCount As String, ByVal ID As Integer) As Boolean
            Dim ret As Boolean = False, SubName As String = Nothing
            Try
                SubName = ErrName & GetCurrentMethod.Name & ":"
                Select Case DataBase_Link_Boolean
                    Case True
                        Dim DataBase_Com As SqlCommand

                        DataBase_Com = New SqlCommand("UPDATE " & Table_Name & " SET LightLeakage_Key_NgCount = N'" & LightLeakage_Key_NgCount & "', HookSnap_key_NgCount = N'" & HookSnap_key_NgCount & "'  WHERE (ID = N'" & ID & "')", DataBase_Connection)
                        DataBase_Com.ExecuteNonQuery()
                        DataBase_Com.Dispose()

                        ret = True
                End Select
            Catch ex As Exception
                Write_Log(SubName & ex.Message)
                ret = False
            End Try
            Return ret
        End Function

        ''' <summary>
        ''' 查询工单号
        ''' </summary>
        ''' <param name="Tray_Code"></param>
        ''' <param name="Order_NO"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function Query_Order_NO(ByVal Tray_Code As String, ByRef Order_NO As String)
            Dim ret As Boolean = False, SubName As String = Nothing
            Try
                SubName = ErrName & GetCurrentMethod.Name & ":"
                Select Case DataBase_Link_Boolean
                    Case True
                        Dim DataBase_Com As SqlCommand
                        Dim DataBase_ConnectionAdapter As SqlDataAdapter
                        Dim _data As New DataSet
                        _data.Clear()

                        DataBase_Com = New SqlCommand("SELECT 工单号 FROM 拆卸机 WHERE [Tray code] ='" & Tray_Code & "'", DataBase_Connection)
                        DataBase_Com.ExecuteNonQuery()
                        DataBase_ConnectionAdapter = New SqlDataAdapter(DataBase_Com.CommandText, DataBase_Connection)
                        DataBase_ConnectionAdapter.Fill(_data)
                        If IsDBNull(_data.Tables(0).Rows(0).Item(0)) = True Then
                            Order_NO = ""
                        Else
                            Order_NO = CType(_data.Tables(0).Rows(0).Item(0), String)
                        End If

                        DataBase_ConnectionAdapter.Dispose()
                        DataBase_Com.Dispose()
                        _data.Dispose()
                        ret = True
                End Select
            Catch ex As Exception
                Write_Log(SubName & ex.Message)
                ret = False
            End Try
            Return ret
        End Function


 

    End Class
    Public Class SQL_LIB_X816

        Public DataBase_Data_Souce As String = "192.168.113.253"
        Public DataBase_ID As String = "sfc"
        Public DataBase_PassWord As String = "HipScf2017"
        Public DataBase_Catalog_Name As String = "X159"

        Dim DataBase_Link_Boolean As Boolean
        Dim Soft_Default_Path As String = IO.Directory.GetCurrentDirectory & "\"
        Dim DataBase_Connection As New SqlConnection
        Dim Sw As IO.StreamWriter
        Dim SqlServerName As String
        Dim InitialCatalogName As String

        Sub Write_Log(ByVal Message As String)
            'MessageBox.Show(Message & vbCrLf & "以上错误如自行不能解决请联系软件工程师！", "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
            Dim File_Path As String = Nothing
            Dim MON As String = Date.Now.Month.ToString.PadLeft(2).Replace(" ", "0") & " MONTH\"
            Dim DAY As String = Date.Now.Day.ToString.PadLeft(2).Replace(" ", "0") & " DAY\"
            File_Path = "C:\" & MON & DAY
            If System.IO.Directory.Exists(File_Path) = False Then
                System.IO.Directory.CreateDirectory(File_Path)
            End If

            Sw = New IO.StreamWriter(File_Path & "SystemLog.txt", True, Encoding.Default)
            Sw.WriteLine(Date.Now & "," & Message)
            Sw.Flush()
            Sw.Close()
            Sw.Dispose()
        End Sub

        ''' <summary>
        ''' 初始化数据库
        ''' </summary>
        ''' <param name="Server_ADD">数据库地址</param>
        ''' <param name="Use_ID">数据库用户名</param>
        ''' <param name="Pass_Word">数据库密码，缺省无密码</param>
        ''' <param name="Initial_Catalog_Name">数据库名称</param>
        ''' <returns>返回值：TRUE（初始化成功），FALSE（初始化失败）</returns>
        ''' <param name="TimeOut">超时</param>
        ''' <remarks></remarks>
        Function DataBase_Initialization(Optional ByVal Server_Add As String = "127.0.0.1", Optional ByVal Use_ID As String = "SA", Optional ByVal Pass_Word As String = Nothing, Optional ByVal Initial_Catalog_Name As String = Nothing, Optional ByVal TimeOut As Integer = 50, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Boolean
            Dim ret As Boolean = False
            Try
                SubName = ErrName & GetCurrentMethod.Name & ":"
                Select Case DataBase_Link_Boolean
                    Case False
                        DataBase_Connection.ConnectionString = "Data Source=" & Server_Add & ";Initial Catalog=" & Initial_Catalog_Name & ";User ID=" & Use_ID & ";Password=" & Pass_Word & ";Connect Timeout=" & TimeOut & ";Asynchronous Processing=True;MultipleActiveResultSets=True"
                        If DataBase_Connection.State = ConnectionState.Closed Then
                            DataBase_Connection.Open()
                            ret = True
                            DataBase_Link_Boolean = True
                        Else
                            ret = False
                        End If
                    Case True
                End Select
            Catch ex As Exception


                Write_Log(SubName & ex.Message)
                DataBase_Link_Boolean = False
                ret = False
            End Try
            Return ret
        End Function

        ''' <summary>
        ''' 关闭数据库
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function Close_DataBase(Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Boolean
            Dim ret As Boolean = False
            Try
                SubName = ErrName & GetCurrentMethod.Name & ":"
                If DataBase_Connection.State = ConnectionState.Open Then
                    DataBase_Connection.Close()
                    DataBase_Connection.Dispose()
                    DataBase_Link_Boolean = False
                    ret = True
                Else
                    ret = False
                End If
            Catch ex As Exception


                Write_Log(SubName & ex.Message)
                ret = False
            End Try
            Return ret
        End Function

        Function Delete_Debug(ByVal Tab_Name As String) As Boolean
            Dim Fun_Name As String = GetCurrentMethod.Name
            Try
                Select Case DataBase_Link_Boolean
                    Case True
                        Dim DataBase_Com As SqlCommand
                        DataBase_Com = New SqlCommand("DELETE FROM " & Tab_Name & "", DataBase_Connection)
                        DataBase_Com.ExecuteScalar()
                        DataBase_Com.Dispose()
                End Select
            Catch ex As Exception
                Return False
            End Try
        End Function

        ''' <summary>
        ''' 插入测量结果，主表AOI
        ''' </summary>
        ''' <param name="Station_Name"></param>
        ''' <param name="Machine_NO"></param>
        ''' <param name="Project_Name"></param>
        ''' <param name="Tray_Code"></param>
        ''' <param name="Language"></param>
        ''' <param name="Layout"></param>
        ''' <param name="Result"></param>
        ''' <param name="Fail_Location"></param>
        ''' <param name="Order_NO"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function Insert_Result(ByVal Table_Name As String, ByVal Station_Name As String, ByVal Machine_NO As String, ByVal Project_Name As String, ByVal Tray_Code As String, ByVal Language As String, ByVal Layout As String, ByVal Result As String, ByVal Fail_Location As String, ByVal Order_NO As String, ByVal Sql_Time As String, ByVal LightLakageResult As String, ByVal HookSnapeResult As String) As Boolean
            Dim ret As Boolean = False, SubName As String = Nothing
            Try
                SubName = ErrName & GetCurrentMethod.Name & ":"
                Select Case DataBase_Link_Boolean
                    Case True
                        Dim DataBase_Com As SqlCommand

                        DataBase_Com = New SqlCommand("INSERT INTO " & Table_Name & " ([Station Name], [Machine No], [Project Name], [Tray Code], Language, Layout, [Fail Location], [Order No],Time,MC_Result,OP_Rejudge,Final_Result,LightLeakage_Key_NgCount,HookSnap_key_NgCount)VALUES(N'" & Station_Name & "', N'" & Machine_NO & "', N'" & Project_Name & "', N'" & Tray_Code & "',N'" & Language & "', N'" & Layout & "', N'" & Fail_Location & "', N'" & Order_NO & "',CONVERT(DATETIME, '" & Sql_Time & "'),N'" & Result & "',N'N/A',N'" & Result & "',N'" & LightLakageResult & "',N'" & HookSnapeResult & "')", DataBase_Connection)
                        DataBase_Com.ExecuteNonQuery()
                        DataBase_Com.Dispose()
                        ret = True
                End Select
            Catch ex As Exception
                Write_Log(SubName & ex.Message)
                ret = False
            End Try
            Return ret
        End Function

        Function Updata_NGCount_Result(ByVal Table_Name As String, ByVal LightLeakage_Key_NgCount As String, ByVal HookSnap_key_NgCount As String, ByVal ID As Integer) As Boolean
            Dim ret As Boolean = False, SubName As String = Nothing
            Try
                SubName = ErrName & GetCurrentMethod.Name & ":"
                Select Case DataBase_Link_Boolean
                    Case True
                        Dim DataBase_Com As SqlCommand

                        DataBase_Com = New SqlCommand("UPDATE " & Table_Name & " SET LightLeakage_Key_NgCount = N'" & LightLeakage_Key_NgCount & "', HookSnap_key_NgCount = N'" & HookSnap_key_NgCount & "'  WHERE (ID = N'" & ID & "')", DataBase_Connection)
                        DataBase_Com.ExecuteNonQuery()
                        DataBase_Com.Dispose()

                        ret = True
                End Select
            Catch ex As Exception
                Write_Log(SubName & ex.Message)
                ret = False
            End Try
            Return ret
        End Function

        ''' <summary>
        ''' 查询工单号
        ''' </summary>
        ''' <param name="Tray_Code"></param>
        ''' <param name="Order_NO"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function Query_Order_NO(ByVal Tray_Code As String, ByRef Order_NO As String) As Boolean
            Dim ret As Boolean = False, SubName As String = Nothing
            Try
                SubName = ErrName & GetCurrentMethod.Name & ":"
                Select Case DataBase_Link_Boolean
                    Case True
                        Dim DataBase_Com As SqlCommand
                        Dim DataBase_ConnectionAdapter As SqlDataAdapter
                        Dim _data As New DataSet
                        _data.Clear()

                        DataBase_Com = New SqlCommand("SELECT [Order No] FROM [SP-13-Decap Machine] WHERE [Tray code] ='" & Tray_Code & "'", DataBase_Connection)
                        DataBase_Com.ExecuteNonQuery()
                        DataBase_ConnectionAdapter = New SqlDataAdapter(DataBase_Com.CommandText, DataBase_Connection)
                        DataBase_ConnectionAdapter.Fill(_data)
                        Order_NO = CType(_data.Tables(0).Rows(0).Item(0), String)
                        DataBase_ConnectionAdapter.Dispose()
                        DataBase_Com.Dispose()
                        _data.Dispose()
                        ret = True
                End Select
            Catch ex As Exception
                Write_Log(SubName & ex.Message)
                ret = False
            End Try
            Return ret
        End Function

        ''' <summary>
        ''' 查询ID
        ''' </summary>
        ''' <param name="Tray_Code"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function Query_ID(ByVal Table_Name As String, ByVal Tray_Code As String) As Integer
            Dim ret As Integer = 0, SubName As String = Nothing
            Try
                SubName = ErrName & GetCurrentMethod.Name & ":"
                Select Case DataBase_Link_Boolean
                    Case True
                        Dim DataBase_Com As SqlCommand

                        DataBase_Com = New SqlCommand("SELECT ID FROM " & Table_Name & " WHERE [Tray Code]='" & Tray_Code & "' ORDER BY ID DESC", DataBase_Connection)
                        ret = CType(DataBase_Com.ExecuteScalar(), Integer)
                        DataBase_Com.Dispose()
                End Select
            Catch ex As Exception
                Write_Log(SubName & ex.Message)
                ret = 0
            End Try
            Return ret
        End Function

    End Class
    Public Class SQL_LIB_X816RSAOI

        Public DataBase_Data_Souce As String = "192.168.117.253"
        Public DataBase_ID As String = "sfs"
        Public DataBase_PassWord As String = "Hippih.*"
        Public DataBase_Catalog_Name As String = "X17RSAOI"

        Dim DataBase_Link_Boolean As Boolean
        Dim Soft_Default_Path As String = IO.Directory.GetCurrentDirectory & "\"
        Dim DataBase_Connection As New SqlConnection
        Dim Sw As IO.StreamWriter
        Dim SqlServerName As String
        Dim InitialCatalogName As String

        Dim callback As New AsyncCallback(AddressOf HandleCallback)
        Private Sub HandleCallback(ByVal result As IAsyncResult)
            Try
                Dim command As SqlCommand = CType(result.AsyncState, SqlCommand)
                Dim rowCount As Integer = command.EndExecuteNonQuery(result)
            Catch ex As Exception
            End Try
        End Sub

        Sub Write_Log(ByVal Message As String)
            'MessageBox.Show(Message & vbCrLf & "以上错误如自行不能解决请联系软件工程师！", "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
            Dim File_Path As String = Nothing
            Dim MON As String = Date.Now.Month.ToString.PadLeft(2).Replace(" ", "0") & " MONTH\"
            Dim DAY As String = Date.Now.Day.ToString.PadLeft(2).Replace(" ", "0") & " DAY\"
            File_Path = "C:\" & MON & DAY
            If System.IO.Directory.Exists(File_Path) = False Then
                System.IO.Directory.CreateDirectory(File_Path)
            End If

            Sw = New IO.StreamWriter(File_Path & "SystemLog.txt", True, Encoding.Default)
            Sw.WriteLine(Date.Now & "," & Message)
            Sw.Flush()
            Sw.Close()
            Sw.Dispose()
        End Sub

        ''' <summary>
        ''' 初始化数据库
        ''' </summary>
        ''' <param name="Server_ADD">数据库地址</param>
        ''' <param name="Use_ID">数据库用户名</param>
        ''' <param name="Pass_Word">数据库密码，缺省无密码</param>
        ''' <param name="Initial_Catalog_Name">数据库名称</param>
        ''' <returns>返回值：TRUE（初始化成功），FALSE（初始化失败）</returns>
        ''' <param name="TimeOut">超时</param>
        ''' <remarks></remarks>
        Function DataBase_Initialization(Optional ByVal Server_Add As String = "127.0.0.1", Optional ByVal Use_ID As String = "SA", Optional ByVal Pass_Word As String = Nothing, Optional ByVal Initial_Catalog_Name As String = Nothing, Optional ByVal TimeOut As Integer = 50, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Boolean
            Dim ret As Boolean = False
            Try
                SubName = ErrName & GetCurrentMethod.Name & ":"
                Select Case DataBase_Link_Boolean
                    Case False
                        DataBase_Connection.ConnectionString = "Data Source=" & Server_Add & ";Initial Catalog=" & Initial_Catalog_Name & ";User ID=" & Use_ID & ";Password=" & Pass_Word & ";Connect Timeout=" & TimeOut & ";Asynchronous Processing=True;MultipleActiveResultSets=True"
                        If DataBase_Connection.State = ConnectionState.Closed Then
                            DataBase_Connection.Open()
                            ret = True
                            DataBase_Link_Boolean = True
                        Else
                            ret = False
                        End If
                    Case True
                End Select
            Catch ex As Exception


                Write_Log(SubName & ex.Message)
                DataBase_Link_Boolean = False
                ret = False
            End Try
            Return ret
        End Function

        ''' <summary>
        ''' 关闭数据库
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function Close_DataBase(Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Boolean
            Dim ret As Boolean = False
            Try
                SubName = ErrName & GetCurrentMethod.Name & ":"
                If DataBase_Connection.State = ConnectionState.Open Then
                    DataBase_Connection.Close()
                    DataBase_Connection.Dispose()
                    DataBase_Link_Boolean = False
                    ret = True
                Else
                    ret = False
                End If
            Catch ex As Exception
                Write_Log(SubName & ex.Message)
                ret = False
            End Try
            Return ret
        End Function

        Function Exits_Measure_Result(ByVal Table_Name As String, ByVal BarCode As String) As Boolean
            Dim ret As Boolean = False
            If BarCode Is Nothing Then
                BarCode = "NULL"
            ElseIf BarCode = "" Then
                BarCode = "NULL"
            End If

            Dim DataBase_Com As SqlCommand
            Try
                Select Case DataBase_Link_Boolean
                    Case True
                        DataBase_Com = New SqlCommand("SELECT COUNT(ID) AS 统计 FROM " & Table_Name & " WHERE [Tray Code] = N'" & BarCode & "'", DataBase_Connection)
                        Dim Select_Count As Integer = DataBase_Com.ExecuteScalar
                        DataBase_Com.Dispose()

                        If Select_Count <= 0 Then
                            ret = False
                        Else
                            ret = True
                        End If
                End Select
            Catch ex As Exception
                Write_Log(ex.Message)
                ret = False
            End Try
            Return ret
        End Function

        Function Delete_Table_Name(ByVal Table_Name As String, ByVal Machine_NO As Integer) As Boolean
            Dim ret As Boolean = False
            Dim DataBase_Com As SqlCommand
            Try
                Select Case DataBase_Link_Boolean
                    Case True
                        If Machine_NO <> 0 Then
                            DataBase_Com = New SqlCommand("DELETE FROM " & Table_Name & " WHERE [Machine No]=N'" & Machine_NO & "'", DataBase_Connection)
                            DataBase_Com.ExecuteNonQuery()
                            DataBase_Com.Dispose()
                        End If
                        ret = True
                End Select
            Catch ex As Exception
                Write_Log(ex.Message)
                ret = False
            End Try
            Return ret
        End Function

        Function Delete_Table_Name(ByVal Table_Name As String) As Boolean
            Dim ret As Boolean = False
            Dim DataBase_Com As SqlCommand
            Try
                Select Case DataBase_Link_Boolean
                    Case True

                        DataBase_Com = New SqlCommand("DELETE FROM " & Table_Name & "", DataBase_Connection)
                        DataBase_Com.ExecuteNonQuery()
                        DataBase_Com.Dispose()

                        ret = True
                End Select
            Catch ex As Exception
                Write_Log(ex.Message)
                ret = False
            End Try
            Return ret
        End Function

        Function Query_Measure_Result(ByVal Table_Name As String, ByVal BarCode As String, ByVal Key_Name As String, ByRef Test_Result As String)
            Dim ret As Boolean = False
            If BarCode Is Nothing Then
                BarCode = "NULL"
            ElseIf BarCode = "" Then
                BarCode = "NULL"
            End If
            If Key_Name Is Nothing Then
                Key_Name = "NULL"
            ElseIf Key_Name = "" Then
                Key_Name = "NULL"
            End If
            If Table_Name Is Nothing Then
                Table_Name = "NULL"
            ElseIf Table_Name = "" Then
                Table_Name = "NULL"
            End If
            Dim DataBase_Com As New SqlCommand
            Try
                Select Case DataBase_Link_Boolean
                    Case True
                        Dim _Dataset As New DataSet
                        Dim DataBase_ConnectionAdapter As SqlDataAdapter
                        _Dataset.Clear()

                        DataBase_Com = New SqlCommand("SELECT Result FROM " & Table_Name & " WHERE ([Tray Code] = N'" & BarCode & "') AND ([Key Name] = N'" & Key_Name & "')", DataBase_Connection)
                        DataBase_ConnectionAdapter = New SqlDataAdapter(DataBase_Com.CommandText, DataBase_Connection)
                        DataBase_ConnectionAdapter.Fill(_Dataset)

                        If IsDBNull(_Dataset.Tables(0).Rows(0).Item(0)) = True Then
                            Test_Result = "NG"
                        Else
                            Test_Result = CType(_Dataset.Tables(0).Rows(0).Item(0), String)
                        End If
                        DataBase_ConnectionAdapter.Dispose()
                        DataBase_Com.ExecuteNonQuery()
                        DataBase_Com.Dispose()
                        ret = True
                End Select
            Catch ex As Exception
                Write_Log(ex.Message)
                ret = False
            End Try
            Return ret
        End Function

        ''' <summary>
        ''' 查询表是否存在
        ''' </summary>
        ''' <param name="Tab_Name">表名称</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function Query_Table_Exist(ByVal Tab_Name As String) As Int64
            Dim rtn As Int64 = 0
            Dim Fun_Name As String = GetCurrentMethod.Name
            Try
                Select Case DataBase_Link_Boolean
                    Case True
                        Dim DataBase_Com As SqlCommand
                        DataBase_Com = New SqlCommand("select * from sys.tables where name=" & "'" & Tab_Name & "'", DataBase_Connection)
                        Dim tablename As String = DataBase_Com.ExecuteScalar()
                        DataBase_Com.Dispose()
                        If Tab_Name <> tablename Then
                            rtn = 1 '表不存在
                        Else
                            rtn = 0 '表存在
                        End If
                End Select
            Catch ex As Exception
                Write_Log(Fun_Name & ex.Message)
                rtn = -1 '错误
            End Try
            Return rtn
        End Function

        Function Delete_Debug(ByVal Tab_Name As String) As Boolean
            Dim Fun_Name As String = GetCurrentMethod.Name
            Try
                Select Case DataBase_Link_Boolean
                    Case True
                        Dim DataBase_Com As SqlCommand
                        DataBase_Com = New SqlCommand("DELETE FROM " & Tab_Name & "", DataBase_Connection)
                        DataBase_Com.ExecuteScalar()
                        DataBase_Com.Dispose()
                End Select
            Catch ex As Exception
                Return False
            End Try
        End Function



        Function Copy_Talbe(ByVal Tab_Name As String) As Boolean
            Dim Fun_Name As String = GetCurrentMethod.Name & ":"
            Try
                Select Case DataBase_Link_Boolean
                    Case True
                        Dim DataBase_Com As SqlCommand
                        Dim Table_Parameters As String = "CREATE TABLE [dbo].[" & Tab_Name & "](" _
                        & "[ID] [int] IDENTITY(1,1) NOT NULL," _
                        & "	[Product ID] [int] NULL," _
                        & "[Machine No] [varchar](5) NULL," _
                        & "	[Project Name] [varchar](5) NULL," _
                        & "	[Tray Code] [varchar](35) NULL," _
                        & "[Language] [varchar](5) NULL," _
                        & "	[Layout] [varchar](5) NULL," _
                        & "	[Time] [datetime] NULL," _
                        & "[Key No] [varchar](10) NULL," _
                        & "	[Key Type] [varchar](10) NULL," _
                        & "	[Key Name] [varchar](20) NULL," _
                        & " [LightLeakage_Left_Result] [nchar](3) NULL," _
                        & "[LightLeakage_Up_Result] [nchar](3) NULL," _
                        & "[LightLeakage_Right_Result] [nchar](3) NULL," _
                        & "	[LightLeakage_Down_Result] [nchar](3) NULL," _
                        & "	[Snap01_Result] [nchar](3) NULL," _
                        & "	[Snap02_Result] [nchar](3) NULL," _
                        & "	[Snap03_Result] [nchar](3) NULL," _
                        & "	[Snap04_Result] [nchar](3) NULL," _
                        & "	[Hook01_Result] [nchar](3) NULL," _
                        & "	[Hook02_Result] [nchar](3) NULL," _
                        & "[Hook03_Result] [nchar](3) NULL," _
                        & "[Hook04_Result] [nchar](3) NULL," _
                        & "[GateVestige01_Result] [nchar](3) NULL," _
                        & "[GateVestige02_Result] [nchar](3) NULL," _
                        & "[GateVestige03_Result] [nchar](3) NULL," _
                        & "[LightLeakageResult] [nchar](3) NULL," _
                        & "[HookSnapResult] [nchar](3) NULL," _
                        & "[MC_Result] [nchar](3) NULL," _
                        & "[OP_Rejudge] [nchar](3) NULL," _
                        & "[Final_Result] [nchar](3) NULL" _
                        & ") ON [PRIMARY]"
                        DataBase_Com = New SqlCommand(Table_Parameters, DataBase_Connection)
                        DataBase_Com.ExecuteScalar()
                        DataBase_Com.Dispose()
                        Return True
                End Select
            Catch ex As Exception
                Write_Log(Fun_Name & ex.Message)
                Return False
            End Try
        End Function







        ''' <summary>
        ''' 查询表是否存在
        ''' </summary>
        ''' <param name="Tab_Name">表名称</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function Copy_Talbe_B(ByVal Tab_Name As String) As Boolean
            Dim Fun_Name As String = GetCurrentMethod.Name & ":"
            Try
                Select Case DataBase_Link_Boolean
                    Case True
                        Dim DataBase_Com As SqlCommand
                        Dim Table_Parameters As String = "CREATE TABLE [dbo].[" & Tab_Name & "](" _
                        & "[ID] [int] IDENTITY(1,1) NOT NULL," _
                        & "	[Product ID] [nvarchar](50) NULL," _
                        & "[Machine No] [nvarchar](50) NULL," _
                        & "	[Project Name] [nvarchar](50) NULL," _
                        & "	[Tray Code] [nvarchar](50) NULL," _
                        & "[Language] [nvarchar](50) NULL," _
                        & "	[Layout] [nvarchar](50) NULL," _
                        & "	[Time] [datetime] NULL," _
                        & "[Key No] [nvarchar](50) NULL," _
                        & "	[Key Type] [nvarchar](50) NULL," _
                        & "	[Key Name] [nvarchar](50) NULL," _
                        & " [LightLeakage_Left_Result] [nchar](3) NULL," _
                        & "[LightLeakage_Up_Result] [nchar](3) NULL," _
                        & "[LightLeakage_Right_Result] [nchar](3) NULL," _
                        & "	[LightLeakage_Down_Result] [nchar](3) NULL," _
                        & "	[Snap01_Result] [nchar](3) NULL," _
                        & "	[Snap02_Result] [nchar](3) NULL," _
                        & "	[Snap03_Result] [nchar](3) NULL," _
                        & "	[Snap04_Result] [nchar](3) NULL," _
                        & "	[Hook01_Result] [nchar](3) NULL," _
                        & "	[Hook02_Result] [nchar](3) NULL," _
                        & "[Hook03_Result] [nchar](3) NULL," _
                        & "[Hook04_Result] [nchar](3) NULL," _
                        & "[GateVestige01_Result] [nchar](3) NULL," _
                        & "[GateVestige02_Result] [nchar](3) NULL," _
                        & "[GateVestige03_Result] [nchar](3) NULL," _
                        & "[LightLeakageResult] [nchar](3) NULL," _
                        & "[HookSnapResult] [nchar](3) NULL," _
                        & "[MC_Result] [nchar](3) NULL," _
                        & "[OP_Rejudge] [nchar](3) NULL," _
                        & "[Final_Result] [nchar](3) NULL" _
                        & ") ON [PRIMARY]"
                        DataBase_Com = New SqlCommand(Table_Parameters, DataBase_Connection)
                        DataBase_Com.ExecuteScalar()
                        DataBase_Com.Dispose()
                        Return True
                End Select
            Catch ex As Exception
                Write_Log(Fun_Name & ex.Message)
                Return False
            End Try
        End Function


        ''' <summary>
        ''' 获取数据库系统时间
        ''' </summary>
        ''' <param name="Sql_Time">数据库系统时间</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function Get_Sql_Time(ByRef Sql_Time As String) As Boolean
            Dim Fun_Name As String = GetCurrentMethod.Name & ":"
            Try
                Select Case DataBase_Link_Boolean
                    Case True
                        Dim DataBase_Com As SqlCommand
                        DataBase_Com = New SqlCommand("SELECT CONVERT(VARCHAR(100),GETDATE(),21)", DataBase_Connection)
                        Sql_Time = CType(DataBase_Com.ExecuteScalar(), String)
                        DataBase_Com.Dispose()
                        Return True
                End Select
            Catch ex As Exception
                Write_Log(Fun_Name & ex.Message)
                Return False
            End Try
        End Function

        Function Insert_Measure_Result(ByVal Table_Name As String, ByVal Test_Time As String, ByVal Parameter_Name() As String, ByVal Parameter_Value() As Object) As Boolean
            Dim ret As Boolean = False
            Dim DataBase_Com As SqlCommand
            Try
                Select Case DataBase_Link_Boolean
                    Case True
                        Dim Parameter_Count As Integer = Parameter_Name.Length
                        Dim Parameter_Total_Str As String = "Time,"
                        Dim Parameter_Value_Total_Str As String = "CONVERT(DATETIME, '" & Test_Time & "'),"

                        For i As Integer = 0 To Parameter_Count - 1
                            Select Case i
                                Case Parameter_Count - 1
                                    Parameter_Total_Str = Parameter_Total_Str & Parameter_Name(i)
                                    If IsNumeric(Parameter_Value(i)) = False Then
                                        Parameter_Value_Total_Str = Parameter_Value_Total_Str & "'" & Parameter_Value(i) & " '"
                                    Else
                                        Parameter_Value_Total_Str = Parameter_Value_Total_Str & Parameter_Value(i)
                                    End If
                                Case Else
                                    Parameter_Total_Str = Parameter_Total_Str & Parameter_Name(i) & ","
                                    If IsNumeric(Parameter_Value(i)) = False Then
                                        Parameter_Value_Total_Str = Parameter_Value_Total_Str & "'" & Parameter_Value(i) & "',"
                                    Else
                                        Parameter_Value_Total_Str = Parameter_Value_Total_Str & Parameter_Value(i) & ","
                                    End If
                            End Select
                        Next
                        DataBase_Com = New SqlCommand("INSERT INTO " & Table_Name & " (" & Parameter_Total_Str & ") VALUES ( " & Parameter_Value_Total_Str & ")", DataBase_Connection)
                        DataBase_Com.ExecuteNonQuery()
                        DataBase_Com.Dispose()

                        ret = True
                End Select
            Catch ex As Exception
                Write_Log(ex.Message)
                ret = False
            End Try
            Return ret
        End Function

        ''' <summary>
        ''' 插入测量结果
        ''' </summary>
        ''' <param name="Table_Name">表明</param>
        ''' <param name="Test_Time">测试时间</param>
        ''' <param name="BarCode">条码</param>
        ''' <param name="Key_Name">键名</param>
        ''' <param name="Parameter_Name">参数名</param>
        ''' <param name="Parameter_Value">参数值</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function Insert_Measure_Result(ByVal Table_Name As String, ByVal Test_Time As String, ByVal BarCode As String, ByVal Key_Name As String, ByVal Parameter_Name() As String, ByVal Parameter_Value() As Object, ByRef Result_ALL As String) As Boolean
            Dim ret As Boolean = False
            If BarCode Is Nothing Then
                BarCode = "NULL"
            ElseIf BarCode = "" Then
                BarCode = "NULL"
            End If
            If Key_Name Is Nothing Then
                Key_Name = "NULL"
            ElseIf Key_Name = "" Then
                Key_Name = "NULL"
            End If

            Dim DataBase_Com As SqlCommand
            Try
                Select Case DataBase_Link_Boolean
                    Case True
                        DataBase_Com = New SqlCommand("SELECT COUNT(ID) AS 统计 FROM " & Table_Name & " WHERE ([Tray Code] = N'" & BarCode & "') AND ([Key Name] = N'" & Key_Name & "')", DataBase_Connection)
                        Dim Select_Count As Integer = DataBase_Com.ExecuteScalar
                        DataBase_Com.Dispose()

                        'Thread.Sleep(1)

                        If Select_Count <= 0 Then '插入新值
                            Write_Log("Insert_Measure_Result:" & Date.Now & "SQLCMD:" & DataBase_Com.CommandText & "rtn:" & Select_Count)
                            Return False
                        Else '更新
                            Dim Parameter_Count As Integer = Parameter_Name.Length
                            Dim Total_Parameter_Name_Str As String = "Time = " & "CONVERT(DATETIME, '" & Test_Time & "')" & ","
                            For i As Integer = 0 To Parameter_Count - 1
                                Select Case i
                                    Case Parameter_Count - 1
                                        Select Case IsNumeric(Parameter_Value(i))
                                            Case True
                                                Total_Parameter_Name_Str = Total_Parameter_Name_Str & Parameter_Name(i) & " = " & Parameter_Value(i)
                                            Case False
                                                Total_Parameter_Name_Str = Total_Parameter_Name_Str & Parameter_Name(i) & " = '" & Parameter_Value(i) & "'"
                                        End Select
                                    Case Else
                                        Select Case IsNumeric(Parameter_Value(i))
                                            Case True
                                                Total_Parameter_Name_Str = Total_Parameter_Name_Str & Parameter_Name(i) & " = " & Parameter_Value(i) & ", "
                                            Case False
                                                Total_Parameter_Name_Str = Total_Parameter_Name_Str & Parameter_Name(i) & " = '" & Parameter_Value(i) & "',"
                                        End Select
                                End Select
                            Next
                            DataBase_Com = New SqlCommand("UPDATE " & Table_Name & " SET " & Total_Parameter_Name_Str & " WHERE ([Tray Code] = '" & BarCode & "') AND ([Key Name] = '" & Key_Name & "')", DataBase_Connection)
                            DataBase_Com.ExecuteNonQuery()
                            DataBase_Com.Dispose()

                            '统计总结果
                            Dim _Dataset As New DataSet
                            Dim DataBase_ConnectionAdapter As SqlDataAdapter
                            _Dataset.Clear()
                            DataBase_Com = New SqlCommand("SELECT LightLeakageResult, HookSnapResult FROM " & Table_Name & " WHERE ([Tray Code] = N'" & BarCode & "') AND ([Key Name] = '" & Key_Name & "')", DataBase_Connection)
                            DataBase_ConnectionAdapter = New SqlDataAdapter(DataBase_Com.CommandText, DataBase_Connection)
                            DataBase_ConnectionAdapter.Fill(_Dataset)

                            Dim Result1, Result2 As String
                            If IsDBNull(_Dataset.Tables(0).Rows(0).Item(0)) = True Then
                                Result1 = "NG"
                            Else
                                Result1 = CType(_Dataset.Tables(0).Rows(0).Item(0), String).ToString.Trim
                            End If
                            If IsDBNull(_Dataset.Tables(0).Rows(0).Item(1)) = True Then
                                Result2 = "NG"
                            Else
                                Result2 = CType(_Dataset.Tables(0).Rows(0).Item(1), String).ToString.Trim
                            End If
                            If Result1 = "OK" And Result2 = "OK" Then
                                Result_ALL = "OK"
                            Else
                                Result_ALL = "NG"
                            End If
                            _Dataset.Dispose()
                            DataBase_ConnectionAdapter.Dispose()
                            DataBase_Com.Dispose()
                            '更新总结果
                            DataBase_Com = New SqlCommand("UPDATE " & Table_Name & " SET MC_Result = N'" & Result_ALL & "', Final_Result = N'" & Result_ALL & "'  WHERE ([Tray Code] = '" & BarCode & "') AND ([Key Name] = '" & Key_Name & "')", DataBase_Connection)
                            DataBase_Com.ExecuteNonQuery()
                            DataBase_Com.Dispose()
                        End If
                        ret = True
                End Select
            Catch ex As Exception
                Write_Log(ex.Message)
                ret = False
            End Try
            Return ret
        End Function

        Function Delete_Measure_Result(ByVal Table_Name As String, ByVal BarCode As String) As Boolean
            Dim ret As Boolean = False
            If BarCode Is Nothing Then
                BarCode = "NULL"
            ElseIf BarCode = "" Then
                BarCode = "NULL"
            End If

            Dim DataBase_Com As SqlCommand
            Try
                Select Case DataBase_Link_Boolean
                    Case True
                        DataBase_Com = New SqlCommand("DELETE FROM " & Table_Name & " WHERE [Tray Code] = N'" & BarCode & "'", DataBase_Connection)
                        DataBase_Com.ExecuteNonQuery()
                        DataBase_Com.Dispose()
                        ret = True
                End Select
            Catch ex As Exception
                Write_Log(ex.Message)
                ret = False
            End Try
            Return ret
        End Function

        Function Read_Measure_Result(ByVal SN As String, ByRef Read_Message_DataSet As DataSet, ByVal Table_Name As String) As Boolean
            Dim ret As Boolean = False
            Dim SubName = ErrName & GetCurrentMethod.Name & ":"
            Try

                Select Case DataBase_Link_Boolean
                    Case True
                        Read_Message_DataSet = New DataSet
                        Read_Message_DataSet.Clear()
                        Dim DataBase_ConnectionAdapter As SqlDataAdapter
                        Dim DataBase_Com As New SqlCommand
                        '无Time
                        DataBase_Com.CommandText = "SELECT [Product ID], [Machine No], [Project Name], [Tray Code], Language, Layout,[Key No], [Key Type], [Key Name], LightLeakage_Left_Result, LightLeakage_Up_Result,LightLeakage_Right_Result, LightLeakage_Down_Result, Snap01_Result, Snap02_Result, Snap03_Result, Snap04_Result, Hook01_Result, Hook02_Result, Hook03_Result,Hook04_Result, GateVestige01_Result, GateVestige02_Result, GateVestige03_Result, LightLeakageResult, HookSnapResult, MC_Result, OP_Rejudge,Final_Result FROM " & Table_Name & " WHERE [Tray Code]=N'" & SN & "' ORDER BY ID ASC"
                        DataBase_ConnectionAdapter = New SqlDataAdapter(DataBase_Com.CommandText, DataBase_Connection)
                        DataBase_ConnectionAdapter.Fill(Read_Message_DataSet)

                        DataBase_ConnectionAdapter.Dispose()
                        DataBase_Com.Dispose()
                        ret = True
                End Select
            Catch ex As Exception
                Write_Log(SubName & ex.Message)
                ret = False
            End Try
            Return ret
        End Function


        ''' <summary>
        ''' 更新产品ID
        ''' </summary>
        ''' <param name="Table_Name"></param>
        ''' <param name="Tray_Code"></param>
        ''' <param name="Product_ID"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function Updata_Product_ID(ByVal Table_Name As String, ByVal Tray_Code As String, ByVal Product_ID As String) As Boolean
            Dim ret As Boolean = False
            Try
                Select Case DataBase_Link_Boolean
                    Case True
                        Dim DataBase_Cmd As SqlCommand
                        DataBase_Cmd = New SqlCommand("UPDATE " & Table_Name & " SET  [Product ID] ='" & Product_ID & "'  WHERE ([Tray Code] = N'" & Tray_Code & "')", DataBase_Connection)
                        Dim rtn As Integer = DataBase_Cmd.ExecuteNonQuery()
                        DataBase_Cmd.Dispose()
                        ret = True
                End Select
            Catch ex As Exception
                Write_Log(ex.Message)
                ret = False
            End Try
            Return ret
        End Function

        ''' <summary>
        ''' 更新产品参数
        ''' </summary>
        ''' <param name="Parameter_Name">项目参数</param>
        ''' <param name="Parameter_Value">参数变量</param>
        ''' <remarks></remarks>
        Function Update_Project_Parameter(ByVal Parameter_Name() As String, ByVal Parameter_Value() As Object, Optional ByVal ID As Integer = 1, Optional ByVal IO_Boolean As Boolean = False, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Boolean
            Dim ret As Boolean = False
            Try
                SubName = ErrName & GetCurrentMethod.Name & ":"
                Dim DataBase_Cmd As New SqlCommand
                Select Case DataBase_Link_Boolean
                    Case True
                        Dim Parameter_Count As Integer = Parameter_Name.Length
                        Dim Total_Parameter_Name_Str As String = Nothing
                        For i As Integer = 0 To Parameter_Count - 1
                            Select Case i
                                Case Parameter_Count - 1
                                    Select Case IsNumeric(Parameter_Value(i))
                                        Case True
                                            Total_Parameter_Name_Str = Total_Parameter_Name_Str & Parameter_Name(i) & " = " & Parameter_Value(i)
                                        Case False
                                            Total_Parameter_Name_Str = Total_Parameter_Name_Str & Parameter_Name(i) & " = '" & Parameter_Value(i) & "'"
                                    End Select
                                Case Else
                                    Select Case IsNumeric(Parameter_Value(i))
                                        Case True
                                            Total_Parameter_Name_Str = Total_Parameter_Name_Str & Parameter_Name(i) & " = " & Parameter_Value(i) & ", "
                                        Case False
                                            Total_Parameter_Name_Str = Total_Parameter_Name_Str & Parameter_Name(i) & " = '" & Parameter_Value(i) & "',"
                                    End Select
                            End Select
                        Next
                        Select Case IO_Boolean
                            Case True
                                DataBase_Cmd = New SqlCommand("UPDATE HARDWARE_PARAMETERS SET " & Total_Parameter_Name_Str & " WHERE (ID =" & ID & ")", DataBase_Connection)
                                DataBase_Cmd.ExecuteNonQuery()
                            Case False
                                DataBase_Cmd = New SqlCommand("UPDATE DEVICE_PARAMETERS SET " & Total_Parameter_Name_Str & " WHERE (ID =" & ID & ")", DataBase_Connection)
                                DataBase_Cmd.ExecuteNonQuery()
                        End Select
                        ret = True
                End Select
            Catch ex As Exception


                Write_Log(SubName & ex.Message)
                ret = False
            End Try
            Return ret
        End Function

    End Class

    Public Class SQL_LIB
        Private SysInit As New SysInit.SysInit
        Public HomeParamObj As SQL_LIB.HOME_PARAMETERS
        Public Structure HOME_PARAMETERS
            Dim ID As Integer
            Dim 轴名称 As String
            Dim 轴号 As Integer
            Dim 导程 As Double
            Dim 回原点模式 As String
            Dim 回原点搜索方向 As String
            Dim 回原点Z相信号 As String
            Dim 回原点曲线 As String
            Dim 回原点速度 As Integer
            Dim 回原点加减速度 As Double
            Dim 回原点偏移 As Integer
            Dim 回原点顺序 As Integer
        End Structure

        Public DataBase_Data_Souce As String = "127.0.0.1"
        Public DataBase_ID As String = "SA"
        Public DataBase_PassWord As String = "123"
        Public DataBase_Catalog_Name As String = "GENERAL_ASSEMBLY"

        Dim DataBase_Link_Boolean As Boolean
        Dim Soft_Default_Path As String = IO.Directory.GetCurrentDirectory & "\"
        Dim DataBase_Connection As New SqlConnection
        Dim DataBase_Connection_2 As New SqlConnection
        Dim DataBase_Command As New SqlCommand

        Dim DataBase_DataReader As SqlDataReader
        Dim DataBase_ConnectionAdapter As SqlDataAdapter
        Dim Sw As IO.StreamWriter
        Dim SqlServerName As String
        Dim InitialCatalogName As String


        Dim callback As New AsyncCallback(AddressOf HandleCallback)
        Private Sub HandleCallback1(ByVal result As IAsyncResult)
            Try
                Dim command As SqlCommand = CType(result.AsyncState, SqlCommand)
                Dim rowCount As Integer = command.EndExecuteNonQuery(result)

            Catch ex As Exception
            End Try
        End Sub
        Dim callback1 As New AsyncCallback(AddressOf HandleCallback1)
        Private Sub HandleCallback(ByVal result As IAsyncResult)
            Try
                Dim command As SqlCommand = CType(result.AsyncState, SqlCommand)
                Dim rowCount As Integer = command.EndExecuteNonQuery(result)

            Catch ex As Exception
            End Try
        End Sub


        Dim callback_Left As New AsyncCallback(AddressOf HandleCallback_Left)
        Private Sub HandleCallback_Left(ByVal result As IAsyncResult)
            Try
                Dim command As SqlCommand = CType(result.AsyncState, SqlCommand)
                Dim rowCount As Integer = command.EndExecuteNonQuery(result)
            Catch ex As Exception
            End Try
        End Sub

        Dim callback_Right As New AsyncCallback(AddressOf HandleCallback_Right)
        Private Sub HandleCallback_Right(ByVal result As IAsyncResult)
            Try
                Dim command As SqlCommand = CType(result.AsyncState, SqlCommand)
                Dim rowCount As Integer = command.EndExecuteNonQuery(result)
            Catch ex As Exception
            End Try
        End Sub

        Public Structure USER_PARAMETER_STRUCTURE
            Dim 坐标添加 As Boolean
            Dim 坐标更新 As Boolean
            Dim 硬件配置 As Boolean
            Dim 功能设置 As Boolean
            Dim 参数设置 As Boolean
            Dim 程式切换 As Boolean
            Dim 手动操作 As Boolean
            Dim 单步执行 As Boolean
            Dim 增加用户 As Boolean
            Dim 删除用户 As Boolean
            Dim 点胶设置 As Boolean
        End Structure

        Sub Write_Log(ByVal Message As String)
            'MessageBox.Show(Message & vbCrLf & "以上错误如自行不能解决请联系软件工程师！", "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)

            Dim File_Path As String = Nothing
            Dim MON As String = Date.Now.Month.ToString.PadLeft(2).Replace(" ", "0") & " MONTH\"
            Dim DAY As String = Date.Now.Day.ToString.PadLeft(2).Replace(" ", "0") & " DAY\"
            File_Path = "C:\" & MON & DAY
            If System.IO.Directory.Exists(File_Path) = False Then
                System.IO.Directory.CreateDirectory(File_Path)
            End If

            Sw = New IO.StreamWriter(File_Path & "SystemLog.txt", True, Encoding.Default)
            Sw.WriteLine(Date.Now & "," & Message)
            Sw.Flush()
            Sw.Close()
            Sw.Dispose()
        End Sub

        ''' <summary>
        ''' 初始化数据库
        ''' </summary>
        ''' <param name="Server_ADD">数据库地址</param>
        ''' <param name="Use_ID">数据库用户名</param>
        ''' <param name="Pass_Word">数据库密码，缺省无密码</param>
        ''' <param name="Initial_Catalog_Name">数据库名称</param>
        ''' <returns>返回值：TRUE（初始化成功），FALSE（初始化失败）</returns>
        ''' <param name="TimeOut">超时</param>
        ''' <remarks></remarks>
        Function DataBase_Initialization(Optional ByVal Server_Add As String = "127.0.0.1", Optional ByVal Use_ID As String = "SA", Optional ByVal Pass_Word As String = Nothing, Optional ByVal Initial_Catalog_Name As String = Nothing, Optional ByVal TimeOut As Integer = 50, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Boolean
            Dim ret As Boolean = False
            Return False

            Try
                SubName = ErrName & GetCurrentMethod.Name & ":"
                Select Case DataBase_Link_Boolean
                    Case False
                        DataBase_Connection.ConnectionString = "Data Source=" & Server_Add & ";Initial Catalog=" & Initial_Catalog_Name & ";User ID=" & Use_ID & ";Password=" & Pass_Word & ";Connect Timeout=" & TimeOut & ";Asynchronous Processing=True;MultipleActiveResultSets=True"
                        DataBase_Connection_2.ConnectionString = "Data Source=" & Server_Add & ";Initial Catalog=" & Initial_Catalog_Name & ";User ID=" & Use_ID & ";Password=" & Pass_Word & ";Connect Timeout=" & TimeOut & ";Asynchronous Processing=True;MultipleActiveResultSets=True"

                        If DataBase_Connection.State = ConnectionState.Closed Then
                            DataBase_Connection.Open()
                            ret = True
                            DataBase_Link_Boolean = True
                        Else
                            ret = False
                        End If
                        If DataBase_Connection_2.State = ConnectionState.Closed Then
                            DataBase_Connection_2.Open()
                            ret = True
                            DataBase_Link_Boolean = True
                        Else
                            ret = False
                        End If
                    Case True
                End Select
            Catch ex As Exception
               

                Write_Log(SubName & ex.Message)
                DataBase_Link_Boolean = False
                ret = False
            End Try
            Return ret
        End Function

        ''' <summary>
        ''' 关闭数据库
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function Close_DataBase(Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Boolean
            Dim ret As Boolean = False
            Try
                SubName = ErrName & GetCurrentMethod.Name & ":"
                If DataBase_Connection.State = ConnectionState.Open Then
                    DataBase_Connection.Close()
                    DataBase_Connection.Dispose()
                    DataBase_Link_Boolean = False
                    ret = True
                Else
                    ret = False
                End If
                If DataBase_Connection_2.State = ConnectionState.Open Then
                    DataBase_Connection_2.Close()
                    DataBase_Connection_2.Dispose()
                    DataBase_Link_Boolean = False
                    ret = True
                Else
                    ret = False
                End If
            Catch ex As Exception
               

                Write_Log(SubName & ex.Message)
                ret = False
            End Try
            Return ret
        End Function


        Function Exits_Measure_Result(ByVal Table_Name As String, ByVal BarCode As String) As Boolean
            Dim ret As Boolean = False
            If BarCode Is Nothing Then
                BarCode = "NULL"
            ElseIf BarCode = "" Then
                BarCode = "NULL"
            End If

            Dim DataBase_Com As SqlCommand
            Try
                Select Case DataBase_Link_Boolean
                    Case True
                        DataBase_Com = New SqlCommand("SELECT COUNT(ID) AS 统计 FROM " & Table_Name & " WHERE [Tray Code] = N'" & BarCode & "'", DataBase_Connection)
                        Dim Select_Count As Integer = DataBase_Com.ExecuteScalar
                        DataBase_Com.Dispose()

                        If Select_Count <= 0 Then
                            ret = False
                        Else
                            ret = True
                        End If
                        Try
                            Write_Log("Exits_Measure_Result 查询上一站数据成功(SN:" & BarCode & "),查询到数据条数：" & Select_Count)
                        Catch ex As Exception

                        End Try

                End Select
            Catch ex As Exception
                Write_Log("Exits_Measure_Result报错，错误信息：" & ex.Message)
                ret = False
            End Try
            Return ret
        End Function

        Function Updata_NGCount_Result(ByVal Table_Name As String, ByVal LightLeakage_Key_NgCount As Integer, ByVal HookSnap_key_NgCount As Integer, ByVal ID As Integer) As Boolean
            Dim ret As Boolean = False, SubName As String = Nothing
            Try
                SubName = ErrName & GetCurrentMethod.Name & ":"
                Select Case DataBase_Link_Boolean
                    Case True
                        Dim DataBase_Com As SqlCommand

                        DataBase_Com = New SqlCommand("UPDATE " & Table_Name & " SET LightLeakage_Key_NgCount = N'" & LightLeakage_Key_NgCount & "', HookSnap_key_NgCount = N'" & HookSnap_key_NgCount & "'  WHERE (ID = '" & ID & "')", DataBase_Connection)
                        DataBase_Com.ExecuteNonQuery()
                        DataBase_Com.Dispose()

                        ret = True
                End Select
            Catch ex As Exception
                Write_Log(SubName & ex.Message)
                ret = False
            End Try
            Return ret
        End Function

        Function Delete_Table_Name(ByVal Table_Name As String) As Boolean
            Dim ret As Boolean = False
            Dim DataBase_Com As SqlCommand
            Try
                Select Case DataBase_Link_Boolean
                    Case True
                        DataBase_Com = New SqlCommand("DELETE FROM " & Table_Name & "", DataBase_Connection)
                        DataBase_Com.ExecuteNonQuery()
                        DataBase_Com.Dispose()
                        ret = True
                End Select
            Catch ex As Exception
                Write_Log(ex.Message)
                ret = False
            End Try
            Return ret
        End Function

        Function Delete_Measure_Result(ByVal Table_Name As String, ByVal BarCode As String) As Boolean
            Dim ret As Boolean = False
            If BarCode Is Nothing Then
                BarCode = "NULL"
            ElseIf BarCode = "" Then
                BarCode = "NULL"
            End If

            Dim DataBase_Com As SqlCommand
            Try
                Select Case DataBase_Link_Boolean
                    Case True
                        DataBase_Com = New SqlCommand("DELETE FROM " & Table_Name & " WHERE [Tray Code] = N'" & BarCode & "'", DataBase_Connection)
                        DataBase_Com.ExecuteNonQuery()
                        DataBase_Com.Dispose()
                        ret = True
                End Select
            Catch ex As Exception
                Write_Log(ex.Message)
                ret = False
            End Try
            Return ret
        End Function


      Function Read_Measure_Result(ByVal SN As String, ByRef Read_Message_DataSet As DataSet, ByVal Table_Name As String) As Boolean
            Dim ret As Boolean = False
            Dim SubName = ErrName & GetCurrentMethod.Name & ":"
            Try

                Select Case DataBase_Link_Boolean
                    Case True
                        Read_Message_DataSet = New DataSet
                        Read_Message_DataSet.Clear()
                        Dim DataBase_ConnectionAdapter As SqlDataAdapter
                        Dim DataBase_Com As New SqlCommand
                        '无Time
                        DataBase_Com.CommandText = "SELECT [Product ID], [Machine No], [Project Name], [Tray Code], Language, Layout,[Key No], [Key Type], [Key Name], LightLeakage_Left_Result, LightLeakage_Up_Result,LightLeakage_Right_Result, LightLeakage_Down_Result, Snap01_Result, Snap02_Result, Snap03_Result, Snap04_Result, Hook01_Result, Hook02_Result, Hook03_Result,Hook04_Result, GateVestige01_Result, GateVestige02_Result, GateVestige03_Result, LightLeakageResult, HookSnapResult, MC_Result, OP_Rejudge,Final_Result FROM " & Table_Name & " WHERE [Tray Code]=N'" & SN & "' ORDER BY ID ASC"
                        DataBase_ConnectionAdapter = New SqlDataAdapter(DataBase_Com.CommandText, DataBase_Connection)
                        DataBase_ConnectionAdapter.Fill(Read_Message_DataSet)

                        DataBase_ConnectionAdapter.Dispose()
                        DataBase_Com.Dispose()
                        ret = True
                End Select
            Catch ex As Exception
                Write_Log(SubName & ex.Message)
                ret = False
            End Try
            Return ret
        End Function

        ''' <summary>
        ''' 插入测量结果
        ''' </summary>
        ''' <param name="Table_Name">表明</param>
        ''' <param name="Test_Time">测试时间</param>
        ''' <param name="BarCode">条码</param>
        ''' <param name="Key_Name">键名</param>
        ''' <param name="Parameter_Name">参数名</param>
        ''' <param name="Parameter_Value">参数值</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function Insert_Measure_Result(ByVal Table_Name As String, ByVal Test_Time As String, ByVal BarCode As String, ByVal Key_Name As String, ByVal Parameter_Name() As String, ByVal Parameter_Value() As Object, ByRef Result_ALL As String, ByRef LightLakageResult As String, ByRef HookSnapeResult As String) As Boolean
            Dim ret As Boolean = False
            If BarCode Is Nothing Then
                BarCode = "NULL"
            ElseIf BarCode = "" Then
                BarCode = "NULL"
            End If
            If Key_Name Is Nothing Then
                Key_Name = "NULL"
            ElseIf Key_Name = "" Then
                Key_Name = "NULL"
            End If

            Dim DataBase_Com As SqlCommand
            Try
                Select Case DataBase_Link_Boolean
                    Case True
                        DataBase_Com = New SqlCommand("SELECT COUNT(ID) AS 统计 FROM " & Table_Name & " WHERE ([Tray Code] = N'" & BarCode & "') AND ([Key Name] = N'" & Key_Name & "')", DataBase_Connection)
                        Dim Select_Count As Integer = DataBase_Com.ExecuteScalar
                        DataBase_Com.Dispose()

                        Thread.Sleep(1)

                        If Select_Count <= 0 Then '插入新值
                            Write_Log("Insert_Measure_Result:" & Date.Now & "SQLCMD:" & DataBase_Com.CommandText & "rtn:" & Select_Count)
                            Return False
                        Else '更新
                            Dim Parameter_Count As Integer = Parameter_Name.Length
                            Dim Total_Parameter_Name_Str As String = "Time = " & "CONVERT(DATETIME, '" & Test_Time & "')" & ","
                            For i As Integer = 0 To Parameter_Count - 1
                                Select Case i
                                    Case Parameter_Count - 1
                                        Select Case IsNumeric(Parameter_Value(i))
                                            Case True
                                                Total_Parameter_Name_Str = Total_Parameter_Name_Str & Parameter_Name(i) & " = " & Parameter_Value(i)
                                            Case False
                                                Total_Parameter_Name_Str = Total_Parameter_Name_Str & Parameter_Name(i) & " = '" & Parameter_Value(i) & "'"
                                        End Select
                                    Case Else
                                        Select Case IsNumeric(Parameter_Value(i))
                                            Case True
                                                Total_Parameter_Name_Str = Total_Parameter_Name_Str & Parameter_Name(i) & " = " & Parameter_Value(i) & ", "
                                            Case False
                                                Total_Parameter_Name_Str = Total_Parameter_Name_Str & Parameter_Name(i) & " = '" & Parameter_Value(i) & "',"
                                        End Select
                                End Select
                            Next
                            DataBase_Com = New SqlCommand("UPDATE " & Table_Name & " SET " & Total_Parameter_Name_Str & " WHERE ([Tray Code] = '" & BarCode & "') AND ([Key Name] = '" & Key_Name & "')", DataBase_Connection)
                            DataBase_Com.ExecuteNonQuery()
                            DataBase_Com.Dispose()

                            '统计总结果
                            Dim _Dataset As New DataSet
                            Dim DataBase_ConnectionAdapter As SqlDataAdapter
                            _Dataset.Clear()
                            DataBase_Com = New SqlCommand("SELECT LightLeakageResult, HookSnapResult FROM " & Table_Name & " WHERE ([Tray Code] = N'" & BarCode & "') AND ([Key Name] = '" & Key_Name & "')", DataBase_Connection)
                            DataBase_ConnectionAdapter = New SqlDataAdapter(DataBase_Com.CommandText, DataBase_Connection)
                            DataBase_ConnectionAdapter.Fill(_Dataset)

                            If IsDBNull(_Dataset.Tables(0).Rows(0).Item(0)) = True Then
                                LightLakageResult = "NG"
                            Else
                                LightLakageResult = CType(_Dataset.Tables(0).Rows(0).Item(0), String).ToString.Trim
                            End If
                            If IsDBNull(_Dataset.Tables(0).Rows(0).Item(1)) = True Then
                                HookSnapeResult = "NG"
                            Else
                                HookSnapeResult = CType(_Dataset.Tables(0).Rows(0).Item(1), String).ToString.Trim
                            End If
                            If LightLakageResult = "OK" And HookSnapeResult = "OK" Then
                                Result_ALL = "OK"
                            Else
                                Result_ALL = "NG"
                            End If
                            _Dataset.Dispose()
                            DataBase_ConnectionAdapter.Dispose()
                            DataBase_Com.Dispose()
                            '更新总结果
                            DataBase_Com = New SqlCommand("UPDATE " & Table_Name & " SET MC_Result = N'" & Result_ALL & "', Final_Result = N'" & Result_ALL & "'  WHERE ([Tray Code] = '" & BarCode & "') AND ([Key Name] = '" & Key_Name & "')", DataBase_Connection)
                            DataBase_Com.ExecuteNonQuery()
                            DataBase_Com.Dispose()
                        End If
                        ret = True
                End Select
            Catch ex As Exception
                Write_Log(ex.Message)
                ret = False
            End Try
            Return ret
        End Function

        Function Delete_Debug(ByVal Tab_Name As String) As Boolean
            Dim SubName As String = GetCurrentMethod.Name
            Try
                Select Case DataBase_Link_Boolean
                    Case True
                        Dim DataBase_Com As SqlCommand
                        DataBase_Com = New SqlCommand("DELETE FROM " & Tab_Name & "", DataBase_Connection)
                        DataBase_Com.ExecuteScalar()
                        DataBase_Com.Dispose()
                End Select
            Catch ex As Exception
                Write_Log(SubName & ex.Message)
                Return False
            End Try
        End Function

        ''' <summary>
        ''' 删除图片
        ''' </summary>
        ''' <param name="BarCode"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function Delete_Image_Left(ByVal Table_Name As String, ByVal BarCode As String) As Boolean
            Dim ret As Boolean = False
            Try
                Select Case DataBase_Link_Boolean
                    Case True
                        Dim DataBase_Cmd As SqlCommand
                        DataBase_Cmd = New SqlCommand("SELECT COUNT( ID) FROM " & Table_Name & " WHERE 键盘条码=N'" & BarCode & "'", DataBase_Connection)
                        Dim RTN As Integer = DataBase_Cmd.ExecuteScalar()
                        DataBase_Cmd.Dispose()

                        If RTN > 0 Then
                            DataBase_Cmd = New SqlCommand("DELETE  FROM " & Table_Name & "  WHERE 键盘条码=N'" & BarCode & "'", DataBase_Connection)
                            DataBase_Cmd.ExecuteNonQuery()
                            DataBase_Cmd.Dispose()
                        End If
                        ret = True
                End Select
            Catch ex As Exception
                Write_Log(ex.Message)
                ret = False
            End Try
            Return ret
        End Function

        ''' <summary>
        ''' 删除图片
        ''' </summary>
        ''' <param name="BarCode"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function Delete_Image_Right(ByVal Table_Name As String, ByVal BarCode As String) As Boolean
            Dim ret As Boolean = False
            Try
                Select Case DataBase_Link_Boolean
                    Case True
                        Dim DataBase_Cmd As SqlCommand
                        DataBase_Cmd = New SqlCommand("SELECT COUNT( ID) FROM " & Table_Name & " WHERE 键盘条码=N'" & BarCode & "'", DataBase_Connection)
                        Dim RTN As Integer = DataBase_Cmd.ExecuteScalar()
                        DataBase_Cmd.Dispose()

                        If RTN > 0 Then
                            DataBase_Cmd = New SqlCommand("DELETE  FROM " & Table_Name & "  WHERE 键盘条码=N'" & BarCode & "'", DataBase_Connection)
                            DataBase_Cmd.ExecuteNonQuery()
                            DataBase_Cmd.Dispose()
                        End If
                        ret = True
                End Select
            Catch ex As Exception
                Write_Log(ex.Message)
                ret = False
            End Try
            Return ret
        End Function

        ''' <summary>
        ''' 插入图片
        ''' </summary>
        ''' <param name="Project_Name"></param>
        ''' <param name="BarCode"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function Insert_Image_Left(ByVal TABLE_NAME As String, ByVal Project_Name As String, ByVal BarCode As String, ByVal Key_Name As String, ByVal ImageByte() As Byte, ByVal Measure_Result As String) As Boolean
            Dim ret As Boolean = False
            Try
                Select Case DataBase_Link_Boolean
                    Case True
                        Dim DataBase_Cmd_Left As SqlCommand
                        DataBase_Cmd_Left = New SqlCommand("INSERT INTO " & TABLE_NAME & " (项目名称, 键盘条码, 键帽名称, 测量日期, 卡扣测量图片, 测量结果) VALUES (N'" & Project_Name & "', N'" & BarCode & "' , N'" & Key_Name & "' , CONVERT(DATETIME, '" & Date.Now & "', 102), @Image, N'" & Measure_Result & "')", DataBase_Connection)
                        DataBase_Cmd_Left.Parameters.Add("@Image", SqlDbType.Image).Value = ImageByte
                        DataBase_Cmd_Left.ExecuteNonQuery()
                        DataBase_Cmd_Left.Dispose()
                        ImageByte = Nothing
                        ret = True
                End Select
            Catch ex As Exception
                Write_Log(ex.Message)
                ret = False
            End Try
            Return ret
        End Function

        ''' <summary>
        ''' 插入图片
        ''' </summary>
        ''' <param name="Project_Name"></param>
        ''' <param name="BarCode"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function Insert_Image_Right(ByVal TABLE_NAME As String, ByVal Project_Name As String, ByVal BarCode As String, ByVal Key_Name As String, ByVal ImageByte() As Byte, ByVal Measure_Result As String) As Boolean
            Dim ret As Boolean = False
            Try
                Select Case DataBase_Link_Boolean
                    Case True

                        Dim DataBase_Cmd_Right As SqlCommand
                        DataBase_Cmd_Right = New SqlCommand("INSERT INTO " & TABLE_NAME & " (项目名称, 键盘条码, 键帽名称, 测量日期, 卡扣测量图片, 测量结果) VALUES (N'" & Project_Name & "', N'" & BarCode & "' , N'" & Key_Name & "' , CONVERT(DATETIME, '" & Date.Now & "', 102), @Image, N'" & Measure_Result & "')", DataBase_Connection)
                        DataBase_Cmd_Right.Parameters.Add("@Image", SqlDbType.Image).Value = ImageByte
                        DataBase_Cmd_Right.ExecuteNonQuery()
                        DataBase_Cmd_Right.Dispose()
                        ImageByte = Nothing
                        ret = True
                End Select
            Catch ex As Exception
                Write_Log(ex.Message)
                ret = False
            End Try
            Return ret
        End Function

        Function Insert_Measure_data_Left(ByVal Project_Name As String, ByVal BarCode As String, ByVal Key_Name As String, ByVal Parameter_Name() As String, ByVal Parameter_Value() As Object) As Boolean
            Dim ret As Boolean = False
            Dim DataBase_Com As New SqlCommand
            Try
                Select Case DataBase_Link_Boolean
                    Case True
                        Dim Parameter_Count As Integer = Parameter_Name.Length
                        Dim Parameter_Total_Str As String = Nothing
                        Dim Parameter_Value_Total_Str As String = Nothing
                        For i As Integer = 0 To Parameter_Count - 1
                            Select Case i
                                Case Parameter_Count - 1
                                    Parameter_Total_Str = Parameter_Total_Str & Parameter_Name(i)
                                    If Parameter_Value(i).GetType.ToString.ToUpper = "SYSTEM.STRING" Then
                                        If Parameter_Value(i) = "" Then
                                            Parameter_Value(i) = "N/A"
                                        End If
                                        Parameter_Value_Total_Str = Parameter_Value_Total_Str & "'" & Parameter_Value(i) & " '"
                                    Else
                                        Parameter_Value_Total_Str = Parameter_Value_Total_Str & Parameter_Value(i)
                                    End If
                                Case Else
                                    Parameter_Total_Str = Parameter_Total_Str & Parameter_Name(i) & ","
                                    If Parameter_Value(i).GetType.ToString.ToUpper = "SYSTEM.STRING" Then
                                        If Parameter_Value(i) = "" Then
                                            Parameter_Value(i) = "N/A"
                                        End If
                                        Parameter_Value_Total_Str = Parameter_Value_Total_Str & "'" & Parameter_Value(i) & "',"
                                    Else
                                        Parameter_Value_Total_Str = Parameter_Value_Total_Str & Parameter_Value(i) & ","
                                    End If
                            End Select
                        Next
                        DataBase_Com = New SqlCommand("INSERT INTO PROJECT_MEASURE_DATA (Project_Name, SN, Key_Name , " & Parameter_Total_Str & ") VALUES (N'" & Project_Name & "', N'" & BarCode & "', N'" & Key_Name & "', " & Parameter_Value_Total_Str & ")", DataBase_Connection)
                        Dim rtn As Integer = DataBase_Com.ExecuteNonQuery()
                        DataBase_Com.Dispose()
                        ret = True
                End Select
            Catch ex As Exception
                Write_Log(ex.Message)
                ret = False
            End Try
            Return ret
        End Function

        Function Insert_Measure_data_Right(ByVal Project_Name As String, ByVal BarCode As String, ByVal Key_Name As String, ByVal Parameter_Name() As String, ByVal Parameter_Value() As Object) As Boolean
            Dim ret As Boolean = False
            Dim DataBase_Com As SqlCommand
            Try
                Select Case DataBase_Link_Boolean
                    Case True
                        Dim Parameter_Count As Integer = Parameter_Name.Length
                        Dim Parameter_Total_Str As String = Nothing
                        Dim Parameter_Value_Total_Str As String = Nothing
                        For i As Integer = 0 To Parameter_Count - 1
                            Select Case i
                                Case Parameter_Count - 1
                                    Parameter_Total_Str = Parameter_Total_Str & Parameter_Name(i)
                                    If Parameter_Value(i).GetType.ToString.ToUpper = "SYSTEM.STRING" Then
                                        If Parameter_Value(i) = "" Then
                                            Parameter_Value(i) = "N/A"
                                        End If
                                        Parameter_Value_Total_Str = Parameter_Value_Total_Str & "'" & Parameter_Value(i) & " '"
                                    Else
                                        Parameter_Value_Total_Str = Parameter_Value_Total_Str & Parameter_Value(i)
                                    End If
                                Case Else
                                    Parameter_Total_Str = Parameter_Total_Str & Parameter_Name(i) & ","
                                    If Parameter_Value(i).GetType.ToString.ToUpper = "SYSTEM.STRING" Then
                                        If Parameter_Value(i) = "" Then
                                            Parameter_Value(i) = "N/A"
                                        End If
                                        Parameter_Value_Total_Str = Parameter_Value_Total_Str & "'" & Parameter_Value(i) & "',"
                                    Else
                                        Parameter_Value_Total_Str = Parameter_Value_Total_Str & Parameter_Value(i) & ","
                                    End If
                            End Select
                        Next
                        DataBase_Com = New SqlCommand("INSERT INTO PROJECT_MEASURE_DATA (Project_Name, SN, Key_Name , " & Parameter_Total_Str & ") VALUES (N'" & Project_Name & "', N'" & BarCode & "', N'" & Key_Name & "', " & Parameter_Value_Total_Str & ")", DataBase_Connection_2)
                        Dim rtn As Integer = DataBase_Com.ExecuteNonQuery()
                        DataBase_Com.Dispose()
                        ret = True
                End Select
            Catch ex As Exception
                Write_Log(ex.Message)
                ret = False
            End Try
            Return ret
        End Function


        ''' <summary>
        ''' 查询数据总数
        ''' </summary>
        ''' <param name="Project_Name"></param>
        ''' <param name="BarCode"></param>
        ''' <param name="Key_Name"></param>
        ''' <param name="Parameter_Value"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function Query_Data_Sum(ByVal Project_Name As String, ByVal BarCode As String, ByVal Key_Name As String, ByRef Parameter_Value() As Double) As Boolean
            Dim ret As Boolean = False
            Dim SubName As String = ErrName & GetCurrentMethod.Name & ":"
            Dim DataBase_Com As SqlCommand
            Try
                Select Case DataBase_Link_Boolean
                    Case True
                        Dim _Dataset As New DataSet
                        _Dataset.Clear()

                        DataBase_Com = New SqlCommand("SELECT SUM(Snap_01) AS Expr1, SUM(Snap_02) AS Expr2, SUM(Snap_03) AS Expr3, SUM(Snap_04) AS Expr4, SUM(Snap_05) AS Expr5, SUM(Snap_06) AS Expr6, SUM(Snap_07) AS Expr6, SUM(Snap_08) AS Expr6,SUM(Hook_01) AS Expr7, SUM(Hook_02) AS Expr8, SUM(Hook_03) AS Expr9, SUM(Hook_04) AS Expr10, SUM(Hook_05) AS Expr11, SUM(Hook_06) AS Expr12, SUM(Hook_07) AS Expr12, SUM(Hook_08) AS Expr12 FROM PROJECT_MEASURE_DATA WHERE (Project_Name = '" & Project_Name & "') AND (SN = '" & BarCode & "') AND (Key_Name = '" & Key_Name & "')", DataBase_Connection)
                        DataBase_ConnectionAdapter = New SqlDataAdapter(DataBase_Com.CommandText, DataBase_Connection)
                        DataBase_ConnectionAdapter.Fill(_Dataset)

                        Array.Resize(Parameter_Value, _Dataset.Tables(0).Columns.Count)
                        For i As Integer = 0 To _Dataset.Tables(0).Columns.Count - 1
                            If IsDBNull(_Dataset.Tables(0).Rows(0).Item(i)) = True Then
                                Parameter_Value(i) = 0
                            Else
                                Parameter_Value(i) = CType(_Dataset.Tables(0).Rows(0).Item(i), Double)
                            End If
                        Next
                        _Dataset.Dispose()
                        DataBase_Com.Dispose()
                        ret = True
                End Select
            Catch ex As Exception
                Write_Log(SubName & "：" & ex.Message)
                ret = False
            End Try
            Return ret
        End Function

        ''' <summary>
        ''' 读取密码
        ''' </summary>
        ''' <param name="UserName"></param>
        ''' <param name="LoginPass"></param>
        ''' <param name="ClearDataPass"></param>
        ''' <param name="DeleteProductPass"></param>
        ''' <param name="SubName"></param>
        ''' <param name="ErrLine"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function Read_User_Password(ByVal UserName As String, ByRef LoginPass As String, ByRef ClearDataPass As String, ByRef DeleteProductPass As String, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Boolean
            Dim ret As Boolean = False
            Try
                Select Case DataBase_Link_Boolean
                    Case True
                        Dim User_Password_DataSet As New DataSet
                        User_Password_DataSet.Clear()
                        DataBase_Command.CommandText = "SELECT DISTINCT 登陆密码, 清空数据密码, 删除项目密码 FROM  SYSTEM_PARAMETERS WHERE  (用户名称 = N'" & UserName & "')"
                        DataBase_ConnectionAdapter = New SqlDataAdapter(DataBase_Command.CommandText, DataBase_Connection)
                        DataBase_ConnectionAdapter.Fill(User_Password_DataSet)
                        If IsDBNull(User_Password_DataSet.Tables(0).Rows(0).Item("登陆密码")) = False Then
                            LoginPass = SysInit.ServiceDesDe(User_Password_DataSet.Tables(0).Rows(0).Item("登陆密码").ToString.Trim)
                        Else
                            LoginPass = Nothing
                        End If

                        If IsDBNull(User_Password_DataSet.Tables(0).Rows(0).Item("清空数据密码")) = False Then
                            ClearDataPass = SysInit.ServiceDesDe(User_Password_DataSet.Tables(0).Rows(0).Item("清空数据密码").ToString.Trim)
                        Else
                            ClearDataPass = Nothing
                        End If

                        If IsDBNull(User_Password_DataSet.Tables(0).Rows(0).Item("删除项目密码")) = False Then
                            DeleteProductPass = SysInit.ServiceDesDe(User_Password_DataSet.Tables(0).Rows(0).Item("删除项目密码").ToString.Trim)
                        Else
                            DeleteProductPass = Nothing
                        End If

                        User_Password_DataSet.Dispose()
                        DataBase_Command.Dispose()
                End Select
            Catch ex As Exception
               

                Write_Log(SubName & ex.Message)
                ret = False
            End Try
            Return ret
        End Function

        Function Update_User_Password(ByVal UserName As String, ByVal Password_Type As String, ByVal Password_Value As String, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Boolean
            Dim ret As Boolean = False

            Try
                Select Case DataBase_Link_Boolean
                    Case True
                        Password_Value = SysInit.ServiceDesEn(Password_Value)
                        DataBase_Command = New SqlCommand("UPDATE SYSTEM_PARAMETERS SET " & Password_Type & "  = '" & Password_Value & "' WHERE (用户名称 = N'" & UserName & "')", DataBase_Connection)
                        DataBase_Command.ExecuteNonQuery()
                        DataBase_Command.Dispose()
                        ret = True
                End Select
            Catch ex As Exception
               

                Write_Log(SubName & ex.Message)
                ret = False
            End Try
            Return ret
        End Function

        Sub Read_User_Parameter(ByVal User_Name() As String, ByRef User_Name_ComBoBox As DevComponents.DotNetBar.Controls.ComboBoxEx)
            Try
                Select Case DataBase_Link_Boolean
                    Case True
                        User_Name_ComBoBox.Items.Clear()
                        DataBase_Command.CommandText = "SELECT DISTINCT 用户名称, ID FROM SYSTEM_PARAMETERS WHERE (用户名称 IS NOT NULL) ORDER BY ID"
                        Dim User_Password_DataSet As New DataSet
                        User_Password_DataSet.Clear()
                        DataBase_ConnectionAdapter = New SqlDataAdapter(DataBase_Command.CommandText, DataBase_Connection)
                        DataBase_ConnectionAdapter.Fill(User_Password_DataSet)
                        Array.Resize(User_Name, User_Password_DataSet.Tables(0).Rows.Count)
                        For i As Integer = 0 To User_Password_DataSet.Tables(0).Rows.Count - 1
                            User_Name(i) = User_Password_DataSet.Tables(0).Rows(i).Item(0).ToString.Trim
                            User_Name_ComBoBox.Items.Add(User_Name(i))
                        Next
                        User_Name_ComBoBox.DropDownStyle = Windows.Forms.ComboBoxStyle.DropDownList
                        User_Name_ComBoBox.ForeColor = Color.Black
                        User_Password_DataSet.Dispose()
                        DataBase_Command.Dispose()
                End Select
            Catch ex As Exception
                Write_Log(ex.Message)
            End Try

        End Sub

        Sub Read_Home_ParamS(ByRef HOME_PARAMETERS() As HOME_PARAMETERS, Optional ByRef User_Name_DataGrid As DevComponents.DotNetBar.Controls.DataGridViewX = Nothing)
            Try
                Select Case DataBase_Link_Boolean
                    Case True

                        Dim User_Password_DataSet As New DataSet
                        User_Password_DataSet.Clear()
                        DataBase_Command.CommandText = "SELECT DISTINCT ID,轴名称,轴号,导程,回原点模式,回原点搜索方向,回原点Z相信号,回原点曲线,回原点速度,回原点加减速度,回原点偏移,回原点顺序 FROM HOME_PARAMETERS"
                        DataBase_ConnectionAdapter = New SqlDataAdapter(DataBase_Command.CommandText, DataBase_Connection)
                        DataBase_ConnectionAdapter.Fill(User_Password_DataSet)

                        Array.Resize(HOME_PARAMETERS, User_Password_DataSet.Tables(0).Rows.Count)
                        For I = 0 To User_Password_DataSet.Tables(0).Rows.Count - 1
                            HOME_PARAMETERS(I).ID = CType(User_Password_DataSet.Tables(0).Rows(I).Item(0), Integer)
                            HOME_PARAMETERS(I).轴名称 = CType(User_Password_DataSet.Tables(0).Rows(I).Item(1), String).ToString.Trim
                            HOME_PARAMETERS(I).轴号 = CType(User_Password_DataSet.Tables(0).Rows(I).Item(2), Integer)
                            HOME_PARAMETERS(I).导程 = CType(User_Password_DataSet.Tables(0).Rows(I).Item(3), Double)
                            HOME_PARAMETERS(I).回原点模式 = CType(User_Password_DataSet.Tables(0).Rows(I).Item(4), String).ToString.Trim
                            HOME_PARAMETERS(I).回原点搜索方向 = CType(User_Password_DataSet.Tables(0).Rows(I).Item(5), String).ToString.Trim
                            HOME_PARAMETERS(I).回原点Z相信号 = CType(User_Password_DataSet.Tables(0).Rows(I).Item(6), String).ToString.Trim
                            HOME_PARAMETERS(I).回原点曲线 = CType(User_Password_DataSet.Tables(0).Rows(I).Item(7), String).ToString.Trim
                            HOME_PARAMETERS(I).回原点速度 = CType(User_Password_DataSet.Tables(0).Rows(I).Item(8), Integer)
                            HOME_PARAMETERS(I).回原点加减速度 = CType(User_Password_DataSet.Tables(0).Rows(I).Item(9), Double)
                            HOME_PARAMETERS(I).回原点偏移 = CType(User_Password_DataSet.Tables(0).Rows(I).Item(10), Integer)
                            HOME_PARAMETERS(I).回原点顺序 = CType(User_Password_DataSet.Tables(0).Rows(I).Item(11), Integer)
                        Next

                        If User_Name_DataGrid IsNot Nothing Then
                            User_Name_DataGrid.Columns.Clear()
                            User_Name_DataGrid.DataSource = User_Password_DataSet.Tables(0)

                            For I = 0 To User_Name_DataGrid.Columns.Count - 1
                                User_Name_DataGrid.Columns(I).ReadOnly = True
                                User_Name_DataGrid.Columns(I).SortMode = Windows.Forms.DataGridViewColumnSortMode.NotSortable
                                Select Case I
                                    Case User_Name_DataGrid.Columns.Count - 1
                                        User_Name_DataGrid.Columns(I).AutoSizeMode = Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells
                                    Case Else
                                        User_Name_DataGrid.Columns(I).AutoSizeMode = Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells
                                End Select

                            Next
                            User_Name_DataGrid.RowHeadersVisible = False
                            User_Name_DataGrid.AllowUserToDeleteRows = False
                            User_Name_DataGrid.AllowUserToResizeColumns = False
                            User_Name_DataGrid.AllowUserToResizeRows = False
                            User_Name_DataGrid.ColumnHeadersHeightSizeMode = Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
                            User_Name_DataGrid.MultiSelect = False
                            User_Name_DataGrid.ReadOnly = True
                            User_Name_DataGrid.RowHeadersWidthSizeMode = Windows.Forms.DataGridViewRowHeadersWidthSizeMode.EnableResizing
                            User_Name_DataGrid.SelectionMode = Windows.Forms.DataGridViewSelectionMode.FullRowSelect
                        End If

                        User_Password_DataSet.Dispose()
                        DataBase_Command.Dispose()
                End Select
            Catch ex As Exception
                Write_Log(ex.Message)
            End Try
        End Sub

        Sub Read_Device_Parameter(ByVal Table_Name As String, Optional ByRef User_Name_DataGrid As DevComponents.DotNetBar.Controls.DataGridViewX = Nothing)
            Try
                Select Case DataBase_Link_Boolean
                    Case True
                        Dim User_Password_DataSet As New DataSet
                        User_Password_DataSet.Clear()
                        DataBase_Command.CommandText = "SELECT " & Table_Name & ".* FROM " & Table_Name & " ORDER BY ID"
                        DataBase_ConnectionAdapter = New SqlDataAdapter(DataBase_Command.CommandText, DataBase_Connection)
                        DataBase_ConnectionAdapter.Fill(User_Password_DataSet)

                        If User_Name_DataGrid IsNot Nothing Then
                            User_Name_DataGrid.Columns.Clear()
                            User_Name_DataGrid.DataSource = User_Password_DataSet.Tables(0)

                            For I = 0 To User_Name_DataGrid.Columns.Count - 1
                                If Table_Name = DEVICEPARAM_TABLENAME.DOUBLE_TYPE Or Table_Name = DEVICEPARAM_TABLENAME.INTEGER_TYPE Then
                                    If I >= User_Name_DataGrid.Columns.Count - 2 Then
                                        User_Name_DataGrid.Columns(I).Visible = False
                                    End If
                                End If
                                User_Name_DataGrid.Columns(I).ReadOnly = True
                                User_Name_DataGrid.Columns(I).SortMode = Windows.Forms.DataGridViewColumnSortMode.NotSortable
                                Select Case I
                                    Case 0
                                        User_Name_DataGrid.Columns(I).AutoSizeMode = Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
                                    Case User_Name_DataGrid.Columns.Count - 1
                                        User_Name_DataGrid.Columns(I).AutoSizeMode = Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
                                    Case Else
                                        User_Name_DataGrid.Columns(I).AutoSizeMode = Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
                                End Select
                            Next
                            User_Name_DataGrid.RowHeadersVisible = False
                            User_Name_DataGrid.AllowUserToDeleteRows = False
                            User_Name_DataGrid.AllowUserToResizeColumns = False
                            User_Name_DataGrid.AllowUserToResizeRows = False
                            User_Name_DataGrid.ColumnHeadersHeightSizeMode = Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
                            User_Name_DataGrid.MultiSelect = False
                            User_Name_DataGrid.ReadOnly = True
                            User_Name_DataGrid.RowHeadersWidthSizeMode = Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
                            User_Name_DataGrid.SelectionMode = Windows.Forms.DataGridViewSelectionMode.FullRowSelect
                        End If

                        User_Password_DataSet.Dispose()
                        DataBase_Command.Dispose()
                End Select
            Catch ex As Exception
                Write_Log(ex.Message)
            End Try
        End Sub

        Function Insert_Device_Param_String(ByVal Col_Name As String, ByVal Col_Value As String) As Boolean
            Try
                Select Case DataBase_Link_Boolean
                    Case True
                        DataBase_Command = New SqlCommand("INSERT INTO DEVICE_STRING_PARAMETERS(变量名称, 当前值)VALUES(N'" & Col_Name & "', '" & Col_Value & "')", DataBase_Connection)
                        DataBase_Command.ExecuteNonQuery()
                        DataBase_Command.Dispose()
                        Return True
                    Case Else
                        Return False
                End Select
            Catch ex As Exception
                Write_Log(ex.Message)
                Return False
            End Try
        End Function
        Function Insert_Device_Param_Bool(ByVal Col_Name As String, ByVal Col_Value As Integer) As Boolean
            Try
                Select Case DataBase_Link_Boolean
                    Case True
                        DataBase_Command = New SqlCommand("INSERT INTO DEVICE_BOOL_PARAMETERS(变量名称, 当前值)VALUES(N'" & Col_Name & "', " & Col_Value & ")", DataBase_Connection)
                        DataBase_Command.ExecuteNonQuery()
                        DataBase_Command.Dispose()
                        Return True
                    Case Else
                        Return False
                End Select
            Catch ex As Exception
                Write_Log(ex.Message)
                Return False
            End Try
        End Function
        Function Insert_Device_Param_Double(ByVal Col_Name As String, ByVal Col_Value As Double, ByVal Col_Value_Min As Double, ByVal Col_Value_Max As Double) As Boolean
            Try
                Select Case DataBase_Link_Boolean
                    Case True
                        DataBase_Command = New SqlCommand("INSERT INTO DEVICE_DOUBLE_PARAMETERS(变量名称, 当前值,最小值,最大值)VALUES(N'" & Col_Name & "', " & Col_Value & ", " & Col_Value_Min & ", " & Col_Value_Max & ")", DataBase_Connection)
                        DataBase_Command.ExecuteNonQuery()
                        DataBase_Command.Dispose()
                        Return True
                    Case Else
                        Return False
                End Select
            Catch ex As Exception
                Write_Log(ex.Message)
                Return False
            End Try
        End Function
        Function Insert_Device_Param_Integer(ByVal Col_Name As String, ByVal Col_Value As Integer, ByVal Col_Value_Min As Integer, ByVal Col_Value_Max As Integer) As Boolean
            Try
                Select Case DataBase_Link_Boolean
                    Case True
                        DataBase_Command = New SqlCommand("INSERT INTO DEVICE_INT_PARAMETERS(变量名称, 当前值,最小值,最大值)VALUES(N'" & Col_Name & "', " & Col_Value & ", " & Col_Value_Min & ", " & Col_Value_Max & ")", DataBase_Connection)
                        DataBase_Command.ExecuteNonQuery()
                        DataBase_Command.Dispose()
                        Return True
                    Case Else
                        Return False
                End Select
            Catch ex As Exception
                Write_Log(ex.Message)
                Return False
            End Try
        End Function

        Function Updata_Device_Param_Double(ByVal Col_Name As String, ByVal Col_Value As Double, ByVal Col_Value_Min As Double, ByVal Col_Value_Max As Double, ByVal F_ID As Int16) As Boolean
            Try
                Select Case DataBase_Link_Boolean
                    Case True
                        DataBase_Command = New SqlCommand("UPDATE DEVICE_DOUBLE_PARAMETERS SET 变量名称 ='" & Col_Name & "', 当前值 = " & Col_Value & ", 最小值 = " & Col_Value_Min & ", 最大值 = " & Col_Value_Max & " WHERE (ID = " & F_ID & ")", DataBase_Connection)
                        DataBase_Command.ExecuteNonQuery()
                        DataBase_Command.Dispose()
                        Return True
                    Case Else
                        Return False
                End Select
            Catch ex As Exception
                Write_Log(ex.Message)
                Return False
            End Try
        End Function
        Function Updata_Device_Param_Integer(ByVal Col_Name As String, ByVal Col_Value As Integer, ByVal Col_Value_Min As Integer, ByVal Col_Value_Max As Integer, ByVal F_ID As Int16) As Boolean
            Try
                Select Case DataBase_Link_Boolean
                    Case True
                        DataBase_Command = New SqlCommand("UPDATE DEVICE_INT_PARAMETERS SET 变量名称 ='" & Col_Name & "', 当前值 = " & Col_Value & ", 最小值 = " & Col_Value_Min & ", 最大值 = " & Col_Value_Max & " WHERE (ID = " & F_ID & ")", DataBase_Connection)
                        DataBase_Command.ExecuteNonQuery()
                        DataBase_Command.Dispose()
                        Return True
                    Case Else
                        Return False
                End Select
            Catch ex As Exception
                Write_Log(ex.Message)
                Return False
            End Try
        End Function
        Function Updata_Device_Param_String(ByVal Col_Name As String, ByVal Col_Value As String, ByVal F_ID As Int16) As Boolean
            Try
                Select Case DataBase_Link_Boolean
                    Case True
                        DataBase_Command = New SqlCommand("UPDATE DEVICE_STRING_PARAMETERS SET 变量名称 ='" & Col_Name & "', 当前值 ='" & Col_Value & "' WHERE (ID = " & F_ID & ")", DataBase_Connection)
                        DataBase_Command.ExecuteNonQuery()
                        DataBase_Command.Dispose()
                        Return True
                    Case Else
                        Return False
                End Select
            Catch ex As Exception
                Write_Log(ex.Message)
                Return False
            End Try
        End Function
        Function Updata_Device_Param_Bool(ByVal Col_Name As String, ByVal Col_Value As Integer, ByVal F_ID As Int16) As Boolean
            Try
                Select Case DataBase_Link_Boolean
                    Case True
                        DataBase_Command = New SqlCommand("UPDATE DEVICE_BOOL_PARAMETERS SET 变量名称 ='" & Col_Name & "', 当前值 =" & Col_Value & " WHERE (ID = " & F_ID & ")", DataBase_Connection)
                        DataBase_Command.ExecuteNonQuery()
                        DataBase_Command.Dispose()
                        Return True
                    Case Else
                        Return False
                End Select
            Catch ex As Exception
                Write_Log(ex.Message)
                Return False
            End Try
        End Function

        Sub Read_WORK_COORDINATE_PARAM(Optional ByRef User_Name_DataGrid As DevComponents.DotNetBar.Controls.DataGridViewX = Nothing)
            Try
                Select Case DataBase_Link_Boolean
                    Case True

                        Dim User_Password_DataSet As New DataSet
                        User_Password_DataSet.Clear()
                        DataBase_Command.CommandText = "SELECT DISTINCT ID,坐标ID,项目名称,坐标名称,轴名称,运行速度,加减速度,点胶状态,镭射触发状态,CCD触发状态,运动顺序 FROM WORK_COORDINATE_PARAM"
                        DataBase_ConnectionAdapter = New SqlDataAdapter(DataBase_Command.CommandText, DataBase_Connection)
                        DataBase_ConnectionAdapter.Fill(User_Password_DataSet)

                        If User_Name_DataGrid IsNot Nothing Then
                            User_Name_DataGrid.Columns.Clear()
                            User_Name_DataGrid.DataSource = User_Password_DataSet.Tables(0)

                            For I = 0 To User_Name_DataGrid.Columns.Count - 1
                                User_Name_DataGrid.Columns(I).ReadOnly = True
                                User_Name_DataGrid.Columns(I).SortMode = Windows.Forms.DataGridViewColumnSortMode.NotSortable
                                Select Case I
                                    Case User_Name_DataGrid.Columns.Count - 1
                                        User_Name_DataGrid.Columns(I).AutoSizeMode = Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
                                    Case Else
                                        User_Name_DataGrid.Columns(I).AutoSizeMode = Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
                                End Select

                            Next
                            User_Name_DataGrid.RowHeadersVisible = False
                            User_Name_DataGrid.AllowUserToDeleteRows = False
                            User_Name_DataGrid.AllowUserToResizeColumns = False
                            User_Name_DataGrid.AllowUserToResizeRows = False
                            User_Name_DataGrid.ColumnHeadersHeightSizeMode = Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
                            User_Name_DataGrid.MultiSelect = False
                            User_Name_DataGrid.ReadOnly = True
                            User_Name_DataGrid.RowHeadersWidthSizeMode = Windows.Forms.DataGridViewRowHeadersWidthSizeMode.EnableResizing
                            User_Name_DataGrid.SelectionMode = Windows.Forms.DataGridViewSelectionMode.FullRowSelect
                        End If

                        User_Password_DataSet.Dispose()
                        DataBase_Command.Dispose()
                End Select
            Catch ex As Exception
                Write_Log(ex.Message)
            End Try
        End Sub

        Sub Read_User_Parameter(ByVal User_Name() As String, ByVal Pass_Word() As String, ByVal User_Param() As USER_PARAMETER_STRUCTURE)
            Try
                Select Case DataBase_Link_Boolean
                    Case True
                        DataBase_Command.CommandText = "SELECT DISTINCT 用户名称, 用户密码, 坐标添加, 坐标更新, 硬件配置, 功能设置, 参数设置, 程式切换, 手动操作, 单步执行, 增加用户, 删除用户, 点胶设置 FROM SYSTEM_PARAMETERS WHERE ((NOT (ID = 5))"
                        Dim User_Password_DataSet As New DataSet
                        User_Password_DataSet.Clear()
                        DataBase_ConnectionAdapter = New SqlDataAdapter(DataBase_Command.CommandText, DataBase_Connection)
                        DataBase_ConnectionAdapter.Fill(User_Password_DataSet)
                        Array.Resize(User_Name, User_Password_DataSet.Tables(0).Rows.Count)
                        Array.Resize(Pass_Word, User_Password_DataSet.Tables(0).Rows.Count)
                        Array.Resize(User_Param, User_Password_DataSet.Tables(0).Rows.Count)
                        For i As Integer = 0 To User_Password_DataSet.Tables(0).Rows.Count - 1
                            User_Name(i) = User_Password_DataSet.Tables(0).Rows(i).Item(0).ToString.Trim
                            Pass_Word(i) = User_Password_DataSet.Tables(0).Rows(i).Item(1).ToString.Trim
                            User_Param(i).坐标添加 = CType(User_Password_DataSet.Tables(0).Rows(i).Item(2), Boolean)
                            User_Param(i).坐标更新 = CType(User_Password_DataSet.Tables(0).Rows(i).Item(3), Boolean)
                            User_Param(i).硬件配置 = CType(User_Password_DataSet.Tables(0).Rows(i).Item(4), Boolean)
                            User_Param(i).功能设置 = CType(User_Password_DataSet.Tables(0).Rows(i).Item(5), Boolean)
                            User_Param(i).参数设置 = CType(User_Password_DataSet.Tables(0).Rows(i).Item(6), Boolean)
                            User_Param(i).程式切换 = CType(User_Password_DataSet.Tables(0).Rows(i).Item(7), Boolean)
                            User_Param(i).手动操作 = CType(User_Password_DataSet.Tables(0).Rows(i).Item(8), Boolean)
                            User_Param(i).单步执行 = CType(User_Password_DataSet.Tables(0).Rows(i).Item(9), Boolean)
                            User_Param(i).增加用户 = CType(User_Password_DataSet.Tables(0).Rows(i).Item(10), Boolean)
                            User_Param(i).删除用户 = CType(User_Password_DataSet.Tables(0).Rows(i).Item(11), Boolean)
                            User_Param(i).点胶设置 = CType(User_Password_DataSet.Tables(0).Rows(i).Item(12), Boolean)
                        Next
                        User_Password_DataSet.Dispose()
                        DataBase_Command.Dispose()
                End Select
            Catch ex As Exception
                Write_Log(ex.Message)
            End Try

        End Sub

        Sub Read_User_Parameter(ByVal User_Name As String, ByVal User_Param_Switch() As DevComponents.DotNetBar.Controls.SwitchButton)
            Try
                Select Case DataBase_Link_Boolean
                    Case True
                        DataBase_Command.CommandText = "SELECT  坐标添加, 坐标更新, 硬件配置, 功能设置, 参数设置, 程式切换, 手动操作, 单步执行, 增加用户, 删除用户, 点胶设置 FROM SYSTEM_PARAMETERS WHERE 用户名称='" & User_Name & "'  ORDER BY ID"
                        Dim User_Password_DataSet As New DataSet
                        User_Password_DataSet.Clear()
                        DataBase_ConnectionAdapter = New SqlDataAdapter(DataBase_Command.CommandText, DataBase_Connection)
                        DataBase_ConnectionAdapter.Fill(User_Password_DataSet)
                        For i As Integer = 0 To User_Password_DataSet.Tables(0).Columns.Count - 1
                            If IsDBNull(User_Password_DataSet.Tables(0).Rows(0).Item(i)) = False Then
                                User_Param_Switch(i).Value = CType(User_Password_DataSet.Tables(0).Rows(0).Item(i), Boolean)
                            Else
                                User_Param_Switch(i).Value = False
                            End If
                        Next
                        User_Password_DataSet.Dispose()
                        DataBase_Command.Dispose()
                End Select
            Catch ex As Exception
                Write_Log(ex.Message)
            End Try

        End Sub

        Sub Read_User_Parameter(ByVal User_Name As String, ByRef User_Structure As USER_PARAMETER_STRUCTURE)
            Try
                Select Case DataBase_Link_Boolean
                    Case True
                        DataBase_Command.CommandText = "SELECT  坐标添加, 坐标更新, 硬件配置, 功能设置, 参数设置, 程式切换, 手动操作, 单步执行, 增加用户, 删除用户, 点胶设置 FROM SYSTEM_PARAMETERS WHERE 用户名称='" & User_Name & "'  ORDER BY ID"
                        Dim User_Password_DataSet As New DataSet
                        User_Password_DataSet.Clear()
                        DataBase_ConnectionAdapter = New SqlDataAdapter(DataBase_Command.CommandText, DataBase_Connection)
                        DataBase_ConnectionAdapter.Fill(User_Password_DataSet)
                        User_Structure.坐标添加 = CType(User_Password_DataSet.Tables(0).Rows(0).Item(0), Boolean)
                        User_Structure.坐标更新 = CType(User_Password_DataSet.Tables(0).Rows(0).Item(1), Boolean)
                        User_Structure.硬件配置 = CType(User_Password_DataSet.Tables(0).Rows(0).Item(2), Boolean)
                        User_Structure.功能设置 = CType(User_Password_DataSet.Tables(0).Rows(0).Item(3), Boolean)
                        User_Structure.参数设置 = CType(User_Password_DataSet.Tables(0).Rows(0).Item(4), Boolean)
                        User_Structure.程式切换 = CType(User_Password_DataSet.Tables(0).Rows(0).Item(5), Boolean)
                        User_Structure.手动操作 = CType(User_Password_DataSet.Tables(0).Rows(0).Item(6), Boolean)
                        User_Structure.单步执行 = CType(User_Password_DataSet.Tables(0).Rows(0).Item(7), Boolean)
                        User_Structure.增加用户 = CType(User_Password_DataSet.Tables(0).Rows(0).Item(8), Boolean)
                        User_Structure.删除用户 = CType(User_Password_DataSet.Tables(0).Rows(0).Item(9), Boolean)
                        User_Structure.点胶设置 = CType(User_Password_DataSet.Tables(0).Rows(0).Item(10), Boolean)
                        User_Password_DataSet.Dispose()
                        DataBase_Command.Dispose()
                End Select
            Catch ex As Exception
                Write_Log(ex.Message)
            End Try

        End Sub

        Sub Read_User_Parameter(ByRef User_Name_DataGrid As DevComponents.DotNetBar.Controls.DataGridViewX)
            Try
                Select Case DataBase_Link_Boolean
                    Case True
                        User_Name_DataGrid.Columns.Clear()
                        Dim User_Password_DataSet As New DataSet
                        User_Password_DataSet.Clear()
                        DataBase_Command.CommandText = "SELECT DISTINCT ID, 用户名称 FROM SYSTEM_PARAMETERS WHERE (用户名称 IS NOT NULL) ORDER BY ID"
                        DataBase_ConnectionAdapter = New SqlDataAdapter(DataBase_Command.CommandText, DataBase_Connection)
                        DataBase_ConnectionAdapter.Fill(User_Password_DataSet)
                        User_Name_DataGrid.DataSource = User_Password_DataSet.Tables(0)
                        For I = 0 To User_Name_DataGrid.Columns.Count - 1
                            User_Name_DataGrid.Columns(I).ReadOnly = True
                            User_Name_DataGrid.Columns(I).SortMode = Windows.Forms.DataGridViewColumnSortMode.NotSortable
                            Select Case I
                                Case User_Name_DataGrid.Columns.Count - 1
                                    User_Name_DataGrid.Columns(I).AutoSizeMode = Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
                                Case Else
                                    User_Name_DataGrid.Columns(I).AutoSizeMode = Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
                            End Select
                        Next
                        User_Name_DataGrid.RowHeadersVisible = False
                        User_Name_DataGrid.AllowUserToDeleteRows = False
                        User_Name_DataGrid.AllowUserToResizeColumns = False
                        User_Name_DataGrid.AllowUserToResizeRows = False
                        User_Name_DataGrid.ColumnHeadersHeightSizeMode = Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
                        User_Name_DataGrid.MultiSelect = False
                        User_Name_DataGrid.ReadOnly = True
                        User_Name_DataGrid.RowHeadersWidthSizeMode = Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
                        User_Name_DataGrid.SelectionMode = Windows.Forms.DataGridViewSelectionMode.FullRowSelect
                        User_Password_DataSet.Dispose()
                        DataBase_Command.Dispose()
                End Select
            Catch ex As Exception
                Write_Log(ex.Message)
            End Try

        End Sub

        Sub Read_User_PassWord(ByVal User_Name As String, ByRef Pass_Word As String)
            Try
                Select Case DataBase_Link_Boolean
                    Case True
                        Dim User_Password_DataSet As New DataSet
                        User_Password_DataSet.Clear()
                        DataBase_Command.CommandText = "SELECT 用户密码 FROM SYSTEM_PARAMETERS WHERE (用户名称 = N'" & User_Name & "') ORDER BY ID"
                        DataBase_ConnectionAdapter = New SqlDataAdapter(DataBase_Command.CommandText, DataBase_Connection)
                        DataBase_ConnectionAdapter.Fill(User_Password_DataSet)
                        If IsDBNull(User_Password_DataSet.Tables(0).Rows(0).Item("用户密码")) = False Then
                            Pass_Word = User_Password_DataSet.Tables(0).Rows(0).Item("用户密码").ToString.Trim
                        Else
                            Pass_Word = Rnd() * 1000
                        End If
                        User_Password_DataSet.Dispose()
                        DataBase_Command.Dispose()
                End Select
            Catch ex As Exception
                Write_Log(ex.Message)
            End Try

        End Sub

        Sub Update_User_Parameter(ByVal User_Name As String, ByVal PassWord As String, ByVal User_Parameter As USER_PARAMETER_STRUCTURE)
            Try
                Select Case DataBase_Link_Boolean
                    Case True
                        DataBase_Command = New SqlCommand("UPDATE SYSTEM_PARAMETERS SET 用户密码 = N'" & PassWord & "', 坐标添加 = @坐标添加, 坐标更新 = @坐标更新, 硬件配置 = @硬件配置, 功能设置 = @功能设置, 参数设置 = @参数设置, 程式切换 = @程式切换, 手动操作 = @手动操作, 单步执行 = @单步执行, 增加用户 =@增加用户, 删除用户 = @删除用户, 点胶设置 = @点胶设置 WHERE (用户名称 = N'" & User_Name & "')", DataBase_Connection)
                        DataBase_Command.Parameters.Add("@坐标添加", SqlDbType.Bit).Value = User_Parameter.坐标添加
                        DataBase_Command.Parameters.Add("@坐标更新", SqlDbType.Bit).Value = User_Parameter.坐标更新
                        DataBase_Command.Parameters.Add("@硬件配置", SqlDbType.Bit).Value = User_Parameter.硬件配置
                        DataBase_Command.Parameters.Add("@功能设置", SqlDbType.Bit).Value = User_Parameter.功能设置
                        DataBase_Command.Parameters.Add("@参数设置", SqlDbType.Bit).Value = User_Parameter.参数设置
                        DataBase_Command.Parameters.Add("@程式切换", SqlDbType.Bit).Value = User_Parameter.程式切换
                        DataBase_Command.Parameters.Add("@手动操作", SqlDbType.Bit).Value = User_Parameter.手动操作
                        DataBase_Command.Parameters.Add("@单步执行", SqlDbType.Bit).Value = User_Parameter.单步执行
                        DataBase_Command.Parameters.Add("@增加用户", SqlDbType.Bit).Value = User_Parameter.增加用户
                        DataBase_Command.Parameters.Add("@删除用户", SqlDbType.Bit).Value = User_Parameter.删除用户
                        DataBase_Command.Parameters.Add("@点胶设置", SqlDbType.Bit).Value = User_Parameter.点胶设置
                        DataBase_Command.ExecuteNonQuery()
                        DataBase_Command.Dispose()
                        MessageBoxEx.Show("用户:" & User_Name & "参数修改完成！", "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1)
                End Select
            Catch ex As Exception
                Write_Log(ex.Message)
            End Try

        End Sub

        Sub Update_User_Parameter(ByVal User_Name As String, ByVal User_Parameter As USER_PARAMETER_STRUCTURE)
            Try
                Select Case DataBase_Link_Boolean
                    Case True
                        DataBase_Command = New SqlCommand("UPDATE SYSTEM_PARAMETERS SET 坐标添加 = @坐标添加, 坐标更新 = @坐标更新, 硬件配置 = @硬件配置, 功能设置 = @功能设置, 参数设置 = @参数设置, 程式切换 = @程式切换, 手动操作 = @手动操作, 单步执行 = @单步执行, 增加用户 =@增加用户, 删除用户 = @删除用户, 点胶设置 = @点胶设置 WHERE (用户名称 = N'" & User_Name & "')", DataBase_Connection)
                        DataBase_Command.Parameters.Add("@坐标添加", SqlDbType.Bit).Value = User_Parameter.坐标添加
                        DataBase_Command.Parameters.Add("@坐标更新", SqlDbType.Bit).Value = User_Parameter.坐标更新
                        DataBase_Command.Parameters.Add("@硬件配置", SqlDbType.Bit).Value = User_Parameter.硬件配置
                        DataBase_Command.Parameters.Add("@功能设置", SqlDbType.Bit).Value = User_Parameter.功能设置
                        DataBase_Command.Parameters.Add("@参数设置", SqlDbType.Bit).Value = User_Parameter.参数设置
                        DataBase_Command.Parameters.Add("@程式切换", SqlDbType.Bit).Value = User_Parameter.程式切换
                        DataBase_Command.Parameters.Add("@手动操作", SqlDbType.Bit).Value = User_Parameter.手动操作
                        DataBase_Command.Parameters.Add("@单步执行", SqlDbType.Bit).Value = User_Parameter.单步执行
                        DataBase_Command.Parameters.Add("@增加用户", SqlDbType.Bit).Value = User_Parameter.增加用户
                        DataBase_Command.Parameters.Add("@删除用户", SqlDbType.Bit).Value = User_Parameter.删除用户
                        DataBase_Command.Parameters.Add("@点胶设置", SqlDbType.Bit).Value = User_Parameter.点胶设置
                        DataBase_Command.ExecuteNonQuery()
                        DataBase_Command.Dispose()
                        MessageBoxEx.Show("用户:" & User_Name & "参数修改完成！", "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1)
                End Select
            Catch ex As Exception
                Write_Log(ex.Message)
            End Try

        End Sub

        Sub Insert_User(ByVal User_Name As String, ByVal Pass_Word As String, ByVal User_Parameter As USER_PARAMETER_STRUCTURE)
            Try
                Select Case DataBase_Link_Boolean
                    Case True
                        DataBase_Command = New SqlCommand("INSERT INTO SYSTEM_PARAMETERS (用户名称, 用户密码, 坐标添加, 坐标更新, 硬件配置, 功能设置, 参数设置, 程式切换, 手动操作, 单步执行, 增加用户, 删除用户, 点胶设置) VALUES (N'" & User_Name & "', N'" & Pass_Word & "', @坐标添加, @坐标更新, @硬件配置, @功能设置, @参数设置, @程式切换, @手动操作, @单步执行, @增加用户, @删除用户, @点胶设置)", DataBase_Connection)
                        DataBase_Command.Parameters.Add("@坐标添加", SqlDbType.Bit).Value = User_Parameter.坐标添加
                        DataBase_Command.Parameters.Add("@坐标更新", SqlDbType.Bit).Value = User_Parameter.坐标更新
                        DataBase_Command.Parameters.Add("@硬件配置", SqlDbType.Bit).Value = User_Parameter.硬件配置
                        DataBase_Command.Parameters.Add("@功能设置", SqlDbType.Bit).Value = User_Parameter.功能设置
                        DataBase_Command.Parameters.Add("@参数设置", SqlDbType.Bit).Value = User_Parameter.参数设置
                        DataBase_Command.Parameters.Add("@程式切换", SqlDbType.Bit).Value = User_Parameter.程式切换
                        DataBase_Command.Parameters.Add("@手动操作", SqlDbType.Bit).Value = User_Parameter.手动操作
                        DataBase_Command.Parameters.Add("@单步执行", SqlDbType.Bit).Value = User_Parameter.单步执行
                        DataBase_Command.Parameters.Add("@增加用户", SqlDbType.Bit).Value = User_Parameter.增加用户
                        DataBase_Command.Parameters.Add("@删除用户", SqlDbType.Bit).Value = User_Parameter.删除用户
                        DataBase_Command.Parameters.Add("@点胶设置", SqlDbType.Bit).Value = User_Parameter.点胶设置
                        DataBase_Command.ExecuteNonQuery()
                        DataBase_Command.Dispose()
                        MessageBoxEx.Show("用户名:" & User_Name & "添加完成！", "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1)

                End Select
            Catch ex As Exception
                Write_Log(ex.Message)
            End Try

        End Sub

        Function Read_Code_Name() As String
            Try
                Dim Name_Str As String = Nothing
                Select Case DataBase_Link_Boolean
                    Case True
                        DataBase_Command = New SqlCommand("SELECT DISTINCT 文件名称列表 FROM DEVICE_SOFT WHERE (Code IS NOT NULL)", DataBase_Connection)
                        Name_Str = DataBase_Command.ExecuteScalar.ToString.Trim
                        DataBase_Command.Dispose()
                End Select
                Return Name_Str
            Catch ex As Exception
                Write_Log(ex.Message)
                Return "NULL"
            End Try
        End Function


        ''' <summary>
        ''' 更新控制卡配置文件
        ''' </summary>
        ''' <param name="Vision_File"></param>
        ''' <param name="Rtn_Message"></param>
        ''' <remarks></remarks>
        Sub Updata_Vision_File(ByVal Project_Name As String, ByVal Vision_File As String, ByRef ReVision_File As DevComponents.DotNetBar.PanelEx, Optional ByRef Rtn_Message As String = Nothing)

            Select Case DataBase_Link_Boolean
                Case True
                    DataBase_Command = New SqlCommand("UPDATE PROJECT_PARAMETERS SET 影像配置文件 = N'" & Vision_File & "' WHERE (项目名称 ='" & Project_Name & "')", DataBase_Connection)
                    Rtn_Message = "更新【" & DataBase_Command.ExecuteNonQuery() & "】条记录。"
                    DataBase_Command.Dispose()
                    DataBase_Command = New SqlCommand("SELECT 影像配置文件 FROM PROJECT_PARAMETERS WHERE (项目名称 ='" & Project_Name & "')", DataBase_Connection)
                    Dim ReStr As String = DataBase_Command.ExecuteScalar
                    ReVision_File.Text = ReStr
                    DataBase_Command.Dispose()
                    MessageBoxEx.Show("影像配置文件更新完成！", "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End Select
            MessageBoxEx.Show(Rtn_Message & vbCrLf & DataBase_Command.CommandText, "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Sub

        ''' <summary>
        ''' 清空所有项目消息
        ''' </summary>
        ''' <param name="SubName"></param>
        ''' <param name="ErrLine"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function Clear_All_Information(Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Boolean
            Dim ret As Boolean = False
            Try
                SubName = ErrName & GetCurrentMethod.Name & ":"
                Select Case DataBase_Link_Boolean
                    Case True
                        Dim DataBase_Com As New SqlCommand

                        DataBase_Com = New SqlCommand("DELETE FROM PROJECT_MESSAGE", DataBase_Connection)
                        DataBase_Com.ExecuteNonQuery()
                        DataBase_Com.Dispose()
                        ret = True
                End Select
            Catch ex As Exception
               

                Write_Log(SubName & ex.Message)
                ret = False
            End Try
            Return ret
        End Function

        Function Read_Project_Measure_Data(ByVal Project_Name As String, ByRef Message_data_View As DevComponents.DotNetBar.Controls.DataGridViewX, ByVal INF_ROW As DevComponents.DotNetBar.PanelEx, Optional ByVal DISP_ALL_INF As Boolean = False, Optional ByVal ROW_INT As Integer = 500, Optional ByVal SN As String = "", Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Boolean
            Dim ret As Boolean = False
            Try
                SubName = ErrName & GetCurrentMethod.Name & ":"
                Select Case DataBase_Link_Boolean
                    Case True
                        Message_data_View.Columns.Clear()
                        Dim Read_Message_DataSet As New DataSet
                        Read_Message_DataSet.Clear()
                        If SN <> "" Then
                            Select Case DISP_ALL_INF
                                Case False
                                    DataBase_Command.CommandText = "SELECT TOP " & ROW_INT & "* FROM PROJECT_MEASURE_DATA  WHERE(Project_Name=N'" & Project_Name & "') AND (SN=N'" & SN & "') ORDER BY ID DESC"
                                Case True
                                    DataBase_Command.CommandText = "SELECT * FROM PROJECT_MEASURE_DATA  WHERE(Project_Name=N'" & Project_Name & "') AND (SN=N'" & SN & "') ORDER BY ID DESC"
                            End Select
                        Else
                            Select Case DISP_ALL_INF
                                Case False
                                    DataBase_Command.CommandText = "SELECT TOP " & ROW_INT & "* FROM PROJECT_MEASURE_DATA  WHERE(Project_Name=N'" & Project_Name & "') ORDER BY ID DESC"
                                Case True
                                    DataBase_Command.CommandText = "SELECT * FROM PROJECT_MEASURE_DATA  WHERE(Project_Name=N'" & Project_Name & "') ORDER BY ID DESC"
                            End Select
                        End If

                        DataBase_ConnectionAdapter = New SqlDataAdapter(DataBase_Command.CommandText, DataBase_Connection)
                        DataBase_ConnectionAdapter.Fill(Read_Message_DataSet)
                        Message_data_View.DataSource = Read_Message_DataSet.Tables(0)
                        For I = 0 To Message_data_View.Columns.Count - 1
                            Message_data_View.Columns(I).SortMode = Windows.Forms.DataGridViewColumnSortMode.NotSortable
                            Message_data_View.Columns(I).ReadOnly = True
                            Select Case I
                                Case 0
                                    Message_data_View.Columns(I).AutoSizeMode = Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells
                                Case 3 'SN
                                    Message_data_View.Columns(I).AutoSizeMode = Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells
                                Case Message_data_View.Columns.Count - 1
                                    Message_data_View.Columns(I).AutoSizeMode = Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
                                Case Else
                                    Message_data_View.Columns(I).AutoSizeMode = Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader
                            End Select
                        Next
                        Message_data_View.RowHeadersVisible = False
                        Message_data_View.AllowUserToDeleteRows = False
                        Message_data_View.AllowUserToResizeColumns = False
                        Message_data_View.AllowUserToResizeRows = False
                        Message_data_View.ColumnHeadersHeightSizeMode = Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
                        Message_data_View.MultiSelect = False
                        Message_data_View.ReadOnly = True
                        Message_data_View.RowHeadersWidthSizeMode = Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
                        Message_data_View.SelectionMode = Windows.Forms.DataGridViewSelectionMode.FullRowSelect

                        If Message_data_View.Rows.Count >= 1 Then
                            INF_ROW.Text = Message_data_View.Rows.Count - 1
                        Else
                            INF_ROW.Text = 0
                        End If

                        DataBase_ConnectionAdapter.Dispose()
                        DataBase_Command.Dispose()
                        ret = True
                End Select
            Catch ex As Exception
               

                Write_Log(SubName & ex.Message)
                ret = False
            End Try
            Return ret
        End Function

        ''' <summary>
        ''' 清空所有项目消息
        ''' </summary>
        ''' <param name="SubName"></param>
        ''' <param name="ErrLine"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function Clear_All_MeasureData(ByVal Project_Name As String, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Boolean
            Dim ret As Boolean = False
            Try
                SubName = ErrName & GetCurrentMethod.Name & ":"
                Select Case DataBase_Link_Boolean
                    Case True
                        DataBase_Command = New SqlCommand("DELETE FROM PROJECT_MEASURE_DATA WHERE(Project_Name=N'" & Project_Name & "')", DataBase_Connection)
                        DataBase_Command.ExecuteNonQuery()
                        DataBase_Command.Dispose()
                        ret = True
                End Select
            Catch ex As Exception
                Write_Log(SubName & ex.Message)
                ret = False
            End Try
            Return ret
        End Function

        ''' <summary>
        ''' 清空所有项目消息
        ''' </summary>
        ''' <param name="SubName"></param>
        ''' <param name="ErrLine"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function Clear_SN_MeasureData(ByVal Project_Name As String, ByVal SN As String, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Boolean
            Dim ret As Boolean = False
            If SN Is Nothing Then
                SN = "NULL"
            End If
            Try
                SubName = ErrName & GetCurrentMethod.Name & ":"
                Select Case DataBase_Link_Boolean
                    Case True
                        DataBase_Command = New SqlCommand("DELETE FROM PROJECT_MEASURE_DATA WHERE(Project_Name=N'" & Project_Name & "') AND (SN=N'" & SN & "')", DataBase_Connection)
                        DataBase_Command.ExecuteNonQuery()
                        DataBase_Command.Dispose()
                        ret = True
                End Select
            Catch ex As Exception
                Write_Log(SubName & ex.Message)
                ret = False
            End Try
            Return ret
        End Function

        ''' <summary>
        ''' 删除所有坐标名称里的坐标
        ''' </summary>
        ''' <param name="SubName"></param>
        ''' <param name="ErrLine"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function Clear_All_CoorName_Pos(ByVal ProjectName As String, ByVal CoordinateName As String, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Boolean
            Dim ret As Boolean = False
            Try
                SubName = ErrName & GetCurrentMethod.Name & ":"
                Select Case DataBase_Link_Boolean
                    Case True
                        DataBase_Command = New SqlCommand("DELETE FROM PROJECT_COORDINATES_PARAMETERS WHERE(项目名称 = N'" & ProjectName & "') AND (坐标名称 = N'" & CoordinateName & "')", DataBase_Connection)
                        DataBase_Command.ExecuteNonQuery()
                        DataBase_Command.Dispose()
                        ret = True
                End Select
            Catch ex As Exception
               

                Write_Log(SubName & ex.Message)
                ret = False
            End Try
            Return ret
        End Function
        ''' <summary>
        ''' 清空指定项目消息
        ''' </summary>
        ''' <param name="Project_Name"></param>
        ''' <param name="SubName"></param>
        ''' <param name="ErrLine"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function Clear_Project_Information(ByVal Project_Name As String, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Boolean
            Dim ret As Boolean = False
            Try
                SubName = ErrName & GetCurrentMethod.Name & ":"
                Select Case DataBase_Link_Boolean
                    Case True
                        DataBase_Command = New SqlCommand("DELETE FROM PROJECT_MESSAGE WHERE (项目名称 = '" & Project_Name & "')", DataBase_Connection)
                        DataBase_Command.ExecuteNonQuery()
                        DataBase_Command.Dispose()
                        ret = True
                End Select
            Catch ex As Exception
               

                Write_Log(SubName & ex.Message)
                ret = False
            End Try
            Return ret
        End Function

        ''' <summary>
        ''' 统计所有项目消息数量
        ''' </summary>
        ''' <param name="Including_number"></param>
        ''' <param name="SubName"></param>
        ''' <param name="ErrLine"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function Read_All_Project_Information(ByRef Including_number As Integer, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Boolean
            Dim ret As Boolean = False
            Try
                SubName = ErrName & GetCurrentMethod.Name & ":"
                Select Case DataBase_Link_Boolean
                    Case True
                        DataBase_Command = New SqlCommand("SELECT COUNT(ID) AS ID FROM PROJECT_MESSAGE", DataBase_Connection)
                        Including_number = DataBase_Command.ExecuteScalar
                        DataBase_Command.Dispose()
                        ret = True
                End Select
            Catch ex As Exception
               

                Write_Log(SubName & ex.Message)
                ret = False
            End Try
            Return ret
        End Function

        ''' <summary>
        ''' 插入图片
        ''' </summary>
        ''' <param name="Project_Name"></param>
        ''' <param name="BarCode"></param>
        ''' <param name="Factory_number"></param>
        ''' <param name="Image_File_Path"></param>
        ''' <param name="Image_File_Name"></param>
        ''' <param name="Image_Info"></param>
        ''' <param name="SubName"></param>
        ''' <param name="ErrLine"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function Insert_Image(ByVal Project_Name As String, ByVal BarCode As String, ByVal Factory_number As String, ByVal Image_File_Path As String, ByVal Image_File_Name As String, Optional ByVal Image_Info As String = "NULL", Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Boolean
            Dim ret As Boolean = False
            Try
                SubName = ErrName & GetCurrentMethod.Name & ":"
                Select Case DataBase_Link_Boolean
                    Case True
                        If IO.File.Exists(Image_File_Path) = True Then
                            Dim st As New System.IO.FileStream(Image_File_Path, IO.FileMode.Open, IO.FileAccess.Read)
                            Dim mbr As New BinaryReader(st)
                            Dim buffer(st.Length) As Byte
                            mbr.Read(buffer, 0, CType(st.Length, Integer))
                            st.Close()
                            mbr.Close()
                            DataBase_Command = New SqlCommand("INSERT INTO DEVICE_IMAGE_DATA (项目名称, 产品条码, 设备编号, 保存日期, 图片名称, 拍照图片, 图片说明) VALUES (N'" & Project_Name & "', N'" & BarCode & "', N'" & Factory_number & "', CONVERT(DATETIME, '" & Date.Now & "', 102), N'" & Image_File_Name & "', @image, N'" & Image_Info & "')", DataBase_Connection)
                            DataBase_Command.Parameters.Add("@image", SqlDbType.Image).Value = buffer
                            DataBase_Command.ExecuteNonQuery()
                            DataBase_Command.Dispose()
                            st.Dispose()
                            ret = True
                        End If
                End Select
            Catch ex As Exception
               

                Write_Log(SubName & ex.Message)
                ret = False
            End Try
            Return ret
        End Function

        ''' <summary>
        ''' 插入图片
        ''' </summary>
        ''' <param name="Project_Name"></param>
        ''' <param name="BarCode"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function Insert_Image(ByVal Project_Name As String, ByVal BarCode As String, ByVal Key_Name As String, ByVal ImageByte() As Byte, ByVal Measure_Result As String) As Boolean
            Dim ret As Boolean = False
            Try
                Select Case DataBase_Link_Boolean
                    Case True
                        DataBase_Command = New SqlCommand("INSERT INTO PROJECT_MESAURE_IMAGE (项目名称, 键盘条码, 测量日期, 测量图片, 测量结果, 图片说明) VALUES (N'" & Project_Name & "', N'" & BarCode & "' , CONVERT(DATETIME, '" & Date.Now & "', 102), @varbinary, N'" & Measure_Result & "')", DataBase_Connection)
                        DataBase_Command.Parameters.Add("@varbinary", SqlDbType.VarBinary).Value = ImageByte
                        DataBase_Command.ExecuteNonQuery()
                        DataBase_Command.Dispose()
                        ret = True
                End Select
            Catch ex As Exception
                Write_Log(ex.Message)
                ret = False
            End Try
            Return ret
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="Combobox_Mode"></param>
        ''' <param name="Product_Name"></param>
        ''' <remarks></remarks>
        Function Read_Image_Introductions(ByRef Combobox_Mode As DevComponents.DotNetBar.Controls.ComboBoxEx, ByVal Product_Name As String, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Boolean
            Dim ret As Boolean = False
            Try
                SubName = ErrName & GetCurrentMethod.Name & ":"
                Select Case DataBase_Link_Boolean
                    Case True
                        Dim Query_image_type_DataSet As New DataSet
                        Query_image_type_DataSet.Clear()
                        Combobox_Mode.Items.Clear()
                        DataBase_Command.CommandText = "SELECT DISTINCT 图片说明 FROM DEVICE_IMAGE_DATA WHERE (项目名称 = N'" & Product_Name & "')"
                        DataBase_ConnectionAdapter = New SqlDataAdapter(DataBase_Command.CommandText, DataBase_Connection)
                        DataBase_ConnectionAdapter.Fill(Query_image_type_DataSet)
                        For i As Integer = 0 To Query_image_type_DataSet.Tables(0).Rows.Count - 1
                            Combobox_Mode.Items.Add(Query_image_type_DataSet.Tables(0).Rows(i).Item(0))
                        Next
                        If Combobox_Mode.Items.Count > 0 Then
                            Combobox_Mode.Items.Add("所有类型")
                        End If
                        Combobox_Mode.DropDownStyle = Windows.Forms.ComboBoxStyle.DropDownList
                        Query_image_type_DataSet.Dispose()
                        DataBase_Command.Dispose()
                        ret = True
                End Select
            Catch ex As Exception
               

                Write_Log(SubName & ex.Message)
                ret = False
            End Try
            Return ret
        End Function

        ''' <summary>
        ''' 读取图片
        ''' </summary>
        ''' <param name="imagebox"></param>
        ''' <param name="ID"></param>
        ''' <param name="SubName"></param>
        ''' <param name="ErrLine"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function Read_Image(ByRef imagebox As System.Windows.Forms.PictureBox, ByVal ID As Integer, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Boolean
            Dim ret As Boolean = False
            Try
                SubName = ErrName & GetCurrentMethod.Name & ":"
                Select Case DataBase_Link_Boolean
                    Case True
                        Dim read_image_DataSet As New DataSet
                        read_image_DataSet.Clear()
                        DataBase_Command.CommandText = "SELECT 拍照图片 FROM DEVICE_IMAGE_DATA WHERE (ID = " & ID & ")"
                        DataBase_ConnectionAdapter = New SqlDataAdapter(DataBase_Command.CommandText, DataBase_Connection)
                        DataBase_ConnectionAdapter.Fill(read_image_DataSet)
                        Dim imaginfo() As Byte = read_image_DataSet.Tables(0).Rows(0).Item("拍照图片")
                        Dim menorystream As New MemoryStream(imaginfo)
                        imagebox.Image = Image.FromStream(menorystream)
                        imagebox.SizeMode = PictureBoxSizeMode.Zoom
                        DataBase_Command.Dispose()
                        read_image_DataSet.Dispose()
                        ret = True
                End Select
            Catch ex As Exception
               

                Write_Log(SubName & ex.Message)
                ret = False
            End Try
            Return ret
        End Function

        ''' <summary>
        ''' 保存图像
        ''' </summary>
        ''' <param name="Image_Save_Dir"></param>
        ''' <param name="ImageBox"></param>
        ''' <param name="SubName"></param>
        ''' <param name="ErrLine"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function Save_Image(ByVal Image_Save_Dir As String, ByVal ImageBox As System.Windows.Forms.PictureBox, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Boolean
            Dim ret As Boolean = False
            Try
                SubName = ErrName & GetCurrentMethod.Name & ":"
                Select Case DataBase_Link_Boolean
                    Case True
                        ImageBox.Image.Save(Image_Save_Dir, System.Drawing.Imaging.ImageFormat.Jpeg)
                        ret = True
                        MessageBoxEx.Show("图片文件保存完成！", "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1)
                End Select
            Catch ex As Exception
               

                Write_Log(SubName & ex.Message)
                ret = False
            End Try
            Return ret
        End Function

        Function Save_Image(ByVal Product_SN As String, ByVal Image_Save_dir As String, ByVal Device_ID As String, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Boolean
            Dim ret As Boolean = False
            Try
                SubName = ErrName & GetCurrentMethod.Name & ":"
                Select Case DataBase_Link_Boolean
                    Case True
                        Dim FileStream_DataSet As New DataSet
                        FileStream_DataSet.Clear()
                        DataBase_Command.CommandText = "SELECT 图片名称, 拍照图片,产品条码 FROM DEVICE_IMAGE_DATA WHERE (产品条码 = '" & Product_SN & "') "
                        DataBase_ConnectionAdapter = New SqlDataAdapter(DataBase_Command.CommandText, DataBase_Connection)
                        DataBase_ConnectionAdapter.Fill(FileStream_DataSet)
                        If IO.Directory.Exists(Image_Save_dir & Product_SN & "\") = False Then
                            IO.Directory.CreateDirectory(Image_Save_dir & Product_SN & "\")
                        End If
                        If FileStream_DataSet.Tables(0).Rows.Count > 0 Then
                            For i As Integer = 0 To FileStream_DataSet.Tables(0).Rows.Count - 1
                                Dim FS As FileStream = Nothing
                                Dim File_Name As String = FileStream_DataSet.Tables(0).Rows(i).Item("图片名称").ToString.Trim
                                Dim BarCode As String = FileStream_DataSet.Tables(0).Rows(i).Item("产品条码").ToString.Trim
                                Dim ImagInfo() As Byte = FileStream_DataSet.Tables(0).Rows(i).Item("拍照图片")
                                Dim menorystream As New MemoryStream(ImagInfo)
                                If BarCode.Length < 12 Then
                                    BarCode = "NULL"
                                End If
                                FS = New FileStream(Image_Save_dir & Product_SN & "\" & Device_ID & "_" & BarCode & "_" & File_Name, FileMode.Create, FileAccess.Write)
                                For j As Integer = 0 To UBound(ImagInfo)
                                    FS.WriteByte(ImagInfo(j))
                                Next
                                FS.Flush()
                                FS.Close()
                                FS.Dispose()
                            Next
                            ret = True
                        Else
                            MessageBoxEx.Show("查询结果为空！没有可以保存的图片！", "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1)
                        End If
                        DataBase_Command.Dispose()
                        DataBase_ConnectionAdapter.Dispose()
                End Select
            Catch ex As Exception
               

                Write_Log(SubName & ex.Message)
                ret = False
            End Try
            Return ret
        End Function

        Function Save_Image(ByVal Image_Save_dir As String, ByVal Image_Id As Integer, ByRef ImageBox As System.Windows.Forms.PictureBox, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Boolean
            Dim ret As Boolean = False
            Try
                SubName = ErrName & GetCurrentMethod.Name & ":"
                Select Case DataBase_Link_Boolean
                    Case True
                        Dim FileStream_DataSet As New DataSet
                        FileStream_DataSet.Clear()
                        DataBase_Command.CommandText = "SELECT 图片名称, 拍照图片,产品条码 FROM DEVICE_IMAGE_DATA WHERE (ID = '" & Image_Id & "') "
                        DataBase_ConnectionAdapter = New SqlDataAdapter(DataBase_Command.CommandText, DataBase_Connection)
                        DataBase_ConnectionAdapter.Fill(FileStream_DataSet)
                        If FileStream_DataSet.Tables(0).Rows.Count > 0 Then
                            Dim FS As FileStream = Nothing
                            Dim File_Name As String = FileStream_DataSet.Tables(0).Rows(0).Item("图片名称").ToString.Trim
                            Dim BarCode As String = FileStream_DataSet.Tables(0).Rows(0).Item("产品条码").ToString.Trim
                            Dim ImagInfo() As Byte = FileStream_DataSet.Tables(0).Rows(0).Item("拍照图片")
                            Dim menorystream As New MemoryStream(ImagInfo)
                            ImageBox.Image = Image.FromStream(menorystream)
                            ImageBox.SizeMode = PictureBoxSizeMode.Zoom
                            If BarCode.Length < 5 Then
                                BarCode = "NULL"
                            End If
                            FS = New FileStream(Image_Save_dir & BarCode & "_" & File_Name, FileMode.Create, FileAccess.Write)
                            For j As Integer = 0 To UBound(ImagInfo)
                                FS.WriteByte(ImagInfo(j))
                            Next
                            FS.Flush()
                            FS.Close()
                            FS.Dispose()
                            ret = True
                        Else
                            MessageBoxEx.Show("查询结果为空！没有可以保存的图片！", "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1)
                        End If
                        DataBase_Command.Dispose()
                        DataBase_ConnectionAdapter.Dispose()
                End Select
            Catch ex As Exception
               

                Write_Log(SubName & ex.Message)
                ret = False
            End Try
            Return ret
        End Function

        Function Insert_Laser_Measure(ByVal Product_Name As String, ByVal Bar_Code As String, ByVal Fixture_Name As String, ByVal ErrorCount As Integer, ByVal Laser_Measure() As Double, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Boolean
            Dim ret As Boolean = False
            Try
                SubName = ErrName & GetCurrentMethod.Name & ":"
                Select Case DataBase_Link_Boolean
                    Case True
                        Dim Ti_str As String = "项目名称, 设备编号, 产品条码, 测量日期, 异常点数量, "
                        For i As Integer = 1 To 180
                            Select Case i
                                Case 180
                                    Ti_str = Ti_str & "P" & i.ToString.PadLeft(3).Replace(" ", "0")
                                Case Else
                                    Ti_str = Ti_str & "P" & i.ToString.PadLeft(3).Replace(" ", "0") & ", "
                            End Select
                        Next
                        Dim Value_Str As String = "'" & Product_Name & "', '" & Fixture_Name & "', '" & Bar_Code & "', " & "(CONVERT(DATETIME, '" & Date.Now & "', 102))" & ", " & ErrorCount & ", "
                        For i As Integer = 0 To 179
                            Select Case i
                                Case 179
                                    Value_Str = Value_Str & Laser_Measure(i)
                                Case Else
                                    Value_Str = Value_Str & Laser_Measure(i) & ", "
                            End Select
                        Next
                        DataBase_Command = New SqlCommand("INSERT INTO LASER_MEASURE (" & Ti_str & ") VALUES (" & Value_Str & ")", DataBase_Connection)
                        DataBase_Command.ExecuteNonQuery()
                        DataBase_Command.Dispose()
                        ret = True
                End Select
            Catch ex As Exception
               

                Write_Log(SubName & ex.Message)
                ret = False
            End Try
            Return ret
        End Function

        Function Read_Glue_ID(ByRef TextBox_Glue_ID As DevComponents.DotNetBar.Controls.TextBoxX, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Boolean
            Dim ret As Boolean = False
            Try
                SubName = ErrName & GetCurrentMethod.Name & ":"
                Select Case DataBase_Link_Boolean
                    Case True
                        Try
                            DataBase_Command = New SqlCommand("SELECT DISTINCT 胶水批次号 FROM DEVICE_PARAMETERS WHERE (ID = 1)", DataBase_Connection)
                            TextBox_Glue_ID.Text = DataBase_Command.ExecuteScalar
                            TextBox_Glue_ID.Enabled = False
                            ret = True
                        Catch ex As Exception
                            TextBox_Glue_ID.Text = "N/A"
                            TextBox_Glue_ID.Enabled = True
                        End Try
                        DataBase_Command.Dispose()
                End Select
            Catch ex As Exception
               

                Write_Log(SubName & ex.Message)
                ret = False
            End Try
            Return ret
        End Function

        Function Updata_Glue_ID(ByVal TextBox_Glue_ID As DevComponents.DotNetBar.Controls.TextBoxX, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Boolean
            Dim ret As Boolean = False
            Try
                SubName = ErrName & GetCurrentMethod.Name & ":"
                Select Case DataBase_Link_Boolean
                    Case True
                        DataBase_Command = New SqlCommand("UPDATE DEVICE_PARAMETERS SET 胶水批次号 = N'" & TextBox_Glue_ID.Text.Trim & "' WHERE (ID = 1)", DataBase_Connection)
                        DataBase_Command.ExecuteNonQuery()
                        DataBase_Command.Dispose()
                        TextBox_Glue_ID.Enabled = False
                        ret = True
                End Select
            Catch ex As Exception
               

                Write_Log(SubName & ex.Message)
                ret = False
            End Try
            Return ret
        End Function

        Function Read_Laser_Measure(ByVal Project_Name As String, ByRef Laser_Measure As DevComponents.DotNetBar.Controls.DataGridViewX, Optional ByVal Record_number As Integer = 500, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Boolean
            Dim ret As Boolean = False
            Try
                SubName = ErrName & GetCurrentMethod.Name & ":"
                Select Case DataBase_Link_Boolean
                    Case True
                        Dim Ti_str As String = "ID, 设备编号, 产品条码, 测量日期, 异常点数量, "
                        For i As Integer = 1 To 180
                            Select Case i
                                Case 180
                                    Ti_str = Ti_str & "P" & i.ToString.PadLeft(3).Replace(" ", "0")
                                Case Else
                                    Ti_str = Ti_str & "P" & i.ToString.PadLeft(3).Replace(" ", "0") & ", "
                            End Select
                        Next
                        Laser_Measure.Columns.Clear()
                        Dim Laser_Measure_DataSet As New DataSet
                        Laser_Measure_DataSet.Clear()
                        DataBase_Command.CommandText = "SELECT TOP(" & Record_number & ") " & Ti_str & " FROM LASER_MEASURE WHERE (项目名称 = N'" & Project_Name & "') ORDER BY ID DESC"
                        DataBase_ConnectionAdapter = New SqlDataAdapter(DataBase_Command.CommandText, DataBase_Connection)
                        DataBase_ConnectionAdapter.Fill(Laser_Measure_DataSet)
                        Laser_Measure.DataSource = Laser_Measure_DataSet.Tables(0)
                        For I = 0 To Laser_Measure.Columns.Count - 1
                            Laser_Measure.Columns(I).ReadOnly = True
                            Laser_Measure.Columns(I).SortMode = Windows.Forms.DataGridViewColumnSortMode.NotSortable
                            Laser_Measure.Columns(I).AutoSizeMode = Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
                        Next
                        Laser_Measure.RowHeadersVisible = False
                        Laser_Measure.AllowUserToDeleteRows = False
                        Laser_Measure.AllowUserToResizeColumns = False
                        Laser_Measure.AllowUserToResizeRows = False
                        Laser_Measure.ColumnHeadersHeightSizeMode = Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
                        Laser_Measure.MultiSelect = False
                        Laser_Measure.ReadOnly = True
                        Laser_Measure.RowHeadersWidthSizeMode = Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
                        Laser_Measure.SelectionMode = Windows.Forms.DataGridViewSelectionMode.FullRowSelect
                        DataBase_Command.Dispose()
                        Laser_Measure_DataSet.Dispose()
                        Laser_Measure.Update()
                        ret = True
                End Select
            Catch ex As Exception
               

                Write_Log(SubName & ex.Message)
                ret = False
            End Try
            Return ret
        End Function

        Function Read_Image(ByVal Project_Name As String, ByRef Project_Name_DataViewGrid As DevComponents.DotNetBar.Controls.DataGridViewX, ByVal Start_Time As DevComponents.Editors.DateTimeAdv.DateTimeInput, ByVal End_Time As DevComponents.Editors.DateTimeAdv.DateTimeInput, ByVal Image_Mode As DevComponents.DotNetBar.Controls.ComboBoxEx, ByVal BarCode As DevComponents.DotNetBar.Controls.TextBoxX, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Boolean
            Dim ret As Boolean = False
            Try
                SubName = ErrName & GetCurrentMethod.Name & ":"
                Select Case DataBase_Link_Boolean
                    Case True
                        Project_Name_DataViewGrid.Columns.Clear()
                        Dim Product_Name_DataSet As New DataSet
                        Product_Name_DataSet.Clear()
                        Select Case Image_Mode.SelectedIndex
                            Case Is >= 0
                                If Image_Mode.SelectedIndex = Image_Mode.Items.Count - 1 Then
                                    If BarCode.Text.Trim.Length <> 0 Then
                                        If Start_Time.LockUpdateChecked = False Or End_Time.LockUpdateChecked = False Then
                                            DataBase_Command.CommandText = "SELECT ID, 图片名称 ,保存日期, 图片说明, 产品条码 FROM DEVICE_IMAGE_DATA WHERE 项目名称='" & Project_Name & "' AND 产品条码='" & BarCode.Text.Trim & "' ORDER BY ID"
                                        Else
                                            DataBase_Command.CommandText = "SELECT ID, 图片名称 ,保存日期, 图片说明, 产品条码 FROM DEVICE_IMAGE_DATA WHERE (保存日期 >= CONVERT(DATETIME, '" & Start_Time.Value & "', 102)) AND (保存日期 <= CONVERT(DATETIME, '" & End_Time.Value & "', 102)) AND 项目名称='" & Project_Name & "' AND 产品条码='" & BarCode.Text.Trim & "' ORDER BY ID"
                                        End If
                                    Else
                                        If Start_Time.LockUpdateChecked = False Or End_Time.LockUpdateChecked = False Then
                                            DataBase_Command.CommandText = "SELECT ID, 图片名称 ,保存日期, 图片说明, 产品条码 FROM DEVICE_IMAGE_DATA WHERE 项目名称='" & Project_Name & "' ORDER BY ID"
                                        Else
                                            DataBase_Command.CommandText = "SELECT ID, 图片名称 ,保存日期, 图片说明, 产品条码  FROM DEVICE_IMAGE_DATA WHERE (保存日期 >= CONVERT(DATETIME, '" & Start_Time.Value & "', 102)) AND (保存日期 <= CONVERT(DATETIME, '" & End_Time.Value & "', 102)) AND 项目名称='" & Project_Name & "' ORDER BY ID"
                                        End If
                                    End If
                                Else
                                    If BarCode.Text.Trim.Length <> 0 Then
                                        If Start_Time.LockUpdateChecked = False Or End_Time.LockUpdateChecked = False Then
                                            DataBase_Command.CommandText = "SELECT ID, 图片名称 ,保存日期, 图片说明, 产品条码 FROM DEVICE_IMAGE_DATA WHERE 图片说明='" & Image_Mode.SelectedItem.ToString.Trim & "' AND 项目名称='" & Project_Name & "' AND BarCode='" & BarCode.Text.Trim & "' ORDER BY ID"
                                        Else
                                            DataBase_Command.CommandText = "SELECT ID, 图片名称 ,保存日期, 图片说明, 产品条码 FROM DEVICE_IMAGE_DATA WHERE (保存日期 >= CONVERT(DATETIME, '" & Start_Time.Value & "', 102)) AND (保存日期 <= CONVERT(DATETIME, '" & End_Time.Value & "', 102)) AND 图片说明='" & Image_Mode.SelectedItem.ToString.Trim & "' AND 项目名称='" & Project_Name & "' AND 产品条码='" & BarCode.Text.Trim & "' ORDER BY ID"
                                        End If
                                    Else
                                        If Start_Time.LockUpdateChecked = False Or End_Time.LockUpdateChecked = False Then
                                            DataBase_Command.CommandText = "SELECT ID, 图片名称 ,保存日期, 图片说明, 产品条码 FROM DEVICE_IMAGE_DATA WHERE 图片说明='" & Image_Mode.SelectedItem.ToString.Trim & "' AND 项目名称='" & Project_Name & "' ORDER BY ID"
                                        Else
                                            DataBase_Command.CommandText = "SELECT ID, 图片名称 ,保存日期, 图片说明, 产品条码 FROM DEVICE_IMAGE_DATA WHERE (保存日期 >= CONVERT(DATETIME, '" & Start_Time.Value & "', 102)) AND (保存日期 <= CONVERT(DATETIME, '" & End_Time.Value & "', 102)) AND 图片说明='" & Image_Mode.SelectedItem.ToString.Trim & "' AND 项目名称='" & Project_Name & "' ORDER BY ID"
                                        End If
                                    End If
                                End If
                            Case Else
                                If BarCode.Text.Trim.Length <> 0 Then
                                    If Start_Time.LockUpdateChecked = False Or End_Time.LockUpdateChecked = False Then
                                        DataBase_Command.CommandText = "SELECT ID, 图片名称 ,保存日期, 图片说明, 产品条码 FROM DEVICE_IMAGE_DATA WHERE 项目名称='" & Project_Name & "' AND BarCode='" & BarCode.Text.Trim & "' ORDER BY ID"
                                    Else
                                        DataBase_Command.CommandText = "SELECT ID, 图片名称 ,保存日期, 图片说明, 产品条码 FROM DEVICE_IMAGE_DATA WHERE (保存日期 >= CONVERT(DATETIME, '" & Start_Time.Value & "', 102)) AND (保存日期 <= CONVERT(DATETIME, '" & End_Time.Value & "', 102)) AND 项目名称='" & Project_Name & "' AND 产品条码='" & BarCode.Text.Trim & "' ORDER BY ID"
                                    End If
                                Else
                                    If Start_Time.LockUpdateChecked = False Or End_Time.LockUpdateChecked = False Then
                                        DataBase_Command.CommandText = "SELECT ID, 图片名称 ,保存日期, 图片说明, 产品条码 FROM DEVICE_IMAGE_DATA WHERE 项目名称='" & Project_Name & "' ORDER BY ID"
                                    Else
                                        DataBase_Command.CommandText = "SELECT ID, 图片名称 ,保存日期, 图片说明, 产品条码 FROM DEVICE_IMAGE_DATA WHERE (保存日期 >= CONVERT(DATETIME, '" & Start_Time.Value & "', 102)) AND (保存日期 <= CONVERT(DATETIME, '" & End_Time.Value & "', 102)) AND 项目名称='" & Project_Name & "' ORDER BY ID"
                                    End If
                                End If
                        End Select
                        DataBase_ConnectionAdapter = New SqlDataAdapter(DataBase_Command.CommandText, DataBase_Connection)
                        DataBase_ConnectionAdapter.Fill(Product_Name_DataSet)
                        Project_Name_DataViewGrid.DataSource = Product_Name_DataSet.Tables(0)
                        For I = 0 To Project_Name_DataViewGrid.Columns.Count - 1
                            Project_Name_DataViewGrid.Columns(I).ReadOnly = True
                            Project_Name_DataViewGrid.Columns(I).SortMode = Windows.Forms.DataGridViewColumnSortMode.NotSortable
                            Project_Name_DataViewGrid.Columns(I).AutoSizeMode = Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
                        Next
                        Project_Name_DataViewGrid.RowHeadersVisible = False
                        Project_Name_DataViewGrid.AllowUserToDeleteRows = False
                        Project_Name_DataViewGrid.AllowUserToResizeColumns = False
                        Project_Name_DataViewGrid.AllowUserToResizeRows = False
                        Project_Name_DataViewGrid.ColumnHeadersHeightSizeMode = Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
                        Project_Name_DataViewGrid.MultiSelect = False
                        Project_Name_DataViewGrid.ReadOnly = True
                        Project_Name_DataViewGrid.RowHeadersWidthSizeMode = Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
                        Project_Name_DataViewGrid.SelectionMode = Windows.Forms.DataGridViewSelectionMode.FullRowSelect
                        DataBase_Command.Dispose()
                        Product_Name_DataSet.Dispose()
                        Project_Name_DataViewGrid.Update()
                        Start_Time.LockUpdateChecked = False
                        End_Time.LockUpdateChecked = False
                        ret = True
                End Select
            Catch ex As Exception
               

                Write_Log(SubName & ex.Message)
                ret = False
            End Try
            Return ret
        End Function


        Function Delete_Image(Optional ByVal BarCodeLen As Integer = 12, Optional ByVal ID_Where As Integer = 10000, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Boolean
            Dim ret As Boolean = False
            Try
                SubName = ErrName & GetCurrentMethod.Name & ":"
                Select Case DataBase_Link_Boolean
                    Case True
                        DataBase_Command = New SqlCommand("DELETE TOP (500) FROM DEVICE_IMAGE_DATA WHERE (LEN(产品条码) < " & BarCodeLen & ") OR (产品条码 IS NULL)", DataBase_Connection)
                        DataBase_Command.ExecuteNonQuery()
                        DataBase_Command.Dispose()

                        DataBase_Command = New SqlCommand("SELECT COUNT(ID)-" & ID_Where & " AS 条码数量 FROM DEVICE_IMAGE_DATA", DataBase_Connection)
                        Dim Deltet_ID As Integer = CType(DataBase_Command.ExecuteScalar, Integer)
                        DataBase_Command.Dispose()

                        DataBase_Command = New SqlCommand("DELETE TOP (500) FROM DEVICE_IMAGE_DATA WHERE (ID <" & Deltet_ID & ")", DataBase_Connection)
                        DataBase_Command.ExecuteNonQuery()
                        DataBase_Command.Dispose()
                        ret = True
                End Select
            Catch ex As Exception
               

                Write_Log(SubName & ex.Message)
                ret = False
            End Try
            Return ret
        End Function

        Function Delete_Image(ByVal Product_SN As String, ByVal Fail_Delete_Boolean As Boolean, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Boolean
            Dim ret As Boolean = False
            Try
                SubName = ErrName & GetCurrentMethod.Name & ":"
                Select Case DataBase_Link_Boolean
                    Case True
                        If Fail_Delete_Boolean = True Then
                            DataBase_Command = New SqlCommand("DELETE FROM DEVICE_IMAGE_DATA WHERE (产品条码 =" & Product_SN & ")", DataBase_Connection)
                            DataBase_Command.ExecuteNonQuery()
                            DataBase_Command.Dispose()
                            ret = True
                        End If
                End Select
            Catch ex As Exception
               

                Write_Log(SubName & ex.Message)
                ret = False
            End Try
            Return ret
        End Function

        Function Delete_Image(ByVal Image_Id As Integer, ByVal Project_Name As String, ByVal Image_Mode As DevComponents.DotNetBar.Controls.ComboBoxEx, ByVal Start_Time As DevComponents.Editors.DateTimeAdv.DateTimeInput, ByVal End_Time As DevComponents.Editors.DateTimeAdv.DateTimeInput, ByVal BarCode As DevComponents.DotNetBar.Controls.TextBoxX, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Boolean
            Dim ret As Boolean = False
            Try
                SubName = ErrName & GetCurrentMethod.Name & ":"
                Select Case DataBase_Link_Boolean
                    Case True
                        Select Case Image_Id
                            Case -1
                                Select Case Image_Mode.SelectedIndex
                                    Case Is >= 0
                                        If Image_Mode.SelectedIndex = Image_Mode.Items.Count - 1 Then
                                            If BarCode.Text.Trim.Length <> 0 Then
                                                If Start_Time.LockUpdateChecked = False Or End_Time.LockUpdateChecked = False Then
                                                    DataBase_Command = New SqlCommand("DELETE FROM DEVICE_IMAGE_DATA WHERE 项目名称='" & Project_Name & "' AND 产品条码='" & BarCode.Text.Trim & "'", DataBase_Connection)
                                                Else
                                                    DataBase_Command = New SqlCommand("DELETE FROM DEVICE_IMAGE_DATA WHERE (保存日期 >= CONVERT(DATETIME, '" & Start_Time.Value & "', 102)) AND (保存日期 <= CONVERT(DATETIME, '" & End_Time.Value & "', 102)) AND 项目名称='" & Project_Name & "' AND 产品条码='" & BarCode.Text.Trim & "'", DataBase_Connection)
                                                End If
                                            Else
                                                If Start_Time.LockUpdateChecked = False Or End_Time.LockUpdateChecked = False Then
                                                    DataBase_Command = New SqlCommand("DELETE FROM DEVICE_IMAGE_DATA WHERE 项目名称='" & Project_Name & "'", DataBase_Connection)
                                                Else
                                                    DataBase_Command = New SqlCommand("DELETE FROM DEVICE_IMAGE_DATA WHERE (保存日期 >= CONVERT(DATETIME, '" & Start_Time.Value & "', 102)) AND (保存日期 <= CONVERT(DATETIME, '" & End_Time.Value & "', 102)) AND 项目名称='" & Project_Name & "'", DataBase_Connection)
                                                End If
                                            End If
                                        Else
                                            If BarCode.Text.Trim.Length <> 0 Then
                                                If Start_Time.LockUpdateChecked = False Or End_Time.LockUpdateChecked = False Then
                                                    DataBase_Command = New SqlCommand("DELETE FROM DEVICE_IMAGE_DATA WHERE 图片说明='" & Image_Mode.SelectedItem.ToString.Trim & "' AND 项目名称='" & Project_Name & "' AND 产品条码='" & BarCode.Text.Trim & "'", DataBase_Connection)
                                                Else
                                                    DataBase_Command = New SqlCommand("DELETE FROM DEVICE_IMAGE_DATA WHERE (保存日期 >= CONVERT(DATETIME, '" & Start_Time.Value & "', 102)) AND (保存日期 <= CONVERT(DATETIME, '" & End_Time.Value & "', 102)) AND 图片说明='" & Image_Mode.SelectedItem.ToString.Trim & "' AND 项目名称='" & Project_Name & "' AND 产品条码='" & BarCode.Text.Trim & "'", DataBase_Connection)
                                                End If
                                            Else
                                                If Start_Time.LockUpdateChecked = False Or End_Time.LockUpdateChecked = False Then
                                                    DataBase_Command = New SqlCommand("DELETE FROM DEVICE_IMAGE_DATA WHERE 图片说明='" & Image_Mode.SelectedItem.ToString.Trim & "' AND 项目名称='" & Project_Name & "'", DataBase_Connection)
                                                Else
                                                    DataBase_Command = New SqlCommand("DELETE FROM DEVICE_IMAGE_DATA WHERE (保存日期 >= CONVERT(DATETIME, '" & Start_Time.Value & "', 102)) AND (保存日期 <= CONVERT(DATETIME, '" & End_Time.Value & "', 102)) AND 图片说明='" & Image_Mode.SelectedItem.ToString.Trim & "' AND 项目名称='" & Project_Name & "'", DataBase_Connection)
                                                End If
                                            End If
                                        End If
                                    Case Else
                                        If BarCode.Text.Trim.Length <> 0 Then
                                            If Start_Time.LockUpdateChecked = False Or End_Time.LockUpdateChecked = False Then
                                                DataBase_Command = New SqlCommand("DELETE FROM DEVICE_IMAGE_DATA WHERE 项目名称='" & Project_Name & "' AND 产品条码='" & BarCode.Text.Trim & "'", DataBase_Connection)
                                            Else
                                                DataBase_Command = New SqlCommand("DELETE FROM DEVICE_IMAGE_DATA WHERE (保存日期 >= CONVERT(DATETIME, '" & Start_Time.Value & "', 102)) AND (保存日期 <= CONVERT(DATETIME, '" & End_Time.Value & "', 102)) AND 项目名称='" & Project_Name & "' AND 产品条码='" & BarCode.Text.Trim & "'", DataBase_Connection)
                                            End If
                                        Else
                                            If Start_Time.LockUpdateChecked = False Or End_Time.LockUpdateChecked = False Then
                                                DataBase_Command = New SqlCommand("DELETE FROM DEVICE_IMAGE_DATA  WHERE 项目名称='" & Project_Name & "'", DataBase_Connection)
                                            Else
                                                DataBase_Command = New SqlCommand("DELETE FROM DEVICE_IMAGE_DATA WHERE (保存日期 >= CONVERT(DATETIME, '" & Start_Time.Value & "', 102)) AND (保存日期 <= CONVERT(DATETIME, '" & End_Time.Value & "', 102)) AND 项目名称='" & Project_Name & "'", DataBase_Connection)
                                            End If
                                        End If
                                End Select
                                DataBase_Command.ExecuteNonQuery()
                                DataBase_Command.Dispose()
                                MessageBoxEx.Show("图片清空完成！", "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1)
                                ret = True
                            Case Else
                                DataBase_Command = New SqlCommand("DELETE FROM DEVICE_IMAGE_DATA WHERE ID=" & Image_Id & "", DataBase_Connection)
                                DataBase_Command.ExecuteNonQuery()
                                DataBase_Command.Dispose()
                                MessageBoxEx.Show("图片删除完成！", "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1)
                                ret = True
                        End Select
                End Select
            Catch ex As Exception
               

                Write_Log(SubName & ex.Message)
                ret = False
            End Try
            Return ret
        End Function

        Function Upload_FileStream(ByVal File_Name() As String, ByVal Upload_File_Path() As String, ByVal Version As String, ByRef ProgressBar As DevComponents.DotNetBar.Controls.ProgressBarX, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Boolean
            Dim ret As Boolean = False
            Try
                SubName = ErrName & GetCurrentMethod.Name & ":"
                Select Case DataBase_Link_Boolean
                    Case True
                        ProgressBar.Maximum = File_Name.Length
                        For i As Integer = 0 To File_Name.Length - 1
                            ProgressBar.Value = i + 1
                            Dim FS As New System.IO.FileStream(Upload_File_Path(i), IO.FileMode.Open, IO.FileAccess.Read)
                            Dim Mbr As New BinaryReader(FS)
                            Dim Buffer_FileStream(FS.Length) As Byte
                            Mbr.Read(Buffer_FileStream, 0, CType(FS.Length, Integer))
                            FS.Close()
                            FS.Dispose()
                            Mbr.Close()
                            DataBase_Command = New SqlCommand("INSERT INTO DEVICE_SOFT (文件名称列表, 设备软件版本, 文件保存列表) VALUES (N'" & File_Name(i) & "', N'" & Version & "',@image)", DataBase_Connection)
                            DataBase_Command.Parameters.Add("@image", SqlDbType.Image).Value = Buffer_FileStream
                            DataBase_Command.ExecuteNonQuery()
                        Next
                        MessageBoxEx.Show("指定文件全部上传完成！", "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        ProgressBar.Value = 0
                        DataBase_Command.Dispose()
                        ret = True
                End Select
            Catch ex As Exception
               

                Write_Log(SubName & ex.Message)
                ret = False
            End Try
            Return ret
        End Function

        Function Upload_FileStream(ByVal File_Name() As String, ByVal Upload_File_Path() As String, ByVal Version As String, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Boolean
            Dim ret As Boolean = False
            Try
                SubName = ErrName & GetCurrentMethod.Name & ":"
                Select Case DataBase_Link_Boolean
                    Case True
                        For i As Integer = 0 To File_Name.Length - 1
                            Dim FS As New System.IO.FileStream(Upload_File_Path(i), IO.FileMode.Open, IO.FileAccess.Read)
                            Dim Mbr As New BinaryReader(FS)
                            Dim Buffer_FileStream(FS.Length) As Byte
                            Mbr.Read(Buffer_FileStream, 0, CType(FS.Length, Integer))
                            FS.Close()
                            FS.Dispose()
                            Mbr.Close()
                            DataBase_Command = New SqlCommand("INSERT INTO DEVICE_SOFT (文件名称列表, 设备软件版本, 文件保存列表) VALUES (N'" & File_Name(i) & "', N'" & Version & "',@image)", DataBase_Connection)
                            DataBase_Command.Parameters.Add("@image", SqlDbType.Image).Value = Buffer_FileStream
                            DataBase_Command.ExecuteNonQuery()
                            ret = True
                        Next
                        DataBase_Command.Dispose()
                End Select
            Catch ex As Exception
               

                Write_Log(SubName & ex.Message)
                ret = False
            End Try
            Return ret
        End Function

        ''' <summary>
        ''' 下载更新软件
        ''' </summary>
        ''' <param name="Version"></param>
        ''' <remarks></remarks>
        Function DownLoad_FileStream(ByVal Version As String, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Boolean
            Dim ret As Boolean = False
            Try
                SubName = ErrName & GetCurrentMethod.Name & ":"
                Select Case DataBase_Link_Boolean
                    Case True
                        Dim FS As FileStream = Nothing
                        Dim Def_Dir As String = Application.StartupPath() & "\"
                        Dim FileStream_DataSet As New DataSet
                        FileStream_DataSet.Clear()
                        DataBase_Command.CommandText = "SELECT 文件名称列表,文件保存列表 FROM DEVICE_SOFT WHERE (设备软件版本 = " & Version & ")"
                        DataBase_ConnectionAdapter = New SqlDataAdapter(DataBase_Command.CommandText, DataBase_Connection)
                        DataBase_ConnectionAdapter.Fill(FileStream_DataSet)
                        For i As Integer = 0 To FileStream_DataSet.Tables(0).Rows.Count - 1
                            Dim ImagInfo() As Byte = FileStream_DataSet.Tables(0).Rows(i).Item("文件保存列表")
                            Dim File_Name As String = FileStream_DataSet.Tables(0).Rows(i).Item("文件名称列表")
                            FS = New FileStream(Def_Dir & File_Name, FileMode.Create, FileAccess.Write)
                            For j As Integer = 0 To UBound(ImagInfo)
                                FS.WriteByte(ImagInfo(j))
                            Next
                            FS.Flush()
                            FS.Close()
                            FS.Dispose()
                        Next
                        DataBase_Command.Dispose()
                        DataBase_ConnectionAdapter.Dispose()
                        ret = True
                End Select
            Catch ex As Exception
               

                Write_Log(SubName & ex.Message)
                ret = False
            End Try
            Return ret
        End Function

        Function Delete_Device_Message(ByVal ID As Integer, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Boolean
            Dim ret As Boolean = False
            Try
                SubName = ErrName & GetCurrentMethod.Name & ":"
                Select Case DataBase_Link_Boolean
                    Case True
                        DataBase_Command = New SqlCommand("DELETE FROM DEVICE_MESSSAGE WHERE (ID = " & ID & ")", DataBase_Connection)
                        DataBase_Command.ExecuteNonQuery()
                        DataBase_Command.Dispose()
                        ret = True
                End Select
            Catch ex As Exception
               

                Write_Log(SubName & ex.Message)
                ret = False
            End Try
            Return ret
        End Function

        ''' <summary>
        ''' 删除坐标
        ''' </summary>
        ''' <param name="ID"></param>
        ''' <param name="SubName"></param>
        ''' <param name="ErrLine"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function Delete_Coordinates(ByVal ID As Integer, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Boolean
            Dim ret As Boolean = False
            Try
                SubName = ErrName & GetCurrentMethod.Name & ":"
                Select Case DataBase_Link_Boolean
                    Case True
                        DataBase_Command = New SqlCommand("DELETE FROM PROJECT_COORDINATES_PARAMETERS WHERE (ID = " & ID & ")", DataBase_Connection)
                        DataBase_Command.ExecuteNonQuery()
                        DataBase_Command.Dispose()
                        ret = True
                End Select
            Catch ex As Exception
               

                Write_Log(SubName & ex.Message)
                ret = False
            End Try
            Return ret
        End Function

        ''' <summary>
        ''' 查询项目名称
        ''' </summary>
        ''' <param name="Product_Name_ComboBox">显示查询到的项目名称</param>
        ''' <remarks></remarks>
        Function Read_Project_Name(ByRef Product_Name_ComboBox As DevComponents.DotNetBar.Controls.ComboBoxEx, ByRef Product_Name_Array() As String, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Boolean
            Dim ret As Boolean = False
            Try
                SubName = ErrName & GetCurrentMethod.Name & ":"
                Select Case DataBase_Link_Boolean
                    Case True
                        DataBase_Command.CommandText = "SELECT DISTINCT ID, 项目名称 FROM PROJECT_PARAMETERS WHERE (项目名称 IS NOT NULL) ORDER BY ID"
                        Dim Product_Name_DataSet As New DataSet
                        Product_Name_DataSet.Clear()
                        Product_Name_ComboBox.Items.Clear()
                        DataBase_ConnectionAdapter = New SqlDataAdapter(DataBase_Command.CommandText, DataBase_Connection)
                        DataBase_ConnectionAdapter.Fill(Product_Name_DataSet)
                        Array.Resize(Product_Name_Array, Product_Name_DataSet.Tables(0).Rows.Count)
                        For i = 0 To Product_Name_DataSet.Tables(0).Rows.Count - 1
                            Product_Name_ComboBox.Items.Add("[" & Product_Name_DataSet.Tables(0).Rows(i).Item(0).ToString.Trim.ToUpper.PadLeft(3).Replace(" ", "0") & "]" & Product_Name_DataSet.Tables(0).Rows(i).Item(1).ToString.Trim.ToUpper)
                            Product_Name_Array(i) = Product_Name_DataSet.Tables(0).Rows(i).Item(1).ToString.Trim.ToUpper
                        Next
                        Product_Name_ComboBox.DropDownStyle = Windows.Forms.ComboBoxStyle.DropDownList
                        DataBase_Command.Dispose()
                        Product_Name_DataSet.Dispose()
                        Product_Name_ComboBox.Enabled = True
                        ret = True
                End Select
            Catch ex As Exception
               

                Write_Log(SubName & ex.Message)
                ret = False
            End Try
            Return ret
        End Function

        ''' <summary>
        ''' 查询项目名称
        ''' </summary>
        ''' <param name="Product_Name_ComboBox">显示查询到的项目名称</param>
        ''' <remarks></remarks>
        Function Read_Project_Name(ByRef Product_Name_ComboBox As DevComponents.DotNetBar.Controls.ComboBoxEx, ByRef Product_Id_Array() As String, ByRef Product_Name_Array() As String, ByRef Product_Color_Name() As String, ByRef Product_Color_Code() As String, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Boolean
            Dim ret As Boolean = False
            Try
                SubName = ErrName & GetCurrentMethod.Name & ":"
                Select Case DataBase_Link_Boolean
                    Case True
                        DataBase_Command.CommandText = "SELECT DISTINCT ID, 项目名称, 颜色名称, 颜色代码 FROM PROJECT_PARAMETERS WHERE (项目名称 IS NOT NULL) ORDER BY ID"
                        Dim Product_Name_DataSet As New DataSet
                        Product_Name_DataSet.Clear()
                        Product_Name_ComboBox.Items.Clear()
                        DataBase_ConnectionAdapter = New SqlDataAdapter(DataBase_Command.CommandText, DataBase_Connection)
                        DataBase_ConnectionAdapter.Fill(Product_Name_DataSet)
                        Array.Resize(Product_Id_Array, Product_Name_DataSet.Tables(0).Rows.Count)
                        Array.Resize(Product_Name_Array, Product_Name_DataSet.Tables(0).Rows.Count)
                        Array.Resize(Product_Color_Name, Product_Name_DataSet.Tables(0).Rows.Count)
                        Array.Resize(Product_Color_Code, Product_Name_DataSet.Tables(0).Rows.Count)
                        For i = 0 To Product_Name_DataSet.Tables(0).Rows.Count - 1
                            Product_Name_ComboBox.Items.Add("[" & Product_Name_DataSet.Tables(0).Rows(i).Item(0).ToString.Trim.ToUpper.PadLeft(3).Replace(" ", "0") & "]" & Product_Name_DataSet.Tables(0).Rows(i).Item(1).ToString.Trim.ToUpper)
                            Product_Id_Array(i) = Product_Name_DataSet.Tables(0).Rows(i).Item(0).ToString.Trim.ToUpper
                            Product_Name_Array(i) = Product_Name_DataSet.Tables(0).Rows(i).Item(1).ToString.Trim.ToUpper

                            If IsDBNull(Product_Name_DataSet.Tables(0).Rows(i).Item(2)) = False Then
                                Product_Color_Name(i) = Product_Name_DataSet.Tables(0).Rows(i).Item(2).ToString.Trim
                            Else
                                Product_Color_Name(i) = "NULL"
                            End If

                            If IsDBNull(Product_Name_DataSet.Tables(0).Rows(i).Item(3)) = False Then
                                Product_Color_Code(i) = Product_Name_DataSet.Tables(0).Rows(i).Item(3).ToString.Trim
                            Else
                                Product_Color_Code(i) = "NULL"
                            End If
                        Next
                        Product_Name_ComboBox.DropDownStyle = Windows.Forms.ComboBoxStyle.DropDownList
                        DataBase_Command.Dispose()
                        Product_Name_DataSet.Dispose()
                        Product_Name_ComboBox.Enabled = True
                        ret = True
                End Select
            Catch ex As Exception
               

                Write_Log(SubName & ex.Message)
                ret = False
            End Try
            Return ret
        End Function

        ''' <summary>
        ''' 查询项目名称
        ''' </summary>
        ''' <param name="Project_Name_DataViewGrid">显示查询到的项目名称</param>
        ''' <remarks></remarks>
        Function Read_Project_Name(ByRef Project_Name_DataViewGrid As DevComponents.DotNetBar.Controls.DataGridViewX, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Boolean
            Dim ret As Boolean = False
            Try
                SubName = ErrName & GetCurrentMethod.Name & ":"
                Select Case DataBase_Link_Boolean
                    Case True
                        Project_Name_DataViewGrid.Columns.Clear()
                        Dim Product_Name_DataSet As New DataSet
                        Product_Name_DataSet.Clear()
                        DataBase_Command.CommandText = "SELECT DISTINCT ID, 项目名称 FROM PROJECT_PARAMETERS WHERE (项目名称 IS NOT NULL) ORDER BY ID"
                        DataBase_ConnectionAdapter = New SqlDataAdapter(DataBase_Command.CommandText, DataBase_Connection)
                        DataBase_ConnectionAdapter.Fill(Product_Name_DataSet)
                        Project_Name_DataViewGrid.DataSource = Product_Name_DataSet.Tables(0)
                        For I = 0 To Project_Name_DataViewGrid.Columns.Count - 1
                            Project_Name_DataViewGrid.Columns(I).ReadOnly = True
                            Project_Name_DataViewGrid.Columns(I).SortMode = Windows.Forms.DataGridViewColumnSortMode.NotSortable
                            Select Case I
                                Case Project_Name_DataViewGrid.Columns.Count - 1
                                    Project_Name_DataViewGrid.Columns(I).AutoSizeMode = Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
                                Case Else
                                    Project_Name_DataViewGrid.Columns(I).AutoSizeMode = Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
                            End Select
                        Next
                        If Project_Name_DataViewGrid.Rows.Count > 1 Then
                            Project_Name_DataViewGrid.FirstDisplayedScrollingRowIndex = Project_Name_DataViewGrid.Rows.Count - 2
                            Project_Name_DataViewGrid.Rows(Project_Name_DataViewGrid.Rows.Count - 2).Selected = True
                        End If
                        Project_Name_DataViewGrid.RowHeadersVisible = False
                        Project_Name_DataViewGrid.AllowUserToDeleteRows = False
                        Project_Name_DataViewGrid.AllowUserToResizeColumns = False
                        Project_Name_DataViewGrid.AllowUserToResizeRows = False
                        Project_Name_DataViewGrid.ColumnHeadersHeightSizeMode = Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
                        Project_Name_DataViewGrid.MultiSelect = False
                        Project_Name_DataViewGrid.ReadOnly = True
                        Project_Name_DataViewGrid.RowHeadersWidthSizeMode = Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
                        Project_Name_DataViewGrid.SelectionMode = Windows.Forms.DataGridViewSelectionMode.FullRowSelect
                        DataBase_Command.Dispose()
                        Product_Name_DataSet.Dispose()
                        ret = True
                End Select
            Catch ex As Exception
               

                Write_Log(SubName & ex.Message)
                ret = False
            End Try
            Return ret
        End Function


        ''' <summary>
        ''' 读取坐标系名称
        ''' </summary>
        ''' <param name="Station">左右工位</param>
        ''' <param name="Coordinate_Dataset">坐标系名称数据源</param>
        ''' <param name="Coordinate_DataViewGrid">坐标系名称列表</param>
        ''' <param name="SubName"></param>
        ''' <param name="ErrLine"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function Read_Coordinate_Name(ByVal Project_Name As String, ByVal Station As String, ByRef Coordinate_Dataset As DataSet, Optional ByRef Coordinate_DataViewGrid As DevComponents.DotNetBar.Controls.DataGridViewX = Nothing, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Boolean
            Dim ret As Boolean = False
            Try
                SubName = ErrName & GetCurrentMethod.Name & ":"
                Select Case DataBase_Link_Boolean
                    Case True

                        Coordinate_Dataset = New DataSet
                        Coordinate_Dataset.Clear()
                        DataBase_Command.CommandText = "SELECT DISTINCT ID, 坐标名称 FROM PROJECT_COORDINATE_NAME WHERE (项目名称 = N'" & Project_Name & "' AND 坐标名称 IS NOT NULL AND 左右工位='" & Station & "') ORDER BY ID"
                        DataBase_ConnectionAdapter = New SqlDataAdapter(DataBase_Command.CommandText, DataBase_Connection)
                        DataBase_ConnectionAdapter.Fill(Coordinate_Dataset)

                        If Coordinate_DataViewGrid IsNot Nothing Then
                            Coordinate_DataViewGrid.Columns.Clear()
                            Coordinate_DataViewGrid.DataSource = Coordinate_Dataset.Tables(0)
                            For I = 0 To Coordinate_DataViewGrid.Columns.Count - 1
                                Coordinate_DataViewGrid.Columns(I).ReadOnly = True
                                Coordinate_DataViewGrid.Columns(I).SortMode = Windows.Forms.DataGridViewColumnSortMode.NotSortable
                                Select Case I
                                    Case Coordinate_DataViewGrid.Columns.Count - 1
                                        Coordinate_DataViewGrid.Columns(I).AutoSizeMode = Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
                                    Case Else
                                        Coordinate_DataViewGrid.Columns(I).AutoSizeMode = Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
                                End Select
                            Next
                            If Coordinate_DataViewGrid.Rows.Count > 1 Then
                                Coordinate_DataViewGrid.FirstDisplayedScrollingRowIndex = Coordinate_DataViewGrid.Rows.Count - 2
                                Coordinate_DataViewGrid.Rows(Coordinate_DataViewGrid.Rows.Count - 2).Selected = True
                            End If
                            Coordinate_DataViewGrid.RowHeadersVisible = False
                            Coordinate_DataViewGrid.AllowUserToDeleteRows = False
                            Coordinate_DataViewGrid.AllowUserToResizeColumns = False
                            Coordinate_DataViewGrid.AllowUserToResizeRows = False
                            Coordinate_DataViewGrid.ColumnHeadersHeightSizeMode = Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
                            Coordinate_DataViewGrid.MultiSelect = False
                            Coordinate_DataViewGrid.ReadOnly = True
                            Coordinate_DataViewGrid.RowHeadersWidthSizeMode = Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
                            Coordinate_DataViewGrid.SelectionMode = Windows.Forms.DataGridViewSelectionMode.FullRowSelect
                        End If

                        DataBase_Command.Dispose()
                        DataBase_ConnectionAdapter.Dispose()

                        ret = True
                End Select
            Catch ex As Exception
               

                Write_Log(SubName & ex.Message)
                ret = False
            End Try
            Return ret
        End Function


        ''' <summary>
        ''' 读取激光位置
        ''' </summary>
        ''' <param name="Project_Name_DataViewGrid">显示查询到的项目名称</param>
        ''' <remarks></remarks>
        Function Read_Laser_Pos(ByVal Project_Name As String, ByRef Project_Name_DataViewGrid As DevComponents.DotNetBar.Controls.DataGridViewX, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Boolean
            Dim ret As Boolean = False
            Try
                SubName = ErrName & GetCurrentMethod.Name & ":"
                Select Case DataBase_Link_Boolean
                    Case True
                        Project_Name_DataViewGrid.Columns.Clear()
                        Dim Product_Name_DataSet As New DataSet
                        Product_Name_DataSet.Clear()
                        DataBase_Command.CommandText = "SELECT * FROM PROJECT_LASER_POS WHERE (项目名称 = N'" & Project_Name & "') ORDER BY ID"
                        DataBase_ConnectionAdapter = New SqlDataAdapter(DataBase_Command.CommandText, DataBase_Connection)
                        DataBase_ConnectionAdapter.Fill(Product_Name_DataSet)
                        Project_Name_DataViewGrid.DataSource = Product_Name_DataSet.Tables(0)
                        For I = 0 To Project_Name_DataViewGrid.Columns.Count - 1
                            If I <= 1 Then
                                Project_Name_DataViewGrid.Columns(I).ReadOnly = True
                            Else
                                Project_Name_DataViewGrid.Columns(I).ReadOnly = False
                            End If

                            Project_Name_DataViewGrid.Columns(I).SortMode = Windows.Forms.DataGridViewColumnSortMode.NotSortable
                            Select Case I
                                Case Project_Name_DataViewGrid.Columns.Count - 1
                                    Project_Name_DataViewGrid.Columns(I).AutoSizeMode = Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
                                Case Else
                                    Project_Name_DataViewGrid.Columns(I).AutoSizeMode = Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
                            End Select
                        Next
                        If Project_Name_DataViewGrid.Rows.Count > 1 Then
                            Project_Name_DataViewGrid.FirstDisplayedScrollingRowIndex = Project_Name_DataViewGrid.Rows.Count - 2
                            Project_Name_DataViewGrid.Rows(Project_Name_DataViewGrid.Rows.Count - 2).Selected = True
                        End If
                        Project_Name_DataViewGrid.RowHeadersVisible = False
                        Project_Name_DataViewGrid.AllowUserToDeleteRows = False
                        Project_Name_DataViewGrid.AllowUserToResizeColumns = False
                        Project_Name_DataViewGrid.AllowUserToResizeRows = False
                        Project_Name_DataViewGrid.ColumnHeadersHeightSizeMode = Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
                        Project_Name_DataViewGrid.MultiSelect = False
                        'Project_Name_DataViewGrid.ReadOnly = True
                        Project_Name_DataViewGrid.RowHeadersWidthSizeMode = Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
                        Project_Name_DataViewGrid.SelectionMode = Windows.Forms.DataGridViewSelectionMode.FullRowSelect
                        Project_Name_DataViewGrid.EditMode = DataGridViewEditMode.EditOnF2

                        DataBase_Command.Dispose()
                        Product_Name_DataSet.Dispose()
                        ret = True
                End Select
            Catch ex As Exception
               

                Write_Log(SubName & ex.Message)
                ret = False
            End Try
            Return ret
        End Function

        ''' <summary>
        ''' 查询项目名称
        ''' </summary>
        ''' <param name="Product_Name_Array">显示查询到的项目名称</param>
        ''' <remarks></remarks>
        Function Read_Project_Name(ByRef Product_Name_Array() As String, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Boolean
            Dim ret As Boolean = False
            Try
                SubName = ErrName & GetCurrentMethod.Name & ":"
                Select Case DataBase_Link_Boolean
                    Case True
                        DataBase_Command.CommandText = "SELECT DISTINCT 项目名称,ID FROM PROJECT_PARAMETERS ORDER BY ID"
                        Dim Product_Name_DataSet As New DataSet
                        Product_Name_DataSet.Clear()
                        DataBase_ConnectionAdapter = New SqlDataAdapter(DataBase_Command.CommandText, DataBase_Connection)
                        DataBase_ConnectionAdapter.Fill(Product_Name_DataSet)
                        Array.Resize(Product_Name_Array, Product_Name_DataSet.Tables(0).Rows.Count)
                        Array.Clear(Product_Name_Array, 0, Product_Name_Array.Length)
                        For i = 0 To Product_Name_DataSet.Tables(0).Rows.Count - 1
                            Product_Name_Array(i) = Product_Name_DataSet.Tables(0).Rows(i).Item(0).ToString.Trim.ToUpper
                        Next
                        DataBase_Command.Dispose()
                        Product_Name_DataSet.Dispose()
                        ret = True
                End Select
            Catch ex As Exception
               

                Write_Log(SubName & ex.Message)
                ret = False
            End Try
            Return ret
        End Function


        ''' <summary>
        ''' 读取参数
        ''' </summary>
        ''' <param name="Parameter_Name">参数表</param>
        ''' <param name="Device_Parameter">控件表</param>
        ''' <remarks></remarks>
        Sub Read_Device_Parameter(ByVal Parameter_Name() As String, ByRef Device_Parameter() As Object, ByVal ID As Integer, Optional ByRef Rtn_Message As String = Nothing)

            Select Case DataBase_Link_Boolean
                Case True
                    Dim Serch_Axis_Parameter_DataSet As New DataSet
                    Serch_Axis_Parameter_DataSet.Clear()
                    Dim Parameter_Count As Integer = Parameter_Name.Length
                    Dim Total_Str As String = Nothing
                    For i As Integer = 0 To Parameter_Count - 1
                        Select Case i
                            Case Parameter_Count - 1
                                Total_Str = Total_Str & Parameter_Name(i)
                            Case Else
                                Total_Str = Total_Str & Parameter_Name(i) & ", "
                        End Select
                    Next

                    DataBase_Command.CommandText = "SELECT  " & Total_Str & " FROM DEVICE_PARAMETERS WHERE (ID = " & ID & ")"
                    DataBase_ConnectionAdapter = New SqlDataAdapter(DataBase_Command.CommandText, DataBase_Connection)
                    DataBase_ConnectionAdapter.Fill(Serch_Axis_Parameter_DataSet)
                    For i As Integer = 0 To Serch_Axis_Parameter_DataSet.Tables(0).Columns.Count - 1
                        If IsDBNull(Serch_Axis_Parameter_DataSet.Tables(0).Rows(0).Item(i)) = False Then
                            Device_Parameter(i) = Serch_Axis_Parameter_DataSet.Tables(0).Rows(0).Item(i)
                        Else
                            Device_Parameter(i) = 0
                        End If
                    Next
                    Rtn_Message = "查询轴参数完成。"
                    Serch_Axis_Parameter_DataSet.Dispose()
                    DataBase_Command.Dispose()
            End Select

        End Sub

        ''' <summary>
        ''' 更新控制卡配置文件
        ''' </summary>
        ''' <param name="Rtn_Message"></param>
        ''' <remarks></remarks>
        Sub Read_Vision_File(ByVal Project_Name As String, ByRef ReVision_File As DevComponents.DotNetBar.PanelEx, Optional ByRef Rtn_Message As String = Nothing)

            Select Case DataBase_Link_Boolean
                Case True
                    DataBase_Command = New SqlCommand("SELECT 影像配置文件 FROM PROJECT_PARAMETERS WHERE (项目名称 ='" & Project_Name & "')", DataBase_Connection)
                    Dim ReStr As String = DataBase_Command.ExecuteScalar.ToString.Trim
                    ReVision_File.Text = ReStr
                    DataBase_Command.Dispose()
            End Select
        End Sub

        ''' <summary>
        ''' 查询测量结果
        ''' </summary>
        ''' <param name="Project_Name"></param>
        ''' <param name="OK_Number"></param>
        ''' <param name="NG_Number"></param>
        ''' <param name="Total_Number"></param>
        ''' <remarks></remarks>
        Sub Query_results(ByVal Project_Name As String, ByRef OK_Number As DevComponents.DotNetBar.PanelEx, ByRef NG_Number As DevComponents.DotNetBar.PanelEx, ByRef Total_Number As DevComponents.DotNetBar.PanelEx, Optional ByVal Rtn_Message As String = "")

            Select Case DataBase_Link_Boolean
                Case True
                    Dim Rtn_Cmd As Object
                    DataBase_Command = New SqlCommand("SELECT COUNT(测量结果) AS Expr1 FROM MEASURE_DATA GROUP BY 项目名称, 测量结果 HAVING (项目名称 = N'" & Project_Name & "') AND (测量结果 = N'OK')", DataBase_Connection)
                    Rtn_Cmd = DataBase_Command.ExecuteScalar
                    If IsNumeric(Rtn_Cmd) = True Then
                        OK_Number.Text = Rtn_Cmd
                    Else
                        OK_Number.Text = "0"
                    End If
                    DataBase_Command = New SqlCommand("SELECT COUNT(测量结果) AS Expr1 FROM MEASURE_DATA GROUP BY 项目名称, 测量结果 HAVING (项目名称 = N'" & Project_Name & "') AND (测量结果 = N'NG')", DataBase_Connection)
                    Rtn_Cmd = DataBase_Command.ExecuteScalar
                    If IsNumeric(Rtn_Cmd) = True Then
                        NG_Number.Text = Rtn_Cmd
                    Else
                        NG_Number.Text = "0"
                    End If
                    Total_Number.Text = CType(OK_Number.Text, Integer) + CType(NG_Number.Text, Integer)
                    DataBase_Command.Dispose()
            End Select
        End Sub

        ''' <summary>
        ''' 更新控制卡配置文件
        ''' </summary>
        ''' <remarks></remarks>
        Function Read_Vision_File(ByVal Project_Name As String, ByRef ReVision_File As String, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Boolean
            Dim ret As Boolean = False
            Try
                Select Case DataBase_Link_Boolean
                    Case True
                        DataBase_Command = New SqlCommand("SELECT 影像配置文件 FROM PROJECT_PARAMETERS WHERE (项目名称 ='" & Project_Name & "')", DataBase_Connection)
                        Dim ReStr As String = DataBase_Command.ExecuteScalar
                        ReVision_File = ReStr.Trim
                        DataBase_Command.Dispose()
                        ret = True
                End Select
            Catch ex As Exception
                MessageBoxEx.Show(ex.Message & vbCrLf & DataBase_Command.CommandText, "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Information)
                ret = False
            End Try
            Return ret
        End Function

        Function Read_Color_Parameters(ByRef Product_Id_Array() As String, ByRef Product_Name_Array() As String, ByRef Product_Color_Name() As String, ByRef Product_Color_Code() As String, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Boolean
            Dim ret As Boolean = False
            Try
                SubName = ErrName & GetCurrentMethod.Name & ":"
                Select Case DataBase_Link_Boolean
                    Case True
                        DataBase_Command.CommandText = "SELECT DISTINCT ID, 项目名称, 颜色名称, 颜色代码 FROM PROJECT_PARAMETERS WHERE (项目名称 IS NOT NULL) ORDER BY ID"
                        Dim Product_Name_DataSet As New DataSet
                        Product_Name_DataSet.Clear()
                        DataBase_ConnectionAdapter = New SqlDataAdapter(DataBase_Command.CommandText, DataBase_Connection)
                        DataBase_ConnectionAdapter.Fill(Product_Name_DataSet)
                        Array.Resize(Product_Id_Array, Product_Name_DataSet.Tables(0).Rows.Count)
                        Array.Resize(Product_Name_Array, Product_Name_DataSet.Tables(0).Rows.Count)
                        Array.Resize(Product_Color_Name, Product_Name_DataSet.Tables(0).Rows.Count)
                        Array.Resize(Product_Color_Code, Product_Name_DataSet.Tables(0).Rows.Count)
                        For i = 0 To Product_Name_DataSet.Tables(0).Rows.Count - 1
                            Product_Id_Array(i) = Product_Name_DataSet.Tables(0).Rows(i).Item(0).ToString.Trim.ToUpper
                            Product_Name_Array(i) = Product_Name_DataSet.Tables(0).Rows(i).Item(1).ToString.Trim.ToUpper

                            If IsDBNull(Product_Name_DataSet.Tables(0).Rows(i).Item(2)) = False Then
                                Product_Color_Name(i) = Product_Name_DataSet.Tables(0).Rows(i).Item(2).ToString.Trim
                            Else
                                Product_Color_Name(i) = "NULL"
                            End If

                            If IsDBNull(Product_Name_DataSet.Tables(0).Rows(i).Item(3)) = False Then
                                Product_Color_Code(i) = Product_Name_DataSet.Tables(0).Rows(i).Item(3).ToString.Trim
                            Else
                                Product_Color_Code(i) = "NULL"
                            End If
                        Next
                        DataBase_Command.Dispose()
                        Product_Name_DataSet.Dispose()
                        ret = True
                End Select
            Catch ex As Exception
               

                Write_Log(SubName & ex.Message)
                ret = False
            End Try
            Return ret
        End Function

        Function Color_Exists(ByVal Color_Code As String, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Boolean
            Dim ret As Boolean = False
            Try
                SubName = ErrName & GetCurrentMethod.Name & ":"
                Select Case DataBase_Link_Boolean
                    Case True
                        DataBase_Command = New SqlCommand("SELECT COUNT(颜色代码) AS 颜色计数 FROM PROJECT_PARAMETERS GROUP BY 颜色代码 HAVING (颜色代码 = N'" & Color_Code & "')", DataBase_Connection)
                        If DataBase_Command.ExecuteScalar > 0 Then
                            ret = True
                        Else
                            ret = False
                        End If
                        DataBase_Command.Dispose()
                End Select
            Catch ex As Exception
               

                Write_Log(SubName & ex.Message)
                ret = False
            End Try
            Return ret
        End Function

        Function Insert_Coordinates(ByVal Project_Name As String, ByVal Coordinates_Name As String, ByVal Parameter_Name() As String, ByVal Parameter_Value() As Object, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Boolean
            Dim ret As Boolean = False
            Try
                SubName = ErrName & GetCurrentMethod.Name & ":"
                Select Case DataBase_Link_Boolean
                    Case True
                        Dim Parameter_Count As Integer = Parameter_Name.Length
                        Dim Parameter_Total_Str As String = Nothing
                        Dim Parameter_Value_Total_Str As String = Nothing
                        For i As Integer = 0 To Parameter_Count - 1
                            Select Case i
                                Case Parameter_Count - 1
                                    Parameter_Total_Str = Parameter_Total_Str & Parameter_Name(i)
                                    Parameter_Value_Total_Str = Parameter_Value_Total_Str & Parameter_Value(i)
                                Case Else
                                    Parameter_Total_Str = Parameter_Total_Str & Parameter_Name(i) & ", "
                                    Parameter_Value_Total_Str = Parameter_Value_Total_Str & Parameter_Value(i) & ", "
                            End Select
                        Next
                        DataBase_Command = New SqlCommand("INSERT INTO PROJECT_COORDINATES_PARAMETERS (项目名称, 坐标名称, " & Parameter_Total_Str & ") VALUES ('" & Project_Name & "', '" & Coordinates_Name & "'," & Parameter_Value_Total_Str & ")", DataBase_Connection)
                        DataBase_Command.ExecuteNonQuery()
                        DataBase_Command.Dispose()

                        ret = True
                End Select
            Catch ex As Exception
               

                Write_Log(SubName & ex.Message)
                ret = False
            End Try
            Return ret

        End Function

        Sub Insert_Object_Value(ByVal Table_Name As String, ByVal Parameter_Name() As String, ByVal Parameter_Value() As Object)
            Select Case DataBase_Link_Boolean
                Case True
                    Dim Parameter_Count As Integer = Parameter_Name.Length
                    Dim Parameter_Total_Str As String = Nothing
                    Dim Parameter_Value_Total_Str As String = Nothing
                    For i As Integer = 0 To Parameter_Count - 1
                        Select Case i
                            Case Parameter_Count - 1
                                Parameter_Total_Str = Parameter_Total_Str & Parameter_Name(i)
                                Parameter_Value_Total_Str = Parameter_Value_Total_Str & Parameter_Value(i)
                            Case Else
                                Parameter_Total_Str = Parameter_Total_Str & Parameter_Name(i) & ", "
                                Parameter_Value_Total_Str = Parameter_Value_Total_Str & Parameter_Value(i) & ", "
                        End Select
                    Next
                    DataBase_Command = New SqlCommand("INSERT INTO " & Table_Name & Parameter_Total_Str & ") VALUES ('" & Parameter_Value_Total_Str & ")", DataBase_Connection)
                    DataBase_Command.Dispose()
            End Select
        End Sub

        Function Insert_Color(ByVal Project_Name As String, ByVal Sf_Color_Code As String, ByVal Color_Name As String, ByVal Switch_Command As String, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Boolean
            Dim ret As Boolean = False
            Try
                SubName = ErrName & GetCurrentMethod.Name & ":"
                Select Case DataBase_Link_Boolean
                    Case True
                        DataBase_Command = New SqlCommand("INSERT INTO PROJECT_PARAMETERS (颜色代码, 颜色名称, 影像程式号) VALUES (N'" & Sf_Color_Code & "', N'" & Color_Name & "', N'" & Switch_Command & "')", DataBase_Connection)
                        DataBase_Command.ExecuteNonQuery()
                        DataBase_Command.Dispose()
                        ret = True
                        MessageBoxEx.Show("颜色参数写入完成！", "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1)
                End Select
            Catch ex As Exception
               

                Write_Log(SubName & ex.Message)
                ret = False
            End Try
            Return ret
        End Function

        Function Update_Color(ByVal Sf_Color_Code As String, ByVal Color_Name As String, ByVal Switch_Command As String, ByVal ID As Integer, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Boolean
            Dim ret As Boolean = False
            Try
                SubName = ErrName & GetCurrentMethod.Name & ":"
                Select Case DataBase_Link_Boolean
                    Case True
                        DataBase_Command = New SqlCommand("UPDATE PROJECT_PARAMETERS SET 颜色名称 = N'" & Color_Name & "', 颜色代码 = N'" & Sf_Color_Code & "' , 影像程式号 = N'" & Switch_Command & "' WHERE (项目名称 IS NOT NULL) AND (颜色名称 IS NOT NULL) AND (颜色代码 IS NOT NULL) AND (ID = " & ID & ")", DataBase_Connection)
                        DataBase_Command.ExecuteNonQuery()
                        DataBase_Command.Dispose()
                        MessageBoxEx.Show("颜色参数更新完成！", "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1)
                End Select
            Catch ex As Exception
               

                Write_Log(SubName & ex.Message)
                ret = False
            End Try
            Return ret
        End Function

        Function Read_Color_List(ByRef Color_List_DataViewGrid As DevComponents.DotNetBar.Controls.DataGridViewX, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Boolean
            Dim ret As Boolean = False
            Try
                SubName = ErrName & GetCurrentMethod.Name & ":"
                Select Case DataBase_Link_Boolean
                    Case True
                        Color_List_DataViewGrid.Columns.Clear()
                        Dim Color_List_DataSet As New DataSet
                        Color_List_DataSet.Clear()
                        Color_List_DataViewGrid.Columns.Clear()
                        DataBase_Command.CommandText = "SELECT DISTINCT ID, 项目名称, 颜色名称,颜色代码,影像程式号 FROM PROJECT_PARAMETERS WHERE (项目名称 IS NOT NULL) AND (颜色名称 IS NOT NULL) AND (颜色代码 IS NOT NULL) ORDER BY ID"
                        DataBase_ConnectionAdapter = New SqlDataAdapter(DataBase_Command.CommandText, DataBase_Connection)
                        DataBase_ConnectionAdapter.Fill(Color_List_DataSet)
                        Color_List_DataViewGrid.DataSource = Color_List_DataSet.Tables(0)
                        For I = 0 To Color_List_DataViewGrid.Columns.Count - 1
                            Color_List_DataViewGrid.Columns(I).ReadOnly = True
                            Color_List_DataViewGrid.Columns(I).SortMode = Windows.Forms.DataGridViewColumnSortMode.NotSortable
                            Select Case I
                                Case 0
                                    Color_List_DataViewGrid.Columns(I).AutoSizeMode = Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader
                                Case 1
                                    Color_List_DataViewGrid.Columns(I).AutoSizeMode = Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
                                Case Else
                                    Color_List_DataViewGrid.Columns(I).AutoSizeMode = Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
                            End Select
                        Next
                        Color_List_DataViewGrid.RowHeadersVisible = False
                        Color_List_DataViewGrid.AllowUserToDeleteRows = False
                        Color_List_DataViewGrid.AllowUserToResizeColumns = False
                        Color_List_DataViewGrid.AllowUserToResizeRows = False
                        Color_List_DataViewGrid.ColumnHeadersHeightSizeMode = Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
                        Color_List_DataViewGrid.MultiSelect = False
                        Color_List_DataViewGrid.ReadOnly = True
                        Color_List_DataViewGrid.RowHeadersWidthSizeMode = Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
                        Color_List_DataViewGrid.SelectionMode = Windows.Forms.DataGridViewSelectionMode.FullRowSelect
                        DataBase_Command.Dispose()
                        Color_List_DataSet.Dispose()
                End Select
            Catch ex As Exception
               

                Write_Log(SubName & ex.Message)
                ret = False
            End Try
            Return ret
        End Function

        Function Read_Color_List(ByRef ComboBox_Color_List As DevComponents.DotNetBar.Controls.ComboBoxEx, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Boolean
            Dim ret As Boolean = False
            Try
                SubName = ErrName & GetCurrentMethod.Name & ":"
                Select Case DataBase_Link_Boolean
                    Case True
                        Dim Color_List_DataSet As New DataSet
                        Color_List_DataSet.Clear()
                        ComboBox_Color_List.Items.Clear()
                        DataBase_Command.CommandText = "SELECT DISTINCT 影像程式号, ID FROM PROJECT_PARAMETERS WHERE (项目名称 IS NOT NULL) AND (影像程式号 IS NOT NULL) ORDER BY ID"
                        DataBase_ConnectionAdapter = New SqlDataAdapter(DataBase_Command.CommandText, DataBase_Connection)
                        DataBase_ConnectionAdapter.Fill(Color_List_DataSet)
                        For i As Integer = 0 To Color_List_DataSet.Tables(0).Rows.Count - 1
                            ComboBox_Color_List.Items.Add(Color_List_DataSet.Tables(0).Rows(i).Item(0).ToString.Trim)
                        Next
                        ComboBox_Color_List.DropDownStyle = Windows.Forms.ComboBoxStyle.DropDownList
                        DataBase_Command.Dispose()
                        DataBase_ConnectionAdapter.Dispose()
                End Select
            Catch ex As Exception
               

                Write_Log(SubName & ex.Message)
                ret = False
            End Try
            Return ret
        End Function


        ''' 统一列坐标
        Function Update_Column_Pos(ByVal PROJECT_NAME As String, ByVal COORDINATE_NAME As String, ByVal COL_NAME As String, ByVal COL_VALUE As Double, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Boolean
            Dim ret As Boolean = False
            Try
                SubName = ErrName & GetCurrentMethod.Name & ":"
                Select Case DataBase_Link_Boolean
                    Case True
                        DataBase_Command = New SqlCommand("UPDATE PROJECT_COORDINATES_PARAMETERS SET " & COL_NAME & "= " & COL_VALUE & "WHERE(项目名称 = N'" & PROJECT_NAME & "') AND (坐标名称 = N'" & COORDINATE_NAME & "')", DataBase_Connection)
                        MessageBoxEx.Show("更新[" & COORDINATE_NAME & "]的所有【" & COL_NAME & "】轴坐标完成，共修改" & DataBase_Command.ExecuteNonQuery() & "条记录！", "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1)
                        DataBase_Command.Dispose()
                        ret = True

                End Select
            Catch ex As Exception
               

                Write_Log(SubName & ex.Message)
                ret = False
            End Try
            Return ret
        End Function

        ''' <summary>
        ''' 更新速度
        ''' </summary>
        ''' <param name="STATION_NAME"></param>
        ''' <param name="SPEED_VALUE"></param>
        ''' <param name="SubName"></param>
        ''' <param name="ErrLine"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function Update_Speed(ByVal Project_Name As String, ByVal STATION_NAME As String, ByVal SPEED_VALUE As Double, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Boolean
            Dim ret As Boolean = False
            Try
                SubName = ErrName & GetCurrentMethod.Name & "："
                Select Case DataBase_Link_Boolean
                    Case True
                        DataBase_Command = New SqlCommand("UPDATE PROJECT_COORDINATES_PARAMETERS SET 运行速度 = " & SPEED_VALUE & " WHERE (坐标名称 = N'" & STATION_NAME & "' AND 项目名称 = N'" & Project_Name & "')", DataBase_Connection)
                        DataBase_Command.Dispose()
                        ret = True
                        MessageBoxEx.Show("已经更新[" & DataBase_Command.ExecuteNonQuery() & "条记录！", "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1)
                End Select
            Catch ex As Exception
                Write_Log(SubName & ex.Message)
                ret = False
            End Try
            Return ret
        End Function

        ''' <summary>
        ''' 更新加减速度
        ''' </summary>
        ''' <param name="STATION_NAME"></param>
        ''' <param name="SPEED_VALUE"></param>
        ''' <param name="SubName"></param>
        ''' <param name="ErrLine"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function Update_ACC_DCC(ByVal Project_Name As String, ByVal STATION_NAME As String, ByVal SPEED_VALUE As Double, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Boolean
            Dim ret As Boolean = False
            Try
                SubName = ErrName & GetCurrentMethod.Name & "："
                Select Case DataBase_Link_Boolean
                    Case True
                        DataBase_Command = New SqlCommand("UPDATE PROJECT_COORDINATES_PARAMETERS SET 加减速度 = " & SPEED_VALUE & " WHERE (坐标名称 = N'" & STATION_NAME & "' AND 项目名称 = N'" & Project_Name & "')", DataBase_Connection)
                        DataBase_Command.Dispose()
                        ret = True
                        MessageBoxEx.Show("已经更新[" & DataBase_Command.ExecuteNonQuery() & "条记录！", "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1)
                End Select
            Catch ex As Exception


                Write_Log(SubName & ex.Message)
                ret = False
            End Try
            Return ret
        End Function

        ''' <summary>
        ''' 查询项目名称是否存在
        ''' </summary>
        ''' <param name="Project_Name">要查询的项目名称</param>
        ''' <returns>返回值：TEUE（存在），FALSE（不存在）</returns>
        ''' <remarks></remarks>
        Function Exists_Project_Name(ByVal Project_Name As String, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Boolean
            Dim ret As Boolean = False
            Try
                SubName = ErrName & GetCurrentMethod.Name & ":"
                Select Case DataBase_Link_Boolean
                    Case True
                        DataBase_Command = New SqlCommand("SELECT COUNT(*) AS Expr1 FROM Project_Parameters GROUP BY 项目名称 HAVING (项目名称 = '" & Project_Name & "')", DataBase_Connection)
                        If DataBase_Command.ExecuteScalar > 0 Then
                            DataBase_Command.Dispose()
                            ret = True
                        Else
                            DataBase_Command.Dispose()
                            ret = False
                        End If
                End Select
            Catch ex As Exception
               

                Write_Log(SubName & ex.Message)
                ret = False
            End Try
            Return ret
        End Function

        ''' <summary>
        ''' 判断坐标系名称是否存在
        ''' </summary>
        ''' <param name="COORDINATE_NAME">坐标系名称</param>
        ''' <param name="SubName"></param>
        ''' <param name="ErrLine"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function Exists_Coordinate_Name(ByVal Project_Name As String, ByVal COORDINATE_NAME As String, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Boolean
            Dim ret As Boolean = False
            Try
                SubName = ErrName & GetCurrentMethod.Name & ":"
                Select Case DataBase_Link_Boolean
                    Case True
                        DataBase_Command = New SqlCommand("SELECT COUNT(*) AS Expr1 FROM PROJECT_COORDINATE_NAME WHERE (项目名称= '" & Project_Name & "') GROUP BY 坐标名称 HAVING (坐标名称 = '" & COORDINATE_NAME & "')", DataBase_Connection)
                        If DataBase_Command.ExecuteScalar > 0 Then
                            DataBase_Command.Dispose()
                            ret = True
                        Else
                            DataBase_Command.Dispose()
                            ret = False
                        End If
                End Select
            Catch ex As Exception
               

                Write_Log(SubName & ex.Message)
                ret = False
            End Try
            Return ret
        End Function



        ''' <summary>
        ''' 查询项目名称是否存在
        ''' </summary>
        ''' <param name="Project_Name_ComboBox">要查询的项目名称</param>
        ''' <returns>返回值：TEUE（存在），FALSE（不存在）</returns>
        ''' <remarks></remarks>
        Function Exists_Project_Name(ByVal Project_Name_ComboBox As DevComponents.DotNetBar.Controls.ComboBoxEx, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Boolean
            Dim ret As Boolean = False
            Try
                SubName = ErrName & GetCurrentMethod.Name & ":"
                Select Case DataBase_Link_Boolean
                    Case True
                        DataBase_Command = New SqlCommand("SELECT COUNT(*) AS Expr1 FROM Project_Parameters GROUP BY 项目名称 HAVING (项目名称 = '" & Project_Name_ComboBox.SelectedItem.ToString.Trim & "')", DataBase_Connection)
                        If DataBase_Command.ExecuteScalar > 0 Then
                            DataBase_Command.Dispose()
                            ret = True
                        Else
                            DataBase_Command.Dispose()
                            ret = False
                        End If
                End Select
            Catch ex As Exception
               

                Write_Log(SubName & ex.Message)
                ret = False
            End Try
            Return ret
        End Function

        ''' <summary>
        ''' 查询项目名称是否存在
        ''' </summary>
        ''' <param name="Project_Name_TextBox">要查询的项目名称</param>
        ''' <returns>返回值：TEUE（存在），FALSE（不存在）</returns>
        ''' <remarks></remarks>
        Function Exists_Project_Name(ByVal Project_Name_TextBox As DevComponents.DotNetBar.Controls.TextBoxX, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Boolean
            Dim ret As Boolean = False
            Try
                SubName = ErrName & GetCurrentMethod.Name & ":"
                Select Case DataBase_Link_Boolean
                    Case True
                        DataBase_Command = New SqlCommand("SELECT COUNT(*) AS Expr1 FROM Project_Parameters GROUP BY 项目名称 HAVING (项目名称 = '" & Project_Name_TextBox.Text & "')", DataBase_Connection)
                        If DataBase_Command.ExecuteScalar > 0 Then
                            DataBase_Command.Dispose()
                            ret = True
                        Else
                            DataBase_Command.Dispose()
                            ret = False
                        End If
                End Select
            Catch ex As Exception
               

                Write_Log(SubName & ex.Message)
                ret = False
            End Try
            Return ret
        End Function

        ''' <summary>
        ''' 查询项目名称是否存在
        ''' </summary>
        ''' <param name="Project_Name_TextBox">要查询的项目名称</param>
        ''' <returns>返回值：TEUE（存在），FALSE（不存在）</returns>
        ''' <remarks></remarks>
        Function Exists_Project_Name(ByVal Project_Name_TextBox As System.Windows.Forms.TextBox, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Boolean
            Dim ret As Boolean = False
            Try
                SubName = ErrName & GetCurrentMethod.Name & ":"
                Select Case DataBase_Link_Boolean
                    Case True
                        DataBase_Command = New SqlCommand("SELECT COUNT(*) AS Expr1 FROM Project_Parameters GROUP BY 项目名称 HAVING (项目名称 = '" & Project_Name_TextBox.Text & "')", DataBase_Connection)
                        If DataBase_Command.ExecuteScalar > 0 Then
                            DataBase_Command.Dispose()
                            ret = True
                        Else
                            DataBase_Command.Dispose()
                            ret = False
                        End If
                End Select
            Catch ex As Exception
               

                Write_Log(SubName & ex.Message)
                ret = False
            End Try
            Return ret
        End Function

        Sub Read_Data_Code_Left(ByRef code As String)
            Try
                Select Case DataBase_Link_Boolean
                    Case True
                        Dim DataBase_Command As New SqlCommand
                        Dim DataBase_ConnectionAdapter As SqlDataAdapter
                        Dim DataBase_Connection As New SqlConnection
                        Dim Color_List_DataSet As New DataSet

                        DataBase_Command.CommandText = "SELECT DISTINCT  漏光站左工位产品条码 FROM DEVICE_PARAMETERS WHERE (ID = 1)"
                        DataBase_ConnectionAdapter = New SqlDataAdapter(DataBase_Command.CommandText, DataBase_Connection)
                        DataBase_ConnectionAdapter.Fill(Color_List_DataSet)

                        code = Color_List_DataSet.Tables(0).Rows(0).Item(0).ToString.Trim()


                        DataBase_Command.Dispose()
                        DataBase_ConnectionAdapter.Dispose()

                End Select
            Catch ex As Exception
                Write_Log(ex.Message)
            End Try

        End Sub

        Sub Read_Data_Code_Right(ByRef code As String)
            Try
                Select Case DataBase_Link_Boolean
                    Case True
                        Dim DataBase_Command As New SqlCommand
                        Dim DataBase_ConnectionAdapter As SqlDataAdapter
                        Dim DataBase_Connection As New SqlConnection
                        Dim Color_List_DataSet As New DataSet

                        DataBase_Command.CommandText = "SELECT DISTINCT  漏光站右工位产品条码 FROM DEVICE_PARAMETERS WHERE (ID = 1)"
                        DataBase_ConnectionAdapter = New SqlDataAdapter(DataBase_Command.CommandText, DataBase_Connection)
                        DataBase_ConnectionAdapter.Fill(Color_List_DataSet)

                        code = Color_List_DataSet.Tables(0).Rows(0).Item(0).ToString.Trim()


                        DataBase_Command.Dispose()
                        DataBase_ConnectionAdapter.Dispose()

                End Select
            Catch ex As Exception
                Write_Log(ex.Message)
            End Try
        End Sub

        Sub RedData_NG键帽个数()
            DataBase_Command = New SqlCommand("SELECT 当前值 FROM DEVICE_DOUBLE_PARAMETERS WHERE (变量名称 = N'NG键帽个数')", DataBase_Connection)
            Dim ReStr As String = DataBase_Command.ExecuteScalar
            NG键帽个数 = ReStr.Trim
            DataBase_Command.Dispose()
        End Sub

        Sub UpData_NG键帽个数()
            DataBase_Command = New SqlCommand("UPDATE DEVICE_DOUBLE_PARAMETERS SET 当前值 = " & NG键帽个数 & " WHERE(变量名称 = N'NG键帽个数')", DataBase_Connection)
            DataBase_Command.ExecuteNonQuery()
            DataBase_Command.Dispose()
        End Sub
        ''' <summary>
        ''' 查询项目参数
        ''' </summary>
        ''' <param name="Parameter_Name">参数名称数组</param>
        ''' <param name="Parameter_Val">返回数据数组</param>
        ''' <param name="Rtn_Message">返回处理信息</param>
        ''' <remarks></remarks>
        Sub Read_Project_Parameter(ByVal Parameter_Name() As String, ByRef Parameter_Val() As Object, Optional ByVal IO_Boolean As Boolean = False, Optional ByRef Rtn_Message As String = Nothing)
            Try
                Select Case DataBase_Link_Boolean
                    Case True

                        Dim DataBase_Cmd As New SqlCommand
                        Dim DataBase_ConnectionAdapter As SqlDataAdapter

                        Dim Serch_Project_Parameter_DataSet As New DataSet
                        Serch_Project_Parameter_DataSet.Clear()
                        Dim Parameter_Count As Integer = Parameter_Name.Length
                        Array.Resize(Parameter_Val, Parameter_Count)
                        Dim Total_Str As String = Nothing
                        For i As Integer = 0 To Parameter_Count - 1
                            Select Case i
                                Case Parameter_Count - 1
                                    Total_Str = Total_Str & Parameter_Name(i)
                                Case Else
                                    Total_Str = Total_Str & Parameter_Name(i) & ", "
                            End Select
                        Next
                        Select Case IO_Boolean
                            Case True
                                DataBase_Cmd.CommandText = "SELECT DISTINCT  " & Total_Str & "  FROM HARDWARE_PARAMETERS WHERE (ID = 1)"
                            Case False
                                DataBase_Cmd.CommandText = "SELECT DISTINCT  " & Total_Str & "  FROM DEVICE_PARAMETERS WHERE (ID = 1)"
                        End Select
                        DataBase_ConnectionAdapter = New SqlDataAdapter(DataBase_Cmd.CommandText, DataBase_Connection)
                        DataBase_ConnectionAdapter.Fill(Serch_Project_Parameter_DataSet)
                        For i As Integer = 0 To Serch_Project_Parameter_DataSet.Tables(0).Columns.Count - 1
                            If IsDBNull(Serch_Project_Parameter_DataSet.Tables(0).Rows(0).Item(i)) = False Then
                                Parameter_Val(i) = Serch_Project_Parameter_DataSet.Tables(0).Rows(0).Item(i)
                            Else
                                Parameter_Val(i) = "0"
                            End If
                        Next


                        Serch_Project_Parameter_DataSet.Dispose()
                        DataBase_Command.Dispose()
                        DataBase_ConnectionAdapter.Dispose()
                        Rtn_Message = "项目参数查询完成。"
                End Select
            Catch ex As Exception
                Write_Log(ex.Message)
            End Try

        End Sub

        ''' <summary>
        ''' 查询项目参数
        ''' </summary>
        ''' <param name="Project_Name">项目名称</param>
        ''' <param name="Parameter_Name">参数名称数组</param>
        ''' <param name="Parameter_Val">返回数据数组</param>
        ''' <remarks></remarks>
        Function Read_Project_Parameter(ByVal Project_Name As String, ByVal Parameter_Name() As String, ByRef Parameter_Val() As Object, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Boolean
            Dim ret As Boolean = False
            Try
                SubName = ErrName & GetCurrentMethod.Name & ":"
                Select Case DataBase_Link_Boolean
                    Case True
                        Select Case Exists_Project_Name(Project_Name)
                            Case True
                                Dim Serch_Project_Parameter_DataSet As New DataSet
                                Serch_Project_Parameter_DataSet.Clear()
                                Dim Parameter_Count As Integer = Parameter_Name.Length
                                Array.Resize(Parameter_Val, Parameter_Count)
                                Dim Total_Str As String = Nothing
                                For i As Integer = 0 To Parameter_Count - 1
                                    Select Case i
                                        Case Parameter_Count - 1
                                            Total_Str = Total_Str & Parameter_Name(i)
                                        Case Else
                                            Total_Str = Total_Str & Parameter_Name(i) & ", "
                                    End Select
                                Next
                                DataBase_Command.CommandText = "SELECT DISTINCT  " & Total_Str & "  FROM PROJECT_PARAMETERS WHERE (项目名称 = '" & Project_Name & "')"
                                DataBase_ConnectionAdapter = New SqlDataAdapter(DataBase_Command.CommandText, DataBase_Connection)
                                DataBase_ConnectionAdapter.Fill(Serch_Project_Parameter_DataSet)
                                For i As Integer = 0 To Serch_Project_Parameter_DataSet.Tables(0).Columns.Count - 1
                                    If IsDBNull(Serch_Project_Parameter_DataSet.Tables(0).Rows(0).Item(i)) = False Then
                                        Parameter_Val(i) = Serch_Project_Parameter_DataSet.Tables(0).Rows(0).Item(i)
                                    Else
                                        Parameter_Val(i) = "0"
                                    End If
                                Next
                                Serch_Project_Parameter_DataSet.Dispose()
                                DataBase_Command.Dispose()
                                DataBase_ConnectionAdapter.Dispose()
                                ret = True
                        End Select
                End Select
            Catch ex As Exception
               

                Write_Log(SubName & ex.Message)
                ret = False
            End Try
            Return ret
        End Function



        ''' <summary>
        ''' 查询对象参数
        ''' </summary>
        ''' <param name="Table_Name">项目名称</param>
        ''' <param name="Parameter_Name">参数名称数组</param>
        ''' <param name="Parameter_DataGrid">返回数据数组</param>
        ''' <remarks></remarks>
        Function Read_Object_Parameter(ByVal Table_Name As String, ByVal Parameter_Name() As String, ByRef Parameter_DataGrid As DevComponents.DotNetBar.Controls.DataGridViewX, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Boolean
            Dim ret As Boolean = False
            Try
                SubName = ErrName & GetCurrentMethod.Name & ":"
                Select Case DataBase_Link_Boolean
                    Case True
                        Parameter_DataGrid.Columns.Clear()
                        Dim Object_Parameter_DataSet As New DataSet
                        Object_Parameter_DataSet.Clear()
                        Dim Parameter_Count As Integer = Parameter_Name.Length
                        Dim Total_Str As String = Nothing
                        For i As Integer = 0 To Parameter_Count - 1
                            Select Case i
                                Case Parameter_Count - 1
                                    Total_Str = Total_Str & Parameter_Name(i)
                                Case Else
                                    Total_Str = Total_Str & Parameter_Name(i) & ", "
                            End Select
                        Next
                        DataBase_Command.CommandText = "SELECT " & Total_Str & " FROM " & Table_Name & ")"
                        DataBase_ConnectionAdapter = New SqlDataAdapter(DataBase_Command.CommandText, DataBase_Connection)
                        DataBase_ConnectionAdapter.Fill(Object_Parameter_DataSet)
                        Parameter_DataGrid.DataSource = Object_Parameter_DataSet.Tables(0)
                        For I = 0 To Parameter_DataGrid.Columns.Count - 1
                            Parameter_DataGrid.Columns(I).ReadOnly = True
                            Parameter_DataGrid.Columns(I).SortMode = Windows.Forms.DataGridViewColumnSortMode.NotSortable
                            Select Case I
                                Case 0
                                    Parameter_DataGrid.Columns(I).AutoSizeMode = Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader
                                Case 1
                                    Parameter_DataGrid.Columns(I).AutoSizeMode = Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
                                Case Else
                                    Parameter_DataGrid.Columns(I).AutoSizeMode = Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
                            End Select
                        Next
                        Parameter_DataGrid.RowHeadersVisible = False
                        Parameter_DataGrid.AllowUserToDeleteRows = False
                        Parameter_DataGrid.AllowUserToResizeColumns = False
                        Parameter_DataGrid.AllowUserToResizeRows = False
                        Parameter_DataGrid.ColumnHeadersHeightSizeMode = Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
                        Parameter_DataGrid.MultiSelect = False
                        Parameter_DataGrid.ReadOnly = True
                        Parameter_DataGrid.RowHeadersWidthSizeMode = Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
                        Parameter_DataGrid.SelectionMode = Windows.Forms.DataGridViewSelectionMode.FullRowSelect
                        DataBase_Command.Dispose()
                        Object_Parameter_DataSet.Dispose()
                        DataBase_ConnectionAdapter.Dispose()
                        ret = True
                End Select
            Catch ex As Exception
               

                Write_Log(SubName & ex.Message)
                ret = False
            End Try
            Return ret
        End Function

        ''' <summary>
        ''' 查询对象参数
        ''' </summary>
        ''' <param name="Table_Name">项目名称</param>
        ''' <param name="Parameter_Name">参数名称数组</param>
        ''' <param name="Parameter_Val">返回数据数组</param>
        ''' <remarks></remarks>
        Function Read_Object_Parameter(ByVal Table_Name As String, ByVal Parameter_Name() As String, ByRef Parameter_Val() As Object, ByVal ID As UInteger, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Boolean
            Dim ret As Boolean = False
            Try
                SubName = ErrName & GetCurrentMethod.Name & ":"
                Select Case DataBase_Link_Boolean
                    Case True
                        Dim Serch_Project_Parameter_DataSet As New DataSet
                        Serch_Project_Parameter_DataSet.Clear()
                        Dim Parameter_Count As Integer = Parameter_Name.Length
                        Array.Resize(Parameter_Val, Parameter_Count)
                        Dim Total_Str As String = Nothing
                        For i As Integer = 0 To Parameter_Count - 1
                            Select Case i
                                Case Parameter_Count - 1
                                    Total_Str = Total_Str & Parameter_Name(i)
                                Case Else
                                    Total_Str = Total_Str & Parameter_Name(i) & ", "
                            End Select
                        Next
                        DataBase_Command.CommandText = "SELECT DISTINCT " & Total_Str & "  FROM " & Table_Name & ") WHERE ID=" & ID & ""
                        DataBase_ConnectionAdapter = New SqlDataAdapter(DataBase_Command.CommandText, DataBase_Connection)
                        DataBase_ConnectionAdapter.Fill(Serch_Project_Parameter_DataSet)
                        For i As Integer = 0 To Serch_Project_Parameter_DataSet.Tables(0).Columns.Count - 1
                            If IsDBNull(Serch_Project_Parameter_DataSet.Tables(0).Rows(0).Item(i)) = False Then
                                Parameter_Val(i) = Serch_Project_Parameter_DataSet.Tables(0).Rows(0).Item(i)
                            Else
                                Parameter_Val(i) = "0"
                            End If
                        Next
                        Serch_Project_Parameter_DataSet.Dispose()
                        DataBase_Command.Dispose()
                        DataBase_ConnectionAdapter.Dispose()
                        ret = True
                End Select
            Catch ex As Exception
               

                Write_Log(SubName & ex.Message)
                ret = False
            End Try
            Return ret
        End Function

        ''' <summary>
        ''' 查询项目参数
        ''' </summary>
        ''' <param name="Project_Name">项目名称</param>
        ''' <param name="Parameter_Name">参数名称数组</param>
        ''' <param name="Parameter_Panel">返回数据数组</param>
        ''' <remarks></remarks>
        Function Read_Project_Parameter(ByVal Project_Name As String, ByVal Parameter_Name() As String, ByRef Parameter_Panel() As DevComponents.Editors.DoubleInput, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Boolean
            Dim ret As Boolean = False
            Try
                SubName = ErrName & GetCurrentMethod.Name & ":"
                Select Case DataBase_Link_Boolean
                    Case True
                        Select Case Exists_Project_Name(Project_Name)
                            Case True
                                Dim Serch_Project_Parameter_DataSet As New DataSet
                                Serch_Project_Parameter_DataSet.Clear()
                                Dim Parameter_Count As Integer = Parameter_Name.Length
                                Dim Total_Str As String = Nothing
                                For i As Integer = 0 To Parameter_Count - 1
                                    Select Case i
                                        Case Parameter_Count - 1
                                            Total_Str = Total_Str & Parameter_Name(i)
                                        Case Else
                                            Total_Str = Total_Str & Parameter_Name(i) & ", "
                                    End Select
                                Next
                                DataBase_Command.CommandText = "SELECT DISTINCT  " & Total_Str & "  FROM PROJECT_PARAMETERS WHERE (项目名称 = '" & Project_Name & "')"
                                DataBase_ConnectionAdapter = New SqlDataAdapter(DataBase_Command.CommandText, DataBase_Connection)
                                DataBase_ConnectionAdapter.Fill(Serch_Project_Parameter_DataSet)
                                For i As Integer = 0 To Serch_Project_Parameter_DataSet.Tables(0).Columns.Count - 1
                                    If IsDBNull(Serch_Project_Parameter_DataSet.Tables(0).Rows(0).Item(i)) = False Then
                                        Parameter_Panel(i).Value = Serch_Project_Parameter_DataSet.Tables(0).Rows(0).Item(i)
                                    Else
                                        Parameter_Panel(i).Value = 0
                                    End If
                                Next
                                Serch_Project_Parameter_DataSet.Dispose()
                                DataBase_Command.Dispose()
                                DataBase_ConnectionAdapter.Dispose()
                                ret = True
                        End Select
                End Select
            Catch ex As Exception
               

                Write_Log(SubName & ex.Message)
                ret = False
            End Try
            Return ret
        End Function

        ''' <summary>
        ''' 查询硬件参数
        ''' </summary>
        ''' <param name="Parameter_Name">参数名称数组</param>
        ''' <param name="Parameter_Val">返回数据数组</param>
        ''' <remarks></remarks>
        Function Read_Hardware_Parameter(ByVal Parameter_Name() As String, ByRef Parameter_Val() As Object, Optional ByVal IO_Boolean As Boolean = False, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Boolean
            Dim ret As Boolean = False
            Try
                SubName = ErrName & GetCurrentMethod.Name & ":"
                Select Case DataBase_Link_Boolean
                    Case True
                        Dim Serch_Project_Parameter_DataSet As New DataSet
                        Serch_Project_Parameter_DataSet.Clear()
                        Dim Parameter_Count As Integer = Parameter_Name.Length
                        Array.Resize(Parameter_Val, Parameter_Count)
                        Dim Total_Str As String = Nothing
                        For i As Integer = 0 To Parameter_Count - 1
                            Select Case i
                                Case Parameter_Count - 1
                                    Total_Str = Total_Str & Parameter_Name(i)
                                Case Else
                                    Total_Str = Total_Str & Parameter_Name(i) & ", "
                            End Select
                        Next
                        Select Case IO_Boolean
                            Case True
                                DataBase_Command.CommandText = "SELECT DISTINCT  " & Total_Str & "  FROM HARDWARE_PARAMETERS WHERE (ID = 1)"
                            Case False
                                DataBase_Command.CommandText = "SELECT DISTINCT  " & Total_Str & "  FROM DEVICE_PARAMETERS WHERE (ID = 1)"
                        End Select
                        DataBase_ConnectionAdapter = New SqlDataAdapter(DataBase_Command.CommandText, DataBase_Connection)
                        DataBase_ConnectionAdapter.Fill(Serch_Project_Parameter_DataSet)
                        For i As Integer = 0 To Serch_Project_Parameter_DataSet.Tables(0).Columns.Count - 1
                            If IsDBNull(Serch_Project_Parameter_DataSet.Tables(0).Rows(0).Item(i)) = False Then
                                Parameter_Val(i) = Serch_Project_Parameter_DataSet.Tables(0).Rows(0).Item(i)
                            Else
                                Parameter_Val(i) = "0"
                            End If
                        Next
                        Serch_Project_Parameter_DataSet.Dispose()
                        DataBase_Command.Dispose()
                        DataBase_ConnectionAdapter.Dispose()
                        ret = True
                End Select
            Catch ex As Exception
               

                Write_Log(SubName & ex.Message)
                ret = False
            End Try
            Return ret
        End Function

        ''' <summary>
        ''' 查询项目参数
        ''' </summary>
        ''' <param name="Project_Name">项目名称</param>
        ''' <param name="Parameter_Name">参数名称数组</param>
        ''' <param name="Parameter_Panel">返回数据数组</param>
        ''' <remarks></remarks>
        Function Read_Project_Parameter(ByVal Project_Name As String, ByVal Parameter_Name() As String, ByRef Parameter_Panel() As DevComponents.DotNetBar.PanelEx, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Boolean
            Dim ret As Boolean = False
            Try
                SubName = ErrName & GetCurrentMethod.Name & ":"
                Select Case DataBase_Link_Boolean
                    Case True
                        Select Case Exists_Project_Name(Project_Name)
                            Case True
                                Dim Serch_Project_Parameter_DataSet As New DataSet
                                Serch_Project_Parameter_DataSet.Clear()
                                Dim Parameter_Count As Integer = Parameter_Name.Length
                                Dim Total_Str As String = Nothing
                                For i As Integer = 0 To Parameter_Count - 1
                                    Select Case i
                                        Case Parameter_Count - 1
                                            Total_Str = Total_Str & Parameter_Name(i)
                                        Case Else
                                            Total_Str = Total_Str & Parameter_Name(i) & ", "
                                    End Select
                                Next
                                DataBase_Command.CommandText = "SELECT DISTINCT  " & Total_Str & "  FROM Project_Parameters WHERE (项目名称 = '" & Project_Name & "')"
                                DataBase_ConnectionAdapter = New SqlDataAdapter(DataBase_Command.CommandText, DataBase_Connection)
                                DataBase_ConnectionAdapter.Fill(Serch_Project_Parameter_DataSet)
                                For i As Integer = 0 To Serch_Project_Parameter_DataSet.Tables(0).Columns.Count - 1
                                    If IsDBNull(Serch_Project_Parameter_DataSet.Tables(0).Rows(0).Item(i)) = False Then
                                        Parameter_Panel(i).Text = Serch_Project_Parameter_DataSet.Tables(0).Rows(0).Item(i)
                                    Else
                                        Parameter_Panel(i).Text = "NULL"
                                    End If
                                Next
                                Serch_Project_Parameter_DataSet.Dispose()
                                DataBase_Command.Dispose()
                                DataBase_ConnectionAdapter.Dispose()
                                ret = True
                            Case False
                                ret = False
                        End Select
                End Select
            Catch ex As Exception
               

                Write_Log(SubName & ex.Message)
                ret = False
            End Try
            Return ret
        End Function

        ''' <summary>
        ''' 读取项目参数
        ''' </summary>
        ''' <param name="Project_Name">项目名称</param>
        ''' <param name="Parameter_Name">参数数组</param>
        ''' <param name="Project_Parameter_DataGridView">参数表</param>
        ''' <param name="Parameter_Object">参数数据</param>
        ''' <remarks></remarks>
        Function Read_Project_Parameter(ByVal Project_Name As String, ByVal Parameter_Name() As String, ByRef Project_Parameter_DataGridView As DevComponents.DotNetBar.Controls.DataGridViewX, ByRef Parameter_Object() As Object, Optional ByVal Fit_Bool As Boolean = True, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Boolean
            Dim ret As Boolean = False
            Try
                SubName = ErrName & GetCurrentMethod.Name & ":"
                Select Case DataBase_Link_Boolean
                    Case True
                        Project_Parameter_DataGridView.Columns.Clear()
                        Dim Serch_Project_Parameter_DataSet As New DataSet
                        Serch_Project_Parameter_DataSet.Clear()
                        Dim Parameter_Count As Integer = Parameter_Name.Length
                        Dim Total_Str As String = Nothing
                        For i As Integer = 0 To Parameter_Count - 1
                            Select Case i
                                Case Parameter_Count - 1
                                    Total_Str = Total_Str & Parameter_Name(i)
                                Case Else
                                    Total_Str = Total_Str & Parameter_Name(i) & ", "
                            End Select
                        Next
                        DataBase_Command.CommandText = "SELECT DISTINCT  " & Total_Str & "  FROM Project_Parameters WHERE (项目名称 = '" & Project_Name.Trim & "')"
                        DataBase_ConnectionAdapter = New SqlDataAdapter(DataBase_Command.CommandText, DataBase_Connection)
                        DataBase_ConnectionAdapter.Fill(Serch_Project_Parameter_DataSet)
                        Project_Parameter_DataGridView.DataSource = Serch_Project_Parameter_DataSet.Tables(0)
                        Array.Resize(Parameter_Object, Project_Parameter_DataGridView.Columns.Count)
                        For I = 0 To Project_Parameter_DataGridView.Columns.Count - 1
                            Project_Parameter_DataGridView.Columns(I).ReadOnly = True
                            Project_Parameter_DataGridView.Columns(I).SortMode = Windows.Forms.DataGridViewColumnSortMode.NotSortable
                            If Fit_Bool = True Then
                                Project_Parameter_DataGridView.Columns(I).AutoSizeMode = Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
                            Else
                                Project_Parameter_DataGridView.Columns(I).AutoSizeMode = Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader
                            End If

                            If IsDBNull(Project_Parameter_DataGridView.Item(I, 0).Value) = False Then
                                Parameter_Object(I) = Project_Parameter_DataGridView.Item(I, 0).Value
                            Else
                                Parameter_Object(I) = -1
                            End If
                        Next
                        If Project_Parameter_DataGridView.Rows.Count > 1 Then
                            Project_Parameter_DataGridView.FirstDisplayedScrollingRowIndex = Project_Parameter_DataGridView.Rows.Count - 2
                            Project_Parameter_DataGridView.Rows(Project_Parameter_DataGridView.Rows.Count - 2).Selected = True
                        End If
                        Project_Parameter_DataGridView.RowHeadersVisible = False
                        Project_Parameter_DataGridView.AllowUserToDeleteRows = False
                        Project_Parameter_DataGridView.AllowUserToResizeColumns = False
                        Project_Parameter_DataGridView.AllowUserToResizeRows = False
                        Project_Parameter_DataGridView.ColumnHeadersHeightSizeMode = Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
                        Project_Parameter_DataGridView.MultiSelect = False
                        Project_Parameter_DataGridView.ReadOnly = True
                        Project_Parameter_DataGridView.RowHeadersWidthSizeMode = Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
                        Project_Parameter_DataGridView.SelectionMode = Windows.Forms.DataGridViewSelectionMode.FullRowSelect
                        DataBase_Command.Dispose()
                        Serch_Project_Parameter_DataSet.Dispose()
                End Select
            Catch ex As Exception
               

                Write_Log(SubName & ex.Message)
                ret = False
            End Try
            Return ret
        End Function

        Sub Update_Device_Station(ByVal Station As String)
            Select Case DataBase_Link_Boolean
                Case True
                    Try
                        DataBase_Command = New SqlCommand("SELECT COUNT(ID) AS ID FROM DEVICE_INFO WHERE (ID = 1)", DataBase_Connection)
                        If CType(DataBase_Command.ExecuteScalar, Integer) > 0 Then
                            DataBase_Command = New SqlCommand("UPDATE DEVICE_INFO SET 设备名称 = N'" & Station & "' WHERE (ID = 1)", DataBase_Connection)
                            DataBase_Command.ExecuteNonQuery()
                            DataBase_Command.Dispose()
                        End If
                    Catch ex As Exception
                        Write_Log(ex.Message)
                    End Try

            End Select
        End Sub

        Function Insert_Temp_Barcode_left(ByVal SN As String)
            Dim ret As Boolean = False, SubName As String = ErrName & GetCurrentMethod.Name & ":"
            Try
                Select Case DataBase_Link_Boolean
                    Case True
                        Dim DataBase_Com As SqlCommand
                        DataBase_Com = New SqlCommand("INSERT INTO TEMP_BARCODE (BarCode) VALUES  (N'" & SN & "')", DataBase_Connection)
                        DataBase_Com.ExecuteNonQuery()
                        DataBase_Com.Dispose()
                        ret = True
                End Select
            Catch ex As Exception
               
                Write_Log(SubName & ex.Message)
                ret = False
            End Try
            Return ret
        End Function

        Function Insert_Temp_Barcode_Right(ByVal SN As String)
            Dim ret As Boolean = False, SubName As String = ErrName & GetCurrentMethod.Name & ":"
            Try
                Select Case DataBase_Link_Boolean
                    Case True
                        Dim DataBase_Com As SqlCommand
                        DataBase_Com = New SqlCommand("INSERT INTO TEMP_BARCODE (BarCode) VALUES  (N'" & SN & "')", DataBase_Connection)
                        DataBase_Com.ExecuteNonQuery()
                        DataBase_Com.Dispose()
                        ret = True
                End Select
            Catch ex As Exception
               
                Write_Log(SubName & ex.Message)
                ret = False
            End Try
            Return ret
        End Function

        Function DELETE_Temp_Barcode_Left(ByVal SN As String)
            Dim ret As Boolean = False, SubName As String = ErrName & GetCurrentMethod.Name & ":"
            Try
                Select Case DataBase_Link_Boolean
                    Case True
                        Dim DataBase_Com As SqlCommand
                        DataBase_Com = New SqlCommand("DELETE FROM TEMP_BARCODE WHERE (BarCode = N'" & SN & "')", DataBase_Connection)
                        DataBase_Com.ExecuteNonQuery()
                        DataBase_Com.Dispose()
                        SN = True
                End Select
            Catch ex As Exception
               
                Write_Log(SubName & ex.Message)
                ret = False
            End Try
            Return ret
        End Function

        Function DELETE_Temp_Barcode_Right(ByVal SN As String)
            Dim ret As Boolean = False, SubName As String = ErrName & GetCurrentMethod.Name & ":"
            Try
                Select Case DataBase_Link_Boolean
                    Case True
                        Dim DataBase_Com As SqlCommand
                        DataBase_Com = New SqlCommand("DELETE FROM TEMP_BARCODE WHERE (BarCode = N'" & SN & "')", DataBase_Connection)
                        DataBase_Com.ExecuteNonQuery()
                        DataBase_Com.Dispose()
                        SN = True
                End Select
            Catch ex As Exception
               
                Write_Log(SubName & ex.Message)
                ret = False
            End Try
            Return ret
        End Function

        Function Query_Temp_Barcode(ByVal SN As String) As Boolean
            Dim ret As Boolean = False, SubName As String = ErrName & GetCurrentMethod.Name & ":"
            Try
                Select Case DataBase_Link_Boolean
                    Case True
                        Dim DataBase_Com As SqlCommand
                        DataBase_Com = New SqlCommand("SELECT COUNT(ID) FROM TEMP_BARCODE WHERE BarCode=N'" & SN & "'", DataBase_Connection)
                        Dim t1 As Integer = DataBase_Com.ExecuteScalar()
                        If t1 > 0 Then
                            ret = True
                        Else
                            ret = False
                        End If
                        DataBase_Com.Dispose()
                End Select
            Catch ex As Exception
               
                Write_Log(SubName & ex.Message)
                ret = False
            End Try
            Return ret
        End Function

        ''' <summary>
        ''' 更新产品参数
        ''' </summary>
        ''' <param name="Parameter_Name">项目参数</param>
        ''' <param name="Parameter_Value">参数变量</param>
        ''' <remarks></remarks>
        Function Update_Project_Parameter_Left(ByVal Parameter_Name() As String, ByVal Parameter_Value() As Object, Optional ByVal ID As Integer = 1, Optional ByVal IO_Boolean As Boolean = False, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Boolean
            Dim ret As Boolean = False
            Try
                SubName = ErrName & GetCurrentMethod.Name & ":"
                Dim DataBase_Cmd As SqlCommand = Nothing
                Select Case DataBase_Link_Boolean
                    Case True
                        Dim Parameter_Count As Integer = Parameter_Name.Length
                        Dim Total_Parameter_Name_Str As String = Nothing
                        For i As Integer = 0 To Parameter_Count - 1
                            Select Case i
                                Case Parameter_Count - 1
                                    Select Case IsNumeric(Parameter_Value(i))
                                        Case True
                                            Total_Parameter_Name_Str = Total_Parameter_Name_Str & Parameter_Name(i) & " = " & Parameter_Value(i)
                                        Case False
                                            Total_Parameter_Name_Str = Total_Parameter_Name_Str & Parameter_Name(i) & " = '" & Parameter_Value(i) & "'"
                                    End Select
                                Case Else
                                    Select Case IsNumeric(Parameter_Value(i))
                                        Case True
                                            Total_Parameter_Name_Str = Total_Parameter_Name_Str & Parameter_Name(i) & " = " & Parameter_Value(i) & ", "
                                        Case False
                                            Total_Parameter_Name_Str = Total_Parameter_Name_Str & Parameter_Name(i) & " = '" & Parameter_Value(i) & "',"
                                    End Select
                            End Select
                        Next

                        Select Case IO_Boolean
                            Case True
                                DataBase_Cmd = New SqlCommand("UPDATE HARDWARE_PARAMETERS SET " & Total_Parameter_Name_Str & " WHERE (ID =" & ID & ")", DataBase_Connection)
                            Case False
                                DataBase_Cmd = New SqlCommand("UPDATE DEVICE_PARAMETERS SET " & Total_Parameter_Name_Str & " WHERE (ID =" & ID & ")", DataBase_Connection)
                        End Select
                        DataBase_Cmd.ExecuteNonQuery()
                        DataBase_Cmd.Dispose()
                        ret = True
                End Select
            Catch ex As Exception
                Write_Log(SubName & ex.Message)
                ret = False
            End Try
            Return ret
        End Function

        ''' <summary>
        ''' 更新产品参数
        ''' </summary>
        ''' <param name="Parameter_Name">项目参数</param>
        ''' <param name="Parameter_Value">参数变量</param>
        ''' <remarks></remarks>
        Function Update_Project_Parameter_Right(ByVal Parameter_Name() As String, ByVal Parameter_Value() As Object, Optional ByVal ID As Integer = 1, Optional ByVal IO_Boolean As Boolean = False, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Boolean
            Dim ret As Boolean = False
            Try
                SubName = ErrName & GetCurrentMethod.Name & ":"
                Dim DataBase_Cmd As SqlCommand = Nothing
                Select Case DataBase_Link_Boolean
                    Case True
                        Dim Parameter_Count As Integer = Parameter_Name.Length
                        Dim Total_Parameter_Name_Str As String = Nothing
                        For i As Integer = 0 To Parameter_Count - 1
                            Select Case i
                                Case Parameter_Count - 1
                                    Select Case IsNumeric(Parameter_Value(i))
                                        Case True
                                            Total_Parameter_Name_Str = Total_Parameter_Name_Str & Parameter_Name(i) & " = " & Parameter_Value(i)
                                        Case False
                                            Total_Parameter_Name_Str = Total_Parameter_Name_Str & Parameter_Name(i) & " = '" & Parameter_Value(i) & "'"
                                    End Select
                                Case Else
                                    Select Case IsNumeric(Parameter_Value(i))
                                        Case True
                                            Total_Parameter_Name_Str = Total_Parameter_Name_Str & Parameter_Name(i) & " = " & Parameter_Value(i) & ", "
                                        Case False
                                            Total_Parameter_Name_Str = Total_Parameter_Name_Str & Parameter_Name(i) & " = '" & Parameter_Value(i) & "',"
                                    End Select
                            End Select
                        Next

                        Select Case IO_Boolean
                            Case True
                                DataBase_Cmd = New SqlCommand("UPDATE HARDWARE_PARAMETERS SET " & Total_Parameter_Name_Str & " WHERE (ID =" & ID & ")", DataBase_Connection)
                            Case False
                                DataBase_Cmd = New SqlCommand("UPDATE DEVICE_PARAMETERS SET " & Total_Parameter_Name_Str & " WHERE (ID =" & ID & ")", DataBase_Connection)
                        End Select
                        DataBase_Cmd.ExecuteNonQuery()
                        DataBase_Cmd.Dispose()
                        ret = True
                End Select
            Catch ex As Exception
                Write_Log(SubName & ex.Message)
                ret = False
            End Try
            Return ret
        End Function
        ''' <summary>
        ''' 更新产品参数
        ''' </summary>
        ''' <param name="Parameter_Name">项目参数</param>
        ''' <param name="Parameter_Value">参数变量</param>
        ''' <remarks></remarks>
        Function Update_Project_Parameter(ByVal Parameter_Name() As String, ByVal Parameter_Value() As Object, Optional ByVal ID As Integer = 1, Optional ByVal IO_Boolean As Boolean = False, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Boolean
            Dim ret As Boolean = False
            Try
                SubName = ErrName & GetCurrentMethod.Name & "："
                Select Case DataBase_Link_Boolean
                    Case True
                        Dim Parameter_Count As Integer = Parameter_Name.Length
                        Dim Total_Parameter_Name_Str As String = Nothing
                        For i As Integer = 0 To Parameter_Count - 1
                            Select Case i
                                Case Parameter_Count - 1
                                    Select Case IsNumeric(Parameter_Value(i))
                                        Case True
                                            Total_Parameter_Name_Str = Total_Parameter_Name_Str & Parameter_Name(i) & " = " & Parameter_Value(i)
                                        Case False
                                            Total_Parameter_Name_Str = Total_Parameter_Name_Str & Parameter_Name(i) & " = '" & Parameter_Value(i) & "'"
                                    End Select
                                Case Else
                                    Select Case IsNumeric(Parameter_Value(i))
                                        Case True
                                            Total_Parameter_Name_Str = Total_Parameter_Name_Str & Parameter_Name(i) & " = " & Parameter_Value(i) & ", "
                                        Case False
                                            Total_Parameter_Name_Str = Total_Parameter_Name_Str & Parameter_Name(i) & " = '" & Parameter_Value(i) & "',"
                                    End Select
                            End Select
                        Next
                        Select Case IO_Boolean
                            Case True
                                DataBase_Command = New SqlCommand("UPDATE HARDWARE_PARAMETERS SET " & Total_Parameter_Name_Str & " WHERE (ID =" & ID & ")", DataBase_Connection)
                                DataBase_Command.ExecuteNonQuery()
                            Case False
                                DataBase_Command = New SqlCommand("UPDATE DEVICE_PARAMETERS SET " & Total_Parameter_Name_Str & " WHERE (ID =" & ID & ")", DataBase_Connection)
                                DataBase_Command.ExecuteNonQuery()
                        End Select
                        ret = True
                End Select
            Catch ex As Exception


                Write_Log(SubName & ex.Message)
                ret = False
            End Try
            Return ret
        End Function
        ''' <summary>
        ''' 更新产品参数
        ''' </summary>
        ''' <param name="Project_Name">项目名称</param>
        ''' <param name="Parameter_Name">项目参数</param>
        ''' <param name="Parameter_Value">参数变量</param>
        ''' <remarks></remarks>
        Function Update_Project_Parameter(ByVal Project_Name As String, ByVal Parameter_Name() As String, ByVal Parameter_Value() As Object, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Boolean
            Dim ret As Boolean = False
            Try
                SubName = ErrName & GetCurrentMethod.Name & ":"
                Select Case DataBase_Link_Boolean
                    Case True
                        Dim Parameter_Count As Integer = Parameter_Name.Length
                        Dim Total_Parameter_Name_Str As String = Nothing
                        For i As Integer = 0 To Parameter_Count - 1
                            Select Case i
                                Case Parameter_Count - 1
                                    Total_Parameter_Name_Str = Total_Parameter_Name_Str & Parameter_Name(i) & " = " & Parameter_Value(i)
                                Case Else
                                    Total_Parameter_Name_Str = Total_Parameter_Name_Str & Parameter_Name(i) & " = " & Parameter_Value(i) & ", "
                            End Select
                        Next
                        DataBase_Command = New SqlCommand("UPDATE PROJECT_PARAMETERS SET " & Total_Parameter_Name_Str & " WHERE (项目名称 = N'" & Project_Name & "')", DataBase_Connection)
                        DataBase_Command.ExecuteNonQuery()
                        DataBase_Command.Dispose()
                        ret = True
                End Select
            Catch ex As Exception
               

                Write_Log(SubName & ex.Message)
                ret = False
            End Try
            Return ret
        End Function

        ''' <summary>
        ''' 更新激光位置
        ''' </summary>
        ''' <param name="Project_Name">项目名称</param>
        ''' <param name="Parameter_Name">项目参数</param>
        ''' <param name="Parameter_Value">参数变量</param>
        ''' <remarks></remarks>
        Function Update_Laser_Pos(ByVal Project_Name As String, ByVal ID As Int64, ByVal Parameter_Name() As String, ByVal Parameter_Value() As Object, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Boolean
            Dim ret As Boolean = False
            Try
                SubName = ErrName & GetCurrentMethod.Name & ":"
                Select Case DataBase_Link_Boolean
                    Case True
                        Dim Parameter_Count As Integer = Parameter_Name.Length
                        Dim Total_Parameter_Name_Str As String = Nothing
                        For i As Integer = 0 To Parameter_Count - 1
                            Select Case i
                                Case Parameter_Count - 1
                                    Total_Parameter_Name_Str = Total_Parameter_Name_Str & Parameter_Name(i) & " = " & Parameter_Value(i)
                                Case Else
                                    Total_Parameter_Name_Str = Total_Parameter_Name_Str & Parameter_Name(i) & " = " & Parameter_Value(i) & ", "
                            End Select
                        Next
                        DataBase_Command = New SqlCommand("UPDATE PROJECT_LASER_POS SET " & Total_Parameter_Name_Str & " WHERE (ID = " & ID & ") AND (项目名称 = N'" & Project_Name & "')", DataBase_Connection)
                        DataBase_Command.ExecuteNonQuery()
                        DataBase_Command.Dispose()
                        ret = True
                End Select
            Catch ex As Exception
               

                Write_Log(SubName & ex.Message)
                ret = False
            End Try
            Return ret
        End Function

        Function Update_Project_Parameter(ByVal Old_Product_Name As String, ByVal New_Product_Name As String, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Boolean
            Dim ret As Boolean = False
            Try
                SubName = ErrName & GetCurrentMethod.Name & ":"
                Select Case DataBase_Link_Boolean
                    Case True
                        DataBase_Command = New SqlCommand("UPDATE PROJECT_COORDINATE_NAME SET 项目名称 = N'" & New_Product_Name & "' WHERE (项目名称 = N'" & Old_Product_Name & "')", DataBase_Connection)
                        DataBase_Command.ExecuteNonQuery()
                        DataBase_Command.Dispose()
                        DataBase_Command = New SqlCommand("UPDATE PROJECT_COORDINATES_ORDER SET 项目名称 = N'" & New_Product_Name & "' WHERE (项目名称 = N'" & Old_Product_Name & "')", DataBase_Connection)
                        DataBase_Command.ExecuteNonQuery()
                        DataBase_Command.Dispose()
                        DataBase_Command = New SqlCommand("UPDATE PROJECT_COORDINATES_PARAMETERS SET 项目名称 = N'" & New_Product_Name & "' WHERE (项目名称 = N'" & Old_Product_Name & "')", DataBase_Connection)
                        DataBase_Command.ExecuteNonQuery()
                        DataBase_Command.Dispose()
                        DataBase_Command = New SqlCommand("UPDATE PROJECT_PARAMETERS SET 项目名称 = N'" & New_Product_Name & "' WHERE (项目名称 = N'" & Old_Product_Name & "')", DataBase_Connection)
                        DataBase_Command.ExecuteNonQuery()
                        DataBase_Command.Dispose()
                        ret = True
                End Select
            Catch ex As Exception
               

                Write_Log(SubName & ex.Message)
                ret = False
            End Try
            Return ret
        End Function

        ''' <summary>
        ''' 更新坐标系参数
        ''' </summary>
        ''' <param name="Old_Product_Name">原产品名称</param>
        ''' <param name="New_Product_Name">新产品名称</param>
        ''' <param name="SubName"></param>
        ''' <param name="ErrLine"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function Update_Coordinate_Parameter(ByVal Old_Product_Name As String, ByVal New_Product_Name As String, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Boolean
            Dim ret As Boolean = False
            Try
                SubName = ErrName & GetCurrentMethod.Name & ":"
                Select Case DataBase_Link_Boolean
                    Case True
                        DataBase_Command = New SqlCommand("UPDATE PROJECT_COORDINATE_NAME SET 坐标名称 = N'" & New_Product_Name & "' WHERE (坐标名称 = N'" & Old_Product_Name & "')", DataBase_Connection)
                        DataBase_Command.ExecuteNonQuery()
                        DataBase_Command.Dispose()
                        DataBase_Command = New SqlCommand("UPDATE PROJECT_COORDINATES_ORDER SET 坐标名称 = N'" & New_Product_Name & "' WHERE (坐标名称 = N'" & Old_Product_Name & "')", DataBase_Connection)
                        DataBase_Command.ExecuteNonQuery()
                        DataBase_Command.Dispose()
                        DataBase_Command = New SqlCommand("UPDATE PROJECT_COORDINATES_PARAMETERS SET 坐标名称 = N'" & New_Product_Name & "' WHERE (坐标名称 = N'" & Old_Product_Name & "')", DataBase_Connection)
                        DataBase_Command.ExecuteNonQuery()
                        DataBase_Command.Dispose()
                        ret = True
                End Select
            Catch ex As Exception
               

                Write_Log(SubName & ex.Message)
                ret = False
            End Try
            Return ret
        End Function

        ''' <summary>
        ''' 统一更新运行速度
        ''' </summary>
        ''' <param name="Project_Name">项目名称</param>
        ''' <param name="Runing_Speed">运行数据</param>
        ''' <remarks></remarks>
        Function Update_Running_Speed(ByVal Project_Name As String, ByVal Runing_Speed As Integer, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Boolean
            Dim ret As Boolean = False
            Try
                SubName = ErrName & GetCurrentMethod.Name & ":"
                Select Case DataBase_Link_Boolean
                    Case True
                        Select Case Exists_Project_Name(Project_Name)
                            Case True
                                DataBase_Command = New SqlCommand("UPDATE Project_Dist SET 运行速度 = " & Runing_Speed & " WHERE (项目名称 = '" & Project_Name & "')", DataBase_Connection)
                                DataBase_Command.ExecuteNonQuery()
                                DataBase_Command.Dispose()
                                ret = True
                        End Select
                End Select
            Catch ex As Exception
               

                Write_Log(SubName & ex.Message)
                ret = False
            End Try
            Return ret
        End Function

        ''' <summary>
        ''' 统一更新运行速度
        ''' </summary>
        ''' <param name="Project_Name">项目名称</param>
        ''' <param name="Runing_Speed_Panel">运行数据</param>
        ''' <remarks></remarks>
        Function Update_Running_Speed(ByVal Project_Name As String, ByVal Runing_Speed_Panel As DevComponents.DotNetBar.PanelEx, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Boolean
            Dim ret As Boolean = False
            Try
                SubName = ErrName & GetCurrentMethod.Name & ":"
                Select Case DataBase_Link_Boolean
                    Case True
                        Select Case Exists_Project_Name(Project_Name)
                            Case True
                                If IsNumeric(Runing_Speed_Panel.Text) = True Then
                                    DataBase_Command = New SqlCommand("UPDATE Project_Dist SET 运行速度 = " & CType(Runing_Speed_Panel.Text, Integer) & " WHERE (项目名称 = '" & Project_Name & "')", DataBase_Connection)
                                    DataBase_Command.ExecuteNonQuery()
                                    DataBase_Command.Dispose()
                                    ret = True
                                End If
                        End Select
                End Select
            Catch ex As Exception
               

                Write_Log(SubName & ex.Message)
                ret = False
            End Try
            Return ret
        End Function

        ''' <summary>
        ''' 统一更新运行速度
        ''' </summary>
        ''' <param name="Project_Name">项目名称</param>
        ''' <param name="Runing_Speed_TextBox">运行数据</param>
        ''' <remarks></remarks>
        Function Update_Running_Speed(ByVal Project_Name As String, ByVal Runing_Speed_TextBox As DevComponents.DotNetBar.Controls.TextBoxX, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Boolean
            Dim ret As Boolean = False
            Try
                SubName = ErrName & GetCurrentMethod.Name & ":"
                Select Case DataBase_Link_Boolean
                    Case True
                        Select Case Exists_Project_Name(Project_Name)
                            Case True
                                If IsNumeric(Runing_Speed_TextBox.Text) = True Then
                                    DataBase_Command = New SqlCommand("UPDATE Project_Dist SET 运行速度 = " & CType(Runing_Speed_TextBox.Text, Integer) & " WHERE (项目名称 = '" & Project_Name & "')", DataBase_Connection)
                                    DataBase_Command.ExecuteNonQuery()
                                    DataBase_Command.Dispose()
                                    ret = True
                                End If
                        End Select
                End Select
            Catch ex As Exception
               

                Write_Log(SubName & ex.Message)
                ret = False
            End Try
            Return ret
        End Function

        ''' <summary>
        ''' 统一更新运行速度
        ''' </summary>
        ''' <param name="Project_Name">项目名称</param>
        ''' <param name="Runing_Speed_IntegerInput">运行数据</param>
        ''' <remarks></remarks>
        Function Update_Running_Speed(ByVal Project_Name As String, ByVal Runing_Speed_IntegerInput As DevComponents.Editors.IntegerInput, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Boolean
            Dim ret As Boolean = False
            Try
                SubName = ErrName & GetCurrentMethod.Name & ":"
                Select Case DataBase_Link_Boolean
                    Case True
                        Select Case Exists_Project_Name(Project_Name)
                            Case True
                                If IsNumeric(Runing_Speed_IntegerInput.Value) = True Then
                                    DataBase_Command = New SqlCommand("UPDATE Project_Dist SET 运行速度 = " & CType(Runing_Speed_IntegerInput.Value, Integer) & " WHERE (项目名称 = '" & Project_Name & "')", DataBase_Connection)
                                    DataBase_Command.ExecuteNonQuery()
                                    DataBase_Command.Dispose()
                                    ret = True
                                End If
                        End Select
                End Select
            Catch ex As Exception
               

                Write_Log(SubName & ex.Message)
                ret = False
            End Try
            Return ret
        End Function

        ''' <summary>
        ''' 统一更新运行速度
        ''' </summary>
        ''' <param name="Project_Name">项目名称</param>
        ''' <param name="Runing_Speed_TextBox">运行数据</param>
        ''' <remarks></remarks>
        Function Update_Running_Speed(ByVal Project_Name As String, ByVal Runing_Speed_TextBox As System.Windows.Forms.TextBox, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Boolean
            Dim ret As Boolean = False
            Try
                SubName = ErrName & GetCurrentMethod.Name & ":"
                Select Case DataBase_Link_Boolean
                    Case True
                        Select Case Exists_Project_Name(Project_Name)
                            Case True
                                If IsNumeric(Runing_Speed_TextBox.Text) = True Then
                                    DataBase_Command = New SqlCommand("UPDATE Project_Dist SET 运行速度 = " & CType(Runing_Speed_TextBox.Text, Integer) & " WHERE (项目名称 = '" & Project_Name & "')", DataBase_Connection)
                                    DataBase_Command.ExecuteNonQuery()
                                    DataBase_Command.Dispose()
                                    ret = True
                                End If
                        End Select
                End Select
            Catch ex As Exception
               

                Write_Log(SubName & ex.Message)
                ret = False
            End Try
            Return ret
        End Function

        ''' <summary>
        ''' 统一更新加减速度
        ''' </summary>
        ''' <param name="Project_Name">项目名称</param>
        ''' <param name="Acc">加减速时间</param>
        ''' <remarks></remarks>
        Function Update_Running_Acc_Dec(ByVal Project_Name As String, ByVal Acc As Double, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Boolean
            Dim ret As Boolean = False
            Try
                SubName = ErrName & GetCurrentMethod.Name & ":"
                Select Case DataBase_Link_Boolean
                    Case True
                        Select Case Exists_Project_Name(Project_Name)
                            Case True
                                DataBase_Command = New SqlCommand("UPDATE Project_Dist SET 加减速时间 = " & Acc & " WHERE (项目名称 = '" & Project_Name & "')", DataBase_Connection)
                                DataBase_Command.ExecuteNonQuery()
                                DataBase_Command.Dispose()
                                ret = True
                        End Select
                End Select
            Catch ex As Exception
               

                Write_Log(SubName & ex.Message)
                ret = False
            End Try
            Return ret

        End Function

        ''' <summary>
        ''' 统一更新加减速度
        ''' </summary>
        ''' <param name="Project_Name">项目名称</param>
        ''' <param name="Acc">加减速时间</param>
        ''' <remarks></remarks>
        Function Update_Running_Acc_Dec(ByVal Project_Name As String, ByVal Acc As DevComponents.Editors.DoubleInput, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Boolean
            Dim ret As Boolean = False
            Try
                SubName = ErrName & GetCurrentMethod.Name & ":"
                Select Case DataBase_Link_Boolean
                    Case True
                        Select Case Exists_Project_Name(Project_Name)
                            Case True
                                DataBase_Command = New SqlCommand("UPDATE Project_Dist SET 加减速时间 = " & Acc.Value & " WHERE (项目名称 = '" & Project_Name & "')", DataBase_Connection)
                                DataBase_Command.ExecuteNonQuery()
                                DataBase_Command.Dispose()
                                ret = True
                        End Select
                End Select
            Catch ex As Exception
               

                Write_Log(SubName & ex.Message)
                ret = False
            End Try
            Return ret
        End Function

        ''' <summary>
        ''' 统一更新加减速度
        ''' </summary>
        ''' <param name="Project_Name">项目名称</param>
        ''' <param name="Acc_TextBox">加减速时间</param>
        ''' <remarks></remarks>
        Function Update_Running_Acc_Dec(ByVal Project_Name As String, ByVal Acc_TextBox As DevComponents.DotNetBar.Controls.TextBoxX, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Boolean
            Dim ret As Boolean = False
            Try
                SubName = ErrName & GetCurrentMethod.Name & ":"
                Select Case DataBase_Link_Boolean
                    Case True
                        Select Case Exists_Project_Name(Project_Name)
                            Case True
                                If IsNumeric(Acc_TextBox.Text) = True Then
                                    DataBase_Command = New SqlCommand("UPDATE Project_Dist SET 加减速时间 = " & CType(Acc_TextBox.Text, Double) & " WHERE (项目名称 = '" & Project_Name & "')", DataBase_Connection)
                                    DataBase_Command.ExecuteNonQuery()
                                    DataBase_Command.Dispose()
                                    ret = True
                                End If
                        End Select
                End Select
            Catch ex As Exception
               

                Write_Log(SubName & ex.Message)
                ret = False
            End Try
            Return ret
        End Function

        ''' <summary>
        ''' 统一更新加减速度
        ''' </summary>
        ''' <param name="Project_Name">项目名称</param>
        ''' <param name="Acc_TextBox">加减速时间</param>
        ''' <remarks></remarks>
        Function Update_Running_Acc_Dec(ByVal Project_Name As String, ByVal Acc_TextBox As Windows.Forms.TextBox, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Boolean
            Dim ret As Boolean = False
            Try
                SubName = ErrName & GetCurrentMethod.Name & ":"
                Select Case DataBase_Link_Boolean
                    Case True
                        Select Case Exists_Project_Name(Project_Name)
                            Case True
                                If IsNumeric(Acc_TextBox.Text) = True Then
                                    DataBase_Command = New SqlCommand("UPDATE Project_Dist SET 加减速时间 = " & CType(Acc_TextBox.Text, Double) & " WHERE (项目名称 = '" & Project_Name & "')", DataBase_Connection)
                                    DataBase_Command.ExecuteNonQuery()
                                    DataBase_Command.Dispose()
                                    ret = True
                                End If
                        End Select
                End Select
            Catch ex As Exception
               

                Write_Log(SubName & ex.Message)
                ret = False
            End Try
            Return ret
        End Function

        ''' <summary>
        ''' 更新坐标
        ''' </summary>
        ''' <param name="Project_Name"></param>
        ''' <param name="F_ID "></param>
        ''' <param name="Parameter_Name"></param>
        ''' <param name="Parameter_Value"></param>
        ''' <remarks></remarks>
        Function Update_Coordinates(ByVal Project_Name As String, ByVal F_ID As Integer, ByVal Parameter_Name() As String, ByVal Parameter_Value() As Object, Optional ByVal FIT As Boolean = False, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Boolean
            Dim ret As Boolean = False
            Try
                SubName = ErrName & GetCurrentMethod.Name & ":"
                Select Case DataBase_Link_Boolean
                    Case True
                        Dim Parameter_Count As Integer = Parameter_Name.Length
                        Dim Total_Parameter_Name_Str As String = Nothing
                        For i As Integer = 0 To Parameter_Count - 1
                            Select Case i
                                Case Parameter_Count - 1
                                    Total_Parameter_Name_Str = Total_Parameter_Name_Str & Parameter_Name(i) & " = " & Parameter_Value(i)
                                Case Else
                                    Total_Parameter_Name_Str = Total_Parameter_Name_Str & Parameter_Name(i) & " = " & Parameter_Value(i) & ", "
                            End Select
                        Next
                        DataBase_Command = New SqlCommand("UPDATE PROJECT_COORDINATES_PARAMETERS SET " & Total_Parameter_Name_Str & " WHERE (项目名称 = N'" & Project_Name & "') AND (ID = " & F_ID & ")", DataBase_Connection)
                        MessageBoxEx.Show("更新【" & DataBase_Command.ExecuteNonQuery() & "】条记录。", "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1)
                        ret = True
                End Select
            Catch ex As Exception
               

                Write_Log(SubName & ex.Message)
                ret = False
            End Try
            Return ret
        End Function

        ''' <summary>
        ''' 更新坐标
        ''' </summary>
        ''' <param name="Project_Name"></param>
        ''' <param name="PROJECT_COORDINATE_NAME"></param>
        ''' <param name="Parameter_Name"></param>
        ''' <param name="Parameter_Value"></param>
        ''' <remarks></remarks>
        Function Update_Coordinates(ByVal Project_Name As String, ByVal PROJECT_COORDINATE_NAME As String, ByVal Parameter_Name() As String, ByVal Parameter_Value() As Object, Optional ByVal F_ID As Integer = 0, Optional ByVal FIT As Boolean = False, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Boolean
            Dim ret As Boolean = False
            Try
                SubName = ErrName & GetCurrentMethod.Name & ":"
                Select Case DataBase_Link_Boolean
                    Case True
                        Dim Parameter_Count As Integer = Parameter_Name.Length
                        Dim Total_Parameter_Name_Str As String = Nothing
                        For i As Integer = 0 To Parameter_Count - 1

                            If Parameter_Value(i).GetType.ToString.ToUpper = "SYSTEM.STRING" Then
                                Select Case i
                                    Case Parameter_Count - 1
                                        Total_Parameter_Name_Str = Total_Parameter_Name_Str & Parameter_Name(i) & " = '" & Parameter_Value(i) & "'"
                                    Case Else
                                        Total_Parameter_Name_Str = Total_Parameter_Name_Str & Parameter_Name(i) & " = '" & Parameter_Value(i) & "', "
                                End Select
                            Else
                                Select Case i
                                    Case Parameter_Count - 1
                                        Total_Parameter_Name_Str = Total_Parameter_Name_Str & Parameter_Name(i) & " = " & Parameter_Value(i)
                                    Case Else
                                        Total_Parameter_Name_Str = Total_Parameter_Name_Str & Parameter_Name(i) & " = " & Parameter_Value(i) & ", "
                                End Select
                            End If


                        Next
                        DataBase_Command = New SqlCommand("SELECT COUNT(坐标名称) AS Expr1 FROM PROJECT_COORDINATES_PARAMETERS GROUP BY 项目名称, 坐标名称 HAVING      (项目名称 = N'" & Project_Name & "') AND (坐标名称 = N'" & PROJECT_COORDINATE_NAME & "')", DataBase_Connection)
                        Dim t1 As Integer = DataBase_Command.ExecuteScalar
                        If t1 > 0 Then
                            Select Case F_ID
                                Case Is = 0
                                    DataBase_Command = New SqlCommand("UPDATE PROJECT_COORDINATES_PARAMETERS SET " & Total_Parameter_Name_Str & " WHERE (项目名称 = N'" & Project_Name & "') AND (坐标名称 = N'" & PROJECT_COORDINATE_NAME & "')", DataBase_Connection)
                                    MessageBoxEx.Show("更新【" & DataBase_Command.ExecuteNonQuery() & "】条记录。", "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1)
                                    ret = True
                                Case Is >= 0
                                    DataBase_Command = New SqlCommand("UPDATE PROJECT_COORDINATES_PARAMETERS SET " & Total_Parameter_Name_Str & " WHERE (项目名称 = N'" & Project_Name & "') AND (坐标名称 = N'" & PROJECT_COORDINATE_NAME & "') AND (ID = N'" & F_ID & "')", DataBase_Connection)
                                    MessageBoxEx.Show("更新【" & DataBase_Command.ExecuteNonQuery() & "】条记录。", "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1)
                                    ret = True
                            End Select
                        End If
                End Select
            Catch ex As Exception
               

                Write_Log(SubName & ex.Message)
                ret = False
            End Try
            Return ret
        End Function

        ''' <summary>
        ''' 读取坐标datagridview
        ''' </summary>
        ''' <param name="Project_Name"></param>
        ''' <param name="Parameter_Name"></param>
        ''' <param name="PROJECT_COORDINATE_NAME"></param>
        ''' <param name="Coordinates_DataGridView"></param>
        ''' <remarks></remarks>
        Function Read_Coordinates(ByVal Project_Name As String, ByVal PROJECT_COORDINATE_NAME As String, ByVal Parameter_Name() As String, Optional ByRef Coordinates_DataGridView As DevComponents.DotNetBar.Controls.DataGridViewX = Nothing, Optional ByVal FIT As Boolean = True, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Boolean
            Dim ret As Boolean = False
            Try
                SubName = ErrName & GetCurrentMethod.Name & ":"
                Select Case DataBase_Link_Boolean
                    Case True

                        Dim Read_Coordinates_DataSet As New DataSet
                        Read_Coordinates_DataSet.Clear()

                        Dim Parameter_Count As Integer = Parameter_Name.Length
                        Dim Total_Str As String = Nothing
                        For i As Integer = 0 To Parameter_Count - 1
                            Select Case i
                                Case Parameter_Count - 1
                                    Total_Str = Total_Str & Parameter_Name(i)
                                Case Else
                                    Total_Str = Total_Str & Parameter_Name(i) & ", "
                            End Select
                        Next
                        DataBase_Command.CommandText = "SELECT " & Total_Str & " FROM PROJECT_COORDINATES_PARAMETERS WHERE (项目名称 = N'" & Project_Name & "') AND (坐标名称 = N'" & PROJECT_COORDINATE_NAME & "') ORDER BY ID"
                        DataBase_ConnectionAdapter = New SqlDataAdapter(DataBase_Command.CommandText, DataBase_Connection)
                        DataBase_ConnectionAdapter.Fill(Read_Coordinates_DataSet)


                        Coordinates_DataGridView.Columns.Clear()
                        Coordinates_DataGridView.DataSource = Read_Coordinates_DataSet.Tables(0)

                        For I = 0 To Coordinates_DataGridView.Columns.Count - 1
                            If I >= Coordinates_DataGridView.Columns.Count - 10 Then '加减速度后的不能编辑
                                Coordinates_DataGridView.Columns(I).ReadOnly = True
                            End If
                            Coordinates_DataGridView.Columns(I).SortMode = Windows.Forms.DataGridViewColumnSortMode.NotSortable
                            If FIT = True Then
                                Coordinates_DataGridView.Columns(I).AutoSizeMode = Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
                            Else
                                Coordinates_DataGridView.Columns(I).AutoSizeMode = Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
                            End If
                        Next
                        If Coordinates_DataGridView.Rows.Count > 1 Then
                            Coordinates_DataGridView.FirstDisplayedScrollingRowIndex = Coordinates_DataGridView.Rows.Count - 2
                            Coordinates_DataGridView.Rows(Coordinates_DataGridView.Rows.Count - 2).Selected = True
                        End If
                        Coordinates_DataGridView.RowHeadersVisible = False
                        Coordinates_DataGridView.AllowUserToDeleteRows = False
                        Coordinates_DataGridView.AllowUserToResizeColumns = False
                        Coordinates_DataGridView.AllowUserToResizeRows = False
                        Coordinates_DataGridView.ColumnHeadersHeightSizeMode = Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
                        Coordinates_DataGridView.MultiSelect = False
                        Coordinates_DataGridView.RowHeadersWidthSizeMode = Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
                        Coordinates_DataGridView.SelectionMode = Windows.Forms.DataGridViewSelectionMode.FullRowSelect
                        Coordinates_DataGridView.ReadOnly = False '只读属性

                        Coordinates_DataGridView.EditMode = DataGridViewEditMode.EditOnF2 '进入编辑状态
                        Coordinates_DataGridView.Cursor = Cursors.Arrow '鼠标编辑形状

                        Read_Coordinates_DataSet.Dispose()
                        DataBase_Command.Dispose()
                        DataBase_ConnectionAdapter.Dispose()
                        ret = True
                End Select
            Catch ex As Exception
               

                Write_Log(SubName & ex.Message)
                ret = False
            End Try
            Return ret
        End Function

        ''' <summary>
        ''' 读取坐标
        ''' </summary>
        ''' <param name="Project_Name"></param>
        ''' <param name="Parameter_Name"></param>
        ''' <param name="PROJECT_COORDINATE_NAME"></param>
        ''' <remarks></remarks>
        Function Read_Coordinates(ByVal Project_Name As String, ByVal PROJECT_COORDINATE_NAME As String, ByVal Parameter_Name() As String, ByRef Coordinates_Array() As Single, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Boolean
            Dim ret As Boolean = False
            Try
                SubName = ErrName & GetCurrentMethod.Name & ":"
                Select Case DataBase_Link_Boolean
                    Case True
                        Dim Read_Coordinates_DataSet As New DataSet
                        Read_Coordinates_DataSet.Clear()
                        Dim Parameter_Count As Integer = Parameter_Name.Length
                        Dim Total_Str As String = Nothing
                        For i As Integer = 0 To Parameter_Count - 1
                            Select Case i
                                Case Parameter_Count - 1
                                    Total_Str = Total_Str & Parameter_Name(i)
                                Case Else
                                    Total_Str = Total_Str & Parameter_Name(i) & ", "
                            End Select
                        Next
                        DataBase_Command.CommandText = "SELECT " & Total_Str & " FROM PROJECT_COORDINATES_PARAMETERS WHERE (项目名称 = N'" & Project_Name & "') AND (坐标名称 = N'" & PROJECT_COORDINATE_NAME & "') ORDER BY ID"
                        DataBase_ConnectionAdapter = New SqlDataAdapter(DataBase_Command.CommandText, DataBase_Connection)
                        DataBase_ConnectionAdapter.Fill(Read_Coordinates_DataSet)
                        Array.Resize(Coordinates_Array, Read_Coordinates_DataSet.Tables(0).Columns.Count)
                        If Read_Coordinates_DataSet.Tables(0).Rows.Count > 0 Then
                            For I = 0 To Read_Coordinates_DataSet.Tables(0).Columns.Count - 1
                                If IsDBNull(Read_Coordinates_DataSet.Tables(0).Rows(0).Item(I)) = False Then
                                    Coordinates_Array(I) = Read_Coordinates_DataSet.Tables(0).Rows(0).Item(I)
                                Else
                                    Coordinates_Array(I) = 0
                                End If
                            Next
                        End If
                        Read_Coordinates_DataSet.Dispose()
                        DataBase_Command.Dispose()
                End Select
            Catch ex As Exception
               

                Write_Log(SubName & ex.Message)
                ret = False
            End Try
            Return ret
        End Function

        ''' <summary>
        ''' 读取坐标dataset
        ''' </summary>
        ''' <param name="Project_Name"></param>
        ''' <param name="Parameter_Name"></param>
        ''' <param name="PROJECT_COORDINATE_NAME"></param>
        ''' <param name="Coordinates_DataSet"></param>
        ''' <remarks></remarks>
        Function Read_Coordinates(ByVal Project_Name As String, ByVal PROJECT_COORDINATE_NAME As String, ByVal Parameter_Name() As String, ByRef Coordinates_DataSet As System.Data.DataSet, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Boolean
            Dim ret As Boolean = False
            Try
                SubName = ErrName & GetCurrentMethod.Name & ":"
                Select Case DataBase_Link_Boolean
                    Case True
                        Coordinates_DataSet = New DataSet
                        Coordinates_DataSet.Clear()
                        Dim Parameter_Count As Integer = Parameter_Name.Length
                        Dim Total_Str As String = Nothing
                        For i As Integer = 0 To Parameter_Count - 1
                            Select Case i
                                Case Parameter_Count - 1
                                    Total_Str = Total_Str & Parameter_Name(i)
                                Case Else
                                    Total_Str = Total_Str & Parameter_Name(i) & ", "
                            End Select
                        Next
                        DataBase_Command.CommandText = "SELECT " & Total_Str & " FROM PROJECT_COORDINATES_PARAMETERS WHERE (项目名称 = N'" & Project_Name & "') AND (坐标名称 = N'" & PROJECT_COORDINATE_NAME & "') ORDER BY ID"
                        DataBase_ConnectionAdapter = New SqlDataAdapter(DataBase_Command.CommandText, DataBase_Connection)
                        DataBase_ConnectionAdapter.Fill(Coordinates_DataSet)
                        DataBase_Command.Dispose()
                        ret = True
                End Select
            Catch ex As Exception
               

                Write_Log(SubName & ex.Message)
                ret = False
            End Try
            Return ret
        End Function

        Function Insert_Message_Left(ByVal Project_Name As String, ByVal Product_SN As String, ByVal Station_Name As String, ByVal Message As String, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Boolean
            Dim ret As Boolean = False
            Try
                SubName = ErrName & GetCurrentMethod.Name & ":"
                Select Case DataBase_Link_Boolean
                    Case True
                        Dim DataBase_Comm As SqlCommand
                        DataBase_Comm = New SqlCommand("INSERT INTO PROJECT_MESSAGE (项目名称, 产品条码, 日期, 动作名称, 消息) VALUES (N'" & Project_Name & "',N'" & Product_SN & "', CONVERT(DATETIME, '" & Date.Now & "', 102), N'" & Station_Name & "', N'" & Message & "')", DataBase_Connection)
                        DataBase_Comm.ExecuteNonQuery()
                        DataBase_Comm.Dispose()
                        ret = True
                End Select
            Catch ex As Exception
               

                Write_Log(SubName & ex.Message)
                ret = False
            End Try
            Return ret
        End Function
        Function Insert_Message_Right(ByVal Project_Name As String, ByVal Product_SN As String, ByVal Station_Name As String, ByVal Message As String, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Boolean
            Dim ret As Boolean = False
            Try
                SubName = ErrName & GetCurrentMethod.Name & ":"
                Select Case DataBase_Link_Boolean
                    Case True
                        Dim DataBase_Comm As SqlCommand
                        DataBase_Comm = New SqlCommand("INSERT INTO PROJECT_MESSAGE (项目名称, 产品条码, 日期, 动作名称, 消息) VALUES (N'" & Project_Name & "',N'" & Product_SN & "', CONVERT(DATETIME, '" & Date.Now & "', 102), N'" & Station_Name & "', N'" & Message & "')", DataBase_Connection)
                        DataBase_Comm.ExecuteNonQuery()
                        DataBase_Comm.Dispose()
                        ret = True
                End Select
            Catch ex As Exception
               

                Write_Log(SubName & ex.Message)
                ret = False
            End Try
            Return ret
        End Function
        Function Insert_Message_Com(ByVal Project_Name As String, ByVal Product_SN As String, ByVal Station_Name As String, ByVal Message As String, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Boolean
            Dim ret As Boolean = False
            Try
                SubName = ErrName & GetCurrentMethod.Name & ":"
                Select Case DataBase_Link_Boolean
                    Case True
                        Dim DataBase_Comm As SqlCommand
                        DataBase_Comm = New SqlCommand("INSERT INTO PROJECT_MESSAGE (项目名称, 产品条码, 日期, 动作名称, 消息) VALUES (N'" & Project_Name & "',N'" & Product_SN & "', CONVERT(DATETIME, '" & Date.Now & "', 102), N'" & Station_Name & "', N'" & Message & "')", DataBase_Connection)
                        DataBase_Comm.ExecuteNonQuery()
                        DataBase_Comm.Dispose()
                        ret = True
                End Select
            Catch ex As Exception
               

                Write_Log(SubName & ex.Message)
                ret = False
            End Try
            Return ret
        End Function

        Function Read_Message(ByRef Message_data_View As DevComponents.DotNetBar.Controls.DataGridViewX, ByVal Start_Time As String, ByVal End_Time As String, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Boolean
            Dim ret As Boolean = False
            Try
                SubName = ErrName & GetCurrentMethod.Name & ":"
                Select Case DataBase_Link_Boolean
                    Case True
                        Message_data_View.Columns.Clear()
                        Dim Read_Message_DataSet As New DataSet
                        Read_Message_DataSet.Clear()
                        DataBase_Command.CommandText = "SELECT ID, 项目名称, 产品条码, 日期, 动作名称, 消息 FROM PROJECT_MESSAGE WHERE (日期 >= CONVERT(DATETIME, '" & Start_Time & "', 102)) AND (日期 <= CONVERT(DATETIME, '" & End_Time & "', 102)) ORDER BY ID"
                        DataBase_ConnectionAdapter = New SqlDataAdapter(DataBase_Command.CommandText, DataBase_Connection)
                        DataBase_ConnectionAdapter.Fill(Read_Message_DataSet)
                        Message_data_View.DataSource = Read_Message_DataSet.Tables(0)
                        For I = 0 To Message_data_View.Columns.Count - 1
                            Message_data_View.Columns(I).SortMode = Windows.Forms.DataGridViewColumnSortMode.NotSortable
                            Message_data_View.Columns(I).ReadOnly = True
                            Select Case I
                                Case Message_data_View.Columns.Count - 1
                                    Message_data_View.Columns(I).AutoSizeMode = Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
                                Case Else
                                    Message_data_View.Columns(I).AutoSizeMode = Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
                            End Select
                        Next
                        Message_data_View.RowHeadersVisible = False
                        Message_data_View.AllowUserToDeleteRows = False
                        Message_data_View.AllowUserToResizeColumns = False
                        Message_data_View.AllowUserToResizeRows = False
                        Message_data_View.ColumnHeadersHeightSizeMode = Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
                        Message_data_View.MultiSelect = False
                        Message_data_View.ReadOnly = True
                        Message_data_View.RowHeadersWidthSizeMode = Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
                        Message_data_View.SelectionMode = Windows.Forms.DataGridViewSelectionMode.FullRowSelect
                        DataBase_Command.Dispose()
                        ret = True
                End Select
            Catch ex As Exception
               

                Write_Log(SubName & ex.Message)
                ret = False
            End Try
            Return ret
        End Function

        Function Read_All_Message(ByRef Message_data_View As DevComponents.DotNetBar.Controls.DataGridViewX, ByVal Project_Name As String, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Boolean
            Dim ret As Boolean = False
            Try
                SubName = ErrName & GetCurrentMethod.Name & ":"
                Select Case DataBase_Link_Boolean
                    Case True
                        Message_data_View.Columns.Clear()
                        Dim Read_Message_DataSet As New DataSet
                        Read_Message_DataSet.Clear()
                        DataBase_Command.CommandText = "SELECT ID, 项目名称, 产品条码, 日期, 动作名称, 消息 FROM PROJECT_MESSAGE WHERE (项目名称 = '" & Project_Name & "') ORDER BY ID"
                        DataBase_ConnectionAdapter = New SqlDataAdapter(DataBase_Command.CommandText, DataBase_Connection)
                        DataBase_ConnectionAdapter.Fill(Read_Message_DataSet)
                        Message_data_View.DataSource = Read_Message_DataSet.Tables(0)
                        For I = 0 To Message_data_View.Columns.Count - 1
                            Message_data_View.Columns(I).SortMode = Windows.Forms.DataGridViewColumnSortMode.NotSortable
                            Message_data_View.Columns(I).ReadOnly = True
                            Select Case I
                                Case Message_data_View.Columns.Count - 1
                                    Message_data_View.Columns(I).AutoSizeMode = Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
                                Case Else
                                    Message_data_View.Columns(I).AutoSizeMode = Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
                            End Select
                        Next
                        Message_data_View.RowHeadersVisible = False
                        Message_data_View.AllowUserToDeleteRows = False
                        Message_data_View.AllowUserToResizeColumns = False
                        Message_data_View.AllowUserToResizeRows = False
                        Message_data_View.ColumnHeadersHeightSizeMode = Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
                        Message_data_View.MultiSelect = False
                        Message_data_View.ReadOnly = True
                        Message_data_View.RowHeadersWidthSizeMode = Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
                        Message_data_View.SelectionMode = Windows.Forms.DataGridViewSelectionMode.FullRowSelect
                        DataBase_Command.Dispose()
                        ret = True
                End Select
            Catch ex As Exception
               

                Write_Log(SubName & ex.Message)
                ret = False
            End Try
            Return ret
        End Function


        Function Read_Message(ByRef Message_data_View As DevComponents.DotNetBar.Controls.DataGridViewX, ByVal INF_ROW As DevComponents.DotNetBar.PanelEx, Optional ByVal DISP_ALL_INF As Boolean = False, Optional ByVal ROW_INT As Integer = 500, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Boolean
            Dim ret As Boolean = False
            Try
                SubName = ErrName & GetCurrentMethod.Name & ":"
                Select Case DataBase_Link_Boolean
                    Case True
                        Message_data_View.Columns.Clear()
                        Dim Read_Message_DataSet As New DataSet
                        Read_Message_DataSet.Clear()
                        Select Case DISP_ALL_INF
                            Case False
                                DataBase_Command.CommandText = "SELECT top " & ROW_INT & " ID, 项目名称, 产品条码, 日期, 动作名称, 消息 FROM PROJECT_MESSAGE ORDER BY ID DESC"
                            Case True
                                DataBase_Command.CommandText = "SELECT ID, 项目名称, 产品条码, 日期, 动作名称, 消息 FROM PROJECT_MESSAGE ORDER BY ID DESC"
                        End Select
                        DataBase_ConnectionAdapter = New SqlDataAdapter(DataBase_Command.CommandText, DataBase_Connection)
                        DataBase_ConnectionAdapter.Fill(Read_Message_DataSet)
                        Message_data_View.DataSource = Read_Message_DataSet.Tables(0)
                        For I = 0 To Message_data_View.Columns.Count - 1
                            Message_data_View.Columns(I).SortMode = Windows.Forms.DataGridViewColumnSortMode.NotSortable
                            Message_data_View.Columns(I).ReadOnly = True
                            Select Case I
                                Case Message_data_View.Columns.Count - 1
                                    Message_data_View.Columns(I).AutoSizeMode = Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
                                Case Else
                                    Message_data_View.Columns(I).AutoSizeMode = Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
                            End Select
                        Next
                        Message_data_View.RowHeadersVisible = False
                        Message_data_View.AllowUserToDeleteRows = False
                        Message_data_View.AllowUserToResizeColumns = False
                        Message_data_View.AllowUserToResizeRows = False
                        Message_data_View.ColumnHeadersHeightSizeMode = Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
                        Message_data_View.MultiSelect = False
                        Message_data_View.ReadOnly = True
                        Message_data_View.RowHeadersWidthSizeMode = Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
                        Message_data_View.SelectionMode = Windows.Forms.DataGridViewSelectionMode.FullRowSelect

                        If Message_data_View.Rows.Count >= 1 Then
                            INF_ROW.Text = Message_data_View.Rows.Count - 1
                        Else
                            INF_ROW.Text = 0
                        End If

                        DataBase_ConnectionAdapter.Dispose()
                        DataBase_Command.Dispose()
                        ret = True
                End Select
            Catch ex As Exception
               

                Write_Log(SubName & ex.Message)
                ret = False
            End Try
            Return ret
        End Function

        ''' <summary>
        ''' 新增项目
        ''' </summary>
        ''' <param name="Project_Name">项目名称</param>
        ''' <param name="Parameter_Name">参数名称数组</param>
        ''' <param name="Parameter_Value">参数变量</param>
        ''' <remarks></remarks>
        Function Create_Coordinates(ByVal Project_Name As String, ByVal PROJECT_COORDINATE_NAME As String, ByVal Parameter_Name() As String, ByVal Parameter_Value() As Object, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Boolean
            Dim ret As Boolean = False
            Try
                SubName = ErrName & GetCurrentMethod.Name & ":"
                Select Case DataBase_Link_Boolean
                    Case True
                        Dim Parameter_Count As Integer = Parameter_Name.Length
                        Dim Parameter_Total_Str As String = Nothing
                        Dim Parameter_Value_Total_Str As String = Nothing
                        For i As Integer = 0 To Parameter_Count - 1

                            If Parameter_Value(i).GetType.ToString.ToUpper = "SYSTEM.STRING" Then
                                Select Case i
                                    Case Parameter_Count - 1
                                        Parameter_Total_Str = Parameter_Total_Str & Parameter_Name(i)
                                        Parameter_Value_Total_Str = Parameter_Value_Total_Str & "'" & Parameter_Value(i) & "'"
                                    Case Else
                                        Parameter_Total_Str = Parameter_Total_Str & Parameter_Name(i) & ", "
                                        Parameter_Value_Total_Str = Parameter_Value_Total_Str & "'" & Parameter_Value(i) & "', "
                                End Select
                            Else
                                Select Case i
                                    Case Parameter_Count - 1
                                        Parameter_Total_Str = Parameter_Total_Str & Parameter_Name(i)
                                        Parameter_Value_Total_Str = Parameter_Value_Total_Str & Parameter_Value(i)
                                    Case Else
                                        Parameter_Total_Str = Parameter_Total_Str & Parameter_Name(i) & ", "
                                        Parameter_Value_Total_Str = Parameter_Value_Total_Str & Parameter_Value(i) & ", "
                                End Select
                            End If


                        Next
                        DataBase_Command = New SqlCommand("INSERT INTO PROJECT_COORDINATES_PARAMETERS (项目名称, 坐标名称 , " & Parameter_Total_Str & ") VALUES (N'" & Project_Name & "', N'" & PROJECT_COORDINATE_NAME & "', " & Parameter_Value_Total_Str & ")", DataBase_Connection)
                        DataBase_Command.ExecuteNonQuery()
                        DataBase_Command.Dispose()
                        ret = True
                End Select
            Catch ex As Exception
               

                Write_Log(SubName & ex.Message)
                ret = False
            End Try
            Return ret
        End Function


        Function New_Coordinates_ID(ByVal Parameter_Value() As Object, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Boolean
            Dim ret As Boolean = False
            Try
                SubName = ErrName & GetCurrentMethod.Name & ":"
                Select Case DataBase_Link_Boolean
                    Case True

                        DataBase_Command = New SqlCommand("INSERT INTO WORK_COORDINATE_PARAM(坐标ID, 项目名称, 坐标名称, 轴名称, 运行速度, 加减速度, 点胶状态, 镭射触发状态, CCD触发状态, 运动顺序)VALUES(N'" & Parameter_Value(0) & "', N'" & Parameter_Value(1) & "', N'" & Parameter_Value(2) & "', N'" & Parameter_Value(3) & "', " & Parameter_Value(4) & ", " & Parameter_Value(5) & ", " & Parameter_Value(6) & ", " & Parameter_Value(7) & ", " & Parameter_Value(8) & "," & Parameter_Value(9) & ")", DataBase_Connection)
                        DataBase_Command.ExecuteNonQuery()
                        DataBase_Command.Dispose()

                        ret = True
                End Select
            Catch ex As Exception
               

                Write_Log(SubName & ex.Message)
                ret = False
            End Try
            Return ret
        End Function

        Function Updata_Coordinates_ID(ByVal Project_Name As String, ByVal PROJECT_COORDINATE_NAME As String, ByVal Point_Len As Int16, ByVal Parameter_Name() As String, ByVal Parameter_Value() As Double, Optional ByVal F_ID As Integer = 0, Optional ByVal FIT As Boolean = False, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Boolean
            Dim ret As Boolean = False
            Try
                SubName = ErrName & GetCurrentMethod.Name & ":"
                Select Case DataBase_Link_Boolean
                    Case True
                        For x As Int16 = 0 To Point_Len - 1
                            Dim Parameter_Count As Integer = Parameter_Name.Length
                            Dim Total_Parameter_Name_Str As String = Nothing
                            For i As Integer = 0 To Parameter_Count - 1
                                Select Case i
                                    Case Parameter_Count - 1
                                        Total_Parameter_Name_Str = Total_Parameter_Name_Str & Parameter_Name(i) & " = " & Parameter_Value(i)
                                    Case Else
                                        Total_Parameter_Name_Str = Total_Parameter_Name_Str & Parameter_Name(i) & " = " & Parameter_Value(i) & ", "
                                End Select
                            Next
                            DataBase_Command = New SqlCommand("SELECT COUNT('坐标ID') as 计数 FROM WORK_COORDINATE_PARAM WHERE 坐标ID =" & F_ID, DataBase_Connection)
                            Dim t1 As Integer = DataBase_Command.ExecuteScalar
                            If t1 > 0 Then
                                Select Case F_ID
                                    Case Is = 0
                                        DataBase_Command = New SqlCommand("UPDATE WORK_COORDINATE_PARAM SET " & Total_Parameter_Name_Str & " WHERE (项目名称 = N'" & Project_Name & "') AND (坐标名称 = N'" & PROJECT_COORDINATE_NAME & "')", DataBase_Connection)
                                        MessageBoxEx.Show("更新【" & DataBase_Command.ExecuteNonQuery() & "】条记录。", "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1)
                                        ret = True
                                    Case Is >= 0
                                        DataBase_Command = New SqlCommand("UPDATE WORK_COORDINATE_PARAM SET " & Total_Parameter_Name_Str & " WHERE (项目名称 = N'" & Project_Name & "') AND (坐标名称 = N'" & PROJECT_COORDINATE_NAME & "') AND (ID = N'" & F_ID & "')", DataBase_Connection)
                                        MessageBoxEx.Show("更新【" & DataBase_Command.ExecuteNonQuery() & "】条记录。", "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1)
                                        ret = True
                                End Select
                            End If
                        Next
                End Select
            Catch ex As Exception
               

                Write_Log(SubName & ex.Message)
                ret = False
            End Try
            Return ret
        End Function

        Function If_CoordinatesID_Exits(ByVal Coordiante_ID As String, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Boolean
            Dim ret As Boolean = False
            Try
                SubName = ErrName & GetCurrentMethod.Name & ":"
                Select Case DataBase_Link_Boolean
                    Case True
                        DataBase_Command = New SqlCommand("SELECT COUNT('坐标ID') as 计数 FROM WORK_COORDINATE_PARAM WHERE 坐标ID =" & Coordiante_ID, DataBase_Connection)
                        Dim INDEX As Integer = CType(DataBase_Command.ExecuteScalar(), Integer)
                        DataBase_Command.Dispose()
                        If INDEX <> 0 Then
                            ret = True
                        Else
                            ret = False
                        End If

                End Select
            Catch ex As Exception
               

                Write_Log(SubName & ex.Message)
                ret = False
            End Try
            Return ret
        End Function

        ''' <summary>
        ''' 新增
        ''' </summary>
        ''' <param name="Project_Name"></param>
        ''' <param name="PROJECT_COORDINATE_NAME"></param>
        ''' <param name="Coordinate_Str"></param>
        ''' <param name="SubName"></param>
        ''' <param name="ErrLine"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function Cheng_New_Coordinate_AxisSystem(ByVal Project_Name As String, ByVal PROJECT_COORDINATE_NAME As String, ByVal Coordinate_Str As String, ByVal Coordinate_Index As String, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Boolean
            Dim ret As Boolean = False
            Try
                SubName = ErrName & GetCurrentMethod.Name & ":"
                Select Case DataBase_Link_Boolean
                    Case True
                        DataBase_Command = New SqlCommand("INSERT INTO PROJECT_COORDINATES_ORDER (项目名称, 坐标名称 , 轴系,运动顺序) VALUES (N'" & Project_Name & "', N'" & PROJECT_COORDINATE_NAME & "', N'" & Coordinate_Str & "', N'" & Coordinate_Index & "')", DataBase_Connection)
                        DataBase_Command.ExecuteNonQuery()
                        DataBase_Command.Dispose()
                        ret = True
                End Select
            Catch ex As Exception
               

                Write_Log(SubName & ex.Message)
                ret = False
            End Try
            Return ret
        End Function
        ''' <summary>
        ''' 更新
        ''' </summary>
        ''' <param name="Project_Name"></param>
        ''' <param name="PROJECT_COORDINATE_NAME"></param>
        ''' <param name="Coordinate_Str"></param>
        ''' <param name="SubName"></param>
        ''' <param name="ErrLine"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function Cheng_Updata_Coordinate_AxisSystem(ByVal Project_Name As String, ByVal PROJECT_COORDINATE_NAME As String, ByVal Coordinate_Str As String, ByVal Coordinate_Index As String, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Boolean
            Dim ret As Boolean = False
            Try
                SubName = ErrName & GetCurrentMethod.Name & ":"
                Select Case DataBase_Link_Boolean
                    Case True
                        DataBase_Command = New SqlCommand("UPDATE PROJECT_COORDINATES_ORDER SET 轴系='" & Coordinate_Str & "',运动顺序='" & Coordinate_Index & "' WHERE (项目名称 = N'" & Project_Name & "') AND (坐标名称 = N'" & PROJECT_COORDINATE_NAME & "')", DataBase_Connection)
                        DataBase_Command.ExecuteNonQuery()
                        DataBase_Command.Dispose()
                        ret = True
                End Select
            Catch ex As Exception
               

                Write_Log(SubName & ex.Message)
                ret = False
            End Try
            Return ret

        End Function
        ''' <summary>
        ''' 查询
        ''' </summary>
        ''' <param name="Project_Name"></param>
        ''' <param name="PROJECT_COORDINATE_NAME"></param>
        ''' <param name="SubName"></param>
        ''' <param name="ErrLine"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function Cheng_Query_Coordinate_AxisSystem(ByVal Project_Name As String, ByVal PROJECT_COORDINATE_NAME As String, Optional ByRef Axis_Name As String = Nothing, Optional ByRef Axis_Index As String = Nothing, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Boolean
            Dim ret As Boolean = False
            Try
                SubName = ErrName & GetCurrentMethod.Name & ":"
                Select Case DataBase_Link_Boolean
                    Case True
                        Dim User_Password_DataSet As New DataSet
                        User_Password_DataSet.Clear()
                        DataBase_Command.CommandText = "SELECT 轴系,运动顺序 FROM PROJECT_COORDINATES_ORDER WHERE (项目名称 = N'" & Project_Name & "') AND (坐标名称 = N'" & PROJECT_COORDINATE_NAME & "')"
                        DataBase_ConnectionAdapter = New SqlDataAdapter(DataBase_Command.CommandText, DataBase_Connection)
                        DataBase_ConnectionAdapter.Fill(User_Password_DataSet)

                        Axis_Name = User_Password_DataSet.Tables(0).Rows(0).Item(0).ToString.Trim
                        Axis_Index = User_Password_DataSet.Tables(0).Rows(0).Item(1).ToString.Trim
                        If Axis_Name.Length >= 1 Then
                            ret = True
                        Else
                            ret = False
                        End If
                End Select
            Catch ex As Exception
                ret = False
            End Try
            Return ret
        End Function

        ''' <summary>
        ''' 新增项目
        ''' </summary>
        ''' <param name="Parameter_Name">参数名称数组</param>
        ''' <param name="Parameter_Value">参数变量</param>
        ''' <remarks></remarks>
        Function Create_Project(ByVal Parameter_Name() As String, ByVal Parameter_Value() As Object, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Boolean
            Dim ret As Boolean = False
            Try
                SubName = ErrName & GetCurrentMethod.Name & ":"
                Select Case DataBase_Link_Boolean
                    Case True
                        Dim Parameter_Count As Integer = Parameter_Name.Length
                        Dim Parameter_Total_Str As String = Nothing
                        Dim Parameter_Value_Total_Str As String = Nothing
                        For i As Integer = 0 To Parameter_Count - 1
                            Select Case i
                                Case Parameter_Count - 1
                                    Parameter_Total_Str = Parameter_Total_Str & Parameter_Name(i)
                                    If IsNumeric(Parameter_Value(i)) = False Then
                                        Parameter_Value_Total_Str = Parameter_Value_Total_Str & "'" & Parameter_Value(i) & " '"
                                    Else
                                        Parameter_Value_Total_Str = Parameter_Value_Total_Str & Parameter_Value(i)
                                    End If
                                Case Else
                                    Parameter_Total_Str = Parameter_Total_Str & Parameter_Name(i) & ","
                                    If IsNumeric(Parameter_Value(i)) = False Then
                                        Parameter_Value_Total_Str = Parameter_Value_Total_Str & "'" & Parameter_Value(i) & "',"
                                    Else
                                        Parameter_Value_Total_Str = Parameter_Value_Total_Str & Parameter_Value(i) & ","
                                    End If
                            End Select
                        Next

                        DataBase_Command = New SqlCommand("INSERT INTO Project_Parameters (" & Parameter_Total_Str & ") VALUES ( " & Parameter_Value_Total_Str & ")", DataBase_Connection)
                        DataBase_Command.ExecuteNonQuery()
                        DataBase_Command.Dispose()
                        ret = True
                End Select
            Catch ex As Exception
               

                Write_Log(SubName & ex.Message)
                ret = False
            End Try
            Return ret
        End Function

        Function Create_Coordinate(ByVal Parameter_Name() As String, ByVal Parameter_Value() As Object, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Boolean
            Dim ret As Boolean = False
            Try
                SubName = ErrName & GetCurrentMethod.Name & ":"
                Select Case DataBase_Link_Boolean
                    Case True
                        Dim Parameter_Count As Integer = Parameter_Name.Length
                        Dim Parameter_Total_Str As String = Nothing
                        Dim Parameter_Value_Total_Str As String = Nothing
                        For i As Integer = 0 To Parameter_Count - 1
                            Select Case i
                                Case Parameter_Count - 1
                                    Parameter_Total_Str = Parameter_Total_Str & Parameter_Name(i)
                                    If IsNumeric(Parameter_Value(i)) = False Then
                                        Parameter_Value_Total_Str = Parameter_Value_Total_Str & "'" & Parameter_Value(i) & " '"
                                    Else
                                        Parameter_Value_Total_Str = Parameter_Value_Total_Str & Parameter_Value(i)
                                    End If
                                Case Else
                                    Parameter_Total_Str = Parameter_Total_Str & Parameter_Name(i) & ","
                                    If IsNumeric(Parameter_Value(i)) = False Then
                                        Parameter_Value_Total_Str = Parameter_Value_Total_Str & "'" & Parameter_Value(i) & "',"
                                    Else
                                        Parameter_Value_Total_Str = Parameter_Value_Total_Str & Parameter_Value(i) & ","
                                    End If
                            End Select
                        Next

                        DataBase_Command = New SqlCommand("INSERT INTO PROJECT_COORDINATE_NAME (" & Parameter_Total_Str & ") VALUES ( " & Parameter_Value_Total_Str & ")", DataBase_Connection)

                        DataBase_Command.ExecuteNonQuery()
                        DataBase_Command.Dispose()
                        ret = True
                End Select
            Catch ex As Exception
               

                Write_Log(SubName & ex.Message)
                ret = False
            End Try
            Return ret
        End Function


        ''' <summary>
        ''' 新增项目
        ''' </summary>
        ''' <param name="Project_Name">项目名称</param>
        ''' <param name="Parameter_Name">参数名称数组</param>
        ''' <param name="Parameter_Value">参数变量</param>
        ''' <remarks></remarks>
        Function Create_Project(ByVal Project_Name As String, ByVal Parameter_Name() As String, ByVal Parameter_Value() As Object, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Boolean
            Dim ret As Boolean = False
            Try
                SubName = ErrName & GetCurrentMethod.Name & ":"
                Select Case DataBase_Link_Boolean
                    Case True
                        Dim Parameter_Count As Integer = Parameter_Name.Length
                        Dim Parameter_Total_Str As String = Nothing
                        Dim Parameter_Value_Total_Str As String = Nothing
                        For i As Integer = 0 To Parameter_Count - 1
                            Select Case i
                                Case Parameter_Count - 1
                                    Parameter_Total_Str = Parameter_Total_Str & Parameter_Name(i)
                                    Parameter_Value_Total_Str = Parameter_Value_Total_Str & Parameter_Value(i)
                                Case Else
                                    Parameter_Total_Str = Parameter_Total_Str & Parameter_Name(i) & ", "
                                    Parameter_Value_Total_Str = Parameter_Value_Total_Str & Parameter_Value(i) & ", "
                            End Select
                        Next
                        DataBase_Command = New SqlCommand("INSERT INTO Project_Parameters (项目名称, " & Parameter_Total_Str & ") VALUES (N'" & Project_Name & "', " & Parameter_Value_Total_Str & ")", DataBase_Connection)
                        DataBase_Command.ExecuteNonQuery()
                        DataBase_Command.Dispose()
                        ret = True
                End Select
            Catch ex As Exception
               

                Write_Log(SubName & ex.Message)
                ret = False
            End Try
            Return ret
        End Function


        Function New_Home_Param(ByVal Parameter_Name() As String, ByVal Parameter_Value() As Object, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Boolean
            Dim ret As Boolean = False
            Try
                SubName = ErrName & GetCurrentMethod.Name & ":"
                Select Case DataBase_Link_Boolean
                    Case True
                        Dim Parameter_Count As Integer = Parameter_Name.Length
                        Dim Parameter_Total_Str As String = Nothing
                        Dim Parameter_Value_Total_Str As String = Nothing
                        For i As Integer = 0 To Parameter_Count - 1
                            Select Case i
                                Case Parameter_Count - 1
                                    Parameter_Total_Str = Parameter_Total_Str & Parameter_Name(i)
                                    If IsNumeric(Parameter_Value(i)) = False Then
                                        Parameter_Value_Total_Str = Parameter_Value_Total_Str & "'" & Parameter_Value(i) & " '"
                                    Else
                                        Parameter_Value_Total_Str = Parameter_Value_Total_Str & Parameter_Value(i)
                                    End If
                                Case Else
                                    Parameter_Total_Str = Parameter_Total_Str & Parameter_Name(i) & ","
                                    If IsNumeric(Parameter_Value(i)) = False Then
                                        Parameter_Value_Total_Str = Parameter_Value_Total_Str & "'" & Parameter_Value(i) & "',"
                                    Else
                                        Parameter_Value_Total_Str = Parameter_Value_Total_Str & Parameter_Value(i) & ","
                                    End If
                            End Select
                        Next
                        DataBase_Command = New SqlCommand("INSERT INTO HOME_PARAMETERS (" & Parameter_Total_Str & ") VALUES ( " & Parameter_Value_Total_Str & ")", DataBase_Connection)
                        DataBase_Command.ExecuteNonQuery()
                        DataBase_Command.Dispose()
                        ret = True
                End Select
            Catch ex As Exception
               

                Write_Log(SubName & ex.Message)
                ret = False
            End Try
            Return ret
        End Function

        ''' <summary>
        ''' 删除项目
        ''' </summary>
        ''' <param name="Project_Name">项目名称</param>
        ''' <remarks></remarks>
        Function Delete_Project(ByVal Project_Name As String, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Boolean
            Dim ret As Boolean = False
            Try
                SubName = ErrName & GetCurrentMethod.Name & ":"
                Select Case DataBase_Link_Boolean
                    Case True
                        DataBase_Command = New SqlCommand("DELETE FROM PROJECT_COORDINATE_NAME WHERE (项目名称 = N'" & Project_Name & "')", DataBase_Connection)
                        DataBase_Command.ExecuteNonQuery()
                        DataBase_Command.Dispose()

                        DataBase_Command = New SqlCommand("DELETE FROM PROJECT_COORDINATES_ORDER WHERE (项目名称 = N'" & Project_Name & "')", DataBase_Connection)
                        DataBase_Command.ExecuteNonQuery()
                        DataBase_Command.Dispose()

                        DataBase_Command = New SqlCommand("DELETE FROM PROJECT_COORDINATES_PARAMETERS WHERE (项目名称 = N'" & Project_Name & "')", DataBase_Connection)
                        DataBase_Command.ExecuteNonQuery()
                        DataBase_Command.Dispose()

                        DataBase_Command = New SqlCommand("DELETE FROM PROJECT_PARAMETERS WHERE (项目名称 = N'" & Project_Name & "')", DataBase_Connection)
                        DataBase_Command.ExecuteNonQuery()
                        DataBase_Command.Dispose()

                        DataBase_Command = New SqlCommand("DELETE FROM PROJECT_MESSAGE WHERE (项目名称 = N'" & Project_Name & "')", DataBase_Connection)
                        DataBase_Command.ExecuteNonQuery()
                        DataBase_Command.Dispose()

                        DataBase_Command = New SqlCommand("DELETE FROM PROJECT_MEASURE_DATA WHERE (Project_Name = N'" & Project_Name & "')", DataBase_Connection)
                        DataBase_Command.ExecuteNonQuery()
                        DataBase_Command.Dispose()

                        DataBase_Command = New SqlCommand("DELETE FROM PROJECT_MESAURE_IMAGE WHERE (项目名称 = N'" & Project_Name & "')", DataBase_Connection)
                        DataBase_Command.ExecuteNonQuery()
                        DataBase_Command.Dispose()

                        DataBase_Command = New SqlCommand("DELETE FROM PROJECT_MESAURE_RESULT WHERE (项目名称 = N'" & Project_Name & "')", DataBase_Connection)
                        DataBase_Command.ExecuteNonQuery()
                        DataBase_Command.Dispose()
                        ret = True
                End Select
            Catch ex As Exception
               

                Write_Log(SubName & ex.Message)
                ret = False
            End Try
            Return ret
        End Function

        Function Create_Coordinate_Table(ByVal Table_Name As String, ByVal Parameter_Name() As String, ByVal Parameter_Value() As Object, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Boolean
            Dim ret As Boolean = False
            Try
                SubName = ErrName & GetCurrentMethod.Name & ":"
                Select Case DataBase_Link_Boolean
                    Case True
                        Dim Parameter_Count As Integer = Parameter_Name.Length
                        Dim Parameter_Total_Str As String = Nothing
                        Dim Parameter_Value_Total_Str As String = Nothing
                        For i As Integer = 0 To Parameter_Count - 1
                            Select Case i
                                Case Parameter_Count - 1
                                    Parameter_Total_Str = Parameter_Total_Str & Parameter_Name(i)
                                    If IsNumeric(Parameter_Value(i)) = False Then
                                        Parameter_Value_Total_Str = Parameter_Value_Total_Str & "'" & Parameter_Value(i) & " '"
                                    Else
                                        Parameter_Value_Total_Str = Parameter_Value_Total_Str & Parameter_Value(i)
                                    End If
                                Case Else
                                    Parameter_Total_Str = Parameter_Total_Str & Parameter_Name(i) & ","
                                    If IsNumeric(Parameter_Value(i)) = False Then
                                        Parameter_Value_Total_Str = Parameter_Value_Total_Str & "'" & Parameter_Value(i) & "',"
                                    Else
                                        Parameter_Value_Total_Str = Parameter_Value_Total_Str & Parameter_Value(i) & ","
                                    End If
                            End Select
                        Next

                        DataBase_Command = New SqlCommand("INSERT INTO " & Table_Name & " (" & Parameter_Total_Str & ") VALUES ( " & Parameter_Value_Total_Str & ")", DataBase_Connection)

                        DataBase_Command.ExecuteNonQuery()
                        DataBase_Command.Dispose()
                        ret = True
                End Select
            Catch ex As Exception
               

                Write_Log(SubName & ex.Message)
                ret = False
            End Try
            Return ret
        End Function

        ''' <summary>
        ''' 查询表中所有的列名
        ''' </summary>
        ''' <param name="Tab_Name">表名</param>
        ''' <param name="Column_Names">查询到的列名</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function Query_All_ColNamesOfTable(ByVal Tab_Name As String, ByRef Column_Names() As String) As Boolean
            Dim Fun_Name As String '当前函数名称
            Fun_Name = GetCurrentMethod.Name
            Try
                Select Case DataBase_Link_Boolean
                    Case True
                        Dim _DadaSet As New DataSet
                        _DadaSet.Clear()
                        Column_Names = New String() {}
                        DataBase_Command = New SqlCommand("select name from syscolumns where id=object_id('" & Tab_Name & "')", DataBase_Connection)
                        DataBase_ConnectionAdapter = New SqlDataAdapter(DataBase_Command.CommandText, DataBase_Connection)
                        DataBase_ConnectionAdapter.Fill(_DadaSet)
                        Array.Resize(Column_Names, _DadaSet.Tables(0).Rows.Count)
                        For i As Int16 = 0 To _DadaSet.Tables(0).Rows.Count - 1
                            Column_Names(i) = _DadaSet.Tables(0).Rows(i).Item(0).ToString
                        Next
                        DataBase_Command.ExecuteScalar()
                        DataBase_Command.Dispose()
                        DataBase_ConnectionAdapter.Dispose()
                        Return True
                    Case False
                        Write_Log(Fun_Name & "数据库处于关闭状态！")
                        Return False
                End Select
                Return True
            Catch ex As Exception
                Write_Log(Fun_Name & ex.Message)
                Return False
            End Try
        End Function

        ''' <summary>
        ''' 复制项目参数
        ''' </summary>
        ''' <param name="Old_Project_Name">原项目名称</param>
        ''' <param name="New_Project_Name">新项目名称</param>
        ''' <param name="SubName"></param>
        ''' <param name="ErrLine"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function Copy_Project(ByVal Old_Project_Name As String, ByVal New_Project_Name As String, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Boolean
            Dim ret As Boolean = False
            Try
                SubName = ErrName & GetCurrentMethod.Name & ":"
                Select Case DataBase_Link_Boolean
                    Case True
                        Dim dataset As New DataSet
                        Dim PARAMETERS_NAME_ARRAY() As String = Nothing, PARAMETERS_VALUE_ARRAY() As Object = Nothing
                        DataBase_Command.CommandText = "SELECT * FROM PROJECT_COORDINATE_NAME WHERE (项目名称 = N'" & Old_Project_Name & "') ORDER BY ID"
                        DataBase_ConnectionAdapter = New SqlDataAdapter(DataBase_Command.CommandText, DataBase_Connection)
                        DataBase_ConnectionAdapter.Fill(dataset)
                        DataBase_Command.Dispose()
                        DataBase_ConnectionAdapter.Dispose()
                        If dataset.Tables(0).Rows.Count >= 1 Then
                            For i As Int16 = 0 To dataset.Tables(0).Rows.Count - 1
                                Array.Resize(PARAMETERS_NAME_ARRAY, 3)
                                Array.Resize(PARAMETERS_VALUE_ARRAY, 3)
                                Array.Clear(PARAMETERS_NAME_ARRAY, 0, PARAMETERS_NAME_ARRAY.Length)
                                Array.Clear(PARAMETERS_VALUE_ARRAY, 0, PARAMETERS_NAME_ARRAY.Length)
                                PARAMETERS_NAME_ARRAY(0) = "项目名称"
                                PARAMETERS_NAME_ARRAY(1) = "左右工位"
                                PARAMETERS_NAME_ARRAY(2) = "坐标名称"
                                PARAMETERS_VALUE_ARRAY(0) = New_Project_Name
                                PARAMETERS_VALUE_ARRAY(1) = dataset.Tables(0).Rows(i).Item("左右工位").ToString.Trim
                                PARAMETERS_VALUE_ARRAY(2) = dataset.Tables(0).Rows(i).Item("坐标名称").ToString.Trim
                                Create_Coordinate_Table("PROJECT_COORDINATE_NAME", PARAMETERS_NAME_ARRAY, PARAMETERS_VALUE_ARRAY)
                            Next
                        End If
                        dataset.Dispose() '拷贝坐标名称

                        dataset = New DataSet
                        DataBase_Command.CommandText = "SELECT * FROM PROJECT_COORDINATES_ORDER WHERE (项目名称 = N'" & Old_Project_Name & "') ORDER BY ID"
                        DataBase_ConnectionAdapter = New SqlDataAdapter(DataBase_Command.CommandText, DataBase_Connection)
                        DataBase_ConnectionAdapter.Fill(dataset)
                        DataBase_Command.Dispose()
                        DataBase_ConnectionAdapter.Dispose()
                        If dataset.Tables(0).Rows.Count >= 1 Then
                            For i As Int16 = 0 To dataset.Tables(0).Rows.Count - 1
                                Array.Resize(PARAMETERS_NAME_ARRAY, 4)
                                Array.Resize(PARAMETERS_VALUE_ARRAY, 4)
                                Array.Clear(PARAMETERS_NAME_ARRAY, 0, PARAMETERS_NAME_ARRAY.Length)
                                Array.Clear(PARAMETERS_VALUE_ARRAY, 0, PARAMETERS_NAME_ARRAY.Length)
                                PARAMETERS_NAME_ARRAY(0) = "项目名称"
                                PARAMETERS_NAME_ARRAY(1) = "坐标名称"
                                PARAMETERS_NAME_ARRAY(2) = "轴系"
                                PARAMETERS_NAME_ARRAY(3) = "运动顺序"
                                PARAMETERS_VALUE_ARRAY(0) = New_Project_Name
                                PARAMETERS_VALUE_ARRAY(1) = dataset.Tables(0).Rows(i).Item("坐标名称").ToString.Trim
                                PARAMETERS_VALUE_ARRAY(2) = dataset.Tables(0).Rows(i).Item("轴系").ToString.Trim
                                PARAMETERS_VALUE_ARRAY(3) = dataset.Tables(0).Rows(i).Item("运动顺序").ToString.Trim
                                Create_Coordinate_Table("PROJECT_COORDINATES_ORDER", PARAMETERS_NAME_ARRAY, PARAMETERS_VALUE_ARRAY)
                            Next
                        End If
                        dataset.Dispose() '拷贝运动顺序

                        dataset = New DataSet
                        DataBase_Command.CommandText = "SELECT * FROM PROJECT_COORDINATES_PARAMETERS WHERE (项目名称 = N'" & Old_Project_Name & "') ORDER BY ID"
                        DataBase_ConnectionAdapter = New SqlDataAdapter(DataBase_Command.CommandText, DataBase_Connection)
                        DataBase_ConnectionAdapter.Fill(dataset)
                        DataBase_Command.Dispose()
                        DataBase_ConnectionAdapter.Dispose()
                        If dataset.Tables(0).Rows.Count >= 1 Then
                            For i As Int16 = 0 To dataset.Tables(0).Rows.Count - 1
                                Dim Col_Names() As String = Nothing
                                Query_All_ColNamesOfTable("PROJECT_COORDINATES_PARAMETERS", Col_Names)
                                If Col_Names.Count >= 2 Then
                                    Array.Resize(PARAMETERS_NAME_ARRAY, Col_Names.Count - 1)
                                    Array.Resize(PARAMETERS_VALUE_ARRAY, Col_Names.Count - 1)
                                    Array.Clear(PARAMETERS_NAME_ARRAY, 0, PARAMETERS_NAME_ARRAY.Length)
                                    Array.Clear(PARAMETERS_VALUE_ARRAY, 0, PARAMETERS_NAME_ARRAY.Length)
                                    For j As Int16 = 0 To PARAMETERS_NAME_ARRAY.Count - 1
                                        If j = 0 Then
                                            PARAMETERS_NAME_ARRAY(j) = Col_Names(j + 1)
                                            PARAMETERS_VALUE_ARRAY(j) = New_Project_Name
                                        Else
                                            PARAMETERS_NAME_ARRAY(j) = Col_Names(j + 1)
                                            If dataset.Tables(0).Rows(i).Item(PARAMETERS_NAME_ARRAY(j)).ToString.Trim = "" Or dataset.Tables(0).Rows(i).Item(PARAMETERS_NAME_ARRAY(j)).ToString.Trim = "NULL" Then
                                                PARAMETERS_VALUE_ARRAY(j) = 0
                                            Else
                                                PARAMETERS_VALUE_ARRAY(j) = dataset.Tables(0).Rows(i).Item(PARAMETERS_NAME_ARRAY(j)).ToString.Trim
                                            End If
                                        End If
                                    Next
                                    Create_Coordinate_Table("PROJECT_COORDINATES_PARAMETERS", PARAMETERS_NAME_ARRAY, PARAMETERS_VALUE_ARRAY)
                                End If
                            Next
                        End If
                        dataset.Dispose() '拷贝坐标

                        ret = True
                End Select
            Catch ex As Exception
               

                Write_Log(SubName & ex.Message)
                ret = False
            End Try
            Return ret
        End Function


        ''' <summary>
        ''' 复制坐标名称
        ''' </summary>
        ''' <param name="Project_Name">项目名称</param>
        ''' <param name="Old_Coor_Station">复制坐标工位</param>
        ''' <param name="New_Coor_Station">粘贴坐标工位</param>
        ''' <param name="Copy_Coor_Name">坐标名称</param>
        ''' <param name="SubName"></param>
        ''' <param name="ErrLine"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function Copy_Coordinate_Name(ByVal Project_Name As String, ByVal Old_Coor_Station As String, ByVal New_Coor_Station As String, ByVal Copy_Coor_Name As String, ByVal Copy_Index As Integer, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Boolean
            Dim ret As Boolean = False
            Try
                SubName = ErrName & GetCurrentMethod.Name & ":"
                Select Case DataBase_Link_Boolean
                    Case True
                        Dim dataset As New DataSet
                        Dim PARAMETERS_NAME_ARRAY() As String = Nothing, PARAMETERS_VALUE_ARRAY() As Object = Nothing

                        DataBase_Command.CommandText = "SELECT * FROM PROJECT_COORDINATE_NAME WHERE (项目名称 = N'" & Project_Name & "' AND 坐标名称 = N'" & Copy_Coor_Name & "') ORDER BY ID"
                        DataBase_ConnectionAdapter = New SqlDataAdapter(DataBase_Command.CommandText, DataBase_Connection)
                        DataBase_ConnectionAdapter.Fill(dataset)
                        DataBase_Command.Dispose()
                        DataBase_ConnectionAdapter.Dispose()
                        If dataset.Tables(0).Rows.Count >= 1 Then
                            For i As Int16 = 0 To dataset.Tables(0).Rows.Count - 1
                                Array.Resize(PARAMETERS_NAME_ARRAY, 3)
                                Array.Resize(PARAMETERS_VALUE_ARRAY, 3)
                                Array.Clear(PARAMETERS_NAME_ARRAY, 0, PARAMETERS_NAME_ARRAY.Length)
                                Array.Clear(PARAMETERS_VALUE_ARRAY, 0, PARAMETERS_NAME_ARRAY.Length)
                                PARAMETERS_NAME_ARRAY(0) = "项目名称"
                                PARAMETERS_NAME_ARRAY(1) = "左右工位"
                                PARAMETERS_NAME_ARRAY(2) = "坐标名称"
                                PARAMETERS_VALUE_ARRAY(0) = dataset.Tables(0).Rows(i).Item("项目名称").ToString.Trim
                                PARAMETERS_VALUE_ARRAY(1) = New_Coor_Station
                                PARAMETERS_VALUE_ARRAY(2) = Copy_Coor_Name & "-副本" & Copy_Index.ToString
                                Create_Coordinate_Table("PROJECT_COORDINATE_NAME", PARAMETERS_NAME_ARRAY, PARAMETERS_VALUE_ARRAY)
                            Next
                        End If
                        dataset.Dispose() '拷贝坐标名称

                        dataset = New DataSet
                        DataBase_Command.CommandText = "SELECT * FROM PROJECT_COORDINATES_ORDER WHERE (项目名称 = N'" & Project_Name & "' AND 坐标名称 = N'" & Copy_Coor_Name & "') ORDER BY ID"
                        DataBase_ConnectionAdapter = New SqlDataAdapter(DataBase_Command.CommandText, DataBase_Connection)
                        DataBase_ConnectionAdapter.Fill(dataset)
                        DataBase_Command.Dispose()
                        DataBase_ConnectionAdapter.Dispose()
                        If dataset.Tables(0).Rows.Count >= 1 Then
                            For i As Int16 = 0 To dataset.Tables(0).Rows.Count - 1
                                Array.Resize(PARAMETERS_NAME_ARRAY, 4)
                                Array.Resize(PARAMETERS_VALUE_ARRAY, 4)
                                Array.Clear(PARAMETERS_NAME_ARRAY, 0, PARAMETERS_NAME_ARRAY.Length)
                                Array.Clear(PARAMETERS_VALUE_ARRAY, 0, PARAMETERS_NAME_ARRAY.Length)
                                PARAMETERS_NAME_ARRAY(0) = "项目名称"
                                PARAMETERS_NAME_ARRAY(1) = "坐标名称"
                                PARAMETERS_NAME_ARRAY(2) = "轴系"
                                PARAMETERS_NAME_ARRAY(3) = "运动顺序"
                                PARAMETERS_VALUE_ARRAY(0) = Project_Name
                                PARAMETERS_VALUE_ARRAY(1) = Copy_Coor_Name & "-副本" & Copy_Index.ToString
                                PARAMETERS_VALUE_ARRAY(2) = dataset.Tables(0).Rows(i).Item("轴系").ToString.Trim
                                PARAMETERS_VALUE_ARRAY(3) = dataset.Tables(0).Rows(i).Item("运动顺序").ToString.Trim
                                Create_Coordinate_Table("PROJECT_COORDINATES_ORDER", PARAMETERS_NAME_ARRAY, PARAMETERS_VALUE_ARRAY)
                            Next
                        End If
                        dataset.Dispose() '拷贝运动顺序

                        dataset = New DataSet
                        DataBase_Command.CommandText = "SELECT * FROM PROJECT_COORDINATES_PARAMETERS WHERE (项目名称 = N'" & Project_Name & "' AND 坐标名称 = N'" & Copy_Coor_Name & "') ORDER BY ID"
                        DataBase_ConnectionAdapter = New SqlDataAdapter(DataBase_Command.CommandText, DataBase_Connection)
                        DataBase_ConnectionAdapter.Fill(dataset)
                        DataBase_Command.Dispose()
                        DataBase_ConnectionAdapter.Dispose()
                        If dataset.Tables(0).Rows.Count >= 1 Then
                            For i As Int16 = 0 To dataset.Tables(0).Rows.Count - 1
                                Dim Col_Names() As String = Nothing
                                Query_All_ColNamesOfTable("PROJECT_COORDINATES_PARAMETERS", Col_Names)
                                If Col_Names.Count >= 2 Then
                                    Array.Resize(PARAMETERS_NAME_ARRAY, Col_Names.Count - 1)
                                    Array.Resize(PARAMETERS_VALUE_ARRAY, Col_Names.Count - 1)
                                    Array.Clear(PARAMETERS_NAME_ARRAY, 0, PARAMETERS_NAME_ARRAY.Length)
                                    Array.Clear(PARAMETERS_VALUE_ARRAY, 0, PARAMETERS_NAME_ARRAY.Length)
                                    For j As Int16 = 0 To PARAMETERS_NAME_ARRAY.Count - 1
                                        PARAMETERS_NAME_ARRAY(j) = Col_Names(j + 1)
                                        If dataset.Tables(0).Rows(i).Item(PARAMETERS_NAME_ARRAY(j)).ToString.Trim = "" Or dataset.Tables(0).Rows(i).Item(PARAMETERS_NAME_ARRAY(j)).ToString.Trim = "NULL" Then
                                            PARAMETERS_VALUE_ARRAY(j) = 0
                                        Else
                                            PARAMETERS_VALUE_ARRAY(j) = dataset.Tables(0).Rows(i).Item(PARAMETERS_NAME_ARRAY(j)).ToString.Trim
                                        End If
                                    Next
                                    PARAMETERS_VALUE_ARRAY(1) = Copy_Coor_Name & "-副本" & Copy_Index.ToString '更换坐标名称
                                    Create_Coordinate_Table("PROJECT_COORDINATES_PARAMETERS", PARAMETERS_NAME_ARRAY, PARAMETERS_VALUE_ARRAY)
                                End If
                            Next
                        End If
                        dataset.Dispose() '拷贝坐标

                        ret = True
                End Select
            Catch ex As Exception
               

                Write_Log(SubName & ex.Message)
                ret = False
            End Try
            Return ret
        End Function


        Function Delete_Coordiante(ByVal Project_Name As String, ByVal PROJECT_COORDINATE_NAME As String, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Boolean
            Dim ret As Boolean = False
            Try
                SubName = ErrName & GetCurrentMethod.Name & ":"
                Select Case DataBase_Link_Boolean
                    Case True
                        DataBase_Command = New SqlCommand("DELETE FROM PROJECT_COORDINATE_NAME WHERE (项目名称 = N'" & Project_Name & "' AND 坐标名称 = N'" & PROJECT_COORDINATE_NAME & "')", DataBase_Connection)
                        DataBase_Command.ExecuteNonQuery()
                        DataBase_Command.Dispose()

                        DataBase_Command = New SqlCommand("DELETE FROM PROJECT_COORDINATES_ORDER WHERE (项目名称 = N'" & Project_Name & "' AND 坐标名称 = N'" & PROJECT_COORDINATE_NAME & "')", DataBase_Connection)
                        DataBase_Command.ExecuteNonQuery()
                        DataBase_Command.Dispose()

                        DataBase_Command = New SqlCommand("DELETE FROM PROJECT_COORDINATES_PARAMETERS WHERE (项目名称 = N'" & Project_Name & "' AND 坐标名称 = N'" & PROJECT_COORDINATE_NAME & "')", DataBase_Connection)
                        DataBase_Command.ExecuteNonQuery()
                        DataBase_Command.Dispose()
                End Select
            Catch ex As Exception
               

                Write_Log(SubName & ex.Message)
                ret = False
            End Try
            Return ret
        End Function

        ''' <summary>
        ''' 删除项目
        ''' </summary>
        ''' <remarks></remarks>
        Function Delete_Param_ID(ByVal Table_Name As String, ByVal Project_Name_ID As Integer, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Boolean
            Dim ret As Boolean = False
            Try
                SubName = ErrName & GetCurrentMethod.Name & ":"
                Select Case DataBase_Link_Boolean
                    Case True
                        DataBase_Command = New SqlCommand("DELETE FROM " & Table_Name & " WHERE (ID = " & Project_Name_ID & ")", DataBase_Connection)
                        DataBase_Command.ExecuteNonQuery()
                        DataBase_Command.Dispose()
                        ret = True
                End Select
            Catch ex As Exception
               

                Write_Log(SubName & ex.Message)
                ret = False
            End Try
            Return ret
        End Function

        Function Delete_Param_ALL(ByVal Table_Name As String, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Boolean
            Dim ret As Boolean = False
            Try
                SubName = ErrName & GetCurrentMethod.Name & ":"
                Select Case DataBase_Link_Boolean
                    Case True
                        DataBase_Command = New SqlCommand("DELETE FROM " & Table_Name & " ", DataBase_Connection)
                        DataBase_Command.ExecuteNonQuery()
                        DataBase_Command.Dispose()
                        ret = True
                End Select
            Catch ex As Exception
               

                Write_Log(SubName & ex.Message)
                ret = False
            End Try
            Return ret
        End Function

        ''' <summary>
        ''' 删除项目
        ''' </summary>
        ''' <remarks></remarks>
        Function Updata_HOme_Param_ID(ByVal Parameter_Name() As String, ByVal Parameter_Value() As Object, ByVal Parameter_ID As Integer, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Boolean
            Dim ret As Boolean = False
            Try
                SubName = ErrName & GetCurrentMethod.Name & ":"
                Select Case DataBase_Link_Boolean
                    Case True
                        Dim Parameter_Count As Integer = Parameter_Name.Length
                        Dim Total_Parameter_Name_Str As String = Nothing
                        For i As Integer = 0 To Parameter_Count - 1
                            Select Case i
                                Case Parameter_Count - 1
                                    Total_Parameter_Name_Str = Total_Parameter_Name_Str & Parameter_Name(i) & " = " & "'" & Parameter_Value(i) & "'"
                                Case Else
                                    Total_Parameter_Name_Str = Total_Parameter_Name_Str & Parameter_Name(i) & " = " & "'" & Parameter_Value(i) & "'" & ", "
                            End Select
                        Next
                        DataBase_Command = New SqlCommand("UPDATE HOME_PARAMETERS SET " & Total_Parameter_Name_Str & " WHERE (ID = " & Parameter_ID & ")", DataBase_Connection)
                        DataBase_Command.ExecuteNonQuery()
                        DataBase_Command.Dispose()
                        ret = True
                End Select
            Catch ex As Exception
               

                Write_Log(SubName & ex.Message)
                ret = False
            End Try
            Return ret
        End Function

        ''' <summary>
        ''' 读取硬件参数
        ''' </summary>
        ''' <param name="Hardware_Parameter">硬件参数表</param>
        ''' <param name="Hardware_IO">硬件输入输出表</param>
        ''' <remarks></remarks>
        Function Read_Hardware_IO(ByVal Hardware_Parameter() As String, ByRef Hardware_IO() As Object, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Boolean
            Dim ret As Boolean = False
            Try
                SubName = ErrName & GetCurrentMethod.Name & ":"
                Select Case DataBase_Link_Boolean
                    Case True
                        Dim Serch_Hardware_Parameter_DataSet As New DataSet
                        Serch_Hardware_Parameter_DataSet.Clear()
                        Dim Parameter_Count As Integer = Hardware_Parameter.Length
                        Dim Total_Str As String = Nothing
                        For i As Integer = 0 To Parameter_Count - 1
                            Select Case i
                                Case Parameter_Count - 1
                                    Total_Str = Total_Str & Hardware_Parameter(i)
                                Case Else
                                    Total_Str = Total_Str & Hardware_Parameter(i) & ", "
                            End Select
                        Next
                        DataBase_Command.CommandText = "SELECT DISTINCT  " & Total_Str & "  FROM Hardware_Parameters WHERE (ID=1)"
                        DataBase_ConnectionAdapter = New SqlDataAdapter(DataBase_Command.CommandText, DataBase_Connection)
                        DataBase_ConnectionAdapter.Fill(Serch_Hardware_Parameter_DataSet)
                        For i As Integer = 0 To Serch_Hardware_Parameter_DataSet.Tables(0).Columns.Count - 1
                            Hardware_IO(i) = Serch_Hardware_Parameter_DataSet.Tables(0).Rows(0).Item(i)
                        Next
                        Serch_Hardware_Parameter_DataSet.Dispose()
                        DataBase_Command.Dispose()
                        ret = True
                End Select
            Catch ex As Exception
               

                Write_Log(SubName & ex.Message)
                ret = False
            End Try
            Return ret
        End Function

        ''' <summary>
        ''' 读取硬件参数
        ''' </summary>
        ''' <param name="Hardware_Parameter">硬件参数表</param>
        ''' <param name="Hardware_IO">硬件输入输出表</param>
        ''' <remarks></remarks>
        Function Read_Hardware_IO(ByVal Hardware_Parameter() As String, ByRef Hardware_IO() As DevComponents.DotNetBar.Controls.ComboBoxEx, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Boolean
            Dim ret As Boolean = False
            Try
                SubName = ErrName & GetCurrentMethod.Name & ":"
                Select Case DataBase_Link_Boolean
                    Case True
                        Dim Serch_Hardware_Parameter_DataSet As New DataSet
                        Serch_Hardware_Parameter_DataSet.Clear()
                        Dim Parameter_Count As Integer = Hardware_Parameter.Length
                        Dim Total_Str As String = Nothing
                        For i As Integer = 0 To Parameter_Count - 1
                            Select Case i
                                Case Parameter_Count - 1
                                    Total_Str = Total_Str & Hardware_Parameter(i)
                                Case Else
                                    Total_Str = Total_Str & Hardware_Parameter(i) & ", "
                            End Select
                        Next
                        DataBase_Command.CommandText = "SELECT DISTINCT  " & Total_Str & "  FROM Hardware_Parameters WHERE (ID=1)"
                        DataBase_ConnectionAdapter = New SqlDataAdapter(DataBase_Command.CommandText, DataBase_Connection)
                        DataBase_ConnectionAdapter.Fill(Serch_Hardware_Parameter_DataSet)
                        For i As Integer = 0 To Serch_Hardware_Parameter_DataSet.Tables(0).Columns.Count - 1
                            Hardware_IO(i).SelectedIndex = Serch_Hardware_Parameter_DataSet.Tables(0).Rows(0).Item(i)
                        Next
                        Serch_Hardware_Parameter_DataSet.Dispose()
                        DataBase_Command.Dispose()
                        ret = True
                End Select
            Catch ex As Exception
               

                Write_Log(SubName & ex.Message)
                ret = False
            End Try
            Return ret
        End Function

        ''' <summary>
        ''' 更新产品参数
        ''' </summary>
        ''' <param name="Parameter_Name">项目参数</param>
        ''' <param name="Parameter_Value">参数变量</param>
        ''' <remarks></remarks>
        Function Update_Hardware_IO_Parameter(ByVal Parameter_Name() As String, ByVal Parameter_Value() As DevComponents.DotNetBar.Controls.ComboBoxEx, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Boolean
            Dim ret As Boolean = False
            Try
                SubName = ErrName & GetCurrentMethod.Name & ":"
                Select Case DataBase_Link_Boolean
                    Case True
                        Dim Parameter_Count As Integer = Parameter_Name.Length
                        Dim Total_Parameter_Name_Str As String = Nothing
                        For i As Integer = 0 To Parameter_Count - 1
                            Select Case i
                                Case Parameter_Count - 1
                                    Total_Parameter_Name_Str = Total_Parameter_Name_Str & Parameter_Name(i) & " = " & Parameter_Value(i).SelectedIndex
                                Case Else
                                    Total_Parameter_Name_Str = Total_Parameter_Name_Str & Parameter_Name(i) & " = " & Parameter_Value(i).SelectedIndex & ", "
                            End Select
                        Next
                        DataBase_Command = New SqlCommand("UPDATE Hardware_Parameters SET " & Total_Parameter_Name_Str & " WHERE (ID = 1)", DataBase_Connection)
                        DataBase_Command.ExecuteNonQuery()
                        DataBase_Command.Dispose()
                        ret = True
                End Select
            Catch ex As Exception
               

                Write_Log(SubName & ex.Message)
                ret = False
            End Try
            Return ret
        End Function

        Function User_Exists(ByVal User_Name As String, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Boolean
            Dim ret As Boolean = False
            Dim User_Ex As Integer = Nothing
            Try
                SubName = ErrName & GetCurrentMethod.Name & ":"
                Select Case DataBase_Link_Boolean
                    Case True
                        DataBase_Command = New SqlCommand("SELECT COUNT(用户名称) AS 用户名称 FROM SYSTEM_PARAMETERS WHERE (用户名称 = N'" & User_Name & "')", DataBase_Connection)
                        User_Ex = DataBase_Command.ExecuteScalar()
                        DataBase_Command.Dispose()
                        If User_Ex > 0 Then
                            ret = True
                        Else
                            ret = False
                        End If
                End Select
            Catch ex As Exception
               

                Write_Log(SubName & ex.Message)
                ret = False
            End Try
            Return ret
        End Function

        Function Delete_User(ByVal User_Name As String, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Boolean
            Dim ret As Boolean = False
            Try
                SubName = ErrName & GetCurrentMethod.Name & ":"
                Select Case DataBase_Link_Boolean
                    Case True
                        DataBase_Command = New SqlCommand("DELETE FROM SYSTEM_PARAMETERS WHERE (用户名称 = N'" & User_Name & "')", DataBase_Connection)
                        DataBase_Command.ExecuteNonQuery()
                        MessageBoxEx.Show("用户名:" & User_Name & "删除完成！", "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1)
                        DataBase_Command.Dispose()
                        ret = True
                End Select
            Catch ex As Exception
               

                Write_Log(SubName & ex.Message)
                ret = False
            End Try
            Return ret
        End Function

        Function Code_Exists(Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Boolean
            Dim ret As Boolean = False
            Try
                SubName = ErrName & GetCurrentMethod.Name & ":"
                Select Case DataBase_Link_Boolean
                    Case True
                        DataBase_Command = New SqlCommand("SELECT COUNT(ID) AS ID FROM DEVICE_SOFT WHERE (Code IS NOT NULL)", DataBase_Connection)
                        Code_Exists = DataBase_Command.ExecuteScalar
                        DataBase_Command.Dispose()

                        ret = True
                End Select
            Catch ex As Exception
               

                Write_Log(SubName & ex.Message)
                ret = False
            End Try
            Return ret
        End Function

        Function Upload_Code(ByVal File_Name As String, ByVal Upload_File_Path As String, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Boolean
            Dim ret As Boolean = False
            Try
                SubName = ErrName & GetCurrentMethod.Name & ":"
                Select Case DataBase_Link_Boolean
                    Case True
                        DataBase_Command = New SqlCommand("DELETE FROM DEVICE_SOFT WHERE (Code IS NOT NULL)", DataBase_Connection)
                        DataBase_Command.ExecuteNonQuery()
                        DataBase_Command.Dispose()
                        Dim FS As New System.IO.FileStream(Upload_File_Path, IO.FileMode.Open, IO.FileAccess.Read)
                        Dim Mbr As New BinaryReader(FS)
                        Dim Buffer_FileStream(FS.Length) As Byte
                        Mbr.Read(Buffer_FileStream, 0, CType(FS.Length, Integer))
                        FS.Close()
                        FS.Dispose()
                        Mbr.Close()
                        DataBase_Command = New SqlCommand("INSERT INTO DEVICE_SOFT (文件名称列表, Code) VALUES (N'" & File_Name & "',@image)", DataBase_Connection)
                        DataBase_Command.Parameters.Add("@image", SqlDbType.Image).Value = Buffer_FileStream
                        DataBase_Command.ExecuteNonQuery()
                        DataBase_Command.Dispose()
                        MessageBoxEx.Show(Upload_File_Path & "文件上传完成！", "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        ret = True
                End Select
            Catch ex As Exception
               

                Write_Log(SubName & ex.Message)
                ret = False
            End Try
            Return ret
        End Function

        Function Read_Code_Name(ByRef Code_Name As String, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Boolean
            Dim ret As Boolean = False
            Try
                SubName = ErrName & GetCurrentMethod.Name & ":"
                Dim Name_Str As String = Nothing
                Select Case DataBase_Link_Boolean
                    Case True
                        DataBase_Command = New SqlCommand("SELECT DISTINCT 文件名称列表 FROM DEVICE_SOFT WHERE (Code IS NOT NULL)", DataBase_Connection)
                        Code_Name = DataBase_Command.ExecuteScalar.ToString.Trim
                        DataBase_Command.Dispose()
                End Select
                Return Name_Str
            Catch ex As Exception
               

                Write_Log(SubName & ex.Message)
                Code_Name = Nothing
                ret = False
            End Try
            Return ret
        End Function

        Function DownLoad_Code(ByVal Code_File_Name As String, ByVal Save_As_File As String, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Boolean
            Dim ret As Boolean = False
            Try
                SubName = ErrName & GetCurrentMethod.Name & ":"
                Select Case DataBase_Link_Boolean
                    Case True
                        Dim Def_Dir As String = Application.StartupPath() & "\"
                        Dim FileStream_DataSet As New DataSet
                        FileStream_DataSet.Clear()
                        DataBase_Command.CommandText = "SELECT 文件名称列表, Code FROM DEVICE_SOFT WHERE (文件名称列表 = '" & Code_File_Name & "') AND (Code IS NOT NULL)"
                        DataBase_ConnectionAdapter = New SqlDataAdapter(DataBase_Command.CommandText, DataBase_Connection)
                        DataBase_ConnectionAdapter.Fill(FileStream_DataSet)
                        If FileStream_DataSet.Tables(0).Rows.Count > 0 Then
                            Dim FS As FileStream = Nothing
                            Dim File_Name As String = FileStream_DataSet.Tables(0).Rows(0).Item("文件名称列表")
                            Dim ImagInfo() As Byte = FileStream_DataSet.Tables(0).Rows(0).Item("Code")
                            FS = New FileStream(Save_As_File, FileMode.Create, FileAccess.Write)
                            For j As Integer = 0 To UBound(ImagInfo)
                                FS.WriteByte(ImagInfo(j))
                            Next
                            FS.Flush()
                            FS.Close()
                            FS.Dispose()
                            MessageBoxEx.Show(Save_As_File & "保存完成！", "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1)
                            ret = True
                        Else
                            MessageBoxEx.Show("查询结果为空！", "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1)
                        End If
                        DataBase_Command.Dispose()
                        DataBase_ConnectionAdapter.Dispose()
                End Select
            Catch ex As Exception
               

                Write_Log(SubName & ex.Message)
                ret = False
            End Try
            Return ret
        End Function

        ''' <summary>
        ''' 读取测量数据
        ''' </summary>
        ''' <param name="Project_Name">项目名称</param>
        ''' <param name="Parameter_Name">项目参数表</param>
        ''' <param name="Measure_Data_DataGridView">显示测量数据表</param>
        ''' <param name="Fit_Bool"></param>
        ''' <remarks></remarks>
        Function Read_Measure_Data(ByVal Project_Name As String, ByVal Parameter_Name() As String, ByRef Measure_Data_DataGridView As DevComponents.DotNetBar.Controls.DataGridViewX, Optional ByVal Fit_Bool As Boolean = True, Optional ByVal SubName As String = Nothing, Optional ByVal ErrLine As String = Nothing) As Boolean
            Dim ret As Boolean = False
            Try
                SubName = ErrName & GetCurrentMethod.Name & ":"
                Select Case DataBase_Link_Boolean
                    Case True
                        Measure_Data_DataGridView.Columns.Clear()
                        Dim Serch_Measure_Data_DataSet As New DataSet
                        Serch_Measure_Data_DataSet.Clear()
                        Dim Parameter_Count As Integer = Parameter_Name.Length
                        Dim Total_Str As String = Nothing
                        For i As Integer = 0 To Parameter_Count - 1
                            Select Case i
                                Case Parameter_Count - 1
                                    Total_Str = Total_Str & Parameter_Name(i)
                                Case Else
                                    Total_Str = Total_Str & Parameter_Name(i) & ", "
                            End Select
                        Next
                        DataBase_Command.CommandText = "SELECT  " & Total_Str & "  FROM Measure_Data WHERE (项目名称 = '" & Project_Name & "')"
                        DataBase_ConnectionAdapter = New SqlDataAdapter(DataBase_Command.CommandText, DataBase_Connection)
                        DataBase_ConnectionAdapter.Fill(Serch_Measure_Data_DataSet)
                        Measure_Data_DataGridView.DataSource = Serch_Measure_Data_DataSet.Tables(0)
                        For I = 0 To Measure_Data_DataGridView.Columns.Count - 1
                            Measure_Data_DataGridView.Columns(I).ReadOnly = True
                            Measure_Data_DataGridView.Columns(I).SortMode = Windows.Forms.DataGridViewColumnSortMode.NotSortable
                            If Fit_Bool = True Then
                                Measure_Data_DataGridView.Columns(I).AutoSizeMode = Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
                            Else
                                Measure_Data_DataGridView.Columns(I).AutoSizeMode = Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader
                            End If
                        Next
                        If Measure_Data_DataGridView.Rows.Count > 1 Then
                            Measure_Data_DataGridView.FirstDisplayedScrollingRowIndex = Measure_Data_DataGridView.Rows.Count - 2
                            Measure_Data_DataGridView.Rows(Measure_Data_DataGridView.Rows.Count - 2).Selected = True
                        End If
                        Measure_Data_DataGridView.RowHeadersVisible = False
                        Measure_Data_DataGridView.AllowUserToDeleteRows = False
                        Measure_Data_DataGridView.AllowUserToResizeColumns = False
                        Measure_Data_DataGridView.AllowUserToResizeRows = False
                        Measure_Data_DataGridView.ColumnHeadersHeightSizeMode = Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
                        Measure_Data_DataGridView.MultiSelect = False
                        Measure_Data_DataGridView.ReadOnly = True
                        Measure_Data_DataGridView.RowHeadersWidthSizeMode = Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
                        Measure_Data_DataGridView.SelectionMode = Windows.Forms.DataGridViewSelectionMode.FullRowSelect
                        DataBase_Command.Dispose()
                        Serch_Measure_Data_DataSet.Dispose()
                        ret = True
                End Select
            Catch ex As Exception
               

                Write_Log(SubName & ex.Message)
                ret = False
            End Try
            Return ret
        End Function

        Enum Pul
            基准对针
            当前对针
        End Enum

        Sub Updata_Pul_Pos(ByRef Axis_Z_Pos As Double, ByVal Pul As Pul)
            Try
                Select Case DataBase_Link_Boolean
                    Case True
                        Select Case Pul
                            Case SQL_LIB.Pul.基准对针
                                DataBase_Command = New SqlCommand("UPDATE DEVICE_PARAMETERS SET Z轴对针初始坐标 = " & Axis_Z_Pos & " WHERE (ID = '1')", DataBase_Connection)
                            Case SQL_LIB.Pul.当前对针
                                DataBase_Command = New SqlCommand("UPDATE DEVICE_PARAMETERS SET Z轴对针当前坐标 = " & Axis_Z_Pos & " WHERE (ID = '1')", DataBase_Connection)
                        End Select
                        DataBase_Command.ExecuteNonQuery()
                        DataBase_Command.Dispose()
                End Select
            Catch ex As Exception
                Write_Log(ex.Message)
            End Try
        End Sub
        Sub Updata_Pul_Pos(ByVal Axis_Z_Pos As Double, ByVal Laser_Z_Pos As Double)
            Try
                Select Case DataBase_Link_Boolean
                    Case True

                        DataBase_Command = New SqlCommand("UPDATE DEVICE_PARAMETERS SET Z轴对针初始坐标 = " & Axis_Z_Pos & ",Z轴对针激光坐标 = " & Laser_Z_Pos & " WHERE (ID = '1')", DataBase_Connection)

                        DataBase_Command.ExecuteNonQuery()
                        DataBase_Command.Dispose()
                End Select
            Catch ex As Exception
                Write_Log(ex.Message)
            End Try
        End Sub

        Function Read_Pul_Pos(ByRef Axis_Z_Pos As Double, ByRef Laser_Z_Pos As Double) As Boolean
            Dim ret As Boolean = False
            Try
                Select Case DataBase_Link_Boolean
                    Case True
                        Dim Product_Name_DataSet_Reg As New DataSet
                        Product_Name_DataSet_Reg.Clear()

                        DataBase_Command.CommandText = "SELECT Z轴对针初始坐标,Z轴对针激光坐标 FROM DEVICE_PARAMETERS WHERE (ID = 1)"

                        DataBase_ConnectionAdapter = New SqlDataAdapter(DataBase_Command.CommandText, DataBase_Connection)
                        DataBase_ConnectionAdapter.Fill(Product_Name_DataSet_Reg)

                        Axis_Z_Pos = Product_Name_DataSet_Reg.Tables(0).Rows(0).Item(0)
                        Laser_Z_Pos = Product_Name_DataSet_Reg.Tables(0).Rows(0).Item(1)
                        DataBase_Command.Dispose()
                        ret = True
                End Select
            Catch ex As Exception
                Write_Log(ex.Message)
                ret = False
            End Try
            Return ret
        End Function

        Function Read_Pul_Pos(ByRef needle_position_register As Double, ByVal Pul As Pul) As Boolean
            Dim ret As Boolean = False
            Try
                Select Case DataBase_Link_Boolean
                    Case True
                        Dim Product_Name_DataSet_Reg As New DataSet
                        Product_Name_DataSet_Reg.Clear()
                        Select Case Pul
                            Case SQL_LIB.Pul.基准对针
                                DataBase_Command.CommandText = "SELECT Z轴对针初始坐标 FROM DEVICE_PARAMETERS WHERE (ID = 1)"
                            Case SQL_LIB.Pul.当前对针
                                DataBase_Command.CommandText = "SELECT Z轴对针当前坐标 FROM DEVICE_PARAMETERS WHERE (ID = 1)"
                        End Select
                        DataBase_ConnectionAdapter = New SqlDataAdapter(DataBase_Command.CommandText, DataBase_Connection)
                        DataBase_ConnectionAdapter.Fill(Product_Name_DataSet_Reg)
                        needle_position_register = Product_Name_DataSet_Reg.Tables(0).Rows(0).Item(0)
                        DataBase_Command.Dispose()
                        ret = True
                End Select
            Catch ex As Exception
                Write_Log(ex.Message)
                ret = False
            End Try
            Return ret
        End Function


        Sub Query_Project_Number(ByRef Total_Count As Integer, ByRef Total_BarCode_Count As Integer, ByRef Total_Not_BarCode_Count As Integer, ByRef Taday_Total_Count As Integer, ByRef Taday_Total_BarCode_Count As Integer, ByRef Taday_Not_BarCode_Count As Integer, ByRef Panel_Count As DevComponents.DotNetBar.PanelEx)
            Try
                Select Case DataBase_Link_Boolean
                    Case True
                        DataBase_Command = New SqlCommand("SELECT COUNT(ID) AS 总数量 FROM LASER_MEASURE", DataBase_Connection)
                        Total_Count = DataBase_Command.ExecuteScalar
                        DataBase_Command.Dispose()

                        DataBase_Command = New SqlCommand("SELECT COUNT(ID) AS 总数量 FROM LASER_MEASURE WHERE (产品条码 <> N'NULL') AND (产品条码 IS NOT NULL) AND 产品条码<> '' AND LEN(产品条码)>=10", DataBase_Connection)
                        Total_BarCode_Count = DataBase_Command.ExecuteScalar
                        DataBase_Command.Dispose()

                        Total_Not_BarCode_Count = Total_Count - Total_BarCode_Count

                        DataBase_Command = New SqlCommand("SELECT COUNT(ID) AS 总数量 FROM LASER_MEASURE WHERE (测量日期 >= CONVERT(DATETIME, '" & Date.Now.Year & "-" & Date.Now.Month & "-" & Date.Now.Day & "', 102))", DataBase_Connection)
                        Taday_Total_Count = DataBase_Command.ExecuteScalar
                        DataBase_Command.Dispose()

                        DataBase_Command = New SqlCommand("SELECT COUNT(ID) AS 总数量 FROM LASER_MEASURE WHERE (产品条码 <> N'NULL') AND (产品条码 IS NOT NULL) AND 产品条码<> '' AND LEN(产品条码)>=10 AND (测量日期 >= CONVERT(DATETIME, '" & Date.Now.Year & "-" & Date.Now.Month & "-" & Date.Now.Day & "', 102))", DataBase_Connection)
                        Taday_Total_BarCode_Count = DataBase_Command.ExecuteScalar
                        DataBase_Command.Dispose()
                        Taday_Not_BarCode_Count = Taday_Total_Count - Taday_Total_BarCode_Count
                        Panel_Count.Text = "总共生产:[" & Taday_Total_Count & "],有条码:[" & Taday_Total_BarCode_Count & "],无条码:[" & Taday_Not_BarCode_Count & "]"
                End Select
            Catch ex As Exception
                Write_Log(ex.Message)
            End Try

        End Sub


        Function Query_Soft_Var() As Boolean
            Try
                Select Case DataBase_Link_Boolean
                    Case True
                        DataBase_Command = New SqlCommand("SELECT COUNT(文件名称列表) AS 计数 FROM DEVICE_SOFT WHERE (Code IS NULL)", DataBase_Connection)
                        If CType(DataBase_Command.ExecuteScalar, Integer) = 0 Then
                            Query_Soft_Var = False
                        Else
                            Query_Soft_Var = True
                        End If
                        DataBase_Command.Dispose()
                End Select
                Return True
            Catch ex As Exception
                Write_Log(ex.Message)
                Return False
            End Try
        End Function

        Sub Update_Soft_Var(ByVal Version As String)
            Try
                Select Case DataBase_Link_Boolean
                    Case True
                        DataBase_Command = New SqlCommand("UPDATE DEVICE_PARAMETERS SET SWVERSION = N'" & Version & "' WHERE (ID = 1)", DataBase_Connection)
                        DataBase_Command.ExecuteNonQuery()
                        DataBase_Command.Dispose()
                End Select
            Catch ex As Exception
                Write_Log(ex.Message)
            End Try

        End Sub

        Sub Update_Device_Info(ByVal Device_Id As String)
            Try
                Select Case DataBase_Link_Boolean
                    Case True
                        DataBase_Command = New SqlCommand("SELECT COUNT(ID) AS ID FROM DEVICE_INFO WHERE (ID = 1)", DataBase_Connection)
                        If CType(DataBase_Command.ExecuteScalar, Integer) > 0 Then
                            DataBase_Command = New SqlCommand("UPDATE DEVICE_INFO SET 识别ID = N'" & Device_Id & "' WHERE (ID = 1)", DataBase_Connection)
                            DataBase_Command.ExecuteNonQuery()
                            DataBase_Command.Dispose()
                        End If
                End Select
            Catch ex As Exception
                Write_Log(ex.Message)
            End Try

        End Sub

        Sub Read_Device_Info(ByRef Info_Array() As String)
            Try
                Select Case DataBase_Link_Boolean
                    Case True
                        Dim Device_Info_DataSet As New DataSet
                        Device_Info_DataSet.Clear()
                        DataBase_Command.CommandText = "SELECT 设备编号, 生产序号 FROM DEVICE_INFO WHERE (ID = 1)"
                        DataBase_ConnectionAdapter = New SqlDataAdapter(DataBase_Command.CommandText, DataBase_Connection)
                        DataBase_ConnectionAdapter.Fill(Device_Info_DataSet)
                        Array.Resize(Info_Array, Device_Info_DataSet.Tables(0).Columns.Count)
                        If Device_Info_DataSet.Tables(0).Rows.Count > 0 Then
                            For i As Integer = 0 To Device_Info_DataSet.Tables(0).Columns.Count - 1
                                If IsDBNull(Device_Info_DataSet.Tables(0).Rows(0).Item(i)) = False Then
                                    Info_Array(i) = Device_Info_DataSet.Tables(0).Rows(0).Item(i).ToString.Trim
                                Else
                                    Info_Array(i) = "NULL"
                                End If
                            Next
                        End If
                        Device_Info_DataSet.Dispose()
                        DataBase_Command.Dispose()
                End Select
            Catch ex As Exception
                Write_Log(ex.Message)
            End Try

        End Sub

        Sub Read_Device_Info(ByRef Panel_Array() As DevComponents.DotNetBar.PanelEx)
            Try
                Select Case DataBase_Link_Boolean
                    Case True
                        Dim Device_Info_DataSet As New DataSet
                        Device_Info_DataSet.Clear()
                        DataBase_Command.CommandText = "SELECT 设备名称, 设备编号, 生产序号, 放置厂区, 放置楼层, 放置线号, 放置站号, 识别ID, 备注信息 FROM DEVICE_INFO WHERE (ID = 1)"
                        DataBase_ConnectionAdapter = New SqlDataAdapter(DataBase_Command.CommandText, DataBase_Connection)
                        DataBase_ConnectionAdapter.Fill(Device_Info_DataSet)
                        If Device_Info_DataSet.Tables(0).Rows.Count > 0 Then
                            For i As Integer = 0 To Device_Info_DataSet.Tables(0).Columns.Count - 1
                                If IsDBNull(Device_Info_DataSet.Tables(0).Rows(0).Item(i)) = False Then
                                    Panel_Array(i).Text = Device_Info_DataSet.Tables(0).Rows(0).Item(i).ToString.Trim
                                Else
                                    Panel_Array(i).Text = "NULL"
                                End If
                            Next
                        End If
                        Device_Info_DataSet.Dispose()
                        DataBase_Command.Dispose()
                End Select
            Catch ex As Exception
                Write_Log(ex.Message)
            End Try
        End Sub

        Sub Read_Device_Info_Array(ByRef Device_Info_Array() As String)
            Try
                Select Case DataBase_Link_Boolean
                    Case True
                        Dim Device_Info_DataSet As New DataSet
                        Device_Info_DataSet.Clear()
                        DataBase_Command.CommandText = "SELECT 设备名称, 设备编号, 放置厂区, 放置楼层, 放置线号, 放置站号 FROM DEVICE_INFO WHERE (ID = 1)"
                        DataBase_ConnectionAdapter = New SqlDataAdapter(DataBase_Command.CommandText, DataBase_Connection)
                        DataBase_ConnectionAdapter.Fill(Device_Info_DataSet)
                        Array.Resize(Device_Info_Array, 6)
                        If Device_Info_DataSet.Tables(0).Rows.Count > 0 Then
                            For i As Integer = 0 To Device_Info_DataSet.Tables(0).Columns.Count - 1
                                If IsDBNull(Device_Info_DataSet.Tables(0).Rows(0).Item(i)) = False Then
                                    Device_Info_Array(i) = Device_Info_DataSet.Tables(0).Rows(0).Item(i).ToString.Trim
                                Else
                                    Device_Info_Array(i) = "NULL"
                                End If
                            Next
                        Else
                            For I As Integer = 0 To Device_Info_Array.Length - 1
                                Device_Info_Array(I) = "NULL"
                            Next
                        End If
                        Device_Info_DataSet.Dispose()
                        DataBase_Command.Dispose()
                End Select
            Catch ex As Exception
                Write_Log(ex.Message)
            End Try
        End Sub

#Region "ADLINK_DIO"
        Function Query_ADLINK_IO_ID(ByVal Value As String, ByVal Card_NO As Integer, ByRef Col_Value As Object) As Integer
            Try
                Select Case DataBase_Link_Boolean
                    Case True
                        Select Case Card_NO
                            Case 1 '1号卡
                                DataBase_Command = New SqlCommand("SELECT ID FROM ADLINK_DIO_PARAMETERS  WHERE(CARD0_DI ='" & Value & "') OR (CARD0_DO ='" & Value & "')", DataBase_Connection)
                            Case 2 '2号卡
                                DataBase_Command = New SqlCommand("SELECT ID FROM ADLINK_DIO_PARAMETERS  WHERE(CARD0_DI ='" & Value & "') OR (CARD0_DO ='" & Value & "') OR (CARD1_DI ='" & Value & "') OR (CARD1_DO ='" & Value & "')", DataBase_Connection)
                            Case 3 '3号卡
                                DataBase_Command = New SqlCommand("SELECT ID FROM ADLINK_DIO_PARAMETERS  WHERE(CARD0_DI ='" & Value & "') OR (CARD0_DO ='" & Value & "') OR (CARD1_DI ='" & Value & "') OR (CARD1_DO ='" & Value & "') OR (CARD2_DI ='" & Value & "') OR (CARD2_DO ='" & Value & "')", DataBase_Connection)
                            Case 4 '4号卡
                                DataBase_Command = New SqlCommand("SELECT ID FROM ADLINK_DIO_PARAMETERS  WHERE(CARD0_DI ='" & Value & "') OR (CARD0_DO ='" & Value & "') OR (CARD1_DI ='" & Value & "') OR (CARD1_DO ='" & Value & "') OR (CARD2_DI ='" & Value & "') OR (CARD2_DO ='" & Value & "') OR (CARD3_DI ='" & Value & "') OR (CARD3_DO ='" & Value & "')", DataBase_Connection)
                        End Select

                        Col_Value = DataBase_Command.ExecuteScalar
                        DataBase_Command.Dispose()
                        If Col_Value IsNot Nothing Then
                            Return 1
                        Else
                            Write_Log("表名：ADLINK_DIO_PARAMETERS 中的【" & Value & "】查询为空，请检查！")
                            Return 0
                        End If
                    Case False
                        Return 0
                End Select
            Catch ex As Exception
                Write_Log(ex.Message)
                Return 0
            End Try
        End Function

        Function Query_Device_Param(ByVal Table_Name As String, ByVal Col_Name As String, ByRef Col_Value As Object) As Boolean
            Try
                Select Case DataBase_Link_Boolean
                    Case True
                        DataBase_Command = New SqlCommand("SELECT 当前值 FROM " & Table_Name & " WHERE (变量名称 = N'" & Col_Name & "')", DataBase_Connection)
                        Col_Value = DataBase_Command.ExecuteScalar
                        DataBase_Command.Dispose()

                        If Col_Value IsNot Nothing Then
                            Dim Type_Str As String = Col_Value.GetType().ToString.ToUpper
                            If Type_Str = "SYSTEM.STRING" Then
                                Col_Value = Col_Value.ToString.Trim
                            End If
                        Else
                            Write_Log("表名：" & Table_Name & "中的【" & Col_Name & "】查询为空，请检查！")
                            Return False
                        End If
                        Return True
                    Case False
                        Return False
                End Select
            Catch ex As Exception
                Write_Log(ex.Message)
                Return False
            End Try
        End Function

#End Region

    End Class

    Public Class KEYENCE_LIB
        Class LINE_LASER
            ''' <summary>设备通信状态</summary>
            Public Enum DeviceStatus
                NoConnection = 0
                Usb
                UsbFast
                Ethernet
                EthernetFast
            End Enum

            ''' <summary>
            ''' 返回值定义
            ''' </summary>
            Public Enum Rc
                Ok = &H0
                ErrOpenDevice = &H1000
                ErrNoDevice
                ErrSend
                ErrReceive
                ErrTimeout
                ErrNomemory
                ErrParameter
                ErrRecvFmt
                ErrHispeedNoDevice = &H1009
                ErrHispeedOpenYet
                ErrHispeedRecvYet
                ErrBufferShort
            End Enum

            ''' <summary>
            ''' Definition that indicates the validity of a measurement value
            ''' </summary>
            Public Enum LJV7IF_MEASURE_DATA_INFO
                LJV7IF_MEASURE_DATA_INFO_VALID = &H0
                LJV7IF_MEASURE_DATA_INFO_ALARM = &H1
                LJV7IF_MEASURE_DATA_INFO_WAIT = &H2
            End Enum

            ''' <summary>
            ''' Definition that indicates the tolerance judgment result of the measurement
            ''' </summary>
            Public Enum LJV7IF_JUDGE_RESULT
                LJV7IF_JUDGE_RESULT_HI = &H1
                LJV7IF_JUDGE_RESULT_GO = &H2
                LJV7IF_JUDGE_RESULT_LO = &H4
            End Enum

            ''' Get batch profile position specification method designation
            Public Enum BatchPos As Byte
                Current = &H0
                Spec = &H2
                Commited = &H3
                CurrentOnly = &H4
            End Enum

            ''' Setting value storage level designation
            Public Enum SettingDepth As Byte
                Write = &H0
                Running = &H1
                Save = &H2
            End Enum

            ''' Definition that indicates the "setting type" in LJV7IF_TARGET_SETTING structure.
            Public Enum SettingType As Byte
                Environment = &H1
                Common = &H2
                Program00 = &H10
                Program01
                Program02
                Program03
                Program04
                Program05
                Program06
                Program07
                Program08
                Program09
                Program10
                Program11
                Program12
                Program13
                Program14
                Program15
            End Enum

            ''' Get profile target buffer designation
            Public Enum ProfileBank As Byte
                Active = &H0
                Inactive = &H1
            End Enum

            ''' Get profile position specification method designation
            Public Enum ProfilePos As Byte
                Current = &H0
                Oldest = &H1
                Spec = &H2
            End Enum

            ''' <summary>
            ''' Ethernet settings structure
            ''' </summary>
            <StructLayout(LayoutKind.Sequential)> Public Structure LJV7IF_ETHERNET_CONFIG
                <MarshalAs(UnmanagedType.ByValArray, SizeConst:=4)> Public abyIpAddress As Byte()
                Public wPortNo As UShort
                <MarshalAs(UnmanagedType.ByValArray, SizeConst:=2)> Public reserve As Byte()
            End Structure

            ''' <summary>
            ''' Date and time structure
            ''' </summary>
            <StructLayout(LayoutKind.Sequential)> Public Structure LJV7IF_TIME
                Public byYear As Byte
                Public byMonth As Byte
                Public byDay As Byte
                Public byHour As Byte
                Public byMinute As Byte
                Public bySecond As Byte
                <MarshalAs(UnmanagedType.ByValArray, SizeConst:=2)> Public reserve As Byte()
            End Structure

            ''' <summary>
            ''' Setting item designation structure
            ''' </summary>
            <StructLayout(LayoutKind.Sequential)> Public Structure LJV7IF_TARGET_SETTING
                Public byType As Byte
                Public byCategory As Byte
                Public byItem As Byte
                Public reserve As Byte
                Public byTarget1 As Byte
                Public byTarget2 As Byte
                Public byTarget3 As Byte
                Public byTarget4 As Byte
            End Structure

            ''' <summary>
            ''' Measurement results structure
            ''' </summary>
            <StructLayout(LayoutKind.Sequential)> Public Structure LJV7IF_MEASURE_DATA
                Public byDataInfo As Byte
                Public byJudge As Byte
                <MarshalAs(UnmanagedType.ByValArray, SizeConst:=2)> Public reserve As Byte()
                Public fValue As Single
            End Structure

            ''' <summary>
            ''' Profile information structure
            ''' </summary>
            <StructLayout(LayoutKind.Sequential)> Public Structure LJV7IF_PROFILE_INFO
                Public byProfileCnt As Byte
                Public byEnvelope As Byte
                <MarshalAs(UnmanagedType.ByValArray, SizeConst:=2)> Public reserve As Byte()
                Public wProfDataCnt As Short
                <MarshalAs(UnmanagedType.ByValArray, SizeConst:=2)> Public reserve2 As Byte()
                Public lXStart As Integer
                Public lXPitch As Integer
            End Structure

            ''' <summary>
            ''' Profile header information structure
            ''' </summary>
            <StructLayout(LayoutKind.Sequential)> Public Structure LJV7IF_PROFILE_HEADER
                Public reserve As UInteger
                Public dwTriggerCnt As UInteger
                Public dwEncoderCnt As UInteger
                <MarshalAs(UnmanagedType.ByValArray, SizeConst:=3)> Public reserve2 As UInteger()
            End Structure

            ''' <summary>
            ''' Profile footer information structure
            ''' </summary>
            <StructLayout(LayoutKind.Sequential)> Public Structure LJV7IF_PROFILE_FOOTER
                Public reserve As UInteger
            End Structure

            ''' <summary>
            ''' High-speed mode get profile request structure (batch measurement: off)
            ''' </summary>
            <StructLayout(LayoutKind.Sequential)> Public Structure LJV7IF_GET_PROFILE_REQ
                Public byTargetBank As Byte
                Public byPosMode As Byte
                <MarshalAs(UnmanagedType.ByValArray, SizeConst:=2)> Public reserve As Byte()
                Public dwGetProfNo As UInteger
                Public byGetProfCnt As Byte
                Public byErase As Byte
                <MarshalAs(UnmanagedType.ByValArray, SizeConst:=2)> Public reserve2 As Byte()
            End Structure

            ''' <summary>
            ''' High-speed mode get profile request structure (batch measurement: on)
            ''' </summary>
            <StructLayout(LayoutKind.Sequential)> Public Structure LJV7IF_GET_BATCH_PROFILE_REQ
                Public byTargetBank As Byte
                Public byPosMode As Byte
                <MarshalAs(UnmanagedType.ByValArray, SizeConst:=2)> Public reserve As Byte()
                Public dwGetBatchNo As UInteger
                Public dwGetProfNo As UInteger
                Public byGetProfCnt As Byte
                Public byErase As Byte
                <MarshalAs(UnmanagedType.ByValArray, SizeConst:=2)> Public reserve2 As Byte()
            End Structure

            ''' <summary>
            ''' Advanced mode get profile request structure (batch measurement: on)
            ''' </summary>
            <StructLayout(LayoutKind.Sequential)> Public Structure LJV7IF_GET_BATCH_PROFILE_ADVANCE_REQ
                Public byPosMode As Byte
                <MarshalAs(UnmanagedType.ByValArray, SizeConst:=3)> Public reserve As Byte()
                Public dwGetBatchNo As UInteger
                Public dwGetProfNo As UInteger
                Public byGetProfCnt As Byte
                <MarshalAs(UnmanagedType.ByValArray, SizeConst:=3)> Public reserve2 As Byte()
            End Structure

            ''' <summary>
            ''' High-speed mode get profile response structure (batch measurement: off)
            ''' </summary>
            <StructLayout(LayoutKind.Sequential)> Public Structure LJV7IF_GET_PROFILE_RSP
                Public dwCurrentProfNo As UInteger
                Public dwOldestProfNo As UInteger
                Public dwGetTopProfNo As UInteger
                Public byGetProfCnt As Byte
                <MarshalAs(UnmanagedType.ByValArray, SizeConst:=3)> Public reserve As Byte()
            End Structure

            ''' <summary>
            ''' High-speed mode get profile response structure (batch measurement: on)
            ''' </summary>
            <StructLayout(LayoutKind.Sequential)> Public Structure LJV7IF_GET_BATCH_PROFILE_RSP
                Public dwCurrentBatchNo As UInteger
                Public dwCurrentBatchProfCnt As UInteger
                Public dwOldestBatchNo As UInteger
                Public dwOldestBatchProfCnt As UInteger
                Public dwGetBatchNo As UInteger
                Public dwGetBatchProfCnt As UInteger
                Public dwGetBatchTopProfNo As UInteger
                Public byGetProfCnt As Byte
                Public byCurrentBatchCommited As Byte
                <MarshalAs(UnmanagedType.ByValArray, SizeConst:=2)> Public reserve As Byte()
            End Structure

            ''' <summary>
            ''' Advanced mode get profile response structure (batch measurement: on)
            ''' </summary>
            <StructLayout(LayoutKind.Sequential)> Public Structure LJV7IF_GET_BATCH_PROFILE_ADVANCE_RSP
                Public dwGetBatchNo As UInteger
                Public dwGetBatchProfCnt As UInteger
                Public dwGetBatchTopProfNo As UInteger
                Public byGetProfCnt As Byte
                <MarshalAs(UnmanagedType.ByValArray, SizeConst:=3)> Public reserve As Byte()
            End Structure

            ''' <summary>
            ''' Storage status request structure
            ''' </summary>
            <StructLayout(LayoutKind.Sequential)> Public Structure LJV7IF_GET_STRAGE_STATUS_REQ
                Public dwRdArea As UInteger
                ' Target surface to read
            End Structure

            ''' <summary>
            ''' Storage status response structure
            ''' </summary>
            <StructLayout(LayoutKind.Sequential)> Public Structure LJV7IF_GET_STRAGE_STATUS_RSP
                Public dwSurfaceCnt As UInteger
                ' Storage surface number
                Public dwActiveSurface As UInteger
                ' Active storage surface
            End Structure

            ''' <summary>
            ''' Storage information structure
            ''' </summary>
            <StructLayout(LayoutKind.Sequential)> Public Structure LJV7IF_STORAGE_INFO
                Public byStatus As Byte
                Public byProgramNo As Byte
                Public byTarget As Byte
                <MarshalAs(UnmanagedType.ByValArray, SizeConst:=5)> Public reserve As Byte()
                Public dwStorageCnt As UInteger
            End Structure

            ''' <summary>
            ''' Get storage data request structure
            ''' </summary>
            <StructLayout(LayoutKind.Sequential)> Public Structure LJV7IF_GET_STORAGE_REQ
                <MarshalAs(UnmanagedType.ByValArray, SizeConst:=4)> Public reserve As Byte()
                Public dwSurface As UInteger
                Public dwStartNo As UInteger
                Public dwDataCnt As UInteger
            End Structure

            ''' <summary>
            ''' Get batch profile storage request structure
            ''' </summary>
            <StructLayout(LayoutKind.Sequential)> Public Structure LJV7IF_GET_BATCH_PROFILE_STORAGE_REQ
                <MarshalAs(UnmanagedType.ByValArray, SizeConst:=4)> Public reserve As Byte()
                Public dwSurface As UInteger
                Public dwGetBatchNo As UInteger
                Public dwGetBatchTopProfNo As UInteger
                Public byGetProfCnt As Byte
                <MarshalAs(UnmanagedType.ByValArray, SizeConst:=3)> Public reserved As Byte()
            End Structure

            ''' <summary>
            ''' Get storage data response structure
            ''' </summary>
            <StructLayout(LayoutKind.Sequential)> Public Structure LJV7IF_GET_STORAGE_RSP
                Public dwStartNo As UInteger
                Public dwDataCnt As UInteger
                Public stBaseTime As LJV7IF_TIME
            End Structure

            ''' <summary>
            ''' Get batch profile storage response structure
            ''' </summary>
            <StructLayout(LayoutKind.Sequential)> Public Structure LJV7IF_GET_BATCH_PROFILE_STORAGE_RSP
                Public dwGetBatchNo As UInteger
                Public dwGetBatchProfCnt As UInteger
                Public dwGetBatchTopProfNo As UInteger
                Public byGetProfCnt As Byte
                <MarshalAs(UnmanagedType.ByValArray, SizeConst:=3)> Public reserve As Byte()
                Public stBaseTime As LJV7IF_TIME
            End Structure

            ''' <summary>
            ''' High-speed communication start preparation request structure
            ''' </summary>
            <StructLayout(LayoutKind.Sequential)> Public Structure LJV7IF_HIGH_SPEED_PRE_START_REQ
                Public bySendPos As Byte
                ' Send start position
                <MarshalAs(UnmanagedType.ByValArray, SizeConst:=3)> Public reserve As Byte()
                ' Reservation 
            End Structure

            ''' <summary>
            ''' Callback function for high-speed communication
            ''' </summary>
            ''' <param name="buffer">Received profile data pointer</param>
            ''' <param name="size">Size in units of bytes of one profile</param>
            ''' <param name="count">Number of profiles</param>
            ''' <param name="notify">Finalization condition</param>
            ''' <param name="user">Thread ID</param>
            Public Delegate Sub HighSpeedDataCallBack(buffer As IntPtr, size As UInteger, count As UInteger, notify As UInteger, user As UInteger)

            ''' <summary>
            ''' Function definitions
            ''' </summary>
            Friend Class NativeMethods
                ''' <summary>
                ''' Get measurement results (the data of all 16 OUTs, including those that are not being measured, is stored).
                ''' </summary>
                Friend Shared ReadOnly Property MeasurementDataCount() As Integer
                    Get
                        Return 16
                    End Get
                End Property

                ''' <summary>
                ''' Number of connectable devices
                ''' </summary>
                Friend Shared ReadOnly Property DeviceCount() As Integer
                    Get
                        Return 6
                    End Get
                End Property

                ''' <summary>
                ''' Fixed value for the bytes of environment settings data 
                ''' </summary>
                Friend Shared ReadOnly Property EnvironmentSettingSize() As UInt32
                    Get
                        Return 60
                    End Get
                End Property

                ''' <summary>
                ''' Fixed value for the bytes of common measurement settings data 
                ''' </summary>
                Friend Shared ReadOnly Property CommonSettingSize() As UInt32
                    Get
                        Return 12
                    End Get
                End Property

                ''' <summary>
                ''' Fixed value for the bytes of program settings data 
                ''' </summary>
                Friend Shared ReadOnly Property ProgramSettingSize() As UInt32
                    Get
                        Return 10932
                    End Get
                End Property

                Declare Function LJV7IF_Initialize Lib "LJV7_IF.DLL" () As Integer
                Declare Function LJV7IF_Finalize Lib "LJV7_IF.DLL" () As Integer
                Declare Function LJV7IF_GetVersion Lib "LJV7_IF.DLL" () As Integer
                Declare Function LJV7IF_UsbOpen Lib "LJV7_IF.DLL" (ByVal lDeviceId As Integer) As Integer
                Declare Function LJV7IF_EthernetOpen Lib "LJV7_IF.DLL" (ByVal lDeviceId As Integer, ByRef ethernetConfig As LJV7IF_ETHERNET_CONFIG) As Integer
                Declare Function LJV7IF_CommClose Lib "LJV7_IF.DLL" (ByVal lDeviceId As Integer) As Integer
                Declare Function LJV7IF_RebootController Lib "LJV7_IF.DLL" (ByVal lDeviceId As Integer) As Integer
                Declare Function LJV7IF_RetrunToFactorySetting Lib "LJV7_IF.DLL" (ByVal lDeviceId As Integer) As Integer
                Declare Function LJV7IF_GetError Lib "LJV7_IF.DLL" (ByVal lDeviceId As Integer, ByVal byRcvMax As Byte, ByRef pbyErrCnt As Byte, ByVal pwErrCode As IntPtr) As Integer
                Declare Function LJV7IF_ClearError Lib "LJV7_IF.DLL" (ByVal lDeviceId As Integer, ByVal wErrCode As Short) As Integer
                Declare Function LJV7IF_Trigger Lib "LJV7_IF.DLL" (ByVal lDeviceId As Integer) As Integer
                Declare Function LJV7IF_StartMeasure Lib "LJV7_IF.DLL" (ByVal lDeviceId As Integer) As Integer
                Declare Function LJV7IF_StopMeasure Lib "LJV7_IF.DLL" (ByVal lDeviceId As Integer) As Integer
                Declare Function LJV7IF_AutoZero Lib "LJV7_IF.DLL" (ByVal lDeviceId As Integer, byOnOff As Byte, dwOut As UInteger) As Integer
                Declare Function LJV7IF_Timing Lib "LJV7_IF.DLL" (ByVal lDeviceId As Integer, byOnOff As Byte, dwOut As UInteger) As Integer
                Declare Function LJV7IF_Reset Lib "LJV7_IF.DLL" (ByVal lDeviceId As Integer, dwOut As UInteger) As Integer
                Declare Function LJV7IF_ClearMemory Lib "LJV7_IF.DLL" (ByVal lDeviceId As Integer) As Integer
                Declare Function LJV7IF_SetSetting Lib "LJV7_IF.DLL" (ByVal lDeviceId As Integer, byDepth As Byte, TargetSetting As LJV7IF_TARGET_SETTING, pData As IntPtr, dwDataSize As UInteger, ByRef pdwError As UInteger) As Integer
                Declare Function LJV7IF_GetSetting Lib "LJV7_IF.DLL" (ByVal lDeviceId As Integer, byDepth As Byte, TargetSetting As LJV7IF_TARGET_SETTING, pData As IntPtr, dwDataSize As UInteger) As Integer
                Declare Function LJV7IF_InitializeSetting Lib "LJV7_IF.DLL" (ByVal lDeviceId As Integer, byDepth As Byte, byTarget As Byte) As Integer
                Declare Function LJV7IF_ReflectSetting Lib "LJV7_IF.DLL" (ByVal lDeviceId As Integer, byDepth As Byte, ByRef pdwError As UInteger) As Integer
                Declare Function LJV7IF_RewriteTemporarySetting Lib "LJV7_IF.DLL" (ByVal lDeviceId As Integer, byDepth As Byte) As Integer
                Declare Function LJV7IF_CheckMemoryAccess Lib "LJV7_IF.DLL" (ByVal lDeviceId As Integer, ByRef pbyBusy As Byte) As Integer
                Declare Function LJV7IF_SetTime Lib "LJV7_IF.DLL" (ByVal lDeviceId As Integer, ByRef time As LJV7IF_TIME) As Integer
                Declare Function LJV7IF_GetTime Lib "LJV7_IF.DLL" (ByVal lDeviceId As Integer, ByRef time As LJV7IF_TIME) As Integer
                Declare Function LJV7IF_ChangeActiveProgram Lib "LJV7_IF.DLL" (ByVal lDeviceId As Integer, byProgNo As Byte) As Integer
                Declare Function LJV7IF_GetActiveProgram Lib "LJV7_IF.DLL" (ByVal lDeviceId As Integer, ByRef pbyProgNo As Byte) As Integer
                Declare Function LJV7IF_GetMeasurementValue Lib "LJV7_IF.DLL" (ByVal lDeviceId As Integer, <Out()> pMeasureData As LJV7IF_MEASURE_DATA()) As Integer
                Declare Function LJV7IF_GetProfile Lib "LJV7_IF.DLL" (ByVal lDeviceId As Integer, ByRef pReq As LJV7IF_GET_PROFILE_REQ, ByRef pRsp As LJV7IF_GET_PROFILE_RSP, ByRef pProfileInfo As LJV7IF_PROFILE_INFO, pdwProfileData As IntPtr, dwDataSize As UInteger) As Integer
                Declare Function LJV7IF_GetBatchProfile Lib "LJV7_IF.DLL" (ByVal lDeviceId As Integer, ByRef pReq As LJV7IF_GET_BATCH_PROFILE_REQ, ByRef pRsp As LJV7IF_GET_BATCH_PROFILE_RSP, ByRef pProfileInfo As LJV7IF_PROFILE_INFO, pdwBatchData As IntPtr, dwDataSize As UInteger) As Integer
                Declare Function LJV7IF_GetProfileAdvance Lib "LJV7_IF.DLL" (ByVal lDeviceId As Integer, ByRef pProfileInfo As LJV7IF_PROFILE_INFO, pdwProfileData As IntPtr, dwDataSize As UInteger, <Out()> pMeasureData As LJV7IF_MEASURE_DATA()) As Integer
                Declare Function LJV7IF_GetBatchProfileAdvance Lib "LJV7_IF.DLL" (ByVal lDeviceId As Integer, ByRef pReq As LJV7IF_GET_BATCH_PROFILE_ADVANCE_REQ, ByRef pRsp As LJV7IF_GET_BATCH_PROFILE_ADVANCE_RSP, ByRef pProfileInfo As LJV7IF_PROFILE_INFO, pdwBatchData As IntPtr, dwDataSize As UInteger, <Out()> pBatchMeasureData As LJV7IF_MEASURE_DATA(), <Out()> pMeasureData As LJV7IF_MEASURE_DATA()) As Integer
                Declare Function LJV7IF_StartStorage Lib "LJV7_IF.DLL" (ByVal lDeviceId As Integer) As Integer
                Declare Function LJV7IF_StopStorage Lib "LJV7_IF.DLL" (ByVal lDeviceId As Integer) As Integer
                Declare Function LJV7IF_GetStorageStatus Lib "LJV7_IF.DLL" (ByVal lDeviceId As Integer, ByRef pReq As LJV7IF_GET_STRAGE_STATUS_REQ, ByRef pRsp As LJV7IF_GET_STRAGE_STATUS_RSP, ByRef pStorageInfo As LJV7IF_STORAGE_INFO) As Integer
                Declare Function LJV7IF_GetStorageData Lib "LJV7_IF.DLL" (ByVal lDeviceId As Integer, ByRef pReq As LJV7IF_GET_STORAGE_REQ, ByRef pStorageInfo As LJV7IF_STORAGE_INFO, ByRef pRsp As LJV7IF_GET_STORAGE_RSP, pdwData As IntPtr, dwDataSize As UInteger) As Integer
                Declare Function LJV7IF_GetStorageProfile Lib "LJV7_IF.DLL" (ByVal lDeviceId As Integer, ByRef pReq As LJV7IF_GET_STORAGE_REQ, ByRef pStorageInfo As LJV7IF_STORAGE_INFO, ByRef pRes As LJV7IF_GET_STORAGE_RSP, ByRef pProfileInfo As LJV7IF_PROFILE_INFO, pdwData As IntPtr, dwDataSize As UInteger) As Integer
                Declare Function LJV7IF_GetStorageBatchProfile Lib "LJV7_IF.DLL" (ByVal lDeviceId As Integer, ByRef pReq As LJV7IF_GET_BATCH_PROFILE_STORAGE_REQ, ByRef pStorageInfo As LJV7IF_STORAGE_INFO, ByRef pRes As LJV7IF_GET_BATCH_PROFILE_STORAGE_RSP, ByRef pProfileInfo As LJV7IF_PROFILE_INFO, pdwData As IntPtr, dwDataSize As UInteger, ByRef pTimeOffset As UInteger, <Out()> pMeasureData As LJV7IF_MEASURE_DATA()) As Integer
                Declare Function LJV7IF_HighSpeedDataUsbCommunicationInitalize Lib "LJV7_IF.DLL" (ByVal lDeviceId As Integer, pCallBack As HighSpeedDataCallBack, dwProfileCnt As UInteger, dwThreadId As UInteger) As Integer
                Declare Function LJV7IF_HighSpeedDataEthernetCommunicationInitalize Lib "LJV7_IF.DLL" (ByVal lDeviceId As Integer, ByRef pEthernetConfig As LJV7IF_ETHERNET_CONFIG, wHighSpeedPortNo As UShort, pCallBack As HighSpeedDataCallBack, dwProfileCnt As UInteger, dwThreadId As UInteger) As Integer
                Declare Function LJV7IF_PreStartHighSpeedDataCommunication Lib "LJV7_IF.DLL" (ByVal lDeviceId As Integer, ByRef pReq As LJV7IF_HIGH_SPEED_PRE_START_REQ, ByRef pProfileInfo As LJV7IF_PROFILE_INFO) As Integer
                Declare Function LJV7IF_StartHighSpeedDataCommunication Lib "LJV7_IF.DLL" (ByVal lDeviceId As Integer) As Integer
                Declare Function LJV7IF_StopHighSpeedDataCommunication Lib "LJV7_IF.DLL" (ByVal lDeviceId As Integer) As Integer
                Declare Function LJV7IF_HighSpeedDataCommunicationFinalize Lib "LJV7_IF.DLL" (ByVal lDeviceId As Integer) As Integer
            End Class

            ''' <summary>
            ''' Measurement data class
            ''' </summary>
            Public Class MeasureData

                ''' <summary>
                ''' Elapsed time(ms)
                ''' </summary>
                Private _offsetTime As UInteger

                ''' <summary>
                ''' Measurement results
                ''' </summary>
                Private _data As LJV7IF_MEASURE_DATA()

                ''' <summary>
                ''' Elapsed time(ms)
                ''' </summary>
                Public Property OffsetTime() As UInteger
                    Get
                        Return _offsetTime
                    End Get
                    Set(value As UInteger)
                        _offsetTime = value
                    End Set
                End Property

                ''' <summary>
                ''' Measurement results
                ''' </summary>
                Public ReadOnly Property Data() As LJV7IF_MEASURE_DATA()
                    Get
                        Return _data
                    End Get
                End Property

                ''' <summary>
                ''' Constructor
                ''' </summary>
                ''' <param name="data">Measurement results</param>
                Public Sub New(data As LJV7IF_MEASURE_DATA())
                    _data = data
                End Sub

                ''' <summary>
                ''' Constructor
                ''' </summary>
                ''' <param name="data">Measurement results</param>
                Public Sub New(data As Byte())
                    _offsetTime = 0
                    Dim measureDataIndex As Integer = 0
                    _data = New LJV7IF_MEASURE_DATA(NativeMethods.MeasurementDataCount - 1) {}
                    Dim measureDataSize As Integer = Utility.GetByteSize(Utility.TypeOfStruct.MEASURE_DATA)

                    For i As Integer = 0 To NativeMethods.MeasurementDataCount - 1
                        _data(i).byDataInfo = data(measureDataIndex + 0)
                        _data(i).byJudge = data(measureDataIndex + 1)
                        _data(i).fValue = BitConverter.ToSingle(data, (measureDataIndex + 4))
                        measureDataIndex += measureDataSize
                    Next
                End Sub

                ''' <summary>
                ''' Constructor
                ''' </summary>
                ''' <param name="offsetTime">Elapsed time</param>
                ''' <param name="data">Measurement results</param>
                Public Sub New(offsetTime As UInteger, data As LJV7IF_MEASURE_DATA())
                    _offsetTime = offsetTime
                    _data = data
                End Sub

                ''' <summary>
                ''' Constructor
                ''' </summary>
                ''' <param name="data">Start position</param>
                Public Sub New(data As Byte(), startIndex As Integer)
                    _offsetTime = BitConverter.ToUInt32(data, startIndex)
                    Dim measureDataIndex As Integer = startIndex + Marshal.SizeOf(GetType(UInteger))
                    _data = New LJV7IF_MEASURE_DATA(NativeMethods.MeasurementDataCount - 1) {}
                    Dim measureDataSize As Integer = Utility.GetByteSize(Utility.TypeOfStruct.MEASURE_DATA)

                    For i As Integer = 0 To NativeMethods.MeasurementDataCount - 1
                        _data(i).byDataInfo = data(measureDataIndex + 0)
                        _data(i).byJudge = data(measureDataIndex + 1)
                        _data(i).fValue = BitConverter.ToSingle(data, (measureDataIndex + 4))
                        measureDataIndex += measureDataSize
                    Next
                End Sub

                ''' <summary>
                ''' Size acquisition
                ''' </summary>
                ''' <returns>Data size</returns>
                Public Shared Function GetByteSize() As Integer
                    Return (Marshal.SizeOf(GetType(UInteger)) + Utility.GetByteSize(Utility.TypeOfStruct.MEASURE_DATA) * NativeMethods.MeasurementDataCount)
                End Function

                ''' <summary>
                ''' ToString override
                ''' </summary>
                ''' <returns>String for display</returns>
                Public Overrides Function ToString() As String
                    Dim sb As New StringBuilder()

                    For i As Integer = 0 To NativeMethods.MeasurementDataCount - 1
                        sb.Append(String.Format("{0,0:f4}" & vbTab, _data(i).fValue))
                    Next

                    Return sb.ToString()
                End Function
            End Class

            ''' <summary>
            ''' Utility class
            ''' </summary>
            NotInheritable Class Utility
                Private Sub New()
                End Sub

                ''' <summary>
                ''' Structure classification
                ''' </summary>
                Public Enum TypeOfStruct
                    PROFILE_HEADER
                    PROFILE_FOOTER
                    MEASURE_DATA
                End Enum

                ''' <summary>
                ''' Storage structure (storage status)
                ''' </summary>
                Private Const STORAGE_INFO_STATUS_EMPTY As Integer = 0
                Private Const STORAGE_INFO_STATUS_STORING As Integer = 1
                Private Const STORAGE_INFO_STATUS_FINISHED As Integer = 2

                ''' <summary>
                ''' Storage structure (storage target)
                ''' </summary>
                Private Const STORAGE_INFO_TARGET_DATA As Integer = 0
                Private Const STORAGE_INFO_TARGET_PROFILE As Integer = 2
                Private Const STORAGE_INFO_TARGET_BATCH As Integer = 3

                ''' <summary>
                ''' Get the byte size of the structure.
                ''' </summary>
                ''' <returns>Byte size</returns>
                Public Shared Function GetByteSize(type As TypeOfStruct) As Integer
                    Select Case type
                        Case TypeOfStruct.PROFILE_HEADER
                            Dim profileHeader As New LJV7IF_PROFILE_HEADER()
                            Return Marshal.SizeOf(profileHeader)

                        Case TypeOfStruct.PROFILE_FOOTER
                            Dim profileFooter As New LJV7IF_PROFILE_FOOTER()
                            Return Marshal.SizeOf(profileFooter)

                        Case TypeOfStruct.MEASURE_DATA
                            Dim measureData As New LJV7IF_MEASURE_DATA()
                            Return Marshal.SizeOf(measureData)
                    End Select
                    Return 0
                End Function

                ''' <summary>
                ''' Get the string for log output.
                ''' </summary>
                ''' <returns>String for log output</returns>
                Public Shared Function ConvertToLogString(storageInfo As LJV7IF_STORAGE_INFO) As StringBuilder
                    Dim sb As New StringBuilder()

                    Dim status As String = String.Empty
                    Select Case storageInfo.byStatus
                        Case STORAGE_INFO_STATUS_EMPTY
                            status = "EMPTY"
                            Exit Select
                        Case STORAGE_INFO_STATUS_STORING
                            status = "STORING"
                            Exit Select
                        Case STORAGE_INFO_STATUS_FINISHED
                            status = "FINISHED"
                            Exit Select
                        Case Else
                            status = "UNEXPECTED"
                            Exit Select
                    End Select
                    sb.AppendLine(String.Format("  Status" & vbTab & vbTab & ": {0}", status))
                    sb.AppendLine(String.Format("  ProgNo" & vbTab & vbTab & ": {0}", storageInfo.byProgramNo))
                    Dim target As String = String.Empty
                    Select Case storageInfo.byTarget
                        Case STORAGE_INFO_TARGET_DATA
                            target = "DATA"
                            Exit Select
                        Case STORAGE_INFO_TARGET_PROFILE
                            target = "PROFILE"
                            Exit Select
                        Case STORAGE_INFO_TARGET_BATCH
                            target = "BATCH PROFILE"
                            Exit Select
                        Case Else
                            target = "UNEXPECTED"
                            Exit Select
                    End Select
                    sb.AppendLine(String.Format("  Target" & vbTab & vbTab & ": {0}", target))
                    sb.Append(String.Format("  StorageCnt" & vbTab & ": {0}", storageInfo.dwStorageCnt))
                    Return sb
                End Function

                ''' <summary>
                ''' Get the string for log output.
                ''' </summary>
                ''' <returns>String for log output</returns>
                Public Shared Function ConvertToLogString(storageRsp As LJV7IF_GET_STORAGE_RSP) As StringBuilder
                    Dim sb As New StringBuilder()
                    sb.AppendLine(String.Format("  StartNo" & vbTab & ": {0}", storageRsp.dwStartNo))
                    sb.AppendLine(String.Format("  DataCnt" & vbTab & ": {0}", storageRsp.dwDataCnt))
                    sb.Append(ConvertToLogString(storageRsp.stBaseTime).ToString())
                    Return sb
                End Function

                ''' <summary>
                ''' Get the string for log output.
                ''' </summary>
                ''' <returns>String for log output</returns>
                Public Shared Function ConvertToLogString(measureData As LJV7IF_MEASURE_DATA) As StringBuilder
                    Dim sb As New StringBuilder()

                    Dim dataInfo As String = String.Empty
                    Select Case measureData.byDataInfo
                        Case CInt(LJV7IF_MEASURE_DATA_INFO.LJV7IF_MEASURE_DATA_INFO_VALID)
                            dataInfo = "Valid" & vbTab & vbTab
                            Exit Select
                        Case CInt(LJV7IF_MEASURE_DATA_INFO.LJV7IF_MEASURE_DATA_INFO_ALARM)
                            dataInfo = "Alarm value  "
                            Exit Select
                        Case CInt(LJV7IF_MEASURE_DATA_INFO.LJV7IF_MEASURE_DATA_INFO_WAIT)
                            dataInfo = "Judgment wait value  "
                            Exit Select
                        Case Else
                            dataInfo = "Unexpected value" & vbTab
                            Exit Select
                    End Select
                    sb.Append(dataInfo)
                    Dim judge As String = String.Empty
                    Select Case measureData.byJudge
                        Case 0
                            judge = "______  "
                            Exit Select
                        Case CInt(LJV7IF_JUDGE_RESULT.LJV7IF_JUDGE_RESULT_HI)
                            judge = "HI____  "
                            Exit Select
                        Case CInt(LJV7IF_JUDGE_RESULT.LJV7IF_JUDGE_RESULT_GO)
                            judge = "__GO__  "
                            Exit Select
                        Case CInt(LJV7IF_JUDGE_RESULT.LJV7IF_JUDGE_RESULT_LO)
                            judge = "____LO  "
                            Exit Select
                        Case CInt(LJV7IF_JUDGE_RESULT.LJV7IF_JUDGE_RESULT_HI Or LJV7IF_JUDGE_RESULT.LJV7IF_JUDGE_RESULT_LO)
                            judge = "HI__LO  "
                            Exit Select
                        Case CInt(LJV7IF_JUDGE_RESULT.LJV7IF_JUDGE_RESULT_HI Or LJV7IF_JUDGE_RESULT.LJV7IF_JUDGE_RESULT_GO Or LJV7IF_JUDGE_RESULT.LJV7IF_JUDGE_RESULT_LO)
                            judge = "ALL BIT  "
                            Exit Select
                        Case Else
                            judge = "UNEXPECTED "
                            Exit Select
                    End Select
                    sb.Append(judge)
                    sb.Append(measureData.fValue.ToString())
                    Return sb
                End Function

                ''' <summary>
                ''' Get the string for log output.
                ''' </summary>
                ''' <returns>String for log output</returns>
                Public Shared Function ConvertToLogString(profileInfo As LJV7IF_PROFILE_INFO) As StringBuilder
                    Dim sb As New StringBuilder()
                    ' Profile information of the profile obtained
                    sb.AppendLine(String.Format("  Profile Data Num" & vbTab & vbTab & vbTab & ": {0}", profileInfo.byProfileCnt))
                    Dim envelope As String = If(profileInfo.byEnvelope = 0, "OFF", "ON")
                    sb.AppendLine(String.Format("  Envelope" & vbTab & vbTab & vbTab & ": {0}", envelope))
                    sb.AppendLine(String.Format("  Profile Data Points" & vbTab & vbTab & vbTab & ": {0}", profileInfo.wProfDataCnt))
                    sb.AppendLine(String.Format("  X coordinate of the first point" & vbTab & ": {0}", profileInfo.lXStart))
                    sb.Append(String.Format("  X-direction interval" & vbTab & vbTab & ": {0}", profileInfo.lXPitch))
                    Return sb
                End Function

                ''' <summary>
                ''' Get the string for log output.
                ''' </summary>
                ''' <returns>String for log output</returns>
                Public Shared Function ConvertToLogString(rsp As LJV7IF_GET_BATCH_PROFILE_STORAGE_RSP) As StringBuilder
                    Dim sb As New StringBuilder()
                    ' Profile information of the profile obtained
                    sb.AppendLine(String.Format("  BatchNo" & vbTab & vbTab & " : {0}", rsp.dwGetBatchNo))
                    sb.AppendLine(String.Format("  BatchProfCnt" & vbTab & " : {0}", rsp.dwGetBatchProfCnt))
                    sb.AppendLine(String.Format("  BatchTopProfNo" & vbTab & " : {0}", rsp.dwGetBatchTopProfNo))
                    sb.AppendLine(String.Format("  ProfCnt" & vbTab & vbTab & " : {0}", rsp.byGetProfCnt))
                    sb.Append(ConvertToLogString(rsp.stBaseTime).ToString())
                    Return sb
                End Function

                ''' <summary>
                ''' Get the string for log output.
                ''' </summary>
                ''' <returns>String for log output</returns>
                Public Shared Function ConvertToLogString(rsp As LJV7IF_GET_BATCH_PROFILE_RSP) As StringBuilder
                    Dim sb As New StringBuilder()
                    ' Profile information of the profile obtained
                    sb.AppendLine(String.Format("  CurrentBatchNo" & vbTab & vbTab & vbTab & ": {0}", rsp.dwCurrentBatchNo))
                    sb.AppendLine(String.Format("  CurrentBatchProfCnt" & vbTab & vbTab & ": {0}", rsp.dwCurrentBatchProfCnt))
                    sb.AppendLine(String.Format("  OldestBatchNo" & vbTab & vbTab & vbTab & ": {0}", rsp.dwOldestBatchNo))
                    sb.AppendLine(String.Format("  OldestBatchProfCnt" & vbTab & vbTab & ": {0}", rsp.dwOldestBatchProfCnt))
                    sb.AppendLine(String.Format("  GetBatchNo" & vbTab & vbTab & vbTab & ": {0}", rsp.dwGetBatchNo))
                    sb.AppendLine(String.Format("  GetBatchProfCnt" & vbTab & vbTab & vbTab & ": {0}", rsp.dwGetBatchProfCnt))
                    sb.AppendLine(String.Format("  GetBatchTopProfNo" & vbTab & vbTab & ": {0}", rsp.dwGetBatchTopProfNo))
                    sb.AppendLine(String.Format("  GetProfCnt" & vbTab & vbTab & vbTab & ": {0}", rsp.byGetProfCnt))
                    sb.Append(String.Format("  CurrentBatchCommited" & vbTab & vbTab & ": {0}", rsp.byCurrentBatchCommited))
                    Return sb
                End Function

                ''' <summary>
                ''' Get the string for log output.
                ''' </summary>
                ''' <returns>String for log output</returns>
                Public Shared Function ConvertToLogString(rsp As LJV7IF_GET_BATCH_PROFILE_ADVANCE_RSP) As StringBuilder
                    Dim sb As New StringBuilder()
                    ' Profile information of the profile obtained
                    sb.AppendLine(String.Format("  GetBatchNo" & vbTab & vbTab & ": {0}", rsp.dwGetBatchNo))
                    sb.AppendLine(String.Format("  GetBatchProfCnt" & vbTab & vbTab & ": {0}", rsp.dwGetBatchProfCnt))
                    sb.AppendLine(String.Format("  GetBatchTopProfNo" & vbTab & ": {0}", rsp.dwGetBatchTopProfNo))
                    sb.Append(String.Format("  GetProfCnt" & vbTab & vbTab & ": {0}", rsp.byGetProfCnt))
                    Return sb
                End Function

                ''' <summary>
                ''' Get the string for log output.
                ''' </summary>
                ''' <returns>String for log output</returns>
                Public Shared Function ConvertToLogString(time As LJV7IF_TIME) As StringBuilder
                    Dim sb As New StringBuilder()
                    sb.Append(String.Format("yy/mm/dd hh:mm:ss " & vbLf & " {0,0:d2}/{1,0:d2}/{2,0:d2} {3,0:d2}:{4,0:d2}:{5,0:d2}", time.byYear, time.byMonth, time.byDay, time.byHour, time.byMinute, _
                     time.bySecond))
                    Return sb
                End Function

                ''' <summary>
                ''' Get the string for log output.
                ''' </summary>
                ''' <returns>String for log output</returns>
                Public Shared Function ConvertToLogString(rsp As LJV7IF_GET_PROFILE_RSP) As StringBuilder
                    Dim sb As New StringBuilder()
                    sb.AppendLine(String.Format("  CurrentProfNo" & vbTab & ": {0}", rsp.dwCurrentProfNo))
                    sb.AppendLine(String.Format("  OldestProfNo" & vbTab & ": {0}", rsp.dwOldestProfNo))
                    sb.AppendLine(String.Format("  GetTopProfNo" & vbTab & ": {0}", rsp.dwGetTopProfNo))
                    sb.Append(String.Format("  GetProfCnt" & vbTab & ": {0}", rsp.byGetProfCnt))
                    Return sb
                End Function

                ''' <summary>
                ''' Get the string for log output.
                ''' </summary>
                ''' <returns>String for log output</returns>
                Public Shared Function ConvertToLogString(rsp As LJV7IF_GET_STRAGE_STATUS_RSP) As StringBuilder
                    Dim sb As New StringBuilder()
                    sb.AppendLine(String.Format("  SurfaceCnt" & vbTab & ": {0}", rsp.dwSurfaceCnt))
                    sb.AppendLine(String.Format("  ActiveSurface" & vbTab & ": {0}", rsp.dwActiveSurface))
                    Return sb
                End Function
            End Class

            ''' <summary>
            ''' Profile data class
            ''' </summary>
            Public Class ProfileData

                ''' <summary>
                ''' Trigger count
                ''' </summary>
                Private _triggerCnt As Integer

                ''' <summary>
                ''' Encoder count
                ''' </summary>
                Private _encoderCnt As Integer

                ''' <summary>
                ''' Profile data
                ''' </summary>
                Private _profData As Integer()

                ''' <summary>
                ''' Profile information
                ''' </summary>
                Private _profileInfo As LJV7IF_PROFILE_INFO

                ''' <summary>
                ''' Trigger count
                ''' </summary>
                Public ReadOnly Property TriggerCnt() As Integer
                    Get
                        Return _triggerCnt
                    End Get
                End Property

                ''' <summary>
                ''' Encoder count
                ''' </summary>
                Public ReadOnly Property EncodeCnt() As Integer
                    Get
                        Return _encoderCnt
                    End Get
                End Property

                ''' <summary>
                ''' Profile Data
                ''' </summary>
                Public ReadOnly Property ProfDatas() As Integer()
                    Get
                        Return _profData
                    End Get
                End Property

                ''' <summary>
                ''' Profile Imformation
                ''' </summary>
                Public ReadOnly Property ProfInfo() As LJV7IF_PROFILE_INFO
                    Get
                        Return _profileInfo
                    End Get
                End Property

                ''' <summary>
                ''' Constructor
                ''' </summary>
                Public Sub New(receiveBuffer As Integer(), profileInfo As LJV7IF_PROFILE_INFO)
                    SetData(receiveBuffer, profileInfo)
                End Sub

                ''' <summary>
                ''' Constructor
                ''' </summary>
                ''' <param name="receiveBuffer">Receive buffer</param>
                ''' <param name="startIndex">Start position</param>
                ''' <param name="profileInfo">Profile information</param>
                Public Sub New(receiveBuffer As Byte(), startIndex As Integer, profileInfo As LJV7IF_PROFILE_INFO)
                    Dim bufIntSize As Integer = CalculateDataSize(profileInfo)
                    Dim bufIntArray As Integer() = New Integer(bufIntSize - 1) {}
                    _profileInfo = profileInfo
                    ' Conversion from byte[] to int[]
                    For i As Integer = 0 To bufIntSize - 1
                        bufIntArray(i) = BitConverter.ToInt32(receiveBuffer, (startIndex + i * Marshal.SizeOf(GetType(Integer))))
                    Next
                    SetData(bufIntArray, profileInfo)
                End Sub

                ''' <summary>
                ''' Constructor Overload
                ''' </summary>
                ''' <param name="receiveBuffer">Receive buffer</param>
                ''' <param name="startIndex">Start position</param>
                ''' <param name="profileInfo">Profile information</param>
                Public Sub New(receiveBuffer As Integer(), startIndex As Integer, profileInfo As LJV7IF_PROFILE_INFO)
                    Dim bufIntSize As Integer = CalculateDataSize(profileInfo)
                    Dim bufIntArray As Integer() = New Integer(bufIntSize - 1) {}
                    _profileInfo = profileInfo

                    Array.Copy(receiveBuffer, startIndex, bufIntArray, 0, bufIntSize)
                    SetData(bufIntArray, profileInfo)
                End Sub

                ''' <summary>
                ''' Set the members to the arguments.
                ''' </summary>
                ''' <param name="receiveBuffer">Receive buffer</param>
                ''' <param name="profileInfo">Profile information</param>
                Private Sub SetData(receiveBuffer As Integer(), profileInfo As LJV7IF_PROFILE_INFO)
                    _profileInfo = profileInfo
                    ' Extract the header.
                    Dim headerSize As Integer = Utility.GetByteSize(Utility.TypeOfStruct.PROFILE_HEADER) \ Marshal.SizeOf(GetType(Integer))
                    Dim headerData As Integer() = New Integer(headerSize - 1) {}
                    Array.Copy(receiveBuffer, 0, headerData, 0, headerSize)
                    _triggerCnt = headerData(1)
                    _encoderCnt = headerData(2)
                    ' Extract the footer.
                    Dim footerSize As Integer = Utility.GetByteSize(Utility.TypeOfStruct.PROFILE_FOOTER) \ Marshal.SizeOf(GetType(Integer))
                    Dim footerData As Integer() = New Integer(footerSize - 1) {}
                    Array.Copy(receiveBuffer, receiveBuffer.Length - footerSize, footerData, 0, footerSize)
                    ' Extract the profile data.
                    Dim profSize As Integer = receiveBuffer.Length - headerSize - footerSize
                    _profData = New Integer(profSize - 1) {}
                    Array.Copy(receiveBuffer, headerSize, _profData, 0, profSize)
                End Sub

                ''' <summary>
                ''' Get the data string.
                ''' </summary>
                ''' <returns>Retained data</returns>
                Public Function GetStringData() As StringBuilder
                    Dim retString As New StringBuilder()
                    retString.AppendLine(String.Format("Trigger count" & vbTab & ":{0}", _triggerCnt))
                    retString.AppendLine(String.Format("Encoder count:{0}", _encoderCnt))
                    Return retString
                End Function

                ''' <summary>
                ''' Data size calculation
                ''' </summary>
                ''' <param name="profileInfo">Profile information</param>
                ''' <returns>Profile data size</returns>
                Public Shared Function CalculateDataSize(profileInfo As LJV7IF_PROFILE_INFO) As Integer
                    Dim header As New LJV7IF_PROFILE_HEADER()
                    Dim footer As New LJV7IF_PROFILE_FOOTER()
                    Return profileInfo.wProfDataCnt * profileInfo.byProfileCnt * (profileInfo.byEnvelope + 1) + (Marshal.SizeOf(header) + Marshal.SizeOf(footer)) \ Marshal.SizeOf(GetType(Integer))
                End Function

                ''' <summary>
                ''' ToString override
                ''' </summary>
                ''' <returns>String for display</returns>
                Public Overrides Function ToString() As String
                    Dim sb As New StringBuilder()
                    ' Data position calculation
                    Dim posX As Double = ProfInfo.lXStart
                    Dim deltaX As Double = ProfInfo.lXPitch
                    Dim singleProfileCount As Integer = ProfInfo.wProfDataCnt
                    Dim dataCount As Integer = CInt(ProfInfo.byProfileCnt) * (ProfInfo.byEnvelope + 1)
                    For i As Integer = 0 To singleProfileCount - 1
                        sb.AppendFormat("{0}" & vbTab, posX + deltaX * i)
                        For j As Integer = 0 To dataCount - 1
                            sb.AppendFormat("{0}" & vbTab, _profData(i + ProfInfo.wProfDataCnt * j))
                        Next
                        sb.AppendLine()
                    Next
                    Return sb.ToString()
                End Function

                ''' <summary>
                ''' Create the X-position string from the profile information.
                ''' </summary>
                ''' <param name="profileInfo">Profile information</param>
                ''' <returns>X-position string</returns>
                Public Shared Function GetXPosString(profileInfo As LJV7IF_PROFILE_INFO) As String
                    Dim sb As New StringBuilder()
                    ' Data position calculation
                    Dim posX As Double = profileInfo.lXStart
                    Dim deltaX As Double = profileInfo.lXPitch
                    Dim singleProfileCount As Integer = profileInfo.wProfDataCnt
                    Dim dataCount As Integer = CInt(profileInfo.byProfileCnt) * (profileInfo.byEnvelope + 1)
                    For i As Integer = 0 To dataCount - 1
                        For j As Integer = 0 To singleProfileCount - 1
                            sb.AppendFormat("{0}" & vbTab, (posX + deltaX * j))
                        Next
                    Next
                    Return sb.ToString()
                End Function
            End Class

            ''' <summary>
            ''' Object pinning class
            ''' </summary>
            Public NotInheritable Class PinnedObject
                Implements IDisposable
                Private _Handle As GCHandle

                ''' <summary>
                ''' Get the address.
                ''' </summary>
                Public ReadOnly Property Pointer() As IntPtr
                    ' Get the leading address of the current object that is pinned.
                    Get
                        Return _Handle.AddrOfPinnedObject()
                    End Get
                End Property

                ''' <summary>
                ''' Constructor
                ''' </summary>
                ''' <param name="target">Target to protect from the garbage collector</param>
                Public Sub New(target As Object)
                    ' Pin the target to protect it from the garbage collector.
                    _Handle = GCHandle.Alloc(target, GCHandleType.Pinned)
                End Sub

                ''' <summary>
                ''' Interface
                ''' </summary>
                Public Sub Dispose() Implements IDisposable.Dispose
                    _Handle.Free()
                    _Handle = New GCHandle()
                End Sub
            End Class

            ''' <summary>
            ''' Device data class
            ''' </summary>
            Public Class DeviceData
                Private _status As DeviceStatus = DeviceStatus.NoConnection
                Private _ethernetConfig As LJV7IF_ETHERNET_CONFIG
                Private _profileData As List(Of ProfileData)
                Private _measureData As List(Of MeasureData)

                ''' <summary>
                ''' Status property
                ''' </summary>
                Public Property Status() As DeviceStatus
                    Get
                        Return _status
                    End Get
                    Set(value As DeviceStatus)
                        _profileData.Clear()
                        _ethernetConfig = New LJV7IF_ETHERNET_CONFIG()
                        _status = value
                    End Set
                End Property

                Public Property EthernetConfig() As LJV7IF_ETHERNET_CONFIG
                    Get
                        Return _ethernetConfig
                    End Get
                    Set(value As LJV7IF_ETHERNET_CONFIG)
                        _ethernetConfig = value
                    End Set
                End Property

                Public Property ProfileData() As List(Of ProfileData)
                    Get
                        Return _profileData
                    End Get
                    Set(value As List(Of ProfileData))
                        _profileData = value
                    End Set
                End Property

                Public Property MeasureData() As List(Of MeasureData)
                    Get
                        Return _measureData
                    End Get
                    Set(value As List(Of MeasureData))
                        _measureData = value
                    End Set
                End Property

                ''' <summary>
                ''' Constructor
                ''' </summary>
                Public Sub New()
                    _ethernetConfig = New LJV7IF_ETHERNET_CONFIG()
                    _profileData = New List(Of ProfileData)()
                    _measureData = New List(Of MeasureData)()
                End Sub

                ''' <summary>
                ''' Connection status acquisition
                ''' </summary>
                ''' <returns>Connection status for display</returns>
                Public Function GetStatusString() As String
                    Dim status As String = _status.ToString()
                    Select Case _status
                        Case DeviceStatus.Ethernet, DeviceStatus.EthernetFast
                            status += String.Format("---{0}.{1}.{2}.{3}", _ethernetConfig.abyIpAddress(0), _ethernetConfig.abyIpAddress(1), _ethernetConfig.abyIpAddress(2), _ethernetConfig.abyIpAddress(3))
                            Exit Select
                        Case Else
                            Exit Select
                    End Select
                    Return status
                End Function


            End Class

            Public Class KEYENCE_FUN

                Structure MEASURE_DATA_STRU
                    Dim MEASE_OUT00 As Double
                    Dim MEASE_OUT01 As Double
                    Dim MEASE_OUT02 As Double
                    Dim MEASE_OUT03 As Double
                    Dim MEASE_OUT04 As Double
                    Dim MEASE_OUT05 As Double
                    Dim MEASE_OUT06 As Double
                    Dim MEASE_OUT07 As Double
                    Dim MEASE_OUT08 As Double
                    Dim MEASE_OUT09 As Double
                    Dim MEASE_OUT10 As Double
                    Dim MEASE_OUT11 As Double
                    Dim MEASE_OUT12 As Double
                    Dim MEASE_OUT13 As Double
                    Dim MEASE_OUT14 As Double
                    Dim MEASE_OUT15 As Double
                End Structure

                Structure MEASURE_PRO_NO_STRU
                    Const PROGRAM_00 As UShort = 0
                    Const PROGRAM_01 As UShort = 1
                    Const PROGRAM_02 As UShort = 2
                    Const PROGRAM_03 As UShort = 3
                    Const PROGRAM_04 As UShort = 4
                    Const PROGRAM_05 As UShort = 5
                    Const PROGRAM_06 As UShort = 6
                    Const PROGRAM_07 As UShort = 7
                    Const PROGRAM_08 As UShort = 8
                    Const PROGRAM_09 As UShort = 9
                    Const PROGRAM_10 As UShort = 10
                    Const PROGRAM_11 As UShort = 11
                    Const PROGRAM_12 As UShort = 12
                    Const PROGRAM_13 As UShort = 13
                    Const PROGRAM_14 As UShort = 14
                    Const PROGRAM_15 As UShort = 15
                End Structure

                Function Initialize(Optional ByRef SubName As String = Nothing, Optional ByRef ErrLine As String = Nothing) As Integer
                    Dim ret As Integer = Nothing
                    Try
                        SubName = ErrName & GetCurrentMethod.Name & ":"
                        ret = NativeMethods.LJV7IF_Initialize()
                    Catch ex As Exception
                       
                     
                        ret = -1
                    End Try
                    Return ret
                End Function

                Function LJV_Finalize(Optional ByRef SubName As String = Nothing, Optional ByRef ErrLine As String = Nothing) As Integer
                    Dim ret As Integer = Nothing
                    Try
                        SubName = ErrName & GetCurrentMethod.Name & ":"
                        ret = NativeMethods.LJV7IF_Finalize()
                    Catch ex As Exception
                       
                     
                        ret = -1
                    End Try
                    Return ret
                End Function

                ''' <summary>
                ''' 用以太网打开控制器
                ''' </summary>
                ''' <param name="DeviceId">控制器ID</param>
                ''' <param name="IP">IP地址</param>
                ''' <param name="PortNo">通讯端口</param>
                ''' <param name="SubName"></param>
                ''' <param name="ErrLine"></param>
                ''' <returns></returns>
                ''' <remarks></remarks>
                Function EthernetOpen(ByVal DeviceId As Integer, ByVal IP As String, ByVal PortNo As Integer, Optional ByRef SubName As String = Nothing, Optional ByRef ErrLine As String = Nothing) As Integer
                    Dim ret As Integer = Nothing
                    Try
                        SubName = ErrName & GetCurrentMethod.Name & ":"
                        Dim IP_BYTE() As String = Nothing
                        Dim IP_ADD() As Byte = Nothing
                        Dim Ethernet_Config As LJV7IF_ETHERNET_CONFIG = Nothing
                        IP_BYTE = Split(IP, ".")
                        If IP_BYTE.Length = 4 Then
                            Array.Resize(IP_ADD, IP_BYTE.Length)
                            IP_ADD(0) = CType(IP_BYTE(0), Byte)
                            IP_ADD(1) = CType(IP_BYTE(1), Byte)
                            IP_ADD(2) = CType(IP_BYTE(2), Byte)
                            IP_ADD(3) = CType(IP_BYTE(3), Byte)
                            Ethernet_Config.abyIpAddress = IP_ADD
                            Ethernet_Config.wPortNo = PortNo
                            ret = NativeMethods.LJV7IF_EthernetOpen(DeviceId, Ethernet_Config)
                        Else
                            ret = -1
                        End If
                    Catch ex As Exception
                       
                     
                        ret = -1
                    End Try
                    Return ret
                End Function


                Function ChangeActiveProgram(ByVal DeviceId As Integer, ByVal Program_Id As UShort, Optional ByRef SubName As String = Nothing, Optional ByRef ErrLine As String = Nothing) As Integer
                    Dim ret As Integer = Nothing
                    Try
                        SubName = ErrName & GetCurrentMethod.Name & ":"
                        ret = NativeMethods.LJV7IF_ChangeActiveProgram(DeviceId, Program_Id)
                    Catch ex As Exception
                       
                     
                        ret = -1
                    End Try
                    Return ret
                End Function

                Function StartStorage(ByVal DeviceId As Integer, Optional ByRef SubName As String = Nothing, Optional ByRef ErrLine As String = Nothing) As Integer
                    Dim ret As Integer = Nothing
                    Try
                        SubName = ErrName & GetCurrentMethod.Name & ":"
                        ret = NativeMethods.LJV7IF_StartStorage(DeviceId)
                    Catch ex As Exception
                       
                     
                        ret = -1
                    End Try
                    Return ret
                End Function

                Function StopStorage(ByVal DeviceId As Integer, Optional ByRef SubName As String = Nothing, Optional ByRef ErrLine As String = Nothing) As Integer
                    Dim ret As Integer = Nothing
                    Try
                        SubName = ErrName & GetCurrentMethod.Name & ":"
                        ret = NativeMethods.LJV7IF_StopStorage(DeviceId)
                    Catch ex As Exception
                       
                     
                        ret = -1
                    End Try
                    Return ret
                End Function

                Function ReadStorageData(ByVal DeviceId As Integer, ByRef MeasureDataArry_List As List(Of MEASURE_DATA_STRU), Optional ByRef SubName As String = Nothing, Optional ByRef ErrLine As String = Nothing) As Integer

                    Dim MeasureDataArry() As MEASURE_DATA_STRU = Nothing
                    MeasureDataArry_List = New List(Of MEASURE_DATA_STRU)
                    MeasureDataArry_List.Clear()

                    Dim ret As Integer = Nothing
                    Try
                        SubName = ErrName & GetCurrentMethod.Name & ":"
                        Dim measureDatas As New List(Of MeasureData)
                        Dim req As New LJV7IF_GET_STRAGE_STATUS_REQ()
                        Dim rsp As New LJV7IF_GET_STRAGE_STATUS_RSP()
                        Dim rsp1 As New LJV7IF_GET_STORAGE_RSP()
                        Dim req1 As New LJV7IF_GET_STORAGE_REQ()
                        Dim storageInfo As New LJV7IF_STORAGE_INFO()
                        measureDatas.Clear()
                        req.dwRdArea = 0
                        ret = NativeMethods.LJV7IF_GetStorageStatus(DeviceId, req, rsp, storageInfo)
                        req1.dwSurface = 0
                        Const LEN As Int16 = 3099

                        Dim oneDataSize As UInt64
                        Dim allDataSize As UInt64
                        Dim receiveData As Byte()

                        If storageInfo.dwStorageCnt > LEN Then
                            Dim M_INT As Int16 = Int(storageInfo.dwStorageCnt / LEN)
                            Dim M_Model As Int16 = storageInfo.dwStorageCnt Mod LEN
                            Dim M As Int16 = 0
                            For M = 0 To M_INT - 1
                                If M = 0 Then
                                    req1.dwStartNo = 0
                                    req1.dwDataCnt = (M + 1) * LEN
                                Else
                                    req1.dwStartNo = M * LEN + 1
                                    req1.dwDataCnt = (M + 1) * LEN
                                End If

                                oneDataSize = CUInt(Marshal.SizeOf(GetType(UInteger)) + CUInt(Utility.GetByteSize(Utility.TypeOfStruct.MEASURE_DATA)) * CUInt(NativeMethods.MeasurementDataCount))
                                allDataSize = 0
                                If req1.dwDataCnt > System.Int16.MaxValue Then
                                    allDataSize = Define.READ_DATA_SIZE
                                Else
                                    allDataSize = CType(Math.Min(Define.READ_DATA_SIZE, oneDataSize * req1.dwDataCnt), UInteger)
                                End If
                                receiveData = Nothing
                                receiveData = New Byte(CType(allDataSize - 1, Int64)) {}
                                Using pin As New PinnedObject(receiveData)
                                    ret = NativeMethods.LJV7IF_GetStorageData(DeviceId, req1, storageInfo, rsp1, pin.Pointer, allDataSize)
                                    If ret = CInt(Rc.Ok) Then
                                        Dim byteSize As Integer = MeasureData.GetByteSize
                                        For i As Integer = 0 To CInt(rsp1.dwDataCnt) - 1
                                            measureDatas.Add(New MeasureData(receiveData, byteSize * i))
                                        Next
                                        MeasureDataArry = Nothing
                                        Array.Resize(MeasureDataArry, rsp1.dwDataCnt)
                                        For i As Integer = 0 To rsp1.dwDataCnt - 1
                                            If measureDatas(i).Data(0).fValue.ToString.Trim.IndexOf("E") < 0 Then
                                                MeasureDataArry(i).MEASE_OUT00 = Format(measureDatas(i).Data(0).fValue, "000.0000")
                                            Else
                                                MeasureDataArry(i).MEASE_OUT00 = 999.999
                                            End If

                                            If measureDatas(i).Data(1).fValue.ToString.Trim.IndexOf("E") < 0 Then
                                                MeasureDataArry(i).MEASE_OUT01 = Format(measureDatas(i).Data(1).fValue, "000.0000")
                                            Else
                                                MeasureDataArry(i).MEASE_OUT01 = 999.999
                                            End If

                                            If measureDatas(i).Data(2).fValue.ToString.Trim.IndexOf("E") < 0 Then
                                                MeasureDataArry(i).MEASE_OUT02 = Format(measureDatas(i).Data(2).fValue, "000.0000")
                                            Else
                                                MeasureDataArry(i).MEASE_OUT02 = 999.999
                                            End If

                                            If measureDatas(i).Data(3).fValue.ToString.Trim.IndexOf("E") < 0 Then
                                                MeasureDataArry(i).MEASE_OUT03 = Format(measureDatas(i).Data(3).fValue, "000.0000")
                                            Else
                                                MeasureDataArry(i).MEASE_OUT03 = 999.999
                                            End If

                                            If measureDatas(i).Data(4).fValue.ToString.Trim.IndexOf("E") < 0 Then
                                                MeasureDataArry(i).MEASE_OUT04 = Format(measureDatas(i).Data(4).fValue, "000.0000")
                                            Else
                                                MeasureDataArry(i).MEASE_OUT04 = 999.999
                                            End If

                                            If measureDatas(i).Data(5).fValue.ToString.Trim.IndexOf("E") < 0 Then
                                                MeasureDataArry(i).MEASE_OUT05 = Format(measureDatas(i).Data(5).fValue, "000.0000")
                                            Else
                                                MeasureDataArry(i).MEASE_OUT05 = 999.999
                                            End If

                                            If measureDatas(i).Data(6).fValue.ToString.Trim.IndexOf("E") < 0 Then
                                                MeasureDataArry(i).MEASE_OUT06 = Format(measureDatas(i).Data(6).fValue, "000.0000")
                                            Else
                                                MeasureDataArry(i).MEASE_OUT06 = 999.999
                                            End If

                                            If measureDatas(i).Data(7).fValue.ToString.Trim.IndexOf("E") < 0 Then
                                                MeasureDataArry(i).MEASE_OUT07 = Format(measureDatas(i).Data(7).fValue, "000.0000")
                                            Else
                                                MeasureDataArry(i).MEASE_OUT07 = 999.999
                                            End If

                                            If measureDatas(i).Data(8).fValue.ToString.Trim.IndexOf("E") < 0 Then
                                                MeasureDataArry(i).MEASE_OUT08 = Format(measureDatas(i).Data(8).fValue, "000.0000")
                                            Else
                                                MeasureDataArry(i).MEASE_OUT08 = 999.999
                                            End If

                                            If measureDatas(i).Data(9).fValue.ToString.Trim.IndexOf("E") < 0 Then
                                                MeasureDataArry(i).MEASE_OUT09 = Format(measureDatas(i).Data(9).fValue, "000.0000")
                                            Else
                                                MeasureDataArry(i).MEASE_OUT09 = 999.999
                                            End If

                                            If measureDatas(i).Data(10).fValue.ToString.Trim.IndexOf("E") < 0 Then
                                                MeasureDataArry(i).MEASE_OUT10 = Format(measureDatas(i).Data(10).fValue, "000.0000")
                                            Else
                                                MeasureDataArry(i).MEASE_OUT10 = 999.999
                                            End If

                                            If measureDatas(i).Data(11).fValue.ToString.Trim.IndexOf("E") < 0 Then
                                                MeasureDataArry(i).MEASE_OUT11 = Format(measureDatas(i).Data(11).fValue, "000.0000")
                                            Else
                                                MeasureDataArry(i).MEASE_OUT11 = 999.999
                                            End If

                                            If measureDatas(i).Data(12).fValue.ToString.Trim.IndexOf("E") < 0 Then
                                                MeasureDataArry(i).MEASE_OUT12 = Format(measureDatas(i).Data(12).fValue, "000.0000")
                                            Else
                                                MeasureDataArry(i).MEASE_OUT12 = 999.999
                                            End If

                                            If measureDatas(i).Data(13).fValue.ToString.Trim.IndexOf("E") < 0 Then
                                                MeasureDataArry(i).MEASE_OUT13 = Format(measureDatas(i).Data(13).fValue, "000.0000")
                                            Else
                                                MeasureDataArry(i).MEASE_OUT13 = 999.999
                                            End If

                                            If measureDatas(i).Data(14).fValue.ToString.Trim.IndexOf("E") < 0 Then
                                                MeasureDataArry(i).MEASE_OUT14 = Format(measureDatas(i).Data(14).fValue, "000.0000")
                                            Else
                                                MeasureDataArry(i).MEASE_OUT14 = 999.999
                                            End If

                                            If measureDatas(i).Data(15).fValue.ToString.Trim.IndexOf("E") < 0 Then
                                                MeasureDataArry(i).MEASE_OUT15 = Format(measureDatas(i).Data(15).fValue, "000.0000")
                                            Else
                                                MeasureDataArry(i).MEASE_OUT15 = 999.999
                                            End If
                                            '增加到列表
                                            MeasureDataArry_List.Add(MeasureDataArry(i))
                                        Next
                                    Else
                                        ret = -1
                                    End If
                                End Using
                            Next
                            req1.dwStartNo = M * LEN + 1
                            req1.dwDataCnt = req1.dwStartNo + M_Model

                            oneDataSize = CUInt(Marshal.SizeOf(GetType(UInteger)) + CUInt(Utility.GetByteSize(Utility.TypeOfStruct.MEASURE_DATA)) * CUInt(NativeMethods.MeasurementDataCount))
                            allDataSize = 0
                            'allDataSize = CType(Math.Min(Define.READ_DATA_SIZE, oneDataSize * req1.dwDataCnt), UInteger)
                            If req1.dwDataCnt > System.Int16.MaxValue Then
                                allDataSize = Define.READ_DATA_SIZE
                            Else
                                allDataSize = CType(Math.Min(Define.READ_DATA_SIZE, oneDataSize * req1.dwDataCnt), UInteger)
                            End If
                            receiveData = Nothing
                            receiveData = New Byte(CType(allDataSize - 1, Int64)) {}
                            Using pin As New PinnedObject(receiveData)
                                ret = NativeMethods.LJV7IF_GetStorageData(DeviceId, req1, storageInfo, rsp1, pin.Pointer, allDataSize)
                                If ret = CInt(Rc.Ok) Then
                                    Dim byteSize As Integer = MeasureData.GetByteSize
                                    For i As Integer = 0 To CInt(rsp1.dwDataCnt) - 1
                                        measureDatas.Add(New MeasureData(receiveData, byteSize * i))
                                    Next
                                    MeasureDataArry = Nothing
                                    Array.Resize(MeasureDataArry, rsp1.dwDataCnt)
                                    For i As Integer = 0 To rsp1.dwDataCnt - 1
                                        If measureDatas(i).Data(0).fValue.ToString.Trim.IndexOf("E") < 0 Then
                                            MeasureDataArry(i).MEASE_OUT00 = Format(measureDatas(i).Data(0).fValue, "000.0000")
                                        Else
                                            MeasureDataArry(i).MEASE_OUT00 = 999.999
                                        End If

                                        If measureDatas(i).Data(1).fValue.ToString.Trim.IndexOf("E") < 0 Then
                                            MeasureDataArry(i).MEASE_OUT01 = Format(measureDatas(i).Data(1).fValue, "000.0000")
                                        Else
                                            MeasureDataArry(i).MEASE_OUT01 = 999.999
                                        End If

                                        If measureDatas(i).Data(2).fValue.ToString.Trim.IndexOf("E") < 0 Then
                                            MeasureDataArry(i).MEASE_OUT02 = Format(measureDatas(i).Data(2).fValue, "000.0000")
                                        Else
                                            MeasureDataArry(i).MEASE_OUT02 = 999.999
                                        End If

                                        If measureDatas(i).Data(3).fValue.ToString.Trim.IndexOf("E") < 0 Then
                                            MeasureDataArry(i).MEASE_OUT03 = Format(measureDatas(i).Data(3).fValue, "000.0000")
                                        Else
                                            MeasureDataArry(i).MEASE_OUT03 = 999.999
                                        End If

                                        If measureDatas(i).Data(4).fValue.ToString.Trim.IndexOf("E") < 0 Then
                                            MeasureDataArry(i).MEASE_OUT04 = Format(measureDatas(i).Data(4).fValue, "000.0000")
                                        Else
                                            MeasureDataArry(i).MEASE_OUT04 = 999.999
                                        End If

                                        If measureDatas(i).Data(5).fValue.ToString.Trim.IndexOf("E") < 0 Then
                                            MeasureDataArry(i).MEASE_OUT05 = Format(measureDatas(i).Data(5).fValue, "000.0000")
                                        Else
                                            MeasureDataArry(i).MEASE_OUT05 = 999.999
                                        End If

                                        If measureDatas(i).Data(6).fValue.ToString.Trim.IndexOf("E") < 0 Then
                                            MeasureDataArry(i).MEASE_OUT06 = Format(measureDatas(i).Data(6).fValue, "000.0000")
                                        Else
                                            MeasureDataArry(i).MEASE_OUT06 = 999.999
                                        End If

                                        If measureDatas(i).Data(7).fValue.ToString.Trim.IndexOf("E") < 0 Then
                                            MeasureDataArry(i).MEASE_OUT07 = Format(measureDatas(i).Data(7).fValue, "000.0000")
                                        Else
                                            MeasureDataArry(i).MEASE_OUT07 = 999.999
                                        End If

                                        If measureDatas(i).Data(8).fValue.ToString.Trim.IndexOf("E") < 0 Then
                                            MeasureDataArry(i).MEASE_OUT08 = Format(measureDatas(i).Data(8).fValue, "000.0000")
                                        Else
                                            MeasureDataArry(i).MEASE_OUT08 = 999.999
                                        End If

                                        If measureDatas(i).Data(9).fValue.ToString.Trim.IndexOf("E") < 0 Then
                                            MeasureDataArry(i).MEASE_OUT09 = Format(measureDatas(i).Data(9).fValue, "000.0000")
                                        Else
                                            MeasureDataArry(i).MEASE_OUT09 = 999.999
                                        End If

                                        If measureDatas(i).Data(10).fValue.ToString.Trim.IndexOf("E") < 0 Then
                                            MeasureDataArry(i).MEASE_OUT10 = Format(measureDatas(i).Data(10).fValue, "000.0000")
                                        Else
                                            MeasureDataArry(i).MEASE_OUT10 = 999.999
                                        End If

                                        If measureDatas(i).Data(11).fValue.ToString.Trim.IndexOf("E") < 0 Then
                                            MeasureDataArry(i).MEASE_OUT11 = Format(measureDatas(i).Data(11).fValue, "000.0000")
                                        Else
                                            MeasureDataArry(i).MEASE_OUT11 = 999.999
                                        End If

                                        If measureDatas(i).Data(12).fValue.ToString.Trim.IndexOf("E") < 0 Then
                                            MeasureDataArry(i).MEASE_OUT12 = Format(measureDatas(i).Data(12).fValue, "000.0000")
                                        Else
                                            MeasureDataArry(i).MEASE_OUT12 = 999.999
                                        End If

                                        If measureDatas(i).Data(13).fValue.ToString.Trim.IndexOf("E") < 0 Then
                                            MeasureDataArry(i).MEASE_OUT13 = Format(measureDatas(i).Data(13).fValue, "000.0000")
                                        Else
                                            MeasureDataArry(i).MEASE_OUT13 = 999.999
                                        End If

                                        If measureDatas(i).Data(14).fValue.ToString.Trim.IndexOf("E") < 0 Then
                                            MeasureDataArry(i).MEASE_OUT14 = Format(measureDatas(i).Data(14).fValue, "000.0000")
                                        Else
                                            MeasureDataArry(i).MEASE_OUT14 = 999.999
                                        End If

                                        If measureDatas(i).Data(15).fValue.ToString.Trim.IndexOf("E") < 0 Then
                                            MeasureDataArry(i).MEASE_OUT15 = Format(measureDatas(i).Data(15).fValue, "000.0000")
                                        Else
                                            MeasureDataArry(i).MEASE_OUT15 = 999.999
                                        End If

                                        '增加到列表
                                        MeasureDataArry_List.Add(MeasureDataArry(i))
                                    Next
                                Else
                                    ret = -1
                                End If
                            End Using
                        Else
                            req1.dwStartNo = 0
                            req1.dwDataCnt = storageInfo.dwStorageCnt

                            oneDataSize = CUInt(Marshal.SizeOf(GetType(UInteger)) + CUInt(Utility.GetByteSize(Utility.TypeOfStruct.MEASURE_DATA)) * CUInt(NativeMethods.MeasurementDataCount))
                            allDataSize = 0
                            'allDataSize = CType(Math.Min(Define.READ_DATA_SIZE, oneDataSize * req1.dwDataCnt), UInteger)
                            If req1.dwDataCnt > System.Int16.MaxValue Then
                                allDataSize = Define.READ_DATA_SIZE
                            Else
                                allDataSize = CType(Math.Min(Define.READ_DATA_SIZE, oneDataSize * req1.dwDataCnt), UInteger)
                            End If
                            receiveData = Nothing
                            receiveData = New Byte(CType(allDataSize - 1, Int64)) {}
                            Using pin As New PinnedObject(receiveData)
                                ret = NativeMethods.LJV7IF_GetStorageData(DeviceId, req1, storageInfo, rsp1, pin.Pointer, allDataSize)
                                If ret = CInt(Rc.Ok) Then
                                    Dim byteSize As Integer = MeasureData.GetByteSize
                                    For i As Integer = 0 To CInt(rsp1.dwDataCnt) - 1
                                        measureDatas.Add(New MeasureData(receiveData, byteSize * i))
                                    Next
                                    MeasureDataArry = Nothing
                                    Array.Resize(MeasureDataArry, rsp1.dwDataCnt)
                                    For i As Integer = 0 To rsp1.dwDataCnt - 1
                                        If measureDatas(i).Data(0).fValue.ToString.Trim.IndexOf("E") < 0 Then
                                            MeasureDataArry(i).MEASE_OUT00 = Format(measureDatas(i).Data(0).fValue, "000.0000")
                                        Else
                                            MeasureDataArry(i).MEASE_OUT00 = 999.999
                                        End If

                                        If measureDatas(i).Data(1).fValue.ToString.Trim.IndexOf("E") < 0 Then
                                            MeasureDataArry(i).MEASE_OUT01 = Format(measureDatas(i).Data(1).fValue, "000.0000")
                                        Else
                                            MeasureDataArry(i).MEASE_OUT01 = 999.999
                                        End If

                                        If measureDatas(i).Data(2).fValue.ToString.Trim.IndexOf("E") < 0 Then
                                            MeasureDataArry(i).MEASE_OUT02 = Format(measureDatas(i).Data(2).fValue, "000.0000")
                                        Else
                                            MeasureDataArry(i).MEASE_OUT02 = 999.999
                                        End If

                                        If measureDatas(i).Data(3).fValue.ToString.Trim.IndexOf("E") < 0 Then
                                            MeasureDataArry(i).MEASE_OUT03 = Format(measureDatas(i).Data(3).fValue, "000.0000")
                                        Else
                                            MeasureDataArry(i).MEASE_OUT03 = 999.999
                                        End If

                                        If measureDatas(i).Data(4).fValue.ToString.Trim.IndexOf("E") < 0 Then
                                            MeasureDataArry(i).MEASE_OUT04 = Format(measureDatas(i).Data(4).fValue, "000.0000")
                                        Else
                                            MeasureDataArry(i).MEASE_OUT04 = 999.999
                                        End If

                                        If measureDatas(i).Data(5).fValue.ToString.Trim.IndexOf("E") < 0 Then
                                            MeasureDataArry(i).MEASE_OUT05 = Format(measureDatas(i).Data(5).fValue, "000.0000")
                                        Else
                                            MeasureDataArry(i).MEASE_OUT05 = 999.999
                                        End If

                                        If measureDatas(i).Data(6).fValue.ToString.Trim.IndexOf("E") < 0 Then
                                            MeasureDataArry(i).MEASE_OUT06 = Format(measureDatas(i).Data(6).fValue, "000.0000")
                                        Else
                                            MeasureDataArry(i).MEASE_OUT06 = 999.999
                                        End If

                                        If measureDatas(i).Data(7).fValue.ToString.Trim.IndexOf("E") < 0 Then
                                            MeasureDataArry(i).MEASE_OUT07 = Format(measureDatas(i).Data(7).fValue, "000.0000")
                                        Else
                                            MeasureDataArry(i).MEASE_OUT07 = 999.999
                                        End If

                                        If measureDatas(i).Data(8).fValue.ToString.Trim.IndexOf("E") < 0 Then
                                            MeasureDataArry(i).MEASE_OUT08 = Format(measureDatas(i).Data(8).fValue, "000.0000")
                                        Else
                                            MeasureDataArry(i).MEASE_OUT08 = 999.999
                                        End If

                                        If measureDatas(i).Data(9).fValue.ToString.Trim.IndexOf("E") < 0 Then
                                            MeasureDataArry(i).MEASE_OUT09 = Format(measureDatas(i).Data(9).fValue, "000.0000")
                                        Else
                                            MeasureDataArry(i).MEASE_OUT09 = 999.999
                                        End If

                                        If measureDatas(i).Data(10).fValue.ToString.Trim.IndexOf("E") < 0 Then
                                            MeasureDataArry(i).MEASE_OUT10 = Format(measureDatas(i).Data(10).fValue, "000.0000")
                                        Else
                                            MeasureDataArry(i).MEASE_OUT10 = 999.999
                                        End If

                                        If measureDatas(i).Data(11).fValue.ToString.Trim.IndexOf("E") < 0 Then
                                            MeasureDataArry(i).MEASE_OUT11 = Format(measureDatas(i).Data(11).fValue, "000.0000")
                                        Else
                                            MeasureDataArry(i).MEASE_OUT11 = 999.999
                                        End If

                                        If measureDatas(i).Data(12).fValue.ToString.Trim.IndexOf("E") < 0 Then
                                            MeasureDataArry(i).MEASE_OUT12 = Format(measureDatas(i).Data(12).fValue, "000.0000")
                                        Else
                                            MeasureDataArry(i).MEASE_OUT12 = 999.999
                                        End If

                                        If measureDatas(i).Data(13).fValue.ToString.Trim.IndexOf("E") < 0 Then
                                            MeasureDataArry(i).MEASE_OUT13 = Format(measureDatas(i).Data(13).fValue, "000.0000")
                                        Else
                                            MeasureDataArry(i).MEASE_OUT13 = 999.999
                                        End If

                                        If measureDatas(i).Data(14).fValue.ToString.Trim.IndexOf("E") < 0 Then
                                            MeasureDataArry(i).MEASE_OUT14 = Format(measureDatas(i).Data(14).fValue, "000.0000")
                                        Else
                                            MeasureDataArry(i).MEASE_OUT14 = 999.999
                                        End If

                                        If measureDatas(i).Data(15).fValue.ToString.Trim.IndexOf("E") < 0 Then
                                            MeasureDataArry(i).MEASE_OUT15 = Format(measureDatas(i).Data(15).fValue, "000.0000")
                                        Else
                                            MeasureDataArry(i).MEASE_OUT15 = 999.999
                                        End If
                                        '增加到列表
                                        MeasureDataArry_List.Add(MeasureDataArry(i))
                                    Next
                                Else
                                    ret = -1
                                End If
                            End Using
                        End If
                    Catch ex As Exception
                       
                     
                        ret = -1
                    End Try
                    Return ret
                End Function

                Function ReadStorageProfile(ByVal DeviceId As Integer) As Integer
                    Dim ret As Integer = Nothing, SubName As String = ErrName & GetCurrentMethod.Name & ":"
                    Try

                        Dim pReq As LJV7IF_GET_STORAGE_REQ = Nothing
                        Dim pStorageInfo As LJV7IF_STORAGE_INFO = Nothing
                        Dim pRes As LJV7IF_GET_STORAGE_RSP = Nothing
                        Dim pProfileInfo As LJV7IF_PROFILE_INFO = Nothing

                        Dim measureDatas As New List(Of LJV7IF_PROFILE_INFO)
                        Dim req As New LJV7IF_GET_STRAGE_STATUS_REQ()
                        Dim rsp As New LJV7IF_GET_STRAGE_STATUS_RSP()
                        Dim rsp1 As New LJV7IF_GET_STORAGE_RSP()
                        Dim req1 As New LJV7IF_GET_STORAGE_REQ()
                        Dim storageInfo As New LJV7IF_STORAGE_INFO()
                        measureDatas.Clear()
                        req.dwRdArea = 0
                        ret = NativeMethods.LJV7IF_GetStorageStatus(DeviceId, req, rsp, storageInfo)
                        req1.dwSurface = 0
                        req1.dwStartNo = 0
                        req1.dwDataCnt = storageInfo.dwStorageCnt
                        Dim oneDataSize As UInteger = CUInt(Marshal.SizeOf(GetType(UInteger)) + CUInt(Utility.GetByteSize(Utility.TypeOfStruct.MEASURE_DATA)) * CUInt(NativeMethods.MeasurementDataCount))
                        Dim allDataSize As UInteger = 0
                        allDataSize = CType(Math.Min(Define.READ_DATA_SIZE, oneDataSize * req1.dwDataCnt), UInteger)
                        Dim receiveData As Byte() = New Byte(CType(allDataSize - 1, Integer)) {}
                        Using pin As New PinnedObject(receiveData)
                            ret = NativeMethods.LJV7IF_GetStorageProfile(DeviceId, pReq, pStorageInfo, pRes, pProfileInfo, pin.Pointer, allDataSize)
                            If ret = CInt(Rc.Ok) Then
                                Dim byteSize As Integer = MeasureData.GetByteSize
                                For i As Integer = 0 To CInt(rsp1.dwDataCnt) - 1

                                Next
                            End If
                        End Using


                        'ret = NativeMethods.LJV7IF_GetStorageProfile(DeviceId, pReq, pStorageInfo, pRes, pProfileInfo, pdwData, dwDataSize)

                    Catch ex As Exception
                        ret = -1
                    End Try
                    Return ret
                End Function

                Function ClearMemory(ByVal DeviceId As Integer, Optional ByRef SubName As String = Nothing, Optional ByRef ErrLine As String = Nothing) As Integer
                    Dim ret As Integer = Nothing
                    Try
                        SubName = ErrName & GetCurrentMethod.Name & ":"
                        ret = NativeMethods.LJV7IF_ClearMemory(DeviceId)
                    Catch ex As Exception
                       
                     
                        ret = -1
                    End Try
                    Return ret
                End Function

                Function UsbOpen(ByVal DeviceId As Integer, Optional ByRef SubName As String = Nothing, Optional ByRef ErrLine As String = Nothing) As Integer
                    Dim ret As Integer = Nothing
                    Try
                        SubName = ErrName & GetCurrentMethod.Name & ":"
                        ret = NativeMethods.LJV7IF_UsbOpen(DeviceId)
                    Catch ex As Exception
                       
                     
                        ret = -1
                    End Try
                    Return ret
                End Function

                Function CommClose(ByVal DeviceId As Integer, Optional ByRef SubName As String = Nothing, Optional ByRef ErrLine As String = Nothing) As Integer
                    Dim ret As Integer = Nothing
                    Try
                        SubName = ErrName & GetCurrentMethod.Name & ":"
                        ret = NativeMethods.LJV7IF_CommClose(DeviceId)
                    Catch ex As Exception
                       
                     
                        ret = -1
                    End Try
                    Return ret
                End Function

                Function RebootController(ByVal DeviceId As Integer, Optional ByRef SubName As String = Nothing, Optional ByRef ErrLine As String = Nothing) As Integer
                    Dim ret As Integer = Nothing
                    Try
                        SubName = ErrName & GetCurrentMethod.Name & ":"
                        ret = NativeMethods.LJV7IF_RebootController(DeviceId)
                    Catch ex As Exception
                       
                     
                        ret = -1
                    End Try
                    Return ret
                End Function

                Function RetrunToFactorySetting(ByVal DeviceId As Integer, Optional ByRef SubName As String = Nothing, Optional ByRef ErrLine As String = Nothing) As Integer
                    Dim ret As Integer = Nothing
                    Try
                        SubName = ErrName & GetCurrentMethod.Name & ":"
                        ret = NativeMethods.LJV7IF_RetrunToFactorySetting(DeviceId)
                    Catch ex As Exception
                       
                     
                        ret = -1
                    End Try
                    Return ret
                End Function

                Function GetError(ByVal DeviceId As Integer, ByVal byRcvMax As Byte, ByRef pbyErrCnt As Byte, ByVal pwErrCode As IntPtr, Optional ByRef SubName As String = Nothing, Optional ByRef ErrLine As String = Nothing) As Integer
                    Dim ret As Integer = Nothing
                    Try
                        SubName = ErrName & GetCurrentMethod.Name & ":"
                        ret = NativeMethods.LJV7IF_GetError(DeviceId, byRcvMax, pbyErrCnt, pwErrCode)
                    Catch ex As Exception
                       
                     
                        ret = -1
                    End Try
                    Return ret
                End Function

                Function GetMeasurementValue(ByVal DeviceId As Integer, ByRef MeasureData() As Double, Optional ByRef SubName As String = Nothing, Optional ByRef ErrLine As String = Nothing) As Integer
                    Dim ret As Integer = Nothing
                    Try
                        SubName = ErrName & GetCurrentMethod.Name & ":"
                        Dim Keyence_MeasureData(15) As LJV7IF_MEASURE_DATA
                        ret = NativeMethods.LJV7IF_GetMeasurementValue(DeviceId, Keyence_MeasureData)
                    Catch ex As Exception
                       
                     
                        ret = -1
                    End Try
                    Return ret
                End Function

            End Class

            NotInheritable Class Define

                Private Sub New()
                End Sub

                Public Const MAX_PROFILE_COUNT As Integer = 3200
                Public Const DEVICE_ID As Integer = 0
                Public Const WRITE_DATA_SIZE As Integer = 20 * 1024
                Public Const READ_DATA_SIZE As Integer = 1024 * 1024
                Public Const PROFILE_DATA_MAX As Integer = 10
                Public Const MEASURE_RANGE_FULL As Integer = 800
                Public Const MEASURE_RANGE_MIDDLE As Integer = 600
                Public Const MEASURE_RANGE_SMALL As Integer = 400
                Public Const RECEIVED_BINNING_OFF As Integer = 1
                Public Const RECEIVED_BINNING_ON As Integer = 2
                Public Const COMPRESS_X_OFF As Integer = 1
                Public Const COMPRESS_X_2 As Integer = 2
                Public Const COMPRESS_X_4 As Integer = 4
                Public Const DEFAULT_PROFILE_FILE_NAME As String = "ReceiveData_VBNET.txt"
                Public Const PROFILE_UNIT_MM As Double = 0.00001
            End Class

            ''' <summary>
            ''' Data export class
            ''' </summary>
            Public NotInheritable Class DataExporter

                Private Sub New()
                End Sub

                ''' <summary>
                ''' Profile output
                ''' </summary>
                ''' <param name="datas">Profile data</param>
                ''' <param name="profileNo">Profile information</param>
                ''' <param name="fileName">File name</param>
                ''' <returns></returns>
                Public Shared Function ExportOneProfile(datas As ProfileData(), profileNo As Integer, fileName As String) As Boolean
                    Try
                        Dim unicode As Encoding = System.Text.Encoding.GetEncoding("utf-16")
                        Using sw As New StreamWriter(fileName, False, unicode)
                            Try
                                If datas(0) Is Nothing Then
                                    Return False
                                End If
                                sw.WriteLine(datas(profileNo).ToString())
                            Finally
                                sw.Close()
                            End Try
                        End Using
                    Catch ex As Exception
                        ' File save failure
                        System.Diagnostics.Debug.WriteLine(ex.Message)
                        System.Diagnostics.Debug.Assert(False)
                        Return False
                    End Try
                    Return True
                End Function

                ''' <summary>
                ''' Measurement value output
                ''' </summary>
                ''' <param name="datas">Measurement data</param>
                ''' <param name="fileName">File name</param>
                ''' <returns></returns>
                Public Shared Function ExportMeasureData(datas As MeasureData(), fileName As String) As Boolean
                    Try
                        Dim unicode As Encoding = System.Text.Encoding.GetEncoding("utf-16")
                        Using sw As New StreamWriter(fileName, False, unicode)
                            For i As Integer = 0 To datas.Length - 1
                                sw.WriteLine(datas(i).ToString())
                            Next
                        End Using
                    Catch ex As Exception
                        System.Diagnostics.Debug.WriteLine(ex.Message)
                        System.Diagnostics.Debug.Assert(False)
                        Return False
                    End Try
                    Return True
                End Function
            End Class
        End Class

        Class POINT_LASER

            Public Enum RC
                RC_OK = &H0
                RC_NAK_COMMAND = &H1001
                RC_NAK_COMMAND_LENGTH = &H1002
                RC_NAK_TIMEOUT = &H1003
                RC_NAK_CHECKSUM = &H1004
                RC_NAK_INVALID_STATE = &H1005
                RC_NAK_OTHER = &H1006
                RC_NAK_PARAMETER = &H1007
                RC_NAK_OUT_STAGE = &H1008
                RC_NAK_OUT_HEAD_NUM = &H1009
                RC_NAK_OUT_INVALID_CALC = &H100A
                RC_NAK_OUT_VOID = &H100B
                RC_NAK_INVALID_CYCLE = &H100C
                RC_NAK_CTRL_ERROR = &H100D
                RC_NAK_SRAM_ERROR = &H100E
                RC_ERR_OPEN_DEVICE = &H2000
                RC_ERR_NO_DEVICE = &H2001
                RC_ERR_SEND = &H2002
                RC_ERR_RECEIVE = &H2003
                RC_ERR_TIMEOUT = &H2004
                RC_ERR_NODATA = &H2005
                RC_ERR_NOMEMORY = &H2006
                RC_ERR_DISCONNECT = &H2007
                RC_ERR_UNKNOWN = &H2008
            End Enum

            Public Enum LKIF_FLOATRESULT
                LKIF_FLOATRESULT_VALID
                LKIF_FLOATRESULT_RANGEOVER_P
                LKIF_FLOATRESULT_RANGEOVER_N
                LKIF_FLOATRESULT_WAITING
                LKIF_FLOATRESULT_ALARM
                LKIF_FLOATRESULT_INVALID
            End Enum

            Public Enum LKIF_ABLEMODE
                LKIF_ABLEMODE_AUTO ' AUTO
                LKIF_ABLEMODE_MANUAL ' manual
            End Enum

            Public Enum LKIF_MEASUREMODE
                LKIF_MEASUREMODE_NORMAL ' normal
                LKIF_MEASUREMODE_HALF_T ' translucent object
                LKIF_MEASUREMDOE_TRAN_1 ' transparent object
                LKIF_MEASUREMODE_TRAN_2 ' transparent object 2
                LKIF_MEASUREMODE_MRS ' multireflective object
                LKIF_MEASUREMODE_OPAQUE = LKIF_MEASUREMODE.LKIF_MEASUREMODE_MRS ' Semi opaque object
            End Enum

            Public Enum LKIF_BASICPOINT
                LKIF_BASICPOINT_NEAR ' NEAR
                LKIF_BASICPOINT_FAR ' FAR
            End Enum

            Public Enum LKIF_REFLECTIONMODE
                LKIF_REFLECTIONMODE_DIFFUSION ' diffuse reflection
                LKIF_REFLECTIONMODE_MIRROR ' specular reflection
            End Enum

            Public Enum LKIF_MEDIAN
                LKIF_MEDIAN_OFF ' OFF
                LKIF_MEDIAN_7 ' 7 point
                LKIF_MEDIAN_15 ' 15 point
                LKIF_MEDIAN_31 ' 31 point
            End Enum

            Public Enum LKIF_LASER_CTRL_GROUP
                LKIF_LASER_CTRL_GROUP_1 ' LASER CTRL 1
                LKIF_LASER_CTRL_GROUP_2 ' LASER CTRL 2
            End Enum

            Public Enum LKIF_RANGE
                LKIF_RANGE_CENTER ' CENTER
                LKIF_RANGE_FAR ' FAR
            End Enum

            Public Enum LKIF_MUTUAL_INTERFERENCE_PREVENTION_GROUP
                LKIF_MUTUAL_INTERFERENCE_PREVENTION_GROUP_A ' Group A
                LKIF_MUTUAL_INTERFERENCE_PREVENTION_GROUP_B ' Group B
                LKIF_MUTUAL_INTERFERENCE_PREVENTION_GROUP_C ' Group C
            End Enum

            Public Enum LKIF_CALCMETHOD
                LKIF_CALCMETHOD_HEADA ' head A
                LKIF_CALCMETHOD_HEADB ' head B
                LKIF_CALCMETHOD_HEAD_HEADA_PLUS_HEADB ' head A+head B
                LKIF_CALCMETHOD_HEAD_HEADA_MINUS_HEADB ' head A-head B
                LKIF_CALCMETHOD_HEAD_HEADA_TRANSPARENT ' head A transparent object
                LKIF_CALCMETHOD_HEAD_HEADB_TRANSPARENT ' head B transparent object
                LKIF_CALCMETHOD_HEAD = 0 ' head
                LKIF_CALCMETHOD_OUT ' OUT
                LKIF_CALCMETHOD_ADD ' ADD
                LKIF_CALCMETHOD_SUB ' SUB
                LKIF_CALCMETHOD_AVE ' AVE
                LKIF_CALCMETHOD_PP ' P-P
                LKIF_CALCMETHOD_MAX ' MAX
                LKIF_CALCMETHOD_MIN ' MIN
            End Enum

            Public Enum LKIF_CALCTARGET
                LKIF_CALCTARGET_PEAK_1 ' peak 1
                LKIF_CALCTARGET_PEAK_2 ' peak 2
                LKIF_CALCTARGET_PEAK_3 ' peak 3
                LKIF_CALCTARGET_PEAK_4 ' peak 4
                LKIF_CALCTARGET_PEAK_1_2 ' peak 1-peak 2
                LKIF_CALCTARGET_PEAK_1_3 ' peak 1-peak 3
                LKIF_CALCTARGET_PEAK_1_4 ' peak 1-peak 4
                LKIF_CALCTARGET_PEAK_2_3 ' peak 2-peak 3
                LKIF_CALCTARGET_PEAK_2_4 ' peak 2-peak 4
                LKIF_CALCTARGET_PEAK_3_4 ' peak 3-peak 4
            End Enum

            Public Enum LKIF_FILTERMODE
                LKIF_FILTERMODE_MOVING_AVERAGE ' moving average
                LKIF_FILTERMODE_LOWPASS ' low pass filter
                LKIF_FILTERMODE_HIGHPASS ' high pass filter
            End Enum

            Public Enum LKIF_FILTERPARA
                LKIF_FILTERPARA_AVE_1 = 0 ' 1 time
                LKIF_FILTERPARA_AVE_4 ' 4 times
                LKIF_FILTERPARA_AVE_16 ' 16 times
                LKIF_FILTERPARA_AVE_64 ' 64 times
                LKIF_FILTERPARA_AVE_256 ' 256 times
                LKIF_FILTERPARA_AVE_1024 ' 1024 times
                LKIF_FILTERPARA_AVE_4096 ' 4096 times
                LKIF_FILTERPARA_AVE_16384 ' 16384 times
                LKIF_FILTERPARA_AVE_65536 ' 65536 times
                LKIF_FILTERPARA_AVE_262144 ' 262144 times
                LKIF_FILTERPARA_COFF_3000 = 0 ' 3000Hz
                LKIF_FILTERPARA_COFF_1000 ' 1000Hz
                LKIF_FILTERPARA_COFF_300 ' 300Hz
                LKIF_FILTERPARA_COFF_100 ' 100Hz
                LKIF_FILTERPARA_COFF_30 ' 30Hz
                LKIF_FILTERPARA_COFF_10 ' 10Hz
                LKIF_FILTERPARA_COFF_3 ' 3Hz
                LKIF_FILTERPARA_COFF_1 ' 1Hz
                LKIF_FILTERPARA_COFF_0_3 ' 0.3Hz
                LKIF_FILTERPARA_COFF_0_1 ' 0.1Hz
            End Enum

            Public Enum LKIF_AVERAGE
                LKIF_AVERAGE_1 ' 1 time
                LKIF_AVERAGE_4 ' 4 times
                LKIF_AVERAGE_16 ' 16 times
                LKIF_AVERAGE_64 ' 64 times
                LKIF_AVERAGE_256 ' 256 times
                LKIF_AVERAGE_1024 ' 1024 times
                LKIF_AVERAGE_4096 ' 4096 times
                LKIF_AVERAGE_16384 ' 16384 times
                LKIF_AVERAGE_65536 ' 65536 times
                LKIF_AVERAGE_262144 ' 262144 times
            End Enum

            Public Enum LKIF_CUTOFFFREQUENCY
                LKIF_CUTOFFFREQUENCY_3000 ' 3000Hz
                LKIF_CUTOFFFREQUENCY_1000 ' 1000Hz
                LKIF_CUTOFFFREQUENCY_300 ' 300Hz
                LKIF_CUTOFFFREQUENCY_100 ' 100Hz
                LKIF_CUTOFFFREQUENCY_30 ' 30Hz
                LKIF_CUTOFFFREQUENCY_10 ' 10Hz
                LKIF_CUTOFFFREQUENCY_3 ' 3Hz
                LKIF_CUTOFFFREQUENCY_1 ' 1Hz
                LKIF_CUTOFFFREQUENCY_0_3 ' 0.3Hz
                LKIF_CUTOFFFREQUENCY_0_1 ' 0.1Hz
            End Enum

            Public Enum LKIF_TRIGGERMODE
                LKIF_TRIGGERMODE_EXT1 ' external trigger 1
                LKIF_TRIGGERMODE_EXT2 ' external trigger 2
            End Enum

            Public Enum LKIF_CALCMODE
                LKIF_CALCMODE_NORMAL ' normal
                LKIF_CALCMODE_PEAKHOLD ' peak hold
                LKIF_CALCMODE_BOTTOMHOLD ' bottom hold
                LKIF_CALCMODE_PEAKTOPEAKHOLD ' peak-to-peak hold
                LKIF_CALCMODE_SAMPLEHOLD ' sample hold
                LKIF_CALCMODE_AVERAGEHOLD ' average hold
            End Enum

            Public Enum LKIF_DISPLAYUNIT
                LKIF_DISPLAYUNIT_0000_01MM = 0 ' 0.01mm
                LKIF_DISPLAYUNIT_000_001MM ' 0.001mm
                LKIF_DISPLAYUNIT_00_0001MM ' 0.0001mm
                LKIF_DISPLAYUNIT_0_00001MM ' 0.00001mm
                LKIF_DISPLAYUNIT_00000_1UM ' 0.1um
                LKIF_DISPLAYUNIT_0000_01UM ' 0.01um
                LKIF_DISPLAYUNIT_000_001UM ' 0.001um
                LKIF_DISPLAYUNIT_00000_1M_S = 0 ' 0.1m/s
                LKIF_DISPLAYUNIT_0000_01M_S ' 0.01m/s
                LKIF_DISPLAYUNIT_000_001M_S ' 0.001m/s
                LKIF_DISPLAYUNIT_00000_1MM_S ' 0.1mm/s
                LKIF_DISPLAYUNIT_0000_01MM_S ' 0.01mm/s
                LKIF_DISPLAYUNIT_000_001MM_S ' 0.001mm/s
                LKIF_DISPLAYUNIT_00_0001MM_S ' 0.0001mm/s
                LKIF_DISPLAYUNIT_00000_1KM_S2 = 0 ' 0.1km/s2
                LKIF_DISPLAYUNIT_0000_01KM_S2 ' 0.01km/s2
                LKIF_DISPLAYUNIT_000_001KM_S2 ' 0.001km/s2
                LKIF_DISPLAYUNIT_00000_1M_S2 ' 0.1m/s2
                LKIF_DISPLAYUNIT_0000_01M_S2 ' 0.01m/s2
                LKIF_DISPLAYUNIT_000_001M_S2 ' 0.001m/s2
                LKIF_DISPLAYUNIT_00_0001M_S2 ' 0.0001m/s2
            End Enum

            Public Enum LKIF_MEASURETYPE
                LKIF_MEASURETYPE_DISPLACEMENT ' Displacement
                LKIF_MEASURETYPE_SPEED ' Speed
                LKIF_MEASURETYPE_ACCELERATION ' Acceleration
            End Enum

            Public Enum LKIF_OUTNO
                LKIF_OUTNO_01 = &H1 ' OUT01
                LKIF_OUTNO_02 = &H2 ' OUT02
                LKIF_OUTNO_03 = &H4 ' OUT03
                LKIF_OUTNO_04 = &H8 ' OUT04
                LKIF_OUTNO_05 = &H10 ' OUT05
                LKIF_OUTNO_06 = &H20 ' OUT06
                LKIF_OUTNO_07 = &H40 ' OUT07
                LKIF_OUTNO_08 = &H80 ' OUT08
                LKIF_OUTNO_09 = &H100 ' OUT09
                LKIF_OUTNO_10 = &H200 ' OUT10
                LKIF_OUTNO_11 = &H400 ' OUT11
                LKIF_OUTNO_12 = &H800 ' OUT12
                LKIF_OUTNO_ALL = &HFFF ' All OUTs
            End Enum

            Public Enum LKIF_SYNCHRONIZATION
                LKIF_SYNCHRONIZATION_ASYNCHRONOUS ' asynchronous
                LKIF_SYNCHRONIZATION_SYNCHRONIZED ' synchronous
            End Enum

            Public Enum LKIF_TARGETOUT
                LKIF_TARGETOUT_NONE ' no target OUT
                LKIF_TARGETOUT_OUT1 ' OUT1
                LKIF_TARGETOUT_OUT2 ' OUT2
                LKIF_TARGETOUT_BOTH ' OUT 1/2
            End Enum

            Public Enum LKIF_STORAGECYCLE
                LKIF_STORAGECYCLE_1 ' sampling rate x 1
                LKIF_STORAGECYCLE_2 ' sampling rate x 2
                LKIF_STORAGECYCLE_5 ' sampling rate x 5
                LKIF_STORAGECYCLE_10 ' sampling rate x 10
                LKIF_STORAGECYCLE_20 ' sampling rate x 20
                LKIF_STORAGECYCLE_50 ' sampling rate x 50
                LKIF_STORAGECYCLE_100 ' sampling rate x 100
                LKIF_STORAGECYCLE_200 ' sampling rate x 200
                LKIF_STORAGECYCLE_500 ' sampling rate x 500
                LKIF_STORAGECYCLE_1000 ' sampling rate x 1000
                LKIF_STORAGECYCLE_TIMING ' Timing sync
            End Enum

            Public Enum LKIF_SAMPLINGCYCLE
                LKIF_SAMPLINGCYCLE_2_55USEC ' 2.55us
                LKIF_SAMPLINGCYCLE_5USEC ' 5us
                LKIF_SAMPLINGCYCLE_10USEC ' 10us
                LKIF_SAMPLINGCYCLE_20USEC ' 20us
                LKIF_SAMPLINGCYCLE_50USEC ' 50us
                LKIF_SAMPLINGCYCLE_100USEC ' 100us
                LKIF_SAMPLINGCYCLE_200USEC ' 200us
                LKIF_SAMPLINGCYCLE_500USEC ' 500us
                LKIF_SAMPLINGCYCLE_1MSEC ' 1ms
            End Enum

            Public Enum LKIF_MUTUAL_INTERFERENCE_PREVENTION
                LKIF_MUTUAL_INTERFERENCE_PREVENTION_OFF
                LKIF_MUTUAL_INTERFERENCE_PREVENTION_AB_ON
                LKIF_MUTUAL_INTERFERENCE_PREVENTION_ABC_ON
            End Enum

            Public Enum LKIF_TOLERANCE_COMPARATOR_OUTPUT_FORMAT
                LKIF_TOLERANCE_COMPARATOR_OUTPUT_FORMAT_NORMAL
                LKIF_TOLERANCE_COMPARATOR_OUTPUT_FORMAT_HOLD
                LKIF_TOLERANCE_COMPARATOR_OUTPUT_FORMAT_OFF_DELAY
            End Enum

            Public Enum LKIF_STOROBETIME
                LKIF_STOROBETIME_2MS
                LKIF_STOROBETIME_5MS
                LKIF_STOROBETIME_10MS
                LKIF_STOROBETIME_20MS
            End Enum

            Public Enum LKIF_ALARM_OUTPUT_FORM
                LKIF_ALARM_OUTPUT_FORM_SYSTEM
                LKIF_ALARM_OUTPUT_FORM_MEASURE
                LKIF_ALARM_OUTPUT_FORM_BOTH
            End Enum

            Public Enum LKIF_MODE
                LKIF_MODE_NORMAL
                LKIF_MODE_COMMUNICATION
            End Enum

            Public Structure LKIF_FLOATVALUE_OUT
                Dim outNo As Integer
                Dim FloatResult As LKIF_FLOATRESULT
                Dim value As Single
            End Structure
            Public Structure LKIF_FLOATVALUE
                Dim FloatResult As LKIF_FLOATRESULT
                Dim value As Single
            End Structure

            Structure IN_ADDR
                Dim S_addr As Integer
            End Structure

            Structure LKIF_OPENPARAM_ETHERNET
                Dim IPAddress As IN_ADDR
            End Structure

            ''' <summary>
            ''' 打开激光控制器
            ''' </summary>
            ''' <param name="DeviceIp"></param>
            ''' <param name="SubName"></param>
            ''' <param name="ErrLine"></param>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Function EthernetOpen(ByVal DeviceIp As String, Optional ByRef SubName As String = Nothing, Optional ByRef ErrLine As String = Nothing) As Integer
                Dim ret As Integer = Nothing
                Try
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    Dim OpenParam As LKIF_OPENPARAM_ETHERNET
                    OpenParam.IPAddress.S_addr = inet_addr(DeviceIp)
                    ret = LKIF2_OpenDeviceETHER(OpenParam)
                Catch ex As Exception
                   
                 
                    ret = -1
                End Try
                Return ret
            End Function

            ''' <summary>
            ''' 打开激光控制器
            ''' </summary>
            ''' <param name="SubName"></param>
            ''' <param name="ErrLine"></param>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Function UsbOpen(Optional ByRef SubName As String = Nothing, Optional ByRef ErrLine As String = Nothing) As Integer
                Dim ret As Integer = Nothing
                Try
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    ret = LKIF2_OpenDeviceUsb()
                Catch ex As Exception
                   
                 
                    ret = -1
                End Try
                Return ret
            End Function

            Function SetProgramNo(ByVal programNo As Long, Optional ByRef SubName As String = Nothing, Optional ByRef ErrLine As String = Nothing) As Integer
                Dim ret As Integer = Nothing
                Try
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    ret = LKIF2_SetProgramNo(programNo)
                Catch ex As Exception
                   
                 
                    ret = -1
                End Try
                Return ret
            End Function


            Function GetCalcDataSingle(ByVal outNo As Long, ByRef Measure_Data As Double, Optional ByRef SubName As String = Nothing, Optional ByRef ErrLine As String = Nothing) As Integer
                Dim Ret As Integer = Nothing
                Try
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    Dim calcData As LKIF_FLOATVALUE_OUT
                    Ret = LKIF2_GetCalcDataSingle(outNo, calcData)
                    If calcData.FloatResult = LKIF_FLOATRESULT.LKIF_FLOATRESULT_VALID Then
                        Measure_Data = calcData.value
                    Else
                        Measure_Data = 999.999
                    End If
                Catch ex As Exception
                   
                 
                    Ret = -1
                End Try
                Return Ret
            End Function

            Function GetCalcDataMulti(ByVal outNo As LKIF_OUTNO, ByRef calcData As LKIF_FLOATVALUE_OUT, Optional ByRef SubName As String = Nothing, Optional ByRef ErrLine As String = Nothing) As Integer
                Dim Ret As Integer = Nothing
                Try
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    Ret = LKIF2_GetCalcDataMulti(outNo, calcData)
                Catch ex As Exception
                   
                 
                    Ret = -1
                End Try
                Return Ret
            End Function

            ''' <summary>
            ''' 开始存储数据
            ''' </summary>
            ''' <param name="SubName"></param>
            ''' <param name="ErrLine"></param>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Function DataStorageStart(Optional ByRef SubName As String = Nothing, Optional ByRef ErrLine As String = Nothing) As Integer
                Dim Ret As Integer = Nothing
                Try
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    Ret = LKIF2_DataStorageInit()
                    If Ret <> RC.RC_OK Then
                        Return Ret
                    End If
                    Ret = LKIF2_DataStorageStart()
                Catch ex As Exception
                   
                 
                    Ret = -1
                End Try
                Return Ret
            End Function

            ''' <summary>
            ''' 停止存储数据
            ''' </summary>
            ''' <param name="SubName"></param>
            ''' <param name="ErrLine"></param>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Function DataStorageStop(Optional ByRef SubName As String = Nothing, Optional ByRef ErrLine As String = Nothing) As Integer
                Dim Ret As Integer = Nothing
                Try
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    Ret = LKIF2_DataStorageStop
                Catch ex As Exception
                   
                 
                    Ret = -1
                End Try
                Return Ret
            End Function

            ''' <summary>
            ''' 读取批量存储数据
            ''' </summary>
            ''' <param name="OutNo"></param>
            ''' <param name="Measure_Data"></param>
            ''' <param name="SubName"></param>
            ''' <param name="ErrLine"></param>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Function DataStorageGetData(ByVal OutNo As Integer, ByRef Measure_Data() As Double, Optional ByRef SubName As String = Nothing, Optional ByRef ErrLine As String = Nothing) As Integer
                Dim Ret As Integer = Nothing
                Try
                    SubName = ErrName & GetCurrentMethod.Name & ":"
                    Dim isStorage, numStorageData, numReceived As Integer
                    Ret = LKIF2_DataStorageGetStatus(isStorage, numStorageData)
                    If Ret <> RC.RC_OK Then
                        Return Ret
                    End If
                    Dim OutBuffer(numStorageData - 1) As LKIF_FLOATVALUE
                    Ret = LKIF2_DataStorageGetData(OutNo, numStorageData, OutBuffer(0), numReceived)
                    If Ret = RC.RC_OK Then
                        Array.Resize(Measure_Data, OutBuffer.Length)
                        For i As Integer = 0 To OutBuffer.Length - 1
                            If OutBuffer(i).FloatResult = LKIF_FLOATRESULT.LKIF_FLOATRESULT_VALID Then
                                Measure_Data(i) = OutBuffer(i).value
                            Else
                                Measure_Data(i) = 999.999
                            End If
                        Next
                    End If
                Catch ex As Exception
                   
                 
                    Ret = -1
                End Try
                Return Ret
            End Function

            Private Declare Function inet_addr Lib "wsock32.dll" (ByVal cp As String) As Integer
            'Measured value output (single)
            Public Declare Function LKIF2_GetCalcDataSingle Lib "LKIF2.dll" (ByVal outNo As Integer, ByRef calcData As LKIF_FLOATVALUE_OUT) As RC
            'Measured value output (multiple)
            Public Declare Function LKIF2_GetCalcDataMulti Lib "LKIF2.dll" (ByVal outNo As LKIF_OUTNO, ByRef calcData As LKIF_FLOATVALUE_OUT) As RC
            'Measured value output (ALL)
            Public Declare Function LKIF2_GetCalcDataALL Lib "LKIF2.dll" (ByRef outNo As Integer, ByRef calcData As LKIF_FLOATVALUE_OUT) As RC
            'Timing ON/OFF (single)
            Public Declare Function LKIF2_SetTimingSingle Lib "LKIF2.dll" (ByVal outNo As Integer, ByVal onOff As Integer) As RC
            'Timing ON/OFF (multiple)
            Public Declare Function LKIF2_SetTimingMulti Lib "LKIF2.dll" (ByVal outNo As LKIF_OUTNO, ByVal onOff As Integer) As RC
            'Timing ON/OFF (synchronous)
            Public Declare Function LKIF2_SetTimingSync Lib "LKIF2.dll" (ByVal onOff As Integer) As RC
            'Auto-zero ON/OFF (single)
            Public Declare Function LKIF2_SetZeroSingle Lib "LKIF2.dll" (ByVal outNo As Integer, ByVal onOff As Integer) As RC
            'Auto-zero ON/OFF (multiple)
            Public Declare Function LKIF2_SetZeroMulti Lib "LKIF2.dll" (ByVal outNo As LKIF_OUTNO, ByVal onOff As Integer) As RC
            'Auto-zero ON/OFF (synchronous)
            Public Declare Function LKIF2_SetZeroSync Lib "LKIF2.dll" (ByVal onOff As Integer) As RC
            'Measured value reset (single)
            Public Declare Function LKIF2_SetResetSingle Lib "LKIF2.dll" (ByVal outNo As Integer) As RC
            'Measured value reset (multiple)
            Public Declare Function LKIF2_SetResetMulti Lib "LKIF2.dll" (ByVal outNo As LKIF_OUTNO) As RC
            'Measured value reset (synchronous)
            Public Declare Function LKIF2_SetResetSync Lib "LKIF2.dll" () As RC
            'Panel lock
            Public Declare Function LKIF2_SetPanelLock Lib "LKIF2.dll" (ByVal onOff As Integer) As RC
            'Program Change
            Public Declare Function LKIF2_SetProgramNo Lib "LKIF2.dll" (ByVal programNo As Integer) As RC
            'Program Check
            Public Declare Function LKIF2_GetProgramNo Lib "LKIF2.dll" (ByRef programNo As Integer) As RC
            'Starting the Data Storage
            Public Declare Function LKIF2_DataStorageStart Lib "LKIF2.dll" () As RC
            'Stopping the Data Storage
            Public Declare Function LKIF2_DataStorageStop Lib "LKIF2.dll" () As RC
            'Outputting the Data Storage
            Public Declare Function LKIF2_DataStorageGetData Lib "LKIF2.dll" (ByVal outNo As Integer, ByVal numOutBuffer As Integer, ByRef OutBuffer As LKIF_FLOATVALUE, ByRef numReceived As Integer) As RC
            'Initializing the Data Storage
            Public Declare Function LKIF2_DataStorageInit Lib "LKIF2.dll" () As RC
            'Data Storage Accumulation Status Output
            Public Declare Function LKIF2_DataStorageGetStatus Lib "LKIF2.dll" (ByRef isStorage As Integer, ByRef numStorageData As Integer) As RC
            'Receive Light Waveform
            Public Declare Function LKIF2_GetLight Lib "LKIF2.dll" (ByVal headNo As Integer, ByVal peekNo As Integer, ByRef measurePosition As Integer, ByRef waveData As Byte) As RC
            'Display Panel Switch
            Public Declare Function LKIF2_SetPanel Lib "LKIF2.dll" (ByVal upperDisp As Integer, ByVal lowerDisp As Integer) As RC
            'Set Tolerance
            Public Declare Function LKIF2_SetTolerance Lib "LKIF2.dll" (ByVal outNo As Integer, ByVal upperLimit As Integer, ByVal lowerLimit As Integer, ByVal hysteresis As Integer) As RC
            'Set ABLE
            Public Declare Function LKIF2_SetAbleMode Lib "LKIF2.dll" (ByVal headNo As Integer, ByVal ableMode As LKIF_ABLEMODE) As RC
            'Set ABLE Control Range
            Public Declare Function LKIF2_SetAbleMinMax Lib "LKIF2.dll" (ByVal headNo As Integer, ByVal min As Integer, ByVal max As Integer) As RC
            'Set Detection mode
            Public Declare Function LKIF2_SetMeasureMode Lib "LKIF2.dll" (ByVal headNo As Integer, ByVal measureMode As LKIF_MEASUREMODE) As RC
            'Set Base point
            Public Declare Function LKIF2_SetBasicPoint Lib "LKIF2.dll" (ByVal headNo As Integer, ByVal basicPoint As LKIF_BASICPOINT) As RC
            'Set Number of Times of Alarm Processing
            Public Declare Function LKIF2_SetNumAlarm Lib "LKIF2.dll" (ByVal headNo As Integer, ByVal numAlarm As Integer) As RC
            'Set Number of Times of Alarm Recovery
            Public Declare Function LKIF2_SetNumRecovery Lib "LKIF2.dll" (ByVal headNo As Integer, ByVal recoveryNum As Integer) As RC
            'Set Alarm Level
            Public Declare Function LKIF2_SetAlarmLevel Lib "LKIF2.dll" (ByVal headNo As Integer, ByVal alaramLevel As Integer) As RC
            'Starting the ABLE Calibration
            Public Declare Function LKIF2_AbleStart Lib "LKIF2.dll" (ByVal headNo As Integer) As RC
            'Finishing the ABLE Calibration
            Public Declare Function LKIF2_AbleStop Lib "LKIF2.dll" () As RC
            'Stopping the ABLE Calibration
            Public Declare Function LKIF2_AbleCancel Lib "LKIF2.dll" () As RC
            'Set Mounting Mode
            Public Declare Function LKIF2_SetReflectionMode Lib "LKIF2.dll" (ByVal headNo As Integer, ByVal reflectionMode As LKIF_REFLECTIONMODE) As RC
            'Set Mask
            Public Declare Function LKIF2_SetMask Lib "LKIF2.dll" (ByVal headNo As Integer, ByVal onOff As Integer, ByVal pos1 As Integer, ByVal pos2 As Integer) As RC
            'Set Median
            Public Declare Function LKIF2_SetMedian Lib "LKIF2.dll" (ByVal headNo As Integer, ByVal median As LKIF_MEDIAN) As RC
            'Set LASER CTRL group
            Public Declare Function LKIF2_SetLaserCtrlGroup Lib "LKIF2.dll" (ByVal headNo As Integer, ByVal laserCtrlGroup As LKIF_LASER_CTRL_GROUP) As RC
            'Set Range
            Public Declare Function LKIF2_SetRange Lib "LKIF2.dll" (ByVal headNo As Integer, ByVal range As LKIF_RANGE) As RC
            'Set Mutual interference prevention group
            Public Declare Function LKIF2_SetMutualInterferencePreventionGroup Lib "LKIF2.dll" (ByVal headNo As Integer, ByVal group As LKIF_MUTUAL_INTERFERENCE_PREVENTION_GROUP) As RC
            'Set Calculation method
            Public Declare Function LKIF2_SetCalcMethod Lib "LKIF2.dll" (ByVal outNo As Integer, ByVal calcMethod As LKIF_CALCMETHOD, ByVal head_Out_No As Integer) As RC
            'Set Surface to be measured
            Public Declare Function LKIF2_SetCalcTarget Lib "LKIF2.dll" (ByVal outNo As Integer, ByVal calcTarget As LKIF_CALCTARGET) As RC
            'Set OUT to be calculated (ADD, SUB)
            Public Declare Function LKIF2_SetOutAddSub Lib "LKIF2.dll" (ByVal outNo As Integer, ByVal addSub1 As Integer, ByVal addSub2 As Integer) As RC
            'Set OUT to be calculated (AVE, MAX, MIN, P-P)
            Public Declare Function LKIF2_SetOutOperation Lib "LKIF2.dll" (ByVal outNo As Integer, ByVal targetOut As LKIF_OUTNO) As RC
            'Set Scaling
            Public Declare Function LKIF2_SetScaling Lib "LKIF2.dll" (ByVal outNo As Integer, ByVal inputValue1 As Integer, ByVal outputValue1 As Integer, ByVal inputValue2 As Integer, ByVal outputValue2 As Integer) As RC
            'Set Filter Mode
            Public Declare Function LKIF2_SetFilter Lib "LKIF2.dll" (ByVal outNo As Integer, ByVal filterMode As LKIF_FILTERMODE, ByVal filterPara As LKIF_FILTERPARA) As RC
            'Set Trigger Mode
            Public Declare Function LKIF2_SetTriggerMode Lib "LKIF2.dll" (ByVal outNo As Integer, ByVal triggerMode As LKIF_TRIGGERMODE) As RC
            'Set Offset
            Public Declare Function LKIF2_SetOffset Lib "LKIF2.dll" (ByVal outNo As Integer, ByVal offset As Integer) As RC
            'Set Calculation Mode
            Public Declare Function LKIF2_SetCalcMode Lib "LKIF2.dll" (ByVal outNo As Integer, ByVal CalcMode As LKIF_CALCMODE) As RC
            'Set Analog Output Scaling
            Public Declare Function LKIF2_SetAnalogScaling Lib "LKIF2.dll" (ByVal outNo As Integer, ByVal inputValue1 As Integer, ByVal outputVoltage1 As Integer, ByVal inputValue2 As Integer, ByVal outputVoltage2 As Integer) As RC
            'Set Minimum Display Unit
            Public Declare Function LKIF2_SetDisplayUnit Lib "LKIF2.dll" (ByVal outNo As Integer, ByVal displayUnit As LKIF_DISPLAYUNIT) As RC
            'Set Measurement type
            Public Declare Function LKIF2_SetMeasureType Lib "LKIF2.dll" (ByVal outNo As Integer, ByVal measureType As LKIF_MEASURETYPE) As RC
            'Set Synchronization
            Public Declare Function LKIF2_SetSynchronization Lib "LKIF2.dll" (ByVal outNo As Integer, ByVal onOff As Integer) As RC
            'Set Storage (OUT No. specification)
            Public Declare Function LKIF2_SetStorageTarget Lib "LKIF2.dll" (ByVal outNo As Integer, ByVal onOff As Integer) As RC
            'Set Sampling Rate
            Public Declare Function LKIF2_SetSamplingCycle Lib "LKIF2.dll" (ByVal samplingCycle As LKIF_SAMPLINGCYCLE) As RC
            'Set Mutual Interference Prevention
            Public Declare Function LKIF2_SetMutualInterferencePrevention Lib "LKIF2.dll" (ByVal onOff As LKIF_MUTUAL_INTERFERENCE_PREVENTION) As RC
            'Set Comparator Output Format
            Public Declare Function LKIF2_SetToleranceComparatorOutputFormat Lib "LKIF2.dll" (ByVal toleranceComparatorOutputFormat As LKIF_TOLERANCE_COMPARATOR_OUTPUT_FORMAT) As RC
            'Set Strobe Time
            Public Declare Function LKIF2_SetStrobeTime Lib "LKIF2.dll" (ByVal storobeTime As LKIF_STOROBETIME) As RC
            'Set Data Storage
            Public Declare Function LKIF2_SetDataStorage Lib "LKIF2.dll" (ByVal storageNum As Integer, ByVal storageCycle As LKIF_STORAGECYCLE) As RC
            'Set Analog output channel
            Public Declare Function LKIF2_SetAnalogChannel Lib "LKIF2.dll" (ByVal chNo As Integer, ByVal outNo As Integer) As RC
            'Set Alarm output type
            Public Declare Function LKIF2_SetAlarmOutputForm Lib "LKIF2.dll" (ByVal alarmOutputForm As LKIF_ALARM_OUTPUT_FORM) As RC
            'Set Active head count
            Public Declare Function LKIF2_SetNumOfUsedHeads Lib "LKIF2.dll" (ByVal numOfUsedHeads As Integer) As RC
            'Set Active OUT count
            Public Declare Function LKIF2_SetNumOfUsedOut Lib "LKIF2.dll" (ByVal numOfUsedOut As Integer) As RC
            'Set Active analog output channel count
            Public Declare Function LKIF2_SetNumOfUsedAnalogCh Lib "LKIF2.dll" (ByVal numOfUsedAnalogCh As Integer) As RC
            'Display Panel Check
            Public Declare Function LKIF2_GetPanel Lib "LKIF2.dll" (ByRef upperDisp As Integer, ByRef lowerDisp As Integer) As RC
            'Get Tolerance
            Public Declare Function LKIF2_GetTolerance Lib "LKIF2.dll" (ByVal outNo As Integer, ByRef upperLimit As Integer, ByRef lowerLimit As Integer, ByRef hysteresis As Integer) As RC
            'Get ABLE
            Public Declare Function LKIF2_GetAbleMode Lib "LKIF2.dll" (ByVal headNo As Integer, ByRef ableMode As LKIF_ABLEMODE) As RC
            'Get ABLE Control Range
            Public Declare Function LKIF2_GetAbleMinMax Lib "LKIF2.dll" (ByVal headNo As Integer, ByRef min As Integer, ByRef max As Integer) As RC
            'Get Detection mode
            Public Declare Function LKIF2_GetMeasureMode Lib "LKIF2.dll" (ByVal headNo As Integer, ByRef measureMode As LKIF_MEASUREMODE) As RC
            'Get Base point
            Public Declare Function LKIF2_GetBasicPoint Lib "LKIF2.dll" (ByVal headNo As Integer, ByRef basicPoint As LKIF_BASICPOINT) As RC
            'Get Number of Times of Alarm Processing
            Public Declare Function LKIF2_GetNumAlarm Lib "LKIF2.dll" (ByVal headNo As Integer, ByRef numAlarm As Integer) As RC
            'Get Number of Times of Alarm Recovery
            Public Declare Function LKIF2_GetNumRecovery Lib "LKIF2.dll" (ByVal headNo As Integer, ByRef recoveryNum As Integer) As RC
            'Get Alarm Level
            Public Declare Function LKIF2_GetAlarmLevel Lib "LKIF2.dll" (ByVal headNo As Integer, ByRef alaramLevel As Integer) As RC
            'Get Mounting Mode
            Public Declare Function LKIF2_GetReflectionMode Lib "LKIF2.dll" (ByVal headNo As Integer, ByRef reflectionMode As LKIF_REFLECTIONMODE) As RC
            'Get Mask
            Public Declare Function LKIF2_GetMask Lib "LKIF2.dll" (ByVal headNo As Integer, ByRef onOff As Integer, ByRef pos1 As Integer, ByRef pos2 As Integer) As RC
            'Get Median
            Public Declare Function LKIF2_GetMedian Lib "LKIF2.dll" (ByVal headNo As Integer, ByRef median As LKIF_MEDIAN) As RC
            'Get LASER CTRL group
            Public Declare Function LKIF2_GetLaserCtrlGroup Lib "LKIF2.dll" (ByVal headNo As Integer, ByRef laserCtrlGroup As LKIF_LASER_CTRL_GROUP) As RC
            'Get Range
            Public Declare Function LKIF2_GetRange Lib "LKIF2.dll" (ByVal headNo As Integer, ByRef range As LKIF_RANGE) As RC
            'Get Mutual interference prevention group
            Public Declare Function LKIF2_GetMutualInterferencePreventionGroup Lib "LKIF2.dll" (ByVal headNo As Integer, ByRef group As LKIF_MUTUAL_INTERFERENCE_PREVENTION_GROUP) As RC
            'Get Calculation method
            Public Declare Function LKIF2_GetCalcMethod Lib "LKIF2.dll" (ByVal outNo As Integer, ByRef calcMethod As LKIF_CALCMETHOD, ByRef head_Out_No As Integer) As RC
            'Get Surface to be measured
            Public Declare Function LKIF2_GetCalcTarget Lib "LKIF2.dll" (ByVal outNo As Integer, ByRef calcTarget As LKIF_CALCTARGET) As RC
            'Get OUT to be calculated (ADD, SUB)
            Public Declare Function LKIF2_GetOutAddSub Lib "LKIF2.dll" (ByVal outNo As Integer, ByRef addSub1 As Integer, ByRef addSub2 As Integer) As RC
            'Get OUT to be calculated (AVE, MAX, MIN, P-P)
            Public Declare Function LKIF2_GetOutOperation Lib "LKIF2.dll" (ByVal outNo As Integer, ByRef targetOut As LKIF_OUTNO) As RC
            'Get Scaling
            Public Declare Function LKIF2_GetScaling Lib "LKIF2.dll" (ByVal outNo As Integer, ByRef inputValue1 As Integer, ByRef outputValue1 As Integer, ByRef inputValue2 As Integer, ByRef outputValue2 As Integer) As RC
            'Get Filter Mode
            Public Declare Function LKIF2_GetFilter Lib "LKIF2.dll" (ByVal outNo As Integer, ByRef filterMode As LKIF_FILTERMODE, ByRef filterPara As LKIF_FILTERPARA) As RC
            'Get Trigger Mode
            Public Declare Function LKIF2_GetTriggerMode Lib "LKIF2.dll" (ByVal outNo As Integer, ByRef triggerMode As LKIF_TRIGGERMODE) As RC
            'Get Offset
            Public Declare Function LKIF2_GetOffset Lib "LKIF2.dll" (ByVal outNo As Integer, ByRef offset As Integer) As RC
            'Get Calculation Mode
            Public Declare Function LKIF2_GetCalcMode Lib "LKIF2.dll" (ByVal outNo As Integer, ByRef CalcMode As LKIF_CALCMODE) As RC
            'Get Analog Output Scaling
            Public Declare Function LKIF2_GetAnalogScaling Lib "LKIF2.dll" (ByVal outNo As Integer, ByRef inputValue1 As Integer, ByRef outputVoltage1 As Integer, ByRef inputValue1 As Integer, ByRef outputVoltage2 As Integer) As RC
            'Get Minimum Display Unit
            Public Declare Function LKIF2_GetDisplayUnit Lib "LKIF2.dll" (ByVal outNo As Integer, ByRef displayUnit As LKIF_DISPLAYUNIT) As RC
            'Get Measurement type
            Public Declare Function LKIF2_GetMeasureType Lib "LKIF2.dll" (ByVal outNo As Integer, ByRef measureType As LKIF_MEASURETYPE) As RC
            'Get Synchronization
            Public Declare Function LKIF2_GetSynchronization Lib "LKIF2.dll" (ByVal outNo As Integer, ByRef onOff As Integer) As RC
            'Get Storage (OUT No. specification)
            Public Declare Function LKIF2_GetStorageTarget Lib "LKIF2.dll" (ByVal outNo As Integer, ByRef onOff As Integer) As RC
            'Get Sampling Rate
            Public Declare Function LKIF2_GetSamplingCycle Lib "LKIF2.dll" (ByRef samplingCycle As LKIF_SAMPLINGCYCLE) As RC
            'Get Mutual Interference Prevention
            Public Declare Function LKIF2_GetMutualInterferencePrevention Lib "LKIF2.dll" (ByRef onOff As LKIF_MUTUAL_INTERFERENCE_PREVENTION) As RC
            'Get Comparator Output Format
            Public Declare Function LKIF2_GetToleranceComparatorOutputFormat Lib "LKIF2.dll" (ByRef toleranceComparatorOutputFormat As LKIF_TOLERANCE_COMPARATOR_OUTPUT_FORMAT) As RC
            'Get Strobe Time
            Public Declare Function LKIF2_GetStrobeTime Lib "LKIF2.dll" (ByRef storobeTime As LKIF_STOROBETIME) As RC
            'Get Data Storage
            Public Declare Function LKIF2_GetDataStorage Lib "LKIF2.dll" (ByRef storageNum As Integer, ByRef storageCycle As LKIF_STORAGECYCLE) As RC
            'Get Analog output channel
            Public Declare Function LKIF2_GetAnalogChannel Lib "LKIF2.dll" (ByVal chNo As Integer, ByRef outNo As Integer) As RC
            'Get Alarm output type
            Public Declare Function LKIF2_GetAlarmOutputForm Lib "LKIF2.dll" (ByRef alarmOutputForm As LKIF_ALARM_OUTPUT_FORM) As RC
            'Get Active head count
            Public Declare Function LKIF2_GetNumOfUsedHeads Lib "LKIF2.dll" (ByRef numOfUsedHeads As Integer) As RC
            'Get Active OUT count
            Public Declare Function LKIF2_GetNumOfUsedOut Lib "LKIF2.dll" (ByRef numOfUsedOut As Integer) As RC
            'Get Active analog output channel count
            Public Declare Function LKIF2_GetNumOfUsedAnalogCh Lib "LKIF2.dll" (ByRef numOfUsedAnalogCh As Integer) As RC
            'Start Measurement (Mode switch)
            Public Declare Function LKIF2_StartMeasure Lib "LKIF2.dll" () As RC
            'Stop Measurement (Mode switch)
            Public Declare Function LKIF2_StopMeasure Lib "LKIF2.dll" () As RC
            'Opens the USB device.
            Public Declare Function LKIF2_OpenDeviceUsb Lib "LKIF2.dll" () As RC
            'Opens the Ethernet device.
            Public Declare Function LKIF2_OpenDeviceETHER Lib "LKIF2.dll" (ByRef openParam As LKIF_OPENPARAM_ETHERNET) As RC
            'Closes the currently open device.
            Public Declare Function LKIF2_CloseDevice Lib "LKIF2.dll" () As RC

        End Class

    End Class
End Module

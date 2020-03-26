Imports System.Net.Sockets
Imports System.Threading
Imports System.Net
Imports System.Text
Imports System.IO
Imports DevComponents.DotNetBar

Public Class TCP_IP

    Private _ClientSocket_buf() As Byte = Nothing, _ServerSocket_buf() As Byte = Nothing
    Private _ClientSocket As Socket '客户端SOCKET
    Private _ServerIP As String = Nothing '服务器IP地址
    Private _ServerPort As String = Nothing '服务器端口号
    Private Bool_Client_ConnOK As Boolean = False '是否连接成功
    Private Bool_Server_ConnOK As Boolean = False '是否连接成功
    Private _TcpListener As TcpListener '服务器端SOCKET
    Private netstream As NetworkStream
    Private readstream As StreamReader
    Private writestream As StreamWriter
    Private _ConSocket As Socket '服务器端连接用SOCKET

    ''' <summary>
    ''' 客户端连接服务器
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function Client_Conn_Server() As Int16
        Dim str As String = Nothing
        Dim IP As IPAddress = IPAddress.Parse(_ServerIP)
        _ClientSocket = New Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
        Dim bool_Ping As Boolean = False
        Try
            'bool_Ping = My.Computer.Network.Ping(_ServerIP)
            bool_Ping = True
            If bool_Ping = True Then
                _ClientSocket.Connect(Net.IPAddress.Parse(_ServerIP), _ServerPort)
                If _ClientSocket.Connected = True Then
                    str = "已连接上"
                    Bool_Client_ConnOK = True
                    Return 0
                End If
            Else
                Bool_Client_ConnOK = False
                str = _ServerIP & "连接失败，请检查IP是否设置正确，及网线是否连接正常！"
                MessageBoxEx.Show(str, "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return 1
            End If
        Catch
            Bool_Client_ConnOK = False
            str = _ServerIP & "连接失败，请检查IP是否设置正确，及网线是否连接正常！"
            MessageBoxEx.Show(str, "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return 1
        End Try

        Return 0
    End Function

    ''' <summary>
    ''' 初始化客户端
    ''' </summary>
    ''' <param name="_ClientSocket"></param>
    ''' <param name="ServerIP"></param>
    ''' <param name="ServerPort"></param>
    ''' <remarks></remarks>
    Public Sub Init_Client(ByVal _ClientSocket As Socket, ByVal ServerIP As String, ByVal ServerPort As String)
        _ClientSocket = _ClientSocket
        _ServerIP = ServerIP
        _ServerPort = ServerPort
    End Sub
    ''' <summary>
    ''' 初始化服务器
    ''' </summary>
    ''' <param name="TcpListener"></param>
    ''' <param name="ServerIP"></param>
    ''' <param name="ServerPort"></param>
    ''' <remarks></remarks>
    Public Sub Init_Server(ByVal TcpListener As TcpListener, ByVal ServerIP As String, ByVal ServerPort As String)
        _TcpListener = TcpListener
        _ServerIP = ServerIP
        _ServerPort = ServerPort
    End Sub

    ''' <summary>
    ''' 释放客户端资源
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Dispos_Client()
        If _ClientSocket IsNot Nothing Then
            _ClientSocket.Dispose()
        End If
        _ClientSocket = Nothing
    End Sub
    ''' <summary>
    ''' 释放服务器端资源
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Dispos_Server()
        If _ConSocket IsNot Nothing Then
            _ConSocket.Dispose()
        End If
        _ConSocket = Nothing
        If netstream IsNot Nothing Then
            netstream.Close()
            readstream.Close()
            writestream.Close()
            netstream.Dispose()
            readstream.Dispose()
            writestream.Dispose()
        End If
        If _TcpListener IsNot Nothing Then
            _TcpListener.Stop()
        End If
        _TcpListener = Nothing
    End Sub

    ''' <summary>
    ''' 客户端发送数据
    ''' </summary>
    ''' <param name="Send_Str">发送的数据</param>
    ''' <param name="CBCRLF">是否加回车换行</param>
    ''' <remarks></remarks>
    Public Sub Client_Send_Data(ByVal Send_Str As String, Optional ByVal CBCRLF As String = "")
        Dim msg As Byte()
        If Bool_Client_ConnOK = True Then
            If _ClientSocket.Connected = True Then
                If CBCRLF = "" Then
                    msg = Encoding.UTF8.GetBytes(Send_Str)
                Else
                    msg = Encoding.UTF8.GetBytes(Send_Str & vbCrLf)
                End If
                _ClientSocket.Send(msg)
            End If
        Else
            MsgBox("连接服务器" & _ServerIP & "失败，无法发送数据！")
        End If
    End Sub

    ''' <summary>
    ''' 服务器发送数据
    ''' </summary>
    ''' <param name="Send_Str">发送的数据</param>
    ''' <param name="CBCRLF">是否加回车换行</param>
    ''' <remarks></remarks>
    Public Sub Server_Send_Data(ByVal Send_Str As String, Optional ByVal CBCRLF As String = "")
        Dim msg As Byte()
        If Bool_Server_ConnOK = True Then
            If _ConSocket.Connected = True Then
                If CBCRLF = "" Then
                    msg = Encoding.UTF8.GetBytes(Send_Str)
                Else
                    msg = Encoding.UTF8.GetBytes(Send_Str & vbCrLf)
                End If
                _ConSocket.Send(msg)
            End If
        Else
            MsgBox("客户端连接服务器" & _ServerIP & "失败，无法发送数据！")
        End If
    End Sub
    ''' <summary>
    ''' 客户端接收数据
    ''' </summary>
    ''' <param name="Recive_str"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function Clinet_Recive_Data(ByRef Recive_str As String) As Int16
        If _ClientSocket.Connected = True Then
            Try '接收数据
                Dim bytesTotal As Int64 = 0
                _ClientSocket_buf = New Byte(1024) {} '这里清空一下
                bytesTotal = _ClientSocket.Receive(_ClientSocket_buf)
                If bytesTotal <> 0 Then
                    Recive_str = Encoding.UTF8.GetString(_ClientSocket_buf, 0, bytesTotal)
                    Return 0
                ElseIf bytesTotal = 0 Then
                    Recive_str = "服务器" & _ServerIP & "断开连接"
                    Return 1
                End If
            Catch ex As Exception
                Recive_str = ex.ToString
                Return 2
            End Try
        End If
        Return 0
    End Function

    ''' <summary>
    ''' 服务器接收数据
    ''' </summary>
    ''' <param name="Recive_str"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function Server_Recive_Data(ByRef Recive_str As String) As Int16
        If _ConSocket.Connected = True Then
            Try '接收数据
                Dim bytesTotal As Int64 = 0
                _ServerSocket_buf = New Byte(1024) {} '这里清空一下
                bytesTotal = _ConSocket.Receive(_ServerSocket_buf)
                If bytesTotal <> 0 Then
                    Recive_str = Encoding.UTF8.GetString(_ServerSocket_buf, 0, bytesTotal)
                    Return 0
                ElseIf bytesTotal = 0 Then
                    Recive_str = "服务器" & _ServerIP & "断开连接"
                    Return 1
                End If
            Catch ex As Exception
                Recive_str = ex.ToString
                Return 2
            End Try
            Return 0
        End If
    End Function

    ''' <summary>
    ''' 服务器断开连接，客户端重连
    ''' </summary>
    ''' <param name="T">显示的控件</param>
    '''  <param name="Conn_Index">重连接次数</param>
    ''' <param name="Wait_Time">重连等待时间</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ServerBreak_ClientConn(ByVal T As Object, Optional ByVal Conn_Index As Int16 = 5, Optional ByVal Wait_Time As Int16 = 500) As Int16
        Dim str As String = "服务器断开连接,等待重连..."
        T.text = str
        T.ForeColor = Color.Red
        Dim Bool_Ping_Again As Boolean = True
        Dim bool_Ping As Boolean = False
        For i As Integer = 1 To Conn_Index
            Dim IP As IPAddress = IPAddress.Parse(_ServerIP)
            _ClientSocket = New Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
            Try
                Thread.Sleep(Wait_Time)
                bool_Ping = My.Computer.Network.Ping(_ServerIP)
                If bool_Ping = True Then
                    _ClientSocket.Connect(Net.IPAddress.Parse(_ServerIP), _ServerPort)
                    If _ClientSocket.Connected = True Then
                        str = "服务器已重新连接上..."
                        T.text = str
                        T.Style.ForeColor.Color = Color.Green
                        Bool_Ping_Again = True
                        Exit For
                    End If
                Else
                    Bool_Ping_Again = False
                    str = "服务器重新连接失败[" & i & "]次"
                    T.text = str
                    T.ForeColor = Color.Red
                End If
            Catch
                Bool_Ping_Again = False
                str = "服务器重新连接失败[" & i & "]次"
                T.text = str
                T.ForeColor = Color.Red
            End Try
        Next
        If Bool_Ping_Again = False Then
            Thread.Sleep(1000)
            str = _ServerIP & "断开重连失败！"
            T.text = str
            T.ForeColor = Color.Red
            Return 1
        Else
            Thread.Sleep(1000)
            str = _ServerIP & "断开重连成功！"
            T.text = str
            T.ForeColor = Color.Green
            Return 0
        End If
    End Function

    ''' <summary>
    ''' 显示消息
    ''' </summary>
    ''' <param name="ListBox_Message">显示窗体</param>
    ''' <param name="Message_Str">显示内容</param>
    ''' <remarks></remarks>
    Sub DISPLAY_MESSAGE(ByVal ListBox_Message As ListBox, ByVal Message_Str As String)
        If Message_Str IsNot Nothing Then
            Message_Str = Message_Str.Trim
            If ListBox_Message.Items.Count <= 100 Then
                ListBox_Message.Items.Add(Format(Date.Now.Hour, "00") & "." & Format(Date.Now.Minute, "00") & "." & Format(Date.Now.Second, "00") & Space(2) & Message_Str)
                ListBox_Message.SelectedIndex = ListBox_Message.Items.Count - 1
            Else
                ListBox_Message.Items.Clear()
            End If
        End If
    End Sub

    ''' <summary>
    ''' 服务器开始监听
    ''' </summary>
    ''' <param name="ServerStr">返回的消息</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function Server_Start_Listen(ByRef ServerStr As String) As Int16
        Try
            Dim IPPort As IPEndPoint = New IPEndPoint(Net.IPAddress.Parse(_ServerIP), _ServerPort) '本机使用的IP地址和端口
            _TcpListener = New TcpListener(IPPort)
            Dim Hostname As String = String.Empty
            _TcpListener.Start()
            _ConSocket = _TcpListener.AcceptSocket()
            If _ConSocket.Connected Then
                netstream = New NetworkStream(_ConSocket)
                readstream = New StreamReader(netstream)
                writestream = New StreamWriter(netstream)
                ServerStr = "客户端已经连接上！"
                Bool_Server_ConnOK = True
                Return 0
            Else
                Return 1
            End If
        Catch
            ServerStr = "客户端连接失败！"
            Bool_Server_ConnOK = False
            Server_Break_Listen()
            Return 1
        End Try
    End Function

    ''' <summary>
    ''' TCP监控到客户端断开允许重连
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function Server_Break_Listen() As Int16
        Try
            If (Not netstream Is Nothing) Then
                netstream.Close()
                readstream.Close()
                writestream.Close()
                netstream.Dispose()
                readstream.Dispose()
                writestream.Dispose()
            End If
            _TcpListener.Stop()
            If _ConSocket IsNot Nothing Then
                _ConSocket.Disconnect(True)
                Return 0
            End If
            Return 0
        Catch
            Return 1
        End Try
    End Function
End Class

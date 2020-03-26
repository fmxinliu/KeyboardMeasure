
Imports System.Math
Imports System.Threading
Imports System.Text

Public Class WARING

    Private _Waring_text As String
    Private _Waring_Second As Double
    Private _Index As Integer = 0
    Private _Colse_Wnd As Boolean = False

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="Waring_Str">显示字符串</param>
    ''' <param name="Waring_Second">倒计时</param>
    ''' <remarks></remarks>
    Sub New(ByVal Waring_Str As String, ByVal Waring_Second As Int64)
        _Waring_text = Waring_Str
        _Waring_Second = Waring_Second
        ' 此调用是 Windows 窗体设计器所必需的。
        InitializeComponent()
        ' 在 InitializeComponent() 调用之后添加任何初始化。
    End Sub

    Private Sub WARING_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Select Case _Colse_Wnd
            Case True
                e.Cancel = False
            Case False
                e.Cancel = True
        End Select

    End Sub


    Private Sub WARING_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        PE_Waring.Text = _Waring_text
        PE_Waring.ColorScheme = DevComponents.DotNetBar.Controls.eWarningBoxColorScheme.Yellow

        'Me.Width = PE_Waring.Font.Height * ((_Waring_text & "剩余【90】S").ToString.Length)
        Me.StartPosition = FormStartPosition.CenterScreen
        PE_Waring.Width = Me.Width

        CheckForIllegalCrossThreadCalls = False
        If _RUN_Thread.IsBusy = False Then
            RUN_Thread.RunWorkerAsync()
        End If
        Timer_Index.Start()

    End Sub

    ''' <summary>
    ''' 获取系统时间，单位S
    ''' </summary>
    ''' <param name="time"></param>
    ''' <remarks></remarks>
    Sub Get_SYS_TIME_S(ByRef time As Int64)
        time = Now.Hour * 3600 + Now.Minute * 60 + Now.Second
    End Sub

    Private Sub RUN_Thread_DoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles RUN_Thread.DoWork
        Dim time1, time2 As Int64
      
        Get_SYS_TIME_S(time1)
        Do While (time2 - time1) < _Waring_Second + 1
            Get_SYS_TIME_S(time2)
            PE_Waring.Text = _Waring_text & "剩余" & "【" & _Waring_Second - _Index & "】" & "S"
        Loop
          
        Thread.Sleep(500)
        RUN_Thread.CancelAsync()
        RUN_Thread.Dispose()
        Timer_Index.Stop()
        Me.DialogResult = Windows.Forms.DialogResult.OK
        _Colse_Wnd = True
        Me.Close()
    End Sub

   
    Private Sub Timer_Index_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer_Index.Tick
        _Index += 1
    End Sub

End Class
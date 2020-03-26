Imports System.Threading
Imports DevComponents.DotNetBar

Public Class ADLINK_1903

    Private buffer(), buffer2(), m_get_range(), m_get_range2(), m_get_channel(), m_get_channel2() As UShort
    Private Const maxChannels As UShort = 8  '1903最大通道数量
    Private rtn As Int16
    Private str As String
    Private m_card_num, m_card_num2 As Short
    Private _KG() As ChanelRange

    ''' <summary>
    '''初始化1903
    ''' </summary>
    ''' <param name="KG_1">通道1KG设定</param>
    ''' <param name="KG_2">通道2KG设定</param>
    ''' <param name="KG_3">通道3KG设定</param>
    ''' <param name="KG_4">通道4KG设定</param>
    ''' <param name="KG_5">通道5KG设定</param>
    ''' <param name="KG_6">通道6KG设定</param>
    ''' <param name="KG_7">通道7KG设定</param>
    ''' <param name="KG_8">通道8KG设定</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function Init_1903(ByVal KG_1 As ChanelRange, Optional ByVal KG_2 As ChanelRange = ChanelRange.KG_50, Optional ByVal KG_3 As ChanelRange = ChanelRange.KG_50, Optional ByVal KG_4 As ChanelRange = ChanelRange.KG_50, Optional ByVal KG_5 As ChanelRange = ChanelRange.KG_50, Optional ByVal KG_6 As ChanelRange = ChanelRange.KG_50, Optional ByVal KG_7 As ChanelRange = ChanelRange.KG_50, Optional ByVal KG_8 As ChanelRange = ChanelRange.KG_50, Optional ByRef Ret_Str As String = "") As Int16
        Array.Resize(_KG, 8)
        _KG(0) = KG_1
        _KG(1) = KG_2
        _KG(2) = KG_3
        _KG(3) = KG_4
        _KG(4) = KG_5
        _KG(5) = KG_6
        _KG(6) = KG_7
        _KG(7) = KG_8

        '初始化1903卡
        m_get_channel = New UShort(maxChannels - 1) {}
        m_get_range = New UShort(maxChannels - 1) {}
        buffer = New UShort(maxChannels - 1) {}
        For i As Integer = 0 To maxChannels - 1
            m_get_channel(i) = i
            m_get_range(i) = USBDASK.AD_B_10_V
        Next
        m_card_num = USBDASK.UD_Register_Card(USBDASK.USB_1903, 0)
        If m_card_num < 0 Then
            Ret_Str = "1903数据采集卡连接失败，请检查连接是否正常！"
            MessageBoxEx.Show(Ret_Str, "1903消息", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return 1
        Else
            rtn = USBDASK.UD_AI_1902_Config(CUShort(m_card_num), USBDASK.P1902_AI_Differential, 0, 0, 0, 0)
            If rtn < 0 Then
                Ret_Str = "1903数据采集卡配置文件失败，请检查设置是否正确！"
                MessageBoxEx.Show(Ret_Str, "1903消息", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return 2
            End If
            Ret_Str = "1903数据采集卡成功初始化！"
        End If
        Return 0
    End Function

    Sub Read_Curve_Value(ByRef Value() As Double)
        '4-20mA
        Array.Resize(Value, maxChannels)
        Dim ScaleD, TempD As Double
        Dim Range As Int16 = 0
        If USBDASK.UD_AI_ReadMultiChannels(CUShort(m_card_num), maxChannels, m_get_channel, m_get_range, buffer) = 0 Then
            For i As UShort = 0 To maxChannels - 1
                If USBDASK.UD_AI_VoltScale(CUShort(m_card_num), m_get_range(i), buffer(i), ScaleD) = 0 Then
                    'TempD = _KG(i) / 16 * (ScaleD * 1000 - 4)
                    TempD = ScaleD * 1000 'mA
                    Value(i) = Format(TempD, "0.000")
                End If
            Next
        End If
    End Sub
    Public Enum ChanelRange
        KG_1 = 1
        KG_2 = 2
        KG_5 = 5
        KG_10 = 10
        KG_20 = 20
        KG_30 = 30
        KG_40 = 40
        KG_50 = 50
        KG_100 = 100
    End Enum

    Sub Close_1903()
        USBDASK.UD_Release_Card(USBDASK.USB_1903)
    End Sub

End Class

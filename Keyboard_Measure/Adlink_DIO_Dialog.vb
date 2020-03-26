Imports System.Data.SqlClient
Imports System.Threading
Imports System.Data.OleDb
Imports DevComponents.DotNetBar

Public Class Adlink_DIO_Dialog

    Dim SQLCON As New SQL_Database
    Dim Card As Adlink_Card
    Dim Card_Type As Integer = 4 '//4轴卡还是8轴卡
    Dim Card_NO As Integer '//运动控制卡的数量
    Dim BK_IO_Thread As Thread
    Dim Bool_rtn, Bool_Run_IO As Boolean
    Dim Def_Path As String = System.IO.Directory.GetCurrentDirectory & "\"
    Dim PE_Value(23, 3) As DevComponents.DotNetBar.Controls.TextBoxX
    Dim Value(23, 3) As String
    Dim PE_Value_1(23, 3) As DevComponents.DotNetBar.Controls.TextBoxX
    Dim Value_1(23, 3) As String
    Dim BoradID, rtn_Int As Integer
    Const Card_ID_00 As Integer = (0) '卡号0
    Const Card_ID_01 As Integer = (1) '卡号1
    Const Card_ID_02 As Integer = (1) '卡号2
    Const Card_ID_03 As Integer = (1) '卡号3
    Dim Card_Init_OK As Boolean = False '卡是否初始化成功


#Region "定义控件名称"
    '//输入输出名称
    Dim CARD0_DI_Name_Array() As DevComponents.DotNetBar.Controls.TextBoxX
    Dim CARD0_DO_Name_Array() As DevComponents.DotNetBar.Controls.TextBoxX
    Dim CARD1_DI_Name_Array() As DevComponents.DotNetBar.Controls.TextBoxX
    Dim CARD1_DO_Name_Array() As DevComponents.DotNetBar.Controls.TextBoxX
    Dim CARD2_DI_Name_Array() As DevComponents.DotNetBar.Controls.TextBoxX
    Dim CARD2_DO_Name_Array() As DevComponents.DotNetBar.Controls.TextBoxX
    Dim CARD3_DI_Name_Array() As DevComponents.DotNetBar.Controls.TextBoxX
    Dim CARD3_DO_Name_Array() As DevComponents.DotNetBar.Controls.TextBoxX
    '//输入输出状态
    Dim CARD0_DI_Statue_Array() As DevComponents.DotNetBar.PanelEx
    Dim CARD0_DO_Statue_Array() As DevComponents.DotNetBar.PanelEx
    Dim CARD1_DI_Statue_Array() As DevComponents.DotNetBar.PanelEx
    Dim CARD1_DO_Statue_Array() As DevComponents.DotNetBar.PanelEx
    Dim CARD2_DI_Statue_Array() As DevComponents.DotNetBar.PanelEx
    Dim CARD2_DO_Statue_Array() As DevComponents.DotNetBar.PanelEx
    Dim CARD3_DI_Statue_Array() As DevComponents.DotNetBar.PanelEx
    Dim CARD3_DO_Statue_Array() As DevComponents.DotNetBar.PanelEx

    '//输出点击
    Dim CARD0_CHECK_Array() As System.Windows.Forms.CheckBox
    Dim CARD1_CHECK_Array() As System.Windows.Forms.CheckBox
    Dim CARD2_CHECK_Array() As System.Windows.Forms.CheckBox
    Dim CARD3_CHECK_Array() As System.Windows.Forms.CheckBox

    '//读取原点和限位
    Dim CARD0_PEL_Array() As DevComponents.DotNetBar.PanelEx
    Dim CARD0_ORG_Array() As DevComponents.DotNetBar.PanelEx
    Dim CARD0_MEL_Array() As DevComponents.DotNetBar.PanelEx
    Dim CARD1_PEL_Array() As DevComponents.DotNetBar.PanelEx
    Dim CARD1_ORG_Array() As DevComponents.DotNetBar.PanelEx
    Dim CARD1_MEL_Array() As DevComponents.DotNetBar.PanelEx
    Dim CARD2_PEL_Array() As DevComponents.DotNetBar.PanelEx
    Dim CARD2_ORG_Array() As DevComponents.DotNetBar.PanelEx
    Dim CARD2_MEL_Array() As DevComponents.DotNetBar.PanelEx
    Dim CARD3_PEL_Array() As DevComponents.DotNetBar.PanelEx
    Dim CARD3_ORG_Array() As DevComponents.DotNetBar.PanelEx
    Dim CARD3_MEL_Array() As DevComponents.DotNetBar.PanelEx
    ''' <summary>
    ''' 定义原点和限位
    ''' </summary>
    ''' <remarks></remarks>
    Sub Dim_CARD_ORGPELMEL()
        '//卡0
        CARD0_PEL_Array = New DevComponents.DotNetBar.PanelEx(100) {}
        CARD0_PEL_Array(0) = PEL_Card0_0
        CARD0_PEL_Array(1) = PEL_Card0_1
        CARD0_PEL_Array(2) = PEL_Card0_2
        CARD0_PEL_Array(3) = PEL_Card0_3
        CARD0_PEL_Array(4) = PEL_Card0_4
        CARD0_PEL_Array(5) = PEL_Card0_5
        CARD0_PEL_Array(6) = PEL_Card0_6
        CARD0_PEL_Array(7) = PEL_Card0_7

        CARD0_ORG_Array = New DevComponents.DotNetBar.PanelEx(100) {}
        CARD0_ORG_Array(0) = ORG_Card0_0
        CARD0_ORG_Array(1) = ORG_Card0_1
        CARD0_ORG_Array(2) = ORG_Card0_2
        CARD0_ORG_Array(3) = ORG_Card0_3
        CARD0_ORG_Array(4) = ORG_Card0_4
        CARD0_ORG_Array(5) = ORG_Card0_5
        CARD0_ORG_Array(6) = ORG_Card0_6
        CARD0_ORG_Array(7) = ORG_Card0_7

        CARD0_MEL_Array = New DevComponents.DotNetBar.PanelEx(100) {}
        CARD0_MEL_Array(0) = MEL_Card0_0
        CARD0_MEL_Array(1) = MEL_Card0_1
        CARD0_MEL_Array(2) = MEL_Card0_2
        CARD0_MEL_Array(3) = MEL_Card0_3
        CARD0_MEL_Array(4) = MEL_Card0_4
        CARD0_MEL_Array(5) = MEL_Card0_5
        CARD0_MEL_Array(6) = MEL_Card0_6
        CARD0_MEL_Array(7) = MEL_Card0_7
        '//卡1
        CARD1_PEL_Array = New DevComponents.DotNetBar.PanelEx(100) {}
        CARD1_PEL_Array(0) = PEL_Card1_0
        CARD1_PEL_Array(1) = PEL_Card1_1
        CARD1_PEL_Array(2) = PEL_Card1_2
        CARD1_PEL_Array(3) = PEL_Card1_3
        CARD1_PEL_Array(4) = PEL_Card1_4
        CARD1_PEL_Array(5) = PEL_Card1_5
        CARD1_PEL_Array(6) = PEL_Card1_6
        CARD1_PEL_Array(7) = PEL_Card1_7

        CARD1_ORG_Array = New DevComponents.DotNetBar.PanelEx(100) {}
        CARD1_ORG_Array(0) = ORG_Card1_0
        CARD1_ORG_Array(1) = ORG_Card1_1
        CARD1_ORG_Array(2) = ORG_Card1_2
        CARD1_ORG_Array(3) = ORG_Card1_3
        CARD1_ORG_Array(4) = ORG_Card1_4
        CARD1_ORG_Array(5) = ORG_Card1_5
        CARD1_ORG_Array(6) = ORG_Card1_6
        CARD1_ORG_Array(7) = ORG_Card1_7

        CARD1_MEL_Array = New DevComponents.DotNetBar.PanelEx(100) {}
        CARD1_MEL_Array(0) = MEL_Card1_0
        CARD1_MEL_Array(1) = MEL_Card1_1
        CARD1_MEL_Array(2) = MEL_Card1_2
        CARD1_MEL_Array(3) = MEL_Card1_3
        CARD1_MEL_Array(4) = MEL_Card1_4
        CARD1_MEL_Array(5) = MEL_Card1_5
        CARD1_MEL_Array(6) = MEL_Card1_6
        CARD1_MEL_Array(7) = MEL_Card1_7
        '//卡2
        CARD2_PEL_Array = New DevComponents.DotNetBar.PanelEx(100) {}
        CARD2_PEL_Array(0) = PEL_Card2_0
        CARD2_PEL_Array(1) = PEL_Card2_1
        CARD2_PEL_Array(2) = PEL_Card2_2
        CARD2_PEL_Array(3) = PEL_Card2_3
        CARD2_PEL_Array(4) = PEL_Card2_4
        CARD2_PEL_Array(5) = PEL_Card2_5
        CARD2_PEL_Array(6) = PEL_Card2_6
        CARD2_PEL_Array(7) = PEL_Card2_7

        CARD2_ORG_Array = New DevComponents.DotNetBar.PanelEx(100) {}
        CARD2_ORG_Array(0) = ORG_Card2_0
        CARD2_ORG_Array(1) = ORG_Card2_1
        CARD2_ORG_Array(2) = ORG_Card2_2
        CARD2_ORG_Array(3) = ORG_Card2_3
        CARD2_ORG_Array(4) = ORG_Card2_4
        CARD2_ORG_Array(5) = ORG_Card2_5
        CARD2_ORG_Array(6) = ORG_Card2_6
        CARD2_ORG_Array(7) = ORG_Card2_7

        CARD2_MEL_Array = New DevComponents.DotNetBar.PanelEx(100) {}
        CARD2_MEL_Array(0) = MEL_Card2_0
        CARD2_MEL_Array(1) = MEL_Card2_1
        CARD2_MEL_Array(2) = MEL_Card2_2
        CARD2_MEL_Array(3) = MEL_Card2_3
        CARD2_MEL_Array(4) = MEL_Card2_4
        CARD2_MEL_Array(5) = MEL_Card2_5
        CARD2_MEL_Array(6) = MEL_Card2_6
        CARD2_MEL_Array(7) = MEL_Card2_7
        '//卡3
        CARD3_PEL_Array = New DevComponents.DotNetBar.PanelEx(100) {}
        CARD3_PEL_Array(0) = PEL_Card3_0
        CARD3_PEL_Array(1) = PEL_Card3_1
        CARD3_PEL_Array(2) = PEL_Card3_2
        CARD3_PEL_Array(3) = PEL_Card3_3
        CARD3_PEL_Array(4) = PEL_Card3_4
        CARD3_PEL_Array(5) = PEL_Card3_5
        CARD3_PEL_Array(6) = PEL_Card3_6
        CARD3_PEL_Array(7) = PEL_Card3_7

        CARD3_ORG_Array = New DevComponents.DotNetBar.PanelEx(100) {}
        CARD3_ORG_Array(0) = ORG_Card3_0
        CARD3_ORG_Array(1) = ORG_Card3_1
        CARD3_ORG_Array(2) = ORG_Card3_2
        CARD3_ORG_Array(3) = ORG_Card3_3
        CARD3_ORG_Array(4) = ORG_Card3_4
        CARD3_ORG_Array(5) = ORG_Card3_5
        CARD3_ORG_Array(6) = ORG_Card3_6
        CARD3_ORG_Array(7) = ORG_Card3_7

        CARD3_MEL_Array = New DevComponents.DotNetBar.PanelEx(100) {}
        CARD3_MEL_Array(0) = MEL_Card3_0
        CARD3_MEL_Array(1) = MEL_Card3_1
        CARD3_MEL_Array(2) = MEL_Card3_2
        CARD3_MEL_Array(3) = MEL_Card3_3
        CARD3_MEL_Array(4) = MEL_Card3_4
        CARD3_MEL_Array(5) = MEL_Card3_5
        CARD3_MEL_Array(6) = MEL_Card3_6
        CARD3_MEL_Array(7) = MEL_Card3_7
    End Sub

    ''' <summary>
    ''' 定义输入输出名称
    ''' </summary>
    ''' <remarks></remarks>
    Sub Dim_CARD_DI_Name()
        '//卡0
        CARD0_DI_Name_Array = New DevComponents.DotNetBar.Controls.TextBoxX(100) {}
        CARD0_DI_Name_Array(0) = CARD0_DI_Name_0
        CARD0_DI_Name_Array(1) = CARD0_DI_Name_1
        CARD0_DI_Name_Array(2) = CARD0_DI_Name_2
        CARD0_DI_Name_Array(3) = CARD0_DI_Name_3
        CARD0_DI_Name_Array(4) = CARD0_DI_Name_4
        CARD0_DI_Name_Array(5) = CARD0_DI_Name_5
        CARD0_DI_Name_Array(6) = CARD0_DI_Name_6
        CARD0_DI_Name_Array(7) = CARD0_DI_Name_7
        CARD0_DI_Name_Array(8) = CARD0_DI_Name_8
        CARD0_DI_Name_Array(9) = CARD0_DI_Name_9
        CARD0_DI_Name_Array(10) = CARD0_DI_Name_10
        CARD0_DI_Name_Array(11) = CARD0_DI_Name_11
        CARD0_DI_Name_Array(12) = CARD0_DI_Name_12
        CARD0_DI_Name_Array(13) = CARD0_DI_Name_13
        CARD0_DI_Name_Array(14) = CARD0_DI_Name_14
        CARD0_DI_Name_Array(15) = CARD0_DI_Name_15
        CARD0_DI_Name_Array(16) = CARD0_DI_Name_16
        CARD0_DI_Name_Array(17) = CARD0_DI_Name_17
        CARD0_DI_Name_Array(18) = CARD0_DI_Name_18
        CARD0_DI_Name_Array(19) = CARD0_DI_Name_19
        CARD0_DI_Name_Array(20) = CARD0_DI_Name_20
        CARD0_DI_Name_Array(21) = CARD0_DI_Name_21
        CARD0_DI_Name_Array(22) = CARD0_DI_Name_22
        CARD0_DI_Name_Array(23) = CARD0_DI_Name_23

        CARD0_DO_Name_Array = New DevComponents.DotNetBar.Controls.TextBoxX(100) {}
        CARD0_DO_Name_Array(0) = CARD0_DO_Name_0
        CARD0_DO_Name_Array(1) = CARD0_DO_Name_1
        CARD0_DO_Name_Array(2) = CARD0_DO_Name_2
        CARD0_DO_Name_Array(3) = CARD0_DO_Name_3
        CARD0_DO_Name_Array(4) = CARD0_DO_Name_4
        CARD0_DO_Name_Array(5) = CARD0_DO_Name_5
        CARD0_DO_Name_Array(6) = CARD0_DO_Name_6
        CARD0_DO_Name_Array(7) = CARD0_DO_Name_7
        CARD0_DO_Name_Array(8) = CARD0_DO_Name_8
        CARD0_DO_Name_Array(9) = CARD0_DO_Name_9
        CARD0_DO_Name_Array(10) = CARD0_DO_Name_10
        CARD0_DO_Name_Array(11) = CARD0_DO_Name_11
        CARD0_DO_Name_Array(12) = CARD0_DO_Name_12
        CARD0_DO_Name_Array(13) = CARD0_DO_Name_13
        CARD0_DO_Name_Array(14) = CARD0_DO_Name_14
        CARD0_DO_Name_Array(15) = CARD0_DO_Name_15
        CARD0_DO_Name_Array(16) = CARD0_DO_Name_16
        CARD0_DO_Name_Array(17) = CARD0_DO_Name_17
        CARD0_DO_Name_Array(18) = CARD0_DO_Name_18
        CARD0_DO_Name_Array(19) = CARD0_DO_Name_19
        CARD0_DO_Name_Array(20) = CARD0_DO_Name_20
        CARD0_DO_Name_Array(21) = CARD0_DO_Name_21
        CARD0_DO_Name_Array(22) = CARD0_DO_Name_22
        CARD0_DO_Name_Array(23) = CARD0_DO_Name_23
        '//卡1
        CARD1_DI_Name_Array = New DevComponents.DotNetBar.Controls.TextBoxX(100) {}
        CARD1_DI_Name_Array(0) = CARD1_DI_Name_0
        CARD1_DI_Name_Array(1) = CARD1_DI_Name_1
        CARD1_DI_Name_Array(2) = CARD1_DI_Name_2
        CARD1_DI_Name_Array(3) = CARD1_DI_Name_3
        CARD1_DI_Name_Array(4) = CARD1_DI_Name_4
        CARD1_DI_Name_Array(5) = CARD1_DI_Name_5
        CARD1_DI_Name_Array(6) = CARD1_DI_Name_6
        CARD1_DI_Name_Array(7) = CARD1_DI_Name_7
        CARD1_DI_Name_Array(8) = CARD1_DI_Name_8
        CARD1_DI_Name_Array(9) = CARD1_DI_Name_9
        CARD1_DI_Name_Array(10) = CARD1_DI_Name_10
        CARD1_DI_Name_Array(11) = CARD1_DI_Name_11
        CARD1_DI_Name_Array(12) = CARD1_DI_Name_12
        CARD1_DI_Name_Array(13) = CARD1_DI_Name_13
        CARD1_DI_Name_Array(14) = CARD1_DI_Name_14
        CARD1_DI_Name_Array(15) = CARD1_DI_Name_15
        CARD1_DI_Name_Array(16) = CARD1_DI_Name_16
        CARD1_DI_Name_Array(17) = CARD1_DI_Name_17
        CARD1_DI_Name_Array(18) = CARD1_DI_Name_18
        CARD1_DI_Name_Array(19) = CARD1_DI_Name_19
        CARD1_DI_Name_Array(20) = CARD1_DI_Name_20
        CARD1_DI_Name_Array(21) = CARD1_DI_Name_21
        CARD1_DI_Name_Array(22) = CARD1_DI_Name_22
        CARD1_DI_Name_Array(23) = CARD1_DI_Name_23

        CARD1_DO_Name_Array = New DevComponents.DotNetBar.Controls.TextBoxX(100) {}
        CARD1_DO_Name_Array(0) = CARD1_DO_Name_0
        CARD1_DO_Name_Array(1) = CARD1_DO_Name_1
        CARD1_DO_Name_Array(2) = CARD1_DO_Name_2
        CARD1_DO_Name_Array(3) = CARD1_DO_Name_3
        CARD1_DO_Name_Array(4) = CARD1_DO_Name_4
        CARD1_DO_Name_Array(5) = CARD1_DO_Name_5
        CARD1_DO_Name_Array(6) = CARD1_DO_Name_6
        CARD1_DO_Name_Array(7) = CARD1_DO_Name_7
        CARD1_DO_Name_Array(8) = CARD1_DO_Name_8
        CARD1_DO_Name_Array(9) = CARD1_DO_Name_9
        CARD1_DO_Name_Array(10) = CARD1_DO_Name_10
        CARD1_DO_Name_Array(11) = CARD1_DO_Name_11
        CARD1_DO_Name_Array(12) = CARD1_DO_Name_12
        CARD1_DO_Name_Array(13) = CARD1_DO_Name_13
        CARD1_DO_Name_Array(14) = CARD1_DO_Name_14
        CARD1_DO_Name_Array(15) = CARD1_DO_Name_15
        CARD1_DO_Name_Array(16) = CARD1_DO_Name_16
        CARD1_DO_Name_Array(17) = CARD1_DO_Name_17
        CARD1_DO_Name_Array(18) = CARD1_DO_Name_18
        CARD1_DO_Name_Array(19) = CARD1_DO_Name_19
        CARD1_DO_Name_Array(20) = CARD1_DO_Name_20
        CARD1_DO_Name_Array(21) = CARD1_DO_Name_21
        CARD1_DO_Name_Array(22) = CARD1_DO_Name_22
        CARD1_DO_Name_Array(23) = CARD1_DO_Name_23
        '//卡2
        CARD2_DI_Name_Array = New DevComponents.DotNetBar.Controls.TextBoxX(100) {}
        CARD2_DI_Name_Array(0) = CARD2_DI_Name_0
        CARD2_DI_Name_Array(1) = CARD2_DI_Name_1
        CARD2_DI_Name_Array(2) = CARD2_DI_Name_2
        CARD2_DI_Name_Array(3) = CARD2_DI_Name_3
        CARD2_DI_Name_Array(4) = CARD2_DI_Name_4
        CARD2_DI_Name_Array(5) = CARD2_DI_Name_5
        CARD2_DI_Name_Array(6) = CARD2_DI_Name_6
        CARD2_DI_Name_Array(7) = CARD2_DI_Name_7
        CARD2_DI_Name_Array(8) = CARD2_DI_Name_8
        CARD2_DI_Name_Array(9) = CARD2_DI_Name_9
        CARD2_DI_Name_Array(10) = CARD2_DI_Name_10
        CARD2_DI_Name_Array(11) = CARD2_DI_Name_11
        CARD2_DI_Name_Array(12) = CARD2_DI_Name_12
        CARD2_DI_Name_Array(13) = CARD2_DI_Name_13
        CARD2_DI_Name_Array(14) = CARD2_DI_Name_14
        CARD2_DI_Name_Array(15) = CARD2_DI_Name_15
        CARD2_DI_Name_Array(16) = CARD2_DI_Name_16
        CARD2_DI_Name_Array(17) = CARD2_DI_Name_17
        CARD2_DI_Name_Array(18) = CARD2_DI_Name_18
        CARD2_DI_Name_Array(19) = CARD2_DI_Name_19
        CARD2_DI_Name_Array(20) = CARD2_DI_Name_20
        CARD2_DI_Name_Array(21) = CARD2_DI_Name_21
        CARD2_DI_Name_Array(22) = CARD2_DI_Name_22
        CARD2_DI_Name_Array(23) = CARD2_DI_Name_23

        CARD2_DO_Name_Array = New DevComponents.DotNetBar.Controls.TextBoxX(100) {}
        CARD2_DO_Name_Array(0) = CARD2_DO_Name_0
        CARD2_DO_Name_Array(1) = CARD2_DO_Name_1
        CARD2_DO_Name_Array(2) = CARD2_DO_Name_2
        CARD2_DO_Name_Array(3) = CARD2_DO_Name_3
        CARD2_DO_Name_Array(4) = CARD2_DO_Name_4
        CARD2_DO_Name_Array(5) = CARD2_DO_Name_5
        CARD2_DO_Name_Array(6) = CARD2_DO_Name_6
        CARD2_DO_Name_Array(7) = CARD2_DO_Name_7
        CARD2_DO_Name_Array(8) = CARD2_DO_Name_8
        CARD2_DO_Name_Array(9) = CARD2_DO_Name_9
        CARD2_DO_Name_Array(10) = CARD2_DO_Name_10
        CARD2_DO_Name_Array(11) = CARD2_DO_Name_11
        CARD2_DO_Name_Array(12) = CARD2_DO_Name_12
        CARD2_DO_Name_Array(13) = CARD2_DO_Name_13
        CARD2_DO_Name_Array(14) = CARD2_DO_Name_14
        CARD2_DO_Name_Array(15) = CARD2_DO_Name_15
        CARD2_DO_Name_Array(16) = CARD2_DO_Name_16
        CARD2_DO_Name_Array(17) = CARD2_DO_Name_17
        CARD2_DO_Name_Array(18) = CARD2_DO_Name_18
        CARD2_DO_Name_Array(19) = CARD2_DO_Name_19
        CARD2_DO_Name_Array(20) = CARD2_DO_Name_20
        CARD2_DO_Name_Array(21) = CARD2_DO_Name_21
        CARD2_DO_Name_Array(22) = CARD2_DO_Name_22
        CARD2_DO_Name_Array(23) = CARD2_DO_Name_23
        '//卡3
        CARD3_DI_Name_Array = New DevComponents.DotNetBar.Controls.TextBoxX(100) {}
        CARD3_DI_Name_Array(0) = CARD3_DI_Name_0
        CARD3_DI_Name_Array(1) = CARD3_DI_Name_1
        CARD3_DI_Name_Array(2) = CARD3_DI_Name_2
        CARD3_DI_Name_Array(3) = CARD3_DI_Name_3
        CARD3_DI_Name_Array(4) = CARD3_DI_Name_4
        CARD3_DI_Name_Array(5) = CARD3_DI_Name_5
        CARD3_DI_Name_Array(6) = CARD3_DI_Name_6
        CARD3_DI_Name_Array(7) = CARD3_DI_Name_7
        CARD3_DI_Name_Array(8) = CARD3_DI_Name_8
        CARD3_DI_Name_Array(9) = CARD3_DI_Name_9
        CARD3_DI_Name_Array(10) = CARD3_DI_Name_10
        CARD3_DI_Name_Array(11) = CARD3_DI_Name_11
        CARD3_DI_Name_Array(12) = CARD3_DI_Name_12
        CARD3_DI_Name_Array(13) = CARD3_DI_Name_13
        CARD3_DI_Name_Array(14) = CARD3_DI_Name_14
        CARD3_DI_Name_Array(15) = CARD3_DI_Name_15
        CARD3_DI_Name_Array(16) = CARD3_DI_Name_16
        CARD3_DI_Name_Array(17) = CARD3_DI_Name_17
        CARD3_DI_Name_Array(18) = CARD3_DI_Name_18
        CARD3_DI_Name_Array(19) = CARD3_DI_Name_19
        CARD3_DI_Name_Array(20) = CARD3_DI_Name_20
        CARD3_DI_Name_Array(21) = CARD3_DI_Name_21
        CARD3_DI_Name_Array(22) = CARD3_DI_Name_22
        CARD3_DI_Name_Array(23) = CARD3_DI_Name_23

        CARD3_DO_Name_Array = New DevComponents.DotNetBar.Controls.TextBoxX(100) {}
        CARD3_DO_Name_Array(0) = CARD3_DO_Name_0
        CARD3_DO_Name_Array(1) = CARD3_DO_Name_1
        CARD3_DO_Name_Array(2) = CARD3_DO_Name_2
        CARD3_DO_Name_Array(3) = CARD3_DO_Name_3
        CARD3_DO_Name_Array(4) = CARD3_DO_Name_4
        CARD3_DO_Name_Array(5) = CARD3_DO_Name_5
        CARD3_DO_Name_Array(6) = CARD3_DO_Name_6
        CARD3_DO_Name_Array(7) = CARD3_DO_Name_7
        CARD3_DO_Name_Array(8) = CARD3_DO_Name_8
        CARD3_DO_Name_Array(9) = CARD3_DO_Name_9
        CARD3_DO_Name_Array(10) = CARD3_DO_Name_10
        CARD3_DO_Name_Array(11) = CARD3_DO_Name_11
        CARD3_DO_Name_Array(12) = CARD3_DO_Name_12
        CARD3_DO_Name_Array(13) = CARD3_DO_Name_13
        CARD3_DO_Name_Array(14) = CARD3_DO_Name_14
        CARD3_DO_Name_Array(15) = CARD3_DO_Name_15
        CARD3_DO_Name_Array(16) = CARD3_DO_Name_16
        CARD3_DO_Name_Array(17) = CARD3_DO_Name_17
        CARD3_DO_Name_Array(18) = CARD3_DO_Name_18
        CARD3_DO_Name_Array(19) = CARD3_DO_Name_19
        CARD3_DO_Name_Array(20) = CARD3_DO_Name_20
        CARD3_DO_Name_Array(21) = CARD3_DO_Name_21
        CARD3_DO_Name_Array(22) = CARD3_DO_Name_22
        CARD3_DO_Name_Array(23) = CARD3_DO_Name_23
    End Sub

    ''' <summary>
    ''' 定义输入输出状态
    ''' </summary>
    ''' <remarks></remarks>
    Sub Dim_CARD_DI_Statue()
        '//卡0
        CARD0_DI_Statue_Array = New DevComponents.DotNetBar.PanelEx(100) {}
        CARD0_DI_Statue_Array(0) = CARD0_DI_Statue_0
        CARD0_DI_Statue_Array(1) = CARD0_DI_Statue_1
        CARD0_DI_Statue_Array(2) = CARD0_DI_Statue_2
        CARD0_DI_Statue_Array(3) = CARD0_DI_Statue_3
        CARD0_DI_Statue_Array(4) = CARD0_DI_Statue_4
        CARD0_DI_Statue_Array(5) = CARD0_DI_Statue_5
        CARD0_DI_Statue_Array(6) = CARD0_DI_Statue_6
        CARD0_DI_Statue_Array(7) = CARD0_DI_Statue_7
        CARD0_DI_Statue_Array(8) = CARD0_DI_Statue_8
        CARD0_DI_Statue_Array(9) = CARD0_DI_Statue_9
        CARD0_DI_Statue_Array(10) = CARD0_DI_Statue_10
        CARD0_DI_Statue_Array(11) = CARD0_DI_Statue_11
        CARD0_DI_Statue_Array(12) = CARD0_DI_Statue_12
        CARD0_DI_Statue_Array(13) = CARD0_DI_Statue_13
        CARD0_DI_Statue_Array(14) = CARD0_DI_Statue_14
        CARD0_DI_Statue_Array(15) = CARD0_DI_Statue_15
        CARD0_DI_Statue_Array(16) = CARD0_DI_Statue_16
        CARD0_DI_Statue_Array(17) = CARD0_DI_Statue_17
        CARD0_DI_Statue_Array(18) = CARD0_DI_Statue_18
        CARD0_DI_Statue_Array(19) = CARD0_DI_Statue_19
        CARD0_DI_Statue_Array(20) = CARD0_DI_Statue_20
        CARD0_DI_Statue_Array(21) = CARD0_DI_Statue_21
        CARD0_DI_Statue_Array(22) = CARD0_DI_Statue_22
        CARD0_DI_Statue_Array(23) = CARD0_DI_Statue_23

        CARD0_DO_Statue_Array = New DevComponents.DotNetBar.PanelEx(100) {}
        CARD0_DO_Statue_Array(0) = CARD0_DO_Statue_0
        CARD0_DO_Statue_Array(1) = CARD0_DO_Statue_1
        CARD0_DO_Statue_Array(2) = CARD0_DO_Statue_2
        CARD0_DO_Statue_Array(3) = CARD0_DO_Statue_3
        CARD0_DO_Statue_Array(4) = CARD0_DO_Statue_4
        CARD0_DO_Statue_Array(5) = CARD0_DO_Statue_5
        CARD0_DO_Statue_Array(6) = CARD0_DO_Statue_6
        CARD0_DO_Statue_Array(7) = CARD0_DO_Statue_7
        CARD0_DO_Statue_Array(8) = CARD0_DO_Statue_8
        CARD0_DO_Statue_Array(9) = CARD0_DO_Statue_9
        CARD0_DO_Statue_Array(10) = CARD0_DO_Statue_10
        CARD0_DO_Statue_Array(11) = CARD0_DO_Statue_11
        CARD0_DO_Statue_Array(12) = CARD0_DO_Statue_12
        CARD0_DO_Statue_Array(13) = CARD0_DO_Statue_13
        CARD0_DO_Statue_Array(14) = CARD0_DO_Statue_14
        CARD0_DO_Statue_Array(15) = CARD0_DO_Statue_15
        CARD0_DO_Statue_Array(16) = CARD0_DO_Statue_16
        CARD0_DO_Statue_Array(17) = CARD0_DO_Statue_17
        CARD0_DO_Statue_Array(18) = CARD0_DO_Statue_18
        CARD0_DO_Statue_Array(19) = CARD0_DO_Statue_19
        CARD0_DO_Statue_Array(20) = CARD0_DO_Statue_20
        CARD0_DO_Statue_Array(21) = CARD0_DO_Statue_21
        CARD0_DO_Statue_Array(22) = CARD0_DO_Statue_22
        CARD0_DO_Statue_Array(23) = CARD0_DO_Statue_23
        '//卡1
        CARD1_DI_Statue_Array = New DevComponents.DotNetBar.PanelEx(100) {}
        CARD1_DI_Statue_Array(0) = CARD1_DI_Statue_0
        CARD1_DI_Statue_Array(1) = CARD1_DI_Statue_1
        CARD1_DI_Statue_Array(2) = CARD1_DI_Statue_2
        CARD1_DI_Statue_Array(3) = CARD1_DI_Statue_3
        CARD1_DI_Statue_Array(4) = CARD1_DI_Statue_4
        CARD1_DI_Statue_Array(5) = CARD1_DI_Statue_5
        CARD1_DI_Statue_Array(6) = CARD1_DI_Statue_6
        CARD1_DI_Statue_Array(7) = CARD1_DI_Statue_7
        CARD1_DI_Statue_Array(8) = CARD1_DI_Statue_8
        CARD1_DI_Statue_Array(9) = CARD1_DI_Statue_9
        CARD1_DI_Statue_Array(10) = CARD1_DI_Statue_10
        CARD1_DI_Statue_Array(11) = CARD1_DI_Statue_11
        CARD1_DI_Statue_Array(12) = CARD1_DI_Statue_12
        CARD1_DI_Statue_Array(13) = CARD1_DI_Statue_13
        CARD1_DI_Statue_Array(14) = CARD1_DI_Statue_14
        CARD1_DI_Statue_Array(15) = CARD1_DI_Statue_15
        CARD1_DI_Statue_Array(16) = CARD1_DI_Statue_16
        CARD1_DI_Statue_Array(17) = CARD1_DI_Statue_17
        CARD1_DI_Statue_Array(18) = CARD1_DI_Statue_18
        CARD1_DI_Statue_Array(19) = CARD1_DI_Statue_19
        CARD1_DI_Statue_Array(20) = CARD1_DI_Statue_20
        CARD1_DI_Statue_Array(21) = CARD1_DI_Statue_21
        CARD1_DI_Statue_Array(22) = CARD1_DI_Statue_22
        CARD1_DI_Statue_Array(23) = CARD1_DI_Statue_23

        CARD1_DO_Statue_Array = New DevComponents.DotNetBar.PanelEx(100) {}
        CARD1_DO_Statue_Array(0) = CARD1_DO_Statue_0
        CARD1_DO_Statue_Array(1) = CARD1_DO_Statue_1
        CARD1_DO_Statue_Array(2) = CARD1_DO_Statue_2
        CARD1_DO_Statue_Array(3) = CARD1_DO_Statue_3
        CARD1_DO_Statue_Array(4) = CARD1_DO_Statue_4
        CARD1_DO_Statue_Array(5) = CARD1_DO_Statue_5
        CARD1_DO_Statue_Array(6) = CARD1_DO_Statue_6
        CARD1_DO_Statue_Array(7) = CARD1_DO_Statue_7
        CARD1_DO_Statue_Array(8) = CARD1_DO_Statue_8
        CARD1_DO_Statue_Array(9) = CARD1_DO_Statue_9
        CARD1_DO_Statue_Array(10) = CARD1_DO_Statue_10
        CARD1_DO_Statue_Array(11) = CARD1_DO_Statue_11
        CARD1_DO_Statue_Array(12) = CARD1_DO_Statue_12
        CARD1_DO_Statue_Array(13) = CARD1_DO_Statue_13
        CARD1_DO_Statue_Array(14) = CARD1_DO_Statue_14
        CARD1_DO_Statue_Array(15) = CARD1_DO_Statue_15
        CARD1_DO_Statue_Array(16) = CARD1_DO_Statue_16
        CARD1_DO_Statue_Array(17) = CARD1_DO_Statue_17
        CARD1_DO_Statue_Array(18) = CARD1_DO_Statue_18
        CARD1_DO_Statue_Array(19) = CARD1_DO_Statue_19
        CARD1_DO_Statue_Array(20) = CARD1_DO_Statue_20
        CARD1_DO_Statue_Array(21) = CARD1_DO_Statue_21
        CARD1_DO_Statue_Array(22) = CARD1_DO_Statue_22
        CARD1_DO_Statue_Array(23) = CARD1_DO_Statue_23
        '//卡2
        CARD2_DI_Statue_Array = New DevComponents.DotNetBar.PanelEx(100) {}
        CARD2_DI_Statue_Array(0) = CARD2_DI_Statue_0
        CARD2_DI_Statue_Array(1) = CARD2_DI_Statue_1
        CARD2_DI_Statue_Array(2) = CARD2_DI_Statue_2
        CARD2_DI_Statue_Array(3) = CARD2_DI_Statue_3
        CARD2_DI_Statue_Array(4) = CARD2_DI_Statue_4
        CARD2_DI_Statue_Array(5) = CARD2_DI_Statue_5
        CARD2_DI_Statue_Array(6) = CARD2_DI_Statue_6
        CARD2_DI_Statue_Array(7) = CARD2_DI_Statue_7
        CARD2_DI_Statue_Array(8) = CARD2_DI_Statue_8
        CARD2_DI_Statue_Array(9) = CARD2_DI_Statue_9
        CARD2_DI_Statue_Array(10) = CARD2_DI_Statue_10
        CARD2_DI_Statue_Array(11) = CARD2_DI_Statue_11
        CARD2_DI_Statue_Array(12) = CARD2_DI_Statue_12
        CARD2_DI_Statue_Array(13) = CARD2_DI_Statue_13
        CARD2_DI_Statue_Array(14) = CARD2_DI_Statue_14
        CARD2_DI_Statue_Array(15) = CARD2_DI_Statue_15
        CARD2_DI_Statue_Array(16) = CARD2_DI_Statue_16
        CARD2_DI_Statue_Array(17) = CARD2_DI_Statue_17
        CARD2_DI_Statue_Array(18) = CARD2_DI_Statue_18
        CARD2_DI_Statue_Array(19) = CARD2_DI_Statue_19
        CARD2_DI_Statue_Array(20) = CARD2_DI_Statue_20
        CARD2_DI_Statue_Array(21) = CARD2_DI_Statue_21
        CARD2_DI_Statue_Array(22) = CARD2_DI_Statue_22
        CARD2_DI_Statue_Array(23) = CARD2_DI_Statue_23

        CARD2_DO_Statue_Array = New DevComponents.DotNetBar.PanelEx(100) {}
        CARD2_DO_Statue_Array(0) = CARD2_DO_Statue_0
        CARD2_DO_Statue_Array(1) = CARD2_DO_Statue_1
        CARD2_DO_Statue_Array(2) = CARD2_DO_Statue_2
        CARD2_DO_Statue_Array(3) = CARD2_DO_Statue_3
        CARD2_DO_Statue_Array(4) = CARD2_DO_Statue_4
        CARD2_DO_Statue_Array(5) = CARD2_DO_Statue_5
        CARD2_DO_Statue_Array(6) = CARD2_DO_Statue_6
        CARD2_DO_Statue_Array(7) = CARD2_DO_Statue_7
        CARD2_DO_Statue_Array(8) = CARD2_DO_Statue_8
        CARD2_DO_Statue_Array(9) = CARD2_DO_Statue_9
        CARD2_DO_Statue_Array(10) = CARD2_DO_Statue_10
        CARD2_DO_Statue_Array(11) = CARD2_DO_Statue_11
        CARD2_DO_Statue_Array(12) = CARD2_DO_Statue_12
        CARD2_DO_Statue_Array(13) = CARD2_DO_Statue_13
        CARD2_DO_Statue_Array(14) = CARD2_DO_Statue_14
        CARD2_DO_Statue_Array(15) = CARD2_DO_Statue_15
        CARD2_DO_Statue_Array(16) = CARD2_DO_Statue_16
        CARD2_DO_Statue_Array(17) = CARD2_DO_Statue_17
        CARD2_DO_Statue_Array(18) = CARD2_DO_Statue_18
        CARD2_DO_Statue_Array(19) = CARD2_DO_Statue_19
        CARD2_DO_Statue_Array(20) = CARD2_DO_Statue_20
        CARD2_DO_Statue_Array(21) = CARD2_DO_Statue_21
        CARD2_DO_Statue_Array(22) = CARD2_DO_Statue_22
        CARD2_DO_Statue_Array(23) = CARD2_DO_Statue_23
        '//卡3
        CARD3_DI_Statue_Array = New DevComponents.DotNetBar.PanelEx(100) {}
        CARD3_DI_Statue_Array(0) = CARD3_DI_Statue_0
        CARD3_DI_Statue_Array(1) = CARD3_DI_Statue_1
        CARD3_DI_Statue_Array(2) = CARD3_DI_Statue_2
        CARD3_DI_Statue_Array(3) = CARD3_DI_Statue_3
        CARD3_DI_Statue_Array(4) = CARD3_DI_Statue_4
        CARD3_DI_Statue_Array(5) = CARD3_DI_Statue_5
        CARD3_DI_Statue_Array(6) = CARD3_DI_Statue_6
        CARD3_DI_Statue_Array(7) = CARD3_DI_Statue_7
        CARD3_DI_Statue_Array(8) = CARD3_DI_Statue_8
        CARD3_DI_Statue_Array(9) = CARD3_DI_Statue_9
        CARD3_DI_Statue_Array(10) = CARD3_DI_Statue_10
        CARD3_DI_Statue_Array(11) = CARD3_DI_Statue_11
        CARD3_DI_Statue_Array(12) = CARD3_DI_Statue_12
        CARD3_DI_Statue_Array(13) = CARD3_DI_Statue_13
        CARD3_DI_Statue_Array(14) = CARD3_DI_Statue_14
        CARD3_DI_Statue_Array(15) = CARD3_DI_Statue_15
        CARD3_DI_Statue_Array(16) = CARD3_DI_Statue_16
        CARD3_DI_Statue_Array(17) = CARD3_DI_Statue_17
        CARD3_DI_Statue_Array(18) = CARD3_DI_Statue_18
        CARD3_DI_Statue_Array(19) = CARD3_DI_Statue_19
        CARD3_DI_Statue_Array(20) = CARD3_DI_Statue_20
        CARD3_DI_Statue_Array(21) = CARD3_DI_Statue_21
        CARD3_DI_Statue_Array(22) = CARD3_DI_Statue_22
        CARD3_DI_Statue_Array(23) = CARD3_DI_Statue_23

        CARD3_DO_Statue_Array = New DevComponents.DotNetBar.PanelEx(100) {}
        CARD3_DO_Statue_Array(0) = CARD3_DO_Statue_0
        CARD3_DO_Statue_Array(1) = CARD3_DO_Statue_1
        CARD3_DO_Statue_Array(2) = CARD3_DO_Statue_2
        CARD3_DO_Statue_Array(3) = CARD3_DO_Statue_3
        CARD3_DO_Statue_Array(4) = CARD3_DO_Statue_4
        CARD3_DO_Statue_Array(5) = CARD3_DO_Statue_5
        CARD3_DO_Statue_Array(6) = CARD3_DO_Statue_6
        CARD3_DO_Statue_Array(7) = CARD3_DO_Statue_7
        CARD3_DO_Statue_Array(8) = CARD3_DO_Statue_8
        CARD3_DO_Statue_Array(9) = CARD3_DO_Statue_9
        CARD3_DO_Statue_Array(10) = CARD3_DO_Statue_10
        CARD3_DO_Statue_Array(11) = CARD3_DO_Statue_11
        CARD3_DO_Statue_Array(12) = CARD3_DO_Statue_12
        CARD3_DO_Statue_Array(13) = CARD3_DO_Statue_13
        CARD3_DO_Statue_Array(14) = CARD3_DO_Statue_14
        CARD3_DO_Statue_Array(15) = CARD3_DO_Statue_15
        CARD3_DO_Statue_Array(16) = CARD3_DO_Statue_16
        CARD3_DO_Statue_Array(17) = CARD3_DO_Statue_17
        CARD3_DO_Statue_Array(18) = CARD3_DO_Statue_18
        CARD3_DO_Statue_Array(19) = CARD3_DO_Statue_19
        CARD3_DO_Statue_Array(20) = CARD3_DO_Statue_20
        CARD3_DO_Statue_Array(21) = CARD3_DO_Statue_21
        CARD3_DO_Statue_Array(22) = CARD3_DO_Statue_22
        CARD3_DO_Statue_Array(23) = CARD3_DO_Statue_23
    End Sub
    ''' <summary>
    ''' 定义点击输出
    ''' </summary>
    ''' <remarks></remarks>
    Sub Dim_CARD_CHECK()
        '//卡0
        CARD0_CHECK_Array = New System.Windows.Forms.CheckBox(100) {}
        CARD0_CHECK_Array(0) = CheckBox0
        CARD0_CHECK_Array(1) = CheckBox1
        CARD0_CHECK_Array(2) = CheckBox2
        CARD0_CHECK_Array(3) = CheckBox3
        CARD0_CHECK_Array(4) = CheckBox4
        CARD0_CHECK_Array(5) = CheckBox5
        CARD0_CHECK_Array(6) = CheckBox6
        CARD0_CHECK_Array(7) = CheckBox7
        CARD0_CHECK_Array(8) = CheckBox8
        CARD0_CHECK_Array(9) = CheckBox9
        CARD0_CHECK_Array(10) = CheckBox10
        CARD0_CHECK_Array(11) = CheckBox11
        CARD0_CHECK_Array(12) = CheckBox12
        CARD0_CHECK_Array(13) = CheckBox13
        CARD0_CHECK_Array(14) = CheckBox14
        CARD0_CHECK_Array(15) = CheckBox15
        CARD0_CHECK_Array(16) = CheckBox16
        CARD0_CHECK_Array(17) = CheckBox17
        CARD0_CHECK_Array(18) = CheckBox18
        CARD0_CHECK_Array(19) = CheckBox19
        CARD0_CHECK_Array(20) = CheckBox20
        CARD0_CHECK_Array(21) = CheckBox21
        CARD0_CHECK_Array(22) = CheckBox22
        CARD0_CHECK_Array(23) = CheckBox23
        '//卡1
        CARD1_CHECK_Array = New System.Windows.Forms.CheckBox(100) {}
        CARD1_CHECK_Array(0) = CheckBox24
        CARD1_CHECK_Array(1) = CheckBox25
        CARD1_CHECK_Array(2) = CheckBox26
        CARD1_CHECK_Array(3) = CheckBox27
        CARD1_CHECK_Array(4) = CheckBox28
        CARD1_CHECK_Array(5) = CheckBox29
        CARD1_CHECK_Array(6) = CheckBox30
        CARD1_CHECK_Array(7) = CheckBox31
        CARD1_CHECK_Array(8) = CheckBox32
        CARD1_CHECK_Array(9) = CheckBox33
        CARD1_CHECK_Array(10) = CheckBox34
        CARD1_CHECK_Array(11) = CheckBox35
        CARD1_CHECK_Array(12) = CheckBox36
        CARD1_CHECK_Array(13) = CheckBox37
        CARD1_CHECK_Array(14) = CheckBox38
        CARD1_CHECK_Array(15) = CheckBox39
        CARD1_CHECK_Array(16) = CheckBox40
        CARD1_CHECK_Array(17) = CheckBox41
        CARD1_CHECK_Array(18) = CheckBox42
        CARD1_CHECK_Array(19) = CheckBox43
        CARD1_CHECK_Array(20) = CheckBox44
        CARD1_CHECK_Array(21) = CheckBox45
        CARD1_CHECK_Array(22) = CheckBox46
        CARD1_CHECK_Array(23) = CheckBox47
        '//卡2
        CARD2_CHECK_Array = New System.Windows.Forms.CheckBox(100) {}
        CARD2_CHECK_Array(0) = CheckBox24
        CARD2_CHECK_Array(1) = CheckBox25
        CARD2_CHECK_Array(2) = CheckBox26
        CARD2_CHECK_Array(3) = CheckBox27
        CARD2_CHECK_Array(4) = CheckBox28
        CARD2_CHECK_Array(5) = CheckBox29
        CARD2_CHECK_Array(6) = CheckBox30
        CARD2_CHECK_Array(7) = CheckBox31
        CARD2_CHECK_Array(8) = CheckBox32
        CARD2_CHECK_Array(9) = CheckBox33
        CARD2_CHECK_Array(10) = CheckBox34
        CARD2_CHECK_Array(11) = CheckBox35
        CARD2_CHECK_Array(12) = CheckBox36
        CARD2_CHECK_Array(13) = CheckBox37
        CARD2_CHECK_Array(14) = CheckBox38
        CARD2_CHECK_Array(15) = CheckBox39
        CARD2_CHECK_Array(16) = CheckBox40
        CARD2_CHECK_Array(17) = CheckBox41
        CARD2_CHECK_Array(18) = CheckBox42
        CARD2_CHECK_Array(19) = CheckBox43
        CARD2_CHECK_Array(20) = CheckBox44
        CARD2_CHECK_Array(21) = CheckBox45
        CARD2_CHECK_Array(22) = CheckBox46
        CARD2_CHECK_Array(23) = CheckBox47
        '//卡3
        CARD3_CHECK_Array = New System.Windows.Forms.CheckBox(100) {}
        CARD3_CHECK_Array(0) = CheckBox24
        CARD3_CHECK_Array(1) = CheckBox25
        CARD3_CHECK_Array(2) = CheckBox26
        CARD3_CHECK_Array(3) = CheckBox27
        CARD3_CHECK_Array(4) = CheckBox28
        CARD3_CHECK_Array(5) = CheckBox29
        CARD3_CHECK_Array(6) = CheckBox30
        CARD3_CHECK_Array(7) = CheckBox31
        CARD3_CHECK_Array(8) = CheckBox32
        CARD3_CHECK_Array(9) = CheckBox33
        CARD3_CHECK_Array(10) = CheckBox34
        CARD3_CHECK_Array(11) = CheckBox35
        CARD3_CHECK_Array(12) = CheckBox36
        CARD3_CHECK_Array(13) = CheckBox37
        CARD3_CHECK_Array(14) = CheckBox38
        CARD3_CHECK_Array(15) = CheckBox39
        CARD3_CHECK_Array(16) = CheckBox40
        CARD3_CHECK_Array(17) = CheckBox41
        CARD3_CHECK_Array(18) = CheckBox42
        CARD3_CHECK_Array(19) = CheckBox43
        CARD3_CHECK_Array(20) = CheckBox44
        CARD3_CHECK_Array(21) = CheckBox45
        CARD3_CHECK_Array(22) = CheckBox46
        CARD3_CHECK_Array(23) = CheckBox47
    End Sub

#End Region

    Public Sub Close_ADLink_IO()
        SQLCON.Close_DataBase()
    End Sub

    Public Sub Init_ADLink_IO()
        Card_Init_OK = Main.Card_Init_OK
        Card_NO = Main.Card_NO
        For i As Integer = 1 To 4
            Select Case Card_NO
                Case 0
                    GroupPanel_Card0.Enabled = False
                    GroupPanel_Card1.Enabled = False
                    GroupPanel_Card2.Enabled = False
                    GroupPanel_Card3.Enabled = False
                Case 1
                    GroupPanel_Card1.Enabled = False
                    GroupPanel_Card2.Enabled = False
                    GroupPanel_Card3.Enabled = False

                    GP_CARD_DI_1.Enabled = False
                    GP_CARD_DO_1.Enabled = False
                    GP_CARD_DI_2.Enabled = False
                    GP_CARD_DO_2.Enabled = False
                    GP_CARD_DI_3.Enabled = False
                    GP_CARD_DO_3.Enabled = False
                Case 2
                    GroupPanel_Card2.Enabled = False
                    GroupPanel_Card3.Enabled = False

                    GP_CARD_DI_2.Enabled = False
                    GP_CARD_DO_2.Enabled = False
                    GP_CARD_DI_3.Enabled = False
                    GP_CARD_DO_3.Enabled = False
                Case 3
                    GroupPanel_Card3.Enabled = False

                    GP_CARD_DI_3.Enabled = False
                    GP_CARD_DO_3.Enabled = False
                Case 4

            End Select
        Next

        Dim_CARD_DI_Name()
        Dim_CARD_DI_Statue()
        Dim_CARD_CHECK()
        Dim_CARD_ORGPELMEL()

        Card = New Adlink_Card
        If SQLCON.DataBase_Initialization("127.0.0.1", "sa", "123", "GENERAL_ASSEMBLY") = True Then
            For i As Int16 = 0 To 23
                PE_Value(i, 0) = CARD0_DI_Name_Array(i)
                PE_Value(i, 1) = CARD1_DI_Name_Array(i)
                PE_Value(i, 2) = CARD0_DO_Name_Array(i)
                PE_Value(i, 3) = CARD1_DO_Name_Array(i)

                PE_Value_1(i, 0) = CARD2_DI_Name_Array(i)
                PE_Value_1(i, 1) = CARD3_DI_Name_Array(i)
                PE_Value_1(i, 2) = CARD2_DO_Name_Array(i)
                PE_Value_1(i, 3) = CARD3_DO_Name_Array(i)
            Next
            Read_ADLINK_CARDIO()
        End If

    End Sub

    Private Sub Adlink_DIO_Dialog_FormClosing(sender As Object, e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Close_ADLink_IO()
        Bool_Run_IO = False
        If BK_IO_Thread IsNot Nothing Then
            If BK_IO_Thread.IsAlive = True Then
                BK_IO_Thread.Abort()
            End If
        End If
    End Sub
    Private Sub Main_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Control.CheckForIllegalCrossThreadCalls = False
   
        Me.Text = "控制卡IO调试助手 V" & ProductVersion
        Init_ADLink_IO() '初始化IO

        Bool_Run_IO = True
        BK_IO_Thread = New Thread(AddressOf BK_RUN_IO)
        BK_IO_Thread.Start()
    End Sub

    Function Read_ADLINK_CARDIO() As Boolean
        Try
            SQLCON.Query_ADLINK_IO(PE_Value, Value, PE_Value_1, Value_1)
            Return True
        Catch ex As Exception
            MsgBox(ex.ToString)
            Return False
        End Try
    End Function

    Sub BK_RUN_IO()
        While Bool_Run_IO
            Thread.Sleep(30)
            If Card_Init_OK = True Then
                For i As Integer = 0 To 23 '24个输入输出点
                    For j As Integer = 1 To Card_NO
                        '//获取输入输出状态
                        Select Case j
                            Case 1 '//1号轴
                                If Card.Get_DI_sample(Card_ID_00, i) = True Then
                                    CARD0_DI_Statue_Array(i).Style.BackColor1.Color = Color.Red
                                    CARD0_DI_Statue_Array(i).Style.BackColor2.Color = Color.Red
                                Else
                                    CARD0_DI_Statue_Array(i).Style.BackColor1.Color = Color.Gray
                                    CARD0_DI_Statue_Array(i).Style.BackColor2.Color = Color.Gray
                                End If
                                If Card.Get_DO_Bit(Card_ID_00, i) = True Then
                                    CARD0_DO_Statue_Array(i).Style.BackColor1.Color = Color.Red
                                    CARD0_DO_Statue_Array(i).Style.BackColor2.Color = Color.Red
                                Else
                                    CARD0_DO_Statue_Array(i).Style.BackColor1.Color = Color.Gray
                                    CARD0_DO_Statue_Array(i).Style.BackColor2.Color = Color.Gray
                                End If
                            Case 2 '//2号轴
                                If Card.Get_DI_sample(Card_ID_01, i) = True Then
                                    CARD1_DI_Statue_Array(i).Style.BackColor1.Color = Color.Red
                                    CARD1_DI_Statue_Array(i).Style.BackColor2.Color = Color.Red
                                Else
                                    CARD1_DI_Statue_Array(i).Style.BackColor1.Color = Color.Gray
                                    CARD1_DI_Statue_Array(i).Style.BackColor2.Color = Color.Gray
                                End If
                                If Card.Get_DO_Bit(Card_ID_01, i) = True Then
                                    CARD1_DO_Statue_Array(i).Style.BackColor1.Color = Color.Red
                                    CARD1_DO_Statue_Array(i).Style.BackColor2.Color = Color.Red
                                Else
                                    CARD1_DO_Statue_Array(i).Style.BackColor1.Color = Color.Gray
                                    CARD1_DO_Statue_Array(i).Style.BackColor2.Color = Color.Gray
                                End If
                            Case 3 '//3号轴
                                If Card.Get_DI_sample(Card_ID_01, i) = True Then
                                    CARD2_DI_Statue_Array(i).Style.BackColor1.Color = Color.Red
                                    CARD2_DI_Statue_Array(i).Style.BackColor2.Color = Color.Red
                                Else
                                    CARD2_DI_Statue_Array(i).Style.BackColor1.Color = Color.Gray
                                    CARD2_DI_Statue_Array(i).Style.BackColor2.Color = Color.Gray
                                End If
                                If Card.Get_DO_Bit(Card_ID_01, i) = True Then
                                    CARD2_DO_Statue_Array(i).Style.BackColor1.Color = Color.Red
                                    CARD2_DO_Statue_Array(i).Style.BackColor2.Color = Color.Red
                                Else
                                    CARD2_DO_Statue_Array(i).Style.BackColor1.Color = Color.Gray
                                    CARD2_DO_Statue_Array(i).Style.BackColor2.Color = Color.Gray
                                End If
                            Case 4 '//4号轴
                                If Card.Get_DI_sample(Card_ID_01, i) = True Then
                                    CARD3_DI_Statue_Array(i).Style.BackColor1.Color = Color.Red
                                    CARD3_DI_Statue_Array(i).Style.BackColor2.Color = Color.Red
                                Else
                                    CARD3_DI_Statue_Array(i).Style.BackColor1.Color = Color.Gray
                                    CARD3_DI_Statue_Array(i).Style.BackColor2.Color = Color.Gray
                                End If
                                If Card.Get_DO_Bit(Card_ID_01, i) = True Then
                                    CARD3_DO_Statue_Array(i).Style.BackColor1.Color = Color.Red
                                    CARD3_DO_Statue_Array(i).Style.BackColor2.Color = Color.Red
                                Else
                                    CARD3_DO_Statue_Array(i).Style.BackColor1.Color = Color.Gray
                                    CARD3_DO_Statue_Array(i).Style.BackColor2.Color = Color.Gray
                                End If
                        End Select

                        '//获取原点和正负极限，1张板卡最多只有8个轴
                        If i < Card_Type Then
                            Select Case j
                                Case 1 '//1个轴
                                    If Card.Read_Motion_IO(i, MIO_PEL) = True Then
                                        CARD0_PEL_Array(i).Style.BackColor1.Color = Color.Red
                                        CARD0_PEL_Array(i).Style.BackColor2.Color = Color.Red
                                    Else
                                        CARD0_PEL_Array(i).Style.BackColor1.Color = Color.Gray
                                        CARD0_PEL_Array(i).Style.BackColor2.Color = Color.Gray
                                    End If
                                    If Card.Read_Motion_IO(i, MIO_ORG) = True Then
                                        CARD0_ORG_Array(i).Style.BackColor1.Color = Color.Red
                                        CARD0_ORG_Array(i).Style.BackColor2.Color = Color.Red
                                    Else
                                        CARD0_ORG_Array(i).Style.BackColor1.Color = Color.Gray
                                        CARD0_ORG_Array(i).Style.BackColor2.Color = Color.Gray
                                    End If
                                    If Card.Read_Motion_IO(i, MIO_MEL) = True Then
                                        CARD0_MEL_Array(i).Style.BackColor1.Color = Color.Red
                                        CARD0_MEL_Array(i).Style.BackColor2.Color = Color.Red
                                    Else
                                        CARD0_MEL_Array(i).Style.BackColor1.Color = Color.Gray
                                        CARD0_MEL_Array(i).Style.BackColor2.Color = Color.Gray
                                    End If
                                Case 2 '//2个轴
                                    If Card.Read_Motion_IO(i + 8, MIO_PEL) = True Then
                                        CARD1_PEL_Array(i).Style.BackColor1.Color = Color.Red
                                        CARD1_PEL_Array(i).Style.BackColor2.Color = Color.Red
                                    Else
                                        CARD1_PEL_Array(i).Style.BackColor1.Color = Color.Gray
                                        CARD1_PEL_Array(i).Style.BackColor2.Color = Color.Gray
                                    End If
                                    If Card.Read_Motion_IO(i + 8, MIO_ORG) = True Then
                                        CARD1_ORG_Array(i).Style.BackColor1.Color = Color.Red
                                        CARD1_ORG_Array(i).Style.BackColor2.Color = Color.Red
                                    Else
                                        CARD1_ORG_Array(i).Style.BackColor1.Color = Color.Gray
                                        CARD1_ORG_Array(i).Style.BackColor2.Color = Color.Gray
                                    End If
                                    If Card.Read_Motion_IO(i + 8, MIO_MEL) = True Then
                                        CARD1_MEL_Array(i).Style.BackColor1.Color = Color.Red
                                        CARD1_MEL_Array(i).Style.BackColor2.Color = Color.Red
                                    Else
                                        CARD1_MEL_Array(i).Style.BackColor1.Color = Color.Gray
                                        CARD1_MEL_Array(i).Style.BackColor2.Color = Color.Gray
                                    End If
                                Case 3 '//3个轴
                                    If Card.Read_Motion_IO(i + 8, MIO_PEL) = True Then
                                        CARD2_PEL_Array(i).Style.BackColor1.Color = Color.Red
                                        CARD2_PEL_Array(i).Style.BackColor2.Color = Color.Red
                                    Else
                                        CARD2_PEL_Array(i).Style.BackColor1.Color = Color.Gray
                                        CARD2_PEL_Array(i).Style.BackColor2.Color = Color.Gray
                                    End If
                                    If Card.Read_Motion_IO(i + 8, MIO_ORG) = True Then
                                        CARD2_ORG_Array(i).Style.BackColor1.Color = Color.Red
                                        CARD2_ORG_Array(i).Style.BackColor2.Color = Color.Red
                                    Else
                                        CARD2_ORG_Array(i).Style.BackColor1.Color = Color.Gray
                                        CARD2_ORG_Array(i).Style.BackColor2.Color = Color.Gray
                                    End If
                                    If Card.Read_Motion_IO(i + 8, MIO_MEL) = True Then
                                        CARD2_MEL_Array(i).Style.BackColor1.Color = Color.Red
                                        CARD2_MEL_Array(i).Style.BackColor2.Color = Color.Red
                                    Else
                                        CARD2_MEL_Array(i).Style.BackColor1.Color = Color.Gray
                                        CARD2_MEL_Array(i).Style.BackColor2.Color = Color.Gray
                                    End If
                                Case 4 '//4个轴
                                    If Card.Read_Motion_IO(i + 8, MIO_PEL) = True Then
                                        CARD3_PEL_Array(i).Style.BackColor1.Color = Color.Red
                                        CARD3_PEL_Array(i).Style.BackColor2.Color = Color.Red
                                    Else
                                        CARD3_PEL_Array(i).Style.BackColor1.Color = Color.Gray
                                        CARD3_PEL_Array(i).Style.BackColor2.Color = Color.Gray
                                    End If
                                    If Card.Read_Motion_IO(i + 8, MIO_ORG) = True Then
                                        CARD3_ORG_Array(i).Style.BackColor1.Color = Color.Red
                                        CARD3_ORG_Array(i).Style.BackColor2.Color = Color.Red
                                    Else
                                        CARD3_ORG_Array(i).Style.BackColor1.Color = Color.Gray
                                        CARD3_ORG_Array(i).Style.BackColor2.Color = Color.Gray
                                    End If
                                    If Card.Read_Motion_IO(i + 8, MIO_MEL) = True Then
                                        CARD3_MEL_Array(i).Style.BackColor1.Color = Color.Red
                                        CARD3_MEL_Array(i).Style.BackColor2.Color = Color.Red
                                    Else
                                        CARD3_MEL_Array(i).Style.BackColor1.Color = Color.Gray
                                        CARD3_MEL_Array(i).Style.BackColor2.Color = Color.Gray
                                    End If
                            End Select
                        End If
                    Next
                Next
            End If
        End While
    End Sub

#Region "Click事件"
    Private Sub CheckBox0_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox0.CheckedChanged
        If Card_Init_OK = True Then
            Dim NO As Integer = 0
            If CARD0_CHECK_Array(NO).Checked = False Then
                Card.Set_DO_Bit(Card_ID_00, NO, False)
            Else
                Card.Set_DO_Bit(Card_ID_00, NO, True)
            End If
        End If
    End Sub

    Private Sub CheckBox1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox1.CheckedChanged
        If Card_Init_OK = True Then
            Dim NO As Integer = 1
            If CARD0_CHECK_Array(NO).Checked = False Then
                Card.Set_DO_Bit(Card_ID_00, NO, False)
            Else
                Card.Set_DO_Bit(Card_ID_00, NO, True)
            End If
        End If
    End Sub

    Private Sub CheckBox2_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox2.CheckedChanged
        If Card_Init_OK = True Then
            Dim NO As Integer = 2
            If CARD0_CHECK_Array(NO).Checked = False Then
                Card.Set_DO_Bit(Card_ID_00, NO, False)
            Else
                Card.Set_DO_Bit(Card_ID_00, NO, True)
            End If
        End If
    End Sub

    Private Sub CheckBox3_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox3.CheckedChanged
        If Card_Init_OK = True Then
            Dim NO As Integer = 3
            If CARD0_CHECK_Array(NO).Checked = False Then
                Card.Set_DO_Bit(Card_ID_00, NO, False)
            Else
                Card.Set_DO_Bit(Card_ID_00, NO, True)
            End If
        End If
    End Sub

    Private Sub CheckBox4_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox4.CheckedChanged
        If Card_Init_OK = True Then
            Dim NO As Integer = 4
            If CARD0_CHECK_Array(NO).Checked = False Then
                Card.Set_DO_Bit(Card_ID_00, NO, False)
            Else
                Card.Set_DO_Bit(Card_ID_00, NO, True)
            End If
        End If
    End Sub

    Private Sub CheckBox5_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox5.CheckedChanged
        If Card_Init_OK = True Then
            Dim NO As Integer = 5
            If CARD0_CHECK_Array(NO).Checked = False Then
                Card.Set_DO_Bit(Card_ID_00, NO, False)
            Else
                Card.Set_DO_Bit(Card_ID_00, NO, True)
            End If
        End If
    End Sub

    Private Sub CheckBox6_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox6.CheckedChanged
        If Card_Init_OK = True Then
            Dim NO As Integer = 6
            If CARD0_CHECK_Array(NO).Checked = False Then
                Card.Set_DO_Bit(Card_ID_00, NO, False)
            Else
                Card.Set_DO_Bit(Card_ID_00, NO, True)
            End If
        End If
    End Sub

    Private Sub CheckBox7_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox7.CheckedChanged
        If Card_Init_OK = True Then
            Dim NO As Integer = 7
            If CARD0_CHECK_Array(NO).Checked = False Then
                Card.Set_DO_Bit(Card_ID_00, NO, False)
            Else
                Card.Set_DO_Bit(Card_ID_00, NO, True)
            End If
        End If
    End Sub

    Private Sub CheckBox8_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox8.CheckedChanged
        If Card_Init_OK = True Then
            Dim NO As Integer = 8
            If CARD0_CHECK_Array(NO).Checked = False Then
                Card.Set_DO_Bit(Card_ID_00, NO, False)
            Else
                Card.Set_DO_Bit(Card_ID_00, NO, True)
            End If
        End If
    End Sub

    Private Sub CheckBox9_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox9.CheckedChanged
        If Card_Init_OK = True Then
            Dim NO As Integer = 9
            If CARD0_CHECK_Array(NO).Checked = False Then
                Card.Set_DO_Bit(Card_ID_00, NO, False)
            Else
                Card.Set_DO_Bit(Card_ID_00, NO, True)
            End If
        End If
    End Sub

    Private Sub CheckBox10_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox10.CheckedChanged
        If Card_Init_OK = True Then
            Dim NO As Integer = 10
            If CARD0_CHECK_Array(NO).Checked = False Then
                Card.Set_DO_Bit(Card_ID_00, NO, False)
            Else
                Card.Set_DO_Bit(Card_ID_00, NO, True)
            End If
        End If
    End Sub

    Private Sub CheckBox11_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox11.CheckedChanged
        If Card_Init_OK = True Then
            Dim NO As Integer = 11
            If CARD0_CHECK_Array(NO).Checked = False Then
                Card.Set_DO_Bit(Card_ID_00, NO, False)
            Else
                Card.Set_DO_Bit(Card_ID_00, NO, True)
            End If
        End If
    End Sub

    Private Sub CheckBox12_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox12.CheckedChanged
        If Card_Init_OK = True Then
            Dim NO As Integer = 12
            If CARD0_CHECK_Array(NO).Checked = False Then
                Card.Set_DO_Bit(Card_ID_00, NO, False)
            Else
                Card.Set_DO_Bit(Card_ID_00, NO, True)
            End If
        End If
    End Sub

    Private Sub CheckBox13_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox13.CheckedChanged
        If Card_Init_OK = True Then
            Dim NO As Integer = 13
            If CARD0_CHECK_Array(NO).Checked = False Then
                Card.Set_DO_Bit(Card_ID_00, NO, False)
            Else
                Card.Set_DO_Bit(Card_ID_00, NO, True)
            End If
        End If
    End Sub

    Private Sub CheckBox14_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox14.CheckedChanged
        If Card_Init_OK = True Then
            Dim NO As Integer = 14
            If CARD0_CHECK_Array(NO).Checked = False Then
                Card.Set_DO_Bit(Card_ID_00, NO, False)
            Else
                Card.Set_DO_Bit(Card_ID_00, NO, True)
            End If
        End If
    End Sub

    Private Sub CheckBox15_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox15.CheckedChanged
        If Card_Init_OK = True Then
            Dim NO As Integer = 15
            If CARD0_CHECK_Array(NO).Checked = False Then
                Card.Set_DO_Bit(Card_ID_00, NO, False)
            Else
                Card.Set_DO_Bit(Card_ID_00, NO, True)
            End If
        End If
    End Sub

    Private Sub CheckBox16_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox16.CheckedChanged
        If Card_Init_OK = True Then
            Dim NO As Integer = 16
            If CARD0_CHECK_Array(NO).Checked = False Then
                Card.Set_DO_Bit(Card_ID_00, NO, False)
            Else
                Card.Set_DO_Bit(Card_ID_00, NO, True)
            End If
        End If
    End Sub

    Private Sub CheckBox17_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox17.CheckedChanged
        If Card_Init_OK = True Then
            Dim NO As Integer = 17
            If CARD0_CHECK_Array(NO).Checked = False Then
                Card.Set_DO_Bit(Card_ID_00, NO, False)
            Else
                Card.Set_DO_Bit(Card_ID_00, NO, True)
            End If
        End If
    End Sub

    Private Sub CheckBox18_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox18.CheckedChanged
        If Card_Init_OK = True Then
            Dim NO As Integer = 18
            If CARD0_CHECK_Array(NO).Checked = False Then
                Card.Set_DO_Bit(Card_ID_00, NO, False)
            Else
                Card.Set_DO_Bit(Card_ID_00, NO, True)
            End If
        End If
    End Sub

    Private Sub CheckBox19_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox19.CheckedChanged
        If Card_Init_OK = True Then
            Dim NO As Integer = 19
            If CARD0_CHECK_Array(NO).Checked = False Then
                Card.Set_DO_Bit(Card_ID_00, NO, False)
            Else
                Card.Set_DO_Bit(Card_ID_00, NO, True)
            End If
        End If
    End Sub

    Private Sub CheckBox20_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox20.CheckedChanged
        If Card_Init_OK = True Then
            Dim NO As Integer = 20
            If CARD0_CHECK_Array(NO).Checked = False Then
                Card.Set_DO_Bit(Card_ID_00, NO, False)
            Else
                Card.Set_DO_Bit(Card_ID_00, NO, True)
            End If
        End If
    End Sub

    Private Sub CheckBox21_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox21.CheckedChanged
        If Card_Init_OK = True Then
            Dim NO As Integer = 21
            If CARD0_CHECK_Array(NO).Checked = False Then
                Card.Set_DO_Bit(Card_ID_00, NO, False)
            Else
                Card.Set_DO_Bit(Card_ID_00, NO, True)
            End If
        End If
    End Sub

    Private Sub CheckBox22_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox22.CheckedChanged
        If Card_Init_OK = True Then
            Dim NO As Integer = 22
            If CARD0_CHECK_Array(NO).Checked = False Then
                Card.Set_DO_Bit(Card_ID_00, NO, False)
            Else
                Card.Set_DO_Bit(Card_ID_00, NO, True)
            End If
        End If
    End Sub

    Private Sub CheckBox23_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox23.CheckedChanged
        If Card_Init_OK = True Then
            Dim NO As Integer = 23
            If CARD0_CHECK_Array(NO).Checked = False Then
                Card.Set_DO_Bit(Card_ID_00, NO, False)
            Else
                Card.Set_DO_Bit(Card_ID_00, NO, True)
            End If
        End If
    End Sub

    Private Sub CheckBox24_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox24.CheckedChanged
        If Card_Init_OK = True Then
            Dim NO As Integer = 0
            If CARD1_CHECK_Array(NO).Checked = False Then
                Card.Set_DO_Bit(Card_ID_01, NO, False)
            Else
                Card.Set_DO_Bit(Card_ID_01, NO, True)
            End If
        End If
    End Sub

    Private Sub CheckBox25_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox25.CheckedChanged
        If Card_Init_OK = True Then
            Dim NO As Integer = 1
            If CARD1_CHECK_Array(NO).Checked = False Then
                Card.Set_DO_Bit(Card_ID_01, NO, False)
            Else
                Card.Set_DO_Bit(Card_ID_01, NO, True)
            End If
        End If
    End Sub

    Private Sub CheckBox26_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox26.CheckedChanged
        If Card_Init_OK = True Then
            Dim NO As Integer = 2
            If CARD1_CHECK_Array(NO).Checked = False Then
                Card.Set_DO_Bit(Card_ID_01, NO, False)
            Else
                Card.Set_DO_Bit(Card_ID_01, NO, True)
            End If
        End If
    End Sub

    Private Sub CheckBox27_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox27.CheckedChanged
        If Card_Init_OK = True Then
            Dim NO As Integer = 3
            If CARD1_CHECK_Array(NO).Checked = False Then
                Card.Set_DO_Bit(Card_ID_01, NO, False)
            Else
                Card.Set_DO_Bit(Card_ID_01, NO, True)
            End If
        End If
    End Sub

    Private Sub CheckBox28_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox28.CheckedChanged
        If Card_Init_OK = True Then
            Dim NO As Integer = 4
            If CARD1_CHECK_Array(NO).Checked = False Then
                Card.Set_DO_Bit(Card_ID_01, NO, False)
            Else
                Card.Set_DO_Bit(Card_ID_01, NO, True)
            End If
        End If
    End Sub

    Private Sub CheckBox29_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox29.CheckedChanged
        If Card_Init_OK = True Then
            Dim NO As Integer = 5
            If CARD1_CHECK_Array(NO).Checked = False Then
                Card.Set_DO_Bit(Card_ID_01, NO, False)
            Else
                Card.Set_DO_Bit(Card_ID_01, NO, True)
            End If
        End If
    End Sub


    Private Sub CheckBox30_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox30.CheckedChanged
        If Card_Init_OK = True Then
            Dim NO As Integer = 6
            If CARD1_CHECK_Array(NO).Checked = False Then
                Card.Set_DO_Bit(Card_ID_01, NO, False)
            Else
                Card.Set_DO_Bit(Card_ID_01, NO, True)
            End If
        End If
    End Sub

    Private Sub CheckBox31_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox31.CheckedChanged
        If Card_Init_OK = True Then
            Dim NO As Integer = 7
            If CARD1_CHECK_Array(NO).Checked = False Then
                Card.Set_DO_Bit(Card_ID_01, NO, False)
            Else
                Card.Set_DO_Bit(Card_ID_01, NO, True)
            End If
        End If
    End Sub

    Private Sub CheckBox32_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox32.CheckedChanged
        If Card_Init_OK = True Then
            Dim NO As Integer = 8
            If CARD1_CHECK_Array(NO).Checked = False Then
                Card.Set_DO_Bit(Card_ID_01, NO, False)
            Else
                Card.Set_DO_Bit(Card_ID_01, NO, True)
            End If
        End If
    End Sub

    Private Sub CheckBox33_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox33.CheckedChanged
        If Card_Init_OK = True Then
            Dim NO As Integer = 9
            If CARD1_CHECK_Array(NO).Checked = False Then
                Card.Set_DO_Bit(Card_ID_01, NO, False)
            Else
                Card.Set_DO_Bit(Card_ID_01, NO, True)
            End If
        End If
    End Sub

    Private Sub CheckBox34_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox34.CheckedChanged
        If Card_Init_OK = True Then
            Dim NO As Integer = 10
            If CARD1_CHECK_Array(NO).Checked = False Then
                Card.Set_DO_Bit(Card_ID_01, NO, False)
            Else
                Card.Set_DO_Bit(Card_ID_01, NO, True)
            End If
        End If
    End Sub

    Private Sub CheckBox35_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox35.CheckedChanged
        If Card_Init_OK = True Then
            Dim NO As Integer = 11
            If CARD1_CHECK_Array(NO).Checked = False Then
                Card.Set_DO_Bit(Card_ID_01, NO, False)
            Else
                Card.Set_DO_Bit(Card_ID_01, NO, True)
            End If
        End If
    End Sub

    Private Sub CheckBox36_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox36.CheckedChanged
        If Card_Init_OK = True Then
            Dim NO As Integer = 12
            If CARD1_CHECK_Array(NO).Checked = False Then
                Card.Set_DO_Bit(Card_ID_01, NO, False)
            Else
                Card.Set_DO_Bit(Card_ID_01, NO, True)
            End If
        End If
    End Sub

    Private Sub CheckBox37_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox37.CheckedChanged
        If Card_Init_OK = True Then
            Dim NO As Integer = 13
            If CARD1_CHECK_Array(NO).Checked = False Then
                Card.Set_DO_Bit(Card_ID_01, NO, False)
            Else
                Card.Set_DO_Bit(Card_ID_01, NO, True)
            End If
        End If
    End Sub

    Private Sub CheckBox38_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox38.CheckedChanged
        If Card_Init_OK = True Then
            Dim NO As Integer = 14
            If CARD1_CHECK_Array(NO).Checked = False Then
                Card.Set_DO_Bit(Card_ID_01, NO, False)
            Else
                Card.Set_DO_Bit(Card_ID_01, NO, True)
            End If
        End If
    End Sub

    Private Sub CheckBox39_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox39.CheckedChanged
        If Card_Init_OK = True Then
            Dim NO As Integer = 15
            If CARD1_CHECK_Array(NO).Checked = False Then
                Card.Set_DO_Bit(Card_ID_01, NO, False)
            Else
                Card.Set_DO_Bit(Card_ID_01, NO, True)
            End If
        End If
    End Sub

    Private Sub CheckBox40_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox40.CheckedChanged
        If Card_Init_OK = True Then
            Dim NO As Integer = 16
            If CARD1_CHECK_Array(NO).Checked = False Then
                Card.Set_DO_Bit(Card_ID_01, NO, False)
            Else
                Card.Set_DO_Bit(Card_ID_01, NO, True)
            End If
        End If
    End Sub

    Private Sub CheckBox41_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox41.CheckedChanged
        If Card_Init_OK = True Then
            Dim NO As Integer = 17
            If CARD1_CHECK_Array(NO).Checked = False Then
                Card.Set_DO_Bit(Card_ID_01, NO, False)
            Else
                Card.Set_DO_Bit(Card_ID_01, NO, True)
            End If
        End If
    End Sub

    Private Sub CheckBox42_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox42.CheckedChanged
        If Card_Init_OK = True Then
            Dim NO As Integer = 18
            If CARD1_CHECK_Array(NO).Checked = False Then
                Card.Set_DO_Bit(Card_ID_01, NO, False)
            Else
                Card.Set_DO_Bit(Card_ID_01, NO, True)
            End If
        End If
    End Sub

    Private Sub CheckBox43_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox43.CheckedChanged
        If Card_Init_OK = True Then
            Dim NO As Integer = 19
            If CARD1_CHECK_Array(NO).Checked = False Then
                Card.Set_DO_Bit(Card_ID_01, NO, False)
            Else
                Card.Set_DO_Bit(Card_ID_01, NO, True)
            End If
        End If
    End Sub

    Private Sub CheckBox44_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox44.CheckedChanged
        If Card_Init_OK = True Then
            Dim NO As Integer = 20
            If CARD1_CHECK_Array(NO).Checked = False Then
                Card.Set_DO_Bit(Card_ID_01, NO, False)
            Else
                Card.Set_DO_Bit(Card_ID_01, NO, True)
            End If
        End If
    End Sub

    Private Sub CheckBox45_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox45.CheckedChanged
        If Card_Init_OK = True Then
            Dim NO As Integer = 21
            If CARD1_CHECK_Array(NO).Checked = False Then
                Card.Set_DO_Bit(Card_ID_01, NO, False)
            Else
                Card.Set_DO_Bit(Card_ID_01, NO, True)
            End If
        End If
    End Sub

    Private Sub CheckBox46_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox46.CheckedChanged
        If Card_Init_OK = True Then
            Dim NO As Integer = 22
            If CARD1_CHECK_Array(NO).Checked = False Then
                Card.Set_DO_Bit(Card_ID_01, NO, False)
            Else
                Card.Set_DO_Bit(Card_ID_01, NO, True)
            End If
        End If
    End Sub

    Private Sub CheckBox47_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox47.CheckedChanged
        If Card_Init_OK = True Then
            Dim NO As Integer = 23
            If CARD1_CHECK_Array(NO).Checked = False Then
                Card.Set_DO_Bit(Card_ID_01, NO, False)
            Else
                Card.Set_DO_Bit(Card_ID_01, NO, True)
            End If
        End If
    End Sub


    '卡2和3
    Private Sub CheckBoxx0_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBoxx0.CheckedChanged
        If Card_Init_OK = True Then
            Dim NO As Integer = 0
            If CARD2_CHECK_Array(NO).Checked = False Then
                Card.Set_DO_Bit(Card_ID_02, NO, False)
            Else
                Card.Set_DO_Bit(Card_ID_02, NO, True)
            End If
        End If
    End Sub
    Private Sub CheckBoxx1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBoxx1.CheckedChanged
        If Card_Init_OK = True Then
            Dim NO As Integer = 1
            If CARD2_CHECK_Array(NO).Checked = False Then
                Card.Set_DO_Bit(Card_ID_02, NO, False)
            Else
                Card.Set_DO_Bit(Card_ID_02, NO, True)
            End If
        End If
    End Sub
    Private Sub CheckBoxx2_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBoxx2.CheckedChanged
        If Card_Init_OK = True Then
            Dim NO As Integer = 2
            If CARD2_CHECK_Array(NO).Checked = False Then
                Card.Set_DO_Bit(Card_ID_02, NO, False)
            Else
                Card.Set_DO_Bit(Card_ID_02, NO, True)
            End If
        End If
    End Sub
    Private Sub CheckBoxx3_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBoxx3.CheckedChanged
        If Card_Init_OK = True Then
            Dim NO As Integer = 3
            If CARD2_CHECK_Array(NO).Checked = False Then
                Card.Set_DO_Bit(Card_ID_02, NO, False)
            Else
                Card.Set_DO_Bit(Card_ID_02, NO, True)
            End If
        End If
    End Sub
    Private Sub CheckBoxx4_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBoxx4.CheckedChanged
        If Card_Init_OK = True Then
            Dim NO As Integer = 4
            If CARD2_CHECK_Array(NO).Checked = False Then
                Card.Set_DO_Bit(Card_ID_02, NO, False)
            Else
                Card.Set_DO_Bit(Card_ID_02, NO, True)
            End If
        End If
    End Sub
    Private Sub CheckBoxx5_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBoxx5.CheckedChanged
        If Card_Init_OK = True Then
            Dim NO As Integer = 5
            If CARD2_CHECK_Array(NO).Checked = False Then
                Card.Set_DO_Bit(Card_ID_02, NO, False)
            Else
                Card.Set_DO_Bit(Card_ID_02, NO, True)
            End If
        End If
    End Sub
    Private Sub CheckBoxx6_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBoxx6.CheckedChanged
        If Card_Init_OK = True Then
            Dim NO As Integer = 6
            If CARD2_CHECK_Array(NO).Checked = False Then
                Card.Set_DO_Bit(Card_ID_02, NO, False)
            Else
                Card.Set_DO_Bit(Card_ID_02, NO, True)
            End If
        End If
    End Sub
    Private Sub CheckBoxx7_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBoxx7.CheckedChanged
        If Card_Init_OK = True Then
            Dim NO As Integer = 7
            If CARD2_CHECK_Array(NO).Checked = False Then
                Card.Set_DO_Bit(Card_ID_02, NO, False)
            Else
                Card.Set_DO_Bit(Card_ID_02, NO, True)
            End If
        End If
    End Sub
    Private Sub CheckBoxx8_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBoxx8.CheckedChanged
        If Card_Init_OK = True Then
            Dim NO As Integer = 8
            If CARD2_CHECK_Array(NO).Checked = False Then
                Card.Set_DO_Bit(Card_ID_02, NO, False)
            Else
                Card.Set_DO_Bit(Card_ID_02, NO, True)
            End If
        End If
    End Sub
    Private Sub CheckBoxx9_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBoxx9.CheckedChanged
        If Card_Init_OK = True Then
            Dim NO As Integer = 9
            If CARD2_CHECK_Array(NO).Checked = False Then
                Card.Set_DO_Bit(Card_ID_02, NO, False)
            Else
                Card.Set_DO_Bit(Card_ID_02, NO, True)
            End If
        End If
    End Sub
    Private Sub CheckBoxx10_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBoxx10.CheckedChanged
        If Card_Init_OK = True Then
            Dim NO As Integer = 10
            If CARD2_CHECK_Array(NO).Checked = False Then
                Card.Set_DO_Bit(Card_ID_02, NO, False)
            Else
                Card.Set_DO_Bit(Card_ID_02, NO, True)
            End If
        End If
    End Sub
    Private Sub CheckBoxx11_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBoxx11.CheckedChanged
        If Card_Init_OK = True Then
            Dim NO As Integer = 11
            If CARD2_CHECK_Array(NO).Checked = False Then
                Card.Set_DO_Bit(Card_ID_02, NO, False)
            Else
                Card.Set_DO_Bit(Card_ID_02, NO, True)
            End If
        End If
    End Sub
    Private Sub CheckBoxx12_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBoxx12.CheckedChanged
        If Card_Init_OK = True Then
            Dim NO As Integer = 12
            If CARD2_CHECK_Array(NO).Checked = False Then
                Card.Set_DO_Bit(Card_ID_02, NO, False)
            Else
                Card.Set_DO_Bit(Card_ID_02, NO, True)
            End If
        End If
    End Sub
    Private Sub CheckBoxx13_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBoxx13.CheckedChanged
        If Card_Init_OK = True Then
            Dim NO As Integer = 13
            If CARD2_CHECK_Array(NO).Checked = False Then
                Card.Set_DO_Bit(Card_ID_02, NO, False)
            Else
                Card.Set_DO_Bit(Card_ID_02, NO, True)
            End If
        End If
    End Sub
    Private Sub CheckBoxx14_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBoxx14.CheckedChanged
        If Card_Init_OK = True Then
            Dim NO As Integer = 14
            If CARD2_CHECK_Array(NO).Checked = False Then
                Card.Set_DO_Bit(Card_ID_02, NO, False)
            Else
                Card.Set_DO_Bit(Card_ID_02, NO, True)
            End If
        End If
    End Sub
    Private Sub CheckBoxx15_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBoxx15.CheckedChanged
        If Card_Init_OK = True Then
            Dim NO As Integer = 15
            If CARD2_CHECK_Array(NO).Checked = False Then
                Card.Set_DO_Bit(Card_ID_02, NO, False)
            Else
                Card.Set_DO_Bit(Card_ID_02, NO, True)
            End If
        End If
    End Sub
    Private Sub CheckBoxx16_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBoxx16.CheckedChanged
        If Card_Init_OK = True Then
            Dim NO As Integer = 16
            If CARD2_CHECK_Array(NO).Checked = False Then
                Card.Set_DO_Bit(Card_ID_02, NO, False)
            Else
                Card.Set_DO_Bit(Card_ID_02, NO, True)
            End If
        End If
    End Sub
    Private Sub CheckBoxx17_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBoxx17.CheckedChanged
        If Card_Init_OK = True Then
            Dim NO As Integer = 17
            If CARD2_CHECK_Array(NO).Checked = False Then
                Card.Set_DO_Bit(Card_ID_02, NO, False)
            Else
                Card.Set_DO_Bit(Card_ID_02, NO, True)
            End If
        End If
    End Sub
    Private Sub CheckBoxx18_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBoxx18.CheckedChanged
        If Card_Init_OK = True Then
            Dim NO As Integer = 18
            If CARD2_CHECK_Array(NO).Checked = False Then
                Card.Set_DO_Bit(Card_ID_02, NO, False)
            Else
                Card.Set_DO_Bit(Card_ID_02, NO, True)
            End If
        End If
    End Sub
    Private Sub CheckBoxx19_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBoxx19.CheckedChanged
        If Card_Init_OK = True Then
            Dim NO As Integer = 19
            If CARD2_CHECK_Array(NO).Checked = False Then
                Card.Set_DO_Bit(Card_ID_02, NO, False)
            Else
                Card.Set_DO_Bit(Card_ID_02, NO, True)
            End If
        End If
    End Sub
    Private Sub CheckBoxx20_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBoxx20.CheckedChanged
        If Card_Init_OK = True Then
            Dim NO As Integer = 20
            If CARD2_CHECK_Array(NO).Checked = False Then
                Card.Set_DO_Bit(Card_ID_02, NO, False)
            Else
                Card.Set_DO_Bit(Card_ID_02, NO, True)
            End If
        End If
    End Sub
    Private Sub CheckBoxx21_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBoxx21.CheckedChanged
        If Card_Init_OK = True Then
            Dim NO As Integer = 21
            If CARD2_CHECK_Array(NO).Checked = False Then
                Card.Set_DO_Bit(Card_ID_02, NO, False)
            Else
                Card.Set_DO_Bit(Card_ID_02, NO, True)
            End If
        End If
    End Sub
    Private Sub CheckBoxx22_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBoxx22.CheckedChanged
        If Card_Init_OK = True Then
            Dim NO As Integer = 22
            If CARD2_CHECK_Array(NO).Checked = False Then
                Card.Set_DO_Bit(Card_ID_02, NO, False)
            Else
                Card.Set_DO_Bit(Card_ID_02, NO, True)
            End If
        End If
    End Sub
    Private Sub CheckBoxx23_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBoxx23.CheckedChanged
        If Card_Init_OK = True Then
            Dim NO As Integer = 23
            If CARD2_CHECK_Array(NO).Checked = False Then
                Card.Set_DO_Bit(Card_ID_02, NO, False)
            Else
                Card.Set_DO_Bit(Card_ID_02, NO, True)
            End If
        End If
    End Sub

#End Region



    Private Sub Btn_Save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Btn_Save.Click
        If SQLCON.Updata_ADLINK_IO(PE_Value, PE_Value_1) = True Then
            Read_ADLINK_CARDIO()
            MessageBox.Show("保存参数成功！", "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Else
            MessageBox.Show("保存参数失败！", "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If

    End Sub

End Class

Public Class Adlink_Card

    Const IDX_DLL = (0)
    Const IDX_DRIVER = (1)
    Const IDX_KERNEL = (2)
    Const IDX_FIRMWARE = (3)
    Const IDX_PCB = (4)
    Const IDX_MAX = (5)
    Const __MAX_DO_CH = (24)
    Const __MAX_DI_CH = (24)

    Public Sub Set_DO_Bit(ByVal Board_ID As Integer, ByVal DO_ID As UInteger, ByVal DO_ON_OFF As Boolean)
        APS_write_d_channel_output(Board_ID, 0, DO_ID, DO_ON_OFF)
    End Sub

    Public Function Get_DI_sample(ByVal Board_ID As Integer, ByVal DI_ID As UInteger) As Boolean

        Dim digital_input_value As Integer = 0
        Dim di_ch(__MAX_DI_CH) As Integer
        Dim i As Integer

        '//**** Read digital input channels **********************************
        APS_read_d_input(Board_ID, 0, digital_input_value)

        For i = 0 To (__MAX_DI_CH - 1)
            di_ch(i) = ((digital_input_value >> i) And 1)
        Next
        If di_ch(DI_ID) = 1 Then
            Return True
        Else
            Return False
        End If
        '//********************************************************************
    End Function

    Public Function Get_DO_Bit(ByVal Board_ID As Integer, ByVal DI_ID As UInteger) As Boolean

        Dim digital_input_value As Integer = 0
        Dim di_ch(__MAX_DI_CH) As Integer
        Dim i As Integer

        '//**** Read digital input channels **********************************
        APS_read_d_output(Board_ID, 0, digital_input_value)

        For i = 0 To (__MAX_DI_CH - 1)
            di_ch(i) = ((digital_input_value >> i) And 1)
        Next
        If di_ch(DI_ID) = 1 Then
            Return True
        Else
            Return False
        End If
        '//********************************************************************
    End Function

    Function Read_Motion_IO(ByVal Axis_ID As Integer, ByVal Motion_IO As Int16) As Boolean

        Dim axis_id_sub As Integer = Axis_ID
        Dim msts As Integer
        msts = APS_motion_io_status(axis_id_sub)
        msts = (msts >> Motion_IO) And 1
        If msts = 1 Then
            Return True
        Else
            Return False
        End If
    End Function

End Class


Public Class SQL_Database
    Dim DataBase_Link_Boolean As Boolean
    Public DataBase_Connection As New SqlConnection
    Public DataBase_Command As New SqlCommand
    Dim DataBase_DataReader As SqlDataReader
    Dim DataBase_ConnectionAdapter As SqlDataAdapter

    Function Query_ADLINK_IO(ByRef PE_Value(,) As DevComponents.DotNetBar.Controls.TextBoxX, ByRef Value(,) As String, ByRef PE_Value_1(,) As DevComponents.DotNetBar.Controls.TextBoxX, ByRef Value_1(,) As String) As Boolean
        Try
            Select Case DataBase_Link_Boolean
                Case True
                    Dim dst As New DataSet
                    DataBase_Command = New SqlCommand("SELECT  CARD0_DI, CARD1_DI, CARD0_DO, CARD1_DO FROM ADLINK_DIO_PARAMETERS ORDER BY ID", DataBase_Connection)
                    DataBase_ConnectionAdapter = New SqlDataAdapter(DataBase_Command.CommandText, DataBase_Connection)
                    DataBase_ConnectionAdapter.Fill(dst)
                    For i As Integer = 0 To dst.Tables(0).Rows.Count - 1
                        Value(i, 0) = dst.Tables(0).Rows(i).Item(0).ToString.Trim
                        PE_Value(i, 0).Text = Value(i, 0)
                        Value(i, 1) = dst.Tables(0).Rows(i).Item(1).ToString.Trim
                        PE_Value(i, 1).Text = Value(i, 1)
                        Value(i, 2) = dst.Tables(0).Rows(i).Item(2).ToString.Trim
                        PE_Value(i, 2).Text = Value(i, 2)
                        Value(i, 3) = dst.Tables(0).Rows(i).Item(3).ToString.Trim
                        PE_Value(i, 3).Text = Value(i, 3)
                    Next
                    DataBase_Command.Dispose()
                    DataBase_ConnectionAdapter.Dispose()
                    dst.Dispose()

                    dst = New DataSet
                    DataBase_Command = New SqlCommand("SELECT  CARD2_DI, CARD3_DI, CARD2_DO, CARD3_DO FROM ADLINK_DIO_PARAMETERS ORDER BY ID", DataBase_Connection)
                    DataBase_ConnectionAdapter = New SqlDataAdapter(DataBase_Command.CommandText, DataBase_Connection)
                    DataBase_ConnectionAdapter.Fill(dst)
                    For i As Integer = 0 To dst.Tables(0).Rows.Count - 1
                        Value_1(i, 0) = dst.Tables(0).Rows(i).Item(0).ToString.Trim
                        PE_Value_1(i, 0).Text = Value_1(i, 0)
                        Value_1(i, 1) = dst.Tables(0).Rows(i).Item(1).ToString.Trim
                        PE_Value_1(i, 1).Text = Value_1(i, 1)
                        Value_1(i, 2) = dst.Tables(0).Rows(i).Item(2).ToString.Trim
                        PE_Value_1(i, 2).Text = Value_1(i, 2)
                        Value_1(i, 3) = dst.Tables(0).Rows(i).Item(3).ToString.Trim
                        PE_Value_1(i, 3).Text = Value_1(i, 3)
                    Next
                    DataBase_Command.Dispose()
                    DataBase_ConnectionAdapter.Dispose()
                    Return True
                Case False
                    Return False
            End Select
        Catch ex As Exception
            MsgBox(ex.ToString)
            Return False
        End Try
    End Function

    Function Query_ADLINK_IO_ID(ByVal Value As String, ByVal Card_NO As Integer) As Int16
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
                    Query_ADLINK_IO_ID = DataBase_Command.ExecuteScalar
                    DataBase_Command.Dispose()
                    Return Query_ADLINK_IO_ID
                Case False
                    Return Query_ADLINK_IO_ID
            End Select
        Catch ex As Exception
            MsgBox(ex.ToString)
            Return -1
        End Try
        Return -1
    End Function


    Function Updata_ADLINK_IO(ByVal PE_Value(,) As DevComponents.DotNetBar.Controls.TextBoxX, ByVal PE_Value_1(,) As DevComponents.DotNetBar.Controls.TextBoxX) As Boolean
        Dim index As Int16
        Try
            Select Case DataBase_Link_Boolean
                Case True
                    Dim Parameter_Name(3) As String
                    Parameter_Name(0) = "CARD0_DI"
                    Parameter_Name(1) = "CARD1_DI"
                    Parameter_Name(2) = "CARD0_DO"
                    Parameter_Name(3) = "CARD1_DO"
                    For F_ID As Int16 = 0 To 23
                        Dim Parameter_Count As Integer = 4
                        Dim Total_Parameter_Name_Str As String = Nothing
                        Dim tempStr As String

                        For i As Integer = 0 To Parameter_Count - 1
                            Select Case i
                                Case Parameter_Count - 1
                                    If PE_Value(F_ID, i).Text.ToString.Trim = "" Then
                                        tempStr = "NULL"
                                    Else
                                        tempStr = PE_Value(F_ID, i).Text.ToString.Trim
                                    End If
                                    Total_Parameter_Name_Str = Total_Parameter_Name_Str & Parameter_Name(i) & " = '" & tempStr & "' "
                                Case Else
                                    If PE_Value(F_ID, i).Text.ToString.Trim = "" Then
                                        tempStr = "NULL"
                                    Else
                                        tempStr = PE_Value(F_ID, i).Text.ToString.Trim
                                    End If
                                    Total_Parameter_Name_Str = Total_Parameter_Name_Str & Parameter_Name(i) & " = '" & tempStr & "', "
                            End Select
                        Next
                        index = F_ID
                        DataBase_Command = New SqlCommand("UPDATE ADLINK_DIO_PARAMETERS SET " & Total_Parameter_Name_Str & " WHERE (ID = N'" & F_ID & "')", DataBase_Connection)
                        DataBase_Command.ExecuteNonQuery()
                        DataBase_Command.Dispose()
                    Next

                    Parameter_Name(0) = "CARD2_DI"
                    Parameter_Name(1) = "CARD3_DI"
                    Parameter_Name(2) = "CARD2_DO"
                    Parameter_Name(3) = "CARD3_DO"
                    For F_ID As Int16 = 0 To 23
                        Dim Parameter_Count As Integer = 4
                        Dim Total_Parameter_Name_Str As String = Nothing
                        Dim tempStr As String

                        For i As Integer = 0 To Parameter_Count - 1
                            Select Case i
                                Case Parameter_Count - 1
                                    If PE_Value_1(F_ID, i).Text.ToString.Trim = "" Then
                                        tempStr = "NULL"
                                    Else
                                        tempStr = PE_Value_1(F_ID, i).Text.ToString.Trim
                                    End If
                                    Total_Parameter_Name_Str = Total_Parameter_Name_Str & Parameter_Name(i) & " = '" & tempStr & "' "
                                Case Else
                                    If PE_Value_1(F_ID, i).Text.ToString.Trim = "" Then
                                        tempStr = "NULL"
                                    Else
                                        tempStr = PE_Value_1(F_ID, i).Text.ToString.Trim
                                    End If
                                    Total_Parameter_Name_Str = Total_Parameter_Name_Str & Parameter_Name(i) & " = '" & tempStr & "', "
                            End Select
                        Next
                        index = F_ID
                        DataBase_Command = New SqlCommand("UPDATE ADLINK_DIO_PARAMETERS SET " & Total_Parameter_Name_Str & " WHERE (ID = N'" & F_ID & "')", DataBase_Connection)
                        DataBase_Command.ExecuteNonQuery()
                        DataBase_Command.Dispose()
                    Next

                    Return True
                Case False
                    Return False
            End Select
        Catch ex As Exception
            MsgBox(index.ToString & ex.ToString)
            Return False
        End Try
    End Function


    ''' <summary>
    ''' 初始化数据库
    ''' </summary>
    ''' <param name="Server_ADD">数据库地址</param>
    ''' <param name="Use_ID">数据库用户名</param>
    ''' <param name="Pass_Word">数据库密码，缺省无密码</param>
    ''' <param name="Initial_Catalog_Name">数据库名称</param>
    ''' <returns>返回值：TRUE（初始化成功），FALSE（初始化失败）</returns>
    ''' <param name="Rtn_Message">返回处理消息</param>
    ''' <remarks></remarks>
    Function DataBase_Initialization(Optional ByVal Server_ADD As String = "127.0.0.1", Optional ByVal Use_ID As String = "SA", Optional ByVal Pass_Word As String = Nothing, Optional ByVal Initial_Catalog_Name As String = Nothing, Optional ByRef Rtn_Message As String = Nothing) As Boolean
        Try
            Select Case DataBase_Link_Boolean
                Case False
                    DataBase_Connection.ConnectionString = "Data Source=" & Server_ADD & ";Initial Catalog=" & Initial_Catalog_Name & ";User ID=" & Use_ID & ";Password=" & Pass_Word & "" & ";Asynchronous Processing=True;MultipleActiveResultSets=True;Connect Timeout=500000"
                    If DataBase_Connection.State = ConnectionState.Closed Then
                        DataBase_Connection.Open()
                        DataBase_Link_Boolean = True
                        Rtn_Message = "数据库初始化完。"
                    Else
                        DataBase_Link_Boolean = False
                    End If
                Case True
                    Rtn_Message = "数据库已经初始化。"
            End Select
            Return DataBase_Link_Boolean
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Function

    ''' <summary>
    ''' 关闭数据库
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function Close_DataBase() As String
        Try
            If DataBase_Connection IsNot Nothing Then
                If DataBase_Connection.State = ConnectionState.Open Then
                    DataBase_Connection.Close()
                    DataBase_Connection.Dispose()
                    DataBase_Command.Dispose()
                    DataBase_Command = Nothing
                    DataBase_Connection = Nothing
                    DataBase_Link_Boolean = False
                    Close_DataBase = "数据库关闭完成。"
                Else
                    Return "数据库已经处于关闭状态。"
                End If
            End If
        Catch ex As Exception
            Return "ERROR"
        End Try
    End Function
End Class

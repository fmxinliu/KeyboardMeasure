Imports Keyboard_Measure.System_SetDialog.COORDINATE_SELECT_PARAMS

Public Class Axis_Order_Dialog

    Dim HomeParamObjArray() As SQL_LIB.HOME_PARAMETERS
    Public Axis_Add_Name As New List(Of String)
    Public Axis_Add_Name_Str As String
    Public Axis_Add_Index_Str As String
    Public Axis_Add_Name_Array() As String
    Public Axis_Add_Index_Array() As String

    Dim _Model As Integer = 0
    Sub New(ByVal Mode As _Model_Enum)
        ' 此调用是设计器所必需的。
        InitializeComponent()
        ' 在 InitializeComponent() 调用之后添加任何初始化。
        _Model = Mode
    End Sub

    Public Enum _Model_Enum
        Add
        Updata
    End Enum

    Private Sub Add_Axis_Dialog_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        HomeParamObjArray = Main.HomeParamObjArray
        DX_Add_Axis.Rows.Clear()
        Select Case _Model
            Case _Model_Enum.Add '新增
                For i As Int16 = 0 To HomeParamObjArray.Length - 1
                    DX_Add_Axis.Rows.Add()
                    DX_Add_Axis.Rows(i).Cells(0).Value = HomeParamObjArray(i).轴名称 '列1轴名称，列2选项
                    DX_Add_Axis.Rows(i).Cells(1).Value = False
                    DX_Add_Axis.Rows(i).Cells(2).Value = 1
                Next
            Case _Model_Enum.Updata '更新
                For i As Int16 = 0 To HomeParamObjArray.Length - 1
                    DX_Add_Axis.Rows.Add()
                    DX_Add_Axis.Rows(i).Cells(0).Value = HomeParamObjArray(i).轴名称 '列1轴名称，列2选项
                    DX_Add_Axis.Rows(i).Cells(1).Value = False
                    DX_Add_Axis.Rows(i).Cells(2).Value = 1
                    If System_SetDialog.COORDINATE_SELECT_PARAMS.轴系.Length >= 1 Then
                        Axis_Add_Name_Array = Split(System_SetDialog.COORDINATE_SELECT_PARAMS.轴系, ";")
                        Axis_Add_Index_Array = Split(System_SetDialog.COORDINATE_SELECT_PARAMS.运动顺序, ";")
                        For j As Int16 = 0 To Axis_Add_Name_Array.Length - 1
                            If HomeParamObjArray(i).轴名称 = Axis_Add_Name_Array(j) Then
                                DX_Add_Axis.Rows(i).Cells(1).Value = True
                                DX_Add_Axis.Rows(i).Cells(2).Value = Axis_Add_Index_Array(j)
                                Exit For
                            End If
                        Next
                    End If
                Next
        End Select

        DX_Add_Axis.RowHeadersVisible = False
        DX_Add_Axis.AllowUserToDeleteRows = False
        DX_Add_Axis.AllowUserToResizeColumns = False
        DX_Add_Axis.AllowUserToResizeRows = False
        DX_Add_Axis.ColumnHeadersHeightSizeMode = Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        DX_Add_Axis.MultiSelect = False
        DX_Add_Axis.ReadOnly = False
        DX_Add_Axis.RowHeadersWidthSizeMode = Windows.Forms.DataGridViewRowHeadersWidthSizeMode.EnableResizing
        DX_Add_Axis.SelectionMode = Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        DX_Add_Axis.EditMode = DataGridViewEditMode.EditOnEnter
    End Sub

    Private Sub Btn_OK_Click(sender As System.Object, e As System.EventArgs) Handles Btn_OK.Click
        Axis_Add_Name.Clear()
        Axis_Add_Name_Str = ""
        Axis_Add_Index_Str = ""
        For i As Int16 = 0 To DX_Add_Axis.Rows.Count - 1
            If DX_Add_Axis.Rows(i).Cells(1).Value = True Then
                Dim str1 As String = DX_Add_Axis.Rows(i).Cells(0).Value.ToString.Trim '把选中的轴名称连接后存入
                Axis_Add_Name.Add(str1)
                If Axis_Add_Name_Str = "" Then
                    Axis_Add_Name_Str = str1
                Else
                    Axis_Add_Name_Str &= ";" & str1
                End If

                Dim str2 As String = DX_Add_Axis.Rows(i).Cells(2).Value.ToString.Trim '把轴运动顺序连接后存入
                If Axis_Add_Index_Str = "" Then
                    Axis_Add_Index_Str = str2
                Else
                    Axis_Add_Index_Str &= ";" & str2
                End If
            End If
        Next
        Me.DialogResult = Windows.Forms.DialogResult.Yes
        Me.Close()
    End Sub

    Private Sub Btn_CANCEL_Click(sender As System.Object, e As System.EventArgs) Handles Btn_CANCEL.Click
        Me.DialogResult = Windows.Forms.DialogResult.No
        Me.Close()
    End Sub

End Class
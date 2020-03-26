<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Modify_Home_Dialog
    Inherits System.Windows.Forms.Form

    'Form 重写 Dispose，以清理组件列表。
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Windows 窗体设计器所必需的
    Private components As System.ComponentModel.IContainer

    '注意: 以下过程是 Windows 窗体设计器所必需的
    '可以使用 Windows 窗体设计器修改它。
    '不要使用代码编辑器修改它。
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.PanelEx1 = New DevComponents.DotNetBar.PanelEx()
        Me.AXIS_NO = New DevComponents.Editors.IntegerInput()
        Me.HOME_ADC = New DevComponents.Editors.DoubleInput()
        Me.PanelEx2 = New DevComponents.DotNetBar.PanelEx()
        Me.PanelEx3 = New DevComponents.DotNetBar.PanelEx()
        Me.PanelEx4 = New DevComponents.DotNetBar.PanelEx()
        Me.PanelEx5 = New DevComponents.DotNetBar.PanelEx()
        Me.PanelEx6 = New DevComponents.DotNetBar.PanelEx()
        Me.HOME_SPEED = New DevComponents.Editors.IntegerInput()
        Me.AXIS_M = New DevComponents.Editors.DoubleInput()
        Me.HOME_OFFSET = New DevComponents.Editors.IntegerInput()
        Me.ButtonX1 = New DevComponents.DotNetBar.ButtonX()
        Me.ButtonX2 = New DevComponents.DotNetBar.ButtonX()
        Me.AXIS_NAME_1 = New DevComponents.DotNetBar.Controls.ComboBoxEx()
        Me.ComboItem1 = New DevComponents.Editors.ComboItem()
        Me.ComboItem2 = New DevComponents.Editors.ComboItem()
        Me.ComboItem3 = New DevComponents.Editors.ComboItem()
        Me.ComboItem4 = New DevComponents.Editors.ComboItem()
        Me.ComboItem5 = New DevComponents.Editors.ComboItem()
        Me.ComboItem6 = New DevComponents.Editors.ComboItem()
        Me.AXIS_NAME_2 = New DevComponents.DotNetBar.Controls.ComboBoxEx()
        Me.ComboItem13 = New DevComponents.Editors.ComboItem()
        Me.ComboItem14 = New DevComponents.Editors.ComboItem()
        Me.ComboItem15 = New DevComponents.Editors.ComboItem()
        Me.ComboItem16 = New DevComponents.Editors.ComboItem()
        Me.ComboItem17 = New DevComponents.Editors.ComboItem()
        Me.ComboItem18 = New DevComponents.Editors.ComboItem()
        Me.ComboItem19 = New DevComponents.Editors.ComboItem()
        Me.ComboItem20 = New DevComponents.Editors.ComboItem()
        Me.ComboItem21 = New DevComponents.Editors.ComboItem()
        Me.ComboItem22 = New DevComponents.Editors.ComboItem()
        Me.PanelEx7 = New DevComponents.DotNetBar.PanelEx()
        Me.PanelEx8 = New DevComponents.DotNetBar.PanelEx()
        Me.PanelEx9 = New DevComponents.DotNetBar.PanelEx()
        Me.PanelEx10 = New DevComponents.DotNetBar.PanelEx()
        Me.HOME_SELECT_MODE = New DevComponents.DotNetBar.Controls.ComboBoxEx()
        Me.ComboItem33 = New DevComponents.Editors.ComboItem()
        Me.ComboItem34 = New DevComponents.Editors.ComboItem()
        Me.ComboItem35 = New DevComponents.Editors.ComboItem()
        Me.HOME_DIRECTION = New DevComponents.DotNetBar.Controls.ComboBoxEx()
        Me.ComboItem37 = New DevComponents.Editors.ComboItem()
        Me.ComboItem38 = New DevComponents.Editors.ComboItem()
        Me.HOME_EZA = New DevComponents.DotNetBar.Controls.ComboBoxEx()
        Me.ComboItem27 = New DevComponents.Editors.ComboItem()
        Me.ComboItem28 = New DevComponents.Editors.ComboItem()
        Me.HOME_CURVE = New DevComponents.DotNetBar.Controls.ComboBoxEx()
        Me.ComboItem30 = New DevComponents.Editors.ComboItem()
        Me.ComboItem31 = New DevComponents.Editors.ComboItem()
        Me.PanelEx11 = New DevComponents.DotNetBar.PanelEx()
        Me.HOME_INDEX = New DevComponents.Editors.IntegerInput()
        CType(Me.AXIS_NO, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.HOME_ADC, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.HOME_SPEED, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.AXIS_M, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.HOME_OFFSET, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.HOME_INDEX, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'PanelEx1
        '
        Me.PanelEx1.CanvasColor = System.Drawing.SystemColors.Control
        Me.PanelEx1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.PanelEx1.DisabledBackColor = System.Drawing.Color.Empty
        Me.PanelEx1.Location = New System.Drawing.Point(12, 12)
        Me.PanelEx1.Name = "PanelEx1"
        Me.PanelEx1.Size = New System.Drawing.Size(107, 21)
        Me.PanelEx1.Style.Alignment = System.Drawing.StringAlignment.Center
        Me.PanelEx1.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground
        Me.PanelEx1.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2
        Me.PanelEx1.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.PanelEx1.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
        Me.PanelEx1.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
        Me.PanelEx1.Style.GradientAngle = 90
        Me.PanelEx1.TabIndex = 0
        Me.PanelEx1.Text = "轴名称"
        '
        'AXIS_NO
        '
        '
        '
        '
        Me.AXIS_NO.BackgroundStyle.Class = "DateTimeInputBackground"
        Me.AXIS_NO.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.AXIS_NO.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2
        Me.AXIS_NO.Location = New System.Drawing.Point(125, 45)
        Me.AXIS_NO.Name = "AXIS_NO"
        Me.AXIS_NO.ShowUpDown = True
        Me.AXIS_NO.Size = New System.Drawing.Size(97, 21)
        Me.AXIS_NO.TabIndex = 1
        Me.AXIS_NO.Value = 1
        '
        'HOME_ADC
        '
        '
        '
        '
        Me.HOME_ADC.BackgroundStyle.Class = "DateTimeInputBackground"
        Me.HOME_ADC.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.HOME_ADC.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2
        Me.HOME_ADC.Increment = 0.1R
        Me.HOME_ADC.Location = New System.Drawing.Point(125, 269)
        Me.HOME_ADC.MaxValue = 1.0R
        Me.HOME_ADC.MinValue = 0.1R
        Me.HOME_ADC.Name = "HOME_ADC"
        Me.HOME_ADC.ShowUpDown = True
        Me.HOME_ADC.Size = New System.Drawing.Size(97, 21)
        Me.HOME_ADC.TabIndex = 2
        Me.HOME_ADC.Value = 0.1R
        '
        'PanelEx2
        '
        Me.PanelEx2.CanvasColor = System.Drawing.SystemColors.Control
        Me.PanelEx2.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.PanelEx2.DisabledBackColor = System.Drawing.Color.Empty
        Me.PanelEx2.Location = New System.Drawing.Point(12, 44)
        Me.PanelEx2.Name = "PanelEx2"
        Me.PanelEx2.Size = New System.Drawing.Size(107, 21)
        Me.PanelEx2.Style.Alignment = System.Drawing.StringAlignment.Center
        Me.PanelEx2.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground
        Me.PanelEx2.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2
        Me.PanelEx2.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.PanelEx2.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
        Me.PanelEx2.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
        Me.PanelEx2.Style.GradientAngle = 90
        Me.PanelEx2.TabIndex = 0
        Me.PanelEx2.Text = "轴号"
        '
        'PanelEx3
        '
        Me.PanelEx3.CanvasColor = System.Drawing.SystemColors.Control
        Me.PanelEx3.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.PanelEx3.DisabledBackColor = System.Drawing.Color.Empty
        Me.PanelEx3.Location = New System.Drawing.Point(12, 76)
        Me.PanelEx3.Name = "PanelEx3"
        Me.PanelEx3.Size = New System.Drawing.Size(107, 21)
        Me.PanelEx3.Style.Alignment = System.Drawing.StringAlignment.Center
        Me.PanelEx3.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground
        Me.PanelEx3.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2
        Me.PanelEx3.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.PanelEx3.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
        Me.PanelEx3.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
        Me.PanelEx3.Style.GradientAngle = 90
        Me.PanelEx3.TabIndex = 0
        Me.PanelEx3.Text = "导程"
        '
        'PanelEx4
        '
        Me.PanelEx4.CanvasColor = System.Drawing.SystemColors.Control
        Me.PanelEx4.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.PanelEx4.DisabledBackColor = System.Drawing.Color.Empty
        Me.PanelEx4.Location = New System.Drawing.Point(12, 236)
        Me.PanelEx4.Name = "PanelEx4"
        Me.PanelEx4.Size = New System.Drawing.Size(107, 21)
        Me.PanelEx4.Style.Alignment = System.Drawing.StringAlignment.Center
        Me.PanelEx4.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground
        Me.PanelEx4.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2
        Me.PanelEx4.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.PanelEx4.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
        Me.PanelEx4.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
        Me.PanelEx4.Style.GradientAngle = 90
        Me.PanelEx4.TabIndex = 0
        Me.PanelEx4.Text = "回原点速度"
        '
        'PanelEx5
        '
        Me.PanelEx5.CanvasColor = System.Drawing.SystemColors.Control
        Me.PanelEx5.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.PanelEx5.DisabledBackColor = System.Drawing.Color.Empty
        Me.PanelEx5.Location = New System.Drawing.Point(12, 268)
        Me.PanelEx5.Name = "PanelEx5"
        Me.PanelEx5.Size = New System.Drawing.Size(107, 21)
        Me.PanelEx5.Style.Alignment = System.Drawing.StringAlignment.Center
        Me.PanelEx5.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground
        Me.PanelEx5.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2
        Me.PanelEx5.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.PanelEx5.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
        Me.PanelEx5.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
        Me.PanelEx5.Style.GradientAngle = 90
        Me.PanelEx5.TabIndex = 0
        Me.PanelEx5.Text = "回原点加减速度"
        '
        'PanelEx6
        '
        Me.PanelEx6.CanvasColor = System.Drawing.SystemColors.Control
        Me.PanelEx6.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.PanelEx6.DisabledBackColor = System.Drawing.Color.Empty
        Me.PanelEx6.Location = New System.Drawing.Point(12, 300)
        Me.PanelEx6.Name = "PanelEx6"
        Me.PanelEx6.Size = New System.Drawing.Size(107, 21)
        Me.PanelEx6.Style.Alignment = System.Drawing.StringAlignment.Center
        Me.PanelEx6.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground
        Me.PanelEx6.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2
        Me.PanelEx6.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.PanelEx6.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
        Me.PanelEx6.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
        Me.PanelEx6.Style.GradientAngle = 90
        Me.PanelEx6.TabIndex = 0
        Me.PanelEx6.Text = "原点偏移"
        '
        'HOME_SPEED
        '
        '
        '
        '
        Me.HOME_SPEED.BackgroundStyle.Class = "DateTimeInputBackground"
        Me.HOME_SPEED.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.HOME_SPEED.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2
        Me.HOME_SPEED.Increment = 10
        Me.HOME_SPEED.Location = New System.Drawing.Point(125, 237)
        Me.HOME_SPEED.MinValue = 1
        Me.HOME_SPEED.Name = "HOME_SPEED"
        Me.HOME_SPEED.ShowUpDown = True
        Me.HOME_SPEED.Size = New System.Drawing.Size(97, 21)
        Me.HOME_SPEED.TabIndex = 1
        Me.HOME_SPEED.Value = 1
        '
        'AXIS_M
        '
        '
        '
        '
        Me.AXIS_M.BackgroundStyle.Class = "DateTimeInputBackground"
        Me.AXIS_M.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.AXIS_M.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2
        Me.AXIS_M.Increment = 1.0R
        Me.AXIS_M.Location = New System.Drawing.Point(125, 77)
        Me.AXIS_M.Name = "AXIS_M"
        Me.AXIS_M.ShowUpDown = True
        Me.AXIS_M.Size = New System.Drawing.Size(97, 21)
        Me.AXIS_M.TabIndex = 2
        Me.AXIS_M.Value = 1.0R
        '
        'HOME_OFFSET
        '
        '
        '
        '
        Me.HOME_OFFSET.BackgroundStyle.Class = "DateTimeInputBackground"
        Me.HOME_OFFSET.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.HOME_OFFSET.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2
        Me.HOME_OFFSET.Location = New System.Drawing.Point(125, 301)
        Me.HOME_OFFSET.Name = "HOME_OFFSET"
        Me.HOME_OFFSET.ShowUpDown = True
        Me.HOME_OFFSET.Size = New System.Drawing.Size(97, 21)
        Me.HOME_OFFSET.TabIndex = 1
        Me.HOME_OFFSET.Value = 1
        '
        'ButtonX1
        '
        Me.ButtonX1.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.ButtonX1.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground
        Me.ButtonX1.Location = New System.Drawing.Point(12, 370)
        Me.ButtonX1.Name = "ButtonX1"
        Me.ButtonX1.Size = New System.Drawing.Size(107, 36)
        Me.ButtonX1.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.ButtonX1.TabIndex = 4
        Me.ButtonX1.Text = "OK"
        '
        'ButtonX2
        '
        Me.ButtonX2.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.ButtonX2.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground
        Me.ButtonX2.Location = New System.Drawing.Point(125, 370)
        Me.ButtonX2.Name = "ButtonX2"
        Me.ButtonX2.Size = New System.Drawing.Size(97, 36)
        Me.ButtonX2.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.ButtonX2.TabIndex = 4
        Me.ButtonX2.Text = "CANCEL"
        '
        'AXIS_NAME_1
        '
        Me.AXIS_NAME_1.DisplayMember = "Text"
        Me.AXIS_NAME_1.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
        Me.AXIS_NAME_1.FormattingEnabled = True
        Me.AXIS_NAME_1.ItemHeight = 15
        Me.AXIS_NAME_1.Items.AddRange(New Object() {Me.ComboItem1, Me.ComboItem2, Me.ComboItem3, Me.ComboItem4, Me.ComboItem5, Me.ComboItem6})
        Me.AXIS_NAME_1.Location = New System.Drawing.Point(125, 12)
        Me.AXIS_NAME_1.Name = "AXIS_NAME_1"
        Me.AXIS_NAME_1.Size = New System.Drawing.Size(48, 21)
        Me.AXIS_NAME_1.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.AXIS_NAME_1.TabIndex = 6
        '
        'ComboItem1
        '
        Me.ComboItem1.Text = "X"
        '
        'ComboItem2
        '
        Me.ComboItem2.Text = "Y"
        '
        'ComboItem3
        '
        Me.ComboItem3.Text = "R"
        '
        'ComboItem4
        '
        Me.ComboItem4.Text = "Z"
        '
        'ComboItem5
        '
        Me.ComboItem5.Text = "U"
        '
        'ComboItem6
        '
        Me.ComboItem6.Text = "S"
        '
        'AXIS_NAME_2
        '
        Me.AXIS_NAME_2.DisplayMember = "Text"
        Me.AXIS_NAME_2.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
        Me.AXIS_NAME_2.FormattingEnabled = True
        Me.AXIS_NAME_2.ItemHeight = 15
        Me.AXIS_NAME_2.Items.AddRange(New Object() {Me.ComboItem13, Me.ComboItem14, Me.ComboItem15, Me.ComboItem16, Me.ComboItem17, Me.ComboItem18, Me.ComboItem19, Me.ComboItem20, Me.ComboItem21, Me.ComboItem22})
        Me.AXIS_NAME_2.Location = New System.Drawing.Point(176, 12)
        Me.AXIS_NAME_2.Name = "AXIS_NAME_2"
        Me.AXIS_NAME_2.Size = New System.Drawing.Size(46, 21)
        Me.AXIS_NAME_2.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.AXIS_NAME_2.TabIndex = 6
        '
        'ComboItem13
        '
        Me.ComboItem13.Text = "01"
        '
        'ComboItem14
        '
        Me.ComboItem14.Text = "02"
        '
        'ComboItem15
        '
        Me.ComboItem15.Text = "03"
        '
        'ComboItem16
        '
        Me.ComboItem16.Text = "04"
        '
        'ComboItem17
        '
        Me.ComboItem17.Text = "05"
        '
        'ComboItem18
        '
        Me.ComboItem18.Text = "06"
        '
        'ComboItem19
        '
        Me.ComboItem19.Text = "07"
        '
        'ComboItem20
        '
        Me.ComboItem20.Text = "08"
        '
        'ComboItem21
        '
        Me.ComboItem21.Text = "09"
        '
        'ComboItem22
        '
        Me.ComboItem22.Text = "10"
        '
        'PanelEx7
        '
        Me.PanelEx7.CanvasColor = System.Drawing.SystemColors.Control
        Me.PanelEx7.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.PanelEx7.DisabledBackColor = System.Drawing.Color.Empty
        Me.PanelEx7.Location = New System.Drawing.Point(12, 108)
        Me.PanelEx7.Name = "PanelEx7"
        Me.PanelEx7.Size = New System.Drawing.Size(107, 21)
        Me.PanelEx7.Style.Alignment = System.Drawing.StringAlignment.Center
        Me.PanelEx7.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground
        Me.PanelEx7.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2
        Me.PanelEx7.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.PanelEx7.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
        Me.PanelEx7.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
        Me.PanelEx7.Style.GradientAngle = 90
        Me.PanelEx7.TabIndex = 0
        Me.PanelEx7.Text = "回原点模式"
        '
        'PanelEx8
        '
        Me.PanelEx8.CanvasColor = System.Drawing.SystemColors.Control
        Me.PanelEx8.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.PanelEx8.DisabledBackColor = System.Drawing.Color.Empty
        Me.PanelEx8.Location = New System.Drawing.Point(12, 140)
        Me.PanelEx8.Name = "PanelEx8"
        Me.PanelEx8.Size = New System.Drawing.Size(107, 21)
        Me.PanelEx8.Style.Alignment = System.Drawing.StringAlignment.Center
        Me.PanelEx8.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground
        Me.PanelEx8.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2
        Me.PanelEx8.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.PanelEx8.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
        Me.PanelEx8.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
        Me.PanelEx8.Style.GradientAngle = 90
        Me.PanelEx8.TabIndex = 0
        Me.PanelEx8.Text = "回原点搜索方向"
        '
        'PanelEx9
        '
        Me.PanelEx9.CanvasColor = System.Drawing.SystemColors.Control
        Me.PanelEx9.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.PanelEx9.DisabledBackColor = System.Drawing.Color.Empty
        Me.PanelEx9.Location = New System.Drawing.Point(12, 172)
        Me.PanelEx9.Name = "PanelEx9"
        Me.PanelEx9.Size = New System.Drawing.Size(107, 21)
        Me.PanelEx9.Style.Alignment = System.Drawing.StringAlignment.Center
        Me.PanelEx9.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground
        Me.PanelEx9.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2
        Me.PanelEx9.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.PanelEx9.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
        Me.PanelEx9.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
        Me.PanelEx9.Style.GradientAngle = 90
        Me.PanelEx9.TabIndex = 0
        Me.PanelEx9.Text = "回原点Z相信号"
        '
        'PanelEx10
        '
        Me.PanelEx10.CanvasColor = System.Drawing.SystemColors.Control
        Me.PanelEx10.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.PanelEx10.DisabledBackColor = System.Drawing.Color.Empty
        Me.PanelEx10.Location = New System.Drawing.Point(12, 204)
        Me.PanelEx10.Name = "PanelEx10"
        Me.PanelEx10.Size = New System.Drawing.Size(107, 21)
        Me.PanelEx10.Style.Alignment = System.Drawing.StringAlignment.Center
        Me.PanelEx10.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground
        Me.PanelEx10.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2
        Me.PanelEx10.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.PanelEx10.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
        Me.PanelEx10.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
        Me.PanelEx10.Style.GradientAngle = 90
        Me.PanelEx10.TabIndex = 0
        Me.PanelEx10.Text = "回原点曲线"
        '
        'HOME_SELECT_MODE
        '
        Me.HOME_SELECT_MODE.DisplayMember = "Text"
        Me.HOME_SELECT_MODE.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
        Me.HOME_SELECT_MODE.FormattingEnabled = True
        Me.HOME_SELECT_MODE.ItemHeight = 15
        Me.HOME_SELECT_MODE.Items.AddRange(New Object() {Me.ComboItem33, Me.ComboItem34, Me.ComboItem35})
        Me.HOME_SELECT_MODE.Location = New System.Drawing.Point(125, 109)
        Me.HOME_SELECT_MODE.Name = "HOME_SELECT_MODE"
        Me.HOME_SELECT_MODE.Size = New System.Drawing.Size(95, 21)
        Me.HOME_SELECT_MODE.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.HOME_SELECT_MODE.TabIndex = 6
        '
        'ComboItem33
        '
        Me.ComboItem33.Text = "HOME_MODE_ORG"
        '
        'ComboItem34
        '
        Me.ComboItem34.Text = "HOME_MODE_EL"
        '
        'ComboItem35
        '
        Me.ComboItem35.Text = "HOME_MODE_EZ"
        '
        'HOME_DIRECTION
        '
        Me.HOME_DIRECTION.DisplayMember = "Text"
        Me.HOME_DIRECTION.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
        Me.HOME_DIRECTION.FormattingEnabled = True
        Me.HOME_DIRECTION.ItemHeight = 15
        Me.HOME_DIRECTION.Items.AddRange(New Object() {Me.ComboItem37, Me.ComboItem38})
        Me.HOME_DIRECTION.Location = New System.Drawing.Point(125, 141)
        Me.HOME_DIRECTION.Name = "HOME_DIRECTION"
        Me.HOME_DIRECTION.Size = New System.Drawing.Size(95, 21)
        Me.HOME_DIRECTION.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.HOME_DIRECTION.TabIndex = 6
        '
        'ComboItem37
        '
        Me.ComboItem37.Text = "Positive"
        '
        'ComboItem38
        '
        Me.ComboItem38.Text = "Negative"
        '
        'HOME_EZA
        '
        Me.HOME_EZA.DisplayMember = "Text"
        Me.HOME_EZA.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
        Me.HOME_EZA.FormattingEnabled = True
        Me.HOME_EZA.ItemHeight = 15
        Me.HOME_EZA.Items.AddRange(New Object() {Me.ComboItem27, Me.ComboItem28})
        Me.HOME_EZA.Location = New System.Drawing.Point(125, 173)
        Me.HOME_EZA.Name = "HOME_EZA"
        Me.HOME_EZA.Size = New System.Drawing.Size(95, 21)
        Me.HOME_EZA.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.HOME_EZA.TabIndex = 6
        '
        'ComboItem27
        '
        Me.ComboItem27.Text = "Disable"
        '
        'ComboItem28
        '
        Me.ComboItem28.Text = "Enable"
        '
        'HOME_CURVE
        '
        Me.HOME_CURVE.DisplayMember = "Text"
        Me.HOME_CURVE.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
        Me.HOME_CURVE.FormattingEnabled = True
        Me.HOME_CURVE.ItemHeight = 15
        Me.HOME_CURVE.Items.AddRange(New Object() {Me.ComboItem30, Me.ComboItem31})
        Me.HOME_CURVE.Location = New System.Drawing.Point(125, 205)
        Me.HOME_CURVE.Name = "HOME_CURVE"
        Me.HOME_CURVE.Size = New System.Drawing.Size(95, 21)
        Me.HOME_CURVE.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.HOME_CURVE.TabIndex = 6
        '
        'ComboItem30
        '
        Me.ComboItem30.Text = "T_curve"
        '
        'ComboItem31
        '
        Me.ComboItem31.Text = "S_curve"
        '
        'PanelEx11
        '
        Me.PanelEx11.CanvasColor = System.Drawing.SystemColors.Control
        Me.PanelEx11.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.PanelEx11.DisabledBackColor = System.Drawing.Color.Empty
        Me.PanelEx11.Location = New System.Drawing.Point(12, 332)
        Me.PanelEx11.Name = "PanelEx11"
        Me.PanelEx11.Size = New System.Drawing.Size(107, 21)
        Me.PanelEx11.Style.Alignment = System.Drawing.StringAlignment.Center
        Me.PanelEx11.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground
        Me.PanelEx11.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2
        Me.PanelEx11.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.PanelEx11.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
        Me.PanelEx11.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
        Me.PanelEx11.Style.GradientAngle = 90
        Me.PanelEx11.TabIndex = 0
        Me.PanelEx11.Text = "回原点顺序"
        '
        'HOME_INDEX
        '
        '
        '
        '
        Me.HOME_INDEX.BackgroundStyle.Class = "DateTimeInputBackground"
        Me.HOME_INDEX.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.HOME_INDEX.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2
        Me.HOME_INDEX.Location = New System.Drawing.Point(125, 333)
        Me.HOME_INDEX.MaxValue = 5
        Me.HOME_INDEX.MinValue = 1
        Me.HOME_INDEX.Name = "HOME_INDEX"
        Me.HOME_INDEX.ShowUpDown = True
        Me.HOME_INDEX.Size = New System.Drawing.Size(97, 21)
        Me.HOME_INDEX.TabIndex = 1
        Me.HOME_INDEX.Value = 1
        '
        'Add_Dialog
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(242, 419)
        Me.Controls.Add(Me.HOME_CURVE)
        Me.Controls.Add(Me.HOME_EZA)
        Me.Controls.Add(Me.HOME_DIRECTION)
        Me.Controls.Add(Me.HOME_SELECT_MODE)
        Me.Controls.Add(Me.AXIS_NAME_2)
        Me.Controls.Add(Me.AXIS_NAME_1)
        Me.Controls.Add(Me.ButtonX2)
        Me.Controls.Add(Me.ButtonX1)
        Me.Controls.Add(Me.HOME_ADC)
        Me.Controls.Add(Me.AXIS_M)
        Me.Controls.Add(Me.HOME_INDEX)
        Me.Controls.Add(Me.HOME_OFFSET)
        Me.Controls.Add(Me.HOME_SPEED)
        Me.Controls.Add(Me.PanelEx11)
        Me.Controls.Add(Me.AXIS_NO)
        Me.Controls.Add(Me.PanelEx6)
        Me.Controls.Add(Me.PanelEx5)
        Me.Controls.Add(Me.PanelEx10)
        Me.Controls.Add(Me.PanelEx9)
        Me.Controls.Add(Me.PanelEx8)
        Me.Controls.Add(Me.PanelEx7)
        Me.Controls.Add(Me.PanelEx4)
        Me.Controls.Add(Me.PanelEx3)
        Me.Controls.Add(Me.PanelEx2)
        Me.Controls.Add(Me.PanelEx1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "Add_Dialog"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        CType(Me.AXIS_NO, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.HOME_ADC, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.HOME_SPEED, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.AXIS_M, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.HOME_OFFSET, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.HOME_INDEX, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ComboItem1 As DevComponents.Editors.ComboItem
    Friend WithEvents ComboItem2 As DevComponents.Editors.ComboItem
    Friend WithEvents ComboItem3 As DevComponents.Editors.ComboItem
    Friend WithEvents ComboItem4 As DevComponents.Editors.ComboItem
    Friend WithEvents ComboItem5 As DevComponents.Editors.ComboItem
    Friend WithEvents ComboItem6 As DevComponents.Editors.ComboItem
    Friend WithEvents ComboItem13 As DevComponents.Editors.ComboItem
    Friend WithEvents ComboItem14 As DevComponents.Editors.ComboItem
    Friend WithEvents ComboItem15 As DevComponents.Editors.ComboItem
    Friend WithEvents ComboItem16 As DevComponents.Editors.ComboItem
    Friend WithEvents ComboItem17 As DevComponents.Editors.ComboItem
    Friend WithEvents ComboItem18 As DevComponents.Editors.ComboItem
    Friend WithEvents ComboItem19 As DevComponents.Editors.ComboItem
    Friend WithEvents ComboItem20 As DevComponents.Editors.ComboItem
    Friend WithEvents ComboItem21 As DevComponents.Editors.ComboItem
    Friend WithEvents ComboItem22 As DevComponents.Editors.ComboItem
    Private WithEvents PanelEx1 As DevComponents.DotNetBar.PanelEx
    Private WithEvents AXIS_NO As DevComponents.Editors.IntegerInput
    Private WithEvents HOME_ADC As DevComponents.Editors.DoubleInput
    Private WithEvents PanelEx2 As DevComponents.DotNetBar.PanelEx
    Private WithEvents PanelEx3 As DevComponents.DotNetBar.PanelEx
    Private WithEvents PanelEx4 As DevComponents.DotNetBar.PanelEx
    Private WithEvents PanelEx5 As DevComponents.DotNetBar.PanelEx
    Private WithEvents PanelEx6 As DevComponents.DotNetBar.PanelEx
    Private WithEvents HOME_SPEED As DevComponents.Editors.IntegerInput
    Private WithEvents AXIS_M As DevComponents.Editors.DoubleInput
    Private WithEvents HOME_OFFSET As DevComponents.Editors.IntegerInput
    Private WithEvents ButtonX1 As DevComponents.DotNetBar.ButtonX
    Private WithEvents ButtonX2 As DevComponents.DotNetBar.ButtonX
    Private WithEvents AXIS_NAME_1 As DevComponents.DotNetBar.Controls.ComboBoxEx
    Private WithEvents AXIS_NAME_2 As DevComponents.DotNetBar.Controls.ComboBoxEx
    Private WithEvents PanelEx7 As DevComponents.DotNetBar.PanelEx
    Private WithEvents PanelEx8 As DevComponents.DotNetBar.PanelEx
    Private WithEvents PanelEx9 As DevComponents.DotNetBar.PanelEx
    Private WithEvents PanelEx10 As DevComponents.DotNetBar.PanelEx
    Private WithEvents HOME_SELECT_MODE As DevComponents.DotNetBar.Controls.ComboBoxEx
    Private WithEvents HOME_DIRECTION As DevComponents.DotNetBar.Controls.ComboBoxEx
    Friend WithEvents ComboItem33 As DevComponents.Editors.ComboItem
    Friend WithEvents ComboItem34 As DevComponents.Editors.ComboItem
    Friend WithEvents ComboItem35 As DevComponents.Editors.ComboItem
    Friend WithEvents ComboItem37 As DevComponents.Editors.ComboItem
    Friend WithEvents ComboItem38 As DevComponents.Editors.ComboItem
    Private WithEvents HOME_EZA As DevComponents.DotNetBar.Controls.ComboBoxEx
    Friend WithEvents ComboItem27 As DevComponents.Editors.ComboItem
    Friend WithEvents ComboItem28 As DevComponents.Editors.ComboItem
    Private WithEvents HOME_CURVE As DevComponents.DotNetBar.Controls.ComboBoxEx
    Friend WithEvents ComboItem30 As DevComponents.Editors.ComboItem
    Friend WithEvents ComboItem31 As DevComponents.Editors.ComboItem
    Private WithEvents PanelEx11 As DevComponents.DotNetBar.PanelEx
    Private WithEvents HOME_INDEX As DevComponents.Editors.IntegerInput
End Class

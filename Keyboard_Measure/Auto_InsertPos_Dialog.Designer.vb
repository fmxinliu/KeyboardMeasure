<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Auto_InsertPos_Dialog
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
        Me.PanelEx2 = New DevComponents.DotNetBar.PanelEx()
        Me.PanelEx3 = New DevComponents.DotNetBar.PanelEx()
        Me.PanelEx4 = New DevComponents.DotNetBar.PanelEx()
        Me.X_Index = New DevComponents.Editors.IntegerInput()
        Me.X_LEN = New DevComponents.Editors.DoubleInput()
        Me.Y_Index = New DevComponents.Editors.IntegerInput()
        Me.Y_LEN = New DevComponents.Editors.DoubleInput()
        Me.Btn_Ok = New DevComponents.DotNetBar.ButtonX()
        Me.Btn_Cancel = New DevComponents.DotNetBar.ButtonX()
        Me.PanelEx5 = New DevComponents.DotNetBar.PanelEx()
        Me.PanelEx6 = New DevComponents.DotNetBar.PanelEx()
        Me.ComboBoxEx_X = New DevComponents.DotNetBar.Controls.ComboBoxEx()
        Me.ComboBoxEx_Y = New DevComponents.DotNetBar.Controls.ComboBoxEx()
        CType(Me.X_Index, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.X_LEN, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Y_Index, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Y_LEN, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'PanelEx1
        '
        Me.PanelEx1.CanvasColor = System.Drawing.SystemColors.Control
        Me.PanelEx1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.PanelEx1.DisabledBackColor = System.Drawing.Color.Empty
        Me.PanelEx1.Location = New System.Drawing.Point(215, 2)
        Me.PanelEx1.Name = "PanelEx1"
        Me.PanelEx1.Size = New System.Drawing.Size(80, 29)
        Me.PanelEx1.Style.Alignment = System.Drawing.StringAlignment.Center
        Me.PanelEx1.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground
        Me.PanelEx1.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2
        Me.PanelEx1.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.PanelEx1.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
        Me.PanelEx1.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
        Me.PanelEx1.Style.GradientAngle = 90
        Me.PanelEx1.TabIndex = 0
        Me.PanelEx1.Text = "行数"
        '
        'PanelEx2
        '
        Me.PanelEx2.CanvasColor = System.Drawing.SystemColors.Control
        Me.PanelEx2.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.PanelEx2.DisabledBackColor = System.Drawing.Color.Empty
        Me.PanelEx2.Location = New System.Drawing.Point(215, 37)
        Me.PanelEx2.Name = "PanelEx2"
        Me.PanelEx2.Size = New System.Drawing.Size(80, 29)
        Me.PanelEx2.Style.Alignment = System.Drawing.StringAlignment.Center
        Me.PanelEx2.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground
        Me.PanelEx2.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2
        Me.PanelEx2.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.PanelEx2.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
        Me.PanelEx2.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
        Me.PanelEx2.Style.GradientAngle = 90
        Me.PanelEx2.TabIndex = 0
        Me.PanelEx2.Text = "列数"
        '
        'PanelEx3
        '
        Me.PanelEx3.CanvasColor = System.Drawing.SystemColors.Control
        Me.PanelEx3.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.PanelEx3.DisabledBackColor = System.Drawing.Color.Empty
        Me.PanelEx3.Location = New System.Drawing.Point(387, 2)
        Me.PanelEx3.Name = "PanelEx3"
        Me.PanelEx3.Size = New System.Drawing.Size(80, 29)
        Me.PanelEx3.Style.Alignment = System.Drawing.StringAlignment.Center
        Me.PanelEx3.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground
        Me.PanelEx3.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2
        Me.PanelEx3.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.PanelEx3.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
        Me.PanelEx3.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
        Me.PanelEx3.Style.GradientAngle = 90
        Me.PanelEx3.TabIndex = 0
        Me.PanelEx3.Text = "行间距"
        '
        'PanelEx4
        '
        Me.PanelEx4.CanvasColor = System.Drawing.SystemColors.Control
        Me.PanelEx4.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.PanelEx4.DisabledBackColor = System.Drawing.Color.Empty
        Me.PanelEx4.Location = New System.Drawing.Point(387, 37)
        Me.PanelEx4.Name = "PanelEx4"
        Me.PanelEx4.Size = New System.Drawing.Size(80, 29)
        Me.PanelEx4.Style.Alignment = System.Drawing.StringAlignment.Center
        Me.PanelEx4.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground
        Me.PanelEx4.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2
        Me.PanelEx4.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.PanelEx4.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
        Me.PanelEx4.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
        Me.PanelEx4.Style.GradientAngle = 90
        Me.PanelEx4.TabIndex = 0
        Me.PanelEx4.Text = "列间距"
        '
        'X_Index
        '
        '
        '
        '
        Me.X_Index.BackgroundStyle.Class = "DateTimeInputBackground"
        Me.X_Index.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.X_Index.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2
        Me.X_Index.Font = New System.Drawing.Font("宋体", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(134, Byte))
        Me.X_Index.Location = New System.Drawing.Point(301, 2)
        Me.X_Index.MaxValue = 20
        Me.X_Index.MinValue = 1
        Me.X_Index.Name = "X_Index"
        Me.X_Index.ShowUpDown = True
        Me.X_Index.Size = New System.Drawing.Size(80, 29)
        Me.X_Index.TabIndex = 1
        Me.X_Index.Value = 1
        '
        'X_LEN
        '
        '
        '
        '
        Me.X_LEN.BackgroundStyle.Class = "DateTimeInputBackground"
        Me.X_LEN.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.X_LEN.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2
        Me.X_LEN.DisplayFormat = "0.000"
        Me.X_LEN.Font = New System.Drawing.Font("宋体", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(134, Byte))
        Me.X_LEN.Increment = 1.0R
        Me.X_LEN.Location = New System.Drawing.Point(473, 2)
        Me.X_LEN.MaxValue = 1000.0R
        Me.X_LEN.MinValue = -1000.0R
        Me.X_LEN.Name = "X_LEN"
        Me.X_LEN.ShowUpDown = True
        Me.X_LEN.Size = New System.Drawing.Size(80, 29)
        Me.X_LEN.TabIndex = 2
        Me.X_LEN.Value = 1.0R
        '
        'Y_Index
        '
        '
        '
        '
        Me.Y_Index.BackgroundStyle.Class = "DateTimeInputBackground"
        Me.Y_Index.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.Y_Index.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2
        Me.Y_Index.Font = New System.Drawing.Font("宋体", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(134, Byte))
        Me.Y_Index.Location = New System.Drawing.Point(301, 37)
        Me.Y_Index.MaxValue = 20
        Me.Y_Index.MinValue = 1
        Me.Y_Index.Name = "Y_Index"
        Me.Y_Index.ShowUpDown = True
        Me.Y_Index.Size = New System.Drawing.Size(80, 29)
        Me.Y_Index.TabIndex = 1
        Me.Y_Index.Value = 1
        '
        'Y_LEN
        '
        '
        '
        '
        Me.Y_LEN.BackgroundStyle.Class = "DateTimeInputBackground"
        Me.Y_LEN.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.Y_LEN.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2
        Me.Y_LEN.DisplayFormat = "0.000"
        Me.Y_LEN.Font = New System.Drawing.Font("宋体", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(134, Byte))
        Me.Y_LEN.Increment = 1.0R
        Me.Y_LEN.Location = New System.Drawing.Point(473, 37)
        Me.Y_LEN.MaxValue = 1000.0R
        Me.Y_LEN.MinValue = -1000.0R
        Me.Y_LEN.Name = "Y_LEN"
        Me.Y_LEN.ShowUpDown = True
        Me.Y_LEN.Size = New System.Drawing.Size(80, 29)
        Me.Y_LEN.TabIndex = 2
        Me.Y_LEN.Value = 1.0R
        '
        'Btn_Ok
        '
        Me.Btn_Ok.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.Btn_Ok.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground
        Me.Btn_Ok.Location = New System.Drawing.Point(3, 72)
        Me.Btn_Ok.Name = "Btn_Ok"
        Me.Btn_Ok.Size = New System.Drawing.Size(292, 38)
        Me.Btn_Ok.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.Btn_Ok.TabIndex = 3
        Me.Btn_Ok.Text = "确定"
        '
        'Btn_Cancel
        '
        Me.Btn_Cancel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.Btn_Cancel.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground
        Me.Btn_Cancel.Location = New System.Drawing.Point(301, 72)
        Me.Btn_Cancel.Name = "Btn_Cancel"
        Me.Btn_Cancel.Size = New System.Drawing.Size(252, 38)
        Me.Btn_Cancel.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.Btn_Cancel.TabIndex = 3
        Me.Btn_Cancel.Text = "取消"
        '
        'PanelEx5
        '
        Me.PanelEx5.CanvasColor = System.Drawing.SystemColors.Control
        Me.PanelEx5.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.PanelEx5.DisabledBackColor = System.Drawing.Color.Empty
        Me.PanelEx5.Location = New System.Drawing.Point(3, 2)
        Me.PanelEx5.Name = "PanelEx5"
        Me.PanelEx5.Size = New System.Drawing.Size(80, 29)
        Me.PanelEx5.Style.Alignment = System.Drawing.StringAlignment.Center
        Me.PanelEx5.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground
        Me.PanelEx5.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2
        Me.PanelEx5.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.PanelEx5.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
        Me.PanelEx5.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
        Me.PanelEx5.Style.GradientAngle = 90
        Me.PanelEx5.TabIndex = 0
        Me.PanelEx5.Text = "行轴名称"
        '
        'PanelEx6
        '
        Me.PanelEx6.CanvasColor = System.Drawing.SystemColors.Control
        Me.PanelEx6.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.PanelEx6.DisabledBackColor = System.Drawing.Color.Empty
        Me.PanelEx6.Location = New System.Drawing.Point(3, 37)
        Me.PanelEx6.Name = "PanelEx6"
        Me.PanelEx6.Size = New System.Drawing.Size(80, 29)
        Me.PanelEx6.Style.Alignment = System.Drawing.StringAlignment.Center
        Me.PanelEx6.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground
        Me.PanelEx6.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2
        Me.PanelEx6.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.PanelEx6.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
        Me.PanelEx6.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
        Me.PanelEx6.Style.GradientAngle = 90
        Me.PanelEx6.TabIndex = 0
        Me.PanelEx6.Text = "列轴名称"
        '
        'ComboBoxEx_X
        '
        Me.ComboBoxEx_X.DisplayMember = "Text"
        Me.ComboBoxEx_X.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
        Me.ComboBoxEx_X.Font = New System.Drawing.Font("宋体", 14.25!)
        Me.ComboBoxEx_X.FormattingEnabled = True
        Me.ComboBoxEx_X.ItemHeight = 23
        Me.ComboBoxEx_X.Location = New System.Drawing.Point(89, 2)
        Me.ComboBoxEx_X.Name = "ComboBoxEx_X"
        Me.ComboBoxEx_X.Size = New System.Drawing.Size(121, 29)
        Me.ComboBoxEx_X.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.ComboBoxEx_X.TabIndex = 4
        '
        'ComboBoxEx_Y
        '
        Me.ComboBoxEx_Y.DisplayMember = "Text"
        Me.ComboBoxEx_Y.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
        Me.ComboBoxEx_Y.Font = New System.Drawing.Font("宋体", 14.25!)
        Me.ComboBoxEx_Y.FormattingEnabled = True
        Me.ComboBoxEx_Y.ItemHeight = 23
        Me.ComboBoxEx_Y.Location = New System.Drawing.Point(89, 37)
        Me.ComboBoxEx_Y.Name = "ComboBoxEx_Y"
        Me.ComboBoxEx_Y.Size = New System.Drawing.Size(121, 29)
        Me.ComboBoxEx_Y.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.ComboBoxEx_Y.TabIndex = 4
        '
        'Auto_InsertPos_Dialog
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(554, 116)
        Me.Controls.Add(Me.ComboBoxEx_Y)
        Me.Controls.Add(Me.ComboBoxEx_X)
        Me.Controls.Add(Me.Btn_Cancel)
        Me.Controls.Add(Me.Btn_Ok)
        Me.Controls.Add(Me.Y_LEN)
        Me.Controls.Add(Me.X_LEN)
        Me.Controls.Add(Me.Y_Index)
        Me.Controls.Add(Me.X_Index)
        Me.Controls.Add(Me.PanelEx4)
        Me.Controls.Add(Me.PanelEx6)
        Me.Controls.Add(Me.PanelEx2)
        Me.Controls.Add(Me.PanelEx3)
        Me.Controls.Add(Me.PanelEx5)
        Me.Controls.Add(Me.PanelEx1)
        Me.Name = "Auto_InsertPos_Dialog"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "自动生成坐标"
        CType(Me.X_Index, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.X_LEN, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Y_Index, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Y_LEN, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents PanelEx1 As DevComponents.DotNetBar.PanelEx
    Friend WithEvents PanelEx2 As DevComponents.DotNetBar.PanelEx
    Friend WithEvents PanelEx3 As DevComponents.DotNetBar.PanelEx
    Friend WithEvents PanelEx4 As DevComponents.DotNetBar.PanelEx
    Friend WithEvents X_Index As DevComponents.Editors.IntegerInput
    Friend WithEvents X_LEN As DevComponents.Editors.DoubleInput
    Friend WithEvents Y_Index As DevComponents.Editors.IntegerInput
    Friend WithEvents Y_LEN As DevComponents.Editors.DoubleInput
    Friend WithEvents Btn_Ok As DevComponents.DotNetBar.ButtonX
    Friend WithEvents Btn_Cancel As DevComponents.DotNetBar.ButtonX
    Friend WithEvents PanelEx5 As DevComponents.DotNetBar.PanelEx
    Friend WithEvents PanelEx6 As DevComponents.DotNetBar.PanelEx
    Friend WithEvents ComboBoxEx_X As DevComponents.DotNetBar.Controls.ComboBoxEx
    Friend WithEvents ComboBoxEx_Y As DevComponents.DotNetBar.Controls.ComboBoxEx
End Class

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class User_Password_Dialog
    Inherits DevComponents.DotNetBar.OfficeForm

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
        Me.ButtonX2 = New DevComponents.DotNetBar.ButtonX()
        Me.Button_Cancel = New DevComponents.DotNetBar.ButtonX()
        Me.TextBoxX2 = New DevComponents.DotNetBar.Controls.TextBoxX()
        Me.TextBoxX1 = New DevComponents.DotNetBar.Controls.TextBoxX()
        Me.PanelEx12 = New DevComponents.DotNetBar.PanelEx()
        Me.PanelEx11 = New DevComponents.DotNetBar.PanelEx()
        Me.PanelEx1 = New DevComponents.DotNetBar.PanelEx()
        Me.TextBoxX3 = New DevComponents.DotNetBar.Controls.TextBoxX()
        Me.PanelEx2 = New DevComponents.DotNetBar.PanelEx()
        Me.ComboBox_Password_type = New DevComponents.DotNetBar.Controls.ComboBoxEx()
        Me.登陆密码 = New DevComponents.Editors.ComboItem()
        Me.清空数据密码 = New DevComponents.Editors.ComboItem()
        Me.删除项目密码 = New DevComponents.Editors.ComboItem()
        Me.SuspendLayout()
        '
        'ButtonX2
        '
        Me.ButtonX2.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.ButtonX2.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground
        Me.ButtonX2.Cursor = System.Windows.Forms.Cursors.Hand
        Me.ButtonX2.Location = New System.Drawing.Point(12, 152)
        Me.ButtonX2.Name = "ButtonX2"
        Me.ButtonX2.Size = New System.Drawing.Size(120, 35)
        Me.ButtonX2.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.ButtonX2.Symbol = ""
        Me.ButtonX2.TabIndex = 10
        Me.ButtonX2.TabStop = False
        Me.ButtonX2.Text = "修改密码"
        '
        'Button_Cancel
        '
        Me.Button_Cancel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.Button_Cancel.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground
        Me.Button_Cancel.Cursor = System.Windows.Forms.Cursors.Hand
        Me.Button_Cancel.Location = New System.Drawing.Point(158, 152)
        Me.Button_Cancel.Name = "Button_Cancel"
        Me.Button_Cancel.Size = New System.Drawing.Size(120, 35)
        Me.Button_Cancel.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.Button_Cancel.Symbol = ""
        Me.Button_Cancel.TabIndex = 9
        Me.Button_Cancel.TabStop = False
        Me.Button_Cancel.Text = "取消修改"
        '
        'TextBoxX2
        '
        Me.TextBoxX2.BackColor = System.Drawing.Color.White
        '
        '
        '
        Me.TextBoxX2.Border.Class = "TextBoxBorder"
        Me.TextBoxX2.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.TextBoxX2.DisabledBackColor = System.Drawing.Color.White
        Me.TextBoxX2.Font = New System.Drawing.Font("宋体", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(134, Byte))
        Me.TextBoxX2.ForeColor = System.Drawing.Color.Black
        Me.TextBoxX2.Location = New System.Drawing.Point(78, 82)
        Me.TextBoxX2.Name = "TextBoxX2"
        Me.TextBoxX2.PasswordChar = Global.Microsoft.VisualBasic.ChrW(35)
        Me.TextBoxX2.PreventEnterBeep = True
        Me.TextBoxX2.Size = New System.Drawing.Size(200, 29)
        Me.TextBoxX2.TabIndex = 13
        '
        'TextBoxX1
        '
        Me.TextBoxX1.BackColor = System.Drawing.Color.White
        '
        '
        '
        Me.TextBoxX1.Border.Class = "TextBoxBorder"
        Me.TextBoxX1.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.TextBoxX1.DisabledBackColor = System.Drawing.Color.White
        Me.TextBoxX1.Font = New System.Drawing.Font("宋体", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(134, Byte))
        Me.TextBoxX1.ForeColor = System.Drawing.Color.Black
        Me.TextBoxX1.Location = New System.Drawing.Point(78, 47)
        Me.TextBoxX1.Name = "TextBoxX1"
        Me.TextBoxX1.PasswordChar = Global.Microsoft.VisualBasic.ChrW(35)
        Me.TextBoxX1.PreventEnterBeep = True
        Me.TextBoxX1.Size = New System.Drawing.Size(200, 29)
        Me.TextBoxX1.TabIndex = 14
        '
        'PanelEx12
        '
        Me.PanelEx12.CanvasColor = System.Drawing.SystemColors.Control
        Me.PanelEx12.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.PanelEx12.DisabledBackColor = System.Drawing.Color.Empty
        Me.PanelEx12.Location = New System.Drawing.Point(12, 82)
        Me.PanelEx12.Name = "PanelEx12"
        Me.PanelEx12.Size = New System.Drawing.Size(60, 29)
        Me.PanelEx12.Style.Alignment = System.Drawing.StringAlignment.Center
        Me.PanelEx12.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground
        Me.PanelEx12.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.PanelEx12.Style.BorderColor.Color = System.Drawing.Color.Black
        Me.PanelEx12.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
        Me.PanelEx12.Style.GradientAngle = 90
        Me.PanelEx12.TabIndex = 11
        Me.PanelEx12.Text = "新密码"
        '
        'PanelEx11
        '
        Me.PanelEx11.CanvasColor = System.Drawing.SystemColors.Control
        Me.PanelEx11.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.PanelEx11.DisabledBackColor = System.Drawing.Color.Empty
        Me.PanelEx11.Location = New System.Drawing.Point(12, 47)
        Me.PanelEx11.Name = "PanelEx11"
        Me.PanelEx11.Size = New System.Drawing.Size(60, 29)
        Me.PanelEx11.Style.Alignment = System.Drawing.StringAlignment.Center
        Me.PanelEx11.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground
        Me.PanelEx11.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.PanelEx11.Style.BorderColor.Color = System.Drawing.Color.Black
        Me.PanelEx11.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
        Me.PanelEx11.Style.GradientAngle = 90
        Me.PanelEx11.TabIndex = 12
        Me.PanelEx11.Text = "原始密码"
        '
        'PanelEx1
        '
        Me.PanelEx1.CanvasColor = System.Drawing.SystemColors.Control
        Me.PanelEx1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.PanelEx1.DisabledBackColor = System.Drawing.Color.Empty
        Me.PanelEx1.Location = New System.Drawing.Point(12, 117)
        Me.PanelEx1.Name = "PanelEx1"
        Me.PanelEx1.Size = New System.Drawing.Size(60, 29)
        Me.PanelEx1.Style.Alignment = System.Drawing.StringAlignment.Center
        Me.PanelEx1.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground
        Me.PanelEx1.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.PanelEx1.Style.BorderColor.Color = System.Drawing.Color.Black
        Me.PanelEx1.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
        Me.PanelEx1.Style.GradientAngle = 90
        Me.PanelEx1.TabIndex = 11
        Me.PanelEx1.Text = "新密码"
        '
        'TextBoxX3
        '
        Me.TextBoxX3.BackColor = System.Drawing.Color.White
        '
        '
        '
        Me.TextBoxX3.Border.Class = "TextBoxBorder"
        Me.TextBoxX3.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.TextBoxX3.DisabledBackColor = System.Drawing.Color.White
        Me.TextBoxX3.Font = New System.Drawing.Font("宋体", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(134, Byte))
        Me.TextBoxX3.ForeColor = System.Drawing.Color.Black
        Me.TextBoxX3.Location = New System.Drawing.Point(78, 117)
        Me.TextBoxX3.Name = "TextBoxX3"
        Me.TextBoxX3.PasswordChar = Global.Microsoft.VisualBasic.ChrW(35)
        Me.TextBoxX3.PreventEnterBeep = True
        Me.TextBoxX3.Size = New System.Drawing.Size(200, 29)
        Me.TextBoxX3.TabIndex = 13
        '
        'PanelEx2
        '
        Me.PanelEx2.CanvasColor = System.Drawing.SystemColors.Control
        Me.PanelEx2.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.PanelEx2.DisabledBackColor = System.Drawing.Color.Empty
        Me.PanelEx2.Location = New System.Drawing.Point(12, 12)
        Me.PanelEx2.Name = "PanelEx2"
        Me.PanelEx2.Size = New System.Drawing.Size(60, 29)
        Me.PanelEx2.Style.Alignment = System.Drawing.StringAlignment.Center
        Me.PanelEx2.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground
        Me.PanelEx2.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.PanelEx2.Style.BorderColor.Color = System.Drawing.Color.Black
        Me.PanelEx2.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
        Me.PanelEx2.Style.GradientAngle = 90
        Me.PanelEx2.TabIndex = 12
        Me.PanelEx2.Text = "密码类型"
        '
        'ComboBox_Password_type
        '
        Me.ComboBox_Password_type.Cursor = System.Windows.Forms.Cursors.Hand
        Me.ComboBox_Password_type.DisplayMember = "Text"
        Me.ComboBox_Password_type.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
        Me.ComboBox_Password_type.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox_Password_type.Font = New System.Drawing.Font("宋体", 14.25!)
        Me.ComboBox_Password_type.ItemHeight = 23
        Me.ComboBox_Password_type.Items.AddRange(New Object() {Me.登陆密码, Me.清空数据密码, Me.删除项目密码})
        Me.ComboBox_Password_type.Location = New System.Drawing.Point(78, 12)
        Me.ComboBox_Password_type.Name = "ComboBox_Password_type"
        Me.ComboBox_Password_type.Size = New System.Drawing.Size(200, 29)
        Me.ComboBox_Password_type.Style = DevComponents.DotNetBar.eDotNetBarStyle.OfficeMobile2014
        Me.ComboBox_Password_type.TabIndex = 15
        Me.ComboBox_Password_type.TabStop = False
        '
        '登陆密码
        '
        Me.登陆密码.Text = "登陆密码"
        '
        '清空数据密码
        '
        Me.清空数据密码.Text = "清空数据密码"
        '
        '删除项目密码
        '
        Me.删除项目密码.Text = "删除项目密码"
        '
        'User_Password_Dialog
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(291, 202)
        Me.Controls.Add(Me.ComboBox_Password_type)
        Me.Controls.Add(Me.TextBoxX3)
        Me.Controls.Add(Me.TextBoxX2)
        Me.Controls.Add(Me.TextBoxX1)
        Me.Controls.Add(Me.PanelEx1)
        Me.Controls.Add(Me.PanelEx12)
        Me.Controls.Add(Me.PanelEx2)
        Me.Controls.Add(Me.PanelEx11)
        Me.Controls.Add(Me.ButtonX2)
        Me.Controls.Add(Me.Button_Cancel)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "User_Password_Dialog"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "用户修改密码"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ButtonX2 As DevComponents.DotNetBar.ButtonX
    Friend WithEvents Button_Cancel As DevComponents.DotNetBar.ButtonX
    Friend WithEvents TextBoxX2 As DevComponents.DotNetBar.Controls.TextBoxX
    Friend WithEvents TextBoxX1 As DevComponents.DotNetBar.Controls.TextBoxX
    Friend WithEvents PanelEx12 As DevComponents.DotNetBar.PanelEx
    Friend WithEvents PanelEx11 As DevComponents.DotNetBar.PanelEx
    Friend WithEvents PanelEx1 As DevComponents.DotNetBar.PanelEx
    Friend WithEvents TextBoxX3 As DevComponents.DotNetBar.Controls.TextBoxX
    Friend WithEvents PanelEx2 As DevComponents.DotNetBar.PanelEx
    Private WithEvents ComboBox_Password_type As DevComponents.DotNetBar.Controls.ComboBoxEx
    Friend WithEvents 登陆密码 As DevComponents.Editors.ComboItem
    Friend WithEvents 清空数据密码 As DevComponents.Editors.ComboItem
    Friend WithEvents 删除项目密码 As DevComponents.Editors.ComboItem

End Class

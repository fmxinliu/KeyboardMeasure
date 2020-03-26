<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Updata_ColPos_Dialog
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
        Me.Btn_CANCEL = New DevComponents.DotNetBar.ButtonX()
        Me.GroupPanel9 = New DevComponents.DotNetBar.Controls.GroupPanel()
        Me.DX_Add_Axis = New System.Windows.Forms.DataGridView()
        Me.Btn_OK = New DevComponents.DotNetBar.ButtonX()
        Me.坐标轴名称 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column2 = New DevComponents.DotNetBar.Controls.DataGridViewDoubleInputColumn()
        Me.Column1 = New DevComponents.DotNetBar.Controls.DataGridViewCheckBoxXColumn()
        Me.GroupPanel9.SuspendLayout()
        CType(Me.DX_Add_Axis, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Btn_CANCEL
        '
        Me.Btn_CANCEL.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.Btn_CANCEL.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground
        Me.Btn_CANCEL.Location = New System.Drawing.Point(155, 355)
        Me.Btn_CANCEL.Name = "Btn_CANCEL"
        Me.Btn_CANCEL.Size = New System.Drawing.Size(146, 36)
        Me.Btn_CANCEL.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.Btn_CANCEL.TabIndex = 24
        Me.Btn_CANCEL.Text = "CANCEL"
        '
        'GroupPanel9
        '
        Me.GroupPanel9.CanvasColor = System.Drawing.SystemColors.Control
        Me.GroupPanel9.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007
        Me.GroupPanel9.Controls.Add(Me.DX_Add_Axis)
        Me.GroupPanel9.DisabledBackColor = System.Drawing.Color.Empty
        Me.GroupPanel9.Font = New System.Drawing.Font("宋体", 10.5!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(134, Byte))
        Me.GroupPanel9.Location = New System.Drawing.Point(2, 1)
        Me.GroupPanel9.Name = "GroupPanel9"
        Me.GroupPanel9.Size = New System.Drawing.Size(299, 349)
        '
        '
        '
        Me.GroupPanel9.Style.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2
        Me.GroupPanel9.Style.BackColorGradientAngle = 90
        Me.GroupPanel9.Style.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground
        Me.GroupPanel9.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid
        Me.GroupPanel9.Style.BorderBottomWidth = 1
        Me.GroupPanel9.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
        Me.GroupPanel9.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid
        Me.GroupPanel9.Style.BorderLeftWidth = 1
        Me.GroupPanel9.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid
        Me.GroupPanel9.Style.BorderRightWidth = 1
        Me.GroupPanel9.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid
        Me.GroupPanel9.Style.BorderTopWidth = 1
        Me.GroupPanel9.Style.CornerDiameter = 4
        Me.GroupPanel9.Style.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.GroupPanel9.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center
        Me.GroupPanel9.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
        Me.GroupPanel9.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near
        '
        '
        '
        Me.GroupPanel9.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square
        '
        '
        '
        Me.GroupPanel9.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.GroupPanel9.TabIndex = 22
        '
        'DX_Add_Axis
        '
        Me.DX_Add_Axis.AllowUserToAddRows = False
        Me.DX_Add_Axis.AllowUserToDeleteRows = False
        Me.DX_Add_Axis.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DX_Add_Axis.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.坐标轴名称, Me.Column2, Me.Column1})
        Me.DX_Add_Axis.Dock = System.Windows.Forms.DockStyle.Fill
        Me.DX_Add_Axis.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnF2
        Me.DX_Add_Axis.Location = New System.Drawing.Point(0, 0)
        Me.DX_Add_Axis.Name = "DX_Add_Axis"
        Me.DX_Add_Axis.ReadOnly = True
        Me.DX_Add_Axis.RowTemplate.Height = 23
        Me.DX_Add_Axis.Size = New System.Drawing.Size(297, 347)
        Me.DX_Add_Axis.TabIndex = 0
        '
        'Btn_OK
        '
        Me.Btn_OK.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.Btn_OK.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground
        Me.Btn_OK.Location = New System.Drawing.Point(3, 356)
        Me.Btn_OK.Name = "Btn_OK"
        Me.Btn_OK.Size = New System.Drawing.Size(146, 36)
        Me.Btn_OK.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.Btn_OK.TabIndex = 23
        Me.Btn_OK.Text = "OK"
        '
        '坐标轴名称
        '
        Me.坐标轴名称.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
        Me.坐标轴名称.HeaderText = "轴名称"
        Me.坐标轴名称.Name = "坐标轴名称"
        Me.坐标轴名称.ReadOnly = True
        '
        'Column2
        '
        '
        '
        '
        Me.Column2.BackgroundStyle.Class = "DataGridViewNumericBorder"
        Me.Column2.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.Column2.DisplayFormat = "0.000"
        Me.Column2.HeaderText = "修改值"
        Me.Column2.Increment = 1.0R
        Me.Column2.Name = "Column2"
        Me.Column2.ReadOnly = True
        Me.Column2.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        '
        'Column1
        '
        Me.Column1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
        Me.Column1.Checked = True
        Me.Column1.CheckState = System.Windows.Forms.CheckState.Indeterminate
        Me.Column1.CheckValue = "N"
        Me.Column1.HeaderText = "是否修改"
        Me.Column1.Name = "Column1"
        Me.Column1.ReadOnly = True
        Me.Column1.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.Column1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        '
        'Updata_ColPos_Dialog
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(308, 395)
        Me.ControlBox = False
        Me.Controls.Add(Me.Btn_CANCEL)
        Me.Controls.Add(Me.GroupPanel9)
        Me.Controls.Add(Me.Btn_OK)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "Updata_ColPos_Dialog"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "统一单列所有坐标"
        Me.GroupPanel9.ResumeLayout(False)
        CType(Me.DX_Add_Axis, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Private WithEvents Btn_CANCEL As DevComponents.DotNetBar.ButtonX
    Friend WithEvents GroupPanel9 As DevComponents.DotNetBar.Controls.GroupPanel
    Friend WithEvents DX_Add_Axis As System.Windows.Forms.DataGridView
    Private WithEvents Btn_OK As DevComponents.DotNetBar.ButtonX
    Friend WithEvents 坐标轴名称 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column2 As DevComponents.DotNetBar.Controls.DataGridViewDoubleInputColumn
    Friend WithEvents Column1 As DevComponents.DotNetBar.Controls.DataGridViewCheckBoxXColumn
End Class

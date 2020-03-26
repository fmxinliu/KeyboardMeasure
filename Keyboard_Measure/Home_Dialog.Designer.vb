<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Home_Dialog
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
        Me.components = New System.ComponentModel.Container()
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.StyleManager = New DevComponents.DotNetBar.StyleManager(Me.components)
        Me.DataGridViewX1 = New DevComponents.DotNetBar.Controls.DataGridViewX()
        Me.ContextMenuStrip = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.单轴回原点ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        Me.所有轴回原点ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.取消ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.Btn_Add = New DevComponents.DotNetBar.ButtonX()
        Me.Btn_Del = New DevComponents.DotNetBar.ButtonX()
        Me.Btn_Updata = New DevComponents.DotNetBar.ButtonX()
        Me.Btn_Servo_On = New DevComponents.DotNetBar.ButtonX()
        Me.BK_HOME = New System.ComponentModel.BackgroundWorker()
        Me.Btn_Exit = New DevComponents.DotNetBar.ButtonX()
        CType(Me.DataGridViewX1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ContextMenuStrip.SuspendLayout()
        Me.SuspendLayout()
        '
        'StyleManager
        '
        Me.StyleManager.ManagerStyle = DevComponents.DotNetBar.eStyle.Office2010Black
        Me.StyleManager.MetroColorParameters = New DevComponents.DotNetBar.Metro.ColorTables.MetroColorGeneratorParameters(System.Drawing.Color.White, System.Drawing.Color.FromArgb(CType(CType(43, Byte), Integer), CType(CType(87, Byte), Integer), CType(CType(154, Byte), Integer)))
        '
        'DataGridViewX1
        '
        Me.DataGridViewX1.AllowUserToAddRows = False
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle1.Font = New System.Drawing.Font("宋体", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(134, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DataGridViewX1.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.DataGridViewX1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridViewX1.ContextMenuStrip = Me.ContextMenuStrip
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle2.Font = New System.Drawing.Font("宋体", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(134, Byte))
        DataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.DataGridViewX1.DefaultCellStyle = DataGridViewCellStyle2
        Me.DataGridViewX1.EnableHeadersVisualStyles = False
        Me.DataGridViewX1.GridColor = System.Drawing.Color.FromArgb(CType(CType(170, Byte), Integer), CType(CType(170, Byte), Integer), CType(CType(170, Byte), Integer))
        Me.DataGridViewX1.Location = New System.Drawing.Point(2, 2)
        Me.DataGridViewX1.Name = "DataGridViewX1"
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle3.Font = New System.Drawing.Font("宋体", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(134, Byte))
        DataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DataGridViewX1.RowHeadersDefaultCellStyle = DataGridViewCellStyle3
        Me.DataGridViewX1.RowTemplate.Height = 23
        Me.DataGridViewX1.Size = New System.Drawing.Size(853, 319)
        Me.DataGridViewX1.TabIndex = 0
        '
        'ContextMenuStrip
        '
        Me.ContextMenuStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.单轴回原点ToolStripMenuItem, Me.ToolStripSeparator2, Me.所有轴回原点ToolStripMenuItem, Me.ToolStripSeparator1, Me.取消ToolStripMenuItem})
        Me.ContextMenuStrip.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip.Size = New System.Drawing.Size(153, 104)
        '
        '单轴回原点ToolStripMenuItem
        '
        Me.单轴回原点ToolStripMenuItem.Name = "单轴回原点ToolStripMenuItem"
        Me.单轴回原点ToolStripMenuItem.Size = New System.Drawing.Size(152, 22)
        Me.单轴回原点ToolStripMenuItem.Text = "单轴回原点"
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(149, 6)
        '
        '所有轴回原点ToolStripMenuItem
        '
        Me.所有轴回原点ToolStripMenuItem.Enabled = False
        Me.所有轴回原点ToolStripMenuItem.Name = "所有轴回原点ToolStripMenuItem"
        Me.所有轴回原点ToolStripMenuItem.Size = New System.Drawing.Size(152, 22)
        Me.所有轴回原点ToolStripMenuItem.Text = "所有轴回原点"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(149, 6)
        '
        '取消ToolStripMenuItem
        '
        Me.取消ToolStripMenuItem.Name = "取消ToolStripMenuItem"
        Me.取消ToolStripMenuItem.Size = New System.Drawing.Size(152, 22)
        Me.取消ToolStripMenuItem.Text = "取消"
        '
        'Btn_Add
        '
        Me.Btn_Add.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.Btn_Add.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground
        Me.Btn_Add.Location = New System.Drawing.Point(2, 326)
        Me.Btn_Add.Name = "Btn_Add"
        Me.Btn_Add.Size = New System.Drawing.Size(81, 32)
        Me.Btn_Add.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.Btn_Add.Symbol = ""
        Me.Btn_Add.TabIndex = 1
        Me.Btn_Add.Text = "新增"
        '
        'Btn_Del
        '
        Me.Btn_Del.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.Btn_Del.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground
        Me.Btn_Del.Location = New System.Drawing.Point(176, 325)
        Me.Btn_Del.Name = "Btn_Del"
        Me.Btn_Del.Size = New System.Drawing.Size(81, 32)
        Me.Btn_Del.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.Btn_Del.Symbol = ""
        Me.Btn_Del.TabIndex = 1
        Me.Btn_Del.Text = "删除"
        '
        'Btn_Updata
        '
        Me.Btn_Updata.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.Btn_Updata.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground
        Me.Btn_Updata.Enabled = False
        Me.Btn_Updata.Location = New System.Drawing.Point(89, 325)
        Me.Btn_Updata.Name = "Btn_Updata"
        Me.Btn_Updata.Size = New System.Drawing.Size(81, 32)
        Me.Btn_Updata.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.Btn_Updata.Symbol = ""
        Me.Btn_Updata.TabIndex = 1
        Me.Btn_Updata.Text = "更新"
        '
        'Btn_Servo_On
        '
        Me.Btn_Servo_On.AccessibleRole = System.Windows.Forms.AccessibleRole.None
        Me.Btn_Servo_On.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground
        Me.Btn_Servo_On.Location = New System.Drawing.Point(525, 327)
        Me.Btn_Servo_On.Name = "Btn_Servo_On"
        Me.Btn_Servo_On.Size = New System.Drawing.Size(169, 31)
        Me.Btn_Servo_On.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.Btn_Servo_On.Symbol = ""
        Me.Btn_Servo_On.TabIndex = 2
        Me.Btn_Servo_On.Text = "伺服On"
        '
        'BK_HOME
        '
        Me.BK_HOME.WorkerReportsProgress = True
        Me.BK_HOME.WorkerSupportsCancellation = True
        '
        'Btn_Exit
        '
        Me.Btn_Exit.AccessibleRole = System.Windows.Forms.AccessibleRole.None
        Me.Btn_Exit.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground
        Me.Btn_Exit.Location = New System.Drawing.Point(700, 327)
        Me.Btn_Exit.Name = "Btn_Exit"
        Me.Btn_Exit.Size = New System.Drawing.Size(154, 31)
        Me.Btn_Exit.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.Btn_Exit.Symbol = ""
        Me.Btn_Exit.TabIndex = 2
        Me.Btn_Exit.Text = "退出"
        '
        'Home_Dialog
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(858, 361)
        Me.ControlBox = False
        Me.Controls.Add(Me.Btn_Exit)
        Me.Controls.Add(Me.Btn_Servo_On)
        Me.Controls.Add(Me.Btn_Updata)
        Me.Controls.Add(Me.Btn_Del)
        Me.Controls.Add(Me.Btn_Add)
        Me.Controls.Add(Me.DataGridViewX1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.Name = "Home_Dialog"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        CType(Me.DataGridViewX1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ContextMenuStrip.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents StyleManager As DevComponents.DotNetBar.StyleManager
    Friend WithEvents DataGridViewX1 As DevComponents.DotNetBar.Controls.DataGridViewX
    Friend WithEvents Btn_Add As DevComponents.DotNetBar.ButtonX
    Friend WithEvents Btn_Del As DevComponents.DotNetBar.ButtonX
    Friend WithEvents Btn_Updata As DevComponents.DotNetBar.ButtonX
    Friend WithEvents Btn_Servo_On As DevComponents.DotNetBar.ButtonX
    Friend WithEvents BK_HOME As System.ComponentModel.BackgroundWorker
    Friend WithEvents Btn_Exit As DevComponents.DotNetBar.ButtonX
    Friend WithEvents ContextMenuStrip As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents 单轴回原点ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents 取消ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents 所有轴回原点ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripSeparator2 As System.Windows.Forms.ToolStripSeparator

End Class

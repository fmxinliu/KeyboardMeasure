<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class WARING
    Inherits DevComponents.DotNetBar.Office2007Form

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
        Me.RUN_Thread = New System.ComponentModel.BackgroundWorker()
        Me.PE_Waring = New DevComponents.DotNetBar.Controls.WarningBox()
        Me.Timer_Index = New System.Windows.Forms.Timer(Me.components)
        Me.SuspendLayout()
        '
        'RUN_Thread
        '
        Me.RUN_Thread.WorkerReportsProgress = True
        Me.RUN_Thread.WorkerSupportsCancellation = True
        '
        'PE_Waring
        '
        Me.PE_Waring.BackColor = System.Drawing.Color.FromArgb(CType(CType(211, Byte), Integer), CType(CType(211, Byte), Integer), CType(CType(211, Byte), Integer))
        Me.PE_Waring.CloseButtonVisible = False
        Me.PE_Waring.Font = New System.Drawing.Font("宋体", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(134, Byte))
        Me.PE_Waring.ForeColor = System.Drawing.Color.Red
        Me.PE_Waring.Location = New System.Drawing.Point(-1, 1)
        Me.PE_Waring.Name = "PE_Waring"
        Me.PE_Waring.OptionsButtonVisible = False
        Me.PE_Waring.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.PE_Waring.Size = New System.Drawing.Size(891, 89)
        Me.PE_Waring.TabIndex = 0
        Me.PE_Waring.Text = "报警界面"
        '
        'Timer_Index
        '
        Me.Timer_Index.Interval = 1000
        '
        'WARING
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(893, 95)
        Me.ControlBox = False
        Me.Controls.Add(Me.PE_Waring)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "WARING"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "WARING"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents RUN_Thread As System.ComponentModel.BackgroundWorker
    Friend WithEvents PE_Waring As DevComponents.DotNetBar.Controls.WarningBox
    Friend WithEvents Timer_Index As System.Windows.Forms.Timer
End Class

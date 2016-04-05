<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class zipSettings
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(zipSettings))
        Me.lstOptions = New System.Windows.Forms.ListBox()
        Me.lstSelected = New System.Windows.Forms.ListBox()
        Me.btnMove = New System.Windows.Forms.Button()
        Me.btnRemove = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.chkSelectAll = New System.Windows.Forms.CheckBox()
        Me.txtDays = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.btnEmail = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'lstOptions
        '
        Me.lstOptions.FormattingEnabled = True
        Me.lstOptions.ItemHeight = 16
        Me.lstOptions.Items.AddRange(New Object() {"AnnotationConversionLog", "BusinessServices", "BusinessServicesCommon", "BusinessServicesCommon_Endpoints", "BusinessServicesCommon_EndpointTrace", "BusinessServicesCommon_StabilityMonitoring", "DatabaseUpgrade", "Infrastructure", "IntegrationService", "License", "MAP_log", "Orchestration", "ProcessingHost_error", "ProcessingHost_info", "ProcessingManager_info", "ProductionSetObjectConversionLog", "ProcessingServices", "SearchLog", "site_server", "ThreatBridge", "WorkDistribution", "WorkManager"})
        Me.lstOptions.Location = New System.Drawing.Point(12, 63)
        Me.lstOptions.Name = "lstOptions"
        Me.lstOptions.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended
        Me.lstOptions.Size = New System.Drawing.Size(274, 196)
        Me.lstOptions.TabIndex = 0
        '
        'lstSelected
        '
        Me.lstSelected.FormattingEnabled = True
        Me.lstSelected.ItemHeight = 16
        Me.lstSelected.Location = New System.Drawing.Point(365, 63)
        Me.lstSelected.Name = "lstSelected"
        Me.lstSelected.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended
        Me.lstSelected.Size = New System.Drawing.Size(251, 196)
        Me.lstSelected.TabIndex = 1
        '
        'btnMove
        '
        Me.btnMove.Location = New System.Drawing.Point(292, 114)
        Me.btnMove.Name = "btnMove"
        Me.btnMove.Size = New System.Drawing.Size(53, 23)
        Me.btnMove.TabIndex = 2
        Me.btnMove.Text = ">>"
        Me.btnMove.UseVisualStyleBackColor = True
        '
        'btnRemove
        '
        Me.btnRemove.Location = New System.Drawing.Point(292, 183)
        Me.btnRemove.Name = "btnRemove"
        Me.btnRemove.Size = New System.Drawing.Size(53, 23)
        Me.btnRemove.TabIndex = 3
        Me.btnRemove.Text = "<<"
        Me.btnRemove.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(13, 40)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(104, 17)
        Me.Label1.TabIndex = 4
        Me.Label1.Text = "Available Logs:"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(362, 40)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(110, 17)
        Me.Label2.TabIndex = 5
        Me.Label2.Text = "Logs to Archive:"
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(12, 279)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(83, 27)
        Me.Button1.TabIndex = 6
        Me.Button1.Text = "Zip Logs"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'chkSelectAll
        '
        Me.chkSelectAll.AutoSize = True
        Me.chkSelectAll.Location = New System.Drawing.Point(493, 287)
        Me.chkSelectAll.Name = "chkSelectAll"
        Me.chkSelectAll.Size = New System.Drawing.Size(123, 21)
        Me.chkSelectAll.TabIndex = 7
        Me.chkSelectAll.Text = "Select All Logs"
        Me.chkSelectAll.UseVisualStyleBackColor = True
        '
        'txtDays
        '
        Me.txtDays.Location = New System.Drawing.Point(339, 286)
        Me.txtDays.Name = "txtDays"
        Me.txtDays.Size = New System.Drawing.Size(25, 22)
        Me.txtDays.TabIndex = 8
        Me.txtDays.Text = "30"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(289, 287)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(44, 17)
        Me.Label3.TabIndex = 9
        Me.Label3.Text = "Days:"
        '
        'btnEmail
        '
        Me.btnEmail.Location = New System.Drawing.Point(101, 279)
        Me.btnEmail.Name = "btnEmail"
        Me.btnEmail.Size = New System.Drawing.Size(80, 27)
        Me.btnEmail.TabIndex = 10
        Me.btnEmail.Text = "Email"
        Me.btnEmail.UseVisualStyleBackColor = True
        Me.btnEmail.Visible = False
        '
        'zipSettings
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(634, 322)
        Me.Controls.Add(Me.btnEmail)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.txtDays)
        Me.Controls.Add(Me.chkSelectAll)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.btnRemove)
        Me.Controls.Add(Me.btnMove)
        Me.Controls.Add(Me.lstSelected)
        Me.Controls.Add(Me.lstOptions)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "zipSettings"
        Me.Text = "Log Retriever"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents lstOptions As ListBox
    Friend WithEvents lstSelected As ListBox
    Friend WithEvents btnMove As Button
    Friend WithEvents btnRemove As Button
    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Button1 As Button
    Friend WithEvents chkSelectAll As CheckBox
    Friend WithEvents txtDays As TextBox
    Friend WithEvents Label3 As Label
    Friend WithEvents btnEmail As Button
End Class

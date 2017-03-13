<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
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

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.picPlot = New System.Windows.Forms.PictureBox
        Me.tmrTX = New System.Windows.Forms.Timer(Me.components)
        Me.trmPaint = New System.Windows.Forms.Timer(Me.components)
        Me.serCOM = New System.IO.Ports.SerialPort(Me.components)
        Me.cmbCOM = New System.Windows.Forms.ComboBox
        Me.btnCOM = New System.Windows.Forms.Button
        Me.lblTime = New System.Windows.Forms.Label
        Me.lblTimel = New System.Windows.Forms.Label
        Me.lblV = New System.Windows.Forms.Label
        Me.lblVl = New System.Windows.Forms.Label
        Me.lblIl = New System.Windows.Forms.Label
        Me.lblI = New System.Windows.Forms.Label
        Me.lblP = New System.Windows.Forms.Label
        Me.lblPl = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.lblE = New System.Windows.Forms.Label
        Me.lblIdl = New System.Windows.Forms.Label
        Me.lblId = New System.Windows.Forms.Label
        Me.lblIql = New System.Windows.Forms.Label
        Me.lblIq = New System.Windows.Forms.Label
        Me.lblPhase = New System.Windows.Forms.Label
        Me.lblPhasel = New System.Windows.Forms.Label
        Me.lblMagl = New System.Windows.Forms.Label
        Me.lblMag = New System.Windows.Forms.Label
        Me.lblIal = New System.Windows.Forms.Label
        Me.lblIa = New System.Windows.Forms.Label
        Me.lblIbl = New System.Windows.Forms.Label
        Me.lblIb = New System.Windows.Forms.Label
        Me.lblIcl = New System.Windows.Forms.Label
        Me.lblIc = New System.Windows.Forms.Label
        Me.lblHalll = New System.Windows.Forms.Label
        Me.lblHall = New System.Windows.Forms.Label
        Me.lblSpeedl = New System.Windows.Forms.Label
        Me.lblSpeed = New System.Windows.Forms.Label
        Me.lblRPM = New System.Windows.Forms.Label
        Me.lblRPMl = New System.Windows.Forms.Label
        Me.lblAccell = New System.Windows.Forms.Label
        Me.lblAccel = New System.Windows.Forms.Label
        Me.trkAccel = New System.Windows.Forms.TrackBar
        Me.trkPhase = New System.Windows.Forms.TrackBar
        Me.lbAcceltl = New System.Windows.Forms.Label
        Me.lblPhasetl = New System.Windows.Forms.Label
        Me.lblAccelt = New System.Windows.Forms.Label
        Me.lblPhaset = New System.Windows.Forms.Label
        Me.chkTX = New System.Windows.Forms.CheckBox
        Me.lblData1 = New System.Windows.Forms.Label
        Me.lblData2 = New System.Windows.Forms.Label
        Me.lblRPMf = New System.Windows.Forms.Label
        Me.lblHalls = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.trkRPM = New System.Windows.Forms.TrackBar
        Me.txtRPM = New System.Windows.Forms.TextBox
        Me.btnRPM = New System.Windows.Forms.Button
        Me.lblRPMs = New System.Windows.Forms.Label
        CType(Me.picPlot, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.trkAccel, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.trkPhase, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.trkRPM, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'picPlot
        '
        Me.picPlot.BackColor = System.Drawing.Color.DimGray
        Me.picPlot.Location = New System.Drawing.Point(12, 92)
        Me.picPlot.Name = "picPlot"
        Me.picPlot.Size = New System.Drawing.Size(640, 480)
        Me.picPlot.TabIndex = 0
        Me.picPlot.TabStop = False
        '
        'tmrTX
        '
        Me.tmrTX.Interval = 50
        '
        'trmPaint
        '
        Me.trmPaint.Enabled = True
        Me.trmPaint.Interval = 50
        '
        'serCOM
        '
        Me.serCOM.BaudRate = 57600
        Me.serCOM.DtrEnable = True
        Me.serCOM.Parity = System.IO.Ports.Parity.Even
        Me.serCOM.RtsEnable = True
        '
        'cmbCOM
        '
        Me.cmbCOM.BackColor = System.Drawing.Color.DarkBlue
        Me.cmbCOM.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbCOM.ForeColor = System.Drawing.Color.White
        Me.cmbCOM.FormattingEnabled = True
        Me.cmbCOM.Location = New System.Drawing.Point(12, 12)
        Me.cmbCOM.Name = "cmbCOM"
        Me.cmbCOM.Size = New System.Drawing.Size(121, 28)
        Me.cmbCOM.TabIndex = 1
        Me.cmbCOM.Text = "COM Port"
        '
        'btnCOM
        '
        Me.btnCOM.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnCOM.Location = New System.Drawing.Point(13, 46)
        Me.btnCOM.Name = "btnCOM"
        Me.btnCOM.Size = New System.Drawing.Size(120, 28)
        Me.btnCOM.TabIndex = 2
        Me.btnCOM.Text = "Connect"
        Me.btnCOM.UseVisualStyleBackColor = True
        '
        'lblTime
        '
        Me.lblTime.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblTime.Font = New System.Drawing.Font("Microsoft Sans Serif", 20.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTime.ForeColor = System.Drawing.Color.Blue
        Me.lblTime.Location = New System.Drawing.Point(139, 12)
        Me.lblTime.Name = "lblTime"
        Me.lblTime.Size = New System.Drawing.Size(143, 40)
        Me.lblTime.TabIndex = 3
        Me.lblTime.Text = "0.00"
        Me.lblTime.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblTimel
        '
        Me.lblTimel.AutoSize = True
        Me.lblTimel.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTimel.ForeColor = System.Drawing.Color.White
        Me.lblTimel.Location = New System.Drawing.Point(180, 54)
        Me.lblTimel.Name = "lblTimel"
        Me.lblTimel.Size = New System.Drawing.Size(102, 20)
        Me.lblTimel.TabIndex = 4
        Me.lblTimel.Text = "Data Time [s]"
        '
        'lblV
        '
        Me.lblV.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblV.Font = New System.Drawing.Font("Microsoft Sans Serif", 20.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblV.ForeColor = System.Drawing.Color.Blue
        Me.lblV.Location = New System.Drawing.Point(704, 9)
        Me.lblV.Name = "lblV"
        Me.lblV.Size = New System.Drawing.Size(143, 40)
        Me.lblV.TabIndex = 5
        Me.lblV.Text = "0.00"
        Me.lblV.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblVl
        '
        Me.lblVl.AutoSize = True
        Me.lblVl.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblVl.ForeColor = System.Drawing.Color.White
        Me.lblVl.Location = New System.Drawing.Point(701, 51)
        Me.lblVl.Name = "lblVl"
        Me.lblVl.Size = New System.Drawing.Size(146, 20)
        Me.lblVl.TabIndex = 6
        Me.lblVl.Text = "DC Bus Voltage [V]"
        '
        'lblIl
        '
        Me.lblIl.AutoSize = True
        Me.lblIl.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblIl.ForeColor = System.Drawing.Color.White
        Me.lblIl.Location = New System.Drawing.Point(884, 51)
        Me.lblIl.Name = "lblIl"
        Me.lblIl.Size = New System.Drawing.Size(112, 20)
        Me.lblIl.TabIndex = 7
        Me.lblIl.Text = "DC Current [A]"
        '
        'lblI
        '
        Me.lblI.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblI.Font = New System.Drawing.Font("Microsoft Sans Serif", 20.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblI.ForeColor = System.Drawing.Color.Blue
        Me.lblI.Location = New System.Drawing.Point(853, 9)
        Me.lblI.Name = "lblI"
        Me.lblI.Size = New System.Drawing.Size(143, 40)
        Me.lblI.TabIndex = 8
        Me.lblI.Text = "0.00"
        Me.lblI.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblP
        '
        Me.lblP.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblP.Font = New System.Drawing.Font("Microsoft Sans Serif", 20.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblP.ForeColor = System.Drawing.Color.Blue
        Me.lblP.Location = New System.Drawing.Point(704, 80)
        Me.lblP.Name = "lblP"
        Me.lblP.Size = New System.Drawing.Size(143, 40)
        Me.lblP.TabIndex = 9
        Me.lblP.Text = "0"
        Me.lblP.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblPl
        '
        Me.lblPl.AutoSize = True
        Me.lblPl.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPl.ForeColor = System.Drawing.Color.White
        Me.lblPl.Location = New System.Drawing.Point(740, 120)
        Me.lblPl.Name = "lblPl"
        Me.lblPl.Size = New System.Drawing.Size(107, 20)
        Me.lblPl.TabIndex = 10
        Me.lblPl.Text = "DC Power [W]"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.White
        Me.Label1.Location = New System.Drawing.Point(874, 120)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(122, 20)
        Me.Label1.TabIndex = 12
        Me.Label1.Text = "DC Energy [Wh]"
        '
        'lblE
        '
        Me.lblE.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblE.Font = New System.Drawing.Font("Microsoft Sans Serif", 20.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblE.ForeColor = System.Drawing.Color.Blue
        Me.lblE.Location = New System.Drawing.Point(853, 80)
        Me.lblE.Name = "lblE"
        Me.lblE.Size = New System.Drawing.Size(143, 40)
        Me.lblE.TabIndex = 11
        Me.lblE.Text = "0.00"
        Me.lblE.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblIdl
        '
        Me.lblIdl.AutoSize = True
        Me.lblIdl.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblIdl.ForeColor = System.Drawing.Color.White
        Me.lblIdl.Location = New System.Drawing.Point(861, 260)
        Me.lblIdl.Name = "lblIdl"
        Me.lblIdl.Size = New System.Drawing.Size(135, 20)
        Me.lblIdl.TabIndex = 20
        Me.lblIdl.Text = "D-Axis Current [A]"
        '
        'lblId
        '
        Me.lblId.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblId.Font = New System.Drawing.Font("Microsoft Sans Serif", 20.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblId.ForeColor = System.Drawing.Color.Blue
        Me.lblId.Location = New System.Drawing.Point(853, 220)
        Me.lblId.Name = "lblId"
        Me.lblId.Size = New System.Drawing.Size(143, 40)
        Me.lblId.TabIndex = 19
        Me.lblId.Text = "0.00"
        Me.lblId.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblIql
        '
        Me.lblIql.AutoSize = True
        Me.lblIql.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblIql.ForeColor = System.Drawing.Color.White
        Me.lblIql.Location = New System.Drawing.Point(712, 260)
        Me.lblIql.Name = "lblIql"
        Me.lblIql.Size = New System.Drawing.Size(135, 20)
        Me.lblIql.TabIndex = 18
        Me.lblIql.Text = "Q-Axis Current [A]"
        '
        'lblIq
        '
        Me.lblIq.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblIq.Font = New System.Drawing.Font("Microsoft Sans Serif", 20.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblIq.ForeColor = System.Drawing.Color.Blue
        Me.lblIq.Location = New System.Drawing.Point(704, 220)
        Me.lblIq.Name = "lblIq"
        Me.lblIq.Size = New System.Drawing.Size(143, 40)
        Me.lblIq.TabIndex = 17
        Me.lblIq.Text = "0.00"
        Me.lblIq.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblPhase
        '
        Me.lblPhase.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblPhase.Font = New System.Drawing.Font("Microsoft Sans Serif", 20.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPhase.ForeColor = System.Drawing.Color.Blue
        Me.lblPhase.Location = New System.Drawing.Point(853, 149)
        Me.lblPhase.Name = "lblPhase"
        Me.lblPhase.Size = New System.Drawing.Size(143, 40)
        Me.lblPhase.TabIndex = 16
        Me.lblPhase.Text = "0.0"
        Me.lblPhase.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblPhasel
        '
        Me.lblPhasel.AutoSize = True
        Me.lblPhasel.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPhasel.ForeColor = System.Drawing.Color.White
        Me.lblPhasel.Location = New System.Drawing.Point(861, 191)
        Me.lblPhasel.Name = "lblPhasel"
        Me.lblPhasel.Size = New System.Drawing.Size(135, 20)
        Me.lblPhasel.TabIndex = 15
        Me.lblPhasel.Text = "PWM Phase [deg]"
        '
        'lblMagl
        '
        Me.lblMagl.AutoSize = True
        Me.lblMagl.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMagl.ForeColor = System.Drawing.Color.White
        Me.lblMagl.Location = New System.Drawing.Point(701, 191)
        Me.lblMagl.Name = "lblMagl"
        Me.lblMagl.Size = New System.Drawing.Size(152, 20)
        Me.lblMagl.TabIndex = 14
        Me.lblMagl.Text = "PWM Magnitude [%]"
        '
        'lblMag
        '
        Me.lblMag.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblMag.Font = New System.Drawing.Font("Microsoft Sans Serif", 20.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMag.ForeColor = System.Drawing.Color.Blue
        Me.lblMag.Location = New System.Drawing.Point(704, 149)
        Me.lblMag.Name = "lblMag"
        Me.lblMag.Size = New System.Drawing.Size(143, 40)
        Me.lblMag.TabIndex = 13
        Me.lblMag.Text = "0.0"
        Me.lblMag.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblIal
        '
        Me.lblIal.AutoSize = True
        Me.lblIal.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblIal.ForeColor = System.Drawing.Color.White
        Me.lblIal.Location = New System.Drawing.Point(847, 441)
        Me.lblIal.Name = "lblIal"
        Me.lblIal.Size = New System.Drawing.Size(149, 20)
        Me.lblIal.TabIndex = 22
        Me.lblIal.Text = "Phase A Current [A]"
        '
        'lblIa
        '
        Me.lblIa.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblIa.Font = New System.Drawing.Font("Microsoft Sans Serif", 20.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblIa.ForeColor = System.Drawing.Color.Blue
        Me.lblIa.Location = New System.Drawing.Point(853, 401)
        Me.lblIa.Name = "lblIa"
        Me.lblIa.Size = New System.Drawing.Size(143, 40)
        Me.lblIa.TabIndex = 21
        Me.lblIa.Text = "0.00"
        Me.lblIa.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblIbl
        '
        Me.lblIbl.AutoSize = True
        Me.lblIbl.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblIbl.ForeColor = System.Drawing.Color.White
        Me.lblIbl.Location = New System.Drawing.Point(847, 511)
        Me.lblIbl.Name = "lblIbl"
        Me.lblIbl.Size = New System.Drawing.Size(149, 20)
        Me.lblIbl.TabIndex = 24
        Me.lblIbl.Text = "Phase B Current [A]"
        '
        'lblIb
        '
        Me.lblIb.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblIb.Font = New System.Drawing.Font("Microsoft Sans Serif", 20.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblIb.ForeColor = System.Drawing.Color.Blue
        Me.lblIb.Location = New System.Drawing.Point(853, 471)
        Me.lblIb.Name = "lblIb"
        Me.lblIb.Size = New System.Drawing.Size(143, 40)
        Me.lblIb.TabIndex = 23
        Me.lblIb.Text = "0.00"
        Me.lblIb.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblIcl
        '
        Me.lblIcl.AutoSize = True
        Me.lblIcl.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblIcl.ForeColor = System.Drawing.Color.White
        Me.lblIcl.Location = New System.Drawing.Point(847, 582)
        Me.lblIcl.Name = "lblIcl"
        Me.lblIcl.Size = New System.Drawing.Size(149, 20)
        Me.lblIcl.TabIndex = 26
        Me.lblIcl.Text = "Phase C Current [A]"
        '
        'lblIc
        '
        Me.lblIc.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblIc.Font = New System.Drawing.Font("Microsoft Sans Serif", 20.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblIc.ForeColor = System.Drawing.Color.Blue
        Me.lblIc.Location = New System.Drawing.Point(853, 542)
        Me.lblIc.Name = "lblIc"
        Me.lblIc.Size = New System.Drawing.Size(143, 40)
        Me.lblIc.TabIndex = 25
        Me.lblIc.Text = "0.00"
        Me.lblIc.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblHalll
        '
        Me.lblHalll.AutoSize = True
        Me.lblHalll.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblHalll.ForeColor = System.Drawing.Color.White
        Me.lblHalll.Location = New System.Drawing.Point(847, 652)
        Me.lblHalll.Name = "lblHalll"
        Me.lblHalll.Size = New System.Drawing.Size(148, 20)
        Me.lblHalll.TabIndex = 29
        Me.lblHalll.Text = "Sensor State [ABC]"
        '
        'lblHall
        '
        Me.lblHall.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblHall.Font = New System.Drawing.Font("Microsoft Sans Serif", 20.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblHall.ForeColor = System.Drawing.Color.Blue
        Me.lblHall.Location = New System.Drawing.Point(853, 612)
        Me.lblHall.Name = "lblHall"
        Me.lblHall.Size = New System.Drawing.Size(143, 40)
        Me.lblHall.TabIndex = 28
        Me.lblHall.Text = "001"
        Me.lblHall.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblSpeedl
        '
        Me.lblSpeedl.AutoSize = True
        Me.lblSpeedl.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSpeedl.ForeColor = System.Drawing.Color.White
        Me.lblSpeedl.Location = New System.Drawing.Point(495, 659)
        Me.lblSpeedl.Name = "lblSpeedl"
        Me.lblSpeedl.Size = New System.Drawing.Size(160, 20)
        Me.lblSpeedl.TabIndex = 34
        Me.lblSpeedl.Text = "Ground Speed [km/h]"
        '
        'lblSpeed
        '
        Me.lblSpeed.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblSpeed.Font = New System.Drawing.Font("Microsoft Sans Serif", 36.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSpeed.ForeColor = System.Drawing.Color.Blue
        Me.lblSpeed.Location = New System.Drawing.Point(465, 593)
        Me.lblSpeed.Name = "lblSpeed"
        Me.lblSpeed.Size = New System.Drawing.Size(187, 66)
        Me.lblSpeed.TabIndex = 33
        Me.lblSpeed.Text = "0.0"
        Me.lblSpeed.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblRPM
        '
        Me.lblRPM.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblRPM.Font = New System.Drawing.Font("Microsoft Sans Serif", 21.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRPM.ForeColor = System.Drawing.Color.Blue
        Me.lblRPM.Location = New System.Drawing.Point(227, 621)
        Me.lblRPM.Name = "lblRPM"
        Me.lblRPM.Size = New System.Drawing.Size(109, 38)
        Me.lblRPM.TabIndex = 31
        Me.lblRPM.Text = "0"
        Me.lblRPM.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblRPMl
        '
        Me.lblRPMl.AutoSize = True
        Me.lblRPMl.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRPMl.ForeColor = System.Drawing.Color.White
        Me.lblRPMl.Location = New System.Drawing.Point(274, 593)
        Me.lblRPMl.Name = "lblRPMl"
        Me.lblRPMl.Size = New System.Drawing.Size(140, 20)
        Me.lblRPMl.TabIndex = 32
        Me.lblRPMl.Text = "Motor Speed [rpm]"
        '
        'lblAccell
        '
        Me.lblAccell.AutoSize = True
        Me.lblAccell.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAccell.ForeColor = System.Drawing.Color.White
        Me.lblAccell.Location = New System.Drawing.Point(21, 659)
        Me.lblAccell.Name = "lblAccell"
        Me.lblAccell.Size = New System.Drawing.Size(200, 20)
        Me.lblAccell.TabIndex = 36
        Me.lblAccell.Text = "Acceleration Command [%]"
        '
        'lblAccel
        '
        Me.lblAccel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblAccel.Font = New System.Drawing.Font("Microsoft Sans Serif", 36.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAccel.ForeColor = System.Drawing.Color.Blue
        Me.lblAccel.Location = New System.Drawing.Point(34, 593)
        Me.lblAccel.Name = "lblAccel"
        Me.lblAccel.Size = New System.Drawing.Size(187, 66)
        Me.lblAccel.TabIndex = 35
        Me.lblAccel.Text = "-100.0"
        Me.lblAccel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'trkAccel
        '
        Me.trkAccel.LargeChange = 10
        Me.trkAccel.Location = New System.Drawing.Point(692, 377)
        Me.trkAccel.Maximum = 255
        Me.trkAccel.Name = "trkAccel"
        Me.trkAccel.Orientation = System.Windows.Forms.Orientation.Vertical
        Me.trkAccel.Size = New System.Drawing.Size(45, 251)
        Me.trkAccel.TabIndex = 37
        Me.trkAccel.TickFrequency = 10
        '
        'trkPhase
        '
        Me.trkPhase.LargeChange = 1092
        Me.trkPhase.Location = New System.Drawing.Point(781, 377)
        Me.trkPhase.Maximum = 21845
        Me.trkPhase.Name = "trkPhase"
        Me.trkPhase.Orientation = System.Windows.Forms.Orientation.Vertical
        Me.trkPhase.Size = New System.Drawing.Size(45, 251)
        Me.trkPhase.TabIndex = 38
        Me.trkPhase.TickFrequency = 1092
        '
        'lbAcceltl
        '
        Me.lbAcceltl.AutoSize = True
        Me.lbAcceltl.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbAcceltl.ForeColor = System.Drawing.Color.White
        Me.lbAcceltl.Location = New System.Drawing.Point(703, 652)
        Me.lbAcceltl.Name = "lbAcceltl"
        Me.lbAcceltl.Size = New System.Drawing.Size(48, 20)
        Me.lbAcceltl.TabIndex = 39
        Me.lbAcceltl.Text = "Accel"
        '
        'lblPhasetl
        '
        Me.lblPhasetl.AutoSize = True
        Me.lblPhasetl.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPhasetl.ForeColor = System.Drawing.Color.White
        Me.lblPhasetl.Location = New System.Drawing.Point(787, 652)
        Me.lblPhasetl.Name = "lblPhasetl"
        Me.lblPhasetl.Size = New System.Drawing.Size(54, 20)
        Me.lblPhasetl.TabIndex = 40
        Me.lblPhasetl.Text = "Phase"
        '
        'lblAccelt
        '
        Me.lblAccelt.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblAccelt.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAccelt.ForeColor = System.Drawing.Color.Blue
        Me.lblAccelt.Location = New System.Drawing.Point(667, 628)
        Me.lblAccelt.Name = "lblAccelt"
        Me.lblAccelt.Size = New System.Drawing.Size(84, 24)
        Me.lblAccelt.TabIndex = 41
        Me.lblAccelt.Text = "0"
        Me.lblAccelt.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblPhaset
        '
        Me.lblPhaset.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblPhaset.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPhaset.ForeColor = System.Drawing.Color.Blue
        Me.lblPhaset.Location = New System.Drawing.Point(757, 628)
        Me.lblPhaset.Name = "lblPhaset"
        Me.lblPhaset.Size = New System.Drawing.Size(84, 24)
        Me.lblPhaset.TabIndex = 42
        Me.lblPhaset.Text = "0"
        Me.lblPhaset.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'chkTX
        '
        Me.chkTX.AutoSize = True
        Me.chkTX.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkTX.ForeColor = System.Drawing.Color.White
        Me.chkTX.Location = New System.Drawing.Point(391, 28)
        Me.chkTX.Name = "chkTX"
        Me.chkTX.Size = New System.Drawing.Size(166, 24)
        Me.chkTX.TabIndex = 43
        Me.chkTX.Text = "Transmit Command"
        Me.chkTX.UseVisualStyleBackColor = True
        '
        'lblData1
        '
        Me.lblData1.BackColor = System.Drawing.Color.DimGray
        Me.lblData1.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblData1.ForeColor = System.Drawing.Color.Yellow
        Me.lblData1.Location = New System.Drawing.Point(21, 542)
        Me.lblData1.Name = "lblData1"
        Me.lblData1.Size = New System.Drawing.Size(273, 23)
        Me.lblData1.TabIndex = 44
        Me.lblData1.Text = "Data1"
        '
        'lblData2
        '
        Me.lblData2.BackColor = System.Drawing.Color.DimGray
        Me.lblData2.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblData2.ForeColor = System.Drawing.Color.Cyan
        Me.lblData2.Location = New System.Drawing.Point(369, 542)
        Me.lblData2.Name = "lblData2"
        Me.lblData2.Size = New System.Drawing.Size(273, 20)
        Me.lblData2.TabIndex = 45
        Me.lblData2.Text = "Data2"
        Me.lblData2.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lblRPMf
        '
        Me.lblRPMf.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblRPMf.Font = New System.Drawing.Font("Microsoft Sans Serif", 21.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRPMf.ForeColor = System.Drawing.Color.Blue
        Me.lblRPMf.Location = New System.Drawing.Point(350, 621)
        Me.lblRPMf.Name = "lblRPMf"
        Me.lblRPMf.Size = New System.Drawing.Size(109, 38)
        Me.lblRPMf.TabIndex = 46
        Me.lblRPMf.Text = "0"
        Me.lblRPMf.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblHalls
        '
        Me.lblHalls.AutoSize = True
        Me.lblHalls.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblHalls.ForeColor = System.Drawing.Color.White
        Me.lblHalls.Location = New System.Drawing.Point(237, 659)
        Me.lblHalls.Name = "lblHalls"
        Me.lblHalls.Size = New System.Drawing.Size(99, 20)
        Me.lblHalls.TabIndex = 47
        Me.lblHalls.Text = "Hall Sensors"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.Color.White
        Me.Label2.Location = New System.Drawing.Point(349, 659)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(110, 20)
        Me.Label2.TabIndex = 48
        Me.Label2.Text = "Flux Estimator"
        '
        'trkRPM
        '
        Me.trkRPM.LargeChange = 100
        Me.trkRPM.Location = New System.Drawing.Point(692, 301)
        Me.trkRPM.Maximum = 10000
        Me.trkRPM.Name = "trkRPM"
        Me.trkRPM.Size = New System.Drawing.Size(300, 45)
        Me.trkRPM.SmallChange = 10
        Me.trkRPM.TabIndex = 49
        Me.trkRPM.TickFrequency = 1000
        '
        'txtRPM
        '
        Me.txtRPM.Font = New System.Drawing.Font("Microsoft Sans Serif", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtRPM.Location = New System.Drawing.Point(692, 336)
        Me.txtRPM.Name = "txtRPM"
        Me.txtRPM.Size = New System.Drawing.Size(112, 35)
        Me.txtRPM.TabIndex = 50
        Me.txtRPM.Text = "0"
        Me.txtRPM.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'btnRPM
        '
        Me.btnRPM.Font = New System.Drawing.Font("Microsoft Sans Serif", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnRPM.Location = New System.Drawing.Point(892, 336)
        Me.btnRPM.Name = "btnRPM"
        Me.btnRPM.Size = New System.Drawing.Size(94, 35)
        Me.btnRPM.TabIndex = 51
        Me.btnRPM.Text = "SET"
        Me.btnRPM.UseVisualStyleBackColor = True
        '
        'lblRPMs
        '
        Me.lblRPMs.AutoSize = True
        Me.lblRPMs.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRPMs.ForeColor = System.Drawing.Color.White
        Me.lblRPMs.Location = New System.Drawing.Point(810, 342)
        Me.lblRPMs.Name = "lblRPMs"
        Me.lblRPMs.Size = New System.Drawing.Size(59, 25)
        Me.lblRPMs.TabIndex = 52
        Me.lblRPMs.Text = "RPM"
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Black
        Me.ClientSize = New System.Drawing.Size(1008, 730)
        Me.Controls.Add(Me.lblRPMs)
        Me.Controls.Add(Me.btnRPM)
        Me.Controls.Add(Me.txtRPM)
        Me.Controls.Add(Me.trkRPM)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.lblHalls)
        Me.Controls.Add(Me.lblRPMf)
        Me.Controls.Add(Me.lblData2)
        Me.Controls.Add(Me.lblData1)
        Me.Controls.Add(Me.chkTX)
        Me.Controls.Add(Me.lblPhaset)
        Me.Controls.Add(Me.lblAccelt)
        Me.Controls.Add(Me.lblPhasetl)
        Me.Controls.Add(Me.lbAcceltl)
        Me.Controls.Add(Me.trkPhase)
        Me.Controls.Add(Me.trkAccel)
        Me.Controls.Add(Me.lblAccell)
        Me.Controls.Add(Me.lblAccel)
        Me.Controls.Add(Me.lblSpeedl)
        Me.Controls.Add(Me.lblSpeed)
        Me.Controls.Add(Me.lblRPMl)
        Me.Controls.Add(Me.lblRPM)
        Me.Controls.Add(Me.lblHalll)
        Me.Controls.Add(Me.lblHall)
        Me.Controls.Add(Me.lblIcl)
        Me.Controls.Add(Me.lblIc)
        Me.Controls.Add(Me.lblIbl)
        Me.Controls.Add(Me.lblIb)
        Me.Controls.Add(Me.lblIal)
        Me.Controls.Add(Me.lblIa)
        Me.Controls.Add(Me.lblIdl)
        Me.Controls.Add(Me.lblId)
        Me.Controls.Add(Me.lblIql)
        Me.Controls.Add(Me.lblIq)
        Me.Controls.Add(Me.lblPhase)
        Me.Controls.Add(Me.lblPhasel)
        Me.Controls.Add(Me.lblMagl)
        Me.Controls.Add(Me.lblMag)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.lblE)
        Me.Controls.Add(Me.lblPl)
        Me.Controls.Add(Me.lblP)
        Me.Controls.Add(Me.lblI)
        Me.Controls.Add(Me.lblIl)
        Me.Controls.Add(Me.lblVl)
        Me.Controls.Add(Me.lblV)
        Me.Controls.Add(Me.lblTimel)
        Me.Controls.Add(Me.lblTime)
        Me.Controls.Add(Me.btnCOM)
        Me.Controls.Add(Me.cmbCOM)
        Me.Controls.Add(Me.picPlot)
        Me.MaximizeBox = False
        Me.MaximumSize = New System.Drawing.Size(1024, 768)
        Me.MinimumSize = New System.Drawing.Size(1024, 768)
        Me.Name = "Form1"
        Me.Text = "FFv1.0 Dashboard"
        CType(Me.picPlot, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.trkAccel, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.trkPhase, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.trkRPM, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents picPlot As System.Windows.Forms.PictureBox
    Friend WithEvents tmrTX As System.Windows.Forms.Timer
    Friend WithEvents trmPaint As System.Windows.Forms.Timer
    Friend WithEvents serCOM As System.IO.Ports.SerialPort
    Friend WithEvents cmbCOM As System.Windows.Forms.ComboBox
    Friend WithEvents btnCOM As System.Windows.Forms.Button
    Friend WithEvents lblTime As System.Windows.Forms.Label
    Friend WithEvents lblTimel As System.Windows.Forms.Label
    Friend WithEvents lblV As System.Windows.Forms.Label
    Friend WithEvents lblVl As System.Windows.Forms.Label
    Friend WithEvents lblIl As System.Windows.Forms.Label
    Friend WithEvents lblI As System.Windows.Forms.Label
    Friend WithEvents lblP As System.Windows.Forms.Label
    Friend WithEvents lblPl As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents lblE As System.Windows.Forms.Label
    Friend WithEvents lblIdl As System.Windows.Forms.Label
    Friend WithEvents lblId As System.Windows.Forms.Label
    Friend WithEvents lblIql As System.Windows.Forms.Label
    Friend WithEvents lblIq As System.Windows.Forms.Label
    Friend WithEvents lblPhase As System.Windows.Forms.Label
    Friend WithEvents lblPhasel As System.Windows.Forms.Label
    Friend WithEvents lblMagl As System.Windows.Forms.Label
    Friend WithEvents lblMag As System.Windows.Forms.Label
    Friend WithEvents lblIal As System.Windows.Forms.Label
    Friend WithEvents lblIa As System.Windows.Forms.Label
    Friend WithEvents lblIbl As System.Windows.Forms.Label
    Friend WithEvents lblIb As System.Windows.Forms.Label
    Friend WithEvents lblIcl As System.Windows.Forms.Label
    Friend WithEvents lblIc As System.Windows.Forms.Label
    Friend WithEvents lblHalll As System.Windows.Forms.Label
    Friend WithEvents lblHall As System.Windows.Forms.Label
    Friend WithEvents lblSpeedl As System.Windows.Forms.Label
    Friend WithEvents lblSpeed As System.Windows.Forms.Label
    Friend WithEvents lblRPM As System.Windows.Forms.Label
    Friend WithEvents lblRPMl As System.Windows.Forms.Label
    Friend WithEvents lblAccell As System.Windows.Forms.Label
    Friend WithEvents lblAccel As System.Windows.Forms.Label
    Friend WithEvents trkAccel As System.Windows.Forms.TrackBar
    Friend WithEvents trkPhase As System.Windows.Forms.TrackBar
    Friend WithEvents lbAcceltl As System.Windows.Forms.Label
    Friend WithEvents lblPhasetl As System.Windows.Forms.Label
    Friend WithEvents lblAccelt As System.Windows.Forms.Label
    Friend WithEvents lblPhaset As System.Windows.Forms.Label
    Friend WithEvents chkTX As System.Windows.Forms.CheckBox
    Friend WithEvents lblData1 As System.Windows.Forms.Label
    Friend WithEvents lblData2 As System.Windows.Forms.Label
    Friend WithEvents lblRPMf As System.Windows.Forms.Label
    Friend WithEvents lblHalls As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents trkRPM As System.Windows.Forms.TrackBar
    Friend WithEvents txtRPM As System.Windows.Forms.TextBox
    Friend WithEvents btnRPM As System.Windows.Forms.Button
    Friend WithEvents lblRPMs As System.Windows.Forms.Label

End Class

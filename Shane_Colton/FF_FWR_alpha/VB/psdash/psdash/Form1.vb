Public Class Form1

    ' Data protocol setup.
    ' ----------------------------------------------------------------------------

    ' COM status indicator
    Dim com_status As Integer = 0
    ' COM status constants
    Const DISCONNECTED = 0
    Const OPEN = 1
    Const CONNECTED = 2

    ' COM timeout counter, interval is 50ms.
    Dim com_timeout As Integer = 5
    Const TIMEOUT = 5

    ' reserved start byte
    Const START = &HFF
    Const ESC = &HFE

    ' global RX variables
    Const RX_LEN As Integer = 56
    Const TX_LEN As Integer = 9

    Dim rx_buffer(RX_LEN) As Byte
    Dim rx_i As Integer = RX_LEN

    ' Data packets are validated by 8-bit CRC.
    ' CRC seed value
    Const CRC_SEED = &H18
    ' CRC-8 Look-up Table
    Dim CRC8LUT() As Byte = { _
    &H0, &H18, &H30, &H28, &H60, &H78, &H50, &H48, &HC0, &HD8, &HF0, &HE8, &HA0, &HB8, &H90, &H88, _
    &H98, &H80, &HA8, &HB0, &HF8, &HE0, &HC8, &HD0, &H58, &H40, &H68, &H70, &H38, &H20, &H8, &H10, _
    &H28, &H30, &H18, &H0, &H48, &H50, &H78, &H60, &HE8, &HF0, &HD8, &HC0, &H88, &H90, &HB8, &HA0, _
    &HB0, &HA8, &H80, &H98, &HD0, &HC8, &HE0, &HF8, &H70, &H68, &H40, &H58, &H10, &H8, &H20, &H38, _
    &H50, &H48, &H60, &H78, &H30, &H28, &H0, &H18, &H90, &H88, &HA0, &HB8, &HF0, &HE8, &HC0, &HD8, _
    &HC8, &HD0, &HF8, &HE0, &HA8, &HB0, &H98, &H80, &H8, &H10, &H38, &H20, &H68, &H70, &H58, &H40, _
    &H78, &H60, &H48, &H50, &H18, &H0, &H28, &H30, &HB8, &HA0, &H88, &H90, &HD8, &HC0, &HE8, &HF0, _
    &HE0, &HF8, &HD0, &HC8, &H80, &H98, &HB0, &HA8, &H20, &H38, &H10, &H8, &H40, &H58, &H70, &H68, _
    &HA0, &HB8, &H90, &H88, &HC0, &HD8, &HF0, &HE8, &H60, &H78, &H50, &H48, &H0, &H18, &H30, &H28, _
    &H38, &H20, &H8, &H10, &H58, &H40, &H68, &H70, &HF8, &HE0, &HC8, &HD0, &H98, &H80, &HA8, &HB0, _
    &H88, &H90, &HB8, &HA0, &HE8, &HF0, &HD8, &HC0, &H48, &H50, &H78, &H60, &H28, &H30, &H18, &H0, _
    &H10, &H8, &H20, &H38, &H70, &H68, &H40, &H58, &HD0, &HC8, &HE0, &HF8, &HB0, &HA8, &H80, &H98, _
    &HF0, &HE8, &HC0, &HD8, &H90, &H88, &HA0, &HB8, &H30, &H28, &H0, &H18, &H50, &H48, &H60, &H78, _
    &H68, &H70, &H58, &H40, &H8, &H10, &H38, &H20, &HA8, &HB0, &H98, &H80, &HC8, &HD0, &HF8, &HE0, _
    &HD8, &HC0, &HE8, &HF0, &HB8, &HA0, &H88, &H90, &H18, &H0, &H28, &H30, &H78, &H60, &H48, &H50, _
    &H40, &H58, &H70, &H68, &H20, &H38, &H10, &H8, &H80, &H98, &HB0, &HA8, &HE0, &HF8, &HD0, &HC8 _
    }

    ' ----------------------------------------------------------------------------

    ' State variables as received.
    ' These are defined the same here as on the STM32F103.
    Dim ia_int As Long
    Dim ib_int As Long
    Dim ic_int As Long
    Dim idc_int As Integer
    Dim Iqf As Single
    Dim Idf As Single
    Dim vdc_int As Integer
    Dim rpmt_echo As Integer
    Dim mag As Byte
    Dim phase As Byte
    Dim hallstate As Byte
    Dim fluxstate As Byte
    Dim v_idx_int_h As Integer
    Dim v_idx_int_f As Integer
    Dim speed_h As Integer
    Dim speed_f As Integer
    Dim faultstate As Integer
    Dim debug_int As Integer

    ' State variable scaling constants. 
    ' These should also be consistent with the STM32 program.
    Const KRPM As Single = 133929               ' (3/25/2012)
    Const KSPEED As Single = 2462               ' not right
    Const KMAG As Single = 0.392                ' [%/lsb]
    Const KPHASE As Single = 1.412              ' [deg/lsb]
    Const A_MIN As Integer = 540                ' [lsb]
    Const A_ZERO As Integer = 1200              ' [lsb]
    Const A_MAX As Integer = 2610               ' [lsb]

    ' State variables as displayed / logged.
    ' These are scaled to be in physical units.
    Dim v_dc_log As Single = 0.0                ' [V]
    Dim i_dc_log As Single = 0.0                ' [A]
    Dim i_a_log As Single = 0.0                 ' [A]
    Dim i_b_log As Single = 0.0                 ' [A]
    Dim i_c_log As Single = 0.0                 ' [A]
    Dim hallstate_log As Single = 0.0           ' [ABC]
    Dim speed_log_h As Single = 0.0             ' [rpm]
    Dim speed_log_f As Single = 0.0             ' [rpm]
    Dim rpmt_log As Single = 0.0                ' [rpm]
    Dim b_log As Single = 0.0                   ' [%]
    Dim mag_log As Single = 0.0                 ' [%]
    Dim phase_log As Single = 0.0               ' [deg]
    Dim Iq_log As Single = 0.0                  ' [A]
    Dim Id_log As Single = 0.0                  ' [A]

    ' Derived variables as displayed. They are not logged.
    ' These are scaled to be in physical units.
    Dim time As System.DateTime                 ' time stamp of current packet
    Dim prev_time As System.DateTime            ' time stamp of previous packet
    Dim dcpower As Single = 0.0                 ' [W]
    Dim dcenergy As Single = 0.0                ' [Wh]
    Dim i_b As Single = 0.0                     ' [A]
    Dim groundspeed As Single = 0.0             ' [mph]

    ' Variables to transmit.
    Dim accelt As Integer = 0
    Dim phaset As Integer = 0
    Dim rpmt As Integer = 0

    ' Data logging.
    Dim path As String                          ' data recording path
    Dim datafile As System.IO.StreamWriter      ' data recording file class
    Dim starttime As Date                       ' recording start time

    ' Data plotting.
    Dim dataidx As Integer
    Dim plotdata1(0 To 1023) As Single
    Dim plotdata2(0 To 1023) As Single
    Dim plotstep1 As Single
    Dim plotstep2 As Single
    Dim data1 As Integer
    Dim data2 As Integer
    Const DATAI As Integer = 1
    Const DATAV As Integer = 2
    Const DATAP As Integer = 3
    Const DATAMAG As Integer = 4
    Const DATAPHASE As Integer = 5
    Const DATAIQ As Integer = 6
    Const DATAID As Integer = 7
    Const DATAIA As Integer = 8
    Const DATAIB As Integer = 9
    Const DATAIC As Integer = 10
    Const DATAHALL As Integer = 11
    Const DATAACCEL As Integer = 12
    Const DATARPM As Integer = 13
    Const DATASPEED As Integer = 14
    Const DATARPMF As Integer = 15

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ' stop yelling at me, I don't even have a multi-threading processor
        System.Windows.Forms.Control.CheckForIllegalCrossThreadCalls = False

        ' initialize to COM1
        cmbCOM_DropDown(Nothing, Nothing)
        cmbCOM.SelectedItem = "COM1"

        reset_globals()
    End Sub

    Private Sub reset_globals()
        ' Initialize or reset global variables.

        rx_i = RX_LEN + 1

        ' Reset state variables.
        ia_int = 0
        ib_int = 0
        ic_int = 0
        idc_int = 0
        Iqf = 0.0
        Idf = 0.0
        vdc_int = 0
        rpmt_echo = 0
        mag = 0
        phase = 0
        hallstate = 0
        fluxstate = 0
        v_idx_int_h = 0
        v_idx_int_f = 0
        speed_h = 0
        speed_f = 0
        faultstate = 0

        convert_variables()

        time = System.DateTime.Now
        prev_time = System.DateTime.Now

        data1 = DATARPM
        plotstep1 = 100
        lblData1.Text = "Armature Current [100A/div]"
        data2 = DATAIQ
        plotstep2 = 1000
        lblData2.Text = "Tachometer [1000rpm/div]"

    End Sub

    Private Sub convert_variables()
        ' Convert state variables as received to physical units.

        ' State variables as logged.
        v_dc_log = vdc_int / 1000.0
        i_dc_log = idc_int / 1000.0
        i_a_log = ia_int / 1000.0
        i_b_log = ib_int / 1000.0
        i_c_log = ic_int / 1000.0
        hallstate_log = fluxstate
        speed_log_h = KRPM * speed_h / &HFFFF
        speed_log_f = KRPM * speed_f / &HFFFF
        rpmt_log = rpmt_echo
        mag_log = mag * KMAG
        phase_log = phase * KPHASE
        Iq_log = Iqf / 1000.0
        Id_log = Idf / 1000.0

        ' Derived variables as displayed.
        prev_time = time
        time = System.DateTime.Now
        dcpower = v_dc_log * i_dc_log
        dcenergy = dcpower * (time - prev_time).TotalHours
        i_b = -(i_a_log + i_c_log)
        groundspeed = KSPEED * speed_h / &HFFFF

    End Sub

    Private Sub cmbCOM_DropDown(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbCOM.DropDown
        ' Refresh the available port list.
        Dim PortList As Array
        Dim i As Integer

        For i = 0 To cmbCOM.Items.Count - 1
            cmbCOM.Items.RemoveAt(0)
        Next

        PortList = System.IO.Ports.SerialPort.GetPortNames()

        For Each PortString As String In PortList
            cmbCOM.Items.Insert(0, PortString)
        Next
    End Sub

    Private Sub btnCOM_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCOM.Click
        ' Toggle connection on or off.

        Try
            If serCOM.IsOpen() Then
                serCOM.Close()
                ' enddata()
            End If
            If com_status = OPEN Or com_status = CONNECTED Then
                tmrTX.Enabled = False
                com_status = DISCONNECTED
                btnCOM.Text = "Connect"
                end_data()
                reset_globals()
            Else
                serCOM.PortName = cmbCOM.SelectedItem
                serCOM.Open()
                'serCOM.DtrEnable = False
                'serCOM.DtrEnable = True
                com_status = OPEN
                com_timeout = 0
                btnCOM.Text = "Disconnect"
                start_data()
                tmrTX.Enabled = True
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

    End Sub

    Private Sub start_data()
        ' Begin recording to file. (Data is written every time a full packet is processed.)
        path = "scooterdata.txt"
        If System.IO.File.Exists(path) Then
            System.IO.File.Delete(path)
        End If
        datafile = System.IO.File.CreateText(path)
        starttime = System.DateTime.Now

        datafile.Write("Pneu Scooter Data Log: " + System.DateTime.Now.ToString)
        datafile.WriteLine()
        datafile.Write("Time [s], ")
        datafile.Write("DC Voltage [V], ")
        datafile.Write("DC Current [A], ")
        datafile.Write("Phase A Current [A], ")
        datafile.Write("Phase B Current [A], ")
        datafile.Write("Phase C Current [A], ")
        datafile.Write("Sensor State [ABC], ")
        datafile.Write("Flux State [ABC], ")
        datafile.Write("Sensor Speed [rpm], ")
        datafile.Write("Flux Speed [rpm], ")
        datafile.Write("Accel Command [%], ")
        datafile.Write("Brake Command [%], ")
        datafile.Write("PWM Magnitude [%], ")
        datafile.Write("PWM Phase [deg], ")
        datafile.Write("Q-Axis Current [A], ")
        datafile.Write("D-Axis Current [A], ")
        datafile.Write("Sensor V-Index [16-bit], ")
        datafile.Write("Flux V-Index [16-bit], ")
        datafile.Write("Fault State, ")
        datafile.Write("Debug Integer")
        datafile.WriteLine()
    End Sub

    Private Sub write_data()
        datafile.Write(Format((time - starttime).TotalSeconds, "0.00") + ", ")
        datafile.Write(Format(v_dc_log, "0.00") + ", ")
        datafile.Write(Format(i_dc_log, "0.00") + ", ")
        datafile.Write(Format(i_a_log, "0.00") + ", ")
        datafile.Write(Format(i_b_log, "0.00") + ", ")
        datafile.Write(Format(i_c_log, "0.00") + ", ")
        datafile.Write(Format(hallstate, "0") + ", ")
        datafile.Write(Format(fluxstate, "0") + ", ")
        datafile.Write(Format(speed_log_h, "0") + ", ")
        datafile.Write(Format(speed_log_f, "0") + ", ")
        datafile.Write(Format(rpmt_log, "0") + ", ")
        datafile.Write(Format(b_log, "0.0") + ", ")
        datafile.Write(Format(mag_log, "0.0") + ", ")
        datafile.Write(Format(phase_log, "0.0") + ", ")
        datafile.Write(Format(Iq_log, "0.00") + ", ")
        datafile.Write(Format(Id_log, "0.00") + ", ")
        datafile.Write(Format(v_idx_int_h, "0") + ", ")
        datafile.Write(Format(v_idx_int_f, "0") + ", ")
        datafile.Write(Format(faultstate, "0") + ", ")
        datafile.Write(Format(debug_int, "0"))
        datafile.WriteLine()
    End Sub

    Private Sub end_data()
        Try
            datafile.Close()
            datafile.Dispose()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub serCOM_DataReceived(ByVal sender As System.Object, ByVal e As System.IO.Ports.SerialDataReceivedEventArgs) Handles serCOM.DataReceived
        ' handles incoming data
        Dim rx_data(4096) As Byte
        Dim rx_size As Integer = 0
        Dim i As Integer = 0

        rx_size = serCOM.BytesToRead
        serCOM.Read(rx_data, 0, rx_size)

        For i = 0 To rx_size - 1
            If rx_data(i) = START Then
                ' If any byte in the rx_data stream is START (&HFF)...
                ' ... start a new packet in rx_buffer.
                rx_buffer(0) = START
                rx_i = 1
            ElseIf rx_i <= RX_LEN - 1 Then
                ' If the byte was not START and the packet is not yet full...
                ' ... add the byte to the packet and increment the index.
                rx_buffer(rx_i) = rx_data(i)
                rx_i = rx_i + 1
            End If
            If rx_i = RX_LEN Then
                ' If the packet is full, process it with rx().
                rx()
            End If
        Next
    End Sub

    Private Sub rx()
        ' Attempt to process a received data packet.

        Dim rflags As Byte = 0
        Dim i, j As Integer
        Dim rx_crc As Byte

        ' replace escaped characters
        For j = 0 To 6
            rflags = rx_buffer(50 + j)
            For i = 0 To 6
                If (rflags And (2 ^ i)) <> 0 Then           ' rflags & (0x01 << i)
                    rx_buffer(7 * j + i + 1) = START
                End If
            Next
        Next

        ' compute CRC
        rx_crc = CRC_SEED
        For i = 1 To 48
            rx_crc = CRC8LUT(rx_buffer(i) Xor rx_crc)
        Next

        If rx_buffer(49) = rx_crc Then
            ' reset COM timeout
            com_timeout = 0
            com_status = CONNECTED

            ' parse state variables
            ia_int = rx_buffer(1) * 2 ^ 24 + rx_buffer(2) * 2 ^ 16 + rx_buffer(3) * 2 ^ 8 + rx_buffer(4) - 100000
            ib_int = rx_buffer(5) * 2 ^ 24 + rx_buffer(6) * 2 ^ 16 + rx_buffer(7) * 2 ^ 8 + rx_buffer(8) - 100000
            ic_int = rx_buffer(9) * 2 ^ 24 + rx_buffer(10) * 2 ^ 16 + rx_buffer(11) * 2 ^ 8 + rx_buffer(12) - 100000

            idc_int = rx_buffer(13) * 2 ^ 24 + rx_buffer(14) * 2 ^ 16 + rx_buffer(15) * 2 ^ 8 + rx_buffer(16) - 100000
            Iqf = rx_buffer(17) * 2 ^ 24 + rx_buffer(18) * 2 ^ 16 + rx_buffer(19) * 2 ^ 8 + rx_buffer(20) - 100000.0
            Idf = rx_buffer(21) * 2 ^ 24 + rx_buffer(22) * 2 ^ 16 + rx_buffer(23) * 2 ^ 8 + rx_buffer(24) - 100000.0
            vdc_int = rx_buffer(25) * 2 ^ 24 + rx_buffer(26) * 2 ^ 16 + rx_buffer(27) * 2 ^ 8 + rx_buffer(28)
            rpmt_echo = rx_buffer(29) * 2 ^ 8 + rx_buffer(30)
            mag = rx_buffer(31)
            phase = rx_buffer(32)
            hallstate = (Int(rx_buffer(33) / 2 ^ 3)) And &H7
            fluxstate = rx_buffer(33) And &H7
            v_idx_int_h = rx_buffer(34) * 2 ^ 8 + rx_buffer(35)
            v_idx_int_f = rx_buffer(36) * 2 ^ 8 + rx_buffer(37)
            speed_h = rx_buffer(38) * 2 ^ 8 + rx_buffer(39)
            speed_f = rx_buffer(40) * 2 ^ 8 + rx_buffer(41)
            faultstate = rx_buffer(42)
            debug_int = rx_buffer(43) * 2 ^ 24 + rx_buffer(44) * 2 ^ 16 + rx_buffer(45) * 2 ^ 8 + rx_buffer(46) - 100000

            ' calculated state variables in normal units
            convert_variables()

            ' write data
            If Not datafile Is Nothing Then
                write_data()
            End If

        End If

        rx_i = RX_LEN

    End Sub

    Private Sub cleardata1()
        Dim n As Integer
        For n = 0 To 1023
            plotdata1(n) = 0
        Next
    End Sub

    Private Sub cleardata2()
        Dim n As Integer
        For n = 0 To 1023
            plotdata2(n) = 0
        Next
    End Sub

    Private Sub trmPaint_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles trmPaint.Tick
        Dim backbuffer = New Bitmap(picPlot.Width, picPlot.Height)
        Dim buffergfx = Graphics.FromImage(backbuffer)
        Dim plot As Graphics
        Dim x1, y1, x2, y2 As Single

        ' timeout handler
        If com_timeout < TIMEOUT Then
            com_timeout = com_timeout + 1
        ElseIf com_status = CONNECTED Then
            com_status = OPEN
        End If

        ' COM status indicator
        Select Case com_status
            Case DISCONNECTED : cmbCOM.BackColor = Color.DarkBlue : Exit Select
            Case OPEN : cmbCOM.BackColor = Color.Blue : Exit Select
            Case CONNECTED : cmbCOM.BackColor = Color.DeepSkyBlue : Exit Select
        End Select

        ' Fill labels.
        lblTime.Text = Format((System.DateTime.Now - starttime).TotalSeconds, "0.0")
        lblV.Text = Format(v_dc_log, "0.00")
        lblI.Text = Format(i_dc_log, "0.00")
        lblIa.Text = Format(i_a_log, "0.00")
        lblIc.Text = Format(i_c_log, "0.00")
        lblHall.Text = Format(hallstate, "0")
        lblRPM.Text = Format(speed_log_h, "0")
        lblRPMf.Text = Format(speed_log_f, "0")
        lblAccel.Text = Format(rpmt_log, "0")
        lblMag.Text = Format(mag_log, "0.0")
        lblPhase.Text = Format(phase_log, "0.0")
        lblIq.Text = Format(Iq_log, "0.00")
        lblId.Text = Format(Id_log, "0.00")
        lblP.Text = Format(dcpower, "0")
        lblE.Text = Format(dcenergy, "0.00")
        lblIb.Text = Format(i_b_log, "0.00")
        lblSpeed.Text = (Format(groundspeed, "0.0"))

        ' Fill the data circular buffer based on plot selections.
        dataidx = (dataidx + 1) Mod 1024
        Select Case data1
            Case DATAI
                plotdata1(dataidx) = i_dc_log
                Exit Select
            Case DATAV
                plotdata1(dataidx) = v_dc_log
                Exit Select
            Case DATAP
                plotdata1(dataidx) = dcpower
                Exit Select
            Case DATAMAG
                plotdata1(dataidx) = mag_log
                Exit Select
            Case DATAPHASE
                plotdata1(dataidx) = phase_log
                Exit Select
            Case DATAIQ
                plotdata1(dataidx) = Iq_log
                Exit Select
            Case DATAID
                plotdata1(dataidx) = Id_log
                Exit Select
            Case DATAIA
                plotdata1(dataidx) = i_a_log
                Exit Select
            Case DATAIB
                plotdata1(dataidx) = i_b
                Exit Select
            Case DATAIC
                plotdata1(dataidx) = i_c_log
                Exit Select
            Case DATAHALL
                plotdata1(dataidx) = hallstate_log
                Exit Select
            Case DATAACCEL
                plotdata1(dataidx) = rpmt_log
                Exit Select
            Case DATARPM
                plotdata1(dataidx) = speed_log_h
                Exit Select
            Case DATASPEED
                plotdata1(dataidx) = groundspeed
                Exit Select
            Case DATARPMF
                plotdata1(dataidx) = speed_log_f
                Exit Select
        End Select
        Select Case data2
            Case DATAI
                plotdata2(dataidx) = i_dc_log
                Exit Select
            Case DATAV
                plotdata2(dataidx) = v_dc_log
                Exit Select
            Case DATAP
                plotdata2(dataidx) = dcpower
                Exit Select
            Case DATAMAG
                plotdata2(dataidx) = mag_log
                Exit Select
            Case DATAPHASE
                plotdata2(dataidx) = phase_log
                Exit Select
            Case DATAIQ
                plotdata2(dataidx) = Iq_log
                Exit Select
            Case DATAID
                plotdata2(dataidx) = Id_log
                Exit Select
            Case DATAIA
                plotdata2(dataidx) = i_a_log
                Exit Select
            Case DATAIB
                plotdata2(dataidx) = i_b
                Exit Select
            Case DATAIC
                plotdata2(dataidx) = i_c_log
                Exit Select
            Case DATAHALL
                plotdata2(dataidx) = hallstate_log
                Exit Select
            Case DATAACCEL
                plotdata2(dataidx) = rpmt_log
                Exit Select
            Case DATARPM
                plotdata2(dataidx) = speed_log_h
                Exit Select
            Case DATASPEED
                plotdata2(dataidx) = groundspeed
                Exit Select
            Case DATARPMF
                plotdata2(dataidx) = speed_log_f
                Exit Select
        End Select

        ' Do the plotting on a back-buffer.
        buffergfx.FillRectangle(Brushes.DimGray, 0, 0, picPlot.Width, picPlot.Height)
        For n = 1 To 20
            x1 = 0
            x2 = picPlot.Width
            y1 = picPlot.Height * n / 20
            y2 = y1
            buffergfx.DrawLine(Pens.Gray, x1, y1, x2, y2)
        Next
        x1 = 0
        x2 = picPlot.Width
        y1 = picPlot.Height * 1 / 2 + 1
        y2 = y1
        buffergfx.DrawLine(Pens.Gray, x1, y1, x2, y2)
        x1 = 0
        x2 = picPlot.Width
        y1 = picPlot.Height * 1 / 2 - 1
        y2 = y1
        buffergfx.DrawLine(Pens.Gray, x1, y1, x2, y2)
        For n = 1 To picPlot.Width
            x1 = n
            y1 = (picPlot.Height / 2) - plotdata1((1024 + dataidx - picPlot.Width + n) Mod 1024) * picPlot.Height / (20 * plotstep1)
            x2 = 2
            y2 = 2
            buffergfx.DrawEllipse(Pens.Yellow, x1, y1, x2, y2)
        Next
        For n = 1 To picPlot.Width
            x1 = n
            y1 = (picPlot.Height / 2) - plotdata2((1024 + dataidx - picPlot.Width + n) Mod 1024) * picPlot.Height / (20 * plotstep2)
            x2 = 2
            y2 = 2
            buffergfx.DrawEllipse(Pens.Cyan, x1, y1, x2, y2)
        Next

        ' Plot in the foreground.
        plot = picPlot.CreateGraphics()
        plot.DrawImage(backbuffer, 0, 0)
        plot.Dispose()
        buffergfx.Dispose()
        backbuffer.Dispose()

        Me.Text = faultstate

    End Sub

    Private Sub lblAccel_MouseClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles lblAccel.MouseClick
        If e.Button = Windows.Forms.MouseButtons.Left Then
            cleardata1()
            data1 = DATAACCEL
            plotstep1 = 20
            lblData1.Text = "Acceleration Command [20%/div]"
        ElseIf e.Button = Windows.Forms.MouseButtons.Right Then
            cleardata2()
            data2 = DATAACCEL
            plotstep2 = 20
            lblData2.Text = "Acceleration Command [20%/div]"
        End If
    End Sub

    Private Sub lblRPM_MouseClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles lblRPM.MouseClick
        If e.Button = Windows.Forms.MouseButtons.Left Then
            cleardata1()
            data1 = DATARPM
            plotstep1 = 200
            lblData1.Text = "RPM (Hall Sensors) [200rpm/div]"
        ElseIf e.Button = Windows.Forms.MouseButtons.Right Then
            cleardata2()
            data2 = DATARPM
            plotstep2 = 200
            lblData2.Text = "RPM (Hall Sensors) [200rpm/div]"
        End If
    End Sub

    Private Sub lblRPMf_MouseClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles lblRPMf.MouseClick
        If e.Button = Windows.Forms.MouseButtons.Left Then
            cleardata1()
            data1 = DATARPMF
            plotstep1 = 1000
            lblData1.Text = "RPM (Flux Estimator) [1000rpm/div]"
        ElseIf e.Button = Windows.Forms.MouseButtons.Right Then
            cleardata2()
            data2 = DATARPMF
            plotstep2 = 1000
            lblData2.Text = "RPM (Flux Estimator) [1000rpm/div]"
        End If
    End Sub

    Private Sub lblSpeed_MouseClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles lblSpeed.MouseClick
        If e.Button = Windows.Forms.MouseButtons.Left Then
            cleardata1()
            data1 = DATASPEED
            plotstep1 = 5
            lblData1.Text = "Ground Speed [5km/h/div]"
        ElseIf e.Button = Windows.Forms.MouseButtons.Right Then
            cleardata2()
            data2 = DATASPEED
            plotstep2 = 5
            lblData2.Text = "Ground Speed [5km/h/div]"
        End If
    End Sub

    Private Sub lblHall_MouseClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles lblHall.MouseClick
        If e.Button = Windows.Forms.MouseButtons.Left Then
            cleardata1()
            data1 = DATAHALL
            plotstep1 = 1
            lblData1.Text = "Sensor State [1ABC/div]"
        ElseIf e.Button = Windows.Forms.MouseButtons.Right Then
            cleardata2()
            data2 = DATAHALL
            plotstep2 = 1
            lblData2.Text = "Sensor State [1ABC/div]"
        End If
    End Sub

    Private Sub lblIc_MouseClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles lblIc.MouseClick
        If e.Button = Windows.Forms.MouseButtons.Left Then
            cleardata1()
            data1 = DATAIC
            plotstep1 = 5
            lblData1.Text = "Phase C Current [5A/div]"
        ElseIf e.Button = Windows.Forms.MouseButtons.Right Then
            cleardata2()
            data2 = DATAIC
            plotstep2 = 5
            lblData2.Text = "Phase C Current [5A/div]"
        End If
    End Sub

    Private Sub lblIb_MouseClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles lblIb.MouseClick
        If e.Button = Windows.Forms.MouseButtons.Left Then
            cleardata1()
            data1 = DATAIB
            plotstep1 = 5
            lblData1.Text = "Phase B Current [5A/div]"
        ElseIf e.Button = Windows.Forms.MouseButtons.Right Then
            cleardata2()
            data2 = DATAIB
            plotstep2 = 5
            lblData2.Text = "Phase B Current [5A/div]"
        End If
    End Sub

    Private Sub lblIa_MouseClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles lblIa.MouseClick
        If e.Button = Windows.Forms.MouseButtons.Left Then
            cleardata1()
            data1 = DATAIA
            plotstep1 = 5
            lblData1.Text = "Phase A Current [5A/div]"
        ElseIf e.Button = Windows.Forms.MouseButtons.Right Then
            cleardata2()
            data2 = DATAIA
            plotstep2 = 5
            lblData2.Text = "Phase A Current [5A/div]"
        End If
    End Sub

    Private Sub lblId_MouseClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles lblId.MouseClick
        If e.Button = Windows.Forms.MouseButtons.Left Then
            cleardata1()
            data1 = DATAID
            plotstep1 = 5
            lblData1.Text = "D-Axis Current [5A/div]"
        ElseIf e.Button = Windows.Forms.MouseButtons.Right Then
            cleardata2()
            data2 = DATAID
            plotstep2 = 5
            lblData2.Text = "D-Axis Current [5A/div]"
        End If
    End Sub

    Private Sub lblIq_MouseClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles lblIq.MouseClick
        If e.Button = Windows.Forms.MouseButtons.Left Then
            cleardata1()
            data1 = DATAIQ
            plotstep1 = 5
            lblData1.Text = "Q-Axis Current [5A/div]"
        ElseIf e.Button = Windows.Forms.MouseButtons.Right Then
            cleardata2()
            data2 = DATAIQ
            plotstep2 = 5
            lblData2.Text = "Q-Axis Current [5A/div]"
        End If
    End Sub

    Private Sub lblPhase_MouseClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles lblPhase.MouseClick
        If e.Button = Windows.Forms.MouseButtons.Left Then
            cleardata1()
            data1 = DATAPHASE
            plotstep1 = 10
            lblData1.Text = "PWM Phase [10deg/div]"
        ElseIf e.Button = Windows.Forms.MouseButtons.Right Then
            cleardata2()
            data2 = DATAPHASE
            plotstep2 = 10
            lblData2.Text = "PWM Phase [10deg/div]"
        End If
    End Sub

    Private Sub lblMag_MouseClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles lblMag.MouseClick
        If e.Button = Windows.Forms.MouseButtons.Left Then
            cleardata1()
            data1 = DATAMAG
            plotstep1 = 20
            lblData1.Text = "PWM Magnitude [20%/div]"
        ElseIf e.Button = Windows.Forms.MouseButtons.Right Then
            cleardata2()
            data2 = DATAMAG
            plotstep2 = 20
            lblData2.Text = "PWM Magnitude [20%/div]"
        End If
    End Sub

    Private Sub lblP_MouseClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles lblP.MouseClick
        If e.Button = Windows.Forms.MouseButtons.Left Then
            cleardata1()
            data1 = DATAP
            plotstep1 = 100
            lblData1.Text = "DC Power [100W/div]"
        ElseIf e.Button = Windows.Forms.MouseButtons.Right Then
            cleardata2()
            data2 = DATAP
            plotstep2 = 100
            lblData2.Text = "DC Power [100W/div]"
        End If
    End Sub

    Private Sub lblI_MouseClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles lblI.MouseClick
        If e.Button = Windows.Forms.MouseButtons.Left Then
            cleardata1()
            data1 = DATAI
            plotstep1 = 5
            lblData1.Text = "DC Current [5A/div]"
        ElseIf e.Button = Windows.Forms.MouseButtons.Right Then
            cleardata2()
            data2 = DATAI
            plotstep2 = 5
            lblData2.Text = "DC Current [5A/div]"
        End If
    End Sub

    Private Sub lblV_MouseClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles lblV.MouseClick
        If e.Button = Windows.Forms.MouseButtons.Left Then
            cleardata1()
            data1 = DATAV
            plotstep1 = 5
            lblData1.Text = "DC Voltage [5V/div]"
        ElseIf e.Button = Windows.Forms.MouseButtons.Right Then
            cleardata2()
            data2 = DATAV
            plotstep2 = 5
            lblData2.Text = "DC Voltage [5V/div]"
        End If
    End Sub

    Private Sub trkAccel_Scroll(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles trkAccel.Scroll
        lblAccelt.Text = trkAccel.Value
        accelt = trkAccel.Value
    End Sub

    Private Sub trkPhase_Scroll(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles trkPhase.Scroll
        lblPhaset.Text = trkPhase.Value
        phaset = trkPhase.Value
    End Sub

    Private Sub tmrTX_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tmrTX.Tick
        Dim tx_buffer(0 To TX_LEN - 1) As Byte
        Dim i, j As Integer
        Dim rflags As Byte
        Dim tx_crc As Byte = CRC_SEED

        tx_buffer(0) = START

        tx_buffer(1) = Int(rpmt / 2 ^ 8) And &HFF
        tx_buffer(2) = rpmt And &HFF
        tx_buffer(3) = Int(phaset / 2 ^ 8) And &HFF
        tx_buffer(4) = phaset And &HFF
        tx_buffer(5) = 0
        tx_buffer(6) = 0

        ' Calculate a CRC on bytes 1-146 and put it in byte 147
        For i = 1 To 6
            tx_crc = CRC8LUT(tx_buffer(i) Xor tx_crc)
        Next
        tx_buffer(7) = tx_crc

        ' Check for restricted character (START, &HFF) in bytes 1-147.
        ' Escape and flag these bytes using 21 groups of 7 flag bits.
        For j = 0 To 0
            rflags = &H0
            For i = 0 To 6
                If tx_buffer(7 * j + i + 1) = START Then
                    tx_buffer(7 * j + i + 1) = ESC
                    rflags = rflags Or (&H1 * 2 ^ i)
                End If
            Next
            tx_buffer(8 + j) = rflags
        Next

        If chkTX.Checked = True Then
            Try
                serCOM.Write(tx_buffer, 0, TX_LEN)
            Catch Ex As Exception
                MsgBox(Ex.Message)
            End Try
        End If
    End Sub

    Private Sub btnRPM_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRPM.Click
        If (IsNumeric(txtRPM.Text)) Then
            If ((Val(txtRPM.Text) >= 0) And (Val(txtRPM.Text) <= 5000)) Then
                rpmt = Int(Val(txtRPM.Text))
                trkRPM.Value = rpmt
            End If
        End If
    End Sub

    Private Sub trkRPM_Scroll(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles trkRPM.Scroll
        txtRPM.Text = trkRPM.Value
    End Sub
End Class

Imports OMRON.Compolet.CIP
Imports System.IO
Public Class Form1
    Public Record1 As String
    Dim StoreData As New TextBox
    Dim Locate As String = " C:\SavingFile"
    Dim Locate1 As String = "" '"E:\SavingFile"
    Dim Header As String = "C:\Program Files\GDI_DataLog\Head\"
    Dim Setup As String = "C:\Program Files\GDI_DataLog\Setup\"
    Sub createfolder()
        If Not Directory.Exists(Locate & "\MC1") Then    ' HOS
            Directory.CreateDirectory(Locate & "\MC1")
        End If

        If Not Directory.Exists(Locate & "\MC2") Then    ' SOL
            Directory.CreateDirectory(Locate & "\MC2")
        End If

        If Not Directory.Exists("C:\Program Files\GDI_DataLog\Head\Head") Then
            Directory.CreateDirectory("C:\Program Files\GDI_DataLog\\Head")
        End If

        If Locate1 <> "" Then
            Try
                If Not Directory.Exists(Locate1 & "\MC1") Then    ' HOS
                    Directory.CreateDirectory(Locate1 & "\MC1")
                End If
            Catch ex As Exception
                MessageBox.Show(Locate1 & "can't create", "Error")
            End Try
            Try
                If Not Directory.Exists(Locate1 & "\MC2") Then     'SOL
                    Directory.CreateDirectory(Locate1 & "\MC2")
                End If
            Catch ex As Exception
                MessageBox.Show(Locate1 & "can't create", "Error")
            End Try

        End If

    End Sub

    Sub init1()
        '   Me.chkCJActive.Checked = SysmacCJ1.Active
        Try
            SysmacCJ1.Active = True
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
        If SysmacCJ1.Active = False Then

            MsgBox("Active Property is FALSE.")
            Exit Sub
        End If
        Try
            With SysmacCJ1
                .NetworkAddress = CLng(1)
                .NodeAddress = CLng(11)
                .UnitAddress = CLng(0)
                .ReceiveTimeLimit = CLng(750)
            End With
        Catch ex As Exception
            'MsgBox(ex.Message)
            Label3.Text = " Fault : FIN 1.11.0 Error , Check communication port and sysmate gateway"
            Label3.BackColor = Color.Red
        End Try

    End Sub
    Sub init2()
        '  Me.chkCJActive.Checked = SysmacCJ2.Active
        Try
            SysmacCJ2.Active = True
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
        If SysmacCJ2.Active = False Then

            MsgBox("Active Property is FALSE.")
            Exit Sub
        End If
        Try
            With SysmacCJ2
                .NetworkAddress = CLng(2)
                .NodeAddress = CLng(12)
                .UnitAddress = CLng(0)
                .ReceiveTimeLimit = CLng(750)
            End With
        Catch ex As Exception
            ' MsgBox(ex.Message)
            Label3.Text = " Fault : FIN 2.12.0 Error , Check communication port and sysmate gateway"
            Label3.BackColor = Color.Red
        End Try

    End Sub

    Sub loadtarget()
        Dim Target As String
        Dim Tar(1) As String
        Try
            Target = IO.File.ReadAllText(Setup & "Target.txt")
            Tar(0) = Target.Trim
        Catch ex As Exception
            '  MessageBox.Show("Setting the Target.txt file is lose", "Setting fault")
            Label3.Text = " Fault : Setting the Target.txt file is lose"
            Label3.BackColor = Color.Red
        End Try
        Locate1 = Tar(0)
    End Sub
    Dim HeaderHOS As New TextBox
    Dim HeaderSOL As New TextBox
    Dim HeaderCOV As New TextBox
    Dim MC1_Name, MC2_Name As String
    Dim HeadHOS() As String
    Dim HeadSOL() As String
    Dim HeadCOV() As String
    Sub LoadHeader() ' initial
        Dim out() As String
        Dim HeaderBUFF As New TextBox
        Try
            HeaderBUFF.Text = IO.File.ReadAllText(Setup & "Name.txt")
        Catch ex As Exception
            ' MessageBox.Show("Setting header file before Starting Solenoid welding", "Setting fault")
            Label3.Text = " Fault : Setting header file before Starting Solenoid welding"
            Label3.BackColor = Color.Red
        End Try
        out = HeaderBUFF.Text.Split(vbCrLf)
        MC1_Name = out(0).Trim
        MC2_Name = out(1).Trim

        Try
            HeaderBUFF.Text = IO.File.ReadAllText(Header & "MC1.txt")
        Catch ex As Exception
            ' MessageBox.Show("Setting header file before Starting " & MC1_Name, "Setting fault")
            Label3.Text = " Fault : Setting header file before Starting" & MC1_Name
            Label3.BackColor = Color.Red
        End Try
        HeaderHOS.Text = ""
        out = HeaderBUFF.Text.Split(vbCrLf)
        HeadHOS = out
        For i = 0 To out.Length - 1
            HeaderHOS.Text += out(i).Trim & vbTab
        Next

        Try
            HeaderBUFF.Text = IO.File.ReadAllText(Header & "MC2.txt")
        Catch ex As Exception
            '  MessageBox.Show("Setting header file before Starting " & MC2_Name, "Setting fault")
            Label3.Text = " Fault : Setting header file before Starting" & MC2_Name
            Label3.BackColor = Color.Red
        End Try
        HeaderSOL.Text = ""
        out = HeaderBUFF.Text.Split(vbCrLf)
        HeadSOL = out
        For i = 0 To out.Length - 1
            HeaderSOL.Text += out(i).Trim & vbTab
        Next

    End Sub

    Function HEXBIN(ByVal HEX As String) As String
        Dim output As UShort
        Dim outstring As String
        Dim fillStr As String = ""

        output = Convert.ToUInt16(HEX, &H10)
        outstring = Convert.ToString(output, 2)
        For i = 0 To (15 - outstring.Length)
            fillStr = fillStr & "0"
        Next
        outstring = fillStr & outstring
        Return outstring
    End Function

    Function RDDM1(ByVal DM1W As String) As String
        Dim memoryType As OMRON.Compolet.SYSMAC.SysmacCJ.MemoryTypes
        Dim txtOffset As String = DM1W
        Dim txtSize As String = "1"    ' 20 = 10word
        
        Try
            memoryType = OMRON.Compolet.SYSMAC.SysmacCJ.MemoryTypes.DM
            Return SysmacCJ1.ReadMemory(memoryType, CLng(txtOffset), CLng(txtSize), True)   ' vakue is output
        Catch ex As Exception
            Label7.Text = MC1_Name & " Disconnected 1.11.0"
            Label7.BackColor = Color.Red
            Return "0"
            ' MsgBox(ex.Message)
            Label3.Text = " error 1 "
        End Try
    End Function

    Private Sub InnerReadArea1()
        Dim memoryType As OMRON.Compolet.SYSMAC.SysmacCJ.MemoryTypes
        Dim txtOffset As String = "102"
        Dim txtSize As String = "20"    ' 14 word  := 20 data (each 4 byte)
        Dim CmbArea As String = "DM"
        
        Try
            memoryType = OMRON.Compolet.SYSMAC.SysmacCJ.MemoryTypes.DM
            Record1 = SysmacCJ1.ReadMemory(memoryType, CLng(txtOffset), CLng(txtSize), True)   ' 
            SysmacCJ1.DM(CLng("100")) = CLng("&H" & "0000")

        Catch ex As Exception
            Label7.Text = MC1_Name & " Disconnected 1.11.0"
            Label7.BackColor = Color.Red

            Label3.Text = " Fault : Setting header file before Starting"
        End Try
    End Sub
    Private Sub ClearDM1(ByVal DM As String)
        
        Try
            SysmacCJ1.DM(CLng(DM)) = CLng("&H" & "0000")    ' wrtie "0" on DM100=0
        Catch ex As Exception
           
        End Try
    End Sub

    Function RDDM2(ByVal DM1W As String) As String
        Dim memoryType As OMRON.Compolet.SYSMAC.SysmacCJ.MemoryTypes
        Dim txtOffset As String = DM1W
        Dim txtSize As String = "1"    ' 20 = 10word
      
        Try
            memoryType = OMRON.Compolet.SYSMAC.SysmacCJ.MemoryTypes.DM
            Return SysmacCJ2.ReadMemory(memoryType, CLng(txtOffset), CLng(txtSize), True)   ' vakue is output
        Catch ex As Exception
            Label8.Text = MC2_Name & " Disconnected 2.12.0"
            Label8.BackColor = Color.Red
            Return "0"
        End Try
    End Function

    Private Sub InnerReadArea2()
        Dim memoryType As OMRON.Compolet.SYSMAC.SysmacCJ.MemoryTypes
        Dim txtOffset As String = "102"
        Dim txtSize As String = "20"    ' 16 word  := 20 data (each 4 byte)
        Dim CmbArea As String = "DM"
        Try
            memoryType = OMRON.Compolet.SYSMAC.SysmacCJ.MemoryTypes.DM
            Record1 = SysmacCJ2.ReadMemory(memoryType, CLng(txtOffset), CLng(txtSize), True)
            SysmacCJ2.DM(CLng("100")) = CLng("&H" & "0000")
        Catch ex As Exception
            Label8.Text = MC2_Name & " Disconnected 2.12.0"
            Label8.BackColor = Color.Red
        End Try
    End Sub
    Private Sub ClearDM2(ByVal DM As String)
        Try
            SysmacCJ2.DM(CLng(DM)) = CLng("&H" & "0000")    ' wrtie "0" on DM100=0
        Catch ex As Exception
            Label8.Text = MC2_Name & " Disconnected 2.12.0"
            Label8.BackColor = Color.Red
            'MsgBox(ex.Message)
        End Try
    End Sub

    Dim WriteData(100) As String
    Sub WRDATA1()
        Dim Source8W As String
        Source8W = Record1
        Dim lenword As Integer = Source8W.Length
        Dim Hdword(40) As String
        Dim myDateTime As DateTime = DateTime.Now
        Dim DateNow As String = myDateTime.Day & "/" & myDateTime.Month & "/" & myDateTime.Year
        Dim TimeNow As String = Convert.ToString(myDateTime.TimeOfDay)
        Dim TimeNow1 As String
        TimeNow1 = TimeNow.Substring(0, 8)
        Dim j As Integer = 0
        Dim SeData(20) As String
        Dim HS1, HS2 As String

        For i = 0 To 35
            WriteData(i) = ""
        Next

        WriteData(j) = RDDM1("141") + RDDM1("140")
        j += 1
        WriteData(j) = RDDM1("134")
        j += 1
        WriteData(j) = RDDM1("136") & RDDM1("137") + RDDM1("138") + RDDM1("139")
        j += 1
        WriteData(j) = DateNow
        j += 1
        WriteData(j) = TimeNow1
        j += 1
        For i = 0 To (lenword - 8) Step 8
            Hdword(j) = Source8W.Substring(i + 4, 4) & Source8W.Substring(i, 4)
            WriteData(j) = ConvertHexToSingle(Hdword(j))
            j += 1
        Next

        HS1 = HexBinStr16(RDDM1("142"))
        HS2 = HexBinStr16(RDDM1("143"))

        WriteData(j) = ""
        j += 1


        For i = 0 To 6
            If HS1.Substring(15 - i, 1) = "1" Then
                WriteData(j) = "OK"
            Else
                WriteData(j) = "NG"
            End If
            j += 1
        Next

        '  For i = 0 To 5      ' Seat upper
        'If HS2.Substring(15 - i, 1) = "1" Then
        'WriteData(j) = "OK"
        ' Else
        ' WriteData(j) = "NG"
        '  End If
        ' j += 1
        ' Next

        StoreData.Text = ""
        For i = 0 To WriteData.Length - 1
            StoreData.Text += WriteData(i) & vbTab
        Next
        ' Label5.Text = j
    End Sub
    Sub WRDATA2()
        Dim Source8W As String
        Source8W = Record1
        Dim lenword As Integer = Source8W.Length
        Dim Hdword(40) As String
        Dim myDateTime As DateTime = DateTime.Now
        Dim DateNow As String = myDateTime.Day & "/" & myDateTime.Month & "/" & myDateTime.Year
        Dim TimeNow As String = Convert.ToString(myDateTime.TimeOfDay)
        Dim TimeNow1 As String
        TimeNow1 = TimeNow.Substring(0, 8)
        Dim j As Integer = 0
        Dim SeData(20) As String
        Dim SV1, SV2 As String

        For i = 0 To 35
            WriteData(i) = ""
        Next


        WriteData(j) = RDDM2("141") + RDDM2("140")
        j += 1
        WriteData(j) = RDDM2("134")
        j += 1
        WriteData(j) = RDDM2("136") & RDDM2("137") + RDDM2("138") + RDDM2("139")
        j += 1
        WriteData(j) = DateNow
        j += 1
        WriteData(j) = TimeNow1
        j += 1
        For i = 0 To (lenword - 8) Step 8
            Hdword(j) = Source8W.Substring(i + 4, 4) & Source8W.Substring(i, 4)
            WriteData(j) = ConvertHexToSingle(Hdword(j))
            j += 1
        Next
        SV1 = HexBinStr16(RDDM2("142"))
        SV2 = HexBinStr16(RDDM2("143"))

        WriteData(j) = ""
        j += 1

        For i = 0 To 6
            If SV1.Substring(15 - i, 1) = "1" Then
                WriteData(j) = "OK"
            Else
                WriteData(j) = "NG"
            End If
            j += 1
        Next


        StoreData.Text = ""
        For i = 0 To WriteData.Length - 1
            StoreData.Text += WriteData(i) & vbTab
        Next

    End Sub

    Function HexBinStr16(ByVal hex As String) As String  'Hex to bin 16 bit { 7FFFF to 0111111111111111}
        Dim data As UInteger
        Dim bin As String
        data = CLng("&H" & hex)
        bin = Convert.ToString(data, 2)
        For j = 0 To (15 - bin.Length)
            bin = "0" & bin
        Next
        Return bin
    End Function

    Function ConvertHexToSingle(ByVal hexValue As String) As Single   ' HEX 32 bits to Single 32 bit
        Try
            Dim iInputIndex As Integer = 0
            Dim iOutputIndex As Integer = 0
            Dim bArray(3) As Byte
            For iInputIndex = 0 To hexValue.Length - 1 Step 2
                bArray(iOutputIndex) = Byte.Parse(hexValue.Chars(iInputIndex) & hexValue.Chars(iInputIndex + 1), Globalization.NumberStyles.HexNumber)
                iOutputIndex += 1
            Next
            Array.Reverse(bArray)
            Return BitConverter.ToSingle(bArray, 0)
        Catch ex As Exception
            Throw New FormatException("The supplied hex value is either empty or in an incorrect format. Use the following format: 00000000", ex)
        End Try
    End Function


    Sub WriteDataHD(ByVal MC As String, ByRef DayMonthYear As String, ByVal Data As String)
        Dim filename As String = ""
        Dim filename1 As String = ""
        Dim Header As New TextBox
        Dim WriteData As String
        Select Case MC
            Case "MC1"
                Header.Text = HeaderHOS.Text & vbCrLf
                filename = Locate & "\MC1\" + DayMonthYear + ".txt"
                filename1 = Locate1 & "\MC1\" + DayMonthYear + ".txt"
            Case "MC2"
                Header.Text = HeaderSOL.Text & vbCrLf
                filename = Locate & "\MC2\" + DayMonthYear + ".txt"
                filename1 = Locate1 & "\MC2\" + DayMonthYear + ".txt"
        End Select
        WriteData = Data + vbCrLf

        ' write the normal file
        If File.Exists(filename) = True Then
            File.AppendAllText(filename, WriteData)
        Else
            Try
                File.WriteAllText(filename, Header.Text)
                File.AppendAllText(filename, WriteData)
            Catch ex As Exception
                '  MessageBox.Show(Locate & "Main HDD failure")
                Label3.Text = " Fault :  " & Locate1 & "HDD Backup failure  "
                Label3.BackColor = Color.Red
            End Try
        End If
        If Locate1 <> "" Then
            ' write the backup file
            If File.Exists(filename1) = True Then
                File.AppendAllText(filename1, WriteData)
            Else
                Try
                    File.WriteAllText(filename1, Header.Text)
                    File.AppendAllText(filename1, WriteData)
                Catch ex As Exception
                    ' MessageBox.Show(Locate1 & "HDD Backup failure ")
                    Label3.Text = " Fault :" & Locate1 & "HDD Backup failure ---> Second Harddrive"
                    Label3.BackColor = Color.Red
                End Try
            End If
        End If
    End Sub

    Dim Display1(30, 50) As String
    Sub GridView1()
        DataGridView1.Rows.Clear()
        Dim NumberColuum As Integer = 24
        Dim RowDataGrid As Integer = 20
        Dim row As String()

        DataGridView1.ColumnCount = NumberColuum
        For i = RowDataGrid To 0 Step -1
            For k = 0 To NumberColuum
                Display1(i + 1, k) = Display1(i, k)
            Next
        Next
        For k = 0 To NumberColuum
            Display1(0, k) = WriteData(k)
        Next

        For i = 0 To RowDataGrid
            row = New String() {Display1(i, 0), Display1(i, 1), Display1(i, 2), Display1(i, 3), Display1(i, 4), Display1(i, 5), Display1(i, 6), Display1(i, 7), Display1(i, 8), Display1(i, 9), Display1(i, 10), Display1(i, 11), Display1(i, 12), Display1(i, 13), Display1(i, 14), Display1(i, 15), Display1(i, 16), Display1(i, 17), Display1(i, 18), Display1(i, 19), Display1(i, 20), Display1(i, 21), Display1(i, 22), Display1(i, 23), Display1(i, 24), Display1(i, 25), Display1(i, 26), Display1(i, 27), Display1(i, 28), Display1(i, 29), Display1(i, 30), Display1(i, 31), Display1(i, 32), Display1(i, 33), Display1(i, 34), Display1(i, 35), Display1(i, 36), Display1(i, 37), Display1(i, 38), Display1(i, 39), Display1(i, 40), Display1(i, 41), Display1(i, 42), Display1(i, 43), Display1(i, 44), Display1(i, 45)}
            DataGridView1.Rows.Add(row)
        Next



        For j = 16 To 18
            For i = 0 To RowDataGrid
                If Display1(i, j) = "OK" Then
                    DataGridView1.Rows(i).Cells(j - 10).Style.BackColor = Color.LawnGreen
                ElseIf Display1(i, j) = "NG" Then
                    DataGridView1.Rows(i).Cells(j - 10).Style.BackColor = Color.Red
                End If
            Next
        Next

        For cullum = 9 To 14   'welding ok or ng checking
            For i = 0 To RowDataGrid
                If Display1(i, 19) = "OK" Then
                    DataGridView1.Rows(i).Cells(cullum).Style.BackColor = Color.LawnGreen

                ElseIf Display1(i, 19) = "NG" Then
                    DataGridView1.Rows(i).Cells(cullum).Style.BackColor = Color.Red
                End If
            Next
        Next


    End Sub
    Dim Display2(30, 50) As String
    Sub GridView2()
        DataGridView2.Rows.Clear()
        Dim NumberColuum As Integer = 24
        Dim RowDataGrid As Integer = 20
        Dim row As String()

        DataGridView2.ColumnCount = NumberColuum



        For i = RowDataGrid To 0 Step -1
            For k = 0 To NumberColuum
                Display2(i + 1, k) = Display2(i, k)
            Next
        Next
        For k = 0 To NumberColuum
            Display2(0, k) = WriteData(k)
        Next

        For i = 0 To RowDataGrid
            row = New String() {Display2(i, 0), Display2(i, 1), Display2(i, 2), Display2(i, 3), Display2(i, 4), Display2(i, 5), Display2(i, 6), Display2(i, 7), Display2(i, 8), Display2(i, 9), Display2(i, 10), Display2(i, 11), Display2(i, 12), Display2(i, 13), Display2(i, 14), Display2(i, 15), Display2(i, 16), Display2(i, 17), Display2(i, 18), Display2(i, 19), Display2(i, 20), Display2(i, 21), Display2(i, 22), Display2(i, 23), Display2(i, 24), Display2(i, 25), Display2(i, 26), Display2(i, 27), Display2(i, 28), Display2(i, 29), Display2(i, 30), Display2(i, 31), Display2(i, 32), Display2(i, 33), Display2(i, 34), Display2(i, 35), Display2(i, 36), Display2(i, 37), Display1(i, 38), Display2(i, 39), Display2(i, 40), Display2(i, 41), Display2(i, 42), Display2(i, 43), Display2(i, 44), Display2(i, 45)}
            DataGridView2.Rows.Add(row)
        Next



        For j = 16 To 18
            For i = 0 To RowDataGrid
                If Display2(i, j) = "OK" Then
                    DataGridView2.Rows(i).Cells(j - 10).Style.BackColor = Color.LawnGreen
                ElseIf Display2(i, j) = "NG" Then
                    DataGridView2.Rows(i).Cells(j - 10).Style.BackColor = Color.Red
                End If
            Next
        Next

        For cullum = 9 To 14   'welding ok or ng checking
            For i = 0 To RowDataGrid
                If Display2(i, 19) = "OK" Then
                    DataGridView2.Rows(i).Cells(cullum).Style.BackColor = Color.LawnGreen

                ElseIf Display2(i, 19) = "NG" Then
                    DataGridView2.Rows(i).Cells(cullum).Style.BackColor = Color.Red
                End If
            Next
        Next


       
    End Sub

    Private Function SystemSerialNumber() As String
        ' Get the Windows Management Instrumentation object.
        Dim wmi As Object = GetObject("WinMgmts:")

        ' Get the "base boards" (mother boards).
        Dim serial_numbers As String = ""
        Dim mother_boards As Object = wmi.InstancesOf("Win32_BaseBoard")
        For Each board As Object In mother_boards
            serial_numbers &= ", " & board.SerialNumber
        Next board
        If serial_numbers.Length > 0 Then serial_numbers = serial_numbers.Substring(2)

        Return serial_numbers
    End Function





    Private Sub Form1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.StartPosition = 0

        '   If SystemSerialNumber() <> "..CN7360405K01TK." Then
        'Me.Close()
        'End If

        Label3.Text = "Status : Normal"
        Label3.ForeColor = Color.White
        Label3.BackColor = Color.DarkGreen
        SysmacCJ1.HeartBeatTimer = 500
        SysmacCJ2.HeartBeatTimer = 500

        ' Label4.ForeColor =#FFF02222

        init1()
        init2()
        loadtarget()
        createfolder()
        LoadHeader()


    End Sub

    Private Sub Timer1_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        Dim myDateTime As DateTime = DateTime.Now
        Dim DateNow As String = myDateTime.Year & "_" & myDateTime.Month & "_" & myDateTime.Day
        Label1.Text = "MC1" & vbNewLine & "Status"
        Label1.BackColor = Color.White
        Label2.Text = "MC2" & vbNewLine & "Status"
        Label2.BackColor = Color.White
        Me.Text = "Data Saving version 1.5.0 Housing Solenoid Welding by AISA KOKI (" & Now() & " )"
        Label7.Text = MC1_Name & " Connecting 1.11.0"
        Label7.BackColor = Color.LawnGreen
        Label8.Text = MC2_Name & " Connecting 2.12.0"
        Label8.BackColor = Color.LawnGreen

        Label3.Text = "Status : Normal"
        Label3.ForeColor = Color.White
        Label3.BackColor = Color.DarkGreen

        Try
            If Val(RDDM1("100")) = 1 Then
                Label1.Text = "Getting"
                Label1.BackColor = Color.Orange
                
                InnerReadArea1()   ' read data from DM

                WRDATA1()          ' seperate DM and change to sign-decimal

                WriteDataHD("MC1", DateNow, StoreData.Text)
                GridView1()


            End If
        Catch ex As Exception
            ' MsgBox(ex.Message)
            Label3.Text = " Fault :" & ex.Message & MC1_Name
            Label3.BackColor = Color.Red
        End Try

        Try
            If Val(RDDM2("100")) = 1 Then
                Label2.Text = "Getting"
                Label2.BackColor = Color.Orange
                InnerReadArea2()   ' read data from DM

                WRDATA2()          ' seperate DM and change to sign-decimal
                WriteDataHD("MC2", DateNow, StoreData.Text)  ' write the data to HDD
                GridView2()
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
            Label3.Text = " Fault :" & ex.Message & MC2_Name
            Label3.BackColor = Color.Red
        End Try

    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub AboutToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AboutToolStripMenuItem.Click
        Dialog1.Show()
    End Sub


    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Me.Close()
    End Sub

    Private Sub HelpToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles HelpToolStripMenuItem1.Click
        System.Diagnostics.Process.Start("C:\Program Files\GDI_DataLog\Head\Help.pdf")
    End Sub

    Private Sub ToolStripMenuItem3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItem3.Click
        Form2.Visible = True
    End Sub

    Private Sub Label2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label2.Click

    End Sub

    Private Sub Label8_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label8.Click

    End Sub

    Private Sub Label3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label3.Click

    End Sub
End Class

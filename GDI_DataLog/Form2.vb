Imports System.IO
Public Class Form2
    Dim locate As String = "C:\Program Files\GDI_DataLog\Head"
 

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        If HOS.Checked = True Then
            TextBox1.Text = IO.File.ReadAllText(locate & "\MC1.txt")
        ElseIf SOL.Checked = True Then
            TextBox1.Text = IO.File.ReadAllText(locate & "MC2.txt")
        
        End If
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        If HOS.Checked = True Then
            File.WriteAllText(locate & "\MC1.txt", TextBox1.Text)
        ElseIf SOL.Checked = True Then
            File.WriteAllText(locate & "\MC2.txt", TextBox1.Text)
        End If
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        Me.Close()
        Form1.Enabled = True
    End Sub

    Private Sub Form2_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim buff, buff2 As New TextBox
        Dim out(1) As String
        TextBox1.Text = IO.File.ReadAllText(locate & "\MC1.txt")
        ' Form1.Enabled = False

        Try
            buff.Text = IO.File.ReadAllText("C:\Program Files\GDI_DataLog\Setup\Name.txt")
        Catch ex As Exception
            MessageBox.Show("File is lose", "Setting fault")
        End Try
        out = BUFF.Text.Split(vbCrLf)
        TextBox2.Text = out(0).Trim
        TextBox3.Text = out(1).Trim

        Try
            buff2.Text = IO.File.ReadAllText("C:\Program Files\GDI_DataLog\Setup\Target.txt")
        Catch ex As Exception
            MessageBox.Show("File is lose", "Setting fault")
        End Try
        out = buff2.Text.Split(vbCrLf)
        TextBox4.Text = out(0).Trim



    End Sub

   
    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        Dim MCName As String
        MCName = TextBox2.Text & vbCrLf & TextBox3.Text
        File.WriteAllText("C:\Program Files\GDI_DataLog\Setup\Name.txt", MCName)
        File.WriteAllText("C:\Program Files\GDI_DataLog\Setup\Target.txt", TextBox4.Text)
        MessageBox.Show("After change setup file ,plaease restart program again", "Warning")
        Me.Close()
    End Sub
End Class
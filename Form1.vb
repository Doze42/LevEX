Public Class Form1
    Dim tempXML As String = System.IO.Path.GetTempFileName
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Try
            Dim temp1 As String = System.IO.Path.GetTempFileName
            My.Computer.FileSystem.DeleteFile(temp1, FileIO.UIOption.OnlyErrorDialogs, FileIO.RecycleOption.DeletePermanently)
            lblStatus.Text = "Downloading Page HTML"
            My.Computer.Network.DownloadFile(txtGameURL.Text, temp1)
            Dim GameHTML As String = My.Computer.FileSystem.ReadAllText(temp1)
            Dim sidx As Integer = GameHTML.IndexOf("s: ")
            Dim cutS As String = GameHTML.Substring(sidx + 4, 8)
            Dim temp2 As String = System.IO.Path.GetTempFileName
            My.Computer.FileSystem.DeleteFile(temp2, FileIO.UIOption.OnlyErrorDialogs, FileIO.RecycleOption.DeletePermanently)
            My.Computer.Network.DownloadFile("http://www.sploder.com/php/getgameprops.php?pubkey=" + cutS, temp2)
            lblStatus.Text = "Downloading Keys"
            Dim gamevars As String = My.Computer.FileSystem.ReadAllText(temp2)
            Dim varArray As String() = gamevars.Split("&")
            Dim u As Integer = varArray(1).Substring(2, varArray(1).Length - 2)
            Dim c As String = varArray(2).Substring(2, varArray(2).Length - 2)
            Dim m As String = varArray(3).Substring(2, varArray(3).Length - 2)
            My.Computer.FileSystem.DeleteFile(tempXML, FileIO.UIOption.OnlyErrorDialogs, FileIO.RecycleOption.DeletePermanently)
            Dim url As String = "http://sploder.s3.amazonaws.com/users/group" & Math.Floor(u / 1000) & "/user" & u & "_" & c & "/projects/proj" & m & "/game.xml"
            lblStatus.Text = "Downloading XML"
            My.Computer.Network.DownloadFile(url, tempXML)
            txtXML.Text = System.Xml.Linq.XDocument.Parse(My.Computer.FileSystem.ReadAllText(tempXML)).ToString()
            lblStatus.Text = "Done"
            My.Computer.FileSystem.DeleteFile(temp1, FileIO.UIOption.OnlyErrorDialogs, FileIO.RecycleOption.DeletePermanently)
            My.Computer.FileSystem.DeleteFile(temp2, FileIO.UIOption.OnlyErrorDialogs, FileIO.RecycleOption.DeletePermanently)
            Button3.Enabled = True

        Catch err As Exception
            lblStatus.Text = "Operation Failed. See Output for details."
            txtXML.Text = err.Message
        End Try
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        SaveFileDialog.ShowDialog()
        My.Computer.FileSystem.CopyFile(tempXML, SaveFileDialog.FileName)
    End Sub
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Me.Close()
        My.Computer.FileSystem.DeleteFile(tempXML, FileIO.UIOption.OnlyErrorDialogs, FileIO.RecycleOption.DeletePermanently)
    End Sub

    Private Sub cmdClear_Click(sender As Object, e As EventArgs) Handles cmdClear.Click
        txtGameURL.Text = Nothing
        txtXML.Text = Nothing
        lblStatus.Text = "Ready"
    End Sub
    Private Sub txtGameURL_TextChanged(sender As Object, e As EventArgs) Handles txtGameURL.TextChanged
        If Not txtGameURL.Text.Trim(" ") = Nothing Then
            Button2.Enabled = True
        Else
            Button2.Enabled = False
        End If
    End Sub
End Class

Public Class Form1
    Dim tempXML As String = System.IO.Path.GetTempFileName
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim temp1 As String = System.IO.Path.GetTempFileName
        My.Computer.FileSystem.DeleteFile(temp1, FileIO.UIOption.OnlyErrorDialogs, FileIO.RecycleOption.DeletePermanently)
        My.Computer.Network.DownloadFile(txtGameURL.Text, temp1)
        Dim GameHTML As String = My.Computer.FileSystem.ReadAllText(temp1)
        Dim sidx As Integer = GameHTML.IndexOf("s: ")
        Dim cutS As String = GameHTML.Substring(sidx + 4, 8)
        Dim temp2 As String = System.IO.Path.GetTempFileName
        My.Computer.FileSystem.DeleteFile(temp2, FileIO.UIOption.OnlyErrorDialogs, FileIO.RecycleOption.DeletePermanently)
        My.Computer.Network.DownloadFile("http://www.sploder.com/php/getgameprops.php?pubkey=" + cutS, temp2)
        Dim gamevars As String = My.Computer.FileSystem.ReadAllText(temp2)
        Dim varArray As String() = gamevars.Split("&")
        Dim u As Integer = varArray(1).Substring(2, varArray(1).Length - 2)
        Dim c As String = varArray(2).Substring(2, varArray(2).Length - 2)
        Dim m As String = varArray(3).Substring(2, varArray(3).Length - 2)
        My.Computer.FileSystem.DeleteFile(tempXML, FileIO.UIOption.OnlyErrorDialogs, FileIO.RecycleOption.DeletePermanently)
        Dim url As String = "http://sploder.s3.amazonaws.com/users/group" & Math.Floor(u / 1000) & "/user" & u & "_" & c & "/projects/proj" & m & "/game.xml"
        My.Computer.Network.DownloadFile(url, tempXML)
        txtXML.Text = System.Xml.Linq.XDocument.Parse(My.Computer.FileSystem.ReadAllText(tempXML)).ToString()
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        SaveFileDialog.ShowDialog()
        My.Computer.FileSystem.CopyFile(tempXML, SaveFileDialog.FileName)
    End Sub
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Me.Close()
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub Label1_Click(sender As Object, e As EventArgs) Handles Label1.Click

    End Sub

    Private Sub txtXML_TextChanged(sender As Object, e As EventArgs) Handles txtXML.TextChanged

    End Sub

    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        Process.Start("http://www.sploder.com/")
    End Sub
End Class

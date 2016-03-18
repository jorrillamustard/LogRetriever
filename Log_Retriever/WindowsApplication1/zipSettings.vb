Imports System.IO
Imports System.IO.Compression

Public Class zipSettings
    Dim format As String = "M-d-yyyy-HH-mm-ss"

    Private Sub zipSettings_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub


    Private Function checkLog()

        Dim SS As String
        Dim SS2 As String
        Dim flag = False


        For Each c In "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray()
            If flag = False Then
                SS = c + ":\Program Files\Resolution1\SiteServer\site_server.log"
                SS2 = c + ":\Program Files\AccessData\SiteServer\site_server.log"
                If (Not System.IO.File.Exists(SS)) Then
                    flag = False
                Else
                    flag = True
                    Return c + ":\Program Files\Resolution1\SiteServer\"

                End If

                If (Not System.IO.File.Exists(SS2)) Then
                    flag = False
                Else
                    flag = True
                    Return c + ":\Program Files\AccessData\SiteServer\"
                End If
            Else
                GoTo endoffunc
            End If
        Next

endoffunc:

    End Function

    Private Function zip()
        Dim count As Integer = count + 1
        Dim R1Logs As String = "C:\Users\Public\Documents\Resolution1Logs\"
        Dim ProcLogs As String = My.Computer.Registry.GetValue(
    "HKEY_LOCAL_MACHINE\SOFTWARE\AccessData\PStore\EvidenceProcessor", "processingstatedirectory", Nothing)
        Dim R1Logs2 As String = "C:\Users\Public\Documents\AccessData\"
        Dim R1SSLogs As String = checkLog()
        Dim zipPath1 As String = "C:\Users\" + Environment.UserName + "\Documents\" + Now.ToString(format) + "R1Logs" + count.ToString + ".zip"
        Dim temp As String = "C:\temp\"
        Dim temp2 As String = temp + "Site_Server.log"




        Try
            Dim itemsToMove = (From i In lstSelected.Items).ToArray()

            For Each item In itemsToMove
                If item.ToString = "site_server" Then
                    For Each f In Directory.GetFiles(R1SSLogs, item + ".*", SearchOption.TopDirectoryOnly)
                        If File.Exists(f) Then
                            My.Computer.FileSystem.CopyFile(f, Path.Combine(temp, Path.GetFileName(f)), True)
                        End If
                    Next
                Else
                    For Each f In Directory.GetFiles(R1Logs, item + ".*", SearchOption.AllDirectories)
                        If File.Exists(f) Then
                            My.Computer.FileSystem.CopyFile(f, Path.Combine(temp, Path.GetFileName(f)), True)

                        End If
                    Next
                    For Each f In Directory.GetFiles(R1Logs2, item + ".*", SearchOption.AllDirectories)
                        If File.Exists(f) Then
                            My.Computer.FileSystem.CopyFile(f, Path.Combine(temp, Path.GetFileName(f)), True)
                        End If
                    Next
                    For Each f In Directory.GetFiles(ProcLogs, item + ".*", SearchOption.AllDirectories)
                        If File.Exists(f) Then
                            My.Computer.FileSystem.CopyFile(f, Path.Combine(temp, Path.GetFileName(f)), True)
                        End If
                    Next
                End If
            Next

            'My.Computer.FileSystem.CopyFile(R1SSLogs, temp2, True)

            ZipFile.CreateFromDirectory(temp, zipPath1)
            Directory.Delete(temp, True)
            MessageBox.Show("Archive Created at: " + zipPath1)
        Catch ex As Exception
            MessageBox.Show(ex.ToString)

        End Try
    End Function

    Private Sub lstOptions_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lstOptions.SelectedIndexChanged

    End Sub

    Private Sub btnMove_Click(sender As Object, e As EventArgs) Handles btnMove.Click


        Dim selectedItems = (From i In lstOptions.SelectedItems).ToArray()

        For Each item In selectedItems
            lstSelected.Items.Add(item)
            lstOptions.Items.Remove(item)
        Next

        lstSelected.EndUpdate()

    End Sub

    Private Sub btnRemove_Click(sender As Object, e As EventArgs) Handles btnRemove.Click
        Dim selectedItems = (From i In lstSelected.SelectedItems).ToArray()

        For Each item In selectedItems
            lstOptions.Items.Add(item)
            lstSelected.Items.Remove(item)
        Next

        lstSelected.EndUpdate()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        zip()

    End Sub

    Private Sub chkSelectAll_CheckedChanged(sender As Object, e As EventArgs) Handles chkSelectAll.CheckedChanged

        If chkSelectAll.Checked Then
            lstSelected.Items.AddRange(lstOptions.Items)
            lstOptions.Items.Clear()

        ElseIf Not chkSelectAll.checked Then
            lstOptions.Items.AddRange(lstSelected.Items)
            lstSelected.Items.Clear()
        End If


    End Sub
End Class

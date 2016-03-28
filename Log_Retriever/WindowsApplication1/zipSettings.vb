Imports System.IO
Imports System.IO.Compression
Imports System.Configuration


Public Class zipSettings
    Dim format As String = "M-d-yyyy-HH-mm-ss"


    Dim cAppConfig As Configuration = ConfigurationManager.OpenExeConfiguration(My.Application.Info.DirectoryPath + "\LogRetriever.exe")
    Dim asSettings As AppSettingsSection = cAppConfig.AppSettings

    Private Sub zipSettings_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Function checkLog(computer As String)

        Dim SS As String
        Dim SS2 As String
        Dim flag = False


        For Each c In "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray()
            If flag = False Then
                SS = "\\" + computer + "\" + c + "$\Program Files\Resolution1\SiteServer"
                SS2 = "\\" + computer + "\" + c + "$\Program Files\AccessData\SiteServer\"
                If (Not Directory.Exists(SS)) Then
                    flag = False
                Else
                    flag = True
                    Return "\\" + computer + "\" + c + "$\Program Files\Resolution1\SiteServer\"

                End If

                If (Not Directory.Exists(SS2)) Then
                    flag = False
                Else
                    flag = True
                    Return "\\" + computer + "\" + c + "$\Program Files\AccessData\SiteServer\"
                End If
            Else
                GoTo endoffunc
            End If
        Next

endoffunc:

    End Function

    Function getSSDirectory()
        Dim readValue = My.Computer.Registry.GetValue(
    "HKEY_LOCAL_MACHINE\SOFTWARE\AccessData\PStore\Services", "INSTALLDIR", Nothing)
        Return readValue.ToString + "\SiteServer\"
    End Function

    Private Function zip()
        Dim count As Integer = count + 1
        Dim R1Logs As String = "C:\Users\Public\Documents\Resolution1Logs\"
        Dim ProcLogs As String = My.Computer.Registry.GetValue(
    "HKEY_LOCAL_MACHINE\SOFTWARE\AccessData\PStore\EvidenceProcessor", "processingstatedirectory", Nothing)
        Dim R1Logs2 As String = "C:\Users\Public\Documents\AccessData\ResolutionOneLogs\"
        Dim R1SSLogs As String = getSSDirectory()
        Dim zipPath1 As String = "C:\Users\" + Environment.UserName + "\Documents\" + Now.ToString(format) + "R1Logs" + count.ToString + ".zip"
        Dim temp As String = "C:\temp\LR\"
        Dim temp2 As String = temp + "Site_Server.log"




        Try
            Dim itemsToMove = (From i In lstSelected.Items).ToArray()

            For Each item In itemsToMove
                If item.ToString = "site_server" Then
                    For Each f In Directory.GetFiles(R1SSLogs, item + ".*", SearchOption.TopDirectoryOnly)
                        If File.Exists(f) And CheckDates(f) = True Then
                            My.Computer.FileSystem.CopyFile(f, Path.Combine(temp, Path.GetFileName(f)), True)
                        End If
                    Next
                Else
                    For Each f In Directory.GetFiles(R1Logs, item + ".*", SearchOption.AllDirectories)
                        If File.Exists(f) And CheckDates(f) Then
                            My.Computer.FileSystem.CopyFile(f, Path.Combine(temp, Path.GetFileName(f)), True)

                        End If
                    Next
                    For Each f In Directory.GetFiles(R1Logs2, item + ".*", SearchOption.AllDirectories)
                        If File.Exists(f) And CheckDates(f) Then
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



    Function DistributedZip()
        'initilize paths and variables needed in the function
        Dim WCFServer As String = asSettings.Settings.Item("WCFServer").Value
        Dim ProcServer As String = asSettings.Settings.Item("ProcessingServer").Value
        Dim SSServer As String = asSettings.Settings.Item("SiteServer").Value
        Dim WMServer As String = asSettings.Settings.Item("CollectionWorkManager").Value
        Dim ProcLogs As String
        Dim R1SSLogs As String = ""
        Dim count As Integer = count + 1
        Dim R1WCFLogs As String = "\\" + WCFServer + "\C$\Users\Public\Documents\Resolution1Logs\"
        Dim R1Logs2 As String = "\\" + ProcServer + "\C$\Users\Public\Documents\AccessData\"
        Dim R1MapLogs As String = "C:\Users\Public\Documents\AccessData\"
        Dim R1WMLogs As String = ""




        'Get the location of Processing Logs
        If ProcServer = "localhost" Then
            ProcLogs = My.Computer.Registry.GetValue(
    "HKEY_LOCAL_MACHINE\SOFTWARE\AccessData\PStore\EvidenceProcessor", "processingstatedirectory", Nothing)
        Else
            ProcLogs = asSettings.Settings.Item("ProcessingLogPath").Value
            ProcLogs = "\\" + ProcServer + "\" + ProcLogs.Replace(":", "$")

        End If

        'Get the location of the Site Server Logs
        If SSServer = "localhost" Then
            R1SSLogs = getSSDirectory()
        Else
            R1SSLogs = checkLog(SSServer)
        End If

        'set the zip path as the current users document folder
        Dim zipPath1 As String = "C:\Users\" + Environment.UserName + "\Documents\" + Now.ToString(format) + "R1Logs" + count.ToString + ".zip"

        'declare temporary storage for transferring logs
        Dim temp As String = "C:\temp\LR\"
        Directory.CreateDirectory(temp)
        Dim temp2 As String = temp + "Site_Server.log"


        Try
            Dim itemsToMove = (From i In lstSelected.Items).ToArray()

            For Each item In itemsToMove
                If item.ToString = "site_server" Then
                    For Each f In Directory.GetFiles(R1SSLogs, item + ".*", SearchOption.TopDirectoryOnly)
                        If File.Exists(f) And CheckDates(f) = True Then
                            My.Computer.FileSystem.CopyFile(f, Path.Combine(temp, Path.GetFileName(f)), True)
                        End If
                    Next
                Else
                    For Each f In Directory.GetFiles(R1WCFLogs, item + ".*", SearchOption.AllDirectories)
                        If File.Exists(f) And CheckDates(f) Then

                            My.Computer.FileSystem.CopyFile(f, Path.Combine(temp, Path.GetFileName(f)), True)

                        End If
                    Next
                    For Each f In Directory.GetFiles(R1Logs2, item + ".*", SearchOption.AllDirectories)
                        If File.Exists(f) And CheckDates(f) Then
                            My.Computer.FileSystem.CopyFile(f, Path.Combine(temp, Path.GetFileName(f)), True)
                        End If
                    Next
                    For Each f In Directory.GetFiles(ProcLogs, item + ".*", SearchOption.AllDirectories)
                        If File.Exists(f) Then
                            My.Computer.FileSystem.CopyFile(f, Path.Combine(temp, Path.GetFileName(f)), True)
                        End If
                    Next
                    For Each f In Directory.GetFiles(R1MapLogs, item + ".*", SearchOption.AllDirectories)
                        If File.Exists(f) Then
                            My.Computer.FileSystem.CopyFile(f, Path.Combine(temp, Path.GetFileName(f)), True)
                        End If
                    Next

                    If WMServer = "localhost" Then

                    Else
                        R1WMLogs = "\\" + WMServer + "\C$\Users\Public\Documents\Resolution1Logs\"
                        For Each f In Directory.GetFiles(R1WMLogs, item + ".*", SearchOption.AllDirectories)
                            If File.Exists(f) And Not WMServer = "localhost" Then
                                My.Computer.FileSystem.CopyFile(f, Path.Combine(temp, Path.GetFileName(f)), True)
                            End If
                        Next
                    End If

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
        Try
            If File.Exists(My.Application.Info.DirectoryPath + "\LogRetriever.exe.config") And asSettings.Settings.Item("DistributedInstall").Value = True Then
                DistributedZip()
            Else
                zip()
            End If
        Catch ex As NullReferenceException
            MessageBox.Show("Error Locating Log files, If this is distributed install please edit the Configuration File.")
        Catch ex As Exception
            MessageBox.Show(ex.ToString)
        End Try

    End Sub

    Private Sub chkSelectAll_CheckedChanged(sender As Object, e As EventArgs) Handles chkSelectAll.CheckedChanged

        If chkSelectAll.Checked Then
            lstSelected.Items.AddRange(lstOptions.Items)
            lstOptions.Items.Clear()

        ElseIf Not chkSelectAll.Checked Then
            lstOptions.Items.AddRange(lstSelected.Items)
            lstSelected.Items.Clear()
        End If


    End Sub

    Function CheckDates(f As String)
        If txtDays.Text.Equals("") Then
            Return True
            Exit Function
        End If

        Dim t As String = "-" + txtDays.Text
        Dim days = Convert.ToInt32(t)
        Dim DateBack = DateAndTime.Now.AddDays(days)
        Dim infoReader As FileInfo
        infoReader = My.Computer.FileSystem.GetFileInfo(f)
        If infoReader.LastWriteTime > DateBack Then
            Return True
        Else
            Return False
        End If
    End Function


End Class

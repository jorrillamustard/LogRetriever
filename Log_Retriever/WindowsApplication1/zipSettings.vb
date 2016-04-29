Imports System.IO
Imports System.IO.Compression
Imports System.Configuration
Imports System.Reflection
Imports Outlook = Microsoft.Office.Interop.Outlook
'Main application class
Public Class zipSettings
    Dim format As String = "M-d-yyyy-HH-mm-ss"
    Dim globalZip As String
    Dim apppath As String = "ProgramData\Fidelis\Endpoint\"
    Dim SSapppath1 As String = "$\Program Files\Fidelis\Endpoint\SiteServer"
    Dim SSapppath2 As String = "$\Program Files\Fidelis\Endpoint\SiteServer"


    Dim cAppConfig As Configuration = ConfigurationManager.OpenExeConfiguration(My.Application.Info.DirectoryPath + "\LogRetriever.exe")
    Dim asSettings As AppSettingsSection = cAppConfig.AppSettings



    'Checks for the location of SS logs
    Private Function checkLog(computer As String)

        Dim SS As String
        Dim SS2 As String
        Dim flag = False

        'MessageBox.Show(computer)
        For Each c In "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray()
            If flag = False Then
                SS = "\\" + computer + "\" + c + SSapppath1
                SS2 = "\\" + computer + "\" + c + SSapppath2
                '  MessageBox.Show(SS + " " + SS2)
                If (Not Directory.Exists(SS)) Then
                    flag = False

                Else
                    flag = True
                    'MessageBox.Show(SS)
                    Return "\\" + computer + "\" + c + SSapppath1

                End If

                If (Not Directory.Exists(SS2)) Then
                    flag = False
                Else
                    flag = True
                    Return "\\" + computer + "\" + c + SSapppath2
                End If
            Else
                GoTo endoffunc
            End If
        Next

endoffunc:

    End Function

    'Reads Registry for install location of SiteServer - Localhost
    Function getSSDirectory()
        Dim readValue = My.Computer.Registry.GetValue(
    "HKEY_LOCAL_MACHINE\SOFTWARE\AccessData\PStore\Services", "INSTALLDIR", Nothing)

        If (Not Directory.Exists(readValue.ToString + "\SiteServer\")) Then
            Return checkLog("localhost")
        Else
            Return readValue.ToString + "\SiteServer\"
        End If


    End Function

    'Gets logs from Child Site Servers
    Function ChildSSLogs()
        If asSettings.Settings.Item("ChildSiteServers").Value.Equals("") Then
        Else

            Dim CSS As String() = Nothing
            CSS = asSettings.Settings.Item("ChildSiteServers").Value.Split(",")
            Dim s As String
            Dim c As Integer = 0
            'MessageBox.Show(CSS(0))
            'For Each s In CSS
            'CSS(c) = s
            'c = c + 1
            'Next

            c = 0
            Dim logPath As String() = Nothing

            Dim temp As String = "C:\temp\LR\"

            For Each s In CSS

                For Each f In Directory.GetFiles(checkLog(CSS(c)), "site_server.*", SearchOption.TopDirectoryOnly)
                    'MessageBox.Show(f)
                    If File.Exists(f) And CheckDates(f) = True Then
                        ' MessageBox.Show(f)
                        My.Computer.FileSystem.CopyFile(f, Path.Combine(temp, CSS(c)) + "." + Path.GetFileName(f), True)
                    End If
                Next
                'c = c + 1
            Next
        End If
    End Function

    'gets logs from all WorkManagers
    Function WMLogs(t As String)
        If asSettings.Settings.Item("WorkManager").Value.Equals("") Then
        Else

            Dim WM As String() = Nothing
            WM = asSettings.Settings.Item("WorkManager").Value.Split(",")


            Dim temp As String = "C:\temp\LR\"
            Dim R1WMLogs As String = Nothing

            For Each w In WM

                R1WMLogs = "\\" + w + "\C$\" + apppath
                'MessageBox.Show(R1WMLogs)
                For Each f In Directory.GetFiles(R1WMLogs, t + ".*", SearchOption.TopDirectoryOnly)

                    If File.Exists(f) And CheckDates(f) = True Then
                        'MessageBox.Show(WM(c))
                        My.Computer.FileSystem.CopyFile(f, Path.Combine(temp, w) + "." + Path.GetFileName(f), True)
                    End If
                Next

            Next
        End If
    End Function

    'Gathers and Zips the logs in the temp folder - Localhost
    Private Function zip()
        Dim count As Integer = count + 1
        Dim R1Logs As String = "C:\" + apppath
        Dim ProcLogs As String = My.Computer.Registry.GetValue(
    "HKEY_LOCAL_MACHINE\SOFTWARE\AccessData\PStore\EvidenceProcessor", "processingstatedirectory", Nothing)
        Dim R1Logs2 As String = "C:\" + apppath
        Dim R1SSLogs As String = getSSDirectory()
        Dim zipPath1 As String = "C:\Users\" + Environment.UserName + "\Documents\" + Now.ToString(format) + "FELogs" + count.ToString + ".zip"
        Dim temp As String = "C:\Users\" + Environment.UserName + "\Documents\FETemp\"
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
            btnEmail.Visible = True
        Catch ex As Exception
            MessageBox.Show(ex.ToString)

        End Try
    End Function

    'Gathers and zips the logs in the temp folder for distributed installs
    Function DistributedZip()
        'initilize paths and variables needed in the function
        Dim MAPServer As String = asSettings.Settings.Item("MAPServer").Value
        Dim WCFServer As String = asSettings.Settings.Item("WCFServer").Value
        Dim ProcServer As String = asSettings.Settings.Item("ProcessingServer").Value
        Dim SSServer As String = asSettings.Settings.Item("SiteServer").Value
        Dim WMServer As String = asSettings.Settings.Item("WorkManager").Value
        Dim TBServer As String = asSettings.Settings.Item("TBServer").Value
        Dim ProcLogs As String
        Dim R1SSLogs As String = ""
        Dim count As Integer = count + 1
        Dim R1WCFLogs As String = "\\" + WCFServer + "\C$\" + apppath
        Dim R1Logs2 As String = "\\" + ProcServer + "\C$\" + apppath
        Dim R1MapLogs As String = ""
        Dim R1WMLogs As String = ""


        Dim zipPath1 As String = "C:\Users\" + Environment.UserName + "\Documents\" + Now.ToString(format) + "FELogs.zip"

        'Get the location of Processing Logs
        If ProcServer = "localhost" Then
            ProcLogs = My.Computer.Registry.GetValue(
    "HKEY_LOCAL_MACHINE\SOFTWARE\AccessData\PStore\EvidenceProcessor", "processingstatedirectory", Nothing)
        Else
            If chkPreSix.Checked = True Then
                ProcLogs = asSettings.Settings.Item("ProcessingLogPath").Value
                ProcLogs = "\\" + ProcServer + "\" + ProcLogs.Replace(":", "$")
            Else
                ProcLogs = "\\" + ProcServer + "\C$\" + apppath
            End If

        End If


        'Get the location of the Site Server Logs
        If SSServer = "localhost" Then
            R1SSLogs = getSSDirectory()
        Else
            R1SSLogs = checkLog(SSServer)
        End If

        'set the zip path as the current users document folder

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

                ElseIf item.ToString = "WorkManager" Then

                    WMLogs(item)
                    ' R1WMLogs = "\\" + WMServer + "\C$\" + apppath
                    ' For Each f In Directory.GetFiles(R1WMLogs, item + ".*", SearchOption.AllDirectories)
                    'If File.Exists(f) And Not WMServer = "localhost" Then
                    'My.Computer.FileSystem.CopyFile(f, Path.Combine(temp, Path.GetFileName(f)), True)
                    'End If
                    'Next


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



                    If MAPServer = "localhost" Then

                    Else
                        R1MapLogs = "\\" + MAPServer + "\C$\" + apppath
                        For Each f In Directory.GetFiles(R1MapLogs, item + ".*", SearchOption.AllDirectories)
                            If File.Exists(f) And Not MAPServer = "localhost" Then
                                My.Computer.FileSystem.CopyFile(f, Path.Combine(temp, Path.GetFileName(f)), True)
                            End If
                        Next
                    End If

                    If TBServer = "localhost" Then

                    Else
                        Dim TBLogs = "\\" + TBServer + "\C$\" + apppath
                        For Each f In Directory.GetFiles(TBLogs, item + ".*", SearchOption.AllDirectories)
                            If File.Exists(f) And Not TBServer = "localhost" Then
                                My.Computer.FileSystem.CopyFile(f, Path.Combine(temp, Path.GetFileName(f)), True)
                            End If
                        Next
                    End If
                End If

                ChildSSLogs()
            Next

            'My.Computer.FileSystem.CopyFile(R1SSLogs, temp2, True)
            globalZip = zipPath1
            ZipFile.CreateFromDirectory(temp, zipPath1)
            Directory.Delete(temp, True)
            MessageBox.Show("Archive Created at: " + zipPath1)
            btnEmail.Visible = True
        Catch ex As Exception
            MessageBox.Show(ex.ToString)

        End Try
    End Function

    'Button Function to move logs from available to archive list
    Private Sub btnMove_Click(sender As Object, e As EventArgs) Handles btnMove.Click


        Dim selectedItems = (From i In lstOptions.SelectedItems).ToArray()

        For Each item In selectedItems
            lstSelected.Items.Add(item)
            lstOptions.Items.Remove(item)
        Next

        lstSelected.EndUpdate()

    End Sub

    'Button Function to move logs from archive list back to available
    Private Sub btnRemove_Click(sender As Object, e As EventArgs) Handles btnRemove.Click
        Dim selectedItems = (From i In lstSelected.SelectedItems).ToArray()

        For Each item In selectedItems
            lstOptions.Items.Add(item)
            lstSelected.Items.Remove(item)
        Next

        lstSelected.EndUpdate()
    End Sub

    'button function to start the gather and zip process - Checks for config and if its a distributed install
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Try
            If File.Exists(My.Application.Info.DirectoryPath + "\LogRetriever.exe.config") Then
                If asSettings.Settings.Item("DistributedInstall").Value = True Then
                    DistributedZip()
                End If
            Else
                zip()
            End If
        Catch ex As NullReferenceException
            MessageBox.Show("Error Locating Log files, If this is distributed install please edit the Configuration File.")
        Catch ex As Exception
            MessageBox.Show(ex.ToString)
        End Try

    End Sub

    'Check box function to move all logs between lists
    Private Sub chkSelectAll_CheckedChanged(sender As Object, e As EventArgs) Handles chkSelectAll.CheckedChanged

        If chkSelectAll.Checked Then
            lstSelected.Items.AddRange(lstOptions.Items)
            lstOptions.Items.Clear()

        ElseIf Not chkSelectAll.Checked Then
            lstOptions.Items.AddRange(lstSelected.Items)
            lstSelected.Items.Clear()
        End If


    End Sub

    'Checks the dates of the logs per the defined amount on in txtdays textbox
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


    'Email
    Private Sub btnEmail_Click(sender As Object, e As EventArgs) Handles btnEmail.Click


        Dim officetype As Type = Type.GetTypeFromProgID("Outlook.Application")
        Try
            If officetype Is Nothing Then

                Dim address As String = "support@fidelissecurity.com"
                Dim subject As String = "Fidelis Endpoint Application Logs"
                Dim body As String = "Please find the attached logs for current issues"
                Dim attach As String = globalZip
                Dim mailto As String

                mailto = String.Format("mailto:{0}?Subject={1}&Body={2}&Attach={3}", address, subject, body, attach)

                System.Diagnostics.Process.Start(mailto)
            Else

                ' Create an Outlook application.
                Dim OutlookMessage As Outlook.MailItem
                Dim AppOutlook As Outlook.Application = New Outlook.Application()
                Try
                    OutlookMessage = AppOutlook.CreateItem(Outlook.OlItemType.olMailItem)
                    Dim Recipents As Outlook.Recipients = OutlookMessage.Recipients
                    Recipents.Add("support@fidelissecurity.com")
                    OutlookMessage.Subject = "Fidelis Endpoint Application Logs"
                    OutlookMessage.Body = "Please find the attached logs for current issues"
                    Dim myAttachments = OutlookMessage.Attachments
                    myAttachments.Add(globalZip)
                    OutlookMessage.BodyFormat = Outlook.OlBodyFormat.olFormatHTML
                    OutlookMessage.Display()
                Catch ex As Exception
                    MessageBox.Show("Mail could Not be sent") 'if you dont want this message, simply delete this line 
                Finally
                    OutlookMessage = Nothing
                    AppOutlook = Nothing
                End Try

            End If
        Catch ex As Exception
            MessageBox.Show("Unable to open email client, please manually send logs.")
        End Try
    End Sub

    Private Sub chkPreSix_CheckedChanged(sender As Object, e As EventArgs) Handles chkPreSix.CheckedChanged
        If chkPreSix.Checked = True Then
            Dim itemsToMove = (From i In lstOptions.Items).ToArray()
            For Each item In itemsToMove
                If item.ToString = "felt" Then
                    lstOptions.Items.Remove(item)
                ElseIf item.ToString = "ResultService" Then
                    lstOptions.Items.Remove(item)
                End If
            Next
            apppath = "Users\Public\Documents\Resolution1Logs\"
            SSapppath1 = "$\Program Files\Resolution1\SiteServer"
            SSapppath2 = "$\Program Files\AccessData\SiteServer\"


        ElseIf Not chkPreSix.Checked Then
            lstOptions.Items.Add("felt")
            lstOptions.Items.Add("ResultService")
            apppath = "ProgramData\Fidelis\Endpoint\"
            SSapppath1 = "$\Program Files\Fidelis\Endpoint\SiteServer"
            SSapppath2 = "$\Program Files\Fidelis\Endpoint\SiteServer"
            lstOptions.Sorted = True
        End If
    End Sub
End Class
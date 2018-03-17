Imports MySql.Data.MySqlClient
Imports System.Threading
Public Class LoginWindow
    Dim MysqlConn As MySqlConnection
    Dim COMMAND As MySqlCommand
    Dim READER As MySqlDataReader

    Dim UserData As UserData
    Dim Diaries As ArrayList
    Dim Contacts As ArrayList
    Dim AccountRecords As ArrayList

    Private Sub Button_Click(sender As Object, e As RoutedEventArgs)
        LoginPanel.Visibility = Visibility.Hidden
        LoadingPanel.Visibility = Visibility.Visible
        Dim thread As New Thread(
         Sub()
             Thread.Sleep(3000)
             Prepare_For_Login()
         End Sub
        )
        thread.Start()



    End Sub

    Private Function Prepare_For_Login()
        Me.Dispatcher.Invoke(Function()
                                 Dim userMail = LoginUserNameTextBox.Text
                                 If Authentic(LoginUserNameTextBox.Text, LoginPasswordBox.Password) Then
                                     Dim NewWindow As Object = New DashBoardWindow

                                     If LoadData(userMail) Then
                                         '加载成功后改换句柄
                                         NewWindow.UserData = UserData
                                         NewWindow.Diaries = Diaries
                                         NewWindow.Contacts = Contacts
                                         NewWindow.AccountRecords = AccountRecords
                                         NewWindow.Show()
                                         Me.Close()
                                     Else
                                         MessageBox.Show("系统错误 与数据库通信失败！")
                                         Application.Current.Shutdown()
                                     End If

                                 Else
                                     LoginPanel.Visibility = Visibility.Visible
                                     LoadingPanel.Visibility = Visibility.Hidden
                                     MessageBox.Show("用户名或者密码不正确")
                                 End If
                                 Return 0
                             End Function
        )
    End Function

    'Authenic Function
    Private Function Authentic(ByVal UserMail As String, ByVal PassWord As String) As Boolean
        MysqlConn = New MySqlConnection
        MysqlConn.ConnectionString =
            "server=localhost;uid=root;password=password;database=vbdashboard"
        MysqlConn.Open()
        Try
            Dim Query As String = "select * from vbdashboard.user where mail = '" + UserMail + "' and password = '" + PassWord + "'"
            COMMAND = New MySqlCommand(Query, MysqlConn)
            READER = COMMAND.ExecuteReader
            Dim count As Integer = 0
            While READER.Read
                count = count + 1
            End While
            If (count = 1) Then
                MysqlConn.Close()
                Return True
            End If
            MysqlConn.Close()
        Catch ex As MySqlException
            MessageBox.Show(ex.Message)
        Finally
            MysqlConn.Dispose()
        End Try
        Return False
    End Function

    'Mysql加载数据
    Private Function LoadData(ByVal Mail As String) As Boolean
        MysqlConn = New MySqlConnection
        MysqlConn.ConnectionString =
            "server=localhost;uid=root;password=password;database=vbdashboard"
        MysqlConn.Open()

        Try
            Dim Query As String = "select * from user where mail = '" + Mail + "'"
            COMMAND = New MySqlCommand(Query, MysqlConn)
            READER = COMMAND.ExecuteReader
            While READER.Read
                UserData.Name = READER.GetString("username")
                UserData.Gender = READER.GetString("gender")
                UserData.Mail = READER.GetString("mail")
                UserData.Age = READER.GetDecimal("age")
                UserData.Birthday = READER.GetString("birthday")
                UserData.id = READER.GetDecimal("id")
            End While
            READER.Close()

            Query = "select * from diary where belong = " & UserData.Id
            COMMAND = New MySqlCommand(Query, MysqlConn)
            READER = COMMAND.ExecuteReader
            Diaries = New ArrayList
            Dim Diary As Diary
            While READER.Read
                Diary = New Diary
                Diary.Title = READER.GetString("title")
                Diary.Content = READER.GetString("content")
                Diary.Format = READER.GetString("format")
                Diary.DateEdit = READER.GetString("date")
                Diaries.Add(Diary)
            End While
            READER.Close()

            Query = "select * from contact where belong = " & UserData.Id
            COMMAND = New MySqlCommand(Query, MysqlConn)
            READER = COMMAND.ExecuteReader
            Contacts = New ArrayList
            Dim Contact As Contact
            While READER.Read
                Contact = New Contact
                Contact.Name = READER.GetString("name")
                Contact.Mail = READER.GetString("mail")
                Contact.Phone = READER.GetString("phone")
                Contacts.Add(Contact)
            End While
            READER.Close()

            Query = "select * from account where belong = " & UserData.Id
            COMMAND = New MySqlCommand(Query, MysqlConn)
            READER = COMMAND.ExecuteReader
            AccountRecords = New ArrayList
            Dim Record As AccountRecord
            While READER.Read
                Record = New AccountRecord
                Record.Id = READER.GetDecimal("id")
                Record.Content = READER.GetString("content")
                Record.Cost = READER.GetString("cost")
                Record.DateCreated = READER.GetString("date")
                AccountRecords.Add(Record)
            End While
            READER.Close()

            MysqlConn.Close()
        Catch ex As MySqlException
            MessageBox.Show(ex.Message)
            Return False
        Finally
            MysqlConn.Dispose()
        End Try
        Return True
    End Function

End Class


Public Structure UserData
    Dim Id As Integer
    Dim Name As String
    Dim Gender As String
    Dim Mail As String
    Dim Age As Integer
    Dim Birthday As String
End Structure

Public Structure Diary
    Dim Title As String
    Dim Content As String
    Dim DateEdit As String
    Dim Format As String
End Structure

Public Structure Contact
    Dim Name As String
    Dim Mail As String
    Dim Phone As String
End Structure

Public Structure AccountRecord
    Dim Id As Integer
    Dim Content As String
    Dim Cost As Double
    Dim DateCreated As String
End Structure
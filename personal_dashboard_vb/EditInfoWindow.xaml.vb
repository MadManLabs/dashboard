Imports System.Threading
Imports MySql.Data.MySqlClient
Public Class EditInfoWindow
    Public Data As UserData

    Private Sub Button_Click(sender As Object, e As RoutedEventArgs)
        '基本信息修改
        Dim UserName As String = UserNameTextBox.Text
        Dim Gender As String = GenderTextBox.Text
        Dim Age As Integer = Integer.Parse(AgeTextBox.Text)
        Dim Mail As String = MailTextBox.Text
        Update_User_Info(UserName, Gender, Age, Mail)
    End Sub

    Private Sub Update_User_Info(UserName As String, Gender As String, Age As Integer, Mail As String)
        Dim thread As New Thread(
        Sub()
            Dim MysqlConn As MySqlConnection
            Dim COMMAND As MySqlCommand
            MysqlConn = New MySqlConnection
            MysqlConn.ConnectionString =
       "server=localhost;uid=root;password=password;database=vbdashboard"
            MysqlConn.Open()
            Try
                Dim Query As String = "update vbdashboard.user set username = '" & UserName & "', age = " & Age.ToString & ",Gender = '" & Gender & "', mail = '" & Mail & "' where id = " & Data.Id.ToString
                MsgBox(Query)
                COMMAND = New MySqlCommand(Query, MysqlConn)
                COMMAND.ExecuteNonQuery()
                MysqlConn.Close()
            Catch ex As MySqlException
                MessageBox.Show(ex.Message)
            Finally
                MysqlConn.Dispose()
            End Try
            MsgBox("修改成功")
        End Sub
            ）
        thread.Start()
    End Sub

    Private Sub Button_Click_1(sender As Object, e As RoutedEventArgs)
        '密码修改
        Dim Pass1 As String = NewPassword1.Password
        Dim Pass2 As String = NewPassword2.Password
        If Pass1 = Pass2 Then
            Update_Password(Pass1)
        Else
            MessageLabel2.Content = "两次密码不一致！"
        End If
    End Sub

    Private Sub Update_Password(Psv As String)
        Dim thread As New Thread(
        Sub()
            Dim MysqlConn As MySqlConnection
            Dim COMMAND As MySqlCommand
            MysqlConn = New MySqlConnection
            MysqlConn.ConnectionString =
       "server=localhost;uid=root;password=password;database=vbdashboard"
            MysqlConn.Open()
            Try
                Dim Query As String = "update vbdashboard.user set password = '" + Psv + "' where id = " + Data.Id.ToString
                'MsgBox(Query)
                COMMAND = New MySqlCommand(Query, MysqlConn)
                COMMAND.ExecuteNonQuery()
                MysqlConn.Close()
            Catch ex As MySqlException
                MessageBox.Show(ex.Message)
            Finally
                MysqlConn.Dispose()
            End Try
            MsgBox("修改成功")
        End Sub
            ）
        thread.Start()
    End Sub

    Private Sub NewPassword2_PasswordChanged(sender As Object, e As RoutedEventArgs)
        MessageLabel2.Content = ""
    End Sub

    Private Sub NewPassword1_PasswordChanged(sender As Object, e As RoutedEventArgs)
        MessageLabel2.Content = ""
    End Sub
End Class

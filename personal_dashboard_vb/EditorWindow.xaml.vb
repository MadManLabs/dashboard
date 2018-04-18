Imports com.github.rjeschke.txtmark
Imports System.Threading
Imports MySql.Data.MySqlClient

Class EditorWindow

    Public Diary As Diary
    Public MotherWindow As DiaryWindow
    Dim headData As String = "<html><head><link href=""http://jasonm23.github.io/markdown-css-themes/markdown.css""></head><body>"
    Dim tailData As String = "</body></html>"
    Private Sub TextBox_TextChanged(sender As Object, e As TextChangedEventArgs)
        Dim markdown As String = InputTextBox.Text
        ReLoad_Lines()
        Dim thread As New Thread(
                                 Sub()
                                     Refresh(markdown)
                                 End Sub
                                )
        thread.Start()
    End Sub

    Private Sub Refresh(ByVal markdown As String)
        Me.Dispatcher.Invoke(Function()
                                 Dim htmlData = Ozone.Markdown.Parser.ParseMarkdown(markdown)
                                 htmlData = headData + htmlData + tailData
                                 TranslatedTextBox.Navigate("about:blank")
                                 TranslatedTextBox.Document.Write(htmlData)
                                 TranslatedTextBox.Refresh()
                                 Return 0
                             End Function
            )
    End Sub

    Private Sub Window_Loaded(sender As Object, e As RoutedEventArgs)
        InputTextBox.Text = Diary.Content
        TitleTextBlock.Text = Diary.Title
        If (Not Diary.Format.Equals("Markdown")) Then
            TranslatedTextBox.Visibility = Visibility.Hidden
        End If
        ReLoad_Lines()

    End Sub

    Private Sub ReLoad_Lines()
        lineNumbers.Items.Clear()
        Dim lines As Integer = InputTextBox.LineCount + 1
        For i As Integer = 1 To lines
            '<ListBoxItem HorizontalAlignment = "Center" Foreground="White" FontSize="15" Padding="1">1</ListBoxItem>
            Dim item As New ListBoxItem
            item.HorizontalContentAlignment = HorizontalAlignment.Center
            item.Foreground = New BrushConverter().ConvertFromString("White")
            item.FontSize = 15
            item.Padding = New ThicknessConverter().ConvertFromString("0.85")
            item.Content = i
            lineNumbers.Items.Add(item)
        Next

    End Sub

    Private Sub Button_Click(sender As Object, e As RoutedEventArgs)
        '单击保存//前提：上一级面板已经生成空表 
        '仅仅作更改 ater update
        Diary.Content = InputTextBox.Text
        Dim MysqlConn As MySqlConnection
        Dim COMMAND As MySqlCommand
        MysqlConn = New MySqlConnection
        MysqlConn.ConnectionString =
            "server=localhost;uid=root;password=password;database=vbdashboard"
        MysqlConn.Open()
        Try
            Dim Query As String = "update vbdashboard.diary set content = '" + Diary.Content + "' , date = '" + Date.Now.ToString + "' where id = " + Diary.Id.ToString
            COMMAND = New MySqlCommand(Query, MysqlConn)
            COMMAND.ExecuteNonQuery()
            MysqlConn.Close()
            MotherWindow.Update_Diaries_Data()
            MessageBox.Show("保存成功")
        Catch ex As MySqlException
            MessageBox.Show(ex.Message)
        Finally
            MysqlConn.Dispose()
        End Try
    End Sub
End Class

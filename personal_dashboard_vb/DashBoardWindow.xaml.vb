Public Class DashBoardWindow

    Public PeopleData As String

    Private Sub Window_Loaded(sender As Object, e As RoutedEventArgs)
        NewsBrowser.Navigate("https://en.m.wikipedia.org/wiki/Main_Page")
        'MsgBox(PeopleData)
    End Sub

    Private Sub Button_Click(sender As Object, e As RoutedEventArgs)
        '打开编辑器
        Dim DiaryWindow As Object = New EditorWindow
        DiaryWindow.show()
    End Sub
End Class

Public Class DashBoardWindow

    Public UserData As UserData
    Public Diaries As ArrayList
    Public Contacts As ArrayList
    Public AccountRecords As ArrayList


    Private Sub Window_Loaded(sender As Object, e As RoutedEventArgs)
        NewsBrowser.Navigate("https://en.m.wikipedia.org/wiki/Main_Page")
        ' MsgBox(UserData.Name)
    End Sub

    Private Sub Button_Click(sender As Object, e As RoutedEventArgs)
        '打开编辑器
        Dim DiaryWindow As Object = New EditorWindow
        DiaryWindow.show()
    End Sub

    Private Sub Button_Click_1(sender As Object, e As RoutedEventArgs)
        '打开钱包管理
        Dim NewWindow As New AccountWindow
        NewWindow.UserData = UserData
        NewWindow.AccountRecords = AccountRecords
        NewWindow.Show()
    End Sub

    Private Sub Button_Click_2(sender As Object, e As RoutedEventArgs)
        '打开通讯录管理
        Dim NewWindow As New ContactsWindow
        NewWindow.UserData = UserData
        NewWindow.Contacts = Contacts
        NewWindow.Show()
    End Sub
End Class

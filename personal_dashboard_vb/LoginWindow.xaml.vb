Imports System.Threading
Public Class LoginWindow
    Private Sub Button_Click(sender As Object, e As RoutedEventArgs)
        LoginPanel.Visibility = Visibility.Hidden
        LoadingPanel.Visibility = Visibility.Visible
        Dim NewWindow As Object = New DashBoardWindow

        Dim UserName As String = "AoDamiao"
        NewWindow.PeopleData = UserName
        'Auth
        'LoadAllData


        Me.Close()
        NewWindow.Show()
    End Sub
End Class

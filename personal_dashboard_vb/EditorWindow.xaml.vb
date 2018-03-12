Imports com.github.rjeschke.txtmark
Class EditorWindow

    Private Sub TextBox_TextChanged(sender As Object, e As TextChangedEventArgs)
        Dim markdown As String = InputTextBox.Text
        Dim htmlData = Ozone.Markdown.Parser.ParseMarkdown(markdown)

        TranslatedTextBox.Navigate("about:blank")
        TranslatedTextBox.Document.Write(htmlData)
        TranslatedTextBox.Refresh()

    End Sub
End Class

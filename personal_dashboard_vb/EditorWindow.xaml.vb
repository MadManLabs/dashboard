Imports com.github.rjeschke.txtmark
Imports System.Threading
Imports MySql.Data.MySqlClient

Class EditorWindow

    Public Diary As Diary
    Public MotherWindow As DiaryWindow
    Dim headData As String = "<html>
<head>
    <style>
    html { font-size: 100%; overflow-y: scroll; -webkit-text-size-adjust: 100%; -ms-text-size-adjust: 100%; }

body{
color:#444;
font-family:Georgia, Palatino, 'Palatino Linotype', Times, 'Times New Roman', serif;
font-size:12px;
line-height:1.5em;
padding:1em;
margin:auto;
max-width:42em;
background:#fefefe;
}

a{ color: #0645ad; text-decoration:none;}
a:visited{ color: #0b0080; }
a:hover{ color: #06e; }
a:active{ color:#faa700; }
a:focus{ outline: thin dotted; }
a:hover, a:active{ outline: 0; }

::-moz-selection{background:rgba(255,255,0,0.3);color:#000}
::selection{background:rgba(255,255,0,0.3);color:#000}

a::-moz-selection{background:rgba(255,255,0,0.3);color:#0645ad}
a::selection{background:rgba(255,255,0,0.3);color:#0645ad}

p{
margin:1em 0;
}

img{
max-width:100%;
}

h1,h2,h3,h4,h5,h6{
font-weight:normal;
color:#111;
line-height:1em;
}
h4,h5,h6{ font-weight: bold; }
h1{ font-size:2.5em; }
h2{ font-size:2em; }
h3{ font-size:1.5em; }
h4{ font-size:1.2em; }
h5{ font-size:1em; }
h6{ font-size:0.9em; }

blockquote{
color:#666666;
margin:0;
padding-left: 3em;
border-left: 0.5em #EEE solid;
}
hr { display: block; height: 2px; border: 0; border-top: 1px solid #aaa;border-bottom: 1px solid #eee; margin: 1em 0; padding: 0; }
pre, code, kbd, samp { color: #000; font-family: monospace, monospace; _font-family: 'courier new', monospace; font-size: 0.98em; }
pre { white-space: pre; white-space: pre-wrap; word-wrap: break-word; }

b, strong { font-weight: bold; }

dfn { font-style: italic; }

ins { background: #ff9; color: #000; text-decoration: none; }

mark { background: #ff0; color: #000; font-style: italic; font-weight: bold; }

sub, sup { font-size: 75%; line-height: 0; position: relative; vertical-align: baseline; }
sup { top: -0.5em; }
sub { bottom: -0.25em; }

ul, ol { margin: 1em 0; padding: 0 0 0 2em; }
li p:last-child { margin:0 }
dd { margin: 0 0 0 2em; }

img { border: 0; -ms-interpolation-mode: bicubic; vertical-align: middle; }

table {
border-collapse: collapse;
border-spacing: 0;
width: 100%;
}
th { border-bottom: 1px solid black; }
td { vertical-align: top; }

@media only screen and (min-width: 480px) {
body{font-size:14px;}
}

@media only screen and (min-width: 768px) {
body{font-size:16px;}
}

@media print {
  * { background: transparent !important; color: black !important; filter:none !important; -ms-filter: none !important; }
  body{font-size:12pt; max-width:100%;}
  a, a:visited { text-decoration: underline; }
  hr { height: 1px; border:0; border-bottom:1px solid black; }
  a[href]:after { content: '('attr(href) ')'; }
  abbr[title]:after { content: '(' attr(title) ')'; }
  .ir a:after, a[href^='javascript:']:after, a[href^='#']:after { content: ""; }
  pre, blockquote { border: 1px solid #999; padding-right: 1em; page-break-inside: avoid; }
  tr, img { page-break-inside: avoid; }
  img { max-width: 100% !important; }
  @page :left { margin: 15mm 20mm 15mm 10mm; }
  @page :right { margin: 15mm 10mm 15mm 20mm; }
  p, h2, h3 { orphans: 3; widows: 3; }
  h2, h3 { page-break-after: avoid; }
}
    </style>
</head>
<body>"
    Dim tailData As String = "</body></html>"

    Private Sub TextBox_TextChanged(sender As Object, e As TextChangedEventArgs)
        Dim markdown As String = InputTextBox.Text
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

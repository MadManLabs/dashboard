Imports MySql.Data.MySqlClient
Public Class ContactsWindow

    Dim MysqlConn As MySqlConnection
    Dim COMMAND As MySqlCommand
    Dim READER As MySqlDataReader

    Function Save(ByVal x)
        MysqlConn = New MySqlConnection
        MysqlConn.ConnectionString =
            "server=localhost;uid=root;password=password;database=market"
        MysqlConn.Open()

        Try
            '  Dim Query As String = "select * from market.items where catagory = '" + selectItem + "'"
            '  COMMAND = New MySqlCommand(Query, MysqlConn)
            READER = COMMAND.ExecuteReader
            '清空ListBox的items
            '  ItemsListBox.Items.Clear()
            '重新从数据库中加载值
            While READER.Read
                '      ItemsListBox.Items.Add(READER.GetString("name"))
            End While

            MysqlConn.Close()
        Catch ex As MySqlException
            MessageBox.Show(ex.Message)
        Finally
            MysqlConn.Dispose()
        End Try

        Return 0
    End Function


    Private Sub ListView_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        '显示选中的人的详情信息
        Dim Selected_Name As String = Get_Selected_Name()
        'MsgBox(Selected_Name)
        Add_New_Contact_To_ListBox("hah" + Selected_Name)

    End Sub

    Private Sub Add_New_Contact_To_ListBox(ByVal name As String)
        '往列表中添加新的一项
        Dim PathTemplate As Path = New Path
        PathTemplate.Data = New GeometryConverter().ConvertFromString("M6,17C6,15 10,13.9 12,13.9C14,13.9 18,15 18,17V18H6M15,9A3,3 0 0,1 12,12A3,3 0 0,1 9,9A3,3 0 0,1 12,6A3,3 0 0,1 15,9M3,5V19A2,2 0 0,0 5,21H19A2,2 0 0,0 21,19V5A2,2 0 0,0 19,3H5C3.89,3 3,3.9 3,5Z")
        PathTemplate.Fill = New BrushConverter().ConvertFromString("Black")
        Dim CanvasTemplate As Canvas = New Canvas
        CanvasTemplate.Width = 24
        CanvasTemplate.Height = 24
        CanvasTemplate.Children.Add(PathTemplate)
        Dim ViewBoxTemplate As Viewbox = New Viewbox
        ViewBoxTemplate.Child = CanvasTemplate
        ViewBoxTemplate.Width = 48
        ViewBoxTemplate.Height = 48
        Dim NewPanel As WrapPanel = New WrapPanel
        NewPanel.Children.Add(ViewBoxTemplate)
        Dim Label1 As Label = New Label()
        Label1.Content = name
        NewPanel.Children.Add(Label1)
        ContactsListBox.Items.Add(NewPanel)
    End Sub

    Private Function Get_Selected_Name() As String
        Try
            Dim elements As UIElementCollection = ContactsListBox.SelectedItem.Children
            Dim element As UIElement
            For Each element In elements
                If element.GetType.ToString.EndsWith("Label") Then
                    Dim label As Label = element
                    Return label.Content
                End If
            Next
        Catch er As Exception
            'nothing
        End Try
        Return ""
    End Function

    Private Sub Button_Click(sender As Object, e As RoutedEventArgs)
        '添加电话项目
        Dim NewTextBox As TextBox = New TextBox
        NewTextBox.Width = 160
        NewTextBox.Height = 25
        NewTextBox.HorizontalAlignment = HorizontalAlignment.Center
        NewTextBox.VerticalAlignment = VerticalAlignment.Bottom
        PhoneNumsListBox.Items.Add(NewTextBox)

    End Sub
End Class

Imports System.Data

Partial Public Class Window_Login

    Private Sub Window1_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Window1.Loaded
        Try
            Dim t As New System.Windows.Media.ImageSourceConverter
            Me.Icon = t.ConvertFromString(Environment.CurrentDirectory & "\Resources\mainico.ico")
            Image1.Source = t.ConvertFromString(Environment.CurrentDirectory & "\Resources\a.jpg")
            ComboBox2.SelectedIndex = 0
            LoadUsers()
            ComboBox1.Focus()
        Catch ex As Exception
            ErrorLogger.LogError(ex, Me.Title)
            MsgBox("Error Occured. Please check log for more details.", MsgBoxStyle.Information, MessageTitle)
        End Try
    End Sub

    Private Sub LoadUsers()
        Try
            Dim MyConn As New OleDb.OleDbConnection(ConnString)
            Dim ConnectionQuery As String

            ConnectionQuery = "select uid from users where companyid='" & ComboBox2.Text & "'"

            Dim MyCommand As New OleDb.OleDbCommand(ConnectionQuery, MyConn)
            Dim MyReader As OleDb.OleDbDataReader
            MyConn.Open()

            MyReader = MyCommand.ExecuteReader

            While MyReader.Read
                ComboBox1.Items.Add(MyReader(0))
            End While

            MyConn.Close()
        Catch ex As Exception
            ErrorLogger.LogError(ex, Me.Title)
            MsgBox("Error Occured. Please check log for more details.", MsgBoxStyle.Information, MessageTitle)
        End Try
    End Sub

    Private Sub Label2_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Input.MouseButtonEventArgs) Handles Label2.MouseDown
        Try
            Dim TheM As New start_window

            If ConnectionObject.Login(ComboBox1.Text, TextBox1.Password, ComboBox2.Text) Then
                LoggedInCompanyName = ComboBox2.Text
                LoggedInUserId = ComboBox1.Text

                TheM.Show()
                Me.Close()
            Else
                MsgBox("Wrong Password !", MsgBoxStyle.Information, MessageTitle)
                TextBox1.Focus()
            End If
        Catch ex As Exception
            ErrorLogger.LogError(ex, Me.Title)
            MsgBox("Error Occured. Please check log for more details.", MsgBoxStyle.Information, MessageTitle)
        End Try

    End Sub

    Private Sub Label2_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles Label2.KeyDown
        Try
            If e.Key = Key.Enter Then
                Dim TheM As New start_window

                If ConnectionObject.Login(ComboBox1.Text, TextBox1.Password, ComboBox2.Text) Then
                    LoggedInCompanyName = ComboBox2.Text
                    LoggedInUserId = ComboBox1.Text

                    TheM.Show()
                    Me.Close()
                Else
                    MsgBox("Wrong Password !", MsgBoxStyle.Information, MessageTitle)
                    TextBox1.Focus()
                End If
            End If
        Catch ex As Exception
            ErrorLogger.LogError(ex, Me.Title)
            MsgBox("Error Occured. Please check log for more details.", MsgBoxStyle.Information, MessageTitle)
        End Try
    End Sub

    Private Sub Label3_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Input.MouseButtonEventArgs) Handles Label3.MouseDown
        End

    End Sub

    Private Sub ComboBox1_SelectionChanged(ByVal sender As Object, ByVal e As System.Windows.Controls.SelectionChangedEventArgs) Handles ComboBox1.SelectionChanged
        TextBox1.Focus()
    End Sub

    Private Sub TextBox1_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles TextBox1.KeyDown
        If e.Key = Key.Enter Then
            Label2.Focus()
        End If
    End Sub

    Private Sub Window_Login_Initialized(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Initialized
        Dim KEY As Integer = Convert.ToInt32(Microsoft.Win32.Registry.CurrentUser.OpenSubKey("BICE").GetValue("KEY"))
        If KEY = 0 Then
            MsgBox("Product Not Authorized. Shutting Down.", MsgBoxStyle.Information, MessageTitle)
            End
        End If
    End Sub

   
End Class

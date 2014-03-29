Partial Public Class Window_ChangePass

    Private Sub Label3_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Input.MouseButtonEventArgs) Handles Label3.MouseDown
        Me.Close()
    End Sub

    Private Sub Window1_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Window1.Loaded
        TextBox1.Text = LoggedInUserId
        TextBox1.IsReadOnly = True
    End Sub

    Private Sub Label2_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Input.MouseButtonEventArgs) Handles Label2.MouseDown
        Try
            If Not TextBox1.Text = Nothing And Not TextBox2.Text = Nothing Then
                ConnectionObject2.ChangeUserPass(TextBox1.Text, TextBox2.Text)
                MsgBox("Password Successfully Changed.", MsgBoxStyle.Information, MessageTitle)
                Me.Close()
            Else
                MsgBox("Please Enter all details.", MsgBoxStyle.Information, MessageTitle)
            End If
        Catch ex As Exception
            ErrorLogger.LogError(ex, Me.Title)
            MsgBox("Error Occured. Please check log for more details.", MsgBoxStyle.Information, MessageTitle)
        End Try
    End Sub

End Class

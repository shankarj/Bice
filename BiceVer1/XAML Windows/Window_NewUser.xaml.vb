Partial Public Class Window_NewUser

    Private Sub Label2_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Input.MouseButtonEventArgs) Handles Label2.MouseDown
        Try
            If Not Text_un.Text = Nothing And Not Text_pass.Text = Nothing Then
                ConnectionObject2.CreateUser(Text_un.Text, Text_pass.Text)
                Text_un.Clear()
                Text_pass.Clear()
                MsgBox("User Successfully Created.", MsgBoxStyle.Information, MessageTitle)
                Me.Close()
            End If
        Catch ex As Exception
            ErrorLogger.LogError(ex, Me.Title)
            MsgBox("Error Occured. Please check log for more details.", MsgBoxStyle.Information, MessageTitle)
        End Try
    End Sub

    Private Sub Label3_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Input.MouseButtonEventArgs) Handles Label3.MouseDown
        Me.Close()
    End Sub

    Private Sub Text_un_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles Text_un.KeyDown
        If e.Key = Key.Enter Then
            Text_pass.Focus()
        End If
    End Sub

    Private Sub Text_pass_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles Text_pass.KeyDown
        If e.Key = Key.Enter Then
            Label2.Focus()
        End If
    End Sub
End Class

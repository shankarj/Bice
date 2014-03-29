Imports System.Data

Partial Public Class Window_PendingCredits
    Dim TheTable As DataTable

    Private Sub Window1_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        Try
            TheTable = ConnectionObject2.GetPendingDetails().DefaultView.Table
            grid_prod.ItemsSource = TheTable.DefaultView
            Label2.Content &= "   Rs. " & Convert.ToString(ConnectionObject2.ReturnsPendingTotal)
        Catch ex As Exception
            ErrorLogger.LogError(ex, Me.Title)
            MsgBox("Error Occured. Please check log for more details.", MsgBoxStyle.Information, MessageTitle)
        End Try
    End Sub

    Private Sub grid_prod_BeginningEdit(ByVal sender As Object, ByVal e As Microsoft.Windows.Controls.DataGridBeginningEditEventArgs) Handles grid_prod.BeginningEdit
        Try
            Dim SelIndex As Int16 = grid_prod.SelectedIndex

            If SelIndex >= 0 Then
                If MsgBox("Mark Bill Number : " & TheTable.Rows(SelIndex)(0) & " as paid ?", MsgBoxStyle.Information + MsgBoxStyle.YesNo, MessageTitle) = MsgBoxResult.Yes Then
                    ConnectionObject2.DeletePendingEntry(TheTable.Rows(SelIndex)(0))
                    Try
                        TheTable.Rows.RemoveAt(SelIndex)
                    Catch ex As Exception
                    End Try

                End If
            End If

            Totaller()
            e.Cancel = True

        Catch ex As Exception
            ErrorLogger.LogError(ex, Me.Title)
            MsgBox("Error Occured. Please check log for more details.", MsgBoxStyle.Information, MessageTitle)
        End Try

    End Sub

    Private Sub Totaller()
        Try
            If TheTable.Rows.Count >= 0 Then
                Dim TheSum = 0

                For index As Integer = 0 To TheTable.Rows.Count - 1
                    TheSum += Val(Convert.ToInt64(TheTable.Rows(index)(2)))
                Next

                Label2.Content = "TOTAL PENDING CREDIT BILLS VALUE : Rs. " & TheSum
            End If
        Catch ex As Exception
            ErrorLogger.LogError(ex, Me.Title)
            MsgBox("Error Occured. Please check log for more details.", MsgBoxStyle.Information, MessageTitle)
        End Try
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button1.Click
        Me.Close()
    End Sub

End Class

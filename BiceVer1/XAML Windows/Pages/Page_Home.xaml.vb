Partial Public Class Page_Home

    Private Sub Page_Home_Initialized(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Initialized
        AssignList(ListBox1)
        Label2.Content &= "    " & LoggedInCompanyName.ToUpper
        Label3.Content &= "    " & LoggedInUserId.ToUpper
    End Sub

    Private Sub ListBox1_SelectionChanged(ByVal sender As System.Object, ByVal e As System.Windows.Controls.SelectionChangedEventArgs) Handles ListBox1.SelectionChanged
        Dim TempString As String = ListBox1.SelectedValue

        If Not ListBox1.SelectedIndex = -1 Then
            Dim OpenNo As Integer = TempString.Substring(0, 3)
            TheListBox.Items.RemoveAt(ListBox1.SelectedIndex)
            BillObject(OpenNo).Show()
        End If



    End Sub

End Class

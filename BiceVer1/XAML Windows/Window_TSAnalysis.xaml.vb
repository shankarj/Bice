Imports System.Data
Imports System.Data.OleDb

Partial Public Class Window_TSAnalysis

#Region "CHART AND DATABASE"

    Private Sub LoadChart1()
        Try
            Dim conn As New OleDbConnection(ConnString)
            Dim Conquery As String

            Conquery = "Select billdate, sum(billvalue) as bs from all_entries where billtype='CASH' and billdate= #" & Date_Today & "# and companyid='" & LoggedInCompanyName & "' group by billdate"

            Dim adapter As OleDbDataAdapter = New OleDbDataAdapter(Conquery, conn)
            Dim ds As New DataSet("mdset")

            conn.Open()
            adapter.Fill(ds, "all_entries")

            Mychart.SeriesSource = ds.Tables(0).DefaultView
            Mychart.IDMemberPath = "billdate"
            Mychart.TextMemberPath = "billdate"

            a1.DataItemsSource = ds.Tables(0).DefaultView
            a1.SeriesIDMemberPath = "billdate"
            a1.ValueMemberPath = "bs"

            ds.Dispose()
            adapter.Dispose()

            Conquery = "Select billdate, sum(billvalue) as bs from all_entries where billtype='CREDIT' and billdate = #" & Date_Today & "# and companyid='" & LoggedInCompanyName & "' group by billdate"
            Dim adapter2 As OleDbDataAdapter = New OleDbDataAdapter(Conquery, conn)
            Dim d2s As New DataSet("mdset2")

            adapter2.Fill(d2s, "all_entries")


            a2.DataItemsSource = d2s.Tables(0).DefaultView
            a2.SeriesIDMemberPath = "billdate"
            a2.ValueMemberPath = "bs"

            d2s.Dispose()
            adapter2.Dispose()
            conn.Close()
        Catch ex As Exception
            ErrorLogger.LogError(ex, Me.Content)
            MsgBox("Error Occured. Please check log for more details.", MsgBoxStyle.Information, MessageTitle)
        End Try
    End Sub

    Private Sub LoadChart2()
        Try
            Dim conn As New OleDbConnection(ConnString)
            Dim Conquery As String

            Conquery = "Select billdate, sum(billvalue) as bs from all_entries where billtype='CASH' and billdate between #" & dtp_from.SelectedDate & "# and #" & dtp_to.SelectedDate & "# and companyid='" & LoggedInCompanyName & "' group by billdate"


            Dim adapter As OleDbDataAdapter = New OleDbDataAdapter(Conquery, conn)
            Dim ds As New DataSet("mdset")

            conn.Open()
            adapter.Fill(ds, "all_entries")

            Mychart.SeriesSource = ds.Tables(0).DefaultView
            Mychart.IDMemberPath = "billdate"
            Mychart.TextMemberPath = "billdate"

            a1.DataItemsSource = ds.Tables(0).DefaultView
            a1.SeriesIDMemberPath = "billdate"
            a1.ValueMemberPath = "bs"
            ds.Dispose()
            adapter.Dispose()


            Conquery = "Select billdate, sum(billvalue) as bs from all_entries where billtype='CREDIT' and billdate between #" & dtp_from.SelectedDate & "# and #" & dtp_to.SelectedDate & "# and companyid='" & LoggedInCompanyName & "' group by billdate"
            Dim adapter2 As OleDbDataAdapter = New OleDbDataAdapter(Conquery, conn)
            Dim d2s As New DataSet("mdset2")

            adapter2.Fill(d2s, "all_entries")


            a2.DataItemsSource = d2s.Tables(0).DefaultView
            a2.SeriesIDMemberPath = "billdate"
            a2.ValueMemberPath = "bs"

            d2s.Dispose()
            adapter2.Dispose()
            conn.Close()
        Catch ex As Exception
            ErrorLogger.LogError(ex, Me.Content)
            MsgBox("Error Occured. Please check log for more details.", MsgBoxStyle.Information, MessageTitle)
        End Try
    End Sub

    Private Sub DrawChart()
        If CheckBox1.IsChecked = True Then
            LoadChart1()
        Else
            LoadChart2()
        End If
    End Sub
#End Region

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button2.Click
        Try
            DrawChart()
        Catch ex As Exception
        End Try
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button1.Click
        Me.Close()
    End Sub
End Class

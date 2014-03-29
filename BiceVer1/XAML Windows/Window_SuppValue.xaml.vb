Imports System.Data
Imports System.Data.OleDb

Partial Public Class Window_SuppValue

    Private Sub Window1_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Window1.Loaded
        LoadChart1()
    End Sub

    Private Sub LoadChart1()
        Try
            Dim conn As New OleDbConnection(ConnString)
            Dim Conquery As String

            Conquery = "select suppliername, totalordervalue from stock_supplier where companyid='" & LoggedInCompanyName & "' group by suppliername, totalordervalue"

            Dim adapter As OleDbDataAdapter = New OleDbDataAdapter(Conquery, conn)
            Dim ds As New DataSet("mdset")

            conn.Open()
            adapter.Fill(ds, "stock_supplier")

            MyChart.SeriesSource = ds.Tables(0).DefaultView
            MyChart.IDMemberPath = "suppliername"
            MyChart.TextMemberPath = "suppliername"

            a1.DataItemsSource = ds.Tables(0).DefaultView
            a1.SeriesIDMemberPath = "suppliername"
            a1.ValueMemberPath = "totalordervalue"

            ds.Dispose()
            adapter.Dispose()

            conn.Close()
        Catch ex As Exception
        End Try
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button1.Click
        Me.Close()
    End Sub
End Class

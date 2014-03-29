Imports System.Data
Imports System.Data.OleDb
Imports AmCharts
Imports AmCharts.Windows.Core

Partial Public Class Window_CustChoice

    Private Sub LoadChart()
        Try
            Dim conn As New OleDb.OleDbConnection(ConnString)
            Dim Conquery As String

            Conquery = "select productname, sum(quantity) as sq from  bill_detail, menu_products where productid=menu_products.id and menu_products.companyid='" & LoggedInCompanyName & "' group by productname"

            Dim adapter As OleDb.OleDbDataAdapter = New OleDb.OleDbDataAdapter(Conquery, conn)
            Dim ds As New DataSet("mdset")

            conn.Open()
            adapter.Fill(ds, " bill_detail, menu_products")

            Dim slicesBinding As New Binding
            slicesBinding.Source = ds.Tables(0).DefaultView
            pieChart1.SetBinding(Windows.PieChart.SlicesSourceProperty, slicesBinding)

            pieChart1.ValueMemberPath = "sq"
            pieChart1.TitleMemberPath = "productname"

            'MyChart.SeriesSource = ds.Tables(0).DefaultView
            'MyChart.IDMemberPath = "productname"
            'MyChart.TextMemberPath = "productname"

            'a1.DataItemsSource = ds.Tables(0).DefaultView
            'a1.SeriesIDMemberPath = "productname"
            'a1.ValueMemberPath = "sq"

            ds.Dispose()
            adapter.Dispose()
        Catch ex As Exception
            ErrorLogger.LogError(ex, "LOAD CHART")
            MsgBox("Error Occured. Please check log for more details.", MsgBoxStyle.Information, MessageTitle)
        End Try
    End Sub

    Private Sub Window1_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Window1.Loaded
        LoadChart()
    End Sub
End Class


Imports System.Data
Partial Public Class Window_StockAnalysis

    Private Sub Window1_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Window1.Loaded
        Try
            grid_list.ItemsSource = ObtainLessStocks().DefaultView
            LoadChart(False)
        Catch ex As Exception
        End Try
    End Sub

    Private Sub CheckBox1_Checked(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles CheckBox1.Checked
        LoadChart(True)
    End Sub

    Private Sub CheckBox1_Unchecked(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles CheckBox1.Unchecked
        LoadChart(False)
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button1.Click
        Me.Close()
    End Sub

#Region "CHART AND DATABASE"
    Private Function ObtainLessStocks() As DataTable
        Dim MyConn As New OleDb.OleDbConnection(ConnString)

        Try
            Dim Yes As Boolean = False
            Dim ConnectionQuery As String = "select  productname, id, unitcost,quantityavailable  from stock_stub where quantityavailable < thresholdlevel and companyid='" & LoggedInCompanyName & "'"
            Dim AReader As OleDb.OleDbDataReader
            Dim MyCommand As New OleDb.OleDbCommand(ConnectionQuery, MyConn)
            Dim TempString() As String = Nothing
            Dim Table_List As New DataTable
            Dim index As Int16 = 0

            MyConn.Open()

            AReader = MyCommand.ExecuteReader
            Table_List.Load(AReader)

            Table_List.Columns(0).ColumnName = "Product Name"
            Table_List.Columns(1).ColumnName = "Id"
            Table_List.Columns(2).ColumnName = "Unit Cost"
            Table_List.Columns(3).ColumnName = "Available"
            MyConn.Close()

            Return Table_List

        Catch ex As Exception
            ErrorLogger.LogError(ex, "ObtainLessStocks")
            MsgBox("Error Occured. Please check log for more details.", MsgBoxStyle.Information, MessageTitle)
            MyConn.Close()
            Return Nothing
        End Try
    End Function

    Private Sub LoadChart(ByVal IsAll As Boolean)
        Try
            Dim conn As New OleDb.OleDbConnection(ConnString)
            Dim ConnStr As String = Nothing

            If IsAll = True Then
                ConnStr = "select  productname, quantityavailable  from stock_stub where companyid='" & LoggedInCompanyName & "'"
            Else
                ConnStr = "select  productname, quantityavailable  from stock_stub where quantityavailable < thresholdlevel and companyid='" & LoggedInCompanyName & "'"
            End If

            Dim adapter As OleDb.OleDbDataAdapter = New OleDb.OleDbDataAdapter(ConnStr, conn)
            Dim ds As New DataSet("mdset")

            conn.Open()
            adapter.Fill(ds, "stock_stub")

            MyChart.SeriesSource = ds.Tables(0).DefaultView
            MyChart.IDMemberPath = "productname"
            MyChart.TextMemberPath = "productname"

            a1.DataItemsSource = ds.Tables(0).DefaultView
            a1.SeriesIDMemberPath = "productname"
            a1.ValueMemberPath = "quantityavailable"


            ds.Dispose()
            adapter.Dispose()
        Catch ex As Exception
            ErrorLogger.LogError(ex, "LOAD CHART")
            MsgBox("Error Occured. Please check log for more details.", MsgBoxStyle.Information, MessageTitle)
        End Try
    End Sub
#End Region

End Class


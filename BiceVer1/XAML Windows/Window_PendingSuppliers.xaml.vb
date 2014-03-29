Imports System.Data
Imports System.Data.OleDb

Partial Public Class Window_PendingSuppliers
    Dim Supplier_Id As String


    Private Sub Window1_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Window1.Loaded
        Try
            grid_list.ItemsSource = LoadPendingOrders.DefaultView
        Catch ex As Exception
            ErrorLogger.LogError(ex, Me.Content)
            MsgBox("Error Occured. Please check log for more details.", MsgBoxStyle.Information, MessageTitle)
        End Try
    End Sub

    Private Sub grid_list_BeginningEdit(ByVal sender As Object, ByVal e As Microsoft.Windows.Controls.DataGridBeginningEditEventArgs) Handles grid_list.BeginningEdit
        Try
            grid_order.ItemsSource = LoadOrderDetails.DefaultView
            UpdateTotal()
        Catch ex As Exception
            ErrorLogger.LogError(ex, Me.Content)
            MsgBox("Error Occured. Please check log for more details.", MsgBoxStyle.Information, MessageTitle)
        End Try
    End Sub

#Region "DATABASE CODES"

    Private Sub UpdateTotal()
        Try
            Dim Sum As Long = 0

            Try
                For index As Integer = 0 To grid_order.Items.Count - 1
                    Sum += grid_order.Items(index)(3)
                Next
            Catch ex As Exception
            End Try

            Label_value.Content = "ORDER VALUE : Rs. " & Sum & " /-"
        Catch ex As Exception
            ErrorLogger.LogError(ex, Me.Content)
            MsgBox("Error Occured. Please check log for more details.", MsgBoxStyle.Information, MessageTitle)
        End Try
    End Sub

    Private Function LoadPendingOrders() As DataTable
        Try
            Dim MyConn As New OleDb.OleDbConnection(ConnString)
            Dim ConnectionQuery As String

            ConnectionQuery = "SELECT suppliername, orderid, stock_orderhistory.duedate, id, sum(cost), invoice FROM stock_orderhistory, stock_supplier " & _
                              "where id=supplierid and pendingstatus='pending' and stock_supplier.companyid='" & LoggedInCompanyName & "' GROUP BY suppliername, orderid, id,invoice, stock_orderhistory.duedate"


            Dim MyCommand As New OleDb.OleDbCommand(ConnectionQuery, MyConn)
            Dim AReader As OleDb.OleDbDataReader
            Dim Table_List As New DataTable

            MyConn.Open()

            AReader = MyCommand.ExecuteReader
            Table_List.Load(AReader)

            Table_List.Columns(0).ColumnName = "Supplier Name"
            Table_List.Columns(1).ColumnName = "Order ID"
            Table_List.Columns(2).ColumnName = "Due"
            Table_List.Columns(3).ColumnName = "Supplier ID"
            Table_List.Columns(4).ColumnName = "Order Value"
            Table_List.Columns(5).ColumnName = "Invoice"

            MyConn.Close()

            Return Table_List
        Catch ex As Exception
            ErrorLogger.LogError(ex, "LoadPendingOrders")
            MsgBox("Error Occured. Please check log for more details.", MsgBoxStyle.Information, MessageTitle)
            Return Nothing
        End Try
    End Function

    Private Function LoadOrderDetails() As DataTable
        Try
            Dim Table_List As DataTable
            Dim MyConn As New OleDb.OleDbConnection(ConnString)
            Dim ConnectionQuery As String
            Dim SelIndex As Int16 = grid_list.SelectedIndex

            ConnectionQuery = "SELECT productid, productquantity, percost, cost FROM  stock_orderhistory, stock_supplier WHERE id=supplierid " & _
                              "AND orderid='" & grid_list.Items(SelIndex)(1) & "' AND pendingstatus='pending' AND stock_orderhistory.companyid='" & LoggedInCompanyName & "' group by productid, productquantity, percost, cost"

            Dim MyCommand As New OleDb.OleDbCommand(ConnectionQuery, MyConn)
            Dim AReader As OleDb.OleDbDataReader


            MyConn.Open()

            Table_List = New DataTable
            AReader = MyCommand.ExecuteReader
            Table_List.Load(AReader)

            Table_List.Columns(0).ColumnName = "Product ID"
            Table_List.Columns(1).ColumnName = "Quantity Ordered"
            Table_List.Columns(2).ColumnName = "Unit Cost"
            Table_List.Columns(3).ColumnName = "Total Cost"

            MyConn.Close()

            Text_id.Text = Convert.ToString(grid_list.Items(SelIndex)(1))
            Text_supplier.Text = Convert.ToString(grid_list.Items(SelIndex)(0))
            Text_invoice.Text = Convert.ToString(grid_list.Items(SelIndex)(5))
            Text_due.Text = Convert.ToString(grid_list.Items(SelIndex)(2))



            Return Table_List
        Catch ex As Exception
            ErrorLogger.LogError(ex, "LoadOrderDetails")
            MsgBox("Error Occured. Please check log for more details.", MsgBoxStyle.Information, MessageTitle)
            Return Nothing
        End Try
    End Function

#End Region

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button1.Click
        Me.Close()
    End Sub
End Class


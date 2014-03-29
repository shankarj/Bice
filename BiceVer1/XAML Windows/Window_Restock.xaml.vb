Imports System.Data

Partial Public Class Window_Restock

#Region "DECLARATIONS"
    Dim OrderDone As Boolean = False
    Dim Table_List, FinalTable As New DataTable
    Dim Supplier_Id As String
    Dim SelIndex As Int16
#End Region

    Private Sub Window1_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Window1.Loaded
        Try
            grid_list.ItemsSource = LoadPendingOrders.DefaultView
        Catch ex As Exception
        End Try
    End Sub

    Private Sub grid_list_BeginningEdit(ByVal sender As Object, ByVal e As Microsoft.Windows.Controls.DataGridBeginningEditEventArgs) Handles grid_list.BeginningEdit
        Try
            grid_order.ItemsSource = LoadOrderDetails.DefaultView
            OrderDone = True
            UpdateTotal()
        Catch ex As Exception
        End Try

    End Sub

    Private Sub Button_finalize_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button_finalize.Click
        Try
            If OrderDone = True Then
                If MsgBox("Finalize the order ?", MsgBoxStyle.Information + MsgBoxStyle.YesNo, MessageTitle) = MsgBoxResult.Yes Then
                    LoadTable()
                    InventoryObject.ReStock(Text_id.Text, Supplier_Id, FinalTable)
                    OrderDone = False
                End If
            Else
                MsgBox("Please choose an order to restock.", MsgBoxStyle.Exclamation, MessageTitle)

            End If
        Catch ex As Exception
        End Try
    End Sub

    Private Sub grid_order_SelectionChanged(ByVal sender As Object, ByVal e As System.Windows.Controls.SelectionChangedEventArgs) Handles grid_order.SelectionChanged
        Try
            For index As Integer = 0 To grid_order.Items.Count - 2
                Dim Val2 As Long = grid_order.Items.Item(index)(2)
                Dim Val1 As Long = grid_order.Items(index)(1)

                Try
                    grid_order.Items(index)(3) = Val1 * Val2
                Catch ex As Exception
                End Try
            Next

            UpdateTotal()
        Catch ex As Exception

        End Try
    End Sub

#Region "CONTROL DATABASE FUNCTIONS"

    Private Function LoadPendingOrders() As DataTable
        Try
            Dim MyConn As New OleDb.OleDbConnection(ConnString)
            Dim ConnectionQuery As String

            ConnectionQuery = "SELECT orderid, stock_orderhistory.duedate, id, suppliername, sum(cost), invoice FROM stock_orderhistory, stock_supplier " & _
                              "where id=supplierid and pendingstatus='pending' and stock_supplier.companyid='" & LoggedInCompanyName & "' GROUP BY orderid, id, suppliername, invoice, stock_orderhistory.duedate"


            Dim MyCommand As New OleDb.OleDbCommand(ConnectionQuery, MyConn)
            Dim AReader As OleDb.OleDbDataReader
            Dim Table_List As New DataTable

            MyConn.Open()

            AReader = MyCommand.ExecuteReader
            Table_List.Load(AReader)

            Table_List.Columns(0).ColumnName = "Order ID"
            Table_List.Columns(1).ColumnName = "DUE"
            Table_List.Columns(2).ColumnName = "Supplier ID"
            Table_List.Columns(3).ColumnName = "Supplier Name"
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
            Dim MyConn As New OleDb.OleDbConnection(ConnString)
            Dim ConnectionQuery As String
            Dim SelIndex As Int16 = grid_list.SelectedIndex

            ConnectionQuery = "SELECT productid, productquantity, percost, cost FROM  stock_orderhistory, stock_supplier WHERE id=supplierid " & _
                              "AND orderid='" & grid_list.Items(SelIndex)(0) & "' AND pendingstatus='pending' AND stock_orderhistory.companyid='" & LoggedInCompanyName & "' group by productid, productquantity, percost, cost"

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

            Text_id.Text = grid_list.Items(SelIndex)(0)
            Text_supplier.Text = grid_list.Items(SelIndex)(3)
            Text_invoice.Text = grid_list.Items(SelIndex)(5)
            Supplier_Id = grid_list.Items(SelIndex)(2)

            MyConn.Close()

            Text_id.Text = Convert.ToString(grid_list.Items(SelIndex)(0))
            Text_supplier.Text = Convert.ToString(grid_list.Items(SelIndex)(3))
            Text_invoice.Text = Convert.ToString(grid_list.Items(SelIndex)(5))

            Return Table_List
        Catch ex As Exception
            ErrorLogger.LogError(ex, "LoadProductDetails")
            MsgBox("Error Occured. Please check log for more details.", MsgBoxStyle.Information, MessageTitle)
            Return Nothing
        End Try
    End Function

    Private Sub UpdateTotal()
        Dim Sum As Long = 0

        Try
            For index As Integer = 0 To grid_order.Items.Count - 1
                Sum += grid_order.Items(index)(3)
            Next
        Catch ex As Exception
        End Try

        Label_value.Content = "ORDER VALUE : Rs. " & Sum & " /-"

    End Sub

    Private Sub LoadTable()
        FinalTable = New DataTable
        FinalTable.Columns.Add("Product ID")
        FinalTable.Columns.Add("Quantity Ordered")
        FinalTable.Columns.Add("Unit Cost")
        FinalTable.Columns.Add("Total Cost")

        For index As Integer = 0 To Table_List.Rows.Count - 1
            FinalTable.Rows.Add(Table_List.Rows(index)(0), Table_List.Rows(index)(1), Table_List.Rows(index)(2), Table_List.Rows(index)(3))
        Next
    End Sub


#End Region

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button1.Click
        Me.Close()
    End Sub
End Class
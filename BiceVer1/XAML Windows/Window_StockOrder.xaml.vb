Imports System.Data

Partial Public Class Window_StockOrder

#Region "DECLARATIONS"
    Dim TheTable As New DataTable
    Dim TotalCost As Integer = 0
    Dim Sum As Integer = 0
#End Region

    Private Sub Window1_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Window1.Loaded
        Try
            grid_list.ItemsSource = ObtainLessStocks.DefaultView
            TheTable.Columns.Add("PRODUCT ID")
            TheTable.Columns.Add("PRODUCT QUANTITY")
            TheTable.Columns.Add("UNIT COST")
            TheTable.Columns.Add("TOTAL COST")
            grid_order.ItemsSource = TheTable.DefaultView
            GetOrderId()
            UpdateItemCount()
        Catch ex As Exception
        End Try
    End Sub

    Private Sub Text_id_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles Text_id.KeyDown
        Try
            If e.Key = Key.Enter Then
                If Not Text_id.Text = Nothing Then
                    Dim Temp() As String = InventoryObject.GetRawProductDetails(Text_id.Text)
                    If Temp(0) = "NAME" Then
                        Text_id.Text = Temp(1)
                    End If
                    Text_cost.Text = Temp(2)
                    Text_measured.Text = Temp(3)
                End If
                Text_quant.Focus()
            End If
        Catch ex As Exception
        End Try
    End Sub

    Private Sub Text_quant_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles Text_quant.KeyDown
        If e.Key = Key.Enter And Not Text_quant.Text = Nothing Then
            Text_thresh.Text = Convert.ToString(Val(Text_quant.Text) * Val(Text_cost.Text))
            Button_add.Focus()
        End If
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button1.Click
        ClearAll()
    End Sub

    Private Sub Button_add_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button_add.Click
        TheTable.Rows.Add(Text_id.Text, Val(Text_quant.Text), Val(Text_cost.Text), Val(Text_thresh.Text))
        UpdateItemCount()
        ClearAll()
    End Sub

#Region "CONTROL DATABASE CODES"

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
            Table_List.Columns.Add("Order Quantity")

            MyConn.Close()

            Return Table_List

        Catch ex As Exception
            ErrorLogger.LogError(ex, "ObtainLessStocks")
            MsgBox("Error Occured. Please check log for more details.", MsgBoxStyle.Information, MessageTitle)
            MyConn.Close()
            Return Nothing
        End Try

    End Function

    Private Sub GetOrderId()
        Dim MyConn As New OleDb.OleDbConnection(ConnString)

        Try
            Dim Yes As Boolean = False
            Dim ConnectionQuery As String = "select max(orderid) from stock_orderhistory where companyid='" & LoggedInCompanyName & "'"
            Dim AReader As OleDb.OleDbDataReader
            Dim MyCommand As New OleDb.OleDbCommand(ConnectionQuery, MyConn)

            MyConn.Open()

            AReader = MyCommand.ExecuteReader

            While AReader.Read
                Try
                    Text_order.Text = Convert.ToString(Val(AReader(0)) + 1)
                Catch ex As Exception
                End Try
            End While

            MyConn.Close()

        Catch ex As Exception
            ErrorLogger.LogError(ex, "GetORderId")
            MsgBox("Error Occured. Please check log for more details.", MsgBoxStyle.Information, MessageTitle)
            MyConn.Close()
        End Try
    End Sub
#End Region

    Private Sub ClearAll()
        Text_id.Text = Nothing
        Text_order.Text = Nothing
        Text_measured.Text = Nothing
        Text_quant.Text = Nothing
        Text_cost.Text = Nothing
        Text_thresh.Text = Nothing
        Text_id.Focus()
    End Sub

    Private Sub UpdateItemCount()

        Label14.Content = "TOTAL ITEMS IN THE ORDER : " & grid_order.Items.Count - 1

        For index As Integer = 0 To TheTable.Rows.Count - 1
            Sum += TheTable.Rows(index)(3)
        Next

        Label_value.Content = "ORDER VALUE : Rs. " & Sum
    End Sub
   
    Private Sub grid_order_BeginningEdit(ByVal sender As Object, ByVal e As Microsoft.Windows.Controls.DataGridBeginningEditEventArgs) Handles grid_order.BeginningEdit
        Try
            Dim SelIndex As Int16 = grid_order.SelectedIndex
            If MsgBox("Confirm Deleting the selected item ?", MsgBoxStyle.Information + MsgBoxStyle.YesNo, MessageTitle) = MsgBoxResult.Yes Then
                TheTable.Rows.RemoveAt(SelIndex)
                UpdateItemCount()
            End If
        Catch ex As Exception
        End Try
    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button4.Click
        Try
            If MsgBox("Finalize the above order with changes completely ?", MsgBoxStyle.Information + MsgBoxStyle.YesNo, MessageTitle) = MsgBoxResult.Yes Then
                Dim Quantity As Integer = 0

                For index As Integer = 0 To grid_list.Items.Count - 2
                    If Not TypeOf (grid_list.Items(index)(4)) Is DBNull Then
                        TotalCost = grid_list.Items(index)(4) * grid_list.Items(index)(2)
                        TheTable.Rows.Add(grid_list.Items(index)(1), grid_list.Items(index)(4), grid_list.Items(index)(2), TotalCost)
                    End If
                Next
                UpdateItemCount()
                grid_list.IsEnabled = False
                Button4.IsEnabled = False
            End If
        Catch ex As Exception
        End Try
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button3.Click
        Try
            Try
                If Not Text_supplier.Text = Nothing And Not Text_order.Text = Nothing And grid_order.Items.Count > 0 Then
                    If InventoryObject.NewOrder(Text_order.Text, Text_supplier.Text, TheTable, Convert.ToString(dtp_from.SelectedDate.Value), Sum) = True Then
                        ClearAll()
                        TheTable.Rows.Clear()
                    End If
                    Text_id.Focus()
                Else
                    MsgBox("Please enter all details", MsgBoxStyle.Information, MessageTitle)
                End If
            Catch ex As InvalidOperationException
                MsgBox("Please Choose a date.", MsgBoxStyle.Information, MessageTitle)
            End Try
        Catch ex As Exception
        End Try
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button2.Click
        Me.Close()
    End Sub

End Class

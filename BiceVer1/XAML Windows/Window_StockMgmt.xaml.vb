Imports System.Data

Partial Public Class Window_StockMgmt

    Private Sub Window1_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Window1.Loaded
        Try
            LoadRawProductsList()
            grid_list.ItemsSource = GetListofRawProducts.DefaultView
        Catch ex As Exception
        End Try
    End Sub

    Private Sub btn_show_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btn_show.Click
        lbl_date.Content = "ORDER DATE :"
        lbl_id.Content = "ORDER ID :"
        lbl_order.Content = "ORDER STATUS :"
        lbl_quant.Content = "ORDER QUANTITY :"
        lbl_supp.Content = "SUPPLIER NAME :"

        Try
            If Not combo_prodid.Text = Nothing Then
                Dim TempString(5) As String
                TempString = InventoryObject.GetLastOrderDetails(combo_prodid.Text)
                lbl_id.Content = "ORDER ID : " & TempString(0).ToUpper
                lbl_date.Content = "ORDER DATE : " & TempString(1).ToUpper
                lbl_quant.Content = "ORDER QUANTITY : " & TempString(2).ToUpper
                lbl_supp.Content = "SUPPLIER NAME : " & TempString(3).ToUpper
                lbl_order.Content = "ORDER STATUS : " & TempString(4).ToUpper
                If TempString(4) = "pending" Then
                    lbl_order.Foreground = Brushes.Red
                Else
                    lbl_order.Foreground = Brushes.White
                End If
            End If
        Catch ex As Exception
        End Try
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button2.Click
        Try
            If Text_id.Text = Nothing Or Text_cost.Text = Nothing Or Text_name.Text = Nothing Or Text_quant.Text = Nothing Or Text_thresh.Text = Nothing Or Combo_measured.Text = Nothing Then
                MsgBox("Please enter all details.", MsgBoxStyle.Information, MessageTitle)
            Else
                InventoryObject.AddNewRawProduct(Text_id.Text, Text_name.Text, Val(Text_quant.Text), Combo_measured.Text, Val(Text_cost.Text), Val(Text_thresh.Text))

                grid_list.ItemsSource = GetListofRawProducts.DefaultView
                LoadRawProductsList()
            End If
            ClearAll()
            Text_id.Focus()
        Catch ex As Exception
        End Try
    End Sub

    Private Sub grid_list_BeginningEdit(ByVal sender As Object, ByVal e As Microsoft.Windows.Controls.DataGridBeginningEditEventArgs) Handles grid_list.BeginningEdit
        Try
            Dim SelIndex As Int16 = grid_list.SelectedIndex
            If SelIndex >= 0 Then
                GetRawProdDetailsToEdit(SelIndex)
                Button_save.IsEnabled = True
                Button4.IsEnabled = True
                Text_id.Focus()
            End If
        Catch ex As Exception
        End Try
    End Sub

    Private Sub Button_save_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button_save.Click
        Try
            If Not Text_id.Text = Nothing Then
                InventoryObject.EditRawProduct(Text_id.Text, Text_name.Text, Val(Text_quant.Text), Combo_measured.Text, Val(Text_cost.Text), Val(Text_thresh.Text))
                ClearAll()
                MsgBox("Details Updated.", MsgBoxStyle.Information, MessageTitle)
                grid_list.ItemsSource = GetListofRawProducts.DefaultView
            Else
                MsgBox("Please enter the product id whose details must be updated.", MsgBoxStyle.Information, MessageTitle)
            End If
        Catch ex As Exception
        End Try
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button1.Click
        ClearAll()
    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button4.Click
        Try
            If Not Text_id.Text = Nothing Then
                DeleteRawProduct()
                ClearAll()
                grid_list.ItemsSource = GetListofRawProducts.DefaultView
                LoadRawProductsList()
                MsgBox("Product Deleted.", MsgBoxStyle.Information, MessageTitle)
            Else
                MsgBox("Please enter the product id whose details must be updated.", MsgBoxStyle.Information, MessageTitle)
            End If
        Catch ex As Exception
        End Try
    End Sub

#Region "NAVIGATION"
    Private Sub Text_id_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles Text_id.KeyDown
        If e.Key = Key.Enter Then
            Text_name.Focus()
        End If
    End Sub
    Private Sub Text_name_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles Text_name.KeyDown
        If e.Key = Key.Enter Then
            Text_quant.Focus()
        End If
    End Sub
    Private Sub Text_quant_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles Text_quant.KeyDown
        If e.Key = Key.Enter Then
            Combo_measured.Focus()
        End If
    End Sub

    Private Sub Combo_measured_SelectionChanged(ByVal sender As Object, ByVal e As System.Windows.Controls.SelectionChangedEventArgs) Handles Combo_measured.SelectionChanged
        Text_cost.Focus()
    End Sub

    Private Sub Text_cost_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles Text_cost.KeyDown
        If e.Key = Key.Enter Then
            Text_thresh.Focus()
        End If
    End Sub

    Private Sub Text_thresh_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles Text_thresh.KeyDown
        If e.Key = Key.Enter Then
            Button2.Focus()
        End If
    End Sub
#End Region

#Region "CONTROL DATABASE CODES"

    Public Function GetListofRawProducts() As DataTable
        Try
            Dim MyConn As New OleDb.OleDbConnection(ConnString)
            Dim ConnectionQuery As String

            ConnectionQuery = "select productname,quantityavailable,id from stock_stub where companyid='" & LoggedInCompanyName & "'"

            Dim MyCommand As New OleDb.OleDbCommand(ConnectionQuery, MyConn)
            Dim AReader As OleDb.OleDbDataReader
            Dim Table_List As New DataTable

            MyConn.Open()

            AReader = MyCommand.ExecuteReader
            Table_List.Load(AReader)

            Table_List.Columns(0).ColumnName = "Product Name"
            Table_List.Columns(1).ColumnName = "Quantity Available"

            Return Table_List
            MyConn.Close()
        Catch ex As Exception
            ErrorLogger.LogError(ex, "GetListOfRawProducts")
            MsgBox("Error Occured. Please check log for more details.", MsgBoxStyle.Information, MessageTitle)
            Return Nothing
        End Try
    End Function

    Private Sub GetRawProdDetailsToEdit(ByVal index As Int16)
        Try
            Dim MyConn As New OleDb.OleDbConnection(ConnString)
            Dim ConnectionQuery As String

            ConnectionQuery = "select productname,quantityavailable,quantityunittext,unitcost,thresholdlevel,id from stock_stub where id='" & grid_list.Items(index)(2) & "' and companyid='" & LoggedInCompanyName & "'"

            Dim MyCommand As New OleDb.OleDbCommand(ConnectionQuery, MyConn)
            Dim AReader As OleDb.OleDbDataReader
            Dim Table_List As New DataTable

            MyConn.Open()
            AReader = MyCommand.ExecuteReader

            While (AReader.Read)
                Text_id.Text = AReader(5)
                Text_name.Text = AReader(0)
                Text_quant.Text = AReader(1)
                Combo_measured.Text = AReader(2)
                Text_cost.Text = AReader(3)
                Text_thresh.Text = AReader(4)
            End While

        Catch ex As Exception
            ErrorLogger.LogError(ex, "GetProductsDetailsToEdit")
            MsgBox("Error Occured. Please check log for more details.", MsgBoxStyle.Information, MessageTitle)
        End Try
    End Sub

    Private Sub ClearAll()
        Text_id.Text = Nothing
        Text_cost.Text = Nothing
        Text_name.Text = Nothing
        Text_quant.Text = Nothing
        Text_thresh.Text = Nothing
        Combo_measured.Text = Nothing
        Text_id.Focus()
        Button_save.IsEnabled = False
        Button4.IsEnabled = False
    End Sub

    Private Sub DeleteRawProduct()
        Try
            Dim MyConn As New OleDb.OleDbConnection(ConnString)
            Dim ConnectionQuery As String

            ConnectionQuery = "delete from stock_stub where id='" & Text_id.Text & "' and companyid='" & LoggedInCompanyName & "'"

            Dim MyCommand As New OleDb.OleDbCommand(ConnectionQuery, MyConn)
          
            MyConn.Open()

            MyCommand.ExecuteReader()
           
            MyConn.Close()
        Catch ex As Exception
            ErrorLogger.LogError(ex, "DeleteRawProduct")
            MsgBox("Error Occured. Please check log for more details.", MsgBoxStyle.Information, MessageTitle)
        End Try
    End Sub

    Private Sub LoadRawProductsList()
        Try
            Dim MyConn As New OleDb.OleDbConnection(ConnString)
            Dim ConnectionQuery As String = "select productname from stock_stub where companyid ='" & LoggedInCompanyName & "'"
            Dim MyCommand As New OleDb.OleDbCommand(ConnectionQuery, MyConn)
            Dim AReader As OleDb.OleDbDataReader

            MyConn.Open()

            combo_prodid.Items.Clear()

            AReader = MyCommand.ExecuteReader

            While AReader.Read
                combo_prodid.Items.Add(AReader(0))
            End While

            AReader.Close()
            MyConn.Close()

        Catch ex As Exception
            ErrorLogger.LogError(ex, "LoadRawProductsList")
            MsgBox("Error Occured. Please check log for more details.", MsgBoxStyle.Information, MessageTitle)
        End Try
    End Sub

#End Region

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button3.Click
        Me.Close()
    End Sub
End Class

Imports System.Data

Partial Public Class Window_SupplierMgm

    Private Sub Window1_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Window1.Loaded
        Try
            grid_list.ItemsSource = GetListofSuppliers.DefaultView
            LoadSuppliersList()
        Catch ex As Exception
        End Try
    End Sub

    Private Sub grid_list_BeginningEdit(ByVal sender As Object, ByVal e As Microsoft.Windows.Controls.DataGridBeginningEditEventArgs) Handles grid_list.BeginningEdit
        Try
            Dim SelIndex As Int16 = grid_list.SelectedIndex
            If SelIndex >= 0 Then
                GetSupplierDetailsToEdit(SelIndex)
                Button_save.IsEnabled = True
                Button4.IsEnabled = True
                Text_id.Focus()
            End If
        Catch ex As Exception
        End Try
    End Sub

    Private Sub btn_show_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btn_show.Click
        lbl_date.Content = "LAST ORDER DATE : "
        lbl_quant.Content = "LAST ORDER VALUE : "
        lbl_supp.Content = "SUPPLIER NAME : "

        Try
            If Not combo_prodid.Text = Nothing Then
                Dim TempString(5) As String
                TempString = GetLastSupplyDetails(combo_prodid.Text)
                lbl_date.Content = "LAST ORDER DATE : " & TempString(0).ToUpper
                lbl_quant.Content = "LAST ORDER VALUE : " & TempString(1).ToUpper
                lbl_supp.Content = "SUPPLIER NAME : " & TempString(2).ToUpper
            End If
        Catch ex As Exception
        End Try
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button2.Click
        Try
            If Text_id.Text = Nothing Or Text_invoice.Text = Nothing Or Text_name.Text = Nothing Or Text_Phone.Text = Nothing Or Text_mail.Text = Nothing Or Text_addr.Text = Nothing Then
                MsgBox("Please enter all details.", MsgBoxStyle.Information, MessageTitle)
            Else
                InventoryObject.AddNewSupplier(Text_id.Text, Text_invoice.Text, Text_name.Text, Text_Phone.Text, Text_mail.Text, Text_addr.Text)
                grid_list.ItemsSource = GetListofSuppliers.DefaultView()
                LoadSuppliersList()
                ClearAll()
            End If

            Text_id.Focus()
        Catch ex As Exception
        End Try
    End Sub

    Private Sub Button_save_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button_save.Click
        Try
            If Not Text_id.Text = Nothing Then
                InventoryObject.EditSupplier(Text_id.Text, Text_invoice.Text, Text_name.Text, Text_Phone.Text, Text_mail.Text, Text_addr.Text)
                ClearAll()
                MsgBox("Details Updated.", MsgBoxStyle.Information, MessageTitle)
                grid_list.ItemsSource = GetListofSuppliers.DefaultView
            Else
                MsgBox("Please enter the Supplier id whose details must be updated.", MsgBoxStyle.Information, MessageTitle)
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
                DeleteSupplier()
                ClearAll()
                grid_list.ItemsSource = GetListofSuppliers.DefaultView
                LoadSuppliersList()
                MsgBox("Supplier Deleted.", MsgBoxStyle.Information, MessageTitle)
            Else
                MsgBox("Please enter the product id whose details must be updated.", MsgBoxStyle.Information, MessageTitle)
            End If
        Catch ex As Exception
        End Try
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button3.Click
        Me.Close()
    End Sub

#Region "CONTROL DATABASE CODES"

    Public Function GetListofSuppliers() As DataTable
        Try
            Dim MyConn As New OleDb.OleDbConnection(ConnString)
            Dim ConnectionQuery As String

            ConnectionQuery = "select suppliername, invoice,id from stock_supplier where companyid='" & LoggedInCompanyName & "'"

            Dim MyCommand As New OleDb.OleDbCommand(ConnectionQuery, MyConn)
            Dim AReader As OleDb.OleDbDataReader
            Dim Table_List As New DataTable

            MyConn.Open()

            AReader = MyCommand.ExecuteReader
            Table_List.Load(AReader)

            Table_List.Columns(0).ColumnName = "Supplier Name"
            Table_List.Columns(1).ColumnName = "Invoice"
            Table_List.Columns(2).ColumnName = "Supplier ID"

            Return Table_List
            MyConn.Close()
        Catch ex As Exception
            ErrorLogger.LogError(ex, "GetListOfSuppliers")
            MsgBox("Error Occured. Please check log for more details.", MsgBoxStyle.Information, MessageTitle)
            Return Nothing
        End Try
    End Function

    Private Sub GetSupplierDetailsToEdit(ByVal index As Int16)
        Try
            Dim MyConn As New OleDb.OleDbConnection(ConnString)
            Dim ConnectionQuery As String

            ConnectionQuery = "select id, invoice, suppliername, phonenum, email, address from stock_supplier where id='" & grid_list.Items(index)(2) & "' and companyid='" & LoggedInCompanyName & "'"

            Dim MyCommand As New OleDb.OleDbCommand(ConnectionQuery, MyConn)
            Dim AReader As OleDb.OleDbDataReader
            Dim Table_List As New DataTable

            MyConn.Open()
            AReader = MyCommand.ExecuteReader

            While (AReader.Read)
                Text_id.Text = AReader(0)
                Text_invoice.Text = AReader(1)
                Text_name.Text = AReader(2)
                Text_Phone.Text = AReader(3)
                Text_mail.Text = AReader(4)
                Text_addr.Text = AReader(5)
            End While

        Catch ex As Exception
            ErrorLogger.LogError(ex, "GetSupplierDetailsToEdit")
            MsgBox("Error Occured. Please check log for more details.", MsgBoxStyle.Information, MessageTitle)
        End Try
    End Sub

    Private Sub ClearAll()
        Text_id.Text = Nothing
        Text_invoice.Text = Nothing
        Text_name.Text = Nothing
        Text_Phone.Text = Nothing
        Text_mail.Text = Nothing
        Text_addr.Text = Nothing
        Text_id.Focus()
        Button_save.IsEnabled = False
        Button4.IsEnabled = False
    End Sub

    Private Sub DeleteSupplier()
        If MsgBox("Confirm Supplier Information delete ?", MsgBoxStyle.YesNo + MsgBoxStyle.Information, MessageTitle) = MsgBoxResult.Yes Then
            Try
                Dim MyConn As New OleDb.OleDbConnection(ConnString)
                Dim ConnectionQuery As String

                ConnectionQuery = "delete from stock_supplier where id='" & Text_id.Text & "' and companyid='" & LoggedInCompanyName & "'"

                Dim MyCommand As New OleDb.OleDbCommand(ConnectionQuery, MyConn)

                MyConn.Open()

                MyCommand.ExecuteReader()

                MyConn.Close()
            Catch ex As Exception
                ErrorLogger.LogError(ex, "DeleteSupplier")
                MsgBox("Error Occured. Please check log for more details.", MsgBoxStyle.Information, MessageTitle)
            End Try
        End If
    End Sub

    Private Sub LoadSuppliersList()
        Try
            Dim MyConn As New OleDb.OleDbConnection(ConnString)
            Dim ConnectionQuery As String = "select suppliername from stock_supplier where companyid ='" & LoggedInCompanyName & "'"
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
            ErrorLogger.LogError(ex, "LoadSuppliersList")
            MsgBox("Error Occured. Please check log for more details.", MsgBoxStyle.Information, MessageTitle)
        End Try
    End Sub

    Public Function GetLastSupplyDetails(ByVal suppliername As String) As String()
        Dim MyConn As New OleDb.OleDbConnection(ConnString)

        Try
            Dim ConnectionQuery As String
            Dim AReader As OleDb.OleDbDataReader
            Dim MyCommand As OleDb.OleDbCommand
            Dim TempString(5) As String
            Dim index As Integer = 0
            Dim sid As String = Nothing

            ConnectionQuery = "select id from stock_supplier where suppliername='" & suppliername & "' and companyid='" & LoggedInCompanyName & "'"

            MyConn.Open()

            MyCommand = New OleDb.OleDbCommand(ConnectionQuery, MyConn)
            AReader = MyCommand.ExecuteReader

            While AReader.Read
                sid = AReader(0)
            End While

            ConnectionQuery = "select lastorderdate, lastordervalue, suppliername from stock_supplier where id='" & sid & "' and companyid='" & LoggedInCompanyName & "'"

            MyCommand = New OleDb.OleDbCommand(ConnectionQuery, MyConn)
            AReader = MyCommand.ExecuteReader

            While AReader.Read
                For index1 As Integer = 0 To 2
                    TempString(index1) = Convert.ToString(AReader(index1))
                Next
            End While

            ConnectionQuery = "select orderid from stock_orderhistory where supplierid='" & sid & "' and pendingstatus='pending' group by orderid"
            MyCommand = New OleDb.OleDbCommand(ConnectionQuery, MyConn)
            AReader = MyCommand.ExecuteReader

            While AReader.Read
                list_pending.Items.Add(Convert.ToString(AReader(0)))
            End While

            Return TempString

        Catch ex As Exception
            ErrorLogger.LogError(ex, "GetLastSupplyDetails")
            MsgBox("Error Occured. Please check log for more details.", MsgBoxStyle.Information, MessageTitle)
            MyConn.Close()
            Return Nothing
        End Try
    End Function


#End Region

#Region "NAVIGATION CODES"
    Private Sub Text_id_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles Text_id.KeyDown
        If e.Key = Key.Enter And Not Text_id.Text = Nothing Then
            Text_invoice.Focus()
        End If
    End Sub

    Private Sub Text_invoice_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles Text_invoice.KeyDown
        If e.Key = Key.Enter And Not Text_invoice.Text = Nothing Then
            Text_name.Focus()
        End If
    End Sub

    Private Sub Text_name_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles Text_name.KeyDown
        If e.Key = Key.Enter And Not Text_name.Text = Nothing Then
            Text_Phone.Focus()
        End If
    End Sub

    Private Sub Text_Phone_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles Text_Phone.KeyDown
        If e.Key = Key.Enter Then
            Text_mail.Focus()
        End If
    End Sub

    Private Sub Text_mail_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles Text_mail.KeyDown
        If e.Key = Key.Enter Then
            Text_addr.Focus()
        End If
    End Sub

    Private Sub Text_addr_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles Text_addr.KeyDown
        If e.Key = Key.Enter Then
            Button2.Focus()
        End If
    End Sub
#End Region

    
End Class

Imports System.Data
Imports System.Data.OleDb

Partial Public Class Window_CustMgmt

    Private Sub Window1_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Window1.Loaded
        Try
            grid_list.ItemsSource = GetCustomerList.DefaultView
        Catch ex As Exception
        End Try
    End Sub

#Region "CONTROL DATABASE CODES"

    Public Function GetCustomerList() As DataTable
        Try
            Dim MyConn As New OleDb.OleDbConnection(ConnString)
            Dim ConnectionQuery As String

            ConnectionQuery = "select * from customer_stub where companyid='" & LoggedInCompanyName & "'"

            Dim MyCommand As New OleDb.OleDbCommand(ConnectionQuery, MyConn)
            Dim AReader As OleDb.OleDbDataReader
            Dim Table_List As New DataTable

            MyConn.Open()

            AReader = MyCommand.ExecuteReader
            Table_List.Load(AReader)

            Table_List.Columns(0).ColumnName = "ID"
            Table_List.Columns(1).ColumnName = "Product Name"
            Table_List.Columns(2).ColumnName = "DOB"
            Table_List.Columns(3).ColumnName = "Phone Number"
            Table_List.Columns(4).ColumnName = "Address"
            Table_List.Columns(5).ColumnName = "E-Mail"
            Table_List.Columns(6).ColumnName = "Region"
            Table_List.Columns(7).ColumnName = "Last Billed On"
            Table_List.Columns(8).ColumnName = "Total Visits"

            Return Table_List
            MyConn.Close()

        Catch ex As Exception
            ErrorLogger.LogError(ex, "GetCustomerList")
            MsgBox("Error Occured. Please check log for more details.", MsgBoxStyle.Information, MessageTitle)
            Return Nothing
        End Try
    End Function

    Private Sub GetCustDetailsToEdit(ByVal index As Int16)
        Try
            Dim MyConn As New OleDb.OleDbConnection(ConnString)
            Dim ConnectionQuery As String

            ConnectionQuery = "select * from customer_stub where id='" & grid_list.Items(index)(0) & "' and companyid='" & LoggedInCompanyName & "'"

            Dim MyCommand As New OleDb.OleDbCommand(ConnectionQuery, MyConn)
            Dim AReader As OleDb.OleDbDataReader
            Dim Table_List As New DataTable

            MyConn.Open()
            AReader = MyCommand.ExecuteReader

            While (AReader.Read)
                Text_id.Text = AReader(0)
                Text_name.Text = AReader(1)
                dtp_dob.SelectedDate = Convert.ToDateTime(AReader(2))
                Text_phone.Text = AReader(3)
                Text_addr.Text = AReader(4)
                Text_email.Text = AReader(5)
                Text_region.Text = AReader(6)
            End While

        Catch ex As Exception
            ErrorLogger.LogError(ex, "GetCustDetailsToEdit")
            MsgBox("Error Occured. Please check log for more details.", MsgBoxStyle.Information, MessageTitle)
        End Try
    End Sub

    Private Sub ClearAll()
        Text_id.Text = Nothing
        Text_name.Text = Nothing
        dtp_dob.SelectedDate = Today
        Text_phone.Text = Nothing
        Text_addr.Text = Nothing
        Text_email.Text = Nothing
        Text_region.Text = Nothing
        Text_id.Focus()
        Button_save.IsEnabled = False
        Button4.IsEnabled = False
    End Sub

    Private Sub DeleteCustomer()
        Try
            Dim MyConn As New OleDb.OleDbConnection(ConnString)
            Dim ConnectionQuery As String

            ConnectionQuery = "delete from customer_stub where id='" & Text_id.Text & "' and companyid='" & LoggedInCompanyName & "'"

            Dim MyCommand As New OleDb.OleDbCommand(ConnectionQuery, MyConn)

            MyConn.Open()

            MyCommand.ExecuteReader()

            MyConn.Close()
        Catch ex As Exception
            ErrorLogger.LogError(ex, "DeleteCustomer")
            MsgBox("Error Occured. Please check log for more details.", MsgBoxStyle.Information, MessageTitle)
        End Try
    End Sub

#End Region

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button2.Click
        Try
            If Text_id.Text = Nothing Or Text_name.Text = Nothing Or Text_phone.Text = Nothing Or Text_addr.Text = Nothing Or Text_email.Text = Nothing Or Text_region.Text = Nothing Then
                MsgBox("Please enter all details.", MsgBoxStyle.Information, MessageTitle)
            Else
                CustomerObject.AddNewCustomer(Text_id.Text, Text_name.Text, dtp_dob.SelectedDate, Text_phone.Text, Text_addr.Text, Text_email.Text, Text_region.Text)
                grid_list.ItemsSource = GetCustomerList.DefaultView
            End If

            ClearAll()
            Text_id.Focus()
        Catch ex As Exception
        End Try
    End Sub

    Private Sub Button_save_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button_save.Click
        Try
            If Not Text_id.Text = Nothing Then
                CustomerObject.EditCustomer(Text_id.Text, Text_name.Text, dtp_dob.SelectedDate, Text_phone.Text, Text_addr.Text, Text_email.Text, Text_region.Text)
                ClearAll()
                MsgBox("Details Updated.", MsgBoxStyle.Information, MessageTitle)
                grid_list.ItemsSource = GetCustomerList.DefaultView
            Else
                MsgBox("Please enter the Customer ID whose details must be updated.", MsgBoxStyle.Information, MessageTitle)
            End If
        Catch ex As Exception
        End Try
    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button4.Click
        Try
            If Not Text_id.Text = Nothing Then
                DeleteCustomer()
                ClearAll()
                grid_list.ItemsSource = GetCustomerList.DefaultView
                MsgBox("Customer Deleted.", MsgBoxStyle.Information, MessageTitle)
            Else
                MsgBox("Please enter the Customer ID whose details must be updated.", MsgBoxStyle.Information, MessageTitle)
            End If
        Catch ex As Exception
        End Try
    End Sub

    Private Sub grid_list_BeginningEdit(ByVal sender As Object, ByVal e As Microsoft.Windows.Controls.DataGridBeginningEditEventArgs) Handles grid_list.BeginningEdit
        Try
            Dim SelIndex As Int16 = grid_list.SelectedIndex
            If SelIndex >= 0 Then
                GetCustDetailsToEdit(SelIndex)
                Button_save.IsEnabled = True
                Button4.IsEnabled = True
                Text_id.Focus()
            End If
        Catch ex As Exception
        End Try
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button1.Click
        ClearAll()
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button3.Click
        Me.Close()
    End Sub
End Class

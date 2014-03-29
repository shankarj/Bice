Imports System.Data

Partial Public Class Window_PayReport

    Private Sub Window1_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        LoadEmployees()
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button1.Click
        If Not ComboBox1.Text = Nothing Then
             grid_prod.ItemsSource = LoadDetails.DefaultView
        End If
    End Sub

#Region "DATABASE CODES"
    Private Sub LoadEmployees()
        Try
            Dim MyConn As New OleDb.OleDbConnection(ConnString)
            Dim ConnectionQuery As String

            ConnectionQuery = "select ename from emp_details where companyid='" & LoggedInCompanyName & "'"

            Dim MyCommand As New OleDb.OleDbCommand(ConnectionQuery, MyConn)
            Dim AReader As OleDb.OleDbDataReader

            MyConn.Open()

            AReader = MyCommand.ExecuteReader

            While AReader.Read
                ComboBox1.Items.Add(Convert.ToString(AReader(0)))
            End While


            MyConn.Close()
        Catch ex As Exception
            ErrorLogger.LogError(ex, "LoadEmployees")
            MsgBox("Error Occured. Please check log for more details.", MsgBoxStyle.Information, MessageTitle)
        End Try
    End Sub

    Private Function LoadDetails() As DataTable
        Try
            Dim MyConn As New OleDb.OleDbConnection(ConnString)
            Dim ConnectionQuery As String
            Dim Table_List As New DataTable

            ConnectionQuery = "select paydate, payamount, daypayamount, DA, TA, Extra from emp_payhistory where ename='" & ComboBox1.Text & "' and companyid='" & LoggedInCompanyName & "'"

            Dim MyCommand As New OleDb.OleDbCommand(ConnectionQuery, MyConn)
            Dim AReader As OleDb.OleDbDataReader

            MyConn.Open()

            AReader = MyCommand.ExecuteReader
            Table_List.Load(AReader)

            Table_List.Columns(0).ColumnName = "DATE PAID"
            Table_List.Columns(1).ColumnName = "TOTAL PAID"
            Table_List.Columns(2).ColumnName = "DAY PAY ALLOTED"
            Table_List.Columns(3).ColumnName = "DA"
            Table_List.Columns(4).ColumnName = "TA"
            Table_List.Columns(5).ColumnName = "Extra"

            MyConn.Close()

            Return Table_List
       
        Catch ex As Exception
            ErrorLogger.LogError(ex, "LoadPayReportDetails")
            MsgBox("Error Occured. Please check log for more details.", MsgBoxStyle.Information, MessageTitle)
            Return Nothing
        End Try
    End Function
#End Region

End Class

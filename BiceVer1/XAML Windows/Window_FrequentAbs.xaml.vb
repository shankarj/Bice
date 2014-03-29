Imports System.Data
Imports System.Data.OleDb

Partial Public Class Window_FrequentAbs

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button3.Click
        Me.Close()
    End Sub

    Private Sub Window1_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        LoadEmployees()
    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button4.Click
        If Not TextBox1.Text = Nothing And HasNumber(TextBox1.Text) Then
            grid_prod.ItemsSource = LoadMinAbs.DefaultView
        End If
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button1.Click
        If Not ComboBox1.Text = Nothing Then
            grid_prod.ItemsSource = LoadEmpAbs.DefaultView
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

    Private Function LoadMinAbs() As DataTable
        Try
            Dim MyConn As New OleDb.OleDbConnection(ConnString)
            Dim ConnectionQuery As String
            Dim Table_List As New DataTable

            ConnectionQuery = "select * from (select uid, count(presence) as sm from emp_history where todaydate between #" & dtp_from.SelectedDate & "# and #" & dtp_to.SelectedDate & "# and presence=0 and companyid='" & LoggedInCompanyName & "' group by uid) where sm > " & Val(TextBox1.Text)

            Dim MyCommand As New OleDb.OleDbCommand(ConnectionQuery, MyConn)
            Dim AReader As OleDb.OleDbDataReader

            MyConn.Open()

            AReader = MyCommand.ExecuteReader
            Table_List.Load(AReader)

            Table_List.Columns(0).ColumnName = "EMPLOYEE"
            Table_List.Columns(0).ColumnName = "DAYS ABSENT"

            MyConn.Close()

            Return Table_List

        Catch ex As Exception
            ErrorLogger.LogError(ex, "LoadMinAbs")
            MsgBox("Error Occured. Please check log for more details.", MsgBoxStyle.Information, MessageTitle)
            Return Nothing
        End Try
    End Function

    Private Function LoadEmpAbs() As DataTable
        Try
            Dim MyConn As New OleDb.OleDbConnection(ConnString)
            Dim ConnectionQuery As String
            Dim Table_List As New DataTable

            ConnectionQuery = "select todaydate from emp_history where uid='" & ComboBox1.Text & "' and todaydate between #" & dtp_from.SelectedDate & "# and #" & dtp_to.SelectedDate & "# and presence=0 and companyid='" & LoggedInCompanyName & "'"

            Dim MyCommand As New OleDb.OleDbCommand(ConnectionQuery, MyConn)
            Dim AReader As OleDb.OleDbDataReader

            MyConn.Open()

            AReader = MyCommand.ExecuteReader
            Table_List.Load(AReader)

            MyConn.Close()

            Table_List.Columns(0).ColumnName = "DATES ABSENT"

            Return Table_List

        Catch ex As Exception
            ErrorLogger.LogError(ex, "LoadEmpAbs")
            MsgBox("Error Occured. Please check log for more details.", MsgBoxStyle.Information, MessageTitle)
            Return Nothing
        End Try
    End Function
#End Region
End Class


Imports System.Data
Imports System.Data.OleDb

Partial Public Class Window_CalculatePay

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button1.Click
        Me.Close()
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button2.Click
        LoadDetails()
        grid_order.ItemsSource = LoadDetails.DefaultView
        LoadDaysPresent()
        RefreshGrid()
    End Sub

    Private Sub Button_finalize_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button_finalize.Click

        RefreshGrid()

        If MsgBox("Finalize the pay details ?", MsgBoxStyle.Information + MsgBoxStyle.YesNo, MessageTitle) = MsgBoxResult.Yes Then
            FinalizePay()
        End If

    End Sub

    Private Sub grid_order_SelectionChanged(ByVal sender As Object, ByVal e As System.Windows.Controls.SelectionChangedEventArgs) Handles grid_order.SelectionChanged
        RefreshGrid()
    End Sub

#Region "DATABASE CODES"

    Private Function LoadDetails() As DataTable
        Try
            Dim MyConn As New OleDb.OleDbConnection(ConnString)
            Dim ConnectionQuery As String
            Dim Table_List As New DataTable

            ConnectionQuery = "select ename, daypayamount, da, ta, extra from emp_details where companyid='" & LoggedInCompanyName & "'"

            Dim MyCommand As New OleDb.OleDbCommand(ConnectionQuery, MyConn)
            Dim AReader As OleDb.OleDbDataReader

            MyConn.Open()

            AReader = MyCommand.ExecuteReader
            Table_List.Load(AReader)

            Table_List.Columns(0).ColumnName = "Employee"
            Table_List.Columns(1).ColumnName = "Day Pay"
            Table_List.Columns(2).ColumnName = "DA"
            Table_List.Columns(3).ColumnName = "TA"
            Table_List.Columns(4).ColumnName = "Extra"
            Table_List.Columns.Add("DAYS PRESENT")
            Table_List.Columns.Add("FINAL PAY")

            MyConn.Close()

            Return Table_List

        Catch ex As Exception
            ErrorLogger.LogError(ex, "LoadDetails")
            MsgBox("Error Occured. Please check log for more details.", MsgBoxStyle.Information, MessageTitle)
            Return Nothing
        End Try
    End Function

    Private Sub LoadDaysPresent()
        Try
            Dim MyConn As New OleDb.OleDbConnection(ConnString)
            Dim ConnectionQuery As String
            Dim Table_List As New DataTable
            Dim MyCommand As OleDb.OleDbCommand
            Dim AReader As OleDb.OleDbDataReader = Nothing
            Dim DaysPres As Int64 = 0

            MyConn.Open()

            For index As Integer = 0 To grid_order.Items.Count - 2
                ConnectionQuery = "select presence from emp_history where todaydate between #" & dtp_from.SelectedDate & "# and #" & dtp_to.SelectedDate & "# and uid='" & grid_order.Items(index)(0) & "' and companyid='" & LoggedInCompanyName & "'"
                MyCommand = New OleDb.OleDbCommand(ConnectionQuery, MyConn)
                AReader = MyCommand.ExecuteReader()
                DaysPres = 0

                While AReader.Read
                    If Convert.ToBoolean(AReader(0)) = True Then
                        DaysPres += 1
                    End If
                End While

                grid_order.Items(index)(5) = DaysPres
            Next

            MyConn.Close()
        Catch ex As Exception
            ErrorLogger.LogError(ex, "LoadDaysPresent")
            MsgBox("Error Occured. Please check log for more details.", MsgBoxStyle.Information, MessageTitle)
        End Try
    End Sub

    Private Sub FinalizePay()
        Try
            Dim MyConn As New OleDb.OleDbConnection(ConnString)
            Dim ConnectionQuery As String
            Dim MyCommand As OleDb.OleDbCommand
            MyConn.Open()

            Dim date1 As Date = Convert.ToDateTime(dtp_from.SelectedDate)
            Dim date2 As Date = Convert.ToDateTime(dtp_to.SelectedDate)
            Dim TempDate As Date = Today

            For index As Integer = 0 To grid_order.Items.Count - 2
                ConnectionQuery = "insert into emp_payhistory values ('" & grid_order.Items(index)(0) & "', #" & TempDate & "#," & grid_order.Items(index)(6) & "," & grid_order.Items(index)(1) & "," & _
                                  grid_order.Items(index)(2) & "," & grid_order.Items(index)(3) & "," & grid_order.Items(index)(4) & _
                                  ",'" & LoggedInCompanyName & "')"
                MyCommand = New OleDb.OleDbCommand(ConnectionQuery, MyConn)
                MyCommand.ExecuteReader()
            Next

            MsgBox("Pay Details uploaded.", MsgBoxStyle.Information, MessageTitle)

            MyConn.Close()
            Me.Close()
        Catch ex As OleDb.OleDbException
            ErrorLogger.LogError(ex, "LoadPayReportDetails")
            MsgBox("Pay Details for the specified dates were already finalized.", MsgBoxStyle.Information, MessageTitle)
        Catch ex As Exception
            ErrorLogger.LogError(ex, "FinalizePay")
            MsgBox("Error Occured. Please check log for more details.", MsgBoxStyle.Information, MessageTitle)
        End Try
    End Sub

    Private Sub RefreshGrid()
        Try
            For index As Integer = 0 To grid_order.Items.Count - 1
                Try
                    grid_order.Items(index)(6) = (grid_order.Items(index)(1) + grid_order.Items(index)(2) + grid_order.Items(index)(3) + grid_order.Items(index)(4)) * Val(grid_order.Items(index)(5))
                Catch ex As Exception
                End Try
            Next
        Catch ex As Exception

        End Try
    End Sub

#End Region
End Class

Imports System.Data
Imports System.Data.OleDb
Imports System.Windows.Controls

Partial Public Class Window_Attendance

    Dim TodayAttendance As New DataTable
    Dim BetweenAttendance As New DataTable

    Private Sub Window1_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Window1.Loaded
        LoadEmployees()
        LoadTodaySheet()
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button3.Click
        If MsgBox("Finalize today's attendance ?", MsgBoxStyle.Information + MsgBoxStyle.YesNo, MessageTitle) = MsgBoxResult.Yes Then
            UpdateTodayAttendance()
            TodayAttendance.Rows.Clear()
            DataGrid1.IsEnabled = False
        End If
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button1.Click
        GenerateSheet()
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button2.Click
        If MsgBox("Finalize the above attendance sheet ?", MsgBoxStyle.Information + MsgBoxStyle.YesNo, MessageTitle) = MsgBoxResult.Yes Then
            If Not ComboBox1.Text = Nothing Then
                UpdateBetweenAttendance()
                BetweenAttendance.Rows.Clear()
            Else
                MsgBox("Please choose an Employee.", MsgBoxStyle.Information, MessageTitle)
            End If
        End If
    End Sub

#Region "DATABASE CODES"

    Private Sub LoadTodaySheet()
        Try
            Dim MyConn As New OleDb.OleDbConnection(ConnString)
            Dim ConnectionQuery As String

            ConnectionQuery = "select max(lastattendance) from emp_details where companyid='" & LoggedInCompanyName & "'"

            Dim MyCommand As New OleDb.OleDbCommand(ConnectionQuery, MyConn)
            Dim AReader As OleDb.OleDbDataReader

            MyConn.Open()

            AReader = MyCommand.ExecuteReader

            While AReader.Read
                Dim Temp As Date = AReader(0)
                If Temp = Today Then
                    Label3.Content = "ATTENDANCE FOR TODAY (FINALIZED)"
                    DataGrid1.IsEnabled = False
                    Exit Sub
                End If
            End While

            TodayAttendance.Columns.Clear()
            TodayAttendance.Columns.Add("Employee Name", GetType(String))
            TodayAttendance.Columns.Add("Present", GetType(Boolean))

            ConnectionQuery = "select ename from emp_details where companyid='" & LoggedInCompanyName & "'"

            MyCommand = New OleDb.OleDbCommand(ConnectionQuery, MyConn)
            AReader = MyCommand.ExecuteReader

            While AReader.Read
                TodayAttendance.Rows.Add(Convert.ToString(AReader(0)), 0)
            End While

            DataGrid1.ItemsSource = TodayAttendance.DefaultView

            MyConn.Close()
        Catch ex As Exception
            ErrorLogger.LogError(ex, "LoadTodaySheet")
            MsgBox("Error Occured. Please check log for more details.", MsgBoxStyle.Information, MessageTitle)
        End Try
    End Sub

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

    Private Sub UpdateTodayAttendance()
        Try
            Dim MyConn As New OleDb.OleDbConnection(ConnString)
            Dim ConnectionQuery As String
            Dim MyCommand As OleDb.OleDbCommand

            MyConn.Open()

            For index As Integer = 0 To DataGrid1.Items.Count - 2
                ConnectionQuery = "update emp_details set lastattendance=#" & Date_Today & "# where ename='" & DataGrid1.Items(index)(0) & "' and companyid='" & LoggedInCompanyName & "'"
                MyCommand = New OleDb.OleDbCommand(ConnectionQuery, MyConn)
                MyCommand.ExecuteReader()

                ConnectionQuery = "insert into emp_history values ('" & DataGrid1.Items(index)(0) & "', #" & Date_Today & "#, " & DataGrid1.Items(index)(1) & ", '" & LoggedInCompanyName & "')"
                MyCommand = New OleDb.OleDbCommand(ConnectionQuery, MyConn)
                MyCommand.ExecuteReader()
            Next


            MyConn.Close()

            MsgBox("Attendance updated.", MsgBoxStyle.Information, MessageTitle)
        Catch ex As Exception
            ErrorLogger.LogError(ex, "UpdateTodayAttendance")
            MsgBox("Error Occured. Please check log for more details.", MsgBoxStyle.Information, MessageTitle)
        End Try
    End Sub

    Private Sub UpdateBetweenAttendance()
        Try
            Dim MyConn As New OleDb.OleDbConnection(ConnString)
            Dim ConnectionQuery As String
            Dim MyCommand As OleDb.OleDbCommand

            MyConn.Open()

            For index As Integer = 0 To grid_bet.Items.Count - 2
                ConnectionQuery = "insert into emp_history values ('" & ComboBox1.Text & "', #" & grid_bet.Items(index)(0) & "#, " & grid_bet.Items(index)(1) & ", '" & LoggedInCompanyName & "')"
                MyCommand = New OleDb.OleDbCommand(ConnectionQuery, MyConn)
                MyCommand.ExecuteReader()
            Next

            MyConn.Close()

            MsgBox("Attendance updated.", MsgBoxStyle.Information, MessageTitle)
        Catch ex As OleDbException
            MsgBox("Attendance details for the specified Employee between the specified dates are already uploaded. Please check absent details for confirmation.", MsgBoxStyle.Information, MessageTitle)
        Catch ex As Exception
            ErrorLogger.LogError(ex, "UpdateBetweenAttendance")
            MsgBox("Error Occured. Please check log for more details.", MsgBoxStyle.Information, MessageTitle)
        End Try
    End Sub

    Private Sub GenerateSheet()
        Try
            BetweenAttendance.Columns.Clear()

            BetweenAttendance.Columns.Add("On Date", GetType(String))
            BetweenAttendance.Columns.Add("Present", GetType(Boolean))

            Dim Diff As Long = DateDiff(DateInterval.Day, Convert.ToDateTime(dtp_on.SelectedDate), Convert.ToDateTime(dtp_to.SelectedDate), FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1)

            Dim date1 As Date = Convert.ToDateTime(dtp_on.SelectedDate)
            Dim date2 As Date = Convert.ToDateTime(dtp_to.SelectedDate)
            Dim TempDate As Date = date1

            While TempDate <= date2
                BetweenAttendance.Rows.Add(Convert.ToString(TempDate), 0)
                TempDate = TempDate.AddDays(1)
            End While

            grid_bet.ItemsSource = BetweenAttendance.DefaultView
        Catch ex As Exception

        End Try
    End Sub


#End Region

End Class

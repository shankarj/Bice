Imports System.Data
Imports System.Data.OleDb

Partial Public Class Window_EmpMgmt

    Private Sub Window1_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Window1.Loaded
        Try
            grid_list.ItemsSource = GetListofEmp.DefaultView
        Catch ex As Exception
        End Try
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button2.Click
        Try
            If Text_name.Text = Nothing Then
                MsgBox("Please enter atleast Employee's name to proceed.", MsgBoxStyle.Information, MessageTitle)
            Else
                EmployeeObject.AddNewEmployee(Text_name.Text, Val(Text_daypay.Text), Val(Text_da.Text), Val(Text_ta.Text), Val(Text_extra.Text))
                grid_list.ItemsSource = GetListofEmp.DefaultView
            End If
            ClearAll()
            Text_name.Focus()
        Catch ex As Exception
        End Try
    End Sub

    Private Sub grid_list_BeginningEdit(ByVal sender As Object, ByVal e As Microsoft.Windows.Controls.DataGridBeginningEditEventArgs) Handles grid_list.BeginningEdit
        Try
            Dim SelIndex As Int16 = grid_list.SelectedIndex
            If SelIndex >= 0 Then
                GetEmpDetailsToEdit(SelIndex)
                Button_save.IsEnabled = True
                Button4.IsEnabled = True
                Text_name.Focus()
            End If
        Catch ex As Exception
        End Try
    End Sub

    Private Sub Button_save_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button_save.Click
        Try
            If Not Text_name.Text = Nothing Then
                EmployeeObject.EditEmployee(Text_name.Text, Val(Text_daypay.Text), Val(Text_da.Text), Val(Text_ta.Text), Val(Text_extra.Text))
                ClearAll()
                MsgBox("Details Updated.", MsgBoxStyle.Information, MessageTitle)
                grid_list.ItemsSource = GetListofEmp.DefaultView
            Else
                MsgBox("Please enter the employee id whose details must be updated.", MsgBoxStyle.Information, MessageTitle)
            End If
        Catch ex As Exception
        End Try
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button1.Click
        ClearAll()
    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button4.Click
        Try
            If Not Text_name.Text = Nothing Then
                DeleteEmp()
                ClearAll()
                grid_list.ItemsSource = GetListofEmp.DefaultView
                MsgBox("Employee Deleted.", MsgBoxStyle.Information, MessageTitle)
            Else
                MsgBox("Please enter the Employee Name whose details must be updated.", MsgBoxStyle.Information, MessageTitle)
            End If
        Catch ex As Exception
        End Try
    End Sub

#Region "CONTROL DATABASE CODES"

    Public Function GetListofEmp() As DataTable
        Try
            Dim MyConn As New OleDb.OleDbConnection(ConnString)
            Dim ConnectionQuery As String

            ConnectionQuery = "select * from emp_details where companyid='" & LoggedInCompanyName & "'"

            Dim MyCommand As New OleDb.OleDbCommand(ConnectionQuery, MyConn)
            Dim AReader As OleDb.OleDbDataReader
            Dim Table_List As New DataTable

            MyConn.Open()

            AReader = MyCommand.ExecuteReader
            Table_List.Load(AReader)

            Table_List.Columns(0).ColumnName = "Employee Name"
            Table_List.Columns(1).ColumnName = "Day Pay"
            Table_List.Columns(2).ColumnName = "DA"
            Table_List.Columns(3).ColumnName = "TA"
            Table_List.Columns(4).ColumnName = "Extra"
            Table_List.Columns(5).ColumnName = "Last Pay Date"
            Table_List.Columns(6).ColumnName = "Last Attendance Entry"
            Table_List.Columns(7).ColumnName = "Last Pay Slip ID"
            Table_List.Columns(8).ColumnName = "In Company"

            Return Table_List
            MyConn.Close()
        Catch ex As Exception
            ErrorLogger.LogError(ex, "GetListOfEmp")
            MsgBox("Error Occured. Please check log for more details.", MsgBoxStyle.Information, MessageTitle)
            Return Nothing
        End Try
    End Function

    Private Sub GetEmpDetailsToEdit(ByVal index As Int16)
        Try
            Dim MyConn As New OleDb.OleDbConnection(ConnString)
            Dim ConnectionQuery As String

            ConnectionQuery = "select * from emp_details where ename='" & grid_list.Items(index)(0) & "' and companyid='" & LoggedInCompanyName & "'"

            Dim MyCommand As New OleDb.OleDbCommand(ConnectionQuery, MyConn)
            Dim AReader As OleDb.OleDbDataReader
            Dim Table_List As New DataTable

            MyConn.Open()
            AReader = MyCommand.ExecuteReader

            While (AReader.Read)
                Text_name.Text = AReader(0)
                Text_daypay.Text = AReader(1)
                Text_da.Text = AReader(2)
                Text_ta.Text = AReader(3)
                Text_extra.Text = AReader(4)
            End While

        Catch ex As Exception
            ErrorLogger.LogError(ex, "GetEmpDetailsToEdit")
            MsgBox("Error Occured. Please check log for more details.", MsgBoxStyle.Information, MessageTitle)
        End Try
    End Sub

    Private Sub ClearAll()
        Text_name.Text = Nothing
        Text_daypay.Text = Nothing
        Text_da.Text = Nothing
        Text_ta.Text = Nothing
        Text_extra.Text = Nothing
        Text_name.Focus()
        Button_save.IsEnabled = False
        Button4.IsEnabled = False
    End Sub

    Private Sub DeleteEmp()
        Try
            Dim MyConn As New OleDb.OleDbConnection(ConnString)
            Dim ConnectionQuery As String

            ConnectionQuery = "delete from emp_details where ename='" & Text_name.Text & "' and companyid='" & LoggedInCompanyName & "'"

            Dim MyCommand As New OleDb.OleDbCommand(ConnectionQuery, MyConn)

            MyConn.Open()

            MyCommand.ExecuteReader()

            MyConn.Close()
        Catch ex As Exception
            ErrorLogger.LogError(ex, "DeleteEmp")
            MsgBox("Error Occured. Please check log for more details.", MsgBoxStyle.Information, MessageTitle)
        End Try
    End Sub

#End Region

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button3.Click
        Me.Close()
    End Sub
End Class

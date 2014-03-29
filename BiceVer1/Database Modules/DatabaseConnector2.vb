Imports System.Data

Public Class DatabaseConnector2

#Region "DECLARATIONS"
    Dim PendingTotal As Integer

#End Region

  
    Public Function GetReportDetails(ByVal TheDate As String) As String()
        Try
            Dim rdr1, rdr2 As OleDb.OleDbDataReader
            Dim str1, str2 As String
            Dim TheArray(6) As String
            Dim Con As New OleDb.OleDbConnection(ConnString)
            Con.Open()

            Try
                ReportTable1.Rows.Clear()
                ReportTable2.Rows.Clear()
            Catch ex As Exception
                'JUST A NULL EXCEPTION RAISED AND MAY BE CAUGHT'
            End Try

            str1 = "Select id, productname from menu_products where companyid='" & LoggedInCompanyName & "'"
            Dim cmd1 As New OleDb.OleDbCommand(str1, Con)

            rdr1 = cmd1.ExecuteReader

            While (rdr1.Read())

                Dim sum As Integer = 0

                str2 = "select quantity from bill_detail where productid='" & rdr1(0) & "' and billdate= #" & TheDate & "# and companyid='" & LoggedInCompanyName & "'"

                Dim cmd2 As New OleDb.OleDbCommand(str2, Con)
                rdr2 = cmd2.ExecuteReader

                While (rdr2.Read())
                    sum += rdr2(0)
                End While

                If Not sum = 0 Then
                    ReportTable1.Rows.Add(rdr1(1), sum)
                End If

            End While


            str1 = "Select billno, billtype, billvalue from all_entries where billdate= #" & TheDate & "# and companyid='" & LoggedInCompanyName & "'"
            cmd1 = New OleDb.OleDbCommand(str1, Con)
            rdr1 = cmd1.ExecuteReader

            While (rdr1.Read())
                ReportTable2.Rows.Add(rdr1(0), rdr1(1), rdr1(2))
            End While


            str1 = "Select count(billno) from all_entries where billtype='CASH' and billdate= #" & TheDate & "# and companyid='" & LoggedInCompanyName & "'"
            cmd1 = New OleDb.OleDbCommand(str1, Con)
            rdr1 = cmd1.ExecuteReader

            While (rdr1.Read())
                TheArray(0) = Convert.ToString(rdr1(0))
            End While


            str1 = "Select count(billno) from all_entries where billtype='CREDIT' and billdate= #" & TheDate & "# and companyid='" & LoggedInCompanyName & "'"
            cmd1 = New OleDb.OleDbCommand(str1, Con)
            rdr1 = cmd1.ExecuteReader

            While (rdr1.Read())
                TheArray(1) = Convert.ToString(rdr1(0))
            End While

            str1 = "Select sum(billvalue) from all_entries where billtype='CASH' and billdate= #" & TheDate & "# and companyid='" & LoggedInCompanyName & "'"
            cmd1 = New OleDb.OleDbCommand(str1, Con)
            rdr1 = cmd1.ExecuteReader

            While (rdr1.Read())
                TheArray(2) = Convert.ToString(rdr1(0))
            End While

            str1 = "Select sum(billvalue) from all_entries where billdate= #" & TheDate & "# and companyid='" & LoggedInCompanyName & "'"
            cmd1 = New OleDb.OleDbCommand(str1, Con)
            rdr1 = cmd1.ExecuteReader

            While (rdr1.Read())
                TheArray(3) = Convert.ToString(rdr1(0))
            End While

            str1 = "Select max(billno) from all_entries where billdate= #" & TheDate & "# and companyid='" & LoggedInCompanyName & "'"
            cmd1 = New OleDb.OleDbCommand(str1, Con)
            rdr1 = cmd1.ExecuteReader

            While (rdr1.Read())
                TheArray(4) = Convert.ToString(rdr1(0))
            End While

            Con.Close()

            Return TheArray
        Catch ex As Exception
            ErrorLogger.LogError(ex, "GetReportDetails")
            MsgBox("Error Occured. Please check log for more details.", MsgBoxStyle.Information, MessageTitle)
            Return Nothing
        End Try
    End Function

    Public Function GetReportDetailsBetweenDates(ByVal FromDate As String, ByVal ToDate As String) As String()
        Try
            Dim rdr1, rdr2 As OleDb.OleDbDataReader
            Dim str1, str2 As String
            Dim TheArray(6) As String
            Dim Con As New OleDb.OleDbConnection(ConnString)
            Con.Open()

            Try
                ReportTable1.Rows.Clear()
                ReportTable2.Rows.Clear()
            Catch ex As Exception
                'JUST A NULL EXCEPTION RAISED AND MAY BE CAUGHT'
            End Try

            str1 = "Select id, productname from menu_products where companyid='" & LoggedInCompanyName & "'"
            Dim cmd1 As New OleDb.OleDbCommand(str1, Con)

            rdr1 = cmd1.ExecuteReader

            While (rdr1.Read())

                Dim sum As Integer = 0

                str2 = "select quantity from bill_detail where productid='" & rdr1(0) & "' and billdate between #" & FromDate & "# and #" & ToDate & "# and companyid='" & LoggedInCompanyName & "'"

                Dim cmd2 As New OleDb.OleDbCommand(str2, Con)
                rdr2 = cmd2.ExecuteReader

                While (rdr2.Read())
                    sum += rdr2(0)
                End While

                If Not sum = 0 Then
                    ReportTable1.Rows.Add(rdr1(1), sum)
                End If

            End While


            str1 = "Select billno, billtype, billvalue from all_entries where billdate between #" & FromDate & "# and #" & ToDate & "# and companyid='" & LoggedInCompanyName & "'"
            cmd1 = New OleDb.OleDbCommand(str1, Con)
            rdr1 = cmd1.ExecuteReader

            While (rdr1.Read())
                ReportTable2.Rows.Add(rdr1(0), rdr1(1), rdr1(2))
            End While


            str1 = "Select count(billno) from all_entries where billtype='CASH' and billdate between #" & FromDate & "# and #" & ToDate & "# and companyid='" & LoggedInCompanyName & "'"
            cmd1 = New OleDb.OleDbCommand(str1, Con)
            rdr1 = cmd1.ExecuteReader

            While (rdr1.Read())
                TheArray(0) = Convert.ToString(rdr1(0))
            End While


            str1 = "Select count(billno) from all_entries where billtype='CREDIT' and billdate between #" & FromDate & "# and #" & ToDate & "# and companyid='" & LoggedInCompanyName & "'"
            cmd1 = New OleDb.OleDbCommand(str1, Con)
            rdr1 = cmd1.ExecuteReader

            While (rdr1.Read())
                TheArray(1) = Convert.ToString(rdr1(0))
            End While

            str1 = "Select sum(billvalue) from all_entries where billtype='CASH' and billdate between #" & FromDate & "# and #" & ToDate & "# and companyid='" & LoggedInCompanyName & "'"
            cmd1 = New OleDb.OleDbCommand(str1, Con)
            rdr1 = cmd1.ExecuteReader

            While (rdr1.Read())
                TheArray(2) = Convert.ToString(rdr1(0))
            End While

            str1 = "Select sum(billvalue) from all_entries where billdate between #" & FromDate & "# and #" & ToDate & "# and companyid='" & LoggedInCompanyName & "'"
            cmd1 = New OleDb.OleDbCommand(str1, Con)
            rdr1 = cmd1.ExecuteReader

            While (rdr1.Read())
                TheArray(3) = Convert.ToString(rdr1(0))
            End While

            str1 = "Select max(billno) from all_entries where billdate between #" & FromDate & "# and #" & ToDate & "# and companyid='" & LoggedInCompanyName & "'"
            cmd1 = New OleDb.OleDbCommand(str1, Con)
            rdr1 = cmd1.ExecuteReader

            While (rdr1.Read())
                TheArray(4) = Convert.ToString(rdr1(0))
            End While

            Con.Close()

            Return TheArray
        Catch ex As Exception
            ErrorLogger.LogError(ex, "GetReportDetailsBetDates")
            MsgBox("Error Occured. Please check log for more details.", MsgBoxStyle.Information, MessageTitle)
            Return Nothing
        End Try
    End Function

    Private Function GetCustomerName(ByVal CustomerId As String) As String
        Try
            Dim MyConn As New OleDb.OleDbConnection(ConnString)
            Dim ConnectionQuery As String
            Dim ReturnString As String = Nothing
            Dim MyReader As OleDb.OleDbDataReader
            ConnectionQuery = "select customername from customer_stub where id='" & CustomerId & "' and companyid='" & LoggedInCompanyName & "'"

            MyConn.Open()
            Dim MyCommand As New OleDb.OleDbCommand(ConnectionQuery, MyConn)

            MyReader = MyCommand.ExecuteReader

            While MyReader.Read
                ReturnString = MyReader(0)
            End While

            MyConn.Close()
            Return (ReturnString)
        Catch ex As Exception
            ErrorLogger.LogError(ex, "GetCustomerName")
            MsgBox("Error Occured. Please check log for more details.", MsgBoxStyle.Information, MessageTitle)
            Return Nothing
        End Try
    End Function

    Public Function ReturnsPendingTotal() As Integer
        Return PendingTotal
    End Function

    Public Function GetPendingDetails() As DataTable
        Try
            PendingTotal = 0
            Dim MyConn As New OleDb.OleDbConnection(ConnString)
            Dim ConnectionQuery As String
            Dim ReturnString As String = Nothing

            Dim PendingCredits As DataTable

            Dim MyReader As OleDb.OleDbDataReader
            ConnectionQuery = "select * from pending_credit where companyid='" & LoggedInCompanyName & "'"

            Dim MyCommand As New OleDb.OleDbCommand(ConnectionQuery, MyConn)
            MyConn.Open()

            PendingCredits = New DataTable
            PendingCredits.Columns.Add("BILL NO")
            PendingCredits.Columns.Add("DATE")
            PendingCredits.Columns.Add("VALUE")
            PendingCredits.Columns.Add("CUSTOMER NAME")

            MyReader = MyCommand.ExecuteReader

            While (MyReader.Read)
                Dim CustName As String = GetCustomerName(MyReader(3))
                PendingCredits.Rows.Add(MyReader(0), MyReader(1), MyReader(2), CustName)
                PendingTotal += Val(Convert.ToInt64(MyReader(2)))
            End While

            MyConn.Close()

            Return PendingCredits
        Catch ex As Exception
            ErrorLogger.LogError(ex, "GetPendingDetails")
            MsgBox("Error Occured. Please check log for more details.", MsgBoxStyle.Information, MessageTitle)
            Return Nothing
        End Try
    End Function

    Public Sub DeletePendingEntry(ByVal Billno As String)
        Try
            Dim MyConn As New OleDb.OleDbConnection(ConnString)
            Dim ConnectionQuery As String
            Dim ReturnString As String = Nothing

            ConnectionQuery = "delete from pending_credit where billno=" & Billno & " and companyid='" & LoggedInCompanyName & "'"

            Dim MyCommand As New OleDb.OleDbCommand(ConnectionQuery, MyConn)
            MyConn.Open()
            MyCommand.ExecuteReader()
            MyConn.Close()
        Catch ex As Exception
            ErrorLogger.LogError(ex, "DeletePendingEntry")
            MsgBox("Error Occured. Please check log for more details.", MsgBoxStyle.Information, MessageTitle)
        End Try
    End Sub

    Public Sub CreateUser(ByVal UN As String, ByVal Pass As String)
        Try
            Dim MyConn As New OleDb.OleDbConnection(ConnString)
            Dim ConnectionQuery As String
            Dim ReturnString As String = Nothing

            ConnectionQuery = "insert into users values ('" & UN & "','" & Pass & "','','','',0,0,'" & LoggedInCompanyName & "')"

            Dim MyCommand As New OleDb.OleDbCommand(ConnectionQuery, MyConn)
            MyConn.Open()
            MyCommand.ExecuteReader()
            MyConn.Close()
        Catch ex As Exception
            ErrorLogger.LogError(ex, "CreateUser")
            MsgBox("Error Occured. Please check log for more details.", MsgBoxStyle.Information, MessageTitle)
        End Try
    End Sub

    Public Sub ChangeUserPass(ByVal UN As String, ByVal Pass As String)
        Try
            Dim MyConn As New OleDb.OleDbConnection(ConnString)
            Dim ConnectionQuery As String
            Dim ReturnString As String = Nothing

            ConnectionQuery = "update users set pass='" & Pass & "' where uid='" & UN & "' and companyid='" & LoggedInCompanyName & "'"

            Dim MyCommand As New OleDb.OleDbCommand(ConnectionQuery, MyConn)
            MyConn.Open()
            MyCommand.ExecuteReader()
            MyConn.Close()
        Catch ex As Exception
            ErrorLogger.LogError(ex, "ChangeUserPass")
            MsgBox("Error Occured. Please check log for more details.", MsgBoxStyle.Information, MessageTitle)
        End Try
    End Sub

    Public Function GetPrintingSize() As Integer
        Try
            Dim MyConn As New OleDb.OleDbConnection(ConnString)
            Dim ConnectionQuery As String
            Dim ReturnInt As Integer = 0
            Dim MyReader As OleDb.OleDbDataReader
            ConnectionQuery = "select printsizeoption from company where companyname='" & LoggedInCompanyName & "'"

            Dim MyCommand As New OleDb.OleDbCommand(ConnectionQuery, MyConn)
            MyConn.Open()
            MyReader = MyCommand.ExecuteReader()

            While MyReader.Read
                ReturnInt = MyReader(0)
            End While

            MyConn.Close()

            Return ReturnInt
        Catch ex As Exception
            ErrorLogger.LogError(ex, "CreateUser")
            MsgBox("Error Occured. Please check log for more details.", MsgBoxStyle.Information, MessageTitle)
        End Try
    End Function

End Class


'        FinalBill.Columns.Add("ITEM NO")
'        FinalBill.Columns.Add("PRODUCT ID")
'        FinalBill.Columns.Add("PRODUCT NAME")
'        FinalBill.Columns.Add("QUANTITY")
'        FinalBill.Columns.Add("PRODUCT COST")
'        FinalBill.Columns.Add("TOTAL")
'        FinalBill.Columns.Add("DISC RATE")
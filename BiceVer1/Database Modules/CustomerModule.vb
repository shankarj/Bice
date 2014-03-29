Imports System.Data
Imports System.Data.OleDb

Public Class CustomerModule

    Public Sub EditCustomer(ByVal cid As String, ByVal custname As String, ByVal dob As Date, ByVal phone As String, _
                               ByVal addr As String, ByVal email As String, ByVal region As String)
        Try
            Dim MyConn As New OleDb.OleDbConnection(ConnString)

            Dim ConnectionQuery As String = "update customer_stub set " & _
                                            " customername='" & custname & "'" & _
                                            ", dob=#" & dob & "#" & _
                                            ", phonenum='" & phone & "'" & _
                                            ", address='" & addr & "'" & _
                                            ", email='" & email & "'" & _
                                            ", region='" & region & "'" & _
                                            " where companyid='" & LoggedInCompanyName & "' and id='" & cid & "'"

            Dim MyCommand As New OleDb.OleDbCommand(ConnectionQuery, MyConn)

            MyConn.Open()
            MyCommand.ExecuteReader()
            MyConn.Close()
        Catch ex As Exception
            ErrorLogger.LogError(ex, "EditCustomer")
            MsgBox("Error Occured. Please check log for more details.", MsgBoxStyle.Information, MessageTitle)
        End Try
    End Sub

    Public Sub AddNewCustomer(ByVal cid As String, ByVal custname As String, ByVal dob As Date, ByVal phone As String, _
                               ByVal addr As String, ByVal email As String, ByVal region As String)

        Try
            Dim MyConn As New OleDb.OleDbConnection(ConnString)
            Dim ConnectionQuery As String
            Dim MyCommand As OleDb.OleDbCommand


            ConnectionQuery = "insert into customer_stub values ('" & cid & _
                          "','" & custname & _
                          "',#" & dob & _
                          "#,'" & phone & _
                          "','" & addr & _
                          "','" & email & _
                          "','" & region & _
                          "',NULL,0,'" & LoggedInCompanyName & "')"

            MyConn.Open()
            MyCommand = New OleDb.OleDbCommand(ConnectionQuery, MyConn)

            MyCommand.ExecuteReader()
            MyCommand.Dispose()


            MyConn.Close()
            MsgBox("Customer Added.", MsgBoxStyle.Information, MessageTitle)


        Catch ex As Exception
            ErrorLogger.LogError(ex, "AddNewCustomer")
            MsgBox("Error Occured. Check for any of these reasons." & vbCrLf & "1. Product already present." & vbCrLf & "2. Wrong Values entered." & vbCrLf & "Please check log for more details.", MsgBoxStyle.Information, MessageTitle)
        End Try

    End Sub

    Public Sub UpdateCustomerDetails(ByVal CustomerId As String)
        Try
            Dim MyConn As New OleDb.OleDbConnection(ConnString)
            Dim ConnectionQuery As String

            ConnectionQuery = "update customer_stub set lastbilldate=#" & Date_Today & "#, totalvisitcount = (totalvisitcount + 1) where id='" & CustomerId & "' and companyid='" & LoggedInCompanyName & "'"

            Dim MyCommand As New OleDb.OleDbCommand(ConnectionQuery, MyConn)

            MyConn.Open()
            MyCommand.ExecuteReader()
            MyConn.Close()
        Catch ex As Exception
            ErrorLogger.LogError(ex, "UpdateCustomerDetails")
            MsgBox("Error Occured. Please check log for more details.", MsgBoxStyle.Information, MessageTitle)
        End Try
    End Sub

    Public Sub UpdateCustomerBuyingHistory(ByVal Id As String, ByVal Thegrid As DataTable, ByVal billno As Integer)
        Try
            Dim MyConn As New OleDb.OleDbConnection(ConnString)
            Dim ConnectionQuery As String


            Dim MyCommand As OleDb.OleDbCommand

            MyConn.Open()


            For index As Integer = 0 To Thegrid.Rows.Count - 1

                ConnectionQuery = "insert into customer_orderhistory values('" & Id & "'" & _
                                   ",'" & Thegrid.Rows(index)(1) & "'" & _
                                   "," & Thegrid.Rows(index)(3) & _
                                   ",#" & Date_Today & "#" & _
                                   "," & billno & _
                                   ",'" & LoggedInCompanyName & "')"

                MyCommand = New OleDb.OleDbCommand(ConnectionQuery, MyConn)

                MyCommand.ExecuteReader()

                MyCommand.Dispose()

            Next


            MyConn.Close()
        Catch ex As Exception
            ErrorLogger.LogError(ex, "UpdateCustomerBuyingHistory")
            MsgBox("Error Occured. Please check log for more details.", MsgBoxStyle.Information, MessageTitle)
        End Try
    End Sub

End Class

Imports System.Data

Public Class InventoryModule

    Public Sub ReduceQuantity(ByVal ProductId As String, ByVal Quantity As Integer)
        Dim MyConn As New OleDb.OleDbConnection(ConnString)

        Try
            Dim Yes As Boolean = False
            Dim ConnectionQuery As String = "select dependsonid from menu_products where id='" & ProductId & "' and companyid='" & LoggedInCompanyName & "'"
            Dim AReader As OleDb.OleDbDataReader
            Dim MyCommand As New OleDb.OleDbCommand(ConnectionQuery, MyConn)
            Dim TempString As String = Nothing
            Dim TempString1() As String = Nothing
            Dim TempString2() As String = Nothing

            MyConn.Open()

            AReader = MyCommand.ExecuteReader

            While AReader.Read
                TempString = AReader(0)
            End While

            If Not TempString = Nothing Then

                Dim SplitChar1() As Char = {";"}
                Dim SplitChar2() As Char = {":"}

                TempString1 = TempString.Split(SplitChar1)

                For index As Integer = 0 To TempString1.Length - 1
                    Try
                        If Not TempString1(index) = Nothing Then
                            TempString2 = TempString1(index).Split(SplitChar2)
                            TempString2(1) = Convert.ToString(Quantity * Val(TempString2(1)))

                            ConnectionQuery = "UPDATE stock_stub SET quantityavailable = quantityavailable - " & TempString2(1) & " WHERE id='" & TempString2(0) & "' and companyid='" & LoggedInCompanyName & "'"
                            MyCommand = New OleDb.OleDbCommand(ConnectionQuery, MyConn)

                            MyCommand.ExecuteReader()
                        End If
                    Catch ex As Exception
                        ErrorLogger.LogError(ex, "ReduceQuantity")
                        MsgBox("Error Occured. Please check log for more details.", MsgBoxStyle.Information, MessageTitle)
                    End Try
                Next
            End If

            MyConn.Close()

        Catch ex As Exception
            ErrorLogger.LogError(ex, "ReduceQuantity")
            MsgBox("Error Occured. Please check log for more details.", MsgBoxStyle.Information, MessageTitle)
            MyConn.Close()
        End Try
    End Sub

    Public Function IsStocksLess() As Boolean
        Dim MyConn As New OleDb.OleDbConnection(ConnString)

        Try
            Dim Yes As Boolean = False
            Dim ConnectionQuery As String = "select id from stock_stub where quantityavailable < thresholdlevel and companyid='" & LoggedInCompanyName & "'"
            Dim AReader As OleDb.OleDbDataReader
            Dim MyCommand As New OleDb.OleDbCommand(ConnectionQuery, MyConn)
            Dim TempString(200) As String
            Dim index As Integer = 0

            MyConn.Open()

            AReader = MyCommand.ExecuteReader

            While AReader.Read
                TempString(index) = AReader(0)
                index += 1
            End While

            If Not TempString(0) = Nothing Then
                Return True
            Else
                Return False
            End If

        Catch ex As Exception
            ErrorLogger.LogError(ex, "IsStockLess")
            MsgBox("Error Occured. Please check log for more details.", MsgBoxStyle.Information, MessageTitle)
            MyConn.Close()
            Return Nothing
        End Try

    End Function

    Public Sub EditRawProduct(ByVal pid As String, ByVal productname As String, ByVal qavail As Int16, ByVal qtext As String, _
                                ByVal cost As Int16, ByVal thresh As Int16)
        Try
            Dim MyConn As New OleDb.OleDbConnection(ConnString)

            Dim ConnectionQuery As String = "update stock_stub set " & _
                                            " productname='" & productname & _
                                            "', quantityavailable=" & qavail & _
                                            ", quantityunittext='" & qtext & "'" & _
                                            ", unitcost=" & cost & _
                                            ", thresholdlevel=" & thresh & _
                                            " where companyid='" & LoggedInCompanyName & "' and id='" & pid & "'"

            Dim MyCommand As New OleDb.OleDbCommand(ConnectionQuery, MyConn)

            MyConn.Open()
            MyCommand.ExecuteReader()
            MyConn.Close()
        Catch ex As Exception
            ErrorLogger.LogError(ex, "EditRawProduct")
            MsgBox("Error Occured. Please check log for more details.", MsgBoxStyle.Information, MessageTitle)
        End Try
    End Sub

    Public Sub AddNewRawProduct(ByVal pid As String, ByVal productname As String, ByVal qavail As Int16, ByVal qtext As String, _
                                ByVal cost As Int16, ByVal thresh As Int16)
        Try
            Dim MyConn As New OleDb.OleDbConnection(ConnString)
            Dim ConnectionQuery As String
            Dim MyCommand As OleDb.OleDbCommand


            ConnectionQuery = "insert into stock_stub values ('" & pid & _
                          "','" & productname & _
                          "'," & qavail & _
                          ",'" & qtext & _
                          "'," & cost & _
                          "," & thresh & ",NULL,NULL,NULL,'" & LoggedInCompanyName & "')"

            MyConn.Open()
            MyCommand = New OleDb.OleDbCommand(ConnectionQuery, MyConn)

            MyCommand.ExecuteReader()
            MyCommand.Dispose()


            MyConn.Close()
            MsgBox("Product Added.", MsgBoxStyle.Information, MessageTitle)


        Catch ex As Exception
            ErrorLogger.LogError(ex, "AddNewRawProduct")
            MsgBox("Error Occured. Check for any of these reasons." & vbCrLf & "1. Product already present." & vbCrLf & "2. Wrong Values entered." & vbCrLf & "Please check log for more details.", MsgBoxStyle.Information, MessageTitle)
        End Try

    End Sub

    Public Function NewOrder(ByVal OrderId As Integer, ByVal SupplierId As String, ByVal TheTable As DataTable, ByVal DueDate As String, ByVal OrderTotal As Long) As Boolean
        Try
            Dim MyConn As New OleDb.OleDbConnection(ConnString)
            Dim ConnectionQuery As String
            Dim MyCommand As OleDb.OleDbCommand
            Dim Reader As OleDb.OleDbDataReader = Nothing

            MyConn.Open()

            ConnectionQuery = "select * from stock_supplier where id='" & SupplierId & "' and companyid='" & LoggedInCompanyName & "'"
            MyCommand = New OleDb.OleDbCommand(ConnectionQuery, MyConn)
            Reader = MyCommand.ExecuteReader()

            If Reader.HasRows = True Then

                For index As Integer = 0 To TheTable.Rows.Count - 1
                    ConnectionQuery = "insert into stock_orderhistory values ('" & OrderId & _
                              "','" & SupplierId & _
                              "','" & TheTable.Rows(index)(0) & _
                              "'," & TheTable.Rows(index)(1) & _
                              "," & TheTable.Rows(index)(2) & _
                              "," & TheTable.Rows(index)(3) & _
                              ",#" & Date_Today & _
                              "#,#" & DueDate & _
                              "#,'pending'" & _
                              ",'" & LoggedInCompanyName & "')"


                    MyCommand = New OleDb.OleDbCommand(ConnectionQuery, MyConn)

                    MyCommand.ExecuteReader()
                Next

                ConnectionQuery = "update stock_supplier set lastorderdate=#" & Date_Today & "#, lastordervalue=" & OrderTotal & ", totalordervalue = totalordervalue + " & OrderTotal & " where id='" & SupplierId & "' and companyid='" & LoggedInCompanyName & "'"
                MyCommand = New OleDb.OleDbCommand(ConnectionQuery, MyConn)
                MyCommand.ExecuteReader()

                MyConn.Close()
                MsgBox("Order Generated.", MsgBoxStyle.Information, MessageTitle)
                Return True

            Else
                MsgBox("NO SUCH SUPPLIER EXISTS. PLEASE ENTER A CORRECT SUPPLIER ID.", MsgBoxStyle.Information, MessageTitle)
            End If
        Catch ex As Exception
            ErrorLogger.LogError(ex, "NewOrder")
            MsgBox("Error Occured. Check for any of these reasons." & vbCrLf & "1. Product already present." & vbCrLf & "2. Wrong Values entered." & vbCrLf & "Please check log for more details.", MsgBoxStyle.Information, MessageTitle)
        End Try

    End Function

    Public Function GetLastOrderDetails(ByVal productname As String) As String()
        Dim MyConn As New OleDb.OleDbConnection(ConnString)

        Try
            Dim ConnectionQuery As String
            Dim AReader As OleDb.OleDbDataReader
            Dim MyCommand As OleDb.OleDbCommand
            Dim TempString(5) As String
            Dim index As Integer = 0
            Dim productid As String = Nothing

            ConnectionQuery = "select id from stock_stub where productname='" & productname & "' and companyid='" & LoggedInCompanyName & "'"

            MyConn.Open()

            MyCommand = New OleDb.OleDbCommand(ConnectionQuery, MyConn)
            AReader = MyCommand.ExecuteReader

            While AReader.Read
                productid = AReader(0)
            End While

            ConnectionQuery = "select orderid,  orderdate, productquantity, suppliername, " & _
                              "pendingstatus from stock_supplier, stock_orderhistory where id=supplierid " & _
                              "and stock_orderhistory.companyid='" & LoggedInCompanyName & "' and  productid='" & productid & "' and " & _
                              "orderdate=(select max(orderdate) from stock_orderhistory where productid='" & productid & "')"

            MyCommand = New OleDb.OleDbCommand(ConnectionQuery, MyConn)
            AReader = MyCommand.ExecuteReader

            While AReader.Read
                For index1 As Integer = 0 To 4
                    TempString(index1) = AReader(index1)
                Next
            End While

            Return TempString

        Catch ex As Exception
            ErrorLogger.LogError(ex, "IsStockLess")
            MsgBox("Error Occured. Please check log for more details.", MsgBoxStyle.Information, MessageTitle)
            MyConn.Close()
            Return Nothing
        End Try
    End Function

    Public Function GetRawProductDetails(ByVal ProductId As String) As String()
        Try
            Dim MyConn As New OleDb.OleDbConnection(ConnString)
            Dim ConnectionQuery As String
            Dim RetString(5) As String

            Dim IsProductNameGiven As Boolean = True

            For index As Integer = 0 To ProductId.Count - 1
                If Char.IsDigit(ProductId.Chars(index)) Then
                    IsProductNameGiven = False
                End If
            Next

            If IsProductNameGiven = False Then
                ConnectionQuery = "select id, unitcost, quantityunittext from stock_stub where id ='" & ProductId & "' and companyid='" & LoggedInCompanyName & "'"
                RetString(0) = "ID"
            Else
                ConnectionQuery = "select id, unitcost, quantityunittext from stock_stub where productname ='" & ProductId & "' and companyid='" & LoggedInCompanyName & "'"
                RetString(0) = "NAME"
            End If

            Dim MyCommand As New OleDb.OleDbCommand(ConnectionQuery, MyConn)
            Dim AReader As OleDb.OleDbDataReader
            Dim Table_List As New DataTable

            MyConn.Open()

            AReader = MyCommand.ExecuteReader

            While (AReader.Read)
                RetString(1) = AReader(0)
                RetString(2) = AReader(1)
                RetString(3) = AReader(2)
            End While

            AReader.Close()
            MyConn.Close()

            Return RetString
        Catch ex As Exception
            ErrorLogger.LogError(ex, "GetRawProductDetails")
            MsgBox("Error Occured. Please check log for more details.", MsgBoxStyle.Information, MessageTitle)
            Return Nothing
        End Try
    End Function

    Public Sub ReStock(ByVal OrderId As String, ByVal SupplierId As String, ByVal Thetable As DataTable)
        Try
            Dim MyConn As New OleDb.OleDbConnection(ConnString)
            Dim ConnectionQuery As String
            Dim MyCommand As OleDb.OleDbCommand

            MyConn.Open()


            For index As Integer = 0 To Thetable.Rows.Count - 1
                ConnectionQuery = "update stock_orderhistory set pendingstatus='received' where orderid='" & OrderId & "' and productid='" & Thetable.Rows(index)(0) & "' and companyid='" & LoggedInCompanyName & "'"
                MyCommand = New OleDb.OleDbCommand(ConnectionQuery, MyConn)
                MyCommand.ExecuteReader()
            Next


            For index As Integer = 0 To Thetable.Rows.Count - 1
                ConnectionQuery = "update stock_stub set lastorderdate=#" & Date_Today & "#, lastorderquantity=" & Thetable.Rows(index)(1) & ", supplierid='" & SupplierId & "', quantityavailable=quantityavailable + " & Val(Thetable.Rows(index)(1)) & " where id='" & Thetable.Rows(index)(0) & "' and companyid='" & LoggedInCompanyName & "'"
                MyCommand = New OleDb.OleDbCommand(ConnectionQuery, MyConn)
                MyCommand.ExecuteReader()
            Next


            MyConn.Close()

        Catch ex As Exception
            ErrorLogger.LogError(ex, "ReStock")
            MsgBox("Error Occured. Please check log for more details.", MsgBoxStyle.Information, MessageTitle)
        End Try

    End Sub

    Public Sub AddNewSupplier(ByVal id As String, ByVal invoice As String, ByVal suppname As String, ByVal phone As String _
                              , ByVal mail As String, ByVal addr As String)
        Try
            Dim MyConn As New OleDb.OleDbConnection(ConnString)
            Dim ConnectionQuery As String
            Dim MyCommand As OleDb.OleDbCommand


            ConnectionQuery = "insert into stock_supplier values ('" & id & _
                          "','" & invoice & _
                          "','" & suppname & _
                          "','" & phone & _
                          "','" & mail & _
                          "','" & addr & _
                          "',NULL,NULL,0,'" & LoggedInCompanyName & "')"

            MyConn.Open()
            MyCommand = New OleDb.OleDbCommand(ConnectionQuery, MyConn)

            MyCommand.ExecuteReader()
            MyCommand.Dispose()


            MyConn.Close()

            MsgBox("Supplier Added.", MsgBoxStyle.Information, MessageTitle)
        Catch ex As Exception
            ErrorLogger.LogError(ex, "AddNewSupplier")
            MsgBox("Error Occured. Check for any of these reasons." & vbCrLf & "1. Supplier already present." & vbCrLf & "2. Wrong Values entered." & vbCrLf & "Please check log for more details.", MsgBoxStyle.Information, MessageTitle)
        End Try

    End Sub

    Public Sub EditSupplier(ByVal id As String, ByVal invoice As String, ByVal suppname As String, ByVal phone As String _
                              , ByVal mail As String, ByVal addr As String)
        Try
            Dim MyConn As New OleDb.OleDbConnection(ConnString)


            Dim ConnectionQuery As String = "update stock_supplier set " & _
                                            " invoice='" & invoice & _
                                            "', suppliername='" & suppname & "'" & _
                                            ", phonenum='" & phone & "'" & _
                                            ", email='" & mail & "'" & _
                                            ", address='" & addr & "'" & _
                                            " where companyid='" & LoggedInCompanyName & "' and id='" & id & "'"

            Dim MyCommand As New OleDb.OleDbCommand(ConnectionQuery, MyConn)

            MyConn.Open()
            MyCommand.ExecuteReader()
            MyConn.Close()
        Catch ex As Exception
            ErrorLogger.LogError(ex, "EditSupplier")
            MsgBox("Error Occured. Please check log for more details.", MsgBoxStyle.Information, MessageTitle)
        End Try
    End Sub

End Class

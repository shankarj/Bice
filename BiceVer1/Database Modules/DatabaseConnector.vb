Imports System.Data


Public Class DatabaseConnector

    '---------------------------------------------------------------------------------
    'FOR BILLS PAGE
    '---------------------------------------------------------------------------------

    Public Function Login(ByVal UserName As String, ByVal Pass As String, ByVal CompanyName As String) As Boolean
        Try

     
            Dim LoginSuccess As Boolean = False

            Dim MyConn As New OleDb.OleDbConnection(ConnString)
            Dim ConnectionQuery As String

            ConnectionQuery = "select pass from users where uid='" & UserName & "' and companyid='" & CompanyName & "'"

            Dim MyCommand As New OleDb.OleDbCommand(ConnectionQuery, MyConn)
            Dim MyReader As OleDb.OleDbDataReader
            MyConn.Open()

            MyReader = MyCommand.ExecuteReader

            While MyReader.Read
                If MyReader.HasRows = True Then
                    If Pass = MyReader(0) Then
                        LoginSuccess = True
                    End If
                Else
                    MsgBox("No Such User Exists !", MsgBoxStyle.Information, MessageTitle)
                End If

            End While

            Return LoginSuccess

        Catch ex As Exception
            ErrorLogger.LogError(ex, "LOGIN")
            MsgBox("Error Occured. Please check log for more details.", MsgBoxStyle.Information, MessageTitle)
        End Try
    End Function

    Public Function GetListofProducts(Optional ByVal ProductId As String = "NothingGivenBalls") As DataTable

        Try
            Dim MyConn As New OleDb.OleDbConnection(ConnString)
            Dim ConnectionQuery As String

            If ProductId = "NothingGivenBalls" Then
                ConnectionQuery = "select id, productname,vat,groupname from menu_products where companyid='" & LoggedInCompanyName & "'"
            Else
                Dim IsProductNameGiven As Boolean = True

                For index As Integer = 0 To ProductId.Count - 1
                    If Char.IsDigit(ProductId.Chars(index)) Then
                        IsProductNameGiven = False
                    End If
                Next

                If IsProductNameGiven = False Then
                    ConnectionQuery = "select id, productname,vat,groupname from menu_products where id like '" & ProductId & "%' and companyid='" & LoggedInCompanyName & "'"
                Else
                    ConnectionQuery = "select id, productname,vat,groupname from menu_products where productname like '" & ProductId & "%' and companyid='" & LoggedInCompanyName & "'"
                End If

            End If


            Dim MyCommand As New OleDb.OleDbCommand(ConnectionQuery, MyConn)
            Dim AReader As OleDb.OleDbDataReader
            Dim Table_List As New DataTable

            MyConn.Open()

            AReader = MyCommand.ExecuteReader
            Table_List.Load(AReader)

            AReader.Close()
            MyConn.Close()

            Table_List.Columns(0).ColumnName = "Product Id"
            Table_List.Columns(1).ColumnName = "Product Name"
            Table_List.Columns(2).ColumnName = "VAT RATE"
            Table_List.Columns(3).ColumnName = "Group Name"

            Return Table_List
        Catch ex As Exception
            ErrorLogger.LogError(ex, "GetListOfProducts")
            MsgBox("Error Occured. Please check log for more details.", MsgBoxStyle.Information, MessageTitle)
            Return Nothing
        End Try
    End Function

    Public Function GetProductDetails(ByVal ProductId As String) As String()
        Try
            Dim ProductDetailsString(5) As String
            Dim MyConn As New OleDb.OleDbConnection(ConnString)
            Dim ConnectionQuery As String

            Dim AReader As OleDb.OleDbDataReader

            Dim IsProductNameGiven As Boolean = True

            For index As Integer = 0 To ProductId.Count - 1
                If Char.IsDigit(ProductId.Chars(index)) Then
                    IsProductNameGiven = False
                End If
            Next

            If IsProductNameGiven = False Then
                ConnectionQuery = "select id, productname, percost, discount from menu_products where id='" & ProductId & "' and companyid='" & LoggedInCompanyName & "'"
            Else
                ConnectionQuery = "select id, productname, percost, discount from menu_products where productname = '" & ProductId & "' and companyid='" & LoggedInCompanyName & "'"
            End If

            Dim MyCommand As New OleDb.OleDbCommand(ConnectionQuery, MyConn)

            MyConn.Open()

            AReader = MyCommand.ExecuteReader

            While AReader.Read
                ProductDetailsString(0) = AReader(0)
                ProductDetailsString(1) = AReader(1)
                ProductDetailsString(2) = AReader(2)
                ProductDetailsString(3) = AReader(3)
            End While

            AReader.Close()
            MyConn.Close()

            Return ProductDetailsString
        Catch ex As Exception
            ErrorLogger.LogError(ex, "GetProductDetails")
            MsgBox("Error Occured. Please check log for more details.", MsgBoxStyle.Information, MessageTitle)
            Return Nothing
        End Try
    End Function

    Public Sub CreateABill_AllEntries(ByVal BillType As String, ByVal BillNo As Integer, ByVal BillDate As String, ByVal BillValue As Integer, ByVal TaxValue As Integer, ByVal BillDiscountRate As Integer, ByVal AmountGiven As Integer, _
                            ByVal CreditStatus As String, ByVal UserId As String, ByVal CompanyId As String)
        Try
            Dim MyConn As New OleDb.OleDbConnection(ConnString)
            Dim ConnectionQuery As String

            ConnectionQuery = "insert into all_entries values (" & BillNo & _
                              ",'" & BillType & _
                              "',#" & BillDate & _
                              "#," & BillValue & _
                              "," & TaxValue & _
                              "," & BillDiscountRate & _
                              "," & AmountGiven & _
                              ",'" & CreditStatus & _
                              "','" & UserId & _
                              "','" & CompanyId & "')"

            Dim MyCommand_AllEntriesTable As New OleDb.OleDbCommand(ConnectionQuery, MyConn)

            MyConn.Open()

            MyCommand_AllEntriesTable.ExecuteReader()

            MyConn.Close()
        Catch ex As Exception
            ErrorLogger.LogError(ex, "CreateABill_AllEntries")
            MsgBox("Error Occured. Please check log for more details.", MsgBoxStyle.Information, MessageTitle)
        End Try
    End Sub

    Public Sub CreateABill_BillDetail(ByVal BillNo As Integer, ByVal BillType As String, ByVal TheFinalBill As DataTable)
        Try
            Dim MyConn As New OleDb.OleDbConnection(ConnString)
            Dim ConnectionQuery As String
            Dim MyCommand As OleDb.OleDbCommand

            Dim Count As Integer = TheFinalBill.Rows.Count + 2

            Dim ProductId(Count) As String
            Dim Quantity(Count) As Integer
            Dim Pcost(Count) As Integer
            Dim TotalCost(Count) As Integer


            MyConn.Open()

            For index As Integer = 0 To TheFinalBill.Rows.Count - 1
                ProductId(index) = TheFinalBill.Rows(index).Item(1)
                Quantity(index) = TheFinalBill.Rows(index).Item(3)
                Pcost(index) = TheFinalBill.Rows(index).Item(4)
                TotalCost(index) = TheFinalBill.Rows(index).Item(5)


                ConnectionQuery = "insert into bill_detail values (" & BillNo & _
                              ",'" & BillType & _
                              "',#" & Date_Today & _
                              "#,'" & ProductId(index) & _
                              "'," & Quantity(index) & _
                              "," & Pcost(index) & _
                              "," & TotalCost(index) & _
                              ",'" & LoggedInCompanyName & "')"

                MyCommand = New OleDb.OleDbCommand(ConnectionQuery, MyConn)

                MyCommand.ExecuteReader()
                MyCommand.Dispose()

            Next

            MyConn.Close()

        Catch ex As Exception

            ErrorLogger.LogError(ex, "CreateABill_BillDetail")
            MsgBox("Error Occured. Please check log for more details.", MsgBoxStyle.Information, MessageTitle)
        End Try

    End Sub

    Public Sub CreateABill_PendingCredit(ByVal BillNo As Integer, ByVal BillDate As String, ByVal BillValue As Integer, ByVal ToCustId As String)
        Try
            Dim MyConn As New OleDb.OleDbConnection(ConnString)
            Dim ConnectionQuery As String
            Dim MyCommand As OleDb.OleDbCommand

            MyConn.Open()

            ConnectionQuery = "insert into pending_credit values (" _
                              & BillNo & _
                              ",#" & BillDate & _
                              "#," & BillValue & _
                              ",'" & ToCustId & _
                              "','" & LoggedInCompanyName & "')"

            MyCommand = New OleDb.OleDbCommand(ConnectionQuery, MyConn)

            MyCommand.ExecuteReader()
            MyCommand.Dispose()

            MyConn.Close()
        Catch ex As Exception
            ErrorLogger.LogError(ex, "CreateABill_PendingCredit")
            MsgBox("Error Occured. Please check log for more details.", MsgBoxStyle.Information, MessageTitle)
        End Try
    End Sub

    Public Function GetCurrentBillNumber() As Integer
        Try
            Dim MyConn As New OleDb.OleDbConnection(ConnString)
            Dim ConnectionQuery As String = "select max(billno) from all_entries where companyid ='" & LoggedInCompanyName & "'"
            Dim MyCommand As New OleDb.OleDbCommand(ConnectionQuery, MyConn)
            Dim AReader As OleDb.OleDbDataReader
            Dim TempNum As Integer

            MyConn.Open()

            AReader = MyCommand.ExecuteReader

            While AReader.Read
                Try
                    TempNum = Convert.ToInt16(AReader(0))
                Catch ex As Exception
                    TempNum = 0
                End Try
            End While

            AReader.Close()
            MyConn.Close()

            Return TempNum
        Catch ex As Exception
            ErrorLogger.LogError(ex, "GetCurrentBillNum")
            MsgBox("Error Occured. Please check log for more details.", MsgBoxStyle.Information, MessageTitle)
        End Try
    End Function

    Public Function VATRATE_GetorSet(ByVal ActionType As Int16, Optional ByVal VatValue As Double = 0) As Double
        Try
            Dim MyConn As New OleDb.OleDbConnection(ConnString)
            Dim ConnectionQuery As String
            Dim TempDouble As Double

            Dim AReader As OleDb.OleDbDataReader

            If ActionType = 0 Then
                ConnectionQuery = "select vatrate from company where companyname='" & LoggedInCompanyName & "'"
            Else
                ConnectionQuery = "update company set vatrate=" & VatValue & " where companyname='" & LoggedInCompanyName & "'"

            End If


            Dim MyCommand As New OleDb.OleDbCommand(ConnectionQuery, MyConn)
            MyConn.Open()


            If ActionType = 0 Then
                AReader = MyCommand.ExecuteReader

                While AReader.Read
                    TempDouble = AReader(0)
                End While

                AReader.Close()

            Else

                MyCommand.ExecuteReader()

            End If


            MyConn.Close()

            Return TempDouble

        Catch ex As Exception
            ErrorLogger.LogError(ex, "VATRATE")
            MsgBox("Error Occured. Please check log for more details.", MsgBoxStyle.Information, MessageTitle)
        End Try
    End Function

    Public Function KOTNO_GetorSet(ByVal ActionType As Int16, Optional ByVal kotValue As String = "0") As String
        Try
            Dim MyConn As New OleDb.OleDbConnection(ConnString)
            Dim ConnectionQuery As String
            Dim TempString As String = Nothing

            Dim AReader As OleDb.OleDbDataReader

            If ActionType = 0 Then
                ConnectionQuery = "select kotno from company where companyname='" & LoggedInCompanyName & "'"
            Else
                ConnectionQuery = "update company set kotno='" & kotValue & "' where companyname='" & LoggedInCompanyName & "'"

            End If


            Dim MyCommand As New OleDb.OleDbCommand(ConnectionQuery, MyConn)
            MyConn.Open()


            If ActionType = 0 Then
                AReader = MyCommand.ExecuteReader

                While AReader.Read
                    TempString = AReader(0)
                End While

                AReader.Close()

            Else

                MyCommand.ExecuteReader()

            End If


            MyConn.Close()

            Return TempString
        Catch ex As Exception
            ErrorLogger.LogError(ex, "KOTNO")
            MsgBox("Error Occured. Please check log for more details.", MsgBoxStyle.Information, MessageTitle)
            Return Nothing
        End Try
    End Function

    Public Function BillFooter_GetorSet(ByVal ActionType As Int16, Optional ByVal FooterValue As String = "0") As String
        Try
            Dim MyConn As New OleDb.OleDbConnection(ConnString)
            Dim ConnectionQuery As String
            Dim TempString As String = Nothing

            Dim AReader As OleDb.OleDbDataReader

            If ActionType = 0 Then
                ConnectionQuery = "select billfooter from company where companyname='" & LoggedInCompanyName & "'"
            Else
                ConnectionQuery = "update company set billfooter='" & FooterValue & "' where companyname='" & LoggedInCompanyName & "'"

            End If


            Dim MyCommand As New OleDb.OleDbCommand(ConnectionQuery, MyConn)
            MyConn.Open()


            If ActionType = 0 Then
                AReader = MyCommand.ExecuteReader

                While AReader.Read
                    TempString = AReader(0)
                End While

                AReader.Close()

            Else

                MyCommand.ExecuteReader()

            End If


            MyConn.Close()

            Return TempString

        Catch ex As Exception
            ErrorLogger.LogError(ex, "BillFooter")
            MsgBox("Error Occured. Please check log for more details.", MsgBoxStyle.Information, MessageTitle)
            Return Nothing
        End Try

    End Function

    Public Function GetCustomerDetails(ByVal CustomerId As String) As String()
        Try
            Dim Details(6) As String
            Dim MyConn As New OleDb.OleDbConnection(ConnString)
            Dim ConnectionQuery As String
            Dim AReader As OleDb.OleDbDataReader

            Dim IsNameGiven As Boolean = True

            For index As Integer = 0 To CustomerId.Count - 1
                If Char.IsDigit(CustomerId.Chars(index)) Then
                    IsNameGiven = False
                End If
            Next

            If IsNameGiven = False Then
                ConnectionQuery = "select id, customername, phonenum, dob, email, address from customer_stub where phonenum='" & CustomerId & "' and companyid='" & LoggedInCompanyName & "'"
            Else
                ConnectionQuery = "select id, customername, phonenum, dob, email, address from customer_stub where customername = '" & CustomerId & "' and companyid='" & LoggedInCompanyName & "'"
            End If

            Dim MyCommand As New OleDb.OleDbCommand(ConnectionQuery, MyConn)

            MyConn.Open()

            AReader = MyCommand.ExecuteReader

            While AReader.Read
                Details(0) = AReader(0)
                Details(1) = AReader(1)
                Details(2) = AReader(2)
                Details(3) = AReader(3)
                Details(4) = AReader(4)
                Details(5) = AReader(5)
            End While

            MyConn.Close()

            Return Details

        Catch ex As Exception
            ErrorLogger.LogError(ex, "GetCustomerDetails")
            MsgBox("Error Occured. Please check log for more details.", MsgBoxStyle.Information, MessageTitle)
            Return Nothing
        End Try

    End Function

    '---------------------------------------------------------------------------------
    'FOR PRODUCT GROUPS PAGE
    '---------------------------------------------------------------------------------

    Public Function CheckForProduct(ByVal productCode As String) As Boolean
        Try
            Dim Yes As Boolean = False
            Dim MyConn As New OleDb.OleDbConnection(ConnString)
            Dim ConnectionQuery As String = "select id from menu_products where companyid='" & LoggedInCompanyName & "'"
            Dim AReader As OleDb.OleDbDataReader
            Dim MyCommand As New OleDb.OleDbCommand(ConnectionQuery, MyConn)

            MyConn.Open()

            AReader = MyCommand.ExecuteReader

            While AReader.Read
                If AReader(0) = productCode Then
                    Yes = True
                End If
            End While

            MyConn.Close()

            Return Yes
        Catch ex As Exception
            ErrorLogger.LogError(ex, "CheckForProduct")
            MsgBox("Error Occured. Please check log for more details.", MsgBoxStyle.Information, MessageTitle)
        End Try
    End Function

    Public Sub InsertANewProduct(ByVal ProductId As String, ByVal ProductName As String, ByVal percost As Double, ByVal QuantityAvail As Integer, _
                                 Optional ByVal Disc As Double = 0, Optional ByVal DependsOn As String = " ", Optional ByVal Group As String = " ", Optional ByVal VAT As String = " ")
        Try
            Dim MyConn As New OleDb.OleDbConnection(ConnString)
            Dim Temp_ListDependent As String = Nothing

            If CheckForProduct(ProductId) = False Then
                For index As Integer = 0 To Temp_RawListbox.Items.Count - 1
                    Temp_ListDependent &= Temp_RawListbox.Items(index) & ";"
                Next

                Dim ConnectionQuery As String = "insert into menu_products values (" & _
                                                "'" & ProductId & "'" & _
                                                ",'" & ProductName & "'" & _
                                                "," & percost & _
                                                "," & Disc & _
                                                ",'" & DependsOn & "'" & _
                                                ",'" & Group & "'" & _
                                                ",'" & VAT & "'" & _
                                                ",'" & LoggedInCompanyName & "'" & _
                                                ",'" & Temp_ListDependent & "')"

                Dim MyCommand As New OleDb.OleDbCommand(ConnectionQuery, MyConn)

                MyConn.Open()
                MyCommand.ExecuteReader()
                MyConn.Close()
            Else
                MsgBox("Product Id already Present.", MsgBoxStyle.Information, MessageTitle)

            End If
        Catch ex As Exception
            ErrorLogger.LogError(ex, "InsertNewProduct")
            MsgBox("Error Occured. Please check log for more details.", MsgBoxStyle.Information, MessageTitle)
        End Try
    End Sub

    Public Sub DeleteProductGroup(ByVal GroupText As String)
        Try
            Dim MyConn As New OleDb.OleDbConnection(ConnString)
            Dim ConnectionQuery As String = "delete from menu_products where groupname='" & GroupText & "' and companyid='" & LoggedInCompanyName & "'"
            Dim MyCommand As New OleDb.OleDbCommand(ConnectionQuery, MyConn)
            MyConn.Open()
            MyCommand.ExecuteReader()
            MyConn.Close()
        Catch ex As Exception
            ErrorLogger.LogError(ex, "DeleteProductGroup")
            MsgBox("Error Occured. Please check log for more details.", MsgBoxStyle.Information, MessageTitle)
        End Try
    End Sub

    Public Function GetProductDetailsToEdit(ByVal ProductCode As String) As String()
        Try
            Temp_RawListbox.Items.Clear()
            DependentsString = Nothing

            Dim ProductDetailsString(10) As String
            Dim MyConn As New OleDb.OleDbConnection(ConnString)
            Dim ConnectionQuery As String

            Dim Temp_String() As String
            Dim Temp_Char() As Char = {";"}

            Dim AReader As OleDb.OleDbDataReader

            ConnectionQuery = "select * from menu_products where id='" & ProductCode & "' and companyid='" & LoggedInCompanyName & "'"

            Dim MyCommand As New OleDb.OleDbCommand(ConnectionQuery, MyConn)

            MyConn.Open()

            AReader = MyCommand.ExecuteReader

            While AReader.Read
                ProductDetailsString(0) = AReader(0)
                ProductDetailsString(1) = AReader(1)
                ProductDetailsString(2) = AReader(2)
                ProductDetailsString(3) = AReader(3)
                ProductDetailsString(4) = AReader(4)
                ProductDetailsString(5) = AReader(5)
                ProductDetailsString(6) = AReader(6)
                ProductDetailsString(7) = AReader(7)
                ProductDetailsString(8) = AReader(8)
            End While

            Temp_String = ProductDetailsString(8).Split(Temp_Char)


            For index As Integer = 0 To Temp_String.Length - 2
                Temp_RawListbox.Items.Add(Temp_String(index))
            Next

            AReader.Close()
            MyConn.Close()

            Return ProductDetailsString
        Catch ex As Exception
            ErrorLogger.LogError(ex, "GetProductDetailstoEdit")
            MsgBox("Error Occured. Please check log for more details.", MsgBoxStyle.Information, MessageTitle)
            Return Nothing
        End Try
    End Function

    Public Sub EditAProduct(ByVal ProductId As String, ByVal ProductName As String, ByVal percost As Double, _
                                 ByVal Disc As Double, ByVal DependsOn As String, ByVal Group As String, ByVal VAT As Integer)
        Try
            Dim MyConn As New OleDb.OleDbConnection(ConnString)
            Dim Temp_ListDependent As String = ""

            For index As Integer = 0 To Temp_RawListbox.Items.Count - 1
                Temp_ListDependent &= Temp_RawListbox.Items(index) & ";"
            Next


            Dim ConnectionQuery As String = "update menu_products set productname='" & ProductName & "'" & _
                                            ", percost=" & percost & _
                                            ", discount=" & Disc & _
                                            ", dependsonid='" & DependsOn & "'" & _
                                            ", groupname='" & Group & "'" & _
                                            ", vat=" & VAT & _
                                            ", temp_depend='" & Temp_ListDependent & "'" & _
                                            " where companyid='" & LoggedInCompanyName & "'" & _
                                            " and id='" & ProductId & "'"

            Dim MyCommand As New OleDb.OleDbCommand(ConnectionQuery, MyConn)

            MyConn.Open()
            MyCommand.ExecuteReader()
            MyConn.Close()
        Catch ex As Exception
            ErrorLogger.LogError(ex, "EditAProduct")
            MsgBox("Error Occured. Please check log for more details.", MsgBoxStyle.Information, MessageTitle)
        End Try
    End Sub

    Public Sub DeleteAProduct(ByVal ProductId As String)
        Try
            Dim MyConn As New OleDb.OleDbConnection(ConnString)

            Dim ConnectionQuery As String = "delete from menu_products where id='" & ProductId & "' and  companyid='" & LoggedInCompanyName & "'"

            Dim MyCommand As New OleDb.OleDbCommand(ConnectionQuery, MyConn)

            MyConn.Open()
            MyCommand.ExecuteReader()
            MyConn.Close()
        Catch ex As Exception
            ErrorLogger.LogError(ex, "DeleteAProduct")
            MsgBox("Error Occured. Please check log for more details.", MsgBoxStyle.Information, MessageTitle)
        End Try
    End Sub

    Public Function GetRawProdDetails(ByVal IDorName As Integer, ByVal RawId As String) As String()
        Try
            Dim MyConn As New OleDb.OleDbConnection(ConnString)
            Dim ConnectionQuery As String
            Dim RawDetails(2) As String

            Dim AReader As OleDb.OleDbDataReader

            If IDorName = 0 Then
                ConnectionQuery = "select productname, quantityunittext from stock_stub where id='" & RawId & "' and companyid='" & LoggedInCompanyName & "'"
            Else
                ConnectionQuery = "select id, quantityunittext from stock_stub where productname='" & RawId & "' and companyid='" & LoggedInCompanyName & "'"
            End If

            Dim MyCommand As New OleDb.OleDbCommand(ConnectionQuery, MyConn)

            MyConn.Open()

            AReader = MyCommand.ExecuteReader

            While AReader.Read
                RawDetails(0) = AReader(0)
                RawDetails(1) = AReader(1)
            End While

            AReader.Close()
            MyConn.Close()

            Return RawDetails
        Catch ex As Exception
            ErrorLogger.LogError(ex, "GetRawDetails")
            MsgBox("Error Occured. Please check log for more details.", MsgBoxStyle.Information, MessageTitle)
            Return Nothing
        End Try
    End Function

End Class


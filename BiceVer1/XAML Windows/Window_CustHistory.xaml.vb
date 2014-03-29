Imports System.Data
Imports System.Data.OleDb

Partial Public Class Window_CustHistory
    Dim ProductId, CustomerId As String

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button1.Click
        Me.Close()
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button2.Click
        LoadCustomerAndProducts(True)
        If CheckBox2.IsChecked = True Then
            LoadChart(True)
        Else
            LoadChart(False)
        End If
    End Sub


    Private Sub LoadChart(ByVal IsOption2 As Boolean)
        Try
            Dim conn As New OleDbConnection(ConnString)
            Dim Conquery As String

            If IsOption2 = False Then
                Conquery = "select orderdate, sum(quantity) as sq from menu_products, customer_orderhistory where menu_products.id=productid and customer_orderhistory.id='" & CustomerId & _
                           "' and productid='" & ProductId & "' and customer_orderhistory.companyid='" & LoggedInCompanyName & "' group by orderdate"

                Dim adapter As OleDbDataAdapter = New OleDbDataAdapter(Conquery, conn)
                Dim ds As New DataSet("mdset")

                conn.Open()
                adapter.Fill(ds, "menu_products, customer_orderhistory")

                Mychart.SeriesSource = ds.Tables(0).DefaultView
                Mychart.IDMemberPath = "orderdate"
                Mychart.TextMemberPath = "orderdate"

                a1.DataItemsSource = ds.Tables(0).DefaultView
                a1.SeriesIDMemberPath = "orderdate"
                a1.ValueMemberPath = "sq"

                ds.Dispose()
                adapter.Dispose()
            Else
                Conquery = "select productname, sum(quantity) as sq from customer_orderhistory, menu_products where productid=menu_products.id and customer_orderhistory.id='" & CustomerId & "'and customer_orderhistory.companyid='" & LoggedInCompanyName & "' group by productname"

                Dim adapter As OleDbDataAdapter = New OleDbDataAdapter(Conquery, conn)
                Dim ds As New DataSet("mdset")

                conn.Open()
                adapter.Fill(ds, "customer_orderhistory, menu_products")

                Mychart.SeriesSource = ds.Tables(0).DefaultView
                Mychart.IDMemberPath = "productname"
                Mychart.TextMemberPath = "productname"

                a1.DataItemsSource = ds.Tables(0).DefaultView
                a1.SeriesIDMemberPath = "productname"
                a1.ValueMemberPath = "sq"

                ds.Dispose()
                adapter.Dispose()
            End If
        Catch ex As Exception
            ErrorLogger.LogError(ex, "LoadChart")
        End Try
    End Sub

    Private Sub LoadCustomerAndProducts(ByVal Idgiven As Boolean)
        Try

            Dim MyConn As New OleDb.OleDbConnection(ConnString)
            Dim ConnectionQuery As String
            Dim MyCommand As OleDb.OleDbCommand = Nothing
            Dim AReader As OleDb.OleDbDataReader = Nothing

            MyConn.Open()

            If Idgiven = False Then
                ConnectionQuery = "select customername from customer_stub where companyid ='" & LoggedInCompanyName & "'"
                ComboBox1.Items.Clear()

                MyCommand = New OleDb.OleDbCommand(ConnectionQuery, MyConn)
                AReader = MyCommand.ExecuteReader

                While AReader.Read
                    ComboBox1.Items.Add(AReader(0))
                End While

                ConnectionQuery = "select productname from menu_products where companyid ='" & LoggedInCompanyName & "'"
                MyCommand = New OleDb.OleDbCommand(ConnectionQuery, MyConn)
                AReader = MyCommand.ExecuteReader

                While AReader.Read
                    ComboBox2.Items.Add(AReader(0))
                End While


            Else
                ConnectionQuery = "select id from customer_stub where customername ='" & ComboBox1.Text & "' and companyid ='" & LoggedInCompanyName & "'"

                MyCommand = New OleDb.OleDbCommand(ConnectionQuery, MyConn)
                AReader = MyCommand.ExecuteReader

                While AReader.Read
                    CustomerId = AReader(0)
                End While

                ConnectionQuery = "select id from menu_products where productname ='" & ComboBox2.Text & "' and companyid ='" & LoggedInCompanyName & "'"

                MyCommand = New OleDb.OleDbCommand(ConnectionQuery, MyConn)
                AReader = MyCommand.ExecuteReader

                While AReader.Read
                    ProductId = AReader(0)
                End While
            End If

            AReader.Close()
            MyConn.Close()
        Catch ex As Exception
            ErrorLogger.LogError(ex, "LoadProducts")
            MsgBox("Error Occured. Please check log for more details.", MsgBoxStyle.Information, MessageTitle)
        End Try
    End Sub

    Private Sub Window1_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Window1.Loaded
        LoadCustomerAndProducts(False)
    End Sub
End Class

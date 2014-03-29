Imports System.Data

Partial Public Class Window_PSAnalysis

    Dim ProductId As String

#Region "DATABASE AND CHART CODES"

    Private Sub LoadProducts(ByVal Idgiven As Boolean)
        Try

            Dim MyConn As New OleDb.OleDbConnection(ConnString)
            Dim ConnectionQuery As String
            Dim MyCommand As OleDb.OleDbCommand = Nothing
            Dim AReader As OleDb.OleDbDataReader = Nothing

            MyConn.Open()

            If Idgiven = False Then
                ConnectionQuery = "select productname from menu_products where companyid ='" & LoggedInCompanyName & "'"
                ComboBox1.Items.Clear()

                MyCommand = New OleDb.OleDbCommand(ConnectionQuery, MyConn)
                AReader = MyCommand.ExecuteReader

                While AReader.Read
                    ComboBox1.Items.Add(AReader(0))
                End While
            Else
                ConnectionQuery = "select id from menu_products where productname ='" & ComboBox1.Text & "' and companyid ='" & LoggedInCompanyName & "'"

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

    Private Sub LoadChart()
        Try
            Dim conn As New OleDb.OleDbConnection(ConnString)
            Dim Conquery As String

            If CheckBox1.IsChecked = True Then
                Conquery = "select productname, sum(quantity) as sq from bill_detail, menu_products where id=productid"
            Else
                Conquery = "select productname, sum(quantity) as sq from bill_detail, menu_products where id=productid and productid='" & ProductId & "'"
            End If

            If CheckBox2.IsChecked = True Then
                Conquery &= " and billdate = #" & Date_Today & "# and bill_detail.companyid='" & LoggedInCompanyName & "' group by productname"
            Else
                Conquery &= " and billdate between #" & dtp_from.SelectedDate & "# and #" & dtp_to.SelectedDate & "# and bill_detail.companyid='" & LoggedInCompanyName & "' group by productname"
            End If

            Dim adapter As OleDb.OleDbDataAdapter = New OleDb.OleDbDataAdapter(Conquery, conn)
            Dim ds As New DataSet("mdset")

            conn.Open()
            adapter.Fill(ds, "bill_detail")

            MyChart.SeriesSource = ds.Tables(0).DefaultView
            MyChart.IDMemberPath = "productname"
            MyChart.TextMemberPath = "productname"

            a1.DataItemsSource = ds.Tables(0).DefaultView
            a1.SeriesIDMemberPath = "productname"
            a1.ValueMemberPath = "sq"

            ds.Dispose()
            adapter.Dispose()
        Catch ex As Exception
            ErrorLogger.LogError(ex, "LOAD CHART")
            MsgBox("Error Occured. Please check log for more details.", MsgBoxStyle.Information, MessageTitle)
        End Try
    End Sub

#End Region

    Private Sub Window1_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Window1.Loaded
        LoadProducts(False)
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button2.Click
        LoadProducts(True)
        LoadChart()
    End Sub

   
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button1.Click
        Me.Close()
    End Sub
End Class

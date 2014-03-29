Imports System.Data
Partial Public Class RawProductAddition
    Dim RawProdDets(3) As String

    Private Sub Text_prodname_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles Text_prodname.KeyDown
        Try
            If e.Key = Key.F2 Then
                Button2_Click(Button2, New RoutedEventArgs())
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub Text_prodid_GotFocus(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Text_prodid.GotFocus
        RawProdDets = ConnectionObject.GetRawProdDetails(1, Text_prodname.Text)
        Text_prodid.Text = RawProdDets(0)
        Text_unit.Text = RawProdDets(1)
        Text_quant.Focus()
    End Sub

    Private Sub Text_prodid_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles Text_prodid.KeyDown
        Try
            If e.Key = Key.Enter Then
                RawProdDets = ConnectionObject.GetRawProdDetails(0, Text_prodid.Text)
                Text_prodname.Text = RawProdDets(0)
                Text_unit.Text = RawProdDets(1)
                Text_quant.Focus()
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button1.Click
        Try
            If Not Text_quant.Text = Nothing And Not Text_prodid.Text = Nothing Then

                Temp_RawListbox.Items.Add("NAME : " & Text_prodname.Text & " | QUANT REDUCED : " & Text_quant.Text & " | ID : " & Text_prodid.Text)

                clearall()
                Text_prodname.Focus()
            Else
                MsgBox("Please enter a Quantity", MsgBoxStyle.Information, MessageTitle)
            End If
        Catch ex As Exception
            ErrorLogger.LogError(ex, Me.Content)
            MsgBox("Error Occured. Please check log for more details.", MsgBoxStyle.Information, MessageTitle)
        End Try

    End Sub

    Private Sub Text_quant_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles Text_quant.KeyDown
        If e.Key = Key.Enter Then
            Button1.Focus()
        End If
    End Sub

    Private Sub list_raw_SelectionChanged(ByVal sender As Object, ByVal e As System.Windows.Controls.SelectionChangedEventArgs) Handles list_raw.SelectionChanged
        Try
            If Not list_raw.SelectedIndex = -1 Then
                Temp_RawListbox.Items.RemoveAt(list_raw.SelectedIndex)
            End If
        Catch ex As Exception
            ErrorLogger.LogError(ex, Me.Content)
            MsgBox("Error Occured. Please check log for more details.", MsgBoxStyle.Information, MessageTitle)
        End Try
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button2.Click
        Try
            DependentsString = Nothing

            If Not list_raw.Items.Count = 0 Then

                Dim TempString1 As String
                Dim TempString2() As String
                Dim TempString3() As String
                Dim SplitChar1() As Char = {"|"}
                Dim SplitChar2() As Char = {":"}
                For index As Integer = 0 To list_raw.Items.Count - 1
                    TempString1 = list_raw.Items(index)
                    TempString2 = TempString1.Split(SplitChar1)
                    TempString3 = TempString2(2).Split(SplitChar2)
                    DependentsString &= TempString3(1).Trim
                    TempString3 = TempString2(1).Split(SplitChar2)
                    DependentsString &= ":" & TempString3(1).Trim & ";"
                Next
                Me.Close()
            Else
                If MsgBox("No RAW items defined. Continue ?", MsgBoxStyle.Exclamation + MsgBoxStyle.YesNo, MessageTitle) = MsgBoxResult.Yes Then
                    DependentsString = "NO"
                    Me.Close()
                End If
            End If
        Catch ex As Exception
            ErrorLogger.LogError(ex, Me.Title)
            MsgBox("Error Occured. Please check log for more details.", MsgBoxStyle.Information, MessageTitle)
        End Try
    End Sub

    Private Sub RawProductAddition_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        Text_prodname.Focus()
        LoadRawProductsList()
        list_raw.ItemsSource = Temp_RawListbox.Items
    End Sub

    Private Sub Text_unit_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles Text_unit.KeyDown
        If e.Key = Key.Enter Then
            Text_quant.Focus()
        End If
    End Sub


#Region "DATABASE CODES"

    Private Sub LoadRawProductsList()
        Try
            Dim MyConn As New OleDb.OleDbConnection(ConnString)
            Dim ConnectionQuery As String = "select productname from stock_stub where companyid ='" & LoggedInCompanyName & "'"
            Dim MyCommand As New OleDb.OleDbCommand(ConnectionQuery, MyConn)
            Dim AReader As OleDb.OleDbDataReader

            MyConn.Open()

            AReader = MyCommand.ExecuteReader

            While AReader.Read
                Text_prodname.Items.Add(AReader(0))
            End While

            AReader.Close()
            MyConn.Close()

        Catch ex As Exception
            ErrorLogger.LogError(ex, "FORM : RawProductAddition ; GetRawProductsList")
            MsgBox("Error Occured. Please check log for more details.", MsgBoxStyle.Information, MessageTitle)
        End Try
    End Sub

    Private Sub LoadProdDetails()
        Try
            Dim MyConn As New OleDb.OleDbConnection(ConnString)
            Dim ConnectionQuery As String = "select id, quantityunittext from stock_stub where companyid ='" & LoggedInCompanyName & "'"
            Dim MyCommand As New OleDb.OleDbCommand(ConnectionQuery, MyConn)
            Dim AReader As OleDb.OleDbDataReader

            MyConn.Open()

            AReader = MyCommand.ExecuteReader

            While AReader.Read
                Text_prodid.Text = AReader(0)
                Text_quant.Text = AReader(1)
            End While

            AReader.Close()
            MyConn.Close()

        Catch ex As Exception
            ErrorLogger.LogError(ex, "LoadRawProdDetails")
            MsgBox("Error Occured. Please check log for more details.", MsgBoxStyle.Information, MessageTitle)
        End Try
    End Sub

    Private Sub clearall()
        Text_prodname.Text = Nothing
        Text_prodid.Text = Nothing
        Text_unit.Text = Nothing
        Text_quant.Text = Nothing
    End Sub

#End Region

   
End Class


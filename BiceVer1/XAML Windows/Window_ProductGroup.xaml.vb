Imports System.Data
Imports System.Data.OleDb

Partial Public Class Window_ProductGroup
    Dim EditMode As Boolean = False
    Dim OriginalValue As Long

    Private Sub Page_ProductGroup_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded

        Try
            Combo_group.Items.Clear()
            LoadGroupValues()

            grid_list.ItemsSource = ConnectionObject.GetListofProducts.DefaultView

            Text_ProductCode.Focus()
            DependentsString = "NO"
        Catch ex As Exception
           
        End Try
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button1.Click
        Try
            If EditMode = False And HasNumber(Text_ProductCode.Text) Then
                'ADD THIS NEXT LINE IF IT IS ONLY FOR BILLING MODULE
                'DependentsString = ""
                If Text_ProductCode.Text = Nothing Or Text_prodname.Text = Nothing Or Text_ProductCost.Text = Nothing Then
                    MsgBox("Please enter the necessary values.", MsgBoxStyle.Information, MessageTitle)
                Else

                    If HasExtraSymbols(Text_ProductCode.Text) Or HasExtraSymbols(Text_prodname.Text) Or HasExtraSymbols(Text_ProductCost.Text) Then
                        MsgBox("Please remove any symbols you entered.", MsgBoxStyle.Information, MessageTitle)
                    Else
                        ConnectionObject.InsertANewProduct(Text_ProductCode.Text, Text_prodname.Text, Val(Text_ProductCost.Text), 0, _
                                                         Val(Text_Discount.Text), DependentsString, Combo_group.Text, Val(Text_vat.Text))
                        DependentsString = Nothing
                        Temp_RawListbox.Items.Clear()
                        ClearAll()
                        Text_ProductCode.Focus()
                        grid_list.ItemsSource = Nothing
                        grid_list.ItemsSource = ConnectionObject.GetListofProducts.DefaultView
                    End If
                End If

            Else
                MsgBox("Please Exit Editing Mode or please check if you have entered atleast ONE NUMBER in the Product Code. Click DONE to exit.", MsgBoxStyle.Information, MessageTitle)

            End If
        Catch ex As Exception

        End Try

    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button2.Click
        Try
            If Not Combo_group.SelectedItem = Nothing Then
                ConnectionObject.DeleteProductGroup(Combo_group.SelectedItem)
                MsgBox("Product Group Deleted.", MsgBoxStyle.Information, MessageTitle)
                Try
                    Dim Sel = Combo_group.SelectedIndex
                    Combo_group.Items.RemoveAt(Sel)
                Catch ex As Exception
                    'ANY NULL ERROR MAY OCCUR. LEAVE IT AND PROCEED.
                End Try

                grid_list.ItemsSource = ConnectionObject.GetListofProducts.DefaultView

            End If
        Catch ex As Exception
           
        End Try
    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button4.Click
        ClearAll()
    End Sub

    Private Sub button_edit_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles button_save.Click
        Try
            Dim TempString(8) As String
            TempString(0) = Text_ProductCode.Text
            TempString(1) = Text_prodname.Text
            TempString(2) = Text_ProductCost.Text
            TempString(3) = Text_Discount.Text
            TempString(4) = DependentsString
            TempString(5) = Combo_group.Text
            TempString(6) = Text_vat.Text
            ConnectionObject.EditAProduct(TempString(0), TempString(1), Val(TempString(2)), Val(TempString(3)), TempString(4), TempString(5), Val(TempString(6)))
            Temp_RawListbox.Items.Clear()
            DependentsString = Nothing
            ClearAll()
            grid_list.ItemsSource = ConnectionObject.GetListofProducts.DefaultView
        Catch ex As Exception

        End Try


    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button3.Click
        'UNCOMMENT AFTER ADDING RAW PRODUCT PAGE
        Dim RawForm As New RawProductAddition
        RawForm.Show()
    End Sub

    Private Sub Text_vat_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles Text_vat.KeyDown
        If e.Key = Key.Enter Or e.Key = Key.Tab Then
            Text_ProductCost.Text = Convert.ToString(OriginalValue + ((OriginalValue * Val(Text_vat.Text)) / 100))
            Combo_group.Focus()
        End If
    End Sub

    Private Sub Text_ProductCode_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles Text_ProductCode.KeyDown
        Try
            If e.Key = Key.Enter Then
                If ConnectionObject.CheckForProduct(Text_ProductCode.Text) = True Then
                    MsgBox("Product Id already Present.", MsgBoxStyle.Information, MessageTitle)
                    Text_ProductCode.Clear()
                    Text_ProductCode.Focus()
                Else
                    Text_prodname.Focus()
                End If
            End If
        Catch ex As Exception
           
        End Try
    End Sub

#Region "NAVIGATION"

    Private Sub Text_prodname_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles Text_prodname.KeyDown
        If e.Key = Key.Enter Then
            Text_ProductCost.Focus()

        End If
    End Sub

    Private Sub Text_ProductCost_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles Text_ProductCost.KeyDown
        If e.Key = Key.Enter Then
            Text_Discount.Focus()

        End If
        OriginalValue = Val(Text_ProductCost.Text)

    End Sub

    Private Sub Text_Discount_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles Text_Discount.KeyDown
        If e.Key = Key.Enter Then
            Text_vat.Focus()

        End If
    End Sub

    Private Sub Combo_group_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles Combo_group.KeyDown
        If e.Key = Key.Enter Then
            Button1.Focus()
        End If
    End Sub

#End Region

    Private Sub grid_list_BeginningEdit(ByVal sender As Object, ByVal e As Microsoft.Windows.Controls.DataGridBeginningEditEventArgs) Handles grid_list.BeginningEdit
        Try
            Dim SelIndex As Int16 = grid_list.SelectedIndex
            Dim TempString(7) As String
            If SelIndex >= 0 Then
                TempString = ConnectionObject.GetProductDetailsToEdit(grid_list.Items(SelIndex)(0))
                Text_ProductCode.Text = TempString(0)
                Text_prodname.Text = TempString(1)
                Text_ProductCost.Text = TempString(2)
                Text_Discount.Text = TempString(3)
                DependentsString = TempString(4)
                Combo_group.Text = TempString(5)
                Text_vat.Text = TempString(6)

                button_save.IsEnabled = True
                Button_del.IsEnabled = True
                Text_ProductCode.Focus()
            End If
        Catch ex As Exception
        End Try
    End Sub

    Private Sub Button_del_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button_del.Click
        If Not Text_ProductCode.Text = Nothing Then
            DeleteProduct()
            ClearAll()
            grid_list.ItemsSource = ConnectionObject.GetListofProducts.DefaultView
            MsgBox("Product Deleted.", MsgBoxStyle.Information, MessageTitle)
        Else
            MsgBox("Please enter the product id whose details must be updated.", MsgBoxStyle.Information, MessageTitle)
        End If
    End Sub

    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button5.Click
        Me.Close()
    End Sub

#Region "DATABASE CODES"

    Private Sub ClearAll()
        Text_Discount.Text = Nothing
        Text_ProductCode.Text = Nothing
        Text_prodname.Text = Nothing
        Text_ProductCost.Text = Nothing
        Text_vat.Text = Nothing
        Combo_group.SelectedIndex = 0
        Button_del.IsEnabled = False
        button_save.IsEnabled = False
    End Sub

    Private Sub DeleteProduct()
        Try
            Dim MyConn As New OleDb.OleDbConnection(ConnString)
            Dim ConnectionQuery As String

            ConnectionQuery = "delete from menu_products where id='" & Text_ProductCode.Text & "' and companyid='" & LoggedInCompanyName & "'"

            Dim MyCommand As New OleDb.OleDbCommand(ConnectionQuery, MyConn)

            MyConn.Open()

            MyCommand.ExecuteReader()

            MyConn.Close()
        Catch ex As Exception
            ErrorLogger.LogError(ex, "DeleteProduct")
            MsgBox("Error Occured. Please check log for more details.", MsgBoxStyle.Information, MessageTitle)
        End Try
    End Sub

    Private Sub LoadGroupValues()
        Try
            Dim MyConn As New OleDb.OleDbConnection(ConnString)
            Dim ConnectionQuery As String
            Dim Areader As System.Data.OleDb.OleDbDataReader = Nothing

            ConnectionQuery = "select groupname from menu_products where groupname not like '' and companyid='" & LoggedInCompanyName & "'"

            Dim MyCommand As New OleDb.OleDbCommand(ConnectionQuery, MyConn)

            MyConn.Open()

            Areader = MyCommand.ExecuteReader()

            While Areader.Read
                Combo_group.Items.Add(Areader(0))
            End While

            MyConn.Close()
        Catch ex As Exception
            ErrorLogger.LogError(ex, "LoadGroupValues")
            MsgBox("Error Occured. Please check log for more details.", MsgBoxStyle.Information, MessageTitle)
        End Try
    End Sub
#End Region

End Class


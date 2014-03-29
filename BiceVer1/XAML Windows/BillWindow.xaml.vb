Imports System.Data
Imports System.Windows
Partial Public Class BillWindow

#Region "Declarations"
    Dim FinalBill As New DataTable
    Dim ItemsCount As Integer = 0
    Dim Temp_ProductName As String
    Dim TotalCostWODiscount As Double
    Dim TotalCostWOBillDiscount As Double
    Dim Total_Cost As Double
    Dim CustomerDetails(5) As String
    Dim CloseFlag As Boolean = True
    Public MyTempBillNum As Integer = 0
    Public MyDBBillNum As Integer = 0
    Dim IndividualDiscountRateTotal As Integer
    Dim IndividualDiscountAmount As Integer
    Dim BillDiscountAmount As Integer

    Dim VATAddedOnce As Boolean = False
    Dim PxQ As Double
    Dim D As Double
    Dim PxQxD As Double
    Dim TempForSavings As Integer 'USED IN FINALIZING BILL FOR ADDING FINAL BILL'S DISCOUNT TO SAVINGS AMOUNT OF CUSTOMER
#End Region

    Private Sub BillPage_Initialized(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Initialized
        Try
            grid_list.ItemsSource = ConnectionObject.GetListofProducts.DefaultView

            FinalBill.Columns.Add("ITEM NO")
            FinalBill.Columns.Add("PRODUCT ID")
            FinalBill.Columns.Add("PRODUCT NAME")
            FinalBill.Columns.Add("QUANTITY")
            FinalBill.Columns.Add("PRODUCT COST")
            FinalBill.Columns.Add("TOTAL")
            FinalBill.Columns.Add("DISC RATE")
            grid_bill.ItemsSource = FinalBill.DefaultView

            Text_ProductCode.Focus()

            Text_BillNumber.Text = CurrentBillNo
            MyTempBillNum = TempBillNo
            MyDBBillNum = CurrentBillNo

            text_vat.Text = ConnectionObject.VATRATE_GetorSet(0)
            Text_KOTNumber.Text = ConnectionObject.KOTNO_GetorSet(0)

            Combo_BillType.SelectedIndex = 0
            Text_CustomerDetail.IsEnabled = False
            ALabel.IsEnabled = False

            text_bottom.Text = ConnectionObject.BillFooter_GetorSet(0)

            If InventoryObject.IsStocksLess = True Then
                label_stocknotif.Visibility = Windows.Visibility.Visible
            End If

        Catch ex As Exception
            ErrorLogger.LogError(ex, Me.Title)
            MsgBox("Error Occured. Please check log for more details.", MsgBoxStyle.Information, MessageTitle)
        End Try
    End Sub

    Private Sub BillWindow_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles Me.Closing
        Try
            If CloseFlag = True Then
                Dim MyRes As MsgBoxResult

                MyRes = MsgBox("Do you want to cancel this Bill ?", MsgBoxStyle.Information + MsgBoxStyle.YesNo, MessageTitle)

                If MyRes = MsgBoxResult.No Then
                    e.Cancel = True
                Else
                    CurrentBillNo -= 1
                End If
            End If
        Catch ex As Exception
            ErrorLogger.LogError(ex, Me.Title)
            MsgBox("Error Occured. Please check log for more details.", MsgBoxStyle.Information, MessageTitle)
        End Try
    End Sub

    Private Sub Text_ProductCode_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles Text_ProductCode.KeyDown
        Try
            Dim ProductDetails(5) As String

            If e.Key = Key.Enter Then
                ProductDetails = ConnectionObject.GetProductDetails(Text_ProductCode.Text)
                Text_ProductCode.Text = ProductDetails(0)
                Temp_ProductName = ProductDetails(1)
                Text_ProductCost.Text = ProductDetails(2)
                Text_Discount.Text = ProductDetails(3)
                Text_Quantity.Focus()
            ElseIf e.Key = Key.F2 Then
                Button3_Click(sender, New Windows.RoutedEventArgs())
            End If
        Catch ex As Exception
            ErrorLogger.LogError(ex, Me.Title)
            MsgBox("Error Occured. Please check log for more details.", MsgBoxStyle.Information, MessageTitle)
        End Try
    End Sub

    Private Sub Text_ProductCode_TextChanged(ByVal sender As Object, ByVal e As System.Windows.Controls.TextChangedEventArgs) Handles Text_ProductCode.TextChanged
        Try
            grid_list.ItemsSource = Nothing

            If Not Text_ProductCode.Text = Nothing Then
                grid_list.ItemsSource = ConnectionObject.GetListofProducts(Text_ProductCode.Text).DefaultView
            Else
                grid_list.ItemsSource = ConnectionObject.GetListofProducts.DefaultView
            End If
        Catch ex As Exception
            ErrorLogger.LogError(ex, Me.Title)
            MsgBox("Error Occured. Please check log for more details.", MsgBoxStyle.Information, MessageTitle)
        End Try
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button1.Click
        Try
            If Not Text_Quantity.Text = Nothing And Not Text_ProductCode.Text = Nothing And Not Text_Discount.Text = Nothing And Not Text_Total.Text = Nothing And Not Text_ProductCost.Text = Nothing Then
                FinalBill.Rows.Add(ItemsCount, Text_ProductCode.Text, Temp_ProductName, Text_Quantity.Text, Text_ProductCost.Text, Text_Total.Text, Text_Discount.Text)

                ItemsCount += Val(Text_Quantity.Text)
                Text_ItemCount.Text = ItemsCount

                TotalCostWOBillDiscount += Val(Text_Total.Text)
                Total_Cost = TotalCostWOBillDiscount - (TotalCostWOBillDiscount * (Val(Text_billdiscount.Text) / 100))
                Total_Cost = Math.Round(Total_Cost, MidpointRounding.AwayFromZero)

                label_total.Content = "Rs. " & Total_Cost & " /-"

                IndividualDiscountAmount += PxQxD
                ClearAllTextBoxes()
            Else
                MsgBox("Please enter the necessary details.", MsgBoxStyle.Information, MessageTitle)
            End If
        Catch ex As Exception
            ErrorLogger.LogError(ex, Me.Title)
            MsgBox("Error Occured. Please check log for more details.", MsgBoxStyle.Information, MessageTitle)
        End Try
    End Sub

    Private Sub Text_Quantity_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles Text_Quantity.KeyDown
        Try
            If e.Key = Key.Enter Then
                Button1.Focus()
                PxQ = Val(Text_ProductCost.Text) * Val(Text_Quantity.Text)
                D = Val(Text_Discount.Text) / 100
                PxQxD = (PxQ) * D
                Text_Total.Text = (PxQ) - (PxQxD)
            End If
        Catch ex As Exception
            ErrorLogger.LogError(ex, Me.Title)
            MsgBox("Error Occured. Please check log for more details.", MsgBoxStyle.Information, MessageTitle)
        End Try
    End Sub

    Private Sub ClearAllTextBoxes()
        Text_ProductCode.Text = Nothing
        Text_ProductCost.Text = Nothing
        Text_Quantity.Text = Nothing
        Text_Discount.Text = Nothing
        Text_Total.Text = Nothing
        Text_ProductCode.Focus()
    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button4.Click
        ClearAllTextBoxes()
    End Sub

    Private Sub Combo_BillType_SelectionChanged(ByVal sender As Object, ByVal e As System.Windows.Controls.SelectionChangedEventArgs) Handles Combo_BillType.SelectionChanged
        If Combo_BillType.SelectedIndex = 1 Then
            ALabel.IsEnabled = True
            Text_CustomerDetail.IsEnabled = True
            Text_CustomerDetail.Focus()
        Else
            ALabel.IsEnabled = False
            Text_CustomerDetail.IsEnabled = False
            Text_ProductCode.Focus()
        End If
    End Sub

    Private Sub text_vat_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles text_vat.KeyDown
        Try
            If e.Key = Key.Enter Then
                ConnectionObject.VATRATE_GetorSet(1, Val(text_vat.Text))
                MsgBox("Changed Successfully", MsgBoxStyle.Information, MessageTitle)
            End If
        Catch ex As Exception
            ErrorLogger.LogError(ex, Me.Title)
            MsgBox("Error Occured. Please check log for more details.", MsgBoxStyle.Information, MessageTitle)
        End Try
    End Sub

    Private Sub Text_KOTNumber_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles Text_KOTNumber.KeyDown
        Try
            If e.Key = Key.Enter Then
                ConnectionObject.KOTNO_GetorSet(1, Text_KOTNumber.Text)
                MsgBox("Changed Successfully", MsgBoxStyle.Information, MessageTitle)
            End If
        Catch ex As Exception
            ErrorLogger.LogError(ex, Me.Title)
            MsgBox("Error Occured. Please check log for more details.", MsgBoxStyle.Information, MessageTitle)
        End Try

    End Sub

    Private Sub grid_bill_BeginningEdit(ByVal sender As Object, ByVal e As Microsoft.Windows.Controls.DataGridBeginningEditEventArgs) Handles grid_bill.BeginningEdit
        Try
            Dim MessageResult As MsgBoxResult
            Dim ItemIndexValue As Integer = grid_bill.SelectedIndex

            If ItemIndexValue >= 0 Then
                MessageResult = MsgBox("Delete the Selected Item from the bill ? ", MsgBoxStyle.Information + MsgBoxStyle.YesNo, MessageTitle)
                If MessageResult = MsgBoxResult.Yes Then
                    Dim Temp_CostToSubtract As Double = FinalBill.Rows(ItemIndexValue).Item(5)
                    TotalCostWOBillDiscount -= Temp_CostToSubtract

                    Total_Cost = TotalCostWOBillDiscount - (TotalCostWOBillDiscount * (Val(Text_billdiscount.Text) / 100))
                    Total_Cost = Math.Round(Total_Cost, MidpointRounding.AwayFromZero)

                    If Total_Cost >= 1 Then
                        Total_Cost = Math.Round(Total_Cost, MidpointRounding.AwayFromZero)
                    Else
                        Total_Cost = 0
                    End If

                    label_total.Content = "Rs. " & Total_Cost & " /-"
                    ItemsCount -= Val(FinalBill.Rows(ItemIndexValue).Item(3))
                    Text_ItemCount.Text = ItemsCount

                    IndividualDiscountRateTotal -= FinalBill.Rows(ItemIndexValue).Item(6)
                    IndividualDiscountAmount -= (Val(FinalBill.Rows(ItemIndexValue).Item(3)) * FinalBill.Rows(ItemIndexValue).Item(4)) * (FinalBill.Rows(ItemIndexValue).Item(6) / 100)
                    TotalCostWODiscount -= Val(FinalBill.Rows(ItemIndexValue).Item(4)) * Val(FinalBill.Rows(ItemIndexValue).Item(3))

                    FinalBill.Rows.RemoveAt(ItemIndexValue)

                End If
            End If

            Text_ProductCode.Focus()
        Catch ex As Exception
            ErrorLogger.LogError(ex, Me.Title)
            MsgBox("Error Occured. Please check log for more details.", MsgBoxStyle.Information, MessageTitle)
        End Try
    End Sub

    Private Sub Text_CustomerDetail_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles Text_CustomerDetail.KeyDown
        Try
            If e.Key = Key.Enter Then
                If Not (Text_CustomerDetail.Text = Nothing) Then

                    CustomerDetails = ConnectionObject.GetCustomerDetails(Text_CustomerDetail.Text)

                    label_id.Content = "ID : " & CustomerDetails(0)
                    label_name.Content = "NAME : " & CustomerDetails(1)
                    label_phone.Content = "PHONE : " & CustomerDetails(2)
                    label_dob.Content = "DOB : " & CustomerDetails(3)
                    label_email.Content = "MAIL : " & CustomerDetails(4)
                    label_address.Content = "ADDRESS : " & CustomerDetails(5)


                End If

                Text_ProductCode.Focus()

            End If
        Catch ex As Exception
            ErrorLogger.LogError(ex, Me.Title)
            MsgBox("Error Occured. Please check log for more details.", MsgBoxStyle.Information, MessageTitle)
        End Try
    End Sub

    Private Sub Button1_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles Button1.KeyDown
        If e.Key = Key.Down Then
            Button3.Focus()
        End If
    End Sub

    Private Sub Text_billdiscount_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles Text_billdiscount.KeyDown
        Try
            If e.Key = Key.Enter And Not (Text_billdiscount.Text = Nothing) Then
                Total_Cost = TotalCostWOBillDiscount - (TotalCostWOBillDiscount * (Val(Text_billdiscount.Text) / 100))
                Total_Cost = Math.Round(Total_Cost, MidpointRounding.AwayFromZero)
                TempForSavings = TotalCostWOBillDiscount * (Val(Text_billdiscount.Text) / 100)

                If Not text_vat.Text = Nothing Then
                    Total_Cost = Total_Cost + (Total_Cost * (Val(text_vat.Text) / 100))
                    Total_Cost = Math.Round(Total_Cost, MidpointRounding.AwayFromZero)

                End If

                label_total.Content = "Rs. " & Total_Cost & " /-"
                Text_amountgiven.Focus()
            End If
        Catch ex As Exception
            ErrorLogger.LogError(ex, Me.Title)
            MsgBox("Error Occured. Please check log for more details.", MsgBoxStyle.Information, MessageTitle)
        End Try
    End Sub

    Private Sub Text_amountgiven_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles Text_amountgiven.KeyDown
        If e.Key = Key.Enter And Not Text_amountgiven.Text = Nothing Then
            Text_amountreturned.Text = Val(Text_amountgiven.Text) - Val(Total_Cost)
            Button2.Focus()
        End If

    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button3.Click
        Try
            If MsgBox("Finalize this Bill with VAT Percentage : " & text_vat.Text & " %", MsgBoxStyle.Information + MsgBoxStyle.YesNo, MessageTitle) = MsgBoxResult.Yes Then
                If FinalBill.Rows.Count > 0 And VATAddedOnce = False Then
                    Total_Cost = TotalCostWOBillDiscount + (TotalCostWOBillDiscount * (Val(text_vat.Text) / 100))
                    Total_Cost = Math.Round(Total_Cost, MidpointRounding.AwayFromZero)

                    If Not Text_billdiscount.Text = Nothing Then
                        Total_Cost = Total_Cost - (Total_Cost * (Val(Text_billdiscount.Text) / 100))
                        Total_Cost = Math.Round(Total_Cost, MidpointRounding.AwayFromZero)
                    End If

                    label_total.Content = "Rs. " & Total_Cost & " /-"
                    VATAddedOnce = True
                    Text_billdiscount.Focus()
                    Button3.IsEnabled = False
                Else
                    If Not FinalBill.Rows.Count > 0 Then
                        MsgBox("Please Add some items to the Bill.", MsgBoxStyle.Exclamation, MessageTitle)
                        Text_ProductCode.Focus()
                    End If
                End If
            End If
        Catch ex As Exception
            ErrorLogger.LogError(ex, Me.Title)
            MsgBox("Error Occured. Please check log for more details.", MsgBoxStyle.Information, MessageTitle)
        End Try
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button2.Click
        Try
            IndividualDiscountAmount += TempForSavings

            If FinalBill.Rows.Count > 0 Then

                If Text_billdiscount.Text = Nothing Then
                    Text_billdiscount.Text = "0"
                End If

                EnterBillDetails()
                EnterInventoryDetails()


                CurrentBillNo += 1
                CloseFlag = False
                Me.Close()

            Else

                MsgBox("Please Add some items.", MsgBoxStyle.Exclamation, MessageTitle)

            End If
        Catch ex As Exception
            ErrorLogger.LogError(ex, Me.Title)
            MsgBox("Error Occured. Please check log for more details.", MsgBoxStyle.Information, MessageTitle)
        End Try
    End Sub

    Private Sub Run_MouseEnter(ByVal sender As System.Object, ByVal e As System.Windows.Input.MouseEventArgs)
        popLink.IsOpen = True
    End Sub

    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button5.Click
        TheListBox.Items.Add(MyTempBillNum & "   : BILL NUMBER : " & Val(Text_BillNumber.Text))
        Me.Hide()
    End Sub

    'FUNCTIONS USED
    ''' <summary>
    ''' Enters all details in the tables all_entries and bill_details respectively. Checks for credit and cash also.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub EnterBillDetails()
        Dim Temp As Integer = ConnectionObject.GetCurrentBillNumber + 1
        Dim RupinWord As New RupeeConverter

        If Combo_BillType.SelectedIndex = 0 Then

            ConnectionObject.CreateABill_AllEntries("CASH", Temp, Date_Today, Total_Cost, Val(text_vat.Text), Val(Text_billdiscount.Text), Val(Text_amountgiven.Text), "NO", LoggedInUserId, LoggedInCompanyName)
            ConnectionObject.CreateABill_BillDetail(Temp, "CASH", FinalBill)

            If ConnectionObject2.GetPrintingSize = 1 Then
                Dim BillPrinterObject As New BillPrinterModule("CASH", FinalBill, New System.Drawing.Size(250, ((FinalBill.Rows.Count * 20) + 300)), Text_billdiscount.Text, IndividualDiscountAmount, CustomerDetails(1), CustomerDetails(5))
                BillPrinterObject.SetDetails(Temp, Val(Text_ItemCount.Text), text_bottom.Text, Total_Cost, RupinWord.AmtInWord(Val(Total_Cost)), Text_KOTNumber.Text, text_vat.Text)
                BillPrinterObject.PrintYouBastard()
            ElseIf ConnectionObject2.GetPrintingSize = 2 Then
                Dim BillPrinterObject As New BillPrinterSetting2("CASH", FinalBill, New System.Drawing.Size(420, ((FinalBill.Rows.Count * 20) + 400)), Text_billdiscount.Text, IndividualDiscountAmount, CustomerDetails(1), CustomerDetails(5))
                BillPrinterObject.SetDetails(Temp, Val(Text_ItemCount.Text), text_bottom.Text, Total_Cost, RupinWord.AmtInWord(Val(Total_Cost)), Text_KOTNumber.Text, text_vat.Text)
                BillPrinterObject.PrintYouBastard()
            End If

        Else

            If Not CustomerDetails(0) = Nothing Then 'ACTUALLY CHECK FOR CUSTOMERDETAILS(0) FOR ONLY BILLING
                ConnectionObject.CreateABill_AllEntries("CREDIT", Temp, Date_Today, Total_Cost, Val(text_vat.Text), Val(Text_billdiscount.Text), Val(Text_amountgiven.Text), "YES", LoggedInUserId, LoggedInCompanyName)
                ConnectionObject.CreateABill_BillDetail(Temp, "CREDIT", FinalBill)
                ConnectionObject.CreateABill_PendingCredit(Temp, Date_Today, Total_Cost, CustomerDetails(0))

                If ConnectionObject2.GetPrintingSize = 1 Then
                    Dim BillPrinterObject As New BillPrinterModule("CREDIT", FinalBill, New System.Drawing.Size(250, ((FinalBill.Rows.Count * 20) + 380)), Text_billdiscount.Text, IndividualDiscountAmount, Text_CustomerDetail.Text, "") 'REPLACE TEXT_CUST WITH CUSTOMERDETAILS(0) AND '' WITH (5)
                    BillPrinterObject.SetDetails(Temp, Val(Text_ItemCount.Text), text_bottom.Text, Total_Cost, RupinWord.AmtInWord(Val(Total_Cost)), Text_KOTNumber.Text, text_vat.Text)
                    BillPrinterObject.PrintYouBastard()
                ElseIf ConnectionObject2.GetPrintingSize = 2 Then
                    Dim BillPrinterObject As New BillPrinterSetting2("CREDIT", FinalBill, New System.Drawing.Size(250, ((FinalBill.Rows.Count * 20) + 480)), Text_billdiscount.Text, IndividualDiscountAmount, Text_CustomerDetail.Text, "") 'REPLACE TEXT_CUST WITH CUSTOMERDETAILS(0) AND '' WITH (5)
                    BillPrinterObject.SetDetails(Temp, Val(Text_ItemCount.Text), text_bottom.Text, Total_Cost, RupinWord.AmtInWord(Val(Total_Cost)), Text_KOTNumber.Text, text_vat.Text)
                    BillPrinterObject.PrintYouBastard()
                End If

                CustomerObject.UpdateCustomerDetails(CustomerDetails(0))
                CustomerObject.UpdateCustomerBuyingHistory(CustomerDetails(0), FinalBill, Temp)

            Else
                MsgBox("Please enter a Customer Name / Phone Number for the Credit Bill", MsgBoxStyle.Information, MessageTitle)
            End If


        End If
    End Sub

    ''' <summary>
    ''' Enters all details in the tables for inventory stocks_stub and checks for less availability.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub EnterInventoryDetails()

        For index As Integer = 0 To FinalBill.Rows.Count - 1
            InventoryObject.ReduceQuantity(FinalBill.Rows(index)(1), FinalBill.Rows(index)(3))
        Next

        If InventoryObject.IsStocksLess = True Then
            label_stocknotif.Visibility = Windows.Visibility.Visible
        End If

    End Sub


    Private Sub Button6_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button6.Click
        Me.Close()
    End Sub

   
    Private Sub Button7_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button7.Click
        Try
            Dim TheString As String = InputBox("Enter the new footer for the bill. 40 Characters Max", MessageTitle, text_bottom.Text)
            If Not TheString = Nothing Then
                ConnectionObject.BillFooter_GetorSet(1, TheString)
                text_bottom.Text = TheString
            End If
        Catch ex As Exception
            ErrorLogger.LogError(ex, Me.Title)
            MsgBox("Error Occured. Please check log for more details.", MsgBoxStyle.Information, MessageTitle)
        End Try
    End Sub
End Class

Imports System.Data

Partial Public Class Window_ReportsByDate

    Dim printtype As Int16
    Dim FromD As String
    Dim ToD As String


    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button3.Click
        Try
            printtype = 0
            ReportTable1 = New DataTable()
            ReportTable2 = New DataTable()

            ReportTable1.Columns.Add("ITEM NAME")
            ReportTable1.Columns.Add("QUANTITY SOLD")

            ReportTable2.Columns.Add("BILL NO")
            ReportTable2.Columns.Add("TYPE")
            ReportTable2.Columns.Add("VALUE")

            Dim SelectedDate As String = dtp_on.SelectedDate.Value.Month & "-" & dtp_on.SelectedDate.Value.Day & "-" & dtp_on.SelectedDate.Value.Year
            Dim StrArray() As String = ConnectionObject2.GetReportDetails(SelectedDate)

            Label_cashnum.Content = "TOTAL CASH BILLS :" & "             " & StrArray(0)
            Label_creditnum.Content = "TOTAL CREDIT BILLS :" & "          " & StrArray(1)
            Label_totalrev1.Content = "TOTAL REVENUE :" & "                 " & "Rs. " & StrArray(2)
            Label_totalrev2.Content = "TOTAL REVENUE :" & "                 " & "Rs. " & StrArray(3)
            Label_billno.Content = "CURRENT BILL NUMBER :" & "     " & StrArray(4)

            grid_prod.ItemsSource = ReportTable1.DefaultView
            grid_rev.ItemsSource = ReportTable2.DefaultView
        Catch ex As Exception
            ErrorLogger.LogError(ex, Me.Title)
            MsgBox("Error Occured. Please check log for more details.", MsgBoxStyle.Information, MessageTitle)
        End Try
    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button4.Click
        Try
            printtype = 1
            ReportTable1 = New DataTable()
            ReportTable2 = New DataTable()

            ReportTable1.Columns.Add("ITEM NAME")
            ReportTable1.Columns.Add("QUANTITY SOLD")

            ReportTable2.Columns.Add("BILL NO")
            ReportTable2.Columns.Add("TYPE")
            ReportTable2.Columns.Add("VALUE")

            Dim SelectedDate1 As String = dtp_from.SelectedDate.Value.Month & "-" & dtp_from.SelectedDate.Value.Day & "-" & dtp_from.SelectedDate.Value.Year
            Dim SelectedDate2 As String = dtp_To.SelectedDate.Value.Month & "-" & dtp_To.SelectedDate.Value.Day & "-" & dtp_To.SelectedDate.Value.Year

            FromD = SelectedDate1
            ToD = SelectedDate2

            Dim StrArray() As String = ConnectionObject2.GetReportDetailsBetweenDates(SelectedDate1, SelectedDate2)

            Label_cashnum.Content = "TOTAL CASH BILLS :" & "             " & StrArray(0)
            Label_creditnum.Content = "TOTAL CREDIT BILLS :" & "          " & StrArray(1)
            Label_totalrev1.Content = "TOTAL REVENUE :" & "                 " & "Rs. " & StrArray(2)
            Label_totalrev2.Content = "TOTAL REVENUE :" & "                 " & "Rs. " & StrArray(3)
            Label_billno.Content = "CURRENT BILL NUMBER :" & "     " & StrArray(4)


            grid_prod.ItemsSource = ReportTable1.DefaultView
            grid_rev.ItemsSource = ReportTable2.DefaultView
        Catch ex As Exception
            ErrorLogger.LogError(ex, Me.Title)
            MsgBox("Error Occured. Please check log for more details.", MsgBoxStyle.Information, MessageTitle)
        End Try
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button1.Click
        Try
            Dim ReportPrinting As ReportPrinterModule
            Dim ReportPrinting2 As ReportPrinterModule2

            If printtype = 0 Then
                Date_Today = dtp_on.SelectedDate.Value.Month & "-" & dtp_on.SelectedDate.Value.Day & "-" & dtp_on.SelectedDate.Value.Year

                If ConnectionObject2.GetPrintingSize = 1 Then
                    ReportPrinting = New ReportPrinterModule(0, 0)
                    ReportPrinting.PrintTheReport1()
                ElseIf ConnectionObject2.GetPrintingSize = 2 Then
                    ReportPrinting2 = New ReportPrinterModule2(0, 0)
                    ReportPrinting2.PrintTheReport1()
                End If
            Else
                If ConnectionObject2.GetPrintingSize = 1 Then
                    ReportPrinting = New ReportPrinterModule(0, 1)
                    ReportPrinting.SetDates(FromD, ToD)
                    ReportPrinting.PrintTheReport1()
                ElseIf ConnectionObject2.GetPrintingSize = 2 Then
                    ReportPrinting2 = New ReportPrinterModule2(0, 1)
                    ReportPrinting2.SetDates(FromD, ToD)
                    ReportPrinting2.PrintTheReport1()
                End If
            End If


        Catch ex As Exception
            ErrorLogger.LogError(ex, Me.Title)
            MsgBox("Error Occured. Please check log for more details.", MsgBoxStyle.Information, MessageTitle)
        End Try
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button2.Click
        Try
            Dim ReportPrinting As ReportPrinterModule
            Dim ReportPrinting2 As ReportPrinterModule2

            If printtype = 0 Then
                Date_Today = dtp_on.SelectedDate.Value.Month & "-" & dtp_on.SelectedDate.Value.Day & "-" & dtp_on.SelectedDate.Value.Year

                If ConnectionObject2.GetPrintingSize = 1 Then
                    ReportPrinting = New ReportPrinterModule(1, 0)
                    ReportPrinting.PrintTheReport2()
                ElseIf ConnectionObject2.GetPrintingSize = 2 Then
                    ReportPrinting2 = New ReportPrinterModule2(1, 0)
                    ReportPrinting2.PrintTheReport2()
                End If
            Else
                If ConnectionObject2.GetPrintingSize = 1 Then
                    ReportPrinting = New ReportPrinterModule(1, 1)
                    ReportPrinting.SetDates(FromD, ToD)
                    ReportPrinting.PrintTheReport2()
                ElseIf ConnectionObject2.GetPrintingSize = 2 Then
                    ReportPrinting2 = New ReportPrinterModule2(1, 1)
                    ReportPrinting2.SetDates(FromD, ToD)
                    ReportPrinting2.PrintTheReport2()
                End If
            End If


        Catch ex As Exception
            ErrorLogger.LogError(ex, Me.Title)
            MsgBox("Error Occured. Please check log for more details.", MsgBoxStyle.Information, MessageTitle)
        End Try
    End Sub

    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button5.Click
        Me.Close()
    End Sub
End Class

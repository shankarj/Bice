Imports System.Data

Partial Public Class Window_MonthlyReport

    Dim FromD As String
    Dim ToD As String

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button4.Click
        Try


            Dim LastDay As Int16 = 0
            Dim SelMonth As Int16 = 1

            If Not ComboBox1.SelectedIndex = -1 Then
                LastDay = Date.DaysInMonth(Now.Year, ComboBox1.SelectedIndex + 1)
                SelMonth = ComboBox1.SelectedIndex + 1
            End If


            ReportTable1 = New DataTable()
            ReportTable2 = New DataTable()

            ReportTable1.Columns.Add("ITEM NAME")
            ReportTable1.Columns.Add("QUANTITY SOLD")

            ReportTable2.Columns.Add("BILL NO")
            ReportTable2.Columns.Add("TYPE")
            ReportTable2.Columns.Add("VALUE")

            Dim SelectedDate1 As String = SelMonth & "-" & 1 & "-" & Now.Year
            Dim SelectedDate2 As String = SelMonth & "-" & LastDay & "-" & Now.Year

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
            If ConnectionObject2.GetPrintingSize = 1 Then
                Dim ReportPrinting As ReportPrinterModule
                ReportPrinting = New ReportPrinterModule(0, 1)
                ReportPrinting.SetDates(FromD, ToD)
                ReportPrinting.PrintTheReport1()
            ElseIf ConnectionObject2.GetPrintingSize = 2 Then
                Dim ReportPrinting As ReportPrinterModule2
                ReportPrinting = New ReportPrinterModule2(0, 1)
                ReportPrinting.SetDates(FromD, ToD)
                ReportPrinting.PrintTheReport1()
            End If
        Catch ex As Exception
            ErrorLogger.LogError(ex, Me.Title)
            MsgBox("Error Occured. Please check log for more details.", MsgBoxStyle.Information, MessageTitle)
        End Try
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button2.Click
        Try
            If ConnectionObject2.GetPrintingSize = 1 Then
                Dim ReportPrinting As ReportPrinterModule
                ReportPrinting = New ReportPrinterModule(1, 1)
                ReportPrinting.SetDates(FromD, ToD)
                ReportPrinting.PrintTheReport2()
            ElseIf ConnectionObject2.GetPrintingSize = 2 Then
                Dim ReportPrinting As ReportPrinterModule2
                ReportPrinting = New ReportPrinterModule2(1, 1)
                ReportPrinting.SetDates(FromD, ToD)
                ReportPrinting.PrintTheReport2()
            End If
        Catch ex As Exception
            ErrorLogger.LogError(ex, Me.Title)
            MsgBox("Error Occured. Please check log for more details.", MsgBoxStyle.Information, MessageTitle)
        End Try
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button3.Click
        Me.Close()
    End Sub
End Class
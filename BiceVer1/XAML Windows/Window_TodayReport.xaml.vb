Imports System.Data
Partial Public Class Window_TodayReport

    Private Sub Window1_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Window1.Loaded
        Dim t As New System.Windows.Media.ImageSourceConverter
        Image1.Source = t.ConvertFromString(Environment.CurrentDirectory & "\Resources\basic.png")

        Try
            ReportTable1 = New DataTable()
            ReportTable2 = New DataTable()

            ReportTable1.Columns.Add("ITEM NAME")
            ReportTable1.Columns.Add("QUANTITY SOLD")

            ReportTable2.Columns.Add("BILL NO")
            ReportTable2.Columns.Add("TYPE")
            ReportTable2.Columns.Add("VALUE")

            Dim StrArray() As String = ConnectionObject2.GetReportDetails(Date_Today)

            Label_cashnum.Content &= "             " & StrArray(0)
            Label_creditnum.Content &= "          " & StrArray(1)
            Label_totalrev1.Content &= "                 " & "Rs. " & StrArray(2)
            Label_totalrev2.Content &= "                 " & "Rs. " & StrArray(3)
            Label_billno.Content &= "     " & StrArray(4)

            grid_prod.ItemsSource = ReportTable1.DefaultView
            grid_rev.ItemsSource = ReportTable2.DefaultView
        Catch ex As Exception
            ErrorLogger.LogError(ex, Me.Title)
            MsgBox("Error Occured. Please check log for more details.", MsgBoxStyle.Information, MessageTitle)
        End Try
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button1.Click
        If ConnectionObject2.GetPrintingSize = 1 Then
            Dim ReportPrinting As New ReportPrinterModule(0, 0)
            ReportPrinting.PrintTheReport1()
        ElseIf ConnectionObject2.GetPrintingSize = 2 Then
            Dim ReportPrinting As New ReportPrinterModule2(0, 0)
            ReportPrinting.PrintTheReport1()
        End If
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button2.Click
        If ConnectionObject2.GetPrintingSize = 1 Then
            Dim ReportPrinting As New ReportPrinterModule(1, 0)
            ReportPrinting.PrintTheReport2()
        ElseIf ConnectionObject2.GetPrintingSize = 2 Then
            Dim ReportPrinting As New ReportPrinterModule2(1, 0)
            ReportPrinting.PrintTheReport2()
        End If
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button3.Click
        Me.Close()
    End Sub
End Class

Imports System.Data
Imports System.Drawing

Public Class ReportPrinterModule2

    Dim WithEvents ReportPrinterDocument1 As New Printing.PrintDocument
    Dim WithEvents ReportPrinterDocument2 As New Printing.PrintDocument
    Dim DatePrintType As Int16
    Dim FromD As String
    Dim ToD As String

    Public Sub New(ByVal PrintType As Int16, ByVal DateType As Int16)
        DatePrintType = DateType
        If PrintType = 0 Then
            ReportPrinterDocument1.DefaultPageSettings.PaperSize = New Printing.PaperSize("Bill", 420, ((ReportTable1.Rows.Count * 20) + 400))
        Else
            ReportPrinterDocument2.DefaultPageSettings.PaperSize = New Printing.PaperSize("Bill", 420, ((ReportTable2.Rows.Count * 20) + 400))
        End If
    End Sub

    Public Sub SetDates(ByVal FromDate As String, ByVal ToDate As String)
        FromD = FromDate
        ToD = ToDate
    End Sub

    Public Sub PrintTheReport1()
        ReportPrinterDocument1.Print()
    End Sub

    Public Sub PrintTheReport2()
        ReportPrinterDocument2.Print()
    End Sub

    Private Sub ReportPrinterDocument1_PrintPage(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles ReportPrinterDocument1.PrintPage
        Try
            Dim f() As Single = {2, 2}

            Dim j As New Font("Tahoma", 12)
            Dim endf As New Font("Tahoma", 10)
            Dim ending As New Font("Tahoma", 12, FontStyle.Bold)
            Dim y As Int16 = 10
            Dim i As Int16 = 0
            Dim h As New Pen(Color.Black, 2)
            h.DashPattern = f

            e.Graphics.DrawString(LoggedInCompanyName, New Font("Tahoma", 12, FontStyle.Bold), Brushes.Black, (e.PageBounds.Size.Width / 2) - (LoggedInCompanyName.Length * 5), 10)

            If DatePrintType = 0 Then
                e.Graphics.DrawString("DATE : " & Date_Today, New Font("Tahoma", 10, FontStyle.Bold), Brushes.Black, 10, 35)
                Date_Today = Now.Month & "-" & Now.Day & "-" & Now.Year
            Else
                e.Graphics.DrawString("FROM : " & FromD, New Font("Tahoma", 10, FontStyle.Bold), Brushes.Black, 10, 35)
                e.Graphics.DrawString("TO : " & ToD, New Font("Tahoma", 10, FontStyle.Bold), Brushes.Black, e.PageBounds.Size.Width - 130, 35)
            End If


            e.Graphics.DrawString("PRODUCT SALES REPORT", New Font("Tahoma", 12, FontStyle.Bold), Brushes.Black, 120, 60)

            e.Graphics.DrawLine(h, 10, 90, e.PageBounds.Width - 10, 90)
            e.Graphics.DrawString("    ITEMNAME                                        QTY SOLD", New Font("Tahoma", 11, FontStyle.Bold), Brushes.Black, 10, 95)
            e.Graphics.DrawLine(h, 10, 120, e.PageBounds.Width - 10, 120)

            y = 130

            While i < ReportTable1.Rows.Count
                e.Graphics.DrawString(Convert.ToString(ReportTable1.Rows(i)(0)), j, Brushes.Black, 35, y)
                e.Graphics.DrawString(Convert.ToString(ReportTable1.Rows(i)(1)), j, Brushes.Black, e.PageBounds.Width - 100, y)

                i += 1
                y += 25
            End While

            i = 0
            Dim tot As Integer
            e.Graphics.DrawLine(h, 10, y, e.PageBounds.Width - 10, y)
            While i < ReportTable2.Rows.Count
                tot += Val(ReportTable2.Rows(i)(2))
                i += 1
            End While
            y += 10
            e.Graphics.DrawString("TOTAL VALUE : Rs. " & tot & " /-", New Font("Tahoma", 12, FontStyle.Bold), Brushes.Black, 10, y)

        Catch ex As Exception

            ErrorLogger.LogError(ex, "REPORTPRINTING1")
            MsgBox("Error Occured. Please check log for more details.", MsgBoxStyle.Information, MessageTitle)


        End Try
    End Sub

    Private Sub ReportPrinterDocument2_PrintPage(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles ReportPrinterDocument2.PrintPage
        Dim tot As Integer

        Try
            Dim f() As Single = {2, 2}

            Dim j As New Font("Tahoma", 12)
            Dim endf As New Font("Tahoma", 10)
            Dim ending As New Font("Tahoma", 12, FontStyle.Bold)
            Dim y As Int16 = 10
            Dim i As Int16 = 0
            Dim h As New Pen(Color.Black, 2)
            h.DashPattern = f

            e.Graphics.DrawString(LoggedInCompanyName, New Font("Tahoma", 12, FontStyle.Bold), Brushes.Black, (e.PageBounds.Size.Width / 2) - (LoggedInCompanyName.Length * 5), 10)

            If DatePrintType = 0 Then
                e.Graphics.DrawString("DATE : " & Date_Today, New Font("Tahoma", 10, FontStyle.Bold), Brushes.Black, 10, 35)
                Date_Today = Now.Month & "-" & Now.Day & "-" & Now.Year
            Else
                e.Graphics.DrawString("FROM : " & FromD, New Font("Tahoma", 10, FontStyle.Bold), Brushes.Black, 10, 35)
                e.Graphics.DrawString("TO : " & ToD, New Font("Tahoma", 10, FontStyle.Bold), Brushes.Black, e.PageBounds.Size.Width - 130, 35)
            End If

            e.Graphics.DrawString("REVENUE REPORT", New Font("Tahoma", 12, FontStyle.Bold), Brushes.Black, 120, 60)

            e.Graphics.DrawLine(h, 10, 90, e.PageBounds.Width - 10, 90)
            e.Graphics.DrawString("    BILLNO                 BILLTYPE                 VALUE", New Font("Tahoma", 11, FontStyle.Bold), Brushes.Black, 10, 95)
            e.Graphics.DrawLine(h, 10, 120, e.PageBounds.Width - 10, 120)

            y = 130


            While i < ReportTable2.Rows.Count

                e.Graphics.DrawString(Convert.ToString(ReportTable2.Rows(i)(0)), j, Brushes.Black, 35, y)
                e.Graphics.DrawString(Convert.ToString(ReportTable2.Rows(i)(1)), j, Brushes.Black, 160, y)
                e.Graphics.DrawString("Rs. " & Convert.ToString(ReportTable2.Rows(i)(2)), j, Brushes.Black, e.PageBounds.Width - 100, y)

                tot += Val(ReportTable2.Rows(i)(2))
                i += 1
                y += 25

            End While

            y += 10

            e.Graphics.DrawLine(h, 10, y, e.PageBounds.Width - 10, y)

            y += 25
            e.Graphics.DrawString("TOTAL VALUE : Rs. " & tot & " /-", New Font("Tahoma", 12, FontStyle.Bold), Brushes.Black, 10, y)


        Catch ex As Exception
            ErrorLogger.LogError(ex, "REPORTPRINTING2")
            MsgBox("Error Occured. Please check log for more details.", MsgBoxStyle.Information, MessageTitle)
        End Try
    End Sub

End Class

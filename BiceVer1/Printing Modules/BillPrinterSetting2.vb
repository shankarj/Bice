Imports System.Data
Imports System.Drawing

'10.4 CMS
'27.05 INCHES


Public Class BillPrinterSetting2

#Region "DECLARATIONS"
    Dim Type As String = Nothing
    Dim WithEvents BillPrinterDocument As Printing.PrintDocument
    Dim TheTable As DataTable = Nothing
    Dim TheSize As Size = Nothing
    Dim BillNumber As Integer = 1
    Dim TotalItems As Integer = 10
    Dim Footer As String = Nothing
    Dim AmountinNum As Integer = Nothing
    Dim AmountText As String = Nothing
    Dim KOT As String = Nothing
    Dim VATVAL As String = Nothing
    Dim DiscountValue As String = Nothing
    Dim SavingAmount As String = Nothing
    '------------------------------
    Dim CompanyPrintLine As String = Nothing
    Dim CompanyTag As String = Nothing
    Dim CompanyAddr As String = Nothing
    Dim PhoneNum As String = Nothing
    Dim Website As String = Nothing
    Dim CustomerName As String = Nothing
    Dim Address As String = Nothing
#End Region


    Public Sub New(ByVal BillType As String, ByVal TheTableToPrint As DataTable, ByVal PageSize As Size, Optional ByVal Discount As String = "0", Optional ByVal Savings As String = "0", Optional ByVal CName As String = Nothing, Optional ByVal CAddress As String = Nothing)
        BillPrinterDocument = New Printing.PrintDocument
        Me.TheTable = TheTableToPrint
        Me.TheSize = PageSize
        BillPrinterDocument.DefaultPageSettings.PaperSize = New Printing.PaperSize("Bill", PageSize.Width, PageSize.Height) '250 width ACTUAL : 420
        Type = BillType
        CustomerName = CName
        Address = CAddress
        DiscountValue = Discount
        SavingAmount = Savings
    End Sub

    Public Sub PrintYouBastard()
        BillPrinterDocument.Print()
    End Sub

    Public Sub SetDetails(ByVal BillNo As Integer, ByVal ItemsCount As Integer, ByVal FooterText As String, _
                          ByVal AmountInNumber As Integer, ByVal AmountinText As String, ByVal KOTNO As String, ByVal VAT As String)
        BillNumber = BillNo
        TotalItems = ItemsCount
        Footer = FooterText
        AmountinNum = AmountInNumber
        AmountText = AmountinText
        KOT = KOTNO
        VATVAL = VAT
        GetOtherDetails()
    End Sub

    Public Sub GetOtherDetails()
        Try
            Dim MyConn As New OleDb.OleDbConnection(ConnString)
            Dim ConnectionQuery As String
            Dim AReader As OleDb.OleDbDataReader

            ConnectionQuery = "select companyname, tagline, address, phone, website from company where companyname='" & LoggedInCompanyName & "'"
            Dim MyCommand As New OleDb.OleDbCommand(ConnectionQuery, MyConn)

            MyConn.Open()

            AReader = MyCommand.ExecuteReader

            While AReader.Read
                CompanyPrintLine = Convert.ToString(AReader(0))
                CompanyTag = Convert.ToString(AReader(1))
                CompanyAddr = Convert.ToString(AReader(2))
                PhoneNum = Convert.ToString(AReader(3))
                Website = Convert.ToString(AReader(4))
            End While

            MyConn.Close()
        Catch ex As Exception
            ErrorLogger.LogError(ex, "PRINTBILL")
            MsgBox("Error Occured. Please check log for more details.", MsgBoxStyle.Information, MessageTitle)
        End Try

    End Sub

    Private Sub PrintDocument1_PrintPage(ByVal sender As System.Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles BillPrinterDocument.PrintPage
        Try
            Dim f() As Single = {2, 2}

            Dim j As New Font("Tahoma", 10)
            Dim endf As New Font("Tahoma", 10)
            Dim ending As New Font("Tahoma", 12, FontStyle.Bold)
            Dim y As Int16 = 10
            Dim i As Int16 = 0
            Dim h As New Pen(Color.Black, 2)
            h.DashPattern = f



            e.Graphics.DrawString(CompanyPrintLine, New Font("Tahoma", 12, FontStyle.Bold), Brushes.Black, (e.PageBounds.Size.Width / 2) - (CompanyPrintLine.Length * 5), 10)
            e.Graphics.DrawString(CompanyTag.TrimStart, New Font("Tahoma", 10), Brushes.Black, ((e.PageBounds.Size.Width / 2) - (CompanyPrintLine.Length * 5)) - 15, 30)
            e.Graphics.DrawString(CompanyAddr, New Font("Tahoma", 8), Brushes.Black, ((e.PageBounds.Size.Width / 2) - (CompanyPrintLine.Length * 5)) - 45, 45)
            e.Graphics.DrawString(PhoneNum, New Font("Tahoma", 8), Brushes.Black, ((e.PageBounds.Size.Width / 2) - (CompanyPrintLine.Length * 4)), 57)
            e.Graphics.DrawString(Website, New Font("Tahoma", 6), Brushes.Black, ((e.PageBounds.Size.Width / 2) - (CompanyPrintLine.Length * 5)) - 15, 65)

            e.Graphics.DrawLine(h, 10, 80, e.PageBounds.Width - 10, 80)

            e.Graphics.DrawString("BILLED BY : " & LoggedInUserId, j, Brushes.Black, 10, 85)
            e.Graphics.DrawString("DATE : " & Today.Day & "/" & Today.Month & "/" & Today.Year, j, Brushes.Black, e.PageBounds.Size.Width - 130, 85)
            e.Graphics.DrawString("BILL NO : " & BillNumber, j, Brushes.Black, 10, 105)
            e.Graphics.DrawString("TIME : " & Now.Hour & ":" & Now.Minute, j, Brushes.Black, e.PageBounds.Size.Width - 130, 105)
            e.Graphics.DrawString("K.O.T NO : " & KOT, j, Brushes.Black, 10, 125)
            e.Graphics.DrawString("VAT RATE : " & VATVAL & " %", j, Brushes.Black, e.PageBounds.Size.Width - 130, 125)


            e.Graphics.DrawLine(h, 10, 145, e.PageBounds.Width - 10, 145)
            e.Graphics.DrawString("NO    ITEMNAME                   QTY     RATE     COST", New Font("Tahoma", 11, FontStyle.Bold), Brushes.Black, 10, 150)
            e.Graphics.DrawLine(h, 10, 170, e.PageBounds.Width - 10, 170)

            y = 185

            While i < TheTable.Rows.Count
                Dim Temp As String = TheTable.Rows(i)(3)
                Temp = Temp.PadLeft(2, "0")

                e.Graphics.DrawString(i + 1, j, Brushes.Black, 15, y)
                e.Graphics.DrawString(TheTable.Rows(i)(2), j, Brushes.Black, 50, y)
                e.Graphics.DrawString(Temp, j, Brushes.Black, 220, y)
                e.Graphics.DrawString(TheTable.Rows(i)(4), j, Brushes.Black, 280, y)
                e.Graphics.DrawString(TheTable.Rows(i)(5), j, Brushes.Black, 340, y)

                i += 1
                y += 25

            End While

            e.Graphics.DrawLine(h, 10, y, e.PageBounds.Width - 10, y)
            y += 10
            e.Graphics.DrawString("TOTAL ITEMS : " & TotalItems, j, Brushes.Black, 10, y)
            e.Graphics.DrawString("DISCOUNT : " & DiscountValue & " %", j, Brushes.Black, e.PageBounds.Size.Width - 130, y)
            y += 20
            e.Graphics.DrawString("FINAL AMOUNT : Rs. " & AmountinNum, ending, Brushes.Black, 10, y)
            y += 20
            e.Graphics.DrawString(AmountText, endf, Brushes.Black, 10, y)
            y += 15
            e.Graphics.DrawString("Inclusive of all taxes and discounts", New Font("Tahoma", 7, FontStyle.Italic), Brushes.Black, 15, y)
            y += 20
            e.Graphics.DrawString("Your Savings Today : Rs. " & SavingAmount, New Font("Tahoma", 8, FontStyle.Italic), Brushes.Black, 10, y)




            If Type = "CREDIT" Then
                e.Graphics.DrawLine(h, 10, y + 15, e.PageBounds.Width - 10, y + 15)

                y += 20
                e.Graphics.DrawString("TYPE : CREDIT", New Font("Tahoma", 10, FontStyle.Bold), Brushes.Black, 10, y)

                y += 25
                e.Graphics.DrawString("TO : " & CustomerName, New Font("Tahoma", 12), Brushes.Black, 10, y)

                y += 30
                e.Graphics.DrawString("ADDRESS : " & Address, New Font("Tahoma", 12), Brushes.Black, 10, y)

            End If


            e.Graphics.DrawLine(h, 10, e.PageBounds.Height - 50, e.PageBounds.Width - 10, e.PageBounds.Height - 50)
            e.Graphics.DrawString(Footer, New Font("Tahoma", 8), Brushes.Black, 10, e.PageBounds.Height - 45)
            e.Graphics.DrawString("SOFTWARE BY MORVO CORP. Visit www.morvocorp.com", New Font("Tahoma", 8), Brushes.Black, 10, e.PageBounds.Height - 30)

        Catch ex As Exception

            ErrorLogger.LogError(ex, "PRINTBILLONPRINT")
            MsgBox("Error Occured. Please check log for more details.", MsgBoxStyle.Information, MessageTitle)

        End Try
    End Sub

End Class


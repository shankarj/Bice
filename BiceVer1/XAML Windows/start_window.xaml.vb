Imports System.Data
Imports System.Data.OleDb
Imports AmCharts
Imports AmCharts.Windows.Core
Imports System.Windows


Partial Public Class start_window

#Region "DECLARATIONS"
    Private WithEvents Myt As New Forms.Timer
#End Region

    Private Sub ListBox1_SelectionChanged(ByVal sender As System.Object, ByVal e As System.Windows.Controls.SelectionChangedEventArgs) Handles ListBox1.SelectionChanged
        Dim TempString As String = ListBox1.SelectedValue

        If Not ListBox1.SelectedIndex = -1 Then
            Dim OpenNo As Integer = TempString.Substring(0, 3)
            TheListBox.Items.RemoveAt(ListBox1.SelectedIndex)
            BillObject(OpenNo).Show()
        End If
    End Sub


    Private Sub start_window_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        Try
            Dim t As New System.Windows.Media.ImageSourceConverter
            Myt.Interval = 5000
            Myt.Start()
            Cash_Bill.Source = t.ConvertFromString(Environment.CurrentDirectory & "\Resources\1.png")
            Image_Report.Source = t.ConvertFromString(Environment.CurrentDirectory & "\Resources\2.png")
            Image_Pending.Source = t.ConvertFromString(Environment.CurrentDirectory & "\Resources\3.png")
            Image_Exit.Source = t.ConvertFromString(Environment.CurrentDirectory & "\Resources\4.png")
            morvo.Source = t.ConvertFromString(Environment.CurrentDirectory & "\Resources\morvo2.png")
            CurrentBillNo = ConnectionObject.GetCurrentBillNumber()
            TempBillNo = 0
            HomeItem.IsSelected = True
            HomeItem.Focus()

            AssignList(ListBox1)

            Label2.Content = LoggedInCompanyName

            If Not LoggedInUserId.ToLower = "admin" Then
                NewUs.IsEnabled = False
                ChangeAdminPass.IsEnabled = False
            End If

        Catch ex As Exception
            ErrorLogger.LogError(ex, Me.Title)
            MsgBox("Error Occured. Please check log for more details.", MsgBoxStyle.Information, MessageTitle)
        End Try
    End Sub

    Private Sub label_cahsbill_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Input.MouseButtonEventArgs)
        CurrentBillNo += 1
        TempBillNo += 1
        BillObject(TempBillNo) = New BillWindow
        BillObject(TempBillNo).Show()
    End Sub

    Private Sub Window1_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Window1.Loaded
        Try
            LoadChart()
            LoadRevenueChart()
            Dim t As New System.Windows.Media.ImageSourceConverter
            Me.Icon = t.ConvertFromString(Environment.CurrentDirectory & "\Resources\mainico.ico")
        Catch ex As Exception
            ErrorLogger.LogError(ex, Me.Title)
            MsgBox("Error Occured. Please check log for more details.", MsgBoxStyle.Information, MessageTitle)
        End Try
    End Sub

#Region "TREE VIEW NAVIGATION CODE"

    Private Sub label_Pending_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Input.MouseButtonEventArgs)
        Dim PD As New Window_PendingCredits
        PD.Show()
    End Sub

    Private Sub label_Exit_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Input.MouseButtonEventArgs)
        End
    End Sub

    Private Sub label_report_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Input.MouseButtonEventArgs)
        Dim TR As New Window_TodayReport
        TR.Show()
    End Sub

    Private Sub start_window_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles Me.Closing
        End
    End Sub

    Private Sub TreeViewItem_Selected_1(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Dim TR As New Window_ProductGroup
        TR.Show()
    End Sub

    Private Sub TreeViewItem_Selected_2(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Dim TR As New Window_TodayReport
        TR.Show()
    End Sub

    Private Sub TreeViewItem_Selected_3(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Dim TR As New Window_ReportsByDate
        TR.Show()
    End Sub

    Private Sub TreeViewItem_Selected_4(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Dim MR As New Window_MonthlyReport
        MR.Show()
    End Sub

    Private Sub TreeViewItem_Selected_5(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Dim PD As New Window_PendingCredits
        PD.Show()
    End Sub

    Private Sub TreeViewItem_Selected_6(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)

    End Sub

    Private Sub TreeViewItem_Selected_7(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Dim NUS As New Window_ChangePass
        NUS.Show()
    End Sub

    Private Sub TreeViewItem_Selected_8(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Dim NUS As New Window_StockMgmt
        NUS.Show()
    End Sub

    Private Sub ChangeAdminPass_Selected(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Dim NUS As New Window_ChangePass
        NUS.Show()
    End Sub

    Private Sub NewUs_Selected(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Dim NUS As New Window_NewUser
        NUS.Show()
    End Sub

    Private Sub TreeViewItem_Selected_9(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Dim TR As New Window_StockOrder
        TR.Show()
    End Sub

    Private Sub TreeViewItem_Selected_10(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Dim TR As New Window_Restock
        TR.Show()
    End Sub

    Private Sub TreeViewItem_Selected_11(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Dim TR As New Window_SupplierMgm
        TR.Show()
    End Sub

    Private Sub TreeViewItem_Selected_12(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Dim TR As New Window_StockAnalysis
        TR.Show()
    End Sub

    Private Sub TreeViewItem_Selected_13(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Dim TR As New Window_PSAnalysis
        TR.Show()
    End Sub

    Private Sub TreeViewItem_Selected_14(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Dim TR As New Window_TSAnalysis
        TR.Show()
    End Sub

    Private Sub TreeViewItem_Selected_15(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Dim TR As New Window_PendingSuppliers
        TR.Show()
    End Sub

    Private Sub TreeViewItem_Selected_16(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Dim TR As New Window_SuppValue
        TR.Show()
    End Sub

    Private Sub TreeViewItem_Selected_17(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Dim TR As New Window_CustMgmt
        TR.Show()
    End Sub

    Private Sub TreeViewItem_Selected_18(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Dim TR As New Window_CustHistory
        TR.Show()
    End Sub

    Private Sub TreeViewItem_Selected_19(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Dim TR As New Window_CustChoice
        TR.Show()
    End Sub

    Private Sub TreeViewItem_Selected_20(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Dim TR As New Window_EmpMgmt
        TR.Show()
    End Sub

    Private Sub TreeViewItem_Selected_21(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Dim TR As New Window_Attendance
        TR.Show()
    End Sub

    Private Sub TreeViewItem_Selected_22(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Dim TR As New Window_CalculatePay
        TR.Show()
    End Sub

    Private Sub TreeViewItem_Selected_23(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Dim TR As New Window_FrequentAbs
        TR.Show()
    End Sub

    Private Sub TreeViewItem_Selected_24(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Dim TR As New Window_PayReport
        TR.Show()
    End Sub
#End Region

#Region "CHARTS"

    Private Sub LoadChart()
        Try
            Dim conn As New OleDb.OleDbConnection(ConnString)
            Dim Conquery As String

            Conquery = "select productname, sum(quantity) as sq from  bill_detail, menu_products where billdate=#" & Date_Today & "# and productid=menu_products.id and menu_products.companyid='" & LoggedInCompanyName & "' group by productname"

            Dim adapter As OleDb.OleDbDataAdapter = New OleDb.OleDbDataAdapter(Conquery, conn)
            Dim ds As New DataSet("mdset")

            conn.Open()
            adapter.Fill(ds, " bill_detail, menu_products")

            Dim slicesBinding As New Binding
            slicesBinding.Source = ds.Tables(0).DefaultView
            pieChart1.SetBinding(Windows.PieChart.SlicesSourceProperty, slicesBinding)

            pieChart1.ValueMemberPath = "sq"
            pieChart1.TitleMemberPath = "productname"

            ds.Dispose()
            adapter.Dispose()
        Catch ex As Exception
            ErrorLogger.LogError(ex, "LOAD CHART")
            MsgBox("Error Occured. Please check log for more details.", MsgBoxStyle.Information, MessageTitle)
        End Try
    End Sub

    Private Sub LoadRevenueChart()
        Try
            Dim conn As New OleDbConnection(ConnString)
            Dim Conquery As String

            Conquery = "Select billdate, sum(billvalue) as bs from all_entries where companyid='" & LoggedInCompanyName & "' group by billdate"

            Dim adapter As OleDbDataAdapter = New OleDbDataAdapter(Conquery, conn)
            Dim ds As New DataSet("mdset")

            conn.Open()
            adapter.Fill(ds, "all_entries")

            Mychart.SeriesSource = ds.Tables(0).DefaultView
            Mychart.IDMemberPath = "billdate"
            Mychart.TextMemberPath = "billdate"

            a1.DataItemsSource = ds.Tables(0).DefaultView
            a1.SeriesIDMemberPath = "billdate"
            a1.ValueMemberPath = "bs"

            ds.Dispose()
            adapter.Dispose()

        Catch ex As Exception
            ErrorLogger.LogError(ex, Me.Content)
            MsgBox("Error Occured. Please check log for more details.", MsgBoxStyle.Information, MessageTitle)
        End Try
    End Sub
#End Region

    Private Sub Myt_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Myt.Tick
        LoadChart()
        LoadRevenueChart()
    End Sub
End Class

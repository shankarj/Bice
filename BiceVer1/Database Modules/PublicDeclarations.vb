Imports System.Data

Module PublicDeclarations

#Region "STRINGS"

    Public ConnString As String = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & Environment.CurrentDirectory & "\SYS_STUB.mdb;Jet OLEDB:Database Password=qwe"
    Public MessageTitle As String = "Product Bice - It's Business Intelligent"
    Public Date_Today As String = Now.Month & "-" & Now.Day & "-" & Now.Year
    Public LoggedInCompanyName As String
    Public LoggedInUserId As String
    Public DependentsString As String


#End Region

#Region "INTEGERS"

    Public CurrentBillNo As Integer
    Public TempBillNo As Integer
    Public QuantAvailCalculated As Integer
    Public Groupindex As Integer = 0

#End Region

#Region "CLASS OBJECTS"

    Public ConnectionObject As New DatabaseConnector
    Public ConnectionObject2 As New DatabaseConnector2
    Public InventoryObject As New InventoryModule
    Public CustomerObject As New CustomerModule
    Public EmployeeObject As New EmployeeModule
    Public BillObject(100) As BillWindow
    Public ErrorLogger As New ErrLogger

#End Region

#Region "CONTROL OBJECTS"

    Public TheListBox As New ListBox
    Public TheTreeView As New TreeView
    Public Temp_RawListbox As New ListBox
    Public ReportTable1 As DataTable
    Public ReportTable2 As DataTable


#End Region

#Region "FUNCTIONS"
    Public Sub AssignList(ByRef IntoListBox As ListBox)
        IntoListBox.ItemsSource = TheListBox.Items
    End Sub

    Public Function HasExtraSymbols(ByVal InpStr As String) As Boolean
        For index As Integer = 0 To InpStr.Length - 1
            If Char.IsSymbol(InpStr.ElementAt(index)) Then
                Return True
            End If
        Next

        Return False
    End Function

    Public Function HasNumber(ByVal InpStr As String) As Boolean
        For index As Integer = 0 To InpStr.Length - 1
            If Char.IsDigit(InpStr.ElementAt(index)) Then
                Return True
            End If
        Next

        Return False
    End Function

#End Region

    
End Module

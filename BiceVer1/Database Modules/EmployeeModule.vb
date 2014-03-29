Imports System.Data
Imports System.Data.OleDb

Public Class EmployeeModule

    Public Sub EditEmployee(ByVal ename As String, ByVal daypay As Long, ByVal da As Long, ByVal ta As Long, ByVal extra As Long)
        Try
            Dim MyConn As New OleDb.OleDbConnection(ConnString)

            Dim ConnectionQuery As String = "update emp_details set " & _
                                            " ename='" & ename & "'" & _
                                            ", daypayamount=" & daypay & _
                                            ", DA=" & da & _
                                            ", TA=" & ta & _
                                            ", Extra=" & extra & _
                                            " where companyid='" & LoggedInCompanyName & "'"

            Dim MyCommand As New OleDb.OleDbCommand(ConnectionQuery, MyConn)

            MyConn.Open()
            MyCommand.ExecuteReader()
            MyConn.Close()
        Catch ex As Exception
            ErrorLogger.LogError(ex, "EditEmployee")
            MsgBox("Error Occured. Please check log for more details.", MsgBoxStyle.Information, MessageTitle)
        End Try
    End Sub

    Public Sub AddNewEmployee(ByVal ename As String, ByVal daypay As Long, ByVal da As Long, ByVal ta As Long, ByVal extra As Long)
        Try
            Dim MyConn As New OleDb.OleDbConnection(ConnString)
            Dim ConnectionQuery As String
            Dim MyCommand As OleDb.OleDbCommand


            ConnectionQuery = "insert into emp_details values ('" & ename & _
                          "'," & daypay & _
                          "," & da & _
                          "," & ta & _
                          "," & extra & _
                          ",NULL,NULL,NULL,'" & LoggedInCompanyName & "')"

            MyConn.Open()
            MyCommand = New OleDb.OleDbCommand(ConnectionQuery, MyConn)

            MyCommand.ExecuteReader()
            MyCommand.Dispose()


            MyConn.Close()
            MsgBox("Employee Added.", MsgBoxStyle.Information, MessageTitle)


        Catch ex As Exception
            ErrorLogger.LogError(ex, "AddNewEmployee")
            MsgBox("Error Occured. Check for any of these reasons." & vbCrLf & "1. Employee already present." & vbCrLf & "2. Wrong Values entered." & vbCrLf & "Please check log for more details.", MsgBoxStyle.Information, MessageTitle)
        End Try

    End Sub

End Class

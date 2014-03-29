Public Class ErrLogger

    Public Sub LogError(ByVal ex As Exception, ByVal FormName As String)
        Dim Logger As New IO.StreamWriter(Environment.CurrentDirectory & "\errlog.txt", True)
        Logger.WriteLine("-----------------------------------------------------------------")
        Logger.WriteLine(Date_Today)
        Logger.WriteLine("OCCURED IN : " & FormName)
        Logger.WriteLine("EXCEPTION MESSG : " & ex.Message)
        Logger.WriteLine("SOURCE : " & ex.Source)
        Logger.Close()
    End Sub
End Class


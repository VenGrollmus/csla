Imports System
Imports System.Linq
Imports Csla

Namespace BusinessRuleDemo
    <Serializable>
    Public Class StatesNVL
        Inherits NameValueListBase(Of String, String)
        <Fetch>
        Private Sub Fetch()
            RaiseListChangedEvents = False
            IsReadOnly = False

            Dim data = File.ReadAllLines("AmericanStates.txt")
            For Each x In data.[Select](Function(s) s.Split(","c))
                Me.Add(New NameValueListBase(Of Global.System.[String], Global.System.[String]).NameValuePair(x(CInt(0)).Trim(), x(CInt(1)).Trim()))
            Next

            IsReadOnly = True
            RaiseListChangedEvents = True
        End Sub
    End Class
End Namespace

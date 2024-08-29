Imports System
Imports System.Linq
Imports Csla

Namespace BusinessRuleDemo
    <Serializable>
    Public Class CountryNVL
        Inherits NameValueListBase(Of String, String)
        Public Const UnitedStates As String = "US"

        <Fetch>
        Private Sub Fetch()
            RaiseListChangedEvents = False
            IsReadOnly = False

            Dim data = File.ReadAllLines("CountryCodes.txt")
            For Each x In data.[Select](Function(s) s.Split(","c))
                Me.Add(New NameValueListBase(Of Global.System.[String], Global.System.[String]).NameValuePair(x(CInt(0)).Trim(), x(CInt(1)).Trim()))
            Next

            ' TODO: load values
            'object listData = null;
            'foreach (var item in listData)
            '  Add(new NameValueListBase<int, string>.
            '    NameValuePair(item.Key, item.Value));
            IsReadOnly = True
            RaiseListChangedEvents = True
        End Sub
    End Class
End Namespace

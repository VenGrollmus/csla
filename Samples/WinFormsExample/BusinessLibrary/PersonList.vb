Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports Csla

Namespace BusinessLibrary
    <Serializable>
    Public Class PersonList
        Inherits ReadOnlyListBase(Of PersonList, BusinessLibrary.PersonInfo)
        <Create, RunLocal>
        Private Sub Create()
        End Sub

        <Fetch>
        Private Sub Fetch(
            <Inject> dal As DataAccess.IPersonDal,
            <Inject> personPortal As IChildDataPortal(Of BusinessLibrary.PersonInfo))
            IsReadOnly = False
            Dim data = dal.Get().[Select](Function(d) personPortal.FetchChild(d))
            MyBase.AddRange(data)
            IsReadOnly = True
        End Sub
    End Class
End Namespace

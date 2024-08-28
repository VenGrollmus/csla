Imports System.Collections.Generic

Namespace DataAccess
    Public Interface IPersonDal
        Function Exists(id As Integer) As Boolean
        Function [Get](id As Integer) As DataAccess.PersonEntity
        Function [Get]() As List(Of DataAccess.PersonEntity)
        Function Insert(person As DataAccess.PersonEntity) As DataAccess.PersonEntity
        Function Update(person As DataAccess.PersonEntity) As DataAccess.PersonEntity
        Function Delete(id As Integer) As Boolean
    End Interface
End Namespace

Imports System
Imports System.Collections.Generic
Imports System.Linq

Namespace DataAccess
    Public Class PersonDal
        Implements DataAccess.IPersonDal
        Private Shared ReadOnly _personTable As List(Of DataAccess.PersonEntity) = New List(Of DataAccess.PersonEntity) From {
      New DataAccess.PersonEntity With {
                .Id = 1,
                .Name = "Andy"
            },
      New DataAccess.PersonEntity With {
                .Id = 3,
                .Name = "Buzz"
            }
    }

        Public Function Delete(id As Integer) As Boolean Implements DataAccess.IPersonDal.Delete
            Dim person = _personTable.Where(Function(p) p.Id = id).FirstOrDefault()
            If person IsNot Nothing Then
                SyncLock _personTable
                    _personTable.Remove(person)
                End SyncLock
                Return True
            Else
                Return False
            End If
        End Function

        Public Function Exists(id As Integer) As Boolean Implements DataAccess.IPersonDal.Exists
            Dim person = _personTable.Where(Function(p) p.Id = id).FirstOrDefault()
            Return Not person Is Nothing
        End Function

        Public Function [Get](id As Integer) As DataAccess.PersonEntity Implements DataAccess.IPersonDal.Get
            Dim person = _personTable.Where(Function(p) p.Id = id).FirstOrDefault()
            If person IsNot Nothing Then
                Return person
            Else
                Throw New KeyNotFoundException($"Id {id}")
            End If
        End Function

        Public Function [Get]() As List(Of DataAccess.PersonEntity) Implements DataAccess.IPersonDal.Get
            ' return projection of entire list
            Return _personTable.Where(Function(r) True).ToList()
        End Function

        Public Function Insert(person As DataAccess.PersonEntity) As DataAccess.PersonEntity Implements DataAccess.IPersonDal.Insert
            If Me.Exists(person.Id) Then Throw New InvalidOperationException($"Key exists {person.Id}")
            SyncLock _personTable
                Dim lastId As Integer = _personTable.Max(Function(m) m.Id)
                person.Id = Threading.Interlocked.Increment(lastId)
                _personTable.Add(person)
            End SyncLock
            Return person
        End Function

        Public Function Update(person As DataAccess.PersonEntity) As DataAccess.PersonEntity Implements DataAccess.IPersonDal.Update
            SyncLock _personTable
                Dim old = Me.Get(person.Id)
                old.Name = person.Name
                Return old
            End SyncLock
        End Function
    End Class
End Namespace

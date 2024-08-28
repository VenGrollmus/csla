Imports System
Imports System.ComponentModel.DataAnnotations
Imports Csla
Imports Csla.Rules

Namespace BusinessLibrary
    <Serializable>
    Public Class PersonEdit
        Inherits BusinessBase(Of PersonEdit)
        Public Shared ReadOnly IdProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(NameOf(PersonEdit.Id))
        Public Property Id As Integer
            Get
                Return GetProperty(IdProperty)
            End Get
            Set(value As Integer)
                SetProperty(IdProperty, value)
            End Set
        End Property

        Public Shared ReadOnly NameProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(NameOf(PersonEdit.Name))
        <Required>
        Public Property Name As String
            Get
                Return GetProperty(NameProperty)
            End Get
            Set(value As String)
                SetProperty(NameProperty, value)
            End Set
        End Property

        Protected Overrides Sub AddBusinessRules()
            MyBase.AddBusinessRules()
            BusinessRules.AddRule(New BusinessLibrary.InfoText(NameProperty, "Person name (required)"))
            BusinessRules.AddRule(New BusinessLibrary.CheckCase(NameProperty))
            BusinessRules.AddRule(New BusinessLibrary.NoZAllowed(NameProperty))
        End Sub

        <Create, RunLocal>
        Private Sub Create()
            Id = -1
            BusinessRules.CheckRules()
        End Sub

        <Fetch>
        Private Sub Fetch(id As Integer,
            <Inject> dal As DataAccess.IPersonDal)
            Dim data = dal.Get(id)
            Using BypassPropertyChecks
                Csla.Data.DataMapper.Map(data, Me)
            End Using
            BusinessRules.CheckRules()
        End Sub

        <Insert>
        Private Sub Insert(
            <Inject> dal As DataAccess.IPersonDal)
            Using BypassPropertyChecks
                Dim data = New DataAccess.PersonEntity With {
          .Name = Name
        }
                Dim result = dal.Insert(data)
                Id = result.Id
            End Using
        End Sub

        <Update>
        Private Sub Update(
            <Inject> dal As DataAccess.IPersonDal)
            Using BypassPropertyChecks
                Dim data = New DataAccess.PersonEntity With {
          .Id = Id,
          .Name = Name
        }
                dal.Update(data)
            End Using
        End Sub

        <DeleteSelf>
        Private Sub DeleteSelf(
            <Inject> dal As DataAccess.IPersonDal)
            Delete(ReadProperty(IdProperty), dal)
        End Sub

        <Delete>
        Private Sub Delete(id As Integer,
            <Inject> dal As DataAccess.IPersonDal)
            dal.Delete(id)
        End Sub

    End Class
End Namespace

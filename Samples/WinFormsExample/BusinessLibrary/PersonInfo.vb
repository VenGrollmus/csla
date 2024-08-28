Imports System
Imports Csla

Namespace BusinessLibrary
    <Serializable>
    Public Class PersonInfo
        Inherits ReadOnlyBase(Of PersonInfo)
        Public Shared ReadOnly IdProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(NameOf(PersonInfo.Id))
        Public Property Id As Integer
            Get
                Return GetProperty(IdProperty)
            End Get
            Private Set(value As Integer)
                LoadProperty(IdProperty, value)
            End Set
        End Property

        Public Shared ReadOnly NameProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(NameOf(PersonInfo.Name))
        Public Property Name As String
            Get
                Return GetProperty(NameProperty)
            End Get
            Private Set(value As String)
                LoadProperty(NameProperty, value)
            End Set
        End Property

        <Create, RunLocal>
        Private Sub Create()
        End Sub

        <Fetch>
        Private Sub Fetch(id As Integer,
            <Inject> dal As DataAccess.IPersonDal)
            Dim data = dal.Get(id)
            Fetch(data)
        End Sub

        <FetchChild>
        Private Sub Fetch(data As DataAccess.PersonEntity)
            Id = data.Id
            Name = data.Name
        End Sub
    End Class
End Namespace

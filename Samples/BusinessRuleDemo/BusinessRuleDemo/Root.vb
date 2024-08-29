Imports System
Imports System.ComponentModel.DataAnnotations
Imports System.Diagnostics
Imports Csla
Imports Csla.Core
Imports Csla.Rules
Imports Csla.Rules.CommonRules

Namespace BusinessRuleDemo
    <Serializable>
    Public Class Root
        Inherits BusinessBase(Of Root)
        Public Shared ReadOnly NameProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(NameOf(Root.Name))
        <Required>
        Public Property Name As String
            Get
                Return GetProperty(NameProperty)
            End Get
            Set(value As String)
                SetProperty(NameProperty, value)
            End Set
        End Property

        Public Shared ReadOnly Num1Property As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(NameOf(Root.Num1))
        Public Property Num1 As Integer
            Get
                Return GetProperty(Num1Property)
            End Get
            Set(value As Integer)
                SetProperty(Num1Property, value)
            End Set
        End Property

        Public Shared ReadOnly Num2Property As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(NameOf(Root.Num2))
        <Range(1, 6000)>
        Public Property Num2 As Integer
            Get
                Return GetProperty(Num2Property)
            End Get
            Set(value As Integer)
                SetProperty(Num2Property, value)
            End Set
        End Property

        Public Shared ReadOnly SumProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(NameOf(Root.Sum))
        Public Property Sum As Integer
            Get
                Return GetProperty(SumProperty)
            End Get
            Set(value As Integer)
                SetProperty(SumProperty, value)
            End Set
        End Property

        Public Shared ReadOnly CountryProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(NameOf(Root.Country))
        Public Property Country As String
            Get
                Return GetProperty(CountryProperty)
            End Get
            Set(value As String)
                SetProperty(CountryProperty, value)
            End Set
        End Property

        Public Shared ReadOnly StateProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(NameOf(Root.State))
        Public Property State As String
            Get
                Return GetProperty(StateProperty)
            End Get
            Set(value As String)
                SetProperty(StateProperty, value)
            End Set
        End Property

        Public Shared ReadOnly StateNameProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(NameOf(Root.StateName))
        Public Property StateName As String
            Get
                Return GetProperty(StateNameProperty)
            End Get
            Set(value As String)
                SetProperty(StateNameProperty, value)
            End Set
        End Property

        Public Shared ReadOnly AdditionalInfoForUSProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(NameOf(Root.AdditionalInfoForUS))
        Public Property AdditionalInfoForUS As String
            Get
                Return GetProperty(AdditionalInfoForUSProperty)
            End Get
            Set(value As String)
                SetProperty(AdditionalInfoForUSProperty, value)
            End Set
        End Property

        Protected Overrides Sub AddBusinessRules()
            ' call base class implementation to add data annotation rules to BusinessRules 
            MyBase.AddBusinessRules()

            ' add authorization rules 
            BusinessRules.AddRule(New BusinessRuleDemo.OnlyForUS(AuthorizationActions.WriteProperty, StateProperty, CountryProperty))

            ' add validation rules 
            ' set up dependencies to that Sum is automatially recaclulated when PrimaryProperty is changed 
            BusinessRules.AddRule(New Dependency(Num1Property, SumProperty))
            BusinessRules.AddRule(New Dependency(Num2Property, SumProperty))

            ' add dependency for LessThanProperty rule on Num1
            BusinessRules.AddRule(New Dependency(Num2Property, Num1Property))

            ' add dependency for StringRequiredIfUS on AddistionalInfoForUs
            BusinessRules.AddRule(New Dependency(CountryProperty, AdditionalInfoForUSProperty))

            BusinessRules.AddRule(New MaxValue(Of Integer)(Num1Property, 5000))
            BusinessRules.AddRule(New BusinessRuleDemo.LessThanProperty(Num1Property, Num2Property))

            ' calculates sum rule - must alwas run before MinValue with lower priority
            BusinessRules.AddRule(New BusinessRuleDemo.CalcSum(SumProperty, Num1Property, Num2Property) With {
                      .Priority = -1
                  })
            BusinessRules.AddRule(New MinValue(Of Integer)(SumProperty, 1))

            BusinessRules.AddRule(New BusinessRuleDemo.StringRequiredIfUS(AdditionalInfoForUSProperty, CountryProperty))

            ' Name Property - uses DataAnnotation Required combined with a Csla MaxLength rule
            'BusinessRules.AddRule(new Required(NameProperty));
            BusinessRules.AddRule(New MaxLength(NameProperty, 10))

            BusinessRules.AddRule(New BusinessRuleDemo.SetStateName(StateProperty, StateNameProperty))
        End Sub

        Private Sub MyChildChanged(sender As Object, e As ChildChangedEventArgs)
            Call Debug.Print(e.ChildObject.ToString(), e.ListChangedArgs)
        End Sub

        <Create>
        Private Sub Create()
            Using BypassPropertyChecks
                Num1 = 5001
                Num2 = 6001
                Country = "UZ"
            End Using
            BusinessRules.CheckRules()
        End Sub
    End Class
End Namespace

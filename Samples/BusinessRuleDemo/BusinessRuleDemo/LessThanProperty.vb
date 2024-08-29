Imports System.Collections.Generic
Imports Csla.Core
Imports Csla.Rules

Namespace BusinessRuleDemo
    ''' <summary>
    ''' Implements a rule to compare 2 property values and make sure property1 is less than property2
    ''' </summary>
    Public Class LessThanProperty
        Inherits BusinessRule
        Private Property CompareTo As IPropertyInfo

        ''' <summary>
        ''' Initializes a new instance of the <seecref="LessThanProperty"/> class.
        ''' </summary>
        ''' <paramname="primaryProperty">The primary property.</param>
        ''' <paramname="compareToProperty">The compare to property.</param>
        Public Sub New(primaryProperty As IPropertyInfo, compareToProperty As IPropertyInfo)
            MyBase.New(primaryProperty)
            CompareTo = compareToProperty

            If InputProperties Is Nothing Then
                InputProperties = New List(Of IPropertyInfo)()
            End If
            InputProperties.Add(primaryProperty)
            InputProperties.Add(compareToProperty)
        End Sub

        ''' <summary>
        ''' Does the check for primary propert less than compareTo property
        ''' </summary>
        ''' <paramname="context">Rule context object.</param>
        Protected Overrides Sub Execute(context As IRuleContext)
            Dim value1 = CType(context.InputPropertyValues(MyBase.PrimaryProperty), dynamic)
            Dim value2 = CType(context.InputPropertyValues(CompareTo), dynamic)

            If value1 > value2 Then
                context.AddErrorResult(String.Format("{0} must be less than or equal {1}", MyBase.PrimaryProperty.FriendlyName, CompareTo.FriendlyName))
            End If
        End Sub
    End Class
End Namespace

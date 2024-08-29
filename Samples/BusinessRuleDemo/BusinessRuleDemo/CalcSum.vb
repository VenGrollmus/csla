Imports System.Collections.Generic
Imports System.Linq
Imports Csla.Core
Imports Csla.Rules

Namespace BusinessRuleDemo
    ''' <summary>
    ''' CalcSum rule will set primary property to the sum of all.
    ''' </summary>
    Public Class CalcSum
        Inherits BusinessRule
        ''' <summary>
        ''' Initializes a new instance of the <seecref="CalcSum"/> class.
        ''' </summary>
        ''' <paramname="primaryProperty">The primary property.</param>
        ''' <paramname="inputProperties">The input properties.</param>
        Public Sub New(primaryProperty As IPropertyInfo, ParamArray inputProperties As IPropertyInfo())
            MyBase.New(primaryProperty)
            If MyBase.InputProperties Is Nothing Then
                MyBase.InputProperties = New List(Of IPropertyInfo)()
            End If
            MyBase.InputProperties.AddRange(inputProperties)
        End Sub

        Protected Overrides Sub Execute(context As IRuleContext)
            ' Use linq Sum to calculate the sum value 
            Dim sum = context.InputPropertyValues.Sum(Function([property]) CType([property].Value, dynamic))

            ' add calculated value to OutValues 
            ' When rule is completed the RuleEngig will update businessobject
            context.AddOutValue(MyBase.PrimaryProperty, sum)
        End Sub
    End Class
End Namespace

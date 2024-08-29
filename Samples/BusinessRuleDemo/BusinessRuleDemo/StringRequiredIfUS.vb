Imports System.Collections.Generic
Imports System.Linq
Imports Csla.Core
Imports Csla.Rules

Namespace BusinessRuleDemo

    ''' <summary>
    ''' This class demonstrates how to utilize inner rules inside a business rule. 
    ''' 
    ''' This rule calls inner rule StringRequired on PrimaryProperty if countryCode is US
    ''' </summary>
    Public Class StringRequiredIfUS
        Inherits BusinessRule
        Private _countryProperty As IPropertyInfo
        Private _innerRule As IBusinessRule
        ' TODO: Add additional parameters to your rule to the constructor
        Public Sub New(primaryProperty As IPropertyInfo, countryProperty As IPropertyInfo)
            MyBase.New(primaryProperty)
            _countryProperty = countryProperty
            _innerRule = CType(New CommonRules.Required(primaryProperty), IBusinessRule)
            InputProperties = New List(Of IPropertyInfo)()

            ' this rule needs the Country property
            InputProperties.Add(countryProperty)

            ' add input properties required by inner rules
            Dim inputProps = _innerRule.InputProperties.Where(Function(inputProp) Not InputProperties.Contains(inputProp))
            If inputProps.Count() > 0 Then InputProperties.AddRange(inputProps)
        End Sub

        Protected Overrides Sub Execute(context As IRuleContext)
            ' TODO: Add actual rule code here. 
            Dim country = CStr(context.InputPropertyValues(_countryProperty))
            If Equals(country, BusinessRuleDemo.CountryNVL.UnitedStates) Then
                _innerRule.Execute(context.GetChainedContext(_innerRule))
            End If
        End Sub
    End Class
End Namespace

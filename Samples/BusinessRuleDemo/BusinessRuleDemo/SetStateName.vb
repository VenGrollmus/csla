Imports System.Collections.Generic
Imports Csla.Core
Imports Csla.Rules

Namespace BusinessRuleDemo
    Public Class SetStateName
        Inherits BusinessRule
        Public Property StateName As IPropertyInfo

        Public Sub New(stateIdProperty As IPropertyInfo, stateNameProperty As IPropertyInfo)
            MyBase.New(stateIdProperty)
            StateName = stateNameProperty
            InputProperties = New List(Of IPropertyInfo) From {
                      stateIdProperty
                  }
            AffectedProperties.Add(StateName)
        End Sub

        ''' <summary>
        ''' Look up State and set the state name 
        ''' </summary>
        ''' <paramname="context">Rule context object.</param>
        Protected Overrides Sub Execute(context As IRuleContext)
            Dim stateId = CStr(context.InputPropertyValues(MyBase.PrimaryProperty))
            Dim state = BusinessRuleDemo.App.ApplicationContext.GetRequiredService(Of IDataPortal(Of BusinessRuleDemo.StatesNVL))().Fetch().Where(Function(p) Equals(p.Key, stateId)).FirstOrDefault()
            context.AddOutValue(StateName, If(state Is Nothing, "Unknown state", state.Value))
        End Sub
    End Class
End Namespace

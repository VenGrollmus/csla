Imports Csla.Rules

Namespace BusinessLibrary
    Public Class InfoText
        Inherits BusinessRule
        Public Property Text As String
        Public Sub New(primaryProperty As Csla.Core.IPropertyInfo, text As String)
            MyBase.New(primaryProperty)
            Me.Text = text
        End Sub

        Protected Overrides Sub Execute(context As IRuleContext)
            context.AddInformationResult(Text)
        End Sub
    End Class
End Namespace

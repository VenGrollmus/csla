Imports Csla.Rules

Namespace BusinessLibrary
    Public Class NoZAllowed
        Inherits BusinessRule
        Public Sub New(primaryProperty As Csla.Core.IPropertyInfo)
            MyBase.New(primaryProperty)
        End Sub

        Protected Overrides Sub Execute(context As IRuleContext)
            Dim text = CStr(ReadProperty(context.Target, MyBase.PrimaryProperty))
            If text.ToLower().Contains("z") Then context.AddErrorResult("No letter Z allowed")
        End Sub
    End Class
End Namespace

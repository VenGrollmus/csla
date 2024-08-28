Imports Csla.Rules

Namespace BusinessLibrary

    Public Class CheckCase
        Inherits BusinessRule
        Public Sub New(primaryProperty As Csla.Core.IPropertyInfo)
            MyBase.New(primaryProperty)
        End Sub

        Protected Overrides Sub Execute(context As IRuleContext)
            Dim text = CStr(ReadProperty(context.Target, MyBase.PrimaryProperty))
            If String.IsNullOrWhiteSpace(text) Then Return
            Dim ideal = text.Substring(0, 1).ToUpper()
            ideal += text.Substring(1).ToLower()
            If Not Equals(text, ideal) Then context.AddWarningResult("Check capitalization")
        End Sub
    End Class
End Namespace

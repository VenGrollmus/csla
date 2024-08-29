Imports System
Imports System.Windows.Forms
Imports Csla.Configuration
Imports Microsoft.Extensions.DependencyInjection

Namespace BusinessRuleDemo
    Friend Module Program
        ''' <summary>
        ''' The main entry point for the application.
        ''' </summary>
        <STAThread>
        Private Sub Main()
            Dim services = New ServiceCollection()
            services.AddCsla()
            Dim provider = services.BuildServiceProvider()
            BusinessRuleDemo.App.ApplicationContext = provider.GetRequiredService(Of Csla.ApplicationContext)()

            Call Application.EnableVisualStyles()
            Application.SetCompatibleTextRenderingDefault(False)
            Call Application.Run(New BusinessRuleDemo.Form1())
        End Sub
    End Module
End Namespace

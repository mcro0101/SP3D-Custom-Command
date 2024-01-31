
Imports System.Windows.Forms
Imports Ingr.SP3D.Common.Client
Imports Ingr.SP3D.Common.Client.Services
Imports Ingr.SP3D.Common.Middle
Imports Ingr.SP3D.Common.Middle.Services
Imports Ingr.SP3D.Equipment.Middle
Imports Ingr.SP3D.ReferenceData.Middle
Imports Ingr.SP3D.Systems.Middle
Public Class Assembly
    Inherits BaseStepCommand
    Private myForm As FrmCommand
    Public Shared activePlant As Plant

    
    Public Overrides Sub OnSuspend()
        MyBase.OnSuspend()
    End Sub
    Public Overrides ReadOnly Property EnableUIFlags() As Integer
        Get
            Return EnableUIFlagSettings.ActiveConnection
        End Get
    End Property

    Public Overrides Sub OnStop()
        'If ofrmSystem IsNot Nothing Then ofrmSystem.Close()
        MyBase.OnStop()
        'myForm.m_oTxnMgr.Abort()
    End Sub

    Public Overrides Sub OnResume()
        MyBase.OnResume()
    End Sub
    Public Overrides Sub OnStart(ByVal commandID As Integer, ByVal argument As Object)
        MyBase.OnStart(commandID, argument)
        Try
            StartAssemblyConnection()
            myForm = New FrmCommand
            myForm.Show()
        Catch ex As Exception
            myForm.Close()
        End Try


    End Sub
    Public Sub StartAssemblyConnection()
        m_running = True
        MyBase.Enabled = False

        MyBase.StepCount = 2

        MyBase.Steps(0).Prompt = "Ready. Select Parent and then Component to be placed."
        MyBase.Steps(0).LocateBehavior = StepDefinition.LocateBehaviors.Boreline
        MyBase.Steps(0).StepFilter.AddInterface("IJStructuralSystem")
        MyBase.Steps(0).StepFilter.AddLogicalOperator(StepFilter.LogicalOperator.OR)
        'MyBase.Steps(0).StepFilter.AddInterface("IJEquipment")
        MyBase.Steps(0).MaximumSelectable = 1
        MyBase.Steps(0).HiliteCumulativeSelection = False

        MyBase.Steps(1).Prompt = "Select Position and/or Left Click"
        MyBase.Steps(1).LocateBehavior = StepDefinition.LocateBehaviors.SmartSketch3D
        MyBase.Steps(1).HiliteLocated = True
        MyBase.Steps(1).HiliteCumulativeSelection = False

        MyBase.Enabled = True

        activePlant = MiddleServiceProvider.SiteMgr.ActiveSite.ActivePlant
        MyBase.StopCommandOnRightClick = False
    End Sub

    Protected Overrides Sub OnMouseDown(ByVal view As GraphicView, ByVal e As GraphicViewManager.GraphicViewEventArgs, ByVal position As Position)
        MyBase.OnMouseDown(view, e, position)
        Try
            If e.Button.ToString() = "Right" Then
			'Add your Function everytime when you perform Mouse Right Click
                MyBase.CurrentStepIndex = 0

            End If
            If e.Button.ToString() = "Left" Then
             'Add your Function everytime when you perform Mouse Left Click
                MyBase.CurrentStepIndex = 0

            End If
        Catch ex As Exception

        End Try
    End Sub

    Protected Overrides Sub OnMouseMove(ByVal view As GraphicView, ByVal e As GraphicViewManager.GraphicViewEventArgs, ByVal position As Position)
        MyBase.OnMouseMove(view, e, position)
        Try

            'Add your Function everytime you hover your mouse
			'usually use for set positioning 
        Catch ex As Exception

        End Try
    End Sub

    Protected Overrides Sub OnKeyDown(e As KeyEventArgs)
        If e.KeyValue = ConsoleKey.Escape Then
            StopCommand()
        End If
    End Sub
End Class

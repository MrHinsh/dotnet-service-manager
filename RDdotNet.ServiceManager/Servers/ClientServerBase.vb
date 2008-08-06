Imports System.Runtime.Serialization
Imports System.Collections.ObjectModel


Namespace Servers

    Public MustInherit Class ClientServerBase
        Implements IClientServer
        Implements IDisposable

        Private _Uri As Uri = New Uri("http://" & System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName).HostName & ":6661")
        Private _ClientServices As New Collection(Of Clients.IClientService)
        Private _ClientServicesLoaded As Boolean = False

        Public ReadOnly Property ServerUri() As Uri Implements IClientServer.ServerUri
            Get
                Return _Uri
            End Get
        End Property

        Public Sub New(Optional ByVal Url As Uri = Nothing)
            If Not Url Is Nothing Then
                _Uri = Url
            End If
        End Sub

        Friend Sub SetUri(Optional ByVal Url As Uri = Nothing)
            If Not Url Is Nothing Then
                _Uri = Url
            End If
        End Sub

        Public Function Authenticated() As Boolean Implements IClientServer.Authenticated
            Throw New NotImplementedException
        End Function

        Public Function GetService(Of TService As Clients.IClientService)() As TService Implements IClientServer.GetService
            If Not _ClientServicesLoaded Then LoadServices()
            For Each service As Clients.IClientService In _ClientServices
                Dim obj As Object = service
                If obj.GetType Is GetType(TService) Then
                    Return CType(service, TService)
                End If
            Next
            Throw New NotImplementedException
        End Function

        Public Function GetService(ByVal Name As String) As Clients.IClientService Implements IClientServer.GetService
            If Not _ClientServicesLoaded Then LoadServices()
            For Each service As Clients.IClientService In _ClientServices
                If service.ServiceName = Name Then
                    Return service
                End If
            Next
            Throw New InvalidOperationException
        End Function

        ''' <summary>
        ''' Returns the service that implements a specific RDdotNet service Contract.
        ''' </summary>
        ''' <param name="ServiceContract">interface as type</param>
        ''' <returns>Service that implements IClientService</returns>
        ''' <remarks></remarks>
        Public Function GetService(ByVal ServiceContract As Type) As Clients.IClientService Implements IClientServer.GetService
            If ServiceContract.GetCustomAttributes(GetType(Clients.ClientServiceContractAttribute), True).Length = 0 Then Throw New InvalidOperationException("You must pass in a contract (interface) that is mapked with the RDdotNetServiceContractAttribute.")
            If Not _ClientServicesLoaded Then LoadServices()
            For Each service As Clients.IClientService In _ClientServices
                For Each contract As Type In service.Contracts
                    If contract Is ServiceContract Then
                        Return service
                    End If
                Next
            Next
            Throw New InvalidOperationException
        End Function

        Public Function GetServices() As Collection(Of Clients.IClientService) Implements IClientServer.GetServices
            Return _ClientServices
        End Function

        Private Sub LoadServices() Implements IClientServer.LoadServices
            OnServicesPreLoad()
            OnServicesLoad()
            OnServicesPostLoad()
        End Sub

        Protected Sub AddClientService(ByVal Service As Clients.IClientService) Implements IClientServer.AddClientService
            If ValidateClientService(Service) Then
                _ClientServices.Add(Service)
            Else
                Throw New InvalidConstraintException("You must pass in a valid client service.")
            End If
        End Sub

        Public Function ValidateClientService(ByVal Service As Clients.IClientService) As Boolean Implements IClientServer.ValidateClientService
            ValidateClientService = True
            Dim o As Object = Service
            'If o.GetType.GetCustomAttributes(GetType(RDdotNetServiceContractAttribute), True).Length = 0 Then Throw New InvalidOperationException("You must pass in a contract (interface) that is mapked with the RDdotNetServiceContractAttribute.")
            'TODO: Validate services

        End Function


        Protected Overridable Sub OnServicesPreLoad() Implements IClientServer.OnServicesPreLoad

        End Sub

        Protected MustOverride Sub OnServicesLoad() Implements IClientServer.OnServicesLoad

        Protected Overridable Sub OnServicesPostLoad() Implements IClientServer.OnServicesPostLoad
            For Each ClientService As Clients.IClientService In _ClientServices
                ClientService.Start()
            Next
        End Sub

        Private Sub UnloadServices() Implements IClientServer.UnloadServices
            OnServicesUnload(_ClientServices)
        End Sub

        Protected MustOverride Sub OnServicesUnload(ByRef ClientServices As Collection(Of Clients.IClientService)) Implements IClientServer.OnServicesUnload

#Region " IDisposable "

        Private disposedValue As Boolean = False        ' To detect redundant calls

        ' IDisposable
        Protected Overridable Sub Dispose(ByVal disposing As Boolean)
            If Not Me.disposedValue Then
                If disposing Then
                    ' TODO: free managed resources when explicitly called
                    UnloadServices()
                End If
                ' TODO: free shared unmanaged resources
            End If
            Me.disposedValue = True
        End Sub

#Region " IDisposable Support "
        ' This code added by Visual Basic to correctly implement the disposable pattern.
        Public Sub Dispose() Implements IDisposable.Dispose
            ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
            Dispose(True)
            GC.SuppressFinalize(Me)
        End Sub
#End Region

#End Region

    End Class


End Namespace
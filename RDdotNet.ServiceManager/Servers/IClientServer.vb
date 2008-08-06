Imports System.Runtime.Serialization
Imports System.Collections.ObjectModel


Namespace Servers

    Public Interface IClientServer

        ReadOnly Property ServerUri() As Uri
        Function Authenticated() As Boolean
        Function GetService(Of TService As Clients.IClientService)() As TService
        Function GetService(ByVal Name As String) As Clients.IClientService
        Function GetService(ByVal ServiceContract As Type) As Clients.IClientService
        Function GetServices() As Collection(Of Clients.IClientService)
        Sub LoadServices()
        Sub AddClientService(ByVal Service As Clients.IClientService)
        Function ValidateClientService(ByVal Service As Clients.IClientService) As Boolean
        Sub OnServicesPreLoad()
        Sub OnServicesLoad()
        Sub OnServicesPostLoad()
        Sub UnloadServices()
        Sub OnServicesUnload(ByRef ClientServices As Collection(Of Clients.IClientService))

    End Interface


End Namespace
Imports System.Runtime.Serialization
Imports System.Collections.ObjectModel

Namespace Clients

    ''' <summary>
    ''' The IClientService interface sets a standard access format for all of the client services that will be loaded. It defignes a number of actions against any client service.
    ''' </summary>
    ''' <remarks></remarks>
    Public Interface IClientService

        ''' <summary>
        ''' Allows a client service to be set as local or remote service. This is set by the developer of the service.
        ''' </summary>
        ''' <value>Local or Remote</value>
        ''' <returns>ClientServiceTypes</returns>
        ''' <remarks></remarks>
        ReadOnly Property ServiceType() As ClientServiceTypes

        ''' <summary>
        ''' A unique name given to a service that is set by the developer of a service
        ''' </summary>
        ''' <value>String value denoting the name of the service</value>
        ''' <returns>The name of the service as a string value</returns>
        ''' <remarks></remarks>
        ReadOnly Property ServiceName() As String

        ''' <summary>
        ''' Links back to the parent client server for that service.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ReadOnly Property Server() As Servers.IClientServer

        ''' <summary>
        ''' This contains a list of all contracts implemented by a client service as system types
        ''' </summary>
        ''' <value>Collection of system types</value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ReadOnly Property Contracts() As Collection(Of Type)

        ''' <summary>
        ''' Determins wither a user is authenticated to use this service. This will be set to True is no authentication is required.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function Authenticated() As Boolean

        Sub Start()
        Sub [Stop]()

    End Interface

    ''' <summary>
    ''' Enumerator that defignes wither a client service is a remote service or a local one.
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum ClientServiceTypes
        ''' <summary>
        ''' Defigned a client service as a localy exicuting service.
        ''' </summary>
        ''' <remarks></remarks>
        Local
        ''' <summary>
        ''' Defignes a client service as a remotly exicuting service.
        ''' </summary>
        ''' <remarks></remarks>
        Remote
    End Enum

End Namespace

Imports System.ServiceModel
Imports System.Runtime.Serialization
Imports System.Collections.ObjectModel

Namespace Clients

    ''' <summary>
    ''' The WcfClientServiceBase is a helper class for any Wcf services. It provides extra information and helpers in addition to the IClientService interface.
    ''' </summary>
    ''' <typeparam name="TClient">TBA</typeparam>
    ''' <typeparam name="TBinding">Determins the binding to be used during the connection.</typeparam>
    ''' <remarks></remarks>
    Public MustInherit Class WcfClientServiceBase(Of TClient As {Class}, TBinding As Channels.Binding)
        Implements IClientService

        Private _Server As Servers.IClientServer

        Public Sub New(ByVal Server As Servers.IClientServer, ByVal Location As String)
            If Not Server Is Nothing Then
                _Server = Server
            End If
            If Not Location = "" Then
                _Location = Location
            Else
                Throw New InvalidOperationException("You must provide a valid location")
            End If
        End Sub

        Public ReadOnly Property Server() As Servers.IClientServer Implements IClientService.Server
            Get
                Return Me._Server
            End Get
        End Property

        Public ReadOnly Property ServiceType() As ClientServiceTypes Implements IClientService.ServiceType
            Get
                Return ClientServiceTypes.Remote
            End Get
        End Property

        Private _Location As String = "WcfServiceBase"

        Public ReadOnly Property Location() As String
            Get
                Return _Location
            End Get
        End Property

        Public ReadOnly Property EndPoint() As EndpointAddress
            Get
                Return New EndpointAddress(New Uri(_Server.ServerUri, _Location))
            End Get
        End Property

        Private _Client As TClient

        Protected ReadOnly Property Client() As TClient
            Get
                If _Client Is Nothing Then
                    _Client = GetClient()
                End If
                Return _Client
            End Get
        End Property

        Protected MustOverride Function GetBinding() As TBinding
        Protected MustOverride Function GetClient() As TClient



        Public ReadOnly Property Contracts() As System.Collections.ObjectModel.Collection(Of System.Type) Implements IClientService.Contracts
            Get
                Dim ServiceContracts As New Collection(Of Type)
                For Each t As Type In Me.GetType.GetInterfaces
                    If t.IsInterface And t.GetCustomAttributes(GetType(ClientServiceContractAttribute), True).Length > 0 Then
                        ServiceContracts.Add(t)
                    End If
                Next
                Return ServiceContracts
            End Get
        End Property

        Public ReadOnly Property ServiceName() As String Implements IClientService.ServiceName
            Get
                Return Me.GetType.Name
            End Get
        End Property



        Public Function Authenticated() As Boolean Implements IClientService.Authenticated
            Throw New NotImplementedException
        End Function


        Public Sub Start() Implements IClientService.Start

        End Sub

        Public Sub [Stop]() Implements IClientService.Stop

        End Sub


    End Class

End Namespace

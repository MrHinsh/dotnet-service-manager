Imports System.Runtime.Serialization
Imports System.Collections.ObjectModel
Imports System.Collections.Generic

Namespace Servers

    Public Class ClientServerFactory

#Region " Singleton "

        Private Shared _Instance As ClientServerFactory

        Private Shared ReadOnly Property Instance() As ClientServerFactory
            Get
                If _Instance Is Nothing Then
                    _Instance = New ClientServerFactory
                End If
                Return _Instance
            End Get
        End Property

        Private Sub New()

        End Sub

#End Region

#Region " Server Mangement "

        Private Servers As New System.Collections.Generic.Dictionary(Of Uri, ClientServerBase)

        Public Shared Function GetServer(Of TRDdotNetServer As {New, ClientServerBase})(ByVal Server As Uri) As TRDdotNetServer
            If Instance.Servers.ContainsKey(Server) Then
                Return CType(Instance.Servers(Server), TRDdotNetServer)
            Else
                Try
                    Dim RDdotNetServer As New TRDdotNetServer()
                    RDdotNetServer.SetUri(Server)
                    If RDdotNetServer.Authenticated Then
                        Instance.Servers.Add(Server, RDdotNetServer)
                    Else
                        Throw New InvalidOperationException
                    End If
                Catch ex As Exception
                    Throw New InvalidOperationException
                End Try
            End If
            Return CType(Instance.Servers(Server), TRDdotNetServer)
        End Function

#End Region

#Region " Client Service Management "


#End Region

        Public Shared Function GetService(ByVal Name As String, Optional ByVal random As Boolean = False) As Clients.IClientService
            Dim FoundServices As Collection(Of Clients.IClientService)
            FoundServices = GetServices(Name)
            If FoundServices.Count > 0 Then
                If Random Then
                    Dim randGen As New Random
                    Dim rndService As Integer = randGen.Next(0, FoundServices.Count - 1)
                    Return FoundServices(rndService)
                Else
                    Return FoundServices(0)
                End If
            Else
                Throw New InvalidOperationException
            End If
        End Function

        Public Shared Function GetService(Of TClientService As Clients.IClientService)(Optional ByVal random As Boolean = False) As Clients.IClientService
            Dim FoundServices As Collection(Of TClientService)
            FoundServices = GetServices(Of TClientService)()
            If FoundServices.Count > 0 Then
                If random Then
                    Dim randGen As New Random
                    Dim rndService As Integer = randGen.Next(0, FoundServices.Count - 1)
                    Return FoundServices(rndService)
                Else
                    Return FoundServices(0)
                End If
            Else
                Throw New InvalidOperationException
            End If
        End Function

        Public Shared Function GetServices(ByVal Name As String) As Collection(Of Clients.IClientService)
            Dim FoundServices As New Collection(Of Clients.IClientService)
            For Each server As ClientServerBase In Instance.Servers.Values
                For Each service As Clients.IClientService In server.GetServices
                    If service.ServiceName = Name Then
                        FoundServices.Add(service)
                    End If
                Next
            Next
            Return FoundServices
        End Function

        Public Shared Function GetServices(Of TClientService As Clients.IClientService)() As Collection(Of TClientService)
            Dim FoundServices As New Collection(Of TClientService)
            For Each server As ClientServerBase In Instance.Servers.Values
                For Each service As TClientService In server.GetServices
                    If service.GetType Is GetType(TClientService) Then
                        FoundServices.Add(service)
                    End If
                Next
            Next
            Return FoundServices
        End Function

    End Class


End Namespace
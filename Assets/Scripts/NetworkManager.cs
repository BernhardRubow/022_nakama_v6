using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Nakama;

public class NetworkManager : MonoBehaviour
{
    // +++ events +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public ObjectEventDelegate OnMatchCreated = delegate {};
    public ObjectEventDelegate OnDebugMessage = delegate {};




    // +++ fields +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    Nakama.Client _client;
    ISession _session;
    IApiAccount _account;
    ISocket _socket;
    IMatch _match;
    [SerializeField] bool _initMatch;
    [SerializeField] UiManager _ui;
    private List<IUserPresence> _presences = new List<IUserPresence>();




    // +++ unity lifecycle ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    async void Start()
    {
        _client = new Client("defaultkey", "aws.mkwindweb2.de", 7350, false);

        // const string email = "hello@example.com";
        // const string password = "somesupersecretpassword";
        // _session = await _client.AuthenticateEmailAsync(email, password);
        _session = await _client.AuthenticateDeviceAsync(System.Guid.NewGuid().ToString());
        Debug.LogFormat("Authenticated session: {0}", _session);
        Debug.Log(_session.AuthToken); // raw JWT token
        Debug.LogFormat("User id '{0}'", _session.UserId);
        Debug.LogFormat("User username '{0}'", _session.Username);
        Debug.LogFormat("Session has expired: {0}", _session.IsExpired);
        Debug.LogFormat("Session expires at: {0}", _session.ExpireTime); // in seconds.

        _account = await _client.GetAccountAsync(_session);
        Debug.LogFormat("User id '{0}'", _account.User.Id);
        Debug.LogFormat("User username '{0}'", _account.User.Username);
        Debug.LogFormat("Account virtual wallet '{0}'", _account.Wallet);

        _socket = _client.CreateWebSocket();
        _socket.OnConnect += _socket_OnConnect;
        _socket.OnDisconnect += _socket_OnDisconnect;
        _socket.OnChannelMessage += _socket_OnChannelMessage;
        _socket.OnChannelPresence += _socket_OnChannelPresence;
        _socket.OnError += _socket_OnError;
        _socket.OnMatchmakerMatched += _socket_OnMatchmakerMatched;
        _socket.OnMatchPresence += _socket_OnMatchPresence;
        _socket.OnMatchState += _socket_OnMatchState;
        _socket.OnNotification += _socket_OnNotification;
        _socket.OnStatusPresence += _socket_OnStatusPresence;
        _socket.OnStreamPresence += _socket_OnStreamPresence;
        _socket.OnStreamState += _socket_OnStreamState;
        await _socket.ConnectAsync(_session);

        // register ui events 
        _ui.OnCreateMatch += OnUiCreateMatch;
        _ui.OnJoinMatch += OnUiJoinMatch;


        
    }




    // +++ used event handlers ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    private void _socket_OnConnect(object sender, System.EventArgs e)
    {
        this.OnDebugMessage("Connected to Nakama Multiplayer Server");
    }

    private void _socket_OnMatchPresence(object sender, IMatchPresenceEvent e)
    {
        this.OnDebugMessage("Presences in match have changed!");

        foreach(var p in e.Joins){
            this.OnDebugMessage(string.Format("Player {0} joins", p.UserId));
            if(_presences.Count(x => x.UserId == p.UserId) == 0)
                _presences.Add(p);
        }        
        List_Players(_presences);  
    }

    private async void OnUiCreateMatch(object eventArgs){
        _match = await _socket.CreateMatchAsync();
        this.OnDebugMessage(string.Format("Created match with ID: {0}", _match.Id));
        this.OnMatchCreated(_match.Id);
    }

    private async void OnUiJoinMatch(object eventArgs){
        this.OnDebugMessage("Joining match...");
        string matchId = eventArgs.ToString();
        _match = await _socket.JoinMatchAsync(matchId);
        foreach(var p in _match.Presences) _presences.Add(p);
        List_Players(_presences);
    }




    // +++ unused eventhandlers (so far) ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

    private void _socket_OnStreamState(object sender, IStreamState e)
    {
        Debug.Log("Call to socket_OnStreamState");
    }

    private void _socket_OnStreamPresence(object sender, IStreamPresenceEvent e)
    {
        Debug.Log("Call to socket_OnStreamPresence");
    }

    private void _socket_OnStatusPresence(object sender, IStatusPresenceEvent e)
    {
        Debug.Log("Call to socket_OnStatusPresence");
    }

    private void _socket_OnNotification(object sender, IApiNotification e)
    {
        Debug.Log("Call to socket_OnNotification");
    }

    private void _socket_OnMatchState(object sender, IMatchState e)
    {
        Debug.Log("Call to socket_OnMatchState");
    }

    private void _socket_OnMatchmakerMatched(object sender, IMatchmakerMatched e)
    {
        Debug.Log("Call to socket_OnMatchmakerMatched");
    }

    private void _socket_OnError(object sender, System.Exception e)
    {
        Debug.Log("Call to socket_OnError");
    }

    private void _socket_OnChannelPresence(object sender, IChannelPresenceEvent e)
    {
        Debug.Log("Call to socket_OnChannelPresence");
    }

    private void _socket_OnChannelMessage(object sender, IApiChannelMessage e)
    {
        Debug.Log("Call to socket_OnChannelMessage");
    }

    private void _socket_OnDisconnect(object sender, System.EventArgs e)
    {
        Debug.Log("Call to socket_OnDisconnect");
    }




    // +++ functions ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    private void List_Players(List<IUserPresence> presences){
        this.OnDebugMessage("Match Joined, listing players:");        
        foreach(var p in presences){
            this.OnDebugMessage(string.Format("--- {0}", p.UserId));
        }
    }
}

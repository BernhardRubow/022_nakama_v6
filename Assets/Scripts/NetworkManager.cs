using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Nakama;

public class NetworkManager : MonoBehaviour
{

    // +++ fields +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    Nakama.Client _client;
    ISession _session;
    IApiAccount _account;
    ISocket _socket;

    // Use this for initialization
    async void Start()
    {
        _client = new Client("defaultkey", "aws.mkwindweb2.de", 7350, false);

        const string email = "hello@example.com";
        const string password = "somesupersecretpassword";
        _session = await _client.AuthenticateEmailAsync(email, password);
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
    }

    private void _socket_OnStreamState(object sender, IStreamState e)
    {
        throw new System.NotImplementedException();
    }

    private void _socket_OnStreamPresence(object sender, IStreamPresenceEvent e)
    {
        throw new System.NotImplementedException();
    }

    private void _socket_OnStatusPresence(object sender, IStatusPresenceEvent e)
    {
        throw new System.NotImplementedException();
    }

    private void _socket_OnNotification(object sender, IApiNotification e)
    {
        throw new System.NotImplementedException();
    }

    private void _socket_OnMatchState(object sender, IMatchState e)
    {
        throw new System.NotImplementedException();
    }

    private void _socket_OnMatchPresence(object sender, IMatchPresenceEvent e)
    {
        throw new System.NotImplementedException();
    }

    private void _socket_OnMatchmakerMatched(object sender, IMatchmakerMatched e)
    {
        throw new System.NotImplementedException();
    }

    private void _socket_OnError(object sender, System.Exception e)
    {
        throw new System.NotImplementedException();
    }

    private void _socket_OnChannelPresence(object sender, IChannelPresenceEvent e)
    {
        throw new System.NotImplementedException();
    }

    private void _socket_OnChannelMessage(object sender, IApiChannelMessage e)
    {
        throw new System.NotImplementedException();
    }

    private void _socket_OnDisconnect(object sender, System.EventArgs e)
    {
        throw new System.NotImplementedException();
    }

    private void _socket_OnConnect(object sender, System.EventArgs e)
    {
        throw new System.NotImplementedException();
    }

    // Update is called once per frame
    void Update()
    {

    }
}

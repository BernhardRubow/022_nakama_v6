using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Nakama;

public class UiManager : MonoBehaviour
{

  // +++ events exposed +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
  public ObjectEventDelegate OnCreateMatch = delegate { };
  public ObjectEventDelegate OnJoinMatch = delegate { };
  public ObjectEventDelegate OnStartGame = delegate {};




  // +++ fields +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
  [SerializeField] private NetworkManager _networkManager;
  [SerializeField] private InputField _matchIdDisplay;
  [SerializeField] private InputField _matchIdToJoin;
  [SerializeField] private Text _debugText;
  [SerializeField] private Button _startGameButton;
	[SerializeField] private Button _createMatchButton;
	[SerializeField] private Button _joinMatchButton;
  [SerializeField] private GameObject _createJoinMatchUI;
  private string _textToAppend;





  // +++ unity life cycle +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
  void Start()
  {
    // initialize
    _debugText.text = "";

    // subscribe to events
    _networkManager.OnMatchCreated += OnMatchCreated;
    _networkManager.OnDebugMessage += OnDebugMessageReceived;
    _networkManager.OnNetworkGameStarted += OnNetworkMatchInitiated;

  }

  void Update()
  {
    if (!string.IsNullOrEmpty(_textToAppend))
    {
      _debugText.text += _textToAppend;
      _textToAppend = string.Empty;
    }
  }

  void Reset()
  {
    _networkManager = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();
  }



  // +++ event handler ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

  // +++ network manager events +++
  private void OnNetworkMatchInitiated(object eventArgs)
  {
    _createJoinMatchUI.SetActive(false);
  }
  void OnMatchCreated(object eventArgs)
  {
    _matchIdDisplay.text = eventArgs.ToString();
  }

  void OnDebugMessageReceived(object eventArgs)
  {
    _textToAppend += "\n" + eventArgs.ToString();
  }

  // +++ UI Events +++
  public void OnCreateMatchClicked()
  {
		this._createMatchButton.gameObject.SetActive(false);
    this._joinMatchButton.gameObject.SetActive(false);
    this._matchIdToJoin.gameObject.SetActive(false);
    this.OnCreateMatch(null);
  }

  public void OnJoinMatchClicked()
  {
    if(_matchIdToJoin.text == string.Empty) return;

    this._joinMatchButton.gameObject.SetActive(false);
    this._createMatchButton.gameObject.SetActive(false);
    this._matchIdDisplay.gameObject.SetActive(false);
    this._matchIdToJoin.gameObject.SetActive(false);
    this.OnJoinMatch(_matchIdToJoin.text);
  }



}

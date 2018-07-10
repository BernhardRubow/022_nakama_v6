using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Nakama;

public class UiManager : MonoBehaviour {

	// +++ events exposed +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	public ObjectEventDelegate OnCreateMatch = delegate {};
	public ObjectEventDelegate OnJoinMatch = delegate {};




	// +++ fields +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	[SerializeField] private NetworkManager _networkManager;
	[SerializeField] private InputField _matchIdDisplay;
	[SerializeField] private InputField _matchIdToJoin;
	[SerializeField] private Text _debugText;
	private string _textToAppend;
	




	// +++ unity life cycle +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	void Start () {
		_debugText.text = "";
		_networkManager.OnMatchCreated +=	OnMatchCreated;
		_networkManager.OnDebugMessage += OnDebugMessageReceived;
	}


	void Update()
	{
			if(!string.IsNullOrEmpty(_textToAppend)){
				_debugText.text += _textToAppend;
				_textToAppend = string.Empty;
			}
	}



	void Reset(){
		_networkManager = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();
	}

	// +++ event handler ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	void OnMatchCreated(object eventArgs){		
		_matchIdDisplay.text = eventArgs.ToString();
	}

	void OnDebugMessageReceived(object eventArgs){
		_textToAppend += "\n" + eventArgs.ToString();
	}

	public void OnCreateMatchClicked(){
		this.OnCreateMatch(null);
	}

	public void OnJoinMatchClicked(){
		this.OnJoinMatch(_matchIdToJoin.text);
	}

	

}

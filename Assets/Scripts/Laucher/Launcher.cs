using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using static UnityEngine.InputSystem.HID.HID;

public class Launcher : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private byte maxPlayersPerRoom = 8;
    [SerializeField]
    private byte minStartPlayersSize = 1;

    private bool isConnecting;
    private LauncherUIManager uiManager;


    void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        uiManager = LauncherUIManager.Instance;
    }

    private void Start()
    {
        Debug.Log("Start");
        Screen.SetResolution(1280, 1080, false);

		uiManager.OnControlPanel();
        uiManager.onConnectButtonClickedListener.AddListener(() =>
        {
            Connect();
        });
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("OnConnetedToMaster");

        if (isConnecting) {
            Debug.Log("isConnecting && PhotoneNetwork.JoinRandomRoom");

            PhotonNetwork.JoinRandomRoom();
        }
      
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("OnDisconnected");

        uiManager.OnControlPanel();   
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("OnJoinRandomFailed");

        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = maxPlayersPerRoom });
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("On Joined Room");

        if (PhotonNetwork.CurrentRoom.PlayerCount >= minStartPlayersSize && PhotonNetwork.IsMasterClient)
        {
			PhotonNetwork.LoadLevel("Lobby");
        }
    }
    
    public void Connect()
    {
        Debug.Log("Connect");

        isConnecting = true;

        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.JoinRandomRoom();
        } else { 
		    PhotonNetwork.ConnectUsingSettings();
		}

    }
    
}
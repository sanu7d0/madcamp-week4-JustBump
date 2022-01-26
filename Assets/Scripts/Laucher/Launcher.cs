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

    [System.Serializable]
    public struct BgmType
    {
        public string name;
        public AudioClip audio;
    }

    // Inspector 에표시할 배경음악 목록
    public BgmType[] BGMList;


    void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        uiManager = LauncherUIManager.Instance;
    }

    private void Start()
    {
        Debug.Log("Start");
        Screen.SetResolution(1920, 1080, false);
        Screen.orientation = ScreenOrientation.LandscapeLeft;

		uiManager.OnControlPanel();
        uiManager.onConnectButtonClickedListener.AddListener(() =>
        {
            Connect();
        });

        if (!LauncherBGM.Instance.isPlayingMusic) {
            LauncherBGM.Instance.StartPlaying(BGMList[0].audio);
        }
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
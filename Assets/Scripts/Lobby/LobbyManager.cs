using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    public static LobbyManager Instance;
    public GameObject playerPrefab;
    public GameObject timerPrefab;
    public RoomManager roomManager;

    void Start()
    {
        Instance = this;
        roomManager = new RoomManager(PhotonNetwork.CurrentRoom, timerPrefab);

        if (playerPrefab == null)
        {
            Debug.LogError("<Color=Red><a>Missing</a></Color> playerPrefab Reference. Please set it up in GameObject 'Game Manager'", this);
        }
        else
        {
            if(PlayerManager.LocalPlayerInstance == null) {
                Debug.LogFormat("We are Instantiating LocalPlayer from {0}", SceneManagerHelper.ActiveSceneName);
                PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector2(0f, 5f), Quaternion.identity, 0);
		    }
            else
            {
                Debug.LogFormat("Ignoring scene load for {0}", SceneManagerHelper.ActiveSceneName);
            }
        }
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(0);
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnPlayerEnteredRoom(Player other)
    {
        if (roomManager.CanStartGame()) { 
			this.roomManager.HideRoom();
            this.roomManager.Ready();
		}

        Debug.LogFormat("OnPlayerEnteredRoom() {0}", other.NickName); // not seen if you're the player connecting
    }

    public override void OnPlayerLeftRoom(Player other)
    {
        if(!roomManager.CanStartGame()) {
            this.roomManager.ShowRoom();
            this.roomManager.Wait();
		}
        Debug.LogFormat("OnPlayerLeftRoom() {0}", other.NickName); // seen when other disconnects
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void LoadArena()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            Debug.LogError("PhotonNetwork : Trying to Load a level but we are not the master Client");
        }
        Debug.LogFormat("PhotonNetwork : Loading Level : {0}", 1);
        PhotonNetwork.LoadLevel("Room");
    }
}

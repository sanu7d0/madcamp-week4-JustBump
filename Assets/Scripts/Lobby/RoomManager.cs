using System;
using Photon.Realtime;
using UnityEngine;
using Photon.Pun;

enum RoomState{ 
    Wait, Ready, Start
}

public class RoomManager : SingletonP<RoomManager>
{
    [SerializeField]
    private GameObject timerPrefab;
    private GameObject timerInstance;
    [SerializeField]
    public int MinStartGame;
    private LobbyUIManager lobbyUIManager;

    protected override void Awake()
    {
        base.Awake();

        lobbyUIManager = LobbyUIManager.Instance;
    }

    private RoomState roomState = RoomState.Wait;

    public void HideRoom() {
        PhotonNetwork.CurrentRoom.IsVisible = false;
    }

    public void ShowRoom() {
        PhotonNetwork.CurrentRoom.IsVisible = true;
    }
    
    public int GetCurrentUser() {
        Debug.Log(PhotonNetwork.CurrentRoom.PlayerCount);
        return PhotonNetwork.CurrentRoom.PlayerCount;
    }

    public bool CanStartGame() {
        return GetCurrentUser() >= MinStartGame;
    }


    public void Wait() {
        if (roomState != RoomState.Ready) {
            Debug.Log("Can not Wait");
            return;
		}
        
        roomState = RoomState.Wait;

        if (timerInstance != null && timerInstance.TryGetComponent<Timer>(out Timer ti)) {
            ti.Destory();
		}

		PhotonNetwork.CurrentRoom.IsVisible = true;
    }

    public void Ready() {
        Debug.Log("Ready");
        if(this.roomState != RoomState.Wait) {
            Debug.Log("Can not Ready");
            return;
	    }

        roomState = RoomState.Ready;

        if(PhotonNetwork.IsMasterClient) {
            Debug.Log("PhotonNetWork.IsMasterClient");
			timerInstance = PhotonNetwork.Instantiate(timerPrefab.name, new Vector3(0, 0, 0), Quaternion.identity);
		}
    }

}   

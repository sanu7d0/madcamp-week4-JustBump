using System;
using Photon.Realtime;
using UnityEngine;
using Photon.Pun;

enum RoomState{ 
    Wait, Ready, Start
}

public class RoomManager : SingletonP<RoomManager>
{
    public int MinStartGame = 2;
    public GameObject timer;
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
        return PhotonNetwork.CurrentRoom.PlayerCount;
    }

    public bool CanStartGame() {
        return GetCurrentUser() >= MinStartGame;
    }


    public void Wait() {
        Debug.Log("Wait");
        if (this.roomState != RoomState.Ready) {
            Debug.Log("Can not Wait");
            return;
		}
        
        this.roomState = RoomState.Wait;
        Timer.Instance.Destory();
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

			PhotonNetwork.Instantiate(timer.name, new Vector3(0, 0, 0), Quaternion.identity);
		}
    }

}   

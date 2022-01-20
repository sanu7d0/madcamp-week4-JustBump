using System;
using Photon.Realtime;
using UnityEngine;
using Photon.Pun;

enum RoomState{ 
    Wait, Ready, Start
}


public class RoomManager
{

    private Room room;
    private GameObject timerPrefab;
    private GameObject instancedTimer;
    private const int MinStartGame = 1;
    private RoomState roomState = RoomState.Wait;

    public RoomManager(Room room, GameObject timerPrefab)
    {
        this.room = room;
        this.timerPrefab = timerPrefab;
    }

    public void HideRoom() {
        this.room.IsVisible = false;
    }

    public void ShowRoom() {
        this.room.IsVisible = true;
    }
    
    public int GetCurrentUser() {
        return this.room.PlayerCount;
    }

    public bool CanStartGame() {
        return this.GetCurrentUser() >= MinStartGame;
    }


    public void Wait() { 
        if(this.roomState != RoomState.Ready) {
            Debug.Log("Can not Wait");
            return;
		}
        
        if(PhotonNetwork.IsMasterClient && instancedTimer is not null) {
            PhotonNetwork.Destroy(instancedTimer);
	    }
        this.roomState = RoomState.Wait;
    }

    public void Ready() {

        if(this.roomState != RoomState.Wait) {
            Debug.Log("Can not Ready");
            return;
	    }

        if(PhotonNetwork.IsMasterClient) { 
			instancedTimer = PhotonNetwork.Instantiate(this.timerPrefab.name, new Vector2(0f, 5f), Quaternion.identity, 0);
		}

        this.roomState = RoomState.Ready;
    }

    public void Start() {
        if(this.roomState != RoomState.Ready) {
            Debug.Log("Can not Start");
            return;
		}
        this.roomState = RoomState.Start;
    }


}

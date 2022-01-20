using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using Photon.Pun;

public class PlayerController : MonoBehaviourPunCallbacks
{
    private Vector2 _moveDir;

    public UnityEvent onAttack;

    public Vector2 moveDir { 
        get { return _moveDir; } 
    }

    void Start() {
    }

    void OnMove(InputValue value) {
        if(photonView.IsMine == false && PhotonNetwork.IsConnected == true) {
            return;
        }
		_moveDir = value.Get<Vector2>();
    }


    void OnAttack(InputValue value) {
        onAttack.Invoke();
    }

    void OnInventory(InputValue value) {
        Debug.Log(value);
    }

}

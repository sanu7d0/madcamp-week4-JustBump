using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using Photon.Pun;

public class PlayerController : MonoBehaviourPunCallbacks
{
    public UnityEvent onAttack;

    public UnityEvent onInteract;

    public UnityEvent onRoll;

    public UnityEvent onShoot;

    public UnityEvent onSwapWeapon;

    public Vector2 moveDir { 
        get; private set;
    }

    public Vector2 mousePos {
        get { return Mouse.current.position.ReadValue(); }
    }

    void OnShoot(InputValue input) {
        if (!CanControl()) {
            return;
		}
        onShoot.Invoke();
    }
    

    void OnMove(InputValue input) {
        if (!CanControl()) {
            return;
		}
		moveDir = input.Get<Vector2>();
    }


    void OnAttack(InputValue input) {
        if (!CanControl()) {
            return;
		}
        onAttack.Invoke();
    }

    void OnInteract(InputValue input) {
        if (!CanControl()) {
            return;
		}
        onInteract.Invoke();
    }

    void OnSwapWeapon(InputValue input) {
        if (!CanControl()) {
            return;
		}
        photonView.RPC("_OnSwapWeapon", RpcTarget.All);
    }

    [PunRPC]
    void _OnSwapWeapon() {
        onSwapWeapon.Invoke();
    }

    void OnRoll(InputValue input) {
        if (!CanControl()) {
            return;
		}
        onRoll.Invoke();
    }
    
    bool CanControl() { 
        if(photonView.IsMine == false && PhotonNetwork.IsConnected == true) {
            return false;
        }
        return true;
    }
}

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using Photon.Pun;

public class PlayerController : MonoBehaviourPunCallbacks
{
    public UnityEvent onAttack;

    public UnityEvent onInteract;

    public UnityEvent onJump;

    public UnityEvent onRoll;

    public UnityEvent onShoot;

    public Vector2 moveDir { 
        get; private set;
    }

    public Vector2 mousePos {
        get { return Mouse.current.position.ReadValue(); }
    }

    void OnShoot(InputValue value) {
        onShoot.Invoke();
    }

    void OnMove(InputValue value) {
        if(photonView.IsMine == false && PhotonNetwork.IsConnected == true) {
            return;
        }
		moveDir = value.Get<Vector2>();
    }


    void OnAttack(InputValue value) {
        onAttack.Invoke();
    }

    void OnInteract(InputValue value) {
        onInteract.Invoke();
    }

    void OnInventory(InputValue value) {
        Debug.Log(value);
    }

    void OnJump(InputValue value) {
        onJump.Invoke();
    }

    void OnRoll(InputValue value) {
        onRoll.Invoke();
    }

}

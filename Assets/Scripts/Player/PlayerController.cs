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

    public UnityEvent onSwapWeapon;

    public Vector2 moveDir { 
        get; private set;
    }

    public Vector2 mousePos {
        get { return Mouse.current.position.ReadValue(); }
    }

    void OnShoot(InputValue input) {
        onShoot.Invoke();
    }

    void OnMove(InputValue input) {
        if(photonView.IsMine == false && PhotonNetwork.IsConnected == true) {
            return;
        }
		moveDir = input.Get<Vector2>();
    }


    void OnAttack(InputValue input) {
        onAttack.Invoke();
    }

    void OnInteract(InputValue input) {
        onInteract.Invoke();
    }

    void OnSwapWeapon(InputValue input) {
        onSwapWeapon.Invoke();
    }

    void OnJump(InputValue input) {
        onJump.Invoke();
    }

    void OnRoll(InputValue input) {
        onRoll.Invoke();
    }

}

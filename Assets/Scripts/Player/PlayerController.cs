using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using Photon.Pun;

public class PlayerController : MonoBehaviourPunCallbacks
{
    #region Events & Values
    public UnityEvent onAttack;

    public UnityEvent onInteract;

    public UnityEvent onRoll;

    public UnityEvent onShoot;

    public UnityEvent onSwapWeapon;

    public UnityEvent onDrop;

    public Vector2 moveDir { 
        get; private set;
    }

    public Vector2 mousePos {
        get { return Mouse.current.position.ReadValue(); }
    }
    #endregion

    private PlayerMediator playerMediator;
    private PlayerDebug playerDebug;

    void Awake() {
        playerMediator = GetComponent<PlayerMediator>();
        playerDebug = GetComponent<PlayerDebug>();
    }

    public override void OnEnable()
    {
        base.OnEnable();
        moveDir = Vector2.zero;
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
        onSwapWeapon.Invoke();
    }

    void OnRoll(InputValue input) {
        if (!CanControl()) {
            return;
		}
        onRoll.Invoke();
    }

    void OnDrop(InputValue input) {
        if (!CanControl()) {
            return;
        }
        onDrop.Invoke();
    }
    
    bool CanControl() { 
        if (playerMediator.IsDead) {
            return false;
        }
        
        if (GameManager.Instance.DEBUG_OfflineMode)
            return true;
        
        if(photonView.IsMine == false && PhotonNetwork.IsConnected == true) {
            return false;
        }
        return true;
    }
}

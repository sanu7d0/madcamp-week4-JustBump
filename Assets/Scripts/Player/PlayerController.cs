using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private PlayerMovement playerMovement;

    private Vector2 _moveDir;

    public Vector2 moveDir { 
        get { return _moveDir; } 
    }

    void Start() {
        playerMovement = GetComponent<PlayerMovement>();


    }

    void OnMove(InputValue value) {
        _moveDir = value.Get<Vector2>();
    }

    void OnAttack(InputValue value) {
        Debug.Log(value);
    }

    void OnInventory(InputValue value) {
        Debug.Log(value);
    }

}

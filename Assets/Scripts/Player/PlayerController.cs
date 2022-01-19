using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    private Vector2 _moveDir;

    public UnityEvent onAttack;

    public Vector2 moveDir { 
        get { return _moveDir; } 
    }

    void Start() {

    }

    void OnMove(InputValue value) {
        _moveDir = value.Get<Vector2>();
    }

    void OnAttack(InputValue value) {
        onAttack.Invoke();
    }

    void OnInventory(InputValue value) {
        Debug.Log(value);
    }

}

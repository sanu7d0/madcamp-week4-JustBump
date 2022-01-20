using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private PlayerController playerController;
    private Vector2 lastMoveDir;
    Animator anim;
    [SerializeField] private float speed;
    [SerializeField] private float rollingSpeed;
    [SerializeField] private float rollingTime;

    private State state;
    private enum State {
        Normal,
        Rolling
    }

    void Awake() {
        rb = GetComponent<Rigidbody2D>();
        playerController = GetComponent<PlayerController>();
        anim = GetComponent<Animator>();
        state = State.Normal;
    }
    
    void Start() {
        playerController.onRoll.AddListener(HandleRoll);
    }

    void Update()
    {
        switch (state) {
            case State.Normal:
                HandleMovement();
                break;
            
            case State.Rolling:
                HandleRollSliding();
                break;
        }
    }
    
    private void HandleMovement() {
        Vector2 moveDir = playerController.moveDir;
        lastMoveDir = moveDir;

        bool isIdle = moveDir.x == 0 && moveDir.y == 0;
        if (isIdle) {
            anim.SetBool("isWalking", false);
        } else {
            if (TryMove(moveDir, speed * Time.deltaTime)) {
                anim.SetBool("isWalking", true);
            } else {
                anim.SetBool("isWalking", false);
            }
        }

        // Flip transform
        if (lastMoveDir.x < 0) {
            transform.SetPositionAndRotation(transform.position,
            Quaternion.Euler(0f, 180f, 0f));
        } else if (lastMoveDir.x > 0) {
            transform.SetPositionAndRotation(transform.position,
            Quaternion.Euler(0f, 0f, 0f));
        }
    }

    private void HandleRoll() {
        // Cannot roll while not moving or rolling
        if (lastMoveDir == Vector2.zero || state == State.Rolling) {
            return;
        }

        // Start roll
        StartCoroutine(RollRoutine());
    }

    IEnumerator RollRoutine() {
        state = State.Rolling;
        anim.SetBool("isRolling", true);
        Debug.Log("Roll start");

        yield return new WaitForSeconds(rollingTime);

        state = State.Normal;
        anim.SetBool("isRolling", false);
        Debug.Log("Roll end");
    }

    private void HandleRollSliding() {
        TryMove(lastMoveDir, rollingSpeed * Time.deltaTime);
    }

    private bool CanMove(Vector3 dir, float distance) {
        return Physics2D.Raycast(transform.position, dir, distance).collider == null;
    }

    private bool TryMove(Vector3 baseMoveDir, float distance) {
        Vector3 moveDir = baseMoveDir;
        bool canMove = CanMove(moveDir, distance);

        if (!canMove) {
            // Cannot move diagonally
            moveDir = new Vector3(moveDir.x, 0f).normalized;
            canMove = moveDir.x != 0f && CanMove(moveDir, distance);

            if (!canMove) {
                // Cannot move horizontally
                moveDir = new Vector3(0f, baseMoveDir.y).normalized;
                canMove = moveDir.y != 0f && CanMove(moveDir, distance);
            }
        }

        if (canMove) {
            lastMoveDir = moveDir;
            transform.position += moveDir * distance;
            return true;
        } else {
            return false;
        }
    }
}

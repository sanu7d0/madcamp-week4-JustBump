using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private PlayerMediator playerMediator;
    private PlayerController playerController;
    private Vector2 lastMoveDir;
    Animator anim;
    [SerializeField] private float speed;
    [SerializeField] private float rollingSpeed;
    [SerializeField] private float rollingTime;

    private State state;
    private enum State {
        Normal,
        Rolling,
        Falling
    }

    void Awake() {
        rb = GetComponent<Rigidbody2D>();
        playerController = GetComponent<PlayerController>();
        playerMediator = GetComponent<PlayerMediator>();
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
        
        case State.Falling:
            HandleFalling();
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

        yield return new WaitForSeconds(rollingTime);

        state = State.Normal;
        anim.SetBool("isRolling", false);
    }

    private void HandleRollSliding() {
        TryMove(lastMoveDir, rollingSpeed * Time.deltaTime);
    }

    private bool CanMove(Vector3 dir, float distance) {
        ContactFilter2D contactFilter = new ContactFilter2D();
        List<RaycastHit2D> results = new List<RaycastHit2D>(); 
        contactFilter.useTriggers = false;

        return Physics2D.Raycast(transform.position, dir, contactFilter, results , distance) == 0;
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

    public void StartFalling() {
        // Already falling
        if (state == State.Falling)
            return;
        
        state = State.Falling;
        // Debug.Log("Falling started");

        playerMediator.InvokeOnFall();
    }

    private void HandleFalling() {
        transform.localScale *= (1 - Time.deltaTime);
        transform.Rotate(0, 0, Time.deltaTime * 100f); // rotate speed
    }
}

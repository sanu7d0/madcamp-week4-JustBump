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
    [SerializeField] private float rollingImpulse;
    [SerializeField] private float rollingTime;

    private State state;
    private enum State {
        Normal,
        Rolling,
        Falling,
        Bumping
    }

    void Awake() {
        rb = GetComponent<Rigidbody2D>();
        playerController = GetComponent<PlayerController>();
        playerMediator = GetComponent<PlayerMediator>();
        anim = GetComponent<Animator>();
    }

    void OnEnable() {
        state = State.Normal;
        rb.velocity = Vector2.zero;
        transform.rotation = Quaternion.identity;
        transform.localScale = Vector3.one;
    }
    
    void Start() {
        playerController.onRoll.AddListener(TryRoll);
        playerMediator.AddListenerToOnBumped(() => {
            state = State.Bumping;

            // Restore to normal state after 0.75 sec
            TimerExtension.CreateEventTimer(() => {
                if (state == State.Bumping) {
                    state = State.Normal;
                }
            }, 0.75f);
        });
    }

    void FixedUpdate()
    {
        switch (state) {
        case State.Normal:
            HandleMovement();
            break;
        
        case State.Falling:
            HandleFalling();
            break;
        }
    }
    
    private void HandleMovement() {
        Vector2 moveDir = playerController.moveDir;
        lastMoveDir = moveDir;

        // rb.AddForce(moveDir * speed * Time.deltaTime);
        rb.velocity = moveDir * speed * Time.deltaTime;
        Debug.Log(rb.velocity);
        if (moveDir != Vector2.zero) {
            anim.SetBool("isWalking", true);
        } else {
            anim.SetBool("isWalking", false);
        }

        // Flip transform
        if (moveDir.x < 0) {
            transform.SetPositionAndRotation(transform.position,
            Quaternion.Euler(0f, 180f, 0f));
        } else if (moveDir.x > 0) {
            transform.SetPositionAndRotation(transform.position,
            Quaternion.Euler(0f, 0f, 0f));
        }
    }

    private void TryRoll() {
        // TODO: 구르기 쿨타임?
        
        // Cannot roll while not moving or rolling
        if (lastMoveDir == Vector2.zero || state == State.Rolling) {
            return;
        }

        // Start roll
        state = State.Rolling;
        anim.SetBool("isRolling", true);
        rb.AddForce(lastMoveDir * rollingImpulse, ForceMode2D.Impulse);

        // Release Roll State after x sec
        TimerExtension.CreateEventTimer(() => {
            if (state == State.Rolling) {
                state = State.Normal;
                anim.SetBool("isRolling", false);
            }
        }, rollingTime);
    }

    public void StartFalling() {
        // Already falling
        if (state == State.Falling)
            return;
        
        state = State.Falling;
        rb.velocity = Vector2.zero;

         anim.SetBool("isWalking", false);
         anim.SetBool("isRolling", false);

        playerMediator.InvokeOnFall();
    }

    private void HandleFalling() {
        transform.localScale *= (1 - Time.deltaTime);
        transform.Rotate(0, 0, Time.deltaTime * 100f); // rotate speed
    }
}

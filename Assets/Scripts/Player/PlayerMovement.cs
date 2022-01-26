using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerMovement : MonoBehaviourPunCallbacks
{
    private Rigidbody2D rb;
    private PlayerMediator playerMediator;
    private PlayerController playerController;
    private AudioSource audioSource;
    private Vector2 lastMoveDir;
    Animator anim;
    [SerializeField] private float defaultSpeed;
    private float speed;
    [SerializeField] private float rollingImpulse;
    [SerializeField] private float rollingTime;
    private bool isJumping;
    private float jumpingCurTime;
    [SerializeField] private float jumpingTotalTime;
    [SerializeField] private float jumpingSpeed;
    [SerializeField] private AudioClip fallingScream;

    private State state;
    private enum State {
        Normal,
        Rolling,
        Falling,
        Bumping
    }
    private bool speedIncrease;

    void Awake() {
        rb = GetComponent<Rigidbody2D>();
        playerController = GetComponent<PlayerController>();
        playerMediator = GetComponent<PlayerMediator>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        isJumping = false;
        speedIncrease = false;
    }

    public override void OnEnable()
    {
        state = State.Normal;
        rb.velocity = Vector2.zero;
        transform.rotation = Quaternion.identity;
        transform.localScale = Vector3.one;
        speed = defaultSpeed;
    }

    void jump() {
        Debug.Log("JUMPING!!!");
        jumpingCurTime = Time.time;

        isJumping = true;
    }

    private void Update() {
        // if (isJumping) {
        //     float jumpingTime = Time.time - jumpingCurTime;
        //     if (jumpingTime >= jumpingTotalTime) {
        //         isJumping = false;
        //     }
        //     transform.SetPositionAndRotation(new Vector3(transform.position.x + jumpingSpeed * Mathf.Cos(50) * jumpingTime, transform.position.y + -1 * (float)0.5 * (float)9.8 * jumpingTime * jumpingTime + jumpingTime * (jumpingSpeed * Mathf.Sin(50)), 0), Quaternion.identity);
        //     // transform.position = new Vector3(transform.position.x + jumpingSpeed * Mathf.Cos(50) * jumpingTime, transform.position.y + -1 * (float)0.5 * (float)9.8 * jumpingTime * jumpingTime + jumpingTime * (jumpingSpeed * Mathf.Sin(50)), 0);
        // }
    }

    void Start() {
        playerController.onRoll.AddListener(TryRoll);
        playerController.onJump.AddListener(jump);

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
        if (moveDir != Vector2.zero) {
            anim.SetBool("isWalking", true);
        } else {
            anim.SetBool("isWalking", false);
        }

        // Flip transform
        if (moveDir.x < 0) {
            transform.localRotation = Quaternion.Euler(0f, 180f, 0f);
        } else if (moveDir.x > 0) {
            transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
        }
    }

    private void TryRoll() {
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
        if(state == State.Falling) {
            return;
		}

        if (PhotonNetwork.IsMasterClient) { 
			state = State.Falling;
			rb.velocity = Vector2.zero;

			anim.SetBool("isWalking", false);
			anim.SetBool("isRolling", false);

			playerMediator.InvokeOnFall();
            audioSource.PlayOneShot(fallingScream);
            
			photonView.RPC("_StartFalling", RpcTarget.All);
		}
    }
    [PunRPC]
    public void _StartFalling() {
        // Already falling
        if (state == State.Falling)
            return;
		
		state = State.Falling;
        rb.velocity = Vector2.zero;
        anim.SetBool("isWalking", false);
        anim.SetBool("isRolling", false);
    }

    private void HandleFalling() {
        transform.localScale *= (1 - Time.deltaTime);
        transform.Rotate(0, 0, Time.deltaTime * 100f); // rotate speed
    }

    public void IncreaseSpeed(float _speed) {
        photonView.RPC("_IncreaseSpeed", RpcTarget.All, _speed);
    }
    
    [PunRPC]
    public void _IncreaseSpeed(float _speed) {
        speedIncrease = true;
        speed += _speed;
    }

    public void InitSpeed() {
        photonView.RPC("_InitSpeed", RpcTarget.All);
    }
    
    [PunRPC]
    public void _InitSpeed() {
        speedIncrease = false;
        speed = defaultSpeed;
    }

    public bool getSpeedIncreaseStatus() {
        return speedIncrease;
    }
}

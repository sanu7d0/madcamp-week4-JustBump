using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;

public class PlayerMovement : MonoBehaviourPunCallbacks
{
    private Rigidbody2D rb;
    private PlayerController playerController;
    private Vector2 lastMoveDir;
    private SpriteRenderer spriteRenderer;
    Animator anim;
    [SerializeField] public float jumpPower;

    [SerializeField] private float speed;

    void Awake() {
        rb = GetComponent<Rigidbody2D>();
        playerController = GetComponent<PlayerController>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    
    void Start() {
        playerController.onJump.AddListener(jump);
    }

    void Update()
    {
        HandleMovement();
    }

    void FixedUpdate() {
        
    }

    private void jump() {
        Debug.Log("Jump!!!");
        rb.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
    }

    private void HandleMovement() {
        Vector2 moveDir = playerController.moveDir;
        lastMoveDir = moveDir;

        // rb.AddForce(moveDir * speed * Time.deltaTime); // option 1
        rb.velocity = moveDir * speed * Time.deltaTime; // option 2

        if (rb.velocity.x == 0 && rb.velocity.y == 0) {
            anim.SetBool("isWalking", false);
        } else {
            anim.SetBool("isWalking", true);
        }

        // spriteRenderer.flipX = true;
        if (moveDir.x < 0) {
            spriteRenderer.flipX = true;
        } else if (moveDir.x > 0) {
            spriteRenderer.flipX = false;
        }

        // rb.MovePosition(rb.position + (moveDir * speed * Time.deltaTime));  // option 3
        // * MovePosition -> friction을 무시한다
    }

    /*private void HandleMovement() {
        Vector3 moveDir = playerController.moveDir;
        lastMoveDir = moveDir;

        bool isIdle = moveDir.x == 0 && moveDir.y == 0;
        if (isIdle) {
            // 여기에 idle 애니메이션 call 추가
            // ex) animator.playIdleAnimation(moveDir);
        } else {
            Vector3 targetMovePosition = transform.position + moveDir * speed * Time.deltaTime;
            RaycastHit2D raycastHit = Physics2D.Raycast(transform.position, moveDir, speed * Time.deltaTime);
            
            if (raycastHit.collider == null) {
                // No hit, can move
                transform.position = targetMovePosition;
                // 여기에 move 애니메이션 call 추가
            } else {
                // Test moving in vertical direction
                Vector3 testMoveDir = new Vector3(moveDir.x, 0f).normalized;
                targetMovePosition = transform.position + testMoveDir * speed * Time.deltaTime;
                raycastHit = Physics2D.Raycast(transform.position, testMoveDir, speed * Time.deltaTime);

                if (testMoveDir.x != 0f && raycastHit.collider == null) {
                    // Can move horizontally
                    lastMoveDir = testMoveDir;
                    // 여기에 move 애니메이션 call 추가
                    transform.position = targetMovePosition;
                } else {
                    // Test moving in horizontal direction 
                    testMoveDir = new Vector3(0f, moveDir.y).normalized;
                    targetMovePosition = transform.position + testMoveDir * speed * Time.deltaTime;
                    raycastHit = Physics2D.Raycast(transform.position, testMoveDir, speed * Time.deltaTime);

                    if (testMoveDir.y != 0f && raycastHit.collider == null) {
                        // Can move vertically
                        lastMoveDir = testMoveDir;
                        // 여기에 move 애니메이션 call 추가
                        transform.position = targetMovePosition;
                    } else {
                        // Cannot move vertically
                        // 여기에 idle 애니메이션 call 추가
                    }
                }
            }
        }
    }*/

    private void TrySomething() {

    }
}

using UnityEngine;
using UnityEngine.Events;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private PlayerController playerController;
    private Vector2 lastMoveDir;
    private SpriteRenderer spriteRenderer;
    private Transform transform;
    private int point_num = 20;
    Animator anim;
    private Vector2[] waypoints;
    private int wayPointIndex = 0;
    [SerializeField] public float jumpPower;
    [SerializeField] private float rollingSpeed;
    [SerializeField] private float speed;

    void Awake() {
        rb = GetComponent<Rigidbody2D>();
        playerController = GetComponent<PlayerController>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        transform = GetComponent<Transform>();
        waypoints = null;
        wayPointIndex = 0;
    }
    
    void Start() {
        playerController.onJump.AddListener(jump);
        playerController.onRoll.AddListener(roll);

    }

    void Update()
    {
        
    }

    void FixedUpdate() {
        HandleMovement();
        HandleRolling();
    }

    private void HandleRolling() {
        if (waypoints == null) return;
        if (wayPointIndex == point_num) {
            wayPointIndex = 0;
            waypoints = null;
            anim.SetBool("isRolling", false);
        }
        Vector2 currPos = transform.position;

        
        transform.position = Vector2.MoveTowards(currPos, waypoints[wayPointIndex++], 10 * Time.deltaTime);
    }

    private void jump() {
        Debug.Log("Jump!!!");
        rb.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
    }

    private void roll() {
        Debug.Log("Roll@@@");
        anim.SetBool("isRolling", true);
        
        waypoints = new Vector2[point_num];
        
        wayPointIndex = 0;
        for (int i = 0; i < point_num; i++) {
            Vector2 moveDir = playerController.moveDir;
            waypoints.SetValue(moveDir * i, i);
        }
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
            transform.SetPositionAndRotation(transform.position,
            Quaternion.Euler(0f, 180f, 0f));
        } else if (moveDir.x > 0) {
            transform.SetPositionAndRotation(transform.position,
            Quaternion.Euler(0f, 0f, 0f));
        }

        // if (moveDir.)

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

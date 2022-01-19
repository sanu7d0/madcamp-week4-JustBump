using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    private PlayerController playerController;

    [SerializeField] private float knockbackPower;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRange;
    [SerializeField] private LayerMask targetLayers;

    void Awake() {
        playerController = GetComponent<PlayerController>();
    }

    void Start() {
        playerController.onAttack.AddListener(TryAttack);
    }

    private void TryAttack() {
        // 공격 쿨타임 안 돌았으면 공격 실패
        if (true) {
            // 어택 로직, 애니메이션
            Collider2D[] hitTargets = 
                Physics2D.OverlapCircleAll(attackPoint.position, attackRange, targetLayers);
            
            foreach(Collider2D target in hitTargets) {
                // 데미지 로직
                Debug.Log("Player hit " + target.name);

                target.GetComponent<Rigidbody2D>().AddForce((target.transform.position - attackPoint.position).normalized * knockbackPower);
            }
        } else {

        }
    }

    void OnDrawGizmosSelected() {
        if (attackPoint == null)
            return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}

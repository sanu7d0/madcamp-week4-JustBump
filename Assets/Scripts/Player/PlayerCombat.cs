using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] private float knockbackPower;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRange;
    [SerializeField] private LayerMask targetLayers;

    private PlayerController playerController;

    public Weapon[] weapons {
        get; private set;
    }
    private int weaponSelection;

    void Awake() {
        playerController = GetComponent<PlayerController>();

        weapons = new Weapon[2];
        weaponSelection = 0;
        // Default weapon = Fist


        // 테스트용
        weapons[0] = GetComponentInChildren<Weapon>();
    }

    void Start() {
        playerController.onAttack.AddListener(TryAttack);
    }

    private void TryAttack() {
        if (weapons[weaponSelection].Use()) {
            // ...
        }
    }
}

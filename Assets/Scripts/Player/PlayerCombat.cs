using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    private PlayerController playerController;

    private Transform weaponHolder;
    public Transform[] weapons {
        get; private set;
    }
    private int currentSelection;
    private Weapon currentWeapon;

    public GameObject appleBullet;
    [SerializeField] public Transform shootPosition;
    [SerializeField] public float shootCoolTime;
    private float lastShootTime;

    void Awake() {
        playerController = GetComponent<PlayerController>();

        playerController.onAttack.AddListener(TryAttack);
        playerController.onSwapWeapon.AddListener(SwapWeapon);
        playerController.onShoot.AddListener(TryShoot);
    }

    void Start() {
        // Weapons
        weapons = new Transform[2];
        currentSelection = 0;

        weaponHolder = transform.Find("Weapon Holder");
        weapons[0] = weaponHolder.GetChild(0); // ?? something // null 이면 대체 넣기
        weapons[1] = weaponHolder.GetChild(1);

        currentWeapon = weapons[currentSelection].GetComponent<Weapon>();
        weapons[1].gameObject.SetActive(false);

        // Shoots
        lastShootTime = 0;
    }

    private void SwapWeapon() {
        weapons[currentSelection].gameObject.SetActive(false);
        
        if (currentSelection == weapons.Length - 1) {
            currentSelection = 0;
        } else {
            currentSelection += 1;
        }

        weapons[currentSelection].gameObject.SetActive(true);
        currentWeapon = weapons[currentSelection].GetComponent<Weapon>();
    }

    private void TryAttack() {
        if (currentWeapon == null) {
            Debug.LogError("No weapon to use");
            return;
        }
        
        if (currentWeapon.Use()) {
            // ...
        }
    }

    private void TryShoot() {
        if (Time.time < lastShootTime + shootCoolTime) {
            return;
        }

        ShootingBullet bullet = Instantiate(appleBullet, shootPosition.position, transform.rotation)
            .GetComponent<ShootingBullet>();
        bullet.direction = Camera.main.ScreenToWorldPoint(playerController.mousePos)
            - shootPosition.position;
    }
}

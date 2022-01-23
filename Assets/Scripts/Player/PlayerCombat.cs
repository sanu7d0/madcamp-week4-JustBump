using UnityEngine;
using Photon.Pun;

public class PlayerCombat : MonoBehaviourPunCallbacks
{
    private PlayerController playerController;

    [SerializeField] private Transform weaponHolder;
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

        weapons[0] = weaponHolder.GetChild(0); // ?? something // null 이면 대체 넣기
        weapons[1] = weaponHolder.GetChild(1);

        currentWeapon = weapons[currentSelection].GetComponent<Weapon>();
        weapons[1].gameObject.SetActive(false);

        // Shoots
        lastShootTime = 0;
    }

    private void SwapWeapon() {
        photonView.RPC("_SwapWeapon", RpcTarget.All);
    }
    [PunRPC]
    private void _SwapWeapon() {
        weapons[currentSelection].gameObject.SetActive(false);
        
        if (currentSelection == weapons.Length - 1) {
            currentSelection = 0;
        } else {
            currentSelection += 1;
        }

        weapons[currentSelection].gameObject.SetActive(true);
        currentWeapon = weapons[currentSelection].GetComponent<Weapon>();
    }

    public void ChangeCurrentWeapon(GameObject newWeapon) {
        photonView.RPC("_ChangeCurrentWeapon", RpcTarget.All, newWeapon.GetComponent<PhotonView>().ViewID);
    }
    [PunRPC]
    public void _ChangeCurrentWeapon(int newWeaponId) {
        Destroy(weapons[currentSelection].gameObject);
        
        GameObject newWeapon = PhotonView.Find(newWeaponId).gameObject;
        weapons[currentSelection] = newWeapon.transform;

        newWeapon.transform.parent = weaponHolder;
        newWeapon.transform.SetSiblingIndex(currentSelection);
        newWeapon.transform.position = weaponHolder.position;
        newWeapon.transform.localPosition = Vector3.zero;
        newWeapon.transform.rotation = weaponHolder.rotation;
        // rotation

        currentWeapon = newWeapon.GetComponent<Weapon>();
    }

    private void TryAttack() {
        if (currentWeapon == null) {
            Debug.LogError("No weapon to use");
            return;
        }
        
        switch (currentWeapon.GetWeaponType()) {
        case WeaponCategory.Melee:
            if (currentWeapon.Use()) {
                // ...
            }
            break;
        
        case WeaponCategory.Range:

            break;
        
        case WeaponCategory.Throwable:
            Debug.Log(currentWeapon.Use(shootPosition.position, Camera.main.ScreenToWorldPoint(playerController.mousePos)));
            // if (currentWeapon.Use(Camera.main.ScreenToWorldPoint(playerController.mousePos))) {
            //     // ...
            // }
            break;
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

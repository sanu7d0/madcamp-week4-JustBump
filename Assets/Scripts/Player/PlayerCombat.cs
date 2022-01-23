using System;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;

public class PlayerCombat : MonoBehaviourPunCallbacks
{
    private PlayerController playerController;

    [SerializeField] private Transform weaponHolder;
    [SerializeField] private GameObject defaultFistPrefab;

    // Tuple<Weapon, bool> -> bool = Is weapon occupied?
    public Tuple<Weapon, bool>[] weapons {
        get; private set;
    }
    private int curWeponIdx;
    private Weapon_Fist deafultFist;

    public GameObject appleBullet;
    [SerializeField] public Transform shootPosition;
    [SerializeField] public float shootCoolTime;
    private float lastShootTime;

    public UnityEvent onWeaponChange;

    void Awake() {
        playerController = GetComponent<PlayerController>();

        playerController.onAttack.AddListener(TryAttack);
        playerController.onSwapWeapon.AddListener(SwapWeapon);
        playerController.onShoot.AddListener(TryShoot);

        weapons = new Tuple<Weapon, bool>[2];

        onWeaponChange = new UnityEvent();
    }

    void Start() {
        // Instantiate default fist
        GameObject _defaultFist =
            PhotonNetwork.Instantiate(defaultFistPrefab.name, weaponHolder.position, Quaternion.identity);
        _defaultFist.transform.parent = weaponHolder;
        deafultFist = _defaultFist.GetComponent<Weapon_Fist>();
    }

    public override void OnEnable()
    {
        base.OnEnable();
        for (int i = 0; i < weapons.Length; i++) {
            weapons[i] = new Tuple<Weapon, bool>(deafultFist, false);
        }
        curWeponIdx = 0;
        onWeaponChange.Invoke();

        // Shoots
        lastShootTime = 0;
    }

    private void ClearWeapon() {
        photonView.RPC("_ClearWeapon", RpcTarget.All);
    }
    [PunRPC]
    private void _ClearWeapon() {
        for (int i = 0; i < weapons.Length; i++) {
            Tuple<Weapon, bool> w = weapons[i];

            if (w.Item2 && w.Item1.TryGetComponent<PhotonView>(out PhotonView target)) {
                PhotonNetwork.Destroy(target);
            }
            weapons[i] = new Tuple<Weapon, bool>(deafultFist, false);
        }
        
        onWeaponChange.Invoke();
        curWeponIdx = 0;
    }

    private void SwapWeapon() {
        photonView.RPC("_SwapWeapon", RpcTarget.All);
    }
    [PunRPC]
    private void _SwapWeapon() {
        if (weapons[curWeponIdx].Item2) {
            weapons[curWeponIdx].Item1.gameObject.SetActive(false);
        }
        
        if (curWeponIdx == weapons.Length - 1) {
            curWeponIdx = 0;
        } else {
            curWeponIdx += 1;
        }

        if (weapons[curWeponIdx].Item2) {
            weapons[curWeponIdx].Item1.gameObject.SetActive(true);
        }

        onWeaponChange.Invoke();
    }

    public void SetWeaponAt(GameObject newWeapon, int idx = -1) {
        // -1 -> Change current
        if (idx == -1) {
            idx = curWeponIdx;
        }
        photonView.RPC("_SetWeaponAt", RpcTarget.All, new object[]{newWeapon.GetComponent<PhotonView>().ViewID, idx});
    }
    [PunRPC]
    public void _SetWeaponAt(int newWeaponId, int changeIdx) {
        // If not empty, spit out weapon
        if (weapons[changeIdx].Item2) {
            // If durability = 0, destroy
            Weapon oldWeapon = weapons[curWeponIdx].Item1;
            if (oldWeapon.weaponDurability > 0) {
                oldWeapon.WeaponToFieldDrop(transform.position);
            }
            
            if (photonView.IsMine) {
                PhotonNetwork.Destroy(oldWeapon.gameObject);
            }
        }
        
        GameObject newWeapon = PhotonView.Find(newWeaponId).gameObject;
        weapons[curWeponIdx] = new Tuple<Weapon, bool>(newWeapon.GetComponent<Weapon>(), true);

        newWeapon.transform.parent = weaponHolder;
        newWeapon.transform.position = weaponHolder.position;
        newWeapon.transform.localPosition = Vector3.zero;
        newWeapon.transform.rotation = weaponHolder.rotation;

        onWeaponChange.Invoke();
    }

    private void TryAttack() {
        if (!weapons[curWeponIdx].Item2) {
            deafultFist.Use();
            return;
        }
        
        Weapon curWeapon = weapons[curWeponIdx].Item1;
        
        WeaponUseResult result = WeaponUseResult.Normal;
        switch (curWeapon.GetWeaponType()) {
        case WeaponCategory.Melee:
            result = curWeapon.Use();
            break;
        
        case WeaponCategory.Range:

            break;
        
        case WeaponCategory.Throwable:
            result = curWeapon.Use(
                shootPosition.position, 
                Camera.main.ScreenToWorldPoint(playerController.mousePos));
            break;
        }

        // If all used, drop it
        if (result == WeaponUseResult.AllUsed) {
            PhotonNetwork.Destroy(curWeapon.gameObject);
            weapons[curWeponIdx] = new Tuple<Weapon, bool>(deafultFist, false);
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

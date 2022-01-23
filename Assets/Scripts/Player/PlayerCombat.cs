using System;
using UnityEngine;
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

    void Awake() {
        playerController = GetComponent<PlayerController>();

        playerController.onAttack.AddListener(TryAttack);
        playerController.onSwapWeapon.AddListener(SwapWeapon);
        playerController.onShoot.AddListener(TryShoot);

        weapons = new Tuple<Weapon, bool>[2];
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
            weapons[i] = new Tuple<Weapon, bool>(null, false);
        }
        curWeponIdx = 0;

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
            weapons[i] = new Tuple<Weapon, bool>(null, false);
        }
        
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
    }

    public void SetWeaponAt(GameObject newWeapon, int idx = -1) {
        // -1 -> Change current
        if (idx == -1) {
            idx = curWeponIdx;
        }
        photonView.RPC("_SetWeaponAt", RpcTarget.All, newWeapon.GetComponent<PhotonView>().ViewID, idx);
    }
    [PunRPC]
    public void _SetWeaponAt(int newWeaponId, int changeIdx) {
        // If empty, just change
        if (!weapons[changeIdx].Item2) {

        } else {
            // If not empty, spit weapon out
            
            // TODO : Spit 으로 바꾸기
            PhotonNetwork.Destroy(weapons[curWeponIdx].Item1.gameObject);
        }
        
        GameObject newWeapon = PhotonView.Find(newWeaponId).gameObject;
        weapons[curWeponIdx] = new Tuple<Weapon, bool>(newWeapon.GetComponent<Weapon>(), true);

        newWeapon.transform.parent = weaponHolder;
        newWeapon.transform.position = weaponHolder.position;
        newWeapon.transform.localPosition = Vector3.zero;
        newWeapon.transform.rotation = weaponHolder.rotation;
    }

    private void TryAttack() {
        if (!weapons[curWeponIdx].Item2) {
            deafultFist.Use();
            return;
        }
        
        Weapon curWeapon = weapons[curWeponIdx].Item1;
        switch (curWeapon.GetWeaponType()) {
        case WeaponCategory.Melee:
            if (curWeapon.Use()) {
                // ...
            }
            break;
        
        case WeaponCategory.Range:

            break;
        
        case WeaponCategory.Throwable:
            Debug.Log(curWeapon.Use(
                shootPosition.position, 
                Camera.main.ScreenToWorldPoint(playerController.mousePos)));
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

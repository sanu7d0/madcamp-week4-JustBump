using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;

public class PlayerInventory : MonoBehaviourPunCallbacks
{
    private PlayerController playerController;

    [SerializeField] private Transform weaponHolder;
    [SerializeField] private GameObject defaultFistPrefab;

    // Tuple<Weapon, bool> -> bool = Is weapon occupied?
    public Weapon[] weapons {
        get; private set;
    }
    private int curWeaponIdx;
    public Weapon currentWeapon {
        get { return weapons[curWeaponIdx]; }
    }

    private Weapon_Fist defaultFist;

    public UnityEvent onWeaponChange;


    void Awake() {
        playerController = GetComponent<PlayerController>();

        playerController.onSwapWeapon.AddListener(SwapWeapon);
        playerController.onDrop.AddListener(DropCurrentWeapon);

        // Instantiate default fist
        GameObject _defaultFist =
            PhotonNetwork.Instantiate(defaultFistPrefab.name, weaponHolder.position, Quaternion.identity);
        _defaultFist.transform.parent = weaponHolder;
        defaultFist = _defaultFist.GetComponent<Weapon_Fist>();

        weapons = new Weapon[2];
        for (int i = 0; i < weapons.Length; i++) {
            weapons[i] = defaultFist;
            Debug.Log($"Weapon {i} is Weapon_Fist: {weapons[i] is Weapon_Fist}");
        }

        onWeaponChange = new UnityEvent();
    }

    public override void OnEnable()
    {
        base.OnEnable();
        
        curWeaponIdx = 0;
        onWeaponChange.Invoke();
    }

    private void ClearWeapon() {
        photonView.RPC("_ClearWeapon", RpcTarget.All);
    }
    [PunRPC]
    private void _ClearWeapon() {
        for (int i = 0; i < weapons.Length; i++) {
            if (weapons[i] is Weapon_Fist) {
                // nothing
                Debug.Log($"Weapon {i} is already default");
            } else {
                PhotonNetwork.Destroy(weapons[i].GetComponent<PhotonView>());
                weapons[i] = defaultFist;
                Debug.Log($"Weapon {i} set to default");
            }
        }
        
        onWeaponChange.Invoke();
        curWeaponIdx = 0;
    }

    private void SwapWeapon() {
        photonView.RPC("_SwapWeapon", RpcTarget.All);
    }
    [PunRPC]
    private void _SwapWeapon() {
        if (currentWeapon is not Weapon_Fist) {
            currentWeapon.gameObject.SetActive(false);
        }
        
        if (curWeaponIdx == weapons.Length - 1) {
            curWeaponIdx = 0;
        } else {
            curWeaponIdx += 1;
        }

        if (currentWeapon is not Weapon_Fist) {
            currentWeapon.gameObject.SetActive(true);
        }

        onWeaponChange.Invoke();
    }

    public void DropCurrentWeapon() {
        photonView.RPC("_DropCurrentWeapon", RpcTarget.All);
    }
    
    [PunRPC]
    private void _DropCurrentWeapon() {
        if (currentWeapon is not Weapon_Fist) {
            if (currentWeapon.weaponDurability > 0) {
                currentWeapon.WeaponToFieldDrop(transform.position);
            } else {
                // If durability = 0, destroy
                if (photonView.IsMine) {
                    PhotonNetwork.Destroy(currentWeapon.gameObject);
                }
            }

            weapons[curWeaponIdx] = defaultFist;
        }
    }

    public void SetWeaponAt(GameObject newWeapon, int idx = -1) {
        // -1 -> Change current
        if (idx == -1) {
            idx = curWeaponIdx;
        }
        photonView.RPC("_SetWeaponAt", RpcTarget.All, newWeapon.GetComponent<PhotonView>().ViewID, idx);
    }
    [PunRPC]
    private void _SetWeaponAt(int newWeaponId, int changeIdx) {
        Weapon oldWeapon = weapons[changeIdx];
        
        // If not weapon empty
        if (oldWeapon is not Weapon_Fist) {
            if (oldWeapon.weaponDurability > 0) {
                oldWeapon.WeaponToFieldDrop(transform.position);
            } else {
                // If durability = 0, destroy
                if (photonView.IsMine) {
                    PhotonNetwork.Destroy(oldWeapon.gameObject);
                }
            }
        }
        
        GameObject newWeapon = PhotonView.Find(newWeaponId).gameObject;
        newWeapon.SetActive(true);
        weapons[changeIdx] = newWeapon.GetComponent<Weapon>();

        newWeapon.transform.parent = weaponHolder;
        newWeapon.transform.position = weaponHolder.position;
        newWeapon.transform.localPosition = Vector3.zero;
        newWeapon.transform.rotation = weaponHolder.rotation;

        onWeaponChange.Invoke();
    }
}

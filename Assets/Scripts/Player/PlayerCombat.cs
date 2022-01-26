using System;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;

public class PlayerCombat : MonoBehaviourPunCallbacks
{
    private PlayerController playerController;
    private PlayerInventory playerInventory;

    public GameObject appleBullet;
    [SerializeField] private Transform weaponHolder;
    [SerializeField] public Transform shootPosition;
    [SerializeField] public float shootCoolTime;
    private float lastShootTime;

    void Awake() {
        playerController = GetComponent<PlayerController>();
        playerInventory = GetComponent<PlayerInventory>();

        playerController.onAttack.AddListener(TryAttack);
        playerController.onShoot.AddListener(TryShoot);
    }

    public override void OnEnable()
    {
        base.OnEnable();

        // Shoots
        lastShootTime = 0;
    }

    void Update() {
        if (photonView.IsMine) {
            Vector2 lookDir = (Camera.main.ScreenToWorldPoint(playerController.mousePos)
                - transform.position).normalized;
            
            weaponHolder.transform.up = lookDir;
        }
    }

    private void TryAttack() {
        Weapon currentWeapon = playerInventory.currentWeapon;
        
        WeaponUseResult result = WeaponUseResult.Normal;
        float delayTime = 0f;

        switch (currentWeapon.GetWeaponType()) {
        case WeaponCategory.Melee:
            result = currentWeapon.Use();
            break;
        
        case WeaponCategory.Range:

            break;
        
        case WeaponCategory.Throwable:
            result = currentWeapon.Use(
                shootPosition.position, 
                Camera.main.ScreenToWorldPoint(playerController.mousePos));
            break;
        }

        // If all used, drop it
        if (result == WeaponUseResult.AllUsed) {
            playerInventory.DropCurrentWeapon(delayTime);
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

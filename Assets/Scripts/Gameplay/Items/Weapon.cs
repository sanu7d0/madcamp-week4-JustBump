using UnityEngine;

[RequireComponent (typeof (AudioSource))]
public abstract class Weapon : MonoBehaviour
{   
    [SerializeField] protected WeaponObject weapon;
    protected AudioSource audioSource;

    protected float lastUseTime = 0;

    protected virtual void Start() {
        weapon = weapon.GetClone();
        audioSource = GetComponent<AudioSource>();
    }

    public virtual bool Use() {
        weapon.durability -= 1;
        if (weapon.durability <= 0) {
            AllUsed();
        }
        return true;
    }

    public virtual bool Use(Vector2 targetPosition) {
        weapon.durability -= 1;
        if (weapon.durability <= 0) {
            AllUsed();
        }
        return true;
    }

    public virtual WeaponCategory GetWeaponType() {
        return weapon.category;
    }

    protected virtual void AllUsed() {

    }

    protected virtual void PlayUseMotion() {

    }

    protected virtual void PlayUseSound() {
        if (weapon.useSound != null) {
            audioSource.PlayOneShot(weapon.useSound);
        }
    }
}

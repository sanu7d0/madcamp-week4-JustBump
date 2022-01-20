using UnityEngine;

public abstract class Weapon : MonoBehaviour
{   
    [SerializeField] protected WeaponObject weapon;

    protected float lastUseTime = 0;

    protected virtual void Start() {
        weapon = weapon.GetClone();
    }

    public virtual bool Use() {
        weapon.durability -= 1;
        if (weapon.durability <= 0) {
            AllUsed();
        }
        return true;
    }

    protected virtual void AllUsed() {

    }
}

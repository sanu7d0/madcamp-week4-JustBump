using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponCategory { Melee, Range, Throwable };

[CreateAssetMenu(fileName = "New Weapon Object", menuName = "Weapons")]
public class WeaponObject : ScriptableObject
{
    public new string name;
    public WeaponCategory category;
    public float power;
    public int durability;
    public float coolTime;

    public AudioClip useSound;

    public WeaponObject GetClone() {
        return ScriptableObjectExtension.Clone<WeaponObject>(this);
    }
}

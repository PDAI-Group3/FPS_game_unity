using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Gun", menuName = "Gun")]
public class Gun : ScriptableObject
{

    public string name;
    public float aimSpeed;
    //fireRate is the amount of seconds to be waited between shooting
    public float fireRate;
    public int maxAmmo;
    //how much 1 bullet does damage
    public int damage;
    

    public GameObject prefab;
}

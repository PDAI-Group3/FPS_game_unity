using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;


public class Weapon : MonoBehaviour
{

    public Gun[] loadout;
    public Transform weaponParent;
    public GameObject bulletholePrefab;
    public LayerMask canBeShot;

    private int currentIndex;

    private GameObject currentWeapon;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Alpha1)) Equip(0);

        if (currentWeapon != null)
        {
            Aim(Input.GetMouseButton(1));

            
            if (Input.GetMouseButtonDown(0))
            {
                Shoot();
            }

            

            
        }
    }

    void Equip (int p_ind)
    {
        if (currentWeapon != null) Destroy(currentWeapon);

        currentIndex = p_ind;

        GameObject t_newWeapon = Instantiate (loadout[p_ind].prefab, weaponParent.position, weaponParent.rotation, weaponParent) as GameObject;
        t_newWeapon.transform.localPosition = Vector3.zero;
        t_newWeapon.transform.localEulerAngles = Vector3.zero;

        currentWeapon = t_newWeapon;

    }

    void Aim(bool p_isAiming)
    {
        Transform t_anchor = currentWeapon.transform.Find("Anchor");
        Transform t_state_ads = currentWeapon.transform.Find("States/ADS");
        Transform t_state_hip = currentWeapon.transform.Find("States/Hip");

        if (p_isAiming)
        {
            //aiming
            t_anchor.position = Vector3.Lerp(t_anchor.position, t_state_ads.position, Time.deltaTime * loadout[currentIndex].aimSpeed);
        }
        else
        {
            //hip
            t_anchor.position = Vector3.Lerp(t_anchor.position, t_state_hip.position, Time.deltaTime * loadout[currentIndex].aimSpeed);
        }
    }

    void Shoot()
    {
        

        RaycastHit t_hit = new RaycastHit();
        if (Physics.Raycast(GameObject.Find("Player/PlayerCamera").transform.position, GameObject.Find("Player/PlayerCamera").transform.TransformDirection(Vector3.forward), out t_hit, Mathf.Infinity, canBeShot))
        {
            GameObject t_newBulletHole = Instantiate(bulletholePrefab, t_hit.point + t_hit.normal * 0.001f, Quaternion.identity) as GameObject;
            t_newBulletHole.transform.LookAt(t_hit.point + t_hit.normal);

            //hole disappears in given seconds
            Destroy(t_newBulletHole, 5f);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using Unity.Netcode;


public class NetworkWeapon : NetworkBehaviour
{
    public override void OnNetworkSpawn() {
        if(!IsOwner) {this.enabled = false;}
    }
    public Gun[] loadout;
    public Transform weaponParent;
    public GameObject bulletholePrefab;
    public LayerMask canBeShot;

    private int currentIndex;

    private GameObject currentWeapon;

    [SerializeField]
    Camera playerCam;

    public ulong clientId;

    public AudioClip shootingSound;
    public AudioClip reloadSound;
    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        clientId = NetworkManager.LocalClientId;
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(IsServer) {
            if (Input.GetKeyUp(KeyCode.Alpha1)) EquipClientRpc(0);

            if (currentWeapon != null)
            {
                AimClientRpc(Input.GetMouseButton(1));

                
                if (Input.GetMouseButtonDown(0))
                {
                    Vector3 transformPos = playerCam.transform.position;
                    Vector3 transformDir = playerCam.transform.TransformDirection(Vector3.forward);
                    ShootClientRpc(transformPos, transformDir);
                }
            }
        }
        else {
            if (Input.GetKeyUp(KeyCode.Alpha1)) EquipServerRpc(0);

            if (currentWeapon != null)
            {
                AimServerRpc(Input.GetMouseButton(1));

                
                if (Input.GetMouseButtonDown(0))
                {
                    Vector3 transformPos = playerCam.transform.position;
                    Vector3 transformDir = playerCam.transform.TransformDirection(Vector3.forward);
                    ShootServerRpc(transformPos, transformDir);
                }
            }
        }
    }

    [ClientRpc]
    void EquipClientRpc(int p_ind)
    {   

            if (currentWeapon != null) Destroy(currentWeapon);

            currentIndex = p_ind;

            GameObject t_newWeapon = Instantiate (loadout[p_ind].prefab, weaponParent.position, weaponParent.rotation, weaponParent) as GameObject;
            t_newWeapon.transform.localPosition = Vector3.zero;
            t_newWeapon.transform.localEulerAngles = Vector3.zero;

            currentWeapon = t_newWeapon;
    }

    [ServerRpc(RequireOwnership = false)]
    public void EquipServerRpc(int p_ind) {
        EquipClientRpc(p_ind);
    }

    [ClientRpc]
    void AimClientRpc(bool p_isAiming)
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

    [ServerRpc(RequireOwnership = false)]
    void AimServerRpc(bool p_isAiming)
    {
                AimClientRpc(p_isAiming);
    }

    [ClientRpc]
    void ShootClientRpc(Vector3 transformPos, Vector3 transformDir)
    {
        //shooting sound
        audioSource.PlayOneShot(shootingSound, 0.8f);

        RaycastHit t_hit = new RaycastHit();
            if (Physics.Raycast(transformPos, transformDir, out t_hit, Mathf.Infinity, canBeShot))
            {

            if(t_hit.collider.gameObject.GetComponent<CapsuleCollider>()) {
                t_hit.collider.gameObject.GetComponent<NetworkHealth>().TakeDamage(loadout[currentIndex].damage);
                return;
            }

            GameObject t_newBulletHole = Instantiate(bulletholePrefab, t_hit.point + t_hit.normal * 0.001f, Quaternion.identity) as GameObject;
            t_newBulletHole.transform.LookAt(t_hit.point + t_hit.normal);

            //hole disappears in given seconds
            Destroy(t_newBulletHole, 5f);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    void ShootServerRpc(Vector3 transformPos, Vector3 transformDir)
    {
        ShootClientRpc(transformPos, transformDir);
    }
}

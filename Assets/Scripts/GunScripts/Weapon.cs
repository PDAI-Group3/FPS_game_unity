using System.Collections;
using UnityEngine;
using TMPro;


public class Weapon : MonoBehaviour
{

    public Gun[] loadout;
    public Transform weaponParent;
    public GameObject bulletholePrefab;
    public LayerMask canBeShot;
    public int currentAmmo;
    public int magazineSize;

    public float reloadTime = 2f;
    private bool isReloading = false;

    private bool isShooting = false;

    private int currentIndex;

    public Animator animator;
    private InputManager inputManager;
    private GameObject currentWeapon;

    public TextMeshProUGUI ammoCountText;

    public AudioClip shootingSound;
    public AudioClip reloadSound;
    AudioSource audioSource;


    // Start is called before the first frame update
    void Start()
    {
        inputManager = GetComponent<InputManager>();
        ammoCountText = FindObjectOfType<TextMeshProUGUI>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        

        if (Input.GetKeyUp(KeyCode.Alpha1)) Equip(0);

        if (currentWeapon != null)
        {
            if(currentAmmo >= 0)
            {
                ammoCountText.text = currentAmmo.ToString() + " / " + magazineSize;
            }else if(isReloading == true)
            {
                ammoCountText.text = "0" + " / " + magazineSize;
            }

            if (inputManager.onFoot.Reload.triggered && !isReloading)
            {
                
                StartCoroutine(Reload());
            }

            Aim(Input.GetMouseButton(1));

            
            if (Input.GetMouseButtonDown(0))
            {
                Shoot();
                if(currentAmmo == 0 && !isReloading)
                {
                    StartCoroutine(Reload());
                }
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

        currentAmmo = loadout[currentIndex].maxAmmo;
        magazineSize = loadout[currentIndex].maxAmmo;

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
        if (isShooting == true)
            return;

        if (currentAmmo >= 0 && !isReloading) {
            currentAmmo--;
        }
        else
        {
            return;
        }
        if (isReloading == true)
            return;

        //shooting sound
        audioSource.PlayOneShot(shootingSound, 0.8f);

        RaycastHit t_hit = new RaycastHit();
        if (Physics.Raycast(GameObject.Find("Player/PlayerCamera").transform.position, GameObject.Find("Player/PlayerCamera").transform.TransformDirection(Vector3.forward), out t_hit, Mathf.Infinity, canBeShot))
        {

            if(t_hit.collider.gameObject.TryGetComponent<EnemyAI>(out EnemyAI enemyAIComponent))
            {
                enemyAIComponent.TakeDamage(loadout[currentIndex].damage);
                StartCoroutine(firerateWait());
                return;
            }

            GameObject t_newBulletHole = Instantiate(bulletholePrefab, t_hit.point + t_hit.normal * 0.001f, Quaternion.identity) as GameObject;
            t_newBulletHole.transform.LookAt(t_hit.point + t_hit.normal);
            StartCoroutine(firerateWait());
            //hole disappears in given seconds
            Destroy(t_newBulletHole, 5f);
        }

        
    }

    IEnumerator Reload()
    {
        isReloading = true;

        audioSource.PlayOneShot(reloadSound, 0.5f);

        weaponParent.GetComponent<Animator>().SetBool("isReloading", true);

        yield return new WaitForSeconds(reloadTime);
        
        weaponParent.GetComponent<Animator>().SetBool("isReloading", false);

        currentAmmo = loadout[currentIndex].maxAmmo;
        isReloading = false;
    }

    IEnumerator firerateWait()
    {
        isShooting = true;

        weaponParent.GetComponent<Animator>().SetBool("isShooting", true);

        yield return new WaitForSeconds(loadout[currentIndex].fireRate);

        weaponParent.GetComponent<Animator>().SetBool("isShooting", false);
        isShooting = false;
    }
}

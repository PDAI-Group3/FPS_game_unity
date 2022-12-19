using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class BotFOVDetection : MonoBehaviour
{

    public Transform player;
    public float maxAngle;
    public float maxRadius;

    NavMeshAgent nav;

    private bool isInFOV = false;

    
    public float botFireRate;
    bool isShooting = false;
    public LayerMask LocalPlayer;
    public Transform projectile;
    public Transform shotTarget;
    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        
        
    }

    private void OnDrawGizmos()
    {

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, maxRadius);

        Vector3 fovLine1 = Quaternion.AngleAxis(maxAngle, transform.up) * transform.forward * maxRadius;
        Vector3 fovLine2 = Quaternion.AngleAxis(-maxAngle, transform.up) * transform.forward * maxRadius;

        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, fovLine1);
        Gizmos.DrawRay(transform.position, fovLine2);

        Gizmos.color = Color.black;
        Gizmos.DrawLine(transform.position, transform.forward * maxRadius);

    }

    public static bool inFOV(Transform checkingObject, Transform target, float maxAngle, float maxRadius)
    {

        Collider[] overlaps = new Collider[100]; // overlaps ottaa 10 objektia botin ympärillä
        int count = Physics.OverlapSphereNonAlloc(checkingObject.position, maxRadius, overlaps);

        for (int i = 0; i < count + 1; i++)
        {

            if (overlaps[i] != null) // jos overlaps-taulukko ei oo tyhjä
            {

                if (overlaps[i].transform == target) // jos overlaps taulukossa on target (meidän pelaaja)
                {

                    //Debug.Log("You are in the bots environment array");
                    Vector3 directionBetween = (target.position - checkingObject.position).normalized;
                    directionBetween.y *= 0; // korkeus otetaan pois FOV tarkistuksesta eli y aina 0

                    float angle = Vector3.Angle(checkingObject.forward, directionBetween);

                    if (angle <= maxAngle)
                    {

                        Ray ray = new Ray(checkingObject.position, target.position - checkingObject.position);
                        RaycastHit hit;

                        if (Physics.Raycast(ray, out hit, maxRadius))
                        {

                            Debug.Log("Raycast hit!");
                            //return true;

                            
                            if (hit.transform == target)
                            {
                                Debug.Log("Bot sees you");
                                return true; // näkee pelaajan
                            }
                            // ylempi if ei toimi jostain syystä, raycast ei tunnu osuvan meihin, vaikka me ollaan target
                            // testattu ylempänä ja ylempi toimii muutenkin, rivin 55 debug.log toimii kun me astutaan botin lähelle
                        }
                    }
                }
            }
        }
        return false; // ei nää pelaajaa
    }

    private void Update()
    {

        isInFOV = inFOV(transform, player, maxAngle, maxRadius);

        if (isInFOV)
        {
            Debug.Log("in isinfov");
            nav.SetDestination(player.position);
            // to do :
            // ampuminen
            
            if (isShooting == false)
            {
                
                Shoot();
                //StartCoroutine(AfterShooting());
            }
            
        }
    }

     
    public void Shoot()
    {
        Debug.Log("shoot funktion alussa");
        RaycastHit t_hit = new RaycastHit();
        if (Physics.Raycast(GameObject.Find("Bot/Eyes").transform.position, GameObject.Find("Bot/Eyes").transform.TransformDirection(Vector3.forward), out t_hit, Mathf.Infinity, LocalPlayer))
        {
            Debug.Log("shoot raycast shot");
            if (t_hit.collider.gameObject.TryGetComponent<Health>(out Health HealthComponent))
            {
                Debug.Log("pelaajaan osu");
                HealthComponent.TakeDamage(2);

                return;
            }
            Debug.Log("pelaajaan ei osunu");
            //GameObject t_newBulletHole = Instantiate(bulletholePrefab, t_hit.point + t_hit.normal * 0.001f, Quaternion.identity) as GameObject;

        }
        Debug.Log("shot if ei toiminu");
    }
    
    /* //delay after shooting
    IEnumerator AfterShooting()
    {
        Debug.Log("aftershotissa");
        isShooting = true;
        yield return new WaitForSeconds(botFireRate);
        isShooting = false;
    }*/
}

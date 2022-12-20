using UnityEngine;
using UnityEngine.AI;


public class BotFOVDetection : MonoBehaviour {

    NavMeshAgent nav;
    public Transform player;
    public float maxAngle;
    public float maxRadius;
    private bool isInFOV = false;
    public LayerMask LocalPlayer;

    void Start() { nav = GetComponent<NavMeshAgent>(); }

    public static bool inFOV(Transform checkingObject, Transform target, float maxAngle, float maxRadius) {

        Collider[] overlaps = new Collider[100]; // overlaps ottaa 10 objektia botin ympärillä
        int count = Physics.OverlapSphereNonAlloc(checkingObject.position, maxRadius, overlaps);

        for (int i = 0; i < count + 1; i++) {

            if (overlaps[i] != null) { // jos overlaps-taulukko ei oo tyhjä

                if (overlaps[i].transform == target) { // jos overlaps taulukossa on target (meidän pelaaja)

                    Vector3 directionBetween = (target.position - checkingObject.position).normalized;
                    directionBetween.y *= 0; // korkeus otetaan pois FOV tarkistuksesta eli y aina 0

                    float angle = Vector3.Angle(checkingObject.forward, directionBetween);

                    if (angle <= maxAngle) {

                        Ray ray = new Ray(checkingObject.position, target.position - checkingObject.position);
                        RaycastHit hit;

                        if (Physics.Raycast(ray, out hit, maxRadius)) { return true; }
                    }
                }
            }
        }
        return false; // ei nää pelaajaa
    }

    private void Update() {
        isInFOV = inFOV(transform, player, maxAngle, maxRadius);
        if (isInFOV) { nav.SetDestination(player.position); }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, maxRadius);
        Vector3 fovLine1 = Quaternion.AngleAxis(maxAngle, transform.up) * transform.forward * maxRadius;
        Vector3 fovLine2 = Quaternion.AngleAxis(-maxAngle, transform.up) * transform.forward * maxRadius;
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, fovLine1);
        Gizmos.DrawRay(transform.position, fovLine2);
    }
}

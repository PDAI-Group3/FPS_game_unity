using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using static UnityEngine.Physics;
public class NetworkHealth : NetworkBehaviour {
    public override void OnNetworkSpawn() {
        if(!IsOwner) {this.enabled = false;}
    }

    [Header("Health Bar")]
    public int  health;
    public int maxHealth = 9;
    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;


    [Header("Damage Overlay")]
    public Image overlay; // Red overlay for taking damage
    public float duration; // Duration for the overlay
    public float fadeSpeed; // how fast the image fades away

    private float durationTimer;
    private bool death = false;

    public ulong clientId;

    private void Start()
    {
        clientId = NetworkManager.LocalClientId;
        health = maxHealth;
        overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, 0);
    }

    private void Update()
    {
        UpdateHealthUI();
        if (overlay.color.a > 0)
        {
            durationTimer += Time.deltaTime;
            if (durationTimer > duration)
            {
                // fade image
                float tempAlpha = overlay.color.a;
                tempAlpha -= Time.deltaTime * fadeSpeed;
                overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, tempAlpha);
            }
        }

        death = CheckHealth();

        if (death) {
            if(IsServer) {
                health = maxHealth;
                gameObject.transform.position = new Vector3(0,1,0);
                SyncTransforms();
            }
            else {
                health = maxHealth;
                gameObject.transform.position = new Vector3(0,1,0);
                SyncTransforms();
            }
        }

        // //testing damage
        // if (Input.GetKeyDown(KeyCode.J))
        // {
        //     TakeDamage(1);
        // }
        // //testing healing
        // if (Input.GetKeyDown(KeyCode.H))
        // {
        //     TakeHeal(1);
        // }

    }

    public void UpdateHealthUI()
    {
        if (health > maxHealth)
        {
            health = maxHealth;
        }

        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < health)
            {
                hearts[i].sprite = fullHeart;
            }
            else
            {
                hearts[i].sprite = emptyHeart;
            }

            if (i < maxHealth)
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
            }
        }
    }

    public void TakeDamage(int damage)
    {
            health -= damage;
            durationTimer = 0;
            overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, 0.25f);
    }

    public void TakeHeal(int healAmount)
    {
        health += healAmount;
    }

    public bool CheckHealth()
    {
        if(health <= 0)
        {
            return true;
        }
        return false;
    }



/*     public void doSyncTransforms() {
        SyncTransforms();
    }

    [ClientRpc]
    public void DeathClientRpc(ulong client) {
        if (IsServer) {
            NetworkObject player = NetworkManager.ConnectedClients[client].PlayerObject;
            player.gameObject.transform.position = new Vector3(0,1,0);
            doSyncTransforms();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void DeathServerRpc(ulong client) {
        DeathClientRpc(client);
    } */
}

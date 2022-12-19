using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Physics;

public class Health : MonoBehaviour
{
    [Header("Health Bar")]
    public int health;
    public int maxHealth = 9;
    public int deathCounter = 0;
    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;
    
    // RESPAWN PLAYER VARIABLES
    public GameObject player;
    public GameObject spawnPoint1;
    public GameObject spawnPoint2;
    public GameObject spawnPoint3;

    private Vector3 location1;
    private Vector3 location2;
    private Vector3 location3;
    // RESPAWN PLAYER VARIABLES

    [Header("Damage Overlay")]
    public Image overlay; // Red overlay for taking damage
    public float duration; // Duration for the overlay
    public float fadeSpeed; // how fast the image fades away

    private float durationTimer;
    private bool death = false;

    private void Start()
    {
        health = maxHealth;
        overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, 0);

        location1 = spawnPoint1.transform.position;
        location2 = spawnPoint2.transform.position;
        location3 = spawnPoint3.transform.position;
    }

    private void LateUpdate()
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

        //testing damage
        if (Input.GetKeyDown(KeyCode.J))
        {
            TakeDamage(1);
        }
        //testing healing
        if (Input.GetKeyDown(KeyCode.H))
        {
            TakeHeal(1);
        }
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

    public void TakeDamage(int damageAmount)
    {
        health -= damageAmount;
        durationTimer = 0;
        overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, 0.25f);

        death = CheckHealth();

        if (death)
        {
            
            deathCounter += 1;
            Debug.Log("kuollut " + deathCounter + " kertaa");

            if (deathCounter <= 1)
            {
                player.transform.position = new Vector3(location1.x, location1.y, location1.z);
            }
            else if (deathCounter == 2)
            {
                player.transform.position = new Vector3(location2.x, location2.y, location2.z);
            }
            else if (deathCounter >= 3)
            {
                player.transform.position = new Vector3(location3.x, location3.y, location3.z);
            }

            health = maxHealth;
            SyncTransforms();
            // synkataan pelaajan sijaintimuutokset fysiikkamoottorin kanssa, respawn ei toimi muutwen
        }
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
}
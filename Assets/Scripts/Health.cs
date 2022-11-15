using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [Header("Health Bar")]
    public int health;
    public int maxHealth = 9;
    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;


    [Header("Damage Overlay")]
    public Image overlay; // Red overlay for taking damage
    public float duration; // Duration for the overlay
    public float fadeSpeed; // how fast the image fades away

    private float durationTimer;

    private void Start()
    {
        health = maxHealth;
        overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, 0);
    }

    void Update()
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


        //UpdateHealthUI();
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
    }

    public void TakeHeal(int healAmount)
    {
        health += healAmount;
    }
}

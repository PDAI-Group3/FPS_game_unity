using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{

    [SerializeField]
    float healthAI, maxHealthAI = 9f;


    // Start is called before the first frame update
    void Start()
    {
        healthAI = maxHealthAI;
    }

    
    public void TakeDamage(float damageAmount)
    {
        healthAI -= damageAmount;

        if(healthAI <= 0)
        {
            Destroy(gameObject);
        }



    }
}

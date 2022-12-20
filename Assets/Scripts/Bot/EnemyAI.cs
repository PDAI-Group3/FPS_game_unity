using UnityEngine;

public class EnemyAI : MonoBehaviour
{

    [SerializeField]
    float healthAI, maxHealthAI = 9f;

    public GameObject bot;
    public GameObject botSpawner;

    private Vector3 spawnerLocation1;

    void Start()
    {
        healthAI = maxHealthAI;
        spawnerLocation1 = botSpawner.transform.position;
    }

    
    public void TakeDamage(float damageAmount)
    {
        healthAI -= damageAmount;

        if(healthAI <= 0)
        {
            healthAI = maxHealthAI;
            bot.transform.position = new Vector3(spawnerLocation1.x, spawnerLocation1.y, spawnerLocation1.z);
            //Destroy(gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    [SerializeField]
    private GameObject botPrefab;

    [SerializeField]
    private float spawnInterval = 4f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(spawnEnemey(spawnInterval, botPrefab));
    }

    // Update is called once per frame
    private IEnumerator spawnEnemey(float interval, GameObject enemy)
    {
        yield return new WaitForSeconds(interval);
        GameObject newEnemy = Instantiate(enemy, new Vector3(Random.Range(-5f, 5), Random.Range(-6f, 6), 3), Quaternion.identity);
        newEnemy.transform.position = transform.position;
        StartCoroutine(spawnEnemey(interval, enemy));
    }
}

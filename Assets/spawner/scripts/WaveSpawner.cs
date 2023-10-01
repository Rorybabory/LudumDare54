using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public Transform[] spawnPoints;
    public GameObject enemyPrefab;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void enemyWave()
    {
        Debug.Log("Enemy spawn!");
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            enemySpawn(spawnPoints[i]);
        }
    }
    
    void enemySpawn(Transform transform)
    {
        Spawner spawner = GetComponent<Spawner>();
        spawner.prefabObj = enemyPrefab;
        spawner.Spawn(default, default, transform);
    }
}

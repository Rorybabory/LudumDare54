using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using UnityEngine;

public class WaveSpawner : MonoBehaviour {

    [SerializeField] private int currentWave;

    [SerializeField] private WaveObject[] waves;
    [SerializeField] private Transform[] spawnpoints;
    [SerializeField] private Transform[] spawnpointsSkull;

    [SerializeField] private float waveDuration;

    public int CurrentWave => this.currentWave;

    public event Action<int> Spawned;

    private float waveTimer = Mathf.Infinity;
    private int points;
    List<GameObject> spawnedEnemies;
    void Start()
    {

        newWave();
    }
    void newWave()
    {
        spawnedEnemies = waves[currentWave].Spawn(spawnpoints, spawnpointsSkull);

        currentWave++;
        waveTimer = 0;
        this.Spawned?.Invoke(spawnedEnemies.Count());
    }
    private void Update() {
        Debug.Log("Wave count: " + spawnedEnemies.Count());

        if (spawnedEnemies.Count() <= 0)
        {
            Debug.Log("ALL ENEMIES KILLED!!!!!!");
            newWave();
        }

        spawnedEnemies.RemoveAll(e => e == null);

        waveTimer += Time.deltaTime;

        if (waveTimer > waveDuration) {

            if (currentWave >= waves.Length) {
                return;
            }
            
        }
        
    }
}

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

    [SerializeField] private float waveDuration;

    public int CurrentWave => this.currentWave;

    public event Action<int> Spawned;

    private float waveTimer = Mathf.Infinity;
    private int points;

    private void Update() {

        waveTimer += Time.deltaTime;

        if (waveTimer > waveDuration) {

            if (currentWave >= waves.Length) {
                Debug.Log("Done");
                return;
            }

            var spawnedEnemies = waves[currentWave].Spawn(spawnpoints);

            currentWave++;
            waveTimer = 0;
            this.Spawned?.Invoke(spawnedEnemies.Count());
        }
    }
}

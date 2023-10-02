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
    void Start()
    {
        var spawnedEnemies = waves[currentWave].Spawn(spawnpoints, spawnpointsSkull);

        currentWave++;
        waveTimer = 0;
        this.Spawned?.Invoke(spawnedEnemies.Count());

    }
    private void Update() {

        waveTimer += Time.deltaTime;

        if (waveTimer > waveDuration) {

            if (currentWave >= waves.Length) {
                return;
            }

        }
    }
}

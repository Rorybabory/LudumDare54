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
    [SerializeField] private SoundEffect waveBeatenSound;

    [SerializeField] private float waveDuration;

    public int CurrentWave => this.currentWave;

    public event Action<int> Spawned;

    private float waveTimer = Mathf.Infinity;
    private int points;
    private List<GameObject> spawnedEnemies;

    private void Start()
    {
        waveBeatenSound.Init(gameObject);

        NewWave();
    }

    private void NewWave()
    {
        waveBeatenSound.Play();

        spawnedEnemies = waves[currentWave].Spawn(spawnpoints, spawnpointsSkull);

        currentWave++;
        if (currentWave >= waves.Length) currentWave = 0;

        waveTimer = 0;
        this.Spawned?.Invoke(spawnedEnemies.Count());
    }

    private void Update() {

        if (spawnedEnemies.Count() <= 0) NewWave();

        spawnedEnemies.RemoveAll(e => e == null);

        waveTimer += Time.deltaTime;

        if (waveTimer > waveDuration) {

            if (currentWave >= waves.Length) {
                return;
            }
            
        }
        
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Security;
using UnityEngine;

public class WaveSpawner : MonoBehaviour {

    [SerializeField] private int currentWave;

    [SerializeField] private WaveObject[] waves;
    [SerializeField] private Transform[] spawnpoints;

    [SerializeField] private float waveDuration;

    private float waveTimer = Mathf.Infinity;
    private int points;

    private void Update() {

        waveTimer += Time.deltaTime;

        if (waveTimer > waveDuration) {

            if (currentWave >= waves.Length) {
                Debug.Log("Done");
                return;
            }

            waves[currentWave].Spawn(spawnpoints);

            currentWave++;
            waveTimer = 0;
        }
    }
}

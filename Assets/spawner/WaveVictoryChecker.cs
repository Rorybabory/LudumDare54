using System;
using Channels;
using UnityEngine;

namespace spawner
{
    public class WaveVictoryChecker : MonoBehaviour
    {
        [SerializeField]
        private WaveSpawner spawner;
        [SerializeField]
        private GameObjectChannel enemyKilledChannel;
        [SerializeField]
        private int numberOfWavesRequiredToWin = 10;
        private int numberOfEnemies;
        [SerializeField]
        private GameObject victoryScreen;

        private void Awake()
        {
            this.spawner.Spawned += OnWaveSpawned;
            this.enemyKilledChannel.Raised += this.OnEnemyKilled;
        }

        private void OnDestroy()
        {
            this.spawner.Spawned -= this.OnWaveSpawned;
            this.enemyKilledChannel.Raised -= this.OnEnemyKilled;
        }

        private void ShowVictoryScreen()
        {
            this.victoryScreen.SetActive(true);
        }
        
        private void OnWaveSpawned(int count)
        {
            this.numberOfEnemies += count;
        }
        
        private void OnEnemyKilled(GameObject obj)
        {
            if (this.numberOfEnemies < 0)
            {
                if (this.spawner.CurrentWave >= this.numberOfWavesRequiredToWin)
                {
                    this.ShowVictoryScreen();
                }
            }
        }
    }
}
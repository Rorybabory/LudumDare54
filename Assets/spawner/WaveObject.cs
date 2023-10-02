using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Wave", menuName = "Systems/Enemy Wave")]
public class WaveObject : ScriptableObject {

    public int points;

    public List<Enemy> enemies;

    [System.Serializable]
    public class Enemy {
        public GameObject prefab;
        public int pointCost;
    }

    public void Spawn(Transform[] spawnpoints) {

        int points = this.points;

        while (enemies.Exists(e => points >= e.pointCost)) {

            Enemy enemy = enemies.Random();
            while (enemy.pointCost > points) enemy = enemies.Random();

            Instantiate(enemy.prefab, spawnpoints.Random().position, Quaternion.identity);
            points -= enemy.pointCost;
        }
    }
}

public static class SpawnUtil {
    public static T Random<T>(this List<T> list) => list[UnityEngine.Random.Range(0, list.Count)];
    public static T Random<T>(this T[] array) => array[UnityEngine.Random.Range(0, array.Length)];
}
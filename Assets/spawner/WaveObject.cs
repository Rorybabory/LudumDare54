using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Wave", menuName = "Systems/Enemy Wave")]
public class WaveObject : ScriptableObject {

    public int points;

    public List<Enemy> enemies;
    public Enemy skull;

    [System.Serializable]
    public class Enemy {
        public GameObject prefab;
        public int pointCost;
    }

    public IEnumerable<GameObject> Spawn(Transform[] spawnpoints, Transform[] spawnpoints_skull)
    {
        var list = new List<GameObject>();
        int points = this.points;

        while (enemies.Exists(e => points >= e.pointCost)) {
            bool isSkull = false;
            
            Enemy enemy = enemies.Random();
            
            while (enemy.pointCost > points) enemy = enemies.Random();
            if (enemy.prefab.name == "Skull")
            {
                isSkull = true;
            }

            GameObject instance;
            if (isSkull)
            {
                instance = Instantiate(enemy.prefab, spawnpoints_skull.Random().position, Quaternion.identity);
            }else
            {
                instance = Instantiate(enemy.prefab, spawnpoints.Random().position, Quaternion.identity);
            }
            points -= enemy.pointCost;

            list.Add(instance);
        }

        return list;
    }
}

public static class SpawnUtil {
    public static T Random<T>(this List<T> list) => list[UnityEngine.Random.Range(0, list.Count)];
    public static T Random<T>(this T[] array) => array[UnityEngine.Random.Range(0, array.Length)];
}
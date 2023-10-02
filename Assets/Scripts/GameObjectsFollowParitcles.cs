using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class GameObjectsFollowParitcles : MonoBehaviour {

    [SerializeField] private ParticleSystem system;
    [SerializeField] private List<GameObject> prefabs;

    private List<GameObject> gameObjects = new();

    private void Awake() => ClearChildren();

    private void ClearChildren() {
        while (transform.childCount != 0)
            DestroyImmediate(transform.GetChild(0).gameObject);
    }

    private void Update() {
        if (!Application.isPlaying) return;
        CreateShips();
    }

    private void CreateShips() {

        var particles = new ParticleSystem.Particle[system.particleCount];
        system.GetParticles(particles);

        gameObjects.RemoveAll(g => g == null);
        foreach (var g in gameObjects)
            g.SetActive(false);

        Transform Get() {

            var activeObjectIndex = gameObjects.FindIndex(g => !g.activeInHierarchy);

            if (activeObjectIndex != -1) {
                var activeObject = gameObjects[activeObjectIndex];
                activeObject.SetActive(true);
                return activeObject.transform;
            }

            else {
                var newGo = Instantiate(prefabs.Random(), transform);
                gameObjects.Add(newGo);
                return newGo.transform;
            }
        }

        foreach (var particle in particles) {
            var ship = Get();
            ship.SetPositionAndRotation(particle.position, Quaternion.Euler(particle.rotation3D));
            ship.localScale = particle.GetCurrentSize3D(system);
        }

        var extra = gameObjects.FindAll(g => !g.activeSelf);
        foreach (var gameObject in extra) {
            gameObjects.Remove(gameObject);
            DestroyImmediate(gameObject);
        }
    }
}

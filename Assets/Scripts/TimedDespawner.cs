using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedDespawner : MonoBehaviour
{
    public float timer;
    public float timerEnd;

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= timerEnd)
        {
            Destroy(gameObject);
        }
    }
}

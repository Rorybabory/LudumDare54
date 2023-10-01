using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SkullEnemy : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Transform playerTransform;
    [SerializeField] private GameObject fireballPrefab;
    private Vector3 basePos;

    private float timetomove;
    private float movetimer;

    private float shoottimer;
    private float timetoshoot;

    private Vector3 offsetvelocity;
    private Vector3 offset;


    void Start()
    {
        basePos = transform.position;
        timetomove = Random.Range(0f, 1f);
        offsetvelocity = new Vector3(0, 0, 0);
        movetimer = 0;
        timetoshoot = Random.Range(0.5f, 4.0f);
        shoottimer = 0;
    }
    void ShootFireball()
    {
        GameObject fireball = Instantiate(fireballPrefab, transform);
        fireball.transform.parent = null;
        fireball.transform.position = transform.position + new Vector3(0, -0.75f, 0.0f);
        Debug.Log("Shoot Fireball");
    }
    // Update is called once per frame
    void Update()
    {
        float wave = Mathf.Sin(Time.time) * 0.35f;
        Vector3 temppos = transform.position;
        transform.position = basePos + new Vector3(0, wave, 0) + offset;
        float target_angle = Mathf.Atan2(this.playerTransform.position.x - this.transform.position.x, this.playerTransform.position.z - this.transform.position.z);
        target_angle *= 180.0f / Mathf.PI;
        Quaternion newrot = transform.rotation;
        newrot = Quaternion.Euler(0, target_angle, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, newrot, Time.time * 0.001f);
        movetimer += Time.deltaTime;
        if (movetimer > timetomove)
        {
            timetomove = Random.Range(1f, 3f);
            movetimer = 0;
            offsetvelocity = new Vector3(Random.Range(-40f, 10f), Random.Range(-3.8f, 3.8f), Random.Range(-40f, 20f));
        }
        shoottimer += Time.deltaTime;
        if (shoottimer > timetoshoot)
        {
            ShootFireball();
            shoottimer = 0;
            timetoshoot = Random.Range(0.5f, 4.0f);
        }
        offset += offsetvelocity * Time.deltaTime;
    }
    void FixedUpdate()
    {
        offset *= 0.96f;
        offsetvelocity *= 0.95f;
    }
}

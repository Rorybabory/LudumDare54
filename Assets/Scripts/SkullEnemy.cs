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

    private Rigidbody rb;

    void Start()
    {
        basePos = transform.position;
        timetomove = Random.Range(0f, 1f);
        offsetvelocity = new Vector3(0, 0, 0);
        movetimer = 0;
        timetoshoot = Random.Range(0.5f, 4.0f);
        shoottimer = 0;
        rb = GetComponent<Rigidbody>();
    }

    public void TakeDamage(GameObject g)
    {
        if (g == this.gameObject)
        {
            Debug.Log("Skull was hit");
        }
    }

    void ShootFireball()
    {
        GameObject fireball = Instantiate(fireballPrefab, transform.position + new Vector3(0, -0.75f, 0.0f), transform.rotation);
        Debug.Log("Shoot Fireball");
    }
    // Update is called once per frame
    void Update()
    {
        float wave = Mathf.Sin(Time.time) * 0.35f;
        Vector3 temppos = transform.position;
        

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
            rb.velocity = new Vector3(Random.Range(-20f, 10f), Random.Range(-3.8f, 15f), Random.Range(-20f, 20f));
        }

        Vector3 vectorfrombase = transform.position - basePos;
        rb.velocity -= Vector3.Normalize(vectorfrombase) * 16.0f * Time.deltaTime;

        shoottimer += Time.deltaTime;
        if (shoottimer > timetoshoot)
        {
            ShootFireball();
            shoottimer = 0;
            timetoshoot = Random.Range(0.5f, 4.0f);
        }
    }
    void FixedUpdate()
    {
        rb.velocity = Vector3.Scale(rb.velocity, new Vector3(0.95f, 0.95f, 0.95f)); 
    }
}

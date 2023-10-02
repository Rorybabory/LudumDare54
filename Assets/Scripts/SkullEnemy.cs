using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SkullEnemy : MonoBehaviour
{
    [SerializeField] private float turnSpeed;

    // Start is called before the first frame update
    private static Transform playerTransform;
    [SerializeField] private GameObject fireballPrefab;
    private Vector3 basePos;

    private float timetomove;
    private float movetimer;

    private float shoottimer;
    private float timetoshoot;

    private Vector3 offsetvelocity;
    private Vector3 offset;

    private Rigidbody rb;

    private Vector3 turnVel;

    void Start()
    {
        basePos = transform.position;
        timetomove = Random.Range(0f, 1f);
        offsetvelocity = new Vector3(0, 0, 0);
        movetimer = 0;
        timetoshoot = Random.Range(0.5f, 4.0f);
        shoottimer = 0;
        rb = GetComponent<Rigidbody>();
        if (playerTransform == null )
        {
            playerTransform = FindObjectOfType<PlayerMovement>().transform;
        }
    }

    public void TakeDamage(GameObject g)
    {
        if (g == this.gameObject)
        {
            Debug.Log("Skull was hit");
            rb.velocity -= g.transform.forward * 20.0f;
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
        
        Vector3 toPlayer = Quaternion.LookRotation(playerTransform.position - transform.position).eulerAngles;
        transform.eulerAngles = new Vector3(
            Mathf.SmoothDampAngle(transform.eulerAngles.x, toPlayer.x, ref turnVel.x, turnSpeed),
            Mathf.SmoothDampAngle(transform.eulerAngles.y, toPlayer.y, ref turnVel.y, turnSpeed),
            Mathf.SmoothDampAngle(transform.eulerAngles.z, toPlayer.z, ref turnVel.z, turnSpeed));

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

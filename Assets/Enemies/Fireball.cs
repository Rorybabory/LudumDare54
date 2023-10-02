using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    [SerializeField] private float speed;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = speed * transform.forward;
        Destroy(this.gameObject, 4);
    }

    void OnTriggerEnter(Collider other)
    {
        var player = other.GetComponentInParent<Damageable>();

        if (player != null && player.CompareTag("Player"))
        {
            player.TakeDamage(1);
            Destroy(this.gameObject);
        }
        
    }
}

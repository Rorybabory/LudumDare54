using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    [SerializeField] private float speed;
    private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = speed * transform.forward;
        Destroy(this.gameObject, 4);
    }

    // Update is called once per frame
    void Update()
    {
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Capsule")
        {
            Debug.Log("HIT PLAYER");
            GameObject player = other.gameObject.transform.parent.parent.gameObject;
            Damageable d = other.gameObject.GetComponentInParent<Damageable>();
            if (d != null)
            {
                d.TakeDamage(1);
            }
            else {
                Debug.LogError("Player does not have damageable component");
            }
        }
        Destroy(this.gameObject);
    }
}

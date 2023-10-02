using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeHitScanner : HitScan
{
    [SerializeField]
    private Transform origin;

    [SerializeField]
    private float radius = 3f;
    [Range(-1f, +1f)]
    [SerializeField]
    private float angle = 0.22f;
    [SerializeField]
    private LayerMask layerMask;
    [SerializeField]
    private QueryTriggerInteraction triggerInteraction = QueryTriggerInteraction.Ignore;

    public Transform Origin
    {
        get => this.origin;
        set => this.origin = value;
    }

    public float Radius
    {
        get => this.radius;
        set => this.radius = Mathf.Clamp(value, -1, +1);
    }

    public float Angle
    {
        get => this.angle;
        set => this.angle = value;
    }

    public LayerMask LayerMask
    {
        get => this.layerMask;
        set => this.layerMask = value;
    }
    public QueryTriggerInteraction TriggerInteraction
    {
        get => this.triggerInteraction;
        set => this.triggerInteraction = value;
    }

    private void Awake()
    {
        if (this.Origin == null)
        {
            this.Origin = this.transform;
        }
    }

    public override Collider[] ScanForColliders()
    {
        var colliders = new List<Collider>();
        var collidersInRadius =
            Physics.OverlapSphere(this.Origin.position, this.Radius, this.LayerMask, this.TriggerInteraction);

        foreach (var colliderInRadius in collidersInRadius)
        {
            var thisTransform = this.transform;
            var forward = thisTransform.forward;
            var directionToEnemy = (colliderInRadius.transform.position - thisTransform.position).normalized;
            var dot = Vector3.Dot(forward, directionToEnemy);

            if (dot > this.Angle)
            {
                colliders.Add(colliderInRadius);
            }
        }

        return colliders.ToArray();
    }
}

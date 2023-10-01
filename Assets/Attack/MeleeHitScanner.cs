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
    [SerializeField]
    private float angle = 80f;
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
        set => this.radius = value;
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

    public override Collider[] ScanForColliders()
    {
        var collidersInRadius =
            Physics.OverlapSphere(this.Origin.position, this.Radius, this.LayerMask, this.TriggerInteraction);
        
        return collidersInRadius;
    }
}

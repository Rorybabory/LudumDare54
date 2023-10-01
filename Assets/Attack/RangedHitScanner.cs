using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.CompilerServices;

public class RangedHitScanner : HitScan
{
    [SerializeField] Transform hitScanOrigin;
    public override Collider[] ScanForColliders()
    {
        Ray ray = new Ray(hitScanOrigin.position, hitScanOrigin.forward);
        RaycastHit hit;


        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            return new[] { hit.collider };
        }
        else
        {
            return new Collider[] { };
        }
        //throw new NotImplementedException();

    }

}

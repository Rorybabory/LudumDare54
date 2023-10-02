using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.CompilerServices;

public class RangedHitScanner : HitScan
{
    [SerializeField] Transform hitScanOrigin;
    [SerializeField] LayerMask hitScanLayerMask;
    private Camera cam;
    void Start()
    {
        cam = GetComponentInChildren<Camera>();
    }
    public override Collider[] ScanForColliders()
    {
        Ray ray = new Ray(cam.ViewportToWorldPoint(new Vector3(0.5f,0.5f,0.0f)), cam.transform.forward);
        RaycastHit hit;

      //  Debug.DrawRay(cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f)), cam.transform.forward);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, hitScanLayerMask))
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

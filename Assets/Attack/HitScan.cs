using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class HitScan : MonoBehaviour
{

    public abstract Collider[] ScanForColliders();
}

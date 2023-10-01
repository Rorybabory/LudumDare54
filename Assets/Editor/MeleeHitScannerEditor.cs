using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MeleeHitScanner))]
public class MeleeHitScannerEditor : Editor
{
    private void OnSceneGUI()
    {
        var scanner = (MeleeHitScanner)this.target;
        var baseColor = Color.yellow;
        var color = new Color()
        {
            r = baseColor.r,
            g = baseColor.g,
            b = baseColor.b,
            a = 0.5f,
        };

        var origin = scanner.Origin == null ? scanner.transform : scanner.Origin;
        
        Handles.color = color;
        Handles.DrawSolidArc(origin.position, origin.up, origin.forward, scanner.Angle, scanner.Radius);
    }
}

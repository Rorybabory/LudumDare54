using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUI : MonoBehaviour {

    [SerializeField] private Transform sizeBar;
    [SerializeField] private SizeTransformer sizeTransformer;

    private void Update() {

        sizeBar.localScale = new(sizeTransformer.Size, 1, 1);
    }
}

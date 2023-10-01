using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class SizeTransformer : MonoBehaviour
{
    [Range(0, 1)]
    [SerializeField]
    private float size = 0f;
    [SerializeField]
    private float increment = 0.05f;
    [SerializeField]
    private AnimationCurve curve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
    [SerializeField]
    private float maximumScale = 5f;

    public float Size
    {
        get => this.size;
        private set
        {
            this.size = Mathf.Clamp01(value);
            this.UpdateSize();
        }
    }
    public float MaximumScale
    {
        get => this.maximumScale;
        set => this.maximumScale = value;
    }

    public float Increment
    {
        get => this.increment;
        set => this.increment = value;
    }
    public AnimationCurve Curve
    {
        get => this.curve;
        set => this.curve = value;
    }

    public void IncreaseSize()
    {
        this.Size += this.Increment;
    }

    public void DecreaseSize()
    {
        this.Size -= this.Increment;
    }

    public void ResetSize()
    {
        this.Size = 0.5f;
    }

    private void OnValidate()
    {
        // This is just a hack so that the size updates when we change the value in the inspector while the game
        // is running.
        if (Application.isPlaying)
        {
            this.Size = this.size;
        }
    }

    private void UpdateSize()
    {
        // Calculate the percentage of the current size and evaluate against a normalized AnimationCurve.
        var percentage = this.Size / 1f;
        var evaluated = this.Curve.Evaluate(percentage);
        
        // Scale the player's height up based on the evaluated value of the maximum scale.
        this.transform.localScale = Vector3.one + (Vector3.up * this.MaximumScale * evaluated);
    }
}

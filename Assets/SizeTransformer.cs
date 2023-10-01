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
    [SerializeField]
    private float minimumScale = 1f;

    [SerializeField] private Transform anchoredToMiddle, anchoredToBottom, renderTextureCanvas;

    [SerializeField] private new Rigidbody rigidbody;

    public float Size
    {
        get => this.size;
        private set => this.UpdateSize(value);
    }

    // Calculate the percentage of the current size and evaluate against a normalized AnimationCurve.
    private float WorldSize(float size) => Mathf.Lerp(minimumScale, maximumScale, curve.Evaluate(size));

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

    private void Update()
    {
        if (Input.GetKey(KeyCode.I)) IncreaseSize();
        if (Input.GetKey(KeyCode.K)) DecreaseSize();
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

    private void UpdateSize(float newSize)
    {
        float worldSize = WorldSize(size),
              newWorldSize = WorldSize(newSize),
              worldDiff = newWorldSize - worldSize;

        size = Mathf.Clamp01(newSize);

        // transform scale
        Vector3 scale = transform.localScale;
        scale.y = newWorldSize;
        transform.localScale = scale;

        // position
        rigidbody.MovePosition(rigidbody.position + worldDiff / 2f * Vector3.up);

        // anchored posiition
        anchoredToBottom.localPosition = Vector3.down / 2f;

        // canvas scale
        Vector3 canvasScale = renderTextureCanvas.localScale;
        canvasScale.y = 1f / newWorldSize;
        renderTextureCanvas.localScale = canvasScale;
    }
}

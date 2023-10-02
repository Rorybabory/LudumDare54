using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using static UnityEngine.Rendering.DebugUI;

public class SizeTransformer : MonoBehaviour
{
    [Range(0, 1)]
    public static float size = 0f;
    [SerializeField]
    private static float increment = 0.05f;
    [SerializeField]
    private AnimationCurve curve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
    [SerializeField]
    private float maximumScale = 5f;
    [SerializeField]
    private float minimumScale = 1f;

    [SerializeField] private new Transform collider;
    [SerializeField] private Transform ranchoredToMiddle, anchoredToBottom;

    [SerializeField] private new Rigidbody rigidbody;

    public float Size
    {
        get => size;
        private set => this.UpdateSize(value);
    }

    // Calculate the percentage of the current size and evaluate against a normalized AnimationCurve.
    private float WorldSize(float size) => Mathf.Lerp(minimumScale, maximumScale, curve.Evaluate(size));

    public float Increment
    {
        get => increment;
        set => increment = value;
    }
    public AnimationCurve Curve
    {
        get => this.curve;
        set => this.curve = value;
    }

    private void Update()
    {
/*        if (Input.GetKey(KeyCode.I)) IncreaseSize();
        if (Input.GetKey(KeyCode.K)) DecreaseSize();
*/        this.UpdateSize(size);
    }

    public static void IncreaseSize()
    {
        size += increment;
        Debug.Log("Increase the Size");
    }

    public static void DecreaseSize()
    {
        size -= increment;

    }

    public void ResetSize()
    {
        size = 0.5f;
    }

    private void OnValidate()
    {
        // This is just a hack so that the size updates when we change the value in the inspector while the game
        // is running.
        if (Application.isPlaying)
        {
            this.Size = size;
        }
    }

    private void UpdateSize(float newSize)
    {
        if (SceneManager.GetActiveScene().name != "Gameplay")
        {
            return;
        }
        float worldSize = WorldSize(size),
              newWorldSize = WorldSize(newSize),
              worldDiff = newWorldSize - worldSize;
        if (size < 0)
        {
            SceneManager.LoadScene("Scenes/Game/Game Over", LoadSceneMode.Single);
            size = 0;
            return;
        }
        size = Mathf.Clamp01(newSize);

        // transform scale
        Vector3 scale = collider.localScale;
        scale.y = newWorldSize;
        collider.localScale = scale;

        // position
        rigidbody.MovePosition(rigidbody.position + worldDiff / 2f * Vector3.up);

        // anchored posiition
        anchoredToBottom.localPosition = Vector3.down * newWorldSize / 2f;
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using UnityEngine.Events;

public class timer : MonoBehaviour
{
    public float maxDuration = 5.0f;
    private float timerElapse = 0.0f;
    private bool isTimeTicking = false;
    [SerializeField]
    private UnityEvent elapsed;
    // testing
    // bool readyToPing;
    
    // Start is called before the first frame update
    void Start()
    {
        // testing
        startTimer();
        // readyToPing = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isTimeTicking)
        {
            timerElapse += 1.0f * Time.deltaTime;
            if (timerElapse > maxDuration)
            {
                elapsed?.Invoke();
                stopTimer();
                timerElapse = maxDuration;
            }
        }

        // testing
        // if (timerElapse > 10.0f && readyToPing)
        // {
        //     Debug.Log("10 second! Ping!");
        //     readyToPing = false;
        // }
    }

    public void startTimer()
    { isTimeTicking=true;}
    public void stopTimer()
    { isTimeTicking=false;}
    public void resetTimer()
    { timerElapse = 0.0f; }
}

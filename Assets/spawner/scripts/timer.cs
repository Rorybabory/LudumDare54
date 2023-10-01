using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class timer : MonoBehaviour
{
    float timerElapse = 0.0f;
    bool isTimeTicking = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isTimeTicking)
        {
            
        }
    }

    public void startTimer()
    { isTimeTicking=true;}
    public void stopTimer()
    { isTimeTicking=false;}
    public void resetTimer()
    { timerElapse = 0.0f; }
}

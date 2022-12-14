using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerScript : MonoBehaviour
{
    public float timeRemaining = 0f;
    public bool timerIsRunning = false;
    private float seconds;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        seconds = Mathf.FloorToInt(timeRemaining / 60);
        if (timerIsRunning)
        {
            GetComponent<TMPro.TextMeshProUGUI>().text = seconds.ToString();
            if (timeRemaining <= 0)
            {
                timeRemaining += Time.deltaTime;
            } else {
                timeRemaining = 0;
                timerIsRunning = false;
            }
        }
    }
}

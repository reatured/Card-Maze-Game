using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class WinningTrigger : MonoBehaviour
{
    public UnityEvent winningEvent; 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if(other.name == "Player")
        {
            print(other.name);
            winningEvent.Invoke(); 
        }
    }
}

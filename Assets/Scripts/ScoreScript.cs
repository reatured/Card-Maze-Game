using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreScript : MonoBehaviour
{
    public static int scoreValue = 0;
    public static bool counter = false;
    private Vector3 startPos;
    private Vector3 newPos;
    private static float posY;
    private static float posZ;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        posY = transform.position.y;
        posZ = transform.position.z;

        resetPos();
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<TMPro.TextMeshProUGUI>().text = scoreValue.ToString();

        if (counter == true)
        {
            scoreValue ++;
            counter = false;
        }

        if(scoreValue >= 10){
            newPos = new Vector3(17, 0, 0);
            transform.position = startPos + newPos;
        }
    }

    void resetPos(){
        transform.position = startPos;
    }

    public void resetScore(){
        scoreValue = 0;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Events;

public class GameUI : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < gameScriptsToBeEnabled.Length; i++)
        {
            gameScriptsToBeEnabled[i].SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (startingGameState) startingGame();
        if (winningState) onWinning(); 
    }

    private bool startingGameState = false;
    public UnityEvent startingGameEvent; 


    public void onStartGame()
    {
        startingGameEvent.Invoke(); 
        for (int i = 0; i < menuItems.Length; i++)
        {
            menuItems[i].SetActive(false);
        }

        for(int i = 0; i < gameScriptsToBeEnabled.Length; i++)
        {
            gameScriptsToBeEnabled[i].SetActive(true);
        }
        startingGameState = true;
    }

    public Volume menuEffectVolume;
    public float enterTime = 1f; 
    public void startingGame()
    {
        float weight = menuEffectVolume.weight - (enterTime * Time.deltaTime);

        if(weight <= 0f)
        {
            endStartingGame();
            return;
        }

        menuCam.transform.position = Vector3.Lerp(menuCam.transform.position, mainCam.transform.position, 0.05f);
        menuCam.transform.rotation = Quaternion.Lerp(menuCam.transform.rotation, mainCam.transform.rotation, 0.05f);

        menuEffectVolume.weight = weight; 
        
    }

    public GameObject[] menuItems;
    public GameObject[] gameScriptsToBeEnabled;
    public GameObject[] winMenuItems;
    public GameObject mainCam;
    public GameObject menuCam; 
    public void endStartingGame()
    {
        mainCam.SetActive(true);
        switchToGameCamera(true);

        startingGameState = false;
    }

    public void switchToGameCamera(bool gameCamState)
    {
        mainCam.SetActive(gameCamState);
        menuCam.SetActive(!gameCamState);
    }

    public bool winningState = false; 
    public void onWinningStart()
    {
        menuCam.transform.position = mainCam.transform.position; 
        for(int i = 0; i < winMenuItems.Length; i++)
        {
            winMenuItems[i].SetActive(true);
        }

        winningState = true;
    }

    public void onWinning()
    {
        float weight = menuEffectVolume.weight + (enterTime * Time.deltaTime);

        if (weight >= 1f)
        {
            endWinning(); 
            return;
        }

        menuEffectVolume.weight = weight;
    }

    public void endWinning()
    {
        switchToGameCamera(false);
        winningState = false;
    }
}

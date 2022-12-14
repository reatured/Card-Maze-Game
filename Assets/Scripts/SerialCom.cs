using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.IO.Ports;
using System.Threading;

public class SerialCom : MonoBehaviour
{
    public String portName = "/dev/cu.usbmodem101";  // use the port name for your Arduino, such as /dev/tty.usbmodem1411 for Mac or COM3 for PC
    public GameObject Player;
    public GameObject Score;
    private ScoreScript scoreScript;
    private PlayerMovement script;
    private int curIndex;


    private SerialPort serialPort = null; 
    private int baudRate = 115200;  // match your rate from your serial in Arduino
    private int readTimeOut = 100;

    private string serialInput;

    bool programActive = true;
    Thread thread;

    void Start()
    {
        try
        {
            serialPort = new SerialPort();
            serialPort.PortName = portName;
            serialPort.BaudRate = baudRate;
            serialPort.ReadTimeout = readTimeOut;
            serialPort.RtsEnable = true;
            serialPort.Open();
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
        thread = new Thread(new ThreadStart(ProcessData));  // serial events are now handled in a separate thread
        thread.Start();

        script = Player.GetComponent<PlayerMovement>();
        scoreScript = Score.GetComponent<ScoreScript>();
    }

    void ProcessData()
    {
        Debug.Log("Thread: Start");
        while (programActive)
        {
            try
            {
                serialInput = serialPort.ReadLine();
            }
            catch (TimeoutException)
            {

            }
        }
        Debug.Log("Thread: Stop");
    }

    void Update()
    {
        if (serialInput != null)
        {
            Debug.Log(serialInput);
            string[] strEul = serialInput.Split(';');  // parses using semicolon ; into a string array called strEul. I originally was sending Euler angles for gyroscopes
            
            if (strEul.Length == 1) // only uses the parsed data if every input expected has been received. In this case, 2 inputs consisting of a button (0 or 1) and an analog values between 0 and 1023
            {
                foreach (string part in strEul){
                    if(String.Equals(part, "move_for")){
                        print("true");
                    }
                    else {
                        break;
                    }
                }

                curIndex = int.Parse(strEul[0]);

                if (curIndex == 1) // if button pressed
                {
                    script.startWalk(new Vector3(0, 0, -1));
                }
                else if(curIndex == 2)
                {
                    script.startWalk(new Vector3(-1,0,0));
                } else if (curIndex == 3)
                {
                    script.startWalk(new Vector3(0, 0, 1));
                } else if (curIndex == 4)
                {
                    script.startWalk(new Vector3(1, 0, 0));
                } else if(curIndex == 5)
                    script.resetGame();
                    // scoreScript.resetScore();
                }
        }
    }

    public static float map(float value, float leftMin, float leftMax, float rightMin, float rightMax)
    {
        return rightMin + (value - leftMin) * (rightMax - rightMin) / (leftMax - leftMin);
    }

    public void OnDisable()  // attempts to closes serial port when the gameobject script is on goes away
    {
        programActive = false;
        if (serialPort != null && serialPort.IsOpen)
            serialPort.Close();
    }
}

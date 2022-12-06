using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.IO.Ports;
using System.Threading;

public class SimpleSerial : MonoBehaviour
{

    public String portName = "COM15";  // use the port name for your Arduino, such as /dev/tty.usbmodem1411 for Mac or COM3 for PC
    public Camera playerCamera;
    public GameObject disScore;
    public float speed = 10.0f;
    public float smooth = 2.0f;
    public bool moving = false;

    private SerialPort serialPort = null; 
    private int baudRate = 115200;  // match your rate from your serial in Arduino
    private int readTimeOut = 100;
    private Rigidbody rb;
    private bool move = false;

    private string serialInput;

    public bool RtsEnalbe = true;

    bool programActive = true;
    Text score;
    Thread thread;

    public static int PlayerScore1 = 0;

    void Start()
    {
        try
        {
            serialPort = new SerialPort();
            serialPort.PortName = portName;
            serialPort.BaudRate = baudRate;
            serialPort.ReadTimeout = readTimeOut;
            serialPort.RtsEnable =  true;
            serialPort.Open();
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
        thread = new Thread(new ThreadStart(ProcessData));  // serial events are now handled in a separate thread
        thread.Start();
        rb = playerCamera.GetComponent<Rigidbody>();
        disScore = GameObject.FindGameObjectWithTag("ScoreBoard");
        Transform textTr = disScore.transform.Find("Score");
        score = textTr.GetComponent<Text>();
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
            string[] strEul = serialInput.Split(';');  // parses using semicolon ; into a string array called strEul. I originally was sending Euler angles for gyroscopes
            int readout = int.Parse(strEul[0]);
            int readout2 = int.Parse(strEul[2]);
            float readout3 = float.Parse(strEul[3]);
            var vel = rb.velocity;
            Debug.Log("readout: " + readout);
            Debug.Log("readout2: " + readout2);
            if (readout3 > 0.0f){
                move = true;
            } else if(readout3 < 0.0f){
                move = true;
            } else {
                move = false;
            }
            Debug.Log("Moving: " + move);
            if (readout == 1 && move == true){
                vel.z = speed;
                moving = true;
                PlayerScore1++;
            } else if (readout2 == 1 && move == true) {
                vel.z = -speed;
                moving = true;
                PlayerScore1--;
            } else {
                vel.z = 0;
                moving = false;
            }
            // if (readout2 == 1){
            //     vel.z = -speed;
            // } else {
            //     vel.z = 0;
            // }
            rb.velocity = vel;
            
            float tiltAngle = float.Parse(strEul[1]);
            tiltAngle = Mathf.Clamp(tiltAngle, -1.0f, 1.0f);
            tiltAngle = map(tiltAngle, -1.0f, 1.0f, -5, 5);

            Quaternion cRot = Quaternion.Euler(0, 0, tiltAngle);
            playerCamera.transform.rotation = cRot; 
            playerCamera.transform.rotation = Quaternion.Slerp(transform.rotation, playerCamera.transform.rotation, Time.deltaTime * smooth);
            if (strEul.Length == 2) // only uses the parsed data if every input expected has been received. In this case, 2 inputs consisting of a button (0 or 1) and an analog values between 0 and 1023
            {
                // if (int.Parse(strEul[1]) == 0) // if button pressed
                // {
                   

                // }
                // else
                // {
                   

                // }
                // float readout = int.Parse(strEul[0]);
                // readout = Mathf.Clamp(readout, 200f, 824f);
                // readout = map(readout, 200f, 824f, -4f, 4f);
                //flip y
                // readout *= -1f;
                // Debug.Log(strEul[0]);

                // Vector3 cPos = playerCamera.transform.position;
                // cPos = new Vector3(cPos.x, cPos.y, readout);
                // playerCamera.transform.position = cPos;
            }
            score.text = PlayerScore1.ToString();
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
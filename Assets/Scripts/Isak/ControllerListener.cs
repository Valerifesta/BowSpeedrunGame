using UnityEngine;
using System.IO.Ports;
using System;

public class ControllerListener : MonoBehaviour
{
    SerialPort port;
    string connectedPortName;

    private void Awake()
    {

        connectedPortName = string.Join("\n", System.IO.Ports.SerialPort.GetPortNames());
        Debug.Log("Existing ports: " + connectedPortName);
        if (string.IsNullOrEmpty(connectedPortName))
        {
            Debug.Log("No port could be found! Please connect controller and restart session.");
        }
        else
        {
            port = new SerialPort(connectedPortName, 115200);
        }

    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //TryConnectPort("COM3", 9600);
        if (port != null)
        {
            if (port.IsOpen)
            {
                port.Close();
                Debug.Log("closerd");
                
            }
            port.Open();
            port.ReadTimeout = 5; //Will time out trying to read if didnt recieve anything after this many miliseconds
            Debug.Log("opened");
        }
        else
        {
            Debug.Log("Port is null, could not open it");
        }
        /*
        try
        {
            port.Open();
            print("opened");
        }
        catch
        {
            print("Could not open " + "COM3" + " at " + 9600 + " baud rate");
        }*/
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}

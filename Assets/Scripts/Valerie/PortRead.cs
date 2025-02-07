using UnityEngine;
using System.IO.Ports;
using System;
//using UnityEditor.Experimental.GraphView;
using System.Runtime.CompilerServices;
using TMPro;
using System.Linq;
using System.Data.Common;
using System.Net.Configuration;

public class PortRead : MonoBehaviour
{
    SerialPort port;
    string connectedPortName;
    public TextMeshProUGUI tmProComp;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

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
            port = new SerialPort(connectedPortName, 9600);
        }

    }
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
        
        if (port != null)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                TryCallbackPing();
            }

            TryPortRead();
        }
    }

    public void TryConnectPort(string comPort, int baudRate)
    {
        
    }

    public void TryCallbackPing()
    {
        port.WriteLine("A");
        //byte[] bytechar = System.Text.Encoding.UTF8.GetBytes("Reset");
        /*
        string[] stringByte = new string[bytechar.Length];

        for (int i = 0; i < bytechar.Length; i++)
        {
            string a = bytechar[i].ToString();
            Debug.Log(a);
            stringByte[i] = a;
        }
        string writeString = string.Join("", stringByte);
        Debug.Log(writeString);
        */

    }
    public void TryPortRead()
    {
        //bool hasRecieved = false;
        string read = string.Empty;


        try
        {
            read = port.ReadLine();
            tmProComp.text = read;
            //Debug.Log(read);
        }
        catch (TimeoutException a)
        {
            //Debug.Log("tmed out");
        }  
        /*
        while (!hasRecieved)
        {
            read = port.ReadLine();
            if (!string.IsNullOrEmpty(read))
            {
                hasRecieved = true;
            }
        }
        if (hasRecieved)
        {
            print(read);
            
        }*/
    }

    private void OnApplicationQuit()
    {
        if (port != null)
        {
            port.Close();
            Debug.Log("Closed Port: " + port.PortName);
        }
    }
}

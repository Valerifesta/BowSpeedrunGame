using UnityEngine;
using System.IO.Ports;
using System;
using System.Threading;
using System.Linq;

public class ControllerListener : MonoBehaviour
{
    SerialPort port;
    string connectedPortName = "/dev/ttyUSB0";

    private void Awake()
    {

        //connectedPortName = string.Join("\n", System.IO.Ports.SerialPort.GetPortNames());
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
            Thread backgroundThread = new Thread(new ThreadStart(ReadFromThread));
            backgroundThread.Start();
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

    void Update()
    {
        
    }
    void ReadFromThread(){
      while(true){
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
    public float ToFloat(Byte[] data){
      return BitConverter.ToSingle(data, 0);

    }
    public void TryPortRead()
    {
        //bool hasRecieved = false;
        Byte[] buff = new Byte[32];


        try
        {
          port.Read(buff, 0, 13);
          if(buff[0] == 1){
            Debug.Log(buff);
            Vector3 list = new Vector3(
            ToFloat(buff.Skip(1).Take(sizeof(float)).ToArray()),
            ToFloat(buff.Skip(1+sizeof(float)).Take(sizeof(float)).ToArray()), 
            ToFloat(buff.Skip(1+2*sizeof(float)).Take(sizeof(float)).ToArray()));
            Debug.Log(list);
          }
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

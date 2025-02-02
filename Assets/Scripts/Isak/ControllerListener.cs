using UnityEngine;
using System.IO.Ports;
using System.IO;
using System;
using System.Threading;
using System.Linq;

//class SerialWrap: Stream {
//  SerialPort port;
//
//  public SerialWrap(SerialPort port) {
//    this.port = port;
//  }
//  
//  public override int Read(byte[] buf, int off, int len) {
//    return this.port.Read(buf, off, len);
//  }
//  public override 
//}

public class ControllerListener : MonoBehaviour
{
    Byte[] starting_bytes = new Byte[16]{255, 254, 100, 50, 51, 101, 253, 252, 70, 80, 96, 75, 71,81, 97, 76}; 
    SerialPort port;
    bool calibrated = false;
    //SerialWrap stream;
    BinaryReader reader;
    string connectedPortName = "COM4";//"/dev/ttyUSB0";
    
    private TestBowBehaviour bowBehaviour;
    private CameraBehaviour camBehaviour;

    
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
        bowBehaviour = FindFirstObjectByType<TestBowBehaviour>();
        camBehaviour = FindFirstObjectByType<CameraBehaviour>();

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

    void WriteToArduino(Byte b){
      Byte[] buff = new Byte[1]{b};
      port.Write(buff, 0, 1);
      port.BaseStream.Flush();
    }
    void Update()
    {
      WriteToArduino(1);
      WriteToArduino(2);
      //WriteToArduino(3);
      //WriteToArduino(4);
    }
    void ReadFromThread(){
      reader = new BinaryReader(port.BaseStream);
      //while(!check_startup(0)){};
      Debug.Log("Starting to read!");
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
      return BitConverter.ToSingle(data.Take(4).ToArray(), 0);
    }

    public Byte[] ReadFromPort(BinaryReader p, int length){
      if(length > 32){
        Debug.Log("Buffer length too large :(");
        return new Byte[32];
      }
      Byte[] buff = new Byte[32];
      int offset = 0;
      while(offset < length){
        offset += p.Read(buff, offset, length - offset);
      }
      return buff;
    }
    public bool check_startup(int length){
      try{
        Byte checkbyte = reader.ReadByte();
        if(checkbyte == starting_bytes[length]){
          if(length == 15){
            return true;
          }
          else{
            return check_startup(length+1);
          }
        }
        else
        {
          return false;
        }

      }
      catch(TimeoutException e){ return check_startup(length); }
    }
    public void TryPortRead()
    {
        //bool hasRecieved = false;
        try
        {
          if(port.BytesToRead < 1 ){
            return;
          }
          byte type = reader.ReadByte();
          if(!calibrated){
            Debug.Log("Calibration Complete!");
            camBehaviour.ResetRot();
          }
          calibrated = true; 
          if(type == 1){
            float gx = reader.ReadSingle();
            float gy = reader.ReadSingle();
            float gz = reader.ReadSingle();
            Vector3 rotation = new Vector3(gx,gy,gz);
            if(rotation.magnitude > 0){
              //Debug.Log(rotation);
            }
            camBehaviour.UpdateCameraRot(rotation);
          }
          if(type == 2){
            float rps = reader.ReadSingle();
            if(rps != 0){
              bowBehaviour.UpdateRotaryValue(rps);
              Debug.Log(rps);
            }
            if(rps == -9999){
              Debug.Log("Same twice!");
            }
          }
          //if(type == 3){
          //  Debug.Log("Button 1 pressed!");
          //}
          //if(type == 4){
          //  Debug.Log("Button 2 pressed!");
          //  WriteToArduino(5);
          //}
          //if(type == 5){
          //  Debug.Log("Calibrating...");
          //  calibrated = false;
          //}
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

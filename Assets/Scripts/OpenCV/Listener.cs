/* 
* The purpose of this program is to provide a minimal example of using UDP to 
* receive data.
* It picks up broadcast packets and displays the text in a console window.
* This was created to work with the program UDP_Minimum_Talker.
* Run both programs, send data with Talker, receive the data with Listener.
* Run multiple copies of each on multiple computers, within the same LAN of course.
* If the broadcast packet contains numbers or binary data or anything other than 
* plain text it may well crash and burn. 
* Adding code to handle unexpected conditions such as that would defeat the 
* simplistic nature of this example program. So would adding code for a gracefull
* exit. Just kill it.
*/
using System;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using System.Collections;


public class Listener : MonoBehaviour {

    private const int listenPort = 11000;

    UdpClient listener;
    IPEndPoint groupEP;
    string received_data;
    byte[] receive_byte_array;
    bool RunThread = true;
    static Thread listen;
	int shot;
    int testInt = 0;

	public static int[] location = new int[8];
	public static bool[] shooting = new bool[2];
	public static float[] angle = new float[2];
	public static float[] force = new float[2];
    void OnEnable()
    {
        listen = new Thread(Listenthread);
        RunThread = true;
        listener = new UdpClient(listenPort);
        groupEP = new IPEndPoint(IPAddress.Any, listenPort);
        Debug.Log("Thread opens!");
        listen.Start();
    }

	void Update()
	{

	}

    void Listenthread()
    {
        while (RunThread)
        {
            /*
            It calls the receive function from the object listener (class UdpClient)
            It passes to listener the end point groupEP.
            It puts the data from the broadcast message into the byte array
            named received_byte_array.
            */
            receive_byte_array = listener.Receive(ref groupEP);
            received_data = Encoding.ASCII.GetString(receive_byte_array, 0, receive_byte_array.Length);
            #region Unit conversion
            //Save location from the data string
            for (int i = 0; i < 12; i += 3)
            {
                
                location[i / 3] = Convert.ToInt32(received_data.Substring(i, 3));
            }
            #region Notes
            /*
            int location[0] angle X
            int location[1] angle Y
            int location[2] power X
            int location[3] power Y
            bool shooting Shot-state
            */
            #endregion

            //Calculates angle and force and saves them in global variables.
            angle[0] = Vector2.Angle(new Vector2(-1, 0), new Vector2(location[2] - location[0], location[3] - location[1]));
            force[0] = Vector2.Distance(new Vector2(location[2], location[3]), new Vector2(location[0], location[1]));
            
            //Saving the shot state in global variable
            if (Convert.ToInt32(received_data.Substring(12,1)) == 1)
                shooting[0] = true;
            else
                shooting[0] = false;

            #endregion
            
        }
    }

    void OnDisable()
    {
        listener.Close();
        Debug.Log("Thread closes");
        RunThread = false;
    }
}

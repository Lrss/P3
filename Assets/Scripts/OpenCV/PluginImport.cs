using UnityEngine;
using System.Collections;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

public class PluginImport : MonoBehaviour {

	//Lets make our calls from the Plugin
    [DllImport("OpenCV")]
    public static extern int DLLArraySize();

    [DllImport("OpenCV")]
	public static extern void OpenCVData(IntPtr Array);

    public int[] location = new int[8];
	public bool[] shooting = new bool[2];
	public float[] angle = new float[2];
	public float[] force = new float[2];

    byte[] PluginData;
    int memorySize;

	void Start()
    {
        memorySize = DLLArraySize();
        //System.Diagnostics.Process.Start("Calibration.exe");
    }

	void Update () 
    {
        // Create a managed array. 
        byte[] managedArray = new byte[memorySize];

        // Initialize unmanaged memory to hold the array. 
        int size = Marshal.SizeOf(managedArray[0]) * managedArray.Length;

        IntPtr pnt = Marshal.AllocHGlobal(size);

        try
        {
            // Copy the array to unmanaged memory so the plugin can access it.
            Marshal.Copy(managedArray, 0, pnt, managedArray.Length);
            //UnityEngine.Debug.Log("The array was copied to unmanaged memory");
            //run plugin
            OpenCVData(pnt);

            //Create managed array to use in unity
            PluginData = new byte[managedArray.Length];

            // Copy the unmanaged array back to another managed array. 
            Marshal.Copy(pnt, PluginData, 0, managedArray.Length);
        }

        finally
        {
            Marshal.FreeHGlobal(pnt);//Free the unmanaged memory.
        }

        string DataString = "";

        for (int i = 0; i < managedArray.Length; i++)
            DataString += Convert.ToChar(PluginData[i]);
        UnityEngine.Debug.Log(DataString);

        //for (int i = 0; i < 24; i += 3)
        //  location[i / 3] = Convert.ToInt32(DataString.Substring(i, 3));
        /* 
            location[0] Left player X
            location[1] Left player Y
            location[2] Left power X
            location[3] Left power Y
            location[4] Right player X
            location[5] Right player Y
            location[6] Right power X
            location[7] Right power Y
            location[8] Is arrow shot
        */
        /*
        angle[0] = Vector2.Angle(new Vector2(-1,0), new Vector2(location[2] - location[0], location[3] - location[1]));
        angle[1] = Vector2.Angle(new Vector2(1, 0), new Vector2(location[6]-location[4], location[7] - location[5]));
        force[0] = Vector2.Distance(new Vector2(location[2], location[3]), new Vector2(location[0], location[1]));
        force[1] = Vector2.Distance(new Vector2(location[6], location[7]), new Vector2(location[4], location[5]));

        //for (int i = 0; i < 2; i++)

            UnityEngine.Debug.Log("Angle is: " + angle[0] + ". Force is: " + force[0]);
            */
        /*
        //Saving the shot state
        int shot = Convert.ToInt32(DataString.Substring(24, 1));
        switch (shot)
        {
            case 0:
                shooting[0] = false;
                shooting[1] = false;
                break;
            case 1:
                shooting[0] = true;
                shooting[1] = false;
                break;
            case 2:
                shooting[0] = false;
                shooting[1] = true;
                break;
            case 3:
                shooting[0] = true;
                shooting[1] = true;
                break;
        } */
    }
}
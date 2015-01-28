using UnityEngine;
using System.Collections;

public class ApplyTexture : MonoBehaviour {
	/*
    int width = 0;
    int height = 0;

    public GameObject PluginHolder;
    PluginImport OpenCV;


	// Use this for initialization
	void Start () 
    {
        PluginHolder = GameObject.FindWithTag("MainCamera");
        
        OpenCV = PluginHolder.GetComponent<PluginImport>();
        OpenCV.CanvasSize(out width, out height);
	}
	
	// Update is called once per frame
	void Update () 
    {

        Texture2D texture = new Texture2D(width, height);
        renderer.material.mainTexture = texture;

        for (int y = 0; y < texture.height; y++ )
            for (int x = 0; x < texture.width; x++)
            {
                //Color color = Color.black; //only for testing
                
                Color color = OpenCV.color(x, y);
                texture.SetPixel(x, y, color);
            }
        
        texture.Apply();
	
        /*int y = 0;
        while (y < texture.height / 2)
        {
            int x = 0;
            while (x < texture.width)
            {
                Color color = Color.black;
                texture.SetPixel(x, y, color);
                ++x;
            }
            ++y;
        }
        texture.Apply();*//*
	}*/
}

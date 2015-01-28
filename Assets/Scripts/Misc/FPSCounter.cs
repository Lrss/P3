using UnityEngine;
using System.Collections;

public class FPSCounter : MonoBehaviour
{

    float frameCount;
    bool showFrameCount;

    void OnPreCull()
    {
        camera.ResetWorldToCameraMatrix();
        camera.ResetProjectionMatrix();
        camera.projectionMatrix = camera.projectionMatrix * Matrix4x4.Scale(new Vector3(-1 ,1, 1));
    }

    void OnPreRender()
    {
        GL.SetRevertBackfacing(true);
    }

    void OnPostRender()
    {
        GL.SetRevertBackfacing(false);
    }

    // Use this for initialization
    void Start()
    {
        showFrameCount = false;
    }

    // Update is called once per frame
    void Update()
    {
        frameCount = 1 / Time.deltaTime;
        if(Input.GetKeyDown("."))
        {
            showFrameCount = !showFrameCount;
        }
    }

    void OnGUI()
    {
        if(showFrameCount)
            GUI.Label(new Rect(10, 40, 100, 20), frameCount.ToString());
    }
}

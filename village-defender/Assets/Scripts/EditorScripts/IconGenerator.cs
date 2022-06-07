using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class IconGenerator : MonoBehaviour
{
    public string prefix;
    public string PathFolder;

    private Camera camera;

    private void Awake()
    {
        Screenshot();
    }
    public void Screenshot()
    {
        string folderPart = @"Assets/";
        TakeScreenshot(folderPart + prefix);
    }

    void TakeScreenshot(string fullPath)
    {
        if(camera == null)
        {
            camera = GetComponent<Camera>();
        }

        RenderTexture rt = new RenderTexture(256, 256, 24);
        camera.targetTexture = rt;
        Texture2D screenshot = new Texture2D(256, 256, TextureFormat.RGBA32, false);
        camera.Render();
        RenderTexture.active = rt;
        screenshot.ReadPixels(new Rect(0, 0, 256, 256), 0, 0);
        camera.targetTexture = null;
        RenderTexture.active = null;

        if(Application.isEditor)
        {
            DestroyImmediate(rt);
        }
        else
        {
            Destroy(rt);
        }
        byte[] bytes = screenshot.EncodeToPNG();
        System.IO.File.WriteAllBytes(fullPath, bytes);
    }
}

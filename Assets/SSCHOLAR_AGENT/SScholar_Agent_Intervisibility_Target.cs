using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SScholar_Agent_Intervisibility_Target : MonoBehaviour {
    public bool visible = false;

    public int resWidth = 64;
    public int resHeight = 64;

    private bool hasRenderTexture = false;
    private Texture2D texture;


    // Use this for initialization
    void Start() {


        resWidth = 64;
        resHeight = 64;
        Texture2D texture = new Texture2D(resWidth, resHeight);
        GetComponent<Renderer>().material.mainTexture = texture;
        //Debug.Log("texture set for rendering");
      
    }
	
	// Update is called once per frame
	void Update () {
        /*
        for (int i = 0; i < 31; i++)
        {
            texture.SetPixel(i, 10, Color.gray);
        }
        texture.Apply();
        */
    }

    void IsHit()
    {
        Debug.Log("Agent has a visible connection to me");
    }

    void ChangeColor()
    {

    }

    public void PaintHit()
    {

    }

    public void DrawTexture(float u, float v)
    {

        //GetComponent<Renderer>().material.mainTexture = texture;
        //Debug.Log("DrawTexture called");
        //Debug.Log("u " + u + " " + v);
        //Debug.Log("resWidth " + resWidth);
        float tempx = u * resWidth;
        float tempy = v * resHeight;

        int x = (int)tempx;
        int y = (int)tempy;
        Debug.Log(x + " " + y);
        if(texture != null)
        {
            texture.SetPixel(x, y, Color.gray);
            texture.Apply();
            Debug.Log("Texture Redrawn");
        }
        else
        {
            Debug.Log("Texture is null");
        }
    }

    void BuildRenderTexture()
    {
        if (hasRenderTexture == false)
        {

            hasRenderTexture = true;
            //build and assign a texture for rendering into
            /*
            RenderTexture rt = new RenderTexture(resWidth, resHeight, 24);
            cam.targetTexture = rt;
            Texture2D screenShot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
            cam.Render();
            RenderTexture.active = rt;
            screenShot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
            cam.targetTexture = null;
            RenderTexture.active = null; // JC: added to avoid errors
            Destroy(rt);
            byte[] bytes = screenShot.EncodeToPNG();
            string filename = ScreenShotName(resWidth, resHeight);
            System.IO.File.WriteAllBytes(filename, bytes);
            Debug.Log(string.Format("Took screenshot to: {0}", filename));
            takeHiResShot = false;
            */
        }
    }

}

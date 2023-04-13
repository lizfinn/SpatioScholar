using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PIV_CamTest : MonoBehaviour
{
    public Camera PIV_Cam;
    public Material PIV_Material;
    public RenderTexture PIV_RenderTexture;
    private Texture2D PIV_Texture;
    public GameObject target;


    public static Color SecondColour = new Color(1f, 0f, 0f, 1f);

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("PIV_CamTest Start() called");
        AssignTarget();
        CameraFocus();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("PIV_CamTest Update() called");
        CameraFocus();
        Debug.Log("PIV_Test result = " + PIV_Test());
    }
    

    void CameraFocus()
    {
        //Vector3 pointOnside = target.transform.position + new Vector3(target.transform.localScale.x * 0.5f, 0.0f, target.transform.localScale.z * 0.5f);
        //float aspect = (float)Screen.width / (float)Screen.height;
        //float maxDistance = (target.transform.localScale.y * 0.5f) / Mathf.Tan(Mathf.Deg2Rad * (PIV_Cam.fieldOfView / aspect));
        //PIV_Cam.transform.position = Vector3.MoveTowards(pointOnside, target.transform.position, -maxDistance);


        //direct camera each frame to look at the target....but this might need to change in the future with large objects. Will need to look at the visible portion?
        //for instance, how to focus on the large wall at Samos? Focusing on the center of the object will not work well?
        PIV_Cam.transform.LookAt(target.transform.position);

    }

    void AssignTarget()
    {
        //find "PIV_Target";
        //Assign that object into the target object

        target = GameObject.Find("PIV_Target");
    }

    void CreateTexture()
    {
        PIV_Texture = new Texture2D(PIV_RenderTexture.width, PIV_RenderTexture.height, TextureFormat.RGB24, false);

        Rect rectReadPicture = new Rect(0, 0, PIV_RenderTexture.width, PIV_RenderTexture.height);

        //sets the render texture active
        RenderTexture.active = PIV_RenderTexture;

        // Read pixels
        PIV_Texture.ReadPixels(rectReadPicture, 0, 0);
        PIV_Texture.Apply();

        //RenderTexture.active = null; // added to avoid errors 
    }

    float PIV_Test()
    {
        float return_value = 0;
        CreateTexture();
        //Store current material of target
        Renderer temp_renderer = target.GetComponent<Renderer>();
        Material storage = temp_renderer.material;
        //Change material of target to PIV_Material
        temp_renderer.material = PIV_Material;


        //Render 1 frame from PIV camera to PIV texture

        //restore orginal material to target
        temp_renderer.material = storage;
        //run compare method on the texture
        return (PIV_Compare());
    }

    float PIV_Compare()
    {
        float return_value = 0;
        int width = PIV_Texture.width;
        int height = PIV_Texture.height;
        float count = 0;

        //Debug.Log("pixel comparison");
        for (int h = 0; h < height; h++)
        {
            for(int w = 0; w < width; w++)
            {
                //if(PIV_Texture.GetPixel(w, h) == Color.red)
                if (PIV_Texture.GetPixel(w, h) == SecondColour)
                {
                    count++;
                }
                else
                {

                }
            }
        }
        
        //check for percentage of full red pixels in the render and return percentage
        return_value = count / (width * height);
        //return_value = count;

        //Debug.Log(return_value);
        return (return_value);
    }
}

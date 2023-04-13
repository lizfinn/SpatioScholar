using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour {
    //public SScholar_Agent_Controller controller;
    //public Camera cam;
    public bool AtoATesting = false;
    public NavMeshAgent agent;
    public bool vector_render = false;
    public bool attributes_render = false;
    public bool debug_rays = false;
    public bool attributes_visible = false;
    public float sky_exposure;
    public float temp_sky_exposure;
    private float hit;
    public bool intervisibility = false;
    //public GameObject home;
    public GameObject target;
    public Mesh mesh;
    private GameObject referenceObject;
    private PlayerController referenceScript;
    //put Agent Controller Reference here - Really should push this to be a singleton so it is easier to call.
    public SScholar_Agent_Controller controller_reference;

    public Renderer rend2;
    public Bounds bounds;
    public Vector3 bound_center;
    public Vector3 bound_extents;
    public float x_div;
    public float y_div;
    public float z_div;
    public float x_start;
    public float y_start;
    public float z_start;
    public int samplesubdiv = 3;
    public Color AgentColor;
    public List<AgentToAgentTestResults> AResults;

    public bool dropmarker = false;

    public string Type;
    public string Block;
    public string Ward;
    public string ID;
    public string Sex;
    public string State;
    public int Total_Number;
    public Color Agent_Color;
    string HomeObjectName = "Home_ObjectName";
    public GameObject HomeObject;
    public int clock_hour = 0;
    public float speed = 1f;
    public Dictionary<string, string> Itinerary = new Dictionary<string, string>();
    bool isDone, section2, section3, section4, noMove, noMove1, sleep8am, isDone1, section21, section31, section41;
    //need to define a dictionary to store inter agent to agent test numbers and time stamp
    public int sleepcall = 0;
    //for color change to code agents
    Material m_Material;

    // for changing patients based on time -- probably inefficient, but as colliders aren't working going with this rn ... finn
    public SScholar_Agent_Clock time;

    public Animator anim;

    // Use this for initialization
    void Start () {
        //print("PlayerController initialization");
        temp_sky_exposure = 0;
        debug_rays = true;
        samplesubdiv = 6;
        isDone = false; section2 = false; section3 = false; section4 = false; noMove = false; noMove1 = false; sleep8am = false ; isDone1 = false; section21 = false; section31 = false; section41 = false;
        //populate the agent controller reference object
        controller_reference = GameObject.Find("SScholar_Agent_Controller").GetComponent<SScholar_Agent_Controller>();



        /// animation
        /// 

        anim = GetComponent<Animator>();
        




        //run the test once for each player controller
        //RunTests();
        // populate the time??
        time = GameObject.Find("SScholar_Agent_Clock").GetComponent<SScholar_Agent_Clock>();

        /*
        target = GameObject.Find("Intervisibility Target");
        if (target != null)
        {
            Debug.Log("intervisibility target(s) found!");
            //Renderer rend = target.GetComponent<Renderer>();
            //Renderer rend2 = target.GetComponent<Renderer>();
            //Collider m_Collider = target.GetComponent<Collider>();
            Renderer rend = target.GetComponent<Renderer>();
            bounds = rend.bounds;
            bound_center = bounds.center;
            //Debug.Log(bound_center);
            bound_extents = bounds.extents;
            //Debug.Log(bound_extents);
        }
        else
        {
            Debug.Log("no intervisibility target(s) found!");
        }
        */


        /*
        //set color
        Component[] renderers;

        renderers = GetComponentsInChildren(typeof(Renderer));

        if (renderers != null)
        {
            //Debug.Log("found agent sub object renderer objects, going to change color now")
            foreach(Renderer r in renderers)
            {
                r.material.color = Color.yellow;
            }
        }
        else
        {
            //do nothing
        }
        */




    }

    // Update is called once per frame
    void Update () {
        //placed here to run the agent sight test over and over
        //RunTests();

        //All of this needs to get put into it's own methods, I think it is being called over and over for each agent
        
        // Bit shift the index of the layer (8) to get a bit mask
        int layerMask = 1 << 8;

        // This would cast rays only against colliders in layer 8.
        // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
        layerMask = ~layerMask;

        if(intervisibility == true) {
            //call the intervisibility Raycast method
            Debug.Log("intervisibility toggle = true, calling the bounding raycast method");
            Intervisibility_Raycast();
            //Intervisibility_Bounding_Raycast(samplesubdiv);
        }

        if(dropmarker == true)
        {
            Debug.Log("Dropping visibility marker");
            //drop object to mark successful hit
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            //cube.AddComponent<Rigidbody>();
            cube.transform.position = gameObject.transform.position;
            Renderer rend = cube.GetComponent<Renderer>();
            rend.material.shader = Shader.Find("_Color");
            rend.material.SetColor("_Color", Color.green);
        }
        
        /*  put into a separate method for calling specific sky exposure raycasts
        if (debug_rays = true) {
            
        //Test RayTrace Code
        int raycount = 20;
        int raynumber = raycount;
        //Vector3 RayDirection = new Vector3(1, 0, 1);
        //Code below randomizes the initial ray orientation to create a monte carlo (?) random sampling
        Vector3 RayDirection = new Vector3(Random.Range(0f,1f), Random.Range(0f, .25f), 0);
        sky_exposure = 0;
        Vector3 NewRayCastLocation = (transform.position + (new Vector3(0.0f, 2.0f, 0.0f)));
            for (int i = 0; i < raynumber; i++)
            {
                //visualize the ray being cast in the interface
                //Debug.DrawRay(NewRayCastLocation, RayDirection * 100, Color.green);

                //if (Physics.Raycast(transform.position, RayDirection, 300))
                if (Physics.Raycast(NewRayCastLocation, RayDirection, 300))
                {
                    print("Ray Hit!");
                    temp_sky_exposure = temp_sky_exposure + (1f / raycount);
                    Debug.DrawRay(NewRayCastLocation, RayDirection * 50, Color.white);
                    print("sky_exposure variable = " + temp_sky_exposure);
                }
                else
                {
                    print("Ray not Hit!");
                }
                //update sky_exposure
                sky_exposure = temp_sky_exposure;
                //rotate Ray Direction Vector3
                RayDirection = Quaternion.Euler(0, (360 / raynumber), 0) * RayDirection;
            }
        }
        */

        //Test for Vector Flag to turn on vector line renderer
        if (vector_render == true) {

            //invoke the LineRenderer to render the direction vector for this agent
            LineRenderer lineRenderer = this.GetComponent<LineRenderer>();
            lineRenderer.SetVertexCount(2);
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, transform.forward * 3 + transform.position);
        }

        //make sure atributes visible is turned off
        if (attributes_render == false)
        {
            if (attributes_visible == true)
            {
                attributes_visible = false;
            }
        }

        //Test for Attribute Render Flag to turn on Canvas
        if (attributes_render == true)
        {
            if (attributes_visible == false){
                attributes_visible = true;
            }

            //make the canvas visible
            //Canvas myCanvas = this.GetComponent<LineRenderer>();

        }


        if ((time.return_hour() <= 7 && !(time.return_hour() == 7 && time.return_minute() >= 45)))
        {
            
            sleeping();

        }
        
        if(Type == "Patient4")
        {
            transform.rotation = Quaternion.Euler(new Vector3(270f, 180f, 0));
            transform.position = transform.position + new Vector3(0f, 0.55f, 0f);
            if (!noMove)
            {
                transform.position = transform.position + new Vector3(0f, 0f, -1f);
            }
            noMove = true;
        }
        if (Type == "Patient4.2")
        {
            transform.rotation = Quaternion.Euler(new Vector3(270f, 0f, 0));
            transform.position = transform.position + new Vector3(0f, 0.55f, 0f);
            if (!noMove1)
            {
                transform.position = transform.position + new Vector3(0f, 0f, 1f);
            }
            noMove1 = true;
        }
        
        

    }

    public void sleeping()
    {
        if (Type == "Patient1" || Type == "Patient3")
        {

            transform.rotation = Quaternion.Euler(new Vector3(270f, 90f, 0));
            //if(!isDone)
            {
                transform.position = transform.position + new Vector3(0f, 0.55f, 0f);
                if (!isDone)
                {
                    transform.position = transform.position + new Vector3(1f, 0f, 0f);
                }

            }
            
            isDone = true;
            
          

        }
        if (Type == "Patient2" || Type == "Patient1.3")
        {
            transform.rotation = Quaternion.Euler(new Vector3(270f, 0f, 0));
            transform.position = transform.position + new Vector3(0f, 0.55f, 0f);
            if (!section2)
            {
                transform.position = transform.position + new Vector3(0f, 0f, 1f);
            }
            section2 = true;
        }
        if (Type == "Patient1.2" || Type == "Patient3.2")
        {
            transform.rotation = Quaternion.Euler(new Vector3(270f, 270f, 0));
            transform.position = transform.position + new Vector3(0f, 0.55f, 0f);
            if (!section3)
            {
                transform.position = transform.position + new Vector3(-1f, 0f, 0f);
            }
            section3 = true;
        }
        if (Type == "Patient2.2" || Type == "Patient3.3")
        {
            transform.rotation = Quaternion.Euler(new Vector3(270f, 180f, 0));
            transform.position = transform.position + new Vector3(0f, 0.55f, 0f);
            if (!section4)
            {
                transform.position = transform.position + new Vector3(0f, 0f, -1f);
            }
            section4 = true;
        }
    }

   
    public void ToggleVector()
    {
        //debug
        print("Setting Vector Render False");
        vector_render = false;
    }

    public void ToggleIntervisibility()
    {
        //debug
        print("Agent Instance Toggle Intervisibility method called");
        if (intervisibility == false)
        {
            Debug.Log("setting intervisibility == true");
            intervisibility = true;
        }
        else
        {
            /*
            if (intervisibility == true)
            {
                Debug.Log("setting intervisibility == false");
                intervisibility = false;
            }
            else
            {

            }
            */
        }
    }

    public void ToggleDebugRays()
    {
        if (debug_rays == false)
        {
            debug_rays = true;
        }
        else
        {
            debug_rays = false;
        }
    }

    public void Intervisibility_Bounding_Raycast(int x)
    {
        print("Intervisibility Bounding Raycast Called");
        dropmarker = false;
        x_div = (bound_extents.x * 2) / x;
        y_div = (bound_extents.y * 2) / x;
        z_div = (bound_extents.z * 2) / x;
        x_start = bound_center.x - bound_extents.x;
        y_start = bound_center.y - bound_extents.y;
        z_start = bound_center.z - bound_extents.z;

        for (int i = 0; i < (x); i++)
        {
            //x
            y_start = bound_center.y - bound_extents.y;
            for (int j = 0; j < (x); j++)
            {
                //y
                z_start = bound_center.z - bound_extents.z;
                for (int k = 0; k < (x); k++)
                {
                    //z
                    //Vector3 RayDirection = target.transform.position - transform.position;
                    Vector3 test_target = new Vector3(x_start, y_start, z_start);
                    //Debug.Log(test_target);
                    Vector3 RayDirection = test_target - transform.position;
                    Vector3 NewRayCastLocation = (transform.position + (new Vector3(0.0f, 2.0f, 0.0f)));

                    // Bit shift the index of the layer (8) to get a bit mask
                    int layerMask = 1 << 8;
                    // This would cast rays only against colliders in layer 8.
                    // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
                    layerMask = ~layerMask;
                    RaycastHit hit;

                    //if (Physics.Raycast(NewRayCastLocation, RayDirection, 300) && )
                    if (Physics.Raycast(NewRayCastLocation, RayDirection, out hit, Mathf.Infinity, layerMask))
                    {
                        //moved this earlier in an effort to have the red rays overwrite the white
                        Debug.DrawRay(NewRayCastLocation, RayDirection * 50, Color.gray);
                        if (hit.collider.gameObject.name == "Intervisibility Target")
                        {
                            print("Ray Hit!");

                            dropmarker = true;
                            

                            if (debug_rays == true)
                            {
                                Debug.DrawRay(NewRayCastLocation, RayDirection * 50, Color.red); 
                            }
                            //Fetch the Material from the Renderer of the GameObject
                            m_Material = hit.collider.GetComponent<Renderer>().material;
                            m_Material.color = Color.red;
                            //hit.collider.gameObject().material.color = Color.red;

                            //Debug.Log(hit.textureCoord);
                            //Vector2 pixelUV = hit.textureCoord;
                            //SScholar_Agent_Intervisibility_Target target = hit.collider.gameObject.GetComponent<SScholar_Agent_Intervisibility_Target>();
                            //target.DrawTexture(pixelUV.x, pixelUV.y);

                            //hit.collider.gameObject.GetComponent<SScholar_Agent_Intervisibility_Target>().
                        }
                    }
                    else
                    {
                        //print("Ray not Hit!");
                    }
                    //z
                    z_start = z_start + z_div;
                }
                //y
                y_start = y_start + y_div;
            }
            //x
            x_start = x_start + x_div;
        }
    }
    public void Find_Intervisibility_Object()
    {
        print("Intervisibility Raycast Called");

        GameObject target = GameObject.Find("Intervisibility Target");

        Vector3 RayDirection = target.transform.position - transform.position;
        Vector3 NewRayCastLocation = (transform.position + (new Vector3(0.0f, 2.0f, 0.0f)));
    }
    public void Intervisibility_Raycast()
    {
        //print("Intervisibility Raycast Called");

        GameObject target = GameObject.Find("Intervisibility Target");
        
        Vector3 RayDirection = target.transform.position - transform.position;
        Vector3 NewRayCastLocation = (transform.position + (new Vector3(0.0f, 2.0f, 0.0f)));

        // Bit shift the index of the layer (8) to get a bit mask
        int layerMask = 1 << 8;
        // This would cast rays only against colliders in layer 8.
        // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
        layerMask = ~layerMask;
        RaycastHit hit;

        //if (Physics.Raycast(NewRayCastLocation, RayDirection, 300) && )
        if (Physics.Raycast(NewRayCastLocation, RayDirection, out hit, Mathf.Infinity, layerMask))
        {
            print("Ray Hit!");
            if(hit.collider.gameObject.name == "Intervisibility Target")
            {
                if(controller_reference.visibility_marker == true)
                {
                    //drop a marker
                    Vector3 AddMarkerLocation = gameObject.transform.position;
                    Quaternion AddMarkerRotation = new Quaternion(0.0f, 0.0f, 0.0f, 0.0f);
                    //GameObject marker = Instantiate(Visibility_Marker, AddMarkerLocation, AddMarkerRotation);
                    GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    cube.AddComponent<Rigidbody>();
                    cube.transform.position = gameObject.transform.position;


                }
                if (debug_rays == true)
                {
                    Debug.DrawRay(NewRayCastLocation, RayDirection * 50, Color.red);
                }
                hit.collider.gameObject.GetComponent<Renderer>().material.color = Color.red;
            }

            Debug.DrawRay(NewRayCastLocation, RayDirection * 50, Color.white);
        }
        else
        {
            print("Ray not Hit!");
        }
    }
    public void RunTests()
    {
        //how to figure out which tests are active and run them? Through the JSON? Through the UI?
        //run every minute?
        //test debug to see if this works
        Debug.Log("Run Tests Method Called");
        if(AtoATesting == true)
        {
            RunAgentVisibilityTest();
        }
    }
    public void RunAgentVisibilityTest()
    {
        //need to set up a location to store all the data in, for each agent the distance, type, ward, block, etc.
        //for each agent in the master list
        try
        {
            //for each Agent in the master list......
            for (int i = 0; i < controller_reference.AgentList.Count; i++)
            {
                //hold the value of the agent location
                Vector3 tempvector = controller_reference.AgentList[i].transform.position;
                //Debug.Log(controller_reference.AgentList[i].transform.position);

                Vector3 RayDirection = tempvector - transform.position - (new Vector3(0.0f, 1.0f, 0.0f));
                Vector3 NewRayCastLocation = (transform.position + (new Vector3(0.0f, 2.0f, 0.0f)));

                // Bit shift the index of the layer (8) to get a bit mask
                int layerMask = 1 << 8;
                // This would cast rays only against colliders in layer 8.
                // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
                layerMask = ~layerMask;
                RaycastHit hit;

                if (Physics.Raycast(NewRayCastLocation, RayDirection, out hit, Mathf.Infinity, layerMask))
                {
                    //test Ray hit object for Agent Name?
                    if (hit.collider.gameObject.name == "Agent_Mesh_Capsule")
                    {
                        print("Ray Hit! "+ hit.collider.gameObject.name);
                        //store clock time, MyObjectName, HitAgentName, HitAgentType
                        PlayerController hitPC = hit.collider.gameObject.GetComponentInParent<PlayerController>();
                        //Debug.Log(hitPC.Type);

                        //build the agent test results into an object and store them in the List
                        AgentToAgentTestResults TempResults = new AgentToAgentTestResults(controller_reference.clock.hour.ToString()+ controller_reference.clock.minute.ToString(), hitPC.Type, hitPC.Block, hitPC.Ward, hitPC.ID, hitPC.Sex, hitPC.State);
                        AResults.Add(TempResults);

                        //maybe build a csv file by raw structure.....
                        Debug.DrawRay(NewRayCastLocation, RayDirection * 1, Color.red);
                    }
                }
                else
                {
                    //print("Ray Not Hit");
                }
            }
        }
        catch (System.Exception e)
        {
            print("error");
        }
        //make a ray test to that agent and return the information specifics of that agent

        //MUST STORE THE RESULTS IN THE AGENT FOR LATER DOWNLOAD INTO A CSV FILE!

    }
    public int FindAgentsInView(int CullingDistance)
    {
        int return_value = 0;
        /*
         * For each agent in the simulation{
         * Send one raycast from this agent to the target agent, if the raycast is successful and length is lesser than the CullingDistance then increment the return value
         * }
         */
        return (return_value);
    }

    public void Sky_Exposure()
    {
        //Test RayTrace Code
        int raycount = 20;
        int raynumber = raycount;
        //Vector3 RayDirection = new Vector3(1, 0, 1);
        //Code below randomizes the initial ray orientation to create a monte carlo (?) random sampling
        Vector3 RayDirection = new Vector3(Random.Range(0f, 1f), Random.Range(0f, .25f), 0);
        sky_exposure = 0;
        Vector3 NewRayCastLocation = (transform.position + (new Vector3(0.0f, 2.0f, 0.0f)));
        for (int i = 0; i < raynumber; i++)
        {
            //visualize the ray being cast in the interface
            //Debug.DrawRay(NewRayCastLocation, RayDirection * 100, Color.green);

            //if (Physics.Raycast(transform.position, RayDirection, 300))
            if (Physics.Raycast(NewRayCastLocation, RayDirection, 300))
            {
                print("Ray Hit!");
                temp_sky_exposure = temp_sky_exposure + (1f / raycount);
                Debug.DrawRay(NewRayCastLocation, RayDirection * 50, Color.white);
                print("sky_exposure variable = " + temp_sky_exposure);
            }
            else
            {
                print("Ray not Hit!");
            }
            //update sky_exposure
            sky_exposure = temp_sky_exposure;
            //rotate Ray Direction Vector3
            RayDirection = Quaternion.Euler(0, (360 / raynumber), 0) * RayDirection;
        }
    }


    /*
    //deprecated
    public void Sky_Exposure()
    {
        //run a test to see what percentage of sky is hit
        int raynumber = 10;
        int RaysHit = 0;
        Vector3 NewRayCastLocation = (transform.position + (new Vector3(0.0f, 2.0f, 2.0f)));
        for (int i = 0; i < raynumber; i++)
        {
            Debug.DrawRay(NewRayCastLocation, new Vector3(1, 1, 1) * 50, Color.white);
            RaycastHit objectHit;
            // Shoot raycast
            if (Physics.Raycast(NewRayCastLocation, new Vector3(1,1,1) , out objectHit, 50))
            {
                //Debug.DrawRay(transform.position, fwd * 50, Color.green);
                RaysHit++;
            }
        }

    }
    */
   


        //why is this called SetHour??
    public void SetHour(int a, string b)
    {
        //Debug.Log("SetHour method called within the PlayerController");

        Vector3 newtargetposition = new Vector3(0,0,0);
        NavMeshAgent tempNav = gameObject.GetComponent<NavMeshAgent>();
        //Debug.Log("Agent SetHour called with " + a + "  " + b);
        //Does the clock_hour actually get used?
        clock_hour = a;
        //if my itinerary has a place to go at this hour then execute a change in target
        if (Itinerary.ContainsKey(b))
        {
            string TT = Itinerary[b];
            //Debug.Log("Key found in Itinerary Dictionary   Value  = " + TT);
            if(Itinerary[b] == "Home")
            {
                //Debug.Log("Going Home");
                //go home
                newtargetposition = gameObject.GetComponent<PlayerController>().HomeObject.transform.position;
                controller_reference.clock.timescaler = 2000;
                this.Start();
                
            }
            else
            {
                GameObject TT1 = GameObject.Find(TT);
                //Debug.Log("Setting Target to    " + TT1);
                if (TT1.GetComponent<SScholar_Agent_Target_Logic>() != null)
                {
                    //query target for whatever type of location the target knows to deliver, for instance a random value within it.
                    newtargetposition = TT1.GetComponent<SScholar_Agent_Target_Logic>().GetTargetLocation();
                    //setting these higher in an attempt to slow down the clock
                    controller_reference.clock.timescaler = 2000;
                    anim.SetTrigger("nowalk");
                    this.Start();
                    anim.SetTrigger("nowalk");
                    

                }
                else
                {
                    //Debug.Log("Going to " + TT1);
                    newtargetposition = TT1.transform.position;
                    //Debug.Log("Setting navigation to " + newtargetposition);
                    //make a call to start the navigation movement of agent?

                    //setting these higher in an attempt to slow down the clock
                    controller_reference.clock.timescaler = 2000;
                    anim.SetTrigger("nowalk");
                    this.Start();
                    anim.SetTrigger("nowalk");
                }

                //Debug.Log("new target Vector3 position = " + newtargetposition);
            }
            
            
            tempNav.SetDestination(newtargetposition);
            //Debug.Log("Setting navigation to " + newtargetposition);
            tempNav.speed = 2;
            //Debug.Log("Current NavMeshAgent destination Vector3 = " + tempNav.destination);
            
        }
    }

    public void SetColor(Color c)
    {
        //Debug.Log("Agent SetColor method called");
        //set color
        Component[] renderers;

        renderers = GetComponentsInChildren(typeof(Renderer));

        if (renderers != null)
        {
            //Debug.Log("found agent sub object renderer objects, going to change color now")
            foreach (Renderer r in renderers)
            {
                r.material.color = c;
            }
        }
        else
        {
            //do nothing
        }
    }

    //This is a master agent initialization script to set all starting variables and object pointers
    public void Init_Agent_From_JSON(AgentInit a)
    {
        //set camera? why
        //set 
        Ward = a.Ward;
        Block = a.Block;
        Sex = a.Sex;
        Type = a.Type;
        State = a.State;
        ID = a.ID;


        //Debug.Log("Instance of new Agent Init method called");
        if (a.Color == "Red")
        {
            //Debug.Log("Agent color definition in JSON registers as Red");
            SetColor(Color.red);
        }
        else
        {
            if (a.Color == "Yellow")
            {
                //Debug.Log("Agent color definition in JSON registers as Yellow");
                SetColor(Color.yellow);
            }
            else
            {

                if (a.Color == "White")
                {
                    //Debug.Log("Agent color definition in JSON registers as White");
                    SetColor(Color.white);
                }
                else
                {
                    if(a.Color == "Green")
                    {
                        SetColor(Color.green);
                    }
                    else if (a.Color == "Gray")
                    {
                        SetColor(Color.gray);
                    }
                    else if(a.Color == "Brown")
                    {
                        Color brown = new Color(0.5f,0.25f,0);
                        SetColor(brown);
                    }
                    else if (a.Color == "Forest")
                    {
                        Color forest = new Color(0.102f, 0.263f, 0.078f);
                        SetColor(forest);
                    }
                    else if (a.Color == "Light Red")
                    {
                        Color lred = new Color(0.89f, 0.32f, 0.32f);
                        SetColor(lred);
                    }
                    else if (a.Color == "Dark Purple")
                    {
                        Color dpurple = new Color(0.37f, 0.18f, 0.47f);
                        SetColor(dpurple);
                    }
                    else if (a.Color == "Light Orange")
                    {
                        Color lorange = new Color(0.93f, 0.57f, 0.13f);
                        SetColor(lorange);
                    }
                    else if (a.Color == "Dark Blue")
                    {
                        Color dblue = new Color(0f, 0f, 0.55f);
                        SetColor(dblue);
                    }
                    else if (a.Color == "Light Green")
                    {
                        Color lgreen = new Color(0.80f, 1.0f, 0.80f);
                        SetColor(lgreen);
                    }
                    else
                    {
                        //Debug.Log("Agent color definition in JSON does NOT register as Red");
                        SetColor(Color.blue);
                    }
                    
                }
            }
        }




    }
}

//each positive Agent To Agent Raycast hits stores the results in one of these objects.
//These should be stored in a single list that can be ripped into a single line of a CSV
[System.Serializable]
public class AgentToAgentTestResults
{
    //clock time of positive test
    public string Time;
    public string Type;
    public string Block;
    public string Ward;
    public string ID;
    public string Sex;
    public string State;

    //definition method called to make each object
    public AgentToAgentTestResults(string t, string ty, string bl, string wa, string id, string se, string st)
    {
        Time = t;
        Type = ty;
        Block = bl;
        Ward = wa;
        ID = id;
        Sex = se;
        State = st;
    }

    public override string ToString()
    {
        string returnvalue = "This is an internal ToString override";
        //returnvalue = Type;
        return returnvalue;
    }
}

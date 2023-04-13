
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;


public class SScholar_Agent_Controller : MonoBehaviour
{
    //Make sure to attach these Buttons in the Inspector
    public Button m_Target1Button, m_Target2Button, m_AddAgentButton, m_ToggleVectorButton, m_CSV_SaveButton, m_DebugRaysButton, m_ClockFastForward;

    public Button m_StartSim, m_PauseSim;
    public Toggle IntervisibilityToggle, m_VisibilityMarker;
    public Toggle AgentAttributesDisplayToggle;
    public Toggle AgentToAgentTestToggle;
    public GameObject controller;
    public GameObject target1;
    public GameObject target2;
    public GameObject spawnpoint;
    public NavMeshAgent SSAgent;
    public Canvas SS_Agent_Canvas;
    public bool simulation_active = false;
    public SScholar_Agent_Clock clock;
    public Text clock_display;
    public bool visibility_marker;
    public int AgentToAgentTestFrequency = 10;

    //for use changing variables
    GameObject referenceObject;
    PlayerController referenceScript;

    //Trying to make the agent list specific to NavMeshAgents so we can call specific NavMeshAgent methods from them.
    public List<NavMeshAgent> AgentList = new List<NavMeshAgent>();

    //more generic List containing only GameObjects rather than more specific NavMeshAgents
    //public List<GameObject> AgentList = new List<GameObject>();

    //List of GameObjects used for intervisibility testing. Realistically, this will often be more than one object
    public List<GameObject> IntervisibilityTargets = new List<GameObject>();

    void Start()
    {
        m_Target1Button.onClick.AddListener(delegate { UpdateTarget1(); });
        m_Target2Button.onClick.AddListener(delegate { UpdateTarget2(); });
        m_AddAgentButton.onClick.AddListener(delegate { AddAgent(); });
        m_ToggleVectorButton.onClick.AddListener(delegate { ToggleVector(); });
        m_PauseSim.onClick.AddListener(delegate { PauseSim(); });
        m_StartSim.onClick.AddListener(delegate { StartSim(); });
        m_ClockFastForward.onClick.AddListener(delegate { Clock_Set_FastForward(); });
        //m_VisibilityMarker.onValueChanged.AddListener(delegate { Set_Visibility_Marker(); });

        IntervisibilityToggle.onValueChanged.AddListener(delegate { ToggleIntervisibility(); });
        AgentAttributesDisplayToggle.onValueChanged.AddListener(delegate { ToggleAgentAttributeVisibility(); });
        AgentToAgentTestToggle.onValueChanged.AddListener(delegate { ToggleAgentToAgentTesting(); });

        m_CSV_SaveButton.onClick.AddListener(delegate {
            //GetComponent<CSV_output>().Save();
            Debug.Log("button called");
            SaveCSV();
        });
        m_DebugRaysButton.onClick.AddListener(delegate { ToggleDebugRays(); });

        //set/populate controller and target objects
        controller = GameObject.Find("SScholar_Agent_Controller");
        target1 = GameObject.Find("Target 1");
        target2 = GameObject.Find("Target 2");

        List<NavMeshAgent> AgentList = new List<NavMeshAgent>();
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            print("(Tab) key was pressed, Toggle Main UI");
            ToggleUI();
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            print("(A) key was pressed, Adding new Agent");
            AddAgent();
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            print("(LeftShift) key was pressed, Click in scene to Add an Agent");
            AddAgent();
        }

        if (simulation_active == true)
        {
            Increment_Time();
        }
    }

    void TaskOnClick()
    {
        //Output this to console when the Button is clicked
        Debug.Log("You have clicked the button!");
    }

    void ToggleUI()
    {
        print("ToggleUI method called");
        if (SS_Agent_Canvas.enabled)
        {
            SS_Agent_Canvas.enabled = false;
        }
        else
        {
            SS_Agent_Canvas.enabled = true;
        }

    }

    void SaveCSV()
    {
        Debug.Log("Agent Controller SaveCSV method called");
        GetComponent<CSV_output>().Save();
    }

    void TaskWithParameters(string message)
    {
        //Output this to console when the Button is clicked
        Debug.Log(message);
    }

    void UpdateTarget1()
    {
        try
        {
            for (int i = 0; i < AgentList.Count; i++)
            {
                AgentList[i].SetDestination(target1.transform.position);
            }
        }
        catch (Exception e)
        {
            print("error");
        }
    }
    //RunAgentTests is an abstract method 
    public void RunAgentTests()
    {
        PauseAgents();
        //cycle through each agent instructing that any active tests be run
        //for each agent run method RunTests()
        for (int i = 0; i < AgentList.Count; i++)
        {
            //fire off the generic RunTests method on every agent
            AgentList[i].GetComponent<PlayerController>().RunTests();
        }
        ResumeAgents();
    }
    public void PauseAgents()
    {
        //Debug.Log("Pause Agents");
        for (int i = 0; i < AgentList.Count; i++)
        {
            AgentList[i].Stop();
        }
    }
    public void ResumeAgents()
    {
        //Debug.Log("Resume Agents");
        for (int i = 0; i < AgentList.Count; i++)
        {
            AgentList[i].Resume();
        }
    }

    void ToggleDebugRays()
    {
        try
        {
            for (int i = 0; i < AgentList.Count; i++)
            {
                AgentList[i].GetComponent<PlayerController>().ToggleDebugRays();
            }
        }
        catch (Exception e)
        {
            print("error");
        }
    }

    //AddAgent is called once per agent to add them as an instance into the simulation
    public void AddAgent()
    {
        //Default Start Location for new agents - this ough to be controlled through a more sophisticated method!
        //Vector3 AddAgentLocation = new Vector3(1.0f, 1.0f, 1.0f);

        //samos spawn location
        //Vector3 AddAgentLocation = new Vector3(-350.0f, 16.0f, -288.0f);

        //SSA Spawn Locator 
        Vector3 AddAgentLocation = spawnpoint.transform.position;

        Quaternion AddAgentRotation = new Quaternion(0.0f, 0.0f, 0.0f, 0.0f);
        NavMeshAgent newAgent = (NavMeshAgent)Instantiate(SSAgent, AddAgentLocation, AddAgentRotation);
        newAgent.GetComponent<PlayerController>().controller_reference = gameObject.GetComponent<SScholar_Agent_Controller>();
        AgentList.Add(newAgent);
        for (int i = 0; i < AgentList.Count; i++)
        {
            print("AgentList List object count number " + i);
            print(AgentList[i]);
        }
    }

    //This is called by the JSON parser iteratively to make each new agent, it is handed a Dictionary built from the JSON importer
    public void Initialize_Agent(AgentInit a, int i)
    {
        //used to push out the AgentInit data
        //Debug.Log(a);
        int useless = i;

        //finds the single home object outlined in the Agent Definition
        GameObject home_obj;
        //Makes a new Vector3 location position to instantiate new agent into
        Vector3 AddAgentLocation;
        //Not sure what this is?
        GameObject Home;
        try
        {
            home_obj = GameObject.Find(a.HomeObject);
            //test if the Home Object has the coordinator script assigned.
            if (home_obj.GetComponent<SScholar_Agent_HomeObjectCoordinator>())
            {
                //This block tries to search for sub objects under home to assign to the actual singular "home" variable in the specific agent
                try
                {
                    //if the number of agents in the JSON file does not match the number of unique home objects in the array default to the first object in the array for all extra agents
                    if (home_obj.GetComponent<SScholar_Agent_HomeObjectCoordinator>().Object_Array[i] == null)
                    {
                        AddAgentLocation = home_obj.GetComponent<SScholar_Agent_HomeObjectCoordinator>().Object_Array[1].transform.position;
                        Home = home_obj.GetComponent<SScholar_Agent_HomeObjectCoordinator>().Object_Array[1];

                        //initialize a rotation for each agent, no reason yet to make this specific to the agent so it is 0,0,0,0
                        Quaternion AddAgentRotation = new Quaternion(0.0f, 0.0f, 0.0f, 0.0f);
                        
                        
                        //Instantiates a single agent
                        NavMeshAgent newAgent = Instantiate(SSAgent, AddAgentLocation, AddAgentRotation);
                        PlayerController PC = newAgent.GetComponent<PlayerController>();
                        PC.Itinerary = a.Itinerary;
                        PC.HomeObject = Home;
                        //what should become a global init that transfers AgentInit values to the Agent Instance
                        //Need to call a single master new agent initialization script - Might need logic or variables to hold the objects this will use to populat
                        PC.Init_Agent_From_JSON(a);

                        AgentList.Add(newAgent);
                    }
                    else
                    {
                        AddAgentLocation = home_obj.GetComponent<SScholar_Agent_HomeObjectCoordinator>().Object_Array[i].transform.position;
                        Home = home_obj.GetComponent<SScholar_Agent_HomeObjectCoordinator>().Object_Array[i];

                        Quaternion AddAgentRotation = new Quaternion(0.0f, 0.0f, 0.0f, 0.0f);
                        
                        NavMeshAgent newAgent = Instantiate(SSAgent, AddAgentLocation, AddAgentRotation);
                        PlayerController PC = newAgent.GetComponent<PlayerController>();
                        PC.Itinerary = a.Itinerary;
                        PC.HomeObject = Home;
                        //what should become a global init that transfers AgentInit values to the Agent Instance
                        PC.Init_Agent_From_JSON(a);
                        AgentList.Add(newAgent);
                    }
                }
                catch (Exception e)
                {
                    print("error");
                }
            }
            else //This happens if there is no sub object coordinator
            {
                Debug.Log("This should be called once in the test scene");
                AddAgentLocation = home_obj.transform.position;
                Home = home_obj;

                Quaternion AddAgentRotation = new Quaternion(0.0f, 0.0f, 0.0f, 0.0f);
                
                NavMeshAgent newAgent = Instantiate(SSAgent, AddAgentLocation, AddAgentRotation);
                PlayerController PC = newAgent.GetComponent<PlayerController>();
                PC.Itinerary = a.Itinerary;
                PC.HomeObject = Home;
                Debug.Log("set the home object of the PlayerController = " + PC.HomeObject);
                //what should become a global init that transfers AgentInit values to the Agent Instance
                PC.Init_Agent_From_JSON(a);
                AgentList.Add(newAgent);
            }
        }
        catch(NullReferenceException e)
        {
            print("error");
        }
        



    }

    void UpdateTarget2()
    {

        try
        {
            for (int i = 0; i < AgentList.Count; i++)
            {
                AgentList[i].SetDestination(target2.transform.position);
                AgentList[i].speed = 1;

            }
        }
        catch (Exception e)
        {
            print("error");
        }

    }

    //Method to return IntervisibilityTargets
    public List<GameObject> GetIntervisibilityTargets()
    {
        return IntervisibilityTargets;
    }

    //function to add a GameObject to the Controller Intervisibility Target List
    //This should function should be called from each instance of the Intervisibility Target Scripts Start() method.
    public void AddIntervisibilityTarget(GameObject target)
    {
        IntervisibilityTargets.Add(target);
        Debug.Log("Intervisibility target " + target.name + " added");
    }

    public void ToggleIntervisibility()
    {
        Debug.Log("intervisibility toggle clicked");
        try
        {
            for (int i = 0; i < AgentList.Count; i++)
            {
                NavMeshAgent t = AgentList[i];
                referenceObject = t.gameObject;
                referenceScript = referenceObject.GetComponent<PlayerController>();

                Debug.Log("calling intervisibilty for 1 agent instance");
                referenceScript.ToggleIntervisibility();
            }
        }
        catch (Exception e)
        {
            print("error");
        }
    }

    void ToggleAgentAttributeVisibility()
    {
        try
        {
            for (int i = 0; i < AgentList.Count; i++)
            {
                NavMeshAgent t = AgentList[i];
                referenceObject = t.gameObject;
                //Canvas referenceCanvas = referenceObject.GetComponent<Canvas>();
                //referenceCanvas.SetActive(false);
                t.GetComponent<Canvas>().enabled = false;
            }
        }
        catch (Exception e)
        {
            print("error");
        }
    }

    void ToggleAgentToAgentTesting()
    {
        try
        {
            for (int i = 0; i < AgentList.Count; i++)
            {
                //Do Something
            }
        }
        catch (Exception e)
        {
            print("error");
        }
    }

    void ToggleVector()
    {

        try
        {
            for (int i = 0; i < AgentList.Count; i++)
            {
                NavMeshAgent t = AgentList[i];
                referenceObject = t.gameObject;
                referenceScript = referenceObject.GetComponent<PlayerController>();
                referenceScript.ToggleVector();
            }
        }
        catch (Exception e)
        {
            print("error");
        }
    }
    void StartSim()
    {
        simulation_active = true;
        //print("StartSim method called");
        try
        {
            for (int i = 0; i < AgentList.Count; i++)
            {
                NavMeshAgent t = AgentList[i];
                t.isStopped = false;
            }
            clock.hour = 6;
            clock.minute = 0;
        }
        catch (Exception e)
        {
            print("error");
        }
    }
    void PauseSim()
    {
        simulation_active = false;
        print("PauseSim method called");

        try
        {
            for (int i = 0; i < AgentList.Count; i++)
            {
                NavMeshAgent t = AgentList[i];
                t.isStopped = true;
            }
        }
        catch (Exception e)
        {
            print("error");
        }
    }
    void Increment_Time()
    {
        clock.increment_time();
        //display time
        clock_display.text = clock.return_time();
        //alert agents of time

        //Debug.Log("Calling the clock method navigation reset. Found that the AgentList.Count = "+ AgentList.Count);

        try
        {
            //Debug.Log("trying to set the navigation target");
            for (int i = 0; i < AgentList.Count; i++)
            {
                //Debug.Log("successful navigation reset call by clock object for object "+i);
                NavMeshAgent t = AgentList[i];
                //Debug.Log("found agent " + t);

                //referenceScript = referenceObject.GetComponent<PlayerController>();
                referenceScript = t.GetComponent<PlayerController>();
                //Debug.Log("Found reference script " + referenceScript);
                //Debug.Log(clock.return_itinerary_time());
                referenceScript.SetHour(clock.hour, clock.return_itinerary_time());
            }
        }
        catch (Exception e)
        {
            //Debug.Log("Exception = " + e);
            print("error setting agents internal clock");
        }
    }
    void GetAgentInfo(int location)
    {
        //For the supplied location value in the Agent index, return the information of the agent
        //AgentList[location];
    }
    Vector3 GetAgentLocation(int index)
    {
        Vector3 return_value = new Vector3(0f, 0f, 0f);
        return_value = AgentList[index].transform.position;
        return (return_value);
    }
    //hooked up to the UI, speeds up the clock so it moves quickly towards the next itinerary item
    public void Clock_Set_FastForward()
    {
        Debug.Log("clock fast forward method called");
        clock.timescaler = 5;
        /*
         * try
        {
            for (int i = 0; i < AgentList.Count; i++)
            {
                NavMeshAgent t = AgentList[i];
                t.speed = 300;
            }
        }
        catch (Exception e)
        {
            print("error");
        }
        */
    }
    void Set_Visibility_Marker()
    {
        if (visibility_marker == true)
        {
            visibility_marker = false;
        }
        if (visibility_marker == false)
        {
            visibility_marker = true;
        }
    }
}

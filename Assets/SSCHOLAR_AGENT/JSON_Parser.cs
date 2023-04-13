using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System.IO;                                                        // The System.IO namespace contains functions related to loading and saving files
using MARECEK;
using UltimateJson;

//This class only loads JSON files from a special folder
//It does not yet load individual files with a requester, which could be a nice feature
//The acutal instantiation of agents takes place in the agent controller
public class JSON_Parser : MonoBehaviour
{

    //array to hold all the JSON object filenames
    private string[] AgentInitJsonFilenames;
    //subfolder is the specific folder that is searched within for JSON files, all are loaded one at a time
    private string subfolder = "Sscholar_Agent_inits/";
    private AgentInit loadedData;
    public SScholar_Agent_Controller Controller;


public void LoadFiles()
    {

        DirectoryInfo dir = new DirectoryInfo("Assets/Sscholar_Agent_inits/");
        FileInfo[] info = dir.GetFiles("*.json");

        AgentInitJsonFilenames = new string[info.Length];

        for (int i = 0; i < info.Length; i++)
        {
            AgentInitJsonFilenames[i] = info[i].ToString();
            LoadGameData(info[i].ToString());
            RunInit();
        }
    }

    //This is run once for each file (JSON) file found
    void RunInit()
    {
        //This method is called for each agent within the Agent Definition File
        for (int i = 0; i < loadedData.Total_Number; i++)
        {
            //this passes the integer number along with the call, this could be useful in determining which child object to use as the true home object
            Controller.Initialize_Agent(loadedData, i);
        }
        //make sure the AgentInit variable is empty
        loadedData = null;
    }

    //This method takes as input a string pointing to the JSON Agent Init file and makes the agents
    private void LoadGameData(string filepath_string)
    {
        //make sure the AgentInit variable is empty
        loadedData = null;
        // Path.Combine combines strings into a file path
        // Application.StreamingAssets points to Assets/StreamingAssets in the Editor, and the StreamingAssets folder in a build
        string filePath = Path.Combine(Application.dataPath, subfolder);
        filePath = Path.Combine(filePath, filepath_string);

        if (File.Exists(filePath))
        {
            // Read the json from the file into a string
            //Debug.Log("Looking for an Agent Init file in " + filePath);
            string dataAsJson = File.ReadAllText(filePath);
            //Debug.Log("loadedData as deserialized string direct from imported file text = " + dataAsJson);

            /*
            StreamReader reader = new StreamReader(filePath);
            //Debug.Log("StreamReader");
            Debug.Log("StreamReader" + reader.ReadToEnd());
            reader.Close();
            */


            //UltimateJSON library - in an attempt to deserialize a dictionary in the Json, meaning unstructured data
            loadedData = UltimateJson.JsonObject.Deserialise<AgentInit>(dataAsJson);

            //Debug.Log("Json cast as object into a ToString function   = " + loadedData);
            //loadedData.ReturnDictionary();
        }
        else
        {
            Debug.LogError("Cannot load json AgentInit object!    for filename  =  " + filepath_string);
        }
    }

}

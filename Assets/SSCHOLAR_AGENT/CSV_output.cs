using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System;
using UnityEngine.AI;

public class CSV_output : MonoBehaviour
{
    public SScholar_Agent_Controller controller;
    private List<string[]> rowData = new List<string[]>();


    // Use this for initialization
    void Start()
    {
        controller = GetComponent<SScholar_Agent_Controller>();
    }

    public void SaveAgentToAgentTest()
    {
        Debug.Log("SaveAgentToAgentTest called");
        //for each agent
        //write a unique CSV file that dumps the contents of several dictionaries.......
        int r = controller.AgentList.Count;
        for (int i = 0; i < r; i++)
        {
            //initialize the unique CSV file for this agent
            //not sure any specific initialization is needed prior to gathering the data
            Debug.Log("testing agent " + i);
            PlayerController tempagent = controller.AgentList[i].GetComponent<PlayerController>();
            if (tempagent.AtoATesting == true)
            {
                Debug.Log("gathering data for agent " + i);
                //For each single Agent to Agent test result
                for (int j = 0; j < tempagent.AResults.Count; j++)
                {
                    //Add this test result to one line in the CSV file for this Agent's results
                    string[] rowDataTemp = new string[7];
                    rowDataTemp[0] = tempagent.AResults[j].Time;
                    rowDataTemp[1] = tempagent.AResults[j].Block;
                    rowDataTemp[2] = tempagent.AResults[j].Ward;
                    rowDataTemp[3] = tempagent.AResults[j].Sex;
                    rowDataTemp[4] = tempagent.AResults[j].Type;
                    rowDataTemp[5] = tempagent.AResults[j].State;
                    rowDataTemp[6] = tempagent.AResults[j].ID;
                    //add the rest of the data stored in the temp object
                    rowData.Add(rowDataTemp);
                }

                //invoke specific CSV file writing functions

                string[][] output = new string[rowData.Count][];

                for (int k = 0; k < output.Length; k++)
                {
                    output[k] = rowData[k];
                }

                int length = output.GetLength(0);
                string delimiter = ",";

                StringBuilder sb = new StringBuilder();

                for (int index = 0; index < length; index++)
                    sb.AppendLine(string.Join(delimiter, output[index]));


                string filePath = getPath(i);
                Debug.Log(filePath);

                StreamWriter outStream = System.IO.File.CreateText(filePath);
                outStream.WriteLine(sb);
                outStream.Close();
            }


        }
    }

    public void Save()
    {
        print("CSV Save Method Called");
        // Creating First row of titles manually. Had to break out Vector3 components so the CSV file was 
        // easier to process with separate X, Y, Z fields.
        string[] rowDataTemp = new string[12];
        rowDataTemp[0] = "ID_Number";
        rowDataTemp[1] = "Birth_Location_X";
        rowDataTemp[2] = "Birth_Location_Y";
        rowDataTemp[3] = "Birth_Location_Z";
        rowDataTemp[4] = "Current_Location_X";
        rowDataTemp[5] = "Current_Location_Y";
        rowDataTemp[6] = "Current_Location_Z";
        rowDataTemp[7] = "Current_Vector_X";
        rowDataTemp[8] = "Current_Vector_Y";
        rowDataTemp[9] = "Current_Vector_Z";
        rowDataTemp[10] = "Target";
        rowDataTemp[11] = "Sky Exposure";
        rowData.Add(rowDataTemp);


        /* SScholar_Agent_Controller calls to retrieve variables and call methods in other components added to the same object
        GetComponent(Gravity).SetGravityRange(1000);
        GetComponent<SScholar_Agent_Controller>().AgentList;
        */

        // You can add up the values in as many cells as you want.
        for (int i = 0; i < GetComponent<SScholar_Agent_Controller>().AgentList.Count; i++)
        //for (int i = 1; i < 3; i++)
        {
            //debug
            print("CSV Save data method : finding agent number " + i);
            //debug object being queried
            print("CSV Save data method : finding information from agent " + GetComponent<SScholar_Agent_Controller>().AgentList[i]);
            rowDataTemp = new string[12];
            //rowDataTemp[0] = "Sushanta" + i; // name
            rowDataTemp[0] = "" + i; // ID_Number
            rowDataTemp[1] = GetComponent<SScholar_Agent_Controller>().AgentList[i].transform.position.x.ToString(); // Birth_Location_X
            rowDataTemp[2] = GetComponent<SScholar_Agent_Controller>().AgentList[i].transform.position.y.ToString(); // Birth_Location_Y
            rowDataTemp[3] = GetComponent<SScholar_Agent_Controller>().AgentList[i].transform.position.z.ToString(); // Birth_Location_Z
            rowDataTemp[4] = GetComponent<SScholar_Agent_Controller>().AgentList[i].transform.position.x.ToString(); // Current_Location_X
            rowDataTemp[5] = GetComponent<SScholar_Agent_Controller>().AgentList[i].transform.position.y.ToString(); // Current_Location_Y
            rowDataTemp[6] = GetComponent<SScholar_Agent_Controller>().AgentList[i].transform.position.z.ToString(); // Current_Location_Z
            rowDataTemp[7] = GetComponent<SScholar_Agent_Controller>().AgentList[i].velocity.x.ToString(); // Current_Vector_X
            rowDataTemp[8] = GetComponent<SScholar_Agent_Controller>().AgentList[i].velocity.y.ToString(); // Current_Vector_Y
            rowDataTemp[9] = GetComponent<SScholar_Agent_Controller>().AgentList[i].velocity.z.ToString(); // Current_Vector_Z
            //rowDataTemp[10] = GetComponent<SScholar_Agent_Controller>().AgentList[i].destination.ToString(); // Target
            rowDataTemp[10] = "No Target Assigned"; // Target
            //need to get sky exposure variable, method below is not working....
            rowDataTemp[11] = GetComponent<SScholar_Agent_Controller>().AgentList[i].GetComponent<PlayerController>().sky_exposure.ToString(); // Sky Exposure
            rowData.Add(rowDataTemp);
        }

        string[][] output = new string[rowData.Count][];

        for (int i = 0; i < output.Length; i++)
        {
            output[i] = rowData[i];
        }

        int length = output.GetLength(0);
        string delimiter = ",";

        StringBuilder sb = new StringBuilder();

        for (int index = 0; index < length; index++)
            sb.AppendLine(string.Join(delimiter, output[index]));


        string filePath = getPath();

        StreamWriter outStream = System.IO.File.CreateText(filePath);
        outStream.WriteLine(sb);
        outStream.Close();
    }


   
    

    // Following method is used to retrive the relative path as device platform
    private string getPath()
    {
#if UNITY_EDITOR
        return Application.dataPath + "/CSV/" + "Saved_data.csv";
#elif UNITY_ANDROID
        return Application.persistentDataPath+"Saved_data.csv";
#elif UNITY_IPHONE
        return Application.persistentDataPath+"/"+"Saved_data.csv";
#else
        return Application.dataPath +"/"+"Saved_data.csv";
#endif
    }
    // Following method is used to retrive the relative path as device platform
    private string getPath(int x)
    {
        return Application.dataPath + "/CSV/" + "Saved_data"+"_"+x+".csv";
    }
}


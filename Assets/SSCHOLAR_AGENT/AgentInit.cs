using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System.IO;


[System.Serializable]
public class AgentInit
{
    public string Type;
    public string Block;
    public string Ward;
    public string ID;
    public string Sex;
    public string State;
    public int Total_Number;
    public string HomeObject;
    public string Color;

    public Dictionary<string, string> Itinerary;
    
    public override string ToString()
    {
        string returnvalue = "yo";
        returnvalue = "AgentInit ToString return = " + Type + " " + Block + " " + Ward + " " + ID + " " + Sex + " " + State + "       dictionary = " + Itinerary.ToString();
        return returnvalue;
    }
    

    public string ReturnTest()
    {
        return Itinerary["key2"].ToString();
    }

    public void ReturnDictionary()
    {
        foreach (string key in Itinerary.Keys)
        {
            string val = Itinerary[key];
            //Debug.Log(key + " = " + val);
        }
    }
}
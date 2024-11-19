using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Level_Manager : MonoBehaviour
{
    // Start is called before the first frame update
    public int level_number;
    private GameObject bp_obj;
    private building_placement bp_script;
    void Start()
    {
        bp_obj = GameObject.Find("Building_Placement");
        bp_script = bp_obj.GetComponent<building_placement>();
        if(level_number==0)
        {
            bp_script.Generate_Random_Terrain();
        }
        else
        {

        }
    }

    // Update is called once per frame
    void Save_Level()
    {
        string json = JsonUtility.ToJson(bp_script);
        string filePath=Application.dataPath+"/Data/Levels/level_"+level_number+".json";
        File.WriteAllText(filePath, json);
    }
}

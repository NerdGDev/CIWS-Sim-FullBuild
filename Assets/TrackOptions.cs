using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrackOptions : MonoBehaviour
{
    public CommandController cc;
    public Dropdown dd;

    public GameController trackCamera;
    public Text techText;

    // Start is called before the first frame update
    void Start()
    {
        cc = FindObjectOfType<CommandController>();
        ListOptions();
        dd.onValueChanged.AddListener(delegate {
            UpdateTrackingCamera();
        });

        trackCamera.target = cc.transform;

    }

    void UpdateTrackingCamera()
    {
        int count = 0;
        if (count == dd.value) 
        {
            trackCamera.target = cc.transform;
        }
        count++;
        foreach (FireControlSystem fcs in cc.FireControlSystems)
        {
            if (count == dd.value)
                trackCamera.target = fcs.transform;
            count++;
        }
        foreach (SearchRadarController src in FindObjectsOfType<SearchRadarController>())
        {
            if (count == dd.value)
                trackCamera.target = src.transform;
            count++;
        }
        foreach (MissileBasic ms in FindObjectsOfType<MissileBasic>())
        {
            if (count == dd.value)
                trackCamera.target = ms.transform;
            count++;
        }
        

    }

    private void LateUpdate()
    {
        int value = dd.value;
        ListOptions();
        dd.value = value;

        UpdateTechData();
    }

    void UpdateTechData() 
    {
        string techData = "";

        int count = 0;
        if (count == dd.value)
        {
            techData = cc.GetStatusData();
        }
        count++;
        foreach (FireControlSystem fcs in cc.FireControlSystems)
        {
            if (count == dd.value)
                techData = fcs.GetStatusData();
            count++;
        }
        foreach (SearchRadarController src in FindObjectsOfType<SearchRadarController>())
        {
            if (count == dd.value)
                techData = "";
            count++;
        }
        foreach (MissileBasic ms in FindObjectsOfType<MissileBasic>())
        {
            if (count == dd.value)
                techData = "";
            count++;
        }
        

        techText.text = techData;

    }

    void ListOptions()
    {
        dd.ClearOptions();
        
        List<string> optionList = new List<string>();

        int count = 0;
        optionList.Add("Command Center");
        count++;
        count = 0;
        foreach (FireControlSystem fcs in cc.FireControlSystems)
        {
            optionList.Add("Weapon " + count.ToString());
            count++;
        }
        count = 0;
        foreach (SearchRadarController src in FindObjectsOfType<SearchRadarController>())
        {
            optionList.Add("Search Radar " + count.ToString());
            count++;
        }
        count = 0;
        foreach (MissileBasic ms in FindObjectsOfType<MissileBasic>())
        {
            optionList.Add("Missile " + count.ToString());
            count++;
        }
        

        dd.AddOptions(optionList);



    }
}

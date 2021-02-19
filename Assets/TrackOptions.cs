using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrackOptions : MonoBehaviour
{
    public CommandController cc;
    public Dropdown dd;

    public GameController trackCamera;

    // Start is called before the first frame update
    void Start()
    {
        cc = FindObjectOfType<CommandController>();
        ListOptions();
        dd.onValueChanged.AddListener(delegate {
            UpdateTrackingCamera();
        });



    }

    void UpdateTrackingCamera()
    {
        int count = 0;
        foreach (FireControlSystem fcs in cc.FireControlSystems)
        {
            if (count == dd.value)
                trackCamera.target = fcs.transform;
            count++;
        }
        foreach (MissileBasic ms in FindObjectsOfType<MissileBasic>())
        {
            if (count == dd.value)
                trackCamera.target = ms.transform;
            count++;
        }
        foreach (SearchRadarController src in FindObjectsOfType<SearchRadarController>())
        {
            if (count == dd.value)
                trackCamera.target = src.transform;
            count++;
        }

    }

    private void LateUpdate()
    {
        int value = dd.value;
        ListOptions();
        dd.value = value;
    }

    void ListOptions()
    {
        dd.ClearOptions();
        
        List<string> optionList = new List<string>();

        int count = 0;
        foreach (FireControlSystem fcs in cc.FireControlSystems)
        {
            optionList.Add("Weapon " + count.ToString());
            count++;
        }
        count = 0;
        foreach (MissileBasic ms in FindObjectsOfType<MissileBasic>())
        {
            optionList.Add("Missile " + count.ToString());
            count++;
        }
        count = 0;
        foreach (SearchRadarController src in FindObjectsOfType<SearchRadarController>())
        {
            optionList.Add("Search Radar " + count.ToString());
            count++;
        }

        dd.AddOptions(optionList);



    }
}

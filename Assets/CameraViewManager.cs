using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraViewManager : MonoBehaviour
{
    public CommandController cc;
    public Dropdown dd;

    int lastValue = 0;

    public RawImage s1;
    public RenderTexture r1;

    public RawImage s2;
    public RenderTexture r2;
    // Start is called before the first frame update
    void Start()
    {
        cc = FindObjectOfType<CommandController>();
        ListOptions();
        dd.onValueChanged.AddListener(delegate {
            UpdateCamera();
         });

    }

    void UpdateCamera() 
    {
        cc.FireControlSystems[lastValue].trackCamera.targetTexture = null;
        cc.FireControlSystems[lastValue].cannonCamera.targetTexture = null;

        Debug.LogError("Camera Changed");
        cc.FireControlSystems[dd.value].trackCamera.targetTexture = r1;
        cc.FireControlSystems[dd.value].cannonCamera.targetTexture = r2;

        lastValue = dd.value;


    }

    void ListOptions() 
    {
        dd.ClearOptions();
        int count = 0;
        List<string> optionList = new List<string>();
        foreach (FireControlSystem fcs in cc.FireControlSystems) 
        {
            optionList.Add(count.ToString());
            count++;
        }
        dd.AddOptions(optionList);


        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

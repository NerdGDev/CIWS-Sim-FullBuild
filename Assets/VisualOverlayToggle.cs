using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VisualOverlayToggle : MonoBehaviour
{
    Toggle tg;

    // Start is called before the first frame update
    void Start()
    {
        tg = GetComponent<Toggle>();
        tg.onValueChanged.AddListener(delegate {
            if (tg.isOn)
            {
                FindObjectOfType<SceneMaster>().SetDebugMode(VisualiserMode.OVERLAY);
            }
            else
            {
                FindObjectOfType<SceneMaster>().SetDebugMode(VisualiserMode.NONE);
            }
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CIWSDebugableObject : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SceneMaster sm = FindObjectOfType<SceneMaster>();
        sm.overlayModeUpdated += OverlayUpdated;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OverlayUpdated(OverlayMode overlayMode) {
        
    }


}

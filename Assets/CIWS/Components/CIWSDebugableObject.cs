using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CIWSDebugableObject : MonoBehaviour
{
    SceneMaster sm;



    // Start is called before the first frame update
    void Start()
    {
        sm = FindObjectOfType<SceneMaster>();
        if (sm == null)
        {
            Debug.LogError("Debuggable Object could not find a Scene Master Object");
        }
        else 
        {
            sm.overlayModeUpdated += OverlayUpdated;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OverlayUpdated(VisualiserMode overlayMode) {
        
    }

    void OnNoneVisualiser() {
    
    }

    void OnOverlayVisualiser() {
    
    }

    void OnShadedVisualiser() {
    
    }

    void OnShadedOverlayVisualiser()
    {

    }

}

public enum VisualiserMode{
    NONE,
    OVERLAY,
    SHADED,
    SHADEDOVERLAY
}

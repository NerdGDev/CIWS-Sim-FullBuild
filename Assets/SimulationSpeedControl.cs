using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SimulationSpeedControl : MonoBehaviour
{
    Slider s;
    // Start is called before the first frame update
    void Start()
    {
        s = GetComponent<Slider>();
        s.onValueChanged.AddListener(delegate 
        {
            UpdateSimulationSpeed();
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UpdateSimulationSpeed() 
    {
        Time.timeScale = s.value;
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissileSpawnButton : MonoBehaviour
{
    public Button b;
    // Start is called before the first frame update
    void Start()
    {
        b.onClick.AddListener(delegate
        {
            FindObjectOfType<MissileSpawnUnit>().SpawnMissile();
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

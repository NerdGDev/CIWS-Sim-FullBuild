using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackingRadarController : MonoBehaviour
{
    public bool isBusy;
    public bool isAssigned;

    public WeaponSystemController bounded;

    public CommandController commandController;

    TargetAssignment targetAssignment;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AssignTarget(TargetAssignment targetAssignment) {
        this.targetAssignment = targetAssignment;
    }




}

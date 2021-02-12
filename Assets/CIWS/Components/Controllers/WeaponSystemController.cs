using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSystemController : MonoBehaviour
{
    public bool isBusy;
    public bool isAssigned;

    public TrackingRadarController bounded;

    public CommandController commandController;

    TargetAssignment targetAssignment;


    // Start is called before the first frame update
    void Start()
    {
        if (commandController)
            commandController.ConnectWeaponSystem(this);


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        
    }

    public void AssignTarget(TargetAssignment targetAssignment)
    {
        this.targetAssignment = targetAssignment;
    }
}

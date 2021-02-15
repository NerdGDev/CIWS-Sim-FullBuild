using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class FireControlSystem : MonoBehaviour
{
    public Status status;

    public CommandController cc;

    public TrackingRadarController trc;

    public WeaponSystemController wsc;

    public CIWSDataLink dl;

    public int targetSignature;
    public Vector3 targetPosition;

    

    // Start is called before the first frame update
    void Start()
    {
        dl.receivedData -= ReceivedData;
        dl.receivedData += ReceivedData;

        status = Status.IDLE;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ReceivedData(CIWSDataLinkPackage dataLinkPackage)
    {
        if (dataLinkPackage.GetPackageContentType() == PackageContent.COMMAND) {
            HandleCommand((DLPCommand)dataLinkPackage);
        }
    }

    void HandleCommand(DLPCommand package) 
    {
        var data = package.GetPackageData();
        FireControlSystem.Commands command = data.command;
        Vector3 targetPosition = data.position;
        int signatureID = data.signatureID;
    }

    public void ChangeStatus(FireControlSystem.Status status) 
    { 
        
    }

    public enum Commands
    {
        RESET, // Return to Idle
        DISENGAGE, // Stop Firing on Target and Return to Idle When Possible
        TRACK, // Track Target but Wait to Fire
        ENGAGE // Fire at Target
    }

    public enum Status {
        IDLE,
        WORKING,
        BUSY
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandController : MonoBehaviour
{
    [SerializeField]
    public List<SearchRadarController> SearchRadars;

    [SerializeField]
    public List<FireControlSystem> FireControlSystems;

    Dictionary<int, Vector3> targetDictionary;
    Dictionary<int, FireControlSystem> targetAssignment;

    //Dictionary<int, TargetAssignment> targetAssignmentsDictionary;
    CIWSDataLink dl;
    

    // Start is called before the first frame update
    void Start()
    {
        targetDictionary = new Dictionary<int, Vector3>();
        targetAssignment = new Dictionary<int, FireControlSystem>();
        //targetAssignmentsDictionary = new Dictionary<int, TargetAssignment>();

        dl = GetComponent<CIWSDataLink>();
        dl.receivedData += ReceivedData;

        StartCoroutine(ComputeCycle());
    }

    IEnumerator ComputeCycle() 
    {
        for (; ; ) 
        {
            // Debug
            Debug.Log("Compute Tick");
            UpdateTargetAssignments();



            // Repeat Timer
            yield return new WaitForSeconds(2f);
        }
    }

    void RequestStatusUpdates() 
    {
        
    }

    void UpdateTargetAssignments() 
    {
        FireControlSystems.ForEach(delegate (FireControlSystem fcs) 
        {
            if (fcs.status == FireControlSystem.Status.IDLE) 
            {
                int? optimalTargetSig = null;
                foreach (var item in targetDictionary)
                {
                    
                    if (!targetAssignment.ContainsKey(item.Key))
                    {
                        if (optimalTargetSig != null)
                        {
                            if (Vector3.Distance(targetDictionary[(int)optimalTargetSig], fcs.transform.position) > Vector3.Distance(item.Value, fcs.transform.position))
                            {
                                optimalTargetSig = item.Key;
                            }
                        }
                        else
                        {
                            optimalTargetSig = item.Key;
                        }
                    }
                }
                if (optimalTargetSig != null) 
                {
                    targetAssignment.Add((int)optimalTargetSig, fcs);
                    SendTargetAssignment(fcs, (int)optimalTargetSig, targetDictionary[(int)optimalTargetSig]);
                }
            }                
        });        
    }


    void SendTargetAssignment(FireControlSystem fcs, int signatureID, Vector3 position) 
    {
        //Debug.Log("Send Target Assignment");
        DLPCommand package = new DLPCommand(FireControlSystem.Commands.ENGAGE, position, signatureID);
        dl.transmitData(package, fcs.gameObject.GetInstanceID());
    }

    // Update is called once per frame
    void Update()
    {


        
    }

    public void ReceivedData(CIWSDataLinkPackage dataLinkPackage) 
    {
        //Debug.Log(dataLinkPackage.GetType().ToString());

        switch (dataLinkPackage.GetPackageContentType()) {
            case PackageContent.DETECTION:
                HandleDetection((DLPDetection)dataLinkPackage);
                break;
            case PackageContent.KILL:
                HandleKill((DLPKill)dataLinkPackage);
                break;
            default:
                break;
        
        }
    }

    private void HandleDetection(DLPDetection package) 
    {
        var data = package.GetPackageData();
        if (targetDictionary.ContainsKey(data.signatureID))
        {
            //Debug.Log("Updating Target");
            targetDictionary[data.signatureID] = data.position;
        }
        else
        {
            //Debug.Log("New Target");
            targetDictionary.Add(data.signatureID, data.position);
        }
    }

    private void HandleKill(DLPKill package) 
    {
        int signature = package.GetPackageData();
        targetDictionary.Remove(signature);
        targetAssignment.Remove(signature);
    }

    public void ConnectSearchRadar(SearchRadarController searchRadarController)
    {
        SearchRadars.Add(searchRadarController);
    }

    public void DisonnectSearchRadar(SearchRadarController searchRadarController)
    {
        SearchRadars.Remove(searchRadarController);
    }

    public void UpdateIDPosition() {
    
    }

    public string GetStatusData() 
    {
        string statusData = "";
        statusData += "Connected Systems: " + FireControlSystems.Count.ToString() + "\n";
        statusData += "\n";
        statusData += "Targets: " + targetDictionary.Count.ToString() + "\n";
        statusData += "Assigned: " + targetAssignment.Count.ToString() + "\n";


        return statusData;
    }

}


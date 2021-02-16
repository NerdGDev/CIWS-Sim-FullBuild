using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandController : MonoBehaviour
{
    [SerializeField]
    List<SearchRadarController> SearchRadars;

    [SerializeField]
    List<FireControlSystem> FireControlSystems;

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
        foreach (var item in targetDictionary)
        {
            if (!targetAssignment.ContainsKey(item.Key)) {
                FireControlSystem optimalFCS = null;

                FireControlSystems.ForEach(delegate (FireControlSystem fcs)
                {
                    if (fcs.status == FireControlSystem.Status.IDLE) 
                    {
                        if (optimalFCS != null)
                        {
                            float d = Vector3.Distance(fcs.transform.position, item.Value);
                            if (d < Vector3.Distance(optimalFCS.transform.position, item.Value)) 
                            {
                                Debug.Log("New Optimal");
                                optimalFCS = fcs;
                            }

                        }
                        else 
                        {
                            optimalFCS = fcs;
                        }
                        

                    }
                });
                if (optimalFCS != null) {
                    SendTargetAssignment(optimalFCS, item.Key, item.Value);
                }
            }
        }
    }


    void SendTargetAssignment(FireControlSystem fcs, int signatureID, Vector3 position) 
    {
        Debug.Log("Send Target Assignment");
        DLPCommand package = new DLPCommand(FireControlSystem.Commands.ENGAGE, position, signatureID);
        dl.transmitData(package, fcs.gameObject.GetInstanceID());
    }

    // Update is called once per frame
    void Update()
    {


        
    }

    public void ReceivedData(CIWSDataLinkPackage dataLinkPackage) 
    {
        Debug.Log(dataLinkPackage.GetType().ToString());

        switch (dataLinkPackage.GetPackageContentType()) {
            case PackageContent.DETECTION:
                HandleDetection((DLPDetection)dataLinkPackage);
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
            Debug.Log("Updating Target");
            targetDictionary[data.signatureID] = data.position;
        }
        else
        {
            Debug.Log("New Target");
            targetDictionary.Add(data.signatureID, data.position);
        }
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
}


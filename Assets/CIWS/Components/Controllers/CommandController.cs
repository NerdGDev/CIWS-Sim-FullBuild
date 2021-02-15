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
    //Dictionary<int, TargetAssignment> targetAssignmentsDictionary;

    

    // Start is called before the first frame update
    void Start()
    {
        targetDictionary = new Dictionary<int, Vector3>();
        //targetAssignmentsDictionary = new Dictionary<int, TargetAssignment>();

        CIWSDataLink dataLink = GetComponent<CIWSDataLink>();
        dataLink.receivedData += ReceivedData;

        StartCoroutine(ComputeCycle());
    }

    IEnumerator ComputeCycle() 
    {
        for (; ; ) 
        {
            // Debug
            Debug.Log("Compute Tick");




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
            
        }
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


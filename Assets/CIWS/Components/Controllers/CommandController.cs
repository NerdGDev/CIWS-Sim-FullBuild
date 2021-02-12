using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandController : MonoBehaviour
{
    [SerializeField]
    List<SearchRadarController> SearchRadars;

    [SerializeField]
    List<TrackingRadarController> TrackingRadars;

    [SerializeField]
    List<WeaponSystemController> WeaponSystems;

    Dictionary<int, Vector3> targetDictionary;
    Dictionary<int, TargetAssignment> targetAssignmentsDictionary;

    

    // Start is called before the first frame update
    void Start()
    {
        targetDictionary = new Dictionary<int, Vector3>();
        targetAssignmentsDictionary = new Dictionary<int, TargetAssignment>();
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var item in targetDictionary) {
            if (!targetAssignmentsDictionary.ContainsKey(item.Key))
            {
                foreach (TrackingRadarController trackingRadar in TrackingRadars) {
                    if (!trackingRadar.isAssigned)
                    {
                        if (trackingRadar.bounded == null)
                        {
                            foreach (WeaponSystemController weaponSystem in WeaponSystems)
                            {
                                if (weaponSystem.bounded == null) {
                                    TargetAssignment ta = new TargetAssignment(item.Key, item.Value, trackingRadar, weaponSystem);
                                    ta.AssignTarget(true);
                                    Debug.Log("Assigned Target");
                                    targetAssignmentsDictionary.Add(item.Key, ta);

                                }
                            }
                        }
                        else
                        { 
                            
                        }

                    }
                    
                    
                }

                
            }
        }
    }

    public void ConnectWeaponSystem(WeaponSystemController weaponSystemController) 
    {
        WeaponSystems.Add(weaponSystemController);
    }

    public void DisonnectWeaponSystem(WeaponSystemController weaponSystemController)
    {
        WeaponSystems.Remove(weaponSystemController);
    }

    public void ConnectTrackingRadar(TrackingRadarController trackingRadarController)
    {
        TrackingRadars.Add(trackingRadarController);
    }

    public void DisonnectTrackingRadar(TrackingRadarController trackingRadarController)
    {
        TrackingRadars.Remove(trackingRadarController);
    }

    public void ConnectSearchRadar(SearchRadarController searchRadarController)
    {
        SearchRadars.Add(searchRadarController);
        searchRadarController.radarToneSignal += RadarTone;
    }

    public void DisonnectSearchRadar(SearchRadarController searchRadarController)
    {
        SearchRadars.Remove(searchRadarController);
        searchRadarController.radarToneSignal -= RadarTone;
    }

    public void RadarTone(int radarID, GameObject radarObject, int signatureID, Vector3 position) {
        Debug.Log("Tone Recieved");
        Debug.DrawLine(transform.position, radarObject.transform.position, Color.red);
        if (!targetDictionary.ContainsKey(signatureID))
        {
            targetDictionary.Add(signatureID, position);
        }
        else 
        {
            targetDictionary[signatureID] = position;
        }
        


    }

    public void ConfirmDeath(int signatureID) {
    
    }

    public void UpdateIDPosition() {
    
    }
}

public class TargetAssignment{

    public int signatureID;
    public Vector3 positionAtAssigned;
    public bool assigned;
    public TrackingRadarController assignedTracker;
    public WeaponSystemController assignedWeaponSystem;
    public TargetAssignment(int signatureID, Vector3 position, TrackingRadarController trackingRadarController, WeaponSystemController weaponSystemController) {
        this.signatureID = signatureID;
        this.positionAtAssigned = position;
        this.assignedTracker = trackingRadarController;
        this.assignedWeaponSystem = weaponSystemController;
        this.assigned = false;
    }

    public void AssignTarget(bool assigned) {
        this.assigned = assigned;
        assignedTracker.AssignTarget(this);

    }

    
}
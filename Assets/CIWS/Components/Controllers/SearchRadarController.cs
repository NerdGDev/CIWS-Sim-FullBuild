using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchRadarController : MonoBehaviour
{
    bool isBusy;

    public CommandController commandController;

    CIWSDataLink dataLink;

    ConstantMotion motionComponent;

    public Transform turretRing;

    public Transform radarBeamShape;

    public float rotationRate;

    public float optimalRange;
    public float width;
    public float height;
    public float elevation;

    public GameObject visualPrefab;

    private void OnValidate()
    {
        radarBeamShape.localRotation = Quaternion.Euler(elevation, 0, 0);
        radarBeamShape.localScale = new Vector3(width, height, 1f * optimalRange * 1.5f);
    }

    // Start is called before the first frame update
    void Start()
    {
        motionComponent = GetComponent<ConstantMotion>();
        motionComponent.motionSpeed = rotationRate;

        DetectionMeshHandler dmh = GetComponentInChildren<DetectionMeshHandler>();
        dmh.detectionEnterDelegate += DetecionEnter;
        dmh.detectionStayDelegate += DetecionStay;
        dmh.detectionExitDelegate += DetectionExit;

        dataLink = GetComponent<CIWSDataLink>();

        dataLink.receivedData += ReceivedData;

        commandController.ConnectSearchRadar(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        turretRing.localRotation *= Quaternion.Euler(0, rotationRate * Time.fixedDeltaTime, 0);
        radarBeamShape.GetComponent<Rigidbody>().MoveRotation(turretRing.localRotation * Quaternion.Euler(0, 0, elevation));

    }

    public void ReceivedData(CIWSDataLinkPackage dataLinkPackage) 
    {
    
    }

    public void SubmitDetection(Transform target)
    {
        
    }

    public void DetecionEnter(int signatureID, Vector3 position) 
    {
        //Debug.DrawLine(transform.position, position, Color.red);
        
        GameObject visualiser = Instantiate(visualPrefab, transform.position, transform.rotation);
        visualiser.GetComponent<RadarBounceVisualiser>().origin = radarBeamShape.position;
        visualiser.GetComponent<RadarBounceVisualiser>().target = position;
        //visualiser.GetComponent<RadarBounceVisualiser>().step = Vector3.Distance(transform.position, position) * (Time.fixedDeltaTime * 2 * 4f);
        CIWSDataLinkPackage package = new DLPackageDetection(GetInstanceID(), this, signatureID, position);
        dataLink.TransmitDataLink(package);
        //radarToneSignal(GetInstanceID(), signatureID, position);
    }

    public void DetecionStay(int signatureID, Vector3 position)
    {
        CIWSDataLinkPackage package = new DLPackageDetection(GetInstanceID(), this, signatureID, position);
        dataLink.TransmitDataLink(package);

        //Debug.DrawLine(transform.position, position, Color.blue);
    }

    public void DetectionExit(int signatureID, Vector3 position)
    {
        CIWSDataLinkPackage package = new DLPackageDetection(GetInstanceID(), this, signatureID, position);
        dataLink.TransmitDataLink(package);

        //Debug.DrawLine(transform.position, position, Color.green);

        //radarToneSignal(GetInstanceID(), signatureID, position);
    }


}

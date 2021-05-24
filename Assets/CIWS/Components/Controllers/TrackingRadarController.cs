using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackingRadarController : MonoBehaviour
{
    public bool isBusy;
    public bool isAssigned;

    public FireControlSystem fcs;

    public TurretMotionBase tm;

    public Transform trackingBeamShape;

    public float optimalRange;
    public float width;
    public float height;

    public bool lastTickFound;
    public bool targetFound;
    public int targetSig;
    public Vector3 targetPos;
    //public Queue<Vector3> movementTracking = new Queue<Vector3>();
    public Vector3[] movementTracking = new Vector3[75 * 2];

    //TargetAssignment targetAssignment;

    public enum Alignment
    {
        UNALIGNED,
        ALIGNING,
        ALIGNED
    }

    public Alignment alignmentStatus = Alignment.UNALIGNED;
    Coroutine alignmentCoroutine;

    IEnumerator StartAlignment(float time)
    {
        yield return new WaitForSecondsRealtime(time);
        alignmentStatus = Alignment.ALIGNED;
    }

    private void OnValidate()
    {
        trackingBeamShape.localScale = new Vector3(width, height, 1f * optimalRange * 1.5f);
    }

    // Start is called before the first frame update
    void Start()
    {

        TrackingDetectionMeshHandler tdmh = GetComponentInChildren<TrackingDetectionMeshHandler>();
        tdmh.detectionStayDelegate += HandleDetection;

    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (!lastTickFound) {
            if (alignmentStatus == Alignment.ALIGNING) 
            {
                targetFound = false;
                StopCoroutine(alignmentCoroutine);
                alignmentStatus = Alignment.UNALIGNED;
            }
            else if (alignmentStatus == Alignment.ALIGNED) 
            {
                targetFound = false;
                alignmentStatus = Alignment.UNALIGNED;
            }

            
        }
        lastTickFound = false;
    }

    //Attempt to Look for Target at Provided Position
    public void LookForTarget(int signatureID, Vector3 position) 
    {
        if (targetFound)
        {
            tm.SetTargetDirection(targetPos - transform.position);
        }
        else 
        {
            tm.SetTargetDirection(position - transform.position);
        }

        
        targetSig = signatureID;
    }

    public void ResetTracking()
    {
        tm.ResetDirection();
        movementTracking = new Vector3[75 * 2];
        targetFound = false;
    }

    public void HandleDetection(int signatureID, Vector3 position) 
    {
        //Debug.Log(signatureID.ToString());
        //Debug.Log(targetSig.ToString());
        if (signatureID == targetSig) 
        {
            lastTickFound = true;
            if (alignmentStatus == Alignment.UNALIGNED)
            {
                alignmentStatus = Alignment.ALIGNING;
                alignmentCoroutine = StartCoroutine(StartAlignment(2.1f));
            }
            //Debug.Log("Found");
            targetFound = true;
            targetPos = position;
            Vector3[] tempArr = new Vector3[75 * 2];
            System.Array.ConstrainedCopy(movementTracking, 1, tempArr, 0, (75 * 2) - 1);
            movementTracking = tempArr;
            movementTracking[movementTracking.GetUpperBound(0)] = position;
            //Debug.Log(string.Join(",", movementTracking));
        }
    }



    //public void AssignTarget(TargetAssignment targetAssignment) {
    //    this.targetAssignment = targetAssignment;
    //}




}

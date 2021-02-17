using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class FireControlSystem : MonoBehaviour
{
    public Status status;
    public Commands command;

    public int targetSignature;
    public Vector3 targetPosition;

    public Commands waitingCommand;

    public CommandController cc;

    public TrackingRadarController trc;

    public WeaponSystemController wsc;

    public CIWSDataLink dl;   

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
        switch (this.command) {
            case Commands.RESET:
                break;
            case Commands.DISENGAGE:
                break;
            case Commands.TRACK:
                break;
            case Commands.ENGAGE:
                EngageTarget();
                break;
            default:
                break;
        }
    }

    void EngageTarget() 
    {
        //Debug.Log("Engage");
        trc.LookForTarget(targetSignature, targetPosition);
        if (trc.targetFound) 
        {
            float calcedAcceleration = Vector3.Distance(trc.movementTracking[24], trc.movementTracking[0]);
            float accelerationOverTime = (1 / calcedAcceleration) * Vector3.Distance(trc.movementTracking[24], trc.movementTracking[0]);
            Vector3 lastPosition = trc.movementTracking[0];
            Vector3 determinedVelocity = trc.movementTracking[24] - lastPosition;

            for (int i = 1; i < 13; i ++) 
            {
                //Debug.Log(i + (24 * i) - 1);
                //Debug.LogError(Vector3.Distance(trc.movementTracking[i + (24 * i) - 1], lastPosition));
                //determinedVelocity += trc.movementTracking[i + (24 * i) - 1] - lastPosition;
                accelerationOverTime += (1 / calcedAcceleration) * Vector3.Distance(trc.movementTracking[i + (24 * i) - 1], lastPosition);
                calcedAcceleration += Vector3.Distance(trc.movementTracking[i + (24 * i) - 1], lastPosition);
                lastPosition = trc.movementTracking[i + (24 * i) - 1];
            }
            //Debug.Log(calcedAcceleration);
            //Debug.Log(determinedVelocity);
            Debug.Log(accelerationOverTime);
            calcedAcceleration /= 4f;
            accelerationOverTime /= 4f;
            accelerationOverTime = Mathf.Pow(accelerationOverTime,2);
            Debug.Log("Accel OT");
            Debug.Log(accelerationOverTime);
            //determinedVelocity /= 4f;
            determinedVelocity = (trc.movementTracking[299] - trc.movementTracking[298]) * 75; 
            //Debug.Log(calcedAcceleration);
            //Debug.Log(determinedVelocity);
            //Debug.Log(determinedVelocity.normalized);
            //Debug.Log(calcedAcceleration);

            determinedVelocity += (determinedVelocity.normalized * calcedAcceleration);
            Debug.DrawLine(trc.targetPos, trc.targetPos + determinedVelocity, Color.red,0.1f,false);
            //Debug.Log(determinedVelocity);

            var fs = CalculateFireSolution(trc.movementTracking[299],determinedVelocity, determinedVelocity.magnitude, accelerationOverTime);

            Debug.Log(fs.direction);
            Debug.Log(fs.timeToImpact);

            if (wsc.AimTo(fs.direction)) 
            {
                wsc.Fire(fs.direction, fs.timeToImpact);
            }


        }
    }

    (Vector3 direction, float timeToImpact) CalculateFireSolution(Vector3 tPosition, Vector3 tVelocity, float tSpeed, float accIncrease) 
    {
        Vector3 turretCenter = transform.position;

        Vector3 p0 = tPosition;
        Vector3 v0 = tVelocity.normalized;
        float s0 = tSpeed * Time.fixedDeltaTime;        

        Vector3 p1 = turretCenter;
        float s1 = 1100f * Time.fixedDeltaTime;

        s0 *= Mathf.Pow(accIncrease,(Vector3.Distance(p0, p1) / 1100f));

        float a = ((v0.x * v0.x) + (v0.y * v0.y) + (v0.z * v0.z)) - (s1 * s1);
        float b = 2 * (((p0.x * v0.x) + (p0.y * v0.y) + (p0.z * v0.z)) - ((p1.x * v0.x) - (p1.y * v0.y) - (p1.z * v0.z)));
        float c = ((p0.x * p0.x) + (p0.y * p0.y) + (p0.z * p0.z)) + ((p1.x * p1.x) + (p1.y + p1.y) + (p1.z + p1.z)) - ((2 * p1.x * p0.x) - (2 * p1.y * p0.y) - (2 * p1.z * p0.z));

        float t1 = (-b + Mathf.Sqrt((b * b) - (4 * a * c))) / (2 * a);
        float t2 = (-b - Mathf.Sqrt((b * b) - (4 * a * c))) / (2 * a);
        float t;

        if (t1 >= 0 && (t2 >= 0 ? t1 < t2 : true))
        {
            t = t1;
        }
        else if (t2 >= 0 && (t1 >= 0 ? t2 < t1 : true))
        {
            t = t2;
        }
        else
        {
            return(Vector3.zero, -1f);
        }
        t *= Time.fixedDeltaTime;
        float timeToImpact = t;
        //Debug.Log(accIncrease);
        //Debug.Log(timeToImpact);
        //Debug.Log(Mathf.Pow(accIncrease, timeToImpact));

        Vector3 v = (p0 - p1 + (t * s0 * v0)) / (t * s1);
        v.x = (p0.x - p1.x + (t * s0 * v0.x)) / (t * s1);
        v.y = (p0.y - p1.y + (t * s0 * v0.y)) / (t * s1);
        v.z = (p0.z - p1.z + (t * s0 * v0.z)) / (t * s1);

        return (v, timeToImpact);
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

        targetSignature = signatureID;
        this.targetPosition = targetPosition;

        this.command = command;

        switch (command) {
            case Commands.RESET:
                status = Status.IDLE;
                break;
            case Commands.DISENGAGE:
                status = Status.WORKING;
                break;
            case Commands.TRACK:
                status = Status.WORKING;
                break;
            case Commands.ENGAGE:
                status = Status.WORKING;
                break;
            default:
                break;
        }
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

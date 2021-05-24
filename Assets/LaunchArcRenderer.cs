using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchArcRenderer : MonoBehaviour
{
    public LineRenderer lr;

    public Transform renderCenter;

    public float velocity;
    public float angle;
    public int resolution;

    float g; //force of gravity on the y axis

    float radianAngle;

    public void SetRender(bool render) 
    {
        lr.enabled = render;
    }

    private void Awake()
    {
        //lr = GetComponent<LineRenderer>();
        g = Mathf.Abs(Physics.gravity.y);

    }

    private void OnValidate()
    {
        if (lr != null && Application.isPlaying)
        {
            RenderArc();
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    private void FixedUpdate()
    {
        RenderArc();
    }

    public void Capture() 
    {
        GameObject go = Instantiate(lr.gameObject, lr.transform.position, lr.transform.rotation);
        Destroy(go, 10f);
    }

    //initialization
    void RenderArc()
    {
        // obsolete: lr.SetVertexCount(resolution + 1);
        angle = 360 - renderCenter.localRotation.eulerAngles.x;
        lr.positionCount = resolution + 1;
        lr.SetPositions(CalculateArcArray());

    }

    //Create an array of Vector 3 positions for the arc
    Vector3[] CalculateArcArray()
    {
        Vector3[] arcArray = new Vector3[resolution + 1];

        radianAngle = Mathf.Deg2Rad * angle;

        float maxDistance = (velocity * velocity * Mathf.Sin(2 * radianAngle)) / g;

        for (int i = 0; i <= resolution; i++)
        {
            float t = (float)i / (float)resolution;
            arcArray[i] = CalculateArcPoint(t, maxDistance);
        }

        return arcArray;

    }



    Vector3 CalculateArcPoint(float t, float maxDistance)
    {
        float z = t * maxDistance;
        float y = z * Mathf.Tan(radianAngle) - ((g * z * z) / (2 * velocity * velocity * Mathf.Cos(radianAngle) * Mathf.Cos(radianAngle)));
        return RotatePointAroundPivot(new Vector3(0, y, z) + renderCenter.position, renderCenter.position, new Vector3(0, renderCenter.rotation.eulerAngles.y,0));
        

    }

    public Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles)
    {
        Vector3 dir = point - pivot; // get point direction relative to pivot
        dir = Quaternion.Euler(angles) * dir; // rotate it
        point = dir + pivot; // calculate rotated point
        return point; // return it
    }



}


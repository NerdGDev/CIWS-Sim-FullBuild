using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Motion)), RequireComponent(typeof(DataLink))]
public class SearchRadarController : Systembase
{
    Visualise visualise;

    [Header("Radar Beam")]
    [SerializeField] public GameObject radarCollider;
    public float optimalRange;
    public float width;
    public float height;
    public float elevation;

    public bool active;
    Motion motion;

    DataLink dataLink;

    LineRenderer lr;

    bool HasHit;

    private void OnValidate()
    {
        radarCollider.transform.localRotation = Quaternion.Euler(elevation, 0, 0);
        radarCollider.transform.localScale = new Vector3(width, height, 1f * optimalRange * 1.5f);
    }

    private void Awake()
    {
        lr = GetComponent<LineRenderer>();
        lr.SetPosition(0, transform.position);
        visualise = GetComponent<Visualise>();
        motion = GetComponent<Motion>();
        dataLink = GetComponent<DataLink>();
    }

    private void FixedUpdate()
    {
        visualise.AddDataField("Status", "Active");
        if (active)
            motion.targetDirection = Quaternion.AngleAxis(100 * Time.fixedDeltaTime, Vector3.up) * motion.targetDirection;
        if (HasHit) 
        {
            lr.enabled = false;
        }
    }
    private void LateUpdate()
    {
        HasHit = false;
    }

    public void ReportHit(int signature, Vector3 position) 
    {
        SendHit(signature, position);
        visualise.AddShortData("Detected", "Distance :" + Vector3.Distance(transform.position, position).ToString());
        HasHit = true;
        lr.enabled = true;
        lr.SetPosition(1, position);
    }

    void SendHit(int signature, Vector3 position) 
    {
        visualise.AddShortData("Sending", "Hit Data");
        DataLink target = dataLink.GetNearestOf<CommandController>();
        CommandController cc = target.GetComponent<CommandController>();
        cc.Report(signature, position);

    }
}

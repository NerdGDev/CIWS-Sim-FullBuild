using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DataLink))]
public class CommandController : Systembase
{
    Visualise visualise;

    DataLink dataLink;

    public Dictionary<int, TargetState> TargetReports = new Dictionary<int, TargetState>();
    public Dictionary<int, DataLink> TargetAssignment = new Dictionary<int, DataLink>();

    int assigned = 0;

    private void Awake()
    {
        visualise = GetComponent<Visualise>();
        dataLink = GetComponent<DataLink>();
        StartCoroutine(ComputeCycle());
    }

    private void FixedUpdate()
    {
        visualise.AddDataField("Talking To", dataLink.Connections.Count.ToString());
        visualise.AddDataField("Monitoring", TargetReports.Count.ToString() + " Threats");
        visualise.AddDataField("Assigned to CIWS", TargetAssignment.Count.ToString());
    }

    IEnumerator ComputeCycle() 
    {
        yield return new WaitForFixedUpdate();
        while (true) 
        {
            foreach (var target in TargetReports) 
            {
                if (!target.Value.assigned) 
                {
                    AssignTarget(target.Value);
                }
            }
            yield return new WaitForFixedUpdate();
        }
    }

    public void AssignTarget(TargetState ts) 
    {        
        var systems = dataLink.GetAllOf<FireControlSystem>();
        float distance = float.PositiveInfinity;
        FireControlSystem optimal = null;
        foreach (DataLink dl in systems) 
        {
            FireControlSystem fcs = dl.GetComponent<FireControlSystem>();
            if (!fcs.assigned && Vector3.Distance(fcs.transform.position, ts.position) < distance) 
            {
                optimal = fcs;
                distance = Vector3.Distance(fcs.transform.position, ts.position);
            }
        }
        if (optimal != null) 
        {
            //Debug.Log("Assigning Target");
            visualise.AddShortData("Action", "Sending Target Order");
            assigned++;
            ts.assigned = true;
            optimal.EngageTarget(ref ts);
        }
    }

    public void Report(int signature, Vector3 position) 
    {
        if (!TargetReports.ContainsKey(signature))
        {
            visualise.AddShortData("Action", "New Target Recieved");
            TargetReports.Add(signature, new TargetState(signature, position));
        }
        else if (TargetReports.ContainsKey(signature)) 
        {
            TargetState ts = TargetReports[signature];
            ts.position = position;
        }
    }

    public void ReturnTargetState(int signature, bool dead) 
    {
        visualise.AddShortData("Action", "Recieved Report");
        if (dead) 
        {
            assigned--;
            visualise.AddShortData("Action", "Kill Confirmed");
            Debug.LogError("Kill Confirmed");
            TargetReports.Remove(signature);
            TargetAssignment.Remove(signature);
        }
    }
}

public class TargetState 
{
    public TargetState(int s, Vector3 p) 
    {
        signature = s;
        position = p;
        assigned = false;
    }

    public int signature { get; set; }
    public Vector3 position { get; set; }
    public bool assigned { get; set; }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadarBounceVisualiser : MonoBehaviour
{
    public Transform origin;
    public Transform target;
    public float step;

    bool bounced = false;
    bool skipStarTick = true;

    TrailRenderer tr;
    // Start is called before the first frame update
    void Start()
    {
        step = 10f;
        tr = GetComponentInChildren<TrailRenderer>();
        tr.startWidth = 1f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Skips first tick for corrected rendering from origin
        if (skipStarTick) {
            skipStarTick = false;
            step = Vector3.Distance(origin.position, target.position) * (Time.fixedDeltaTime * 5f);
            return;
        }

        transform.position = Vector3.MoveTowards(transform.position, target.position, step);
        tr.startWidth += 1.5f * Time.fixedDeltaTime;
        if (Vector3.Distance(transform.position, target.position) < 1f && !bounced)
        {
            //Debug.Log("Bounced");
            tr.startWidth = 1f;
            // Swap the position of the cylinder.
            this.target = this.origin;
            bounced = true;
        }

        if (Vector3.Distance(transform.position, target.position) < 1f && bounced)
        {
            Destroy(gameObject,1f);
        }

    }
}

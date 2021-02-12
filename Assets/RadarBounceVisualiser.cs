using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadarBounceVisualiser : MonoBehaviour
{
    public Vector3 origin;
    public Vector3 target;
    public float step;

    bool bounced = false;
    bool skipStarTick = true;
    // Start is called before the first frame update
    void Start()
    {
        step = 3000f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (skipStarTick) {
            skipStarTick = false;
            return;
        }

        transform.position = Vector3.MoveTowards(transform.position, target, step);
        if (Vector3.Distance(transform.position, target) < 1f && !bounced)
        {
            Debug.Log("Bounced");
            // Swap the position of the cylinder.
            this.target = this.origin;
            bounced = true;
        }

        if (Vector3.Distance(transform.position, target) < 1f && bounced)
        {
            Destroy(gameObject,1f);
        }

    }
}

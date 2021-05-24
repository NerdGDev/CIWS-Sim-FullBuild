using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataLinkVisualiser : MonoBehaviour
{
    public Vector3 target;
    public float step;

    bool skipStarTick = true;
    // Start is called before the first frame update
    void Start()
    {
        step = 300f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (skipStarTick)
        {
            skipStarTick = false;
            return;
        }

        transform.position = Vector3.MoveTowards(transform.position, target, step);
        
        if (Vector3.Distance(transform.position, target) < 1f)
        {
            Destroy(gameObject, 1f);
        }

    }
}

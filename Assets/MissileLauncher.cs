using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileLauncher : Systembase
{
    public Transform[] Launchers;
    public Motion motion;
    public LaunchArcRenderer arcRender;

    public Transform Target;

    public GameObject missileReference;

    
    private void Awake()
    {
        arcRender = GetComponent<LaunchArcRenderer>();
        motion = GetComponent<Motion>();
    }

    public void NewLaunch() 
    {
        AddOrder((go) => { StartCoroutine(LaunchMissile()); });
    }

    IEnumerator LaunchMissile() 
    {
        Vector3 AimCal = Target.transform.position + (Random.insideUnitSphere * 2500f);
        AimCal.y += 4000f;
        motion.TargetToDirection(AimCal);
        arcRender.SetRender(true);
        while (motion.GetAlignment() > 0.5f) 
        {
            yield return new WaitForFixedUpdate();
        }
        Launch();
        arcRender.Capture();
        yield return new WaitForSeconds(1f);
        NextOrder();
    }

    public void Launch() 
    {
        Transform launcher = Launchers[Random.Range(0, Launchers.Length)];
        GameObject missile = Instantiate(missileReference, launcher.position, launcher.rotation);
        missile.GetComponent<Rigidbody>().velocity = launcher.forward * 300f;
    }

    
}

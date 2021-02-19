using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileSpawnUnit : MonoBehaviour
{
    public GameObject missile;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnMissile() 
    {
        Vector3 spawnPosition = transform.position + (new Vector3(Random.Range(-1f,1f),0,Random.Range(-1f,1f)).normalized * 10000f);



        Vector3 aimPoint = transform.position + Random.insideUnitSphere * 1500f;

        GameObject go = Instantiate(missile, spawnPosition, Quaternion.LookRotation(aimPoint - spawnPosition));
        go.GetComponent<MissileBasic>().thrust = Random.Range(100f, 1250f);
    }
}

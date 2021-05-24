using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[RequireComponent(typeof(Collider))]
public class DataLink : MonoBehaviour
{
    public enum Type
    {
        GENERIC,
        COMMAND,
        FIRECONTROLSYSTEM,        
        RADAR
    }

    [SerializeField] public int identification;
    [SerializeField] public Type type;

    [SerializeField] public float range = 1000f;
    [SerializeField] public Vector3 position;

    [SerializeField] public List<DataLink> Connections = new List<DataLink>();

    public GameObject DataLinkFX;
    public Dictionary<DataLink, bool> FXLine = new Dictionary<DataLink, bool>();

    private void Awake()
    {
        identification = GetInstanceID();
        position = transform.position;
        Collider[] colliders = Physics.OverlapSphere(transform.position, range);

        foreach (Collider col in colliders) 
        {
            if (col.GetComponent<DataLink>() && col.GetComponent<DataLink>() != this) 
            {
                Connections.Add(col.GetComponent<DataLink>());
                FXLine.Add(col.GetComponent<DataLink>(), false);
            }
        }
    }

    public DataLink GetNearestOf<T>() where T : MonoBehaviour
    {
        float nearestPos = float.PositiveInfinity;
        DataLink nearest = null;
        foreach (DataLink dataLink in Connections)
        {
            if (dataLink.GetComponent<T>())
            {
                if (nearestPos > Vector3.Distance(transform.position, dataLink.transform.position))
                    nearest = dataLink;
            }
        }

        if (nearest != null)
            Connect(nearest);

        return nearest;
    }

    public List<DataLink> GetAllOf<T>() where T : MonoBehaviour
    {
        List<DataLink> links = new List<DataLink>();
        foreach (DataLink dataLink in Connections) 
        {
            if (dataLink.GetComponent<T>()) 
            {
                links.Add(dataLink);
                Connect(dataLink);
            }
        }
        return links;
    }

    // Simulate a Connection Event
    void Connect(DataLink dataLink)
    {
        StartCoroutine(SimulateLink(dataLink));
    }

    IEnumerator SimulateLink(DataLink dataLink) 
    {
        if (!FXLine[dataLink]) 
        {
            FXLine[dataLink] = true;
            GameObject go = dataLink.gameObject;
            GameObject psObj = Instantiate(DataLinkFX, transform.position, Quaternion.LookRotation(go.transform.position - transform.position));
            var main = psObj.GetComponent<ParticleSystem>().main;
            main.startLifetime = (go.transform.position - transform.position).magnitude / main.startSpeed.constant;
            psObj.GetComponent<ParticleSystem>().Play();
            yield return new WaitForSeconds(1.5f);
            Destroy(psObj);
            FXLine[dataLink] = false;
        }        
    }

    /*
     * 
     * 
     * IEnumerator StartMiningAnim(MineralNode node)
    {
        yield return new WaitForFixedUpdate();
        GameObject go = node.gameObject;
        List<ParticleSystem> ps = new List<ParticleSystem>();
        for (int x = 0; x < miningSockets.Length; x++)
        {   
            GameObject psobj = Instantiate(m_ParticleObject, miningSockets[x].transform.position, Quaternion.LookRotation(go.transform.position - miningSockets[x].transform.position));
            m_Particle.Add(psobj);
            ps.Add(psobj.GetComponent<ParticleSystem>());
        }

        while (true)
        {
            GetComponent<Rigidbody>().rotation = Quaternion.LookRotation(go.transform.position - transform.position);
            for (int x = 0; x < miningSockets.Length; x++)
            {
                var main = ps[x].main;
                m_Particle[x].transform.position = miningSockets[x].transform.position;
                m_Particle[x].transform.rotation = Quaternion.LookRotation(go.transform.position - miningSockets[x].transform.position);
                main.startLifetime = (go.transform.position - miningSockets[x].transform.position).magnitude / main.startSpeed.constant;
                yield return new WaitForFixedUpdate();
            }
        }

    }

    IEnumerator EndMiningAnim()
    {
        //Debug.LogWarning("Ending Anim");
        yield return new WaitForFixedUpdate();
        foreach (GameObject go in m_Particle) 
        {
            //Debug.LogWarning("Destroy Anim");
            //Debug.LogWarning(go);
            Destroy(go);
        }
        yield return new WaitForFixedUpdate();
        m_Particle.Clear();
    }
     * 
     * 
     */



    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}

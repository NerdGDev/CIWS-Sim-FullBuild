using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CIWSDataLink : MonoBehaviour
{
    //Communicators for Other DataLinked Objects    
    public delegate void DataLinkTransmit(CIWSDataLinkPackage dataLinkPackage, ConfirmReceivedDL callback, CIWSDataLink sender, int? target = null);
    public DataLinkTransmit dataLinkTransmit;

    public delegate void DataLinkReceived(int sender, int? target = null);
    public DataLinkReceived dataLinkReceived;

    public delegate void ConfirmReceivedDL(CIWSDataLink receiver);
    public ConfirmReceivedDL confirmReceivedDL;

    [SerializeField]
    public List<CIWSDataLink> connectedDataLinks;

    //Relay to Control Scripts that require the DataLink
    public delegate void TransmitData(CIWSDataLinkPackage dataLinkPackage, int? target = null);
    public TransmitData transmitData;

    public delegate void ReceivedData(CIWSDataLinkPackage dataLinkPackage);
    public ReceivedData receivedData;

    //Visualisers and Debuggables
    public GameObject dataLinkVisualiser;



    private void Start()
    {        
        ConnectDataLinks();
        confirmReceivedDL = ConfirmReceivedDataLink;
        transmitData -= TransmitDataLink;
        transmitData += TransmitDataLink;

        //StartCoroutine(TestTransmit());
    }

    IEnumerator TestTransmit() {
        Debug.Log("Testing Transmit");
        print("Debugging Transmit");
        yield return new WaitForSecondsRealtime(1);
        TransmitDataLink(new CIWSDataLinkPackage());
    }

    private void Update()
    {
        //Debug.Log("Vibe Check");
    }

    private void ConnectDataLinks() {
        connectedDataLinks.ForEach(delegate (CIWSDataLink externalDataLink)
        {
            Debug.Log("Connecting");
            externalDataLink.dataLinkTransmit -= ReceiveDataLink;
            externalDataLink.dataLinkTransmit += ReceiveDataLink;
        });
    }

    private void DisconnectDataLinks()
    {

    }

    //Handles all Received Data Packages from Other DataLinks
    private void ReceiveDataLink(CIWSDataLinkPackage dataLinkPackage, ConfirmReceivedDL callback, CIWSDataLink sender, int? target = null) {
        Debug.Log(target.ToString() + " : " + GetInstanceID().ToString() + " : " + gameObject.GetInstanceID().ToString());
        if (target == null || target == gameObject.GetInstanceID()) {
            //Debug.Log("Received Data at " + GetInstanceID().ToString());
            receivedData(dataLinkPackage);
            callback(this);
        }
    }

    private void ConfirmReceivedDataLink(CIWSDataLink receiver) {
        //Debug.Log("Confirmed Receieved to " + GetInstanceID().ToString());
        GameObject visualiser = Instantiate(dataLinkVisualiser, transform.position + new Vector3(0,1,0), transform.rotation);
        visualiser.GetComponent<DataLinkVisualiser>().target = receiver.transform.position + new Vector3(0, 1, 0);
    }

    //Handles sending of Data Packages to all connected DataLinks (Setting a Target will mark the package with a single intended receiver and tell others to ignore in incoming package)
    public void TransmitDataLink(CIWSDataLinkPackage dataLinkPackage, int? target = null) {
        //Debug.Log("Sending Data from " + GetInstanceID().ToString());
        dataLinkTransmit(dataLinkPackage, confirmReceivedDL, this, target);
    }


}

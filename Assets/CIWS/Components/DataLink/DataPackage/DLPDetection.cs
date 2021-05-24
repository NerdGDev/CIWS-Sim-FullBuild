using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DLPDetection : CIWSDataLinkPackage
{
    int radarID;
    SearchRadarController sendingController;

    int signatureID;
    Vector3 position;

    public DLPDetection(int radarID, SearchRadarController sendingController, int signatureID, Vector3 position) : base(PackageContent.DETECTION)
    {
        this.radarID = radarID;
        this.sendingController = sendingController;

        this.signatureID = signatureID;
        this.position = position;
    }

    public (int radarID, SearchRadarController sendingController, int signatureID, Vector3 position) GetPackageData(){
        return (radarID, sendingController, signatureID, position);
    }
}

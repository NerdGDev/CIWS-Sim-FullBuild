using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DLPackageDetection : CIWSDataLinkPackage
{
    int radarID;
    SearchRadarController sendingController;

    int signatureID;
    Vector3 position;

    public DLPackageDetection(int radarID, SearchRadarController sendingController, int signatureID, Vector3 position) : base(PackageContent.DETECTION)
    {
        this.radarID = radarID;
        this.sendingController = sendingController;

        this.signatureID = signatureID;
        this.position = position;
    }

    public (int radarId, SearchRadarController sendingController, int signatureID, Vector3 position) getPackageData(){
        return (radarID, sendingController, signatureID, position);
    }
}

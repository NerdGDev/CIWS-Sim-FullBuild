using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DLPKill : CIWSDataLinkPackage
{
    int signatureID;

    public DLPKill(int signatureID) : base(PackageContent.KILL)
    {
        this.signatureID = signatureID;
    }

    public int GetPackageData()
    {
        return signatureID;
    }
}

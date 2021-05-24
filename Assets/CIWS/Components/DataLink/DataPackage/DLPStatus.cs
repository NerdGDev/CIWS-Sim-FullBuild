using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DLPStatus : CIWSDataLinkPackage
{
    FireControlSystem fcs;
    FireControlSystem.Status status;

    public DLPStatus(FireControlSystem fcs, FireControlSystem.Status status) : base(PackageContent.STATUS) 
    {
        this.fcs = fcs;
        this.status = status;
    }

    public (FireControlSystem fcs, FireControlSystem.Status) GetPackageData() {
        return (fcs, status);
    }
}

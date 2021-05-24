using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DLPCommand : CIWSDataLinkPackage
{
    FireControlSystem.Commands command;
    Vector3 position;
    int signatureID;

    public DLPCommand(FireControlSystem.Commands command, Vector3? position = null, int? signatureID = null) : base(PackageContent.COMMAND) 
    {
        this.command = command;
        this.position = (Vector3)position;
        this.signatureID = (int)signatureID;
    }

    public FireControlSystem.Commands GetCommand()
    {
        return command;
    }

    public (FireControlSystem.Commands command, Vector3 position, int signatureID) GetPackageData() 
    {
        return (command, position, signatureID);
    }
}

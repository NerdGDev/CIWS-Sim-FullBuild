using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PackageContent{
    BASIC,
    DETECTION,
    TRACKING,
    INSTRUCTION
}

public class CIWSDataLinkPackage {
    PackageContent type;

    public CIWSDataLinkPackage() {
        this.type = PackageContent.BASIC;
    }

    protected CIWSDataLinkPackage(PackageContent type)
    {
        this.type = type;
    }

    public PackageContent GetType() {
        return this.type;
    }

}
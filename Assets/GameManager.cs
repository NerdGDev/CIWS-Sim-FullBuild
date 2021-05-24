using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    void Start()
    {
        Physics.IgnoreLayerCollision(8, 0);
        Physics.IgnoreLayerCollision(8, 1);
        Physics.IgnoreLayerCollision(8, 2);
        Physics.IgnoreLayerCollision(8, 3);
        Physics.IgnoreLayerCollision(8, 4);
        Physics.IgnoreLayerCollision(8, 5);
        Physics.IgnoreLayerCollision(8, 6);
        Physics.IgnoreLayerCollision(8, 7);
        Physics.IgnoreLayerCollision(8, 8);
        Physics.IgnoreLayerCollision(8, 10);

        Physics.IgnoreLayerCollision(9, 0);
        Physics.IgnoreLayerCollision(9, 1);
        Physics.IgnoreLayerCollision(9, 2);
        Physics.IgnoreLayerCollision(9, 3);
        Physics.IgnoreLayerCollision(9, 4);
        Physics.IgnoreLayerCollision(9, 5);
        Physics.IgnoreLayerCollision(9, 6);
        Physics.IgnoreLayerCollision(9, 7);
        Physics.IgnoreLayerCollision(9, 9);
        Physics.IgnoreLayerCollision(9, 10);
    }

    private void Awake()
    {

    }

    public void LaunchNewMissile(int salvo) 
    {
        MissileLauncher[] missileLaunchers = FindObjectsOfType<MissileLauncher>();
        for (int x = 0; x < salvo; x++) 
        {
            missileLaunchers[Random.Range(0, missileLaunchers.Length)].NewLaunch();
        }
    }
}

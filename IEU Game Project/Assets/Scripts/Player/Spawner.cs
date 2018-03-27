using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Spawner : NetworkBehaviour
{
    [SerializeField]
    private GameObject _character = null;

    [SerializeField]
    private Vector3 _spawnPoint = Vector3.zero;

    // Use this for initialization
    void Start()
    {
        //NetworkServer.SpawnObjects();
        GameObject instance = GameObject.Instantiate(_character);
        NetworkServer.Spawn(instance);
    }
}

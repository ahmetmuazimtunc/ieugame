using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyPlayerNetwork : MonoBehaviour {

    void OnStartLocalPlayer()
    {
        Camera cam = new Camera();
        GameObject.Instantiate(cam, transform);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionHandler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("Test Message from " + gameObject.name);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        // Get gameObject info and send alert
        string square = gameObject.name;
        string piece = other.gameObject.name;
        //Debug.Log(piece + " triggered " + square);
    }
}

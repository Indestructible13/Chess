using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionHandler : MonoBehaviour
{
    private bool attackedByWhite;
    private bool attackedByBlack;

    public bool AttackedByWhite { get => attackedByWhite; set => attackedByWhite = value; }
    public bool AttackedByBlack { get => attackedByBlack; set => attackedByBlack = value; }

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

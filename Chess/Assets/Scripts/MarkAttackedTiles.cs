using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkAttackedTiles : MonoBehaviour
{
    private List<GameObject> listOfTiles = new List<GameObject>(); // List of all 64 tiles

    // Start is called before the first frame update
    void Start()
    {
        // Create a list of all the detection tiles (works because they all have the DetectionHandler script attached)
        DetectionHandler[] arrayOfDetectionHandlers = GameObject.FindObjectsOfType<DetectionHandler>();
        foreach (DetectionHandler dh in arrayOfDetectionHandlers)
        {
            listOfTiles.Add(dh.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MarkTilesAsAttacked()
    {
        // First, go through and unmark ALL tiles
        foreach (GameObject tile in listOfTiles)
        {
            tile.GetComponent<DetectionHandler>().AttackedByWhite = false;
            tile.GetComponent<DetectionHandler>().AttackedByBlack = false;
        }

        // Then, go through and check every piece and mark every tile they attack.
        // For White
        for (int i = 0; i < GameObject.Find("White Pieces").transform.childCount; i++)
        {

        }

        // For Black
        for (int i = 0; i < GameObject.Find("Black Pieces").transform.childCount; i++)
        {

        }

        GameObject.Find("White Pieces").transform.GetChild(0).gameObject.GetComponent<MoveKing>().FindTilesAttacked("King's current tile");
    }
}

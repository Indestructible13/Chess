using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkAttackedTiles : MonoBehaviour
{
    private List<GameObject> allTiles = new List<GameObject>(); // List of all 64 tiles
    private List<GameObject> tilesAttackedByWhite = new List<GameObject>();
    private List<GameObject> tilesAttackedByBlack = new List<GameObject>();

    private List<GameObject> whitePieces = new List<GameObject>(); // List of white pieces currently on the board
    private List<GameObject> blackPieces = new List<GameObject>(); // List of black pieces currently on the board
    private List<GameObject> allPieces = new List<GameObject>(); // List of pieces currently on the board

    // Start is called before the first frame update
    void Start()
    {
        GetAllTiles();
        GetAllPieces();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MarkTilesAsAttacked()
    {
        // First, go through and unmark ALL tiles
        UnmarkTiles();

        // Then, go through and check every piece and mark every tile they attack.
        // For White
        for (int i = 0; i < whitePieces.Count; i++)
        {
            whitePieces[i].GetComponent<MoveKing>().FindTilesAttacked();
            List<GameObject> tilesAttackedByKing = whitePieces[i].GetComponent<MoveKing>().GetListOfTilesAttacked();
            for (int j = 0; j < tilesAttackedByKing.Count; j++)
            {
                tilesAttackedByWhite.Add(tilesAttackedByKing[j]);
            }
        }

        // For Black
        for (int i = 0; i < GameObject.Find("Black Pieces").transform.childCount; i++)
        {
            blackPieces[i].GetComponent<MoveKing>().FindTilesAttacked();
            List<GameObject> tilesAttackedByKing = blackPieces[i].GetComponent<MoveKing>().GetListOfTilesAttacked();
            for (int j = 0; j < tilesAttackedByKing.Count; j++)
            {
                tilesAttackedByBlack.Add(tilesAttackedByKing[j]);
            }
        }

        // Go through list of all tiles. If a tile is also in the list of pieces attacked by white/black, then mark the tile as such.
        foreach (GameObject tile in allTiles)
        {
            if (tilesAttackedByWhite.Contains(tile))
            {
                tile.GetComponent<DetectionHandler>().AttackedByWhite = true;
            }

            if (tilesAttackedByBlack.Contains(tile))
            {
                tile.GetComponent<DetectionHandler>().AttackedByBlack = true;
            }
        }
    }

    private void GetListOfWhitePieces()
    {
        //Debug.Log("List of White Pieces:");
        for (int i = 0; i < GameObject.Find("White Pieces").transform.childCount; i++)
        {
            whitePieces.Add(GameObject.Find("White Pieces").transform.GetChild(i).gameObject);
            //Debug.Log(GameObject.Find("White Pieces").transform.GetChild(i).gameObject.name);
        }
    }

    private void GetListOfBlackPieces()
    {
        //Debug.Log("List of Black Pieces:");
        for (int i = 0; i < GameObject.Find("Black Pieces").transform.childCount; i++)
        {
            whitePieces.Add(GameObject.Find("Black Pieces").transform.GetChild(i).gameObject);
            //Debug.Log(GameObject.Find("Black Pieces").transform.GetChild(i).gameObject.name);
        }
    }

    private void GetAllPieces()
    {
        GetListOfWhitePieces();
        GetListOfBlackPieces();

        foreach (GameObject whitePiece in whitePieces)
        {
            allPieces.Add(whitePiece);
        }

        foreach (GameObject blackPiece in blackPieces)
        {
            allPieces.Add(blackPiece);
        }

        int i = 0;
        foreach (GameObject piece in allPieces)
        {
            i++;
            Debug.Log("Piece #" + i + " is " + piece.name);
        }
    }

    private void GetAllTiles()
    {
        // Create a list of all the detection tiles (works because they all have the DetectionHandler script attached)
        DetectionHandler[] arrayOfDetectionHandlers = GameObject.FindObjectsOfType<DetectionHandler>();
        foreach (DetectionHandler dh in arrayOfDetectionHandlers)
        {
            allTiles.Add(dh.gameObject);
        }
    }

    private void UnmarkTiles()
    {
        foreach (GameObject tile in allTiles)
        {
            tile.GetComponent<DetectionHandler>().AttackedByWhite = false;
            tile.GetComponent<DetectionHandler>().AttackedByBlack = false;
        }
    }
}

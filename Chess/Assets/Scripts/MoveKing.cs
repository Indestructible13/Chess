using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveKing : MonoBehaviour
{
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private bool whitePiece; // True if white king, false if black king
    [SerializeField] private GameObject currentTile; // Tile the king is currently on
    private GameObject futureTile; // Updates while dragging the king

    // Used to move the piece while dragging
    private bool dragging = false;
    private float distance;
    private bool moveDone = false;

    private List<GameObject> listOfTiles = new List<GameObject>(); // List of all 64 tiles
    private List<GameObject> tilesAttacked = new List<GameObject>(); // List of tiles the king is attacking
    private List<GameObject> validMoves = new List<GameObject>(); // List of tiles the king can move to (i.e. safe tiles that will not result in check)

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
        // Keep track of the tile the piece is hovering over when dragging
        Collider[] overlaps = Physics.OverlapSphere(gameObject.transform.position, 0.0f, layerMask);
        if (overlaps.Length > 0)
        {
            futureTile = overlaps[0].gameObject;
        } else
        {
            GameObject board = GameObject.Find("Board");
            futureTile = board.transform.GetChild(3).gameObject;
        }

        // Moves the piece while dragging
        if (dragging)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Vector3 rayPoint = ray.GetPoint(distance);
            rayPoint.y = GameObject.Find("Board").transform.position.y + 2;
            transform.position = rayPoint;
        }

        // if no longer dragging, and the move is valid, then move the piece to the center of the tile
        if (!dragging && !moveDone && CheckValidMove())
        {
            gameObject.transform.position = futureTile.transform.position;
            currentTile = futureTile;
            moveDone = true; // Essential to make sure this bracket only runs once

            // After the king has moved, update the tilesAttacked list.
            tilesAttacked.Clear();
            FindTilesAttacked();

            Debug.Log(gameObject.name + " moved to " + futureTile.name);
        }  
        else if (!dragging && !moveDone) // CheckValidMove() failed (i.e. invalid move) Move the piece back where it came from!
        {
            gameObject.transform.position = currentTile.transform.position;
            moveDone = true;
            Debug.Log(futureTile.name + " is an invalid move for " + gameObject.name + "! Try again!");
        }
    }

    private void OnMouseEnter()
    {
        
    }

    private void OnMouseExit()
    {
        
    }

    private void OnMouseDown()
    {
        distance = Vector3.Distance(transform.position, Camera.main.transform.position);
        dragging = true;
        moveDone = false;
    }

    private void OnMouseUp()
    {
        dragging = false;
    }

    //TODO
    private bool CheckValidMove()
    {
        FindTilesAttacked();

        // Check all the tiles the king can attack. If the tile is safe, then it is a valid move for the king.
        // Necessary because the king cannot walk into a check.
        foreach (GameObject tile in tilesAttacked)
        {
            if (CheckSafeSquare(tile))
            {
                validMoves.Add(tile);
            }
        }

        // Check if the attempted move is in the list of valid moves the king has.
        foreach (GameObject tile in validMoves)
        {
            if (futureTile == tile)
            {
                return true;
            }
        }
        return false;
    }

    // TODO
    // Checks the position to see if it is safe for the king to move there.
    private bool CheckSafeSquare(GameObject tile)
    {
        if (whitePiece)
        {
            if (tile.GetComponent<DetectionHandler>().AttackedByBlack)
            {
                return false;
            }
        } 
        else
        {
            if (tile.GetComponent<DetectionHandler>().AttackedByWhite)
            {
                return false;
            }
        }
        return true;
    }

    // Returns true if the point is inside the collider
    private bool IsInside(BoxCollider c, Vector3 point)
    {
        Vector3 closest = c.ClosestPoint(point);
        // Because closest=point if point is inside - not clear from docs I feel
        return closest == point;
    }

    // Based on the given tile, return a list of all tiles that the king can attack.
    public void FindTilesAttacked()
    {
        // Get the center of the tile and draw a ray 1 unit in each direction (sqrt 2 units for diagonals)
        // This is because the king can move 1 tile in each direction
        Vector3 centerOfTile = currentTile.transform.position;

        // Add the endpoints of the 8 rays to this list
        List<Vector3> rayEndpoints = new List<Vector3>();

        // Check 4 straights
        Ray r1 = new Ray(centerOfTile, Vector3.forward);
        rayEndpoints.Add(r1.GetPoint(1));

        Ray r2 = new Ray(centerOfTile, Vector3.back);
        rayEndpoints.Add(r2.GetPoint(1));

        Ray r3 = new Ray(centerOfTile, Vector3.right);
        rayEndpoints.Add(r3.GetPoint(1));

        Ray r4 = new Ray(centerOfTile, Vector3.left);
        rayEndpoints.Add(r4.GetPoint(1));

        // Check 4 diagonals
        Ray r5 = new Ray(centerOfTile, Vector3.forward + Vector3.right);
        rayEndpoints.Add(r5.GetPoint(Mathf.Sqrt(2)));

        Ray r6 = new Ray(centerOfTile, Vector3.forward + Vector3.left);
        rayEndpoints.Add(r6.GetPoint(Mathf.Sqrt(2)));

        Ray r7 = new Ray(centerOfTile, Vector3.back + Vector3.right);
        rayEndpoints.Add(r7.GetPoint(Mathf.Sqrt(2)));

        Ray r8 = new Ray(centerOfTile, Vector3.back + Vector3.left);
        rayEndpoints.Add(r8.GetPoint(Mathf.Sqrt(2)));

        // Checks all endpoints to see if they are contained in any tiles. If so, add the tile to list of tilesAttacked.
        foreach (Vector3 endpoint in rayEndpoints)
        {
            foreach (GameObject tile in listOfTiles)
            {
                if (IsInside(tile.GetComponent<BoxCollider>(), endpoint))
                {
                    tilesAttacked.Add(tile);
                }
            }
        }
    }

    public List<GameObject> GetListOfTilesAttacked()
    {
        return tilesAttacked;
    }
}
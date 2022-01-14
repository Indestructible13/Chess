using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveKing : MonoBehaviour
{
    public GameObject currentTile;
    [SerializeField] private LayerMask layerMask;

    private GameObject futureTile;
    private bool dragging = false;
    private float distance;
    private bool moveDone = false;
    private List<GameObject> listOfDetectionTiles = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        // Create a list of all the detection tiles (works because they all have the DetectionHandler script attached)
        DetectionHandler[] arrayOfDetectionHandlers = GameObject.FindObjectsOfType<DetectionHandler>();
        foreach (DetectionHandler dh in arrayOfDetectionHandlers)
        {
            listOfDetectionTiles.Add(dh.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Keep track of the current tile the piece is hovering over when moving
        Collider[] overlaps = Physics.OverlapSphere(gameObject.transform.position, 0.0f, layerMask);
        if (overlaps.Length > 0)
        {
            futureTile = overlaps[0].gameObject;
        } else
        {
            //currentTile = GameObject.Find("OffTheBoard");
            GameObject board = GameObject.Find("Board");
            futureTile = board.transform.GetChild(3).gameObject;
        }

        if (dragging)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Vector3 rayPoint = ray.GetPoint(distance);
            rayPoint.y = 2;
            transform.position = rayPoint;
        }

        // if no longer dragging, and the move is valid, then move the piece to the center of the square
        if (!dragging && !moveDone && CheckValidMove())
        {
            gameObject.transform.position = futureTile.transform.position;
            currentTile = futureTile;
            moveDone = true; // Essential to make sure this only runs once
            Debug.Log(gameObject.name + " moved to " + futureTile.name);
        }  
        else if (!dragging && !moveDone) // CheckValidMove() failed (i.e. invalid move)
        {
            gameObject.transform.position = currentTile.transform.position;
            moveDone = true;
            Debug.Log(futureTile.name + " is an invalid move for " + gameObject.name + "! Try again!");
        }
    }

    private void OnMouseEnter()
    {
        //GetComponent<Renderer>().material.color = mouseOverColor;
    }

    private void OnMouseExit()
    {
        //GetComponent<Renderer>().material.color = originalColor;
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
        // Make a list of valid moves based on the oldSquare
        List<GameObject> validTiles = new List<GameObject>();

        // Get the center of the tile and draw a ray 1 unit in each direction (including diagonals)
        // This is because the king can move 1 tile in each direction (For diagonals maybe use pythagorean thorem to draw longer rays?)
        Vector3 centerOfOldSquare = currentTile.transform.position;

        // Add the endpoints of the 8 rays to this list
        List<Vector3> rayEndpoints = new List<Vector3>();

        // Check 4 straights
        Ray r1 = new Ray(centerOfOldSquare, Vector3.forward);
        rayEndpoints.Add(r1.GetPoint(1));

        Ray r2 = new Ray(centerOfOldSquare, Vector3.back);
        rayEndpoints.Add(r2.GetPoint(1));

        Ray r3 = new Ray(centerOfOldSquare, Vector3.right);
        rayEndpoints.Add(r3.GetPoint(1));

        Ray r4 = new Ray(centerOfOldSquare, Vector3.left);
        rayEndpoints.Add(r4.GetPoint(1));

        // Check 4 diagonals
        Ray r5 = new Ray(centerOfOldSquare, Vector3.forward + Vector3.right);
        rayEndpoints.Add(r5.GetPoint(Mathf.Sqrt(2)));

        Ray r6 = new Ray(centerOfOldSquare, Vector3.forward + Vector3.left);
        rayEndpoints.Add(r6.GetPoint(Mathf.Sqrt(2)));

        Ray r7 = new Ray(centerOfOldSquare, Vector3.back + Vector3.right);
        rayEndpoints.Add(r7.GetPoint(Mathf.Sqrt(2)));

        Ray r8 = new Ray(centerOfOldSquare, Vector3.back + Vector3.left);
        rayEndpoints.Add(r8.GetPoint(Mathf.Sqrt(2)));

        foreach (Vector3 endpoint in rayEndpoints)
        {
            foreach (GameObject detectionTile in listOfDetectionTiles)
            {
                if (IsInside(detectionTile.GetComponent<BoxCollider>(), endpoint))
                {
                    // Prevents adding unsafe tiles (e.g. tiles in check) to the list
                    if (CheckSafeSquare(detectionTile))
                    {
                        validTiles.Add(detectionTile);
                    }
                }
            }
        }
       
        // Check if the attempted move is in the list of valid moves.
        foreach(GameObject tile in validTiles)
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
        return true;
    }

    // Returns true if the point is inside the collider
    private bool IsInside(BoxCollider c, Vector3 point)
    {
        Vector3 closest = c.ClosestPoint(point);
        // Because closest=point if point is inside - not clear from docs I feel
        return closest == point;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAnywhere : MonoBehaviour
{
    //private Color mouseOverColor = Color.blue;
    //private Color originalColor = Color.yellow;
    private bool dragging = false;
    private float distance;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (dragging)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Vector3 rayPoint = ray.GetPoint(distance);
            rayPoint.y = 2;
            transform.position = rayPoint;
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
    }

    private void OnMouseUp()
    {
        dragging = false;
    }
}
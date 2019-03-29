using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Author: Sherry Fan
public class CECameraGaze : MonoBehaviour
{
    public float sightlength = 100.0f;
    public GameObject targetObj;

    private GameObject targetHolder;
    private LineRenderer laserLine;

    public float laserWidth = 0.1f;
    public float laserMaxLength = 5f;

    public bool entered, exited;
    void Start()
    {
        // laserLine = gameObject.AddComponent<LineRenderer>();
        // Vector3[] initLaserPositions = new Vector3[ 2 ] { Vector3.zero, Vector3.zero };
        // laserLine.SetPositions(initLaserPositions);
        // laserLine.SetWidth(laserWidth, laserWidth);

    }
    void FixedUpdate()
    {
        RaycastHit seen;
        Ray raydirection = new Ray(transform.position, transform.forward);
        Debug.DrawRay(transform.position, transform.forward, Color.green);

        if (Physics.Raycast(raydirection, out seen, sightlength))
        {

            if (seen.collider.gameObject == targetObj)
            {
                if (!entered)
                {
                    targetHolder = seen.collider.gameObject;
                    targetHolder.GetComponent<CEKeyObject>().OnGazeEnter();
                    entered = true;
                    exited = false;

                }
            }
            else
            {
                if (targetHolder)
                {
                    if (!exited)
                    {
                        targetHolder.GetComponent<CEKeyObject>().OnGazeExit();
                        exited = true;
                        entered = false;
                    }
                }
            }

        }
        else
        {
            if (targetHolder)
            {
                if (!exited)
                {
                    targetHolder.GetComponent<CEKeyObject>().OnGazeExit();
                    exited = true;
                    entered = false;

                }
            }
        }
    }
}


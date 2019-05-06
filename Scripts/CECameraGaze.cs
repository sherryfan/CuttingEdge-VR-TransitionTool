using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Author: Sherry Fan
public class CECameraGaze : MonoBehaviour
{
    public bool EnableRaycast = true;
    public float sightlength = 100.0f;
    public GameObject targetObj;

    private GameObject targetHolder;
    private LineRenderer laserLine;

    public float laserWidth = 0.1f;
    public float laserMaxLength = 5f;
    public bool searchAncestors = false;
    public bool entered, exited;
    void Start()
    {

    }
    void FixedUpdate()
    {
        if (targetObj == null)
            return;
        if (!EnableRaycast) {
            if (!exited && targetHolder) {
                targetHolder.GetComponent<CEInteractable>().OnGazeExit();
                exited = true;
                entered = false;
            }
            return;
        }
        RaycastHit seen;
        Ray raydirection = new Ray(transform.position, transform.forward);
        Debug.DrawRay(transform.position, transform.forward, Color.green);

        if (Physics.Raycast(raydirection, out seen, sightlength))
        {
            GameObject seenKeyObject = null, tmpObj = seen.collider.gameObject;
            if (searchAncestors) {
                while (true) {
                    if (tmpObj.GetComponent<CEInteractable>()) {
                        seenKeyObject = tmpObj;
                        break;
                    }
                    if (tmpObj.transform.parent != null) {
                        tmpObj = tmpObj.transform.parent.gameObject;
                    }
                    else {
                        break;
                    }
                }
            }
            else {
                seenKeyObject = seen.collider.gameObject;
            }
            if (seenKeyObject == targetObj)
            {
                if (!entered)
                {
                    targetHolder = seenKeyObject;
                    targetHolder.GetComponent<CEInteractable>().OnGazeEnter();
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
                        targetHolder.GetComponent<CEInteractable>().OnGazeExit();
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
                    targetHolder.GetComponent<CEInteractable>().OnGazeExit();
                    exited = true;
                    entered = false;

                }
            }
        }
    }
}


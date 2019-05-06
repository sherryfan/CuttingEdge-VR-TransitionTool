using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Author: Sherry Fan
// This script should be attached with every object you will be interacte with
// in the expereinece. Interactions currently including: gaze and grab.
// [RequireComponent(typeof(OVRGrabbable))]
public class CEInteractable : MonoBehaviour
{
    [SerializeField]
    UnityEvent gazeEnter, gazeExit, gazeStay;
    [SerializeField]
    private float minGazeTime;
    private OVRGrabbable oGrabbable;

    private Material outline;
    public bool isGazing, isHolding;
    public float gazeDuration;
    // Start is called before the first frame update
    void Start()
    {
        try {
            oGrabbable = GetComponent<OVRGrabbable>();
            outline = GetComponent<MeshRenderer>().materials[1];
        }
        catch (System.Exception) {}
        
        gazeDuration = 0f;
    }

    // Update is called once per frame
    void Update()
    {

        if (oGrabbable != null && oGrabbable.isGrabbed)
        {
            if (!isHolding)
            {
                OnGrabEnter();
            }
            if (isGazing)
            {
                gazeDuration += Time.deltaTime;
            }
        }
        if (isGazing) {
            OnGazeStay();
        }
    }

    public virtual void OnGazeEnter()
    {
        if (outline != null)
            outline.SetFloat("_Thickness", 0.2f);
        gazeDuration = 0f;
        isGazing = true;
        print("OnGazeEnter " + gameObject.name);
        gazeEnter.Invoke();
    }
    public void OnGazeStay() {
        gazeStay.Invoke();
    }

    public virtual void OnGazeExit()
    {
        if (outline != null)
            outline.SetFloat("_Thickness", 0f);
        gazeExit.Invoke();
        isGazing = false;
        //print("Gaze Exit with gaze time " + gazeDuration);

        if (gazeDuration > minGazeTime)
        { // it was a qualified gaze
            LookawayFromGaze();
        }
    }

    public void LookawayFromGaze()
    {
        if ((oGrabbable != null && oGrabbable.isGrabbed))
        {
            print("look away triggered");
            StartCoroutine(CEAttentionTrigger.instance.OnCutTriggered(true));
        }
    }

    public void OnGrabEnter()
    {
        print("OnGrabEnter");

        isHolding = true;
        StartCoroutine(CEAttentionTrigger.instance.OnCutTriggered(false));
    }

}

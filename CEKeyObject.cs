using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Author: Sherry Fan

[RequireComponent(typeof(OVRGrabbable))]
public class CEKeyObject : MonoBehaviour
{

    [SerializeField]
    private float minGazeTime;
    private VIVEGrabbable vGrabbable;
    private OVRGrabbable oGrabbable;

    private Material outline;
    public bool isGazing, isHolding;
    public float gazeDuration;
    // Start is called before the first frame update
    void Start()
    {
        if (CEUtilities.Singleton.isVIVE)
        {
            vGrabbable = GetComponent<VIVEGrabbable>();
        }
        else
        {
            oGrabbable = GetComponent<OVRGrabbable>();

        }
        outline = GetComponent<MeshRenderer>().materials[1];
        gazeDuration = 0f;
    }

    // Update is called once per frame
    void Update()
    {

        if ((oGrabbable != null && oGrabbable.isGrabbed)
            || (vGrabbable != null && vGrabbable.isGrabbed))
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
    }

    public void OnGazeEnter()
    {
        outline.SetFloat("_Thickness", 0.2f);
        gazeDuration = 0f;
        isGazing = true;
        print("OnGazeEnter");

    }

    public void OnGazeExit()
    {
        outline.SetFloat("_Thickness", 0f);
        isGazing = false;
        //print("Gaze Exit with gaze time " + gazeDuration);

        if (gazeDuration > minGazeTime)
        { // it was a qualified gaze
            LookawayFromGaze();
        }
    }

    public void LookawayFromGaze()
    {
        if ((oGrabbable != null && oGrabbable.isGrabbed)
            || (vGrabbable != null && vGrabbable.isGrabbed))
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

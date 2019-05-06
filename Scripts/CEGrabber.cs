using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CEGrabber : MonoBehaviour
{

    // Grip trigger thresholds for picking up objects, with some hysteresis.
    public float grabBegin = 0.55f;
    public float grabEnd = 0.35f;
    protected Quaternion m_anchorOffsetRotation;
    protected Vector3 m_anchorOffsetPosition;

    private GameObject m_grabbedObj;
    public CEGrabbable grabbable;
    private GameObject touchedObj;
    public bool isPressed;


    protected float m_prevFlex;
    [SerializeField]
    protected OVRInput.Controller m_controller;

    private Transform previousParent;

    private void Awake() {
        m_anchorOffsetPosition = transform.localPosition;
        m_anchorOffsetRotation = transform.localRotation;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Holding object
        OVRInput.Update();

        // Update values from inputs
        float prevFlex1 = OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, m_controller);
        float prevFlex2 = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, m_controller);
        m_prevFlex = Mathf.Max(prevFlex1, prevFlex2);
        CheckForGrabOrRelease(m_prevFlex);
    }
    private void OnTriggerEnter(Collider other)
    {

    }

    void OnTriggerStay(Collider otherCollider)
    {
        if (m_grabbedObj == null)
        {
            grabbable = otherCollider.gameObject.GetComponent<CEGrabbable>();
            if (isPressed && grabbable != null && m_grabbedObj == null)
            {
                touchedObj = otherCollider.gameObject;
                grabbable.isGrabbed = true;
                PickUp();
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        //print("hand exit from " + other.name);
        touchedObj = null;
    }


    void PickUp()
    {
    
        // Grab the object touched
        m_grabbedObj = touchedObj;
        m_grabbedObj.GetComponent<Rigidbody>().useGravity = false;
        m_grabbedObj.transform.position = transform.position;
        m_grabbedObj.transform.rotation = transform.rotation;

        // Add fixed joint between grabber and grabbable
        var fj = gameObject.AddComponent<FixedJoint>();
        fj.connectedBody = m_grabbedObj.GetComponent<Rigidbody>();
        fj.enablePreprocessing = false;

    }

    public void Drop()
    {
        // Destroy the joint between
        var fixJoint = gameObject.GetComponent<FixedJoint>();
        Destroy(fixJoint);
        OVRPose localPose = new OVRPose { position = OVRInput.GetLocalControllerPosition(m_controller), orientation = OVRInput.GetLocalControllerRotation(m_controller) };
        OVRPose offsetPose = new OVRPose { position = m_anchorOffsetPosition, orientation = m_anchorOffsetRotation };
        localPose = localPose * offsetPose;

        OVRPose trackingSpace = transform.ToOVRPose() * localPose.Inverse();
        Vector3 linearVelocity = trackingSpace.orientation * OVRInput.GetLocalControllerVelocity(m_controller);
        Vector3 angularVelocity = trackingSpace.orientation * OVRInput.GetLocalControllerAngularVelocity(m_controller);

        m_grabbedObj = null;
        grabbable.Drop(linearVelocity, angularVelocity);
        grabbable = null;

    }


    private void CheckForGrabOrRelease(float prevFlex)
    {
        if (m_prevFlex >= grabBegin)
        {
            isPressed = true;
        }
        else
        {
            isPressed = false;
            if (m_grabbedObj != null)
            { // Drop the grabbedObj in hand
                Drop();
            }
        }
    }
}

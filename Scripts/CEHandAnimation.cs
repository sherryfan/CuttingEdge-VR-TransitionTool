using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CEHandAnimation : MonoBehaviour
{
    [SerializeField]
    protected OVRInput.Controller m_controller;
    Animator m_Animator;
    // Start is called before the first frame update
    void Start()
    {
        m_Animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        //Holding object
        OVRInput.Update();

        // Update values from inputs
        float prevFlex1 = OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, m_controller);
        float prevFlex2 = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, m_controller);
        var prevFlex = Mathf.Max(prevFlex1, prevFlex2);
        m_Animator.Play("Fist", 0, prevFlex);
    }
}

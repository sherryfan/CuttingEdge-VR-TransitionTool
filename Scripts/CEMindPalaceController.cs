using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CEMindPalaceController : MonoBehaviour
{
    [SerializeField]
    List<CECameraSlave> Slaves;
    [SerializeField]
    GameObject CameraRig, CameraBoth, CameraL, CameraR;
    CECameraBrain brain;
    [SerializeField]
    bool syncRigToMainDisplayCamera = true;
    // Start is called before the first frame update
    void Start()
    {
        // foreach (var slave in Slaves) {
        //     slave.CameraRig.transform.localPosition = CameraRig.transform.localPosition;
        //     slave.CameraRig.transform.localRotation = CameraRig.transform.localRotation;
        // }
        brain = GetComponentInChildren<CECameraBrain>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        foreach (var slave in Slaves) {
            slave.CameraBoth.transform.localPosition = CameraBoth.transform.localPosition;
            slave.CameraBoth.transform.localRotation = CameraBoth.transform.localRotation;
            if (slave.CameraL != null) {
                slave.CameraL.transform.localPosition = CameraL.transform.localPosition;
                slave.CameraL.transform.localRotation = CameraL.transform.localRotation;
            }
            if (slave.CameraR != null) {
                slave.CameraR.transform.localPosition = CameraR.transform.localPosition;
                slave.CameraR.transform.localRotation = CameraR.transform.localRotation;
            }
        }
        if (syncRigToMainDisplayCamera && brain != null) {
            Camera targetCam = brain.GetMainDisplayCamera();
            var targetRig = targetCam.transform.parent;
            var targetSlave = targetRig.parent;
            transform.position = targetSlave.position;
            transform.rotation = targetSlave.rotation;
            CameraRig.transform.localPosition = targetRig.localPosition;
            CameraRig.transform.localRotation = targetRig.localRotation;
        }
    }
}

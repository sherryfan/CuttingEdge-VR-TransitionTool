using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CEMindPalaceController : MonoBehaviour
{
    [SerializeField]
    List<CECameraSlave> Slaves;
    [SerializeField]
    GameObject CameraRig, CameraBoth, CameraL, CameraR;
    // Start is called before the first frame update
    void Start()
    {
        foreach (var slave in Slaves) {
            slave.CameraRig.transform.localPosition = CameraRig.transform.localPosition;
            slave.CameraRig.transform.localRotation = CameraRig.transform.localRotation;
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        foreach (var slave in Slaves) {
            slave.CameraBoth.transform.localPosition = CameraBoth.transform.localPosition;
            slave.CameraBoth.transform.localRotation = CameraBoth.transform.localRotation;
            slave.CameraL.transform.localPosition = CameraL.transform.localPosition;
            slave.CameraL.transform.localRotation = CameraL.transform.localRotation;
            slave.CameraR.transform.localPosition = CameraR.transform.localPosition;
            slave.CameraR.transform.localRotation = CameraR.transform.localRotation;
        }
    }
}

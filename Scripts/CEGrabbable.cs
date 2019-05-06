using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CEGrabbable : MonoBehaviour
{
    public bool isGrabbed;
    Vector3 startPos;
    Quaternion startRot; 
    private void Awake() {
        startPos = transform.position;
        startRot = transform.rotation;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B)) {
            transform.position = startPos;
            transform.rotation = startRot;
            Rigidbody rb = gameObject.GetComponent<Rigidbody>();
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }


    public void Drop()
    {
        Drop(Vector3.zero, Vector3.zero);
        
    }
    public void Drop(Vector3 linearVelocity, Vector3 angularVelocity) {
        isGrabbed = false;
        // Check if stuck (under surface)
        if (!CheckStuck())
        {   // Not stuck, let it fall
            Rigidbody rb = gameObject.GetComponent<Rigidbody>();
            rb.useGravity = true;
            rb.velocity = linearVelocity;
            rb.angularVelocity = angularVelocity;
        }

        // Handled by reset
        
    }
    bool CheckStuck()
    {

        RaycastHit hit;
        Vector3 rayOrigin = transform.position + new Vector3(0, 100f, 0);
        int layermask = ~(1 << (LayerMask.NameToLayer("HandCollider")));
        if (Physics.Raycast(rayOrigin, Vector3.down, out hit, 200f, layermask))
        {
            if (hit.collider.gameObject.GetComponent<CEGrabbable>() == null)
            {
                print("hit " + hit.collider.name);
                if ((rayOrigin.y - hit.transform.position.y) < 100f)
                
                    Debug.Log("<color=yellow>Check Stuck: object under non-grabbable object. Start Reset.. </color>");
                    GetComponent<Rigidbody>().isKinematic = true;
                    StartCoroutine(Reset(hit.point));
                    return true;
                
            }
        }
        return false;
    }

    IEnumerator Reset(Vector3 hitPoint)
    {
        // reset the transform
        transform.rotation = Quaternion.identity;
        transform.position = hitPoint + new Vector3(0, 0.1f, 0);
        yield return new WaitForEndOfFrame();
        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<Rigidbody>().useGravity = true;

    }

}

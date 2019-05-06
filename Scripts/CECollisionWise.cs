using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CECollisionWise : MonoBehaviour
{
    public AK.Wwise.Event collisionSFX;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnCollisionEnter(Collision other) {
        collisionSFX.Post(gameObject);
    }

    private void OnTriggerEnter(Collider other) {
        collisionSFX.Post(gameObject);
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}

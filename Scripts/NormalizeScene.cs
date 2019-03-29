using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Sherry Fan
public class NormalizeScene : MonoBehaviour
{
    [SerializeField]
    GameObject Head;
    private void Awake()
    {
        StartCoroutine(Calibrate());
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            StartCoroutine(Calibrate());
        }
    }

    IEnumerator Calibrate()
    {
        yield return new WaitForEndOfFrame();
        Vector3 newDir = new Vector3(Head.transform.forward.x, 0, Head.transform.forward.z);
        this.transform.rotation = Quaternion.LookRotation(newDir);

    }
}

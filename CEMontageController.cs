using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Author: Sherry Fan
[RequireComponent(typeof(CESceneHelper))]
public class CEMontageController : MonoBehaviour
{
    static public CEMontageController Singleton;

    [SerializeField]
    Vector3 MovementVec = new Vector3(0, 0, 4f);
    [SerializeField]
    GameObject CamRig, Head, EndPoint;
    [SerializeField]
    float StandToSeat = 0.5f;
    [SerializeField]
    string NextScene = "GS4Room";
    [SerializeField]
    CESceneHelper sceneHelper;
    private void Awake()
    {
        Singleton = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        // prepare the next scene at start
        sceneHelper = GetComponent<CESceneHelper>();
        sceneHelper.PrepareScene(NextScene);
    }

    public void LoadScene()
    {
        sceneHelper.LoadScene();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(0);
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            MovementVec = new Vector3(0, 0, 0f);
        }


        var tPos = CamRig.transform.position;
        tPos += MovementVec * Time.deltaTime;
        CamRig.transform.position = tPos;
        Vector3 CamToEnd = EndPoint.transform.position - Head.transform.position;

        float d = Vector3.Dot(CamToEnd, MovementVec);
        if (d < 0)
        {
            // pass the end point           
            LoadScene();
        }
    }
}
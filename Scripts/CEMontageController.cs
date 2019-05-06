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
    Vector3 CarVelocity = new Vector3(0, 0, 4f);
    float elapse = 0f;
    [SerializeField]
    float CutAt = 10f;
    [SerializeField]
    float StandToSeat = 0.5f;
    [SerializeField]
    string NextScene = "GS4Room";
    // [SerializeField]
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

       var tPos = CEUtilities.Singleton.GetCameraRig().transform.position;
            tPos += CarVelocity * Time.deltaTime;
            CEUtilities.Singleton.GetCameraRig().transform.position = tPos;
            elapse += Time.deltaTime;
            if (elapse > CutAt)
            {
                sceneHelper.LoadScene();
            }
    }
}
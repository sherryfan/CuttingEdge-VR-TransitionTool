using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class CESceneManager : MonoBehaviour
{
    public static CESceneManager Singleton;
    [SerializeField]
    string defaultScene;
    [SerializeField]
    List<string> sceneList;
    void Awake()
    {
        //Check if instance already exists
        if (Singleton == null)

            //if not, set instance to this
            Singleton = this;

        //If instance already exists and it's not this:
        else if (Singleton != this)

            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);

        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);

    }
    void Start()
    {
        SceneManager.LoadScene(defaultScene);
    }
    void Update()
    {
        if (Input.GetKey(KeyCode.S))
        {
            for (int i = 0; i < sceneList.Count; i++)
            {
                KeyCode t = (KeyCode)System.Enum.Parse(typeof(KeyCode), "Alpha" + (i + 1).ToString());
                if (Input.GetKeyDown(t))
                {
                    if (i == 3)
                    {
                        CESoundManager.Singleton.StopAmbience();
                    }
                    SceneManager.LoadScene(sceneList[i]);
                }
            }
        }
    }
}

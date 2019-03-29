using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CESceneHelper : MonoBehaviour
{
    AsyncOperation asyncOperation;
    public void PrepareScene(string name) {
        asyncOperation = SceneManager.LoadSceneAsync(name);
        asyncOperation.allowSceneActivation = false;
    }
    public bool LoadScene() {
        if (asyncOperation == null) {
            return false;
        }
        asyncOperation.allowSceneActivation = true;
        return true;
    }
}

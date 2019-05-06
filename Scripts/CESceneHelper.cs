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
    public static bool IsPAScene() {
        string t = SceneManager.GetActiveScene().name;
        try {
            string[] s = t.Split('_');
            if (s.Length > 1) {
                string tmp = s[0];
                if (tmp[tmp.Length - 1] == 'a') {
                    return true;
                }
            }
        }
        catch (System.Exception) {}
        return false;
    }
    private void Update() {
        if (Input.GetKeyDown(KeyCode.R)) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        if (Input.GetKeyDown(KeyCode.L)) {
            LoadScene();
        }
    }
}

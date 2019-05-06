using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Camera))]
public class CEWhiteFrame : MonoBehaviour
{
    [SerializeField]
    Material whiteFrameMat;
    [SerializeField]
    public Color col = Color.white;
    [SerializeField]
    public List<Color> randomColors;

    // Update is called once per frame
    private void Awake() {
        whiteFrameMat = new Material(whiteFrameMat);
    }
    void Update()
    {
        whiteFrameMat.SetColor("_Color", col);
    }
    private void OnRenderImage(RenderTexture src, RenderTexture dest) {
        Graphics.Blit(src, dest, whiteFrameMat);
    }
}

using UnityEngine;
using System.Collections;

public class MyRenderTexture : MonoBehaviour {

    public bool grab;
    public Renderer display;
    void OnPostRender() {
        if (grab) {
            Texture2D tex = new Texture2D(128, 128);
            tex.ReadPixels(new Rect(0, 0, 128, 128), 0, 0);
            tex.Apply();
            display.material.mainTexture = tex;
            grab = false;
        }
    }
}

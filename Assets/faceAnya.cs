using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceAnya : MonoBehaviour
{
    public Material matEyes;
    public Material matMouth;

    public string shaderPropertyName = "_UVOffset";

    [Range(0, 4)]
    public int eyes;

    [Range(0, 4)]
    public int mouths;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (eyes >= 0 && eyes < 4)
        {
            Vector4 uvOffset = new Vector4(-0f, -0.25f * eyes, 1f, 1f);
            matEyes.SetVector(shaderPropertyName, uvOffset);
        }
        if (eyes >= 4)
        {
            Vector4 uvOffset = new Vector4(-0.5f, -0.25f * (eyes - 4f), 1f, 1f);
            matEyes.SetVector(shaderPropertyName, uvOffset);
        }

        if (mouths >= 0 && mouths < 4)
        {
            Vector4 uvOffset = new Vector4(- 0f, -0.25f * mouths, 1f, 1f);
            matMouth.SetVector(shaderPropertyName, uvOffset);
        }
        if (mouths >= 4)
        {
            Vector4 uvOffset = new Vector4(-0.5f, -0.25f * (mouths - 4f), 1f, 1f);
            matMouth.SetVector(shaderPropertyName, uvOffset);
        }
    }
}

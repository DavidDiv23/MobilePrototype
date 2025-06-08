using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class FaceAnya : MonoBehaviour
{
    public Material matEyes;
    public Material matMouth;
    public DialogueRunner dialogueRunner;

    public string shaderPropertyName = "_UVOffset";

    [Range(0, 4)]
    public int eyes;

    [Range(0, 4)]
    public int mouths;


    // Start is called before the first frame update
    void Start()
    {
        dialogueRunner.AddCommandHandler<string, string>("set_face", HandleSetFace);
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

    public void SetFace(int eyesIndex, int mouthsIndex)
    {
        eyes = eyesIndex;
        mouths = mouthsIndex;
    }

    private void HandleSetFace(string eyeIndexStr, string mouthIndexStr)
    {
        int eyeIndex = int.Parse(eyeIndexStr);
        int mouthIndex = int.Parse(mouthIndexStr);

        var anya = FindObjectOfType<FaceAnya>();
        if (anya != null)
        {
            anya.SetFace(eyeIndex, mouthIndex);
        }
    }
}

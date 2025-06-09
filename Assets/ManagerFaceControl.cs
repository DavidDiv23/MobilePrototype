using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class ManagerFaceControl : MonoBehaviour
{
    public Material material;
    public DialogueRunner dialogueRunner;

    [Range(0, 3)]
    public int face;

    private void Start()
    {
        if (dialogueRunner != null)
        {
            dialogueRunner.AddCommandHandler<string>("set_manager_face", HandleSetFace);
        }
        else
        {
            Debug.LogError("DialogueRunner reference is missing in ManagerFaceControl.");
        }
    }

    private void Update()
    {
        Vector2 offset = new Vector2(0f, -0.25f * face);
        material.SetTextureOffset("_BaseMap", offset);
    }

    public void SetFace(int faceIndex)
    {
        face = faceIndex;
    }

    private void HandleSetFace(string faceIndexStr)
    {
        if (int.TryParse(faceIndexStr, out int faceIndex))
        {
            var managerFace = FindObjectOfType<ManagerFaceControl>();
            if (managerFace != null)
            {
                managerFace.SetFace(faceIndex);
            }
        }
        else
        {
            Debug.LogError($"Invalid index passed to set_manager_face: {faceIndexStr}");
        }
    }
}
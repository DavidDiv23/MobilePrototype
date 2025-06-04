using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerFaceControl : MonoBehaviour
{
    public Material material;

    [Range(0, 3)]
    public int face;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 offset = new Vector2(0f, -0.25f * face);
        material.SetTextureOffset("_BaseMap", offset);
    }
}

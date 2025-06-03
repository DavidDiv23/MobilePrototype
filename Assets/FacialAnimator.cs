using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class FacialAnimator : MonoBehaviour
{
    [Header("Atlas Settings")]
    public int columns = 4;           // Number of columns in the texture atlas
    public int rows = 4;              // Number of rows in the texture atlas
    public float frameRate = 8f;      // Frames per second

    [Header("Material Settings")]
    public string shaderPropertyName = "_UVOffset"; // Must match your shader
    private Material mat;

    private int currentFrame = 0;
    private float timer = 0f;
    private int totalFrames;

    void Start()
    {
        // Clone material instance so it doesn't modify the shared material
        mat = GetComponent<Renderer>().material;
        totalFrames = columns * rows;
    }

    void Update()
    {
        if (mat == null) return;

        timer += Time.deltaTime;
        if (timer >= 1f / frameRate)
        {
            timer = 0f;
            currentFrame = (currentFrame + 1) % totalFrames;

            // Calculate offset based on current frame
            int col = currentFrame % columns;
            int row = currentFrame / columns;

            // Texture origin is bottom-left, so flip the Y axis
            float xOffset = (float)col / columns;
            float yOffset = 1f - ((float)row + 1) / rows;

            // Apply offset to shader
            Vector4 uvOffset = new Vector4(xOffset, yOffset, 1f, 1f);
            mat.SetVector(shaderPropertyName, uvOffset);
        }
    }
}
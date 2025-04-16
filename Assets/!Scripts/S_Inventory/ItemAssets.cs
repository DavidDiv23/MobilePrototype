using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAssets : MonoBehaviour
{
    public static ItemAssets Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }
    
    public Sprite berrySprite;
    public Sprite crystalSprite;
    public Sprite featherSprite;
    public Sprite leafSprite;
    public Sprite orangeSprite;
    public Sprite rockSprite;
    public Sprite snailSprite;
    public Sprite stickSprite;
    public Sprite blueprintSprite;
    
}

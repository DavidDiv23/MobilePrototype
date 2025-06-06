using System.Collections;
using System.Collections.Generic;
using NodeCanvas.Framework;
using UnityEngine;

[CreateAssetMenu(menuName = "NPC/Personality")]
public class Personality : ScriptableObject {
    public string personalityName;
    [Range(0, 100)] public int friendliness;
    [Range(0, 100)] public int energy;
    
    [SerializeField]
    public BehaviourTree behaviourTree;
}

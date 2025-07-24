using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "JuiceMakerSO", menuName = "ScriptibleObjects/JuiceMakerSO")]

public class JuiceMakerSO : ScriptableObject
{
    public float juiceMakingTime;
    public int juiceMakerCapacity;
}

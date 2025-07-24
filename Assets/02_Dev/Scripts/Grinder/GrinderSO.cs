using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "GrinderSO", menuName = "ScriptibleObjects/GrinderSO")]
public class GrinderSO : ScriptableObject
{
    public float grindingTime;
    public int grinderCapacity;
    public int grindedFruitBowlCapacity;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "JuiceSO", menuName = "ScriptibleObjects/JuiceSO")]

public class JuiceSO : ScriptableObject
{
    public int juiceCapacity;
    public GameObject juicePrefab;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GrindedFruitSO", menuName = "ScriptibleObjects/GrindedFruitSO")]
public class GrindedFruitSO : ScriptableObject
{
    public int grindedFruitCapacity;
    public GameObject grindedFruitPrefab;
}

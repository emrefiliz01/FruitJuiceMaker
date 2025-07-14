using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FruitType {Lemon, Orange}

[CreateAssetMenu(fileName = "FruitPatchSO", menuName = "ScriptibleObjects/FruitPatchSO")]
public class FruitPatchSO : ScriptableObject
{
    public FruitType fruitType;

    public float stageTimer;
    public int spawnLimit;
    public Sprite fruitImage;
}

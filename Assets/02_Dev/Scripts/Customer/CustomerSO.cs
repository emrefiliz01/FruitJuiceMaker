using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CustomerSO", menuName = "ScriptibleObjects/CustomerSO")]
public class CustomerSO : ScriptableObject
{
    public GameObject customerPrefab;
    public float customerMoveSpeed;
}

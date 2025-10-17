using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SellingTableController : MonoBehaviour
{
    public List<Transform> sellingSlots;
    
    public List<GameObject> juiceOnTable;

    public List<GameObject> customerStopSlots;

    public Transform dollarSpawnPoint;

    public bool CanPlaceJuice()
    {
        for (int i = 0; i < juiceOnTable.Count; i++)
        {
            if (juiceOnTable[i] == null)
            {
                return true;
            }
        }
        Debug.Log("NOT NULL");
        return false;
    }

    public int EmptySlot()
    {
        for (int i = 0; i < sellingSlots.Count; i++)
        {
            if (juiceOnTable[i] == null)
            {
                return i;
            }
        }
        return 0;
    }
}

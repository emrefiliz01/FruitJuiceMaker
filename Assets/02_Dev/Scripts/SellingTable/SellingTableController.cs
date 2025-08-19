using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SellingTableController : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private List<Transform> sellingSlots;

    public List<GameObject> placedJuiceList = new List<GameObject>();

    private Dictionary<Transform, bool> slotStatus = new Dictionary<Transform, bool>();

    private void Start()
    {
        foreach (var slot in sellingSlots)
        {
            slotStatus.Add(slot, false);
        }
    }
    public bool CanPutJuiceOnSellingTable()
    {
        return slotStatus.ContainsValue(false);
    }
}

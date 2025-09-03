using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CustomerManager : MonoBehaviour
{
    [SerializeField] private GameObject customerSpawnPoint;
    [SerializeField] private CustomerSO customerSO;
    [SerializeField] private GameObject customerContainer;
    [SerializeField] private SellingTableController sellingTableController;
    public GameObject customerExitPoint;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            SpawnAndMoveCustomer();
        }
    }

    private void SpawnAndMoveCustomer()
    {
        GameObject customer = Instantiate(customerSO.customerPrefab, customerSpawnPoint.transform.position, Quaternion.Euler(0, 180f, 0));
        customer.transform.SetParent(customerContainer.transform);

        customer.GetComponent<Customer>().StartMoving(customerExitPoint.transform.position);
    }

    public SellingTableController GetSellingTableController()
    {
        return sellingTableController;
    }
}

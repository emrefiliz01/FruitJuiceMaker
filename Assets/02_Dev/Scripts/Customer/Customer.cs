using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEditor.Timeline;
using UnityEngine.UIElements;
using UnityEngine.PlayerLoop;
public class Customer : MonoBehaviour
{
    public SellingTableController sellingTableController;
    private CustomerManager customerManager;
    private Vector3 decisionSpot;
    private Coroutine customerMoveCoroutine;
    private CurrencyManager currencyManager;

    private bool hasEntered = false;
    private int randomSlot;

    private bool canMove = false;
    private Vector3 targetPos;
    private float customerMoveSpeed = 7f;

    private void Update()
    {
        if (canMove)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, customerMoveSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetPos) < 0.1f)
            {
                canMove = false;
            }
        }
    }

    public void StartMoving(Vector3 endPos)
    {
        decisionSpot = endPos;
        targetPos = decisionSpot;
        canMove = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "DecisionSpot" && !hasEntered)
        {
            hasEntered = true;

            Debug.Log(transform.name);
            Debug.Log("You entered the Decision Spot");

            customerManager = other.GetComponentInParent<CustomerManager>();

            sellingTableController = customerManager.GetSellingTableController();

            MakeDecision();
        }
    }

    private void MakeDecision()
    {
        List<int> availableSlots = new List<int>();

        for (int i = 0; i < sellingTableController.juiceOnTable.Count; i++)
        {
            if (sellingTableController.juiceOnTable[i] != null)
            {
                availableSlots.Add(i);
            }
        }

        if (availableSlots.Count > 0)
        {
            randomSlot = availableSlots[Random.Range(0, availableSlots.Count)];
            Vector3 stopPos = sellingTableController.customerStopSlots[randomSlot].transform.position;

            targetPos = stopPos;
            canMove = true;

            CustomerMoveAndRotate();

        }
        else
        {
            targetPos = customerManager.customerExitPoint.transform.position;
            canMove = true;

            StartCoroutine(WaitAndDestroy());
        }
    }

    private void CustomerMoveAndRotate()
    {
        customerMoveCoroutine = StartCoroutine(CustomerMoveAndRotateCoroutine());
    }

    private IEnumerator CustomerMoveAndRotateCoroutine()
    {
        while (canMove)
        {
            yield return null;
        }

        transform.DORotate(new Vector3(0, 90f, 0), 1f);
        yield return new WaitForSeconds(0.7f);

        CollectJuice();
        yield return new WaitForSeconds(1f);

        transform.DORotate(new Vector3(0, 180f, 0), 1f);
        yield return new WaitForSeconds(0.7f);

        targetPos = customerManager.customerExitPoint.transform.position;
        canMove = true;

        while (canMove)
        {
            yield return null;
        }

        Destroy(gameObject);
    }

    private void CollectJuice()
    {
        GameObject juice = sellingTableController.juiceOnTable[randomSlot];
        Debug.Log(juice.transform.name);

        Juice juiceScript = juice.GetComponent<Juice>();
        int juicePrice = juiceScript.juiceSO.juicePrice;
        CurrencyManager.Instance.IncreaseMoney(juicePrice);
        Transform slotTransform = sellingTableController.sellingSlots[randomSlot];
        FindObjectOfType<DollarFlyer>().FlyDollar(slotTransform.position);
        
        sellingTableController.juiceOnTable[randomSlot] = null;
        Destroy(juice);
    }

    private IEnumerator WaitAndDestroy()
    {
        while (canMove)
        {
            yield return null;  
        }

        Destroy(gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteracton : MonoBehaviour
{

    private FruitPatchController  fruitPatchController;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "FruitPatch")
        {
            Debug.Log("You entered the fruit patch");
            //   fruitPatchController.CollectFruit();
        }
    }

     
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "FruitPatch")
        {
            fruitPatchController = null;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "FruitPatch")
        {
            fruitPatchController = other.GetComponent<FruitPatchController>();
        }
    }
    public FruitPatchController GetFruitPatchController()
    {
        return fruitPatchController;
    }
}

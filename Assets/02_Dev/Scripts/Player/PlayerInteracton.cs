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

            fruitPatchController = other.GetComponent<FruitPatchController>();

         //   fruitPatchController.CollectFruit();

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "FruitPatch")
        {
            Debug.Log("You exit the fruit patch");
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "FruitPatch")
        {
            fruitPatchController.CollectFruit();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteracton : MonoBehaviour
{

    private FruitPatchController  fruitPatchController;
    private GrinderController grinderController;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "FruitPatch")
        {
            Debug.Log("You entered the fruit patch");
            fruitPatchController = other.GetComponent<FruitPatchController>();
            //   fruitPatchController.CollectFruit();
        }

        if (other.tag == "GrinderSpot")
        {
            Debug.Log("You entered thee grinder spot");
            grinderController = other.GetComponent<GrinderController>();
            // grinderController.StartGrinder();
        }

        if (other.tag == "GrindedFruitTableSpot")
        {
            Debug.Log("You entered thee grinded fruit table spot");
            grinderController = other.GetComponent<GrinderController>();
        }
    }
     
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "FruitPatch")
        {
            fruitPatchController = null;
        }

        if (other.tag == "GrinderSpot")
        {
            grinderController = null;
        }

        if (other.tag == "GrindedFruitTableSpot")
        {
            grinderController = null;
        }
    }

    public FruitPatchController GetFruitPatchController()
    {
        return fruitPatchController;
    }

    public GrinderController GetGrinderController()
    {
        return grinderController;
    }
}

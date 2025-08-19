using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteracton : MonoBehaviour
{

    private FruitPatchController  fruitPatchController;
    private GrinderController grinderController;
    private PlayerController playerController;
    private GrindedFruitController grindedFruitController;
    private JuiceMakerController juiceMakerController;


    private bool isCollectingJuice;


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
            grindedFruitController = other.GetComponent <GrindedFruitController>();
              //  grindedFruitController.CreateGrindedFruit();
        }

        if (other.tag == "JuiceMakerSpot")
        {
            Debug.Log("You entered thee Juice Maker spot");
            juiceMakerController = other.GetComponent<JuiceMakerController>();
            isCollectingJuice = false;
        }

        if (other.tag == "CollectJuiceSpot")
        {
            Debug.Log("You entered thee Juice Collect spot");
            juiceMakerController = other.GetComponent<JuiceMakerController>();
            isCollectingJuice = true;
        }
    }
     
    public void OnTriggerExit(Collider other)
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
            grindedFruitController = null;
        }

        if (other.tag == "JuiceMakerSpot")
        {
            juiceMakerController = null;
            isCollectingJuice = false;
        }

        if (other.tag == "CollectJuiceSpot")
        {
            juiceMakerController = null;
            isCollectingJuice = false;
        }
    }

    public GrindedFruitController GetGrindedFruitController()
    {
        return grindedFruitController;
    }

    public FruitPatchController GetFruitPatchController()
    {
        return fruitPatchController;
    }

    public GrinderController GetGrinderController()
    {
        return grinderController;
    }

    public JuiceMakerController GetJuiceMakerController()
    {
        return juiceMakerController;
    }

    public bool IsCollectingJuice()
    {
        return isCollectingJuice;
    }
}

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
        }

        if (other.tag == "GrinderSpot")
        {
            Debug.Log("You entered thee grinder spot");
            grinderController = other.GetComponent<GrinderController>();
        }

        if (other.tag == "GrindedFruitTableSpot")
        {
            Debug.Log("You entered thee grinded fruit table spot");
            grindedFruitController = other.GetComponent <GrindedFruitController>();
        }

        if (other.tag == "JuiceMakerInput")
        {
            Debug.Log("You entered thee Juice Maker Input");
            juiceMakerController = other.GetComponent<JuiceMakerController>();
            isCollectingJuice = false;
        }

        if (other.tag == "JuiceMakerOutput")
        {
            Debug.Log("You entered thee Juice Maker Output");
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

    public FruitPatchController GetFruitPatchController()
    {
        return fruitPatchController;
    }

    public GrindedFruitController GetGrindedFruitController()
    {
        return grindedFruitController;
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

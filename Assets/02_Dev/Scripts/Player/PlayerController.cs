using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private FloatingJoystick joystick;
    [SerializeField] private GameObject playerModel;
    [SerializeField] private PlayerInteracton playerInteracton;
    [SerializeField] private GameObject fruitSpawnerContainer;
    [SerializeField] private GrinderController grinderController;

    private FruitPatchController fruitPatchController;
    private FruitPatchSO fruitPatchSO;
    private GrinderSO grinderSO;
    

    private Animator animator;
    private Rigidbody rb;
    private bool isRunning;
    public bool isHolding;


    public List<GameObject> collectedFruitList = new List<GameObject>();
    private Transform fruitSpawnPosition;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator= playerModel.GetComponent<Animator>();

        fruitSpawnPosition = playerModel.transform;
    }

    void FixedUpdate()
    {
        Move();
        Rotate();
        ChangeAnimation();
    }

    private void Move()
    {
        float horizontal = joystick.Horizontal;
        float vertical = joystick.Vertical;

        Vector3 moveDirection = new Vector3(horizontal, 0f, vertical);

        rb.velocity = moveDirection * moveSpeed;

        if ((horizontal != 0f || vertical != 0f))
        {
            isRunning = true;
        }
        else
        {
            isRunning = false;
        }

    }

    private void Rotate()
    {
        if (rb.velocity.magnitude > 0.1f)
        {
            playerModel.transform.rotation = Quaternion.LookRotation(rb.velocity);
        }
    }

    private void ChangeAnimation()
    {
        if (isHolding)
        {
            animator.SetBool("isHolding", true);
        }
        else
        {
            animator.SetBool("isHolding", false);
        }


        if (isRunning)
        {
            animator.SetBool("isRunning", true);
            animator.SetBool("isIdle", false);
        }
        else
        {
            animator.SetBool("isRunning", false);
            animator.SetBool("isIdle", true);
        }
    }

    private void Update()
    {
        fruitPatchController = playerInteracton.GetFruitPatchController();
        grinderController = playerInteracton.GetGrinderController();

        if (CanCollectFruit())
        {
            fruitPatchController.CollectFruit();

            if (CanSpawnFruit())
            {
                isHolding = true;
            }
        }

        if (CanGrindFruit())
        {
            grinderController.AddFruitIntoGrinder(collectedFruitList);
            grinderController.StartGrinder();
            DestroyAndClearFruitList();

            isHolding = false;
        }
    }

    private bool CanCollectFruit()  
    {
        if (fruitPatchController != null && fruitPatchController.IsReady())
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    private bool CanSpawnFruit()
    {
        fruitPatchSO = fruitPatchController.GetFruitPatchSO();

        int spawnLimit = fruitPatchSO.spawnLimit;

        if (fruitPatchSO != null && collectedFruitList.Count < spawnLimit)
        {
            GameObject fruitSpawn = Instantiate(fruitPatchSO.fruitPrefab, fruitSpawnerContainer.transform.position + new Vector3(0, 1 * collectedFruitList.Count, 0), Quaternion.identity);

            fruitSpawn.transform.SetParent(fruitSpawnerContainer.transform);

            collectedFruitList.Add(fruitSpawn);

            return true;
        }
        else
        {
            return false;
        }
    }

    private bool CanGrindFruit()
    {
        if (grinderController != null && collectedFruitList.Count > 0 && grinderController.CanAddFruit())
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void DestroyAndClearFruitList()
    {
        foreach (var fruit in collectedFruitList)
        {
            Destroy(fruit);
        }

        collectedFruitList.Clear();
    }
}

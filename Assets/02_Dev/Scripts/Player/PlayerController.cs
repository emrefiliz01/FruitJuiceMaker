using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private FloatingJoystick joystick;
    [SerializeField] private GameObject playerModel;
    [SerializeField] private PlayerInteracton playerInteracton;
    [SerializeField] private GameObject fruitContainer;
    [SerializeField] private GrinderController grinderController;
    [SerializeField] private GameObject lemonPatch;
    [SerializeField] private GameObject grinder;
    [SerializeField] private Vector3 grinderOffSet;

    private FruitPatchController fruitPatchController;
    private FruitPatchSO fruitPatchSO;
    private GrinderSO grinderSO;
    private bool isAddFruitIntoGrinderCoroutineRunning;
    private Coroutine addFruitIntoGrinderCoroutine;

    private Coroutine collectFruitCoroutine;

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
            addFruitIntoGrinderCoroutine = StartCoroutine(AddFruitIntoGrinderCoroutine());
            isHolding = false;
        }
    }

    private bool CanCollectFruit()  
    {
        if (fruitPatchController != null && fruitPatchController.IsReady() && isAddFruitIntoGrinderCoroutineRunning == false)
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
            collectFruitCoroutine = StartCoroutine(CollectFruitCoroutine());

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

    private IEnumerator CollectFruitCoroutine()
    {
        bool isCollected = false;

        Vector3 startPos = lemonPatch.transform.position + new Vector3(0, 0.5f, 0);
        Vector3 localEndPos =  new Vector3(0, collectedFruitList.Count * 1f, 0);

        GameObject fruit = Instantiate(fruitPatchSO.fruitPrefab, startPos, Quaternion.identity);

        fruit.transform.SetParent(fruitContainer.transform);

        collectedFruitList.Add(fruit);
        
        fruit.transform.DOLocalMove(localEndPos, 2f).SetEase(Ease.OutQuart).OnComplete(() =>
        {
            isCollected = true;
        });

        while (!isCollected)
        {
            localEndPos = fruitContainer.transform.position + new Vector3(0, collectedFruitList.Count * 1, 0);
            yield return null;
        }
    }

    public IEnumerator AddFruitIntoGrinderCoroutine()
    {
        if (collectedFruitList.Count > 0 && !isAddFruitIntoGrinderCoroutineRunning)
        {
            isAddFruitIntoGrinderCoroutineRunning = true;

            Vector3 grinderPosition = grinder.transform.position + grinderOffSet;

            GrinderController tempGrinderController = grinderController;

            while (collectedFruitList.Count > 0 && grinderController != null)
            {
                GameObject lastFruit = collectedFruitList[collectedFruitList.Count - 1];

                if (tempGrinderController != null)
                {
                    if (tempGrinderController.CanAddFruit())
                    {
                        lastFruit.transform.DOMove(grinderPosition, 1f).SetEase(Ease.OutQuart).OnComplete(() =>
                        {
                            tempGrinderController.AddFruit(lastFruit);
                                      
                            tempGrinderController.StartGrinder();

                            collectedFruitList.RemoveAt(collectedFruitList.Count - 1);

                            Destroy(lastFruit);;
                        });
                    }
                    yield return new WaitForSeconds(1f);
                }
                yield return null;
            }

            isAddFruitIntoGrinderCoroutineRunning = false;
        }     
    }
}

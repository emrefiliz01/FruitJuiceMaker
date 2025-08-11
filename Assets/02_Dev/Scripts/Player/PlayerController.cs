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
    [SerializeField] private GameObject grindedFruitBowlSpawnPoint;
    [SerializeField] private GameObject grinder;
    [SerializeField] private GameObject juiceMaker;
    [SerializeField] private Vector3 grinderOffSet;
    [SerializeField] private GameObject grindedFruitContainer;
    [SerializeField] private GrindedFruitController grindedFruitController;
    [SerializeField] private GrindedFruitSO grindedFruitSO;

    public JuiceMakerController juiceMakerController;
    private PlayerController playerController;
    private FruitPatchController fruitPatchController;
    private FruitPatchSO fruitPatchSO;
    private GrinderSO grinderSO;
    private bool isAddFruitIntoGrinderCoroutineRunning;
    private bool isAddGrindedFruitIntoJuiceMakerCoroutineRunning;
    private Coroutine addFruitIntoGrinderCoroutine;
    private Coroutine addGrindedFruitIntoJuiceMakerCoroutine;

    private Coroutine collectFruitCoroutine;
    private Coroutine collectGrindedFruitCoroutine;

    private Animator animator;
    private Rigidbody rb;
    private bool isRunning;
    public bool isHolding;

    public List<GameObject> collectedFruitList;
    public List<GameObject> collectedGrindedFruitList;


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
        grindedFruitController = playerInteracton.GetGrindedFruitController();
        fruitPatchController = playerInteracton.GetFruitPatchController();
        grinderController = playerInteracton.GetGrinderController();
        juiceMakerController = playerInteracton.GetJuiceMakerController();
        
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

        if (CanCollectGrindedFruit())
        {
            collectGrindedFruitCoroutine = StartCoroutine(CollectGrindedFruitCoroutine());
        }

        if (CanJuicingGrindedFruit())
        {
            addGrindedFruitIntoJuiceMakerCoroutine = StartCoroutine(AddGrindedFruitIntoJuiceMakerCoroutine());
            isHolding = false;
        }
    }

    private bool CanCollectFruit()  
    {
        if (fruitPatchController != null && fruitPatchController.IsReady() && isAddFruitIntoGrinderCoroutineRunning == false && collectedGrindedFruitList.Count <= 0)
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

    private bool CanJuicingGrindedFruit()
    {
        if (juiceMakerController != null && collectedGrindedFruitList.Count > 0 && juiceMakerController.CanAddGrindedFruit())
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
        
        fruit.transform.DOLocalMove(localEndPos, 0.7f).SetEase(Ease.OutQuart).OnComplete(() =>
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
                        lastFruit.transform.DOMove(grinderPosition, 0.5f).SetEase(Ease.OutQuart).OnComplete(() =>
                        {
                            tempGrinderController.AddFruit(lastFruit);
                                      
                            tempGrinderController.StartGrinder();

                            collectedFruitList.RemoveAt(collectedFruitList.Count - 1);

                            Destroy(lastFruit);
                        });
                    }
                    yield return new WaitForSeconds(1f);
                }
                yield return null;
            }

            isAddFruitIntoGrinderCoroutineRunning = false;
        }     
    }

    public bool CanCollectGrindedFruit()
    {
        if (grindedFruitController != null && grindedFruitController.grindedFruitBowlList.Count > 0 && collectedFruitList.Count <= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public IEnumerator CollectGrindedFruitCoroutine()
    {
        while (collectedGrindedFruitList.Count < grindedFruitSO.grindedFruitCapacity)
        {
            if (CanCollectGrindedFruit())
            {
                GameObject lastGrindedFruitBowl = grindedFruitController.grindedFruitBowlList[grindedFruitController.grindedFruitBowlList.Count - 1];

                lastGrindedFruitBowl.transform.SetParent(grindedFruitContainer.transform);

                Vector3 localEndPos = new Vector3(0, collectedGrindedFruitList.Count * 1f, 0);   

                lastGrindedFruitBowl.transform.DOLocalMove(localEndPos, 0.7f).SetEase(Ease.OutQuart).OnComplete(() =>
                {
                    grindedFruitController.grindedFruitBowlList.Remove(lastGrindedFruitBowl);

                    Debug.Log("giriyo on complete'e");
                    isHolding = true;

                    if (!collectedGrindedFruitList.Contains(lastGrindedFruitBowl))
                    {
                        collectedGrindedFruitList.Add(lastGrindedFruitBowl);
                    }
                });

                yield return new WaitForSeconds(1f);
            }
            yield return null;
        }
    }

    public IEnumerator AddGrindedFruitIntoJuiceMakerCoroutine()
    {
        if (collectedGrindedFruitList.Count > 0 && !isAddGrindedFruitIntoJuiceMakerCoroutineRunning)
        {
            isAddGrindedFruitIntoJuiceMakerCoroutineRunning = true;

            Vector3 juiceMakerPosition = juiceMaker.transform.position + new Vector3(0,3f,0);

            while (collectedGrindedFruitList.Count > 0)
            {
                GameObject lastGrindedFruit = collectedGrindedFruitList[collectedGrindedFruitList.Count - 1];

                if (juiceMakerController.CanAddGrindedFruit())
                {
                    lastGrindedFruit.transform.DOMove(juiceMakerPosition, 0.5f).SetEase(Ease.OutQuart).OnComplete(() =>
                    {
                        juiceMakerController.AddGrindedFruit(lastGrindedFruit);

                        juiceMakerController.StartJuiceMaker();

                        collectedGrindedFruitList.RemoveAt(collectedGrindedFruitList.Count-1);

                        Destroy(lastGrindedFruit);
                    });
                }
                yield return new WaitForSeconds(1f);
            }

            isAddGrindedFruitIntoJuiceMakerCoroutineRunning = false;
        }
    }
}

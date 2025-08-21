using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{
    #region Player Settings
    [SerializeField] private float moveSpeed;
    [SerializeField] private FloatingJoystick joystick;
    [SerializeField] private GameObject playerModel;
    [SerializeField] private PlayerInteracton playerInteracton;
    private Animator animator;
    private Rigidbody rb;
    private bool isRunning;
    public bool isHolding;
    private PlayerController playerController;
    #endregion

    #region Patch Settings
    [SerializeField] private GameObject lemonPatch;
    [SerializeField] private GameObject fruitContainer;
    private FruitPatchSO fruitPatchSO;
    private FruitPatchController fruitPatchController;
    public List<GameObject> collectedFruitList;
    private Coroutine collectFruitCoroutine;
    private Transform fruitSpawnPosition;
    #endregion

    #region Grinder Settings
    [SerializeField] private GrinderController grinderController;
    [SerializeField] private GameObject grinder;
    [SerializeField] private Vector3 grinderOffSet;
    private GrinderSO grinderSO;
    private Coroutine addFruitIntoGrinderCoroutine;
    private bool isAddFruitIntoGrinderCoroutineRunning;
    #endregion

    #region GrindedFruit Settings
    [SerializeField] private GameObject grindedFruitBowlSpawnPoint;
    [SerializeField] private GameObject grindedFruitContainer;
    [SerializeField] private GrindedFruitController grindedFruitController;
    [SerializeField] private GrindedFruitSO grindedFruitSO;
    public List<GameObject> collectedGrindedFruitList;
    private Coroutine collectGrindedFruitCoroutine;
    private bool isCollectingGrindedFruit = false;
    #endregion

    #region Juice Maker Settings
    [SerializeField] private GameObject juiceMaker;
    [SerializeField] private GameObject juiceContainer;
    [SerializeField] private JuiceSO juiceSO;
    public JuiceMakerController juiceMakerSpotController;
    private JuiceMakerController juiceMakerController;
    public JuiceMakerController collectJuiceSpotController;
    private Coroutine addGrindedFruitIntoJuiceMakerCoroutine;
    private bool isAddGrindedFruitIntoJuiceMakerCoroutineRunning;
    public List<GameObject> collectedJuiceList;
    private Coroutine collectJuiceCoroutine;
    #endregion

    #region Selling Table Settings
    [SerializeField] private GameObject sellingTable;
    private SellingTableController sellingTableController;
    #endregion


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


        //juiceMakerController = juiceMakerSpotController; // for test


        bool isCollectingJuice = playerInteracton.IsCollectingJuice();
        
        if (CanCollectFruit())
        {
            fruitPatchController.CollectFruit();

            if (CanSpawnFruit())
            {
                isHolding = true;
            }
        }

        if (juiceMakerController != null)
        {
            if (playerInteracton.IsCollectingJuice() == true)
            {
                if (CanPickUpFromJuiceMaker())
                {
                    collectJuiceCoroutine = StartCoroutine(CollectJuiceCoroutine());
                }
            }
            else
            {
                if (CanDropOnJuiceMaker())
                {
                    addGrindedFruitIntoJuiceMakerCoroutine = StartCoroutine(AddGrindedFruitIntoJuiceMakerCoroutine());
                    isHolding = false;
                }
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

        if (isCollectingJuice && CanPickUpFromJuiceMaker())
        {
            collectJuiceCoroutine = StartCoroutine(CollectJuiceCoroutine());
        }    
    }

    #region Fruit Patch
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

    private bool CanCollectFruit()
    {
        if (fruitPatchController != null && fruitPatchController.IsReady() && collectedFruitList.Count < 3 && isAddFruitIntoGrinderCoroutineRunning == false && collectedGrindedFruitList.Count <= 0 && collectedJuiceList.Count <= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
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

    #endregion

    #region Grinder
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
                        lastFruit.transform.DOMove(grinderPosition, 0.6f).SetEase(Ease.OutQuart).OnComplete(() =>
                        {
                            tempGrinderController.AddFruit(lastFruit);
                                      
                            tempGrinderController.StartGrinder();

                            collectedFruitList.RemoveAt(collectedFruitList.Count - 1);

                            Destroy(lastFruit);
                        });
                    }
                   // yield return new WaitForSeconds(1f);
                }
                yield return null;
            }

            isAddFruitIntoGrinderCoroutineRunning = false;
        }     
    }

    #endregion

    #region Grinded Fruits
    public bool CanCollectGrindedFruit()
    {
        if (grindedFruitController != null && grindedFruitController.grindedFruitBowlList.Count > 0 && collectedFruitList.Count <= 0 && collectedJuiceList.Count <= 0)
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
            if (CanCollectGrindedFruit() && !isCollectingGrindedFruit)
            {
                isCollectingGrindedFruit = true;
                bool isCollected = false;

                GameObject lastGrindedFruitBowl = grindedFruitController.grindedFruitBowlList[grindedFruitController.grindedFruitBowlList.Count - 1];

                grindedFruitController.grindedFruitBowlList.Remove(lastGrindedFruitBowl);

                lastGrindedFruitBowl.transform.SetParent(grindedFruitContainer.transform);

                if (!collectedGrindedFruitList.Contains(lastGrindedFruitBowl))
                {
                    collectedGrindedFruitList.Add(lastGrindedFruitBowl);
                }

                Vector3 localEndPos = new Vector3(0, collectedGrindedFruitList.Count * 1f, 0);

                lastGrindedFruitBowl.transform.DOLocalMove(localEndPos, 0.7f).SetEase(Ease.OutQuart).OnComplete(() =>
                {
                    isCollected = true;

                    isHolding = true;

                    isCollectingGrindedFruit = false;

                    grindedFruitController.CheckAndStartGrinder();
                });

                //while (!isCollected)
                // {
                //     yield return null;
                // }

                
                
            }
            /* else
             {
                 yield return null;
             }*/
            yield return null;

            
        }
    }

    #endregion

    #region Juice Maker
    private bool CanDropOnJuiceMaker()
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

    public bool CanPickUpFromJuiceMaker()
    {
        if (juiceMakerController != null && juiceMakerController.juiceList.Count > 0 && collectedFruitList.Count <= 0 && collectedGrindedFruitList.Count <= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }



    public IEnumerator CollectJuiceCoroutine()
    {
        while (collectedJuiceList.Count < juiceSO.juiceCapacity)
        {
            if (CanPickUpFromJuiceMaker())
            {
                GameObject lastJuice = juiceMakerController.juiceList[juiceMakerController.juiceList.Count - 1];

                lastJuice.transform.SetParent(juiceContainer.transform);

                Vector3 localEndPos = new Vector3(0, collectedJuiceList.Count * 1f, 0);

                lastJuice.transform.DOLocalMove(localEndPos, 1f).SetEase(Ease.OutQuart).OnComplete(() =>
                {
                    Debug.Log("entered the OnComplete");
                    juiceMakerController.juiceList.Remove(lastJuice);

                    if (!collectedJuiceList.Contains(lastJuice))
                    {
                        collectedJuiceList.Add(lastJuice);
                    }
                    isHolding = true;
                });

                Debug.Log("didn't entered the OnComplete");

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

    #endregion

    /* public IEnumerator AddJuiceOnTheSellingTable()
     {
         if (collectedJuiceList.Count > 0)
         {
             Vector3 sellingTablePosition = sellingTable.transform.position;

             while (collectedJuiceList.Count > 0)
             {
                 GameObject lastJuice = collectedJuiceList[collectedJuiceList.Count-1];

                 if (sellingTableController.CanPutJuiceOnSellingTable())
                 {
                     lastJuice.transform.DOMove(sellingTablePosition, 0.5f).SetEase(Ease.OutQuart).OnComplete(() =>
                     {

                     });
                 }
             }
         }
     } */
}

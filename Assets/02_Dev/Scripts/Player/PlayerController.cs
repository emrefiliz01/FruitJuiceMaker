using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private FloatingJoystick joystick;
    [SerializeField] private GameObject playerModel;
    [SerializeField] private PlayerInteracton playerInteracton;

    private Animator animator;
    private Rigidbody rb;
    private bool isRunning;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator= playerModel.GetComponent<Animator>();
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
}

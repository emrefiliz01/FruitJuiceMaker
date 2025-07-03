using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementHandler : MonoBehaviour
{
    [SerializeField] private float moveSpeed;

    Animator animator;
    Rigidbody rb;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        bool isRunning = Input.GetKey(KeyCode.W);
        animator.SetBool("isRunning", isRunning);

        if (isRunning )
        {
            Vector3 moveForward = transform.forward * moveSpeed;
            rb.velocity = moveForward;
        }
        else
        {
            rb.velocity = Vector3.zero;
        }
    }
}

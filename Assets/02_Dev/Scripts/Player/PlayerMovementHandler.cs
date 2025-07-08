using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementHandler : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private FloatingJoystick joystick;
    [SerializeField] private Transform playerTransform;

    Animator animator;
    Rigidbody rb;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        float horizontal = joystick.Horizontal;
        float vertical = joystick.Vertical;

        Vector3 moveDirection = new Vector3 (horizontal, 0f, vertical);

        rb.velocity = moveDirection * moveSpeed;

        bool isRunning = (horizontal != 0f || vertical != 0f);
        animator.SetBool("isRunning", isRunning);

        if (isRunning)
        {
            playerTransform.rotation = Quaternion.LookRotation(rb.velocity);
        }
    }
}

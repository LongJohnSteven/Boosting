using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    private float horizontal;
    private float vertical;
    private float acceleration = 1f;
    private float drag = 0.5f;
    private float maxSpeed = 8f;
    private float jumpStrength = 6f;
    private float swingStrength = 15f;
    private bool leftFacing = false;
    private bool control = true;
    public Animator animator;
    [SerializeField] private Rigidbody2D Rigidbody2D;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform leftCheck;
    [SerializeField] private Transform rightCheck;
    [SerializeField] private LayerMask groundLayer;
    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        animator.SetBool("running", horizontal!=0 && IsGrounded());

        FlipPlayer();

        Attack();

        Jump();
    }

    private void FixedUpdate()
    {
        HorizontalAcceleration();
        Drag();
    }

    private void Attack()
    {
        if (Input.GetButtonDown("Fire1") && !IsGrounded())
        {
            RemoveControlForTime(0.5f); 
            if (vertical != 0)
            {
                Rigidbody2D.velocity = new Vector2(Rigidbody2D.velocity.x, swingStrength * ((vertical > 0) ? -1 : 1)/2);
            }
            if (horizontal != 0)
            {
                Rigidbody2D.velocity = new Vector2(swingStrength * ((horizontal > 0) ? -1 : 1), Rigidbody2D.velocity.y);
            }
            else
            {
                Rigidbody2D.velocity = new Vector2(swingStrength * (isFacingLeft() ? 1 : -1), Rigidbody2D.velocity.y);
            }

        }
    }

    private void Jump()
    {
        if (Input.GetButtonDown("Jump"))
        {
            if (IsGrounded())
            {
                Rigidbody2D.velocity = new Vector2(Rigidbody2D.velocity.x, jumpStrength);
            }
            else if (TouchedLeft() || TouchedRight())
            {
                Rigidbody2D.velocity = new Vector2((TouchedLeft() ? 1f : -1f) * jumpStrength * 0.5f, jumpStrength * 0.7f); ;
            }
        }
        if (Input.GetButtonUp("Jump") && Rigidbody2D.velocity.y > 0f)
        {
            Rigidbody2D.velocity = new Vector2(Rigidbody2D.velocity.x, Rigidbody2D.velocity.y * 0.7f);
        }
        if (IsGrounded())
        {
            animator.SetBool("jumping", false);
        }
        else
        {
            animator.SetBool("jumping", true);
        }
    }

    private void FlipPlayer()
    {
        if(leftFacing && horizontal >0f || !leftFacing && horizontal < 0f) 
        {
            leftFacing = !leftFacing;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }
    private void HorizontalAcceleration()
    {
        if (Rigidbody2D.velocity.x * horizontal < maxSpeed && control)
        {
            Rigidbody2D.velocity = new Vector2(Rigidbody2D.velocity.x + (IsGrounded() ? acceleration : acceleration / 5) * horizontal, Rigidbody2D.velocity.y);
        }
    }
    private void Drag()
    {
        float startingSign = Mathf.Sign(Rigidbody2D.velocity.x);
        Rigidbody2D.velocity = new Vector2(Rigidbody2D.velocity.x - ((IsGrounded() ? drag : drag / 5) * startingSign), Rigidbody2D.velocity.y);
        if(Mathf.Sign(Rigidbody2D.velocity.x) != startingSign) { Rigidbody2D.velocity = new Vector2(0, Rigidbody2D.velocity.y); }
    }

    private IEnumerator RemoveControlForTime(float time)
    {
        control = false;
        yield return new WaitForSeconds(time);
        control = true;
    }
    public bool isFacingLeft()
    {
        return leftFacing;
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);
    }

    private bool TouchedLeft()
    {
        return Physics2D.OverlapCircle(leftCheck.position, 0.2f, groundLayer);
    }
    private bool TouchedRight()
    {
        return Physics2D.OverlapCircle(rightCheck.position, 0.2f, groundLayer);
    }
}



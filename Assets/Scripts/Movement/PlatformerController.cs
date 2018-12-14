using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A comprehensive box & raycast-based 2D PlayerController for platformers.
/// </summary>
public class PlatformerController : RaycastRigidbody
{
    //Holds the data for the player controllers for easy preset management and inspector viewing.
    [Serializable]
    private class PlatformerControllerData
    {
        public MovementPlatformerControllerData groundMovement = new MovementPlatformerControllerData();
        public MovementPlatformerControllerData airMovement = new MovementPlatformerControllerData();

        //Starting rigidbody.velocity of a jump
        public float jumpStrength = 8;
        //Starting rigidbody.velocity of a mid-air jump
        public float multiJumpStrength = 7;
        //Gravity acceleration
        public float gravityAcceleration = 14f;
        //Max Jump Count, used for double jumping
        public int jumpCount = 2;

        public AdvancedPlatformerControllerData advancedSettings = new AdvancedPlatformerControllerData();
    }

    //Used for better separation and better inspector viewing, more nitpicky values like differing fall speeds.
    [Serializable]
    private class AdvancedPlatformerControllerData
    {

        //Free falling gravity multiplier
        public float fallMultiplier = 2.3f;
        //Small jump press gravity multiplier
        public float lowJumpMultiplier = 2.3f;

    }

    [Serializable]
    private class MovementPlatformerControllerData
    {
        public float walkSpeed = 3;
        public float sprintSpeed = 4.5f;

        //Horizontal accelleration speed
        public float moveAcceleration = 15f;
        //Horizontal deceleration speed
        public float moveDeceleration = 15f;
    }

    private struct PlatformerInput
    {
        public Vector2 wasd;
        public bool jumpStart;
        public bool jumping;
        public bool sprinting;
    }

    //The currently used data for the movement
    [SerializeField]
    private PlatformerControllerData movementData = new PlatformerControllerData();
    private PlatformerInput input = new PlatformerInput();


    private new RaycastRigidbody rigidbody;


    //Keeps track of how many jumps have been made in the current jump. For multi-jumps
    private int jumpsUsed = 0;

    Animator anim;

    [SerializeField]
    private HealthBar bar;

    private bool grounded = false;
    private float amgrounded = 0;



    // Use this for initialization
    protected new void Start()
    {
        base.Start();
        input.wasd = new Vector2(0, 0);
        rigidbody = GetComponent<RaycastRigidbody>();
        anim = GetComponent<Animator>();
    }

    //called on graphics frames.
    protected void Update()
    {
        //Handle Input in Update for smoother controls.
        input.wasd.x = Input.GetAxisRaw("Horizontal");
        input.wasd.y = Input.GetAxisRaw("Vertical");
        if (Input.GetButtonDown("Jump"))
            input.jumpStart = true;
        input.jumping = Input.GetButton("Jump");
        input.sprinting = Input.GetButton("Sprint");

        if(Input.GetMouseButtonDown(0))
        {
            bar.TakeDamage(5);
        }

        //fall damage
        this.grounded = rigidbody.IsColliding(RayDirection.Down);

        if (amgrounded <= -19 && this.grounded)
            bar.TakeDamage(10);

        this.amgrounded = this.rigidbody.velocity.y;

    }

    protected void FixedUpdate()
    {
        base.FixedUpdate();
        anim.SetFloat("Speed", Math.Abs(rigidbody.velocity.x));
        if (Input.GetKeyDown(KeyCode.F))
            anim.SetTrigger("Attack");
    }

    // MovementLogic is called once per physics frame
    protected override void MovementLogic()
    {

        this.grounded = rigidbody.IsColliding(RayDirection.Down);

        //Point sprite in right direction
        if (input.wasd.x > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (input.wasd.x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }

        //Check Grounded & Collisions

        //Set move speed based on sprint
        float moveSpeed;

        //Move horizontally
        if (grounded)
        {

            jumpsUsed = 0;

            //Jump
            if (this.movementData.jumpCount > 0 && input.jumpStart)
            {
                rigidbody.velocity.y = this.movementData.jumpStrength;
                jumpsUsed++;
                rigidbody.SetColliding(RayDirection.Down, false);
                grounded = false;
            }

        }
        else
        {
            if (jumpsUsed == 0) jumpsUsed = 1;
            //Fall
            rigidbody.velocity.y -= this.movementData.gravityAcceleration * Time.fixedDeltaTime;

            //Jump crisp falling
            if (rigidbody.velocity.y < 0)
            {
                rigidbody.velocity.y -= this.movementData.gravityAcceleration * (this.movementData.advancedSettings.fallMultiplier - 1) * Time.fixedDeltaTime;
            }
            else if (rigidbody.velocity.y > 0 && !input.jumping)
            {
                rigidbody.velocity.y -= this.movementData.gravityAcceleration * (this.movementData.advancedSettings.lowJumpMultiplier - 1) * Time.fixedDeltaTime;
            }

            //Midair Jump
            if (jumpsUsed < this.movementData.jumpCount && input.jumpStart)
            {
                rigidbody.velocity.y = this.movementData.multiJumpStrength;
                jumpsUsed++;
            }
        }

        //Move horizontally
        {
            MovementPlatformerControllerData moveDataSet = (grounded) ? this.movementData.groundMovement : this.movementData.airMovement;

            moveSpeed = (!input.sprinting) ? moveDataSet.walkSpeed : moveDataSet.sprintSpeed;
            if (input.wasd.x != 0 && Mathf.Abs(rigidbody.velocity.x) < moveSpeed)
            {
                //accelerate to max speed
                rigidbody.velocity.x += moveDataSet.moveAcceleration * Time.fixedDeltaTime * Mathf.Sign(input.wasd.x);
            }
            else
            {
                //deccelerate to 0
                if (rigidbody.velocity.x != 0)
                {
                    rigidbody.velocity.x += moveDataSet.moveDeceleration * Time.fixedDeltaTime * -Mathf.Sign(rigidbody.velocity.x);
                    //snap rigidbody.velocity to 0 when it gets close enough so that it doesn't keep wobbling
                    if (Mathf.Abs(rigidbody.velocity.x) < 0.4f) rigidbody.velocity.x = 0;
                }
            }
        }

        //Reset down inputs, must be done last
        input.jumpStart = false;
    }

    protected override void CollisionEnter()
    {
        //if((rigidbody.IsColliding(RayDirection.Right) && )

        //if (rigidbody.IsColliding(RayDirection.Right))
        //    velocity += new Vector2(-12, 12);
        //else if (rigidbody.IsColliding(RayDirection.Left))
        //    velocity += new Vector2(12, 12);
    }

}
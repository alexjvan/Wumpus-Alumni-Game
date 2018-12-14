using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightController : MonoBehaviour {
    public float maxSpeed = 5f;
    bool facingRight = true;
    private bool attacking = false;

    [SerializeField]
    private Rigidbody2D body;

    Animator anim;

    bool grounded = false;
    public Transform groundCheck;
    float groundRadius = 0.2f;
    public LayerMask whatIsGround;
    public float jumpForce = 700f;
    bool doubleJump = false;

    // Use this for initialization
    void Start () {
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround);
        anim.SetBool("Ground", grounded);

        anim.SetFloat("vSpeed", body.velocity.y);
        if (grounded)
            doubleJump = false;
        
        // Get the X andY values from the Horizontal buttons or Joystick directions.
        float move = Input.GetAxis("Horizontal");

        anim.SetFloat("Speed", Mathf.Abs(move));

        this.body.velocity = new Vector2(move * maxSpeed, this.body.velocity.y);

        if (move > 0 && !facingRight)
            Flip();
        else if (move < 0 && facingRight)
            Flip();
    }

    void Update()
    {
        if ((grounded || !doubleJump) && Input.GetButtonDown("Jump"))
        {
            anim.SetBool("Ground", false);
            body.AddForce(new Vector2(0, jumpForce));

            if (!grounded && !doubleJump)
                doubleJump = true;
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            anim.SetTrigger("Slash");
        }
        // Teleporting back to starting location
        if (this.transform.position.y <= -10f || Input.GetKeyDown(KeyCode.P))
        {
            this.transform.position = new Vector2(0, 5);
            this.body.velocity = new Vector2(0, 0);
        }
    }

    void Flip()
    {
        this.facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    public bool Direction()
    {
        return facingRight;
    }
}

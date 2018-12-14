using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour {

    private bool attacking = false;

    private float attackTimer = 0f;
    private float attackCD = 0.5f;
    public CircleCollider2D attackBox;

    // Use this for initialization
    void Start () {
        attackBox.enabled = false;
    }

    // Update is called once per frame
    void FixedUpdate () {
        // attacking script
        if (Input.GetKeyDown(KeyCode.F) && attackTimer <= 0)
        {
            attackBox.enabled = true;
            attackTimer = attackCD;
            attacking = true;
        }

        if (attacking)
        {
            if (attackTimer > 0)
            {
                attackTimer -= Time.deltaTime;
            }
            else
            {
                attacking = false;
                attackBox.enabled = false;
            }
        }


    }
}

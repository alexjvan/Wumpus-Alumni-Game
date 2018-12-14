using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightEnemy : MonoBehaviour {

    [SerializeField]
    private Rigidbody2D body;

    private SpriteRenderer spite;
    private bool hit = false;
    private float timer = 0f;
    private EnemyHealth k;
    // Use this for initialization

    void Start()
    {
        spite = GetComponent<SpriteRenderer>();
        k = GetComponent<EnemyHealth>();
    }
    public void Damage(int dmg)
    {
        spite.material.SetColor("_Color", Color.red);
        hit = true;
        k.Damage(10);
    }

    void Update()
    {
        // Might change due to easier references

        //if (hit && timer >= 0.5f)
        //{
        //  Destroy(gameObject);
        //}
        //else if (hit)
        //{
        //    timer += Time.deltaTime;
        //}
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Robot"))
        {
            Damage(10);
        }
        Vector3 playerPos = GameObject.Find("RobotChar").transform.position;
        //this.body = GetComponent<Rigidbody2D>();
        //Debug.Log(player.transform.position.x);
        if (playerPos.x > this.transform.position.x)
        {
            this.body.velocity = new Vector2(-3, 10);
        }
        else
            this.body.velocity = new Vector2(3, 10);

    }
}

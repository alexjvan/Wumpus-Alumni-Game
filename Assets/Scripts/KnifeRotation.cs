using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeRotation : MonoBehaviour {

    //public float rotationSpeed = -300f;
    [SerializeField]
    public Rigidbody2D knife;

    [SerializeField]
    BoxCollider2D knifeEdge;
    private bool facingRight;
    private bool hit = false;

    // Called once initialized
    void Start ()
    {
        GameObject k = GameObject.Find("Character");
        KnightController knight = k.GetComponent<KnightController>();
        facingRight = knight.Direction();
    }
    // Update is called once per frame
    void Update () {
        if(!hit)
        transform.Rotate(new Vector3(0, 0, 1 * (facingRight ? 1 : -1) * Random.Range(-1000f, -300f) * Time.deltaTime));
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        this.knife.freezeRotation = true;
        knife.velocity = new Vector2(0, 0);
        hit = true;
        knifeEdge.enabled = false;
    }
}

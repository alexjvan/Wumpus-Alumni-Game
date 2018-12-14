using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy : MonoBehaviour {

    public float timeDestroy = 0.5f;

    // Use this for initialization
    void Start () {
        GameObject k = GameObject.Find("Character");
        KnightController knight = k.GetComponent<KnightController>();
        bool facingRight = knight.Direction();

        if (!facingRight)
        {
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }
        Destroy(gameObject, timeDestroy);
	}
	

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeThrower : MonoBehaviour {

    [SerializeField]
    public Rigidbody2D knife;
    public Transform knifeLocation;
    public float knifeSpeed = 10;

    // Update is called once per frame
    void Update () {
        GameObject k = GameObject.Find("Character");
        KnightController knight = k.GetComponent<KnightController>();
        bool facingRight = knight.Direction();

        if (Input.GetButtonDown("Fire1"))
        {
            Rigidbody2D knifeInstance;
            knifeInstance = Instantiate(knife, knifeLocation.position, knifeLocation.rotation) as Rigidbody2D;
            knifeInstance.GetComponent<Rigidbody2D>().velocity += 
                new Vector2( facingRight ? 1 : -1, 0).normalized * knifeSpeed;
        }
    }

}

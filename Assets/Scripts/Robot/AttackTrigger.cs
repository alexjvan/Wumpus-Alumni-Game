using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTrigger : MonoBehaviour {

    private int dmg = 20;

    void OnTriggerEnter2D(Collider2D col)
    {
        // Might change due to simpler references

        //if (col.isTrigger != true && col.CompareTag("Enemy"))
        //{
        //    col.SendMessageUpwards("Damage", dmg);
        //}

    }
	
}

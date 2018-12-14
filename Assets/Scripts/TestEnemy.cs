using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemy : MonoBehaviour {

    private SpriteRenderer spite;
	// Use this for initialization
	void Start () {
        spite.material.SetColor("_Color", Color.red);
	}
	
	// Update is called once per frame
	void Update () {
	}
}

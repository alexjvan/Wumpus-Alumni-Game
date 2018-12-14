using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {

    [SerializeField]
    private Text valueText;

    [SerializeField]
    private float maxHealth = 100;

    [SerializeField]
    private float currentHealth;

    //[SerializeField]
    //private Image bar;

	// Use this for initialization
	void Start () {
        currentHealth = maxHealth;
        valueText.text = "Health : " + maxHealth;
	}
	
	// Update is called once per frame
	void Update () {
		//if(Input.GetMouseButtonDown(0))
  //      {
  //          TakeDamage();
  //      }
	}

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        transform.localScale = new Vector2((currentHealth / maxHealth), 1);
        string[] tmp = valueText.text.Split(':');
        valueText.text = tmp[0] + ": " + currentHealth;
    }
}

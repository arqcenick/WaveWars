﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {

    // Use this for initialization

    PlayerController.Attacks type;

	void Start () {
        int random = Random.Range(0, 1);
        type = (PlayerController.Attacks)random;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Ball")
        {
            PlayerController controller = collision.gameObject.GetComponent<PlayerController>();
            controller.PickUpItem(type);
        }
    }
}

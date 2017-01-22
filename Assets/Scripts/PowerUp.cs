using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour {


    PlayerController.Attacks type;
    // Use this for initialization
    void Start()
    {
        int random = Random.Range(0, 1);
        type = (PlayerController.Attacks)random;
        //type = PlayerController.Attacks.IronBall;
    }

    // Update is called once per frame
    void Update () {

        gameObject.transform.Rotate(Vector3.up, 100 * Time.deltaTime, Space.Self);
       
	}

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.tag == "Ball")
        {

            PlayerController controller = collision.gameObject.GetComponent<PlayerController>();
            controller.PickUpItem(type);
            GameController.currentPowerUp = null;
            Destroy(gameObject);
        }
    }
}

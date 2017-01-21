using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {


    bool movementComplete;
    Vector3 position;
    GameObject prevColObject;
    Rigidbody rigid;
    float frames = 40;
    float accCons = 25f;
    // Use this for initialization
    Renderer rend;
    int playerId;

    private void Awake()
    {
        rend = transform.GetChild(0).GetComponent<Renderer>();
    }
    void Start () {
        movementComplete = true;
        rigid = gameObject.GetComponent<Rigidbody>();
        
        
	}
    public bool snapped = false;
    public int[] snapTile;

    KeyCode up;
    KeyCode left;
    KeyCode down;
    KeyCode right;
    KeyCode jump;

    public int PlayerId
    {
        get
        {
            return playerId;
        }

        set
        {
            if(value == 0)
            {
                up = KeyCode.W;
                left = KeyCode.A;
                down = KeyCode.S;
                right = KeyCode.D;
                jump = KeyCode.Space;
                rend.material.color = Color.white;
                //Kouhai suki

            }
            else if(value == 1)
            {
                up = KeyCode.UpArrow;
                left = KeyCode.LeftArrow;
                down = KeyCode.DownArrow;
                right = KeyCode.RightArrow;
                jump = KeyCode.KeypadEnter;
                rend.material.color = Color.red;
            }
            playerId = value;

        }
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        if(transform.position.y < -10f)
        {
            GameController.Lose(this);
        }
        Vector3 forceVector = Vector3.zero;
        if (Input.anyKey)
        {
            if (Input.GetKey(up))
            {
                //movementComplete = false;
                //Kouhai <3 Sempai

                forceVector = Vector3.right + Vector3.forward;


                //Piremses

            }
            if (Input.GetKey(left))
            {
                forceVector += Vector3.left + Vector3.forward;

            }
            if (Input.GetKey(down))
            {
                forceVector += Vector3.back + Vector3.left;
            }
            if (Input.GetKey(right))
            {
                forceVector += Vector3.right + Vector3.back;
            }
            if (Input.GetKeyDown(jump))
            {
                
                rigid.AddForce(Vector3.up * 600f);
            }

            forceVector = forceVector.normalized * accCons;
            rigid.AddForce(forceVector);
        }
    }
    void Update () {
		
       

	}

    IEnumerator InitMoveUp()
    {
        float oldPosX = position[0];
        float oldPosY = position[1];
        for(int i = 0; i<frames; i++)
        {
            float x = (i+1) / frames;
            position[0] = oldPosX + x;
            position[1] = oldPosY -x * (x - 1)*10;
            transform.position = position;
            yield return null;
        }
        movementComplete = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        CreateWave(collision);
    }



    private void OnCollisionStay(Collision collision)
    {
        if (rigid.velocity.y > 0.0f)
        {
            for(int i = 0; i < GameController.hasWaveSource.Length; i++)
            {
                if(GameController.hasWaveSource[i])
                {
                    rigid.AddExplosionForce(rigid.velocity.y , new Vector3(i / 15, i % 15), 1000f);

                }
            }
            
        }
    }

    void CreateWave(Collision collision)
    {
        if (collision.relativeVelocity.y > 10f || collision.relativeVelocity.y < -10f)
        {
            Debug.Log("Collision");
            int[] position = Transform2Index(collision.gameObject.transform);
            if (!GameController.hasWaveSource[position[0] * 15 + position[1]] || true)
            {
                int x = position[0];
                int y = position[1];
                Wave wave = new Wave(GameController.gridX, GameController.gridY, x, y, GameController.phaseGrid[x * 15 + y], collision.relativeVelocity.y);
                GameController.WaveCollection.Add(wave);
                GameController.hasWaveSource[x * 15 + y] = true;
            }
                
        }
    }

    int[] Transform2Index(Transform trans)
    {
        
        int[] result = new int[2];
        result[0] = (int) trans.position.x;
        result[1] = (int) trans.position.z;
        return result;
    }

    void SnapGrid(Transform trans)
    {
        snapTile = Transform2Index(trans);
        snapped = true;
    }

}

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
        rigid.WakeUp();
        
        
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
        rigid.WakeUp();
        if (transform.position.y < -10f)
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
                StartCoroutine("TryJump");
            }

            forceVector = forceVector.normalized * accCons;
            rigid.AddForce(forceVector);
        }
    }

    IEnumerator TryJump()
    {
        for(int i = 0; i<15; i++)
        {
            if(CheckBelow())
            {
                rigid.AddForce(Vector3.up * 700f);
                int[] position = Transform2Index(transform, true);
                int x = position[0];
                int y = position[1];
                Debug.Log(x);
                GameController.hitCount[x * 15 + y] -= 1;

                i = 15;
            }
            yield return null;
        }
    }

    void Update () {

        
        if (CheckBelow() &&  rigid.velocity.y > 0.0f)
        {
            Debug.Log("Player Id" + playerId);
            for (int i = 0; i < GameController.WaveCollection.Count; i++)
            {
                Wave wave = GameController.WaveCollection[i];
                    rigid.AddExplosionForce(rigid.velocity.y * wave.Force * 0.1f, GameController.posGrid[i], 100f);

            }

        }

    }

    bool CheckBelow()
    {
        Ray ray = new Ray(transform.position, Vector3.down);
        if (Physics.Raycast(ray, 0.6f))
        {
            
            return true;
            
        }
        else
            return false;
    }


    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Ball")
        {
            rigid.AddExplosionForce(800f, (transform.position + collision.transform.position) / 2f, 100f);
        }
        else
        {
            CreateWave(collision);
            
        }
        
    }



    private void OnCollisionStay(Collision collision)
    {
        
    }

    void CreateWave(Collision collision)
    {
        if (collision.relativeVelocity.y > 10f || collision.relativeVelocity.y < -10f)
        {
            Debug.Log("Collision");
            int[] position = Transform2Index(collision.gameObject.transform, false);
                int x = position[0];
                int y = position[1];
            Debug.Log(collision.gameObject.transform.position.z);
            Debug.Log(x);
            Debug.Log(y);
                Wave wave = new Wave(GameController.gridX, GameController.gridY, x, y, 0, collision.relativeVelocity.y);
                GameController.WaveCollection.Add(wave);
                GameController.hasWaveSource[x * 15 + y] = true;
                
        }
    }

    int[] Transform2Index(Transform trans, bool isBall)
    {
        
        int[] result = new int[2];
        result[0] = Mathf.FloorToInt(trans.position.x);
        result[1] = Mathf.FloorToInt(trans.position.z);
        if (isBall)
        {
            result[0] = Mathf.FloorToInt(trans.position.x) + 1;
            result[1] = Mathf.FloorToInt(trans.position.z) + 1;
        }
        return result;
    }

    void SnapGrid(Transform trans)
    {
        snapTile = Transform2Index(trans, false);
        snapped = true;
    }

}

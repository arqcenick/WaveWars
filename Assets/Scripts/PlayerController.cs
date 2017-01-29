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
    float jumpCons = 700f;
    // Use this for initialization

    AudioSource[] sources;
    Renderer rend;
    int playerId;
    public AudioClip JumpClip;
    public AudioClip JumpKickClip;
    public static GameObject Wall;

    private void Awake()
    {
        rend = transform.GetChild(0).GetComponent<Renderer>();
    }
    void Start() {
        InputsActive = true;
        movementComplete = true;
        rigid = gameObject.GetComponent<Rigidbody>();
        rigid.WakeUp();
        sources = gameObject.GetComponents<AudioSource>();
        sources[0].clip = JumpClip;
        sources[1].clip = JumpKickClip;

    }
    public bool snapped = false;
    public int[] snapTile;

    private Attacks Inventory = Attacks.None;

    KeyCode up;
    KeyCode left;
    KeyCode down;
    KeyCode right;
    KeyCode jump;
    KeyCode attack;

    bool jumping = false;
    bool attacking = false;
    public bool InputsActive;


    bool ironBall = false;
    float ironTime = 10f;
    float ironTimer = 0f;
    



    public enum Attacks
    {
        WallUp,
        IronBall,
        None
    }

    public int PlayerId
    {
        get
        {
            return playerId;
        }

        set
        {
            if (value == 0)
            {
                up = KeyCode.W;
                left = KeyCode.A;
                down = KeyCode.S;
                right = KeyCode.D;
                jump = KeyCode.J;
                attack = KeyCode.K;

                rend.material.color = Color.white;
                //Kouhai suki

            }
            else if (value == 1)
            {
                up = KeyCode.UpArrow;
                left = KeyCode.LeftArrow;
                down = KeyCode.DownArrow;
                right = KeyCode.RightArrow;
                jump = KeyCode.Keypad2;
                attack = KeyCode.Keypad3;

                rend.material.color = Color.red;
            }
            playerId = value;

        }
    }


    // Update is called once per frame
    void Update()
    {

        if (transform.position.y < -10f)
        {
            GameController.Lose(this);
            InputsActive = false;
        }
        Vector3 forceVector = Vector3.zero;

        if(InputsActive)
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
            if (Input.GetKeyDown(jump) && !jumping)
            {
                jumping = true;
                StartCoroutine("TryJump");


            }
            if (Input.GetKeyDown(attack) && !attacking)
            {

                attacking = true;
                switch (Inventory)
                {
                    case Attacks.None:
                        DiveKick();
                        break;
                    default:
                        break;
                }
            }
        }
        


        forceVector = forceVector.normalized * accCons;
        rigid.AddForce(forceVector);

        if (CheckBelow() && rigid.velocity.y > 0.0f)
        {
            for (int i = 0; i < GameController.WaveCollection.Count; i++)
            {
                Wave wave = GameController.WaveCollection[i];
                rigid.AddExplosionForce(rigid.velocity.y * wave.Force * 0.1f, GameController.posGrid[i], 100f);

            }

        }
    }


    IEnumerator TryJump()
    {
        for(int i = 0; i<20; i++)
        {
            if(CheckBelow())
            {
                rigid.AddForce(Vector3.up * jumpCons);
                int[] position = Transform2Index(transform, true);
                int x = position[0];
                int y = position[1];
                GameController.hitCount[x * 15 + y] -= 1;
                i = 20;
                Debug.Log("jumping");
                
                sources[0].Play();
                
                
            }
  
            
            yield return null;
        }
        jumping = false;

    }

    IEnumerator TryDiveKick()
    {
        float magnitude = 600f;
        for (int i = 0; i < 10; i++)
        {
            attacking = false;
            if (!CheckBelow())
            {
                
                Vector3 direction = (GameController.GetOtherPlayer(playerId).transform.position - transform.position).normalized;
                rigid.AddForce(direction * magnitude);
                attacking = true;
                i = 10;
                sources[1].Play();

            }
            yield return null;
        }
        
    }



    bool CheckBelow()
    {
        
        Ray ray1 = new Ray(transform.position, Vector3.down + Vector3.right * 0.3f);
        Ray ray2 = new Ray(transform.position, Vector3.down + Vector3.left * 0.3f);
        Ray ray3 = new Ray(transform.position, Vector3.down + Vector3.forward * 0.3f);
        Ray ray4 = new Ray(transform.position, Vector3.down + Vector3.back * 0.3f);
        //Debug.DrawRay(transform.position, Vector3.down + Vector3.forward *0.5f, Color.white,10f);
        if (Physics.Raycast(ray1, 0.7f) || Physics.Raycast(ray2, 0.7f) || Physics.Raycast(ray3, 0.7f) || Physics.Raycast(ray4, 0.7f))
        {
            attacking = false;
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
        else if(collision.gameObject.tag == "Tile")
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
            int[] position = Transform2Index(collision.gameObject.transform, false);
                int x = position[0];
                int y = position[1];
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
        result[0] = Mathf.Max(result[0], 0);
        result[1] = Mathf.Max(result[1], 0);

        result[0] = Mathf.Min(result[0], 14);
        result[1] = Mathf.Min(result[1], 14);
        return result;
    }

    void SnapGrid(Transform trans)
    {
        snapTile = Transform2Index(trans, false);
        snapped = true;
    }

    public bool PickUpItem(Attacks item)
    {
        Inventory = item;
        return true;
    }

    void WallUp(Vector3 forceVector)
    {
        
        if(forceVector.magnitude > 0)
        {
            Vector3 pos = transform.position + forceVector.normalized*3f;

            float angle = Vector3.Angle(Vector3.forward, forceVector);
            float angle2 = Vector3.Angle(Vector3.right, forceVector);
            int sign = angle2 == angle ? 1 : -1;
            if (angle < 90)
                Instantiate(Wall, pos, Quaternion.AngleAxis(sign * angle, Vector3.up));
            else if(angle >= 90)
            {
                if(angle2 == 180)
                    Instantiate(Wall, pos, Quaternion.AngleAxis(sign * angle, Vector3.up));
                else
                    Instantiate(Wall, pos, Quaternion.AngleAxis(sign * angle, Vector3.down));
            }
                
        }

    }

    void DiveKick()
    {
        StartCoroutine("TryDiveKick");
        
    }


    

}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {


    public GameObject PreTile;
    public GameObject Player;
    public static GameObject Player1;
    public static int gridX = 15;
    public static int gridY = 15;
    public static int[] hitCount;

    public static Vector3[] posGrid;
    public static float[] phaseGrid;
    public static bool[] hasWaveSource;
    float[] posX;
    float[] posY;

    public static bool GameEnd = false;

    static PlayerController[] playerControls;


    GameObject[] grid;
    Vector2[] playerGrid;
    Vector2[] spawnPos;
    Rigidbody[] rigids;
    BoxCollider[] colliders;

    float timeStep = 0f;

    public static List<Wave> WaveCollection;

    // Use this for initialization

    private void Awake()
    {
        playerControls = new PlayerController[2];
    }

    void Start () {

        WaveCollection = new List<Wave>();
        
         Wave wave1 = new Wave(gridX, gridY, 7, 7, 0f, 10f);
         //WaveCollection.Add(wave1);
        CreateGrid();
        spawnPos = new Vector2[2];
        spawnPos[0] = playerGrid[0];
        spawnPos[1] = playerGrid[playerGrid.Length-1];

        CreatePlayer(0);
        CreatePlayer(1);


    }

    
	
	// Update is called once per frame
	void Update () {

        timeStep += Time.deltaTime;
        
        UpdatePositions(WaveCollection);

        if(timeStep > 4f)
        {
            timeStep = 0;
            RandomLoss();
        }
        
        

	}

    void CreateGrid()
    {
        grid = new GameObject[gridX * gridY ];
        phaseGrid = new float[gridX * gridY];
        posGrid = new Vector3[gridX * gridY];
        playerGrid = new Vector2[gridX * gridY];
        hasWaveSource = new bool[gridX * gridY];
        hitCount = new int[gridX * gridY];
        rigids = new Rigidbody[gridX * gridY];
        colliders = new BoxCollider[gridX * gridY];

        for (int x = 0; x < gridX; x++)
            for(int y = 0; y < gridY; y++)
            {
                grid[x * gridX + y] = (Instantiate(PreTile, new Vector3(x * 1f, 0f, y * 1f), transform.rotation) as GameObject);
                posGrid[x * gridX + y] = grid[x * gridX + y].transform.position;
                rigids[x * gridX + y] = grid[x * gridX + y].GetComponent<Rigidbody>();
                colliders[x * gridX + y] = grid[x * gridX + y].transform.GetChild(0).GetComponent<BoxCollider>();
            }

        for (int x = 0; x < gridX; x++)
            for (int y = 0; y < gridY; y++)
            {
                playerGrid[x * gridX + y] = new Vector2(x - 0.5f, y - 0.5f);
                hasWaveSource[x * gridX + y] = false;
                phaseGrid[x * gridX + y] = 0f;
                hitCount[x * gridX + y] = 1;
            }


    }

    
    void CreatePlayer(int id)
    {
        float x = spawnPos[id].x;
        float z = spawnPos[id].y;
        playerControls[id] = (Instantiate(Player, new Vector3(x,4.05f,z), Player.transform.rotation) as GameObject).GetComponent<PlayerController>();
        if(id == 0)
            CameraControl.Player1 = playerControls[id].gameObject;
        else
            CameraControl.Player2 = playerControls[id].gameObject;
        playerControls[id].PlayerId = id;  
    }

    public static void Lose(PlayerController player)
    {
        GameEnd = true;
        CameraControl.Winner = playerControls[1-player.PlayerId].gameObject;
        { 
}
    }

    void UpdatePositions(List<Wave> waveCollection)
    {
        float[] updates = new float[gridX * gridY];

        for (int i = 0; i < waveCollection.Count; i++)
        {
            Wave wave = waveCollection[i];
            if(wave.FinishWave)
            {
                hasWaveSource[wave.sourceX * gridX + wave.sourceY] = false;
                waveCollection.Remove(wave);
                
            }
                
        }

         for (int i = 0; i < waveCollection.Count; i++)
        {
            Wave wave = waveCollection[i];
            updates =  GridSum(updates, wave.WaveCalculate());
            for (int x = 0; x < gridX; x++)
                for (int y = 0; y < gridY; y++)
                {
                    if (hitCount[x * gridX + y] > 0)
                    {
                        Vector3 pos = grid[x * gridX + y].transform.position;
                    pos[1] = updates[x * gridX + y];
                    grid[x * gridX + y].transform.position = pos;

                    posGrid[x * gridX + y] = pos;
                    
                    
                    }
                    else
                    {
                        rigids[x * gridX + y].constraints = RigidbodyConstraints.FreezeRotation;
                        colliders[x * gridX + y].isTrigger = true;  
                    }


                }
          
        }
    }

    float[] GridSum(float[] gridOne, float[] gridTwo)
    {
        for(int i=0; i < grid.Length; i++)
        {
            gridTwo[i] += gridOne[i];
        }
        return gridTwo;

    }

    void RandomLoss()
    {
        // Kouhai!!!

    }





}

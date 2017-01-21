using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {


    public GameObject PreTile;
    public GameObject Player;
    public static GameObject Player1;
    public static int gridX = 15;
    public static int gridY = 15;

    float[] posGrid;
    public static float[] phaseGrid;
    public static bool[] hasWaveSource;
    float[] posX;
    float[] posY;

    public static bool GameEnd = false;

    static PlayerController[] playerControls;


    GameObject[] grid;
    Vector2[] playerGrid;
    Vector2[] spawnPos;

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
        
        

	}

    void CreateGrid()
    {
        grid = new GameObject[gridX * gridY ];
        phaseGrid = new float[gridX * gridY];
        posGrid = new float[gridX * gridY];
        playerGrid = new Vector2[gridX * gridY];
        hasWaveSource = new bool[gridX * gridY];

        for (int x = 0; x < gridX; x++)
            for(int y = 0; y < gridY; y++)
            {
                grid[x * gridX + y] = (Instantiate(PreTile, new Vector3(x * 1f, 0f, y * 1f), transform.rotation) as GameObject);
            }

        for (int x = 0; x < gridX; x++)
            for (int y = 0; y < gridY; y++)
            {
                playerGrid[x * gridX + y] = new Vector2(x - 0.5f, y - 0.5f);
            }

        for (int x = 0; x < gridX; x++)
            for (int y = 0; y < gridY; y++)
            {
                hasWaveSource[x * gridX + y] = false;
            }
        for (int x = 0; x < gridX; x++)
            for (int y = 0; y < gridY; y++)
            {
                phaseGrid[x * gridX + y] = 0f;
            }


    }

    
    void CreatePlayer(int id)
    {
        float x = spawnPos[id].x;
        float z = spawnPos[id].y;
        playerControls[id] = (Instantiate(Player, new Vector3(x,4.01f,z), Player.transform.rotation) as GameObject).GetComponent<PlayerController>();
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
                    Vector3 pos = grid[x * gridX + y].transform.position;
                    pos[1] = updates[x * gridX + y];
                    grid[x * gridX + y].transform.position = pos;


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




}

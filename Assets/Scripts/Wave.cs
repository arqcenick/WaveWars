using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave {


    int gridX, gridY;
    public int sourceX, sourceY;
    public float phase, Force;
    float phaseStat;
    float magnitude = 0;
    float finalmagnitude = 1f;
    float initTime = 0;
    public bool FinishWave = false;
    float speed = 4f;
    
    public Wave(int gridX, int gridY, int sourceX, int sourceY, float phase, float force)
    {
        this.gridX = gridX;
        this.gridY = gridY;
        this.sourceX = sourceX;
        this.sourceY = sourceY;
        this.phase = phase;
        this.Force = force;
        phaseStat = phase;
    }

    private void SimpleWave(float[] updatePosGrid)
    {
        for (int x = 0; x < gridX; x++)
            for (int y = 0; y < gridY; y++)
            {
                float dist = Mathf.Sqrt((x - sourceX) * (x - sourceX) + (y - sourceY) * (y - sourceY));
                //Wave equation with dampening over distance


                updatePosGrid[x * gridX + y] = Mathf.Sin((dist - 1 * initTime)) / (dist + 4f) * Force / 10f * magnitude * 1f;


            }
    }

    private void DirectionalWave(float[] updatePosGrid)
    {
        for (int x = 0; x < gridX; x++)
            for (int y = 0; y < gridY; y++)
            {
                int distX = x - sourceX;
                int distY = y - sourceY;
                if(distX >= y/3 && distY >= x/3)
                {
                    float dist = Mathf.Sqrt((x - sourceX) * (x - sourceX) + (y - sourceY) * (y - sourceY));
                    //Wave equation with dampening over distance


                    updatePosGrid[x * gridX + y] = Mathf.Sin((dist - 1 * initTime)/2f) / (dist + 3f) * Force / 10f * magnitude * 2f;
                }
                


            }
    }

    public float[] WaveCalculate()
    {
        
        float[] updatePosGrid = new float[gridX * gridY];
        bool[] propagated = new bool[gridX * gridY];


        SimpleWave(updatePosGrid);
        //DirectionalWave(updatePosGrid);

        magnitude = Mathf.Lerp(magnitude, finalmagnitude, 0.05f);
        initTime += Time.deltaTime*speed;
        if(initTime > 6f*speed)
        {
            finalmagnitude *=0.99f;
        }
        if(finalmagnitude < 0.01f)
        {
            FinishWave = true;
        }
        return updatePosGrid;
    }
}

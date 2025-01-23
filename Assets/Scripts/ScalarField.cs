using System;
using UnityEngine;

public class ScalarField 
{
    
    public static GridPoint[,,] GenerateGridPoints(Vector3Int GridSize, GridPoint[,,] gridPoints,float frequency, int octaves, Vector3 offsets, float gain, float lacunarity, int seed)
    {
        gridPoints = new GridPoint[GridSize.x,GridSize.y,GridSize.z];

        System.Random r = new System.Random(seed);
        int Nseed = r.Next(-100000, 100000);

        if (frequency < 0)
            frequency = 0.00001f;
        

        Noise.setVariables(Nseed, frequency, gain, octaves, lacunarity);
        float minValue = float.MaxValue;
        float maxValue = float.MinValue;

        for(int x = 0; x < GridSize.x; x++)
        {
            for (int y = 0; y < GridSize.y; y++)
            {
                for(int z = 0; z < GridSize.z; z++)
                {
                    gridPoints[x, y, z] = new GridPoint();
                    gridPoints[x, y, z].Position = new Vector3(x,y,z);
                        
                    gridPoints[x, y, z].Value = Noise.GenerateNoise(x + offsets.x, y + offsets.y, z + offsets.z);

                    if(gridPoints[x, y, z].Value < minValue)
                        minValue = gridPoints[x, y, z].Value;

                    if (gridPoints[x, y, z].Value > maxValue)
                        maxValue = gridPoints[x, y, z].Value;
                }
            }
        }

        for(int x = 0; x < GridSize.x; x++)
        {
            for( int y = 0; y < GridSize.y; y++)
            {
                for( int z = 0; z < GridSize.z; z++)
                {
                    gridPoints[x, y, z].Value = (gridPoints[x, y, z].Value - minValue)/(maxValue-minValue);
                }
            }
        }

        return gridPoints;
    }
    public static void SetPointsOnGridcell(Vector3Int gridSize, ref Gridcell gridCell, GridPoint[,,] gridPoints, float surfaceLevel)
    {

        if (gridPoints == null)
            return;

        /*  vertex 8 (0-7)
              E4-------------F5         7654-3210
              |               |         HGFE-DCBA
              |               |
        H7-------------G6     |
        |     |         |     |
        |     |         |     |
        |     A0--------|----B1  
        |               |
        |               |
        D3-------------C2               */

        for (int x = 0; x < gridSize.x - 1; x++)
        {
            for (int y = 0;y < gridSize.y - 1; y++)
            {
                for(int z = 0;z < gridSize.z - 1; z++)
                {
                    gridCell.p[0] = gridPoints[x, y, z + 1]; // A0
                    gridCell.p[1] = gridPoints[x + 1, y, z + 1]; //B1
                    gridCell.p[2] = gridPoints[x + 1, y, z]; //C2
                    gridCell.p[3] = gridPoints[x, y, z]; //D3
                    gridCell.p[4] = gridPoints[x, y + 1, z + 1]; //E4
                    gridCell.p[5] = gridPoints[x + 1, y + 1, z + 1]; //F5
                    gridCell.p[6] = gridPoints[x + 1, y + 1, z]; //G6
                    gridCell.p[7] = gridPoints[x, y + 1, z]; //H7

                    IsoFaces.CalcIsoFaces(ref gridCell, surfaceLevel);
                }
            }
        }

    }

}

public class Gridcell
{
    public GridPoint[] p = new GridPoint[8];
    public int config = 0;
    public int numTriangles = 0;
    public Vector3[] edgepoints = new Vector3[12];
    public Triangle[] triangles = new Triangle[5];

    public void clearCalculation()
    {
        config = 0;
        numTriangles = 0;

        for (int i = 0; i < triangles.Length; i++)
        {
            if(triangles[i] == null)
            {
                triangles[i] = new Triangle();
            }
            triangles[i].clear();
        }

        Array.Fill(edgepoints, Vector3.zero);
    }
}

public class GridPoint
{
    public Vector3 Position { get; set; }
    public float Value { get; set; }

    public void debugInfo()
    {
        Debug.Log($"Valor del gridPoint: {Value}");
        Debug.Log($"posicion del gridPoint: {Position}");
    }
}
public class Triangle
{
    public Vector3[] p = new Vector3[3];

    public Triangle()
    {
        clear();
    }

    public void clear()
    {
        p[0] = Vector3.zero;
        p[1] = Vector3.zero;
        p[2] = Vector3.zero;
    }
}
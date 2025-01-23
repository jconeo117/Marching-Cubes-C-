using UnityEngine;

public class IsoFaces 
{
    
    public static void CalcIsoFaces(ref Gridcell gridcell, float surfaceLevel)
    {
        gridcell.clearCalculation();

        gridcell.config = 0;



        for(int i = 0; i < gridcell.p.Length; i++)
        {
            if (gridcell.p[i] != null && gridcell.p[i].Value < surfaceLevel)
                Bits.SetBit(ref gridcell.config, i);
        }

        if (MarchingCubesTables.edgeTable[gridcell.config] == 0) return;

        if (Bits.IsSet(MarchingCubesTables.edgeTable[gridcell.config], 0))
            gridcell.edgepoints[0] = InterpolateEdgePosition(surfaceLevel, gridcell.p[0], gridcell.p[1]);
        if (Bits.IsSet(MarchingCubesTables.edgeTable[gridcell.config], 1))
            gridcell.edgepoints[1] = InterpolateEdgePosition(surfaceLevel, gridcell.p[1], gridcell.p[2]);
        if (Bits.IsSet(MarchingCubesTables.edgeTable[gridcell.config], 2))
            gridcell.edgepoints[2] = InterpolateEdgePosition(surfaceLevel, gridcell.p[2], gridcell.p[3]);
        if (Bits.IsSet(MarchingCubesTables.edgeTable[gridcell.config], 3))
            gridcell.edgepoints[3] = InterpolateEdgePosition(surfaceLevel, gridcell.p[3], gridcell.p[0]);
        if (Bits.IsSet(MarchingCubesTables.edgeTable[gridcell.config], 4))
            gridcell.edgepoints[4] = InterpolateEdgePosition(surfaceLevel, gridcell.p[4], gridcell.p[5]);
        if (Bits.IsSet(MarchingCubesTables.edgeTable[gridcell.config], 5))
            gridcell.edgepoints[5] = InterpolateEdgePosition(surfaceLevel, gridcell.p[5], gridcell.p[6]);
        if (Bits.IsSet(MarchingCubesTables.edgeTable[gridcell.config], 6))
            gridcell.edgepoints[6] = InterpolateEdgePosition(surfaceLevel, gridcell.p[6], gridcell.p[7]);
        if (Bits.IsSet(MarchingCubesTables.edgeTable[gridcell.config], 7))
            gridcell.edgepoints[7] = InterpolateEdgePosition(surfaceLevel, gridcell.p[7], gridcell.p[4]);
        if (Bits.IsSet(MarchingCubesTables.edgeTable[gridcell.config], 8))
            gridcell.edgepoints[8] = InterpolateEdgePosition(surfaceLevel, gridcell.p[0], gridcell.p[4]);
        if (Bits.IsSet(MarchingCubesTables.edgeTable[gridcell.config], 9))
            gridcell.edgepoints[9] = InterpolateEdgePosition(surfaceLevel, gridcell.p[1], gridcell.p[5]);
        if (Bits.IsSet(MarchingCubesTables.edgeTable[gridcell.config], 10))
            gridcell.edgepoints[10] = InterpolateEdgePosition(surfaceLevel, gridcell.p[2], gridcell.p[6]);
        if (Bits.IsSet(MarchingCubesTables.edgeTable[gridcell.config], 11))
            gridcell.edgepoints[11] = InterpolateEdgePosition(surfaceLevel, gridcell.p[3], gridcell.p[7]);
        
        for(int i = 0; MarchingCubesTables.triangleTable[gridcell.config,i] != -1; i +=3 )
        {
            gridcell.triangles[gridcell.numTriangles].p[0] = gridcell.edgepoints[MarchingCubesTables.triangleTable[gridcell.config, i]];
            gridcell.triangles[gridcell.numTriangles].p[1] = gridcell.edgepoints[MarchingCubesTables.triangleTable[gridcell.config, i + 1]];
            gridcell.triangles[gridcell.numTriangles].p[2] = gridcell.edgepoints[MarchingCubesTables.triangleTable[gridcell.config, i + 2]];
            gridcell.numTriangles++;
        }
    }

    public static Vector3 InterpolateEdgePosition(float surfaceLevel, GridPoint V1, GridPoint V2)
    {
        Vector3 EdgePoint = Vector3.zero;

        if (Mathf.Approximately(surfaceLevel - V1.Value, 0)) return V1.Position;
        if (Mathf.Approximately(surfaceLevel - V2.Value, 0)) return V2.Position;
        if (Mathf.Approximately(V1.Value - V2.Value, 0)) return V1.Position;

        float Mu = (surfaceLevel - V1.Value) / (V2.Value - V1.Value);

        EdgePoint.x = V1.Position.x + Mu * (V2.Position.x - V1.Position.x);
        EdgePoint.y = V1.Position.y + Mu * (V2.Position.y - V1.Position.y);
        EdgePoint.z = V1.Position.z + Mu * (V2.Position.z - V1.Position.z);

        return EdgePoint;
    }
}

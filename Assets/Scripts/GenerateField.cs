using UnityEngine;

public class GenerateField : MonoBehaviour
{
    [SerializeField]
    [Header("Field settings")]
    public Vector3Int GridSize = new Vector3Int(24, 12, 24);
    public Vector3 Offsets = new Vector3(1, 1, 1);

    [Header("Noise settings")]
    public int seed;
    public float gain;

    [Range(0f, 1f)]
    public float frequency;
    [Range(1, 8)]
    public int octaves;
    public float lacunarity;

    public bool autoUpdate;

    [Header("Marching Cubes Settings")]
    [Range(0f, 1f)]
    public float surfaceLevel;
    private GridPoint[,,] gridPoints;
    private Gridcell gridCell = new Gridcell();

    private void Start()
    {
        FieldGenerator();
    }
    public void FieldGenerator()
    {

        if (GridSize.x <= 0)
            GridSize.x = 1;
        if (GridSize.y <= 0)
            GridSize.y = 1;
        if (GridSize.z <= 0)
            GridSize.z = 1;

        gridPoints = new GridPoint[GridSize.x, GridSize.y, GridSize.z];
        gridPoints = ScalarField.GenerateGridPoints(GridSize, gridPoints, frequency, octaves, Offsets, gain, lacunarity, seed);

        SetPointsOnCell();
    }

    public void SetPointsOnCell()
    {
        ScalarField.SetPointsOnGridcell(GridSize, ref gridCell, gridPoints, surfaceLevel);

        Debug.Log($"Numero de triangulos: {gridCell.numTriangles}");
    }

    private void OnDrawGizmos()
    {
        if (gridPoints == null)
            return;

        for(int x = 0; x < GridSize.x; x++)
        {
            for(int y = 0; y < GridSize.y; y++)
            {
                for (int z = 0; z < GridSize.z; z++)
                {
                    Gizmos.color = Color.Lerp(Color.white, Color.black, gridPoints[x, y, z].Value);
                    Gizmos.DrawSphere(gridPoints[x ,y ,z].Position, 0.15f);
                }
            }
        }
    }
}

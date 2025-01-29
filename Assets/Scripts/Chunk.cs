using UnityEngine;

public class Chunk : MonoBehaviour
{
    public NoiseGenerator NoiseGenerator;
    float[] _Weights;

    public MeshFilter MeshFilter;

    public ComputeShader MarchingShader;

    [SerializeField, Range(0f,1f)]
    public float isoLevel;

    ComputeBuffer _trianglesBuffer;
    ComputeBuffer _trianglescountBuffer;
    ComputeBuffer _weightsBuffer;

    struct Triangle
    {
        public Vector3 a;
        public Vector3 b;
        public Vector3 c;

        public static int SizeOf => sizeof(float) * 3 * 3;
    }
    private void Awake()
    {
        CreateBuffers();
    }

    private void OnDestroy()
    {
        ReleaseBuffers();
        if (MeshFilter.sharedMesh != null)
        {
            Destroy(MeshFilter.sharedMesh);
        }
    }

    void Start()
    {
        if(NoiseGenerator == null )
            Debug.Log("NoiseGenerator NO se ha instanciado correctamente");
        
        _Weights = NoiseGenerator.GetNoise();
        MeshFilter.sharedMesh = ConstructMesh();
    }

    void CreateBuffers()
    {
        ReleaseBuffers();

        _trianglesBuffer = new ComputeBuffer(5 * (GridMetrics.PointsPerChunks * GridMetrics.PointsPerChunks * GridMetrics.PointsPerChunks), Triangle.SizeOf , ComputeBufferType.Append);
        _trianglescountBuffer = new ComputeBuffer(1, sizeof(int), ComputeBufferType.Raw);
        _weightsBuffer = new ComputeBuffer(GridMetrics.PointsPerChunks * GridMetrics.PointsPerChunks * GridMetrics.PointsPerChunks, sizeof(float));
    }

    void ReleaseBuffers()
    {
        if(_trianglesBuffer != null)
        {
            _trianglesBuffer.Release();
            _trianglesBuffer = null;
        }
        if(_weightsBuffer != null)
        {
            _weightsBuffer.Release();
            _weightsBuffer = null;
        }
        if (_trianglescountBuffer != null)
        {
            _trianglescountBuffer.Release();
            _trianglescountBuffer = null;
        }
    }

    Mesh ConstructMesh()
    {

        if (MeshFilter.sharedMesh != null)
        {
            Destroy(MeshFilter.sharedMesh);
        }

        // Asegúrate de reiniciar los buffers antes de usarlos
        
        MarchingShader.SetBuffer(0,"_weights", _weightsBuffer);
        MarchingShader.SetBuffer(0, "_triangles", _trianglesBuffer);

        MarchingShader.SetInt("_chunksize", GridMetrics.PointsPerChunks);
        MarchingShader.SetFloat("_isolevel", isoLevel);

        _trianglesBuffer.SetCounterValue(0);
        _weightsBuffer.SetData(_Weights);

        MarchingShader.Dispatch(0,
            GridMetrics.PointsPerChunks / GridMetrics.numThreads,
            GridMetrics.PointsPerChunks / GridMetrics.numThreads,
            GridMetrics.PointsPerChunks / GridMetrics.numThreads
            );

        Debug.Log($"Triangulos generados: {ReadTriangleCount()}");
        Triangle[] triangles = new Triangle[ReadTriangleCount()];
        _trianglesBuffer.GetData(triangles);

        return createMeshFromTriangles(triangles);
    }

    int ReadTriangleCount()
    {
        int[] Tricount = { 0 };
        ComputeBuffer.CopyCount(_trianglesBuffer, _trianglescountBuffer, 0);
        _trianglescountBuffer.GetData(Tricount);
        return Tricount[0];
    }

    Mesh createMeshFromTriangles(Triangle[] triangles)
    {
        Vector3[] Vertex = new Vector3[triangles.Length * 3];
        int[] tris = new int[triangles.Length * 3];

        for(int i = 0;  i < triangles.Length; i++)
        {
            int startIndex = i * 3;

            Vertex[startIndex] = triangles[i].a;
            Vertex[startIndex + 1] = triangles[i].b;
            Vertex[startIndex + 2] = triangles[i].c;

            tris[startIndex] = startIndex;
            tris[startIndex + 1] = startIndex + 1;
            tris[startIndex + 2] = startIndex + 2;
        }

        Mesh mesh = new Mesh();
        mesh.vertices = Vertex;
        mesh.triangles = tris;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        Debug.Log($"conteo de vertices: {mesh.vertexCount}");
        return mesh;
    }
}

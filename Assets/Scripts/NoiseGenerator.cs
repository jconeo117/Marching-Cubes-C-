using UnityEngine;

public class NoiseGenerator : MonoBehaviour
{
    ComputeBuffer _weightsBuffer;
    public ComputeShader NoiseShader;


    //Noise settings

    [Header("Noise Settings")]
    [SerializeField]public float _NoiseScale = 1f;
    [SerializeField] public float _Frequency = .005f;
    [SerializeField] public float _Amplitude = 5f;
    [SerializeField] public int _Octaves = 8;
    [SerializeField, Range(0f, 1f)] public float _GroundLevel = 0.2f;
    [SerializeField] public int _Seed = 1;
    [SerializeField] public bool autoUpdate;

    private void Awake()
    {
        CreateBuffer();
    }

    private void OnDestroy()
    {
        ReleaseBuffer();
    }

    void CreateBuffer()
    {
        _weightsBuffer = new ComputeBuffer(
            GridMetrics.PointsPerChunks * GridMetrics.PointsPerChunks * GridMetrics.PointsPerChunks, 
            sizeof(float)
            );
    }

    void ReleaseBuffer()
    {
        _weightsBuffer.Release();
    }

    //Funcion para la generacion de los valores usando Ruido
    public float[] GetNoise()
    {
        //declaramos la variable que almacenara estos valores.
        //declaramos un array 1D, ya que nuestro Compute Shader devolvera un array 1D
        float[] NoiseValue = 
            new float[GridMetrics.PointsPerChunks * GridMetrics.PointsPerChunks * GridMetrics.PointsPerChunks];

        //seteamos el buffer al Noiseshader que sera la conexion entre nuestra CPU y GPU
        NoiseShader.SetBuffer(0, "_weights", _weightsBuffer);


        //Seteamos las variables del Compute Shader
        NoiseShader.SetInt("_ChunkSize", GridMetrics.PointsPerChunks);
        NoiseShader.SetFloat("_NoiseScale", _NoiseScale);
        NoiseShader.SetFloat("_Amplitude", _Amplitude);
        NoiseShader.SetFloat("_Frequency", _Frequency);
        NoiseShader.SetInt("_Octaves", _Octaves);
        NoiseShader.SetInt("_Seed", _Seed);
        NoiseShader.SetFloat("_GroundPercent", _GroundLevel);

        //Despachamos el Compute Shader.
        NoiseShader.Dispatch(0,
            GridMetrics.PointsPerChunks / GridMetrics.numThreads,
            GridMetrics.PointsPerChunks / GridMetrics.numThreads,
            GridMetrics.PointsPerChunks / GridMetrics.numThreads);

        //Obtenemoslos valores y los almacenamos en nuestro array.

        _weightsBuffer.GetData(NoiseValue);
        //retornamos los valores obtenidos.
        return NoiseValue;
    }
}

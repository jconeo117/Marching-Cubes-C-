using UnityEngine;

public class Chunk : MonoBehaviour
{
    public NoiseGenerator NoiseGenerator;
    float[] _Weights;

    void Start()
    {
        if(NoiseGenerator == null )
            Debug.Log("NoiseGenerator NO se ha instanciado correctamente");
        
        _Weights = NoiseGenerator.GetNoise();

        //_Weights = new float[GridMetrics.PointsPerChunks * GridMetrics.PointsPerChunks * GridMetrics.PointsPerChunks];
        //for(int i = 0; i< _Weights.Length; i++)
        //{
        //    _Weights[i] = Random.value;
        //}
    }

    private void OnDrawGizmos()
    {
        if(_Weights == null || _Weights.Length == 0)
        {
            return;
        } 

        for(int x = 0; x < GridMetrics.PointsPerChunks; x++)
        {
            for(int y = 0; y < GridMetrics.PointsPerChunks; y++)
            {
                for(int z = 0; z< GridMetrics.PointsPerChunks; z++)
                {
                    int index = x + GridMetrics.PointsPerChunks * (y + GridMetrics.PointsPerChunks * z);
                    float Value = _Weights[index];

                    Gizmos.color = Color.Lerp(Color.white, Color.black, Value);
                    Gizmos.DrawCube(new Vector3(x, y, z), Vector3.one * .2f);
                }
            }
        }
    }
}

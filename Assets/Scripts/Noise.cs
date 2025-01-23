using Unity.Mathematics;
using UnityEngine;

public static class Noise 
{
    private static FastNoiseLite noise;

    static Noise()
    {
        noise = new FastNoiseLite();
        noise.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2S);
        noise.SetFractalType(FastNoiseLite.FractalType.FBm);
    }

    public static float GenerateNoise(float x, float y, float z)
    {
        return noise.GetNoise(x, y, z);
    } 

    public static void setVariables(int seed, float frequency, float gain, int octaves, float lacunarity)
    {
        noise.SetSeed(seed);
        noise.SetFrequency(frequency);
        noise.SetFractalGain(gain);
        noise.SetFractalLacunarity(lacunarity);
        noise.SetFractalOctaves(octaves);
    }
}

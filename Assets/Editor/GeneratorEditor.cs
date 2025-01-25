using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(NoiseGenerator))]
public class GeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
       NoiseGenerator NoiseSettings = (NoiseGenerator)target;

        if (DrawDefaultInspector())
        {
            if (NoiseSettings.autoUpdate)
            {
                NoiseSettings.GetNoise();
            }
        }

        if (GUILayout.Button("Generate"))
        {
            NoiseSettings.GetNoise();
        }
    }
}

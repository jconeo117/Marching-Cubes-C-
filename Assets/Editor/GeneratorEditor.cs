using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GenerateField))]
public class GeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
       GenerateField FieldGen = (GenerateField)target;

        if (DrawDefaultInspector())
        {
            if (FieldGen.autoUpdate)
            {
                FieldGen.FieldGenerator();
            }
        }

        if (GUILayout.Button("Generate"))
        {
            FieldGen.FieldGenerator();
        }
    }
}

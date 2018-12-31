using Client.Scripts.MonoBehaviours;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(MonoNoiseTester))]
public class MonoNoiseTesterEditor : Editor
{
    public override void OnInspectorGUI()
    {
        MonoNoiseTester noise = (MonoNoiseTester)target;
        if (DrawDefaultInspector() && noise.AutoUpdate)
        {
            noise.StartDraw();
        }
        else if (!noise.AutoUpdate)
        {
            noise.StopDraw();
        }
        if (GUILayout.Button("Generate"))
        {
            noise.GenerateMap();
        }
        else if (GUILayout.Button("Randomize Seed"))
        {
            noise.RandomizeSeed();
        }
        else if (GUILayout.Button("Randomize"))
        {
            noise.Randomize();
        }
    }
}

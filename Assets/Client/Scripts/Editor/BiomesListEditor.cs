using Client.ScriptableObjects;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(BiomesGeneratorObject))]
public class BiomesListEditor : PropertyDrawer
{
    public float Height = 0;
    
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.PrefixLabel(position, label);
        Rect newposition = position;
        newposition.y += 18f;
        SerializedProperty data = property.FindPropertyRelative("Data");
        for (int j = 0; j < data.arraySize; j++)
        {
            SerializedProperty row = data.GetArrayElementAtIndex(j).FindPropertyRelative("Row");
            newposition.height = 18f;
            if (row.arraySize != 7)
                row.arraySize = 7;
            newposition.width = position.width / 7;
            for (int i = 0; i < 7; i++)
            {
                EditorGUI.PropertyField(newposition, row.GetArrayElementAtIndex(i), GUIContent.none);
                newposition.x += newposition.width;
            }

            newposition.x = position.x;
            newposition.y += 18f;
        }
        Height = newposition.y - position.y;
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return Height;
    }
}
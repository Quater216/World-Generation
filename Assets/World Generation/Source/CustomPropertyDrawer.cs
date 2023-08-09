using Model;
using UnityEditor;
using UnityEngine;

namespace Source
{
    [UnityEditor.CustomPropertyDrawer(typeof(BiomeTable))]
    public class CustomPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.PrefixLabel(position, label);
            
            var newPosition = position;
            newPosition.y += 18f;
            
            var data = property.FindPropertyRelative("Rows");

            for (int j = 0; j < data.arraySize; j++)
            {
                var row = data.GetArrayElementAtIndex(j).FindPropertyRelative("Row");
                
                newPosition.height = 18f;

                if (row.arraySize == data.arraySize == false)
                    row.arraySize = data.arraySize;
                
                newPosition.width = position.width / 7;
                
                for (int i = 0; i < row.arraySize; i++)
                {
                    EditorGUI.PropertyField(newPosition, row.GetArrayElementAtIndex(i), GUIContent.none);
                    newPosition.x += newPosition.width;
                }

                newPosition.x = position.x;
                newPosition.y += 18f;
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return 18f * 8;
        }
    }
}
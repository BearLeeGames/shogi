using UnityEngine;
using UnityEditor;



/// <summary>
/// Displays a field as ReadOnly in the inspector.
/// </summary>
public class ReadOnlyAttribute : PropertyAttribute
{

}

[CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
public class ReadOnlyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        string varString;

        switch (property.propertyType)
        {
            case SerializedPropertyType.Integer:
                varString = property.intValue.ToString();
                break;

            case SerializedPropertyType.Boolean:
                varString = property.boolValue.ToString();
                break;

            case SerializedPropertyType.Float:
                varString = property.floatValue.ToString("0.00000");
                break;

            case SerializedPropertyType.String:
                varString = property.stringValue;
                break;

            default:
                varString = "[TYPE NOT SUPPORTED]";
                break;
        }

        EditorGUI.LabelField(position, label.text, varString);
    }
}

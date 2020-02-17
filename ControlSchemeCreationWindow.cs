using UnityEngine;
using UnityEditor;

/// <summary>
/// This custom editor allows for the creation of new control schemes.
/// The data is stored in scriptable object assets, filepath: Assets/ScriptableObjects/ControlSchemes
/// These scriptable object assets can then be assigned within the inspector, allowing for alternative controls without requiring any code to be changed.
/// </summary>
public class ControlSchemeCreationWindow : EditorWindow
{
    string schemeName = "Scheme Name";
    KeyCode f;
    KeyCode b;
    KeyCode l;
    KeyCode r;
    KeyCode i;
    string filename = "filename";

    [MenuItem("Tools/Create Control Scheme")]
    public static void DisplayWindow()
    {
        GetWindow<ControlSchemeCreationWindow>("Create Control Scheme");
    }

    private void OnGUI()
    {
        GUILayout.Label("Create a custom control scheme", EditorStyles.boldLabel);
        //String field to enter name for control scheme
        schemeName = EditorGUILayout.TextField("Name: ", schemeName);
        //Enum Fields to select KeyCodes for each movement direction/action
        f = (KeyCode)EditorGUILayout.EnumPopup("Forward Key: ", f);
        b = (KeyCode)EditorGUILayout.EnumPopup("Backwards Key: ", b);
        l = (KeyCode)EditorGUILayout.EnumPopup("Left Key: ", l);
        r = (KeyCode)EditorGUILayout.EnumPopup("Right Key: ", r);
        i = (KeyCode)EditorGUILayout.EnumPopup("Interact Key: ", i);
        //String field to enter the filename for the scriptable object asset
        filename = EditorGUILayout.TextField("Filename (no spaces): ", filename);

        //When the button is pressed, create a new control scheme with the values entered
        //And create a Scriptable Object asset to save these values.
        if (GUILayout.Button("Create Control Scheme"))
        {
            ControlScheme c = new ControlScheme(schemeName, f, b, l, r, i);
            AssetDatabase.CreateAsset(c, "Assets/ScriptableObjects/ControlSchemes/" + filename + ".asset");
        }
    }
}

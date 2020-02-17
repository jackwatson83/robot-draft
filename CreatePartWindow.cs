using UnityEngine;
using UnityEditor;

/// <summary>
/// This custom editor allows for the creation of robot parts
/// You can set the values for each of the part's stats,
/// the material the part has
/// the mesh for the part
/// the filename to store the part as (stored as scriptable object assets)
/// the part type (which slot it fits in on the robot)
/// </summary>
public class CreatePartWindow : EditorWindow
{
    string partName = "Part Name";
    PartType partType;
    Mesh partMesh;
    Material partMaterial;
    string filename = "filename";
    float partHealth = 0;
    float partArmour = 0;
    float partDamage = 0;
    float partAttackRange = 0;
    float partSpeed = 0;

    [MenuItem("Tools/Create Part")]
    public static void DisplayWindow()
    {
        GetWindow<CreatePartWindow>("Create Part");
    }

    private void OnGUI()
    {
        GUILayout.Label("Create a Scriptable Object to represent a robot part", EditorStyles.boldLabel);
        //Fields for the editor window
        partName = EditorGUILayout.TextField("Name: ", partName);
        partType = (PartType)EditorGUILayout.EnumPopup("Part Type: ", partType);
        partMesh = (Mesh)EditorGUILayout.ObjectField("Part Mesh: ", partMesh, typeof(Mesh), true);
        partMaterial = (Material)EditorGUILayout.ObjectField("Part Material: ", partMaterial, typeof(Material), true);
        partHealth = EditorGUILayout.FloatField("Part Health Modifier: ", partHealth);
        partArmour = EditorGUILayout.FloatField("Part Armour Modifier: ", partArmour);
        partDamage = EditorGUILayout.FloatField("Part Damage Modifier: ", partDamage);
        partAttackRange = EditorGUILayout.FloatField("Part Range Modifier: ", partAttackRange);
        partSpeed = EditorGUILayout.FloatField("Part Speed Modifier: ", partSpeed);
        filename = EditorGUILayout.TextField("Filename (no spaces): ", filename);

        //Create the scriptable object asset when the Create button is pressed.
        if (GUILayout.Button("Create Part"))
        {
            RobotPart p = new RobotPart(partName, partType, partMesh, partMaterial, partHealth, partArmour, partDamage, partAttackRange, partSpeed);
            AssetDatabase.CreateAsset(p, "Assets/ScriptableObjects/RobotParts/" + filename + ".asset");
        }
    }
}

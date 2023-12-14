using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(HexGridCellGenerator))]
public class HexGridGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        HexGridCellGenerator hexGridMeshGenerator = (HexGridCellGenerator)target;

        if (GUILayout.Button("Re/Generate this Grid (get new Cells)"))
        {
            hexGridMeshGenerator.ReCreateThisGrid();
        }

        if (GUILayout.Button("Reload Terrain Types"))
        {
            hexGridMeshGenerator.ReloadTerrainTypes();
        }

        if (GUILayout.Button("Recreate Cells"))
        {
            hexGridMeshGenerator.ReGenerateCells();
        }

        if (GUILayout.Button("Clear Visual Cells"))
        {
            hexGridMeshGenerator.ClearCells();
        }
    }
}

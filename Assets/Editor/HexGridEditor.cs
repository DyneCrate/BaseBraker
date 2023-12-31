using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/*
 * Draws the position/number of the cell in the editor
 */

[CustomEditor(typeof(HexGrid))]
public class HexGridEditor : Editor
{
    private void OnSceneGUI()
    {
        bool drawText = true;
        if (drawText)
        {
            HexGrid hexGrid = (HexGrid)target;
            int i = 0;
            for (int z = 0; z < hexGrid.Height; z++)
            {
                for (int x = 0; x < hexGrid.Width; x++)
                {
                    Vector3 centrePosition = HexMetrics.Center(hexGrid.HexSize, x, z, hexGrid.Orientation) + hexGrid.transform.position;

                    int centerX = x;
                    int centerZ = z;
                    
                    Vector3 cubeCoord = HexMetrics.OffsetToCube(centerX, centerZ, hexGrid.Orientation);
                    //Handles.Label(centrePosition + (Vector3.forward * 0.5f), $"[{centerX}, {centerZ}]"); // offset coordinates
                   // Handles.Label(centrePosition, $"({cubeCoord.x}, {cubeCoord.y}, {cubeCoord.z})"); //q, r, s
                    Handles.Label(centrePosition, $"({i})"); //i
                    i++;
                }
            }
        }
    }
}

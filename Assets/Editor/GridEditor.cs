using UnityEditor;
using UnityEngine;

public class GridEditor : EditorWindow
{
    #region member fields
    GameObject parent;
    //GameObject tile;
    Vector3 gridPosition;
    Vector2Int gridSize = new Vector2Int(15, 12);
    #endregion

    [MenuItem("Window / Tools / Grid Generator")]

    public static void ShowWindow()
    {
        EditorWindow window = GetWindow(typeof(GridEditor));
    }

    void OnGUI()
    {
        if (!CanShowWindow())
            return;

        SetFields();
    }

    void SetFields()
    {
        gridPosition = EditorGUILayout.Vector3Field("Grid Start Position", gridPosition);

        EditorGUILayout.Space();

        gridSize.x = Mathf.Clamp(EditorGUILayout.IntField("Width", gridSize.x), 0, 99);
        gridSize.y = Mathf.Clamp(EditorGUILayout.IntField("Length", gridSize.y), 0, 99);

        EditorGUILayout.Space(20f);

        if (GUILayout.Button("(re)Generate"))
            CreateGrid();

        if (GUILayout.Button("Reload Terrain Types"))
            CreateLadder();

        if (GUILayout.Button("re genarate cells"))
            ReGenerateCells();

    }

    /// <summary>
    /// Add the TileGenerator script to a new object with the currently selected values
    /// </summary>
    void CreateGrid()
    {
        HexGrid tg;

        AssignGridParent();

        if (!parent.GetComponent<HexGrid>())
            tg = parent.AddComponent<HexGrid>();
        else
            tg = parent.GetComponent<HexGrid>();

        tg.GenerateGrid();
    }

    void CreateLadder()
    {
        HexGrid tg;
        tg = parent.GetComponent<HexGrid>();
        tg.ReloadTerrainType();
    }

    void ReGenerateCells()
    {
        HexGrid tg;
        tg = parent.GetComponent<HexGrid>();
        tg.ReGenerateCells();
    }

    void AssignGridParent()
    {
        if (parent == null)
            parent = new GameObject("Grid");

        parent.transform.position = gridPosition;
    }

    bool CanShowWindow()
    {
        parent = (GameObject)EditorGUILayout.ObjectField("HexGrid", parent, typeof(GameObject), true);

        if (parent == null)
        {
            GUILayout.Label("Please attach a GameObject to create a grid from");
            return false;
        }

        return true;
    }
}

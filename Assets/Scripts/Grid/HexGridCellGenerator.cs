using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HexGridCellGenerator : MonoBehaviour
{
    [field: SerializeField] public HexGrid Grid { get; set; }

    private void Awake()
    {
        if (Grid == null)
            Grid = GetComponentInParent<HexGrid>();
        if (Grid == null)
            Debug.Log("HexGridMeshGenerator could not find a Hexgrid component in its parents or in itself.");
    }

    public void ReCreateThisGrid()
    {
        Grid.GenerateGrid();
    }

    public void ReloadTerrainTypes()
    {
        Grid.ReloadTerrainType();
    }

    public void ReGenerateCells()
    {
        Grid.ReGenerateCells();
    }

    public void ClearCells()
    {
        Grid.ClearCells();
    }


}

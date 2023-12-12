using System;
using System.Collections.Generic;
using UnityEngine;

public class HexCell
{
    [Header("Cell Properties")]
    [SerializeField] private HexOrientation orientation;
    [field:SerializeField] public HexGrid Grid {  get; set; }
    [field:SerializeField] public float HexSize { get; set; }
    [field:SerializeField] public TerrainType TerrainType { get; private set; }
    [field:SerializeField] public Vector2 OffsetCoordinates { get; private set; }
    [field:SerializeField] public Vector3 CubeCoordinates { get; private set; }
    [field:SerializeField] public Vector2 AxialCoordinates { get; private set; }
    [field:NonSerialized] public List<HexCell> Neighbours { get; private set; }

    private Transform terrain;

    public void SetCoordinates(Vector2 offsetCoordinates, HexOrientation orientation)
    {
        this.OffsetCoordinates = offsetCoordinates;
        this.orientation = orientation;
        this.CubeCoordinates = HexMetrics.OffsetToCube(offsetCoordinates, orientation);
        AxialCoordinates = HexMetrics.CubeToAxial(CubeCoordinates);
    }

    public void CreateTerrain()
    {
        if (TerrainType == null)
        {
            Debug.LogError("TerrainType is null");
            return;
        }
        if (Grid == null)
        {
            Debug.LogError("Grid is null");
            return;
        }
        if (HexSize == 0)
        {
            Debug.LogError("HexSize is 0");
            return;
        }
        if (TerrainType.Prefab == null)
        {
            Debug.LogError("TerrainType Prefab is null");
            return;
        }

        Vector3 centrePosition = HexMetrics.Center(
            HexSize,
            (int)OffsetCoordinates.x,
            (int)OffsetCoordinates.y, orientation
            ) + Grid.transform.position;

        terrain = UnityEngine.Object.Instantiate(
            TerrainType.Prefab,
            centrePosition,
            Quaternion.identity,
            Grid.transform
            );

        //terrain.gameObject.layer = LayerMask.NameToLayer("Grid");
        terrain.gameObject.layer = LayerMask.NameToLayer("Default");
        //TODO: Adjust the size of the prefab to the size of the grid cell

        if (orientation == HexOrientation.FlatTop)
        {
            terrain.Rotate(new Vector3(0, 30, 0));
        }
        //Temporary random rotation to make the terrain look more natural
        int randomRotation = UnityEngine.Random.Range(0, 6);
        terrain.Rotate(new Vector3(0, randomRotation * 60, 0));
    }

    public void SetTerrainType(TerrainType terrainType)
    {
        this.TerrainType = terrainType;
    }
}

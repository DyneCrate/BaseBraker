using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]
public class HexGridMeshGenerator : MonoBehaviour
{
    [field:SerializeField] public LayerMask gridLayer {  get; set; }
    [field:SerializeField] public HexGrid hexGrid { get; set; }

    private void Awake()
    {
        if (hexGrid == null)
           hexGrid = GetComponentInParent<HexGrid>();
        if (hexGrid == null)
            Debug.Log("HexGridMeshGenerator could not find a Hexgrid component in its parents or in itself.");
    }

    private void OnEnable()
    {
        MouseController.Instance.OnLeftMouseCklick += OnLeftMouseClick;
        MouseController.Instance.OnRightMouseCklick += OnRightMouseClick;
    }

    private void OnDisable()
    {
        MouseController.Instance.OnLeftMouseCklick -= OnLeftMouseClick;
        MouseController.Instance.OnRightMouseCklick -= OnRightMouseClick;
    }

    public void CreateHexMesh()
    {
        CreateHexMesh(hexGrid.Width, hexGrid.Height, hexGrid.HexSize, hexGrid.Orientation, gridLayer);
    }

    public void CreateHexMesh(HexGrid hexGrid, LayerMask layerMask)
    {
        this.hexGrid = hexGrid;
        this.gridLayer = layerMask;
        CreateHexMesh(hexGrid.Width, hexGrid.Height, hexGrid.HexSize, hexGrid.Orientation, layerMask);
    }

    public void CreateHexMesh(int width, int height, float hexSize, HexOrientation orientation, LayerMask layerMask)
    {
        ClearHexGridMesh();
        Vector3[] vertices = new Vector3[7 * width * height];

        for (int z = 0; z < height; z++)
        {
            for (int x = 0; x < width; x++)
            {
                Vector3 centrePosition = HexMetrics.Center(hexSize, x, z, orientation);
                vertices[(z * width + x) * 7] = centrePosition;
                for (int s = 0; s < HexMetrics.Corners(hexSize, orientation).Length; s++)
                {
                    vertices[(z * width + x) * 7 + s + 1] = centrePosition + HexMetrics.Corners(hexSize, orientation)[s % 6];
                }
            }
        }

        int[] triangles = new int[3 * 6 * width * height];
        for (int z = 0;z < height; z++)
        {
            for (int x = 0;x < width; x++)
            {
                for (int s = 0; s < HexMetrics.Corners(hexSize,orientation).Length; s++)
                {
                    int cornerIndex = s + 2 > 6 ? s + 2 - 6 : s + 2;
                    triangles[3 * 6 * (z * width + x) + s * 3 + 0] = (z * width + x) * 7;
                    triangles[3 * 6 * (z * width + x) + s * 3 + 1] = (z * width + x) * 7 + s + 1;
                    triangles[3 * 6 * (z * width + x) + s * 3 + 2] = (z * width + x) * 7 + cornerIndex;
                }
            }
        }

        Mesh mesh = new Mesh();
        mesh.name = "Hex Mesh";
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        mesh.Optimize();
        mesh.RecalculateUVDistributionMetrics();

        GetComponent<MeshFilter>().sharedMesh = mesh;
        GetComponent<MeshCollider>().sharedMesh = mesh;

        int gridlayerIndex = GetLayerIndex(layerMask);
        print("Layer Index: " + gridlayerIndex);

        gameObject.layer = gridlayerIndex;
    }
    public void ClearHexGridMesh()
    {
        if (GetComponent<MeshFilter>().sharedMesh == null)
            return;
        GetComponent<MeshFilter>().sharedMesh.Clear();
        GetComponent<MeshCollider>().sharedMesh.Clear();
    }

    private int GetLayerIndex(LayerMask layerMask)
    {
        int layerMaskValue = layerMask.value;
        print("Layer Mask value: " + layerMaskValue);
        for (int i = 0; i < 32; i++)
        {
            if (((1 << i) & layerMaskValue) != 0)
            {
                print("Layer Index Loop: " + i);
                return i;
            }
        }
        return 0;
    }

    private void OnLeftMouseClick(RaycastHit hit)
    {
        Debug.Log("Hit Object: " + hit.transform.name + " at position " + hit.point);
        float localX = hit.point.x - hit.transform.position.x;
        float localZ = hit.point.z - hit.transform.position.z;

        Debug.Log("Offset Position: " + HexMetrics.CoordinateToOffset(localX, localZ, hexGrid.HexSize, hexGrid.Orientation));
    }

    private void OnRightMouseClick(RaycastHit hit)
    {
        float localX = hit.point.x - hit.transform.position.x;
        float localZ = hit.point.z - hit.transform.position.z;

        Vector2 location = HexMetrics.CoordinateToOffset(localX, localZ, hexGrid.HexSize, hexGrid.Orientation);
        Vector3 center = HexMetrics.Center(hexGrid.HexSize, (int)location.x, (int)location.y, hexGrid.Orientation);
        Vector3 cube = HexMetrics.OffsetToCube(location, hexGrid.Orientation);
        Debug.Log("Right Click on hex: " + center + " cube " + cube);
    }

}

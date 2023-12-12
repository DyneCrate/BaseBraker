using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using System.Threading.Tasks;
using System.Resources;

public class HexGrid : MonoBehaviour
{
    //TODO: Add properties for grid size, hex size and hex prefab
    [field: SerializeField] public HexOrientation Orientation { get; set; }
    [field: SerializeField] public int Width { get; set; }
    [field: SerializeField] public int Height { get; set; }
    [field: SerializeField] public float HexSize { get; set; }

    [field:SerializeField] public int BatchSize { get; set; }

    [field: SerializeField] public List<TerrainType> TerrainTypes { get; set; } = new List<TerrainType>();

    [SerializeField] private List<HexCell> cells = new List<HexCell>();

    private Task<List<HexCell>> hexGenerationTask;

    //TODO Methods to get, change, add and remove hexes
    private Vector3 gridOrigin;

    public event System.Action OnMapInfoGenerated;
    public event System.Action<float> OnCellBatchgenerated;

    public event System.Action OnCellInstancesGenerated;

    private void OnDrawGizmos()
    {
        for (int z = 0; z < Height; z++)
        {
            for (int x = 0; x < Width; x++)
            {
                Vector3 centrePosition = HexMetrics.Center(HexSize, x, z, Orientation) + transform.position;
                for (int s = 0; s < HexMetrics.Corners(HexSize, Orientation).Length; s++)
                {
                    Gizmos.DrawLine(
                        centrePosition + HexMetrics.Corners(HexSize, Orientation)[s % 6],
                        centrePosition + HexMetrics.Corners(HexSize, Orientation)[(s + 1) % 6]
                        );
                }
            }
        }
    }
    public void Awake()
    {
        gridOrigin = transform.position;
    }

    private void Start()
    {
        //StartCoroutine(GenerateHexCells());
        hexGenerationTask = Task.Run(() => GenerateHexCellData());        
    }

    private void Update()
    {        
        if ( hexGenerationTask != null && hexGenerationTask.IsCompleted)
        {
            Debug.Log("generate more cells");
            cells = hexGenerationTask.Result;
            OnMapInfoGenerated?.Invoke();
            StartCoroutine(InstantiateCells());
            hexGenerationTask = null; //dispose?
        }
    }

    private List<HexCell> GenerateHexCellData()
    {
        Debug.Log("Generating Hex Cell Data");
        System.Random rng = new System.Random();
        List<HexCell> hexCells = new List<HexCell>();

        for (int z = 0; z < Height; z++)
        {
            for (int x = 0; x < Width; x++)
            {
                //Vector3 centrePosition = HexMetrics.Center(HexSize, x, z, Orientation) + gridOrigin;
                HexCell cell = new HexCell();
                cell.SetCoordinates(new Vector2(x, z), Orientation);
                cell.Grid = this;
                cell.HexSize = HexSize;
                //tmp
                int randomTerrainTypeIndex = rng.Next(0, TerrainTypes.Count);
                TerrainType terrain = TerrainTypes[randomTerrainTypeIndex];
                cell.SetTerrainType(terrain);

                hexCells.Add(cell);
            }
        }
        return hexCells;
    }

    private IEnumerator InstantiateCells()
    {
        int batchCount = 0;
        if (BatchSize == 0)
        {
            BatchSize = 1;
        }
        Debug.Log("Instantiating Hex Cells");
        int totalBatches = Mathf.CeilToInt(cells.Count / BatchSize);
        for ( int i = 0; i <cells.Count; i++)
        {
            Debug.Log("Instantiating Hex Cell: " + i + " batchCount " + batchCount);
            cells[i].CreateTerrain();
            if (i % BatchSize == 0 && i != 0)
            {
                Debug.Log("return Instantiating Hex Cell: " + i + " batchCount " + batchCount);
                batchCount++;
                OnCellBatchgenerated?.Invoke((float)batchCount / totalBatches);
                yield return null;
            }            
        }
        OnCellInstancesGenerated?.Invoke();
    }

    Color[] colors = new Color[] { Color.red, Color.blue, Color.green, Color.yellow, Color.magenta, Color.cyan };
}

public enum HexOrientation
{
    FlatTop,
    PointyTop
}
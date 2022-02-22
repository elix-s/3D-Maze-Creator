using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  Основной алгоритм генерации лабиринта
/// </summary>
public class MazeCreator : MonoBehaviour
{
    #region Private Variables

    [SerializeField] private Material _floorMaterial;
    [SerializeField] private Material _wallMaterial;

    private MazeDataCreator _dataCreator;
    private MazeMeshCreator _meshCreator;

    #endregion

    #region Properties

    public int[,] MazeData
    {
        get; private set;
    }

    public float WallWidth
    {
        get; private set;
    }
    public float WallHeight
    {
        get; private set;
    }

    public int StartRow
    {
        get; private set;
    }
    public int StartCol
    {
        get; private set;
    }

    public int FinalRow
    {
        get; private set;
    }
    public int FinalCol
    {
        get; private set;
    }

    #endregion

    #region Private Methods

    private void MazeMapConstruct()
    {
        GameObject mazeObject = new GameObject();
        mazeObject.transform.position = Vector3.zero;

        MeshFilter meshFilter = mazeObject.AddComponent<MeshFilter>();
        meshFilter.mesh = _meshCreator.MeshData(MazeData);

        MeshCollider meshCollider = mazeObject.AddComponent<MeshCollider>();
        meshCollider.sharedMesh = meshFilter.mesh;

        MeshRenderer meshRenderer = mazeObject.AddComponent<MeshRenderer>();
        meshRenderer.materials = new Material[2] {_floorMaterial, _wallMaterial};

        mazeObject.AddComponent<GeneratedObject>();
    }

    #endregion

    #region Public Methods

    public void MazeCreate(int sizeRows, int sizeCols)
    {
        MazeData = _dataCreator.DataCreate(sizeRows, sizeCols);

        WallWidth = _meshCreator.HallwayWidth;
        WallHeight = _meshCreator.HallwayHeight;

        MazeMapConstruct();
    }

    #endregion

    #region MonoBehaviour

    void Awake()
    {
        _dataCreator = new MazeDataCreator();
        _meshCreator = new MazeMeshCreator();

        // ячейка лабиринта по умолчанию, 1 - стены, 0 -пустое пространство
        MazeData = new int[,]
        {
            {1, 1, 1},
            {1, 0, 1},
            {1, 1, 1}
        };
    }

    void Start()
    {
        MazeCreate(21, 23);
    }

    #endregion
}

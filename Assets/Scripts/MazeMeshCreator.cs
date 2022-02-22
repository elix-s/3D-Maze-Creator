using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  Генерация меша лабиринта
/// </summary>
public class MazeMeshCreator
{    
    #region Public Variables

    public float HallwayWidth;     
    public float HallwayHeight;

    #endregion

    #region Class Constructor    

    public MazeMeshCreator()
    {
        HallwayWidth = 3.0f;
        HallwayHeight = 3.0f;
    }

    #endregion

    #region Private Methods

    private void CreateQuad(Matrix4x4 matrix, ref List<Vector3> newVertices,
        ref List<Vector2> newUVs, ref List<int> newTriangles)
    {
        int vertexCounter = newVertices.Count;

        Vector3 v1 = new Vector3(-0.5f, -0.5f, 0);
        Vector3 v2 = new Vector3(-0.5f, 0.5f, 0);
        Vector3 v3 = new Vector3(0.5f, 0.5f, 0);
        Vector3 v4 = new Vector3(0.5f, -0.5f, 0);

        newVertices.Add(matrix.MultiplyPoint3x4(v1));
        newVertices.Add(matrix.MultiplyPoint3x4(v2));
        newVertices.Add(matrix.MultiplyPoint3x4(v3));
        newVertices.Add(matrix.MultiplyPoint3x4(v4));

        newUVs.Add(new Vector2(1, 0));
        newUVs.Add(new Vector2(1, 1));
        newUVs.Add(new Vector2(0, 1));
        newUVs.Add(new Vector2(0, 0));

        newTriangles.Add(vertexCounter + 2);
        newTriangles.Add(vertexCounter + 1);
        newTriangles.Add(vertexCounter);

        newTriangles.Add(vertexCounter + 3);
        newTriangles.Add(vertexCounter + 2);
        newTriangles.Add(vertexCounter);
    }

    #endregion

    #region Public Methods

    public Mesh MeshData(int[,] data)
    {
        Mesh mazeMesh = new Mesh();

        List<Vector3> newVertices = new List<Vector3>();
        List<Vector2> newUVs = new List<Vector2>();

        mazeMesh.subMeshCount = 2;
        List<int> floorTriangles = new List<int>();
        List<int> wallTriangles = new List<int>();

        int rMax = data.GetUpperBound(0);
        int cMax = data.GetUpperBound(1);

        float halfHeight = HallwayHeight * 0.5f;

        for (int i = 0; i <= rMax; i++)
            for (int j = 0; j <= cMax; j++)
                if (data[i, j] != 1)
                {
                    // создание пола
                    CreateQuad(Matrix4x4.TRS(
                        new Vector3(j * HallwayWidth, 0, i * HallwayWidth),
                        Quaternion.LookRotation(Vector3.up),
                        new Vector3(HallwayWidth, HallwayWidth, 1)
                    ), ref newVertices, ref newUVs, ref floorTriangles);

                    // создание боковых стенок
                    if (i - 1 < 0 || data[i-1, j] == 1)
                    {
                        CreateQuad(Matrix4x4.TRS(
                            new Vector3(j * HallwayWidth, halfHeight, (i - 0.5f) * HallwayWidth),
                            Quaternion.LookRotation(Vector3.forward),
                            new Vector3(HallwayWidth, HallwayHeight, 1)
                        ), ref newVertices, ref newUVs, ref wallTriangles);
                    }

                    if (j + 1 > cMax || data[i, j+1] == 1)
                    {
                        CreateQuad(Matrix4x4.TRS(
                            new Vector3((j+.5f) * HallwayWidth, halfHeight, i * HallwayWidth),
                            Quaternion.LookRotation(Vector3.left),
                            new Vector3(HallwayWidth, HallwayHeight, 1)
                        ), ref newVertices, ref newUVs, ref wallTriangles);
                    }

                    if (j - 1 < 0 || data[i, j-1] == 1)
                    {
                        CreateQuad(Matrix4x4.TRS(
                            new Vector3((j-.5f) * HallwayWidth, halfHeight, i * HallwayWidth),
                            Quaternion.LookRotation(Vector3.right),
                            new Vector3(HallwayWidth, HallwayHeight, 1)
                        ), ref newVertices, ref newUVs, ref wallTriangles);
                    }

                    if (i + 1 > rMax || data[i+1, j] == 1)
                    {
                        CreateQuad(Matrix4x4.TRS(
                            new Vector3(j * HallwayWidth, halfHeight, (i+.5f) * HallwayWidth),
                            Quaternion.LookRotation(Vector3.back),
                            new Vector3(HallwayWidth, HallwayHeight, 1)
                        ), ref newVertices, ref newUVs, ref wallTriangles);
                    }
                }
            
        mazeMesh.vertices = newVertices.ToArray();
        mazeMesh.uv = newUVs.ToArray();
        mazeMesh.SetTriangles(floorTriangles.ToArray(), 0);
        mazeMesh.SetTriangles(wallTriangles.ToArray(), 1);
        mazeMesh.RecalculateNormals();

        return mazeMesh;
    }

    #endregion
}

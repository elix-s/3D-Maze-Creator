using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  Генерация данных, используемых в MazeCreator
/// </summary>
public class MazeDataCreator
{   
    #region Private Variables

    private float _emptySpaceChance;

    #endregion

    #region Class Constructor    

    public MazeDataCreator()
    {
        _emptySpaceChance = 0.1f;
    }

    #endregion

    #region Public Methods

    public int[,] DataCreate(int sizeRows, int sizeCols)
    {
        int[,] mazeData = new int[sizeRows, sizeCols];

        int rMax = mazeData.GetUpperBound(0);
        int cMax = mazeData.GetUpperBound(1);

        for (int i = 0; i <= rMax; i++)
            for (int j = 0; j <= cMax; j++)
                if (i == 0 || j == 0 || i == rMax || j == cMax)
                    mazeData[i, j] = 1;

                else if (i % 2 == 0 && j % 2 == 0)
                    if (Random.value > _emptySpaceChance)
                    {
                        mazeData[i, j] = 1;
                        int a = Random.value < .5 ? 0 : (Random.value < .5 ? -1 : 1);
                        int b = a != 0 ? 0 : (Random.value < .5 ? -1 : 1);
                        mazeData[i+a, j+b] = 1;
                    }

        return mazeData;
    }

    #endregion
}

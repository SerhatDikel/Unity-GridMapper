using UnityEngine;

[CreateAssetMenu(fileName = "Grid Map", menuName = "GridMapper/Grid Map")]
public class GridMapperLite : ScriptableObject
{
    public int row = 1;
    public int column = 1;
    public int[] gridArray = new int[1];

    /// <summary>
    /// Return grid.
    /// </summary>
    /// <returns>
    /// Two-Dimensional Array of Integers.
    /// </returns>
    public int[,] Grid()
    {
        return Grid(row, column, gridArray);
    }

    /// <summary>
    /// Create a grid from One Dimensional Array.
    /// </summary>
    /// <param name="row">Number of Rows</param>
    /// <param name="column">Number of Columns</param>
    /// <param name="gridArray">One-Dimensional Array to read.</param>
    /// <returns>
    /// Two-Dimensional Array of Integers.
    /// </returns>
    public static int[,] Grid(int row, int column, int[] array)
    {
        int[,] grid = new int[row, column];

        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < column; j++)
            {
                grid[i, j] = array[j * row + i];

            }
        }

        return grid;
    }

}

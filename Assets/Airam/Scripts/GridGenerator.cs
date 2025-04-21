using System.Collections.Generic;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    [Header("Grid Settings")]
    [SerializeField]
    private int totalColumns;
    [SerializeField]
    private int totalRow;
    [SerializeField]
    private float gridStartPosX;
    [SerializeField]
    private float gridStartPosY;
    [SerializeField]
    private float gridStartPosZ;
    [SerializeField]
    private GameObject[] gridBlocks;

    private List<List<GameObject>> generatedGrid = new List<List<GameObject>>();

    public void GridSpawner()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < totalColumns; i++)
        {
            generatedGrid.Add(new List<GameObject>());

            for (int j = 0; j < totalRow; j++)
            {
                Vector3 position = new Vector3(gridStartPosX, gridStartPosY, gridStartPosZ);
                position.x = position.x + i;
                position.z = position.z - j;

                int blocksIndex = Random.Range(0, gridBlocks.Length);
                GameObject gridCell = Instantiate(gridBlocks[blocksIndex], position, Quaternion.identity, transform);

                generatedGrid[i].Add(gridCell);
            }
        }

    }
    public void DeleteGrid()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }
}

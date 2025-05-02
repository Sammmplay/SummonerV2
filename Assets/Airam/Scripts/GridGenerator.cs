using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    [Header("Grid Settings")]
    [SerializeField]
    public int totalColumns;
    [SerializeField]
    public int totalRows;
    [SerializeField]
    private float gridStartPosX;
    [SerializeField]
    private float gridStartPosY;
    [SerializeField]
    private float gridStartPosZ;
    [SerializeField]
    private GameObject[] gridBlocks;
    [SerializeField]
    private GameObject[] gridLimitBlocks;

    private List<List<GameObject>> generatedGrid = new List<List<GameObject>>();
    
    private NavMeshSurface navMesh;

    private void Awake()
    {
        navMesh = GetComponent<NavMeshSurface>();
    }

    /// <summary>
    /// Se crea el grid y el navmesh según el tamaño asignado
    /// </summary>
    private void Start()
    {
        gridStartPosX = -totalColumns / 2f + 0.5f;
        gridStartPosZ = totalRows / 2f - 0.5f;

        navMesh.BuildNavMesh();
    }

    /// <summary>
    /// Función que crea un grid o matriz, a partir de diferentes prefabs de forma aleatoria
    /// </summary>
    public void GridSpawner()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < totalColumns; i++)
        {
            generatedGrid.Add(new List<GameObject>());
            
            for (int j = 0; j < totalRows; j++)
            {
                Vector3 position = new Vector3(gridStartPosX, gridStartPosY, gridStartPosZ);
                position.x = position.x + i;
                position.z = position.z - j;

                GameObject gridCell;
                int blocksIndex;
                bool isLimit = i == 0f || i == totalColumns - 1 || j == 0f || j == totalRows - 1;

                //Se calcula si es el perímetro y se instancian prefabs específicos si lo es
                if (isLimit)
                {
                    blocksIndex = Random.Range(0, gridLimitBlocks.Length);
                    gridCell = Instantiate(gridLimitBlocks[blocksIndex], position, Quaternion.identity, transform);
                }
                else
                {
                    blocksIndex = Random.Range(0, gridBlocks.Length);
                    gridCell = Instantiate(gridBlocks[blocksIndex], position, Quaternion.identity, transform);
                }
                
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

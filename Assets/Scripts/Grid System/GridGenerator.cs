using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.GridSystem
{
    public class GridGenerator : MonoBehaviour
    {
        [SerializeField] private Transform gridArea;

        private Vector3 leftBorderPos, rightBorderPos, topBorder;
        private float spacing;
        private float cellSize;

        private int gridSize;

        private GameObject gridCellPrefab;
        private GameObject gridParent;

        private Grid grid;

        private void OnEnable()
        {
            EventManager.StartListening(GameEvents.GENERATE_GRID, GenerateGridOnScrene);
        }

        private void OnDisable()
        {
            EventManager.StopListening(GameEvents.GENERATE_GRID, GenerateGridOnScrene);
        }

        private void CalculateCellSize()
        {
            float distance = rightBorderPos.x - leftBorderPos.x;
            float totalSpacing = spacing * (gridSize + 1);
            cellSize = (distance - totalSpacing) / gridSize;
            topBorder.y -= cellSize / 2 + spacing;
        }
        private void CalculateBorders()
        {
            Camera camera = Camera.main;
            Vector3 border = camera.WorldToViewportPoint(gridArea.position);
            border.x = 0;
            leftBorderPos = camera.ViewportToWorldPoint(border);
            border.x = 1;
            rightBorderPos = camera.ViewportToWorldPoint(border);
            border.y = 1;
            topBorder = camera.ViewportToWorldPoint(border);

        }
        private void GenerateCell()
        {
            if (gridParent != null)
                Destroy(gridParent);

            GameObject[,] createdCells = new GameObject[gridSize, gridSize];
            GameObject tempParent = new GameObject("Cell Parent");

            gridParent = tempParent;
            tempParent.transform.parent = gridArea.transform;
            for (int i = 0; i < gridSize; i++)
            {
                for (int j = 0; j < gridSize; j++)
                {
                    Vector3 gridPos = gridArea.position;
                    float distance = cellSize + spacing;
                    gridPos.y = topBorder.y - distance * i;
                    gridPos.x = spacing + leftBorderPos.x + distance * j;
                    GameObject temp = Instantiate(gridCellPrefab, tempParent.transform);
                    temp.transform.position = gridPos;
                    temp.transform.localScale = Vector3.one * cellSize;
                    createdCells[i, j] = temp;
                }
            }
            grid.ClearPlacedObjects();
            grid.Cells = createdCells;
        }
        public void GenerateGridOnScrene(object[] obj)
        {
            if (gridArea == null)
                gridArea = transform;
            grid = (Grid)obj[0];
            if (grid == null)
            {
                Debug.LogWarning("Grid missing please check codes and prefabs!!");
                return;
            }
            gridSize = grid.Properties.Size;
            spacing = grid.Properties.Spacing;
            gridCellPrefab = grid.Properties.CellPrefab;
            CalculateBorders();
            CalculateCellSize();
            GenerateCell();
        }
    }
}
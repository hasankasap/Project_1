using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.GridSystem
{
    [RequireComponent(typeof(Grid), typeof(GridGenerator))]
    public class GridManager : MonoBehaviour
    {
        public int Score;
        [SerializeField] private GridInitSettings gridInitSettings;
        private Grid grid;
        private GridGenerator generator;

        void OnEnable()
        {
            EventManager.StartListening(GameEvents.PLACE_INTO_CELL, PlaceIntoCell);
        }
        void OnDisable()
        {
            EventManager.StopListening(GameEvents.PLACE_INTO_CELL, PlaceIntoCell);
        }
        void Start()
        {
            Initialize();
        }
        public void SetGridSize(string size)
        {
            int wantedSize = int.Parse(size);
            if (wantedSize >= gridInitSettings.GridMinSize)
                grid.Properties.Size = wantedSize;
            else
            {
                grid.Properties.Size = gridInitSettings.GridMinSize;
                // TODO: min size warning
            }
        }
        public void Initialize()
        {
            generator = GetComponent<GridGenerator>();
            if (grid == null)
                grid = GetComponent<Grid>();
            grid.Properties = gridInitSettings.GridProperties;
            ResetScore();
            GenerateGrid();
        }
        public void GenerateGrid()
        {
            if (grid == null)
            {
                Debug.LogError("Grid missing!!");
                return;
            }
                
            EventManager.TriggerEvent(GameEvents.GENERATE_GRID, new object[] { grid });
            ResetScore();
        }
        private void ResetScore()
        {
            Score = 0;
            // TODO : reset score ui
        }
        private void PlaceIntoCell(object[] obj)
        {
            if (grid == null)
                return;
            GameObject target = (GameObject)obj[0];
            if (target == null)
            {
                return;
            }
            Vector3 hitPoint = (Vector3)obj[1];
            int[] index = grid.FindSlotIndex(target);

            if (index == null || grid.IsIndexEmpty(index))
                return;

            grid.PlaceIntoCell(index, target, hitPoint.z);

            bool scoreCondition = grid.CheckCellsForScore();
            if (scoreCondition)
            {
                Score++;
                // TODO : update score ui
                grid.ClearPlacedObjects();
            }
        }
    }
}
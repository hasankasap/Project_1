using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.GridSystem
{
    [RequireComponent(typeof(Grid), typeof(GridGenerator))]
    public class GridManager : MonoBehaviour
    {
        [SerializeField] private GridInitSettings gridInitSettings;
        private Grid grid;
        private GridGenerator generator;

        void OnEnable()
        {
            EventManager.StartListening(GameEvents.PLACE_INTO_CELL, PlaceIntoCell);
            EventManager.StartListening(GameEvents.REBUILD, OnRebuild);
        }
        void OnDisable()
        {
            EventManager.StopListening(GameEvents.PLACE_INTO_CELL, PlaceIntoCell);
            EventManager.StopListening(GameEvents.REBUILD, OnRebuild);
        }
        void Start()
        {
            Initialize();
        }
        public void Initialize()
        {
            generator = GetComponent<GridGenerator>();
            if (grid == null)
                grid = GetComponent<Grid>();
            grid.SetProperTies(gridInitSettings.GridProperties);
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
        }
        private void OnRebuild(object[] obj)
        {
            int newSize = (int)obj[0];
            grid.Properties.Size = newSize;
            GenerateGrid();
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

            if (index == null || !grid.IsIndexEmpty(index))
                return;

            grid.PlaceIntoCell(index, target, hitPoint.z);
            target.transform.parent.GetComponent<Animator>().SetTrigger("Click");

            bool scoreCondition = grid.CheckCellsForScore();
            if (scoreCondition)
            {
                EventManager.TriggerEvent(GameEvents.INCREASE_SCORE, null);
                grid.ClearMatchedObjects();
            }
        }
    }
}
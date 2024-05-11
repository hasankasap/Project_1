using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.GridSystem
{
    public class Grid : MonoBehaviour
    {  
        public GridProperties Properties;
        public GameObject[,] Cells;
        public List<GridPlacedObject> PlacedObjects = new List<GridPlacedObject>();

        public void PlaceIntoCell(int[] index, GameObject target, float hitPoint_Z)
        {
            Vector3 position = target.transform.position;
            GameObject temp = Instantiate(Properties.PlacementObject, position, Quaternion.identity, target.transform);
            GridPlacedObject newObject = new GridPlacedObject();
            position = temp.transform.position;
            position.z = hitPoint_Z;
            newObject.PlacementObject = temp;
            newObject.Index = index;
            PlacedObjects.Add(newObject);
        }
        public bool IsIndexEmpty(int[] targetIndex)
        {
            if (PlacedObjects.Count == 0) return false;
            return !PlacedObjects.Any(po => po.Index.SequenceEqual(targetIndex));
        }
        public int[] FindSlotIndex(GameObject target)
        {
            int range = Properties.Size;
            for (int i = 0; i < range; i++)
            {
                for (int j = 0; j < range; j++)
                {
                    if (Cells[i, j] == target.transform.parent.gameObject)
                    {
                        return new int[] { i, j };
                    }

                }
            }
            return null;
        }

        public bool CheckCellsForScore()
        {
            if (PlacedObjects.Count == 0) return false;
            int count = 0;
            foreach (GridPlacedObject item in PlacedObjects)
            {
                count = 0;
                List<int[]> ints = new List<int[]>();
                ints.Add(new int[] { item.Index[0], item.Index[1] + 1 }); // right
                ints.Add(new int[] { item.Index[0], item.Index[1] - 1 }); // left
                ints.Add(new int[] { item.Index[0] + 1, item.Index[1] }); // up
                ints.Add(new int[] { item.Index[0] - 1, item.Index[1] }); // down
                foreach (int[] i in ints)
                {
                    if (PlacedObjects.Any(po => po.Index.SequenceEqual(i)))
                        count++;
                }
                if (count >= 2)
                    return true;
            }
            return false;
        }

        public void ClearPlacedObjects()
        {
            if (PlacedObjects.Count == 0) return;
            foreach (GridPlacedObject item in PlacedObjects)
            {
                if (item != null && item.PlacementObject != null)
                    Destroy(item.PlacementObject);
            }
            PlacedObjects.Clear();
        }
    }
}


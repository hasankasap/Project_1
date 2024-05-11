using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.GridSystem
{
    [System.Serializable]
    public class GridProperties
    {
        public int Size;
        public float Spacing;
        public GameObject CellPrefab;
        public GameObject PlacementObject;
    }
}
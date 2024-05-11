using UnityEngine;

namespace Game.GridSystem
{
    [CreateAssetMenu(fileName = "GridInitSettings", menuName = "ScriptableObjects/GridInitSettings")]
    public class GridInitSettings : ScriptableObject
    {
        [Min(1)] public int GridMinSize;
        public GridProperties GridProperties;
    }
}
using UnityEngine;

namespace map
{
    public class MainMap
    {
        private readonly MapNode _rootMapNode = new MapNode(new Vector2Int(0, 0));

        public MainMap()
        {
            MapNode splitNode1 = _rootMapNode.AddNeighbor(MapDirection.East);
            
            splitNode1.AddNeighbor(MapDirection.North);
            splitNode1.AddNeighbor(MapDirection.South);
        }

        public MapNode GetRootNode()
        {
            return _rootMapNode;
        }
    }
}
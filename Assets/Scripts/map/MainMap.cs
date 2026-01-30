using UnityEngine;

namespace map
{
    public class MainMap
    {
        private readonly MapNode _rootMapNode = new MapNode(new Vector2Int(0, 0));

        public MainMap()
        {
            _rootMapNode.AddNeighbor(MapDirection.East)
                .AddNeighbor(MapDirection.North);
        }

        public MapNode GetRootNode()
        {
            return _rootMapNode;
        }
    }
}
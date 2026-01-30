using UnityEngine;

namespace map
{
    public class MainMap
    {
        private readonly MapNode _rootMapNode = new MapNode(new Vector2Int(0, 0));

        public MainMap()
        {
            MapNode splitNode1 = _rootMapNode.AddNeighbor(MapDirection.East);

            splitNode1
                .AddNeighbor(MapDirection.North)
                .AddNeighbor(MapDirection.East);
            
            MapNode splitNode2 = splitNode1
                .AddNeighbor(MapDirection.South)
                .AddNeighbor(MapDirection.South)
                .AddNeighbor(MapDirection.East)
                .AddNeighbor(MapDirection.North)
                .AddNeighbor(MapDirection.East)
                .AddNeighbor(MapDirection.North);

            splitNode2
                .AddNeighbor(MapDirection.West);
            
            MapNode splitNode3 = splitNode2
                .AddNeighbor(MapDirection.North)
                .AddNeighbor(MapDirection.North)
                .AddNeighbor(MapDirection.East);
            
            splitNode3
                .AddNeighbor(MapDirection.East);

            MapNode splitNode4 = splitNode3
                .AddNeighbor(MapDirection.South)
                .AddNeighbor(MapDirection.South)
                .AddNeighbor(MapDirection.East)
                .AddNeighbor(MapDirection.South)
                .AddNeighbor(MapDirection.West)
                .AddNeighbor(MapDirection.South);
            
            splitNode4
                .AddNeighbor(MapDirection.South)
                .AddNeighbor(MapDirection.West);

            MapNode splitNode5 = splitNode4
                .AddNeighbor(MapDirection.East)
                .AddNeighbor(MapDirection.South)
                .AddNeighbor(MapDirection.East);

            splitNode5
                .AddNeighbor(MapDirection.East)
                .AddNeighbor(MapDirection.East);

            MapNode splitNode6 = splitNode5
                .AddNeighbor(MapDirection.North)
                .AddNeighbor(MapDirection.North);

            splitNode6
                .AddNeighbor(MapDirection.North)
                .AddNeighbor(MapDirection.North);

            splitNode6
                .AddNeighbor(MapDirection.East)
                .AddNeighbor(MapDirection.South);
        }

        public MapNode GetRootNode()
        {
            return _rootMapNode;
        }
    }
}
using UnityEngine;

namespace Map
{
    public class MainMap
    {
        private readonly MapNode _rootMapNode;
        
        public MainMap(Transform parentTransform, Sprite nodeSprite, Sprite lockSprite)
        {
            _rootMapNode = new MapNode(Vector2Int.zero, parentTransform, nodeSprite, lockSprite);
            _rootMapNode.Unlock();
            
            var splitNode1 = _rootMapNode.AddNeighbor(MapDirection.East);

            splitNode1
                .AddNeighbor(MapDirection.North)
                .AddNeighbor(MapDirection.East);
            
            var splitNode2 = splitNode1
                .AddNeighbor(MapDirection.South)
                .AddNeighbor(MapDirection.South)
                .AddNeighbor(MapDirection.East)
                .AddNeighbor(MapDirection.North)
                .AddNeighbor(MapDirection.East)
                .AddNeighbor(MapDirection.North);

            splitNode2
                .AddNeighbor(MapDirection.West);
            
            var splitNode3 = splitNode2
                .AddNeighbor(MapDirection.North)
                .AddNeighbor(MapDirection.North)
                .AddNeighbor(MapDirection.East);
            
            splitNode3
                .AddNeighbor(MapDirection.East);

            var splitNode4 = splitNode3
                .AddNeighbor(MapDirection.South)
                .AddNeighbor(MapDirection.South)
                .AddNeighbor(MapDirection.East)
                .AddNeighbor(MapDirection.South)
                .AddNeighbor(MapDirection.West)
                .AddNeighbor(MapDirection.South);
            
            splitNode4
                .AddNeighbor(MapDirection.South)
                .AddNeighbor(MapDirection.West);

            var splitNode5 = splitNode4
                .AddNeighbor(MapDirection.East)
                .AddNeighbor(MapDirection.South)
                .AddNeighbor(MapDirection.East);

            splitNode5
                .AddNeighbor(MapDirection.East)
                .AddNeighbor(MapDirection.East);

            var splitNode6 = splitNode5
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
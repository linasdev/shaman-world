using UnityEngine;

namespace Map
{
    public class MainMap
    {
        private readonly MapNode _rootMapNode;

        public MainMap()
        {
            _rootMapNode = new MapNode(Vector2Int.zero);
            _rootMapNode.Unlock();

            var splitNode1 = _rootMapNode.AddNeighbor(MapDirection.East);
            splitNode1.SetSceneName("Scenes/Villages/Village1");

            splitNode1
                .AddNeighbor(MapDirection.North)
                .AddNeighbor(MapDirection.East);

            var village2 = splitNode1
                .AddNeighbor(MapDirection.South)
                .AddNeighbor(MapDirection.South)
                .AddNeighbor(MapDirection.East)
                .AddNeighbor(MapDirection.North);
            village2.SetSceneName("Scenes/Villages/Village2");

            var splitNode2 = village2
                .AddNeighbor(MapDirection.East)
                .AddNeighbor(MapDirection.North);

            splitNode2
                .AddNeighbor(MapDirection.West);

            var village3 = splitNode2
                .AddNeighbor(MapDirection.North);
            village3.SetSceneName("Scenes/Villages/Village3");

            var splitNode3 = village3
                .AddNeighbor(MapDirection.North)
                .AddNeighbor(MapDirection.East);

            splitNode3
                .AddNeighbor(MapDirection.East);

            var village4 = splitNode3
                .AddNeighbor(MapDirection.South);
            village4.SetSceneName("Scenes/Villages/Village4");

            var village5 = village4
                .AddNeighbor(MapDirection.South)
                .AddNeighbor(MapDirection.East)
                .AddNeighbor(MapDirection.South);
            village5.SetSceneName("Scenes/Villages/Village5");

            var splitNode4 = village5
                .AddNeighbor(MapDirection.West)
                .AddNeighbor(MapDirection.South);
            splitNode4.SetSceneName("Scenes/Villages/Village6");

            splitNode4
                .AddNeighbor(MapDirection.South)
                .AddNeighbor(MapDirection.West);

            var splitNode5 = splitNode4
                .AddNeighbor(MapDirection.East)
                .AddNeighbor(MapDirection.South)
                .AddNeighbor(MapDirection.East);
            splitNode5.SetSceneName("Scenes/Villages/Village7");

            splitNode5
                .AddNeighbor(MapDirection.East)
                .AddNeighbor(MapDirection.East);

            var splitNode6 = splitNode5
                .AddNeighbor(MapDirection.North)
                .AddNeighbor(MapDirection.North);
            splitNode6.SetSceneName("Scenes/Villages/Village8");

            var village9 = splitNode6
                .AddNeighbor(MapDirection.North)
                .AddNeighbor(MapDirection.North);
            village9.SetSceneName("Scenes/Villages/Village9");

            splitNode6
                .AddNeighbor(MapDirection.East)
                .AddNeighbor(MapDirection.South);
        }

        public MapNode GetRootNode()
        {
            return _rootMapNode;
        }

        public void LoadGameObjects(float distanceBetweenNodes, Transform parentTransform, Sprite nodeSprite, Sprite lockSprite)
        {
            _rootMapNode.LoadGameObjects(null, distanceBetweenNodes, parentTransform, nodeSprite, lockSprite);
        }

        public void UnloadGameObjects()
        {
            _rootMapNode.UnloadGameObjects(null);
        }
    }
}

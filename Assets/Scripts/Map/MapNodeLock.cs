using UnityEngine;

namespace Map
{
    public class MapNodeLock
    {
        private readonly MapNode _nodeA;
        private readonly MapNode _nodeB;
        private readonly Vector3 _worldPosition;
        private GameObject _lockObject;

        public MapNodeLock(MapNode nodeA, MapNode nodeB, Vector3 worldPosition)
        {
            _nodeA = nodeA;
            _nodeB = nodeB;
            _worldPosition = worldPosition;
        }

        public void LoadGameObject(Transform parentTransform, Sprite lockSprite)
        {
            if (_lockObject)
            {
                return;
            }

            _lockObject = new GameObject($"MapLock_{_worldPosition.x}_{_worldPosition.y}")
            {
                transform =
                {
                    position = _worldPosition,
                    parent = parentTransform
                }
            };

            var spriteRenderer = _lockObject.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = lockSprite;
        }

        public void UnloadGameObject()
        {
            if (!_lockObject) {
                return;
            }

            Object.Destroy(_lockObject);
        }

        public MapNode GetOtherNode(MapNode mapNode)
        {
            return mapNode == _nodeA ? _nodeB : _nodeA;
        }
    }
}

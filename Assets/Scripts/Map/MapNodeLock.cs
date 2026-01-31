using UnityEngine;

namespace Map
{
    public class MapNodeLock
    {
        private readonly MapNode _nodeA;
        private readonly MapNode _nodeB;
        private GameObject _lockObject;

        public MapNodeLock(MapNode nodeA, MapNode nodeB)
        {
            _nodeA = nodeA;
            _nodeB = nodeB;
        }

        public void LoadGameObject(float distanceBetweenNodes, Transform parentTransform, Sprite lockSprite)
        {
            if (_lockObject)
            {
                return;
            }

            var worldPosition = GetWorldPosition(distanceBetweenNodes);
            if (!worldPosition.HasValue)
            {
                return;
            }

            var worldPositionValue = worldPosition.Value;

            _lockObject = new GameObject($"MapLock_{worldPositionValue.x}_{worldPositionValue.y}")
            {
                transform =
                {
                    position = worldPositionValue,
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

        private Vector3? GetWorldPosition(float distanceBetweenNodes)
        {
            var worldPositionA = new Vector2(_nodeA.GetPosition().x, _nodeA.GetPosition().y) * distanceBetweenNodes;
            var worldPositionB =
                new Vector2(_nodeB.GetPosition().x, _nodeB.GetPosition().y) * distanceBetweenNodes;

            switch (_nodeA.IsLocked(), _nodeB.IsLocked())
            {
                case (true, true):
                    return Vector3.Lerp(worldPositionA, worldPositionB, 0.5f);
                case (true, false):
                    return Vector3.Lerp(worldPositionA, worldPositionB, 0.25f);
                case (false, true):
                    return Vector3.Lerp(worldPositionA, worldPositionB, 0.75f);
            }

            return null;
        }
    }
}

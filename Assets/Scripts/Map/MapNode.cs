using System.Collections.Generic;
using System.Linq;
using Exceptions;
using Save;
using UnityEngine;

namespace Map
{
    public class MapNode
    {
        private readonly IDictionary<MapDirection, MapNode> _neighborNodes = new SortedDictionary<MapDirection, MapNode>();
        private readonly List<MapNodeLock> _locks;
        private readonly Vector2Int _position;
        private float? _distanceBetweenNodes;
        private Transform _parentTransform;
        private Sprite _lockSprite;
        private GameObject _nodeObject;
        private bool _locked;
        private bool _gameObjectsLoaded;

        public MapNode(Vector2Int position)
        {
            _locks = new List<MapNodeLock>();
            _position = position;
            _locked = GameStateManager.GetCurrentState().IsMapNodeLocked(position);
            _gameObjectsLoaded = false;
        }

        public void LoadGameObjects(MapNode previousNode, float distanceBetweenNodes, Transform parentTransform, Sprite nodeSprite, Sprite lockSprite)
        {
            var worldPosition = new Vector2(_position.x, _position.y) * distanceBetweenNodes;

            _distanceBetweenNodes = distanceBetweenNodes;
            _parentTransform = parentTransform;
            _lockSprite = lockSprite;
            _nodeObject = new GameObject($"MapNode_{worldPosition.x}_{worldPosition.y}")
            {
                transform =
                {
                    position = new Vector3(worldPosition.x, worldPosition.y, 0),
                    parent = _parentTransform
                }
            };

            var spriteRenderer = _nodeObject.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = nodeSprite;

            foreach (var mapLock in _locks)
            {
                mapLock.LoadGameObject(distanceBetweenNodes, _parentTransform, _lockSprite);
            }

            foreach (var neighborNode in _neighborNodes.Values)
            {
                if (previousNode != null && neighborNode == previousNode)
                {
                    continue;
                }

                neighborNode.LoadGameObjects(this, distanceBetweenNodes, _parentTransform, nodeSprite, _lockSprite);
            }

            _gameObjectsLoaded = true;
        }

        public void UnloadGameObjects(MapNode previousNode)
        {
            Object.Destroy(_nodeObject);

            foreach (var mapLock in _locks)
            {
                mapLock.UnloadGameObject();
            }

            foreach (var neighborNode in _neighborNodes.Values)
            {
                if (previousNode != null && neighborNode == previousNode)
                {
                    continue;
                }

                neighborNode.UnloadGameObjects(this);
            }

            _distanceBetweenNodes = null;
            _parentTransform = null;
            _lockSprite = null;
            _gameObjectsLoaded = false;
        }

        public MapNode AddNeighbor(MapDirection direction)
        {
            MapNode neighborNode = new(direction.IncrementPosition(_position));

            if (!_neighborNodes.TryAdd(direction, neighborNode))
            {
                throw new DirectionAlreadyPresentInMapNodeException(_position, direction);
            }

            var reverseDirection = direction.Reverse();

            if (!neighborNode._neighborNodes.TryAdd(reverseDirection, this))
            {
                throw new DirectionAlreadyPresentInMapNodeException(neighborNode._position, direction);
            }

            AddLocksForNeighborNode(neighborNode);

            return neighborNode;
        }

        public MapNode GetNeighbor(MapDirection direction)
        {
            return !_neighborNodes.TryGetValue(direction, out var neighbor) ? null : neighbor;
        }

        public Vector2Int GetPosition()
        {
            return _position;
        }

        public Vector2? GetWorldPosition()
        {
            return new Vector2(_position.x, _position.y) * _distanceBetweenNodes;
        }

        public bool IsLocked()
        {
            return _locked;
        }

        public bool UnlockAtPosition(Vector2Int lastPosition, Vector2Int position)
        {
            if (position != _position)
                return _neighborNodes.Values
                    .Where(neighborNode => neighborNode._position != lastPosition)
                    .Any(neighborNode => neighborNode.UnlockAtPosition(_position, position));

            Unlock();

            return true;
        }

        public bool LockAtPosition(Vector2Int lastPosition, Vector2Int position)
        {
            if (position != _position)
                return _neighborNodes.Values
                    .Where(neighborNode => neighborNode._position != lastPosition)
                    .Any(neighborNode => neighborNode.LockAtPosition(_position, position));

            Lock();

            return true;
        }

        public void Unlock()
        {
            if (!_locked) return;

            _locked = false;
            ResetLocks();

            GameStateManager
                .GetCurrentState()
                .SetMapNodeLocked(_position, _locked);
            GameStateManager.SaveToDisk();
        }

        public void Lock()
        {
            if (_locked) return;

            _locked = true;
            ResetLocks();

            GameStateManager
                .GetCurrentState()
                .SetMapNodeLocked(_position, _locked);
            GameStateManager.SaveToDisk();
        }

        private void ResetLocks()
        {
            ClearLocks();

            foreach (var neighborNode in _neighborNodes.Values)
            {
                AddLocksForNeighborNode(neighborNode);
            }
        }

        private void ClearLocks()
        {
            foreach (var mapLock in _locks)
            {
                mapLock.GetOtherNode(this)._locks.Remove(mapLock);
                mapLock.UnloadGameObject();
            }

            _locks.Clear();
        }

        private void AddLocksForNeighborNode(MapNode neighborNode)
        {
            if (!_locked && !neighborNode._locked)
            {
                return;
            }

            MapNodeLock mapLock = new MapNodeLock(this, neighborNode);

            _locks.Add(mapLock);
            neighborNode._locks.Add(mapLock);

            if (_gameObjectsLoaded && _distanceBetweenNodes != null)
            {
                mapLock.LoadGameObject((float) _distanceBetweenNodes, _parentTransform, _lockSprite);
            }
        }
    }
}

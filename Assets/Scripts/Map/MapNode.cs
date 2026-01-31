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
        private GameObject _nodeObject;
        private bool _locked;

        public MapNode(Vector2Int position)
        {
            _locks = new List<MapNodeLock>();
            _position = position;
            _locked = GameStateManager.GetCurrentState().IsMapNodeLocked(position);
        }

        public void LoadGameObjects(MapNode previousNode, Transform parentTransform, Sprite nodeSprite, Sprite lockSprite)
        {
            _nodeObject = new GameObject($"MapNode_{_position.x}_{_position.y}")
            {
                transform =
                {
                    position = new Vector3(_position.x, _position.y, 0),
                    parent = parentTransform
                }
            };

            var spriteRenderer = _nodeObject.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = nodeSprite;

            foreach (var mapLock in _locks)
            {
                mapLock.LoadGameObject(parentTransform, lockSprite);
            }

            foreach (var neighborNode in _neighborNodes.Values)
            {
                if (previousNode != null && neighborNode == previousNode)
                {
                    continue;
                }

                neighborNode.LoadGameObjects(this, parentTransform, nodeSprite, lockSprite);
            }
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
            var worldPosition = new Vector3(_position.x, _position.y);
            var neighborWorldPosition = new Vector3(neighborNode._position.x, neighborNode._position.y);

            switch (_locked, neighborNode._locked)
            {
                case (true, true):
                {
                    var lockWorldPosition = Vector3.Lerp(worldPosition, neighborWorldPosition, 0.5f);
                    var mapLock = new MapNodeLock(this, neighborNode, lockWorldPosition);
                    _locks.Add(mapLock);
                    neighborNode._locks.Add(mapLock);
                    break;
                }
                case (true, false):
                {
                    var lockWorldPosition = Vector3.Lerp(worldPosition, neighborWorldPosition, 0.25f);
                    var mapLock = new MapNodeLock(this, neighborNode, lockWorldPosition);
                    _locks.Add(mapLock);
                    neighborNode._locks.Add(mapLock);
                    break;
                }
                case (false, true):
                {
                    var lockWorldPosition = Vector3.Lerp(worldPosition, neighborWorldPosition, 0.75f);
                    var mapLock = new MapNodeLock(neighborNode, this, lockWorldPosition);
                    _locks.Add(mapLock);
                    neighborNode._locks.Add(mapLock);
                    break;
                }
            }
        }
    }
}

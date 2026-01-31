using System.Collections.Generic;
using System.Linq;
using Exceptions;
using UnityEngine;

namespace Map
{
    public class MapNode 
    {
        private readonly IDictionary<MapDirection, MapNode> _neighborNodes = new SortedDictionary<MapDirection, MapNode>();
        private readonly Vector2Int _position;
        private readonly Transform _parentTransform;
        private readonly Sprite _nodeSprite;
        private readonly Sprite _lockSprite;
        private readonly List<MapNodeLock> _locks;
        private bool _locked;
        
        public MapNode(Vector2Int position, Transform parentTransform, Sprite nodeSprite, Sprite lockSprite)
        {
            _position = position;
            _parentTransform = parentTransform;
            _nodeSprite = nodeSprite;
            _lockSprite = lockSprite;
            _locked = true; // TODO: Load from save file
            _locks = new List<MapNodeLock>();
            
            var nodeObject = new GameObject($"MapNode_{position.x}_{position.y}")
            {
                transform =
                {
                    position = new Vector3(_position.x, _position.y, 0),
                    parent = _parentTransform
                }
            };
            
            var spriteRenderer = nodeObject.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = _nodeSprite;
        }

        private MapNode(Vector2Int position, MapNode mapNode) : this(
            position,
            mapNode._parentTransform,
            mapNode._nodeSprite,
            mapNode._lockSprite
        ) {}

        public MapNode AddNeighbor(MapDirection direction)
        {
            MapNode neighborNode = new(direction.IncrementPosition(_position), this);
            
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

        public void ResetLocks()
        {
            ClearLocks();
            
            foreach (var neighborNode in _neighborNodes.Values)
            {
                AddLocksForNeighborNode(neighborNode);
            }
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
            if (_locked)
            {
                _locked = false;
                ResetLocks();
            }
        }

        public void Lock()
        {
            if (!_locked)
            {
                _locked = true;
                ResetLocks();
            }
        }

        private void ClearLocks()
        {
            foreach (var mapLock in _locks)
            {
                mapLock.GetOtherNode(this)._locks.Remove(mapLock);
                mapLock.Destroy();
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
                    var mapLock = new MapNodeLock(this, neighborNode, lockWorldPosition, _parentTransform, _lockSprite);
                    _locks.Add(mapLock);
                    neighborNode._locks.Add(mapLock);
                    break;
                }
                case (true, false):
                {
                    var lockWorldPosition = Vector3.Lerp(worldPosition, neighborWorldPosition, 0.25f);
                    var mapLock = new MapNodeLock(this, neighborNode, lockWorldPosition, _parentTransform, _lockSprite);
                    _locks.Add(mapLock);
                    neighborNode._locks.Add(mapLock);
                    break;
                }
                case (false, true):
                {
                    var lockWorldPosition = Vector3.Lerp(worldPosition, neighborWorldPosition, 0.75f);
                    var mapLock = new MapNodeLock(neighborNode, this, lockWorldPosition, _parentTransform, _lockSprite);
                    _locks.Add(mapLock);
                    neighborNode._locks.Add(mapLock);
                    break;
                }
            }
        }
    }
}
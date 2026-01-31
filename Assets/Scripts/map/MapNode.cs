using System.Collections.Generic;
using exceptions;
using UnityEngine;

namespace map
{
    public class MapNode 
    {
        private readonly IDictionary<MapDirection, MapNode> _neighborNodes = new SortedDictionary<MapDirection, MapNode>();
        private readonly Vector2Int _position;
        private readonly Transform _parentTransform;
        private readonly Sprite _nodeSprite;
        private readonly Sprite _lockSprite;
        private bool _locked;

        public MapNode(Vector2Int position, Transform parentTransform, Sprite nodeSprite, Sprite lockSprite)
        {
            _position = position;
            _parentTransform = parentTransform;
            _nodeSprite = nodeSprite;
            _lockSprite = lockSprite;
            _locked = true; // TODO: Load from save file
            
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

            var position = new Vector3(_position.x, _position.y);
            var neighborPosition = new Vector3(neighborNode._position.x, neighborNode._position.y);
            
            switch (_locked, neighborNode._locked)
            {
                case (true, true):
                    CreateLockObject(position, neighborPosition, 0.5f);
                    break;
                case (true, false):
                    CreateLockObject(position, neighborPosition, 0.25f);
                    break;
                case (false, true):
                    CreateLockObject(position, neighborPosition, 0.75f);
                    break;
            }

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

        public void Unlock()
        {
            _locked = false;
        }

        public void Lock()
        {
            _locked = true;
        }

        private void CreateLockObject(Vector3 position, Vector3 neighborPosition, float lerpFactor)
        {
            var lockObject = new GameObject($"MapLock_{position.x}_{position.y}_{neighborPosition.x}_{neighborPosition.y}_{lerpFactor}")
            {
                transform =
                {
                    position = Vector3.Lerp(position, neighborPosition, lerpFactor),
                    parent = _parentTransform
                }
            };
            
            var spriteRenderer = lockObject.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = _lockSprite;
        }
    }
}
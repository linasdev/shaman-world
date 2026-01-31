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
        private readonly Sprite _sprite;

        public MapNode(Vector2Int position, Transform parentTransform, Sprite sprite)
        {
            _position = position;
            _parentTransform = parentTransform;
            _sprite = sprite;
            
            var gameObject = new GameObject("MapNode_" + _position.x + "_" + _position.y)
            {
                transform =
                {
                    position = new Vector3(_position.x, _position.y, 0),
                    parent = _parentTransform
                }
            };
            
            var spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = _sprite;
        }

        public MapNode AddNeighbor(MapDirection direction)
        {
            MapNode mapNode = new(direction.IncrementPosition(_position), _parentTransform, _sprite);
            
            if (!_neighborNodes.TryAdd(direction, mapNode))
            {
                throw new DirectionAlreadyPresentInMapNodeException(direction);
            }

            mapNode._neighborNodes.TryAdd(direction.Reverse(), this);

            return mapNode;
        }

        public MapNode GetNeighbor(MapDirection direction)
        {
            return !_neighborNodes.TryGetValue(direction, out var neighbor) ? null : neighbor;
        }
        
        public Vector2Int GetPosition()
        {
            return _position;
        }
    }
}
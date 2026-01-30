using System.Collections.Generic;
using exceptions;
using UnityEngine;

namespace map
{
    public class MapNode 
    {
        private readonly IDictionary<MapDirection, MapNode> _neighborNodes = new SortedDictionary<MapDirection, MapNode>();
        private readonly Vector2Int _position;

        public MapNode(Vector2Int position)
        {
            _position = position;
        }

        public MapNode AddNeighbor(MapDirection direction)
        {
            MapNode mapNode = new MapNode(direction.IncrementPosition(_position));
            
            if (!_neighborNodes.TryAdd(direction, mapNode))
            {
                throw new DirectionAlreadyPresentInMapNodeException(direction);
            }

            mapNode._neighborNodes.TryAdd(direction.Reverse(), this);

            return mapNode;
        }

        public MapNode GetNeighbor(MapDirection direction)
        {
            if (!_neighborNodes.TryGetValue(direction, out var neighbor))
            {
                return null;
            }

            return neighbor;
        }
        
        public Vector2Int GetPosition()
        {
            return _position;
        }
    }
}
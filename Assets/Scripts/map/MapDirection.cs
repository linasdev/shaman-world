using System;
using UnityEngine;

namespace map
{
    public enum MapDirection
    {
        North,
        East,
        South,
        West,
    }

    public static class MapDirectionMethods
    {
        public static MapDirection Reverse(this MapDirection direction)
        {
            return direction switch
            {
                MapDirection.North => MapDirection.South,
                MapDirection.East => MapDirection.West,
                MapDirection.South => MapDirection.North,
                MapDirection.West => MapDirection.East,
                _ => throw new ArgumentException("Invalid map direction")
            };
        }
        
        public static Vector2Int IncrementPosition(this MapDirection direction, Vector2Int position)
        {
            return direction switch
            {
                MapDirection.North => new Vector2Int(position.x, position.y + 1),
                MapDirection.East => new Vector2Int(position.x + 1, position.y),
                MapDirection.South => new Vector2Int(position.x, position.y - 1),
                MapDirection.West => new Vector2Int(position.x - 1, position.y),
                _ => throw new ArgumentException("Invalid map direction")
            };
        }
        
        public static MapDirection? FromVector2Int(Vector2Int direction)
        {
            return direction switch
            {
                { x: 0, y: 1 } => MapDirection.North,
                { x: 1, y: 0 } => MapDirection.East,
                { x: 0, y: -1 } => MapDirection.South,
                { x: -1, y: 0 } => MapDirection.West,
                _ => null
            };
        }
    }
}
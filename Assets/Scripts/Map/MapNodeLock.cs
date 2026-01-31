using Map;
using UnityEngine;

public class MapNodeLock
{
    private readonly MapNode _nodeA;
    private readonly MapNode _nodeB;
    private readonly GameObject _lockObject;

    public MapNodeLock(MapNode nodeA, MapNode nodeB, Vector3 worldPosition, Transform parentTransform, Sprite lockSprite)
    {
        _nodeA = nodeA;
        _nodeB = nodeB;
        _lockObject = new GameObject($"MapLock_{worldPosition.x}_{worldPosition.y}")
        {
            transform =
            {
                position = worldPosition,
                parent = parentTransform
            }
        };
        
        var spriteRenderer = _lockObject.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = lockSprite;
    }

    public MapNode GetOtherNode(MapNode mapNode)
    {
        return mapNode == _nodeA ? _nodeB : _nodeA;
    }

    public void Destroy()
    {
        Object.Destroy(_lockObject);
    }
}
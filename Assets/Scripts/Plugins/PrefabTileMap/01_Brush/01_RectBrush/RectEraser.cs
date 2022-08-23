#if UNITY_EDITOR//在打包时记得添加
using UnityEngine;

namespace Plugins._01_PrefabTileMap._01_Brush._01_RectBrush
{
    public class RectEraser : RectBrush
    {
        protected override void RectOperation(Vector2Int cellPos)
        {
            map.DestroyTileImmediate(cellPos);
        }
    }
}
#endif
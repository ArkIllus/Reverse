#if UNITY_EDITOR//�ڴ��ʱ�ǵ����
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
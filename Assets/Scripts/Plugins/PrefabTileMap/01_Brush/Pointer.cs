#if UNITY_EDITOR //�ڴ��ʱ�ǵ����
using UnityEditor;
using UnityEngine;

namespace Plugins._01_PrefabTileMap._01_Brush
{
    public class Pointer : Brush
    {
        public Pointer()
        {
            icon = "Grid.Default";
        }

        public override void OnMouseDown()
        {
            window.ExitEditMode();
            Selection.activeObject = map.GetTileByCellPos(map.mouseCellPos);
        }
    }
}
#endif //�ڴ��ʱ�ǵ����
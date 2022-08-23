#if UNITY_EDITOR //在打包时记得添加
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
#endif //在打包时记得添加
#if UNITY_EDITOR //在打包时记得添加
namespace Plugins._01_PrefabTileMap._01_Brush
{
    public class Eraser : Brush
    {
        public Eraser()
        {
            icon = "Grid.EraserTool";
        }

        public override void OnMouseDown()
        {
            map.DestroyTileImmediate(map.mouseCellPos);
        }

        public override void OnMouseCellPosChange()
        {
            map.DestroyTileImmediate(map.mouseCellPos);
        }
    }
}
#endif //在打包时记得添加
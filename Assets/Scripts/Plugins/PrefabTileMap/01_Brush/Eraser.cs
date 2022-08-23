#if UNITY_EDITOR //�ڴ��ʱ�ǵ����
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
#endif //�ڴ��ʱ�ǵ����
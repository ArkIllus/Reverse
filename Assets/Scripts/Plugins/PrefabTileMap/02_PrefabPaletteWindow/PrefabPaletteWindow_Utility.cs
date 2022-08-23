#if UNITY_EDITOR //在打包时记得添加
namespace Plugins._01_PrefabTileMap._02_PrefabPaletteWindow
{
    public partial class PrefabPaletteWindow
    {
        private void UpdateAllMaps()
        {
            allMaps = FindObjectsOfType<E_PrefabTileMap>();
        }

        public void ExitEditMode()
        {
            OnExitEditMode();
        }
    }
}
#endif //在打包时记得添加
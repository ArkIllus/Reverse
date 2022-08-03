using TinyCeleste._04_Extension._01_UnityComponent;
using UnityEditor;
using UnityEngine;

namespace Plugins._01_PrefabTileMap
{
    /// <summary>
    /// ��Ƭ��
    /// </summary>
    public class E_PrefabTile : Entity
    {
        /// <summary>
        /// ��ǰ�����ĸ���ͼ
        /// </summary>
        public E_PrefabTileMap map;

        /// <summary>
        /// ��������
        /// </summary>
        public Vector2Int gridPos;

        /// <summary>
        /// �Ƿ�����ʱ���ص�״̬
        /// ����ʱ��: Ǧ�ʱ�ˢ������ǰ��ͼԪ��ʱ������ʱ����ǰ��ͼԪ������
        /// ����������������ǰ��ͼԪ�ر�����ɾ��
        /// ������뿪����ǰ��ͼԪ���ֱ���ʾ
        /// </summary>
        public bool isTempHide;

        /// <summary>
        /// ����
        /// </summary>
        public void TempHide()
        {
            isTempHide = true;
            gameObject.SetActive(false);
        }

        /// <summary>
        /// ��ʾ
        /// </summary>
        public void TempShow()
        {
            isTempHide = false;
            gameObject.SetActive(true);
        }

        /// <summary>
        /// �����������꣬ͬʱ������������
        /// </summary>
        /// <param name="gridPos"></param>
        /// <returns></returns>
        public E_PrefabTile SetCellPos(Vector2Int gridPos)
        {
            this.gridPos = gridPos;
            transform.SetPos2D(map.GridToWorld(gridPos));
            return this;
        }

        /// <summary>
        /// ��������
        /// </summary>
        public void DestroySelfImmediate()
        {
            DestroyImmediate(gameObject);
        }

        /// <summary>
        /// ����
        /// </summary>
        /// <param name="prefab"></param>
        /// <returns></returns>
        public static E_PrefabTile Create(E_PrefabTile prefab)
        {
            if (prefab == null) return null;
            return (E_PrefabTile)PrefabUtility.InstantiatePrefab(prefab);
        }

        /// <summary>
        /// ����������map��ͬʱ����parent
        /// </summary>
        /// <param name="map"></param>
        /// <returns></returns>
        public E_PrefabTile SetMap(E_PrefabTileMap map)
        {
            this.map = map;
            transform.parent = map.transform;
            return this;
        }
    }
}
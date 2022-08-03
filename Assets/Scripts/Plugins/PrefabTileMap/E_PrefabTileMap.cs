using System;
//using Boo.Lang;
using TinyCeleste._04_Extension._01_UnityComponent;
using TinyCeleste._05_MyTool._04_Editor;
using TinyCeleste._05_MyTool._06_Math;
using UnityEngine;

namespace Plugins._01_PrefabTileMap
{
    /// <summary>
    /// ��Ƭ��ͼ
    /// </summary>
    public class E_PrefabTileMap : Entity
    {
        /// <summary>
        /// ��ԭ����Tilemap����ͬһgrid���
        /// ��Ҫ�ֶ���ֵ
        /// </summary>
        public Grid grid;

        /// <summary>
        /// ��ǰ�����ͼ��С
        /// </summary>
        public Vector2 gridSize => tileArray2D.capacity;

        /// <summary>
        /// �����С
        /// </summary>
        public Vector2 cellSize => grid.cellSize;

        /// <summary>
        /// ������
        /// </summary>
        public Vector2 cellGap => grid.cellGap;

        /// <summary>
        /// ��������
        /// </summary>
        public Vector2 cellCenter => transform.position;

        /// <summary>
        /// ��׼�����������
        /// </summary>
        public Vector2 normalizedMousePos => WorldToGridCenter(Tool_GUI.GetMouseWorldPosition());

        /// <summary>
        /// �����е��������
        /// </summary>
        public Vector2Int mouseCellPos => WorldToGrid(Tool_GUI.GetMouseWorldPosition());

        /// <summary>
        /// ��Ƭ��ά����
        /// </summary>
        [HideInInspector]
        public PrefabTileArray2D tileArray2D;

        /// <summary>
        /// ��������ת��Ϊ��������
        /// </summary>
        /// <param name="worldPos"></param>
        /// <returns></returns>
        public Vector2Int WorldToGrid(Vector2 worldPos)
        {
            return Tool_Grid.WorldToGrid(worldPos, cellCenter, cellSize, cellGap);
        }

        /// <summary>
        /// ��������ת��Ϊ��������
        /// </summary>
        /// <param name="gridPos"></param>
        /// <returns></returns>
        public Vector2 GridToWorld(Vector2Int gridPos)
        {
            return Tool_Grid.GridToWorld(gridPos, cellCenter, cellSize, cellGap);
        }

        /// <summary>
        /// ��������������ӽ��ķ�����������
        /// </summary>
        /// <param name="worldPos"></param>
        /// <returns></returns>
        public Vector2 WorldToGridCenter(Vector2 worldPos)
        {
            return Tool_Grid.WorldToGridCenter(worldPos, cellCenter, cellSize, cellGap);
        }

        /// <summary>
        /// ɾ��ָ����Ԫ�����Tile
        /// </summary>
        /// <param name="cellPos"></param>
        public void DestroyTileImmediate(Vector2Int cellPos)
        {
            if (tileArray2D.IsOutOfCapacity(cellPos)) return;
            var tile = tileArray2D.GetByCoord(cellPos);
            if (tile != null)
                tile.DestroySelfImmediate();
            tileArray2D.SetByCoord(cellPos, null);
        }

        /// <summary>
        /// �ڵ�ͼ��ָ��λ����Ԥ���崴��Tile
        /// ͬʱɾ��ԭ�е�Tile
        /// </summary>
        /// <param name="cellPos"></param>
        /// <param name="prefab"></param>
        public void CreateTileAt(Vector2Int cellPos, E_PrefabTile prefab)
        {
            DestroyTileImmediate(cellPos);
            var tile = E_PrefabTile.Create(prefab).SetMap(this).SetCellPos(cellPos);
            tileArray2D.SetByCoord(cellPos, tile);
        }

        /// <summary>
        /// ������������Tile������
        /// </summary>
        [ContextMenu("Reset Tile Position")]
        private void ResetTilePosition()
        {
            foreach (var tile in tileArray2D.array)
            {
                if (tile != null)
                {
                    tile.SetCellPos(tile.gridPos);
                }
            }
        }

        /// <summary>
        /// ͨ�����������ȡTile
        /// </summary>
        /// <param name="cellPos"></param>
        /// <returns></returns>
        public E_PrefabTile GetTileByCellPos(Vector2Int cellPos)
        {
            return tileArray2D.GetByCoord(cellPos);
        }

        private void Reset()
        {
            tileArray2D = new PrefabTileArray2D();
            grid = transform.parent.GetComponent<Grid>();
        }

        private void OnDrawGizmosSelected()
        {
            // ���ϼ���ĵ�Ԫ���С
            var cellSizeWithGap = cellSize + cellGap;
            // ��ͼ��ʵ�ʴ�С
            var mapSize = cellSizeWithGap * gridSize;
            // ��ͼ�Ķ�ά����
            var mapPos = transform.GetPos2D();
            // ԭ�������λ��
            var originCoord = mapPos + cellSize / 2;
            // ���½ǵ�Ԫ�������
            var leftBottom = originCoord + tileArray2D.originCoord * cellSizeWithGap;
            // ���Ʊ߽�
            var _leftBottom = leftBottom - cellSizeWithGap / 2;
            var _center = _leftBottom + mapSize / 2;
            Gizmos.color = new Color(1f, 1f, 1f, 0.8f);
            Gizmos.DrawWireCube(_center, mapSize);
        }
    }
}
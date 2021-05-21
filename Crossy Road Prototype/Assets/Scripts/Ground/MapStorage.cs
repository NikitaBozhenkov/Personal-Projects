using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapStorage
{
    private const int StorageCapacity = 50;
    [SerializeField] private int m_storageSize = 0;
    private const int squaresPerLine = 9;
    private int curAddIndex = 0;

    private int[,] m_playableGround = new int[StorageCapacity, squaresPerLine];
    private Vector2Int m_curPlayerPos;

    public MapStorage(Vector2Int playerStartPos) {
        m_curPlayerPos = playerStartPos;
    }

    public int GetStorageSize() {
        return m_storageSize;
    }

    public void SetStorageSize(int value) {
        m_storageSize = value;
    }

    public int GetValueAt(int x, int y) {
        return m_playableGround[x % StorageCapacity, y];
    }

    public Vector2Int GetCurPlayerPos() {
        return m_curPlayerPos;
    }

    public void SetCurPlayerPos(Vector2Int value) {
        m_curPlayerPos = value;
    }


    private void UpdateAddIndex() => curAddIndex = (curAddIndex + 1) % 50;

    public void UpdateMap(List<HashSet<int>> list) {
        for (int i = 0; i < list.Count; ++i) {

            if(list[i].Contains(-1)) {
                for(int j = 0; j < 9; ++j)
                    m_playableGround[curAddIndex, j] = -1;
                foreach (var elem in list[i]) {
                    if(elem != -1)
                    m_playableGround[curAddIndex, elem] = 0;
                }
            } else
            foreach (var elem in list[i]) {
                m_playableGround[curAddIndex, elem] = 1;
            }
            UpdateAddIndex();
            ++m_storageSize;
            if(m_storageSize > StorageCapacity) {
                Debug.LogError("[MapStorage] Size > capacity");
            }
        }
    }
}

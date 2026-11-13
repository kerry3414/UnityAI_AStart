using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 路徑圖中的單一節點（純資料，不是 MonoBehaviour）。
/// 執行時由 txt 讀入建立，不需要場景中的實體物件。
/// </summary>
[Serializable]
public class NodeData
{
    /// <summary>唯一識別碼，由 WaypointExporter 自動分配。</summary>
    public int Id;

    /// <summary>世界座標（保留完整 XYZ，供多樓層使用）。</summary>
    public Vector3 Position;

    /// <summary>所在樓層，目前全部為 0，預留給進階功能 3。</summary>
    public int Floor;

    /// <summary>相鄰節點的 ID 列表。存 ID 而不是物件參照，才能直接寫進 txt。</summary>
    public List<int> NeighborIds = new List<int>();

    /// <summary>連接數量，由鄰居列表推算，不另外儲存。</summary>
    public int NeighborCount => NeighborIds.Count;

    public NodeData() { }

    public NodeData(int id, Vector3 position, int floor = 0)
    {
        Id = id;
        Position = position;
        Floor = floor;
    }

    /// <summary>加入鄰居，自動忽略自己與重複的 ID。</summary>
    public void AddNeighbor(int neighborId)
    {
        if (neighborId == Id || NeighborIds.Contains(neighborId))
            return;
        NeighborIds.Add(neighborId);
    }

    public bool RemoveNeighbor(int neighborId) => NeighborIds.Remove(neighborId);

    public bool IsNeighbor(int neighborId) => NeighborIds.Contains(neighborId);

    public override string ToString() =>
        $"Node {Id} @ {Position} (Floor {Floor}, Neighbors: {string.Join("|", NeighborIds)})";
}

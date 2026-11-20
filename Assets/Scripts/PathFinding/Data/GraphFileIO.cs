using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// 路徑點的存檔與讀檔
/// 格式：
///   NumNode N
///   ID x y z 樓層 Link 鄰居數 鄰居ID1 鄰居ID2 ...
/// </summary>
public class GraphFileIO
{
    public List< NodeData > m_NodeList = new List<NodeData>(); //節點資料庫（一開始就建立，避免 LoadWP 時是 null）

    // Application.streamingAssetsPath 是「資料夾」路徑（編輯器裡是 Assets/StreamingAssets），
    // 不是檔案，所以要再接上檔名。

    public static string FilePath => Path.Combine(Application.streamingAssetsPath, "waypoint.txt");
    //初始化：把場景中的 WP 物件轉成節點資料（ID = 在陣列中的 index）
    public void Init() 
    {
        m_NodeList = new List<NodeData>(); //節點資料庫建立記憶體
        GameObject[] gos = GameObject.FindGameObjectsWithTag("WP");  //宣告WG的GameObject資料庫
        List<GameObject> goList = new List<GameObject>(gos);
        for (int i = 0; i < gos.Length; i++)
        {
            Waypoint wp = gos[i].GetComponent<Waypoint>();
            NodeData point = new NodeData();
            point.Id = i;
            point.Position =gos[i].transform.position;
            point.m_bLink = wp.bLink;
            point.Floor = wp.inFloor;
            foreach (GameObject nei in wp.m_Neibores)
            {
                if (nei == null) continue;
                int neiId = goList.IndexOf(nei);
                if(neiId >= 0) point.AddNeighbor(neiId);
            }
            m_NodeList.Add(point);
        }
    }

    //存檔
    public void Save()
    {
        GameObject[] gos = GameObject.FindGameObjectsWithTag("WP");
        List<GameObject> goList =new List<GameObject>(gos);
        Directory.CreateDirectory(Application.streamingAssetsPath);
        StreamWriter sw = new StreamWriter(FilePath, false); //Application.streamingAssetsPath這是什麼工具?

        Vector3 position;
        int iNeiId = -1;
        int i, j;
        int nNode = gos.Length;
        string s = "NumNode" + nNode.ToString();
        sw.WriteLine(s);
        for (i = 0; i < gos.Length; i++)
        {
            Waypoint wp = gos[i].GetComponent<Waypoint>();
            if (wp == null) 
            {
                Debug.LogError(gos[i].name + "有 WP Tag 但沒有 Waypoint 元件，存檔中止");
                sw.Close();
                return;
            }

            position = gos[i].transform.position;
            s = "";
            s += i;
            s += " ";

            s += position.x;
            s += " ";
            s += position.y;
            s += " ";
            s += position.z;
            s += " ";

            s += wp.inFloor;
            s += " ";
            s += wp.bLink;
            s += " ";
            s += wp.m_Neibores.Count;
            for (j = 0; j < wp.m_Neibores.Count; j++)
            { 
                GameObject nei = wp.m_Neibores[j];
                iNeiId = (nei == null) ? -1 : goList.IndexOf(nei);
                s += " ";
                s += iNeiId;
            }

            sw.WriteLine(s);
        }
        sw.Close();
    }

    //讀檔：成功回傳 true
    public bool LoadWP() 
    {
        m_NodeList = new List<NodeData> (); //每次讀檔都清空重來

        // new StreamReader 找不到檔案會直接丟例外，不會回傳 null，所以要先檢查
        if (!File.Exists(FilePath))
        {
            Debug.LogWarning("找不到路徑檔：" + FilePath);
            return false;
        }

        StreamReader sr = new StreamReader(FilePath);
        string sAll =sr.ReadToEnd();
        sr.Close();

        string[] sLine = sAll.Split('\n');
        int iLine = sLine.Length;
        int iLineIndex = 0;
        int i;
        int nNode = 0;
        NodeData pNode;
        while (iLineIndex < iLine) 
        {
            string s = sLine[iLineIndex];
            iLineIndex++;
            s = s.Trim();
            if(s.Length == 0) continue;

            string[] ss = s.Split(' ');
            if (ss[0].Equals("NumNode")) 
            {
                nNode = int.Parse(ss[1]);
                for (i = 0; i < nNode; i++) 
                {
                    pNode = new NodeData();
                    pNode.Id = i; // ★ ID 要設定，AddNeighbor 會用 Id 判斷是不是自己

                    m_NodeList.Add(pNode);
                }
                continue;
            }

            int nId = -1;
            if (!int.TryParse(ss[0], out nId)) continue;
            if(ss.Length < 7) continue;

            int nNei = 0;
            if (nId >= 0 && nId < nNode) 
            {
                pNode = m_NodeList[nId];
                pNode.Position.x = float.Parse(ss[1]);
                pNode.Position.y = float.Parse(ss[2]);
                pNode.Position.z = float.Parse(ss[3]);
                pNode.Floor = int.Parse(ss[4]);
                pNode.m_bLink = ss[5].Equals("True");
                nNei = int.Parse(ss[6]);
                for (i = 0; i < nNei && 7 + i < ss.Length; i++)
                {
                    int neiId = int.Parse(ss[7 + i]);
                    if (neiId > -1 && neiId < nNode)
                    {
                        pNode.AddNeighbor(neiId);
                    }
                }
            }
        }
        return true;
    }
}

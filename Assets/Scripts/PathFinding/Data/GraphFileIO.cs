using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static Unity.Collections.AllocatorManager;

public class GraphFileIO
{
    public List< NodeData > m_NodeList;
    public void Init() 
    {
        m_NodeList = new List<NodeData>();
        GameObject[] gos = GameObject.FindGameObjectsWithTag("WP");
        foreach (GameObject g in gos) 
        {
            NodeData point = new NodeData();
            point.Id = 0;
            point.Position = g.transform.position;
            point.m_bLink = g.GetComponent<Waypoint> ();
            point.Floor = g.GetComponent<Waypoint>();
            point.NeighborIds = new List<int>();

            m_NodeList.Add(point);
        }
    }

    public void Save()
    {
        GameObject[] gos = GameObject.FindGameObjectsWithTag("WP");
        StreamWriter sw = new StreamWriter("StreamingAssets/waypoints.txt", false);

        Vector3 position;
        int iNeiId = -1;
        int i, j, k;
        int nNode = gos.Length;
        string s = "NumNode" + nNode.ToString();
        sw.WriteLine(s);
        for (i = 0; i < gos.Length; i++)
        {
            position = gos[i].transform.position;
            s = " ";
            s += i;
            s += " ";

            s += position.x;
            s += " ";
            s += position.y;
            s += " ";
            s += position.z;
            s += " ";

            Waypoint wp = gos[i].GetComponent<Waypoint>();
            s += wp.inFloor;
            s += " ";
            s += wp.bLink;
            s += " ";
            s += wp.m_Neibores.Count;
            s += " ";
            for (j = 0; j < wp.m_Neibores.Count; j++)
            {
                iNeiId = -1;
                for (k = 0; k < gos.Length; k++)
                {
                    if (k == i) continue;
                    if (wp.m_Neibores[j].name.Equals(gos[k].name))
                    {
                        iNeiId = k;
                        break;
                    }
                }

                s += iNeiId;
                s += " ";
            }

            sw.WriteLine(s);
        }
        sw.Close();
    }

    public void LoadWP() 
    {
        StreamReader sr = new StreamReader("StreamingAssets/waypoints.txt");
        if (sr == null) 
        {
            return;
        }

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
            s.Trim();
            string[] ss = s.Split(' ');
            if (ss[0].Equals("NumNode")) 
            {
                nNode = int.Parse(ss[1]);
                for (i = 0; i < nNode; i++) 
                {
                    pNode = new NodeData();
                    pNode.IsNeighbor = new List<NodeData>();
                    m_NodeList.Add(pNode);
                }
                continue;
            }

            if (ss[0] == " ") continue;

            int nId = -1;
            bool bInt = int.TryParse(ss[0], out nId);
            int nNei = 0;
            if (nId >= 0 && nId < nNode) 
            {
                pNode = m_NodeList[nId];
                pNode.Position.x = float.Parse(ss[1]);
                pNode.Position.y = float.Parse(ss[2]);
                pNode.Position.z = float.Parse(ss[3]);
                pNode.Floor = int.Parse(ss[4]);
                pNode.m_bLink = ss[5].Equals("True") ? true : false;
                nNei = int.Parse(ss[6]);
                for (i = 0; i < nNei; i++)
                {
                    int neiId = int.Parse(ss[7 + i]);
                    if (neiId > -1 && neiId < nNode)
                    {
                        pNode.IsNeighbor.Add(m_NodeList[neiId]);
                    }
                }
            }
        }
    }
}

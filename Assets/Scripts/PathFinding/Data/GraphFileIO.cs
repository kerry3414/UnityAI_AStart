using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GraphFileIO : MonoBehaviour
{
    public List< NodeData > m_NodeList;
    public GameObject[] m_Walls;
    public void Init() 
    {
        m_Walls = GameObject.FindGameObjectsWithTag("Wall");
        m_NodeList = new List<NodeData>();
        GameObject[] gos = GameObject.FindGameObjectsWithTag("WP");
        foreach (GameObject g in gos) 
        {
            NodeData point = new NodeData();
            point.Id = 0;
            point.Position = Vector3.zero;
            point.m_bLink = false;
            point.Floor = 0;
            point.NeighborIds = new List<int>();
        }
    }
}

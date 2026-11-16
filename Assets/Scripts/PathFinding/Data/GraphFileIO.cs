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
            point.m_bLink = g.GetComponent<Waypoint.Instantiate.bLink> ();
            point.Floor = g.GetComponent<Waypoint.Instantiate.inFloor>();
            point.NeighborIds = new List<int>();

            m_NodeList.Add(point);
        }
    }
}

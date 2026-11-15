using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    public List<GameObject> m_Neibores;
    public bool bLink = false;
    public int inFloor = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnDrawGizmos()
    {
        if (m_Neibores != null && m_Neibores.Count > 0) 
        {
            foreach (GameObject g in m_Neibores)
            {
                //Draw link m_Neibores.
                Gizmos.color = Color.green;
                Gizmos.DrawLine(this.transform.position, g.transform.position);
            
                //Draw link direction arrow.
                Vector3 vDir =g.transform.position - this.transform.position;
                Vector3 vArrow = Vector3.Cross(vDir, Vector3.up).normalized * 0.2f;
                Vector3 vTemp = g.transform.position - vDir.normalized * 0.5f;
                Vector3 v1 = vTemp + vArrow;
                Vector3 v2 = vTemp - vArrow;

                Gizmos.DrawLine(v1, g.transform.position);
                Gizmos.DrawLine(v2, g.transform.position);
            }
        }
    }
}

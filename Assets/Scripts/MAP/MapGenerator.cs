using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class MapGenerator : MonoBehaviour
{
    [SerializeField] List<GameObject> waypoints;
    public Graph _Map;
    public List<Waypoint> path = new List<Waypoint>();

    // Start is called before the first frame update
    void Start()
    {
        _Map = GetMap();
    }

    //Drawing Map in the editor
    private void OnDrawGizmos()
    {
        if (_Map == null)
            Start();

        foreach (Waypoint waypoint in _Map.graphNodes)
        {
            //Drawing nodes
            Gizmos.color = Color.red;

            //Draw path
            if (path != null)
                if (path.Contains(waypoint))
                    Gizmos.color = Color.green;

            Gizmos.DrawSphere(waypoint.Position, 0.4f);

            //Drawing edges
            if (waypoint.Neighbors != null)
            {
                Gizmos.color = Color.magenta;
                foreach (Waypoint neighbors in waypoint.Neighbors)
                {
                    Gizmos.DrawLine(waypoint.Position, neighbors.Position);
                }
            }
        }
    }

    //Getting the Map information (Nodes/Vertices connections)
    public Graph GetMap()
    {
        Graph _map = new Graph();

        GetNodes(_map);
        GetVertices(_map);

        return _map;
    }

    public void GetNodes(Graph Map)
    {
        //Getting every node for the graph
        //Map = new Graph();
        foreach (GameObject waypointPos in waypoints)
        {
            Map.AddNode(waypointPos.transform.position);
        }
    }

    public void GetVertices(Graph Map)
    {
        //Getting every connection of the graph (HARD CODED)
        //0
        Map.AddUndirectedEdge(Map.graphNodes[0], Map.graphNodes[1]);
        Map.AddUndirectedEdge(Map.graphNodes[0], Map.graphNodes[5]);
        Map.AddUndirectedEdge(Map.graphNodes[0], Map.graphNodes[9]);
        Map.AddUndirectedEdge(Map.graphNodes[0], Map.graphNodes[40]);
        //1
        Map.AddUndirectedEdge(Map.graphNodes[1], Map.graphNodes[2]);
        Map.AddUndirectedEdge(Map.graphNodes[1], Map.graphNodes[9]);
        //2
        Map.AddUndirectedEdge(Map.graphNodes[2], Map.graphNodes[3]);
        //3
        Map.AddUndirectedEdge(Map.graphNodes[3], Map.graphNodes[4]);
        //4
        //5
        Map.AddUndirectedEdge(Map.graphNodes[5], Map.graphNodes[6]);
        Map.AddUndirectedEdge(Map.graphNodes[5], Map.graphNodes[9]);
        Map.AddUndirectedEdge(Map.graphNodes[5], Map.graphNodes[46]);
        //6
        Map.AddUndirectedEdge(Map.graphNodes[6], Map.graphNodes[8]);
        Map.AddUndirectedEdge(Map.graphNodes[6], Map.graphNodes[46]);
        //7
        Map.AddUndirectedEdge(Map.graphNodes[7], Map.graphNodes[8]);
        Map.AddUndirectedEdge(Map.graphNodes[7], Map.graphNodes[9]);
        Map.AddUndirectedEdge(Map.graphNodes[7], Map.graphNodes[11]);
        //8
        Map.AddUndirectedEdge(Map.graphNodes[8], Map.graphNodes[13]);
        Map.AddUndirectedEdge(Map.graphNodes[8], Map.graphNodes[95]);
        //9
        //10
        Map.AddUndirectedEdge(Map.graphNodes[10], Map.graphNodes[40]);
        Map.AddUndirectedEdge(Map.graphNodes[10], Map.graphNodes[46]);
        //11
        Map.AddUndirectedEdge(Map.graphNodes[11], Map.graphNodes[12]);
        Map.AddUndirectedEdge(Map.graphNodes[11], Map.graphNodes[14]);
        //12
        Map.AddUndirectedEdge(Map.graphNodes[12], Map.graphNodes[13]);
        //13
        Map.AddUndirectedEdge(Map.graphNodes[13], Map.graphNodes[101]);
        //14
        Map.AddUndirectedEdge(Map.graphNodes[14], Map.graphNodes[15]);
        Map.AddUndirectedEdge(Map.graphNodes[14], Map.graphNodes[76]);
        //15
        Map.AddUndirectedEdge(Map.graphNodes[15], Map.graphNodes[18]);
        //16
        Map.AddUndirectedEdge(Map.graphNodes[16], Map.graphNodes[17]);
        Map.AddUndirectedEdge(Map.graphNodes[16], Map.graphNodes[18]);
        //17
        //18
        Map.AddUndirectedEdge(Map.graphNodes[18], Map.graphNodes[19]);
        //19
        Map.AddUndirectedEdge(Map.graphNodes[19], Map.graphNodes[20]);
        //20
        Map.AddUndirectedEdge(Map.graphNodes[20], Map.graphNodes[21]);
        Map.AddUndirectedEdge(Map.graphNodes[20], Map.graphNodes[82]);
        //21
        Map.AddUndirectedEdge(Map.graphNodes[21], Map.graphNodes[22]);
        Map.AddUndirectedEdge(Map.graphNodes[21], Map.graphNodes[24]);
        //22
        Map.AddUndirectedEdge(Map.graphNodes[22], Map.graphNodes[23]);
        //23
        //24
        Map.AddUndirectedEdge(Map.graphNodes[24], Map.graphNodes[25]);
        //25
        Map.AddUndirectedEdge(Map.graphNodes[25], Map.graphNodes[26]);
        Map.AddUndirectedEdge(Map.graphNodes[25], Map.graphNodes[28]);
        Map.AddUndirectedEdge(Map.graphNodes[25], Map.graphNodes[30]);
        //26
        Map.AddUndirectedEdge(Map.graphNodes[26], Map.graphNodes[27]);
        //27
        //28
        Map.AddUndirectedEdge(Map.graphNodes[28], Map.graphNodes[29]);
        Map.AddUndirectedEdge(Map.graphNodes[28], Map.graphNodes[30]);
        //29
        Map.AddUndirectedEdge(Map.graphNodes[29], Map.graphNodes[31]);
        Map.AddUndirectedEdge(Map.graphNodes[29], Map.graphNodes[32]);
        //30
        Map.AddUndirectedEdge(Map.graphNodes[30], Map.graphNodes[31]);
        //31
        Map.AddUndirectedEdge(Map.graphNodes[31], Map.graphNodes[32]);
        //32
        Map.AddUndirectedEdge(Map.graphNodes[32], Map.graphNodes[33]);
        Map.AddUndirectedEdge(Map.graphNodes[32], Map.graphNodes[35]);
        //33
        Map.AddUndirectedEdge(Map.graphNodes[33], Map.graphNodes[34]);
        //34
        //35
        Map.AddUndirectedEdge(Map.graphNodes[35], Map.graphNodes[36]);
        Map.AddUndirectedEdge(Map.graphNodes[35], Map.graphNodes[39]);
        //36
        Map.AddUndirectedEdge(Map.graphNodes[36], Map.graphNodes[37]);
        //37
        Map.AddUndirectedEdge(Map.graphNodes[37], Map.graphNodes[38]);
        //38
        //39
        Map.AddUndirectedEdge(Map.graphNodes[39], Map.graphNodes[40]);
        Map.AddUndirectedEdge(Map.graphNodes[39], Map.graphNodes[41]);
        //40
        //41
        Map.AddUndirectedEdge(Map.graphNodes[41], Map.graphNodes[42]);
        //42
        Map.AddUndirectedEdge(Map.graphNodes[42], Map.graphNodes[43]);
        Map.AddUndirectedEdge(Map.graphNodes[42], Map.graphNodes[44]);
        //43
        //44
        Map.AddUndirectedEdge(Map.graphNodes[44], Map.graphNodes[45]);
        //45
        Map.AddUndirectedEdge(Map.graphNodes[45], Map.graphNodes[67]);
        //46
        Map.AddUndirectedEdge(Map.graphNodes[46], Map.graphNodes[111]);
        //47
        Map.AddUndirectedEdge(Map.graphNodes[47], Map.graphNodes[48]);
        Map.AddUndirectedEdge(Map.graphNodes[47], Map.graphNodes[49]);
        Map.AddUndirectedEdge(Map.graphNodes[47], Map.graphNodes[51]);
        //48
        //49
        //50
        Map.AddUndirectedEdge(Map.graphNodes[50], Map.graphNodes[51]);
        Map.AddUndirectedEdge(Map.graphNodes[50], Map.graphNodes[65]);
        Map.AddUndirectedEdge(Map.graphNodes[50], Map.graphNodes[67]);
        //51
        Map.AddUndirectedEdge(Map.graphNodes[51], Map.graphNodes[58]);
        //52
        Map.AddUndirectedEdge(Map.graphNodes[52], Map.graphNodes[53]);
        Map.AddUndirectedEdge(Map.graphNodes[52], Map.graphNodes[72]);
        Map.AddUndirectedEdge(Map.graphNodes[52], Map.graphNodes[107]);
        //53
        Map.AddUndirectedEdge(Map.graphNodes[53], Map.graphNodes[54]);
        //54
        Map.AddUndirectedEdge(Map.graphNodes[54], Map.graphNodes[55]);
        Map.AddUndirectedEdge(Map.graphNodes[54], Map.graphNodes[73]);
        //55
        Map.AddUndirectedEdge(Map.graphNodes[55], Map.graphNodes[56]);
        //56
        Map.AddUndirectedEdge(Map.graphNodes[56], Map.graphNodes[57]);
        //57
        //58
        Map.AddUndirectedEdge(Map.graphNodes[58], Map.graphNodes[59]);
        Map.AddUndirectedEdge(Map.graphNodes[58], Map.graphNodes[72]);
        //59
        Map.AddUndirectedEdge(Map.graphNodes[59], Map.graphNodes[60]);
        Map.AddUndirectedEdge(Map.graphNodes[59], Map.graphNodes[65]);
        //60
        Map.AddUndirectedEdge(Map.graphNodes[60], Map.graphNodes[72]);
        //61
        Map.AddUndirectedEdge(Map.graphNodes[61], Map.graphNodes[62]);
        Map.AddUndirectedEdge(Map.graphNodes[61], Map.graphNodes[68]);
        Map.AddUndirectedEdge(Map.graphNodes[61], Map.graphNodes[87]);
        Map.AddUndirectedEdge(Map.graphNodes[61], Map.graphNodes[100]);
        //62
        Map.AddUndirectedEdge(Map.graphNodes[62], Map.graphNodes[63]);
        //63
        Map.AddUndirectedEdge(Map.graphNodes[63], Map.graphNodes[64]);
        //64
        //65
        Map.AddUndirectedEdge(Map.graphNodes[65], Map.graphNodes[66]);
        //66
        Map.AddUndirectedEdge(Map.graphNodes[66], Map.graphNodes[67]);
        //67
        //68
        Map.AddUndirectedEdge(Map.graphNodes[68], Map.graphNodes[69]);
        Map.AddUndirectedEdge(Map.graphNodes[68], Map.graphNodes[98]);
        Map.AddUndirectedEdge(Map.graphNodes[68], Map.graphNodes[99]);
        //69
        Map.AddUndirectedEdge(Map.graphNodes[69], Map.graphNodes[70]);
        Map.AddUndirectedEdge(Map.graphNodes[69], Map.graphNodes[74]);
        Map.AddUndirectedEdge(Map.graphNodes[69], Map.graphNodes[101]);
        //70
        Map.AddUndirectedEdge(Map.graphNodes[70], Map.graphNodes[71]);
        //71
        //72
        //73
        Map.AddUndirectedEdge(Map.graphNodes[73], Map.graphNodes[87]);
        Map.AddUndirectedEdge(Map.graphNodes[73], Map.graphNodes[93]);
        Map.AddUndirectedEdge(Map.graphNodes[73], Map.graphNodes[98]);
        //74
        Map.AddUndirectedEdge(Map.graphNodes[74], Map.graphNodes[75]);
        Map.AddUndirectedEdge(Map.graphNodes[74], Map.graphNodes[92]);
        //75
        Map.AddUndirectedEdge(Map.graphNodes[75], Map.graphNodes[76]);
        //76
        Map.AddUndirectedEdge(Map.graphNodes[76], Map.graphNodes[77]);
        //77
        Map.AddUndirectedEdge(Map.graphNodes[77], Map.graphNodes[78]);
        Map.AddUndirectedEdge(Map.graphNodes[77], Map.graphNodes[81]);
        Map.AddUndirectedEdge(Map.graphNodes[77], Map.graphNodes[83]);
        Map.AddUndirectedEdge(Map.graphNodes[77], Map.graphNodes[90]);
        //78
        Map.AddUndirectedEdge(Map.graphNodes[78], Map.graphNodes[79]);
        Map.AddUndirectedEdge(Map.graphNodes[78], Map.graphNodes[81]);
        Map.AddUndirectedEdge(Map.graphNodes[78], Map.graphNodes[89]);
        //79
        Map.AddUndirectedEdge(Map.graphNodes[79], Map.graphNodes[80]);
        Map.AddUndirectedEdge(Map.graphNodes[79], Map.graphNodes[81]);
        Map.AddUndirectedEdge(Map.graphNodes[79], Map.graphNodes[82]);
        Map.AddUndirectedEdge(Map.graphNodes[79], Map.graphNodes[89]);
        //80
        //81
        //82
        //83
        Map.AddUndirectedEdge(Map.graphNodes[83], Map.graphNodes[90]);
        //84
        Map.AddUndirectedEdge(Map.graphNodes[84], Map.graphNodes[85]);
        Map.AddUndirectedEdge(Map.graphNodes[84], Map.graphNodes[86]);
        Map.AddUndirectedEdge(Map.graphNodes[84], Map.graphNodes[88]);
        //85
        Map.AddUndirectedEdge(Map.graphNodes[85], Map.graphNodes[86]);
        Map.AddUndirectedEdge(Map.graphNodes[85], Map.graphNodes[88]);
        //86
        Map.AddUndirectedEdge(Map.graphNodes[86], Map.graphNodes[88]);
        //87
        Map.AddUndirectedEdge(Map.graphNodes[87], Map.graphNodes[94]);
        //88
        Map.AddUndirectedEdge(Map.graphNodes[88], Map.graphNodes[89]);
        //89
        Map.AddUndirectedEdge(Map.graphNodes[89], Map.graphNodes[90]);
        Map.AddUndirectedEdge(Map.graphNodes[89], Map.graphNodes[92]);
        //90
        Map.AddUndirectedEdge(Map.graphNodes[90], Map.graphNodes[92]);
        //91
        Map.AddUndirectedEdge(Map.graphNodes[91], Map.graphNodes[92]);
        //92
        //93
        Map.AddUndirectedEdge(Map.graphNodes[93], Map.graphNodes[94]);
        //94
        //95
        Map.AddUndirectedEdge(Map.graphNodes[95], Map.graphNodes[96]);
        //96
        Map.AddUndirectedEdge(Map.graphNodes[96], Map.graphNodes[97]);
        //97
        //98
        //99
        Map.AddUndirectedEdge(Map.graphNodes[99], Map.graphNodes[100]);
        //100
        //101
        Map.AddUndirectedEdge(Map.graphNodes[101], Map.graphNodes[102]);
        //102
        Map.AddUndirectedEdge(Map.graphNodes[102], Map.graphNodes[103]);
        //103
        Map.AddUndirectedEdge(Map.graphNodes[103], Map.graphNodes[104]);
        Map.AddUndirectedEdge(Map.graphNodes[103], Map.graphNodes[105]);
        //104
        //105
        Map.AddUndirectedEdge(Map.graphNodes[105], Map.graphNodes[106]);
        //106
        Map.AddUndirectedEdge(Map.graphNodes[106], Map.graphNodes[107]);
        //107
        Map.AddUndirectedEdge(Map.graphNodes[107], Map.graphNodes[108]);
        //108
        Map.AddUndirectedEdge(Map.graphNodes[108], Map.graphNodes[111]);
        //109
        Map.AddUndirectedEdge(Map.graphNodes[109], Map.graphNodes[110]);
        Map.AddUndirectedEdge(Map.graphNodes[109], Map.graphNodes[111]);
        //110
        //111
    }
}

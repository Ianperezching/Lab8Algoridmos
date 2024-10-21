using UnityEngine;

public class GraphsControl : MonoBehaviour
{
    public GameObject nodePrefabs;
    public TextAsset nodePositionTxt;
    public string[] arrayNodePositions;
    public string[] currentNodePositions;
    public ListaSimple<GameObject> AllNodes;
    public TextAsset nodeConnectionsTxt;
    public string[] arrayNodeConnections;
    public string[] currentNodeConnections;
    public Enemy enemy;

    void Start()
    {
        AllNodes = new ListaSimple<GameObject>();
        CreateNodes();
        CreateConnections();
        CreateEnemyPath();
    }

    void CreateNodes()
    {
        if (nodePositionTxt != null)
        {
            arrayNodePositions = nodePositionTxt.text.Split('\n');
            for (int i = 0; i < arrayNodePositions.Length; i++)
            {
                currentNodePositions = arrayNodePositions[i].Split(',');
                Vector2 position = new Vector2(float.Parse(currentNodePositions[0]), float.Parse(currentNodePositions[1]));
                GameObject tmp = Instantiate(nodePrefabs, position, transform.rotation);
                AllNodes.Add(tmp);
            }
        }
    }

    void CreateEnemyPath()
    {
        enemy.InitializePath(AllNodes);
    }

    void CreateConnections()
    {
        if (nodeConnectionsTxt != null)
        {
            arrayNodeConnections = nodeConnectionsTxt.text.Split('\n');
            for (int i = 0; i < arrayNodeConnections.Length; i++)
            {
                currentNodeConnections = arrayNodeConnections[i].Split(',');
                int fromNodeIndex = int.Parse(currentNodeConnections[0]);
                int toNodeIndex = int.Parse(currentNodeConnections[1]);
                float weight = float.Parse(currentNodeConnections[2]);

                if (AllNodes.Get(fromNodeIndex) != null && AllNodes.Get(toNodeIndex) != null)
                {
                    AllNodes.Get(fromNodeIndex).GetComponent<Node>().AddConnection(AllNodes.Get(toNodeIndex).GetComponent<Node>(), weight);
                }
            }
        }
    }
}
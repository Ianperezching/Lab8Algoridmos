using UnityEngine;

public class Node : MonoBehaviour
{
    public ListaSimple<(Node node, float weight)> connections;

    void Awake()
    {
        connections = CreateListConnections();
    }

    public void AddConnection(Node node, float weight)
    {
        var connection = CreateConnection(node, weight);
        connections.Add(connection);
    }

    public (Node node, float weight) SelectRandomConnection()
    {
        return connections.Get(ObtenerIndiceAleatorio());
    }

    private ListaSimple<(Node, float)> CreateListConnections()
    {
        return new ListaSimple<(Node, float)>();
    }

    private (Node, float) CreateConnection(Node node, float weight)
    {
        return (node, weight);
    }

    private int ObtenerIndiceAleatorio()
    {
        return Random.Range(0, connections.Length);
    }
}

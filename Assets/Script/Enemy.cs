using UnityEngine;
public class Enemy : MonoBehaviour
{
    public ListaSimple<GameObject> pathNodes;
    public Vector2 speedReference;
    public float energy;
    public float maxEnergy;
    public float restTime;
    private bool isResting;
    private float restTimer;
    private int currentIndex;
    private float currentWeight;
    public GameObject objectiveNode;
    public GameObject player;
    private bool isChasingPlayer;
    public GameObject visionCone;
    private SpriteRenderer spriteRenderer;

    public void InitializePath(ListaSimple<GameObject> nodes)
    {
        pathNodes = nodes;
        currentIndex = 0;
        SetObjective(pathNodes.Get(currentIndex));
        currentWeight = 0;
    }

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        InitializeEnemy();
        Debug.Log("Energía Inicial: " + maxEnergy);

        visionCone = Instantiate(visionCone, transform.position, transform.rotation);
        visionCone.transform.parent = transform;
    }

    void Update()
    {
        Debug.Log("Energía Actual: " + energy);
        if (isResting)
        {
            HandleResting();
        }
        else
        {
            if (isChasingPlayer)
            {
                Debug.Log("Persiguiendo al jugador");
                MoveTowardsObjective(player.transform.position);
                energy -= Time.deltaTime;

                if (energy <= 0)
                {
                    isResting = true;
                    isChasingPlayer = false;
                }
            }
            else
            {
                MoveTowardsObjective(objectiveNode.transform.position);
            }
        }
        if (speedReference.x > 0)
        {
            spriteRenderer.flipX = true;
            visionCone.transform.localPosition = new Vector3(0.5f, 0, 0);
            visionCone.transform.localRotation = Quaternion.Euler(0, 0, -90);
        }
        else if (speedReference.x < 0)
        {
            spriteRenderer.flipX = false;
            visionCone.transform.localPosition = new Vector3(-0.5f, 0, 0);
            visionCone.transform.localRotation = Quaternion.Euler(0, 0, 90);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == player)
        {
            isChasingPlayer = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            isChasingPlayer = false;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == objectiveNode)
        {
            HandleNodeCollision(collision);
        }
    }
    private void MoveTowardsObjective(Vector2 objective)
    {
        transform.position = Vector2.SmoothDamp(transform.position, objective, ref speedReference, 0.5f);
    }
    private void InitializeEnemy()
    {
        energy = maxEnergy;
        isResting = false;
        restTimer = 0;
        currentIndex = 0;
        if (pathNodes != null && pathNodes.Length > 0)
        {
            SetObjective(pathNodes.Get(currentIndex));
        }
    }

    private void HandleResting()
    {
        Debug.Log("Recargando Energía...");
        restTimer += Time.deltaTime;
        if (restTimer >= restTime)
        {
            isResting = false;
            energy = maxEnergy;
            restTimer = 0;
            Debug.Log("Energía Recargada");
        }
    }

    private void HandleNodeCollision(Collision2D collision)
    {
        if (collision.gameObject == objectiveNode)
        {
            Node currentNode = collision.gameObject.GetComponent<Node>();

            UpdateCurrentIndex();
            (Node nextNode, float weight) = currentNode.SelectRandomConnection();
            SetObjective(nextNode.gameObject);
            currentWeight = weight;

            energy -= currentWeight;
            Debug.Log("Peso del Nodo:" + currentWeight);
            if (energy <= 0)
            {
                isResting = true;
            }
        }
    }

    private void UpdateCurrentIndex()
    {
        currentIndex++;
        if (currentIndex >= pathNodes.Length)
        {
            currentIndex = 0;
        }
    }

    private void SetObjective(GameObject newObjective)
    {
        objectiveNode = newObjective;
    }
}

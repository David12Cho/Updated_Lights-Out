using UnityEngine;

public class TableFlip : MonoBehaviour
{
    public float interactionRange = 0.5f; // Max distance to interact
    public KeyCode interactKey = KeyCode.E; // Key to press
    public Color highlightColor = Color.yellow; // Color when highlighted

    private Transform nearestTable;
    private Renderer nearestRenderer;
    private Color originalColor;

    void Update()
    {
        FindNearestTable();

        if (nearestTable != null && Input.GetKeyDown(interactKey))
        {
            InteractWithTable();
        }
    }

    void FindNearestTable()
    {
        float minDistance = interactionRange;
        Transform newNearestTable = null;
        Renderer newNearestRenderer = null;

        foreach (GameObject table in GameObject.FindGameObjectsWithTag("Table"))
        {
            float distance = Vector3.Distance(transform.position, table.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                newNearestTable = table.transform;
                newNearestRenderer = table.GetComponent<Renderer>();
            }
        }

        if (newNearestTable != nearestTable)
        {
            RemoveHighlight(); // Remove highlight from the old table
            nearestTable = newNearestTable;
            nearestRenderer = newNearestRenderer;
            ApplyHighlight(); // Apply highlight to the new table
        }
    }

    void ApplyHighlight()
    {
        if (nearestRenderer != null)
        {
            originalColor = nearestRenderer.material.color; // Store original color
            nearestRenderer.material.color = highlightColor; // Apply highlight
        }
    }

    void RemoveHighlight()
    {
        if (nearestRenderer != null)
        {
            nearestRenderer.material.color = originalColor; // Restore original color
        }
    }

    void InteractWithTable()
    {
        Debug.Log("Interacted with table: " + nearestTable.name);
        // Add interaction logic here
    }

    private void OnDisable()
    {
        RemoveHighlight(); // Ensure highlight is removed if script is disabled
    }
}

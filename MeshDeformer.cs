using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshCollider))]
public class MeshDeformer : MonoBehaviour
{
    private MeshFilter meshFilter;
    private Vector3[] originalVertices;
    private Vector3[] displacedVertices;

    void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
        if (meshFilter != null)
        {
            originalVertices = meshFilter.mesh.vertices;
            displacedVertices = new Vector3[originalVertices.Length];
            originalVertices.CopyTo(displacedVertices, 0);
        }
    }

    public void hitItWithAHammer(Vector3 point, float radius, Vector3 direction, float strikeStrength)
    {
        if (meshFilter != null)
        {
            Vector3 sum = Vector3.zero;
            int count = 0;

            // Calculate average position of vertices within the radius
            for (int i = 0; i < displacedVertices.Length; i++)
            {
                Vector3 vertex = displacedVertices[i] + transform.position;
                float distance = Vector3.Distance(vertex, point);
                if (distance < radius)
                {
                    sum += vertex;
                    count++;
                }
            }

            if (count == 0) return;

            Vector3 averagePosition = sum / count;

            // Move vertices towards the average position and apply the strike direction
            for (int i = 0; i < displacedVertices.Length; i++)
            {
                Vector3 vertex = displacedVertices[i] + transform.position;
                float distance = Vector3.Distance(vertex, point);
                if (distance < radius)
                {
                    float influence = 1 - (distance / radius); // Influence decreases with distance
                    Vector3 flatteningDirection = (averagePosition - vertex).normalized;
                    vertex += flatteningDirection * strikeStrength * influence;
                    vertex += direction.normalized * strikeStrength * influence;
                    displacedVertices[i] = vertex - transform.position;
                }
            }

            Mesh mesh = meshFilter.mesh;
            mesh.vertices = displacedVertices;
            mesh.RecalculateNormals();

            // Update the mesh collider as well
            MeshCollider meshCollider = GetComponent<MeshCollider>();
            if (meshCollider != null)
            {
                meshCollider.sharedMesh = mesh;
            }
        }
    }
}

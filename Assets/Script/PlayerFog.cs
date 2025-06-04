using UnityEngine;
using System.Collections.Generic;

public class PlayerFog : MonoBehaviour
{
    public Transform cameraTransform;
    public LayerMask obstacleMask;
    public float maxDistance = 100f;

    public float raySpacing = 0.3f; // расстояние между лучами
    public int raysPerAxis = 3;     // количество лучей по оси X и Y

    public List<Transform> obstructingObjects = new List<Transform>();
    private Dictionary<Renderer, Material[]> originalMaterials = new Dictionary<Renderer, Material[]>();

    void Update()
    {
        UpdateObstructions();
    }

    void UpdateObstructions()
    {
        foreach (var entry in originalMaterials)
        {
            if (entry.Key != null)
                entry.Key.materials = entry.Value;
        }

        originalMaterials.Clear();
        obstructingObjects.Clear();

        Vector3 direction = transform.position - cameraTransform.position;
        float distance = Mathf.Min(direction.magnitude, maxDistance);
        Vector3 dirNormalized = direction.normalized;

        Vector3 right = Vector3.Cross(Vector3.up, dirNormalized).normalized;
        Vector3 up = Vector3.Cross(dirNormalized, right).normalized;

        for (int x = -raysPerAxis; x <= raysPerAxis; x++)
        {
            for (int y = -raysPerAxis; y <= raysPerAxis; y++)
            {
                Vector3 offset = right * x * raySpacing + up * y * raySpacing;
                Vector3 origin = cameraTransform.position + offset;

                RaycastHit[] hits = Physics.RaycastAll(origin, dirNormalized, distance, obstacleMask);
                foreach (RaycastHit hit in hits)
                {
                    Transform obj = hit.transform;

                    if (!obstructingObjects.Contains(obj))
                    {
                        obstructingObjects.Add(obj);

                        Renderer rend = obj.GetComponent<Renderer>();
                        if (rend != null)
                        {
                            if (!originalMaterials.ContainsKey(rend))
                                originalMaterials[rend] = rend.materials;

                            Material[] newMats = rend.materials;
                            for (int i = 0; i < newMats.Length; i++)
                            {
                                Material mat = new Material(newMats[i]);
                                SetMaterialTransparent(mat, 0.5f);
                                newMats[i] = mat;
                            }

                            rend.materials = newMats;
                        }
                    }
                }
            }
        }
    }

    void SetMaterialTransparent(Material mat, float alpha)
    {
        mat.SetFloat("_Mode", 3);
        mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        mat.SetInt("_ZWrite", 0);
        mat.DisableKeyword("_ALPHATEST_ON");
        mat.EnableKeyword("_ALPHABLEND_ON");
        mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        mat.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;

        Color col = mat.color;
        col.a = alpha;
        mat.color = col;
    }
}

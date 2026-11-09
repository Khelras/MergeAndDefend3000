using UnityEngine;

public class TowerHighlight : MonoBehaviour
{
    [SerializeField] private Color m_highlightColor = Color.cyan;
    [SerializeField] private float m_emissionIntensity = 2f;

    private Renderer[] m_renderers;
    private Material[][] m_originalMaterials;
    private Material[][] m_highlightMaterials;

    private void Awake()
    {
        m_renderers = GetComponentsInChildren<Renderer>();

        m_originalMaterials = new Material[m_renderers.Length][];
        m_highlightMaterials = new Material[m_renderers.Length][];

        for (int i = 0; i < m_renderers.Length; i++)
        {
            m_originalMaterials[i] = m_renderers[i].sharedMaterials;

            Material[] originals = m_originalMaterials[i];
            Material[] highlighted = new Material[originals.Length];

            for (int j = 0; j < originals.Length; j++)
            {
                Material material = new Material(originals[j]);

                if (material.HasProperty("_EmissionColor"))
                {
                    material.EnableKeyword("_EMISSION");
                    material.SetColor( "_EmissionColor", m_highlightColor * m_emissionIntensity);
                }

                highlighted[j] = material;
            }

            m_highlightMaterials[i] = highlighted;  
        }
    }

    public void SetHighlight(bool highlighted)
    {
        for (int i = 0; i < m_renderers.Length; i++)
        {
            m_renderers[i].sharedMaterials = highlighted ? m_highlightMaterials[i] : m_originalMaterials[i];
        }
    }

    private void OnDestroy()
    {
        if (m_highlightMaterials == null)
        {
            return;
        }

        foreach (Material[] materials in m_highlightMaterials)
        {
            foreach (Material material in materials)
            {
                if (material != null)
                {
                    Destroy(material);
                }
            }
        }
    }
}

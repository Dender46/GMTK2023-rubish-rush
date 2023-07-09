using System.Collections.Generic;
using UnityEngine;

public class WindowStatus : MonoBehaviour
{
    [SerializeField] List<Material> m_ShatteredGlassMats;

    bool m_IsShattered = false;
    public void Shatter()
    {
        if (m_IsShattered)
        {
            return;
        }

        var randomMaterial = m_ShatteredGlassMats[Random.Range(0, m_ShatteredGlassMats.Count)];
        GetComponent<MeshRenderer>().material = randomMaterial;

        GameManager.ShowBonusText(transform.position);

        m_IsShattered = true;
    }
}

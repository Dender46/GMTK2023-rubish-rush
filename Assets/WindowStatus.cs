using System.Collections.Generic;
using UnityEngine;

public class WindowStatus : MonoBehaviour
{
    [SerializeField] List<Material> m_ShatteredGlassMats;

    public void Shatter()
    {
        var randomMaterial = m_ShatteredGlassMats[Random.Range(0, m_ShatteredGlassMats.Count)];
        GetComponent<MeshRenderer>().material = randomMaterial;
    }
}

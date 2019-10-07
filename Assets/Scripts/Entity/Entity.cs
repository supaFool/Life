using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    private FileReader m_reader;

    public FileReader Reader { get => m_reader; set => m_reader = value; }

    public abstract string ID();
    public abstract string Description();

    //Use this to init all Entity data
    private void Awake()
    {
        Debug.Log("Entity is Awake");
        Reader = GameObject.FindGameObjectWithTag("Tools").GetComponent<FileReader>();
    }
}

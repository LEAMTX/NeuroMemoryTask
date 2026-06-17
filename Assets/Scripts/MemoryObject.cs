using UnityEngine;

public class MemoryObject : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private float visibleDuration = 10f;

    public Vector3 OriginalPosition { get; private set; }

    private void Start()
    {
        OriginalPosition = transform.position; // enregistre la position du transform ( composé de position, scale, rotation) du cube actuel (-2 x, 0.5 y , 2 z)
//conservé en mémoire pour savoir où était le cube avant de disparaitre, calcul erreur joueur ( comparaison position choisie, position réelle)
        Invoke(nameof(HideObject), visibleDuration);
    }

    private void HideObject()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

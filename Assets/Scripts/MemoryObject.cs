using System.Collections;
using UnityEngine;

public class MemoryObject : MonoBehaviour
{
    [SerializeField, Min(0.1f)] private float dureeVisible = 10f;

    public Vector3 PositionInitiale { get; private set; }
    public bool EstCache { get; private set; }
    public float DureeVisible => dureeVisible;

    private Coroutine routineDisparition;

    // Enregistre la position de départ de l'objet.
    private void Awake()
    {
        EnregistrerPositionInitiale();
    }

    // Relance l'objet cible pour un nouvel essai.
    public void RelancerEssai()
    {
        AfficherObjet();

        if (routineDisparition != null)
        {
            StopCoroutine(routineDisparition);
        }

        routineDisparition = StartCoroutine(CacherApresDelai());
    }

    // Garde en mémoire la position actuelle de l'objet.
    private void EnregistrerPositionInitiale()
    {
        PositionInitiale = transform.position;
    }

    // Rend l'objet visible et remet son état à zéro.
    public void AfficherObjet()
    {
        if (routineDisparition != null)
        {
            StopCoroutine(routineDisparition);
            routineDisparition = null;
        }

        gameObject.SetActive(true);
        EnregistrerPositionInitiale();
        EstCache = false;
    }

    // Attend la durée choisie puis cache l'objet.
    private IEnumerator CacherApresDelai()
    {
        yield return new WaitForSeconds(dureeVisible);
        routineDisparition = null;
        CacherObjet();
    }

    // Cache l'objet cible.
    private void CacherObjet()
    {
        EstCache = true;
        gameObject.SetActive(false);
    }

    // Affiche un repère dans l'éditeur Unity.
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(Application.isPlaying ? PositionInitiale : transform.position, 0.2f);
    }
}

using System.Globalization;
using System.IO;
using UnityEngine;

public class MemoryManager : MonoBehaviour
{
    [SerializeField] private MemoryObject objetCible;
    [SerializeField] private MemoryObject[] objetsDeMemoire;
    [SerializeField] private bool choisirUnObjetAuHasard = true;
    [SerializeField] private Camera cameraDeJeu;
    [SerializeField] private LayerMask couchesCliquables = Physics.DefaultRaycastLayers;
    [SerializeField, Min(0.01f)] private float distanceErreurMaximale = 5f;
    [SerializeField] private bool enregistrerResultatsCsv = true;
    [SerializeField] private KeyCode toucheRecommencer = KeyCode.R;

    private bool essaiTermine;
    private string cheminFichierResultats;

    // Prépare la tâche au lancement de la scène.
    private void Start()
    {
        InitialiserListeObjets();

        if (objetsDeMemoire.Length == 0)
        {
            Debug.LogError("La tâche ne peut pas démarrer car aucun objet de mémoire n'a été trouvé.");
            enabled = false;
            return;
        }

        if (cameraDeJeu == null)
        {
            cameraDeJeu = Camera.main;
        }

        if (cameraDeJeu == null)
        {
            Debug.LogError("La tâche ne peut pas démarrer car aucune caméra n'a été trouvée.");
            enabled = false;
            return;
        }

        cheminFichierResultats = Path.Combine(Application.persistentDataPath, "memory_trial_results.csv");

        if (enregistrerResultatsCsv)
        {
            CreerFichierResultatsSiBesoin();
        }

        CommencerNouvelEssai();
    }

    // Vérifie les actions du joueur à chaque image.
    private void Update()
    {
        if (Input.GetKeyDown(toucheRecommencer))
        {
            RecommencerEssai();
        }

        if (!essaiTermine && EssayerLirePositionJoueur(out Vector2 positionEcran))
        {
            GererReponse(positionEcran);
        }
    }

    // Récupère la position du clic ou du toucher du joueur.
    private bool EssayerLirePositionJoueur(out Vector2 positionEcran)
    {
        if (Input.GetMouseButtonDown(0))
        {
            positionEcran = Input.mousePosition;
            return true;
        }

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            positionEcran = Input.GetTouch(0).position;
            return true;
        }

        positionEcran = Vector2.zero;
        return false;
    }

    // Transforme le clic du joueur en position dans la scène.
    private void GererReponse(Vector2 positionEcran)
    {
        if (!objetCible.EstCache)
        {
            Debug.Log("Attends que l'objet cible disparaisse avant de répondre.");
            return;
        }

        Ray rayonDepuisCamera = cameraDeJeu.ScreenPointToRay(positionEcran);

        if (!Physics.Raycast(rayonDepuisCamera, out RaycastHit pointTouche, Mathf.Infinity, couchesCliquables))
        {
            Debug.LogWarning("Aucune surface valide n'a été touchée.");
            return;
        }

        EnregistrerReponse(pointTouche.point);
    }

    // Calcule le score du joueur et enregistre le résultat.
    private void EnregistrerReponse(Vector3 positionCliquee)
    {
        essaiTermine = true;

        Vector3 positionReelle = objetCible.PositionInitiale;
        float distanceErreur = Vector3.Distance(positionReelle, positionCliquee);
        float score = Mathf.Clamp01(1f - distanceErreur / distanceErreurMaximale) * 100f;

        Debug.Log(
            $"Résultat | position choisie={FormaterVecteur(positionCliquee)} | position réelle={FormaterVecteur(positionReelle)} | erreur={distanceErreur.ToString("0.###", CultureInfo.InvariantCulture)} | score={score.ToString("0.#", CultureInfo.InvariantCulture)}/100"
        );

        if (enregistrerResultatsCsv)
        {
            SauvegarderResultat(positionCliquee, positionReelle, distanceErreur, score);
        }
    }

    // Relance un essai avec une nouvelle cible.
    private void RecommencerEssai()
    {
        essaiTermine = false;
        CommencerNouvelEssai();
        Debug.Log("Nouvel essai lancé.");
    }

    // Cherche les objets de mémoire disponibles dans la scène.
    private void InitialiserListeObjets()
    {
        if (objetsDeMemoire != null && objetsDeMemoire.Length > 0)
        {
            return;
        }

        if (objetCible != null)
        {
            objetsDeMemoire = new[] { objetCible };
            return;
        }

        objetsDeMemoire = FindObjectsByType<MemoryObject>(FindObjectsSortMode.None);
    }

    // Prépare tous les objets puis choisit une seule cible à cacher.
    private void CommencerNouvelEssai()
    {
        foreach (MemoryObject objetDeMemoire in objetsDeMemoire)
        {
            objetDeMemoire.AfficherObjet();
        }

        if (choisirUnObjetAuHasard)
        {
            int indexObjetChoisi = Random.Range(0, objetsDeMemoire.Length);
            objetCible = objetsDeMemoire[indexObjetChoisi];
        }
        else
        {
            objetCible = objetsDeMemoire[0];
        }

        objetCible.RelancerEssai();

        Debug.Log(
            $"Cible choisie : {objetCible.name}. Elle reste visible pendant {objetCible.DureeVisible.ToString("0.##", CultureInfo.InvariantCulture)} secondes."
        );
    }

    // Crée le fichier CSV si c'est le premier résultat.
    private void CreerFichierResultatsSiBesoin()
    {
        if (!File.Exists(cheminFichierResultats))
        {
            File.WriteAllText(
                cheminFichierResultats,
                "timestamp,clicked_x,clicked_y,clicked_z,target_x,target_y,target_z,error_distance,score\n"
            );
        }
    }

    // Ajoute le résultat de l'essai dans le fichier CSV.
    private void SauvegarderResultat(Vector3 positionCliquee, Vector3 positionReelle, float distanceErreur, float score)
    {
        string ligneCsv = string.Join(
            ",",
            System.DateTime.UtcNow.ToString("o", CultureInfo.InvariantCulture),
            positionCliquee.x.ToString(CultureInfo.InvariantCulture),
            positionCliquee.y.ToString(CultureInfo.InvariantCulture),
            positionCliquee.z.ToString(CultureInfo.InvariantCulture),
            positionReelle.x.ToString(CultureInfo.InvariantCulture),
            positionReelle.y.ToString(CultureInfo.InvariantCulture),
            positionReelle.z.ToString(CultureInfo.InvariantCulture),
            distanceErreur.ToString(CultureInfo.InvariantCulture),
            score.ToString(CultureInfo.InvariantCulture)
        );

        File.AppendAllText(cheminFichierResultats, ligneCsv + "\n");
    }

    // Affiche un Vector3 avec un format lisible.
    private static string FormaterVecteur(Vector3 valeur)
    {
        return $"({valeur.x.ToString("0.###", CultureInfo.InvariantCulture)}, {valeur.y.ToString("0.###", CultureInfo.InvariantCulture)}, {valeur.z.ToString("0.###", CultureInfo.InvariantCulture)})";
    }
}

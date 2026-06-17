using UnityEngine;

public class Script : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private float visibleDuration = 10f;

    private void Start()
    {
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

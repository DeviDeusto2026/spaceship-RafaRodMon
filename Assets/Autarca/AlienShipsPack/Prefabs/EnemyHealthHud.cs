using UnityEngine;

using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthUI : MonoBehaviour
{
    public Slider slider;
    private Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    public void UpdateHealth(float currentHealth, float maxHealth)
    {
        slider.value = currentHealth / maxHealth;
    }

    void Update()
    {
        transform.LookAt(transform.position + cam.transform.forward);
    }
}
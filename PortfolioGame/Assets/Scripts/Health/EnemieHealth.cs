using UnityEngine;
using UnityEngine.UI;

public class EnemieHealth : MonoBehaviour
{
    [Header("Health")]
    public int maxHealth = 50;
    public int currentHealth;

    [Header("UI")]
    public Slider enemyHealthSlider; // sleep hier je Slider (bij voorkeur in een world-space Canvas)

    [Header("Player detection")]
    public string playerTag = "Player"; // Zorg dat je speler de juiste tag heeft (of gebruik PlayerHealth component)

    // interne variabelen
    private GameObject sliderGO;
    private int playerOverlapCount = 0; // telt hoeveel player-colliders momenteel in de trigger zitten

    void Start()
    {
        currentHealth = maxHealth;

        if (enemyHealthSlider == null)
        {
            Debug.LogWarning($"EnemyHealth: enemyHealthSlider niet ingesteld op {name}");
        }
        else
        {
            enemyHealthSlider.maxValue = maxHealth;
            enemyHealthSlider.value = currentHealth;
            sliderGO = enemyHealthSlider.gameObject;
            // begin verborgen — healthbar alleen zichtbaar als player IN de collider is
            sliderGO.SetActive(false);
        }
    }

    void Update()
    {
        // Zorg dat de slider altijd actuele waarde heeft (zodat wanneer zichtbaar de juiste waarde getoond wordt)
        if (enemyHealthSlider != null)
            enemyHealthSlider.value = currentHealth;
    }

    // ------------------------
    // Trigger-detectie (3D)
    // Zorg dat de enemy een collider heeft met "Is Trigger" aan
    // ------------------------
    void OnTriggerEnter(Collider other)
    {
        if (IsPlayerCollider(other))
        {
            playerOverlapCount++;
            UpdateSliderVisibility();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (IsPlayerCollider(other))
        {
            playerOverlapCount = Mathf.Max(0, playerOverlapCount - 1);
            UpdateSliderVisibility();
        }
    }

    bool IsPlayerCollider(Collider c)
    {
        // detecteer player via tag óf via aanwezigheid van PlayerHealth component op parent/zelf
        if (c == null) return false;
        if (c.CompareTag(playerTag)) return true;
        if (c.GetComponentInParent<PlayerHealth>() != null) return true;
        return false;
    }

    void UpdateSliderVisibility()
    {
        if (sliderGO == null) return;
        sliderGO.SetActive(playerOverlapCount > 0);
    }

    // ------------------------
    // Damage / death
    // ------------------------
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);

        if (enemyHealthSlider != null)
            enemyHealthSlider.value = currentHealth;

        Debug.Log($"{name} took {damage} damage. Current health: {currentHealth}");
        if (currentHealth <= 0)
            Die();
    }

    void Die()
    {
        Debug.Log($"Enemy Died: {name}");
        Destroy(gameObject);
    }

    // optioneel: teken de trigger bounds in de editor (indien gewenst)
    void OnDrawGizmosSelected()
    {
        Collider col = GetComponent<Collider>();
        if (col == null) return;

        Gizmos.color = Color.yellow;
        // laat enkel een eenvoudige gizmo zien voor Trigger (niet perfect voor alle colliders, maar handig)
        Gizmos.DrawWireCube(col.bounds.center, col.bounds.size);
    }
}

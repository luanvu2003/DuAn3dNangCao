using UnityEngine;

public class Pillar : MonoBehaviour
{
    public int health = 3;

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            DestroyPillar();
        }
    }

    void DestroyPillar()
    {
        QuestManager.Instance.AddProgress(1);
        Destroy(gameObject);
    }
}
using UnityEngine;
using System.Collections;

public class Pillar : MonoBehaviour
{
    public int health = 3;

    private Vector3 originalPos;
    private bool isShaking;

    void Start()
    {
        originalPos = transform.localPosition;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        // Rung mỗi lần bị đánh
        if (!isShaking)
            StartCoroutine(Shake());

        if (health <= 0)
        {
            DestroyPillar();
        }
    }

    IEnumerator Shake()
    {
        isShaking = true;

        float duration = 0.1f;     // thời gian rung
        float magnitude = 0.08f;   // độ mạnh rung

        float elapsed = 0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float z = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = originalPos + new Vector3(x, 0, z);

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originalPos;
        isShaking = false;
    }

    void DestroyPillar()
    {
        QuestManager.Instance.AddProgress(1);
        Destroy(gameObject);
    }
}
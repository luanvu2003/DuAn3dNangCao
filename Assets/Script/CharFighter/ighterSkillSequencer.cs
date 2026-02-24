using UnityEngine;
using System.Collections;

public class FighterSkillSequencer : MonoBehaviour
{
    [Header("Phases")]
    public GameObject phase1Charge; // Hiệu ứng tụ lực/cảnh báo
    public GameObject phase2Explosion; // Hiệu ứng nổ/gây dame

    [Header("Timing")]
    public float chargeTime = 1.0f;
    public float explosionTime = 2.0f;

    void Start()
    {
        StartCoroutine(RunSequence());
    }

    IEnumerator RunSequence()
    {
        // Giai đoạn 1: Tụ lực
        if(phase1Charge) phase1Charge.SetActive(true);
        if(phase2Explosion) phase2Explosion.SetActive(false);

        yield return new WaitForSeconds(chargeTime);

        // Giai đoạn 2: Nổ
        if(phase1Charge) phase1Charge.SetActive(false);
        if(phase2Explosion) phase2Explosion.SetActive(true);
        
        // (Ở đây bạn có thể bật Collider gây dame của Phase 2 lên)

        yield return new WaitForSeconds(explosionTime);

        Destroy(gameObject);
    }
}
using UnityEngine;

public class NPCInteraction : MonoBehaviour
{
    private bool playerInRange = false;

    [SerializeField] private GameObject pressFUI;

    void Start()
    {
        if (pressFUI != null)
            pressFUI.SetActive(false);
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.F))
        {
            if (!DialogueManager.Instance.IsTalking())
            {
                DialogueManager.Instance.StartDialogue(this);

                if (pressFUI != null)
                    pressFUI.SetActive(false);
            }
        }
    }

    public bool PlayerInRange()
    {
        return playerInRange;
    }

    public void ShowPressF()
    {
        if (pressFUI != null)
            pressFUI.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;

            if (!DialogueManager.Instance.IsTalking())
            {
                if (pressFUI != null)
                    pressFUI.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;

            if (pressFUI != null)
                pressFUI.SetActive(false);
        }
    }
}
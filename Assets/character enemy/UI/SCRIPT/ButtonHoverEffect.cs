using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonHoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Vector3 originalScale;
    private Image buttonImage;

    [Header("Hover Settings")]
    public float scaleMultiplier = 1.2f;
    public Color hoverColor = Color.yellow;
    public float speed = 10f;

    private Color originalColor;
    private bool isHovering = false;

    void Start()
    {
        originalScale = transform.localScale;

        buttonImage = GetComponent<Image>();
        if (buttonImage != null)
        {
            originalColor = buttonImage.color;
        }
    }

    void Update()
    {
        if (isHovering)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, originalScale * scaleMultiplier, Time.deltaTime * speed);
        }
        else
        {
            transform.localScale = Vector3.Lerp(transform.localScale, originalScale, Time.deltaTime * speed);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovering = true;
        if (buttonImage != null)
        {
            buttonImage.color = hoverColor;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovering = false;
        if (buttonImage != null)
        {
            buttonImage.color = originalColor;
        }
    }
}
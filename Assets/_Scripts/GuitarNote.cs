using UnityEngine;

public class GuitarNote : MonoBehaviour
{
    [SerializeField] private int stringIndex;
    [SerializeField] private int fretIndex;

    private SpriteRenderer visual;
    private Color defaultColor;

    public int StringIndex => stringIndex;
    public int FretIndex => fretIndex;

    void Awake()
    {
        BoxCollider2D collider = GetComponent<BoxCollider2D>();

        // 1. Find or create a child object named "Visual"
        Transform visualTransform = transform.Find("Visual");
        if (visualTransform == null)
        {
            GameObject visualObj = new GameObject("Visual");
            visualTransform = visualObj.transform;
            visualTransform.SetParent(this.transform, false);
        }

        // 2. Get or add a SpriteRenderer to that child object
        visual = visualTransform.GetComponent<SpriteRenderer>();
        if (visual == null)
        {
            visual = visualTransform.gameObject.AddComponent<SpriteRenderer>();
        }

        // 3. Set the child's local position to match the collider's offset
        visualTransform.localPosition = collider.offset;
        visualTransform.localScale = new Vector3(collider.size.x, collider.size.y, 1f);


    }

    void Start()
    {
        // 1. Get the sprite (Instance is guaranteed to exist now)
        if (visual.sprite == null)
        {
            visual.sprite = GameManager.Instance.NoteSprite;
        }

        // 2. Set final properties
        visual.sortingOrder = 1;

        defaultColor = visual.color;
        defaultColor.a = 0f;
        visual.color = defaultColor;
    }

    /// <summary>
    /// Sets the visual's color to the new highlight color.
    /// </summary>
    public void Highlight(Color highlightColor)
    {
        if (visual != null)
        {
            visual.color = highlightColor;
        }
    }

    /// <summary>
    /// Resets the visual's color back to default (invisible).
    /// </summary>
    public void ResetColor()
    {
        if (visual != null)
        {
            visual.color = defaultColor;
        }
    }

}
using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(ColorTheme))]
public class ColoredSquare : MonoBehaviour
{
    ColorTheme _colortheme;
    SpriteRenderer _spriteRenderer;
    void Awake()
    {
        _colortheme = GetComponent<ColorTheme>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
    void Start()
    {
        int random = Random.Range(0, 5);
        _spriteRenderer.color = _colortheme.colors[random];
    }
}

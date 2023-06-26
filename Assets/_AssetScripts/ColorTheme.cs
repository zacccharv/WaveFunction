using System.Collections.Generic;
using UnityEngine;

public class ColorTheme : MonoBehaviour
{
    public List<Color> colors = new List<Color>();
    private void Awake() 
    {
        colors.Add(HexColor("#442288"));
        colors.Add(HexColor("#6CA2EA"));
        colors.Add(HexColor("#B5D33D"));
        colors.Add(HexColor("#FED23F"));
        colors.Add(HexColor("#EB7D5B"));
    }
    Color HexColor(string colorName)
    {
        Color outColor = new Color();
        ColorUtility.TryParseHtmlString(colorName, out outColor);

        return outColor;
    }

}

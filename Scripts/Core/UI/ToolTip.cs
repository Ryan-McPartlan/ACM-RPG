using UnityEngine;
using UnityEngine.UI;

public class ToolTip : MonoBehaviour {

    public static ToolTip toolTip;

    [SerializeField]
    Text title;
    [SerializeField]
    Text type;
    [SerializeField]
    Text description;

    public GameObject target; // The object currently being described

    //Singleton, the tooltip flies around
    void Awake()
    {
        if(toolTip == null)
        {
            toolTip = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void MoveToMouse()
    {
        transform.position = new Vector2(Input.mousePosition.x + 50, Input.mousePosition.y + 50); //TODO fix magic number, should appear above buff
    }

    //Called when we are moused over a character effect to generate its tooltip
    public void UpdateToolTipCharacterEffect(string newTitle, string newType, string newDescription, bool negativeEffect)
    {
        title.text = newTitle;

        string colorstring = "";
        switch (newType)
        {
            case "Magic":
                colorstring = "<color=#00ffffff>";
                break;
            case "Physical":
                colorstring = "<color=#e0b002>";
                break;
            case "Poisonous":
                colorstring = "<color=#7af442>";
                break;
            case "Holy":
                colorstring = "<color=#fffa00>";
                break;
            case "Dark":
                colorstring = "<color=#58008c>";
                break;
            case "Fire":
                colorstring = "<color=#ef7707>";
                break;
            case "Icy":
                colorstring = "<color=#ef7707>";
                break;
            default:
                colorstring = "<color=#777673>";
                newType = "Void";
                break;
        }


        //Good effects are boons, bad effects are ailments
        string affiliation = "Boon";

        if (negativeEffect)
        {
            affiliation = "Ailment";
        }

        type.text = colorstring + newType + " " + affiliation + "</color>";

        description.text = newDescription;
    }

    public void SetInactive()
    {
        target = null;
        transform.position = new Vector2(9999, 9999);
    }
}

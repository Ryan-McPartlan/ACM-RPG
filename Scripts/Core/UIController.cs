using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {

    public static UIController uIController;

    [SerializeField]
    GameObject popupText;
    [SerializeField]
    GameObject healthBar;

    [SerializeField]
    GameObject defaultEffectThumbNail;
    [SerializeField]
    GameObject buffPanel;

    [SerializeField]
    GameObject characterPanel;
    [SerializeField]
    GameObject inventoryPanel;
    [SerializeField]
    GameObject abilitiesPanel;
    [SerializeField]
    GameObject optionsPanel;
    [SerializeField]
    GameObject mapPanel;
    [SerializeField]
    GameObject PH1Panel;
    [SerializeField]
    GameObject PH2Panel;
    [SerializeField]
    GameObject creditsPanel;

    [SerializeField]
    Text[] CharacterPageText = new Text[21];

    void Awake()
    {
        if(uIController == null)
        {
            uIController = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.Log("extra singleton");
            Destroy(gameObject);
        }
    }

    //Add a buff to the buff bar
    public void AddBuffToBar(CharacterEffect effect)
    {
        GameObject temp = Instantiate(defaultEffectThumbNail, Vector3.zero, Quaternion.identity) as GameObject;
        temp.transform.SetParent(buffPanel.transform);
        CharacterEffectThumbnail temp2 = temp.GetComponent<CharacterEffectThumbnail>();
        temp2.effect = effect;
    }

    //Before setting any UI panel to active, we make sure all others are off
    public void DisableAllPanels()
    {
        characterPanel.SetActive(false);
        inventoryPanel.SetActive(false);
        abilitiesPanel.SetActive(false);
        optionsPanel.SetActive(false);
        mapPanel.SetActive(false);
        PH1Panel.SetActive(false);
        PH2Panel.SetActive(false);
        creditsPanel.SetActive(false);
    }

    //When clicked, a UI button will call this and referance its corresponding panel
    public void EnablePanel(GameObject selectedPanel)
    {
        selectedPanel.SetActive(true);
    }

    public void SetCharacterPageText()
    {
        string mainColor = "";

        CharacterPageText[0].text = "Strength: " + SetStringColor(PlayerController.playerController.mainPlayer.strength.ToString(), mainColor);
        CharacterPageText[1].text = "Intellect: " + SetStringColor(PlayerController.playerController.mainPlayer.intellect.ToString(), mainColor);
        CharacterPageText[2].text = "Constitution: " + SetStringColor(PlayerController.playerController.mainPlayer.constitution.ToString(), mainColor);
        CharacterPageText[3].text = "Spirit: " + SetStringColor(PlayerController.playerController.mainPlayer.spirit.ToString(), mainColor);
        CharacterPageText[4].text = "Dexterity: " + SetStringColor(PlayerController.playerController.mainPlayer.dexterity.ToString(), mainColor);
        CharacterPageText[5].text = "Agility: " + SetStringColor(PlayerController.playerController.mainPlayer.agility.ToString(), mainColor);
        CharacterPageText[6].text = "Luck: " + SetStringColor(PlayerController.playerController.mainPlayer.luck.ToString(), mainColor);

        CharacterPageText[7].text = "Physical Damage:\n" + SetStringColor((PlayerController.playerController.mainPlayer.MeleeMod * 100).ToString() + "%", mainColor);
        CharacterPageText[8].text = "Resist Chance:\n" + SetStringColor((PlayerController.playerController.mainPlayer.AilmentResist * 100).ToString() + "%", mainColor);
        CharacterPageText[9].text = "Magic Damage:\n" + SetStringColor((PlayerController.playerController.mainPlayer.MagicMod * 100).ToString() + "%", mainColor);
        CharacterPageText[10].text = "CD Reduction:\n" + SetStringColor((PlayerController.playerController.mainPlayer.CooldownReduction * 100).ToString() + "%", mainColor);
        CharacterPageText[11].text = "Max Health:\n" + SetStringColor(PlayerController.playerController.mainPlayer.MaxHealth.ToString(), mainColor);
        CharacterPageText[12].text = "Health Regen:\n" + SetStringColor(PlayerController.playerController.mainPlayer.RegenHealth.ToString() + "/second", mainColor);
        CharacterPageText[13].text = "Max Mana:\n" + SetStringColor(PlayerController.playerController.mainPlayer.MaxMana.ToString(), mainColor);
        CharacterPageText[14].text = "Mana Regen:\n" + SetStringColor(PlayerController.playerController.mainPlayer.RegenMana.ToString() + "/second", mainColor);
        CharacterPageText[15].text = "Critical Chance:\n" + SetStringColor((PlayerController.playerController.mainPlayer.CritChance * 100).ToString() + "%", mainColor);
        CharacterPageText[16].text = "Critical Damage:\n" + SetStringColor((PlayerController.playerController.mainPlayer.CritDamage * 100).ToString() + "%", mainColor);
        CharacterPageText[17].text = "MoveSpeed:\n" + SetStringColor(PlayerController.playerController.mainPlayer.MoveSpeed.ToString() + " tile/second", mainColor);
        CharacterPageText[18].text = "Dodge Chance:\n" + SetStringColor((PlayerController.playerController.mainPlayer.DodgeChance * 100).ToString() + "%", mainColor);
        CharacterPageText[19].text = "Item Find:\n" + SetStringColor((PlayerController.playerController.mainPlayer.ItemFind * 100).ToString() + "%", mainColor);
        CharacterPageText[20].text = "Gold Find:\n" + SetStringColor((PlayerController.playerController.mainPlayer.GoldFind * 100).ToString() + "%", mainColor);
    }

    public string SetStringColor(string stringIn, string colorCode)
    {
        string newString = "<color" + colorCode + ">";
        newString += stringIn;
        newString += "</color>";

        return newString;
    }

    public void CreateFloatingText(string text, Vector2 location, Color color)
    {
        GameObject newFloatingText = Instantiate(popupText, location, Quaternion.identity) as GameObject;
        newFloatingText.transform.SetParent(transform);
        newFloatingText.GetComponentInChildren<Text>().text = text;
        newFloatingText.GetComponentInChildren<Text>().color = color;

        Vector2 offset = new Vector2(Random.Range(-12, 12), Random.Range(10, 25));

        newFloatingText.transform.position = (Vector2)Camera.main.WorldToScreenPoint(location) + offset;


    }

    public HealthBar CreateHealthBar(GameObject unit)
    {
        GameObject newHealthbar = Instantiate(healthBar) as GameObject;
        newHealthbar.transform.SetParent(transform);
        
        newHealthbar.GetComponent<HealthBar>().unit = unit;
        return newHealthbar.GetComponent<HealthBar>();
    }
}

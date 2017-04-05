using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {

    [SerializeField]
    Image health;
    [SerializeField]
    Image mana;
    
    float xSize = 26;
    float heightIncrease = 20;

    public GameObject unit;

    void Start()
    {
        Debug.Log(health.rectTransform.sizeDelta.x);
    }

    public void Update()
    {
        transform.position = (Vector2) Camera.main.WorldToScreenPoint(unit.transform.position) + new Vector2(0, heightIncrease);
    }

    public void UpdateBar(int HP, int maxHP, int MP = 0, int maxMP = 0)
    {
        float percentHP = HP / (float)maxHP;
        health.rectTransform.sizeDelta = new Vector2(xSize * percentHP, health.rectTransform.sizeDelta.y);

        if (maxMP == 0) {
            mana.color = Color.gray;
        }
        else
        {
            float percentMP = MP / (float)maxMP;
            mana.rectTransform.sizeDelta = new Vector2(xSize * percentMP, mana.rectTransform.sizeDelta.y);
        }
    }
}

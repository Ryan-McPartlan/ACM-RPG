using UnityEngine;
using UnityEngine.UI;

public class CharacterEffectThumbnail : MonoBehaviour {

    public CharacterEffect effect;
    public Text timeString;
    public Image image;
    public RectTransform cDImage;

    //Setup the image
    void Start()
    {
        if(effect != null)
        {
            image.sprite = effect.sprite;
        }
    }

    //When our effect vanishes, so does this. Update the time each frame
    void Update()
    {
        if(effect == null)
        {
            Destroy(gameObject);
        }
        else
        {
            UpdateTimeString();
        }
    }

    //We update time each frame    //TODO: OPTIMIZE: Only update time once per second!
    void UpdateTimeString()
    {
        float timeLeft = (effect.ticksRemaining - 1) * effect.tickDelay + effect.nextTickTime - Time.time;

        float timeTillFinish = Mathf.Max((effect.ticksRemaining - 1) * effect.tickDelay - Time.time + effect.nextTickTime, 0);
        float percentComplete = 1 - (timeTillFinish / (effect.startTicks * effect.tickDelay));
        
        cDImage.sizeDelta = new Vector2(60, 60 * percentComplete);

        if (timeLeft < 0)
        {
            timeString.text = "";
        }
        else if(timeLeft < 60)
        {
            timeString.text = ((int)timeLeft + 1).ToString() + "s";
        }
        else if (timeLeft < 60 * 60)
        {
            timeString.text = ((int)timeLeft / 60 + 1).ToString() + "m";
        }
        else
        {
            timeString.text = ((int)timeLeft / 60 / 60 + 1).ToString() + "h";
        }

    }
    
    public void OnClick()
    {
        Debug.Log("test");

        ToolTip.toolTip.MoveToMouse();
        ToolTip.toolTip.UpdateToolTipCharacterEffect(effect.effectName, effect.type, effect.GetDescription(), effect.negativeEffect);
        ToolTip.toolTip.target = gameObject;
    }
}

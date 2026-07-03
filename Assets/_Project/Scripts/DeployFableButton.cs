using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Button))]
public class DeployFableButton : MonoBehaviour
{
    public TMP_Text label;
    public Image icon;

    public void Setup(int rosterIndex, FableDefinition def, Deployer deployer)
    {
        if (label != null) label.text = def != null ? def.displayName : "?";
        if (icon != null && def != null)
        {
            if (def.art != null) { icon.sprite = def.art; icon.color = Color.white; }
            else icon.color = def.placeholderColor;
        }
        GetComponent<Button>().onClick.AddListener(() => deployer.SelectOwned(rosterIndex));
    }
}

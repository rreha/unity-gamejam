using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BuffButtonUI : MonoBehaviour
{

    public TextMeshProUGUI buffNameText;
    public TextMeshProUGUI buffDescriptionText;
    public Button selectButton;

    private Buff buff;
    private BuffSelectionUI parentUI;

    public void SetBuff(Buff buffToSet, BuffSelectionUI parent)
    {
        buff = buffToSet;
        parentUI = parent;
        buffNameText.text = buff.buffName;
        buffDescriptionText.text = buff.buffDescription;
        selectButton.onClick.RemoveAllListeners();
        selectButton.onClick.AddListener(() => parentUI.OnBuffSelected(buff));
    }
}

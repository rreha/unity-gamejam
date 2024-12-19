using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffSelectionUI : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject buffSelectionPanel; // The main panel
    public BuffButtonUI[] buffButtons; // Array of BuffButtonUI scripts for each buff panel

    private List<Buff> currentBuffs = new List<Buff>();
    private Player player;

    void Start()
    {
        buffSelectionPanel.SetActive(false);
        player = FindObjectOfType<Player>(); // Assumes there's only one Player in the scene
    }

    // Call this method to display buffs
    public void ShowBuffSelection(List<Buff> buffs)
    {
        currentBuffs = buffs;
        for (int i = 0; i < buffButtons.Length; i++)
        {
            if (i < buffs.Count)
            {
                buffButtons[i].SetBuff(buffs[i], this);
                buffButtons[i].gameObject.SetActive(true);
            }
            else
            {
                buffButtons[i].gameObject.SetActive(false);
            }
        }
        buffSelectionPanel.SetActive(true);
        Time.timeScale = 0f; // Pause the game
    }

    // Method to handle buff selection
    public void OnBuffSelected(Buff selectedBuff)
    {
        selectedBuff.ApplyBuff(player);
        Debug.Log($"Buff applied: {selectedBuff.buffName}");
        buffSelectionPanel.SetActive(false);
        Time.timeScale = 1f; // Resume the game
    }
}

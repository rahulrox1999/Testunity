using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SlotMachineNew : MonoBehaviour
{
    public Image[][] reels;  // Each reel contains multiple images (3 reels)
    public int[] activeIndices = { 0, 0, 0 }; // Current active index per reel
    public Button spinButton;
    public Text resultText;

    private bool isSpinning = false;

    void Start()
    {
        spinButton.onClick.AddListener(StartSpin);
        InitializeReels();
    }

    void InitializeReels()
    {
        // Example: Assume each reel has 5 images (Assign these manually in the Unity Editor)
        reels = new Image[3][]; // 3 reels

        for (int i = 0; i < 3; i++) // 3 reels
        {
            reels[i] = new Image[5]; // Each reel has 5 images
        }

        // Ensure only the first image of each reel is visible
        for (int i = 0; i < 3; i++)
        {
            SetActiveImage(i, activeIndices[i]);
        }
    }

    void StartSpin()
    {
        if (isSpinning) return;

        isSpinning = true;
        resultText.text = "Spinning...";
        StartCoroutine(SpinReels());
    }

    IEnumerator SpinReels()
    {
        float spinTime = 1.5f; // Total spin time
        float slowDownFactor = 0.05f; // Slow down gradually

        for (int i = 0; i < 3; i++) // Each reel stops at different times
        {
            float elapsedTime = 0f;
            while (elapsedTime < spinTime)
            {
                activeIndices[i] = (activeIndices[i] + 1) % reels[i].Length;
                SetActiveImage(i, activeIndices[i]);
                elapsedTime += 0.1f;
                yield return new WaitForSeconds(0.1f + slowDownFactor * i);
            }
        }

        isSpinning = false;
        CheckWinCondition();
    }

    void SetActiveImage(int reelIndex, int activeIndex)
    {
        for (int i = 0; i < reels[reelIndex].Length; i++)
        {
            reels[reelIndex][i].gameObject.SetActive(i == activeIndex);
        }
    }

    void CheckWinCondition()
    {
        // Get the ID of each active image
        int id1 = activeIndices[0];
        int id2 = activeIndices[1];
        int id3 = activeIndices[2];

        if (id1 == id2 && id2 == id3)
        {
            resultText.text = "You Win! 🎉";
        }
        else
        {
            resultText.text = "You Lose! ❌";
        }
    }
}

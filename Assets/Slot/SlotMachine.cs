using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SlotMachine : MonoBehaviour
{
    public Transform[] reelContainers; // Assign Reel Parents in Unity (Reel1, Reel2, Reel3)
    private Image[][] reels; // Holds images for each reel
    private int[] activeIndices; // Track active image index per reel
    public Button spinButton;

    public GameObject winObject;
    public GameObject loseObject;

    private bool isSpinning = false;

    [Header("Spin Speed Settings")]
    public float baseSpinDuration = 2f; // Base duration each reel spins
    public float slowDownTime = 1.5f; // Time to slow down smoothly
    public float initialSpeed = 0.05f; // Speed at which reels initially spin
    public float minFinalWaitTime = 0.2f; // Minimum delay before stopping

    void Start()
    {
        spinButton.onClick.AddListener(StartSpin);
        InitializeReels();
        ResetGame();
    }

    void InitializeReels()
    {
        reels = new Image[reelContainers.Length][];
        activeIndices = new int[reelContainers.Length];

        for (int i = 0; i < reelContainers.Length; i++)
        {
            int childCount = reelContainers[i].childCount;
            reels[i] = new Image[childCount];

            for (int j = 0; j < childCount; j++)
            {
                reels[i][j] = reelContainers[i].GetChild(j).GetComponent<Image>();
            }

            activeIndices[i] = Random.Range(0, childCount); // Start with a random image
            SetActiveImage(i, activeIndices[i]);
        }
    }

    void StartSpin()
    {
        if (isSpinning) return;

        isSpinning = true;
        ResetGame(); // Ensure everything is reset before spinning
        StartCoroutine(SpinReels());
    }

    IEnumerator SpinReels()
    {
        for (int i = 0; i < reelContainers.Length; i++)
        {
            StartCoroutine(SpinSingleReel(i));
            yield return new WaitForSeconds(0.5f); // Small delay before the next reel starts spinning
        }
    }

    IEnumerator SpinSingleReel(int reelIndex)
    {
        float spinTime = baseSpinDuration + Random.Range(0.5f, 1.5f); // Randomized spin duration per reel
        float elapsedTime = 0f;
        float speed = initialSpeed;

        while (elapsedTime < spinTime)
        {
            activeIndices[reelIndex] = (activeIndices[reelIndex] + 1) % reels[reelIndex].Length;
            SetActiveImage(reelIndex, activeIndices[reelIndex]);

            yield return new WaitForSeconds(speed);
            elapsedTime += speed;

            // Apply slowdown effect
            if (elapsedTime >= spinTime - slowDownTime)
            {
                speed *= 1.2f; // Increase delay gradually to slow down
            }
        }

        // Remove this line because it overrides the final stopping position
        // activeIndices[reelIndex] = Random.Range(0, reels[reelIndex].Length);

        // Set final image based on last position in loop
        SetActiveImage(reelIndex, activeIndices[reelIndex]);

        if (reelIndex == reelContainers.Length - 1) // If last reel stopped, check result
        {
            isSpinning = false;
            CheckWinCondition();
        }
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
        int id1 = activeIndices[0];
        int id2 = activeIndices[1];
        int id3 = activeIndices[2];

        if (id1 == id2 && id2 == id3)
        {
            winObject.SetActive(true);
            loseObject.SetActive(false);
        }
        else
        {
            winObject.SetActive(false);
            loseObject.SetActive(true);
        }
    }

    void ResetGame()
    {
        winObject.SetActive(false);
        loseObject.SetActive(false);
        for (int i = 0; i < reelContainers.Length; i++)
        {
            activeIndices[i] = Random.Range(0, reels[i].Length);
            SetActiveImage(i, activeIndices[i]);
        }
    }
}

using System;
using System.Collections.Generic;
using UnityEngine;

public enum GamePhase
{
    Start,
    Keycard1
}

public enum ECollectible
{
    EntryKeycard
}

public class GameManager : Singleton<GameManager>
{
    public static event Action<GamePhase> OnGamePhaseChanged;

    private List<ECollectible> pickedCollectibles = new List<ECollectible>();

    void OnEnable()
    {
        Keycard.OnCollectiblePicked += AddCollectible;
    }

    void OnDisable()
    {
        Keycard.OnCollectiblePicked -= AddCollectible;
    }

    private void AddCollectible(ECollectible col)
    {
        pickedCollectibles.Add(col);
    }

    public bool ContainsCollectible(ECollectible col) => pickedCollectibles.Contains(col);
}
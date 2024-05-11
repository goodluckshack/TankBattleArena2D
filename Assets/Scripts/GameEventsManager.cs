using System;
using UnityEngine;

public class GameEventsManager : MonoBehaviour
{
    public static GameEventsManager Instance { get; private set; }

    public MapEvents MapEvents { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        MapEvents = new();
    }
}

public class MapEvents
{
    public event Action<Vector2Int> OnPlayerMoved;
    public void PlayerMoved(Vector2Int position)
    {
        OnPlayerMoved?.Invoke(position);
    }
}

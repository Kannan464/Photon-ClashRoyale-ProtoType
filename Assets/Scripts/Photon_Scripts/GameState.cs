


[System.Serializable]
public class GameState
{
    public PlayerState player1, player2;
    public int CurrentState = 0;
}

[System.Serializable]
public class PlayerState
{
    public float ElixirAmount = 0;
    public int PlayerHealth = 100;
    public string PlayerID;
}
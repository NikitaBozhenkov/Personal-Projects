using System;

public class EventBroker
{
    public static event Action GameStateChanged;
    public static event Action ButtonTap;
    public static event Action InputMade;
    public static event Action PlayerCanMove;
    public static event Action KillPlayer;
    public static event Action PlayerKilled;
    public static event Action LineDeleted;
    public static event Action CoinPicked;

    public static void CallGameStateChanged() {
        GameStateChanged?.Invoke();
    }

    public static void CallButtonTap() {
        ButtonTap?.Invoke();
    }

    public static void CallInputMade() {
        InputMade?.Invoke();
    }

    public static void CallPlayerCanMove() {
        PlayerCanMove?.Invoke();
    }

    public static void CallKillPlayer() {
        KillPlayer?.Invoke();
    }

    public static void CallPlayerKilled() {
        PlayerKilled?.Invoke();
    }

    public static void CallLineDeleted() {
        LineDeleted?.Invoke();
    }

    public static void CallCoinPicked() {
        CoinPicked?.Invoke();
    }
}

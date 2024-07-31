using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameConfig
{
    public static int MAX_HEART = 5;

    public static float TIME_COUNT_DOWN = 300f;

    //public static string COUNT_DOWN_TIMER = "CountdownTimer";

    public static string LAST_HEART_LOSS = "LastHeartLoss";

    public static float OFFSET_PLATE = 0.2f;

    public static string TXT_REFRESH = "refresh tray to get new stack options";

    public static string TXT_SWAP = "drag any stack to move it";

    public static string TXT_HAMMER = "tap any stack to clear it";

    public static int LEVEL_REFRESH = 6;

    public static int LEVEL_HAMMER = 9;

    public static int LEVEL_SWAP = 14;

    public static int LEVEL_YELLOW = 2;

    public static int LEVEL_PURPLE = 5;

    public static int LEVEL_PINK = 8;

    public static int LEVEL_RANDOM = 12;

    public static int LEVEL_ORANGE = 16;

    public static int COIN_HAMMER = 250;

    public static int COIN_SWAP = 400;

    public static int COIN_REFRESH = 200;

    public static bool EnoughCoinBuyHammer
    {
        get
        {
            return SaveGame.Coin >= COIN_HAMMER;
        }
    }

    public static bool EnoughCoinBuySwap
    {
        get
        {
            return SaveGame.Coin >= COIN_SWAP;
        }
    }

    public static bool EnoughCoinBuyRefresh
    {
        get
        {
            return SaveGame.Coin >= COIN_REFRESH;
        }
    }

    public static string DATACOIN = "DATACOIN";
}

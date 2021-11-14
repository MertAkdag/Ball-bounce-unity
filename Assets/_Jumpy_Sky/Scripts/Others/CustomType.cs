using System.Collections.Generic;
using UnityEngine;

namespace CBGames
{
    public enum IngameState
    {
        Ingame_Playing,
        Ingame_Revive,
        Ingame_GameOver,
        Ingame_CompletedLevel,
    }

    public enum PlayerState
    {
        Player_Prepare,
        Player_Living,
        Player_Died,
        Player_CompletedLevel,
    }


    public enum PlatformType
    {
        SQUARE,
        HEXAGON,
        HEPTAGON,
        OCTAGON,
        CIRCLE,
    }

    public enum PlatformSize
    {
        SMALL,
        NORMAL,
        BIG,
    }

    [System.Serializable]
    public class PlatformSelectionData
    {
        [SerializeField] private PlatformType platformType = PlatformType.SQUARE;
        public PlatformType PlatformType { get { return platformType; } }

        [SerializeField] private List<PlatformController> listPlatformControl = new List<PlatformController>();
        public List<PlatformController> ListPlatformControl { get { return listPlatformControl; } }
    }

    [System.Serializable]
    public class LevelConfig
    {
        [Header("Level Number Configuration")]
        [SerializeField] private int minLevel = 1;
        public int MinLevel { get { return minLevel; } }
        [SerializeField] private int maxLevel = 1;
        public int MaxLevel { get { return maxLevel; } }

        [Header("Background Colors Configuration")]
        [SerializeField] private Color backgroundTopColor = Color.white;
        public Color BackgroundTopColor { get { return backgroundTopColor; } }
        [SerializeField] private Color backgroundBottomColor = Color.white;
        public Color BackgroundBottomColor { get { return backgroundBottomColor; } }


        [Header("Background Music Configuration")]
        [SerializeField] private SoundClip musicClip = null;
        public SoundClip MusicClip { get { return musicClip; } }


        [Header("Platform Colors Configuration")]
        [SerializeField] private int jumpSteps = 10;
        public int JumpSteps { get { return jumpSteps; } }
        [SerializeField] private List<PlatformColorsConfig> listPlatformColorsConfig = new List<PlatformColorsConfig>();
        public List<PlatformColorsConfig> ListPlatformColorsConfig { get { return listPlatformColorsConfig; } }


        [Header("List Platform Configuration")]
        [SerializeField] private List<PlatformConfig> listPlatformConfig = new List<PlatformConfig>();
        public List<PlatformConfig> ListPlatformConfig { get { return listPlatformConfig; } }
    }

    [System.Serializable]
    public class PlatformConfig
    {
        [Header("Platform Type And Size Configuration")]
        [SerializeField] private PlatformType platformType = PlatformType.SQUARE;
        public PlatformType PlatformType { get { return platformType; } }
        [SerializeField] private PlatformSize platformSize = PlatformSize.SMALL;
        public PlatformSize PlatformSize { get { return platformSize; } }


        [Header("Platform Number Configuration")]
        [SerializeField] private int minPlatform = 1;
        public int MinPlatform { get { return minPlatform; } }
        [SerializeField] private int maxPlatform = 1;
        public int MaxPlatform { get { return maxPlatform; } }

        [Header("Platform Distance Configuration")]
        [SerializeField] private int platformDistance = 3;
        public float PlatformDistance { get { return platformDistance; } }

        [Header("XAxis Devitation Configuration")]
        [SerializeField] private float minXAxisDevitation = -2f;
        public float MinXAxisDevitation { get { return minXAxisDevitation; } }
        [SerializeField] private float maxXAxisDevitation = 2f;
        public float MaxXAxisDevitation { get { return maxXAxisDevitation; } }

        [Header("Moving Parameters Configuration")]
        [SerializeField] [Range(0f, 1f)] private float movingPlatformFrequency = 0.1f;
        public float MovingPlatformFrequency { get { return movingPlatformFrequency; } }
        [SerializeField] private float movingLeftDistance = 0.5f;
        public float MovingLeftDistance { get { return movingLeftDistance; } }
        [SerializeField] private float movingRightDistance = 0.5f;
        public float MovingRightDistance { get { return movingRightDistance; } }
        [SerializeField] private float movingSpeed = 1f;
        public float MovingSpeed { get { return movingSpeed; } }
        [SerializeField] private LerpType lerpType = LerpType.Liner;
        public LerpType LerpType { get { return lerpType; } }


        [Header("Coin Number Configuration")]
        [SerializeField] [Range(0f, 1f)] private float coinFrequency = 0.1f;
        public float CoinFrequency { get { return coinFrequency; } }
        [SerializeField] private int minCoinNumber = 0;
        public int MinCoinNumber { get { return minCoinNumber; } }
        [SerializeField] private int maxCoinNumber = 3;
        public int MaxCoinNumber { get { return maxCoinNumber; } }

    }


    [System.Serializable]
    public class PlatformColorsConfig
    {
        [SerializeField] private Color platformColor = Color.white;
        public Color PlatformColor { get { return platformColor; } }
        [SerializeField] private Color platformInnerColor = Color.white;
        public Color PlatformInnerColor { get { return platformInnerColor; } }
    }



    [System.Serializable]
    public class PlatformCreationData
    {
        public PlatformType PlatformType { private set; get; }
        public void SetPlatformType(PlatformType type)
        {
            PlatformType = type;
        }


        public PlatformSize PlatformSize { private set; get; }
        public void SetPlatformSize(PlatformSize size)
        {
            PlatformSize = size;
        }


        public float PlatformDistance { private set; get; }
        public void SetPlatformDistance(float platformDistance)
        {
            PlatformDistance = platformDistance;
        }


        public float XAxisDevitation { private set; get; }
        public void SetXAxisDevitation(float xAxisDevitation)
        {
            XAxisDevitation = xAxisDevitation;
        }


        public float CoinFrequency { private set; get; }
        public void SetCoinFrequency(float coinFrequency)
        {
            CoinFrequency = coinFrequency;
        }
        public int CoinNumber { private set; get; }
        public void SetCoinNumber(int coinNumber)
        {
            CoinNumber = coinNumber;
        }


        public PlatformActionData PlatformActionData { private set; get; }
        public void SetPlatformActionData(PlatformActionData data)
        {
            PlatformActionData = data;
        }
    }

    [System.Serializable]
    public class PlatformActionData
    {
        public float MovingPlatformFrequency { private set; get; }
        public void SetMovingPlatformFrequency(float frequency)
        {
            MovingPlatformFrequency = frequency;
        }


        public float MovingLeftDistance { private set; get; }
        public void SetMovingLeftDistance(float leftDistance)
        {
            MovingLeftDistance = leftDistance;
        }

        public float MovingRightDistance { private set; get; }
        public void SetMovingRightDistance(float rightDistance)
        {
            MovingRightDistance = rightDistance;
        }

        public float MovingSpeed { private set; get; }
        public void SetMovingSpeed(float speed)
        {
            MovingSpeed = speed;
        }


        public LerpType LerpType { private set; get; }
        public void SetLerpType(LerpType lerpType)
        {
            LerpType = lerpType;
        }
    }

    public class PlayerLeaderboardData
    {
        public string Name { private set; get; }
        public void SetName(string name)
        {
            Name = name;
        }

        public int Level { private set; get; }
        public void SetLevel(int level)
        {
            Level = level;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public abstract class RelicSO : ScriptableObject
{
    [SerializeField] protected StatSO _playerStats;
    [SerializeField] protected RelicRarity rarity;
    [SerializeField] protected Sprite spriteIcon;
    [SerializeField] protected string relicName;
    [SerializeField] protected GameObject prefab;
    [SerializeField] protected int cost;
    [SerializeField] protected bool cursed = false;
    [TextArea(3, 4)]
    [SerializeField] protected string relicDescription;
    protected string relicDescriptionOrig;
    protected string relicDescriptionEdit;
    [TextArea(2, 4)]
    [SerializeField] protected string flavorTextDescription;
    [SerializeField] protected List<RelicDescriptionSetting> descSettings = new List<RelicDescriptionSetting>();
    [SerializeField] protected string _pickupSFX = "event:/Relic/RelicPickUp";
    [SerializeField] protected string _dropSFX = "event:/Relic/RelicDrop";

    #region Properties
    public string Name { get { return relicName; } }
    public string FlavorText {  get { return flavorTextDescription; } }
    public string RelicDescription { get { return relicDescription; } set { relicDescription = value; } }
    public string RelicDescriptionOriginal { get { return relicDescriptionOrig; } }
    public RelicRarity Rarity {  get { return rarity; } }
    public Sprite Icon { get { return spriteIcon; } }
    public GameObject Prefab { get { return prefab; } }
    public int Cost { get { return cost; } }
    public StatSO Stats { get { return _playerStats; } }
    public bool Cursed { get { return cursed; } }

    #endregion

    #region Relic Description

    [System.Serializable]
    protected class RelicDescriptionSetting
    {
        public UniqueDescriptionEnum unique_type;
        public UIColors textColor;
        [TextArea(2, 2)]
        public string text;

        public enum UIColors
        {
            ORANGE_GOLD,
            AQUA,
            NEON_GREEN,
            CURSE_RED,
            CURSE_PURPLE,
            WHITE,
            LIGHT_GREY,
            COMMMON_BLUE,
            UNCOMMON_GREEN,
            RARE_RED,
            LEGENDARY_GOLD,
            LIGHT_PURPLE
        }
        public enum UniqueDescriptionEnum
        {
            EVOLES_BY,
            MERGED_FROM,
            REQUIRED_FOR,
            CURSE_NAME,
            STAT_BONUS,
            EVOLUTION_COUNTER
        }
        #region Color Hex Codes

        private string orangeGoldHex = "#FFC500";
        private string aquaHex = "#00ffff";
        private string neonGreenHex = "#11ff00";
        private string curseRedHex = "#c20000";
        private string cursePurpleHex = "#be26ff";
        private string whiteHex = "#ffffff";
        private string lightGreyHex = "#c4c4c4";
        private string commonBlueHex = "#00AFFF";
        private string uncommonGreenHex = "#00FF91";
        private string rareRedHex = "#FF4747";
        private string legendaryGoldHex = "#FFBB47";
        private string lightPurpleHEX = "#f2a1ff";

        #endregion

        public string GetColorHex(UIColors c)
        {
            switch(c)
            {
                case UIColors.ORANGE_GOLD:
                    return orangeGoldHex;
                case UIColors.AQUA:
                    return aquaHex;
                case UIColors.NEON_GREEN:
                    return neonGreenHex;
                case UIColors.CURSE_RED:
                    return curseRedHex;
                case UIColors.CURSE_PURPLE:
                    return cursePurpleHex;
                case UIColors.WHITE:
                    return whiteHex;
                case UIColors.LIGHT_GREY:
                    return lightGreyHex;
                case UIColors.COMMMON_BLUE:
                    return commonBlueHex;
                case UIColors.UNCOMMON_GREEN:
                    return uncommonGreenHex;
                case UIColors.RARE_RED:
                    return rareRedHex;
                case UIColors.LEGENDARY_GOLD:
                    return legendaryGoldHex;
                case UIColors.LIGHT_PURPLE:
                    return lightPurpleHEX;
            }

            return "";
        }
    }

    #endregion

    public virtual void Initialize(StatSO playerstats)
    {
        _playerStats = playerstats;
        relicDescriptionOrig = relicDescription;
    }

    public virtual void OnPickup()
    {
        AudioManager.PlayOneShot(_pickupSFX);
    }
    public virtual void OnDrop()
    {
        AudioManager.PlayOneShot(_pickupSFX);
    }
    public abstract void ActivateBuffs();
    public virtual void ResetRelic()
    {

    }

    // Update Description to show values if needed
    public string UpdateDescription(bool skipStat)
    {
         /*
         * 1. Base Description +
         * 2. Evolves By + Merged From + Required For
         * 3. Stat Bonus
         * 4. Curse Name
         */

        string mainBody = "";

        mainBody += relicDescription;
        foreach (RelicDescriptionSetting s in descSettings)
        {
            if (s.unique_type == RelicDescriptionSetting.UniqueDescriptionEnum.STAT_BONUS
                && skipStat)
            {
                continue;
            }
            else
            {
                mainBody += "<br>" + "<color=" + s.GetColorHex(s.textColor) + ">" + s.text;
            }

        }

        return mainBody;
    }
}

public enum RelicRarity
{
    COMMON,
    UNCOMMON,
    RARE,
    LEGENDARY
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using static InventoryItem;

public class AttributesSystem : MonoBehaviour
{
    [SerializeField]
    Transform strengthUiElement;
    [SerializeField]
    Transform dexterityUIElement;
    [SerializeField]
    Transform charismaUIElement;
    [SerializeField]
    Transform intelligenceUIElement;
    [SerializeField]
    Transform luckUIElement;
    [SerializeField]
    TextMeshProUGUI unspendAP;
    [SerializeField]
    TextMeshProUGUI infoText;

    public float playerDamageResist { get; private set; }
    public float playerFinalDamage { get; private set; }
    public float playerXPGainCoef { get; private set; }
    public float playerLuckCoef { get; private set; }
    public float playerMaxHP { get; private set; }
    public float playerAttackSpeed { get; private set; }

    public float playerMoveSpeed { get; private set; }
    public float playerSprintSpeed { get; private set; }
    public float playerDodgeDistance { get; private set; }

    stat armor = new stat(StatType.Armor);
    stat attackDamage = new stat(StatType.Damage);
    stat attackSpeed = new stat(StatType.AttackSpeed, 1);
    stat moveSpeed = new stat(StatType.MovementSpeed, 7);
    stat maxHp = new stat(StatType.MaxHP, 100);

    stat strength = new stat(StatType.Strength, 10);
    stat dexterity = new stat(StatType.Dexterity, 10);
    stat charisma = new stat(StatType.Charisma, 10);
    stat intelligence = new stat(StatType.Intelligence, 10);
    stat luck = new stat(StatType.Luck, 10);

    XPSystem playerXPSystem;

    float temporaryStrengthValue;
    float temporaryDexterityValue;
    float temporaryCharismaValue;
    float temporaryIntelligenceValue;
    float temporaryLuckValue;
    int oldAPValue;

    List<Transform> abilitiesUIElements = new List<Transform>();
    List<stat> stats = new List<stat>();

    private void Start()
    {
        playerXPSystem = gameObject.GetComponent<XPSystem>();
        abilitiesUIElements.Add(strengthUiElement);
        abilitiesUIElements.Add(dexterityUIElement);
        abilitiesUIElements.Add(charismaUIElement);
        abilitiesUIElements.Add(intelligenceUIElement);
        abilitiesUIElements.Add(luckUIElement);

        stats.Add(armor);
        stats.Add(attackDamage);
        stats.Add(attackSpeed);
        stats.Add(moveSpeed);
        stats.Add(maxHp);
        stats.Add(strength);
        stats.Add(dexterity);
        stats.Add(charisma);
        stats.Add(intelligence);
        stats.Add(luck);

        updateAttributes();
    }

    public void updateAttributes()
    {
        if (playerXPSystem == null) return; //If player XP system is null, then AttributeSystem was not started yet
        oldAPValue = playerXPSystem.attributesPoints;

        foreach (stat item in stats)
        {
            item.cleanValue = item.baseValue;
        }

        List<EquippableItem> items = GetComponent<PlayerInventory>().getEquippedItems();
        foreach (EquippableItem item in items)
        {
            if (item == null) continue;
            foreach(EquipebleItemStat itemStat in item.itemStats)
            {
                stat attribute = stats.Find(x => x.StatName == itemStat.attributeToChange);
                if (itemStat.isProcent) attribute.procentValue += itemStat.value;
                else if (itemStat.isAbsolute && attribute.absoluteValue < itemStat.value) attribute.absoluteValue = itemStat.value;
                else attribute.cleanValue += itemStat.value;
            }
        }

        foreach (stat item in stats)
        {
            if (item.absoluteValue != -1) item.finalValue = item.absoluteValue;
            else item.finalValue = item.cleanValue + (item.cleanValue * (item.procentValue / 100));
        }

        playerDamageResist = armor.finalValue * (strength.finalValue / 10);
        playerFinalDamage = attackDamage.finalValue * (strength.finalValue / 10);
        playerXPGainCoef = intelligence.finalValue / 10;
        playerLuckCoef = luck.finalValue / 10;
        playerMaxHP = maxHp.finalValue + (strength.finalValue - 10) * 5;
        playerAttackSpeed = attackSpeed.finalValue * dexterity.finalValue / 10;
        playerMoveSpeed = moveSpeed.finalValue * (dexterity.finalValue / 10);
        playerSprintSpeed = playerMoveSpeed * 1.75f;
        playerDodgeDistance = playerMoveSpeed / 2;

        temporaryStrengthValue = strength.baseValue;
        temporaryDexterityValue = dexterity.baseValue;
        temporaryCharismaValue = charisma.baseValue;
        temporaryIntelligenceValue = intelligence.baseValue;
        temporaryLuckValue = luck.baseValue;

        GetComponent<PlayerHitPoints>().changeMaxHp(playerMaxHP);

        updateUi();
    }

    public void plusButtonPressed(string value)
    {
        playerXPSystem.changeAPValue(playerXPSystem.attributesPoints - 1);
        unspendAP.text = "Unspend AP: " + playerXPSystem.attributesPoints;
        GameObject attributeChange;
        switch (value)
        {
            case "strength":
                temporaryStrengthValue++;
                attributeChange = strengthUiElement.Find("AttributeChange").gameObject;
                attributeChange.SetActive(true);
                attributeChange.GetComponent<TextMeshProUGUI>().text = "+ " + (temporaryStrengthValue - strength.baseValue);
                break;
            case "dexterity":
                temporaryDexterityValue++;
                attributeChange = dexterityUIElement.Find("AttributeChange").gameObject;
                attributeChange.SetActive(true);
                attributeChange.GetComponent<TextMeshProUGUI>().text = "+ " + (temporaryDexterityValue - dexterity.baseValue);
                break;
            case "charisma":
                temporaryCharismaValue++;
                attributeChange = charismaUIElement.Find("AttributeChange").gameObject;
                attributeChange.SetActive(true);
                attributeChange.GetComponent<TextMeshProUGUI>().text = "+ " + (temporaryCharismaValue - charisma.baseValue);
                break;
            case "intelligence":
                temporaryIntelligenceValue++;
                attributeChange = intelligenceUIElement.Find("AttributeChange").gameObject;
                attributeChange.SetActive(true);
                attributeChange.GetComponent<TextMeshProUGUI>().text = "+ " + (temporaryIntelligenceValue - intelligence.baseValue);
                break;
            case "luck":
                temporaryLuckValue++;
                attributeChange = luckUIElement.Find("AttributeChange").gameObject;
                attributeChange.SetActive(true);
                attributeChange.GetComponent<TextMeshProUGUI>().text = "+ " + (temporaryLuckValue - luck.baseValue);
                break;
        }

        if(playerXPSystem.attributesPoints <= 0) foreach(Transform item in abilitiesUIElements) item.Find("AddOneXP").gameObject.SetActive(false);
    }

    public void acceptButtonPressed()
    {
        strength.baseValue = temporaryStrengthValue;
        dexterity.baseValue = temporaryDexterityValue;
        charisma.baseValue = temporaryCharismaValue;
        intelligence.baseValue = temporaryIntelligenceValue;
        luck.baseValue = temporaryLuckValue;
        updateAttributes();
    }

    public void cancelButtonPressed()
    {
        playerXPSystem.changeAPValue(oldAPValue);
        updateAttributes();
    }

    void updateUi()
    {
        unspendAP.text = "Unspend AP: " + playerXPSystem.attributesPoints;
        strengthUiElement.Find("AttributeValue").GetComponent<TextMeshProUGUI>().text = Mathf.RoundToInt(strength.baseValue).ToString();
        dexterityUIElement.Find("AttributeValue").GetComponent<TextMeshProUGUI>().text = Mathf.RoundToInt(dexterity.baseValue).ToString();
        charismaUIElement.Find("AttributeValue").GetComponent<TextMeshProUGUI>().text = Mathf.RoundToInt(charisma.baseValue).ToString();
        intelligenceUIElement.Find("AttributeValue").GetComponent<TextMeshProUGUI>().text = Mathf.RoundToInt(intelligence.baseValue).ToString();
        luckUIElement.Find("AttributeValue").GetComponent<TextMeshProUGUI>().text = Mathf.RoundToInt(luck.baseValue).ToString();

        if (playerXPSystem.attributesPoints > 0) foreach (Transform item in abilitiesUIElements) item.Find("AddOneXP").gameObject.SetActive(true);
        else foreach (Transform item in abilitiesUIElements) item.Find("AddOneXP").gameObject.SetActive(false);
        foreach (Transform item in abilitiesUIElements) item.Find("AttributeChange").gameObject.SetActive(false);

        string additionalInfo = "";
        foreach (stat item in stats) additionalInfo += item.getValuesInLine();
        additionalInfo += "\n";
        additionalInfo += $"<b>Damage resist</b>: {playerDamageResist} =  armor({armor.finalValue}) * (strength({strength.finalValue}) / 10) \n";
        additionalInfo += $"<b>XP gain coeficient</b>: {playerXPGainCoef} =  intelligence({intelligence.finalValue}) / 10 \n";
        additionalInfo += $"<b>Luck coeficient</b>: {playerLuckCoef} =  luck({luck.finalValue}) / 10 \n";
        additionalInfo += $"<b>Real max hit points</b>: {playerMaxHP} =  max HP from items({maxHp.finalValue}) + (Strength({strength.finalValue}) - 10) * 5 \n";
        additionalInfo += $"<b>Real attack speed</b>: {playerAttackSpeed} =  attack speed from items({attackSpeed.finalValue}) + dexterity({dexterity.finalValue}) / 10 \n";
        additionalInfo += $"<b>Real move speed</b>: {playerMoveSpeed} =  move speed from items({moveSpeed.finalValue}) + dexterity({dexterity.finalValue}) / 10 \n";
        additionalInfo += $"<b>Sprint speed</b>: {playerSprintSpeed} =  real move speed({playerMoveSpeed}) * 1.75 \n";
        additionalInfo += $"<b>Dodge distance</b>: {playerDodgeDistance} =  real move speed({playerMoveSpeed}) / 2 \n";
        infoText.text = additionalInfo;

    }

    public class stat
    {
        public stat(StatType statType, float startValue = 0)
        {
            StatName = statType;
            baseValue = startValue;
            cleanValue = startValue;
            procentValue = 0;
            absoluteValue = -1;
            finalValue = 0;
        }
        public StatType StatName { get; }
        public float baseValue;
        public float cleanValue;
        public float procentValue;
        public float absoluteValue;
        public float finalValue;

        public string getValuesInLine()
        {
            if(absoluteValue != -1 ) return $"{StatName.ToString()}: {absoluteValue} is absolute value \n";
            return $"<b>{StatName.ToString()}</b>: {finalValue} = Base value({baseValue}) + Buff in hard number({cleanValue - baseValue}) + Buff in procent number({procentValue}%) \n";
        }
    }
}

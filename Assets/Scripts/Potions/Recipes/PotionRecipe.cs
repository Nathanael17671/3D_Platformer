using System.Collections.Generic;

[System.Serializable]
public class PotionRecipe
{
    public List<PotionType> ingredients = new List<PotionType>();
    public bool useNumber = false;
    public int requiredCode = -1;
    public PotionType result;
}
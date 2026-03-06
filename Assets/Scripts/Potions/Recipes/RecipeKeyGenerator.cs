using System.Collections.Generic;
using System.Linq;

public static class RecipeKeyGenerator
{
    public static string Generate(List<PotionType> ingredients, bool useNumber, int code)
    {
        List<int> ids = new List<int>();

        foreach (var i in ingredients)
            ids.Add((int)i);

        ids.Sort();

        string key = string.Join("_", ids);

        if (useNumber)
            key += "_" + code;

        return key;
    }
}
namespace CrystalApi.Library;

public static class NumberGenerator
{
    public static int RandomNum()
    {
        Random numberGen = new Random();
        int amountToOutput = 6;
        int minimumRange = 100000;
        int maximumRange = 999999;
        int randomNumber = 0;

        for(var i = 0; i < amountToOutput; i++)
        {
            randomNumber = numberGen.Next(minimumRange, maximumRange);
        }
        return randomNumber;
    }
}
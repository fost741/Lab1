public enum Family
{
    Strings,
    Woodwinds,
    Brass,
    Percussion,
    Keyboard
}

public enum Brand
{
    Yamaha,
    Gibson,
    Pearl,
    Roland
}

public class Instruments
{
    public string name;
    public Brand brand;
    public string model;
    public int keyCount;
    public int stringCount;
    public bool isElectric;
    public bool isConnected;
    public Family family;
    public int currentVolume;

    public string Tune()
    {
            if (isElectric && !isConnected)
            {
                return $"Can not tune. Instrument {name} is not connected to power.";
            }
            
            string instName = name.ToLower();
            if (family == Family.Strings)
            {
                
                if ((instName.Contains("ukulele") || instName.Contains("violin") && stringCount!= 4))
                {
                    return $"Instrument {name} can not be tuned: string amount is not 4. Your input: {stringCount}";
                }
                else if ((instName.Contains("guitar") && stringCount != 6 && stringCount != 7 && stringCount != 12))
                {
                    return $"Instrument {name} can not be tuned: string amount should be 6 / 7 / 12. Your input: {stringCount}";

                }
                else if (stringCount < 4) return $"Instrument {name} can not be tuned: string amount can not be < 4. Your input: {stringCount}";

        }
            if (family == Family.Keyboard)
            {
                if (!isElectric && keyCount != 88 )
                {
                    return $"Instrument {name} can not be tuned: key amount is not 88. Your input: {keyCount}";
                }
                else if (keyCount != 61 && keyCount != 76 && keyCount != 88)
                {
                    return $"Instrument {name} can not be tuned: key amount should be (61 / 76 / 88). Your input: {keyCount}";

                }

            }
            
        return $"{name} was successfuly tuned.";
    }
    public string Play() { return name; }
    public string Connect() { return name; }


    
}


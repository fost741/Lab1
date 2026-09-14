
public class Instruments
{
    public string name;
    public Brand brand;
    public int keyCount;
    public int stringCount;
    public bool isElectric;
    public bool isConnected;
    public Family family;
    public bool isTuned;
    public int currentVolume;

    public string Tune() //настроювання інструменту
    {
        if (isElectric && !isConnected)
        {
            isTuned = false;
            return $"Can not tune. Instrument {name} is not connected to power.";
        }

        string instName = name.ToLower();
        if (family == Family.Strings)
        {

            if ((instName.Contains("ukulele") || instName.Contains("violin")) && stringCount != 4)
            {
                isTuned = false;
                return $"Instrument {name} can not be tuned: string amount is not 4. Your input: {stringCount}";
            }
            else if ((instName.Contains("guitar") && stringCount != 6 && stringCount != 7 && stringCount != 12))
            {
                isTuned = false;
                return $"Instrument {name} can not be tuned: string amount should be 6 / 7 / 12. Your input: {stringCount}";

            }
            else if (stringCount < 4)
            {
                isTuned = false;
                return $"Instrument {name} can not be tuned: string amount can not be < 4. Your input: {stringCount}";
            }

        }
        if (family == Family.Keyboard)
        {
            if (!isElectric && keyCount != 88)
            {
                isTuned = false;
                return $"Instrument {name} can not be tuned: key amount is not 88. Your input: {keyCount}";
            }
            else if (keyCount != 61 && keyCount != 76 && keyCount != 88)
            {
                isTuned = false;
                return $"Instrument {name} can not be tuned: key amount should be (61 / 76 / 88). Your input: {keyCount}";

            }

        }
        isTuned = true;
        return $"{name} was successfuly tuned.";
    }
    public string Play() 
    {
        if (!isTuned) return $"Can not play {name}. The instrument is not tuned.";
        if (isElectric) return $"Instrument {name} is playing at {currentVolume}% volume.";
        else return $"Instrument {name} is playing.";

    }
    public string Connect() //підключення інструменту до електроживлення
    {
        if (!isElectric) return $"Instrument {name} is not electric and does not require power connection.";

        isConnected = true;
        return $"Instrument {name} was successfuly connected to power.";
    }

    public string SetVolume(int volume) //встановлення гучності інструменту
    {
        if (!isElectric) return $"Can not set volume for this instrument. Instrument {name} is acoustic instrument.";
        if (volume < 0 || volume > 100) return $"Can not set this volume. Volume should be between 0 and 100.";
        currentVolume = volume;
        return $"Volume for instrument {name} is set to {currentVolume}%.";
    }

}


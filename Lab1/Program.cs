using System.Collections.Specialized;

namespace Lab1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.Clear();

            int maxArg = Menu("Enter the max amount of elements (N > 0): ", 1, 10);
            List<Instruments> instruments = new List<Instruments>();

            while (true)
            {
                Console.WriteLine("\n------MENU------" +
                                    "\n1. Add new\n" +
                                    "2. Show all objects\n" +
                                    "3. Search for object\n" +
                                    "4. Show behaviour\n" +
                                    "5. Delete object\n" +
                                    "0. Exit");
                int choice = Menu("Choose option: ", 0, 5);
                switch (choice)
                {
                    case 1: AddObj(instruments, maxArg); break;
                    case 2: ShowAll(instruments); break;
                    case 3: Search(instruments); break;
                    case 4: ShowBehaviour(instruments); break;
                    case 5: DeleteObj(instruments); break;
                    case 0:
                        Console.WriteLine("Program is ended. Goodbye!");
                        return;
                }
            }
        }

        static int Menu(string message, int min, int max)
        {
            int a;
            while (true)
            {
                Console.Write(message);
                if (int.TryParse(Console.ReadLine(), out a) && a >= min && a <= max) return a;
                Console.WriteLine($"Invalid input. Enter number from {min} to {max} ");
            }
        }

        static void AddObj(List<Instruments> list, int max)
        {
            if (list.Count >= max)
            {
                Console.WriteLine($"Error. Max permitted number of objects is {max}. ");
                return;
            }

            Console.WriteLine("\nAdding options" +
                                "\n1. Add manually\n" +
                                "2. Add randomly\n");
            int mode = Menu("Choose option: ", 1, 2);

            Instruments inst = new Instruments();
            if (mode == 1)
            {
                while (true)
                {
                    Console.Write("Enter name(f.e. piano): ");
                    string name = Console.ReadLine()?.Trim();
                    if (!string.IsNullOrEmpty(name)) { inst.name = name; break; }
                    Console.WriteLine("Name can not be empty space ");
                }

                Family selectedFamily;
                while (true)
                {
                    Console.Write("Enter family(strings / woodwinds / brass / percussion / keyboard): ");
                    string inp = Console.ReadLine();

                    if (!string.IsNullOrEmpty(inp) && Enum.TryParse<Family>(inp, true, out selectedFamily) && Enum.IsDefined(typeof(Family), selectedFamily))
                    {
                        if ((inst.name.ToLower().Contains("guitar") || inst.name.ToLower().Contains("violin") || inst.name.ToLower().Contains("ukulele")) && selectedFamily != Family.Strings)
                        {
                            Console.WriteLine($"Wrong family. Instrument {inst.name} belongs to STRINGS family."); continue;
                        }
                        else if ((inst.name.ToLower().Contains("piano") || inst.name.ToLower().Contains("keyboard")) && selectedFamily != Family.Keyboard)
                        {
                            Console.WriteLine($"Wrong family. Instrument {inst.name} belongs to KEYBOARD family."); continue;
                        }
                        else if ((inst.name.ToLower().Contains("flute") || inst.name.ToLower().Contains("clarinet")) && selectedFamily != Family.Woodwinds)
                        {
                            Console.WriteLine($"Wrong family. Instrument {inst.name} belongs to WOODWINDS family."); continue;
                        }
                        else if ((inst.name.ToLower().Contains("trumpet")) && selectedFamily != Family.Brass)
                        {
                            Console.WriteLine($"Wrong family. Instrument {inst.name} belongs to BRASS family."); continue;
                        }
                        else if ((inst.name.ToLower().Contains("drum")) && selectedFamily != Family.Percussion)
                        {
                            Console.WriteLine($"Wrong family. Instrument {inst.name} belongs to PERCUSSION family."); continue;
                        }

                        inst.family = selectedFamily;
                        break;
                    }
                    Console.WriteLine("Invalid input. Try again.");
                }


                Brand selectedBrand;
                while (true)
                {
                    Console.Write("Enter brand(f.e. yamaha): ");
                    string inp = Console.ReadLine();
                    if (!string.IsNullOrEmpty(inp) && Enum.TryParse<Brand>(inp, true, out selectedBrand) && Enum.IsDefined(typeof(Brand), selectedBrand))
                    {
                        inst.brand = selectedBrand;
                        break;
                    }
                    Console.WriteLine("Invalid input. Try again.");
                }

                while (true)
                {
                    Console.Write("Need power or not (1 - yes, 0 - no): ");
                    string inp = Console.ReadLine();

                    if (inp == "1") { inst.isElectric = true; break;}

                    else if (inp == "0") { inst.isElectric = false; break;}
                    else Console.WriteLine("Invalid input. Try again.");

                }

                inst.stringCount = StringCount(inst.name, inst.family);
                inst.keyCount = KeyCount(inst.family, inst.isElectric);

                inst.isConnected = false;
                inst.isTuned = false;
                inst.currentVolume = inst.isElectric ? Menu("Enter start volume (0-100%): ", 0, 100) : 0;
            }
            else
            {
                Random rand = new Random();
                string[] names = { "guitar", "violin", "piano", "keyboard", "drums", "ukulele", "flute", "clarinet", "trumpet" };
                inst.name = names[rand.Next(names.Length)];

                string nameToLow = inst.name.ToLower();
                if (nameToLow.Contains("guitar") || nameToLow.Contains("violin") || nameToLow.Contains("ukulele")) inst.family = Family.Strings;
                else if (nameToLow.Contains("piano") || nameToLow.Contains("keyboard")) inst.family = Family.Keyboard;
                else if (nameToLow.Contains("flute") || nameToLow.Contains("clarinet")) inst.family = Family.Woodwinds;
                else if (nameToLow.Contains("trumpet")) inst.family = Family.Brass;
                else inst.family = Family.Percussion;

                Array brands = Enum.GetValues(typeof(Brand));
                inst.brand = (Brand)brands.GetValue(rand.Next(brands.Length));
                inst.isElectric = rand.Next(0, 2) == 1;

                if (inst.family == Family.Strings)
                {
                    inst.stringCount = inst.name.Contains("guitar") ? 6 : 4;
                    inst.keyCount = 0;
                }
                else if (inst.family == Family.Keyboard)
                {
                    inst.keyCount = inst.isElectric ? 61 : 88;
                    inst.stringCount = 0;
                }
                else
                {
                    inst.stringCount = 0;
                    inst.keyCount = 0;
                }

                inst.isConnected = false;
                inst.isTuned = false;
                inst.currentVolume = inst.isElectric ? 50 : 0;
                Console.WriteLine("Object has been generated successfuly! ");
            }

            list.Add(inst);
            Console.WriteLine($"Instrument {inst.name} is added ({list.Count} / {max})");
        }

        static void ShowAll(List<Instruments> list)
        {
            if (list.Count == 0)
            {
                Console.WriteLine("\nList is empty!"); return;
            }

            Console.WriteLine(new string('-', 98));
            Console.WriteLine($"| {"№",-3} | {"Name",-15} | {"Family",-10} | {"Brand",-8} | {"Electric",-8} | {"Keys",-5} | {"Strings",-7} | {"Tuned",-6} | {"Vol",-4} |");
            Console.WriteLine(new string('-', 98));

            for (int i = 0; i < list.Count; i++)
            {
                PrintRow(i + 1, list[i]);
            }
            Console.WriteLine(new string('-', 98));
        }

        static void PrintRow(int num, Instruments inst)
        {
            string keys = inst.keyCount == 0 ? "-" : inst.keyCount.ToString();
            string strings = inst.stringCount == 0 ? "-" : inst.stringCount.ToString();
            string volume = inst.isElectric == false ? "-" : inst.currentVolume.ToString();

            Console.WriteLine($"| {num,-3} | {inst.name,-15} | {inst.family,-10} | {inst.brand,-8} | {inst.isElectric,-8} | {keys,-5} | {strings,-7} | {inst.isTuned,-6} | {volume,-4} |");
        }

        static void Search(List<Instruments> list)
        {
            if (list.Count == 0)
            {
                Console.WriteLine("\nList is empty!"); return;
            }

            int choice = Menu("\nChoose option to search: \n1-name \n2-family \n3-brand \n4-is electric \n0-back to main menu\n", 0, 4);
            int count = 0;

            switch (choice)
            {
                case 1:
                    while (true)
                    {
                        Console.WriteLine("Enter name to find: ");
                        string searchName = Console.ReadLine().ToLower().Trim();

                        if (!string.IsNullOrEmpty(searchName))
                        {
                            for (int i = 0; i < list.Count; i++)
                            {
                                if (list[i].name.ToLower().Contains(searchName))
                                {
                                    count++;
                                    PrintRow(i + 1, list[i]);
                                }
                            }
                        }
                        break;
                    }
                    break;
                case 2:
                    Console.Write("Enter family (Strings, Woodwinds, Brass, Percussion, Keyboard): ");
                    string familyInp = Console.ReadLine();

                    if (Enum.TryParse(familyInp, true, out Family searchFamily) && Enum.IsDefined(typeof(Family), searchFamily))
                    {
                        for (int i = 0; i < list.Count; i++)
                        {
                            if (list[i].family == searchFamily)
                            {
                                count++;
                                PrintRow(i + 1, list[i]);
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid family name.");
                        return;
                    }
                    break;
                case 3:
                    Console.Write("Enter brand (Yamaha, Gibson, Pearl, Roland): ");
                    string brandInp = Console.ReadLine();

                    if (Enum.TryParse(brandInp, true, out Brand searchBrand) && Enum.IsDefined(typeof(Brand), searchBrand))
                    {
                        for (int i = 0; i < list.Count; i++)
                        {
                            if (list[i].brand == searchBrand)
                            {
                                count++;
                                PrintRow(i + 1, list[i]);
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid brand name.");
                    }
                    break;
                case 4:
                    int electric = Menu("Search for: 1 - Electric, 0 - Acoustic: ", 0, 1);
                    bool isElec = electric == 1;

                    for (int i = 0; i < list.Count; i++)
                    {
                        if (list[i].isElectric == isElec)
                        {
                            count++;
                            PrintRow(i + 1, list[i]);
                        }
                    }
                    break;


                case 0:
                    return;
            }
            if (count == 0)
            {
                Console.WriteLine("Such elements were not found. ");
            }
        }

        static void ShowBehaviour(List<Instruments> list)
        {
            if (list.Count == 0) { Console.WriteLine("\nNo instruments to display."); return; }

            ShowAll(list);
            int index = Menu("Choose instrument number to show behaviour (0 to cancel): ", 0, list.Count);

            while (true)
            {
                Console.WriteLine("\n------BEHAVIOUR MENU------" +
                                    "\n1. Tune\n" +
                                    "2. Play\n" +
                                    "3. Connect\n" +
                                    "4. Set volume\n" +
                                    "0. Back to main menu");
                int choice = Menu("Choose option: ", 0, 4);

                switch (choice)
                {
                    case 1:
                        Console.WriteLine(list[index - 1].Tune());
                        break;
                    case 2:
                        Console.WriteLine(list[index - 1].Play());
                        break;
                    case 3:
                        Console.WriteLine(list[index - 1].Connect());
                        break;
                    case 4:
                        if (!list[index - 1].isElectric)
                        {
                            Console.WriteLine("Error: This instrument is not electric and does not have volume control.");
                            break;
                        }
                        int newVolume = Menu("Enter new volume (0-100%): ", 0, 100);
                        Console.WriteLine(list[index - 1].SetVolume(newVolume));
                        break;
                    case 0:
                        return;
                }
            }
        }

        static void DeleteObj(List<Instruments> list)
        {
            if (list.Count == 0) { Console.WriteLine("\nNo instruments to delete."); return; }
            ShowAll(list);
            int index = Menu("Choose instrument number to delete (0 to cancel): ", 0, list.Count);
            if (index == 0) return;
            Instruments instToDel = list[index - 1];
            list.RemoveAt(index - 1);
            Console.WriteLine($"Instrument {instToDel.name} has been deleted.");
        }

        static int StringCount(string name, Family family)
            {
                if (family != Family.Strings) return 0;
                int strings;
                string nameLow = name.ToLower();

                while (true)
                {
                    Console.Write("Enter string count: ");
                    string inp = Console.ReadLine();
                    if (int.TryParse(inp, out strings))
                    {
                        if ((nameLow.Contains("ukulele") || nameLow.Contains("violin")) && strings != 4)
                        {
                            Console.WriteLine($"Error: {name} must have exactly 4 strings.");
                            continue;
                        }

                        if (nameLow.Contains("guitar") && strings != 6 && strings != 7 && strings != 12)
                        {
                            Console.WriteLine("Error. Guitar must have 6, 7, or 12 strings.");
                            continue;
                        }

                        if (strings < 4)
                        {
                            Console.WriteLine("Error: String instrument must have at least 4 strings.");
                            continue;
                        }

                        return strings;
                    }

                    Console.WriteLine("Invalid input. Try again.");
                }
            }

       
        
            static int KeyCount(Family family, bool isElectric)
            {
                if (family != Family.Keyboard) return 0;
                int keys;

                while (true)
                {
                    Console.Write("Enter key count: ");
                    string inp = Console.ReadLine();
                    if (int.TryParse(inp, out keys))
                    {
                        if (!isElectric && keys != 88)
                        {
                            Console.WriteLine("Error: Acoustic keyboard instruments (like piano) must have 88 keys!");
                            continue;
                        }
                        if (isElectric && keys != 61 && keys != 76 && keys != 88)
                        {
                            Console.WriteLine("Error: Electric keyboard instruments must have 61, 76 or 88 keys!");
                            continue;
                        }
                        return keys;
                    }
                    Console.WriteLine("Invalid input. Try again.");

                }
            }
        }
    }

using System;

class MUDGame
{
    static char[] roomSymbols = { 'E', 'M', 'L', 'T', 'H', 'B' };

    static int playerHP = 10; // Player's health
    static int playerDamage = 1; // Player's base damage
    static int playerExtraHP = 0; // Player's extra health from treasure
    static int playerExtraDamage = 0; // Player's extra damage from treasure
    static int goblinHP = 2; // Goblin's base health
    static int goblinDamage = 1; // Goblin's base damage
    static int bossHP = 5; // Boss's base health
    static int bossDamage = 2; // Boss's base damage
    static int trapDamage = 5; // Trap's base damage

    static int currentFloor = 1; // Track the current floor
    static bool bossDefeated = false; // Track if the boss has been defeated
    static int bossesDefeated = 0; // Track the number of bosses defeated

    static int goblinsKilled = 0; // Track the number of goblins killed
    static int score = 0; // Player's score

    static void Main(string[] args)
    {
        Console.Title = "Goblin Tower";
        Console.WriteLine(@"
 _______  _______  _______  ___      ___   __    _    _______  _______  _     _  _______  ______   
|       ||       ||  _    ||   |    |   | |  |  | |  |       ||       || | _ | ||       ||    _ |  
|    ___||   _   || |_|   ||   |    |   | |   |_| |  |_     _||   _   || || || ||    ___||   | ||  
|   | __ |  | |  ||       ||   |    |   | |       |    |   |  |  | |  ||       ||   |___ |   |_||_ 
|   ||  ||  |_|  ||  _   | |   |___ |   | |  _    |    |   |  |  |_|  ||       ||    ___||    __  |
|   |_| ||       || |_|   ||       ||   | | | |   |    |   |  |       ||   _   ||   |___ |   |  | |
|_______||_______||_______||_______||___| |_|  |__|    |___|  |_______||__| |__||_______||___|  |_|");

        Console.WriteLine("Move with WASD, Press Enter to Start...");
        Console.ReadLine();

        int mapWidth = 5;
        int mapHeight = 10;
        char[,] map = GenerateMap(mapWidth, mapHeight);
        int playerRow = 0; // Initial player position
        int playerCol = 0;

    Console.WriteLine("Welcome to the MUD Game!");
        Console.WriteLine("Legend: E - Empty Room, M - Monster Room, L - Treasure Room, T - Trap Room, H - Healing Fountain, B - Boss Room, P - Player");
        Console.WriteLine();

        string roomInfo = ""; // To store room information

        while (playerHP > 0)
        {
            DisplayMap(map);
            Console.WriteLine($"Player HP: {playerHP}");
            Console.WriteLine($"Player Damage: {playerDamage}");
            Console.WriteLine($"Current Floor: {currentFloor}");
            Console.WriteLine($"Goblins Killed: {goblinsKilled}");
            Console.WriteLine($"Bosses Defeated: {bossesDefeated}");
            Console.WriteLine($"Score: {score}");
            Console.WriteLine(roomInfo); // Display room information

            var key = Console.ReadKey(true).Key;

            if (key == ConsoleKey.R)
            {
                map = GenerateMap(mapWidth, mapHeight);
                playerRow = 0;
                playerCol = 0;
                playerHP = 10;
                playerExtraHP = 0;
                playerDamage = 1;
                playerExtraDamage = 0;
                bossDefeated = false;
                bossesDefeated = 0; // Reset the number of bosses defeated
                roomInfo = ""; // Clear room information on reroll
                currentFloor = 1; // Reset the current floor
                goblinsKilled = 0; // Reset the number of goblins killed
                score = 0; // Reset the score
                continue;
            }

            int newPlayerRow = playerRow;
            int newPlayerCol = playerCol;

            // Update player's new position based on user input
            switch (key)
            {
                case ConsoleKey.W:
                    if (playerRow > 0) newPlayerRow--;
                    break;
                case ConsoleKey.S:
                    if (playerRow < mapHeight - 1) newPlayerRow++;
                    break;
                case ConsoleKey.A:
                    if (playerCol > 0) newPlayerCol--;
                    break;
                case ConsoleKey.D:
                    if (playerCol < mapWidth - 1) newPlayerCol++;
                    break;
                case ConsoleKey.Escape:
                    return;
            }

            char currentRoomSymbol = map[newPlayerRow, newPlayerCol];

            if (currentRoomSymbol != 'T')
            {
                // Handle room events
                roomInfo = GetRoomInfo(currentRoomSymbol);
                if (currentRoomSymbol == 'H')
                {
                    playerHP = 10 + playerExtraHP; // Healing Fountain restores player's HP and extra HP
                }
                else if (currentRoomSymbol == 'M' && bossesDefeated < currentFloor)
                {
                    playerHP -= goblinDamage * currentFloor; // Player encounters a Goblin and takes damage
                    goblinsKilled++;
                    score += 10; // Increase score for killing a goblin
                    if (playerHP <= 0)
                    {
                        if (playerHP <= 0)
                        {
                            Console.Clear();
                            Console.WriteLine($"Game Over! You reached floor {currentFloor}.");
                            Console.WriteLine($"Goblins Killed: {goblinsKilled}");
                            Console.WriteLine($"Bosses Defeated: {bossesDefeated}");
                            Console.WriteLine($"Score: {score}");
                            Console.ReadKey();
                            return;
                        }
                    }
                }
                else if (currentRoomSymbol == 'L')
                {
                    roomInfo = "You found a Treasure Room! You gain 1 extra damage and 5 extra health.";
                    playerExtraDamage++;
                    playerExtraHP += 5;
                    playerDamage = 1 + playerExtraDamage; // Update player's damage
                    score += 50; // Increase score for finding treasure
                }
                else if (currentRoomSymbol == 'B' && bossesDefeated < currentFloor)
                {
                    playerHP -= bossDamage * currentFloor; // Player encounters the Boss (Ogre) and takes damage
                    if (playerHP <= 0)
                    {
                        Console.Clear();
                        Console.WriteLine($"Game Over! You reached floor {currentFloor}.");
                        Console.WriteLine($"Goblins Killed: {goblinsKilled}");
                        Console.WriteLine($"Bosses Defeated: {bossesDefeated}");
                        Console.WriteLine($"Score: {score}");
                        Console.ReadKey();
                        return;
                    }

                    // Check if the Boss has been defeated
                    if (playerHP > 0)
                    {
                        roomInfo = "You have defeated the Ogre!";
                        bossesDefeated++;
                        score += 100; // Increase score for defeating the boss

                        if (bossesDefeated == currentFloor)
                        {
                            // Increase floor level and reset the map
                            currentFloor++;
                            map = GenerateMap(mapWidth, mapHeight);
                            playerRow = 0;
                            playerCol = 0;

                            // Scale goblin, boss, and trap damage based on floor level
                            goblinHP += currentFloor;
                            goblinDamage += currentFloor;
                            bossHP += currentFloor;
                            bossDamage += currentFloor;
                            trapDamage += currentFloor;
                        }
                    }
                }
            }
            else
            {
                roomInfo = $"You stepped into a trap and took {trapDamage * currentFloor} damage!";
                playerHP -= trapDamage * currentFloor;
                if (playerHP <= 0)
                {
                    Console.Clear();
                    Console.WriteLine($"Game Over! You reached floor {currentFloor}.");
                    Console.WriteLine($"Goblins Killed: {goblinsKilled}");
                    Console.WriteLine($"Bosses Defeated: {bossesDefeated}");
                    Console.WriteLine($"Score: {score}");
                    Console.ReadKey();
                    return;
                }
            }

            // Update the map to move the player
            map[playerRow, playerCol] = 'E';
            map[newPlayerRow, newPlayerCol] = 'P';
            playerRow = newPlayerRow;
            playerCol = newPlayerCol;
        }
    }

    static char[,] GenerateMap(int width, int height)
    {
        char[,] map = new char[height, width];
        Random rand = new Random();

        for (int row = 0; row < height; row++)
        {
            for (int col = 0; col < width; col++)
            {
                map[row, col] = roomSymbols[rand.Next(roomSymbols.Length)];
            }
        }

        // Set the player's initial position
        map[0, 0] = 'P';

        // Ensure there's only one boss room per floor
        int bossCount = 0;
        for (int row = 0; row < height; row++)
        {
            for (int col = 0; col < width; col++)
            {
                if (map[row, col] == 'B')
                {
                    if (bossCount == 0)
                    {
                        bossCount++;
                    }
                    else
                    {
                        map[row, col] = 'M';
                    }
                }
            }
        }

        return map;
    }

    static void DisplayMap(char[,] map)
    {
        int height = map.GetLength(0);
        int width = map.GetLength(1);

        Console.Clear(); // Clear the console to update the map

        for (int row = 0; row < height; row++)
        {
            for (int col = 0; col < width; col++)
            {
                Console.Write(map[row, col] + "\t");
            }
            Console.WriteLine();
        }
    }

    static string GetRoomInfo(char roomSymbol)
    {
        switch (roomSymbol)
        {
            case 'E':
                return "Empty room. Keep moving.";
            case 'M':
                return "You encountered a Goblin! Prepare for a fight.";
            case 'L':
                return "You found a Treasure Room! Grab the loot.";
            case 'T':
                return "You stepped on a trap. Ouch!";
            case 'H':
                return "You found a Healing Fountain. Rest and recover.";
            case 'B':
                return "You entered the Boss Room. You see an ogre! Get ready for a tough battle!";
            default:
                return "Here be dragons.";
        }
    }
}

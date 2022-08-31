using System;
using System.Collections.Generic;
using System.Linq;

//Battle of the Kingdoms. By Caroline Wallbäck 

namespace LexiconBattle
{
    public class Character
    {
        public string name;
        public int strength;
        public int health;
        public int wins = 0;
        public int losses = 0;
        public int gear = 0;
        

        public Dictionary<int, string> items = new Dictionary<int, string>
        {
            {0, "no gear"},
            {1, "water gun"},
            {2, "slingshot"},
            {3, "shield"},
            {4, "cannon"}
        };       
    }

    public class Battle
    {
        public Character winningCharacter;
        public Round round;
        public Character battlePlayer;
        public Character battleOpponent;
    }
    
    public class Round
    {
        public int playerRoll;
        public int opponentRoll;
    }

    public class Highscore
    {
        public int score;
        public string playerName;
    }

    public class Difficulty
    {
        public string level;
    }

    class Program
    {
        static void Main(string[]args)
        {
            List <Battle> allBattles = new List<Battle>();
            var difficulty = new Difficulty();

            Random random = new Random();

            Console.WriteLine("Welcome to the Battle of All Kingdoms!");
            Console.WriteLine("Start the game by giving your player a name.");

            Character player = new Character();
            Console.ForegroundColor = ConsoleColor.Green;
            player.name = Console.ReadLine();
            Console.ForegroundColor = ConsoleColor.White;

            Console.Write($"\nYou ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(player.name);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(", must fight to protect all the kingdoms.");
            Console.WriteLine("You will face different opponents every battle, which all have a different strength.");
            Console.WriteLine("If you lose a battle you will decrease your health by your opponent's strength.");
            Console.WriteLine("You and your opponent will each roll a dice and whoever gets the highest roll wins the battle.");
            Console.WriteLine("You will play until your character dies or if you choose to retrieve efter a battle.");
            Console.WriteLine("You must battle at least 3 times before you can choose to retrieve.");
            Console.WriteLine("In the end you will be scored 2 points for every battle you won, ");
            Console.WriteLine("but you will lose 2 points for every loss. You get 3 extra points if you are still alive when the game ends.");
            Console.WriteLine("After each battle, if you win, your gear will be upgraded, and you can choose to use it next battle.");
            Console.WriteLine("If you choose to use your gear it will be downgraded to a lower level, otherwise it will be saved for next battle.\n");

            DifficultyLevel();

            Thread.Sleep(500);
            player.strength = random.Next(3, 11);
            ShowPlayerStatus();

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Are you ready? Press enter to continue");
            Console.ForegroundColor = ConsoleColor.White;
            Console.ReadKey();

            NewRound();

            void DifficultyLevel()
            {
                Console.WriteLine("Choose difficulty level. Easy (E), Medium (M), Hard (H)");
                var level = Console.ReadLine().ToUpper();

                switch (level)
                {
                    case "E" : difficulty.level = "easy";
                    player.health = random.Next(30, 36);
                    break;

                    case "M" : difficulty.level = "medium";
                    player.health = random.Next(25, 31);
                    break;

                    case "H" : difficulty.level = "hard";
                    player.health = random.Next(20, 26);
                    break;

                    default : DifficultyLevel();
                    break;
                }
            }
          
            void NewRound()
            {
                string[] names = { "Garfield", "Titan", "Hercules", "Pluto", "Dracula", "Black Knight", "Claaaws", "Joker", "Bennet", "Big Bad Wolf" };

                Character opponent = new Character();
                opponent.name = names[random.Next(0, 10)];
                opponent.health = random.Next(20, 31);

                Battle battle = new Battle();
                allBattles.Add(battle);
                battle.battlePlayer = player;
                battle.battleOpponent = opponent;
                
                battle.round = new Round();
                
                switch (difficulty.level)
                {
                    case "easy" : 
                    opponent.strength = random.Next(3, 6);
                    battle.round.playerRoll = random.Next(2, 7);
                    battle.round.opponentRoll = random.Next(1, 7);
                    break;

                    case "medium" : 
                    opponent.strength = random.Next(5, 8);
                    battle.round.playerRoll = random.Next(1, 7);
                    battle.round.opponentRoll = random.Next(1, 7);
                    break;

                    case "hard" : 
                    opponent.strength = random.Next(7, 10);
                    battle.round.playerRoll = random.Next(1, 6);
                    battle.round.opponentRoll = random.Next(2, 7);
                    break;

                }
                                
                Thread.Sleep(1000);
                Console.WriteLine("--------------------");
                Console.WriteLine($"Battle no. {allBattles.Count}\n");
                Console.Write("Oh no, here comes your opponent, ");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write(opponent.name);
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(", his current status is: \n");
                Console.WriteLine($"Strength: {opponent.strength}");
                Console.WriteLine($"Health: {opponent.health}\n");
                Console.WriteLine($"You must stop {opponent.name} from taking over your kingdom.");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Press enter to roll the dice");
                Console.ForegroundColor = ConsoleColor.White;
                Console.ReadKey();

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"\nYou rolled a {battle.round.playerRoll}");
                Thread.Sleep(500);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Your opponent {opponent.name} rolled a {battle.round.opponentRoll}");
                Console.ForegroundColor = ConsoleColor.White;
                Thread.Sleep(500);

                if(player.gear > 0)
                {
                    ActivateGearOrNot(battle);
                    Thread.Sleep(500);
                }
                
                RandomHappenings(battle);

                if (battle.round.playerRoll > battle.round.opponentRoll)
                    PlayerWin(opponent, battle);
                
                else if(battle.round.playerRoll == battle.round.opponentRoll)
                {
                    Console.WriteLine("It's a tie!");
                    Thread.Sleep(1000);
                    ShowPlayerStatus();
                    Thread.Sleep(1000);
                    if(allBattles.Count >= 3)
                        PlayOrRetrieve();
                    else
                        NewRound();
                }
                else
                    OpponentWin(opponent, battle);

            }

            void UpgradeGear()
            {
                if(player.gear < 4)
                {
                    player.gear++;
                    Console.WriteLine($"Congratulations, your gear has been upgraded. You now have a {player.items.GetValueOrDefault(player.gear)}.");
                }
            }

            void ActivateGearOrNot(Battle battle)
            {
                Console.WriteLine($"Would you like to use your {player.items.GetValueOrDefault(player.gear)}? (Y/N)");
                var answer = Console.ReadLine().ToUpper();

                switch (answer)
                {
                    case "Y" : 
                    UseGear(battle);
                    break;

                    case "N" : break;

                    default : ActivateGearOrNot(battle);
                    break;
                }
            }

            void UseGear(Battle battle)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                switch(player.gear)
                {
                    case 1:
                    Console.WriteLine("You have used your water gun.");
                    Console.WriteLine("Your opponent slips on the water, and loses 2 strength-points");
                    battle.battleOpponent.strength -= 2;
                    break;

                    case 2 : 
                    Console.WriteLine("You have used your slingshot");
                    Console.WriteLine("Your opponent's dice got hit and you knocked off 2 points.");
                    battle.round.opponentRoll -= 2;
                    Console.WriteLine($"{battle.battleOpponent.name} new roll is {battle.round.opponentRoll}.");
                    break;

                    case 3 : 
                    Console.WriteLine("You have used your shield");
                    Console.WriteLine("You got a minute to rest while using your shield, and gained 5 health-points");
                    player.health += 5;
                    break;

                    case 4 :
                    Console.WriteLine("You have used your cannon");
                    Console.WriteLine("You knocked over your opponent who loses all his strength");
                    battle.battleOpponent.strength = 0;

                    break;
                }

                player.gear--;
                Console.ForegroundColor = ConsoleColor.White;
                Thread.Sleep(1000);
            }

            void RandomHappenings(Battle battle)
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                int number = random.Next(1,11);

                switch (number)
                {
                    case 1 : 
                    Console.WriteLine("Oh no, you tripped on a big ugly rock and lost one of your dice-points.");
                    battle.round.playerRoll--;
                    Console.WriteLine($"Your new dice roll is {battle.round.playerRoll}.");
                    Thread.Sleep(2000);
                    break;

                    case 3 : 
                    Console.WriteLine("AAAHHH! This is an ambush!! Your opponent has company and you are being attacked from all directions. You lose all your dice-points.");
                    battle.round.playerRoll = 0;
                    Console.WriteLine($"Your new dice roll is {battle.round.playerRoll}.");
                    Thread.Sleep(2000);
                    break;

                    case 5 : 
                    Console.WriteLine ("Your opponent got stuck in that trap you set up a few days ago. He loses 2 dice-points.");
                    battle.round.opponentRoll -= 2;
                    Console.WriteLine($"Your opponent {battle.battleOpponent.name} new dice roll is {battle.round.opponentRoll}.");
                    Thread.Sleep(2000);
                    break;

                    case 9 : 
                    Console.WriteLine("The sky is opening up and heavy rain falls. Neither of you can see anything, so you call it a tie.");
                    battle.round.playerRoll = battle.round.opponentRoll;
                    Thread.Sleep(2000);
                    break;

                    default: break;
                }

                Console.ForegroundColor = ConsoleColor.White;
            }

            void PlayerWin(Character opponent, Battle battle)
            {
                battle.winningCharacter = player;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Great work! You won this fight and decreased your opponent's health with {player.strength} points.");
                opponent.health -= player.strength;
                player.wins++;
                UpgradeGear();
                Console.ForegroundColor = ConsoleColor.White;
                Thread.Sleep(2000);
                ShowPlayerStatus();
                Thread.Sleep(1000);
                if (allBattles.Count >= 3)
                    PlayOrRetrieve();
                else
                    NewRound();
            }

            void OpponentWin(Character opponent, Battle battle)
            {
                battle.winningCharacter = opponent;
                player.losses++;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\nOh no, your opponent won, and you were damaged. You lost {opponent.strength} health-points.");
                Console.ForegroundColor = ConsoleColor.White;
                player.health -= opponent.strength;
                Thread.Sleep(2000);
                ShowPlayerStatus();
                Thread.Sleep(1000);
                if(player.health > 0)
                {
                    if (allBattles.Count >= 3)
                        PlayOrRetrieve();
                    else
                        NewRound();
                }
                   
                else
                {
                    Console.WriteLine("--------------------");
                    Console.WriteLine("You are out of health-points, and died.");
                    EndGame();
                }

            }

            void PlayOrRetrieve()
            {
                Console.WriteLine("Do you want to keep playing? (Press Y) or retrieve? (Press N)");
                var input = Console.ReadLine().ToUpper();

                switch (input)
                {
                    case "Y":
                        NewRound();
                        break;
                    case "N":
                        EndGame();
                        break;

                    default: PlayOrRetrieve();
                        break;
                }
            }

            void ShowPlayerStatus()
            {
                Console.WriteLine("--------------------");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\nYour current status:");
                Console.WriteLine($"Your strength: {player.strength}");
                Console.WriteLine($"Your health: {player.health}");
                Console.WriteLine($"Your gear: {player.items.GetValueOrDefault(player.gear)}");
                if(allBattles.Count>0)
                    Console.WriteLine($"Wins: {player.wins}/{player.wins+player.losses}");
                
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.White;
            }

            void EndGame()
            {
                Console.WriteLine("--------------------");
                int score = 0;
                
                score += player.wins*2;
                score -= player.losses*2;

                if (player.health > 0)
                    score += 3;

                Console.WriteLine ($"You battled {allBattles.Count} time(s), you won {player.wins} time(s), {allBattles.Count - player.wins - player.losses} were a tie.");
                Console.WriteLine($"Your total score is: {score} ");
                if(score > 0)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("HOORAAY!! You won the game and all the kingdoms!");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("You lost the game and all your kingdoms");
                }
            
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Would you like to see the scores for all your rounds? (Y/N)");
                var input = Console.ReadLine().ToUpper();

                if (input == "Y")
                {
                    Thread.Sleep(1000);
                    ScoreboardForAllRounds(score);
                }
                else
                {
                    Console.WriteLine("--------------------");
                    bool saveScore = false;
                    if(score > 0)
                    {
                        Console.WriteLine("Save to highscore board? (Y/N)");
                        saveScore = true;
                    }
                    else
                        Console.WriteLine("Would you like to see the highscore board? (Y/N)");

                    var answer = Console.ReadLine().ToUpper();
                    switch (answer)
                    {
                        case "Y":
                            ViewHighscore(score, saveScore);
                            break;
                        case "N":
                            Console.WriteLine("--------------------");
                            Console.WriteLine("Press any key to exit.");
                            Console.ReadKey();
                            Environment.Exit(0);;
                            break;     
                    }   
                }
            }

            void ScoreboardForAllRounds(int score)
            {
                for (int i = 0; i < allBattles.Count; i++)
                {
                    Console.WriteLine($"\nBattle {i + 1}: ");
                    Console.WriteLine($"Opponent: {allBattles[i].battleOpponent.name}. ");
                    Console.WriteLine($"You rolled a {allBattles[i].round.playerRoll} and {allBattles[i].battleOpponent.name} rolled a {allBattles[i].round.opponentRoll}.");
                    Console.WriteLine(allBattles[i].winningCharacter == player ? $"You won this round, and your opponent's health went from {allBattles[i].battleOpponent.health + player.strength} to {allBattles[i].battleOpponent.health}." : $"You lost this round, and lost {allBattles[i].battleOpponent.strength} points from your health."); 
                    Console.WriteLine(allBattles[i].winningCharacter == player ? "You gained 2 points" : "You lost 2 points");
                }

                Console.WriteLine();
                Console.WriteLine(player.health > 0 ? "You gained 3 extra points for being alive at the end of the game.\n" : "You did not receive any bonus points, since your health went down to 0.\n");

                Console.WriteLine("--------------------");
                bool saveScore = false;
                if(score > 0)
                {
                    Console.WriteLine("Save to highscore board? (Y/N)");
                    saveScore = true;
                }
                else
                {
                    Console.WriteLine("Would you like to see the highscore board? (Y/N)");
                }

                var input = Console.ReadLine().ToUpper();
                    switch (input)
                    {
                        case "Y":
                            ViewHighscore(score, saveScore);
                            break;
                        case "N":
                            Console.WriteLine("--------------------");
                            Console.WriteLine("Press any key to exit.");
                            Console.ReadKey();
                            Environment.Exit(0);
                            break;     
                    }
                
                
            }   

            void ViewHighscore(int _score, bool saveScore)
            {
                var scores = new List<Highscore>();
                string nameInput = "";
                Highscore currentPlayer = new Highscore();
                
                List <string> highscoreList = new List<string>();

                try
                {
                    switch(difficulty.level)
                    {
                        case "easy" : 
                        highscoreList = File.ReadAllLines(@"highscoreEasy.txt").ToList();
                        File.WriteAllText(@"highscoreEasy.txt", "");
                        break;

                        case "medium" : 
                        highscoreList = File.ReadAllLines(@"highscoreMedium.txt").ToList();
                        File.WriteAllText(@"highscoreMedium.txt", "");
                        break;

                        case "hard" : 
                        highscoreList = File.ReadAllLines(@"highscoreHard.txt").ToList();
                        File.WriteAllText(@"highscoreHard.txt", "");
                        break;
                    }
                    

                    foreach (string scoreLine in highscoreList)
                    {
                        var line = scoreLine.Split(' ');
                        scores.Add(new Highscore {score = int.Parse(line[0]), playerName = line[1]});
                    }

                }
                catch(Exception)
                {
                    switch(difficulty.level)
                    {
                        case "easy" : 
                        File.WriteAllText(@"highscoreEasy.txt", "");
                        break;

                        case "medium" : 
                        File.WriteAllText(@"highscoreMedium.txt", "");
                        break;

                        case "hard" : 
                        File.WriteAllText(@"highscoreHard.txt", "");
                        break;
                    }
                }

                if(saveScore)
                {
                    Console.WriteLine("Type in your name for the highscore board");
                    nameInput = Console.ReadLine();
                    currentPlayer.score = _score;
                    currentPlayer.playerName = nameInput;
                    scores.Add(currentPlayer);
                }        

                var ordered = scores.OrderByDescending(f => f.score).ToList();

                while(ordered.Count > 10)
                    ordered.RemoveAt(10);

                Console.WriteLine($"\nHIGHSCORES - {difficulty.level.ToUpper()}");
                foreach (var scoreLine in ordered)
                {
                    if(scoreLine == currentPlayer)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        Console.WriteLine("{0}: {1}", scoreLine.score, scoreLine.playerName);
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else
                        Console.WriteLine("{0}: {1}", scoreLine.score, scoreLine.playerName);

                    switch(difficulty.level)
                    {
                        case "easy" : 
                        File.AppendAllText(@"highscoreEasy.txt", $"{scoreLine.score.ToString()} {scoreLine.playerName}\n");
                        break;

                        case "medium" : 
                        File.AppendAllText(@"highscoreMedium.txt", $"{scoreLine.score.ToString()} {scoreLine.playerName}\n");
                        break;

                        case "hard" : 
                        File.AppendAllText(@"highscoreHard.txt", $"{scoreLine.score.ToString()} {scoreLine.playerName}\n");
                        break;
                    }
                    
                }

                if(!ordered.Contains(currentPlayer) && currentPlayer.score > 0)
                {
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine($"\n{currentPlayer.score} {currentPlayer.playerName}");
                    Console.ForegroundColor = ConsoleColor.White;
                }

                Console.WriteLine("--------------------");
                Console.WriteLine("Press any key to exit.");
                Console.ReadKey();
                Environment.Exit(0);
            }
        }
    }
}
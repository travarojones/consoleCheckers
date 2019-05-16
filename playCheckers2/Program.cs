using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace playCheckers
{
    class Program
    {
        static void Main(string[] args)
        {
            string[,] gameBoard = new string[8, 8];//the game and all of its data
            string[,] showBoard = new string[8, 8];//used to show the board
            Dictionary<string, string> spaceName = new Dictionary<string, string>();/*each playabele space will have a corresponding spaceName 
                                                  (enter spaceName["blah"] and in return get its x axis coord then y axis coord seperated by a space)*/
            #region spaceName Assignment
            //each playable spot will have a name to help users make moves 
            //ex: spaceName.Add("B1", "1 0") / if user enters "B1" the corresponding gameBoard space is x-Axis = 1 (space) y-Axis = 0;

            spaceName.Add("A2", "0 1");
            spaceName.Add("A4", "0 3");
            spaceName.Add("A6", "0 5");
            spaceName.Add("A8", "0 7");
            spaceName.Add("B1", "1 0");
            spaceName.Add("B3", "1 2");
            spaceName.Add("B5", "1 4");
            spaceName.Add("B7", "1 6");
            spaceName.Add("C2", "2 1");
            spaceName.Add("C4", "2 3");
            spaceName.Add("C6", "2 5");
            spaceName.Add("C8", "2 7");
            spaceName.Add("D1", "3 0");
            spaceName.Add("D3", "3 2");
            spaceName.Add("D5", "3 4");
            spaceName.Add("D7", "3 6");
            spaceName.Add("E2", "4 1");
            spaceName.Add("E4", "4 3");
            spaceName.Add("E6", "4 5");
            spaceName.Add("E8", "4 7");
            spaceName.Add("F1", "5 0");
            spaceName.Add("F3", "5 2");
            spaceName.Add("F5", "5 4");
            spaceName.Add("F7", "5 6");
            spaceName.Add("G2", "6 1");
            spaceName.Add("G4", "6 3");
            spaceName.Add("G6", "6 5");
            spaceName.Add("G8", "6 7");
            spaceName.Add("H1", "7 0");
            spaceName.Add("H3", "7 2");
            spaceName.Add("H5", "7 4");
            spaceName.Add("H7", "7 6");
            //End spaceName creator
            #endregion


            #region Game Setup
            /*
              In checkers, each player starts with 12 game pieces. Starting postions are set as literals below.
              Each piece will be stored as string ("p1" || "p2" || "k1" || "k2") in the gameBoard which is a string array of pieces and spaces.
              The object of the game is to conquer (jump over) all of your opponent's game pieces.
              If a piece is jumped, it is removed from the gameBoard and is, from then on, an unplayable game piece.
              A regular ("p1" || "p2") piece can only move foreward 1 playable space at a time. 
              If a regular ("p1" || "p2") piece makes it to the opponent's last line , it is promoted to a king piece ("k1' || "k2").
              Kings can move forewards AND BACKWARDS but only on playable squares
            */

            #region Player 1's starting positions
            //"p1" means it is a player 1 game piece 
            //if a "p1" game piece reaches king territory it becomes a king piece (represented by "k1") and plays by a different set of rules
           gameBoard[0,1] = "p1";
           gameBoard[0,3] = "p1";
           gameBoard[0,5] = "p1";
           gameBoard[0,7] = "p1";
           gameBoard[1,0] = "p1";
           gameBoard[1,2] = "p1";
           gameBoard[1,4] = "p1";
           gameBoard[1,6] = "p1";
           gameBoard[2,1] = "p1";
           gameBoard[2,3] = "p1";
           gameBoard[2,5] = "p1";
           gameBoard[2,7] = "p1";
            //player 1's game setup
            #endregion

            #region Player 2's starting positions
            //"p2" means it is a player 2 game piece
            //"p2" as king is ("k2") / and "k2", like "k1" plays by different rules
            gameBoard[5,0] = "p2";
            gameBoard[5,2] = "p2";
            gameBoard[5,4] = "p2";
            gameBoard[5,6] = "p2";
            gameBoard[6,1] = "p2";
            gameBoard[6,3] = "p2";
            gameBoard[6,5] = "p2";
            gameBoard[6,7] = "p2";
            gameBoard[7,0] = "p2";
            gameBoard[7,2] = "p2";
            gameBoard[7,4] = "p2";
            gameBoard[7,6] = "p2";
            //player 2's game setup
            #endregion


            bool whosTurn = true; // when true, its player1's turn / if you don't know what false is, this is maybe a lost cause
            Dictionary<bool, string> yourTurn = new Dictionary<bool, string>();//this will ensure the turns are established (definition below) / will find pieces to match player's turn 
            yourTurn.Add(whosTurn == true, "p1");//type this : yourTurn[whosTurn] (if whosTurn is true, this will retun "p1" as a string)
            yourTurn.Add(whosTurn == false, "p2");//type this : yourTurn[whosTurn] (if whosTurn is false, this will retun "p2" as a string)
                       
            #endregion

            bool gameOn = true;/*the game has been set and is from this point "on".
                                game ends once a player loses all of their game pieces*/
            while (gameOn)
            {
                Console.Clear();//refresh the screen to update gameBoard
                buildGameBoard(gameBoard);//displays the gameBoard to the console

                int[] moveThis = new int[2];//coordinates for player's selected piece to play / look below to fully understand 
                int[] moveTo = new int[2];//player's desired location
                bool makingMoves = true;//if true, a valid move has not yet been made
                bool goodChoice = false;//is false until the user chooses an actual starting piece
                bool legalMove = false;//is false unitl a valid move is made

                #region Users make moves
                while (makingMoves)/*user will be asked to select a piece according to its corresponding square. 
                                      as long as a valid move has not been made, this loop will continue*/
                {
                    #region Choosing a piece to move
                    if (!goodChoice)/*the first step in making a move is to select a valid piece ;
                                    this will loop until a valid piece is selected
                                    look below to see how it is determined*/
                    {
                        Console.Write("{0}, its your turn. Which piece would you like to move?\t", yourTurn[whosTurn]);//asks players to make a move 
                        string moveFrom = Console.ReadLine().ToUpper();//user enters coordinates (hopefully they're valid. (we'll check below) / to upper to compensate the dictionary
                        if (spaceName.Keys.Contains(moveFrom))//this checks to see if the dictionary contains the data entered by the user ("a2" || "A2" and so forth) 
                        //yes there's a matching name
                        {
                            string[] thePlaceholder = spaceName[moveFrom].Split();/*spaceName[moveFrom] returns a string containing the x-Axis "space" and the y-Axis / split by the space to get a string[]
                                            thePlaceholder hase 2 elements the x-Axis as a string in thePlaceholder[0] and the y-Axis also string int thePlaceholder[1]*/
                            moveThis[0] = int.Parse(thePlaceholder[0]);//stores the xAxis as an int 
                            moveThis[1] = int.Parse(thePlaceholder[1]);//stores the yaxis as an int

                            if ((yourTurn[whosTurn] == "p1" && gameBoard[moveThis[0], moveThis[1]] == "p1") || (yourTurn[whosTurn] == "p1" && gameBoard[moveThis[0], moveThis[1]] == "k1"))
                            {                                
                                goodChoice = true;//a playable "p1" piece has been selected
                                makingMoves = true;//move is still incomplete
                            }//end if (player 1) 
                            else if ((yourTurn[whosTurn] == "p2" && gameBoard[moveThis[0], moveThis[1]] == "p2") || (yourTurn[whosTurn] == "p2" && gameBoard[moveThis[0], moveThis[1]] == "k2"))
                            {
                                goodChoice = true;//a playable "p2" piece has been selected
                                makingMoves = true;//move is still incomplete
                            }//end else if (player 2)
                            else if (gameBoard[moveThis[0], moveThis[1]] == null)//the space is empty
                            {
                                Console.WriteLine("Nothing's there, {0}. Let's try again.", yourTurn[whosTurn]);//user chose an empty space
                                goodChoice = false;//a playable piece has not been selected
                                makingMoves = true;//move is still incomplete
                            }//end else if (the selected space is empty)
                            else
                            //opponent's game piece 
                            {
                                if (yourTurn[whosTurn] == "p1" && gameBoard[moveThis[0], moveThis[1]] == "p2")//player 1 chose a player 2 game piece
                                {
                                    Console.WriteLine("Try moving your own piece this time, {0}", yourTurn[whosTurn]);//advises player 1 to try moving a player 1 game piece
                                    goodChoice = false;//a playable piece has not been selected
                                    makingMoves = true;//move is still incomplete
                                }//end if (player 1 check)
                                else if (yourTurn[whosTurn] == "p1" && gameBoard[moveThis[0], moveThis[1]] == "k2")//player 1 chose a player 2 game piece
                                {
                                    Console.WriteLine("Try moving your own piece this time, {0}", yourTurn[whosTurn]);//advises player 1 to try moving a player 1 game piece
                                    goodChoice = false;//a playable piece has not been selected
                                    makingMoves = true;//move is still incomplete
                                }//end else if (opponent's king) 
                                else if (yourTurn[whosTurn] == "p2" && gameBoard[moveThis[0], moveThis[1]] == "p1")//player 2 chose a player 1 game piece
                                {
                                    Console.WriteLine("Try moving your own piece this time, {0}", yourTurn[whosTurn]);//advises player 2 to try moving a player 2 game piece
                                    goodChoice = false;//a playable piece has not been selected
                                    makingMoves = true;//move is still incomplete
                                }//end else if (player 2 check)
                                else if (yourTurn[whosTurn] == "p2" && gameBoard[moveThis[0], moveThis[1]] == "k1")//player 2 chose a player 1 game piece
                                {
                                    Console.WriteLine("Try moving your own piece this time, {0}", yourTurn[whosTurn]);//advises player 2 to try moving a player 2 game piece
                                    goodChoice = false;//a playable piece has not been selected
                                    makingMoves = true;//move is still incomplete
                                }//end else if (opponent's king)
                            }//end else (wrong piece selected)

                        }//end if (checking user's input)
                        else
                        //if user input is not found in the spaceName dictionary
                        {
                            Console.WriteLine("Let's try inputing a playable square.");//explains the problem
                            goodChoice = false;//user makes another selection
                            makingMoves = true;//user is still making their move
                        }//end else for spaceName check
                    }//end if (!goodChoice)
                    #endregion

                    #region Move selected piece
                    else//after a valid piece is selected, the player has to make a valid move. the player is still (making a move)
                    {
                        while (!legalMove)//until player makes a valid move, this will be false  
                        {
                            Console.Write("{0}, where would you like to place the piece?\t", yourTurn[whosTurn]);//where is the player trying to go?
                            string goHere = Console.ReadLine().ToUpper();//user enters desired spot

                            if (spaceName.Keys.Contains(goHere))//if what the user entered is in the dictionary
                            {
                                string[] thePlaceholder = spaceName[goHere].Split();/*spaceName[goHere] returns a string containing the x-Axis "a space" and the y-Axis / split by the space to get a string[]
                                                thePlaceholder hase 2 elements the x-Axis as a string in thePlaceholder[0] and the y-Axis also string int thePlaceholder[1]*/

                                moveTo[0] = int.Parse(thePlaceholder[0]);//stores the xAxis as an int 
                                moveTo[1] = int.Parse(thePlaceholder[1]);//stores the yaxis as an int
                                if (gameBoard[moveTo[0], moveTo[1]] == null )
                                {//if space is blank, and piece is not a king

                                    gameBoard = updatedBoard(moveThis, moveTo, gameBoard, yourTurn[whosTurn]);//checks the move for validation
                                    if (gameBoard[moveTo[0], moveTo[1]] == null)//gameBoard didn't change
                                    {
                                        Console.WriteLine("Let's try this again. You can only move towards your opponent using one of the blue spaces connected to your selected pieces' space.");
                                        goodChoice = false;//user will be asked to start this turn over and select a valid piece
                                        makingMoves = true;//since that move wasn't valid. The player restarts their turn
                                        break;//break the loop and start this turn over
                                    }//end if (gameBoard == null)
                                    else//everything checks out 
                                    {
                                        goodChoice = true;//dont reselect a piece
                                        makingMoves = false;//then player's done making their move
                                        legalMove = true;//the move has been finalized
                                        moveThis = new int[2];//reset the piece selection
                                        moveTo = new int[2];//reset the target destination
                                    }//end else (gameBoard != null)
                                }//end if (moveTo == null)
                                else if (gameBoard[moveTo[0], moveTo[1]] != null)//target destination is not empty
                                {
                                    Console.WriteLine("Something's in that spot, {0}. Try jumping over your opponent's piece or choosing a different piece to move", yourTurn[whosTurn]);
                                    moveThis = new int[2];//reset the piece selection
                                    moveTo = new int[2];//reset the target destination
                                    goodChoice = false;//user reselects a piece to move
                                    makingMoves = true;//the turn is restarted
                                    break;//break the loop and start this turn over
                                }//end else if (moveTo != null)
                            }//end if (spaceName contain input)
                            else
                            {
                                Console.WriteLine("Not a valid move. Press ENTER and try again.");//user's entry wasn't found in the dictionary
                                goodChoice = false;//reselect a piece to move
                                Console.ReadKey();
                            }//end else (input not in dictionary)
                        }//end while (legalMove)

                    }//end else (goodChoice = true) 
                    #endregion

                }//end while (makingMoves)
                #endregion

                #region Check for winner
                int gameCheck = playerTally(gameBoard);//does each player still have eligible pieces? see method.playerTally for explanation
                if (gameCheck == 1)//playerTally returned a 1
                {
                    gameOn = false;//game's over
                    Console.WriteLine("\n\nPlayer 1 wins");//display winner
                    break;//break gameOn loop
                }//end if (playerTally == 1)

                else if (gameCheck == 2)//playerTally returned a 2
                {
                    gameOn = false;//game's over
                    Console.WriteLine("\n\nPlayer 2 wins");//display winner
                    break;//break gameOn loop
                }//end else if (playerTally == 2)
                #endregion

                #region Next player's turn
                if (whosTurn == true)//<- from player 1 
                {                    //to
                    whosTurn = false;//<- player 2
                }//end if (whosTurn == "player 1")
                else if (whosTurn == false)//<- from player 2
                {                          //to 
                    whosTurn = true;       //<- player 1 
                }//end else if (whosTurn == "player 2")
                #endregion

            }//end while (gameOn) 

            Console.WriteLine("Press ENTER to close");
            Console.ReadKey();//Press ENTER to close
        }//End Main


        #region writesBoard
        static void buildGameBoard(string[,] gameBoard)//displays gameBoard to console
        {
            for (int newX = 0; newX < gameBoard.GetLength(0); newX++)//x axis
            {
                for (int newY = 0; newY < gameBoard.GetLength(1); newY++)// y axis 
                {
                    if (newX % 2 == 0 && newY % 2 == 1 || newX % 2 == 1 && newY % 2 == 0)//determines if coords create a playable spot or not
                    {
                        if (gameBoard[newX, newY] == "p1" || gameBoard[newX, newY] == "k1")//if the string inside the coord represents player 1
                        {
                            Console.BackgroundColor = ConsoleColor.Blue;//all playablespots are blue
                            Console.ForegroundColor = ConsoleColor.Yellow;//player 1 is yellow
                            Console.Write(gameBoard[newX,newY].PadLeft(4, ' '));//writes the content / pad left by 4 to ensure all spots are even in size
                            Console.BackgroundColor = ConsoleColor.Black;//changes the background color back to default
                            Console.ForegroundColor = ConsoleColor.White;//return foreground color to default
                        }//end if (player 1 pieces)
                        else if (gameBoard[newX, newY] == "p2" || gameBoard[newX, newY] == "k2")//As above
                        {                                                                       //So below (but with player 2)
                            Console.BackgroundColor = ConsoleColor.Blue;//all playable spots are blue
                            Console.ForegroundColor = ConsoleColor.Green;//player 2 is cyan
                            Console.Write(gameBoard[newX, newY].PadLeft(4, ' '));//write content / pad left by 4 to ensure all spots are even in size
                            Console.BackgroundColor = ConsoleColor.Black;//backgound back to default
                            Console.ForegroundColor = ConsoleColor.White;//return foreground color to default
                        }//end else if (player 2 pieces)
                        else//an empty playable spot
                        {
                            Console.BackgroundColor = ConsoleColor.Blue;//show playable spot as empty
                            Console.ForegroundColor = ConsoleColor.Blue;//guarantees the spot will look empty as the foreground is same color as the background
                            Console.Write("X".PadLeft(4, ' '));//will write an X so the console has something to pad left, ensuring size parity
                            Console.BackgroundColor = ConsoleColor.Black;//return background color to default
                            Console.ForegroundColor = ConsoleColor.White;//return foreground color to default
                        }//end else (empty spot)
                    }//end if (playable spot)
                    else//the spaces that are not playable positions will always be white spaces 
                    {
                        Console.BackgroundColor = ConsoleColor.White;//unplayable spaces are white
                        Console.ForegroundColor = ConsoleColor.White;//because the spot is unplayable it shall always look empty (background same color as foreground)
                        Console.Write("X".PadLeft(4,' '));//will write an X so the console has something to pad left, ensuring size parity
                        Console.BackgroundColor = ConsoleColor.Black;//return background color to default
                        Console.ForegroundColor = ConsoleColor.White;//return foreground color to default
                    }//end else (creates white spaces)
                }//end for (y-axis) 
                            switch(newX)//used to label each horizontal line (row 0 will now be known as row A, row 1 is B, and so on (see subsequent code))
                            {
                                case 0: { Console.Write(" A"); break; }
                                case 1: { Console.Write(" B"); break; }
                                case 2: { Console.Write(" C"); break; }
                                case 3: { Console.Write(" D"); break; }
                                case 4: { Console.Write(" E"); break; }
                                case 5: { Console.Write(" F"); break; }
                                case 6: { Console.Write(" G"); break; }
                                case 7: { Console.Write(" H"); break; }
                            }//end switch (newX)
                        Console.WriteLine();//moves cursor to next line after labeling the row with its corresponding alphabet
            }//end for (x-axis)
                Console.WriteLine(" 1   2   3   4   5   6   7   8  ");//labels each vertical lines (the format is literal (so to adjust the string is to adjust the output)) 
                Console.WriteLine();//this newlines the cursors position to the next line under the borad and its labels

        }//End GameBoard build
        #endregion //rewrites the board after every move

        #region UpdateBoard (moveCheck)
        static string[,] updatedBoard(int[] moveFrom, int[] moveTo, string[,] currentState, string p1op2)   //p1op2 means player 1 or player 2
        {
            string[,] returnBoard = currentState;
            int[] startingSpot = moveFrom;
            int[] goHere = moveTo;
            bool moveMade = false;

            #region  Player 1 Move
            if (p1op2 == "p1")
            {

                #region Not King P1
                if (returnBoard[startingSpot[0], startingSpot[1]] == "p1")
                {
                    if (startingSpot[1] != 0 || startingSpot[1] != 7)
                    {
                        if (goHere[0] == startingSpot[0] + 1 && goHere[1] == startingSpot[1] + 1)
                        {
                            returnBoard[goHere[0], goHere[1]] = "p1";
                            returnBoard[startingSpot[0], startingSpot[1]] = null;
                            moveMade = true;
                        }//end if (p1MoveDownAndRight)
                        else if (goHere[0] == startingSpot[0] + 1 && goHere[1] == startingSpot[1] - 1)
                        {
                            returnBoard[goHere[0], goHere[1]] = "p1";
                            returnBoard[startingSpot[0], startingSpot[1]] = null;
                            moveMade = true;
                        }//end else if (p1MoveDownAndLeft)
                        else if (goHere[0] == startingSpot[0] + 2 && goHere[1] == startingSpot[1] + 2)
                        {
                            if (returnBoard[startingSpot[0] + 1, startingSpot[1] + 1] == "p2" || returnBoard[startingSpot[0] + 1, startingSpot[1] + 1] == "k2")
                            {
                                returnBoard[goHere[0], goHere[1]] = "p1";
                                returnBoard[startingSpot[0] + 1, startingSpot[1] + 1] = null;
                                returnBoard[startingSpot[0], startingSpot[1]] = null;
                                moveMade = true;
                            }//end if (p1JumpRight)
                        }//end else if (p1PlanRightJump)
                        else if (goHere[0] == startingSpot[0] + 2 && goHere[1] == startingSpot[1] - 2)
                        {
                            if (returnBoard[startingSpot[0] + 1, startingSpot[1] - 1] == "p2" || returnBoard[startingSpot[0] - 1, startingSpot[1] + 1] == "k2")
                            {
                                returnBoard[goHere[0], goHere[1]] = "p1";
                                returnBoard[startingSpot[0] + 1, startingSpot[1] - 1] = null;
                                returnBoard[startingSpot[0], startingSpot[1]] = null;
                                moveMade = true;
                            }//end if (p1JumpLeft)
                        }//end else if (p1PlanJumpLeft)
                    }//end if (p1MiddleOfBoard)

                    else if (startingSpot[1] == 0)
                    {
                        if (goHere[0] == startingSpot[0] + 1 && goHere[1] == startingSpot[1] + 1)
                        {
                            returnBoard[goHere[0], goHere[1]] = "p1";
                            returnBoard[startingSpot[0], startingSpot[1]] = null;
                            moveMade = true;
                        }//end if (moveFrom0)
                        else if (goHere[0] == startingSpot[0] + 2 && goHere[1] == startingSpot[1] + 2)
                        {
                            if (returnBoard[startingSpot[0] + 1, startingSpot[1] + 1] == "p2" || returnBoard[startingSpot[0] + 1, startingSpot[1] + 1] == "k2")
                            {
                                returnBoard[goHere[0], goHere[1]] = "p1";
                                returnBoard[startingSpot[0] + 1, startingSpot[1] + 1] = null;
                                returnBoard[startingSpot[0], startingSpot[1]] = null;
                                moveMade = true;
                            }//end if (jumpFrom0)
                        }//end else if (planJump0)
                    }//end else if (on0Edge)

                    else if (startingSpot[1] == 7)
                    {
                        if (goHere[0] == startingSpot[0] + 1 && goHere[1] == startingSpot[1] - 1)
                        {
                            returnBoard[goHere[0], goHere[1]] = "p1";
                            returnBoard[startingSpot[0], startingSpot[1]] = null;
                            moveMade = true;
                        }//end if (moveFrom7)
                        else if (goHere[0] == startingSpot[0] + 2 && goHere[1] == startingSpot[1] - 2)
                        {
                            if (returnBoard[startingSpot[0] + 1, startingSpot[1] - 1] == "p2" || returnBoard[startingSpot[0] + 1, startingSpot[1] - 1] == "k2")
                            {
                                returnBoard[goHere[0], goHere[1]] = "p1";
                                returnBoard[startingSpot[0] + 1, startingSpot[0] - 1] = null;
                                returnBoard[startingSpot[0], startingSpot[1]] = null;
                                moveMade = true;
                            }//end if (jumpFrom7)
                        }//end else if (planJump7)
                    }//end else if (on7Edge)
                }//end if (notKing)
             #endregion
                              
                 #region King P1
                 else if (returnBoard[startingSpot[0], startingSpot[1]] == "k1")
                {
                    if (startingSpot[0] == 7)
                    {
                        if (startingSpot[1] != 0 || startingSpot[1] != 7)
                        {
                            if (goHere[0] == startingSpot[0] - 1 && goHere[1] == startingSpot[1] - 1)
                            {
                                returnBoard[goHere[0], goHere[1]] = "k1";
                                returnBoard[startingSpot[0], startingSpot[1]] = null;
                                moveMade = true;
                            }//end if (king1MoveLeftFromBottom)
                            else if (goHere[0] == startingSpot[0] - 1 && goHere[1] == startingSpot[1] + 1)
                            {
                                returnBoard[goHere[0], goHere[1]] = "k1";
                                returnBoard[startingSpot[0], startingSpot[1]] = null;
                                moveMade = true;
                            }//end if (king1MoveRightFromBottom)
                            else if (goHere[0] == startingSpot[0] - 2 && goHere[1] == startingSpot[1] - 2)
                            {
                                if (returnBoard[startingSpot[0] - 1, startingSpot[1] - 1] == "p2" || returnBoard[startingSpot[0] - 1, startingSpot[1] - 1] == "k2")
                                {
                                    returnBoard[goHere[0], goHere[1]] = "k1";
                                    returnBoard[startingSpot[0] - 1, startingSpot[1] - 1] = null;
                                    returnBoard[startingSpot[0], startingSpot[1]] = null;
                                    moveMade = true;
                                }//end if (king1JumpLeftFromBottom)
                            }//end else if (planKing1JumpLeftFromBottom)
                            else if (goHere[0] == startingSpot[0] - 2 && goHere[1] == startingSpot[1] + 2)
                            {
                                if (returnBoard[startingSpot[0] - 1, startingSpot[1] + 1] == "p2" || returnBoard[startingSpot[0] - 1, startingSpot[1] + 1] == "k2")
                                {
                                    returnBoard[goHere[0], goHere[1]] = "k1";
                                    returnBoard[startingSpot[0] - 1, startingSpot[1] + 1] = null;
                                    returnBoard[startingSpot[0], startingSpot[1]] = null;
                                    moveMade = true;
                                }//end if (king1JumpRightFromBottom)
                            }//end else if (planKing1JumpRightFromBottom)
                        }//end if (notOnEdgeOfBoard)
                        else if (startingSpot[1] == 0)
                        {
                            if (goHere[0] == startingSpot[0] - 1 && goHere[1] == startingSpot[1] + 1)
                            {
                                returnBoard[goHere[0], goHere[1]] = "k1";
                                returnBoard[startingSpot[0], startingSpot[1]] = null;
                                moveMade = true;
                            }//end if (kingMoveFromBottom0Edge)
                            else if (goHere[0] == startingSpot[0] - 2 && goHere[1] == startingSpot[1] + 2)
                            {
                                if (returnBoard[startingSpot[0] - 1, startingSpot[1] + 1] == "p2" || returnBoard[startingSpot[0] - 1, startingSpot[1] + 1] == "k2")
                                {
                                    returnBoard[goHere[0], goHere[1]] = "k1";
                                    returnBoard[startingSpot[0] - 1, startingSpot[1] + 1] = null;
                                    returnBoard[startingSpot[0], startingSpot[1]] = null;
                                    moveMade = true;
                                }//end if (kingJumpRightFromBottom0Edge)
                            }//end else if (planKingJumpRightFromBottom0Edge)
                        }//end else if (movingKingFromBottom0Edge)
                        else if (startingSpot[1] == 7)
                        {
                            if (goHere[0] == startingSpot[0] - 1 && goHere[1] == startingSpot[1] - 1)
                            {
                                returnBoard[goHere[0], goHere[1]] = "k1";
                                returnBoard[startingSpot[0], startingSpot[1]] = null;
                                moveMade = true;
                            }//end if (kingMoveFromBottom7Edge)
                            else if (goHere[0] == startingSpot[0] - 2 && goHere[1] == startingSpot[1] - 2)
                            {
                                if (returnBoard[startingSpot[0] - 1, startingSpot[1] - 1] == "p2" || returnBoard[startingSpot[0] - 1, startingSpot[1] - 1] == "k2")
                                {
                                    returnBoard[goHere[0], goHere[1]] = "k1";
                                    returnBoard[startingSpot[0] - 1, startingSpot[1] - 1] = null;
                                    returnBoard[startingSpot[0], startingSpot[1]] = null;
                                    moveMade = true;
                                }//end if (kingJumpLeftFromBottom7Edge)
                            }//end else if (planJumpLeftFromBottom7Edge)
                        }//end if (movingKingFromBottom7Edge) 
                    }//end if (movingKing1FromBottom)
                    else if (startingSpot[0] == 0)
                    {
                        if (startingSpot[1] != 0 || startingSpot[1] != 7)
                        {
                            if (goHere[0] == startingSpot[0] + 1 && goHere[1] == startingSpot[1] - 1)
                            {
                                returnBoard[goHere[0], goHere[1]] = "k1";
                                returnBoard[startingSpot[0], startingSpot[1]] = null;
                                moveMade = true;
                            }//end if (king1MoveLeftFromTop)
                            else if (goHere[0] == startingSpot[0] + 1 && goHere[1] == startingSpot[1] + 1)
                            {
                                returnBoard[goHere[0], goHere[1]] = "k1";
                                returnBoard[startingSpot[0], startingSpot[1]] = null;
                                moveMade = true;
                            }//end if (king1MoveRightFromTop)
                            else if (goHere[0] == startingSpot[0] + 2 && goHere[1] == startingSpot[1] - 2)
                            {
                                if (returnBoard[startingSpot[0] + 1, startingSpot[1] - 1] == "p2" || returnBoard[startingSpot[0] + 1, startingSpot[1] - 1] == "k2")
                                {
                                    returnBoard[goHere[0], goHere[1]] = "k1";
                                    returnBoard[startingSpot[0] + 1, startingSpot[1] - 1] = null;
                                    returnBoard[startingSpot[0], startingSpot[1]] = null;
                                    moveMade = true;
                                }//end if (king1JumpLeftFromTop)
                            }//end else if (planKing1JumpLeftFromTop)
                            else if (goHere[0] == startingSpot[0] + 2 && goHere[1] == startingSpot[1] + 2)
                            {
                                if (returnBoard[startingSpot[0] + 1, startingSpot[1] + 1] == "p2" || returnBoard[startingSpot[0] + 1, startingSpot[1] + 1] == "k2")
                                {
                                    returnBoard[goHere[0], goHere[1]] = "k1";
                                    returnBoard[startingSpot[0] + 1, startingSpot[1] + 1] = null;
                                    returnBoard[startingSpot[0], startingSpot[1]] = null;
                                    moveMade = true;
                                }//end if (king1JumpRightFromTop)
                            }//end else if (planKing1JumpRightFromTop)
                        }//end if (notOnEdgeOfBoard)
                        else if (startingSpot[1] == 0)
                        {
                            if (goHere[0] == startingSpot[0] + 1 && goHere[1] == startingSpot[1] + 1)
                            {
                                returnBoard[goHere[0], goHere[1]] = "k1";
                                returnBoard[startingSpot[0], startingSpot[1]] = null;
                                moveMade = true;
                            }//end if (kingMoveFromTop0Edge)
                            else if (goHere[0] == startingSpot[0] + 2 && goHere[1] == startingSpot[1] + 2)
                            {
                                if (returnBoard[startingSpot[0] + 1, startingSpot[1] + 1] == "p2" || returnBoard[startingSpot[0] + 1, startingSpot[1] + 1] == "k2")
                                {
                                    returnBoard[goHere[0], goHere[1]] = "k1";
                                    returnBoard[startingSpot[0] - 1, startingSpot[1] + 1] = null;
                                    returnBoard[startingSpot[0], startingSpot[1]] = null;
                                    moveMade = true;
                                }//end if (kingJumpFromTop0Edge)
                            }//end if (planKingJumpFromTop0Edge)
                        }//end if (kingMoveFromTop)
                        else if (startingSpot[1] == 7)
                        {
                            if (goHere[0] == startingSpot[0] + 1 && goHere[1] == startingSpot[1] - 1)
                            {
                                returnBoard[goHere[0], goHere[1]] = "k1";
                                returnBoard[startingSpot[0], startingSpot[1]] = null;
                                moveMade = true;
                            }//end if (kingMoveFromTop7Edge)
                            else if (goHere[0] == startingSpot[0] + 2 && goHere[1] == startingSpot[1] - 2)
                            {
                                if (returnBoard[startingSpot[0] + 1, startingSpot[1] - 1] == "p2" || returnBoard[startingSpot[0] + 1, startingSpot[1] - 1] == "k2")
                                {
                                    returnBoard[goHere[0], goHere[1]] = "k1";
                                    returnBoard[startingSpot[0] + 1, startingSpot[1] - 1] = null;
                                    returnBoard[startingSpot[0], startingSpot[1]] = null;
                                    moveMade = true;
                                }//end if (kingJumpLeftFromTop7Edge)
                            }//end else if (planJumpLeftFromTop7Edge)
                        }
                    }//end else if (movingKing1FromTop)
                    else
                    {
                        if (startingSpot[1] != 0 || startingSpot[1] != 7)
                        {
                            if (goHere[0] == startingSpot[0] - 1 && goHere[1] == startingSpot[1] - 1)
                            {
                                returnBoard[goHere[0], goHere[1]] = "k1";
                                returnBoard[startingSpot[0], startingSpot[1]] = null;
                                moveMade = true;
                            }//end if (king1MoveUpLeft)
                            else if (goHere[0] == startingSpot[0] - 1 && goHere[1] == startingSpot[1] + 1)
                            {
                                returnBoard[goHere[0], goHere[1]] = "k1";
                                returnBoard[startingSpot[0], startingSpot[1]] = null;
                                moveMade = true;
                            }//end if (king1MoveUpRight)
                            else if (goHere[0] == startingSpot[0] + 1 && goHere[1] == startingSpot[1] - 1)
                            {
                                returnBoard[goHere[0], goHere[1]] = "k1";
                                returnBoard[startingSpot[0], startingSpot[1]] = null;
                                moveMade = true;
                            }//end if (king1MoveDownLeft)
                            else if (goHere[0] == startingSpot[0] + 1 && goHere[1] == startingSpot[1] + 1)
                            {
                                returnBoard[goHere[0], goHere[1]] = "k1";
                                returnBoard[startingSpot[0], startingSpot[1]] = null;
                                moveMade = true;
                            }//end if (king1MoveDownRight)
                            else if (goHere[0] == startingSpot[0] - 2 && goHere[1] == startingSpot[1] - 2)
                            {
                                if (returnBoard[startingSpot[0] - 1, startingSpot[1] - 1] == "p2" || returnBoard[startingSpot[0] - 1, startingSpot[1] - 1] == "k2")
                                {
                                    returnBoard[goHere[0], goHere[1]] = "k1";
                                    returnBoard[startingSpot[0] - 1, startingSpot[1] - 1] = null;
                                    returnBoard[startingSpot[0], startingSpot[1]] = null;
                                    moveMade = true;
                                }//end if (king1JumpUpLeft)
                            }//end else if (planKing1JumpUpLeft)
                            else if (goHere[0] == startingSpot[0] - 2 && goHere[1] == startingSpot[1] + 2)
                            {
                                if (returnBoard[startingSpot[0] - 1, startingSpot[1] + 1] == "p2" || returnBoard[startingSpot[0] - 1, startingSpot[1] + 1] == "k2")
                                {
                                    returnBoard[goHere[0], goHere[1]] = "k1";
                                    returnBoard[startingSpot[0] - 1, startingSpot[1] + 1] = null;
                                    returnBoard[startingSpot[0], startingSpot[1]] = null;
                                    moveMade = true;
                                }//end if (king1JumpUpRight)
                            }//end else if (planKing1JumpUpRight)
                            else if (goHere[0] == startingSpot[0] + 2 && goHere[1] == startingSpot[1] - 2)
                            {
                                if (returnBoard[startingSpot[0] + 1, startingSpot[1] - 1] == "p2" || returnBoard[startingSpot[0] + 1, startingSpot[1] - 1] == "k2")
                                {
                                    returnBoard[goHere[0], goHere[1]] = "k1";
                                    returnBoard[startingSpot[0] + 1, startingSpot[1] - 1] = null;
                                    returnBoard[startingSpot[0], startingSpot[1]] = null;
                                    moveMade = true;
                                }//end if (king1JumpDownLeft)
                            }//end else if (planKing1JumpDownLeft)
                            else if (goHere[0] == startingSpot[0] + 2 && goHere[1] == startingSpot[1] + 2)
                            {
                                if (returnBoard[startingSpot[0] + 1, startingSpot[1] + 1] == "p2" || returnBoard[startingSpot[0] + 1, startingSpot[1] + 1] == "k2")
                                {
                                    returnBoard[goHere[0], goHere[1]] = "k1";
                                    returnBoard[startingSpot[0] + 1, startingSpot[1] + 1] = null;
                                    returnBoard[startingSpot[0], startingSpot[1]] = null;
                                    moveMade = true;
                                }//end if (king1JumpDownRight)
                            }//end else if (planKing1JumpDownRight)
                        }//end if (notOnEdgeOfBoard)
                        else if (startingSpot[1] == 0)
                        {
                            if (goHere[0] == startingSpot[0] - 1 && goHere[1] == startingSpot[1] + 1)
                            {
                                returnBoard[goHere[0], goHere[1]] = "k1";
                                returnBoard[startingSpot[0], startingSpot[1]] = null;
                                moveMade = true;
                            }//end if (king1MoveUpFrom0)
                            else if (goHere[0] == startingSpot[0] + 1 && goHere[1] == startingSpot[1] + 1)
                            {
                                returnBoard[goHere[0], goHere[1]] = "k1";
                                returnBoard[startingSpot[0], startingSpot[1]] = null;
                                moveMade = true;
                            }//end else if (king1MoveDownFrom0)
                            else if (goHere[0] == startingSpot[0] - 2 && goHere[1] == startingSpot[1] + 2)
                            {
                                if (returnBoard[startingSpot[0] - 1, startingSpot[1] + 1] == "p2" || returnBoard[startingSpot[0] - 1, startingSpot[1] + 1] == "k2")
                                {
                                    returnBoard[goHere[0], goHere[1]] = "k1";
                                    returnBoard[startingSpot[0] - 1, startingSpot[1] + 1] = null;
                                    returnBoard[startingSpot[0], startingSpot[1]] = null;
                                    moveMade = true;
                                }//end if (king1JumpUpFrom0)
                            }//end else if (planKing1JumpUpFrom0)
                            else if (goHere[0] == startingSpot[0] + 2 && goHere[1] == startingSpot[1] + 2)
                            {
                                if (returnBoard[startingSpot[0] + 1, startingSpot[1] + 1] == "p2" || returnBoard[startingSpot[0] + 1, startingSpot[1] + 1] == "k2")
                                {
                                    returnBoard[goHere[0], goHere[1]] = "k1";
                                    returnBoard[startingSpot[0] + 1, startingSpot[1] + 1] = null;
                                    returnBoard[startingSpot[0], startingSpot[1]] = null;
                                    moveMade = true;
                                }//end if (king1JumpDownFrom0)
                            }//end else if (planKing1JumpDownFrom0)
                        }//end else if (movingKing1From0EdgeNotTopOrBottom)
                        else if (startingSpot[1] == 7)
                        {
                            if (goHere[0] == startingSpot[0] - 1 && goHere[1] == startingSpot[1] - 1)
                            {
                                returnBoard[goHere[0], goHere[1]] = "k1";
                                returnBoard[startingSpot[0], startingSpot[1]] = null;
                                moveMade = true;
                            }//end if (king1MoveUpFrom7)
                            else if (goHere[0] == startingSpot[0] + 1 && goHere[1] == startingSpot[1] - 1)
                            {
                                returnBoard[goHere[0], goHere[1]] = "k1";
                                returnBoard[startingSpot[0], startingSpot[1]] = null;
                                moveMade = true;
                            }//end else if (king1MoveDownFrom7)
                            else if (goHere[0] == startingSpot[0] - 2 && goHere[1] == startingSpot[1] - 2)
                            {
                                if (returnBoard[startingSpot[0] - 1, startingSpot[1] - 1] == "p2" || returnBoard[startingSpot[0] - 1, startingSpot[1] - 1] == "k2")
                                {
                                    returnBoard[goHere[0], goHere[1]] = "k1";
                                    returnBoard[startingSpot[0] - 1, startingSpot[1] - 1] = null;
                                    returnBoard[startingSpot[0], startingSpot[1]] = null;
                                    moveMade = true;
                                }//end if (king1JumpUpFrom7)
                            }//end else if (planKing1JumpUpFrom7)
                            else if (goHere[0] == startingSpot[0] + 2 && goHere[1] == startingSpot[1] - 2)
                            {
                                if (returnBoard[startingSpot[0] + 1, startingSpot[1] - 1] == "p2" || returnBoard[startingSpot[0] + 1, startingSpot[1] - 1] == "k2")
                                {
                                    returnBoard[goHere[0], goHere[1]] = "k1";
                                    returnBoard[startingSpot[0] + 1, startingSpot[1] - 1] = null;
                                    returnBoard[startingSpot[0], startingSpot[1]] = null;
                                    moveMade = true;
                                }//end if (king1JumpDownFrom7)
                            }//end else if (planKing1JumpDownFrom7)
                        }//end else if (movingKing1From7EdgeNotTopOrBottom)            
                    }//end else (notOnTopOrBottom)
                }//end else if (movingP1King)
                #endregion

                if (moveMade)
                {
                    if (goHere[0] == 7) //bottom of board
                    {
                        returnBoard[goHere[0], goHere[1]] = "k1";
                    }//end if (kingQuarters)
                }//end if (moveMade)

            }//end if ("p1" turn)
            #endregion

            #region  Player 2 Move
            if (p1op2 == "p2")
            {

                #region Not King P2
                if (returnBoard[startingSpot[0], startingSpot[1]] == "p2")
                {
                    if (startingSpot[1] != 0 || startingSpot[1] != 7)
                    {
                        if (goHere[0] == startingSpot[0] - 1 && goHere[1] == startingSpot[1] + 1)
                        {
                            returnBoard[goHere[0], goHere[1]] = "p2";
                            returnBoard[startingSpot[0], startingSpot[1]] = null;
                            moveMade = true;
                        }//end if (p2MoveUpAndRight)
                        else if (goHere[0] == startingSpot[0] - 1 && goHere[1] == startingSpot[1] - 1)
                        {
                            returnBoard[goHere[0], goHere[1]] = "p2";
                            returnBoard[startingSpot[0], startingSpot[1]] = null;
                            moveMade = true;
                        }//end else if (p2MoveUpAndLeft)
                        else if (goHere[0] == startingSpot[0] - 2 && goHere[1] == startingSpot[1] + 2)
                        {
                            if (returnBoard[startingSpot[0] - 1, startingSpot[1] + 1] == "p1" || returnBoard[startingSpot[0] - 1, startingSpot[1] + 1] == "k1")
                            {
                                returnBoard[goHere[0], goHere[1]] = "p2";
                                returnBoard[startingSpot[0] - 1, startingSpot[1] + 1] = null;
                                returnBoard[startingSpot[0], startingSpot[1]] = null;
                                moveMade = true;
                            }//end if (p2JumpRight)
                        }//end else if (p2PlanRightJump)
                        else if (goHere[0] == startingSpot[0] - 2 && goHere[1] == startingSpot[1] - 2)
                        {
                            if (returnBoard[startingSpot[0] - 1, startingSpot[1] - 1] == "p1" || returnBoard[startingSpot[0] - 1, startingSpot[1] - 1] == "k1")
                            {
                                returnBoard[goHere[0], goHere[1]] = "p2";
                                returnBoard[startingSpot[0] - 1, startingSpot[1] - 1] = null;
                                returnBoard[startingSpot[0], startingSpot[1]] = null;
                                moveMade = true;
                            }//end if (p2JumpLeft)
                        }//end else if (p2PlanJumpLeft)
                    }//end if (p2MiddleOfBoard)

                    else if (startingSpot[1] == 0)
                    {
                        if (goHere[0] == startingSpot[0] - 1 && goHere[1] == startingSpot[1] + 1)
                        {
                            returnBoard[goHere[0], goHere[1]] = "p2";
                            returnBoard[startingSpot[0], startingSpot[1]] = null;
                            moveMade = true;
                        }//end if (moveFrom0)
                        else if (goHere[0] == startingSpot[0] - 2 && goHere[1] == startingSpot[1] + 2)
                        {
                            if (returnBoard[startingSpot[0] - 1, startingSpot[1] + 1] == "p1" || returnBoard[startingSpot[0] - 1, startingSpot[1] + 1] == "k1")
                            {
                                returnBoard[goHere[0], goHere[1]] = "p2";
                                returnBoard[startingSpot[0] - 1, startingSpot[1] + 1] = null;
                                returnBoard[startingSpot[0], startingSpot[1]] = null;
                                moveMade = true;
                            }//end if (jumpFrom0)
                        }//end else if (planJump0)
                    }//end else if (on0Edge)

                    else if (startingSpot[1] == 7)
                    {
                        if (goHere[0] == startingSpot[0] - 1 && goHere[1] == startingSpot[1] - 1)
                        {
                            returnBoard[goHere[0], goHere[1]] = "p2";
                            returnBoard[startingSpot[0], startingSpot[1]] = null;
                            moveMade = true;
                        }//end if (moveFrom7)
                        else if (goHere[0] == startingSpot[0] - 2 && goHere[1] == startingSpot[1] - 2)
                        {
                            if (returnBoard[startingSpot[0] - 1, startingSpot[1] - 1] == "p1" || returnBoard[startingSpot[0] - 1, startingSpot[1] - 1] == "k1")
                            {
                                returnBoard[goHere[0], goHere[1]] = "p2";
                                returnBoard[startingSpot[0] - 1, startingSpot[0] - 1] = null;
                                returnBoard[startingSpot[0], startingSpot[1]] = null;
                                moveMade = true;
                            }//end if (jumpFrom7)
                        }//end else if (planJump7)
                    }//end else if (on7Edge)
                }//end if (notKing)
                #endregion

                #region King P2
                else if (returnBoard[startingSpot[0], startingSpot[1]] == "k2")
                {
                    if (startingSpot[0] == 7)
                    {
                        if (startingSpot[1] != 0 || startingSpot[1] != 7)
                        {
                            if (goHere[0] == startingSpot[0] - 1 && goHere[1] == startingSpot[1] - 1)
                            {
                                returnBoard[goHere[0], goHere[1]] = "k2";
                                returnBoard[startingSpot[0], startingSpot[1]] = null;
                                moveMade = true;
                            }//end if (king2MoveLeftFromBottom)
                            else if (goHere[0] == startingSpot[0] - 1 && goHere[1] == startingSpot[1] + 1)
                            {
                                returnBoard[goHere[0], goHere[1]] = "k2";
                                returnBoard[startingSpot[0], startingSpot[1]] = null;
                                moveMade = true;
                            }//end if (king2MoveRightFromBottom)
                            else if (goHere[0] == startingSpot[0] - 2 && goHere[1] == startingSpot[1] - 2)
                            {
                                if (returnBoard[startingSpot[0] - 1, startingSpot[1] - 1] == "p1" || returnBoard[startingSpot[0] - 1, startingSpot[1] - 1] == "k1")
                                {
                                    returnBoard[goHere[0], goHere[1]] = "k2";
                                    returnBoard[startingSpot[0] - 1, startingSpot[1] - 1] = null;
                                    returnBoard[startingSpot[0], startingSpot[1]] = null;
                                    moveMade = true;
                                }//end if (king2JumpLeftFromBottom)
                            }//end else if (planKing2JumpLeftFromBottom)
                            else if (goHere[0] == startingSpot[0] - 2 && goHere[1] == startingSpot[1] + 2)
                            {
                                if (returnBoard[startingSpot[0] - 1, startingSpot[1] + 1] == "p1" || returnBoard[startingSpot[0] - 1, startingSpot[1] + 1] == "k1")
                                {
                                    returnBoard[goHere[0], goHere[1]] = "k2";
                                    returnBoard[startingSpot[0] - 1, startingSpot[1] + 1] = null;
                                    returnBoard[startingSpot[0], startingSpot[1]] = null;
                                    moveMade = true;
                                }//end if (king2JumpRightFromBottom)
                            }//end else if (planKing2JumpRightFromBottom)
                        }//end if (notOnEdgeOfBoard)
                        else if (startingSpot[1] == 0)
                        {
                            if (goHere[0] == startingSpot[0] - 1 && goHere[1] == startingSpot[1] + 1)
                            {
                                returnBoard[goHere[0], goHere[1]] = "k2";
                                returnBoard[startingSpot[0], startingSpot[1]] = null;
                                moveMade = true;
                            }//end if (kingMoveFromBottom0Edge)
                            else if (goHere[0] == startingSpot[0] - 2 && goHere[1] == startingSpot[1] + 2)
                            {
                                if (returnBoard[startingSpot[0] - 1, startingSpot[1] + 1] == "p1" || returnBoard[startingSpot[0] - 1, startingSpot[1] + 1] == "k1")
                                {
                                    returnBoard[goHere[0], goHere[1]] = "k2";
                                    returnBoard[startingSpot[0] - 1, startingSpot[1] + 1] = null;
                                    returnBoard[startingSpot[0], startingSpot[1]] = null;
                                    moveMade = true;
                                }//end if (kingJumpRightFromBottom0Edge)
                            }//end else if (planKingJumpRightFromBottom0Edge)
                        }//end else if (movingKingFromBottom0Edge)
                        else if (startingSpot[1] == 7)
                        {
                            if (goHere[0] == startingSpot[0] - 1 && goHere[1] == startingSpot[1] - 1)
                            {
                                returnBoard[goHere[0], goHere[1]] = "k2";
                                returnBoard[startingSpot[0], startingSpot[1]] = null;
                                moveMade = true;
                            }//end if (kingMoveFromBottom7Edge)
                            else if (goHere[0] == startingSpot[0] - 2 && goHere[1] == startingSpot[1] - 2)
                            {
                                if (returnBoard[startingSpot[0] - 1, startingSpot[1] - 1] == "p1" || returnBoard[startingSpot[0] - 1, startingSpot[1] - 1] == "k1")
                                {
                                    returnBoard[goHere[0], goHere[1]] = "k2";
                                    returnBoard[startingSpot[0] - 1, startingSpot[1] - 1] = null;
                                    returnBoard[startingSpot[0], startingSpot[1]] = null;
                                    moveMade = true;
                                }//end if (kingJumpLeftFromBottom7Edge)
                            }//end else if (planJumpLeftFromBottom7Edge)
                        }//end if (movingKingFromBottom7Edge) 
                    }//end if (movingKing2FromBottom)
                    else if (startingSpot[0] == 0)
                    {
                        if (startingSpot[1] != 0 || startingSpot[1] != 7)
                        {
                            if (goHere[0] == startingSpot[0] + 1 && goHere[1] == startingSpot[1] - 1)
                            {
                                returnBoard[goHere[0], goHere[1]] = "k2";
                                returnBoard[startingSpot[0], startingSpot[1]] = null;
                                moveMade = true;
                            }//end if (king2MoveLeftFromTop)
                            else if (goHere[0] == startingSpot[0] + 1 && goHere[1] == startingSpot[1] + 1)
                            {
                                returnBoard[goHere[0], goHere[1]] = "k2";
                                returnBoard[startingSpot[0], startingSpot[1]] = null;
                                moveMade = true;
                            }//end if (king2MoveRightFromTop)
                            else if (goHere[0] == startingSpot[0] + 2 && goHere[1] == startingSpot[1] - 2)
                            {
                                if (returnBoard[startingSpot[0] + 1, startingSpot[1] - 1] == "p1" || returnBoard[startingSpot[0] + 1, startingSpot[1] - 1] == "k1")
                                {
                                    returnBoard[goHere[0], goHere[1]] = "k2";
                                    returnBoard[startingSpot[0] + 1, startingSpot[1] - 1] = null;
                                    returnBoard[startingSpot[0], startingSpot[1]] = null;
                                    moveMade = true;
                                }//end if (king2JumpLeftFromTop)
                            }//end else if (planKing2JumpLeftFromTop)
                            else if (goHere[0] == startingSpot[0] + 2 && goHere[1] == startingSpot[1] + 2)
                            {
                                if (returnBoard[startingSpot[0] + 1, startingSpot[1] + 1] == "p1" || returnBoard[startingSpot[0] + 1, startingSpot[1] + 1] == "k1")
                                {
                                    returnBoard[goHere[0], goHere[1]] = "k2";
                                    returnBoard[startingSpot[0] + 1, startingSpot[1] + 1] = null;
                                    returnBoard[startingSpot[0], startingSpot[1]] = null;
                                    moveMade = true;
                                }//end if (king2JumpRightFromTop)
                            }//end else if (planKing2JumpRightFromTop)
                        }//end if (notOnEdgeOfBoard)
                        else if (startingSpot[1] == 0)
                        {
                            if (goHere[0] == startingSpot[0] + 1 && goHere[1] == startingSpot[1] + 1)
                            {
                                returnBoard[goHere[0], goHere[1]] = "k2";
                                returnBoard[startingSpot[0], startingSpot[1]] = null;
                                moveMade = true;
                            }//end if (kingMoveFromTop0Edge)
                            else if (goHere[0] == startingSpot[0] + 2 && goHere[1] == startingSpot[1] + 2)
                            {
                                if (returnBoard[startingSpot[0] + 1, startingSpot[1] + 1] == "p1" || returnBoard[startingSpot[0] + 1, startingSpot[1] + 1] == "k1")
                                {
                                    returnBoard[goHere[0], goHere[1]] = "k2";
                                    returnBoard[startingSpot[0] - 1, startingSpot[1] + 1] = null;
                                    returnBoard[startingSpot[0], startingSpot[1]] = null;
                                    moveMade = true;
                                }//end if (kingJumpFromTop0Edge)
                            }//end if (planKingJumpFromTop0Edge)
                        }//end if (kingMoveFromTop)
                        else if (startingSpot[1] == 7)
                        {
                            if (goHere[0] == startingSpot[0] + 1 && goHere[1] == startingSpot[1] - 1)
                            {
                                returnBoard[goHere[0], goHere[1]] = "k2";
                                returnBoard[startingSpot[0], startingSpot[1]] = null;
                                moveMade = true;
                            }//end if (kingMoveFromTop7Edge)
                            else if (goHere[0] == startingSpot[0] + 2 && goHere[1] == startingSpot[1] - 2)
                            {
                                if (returnBoard[startingSpot[0] + 1, startingSpot[1] - 1] == "p1" || returnBoard[startingSpot[0] + 1, startingSpot[1] - 1] == "k1")
                                {
                                    returnBoard[goHere[0], goHere[1]] = "k2";
                                    returnBoard[startingSpot[0] + 1, startingSpot[1] - 1] = null;
                                    returnBoard[startingSpot[0], startingSpot[1]] = null;
                                    moveMade = true;
                                }//end if (kingJumpLeftFromTop7Edge)
                            }//end else if (planJumpLeftFromTop7Edge)
                        }//end else if (movingKingFrom7Edge)
                    }//end else if (movingKing1FromTop)
                    else
                    {
                        if (startingSpot[1] != 0 || startingSpot[1] != 7)
                        {
                            if (goHere[0] == startingSpot[0] - 1 && goHere[1] == startingSpot[1] - 1)
                            {
                                returnBoard[goHere[0], goHere[1]] = "k2";
                                returnBoard[startingSpot[0], startingSpot[1]] = null;
                                moveMade = true;
                            }//end if (king2MoveUpLeft)
                            else if (goHere[0] == startingSpot[0] - 1 && goHere[1] == startingSpot[1] + 1)
                            {
                                returnBoard[goHere[0], goHere[1]] = "k2";
                                returnBoard[startingSpot[0], startingSpot[1]] = null;
                                moveMade = true;
                            }//end if (king2MoveUpRight)
                            else if (goHere[0] == startingSpot[0] + 1 && goHere[1] == startingSpot[1] - 1)
                            {
                                returnBoard[goHere[0], goHere[1]] = "k2";
                                returnBoard[startingSpot[0], startingSpot[1]] = null;
                                moveMade = true;
                            }//end if (king2MoveDownLeft)
                            else if (goHere[0] == startingSpot[0] + 1 && goHere[1] == startingSpot[1] + 1)
                            {
                                returnBoard[goHere[0], goHere[1]] = "k2";
                                returnBoard[startingSpot[0], startingSpot[1]] = null;
                                moveMade = true;
                            }//end if (king2MoveDownRight)
                            else if (goHere[0] == startingSpot[0] - 2 && goHere[1] == startingSpot[1] - 2)
                            {
                                if (returnBoard[startingSpot[0] - 1, startingSpot[1] - 1] == "p1" || returnBoard[startingSpot[0] - 1, startingSpot[1] - 1] == "k1")
                                {
                                    returnBoard[goHere[0], goHere[1]] = "k2";
                                    returnBoard[startingSpot[0] - 1, startingSpot[1] - 1] = null;
                                    returnBoard[startingSpot[0], startingSpot[1]] = null;
                                    moveMade = true;
                                }//end if (king2JumpUpLeft)
                            }//end else if (planKing2JumpUpLeft)
                            else if (goHere[0] == startingSpot[0] - 2 && goHere[1] == startingSpot[1] + 2)
                            {
                                if (returnBoard[startingSpot[0] - 1, startingSpot[1] + 1] == "p1" || returnBoard[startingSpot[0] - 1, startingSpot[1] + 1] == "k1")
                                {
                                    returnBoard[goHere[0], goHere[1]] = "k2";
                                    returnBoard[startingSpot[0] - 1, startingSpot[1] + 1] = null;
                                    returnBoard[startingSpot[0], startingSpot[1]] = null;
                                    moveMade = true;
                                }//end if (king2JumpUpRight)
                            }//end else if (planKing2JumpUpRight)
                            else if (goHere[0] == startingSpot[0] + 2 && goHere[1] == startingSpot[1] - 2)
                            {
                                if (returnBoard[startingSpot[0] + 1, startingSpot[1] - 1] == "p1" || returnBoard[startingSpot[0] + 1, startingSpot[1] - 1] == "k1")
                                {
                                    returnBoard[goHere[0], goHere[1]] = "k2";
                                    returnBoard[startingSpot[0] + 1, startingSpot[1] - 1] = null;
                                    returnBoard[startingSpot[0], startingSpot[1]] = null;
                                    moveMade = true;
                                }//end if (king2JumpDownLeft)
                            }//end else if (planKing2JumpDownLeft)
                            else if (goHere[0] == startingSpot[0] + 2 && goHere[1] == startingSpot[1] + 2)
                            {
                                if (returnBoard[startingSpot[0] + 1, startingSpot[1] + 1] == "p1" || returnBoard[startingSpot[0] + 1, startingSpot[1] + 1] == "k1")
                                {
                                    returnBoard[goHere[0], goHere[1]] = "k2";
                                    returnBoard[startingSpot[0] + 1, startingSpot[1] + 1] = null;
                                    returnBoard[startingSpot[0], startingSpot[1]] = null;
                                    moveMade = true;
                                }//end if (king2JumpDownRight)
                            }//end else if (planKing2JumpDownRight)
                        }//end if (notOnEdgeOfBoard)
                        else if (startingSpot[1] == 0)
                        {
                            if (goHere[0] == startingSpot[0] - 1 && goHere[1] == startingSpot[1] + 1)
                            {
                                returnBoard[goHere[0], goHere[1]] = "k2";
                                returnBoard[startingSpot[0], startingSpot[1]] = null;
                                moveMade = true;
                            }//end if (king2MoveUpFrom0)
                            else if (goHere[0] == startingSpot[0] + 1 && goHere[1] == startingSpot[1] + 1)
                            {
                                returnBoard[goHere[0], goHere[1]] = "k2";
                                returnBoard[startingSpot[0], startingSpot[1]] = null;
                                moveMade = true;
                            }//end else if (king2MoveDownFrom0)
                            else if (goHere[0] == startingSpot[0] - 2 && goHere[1] == startingSpot[1] + 2)
                            {
                                if (returnBoard[startingSpot[0] - 1, startingSpot[1] + 1] == "p1" || returnBoard[startingSpot[0] - 1, startingSpot[1] + 1] == "k1")
                                {
                                    returnBoard[goHere[0], goHere[1]] = "k2";
                                    returnBoard[startingSpot[0] - 1, startingSpot[1] + 1] = null;
                                    returnBoard[startingSpot[0], startingSpot[1]] = null;
                                    moveMade = true;
                                }//end if (king2JumpUpFrom0)
                            }//end else if (planKing2JumpUpFrom0)
                            else if (goHere[0] == startingSpot[0] + 2 && goHere[1] == startingSpot[1] + 2)
                            {
                                if (returnBoard[startingSpot[0] + 1, startingSpot[1] + 1] == "p1" || returnBoard[startingSpot[0] + 1, startingSpot[1] + 1] == "k1")
                                {
                                    returnBoard[goHere[0], goHere[1]] = "k2";
                                    returnBoard[startingSpot[0] + 1, startingSpot[1] + 1] = null;
                                    returnBoard[startingSpot[0], startingSpot[1]] = null;
                                    moveMade = true;
                                }//end if (king2JumpDownFrom0)
                            }//end else if (planKing2JumpDownFrom0)
                        }//end else if (movingKing2From0EdgeNotTopOrBottom)
                        else if (startingSpot[1] == 7)
                        {
                            if (goHere[0] == startingSpot[0] - 1 && goHere[1] == startingSpot[1] - 1)
                            {
                                returnBoard[goHere[0], goHere[1]] = "k2";
                                returnBoard[startingSpot[0], startingSpot[1]] = null;
                                moveMade = true;
                            }//end if (king2MoveUpFrom7)
                            else if (goHere[0] == startingSpot[0] + 1 && goHere[1] == startingSpot[1] - 1)
                            {
                                returnBoard[goHere[0], goHere[1]] = "k2";
                                returnBoard[startingSpot[0], startingSpot[1]] = null;
                                moveMade = true;
                            }//end else if (king2MoveDownFrom7)
                            else if (goHere[0] == startingSpot[0] - 2 && goHere[1] == startingSpot[1] - 2)
                            {
                                if (returnBoard[startingSpot[0] - 1, startingSpot[1] - 1] == "p1" || returnBoard[startingSpot[0] - 1, startingSpot[1] - 1] == "k1")
                                {
                                    returnBoard[goHere[0], goHere[1]] = "k2";
                                    returnBoard[startingSpot[0] - 1, startingSpot[1] - 1] = null;
                                    returnBoard[startingSpot[0], startingSpot[1]] = null;
                                    moveMade = true;
                                }//end if (king2JumpUpFrom7)
                            }//end else if (planKing2JumpUpFrom7)
                            else if (goHere[0] == startingSpot[0] + 2 && goHere[1] == startingSpot[1] - 2)
                            {
                                if (returnBoard[startingSpot[0] + 1, startingSpot[1] - 1] == "p1" || returnBoard[startingSpot[0] + 1, startingSpot[1] - 1] == "k1")
                                {
                                    returnBoard[goHere[0], goHere[1]] = "k2";
                                    returnBoard[startingSpot[0] + 1, startingSpot[1] - 1] = null;
                                    returnBoard[startingSpot[0], startingSpot[1]] = null;
                                    moveMade = true;
                                }//end if (king2JumpDownFrom7)
                            }//end else if (planKing2JumpDownFrom7)
                        }//end else if (movingKing2From7EdgeNotTopOrBottom)            
                    }//end else (notOnTopOrBottom)
                }//end else if (movingP2King)
                #endregion

                if (moveMade)
                {
                    if (goHere[0] == 0)//top of board
                    {
                        returnBoard[goHere[0], goHere[1]] = "k2";
                    }//end if (kingQuarters)
                }//end if (moveMade)

            }//end if ("p2" turn)
            #endregion

            return returnBoard;
        }//end function
        #endregion

        #region playerTally
        static int playerTally(string[,] gameBoard)//this method checks the gameboard for playable game pieces
        {
            int player1Check = 0;//used to tally player 1's gamepieces
            int player2Check = 0;//used to tally player 2's gamepieces
            /*
                below, the 'for' loops will check the gameBoard
                if the element in the gameBoard equals string "p1" or string "k1", the player1Check is incremented by 1 (meaning the loop has found a player 1 piece)
                if the element in the gameBoard equals string "p2" or string "k2", the player2Check is incremented by 1 (meaning the loop has found a player 2 piece)
            */

           for (int i = 0; i < gameBoard.GetLength(0); i ++)//int i : runs through the x axis
           {
               for (int j = 0; j < gameBoard.GetLength(1); j ++)//int j : runs through the y axis
               {
                    if (gameBoard[i,j] == "p1" || gameBoard[i,j] == "k1")
                    {
                        player1Check++;//loop has found a player 1 piece
                    }//end if (gameBoard == "p1" || "k1")
                    else if (gameBoard[i, j] == "p2" || gameBoard[i, j] == "k2")
                    {
                        player2Check++;//loop has found a player 2 piece
                    }//end else if (gameBoard == "p2" || "k2")
               }//end for (int j)
           }//end for (int i)

           /*
             if player1Check == 0 after searching the gameBoard, no player 1 game pieces are present on the gameBoard, Player 2 wins so the method will return a 2
             as above - so below, but if no player 2 pieces are there the method will return a 1 for player 1 as the winner
             if both players still have gamepieces the method will return a zero and the game continues
            */

           if (player1Check == 0)//no player 1 pieces
           {
               return 2;//player two wins
           }//end if  (no player 1 pieces)
           else if (player2Check == 0)//no player 2 pieces
           {
               return 1;//player 1 wins
           }//end else if
           else//no winner
           {
               return 0;//no winner both players have playable pieces
           }//end else (no winner)
        }//end method (playerTally) 
        #endregion

    }//End Class
}//End Name

using System;
// you can also use other imports, for example:
// using System.Collections.Generic;

// you can write to stdout for debugging purposes, e.g.
// Console.WriteLine("this is a debug message");

class Solution
{
    public static void Main(string[] args)
    {
        solution(4, "1A 1A,26Z 26Z", "1A 26Z");
    }
    public static int ConvertCharToBoardCol(char c)
    {
        return c - 'A' + 1;
    }

    public static Tuple<int, int> GetBoardCoordsFromString(string coords)
    {
        int row = int.Parse(coords.Substring(0, coords.Length - 1));
        int col = ConvertCharToBoardCol(coords[coords.Length - 1]);
        return new Tuple<int, int>(row, col);
    }

    public static string solution(int N, string S, string T)
    {
        // write your code in C# 6.0 with .NET 4.5 (Mono)

        if(S.Length == 0 || T.Length == 0)
        {
            //No ships or no shots so return 0,0
            return "0,0";
        }
        

        //Split ships on , to get each ship group of coordinates. Then split this on ' ' to get top left to bottom right
        string[] ships = S.Split(',');
        string[] shots = T.Split(' ');

        //Create an array of ships, simply Ship0 to ShipT.length
        int[] shipsRemainingParts = new int[ships.Length + 1];
        //Create another array of ships, same indexing simply to hold the initial size of the ships
        int[] shipsTotalParts = new int[ships.Length + 1];
        //Create matrix to represent game board. 1-> 26 by 1->26
        int[][] gameBoard = new int[N+1][];
        for(int i = 0; i < N+1; i++)
        {
            gameBoard[i] = new int[N+1];
        }
        // Note : When mapping shots and ships to the board, take letter - 'A' + 1 to get index

        //Loop through ships, adding each one to the array of ships (value increments with number of boxes in ship, and then putting the ShipIndex in the positions on the board. 
        for(int shipIndex = 0; shipIndex < ships.Length; shipIndex++)
        {
            string[] coords = ships[shipIndex].Split(' ');
            int totalParts = 0;
            string topLeft = coords[0];
            string bottomRight = coords[1];
            var topLeftCoords = GetBoardCoordsFromString(topLeft);
            int topLeftRow = topLeftCoords.Item1;
            int topLeftCol = topLeftCoords.Item2;
            var bottomRightCoords = GetBoardCoordsFromString(bottomRight);
            int bottomRightRow = bottomRightCoords.Item1;
            int bottomRightCol = bottomRightCoords.Item2;
            int startRow = topLeftRow;
            while(startRow <= bottomRightRow)
            {
                int startCol = topLeftCol;
                while(startCol <= bottomRightCol)
                {
                    gameBoard[startRow][startCol] = shipIndex + 1;
                    totalParts++;
                    startCol++;
                }
                startRow++;
            }
            shipsRemainingParts[shipIndex + 1] = totalParts;
            shipsTotalParts[shipIndex + 1] = totalParts;

        }

        //Loop through shots T, and if board has an index in then go to the ship array index, and -- the number of boxes remaining of the ship
        foreach(var shot in shots)
        {
            var shotCoords = GetBoardCoordsFromString(shot);
            int shotRow = shotCoords.Item1;
            int shotCol = shotCoords.Item2;

            int gameBoardElement = gameBoard[shotRow][shotCol];
            if(gameBoardElement != 0)
            {
                //Then we have hit a ship
                shipsRemainingParts[gameBoardElement]--;
            }
        }

        //Finally, loop through ship array. Any elements that are 0 are ships destroyed. Any other elements are ships that have been hit if their value != initialSizeArray[index]
        int shipsDestroyed = 0;
        int shipsHit = 0;
        for(int i = 1; i < shipsRemainingParts.Length; i++)
        {
            int shipParts = shipsRemainingParts[i];
            int shipTotal = shipsTotalParts[i];
            if(shipParts == 0)
            {
                shipsDestroyed++;
            }
            else if(shipTotal > shipParts)
            {
                shipsHit++;
            }
        }
        return $"{shipsDestroyed},{shipsHit}";
    }
}
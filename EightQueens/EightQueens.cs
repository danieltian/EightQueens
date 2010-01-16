using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace EightQueens
{
    class EightQueens
    {
        private int boardSize;

        public int BoardSize
        {
            get { return boardSize; }
            set
            {
                boardSize = value;
                queenLocations = new bool[value, value];
                takenLocations = new bool[value, value];
            }
        }

        public TimeSpan TotalTime
        {
            get
            {
                return timeStop - timeStart;
            }
        }

        private bool [,] queenLocations;
        private bool [,] takenLocations;
        private List<bool[,]> uniqueSolutions;
        private List<bool[,]> distinctSolutions;
        public long cycles;
        public DateTime timeStart;
        public DateTime timeStop;

        public EightQueens()
        {
            BoardSize = 8;
            uniqueSolutions = new List<bool[,]>();
            distinctSolutions = new List<bool[,]>();
            cycles = 0;
        }

        // ****************************************************************************************
        public void Reset()
        {
            // set all array values to false
            for (int i = 0; i < boardSize; i++)
            {
                for (int j = 0; j < boardSize; j++)
                {
                    queenLocations[i, j] = false;
                    takenLocations[i, j] = false;
                }
            }
            // clear the solutions lists
            uniqueSolutions.Clear();
            distinctSolutions.Clear();
            cycles = 0;
        }

        // ****************************************************************************************
        public void CalculateSolutions()
        {
            // start the actual recursion
            timeStart = DateTime.Now;
            CalculateSolutions(queenLocations, takenLocations, 0);
            timeStop = DateTime.Now;
        }

        // ****************************************************************************************
        public void CalculateSolutions(bool[,] queenLocations, bool[,] takenLocations, int row)
        {
            cycles++;

            // if we're in the last row
            if (row == boardSize - 1)
            {
                // for every spot in the last row
                for (int i = 0; i < boardSize; i++)
                {
                    // if the spot is available
                    if (takenLocations[row, i] == false)
                    {
                        // Add a queen there, add it to the solutions, then return. There will
                        // always only be one spot where a queen can go in for the last row
                        // in a valid solution.
                        queenLocations[row, i] = true;
                        AddSolution(queenLocations);
                        return;
                    }
                }
            }
            else
            {
                // go through each spot in the current row
                for(int i = 0; i < boardSize; i++)
                {
                    if (takenLocations[row, i] == false)
                    {
                        // both arrays are cloned each time because we need to remember what the old boards looked like
                        // in order to go back to them and try the next available spot
                        bool[,] newQueenLoc = (bool[,])queenLocations.Clone();
                        newQueenLoc[row, i] = true;
                        bool[,] newTakenLoc = (bool[,])takenLocations.Clone();
                        FillTakenLocations(newTakenLoc, i, row);

                        CalculateSolutions(newQueenLoc, newTakenLoc, row + 1);
                    }
                }
            }
        }

        // ****************************************************************************************
        private void AddSolution(bool[,] queenLocations)
        {
            bool foundSame = false;

            // search solutions list for a duplicate solution (including rotations and reflections)
            foreach (bool[,] solution in uniqueSolutions)
            {
                if (CheckArrayEquality(solution, queenLocations))
                {
                    foundSame = true;
                    break;
                }
            }
            // if no duplicates are found, add it to the list
            if (foundSame == false)
            {
                uniqueSolutions.Add(queenLocations);
            }

            // get number of distinct solutions
            foundSame = false;
            foreach (bool[,] solution in distinctSolutions)
            {
                if (EqualsOriginal(solution, queenLocations))
                {
                    foundSame = true;
                    break;
                }
            }
            // if no distinct solutions are found, add it to the list
            if (foundSame == false)
            {
                distinctSolutions.Add(queenLocations);
            }
        }

        // ****************************************************************************************
        public bool CheckArrayEquality(bool[,] array1, bool[,] array2)
        {
            // Because checking equality is an expensive operation, we factor out each check into
            // a method. If an equality is found, return true and skip the rest of the equality checks.

            // check equality for original and its reflection
            if (EqualsOriginal(array1, array2))
            {
                return true;
            }
            if (EqualsReflectOriginal(array1, array2))
            {
                return true;
            }
            
            // check equality for array rotated 90 degrees clockwise and its reflection
            if (Equals90(array1, array2))
            {
                return true;
            }
            if (EqualsReflect90(array1, array2))
            {
                return true;
            }
            
            // check equality for array rotated 180 degrees clockwise and its reflection
            if (Equals180(array1, array2))
            {
                return true;
            }
            if (EqualsReflect180(array1, array2))
            {
                return true;
            }

            // check equality for array rotated 270 degrees clockwise and its reflection
            if (Equals270(array1, array2))
            {
                return true;
            }
            if (EqualsReflect270(array1, array2))
            {
                return true;
            }

            // return false if nothing was true
            return false;
        }

        // ****************************************************************************************
        private bool EqualsOriginal(bool[,] array1, bool[,] array2)
        {
            for (int i = 0; i < boardSize; i++)
            {
                for (int j = 0; j < boardSize; j++)
                {
                    if (array1[i, j] != array2[i, j])
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        // ****************************************************************************************
        private bool EqualsReflectOriginal(bool[,] array1, bool[,] array2)
        {
            for (int i = 0; i < boardSize; i++)
            {
                for (int j = 0; j < boardSize; j++)
                {
                    if (array1[i, (boardSize - 1) - j] != array2[i, j])
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        // ****************************************************************************************
        private bool Equals90(bool[,] array1, bool[,] array2)
        {
            for (int i = 0; i < boardSize; i++)
            {
                for (int j = 0; j < boardSize; j++)
                {
                    if (array1[j, (boardSize - 1) - i] != array2[i, j])
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        // ****************************************************************************************
        private bool EqualsReflect90(bool[,] array1, bool[,] array2)
        {
            for (int i = 0; i < boardSize; i++)
            {
                for (int j = 0; j < boardSize; j++)
                {
                    if (array1[j, i] != array2[i, j])
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        // ****************************************************************************************
        private bool Equals180(bool[,] array1, bool[,] array2)
        {
            for (int i = 0; i < boardSize; i++)
            {
                for (int j = 0; j < boardSize; j++)
                {
                    if (array1[(boardSize - 1) - i, (boardSize - 1) - j] != array2[i, j])
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        // ****************************************************************************************
        private bool EqualsReflect180(bool[,] array1, bool[,] array2)
        {
            for (int i = 0; i < boardSize; i++)
            {
                for (int j = 0; j < boardSize; j++)
                {
                    if (array1[(boardSize - 1) - i, j] != array2[i, j])
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        // ****************************************************************************************
        private bool Equals270(bool[,] array1, bool[,] array2)
        {
            for (int i = 0; i < boardSize; i++)
            {
                for (int j = 0; j < boardSize; j++)
                {
                    if (array1[(boardSize - 1) - j, i] != array2[i, j])
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        // ****************************************************************************************
        private bool EqualsReflect270(bool[,] array1, bool[,] array2)
        {
            for (int i = 0; i < boardSize; i++)
            {
                for (int j = 0; j < boardSize; j++)
                {
                    if (array1[(boardSize - 1) - j, (boardSize - 1) - i] != array2[i, j])
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        // ****************************************************************************************
        public void FillTakenLocations(bool[,] takenLocations, int x, int y)
        {
            // fill the space the queen itself occupies
            takenLocations[y, x] = true;

            // fill horizontal row
            for (int i = 0; i < boardSize; i++)
            {
                takenLocations[y, i] = true;
            }
            // fill vertical row
            for (int i = 0; i < boardSize; i++)
            {
                takenLocations[i, x] = true;
            }
            // fill diagonal down right
            for (int i = 1; (x + i < boardSize) && (y + i < boardSize); i++)
            {
                takenLocations[y + i, x + i] = true;
            }
            // fill diagonal down left
            for (int i = 1; (x - i >= 0) && (y + i < boardSize); i++)
            {
                takenLocations[y + i, x - i] = true;
            }
            // fill diagonal up right
            for (int i = 1; (x + i < boardSize) && (y - i >= 0); i++)
            {
                takenLocations[y - i, x + i] = true;
            }
            // fill diagonal up left
            for (int i = 1; (x - i >= 0) && (y - i >= 0); i++)
            {
                takenLocations[y - i, x - i] = true;
            }
        }

        // ****************************************************************************************
        private string GetSolutions()
        {
            int i = 0;
            int solutionCount = 0;
            StringBuilder output = new StringBuilder();

            output.AppendLine("Total execution time: " + TotalTime.Hours + " hours, " + TotalTime.Minutes + " minutes, " + TotalTime.Seconds + " seconds, " + TotalTime.Milliseconds + " milliseconds");
            output.AppendLine("Number of cycles: " + cycles);
            output.AppendLine("Number of unique solutions: " + uniqueSolutions.Count);
            output.AppendLine("Number of distinct solutions: " + distinctSolutions.Count);
            output.AppendLine();

            foreach (bool[,] solution in uniqueSolutions)
            {
                output.AppendLine("Solution " + (++solutionCount) + ":");

                foreach (bool space in solution)
                {
                    // if space is taken, display Q
                    if (space == true)
                    {
                        output.Append("Q");
                    }
                    // otherwise, display a dash
                    else
                    {
                        output.Append("-");
                    }

                    i++;
                    output.Append(" ");
                    // if the number of items displayed exceeds the board size,
                    // insert a line break to show the next row (index starts at 0)
                    if (i == boardSize)
                    {
                        i = 0;
                        output.AppendLine();
                    }
                }

                output.AppendLine();
            }

            return output.ToString();
        }

        // ****************************************************************************************
        public void WriteSolutionsToFile()
        {
            StreamWriter file = new StreamWriter(System.Environment.GetEnvironmentVariable("USERPROFILE") + "\\Desktop\\log.txt");
            file.Write(GetSolutions());
            file.Close();
        }

        // ****************************************************************************************
        public void WriteSolutionsToScreen()
        {
            Console.Write(GetSolutions());
        }

        // ****************************************************************************************
        public void ShowBoard(bool[,] board)
        {
            int i = 0;
            // displays a specific board, for debugging use
            foreach (bool space in board)
            {
                // if space is taken, display Q
                if (space == true)
                {
                    Console.Write("Q");
                }
                // otherwise, display a dash
                else
                {
                    Console.Write("-");
                }

                i++;
                Console.Write(" ");
                // if the number of items displayed exceeds the board size,
                // insert a line break to show the next row (index starts at 0)
                if (i == boardSize)
                {
                    i = 0;
                    Console.WriteLine();
                }
            }

            Console.WriteLine();
            Console.ReadLine();
        }
    }
}
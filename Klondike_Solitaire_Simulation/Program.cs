using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Klondike_Solitaire_Simulation.Heuristics;

namespace Klondike_Solitaire_Simulation
{
    public class Program
    {
        private static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Console.WriteLine("Welcome to the Klondike Solitaire Simulator!");
            Console.Title = "Klondike Solitaire Simulator";

			// Some locks for multithreading
            Object stateLock = new Object();
            Object writeLock = new Object();
            Object iterationLock = new Object();
            Object h1WincounterLock = new Object();
            Object winPercentageLock = new Object();

            List<Heuristic> heuristics = new List<Heuristic>
            {
                //new ChanceHeuristic(),
                new HighestRankHeuristic(),
                //new PlaceHoldersHeuristic(),
                //new PlaceHolderHeuristic2(),
                new RandomHeuristic(),
                new TableauHeuristic(),
                new WindowsHeuristic(),
                new WouterHeuristic()
            };

			// Keep track of scores
            int gamesNotLost = 0;
            int h1Wincounter = 0;
            int iteration = 1;
            int iterations = 1000;
            List<float> winPercentage = new List<float>(new float[heuristics.Count]);
            List<long> averageMoves = new List<long>(new long[heuristics.Count]);
            List<long> averageWinMoves = new List<long>(new long[heuristics.Count]);

            for (int h = 0; h < heuristics.Count; h++) { 
                averageMoves[h] = 0; averageWinMoves[h] = 0;
            }

            Parallel.For(0, iterations, index =>
            {
                State startState;
                lock (stateLock)
                {
                    startState = new State();
                }

                List<bool> wonGames = new List<bool>();
                string output = "";

                //Parallel.For(0, heuristics.Count, heuristicIndex =>
                for (int heuristicIndex = 0; heuristicIndex < heuristics.Count; ++heuristicIndex)
                {
                    Heuristic heuristic = heuristics[heuristicIndex];

                    State state = new State(startState);

                    // Make moves until you've reached an end state
                    List<State> moves;
                    while (!state.IsWinState && (moves = state.GetMoves()).Count > 0)
                    {
                        state = heuristic.GetMove(state, moves);
                    }

                    output += "[" + heuristic.GetType().Name + "] Iteration " + iteration + "/" + iterations + " (" + index + "): ";

                    wonGames.Add(state.IsWinState);

                    // Check the type of state once done
                    if (state.IsWinState)
                    {
                        // If we won, add that
                        lock (h1WincounterLock)
                        {
                            ++h1Wincounter;
                        }

                        output += "Win";
                        averageWinMoves[heuristicIndex] += state.MovesMade;
                    }
                    else
                    {
                        // Otherwise, show we lost
                        output += "Loss";
                    }

                    output += " after " + state.MovesMade + " moves\n";
                    averageMoves[heuristicIndex] += state.MovesMade;

                    //lock (writeLock)
                    //{
                    // Console.WriteLine(output);
                    //}

                    //StreamWriter writer = new StreamWriter();
                    //string stateText = state.ToString(true, false);
                    //writer.WriteLine(stateText);
                    //writer.Flush();
                    //writer.Close();

                    //System.IO.File.WriteAllLines(@"C:\Users\Wouter\Desktop\Output\" + index + ".txt", state.ToString(true, false).Split('\n'));
                    //});

                    //lock (winPercentageLock)
                    //{
                    //    winPercentage[heuristicIndex] = (float)h1Wincounter / iterations * 100.0f;
                    //}
                }//);

                lock (iterationLock)
                {
                    ++iteration;
                }

                bool allLost = true;
                for (int i = 0; i < wonGames.Count; i++)
                {
                    if (wonGames[i])
                    {
                        lock (winPercentageLock)
                        {
                            winPercentage[i]++;
                        }
                        allLost = false;
                    }
                }

                if (allLost)
                    output = "All games lost\n";
                else
                    gamesNotLost++; 

                Console.Write(output);
            });

            for (int i = 0; i < heuristics.Count; ++i)
            {
                Console.WriteLine("[" + heuristics[i].GetType().Name + "] Win percentage: " + (float)(winPercentage[i]/gamesNotLost)*100 + " % ");
                Console.WriteLine("  Average moves: " + averageMoves[i]/iterations + ".  Average moves when won: " + averageWinMoves[i]/gamesNotLost + ".");
            }
            Console.WriteLine(gamesNotLost + " out of " + iterations + " games won");

            Console.ReadLine();
        }
    }
}
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

			RandomNumberGenerator r = new RandomNumberGenerator();

			// Initialize the game state
			//State startState = new State();

			//Console.WriteLine("Start state:");
			//Console.WriteLine(startState.ToString(false));

			//Console.WriteLine("Possible end states:");

			int h1Wincounter = 0;
			int iteration = 1;
			int iterations = 1000;

			Object writeLock = new Object();

			Parallel.For(0, iterations, index =>
			{
				State state = new State(); //startState;

				// Make moves until you've reached an end state
				List<State> moves;
				while (!state.IsWinState && (moves = state.GetMoves()).Count > 0)
				{
                    state = new RandomHeuristic().GetMove(state, moves);//new WoutersHeuristic().GetMove(state, moves); //new WindowsHeuristic().GetMove(state, moves);
				}

				lock (writeLock)
				{
					Console.Write("Iteration " + iteration + "/" + iterations + ": ");
					++iteration;

					// Check the type of state once done
					if (state.IsWinState)
					{
						// If we won, add that
						++h1Wincounter;

						Console.Write("Win");
					}
					else
					{
						// Otherwise, show we lost
						Console.Write("Loss");
					}

					Console.WriteLine(" after " + state.MovesMade + " moves");
				}
			});

			//Parallel.ForEach(Partitioner.Create(0, iterations), range =>
			//{
			//	for (int index = range.Item1; index < range.Item2; ++index)
			//	{
			//		State state = new State(); //startState;

			//		// Make moves until you've reached an end state
			//		List<State> moves;
			//		while (!state.IsWinState && (moves = state.GetMoves()).Count > 0)
			//		{
			//			state = new WindowsHeuristic().GetMove(state, moves);
			//		}

			//		lock (writeLock)
			//		{
			//			Console.Write("Iteration " + iteration + "/" + iterations + ": ");
			//			++iteration;

			//			// Check the type of state once done
			//			if (state.IsWinState)
			//			{
			//				// If we won, add that
			//				++h1Wincounter;

			//				Console.Write("Win");
			//			}
			//			else
			//			{
			//				// Otherwise, show we lost
			//				Console.Write("Loss");
			//			}

			//			Console.WriteLine(" after " + state.MovesMade + " moves");
			//		}
			//	}
			//	/*
			//	string stateText = state.ToString(true, 10);
			//	Console.WriteLine(stateText);
			//	StreamWriter writer = new StreamWriter(@"C:\Users\qub1\Desktop\Output.txt");
			//	writer.Write(stateText);
			//	writer.Close();
			//             */
			//});

			//Parallel.ForEach(Partitioner.Create(0, iterations), range =>
			//{
			//	for (int index = range.Item1; index < range.Item2; ++index)
			//	{
			//		State state = new State(); //startState;
			//		Console.WriteLine(index);

			//		// Make moves until you've reached an end state
			//		List<State> moves;
			//		while ((moves = state.GetMoves()).Count > 0)
			//		{
			//			if (!state.IsWinState)
			//				state = new WindowsHeuristic().GetMove(state, moves);
			//			else if (state.IsWinState)
			//			{
			//				Interlocked.Increment(ref h1Wincounter);
			//				Console.WriteLine("Win");
			//				break;
			//			}

			//			//state = moves[r.Next(moves.Count)];
			//		}
			//	}
			//	/*
			//	string stateText = state.ToString(true, 10);
			//	Console.WriteLine(stateText);
			//	StreamWriter writer = new StreamWriter(@"C:\Users\qub1\Desktop\Output.txt");
			//	writer.Write(stateText);
			//	writer.Close();
			//             */
			//});

			Console.WriteLine((float)h1Wincounter / iterations * 100.0f + "%");
			Console.ReadLine();
		}
	}
}
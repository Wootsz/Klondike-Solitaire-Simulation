using System.Collections.Generic;

namespace Klondike_Solitaire_Simulation.Heuristics
{
	class RandomHeuristic : BaseHeuristic
	{
		public override State GetMove(State currentState, List<State> moves) => moves[Utility.Random.Next(moves.Count)];
	}
}
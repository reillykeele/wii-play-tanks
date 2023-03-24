using System.Collections.Generic;

namespace Util.AI.BehaviourTree
{
    /// <summary>
    /// A control flow node representing a Parallel Sequence.
    /// </summary>
    public class ParallelNode : Node
    {
        public List<Node> Nodes;

        public ParallelNode(List<Node> nodes)
        {
            Nodes = nodes;
        }

        /// <summary>
        /// Ticks all child nodes of the parallel sequence in-order.
        /// </summary>
        /// <returns>
        /// The <c>NodeState</c> of the parallel sequence after evaluation.
        /// <list type="bullet">
        /// <item>
        ///     <term><c>NodeState.Success</c></term>
        ///     <description>At least one of the child nodes was successful.</description>
        /// </item>
        /// <item>
        ///     <term><c>NodeState.Running</c></term>
        ///     <description>
        ///     None of the children were successful, and at least one of the child nodes is running.
        ///     </description>
        /// </item>
        /// <item>
        ///     <term><c>NodeState.Failure</c></term>
        ///     <description>All of the child nodes failed.</description>
        /// </item>
        /// </list>
        /// </returns>
        public override NodeState Tick()
        {
            var anySuccessful = false;
            var anyRunning = false;
            foreach (var node in Nodes)
            {
                switch (node.Tick())
                {
                    case NodeState.Success:
                        // A child is successful,evaluate next child
                        anySuccessful = true;
                        break;
                    case NodeState.Running:
                        // A child is running, evaluate next child
                        anyRunning = true;
                        break;
                    case NodeState.Failure:
                        // Do nothing, evaluate next child
                        break;
                }
            }

            NodeState = anySuccessful ? NodeState.Success : anyRunning ? NodeState.Running : NodeState.Failure;
            return NodeState;
        }
    }
}
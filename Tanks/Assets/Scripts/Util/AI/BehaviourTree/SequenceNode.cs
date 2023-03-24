using System.Collections.Generic;

namespace Util.AI.BehaviourTree
{
    /// <summary>
    /// A control flow node representing a Sequence.
    /// </summary>
    public class SequenceNode : Node
    {
        public List<Node> Nodes;

        public SequenceNode(List<Node> nodes)
        {
            Nodes = nodes;
        }

        /// <summary>
        /// Ticks child nodes of the sequence in-order until a child returns
        /// <c>NodeState.Failure</c>, or or all of the children have been ticked.
        /// </summary>
        /// <returns>
        /// The <c>NodeState</c> of the sequence after evaluation.
        /// <list type="bullet">
        /// <item>
        ///     <term><c>NodeState.Success</c></term>
        ///     <description>All of the child nodes were successful.</description>
        /// </item>
        /// <item>
        ///     <term><c>NodeState.Running</c></term>
        ///     <description>At least one of the child nodes is running.</description>
        /// </item>
        /// <item>
        ///     <term><c>NodeState.Failure</c></term>
        ///     <description>One of the child nodes failed.</description>
        /// </item>
        /// </list>
        /// </returns>
        public override NodeState Tick()
        {
            var anyRunning = false;
            foreach (var node in Nodes)
            {
                switch (node.Tick())
                {
                    case NodeState.Success:
                        // Do nothing, evaluate next child
                        break;
                    case NodeState.Running:
                        // A child is running, evaluate next child
                        anyRunning = true;
                        break;
                    case NodeState.Failure:
                        // Short circuit evaluation, exit out 
                        return NodeState.Failure;
                }
            }

            NodeState = anyRunning ? NodeState.Running : NodeState.Success;
            return NodeState;
        }
    }
}
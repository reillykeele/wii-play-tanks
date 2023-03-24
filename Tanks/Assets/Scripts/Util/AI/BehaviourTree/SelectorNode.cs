using System.Collections.Generic;

namespace Util.AI.BehaviourTree
{
    /// <summary>
    /// A control flow node representing a Selector.
    /// </summary>
    public class SelectorNode : Node
    {
        public List<Node> Nodes;

        public SelectorNode(List<Node> nodes)
        {
            Nodes = nodes;
        }

        /// <summary>
        /// Short-circuit ticks all child nodes of the selector. Ticks children in-order
        /// until a child returns <c>NodeState.Success</c> or <c>NodeState.Running</c>, or all
        /// of the children have been ticked.
        /// </summary>
        /// <returns>
        /// The <c>NodeState</c> of the selector after evaluation.
        /// <list type="bullet">
        /// <item>
        ///     <term><c>NodeState.Success</c></term>
        ///     <description>One of the child nodes was successful.</description>
        /// </item>
        /// <item>
        ///     <term><c>NodeState.Running</c></term>
        ///     <description>One of the child nodes is running.</description>
        /// </item>
        /// <item>
        ///     <term><c>NodeState.Failure</c></term>
        ///     <description>None of the child nodes were successful/running.</description>
        /// </item>
        /// </list>
        /// </returns>
        public override NodeState Tick()
        {
            foreach (var node in Nodes)
            {
                switch (node.Tick())
                {
                    case NodeState.Success:
                        // Short circuit evaluation, return success 
                        NodeState = NodeState.Success;
                        return NodeState;
                    case NodeState.Running:
                        // Short circuit evaluation, a child is already running
                        NodeState = NodeState.Running;
                        return NodeState;
                    case NodeState.Failure:
                        // Do nothing, evaluate next child
                        break;
                }
            }

            // None of the child nodes were successful/running, return failure
            NodeState = NodeState.Failure;
            return NodeState;
        }
    }
}

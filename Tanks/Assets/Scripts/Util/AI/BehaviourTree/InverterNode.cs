namespace Util.AI.BehaviourTree
{
    /// <summary>
    /// A decorator node representing an Inverter that inverts the result of its child node.
    /// </summary>
    public class InverterNode : Node
    {
        public Node Node;

        public InverterNode(Node node)
        {
            Node = node;
        }

        /// <summary>
        /// Ticks the child node. If the child returns <c>NodeState.Running</c>, return
        /// <c>NodeState.Running</c>. Otherwise, return the opposite of the result.
        /// </summary>
        /// <returns>
        /// The <c>NodeState</c> of the inverter after evaluation.
        /// <list type="bullet">
        /// <item>
        ///     <term><c>NodeState.Success</c></term>
        ///     <description>The child node failed.</description>
        /// </item>
        /// <item>
        ///     <term><c>NodeState.Running</c></term>
        ///     <description>The child node is running.</description>
        /// </item>
        /// <item>
        ///     <term><c>NodeState.Failure</c></term>
        ///     <description>The child node succeeded.</description>
        /// </item>
        /// </list>
        /// </returns>
        public override NodeState Tick()
        {
            
            switch (Node.Tick())
            {
                case NodeState.Success:
                    // Do nothing, evaluate next child
                    NodeState = NodeState.Failure;
                    break;
                case NodeState.Running:
                    // A child is running evaluate next child
                    NodeState = NodeState.Running;
                    break;
                case NodeState.Failure:
                    // Short circuit evaluation, exit out 
                    NodeState = NodeState.Success;
                    break;
            }
            
            return NodeState;
        }
    }
}
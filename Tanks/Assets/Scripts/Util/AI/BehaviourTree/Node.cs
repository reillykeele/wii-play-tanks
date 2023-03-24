namespace Util.AI.BehaviourTree
{
    public enum NodeState
    {
        Success,
        Running,
        Failure
    }

    public abstract class Node
    {
        /// <summary>
        /// The current state of the Node. Set during <c>Tick()</c>.
        /// </summary>
        public NodeState NodeState { get; protected set; }

        public abstract NodeState Tick();
    }
}

namespace ATBMI
{
    public interface IDialogueRule<T>
    {
        public int RulePriority { get; }  // Represents the specific enum value
        public bool Evaluate(T context);
        public void Execute(T context);
    }
}

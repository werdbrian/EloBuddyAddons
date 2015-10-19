using EloBuddy.SDK;

namespace JokerQuinnBuddy.Modes
{
    public abstract class ModeBase
    {
        public abstract bool ShouldBeExecuted();

        public abstract void Execute();
    }
}

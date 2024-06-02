namespace AlgebraJump.Scripts
{
    public interface IGameStateProvider
    {
        //TODO: Добавить асинхронщину, чтобы подождать
        void SaveGameState();
        void LoadGameState();
    }
}
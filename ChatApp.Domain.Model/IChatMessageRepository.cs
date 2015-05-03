namespace ChatApp.Domain.Model
{
    public interface IChatMessageRepository
    {
        void Save(ChatMessage message);
    }
}

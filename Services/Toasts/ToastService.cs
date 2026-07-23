namespace QuestLog.Services.Toasts;

public enum ToastLevel
{
    Info,
    Success,
    Warning,
    Error,
}

public record ToastMessage(Guid Id, string Text, ToastLevel Level);

public class ToastService
{
    public event Action<ToastMessage>? OnShow;

    public void Show(string text, ToastLevel level = ToastLevel.Info) =>
        OnShow?.Invoke(new ToastMessage(Guid.NewGuid(), text, level));
}

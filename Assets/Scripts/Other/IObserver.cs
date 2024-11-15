public interface IObserver
{
    public void OnNotify(PlayerAction action, float n);
    public void OnNotify(BossAction action, float n);
}

namespace GDD
{
    public interface IComponentUI
    {
        public void OnHighlight();
        public void OnHighlight(int i);

        public void OnDisableHighlight();
        public void OnDisableHighlight(int i);
    }
}
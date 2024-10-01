namespace GDD.Serialize
{
    public interface IScriptableObjectSerialize
    {
        public object[] OnSerialize(object[] value, string _fileName);
        public object[] OnDeSerialize(string _fileName);
    }
}
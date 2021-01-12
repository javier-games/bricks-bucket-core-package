namespace BricksBucket.Core.Generic
{
    public class AbstractValue: IValue
    {
        public virtual object Get(System.Type desiredType) { return null; }

        public virtual void Set(object value) { }
    }
}
namespace BaseStructures
{
    public class Pair<T, V>
    {
        public T first { get; set; }
        public V second { get; set; }

        public Pair()
        { }

        public Pair(T first, V second)
        {
            this.first = first;
            this.second = second;
        }
    }
}

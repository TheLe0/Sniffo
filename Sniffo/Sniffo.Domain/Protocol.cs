namespace Sniffo.Domain
{
    public abstract class Protocol
    {
        protected int Length { get; set; }

        public Protocol(int length)
        {
            Length = length;
        }
    }
}

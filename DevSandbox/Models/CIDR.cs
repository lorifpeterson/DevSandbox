namespace DevSandbox.Models
{
    public class CIDR
    {
        public int Address { get; set; }
        public int Mask { get; set; }

        public bool IsInRange(int address)
        {
            return (Address & Mask) == (address & Mask);
        }


    }

}

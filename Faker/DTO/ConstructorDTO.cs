namespace Faker.DTO
{
    public class ConstructorDto
    {
        private bool _bool;
        private byte _byte;
        private char _char;
        private double _double;
        private float _float;
        private int _int;
        private ulong _long;
        private short _short;

        public ConstructorDto()
        {
        }

        public ConstructorDto(bool _b, byte b, char c, double d, float f, int i, ulong l, short s)
        {
            _bool = _b;
            _byte = b;
            _char = c;
            _double = d;
            _float = f;
            _int = i;
            _long = l;
            _short = s;
        }
    }
}
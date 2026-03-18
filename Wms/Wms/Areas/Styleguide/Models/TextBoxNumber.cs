namespace Wms.Areas.Styleguide.Models
{
    using System.ComponentModel.DataAnnotations;

    public class TextBoxNumber
    {
        [Range(0, 12)]
        public byte ByteField { get; set; }

        // [Range(sbyte.MinValue, sbyte.MaxValue)]
        public sbyte SByteField { get; set; }

        public decimal DecimalField { get; set; }

        public double DoubleField { get; set; }

        public float FloatField { get; set; }

        public int IntField { get; set; }

        public uint UIntField { get; set; }

        public long LongField { get; set; }

        public ulong ULongField { get; set; }

        public short ShortField { get; set; }

        public ushort UShortField { get; set; }
    }
}
using Microsoft.Xrm.Sdk.Metadata;

namespace MsCrmTools.MetadataBrowser.AppCode.AttributeMd
{
    public class MoneyAttributeMetadataInfo : AttributeMetadataInfo
    {
        private readonly MoneyAttributeMetadata amd;

        public MoneyAttributeMetadataInfo(MoneyAttributeMetadata amd)
            : base(amd)
        {
            this.amd = amd;
        }

        public string CalculationOf => amd.CalculationOf;

        public string FormulaDefinition => amd.FormulaDefinition;
        public ImeMode ImeMode => amd.ImeMode.Value;

        public bool IsBaseCurrency => amd.IsBaseCurrency.HasValue && amd.IsBaseCurrency.Value;

        public double MaxValue => amd.MaxValue.Value;

        public double MinValue => amd.MinValue.Value;

        public decimal Precision => amd.Precision.Value;
        public decimal PrecisionSource => amd.PrecisionSource.Value;
        public int SourceTypeMask => amd.SourceTypeMask ?? -1;
    }
}
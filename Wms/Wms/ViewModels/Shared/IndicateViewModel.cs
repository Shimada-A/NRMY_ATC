namespace Wms.ViewModels.Shared
{
    public class IndicateViewModel
    {
        public int IndicateMolecule { get; set; } = 0;

        public int IndicateDenominator { get; set; } = 0;
        public string IndicateTitle { get; set; } = "loading...";

        public string IndicateProgressText => IndicateMolecule < IndicateDenominator ?IndicateMolecule.ToString() + " ／ " + IndicateDenominator.ToString():string.Empty;
        public string IsIndicateVisible => IndicateMolecule < IndicateDenominator?  "initial" : "none" ;
        public string ProcessColor { get; set; } = "#007BFF";

        public void IncrementMolecule()
        {
            IndicateMolecule += 1;
        }
    }
}
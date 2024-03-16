namespace GotoFreight.IATA.Models.Dto
{
    public class ImportGoodDto
    {
        public List<ImportGoodItem> Goods { get; set; } = new List<ImportGoodItem>();

        public List<string> AIResults { get; set; } = new List<string>();

    }

    public class ImportGoodItem
    {
        public string Commodity { get; set; }
        public string PCS { get; set; }
        public string Price { get; set; }
        public string Amount { get; set; }
        public string Usage { get; set; }
        public string Materia { get; set; }
        public string Orginal { get; set; }
        public string Photo { get; set; }
        public string AiResult { get; set; }
    }

}

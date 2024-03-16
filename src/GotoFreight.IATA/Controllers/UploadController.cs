using GotoFreight.AICore.Services;
using GotoFreight.IATA.Models.Dto;
using GotoFreight.IATA.Models.Entity;
using GotoFreight.IATA.Services;
using Microsoft.AspNetCore.Mvc;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.XWPF.UserModel;
using SJZY.Expand.ABP.Core.Common.Dto;

namespace GotoFreight.IATA.Controllers;

public class UploadController : BaseController
{
    #region 构造函数
    private readonly ILogger<HomeController> _logger;
    private readonly ResourceService _resourceService;
    private readonly IImageDetectionService _aiService;
    private const string _systemMessage = "You are a package dangerous goods detection system, need to identify the dangerous goods or controlled goods in the picture, determine whether it is dangerous goods. The following descriptions may indicate dangerous goods: aerosols, cosmetics, chemicals, cleaning solutions, compressed gases, flammable liquids, lighters, lithium batteries, machinery parts, matches, medicines, oxidants, paints, perfumes, solvents.";
    private const string _userMessage = "请分析图片中的货物、品名、材料，根据国际航空运输协会危险品运输规则 (IATA DGR)，分析每个货物属于以下九种货类中的哪一种？请用Json字符串返回，Json字段：Commodity，Materia，是否危险品，DangerousType(危险品第几类)\r\n回复时不要使用markdown语法\r\n识别场景用于运输，客户把图中的货物交给我们，走国际航空运输，运输过程中货物包装不会打开，到达目的地再交给收货人。\r\n分析结果不用要太严谨和正确，提供你倾向的分析就行。\r\n\r\n危险第1类. 爆炸品\r\n例如：炸药、雷管、导火索、信号弹、子弹、烟花、爆竹等\r\n危险第2类. 气体（易燃气体，非易燃无毒气体，有毒气体）\r\n例如：丁烷（打火机燃料）、甲烷、液氮、压缩天然气、喷雾杀虫剂、化清剂、泡沫清洗剂等\r\n危险第3类. 易燃液体\r\n        例如：汽油/柴油煤油、指甲油、香水、酒精或含酒精液体等，以及部分油漆及其稀料、粘合剂等\r\n危险第4类. 易燃固体，自燃物质和遇水释放易燃气体的物质\r\n例如：火柴、硫磺、固体酒精、黄磷和电石等\r\n危险第5类. 氧化剂和有机过氧化物\r\n例如：肥料、漂白粉、双氧水及其他化工产品等\r\n危险第6类. 毒性物质和感染性物质\r\n例如：水银及其化合物、农药、病毒、诊断标本及医疗废弃物等\r\n危险第7类. 放射性物质\r\n危险第8类. 腐蚀性物质\r\n例如：电池电解液、硫酸、盐酸、碱类、汞类等\r\n危险第9类. 杂项危险品\r\n例如：磁性材料、高温物资、聚合物颗粒、干冰、内燃机，mobile, laptop, 手机，笔记本电脑";

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="iOSSService"></param>
    /// <param name="imageDetectionService"></param>
    public UploadController(ILogger<HomeController> logger, ResourceService resourceService, IImageDetectionService aiService)
    {
        _logger = logger;
        _resourceService = resourceService;
        _aiService = aiService;
    }
    #endregion

    /// <summary>
    /// 获取样例文件
    /// </summary>
    /// <param name="file"></param>
    /// <returns></returns>
    [HttpGet("/GetExampleFile")]
    public IActionResult GetExampleFile()
    {
        var fileStream = _resourceService.GetExampleFile();
        return File(fileStream, "application/octet-stream", $"GoodsExample_{DateTime.Now.ToString("MMddHHmmss")}.xlsx");
    }

    /// <summary>
    /// 单文件上传接口
    /// </summary>
    /// <param name="file"></param>
    /// <returns></returns>
    [HttpPost("/Upload")]
    public async Task<UnifyResultDto> Upload(IFormFile file)
    {
        var fullPath = await _resourceService.SaveFileToLocalAsync(file);

        return new UnifyResultDto(true, fullPath);
    }

    /// <summary>
    /// excel导入
    /// </summary>
    /// <param name="file"></param>
    /// <returns></returns>
    [HttpPost("/Import")]
    public async Task<UnifyResultDto> Import(IFormFile form)
    {
        IWorkbook workbook;
        using (var memoryStream = new MemoryStream())
        {
            await form.CopyToAsync(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);

            if (Path.GetExtension(form.FileName) == ".xls")
                workbook = new HSSFWorkbook(memoryStream);
            else if (Path.GetExtension(form.FileName) == ".xlsx")
                workbook = new XSSFWorkbook(memoryStream);
            else
                throw new Exception("不支持的文件类型");
        }

        var res = new ImportGoodDto();
        for (int sheetIndex = 0; sheetIndex < workbook.NumberOfSheets; sheetIndex++)
        {
            ISheet sheet = workbook.GetSheetAt(sheetIndex);

            for (int rowIndex = 1; rowIndex <= sheet.LastRowNum; rowIndex++)
            {
                IRow row = sheet.GetRow(rowIndex);
                if (row == null) continue;

                var model = await GetModelFromCellAsync(row, sheet, rowIndex);
                res.Goods.Add(model);
            }
        }

        foreach (var item in res.Goods)
        {
            //await Task.Delay(1000 * 4);
            //var aiRes = await _aiService.ImageDetectionChatMessageContentAsync(item.Photo, _systemMessage, _userMessage);

            var msg = GetMessageByMimeType("9", item.Commodity);
            item.AiResult = msg;
            res.AIResults.Add(msg);
        }


        return new UnifyResultDto(true, res);
    }

    private string GetMessageByMimeType(string type, string goodName)
    {
        return type switch
        {
            "1" => $"Seems the goods {goodName} is Class 1 Explosives Dangerous goods(IATA DGR).\r\nUsually this kind of goods are not allowed to tranport by major of airlines.\r\nPlease contact Customer Service for further guiding declaration.",
            "2" => $"Seems the goods {goodName} is Class 2 Gases Dangerous goods(IATA DGR).\r\nUsually this kind of goods are not allowed to tranport by major of airlines.\r\nPlease contact Customer Service for further guiding declaration.",
            "3" => $"Seems the goods {goodName} is Class 3 Flammable liquids Dangerous goods(IATA DGR).\r\nPlease contact Customer Service for further guiding declaration.",
            "4" => $"Seems the goods {goodName} is Class 4 Flammable solids Dangerous goods(IATA DGR).\r\nPlease contact Customer Service for further guiding declaration.",
            "5" => $"Seems the goods {goodName} is Class 5 Oxidizing substances or organic peroxides Dangerous goods(IATA DGR).\r\nPlease contact Customer Service for further guiding declaration.",
            "6" => $"Seems the goods {goodName} is Class 6 Toxic or infectious substances Dangerous goods(IATA DGR).\r\nPlease contact Customer Service for further guiding declaration.",
            "7" => $"Seems the goods {goodName} is Class 7 Radioactive material Dangerous goods(IATA DGR).\r\nPlease contact Customer Service for further guiding declaration.",
            "8" => $"Seems the goods {goodName} is Class 8 Corrosive substances Dangerous goods(IATA DGR).\r\nPlease contact Customer Service for further guiding declaration.",
            "9" => $"Seems the goods {goodName} is Class 9 Miscellaneous dangerous substances and articles(IATA DGR).\r\nPlease refer to following criteria for shipping dangerous goods:\r\n1. Providing Material Safety Data Sheet (MSDS)?\r\n2. Providing UN/DOT 38.3, testing helps ensure the safety of lithium ion or lithium metal batteries during shipping.",
            _ => ""
        };
    }

    private async Task<ImportGoodItem> GetModelFromCellAsync(IRow row, ISheet sheet, int rowIndex)
    {
        var res = new ImportGoodItem();
        res.Commodity = row.Cells[0].ToString();
        res.PCS = row.Cells[1].ToString();
        res.Price = row.Cells[2].ToString();
        res.Amount = row.Cells[3].ToString();
        res.Usage = row.Cells[4].ToString();
        res.Materia = row.Cells[5].ToString();
        res.Orginal = row.Cells[7].ToString();

        // 获取图片
        var cell = row.GetCell(8);

        if (cell.CellType == CellType.Blank && cell.CellStyle != null)
        {
            XSSFDrawing drawing = sheet.CreateDrawingPatriarch() as XSSFDrawing;
            if (drawing != null)
            {

                foreach (XSSFShape shape in drawing.GetShapes())
                {
                    if (shape is XSSFPicture)
                    {
                        var picture = (XSSFPicture)shape;
                        var anchor = (XSSFClientAnchor)picture.GetAnchor();

                        if (anchor.Col1 == 8 && anchor.Row1 == rowIndex)
                        {
                            var fileName = "import." + picture.PictureData.PictureType.ToString().ToLower();
                            res.Photo = await _resourceService.SaveFileToLocalAsync(picture.PictureData.Data, fileName);
                            break;
                        }
                    }
                }
            }
        }

        return res;
    }

}
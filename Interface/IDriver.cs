using Crud.Model;

namespace Crud.Interface
{
    public interface IDriver
    {
        public  string ImportExcel(IFormFile file);
        Task<List<Driver>> GetALL(int page);
        Task<byte[]> GenerateExcelAsync();

        //public string DownloadExcel();
    }
}

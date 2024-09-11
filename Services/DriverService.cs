using Crud.Context;
using Crud.Interface;
using Crud.Model;
using ExcelDataReader;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using OfficeOpenXml;
using System.Reflection.Metadata.Ecma335;

namespace Crud.Services
{
    public class DriverService : IDriver
    {
        private readonly UserContext _userContext;
        public DriverService(UserContext userContext)
        {
            _userContext = userContext;
        }


        public async Task<byte[]> GenerateExcelAsync()
        {
            var data = new List<Driver>
        {
            new Driver { Name = "John Doe", Description = "Software Engineer", Area = "Engineering" },
            new Driver { Name = "Jane Smith", Description = "Product Manager", Area = "Product" }
            // Add more records as needed
        };

            using (var stream = new MemoryStream())
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                using (var package = new ExcelPackage(stream))
                {
                    var worksheet = package.Workbook.Worksheets.Add("Drivers");

                    // Add header
                    worksheet.Cells[1, 1].Value = "Name";
                    worksheet.Cells[1, 2].Value = "Description";
                    worksheet.Cells[1, 3].Value = "Area";

                    // Add data
                    for (int i = 0; i < data.Count; i++)
                    {
                        var driver = data[i];
                        worksheet.Cells[i + 2, 1].Value = driver.Name;
                        worksheet.Cells[i + 2, 2].Value = driver.Description;
                        worksheet.Cells[i + 2, 3].Value = driver.Area;
                    }

                    // Auto-fit columns
                    worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                    package.Save();
                }

                return stream.ToArray();
            }
        }

        public  async Task<List<Driver>> GetALL(int page)        {
            
            var result = await _userContext.Drivers
        .Skip((page - 1) * 10)
        .Take(10)
        .ToListAsync();

            return result;
        }

        public string ImportExcel(IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    return "Please upload a correct file.";
                }
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                // Bulk upload
                var drivers = new List<Driver>(); // Create a list to hold records

                using (var stream = new MemoryStream())
                {
                    file.CopyTo(stream);
                    using (var package = new ExcelPackage(stream))
                    {
                        // Get the first worksheet in the workbook
                        ExcelWorksheet worksheet = package.Workbook.Worksheets.FirstOrDefault();
                        if (worksheet == null)
                        {
                            return "No worksheet found in the Excel file.";
                        }

                        var rowCount = worksheet.Dimension.Rows;
                        for (int row = 2; row <= rowCount; row++) // Start from row 2 assuming row 1 is header
                        {
                            var driver = new Driver
                            {
                                Name = worksheet.Cells[row, 1].Value?.ToString()?.Trim(),
                                Description = worksheet.Cells[row, 2].Value?.ToString()?.Trim(),
                                Area = worksheet.Cells[row, 3].Value?.ToString()?.Trim(),
                            };

                            drivers.Add(driver); // Add the record to the list
                        }

                        _userContext.AddRange(drivers); // Add all records to the DbContext at once
                        _userContext.SaveChanges(); // Save changes to the database
                    }
                }

                return "File uploaded successfully.";
            }
            catch (FileNotFoundException ex)
            {
                return "The specified file was not found.";
            }
            catch (Exception ex)
            {
                return "An error occurred: " + ex.Message; // Return the exception message
            }
        }

        //public string ImportExcel(IFormFile file)
        //{
        //    System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
        //    if(file == null) throw new ArgumentNullException("file");   
        //    if(file!=null && file.Length > 0) { 
        //        var uploadDic=$"{Directory.GetCurrentDirectory()}//Uploads";
        //        if (!Directory.Exists(uploadDic))
        //        {
        //            Directory.CreateDirectory(uploadDic);

        //        }
        //        var filePath=Path.Combine(uploadDic,file.FileName);
        //        using (var stream=new FileStream(filePath, FileMode.Create))
        //        {
        //             file.CopyToAsync(stream);
        //        }
        //        //Read file
        //        using (var stream = System.IO.File.Open(filePath, FileMode.Open, FileAccess.Read))
        //        {
        //            var excelData = new List<List<Object>>();

        //            using (var reader = ExcelReaderFactory.CreateReader(stream))
        //            {

        //                do
        //                {
        //                    while (reader.Read())
        //                    {
        //                        var ROWData = new List<Object>();
        //                        for(int col = 0; col<reader.FieldCount; col++)
        //                        {
        //                            ROWData.Add(reader.GetValue(col));  
        //                        }

        //                    }
        //                } while (reader.NextResult());

        //            }
        //        }
        //    }
        //    return "file upload Succefuuly";

        //}

    }

}

using HabitTrackerRazor.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlTypes;
using Microsoft.Data.Sqlite;
using System.Data.Common;
using System.Globalization;

namespace HabitTrackerRazor.Pages
{
    public class DeleteModel : PageModel
    {
        private readonly IConfiguration _configuration;
        public List<DrinkingWaterModel> Records { get; set; }

        public DeleteModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [BindProperty]
        public DrinkingWaterModel DrinkingWater { get; set; }


        public IActionResult OnGet(int id)
        {
            DrinkingWater = GetById(id);
            return Page();
        }
        //
        private DrinkingWaterModel GetById(int id)
        {
            var drinkingWaterRecord = new DrinkingWaterModel();
            using (var connection = new SqliteConnection(_configuration.GetConnectionString("ConnectionString")))
            {
                connection.Open();
                var tablecmd = connection.CreateCommand();
                tablecmd.CommandText =
                    $"SELECT * FROM drinking_water WHERE Id={id}";
                
                SqliteDataReader reader = tablecmd.ExecuteReader();
                reader.Read();
                drinkingWaterRecord = new DrinkingWaterModel{
                            Id = reader.GetInt32(0),
                            Date = DateTime.ParseExact(reader.GetString(1),
                                "d/M/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture),
                    Quantity = reader.GetInt32(2)
                        };
                return drinkingWaterRecord;
            }
        }

        public IActionResult OnPost(int id)
        {
            using (var connection = new SqliteConnection(_configuration.GetConnectionString("ConnectionString")))
            {
                connection.Open();
                var tablecmd = connection.CreateCommand();
                tablecmd.CommandText =
                    $"DELETE FROM drinking_water WHERE Id={id}";
                tablecmd.ExecuteNonQuery();
            }
            return RedirectToPage("./Index");
        }
    }
}
using HabitTrackerRazor.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlTypes;
using Microsoft.Data.Sqlite;
using System.Data.Common;
using System.Globalization;

namespace HabitTrackerRazor.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IConfiguration _configuration;
        public List<DrinkingWaterModel> Records { get; set; }

        public IndexModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void OnGet()
        {
            Records = GetAllRecords();
        }

        private List<DrinkingWaterModel> GetAllRecords()
        {
            using (var connection = new SqliteConnection(_configuration.GetConnectionString("ConnectionString")))
            {
                connection.Open();
                var tablecmd = connection.CreateCommand();
                tablecmd.CommandText =
                    $"SELECT * FROM drinking_water ";
                var tabledata = new List<DrinkingWaterModel>();
                SqliteDataReader reader = tablecmd.ExecuteReader();

                while (reader.Read())
                {
                    tabledata.Add(
                        new DrinkingWaterModel {
                            Id = reader.GetInt32(0),
                            Date = DateTime.ParseExact(reader.GetString(1), 
                                "d/M/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture),
                            Quantity = reader.GetInt32(2)
                        });
                }

                return tabledata;
            }
        }
    }
}
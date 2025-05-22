using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Casino_Project.Data;
using Casino_Project.Model;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Text;
using RhykerHackarryV1.Controllers;
using static RhykerHackarryV1.Controllers.SpinRhykerController;
using System.Reflection.PortableExecutable;

namespace Casino_Project.Pages.Games.Match3
{
    [IgnoreAntiforgeryToken]
    public class IndexModel : PageModel
    {
        const int Rows = 3;              // Количество строк (высота грида)
        const int Columns = 5; // Минимальное количество колонок (ширина)
        const int AmtColors = 4; // Количество типов цветов (0, 1, 2, 3)

        public class SpinApiResponseDto
        {
            public List<int>[] Grid { get; set; }
            public int Del { get; set; }
        }


        private readonly AplicationDbContext _context;

        public IndexModel(AplicationDbContext context)
        {
            _context = context;
        }

        public User MyUser { get; set; }

        public void OnGet()
        {
            //MyUser = _context.Users.FirstOrDefault();  // ДЛЯ ВАС
            MyUser = new User(); // НЕ ДЛЯ ВАС (комментриуйте)

            if (MyUser != null)
            {
                if (MyUser.Balance <= 0)
                    MyUser.Balance = 1265;
                _context.SaveChanges(); // Сохраняем изменения в базу данных
            }
            return;
        }
        public async Task<JsonResult> OnPost()
        {
            try
            {
                if (MyUser == null)
                {
                    Console.WriteLine("УСЕР НУЛЛ! _");
                    //MyUser = _context.Users.FirstOrDefault();
                    MyUser = new User(); // НЕ ДЛЯ ВАС (комментриуйте)
                    MyUser.Balance = 1265;
                }

                Console.WriteLine("Начальный баланс: " + MyUser.Balance);

                using var reader = new StreamReader(Request.Body);
                var body = await reader.ReadToEndAsync(); // 🔁 Используем await

                Console.WriteLine("Тело запроса: " + body);

                double input = JsonSerializer.Deserialize<double>(body);
                Console.WriteLine("OnPostSpin вызван! Ставка: " + input);

                //SpinRhykerController temp2 = new SpinRhykerController();
                //return new JsonResult(new { grid = temp2.GetSpinResult().data });

                if (MyUser.Balance < input)
                    return new JsonResult(new { error = "bad balance" }) { StatusCode = 227 };
                else
                {
                    MyUser.Balance -= input;
                }

                SpinRhykerController temp = new SpinRhykerController(); // от такого дерьмокода даже я в ахуе. но вроде должно работать
                GridAndInt aboba = temp.GetSpinResult();
                if (aboba.amt < 0)
                    return new JsonResult(new { error = "bad result" }) { StatusCode = 228 };
                else
                {
                    MyUser.Balance += input / 5 * aboba.amt;
                }

                Console.WriteLine("Конечный баланс: " + MyUser.Balance);
                _context.SaveChanges(); // Сохраняем изменения в базу данных
                // какой то код
                return new JsonResult(new { grid = aboba.data });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка: " + ex.ToString());
                return new JsonResult(new { error = ex.Message });
            }
        }

        public class SpinInputDto
        {
            public double Bet { get; set; }
        }
    }
}

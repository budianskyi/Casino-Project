using Microsoft.AspNetCore.Mvc;
using System.Drawing; // Подключаем поддержку MVC и API контроллеров

namespace RhykerHackarryV1.Controllers // Пространство имён, в котором находится контроллер
{
    [ApiController] // Указывает, что этот класс — API контроллер
    [Route("api/[controller]")] // Автоматически задаёт маршрут api/Spin, если класс называется SpinController
    public class SpinRhykerController : Controller // Наследуемся от базового контроллера API
    {
        const int Rows = 3;              // Количество строк (высота грида)
        const int Columns = 5; // Минимальное количество колонок (ширина)
        const int AmtColors = 4; // Количество типов цветов (0, 1, 2, 3)
        int BudgetDeviat = 0;
        int ZeroBlyat = 0;

        private bool InRange(int source, int min, int max)
        {
            return source >= min && source <= max;
        }
        private bool InRange(float source, float min, float max)
        {
            return source >= min && source <= max;
        }

        private int Pattern(int source)
        {
            if (source < 60)
                return 0;
            else return source / 20;
        }

        private int RandBudget(Random rnd, int bet)
        {
            int result = 0;
            float temp = BudgetDeviat/bet;
            if (InRange(temp, -0.5f, 0.5f) && BudgetDeviat > -1000)
            {
                result += rnd.Next() % 10 + (rnd.Next() % 10 * rnd.Next() % 10 * rnd.Next() % 10 * rnd.Next() % 10)/3;

            }
            else if (BudgetDeviat > 0)
            {
                result = result / 2 + 30;

            }
            else
            {
                result = result + 30;
            }
            return result;
        }

        private int[] GridZeroElem(int[][] source)
        {
            for (int i = 0; i < Columns; i++)
            {
                for (int j = 0; j < Rows; j++)
                {
                    if (source[i][j] == 0)
                        return new int[2] { i, j };
                }
            }
            return null;
        }

        private int GenerGarbage(int[][] source, Random rnd, int x, int y)
        {
            bool[] isPossible = new bool[AmtColors];
            for (int i = 0; i < AmtColors; i++)
            {
                isPossible[i] = true;
                int[] buf = CheckSequences(source, x, y, i+1);
                for (int j = 0; j < 4; j++)
                {
                    isPossible[i] = isPossible[i] && (buf[j] <= 2);
                }
            }
            
            int temp = 0;
            foreach (bool b in isPossible)
                if (b)
                    temp++;
            if (temp == 0)
                return 0;
            int result = (rnd.Next() % temp) + 1;
            while (isPossible[result - 1] == false)
                result++;
            return result;
        }

        private int GenerRandom(int[][] source, Random rnd, int x = 0, int y = 0)
        {
            int result = (rnd.Next() % AmtColors) + 1;
            return result;
        }

        private void CheckSaveSequences(int[][] source, int x, int y, int color, int oldX, int oldY, ref Sequence curSeq)
        {
            if (!InRange(x, 0, Columns - 1) || !InRange(y, 0, Rows - 1))
                return;
            else if (source[x][y] == color)
            {
                curSeq.length++;
                Coord[] temp = new Coord[curSeq.coords.Length + 1];
                for (int i = 0; i < curSeq.coords.Length; i++)
                {
                    temp[i] = curSeq.coords[i];
                }
                temp[curSeq.coords.Length] = new Coord(x, y);
                curSeq.coords = temp;
                CheckSaveSequences(source, x + x - oldX, y + y - oldY, color, x, y, ref curSeq); 
            }
            else return;
        }

        private Sequence[] CheckSaveSequences(int[][] source, List<Sequence>[] allSeq, int x, int y, int color = 0)
        {
            if (color == 0)
                color = source[x][y];
            Sequence[] result = new Sequence[4];
            int i = 0;
            int j = 1;
            for (int k = 0; k < 4; k++)
            {
                foreach (List<Sequence> temp1 in allSeq)
                    foreach (Sequence temp2 in temp1)
                        foreach (Coord temp3 in temp2.coords)
                            if (temp3.x == x && temp3.y == y)
                            {
                                goto end;
                            }
                result[k].coords = new Coord[1];
                result[k].coords[0] = new Coord(x, y);
                result[k].direction = k;
                result[k].length++;
                result[k].color = color;
                CheckSaveSequences(source, x + i, y + j, color, x, y, ref result[k]);
                CheckSaveSequences(source, x - i, y - j, color, x, y, ref result[k]);

            end:
                i++;
                if (i > 1)
                {
                    i = 1;
                    j--;
                }
            }
            return result;
        }

        private int CheckSequences(int[][] source, int x, int y, int color, int oldX, int oldY)
        {
            if (!InRange(x, 0, Columns - 1) || !InRange(y, 0, Rows - 1))
                return 0;
            else if (source[x][y] == color)
                return CheckSequences(source, x + x - oldX, y + y - oldY, color, x, y) + 1;
            else return 0;
        }

        private int[] CheckSequences(int[][] source, int x, int y, int color = 0)
        {
            if (color == 0)
                color = source[x][y];
            int[] result = new int[4];
            int i = 0;
            int j = 1;
            for (int k = 0; k < 4; k++)
            {
                result[k]++;
                result[k] += CheckSequences(source, x + i, y + j, color, x, y);
                result[k] += CheckSequences(source, x - i, y - j, color, x, y);
                i++;
                if (i > 1)
                {
                    i = 1;
                    j--;
                }
            }
            return result;
        }

        private void UpdateResult(int[][] source, bool[][] counter, ref List<int>[] result)
        {
            for (int x = 0; x < Columns; x++)
            {
                for (int y = 0; y < Rows; y++)
                {
                    if (counter[x][y]) // если editGrid в этой ячейке true counter[x][y]
                    {
                        result[x].Insert(0, source[x][y]); // теперь добавляет в НАЧАЛО списка
                        //result[x].Add(source[x][y]); // добавляем значение из curGrid в соответствующий лист source[x][y]
                    }
                }
            }
            return;
        }


        struct Sequence
        {
            public int color;
            public int length;
            public int direction;

            public Coord[] coords;

            public Sequence(int color, int length, int direction, Coord[] coords = null)
            {
                this.color = color;
                this.length = length;
                this.direction = direction;
                this.coords = coords;
            }
        }


        struct Coord
        {
            public int x;
            public int y;

            public Coord(int x, int y)
            {
                this.x = x;
                this.y = y;
            }
        }



        ref int daunfunct(ref int[][] source, int x, int y, bool[][] delete)
        {
            int counter = 0;
            for (int j = 0; j < y + counter; j++)
                {
                if (y + counter > Rows)
                    return ref ZeroBlyat;
                if (delete[x][y + counter])
                    counter++;
                }
            return ref source[x][y + counter];
        }


        //[HttpGet("balance")] // Метод вызывается по GET-запросу: /api/Spin/spin

        [HttpGet("spin")] // Метод вызывается по GET-запросу: /api/Spin/spin

        public IActionResult GetSpinResult(int getbet = 100)
        {
            Console.WriteLine();
            Console.WriteLine("СТАРТСТАРТСТАРТСТАРТСТАРТСТАРТСТАРТСТАРТСТАРТ");
            int bet = getbet;
            var rnd = new Random(); // Генератор случайных чисел

            int[][] curGrid = new int[Columns][]; // Для текущих обсчетов
            bool[][] editGrid = new bool[Columns][];
            bool[][] deleteGrid = new bool[Columns][];
            for (int i = 0; i < Columns; i++)
            {
                editGrid[i] = new bool[Rows];
                curGrid[i] = new int[Rows];
                deleteGrid[i] = new bool[Rows];
            }


            List<int>[] resultGrid = new List<int>[Columns]; // Массив для результата
            for (int i = 0; i < Columns; i++)
            {
                resultGrid[i] = new List<int>();
            }

        random:
            for (int i = 0; i < Columns; i++)
            {
                for (int j = 0; j < Rows; j++)
                    if (curGrid[i][j] == 0)
                    {
                        curGrid[i][j] = GenerRandom(curGrid, rnd, i, j);
                        editGrid[i][j] = true;
                    }
            }
            goto endgen;
        gener:
            for (int i = 0; i < Columns; i++)
            {
                for (int j = 0; j < Rows; j++)
                    if (curGrid[i][j] == 0)
                    {
                        curGrid[i][j] = GenerGarbage(curGrid, rnd, i, j);
                        editGrid[i][j] = true;
                    }
            }

        endgen:

            UpdateResult(curGrid, editGrid, ref resultGrid);
            for (int x = 0; x < Columns; x++)
            {
                for (int y = 0; y < Rows; y++)
                {
                    editGrid[x][y] = false;
                    deleteGrid[x][y] = false;
                }
            }


            bool delete = false;

            for (int i = 0; i < Columns; i++)
            {
                for (int j = 0; j < Rows; j++)
                {
                    int[] seq = CheckSequences(curGrid, i, j);
                    for (int k = 0; k < 4; k++)
                    {
                        deleteGrid[i][j] = deleteGrid[i][j] || (seq[k] > 2);
                        delete = delete || (seq[k] > 2);
                    }
                }
            }

            Console.WriteLine("Текущее состояние grid:");
            for (int y = Rows - 1; y >= 0; y--)  // Перебираем строки
            {
                string row = "";
                for (int x = 0; x < Columns; x++)  // Перебираем столбцы
                {
                    string symbol = deleteGrid[x][y] ? "+" : "-";  // Если ячейка помечена для удаления, ставим "+"
                    row += curGrid[x][y] + symbol + " ";  // Добавляем значение с символом
                }
                Console.WriteLine(row.Trim());  // Печатаем строку
            }
            Console.WriteLine(); // Пустая строка для разделения

            if (!delete)
            {
                Console.WriteLine("КОНЕЦКОНЕЦКОНЕЦКОНЕЦКОНЕЦКОНЕЦКОНЕЦКОНЕЦКОНЕЦ");
                return Ok(new { grid = resultGrid }); // Возвращаем результат клиенту в формате JSON
            }

            else
                for (int x = 0; x < Columns; x++)
                {
                    int counter = 0;
                    for (int y = 0; y < Rows; y++)
                    {

                    aboba:
                        bool check = true;
                        check = check && (y + counter < 3);
                        if (check)
                            check = check && deleteGrid[x][y + counter];
                        if (check)
                        {
                            counter++;
                            goto aboba;
                        }

                        if (y + counter < 3)
                            curGrid[x][y] = curGrid[x][y + counter];
                        else
                            curGrid[x][y] = 0;
                    }
                }

            goto random;

            blyat:
            //daunfunct();
            bet = 100;
            rnd = new Random(); // Генератор случайных чисел
            int budget = RandBudget(rnd, bet);
            budget = 0;


            List<Sequence>[] isPossible = new List<Sequence>[3];
            Sequence[] temp = null;

            for (int i = 0; i < Columns; i++)
            {
                for (int j = 0; j < Rows; j++)
                {
                    if (curGrid[i][j] == 0)
                    {
                        for (int c = 1; c <= AmtColors; c++)
                        {
                            temp = CheckSaveSequences(curGrid, isPossible, i, j, c);  // оно должно давать массив куда поставить в последовательность.
                            foreach(Sequence s in temp)
                            {
                                if (s.length < 3)
                                    continue;
                                isPossible[s.length].Add(s);
                            }
                        }
                    }
                }
            }

            int curIndex = -1;
            int curLength = -1;
            Sequence curSequence;

            goto b10;

            b10:

            if (budget < 100)
                goto b8;
            if (isPossible[2].Count == 0)
                goto b8;
            else if (rnd.Next() % 3 == 0)
                if (isPossible[1].Count != 0)
                    goto b8;
                else if (isPossible[0].Count != 0)
                    goto b6;
            else
                {
                    curLength = 2;
                    curIndex = rnd.Next() % isPossible[2].Count;
                    curSequence = isPossible[curLength][curIndex];
                    goto makeSeq;
                }

            b8:

            if (budget < 80)
                goto b6;
            if (isPossible[1].Count == 0)
                goto b6;
            else if (rnd.Next() % 3 == 0)
                if (isPossible[0].Count != 0)
                    goto b6;
            else
                {
                    curLength = 1;
                    curIndex = rnd.Next() % isPossible[1].Count;
                    curSequence = isPossible[curLength][curIndex];
                    goto makeSeq;
                }

            b6:
            
            if (budget < 60)
                goto b0;
            if (isPossible[0].Count == 0)
                goto b0;
            else 
                {
                    curLength = 0;
                    curIndex = rnd.Next() % isPossible[0].Count;
                    curSequence = isPossible[curLength][curIndex];
                goto makeSeq;
            }

            makeSeq:
            
            foreach(Coord c in curSequence.coords)
            {
                curGrid[c.x][c.y] = curSequence.color;
                editGrid[c.x][c.y] = true;
                deleteGrid[c.x][c.y] = true;
            }

            if (rnd.Next()%2 == 1)
            {

            }

            b0:

            for (int i = 0; i < Columns; i++)
            {
                for (int j = 0; j < Rows; j++)
                    if (curGrid[i][j] == 0)
                    {
                        curGrid[i][j] = GenerGarbage(curGrid, rnd, i, j);
                        editGrid[i][j] = true;
                    }
            }


            UpdateResult(curGrid, editGrid, ref resultGrid);
            for (int x = 0; x < Columns; x++)
            {
                for (int y = 0; y < Rows; y++)
                {
                    editGrid[x][y] = false;
                }
            }

            return Ok(new { grid = resultGrid }); // Возвращаем результат клиенту в формате JSON
        error:
            return Ok();
                
            end:
            // Допустим, у тебя где-то определено количество строк и колонок

            // Генератор случайных чисел
            /*Random random = new Random();

            // Создание массива
            List<int>[] result = new List<int>[Columns];
            for (int i = 0; i < Columns; i++)
            {
                result[i] = new List<int>(Rows);
            }

            // Заполняем массив случайными числами (индексами цветов)
            for (int x = 0; x < Columns; x++)
            {
                for (int y = 0; y < Rows; y++)
                {
                    int colorIndex = random.Next(0, 5); // от 0 до 4 включительно
                    result[x].Add(colorIndex);
                }
            }*/

            UpdateResult(curGrid, editGrid, ref resultGrid);
            // Возвращаем JSON
            return Ok(new { grid = resultGrid });
        }
    }
}

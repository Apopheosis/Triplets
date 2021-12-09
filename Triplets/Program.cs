using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Mime;
using System.Threading;
using System.Threading.Tasks;



namespace Triplets
{

    class Program
    {
        private static Alphabet alphabet;
        static string text = "";
        

        public static void FindRepetition(object obj)
        {
            Letter let = (Letter) obj;
            for (int i = 0; i < text.Length-3; i++)
            {
                if((let.character==text[i])&&(let.character==text[i+1])&&(let.character==text[i+2])) //Триплеты могут "пересекаться" между собой
                {
                    let.repetition++;
                }
            }

            if (let.repetition == 0)
            {
                alphabet.Remove(let.character);
            }
        }
        public static void BubbleSort(Alphabet alphabet)
        {
            Letter current = alphabet.head;
            while (current.next != null)
            {
                if (current.repetition < current.next.repetition)
                {
                    char tmp_char;
                    int tmp_rep;
                    tmp_char = current.character;
                    tmp_rep = current.repetition;
                    current.character = current.next.character;
                    current.repetition = current.next.repetition;
                    current.next.character = tmp_char;
                    current.next.repetition = tmp_rep;
                    current = alphabet.head; //Для того, чтобы не нарушать связь, мы меняем местами только значения буквы и повторяемости.
                    //Этот алгоритм можно заменить другим, более быстрым алгоритмом сортировки для улучшения производительности.
                }
                else
                {
                    current = current.next;
                }
            }
        }
        
        static async Task Main(string[] args)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            string path;
            Console.WriteLine("Путь к файлу: ");
            path = Console.ReadLine();
            try
            {
                using (FileStream fstream = File.OpenRead(path))
                {
                    byte[] array = new byte[fstream.Length];
                    await fstream.ReadAsync(array, 0, array.Length); //Мультипоточное считывание из файла.
                    text = System.Text.Encoding.Default.GetString(array);
                    Console.WriteLine(text);
                }
            }
            catch (System.UnauthorizedAccessException)
            {
                Console.WriteLine("Недостаточно прав для чтения из файла.");
            }
            catch (System.IO.DirectoryNotFoundException)
            {
                Console.WriteLine("Файл не найден.");
            }

            char[] ForbiddenSymbols = {'!', '.', ',', ':', '?',':',';', '\r', '\n', ' ', '0', '1', '2', '3', '4','5','6','7','8','9'}; //Запрещённые символы, так как задание просит сделать частотный анализ именно букв. 

            alphabet = new Alphabet();

            foreach (var let in text)
            {
                Letter letter = new Letter(let);
                if ((!alphabet.Contains(letter.character))&&(!ForbiddenSymbols.Contains(let))) //Создаём связанный список уникальных букв, содержащихся в тексте и заполняем его.
                {
                    alphabet.Add(letter);
                }
            }

            Thread[] threads = new Thread[alphabet.count];
            int i = 0;
            foreach (Letter let in alphabet) //Подсчитываем кол-во триплетов и убираем из алфавита те буквы, у которых триплетов нет.
            {
                threads[i] = new Thread(FindRepetition);//В этой функции реализация того, о чём говорится выше.
                threads[i].Start(let);
                
                threads[i].Join();
                i++;
                
            }
            

            BubbleSort(alphabet); //Сортируем список триплетов методом пузырька.
            int place = 1;

            foreach (Letter let in alphabet)//Выводим первые десять триплетов.
            {
                
                Console.WriteLine(place + ". " + let.character + " - " + let.repetition);
                place++;
                if (place > 10)
                {
                    break;
                }
            }
            
            Console.WriteLine(stopwatch.Elapsed.Seconds*1000 + stopwatch.Elapsed.Milliseconds + " миллисекунд.");//Вывод времени работы программы в миллисекундах.
        }
    }
}
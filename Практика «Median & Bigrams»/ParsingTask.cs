using System;
using System.Collections.Generic;
using System.Linq;

namespace linq_slideviews;

public class ParsingTask
{
	/// <param name="lines">все строки файла, которые нужно распарсить. Первая строка заголовочная.</param>
	/// <returns>Словарь: ключ — идентификатор слайда, значение — информация о слайде</returns>
	/// <remarks>Метод должен пропускать некорректные строки, игнорируя их</remarks>
	public static IDictionary<int, SlideRecord> ParseSlideRecords(IEnumerable<string> lines)
	{
        return lines.Select(line => line.Split(';'))
                .Where(line => line.Length == 3)
                .Where(line => SlideType(line[1]) != -1)
                .Select(line =>
                {
                    int id;
                    if (int.TryParse(line[0], out id))
                    {
                        return new SlideRecord(id, (SlideType)SlideType(line[1]), line[2]);
                    }
                    return null;
                })
                .Where(slide => slide != null)
                .ToDictionary(slide => slide.SlideId, slide => slide);

    }
    private static int SlideType(string type)
    {
        switch (type)
        {
            case "theory":
                return 0;
            case "exercise":
                return 1;
            case "quiz":
                return 2;
            default:
                return -1;
        }
    }

    /// <param name="lines">все строки файла, которые нужно распарсить. Первая строка — заголовочная.</param>
    /// <param name="slides">Словарь информации о слайдах по идентификатору слайда. 
    /// Такой словарь можно получить методом ParseSlideRecords</param>
    /// <returns>Список информации о посещениях</returns>
    /// <exception cref="FormatException">Если среди строк есть некорректные</exception>
    public static IEnumerable<VisitRecord> ParseVisitRecords
            (IEnumerable<string> lines, IDictionary<int, SlideRecord> slides)
    {
        return lines.Skip(1)
        .Select(line =>
        {
            int UserId;
            int SlideId;
            var visit = line.Split(';');
            DateTime Data;
            if (visit.Length == 4 &&
                int.TryParse(visit[1], out SlideId) &&
                int.TryParse(visit[0], out UserId) &&
                DateTime.TryParse(visit[3], out Data) &&
                DateTime.TryParse(visit[2], out Data) &&
                slides.ContainsKey(SlideId))
            {
                return new VisitRecord(UserId,SlideId, DateTime.Parse($"{visit[2]} {visit[3]}"),slides[SlideId].SlideType);
            }
            else
            {
                throw new FormatException("Wrong line [" + line + "]");
            }
        });
    }
}
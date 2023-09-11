using System;
using System.IO;
using System.Threading.Tasks;
namespace FileUtils{
internal class DataHelper
{
    public static List<string> ExtractValuesAfterColon(string[] inputArray)
    {
        List<string> resultList = new List<string>();

        foreach (string item in inputArray)
        {
            int colonIndex = item.IndexOf(":");
            if (colonIndex != -1 && colonIndex < item.Length - 1)
            {
                string value = item.Substring(colonIndex + 1).Trim();
                resultList.Add(value);
            }
        }

        return resultList;
    }
   public static async Task<List<string>> ReadDataFromFile(string filePath)
{
    List<string> none = new List<string> { "" };
    if (!File.Exists(filePath))
    {
        return none;
    }

    List<string> dataArray = new List<string>(); // Will hold all the extracted values

    using (StreamReader reader = new StreamReader(filePath))
    {
        while (!reader.EndOfStream)
        {
            string line = await reader.ReadLineAsync();
            string[] parts = line.Split(new[] { ", " }, StringSplitOptions.None);

            List<string> resultArray = ExtractValuesAfterColon(parts);

            // Add each value to the dataArray separately
            dataArray.AddRange(resultArray);
        }
    }

    return dataArray;
}
}
}
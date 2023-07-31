using System;
using System.IO;
using System.Threading.Tasks;
namespace FileUtils{
internal class DataHelper
{
    public static string[] ExtractValuesAfterColon(string[] inputArray)
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

        return resultList.ToArray();
    }
    public static async Task<string[]> ReadDataFromFile(string filePath)
    {
        string[] none ={""};
        if (!File.Exists(filePath))
        {
            
            return none;
        }

        string[] dataArray = new string[3]; // Assuming there are three pieces of information per line

        using (StreamReader reader = new StreamReader(filePath))
        {
            int index = 0;
            while (!reader.EndOfStream)
            {
                string line = await reader.ReadLineAsync();
               string[] parts = line.Split(new[] { ", " }, StringSplitOptions.None);

                if (parts.Length != 3)
                {
                return none;
                }

                dataArray[index] = parts[0].Trim();
                index++;

                if (index >= 3) // Assuming we read three pieces of information for each line (username, IP address, game type)
                {
                    index = 0;
                }
                string[] resultArray = ExtractValuesAfterColon(parts);

        for (int i=0;i<3;i++)
        {
            dataArray[i]=resultArray[i];
        }

            }
        }

        return dataArray;
    }
}
}
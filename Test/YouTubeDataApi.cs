using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace  YouTubeHelpers{
internal class YouTubeHelper
{
 
    private static string apiKey = "AIzaSyB8gNVx2CFv0e5DXlR2xKbNgo4M_cAjktg";
    private static string baseURL = "https://www.googleapis.com/youtube/v3";
 private string nextPageToken = null;

    public async Task<List<VideoData>> SearchVideosAsync(string searchQuery, int maxResults)
{
    using (HttpClient client = new HttpClient())
    {
        try
        {
            List<VideoData> videoData = new List<VideoData>();
            int totalResults = 0;
            int resultsPerPage = 10; // Default results per page

            // Calculate the number of pages required based on maxResults
            int totalPages = (int)Math.Ceiling((double)maxResults / resultsPerPage);

            for (int page = 1; page <= totalPages; page++)
            {
                // Make a request to search for videos for each page
                HttpResponseMessage response = await client.GetAsync($"{baseURL}/search?key={apiKey}&q={searchQuery}&part=snippet&type=video&order=viewCount&maxResults={resultsPerPage}&videoLanguage=en&pageToken={(page == 1 ? "" : nextPageToken)}");

                if (response.IsSuccessStatusCode)
                {
                    // Parse the JSON response
                    string responseBody = await response.Content.ReadAsStringAsync();
                    var result = Newtonsoft.Json.JsonConvert.DeserializeObject<YouTubeSearchResult>(responseBody);

                    // Extract video URLs, titles, video creators, and durations
                    foreach (var item in result.items)
                    {
                        videoData.Add(new VideoData
                        {
                            VideoId = item.id.videoId,
                            Title = item.snippet.title,
                            Creator = item.snippet.channelTitle,
                            Duration = item.snippet.liveBroadcastContent
                        });
                    }

                    totalResults += result.pageInfo.resultsPerPage;

                    // Check if we have reached the desired number of results
                    if (totalResults >= maxResults)
                        break;

                    // Store the nextPageToken for the next page (if available)
                    nextPageToken = result.nextPageToken;
                }
                else
                {
                    Console.WriteLine($"HTTP Error: {response.StatusCode}");
                    break;
                }
            }

            return videoData;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return null;
        }
    }
}


     
private class YouTubeSearchResult
{
    public string kind { get; set; }
    public string etag { get; set; }
    public string nextPageToken { get; set; }
    public string regionCode { get; set; }
    public PageInfo pageInfo { get; set; }
    public List<YouTubeVideo> items { get; set; }
}

private class PageInfo
{
    public int totalResults { get; set; }
    public int resultsPerPage { get; set; }
}

private class YouTubeVideo
{
    public string kind { get; set; }
    public string etag { get; set; }
    public VideoId id { get; set; }
    public YouTubeVideoSnippet snippet { get; set; }
}

private class VideoId
{
    public string kind { get; set; }
    public string videoId { get; set; }
}

private class YouTubeVideoSnippet
{
    public DateTime publishedAt { get; set; }
    public string channelId { get; set; }
    public string title { get; set; }
    public string description { get; set; }
    public Thumbnails thumbnails { get; set; }
    public string channelTitle { get; set; }
    public string liveBroadcastContent { get; set; }
    public DateTime publishTime { get; set; }
}

private class Thumbnails
{
    public Thumbnail defaultThumbnail { get; set; }
    public Thumbnail medium { get; set; }
    public Thumbnail high { get; set; }
}

private class Thumbnail
{
    public string url { get; set; }
    public int width { get; set; }
    public int height { get; set; }
}
public class VideoData
{
    public string VideoId { get; set; }
    public string Title { get; set; }
    public string Creator { get; set; }
    public string Duration { get; set; }
}



};
};
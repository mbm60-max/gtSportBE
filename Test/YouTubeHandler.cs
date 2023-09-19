using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using YouTubeHelpers;
using MongoHelpers;

namespace  YouTubeHandlers{
internal class YouTubeHandler
{

    public async Task SearchAndUploadVideos()
        {
            
            string searchQuery = "Simulator Racing Tips/Technique"; // Replace with your search query
            int maxResults = 100; // Replace with the maximum number of results you want
            string gmtDate = DateTimeHelper.GetCurrentGMTDate();
            string collectionName = "LastUpdatedVideo";
            string insertCollectionName = "Videos";
            string databaseName = "GeneralApp";
            string targetAttribute = "lastUpdatedDate";

            //need to change collection name for videos 
            MongoDBHelper MongoHelper= new MongoDBHelper();
            string lastUpdatedDate = MongoHelper.GetLastUpdatedDate(databaseName,collectionName);
            if(lastUpdatedDate != gmtDate){
                //fetch the youtube data

                 YouTubeHelper youTubeHelper = new YouTubeHelper();
                 List<YouTubeHelpers.YouTubeHelper.VideoData> videoData = await youTubeHelper.SearchVideosAsync(searchQuery, maxResults);
                 MongoHelper.InsertYTVideos(insertCollectionName,videoData,databaseName,searchQuery);
                 //update date
                 MongoHelper.UpdateTarget(databaseName,collectionName, gmtDate, targetAttribute);
            }else{
                Console.WriteLine("Date is the same");
                Console.WriteLine($"Current GMT Date: {gmtDate}");
                Console.WriteLine($"Current Stored Date: {lastUpdatedDate}");
                return;
            }
        }
};
};
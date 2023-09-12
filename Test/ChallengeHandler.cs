using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using YouTubeHelpers;
using MongoHelpers;
using ChallengeHelpers;
namespace  ChallengeHandlers{
internal class ChallengeHandler
{

    public  void UploadChallenges()
        {

            string gmtDate = DateTimeHelper.GetCurrentGMTDate();
            string collectionName = "LastUpdatedChallenge";
            string insertCollectionName = "Challenges";
            string databaseName = "GeneralApp";
            string targetAttribute = "lastUpdatedDate";

            //need to change collection name for videos 
            MongoDBHelper MongoHelper= new MongoDBHelper();
            string lastUpdatedDate = MongoHelper.GetLastUpdatedDate(databaseName,collectionName);
            if(lastUpdatedDate != gmtDate){
                //update the values
                 ChallengeHelper challengeHelper = new ChallengeHelper();
                 List<ChallengeHelpers.ChallengeHelper.ChallengeData> challengeData = challengeHelper.GenerateChallenges();
                 MongoHelper.InsertChallenges(insertCollectionName,challengeData,databaseName);
                
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
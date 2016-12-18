using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Vision.v1;
using Google.Apis.Vision.v1.Data;
using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GoogleCloudSamples
{
    public class TextDetection
    {
        public VisionService CreateAuthorizedClient()
        {
            GoogleCredential credential = GoogleCredential.GetApplicationDefaultAsync().Result;
            // Inject the Cloud Vision scopes
            if (credential.IsCreateScopedRequired)
            {
                credential = credential.CreateScoped(new[]
                {
                    VisionService.Scope.CloudPlatform
                });
            }
            return new VisionService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                GZipEnabled = false
            });
        }
        public IList<AnnotateImageResponse> DetectText(VisionService vision, string imagePath)
        {
            Console.WriteLine("Detecting Text...");
            // Convert image to Base64 encoded for JSON ASCII text based request   
            byte[] imageArray = System.IO.File.ReadAllBytes(imagePath);
            string imageContent = Convert.ToBase64String(imageArray);

            // Post text detection request to the Vision API
            ImageContext imageContext = new ImageContext();
            List<string> languages = new List<string>();
            languages.Add("ru");
            var responses = vision.Images.Annotate(
                new BatchAnnotateImagesRequest()
                {
                    Requests = new[] {
                    new AnnotateImageRequest() {
                        Features = new [] { new Feature() { Type ="TEXT_DETECTION"} },
                        Image = new Image() { Content = imageContent },
                        ImageContext = new ImageContext { LanguageHints= languages }
                    }
               }
                }).Execute();
            return responses.Responses;
        }

        public string photo2string(string imagePath)
        {
            TextDetection sample = new TextDetection();

            // Create a new Cloud Vision client authorized via Application 
            VisionService vision = sample.CreateAuthorizedClient();

            // Use the client to get text annotations for the given image
            IList<AnnotateImageResponse> result = sample.DetectText(vision, imagePath);
            // Check for valid text annotations in response
            string s = "";
            if (result[0].TextAnnotations != null)
                foreach (var response in result)
                    foreach (var text in response.TextAnnotations)
                        s = s + text.Description + " ";
            else
            {
                if (result[0].Error == null)
                    s = "No text found.";
                else
                    s = "Not a valid image.";
            }
            return s;
        }


        private static void Main(string[] args)
        {

            string GOOGLE_APPLICATION_CREDENTIALS = @"..\..\My trial project-b349a6699ad0.json";
            string GOOGLE_PROJECT_ID = @"my-trial-project-151511";

            Environment.SetEnvironmentVariable(GOOGLE_APPLICATION_CREDENTIALS, @"..\..\My trial project-b349a6699ad0.json");
            Environment.SetEnvironmentVariable(GOOGLE_PROJECT_ID, @"my-trial-project-151511");
        }

    }
}

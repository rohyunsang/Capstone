using Postgrest.Models; // Postgrest.Models.BaseModels
using Supabase;
using System;
using System.IO;
using UnityEngine;
using Client = Supabase.Client;
using FileOptions = Supabase.Storage.FileOptions;

namespace com.example
{
    public class user_img : BaseModel
    {
        public string user_id { get; set; }
        public string original { get; set; }

    }
    public class user_result : BaseModel
    {
        public string user_id { get; set; }
        public int product_id { get; set; }
        public string result_img { get; set; }

    }

    public class user_size : BaseModel  //
    {
        public string user_id { get; set; }
        public int height { get; set; }
        public int width { get; set; }
        public int s_sleeve { get; set; }
        public int l_sleeve { get; set; }

        public override bool Equals(object obj)
        {
            return obj is user_size productInstance &&
                    height == productInstance.height;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(height);
        }

    }

    public class SupaManager : MonoBehaviour
    {
        public static SupaManager Instance { get; private set; } // singleton
        public SupabaseSettings SupabaseSettings = null!;
        private Client supabase;
        public Texture2D texture2D;
        private async void Awake()
        {
            // 싱글톤 인스턴스 초기화
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject); // 싱글톤 오브젝트 파괴 방지
            }
            else
            {
                Destroy(gameObject); // 중복 인스턴스 제거
                return;
            }

            var options = new SupabaseOptions
            {
                AutoConnectRealtime = true
            };

            supabase = new Supabase.Client(SupabaseSettings.SupabaseURL, SupabaseSettings.SupabaseAnonKey, options);
            await supabase.InitializeAsync();
            /*
             var result = await supabase.From<product>().Select("name").Get();
            List<product> products = result.Models;
            foreach(var product in products){
                Debug.Log(product.name);
            }
             */


        }

        public async void PostHeight()  // using debug button
        {
            var model = new user_size()
            {
                user_id = "db438da4-6bc4-4c10-9bde-6b52fab38e4f",
                height = 185

            };

            await supabase.From<user_size>().Insert(model);


        }
        /*
        public async void SubscribeTable()
        {


            // after Unsubscribe from a channel
        }
        */
        

        public async void UploadImage(byte[] imageBytes)
        {
            SaveImage(imageBytes, "original.jpg");
            // "db438da4-6bc4-4c10-9bde-6b52fab38e4f" is id

            var imagePath = Path.Combine(Application.persistentDataPath, "original.jpg");
            /*
             await supabase.Storage
                          .From("user_result")
                          .Upload(imagePath, "db438da4-6bc4-4c10-9bde-6b52fab38e4f/original.jpg", new FileOptions { CacheControl = "3600", Upsert = true });
            
             var model = new user_img()
            {
                user_id = "db438da4-6bc4-4c10-9bde-6b52fab38e4f",
                original = "db438da4-6bc4-4c10-9bde-6b52fab38e4f/original.jpg"
            };

            await supabase.From<user_img>().Upsert(model);

            var model2 = new user_result()
            {
                user_id = "db438da4-6bc4-4c10-9bde-6b52fab38e4f",
                product_id = 2,
                result_img = "db438da4-6bc4-4c10-9bde-6b52fab38e4f/result.jpg"
            };

            await supabase.From<user_result>().Upsert(model2);
             */

            var result_image = await supabase.Storage.From("user_result").Download("db438da4-6bc4-4c10-9bde-6b52fab38e4f/result.jpg",null);

            texture2D = new Texture2D(2, 2);
            texture2D.LoadImage(result_image);
            
        }
        public async void DownResultImage()
        {
            //await supabase.Storage.From("")
        }

        public void SaveImage(byte[] imageBytes, string fileName)
        {
            // 파일 경로 생성
            string filePath = Path.Combine(Application.persistentDataPath, fileName);

            try
            {
                // 파일 쓰기
                File.WriteAllBytes(filePath, imageBytes);
                Debug.Log("Image saved to: " + filePath);
            }
            catch (Exception ex)
            {
                Debug.LogError("Error saving image: " + ex.Message);
            }
        }

        


    }
}
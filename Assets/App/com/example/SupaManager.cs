using Postgrest.Models; // Postgrest.Models.BaseModels
using Supabase;
using Supabase.Realtime.PostgresChanges;
using Supabase.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using Client = Supabase.Client;
using FileOptions = Supabase.Storage.FileOptions;
//using System.Threading.Tasks;
namespace com.example
{
    #region DB
    public class b_user_height : BaseModel
    {
        public int b_height { get; set; }
        public string user_id { get; set; }
    }

    public class product : BaseModel  //
    {
        public int id { get; set; }
        public string name { get; set; }
        public string model_code { get; set; }
        public int stock { get; set; }
        public int brand_id { get; set; }
        public int price { get; set; }
        public string description { get; set; }
        public string image_url { get; set; }



        public override bool Equals(object obj)
        {
            return obj is product productInstance &&
                    id == productInstance.id;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(id);
        }

    }
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
    #endregion 
    public class SupaManager : MonoBehaviour
    {
        public static SupaManager Instance { get; private set; } // singleton
        public SupabaseSettings SupabaseSettings = null!;
        private Client supabase;
        public byte[] imageBytes;
        public List<string> image_urls;
        [SerializeField]
        public List<product> products;
        public List<Texture2D> texture2Ds;
        public List<byte[]> bytes;

        public Transform scrollViewMain;
        public GameObject clothProduct;

        public Text widthText;
        public Text sSleeveText;
        public Text lSleeveText;
        public Text recommendText;

        public GameObject widthPraceHolder;
        public GameObject sSleevePraceHolder;
        public GameObject lSleevePraceHolder;
        public GameObject recommendPraceHolder;

        public List<GameObject> clothes = new List<GameObject>();
        public List<Texture2D> resultImages = new List<Texture2D>();
        public List<string> urls = new List<string>();
        public Queue<byte[]> byteQueue = new Queue<byte[]>();

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

            SubscribeUserSizeTable();
            DownResultPath();

        }
        public async void GetProductImage(product product)
        {
            string keyword = "folder/";
            int startIndex = product.image_url.IndexOf(keyword);
            string sub_url = "";
            sub_url = product.image_url.Substring(startIndex);
            Debug.Log(sub_url);

            var objects = await supabase.Storage
                          .From("clothes")
                          .Download(sub_url, (sender, progress) => Debug.Log($"{progress}%"));
            Texture2D texture = ConvertToTexture2D(objects);
            InstantiateCloth(texture, product);
        }

        private void Update()
        {
            if (byteQueue.Count > 0)
            {
                Texture2D texture = ConvertToTexture2D(byteQueue.Dequeue());
                resultImages.Add(texture);
            }
        }

        public async void DownResultPath()  // 결과 이미지 다운로드 부분
        {
            await supabase.From<user_result>().On(PostgresChangesOptions.ListenType.All, (sender, change) =>
            {
                string resultPath = change.Model<user_result>().result_img;
                string keyword = "db438da4-6bc4-4c10-9bde-6b52fab38e4f/";
                int startIndex = resultPath.IndexOf(keyword);
                string sub_url = "";
                sub_url = resultPath.Substring(startIndex);
                urls.Add(sub_url);
                Debug.Log(sub_url);
                DownResultImage();
            });
        }
        
        public async void DownResultImage()
        {
            var result_image = await supabase.Storage.From("user_result").Download(urls[0], null);
            byteQueue.Enqueue(result_image);
        }
        public async void UserResultTableUpload()  // 결과 이미지 다운로드 부분
        {
            var model = new user_result()
            {
                user_id = "db438da4-6bc4-4c10-9bde-6b52fab38e4f",
                product_id = 1,
                result_img = "https://gjlvbikvkbtasqrcpsbf.supabase.co/storage/v1/object/public/user_result/db438da4-6bc4-4c10-9bde-6b52fab38e4f/result_1.jpg"
            };

            await supabase.From<user_result>().Upsert(model);
        }
        // https://gjlvbikvkbtasqrcpsbf.supabase.co/storage/v1/object/public/user_result/db438da4-6bc4-4c10-9bde-6b52fab38e4f/result_1.jpg

        public async void GetProductPath()  //using StartBtn in InitPanel
        {
            var productPaths = await supabase.From<product>().Get();
            products = productPaths.Models;
            foreach (var product in products)
            {
                image_urls.Add(product.image_url);
                Debug.Log(product.image_url);
                GetProductImage(product);
            }
        }
        


        public async void UploadHeight(int height)  // using debug button
        {
            var model = new b_user_height()
            {
                user_id = "db438da4-6bc4-4c10-9bde-6b52fab38e4f",
                b_height = height

            };

            await supabase.From<b_user_height>().Upsert(model);
        }



        public async void UploadImage(byte[] imageBytes)
        {
            Debug.Log("UploadImage Call");
            SaveImage(imageBytes, "original.jpeg");
            // "db438da4-6bc4-4c10-9bde-6b52fab38e4f" is id


            var imagePath = Path.Combine(Application.persistentDataPath, "original.jpeg");
            await supabase.Storage
                          .From("user_img")
                          .Upload(imagePath, "db438da4-6bc4-4c10-9bde-6b52fab38e4f/original.jpeg", new FileOptions { CacheControl = "3600", Upsert = true });

            var model = new user_img()
            {
                user_id = "db438da4-6bc4-4c10-9bde-6b52fab38e4f",
                original = "https://gjlvbikvkbtasqrcpsbf.supabase.co/storage/v1/object/public/user_img/db438da4-6bc4-4c10-9bde-6b52fab38e4f/original.jpeg"
            };

            await supabase.From<user_img>().Upsert(model);
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

        private Texture2D ConvertToTexture2D(byte[] imageBytes)
        {
            Texture2D texture = new Texture2D(2, 2);
            if (imageBytes != null && imageBytes.Length > 0)
            {
                if (texture.LoadImage(imageBytes))
                {
                    texture.Apply();
                    return texture;
                }
            }
            return null;
        }

        public void InstantiateCloth(Texture2D texture, product product) //using btn StartBtn in InitPanel
        {
            //size setting is GridLay Out Group
            GameObject instance = Instantiate(clothProduct, scrollViewMain);
            clothes.Add(instance);
            // Find the 'ClothImage' child object of the instance and insert Texture2D cloths
            Transform clothImageTransform = instance.transform.Find("ClothImage");
            Transform clothNameTransform = instance.transform.Find("ClothName");
            Transform clothPriceTransform = instance.transform.Find("ClothPrice");

            if (clothImageTransform != null)
            {
                Image clothImage = clothImageTransform.GetComponent<Image>();
                clothImage.sprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);
                Text nameText = clothNameTransform.GetComponent<Text>();
                nameText.text = product.name;
                Text priceText = clothPriceTransform.GetComponent<Text>();
                priceText.text = product.price.ToString("N0");
                instance.name = product.id.ToString();
            }
        }
        
        




        public async void SubscribeUserSizeTable()
        {
            await supabase.From<user_size>().On(PostgresChangesOptions.ListenType.All, (sender, change) =>
            {
                widthText.text = change.Model<user_size>().width.ToString();
                sSleeveText.text = change.Model<user_size>().s_sleeve.ToString();
                lSleeveText.text = change.Model<user_size>().l_sleeve.ToString();
                Debug.Log(change.Model<user_size>().l_sleeve.ToString());

                widthPraceHolder.SetActive(false);
                sSleevePraceHolder.SetActive(false);
                lSleevePraceHolder.SetActive(false);
                recommendPraceHolder.SetActive(false);
                recommendText.text = DetermineSize(int.Parse(widthText.text));
            });
            
        }
        string DetermineSize(int width)
        {
            string str1 = "고객님의 추천 사이즈는 ";
            string str2 = " 입니다! ";
            if (width <= 47)
            {

                return str1 + "S" + str2;
            }
            else if (width <= 49)
            {
                return str1 + "M" + str2;
            }
            else if (width <= 55)
            {
                return str1 + "L" + str2;
            }
            else if (width <= 60)
            {
                return str1 + "2XL" + str2;
            }
            else 
            {
                return str1 + "3XL" + str2;
            }
        }
    }
}
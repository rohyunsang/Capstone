using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
public class ClothesManager : MonoBehaviour
{
    public UIPrefabManager uIPrefabManager;
    int idx = 0;
    [System.Serializable]
    public class Cloth
    {
        public string _id;
        public string name;
        public int price;
        public string brand;
        public string category;
        public string color;
        public string size;
        public int stock;
        public string imgUrl;
        public int __v;
    }

    public List<Cloth> clothes = new List<Cloth>();
    public string apiURL = "http://220.149.231.136:8051/api/clothes";

    private void Start()
    {
        //GameObject uIPrefabManagerObject = GameObject.Find("UIPrefabManager");
        //uIPrefabManager = uIPrefabManagerObject.GetComponent<UIPrefabManager>();
        StartCoroutine(APIDownload());
    }

    IEnumerator APIDownload()
    {
        UnityWebRequest www = UnityWebRequest.Get(apiURL);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            string jsonData = www.downloadHandler.text;
            ParseClothesData(jsonData);
            uIPrefabManager.PrefabInstantiate();
            
        }
        else
        {
            Debug.Log("Image download failed: " + www.error);
        }
    }

    private void ParseClothesData(string jsonData)
    {
        // Check if the jsonData is empty or null
        if (string.IsNullOrEmpty(jsonData))
        {
            Debug.Log("JSON data is empty or null");
            return;
        }

        // Check if the jsonData is in the correct format
        if (!jsonData.StartsWith("[") || !jsonData.EndsWith("]"))
        {
            Debug.Log("JSON data is not in the correct format");
            return;
        }

        // Remove unnecessary characters at the beginning and end of the JSON data
        jsonData = jsonData.TrimStart('[').TrimEnd(']');

        // Split the jsonData into individual cloth data
        string[] clothDataArray = jsonData.Split("},");

        // Iterate through the clothDataArray and parse each cloth data
        foreach (string clothData in clothDataArray)
        {
            idx++;  // last cloth object only got a '}' So make a branch
            // Remove unnecessary characters from the clothData
            if(idx<10){
                Cloth cloth = JsonUtility.FromJson<Cloth>(clothData + "}");
                clothes.Add(cloth);
            }
            else{
                Cloth cloth = JsonUtility.FromJson<Cloth>(clothData);
                clothes.Add(cloth);
            }
        }

        foreach(Cloth cloth in clothes){
            string clothInfo = string.Format("Cloth Name: {0}, Cloth Price: {1}, Cloth Brand: {2}, Cloth Category: {3}, Cloth Color: {4}, Cloth Size: {5}, Cloth Stock: {6}, Cloth Image URL: {7}, Cloth __V: {8}",
            cloth.name, cloth.price, cloth.brand, cloth.category, cloth.color, cloth.size, cloth.stock, cloth.imgUrl, cloth.__v);
            Debug.Log(clothInfo);
        }
    }
}
using System;
using Supabase;
using UnityEngine;
using Client = Supabase.Client;
using System.Collections.Generic;
using Postgrest.Models; // Postgrest.Models.BaseModels

namespace com.example
{
    public class product : BaseModel  //
    {
        public int id { get; set; }
        public string name { get; set; }
        public string model_code{ get; set; }
        public int stock{ get; set; }
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

    public class SupabaseGet : MonoBehaviour
    {
        public SupabaseSettings SupabaseSettings = null!;
        private Client client;

        private async void Start()
        {
            var options = new SupabaseOptions
            {
                AutoConnectRealtime = true
            };

            var supabase = new Supabase.Client(SupabaseSettings.SupabaseURL, SupabaseSettings.SupabaseAnonKey, options);
            await supabase.InitializeAsync();

            var result = await supabase.From<product>().Select("name").Get();
            List<product> products = result.Models;
            foreach(var product in products){
                Debug.Log(product.name);
            }
            
        }
    }
}
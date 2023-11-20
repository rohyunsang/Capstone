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
        public int Id { get; set; }
        public string Name { get; set; }

        public override bool Equals(object obj)
        {
            return obj is product productInstance &&
                    Id == productInstance.Id;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id);
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
                Debug.Log(product.Name);
            }
            
        }
    }
}
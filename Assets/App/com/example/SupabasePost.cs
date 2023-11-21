using System;
using Supabase;
using UnityEngine;
using Client = Supabase.Client;
using System.Collections.Generic;
using Supabase.Realtime.PostgresChanges; // PostgresChangesOptions.ListenType.Inserts

namespace com.example
{
    public class SupabasePost : MonoBehaviour  //insert data
    {
        public SupabaseSettings SupabaseSettings = null!;
        private Client client;


        public async void Start()
        {

            var options = new SupabaseOptions
            {
                AutoConnectRealtime = true
            };

            var supabase = new Supabase.Client(SupabaseSettings.SupabaseURL, SupabaseSettings.SupabaseAnonKey, options);
            await supabase.InitializeAsync();

#region DB Subscribe 

            await supabase.From<product>().On(PostgresChangesOptions.ListenType.Inserts, (sender, change) =>
            {
                Debug.Log(change.Model<product>().description);
            });
#endregion
            new WaitForSecondsRealtime(2f);

            var model = new product()
            {
                id = 50,
                name = "1",
                model_code = "1",
                stock = 1,
                brand_id = 1,
                price = 1000,
                description = "new post Subscribe 1 ",
                image_url = "http://hello"
            };

            await supabase.From<product>().Insert(model);

        }

    }
}
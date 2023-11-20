using UnityEngine;

namespace com.example
{
	[CreateAssetMenu(fileName = "Supabase", menuName = "Supabase/Supabase Settings", order = 1)]
	public class SupabaseSettings : ScriptableObject
	{
		public string SupabaseURL = "https://gjlvbikvkbtasqrcpsbf.supabase.co";
		public string SupabaseAnonKey = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6ImdqbHZiaWt2a2J0YXNxcmNwc2JmIiwicm9sZSI6ImFub24iLCJpYXQiOjE3MDAxMDkxOTYsImV4cCI6MjAxNTY4NTE5Nn0.OS-yuByVNVyLHltm0ijcWbqhj3O8kkkdtFXiYU2Ep7w";
	}
}

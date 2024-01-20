// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Session.cs" company="">
//
// </copyright>
// <summary>
//   Defines the Session utils type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace NovaFinds.MVC.Utils
{
    using System.Text.Json;

    /// <summary>
    /// The session utils.
    /// </summary>
    public static class Session
    {
        /// <summary>
        /// Store a List in session
        /// </summary>
        public static void StoreListInSession(ISession session, string key, List<Dictionary<string, int>> data)
        {
            var jsonData = JsonSerializer.Serialize(data);
            var byteData = System.Text.Encoding.UTF8.GetBytes(jsonData);
            session.Set(key, byteData);
        }

        /// <summary>
        /// Retrieve a List from session
        /// </summary>
        public static List<Dictionary<string, int>>? RetrieveListFromSession(ISession session, string key)
        {
            if (!session.TryGetValue(key, out var byteData)) return null;
            var jsonData = System.Text.Encoding.UTF8.GetString(byteData);

            return JsonSerializer.Deserialize<List<Dictionary<string, int>>>(jsonData);
        }
    }
}
using Android.Graphics;
using Newtonsoft.Json;
using RestSharp;
using ShapesAndColorsChallenge.DataBase.Controllers;
using ShapesAndColorsChallenge.DataBase.Tables;
using ShapesAndColorsChallenge.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace ShapesAndColorsChallenge.Class.Web
{
    internal static class Rest
    {
        #region VARS

        static AccountToken accountToken;
        static BaseToken baseToken;

        #endregion

        #region METHODS

        /// <summary>
        /// Establece el token de cuenta de SeaTable.
        /// </summary>
        /// <returns></returns>
        static void SetAccountToken()
        {
            try
            {
                if(!Statics.CheckConectivity())
                    accountToken = new();

                RestClientOptions restOptions = new("https://cloud.seatable.io/api2/auth-token/") { MaxTimeout = 10000 };
                RestClient client = new(restOptions);
                RestRequest request = new("https://cloud.seatable.io/api2/auth-token/", Method.Post) { AlwaysMultipartFormData = true };
                request.AddParameter("username", "danielxf@gmail.com");
                request.AddParameter("password", ")Lt5V92^Z3Yv}A@");
                RestResponse response = client.ExecutePost(request);
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                accountToken = JsonConvert.DeserializeObject<AccountToken>(response.Content);
            }
            catch (Exception ex)
            {
                accountToken = new();
            }            
        }

        /// <summary>
        /// Establece un token de acceso de SeaTable.
        /// </summary>
        /// <returns></returns>
        static void SetBaseToken()
        {
            try
            {
                if (!Statics.CheckConectivity())
                    baseToken = new();

                SetAccountToken();

                if (accountToken == null || string.IsNullOrEmpty(accountToken.token))
                    baseToken = new();

                RestClientOptions restOptions = new("https://cloud.seatable.io/api/v2.1/workspace/33837/dtable/SCC/access-token/") { MaxTimeout = 10000 };
                var client = new RestClient(restOptions);
                var request = new RestRequest("https://cloud.seatable.io/api/v2.1/workspace/33837/dtable/SCC/access-token/", Method.Get);
                request.AddHeader("Authorization", $"Token {accountToken.token}");
                RestResponse response = client.ExecuteGet(request);
                baseToken = JsonConvert.DeserializeObject<BaseToken>(response.Content);
            }
            catch
            {
                baseToken = new();
            }
        }

        /// <summary>
        /// Comprueba si el usuario puede usar el token actual como único.
        /// </summary>
        /// <param name="playerToken"></param>
        /// <returns></returns>
        internal static string ExistPlayerToken(string playerToken)
        {
            try
            {
                if (!Statics.CheckConectivity())
                    return "-1";

                SetBaseToken();

                if (baseToken == null || string.IsNullOrEmpty(baseToken.access_token))
                    return "-1";

                RestClientOptions restOptions = new("https://cloud.seatable.io/dtable-db/api/v1/query/50d19bd4-0507-4ad4-bf2c-995cfc6fbf2d") { MaxTimeout = 10000 };
                var client = new RestClient(restOptions);
                var request = new RestRequest("https://cloud.seatable.io/dtable-db/api/v1/query/50d19bd4-0507-4ad4-bf2c-995cfc6fbf2d", Method.Post);
                request.AddHeader("Authorization", $"Token {baseToken.access_token}");
                request.AddHeader("Content-Type", "application/json");
                var body = "{\"sql\": \"select _id, PlayerToken from UserData where PlayerToken='" + playerToken + "'\",\"convert_keys\": true}";
                request.AddStringBody(body, DataFormat.Json);
                RestResponse response = client.ExecutePost(request);
                GlobalRanking globalRanking = JsonConvert.DeserializeObject<GlobalRanking>(response.Content);

                return !globalRanking.results.Any() ? "" : globalRanking.results[0].PlayerToken;
            }
            catch
            {
                return "-1";
            }
        }

        /// <summary>
        /// Obtiene la puntiación de un jugador en un modo de terminado.
        /// </summary>
        /// <param name="gameMode"></param>
        /// <param name="playerToken"></param>
        static (string, long) GetUserScore(GameMode gameMode, string playerToken)
        {
            try 
            {
                if (!Statics.CheckConectivity())
                    return (string.Empty, long.MaxValue);

                SetBaseToken();

                if (baseToken == null || string.IsNullOrEmpty(baseToken.access_token))
                    return (string.Empty, long.MaxValue);

                RestClientOptions restOptions = new("https://cloud.seatable.io/dtable-db/api/v1/query/50d19bd4-0507-4ad4-bf2c-995cfc6fbf2d") { MaxTimeout = 10000 };
                var client = new RestClient(restOptions);
                var request = new RestRequest("https://cloud.seatable.io/dtable-db/api/v1/query/50d19bd4-0507-4ad4-bf2c-995cfc6fbf2d", Method.Post);
                request.AddHeader("Authorization", $"Token {baseToken.access_token}");
                request.AddHeader("Content-Type", "application/json");
                var body = "{\"sql\": \"select _id, Score from Ranking where PlayerToken='" + playerToken + "' AND GameMode=" + gameMode.ToInt() + "\",\"convert_keys\": true}";
                request.AddStringBody(body, DataFormat.Json);
                RestResponse response = client.ExecutePost(request);
                dynamic data = JsonConvert.DeserializeObject(response.Content);
                long score = data.results[0].Score;
                string id = data.results[0]._id;
                return (id, score);
            }
            catch
            {
                return (string.Empty, long.MaxValue);
            }
        }

        /// <summary>
        /// Actualiza una puntuación de jugador.
        /// </summary>
        internal static void UpdateUserScore(GameMode gameMode, string playerToken, long score)
        {
            try
            {
                if (!Statics.CheckConectivity())
                    return;

                playerToken = SetPlayerTokenIfNeeded(playerToken);

                if (string.IsNullOrEmpty(playerToken))/*No tiene playerToken se sale, no hay más que hacer*/
                    return;

                (string id, long lastScore) = GetUserScore(gameMode, playerToken);

                if (score < lastScore)/*No hay nada que guardar, el record actual es inferior*/
                    return;

                RestClientOptions restOptions = new("https://cloud.seatable.io/dtable-server/api/v1/dtables/50d19bd4-0507-4ad4-bf2c-995cfc6fbf2d/rows/") { MaxTimeout = 10000 };
                var client = new RestClient(restOptions);
                var request = new RestRequest("https://cloud.seatable.io/dtable-server/api/v1/dtables/50d19bd4-0507-4ad4-bf2c-995cfc6fbf2d/rows/", Method.Put);
                request.AddHeader("Authorization", $"Token {baseToken.access_token}");
                request.AddHeader("Content-Type", "application/json");
                var body = "{\"row\": {\"Score\": \"" + score.ToString() + "\"},\"table_name\": \"Ranking\", \"row_id\": \"" + id + "\"}";
                request.AddStringBody(body, DataFormat.Json);
                RestResponse response = client.ExecutePut(request);
            }
            catch 
            {

            }
        }

        /// <summary>
        /// Actualiza el nombre de usuario y su pais.
        /// </summary>
        internal static void UpdatePlayerNameCountry(string playerToken)
        { 

        }

        /// <summary>
        /// Almacena el progreso del usuario.
        /// </summary>
        /// <param name="playerToken"></param>
        internal static void UpdateProgress(string playerToken)
        { 

        }

        /// <summary>
        /// Recupera el progreso del usuario.
        /// </summary>
        /// <param name="playerToken"></param>
        /// <returns></returns>
        internal static string GetProgress(string playerToken)
        {
            return string.Empty;
        }

        /// <summary>
        /// Si el usuario no tiene playerToken en local hay que crear uno tanto en local como en la nube.
        /// </summary>
        /// <param name="playerToken"></param>
        /// <returns></returns>
        static string SetPlayerTokenIfNeeded(string playerToken)
        {
            try 
            {
                if (!string.IsNullOrEmpty(playerToken))
                    return playerToken;

                int tries = 0;

                while (tries < 10)
                {
                    string newPlayerToken = Guid.NewGuid().ToString("N").ToUpper();
                    newPlayerToken = newPlayerToken.Length >= 10 ? newPlayerToken.Substring(0, 10) : newPlayerToken.PadRight(10, '0');
                    string currentPlayerToken = ExistPlayerToken(newPlayerToken);

                    if (currentPlayerToken == "-1")/*No se ha podido obtener el playerToken actual, salimos*/
                        return string.Empty;

                    if(currentPlayerToken == newPlayerToken)/*Si se da el caso de que el token nuevo sea igual que el de otro usuario*/
                        tries++;

                    return !SetNewPlayerData(newPlayerToken) ? string.Empty : newPlayerToken;
                }
            }
            catch
            {
                return string.Empty;
            }

            return string.Empty;
        }

        /// <summary>
        /// Crea los datos de usuario nuevos.
        /// </summary>
        static bool SetNewPlayerData(string playerToken)
        {
            try
            {
                RestClientOptions restOptions = new("https://cloud.seatable.io/dtable-server/api/v1/dtables/50d19bd4-0507-4ad4-bf2c-995cfc6fbf2d/rows/") { MaxTimeout = 10000 };
                var client = new RestClient(restOptions);
                var request = new RestRequest("https://cloud.seatable.io/dtable-server/api/v1/dtables/50d19bd4-0507-4ad4-bf2c-995cfc6fbf2d/rows/", Method.Post);
                request.AddHeader("Authorization", $"Token {baseToken.access_token}");
                request.AddHeader("Content-Type", "application/json");
                var body = "{\"row\": {\"PlayerToken\": \"" + playerToken + "\",\"Data\": \"" + string.Empty +  "\", \"Date\": \"" + DateTime.Now.ToString("yyyy-MM-dd") + "\",\"CRC\": \"" + string.Empty + "\"},\"table_name\": \"UserData\"}";
                request.AddStringBody(body, DataFormat.Json);                
                RestResponse response = client.ExecutePost(request);

                if(!response.IsSuccessful)
                    return false;

                Settings settings = ControllerSettings.Get();
                List<string> rows = new();
                StringBuilder stringBuilder = new();
                stringBuilder.Append("{\"rows\":[");

                foreach (GameMode gameMode in System.Enum.GetValues(typeof(GameMode)))
                    if(gameMode != GameMode.None)
                    rows.Add("{\"PlayerToken\": \"" + playerToken + "\",\"Name\": \"" + settings.PlayerName + "\",\"GameMode\": \"" + gameMode.ToInt() + "\",\"Score\": \"" + 0 + "\",\"Country\": \"" + settings.CountryCode + "\"}");

                stringBuilder.Append(string.Join(',', rows));
                stringBuilder.Append("],\"table_name\":\"Ranking\"}");
                restOptions = new("https://cloud.seatable.io/dtable-server/api/v1/dtables/50d19bd4-0507-4ad4-bf2c-995cfc6fbf2d/batch-append-rows/") { MaxTimeout = 10000 };
                client = new RestClient(restOptions);
                request = new RestRequest("https://cloud.seatable.io/dtable-server/api/v1/dtables/50d19bd4-0507-4ad4-bf2c-995cfc6fbf2d/batch-append-rows/", Method.Post);
                request.AddHeader("Authorization", $"Token {baseToken.access_token}");
                request.AddHeader("Content-Type", "application/json");
                request.AddStringBody(stringBuilder.ToString(), DataFormat.Json);
                response = client.ExecutePost(request);

                if (response.IsSuccessful)
                {
                    settings.PlayerToken = playerToken;
                    ControllerSettings.Update(settings);
                    return true;
                }
            }
            catch
            {
                return false;
            }

            return false;
        }

        /// <summary>
        /// Obtiene el listado de ranking total.
        /// </summary>
        /// <param name="gameMode"></param>
        internal static GlobalRanking GetRanking(GameMode gameMode)
        {
            try
            {
                if (!Statics.CheckConectivity())
                    return null;

                SetBaseToken();

                if (baseToken == null || string.IsNullOrEmpty(baseToken.access_token))
                    return null;

                RestClientOptions restOptions = new("https://cloud.seatable.io/dtable-db/api/v1/query/50d19bd4-0507-4ad4-bf2c-995cfc6fbf2d") { MaxTimeout = 10000 };
                RestClient client = new(restOptions);
                var request = new RestRequest("https://cloud.seatable.io/dtable-db/api/v1/query/50d19bd4-0507-4ad4-bf2c-995cfc6fbf2d", Method.Post);
                request.AddHeader("Authorization", $"Token {baseToken.access_token}");
                request.AddHeader("Content-Type", "application/json");
                var body = "{\"sql\": \"select PlayerToken, Name, Score, Country from Ranking where GameMode=" + gameMode.ToInt() + " order by Score desc Limit 200\",\"convert_keys\": true}";
                request.AddStringBody(body, DataFormat.Json);
                RestResponse response = client.ExecutePost(request);
                GlobalRanking globalRanking = JsonConvert.DeserializeObject<GlobalRanking>(response.Content);
                return globalRanking;
            }
            catch
            {
                return null;
            }
        }

        #endregion
    }
}

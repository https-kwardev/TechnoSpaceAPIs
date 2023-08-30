using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using TechnoSpaceAPIs.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TechnoSpaceAPIs.Controllers
{
    [Route("api/paygate")]
    public class PaymentController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public PaymentController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [Route("checkout")]
        public async Task<IActionResult> InitiateCheckOut()
        {
            // Encryption key set in the Merchant Access Portal
            string encryptionKey = "secret";

            DateTime dateTime = DateTime.Now;

            PayData data = new PayData
            {
                PAYGATE_ID = 10011072130,
                REFERENCE = "pgtest_123456789",
                AMOUNT = 3299,
                CURRENCY = "ZAR",
                RETURN_URL = "https://my.return.url/page",
                //TRANSACTION_DATE = dateTime.ToString("yyyy-MM-dd HH:mm:ss"),
                TRANSACTION_DATE = "2018-01-01 12:00:00",
                LOCALE = "en-za",
                COUNTRY = "ZAF",
                EMAIL = "customer@paygate.co.za"
            };

            string dataString = string.Join("", data) + encryptionKey;
            //string checksum = CalculateMD5Hash(dataString);
            string checksum = "59229d9c6cb336ae4bd287c87e6f0220";

            data.CHECKSUM = checksum;

            var httpClient = _httpClientFactory.CreateClient();

            var content = new FormUrlEncodedContent(data.ToDictionary());

            var response = await httpClient.PostAsync("https://secure.paygate.co.za/payweb3/initiate.trans", content);

            string result = await response.Content.ReadAsStringAsync();

            TempData["PaymentResult"] = result;

            if (result != null )
            {
                //return Ok(result);
                return RedirectToAction("Redirect", "Payment");
            }
            else
            {
                return RedirectToAction("PaymentFailedAction", "Payment");
            }
        }

        [Route("redirect")]
        public IActionResult Redirect()
        {
            if (TempData.ContainsKey("PaymentResult"))
            {
                if(TempData["PaymentResult"] != null)
                {
                    var paymentResult = TempData["PaymentResult"]!.ToString()!;
                    _ = new Dictionary<string, string>();
                    Dictionary<string, string> parsedData = ResultToDictionary(paymentResult);

                    string payRequestId = parsedData["PAY_REQUEST_ID"];

                    string checksum = parsedData["CHECKSUM"];


                    ViewBag.PayRequestId = payRequestId;
                    ViewBag.Checksum = checksum;

                    return View("Redirect");
                }
                
            }
            return Ok("Dead End! Contact TechnoSpace or try again later.");
        }

        [Route("return")]
        public IActionResult ReturnUrl()
        {
            var formData = Request.Form;
            if (formData != null)
            {
                return View();
            }
            return View();
        }

        public IActionResult PaymentFailedAction()
        {
            return View();
        }

        public Dictionary<string, string> ResultToDictionary(string result)
        {
            Dictionary<string, string> parsedData = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(result))
            {
                string[] keyValuePairs = result.Split('&');



                foreach (string pair in keyValuePairs)
                {
                    string[] keyValue = pair.Split('=');
                    if (keyValue.Length == 2)
                    {
                        string key = keyValue[0];
                        string value = keyValue[1];
                        parsedData[key] = value;
                    }
                }
            }
            return parsedData;
        }

        static string CalculateMD5Hash(string input)
        {
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }


    }
}

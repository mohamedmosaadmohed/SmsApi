using Microsoft.AspNetCore.Mvc;
using SOfCO.Helpers;
using System.Data;
using System.Data.SqlClient;
using Task.Models;
using PhoneNumbers;
using System.Text.RegularExpressions;

namespace Task.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SmsController : ControllerBase
    {
        private readonly SmsMisrProperties _smsConfig;
        private readonly SQL _sql;

        public SmsController()
        {
            _smsConfig = new SmsMisrProperties(
                "ed810713d5566299555e961265f91315e5ec169924727e38eeac06b2fb736b13",
                "b611afb996655a94c8e942a823f1421de42bf8335d24ba1f84c437b2ab11ca27",
                "c96c511a226d104e40251607398596bc231ffcce63207b67861ef1e94672642e");
            string conection = "Data Source=MOHAMED;Initial Catalog=company;Integrated Security=True";
            _sql = new SQL(conection);
        }

        [HttpPost] // When I send Egyption number I can Enter like (01069584593) or (+201069584593)   In Egypt 010,011,012,015
                   // When I send to another country Must put +(code)Numer 
        public async Task<ActionResult> SendSmsAsync(string mobileNumber, string message, string Language)
        {
            try
            {
                // English message and Egyption number or other country
                if (Language.ToLower() == "english" || Language.ToLower() == "eg")
                {
                    Regex regex = new Regex(@"^(?:\+2)?01[0125]\d{8}$");
                    if (regex.IsMatch(mobileNumber))
                    {
                        if (message.Length <= 160 && message != null)
                        {
                            var encodedMessage = Uri.EscapeDataString(message);
                            var url = $"https://smsmisr.com/api/SMS/?environment=2&username={_smsConfig.Username}&password={_smsConfig.Password}&sender={_smsConfig.SenderID}&mobile={mobileNumber}&language=1&message={encodedMessage}&DelayUntil=''";
                            HttpClient client = new HttpClient();
                            var response = await client.PostAsync(url, null);
                            response.EnsureSuccessStatusCode();

                            var responseContent = await response.Content.ReadAsStringAsync();

                            // Handle the response from the SMS provider's API
                            if (responseContent != null)
                            {
                                return Ok("SMS sent successfully");
                            }
                            else
                            {
                                return BadRequest("SMS Failed to send");
                            }
                        }
                        else
                        {
                            return BadRequest($"SMS length is {message.Length} the min count is 130 Or Message is Empty");
                        }
                    }
                    // other country but must put (+code) 
                    else
                    {
                        PhoneNumberUtil phoneUtil = PhoneNumberUtil.GetInstance();
                        PhoneNumber phoneNumber = phoneUtil.Parse(mobileNumber, "AE");
                        if (!phoneUtil.IsValidNumber(phoneNumber))
                        {
                            return BadRequest("Invalid phone number");
                        }
                        if (message.Length <= 160 && message != null)
                        {
                            var encodedMessage = Uri.EscapeDataString(message);
                            var url = $"https://smsmisr.com/api/SMS/?environment=2&username={_smsConfig.Username}&password={_smsConfig.Password}&sender={_smsConfig.SenderID}&mobile={mobileNumber}&language=1&message={encodedMessage}&DelayUntil=''";
                            HttpClient client = new HttpClient();
                            var response = await client.PostAsync(url, null);
                            response.EnsureSuccessStatusCode();

                            var responseContent = await response.Content.ReadAsStringAsync();
                            if (responseContent != null)
                            {
                                return Ok("SMS sent successfully");
                            }
                            else
                            {
                                return BadRequest("SMS Failed to send");
                            }
                        }
                        else
                        {
                            return BadRequest($"SMS length is {message.Length} the min count is 130 Or Message is Empty");
                        }
                    }
                }
                // Arabic or (Arabic&English) message and Egyption number or other country
                else if (Language.ToLower() == "arabic" || Language.ToLower() == "ab" ||
                    Language.ToLower() == "arabic&english" || Language.ToLower() == "ab&eg")
                {
                    Regex regex = new Regex(@"^(?:\+2)?01[0125]\d{8}$");
                    if (regex.IsMatch(mobileNumber))
                    {
                        if (message.Length <= 70 && message != null)
                        {
                            var encodedMessage = Uri.EscapeDataString(message);
                            var url = $"https://smsmisr.com/api/SMS/?environment=2&username={_smsConfig.Username}&password={_smsConfig.Password}&sender={_smsConfig.SenderID}&mobile={mobileNumber}&language=2&message={encodedMessage}&DelayUntil=''";
                            HttpClient client = new HttpClient();
                            var response = await client.PostAsync(url, null);
                            response.EnsureSuccessStatusCode();

                            var responseContent = await response.Content.ReadAsStringAsync();

                            // Handle the response from the SMS provider's API
                            if (responseContent != null)
                            {
                                return Ok("SMS sent successfully");
                            }
                            else
                            {
                                return BadRequest("SMS Failed to send");
                            }
                        }
                        else
                        {
                            return BadRequest($"SMS length is {message.Length} the min count is 70 Or Message is Empty");
                        }
                    }
                    else
                    {
                        PhoneNumberUtil phoneUtil = PhoneNumberUtil.GetInstance();
                        PhoneNumber phoneNumber = phoneUtil.Parse(mobileNumber, "AE");
                        if (!phoneUtil.IsValidNumber(phoneNumber))
                        {
                            return BadRequest("Invalid phone number");
                        }
                        if (message.Length <= 70 && message != null)
                        {
                            var encodedMessage = Uri.EscapeDataString(message);
                            var url = $"https://smsmisr.com/api/SMS/?environment=2&username={_smsConfig.Username}&password={_smsConfig.Password}&sender={_smsConfig.SenderID}&mobile={mobileNumber}&language=2&message={encodedMessage}&DelayUntil=''";
                            HttpClient client = new HttpClient();
                            var response = await client.PostAsync(url, null);
                            response.EnsureSuccessStatusCode();

                            var responseContent = await response.Content.ReadAsStringAsync();

                            // Handle the response from the SMS provider's API
                            if (responseContent != null)
                            {
                                return Ok("SMS sent successfully");
                            }
                            else
                            {
                                return BadRequest("SMS Failed to send");
                            }
                        }
                        else
                        {
                            return BadRequest($"SMS length is {message.Length} the min count is 130 Or Message is Empty");
                        }
                    }
                }
                return Ok();
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error sending SMS: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        [HttpPost("OTP")]
        public async Task<ActionResult> SendOTPAsync(string mobileNumber)
        {
            try
            {
                Regex regex = new Regex(@"^(?:\+2)?01[0125]\d{8}$");
                if (regex.IsMatch(mobileNumber))
                {
                    // Generate a random 6-digit OTP
                    Random random = new Random();
                    int otpValue = random.Next(100000, 999999);

                    // Set the SMS template token and URL using the generated OTP
                    string templateToken = "0f9217c9d760c1c0ed47b8afb5425708da7d98729016a8accfc14f9cc8d1ba83";
                    string url = $"https://smsmisr.com/api/OTP/?environment=2&username={_smsConfig.Username}&password={_smsConfig.Password}&sender={_smsConfig.SenderID}&mobile={mobileNumber}&template={templateToken}&otp={otpValue}";

                    HttpClient client = new HttpClient();
                    HttpResponseMessage response = await client.PostAsync(url, null);
                    response.EnsureSuccessStatusCode();
                    string responseContent = await response.Content.ReadAsStringAsync();

                    if (responseContent != null)
                    {
                        return Ok($"OTP sent successfully to {mobileNumber}");
                    }
                    else
                    {
                        return BadRequest($"Failed to send OTP to {mobileNumber}");
                    }
                }
                return Ok();
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error sending OTP message: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        [HttpPost("SendDate")]
        public async Task<ActionResult> SendSmsAsyncScheduler(string mobileNumber, string message, string Language, DateTime sendDate)
        {
            try
            {
                // English message and Egyption number or other country
                if (Language.ToLower() == "english" || Language.ToLower() == "eg")
                {
                    Regex regex = new Regex(@"^(?:\+2)?01[0125]\d{8}$");
                    if (regex.IsMatch(mobileNumber))
                    {
                        if (message.Length <= 160 && message != null)
                        {
                            var encodedMessage = Uri.EscapeDataString(message);
                            var url = $"https://smsmisr.com/api/SMS/?environment=2&username={_smsConfig.Username}&password={_smsConfig.Password}&sender={_smsConfig.SenderID}&mobile={mobileNumber}&language=1&message={encodedMessage}&DelayUntil={sendDate.ToString("yyyy-MM-ddTHH:mm:ssZ")}";
                            HttpClient client = new HttpClient();
                            var response = await client.PostAsync(url, null);
                            response.EnsureSuccessStatusCode();

                            var responseContent = await response.Content.ReadAsStringAsync();

                            // Handle the response from the SMS provider's API
                            if (responseContent != null)
                            {
                                return Ok($"SMS will be send to this Numbers at {sendDate.ToString("yyyy-MM-ddTHH:mm:ssZ")}");
                            }
                            else
                            {
                                return BadRequest("SMS Failed to send");
                            }
                        }
                        else
                        {
                            return BadRequest($"SMS length is {message.Length} the min count is 130 Or Message is Empty");
                        }
                    }
                    // other country but must put (+code) 
                    else
                    {
                        PhoneNumberUtil phoneUtil = PhoneNumberUtil.GetInstance();
                        PhoneNumber phoneNumber = phoneUtil.Parse(mobileNumber, "AE");
                        if (!phoneUtil.IsValidNumber(phoneNumber))
                        {
                            return BadRequest("Invalid phone number");
                        }
                        if (message.Length <= 160 && message != null)
                        {
                            var encodedMessage = Uri.EscapeDataString(message);
                            var url = $"https://smsmisr.com/api/SMS/?environment=2&username={_smsConfig.Username}&password={_smsConfig.Password}&sender={_smsConfig.SenderID}&mobile={mobileNumber}&language=1&message={encodedMessage}&DelayUntil={sendDate.ToString("yyyy-MM-ddTHH:mm:ssZ")}";
                            HttpClient client = new HttpClient();
                            var response = await client.PostAsync(url, null);
                            response.EnsureSuccessStatusCode();

                            var responseContent = await response.Content.ReadAsStringAsync();
                            if (responseContent != null)
                            {
                                return Ok($"SMS will be send to this Numbers at {sendDate.ToString("yyyy-MM-ddTHH:mm:ssZ")}");
                            }
                            else
                            {
                                return BadRequest("SMS Failed to send");
                            }
                        }
                        else
                        {
                            return BadRequest($"SMS length is {message.Length} the min count is 130 Or Message is Empty");
                        }
                    }
                }
                // Arabic or (Arabic&English) message and Egyption number or other country
                else if (Language.ToLower() == "arabic" || Language.ToLower() == "ab" ||
                    Language.ToLower() == "arabic&english" || Language.ToLower() == "ab&eg")
                {
                    Regex regex = new Regex(@"^(?:\+2)?01[0125]\d{8}$");
                    if (regex.IsMatch(mobileNumber))
                    {
                        if (message.Length <= 70 && message != null)
                        {
                            var encodedMessage = Uri.EscapeDataString(message);
                            var url = $"https://smsmisr.com/api/SMS/?environment=2&username={_smsConfig.Username}&password={_smsConfig.Password}&sender={_smsConfig.SenderID}&mobile={mobileNumber}&language=2&message={encodedMessage}&DelayUntil={sendDate.ToString("yyyy-MM-ddTHH:mm:ssZ")}";
                            HttpClient client = new HttpClient();
                            var response = await client.PostAsync(url, null);
                            response.EnsureSuccessStatusCode();

                            var responseContent = await response.Content.ReadAsStringAsync();

                            // Handle the response from the SMS provider's API
                            if (responseContent != null)
                            {
                                return Ok($"SMS will be send to this Numbers at {sendDate.ToString("yyyy-MM-ddTHH:mm:ssZ")}");
                            }
                            else
                            {
                                return BadRequest("SMS Failed to send");
                            }
                        }
                        else
                        {
                            return BadRequest($"SMS length is {message.Length} the min count is 70 Or Message is Empty");
                        }
                    }
                    else
                    {
                        PhoneNumberUtil phoneUtil = PhoneNumberUtil.GetInstance();
                        PhoneNumber phoneNumber = phoneUtil.Parse(mobileNumber, "AE");
                        if (!phoneUtil.IsValidNumber(phoneNumber))
                        {
                            return BadRequest("Invalid phone number");
                        }
                        if (message.Length <= 70 && message != null)
                        {
                            var encodedMessage = Uri.EscapeDataString(message);
                            var url = $"https://smsmisr.com/api/SMS/?environment=2&username={_smsConfig.Username}&password={_smsConfig.Password}&sender={_smsConfig.SenderID}&mobile={mobileNumber}&language=2&message={encodedMessage}&DelayUntil={sendDate.ToString("yyyy-MM-ddTHH:mm:ssZ")}";
                            HttpClient client = new HttpClient();
                            var response = await client.PostAsync(url, null);
                            response.EnsureSuccessStatusCode();

                            var responseContent = await response.Content.ReadAsStringAsync();

                            // Handle the response from the SMS provider's API
                            if (responseContent != null)
                            {
                                return Ok($"SMS will be send to this Numbers at {sendDate.ToString("yyyy-MM-ddTHH:mm:ssZ")}");
                            }
                            else
                            {
                                return BadRequest("SMS Failed to send");
                            }
                        }
                        else
                        {
                            return BadRequest($"SMS length is {message.Length} the min count is 130 Or Message is Empty");
                        }
                    }
                }
                return Ok();
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error sending SMS: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        [HttpPost("List")]
        public async Task<ActionResult> SendSmsAsyncToGroup(List<string> mobileNumbers, string message, string Language)
        {
            try
            {
                Regex regex = new Regex(@"^(?:\+2)?01[0125]\d{8}$");
                foreach (string Number in mobileNumbers)
                {
                    if (!regex.IsMatch(Number))
                    {
                        return BadRequest("Invalid phone number");
                    }
                }
                if (Language.ToLower() == "english" || Language.ToLower() == "eg")
                {
                    if (message.Length <= 160 && message != null)
                    {
                        var encodedMessage = Uri.EscapeDataString(message);
                        var url = $"https://smsmisr.com/api/SMS/?environment=2&username={_smsConfig.Username}&password={_smsConfig.Password}&sender={_smsConfig.SenderID}&mobile={string.Join(",", mobileNumbers)}&language=1&message={encodedMessage}&DelayUntil=''";
                        HttpClient client = new HttpClient();
                        var response = await client.PostAsync(url, null);
                        response.EnsureSuccessStatusCode();

                        var responseContent = await response.Content.ReadAsStringAsync();

                        // Handle the response from the SMS provider's API
                        if (responseContent != null)
                        {
                            return Ok("SMS sent successfully");
                        }
                        else
                        {
                            return BadRequest("SMS Failed to send");
                        }
                    }
                    else
                    {
                        return BadRequest($"SMS length is {message.Length} the min count is 130 Or Message is Empty");
                    }
                }
                else if (Language.ToLower() == "arabic" || Language.ToLower() == "ab" ||
                         Language.ToLower() == "arabic&english" || Language.ToLower() == "ab&eg")
                {
                    if (message.Length <= 70 && message != null)
                    {
                        var encodedMessage = Uri.EscapeDataString(message);
                        var url = $"https://smsmisr.com/api/SMS/?environment=2&username={_smsConfig.Username}&password={_smsConfig.Password}&sender={_smsConfig.SenderID}&mobile={string.Join(",", mobileNumbers)}&language=2&message={encodedMessage}&DelayUntil=''";
                        HttpClient client = new HttpClient();
                        var response = await client.PostAsync(url, null);
                        response.EnsureSuccessStatusCode();

                        var responseContent = await response.Content.ReadAsStringAsync();

                        // Handle the response from the SMS provider's API
                        if (responseContent != null)
                        {
                            return Ok("SMS sent successfully");
                        }
                        else
                        {
                            return BadRequest("SMS Failed to send");
                        }
                    }
                    else
                    {
                        return BadRequest($"SMS length is {message.Length} the min count is 70 Or Message is Empty");
                    }
                }
                return Ok();
            }

            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error sending SMS: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        [HttpPost("ListSenddata")]
        public async Task<ActionResult> SendSmsAsyncToGroupScheduler(List<string> mobileNumbers, string message, string Language, DateTime sendDate)
        {
            try
            {
                Regex regex = new Regex(@"^(?:\+2)?01[0125]\d{8}$");
                foreach (string Number in mobileNumbers)
                {
                    if (!regex.IsMatch(Number))
                    {
                        return BadRequest("Invalid phone number");
                    }
                }
                if (Language.ToLower() == "english" || Language.ToLower() == "eg")
                {
                    if (message.Length <= 160 && message != null)
                    {
                        var encodedMessage = Uri.EscapeDataString(message);
                        var url = $"https://smsmisr.com/api/SMS/?environment=2&username={_smsConfig.Username}&password={_smsConfig.Password}&sender={_smsConfig.SenderID}&mobile={string.Join(",", mobileNumbers)}&language=1&message={encodedMessage}&DelayUntil={encodedMessage}&DelayUntil={sendDate.ToString("yyyy-MM-ddTHH:mm:ssZ")}";
                        HttpClient client = new HttpClient();
                        var response = await client.PostAsync(url, null);
                        response.EnsureSuccessStatusCode();

                        var responseContent = await response.Content.ReadAsStringAsync();

                        // Handle the response from the SMS provider's API
                        if (responseContent != null)
                        {
                            return Ok($"SMS will be send to this Numbers at {sendDate.ToString("yyyy-MM-ddTHH:mm:ssZ")}");
                        }
                        else
                        {
                            return BadRequest("SMS Failed to send");
                        }
                    }
                    else
                    {
                        return BadRequest($"SMS length is {message.Length} the min count is 130 Or Message is Empty");
                    }
                }
                else if (Language.ToLower() == "arabic" || Language.ToLower() == "ab" ||
                         Language.ToLower() == "arabic&english" || Language.ToLower() == "ab&eg")
                {
                    if (message.Length <= 70 && message != null)
                    {
                        var encodedMessage = Uri.EscapeDataString(message);
                        var url = $"https://smsmisr.com/api/SMS/?environment=2&username={_smsConfig.Username}&password={_smsConfig.Password}&sender={_smsConfig.SenderID}&mobile={string.Join(",", mobileNumbers)}&language=2&message={encodedMessage}&DelayUntil={encodedMessage}&DelayUntil={sendDate.ToString("yyyy-MM-ddTHH:mm:ssZ")}";
                        HttpClient client = new HttpClient();
                        var response = await client.PostAsync(url, null);
                        response.EnsureSuccessStatusCode();

                        var responseContent = await response.Content.ReadAsStringAsync();

                        // Handle the response from the SMS provider's API
                        if (responseContent != null)
                        {
                            return Ok($"SMS will be send to this Numbers at {sendDate.ToString("yyyy-MM-ddTHH:mm:ssZ")}");
                        }
                        else
                        {
                            return BadRequest("SMS Failed to send");
                        }
                    }
                    else
                    {
                        return BadRequest($"SMS length is {message.Length} the min count is 70 Or Message is Empty");
                    }
                }
                return Ok();
            }

            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error sending SMS: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        [HttpPost("AllSystem")]
        public async Task<ActionResult<Employees>> sendSmsforAllNumberInSystem(string message, string Language)
        {
            try
            {
                Regex regex = new Regex(@"^(?:\+2)?01[0125]\d{8}$");
                int count = 0;
                List<string> mobileNumbers = new List<string>();
                SqlCommand cmd = new SqlCommand("Sp_GetMobileNumber");
                var result = await _sql.ExecuteQueryAsync(cmd);
                foreach (DataRow row in result.Rows)
                {
                    string mobileNumber = row["MobileNumber"]?.ToString();
                    if (!string.IsNullOrEmpty(mobileNumber) && regex.IsMatch(mobileNumber))
                    {
                        mobileNumbers.Add(mobileNumber);
                    }
                    else
                    {
                        return BadRequest($"Invalid phone number: {mobileNumber}");
                    }
                }

                if (Language.ToLower() == "english" || Language.ToLower() == "eg")
                {
                    if (message.Length <= 160 && message != null)
                    {
                        var encodedMessage = Uri.EscapeDataString(message);
                        var url = $"https://smsmisr.com/api/SMS/?environment=2&username={_smsConfig.Username}&password={_smsConfig.Password}&sender={_smsConfig.SenderID}&mobile={string.Join(",", mobileNumbers)}&language=1&message={encodedMessage}&DelayUntil=''";
                        HttpClient client = new HttpClient();
                        var response = await client.PostAsync(url, null);
                        response.EnsureSuccessStatusCode();

                        var responseContent = await response.Content.ReadAsStringAsync();

                        // Handle the response from the SMS provider's API
                        if (responseContent != null)
                        {
                            return Ok("SMS sent successfully to all System ");
                        }
                        else
                        {
                            return BadRequest("SMS Failed to send");
                        }
                    }
                    else
                    {
                        return BadRequest($"SMS length is {message.Length} the min count is 130 Or Message is Empty");
                    }
                }
                else if (Language.ToLower() == "arabic" || Language.ToLower() == "ab" ||
                         Language.ToLower() == "arabic&english" || Language.ToLower() == "ab&eg")
                {
                    if (message.Length <= 70 && message != null)
                    {
                        var encodedMessage = Uri.EscapeDataString(message);
                        var url = $"https://smsmisr.com/api/SMS/?environment=2&username={_smsConfig.Username}&password={_smsConfig.Password}&sender={_smsConfig.SenderID}&mobile={string.Join(",", mobileNumbers)}&language=2&message={encodedMessage}&DelayUntil=''";
                        HttpClient client = new HttpClient();
                        var response = await client.PostAsync(url, null);
                        response.EnsureSuccessStatusCode();

                        var responseContent = await response.Content.ReadAsStringAsync();

                        // Handle the response from the SMS provider's API
                        if (responseContent != null)
                        {
                            return Ok("SMS sent successfully to all System ");
                        }
                        else
                        {
                            return BadRequest("SMS Failed to send");
                        }
                    }
                    else
                    {
                        return BadRequest($"SMS length is {message.Length} the min count is 70 Or Message is Empty");
                    }
                }
                return Ok();
            }

            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error sending SMS: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        [HttpPost("DateAllSystem")]
        public async Task<ActionResult<Employees>> sendSmsforAllNumberInSystem(string message, string Language,DateTime sendDate)
        {
            try
            {
                Regex regex = new Regex(@"^(?:\+2)?01[0125]\d{8}$");
                int count = 0;
                List<string> mobileNumbers = new List<string>();
                SqlCommand cmd = new SqlCommand("Sp_GetMobileNumber");
                var result = await _sql.ExecuteQueryAsync(cmd);
                foreach (DataRow row in result.Rows)
                {
                    string mobileNumber = row["MobileNumber"]?.ToString();
                    if (!string.IsNullOrEmpty(mobileNumber) && regex.IsMatch(mobileNumber))
                    {
                        mobileNumbers.Add(mobileNumber);
                    }
                    else
                    {
                        return BadRequest($"Invalid phone number: {mobileNumber}");
                    }
                }

                if (Language.ToLower() == "english" || Language.ToLower() == "eg")
                {
                    if (message.Length <= 160 && message != null)
                    {
                        var encodedMessage = Uri.EscapeDataString(message);
                        var url = $"https://smsmisr.com/api/SMS/?environment=2&username={_smsConfig.Username}&password={_smsConfig.Password}&sender={_smsConfig.SenderID}&mobile={string.Join(",", mobileNumbers)}&language=1&message={encodedMessage}&DelayUntil={sendDate.ToString("yyyy-MM-ddTHH:mm:ssZ")}";
                        HttpClient client = new HttpClient();
                        var response = await client.PostAsync(url, null);
                        response.EnsureSuccessStatusCode();

                        var responseContent = await response.Content.ReadAsStringAsync();

                        // Handle the response from the SMS provider's API
                        if (responseContent != null)
                        {
                            return Ok("SMS will be send to all System at {sendDate.ToString(yyyy-MM-ddTHH:mm:ssZ)}");
                        }
                        else
                        {
                            return BadRequest("SMS Failed to send");
                        }
                    }
                    else
                    {
                        return BadRequest($"SMS length is {message.Length} the min count is 130 Or Message is Empty");
                    }
                }
                else if (Language.ToLower() == "arabic" || Language.ToLower() == "ab" ||
                         Language.ToLower() == "arabic&english" || Language.ToLower() == "ab&eg")
                {
                    if (message.Length <= 70 && message != null)
                    {
                        var encodedMessage = Uri.EscapeDataString(message);
                        var url = $"https://smsmisr.com/api/SMS/?environment=2&username={_smsConfig.Username}&password={_smsConfig.Password}&sender={_smsConfig.SenderID}&mobile={string.Join(",", mobileNumbers)}&language=2&message={encodedMessage}&DelayUntil={sendDate.ToString("yyyy-MM-ddTHH:mm:ssZ")}";
                        HttpClient client = new HttpClient();
                        var response = await client.PostAsync(url, null);
                        response.EnsureSuccessStatusCode();

                        var responseContent = await response.Content.ReadAsStringAsync();

                        // Handle the response from the SMS provider's API
                        if (responseContent != null)
                        {
                            return Ok("SMS will be send to all System at {sendDate.ToString(yyyy-MM-ddTHH:mm:ssZ)}");
                        }
                        else
                        {
                            return BadRequest("SMS Failed to send");
                        }
                    }
                    else
                    {
                        return BadRequest($"SMS length is {message.Length} the min count is 70 Or Message is Empty");
                    }
                }
                return Ok();
            }

            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error sending SMS: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
//2023-07-05T10:00:00Z
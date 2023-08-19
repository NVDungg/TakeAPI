using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TakeAPI.Models;
using Newtonsoft.Json;
using TakeAPI.Models;
using System.Text;
using System.Diagnostics;

namespace TakeAPI.Controllers
{
    public class VIllaController : Controller
    {
        //private var originalApi = new Uri("https://localhost:7145/api/VillaAPI");
        //With get we just need to call the API to get data
        public async Task<IActionResult> GetVillas()
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7145/api/VillaAPI");    //Get the API link

                // Make an HTTP GET request and get the response
                HttpResponseMessage response = await client.GetAsync("/api/VillaAPI");  //wait the response to access the endpoint

                // Check if the request was successful
                if (response.IsSuccessStatusCode)
                {
                    // Read the response content
                    var responseData = await response.Content.ReadAsStringAsync();

                    // Process the response or deserialize JSON data
                    List<VillaModel> villas = JsonConvert.DeserializeObject<List<VillaModel>>(responseData);

                    //Below you can use the list villas to fillter, order...

                    // Check if the request is AJAX
                    if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    {
                    //because the jquery/ ajax can only take the data type json so we return it in json
                        return Json(villas);
                    }
                    else
                    {
                        // Return the view for regular requests
                        return View(villas);
                    }
                }
                else
                {
                    return View("Error");
                }
            }
        }

        //With post we need a form, model and parameter stand for the new object we create 
        public async Task<IActionResult> CreateVilla([FromBody] VillaModel villa)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7145/api/VillaAPI");

                //Serialize villa obj to Json
                string jsonVilla = JsonConvert.SerializeObject(villa);

                //create a StringContent with Json data
                StringContent content = new StringContent(jsonVilla, Encoding.UTF8, "application/json");

                //send POST request(will send response mess, status code and data)
                HttpResponseMessage response = await client.PostAsync("/api/VillaAPI", content);

                if(response.IsSuccessStatusCode)
                {
                    return Json(new { success = true });
                }
                else
                {
                    return Json(new { success = false });
                }
            }
        }

        ////With put we need id object we choise to input data in form, model and parameter stand for the new data we overide in
        ///To get the right id you will have a controller to take the id for you
        [HttpPut("Villa/UpdateVilla/{id}")]
        public async Task<IActionResult> UpdateVilla(int id, [FromBody] VillaModel updatedVilla)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7145/api/VillaAPI");

                string jsonVilla = JsonConvert.SerializeObject(updatedVilla);

                StringContent content = new StringContent(jsonVilla, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PutAsync($"/api/VillaAPI/{id}", content);

                if (response.IsSuccessStatusCode)
                {
                    return Json(new { success = true });
                }
                else
                {
                    return Json(new { success = false });
                }
            }
        }

        [HttpGet("Villa/GetVillaById/{id}")]
        public async Task<IActionResult> GetVillaById(int id)
        {
            try {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://localhost:7145/api/VillaAPI");

                    HttpResponseMessage response = await client.GetAsync($"/api/VillaAPI/{id}");
                    Console.WriteLine("B4 go to the logic if else");
                    if (response.IsSuccessStatusCode)
                    {
                        var responseData = await response.Content.ReadAsStringAsync();
                        var villa = JsonConvert.DeserializeObject<VillaModel>(responseData);

                        return Json(villa);
                    }
                    else
                    {
                        Console.WriteLine("Print the Error");
                        return Json(BadRequest());
                    }
                }
            }
            catch (Exception )
            {
                return StatusCode(StatusCodes.Status500InternalServerError,"Error retrieving data from the database");
            }
           
        }


        //All we need in delete it the id
        public async Task<IActionResult> DeleteVilla(int id )
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7145/api/VillaAPI");

                HttpResponseMessage response = await client.DeleteAsync($"/api/VillaAPI/{id}");

                if (response.IsSuccessStatusCode)
                {
                    return Json(new{success = true});
                }
                else
                {
                    return BadRequest();
                }
            }
        }
        public IActionResult VillaListPartial()
        {
            return View();
        }
       
    }
}

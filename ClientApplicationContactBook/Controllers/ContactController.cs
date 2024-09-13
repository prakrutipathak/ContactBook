using ClientApplicationContactBook.Implementation;
using ClientApplicationContactBook.Infrastructure;
using ClientApplicationContactBook.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace ClientApplicationContactBook.Controllers
{
    public class ContactController : Controller
    {

        private readonly IHttpClientService _httpClientService;
        private readonly IConfiguration _configuration;
        private readonly IImageUpload _imageUpload;
        private string endPoint;
        public ContactController(IHttpClientService httpClientService, IConfiguration configuration, IImageUpload imageUpload)
        {
            _httpClientService = httpClientService;
            _configuration = configuration;
            _imageUpload = imageUpload;
            endPoint = _configuration["EndPoint:CivicaApi"];
        }
        public IActionResult Index()
        {
            ServiceResponse<IEnumerable<ContactViewModel>> response = new ServiceResponse<IEnumerable<ContactViewModel>>();
            response = _httpClientService.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactViewModel>>>
                ($"{endPoint}Contact/GetAllContacts", HttpMethod.Get, HttpContext.Request);
            if (response.Success)
            {
                return View(response.Data);
            }
            return View(new List<ContactViewModel>());
        }
        public IActionResult Index1(char? letter, string? search, int page = 1, int pageSize = 2, string sortOrder= "asc")
        {
            var apiGetContactsUrl = "";
            var apiGetLettersUrl = $"{endPoint}Contact/GetAllContacts";
            var apiGetCountUrl = "";
            if (letter != null || search !=null)
            {
                apiGetContactsUrl = $"{endPoint}Contact/GetAllContactsByPagination/?letter={letter}&search={search}&page={page}&pageSize={pageSize}&sortOrder={sortOrder}";
                apiGetCountUrl = $"{endPoint}Contact/GetContactsCount/?letter={letter}&search={search}";
            }
            else
            {
                apiGetContactsUrl = $"{endPoint}Contact/GetAllContactsByPagination?page={page}&pageSize={pageSize}&sortOrder={sortOrder}";
                apiGetCountUrl = $"{endPoint}Contact/GetContactsCount";

            }
           
            ServiceResponse<int> countOfContact = new ServiceResponse<int>();

            countOfContact = _httpClientService.ExecuteApiRequest<ServiceResponse<int>>
                (apiGetCountUrl, HttpMethod.Get, HttpContext.Request);

            int totalCount = countOfContact.Data;
            if (totalCount == 0)
            {
                return View(new List<ContactViewModel>());
            }
            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);
            if (page > totalPages)
            {
                return RedirectToAction("Index1", new { page = 1, pageSize,letter,sortOrder, search });
            }
            ViewBag.CurrentPage = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalPages = totalPages;
            ViewBag.Letter = letter;
            ViewBag.SortOrder = sortOrder;
            ViewBag.Search = search;

            ServiceResponse<IEnumerable<ContactViewModel>> response = new ServiceResponse<IEnumerable<ContactViewModel>>();

            response = _httpClientService.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactViewModel>>>
                (apiGetContactsUrl, HttpMethod.Get, HttpContext.Request);
            ServiceResponse<IEnumerable<ContactViewModel>>? getLetters = new ServiceResponse<IEnumerable<ContactViewModel>>();

            getLetters = _httpClientService.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactViewModel>>>
                (apiGetLettersUrl, HttpMethod.Get, HttpContext.Request);

            if (getLetters.Success)
            {
                var distinctLetters = getLetters.Data.Select(contact => char.ToUpper(contact.FirstName.FirstOrDefault()))
                                                            .Where(firstLetter => firstLetter != default(char))
                                                            .Distinct()
                                                             .OrderBy(letter => letter)
                                                            .ToList();
                ViewBag.DistinctLetters = distinctLetters;

            }

            if (response.Success)
            {
                return View(response.Data);
            }

            return View(new List<ContactViewModel>());
        }
        public IActionResult Favourite(char? letter, int page = 1, int pageSize = 2, string sortOrder = "asc")
        {
            var apiGetContactsUrl = "";

            var apiGetCountUrl = "";
            var apiGetLettersUrl = $"{endPoint}Contact/GetAllFavourite";
            if (letter != null)
            {
                apiGetContactsUrl = $"{endPoint}Contact/GetPaginatedFavouriteContacts/?letter={letter}&page={page}&pageSize={pageSize}&sortOrder={sortOrder}";
                apiGetCountUrl = $"{endPoint}Contact/TotalContactFavourite/?letter={letter}";
            }
            else
            {
                apiGetContactsUrl = $"{endPoint}Contact/GetPaginatedFavouriteContacts" + "?page=" + page + "&pageSize=" + pageSize +"&sortOrder="+ sortOrder;
                apiGetCountUrl = $"{endPoint}Contact/TotalContactFavourite";

            }
            ServiceResponse<int> countOfContact = new ServiceResponse<int>();

            countOfContact = _httpClientService.ExecuteApiRequest<ServiceResponse<int>>
                (apiGetCountUrl, HttpMethod.Get, HttpContext.Request);

            int totalCount = countOfContact.Data;
            if (totalCount == 0)
            {
                return View(new List<ContactViewModel>());
            }
            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);
            if (page > totalPages)
            {
                return RedirectToAction("Index1", new { page = 1, pageSize, sortOrder,letter });
            }
            ViewBag.CurrentPage = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalPages = totalPages;
            ViewBag.SortOrder = sortOrder;
            ViewBag.Letter = letter;

            ServiceResponse<IEnumerable<ContactViewModel>> response = new ServiceResponse<IEnumerable<ContactViewModel>>();

            response = _httpClientService.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactViewModel>>>
                (apiGetContactsUrl, HttpMethod.Get, HttpContext.Request);
            ServiceResponse<IEnumerable<ContactViewModel>>? getLetters = new ServiceResponse<IEnumerable<ContactViewModel>>();

            getLetters = _httpClientService.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactViewModel>>>
                (apiGetLettersUrl, HttpMethod.Get, HttpContext.Request);

            if (getLetters.Success)
            {
                var distinctLetters = getLetters.Data.Select(contact => char.ToUpper(contact.FirstName.FirstOrDefault()))
                                                            .Where(firstLetter => firstLetter != default(char))
                                                            .Distinct()
                                                             .OrderBy(letter => letter)
                                                            .ToList();
                ViewBag.DistinctLetters = distinctLetters;

            }

            if (response.Success)
            { 
                return View(response.Data);
            }

            return View(new List<ContactViewModel>());
        }
        [Authorize]
        [HttpGet]
        public IActionResult Create()
        {
            AddContactViewModel viewModel = new AddContactViewModel();
            viewModel.Countries=GetCountries();
            viewModel.States = GetStates();
            return View(viewModel);
        }
        [Authorize]
        [HttpPost]
        public IActionResult Create(AddContactViewModel viewModel)
        {
            viewModel.Countries = GetCountries();
            viewModel.States = GetStates();
            if (ModelState.IsValid)
            {
                IFormFile imageFile = viewModel.File;
                if (imageFile != null && imageFile.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        viewModel.File.CopyTo(memoryStream);
                        viewModel.ImageByte = memoryStream.ToArray();
                    }
                    // Process the image file
                    var fileName = _imageUpload.AddImageFileToPath(imageFile);
                    viewModel.Image = fileName;
                }

                var apiUrl = $"{endPoint}Contact/Create/";
                var response = _httpClientService.PostHttpResponseMessage<AddContactViewModel>(apiUrl, viewModel, HttpContext.Request);
                if (response.IsSuccessStatusCode)
                {
                    string successResponse = response.Content.ReadAsStringAsync().Result;
                    var serviceResponse = JsonConvert.DeserializeObject<ServiceResponse<string>>(successResponse);
                    TempData["SuccessMessage"] = serviceResponse.Message;
                    return RedirectToAction("Index1");
                }
                else
                {
                    string errorData = response.Content.ReadAsStringAsync().Result;
                    var errorResponse = JsonConvert.DeserializeObject<ServiceResponse<string>>(errorData);
                    if (errorResponse != null)
                    {
                        TempData["ErrorMessage"] = errorResponse.Message;
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Something went wrong try after some time";
                        return RedirectToAction("Index1");
                    }
                }
            }
           
            return View(viewModel);
        }

        public IActionResult Details(int id)
        {
            var apiUrl = $"{endPoint}Contact/GetContactById/" + id;
            var response = _httpClientService.GetHttpResponseMessage<ContactViewModel>(apiUrl, HttpContext.Request);
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                var serviceResponse = JsonConvert.DeserializeObject<ServiceResponse<ContactViewModel>>(data);
                if (serviceResponse != null && serviceResponse.Success && serviceResponse.Data != null)
                {
                    return View(serviceResponse.Data);
                }
                else
                {
                    TempData["ErrorMessage"] = serviceResponse.Message;
                    return RedirectToAction("Index1");
                }
            }
            else
            {
                string errorData = response.Content.ReadAsStringAsync().Result;
                var errorResponse = JsonConvert.DeserializeObject<ServiceResponse<ContactViewModel>>(errorData);
                if (errorResponse != null)
                {
                    TempData["ErrorMessage"] = errorResponse.Message;
                }
                else
                {
                    TempData["ErrorMessage"] = "Something went wrong.Please try after sometime.";
                }
                return RedirectToAction("Index1");
            }

        }
        [Authorize]
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var apiUrl = $"{endPoint}Contact/GetContactById/" + id;
            var response = _httpClientService.GetHttpResponseMessage<AddContactViewModel>(apiUrl, HttpContext.Request);
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                var serviceResponse = JsonConvert.DeserializeObject<ServiceResponse<AddContactViewModel>>(data);
                if (serviceResponse != null && serviceResponse.Success && serviceResponse.Data != null)
                {
                    AddContactViewModel viewModel = serviceResponse.Data;
                    viewModel.Countries = GetCountries();
                    viewModel.States = GetStates();
                    return View(viewModel);
                }
                else
                {
                    TempData["ErrorMessage"] = serviceResponse.Message;
                    return RedirectToAction("Index1");
                }
            }
            else
            {
                string errorData = response.Content.ReadAsStringAsync().Result;
                var errorResponse = JsonConvert.DeserializeObject<ServiceResponse<AddContactViewModel>>(errorData);
                if (errorResponse != null)
                {
                    TempData["ErrorMessage"] = errorResponse.Message;
                }
                else
                {
                    TempData["ErrorMessage"] = "Something went wrong.Please try after sometime.";
                }
                return RedirectToAction("Index1");
            }
        }
        [Authorize]
        [HttpPost]
        public IActionResult Edit(AddContactViewModel updateContact)
        {
            updateContact.Countries = GetCountries();
            updateContact.States = GetStates();
            if (ModelState.IsValid)
            {
                if (updateContact.File != null && updateContact.File.Length > 0)
                {
                   
                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" }; // Add other allowed extensions as needed
                    var fileExtension = Path.GetExtension(updateContact.File.FileName).ToLower();
                    if (!allowedExtensions.Contains(fileExtension))
                    {
                        TempData["ErrorMessage"] = "Only JPG, JPEG, and PNG file extensions are allowed.";
                        return View(updateContact);
                    }
                    using (var memoryStream = new MemoryStream())
                    {
                        updateContact.File.CopyTo(memoryStream);
                        updateContact.ImageByte = memoryStream.ToArray();
                    }
                    // Process the image file
                    var fileName = _imageUpload.AddImageFileToPath(updateContact.File);
                    updateContact.Image = fileName;
                }
                else if (updateContact.RemoveImageHidden == "true")
                {
                    // User wants to remove the current image
                    updateContact.Image = string.Empty;
                    updateContact.ImageByte = null;
                }
                else
                {
                    // Use the previous file name if no new file is provided
                    updateContact.Image = updateContact.Image;
                }

                var apiUrl = $"{endPoint}Contact/Edit";
                HttpResponseMessage response = _httpClientService.PutHttpResponseMessage(apiUrl, updateContact, HttpContext.Request);
                if (response.IsSuccessStatusCode)
                {
                    string successResponse = response.Content.ReadAsStringAsync().Result;
                    var serviceResponse = JsonConvert.DeserializeObject<ServiceResponse<string>>(successResponse);
                    TempData["SuccessMessage"] = serviceResponse.Message;
                    return RedirectToAction("Index1");
                }
                else
                {
                    string errorData = response.Content.ReadAsStringAsync().Result;
                    var errorResponse = JsonConvert.DeserializeObject<ServiceResponse<string>>(errorData);
                    if (errorResponse != null)
                    {
                        TempData["ErrorMessage"] = errorResponse.Message;
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Something went wrong try after some time";
                        return RedirectToAction("Index1");
                    }
                }
            }
            return View(updateContact);
        }
        [Authorize]
        public IActionResult Delete(int id)
        {
            var apiUrl = $"{endPoint}Contact/GetContactById/" + id;
            var response = _httpClientService.GetHttpResponseMessage<ContactViewModel>(apiUrl, HttpContext.Request);
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                var serviceResponse = JsonConvert.DeserializeObject<ServiceResponse<ContactViewModel>>(data);
                if (serviceResponse != null && serviceResponse.Success && serviceResponse.Data != null)
                {
                    return View(serviceResponse.Data);
                }
                else
                {
                    TempData["ErrorMessage"] = serviceResponse.Message;
                    return RedirectToAction("Index1");
                }
            }
            else
            {
                string errorData = response.Content.ReadAsStringAsync().Result;
                var errorResponse = JsonConvert.DeserializeObject<ServiceResponse<ContactViewModel>>(errorData);
                if (errorResponse != null)
                {
                    TempData["ErrorMessage"] = errorResponse.Message;
                }
                else
                {
                    TempData["ErrorMessage"] = "Something went wrong.Please try after sometime.";
                }
                return RedirectToAction("Index1");
            }

        }
        [Authorize]
        public IActionResult DeleteConfirmed(int ContactId)
        {
            var apiUrl = $"{endPoint}Contact/Delete/" + ContactId;
            //var response = _httpClientService.GetHttpResponseMessage<string>(apiUrl, HttpContext.Request);
            var response = _httpClientService.ExecuteApiRequest<ServiceResponse<string>>
                   ($"{apiUrl}", HttpMethod.Delete, HttpContext.Request);
            if (response.Success)
            {
                TempData["SuccessMessage"] = response.Message;
                return RedirectToAction("Index1");
            }
            else
            {
                TempData["ErrorMessage"] = response.Message;

            }
            return RedirectToAction("Index1");
        }

        public IActionResult CountContactBasedOnCountry(int countryId)
        {
            var countries = GetCountries();
            ViewBag.CountryId = countryId;
            ViewBag.Countries = countries;
            var states = GetStates();
            ViewBag.States = states;
            var apiurl = "";
            ServiceResponse<int> response = new ServiceResponse<int>();
          
            apiurl = $"{endPoint}Contact/CountContactBasedOnCountry/"+countryId;
           
            response = _httpClientService.ExecuteApiRequest<ServiceResponse<int>>
                (apiurl, HttpMethod.Get, HttpContext.Request);
            var totalCount = response.Data;
            ViewBag.TotalCount = totalCount;
            if (response.Success)
            {
                return View(new List<ReportCount>());
            }
            return View();
        }

        public IActionResult CountContactBasedOnGender(char gender)
        {
            var countries = GetCountries();
            ViewBag.Gender = gender;
            ViewBag.Countries = countries;
            var states = GetStates();
            ViewBag.States = states;
            var apiurl = "";
            if(gender== '\0')
            {
                gender = 'O';
            }
            ServiceResponse<int> response = new ServiceResponse<int>();

            apiurl = $"{endPoint}Contact/CountContactBasedOnGender/" + gender;

            response = _httpClientService.ExecuteApiRequest<ServiceResponse<int>>
                (apiurl, HttpMethod.Get, HttpContext.Request);
            var totalCount = response.Data;
            ViewBag.TotalCount = totalCount;
            if (response.Success)
            {
                return View(new List<ReportCount>());
            }
            return View();
        }
       
        public IActionResult GetDetailByBirthMonth(int month)
        {
            ViewBag.Month = month;
            var countries = GetCountries();
            ViewBag.Countries = countries;
            var states = GetStates();
            ViewBag.States = states;

            ServiceResponse<IEnumerable<ContactPaginatedViewModel>> response = new ServiceResponse<IEnumerable<ContactPaginatedViewModel>>();

                response = _httpClientService.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactPaginatedViewModel>>>
                    ($"{endPoint}Contact/GetDetailByBirthMonth/" + month, HttpMethod.Get, HttpContext.Request);

                if (response.Success)
                {
                    return View(response.Data);
                }
            
            return View(new List<ContactPaginatedViewModel>());
        }
        public IActionResult GetDetailByStateId(int stateId)
        {
            var countries = GetCountries();
            ViewBag.StateId = stateId;
            ViewBag.Countries = countries;
            var states = GetStates();
            ViewBag.States = states;

            ServiceResponse<IEnumerable<ContactPaginatedViewModel>> response = new ServiceResponse<IEnumerable<ContactPaginatedViewModel>>();
            response = _httpClientService.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactPaginatedViewModel>>>
                ($"{endPoint}Contact/GetDetailByStateId/" + stateId, HttpMethod.Get, HttpContext.Request);
            if (response.Success)
            {
                return View(response.Data);
            }

            return View(new List<ContactPaginatedViewModel>());
        }


        private List<ContactsCountryViewModel> GetCountries()
        {
            ServiceResponse<IEnumerable<ContactsCountryViewModel>> response = new ServiceResponse<IEnumerable<ContactsCountryViewModel>>();
            string endPoint = _configuration["EndPoint:CivicaApi"];
            response = _httpClientService.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactsCountryViewModel>>>
                ($"{endPoint}Country/GetAllCountries", HttpMethod.Get, HttpContext.Request);

            if (response.Success)
            {
                return response.Data.ToList();
            }
            return new List<ContactsCountryViewModel>();
        }
        private List<ContactsStateViewModel> GetStates()
        {
            ServiceResponse<IEnumerable<ContactsStateViewModel>> response = new ServiceResponse<IEnumerable<ContactsStateViewModel>>();
            string endPoint = _configuration["EndPoint:CivicaApi"];
            response = _httpClientService.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactsStateViewModel>>>
                ($"{endPoint}State/GetAllStates", HttpMethod.Get, HttpContext.Request);

            if (response.Success)
            {
                return response.Data.ToList();
            }
            return new List<ContactsStateViewModel>();
        }

    }
}



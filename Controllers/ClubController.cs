using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using RunGroupWebApp.Interfaces;
using RunGroupWebApp.Models;
using RunGroupWebApp.ViewModel;

namespace RunGroupWebApp.Controllers
{
    public class ClubController : Controller
    {
        private readonly ICLubRepository _cLubRepository;
        private readonly IPhotoService _photoService;
        private readonly IHttpContextAccessor  _httpContextAccessor;

        public ClubController(ICLubRepository cLubRepository, IPhotoService photoService, IHttpContextAccessor httpContextAccessor)
        {
            _cLubRepository = cLubRepository;
            _photoService = photoService;
            _httpContextAccessor = httpContextAccessor;

        }
        public  async Task<IActionResult> Index()
        {
            IEnumerable<Club> clubs = await _cLubRepository.GetAll();
            return View(clubs);
        }
        public async Task<IActionResult> Detail(int id)
        {
            var club = await _cLubRepository.GetByIdAsync(id);
            return View(club);
        }
        public IActionResult Create()
        {
            var userId = User.GetUserID();
            if (userId== null)
            {
                return View();
            }
            var createClubViewModel = new CreateClubViewModel{AppUserId = userId};
            return View(createClubViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateClubViewModel clubVM)
        {
            if (!ModelState.IsValid)
                return View(clubVM);

            if (clubVM.Image == null || clubVM.Image.Length == 0)
            {
                ModelState.AddModelError("Image", "Please upload an image");
                return View(clubVM);
            }

            var result = await _photoService.AddPhotoAsync(clubVM.Image);

            if (result == null || result.Error != null)
            {
                ModelState.AddModelError("", "Photo upload failed");
                return View(clubVM);
            }

            var club = new Club
            {
                Title = clubVM.Title,
                Description = clubVM.Description,
                AppUserId = clubVM.AppUserId,
                Address = new Address
                {
                    Street = clubVM.Address.Street,
                    City = clubVM.Address.City,
                    State = clubVM.Address.State
                },
                Image = result.SecureUrl.ToString()
            };

            await _cLubRepository.Add(club);
            return RedirectToAction("Index");
        }


        public async Task<IActionResult> Edit(int id)
        {
            var club = await _cLubRepository.GetByIdAsync(id);
            if (club == null)  return View("Error");
            var clubVM = new EditClubViewModel
            {
                Title = club.Title,
                Description = club.Description,
                AddressId = club.AddressId,
                Address = club.Address,
                Url = club.Image,
                ClubCategory = club.ClubCategory
            };
            return View(clubVM);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditClubViewModel clubVM )
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Faild to edit.");
                return View(clubVM);
            }
            var userCLub = await _cLubRepository.GetByIdAsyncNoTracking(id);
            if(userCLub != null)
            {
                try
                {
                    await _photoService.DeletePhotoAsync(userCLub.Image);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    return View(clubVM);
                }
                var photoResult = await _photoService.AddPhotoAsync(clubVM.Image);
                var club = new Club
                {
                    Id = id,
                    Title = clubVM.Title,
                    Description = clubVM.Description,
                    AddressId = clubVM.AddressId,
                    Address = clubVM.Address,
                    Image = photoResult.SecureUrl.ToString()
                };
                _cLubRepository.Update(club);
                return RedirectToAction("Index");
            }
            else
            {
                return View(clubVM);
            }

        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var userClub = await _cLubRepository.GetByIdAsyncNoTracking(id);

            if (userClub == null)
            {
                return NotFound();
            }
            _cLubRepository.Delete(userClub);
            return RedirectToAction("Index");
        }

    }
}

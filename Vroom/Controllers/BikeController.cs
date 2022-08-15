using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Vroom.Data.Common;
using Vroom.Data.Models;
using Vroom.Helpers;
using Vroom.Models;
using System.IO;

using AutoMapper;
using System.Collections.Generic;
using Vroom.Service.Contracts;
using Vroom.Service.Models;
using Vroom.Providers.Contracts;
using cloudscribe.Pagination.Models;
using Bytes2you.Validation;
using Vroom.Common;
using Microsoft.AspNetCore.Hosting;
using Vroom.Models.Factories.NewFolder;

namespace Vroom.Controllers
{
    [Authorize(Roles = Roles.Admin + "," + Roles.Executive)]
    public class BikeController : Controller
    {
        private readonly IWebHostEnvironment hostingEnvironment;
        private readonly IMapper mapper;
        private readonly IBikeService bikeService;
        private readonly IMakeService makeService;
        private readonly IModelService modelService;
        private readonly IHttpContextProvider httpContextProvider;
        private readonly ICacheProvider cacheProvider;
        private readonly IIOProvider iOProvider;
        private readonly IEncryptionProvider encryptionProvider;
        private readonly ISelectedItemFactory selectedItemFactory;

        public BikeController(IWebHostEnvironment hostingEnvironment, IMapper mapper, IBikeService bikeService, IMakeService makeService, IModelService modelService, IHttpContextProvider httpContextProvider, ICacheProvider cacheProvider, IIOProvider iOProvider, IEncryptionProvider encryptionProvider, ISelectedItemFactory selectedItemFactory)
        {
            Guard.WhenArgument<IBikeService>(bikeService, GlobalConstants.BikeServiceNullExceptionMessege).IsNull().Throw();
            Guard.WhenArgument<IMakeService>(makeService, GlobalConstants.MakeServiceNullExceptionMessege).IsNull().Throw();
            Guard.WhenArgument<IModelService>(modelService, GlobalConstants.ModelServiceNullExceptionMessege).IsNull().Throw();
            Guard.WhenArgument<IMapper>(mapper, GlobalConstants.MapperProviderNullExceptionMessege).IsNull().Throw();
            Guard.WhenArgument<IWebHostEnvironment>(hostingEnvironment, GlobalConstants.GetMemberName(() => hostingEnvironment)).IsNull().Throw();
            Guard.WhenArgument<IHttpContextProvider>(httpContextProvider, GlobalConstants.GetMemberName(() => httpContextProvider)).IsNull().Throw();
            Guard.WhenArgument<ICacheProvider>(cacheProvider, GlobalConstants.GetMemberName(() => cacheProvider)).IsNull().Throw();
            Guard.WhenArgument<IIOProvider>(iOProvider, GlobalConstants.GetMemberName(() => iOProvider)).IsNull().Throw();
            Guard.WhenArgument<IEncryptionProvider>(encryptionProvider, GlobalConstants.GetMemberName(() => encryptionProvider)).IsNull().Throw();
            Guard.WhenArgument<ISelectedItemFactory>(selectedItemFactory, GlobalConstants.GetMemberName(() => selectedItemFactory)).IsNull().Throw();

            this.hostingEnvironment = hostingEnvironment;
            this.mapper = mapper;
            this.bikeService = bikeService;
            this.makeService = makeService;
            this.modelService = modelService;
            this.httpContextProvider = httpContextProvider;
            this.cacheProvider = cacheProvider;
            this.iOProvider = iOProvider;
            this.encryptionProvider = encryptionProvider;
            this.selectedItemFactory = selectedItemFactory;
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Index(string searchString, string sortOrder, int pageNumber = 1, int pageSize = 2)
        {
            ViewBag.CurrentSortOrder = sortOrder;
            ViewBag.CurrentSearchString = searchString;
            ViewBag.PriceSortParam = string.IsNullOrEmpty(sortOrder) ? "price_desc" : "";

            PagedBikeServiceModel result;
            if (pageSize == 2 && string.IsNullOrEmpty(searchString))
            {

               result = this.cacheProvider.Get<PagedBikeServiceModel>($"FilteredBikes-pageNumber={pageNumber},sortOrder={sortOrder}",() => this.bikeService.GetFilteredBikes(pageNumber, pageSize, searchString, sortOrder), 60);
            }
            else
            {
                result = this.bikeService.GetFilteredBikes(pageNumber, pageSize, searchString, sortOrder);
            }

            var bikeList = this.mapper.Map<IEnumerable<BikeServiceModel>, IEnumerable<BikeViewModel>>(result.Bikes).ToList();
            var viewResult = new PagedResult<BikeViewModel>()
            {
                Data = bikeList,
                TotalItems = result.TotalSize,
                PageNumber = pageNumber,
                PageSize = pageSize

            };
            if (this.httpContextProvider.IsAjaxRequest())
            {
                ViewData["Action"] = nameof(this.Index);
                ViewData["Controller"] = "Bike";
                ViewData["Replace"] = "#bikeInfo";
                return PartialView("_PagedRequestPartial", viewResult);
            }
            return View(viewResult);
        }


        [HttpGet]
        [AllowAnonymous]
        public IActionResult ViewBike(string id)
        {
            int decId = this.encryptionProvider.Decrypt(id);

            var bikeViewModel = this.mapper.Map<BikeServiceModel, BikeViewModel>(this.bikeService.GetById(decId));

            var makeList = this.mapper.Map<List<MakeServiceModel>, List<MakeViewModel>>(this.makeService.GetAll().ToList());
            var modelList = this.mapper.Map<List<ModelServiceModel>, List<ModelViewModel>>(this.modelService.GetAll(this.encryptionProvider.Decrypt(bikeViewModel.MakeId)).ToList());
            var createModel = new CreateBikeViewModel() { Makes = this.selectedItemFactory.GetSelectList(makeList, bikeViewModel.MakeId), Models = this.selectedItemFactory.GetSelectList(modelList,bikeViewModel.ModelId), Bike = bikeViewModel };

            return View(createModel);
        }

        [HttpGet]

        public IActionResult Create()
        {
            var makeList = this.mapper.Map<List<MakeServiceModel>, List<MakeViewModel>>(this.makeService.GetAll().ToList());
            var modelList = this.mapper.Map<List<ModelServiceModel>, List<ModelViewModel>>(this.modelService.GetAll().ToList());
            var createModel = new CreateBikeViewModel() { Makes = this.selectedItemFactory.GetSelectList(makeList), Models = this.selectedItemFactory.GetSelectList(modelList), Bike = new BikeViewModel() };
            return this.View(createModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CreateBikeViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            var modelDb = this.mapper.Map<BikeViewModel, BikeServiceModel>(model.Bike);

            modelDb.SellerId = this.httpContextProvider.GetCurrentUsedId();
            int newBikeId = this.bikeService.Add(modelDb);


            var wwrootPath = hostingEnvironment.WebRootPath;

            var files = this.httpContextProvider.GetPostedFormFiles();

            if (files.Count != 0)
            {
                string RelativeImagePath = this.iOProvider.SaveImage(newBikeId, wwrootPath, files);
                this.bikeService.UpdateImg(newBikeId, RelativeImagePath);
            }

            return this.RedirectToAction(nameof(this.Index));
        }



        [HttpPost]
        public IActionResult Delete(string id)
        {
            int decId = this.encryptionProvider.Decrypt(id);
            
            var doesExist = this.bikeService.DoesExist(decId);

            if (!doesExist)
            {
                return NotFound();
            }

            this.bikeService.Delete(decId);

            return this.RedirectToAction(nameof(this.Index));
        }

        [HttpGet]
        public IActionResult Edit(string id)
        {
            int decId = this.encryptionProvider.Decrypt(id);
            
            var model = this.bikeService.GetById(decId);

            if (model == null)
            {
                return NotFound();
            }
            var bikeViewModel = this.mapper.Map<BikeServiceModel, BikeViewModel>(model);
            var makeList = this.mapper.Map<List<MakeServiceModel>, List<MakeViewModel>>(this.makeService.GetAll().ToList());
            var modelList = this.mapper.Map<List<ModelServiceModel>, List<ModelViewModel>>(this.modelService.GetAll(model.Model.MakeId).ToList());
            var createModel = new CreateBikeViewModel() { Makes = this.selectedItemFactory.GetSelectList(makeList, bikeViewModel.MakeId), Models = this.selectedItemFactory.GetSelectList(modelList, bikeViewModel.ModelId), Bike = bikeViewModel };

            return this.View(createModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CreateBikeViewModel createViewModel)
        {
            if (!ModelState.IsValid)
            {
                return NotFound();
            }
            var userId = this.httpContextProvider.GetCurrentUsedId();
            var doesModelExist = this.modelService.DoesExist(this.encryptionProvider.Decrypt(createViewModel.Bike.ModelId));
            var doesMakeExist = this.makeService.DoesExist(this.encryptionProvider.Decrypt(createViewModel.Bike.MakeId));
            var bike = this.bikeService.GetById(this.encryptionProvider.Decrypt(createViewModel.Bike.Id));

            if (!doesModelExist || !doesMakeExist || bike == null)
            {
                return NotFound();
            }

            if (bike.Seller.Id.ToString() != userId)
            {
                return BadRequest();
            }

            var bikeServiceModel = this.mapper.Map<BikeViewModel, BikeServiceModel>(createViewModel.Bike);

            var files = this.httpContextProvider.GetPostedFormFiles();
            var wwrootPath = hostingEnvironment.WebRootPath;

            if (files.Count != 0)
            {
                string RelativeImagePath = this.iOProvider.SaveImage(bike.Id, wwrootPath, files);
                bikeServiceModel.ImagePath = RelativeImagePath;
            }

            this.bikeService.Update(bikeServiceModel);

            return this.RedirectToAction(nameof(this.Index));
        }

        private string SaveImage(int modelDbId, string wwrootPath, Microsoft.AspNetCore.Http.IFormFileCollection files)
        {
            var imgPath = @"images\bike\";
            var Extension = Path.GetExtension(files[0].FileName);
            var RelativeImagePath = imgPath + modelDbId + Extension;
            var AbsImagePath = Path.Combine(wwrootPath, RelativeImagePath);

            using (var filestream = new FileStream(AbsImagePath, FileMode.Create))
            {
                files[0].CopyTo(filestream);
            }

            return RelativeImagePath;
        }
    }
}

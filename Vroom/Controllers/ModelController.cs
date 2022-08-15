using AutoMapper;
using Bytes2you.Validation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using Vroom.Common;
using Vroom.Helpers;
using Vroom.Models;
using Vroom.Models.Factories.NewFolder;
using Vroom.Providers.Contracts;
using Vroom.Service.Contracts;
using Vroom.Service.Models;
using Vroom.Service.Models.Contracts;

namespace Vroom.Controllers
{
    [Authorize(Roles = Roles.Admin + "," + Roles.Executive)]
    public class ModelController : Controller
    {
        private readonly IMapper mapper;
        private readonly IModelService modelService;
        private readonly IMakeService makeService;
        private readonly IEncryptionProvider encryptionProvider;
        private readonly ISelectedItemFactory selectedItemFactory;
        public ModelController(IMapper mapper, IModelService modelService, IMakeService makeService, IEncryptionProvider encryptionProvider, ISelectedItemFactory selectedItemFactory)
        {
            Guard.WhenArgument<IMakeService>(makeService, GlobalConstants.MakeServiceNullExceptionMessege).IsNull().Throw();
            Guard.WhenArgument<IModelService>(modelService, GlobalConstants.ModelServiceNullExceptionMessege).IsNull().Throw();
            Guard.WhenArgument<IMapper>(mapper, GlobalConstants.MapperProviderNullExceptionMessege).IsNull().Throw();
            Guard.WhenArgument<IEncryptionProvider>(encryptionProvider, GlobalConstants.GetMemberName(() => encryptionProvider)).IsNull().Throw();
            Guard.WhenArgument<ISelectedItemFactory>(selectedItemFactory, GlobalConstants.GetMemberName(() => selectedItemFactory)).IsNull().Throw();

            this.modelService = modelService;
            this.makeService = makeService;
            this.mapper = mapper;
            this.encryptionProvider = encryptionProvider;
            this.selectedItemFactory = selectedItemFactory;
        }
        [HttpGet]
        public IActionResult Index()
        {

            var modelList = this.mapper.Map<List<ModelServiceModel>,List<ModelViewModel>>(modelService.GetAll().ToList());
            
            return View(modelList);
        }

        [HttpGet]
         
        public IActionResult Create()
        {
            var makeList = this.mapper.Map<IEnumerable<IMakeServiceModel>,IEnumerable<MakeViewModel>>(this.makeService.GetAll());
            var createModel = new CreateViewModel() { MakeViewModels = this.selectedItemFactory.GetSelectList(makeList) };
            return this.View(createModel);
        }

        [HttpPost]
        public IActionResult Create(CreateViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            var modelDb = mapper.Map<CreateViewModel, ModelServiceModel>(model);
            this.modelService.Add(modelDb);

            return this.RedirectToAction(nameof(this.Index));
        }

        [HttpPost]
        public IActionResult Delete(string id)
        {
            var decryptedId = this.encryptionProvider.Decrypt(id);
            var doestExist = this.modelService.DoesExist(decryptedId);

            if(!doestExist)
            {
                return NotFound();
            }

            this.modelService.Delete(decryptedId);

            return this.RedirectToAction(nameof(this.Index));
        }

        [HttpGet]
        public IActionResult Edit(string id)
        {
            var decryptedId = this.encryptionProvider.Decrypt(id);
            var model = this.modelService.GetById(decryptedId);

            if(model == null)
            {
                return NotFound();
            }

            var makeList = this.mapper.Map<IEnumerable<IMakeServiceModel>,IEnumerable<MakeViewModel>>(this.makeService.GetAll());
            var encryptedMakeId = this.encryptionProvider.Encrypt(model.MakeId);
            var createModel = new CreateViewModel() { Name = model.Name, MakeId=encryptedMakeId, Id = this.encryptionProvider.Encrypt(model.Id), MakeViewModels = this.selectedItemFactory.GetSelectList(makeList, encryptedMakeId) };
            return this.View(createModel);
        }

        [HttpPost]

        public IActionResult Edit(CreateViewModel createViewModel)
        {
            if (!ModelState.IsValid)
            {
                return NotFound();
            }

            var modelDecryptedId = this.encryptionProvider.Decrypt(createViewModel.Id);
            var makeDecrptedId = this.encryptionProvider.Decrypt(createViewModel.MakeId);

            var model = this.modelService.DoesExist(modelDecryptedId);
            var make = this.makeService.DoesExist(makeDecrptedId);

            if(!model || !make)
            {
                return NotFound();
            }

            
            this.modelService.Update(modelDecryptedId, createViewModel.Name, makeDecrptedId);

            return this.RedirectToAction(nameof(this.Index));
        }

        [AllowAnonymous]
        [HttpGet("api/models/{makeId}")]
        public IEnumerable<ModelViewModel> Models(string makeId)
        {
            
            return this.mapper.Map<IEnumerable<ModelServiceModel>,IEnumerable<ModelViewModel>>(this.modelService.GetAll(this.encryptionProvider.Decrypt(makeId)));
        }
    }
}

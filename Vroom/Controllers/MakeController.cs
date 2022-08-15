using AutoMapper;
using Bytes2you.Validation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vroom.Common;
using Vroom.Data.Common;
using Vroom.Helpers;
using Vroom.Models;
using Vroom.Providers.Contracts;
using Vroom.Service.Contracts;
using Vroom.Service.Models;
using Vroom.Service.Models.Contracts;

namespace Vroom.Controllers
{
    [Authorize(Roles = Roles.Admin+","+Roles.Executive)]
    public class MakeController : Controller
    {
        private readonly IMapper mapper;
        private readonly IMakeService makeService;
        private readonly IEncryptionProvider encryptionProvider;

        public MakeController(IMapper mapper, IMakeService makeService, IEncryptionProvider encryptionProvider)
        {
            Guard.WhenArgument<IMakeService>(makeService, GlobalConstants.MakeServiceNullExceptionMessege).IsNull().Throw();
            Guard.WhenArgument<IMapper>(mapper, GlobalConstants.MapperProviderNullExceptionMessege).IsNull().Throw();
            Guard.WhenArgument<IEncryptionProvider>(encryptionProvider, GlobalConstants.GetMemberName(() => encryptionProvider)).IsNull().Throw();

            this.mapper = mapper;
            this.makeService = makeService;
            this.encryptionProvider = encryptionProvider;
        }
        public IActionResult Index()
        {
            var makeList = this.mapper.Map<IEnumerable<IMakeServiceModel>, List<MakeViewModel>>(this.makeService.GetAll().ToList());
            return this.View(makeList);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        public IActionResult Create(MakeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.View(model);
            }

            var newMake = this.mapper.Map<MakeViewModel, MakeServiceModel>(model);

            this.makeService.Add(newMake);
            return RedirectToAction(nameof(this.Index));
        }

        [HttpGet]
        public IActionResult Edit(string id)
        {
            var decryptedId = this.encryptionProvider.Decrypt(id);
            var make = this.makeService.GetById(decryptedId);
            if (make == null)
            {
                return NotFound();
            }

            return this.View(this.mapper.Map<IMakeServiceModel,MakeViewModel>(make));
        }

        [HttpPost]
        public IActionResult Edit(MakeViewModel makeModel)
        {

            if (!ModelState.IsValid)
            {
                return this.View(makeModel);
            }

            var decryptedId = this.encryptionProvider.Decrypt(makeModel.Id);
            if (!this.makeService.DoesExist(decryptedId))
            {
                return this.View(makeModel);
            }

            this.makeService.UpdateName(decryptedId, makeModel.Name);

            return this.RedirectToAction(nameof(this.Index));
        }

        [HttpPost]
        public IActionResult Delete(string id)
        {
            var decryptedId = this.encryptionProvider.Decrypt(id);

            if (this.makeService.DoesExist(decryptedId))
            {
                this.makeService.Delete(decryptedId);
            }

            return this.RedirectToAction(nameof(this.Index));
        }
    }
}

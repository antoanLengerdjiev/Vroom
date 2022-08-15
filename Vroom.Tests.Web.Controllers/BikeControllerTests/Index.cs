using AutoMapper;
using cloudscribe.Pagination.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vroom.Controllers;
using Vroom.Models;
using Vroom.Models.Factories.NewFolder;
using Vroom.Providers.Contracts;
using Vroom.Service.Contracts;
using Vroom.Service.Models;

namespace Vroom.Tests.Web.Controllers.BikeControllerTests
{
    [TestFixture]
    public class Index
    {
        private string searchStringParam;
        private int pageSizeTwoParam;
        private int pageSizeThree;
        private int pageNumberParam;
        private string sortOrderParam;
        private string sortOrderParamDesc;
        private List<BikeServiceModel> bikeServiceModelList;
        private List<BikeViewModel> bikeViewModeList;
        private PagedBikeServiceModel pagedBikeServiceModel;
        private Mock<IWebHostEnvironment> mockHostingEnvironment;
        private Mock<IMapper> mockMapper;
        private Mock<IBikeService> mockBikeService;
        private Mock<IMakeService> mockMakeService;
        private Mock<IModelService> mockModelService;
        private Mock<IHttpContextProvider> mockHttpContextProvider;
        private Mock<ICacheProvider> mockCacheProvider;
        private Mock<IIOProvider> mockIOprovider;
        private Mock<IEncryptionProvider> mockEncryptionProvider;
        private Mock<ISelectedItemFactory> mockSelectItemFactory;
        private BikeController controller;

        [SetUp]
        public void Setup()
        {
            this.searchStringParam = "searchString";
            this.pageSizeTwoParam = 2;
            this.pageSizeThree = 3;
            this.pageNumberParam = 1;
            this.sortOrderParam = "";
            this.sortOrderParamDesc = "price_desc";

            this.bikeServiceModelList = new List<BikeServiceModel> { new BikeServiceModel(), new BikeServiceModel() };
            this.bikeViewModeList = new List<BikeViewModel> { new BikeViewModel(), new BikeViewModel() };

            this.pagedBikeServiceModel = new PagedBikeServiceModel() { Bikes = this.bikeServiceModelList, TotalSize = 2};


            this.mockHostingEnvironment = new Mock<IWebHostEnvironment>();
            this.mockMapper = new Mock<IMapper>();
            this.mockBikeService = new Mock<IBikeService>();
            this.mockMakeService = new Mock<IMakeService>();
            this.mockModelService = new Mock<IModelService>();
            this.mockHttpContextProvider = new Mock<IHttpContextProvider>();
            this.mockCacheProvider = new Mock<ICacheProvider>();
            this.mockIOprovider = new Mock<IIOProvider>();
            this.mockEncryptionProvider = new Mock<IEncryptionProvider>();
            this.mockSelectItemFactory = new Mock<ISelectedItemFactory>();

            this.mockHttpContextProvider.Setup(x => x.IsAjaxRequest()).Returns(false);
            this.mockCacheProvider.Setup(x => x.Get<PagedBikeServiceModel>($"FilteredBikes-pageNumber={pageNumberParam},sortOrder={sortOrderParamDesc}", It.IsAny<Func<PagedBikeServiceModel>>(), 60)).Returns(this.pagedBikeServiceModel);
            this.mockCacheProvider.Setup(x => x.Get<PagedBikeServiceModel>($"FilteredBikes-pageNumber={pageNumberParam},sortOrder={sortOrderParam}", It.IsAny<Func<PagedBikeServiceModel>>(), 60)).Returns(this.pagedBikeServiceModel);
            this.mockBikeService.Setup(x => x.GetFilteredBikes(pageNumberParam, pageSizeTwoParam, searchStringParam, sortOrderParam)).Returns(this.pagedBikeServiceModel);
            this.mockBikeService.Setup(x => x.GetFilteredBikes(pageNumberParam, pageSizeTwoParam, searchStringParam, null)).Returns(this.pagedBikeServiceModel);
            this.mockBikeService.Setup(x => x.GetFilteredBikes(pageNumberParam, pageSizeThree, searchStringParam, sortOrderParam)).Returns(this.pagedBikeServiceModel);
            this.mockBikeService.Setup(x => x.GetFilteredBikes(pageNumberParam, pageSizeThree, searchStringParam, null)).Returns(this.pagedBikeServiceModel);
            this.mockBikeService.Setup(x => x.GetFilteredBikes(pageNumberParam, pageSizeTwoParam, null, sortOrderParam)).Returns(this.pagedBikeServiceModel);
            this.mockBikeService.Setup(x => x.GetFilteredBikes(pageNumberParam, pageSizeTwoParam, "", null)).Returns(this.pagedBikeServiceModel);
            this.mockBikeService.Setup(x => x.GetFilteredBikes(pageNumberParam, pageSizeTwoParam, null, null)).Returns(this.pagedBikeServiceModel);

            this.mockBikeService.Setup(x => x.GetFilteredBikes(pageNumberParam, pageSizeThree, null, sortOrderParam)).Returns(this.pagedBikeServiceModel);
            this.mockBikeService.Setup(x => x.GetFilteredBikes(pageNumberParam, pageSizeThree, "", sortOrderParam)).Returns(this.pagedBikeServiceModel);
            this.mockBikeService.Setup(x => x.GetFilteredBikes(pageNumberParam, pageSizeThree, null, null)).Returns(this.pagedBikeServiceModel);
            this.mockBikeService.Setup(x => x.GetFilteredBikes(pageNumberParam, pageSizeThree, "", null)).Returns(this.pagedBikeServiceModel);

            this.mockBikeService.Setup(x => x.GetFilteredBikes(pageNumberParam, pageSizeTwoParam, searchStringParam, sortOrderParamDesc)).Returns(this.pagedBikeServiceModel);
           
            this.mockBikeService.Setup(x => x.GetFilteredBikes(pageNumberParam, pageSizeThree, searchStringParam, sortOrderParamDesc)).Returns(this.pagedBikeServiceModel);
     
            this.mockBikeService.Setup(x => x.GetFilteredBikes(pageNumberParam, pageSizeTwoParam, null, sortOrderParamDesc)).Returns(this.pagedBikeServiceModel);

            this.mockBikeService.Setup(x => x.GetFilteredBikes(pageNumberParam, pageSizeThree, null, sortOrderParamDesc)).Returns(this.pagedBikeServiceModel);
            this.mockBikeService.Setup(x => x.GetFilteredBikes(pageNumberParam, pageSizeThree, "", sortOrderParamDesc)).Returns(this.pagedBikeServiceModel);


            this.mockMapper.Setup(x => x.Map<IEnumerable<BikeServiceModel>, IEnumerable<BikeViewModel>>(this.pagedBikeServiceModel.Bikes)).Returns(this.bikeViewModeList); ;

            this.controller = new BikeController(this.mockHostingEnvironment.Object, this.mockMapper.Object,
                this.mockBikeService.Object, this.mockMakeService.Object,
                this.mockModelService.Object, this.mockHttpContextProvider.Object,
                this.mockCacheProvider.Object, this.mockIOprovider.Object,
                this.mockEncryptionProvider.Object, this.mockSelectItemFactory.Object);
        }

        [TestCase("")]
        [TestCase(null)]
        public void ShouldCallGetMethodFromCacheProvider_WhenPageSizeIs2AndSortOrderParamIsNullOrEmptyString(string searchString)
        {
            // Arrange

            // Act
            this.controller.Index(searchString, this.sortOrderParam, this.pageNumberParam, this.pageSizeTwoParam);
            // Assert

            this.mockCacheProvider.Verify(x => x.Get<PagedBikeServiceModel>($"FilteredBikes-pageNumber={pageNumberParam},sortOrder={sortOrderParam}", It.IsAny<Func<PagedBikeServiceModel>>(), 60),Times.Once);
        }

        [TestCase("")]
        [TestCase(null)]
        public void ShouldNotCallGetFilteredBikesMethodFromBikeService_WhenPageSizeIs2AndSortOrderParamIsNullOrEmptyString(string searchString)
        {
            // Arrange

            // Act
            this.controller.Index(searchString, this.sortOrderParam, this.pageNumberParam, this.pageSizeTwoParam);
            // Assert

            this.mockBikeService.Verify(x => x.GetFilteredBikes(pageNumberParam, this.pageSizeTwoParam, searchString, this.sortOrderParam), Times.Never);
        }

        [TestCase(3,"")]
        [TestCase(3,null)]
        [TestCase(2, "searchString")]

        public void ShouldCallGetFilteredBikesMethodFromBikeService_WhenPageSizeIsNot2OrSortOrderParamNotIsNullOrEmptyString(int pageSize, string searchString)
        {
            // Arrange

            // Act
            this.controller.Index(searchString, this.sortOrderParam, this.pageNumberParam, pageSize);
            // Assert

            this.mockBikeService.Verify(x => x.GetFilteredBikes(pageNumberParam, pageSize, searchString, this.sortOrderParam), Times.Once);
        }

        [TestCase(3, "")]
        [TestCase(3, null)]
        [TestCase(2, "searchString")]

        public void ShouldNotCallGetMethodFromCacheProvider_WhenPageSizeIsNot2OrSortOrderParamNotIsNullOrEmptyString(int pageSize, string searchString)
        {
            // Arrange

            // Act
            this.controller.Index(searchString, this.sortOrderParam, this.pageNumberParam, pageSize);
            // Assert

            this.mockCacheProvider.Verify(x => x.Get<PagedBikeServiceModel>($"FilteredBikes-pageNumber={pageNumberParam},sortOrder={sortOrderParam}", It.IsAny<Func<PagedBikeServiceModel>>(), 60), Times.Never);
        }

        [TestCase(3, "")]
        [TestCase(3, null)]
        [TestCase(2, "searchString")]

        public void ShouldCallMapMethodFromMapper_WhenPageSizeIsNot2OrSortOrderParamNotIsNullOrEmptyString(int pageSize, string searchString)
        {
            // Arrange

            // Act
            this.controller.Index(searchString, this.sortOrderParam, this.pageNumberParam, pageSize);
            // Assert

            this.mockMapper.Verify(x => x.Map<IEnumerable<BikeServiceModel>, IEnumerable<BikeViewModel>>(this.pagedBikeServiceModel.Bikes), Times.Once);
        }

        [TestCase("")]
        [TestCase(null)]
        public void ShouldCallMapMethodFromMapper_WhenPageSizeIs2AndSortOrderParamIsNullOrEmptyString(string searchString)
        {
            // Arrange

            // Act
            this.controller.Index(searchString, this.sortOrderParam, this.pageNumberParam, this.pageSizeTwoParam);
            // Assert

            this.mockMapper.Verify(x => x.Map<IEnumerable<BikeServiceModel>, IEnumerable<BikeViewModel>>(this.pagedBikeServiceModel.Bikes), Times.Once);
        }

        [TestCase(3, "")]
        [TestCase(3, null)]
        [TestCase(2, "searchString")]

        public void ShouldCallIsAjaxRequestMethodFromHttpContextProvider_WhenPageSizeIsNot2OrSortOrderParamNotIsNullOrEmptyString(int pageSize, string searchString)
        {
            // Arrange

            // Act
            this.controller.Index(searchString, this.sortOrderParam, this.pageNumberParam, pageSize);
            // Assert

            this.mockHttpContextProvider.Verify(x => x.IsAjaxRequest(), Times.Once);
        }

        [TestCase("")]
        [TestCase(null)]
        public void ShouldCallIsAjaxRequestMethodFromHttpContextProvider_WhenPageSizeIs2AndSortOrderParamIsNullOrEmptyString(string searchString)
        {
            // Arrange

            // Act
            this.controller.Index(searchString, this.sortOrderParam, this.pageNumberParam, this.pageSizeTwoParam);
            // Assert

            this.mockHttpContextProvider.Verify(x => x.IsAjaxRequest(), Times.Once);
        }


        [TestCase(3, "")]
        [TestCase(3, null)]
        [TestCase(2, "searchString")]

        public void ShouldViewModelShouldBePagedResultOfBikeViewModel_WhenPageSizeIsNot2OrSortOrderParamNotIsNullOrEmptyString(int pageSize, string searchString)
        {
            // Arrange

            // Act
            this.controller.Index(searchString, this.sortOrderParam, this.pageNumberParam, pageSize);
            // Assert

            Assert.IsInstanceOf<PagedResult<BikeViewModel>>(this.controller.View().Model);
        }

        [TestCase("")]
        [TestCase(null)]
        public void ShouldViewModelShouldBePagedResultOfBikeViewModel_WhenPageSizeIs2AndSortOrderParamIsNullOrEmptyString(string searchString)
        {
            // Arrange

            // Act
            this.controller.Index(searchString, this.sortOrderParam, this.pageNumberParam, this.pageSizeTwoParam);
            // Assert

            Assert.IsInstanceOf<PagedResult<BikeViewModel>>(this.controller.View().Model);
        }

        [TestCase(3, "")]
        [TestCase(3, null)]
        [TestCase(2, "searchString")]

        public void ShouldSetDataPropertyDataOfModelCorrectly_WhenPageSizeIsNot2OrSortOrderParamNotIsNullOrEmptyString(int pageSize, string searchString)
        {
            // Arrange

            // Act
            var result = this.controller.Index(searchString, this.sortOrderParam, this.pageNumberParam, pageSize);
            // Assert
            var model = this.controller.View().Model as PagedResult<BikeViewModel>;

            Assert.AreEqual(this.bikeViewModeList, model.Data);
        }

        [TestCase("")]
        [TestCase(null)]
        public void ShouldSetDataPropertyDataOfModelCorrectly_WhenPageSizeIs2AndSortOrderParamIsNullOrEmptyString(string searchString)
        {
            // Arrange

            // Act
            this.controller.Index(searchString, this.sortOrderParam, this.pageNumberParam, this.pageSizeTwoParam);
            // Assert
            var model = this.controller.View().Model as PagedResult<BikeViewModel>;

            Assert.AreEqual(this.bikeViewModeList, model.Data);
        }


        [TestCase(3, "")]
        [TestCase(3, null)]
        [TestCase(2, "searchString")]

        public void ShouldSetPropertyTotalSizeOfModelCorrectly_WhenPageSizeIsNot2OrSortOrderParamNotIsNullOrEmptyString(int pageSize, string searchString)
        {
            // Arrange

            // Act
            var result = this.controller.Index(searchString, this.sortOrderParam, this.pageNumberParam, pageSize);
            // Assert
            var model = this.controller.View().Model as PagedResult<BikeViewModel>;

            Assert.AreEqual(this.pagedBikeServiceModel.TotalSize, model.TotalItems);
        }

        [TestCase("")]
        [TestCase(null)]
        public void ShouldSetPropertyTotalSizeOfModelCorrectly_WhenPageSizeIs2AndSortOrderParamIsNullOrEmptyString(string searchString)
        {
            // Arrange

            // Act
            this.controller.Index(searchString, this.sortOrderParam, this.pageNumberParam, this.pageSizeTwoParam);
            // Assert
            var model = this.controller.View().Model as PagedResult<BikeViewModel>;

            Assert.AreEqual(this.pagedBikeServiceModel.TotalSize, model.TotalItems);
        }


        [TestCase(3, "")]
        [TestCase(3, null)]
        [TestCase(2, "searchString")]

        public void ShouldSetDataPropertyPageNumberOfModelCorrectly_WhenPageSizeIsNot2OrSortOrderParamNotIsNullOrEmptyString(int pageSize, string searchString)
        {
            // Arrange

            // Act
            var result = this.controller.Index(searchString, this.sortOrderParam, this.pageNumberParam, pageSize);
            // Assert
            var model = this.controller.View().Model as PagedResult<BikeViewModel>;

            Assert.AreEqual(this.pageNumberParam, model.PageNumber);
        }

        [TestCase("")]
        [TestCase(null)]
        public void ShouldSetPropertyPageNumberOfModelCorrectly_WhenPageSizeIs2AndSortOrderParamIsNullOrEmptyString(string searchString)
        {
            // Arrange

            // Act
            this.controller.Index(searchString, this.sortOrderParam, this.pageNumberParam, this.pageSizeTwoParam);
            // Assert
            var model = this.controller.View().Model as PagedResult<BikeViewModel>;

            Assert.AreEqual(this.pageNumberParam, model.PageNumber);
        }

        [TestCase(3, "")]
        [TestCase(3, null)]
        [TestCase(2, "searchString")]

        public void ShouldSetPropertyPageSizeOfModelCorrectly_WhenPageSizeIsNot2OrSortOrderParamNotIsNullOrEmptyString(int pageSize, string searchString)
        {
            // Arrange

            // Act
            var result = this.controller.Index(searchString, this.sortOrderParam, this.pageNumberParam, pageSize);
            // Assert
            var model = this.controller.View().Model as PagedResult<BikeViewModel>;

            Assert.AreEqual(pageSize, model.PageSize);
        }

        [TestCase("")]
        [TestCase(null)]
        public void ShouldSetPropertyPageSizeOfModelCorrectly_WhenPageSizeIs2AndSortOrderParamIsNullOrEmptyString(string searchString)
        {
            // Arrange

            // Act
            this.controller.Index(searchString, this.sortOrderParam, this.pageNumberParam, this.pageSizeTwoParam);
            // Assert
            var model = this.controller.View().Model as PagedResult<BikeViewModel>;

            Assert.AreEqual(pageSizeTwoParam, model.PageSize);
        }


        [TestCase(3, "")]
        [TestCase(3, null)]
        [TestCase(2, "searchString")]

        public void ShouldSetViewbagSortOrderCorrectly_WhenPageSizeIsNot2OrSortOrderParamNotIsNullOrEmptyString(int pageSize, string searchString)
        {
            // Arrange

            // Act
            var result = this.controller.Index(searchString, this.sortOrderParam, this.pageNumberParam, pageSize);
            // Assert
            Assert.AreEqual(this.sortOrderParam,this.controller.ViewBag.CurrentSortOrder);
        }

        [TestCase("")]
        [TestCase(null)]
        public void ShouldSetViewbagSortOrderCorrectly_WhenPageSizeIs2AndSortOrderParamIsNullOrEmptyString(string searchString)
        {
            // Arrange

            // Act
            this.controller.Index(searchString, this.sortOrderParam, this.pageNumberParam, this.pageSizeTwoParam);
            // Assert

            Assert.AreEqual(this.sortOrderParam, this.controller.ViewBag.CurrentSortOrder);
        }


        [TestCase(3, "")]
        [TestCase(3, null)]
        [TestCase(2, "searchString")]

        public void ShouldSetViewbagCurrentSearchStringCorrectly_WhenPageSizeIsNot2OrSortOrderParamNotIsNullOrEmptyString(int pageSize, string searchString)
        {
            // Arrange

            // Act
            var result = this.controller.Index(searchString, this.sortOrderParam, this.pageNumberParam, pageSize);
            // Assert
            Assert.AreEqual(searchString, this.controller.ViewBag.CurrentSearchString);
        }

        [TestCase("")]
        [TestCase(null)]
        public void ShouldSetViewbagCurrentSearchStringCorrectly_WhenPageSizeIs2AndSortOrderParamIsNullOrEmptyString(string searchString)
        {
            // Arrange

            // Act
            this.controller.Index(searchString, this.sortOrderParam, this.pageNumberParam, this.pageSizeTwoParam);
            // Assert

            Assert.AreEqual(searchString, this.controller.ViewBag.CurrentSearchString);
        }

        [TestCase(3, "")]
        [TestCase(3, null)]
        [TestCase(2, "searchString")]

        public void ShouldSetViewbagPriceSortParamToPrice_Desc_WhenPageSizeIsNot2OrSearchStringParamNotIsNullOrEmptyStringAndSortOrderParamIsEmptyStringOrNull(int pageSize, string searchString)
        {
            // Arrange

            // Act
            var result = this.controller.Index(searchString, this.sortOrderParam, this.pageNumberParam, pageSize);
            // Assert
            Assert.AreEqual(this.sortOrderParamDesc, this.controller.ViewBag.PriceSortParam);
        }

        [TestCase("", "")]
        [TestCase("",null)]
        [TestCase(null, "")]
        [TestCase(null,null)]
        public void ShouldSetViewbagPriceSortParamToPrice_Desc_WhenPageSizeIs2AndSortOrderParamIsNullOrEmptyStringAndSortOrderParamIsEmptyStringOrNull(string searchString, string sortOrder)
        {
            // Arrange

            // Act
            this.controller.Index(searchString, sortOrder, this.pageNumberParam, this.pageSizeTwoParam);
            // Assert

            Assert.AreEqual(this.sortOrderParamDesc, this.controller.ViewBag.PriceSortParam);
        }

        [TestCase(3, "")]
        [TestCase(3, null)]
        [TestCase(2, "searchString")]

        public void ShouldSetViewbagPriceSortParamToEmptyString_WhenPageSizeIsNot2OrSearchStringParamNotIsNullOrEmptyStringAndSortOrderParamIsPriceDesc(int pageSize, string searchString)
        {
            // Arrange

            // Act
            var result = this.controller.Index(searchString, this.sortOrderParamDesc, this.pageNumberParam, pageSize);
            // Assert
            Assert.AreEqual(this.sortOrderParam, this.controller.ViewBag.PriceSortParam);
        }

        [TestCase("")]
        [TestCase(null)]
        public void ShouldSetViewbagPriceSortParamToEmptyString_WhenPageSizeIs2AndSortOrderParamIsNullOrEmptyStringAndSortOrderParamIsPriceDesc(string searchString)
        {
            // Arrange

            // Act
            this.controller.Index(searchString, this.sortOrderParamDesc, this.pageNumberParam, this.pageSizeTwoParam);
            // Assert

            Assert.AreEqual(this.sortOrderParam, this.controller.ViewBag.PriceSortParam);
        }

        [TestCase(3, "")]
        [TestCase(3, null)]
        [TestCase(2, "searchString")]

        public void ShouldReturnViewResult_WhenPageSizeIsNot2OrSearchStringParamNotIsNullOrEmptyStringAndSortOrderParamIsPriceDesc(int pageSize, string searchString)
        {
            // Arrange

            // Act
            var result = this.controller.Index(searchString, this.sortOrderParamDesc, this.pageNumberParam, pageSize);
            // Assert
            Assert.IsAssignableFrom<ViewResult>(result);
        }

        [TestCase("")]
        [TestCase(null)]
        public void ShouldReturnViewResult_WhenPageSizeIsNot2OrSearchStringParamNotIsNullOrEmptyStringAndSortOrderParamIsPriceDesc_WhenPageSizeIs2AndSortOrderParamIsNullOrEmptyStringAndSortOrderParamIsPriceDesc(string searchString)
        {
            // Arrange

            // Act
            var result = this.controller.Index(searchString, this.sortOrderParamDesc, this.pageNumberParam, this.pageSizeTwoParam) as ViewResult;
            // Assert

            Assert.IsAssignableFrom<ViewResult>(result);
        }

        [TestCase(3, "")]
        [TestCase(3, null)]
        [TestCase(2, "searchString")]

        public void ShouldReturnCorrectPartialViewWithName_PagedRequestPartial_WhenPageSizeIsNot2OrSearchStringParamNotIsNullOrEmptyStringAndSortOrderParamIsEmptyStringOrNullAndItsAjaxRequest(int pageSize, string searchString)
        {
            // Arrange
            this.mockHttpContextProvider.Setup(x => x.IsAjaxRequest()).Returns(true);
            // Act
           var result = this.controller.Index(searchString, this.sortOrderParamDesc, this.pageNumberParam, pageSize) as PartialViewResult;
            // Assert
            Assert.IsAssignableFrom<PartialViewResult>(result);
            Assert.AreEqual("_PagedRequestPartial", result.ViewName);
        }

        [TestCase("")]
        [TestCase(null)]
        public void ShouldReturnCorrectPartialViewWithName_PagedRequestPartial_WhenPageSizeIs2AndSortOrderParamIsNullOrEmptyStringAndSortOrderParamIsEmptyStringOrNullAndItsAjaxRequest(string searchString)
        {
            // Arrange
            this.mockHttpContextProvider.Setup(x => x.IsAjaxRequest()).Returns(true);
            // Act
            var result = this.controller.Index(searchString, this.sortOrderParamDesc, this.pageNumberParam, this.pageSizeTwoParam) as PartialViewResult;
            // Assert
            Assert.IsAssignableFrom<PartialViewResult>(result);
            Assert.AreEqual("_PagedRequestPartial", result.ViewName);
        }


        [TestCase(3, "")]
        [TestCase(3, null)]
        [TestCase(2, "searchString")]

        public void ShouldSetViewDataActionToIndex_WhenPageSizeIsNot2OrSearchStringParamNotIsNullOrEmptyStringAndSortOrderParamIsEmptyStringOrNullAndItsAjaxRequest(int pageSize, string searchString)
        {
            // Arrange
            this.mockHttpContextProvider.Setup(x => x.IsAjaxRequest()).Returns(true);
            // Act
            var result = this.controller.Index(searchString, this.sortOrderParamDesc, this.pageNumberParam, pageSize) as PartialViewResult;
            // Assert
            Assert.AreEqual("Index", result.ViewData["Action"]);
        }

        [TestCase("")]
        [TestCase(null)]
        public void ShouldSetViewDataActionToIndex_WhenPageSizeIs2AndSortOrderParamIsNullOrEmptyStringAndSortOrderParamIsEmptyStringOrNullAndItsAjaxRequest(string searchString)
        {
            // Arrange
            this.mockHttpContextProvider.Setup(x => x.IsAjaxRequest()).Returns(true);
            // Act
            var result = this.controller.Index(searchString, this.sortOrderParamDesc, this.pageNumberParam, this.pageSizeTwoParam) as PartialViewResult;
            // Assert
            Assert.AreEqual("Index",result.ViewData["Action"]);
        }

        [TestCase(3, "")]
        [TestCase(3, null)]
        [TestCase(2, "searchString")]

        public void ShouldSetViewDataControllerToBike_WhenPageSizeIsNot2OrSearchStringParamNotIsNullOrEmptyStringAndSortOrderParamIsEmptyStringOrNullAndItsAjaxRequest(int pageSize, string searchString)
        {
            // Arrange
            this.mockHttpContextProvider.Setup(x => x.IsAjaxRequest()).Returns(true);
            // Act
            var result = this.controller.Index(searchString, this.sortOrderParamDesc, this.pageNumberParam, pageSize) as PartialViewResult;
            // Assert
            Assert.AreEqual("Bike", result.ViewData["Controller"]);
        }

        [TestCase("")]
        [TestCase(null)]
        public void ShouldSetViewDataControllerToBike_WhenPageSizeIs2AndSortOrderParamIsNullOrEmptyStringAndSortOrderParamIsEmptyStringOrNullAndItsAjaxRequest(string searchString)
        {
            // Arrange
            this.mockHttpContextProvider.Setup(x => x.IsAjaxRequest()).Returns(true);
            // Act
            var result = this.controller.Index(searchString, this.sortOrderParamDesc, this.pageNumberParam, this.pageSizeTwoParam) as PartialViewResult;
            // Assert
            Assert.AreEqual("Bike", result.ViewData["Controller"]);
        }

        [TestCase(3, "")]
        [TestCase(3, null)]
        [TestCase(2, "searchString")]

        public void ShouldSetViewDataReplaceToBikeInfo_WhenPageSizeIsNot2OrSearchStringParamNotIsNullOrEmptyStringAndSortOrderParamIsEmptyStringOrNullAndItsAjaxRequest(int pageSize, string searchString)
        {
            // Arrange
            this.mockHttpContextProvider.Setup(x => x.IsAjaxRequest()).Returns(true);
            // Act
            var result = this.controller.Index(searchString, this.sortOrderParamDesc, this.pageNumberParam, pageSize) as PartialViewResult;
            // Assert
            Assert.AreEqual("#bikeInfo", result.ViewData["Replace"]);
        }

        [TestCase("")]
        [TestCase(null)]
        public void ShouldSetViewDataReplaceToBikeInfo_WhenPageSizeIs2AndSortOrderParamIsNullOrEmptyStringAndSortOrderParamIsEmptyStringOrNullAndItsAjaxRequest(string searchString)
        {
            // Arrange
            this.mockHttpContextProvider.Setup(x => x.IsAjaxRequest()).Returns(true);
            // Act
            var result = this.controller.Index(searchString, this.sortOrderParamDesc, this.pageNumberParam, this.pageSizeTwoParam) as PartialViewResult;
            // Assert
            Assert.AreEqual("#bikeInfo", result.ViewData["Replace"]);
        }


        [TestCase(3, "")]
        [TestCase(3, null)]
        [TestCase(2, "searchString")]

        public void ShouldNotSetViewDataActionToIndex_WhenPageSizeIsNot2OrSearchStringParamNotIsNullOrEmptyStringAndSortOrderParamIsEmptyStringOrNullAndItsNotAjaxRequest(int pageSize, string searchString)
        {
            // Arrange

            // Act
            var result = this.controller.Index(searchString, this.sortOrderParamDesc, this.pageNumberParam, pageSize) as ViewResult;
            // Assert
            Assert.IsNull(result.ViewData["Action"]);
        }

        [TestCase("")]
        [TestCase(null)]
        public void ShouldNotSetViewDataActionToIndex_WhenPageSizeIs2AndSortOrderParamIsNullOrEmptyStringAndSortOrderParamIsEmptyStringOrNullAndItsNotAjaxRequest(string searchString)
        {
            // Arrange

            // Act
            var result = this.controller.Index(searchString, this.sortOrderParamDesc, this.pageNumberParam, this.pageSizeTwoParam) as ViewResult;
            // Assert
            Assert.IsNull(result.ViewData["Action"]);
        }

        [TestCase(3, "")]
        [TestCase(3, null)]
        [TestCase(2, "searchString")]

        public void ShouldNotSetViewDataControllerToBike_WhenPageSizeIsNot2OrSearchStringParamNotIsNullOrEmptyStringAndSortOrderParamIsEmptyStringOrNullAndItsNotAjaxRequest(int pageSize, string searchString)
        {
            // Arrange

            // Act
            var result = this.controller.Index(searchString, this.sortOrderParamDesc, this.pageNumberParam, pageSize) as ViewResult;
            // Assert
            Assert.IsNull(result.ViewData["Controller"]);
        }

        [TestCase("")]
        [TestCase(null)]
        public void ShouldNotSetViewDataControllerToBike_WhenPageSizeIs2AndSortOrderParamIsNullOrEmptyStringAndSortOrderParamIsEmptyStringOrNullAndItsNotAjaxRequest(string searchString)
        {
            // Arrange

            // Act
            var result = this.controller.Index(searchString, this.sortOrderParamDesc, this.pageNumberParam, this.pageSizeTwoParam) as ViewResult;
            // Assert
            Assert.IsNull(result.ViewData["Controller"]);
        }

        [TestCase(3, "")]
        [TestCase(3, null)]
        [TestCase(2, "searchString")]

        public void ShouldNotSetViewDataReplaceToBikeInfo_WhenPageSizeIsNot2OrSearchStringParamNotIsNullOrEmptyStringAndSortOrderParamIsEmptyStringOrNullAndItsNotAjaxRequest(int pageSize, string searchString)
        {
            // Arrange

            // Act
            var result = this.controller.Index(searchString, this.sortOrderParamDesc, this.pageNumberParam, pageSize) as ViewResult;
            // Assert
            Assert.IsNull(result.ViewData["Replace"]);
        }

        [TestCase("")]
        [TestCase(null)]
        public void ShouldNotSetViewDataReplaceToBikeInfo_WhenPageSizeIs2AndSortOrderParamIsNullOrEmptyStringAndSortOrderParamIsEmptyStringOrNullAndItsNotAjaxRequest(string searchString)
        {
            // Arrange
            
            // Act
            var result = this.controller.Index(searchString, this.sortOrderParamDesc, this.pageNumberParam, this.pageSizeTwoParam) as ViewResult;
            // Assert
            Assert.IsNull(result.ViewData["Replace"]);
        }
    }
    
}

﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ReleaseNotes.Models;
using ReleaseNotes.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ReleaseNotes.ViewModels;
using ReleaseNotes.Models;

namespace ReleaseNotes.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private HttpClient _productsClient;

        public HomeController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;

            _productsClient = _httpClientFactory.CreateClient("ReleaseNotesApiClient");
        }

        public async Task<IActionResult> Index()
        {
            //Skal vi bare hardkode produktid'en i "url"'en?
            var productResult = await _productsClient.GetAsync("/Home/Index");
            var productList = new List<ProductViewModel>();

            if (productResult.IsSuccessStatusCode)
            {
                var responseStream = await productResult.Content.ReadAsStringAsync();
                var products = JsonConvert.DeserializeObject<ProductList>(responseStream);

                foreach (var product in products.Products)
                {
                    var productVm = new ProductViewModel()
                    {
                        ProductId = product.ProductId,
                        ProductName = product.ProductName,
                        ProductImage = product.ProductImage,
                        ProductDescription = product.ProductDescription
                    };
                    productList.Add(productVm);

                    // For debug
                    Console.WriteLine(product);
                }
            }
            else
            {

            }

            return View(productList);
        }

        public IActionResult TalentRecruiter()
        {
            var Connection = new DBContext();
            List<ReleaseNoteViewModel> InReleaseNotes = Connection.MockDataList();

            var TargetProductId = 1;

            List<ReleaseNoteViewModel> releaseNotesListNew = new List<ReleaseNoteViewModel>();

            for (var i = 0; i < InReleaseNotes.Count; i++)
            {
                if (InReleaseNotes[i].ProductId == TargetProductId)
                {
                    releaseNotesListNew.Add(new ReleaseNoteViewModel
                    {
                        Title = InReleaseNotes[i].Title,
                        Bodytext = InReleaseNotes[i].Bodytext,
                        Id = InReleaseNotes[i].Id,
                        ProductId = InReleaseNotes[i].ProductId,
                        CreatedBy = InReleaseNotes[i].CreatedBy,
                        CreatedDate = InReleaseNotes[i].CreatedDate,
                        LastUpdatedBy = InReleaseNotes[i].LastUpdatedBy,
                        LastUpdatedDate = InReleaseNotes[i].LastUpdatedDate
                    });
                }
                ViewData.Model = releaseNotesListNew;
            }
            /*
            if (releaseNotesListNew.Count < 1)
            {
                var TargetIdNotFound = "There is currently no release notes published for this product.";

                releaseNotesListNew.Add(new ReleaseNoteViewModel
                {
                    Title = "",
                    Bodytext = TargetIdNotFound,
                    Id = null,
                    ProductId = null,
                    CreatedBy = null,
                    CreatedDate = null,
                    LastUpdatedBy = null,
                    LastUpdatedDate = null
                });
            } */
            return View();
        }

        public IActionResult TalentOnboarding()
        {
            var viewModel = new HomeControllerViewModel { ReleaseNotes = new List<ReleaseNoteViewModel>() };

            var Connection = new DBContext();
            List<ReleaseNoteViewModel> InReleaseNotes = Connection.MockDataList();

            //var TargetId = 2;

            var talentOnboardingReleaseNotes = InReleaseNotes.Where(x => x.ProductId == 2).ToList();
            /*
           for (var i = 0; i < InReleaseNotes.Count; i++)
           {
               if (InReleaseNotes[i].productId == TargetId)
               {
                   List<releaseNotes> releaseNotesListNew = new List<releaseNotes>
                   {
                       new releaseNotes {
                           title = InReleaseNotes[i].title,
                           bodytext = InReleaseNotes[i].bodytext,
                           id = InReleaseNotes[i].id,
                           productId = InReleaseNotes[i].productId,
                           createdBy = InReleaseNotes[i].createdBy,
                           createdDate = InReleaseNotes[i].createdDate,
                           lastUpdatedBy = InReleaseNotes[i].lastUpdatedBy,
                           lastUpdatedDate = InReleaseNotes[i].lastUpdatedDate,
                       }
                   };
                   ViewData.Model = releaseNotesListNew;
               }
           } */
            ViewData.Model = talentOnboardingReleaseNotes;

            return View(viewModel);
        }

        public IActionResult TalentManager()
        {
            var Connection = new DBContext();
            List<ReleaseNoteViewModel> InReleaseNotes = Connection.MockDataList();

            var TargetProductId = 3;

            List<ReleaseNoteViewModel> releaseNotesListNew = new List<ReleaseNoteViewModel>();

            for (var i = 0; i < InReleaseNotes.Count; i++)
            {
                if (InReleaseNotes[i].ProductId == TargetProductId)
                {
                    releaseNotesListNew.Add(new ReleaseNoteViewModel
                    {
                        Title = InReleaseNotes[i].Title,
                        Bodytext = InReleaseNotes[i].Bodytext,
                        Id = InReleaseNotes[i].Id,
                        ProductId = InReleaseNotes[i].ProductId,
                        CreatedBy = InReleaseNotes[i].CreatedBy,
                        CreatedDate = InReleaseNotes[i].CreatedDate,
                        LastUpdatedBy = InReleaseNotes[i].LastUpdatedBy,
                        LastUpdatedDate = InReleaseNotes[i].LastUpdatedDate
                    });
                }
                ViewData.Model = releaseNotesListNew;
            }
            return View();
        }


        public IActionResult LatestRelease()
        {
            return View();
        }

        // Har denne metoden i HomeController midlertidig, evt. flyttes den over til ReleaseNoteController om funksjonaliteten trengs
        public IActionResult ListLatestReleaseNote()
        {
            var BodytextData = "Lorem ipsum dolor sit amet, in nonummy lectus venenatis posuere risus ipsum, nulla vel lorem vitae bibendum sed, elit lacinia urna convallis eget placerat, duis wisi mauris nullam mauris, nulla vitae eu nunc nisl est.Odio justo dui ut nulla proin turpis, facere varius dolor eu ipsum congue orci, dolor lorem facilisis mauris euismod, viverra ipsum eros conubia tellus habitant. Mauris fusce egestas sodales rutrum, tellus odio tortor donec justo nec, aptent dictum dui elit mi dui, diam aliquam suscipit placerat, justo turpis integer sed.Leo ac eros ullamcorper eum sapien quam, ut quis felis, magna senectus fringilla eu ultricies vel, ac arcu sodales at urna sit mattis, nulla imperdiet quisque pede sit rutrum.Suscipit suspendisse. In hendrerit ipsum pellentesque aptent sollicitudin sapien, donec magna in cras in pulvinar quisque, eros adipiscing dui cursus hendrerit. Diam quam. Nunc elit elit semper in, nulla nam eros nonummy vestibulum suscipit, sed vitae. Vulputate ac sagittis amet nulla, ipsum aenean ante quis id duis, nisl nulla risus.";

            List<ReleaseNoteViewModel> releaseNotesList = new List<ReleaseNoteViewModel>
            {
                new ReleaseNoteViewModel {
                    Title = "Release note 0.1 - Onboarding",
                    Bodytext = BodytextData,
                    Id = 1,
                    ProductId = 1,
                    CreatedBy = "Fredrik Svevad Riise",
                    CreatedDate = DateTime.ParseExact("27/01/2020", "dd/MM/yyyy", null),
                    LastUpdatedBy = "",
                    LastUpdatedDate = null,
                },

                new ReleaseNoteViewModel {
                    Title = "Release note 0.93 - Manager",
                    Bodytext = BodytextData,
                    Id = 2,
                    ProductId = 2,
                    CreatedBy = "Felix Thu Falkendal Nilsen",
                    CreatedDate = DateTime.ParseExact("28/01/2020", "dd/MM/yyyy", null),
                    LastUpdatedBy = "Felix Thu Falkendal Nilsen",
                    LastUpdatedDate = DateTime.ParseExact("31/01/2020", "dd/MM/yyyy", null),
                }
            };

            DateTime? val1 = DateTime.MinValue;

            for (var i = 0; i < releaseNotesList.Count; i++)
            {
                if (releaseNotesList[i].CreatedDate > val1 || releaseNotesList[i].CreatedDate == val1)
                {
                    val1 = releaseNotesList[i].CreatedDate;

                    List<ReleaseNoteViewModel> releaseNotesListNew = new List<ReleaseNoteViewModel>
                    {
                        new ReleaseNoteViewModel {
                            Title = releaseNotesList[i].Title,
                            Bodytext = releaseNotesList[i].Bodytext,
                            Id = releaseNotesList[i].Id,
                            ProductId = releaseNotesList[i].ProductId,
                            CreatedBy = releaseNotesList[i].CreatedBy,
                            CreatedDate = releaseNotesList[i].CreatedDate,
                            LastUpdatedBy = releaseNotesList[i].LastUpdatedBy,
                            LastUpdatedDate = releaseNotesList[i].LastUpdatedDate,
                        }
                    };
                    ViewData.Model = releaseNotesListNew;
                }
            }
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
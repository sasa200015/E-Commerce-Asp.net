using E_Commerce.DTO;
using E_Commerce.Interfaces;
using E_Commerce.Models;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Authorization;
using Google.Cloud.Storage.V1;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Google.Cloud.Firestore;
using System.IO;
using FirebaseAdmin.Messaging;
using E_Commerce.Repository;

namespace E_Commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductController : ControllerBase
    {
        private readonly IProduct productrepo;
        private readonly GeneralRes res;
        private readonly ICategory categoryrepo;
        private readonly StorageClient _storageClient;

        public ProductController(IProduct productrepo, GeneralRes res, ICategory categoryrepo)
        {
            this.productrepo = productrepo;
            this.res = res;
            this.categoryrepo = categoryrepo;
            if (FirebaseApp.DefaultInstance == null)
            {
                var credentialPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "root", "second-hand-c1094-firebase-adminsdk-4g4d9-a0b33c4e96.json");
                FirebaseApp.Create(new AppOptions()
                {
                    Credential = GoogleCredential.FromFile(credentialPath),
                    ProjectId = "second-hand-c1094"
                });
            }

            _storageClient = StorageClient.Create();
        }
        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            List<Product> products = productrepo.GetAll();
            List<productDto> productsDto = new List<productDto>();
            foreach (var item in products)
            {
                productDto productDto = new productDto();
                productDto.Id = item.Id;
                productDto.Title = item.Title;
                productDto.CategoryId = item.CategoryId;
                productDto.Category = item.Category;
                productDto.Description = item.Description;
                productDto.Price = item.Price;
                productDto.ImageUrl=item.ImageUrl;
                productsDto.Add(productDto);
            }
            return StatusCode(StatusCodes.Status200OK, productsDto);
        }
        [HttpGet("{Id:int}")]
        public IActionResult GetById(int Id)
        {
            Product product = productrepo.GetById(Id);
            if (product != null)
            {
                productDto productDto = new productDto();
                productDto.Id = product.Id;
                productDto.Title = product.Title;
                productDto.CategoryId = product.CategoryId;
                productDto.Category = product.Category;
                productDto.Description = product.Description;
                productDto.Price = product.Price;
                productDto.ImageUrl=product.ImageUrl;
                return StatusCode(StatusCodes.Status200OK, productDto);
            }
            return StatusCode(StatusCodes.Status404NotFound, "This product does not exist");
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Add([FromForm] AddProduct Modelproduct)
        {
            if (ModelState.IsValid)
            {
                var categoryres = categoryrepo.GetById(Modelproduct.CategoryId);
                if (categoryres != null)
                {
                    if (Modelproduct.Category.ToLower() != categoryres.Name.ToLower())
                    {
                        return StatusCode(StatusCodes.Status400BadRequest, "The choosen category it's name does not match with the category field");
                    }
                    Product product = new Product();
                    string fileName = $"{Path.GetFileName(Modelproduct.Image.FileName)}";

                    var bucketName = "second-hand-c1094.appspot.com";
                    using (var fileStream = Modelproduct.Image.OpenReadStream())
                    {
                        // Upload the file to Firestore
                        var objectName = $"products/{fileName}";
                        var uploadTask = await _storageClient.UploadObjectAsync(
                            bucketName,
                            objectName,
                            Modelproduct.Image.ContentType,
                            fileStream
                        );

                        // Generate the public URL for the uploaded file
                        string fileUrl = $"https://storage.googleapis.com/{bucketName}/{objectName}";
                        product.Description = Modelproduct.Description;
                        product.ImageUrl = fileUrl;
                        product.Title = Modelproduct.Title;
                        product.Category = Modelproduct.Category;
                        product.CategoryId = Modelproduct.CategoryId;
                        product.Price = Modelproduct.Price;
                        productrepo.Add(product);
                        await productrepo.Saveasync();
                        var result = new
                        {
                            Title = Modelproduct.Title,
                            Price = Modelproduct.Price,
                            Description = Modelproduct.Description,
                            CategoryId = Modelproduct.CategoryId,
                            CategoryName = categoryres.Name,
                            ImageUrl = fileUrl

                        };
                        res.Message = "success";
                        res.Data = result;
                        return StatusCode(StatusCodes.Status200OK, res);
                    }
                }
                return StatusCode(StatusCodes.Status404NotFound, "This category does not exist");
            }
            var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();
            res.Message = "fail";
            res.Data = errors;
            return StatusCode(StatusCodes.Status400BadRequest, res);
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update([FromForm] UpdateProductDto productModel)
        {
            var name = "";
            var product = productrepo.GetById(productModel.Id);

            if (product != null)
            {
                if (productModel.CategoryId != null)
                {
                    var categoryres = categoryrepo.GetById((int)productModel.CategoryId);
                    if (categoryres == null)
                    {
                        return StatusCode(StatusCodes.Status404NotFound, "This category does not exist");
                    }
                    name = categoryres.Name;
                    if (productModel.Category != null && productModel.Category.ToLower() != categoryres.Name.ToLower())
                    {
                        return StatusCode(StatusCodes.Status400BadRequest, "The choosen category it's name does not match with the category field");
                    }
                }

                product.Id = productModel.Id;
                product.Title = productModel.Title ?? product.Title;
                product.Description = productModel.Description ?? product.Description;
                product.Price = productModel.Price ?? product.Price;
                product.Category = productModel.Category ?? product.Category;
                product.CategoryId = productModel.CategoryId ?? product.CategoryId;
                if (productModel.Image != null && productModel.Image.Length > 0)
                {
                    string bucketName = "second-hand-c1094.appspot.com";
                    string fileName = $"{Path.GetFileName(productModel.Image.FileName)}";
                    using (var fileStream = productModel.Image.OpenReadStream())
                    {
                        var objectName = $"products/{fileName}";
                        var uploadTask = await _storageClient.UploadObjectAsync(
                            bucketName,
                            objectName,
                            productModel.Image.ContentType,
                            fileStream
                        );
                        string fileUrl = $"https://storage.googleapis.com/{bucketName}/{objectName}";
                        product.ImageUrl = fileUrl;

                    }
                }
                        productrepo.Update(product);
                        await productrepo.Saveasync();
                        var result = new
                        {
                            Title = product.Title,
                            Price = product.Price,
                            Description = product.Description,
                            CategoryId = product.CategoryId,
                            ImageUrl = product.ImageUrl

                        };
                        res.Message = "Successs";
                        res.Data = result;
                        return StatusCode(StatusCodes.Status200OK, result);
            }
                return StatusCode(StatusCodes.Status404NotFound, "This product does not exist");
        }

            [HttpDelete("{id:int}")]
            [Authorize(Roles = "Admin")]
            public IActionResult Delete(int id)
            {
                Product product = productrepo.GetById(id);
                if (product != null)
                {
                    productrepo.Delete(id);
                    productrepo.Save();
                    return StatusCode(StatusCodes.Status200OK, "Deleted Successfully");
                }
                return StatusCode(StatusCodes.Status404NotFound, "This product does not exist");
            }
        }
    }

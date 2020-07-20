using API.DTO;
using AutoMapper;
using Core.Entity;
using Microsoft.Extensions.Configuration;

namespace API.Helpers
{
    public class ProductUrlResolver : IValueResolver<Product, ProducttoReturnDto, string>
    {
        private readonly IConfiguration _configuration;
        public  ProductUrlResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }
           
        public string Resolve(Product source, ProducttoReturnDto destination, string destMember, ResolutionContext context)
        {
           if(!string.IsNullOrEmpty(source.PictureUrl))
            {
                return _configuration["ApiUrl"] + source.PictureUrl;
            }
            return null;
        }

       
    }
}
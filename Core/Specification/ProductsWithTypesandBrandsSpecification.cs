using Core.Entity;

namespace Core.Specification
{
    public class ProductsWithTypesandBrandsSpecification:BaseSpecification<Product>
    {
        public ProductsWithTypesandBrandsSpecification(ProductSpecParams productSpecParams)
        :base( x=>(string.IsNullOrEmpty(productSpecParams.Search) || x.Name.ToLower().Contains(productSpecParams.Search))&&
            (!productSpecParams.BrandId.HasValue||x.ProductBrandId==productSpecParams.BrandId)&&     
            (!productSpecParams.TypeId.HasValue ||  x.ProductTypeId==productSpecParams.TypeId)
        )
        {
            AddInclude(X => X.ProductType);
            AddInclude(X => X.ProductBrand);
            AddOrderBy(x=>x.Name);
            ApplyPaging(productSpecParams.PageSize *(productSpecParams.PageIndex-1),productSpecParams.PageSize);
            if(!string.IsNullOrEmpty(productSpecParams.Sort))
            {
                switch(productSpecParams.Sort)
                {
                    case "priceAsc":
                    AddOrderBy(p=>p.Price);
                    break;
                    case "priceDesc":
                    AddOrderByDescending(p=>p.Price);
                    break;
                    default:
                    AddOrderBy(n=>n.Name);
                    break;
                }
            }
        }
        public ProductsWithTypesandBrandsSpecification(int id):base(x=>x.Id==id)
        {
            AddInclude(X => X.ProductType);
            AddInclude(X => X.ProductBrand);
        }
    }
}
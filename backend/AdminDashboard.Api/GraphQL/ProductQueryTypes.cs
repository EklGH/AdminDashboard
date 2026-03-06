using AdminDashboard.Application.Dtos;

namespace AdminDashboard.Api.GraphQL
{
    // Type Product
    public class ProductType : ObjectType<ProductResponseDto>
    {
        protected override void Configure(IObjectTypeDescriptor<ProductResponseDto> descriptor)
        {
            descriptor.Field(f => f.Id).Type<NonNullType<UuidType>>();
            descriptor.Field(f => f.Name).Type<NonNullType<StringType>>();
            descriptor.Field(f => f.Category).Type<NonNullType<StringType>>();
            descriptor.Field(f => f.Price).Type<NonNullType<DecimalType>>();
            descriptor.Field(f => f.Stock).Type<NonNullType<IntType>>();
        }
    }



    // Type pagination de Product
    public class PaginatedProductType : ObjectType<PaginatedProductResponseDto>
    {
        protected override void Configure(IObjectTypeDescriptor<PaginatedProductResponseDto> descriptor)
        {
            descriptor.Field(f => f.Items).Type<NonNullType<ListType<NonNullType<ProductType>>>>();
            descriptor.Field(f => f.Page).Type<NonNullType<IntType>>();
            descriptor.Field(f => f.PageSize).Type<NonNullType<IntType>>();
            descriptor.Field(f => f.TotalItems).Type<NonNullType<IntType>>();
        }
    }
}

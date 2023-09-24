using AutoMapper;
using DataModels;
using DTOModels;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RealEstate.Mediator.Query.PropertyCommand;

namespace RealEstate.Mediator.QueryHandlers.PropertyQuery
{
    internal class GetPropertiesQueryHandler : IRequestHandler<GetPropertiesQuery, IEnumerable<PropertyDto>>
    {
        private readonly RealEstateDbContext _context;
        private readonly IMapper _mapper;

        public GetPropertiesQueryHandler(RealEstateDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PropertyDto>> Handle(GetPropertiesQuery request, CancellationToken cancellationToken)
        {
            string sql = @"
               SELECT *
                FROM (
                    SELECT *,
                           ROW_NUMBER() OVER (ORDER BY IdProperty) AS RowNum
                    FROM Properties
                    WHERE (@Price IS NULL OR Price = @Price)
                      AND (@Year IS NULL OR Year = @Year)
                      AND (@Name IS NULL OR LOWER(Name) LIKE @Name)
                      AND (@Address IS NULL OR LOWER(Address) LIKE @Address)
                      AND (@CodeInternal IS NULL OR LOWER(CodeInternal) = @CodeInternal)
                      AND (@IdOwner IS NULL OR IdOwner = @IdOwner)
                ) AS SubQuery
                WHERE RowNum BETWEEN @StartRow AND @EndRow;
            ";

            Microsoft.Data.SqlClient.SqlParameter[] parameters = new[]
            {
                new Microsoft.Data.SqlClient.SqlParameter("@Price", request.Price > 0 ? request.Price : DBNull.Value),
                new Microsoft.Data.SqlClient.SqlParameter("@Year", request.Year > 0 ? request.Year : DBNull.Value),
                new Microsoft.Data.SqlClient.SqlParameter("@Name", string.IsNullOrEmpty(request.Name) ? DBNull.Value : $"%{request.Name.ToLower()}%"),
                new Microsoft.Data.SqlClient.SqlParameter("@Address", string.IsNullOrEmpty(request.Address) ? DBNull.Value : $"%{request.Address.ToLower()}%"),
                new Microsoft.Data.SqlClient.SqlParameter("@CodeInternal", string.IsNullOrEmpty(request.CodeInternal) ? DBNull.Value : request.CodeInternal.ToLower()),
                new Microsoft.Data.SqlClient.SqlParameter("@IdOwner", request.IdOwner != Guid.Empty ? request.IdOwner.ToString() : DBNull.Value),
                new Microsoft.Data.SqlClient.SqlParameter("@StartRow", ((request.Page - 1) * request.PageSize) + 1), // Calcular el valor de StartRow
                new Microsoft.Data.SqlClient.SqlParameter("@EndRow", request.Page * request.PageSize) // Calcular el valor de EndRow
            };


            List<Property> result = await _context.Properties
                .FromSqlRaw(sql, parameters)
                .ToListAsync();

            return result
                .Select(property => _mapper.Map<PropertyDto>(property))
                .ToList();

        }
    }
}
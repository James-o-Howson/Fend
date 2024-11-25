using Fend.Application.Pagination.Enums;
using Fend.Core.SharedKernel.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Fend.Application.Pagination;

public static class PaginationQueryableExtensions
{
    public static Task<PaginatedList<TDestination>> ToPaginatedListAsync<TDestination>(
        this IQueryable<TDestination> queryable, PaginatedRequest<TDestination> request,
        CancellationToken cancellationToken) 
        where TDestination : class
    {
        var query = queryable
            .AddOrdering(request.SortField, request.SortOrder)
            .AsNoTracking();

        return PaginatedList<TDestination>.CreateAsync(query, request.PageNumber, request.PageSize, cancellationToken);
    }

    private static IQueryable<TDestination> AddOrdering<TDestination>(this IQueryable<TDestination> query, string sortField, SortOrder sortOrder)
    {
        return sortOrder switch
        {
            SortOrder.Ascending => query.OrderByColumn<TDestination>(sortField),
            SortOrder.Descending => query.OrderByColumnDescending<TDestination>(sortField),
            _ => throw new ArgumentOutOfRangeException(nameof(sortOrder), $"Invalid {nameof(sortOrder)} value {sortOrder}")
        };
    }
}

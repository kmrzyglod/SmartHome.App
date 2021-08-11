using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SmartHome.Application.Extensions;
using SmartHome.Application.Interfaces.DbContext;
using SmartHome.Application.Shared.Models;
using SmartHome.Application.Shared.Queries.Rules.GetRulesList;

namespace SmartHome.Application.Queries.Rules.GetRulesList
{
    public class GetRulesListQueryHandler : IRequestHandler<GetRulesListQuery, PaginationResult<RulesListEntryVm>>
    {
        private readonly IApplicationDbContext _dbContext;

        public GetRulesListQueryHandler(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<PaginationResult<RulesListEntryVm>> Handle(GetRulesListQuery request,
            CancellationToken cancellationToken)
        {
            var query = _dbContext.Rules.AsNoTracking();
            int count = await query.CountAsync(cancellationToken);

            var result = await query
                .AddPagination(request.PageNumber, request.PageSize)
                .Select(x => new RulesListEntryVm
                {
                    Name = x.Name,
                    Id = x.Id,
                    IsActive = x.IsActive
                })
                .ToListAsync(cancellationToken);

            return new PaginationResult<RulesListEntryVm>
            {
                ResultTotalCount = count,
                PageNumber = request.PageNumber,
                PageSize = result.Count,
                Result = result
            };
        }
    }
}
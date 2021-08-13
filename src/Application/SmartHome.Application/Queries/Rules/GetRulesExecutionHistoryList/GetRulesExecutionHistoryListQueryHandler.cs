using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SmartHome.Application.Extensions;
using SmartHome.Application.Interfaces.DbContext;
using SmartHome.Application.Shared.Models;
using SmartHome.Application.Shared.Queries.Rules.GetRulesExecutionHistoryList;

namespace SmartHome.Application.Queries.Rules.GetRulesExecutionHistoryList
{
    public class GetRulesExecutionHistoryListQueryHandler : IRequestHandler<GetRulesExecutionHistoryListQuery,
        PaginationResult<RulesExecutionHistoryListVm>>
    {
        private readonly IApplicationDbContext _dbContext;

        public GetRulesExecutionHistoryListQueryHandler(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<PaginationResult<RulesExecutionHistoryListVm>> Handle(
            GetRulesExecutionHistoryListQuery request, CancellationToken cancellationToken)
        {
            var query = _dbContext.RulesExecutionHistory
                .AsNoTracking()
                .OrderByDescending(x => x.Timestamp);

            int count = await query.CountAsync(cancellationToken);

            var result = await query
                .AddPagination(request.PageNumber, request.PageSize)
                .Select(x => new RulesExecutionHistoryListVm
                {
                    Id = x.Id,
                    RuleId = x.RuleId,
                    RuleName = x.Rule.Name,
                    Timestamp = x.Timestamp,
                    ErrorMessage = x.ErrorMessage,
                    ExecutionStatus = x.ExecutionStatus
                })
                .ToListAsync(cancellationToken);

            return new PaginationResult<RulesExecutionHistoryListVm>
            {
                ResultTotalCount = count,
                PageNumber = request.PageNumber,
                PageSize = result.Count,
                Result = result
            };
        }
    }
}
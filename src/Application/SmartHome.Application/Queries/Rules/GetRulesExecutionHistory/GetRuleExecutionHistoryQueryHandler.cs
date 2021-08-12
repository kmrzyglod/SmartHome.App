using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SmartHome.Application.Extensions;
using SmartHome.Application.Interfaces.DbContext;
using SmartHome.Application.Shared.Models;
using SmartHome.Application.Shared.Queries.Rules.GetRuleExecutionHistory;

namespace SmartHome.Application.Queries.Rules.GetRulesExecutionHistory
{
    public class GetRuleExecutionHistoryQueryHandler : IRequestHandler<GetRuleExecutionHistoryQuery,
        PaginationResult<RuleExecutionHistoryVm>>
    {
        private readonly IApplicationDbContext _dbContext;

        public GetRuleExecutionHistoryQueryHandler(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<PaginationResult<RuleExecutionHistoryVm>> Handle(GetRuleExecutionHistoryQuery request,
            CancellationToken cancellationToken)
        {
            var query = _dbContext.RulesExecutionHistory
                .AsNoTracking()
                .Where(x => x.Rule.Id == request.RuleId);

            int count = await query.CountAsync(cancellationToken);

            var result = await query
                .AddPagination(request.PageNumber, request.PageSize)
                .Select(x => new RuleExecutionHistoryVm
                {
                    Id = x.Id,
                    Timestamp = x.Timestamp,
                    ExecutionStatus = x.ExecutionStatus
                })
                .ToListAsync(cancellationToken);

            return new PaginationResult<RuleExecutionHistoryVm>
            {
                ResultTotalCount = count,
                PageNumber = request.PageNumber,
                PageSize = result.Count,
                Result = result
            };
        }
    }
}
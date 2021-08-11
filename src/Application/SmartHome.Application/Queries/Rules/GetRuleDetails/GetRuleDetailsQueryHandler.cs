using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SmartHome.Application.Interfaces.DbContext;
using SmartHome.Application.Shared.Queries.Rules.GetRuleDetails;

namespace SmartHome.Application.Queries.Rules.GetRuleDetails
{
    public class GetRuleDetailsQueryHandler : IRequestHandler<GetRuleDetailsQuery,
        RuleDetailsVm?>
    {
        private readonly IApplicationDbContext _dbContext;

        public GetRuleDetailsQueryHandler(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<RuleDetailsVm?> Handle(GetRuleDetailsQuery request, CancellationToken cancellationToken)
        {
            var ruleDetails = await _dbContext.Rules.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
           
            if (ruleDetails == null)
            {
                return null;
            }

            return new RuleDetailsVm
            {
                Id = ruleDetails.Id,
                Name = ruleDetails.Name,
                IsActive = ruleDetails.IsActive,
                Body = ruleDetails.Body,
                OutputAction = ruleDetails.OutputAction
            };
        }
    }
}
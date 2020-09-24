using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace SmartHome.Application.Shared.Queries.GreenhouseController.GetWindowsStatus
{
    public class GetWindowsStatusQuery: IRequest<WindowsStatusVm>
    {
    }
}
